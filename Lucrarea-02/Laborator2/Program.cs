using Laborator2.Domain.Models;
using Laborator2.Domain;
using System;
using System.Collections.Generic;
using static Laborator2.Domain.Models.CartStates;
using static Laborator2.Domain.Models.PaidCartEvent;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Threading.Tasks;
using System.Net.Http;
using Laborator2.Data.Repositories;
using Laborator2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Laborator2
{
    class Program
    {
        private static readonly Random random = new Random();
        private static string ConnectionString = "Server=DESKTOP-5M0QVHJ;Database=pssc_db_hw;Trusted_Connection=True;MultipleActiveResultSets=true";
        static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
            ILogger<PaidCartWorkflow> logger = loggerFactory.CreateLogger<PaidCartWorkflow>();

            var productsList = ReadProductsList().ToArray();
            PayCartCommand command = new(productsList);
            var dbContextBuilder = new DbContextOptionsBuilder<CartsContext>()
                                                .UseSqlServer(ConnectionString)
                                                .UseLoggerFactory(loggerFactory);
            CartsContext cartsContext = new CartsContext(dbContextBuilder.Options);
            ProductsRepository productsRepository = new(cartsContext);
            OrderLinesRepository orderLinesRepository = new(cartsContext);
            OrderHeadersRepository orderHeadersRepository = new(cartsContext);

            PaidCartWorkflow workflow = new(productsRepository, orderLinesRepository, orderHeadersRepository, logger);
            var result = await workflow.ExecuteAsync(command);
            //var result = await workflow.ExecuteAsync(command, CheckProductExists, CheckStock, CheckAddress);
            result.Match(
                    whenPaidCartFailedEvent: @event =>
                    {
                        Console.WriteLine($"Payment failed: {@event.Reason}");
                        Console.WriteLine("We are sorry to announce you that the delivery could not be proccesed any further away.");
                        return @event;
                    },
                    whenPaidCartSuccedeedEvent: @event =>
                    {
                        Console.WriteLine($"Payment succeeded.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );
        }
        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
                                builder.AddSimpleConsole(options =>
                                {
                                    options.IncludeScopes = true;
                                    options.SingleLine = true;
                                    options.TimestampFormat = "hh:mm:ss ";
                                })
                                .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
        }
        private static List<UnvalidatedCart> ReadProductsList()
        {
            List<UnvalidatedCart> productsList = new();
            bool state = false;
            var name = ReadValue("Client name: ");
            Console.WriteLine("Welcome, " + name.ToString());
            var address = ReadValue("Client address: ");
            Console.WriteLine("Delivery will be made at: " + address.ToString());
            Console.WriteLine("Welcome! Please enjoy your shopping!");
            do
            {
                var code = ReadValue("Product code: ");
                if (string.IsNullOrEmpty(code))
                {
                    break;
                }
                var price = ReadValue("Product price: ");
                if (string.IsNullOrEmpty(price))
                {
                    break;

                }
                var quantity = ReadValue("Product quantity: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }
                productsList.Add(new(code, price, quantity, address));

                var finishShopping = ReadValue("Finish shopping (Y/N): ");
                if (string.IsNullOrEmpty(finishShopping))
                {
                    break;
                }
                if (string.Compare(finishShopping, "Y") == 0)
                {
                    state = false;
                }
                else if (string.Compare(finishShopping, "N") == 0)
                {
                    state = true;
                }
            } while (state);
            return productsList;
        }

        private static string ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}


/*
           var testCode = ProductCode.TryParseCode("ID123");
           var productExists = await testCode.Match(
               Some: testCode => CheckProductExists(testCode).Match(Succ: value => value, exception => false),
               None: () => Task.FromResult(false)
           );

           var result = from productCode in ProductCode.TryParseCode("ID123")
                                                   .ToEitherAsync(() => "Invlid product code.")
                        from exists in CheckProductExists(productCode)
                                                   .ToEither(ex =>
                                                   {
                                                       Console.Error.WriteLine(ex.ToString());
                                                       return "Could not validate product code";
                                                   })
                        select exists;

           await result.Match(
                Left: message => Console.WriteLine(message),
                Right: flag => Console.WriteLine(flag));
           //*/

///*
