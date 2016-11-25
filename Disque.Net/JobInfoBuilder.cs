namespace Disque.Net
{
    public class JobInfoBuilder : ResultBuilder<JobInfo>, IJobInfoBuilder
    {
        protected override void Set(string key, object value, JobInfo inst)
        {
            switch (key)
            {
                case "id":
                    {
                        inst.Id = (string)value;
                        break;
                    }

                case "queue":
                    {
                        inst.Queue = (string)value;
                        break;
                    }
                case "state":
                    {
                        inst.State = (string)value;
                        break;
                    }
                case "repl":
                    {
                        inst.Repl = (long)value;
                        break;
                    }
                case "ttl":
                    {
                        inst.Ttl = (long)value;
                        break;
                    }
                case "ctime":
                    {
                        inst.Ctime = (long)value;
                        break;
                    }
                case "delay":
                    {
                        inst.Delay = (long)value;
                        break;
                    }
                case "retry":
                    {
                        inst.Retry = (long)value;
                        break;
                    }
                case "nodes-delivered":
                    {
                        inst.NodesDelivered = ParseNodeArray(value);
                        break;
                    }
                case "nodes-confirmed":
                    {
                        inst.NodesConfirmed = ParseNodeArray(value);
                        break;
                    }
                case "next-requeue-within":
                    {
                        inst.RequeueWithin = (long)value;
                        break;
                    }
                case "next-awake-within":
                    {
                        inst.AwakeWithin = (long)value;
                        break;
                    }
                case "body":
                    {
                        inst.Body = (string)value;
                        break;
                    }
            }
        }
    }
}