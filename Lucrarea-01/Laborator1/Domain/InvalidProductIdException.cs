using System;
using System.Runtime.Serialization;

namespace Laborator1.Domain
{
    [Serializable]
    internal class InvalidProductIdException : Exception
    {
        public InvalidProductIdException()
        {
        }

        public InvalidProductIdException(string? message) : base(message)
        {
        }

        public InvalidProductIdException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProductIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}