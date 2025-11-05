using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.BLL.DAL
{
    /// <summary>
    /// Database Access class for file settings
    /// </summary>
    internal class FileSettingDAL
    {
        static string connStr = ApplicationConstants.PranaConnectionString;

        /// <summary>
        /// Get the file setting details for all the third parties from the database
        /// </summary>
        /// <returns>Dictionary of FileSettingID-ThirdPartyFileSetting pair</returns>
        internal static Dictionary<int, FileSettingItem> GetFileSettingsFromDB(int thirdPartyID)
        {
            Dictionary<int, FileSettingItem> dictSetting = new Dictionary<int, FileSettingItem>();
            try
            {
                string sProc = "P_GetAllImportFileSettings";
                //object[] param = { thirdPartyID };
                using (IDataReader drSetting = DatabaseManager.DatabaseManager.ExecuteReader(sProc, new object[] { thirdPartyID }, connStr))
                {
                    while (drSetting.Read())
                    {
                        int settingID = drSetting.GetInt32(0);
                        if (dictSetting.ContainsKey(settingID))
                        {
                            if (!dictSetting[settingID].ClientID.Contains(drSetting.GetInt32(4)))
                            {
                                dictSetting[settingID].ClientID.Add(drSetting.GetInt32(4));
                            }
                            if (!dictSetting[settingID].CompanyAccountID.Contains(drSetting.GetInt32(6)))
                            {
                                dictSetting[settingID].CompanyAccountID.Add(drSetting.GetInt32(6));
                            }
                        }
                        else
                        {
                            FileSettingItem fileSetting = new FileSettingItem();
                            fileSetting.FileSettingID = drSetting.GetInt32(0);
                            fileSetting.IsActive = drSetting.GetBoolean(1);
                            fileSetting.FormatName = drSetting.GetString(2);
                            fileSetting.ImportTypeID = drSetting.GetInt32(3);
                            fileSetting.ClientID.Add(drSetting.GetInt32(4));
                            fileSetting.ReleaseID = drSetting.GetInt32(5);
                            fileSetting.CompanyAccountID.Add(drSetting.GetInt32(6));
                            fileSetting.XsltPath = drSetting.GetString(7);
                            fileSetting.XsdPath = drSetting.GetString(8);
                            fileSetting.ImportSpName = drSetting.GetString(9);
                            fileSetting.FTPFolderPath = drSetting.GetString(10);
                            fileSetting.LocalFolderPath = drSetting.GetString(11);
                            fileSetting.FtpID = drSetting.GetInt32(12);
                            fileSetting.EmailID = drSetting.GetInt32(13);
                            fileSetting.EmailLogID = drSetting.GetInt32(14);
                            fileSetting.DecryptionID = drSetting.GetInt32(15);
                            fileSetting.ThirdPartyID = drSetting.GetInt32(16);
                            fileSetting.PriceToleranceColumns = drSetting.GetString(17);
                            fileSetting.FormatType = drSetting.GetString(18);
                            if (drSetting.GetValue(19) != null && !string.IsNullOrEmpty(drSetting.GetValue(19).ToString()))
                            {
                                fileSetting.BatchStartDate = drSetting.GetDateTime(19);
                            }
                            if (drSetting.FieldCount > 20 && drSetting.GetValue(20) != null && !string.IsNullOrEmpty(drSetting.GetValue(20).ToString()))
                            {
                                fileSetting.ImportFormatID = drSetting.GetInt32(20);
                            }
                            else
                            {
                                fileSetting.ImportFormatID = int.MinValue;
                            }
                            dictSetting.Add(settingID, fileSetting);
                        }
                    }
                }
                return dictSetting;
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
        /// Get the release information from the DB
        /// </summary>
        /// <returns>Get Dictionary of ReleaseID-ReleaseName pair</returns>
        internal static Dictionary<int, string> GetReleaseFromDB()
        {
            Dictionary<int, string> dictRelease = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllReleaseIDNames";

                using (IDataReader drRelease = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drRelease.Read())
                    {
                        if (drRelease.GetValue(0) != DBNull.Value && drRelease.GetValue(1) != DBNull.Value)
                        {
                            if (!dictRelease.ContainsKey(drRelease.GetInt32(0)))
                                dictRelease.Add(drRelease.GetInt32(0), drRelease.GetString(1));
                        }
                    }
                }
                return dictRelease;
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

        internal static DataTable GetAccountsForThirdParty()  //int primeBrokerID)
        {
            DataTable dtAccounts = new DataTable();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllThirdPartyPermittedFunds";

                dtAccounts = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, connStr).Tables[0];
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
            return dtAccounts;
        }


        /// <summary>
        /// Get the formatID-FormatName from database
        /// </summary>
        /// <returns>The dictionary of formatID-FormatName</returns>
        internal static Dictionary<int, string> GetImportTypesFromDB()
        {
            Dictionary<int, string> dictImport = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllImportTypes";

                using (IDataReader drImport = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drImport.Read())
                    {
                        if (drImport.GetValue(0) != DBNull.Value && drImport.GetValue(1) != DBNull.Value)
                        {
                            if (!dictImport.ContainsKey(drImport.GetInt32(0)))
                                dictImport.Add(drImport.GetInt32(0), drImport.GetString(1));
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
            return dictImport;
        }


        /// <summary>
        /// Get the formatID-FormatAcronym from database
        /// </summary>
        /// <returns>The dictionary of formatID-FormatName</returns>
        internal static Dictionary<int, string> GetImportTypesAcronymFromDB()
        {
            Dictionary<int, string> dictImportAcronym = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllImportAcronymTypes";

                using (IDataReader drImport = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drImport.Read())
                    {
                        if (drImport.GetValue(0) != DBNull.Value && drImport.GetValue(1) != DBNull.Value)
                        {
                            if (!dictImportAcronym.ContainsKey(drImport.GetInt32(0)))
                                dictImportAcronym.Add(drImport.GetInt32(0), drImport.GetString(1));
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
            return dictImportAcronym;
        }



        /// <summary>
        /// Get the Details of FTP servers from DB
        /// </summary>
        /// <returns>The FtpID-FtpName dictionary</returns>
        internal static Dictionary<int, string> GetFtpFromDB()
        {
            Dictionary<int, string> dictFtp = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllFtpIDNames";

                using (IDataReader drFtp = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drFtp.Read())
                    {
                        if (drFtp.GetValue(0) != DBNull.Value && drFtp.GetValue(1) != DBNull.Value)
                        {
                            int ftpID = drFtp.GetInt32(0);
                            string ftpName = drFtp.GetString(1);
                            dictFtp.Add(ftpID, ftpName);
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
            return dictFtp;
        }

        /// <summary>
        /// Get the Email list for the File setting
        /// </summary>
        /// <returns>The dictionary of mailtype-[EmailID-EmailName] pair(mailtype 0 for mail, 1 for log)</returns>
        internal static Dictionary<int, string> GetEMailsFromDB(int mailType)
        {
            Dictionary<int, string> dictEmail = new Dictionary<int, string>();
            try
            {
                string sProc = "P_GetEmailForFileSetting";
                object[] param = { mailType };
                using (IDataReader drEmail = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, connStr))
                {
                    while (drEmail.Read())
                    {
                        if (drEmail.GetValue(0) != DBNull.Value && drEmail.GetValue(1) != DBNull.Value)
                        {
                            int emailID = drEmail.GetInt32(0);
                            string emailName = drEmail.GetString(1);
                            dictEmail.Add(emailID, emailName);
                        }
                    }
                }
                return dictEmail;
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
        /// Get the Details of FTP servers from DB
        /// </summary>
        /// <returns>The FtpID-FtpName dictionary</returns>
        internal static Dictionary<int, string> GetDecryptionFromDB()
        {
            Dictionary<int, string> dictDecryption = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDecryptionForFileSetting";

                using (IDataReader drDecryption = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drDecryption.Read())
                    {
                        if (drDecryption.GetValue(0) != DBNull.Value && drDecryption.GetValue(1) != DBNull.Value)
                        {
                            int decryptionID = drDecryption.GetInt32(0);
                            string decryptionName = drDecryption.GetString(1);
                            dictDecryption.Add(decryptionID, decryptionName);
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
            return dictDecryption;
        }

        /// <summary>
        /// get the clients for the release
        /// </summary>
        /// <returns>Dictionary of client-release information</returns>
        internal static Dictionary<int, Dictionary<int, string>> GetClientsForRelease()
        {
            Dictionary<int, Dictionary<int, string>> dictClient = new Dictionary<int, Dictionary<int, string>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetClientsForReleases";

                using (IDataReader drClient = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drClient.Read())
                    {
                        if (!string.IsNullOrWhiteSpace(drClient.GetValue(0).ToString()) && !string.IsNullOrWhiteSpace(drClient.GetValue(1).ToString()) && !string.IsNullOrWhiteSpace(drClient.GetValue(2).ToString()))
                        {
                            int releaseID = drClient.GetInt32(0);
                            int clientID = drClient.GetInt32(1);
                            string clientName = drClient.GetString(2);
                            if (dictClient.ContainsKey(releaseID))
                            {
                                if (!dictClient[releaseID].ContainsKey(clientID))
                                {
                                    dictClient[releaseID].Add(clientID, clientName);
                                }
                            }
                            else
                            {
                                Dictionary<int, string> clientList = new Dictionary<int, string>();
                                clientList.Add(clientID, clientName);
                                dictClient.Add(releaseID, clientList);
                            }
                        }
                    }
                }
                return dictClient;
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
        /// Save the file setting details for the current third party in the db
        /// </summary>
        /// <param name="xmlSetting">XML document representing the settings</param>
        /// <param name="thirdPartyID">ID of the third party</param>
        /// <returns>number of affected records in the db </returns>
        internal static int SaveFileSettingIntoDB(string xmlSetting, int thirdPartyID)
        {
            int i = 0;
            try
            {
                string sProc = "P_SaveThirdPartyFileSettingDetails";
                object[] param = { xmlSetting, thirdPartyID };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProc, param, connStr);
                return i;
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
            return 0;
        }
        /// <summary>
        /// Fill all the company details in dictCompanyDetails key=releaseName,value= CommanyReleaseDetail 
        /// from sp=CommanyReleaseDetail
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, CompanyReleaseDetail> GetCommanyReleaseDetail()
        {
            Dictionary<string, CompanyReleaseDetail> dictCommanyReleaseDetail = new Dictionary<string, CompanyReleaseDetail>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanyReleaseDetails";

                using (IDataReader drCLient = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drCLient.Read())
                    {
                        string releasename = drCLient.GetString(2);
                        //Adds accountID in list of account if the release name already exist in dictionary
                        if (dictCommanyReleaseDetail.ContainsKey(releasename))
                        {
                            //dictCommanyReleaseDetail[releasename].CompanyAccountID.Add(drCLient.GetInt32(5));

                            // For multiple client IDs
                            dictCommanyReleaseDetail[releasename].CompanyID.Add(drCLient.GetInt32(1));
                        }
                        else
                        {
                            CompanyReleaseDetail detail = new CompanyReleaseDetail();
                            detail.CompanyReleaseID = drCLient.GetInt32(0);
                            //detail.CompanyID = drCLient.GetInt32(1);
                            detail.ReleaseName = drCLient.GetString(2);
                            detail.Ip = drCLient.GetString(3);
                            detail.ReleasePath = drCLient.GetString(4);
                            //detail.CompanyAccountID.Add(drCLient.GetInt32(5));
                            detail.NirvanaClient = drCLient.GetString(5);
                            detail.SecurityMasterDB = drCLient.GetString(6);
                            dictCommanyReleaseDetail.Add(releasename, detail);
                        }
                    }
                }
                return dictCommanyReleaseDetail;
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
        /// Get maximum file setting ID from the database
        /// </summary>
        /// <returns>Integer File setting ID</returns>
        internal static int GetMaxFileSettingID()
        {
            int i = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxFileSettingID";

                if (DatabaseManager.DatabaseManager.ExecuteScalar(queryData, connStr) != DBNull.Value)
                {
                    i = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar(queryData, connStr));
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
            return i;
        }
    }
}
