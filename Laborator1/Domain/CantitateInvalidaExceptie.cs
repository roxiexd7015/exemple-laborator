using System;
using System.Runtime.Serialization;

namespace Laborator1.Domain
{
    [Serializable]
    internal class CantitateInvalidaExceptie : Exception
    {
        public CantitateInvalidaExceptie()
        {
        }

        public CantitateInvalidaExceptie(string? mesaj) : base(mesaj)
        {
            Console.WriteLine($"{mesaj}");
        }

        public CantitateInvalidaExceptie(string? mesaj, Exception? innerException) : base(mesaj, innerException)
        {
        }

        protected CantitateInvalidaExceptie(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}