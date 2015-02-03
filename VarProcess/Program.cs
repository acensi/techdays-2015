using System;
using System.Diagnostics;
using VarProcess.Calculators;
using VarProcess.Providers;

namespace VarProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var portfolioProvider = new PortfoliosProvider(@"..\..\..\datas\Portfolios");
            var productParametersProvider = new StocksPricesProvider(@"..\..\..\datas\Parameters");
           
            RunBasicVarCalculator(portfolioProvider, productParametersProvider);
            
            RunDataflowCalculator(portfolioProvider, productParametersProvider);
           
            Console.ReadKey();

        }

        private static void RunDataflowCalculator(PortfoliosProvider portfolioProvider, 
                                                  StocksPricesProvider productParametersProvider)
        {
            var sw2 = new Stopwatch();
            sw2.Start();
            var dataflowVarCalculator = new DataFlowVarCalculator
            {
                PortfolioProvider = portfolioProvider,
                ProductParametersProvider = productParametersProvider
            };
            var result = dataflowVarCalculator.Calculate();
            sw2.Stop();
            Console.WriteLine("Var Dataflow Algorithm : Result -> {0} Duration -> {1} ms", 
                                result, sw2.ElapsedMilliseconds);
        }

        private static void RunBasicVarCalculator(PortfoliosProvider portfolioProvider,
                                                 StocksPricesProvider productParametersProvider)
        {
            var sw = new Stopwatch();
            sw.Start();
            var basicVarCalculator = new BasicVarCalculator
            {
                PortfolioProvider = portfolioProvider,
                ProductParametersProvider = productParametersProvider
            };
            var result = basicVarCalculator.Calculate();
            sw.Stop();
            Console.WriteLine("Var Basic Algorithm : Result -> {0} Duration -> {1} ms", 
                                result, sw.ElapsedMilliseconds);
        }
    }
}
