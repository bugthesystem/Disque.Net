using System.Collections.Generic;
using System.Linq;

namespace Disque.Net
{
    public class JobInfoBuilder : IJobInfoBuilder
    {
        public JobInfo BuildFrom(object[] o)
        {

            JobInfo resultInfo = new JobInfo();
            for (int i = 0; i < o.Length; i = i + 2)
            {
                string key = (string)o[i];
                object value = o[i + 1];
                switch (key)
                {
                    case "id":
                        {
                            resultInfo.Id = (string)value;
                            break;
                        }

                    case "queue":
                        {
                            resultInfo.Queue = (string)value;
                            break;
                        }
                    case "state":
                        {
                            resultInfo.State = (string)value;
                            break;
                        }
                    case "repl":
                        {
                            resultInfo.Repl = (long)value;
                            break;
                        }
                    case "ttl":
                        {
                            resultInfo.Ttl = (long)value;
                            break;
                        }
                    case "ctime":
                        {
                            resultInfo.Ctime = (long)value;
                            break;
                        }
                    case "delay":
                        {
                            resultInfo.Delay = (long)value;
                            break;
                        }
                    case "retry":
                        {
                            resultInfo.Retry = (long)value;
                            break;
                        }
                    case "nodes-delivered":
                        {
                            resultInfo.NodesDelivered = ParseNodeArray(value);
                            break;
                        }
                    case "nodes-confirmed":
                        {
                            resultInfo.NodesConfirmed = ParseNodeArray(value);
                            break;
                        }
                    case "next-requeue-within":
                        {
                            resultInfo.RequeueWithin = (long)value;
                            break;
                        }
                    case "next-awake-within":
                        {
                            resultInfo.AwakeWithin = (long)value;
                            break;
                        }
                    case "body":
                        {
                            resultInfo.Body = (string)value;
                            break;
                        }
                }
            }

            return resultInfo;
        }

        private static List<string> ParseNodeArray(object value)
        {
            List<string> result = new List<string>();

            object[] nodes = value as object[];

            if (nodes != null)
            {
                result.AddRange(nodes.Select(node => node.ToString()));
            }

            return result;
        }
    }
}