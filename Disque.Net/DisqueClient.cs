using System;
using System.Collections.Generic;
using Nhiredis;

namespace Disque.Net
{
    public class DisqueClient : ISyncDisqueClient, IDisposable
    {
        private const string DISQUE_PROTOCOL = "disque://";
        private const string DISQUE_HOST = "127.0.0.1";
        private const int DISQUE_PORT = 7711;
        private const int DEFAULT_TIMEOUT_IN_SECS = 2;
        private readonly List<Uri> _uris = new List<Uri>();
        private readonly Random _randomGenerator = new Random();
        private RedisClient _c;

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
                    _c = new RedisClient(uri.Host, uri.Port, TimeSpan.FromSeconds(DEFAULT_TIMEOUT_IN_SECS));
                }
                catch (NhiredisException e)
                {
                    _uris.RemoveAt(index);
                }
            }
        }

        public string AddJob(string queueName, string job, int mstimeout)
        {
            //ADDJOB queue_name job <ms-timeout> [REPLICATE <count>] [DELAY <sec>] [RETRY <sec>] [TTL <sec>] [MAXLEN <count>] [ASYNC]
            return _c.RedisCommand<string>(Commands.ADDJOB.ToString(), queueName, job, mstimeout);
        }

        public string AddJob(string queueName, string job, long mstimeout, JobParams jobParams)
        {
            //ADDJOB queue_name job <ms-timeout> [REPLICATE <count>] [DELAY <sec>] [RETRY <sec>] [TTL <sec>] [MAXLEN <count>] [ASYNC]
            return _c.RedisCommand<string>(Commands.ADDJOB.ToString(), queueName, job, mstimeout, jobParams.Replicate, jobParams.Delay, jobParams.Retry,
                                                                         jobParams.Ttl, jobParams.Maxlen, jobParams.Async);

        }

        public List<Job> GetJob(List<string> queueNames)
        {
            throw new NotImplementedException();
        }

        public List<Job> GetJob(long timeout, long count, List<string> queueNames)
        {
            throw new NotImplementedException();
        }

        public long Ackjob(List<string> jobIds)
        {
            throw new NotImplementedException();
        }

        public string Info()
        {
            var arguments = Commands.INFO.ToString();
            return _c.RedisCommand<string>(arguments);
        }

        public string Info(string section)
        {
            return _c.RedisCommand<string>("INFO", section);
        }

        public long Qlen(string queueName)
        {
            return 10;
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

        public long Fastack(List<string> jobIds)
        {
            throw new NotImplementedException();
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

        public bool Close()
        {
            //TODO:
            _c.Dispose();
            return true;
        }

        public void Dispose()
        {
            _c.Dispose();
        }
    }
}
