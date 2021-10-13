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
        public record CosInvalid(IReadOnlyCollection<CosClientNevalidat> ListaProduse, string motiv) : IStareCos;
        public record CosValid(IReadOnlyCollection<CosClientValidat> ListaProduse) : IStareCos;
        public record CosNevalid(IReadOnlyCollection<CosClientNevalidat> ListaProduse) : IStareCos;
        public record CosPlatit(IReadOnlyCollection<CosClientValidat> ListaProduse) : IStareCos;
        public record CosGol() : IStareCos;
    }
}
