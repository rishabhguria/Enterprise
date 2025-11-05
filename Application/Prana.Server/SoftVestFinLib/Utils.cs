using Prana.LogManager;
using System;

namespace SoftVest.FinLib
{
    public class Utils
    {

        public static string GetStandardUSDateFormat(DateTime dt)
        {
            try
            {
                string dateString = "";
                dateString = String.Format("{0:MM/dd/yyyy}", dt);
                return dateString;
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

                return string.Empty;
            }
        }

        public static string GetXmlElement(string sTag, string sVal)
        {
            try
            {
                string sXml = "<" + sTag + ">" + sVal + "</" + sTag + ">";
                return sXml;
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

                return string.Empty;
            }
        }
    }
}
