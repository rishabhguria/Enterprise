using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    public class EnableDisabledItemsDAL
    {
        /// <summary>
        /// Initialize Connection string for data base 
        /// </summary>
        static string connStr = "PranaConnectionString";


        /// <summary>
        /// Method To Get All the entries that are currently Disabled pertaining to a particular Type
        /// Created By Faisal Shah
        /// Dated 14/07/14
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>

        internal static DataTable GetAllDisabledItems(int itemType)
        {
            DataTable dtEnableDisabledItems = new DataTable();
            string spName = "P_GetDisabledItems";
            object[] parameter = { itemType };
            try
            {
                dtEnableDisabledItems = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter, connStr).Tables[0];
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dtEnableDisabledItems;
        }


        /// <summary>
        /// Created By Faisal Shah 14/07/14
        /// Updating Data in Database. Selected Values are Enabled
        /// </summary>
        /// <param name="xmlItemsTeBeEnabled"></param>
        /// <returns></returns>
        internal static int SaveItemsToBeEnabled(string xmlItemsTeBeEnabled)
        {
            int count = 0;

            object[] parameter = { xmlItemsTeBeEnabled };

            string sProcSaveData = "P_SaveItemsToBeEnabled";
            try
            {
                count = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProcSaveData, parameter, connStr);
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
            return count;
        }
    }
}
