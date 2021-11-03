using System;
using System.Runtime.Serialization;

namespace Laborator2.Domain
{
    [Serializable]
    internal class InvalidProductPriceException : Exception
    {
        public InvalidProductPriceException()
        {
        }

        public InvalidProductPriceException(string message) : base(message)
        {
        }

        public InvalidProductPriceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidProductPriceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}