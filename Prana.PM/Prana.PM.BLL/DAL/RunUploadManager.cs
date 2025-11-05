using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace Prana.PM.DAL
{
    public class RunUploadManager
    {
        public static event EventHandler DataImport_OMI;
        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        /// <summary>
        /// Gets the run upload list.
        /// </summary>
        /// <param name="companyNameIDValue">The company name ID value.</param>
        /// <returns></returns>
        public static UploadClientSetUpList GetRunUploadSetupDetails(CompanyNameID companyNameIDValue)
        {
            UploadClientSetUpList uploadSetUpDetailsList = new UploadClientSetUpList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetRunUploadSetupDetailsForID";
            queryData.DictionaryDatabaseParameter.Add("@PMCompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@PMCompanyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyNameIDValue.ID
            });

            //db.AddOutParameter(commandSP, "@ErrorMessage", DbType.Int32, 200);
            XMLSaveManager.AddOutErrorParameters(queryData);
            //string errorMessage = string.Empty;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        uploadSetUpDetailsList.Add(FillUploadClientSetUPDetails(row, 0));

                    }
                    //errorMessage = Convert.ToString(commandSP.Parameters["@ErrorMessage"].Value);
                }


            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    //throw;
                }
            }


            return uploadSetUpDetailsList;
        }

        /// <summary>
        /// Fills the upload client set UP details.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static SetUploadClient FillUploadClientSetUPDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            SetUploadClient uploadSetUpDetail = null;


            if (row != null)
            {
                uploadSetUpDetail = new SetUploadClient();
                int COMPANYUPLOADSETUPID = offset + 0;
                int PMCOMPANYID = offset + 1;
                int COMPANYSHORTNAME = offset + 2;
                int THIRDPARTYID = offset + 3;
                int DATASOURCESHORTNAME = offset + 4;
                int FTPSERVER = offset + 5;
                int FTPPORT = offset + 6;
                int FTPUSERNAME = offset + 7;
                int FTPPASSWORD = offset + 8;
                int AUTOTIME = offset + 9;
                int DIRECTORYPATH = offset + 10;
                int FILENAME = offset + 11;
                // int LASTRUNUPLOADDATE = offset + 12;
                int AUTOIMPORT = offset + 13;
                int TableTypeID = offset + 14;
                int FileLayoutType = offset + 15;
                int DataSourceXSLT = offset + 16;
                int TableFormatName = offset + 17;
                int DataSourceXSLTFileID = offset + 18;
                int FTPFILEPATH = offset + 19;
                //int FirstRecordNumber = offset + 17;

                try
                {
                    uploadSetUpDetail.CompanyUploadSetupID = Convert.ToInt32(row[COMPANYUPLOADSETUPID].ToString());
                    uploadSetUpDetail.CompanyID = Convert.ToInt32(row[PMCOMPANYID].ToString());
                    uploadSetUpDetail.CompanyNameIDValue.ID = Convert.ToInt32(row[PMCOMPANYID].ToString());
                    uploadSetUpDetail.CompanyNameIDValue.ShortName = Convert.ToString(row[COMPANYSHORTNAME]);
                    uploadSetUpDetail.DataSourceNameIDValue.ID = Convert.ToInt32(row[THIRDPARTYID].ToString());
                    uploadSetUpDetail.ThirdPartyID = Convert.ToInt32(row[THIRDPARTYID].ToString());
                    uploadSetUpDetail.DataSourceNameIDValue.ShortName = Convert.ToString(row[DATASOURCESHORTNAME]);
                    uploadSetUpDetail.FTPFilePath = Convert.ToString(row[FTPFILEPATH]);
                    uploadSetUpDetail.FTPServer = Convert.ToString(row[FTPSERVER]);
                    uploadSetUpDetail.Port = Convert.ToInt32(row[FTPPORT].ToString());
                    uploadSetUpDetail.UserName = Convert.ToString(row[FTPUSERNAME]);
                    uploadSetUpDetail.Password = Convert.ToString(row[FTPPASSWORD]);
                    uploadSetUpDetail.AutoTime = Convert.ToDateTime(row[AUTOTIME].ToString());
                    uploadSetUpDetail.DirPath = Convert.ToString(row[DIRECTORYPATH]);// modifify by sandeep as on 14-Nov-2005
                    //uploadSetUpDetail.DirPath = filePath;
                    uploadSetUpDetail.FileName = Convert.ToString(row[FILENAME]);
                    uploadSetUpDetail.AutoImport = Convert.ToBoolean(row[AUTOIMPORT]);
                    uploadSetUpDetail.TableTypeID = Convert.ToInt32(row[TableTypeID]);
                    uploadSetUpDetail.DataSourceXSLTFileID = Convert.ToInt32(row[DataSourceXSLTFileID]);
                    if (!(row[FileLayoutType] is DBNull))
                    {
                        uploadSetUpDetail.FileLayoutType = (DataSourceFileLayout)Convert.ToInt16(row[FileLayoutType]);
                    }
                    if (row[DataSourceXSLT] != System.DBNull.Value)
                    {
                        uploadSetUpDetail.DataSourceXSLT = row[DataSourceXSLT].ToString();
                    }
                    if (row[TableFormatName] != System.DBNull.Value)
                    {
                        uploadSetUpDetail.TableFormatName = row[TableFormatName].ToString();
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

            return uploadSetUpDetail;
        }

        /// <summary>
        /// Saves the last imported file for FTP.
        /// </summary>
        /// <param name="importType">Type of the import.</param>
        /// <returns></returns>
        public static List<string> GetImportedFilesForType(ImportType importType)
        {
            List<string> importedFiles = new List<string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetImportedFilesForType";
                queryData.DictionaryDatabaseParameter.Add("@ImportType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ImportType",
                    ParameterType = DbType.String,
                    ParameterValue = importType
                });
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        string fileName = reader["ImportFileName"].ToString();
                        importedFiles.Add(fileName);
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
            return importedFiles;
        }

        /// <summary>
        /// Saves the run upload setup list.
        /// </summary>
        /// <param name="runUploadList">The run upload list.</param>
        /// <returns></returns>
        public static int SaveRunUploadSetupList(UploadClientSetUpList setUploadList)
        {


            try
            {
                string xml = XMLUtilities.SerializeToXML(setUploadList);

                return XMLSaveManager.SaveThroughXML("PMAddUpdateRunUploadSetup_New", xml);

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
        /// Saves the up load run data.
        /// </summary>
        /// <param name="_successfullUploads">The _successfull uploads.</param>
        /// <returns></returns>
        public static int SaveUpLoadRunData(RunUpload runUpload)
        {

            //string dbMessage = string.Empty;
            //int numberOfRowsEffected = 0;
            int uploadID = 0;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveUploadRunFileInformation";
                queryData.DictionaryDatabaseParameter.Add("@CompanyUploadSetupID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyUploadSetupID",
                    ParameterType = DbType.Int32,
                    ParameterValue = runUpload.CompanyUploadSetupID
                });
                queryData.DictionaryDatabaseParameter.Add("@Status", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Status",
                    ParameterType = DbType.Int32,
                    ParameterValue = runUpload.Status
                });
                queryData.DictionaryDatabaseParameter.Add("@UploadStart", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@UploadStart",
                    ParameterType = DbType.DateTime,
                    ParameterValue = runUpload.UploadStartTime
                });
                queryData.DictionaryDatabaseParameter.Add("@UploadEnd", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@UploadEnd",
                    ParameterType = DbType.DateTime,
                    ParameterValue = runUpload.UploadEndTime
                });
                queryData.DictionaryDatabaseParameter.Add("@TotalRecords", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TotalRecords",
                    ParameterType = DbType.Int32,
                    ParameterValue = runUpload.TotalRecords
                });
                queryData.DictionaryDatabaseParameter.Add("@StatusDescription", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StatusDescription",
                    ParameterType = DbType.String,
                    ParameterValue = runUpload.StatusDescription
                });
                queryData.DictionaryDatabaseParameter.Add("@HeaderRowIndex", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@HeaderRowIndex",
                    ParameterType = DbType.Int32,
                    ParameterValue = runUpload.HeaderIndex
                });
                queryData.DictionaryDatabaseParameter.Add("@FirstRecordIndex", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FirstRecordIndex",
                    ParameterType = DbType.Int32,
                    ParameterValue = runUpload.FirstRecordIndex
                });
                queryData.DictionaryDatabaseParameter.Add("@UploadID", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@UploadID",
                    ParameterType = DbType.Int32,
                    ParameterValue = 200,
                    OutParameterSize = sizeof(Int32)
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                uploadID = Convert.ToInt32(queryData.DictionaryDatabaseParameter["@UploadID"].ParameterValue);

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

            return uploadID;


        }

        /// <summary>
        /// PMs the get data source columns by ID.
        /// </summary>
        /// <param name="thirdPartyID">The data source ID.</param>
        /// <returns></returns>
        public static DataSourceColumnList PMGetDataSourceColumnsByID(int thirdPartyID, int tableTypeID)
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetDataSourceColumnsByID";
            queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ThirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyID
            });
            queryData.DictionaryDatabaseParameter.Add("@TableTypeID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@TableTypeID",
                ParameterType = DbType.Int32,
                ParameterValue = tableTypeID
            });

            DataSourceColumnList columnList = new DataSourceColumnList();

            try
            {
                XMLSaveManager.AddOutErrorParameters(queryData);

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        columnList.Add(FillDataSourceColumn(row, 0));

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

            return columnList;
        }

        /// <summary>
        /// Fills the data source column.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static DataSourceColumn FillDataSourceColumn(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            DataSourceColumn dataSourceColumn = null;

            if (row != null)
            {
                dataSourceColumn = new DataSourceColumn();
                int DataSourceColumnID = offset + 0;
                int ColumnName = offset + 1;
                int Type = offset + 2;
                int ApplicationColumnId = offset + 3;
                int RequiredInUpload = offset + 4;
                int ColumnSequenceNo = offset + 5;

                try
                {
                    dataSourceColumn.DataSourceColumnID = Convert.ToInt32(row[DataSourceColumnID]);
                    dataSourceColumn.ColumnName = Convert.ToString(row[ColumnName]);
                    dataSourceColumn.ColumnType = Convert.ToInt16(row[Type]);
                    dataSourceColumn.ApplicationColumnID = Convert.ToInt32(row[ApplicationColumnId]);
                    dataSourceColumn.IsRequiredInUpload = Convert.ToBoolean(row[RequiredInUpload]);
                    dataSourceColumn.ColumnSequenceNumber = Convert.ToInt32(row[ColumnSequenceNo]);
                    dataSourceColumn.Type = ((SelectColumnsType)dataSourceColumn.ColumnType).ToString();


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

            return dataSourceColumn;
        }

        /// <summary>
        /// Saves the run upload file data.
        /// </summary>
        /// <param name="uploadData">The upload data.</param>
        /// <returns></returns>
        public static string SaveRunUploadFileData(string tableName, string values)
        {
            // string dbMessage = string.Empty;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveDataSourceFileData";
                queryData.DictionaryDatabaseParameter.Add("@Values", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Values",
                    ParameterType = DbType.String,
                    ParameterValue = values
                });
                queryData.DictionaryDatabaseParameter.Add("@TableName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TableName",
                    ParameterType = DbType.String,
                    ParameterValue = tableName
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                //To do: execute commmand ...
                //int result = CommonManager.SaveThroughXML(xml, "PMSaveDataSourceFileData", tableName);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);

                ////CommonManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);
                return _errorMessage;
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

            return _errorMessage;
        }

        public static int SaveRunUploadFileDataNew(List<PositionMaster> positionMasterCollection)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(positionMasterCollection);

                return XMLSaveManager.SaveThroughXML("P_SaveAndUpdatePositionMaster", xml);

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
        public static int SaveRunUploadFileDataForCollateral(DataTable CollateralVariable)
        {
            int rowcount = 0;
            try
            {
                foreach (DataRow row in CollateralVariable.Rows)
                {
                    DataTable dt = CollateralVariable.Clone();
                    dt.LoadDataRow(row.ItemArray, true);
                    string xml = XMLUtilities.SerializeToXML(dt);
                    XMLSaveManager.SaveThroughXML("[SaveDataCollateralInterest]", xml);
                    rowcount += 1;
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
            return rowcount;
        }
        public static int SaveRunUploadFileDataForCash(List<CashCurrencyValue> cashCurrencyCollection)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(cashCurrencyCollection);

                return XMLSaveManager.SaveThroughXML("[PM_SaveCompanyAccountCashCurrencyValue]", xml);

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

        //Created By: Pooja Porwal
        //Date:12 Feb 2015
        //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-5820

        /// <summary>
        /// Save Run Upload File Data For SettlementDate Cash From xml in DB
        /// </summary>
        /// <param name="SaveRunUploadFileDataForSettlementDateCash"> SettlementDateCashCurrencyValue Class type</param>
        /// <returns>0/1 0 if no error and 1 for Error</returns>
        public static int SaveRunUploadFileDataForSettlementDateCash(List<SettlementDateCashCurrencyValue> settlementDateCashCurrencyCollection)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(settlementDateCashCurrencyCollection);

                return XMLSaveManager.SaveThroughXML("PM_SaveCompanyAccountCashCurrencyValueOnSettleDate", xml);

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

        public static int SaveRunUploadFileDataForMarkPrice(List<MarkPriceImport> markPriceImportCollection)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(markPriceImportCollection);

                return XMLSaveManager.SaveThroughXML("[PM_SaveMarkPrices]", xml);

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

        public static string GetOldValues()
        {
            try
            {
                return XMLSaveManager.OldValues;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public static int SaveRunUploadFileDataForForexPrice(List<ForexPriceImport> forexPriceImportCollection)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(forexPriceImportCollection);
                return XMLSaveManager.SaveThroughXML("[PMSaveForexRate_Import]", xml);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        public static void UpdateOMI(int rowsupdated, ref bool OMIImport)
        {
            try
            {
                if (rowsupdated > 0 && DataImport_OMI != null)
                {
                    DataImport_OMI(null, null);
                    OMIImport = true;
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

        public static List<string> GetCurrencyStandardPairs()
        {
            List<string> currencystandardPairs = new List<string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetCurrencyStandardPairs";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != System.DBNull.Value && (row[1] != System.DBNull.Value))
                        {
                            int fromCurrencyID = int.Parse(row[0].ToString());
                            int toCurrencyID = int.Parse(row[1].ToString());
                            string pairID = fromCurrencyID + Seperators.SEPERATOR_7 + toCurrencyID;
                            if (!currencystandardPairs.Contains(pairID))
                            {
                                currencystandardPairs.Add(pairID);
                            }
                        }
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

            return currencystandardPairs;

        }

        /// <summary>
        /// Gets the run upload data by company ID.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <returns></returns>
        public static RunUploadList GetRunUploadDataByCompanyID(int companyID, bool isShortLocateAllowed = false)
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetRunUploadDataByCompanyID";
            queryData.DictionaryDatabaseParameter.Add("@PMCompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@PMCompanyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });

            RunUploadList runUploadList = new RunUploadList();

            XMLSaveManager.AddOutErrorParameters(queryData);
            //string errorMessage = string.Empty;

            try
            {

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        //object[] row = new object[reader.FieldCount];
                        //reader.GetValues(row);
                        RunUpload runUploadDetails = new RunUpload();
                        //Modified By Faisal Shah on 01/09/14
                        //We needed to remove dependency on Indices and fill details with Column Names
                        runUploadDetails.CompanyNameIDValue.ID = Convert.ToInt32(reader["PMCOMPANYID"].ToString());
                        runUploadDetails.CompanyNameIDValue.ShortName = Convert.ToString(reader["COMPANYSHORTNAME"]);
                        runUploadDetails.DataSourceNameIDValue.ID = Convert.ToInt32(reader["ThirdPartyID"].ToString());
                        runUploadDetails.DataSourceNameIDValue.ShortName = Convert.ToString(reader["THIRDPARTYSHORTNAME"]);
                        runUploadDetails.FTPServer = Convert.ToString(reader["FTPSERVER"]);
                        runUploadDetails.FTPWatcherFilePath = Convert.ToString(reader["FTPFilePath"]);
                        runUploadDetails.LastImportedFile = Convert.ToString(reader["LastImportedFile"]);
                        runUploadDetails.Port = Convert.ToInt32(reader["FTPPORT"].ToString());
                        runUploadDetails.UserName = Convert.ToString(reader["FTPUSERNAME"]);
                        runUploadDetails.Password = Convert.ToString(reader["FTPPASSWORD"]);
                        runUploadDetails.AutoTime = Convert.ToDateTime(reader["AUTOTIME"].ToString());
                        runUploadDetails.DirPath = (reader["DIRECTORYPATH"] != DBNull.Value) ? Convert.ToString(reader["DIRECTORYPATH"]) : String.Empty;
                        runUploadDetails.FileName = (reader["FILENAME"] != DBNull.Value) ? Convert.ToString(reader["FILENAME"]) : String.Empty;
                        runUploadDetails.AutoImport = Convert.ToBoolean(reader["AUTOIMPORT"]);
                        //runUploadDetails.LastRunUploadDate = row[LASTRUNUPLOADDATE] ?? null; DateTime.Parse((row[LASTRUNUPLOADDATE].ToString()));
                        if (!(reader["LASTRUNUPLOADDATE"] is DBNull))
                        {
                            runUploadDetails.LastRunUploadDate = DateTime.Parse((reader["LASTRUNUPLOADDATE"].ToString()));
                        }
                        else
                        {
                            runUploadDetails.LastRunUploadDate = null;
                        }
                        runUploadDetails.UploadID = Convert.ToInt32(reader["UPLOADID"].ToString());
                        runUploadDetails.HeaderIndex = Convert.ToInt32(reader["HeaderIndex"]);
                        runUploadDetails.FirstRecordIndex = Convert.ToInt32(reader["FirstRecordIndex"]);
                        runUploadDetails.Status = (RunUploadStatus)Convert.ToInt16(reader["FILESTATUS"]);
                        runUploadDetails.CompanyUploadSetupID = Convert.ToInt32(reader["COMPANYUPLOADSETUPID"].ToString());
                        runUploadDetails.TotalRecords = Convert.ToInt32(reader["TOTALRECORDS"].ToString());
                        runUploadDetails.TableTypeID = Convert.ToInt32(reader["TABLETYPEID"]);
                        if (!(reader["FILELAYOUTTYPE"] is DBNull))
                            runUploadDetails.FileLayoutType = (DataSourceFileLayout)Convert.ToInt16(reader["FILELAYOUTTYPE"]);
                        if (reader["DATASOURCEXSLT"] != System.DBNull.Value)
                        {
                            runUploadDetails.DataSourceXSLT = reader["DATASOURCEXSLT"].ToString();
                        }
                        if (reader["TableFormatName"] != System.DBNull.Value)
                        {
                            runUploadDetails.TableFormatName = reader["TableFormatName"].ToString();
                        }
                        if (reader["ImportTypeAcronym"] != System.DBNull.Value)
                        {
                            runUploadDetails.ImportTypeAcronym = (ImportType)Enum.Parse(typeof(ImportType), reader["ImportTypeAcronym"].ToString(), true);
                        }

                        if (isShortLocateAllowed)
                        {
                            if (runUploadDetails.ImportTypeAcronym.Equals(ImportType.ShortLocate))
                                runUploadList.Add(runUploadDetails);
                        }
                        else
                        {
                            if (!runUploadDetails.ImportTypeAcronym.Equals(ImportType.ShortLocate))
                                runUploadList.Add(runUploadDetails);
                        }

                    }
                    //errorMessage = Convert.ToString(commandSP.Parameters["@ErrorMessage"].Value);
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
            return runUploadList;
        }

        public static int GetPMCompanyID(int companyID)
        {
            int pMComapnyID = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetPMCompanyID", parameter))
                {
                    if (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != System.DBNull.Value)
                        {
                            pMComapnyID = int.Parse(row[0].ToString());
                        }
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
            return pMComapnyID;
        }

        #region CommentedFunction FillRunUploadDetails()
        //Commented By Faisal Shah as we needed to remove dependency on Indexes
        /// <summary>
        /// Fills the run upload details.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        //private static RunUpload FillRunUploadDetails(object[] row)
        //{

        //    RunUpload runUploadDetails = null;

        //    if (row != null)
        //    {
        //        //runUploadDetails = new RunUpload();
        //        //int PMCOMPANYID = row[
        //        //int COMPANYSHORTNAME = offset + 1;
        //        //int THIRDPARTYID = offset + 2;
        //        //int DATASOURCESHORTNAME = offset + 3;
        //        //int FTPSERVER = offset + 4;
        //        //int FTPPORT = offset + 5;
        //        //int FTPUSERNAME = offset + 6;
        //        //int FTPPASSWORD = offset + 7;
        //        //int AUTOTIME = offset + 8;
        //        //int DIRECTORYPATH = offset + 9;
        //        //int FILENAME = offset + 10;
        //        //int AUTOIMPORT = offset + 11;
        //        //int LASTRUNUPLOADDATE = offset + 12;
        //        //int UPLOADID = offset + 13;
        //        //int HEADERROWNUMBER = offset + 14;
        //        //int FIRSTRECORDNUMBER = offset + 15;
        //        //int FILESTATUS = offset + 16;
        //        //int COMPANYUPLOADSETUPID = offset + 17;
        //        //int TOTALRECORDS = offset + 18;
        //        //int TABLETYPEID = offset + 19;
        //        //int FILELAYOUTTYPE = offset + 20;
        //        //int DATASOURCEXSLT = offset + 21;
        //        //int TableFormatName = offset + 22;
        //        //int ImportTypeAcronym = offset + 23;
        //        //try
        //        //{
        //        //    //runUploadDetails.CompanyID = Convert.ToInt32(row[PMCOMPANYID].ToString());
        //        //    runUploadDetails.CompanyNameIDValue.ID = Convert.ToInt32(row[PMCOMPANYID].ToString());
        //        //    runUploadDetails.CompanyNameIDValue.ShortName = Convert.ToString(row[COMPANYSHORTNAME]);
        //        //    runUploadDetails.DataSourceNameIDValue.ID = Convert.ToInt32(row[THIRDPARTYID].ToString());
        //        //    //runUploadDetails.ThirdPartyID = Convert.ToInt32(row[THIRDPARTYID].ToString());
        //        //    runUploadDetails.DataSourceNameIDValue.ShortName = Convert.ToString(row[DATASOURCESHORTNAME]);
        //        //    runUploadDetails.FTPServer = Convert.ToString(row[FTPSERVER]);
        //        //    runUploadDetails.Port = Convert.ToInt32(row[FTPPORT].ToString());
        //        //    runUploadDetails.UserName = Convert.ToString(row[FTPUSERNAME]);
        //        //    runUploadDetails.Password = Convert.ToString(row[FTPPASSWORD]);
        //        //    runUploadDetails.AutoTime = Convert.ToDateTime(row[AUTOTIME].ToString());
        //        //    runUploadDetails.DirPath = (row[DIRECTORYPATH] != DBNull.Value) ? Convert.ToString(row[DIRECTORYPATH]) : String.Empty;
        //        //    runUploadDetails.FileName = (row[FILENAME] != DBNull.Value) ?  Convert.ToString(row[FILENAME]) : String.Empty;
        //        //    runUploadDetails.AutoImport = Convert.ToBoolean(row[AUTOIMPORT]);
        //        //    //runUploadDetails.LastRunUploadDate = row[LASTRUNUPLOADDATE] ?? null; DateTime.Parse((row[LASTRUNUPLOADDATE].ToString()));
        //        //    if (!(row[LASTRUNUPLOADDATE] is DBNull))
        //        //    {
        //        //        runUploadDetails.LastRunUploadDate = DateTime.Parse((row[LASTRUNUPLOADDATE].ToString()));
        //        //    }
        //        //    else
        //        //    {
        //        //        runUploadDetails.LastRunUploadDate = null;
        //        //    }
        //        //    runUploadDetails.UploadID = Convert.ToInt32(row[UPLOADID].ToString());
        //        //    runUploadDetails.HeaderIndex = Convert.ToInt32(row[HEADERROWNUMBER]);
        //        //    runUploadDetails.FirstRecordIndex = Convert.ToInt32(row[FIRSTRECORDNUMBER]);
        //        //    runUploadDetails.Status = (RunUploadStatus)Convert.ToInt16(row[FILESTATUS]);
        //        //    runUploadDetails.CompanyUploadSetupID = Convert.ToInt32(row[COMPANYUPLOADSETUPID].ToString());
        //        //    runUploadDetails.TotalRecords = Convert.ToInt32(row[TOTALRECORDS].ToString());
        //        //    runUploadDetails.TableTypeID = Convert.ToInt32(row[TABLETYPEID]);
        //        //    if (!(row[FILELAYOUTTYPE] is DBNull))
        //        //        runUploadDetails.FileLayoutType = (DataSourceFileLayout)Convert.ToInt16(row[FILELAYOUTTYPE]);
        //        //    if (row[DATASOURCEXSLT] != System.DBNull.Value)
        //        //    {
        //        //        runUploadDetails.DataSourceXSLT = row[DATASOURCEXSLT].ToString();
        //        //    }
        //        //    if (row[TableFormatName] != System.DBNull.Value)
        //        //    {
        //        //        runUploadDetails.TableFormatName  = row[TableFormatName].ToString();
        //        //    }
        //        //    if (row[ImportTypeAcronym] != System.DBNull.Value)
        //        //    {
        //        //        runUploadDetails.ImportTypeAcronym = (ImportType)Enum.Parse(typeof(ImportType), row[ImportTypeAcronym].ToString(), true); 
        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{

        //        //    // Invoke our policy that is responsible for making sure no secure information
        //        //    // gets out of our layer.
        //        //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        //    if (rethrow)
        //        //    {
        //        //        throw;
        //        //    }
        //        //}
        //    }

        //    return runUploadDetails;
        //}

        #endregion


        /// <summary>
        /// Gets the allow daily import status for upload client.
        /// </summary>
        /// <param name="companyNameID">The company name ID.</param>
        /// <returns></returns>
        public static bool GetAllowDailyImportStatusForUploadClient(CompanyNameID companyNameID)
        {
            bool result = false;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllowDailyImportStatusForUploadClient";
            queryData.DictionaryDatabaseParameter.Add("@PMCompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@PMCompanyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyNameID.ID
            });
            queryData.DictionaryDatabaseParameter.Add("@Permission", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Permission",
                ParameterType = DbType.Boolean,
                ParameterValue = 16
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                result = Convert.ToBoolean(queryData.DictionaryDatabaseParameter["@Permission"].ParameterValue);
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


            return result;

        }

        //Save XSLT's From DB to PMImportXSLT Folder
        public static void SavePMImportXSLTfromDB(string startupPath)
        {
            string directoryPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + Prana.Global.ApplicationConstants.MappingFileType.PMImportXSLT.ToString();
            string path = startupPath + "\\" + directoryPath + "\\";
            DirectoryInfo dir = new DirectoryInfo(path);
            string allFileDetails = string.Empty;
            char seperator1 = Seperators.SEPERATOR_5;
            char seperator2 = Seperators.SEPERATOR_6;
            FileStream fs = null;

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (FileInfo f in dir.GetFiles("*.*"))
                {
                    string fileDetails = f.Name + seperator2 + f.LastWriteTimeUtc.ToString();
                    if (allFileDetails != string.Empty)
                        allFileDetails = allFileDetails + seperator1 + fileDetails;
                    else
                        allFileDetails = fileDetails;
                }

                object[] parameter = new object[3];
                parameter[0] = allFileDetails;
                parameter[1] = seperator1;
                parameter[2] = seperator2;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("PMGetRunUploadXSLTFileData", parameter))
                {
                    while (reader.Read())
                    {

                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string fileName = row[0].ToString();
                        byte[] data = Convert.FromBase64String(row[1].ToString());
                        //byte[] data = (byte[])row[1];
                        fs = new FileStream(path + "\\" + fileName, FileMode.Create);
                        fs.Write(data, 0, data.Length);

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
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }

        #region Recon XSLT Setup Methods

        public static List<ReconDataSource> GetCompanyDataSourceNames()
        {
            List<ReconDataSource> dataSourceList = new List<ReconDataSource>();
            object[] parameter = new object[1];

            parameter[0] = 5;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("PMGetCompanyDataSourceNames", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceList.Add(FillDataSourcesNameID(row, 0));
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
            return dataSourceList;
        }

        private static ReconDataSource FillDataSourcesNameID(object[] row, int offset)
        {
            ReconDataSource dataSource = null;

            try
            {
                if (row != null)
                {
                    dataSource = new ReconDataSource();

                    int THIRDPARTYID = offset + 0;
                    int THIRDPARTYNAME = offset + 1;
                    int THIRDPARTYSHORTNAME = offset + 2;

                    dataSource.ThirdPartyID = Convert.ToInt32(row[THIRDPARTYID].ToString());
                    dataSource.DataSourceName = Convert.ToString(row[THIRDPARTYNAME]);
                    dataSource.DataSourceShortName = Convert.ToString(row[THIRDPARTYSHORTNAME]);
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

            return dataSource;
        }

        public static ReconSetups GetReconXSLTSetupDetails()
        {
            ReconSetups reconSetups = new ReconSetups();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetReconXSLTSetupDetails";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            reconSetups.Add(FillReconXSLTSetupDetails(row, 0));
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
            return reconSetups;
        }

        private static ReconSetup FillReconXSLTSetupDetails(object[] row, int offset)
        {
            ReconSetup reconSetup = new ReconSetup();

            int COL_ReconThirdPartyID = offset + 0;
            int COL_ThirdPartyID = offset + 1;
            int COL_ReconTypeID = offset + 2;
            int COL_FormatType = offset + 3;
            int COL_XSLTID = offset + 4;
            int COL_XSLTName = offset + 5;

            try
            {
                reconSetup.ReconThirdPartyID = Convert.ToInt32((row[COL_ReconThirdPartyID]).ToString());
                reconSetup.ThirdPartyID = Convert.ToInt32((row[COL_ThirdPartyID]).ToString());
                reconSetup.ReconTypeID = Convert.ToInt32((row[COL_ReconTypeID]).ToString());
                reconSetup.ReconType = (ReconType)Convert.ToInt32((row[COL_ReconTypeID]).ToString());
                reconSetup.FormatType = Convert.ToString(row[COL_FormatType]);
                reconSetup.XSLTID = Convert.ToInt32((row[COL_XSLTID]).ToString());
                reconSetup.XSLTName = Convert.ToString(row[COL_XSLTName]);
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

            return reconSetup;
        }

        public static void SaveReconXSLTDetails(Prana.PM.BLL.ReconSetups reconSetups)
        {
            object[] parameter = new object[9];

            try
            {
                foreach (ReconSetup reconSetup in reconSetups)
                {
                    byte[] binaryData = TransformToBinary(reconSetup.XSLTName);
                    reconSetup.XSLTName = GetFileNameFromPath(reconSetup.XSLTName);

                    parameter[0] = reconSetup.ReconThirdPartyID;
                    parameter[1] = reconSetup.ThirdPartyID;
                    parameter[2] = reconSetup.ReconTypeID;
                    parameter[3] = reconSetup.FormatType;
                    parameter[4] = reconSetup.XSLTID;
                    parameter[5] = reconSetup.XSLTName;
                    parameter[6] = binaryData;
                    parameter[7] = DateTime.UtcNow;
                    parameter[8] = Convert.ToInt32(Prana.Global.ApplicationConstants.MappingFileType.ReconXSLT);

                    DatabaseManager.DatabaseManager.ExecuteScalar("[PMSaveReconXSLTSetupDetails]", parameter);
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

        private static string GetFileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        private static byte[] TransformToBinary(string path)
        {
            if (path != "" && path.Contains("\\"))
            {
                FileStream fs = null;
                BinaryReader br = null;
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] data = new byte[fs.Length];

                try
                {
                    br = new BinaryReader(fs);
                    int length = (int)fs.Length;
                    br.Read(data, 0, length);
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
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        //br.Close();
                    }
                }
                return data;
            }
            else
            {
                return null;
            }
        }

        public static void RemoveReconXSLTDetailEntry(int reconThirdPartyID, int xsltID)
        {
            object[] parameter = new object[2];

            parameter[0] = reconThirdPartyID;
            parameter[1] = xsltID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteScalar("PMDeleteReconXSLTSetupDetails", parameter);
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

        #endregion

    }
}
