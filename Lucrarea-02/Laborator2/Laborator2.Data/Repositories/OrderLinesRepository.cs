using Laborator2.Domain.Models;
using Laborator2.Domain.Repositories;
using LanguageExt;
using Laborator2.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Laborator2.Domain.Models.CartStates;
using static LanguageExt.Prelude;

namespace Laborator2.Data.Repositories
{
    public class OrderLinesRepository : IOrderLinesRepository
    {
        private readonly CartsContext dbContext;

        public OrderLinesRepository(CartsContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public TryAsync<List<CalculatedFinalPrice>> TryGetExistingOrderLine() => async () => (await (
                        from ol in dbContext.OrderLines
                        join oh in dbContext.OrderHeaders on ol.OrderId equals oh.OrderId
                        join p in dbContext.Products on ol.ProductId equals p.ProductId
                        select new { oh.OrderId, p.Code, p.Price, ol.Quantity, oh.Total })
                        .AsNoTracking()
                        .ToListAsync())
                        .Select(result => new CalculatedFinalPrice(
                                                ProductCode: new(result.Code),
                                                ProductPrice: new(result.Price ?? 0m),
                                                ProductQuantity: new(result.Quantity),
                                                TotalPrice: new(result.Total ?? 0m))
                        {
                            OrderId = result.OrderId
                        })
                        .ToList();

        public TryAsync<Unit> TrySaveOrderLinePrice(PaidCartState orderPrice) => async () =>
        {
            var products = (await dbContext.Products.ToListAsync()).ToLookup(product => product.Code);
            var orderLines = (await dbContext.OrderHeaders.ToListAsync()).ToLookup(orderLine => orderLine.OrderId);
            var newOrders = orderPrice.ProductsList
                                     .Where(ol => ol.IsUpdated && ol.OrderId == 0)
                                     .Select(ol => new OrderLineDto()
                                     {
                                         ProductId = products[ol.ProductCode.Value].Single().ProductId,
                                         OrderId = orderLines[ol.OrderId].Single().OrderId,
                                         Quantity = ol.ProductQuantity.Value,
                                         Price = ol.ProductPrice.Value
                                     });
            var updatedOrders = orderPrice.ProductsList.Where(ol => ol.IsUpdated && ol.OrderId > 0)
                                    .Select(ol => new OrderLineDto()
                                    {
                                        OrderLineId = ol.OrderId,
                                        ProductId = products[ol.ProductCode.Value].Single().ProductId,
                                        OrderId = orderLines[ol.OrderId].Single().OrderId,
                                        Quantity = ol.ProductQuantity.Value,
                                        Price = ol.ProductPrice.Value
                                    });

            dbContext.AddRange(newOrders);
            foreach (var entity in updatedOrders)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}