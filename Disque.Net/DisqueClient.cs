﻿using CSRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Net
{
    public class DisqueClient : ISyncDisqueClient, IDisposable
    {
        private const string DISQUE_PROTOCOL = "disque://";
        private const string DISQUE_HOST = "127.0.0.1";
        private const int DISQUE_PORT = 7711;
        private readonly List<Uri> _uris = new List<Uri>();
        private readonly Random _random = new Random();
        private IRedisClient _c;
        private readonly IJobInfoBuilder _jobInfoBuilder;
        private readonly IQstatBuilder _queueStatBuilder;
        private readonly int _reconnectAttempts;

        public DisqueClient(int reconnectAttempts = 0) : this(reconnectAttempts, new Uri(string.Format("{0}{1}:{2}", DISQUE_PROTOCOL, DISQUE_HOST, DISQUE_PORT)))
        {
        }

        public DisqueClient(string host, int port, int reconnectAttempts = 0) : this(reconnectAttempts, new Uri(string.Format("{0}{1}:{2}", DISQUE_PROTOCOL, host, port)))
        {
        }

        public DisqueClient(params Uri[] uris) : this(uris.ToList())
        {
        }

        public DisqueClient(int reconnectAttempts = 0, params Uri[] uris) : this(uris.ToList(), reconnectAttempts)
        {
        }

        public DisqueClient(List<Uri> uris, int reconnectAttempts = 0)
        {
            _uris.AddRange(uris);
            _jobInfoBuilder = new JobInfoBuilder();
            _queueStatBuilder = new QstatBuilder();
            _reconnectAttempts = reconnectAttempts;
            Connect();
        }

        private void Connect()
        {
            while (_c == null)
            {
                if (_uris.Count == 0)
                {
                    throw new DisqueConnectionException("Could not connect to any of the provided nodes");
                }
                int index = _random.Next(_uris.Count);

                try
                {
                    Uri uri = _uris[index];
                    _c = new RedisClient(uri.Host, uri.Port)
                    {
                        ReconnectAttempts = _reconnectAttempts
                    };
                }
                catch (Exception e)
                {
                    _uris.RemoveAt(index);
                }
            }
        }

        public string Info()
        {
            return (string)_c.Call(Commands.INFO.ToString());
        }

        public string Info(string section)
        {
            return (string)_c.Call(Commands.INFO.ToString(), section);
        }

        public string AddJob(string queueName, string job, int mstimeout)
        {
            //ADDJOB queue_name job <ms-timeout> [REPLICATE <count>] [DELAY <sec>] [RETRY <sec>] [TTL <sec>] [MAXLEN <count>] [ASYNC]
            return (string)_c.Call(Commands.ADDJOB.ToString(), queueName, job, mstimeout.ToString());
        }

        public string AddJob(string queueName, string job, long mstimeout, JobParams jobParams)
        {
            //ADDJOB queue_name job <ms-timeout> [REPLICATE <count>] [DELAY <sec>] [RETRY <sec>] [TTL <sec>] [MAXLEN <count>] [ASYNC]
            string result;
            if (jobParams.Async)
            {
                result = (string)_c.Call(
                    Commands.ADDJOB.ToString(), queueName, job, mstimeout.ToString(),
                    Keywords.REPLICATE.ToString(), jobParams.Replicate.ToString(),
                    Keywords.DELAY.ToString(), jobParams.Delay.ToString(),
                    Keywords.RETRY.ToString(), jobParams.Retry.ToString(),
                    Keywords.TTL.ToString(), jobParams.Ttl.ToString(),
                    Keywords.MAXLEN.ToString(), jobParams.Maxlen.ToString(),
                    Keywords.ASYNC.ToString());
            }
            else
            {
                result = (string)_c.Call(
                  Commands.ADDJOB.ToString(), queueName, job, mstimeout.ToString(),
                  Keywords.REPLICATE.ToString(), jobParams.Replicate.ToString(),
                  Keywords.DELAY.ToString(), jobParams.Delay.ToString(),
                  Keywords.RETRY.ToString(), jobParams.Retry.ToString(),
                  Keywords.TTL.ToString(), jobParams.Ttl.ToString(),
                  Keywords.MAXLEN.ToString(), jobParams.Maxlen.ToString());
            }

            return result;
        }

        public List<Job> GetJob(List<string> queueNames)
        {
            //GETJOB [TIMEOUT <ms-timeout>] [COUNT <count>] FROM queue1 queue2 ... queueN
            var result = new List<Job>();
            var args = new List<string>
        {
            Keywords.FROM.ToString()
        };
            args.AddRange(queueNames);

            object call = _c.Call(Commands.GETJOB.ToString(), args.ToArray());

            ParseGetJobResponse(call, result);

            return result;
        }

        public List<Job> GetJob(params string[] queues)
        {
            //GETJOB [TIMEOUT <ms-timeout>] [COUNT <count>] FROM queue1 queue2 ... queueN
            return GetJob(queueNames: queues.ToList());
        }

        public List<Job> GetJob(long timeout, long count, List<string> queueNames)
        {
            //GETJOB [TIMEOUT <ms-timeout>] [COUNT <count>] FROM queue1 queue2 ... queueN
            var result = new List<Job>();

            var args = new List<string>
        {
            Keywords.TIMEOUT.ToString(),
            timeout.ToString(),
            Keywords.COUNT.ToString(),
            count.ToString(),
            Keywords.FROM.ToString()
        };

            args.AddRange(queueNames);

            object call = _c.Call(Commands.GETJOB.ToString(), args.ToArray());

            ParseGetJobResponse(call, result);

            return result;
        }

        public List<Job> GetJob(long timeout, long count, params string[] queues)
        {
            //GETJOB [TIMEOUT <ms-timeout>] [COUNT <count>] FROM queue1 queue2 ... queueN
            return GetJob(timeout, count, queueNames: queues.ToList());
        }

        public long Ackjob(List<string> jobIdList)
        {
            return (long)_c.Call(Commands.ACKJOB.ToString(), jobIdList.ToArray());
        }

        public long Ackjob(params string[] jobIds)
        {
            return Ackjob(jobIdList: jobIds.ToList());
        }

        public long Qlen(string queueName)
        {
            return (long)_c.Call(Commands.QLEN.ToString(), queueName);
        }

        public List<Job> Qpeek(string queueName, long count)
        {
            var result = new List<Job>();

            object call = _c.Call(Commands.QPEEK.ToString(), queueName, count.ToString());

            object[] objects = call as object[];

            ParseGetJobResponse(objects, result);

            return result;
        }

        public long DelJob(string jobId)
        {
            return (long)_c.Call(Commands.DELJOB.ToString(), jobId);
        }

        public long Dequeue(List<string> jobIdList)
        {
            return (long)_c.Call(Commands.DEQUEUE.ToString(), jobIdList.ToArray());
        }

        public long Dequeue(params string[] jobIds)
        {
            return Dequeue(jobIdList: jobIds.ToList());
        }

        public long Enqueue(List<string> jobIdList)
        {
            return (long)_c.Call(Commands.ENQUEUE.ToString(), jobIdList.ToArray());
        }

        public long Enqueue(params string[] jobIds)
        {
            return Enqueue(jobIdList: jobIds.ToList());
        }

        public long Fastack(List<string> jobIdList)
        {
            return (long)_c.Call(Commands.FASTACK.ToString(), jobIdList.ToArray());
        }

        public long Fastack(params string[] jobIds)
        {
            return Fastack(jobIdList: jobIds.ToList());
        }

        public JobInfo Show(string jobId)
        {
            object call = _c.Call(Commands.SHOW.ToString(), jobId);

            object[] o = call as object[];

            if (o != null)
            {
                return _jobInfoBuilder.BuildFrom(o);
            }

            return null;
        }

        public Qstat Qstat(string queueName)
        {
            var call = _c.Call(Commands.QSTAT.ToString(), queueName);
            var o = call as object[];
            if (o != null)
            {
                return _queueStatBuilder.BuildFrom(o);
            }

            return null;
        }

        public string Ping()
        {
            return (string)_c.Call(Commands.PING.ToString());
        }

        public long Working(string jobId)
        {
            return (long)_c.Call(Commands.WORKING.ToString(), jobId);
        }

        public void Close()
        {
            _c.Dispose();
        }

        public void Dispose()
        {
            _c.Dispose();
        }

        private static void ParseGetJobResponse(object response, List<Job> jobs)
        {
            var objects = response as object[];

            if (objects != null)
            {
                jobs.AddRange(ParseJobs(objects));
            }
        }

        private static IEnumerable<Job> ParseJobs(object[] response)
        {
            return response
                .OfType<object[]>()
                .Select(child => new Job(Conv(child[0]), Conv(child[1]), Conv(child[2])));
        }

        private static string Conv(object b)
        {
            var str = b as string;
            return str ?? Encoding.UTF8.GetString((byte[])b);
        }
    }
}