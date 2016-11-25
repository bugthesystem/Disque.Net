using System.Collections.Generic;

namespace Disque.Net
{
    public class Qstat
    {
        public string Name { get; set; }
        public long Length { get; set; }
        public long Age { get; set; }
        public long Idle { get; set; }
        public long Blocked { get; set; }
        public List<string> ImportFrom { get; set; }
        public long ImportRate { get; set; }
        public long JobsIn { get; set; }
        public long JobsOut { get; set; }
        public string Pause { get; set; }
    }
}