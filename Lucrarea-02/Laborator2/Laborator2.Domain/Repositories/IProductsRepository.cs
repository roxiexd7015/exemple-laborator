using Laborator2.Domain.Models;
using LanguageExt;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Repositories
{
    public interface IProductsRepository
    {
        TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> productToCheck);
    }
}