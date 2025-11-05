using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.PostTradeServices.RollOver
{
    internal class ClearanceDataBaseManager
    {
        readonly static object _locker = new object();

        #region Singleton
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        private static ClearanceDataBaseManager _clearanceDataBaseManager;
        public static ClearanceDataBaseManager GetInstance
        {
            get
            {
                lock (_locker)
                {
                    if (_clearanceDataBaseManager == null)
                        _clearanceDataBaseManager = new ClearanceDataBaseManager();
                    return _clearanceDataBaseManager;
                }
            }
        }
        #endregion

        #region Clearance Data
        /// <summary>
        /// Get Clearance Data
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="dictRolloverPermittedAUEC"></param>
        /// <returns></returns>
        public GenericRepository<ClearanceData> GetClearanceData(int companyID, ref Dictionary<int, bool> dictRolloverPermittedAUEC)
        {
            GenericRepository<ClearanceData> clearanceCompleteData = new GenericRepository<ClearanceData>();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetBlotterClearanceForCompany", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        StringBuilder sb = null;
                        ClearanceData clearanceData = FillClearanceData(row, 0);

                        //Rollover permitted AUEC adding these in Dictionary
                        if (!dictRolloverPermittedAUEC.ContainsKey(clearanceData.AUECID))
                            dictRolloverPermittedAUEC.Add(clearanceData.AUECID, clearanceData.PermitRollover);

                        ClearanceData clearanceDataOld = clearanceCompleteData.GetItem(clearanceData.GetKey());
                        if (clearanceDataOld == null)
                        {
                            sb = new StringBuilder(clearanceData.AUECID.ToString());
                            sb.Append(",");
                            clearanceData.AUECIDStr = sb.ToString();
                            clearanceCompleteData.Add(clearanceData);
                        }
                        else
                        {
                            sb = new StringBuilder(clearanceDataOld.AUECIDStr);
                            sb.Append(clearanceData.AUECID.ToString());
                            sb.Append(",");
                            clearanceDataOld.AUECIDStr = sb.ToString();
                        }
                    }
                }
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
            return clearanceCompleteData;
        }

        /// <summary>
        /// Fill the clearance data
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private ClearanceData FillClearanceData(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ClearanceData _clearanceData = null;
            try
            {
                if (row != null)
                {
                    _clearanceData = new ClearanceData();

                    int AUEC = 0 + offset;
                    int AUECID = 1 + offset;
                    int STARTTIME = 2 + offset;
                    int ENDTIME = 3 + offset;
                    int CLEARANCETIME = 4 + offset;
                    int CLEARANCETIMEID = 5 + offset;
                    int COMPANYAUECID = 6 + offset;
                    int EXCHANGEIDENTIFIER = 7 + offset;
                    int PERMITROLLOVER = 8 + offset;

                    if (row != null)
                    {
                        _clearanceData.AUEC = Convert.ToString(row[AUEC]);
                        _clearanceData.AUECID = Convert.ToInt32(row[AUECID]);
                        _clearanceData.ExchangeRegularTradingStartTime = Convert.ToString(row[STARTTIME]);
                        _clearanceData.ExchangeRegularTradingEndTime = Convert.ToString(row[ENDTIME]);
                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-22503
                        _clearanceData.ClearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(Convert.ToDateTime(row[CLEARANCETIME]), CachedDataManager.GetInstance.GetAUECTimeZone(_clearanceData.AUECID));
                        _clearanceData.ClearanceTimeID = Convert.ToInt32(row[CLEARANCETIMEID]);
                        _clearanceData.CompanyAUECID = Convert.ToInt32(row[COMPANYAUECID]);
                        _clearanceData.ExchangeIdentifier = Convert.ToString(row[EXCHANGEIDENTIFIER]);
                        _clearanceData.PermitRollover = Convert.ToBoolean(row[PERMITROLLOVER]);
                    }
                }
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
            return _clearanceData;
        }

        /// <summary>
        /// Get Company Wise Clearance common data
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public BlotterClearanceCommonData GetCompanyClearanceCommonData(int companyID)
        {
            BlotterClearanceCommonData blotterClearanceCommonData = new BlotterClearanceCommonData();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClearanceCommonData", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value)
                        {
                            blotterClearanceCommonData.TimeZone = row[0].ToString();
                        }
                        if (row[1] != DBNull.Value)
                        {
                            blotterClearanceCommonData.AutoClearing = Convert.ToBoolean(row[1].ToString());
                        }
                        if (row[2] != DBNull.Value)
                        {
                            blotterClearanceCommonData.BaseTime = Convert.ToDateTime(row[2].ToString());
                        }
                        if (row[3] != DBNull.Value)
                        {
                            blotterClearanceCommonData.RolloverPermittedUserID = Convert.ToInt32(row[3].ToString());
                        }
                    }
                }
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
            return blotterClearanceCommonData;
        }
        #endregion
    }
}
