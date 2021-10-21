using System;
using System.Runtime.Serialization;

namespace Laborator2.Domain
{
    [Serializable]
    internal class InvalidProductNameException : Exception
    {
        public InvalidProductNameException()
        {
        }

        public InvalidProductNameException(string? message) : base(message)
        {
        }

        public InvalidProductNameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProductNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}