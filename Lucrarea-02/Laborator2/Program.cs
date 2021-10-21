using Laborator2.Domain.Models;
using System;
using System.Collections.Generic;
using static Laborator2.Domain.Models.CartStates;
using static Laborator2.Domain.ProductsPriceOperation;
using Laborator2.Domain;

namespace Laborator2
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var productsList = ReadProductsList().ToArray();
            PublishCartCommand command = new(productsList);
            PublishCartWorkflow workflow = new PublishCartWorkflow();
            var result = workflow.Execute(command, (name) => true);

            result.Match(
                    whenCartPublishFailedEvent: @event =>
                    {
                        Console.WriteLine($"Publish failed: {@event.Reason}");
                        return @event;
                    },
                    whenCartPublishSucceededEvent: @event =>
                    {
                        Console.WriteLine($"Publish succeeded.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );

            Console.WriteLine("Hello World!");
        }

        private static List<UnvalidateCart> ReadProductsList()
        {
            List<UnvalidateCart> productsList = new();
            do
            {
                var name = ReadValue("Client name: ");
                if (string.IsNullOrEmpty(name))
                {
                    break;
                }

                var cart = ReadValue("Cart: ");
                if (string.IsNullOrEmpty(cart))
                {
                    break;
                }

                var activityClient = ReadValue("Activity Client: ");
                if (string.IsNullOrEmpty(activityClient))
                {
                    break;
                }

                productsList.Add(new(name, cart, activityClient));
            } while (true);
            return productsList;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
