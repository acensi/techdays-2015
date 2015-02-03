using System.Collections.Generic;
using System.Linq;

namespace VarProcess.Utilities
{
    public static class Helpers
    {
        public static IList<double> SumList(IList<double> firsts, IList<double> seconds)
        {
            var result = new List<double>();
            if (firsts.Count == 0)
            {
                return seconds;
            }

            foreach (var i in Enumerable.Range(0, seconds.Count))
            {
                result.Add(firsts[i] + seconds[i]);
            }
            return result;

        }
    }
}
