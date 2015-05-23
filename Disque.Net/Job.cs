namespace Disque.Net
{
    public class Job
    {
        public string Id { get; set; }
        public string QueueName { get; set; }
        public string Body { get; set; }

        public Job()
        {
        }

        public Job(string queueName, string id, string body)
        {
            this.Id = id;
            this.QueueName = queueName;
            this.Body = body;
        }

    }
}