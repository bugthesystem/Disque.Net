using System;
using System.Collections.Generic;
using System.Linq;
using CSRedis;

namespace Disque.Net
{
    public class DisqueClient : ISyncDisqueClient, IDisposable
    {
        private const string DISQUE_PROTOCOL = "disque://";
        private const string DISQUE_HOST = "127.0.0.1";
        private const int DISQUE_PORT = 7711;
        private const int DEFAULT_TIMEOUT_IN_SECS = 30;
        private readonly List<Uri> _uris = new List<Uri>();
        private readonly Random _randomGenerator = new Random();
        private IRedisClient _c;

        public DisqueClient() : this(new List<Uri> { new Uri(string.Format("{0}{1}:{2}", DISQUE_PROTOCOL, DISQUE_HOST, DISQUE_PORT)) })
        {

        }

        public DisqueClient(string host, int port) : this(new List<Uri> { new Uri(string.Format("{0}{1}:{2}", DISQUE_PROTOCOL, host, port)) })
        {

        }

        public DisqueClient(List<Uri> uris)
        {
            _uris.AddRange(uris);
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
                int index = _randomGenerator.Next(_uris.Count);

                try
                {
                    Uri uri = _uris[index];
                    _c = new RedisClient(uri.Host, uri.Port);
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

            object call = _c.Call(Commands.GETJOB.ToString(), Keywords.FROM.ToString(), string.Join(" ", queueNames));

            ParseGetJobResponse(call, result);

            return result;
        }

        private void ParseGetJobResponse(object response, List<Job> jobs)
        {
            object[] objects = response as object[];
            if (objects != null)
                jobs.AddRange(from dynamic o in objects select new Job(o[0], o[1], o[2]));
        }

        public List<Job> GetJob(long timeout, long count, List<string> queueNames)
        {
            //GETJOB [TIMEOUT <ms-timeout>] [COUNT <count>] FROM queue1 queue2 ... queueN
            var result = new List<Job>();

            object call = _c.Call(Commands.GETJOB.ToString(),
                Keywords.TIMEOUT.ToString(), timeout.ToString(),
                Keywords.COUNT.ToString(), count.ToString(),
                Keywords.FROM.ToString(),
                string.Join(" ", queueNames));

            ParseGetJobResponse(call, result);

            return result;
        }

        public long Ackjob(List<string> jobIdList)
        {
            return (long)_c.Call(Commands.ACKJOB.ToString(), string.Join(" ", jobIdList));
        }

        public long Ackjob(params string[] jobIds)
        {
            return Ackjob(jobIdList: jobIds.ToList());
        }

        public long Qlen(string queueName)
        {
            throw new NotImplementedException();
        }

        public List<Job> Qpeek(string queueName, long count)
        {
            throw new NotImplementedException();
        }

        public long DelJob(string jobId)
        {
            throw new NotImplementedException();
        }

        public long Dequeue(List<string> jobIds)
        {
            throw new NotImplementedException();
        }

        public long Enqueue(List<string> jobIds)
        {
            throw new NotImplementedException();
        }

        public long Fastack(List<string> jobIdList)
        {
            return (long)_c.Call(Commands.FASTACK.ToString(), string.Join(" ", jobIdList));
        }

        public long Fastack(params string[] jobIds)
        {
            return Fastack(jobIdList: jobIds.ToList());
        }

        public JobInfo Show(string jobId)
        {
            throw new NotImplementedException();
        }

        public string Ping()
        {
            throw new NotImplementedException();
        }

        public long Working(string jobId)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            _c.Dispose();
        }

        public void Dispose()
        {
            _c.Dispose();
        }
    }
}
