using System;
using System.Runtime.Serialization;

namespace Laborator2.Domain
{
    [Serializable]
    internal class InvalidClientNameException : Exception
    {
        public InvalidClientNameException()
        {
        }

        public InvalidClientNameException(string? message) : base(message)
        {
        }

        public InvalidClientNameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidClientNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}