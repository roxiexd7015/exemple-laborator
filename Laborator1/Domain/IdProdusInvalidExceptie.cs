using System;
using System.Runtime.Serialization;

namespace Laborator1.Domain
{
    [Serializable]
    internal class IdProdusInvalidExceptie : Exception
    {
        public IdProdusInvalidExceptie()
        {
        }

        public IdProdusInvalidExceptie(string? mesaj) : base(mesaj)
        {
            Console.WriteLine($"{mesaj}");
        }

        public IdProdusInvalidExceptie(string? mesaj, Exception? innerException) : base(mesaj, innerException)
        {
        }

        protected IdProdusInvalidExceptie(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}