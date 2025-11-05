using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.BusinessObjects.TextSearch.SymbolSearch;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.SecurityMasterNew.BLL;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace Prana.SecurityMasterNew
{
    public class SecMasterDataManager
    {
        public SecMasterDataManager()
        {
        }

        private const string CONST_connStringName = "SMConnectionString";
        private const string CONST_ClientConnStringName = "PranaConnectionString";
        private static int _heavySaveTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavySaveTimeout"]);
        public static List<SecMasterBaseObj> GetSecMasterDataFromDB_XML(SecMasterRequestObj secMasterRequestObj)
        {
            List<SecMasterBaseObj> secMasterObjList = new List<SecMasterBaseObj>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SMGetSecurityMasterData_XML";
                object[] parameter = new object[1];

                parameter[0] = secMasterRequestObj.CreateXml();
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.String,
                    ParameterValue = parameter[0]
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData, CONST_connStringName))
                {
                    while (reader.Read())
                    {
                        SecMasterBaseObj secMasterObj = GetSecMasterObj(reader);
                        if (secMasterObj != null)
                        {
                            secMasterObjList.Add(secMasterObj);
                        }
                    }
                }
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
            return secMasterObjList;
        }

        public static List<SecMasterBaseObj> GetSecMasterDataFromDB_XML(SecMasterRequestObj secMasterRequestObj, string connString)
        {
            List<SecMasterBaseObj> secMasterObjList = new List<SecMasterBaseObj>();
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("P_SMGetSecurityMasterData_XML", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@xml", secMasterRequestObj.CreateXml());
                cmd.CommandTimeout = 300;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SecMasterBaseObj secMasterObj = GetSecMasterObj(reader);
                    if (secMasterObj != null)
                    {
                        secMasterObjList.Add(secMasterObj);
                    }
                }
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
            return secMasterObjList;
        }

        public static SecMasterBaseObj GetSecMasterObj(IDataReader reader)
        {
            SecMasterBaseObj secMasterObj = null;
            try
            {
                object[] row = new object[reader.FieldCount];
                reader.GetValues(row);
                int AssetId = int.Parse(reader["AssetID"].ToString());
                secMasterObj = SecurityMasterFactory.GetSecmasterObject((AssetCategory)AssetId);
                int offset = 9; // 6 basic data elements coming + symbology codes are coming 
                offset = offset + ApplicationConstants.SymbologyCodesCount;
                if (secMasterObj != null)
                {
                    switch ((AssetCategory)AssetId)
                    {
                        case AssetCategory.FXForward:
                            offset += 13;
                            secMasterObj.FillData(row, offset);
                            break;
                        default:
                            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)AssetId);
                            if (secMasterObj != null)
                            {

                                switch (baseAssetCategory)
                                {
                                    case AssetCategory.Equity:
                                        secMasterObj.FillData(row, offset);
                                        break;
                                    case AssetCategory.Option:
                                        offset += 1; // 1 equity element before
                                        secMasterObj.FillData(row, offset);

                                        break;
                                    case AssetCategory.Future:
                                        offset += 6; // 5 option elements are there before.
                                        secMasterObj.FillData(row, offset);
                                        break;
                                    case AssetCategory.FX:
                                        offset += 10; // 3 future elements are there before.
                                        secMasterObj.FillData(row, offset);
                                        break;

                                    case AssetCategory.FixedIncome:
                                    case AssetCategory.ConvertibleBond:
                                        offset += 19;
                                        secMasterObj.FillData(row, offset);
                                        break;
                                    case AssetCategory.Indices:
                                        offset += 7;
                                        secMasterObj.FillData(row, offset);
                                        break;

                                }
                            }
                            break;
                    }
                    // Update Risk Currency automatically
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-9214
                    if (secMasterObj.DynamicUDA.ContainsKey("RiskCurrency"))
                        secMasterObj.DynamicUDA["RiskCurrency"] = GetRiskCurrency(secMasterObj);
                    else
                        secMasterObj.DynamicUDA.Add("RiskCurrency", GetRiskCurrency(secMasterObj));

                    // Issuer update has been moved from Sp [P_SMGetSecurityMasterData_XML ] to code here as in SP it was taking too much time in looping through
                    // JIRA: https://jira.nirvanasolutions.com:8443/browse/PRANA-18276
                    if (!secMasterObj.DynamicUDA.ContainsKey("Issuer"))
                        secMasterObj.DynamicUDA.Add("Issuer", secMasterObj.LongName);
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
            return secMasterObj;
        }

        /// <summary>
        /// Get value of risk currency
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <returns></returns>
        public static string GetRiskCurrency(SecMasterBaseObj secMasterObj)
        {
            try
            {
                int baseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                string riskCurrency = string.Empty;
                if (secMasterObj.AssetCategory == AssetCategory.FX)
                {
                    SecMasterFxObj obj = (SecMasterFxObj)secMasterObj;
                    if (obj.VsCurrencyID == baseCurrency)
                        riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(obj.LeadCurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[obj.LeadCurrencyID] : string.Empty;
                    else
                        riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(obj.VsCurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[obj.VsCurrencyID] : string.Empty;
                }
                else if (secMasterObj.AssetCategory == AssetCategory.FXForward)
                {
                    SecMasterFXForwardObj obj = (SecMasterFXForwardObj)secMasterObj;
                    if (obj.VsCurrencyID == baseCurrency)
                        riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(obj.LeadCurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[obj.LeadCurrencyID] : string.Empty;
                    else
                        riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(obj.VsCurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[obj.VsCurrencyID] : string.Empty;
                }
                else
                    riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(secMasterObj.CurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[secMasterObj.CurrencyID] : string.Empty;

                return riskCurrency;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        public static DataSet GetDetails(string symbol, string companyName)
        {
            DataSet ds = new DataSet();

            try
            {
                object[] parameter = new object[2];

                parameter[0] = symbol.Equals(string.Empty) ? null : symbol;
                parameter[1] = companyName.Equals(string.Empty) ? null : companyName;

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetDetails", parameter, CONST_connStringName);

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

            return ds;
        }

        /// <summary>
        /// Gets the pricing data from DB for specific symbolpk,date,datasource and fields
        /// </summary>
        /// <param name="xmlParam1"></param>
        /// <param name="commaSeparatedFields"></param>
        /// <returns></returns>
        public static DataSet GetSecurityPricingData(string xmlParam1, string commaSeparatedFields)
        {
            DataSet ds = new DataSet();
            try
            {
                object[] parameter = new object[2];

                parameter[0] = xmlParam1;
                parameter[1] = commaSeparatedFields;

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetSecurityPricingData", parameter, CONST_connStringName);
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
            return ds;
        }

        public static int GetUnExpiredOptionCountForUnderlying(string underlyingSymbol, DateTime effectiveDate)
        {
            int optionCount = 0;

            try
            {
                object[] parameter = new object[2];

                parameter[0] = underlyingSymbol;
                parameter[1] = effectiveDate;

                optionCount = (int)DatabaseManager.DatabaseManager.ExecuteScalar("P_GetUnExpiredOptionCountForUnderlying", parameter, CONST_connStringName);

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

            return optionCount;
        }

        /// <summary>
        /// Saves the Symbol Name change along with the company name change
        /// </summary>
        /// <param name="corporateActionList"></param>
        /// <returns></returns>
        public static int SaveCorpActionWithSymbolAndCompanyNameChange(string corporateActionList, Int64 symbol_PK)
        {
            int affectedPositions = 0;

            try
            {
                object[] parameter = new object[4];

                parameter[0] = corporateActionList;
                parameter[1] = string.Empty;
                parameter[2] = 0;
                parameter[3] = symbol_PK;
                //parameter[4] = optionSymbolPKs; //For which we need to change the underlying symbol name

                affectedPositions = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCorpActionWithSymbolAndCompNameChange", parameter, CONST_connStringName);

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

            return affectedPositions;
        }

        /// <summary>
        /// Returns full corporate action data.
        /// </summary>
        /// <returns></returns>
        public static DataSet GetFullCAData()
        {
            DataSet ds = new DataSet();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SMGetFullCAData";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, CONST_connStringName);
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

            return ds;
        }

        public static int ConvertFixToXmlFormatInDb(string convertedXMLStr)
        {
            int rowsAffected = 0;
            try
            {
                object[] parameter = new object[3];

                parameter[0] = convertedXMLStr;
                parameter[1] = String.Empty;
                parameter[2] = 0;

                string spName = "P_SMConvertFixToXmlFormat";

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter, CONST_connStringName);

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            #endregion

            return rowsAffected;
        }

        public static DataSet GetAllCorporateActions(CorporateActionType caType, DateTime fromDate, DateTime toDate, bool isApplied)
        {
            DataSet ds = new DataSet();

            try
            {
                object[] parameter = new object[4];
                parameter[0] = Convert.ToInt32(caType);
                parameter[1] = fromDate.ToString();
                parameter[2] = toDate.ToString();
                parameter[3] = isApplied;

                string spName = "P_SMGetAllCorporateActions";
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter, CONST_connStringName);
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

            return ds;
        }

        public static DataSet getLatestCorpAction(string caSymbols)
        {
            DataSet ds = new DataSet();
            try
            {
                if (!string.IsNullOrWhiteSpace(caSymbols))
                {
                    object[] parameter = new object[1];
                    parameter[0] = caSymbols.ToString();

                    ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_getLatestCorporateActionSymbolWise", parameter, CONST_connStringName);
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
            return ds;
        }

        public static int UndoNameChange(string caIds)
        {
            int rowsAffected = 0;
            try
            {
                object[] parameter = new object[3];

                parameter[0] = caIds;
                parameter[1] = string.Empty;
                parameter[2] = 0;

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SMUndoNameChange", parameter, CONST_connStringName);
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            #endregion

            return rowsAffected;
        }

        public static int UndoCA(string caIds)
        {
            int rowsAffected = 0;
            try
            {
                object[] parameter = new object[3];

                parameter[0] = caIds;
                parameter[1] = string.Empty;
                parameter[2] = 0;

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SMUndoCA", parameter, CONST_connStringName);

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            #endregion

            return rowsAffected;
        }

        public static int SaveCorporateAction(string corporateActionListString, bool isApplied)
        {
            int affectedPositions = 0;

            try
            {
                object[] parameter = new object[4];

                parameter[0] = corporateActionListString;
                if (isApplied)
                {
                    parameter[1] = 1;
                }
                else
                {
                    parameter[1] = 0;
                }
                parameter[2] = string.Empty;
                parameter[3] = 0;

                affectedPositions = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCorpActionInfo", parameter, CONST_connStringName);
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

            return affectedPositions;
        }

        public static int DeleteCorproateActions(string caIDs)
        {
            int affectedrecords = 0;

            try
            {
                object[] parameter = new object[3];

                parameter[0] = caIDs;
                parameter[1] = string.Empty;
                parameter[2] = 0;

                //string spName = "P_SaveCorporateAction"; 
                string spName = "P_DeleteSelectedCorporateAction";

                affectedrecords = DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter, CONST_connStringName);

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

            return affectedrecords;
        }

        public static string GetOldCompanyNameForNameChange(Guid CAId)
        {
            string oldCompanyName = string.Empty;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = CAId;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_SMGetOldCompanyNameForCompanyNameChangeCA", parameter, CONST_connStringName))
                {
                    while (reader.Read())
                    {
                        oldCompanyName = reader["CompanyName"].ToString();
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

            return oldCompanyName;
        }

        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        /// <summary>
        /// to check if process to save symbol is in process
        /// </summary>
        private static readonly object _isSaving = new object();

        internal static void SaveNewSymbolResponsetoSecurityMaster(string secMasterXml, string connString, bool IsAutoUpdateDerivateUDA)
        {
            try
            {
                // added lock to avoid deadlock condition if request is raised to save symbol from symbol look up and live feed simultaneously, PRANA-9838
                lock (_isSaving)
                {
                    if (connString == string.Empty)
                    {
                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_SaveSecurityMasterDataForSymbol";
                        queryData.CommandTimeout = _heavySaveTimeout;

                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = secMasterXml
                        });
                        queryData.DictionaryDatabaseParameter.Add("@IsAutoUpdateDerivateUDA", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@IsAutoUpdateDerivateUDA",
                            ParameterType = DbType.Boolean,
                            ParameterValue = IsAutoUpdateDerivateUDA
                        });

                        //db.AddInParameter(cmd, "@dataSource", DbType.Int16, dataSource);

                        XMLSaveManager.AddOutErrorParameters(queryData);
                        DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, CONST_connStringName);
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }
                    else
                    {
                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_SaveSecurityMasterDataForSymbol";
                        queryData.CommandTimeout = 300;
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = secMasterXml
                        });
                        queryData.DictionaryDatabaseParameter.Add("@IsAutoUpdateDerivateUDA", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@IsAutoUpdateDerivateUDA",
                            ParameterType = DbType.Boolean,
                            ParameterValue = IsAutoUpdateDerivateUDA
                        });

                        //cmd.Parameters.AddWithValue("@dataSource", dataSource);
                        XMLSaveManager.AddOutErrorParameters(queryData);
                        DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, connString);
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), LoggingConstants.POLICY_LOGANDSHOW);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        internal static void SaveNewSymbolResponsetoSecurityMaster_Import(string secMasterXml, int dataSource)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveSecurityMasterDataForSymbol_Import";
                queryData.CommandTimeout = _heavySaveTimeout;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = secMasterXml
                });
                queryData.DictionaryDatabaseParameter.Add("@dataSource", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@dataSource",
                    ParameterType = DbType.Int16,
                    ParameterValue = dataSource
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, CONST_connStringName);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);

            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), LoggingConstants.POLICY_LOGANDSHOW);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        internal static void UpdateSymbolToSecurityMaster_Import(string secMasterXml, int dataSource)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_UpdateSecurityMasterDataForSymbol_Import";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = secMasterXml
                });
                queryData.DictionaryDatabaseParameter.Add("@dataSource", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@dataSource",
                    ParameterType = DbType.Int16,
                    ParameterValue = dataSource
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, CONST_connStringName);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), LoggingConstants.POLICY_LOGANDSHOW);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Get SymbolLookup Requested Data  normal filter
        /// </summary>
        /// <param name="symbollookXml"></param>
        /// <returns></returns>
        public static DataSet GetSymbolLookupRequestedData(string symbollookXml)
        {
            DataSet ds = new DataSet();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = symbollookXml;

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_SearchSecMaster_New", parameter, CONST_connStringName);

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

            return ds;
        }

        /// <summary>
        /// Get Future Root Data from DB
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static DataSet GetFutureRootData(String connString)
        {
            //System.Collections.Generic.Dictionary<string, FutureRootData> ContractMultipliers
            //    = new Dictionary<string, FutureRootData>();
            if (connString != string.Empty) // for Automation with Connection String 
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetContractMultipliers";

                DataSet ds = new DataSet();
                try
                {
                    ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, connString);
                }
                catch (Exception)
                {
                    throw;
                }
                return ds;
            }
            else
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetContractMultipliers";

                try
                {
                    DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "SMConnectionString");
                    return ds;
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
                return null;
            }
        }

        /// <summary>
        /// Save Future Root Data to DB
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string SaveFutureRootData(DataTable dt)
        {
            string secMasterXml = string.Empty;
            try
            {
                MemoryStream stream = new MemoryStream();
                dt.WriteXml(stream);

                byte[] bytes = stream.ToArray();
                secMasterXml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveFutureRootData";
                queryData.CommandTimeout = _heavySaveTimeout;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = secMasterXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, CONST_connStringName);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);

                return _errorMessage;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), LoggingConstants.POLICY_LOGANDSHOW);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _errorMessage;
            #endregion
        }

        public static SecMasterGlobalPreferences GetPreferencesFromDB()
        {
            SecMasterGlobalPreferences preferences = new SecMasterGlobalPreferences();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSMPreferences";

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, CONST_connStringName);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    bool isCutOffTime = Convert.ToBoolean(row["useCutOffTime"].ToString());
                    preferences.UseCutOffTime = isCutOffTime;
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
            return preferences;
        }

        public static void SavePreferencesintoDB(SecMasterGlobalPreferences preferences)
        {

            try
            {
                object[] parameter = new object[1];
                parameter[0] = preferences.UseCutOffTime;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveSMPreferences", parameter, CONST_connStringName);

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

        internal static DataSet GetUnderLyingSymbolDetails(string UnderLyingSymbol, string connString)
        {
            DataSet ds = new DataSet();

            try
            {
                if (connString == string.Empty)
                {
                    object[] parameter = new object[1];

                    parameter[0] = UnderLyingSymbol;

                    ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_SearchUnderLyingSymbol", parameter, CONST_connStringName);
                }
                else
                {
                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_SearchUnderLyingSymbol";
                    queryData.DictionaryDatabaseParameter.Add("@UnderLyingSymbol", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@UnderLyingSymbol",
                        ParameterType = DbType.String,
                        ParameterValue = UnderLyingSymbol
                    });

                    try
                    {
                        ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, connString);
                    }
                    catch (Exception)
                    {
                        throw;
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

            return ds;
        }

        public static int UpdateCorporateAction(string corporateActionListString, bool isApplied)
        {
            int affectedPositions = 0;

            try
            {
                object[] parameter = new object[4];

                parameter[0] = corporateActionListString;
                if (isApplied)
                {
                    parameter[1] = 1;
                }
                else
                {
                    parameter[1] = 0;
                }
                parameter[2] = string.Empty;
                parameter[3] = 0;

                affectedPositions = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateCorpActionInfo", parameter, CONST_connStringName);

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

            return affectedPositions;
        }

        /// <summary>
        /// Get all open or historicaly traded symbols
        /// created by: Omshiv, Nov 2013
        /// </summary>
        /// <param name="isOpenSymbolsReq"></param>
        /// <returns></returns>
        internal static SecMasterRequestObj GetHistOrOpenTradedSymbols(Boolean isOpenSymbolsReq)
        {
            object[] parameter = new object[1];
            parameter[0] = isOpenSymbolsReq;

            SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetHistOrOpenTradedSymbols", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int taxlotIdIndex = 0;
                            if (row[taxlotIdIndex] != System.DBNull.Value)
                            {
                                secMasterReqobj.AddData(row[taxlotIdIndex].ToString(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                            }
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

            return secMasterReqobj;
        }

        /// <summary>
        /// Get  Advncd Search Data frm DB
        /// </summary>
        /// <param name="queuryString"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        internal static DataSet GetAdvncdSearchDatafrmDB(string queuryString, int startIndex, int endIndex)
        {
            DataSet ds = new DataSet();

            try
            {
                object[] parameter = new object[3];
                parameter[0] = queuryString;
                parameter[1] = startIndex;
                parameter[2] = endIndex;

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_AdvancedSearchSecMaster", parameter, CONST_connStringName);
            }

            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Error in Advanced Search Query: " + queuryString, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);


                if (rethrow)
                {
                    throw;
                }
                return ds;
            }

            return ds;
        }

        internal static void SaveGenericPricingData(string pricingDataXmlString)
        {

            try
            {
                object[] parameter = new object[3];

                parameter[0] = pricingDataXmlString;
                parameter[1] = string.Empty;
                parameter[2] = 0;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveSM_PricingData", parameter, CONST_connStringName);

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
        }

        /// <summary>
        /// returns dictionary of all pricing Fields
        /// </summary>
        /// <returns></returns>
        public static ConcurrentDictionary<string, StructPricingField> GetPricingFields()
        {
            ConcurrentDictionary<string, StructPricingField> PricingFields = new ConcurrentDictionary<string, StructPricingField>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFields";
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData, "SMConnectionString"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        StructPricingField pricingField = new StructPricingField();
                        if (row[0] != System.DBNull.Value)
                        {
                            int fieldID;
                            if (int.TryParse(row[0].ToString(), out fieldID))
                            {
                                pricingField.FieldID = fieldID;
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("FieldID not integer in database", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                            }
                        }
                        if (row[1] != System.DBNull.Value)
                        {
                            pricingField.FieldName = row[1].ToString();
                        }
                        if (row[2] != System.DBNull.Value)
                        {
                            pricingField.IsRealTime = Convert.ToBoolean(row[2].ToString());
                        }
                        if (row[3] != System.DBNull.Value)
                        {
                            pricingField.IsHistorical = Convert.ToBoolean(row[3].ToString());
                        }
                        if (row[4] != System.DBNull.Value)
                        {
                            pricingField.Esignal = row[4].ToString();
                        }
                        if (row[5] != System.DBNull.Value)
                        {
                            pricingField.Bloomberg = row[5].ToString();
                        }

                        PricingFields.TryAdd(row[1].ToString(), pricingField);
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
            return PricingFields;
        }

        /// <summary>
        /// Bharat Jangir (22 September, 2014)
        /// Saving AUEC Mappings for Option & Portfolio Science symbols auto generation
        /// </summary>
        /// <param name="saveDataSetTemp"></param>
        /// <returns>number of rows affected</returns>
        public static int SaveAUECMappings(DataSet saveDataSetTemp)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveAUECMapping";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.Xml,
                    ParameterValue = saveDataSetTemp.GetXml()
                });

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
            return rowsAffected;
        }

        public static DataSet GetAUECMappings()
        {
            DataSet dsAUECMapping = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAUECMapping";
                queryData.CommandTimeout = 200;

                dsAUECMapping = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return dsAUECMapping;
        }

        public static SecMasterSymbolCache[] getSymbolCache()
        {
            //SecMasterSymbolCache [] cache = {new SecMasterSymbolCache(), new SecMasterSymbolCache()};
            SecMasterSymbolCache[] cache = { new SecMasterSymbolCache() };
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllSymbol";

                int count = 0;
                List<SymbolSearchDataModel> tickerSymbolDataList = new List<SymbolSearchDataModel>();
                SymbolSearchDataModel symbolDataModel = null;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData, CONST_connStringName))
                {
                    //while (reader.Read() && count < 1000)
                    while (reader.Read())
                    {
                        string symbol = reader.GetString(0);
                        //string columnName = reader.GetName(0);

                        symbolDataModel = new SymbolSearchDataModel();
                        symbolDataModel.TickerSymbol = symbol;

                        //cache[0].lucene_fillData(symbol, columnName);
                        //cache[0].fillData(symbol);
                        if (!reader.IsDBNull(1))
                        {
                            symbol = reader.GetString(1);
                            //columnName = reader.GetName(1);
                            symbolDataModel.BloombergSymbol = symbol;
                            if (symbol != null && symbol.Length > 0)
                            {
                                //cache[1].fillData(symbol);
                                //cache[1].lucene_fillData(symbol, columnName);
                            }
                        }
                        else
                            symbolDataModel.BloombergSymbol = "";

                        if (!reader.IsDBNull(2))
                        {
                            symbol = reader.GetString(2);
                            symbolDataModel.FactSetSymbol = symbol;
                        }
                        else
                            symbolDataModel.FactSetSymbol = "";

                        if (!reader.IsDBNull(3))
                        {
                            symbol = reader.GetString(3);
                            symbolDataModel.ActivSymbol = symbol;
                        }
                        else
                            symbolDataModel.ActivSymbol = "";

                        tickerSymbolDataList.Add(symbolDataModel);
                        count++;
                    }
                }

                cache[0].symbolFillDataList(tickerSymbolDataList);
                //cache[1].symbolFillDataList(tickerSymbolDataList);

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
            return cache;
        }
    }
}

