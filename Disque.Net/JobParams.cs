namespace Disque.Net
{
    public class JobParams
    {
        public int Replicate { get; set; }
        public int Delay { get; set; }
        public int Retry { get; set; }
        public int Ttl { get; set; }
        public int Maxlen { get; set; }
        public bool Async { get; set; }
    }
}