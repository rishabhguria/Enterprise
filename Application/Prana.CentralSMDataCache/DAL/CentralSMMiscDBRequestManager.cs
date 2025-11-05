using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Prana.CentralSMDataCache.DAL
{
    sealed class CentralSMMiscDBRequestManager
    {
        #region Singleton

        private static volatile CentralSMMiscDBRequestManager instance;
        private static object syncRoot = new Object();

        private CentralSMMiscDBRequestManager() { }

        public static CentralSMMiscDBRequestManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CentralSMMiscDBRequestManager();
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// Gets the prana Preferences from DB and stores it in the cache
        /// </summary>
        /// <returns></returns>
        public DataSet GetPranaPreference()
        {
            DataSet ds = new DataSet();
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                string spName = "P_GetPranaPreferences";
                ds = db.ExecuteDataSet(spName);
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return ds;
        }

        internal DataSet GetPranaKeyValuePairsSM()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DataSet keyvalues = new DataSet();
            try
            {
                keyvalues = db.ExecuteDataSet("P_GetKeyValuePairSM");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return keyvalues;
        }

        /// <summary>
        /// Gets the pricing data from DB for specific symbolpk,date,datasource and fields
        /// </summary>
        /// <param name="xmlParam1"></param>
        /// <param name="commaSeparatedFields"></param>
        /// <returns></returns>
        public DataSet GetSecurityPricingData(string xmlParam1, string commaSeparatedFields)
        {
            DataSet ds = new DataSet();
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                object[] parameter = new object[2];

                parameter[0] = xmlParam1;
                parameter[1] = commaSeparatedFields;

                string spName = "P_GetSecurityPricingData";

                ds = db.ExecuteDataSet(spName, parameter);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }

        public static void SaveGenericPricingData(string pricingDataXmlString)
        {
            int affectedPositions = 0;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                object[] parameter = new object[3];

                parameter[0] = pricingDataXmlString;
                parameter[1] = string.Empty;
                parameter[2] = 0;

                string spName = "P_SaveSM_PricingData";
                affectedPositions = db.ExecuteNonQuery(spName, parameter);

            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            #endregion
        }
        
    }
}
