using System.Collections;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using VarProcess.Data;
namespace VarProcess.Utilities
{
    public class NormalEnumerable : IEnumerable<double>
    {
        public IEnumerator<double> GetEnumerator()
        {
            return new NormalEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NormalEnumerator : IEnumerator<double>
    {
        private Normal _distrib = new Normal();
        private double _current = 0.0;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            _current = _distrib.Sample();
            return true;
        }

        public void Reset()
        {
            _distrib = new Normal();
        }

        public double Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }

    public static class StatisticsUtilities
    {
        public static IEnumerable<double> SimulateMonteCarloWithPosition(MonteCarloInput input, int totalSimulations)
        {
            var distribution = new Normal();
            return Enumerable.Range(1, totalSimulations).Select(x => distribution.Sample()).Select(alea => CalculateLoss(input, alea)).ToList();
        }

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

    public struct MonteCarloInput
    {
        public Product Product;
        public ProductParameters Parameters;
        public double Position;
    }
}
