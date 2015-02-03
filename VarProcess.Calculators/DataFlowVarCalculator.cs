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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using VarProcess.Data;
using VarProcess.Providers;
using VarProcess.Utilities;

namespace VarProcess.Calculators
{
    public sealed class DataFlowVarCalculator : IVarCalculator
    {
        private const int TotalSimulations = 2000000;
        private IEnumerable<ProductParameters> ProductParameters { get; set; }
        private IEnumerable<Portfolio> Portfolios { get; set; }
        public IPortfoliosProvider PortfolioProvider { get; set; }
        public IProductParametersProvider ProductParametersProvider { get; set; }

        private ExecutionDataflowBlockOptions _executionDataflowBlockOptions;
        private ExecutionDataflowBlockOptions ExecutionOptions
        {
            get
            {
                return _executionDataflowBlockOptions ?? (_executionDataflowBlockOptions = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount * 2
                });
            }
        }

        private DataflowLinkOptions _dataflowLinkOptions;
        private DataflowLinkOptions DataflowLinkOptions
        {
            get
            {
                return _dataflowLinkOptions ?? (_dataflowLinkOptions = new DataflowLinkOptions
                {
                    PropagateCompletion = true
                });
            }
        }

        public double Calculate()
        {
            Portfolios = PortfolioProvider.Portfolios.ToList();
            ProductParameters = ProductParametersProvider.ProductsParameters.ToList();

            var monteCarlo = new TransformBlock<MonteCarloInput, IEnumerable<double>>(input =>
            {
                var normalDistribution = new NormalEnumerable();
                return normalDistribution.Take(TotalSimulations)
                                         .Select(alea => StatisticsUtilities.CalculateLoss(input, alea))
                                         .ToList();
            }, ExecutionOptions);

            var totals = new List<double>();
            var aggregate = new ActionBlock<IEnumerable<double>>(doubles =>
            {
                lock (totals)
                {
                    if (!totals.Any())
                    {
                        totals.AddRange(doubles);
                    }
                    else
                    {
                        var losses = doubles.ToList();
                        foreach (var i in Enumerable.Range(0, losses.Count()))
                        {
                            totals[i] += losses[i];
                        }
                    }
                }
            }, ExecutionOptions);

            monteCarlo.LinkTo(aggregate, DataflowLinkOptions);
#if false
            monteCarlo.Completion.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    ((IDataflowBlock) aggregate).Fault(t.Exception); // Pass exception
                }
                else
                {
                    aggregate.Complete(); // Mark next completed
                }
            });
#endif
            foreach (var portfolio in Portfolios.SelectMany(x => x.Transactions)
                                                .GroupBy(y => y.Product)
                                                .Select(z => new KeyValuePair<Product, long>(z.Key, z.Sum(x => x.Position))))
            {
                var position = portfolio.Value;
                var parameters = ProductParameters.First(x => x.Product.Equals(portfolio.Key));
                monteCarlo.Post(new MonteCarloInput
                {
                    Parameters = parameters,
                    Position = position,
                });
            }
            monteCarlo.Complete();
            aggregate.Completion.Wait();
            return StatisticsUtilities.CalculateVar(totals, 0.99);
        }
    }
}
