using System;
using System.Runtime.Serialization;

namespace Prana.ExposurePnlCache
{
    [Serializable]
    public class PropertyAccessorException : Exception
    {
        public PropertyAccessorException(string message) : base(message)
        {

        }

        protected PropertyAccessorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
