using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    [AsChoice]
    public static partial class CartPublishedEvent
    {
        public interface ICartPublishedEvent { }

        public record CartPublishSucceededEvent : ICartPublishedEvent
        {
            public string Csv { get; }

            internal ExamGradesPublishSucceededEvent(string csv)
            {
                Csv = csv;
            }
        }

        public record CartPublishFailedEvent : ICartPublishedEvent
        {
            public string Reason { get; }

            internal CartPublishFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}