using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Prana.Global;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

using System.Data.Common;
using System.Data;
using Prana.Utilities.XMLUtilities;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;

namespace Prana.CentralSMDataCache.DAL
{
    internal class UDADataManager
    {

        //private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        /// <summary>
        /// Get symbols company Name
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string GetSymbolData(string symbol) 
        {
            //TODO find out use of this. -om
            // why we requesting uda data for  find company name of symbol -om.
            string companyName = string.Empty;
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[1];
            parameter[0] = symbol;
            try
            {

                object obj = db.ExecuteScalar("P_GetSymbolUdaData", parameter);
                if (obj != null)
                {
                    companyName = obj.ToString();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return companyName;
        }

        /// <summary>
        /// Save uda Attributes collection in DB.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="udaCollection"></param>
        public static void SaveInformation(string spName, UDACollection udaCollection)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(ApplicationConstants.SMConnectionString);
                foreach (UDA uda in udaCollection)
                {
                    object[] parameter = new object[2];
                    parameter[0] = uda.Name;
                    parameter[1] = uda.ID;
                    db.ExecuteNonQuery(spName, parameter);
                }
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
        }

        /// <summary>
        /// Delete UDA Attributes
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="deletedIDS"></param>
        public static void DeleteInformation(string spName, List<int> deletedIDS)
        {
            try
            {
                if (deletedIDS.Count.Equals(0))
                    return;
                Database db = DatabaseFactory.CreateDatabase(ApplicationConstants.SMConnectionString);
                foreach (int i in deletedIDS)
                {
                   
                    object[] parameter = new object[1];
                    parameter[0] = i;

                    try
                    {
                        db.ExecuteNonQuery(spName, parameter);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                }
                deletedIDS.Clear();
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
        }


        /// <summary>
        /// Get In use IDS
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        internal static Dictionary<int, string> GetInUseUDAIDs(string spName)
        {
            Database db = DatabaseFactory.CreateDatabase(ApplicationConstants.SMConnectionString);
            Dictionary<int, string> udaIDList = new Dictionary<int, string>();

            try
            {
                using (SqlDataReader dr = (SqlDataReader)db.ExecuteReader(spName))
                {
                    while (dr.Read())
                    {
                        object[] row = new object[dr.FieldCount];
                        dr.GetValues(row);
                        int udaID = Int32.Parse(row[0].ToString());
                        string udaName = row[1].ToString();
                        //Bharat Kumar Jangir (11 March 2014)
                        //UDA Id 0 can be possible
                        if (udaID >= 0)
                        {
                            udaIDList.Add(udaID, udaName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return udaIDList;
        }


        
        #region All UDA Attributs Data Key Value Pair
        /// <summary>
        /// get All UDA Assets
        /// </summary>
        /// <param name="SP Name"></param>
        /// <returns> All UDA Attributs Data Key Value Pair </returns>
        /// 

        public static Dictionary<int, string> GetUDAAttributeData(string spName)
        {
            Dictionary<int, string> dictUDAAttributeData = new Dictionary<int, string>();
            Database db = DatabaseFactory.CreateDatabase(ApplicationConstants.SMConnectionString);
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(spName))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dictUDAAttributeData.Add(int.Parse(row[1].ToString()), row[0].ToString());//(FillKeyValuePair(row, 0));

                    }
                }
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
            return dictUDAAttributeData;
        }


        #endregion


    }
}
