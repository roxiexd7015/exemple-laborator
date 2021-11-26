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
        private readonly CartsContext cartsContext;

        public OrderLinesRepository(CartsContext cartsContext, CartsContext dbContext)
        {
            this.cartsContext = cartsContext;
            this.dbContext = dbContext;
        }

        public TryAsync<List<ProductQuantity>> TryGetExistingQuantity(IEnumerable<int> quantityToCheck) => async () =>
        {
            var products = await cartsContext.OrderLines
                                             .Where(productQuantity => quantityToCheck.Equals(productQuantity.Quantity))
                                             .AsNoTracking()
                                             .ToListAsync();
            return products.Select(productQuantity => new ProductQuantity(productQuantity.Quantity))
                           .ToList();
        };
        //public TryAsync<List<CalculatedFinalPrice>> TryGetExistingPrice()

    }
}