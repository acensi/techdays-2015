using System.Collections.Generic;
using VarProcess.Data;

namespace VarProcess.Providers
{
    public interface IProductParametersProvider
    {
       IEnumerable<ProductParameters> ProductsParameters { get; }
    }
}