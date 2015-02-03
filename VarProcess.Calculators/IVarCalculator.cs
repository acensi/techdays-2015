using VarProcess.Providers;
namespace VarProcess.Calculators
{
    public interface IVarCalculator
    {
        IPortfoliosProvider PortfolioProvider { get; set; }
        IProductParametersProvider ProductParametersProvider { get; set; }
        double Calculate();
    }
}
