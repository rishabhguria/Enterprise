using Prana.LogManager;
using System;

namespace SoftVest.FinLib
{
    public class DateUtils
    {
        public static DateTime SetYear(DateTime dt, int nYear)
        {
            try
            {
                DateTime dtNew = new DateTime(nYear, dt.Month, dt.Day);
                return dtNew;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return new DateTime();
            }
        }
    }
}
