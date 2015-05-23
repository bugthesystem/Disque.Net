using System;
using System.Runtime.Serialization;

namespace Disque.Net
{
    [Serializable]
    public class DisqueConnectionException : Exception
    {
        public DisqueConnectionException()
        {
        }

        public DisqueConnectionException(string message) : base(message)
        {
        }

        public DisqueConnectionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DisqueConnectionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}