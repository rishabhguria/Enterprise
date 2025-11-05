using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.SecurityMasterNew.DAL
{
    internal class UDADataManager
    {

        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        /// <summary>
        /// Save uda Attributes collection in DB.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="udaCollection"></param>
        public static void SaveInformation(string spName, UDACollection udaCollection)
        {
            try
            {
                foreach (UDA uda in udaCollection)
                {
                    object[] parameter = new object[2];
                    parameter[0] = uda.Name;
                    parameter[1] = uda.ID;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter, ApplicationConstants.SMConnectionString);
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

                foreach (int i in deletedIDS)
                {

                    object[] parameter = new object[1];
                    parameter[0] = i;

                    try
                    {
                        DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter, ApplicationConstants.SMConnectionString);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

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
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = spName;

            Dictionary<int, string> udaIDList = new Dictionary<int, string>();

            try
            {
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData, ApplicationConstants.SMConnectionString))
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return udaIDList;
        }

        internal static DataSet GetAccountWiseUDAData()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetFundWiseUDAData";
                queryData.CommandTimeout = 900;
                DataSet data = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, ApplicationConstants.SMConnectionString);
                return data;

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


            return null;
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
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = spName;

            Dictionary<int, string> dictUDAAttributeData = new Dictionary<int, string>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData, ApplicationConstants.SMConnectionString))
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return dictUDAAttributeData;
        }


        #endregion


        /// <summary>
        /// Saving Dynamic UDA to DB
        /// </summary>
        /// <param name="dataAsXML"></param>
        /// <returns></returns>
        internal static int SaveAccountWiseUDADataInDB(String dataAsXML)
        {
            int affectedPositions = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveFundWiseUDAData";
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = dataAsXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                affectedPositions = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, ApplicationConstants.SMConnectionString);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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
            return affectedPositions;
        }

        /// <summary>
        /// Getting Dynamic UDA
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        internal static SerializableDictionary<string, DynamicUDA> GetDynamicUDA(string spName)
        {
            try
            {
                SerializableDictionary<string, DynamicUDA> dictDynamicUDAAttributeData = new SerializableDictionary<string, DynamicUDA>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;

                DataSet data = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, ApplicationConstants.SMConnectionString);
                if ((data.Tables[0].Rows.Count) > 0)
                {
                    foreach (DataRow dr in data.Tables[0].Rows)
                    {
                        string defaultValue = string.Empty;
                        if (!string.IsNullOrEmpty(dr["DefaultValue"].ToString()))
                            defaultValue = dr["DefaultValue"].ToString();
                        else
                            defaultValue = "Undefined";

                        DynamicUDA dynamicUDa = new DynamicUDA(dr["Tag"].ToString(), dr["HeaderCaption"].ToString(), defaultValue, dr["MasterValues"].ToString());

                        if (dictDynamicUDAAttributeData.ContainsKey(dr["Tag"].ToString()))
                        {
                            dictDynamicUDAAttributeData[dr["Tag"].ToString()] = dynamicUDa;
                        }
                        else
                        {
                            dictDynamicUDAAttributeData.Add(dr["Tag"].ToString(), dynamicUDa);
                        }
                    }
                }
                return dictDynamicUDAAttributeData;
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
                return null;
            }
        }

        /// <summary>
        /// Saving the Dynamic UDA
        /// </summary>
        /// <param name="dynamicUda"></param>
        /// <returns></returns>
        internal static bool SaveDynamicUDA(DynamicUDA dynamicUda, string renamedKeys)
        {
            bool saved;
            try
            {
                object[] parameter = new object[5];
                parameter[0] = dynamicUda.Tag.Trim();
                parameter[1] = dynamicUda.HeaderCaption.Trim();
                parameter[2] = dynamicUda.DefaultValue.Trim();
                parameter[3] = dynamicUda.SerializeToXML(dynamicUda.MasterValues);
                parameter[4] = renamedKeys;
                // Use P_UDA_SaveDynamicUDAAndUpdateView to save dynamic uda values and update dynmic UDA view, PRANA-10125
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UDA_SaveDynamicUDAAndUpdateView", parameter, ApplicationConstants.PranaConnectionString);
                saved = true;
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
                saved = false;
            }
            return saved;
        }

        /// <summary>
        /// To check master value is used
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool CheckMasterValueAssigned(string tag, string value)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = tag;
                parameter[1] = value;
                var value1 = DatabaseManager.DatabaseManager.ExecuteScalar("P_UDA_CheckMasterValueAssigned", parameter, ApplicationConstants.SMConnectionString);

                if (value1.Equals(1))
                    return true;
                else
                    return false;
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
    }
}
