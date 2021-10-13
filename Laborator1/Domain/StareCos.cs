using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;

namespace Laborator1.Domain
{
    [AsChoice]
    public static partial class StareCos
    {
        public interface IStareCos { }
        public record CosValid(IReadOnlyCollection<CosClientValidat> ListaProduse): IStareCos;
        public record CosNevalid(IReadOnlyCollection<CosClientNevalidat> ListaProduse, string motiv): IStareCos;
        public record CosPlatit(IReadOnlyCollection<CosClientValidat> ListaProduse, float totalPlatit): IStareCos;
        public record CosGol(IReadOnlyCollection<CosClientValidat> ListaProduse): IStareCos;
    }
}
