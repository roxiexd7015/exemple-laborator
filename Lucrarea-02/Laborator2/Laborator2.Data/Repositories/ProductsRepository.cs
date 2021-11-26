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
    public class ProductsRepository : IProductsRepository
    {
        private readonly CartsContext cartsContext;

        public ProductsRepository(CartsContext cartsContext)
        {
            this.cartsContext = cartsContext;
        }

        public TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> productToCheck) => async () =>
        {
            var products = await cartsContext.Products
                                             .Where(product => productToCheck.Contains(product.Code))
                                             .AsNoTracking()
                                             .ToListAsync();
            return products.Select(product => new ProductCode(product.Code))
                           .ToList();
        };

    }
}