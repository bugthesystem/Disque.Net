using System.Collections.Generic;
using System.Linq;

namespace Disque.Net
{
    public abstract class ResultBuilder<T> where T : new()
    {
        public T BuildFrom(object[] o)
        {
            var res = new T();

            for (int i = 0; i < o.Length; i = i + 2)
            {
                string key = (string) o[i];
                object value = o[i + 1];
                Set(key, value, res);
            }

            return res;
        }

        protected abstract void Set(string key, object value, T inst);

        protected static List<string> ParseNodeArray(object value)
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