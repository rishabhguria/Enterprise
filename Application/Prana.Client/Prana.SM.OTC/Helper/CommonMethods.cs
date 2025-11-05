using Prana.LogManager;
using System;

namespace Prana.SM.OTC.Helper
{
    public class CommonMethods
    {
        /// <summary>
        /// Sets the precision string format.
        /// </summary>
        /// <param name="precisionDigit">The precision digit.</param>
        /// <returns></returns>
        internal static string SetPrecisionStringFormat(int precisionDigit)
        {
            string precisionFormat = "{0:####,###,###,###,##0.";
            try
            {
                for (int i = 0; i < precisionDigit; i++)
                {
                    precisionFormat += "#";
                }
                precisionFormat += "}";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return precisionFormat;
        }
    }
}
