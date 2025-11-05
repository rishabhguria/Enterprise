using System;
using System.Runtime.Serialization;

namespace Prana.LogManager
{
    [Serializable]
    public class ProxyException : Exception
    {
        public ProxyException(string message)
            : base(message)
        {
        }

        protected ProxyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}