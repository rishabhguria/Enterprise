using Prana.LogManager;
using System;
using System.Data;
using System.Text;

namespace Prana.Tools
{
    /// <summary>
    /// class for database connection of PricingData
    /// </summary>
    internal class PricingDataDAL
    {
        /// <summary>
        /// get the SM batch data from the database
        /// </summary>
        /// <returns>Datatable holding the data</returns>
        internal static DataTable GetPricingDetails(string symbol, StringBuilder field, DateTime startDate, DateTime endDate)
        {
            DataTable dtPricing = new DataTable();
            try
            {
                string sProc = "P_GetSMPricingData";

                object[] parameter = {
                                       string.IsNullOrEmpty(symbol)?string.Empty:symbol,
                                       string.IsNullOrEmpty(field.ToString())?string.Empty:field.ToString(),
                                       startDate,
                                       endDate
                                     };
                dtPricing = DatabaseManager.DatabaseManager.ExecuteDataSet(sProc, parameter).Tables[0];
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
            }
            return dtPricing;
        }
    }
}
