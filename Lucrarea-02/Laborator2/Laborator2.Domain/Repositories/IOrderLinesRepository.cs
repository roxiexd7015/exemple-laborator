using Laborator2.Domain.Models;
using LanguageExt;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Laborator2.Domain.Models.CartStates;

namespace Laborator2.Domain.Repositories
{
    public interface IOrderLinesRepository
    {
        TryAsync<List<CalculatedFinalPrice>> TryGetExistingOrderLine();
        TryAsync<Unit> TrySaveOrderLinePrice(PaidCartState orderPrice);
    }
}