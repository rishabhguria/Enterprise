using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.TradeAudit;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Prana.PostTrade
{
    public class AuditTrailDataManager
    {
        private static AuditTrailDataManager instance;
        /// <summary>
        /// lock is applied to this variable to ensure singleton behaviour while using multiple threads
        /// </summary>
        private static readonly object syncRoot = new Object();
        private static readonly object tradeAuditLock = new Object();
        private static readonly object groupTaxlotAuditLock = new Object();
        private static readonly object swapParametersAuditLock = new Object();
        private static readonly object cashjournalAuditLock = new Object();

        private static readonly int _heavyGetTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavyGetTimeout"]);

        public static AuditTrailDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AuditTrailDataManager();
                    }
                }
                return instance;
            }
        }

        public int SaveAuditList(List<TradeAuditEntry> tradeAuditCollection)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                if (tradeAuditCollection.Count == 0)
                {
                    return 0;
                }
                else
                {
                    rowsAffected = tradeAuditCollection.Count;
                }
                lock (tradeAuditLock)
                {
                    using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                    {
                        conn.Open();
                        using (SqlTransaction transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn))
                        {
                            using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
                            {
                                try
                                {
                                    copy.BatchSize = 1000;
                                    copy.ColumnMappings.Add("AUECLocalDate", "ActionDate");
                                    copy.ColumnMappings.Add("OriginalDate", "OriginalDate");
                                    copy.ColumnMappings.Add("GroupID", "GroupId");
                                    copy.ColumnMappings.Add("TaxLotID", "TaxlotId");
                                    copy.ColumnMappings.Add("ParentClOrderID", "ParentOrderId");
                                    copy.ColumnMappings.Add("ClOrderID", "OrderId");
                                    copy.ColumnMappings.Add("TaxLotClosingId", "TaxlotClosingId");
                                    copy.ColumnMappings.Add("Action", "Action");
                                    copy.ColumnMappings.Add("OriginalValue", "OriginalValue");
                                    copy.ColumnMappings.Add("NewValue", "NewValue");
                                    copy.ColumnMappings.Add("Comment", "Comment");
                                    copy.ColumnMappings.Add("CompanyUserId", "CompanyUserId");
                                    copy.ColumnMappings.Add("Symbol", "Symbol");
                                    copy.ColumnMappings.Add("Level1ID", "FundID");
                                    copy.ColumnMappings.Add("OrderSideTagValue", "OrderSideTagValue");
                                    copy.ColumnMappings.Add("Source", "Source");
                                    copy.DestinationTableName = "T_TradeAudit";

                                    List<List<TradeAuditEntry>> taxlotChunks = ChunkingManager.CreateChunks(tradeAuditCollection, 1000);
                                    ds = GeneralUtilities.CreateTableStructureFromObject(tradeAuditCollection);
                                    ds.Tables[0].Columns["AUECLocalDate"].DataType = typeof(DateTime);
                                    ds.Tables[0].Columns["TaxLotClosingId"].DataType = typeof(Guid);
                                    ds.Tables[0].Columns["CompanyUserId"].DataType = typeof(int);
                                    ds.Tables[0].Columns["Level1ID"].DataType = typeof(int);
                                    foreach (List<TradeAuditEntry> list in taxlotChunks)
                                    {
                                        GeneralUtilities.FillDataSetFromCollection(list, ref ds, true, false);
                                        copy.WriteToServer(ds.Tables[0]);
                                        ds.Tables[0].Clear();
                                    }
                                    taxlotChunks.Clear();
                                    copy.ColumnMappings.Clear();
                                    transaction.Commit();
                                    ds = null;
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    //unreachable code error 
                                    rowsAffected = 0;
                                    throw new Exception("Not able to save Audit data to T_TradeAudit, Please check", ex);

                                }
                            }

                        }
                    }
                }

                string taxlotXmlToUpdate = "<TaxLots>";
                foreach (TradeAuditEntry item in tradeAuditCollection)
                {
                    if (item.Action == TradeAuditActionType.ActionType.Unwinding && String.IsNullOrWhiteSpace(item.OrderSideTagValue))
                    {
                        continue;
                    }
                    if (!String.IsNullOrWhiteSpace(item.GroupID) && !String.IsNullOrWhiteSpace(item.TaxLotID) && !String.IsNullOrWhiteSpace(item.Level1AllocationID))
                        taxlotXmlToUpdate += String.Format("<TaxLot GroupID =\"{0}\" TaxLotID =\"{1}\" Level1AllocationID =\"{2}\" Action=\"{3}\"/>", item.GroupID, item.TaxLotID, item.Level1AllocationID, item.Action);
                }

                taxlotXmlToUpdate += "</TaxLots>";

                UpdatePBWiseTaxlotState(taxlotXmlToUpdate);
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

        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;
        private int rowsAffected = 0;

        private void UpdatePBWiseTaxlotState(string taxlotXmlToUpdate)
        {
            try
            {
                DbTransaction transaction = null;
                using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                {
                    conn.Open();
                    transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn);

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_UpdatePBWiseTaxlotState";
                    queryData.CommandTimeout = 50000;//3000; - This is in seconds so changed from 3000 seconds to 50000 Seconds
                    queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@Xml",
                        ParameterType = DbType.String,
                        ParameterValue = taxlotXmlToUpdate.ToString()
                    });

                    AddOutErrorParameters(queryData);

                    rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                    XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    transaction.Commit();
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
        }

        private void AddOutErrorParameters(QueryData queryData)
        {
            queryData.DictionaryDatabaseParameter.Add("@ErrorMessage", new DatabaseParameter()
            {
                IsOutParameter = true,
                ParameterName = "@ErrorMessage",
                ParameterType = DbType.String,
                OutParameterSize = -1
            });

            queryData.DictionaryDatabaseParameter.Add("@ErrorNumber", new DatabaseParameter()
            {
                IsOutParameter = true,
                ParameterName = "@ErrorNumber",
                ParameterType = DbType.Int32,
                ParameterValue = 0,
                OutParameterSize = sizeof(Int32)
            });
        }

        public int SaveAuditListForDailyValuation(List<TradeAuditEntry> tradeAuditCollection)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                if (tradeAuditCollection.Count == 0)
                {
                    return 0;
                }
                else
                {
                    rowsAffected = tradeAuditCollection.Count;
                }
                lock (tradeAuditLock)
                {
                    QueryData queryData = new QueryData();

                    if (tradeAuditCollection[0].Action == TradeAuditActionType.ActionType.MarkPrice_Changed)
                    {
                        queryData.StoredProcedureName = "P_SaveMarkPriceChangesinAuditTrail";
                    }
                    else
                    {
                        queryData.StoredProcedureName = "P_SaveForexRateChangesinAuditTrail";
                    }
                    ds = GeneralUtilities.CreateTableStructureFromObject(tradeAuditCollection);
                    GeneralUtilities.FillDataSetFromCollection(tradeAuditCollection, ref ds, true, false);

                    queryData.DictionaryDatabaseParameter.Add("@tradeAuditCollection", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@tradeAuditCollection",
                        ParameterType = DbType.String,
                        ParameterValue = ds.GetXml()
                    });
                    queryData.CommandTimeout = 300;

                    DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                    ds.Tables[0].Clear();
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
            return rowsAffected;
        }

        public int SaveAuditListForCashJournal(List<CashJournalAuditEntry> cashAuditCollection)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                DataSet distinctDataToSave = new DataSet();
                if (cashAuditCollection.Count == 0)
                {
                    return 0;
                }
                else
                {
                    rowsAffected = cashAuditCollection.Count;
                }
                lock (cashjournalAuditLock)
                {
                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_SaveCashJournalChangesinAuditTrail";

                    ds = GeneralUtilities.CreateTableStructureFromObject(cashAuditCollection);
                    GeneralUtilities.FillDataSetFromCollection(cashAuditCollection, ref ds, true, false);
                    distinctDataToSave.Tables.Add(ds.Tables[0].DefaultView.ToTable(true));

                    queryData.DictionaryDatabaseParameter.Add("@cashAuditCollection", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@cashAuditCollection",
                        ParameterType = DbType.String,
                        ParameterValue = distinctDataToSave.GetXml()
                    });

                    queryData.CommandTimeout = 300;

                    DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                    ds.Tables[0].Clear();
                    distinctDataToSave.Tables[0].Clear();
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
            return rowsAffected;
        }

        public int SaveAuditDeletedTaxlots(List<PM_Taxlots_DeletedAudit> deletedTaxlots)
        {
            int rowsAffected = 0;

            try
            {
                DataSet ds = new DataSet();
                if (deletedTaxlots.Count == 0)
                {
                    return 0;
                }
                else
                {
                    rowsAffected = deletedTaxlots.Count;
                }
                lock (groupTaxlotAuditLock)
                {
                    using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                    {
                        conn.Open();
                        using (SqlTransaction transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn))
                        {
                            using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
                            {
                                try
                                {
                                    copy.BatchSize = 1000;
                                    copy.ColumnMappings.Add("TaxLotID", "TaxLotID");
                                    copy.ColumnMappings.Add("Symbol", "Symbol");
                                    copy.ColumnMappings.Add("TaxLotQty", "TaxLotOpenQty");
                                    copy.ColumnMappings.Add("AvgPrice", "AvgPrice");
                                    copy.ColumnMappings.Add("TimeOfSaveUTC", "TimeOfSaveUTC");
                                    copy.ColumnMappings.Add("GroupID", "GroupID");
                                    copy.ColumnMappings.Add("AUECModifiedDate", "AUECModifiedDate");
                                    copy.ColumnMappings.Add("Level1ID", "FundID");
                                    copy.ColumnMappings.Add("Level2ID", "Level2ID");
                                    copy.ColumnMappings.Add("OpenTotalCommissionandFees", "OpenTotalCommissionandFees");
                                    copy.ColumnMappings.Add("ClosedTotalCommissionandFees", "ClosedTotalCommissionandFees");
                                    copy.ColumnMappings.Add("PositionTag", "PositionTag");
                                    copy.ColumnMappings.Add("OrderSideTagValue", "OrderSideTagValue");
                                    copy.ColumnMappings.Add("TaxLotClosingId", "TaxLotClosingId");
                                    //Save LotID and ExternalTransID in closing taxlot also
                                    copy.ColumnMappings.Add("LotId", "LotId");
                                    copy.ColumnMappings.Add("ExternalTransId", "ExternalTransId");
                                    copy.ColumnMappings.Add("TradeAttribute1", "TradeAttribute1");
                                    copy.ColumnMappings.Add("TradeAttribute2", "TradeAttribute2");
                                    copy.ColumnMappings.Add("TradeAttribute3", "TradeAttribute3");
                                    copy.ColumnMappings.Add("TradeAttribute4", "TradeAttribute4");
                                    copy.ColumnMappings.Add("TradeAttribute5", "TradeAttribute5");
                                    copy.ColumnMappings.Add("TradeAttribute6", "TradeAttribute6");
                                    copy.ColumnMappings.Add("AccruedInterest", "AccruedInterest");
                                    copy.ColumnMappings.Add("FXRate", "FXRate");
                                    copy.ColumnMappings.Add("FXConversionMethodOperator", "FXConversionMethodOperator");
                                    copy.DestinationTableName = "PM_Taxlots_DeletedAudit";

                                    List<List<PM_Taxlots_DeletedAudit>> taxlotChunks = ChunkingManager.CreateChunks(deletedTaxlots, 1000);
                                    ds = GeneralUtilities.CreateTableStructureFromObject(deletedTaxlots);
                                    ds.Tables[0].Columns["TaxLotClosingId"].DataType = typeof(Guid);
                                    ds.Tables[0].Columns["PositionTag"].DataType = typeof(Int32);
                                    foreach (List<PM_Taxlots_DeletedAudit> list in taxlotChunks)
                                    {
                                        GeneralUtilities.FillDataSetFromCollection(list, ref ds, true, false);
                                        copy.WriteToServer(ds.Tables[0]);
                                        ds.Tables[0].Clear();
                                    }
                                    taxlotChunks.Clear();
                                    copy.ColumnMappings.Clear();
                                    //Xml = string.Empty;
                                    transaction.Commit();
                                    ds = null;
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    rowsAffected = 0;
                                    throw new Exception("Not able to save Audit data to PM_Taxlots_DeletedAudit, Please check", ex);

                                }
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
            return rowsAffected;
        }

        public int SaveAuditDeletedGroups(List<T_Group_DeletedAudit> deletedGroups)
        {
            int rowsAffected = 0;

            try
            {
                DataSet ds = new DataSet();
                if (deletedGroups.Count == 0)
                {
                    return 0;
                }
                else
                {
                    rowsAffected = deletedGroups.Count;
                }
                lock (groupTaxlotAuditLock)
                {
                    using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                    {
                        conn.Open();
                        using (SqlTransaction transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn))
                        {
                            using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
                            {
                                try
                                {
                                    copy.BatchSize = 1000;
                                    copy.ColumnMappings.Add("GroupID", "GroupID");
                                    copy.ColumnMappings.Add("OrderSideTagValue", "OrderSideTagValue");
                                    copy.ColumnMappings.Add("Symbol", "Symbol");
                                    copy.ColumnMappings.Add("OrderTypeTagValue", "OrderTypeTagValue");
                                    copy.ColumnMappings.Add("CounterPartyID", "CounterPartyID");
                                    copy.ColumnMappings.Add("VenueID", "VenueID");
                                    copy.ColumnMappings.Add("TradingAccountID", "TradingAccountID");
                                    copy.ColumnMappings.Add("AuecID", "AUECID");
                                    copy.ColumnMappings.Add("CumQty", "CumQty");
                                    copy.ColumnMappings.Add("AllocatedQty", "AllocatedQty");
                                    copy.ColumnMappings.Add("Quantity", "Quantity");
                                    copy.ColumnMappings.Add("AvgPrice", "AvgPrice");
                                    copy.ColumnMappings.Add("IsPreAllocated", "IsPreAllocated");
                                    copy.ColumnMappings.Add("ListID", "ListID");
                                    copy.ColumnMappings.Add("UserID", "UserID");
                                    copy.ColumnMappings.Add("IsProrataActive", "ISProrataActive");
                                    copy.ColumnMappings.Add("AutoGrouped", "AutoGrouped");
                                    copy.ColumnMappings.Add("State", "StateID");//enum
                                    copy.ColumnMappings.Add("IsManualGroup", "IsManualGroup");
                                    copy.ColumnMappings.Add("AllocationDate", "AllocationDate");
                                    copy.ColumnMappings.Add("SettlementDate", "SettlementDate");
                                    copy.ColumnMappings.Add("AssetID", "AssetID");
                                    copy.ColumnMappings.Add("UnderlyingID", "UnderLyingID");
                                    copy.ColumnMappings.Add("ExchangeID", "ExchangeID");
                                    copy.ColumnMappings.Add("CurrencyID", "CurrencyID");
                                    copy.ColumnMappings.Add("Description", "Description");
                                    copy.ColumnMappings.Add("InternalComments", "InternalComments");
                                    copy.ColumnMappings.Add("AUECLocalDate", "AUECLocalDate");
                                    copy.ColumnMappings.Add("IsSwapped", "IsSwapped");
                                    copy.ColumnMappings.Add("AvgFXRateForTrade", "FXRate");
                                    copy.ColumnMappings.Add("FXConversionMethodOperator", "FXConversionMethodOperator");
                                    copy.ColumnMappings.Add("TaxLotClosingId", "TaxlotClosingID_Fk");
                                    copy.ColumnMappings.Add("Commission", "Commission");
                                    copy.ColumnMappings.Add("OtherBrokerfees", "OtherBrokerFees");
                                    copy.ColumnMappings.Add("StampDuty", "StampDuty");
                                    copy.ColumnMappings.Add("TransactionLevy", "TransactionLevy");
                                    copy.ColumnMappings.Add("ClearingFee", "ClearingFee");
                                    copy.ColumnMappings.Add("TaxOnCommissions", "TaxOnCommissions");
                                    copy.ColumnMappings.Add("MiscFees", "MiscFees");
                                    copy.ColumnMappings.Add("SecFee", "SecFee");
                                    copy.ColumnMappings.Add("OccFee", "OccFee");
                                    copy.ColumnMappings.Add("OrfFee", "OrfFee");
                                    copy.ColumnMappings.Add("AccruedInterest", "AccruedInterest");
                                    copy.ColumnMappings.Add("ProcessDate", "ProcessDate");
                                    copy.ColumnMappings.Add("OriginalPurchaseDate", "OriginalPurchaseDate");
                                    copy.ColumnMappings.Add("IsModified", "IsModified");
                                    copy.ColumnMappings.Add("AllocationSchemeID", "AllocationSchemeID");
                                    copy.ColumnMappings.Add("CommissionSource", "CommissionSource");
                                    copy.ColumnMappings.Add("TradeAttribute1", "TradeAttribute1");
                                    copy.ColumnMappings.Add("TradeAttribute2", "TradeAttribute2");
                                    copy.ColumnMappings.Add("TradeAttribute3", "TradeAttribute3");
                                    copy.ColumnMappings.Add("TradeAttribute4", "TradeAttribute4");
                                    copy.ColumnMappings.Add("TradeAttribute5", "TradeAttribute5");
                                    copy.ColumnMappings.Add("TradeAttribute6", "TradeAttribute6");
                                    copy.ColumnMappings.Add("TaxLotIdsWithAttributes", "TaxlotIdsWithAttributes");
                                    copy.ColumnMappings.Add("TransactionType", "TransactionType");
                                    copy.ColumnMappings.Add("OptionPremiumAdjustment", "OptionPremiumAdjustment");

                                    copy.DestinationTableName = "T_Group_DeletedAudit";

                                    List<List<T_Group_DeletedAudit>> groupChunks = ChunkingManager.CreateChunks(deletedGroups, 1000);
                                    ds = GeneralUtilities.CreateTableStructureFromObject(deletedGroups);
                                    ds.Tables[0].Columns["TaxLotClosingId"].DataType = typeof(Guid);
                                    ds.Tables[0].Columns["State"].DataType = typeof(Int32);
                                    foreach (List<T_Group_DeletedAudit> list in groupChunks)
                                    {
                                        GeneralUtilities.FillDataSetFromCollection(list, ref ds, false, false);
                                        copy.WriteToServer(ds.Tables[0]);
                                        ds.Tables[0].Clear();
                                    }
                                    groupChunks.Clear();
                                    copy.ColumnMappings.Clear();
                                    transaction.Commit();
                                    ds = null;
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    rowsAffected = 0;
                                    throw new Exception("Not able to save Audit data to T_Group_DeletedAudit, Please check", ex);
                                }
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
            return rowsAffected;
        }

        public int SaveAuditDeletedSwap(List<SwapParameters> deletedSwaps)
        {
            int rowsAffected = 0;

            try
            {
                DataSet ds = new DataSet();
                if (deletedSwaps.Count == 0)
                {
                    return 0;
                }
                else
                {
                    rowsAffected = deletedSwaps.Count;
                }
                lock (swapParametersAuditLock)
                {
                    using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                    {
                        conn.Open();
                        using (SqlTransaction transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn))
                        {
                            using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
                            {
                                try
                                {
                                    copy.BatchSize = 1000;
                                    copy.ColumnMappings.Add("GroupID", "GroupID");
                                    copy.ColumnMappings.Add("NotionalValue", "NotionalValue");
                                    copy.ColumnMappings.Add("BenchMarkRate", "BenchMarkRate");
                                    copy.ColumnMappings.Add("Differential", "Differential");
                                    copy.ColumnMappings.Add("OrigCostBasis", "OrigCostBasis");
                                    copy.ColumnMappings.Add("DayCount", "DayCount");
                                    copy.ColumnMappings.Add("SwapDescription", "SwapDescription");
                                    copy.ColumnMappings.Add("FirstResetDate", "FirstResetDate");
                                    copy.ColumnMappings.Add("OrigTransDate", "OrigTransDate");
                                    copy.ColumnMappings.Add("ResetFrequency", "ResetFrequency");
                                    copy.ColumnMappings.Add("ClosingPrice", "ClosingPrice");
                                    copy.ColumnMappings.Add("ClosingDate", "ClosingDate");
                                    copy.ColumnMappings.Add("TransDate", "TransDate");
                                    copy.DestinationTableName = "T_SwapParameters_DeletedAudit";

                                    List<List<SwapParameters>> groupChunks = ChunkingManager.CreateChunks(deletedSwaps, 1000);
                                    ds = GeneralUtilities.CreateTableStructureFromObject(deletedSwaps);

                                    foreach (List<SwapParameters> list in groupChunks)
                                    {
                                        GeneralUtilities.FillDataSetFromCollection(list, ref ds, true, true);
                                        copy.WriteToServer(ds.Tables[0]);
                                        ds.Tables[0].Clear();
                                    }
                                    groupChunks.Clear();
                                    copy.ColumnMappings.Clear();
                                    transaction.Commit();
                                    ds = null;
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    rowsAffected = 0;
                                    throw new Exception("Not able to save Audit data to T_SwapParameters_DeletedAudit, Please check", ex);
                                }
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
            return rowsAffected;
        }

        public DataTable GetTradeAuditsForGroups(List<string> groupIds, string ignoredUser, string accountIdsCommaSeparated)
        {
            DataSet dsAuditEntriesForSpecificGroups = new DataSet();
            try
            {
                lock (tradeAuditLock)
                {
                    object[] parameter = new object[3];
                    parameter[0] = String.Join(",", groupIds.ToArray());
                    if (String.IsNullOrEmpty(ignoredUser))
                        parameter[1] = DBNull.Value;
                    else
                        parameter[1] = ignoredUser;
                    if (String.IsNullOrEmpty(accountIdsCommaSeparated))
                        parameter[2] = DBNull.Value;
                    else
                        parameter[2] = accountIdsCommaSeparated;
                    string spName = "P_GetAuditEntriesForGroupIds";
                    dsAuditEntriesForSpecificGroups = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter, timeout: _heavyGetTimeout);
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
            return dsAuditEntriesForSpecificGroups.Tables[0];
        }

        public DataTable GetTradeAuditsForDates(AuditTrailFilterParams searchParams)
        {
            DataSet dsAuditEntries = new DataSet();
            try
            {
                lock (tradeAuditLock)
                {
                    object[] parameter = new object[7];
                    parameter[0] = searchParams.FromDate.Date.ToString();
                    parameter[1] = searchParams.ToDate.Date.ToString();
                    if (String.IsNullOrEmpty(searchParams.Symbol))
                    {
                        parameter[2] = DBNull.Value;
                    }
                    else
                    {
                        parameter[2] = searchParams.Symbol;
                    }
                    if (searchParams.AccountIDs == null)
                        parameter[3] = DBNull.Value;
                    else
                        parameter[3] = searchParams.AccountIDs;
                    if (String.IsNullOrEmpty(searchParams.OrderSides))
                    {
                        parameter[4] = DBNull.Value;
                    }
                    else
                    {
                        parameter[4] = searchParams.OrderSides;
                    }
                    if (String.IsNullOrEmpty(searchParams.IgnoredUsers))
                    {
                        parameter[5] = DBNull.Value;
                    }
                    else
                    {
                        parameter[5] = searchParams.IgnoredUsers;
                    }

                    if (String.IsNullOrEmpty(searchParams.SourceIDs))
                    {
                        parameter[6] = DBNull.Value;
                    }
                    else
                    {
                        parameter[6] = searchParams.SourceIDs;
                    }
                    string spName = "P_GetAuditEditedByDate";
                    dsAuditEntries = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter);
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
            return dsAuditEntries.Tables[0];
        }

        internal string GetIgnoredUsersForAudit(int companyId, int companyUserId)
        {
            DataSet dsAuditEntries = new DataSet();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = companyUserId;
                parameter[1] = companyId;

                string spName = "P_GetAuditTrailIgnoredUsers";
                dsAuditEntries = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter);
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
            if ((dsAuditEntries.Tables[0].Rows.Count) > 0)
            {
                return dsAuditEntries.Tables[0].Rows[0]["IgnoredUsers"].ToString();
            }
            else
                return "";

        }

        /// <summary>
        /// Get Order Details By Ids
        /// </summary>
        /// <param name="parentOrderID"></param>
        /// <param name="clOrderId"></param>
        /// <returns></returns>
        internal DataTable GetOrderDetailsByIds(string parentOrderID, string clOrderId)
        {
            DataSet dsTables = new DataSet();
            try
            {
                lock (groupTaxlotAuditLock)
                {

                    object[] parameter = new object[2];
                    parameter[0] = parentOrderID;
                    parameter[1] = clOrderId;
                    //parameter[2] = auditId;
                    string spName = "P_GetOrderDetailInAudit";
                    dsTables = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter);
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
            return dsTables.Tables[0];
        }

        internal DataTable GetDetailsGroupTaxlotForIds(string groupId, string taxlotId)
        {
            DataSet dsTables = new DataSet();
            try
            {
                lock (groupTaxlotAuditLock)
                {
                    object[] parameter = new object[2];
                    parameter[0] = groupId;
                    parameter[1] = taxlotId;
                    //parameter[2] = auditId;
                    string spName = "P_GetGroupsOrTaxlotsDetailInAudit";
                    dsTables = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter);
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
            return dsTables.Tables[0];
        }
    }
}
