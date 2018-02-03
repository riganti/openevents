using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenEvents.Backend.Common.Exceptions
{
    public abstract class ConflictException : ApplicationException
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
