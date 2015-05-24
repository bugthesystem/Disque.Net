using System.Collections.Generic;

namespace Disque.Net
{
    public class JobInfo
    {
        public string Id { get; set; }
        public string Queue { get; set; }
        public string State { get; set; }
        public long Repl { get; set; }
        public long Ttl { get; set; }
        public long Ctime { get; set; }
        public long Delay { get; set; }
        public long Retry { get; set; }
        public List<string> NodesDelivered { get; set; }
        public List<string> NodesConfirmed { get; set; }
        public long RequeueWithin { get; set; }
        public long AwakeWithin { get; set; }
        public string Body { get; set; }
    }
}