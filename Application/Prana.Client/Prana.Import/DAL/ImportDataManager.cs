using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
using Prana.DatabaseManager;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Prana.Import
{
    public class ImportDataManager
    {
        public static int SaveImportedFileDetails(string importFileName, string importFilePath, ImportType importType, DateTime importFIleLastModifiedTime)
        {
            int importFileID = int.MinValue;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveImportedFileDetails";
                queryData.DictionaryDatabaseParameter.Add("@importFileID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@importFileID",
                    ParameterType = DbType.Int32,
                    ParameterValue = 0
                });
                queryData.DictionaryDatabaseParameter.Add("@importFileName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@importFileName",
                    ParameterType = DbType.String,
                    ParameterValue = importFileName
                });
                queryData.DictionaryDatabaseParameter.Add("@importFilePath", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@importFilePath",
                    ParameterType = DbType.String,
                    ParameterValue = importFilePath
                });
                queryData.DictionaryDatabaseParameter.Add("@importType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@importType",
                    ParameterType = DbType.String,
                    ParameterValue = importType
                });
                queryData.DictionaryDatabaseParameter.Add("@importFIleLastModifiedTime", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@importFIleLastModifiedTime",
                    ParameterType = DbType.DateTime,
                    ParameterValue = importFIleLastModifiedTime
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                importFileID = Convert.ToInt32(queryData.DictionaryDatabaseParameter["@importFileID"].ParameterValue);
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
            return importFileID;
        }

        public static DataSet GetImportedFileDetails(string dirPath)
        {
            DataSet dsImportedFileDetails = new DataSet();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = dirPath;
                dsImportedFileDetails = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetImportedFileDetails", parameter);
                return dsImportedFileDetails;
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
            return dsImportedFileDetails;
        }

        /// <summary>
        /// AMAN TODO: Fetch details as mentioned in JIRA: CHMW-282
        /// Decryption details and Ftp details will be fetched another sp's
        /// </summary>
        /// <param name="formatName"></param>
        /// <returns>Return ThirdPartyTypeID, ThirdPartyID, FtpID, DecryptionID, AccountIDs, SourceFolderPath, LocalFolderPath, FileName</returns>
        public static DataSet GetThirdPartyFileSettingDetails(string formatName)
        {
            DataSet dsImportedFileDetails = new DataSet();
            try
            {
                //Need to split format name to get ThirdPartyType, ThirdPartyShrtName,ImportType and ImportName
                string[] formatString = formatName.Split('.');
                string thirdPartyTypeShortName = formatString[0];
                string thirdPartyName = formatString[1];
                string importType = formatString[2];
                string name = formatString[3];

                //Modifiedby sachin mishra 3/25/15 Jira- CHMW-3084
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetThirdPartyFileSettingDetails";
                queryData.CommandTimeout = 800;
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyTypeShortName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyTypeShortName",
                    ParameterType = DbType.String,
                    ParameterValue = thirdPartyTypeShortName
                });
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyName",
                    ParameterType = DbType.String,
                    ParameterValue = thirdPartyName
                });
                queryData.DictionaryDatabaseParameter.Add("@ImportType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ImportType",
                    ParameterType = DbType.String,
                    ParameterValue = importType
                });
                queryData.DictionaryDatabaseParameter.Add("@FormatName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FormatName",
                    ParameterType = DbType.String,
                    ParameterValue = name
                });

                //Need to write a new SP, following SP does not exist,
                //We need to write this
                dsImportedFileDetails = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return dsImportedFileDetails;
        }
        public static void SaveDataForTheGivenSP(string xml, string spName)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = xml;
                int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter).ToString());
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
        /// Fetch all import format names
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, String> GetImportFormatNames()
        {
            Dictionary<int, String> dictImportFormats = new Dictionary<int, string>();
            try
            {
                String procedureName = "P_GetAllBatchDetails";
                //DataTable dtList = dsRule.Tables[0];

                object[] parameters = new object[1];
                parameters[0] = -1;

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(procedureName, parameters);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string[] checkifRecon = dr["FormatName"].ToString().Split('.');
                        if (checkifRecon.Length > 2 && (!string.IsNullOrEmpty(checkifRecon[2])) && checkifRecon[2] != "Recon")
                        {
                            if (!dictImportFormats.ContainsKey(Convert.ToInt32(dr["BatchSchedulerID"])))
                            {
                                dictImportFormats.Add(Convert.ToInt32(dr["BatchSchedulerID"]), dr["FormatName"].ToString());
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
            return dictImportFormats;
        }

        /// <summary>
        /// Fill run upload details fetched from database
        /// </summary>
        /// <param name="dsThirdPartyDetails"></param>
        /// <returns></returns>
        public static RunUpload FillRunUploadDetails(string formatName)
        {
            RunUpload runUpload = new RunUpload();
            try
            {
                DataSet dsThirdPartyDetails = ImportDataManager.GetThirdPartyFileSettingDetails(formatName);
                if (dsThirdPartyDetails == null || dsThirdPartyDetails.Tables.Count == 0 || dsThirdPartyDetails.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                DataRow dr;

                DataTable dtThirdPartyFileSettingDetails = dsThirdPartyDetails.Tables[0];
                //Fill ftp details in business object FTPDetails

                #region Fill runUpload

                dr = dtThirdPartyFileSettingDetails.Rows[0];
                //TODO: Here we are filling third party details in datasource deatils of runUpload

                //ThirdPartyName 
                runUpload.DataSourceNameIDValue.ShortName = Convert.ToString(dr[1]);
                //TODO: Third Party Short name is assigned to fullname, need to fetch full name from DB
                //ThirdPartyName 
                runUpload.DataSourceNameIDValue.FullName = Convert.ToString(dr[1]);
                //ImportType           
                runUpload.ImportTypeAcronym = (ImportType)Enum.Parse(typeof(ImportType), Convert.ToString(dr[2]));
                //ImportFormatName e.g. englTradeImport
                //string ImportFormatName = Convert.ToString(dr[3]);
                //FTPFolderPath
                runUpload.FileName = Convert.ToString(dr[4]);
                //LocalFolderPath
                runUpload.FtpFilePath = Convert.ToString(dr[4]);
                runUpload.FilePath = Convert.ToString(dr[5]);
                runUpload.RawFilePath = Convert.ToString(dr[5]);
                runUpload.ProcessedFilePath = Convert.ToString(dr[5]);
                //XSLTPath-only xslt name
                runUpload.DataSourceXSLT = Convert.ToString(dr[6]);
                //XSDPath-only xsd name
                runUpload.XSDName = Convert.ToString(dr[7]);
                //ThirdPartyID
                runUpload.DataSourceNameIDValue.ID = Convert.ToInt32(dr[8]);

                int ftpID = Convert.ToInt32(dr[9]);
                int decryptionID = Convert.ToInt32(dr[10]);
                int emailID = Convert.ToInt32(dr[11]);

                runUpload.IsPriceToleranceChecked = Convert.ToBoolean(dr[12]);
                runUpload.PriceTolerance = Convert.ToDouble(dr[13]);
                runUpload.PriceToleranceColumns = Convert.ToString(dr[14]);
                runUpload.CompanyNameIDValue.ID = Convert.ToInt32(dr[16]);
                runUpload.CompanyNameIDValue.ShortName = Convert.ToString(dr[17]);
                runUpload.CompanyNameIDValue.FullName = Convert.ToString(dr[18]);
                runUpload.BatchStartDate = Convert.ToDateTime(dr[19]);
                runUpload.SPName = dr[20].ToString();
                #endregion
                if (ftpID > 0)
                    runUpload.FtpDetails = GetThirdPartyFtp(ftpID);

                if (decryptionID > 0)
                    runUpload.DecryptionDetails = (ThirdPartyGnuPG)(GetThirdPartyGnuPGForDecryption(decryptionID)[0]);

                if (emailID > 0)
                    runUpload.EmailDetails = GetThirdPartyEmail(emailID);
                runUpload.ImportDataSource = ImportHandlerManager.GetImportDataSource(runUpload.ImportTypeAcronym);
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
            return runUpload;
        }

        /// <summary>
        /// Gets the third party Email.
        /// </summary>
        /// <param name="ftpId">The email id.</param>
        /// <returns>The third party email</returns>
        /// <remarks></remarks>
        private static ThirdPartyEmail GetThirdPartyEmail(int ftpId)
        {
            try
            {
                ThirdPartyEmails emails = GetData<ThirdPartyEmails, ThirdPartyEmail>("P_GetThirdPartyEmail", ftpId, -1);
                return (ThirdPartyEmail)emails[0];
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
        /// Gets the third party gnu PG.
        /// </summary>
        /// <param name="gnuPGId">The gnu PG id.</param>
        /// <returns>The  GnuPG object</returns>
        /// <remarks></remarks>
        private static ThirdPartyGnuPGs GetThirdPartyGnuPGForDecryption(int gnuPGId)
        {
            try
            {
                return GetData<ThirdPartyGnuPGs, ThirdPartyGnuPG>("P_GetThirdPartyGnuPGForDecryption", gnuPGId);
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
        /// Gets the third party FTP.
        /// </summary>
        /// <param name="ftpId">The FTP id.</param>
        /// <returns>The third party FTP</returns>
        /// <remarks></remarks>
        private static ThirdPartyFtp GetThirdPartyFtp(int ftpId)
        {
            try
            {
                ThirdPartyFtps ftps = GetData<ThirdPartyFtps, ThirdPartyFtp>("P_GetThirdPartyFtp", ftpId);
                if (ftps.Count > 0)
                {
                    return (ThirdPartyFtp)ftps[0];
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
            return null;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static T GetData<T, U>(string sql, params Object[] parameters)
            where T : IList, new()
            where U : class, new()
        {
            T list = new T();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(sql, parameters))
                {
                    ReadItems<U>(reader, list);
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

            return list;
        }

        /// <summary>
        /// Reads the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IList ReadItems<T>(IDataReader reader, IList list) where T : new()
        {
            try
            {
                while (reader.Read())
                {
                    T entity = new T();
                    Type entityType = typeof(T);

                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        string name = reader.GetName(index);
                        PropertyInfo info = entityType.GetProperty(name);

                        try
                        {
                            if (info != null && info.CanWrite && reader[index] != DBNull.Value)
                                info.SetValue(entity, reader[index], null);
                            else if (info == null)
                                Debug.Print("Can't find field {0}", name);
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
                            continue;
                        }
                        #endregion

                    }
                    list.Add(entity);
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
            return list;
        }
    }
}