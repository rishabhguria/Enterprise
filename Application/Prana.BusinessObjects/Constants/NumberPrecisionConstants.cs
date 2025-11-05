using System;

namespace Prana.BusinessObjects.Constants
{
    /// <summary>
    /// Number precision Constants
    /// </summary>
    public static class NumberPrecisionConstants
    {
        /// <summary>
        /// Constants decimal.One which should be used to devide the decimal number before converting them to double
        /// </summary>
        public const decimal DecimalPreciseOne = 1.000000000000000000000000000000000000000000000000M;

        /// <summary>
        /// Precision format
        /// </summary>
        static string _precisionFormat = "####,###,###,###,##0.";
        static int _precisionDigits = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PrecisionDigits"]);

        /// <summary>
        /// Constructor to add precision digit in precision format
        /// </summary>
        static NumberPrecisionConstants()
        {
            for (int i = 0; i < _precisionDigits; i++)
            {
                _precisionFormat += "#";
            }
        }

        /// <summary>
        /// Getting precision digits
        /// </summary>
        public static int GetPrecisionDigit()
        {
            return _precisionDigits;
        }

        /// <summary>
        /// Converts decimal number to double with taking care of precision
        /// </summary>
        /// <param name="val">Decimal value</param>
        /// <returns>Double output</returns>
        public static double ToDoublePrecise(this decimal val)
        {
            return Convert.ToDouble(Math.Round(val, 10) / DecimalPreciseOne);
        }

        /// <summary>
        /// Checks if given decimal number is equal to provided double
        /// </summary>
        /// <param name="val">Decimal number</param>
        /// <param name="doubleValue">Value to be compared to decimal</param>
        /// <returns>true if equal otherwise false</returns>
        public static bool EqualsPrecise(this decimal val, double doubleValue)
        {
            return Math.Round(val, 10).ToDoublePrecise().Equals(Math.Round(doubleValue, 10));
        }

        public static bool EqualsPrecise(this decimal val, decimal decimalValue)
        {
            return Math.Round(val, 10) == Math.Round(decimalValue, 10);
        }

        /// <summary>
        /// Calculates precision for columns format
        /// </summary>
        public static string GetPrecisionFormat()
        {
            return _precisionFormat;
        }

        /// <summary>
        /// Removing trailing Zero's
        /// </summary>
        /// <param name="val">Decimal value</param>
        /// <returns>String without trailing zero's</returns>
        public static string RemoveTrailingZeros(this decimal val)
        {
            return (val / DecimalPreciseOne).ToString(GetPrecisionFormat());
        }

    }
}
