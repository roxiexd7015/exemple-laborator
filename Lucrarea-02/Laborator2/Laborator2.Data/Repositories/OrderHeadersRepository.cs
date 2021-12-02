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
    public class OrderHeadersRepository : IOrderHeadersRepository
    {
        private readonly CartsContext cartsContext;

        public OrderHeadersRepository(CartsContext cartsContext)
        {
            this.cartsContext = cartsContext;
        }
        public TryAsync<List<Client>> TryGetExistingOrderHeader(IEnumerable<string> addressToCheck) => async () =>
          {
              var orderHeaders = await cartsContext.OrderHeaders
                                              .Where(orderHeaders => addressToCheck.Contains(orderHeaders.Address))
                                              .AsNoTracking()
                                              .ToListAsync();
              return orderHeaders.Select(orderHeaders => new Client(orderHeaders.Address))
                                  .ToList();
          };

    }
}