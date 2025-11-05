using Cryptography.GnuPG;
using Prana.AllocationProcessor;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.FixEngineConnectionManager;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.ThirdPartyManager.DataAccess;
using Prana.ThirdPartyManager.Helper;
using Prana.ThirdPartyManager.Helpers;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static Prana.Global.ApplicationConstants;

namespace Prana.ThirdPartyManager.BusinessLogic
{
    public static class ThirdPartyLogic
    {
        private static ProxyBase<IPublishing> _proxyPublishing = null;
        private static readonly object _publishLock = new object();
        private const string CONST_TIME_FORMAT = "HH:mm:ss";


        static ThirdPartyLogic()
        {
            PranaAllocationManager.NewEODMessageReceived += HandleNewEODMessage;
            CreatePublishingProxy();
        }
        /// <summary>
        /// Gets the third batches.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ThirdPartyBatches GetThirdPartyBatch()
        {
            return ThirdPartyDataManager.GetData<ThirdPartyBatches, ThirdPartyBatch>("P_GetThirdPartyBatch", -1);
        }

        /// <summary>
        /// Gets the third batches.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ThirdPartyBatch GetThirdPartyBatch(int batchId)
        {
            ThirdPartyBatch thirdPartyBatch = null;
            try
            {
                ThirdPartyBatches thirdPartyBatches = ThirdPartyDataManager.GetData<ThirdPartyBatches, ThirdPartyBatch>("P_GetThirdPartyBatch", batchId);
                if (thirdPartyBatches != null && thirdPartyBatches.Count > 0)
                    thirdPartyBatch = (ThirdPartyBatch)thirdPartyBatches[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyBatch;
        }

        /// <summary>
        /// This method gets the third party batch by its ThirdPartyBatchId
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public static ThirdPartyBatch GetThirdPartyBatchById(int batchId)
        {
            try
            {
                ThirdPartyBatches thirdPartyBatches = ThirdPartyLogic.GetThirdPartyBatch();
                foreach (ThirdPartyBatch batch in thirdPartyBatches)
                {
                    if (batch.ThirdPartyBatchId == batchId)
                    {
                        return batch;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the third party batches.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ThirdPartyBatches LoadThirdPartyBacthes(DateTime runDate)
        {

            ThirdPartyBatches batches = GetThirdPartyBatch();

            List<ThirdPartyFtp> ftps = ThirdPartyDataManager.GetThirdPartyFtps();
            List<ThirdPartyGnuPG> gnuPGs = ThirdPartyDataManager.GetThirdPartyGnuPG();
            List<ThirdPartyEmail> emails = ThirdPartyDataManager.GetThirdPartyEmail();
            DateTime currentESTTime = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, BusinessObjects.TimeZoneInfo.EasternTimeZone);

            InitiateAllocationBlockCache(runDate);
            InitiateFileStatusCache(runDate);
            foreach (ThirdPartyBatch batch in batches)
            {
                batch.Ftp = ThirdPartyDataManager.GetThirdPartyFtp(ftps, batch.FtpId);
                batch.GnuPG = ThirdPartyDataManager.GetThirdPartyGnuPG(gnuPGs, batch.GnuPGId);
                batch.EmailData = ThirdPartyDataManager.GetThirdPartyEmail(emails, batch.EmailDataId);
                batch.EmailLog = ThirdPartyDataManager.GetThirdPartyEmail(emails, batch.EmailLogId);
                batch.ThirdPartyShortName = CachedDataManager.GetInstance.GetThirdPartyShortNameByID(batch.ThirdPartyId);
                if (batch.AllowedFixTransmission != null && batch.AllowedFixTransmission.Value)
                {
                    batch.TransmissionType = ((int)TransmissionType.FIX).ToString();
                    if (ThirdPartyCache.DateWiseAllocationBlockDetails[runDate.ToShortDateString()].ContainsKey(batch.ThirdPartyBatchId))
                    {
                        batch.AllocationMatchStatus = ThirdPartyCache.DateWiseAllocationBlockDetails[runDate.ToShortDateString()][batch.ThirdPartyBatchId].AllocationMatchStatus.ToString();
                    }
                    else
                        batch.AllocationMatchStatus = AllocationMatchStatus.NotSent.ToString();
                }
                else if (batch.FileEnabled)
                {
                    batch.TransmissionType = ((int)TransmissionType.File).ToString();
                    if (ThirdPartyCache.DateWiseFileStatusDetails.ContainsKey(runDate.ToShortDateString())
                        && ThirdPartyCache.DateWiseFileStatusDetails[runDate.ToShortDateString()].Contains(batch.ThirdPartyBatchId))
                    {
                        batch.AllocationMatchStatus = AllocationMatchStatus.CompleteMatch.ToString();
                    }
                    else
                    {
                        batch.AllocationMatchStatus = AllocationMatchStatus.NotSent.ToString();
                    }
                }

                if(runDate.Date != currentESTTime.Date)
                {
                    batch.FixAutomatedBatchStatus = ThirdPartyConstants.CONST_AUTOMATED_BATCH_STATUS_NA;
                }
                else if (ThirdPartyCache.AutomatedBatchStatus.ContainsKey(batch.ThirdPartyBatchId))
                {
                    batch.FixAutomatedBatchStatus = ThirdPartyCache.AutomatedBatchStatus[batch.ThirdPartyBatchId];
                }
                else
                {
                    batch.FixAutomatedBatchStatus = ThirdPartyConstants.CONST_AUTOMATED_BATCH_STATUS_NO_BATCH_SET;
                }
            }
            return batches;
        }

        public static ThirdParty GetThirdParty(int thirdPartyID)
        {
            try
            {
                return ThirdPartyDataManager.GetThirdParty(thirdPartyID);
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

        public static ThirdPartyFtp GetThirdPartyFtp(int ftpId)
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyFtp(ftpId);
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

        public static ThirdPartyGnuPGs GetThirdPartyGnuPGForDecryption(int gnuPGId)
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyGnuPGForDecryption(gnuPGId);
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

        public static ThirdPartyEmail GetThirdPartyEmail(int ftpId)
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyEmail(ftpId);
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

        public static List<ThirdPartyType> GetThirdPartyTypes()
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyTypes();
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

        public static List<ThirdParty> GetCompanyThirdParties_DayEnd(int companyID)
        {
            try
            {
                return ThirdPartyDataManager.GetCompanyThirdParties_DayEnd(companyID);
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

        public static List<ThirdParty> GetCompanyThirdPartyTypeParty(int companyID, int thirdPartyTypeID)
        {
            try
            {
                return ThirdPartyDataManager.GetCompanyThirdPartyTypeParty(companyID, thirdPartyTypeID);
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

        public static List<ThirdPartyPermittedAccount> GetThirdPartyPermittedAccounts(int thirdPartyID)
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyPermittedAccounts(thirdPartyID);
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

        public static List<ThirdPartyPermittedAccount> GetThirdPartyAccounts(int companyID, int userID)
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyAccounts(companyID, userID);
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

        public static List<ThirdPartyFileFormat> GetCompanyThirdPartyFileFormats(int companyThirdPartyId)
        {
            try
            {
                return ThirdPartyDataManager.GetCompanyThirdPartyFileFormats(companyThirdPartyId);
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

        public static List<ThirdPartyFileFormat> GetCompanyThirdPartyTypeFileFormats(int companyThirdPartyId)
        {
            try
            {
                return ThirdPartyDataManager.GetCompanyThirdPartyTypeFileFormats(companyThirdPartyId);
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
        /// Gets the account ids.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static StringBuilder GetAccountIds(ThirdPartyBatch batch)
        {
            StringBuilder accounts = new StringBuilder();
            try
            {
                List<ThirdPartyPermittedAccount> _accounts = null;

                if (batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                    _accounts = ThirdPartyDataManager.GetThirdPartyPermittedAccounts(batch.ThirdPartyCompanyId);
                else if (batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker
                    || batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties)
                    _accounts = ThirdPartyDataManager.GetThirdPartyAccounts(batch.CompanyId, batch.UserId);

                var dictUserAccounts = WindsorContainerManager.GetAccounts(batch.UserId);


                // Added by Sunil Sharma, required as per http://jira.nirvanasolutions.com:8080/browse/PRANA-12432
                List<ThirdPartyPermittedAccount> toRemove = new List<ThirdPartyPermittedAccount>();

                foreach (ThirdPartyPermittedAccount value in _accounts)
                {
                    if (!dictUserAccounts.Contains(value.CompanyAccountID))
                    {
                        toRemove.Add(value);
                        continue;
                    }
                    accounts.Append(value.CompanyAccountID + ",");
                }

                foreach (ThirdPartyPermittedAccount account in toRemove)
                {
                    _accounts.Remove(account);
                }

                toRemove.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accounts;
        }

        /// <summary>
        /// Gets the comma-separated account IDs.
        /// </summary>
        public static StringBuilder GetAccountIds()
        {
            StringBuilder accounts = new StringBuilder();
            try
            {
                AccountCollection dictAccounts = WindsorContainerManager.GetAccounts();

                foreach (Account value in dictAccounts)
                {
                    accounts.Append(value.AccountID + ",");
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
            return accounts;
        }

        /// <summary>Updates the data set for fix.</summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="batch">The batch.</param>
        internal static void UpdateDataSetForFIX(DataSet dataSet, ThirdPartyBatch batch)
        {
            try
            {
                if (dataSet != null && dataSet.Tables.Count == 1 && batch.TransmissionType == ((int)TransmissionType.FIX).ToString())
                {
                    SerializableDictionary<string, List<ThirdPartyOrderDetail>> orderDetail = new SerializableDictionary<string, List<ThirdPartyOrderDetail>>();
                    DataTable dt = dataSet.Tables[0];
                    DataColumn entityId = dt.Columns[1];
                    entityId.SetOrdinal(dt.Columns.Count - 1);
                    entityId.ColumnMapping = MappingType.Attribute;
                    dt.Columns.RemoveAt(0);
                    DataColumn col = dt.Columns.Add("Group_Id", typeof(string));
                    col.SetOrdinal(0);
                    col.ColumnMapping = MappingType.Hidden;
                    DataTable groupTable = dt.Clone();
                    groupTable.TableName = "Group";
                    DataTable taxlotTable = dt.Clone();
                    int count = 0;
                    Dictionary<string, int> groupIds = new Dictionary<string, int>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string allocId = row["AllocID"].ToString();
                        string account = row["AllocAccount"].ToString();
                        if (string.IsNullOrEmpty(account))
                        {
                            if (!orderDetail.ContainsKey(allocId))
                            {
                                orderDetail[allocId] = new List<ThirdPartyOrderDetail>();
                                groupTable.ImportRow(row);
                            }
                            ThirdPartyOrderDetail thirdPartyGroupedOrderDetail = new ThirdPartyOrderDetail();
                            thirdPartyGroupedOrderDetail.OrderID = row["OrderID"].ToString();
                            thirdPartyGroupedOrderDetail.CLOrderID = row["CLOrderID"].ToString();
                            orderDetail[allocId].Add(thirdPartyGroupedOrderDetail);
                        }
                        else
                            taxlotTable.ImportRow(row);
                        if (!groupIds.ContainsKey(allocId))
                        {
                            groupIds.Add(allocId, count++);
                        }
                    }
                    foreach (DataRow row in taxlotTable.Rows)
                    {
                        string allocId = row["AllocID"].ToString();
                        row[0] = groupIds[allocId];
                    }
                    foreach (DataRow row in groupTable.Rows)
                    {
                        string allocId = row["AllocID"].ToString();
                        row[0] = groupIds[allocId];
                        row["NoOrders"] = orderDetail[allocId].Count.ToString();
                        if (orderDetail[allocId].Count > 1)
                        {
                            row["OrderID"] = string.Empty;
                            row["CLOrderID"] = string.Empty;
                        }
                    }
                    dataSet.Tables.Clear();
                    dataSet.Tables.Add(groupTable);
                    dataSet.Tables.Add(taxlotTable);
                    DataRelation dr = new DataRelation("Group_ThirdPartyFlatFileDetail", groupTable.Columns["Group_Id"], taxlotTable.Columns["Group_Id"]);
                    dr.Nested = true;
                    dataSet.Relations.Add(dr);
                    batch.OrderDetail = orderDetail;
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
        /// Generates the XML.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataSet GenerateXML(ThirdPartyBatch batch, ThirdPartyFlatFileDetailCollection details, ThirdPartyFileFormat format, EventHandler<MessageEventArgs> OnMessage)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());

            ThirdPartyGenerator gen = new ThirdPartyGenerator();
            gen.Message += OnMessage;
            var data = gen.GenerateXML(batch, details, format);
            gen.Message -= OnMessage;
            return data.Tables.Count == 0 ? ds : data;
        }

        public static DataSet UpdateCaptions(DataSet ds)
        {
            try
            {
                if (ds == null) return null;

                string captionChangeRequiredValue = string.Empty;
                bool captionChangeRequiredExists = ds.Tables[0].Columns.Contains("IsCaptionChangeRequired");
                if (captionChangeRequiredExists)
                {
                    if (ds.Tables[0].Columns.Contains("TaxLotState") && (ds.Tables[0].Rows[0]["TaxLotState"].ToString().ToUpper() == "TAXLOTSTATE" || ds.Tables[0].Rows[0]["TaxLotState"].ToString().ToUpper() == "TRUE"))
                    {
                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            captionChangeRequiredValue = ds.Tables[0].Rows[1]["IsCaptionChangeRequired"].ToString();

                            if (!String.IsNullOrEmpty(captionChangeRequiredValue) && captionChangeRequiredValue.Trim().ToUpper().Equals("TRUE"))
                            {
                                DataTable dtCaption = ds.Tables[0].Copy();
                                foreach (DataColumn col in ds.Tables[0].Columns)
                                {
                                    DataRow rowCaption = dtCaption.Rows[0];
                                    foreach (DataColumn captionCol in dtCaption.Columns)
                                    {
                                        if (captionCol.ColumnName.Equals(col.ColumnName) && !ColumnExists(col.ColumnName) && !col.ColumnName.Equals("TaxlotState"))
                                        {
                                            string temp = rowCaption[captionCol.ColumnName].ToString();
                                            if (!string.IsNullOrWhiteSpace(temp))
                                                col.ColumnName = temp;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        ds.Tables[0].Rows[0].Delete();
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
            return ds;
        }

        private static bool ColumnExists(string colName)
        {
            if (colName.Equals("CompanyID") || colName.Equals("ThirdPartyID") || colName.Equals("CompanyAccountID") ||
                colName.Equals("AssetID") || colName.Equals("UnderLyingID") || colName.Equals("CurrencyID") ||
                colName.Equals("ExchangeID") || colName.Equals("AUECID") || colName.Equals("CompanyAccountTypeID") ||
                colName.Equals("CommissionRateTypeID") || colName.Equals("ThirdPartyTypeID") || colName.Equals("CompanyCVID") ||
                colName.Equals("VenueID") || colName.Equals("EntityID") || colName.Equals("CounterPartyID") ||
                colName.Equals("TradAccntID") || colName.Equals("GroupEnds") || colName.Equals("GroupAllocationReq") ||
                colName.Equals("FileHeader") || colName.Equals("FileFooter") || colName.Equals("PBUniqueID") ||
                colName.Equals("RowHeader") || colName.Equals("TaxLotStateID") || colName.ToUpper().Equals("ALLOCQTY") ||
                colName.Equals("TaxLots_Id") || colName.Equals("Group_Id") || colName.Equals("TaxLots_ThirdPartyFlatFileDetail") ||
                colName.Equals("TaxLotState1") || colName.Equals("IsCaptionChangeRequired") || colName.Equals("FromDeleted"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the AUEC ids.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static StringBuilder GetAUECIds()
        {
            StringBuilder auecIds = new StringBuilder();
            try
            {
                auecIds = ThirdPartyDataManager.GetAUECIds(CommonDataCache.CachedDataManager.GetInstance.GetCompanyID());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return auecIds;
        }

        public static int SaveThirdPartyFtp(ThirdPartyFtp ftp)
        {
            try
            {
                return ThirdPartyDataManager.SaveThirdPartyFtp(ftp);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return int.MinValue;
        }

        public static int SaveThirdPartyGnuPG(ThirdPartyGnuPG gnuPG)
        {
            try
            {
                return ThirdPartyDataManager.SaveThirdPartyGnuPG(gnuPG);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return int.MinValue;
        }

        /// <summary>
        /// This method is to check for unallocated trades
        /// </summary>
        /// <param name="tradedate"></param>
        /// <returns>true if there are unallocated trades, otherwise false</returns>
        public static bool CheckForUnallocatedTrades(DateTime tradedate)
        {
            try
            {
                return ThirdPartyDataManager.CheckForUnallocatedTrades(tradedate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <returns></returns>
        public static List<ThirdPartyForceConfirm> GetThirdPartyForceConfirmAuditData(int thirdPartyBatchId, DateTime runDate)
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyForceConfirmAuditData(thirdPartyBatchId, runDate);
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
        /// Saves Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <returns></returns>
        public static int SaveThirdPartyForceConfirmAuditData(List<ThirdPartyForceConfirm> dataSaveList, ThirdPartyBatch batch)
        {
            try
            {
                return ThirdPartyDataManager.SaveThirdPartyForceConfirmAuditData(dataSaveList);
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

        /// <summary>
        /// This method is to get Force Confirm Details
        /// </summary>
        /// <param name="allocationId"></param>
        /// <param name="broker"></param>
        /// <returns></returns>
        public static List<ThirdPartyForceConfirm> GetForceConfirmDetails(string allocationId, string broker)
        {
            List<ThirdPartyForceConfirm> result = new List<ThirdPartyForceConfirm>();
            try
            {
                var forceConfirmDetails = ThirdPartyDataManager.GetForceConfirmDetails(allocationId);
                foreach (DataRow row in forceConfirmDetails.Tables[0].Rows)
                {
                    var forceConfirmDetail = CreateForceConfirmData(row);
                    forceConfirmDetail.Broker = broker;
                    result.Add(forceConfirmDetail);
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
            return result;
        }

        /// <summary>
        /// This method gets taxlots which are eligible to be force matched/ reject and Send AU msg for them
        /// </summary>
        /// <param name="allocationIdList"></param>
        /// <param name="counterPartyID"></param>
        /// <param name="affirmStatus"></param>
        /// <returns></returns>
        public static bool GetRequiredTaxlotsAndSendAUMsg(List<string> allocationIdList, int counterPartyID, int affirmStatus)
        {
            try
            {
                DataTable requiredTaxlots = ThirdPartyDataManager.GetRequiredTaxlotsForAU(allocationIdList).Tables[0];
                Dictionary<string, List<RepeatingMessageFieldCollection>> messageFieldsByAllocId = new Dictionary<string, List<RepeatingMessageFieldCollection>>();
                foreach (DataRow row in requiredTaxlots.Rows)
                {
                    string allocId = row[3].ToString();
                    if (!messageFieldsByAllocId.ContainsKey(allocId))
                    {
                        messageFieldsByAllocId.Add(allocId, new List<RepeatingMessageFieldCollection>());
                    }
                    RepeatingMessageFieldCollection messageFieldCollection = new RepeatingMessageFieldCollection();
                    messageFieldCollection.AddField(FIXConstants.TagConfirmID, row[0].ToString());
                    messageFieldCollection.AddField(FIXConstants.TagConfirmStatus, row[1].ToString());
                    DateTime tradeDate = ((DateTime)row[2]);
                    messageFieldCollection.AddField(FIXConstants.TagTradeDate, tradeDate.ToString("yyyyMMdd"));
                    DateTime PMsgTransactTime = ((DateTime)row[4]);
                    messageFieldCollection.AddField(FIXConstants.TagTransactTime, PMsgTransactTime.ToString(DateTimeConstants.NirvanaDateTimeFormat));
                    messageFieldsByAllocId[allocId].Add(messageFieldCollection);
                }
                if (messageFieldsByAllocId.Count > 0)
                {
                    ThirdPartyFixManager.SendAUMessage(messageFieldsByAllocId, counterPartyID, affirmStatus);
                    return true;
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
            return false;
        }

        /// <summary>
        /// This method sends AT msg for specified allocId and allocStatus
        /// </summary>
        /// <param name="allocIdAllocReportIdPairs"></param>
        /// <param name="counterPartyID"></param>
        /// <param name="allocStatus"></param>
        /// <returns></returns>
        public static bool GetRequiredBlocksAndSendATMsg(Dictionary<string, string> allocIdAllocReportIdPairs, int counterPartyID, int allocStatus)
        {
            try
            {
                if (allocStatus == (int)BlockMatchStatus.Incomplete)
                {
                    DataTable rejectableBlocks = ThirdPartyDataManager.GetRejectableBlocksForAT(allocIdAllocReportIdPairs.Keys.ToList<string>()).Tables[0];
                    allocIdAllocReportIdPairs.Clear();
                    foreach (DataRow row in rejectableBlocks.Rows)
                    {
                        string allocId = string.Empty;
                        string allocReportId = string.Empty;
                        if (!(row[0] is DBNull))
                        {
                            allocId = row[0].ToString();
                        }
                        if (!(row[1] is DBNull))
                        {
                            allocReportId = row[1].ToString();
                        }
                        allocIdAllocReportIdPairs[allocId] = allocReportId;
                    }
                }
                if (allocIdAllocReportIdPairs.Count > 0)
                {
                    ThirdPartyFixManager.SendATMessage(allocIdAllocReportIdPairs, counterPartyID, allocStatus);
                    return true;
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
            return false;
        }
        /// <summary>
        /// This method is get the block level details for a third party batch
        /// </summary>
        /// <param name="thirdPartyBatchId"></param>
        /// <param name="runDate"></param>
        /// <returns>List of ThirdPartyBlockLevelDetails if available, otherwise empty list</returns>
        public static List<ThirdPartyBlockLevelDetails> GetBlockAllocationDetails(int thirdPartyBatchId, DateTime runDate)
        {
            List<ThirdPartyBlockLevelDetails> blockDetails = new List<ThirdPartyBlockLevelDetails>();
            try
            {
                var blockAllocationDetails = ThirdPartyDataManager.GetBlockAllocationDetails(thirdPartyBatchId, runDate);

                if (blockAllocationDetails != null)
                {
                    Dictionary<int, ThirdPartyBlockLevelDetails> blockLevelDetails = new Dictionary<int, ThirdPartyBlockLevelDetails>();
                    Dictionary<int, Dictionary<string, ThirdPartyAllocationLevelDetails>> blockWiseAllocationLevelDetails = new Dictionary<int, Dictionary<string, ThirdPartyAllocationLevelDetails>>();
                    var blockDataTable = blockAllocationDetails.Tables[0];
                    var allocationDataTable = blockAllocationDetails.Tables[1];
                    foreach (DataRow row in allocationDataTable.Rows)
                    {
                        int blockId = int.Parse(row["BlockId"].ToString());
                        DataRow blockDetailsForCurrentTaxlot = blockDataTable.AsEnumerable().FirstOrDefault(block => block.Field<int>("BlockId") == blockId);
                        var allocationLevelData = CreateAllocationLevelData(row, blockDetailsForCurrentTaxlot["AllocStatus"].ToString());
                        if (allocationLevelData != null && !string.IsNullOrEmpty(allocationLevelData.AccountAllocationID))
                        {
                            if (!blockWiseAllocationLevelDetails.ContainsKey(allocationLevelData.BlockDetailID))
                            {
                                blockWiseAllocationLevelDetails.Add(allocationLevelData.BlockDetailID, new Dictionary<string, ThirdPartyAllocationLevelDetails>());
                            }
                            if (!blockWiseAllocationLevelDetails[allocationLevelData.BlockDetailID].ContainsKey(allocationLevelData.AccountAllocationID))
                            {
                                blockWiseAllocationLevelDetails[allocationLevelData.BlockDetailID].Add(allocationLevelData.AccountAllocationID, allocationLevelData);
                            }
                            else
                            {
                                CreateAllocationDetailComparissons(blockWiseAllocationLevelDetails[allocationLevelData.BlockDetailID][allocationLevelData.AccountAllocationID], allocationLevelData);
                            }
                        }
                    }
                    foreach (DataRow row in blockDataTable.Rows)
                    {
                        var blockLevelData = CreateBlockLevelData(row);
                        if (blockLevelData != null && !string.IsNullOrEmpty(blockLevelData.AllocationID) && blockLevelData.MsgType.Equals(FIXConstants.MSGAllocationInstruction))
                        {
                            if (!blockLevelDetails.ContainsKey(blockLevelData.BlockDetailId))
                            {
                                if (blockWiseAllocationLevelDetails.ContainsKey(blockLevelData.BlockDetailId))
                                {
                                    blockLevelData.AllocationLevelDetails = blockWiseAllocationLevelDetails[blockLevelData.BlockDetailId].Values.ToList();
                                }
                                blockLevelDetails.Add(blockLevelData.BlockDetailId, blockLevelData);
                            }
                            else
                            {
                                blockLevelDetails[blockLevelData.BlockDetailId].MatchStatus = blockLevelData.MatchStatus;
                                blockLevelDetails[blockLevelData.BlockDetailId].SubStatus = blockLevelData.SubStatus;
                            }
                        }
                    }
                    blockDetails = blockLevelDetails.Values.ToList();
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
            return blockDetails;
        }

        /// <summary>
        /// This method is to create allocation detail comparisson
        /// </summary>
        /// <param name="nirvanaAllocations"></param>
        /// <param name="brokerAllocations"></param>
        private static void CreateAllocationDetailComparissons(ThirdPartyAllocationLevelDetails nirvanaAllocations, ThirdPartyAllocationLevelDetails brokerAllocations)
        {
            try
            {
                nirvanaAllocations.AllocationComparisons = new List<ThirdPartyAllocationDetailComparison>();
                nirvanaAllocations.AllocationComparisons.Add(new ThirdPartyAllocationDetailComparison()
                {
                    Item = "Account",
                    Nirvana = nirvanaAllocations.Account,
                    Broker = brokerAllocations.Account
                });
                nirvanaAllocations.AllocationComparisons.Add(new ThirdPartyAllocationDetailComparison()
                {
                    Item = "Average PX",
                    Nirvana = nirvanaAllocations.AveragePX,
                    Broker = brokerAllocations.AveragePX
                });
                nirvanaAllocations.AllocationComparisons.Add(new ThirdPartyAllocationDetailComparison()
                {
                    Item = "Net Money",
                    Nirvana = nirvanaAllocations.NetMoney,
                    Broker = brokerAllocations.NetMoney
                });
                nirvanaAllocations.AllocationComparisons.Add(new ThirdPartyAllocationDetailComparison()
                {
                    Item = "Quantity",
                    Nirvana = nirvanaAllocations.Quantity,
                    Broker = brokerAllocations.Quantity
                });
                nirvanaAllocations.AllocationComparisons.Add(new ThirdPartyAllocationDetailComparison()
                {
                    Item = "Commisson",
                    Nirvana = nirvanaAllocations.Commission,
                    Broker = brokerAllocations.Commission
                });
                nirvanaAllocations.AllocationComparisons.Add(new ThirdPartyAllocationDetailComparison()
                {
                    Item = "Misc Fees",
                    Nirvana = nirvanaAllocations.MiscFees,
                    Broker = brokerAllocations.MiscFees
                });
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
        /// This method is to create Force Confirm data
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static ThirdPartyForceConfirm CreateForceConfirmData(DataRow row)
        {
            try
            {
                var forceConfirmData = new ThirdPartyForceConfirm();

                if (row == null)
                {
                    return null;
                }

                if (!(row[0] is DBNull))
                    forceConfirmData.Account = row[0].ToString();
                if (!(row[1] is DBNull))
                    forceConfirmData.Symbol = row[1].ToString();
                if (!(row[2] is DBNull))
                    forceConfirmData.Side = GetOrderSide(row[2].ToString());
                if (!(row[3] is DBNull))
                    forceConfirmData.Quantity = row[3].ToString();
                if (!(row[4] is DBNull))
                    forceConfirmData.AveragePX = row[4].ToString();
                if (!(row[5] is DBNull))
                    forceConfirmData.Commission = row[5].ToString();
                if (!(row[6] is DBNull))
                    forceConfirmData.MiscFees = row[6].ToString();
                if (!(row[7] is DBNull))
                    forceConfirmData.NetMoney = row[7].ToString();
                if (!(row[8] is DBNull))
                    forceConfirmData.TradeDate = row[8].ToString();
                if (!(row[9] is DBNull))
                    forceConfirmData.AllocationID = row[9].ToString();
                if (!(row[10] is DBNull))
                {
                    int status = 0;
                    if (int.TryParse(row[10].ToString(), out status))
                    {
                        forceConfirmData.MatchStatus = EnumHelper.GetDescriptionWithDescriptionAttribute((AllocMatchStatus)status);
                    }
                    else
                    {
                        forceConfirmData.MatchStatus = row[10].ToString();
                    }
                }
                if (!(row[11] is DBNull))
                    forceConfirmData.ConfirmStatus = int.Parse(row[11].ToString());
                if (!(row[12] is DBNull))
                    forceConfirmData.BlockID = int.Parse(row[12].ToString());
                if (!(row[13] is DBNull))
                    forceConfirmData.MsgType = row[13].ToString();
                if (!(row[14] is DBNull))
                    forceConfirmData.AllocReportId = row[14].ToString();
                return forceConfirmData;
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
        /// This method is to create block level data
        /// </summary>
        /// <param name="row"></param>
        /// <returns>ThirdPartyBlockLevelDetails</returns>
        private static ThirdPartyBlockLevelDetails CreateBlockLevelData(DataRow row)
        {
            try
            {
                var blockData = new ThirdPartyBlockLevelDetails();

                if (row == null)
                {
                    return null;
                }

                if (!(row[0] is DBNull))
                    blockData.BlockDetailId = int.Parse(row[0].ToString());
                if (!(row[1] is DBNull))
                    blockData.LastUpdated = row[1].ToString();
                if (!(row[2] is DBNull))
                    blockData.Currency = row[2].ToString();
                if (!(row[3] is DBNull))
                    blockData.Symbol = row[3].ToString();
                if (!(row[4] is DBNull))
                    blockData.ISIN = row[4].ToString();
                if (!(row[5] is DBNull))
                    blockData.Sedol = row[5].ToString();
                if (!(row[6] is DBNull))
                    blockData.CUSIP = row[6].ToString();
                if (!(row[7] is DBNull))
                    blockData.GrossAmount = row[7].ToString();
                if (!(row[8] is DBNull))
                    blockData.Side = GetOrderSide(row[8].ToString());
                if (!(row[9] is DBNull))
                    blockData.Quantity = row[9].ToString();
                if (!(row[10] is DBNull))
                    blockData.TradeDate = row[10].ToString();
                if (!(row[11] is DBNull))
                    blockData.SettlementDate = row[11].ToString();
                if (!(row[12] is DBNull))
                    blockData.AveragePX = row[12].ToString();
                if (!(row[13] is DBNull))
                    blockData.Commission = row[13].ToString();
                if (!(row[14] is DBNull))
                    blockData.NetAmount = row[14].ToString();
                if (!(row[15] is DBNull))
                    blockData.MatchStatus = EnumHelper.GetDescriptionWithDescriptionAttribute((BlockMatchStatus)int.Parse(row[15].ToString()));
                if (!(row[16] is DBNull))
                    blockData.AllocationID = row[16].ToString();
                if (!(row[17] is DBNull))
                {
                    if (blockData.MatchStatus == "Rejected" || int.Parse(row[15].ToString()) == (int)BlockMatchStatus.MismatchedWithAllocRejCode)
                    {
                        blockData.SubStatus = EnumHelper.GetDescriptionWithDescriptionAttribute((AllocRejCode)int.Parse(row[17].ToString()));
                    }
                    else
                    {
                        blockData.SubStatus = EnumHelper.GetDescriptionWithDescriptionAttribute((BlockSubStatus)int.Parse(row[17].ToString()));
                    }
                }
                if (!(row[18] is DBNull))
                    blockData.AllocReportId = row[18].ToString();
                if (!(row[19] is DBNull))
                    blockData.MsgType = row[19].ToString();
                blockData.AllocationLevelDetails = new List<ThirdPartyAllocationLevelDetails>();
                return blockData;
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
        /// This method is to get Order Side
        /// </summary>
        /// <param name="orderSideId"></param>
        /// <returns>Order Side</returns>
        private static string GetOrderSide(string orderSideId)
        {
            try
            {
                switch (orderSideId)
                {
                    case FIXConstants.SIDE_Buy:
                        return "Buy";
                    case FIXConstants.SIDE_BuyMinus:
                        return "Buy Minus";
                    case FIXConstants.SIDE_Buy_Open:
                        return "Buy To Open";
                    case FIXConstants.SIDE_Buy_Closed:
                        return "Buy To Close";
                    case FIXConstants.SIDE_Buy_Cover:
                        return "Buy To Cover";
                    case FIXConstants.SIDE_Sell:
                        return "Sell";
                    case FIXConstants.SIDE_SellPlus:
                        return "Sell Plus";
                    case FIXConstants.SIDE_SellShort:
                        return "Sell Short";
                    case FIXConstants.SIDE_SellShortExempt:
                        return "Sell Short Exempt";
                    case FIXConstants.SIDE_Sell_Open:
                        return "Sell To Open";
                    case FIXConstants.SIDE_Sell_Closed:
                        return "Sell To Close";
                    case FIXConstants.SIDE_Cross:
                        return "Cross";
                    case FIXConstants.SIDE_CrossShort:
                        return "Cross Short";
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
            return string.Empty;
        }

        /// <summary>
        /// This method is to create allocation level data
        /// </summary>
        /// <param name="row"></param>
        /// <returns>ThirdPartyAllocationLevelDetails</returns>
        private static ThirdPartyAllocationLevelDetails CreateAllocationLevelData(DataRow row, string blockMatchStatus)
        {
            try
            {
                var allocationData = new ThirdPartyAllocationLevelDetails();

                if (row == null)
                {
                    return null;
                }

                if (!(row[0] is DBNull))
                    allocationData.AllocationDetailId = int.Parse(row[0].ToString());
                if (!(row[1] is DBNull))
                    allocationData.Account = row[1].ToString();
                if (!(row[2] is DBNull))
                    allocationData.Commission = row[2].ToString();
                if (!(row[3] is DBNull))
                    allocationData.Quantity = row[3].ToString();
                if (!(row[4] is DBNull))
                    allocationData.MiscFees = row[4].ToString();
                if (!(row[5] is DBNull))
                    allocationData.AveragePX = row[5].ToString();
                if (!(row[6] is DBNull))
                    allocationData.NetMoney = row[6].ToString();
                if (!(row[7] is DBNull))
                {
                    if (blockMatchStatus == ((int)BlockMatchStatus.BlockLevelReject).ToString() || blockMatchStatus == ((int)BlockMatchStatus.MismatchedWithAllocRejCode).ToString())
                    {
                        allocationData.MatchStatus = EnumHelper.GetDescriptionWithDescriptionAttribute((AllocRejCode)int.Parse(row[7].ToString()));
                    }
                    else
                    {
                        allocationData.MatchStatus = EnumHelper.GetDescriptionWithDescriptionAttribute((AllocMatchStatus)int.Parse(row[7].ToString()));
                    }
                }
                if (!(row[8] is DBNull))
                    allocationData.AccountAllocationID = row[8].ToString();
                if (!(row[9] is DBNull))
                    allocationData.BlockDetailID = int.Parse(row[9].ToString());
                if (!(row[10] is DBNull))
                    allocationData.MessageType = row[10].ToString();
                allocationData.AllocationComparisons = new List<ThirdPartyAllocationDetailComparison>();

                return allocationData;
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
        /// Send mail for allocation match status for complete match, complete mismatch and partial mismatch
        /// </summary>
        /// <param name="thirdPartyJobName"></param>
        /// <param name="clientName"></param>
        public static void SendAllocationStatusChangeMail(string thirdPartyJobName, AllocationMatchStatus allocationMatchStatus)
        {
            try
            {
                ThirdPartyEmailHelper.SendAllocationStatusChangeMail(thirdPartyJobName, allocationMatchStatus);
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

        public static void HandleNewEODMessage(PranaMessage message)
        {
            try
            {
                if (message == null) return;

                string allocId = message.FIXMessage.ExternalInformation[FIXConstants.TagAllocID].Value;
                if (allocId.EndsWith("C")) return;

                if (ThirdPartyCache.DateWiseAllocationBlockDetails.Count == 0
                    || !ThirdPartyCache.DateWiseAllocationBlockDetails.ContainsKey(DateTime.Today.ToShortDateString()))
                {
                    InitiateAllocationBlockCache(DateTime.Today);
                }

                int cpID;
                if (message.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CounterPartyID))
                    cpID = int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value);
                else
                    cpID = int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrigCounterPartyID].Value);


                allocId = allocId.Replace("N", string.Empty).Replace("R", string.Empty);

                int batchId = int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ThirdPartyBatchId].Value);

                var allocationBlockDetails = ThirdPartyCache.DateWiseAllocationBlockDetails[DateTime.Today.ToShortDateString()];
                if (message.MessageType == FIXConstants.MSGAllocation)
                {
                    string connType = message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_BrokerConnectionType].Value;
                    string tpJobName = message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ThirdPartyJobName].Value;
                    bool isStatusRecalculationRequired = true;
                    if (!allocationBlockDetails.ContainsKey(batchId))
                    {
                        allocationBlockDetails.Add(batchId, new JobMatchDetails
                        {
                            ThirdPartyJobName = tpJobName,
                            ThirdPartyBatchId = batchId,
                            BatchRunDate = DateTime.Today,
                            AllocationMatchStatus = connType == "0" ? AllocationMatchStatus.CompleteMatch : AllocationMatchStatus.Pending
                        });
                        isStatusRecalculationRequired = false;
                    }

                    JobMatchDetails jobMatchDetails = allocationBlockDetails[batchId];

                    jobMatchDetails.AllocIdWiseDetails[allocId] = new BlockMatchDetails
                    {
                        BlockMatchStatus = connType == "0" ? BlockMatchStatus.Accepted : BlockMatchStatus.Pending,
                        JMsgBlockId = 0
                    };

                    if (isStatusRecalculationRequired)
                    {
                        CalculateJobStatus(jobMatchDetails);
                    }

                    UpdateStatusForAllocationMatchUpdate(new ThirdPartyAllocationMatchDetails
                    {
                        AllocationMatchStatus = jobMatchDetails.AllocationMatchStatus,
                        ThirdPartyBatchId = jobMatchDetails.ThirdPartyBatchId,
                        BatchRunDate = jobMatchDetails.BatchRunDate
                    });
                }
                else if (message.MessageType == FIXConstants.MSGAllocationReport || message.MessageType == FIXConstants.MSGAllocationACK
                    || message.MessageType == FIXConstants.MSGAllocationReportAck || message.MessageType == FIXConstants.MSGConfirmation
                    || message.MessageType == FIXConstants.MSGConfirmationAck)
                {
                    if (allocationBlockDetails.ContainsKey(batchId))
                    {
                        JobMatchDetails jobMatchDetails = allocationBlockDetails[batchId];
                        AllocationMatchStatus oldStatus = jobMatchDetails.AllocationMatchStatus;
                        if (jobMatchDetails.AllocIdWiseDetails.ContainsKey(allocId))
                        {
                            if (message.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocStatus))
                            {
                                int matchStatus = int.Parse(message.FIXMessage.ExternalInformation[FIXConstants.TagAllocStatus].Value);
                                BlockMatchDetails blockMatchDetails = jobMatchDetails.AllocIdWiseDetails[allocId];
                                blockMatchDetails.BlockMatchStatus = (BlockMatchStatus)matchStatus;

                                CalculateJobStatus(jobMatchDetails);
                                if (oldStatus != jobMatchDetails.AllocationMatchStatus)
                                {
                                    SendAllocationStatusChangeMail(jobMatchDetails.ThirdPartyJobName, jobMatchDetails.AllocationMatchStatus);
                                }
                                UpdateStatusForAllocationMatchUpdate(new ThirdPartyAllocationMatchDetails
                                {
                                    AllocationMatchStatus = jobMatchDetails.AllocationMatchStatus,
                                    ThirdPartyBatchId = jobMatchDetails.ThirdPartyBatchId,
                                    BatchRunDate = jobMatchDetails.BatchRunDate
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public static void InitiateAllocationBlockCache(DateTime date)
        {
            try
            {
                var shortDateString = date.ToShortDateString();
                if (!ThirdPartyCache.DateWiseAllocationBlockDetails.ContainsKey(shortDateString))
                {
                    var jobStatusData = ThirdPartyDataManager.GetDailyJobStatuses(date);
                    ThirdPartyCache.DateWiseAllocationBlockDetails.Add(shortDateString, new Dictionary<int, JobMatchDetails>());
                    var allocationBlockDetails = ThirdPartyCache.DateWiseAllocationBlockDetails[shortDateString];
                    if (jobStatusData != null && jobStatusData.Rows.Count > 0)
                    {
                        foreach (DataRow row in jobStatusData.Rows)
                        {
                            var thirdPartyJobName = row[1].ToString();
                            var blockStatus = (BlockMatchStatus)int.Parse(row[2].ToString());
                            var allocId = row[3].ToString();
                            var thirdPartyBatchId = int.Parse(row[4].ToString());
                            var batchRunDate = row[5].ToString();
                            var blockId = int.Parse(row[6].ToString());
                            var msgType = row[7].ToString();
                            int cpID = int.Parse(row[8].ToString());

                            if (allocId.EndsWith("C")) continue;

                            allocId = allocId.Replace("N", string.Empty).Replace("R", string.Empty);

                            if (!allocationBlockDetails.ContainsKey(thirdPartyBatchId))
                            {
                                allocationBlockDetails.Add(thirdPartyBatchId, new JobMatchDetails
                                {
                                    ThirdPartyJobName = thirdPartyJobName,
                                    ThirdPartyBatchId = thirdPartyBatchId,
                                    BatchRunDate = DateTime.Parse(batchRunDate)
                                });
                            }
                            JobMatchDetails jobMatchDetails = allocationBlockDetails[thirdPartyBatchId];
                            if (!jobMatchDetails.AllocIdWiseDetails.ContainsKey(allocId))
                            {
                                jobMatchDetails.AllocIdWiseDetails.Add(allocId, new BlockMatchDetails
                                {
                                    BlockMatchStatus = blockStatus,
                                    JMsgBlockId = blockId
                                });
                            }
                            else
                            {
                                jobMatchDetails.AllocIdWiseDetails[allocId].BlockMatchStatus = blockStatus;
                            }
                        }
                        CalculateAllJobStatus(allocationBlockDetails);
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
        }

        /// <summary>
        /// Initializing the file status cache
        /// </summary>
        /// <param name="rundate"></param>
        public static void InitiateFileStatusCache(DateTime rundate)
        {
            try
            {
                string shortDateString = rundate.ToString();
                if(ThirdPartyCache.DateWiseFileStatusDetails.Count == 0 || !ThirdPartyCache.DateWiseFileStatusDetails.ContainsKey(shortDateString))
                {
                    var filestatus = ThirdPartyDataManager.GetFileStatus(rundate);
                    ThirdPartyCache.DateWiseFileStatusDetails.Add(shortDateString, new HashSet<int>());
                    foreach (DataRow row in filestatus.Rows)
                    {
                        ThirdPartyCache.DateWiseFileStatusDetails[shortDateString].Add(int.Parse(row[0].ToString()));
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
        }
        private static void CalculateAllJobStatus(Dictionary<int, JobMatchDetails> allocationBlockDetails)
        {
            try
            {
                foreach (var job in allocationBlockDetails.Values)
                {
                    CalculateJobStatus(job);
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

        private static void CalculateJobStatus(JobMatchDetails job)
        {
            try
            {
                if(job.AllocIdWiseDetails.Values.Any(x => x.BlockMatchStatus == BlockMatchStatus.BlockLevelReject))
                {
                    job.AllocationMatchStatus = AllocationMatchStatus.CompleteMismatch;
                }
                else if(job.AllocIdWiseDetails.Values.Any(x => x.BlockMatchStatus == BlockMatchStatus.AccountLevelReject
                                                                    || x.BlockMatchStatus == BlockMatchStatus.MismatchedWithAllocRejCode))
                {
                    job.AllocationMatchStatus = AllocationMatchStatus.PartialMismatch;
                }
                else if (job.AllocIdWiseDetails.Values.Any(x => x.BlockMatchStatus == BlockMatchStatus.Pending
                                                                    || x.BlockMatchStatus == BlockMatchStatus.PendingUserAction))
                {
                    job.AllocationMatchStatus = AllocationMatchStatus.Pending;
                }
                else
                {
                    job.AllocationMatchStatus = AllocationMatchStatus.CompleteMatch;
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

        private static void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void Publish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
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
        /// Sends updated allocation match status to client
        /// </summary>
        /// <param name="thirdPartyAllocationMatchDetails"></param>
        public static void UpdateStatusForAllocationMatchUpdate(ThirdPartyAllocationMatchDetails thirdPartyAllocationMatchDetails)
        {
            try
            {
                MessageData messageData = new MessageData();
                messageData.EventData = new List<object> { thirdPartyAllocationMatchDetails };
                messageData.TopicName = Topics.Topic_ThirdPartyAllocationMatchStatusUpdate;
                Publish(messageData);
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
        /// Saves the file status in the cache
        /// </summary>
        /// <param name="rundate"></param>
        /// <param name="thirdPartyBatchId"></param>
        public static void SaveFileStatus(DateTime rundate, int thirdPartyBatchId)
        {
            try
            {
                if (ThirdPartyDataManager.SaveFileStatus(rundate, thirdPartyBatchId) > 0)
                {
                    var shortDateString = rundate.ToShortDateString();
                    if (!ThirdPartyCache.DateWiseFileStatusDetails.ContainsKey(shortDateString))
                    {
                        ThirdPartyCache.DateWiseFileStatusDetails.Add(shortDateString, new HashSet<int>());
                    }
                    ThirdPartyCache.DateWiseFileStatusDetails[shortDateString].Add(thirdPartyBatchId);
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
        /// This method retrieves third-party batch event data based on the provided allocationID.
        /// </summary>
        /// <param name="allocationID"></param>
        public static List<ThirdPartyBatchEventData> GetThirdPartyBatchEventData(int blockId)
        {
            List<ThirdPartyBatchEventData> thirdPartyBatchEventsData = new List<ThirdPartyBatchEventData>();
            try
            {
                var batchEventData = ThirdPartyDataManager.GetThirdPartyBatchEventData(blockId);
                if (batchEventData != null)
                {
                    var dataTable = batchEventData.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ThirdPartyBatchEventData thirdPartyBatchEventData = CreateThirdPartyBatchEventData(row);
                        thirdPartyBatchEventsData.Add(thirdPartyBatchEventData);
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
            return thirdPartyBatchEventsData;
        }

        /// <summary>
        /// This method is to create Third Party Batch Event data
        /// </summary>
        /// <param name="row"></param>
        /// <returns>ThirdPartyBatchEventData</returns>
        private static ThirdPartyBatchEventData CreateThirdPartyBatchEventData(DataRow row)
        {
            ThirdPartyBatchEventData eventData = new ThirdPartyBatchEventData();
            try
            {
                if (!(row[0] is DBNull))
                    eventData.TransmissionTime = row[0].ToString();
                if (!(row[1] is DBNull))
                    eventData.MsgDescription = row[1].ToString();
                if (!(row[2] is DBNull))
                    eventData.MsgDirection = row[2].ToString();
                if (!(row[3] is DBNull))
                    eventData.AllocID = row[3].ToString();
                if (!(row[4] is DBNull))
                    eventData.FixMsg = row[4].ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return eventData;
        }

        /// <summary>
        /// This method gets the entity ids from the batch
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static string GetEntityIds(ThirdPartyBatch batch)
        {
            string entityIds = string.Empty;
            try
            {
                var _dsXML = new DataSet();
                if (!string.IsNullOrEmpty(batch.SerializedDataSet))
                {
                    _dsXML.ReadXml(new StringReader(batch.SerializedDataSet));
                }
                HashSet<string> entityIdsList = new HashSet<string>();
                foreach (DataRow row in _dsXML.Tables[0].Rows)
                {
                    string entityId = row["EntityID"].ToString();
                    entityIdsList.Add(entityId);
                }
                entityIds = string.Join(",", entityIdsList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return entityIds;
        }

        /// <summary>
        /// This method saves details when user overrides mismatch warning for prime broker
        /// </summary>
        /// <param name="batch"></param>
        public static void SavePBMismatchOverrideAudit(ThirdPartyBatch batch)
        {
            try
            {
                ThirdPartyDataManager.SavePBMismatchOverrideAudit(batch);
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
        /// This method is get the third party file logs
        /// </summary>
        /// <param name="runDate"></param>
        public static List<ThirdPartyFileLog> GetThirdPartyFileLogs(DateTime runDate)
        {
            List<ThirdPartyFileLog> thirdPartyFileAuditLogs = new List<ThirdPartyFileLog>();
            try
            {
                var fileAuditLogs = ThirdPartyDataManager.GetThirdPartyFileLogs(runDate);
                if (fileAuditLogs != null)
                {
                    var dataTable = fileAuditLogs.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ThirdPartyFileLog thirdPartyFileAuditLog = CreateThirdPartyFileLogs(row);
                        thirdPartyFileAuditLogs.Add(thirdPartyFileAuditLog);
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
            return thirdPartyFileAuditLogs;
        }

        /// <summary>
        /// This method is to create Third Party File Logs data
        /// </summary>
        /// <param name="row"></param>
        private static ThirdPartyFileLog CreateThirdPartyFileLogs(DataRow row)
        {
            ThirdPartyFileLog fileLogs = new ThirdPartyFileLog();
            try
            {
                if (!(row[0] is DBNull))
                    fileLogs.Date = row[0].ToString();
                if (!(row[1] is DBNull))
                    fileLogs.Time = row[1].ToString();
                if (!(row[2] is DBNull))
                    fileLogs.Type = row[2].ToString();
                if (!(row[3] is DBNull))
                    fileLogs.Action = row[3].ToString();
                if (!(row[4] is DBNull))
                    fileLogs.Details = row[4].ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return fileLogs;
        }

        /// <summary>
        /// This method saves the third party file logs.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="loggedInUser"></param>
        public static void SaveThirdPartyFileLogs(ThirdPartyBatch batch, string loggedInUser)
        {
            try
            {
                ThirdPartyDataManager.SaveThirdPartyFileLogs(batch, loggedInUser);
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
        /// This Method is to handle View Operation
        /// </summary>
        /// <param name="batch">The ThirdPartyBatch object.</param>
        /// <param name="runDate">The date of the batch run.</param>
        /// <param name="isViewClicked">Indicates whether the view action was clicked.</param>
		/// <param name="isAutomatedBatch">Indicates whether the batch is automated.</param>
        /// <param name="OnStatus"></param>
        /// <param name="OnMessage"></param>
        /// <returns></returns>
        public static ThirdPartyBatch View(ThirdPartyBatch batch, DateTime runDate, bool isViewClicked, bool isAutomatedBatch, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage)
        {
            ThirdPartyBatch resultBatch = batch;
            try
            {
                int flag = (batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker
                    || batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties) ? 1 : 0;

                batch.TaxLotWithStateDict.Clear();
                batch.TaxLotIgnoreStateDict.Clear();
                batch.DeletedToIgnoreDict.Clear();
                bool isFixTransmissionType = batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString());

                try
                {
                    ResetLog(batch.LogFile);
                }
                catch (Exception ex)
                {
                    if (!isAutomatedBatch)
                        OnMessage(string.Empty, new MessageEventArgs("Unable to reset log file\r\n" + ex.Message));
                }

                DataSet dataSet = null;
                if (isFixTransmissionType && isViewClicked)
                    OnStatus(string.Empty, new StatusEventArgs(ThirdPartyConstants.STATUS_LOADING_DATA));
                else if (!isFixTransmissionType)
                    OnStatus(string.Empty, new StatusEventArgs("Loading data..."));

                ThirdPartyFileFormat eodFormat = GetFormat(batch);
                string customSPName;
                bool isFix = batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString());
                if (isFix)
                {
                    if (!string.IsNullOrWhiteSpace(eodFormat.FIXStorProc))
                        customSPName = eodFormat.FIXStorProc;
                    else if (eodFormat.GenerateCancelNewForAmend)
                        customSPName = "P_FFGetThirdPartyFundsDetails_FIX_CancelNew";
                    else
                        customSPName = "P_FFGetThirdPartyFundsDetails_FIX";
                }
                else
                    customSPName = eodFormat.StoredProcName;

                if (Ext.IsNull(customSPName))
                {
                    ThirdPartyFlatFileDetailCollection collection = ThirdPartyValidation.FillData(batch.ThirdPartyCompanyId, ThirdPartyLogic.GetAccountIds(batch), runDate,
                        batch.CompanyId, batch.IsLevel2Data, ThirdPartyLogic.GetAUECIds(), eodFormat.ExportOnly, flag, 1, batch.ThirdPartyFormatId,
                        batch.IncludeSent);

                    ThirdPartyFlatFileDetailCollection collectionForFTP;
                    GetOperationWiseCollection(collection, out collectionForFTP, runDate);
                    if (!isFixTransmissionType)
                        OnStatus(string.Empty, new StatusEventArgs("Formatting data for FTP..."));

                    if (collectionForFTP != null)
                    {
                        var formatter = new ThirdPartyDataFomatter();
                        formatter.FormatData(batch, collectionForFTP, batch.ThirdPartyName, batch.CompanyId, runDate);

                        dataSet = GenerateXML(batch, collectionForFTP, eodFormat, OnMessage);
                    }
                }
                else
                {
                    StringBuilder accountIds = isAutomatedBatch ? ThirdPartyLogic.GetAccountIds() : ThirdPartyLogic.GetAccountIds(batch);
                    dataSet = ThirdPartyValidation.FillData(customSPName, batch.ThirdPartyCompanyId, accountIds, runDate, batch.CompanyId, ThirdPartyLogic.GetAUECIds(), flag, 1, batch.ThirdPartyFormatId);
                    if (!isFixTransmissionType)
                        OnStatus(string.Empty, new StatusEventArgs("Formatting data for FTP..."));

                    var formatter = new ThirdPartyDataFomatter();
                    dataSet = formatter.FormatData(dataSet, eodFormat, batch, customSPName);
                    UpdateDataSetForFIX(dataSet, batch);
                }

                if (dataSet == null || dataSet.Tables[0].Rows.Count == 0)
                {
                    if (isFixTransmissionType && isViewClicked)
                        OnMessage(string.Empty, new MessageEventArgs(ThirdPartyConstants.STATUS_NO_DATA_FOUND));
                    else if (!isFixTransmissionType)
                        OnMessage(string.Empty, new MessageEventArgs("No Data Found for FTP (Review settings or Set XSLT file)."));

                    dataSet = null;
                    return resultBatch;
                }

                if (!(eodFormat.DelimiterName.ToUpper().ToString().Equals("XML")))
                    dataSet = UpdateCaptions(dataSet);
                else
                {
                    bool captionChangeRequiredExists = dataSet.Tables[0].Columns.Contains("IsCaptionChangeRequired");
                    DataColumnCollection columns = dataSet.Tables[0].Columns;
                    if (captionChangeRequiredExists && columns.Contains("TaxLotState") && dataSet.Tables[0].Rows[0]["TaxLotState"].ToString().ToUpper().Equals("TAXLOTSTATE"))
                        dataSet.Tables[0].Rows[0].Delete();
                }
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (!isFixTransmissionType)
                        OnStatus(string.Empty, new StatusEventArgs("Formatting data for FTP has been completed."));

                    batch.SerializedDataSource = dataSet.GetXml();
                    batch.SerializedDataSet = batch.SerializedDataSource;

                    if (isFixTransmissionType && isViewClicked && !isAutomatedBatch)
                        OnStatus(string.Empty, new StatusEventArgs(ThirdPartyConstants.STATUS_DATA_LOADED_SUCCESSFULLY));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (!isAutomatedBatch)
                    OnMessage(string.Empty, new MessageEventArgs("Problem in Formatting data. " + ex.Message));
                return resultBatch;
            }
            return resultBatch;
        }

        /// <summary>
        /// Sends a file for the specified ThirdPartyBatch.
        /// </summary>
        /// <param name="batch">The ThirdPartyBatch object.</param>
        /// <param name="runDate">The date of the batch run.</param>
        /// <param name="loggedInUser">The username of the logged-in user.</param>
        /// <param name="isAutomatedBatch">Indicates whether the batch is automated.</param>
        /// <param name="OnStatus">The on status.</param>
        /// <param name="OnMessage">The on message.</param>
        public static bool SendFile(ThirdPartyBatch batch, DateTime runDate, string loggedInUser, bool isAutomatedBatch, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage)
        {
            bool isSuccessful = false;
            try
            {
                bool isFixTransmissionType = batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString());

                if (string.IsNullOrEmpty(batch.SerializedDataSource))
                {
                    batch = View(batch, runDate, false, false,OnStatus, OnMessage);
                }
                if (string.IsNullOrEmpty(batch.SerializedDataSource))
                {
                    if (isFixTransmissionType && !isAutomatedBatch)
                    {
                        OnStatus(string.Empty, new StatusEventArgs(ThirdPartyConstants.STATUS_FIX_GENERATION_UNSUCCESSFUL));
                        OnStatus(string.Empty, new StatusEventArgs(ThirdPartyConstants.STATUS_ALLOCATION_INSTRUCTIONS_FAILED));
                    }
                    return false;
                }
                ThirdPartyExecutor executor = GetCustomisedExecutor(batch, runDate, OnMessage);
                if (executor != null && !string.IsNullOrEmpty(batch.TransmissionType))
                {
                    if (isFixTransmissionType)
                    {
                        isSuccessful = ThirdPartyFixManager.Instance.SendbatchData(batch, runDate, OnStatus);
                        if (isSuccessful)
                        {
                            if (string.IsNullOrEmpty(batch.Format.FileName))
                            {
                                batch.Format.FileName = batch.FileFormatName + "_" + CachedDataManager.GetInstance.GetCounterPartyText(batch.CounterPartyID);
                            }
                            UpdateTaxlotStateAndSaveData(batch, false);
                            UpdateTaxlotsToIgnoreState(batch);
                            if (batch.BrokerConnectionType == EnumHelper.GetDescriptionWithDescriptionAttribute(PranaServerConstants.BrokerConnectionType.SendOnly))
                            {
                                ThirdPartyEmailHelper.SendAllocationStatusChangeMail(batch.Description, AllocationMatchStatus.CompleteMatch);
                            }
                            if (!isAutomatedBatch)
                                OnStatus(string.Empty, new StatusEventArgs(ThirdPartyConstants.STATUS_ALLOCATION_INSTRUCTIONS_SENT));
                        }
                        else if (!isAutomatedBatch)
                            OnStatus(string.Empty, new StatusEventArgs(ThirdPartyConstants.STATUS_ALLOCATION_INSTRUCTIONS_FAILED));
                    }
                    else
                    {
                        if (batch.ThirdPartyTypeId == 1 && GenerateFile(batch, executor) && !CheckForMismatchAndGetConfirmation(batch, runDate) && SendFileValidations(batch, runDate, OnStatus, OnMessage))
                        {
                            isSuccessful = EncryptAndSendFile(batch, runDate, OnStatus, OnMessage, loggedInUser);
                        }
                        else if (batch.ThirdPartyTypeId != 1 && GenerateFile(batch, executor) && SendFileValidations(batch, runDate, OnStatus, OnMessage))
                        {
                            isSuccessful = EncryptAndSendFile(batch, runDate, OnStatus, OnMessage, loggedInUser);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //OnMessage(string.Empty, new MessageEventArgs(ex.Message));
                if (!isAutomatedBatch)
                    OnMessage(string.Empty, new MessageEventArgs("Error in Sending file! Please review FTP/FIX settings."));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSuccessful;
        }

        /// <summary>
        /// Resets the log.
        /// </summary>
        /// <param name="OnMessage">The on message.</param>
        /// <remarks></remarks>
        private static void ResetLog(string logFile)
        {
            try
            {
                string logFilepath = Path.GetDirectoryName(logFile);
                if (!Directory.Exists(logFilepath))
                {
                    if (logFilepath != null)
                        Directory.CreateDirectory(logFilepath);
                }
                File.WriteAllText(logFile, "");
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

        private static void GetOperationWiseCollection(ThirdPartyFlatFileDetailCollection collection, out ThirdPartyFlatFileDetailCollection collectionForFTP, DateTime runDate)
        {
            collectionForFTP = new ThirdPartyFlatFileDetailCollection();
            try
            {
                foreach (ThirdPartyFlatFileDetail item in collection)
                {
                    if ((item.TaxLotState != PranaTaxLotState.Sent || (item.TaxLotState == PranaTaxLotState.Sent && DateTime.Parse(item.TradeDate).Date == runDate.Date)) &&
                        (item.TaxLotState != PranaTaxLotState.Ignore || (item.TaxLotState == PranaTaxLotState.Ignore && DateTime.Parse(item.TradeDate).Date == runDate.Date)))
                        collectionForFTP.AddItem(item);
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
        /// Gets the format.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ThirdPartyFileFormat GetFormat(ThirdPartyBatch batch)
        {
            try
            {
                List<ThirdPartyFileFormat> formats = null;

                if (batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                    formats = ThirdPartyLogic.GetCompanyThirdPartyFileFormats(batch.ThirdPartyCompanyId);
                else
                    formats = ThirdPartyLogic.GetCompanyThirdPartyTypeFileFormats(batch.ThirdPartyCompanyId);

                foreach (object item in formats)
                {
                    if (((ThirdPartyFileFormat)item).FileFormatId == batch.ThirdPartyFormatId)
                    {
                        return (ThirdPartyFileFormat)item;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates the taxlot state and save data.
        /// </summary>
        /// <param name="executor">The executor.</param>
        /// <remarks></remarks>
        public static void UpdateTaxlotStateAndSaveData(ThirdPartyBatch batch, bool isExportonly)
        {
            try
            {
                ThirdPartyFileFormat fileFormat = batch.Format.Formatter;
                DataSet dsGrid = null;

                if (!string.IsNullOrEmpty(batch.SerializedDataSet))
                {
                    dsGrid = new DataSet();
                    dsGrid.ReadXml(new StringReader(batch.SerializedDataSet));
                }

                if (dsGrid != null)
                {
                    string taxLotIDs = string.Empty;
                    string deletedtaxLotIDs = "<TaxLots>";
                    string taxlotXmlToInsert = "<TaxLots>";

                    if (batch.IsLevel2Data == true)
                    {
                        taxLotIDs = "<TaxLots IsL1Data=\"" + "True" + "\">";
                        taxlotXmlToInsert = "<TaxLots IsL1Data=\"" + "True" + "\">";
                    }
                    else
                    {
                        taxLotIDs = "<TaxLots IsL1Data=\"" + "False" + "\">";
                        taxlotXmlToInsert = "<TaxLots IsL1Data=\"" + "False" + "\">";
                    }

                    DataTable dtUpdateTaxLotState = dsGrid.Tables.Count == 2 ? dsGrid.Tables[1] : dsGrid.Tables[0];
                    PranaTaxLotState myEnum; int stateID = 0;
                    DataColumnCollection columns = dtUpdateTaxLotState.Columns;
                    foreach (DataRow dr in dtUpdateTaxLotState.Rows)
                    {
                        if (Regex.IsMatch(dr["EntityID"].ToString(), "^[0-9]+$", RegexOptions.Compiled))
                            taxLotIDs += String.Format("<TaxLot TaxLotID =\"{0}\"/>", dr["EntityID"]);

                        if (fileFormat.ClearExternalTransID && columns.Contains("TaxlotState") && dr["TaxLotState"].Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Deleted.ToString()))
                        {
                            if (Regex.IsMatch(dr["EntityID"].ToString(), "^[0-9]+$", RegexOptions.Compiled))
                                deletedtaxLotIDs += String.Format("<TaxLot TaxLotID =\"{0}\"/>", dr["EntityID"]);
                        }

                        if (columns.Contains("EntityID") && columns.Contains("TaxLotState") && dr["TaxLotState"] != null)
                        {
                            stateID = 0;

                            if (!string.IsNullOrEmpty(dr["TaxLotState"].ToString()) && (Enum.TryParse<PranaTaxLotState>(dr["TaxLotState"].ToString(), out myEnum)))
                                stateID = (int)(PranaTaxLotState)Enum.Parse(typeof(PranaTaxLotState), dr["TaxLotState"].ToString());

                            if (Regex.IsMatch(dr["EntityID"].ToString(), "^[0-9]+$", RegexOptions.Compiled))
                                taxlotXmlToInsert += String.Format("<TaxLot TaxLotID =\"{0}\" TaxlotState =\"{1}\"/>", dr["EntityID"], stateID);
                        }
                    }

                    taxLotIDs += "</TaxLots>";
                    taxlotXmlToInsert += "</TaxLots>";
                    deletedtaxLotIDs += "</TaxLots>";

                    if (!isExportonly)
                    {
                        ThirdPartyFlatFileManager.InsertPBWiseTaxlotState(batch.ThirdPartyCompanyId, batch.ThirdPartyFormatId, taxlotXmlToInsert);

                        ThirdPartyFlatFileManager.UpdateTaxlotState(batch.ThirdPartyCompanyId, batch.ThirdPartyFormatId, taxLotIDs, deletedtaxLotIDs, fileFormat.GenerateCancelNewForAmend);
                    }

                    Int64 fileId = Int64.Parse(DateTime.Now.ToString("MMddHHmmss"));

                    ThirdPartyFlatFileManager.SaveThirdPartyDetails(batch.ThirdPartyCompanyId, DateTime.Now, string.IsNullOrEmpty(batch.Format.FileName) ? string.Empty : batch.Format.FileName, fileId, taxLotIDs);
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
        /// To get customized executor for generating files
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="runDate"></param>
        /// <returns></returns>
        public static ThirdPartyExecutor GetCustomisedExecutor(ThirdPartyBatch batch, DateTime runDate, EventHandler<MessageEventArgs> OnMessage, bool isComingForExport = false)
        {
            try
            {
                ThirdPartyExecutor executor = new ThirdPartyExecutor(batch);
                executor.OnMessage += OnMessage;
                if (!isComingForExport && !string.IsNullOrEmpty(batch.TransmissionType) && batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString()))
                {
                    batch.Format = new ThirdPartyUserDefinedFormat();
                    batch.Format.Formatter = GetFormat(batch);
                }
                else
                {
                    if (executor.CustomiseFormat(batch, GetFormat(batch), runDate) == false)
                    {
                        return null;
                    }
                }

                return executor;
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
        /// Updates the state of the taxlots to ignore.
        /// </summary>
        /// <param name="executor">The executor.</param>
        /// <remarks></remarks>
        public static void UpdateTaxlotsToIgnoreState(ThirdPartyBatch batch)
        {
            try
            {
                var _dsXML = new DataSet();
                if (!string.IsNullOrEmpty(batch.SerializedDataSet))
                {
                    _dsXML.ReadXml(new StringReader(batch.SerializedDataSet));
                }

                string taxLotIDs = string.Empty;
                if (batch.IsLevel2Data == true)
                {
                    taxLotIDs = "<TaxLots IsL1Data=\"" + "True" + "\">";
                }
                else
                {
                    taxLotIDs = "<TaxLots IsL1Data=\"" + "False" + "\">";
                }

                int tableCount = _dsXML.Tables.Count;
                DataTable dt = new DataTable();
                if (tableCount.Equals(1))
                {
                    dt = _dsXML.Tables[0];
                }
                else
                {
                    dt = _dsXML.Tables[1];
                }
                DataColumnCollection columns = dt.Columns;
                foreach (DataRow row in dt.Rows)
                {
                    if (columns.Contains("TaxlotState") && row["TaxlotState"].ToString().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString()))
                    {
                        taxLotIDs += String.Format("<TaxLot TaxLotID =\"{0}\" TaxLotState =\"{1}\"/>",
                            row["EntityID"], (int)Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore);
                    }
                }
                if (batch.TaxLotIgnoreStateDict.Count > 0)
                {
                    foreach (KeyValuePair<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> kvp in batch.TaxLotIgnoreStateDict)
                    {
                        string TaxlotId = kvp.Key;
                        if (batch.TaxLotIgnoreStateDict[TaxlotId].Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated))
                        {
                            taxLotIDs += String.Format("<TaxLot TaxLotID =\"{0}\" TaxLotState =\"{1}\"/>",
                                TaxlotId, (int)Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated);
                        }
                    }
                }

                if (batch.DeletedToIgnoreDict.Count > 0)
                {
                    ICollection<String> orig = new List<string>();

                    foreach (KeyValuePair<string, string> keyvalpair in batch.DeletedToIgnoreDict)
                    {
                        orig.Add(keyvalpair.Key);
                    }

                    foreach (String key in orig)
                    {
                        if (batch.DeletedToIgnoreDict.ContainsKey(key))
                        {
                            batch.DeletedToIgnoreDict[key] = "yes";
                        }
                    }
                }

                taxLotIDs += "</TaxLots>";
                ThirdPartyFlatFileManager.UpdateTaxlotsToIgnoreState(batch.ThirdPartyId, taxLotIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method checks if there is a mismatch in groups and asks for user confirmation 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="runDate"></param>
        /// <returns></returns>
        public static bool CheckForMismatchAndGetConfirmation(ThirdPartyBatch batch, DateTime runDate)
        {
            bool isMismatch = false;
            try
            {
                string entityIds = ThirdPartyLogic.GetEntityIds(batch);
                isMismatch = ThirdPartyDataManager.GetMismatchStatus(runDate, entityIds, batch.IsLevel2Data);
                if (isMismatch)
                {
                    var message = $"Some “trades” are pending acceptance from the broker. Are you sure you wish to proceed with sending data to {batch.ThirdPartyName}?";
                    GetFileSendConfirmation(message, batch, Topics.Topic_ThirdPartyMismatchFileConfirmation);
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
            return isMismatch;
        }

        private static void GetFileSendConfirmation(string message, ThirdPartyBatch batch, string topicName)
        {
            try
            {
                List<object> listData = new List<object>();
                listData.Add(message);
                listData.Add(batch);

                MessageData messageData = new MessageData();
                messageData.EventData = listData;
                messageData.TopicName = topicName;
                Publish(messageData);
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
        /// Generates the file.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="executor"></param>       
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool GenerateFile(ThirdPartyBatch batch, ThirdPartyExecutor executor)
        {
            try
            {
                if (batch.Format.Formatter.FileExtension.ToUpper() == "XML")
                {
                    executor.GenerateUserDefindFormatForXML(batch);
                }
                else if (batch.Format.Formatter.FileExtension.ToUpper() == "XLS")
                {
                    executor.GenerateExcelFile(batch);
                }
                else
                {
                    executor.GenerateUserDefindFormat(batch);
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
            return true;
        }

        /// <summary>
        /// This method is to perform validations before sending file
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="OnStatus"></param>
        /// <param name="OnMessage"></param>
        /// <returns></returns>
        private static bool SendFileValidations(ThirdPartyBatch batch, DateTime runDate, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage)
        {
            try
            {
                if (batch.Format == null)
                {
                    OnMessage(String.Empty, new MessageEventArgs("Unable to send file for FTP. File has not been generated."));
                    return false;
                }
                string file = Path.GetFullPath(batch.Format.FilePath);

                if (batch.Ftp == null && batch.EmailData == null)
                {
                    OnMessage(String.Empty, new MessageEventArgs("No Ftp or Email has been assigned to this job"));
                    return false;
                }

                if (File.Exists(file) == false)
                {
                    OnStatus(String.Empty, new StatusEventArgs(string.Format("Unable to send file for FTP. File {0} not found", file)));
                    return false;
                }

                if (!batch.HaveOverridePBDuplicateFileWarning && CheckForDuplicateFile(batch, OnStatus))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                OnMessage(String.Empty, new MessageEventArgs("Error in Sending file! Please review FTP settings."));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }

            return true;
        }

        private static bool CheckForDuplicateFile(ThirdPartyBatch batch, EventHandler<StatusEventArgs> OnStatus)
        {
            ThirdPartyFtp ftpToSend = batch.Ftp;
            try
            {
                if (ftpToSend != null)
                {
                    NirvanaWinSCPUtility winScp = new NirvanaWinSCPUtility(ftpToSend);
                    var destination = "/";
                    if (!string.IsNullOrEmpty(ftpToSend.FtpFolderPath))
                    {
                        destination = ftpToSend.FtpFolderPath;
                    }

                    OnStatus(string.Empty, new StatusEventArgs("Checking file on " + ftpToSend.FtpType.ToUpper() + " server..."));

                    if (winScp.IsAlreadySend(batch.Format.FilePath, destination))
                    {
                        var message = "The file for " + ftpToSend.FtpType.ToUpper() + " has already been Sent. Continue Anyway?";
                        GetFileSendConfirmation(message, batch, Topics.Topic_ThirdPartyDuplicateFileConfirmation);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                OnStatus(string.Empty, new StatusEventArgs("File(s) sending to " + ftpToSend.FtpType.ToUpper() + " failed"));

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method encrypts and sends file for given third party batch and date
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="rundate"></param>
        /// <param name="OnStatus"></param>
        /// <param name="OnMessage"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        public static bool EncryptAndSendFile(ThirdPartyBatch batch, DateTime rundate, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage, string loggedInUser)
        {
            try
            {
                if (batch.HaveFoundPBMismatchOverride && !SendFileValidations(batch, rundate, OnStatus, OnMessage))
                {
                    return false;
                }
                if (batch.GnuPG != null && batch.GnuPG.Enabled)
                {
                    string data = string.Empty;
                    if (EncryptFile(batch, File.ReadAllText(batch.Format.FilePath), ref data, OnStatus, OnMessage) != 0)
                    {
                        return false;
                    }

                    OnStatus(string.Empty, new StatusEventArgs("Successfuly Encrypted file for FTP."));
                    File.WriteAllText(batch.Format.FilePath, data);
                }
                bool isSuccessful = SendFileOnFTPServerAndMail(batch, batch.Format.FilePath, OnStatus, OnMessage);
                if (isSuccessful)
                {
                    ThirdPartyLogic.UpdateStatusForAllocationMatchUpdate(new ThirdPartyAllocationMatchDetails
                    {
                        ThirdPartyBatchId = batch.ThirdPartyBatchId,
                        AllocationMatchStatus = AllocationMatchStatus.CompleteMatch
                    });
                    if (batch.HaveFoundPBMismatchOverride)
                    {
                        ThirdPartyLogic.SavePBMismatchOverrideAudit(batch);
                    }
                    ThirdPartyLogic.SaveFileStatus(rundate, batch.ThirdPartyBatchId);
                    ThirdPartyLogic.UpdateTaxlotStateAndSaveData(batch, batch.Format.Formatter.ExportOnly);
                    if (!batch.Format.Formatter.ExportOnly)
                    {
                        //ThirdPartyLogic.UpdateTaxlotStateAndSaveData(batch);
                        ThirdPartyLogic.UpdateTaxlotsToIgnoreState(batch);
                    }
                    SaveThirdPartyFileLogs(batch, loggedInUser);
                }
                return isSuccessful;
            }
            catch (Exception ex)
            {
                OnMessage(string.Empty, new MessageEventArgs("Error in Sending file! Please review FTP settings."));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="outputText">The output text.</param>
        /// <param name="OnStatus">The on status.</param>
        /// <param name="OnMessage">The on message.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int EncryptFile(ThirdPartyBatch batch, string inputText, ref string outputText, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage)
        {
            try
            {
                var GnuPG = batch.GnuPG;
                //encryption not required
                if (GnuPG == null) return 0;

                if (GnuPG.Enabled == false) return 0;

                if (Directory.Exists(GnuPG.HomeDirectory) == false)
                {
                    OnMessage(string.Empty, new MessageEventArgs(string.Format("Can't find Encryption Path {0}", GnuPG.HomeDirectory)));
                    return 0;
                }
                OnStatus(string.Empty, new StatusEventArgs("Encrypting file.."));

                // Create GnuPG wrapping class
                GnuPGWrapper gpg = new GnuPGWrapper();

                // Set command
                gpg.command = (Commands)GnuPG.Command;

                // Set some parameters from on Web.Config file
                gpg.homedirectory = GnuPG.HomeDirectory;

                //setup pass phrase
                gpg.passphrase = GnuPG.PassPhrase;
                // Set other parameters from Web Controls
                gpg.originator = GnuPG.Originator;
                gpg.recipient = GnuPG.Recipient;
                gpg.batch = GnuPG.UseCmdBatch;
                gpg.yes = GnuPG.UseCmdYes;
                gpg.armor = GnuPG.UseCmdArmor;

                outputText = "";

                // Execute GnuPG
                gpg.ExecuteCommand(inputText, out outputText);

                return gpg.exitcode;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                OnMessage(string.Empty, new MessageEventArgs(string.Format("Problem in Encrypting file. Please review your settings.")));

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
        }

        /// <summary>
        /// Sends the and mail.
        /// Modified by omshiv, Jan 2014
        /// </summary>
        /// <param name="sftp">The SFTP.</param>
        /// <param name="file">The file.</param>
        /// <param name="OnStatus"></param>
        /// <param name="OnMessage"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool SendFileOnFTPServerAndMail(ThirdPartyBatch batch, string file, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage)
        {
            bool isFileSent = false;
            try
            {
                isFileSent = SendFileOnFTPServer(batch, file, isFileSent, OnStatus);
                string status = isFileSent ? "Success" : "Failed";
                SendFileOnMailServer(batch, file, status, OnStatus, OnMessage);
                ArchiveFileAndLogs(batch, OnStatus);
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
            return isFileSent;
        }

        private static void ArchiveFileAndLogs(ThirdPartyBatch batch, EventHandler<StatusEventArgs> OnStatus)
        {
            try
            {
                OnStatus(string.Empty, new StatusEventArgs("Archiving files and logs..."));
                if (File.Exists(batch.Format.ArchivePath))
                {
                    File.Delete(batch.Format.ArchivePath);
                }

                if (File.Exists(batch.Format.LogPath))
                {
                    File.Delete(batch.Format.LogPath);
                }

                string archiveDirPath = Path.GetDirectoryName(batch.Format.ArchivePath);
                if (!Directory.Exists(archiveDirPath))
                {
                    Directory.CreateDirectory(archiveDirPath);
                }

                string currentTimeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssff");
                string archivePathWithTimeStamp = Path.Combine(Path.GetDirectoryName(batch.Format.ArchivePath),
                    new StringBuilder(Path.GetFileNameWithoutExtension(batch.Format.ArchivePath)).Append("_")
                        .Append(currentTimeStamp).Append(Path.GetExtension(batch.Format.ArchivePath)).ToString());
                string logFilePathWithTimeStamp = Path.Combine(Path.GetDirectoryName(batch.Format.ArchivePath),
                    new StringBuilder(Path.GetFileNameWithoutExtension(batch.Format.LogPath)).Append("_").Append(currentTimeStamp)
                        .Append(Path.GetExtension(batch.Format.LogPath)).ToString());
                File.Copy(batch.Format.FilePath, archivePathWithTimeStamp, true);

                OnStatus(string.Empty, new StatusEventArgs("Archive files and logs has been completed."));
                OnStatus(string.Empty, new StatusEventArgs("_______________________________________________________" + System.Environment.NewLine));
                batch.ThirdPartyShortName = CachedDataManager.GetInstance.GetThirdPartyShortNameByID(batch.ThirdPartyId);
                if (File.Exists(batch.LogFile))
                    File.Move(batch.LogFile, logFilePathWithTimeStamp);
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

        private static bool SendFileOnFTPServer(ThirdPartyBatch batch, string file, bool isSendFileOnFtpServer, EventHandler<StatusEventArgs> OnStatus)
        {
            ThirdPartyFtp ftpToSend = batch.Ftp;
            try
            {
                if (ftpToSend != null)
                {
                    string sendResult = string.Empty;

                    NirvanaWinSCPUtility winScp = new NirvanaWinSCPUtility(ftpToSend);
                    String destination = "/";
                    if (!string.IsNullOrEmpty(ftpToSend.FtpFolderPath))
                    {
                        destination = ftpToSend.FtpFolderPath;
                    }

                    OnStatus(string.Empty, new StatusEventArgs("Sending file to " + ftpToSend.FtpType.ToUpper() + " server..."));
                    sendResult = winScp.SendFile(file, destination);
                    if (!string.IsNullOrEmpty(sendResult))
                    {
                        isSendFileOnFtpServer = true;
                        OnStatus(string.Empty, new StatusEventArgs("File(s) sent result :" + sendResult));
                    }
                }
                else
                {
                    OnStatus(string.Empty, new StatusEventArgs("Unable to send file to ftp as no settings available for this job."));
                }
            }
            catch (Exception ex)
            {
                OnStatus(string.Empty, new StatusEventArgs("File(s) sending to " + ftpToSend.FtpType.ToUpper() + " failed"));

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return isSendFileOnFtpServer;
        }

        private static void SendFileOnMailServer(ThirdPartyBatch batch, string file, string status, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage)
        {
            try
            {
                //Send file through Mail
                if (batch.EmailData != null)
                {
                    List<String> mailAttachments = new List<string>();
                    mailAttachments.Add(file);

                    String mailContent = GetMailBody(batch.EmailData);
                    OnStatus(string.Empty, new StatusEventArgs("Sending data file through Email..."));
                    SendEmail(batch, mailAttachments, status, mailContent, OnStatus, OnMessage);
                }

                if (batch.EmailLog != null)
                {
                    OnStatus(string.Empty, new StatusEventArgs("Sending log file through Email..."));
                    batch.ThirdPartyShortName = CachedDataManager.GetInstance.GetThirdPartyShortNameByID(batch.ThirdPartyId);
                    List<string> mailAttachmentsLog = new List<string> { batch.LogFile };
                    string mailContent = GetMailBody(batch.EmailLog);
                    SendEmail(batch, mailAttachmentsLog, status, mailContent, OnStatus, OnMessage, "log");
                }
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="file">The file.</param>
        /// <param name="OnStatus"></param>
        /// <param name="OnMessage"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool SendEmail(ThirdPartyBatch batch, List<string> mailAttachments, string status, string body, EventHandler<StatusEventArgs> OnStatus, EventHandler<MessageEventArgs> OnMessage, string type = "email")
        {
            try
            {
                ThirdPartyEmail settings = null;
                if (type == "email")
                {
                    settings = batch.EmailData;
                }
                else
                {
                    settings = batch.EmailLog;
                }
                if (settings != null && settings.Enabled)
                {
                    //check for mailTo data
                    if (string.IsNullOrEmpty(settings.MailTo))
                    {
                        OnMessage(string.Empty, new MessageEventArgs("No Mail-To was defined. Please review email settings"));
                        return false;
                    }
                    else if (settings.MailTo.Contains(",") || settings.MailTo.Contains(":"))
                    {
                        OnMessage(string.Empty, new MessageEventArgs("Email not Sent, Please use only ';' between email Ids. Review email settings"));
                        return false;
                    }

                    //check for MailFrom data
                    if (string.IsNullOrEmpty(settings.MailFrom))
                    {
                        OnMessage(string.Empty, new MessageEventArgs("No Mail-From was defined. Please review email settings"));
                        return false;
                    }

                    //check for CcTo data
                    if (!string.IsNullOrEmpty(settings.CcTo) && (settings.CcTo.Contains(",") || settings.CcTo.Contains(":")))
                    {
                        OnMessage(string.Empty, new MessageEventArgs("Email not Sent, Please use only ';' between email Ids in CC. Review email settings"));
                        return false;
                    }

                    using (SmtpClient client = new SmtpClient(settings.Smtp, settings.Port))
                    {
                        client.Credentials = new System.Net.NetworkCredential(settings.UserName, settings.Password);
                        client.EnableSsl = settings.SSLEnabled;
                        using (MailMessage oMsg = new MailMessage())
                        {

                            if (settings.CcTo != null)
                            {
                                if (settings.CcTo.Contains(";"))
                                {
                                    String[] emailIDsList = settings.CcTo.Split(';');
                                    foreach (string emailId in emailIDsList)
                                    {
                                        oMsg.CC.Add(new MailAddress(emailId));
                                    }
                                }
                                else
                                {
                                    oMsg.CC.Add(new MailAddress(settings.CcTo));
                                }

                            }

                            if (settings.BccTo != null)
                            {
                                if (settings.BccTo.Contains(";"))
                                {
                                    String[] emailIDsList = settings.BccTo.Split(';');
                                    foreach (string emailId in emailIDsList)
                                    {
                                        oMsg.Bcc.Add(new MailAddress(emailId));
                                    }
                                }
                                else
                                {
                                    oMsg.Bcc.Add(new MailAddress(settings.BccTo));
                                }
                            }

                            if (settings.MailTo.Contains(";"))
                            {
                                String[] emailIDsList = settings.MailTo.Split(';');
                                foreach (string emailId in emailIDsList)
                                {
                                    oMsg.To.Add(new MailAddress(emailId));
                                }
                            }
                            else
                            {
                                oMsg.To.Add(new MailAddress(settings.MailTo));
                            }

                            oMsg.From = new MailAddress(settings.MailFrom);

                            if (string.IsNullOrEmpty(settings.Subject) == false)
                            {
                                oMsg.Subject = string.Format(settings.Subject, status);
                            }
                            else
                            {
                                oMsg.Subject = "Nirvana EOD Mail";
                            }


                            oMsg.Body = body;
                            oMsg.Priority = settings.Priority;
                            oMsg.IsBodyHtml = true;

                            foreach (String file in mailAttachments)
                            {
                                if (type == "email")
                                {
                                    oMsg.Attachments.Add(new Attachment(file));
                                    string msg = string.Format("Attaching file {0} ", Path.GetFileName(file));
                                    OnStatus(string.Empty, new StatusEventArgs(msg));
                                }
                                else if (type == "log")
                                {

                                    if (File.Exists(Directory.GetCurrentDirectory() + "\\EOD\\Log_" + Path.GetFileName(file)))
                                        File.Delete(Directory.GetCurrentDirectory() + "\\EOD\\Log_" + Path.GetFileName(file));
                                    File.Copy(file, Directory.GetCurrentDirectory() + "\\EOD\\Log_" + Path.GetFileName(file));
                                    oMsg.Attachments.Add(
                                        new Attachment(Directory.GetCurrentDirectory() + "\\EOD\\Log_" +
                                                       Path.GetFileName(file)));
                                    string msg = string.Format("Attaching file {0} ", Path.GetFileName(file));
                                    OnStatus(string.Empty, new StatusEventArgs(msg));
                                }
                            }

                            client.Send(oMsg);
                            if (string.IsNullOrEmpty(settings.CcTo) && string.IsNullOrEmpty(settings.BccTo))
                                OnStatus(string.Empty, new StatusEventArgs("Email sent to " + settings.MailTo));
                            else if (settings.CcTo != null && string.IsNullOrEmpty(settings.BccTo))
                                OnStatus(string.Empty, new StatusEventArgs("Email sent to " + settings.MailTo + " and CC to " +
                                                        settings.CcTo));
                            else if (string.IsNullOrEmpty(settings.CcTo) && settings.BccTo != null)
                                OnStatus(string.Empty, new StatusEventArgs("Email sent to " + settings.MailTo + " and Bcc to " + settings.BccTo));
                            else
                                OnStatus(string.Empty, new StatusEventArgs("Email sent to " + settings.MailTo + "and Cc to" + settings.CcTo +
                                                        " and Bcc to " + settings.BccTo));
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (FileNotFoundException fnfex)
            {
                OnMessage(string.Empty, new MessageEventArgs("File, to be sent, not found."));
                bool rethrow = Logger.HandleException(fnfex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }

                return false;
            }
            catch (Exception ex)
            {
                OnMessage(string.Empty, new MessageEventArgs("Error in Sending mail! Please review email settings. Error details: " + ex.Message.ToString()));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Get mail content to sending in mail
        /// Created by omshiv, Jan 2014
        /// </summary>
        /// <param name="file"></param>
        /// <param name="LogFile"></param>
        /// <returns></returns>
        private static string GetMailBody(ThirdPartyEmail emailSettings)
        {
            try
            {
                StringBuilder mailbody = new StringBuilder();
                // FileInfo fileInfo = new FileInfo(file);

                const string header = "<html> <body style ='border: 2px solid black; padding:20px;'> Hi, <p> ";
                const string body = "<p>This is an automatically generated notification email from an unattended mailbox. Please do not directly reply. Open attached document(s) for your reference. For questions and concerns please contact support@nirvanasolutions.com or call 212-768-3410. </p>";
                const string footer = "<p>NOTICE - This message contains privileged and confidential information intended only for the use of the addressee named above. If you are not the intended recipient of this message, you are hereby notified that you must not disseminate, copy or take any action in reliance on it. If you have received this message in error, please immediately notify Nirvana Financial Solutions, INC, its subsidiaries or associates. Any views expressed in this message are those of the individual sender, except where the sender specifically states them to be the view of Nirvana Financial Solutions, INC , its subsidiaries and associates.</p></br>Thank you.</p> <h2>Nirvana Team </h2> </body></html> ";
                mailbody.Append(header);
                if (emailSettings != null && !string.IsNullOrEmpty(emailSettings.Body))
                {
                    mailbody.Append(emailSettings.Body);
                    mailbody.Append("<br/>");
                    mailbody.Append("<br/>");
                    mailbody.Append(body);
                }
                mailbody.Append("<br/>");
                mailbody.Append(body);
                mailbody.Append("<br/>");
                mailbody.Append("<br/>");
                mailbody.Append(footer);

                return mailbody.ToString();
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
                return string.Empty;
            }

        }

        /// <summary>
        /// Publishes the status of third-party automated batches.
        /// </summary>
        public static void PublishThirdPartyAutomatedBatchStatus()
        {
            try
            {               
                Dictionary<int, string> dictAutomatedBatchStatus = LoadThirdPartyAutomatedBatchStatus();
                ThirdPartyCache.AutomatedBatchStatus = dictAutomatedBatchStatus;

                MessageData messageData = new MessageData();
                messageData.EventData = new List<object> { JsonHelper.SerializeObject(dictAutomatedBatchStatus), false };            
                messageData.TopicName = Topics.Topic_ThirdPartyAutomatedBatchStatus;
                Publish(messageData);
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
        /// Retrieves the status of third-party automated batches.
        /// </summary>
        /// <returns>A dictionary containing batch IDs and their corresponding status messages.</returns>
        public static Dictionary<int, string> LoadThirdPartyAutomatedBatchStatus()
        {
            Dictionary<int, string> dictAutomatedBatchStatus = new Dictionary<int, string>();
            try
            {
                DateTime currentESTTime = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, BusinessObjects.TimeZoneInfo.EasternTimeZone);
                var automatedBatchStatusData = ThirdPartyDataManager.LoadThirdPartyAutomatedBatchStatus(currentESTTime);
                if (automatedBatchStatusData != null)
                {
                    var dataTable = automatedBatchStatusData.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string status = String.Empty;
                        if (row[0] != DBNull.Value)
                        {
                            int batchID = int.Parse(row[0].ToString());
                            bool batchSuccess = row[3] != DBNull.Value && Convert.ToBoolean(row[3].ToString());
                            DateTime lastRunDate = row[4] != DBNull.Value ? DateTime.Parse(row[4].ToString()) : DateTime.MinValue;
                            int TimeBatchId = row[5] != DBNull.Value ? int.Parse(row[5].ToString()) : int.MinValue;

                            if (row[1] != DBNull.Value)
                            {
                                DateTime lastRunTime = DateTime.ParseExact(row[1].ToString(), CONST_TIME_FORMAT, CultureInfo.InvariantCulture);
                                if (currentESTTime.Date == lastRunDate.Date)
                                {
                                    status = GetAutomatedBatchStatus(lastRunDate, false, batchSuccess);
                                }
                                else
                                {
                                    ThirdPartyBatch thirdPartyBatch = GetThirdPartyBatch(batchID);
                                    if (thirdPartyBatch != null)
                                    {
                                        bool isSuccessful = SendAutomatedBatch(thirdPartyBatch, currentESTTime.ToString(), TimeBatchId);
                                        status = GetAutomatedBatchStatus(currentESTTime, false, isSuccessful);
                                    }
                                }
                            }
                            if (row[2] != DBNull.Value)
                            {
                                DateTime nextRunTime = DateTime.ParseExact(row[2].ToString(), CONST_TIME_FORMAT, CultureInfo.InvariantCulture);
                                if ((nextRunTime.TimeOfDay - currentESTTime.TimeOfDay).TotalMinutes <= ThirdPartyConstants.CONST_TIME_BEFORE_JOB_EXECUTION_FOR_NOTIFICATION || string.IsNullOrEmpty(status))
                                {
                                    status = GetAutomatedBatchStatus(nextRunTime, true);
                                }
                            }
                            dictAutomatedBatchStatus.Add(batchID, status);
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
            return dictAutomatedBatchStatus;
        }

        /// <summary>
        /// Sends an automated batch to a third party.
        /// </summary>
        /// <param name="thirdPartyBatch">The batch to send.</param>
        /// <param name="scheduledTime">The scheduled time for the batch.</param>
        /// <param name="timeBatchId">The ID of the time batch.</param>
        /// <returns>True if the batch was sent successfully; otherwise, false.</returns>
        public static bool SendAutomatedBatch(ThirdPartyBatch thirdPartyBatch, string scheduledTime, int timeBatchId)
        {
            bool isSuccessful = false;
            try
            {
                thirdPartyBatch.TransmissionType = ((int)TransmissionType.FIX).ToString();

                if (CheckConnection(thirdPartyBatch))
                {
                    View(thirdPartyBatch, DateTime.Now, false, true, null, null);
                    if (!string.IsNullOrEmpty(thirdPartyBatch.SerializedDataSource))
                    {
                        isSuccessful = SendFile(thirdPartyBatch, DateTime.Now, String.Empty, true, null, null);
                        ThirdPartyEmailHelper.SendScheduledTimeBatchStatusMail(isSuccessful, thirdPartyBatch.Description);
                    }
                    else
                    {
                        isSuccessful = true;
                    }
                }
                ThirdPartyDataManager.UpdateAutomatedBatchExecutionDetail(timeBatchId, scheduledTime, isSuccessful);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSuccessful;
        }

        /// <summary>
        /// Checks if a connection exists for the given third-party batch.
        /// </summary>
        /// <param name="batch">The third-party batch to check.</param>
        /// <returns>true if a valid connection exists; otherwise,false. </returns>
        private static bool CheckConnection(ThirdPartyBatch batch)
        {
            try
            {
                Dictionary<int, FixPartyDetails> allPartyDetails = FixEngineConnectionPoolManager.GetInstance().GetAllFixConnections();
                foreach (KeyValuePair<int, FixPartyDetails> partyDetails in allPartyDetails)
                {
                    if (partyDetails.Value.PartyID == batch.CounterPartyID && partyDetails.Value.OriginatorType == PranaServerConstants.OriginatorType.Allocation && partyDetails.Value.BuySideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED && partyDetails.Value.BuyToSellSideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        batch.BrokerConnectionType = EnumHelper.GetDescriptionWithDescriptionAttribute(partyDetails.Value.BrokerConnectionType);
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

        /// <summary>
        /// Generates the status message for an automated batch.
        /// </summary>
        /// <param name="time">The relevant time</param>
        /// <param name="isBatchNotification">True if the status is related to batch execution notification; otherwise, false.</param>
        /// <param name="batchSuccess">True if the batch was successful; otherwise, false.</param>
        /// <returns>The formatted batch status message.</returns>
        public static string GetAutomatedBatchStatus(DateTime time, bool isBatchNotification, bool batchSuccess = false)
        {
            string status = string.Empty;
            try
            {
                if (!isBatchNotification)
                {
                    // Format the status for successful or failed batch
                    status = $"Batch {(batchSuccess ? "successful" : "failed")} at [{time.ToString(CONST_TIME_FORMAT)}] EST{(batchSuccess ? "G" : "R")}";
                }
                else
                {
                    // Format the status for next scheduled time
                    status = "Next scheduled at [" + time.ToString(CONST_TIME_FORMAT) + "] EST";
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
            return status;
        }
    }
}
