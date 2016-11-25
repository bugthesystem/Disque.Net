namespace Disque.Net
{
    public class QstatBuilder : ResultBuilder<Qstat>, IQstatBuilder
    {
        protected override void Set(string key, object value, Qstat inst)
        {
            switch (key)
            {
                case "name":
                    inst.Name = (string) value;
                    break;
                case "len":
                    inst.Length = (long) value;
                    break;
                case "age":
                    inst.Age = (long) value;
                    break;
                case "idle":
                    inst.Idle = (long) value;
                    break;
                case "blocked":
                    inst.Blocked = (long) value;
                    break;
                case "import-from":
                    inst.ImportFrom = ParseNodeArray(value);
                    break;
                case "jobs-in":
                    inst.JobsIn = (long) value;
                    break;
                case "jobs-out":
                    inst.JobsOut = (long) value;
                    break;
                case "pause":
                    inst.Pause = (string) value;
                    break;
            }
        }
    }
}