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
using System.Linq;
using VarProcess.Data;
using VarProcess.Providers;
using VarProcess.Utilities;

namespace VarProcess.Calculators
{
    public sealed class BasicVarCalculator : IVarCalculator
    {
        private const int TotalSimulations = 2000000;

        private IEnumerable<ProductParameters> ProductParameters { get; set; }

        private IEnumerable<Portfolio> Portfolios { get; set; }

        private IDictionary<Product, long> Positions { get; set; }

        public IPortfoliosProvider PortfolioProvider { get; set; }

        public IProductParametersProvider ProductParametersProvider { get; set; }

        public double Calculate()
        {
            // fetch portfolios by using the provider
            Portfolios = PortfolioProvider.Portfolios;

            // call market data provider and gathering product parameters
            ProductParameters = ProductParametersProvider.ProductsParameters;

            // look over all portfolios and getting the positions
            IEnumerable<KeyValuePair<Product, long>> allTransactions = 
                Portfolios.SelectMany(x => x.Transactions)
                          .GroupBy(y => y.Product)
                          .Select(z => new KeyValuePair<Product, long>(z.Key, z.Sum(x => x.Position)));

            Positions = allTransactions.ToDictionary(t => t.Key, t => t.Value);

            // foreach product do the montecarlo simulation with the target product parameter
            // and multiply it by the position value and calculate the lost value
            var lostsValuesByProduct = new Dictionary<string, IEnumerable<double>>();
            foreach (var pos in Positions)
            {
                long position = pos.Value;
                ProductParameters parameters = ProductParameters.First(x => x.Product.Equals(pos.Key));
                var normalDistribution = new NormalEnumerable();
                var input = new MonteCarloInput
                {
                    Parameters = parameters,
                    Position = position,
                };
                IEnumerable<double> results = normalDistribution.Take(TotalSimulations)
                                                                .Select(alea => StatisticsUtilities.CalculateLoss(input, alea))
                                                                .ToList();
                lostsValuesByProduct.Add(pos.Key.Name, results);
            }

            // aggregate the lost value for all products
            IList<double> totals = new List<double>();
            Func<IList<double>, string, IList<double>> sumList = 
                (current, key) => Helpers.SumList(current, lostsValuesByProduct[key].ToList());
            
            totals = lostsValuesByProduct.Keys.Aggregate(totals, sumList);

            // put prices and positions at the portfolio level
            // choose the var at 99% for 1 day
            return StatisticsUtilities.CalculateVar(totals, 0.99);
        }
    }
}