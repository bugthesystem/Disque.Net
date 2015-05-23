using System.Collections.Generic;

namespace Disque.Net
{
    public class JobInfo
    {
        public string Id { get; }
        public string Queue { get; }
        public string State { get; }
        public long Repl { get; }
        public long Ttl { get; }
        public long Ctime { get; }
        public long Delay { get; }
        public long Retry { get; }
        public List<string> NodesDelivered { get; }
        public List<string> NodesConfirmed { get; }
        public long RequeueWithin { get; }
        public long AwakeWithin { get; }
        public string Body { get; }

        public JobInfo(string id, string queue, string state, long repl, long ttl, long ctime, long delay, long retry,
            List<string> nodesDelivered, List<string> nodesConfirmed, long requeueWithin, long awakeWithin, string body)
        {

            this.Id = id;
            this.Queue = queue;
            this.State = state;
            this.Repl = repl;
            this.Ttl = ttl;
            this.Ctime = ctime;
            this.Delay = delay;
            this.Retry = retry;
            this.NodesDelivered = nodesDelivered;
            this.NodesConfirmed = nodesConfirmed;
            this.RequeueWithin = requeueWithin;
            this.AwakeWithin = awakeWithin;
            this.Body = body;
        }
    }
}