using Prana.CommonDataCache.DAL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.CommonDataCache.Cache_Classes
{
    class HeatMapCacheData
    {
        private static HeatMapCacheData _cachedData = null;

        public static HeatMapCacheData GetInstance()
        {
            return _cachedData;
        }
        /// <summary>
        /// Static Constructor
        /// </summary>
        static HeatMapCacheData()
        {
            try
            {
                _cachedData = new HeatMapCacheData();
                _companyID = CachedDataManager.GetInstance.GetCompanyID();
                //load permissions of HeatMap check and override permission for a particular user in separate dictionary
                _cachedData.LoadHeatMapModuleEnabled(_companyID);
                if (_cachedData.GetHeatMapModuleEnabledForCompany())
                {
                    _cachedData.LoadHeatMapPermissions();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }

        /// <summary>
        /// Used to avoid multithread access when _heatMap dictionary are accessed 
        /// </summary>
        readonly Object _lockerObject = new object();

        /// <summary>
        /// Stores true or false company-wise if HeatMap module is enabled or not
        /// </summary>
        Dictionary<int, bool> _heatMap = new Dictionary<int, bool>();

        /// <summary>
        /// This Dictionary Contain Information Whether the user have permission of read or read write both for HeatMap module 
        /// </summary>
        readonly Dictionary<int, bool> _heatMapModule = new Dictionary<int, bool>();
        /// <summary>
        /// Getting HeatMap Module Enabled for Company or not
        /// </summary>
        /// <returns> Company Permission </returns>
        internal bool GetHeatMapModuleEnabledForCompany()
        {
            try
            {
                lock (_lockerObject)
                {
                    if ((_heatMap.ContainsKey(_companyID) && _heatMap[_companyID]))
                    {
                        return true;
                    }
                    else
                        return false;
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
                return false;
            }
        }

        private static int _companyID;

        public static int CompanyID
        {
            get { return _companyID; }
        }

        /// <summary>
        /// Return permission of HeatMap module Enabled for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user have enabled of Heatmap module</returns>
        internal bool GetHeatMapModuleEnabledForUser(int companyUserId)
        {
            try
            {
                lock (_heatMapModule)
                {
                    if (_heatMapModule.ContainsKey(companyUserId))
                        return true;
                    else
                        return false;
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
                return false;
            }
        }

        /// <summary>
        /// Load Company Permission in dictionary
        /// </summary>
        /// <param name="_companyID">Company Id</param>
        private void LoadHeatMapModuleEnabled(int _companyID)
        {
            try
            {
                if (!_heatMap.ContainsKey(_companyID))
                    _heatMap.Add(_companyID, HeatMapPermission.GetInstance().CheckHeatMapEnabled(_companyID));
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
        }

        /// <summary>
        /// load Read write permission for a User  of HeatMap Module.
        /// </summary>
        private void LoadHeatMapPermissions()
        {
            try
            {


                DataSet ds = HeatMapPermission.GetInstance().GetHeatMapModulePermission(_companyID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int companyUserId = Convert.ToInt32(dr["CompanyUserId"]);
                    String moduleName = (dr["ModuleName"].ToString());
                    bool read_WriteId = Convert.ToBoolean(dr["ReadWriteId"]);
                    if (moduleName == "HeatMap")
                    {
                        if (_heatMapModule.ContainsKey(companyUserId))
                        {
                            _heatMapModule[companyUserId] = read_WriteId;
                        }
                        else
                        {
                            _heatMapModule.Add(companyUserId, read_WriteId);
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


        }

    }
}
