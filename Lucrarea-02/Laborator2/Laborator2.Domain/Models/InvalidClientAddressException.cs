using System;
using System.Runtime.Serialization;

namespace Laborator2.Domain
{
    [Serializable]
    internal class InvalidClientAddressException : Exception
    {
        public InvalidClientAddressException()
        {
        }

        public InvalidClientAddressException(string message) : base(message)
        {
        }

        public InvalidClientAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidClientAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}