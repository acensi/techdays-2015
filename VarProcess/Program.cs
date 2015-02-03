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

            Console.WriteLine(" * Starting BasicVarCalculator");
            var basicPerf = RunCalculator<BasicVarCalculator>(portfolioProvider, productParametersProvider);
            Console.WriteLine(" * Starting DataFlowCalculator with {0} processors", Environment.ProcessorCount);
            var dataFlowPerf = RunCalculator<DataFlowVarCalculator>(portfolioProvider, productParametersProvider);
            Console.WriteLine(" Delta t = {0} ms ({1:0.00} %)", dataFlowPerf - basicPerf, (dataFlowPerf - basicPerf) / (double)basicPerf * 100.0);
            Console.ReadKey();
        }

        private static long RunCalculator<TCalculator>(PortfoliosProvider portfolioProvider,
                                                       StocksPricesProvider productParametersProvider) where TCalculator : IVarCalculator, new()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var calculator = new TCalculator
            {
                PortfolioProvider = portfolioProvider,
                ProductParametersProvider = productParametersProvider
            };
            var result = calculator.Calculate();
            stopwatch.Stop();
            Console.WriteLine("\tResult -> {0}, Duration -> {1} ms",
                                result, stopwatch.ElapsedMilliseconds);
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
