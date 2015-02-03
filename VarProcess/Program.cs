// Copyright 2015 ACENSI http://www.acensi.fr/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
