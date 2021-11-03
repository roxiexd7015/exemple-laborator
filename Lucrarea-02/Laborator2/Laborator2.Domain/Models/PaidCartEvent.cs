using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    [AsChoice]
    public static partial class PaidCartEvent
    {
        public interface IPaidCartEvent { }

        public record PaidCartSuccedeedEvent : IPaidCartEvent
        {
            public string Csv { get; }

            internal PaidCartSuccedeedEvent(string csv)
            {
                Csv = csv;
            }
        }

        public record PaidCartFailedEvent : IPaidCartEvent
        {
            public string Reason { get; }

            internal PaidCartFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}