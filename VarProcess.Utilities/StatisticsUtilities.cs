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

using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VarProcess.Utilities
{
    public static class StatisticsUtilities
    {
        public static double CalculateLoss(MonteCarloInput input, double alea)
        {
            var pricePosition = input.Position * input.Parameters.Price * Math.Exp(input.Parameters.Mean - Math.Pow(input.Parameters.StandardDeviation, 2) / 2 + alea * input.Parameters.StandardDeviation);
            return (input.Position * input.Parameters.Price) - pricePosition;
        }

        public static double CalculateVar(IList<double> datas, double quantile)
        {
            return ArrayStatistics.QuantileInplace(datas.ToArray(), 0.99);

        }
    }
}
