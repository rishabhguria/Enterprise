using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ClientCommon.Data_Layer
{
    /// <summary>
    /// Data layer for all common data fetching from DB
    /// Created By: Omshiv, March 2014
    /// </summary>
    class CommonDataManager
    {
        /// <summary>
        /// get NAV lock details like lock dates, Lock status
        /// Created By: Omshiv , March 2014
        /// </summary>
        public static Dictionary<int, NAVLockItem> GetAccountNAVLockDetails()
        {
            Dictionary<int, NAVLockItem> accountNavLockDict = new Dictionary<int, NAVLockItem>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetFundsNAVLockDetails";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        NAVLockItem navLockItem = NAVLockItem.fillData(row, 0);

                        if (navLockItem != null && !accountNavLockDict.ContainsKey(navLockItem.AccountID))
                        {
                            accountNavLockDict.Add(navLockItem.AccountID, navLockItem);
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return accountNavLockDict;
        }
    }
}
