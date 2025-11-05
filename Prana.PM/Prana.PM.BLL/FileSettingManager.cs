using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.PM.BLL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.PM.BLL
{
    public class FileSettingManager
    {

        #region Dictionaries

        /// <summary>
        /// Dictionary of file settings
        /// </summary>
        public static Dictionary<int, FileSettingItem> dictFileSetting = new Dictionary<int, FileSettingItem>();

        /// <summary>
        /// Dictionary for holding the ThirdPartyID-[AccountID-AccountName] pair
        /// </summary>
        public static Dictionary<int, Dictionary<int, string>> dictAccount = new Dictionary<int, Dictionary<int, string>>();

        public static DataTable dtAccounts = new DataTable();

        /// <summary>
        /// Dictionary for holding the FormatID-Format Name pair 
        /// </summary>
        public static Dictionary<int, string> dictImport = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for holding the FormatTypeID-FormatType Name pair 
        /// </summary>
        public static Dictionary<int, string> dictFormatType = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for holding the FormatID-Format Acronym pair 
        /// </summary>
        private static Dictionary<int, string> _dictImportAcronym = new Dictionary<int, string>();

        public static Dictionary<int, string> DictImportAcronym
        {
            get { return FileSettingManager._dictImportAcronym; }
            set { FileSettingManager._dictImportAcronym = value; }
        }

        /// <summary>
        /// Dictionary for holding the ReleaseID-ReleaseName pair 
        /// </summary>
        public static Dictionary<int, string> dictRelease = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for holding the Ftp ID-FTP Name pair 
        /// </summary>
        public static Dictionary<int, string> dictFtp = new Dictionary<int, string>();

        /// <summary>
        /// List of emails that the messaegs will be sent to
        /// </summary>
        public static Dictionary<int, string> dictSendMail = new Dictionary<int, string>();

        /// <summary>
        /// List of Emails that the logs will be sent to
        /// </summary>
        public static Dictionary<int, string> dictLogMail = new Dictionary<int, string>();

        /// <summary>
        /// List of decryptions
        /// </summary>
        public static Dictionary<int, string> dictDecryption = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary of releaseID-[Client List] pair
        /// </summary>
        public static Dictionary<int, Dictionary<int, string>> dictClient = new Dictionary<int, Dictionary<int, string>>();

        /// <summary>
        /// Dictionary of releaseName-CommanyReleaseDetail pair
        /// </summary>
        private static Dictionary<string, CompanyReleaseDetail> _dictCommanyDetails = new Dictionary<string, CompanyReleaseDetail>();

        public static Dictionary<string, CompanyReleaseDetail> DictCommanyDetails
        {
            get { return FileSettingManager._dictCommanyDetails; }
            set { FileSettingManager._dictCommanyDetails = value; }
        }

        #endregion

        /// <summary>
        /// checkl if there are blank rules
        /// </summary>
        /// <param name="dt">Datasource of the grid</param>
        /// <returns>True if there are empty rules </returns>
        public static bool HasEmpty(DataTable dt, int thirdpartyID)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["ThirdPartyID"] = thirdpartyID;
                    if (string.IsNullOrEmpty(row["FormatName"].ToString().Trim()) || string.IsNullOrEmpty(row["ImportTypeID"].ToString()) ||
                            string.IsNullOrEmpty(row["AccountID"].ToString()) || string.IsNullOrEmpty(row["BatchStartDate"].ToString()) || string.IsNullOrEmpty(row["ClientID"].ToString())
                            || string.IsNullOrEmpty(row["FormatType"].ToString()))
                    {
                        return true;
                    }
                    if (!row["FormatType"].Equals("0") && !row["FormatType"].Equals("1") && !row["FormatType"].Equals("2"))
                    {
                        return true;
                    }
                    if (((List<object>)row["AccountID"]).Count <= 0)
                    {
                        return true;
                    }
                    if (((List<object>)row["ClientID"]).Count <= 0)
                    {
                        return true;
                    }
                    if (dt.Columns.Contains("XSLTPath") && string.IsNullOrEmpty(row["XSLTPath"].ToString()))
                    {
                        return true;
                    }
                    if (string.IsNullOrEmpty(row["ReleaseID"].ToString()))
                    {
                        row["ReleaseID"] = int.MinValue;
                    }

                    if (string.IsNullOrEmpty(row["XSDPath"].ToString()))
                    {
                        row["XSDPath"] = string.Empty;
                    }
                    if (string.IsNullOrEmpty(row["ImportSPName"].ToString()))
                    {
                        row["ImportSPName"] = string.Empty;
                    }
                    if (string.IsNullOrEmpty(row["FTPFolderPath"].ToString()))
                    {
                        row["FTPFolderPath"] = string.Empty;
                    }
                    if (string.IsNullOrEmpty(row["LocalFolderPath"].ToString()))
                    {
                        row["LocalFolderPath"] = string.Empty;
                    }
                    if (string.IsNullOrEmpty(row["FtpID"].ToString()))
                    {
                        row["FtpID"] = int.MinValue;
                    }
                    if (string.IsNullOrEmpty(row["EmailID"].ToString()))
                    {
                        row["EmailID"] = int.MinValue;
                    }
                    if (string.IsNullOrEmpty(row["EmailLogID"].ToString()))
                    {
                        row["EmailLogID"] = int.MinValue;
                    }
                    if (string.IsNullOrEmpty(row["DecryptionID"].ToString()))
                    {
                        row["DecryptionID"] = int.MinValue;
                    }
                    if (string.IsNullOrEmpty(row["PriceToleranceColumns"].ToString()))
                    {
                        row["PriceToleranceColumns"] = string.Empty;
                    }
                    if (string.IsNullOrEmpty(row["BatchStartDate"].ToString()))
                    {
                        row["BatchStartDate"] = DBNull.Value;
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
            return false;
        }

        /// <summary>
        /// Load the variables with data from database
        /// </summary>
        public static void InitializeData()
        {
            try
            {
                dtAccounts = FileSettingDAL.GetAccountsForThirdParty();
                dictRelease = FileSettingDAL.GetReleaseFromDB();
                dictImport = FileSettingDAL.GetImportTypesFromDB();
                _dictImportAcronym = FileSettingDAL.GetImportTypesAcronymFromDB();
                dictSendMail = FileSettingDAL.GetEMailsFromDB(0);
                dictLogMail = FileSettingDAL.GetEMailsFromDB(1);
                dictDecryption = FileSettingDAL.GetDecryptionFromDB();
                dictFtp = FileSettingDAL.GetFtpFromDB();
                dictClient = FileSettingDAL.GetClientsForRelease();
                _dictCommanyDetails = FileSettingDAL.GetCommanyReleaseDetail();

                dictFormatType.Clear();
                int formatTypeID = 0;
                foreach (FormatType val in FormatType.GetValues(typeof(FormatType)))
                {
                    dictFormatType.Add(formatTypeID++, val.ToString());

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
        /// Get the accounts for the currenct third party
        /// </summary>
        /// <param name="thirdPartyID">ID of the third party</param>
        /// <returns>The datatable of Account details</returns>
        public static DataTable GetCurrentThirdPartyAccounts(int thirdPartyID)
        {
            DataTable dtAccounts = new DataTable();
            dtAccounts.Columns.Add("AccountID", typeof(int));
            dtAccounts.Columns.Add("AccountName", typeof(string));
            try
            {
                if (dictAccount.ContainsKey(thirdPartyID))
                {
                    foreach (int accountID in dictAccount[thirdPartyID].Keys)
                    {
                        dtAccounts.Rows.Add(accountID, dictAccount[thirdPartyID][accountID]);
                    }
                }
                return dtAccounts;
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

        /// <summary>
        /// Get the accounts for the currenct third party
        /// </summary>
        /// <param name="thirdPartyID">ID of the third party</param>
        /// <returns>The datatable of Account details</returns>
        public static DataTable GetCurrentThirdPartyClientAccounts(int thirdPartyID, List<int> clientID, int releaseID)
        {
            DataTable dtClientAccounts = new DataTable();
            dtClientAccounts.Columns.Add("FundID", typeof(int));
            dtClientAccounts.Columns.Add("FundName", typeof(string));
            try
            {
                foreach (DataRow dr in dtAccounts.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(dr["CompanyThirdPartyID"].ToString()) && !string.IsNullOrWhiteSpace(dr["CompanyID"].ToString()) && !string.IsNullOrWhiteSpace(dr["CompanyFundID"].ToString()) && !string.IsNullOrWhiteSpace(dr["FundName"].ToString()) && !string.IsNullOrWhiteSpace(dr["ReleaseID"].ToString()))
                    {
                        int thirdParty = Convert.ToInt32(dr["CompanyThirdPartyID"]);
                        int companyID = Convert.ToInt32(dr["CompanyID"]);
                        int release = Convert.ToInt32(dr["ReleaseID"]);
                        int accountID = Convert.ToInt32(dr["CompanyFundID"]);
                        string accountName = dr["FundName"].ToString();
                        if (clientID != null)
                        {
                            if (thirdParty == thirdPartyID && release == releaseID && clientID.Contains(companyID))
                            {
                                dtClientAccounts.Rows.Add(accountID, accountName);
                            }
                        }
                    }
                }
                return dtClientAccounts;
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

        /// <summary>
        /// Create the datasource for the Grid Control 
        /// </summary>
        /// <returns>Datatable holding the data</returns>
        public static DataTable GetFileSettingDetails(int thirdPartyID)
        {
            DataTable dtFileSetting = new DataTable();
            dtFileSetting.Columns.Add("SettingID", typeof(int));
            dtFileSetting.Columns.Add("IsActive", typeof(bool));
            dtFileSetting.Columns.Add("FormatName", typeof(string));
            dtFileSetting.Columns.Add("ImportTypeID", typeof(int));
            dtFileSetting.Columns.Add("ClientID", typeof(object));
            dtFileSetting.Columns.Add("ReleaseID", typeof(int));
            dtFileSetting.Columns.Add("AccountID", typeof(object));
            dtFileSetting.Columns.Add("XSLTPath", typeof(string));
            dtFileSetting.Columns.Add("XSDPath", typeof(string));
            dtFileSetting.Columns.Add("ImportSPName", typeof(string));
            dtFileSetting.Columns.Add("FTPFolderPath", typeof(string));
            dtFileSetting.Columns.Add("LocalFolderPath", typeof(string));
            dtFileSetting.Columns.Add("FtpID", typeof(int));
            dtFileSetting.Columns.Add("EmailID", typeof(int));
            dtFileSetting.Columns.Add("EmailLogID", typeof(int));
            dtFileSetting.Columns.Add("DecryptionID", typeof(int));
            dtFileSetting.Columns.Add("PriceToleranceColumns", typeof(string));
            dtFileSetting.Columns.Add("ThirdPartyID", typeof(int));
            dtFileSetting.Columns.Add("FormatType", typeof(string));
            //added by: Bharat raturi, 28 may 2014
            //purpose: get new detail BatchStartDate
            dtFileSetting.Columns.Add("BatchStartDate", typeof(DateTime));
            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1648
            //To map recon batch setting with import batch
            dtFileSetting.Columns.Add("ImportFormatID", typeof(string));

            try
            {
                dictFileSetting = FileSettingDAL.GetFileSettingsFromDB(thirdPartyID);
                foreach (int settingID in dictFileSetting.Keys)
                {
                    dtFileSetting.Rows.Add(settingID, dictFileSetting[settingID].IsActive,
                        dictFileSetting[settingID].FormatName, dictFileSetting[settingID].ImportTypeID,
                        dictFileSetting[settingID].ClientID, dictFileSetting[settingID].ReleaseID,
                        dictFileSetting[settingID].CompanyAccountID, dictFileSetting[settingID].XsltPath,
                        dictFileSetting[settingID].XsdPath, dictFileSetting[settingID].ImportSpName,
                        dictFileSetting[settingID].FTPFolderPath, dictFileSetting[settingID].LocalFolderPath,
                        dictFileSetting[settingID].FtpID, dictFileSetting[settingID].EmailID,
                        dictFileSetting[settingID].EmailLogID, dictFileSetting[settingID].DecryptionID,
                        dictFileSetting[settingID].PriceToleranceColumns, dictFileSetting[settingID].ThirdPartyID,
                        dictFileSetting[settingID].FormatType, dictFileSetting[settingID].BatchStartDate
                        , dictFileSetting[settingID].ImportFormatID);
                }
                return dtFileSetting;
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

        /// <summary>
        /// Get the datatable for clients
        /// </summary>
        /// <param name="releaseID">THe ID of the release</param>
        /// <returns>datatable of the clients</returns>
        public static DataTable GetClientsForRelease(int releaseID)
        {
            DataTable dtClients = new DataTable();
            dtClients.Columns.Add("CompanyID", typeof(int));
            dtClients.Columns.Add("CompanyName", typeof(string));
            try
            {
                if (dictClient.ContainsKey(releaseID))
                {
                    foreach (KeyValuePair<int, string> client in dictClient[releaseID])
                    {
                        dtClients.Rows.Add(client.Key, client.Value);
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
            return dtClients;
        }

        /// <summary>
        /// Generate IDs for the settings that are added new
        /// </summary>
        /// <param name="dt">Datatable holding the settings</param>
        private static void GenerateSettingIDs(DataTable dt, int thirdPartyID)
        {
            int maxID = FileSettingDAL.GetMaxFileSettingID();
            try
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    //check if there is no ID for the setting
                    if (string.IsNullOrEmpty(dRow["SettingID"].ToString()))
                    {
                        //try
                        //{
                        //assign the ID one more than the previous one for the current setting
                        dRow["SettingID"] = maxID + 1;// Convert.ToInt32(dt.Rows[dt.Rows.IndexOf(dRow) - 1]["SettingID"]) + 1;
                        dRow["ThirdPartyID"] = thirdPartyID;
                        maxID++;
                        //}
                        //catch (IndexOutOfRangeException)
                        //{
                        //    //if this is the first row, assign ID 1
                        //    dRow["SettingID"] = 1;
                        //    dRow["ThirdPartyID"] = thirdPartyID;
                        //}
                    }
                    else
                    {
                        if (Convert.ToInt32(dRow["SettingID"]) > maxID)
                        {
                            maxID = Convert.ToInt32(dRow["SettingID"]);
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
        /// <summary>
        /// Create new SettingIDs for Old Recon Batch
        /// </summary>
        /// <param name="dt"></param>
        private static void GenerateSettingIDsForRecon(DataTable dt)
        {
            int maxID = FileSettingDAL.GetMaxFileSettingID();
            try
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    //check if there is no ID for the setting
                    if (int.Parse(dRow["SettingID"].ToString()) == int.MinValue)
                    {
                        //assign the ID one more than the previous one for the current setting
                        dRow["SettingID"] = maxID + 1;
                        maxID++;

                    }
                    else
                    {
                        if (Convert.ToInt32(dRow["SettingID"]) > maxID)
                        {
                            maxID = Convert.ToInt32(dRow["SettingID"]);
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
        /// <summary>
        /// Create the dataset of the pricing details
        /// </summary>
        /// <param name="dtFileSetting">Datatable of the file setting details</param>
        /// <returns>The dataset holding the file settings in atomic form</returns>
        public static DataSet CreateFileDataSet(DataTable dtFileSetting)
        {
            DataSet dsSetting = new DataSet("dsSetting");
            DataTable dtSetting = new DataTable("dtSetting");
            dtSetting.Columns.Add("SettingID", typeof(int));
            dtSetting.Columns.Add("IsActive", typeof(bool));
            dtSetting.Columns.Add("FormatName", typeof(string));
            dtSetting.Columns.Add("ImportTypeID", typeof(int));
            dtSetting.Columns.Add("ClientID", typeof(int));
            dtSetting.Columns.Add("ReleaseID", typeof(int));
            dtSetting.Columns.Add("FundID", typeof(int));
            dtSetting.Columns.Add("XSLTPath", typeof(string));
            dtSetting.Columns.Add("XSDPath", typeof(string));
            dtSetting.Columns.Add("ImportSPName", typeof(string));
            dtSetting.Columns.Add("FTPFolderPath", typeof(string));
            dtSetting.Columns.Add("LocalFolderPath", typeof(string));
            dtSetting.Columns.Add("FtpID", typeof(int));
            dtSetting.Columns.Add("EmailID", typeof(int));
            dtSetting.Columns.Add("EmailLogID", typeof(int));
            dtSetting.Columns.Add("DecryptionID", typeof(int));
            dtSetting.Columns.Add("ThirdPartyID", typeof(int));
            dtSetting.Columns.Add("PriceToleranceColumns", typeof(string));
            dtSetting.Columns.Add("FormatType", typeof(int));
            dtSetting.Columns.Add("BatchStartDate", typeof(string));

            dtSetting.Columns.Add("ImportFormatID", typeof(int));

            try
            {
                foreach (DataRow row in dtFileSetting.Rows)
                {
                    List<object> listAccount = (List<object>)row["AccountID"];
                    List<object> listClient = (List<object>)row["ClientID"];
                    int settingID = (int)row["SettingID"];
                    bool isActive = (bool)row["IsActive"];
                    string formatName = (string)row["FormatName"];
                    int importTypeID = (int)row["ImportTypeID"];
                    //string clientNameList = Convert.ToString(row["ClientID"] as object);
                    int releaseID = (int)row["ReleaseID"];
                    string xsltPath = Convert.ToString(row["XSLTPath"] as object);
                    string xsdPath = Convert.ToString(row["XSDPath"] as object);
                    string importSPName = Convert.ToString(row["ImportSPName"] as object);
                    string ftpFolderPath = Convert.ToString(row["FTPFolderPath"] as object);
                    string localFolderPath = Convert.ToString(row["LocalFolderPath"] as object);
                    int ftpId = (int)row["FtpID"];
                    int emailID = (int)row["EmailID"];
                    int emailLogId = (int)row["EmailLogID"];
                    int decryptID = (int)row["DecryptionID"];
                    int thirdPartyID = (int)row["ThirdPartyID"];
                    string priceToleranceColumns = (string)row["PriceToleranceColumns"];
                    int formatType = Convert.ToInt32(row["FormatType"]);
                    string dtBatchDate = Convert.ToString(row["BatchStartDate"] as object);

                    int importFormatID = Convert.ToInt32(row["ImportFormatID"]);

                    foreach (int clientID in listClient)
                    {
                        foreach (int accountID in listAccount)
                        {
                            // Commented the code as there was no need of a Primary Key
                            //Commented By faisal Shah 27/08/14
                            //    //create the primary key for the setting
                            //    string pKey = thirdPartyID.ToString() + importTypeID.ToString() + clientID.ToString();
                            //    foreach (DataRow dRow in dtSetting.Rows)
                            //    {
                            //        int value;
                            //        int.TryParse(dRow["FormatType"].ToString(), out value);
                            //        if (value != 1)
                            //        {
                            //            //check if the key is duplicated
                            //            string currVal = dRow["AccountID"].ToString() + dRow["ThirdPartyID"].ToString() + dRow["ImportTypeID"].ToString() + dRow["ClientID"].ToString();
                            //            if (currVal == pKey)
                            //            {
                            //                //change the name of the dataset in the case of duplication
                            //                dsSetting.DataSetName = "Duplicate";
                            //                return dsSetting;
                            //            }
                            //        }
                            //    }
                            dtSetting.Rows.Add(settingID, isActive, formatName, importTypeID, clientID,
                                releaseID, accountID, xsltPath, xsdPath, importSPName, ftpFolderPath, localFolderPath,
                                ftpId, emailID, emailLogId, decryptID, thirdPartyID, priceToleranceColumns, formatType, dtBatchDate, importFormatID);
                        }
                    }
                }
                dsSetting.Tables.Add(dtSetting);
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
            return dsSetting;
        }

        /// <summary>
        /// save the records in the database
        /// </summary>
        /// <param name="dt">Datatable holding the details of the data</param>
        /// <param name="thirdPartyID">ID of the third party</param>
        /// <returns>Number of records affected in the database</returns>
        public static int SaveFileSetting(DataTable dt, int thirdPartyID)
        {
            int i = 0;
            try
            {
                GenerateSettingIDs(dt, thirdPartyID);
                DataSet ds = CreateFileDataSet(dt);
                if (ds.DataSetName == "Duplicate")
                {
                    return -1;
                }
                string xmlDoc = ds.GetXml();
                i = FileSettingDAL.SaveFileSettingIntoDB(xmlDoc, thirdPartyID);
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
            return i;
        }
        /// <summary>
        /// save batch record in DB for old recon preference  
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="thirdpartyID"></param>
        /// <returns></returns>
        public static int SaveFileSettingForRecon(DataTable dt)
        {
            int i = 0;
            int thirdpartyID = int.MinValue;
            try
            {
                GenerateSettingIDsForRecon(dt);
                DataSet ds = CreateFileDataSetForRecon(dt);
                if (ds.DataSetName == "Duplicate")
                {
                    return -1;
                }
                string xmlDoc = ds.GetXml();
                i = FileSettingDAL.SaveFileSettingIntoDB(xmlDoc, thirdpartyID);
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
            return i;
        }
        /// <summary>
        /// Create dataset for old recon preference 
        /// </summary>
        /// <param name="dtFileSetting"></param>
        /// <returns></returns>
        private static DataSet CreateFileDataSetForRecon(DataTable dtFileSetting)
        {
            DataSet dsSetting = new DataSet("dsSetting");
            DataTable dtSetting = new DataTable("dtSetting");
            dtSetting.Columns.Add("SettingID", typeof(int));
            dtSetting.Columns.Add("IsActive", typeof(bool));
            dtSetting.Columns.Add("FormatName", typeof(string));
            dtSetting.Columns.Add("ImportTypeID", typeof(int));
            dtSetting.Columns.Add("ClientID", typeof(int));
            dtSetting.Columns.Add("ReleaseID", typeof(int));
            dtSetting.Columns.Add("FundID", typeof(int));
            dtSetting.Columns.Add("XSLTPath", typeof(string));
            dtSetting.Columns.Add("XSDPath", typeof(string));
            dtSetting.Columns.Add("ImportSPName", typeof(string));
            dtSetting.Columns.Add("FTPFolderPath", typeof(string));
            dtSetting.Columns.Add("LocalFolderPath", typeof(string));
            dtSetting.Columns.Add("FtpID", typeof(int));
            dtSetting.Columns.Add("EmailID", typeof(int));
            dtSetting.Columns.Add("EmailLogID", typeof(int));
            dtSetting.Columns.Add("DecryptionID", typeof(int));
            dtSetting.Columns.Add("ThirdPartyID", typeof(int));
            dtSetting.Columns.Add("PriceToleranceColumns", typeof(string));
            dtSetting.Columns.Add("FormatType", typeof(int));
            dtSetting.Columns.Add("BatchStartDate", typeof(string));

            dtSetting.Columns.Add("ImportFormatID", typeof(int));

            try
            {
                foreach (DataRow row in dtFileSetting.Rows)
                {
                    List<int> listAccount = (List<int>)row["FundID"];
                    int clientID = (int)row["ClientID"];
                    int settingID = (int)row["SettingID"];
                    bool isActive = (bool)row["IsActive"];
                    string formatName = (string)row["FormatName"];
                    int importTypeID = (int)row["ImportTypeID"];
                    //string clientNameList = Convert.ToString(row["ClientID"] as object);
                    int releaseID = (int)row["ReleaseID"];
                    string xsltPath = Convert.ToString(row["XSLTPath"] as object);
                    string xsdPath = Convert.ToString(row["XSDPath"] as object);
                    string importSPName = Convert.ToString(row["ImportSPName"] as object);
                    string ftpFolderPath = Convert.ToString(row["FTPFolderPath"] as object);
                    string localFolderPath = Convert.ToString(row["LocalFolderPath"] as object);
                    int ftpId = (int)row["FtpID"];
                    int emailID = (int)row["EmailID"];
                    int emailLogId = (int)row["EmailLogID"];
                    int decryptID = (int)row["DecryptionID"];
                    int thirdPartyID = (int)row["ThirdPartyID"];
                    string priceToleranceColumns = (string)row["PriceToleranceColumns"];
                    int formatType = Convert.ToInt32(row["FormatType"]);
                    string dtBatchDate = Convert.ToString(row["BatchStartDate"] as object);

                    int importFormatID = Convert.ToInt32(row["ImportFormatID"]);


                    foreach (int accountID in listAccount)
                    {

                        dtSetting.Rows.Add(settingID, isActive, formatName, importTypeID, clientID,
                            releaseID, accountID, xsltPath, xsdPath, importSPName, ftpFolderPath, localFolderPath,
                            ftpId, emailID, emailLogId, decryptID, thirdPartyID, priceToleranceColumns, formatType, dtBatchDate, importFormatID);
                    }

                }
                dsSetting.Tables.Add(dtSetting);
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
            return dsSetting;
        }

        /// <summary>
        /// Get the updated Advance details from DB
        /// </summary>
        public static void RefreshAdvanceDetails()
        {
            try
            {
                dictSendMail = FileSettingDAL.GetEMailsFromDB(0);
                dictLogMail = FileSettingDAL.GetEMailsFromDB(1);
                dictDecryption = FileSettingDAL.GetDecryptionFromDB();
                dictFtp = FileSettingDAL.GetFtpFromDB();
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
        /// Get the IDs of the clients for release from the names of the clients
        /// </summary>
        /// <param name="clientList">List of client names</param>
        /// <returns>List of client IDs</returns>
        public static List<int> GetClientIDsFromNames(string clientList)
        {
            List<int> clientIDs = new List<int>();
            try
            {
                string[] clients = clientList.Split(',');
                foreach (string clientName in clients)
                {
                    if (!string.IsNullOrWhiteSpace(clientName))
                    {
                        int clientID = CachedDataManager.GetCompanyID(clientName.Trim());
                        if (clientID != -1)
                        {
                            clientIDs.Add(clientID);
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
            return clientIDs;
        }

        /// <summary>
        /// added by: Bharat Raturi, 16 jun 2014
        /// Get the batch information from the data row so that the batch, if has been run, cannot be deleted
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>true if the batch has been run</returns>
        public static bool IsBatchExecuted(DataRow dr)
        {
            try
            {
                if (dr != null && !string.IsNullOrWhiteSpace(dr["SettingID"].ToString()) && !string.IsNullOrWhiteSpace(dr["LocalFolderPath"].ToString()) && !string.IsNullOrWhiteSpace(dr["ReleaseID"].ToString()))
                {
                    string formatName = BatchSetupManager.GetBatchNameFromID(Convert.ToInt32(dr["SettingID"]));

                    string dashboardPath = string.Empty;
                    if (dictRelease.ContainsKey(Convert.ToInt32(dr["ReleaseID"])))
                    {
                        string releaseName = dictRelease[Convert.ToInt32(dr["ReleaseID"])];
                        string releasePath = _dictCommanyDetails[releaseName].ReleasePath; //dr["LocalFolderPath"].ToString();
                        dashboardPath = releasePath + "\\DashBoardData\\Import\\";
                    }
                    //.Substring(0, releasePath.LastIndexOf("Debug") + 5) + "\\DashBoardData\\Import\\";
                    if (!Directory.Exists(dashboardPath) || formatName.Equals(string.Empty))
                    {
                        return false;
                    }
                    //string[] dashboardDirs = Directory.GetDirectories(dashboardPath);
                    //List<String> filesList = new List<string>();
                    //foreach (string dir in dashboardDirs)
                    //{
                    //    string[] files = Directory.GetFiles(dir);
                    //    filesList.AddRange(files);
                    //}
                    string[] filesList = Directory.GetFiles(dashboardPath, "*.*", SearchOption.AllDirectories);
                    var dashBoardFiles = from file in filesList where file.Contains(formatName) select file;
                    if (dashBoardFiles.ToList().Count > 0)
                    {
                        return true;
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
            return false;
        }
    }
}
