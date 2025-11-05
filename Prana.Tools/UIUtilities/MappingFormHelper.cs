using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.Tools
{
    class MappingFormHelper
    {
        static int _hashCode;
        static Boolean _isCheckSymbolValidationOnMapping = true;
        internal static void SetUp(int hashCode)
        {
            try
            {
                _hashCode = hashCode;
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2146
                _isCheckSymbolValidationOnMapping = Boolean.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsCheckSymbolValidationOnMapping"));
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
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        private static string GetPath(string filename, string startupPath)
        {
            string path = string.Empty;
            try
            {
                path = startupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconMappingXml.ToString() + @"\" + filename;
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
            return path;
        }
        internal static void SendSMRequest(string value)
        {
            try
            {
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2146
                if (!string.IsNullOrWhiteSpace(value) && _isCheckSymbolValidationOnMapping)
                {
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();
                    reqObj.AddData(value, ApplicationConstants.PranaSymbology);
                    reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    reqObj.HashCode = _hashCode;
                    NewUtilities.SecurityMaster.SendRequest(reqObj);
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
        }

        /// <summary>
        /// Save counter party on the fly
        /// </summary>
        /// <param name="counterParty"></param>
        internal static bool SaveCounterPartyVenueDetails(DataSet ds)
        {
            bool isNewCounterPartyAdded = false;
            try
            {
                var results = from DataRow myRow in ds.Tables[1].Rows
                              where myRow["MappedBrokerCode"].ToString() == "New"
                              select myRow;
                foreach (DataRow item in results)
                {
                    if (item.Table.Columns.Contains("NewBrokerCode") && !string.IsNullOrWhiteSpace(item["NewBrokerCode"].ToString()))
                    {
                        string counterPartyName = item["NewBrokerCode"].ToString();

                        object[] parameter = new object[3];
                        parameter[0] = counterPartyName;
                        parameter[1] = counterPartyName;
                        parameter[2] = 1;
                        DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCounterPartyOnTheFly", parameter);
                    }
                }
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    if (row.Table.Columns.Contains("MappedBrokerCode") && row["MappedBrokerCode"].ToString() == "New")
                    {
                        isNewCounterPartyAdded = true;
                        if (row.Table.Columns.Contains("NewBrokerCode"))
                        {
                            row["MappedBrokerCode"] = row["NewBrokerCode"];
                            row["NewBrokerCode"] = string.Empty;
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
            return isNewCounterPartyAdded;
        }

        /// <summary>
        /// added by: Bharat Raturi, 19 jun 2014
        /// Get the accounts valuelist according to third party mapping.
        /// Modified by Aman Seth, 28 Jan 2015
        /// CHMW-2472	[Client][MappingForm]Only Corresponding accounts should be displayed
        /// </summary>
        /// <returns></returns>
        internal static Infragistics.Win.IValueList GetAccountValueList(string thirdPartyName)
        {
            ValueList vlAccounts = new ValueList();
            try
            {
                if (!string.IsNullOrEmpty(thirdPartyName))
                {
                    int thirdPartyID = CachedDataManager.GetInstance.GetThirdPartyIDByFullName(thirdPartyName);
                    if (thirdPartyID != int.MinValue)
                    {
                        // Dictionary<int,string> dict=  CachedDataManager.GetInstance.GetUserAccountsAsDict();
                        Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                        List<int> permittedAccounts = CachedDataManager.GetInstance.GetThirdPartyAccounts(thirdPartyID);
                        foreach (int accountID in dictAccounts.Keys)
                        {
                            if (!string.IsNullOrWhiteSpace(dictAccounts[accountID].ToString()) && permittedAccounts.Contains(accountID))
                            {
                                vlAccounts.ValueListItems.Add(dictAccounts[accountID], dictAccounts[accountID]);
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
            return vlAccounts;
        }

        /// <summary>
        /// Get all accounts as ValueList.
        /// </summary>
        /// <returns></returns>
        internal static Infragistics.Win.IValueList GetAccountValueList()
        {
            try
            {
                ValueList vlAccounts = new ValueList();
                //vlAccounts.ValueListItems.Add(-1, "-Select-");
                Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetAccounts();
                foreach (int accountID in dictAccounts.Keys)
                {
                    if (!string.IsNullOrWhiteSpace(dictAccounts[accountID].ToString()))
                    {
                        vlAccounts.ValueListItems.Add(dictAccounts[accountID], dictAccounts[accountID]);
                    }
                }
                return vlAccounts;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the PranaImportTag valuelist
        /// </summary>
        /// <returns></returns>
        internal static IValueList GetPranaImportTagValueList()
        {
            ValueList vlPranaImportTag = new ValueList();
            try
            {
                //vlAccounts.ValueListItems.Add(-1, "-Select-");
                Dictionary<string, string> dictTags = CachedDataManager.GetInstance.GetPranaImportTags();
                foreach (string tag in dictTags.Keys)
                {
                    if (!string.IsNullOrWhiteSpace(dictTags[tag].ToString()))
                    {
                        vlPranaImportTag.ValueListItems.Add(tag, dictTags[tag]);
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
            return vlPranaImportTag;
        }

        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        internal static ValueList GetValueList<TKey>(Dictionary<TKey, string> dict)
        {
            ValueList coll = new ValueList();
            try
            {
                foreach (KeyValuePair<TKey, string> item in dict)
                {
                    coll.ValueListItems.Add(item.Key, item.Value);
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
            return coll;
        }

        ///// <summary>
        ///// modified by: sachin mishra,02 Feb 2015
        ///// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        ///// </summary>
        ///// <param name="dict"></param>
        ///// <returns></returns>
        //private ValueList GetValueList(Dictionary<int, string> dict)
        //{
        //    ValueList coll = new ValueList();
        //    try
        //    {
        //        foreach (KeyValuePair<int, string> item in dict)
        //        {
        //            coll.ValueListItems.Add(item.Key, item.Value);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return coll;
        //}

        internal static System.Data.DataTable GetFileListTable(string startupPath)
        {
            try
            {
                DataTable mappingFileData = new DataTable();
                mappingFileData.Columns.Add("DisplayName");
                mappingFileData.Columns.Add("Name");
                string path = startupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconMappingXml.ToString();
                DirectoryInfo dir = new DirectoryInfo(path);
                mappingFileData.Rows.Add(new object[] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                if (Directory.Exists(path))
                {
                    foreach (FileInfo f in dir.GetFiles("*.*"))
                    {
                        mappingFileData.Rows.Add(new object[] { f.Name.Substring(0, f.Name.Split('.')[0].Length), f.Name });
                    }
                }
                return mappingFileData;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }


        /// <summary>
        /// For a ch user bind all the third parties if they didn't exists
        /// </summary>
        /// <param name="dt"></param>
        internal static void BindAllThirdParties(DataTable dt)
        {
            // http://jira.nirvanasolutions.com:8080/browse/CHMW-3216
            //if (CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
            //{
            if (dt.Columns.Contains("Name"))
            {
                List<string> lstPBNames = dt.AsEnumerable().Select(dr => dr.Field<string>("Name")).ToList();
                //iterate for all the available third parties
                //here third party short name is used not the long name
                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetAllThirdParties())
                {
                    if (!lstPBNames.Contains(kvp.Value))
                    {
                        dt.Rows.Add(dt.Rows.Count + 1, kvp.Value);
                    }
                }
            }
            //}
        }

        internal static DataSet GetDataSet(string fileName, string startupPath)
        {
            DataSet ds = new DataSet();
            try
            {
                //Format of the XML file should be same for all files
                string path = GetPath(fileName, startupPath);
                //ds.ReadXml(path);
                ds = XMLUtilities.ReadXmlUsingBufferedStream(path);
                // Modified by Ankit Gupta on 7 Nov, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1779                    
                // Duplicate mapping was saving from mapping from, therefore, made the combination of columns PranaAccount, PBAccountCode, PBAccountName & PB_Id as Primary Key.					
                if (ds.Tables.Contains("FundData")
                    && ds.Tables["FundData"].Columns.Contains("PBAccountName")
                    && ds.Tables["FundData"].Columns.Contains("PBAccountCode")
                    && ds.Tables["FundData"].Columns.Contains("PranaAccount"))
                {
                    foreach (DataRow dRow in ds.Tables["AccountData"].Select("PBAccountName is null OR PBAccountCode is null OR PranaAccount is null"))
                    {
                        if (dRow["PBAccountName"] == DBNull.Value)
                            dRow["PBAccountName"] = "Value does not exist";

                        if (dRow["PBAccountCode"] == DBNull.Value)
                            dRow["PBAccountCode"] = "Value does not exist";

                        if (dRow["PranaAccount"] == DBNull.Value)
                            dRow["PranaAccount"] = "Value does not exist";
                    }
                    //ds.Tables["AccountData"].Columns["PBAccountName"].Expression = "ISNULL(PBAccountName,'Value does not exist')";
                    //ds.Tables["AccountData"].Columns["PBAccountCode"].Expression = "ISNULL(PBAccountCode,'Value does not exist')";
                    //Added by : sachin mishra 4 Feb 2015
                    //Purpose: for replaceing the null value to "Value does not exist"  http://jira.nirvanasolutions.com:8080/browse/CHMW-2443
                    DataColumn[] columns = new DataColumn[4];

                    ds.Tables["FundData"].Columns["PranaAccount"].DefaultValue = string.Empty;
                    columns[0] = ds.Tables["FundData"].Columns["PranaAccount"];

                    ds.Tables["FundData"].Columns["PBAccountName"].DefaultValue = string.Empty;
                    columns[1] = ds.Tables["FundData"].Columns["PBAccountName"];

                    ds.Tables["FundData"].Columns["PBAccountCode"].DefaultValue = string.Empty;
                    columns[2] = ds.Tables["FundData"].Columns["PBAccountCode"];

                    if (ds.Tables["FundData"].Columns.Contains("PB_Id"))
                    {
                        columns[3] = ds.Tables["FundData"].Columns["PB_Id"];
                    }
                    ds.Tables["FundData"].PrimaryKey = columns;
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
            return ds;
        }



        internal static void WriteDataSet(DataSet ds, string fileName, string startupPath)
        {
            try
            {
                ds.WriteXml(GetPath(fileName, startupPath));
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

        internal static IValueList GetAccountCollectionValueList()
        {
            ValueList coll = new ValueList();
            try
            {
                AccountCollection userAccounts = CachedDataManager.GetInstance.GetUserAccounts();
                foreach (Account account in userAccounts)
                {
                    coll.ValueListItems.Add(account.AccountID, account.Name);
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
            return coll;
        }

        internal static IValueList GetLongShortValueList()
        {
            ValueList vlLongOrShort = new ValueList();
            try
            {
                vlLongOrShort.ValueListItems.Add("Positive");
                vlLongOrShort.ValueListItems.Add("Negative");
                vlLongOrShort.SortStyle = ValueListSortStyle.Ascending;
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
            return vlLongOrShort;
        }

        internal static IValueList GetActivityValueList()
        {
            //Narendra Kumar Jangir, Sept 07, 2013
            //Here we don't use cached value list vlActivityType of cacheddatamanager because we need both key an value as ActivityType
            ValueList vlActivityType = new ValueList();
            try
            {
                foreach (ValueListItem item in ValueListHelper.GetInstance.GetCashTransactionTypeValueList().ValueListItems)
                {
                    vlActivityType.ValueListItems.Add(item.DisplayText);
                }
                //Arrange items of value list in ascending order.
                vlActivityType.SortStyle = ValueListSortStyle.Ascending;
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
            return vlActivityType;
        }

        internal static Dictionary<string, string> GetAllOrderSides()
        {
            try
            {
                return TagDatabaseManager.GetInstance.GetAllOrderSides();
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
            return new Dictionary<string, string>();
        }

        internal static IValueList GetCounterPartyValueList(bool isAddNewValue)
        {
            ValueList vlCounterParties = new ValueList();
            try
            {
                Dictionary<int, string> dictCounterParties = CachedDataManager.GetInstance.GetAllCounterParties();
                vlCounterParties = GetValueList<int>(dictCounterParties);
                if (isAddNewValue)
                {
                    vlCounterParties.ValueListItems.Add("New", "New");
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
            return vlCounterParties;
        }

        internal static IValueList GetAssetValueList()
        {
            ValueList vlAssets = new ValueList();
            try
            {
                Dictionary<int, string> dictAssets = CachedDataManager.GetInstance.GetAllAssets();
                vlAssets = GetValueList<int>(dictAssets);
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
            return vlAssets;
        }
    }
}