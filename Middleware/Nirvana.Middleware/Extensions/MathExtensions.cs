using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Math Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class MathExtensions
    {
        /// <summary>
        /// Calculates standard deviation, from C++ library
        /// The StdDev function calculates the standard deviation of values in an Array
        /// </summary>
        /// <param name="values">enumumerable data</param>
        /// <param name="samp">The samp. 0 = population, Non Zero = sample</param>
        /// <param name="start">The start.</param>
        /// <param name="stop">The stop.</param>
        /// <returns>Standard deviation</returns>
        /// <remarks></remarks>
        public static double StdDev(this IQueryable<double> values, long samp, int start, int stop)
        {
            //validation
            if (values == null || stop < start)
                throw new ArgumentNullException();
            double accum = 0.0;
            double avg = Average(values, start, stop);
            samp = samp == 0 ? 0 : 1;
            for (int i = start; i < stop; i++)
                accum += (values.ElementAt(i) - avg) * (values.ElementAt(i) - avg);
            return (Math.Sqrt(accum / (stop - start + 1 - samp)));
        }
       
        /// <summary>
        /// Calculates standard deviation, same as MATLAB std(X,0) function
        /// <seealso href="http://www.mathworks.co.uk/help/techdoc/ref/std.html">
        /// Mathworks
        /// </seealso>
        /// </summary>
        /// <param name="values">enumumerable data</param>
        /// <param name="start">The start.</param>
        /// <param name="stop">The stop.</param>
        /// <returns>Standard deviation</returns>
        /// <remarks></remarks>
        public static double StdDev(this IEnumerable<double> values, int start=-1, int stop=-1)
        {
            if (start == -1) start = 0;
            if (stop == -1) stop = values.Count();

            //validation
            if (values == null || stop < start)
                throw new ArgumentNullException();
            //saves from devision by 0
            if (stop == 0 || stop == 1)
                return 0;
            double sum = 0.0;
            double sum2 = 0.0;
            for (int i = start; i < stop; i++)
            {
                double item = values.ElementAt(i);
                sum += item;
                sum2 += item * item;
            }
            return Math.Sqrt((sum2 - sum * sum / stop) / (stop - 1));
        }
     
        /// <summary>
        /// Averages the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="start">The start.</param>
        /// <param name="stop">The stop.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double Average(this IQueryable<double> values, int start = 0, int stop = -1)
        {
            double sum = 0.0;
            stop = stop == -1 ? values.Count() : stop;
            if (stop < start)
                throw new ArgumentNullException("Invalid Range");
            for (int i = start; i < stop; i++)
            {
                double item = values.ElementAt(i);
                sum += item;
            }
            return (sum / (stop - start));
        }
      
        /// <summary>
        /// Standards the deviation. Pass in the ma matches excel
        /// </summary>
        /// <param name="valueList">The value list.</param>
        /// <param name="ma">The ma.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double StdDev(this IQueryable<double> valueList, double ma)
        {
            double xMinusMovAvg = 0.0;
            double Sigma = 0.0;
            int k = valueList.Count();
            foreach (double value in valueList)
            {
                xMinusMovAvg = value - ma;
                Sigma = Sigma + (xMinusMovAvg * xMinusMovAvg);
            }
            return Math.Sqrt(Sigma / (k - 1));
        }

        /// <summary>
        /// Calculates Probability Density
        /// </summary>
        /// <param name="t">The t.</param>
        public static double ProbabilityDensity(this double t)
        {
            double _ProbabilityDensity =
                Math.Exp(-(Math.Pow(t, 2) / 2.00)) / (Math.Sqrt(2.00 * 3.14159265358979));
            return _ProbabilityDensity;
        }

        /// <summary>
        /// Calculates Cumulative Distribution
        /// </summary>
        /// <param name="t">The t.</param>
        public static double CumulativeDistribution(this double t)
        {
            double _CumulativeDistribution = 0;
            double t2 = 0, t3 = 0, t4 = 0, t5 = 0;

            t2 = Math.Abs(t);
            t3 = 1 / (1 + 0.23164191 * t2);
            t4 = 0.39894232 * Math.Exp(-t * t / 2);
            t5 = ((((1.330274 * t3 - 1.821256) * t3 + 1.781478) * t3 - 0.3565638) * t3 + 0.319381511);
            if (t > 0)
                _CumulativeDistribution = 1.0 - t4 * t3 * t5;
            else
                _CumulativeDistribution = 1.0 - (1.0 - t4 * t3 * t5);
            return (_CumulativeDistribution);
        }
    }
}
