using System.Collections.Generic;
using VarProcess.Data;
namespace VarProcess.Providers
{
    public interface IPortfoliosProvider
    {
        IEnumerable<Portfolio> Portfolios { get; }
    }
}
