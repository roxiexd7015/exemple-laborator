using Laborator2.Domain.Models;
using LanguageExt;
using System.Collections.Generic;
using static Laborator2.Domain.Models.CartStates;

namespace Laborator2.Domain.Repositories
{
    public interface IOrderHeadersRepository
    {
        TryAsync<List<CalculatedFinalPrice>> TryGetExistingPrice();

        TryAsync<Unit> TrySavePrice(PaidCartState price);
    }
}