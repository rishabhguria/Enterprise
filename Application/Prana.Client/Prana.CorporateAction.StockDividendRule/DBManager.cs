using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.SecurityMasterNew;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace Prana.CorporateAction.StockDividendRule
{
    static class DBManager
    {
        internal static bool SaveCAForStockDividend(CAOnProcessObjects caOnProcessObject, ProxyBase<IPublishing> proxyPublishing, IActivityServices activityService, IAllocationServices allocationServices, IClosingServices closingServices, int userID)
        {
            int affectedPositions_CashInLieu = 0;
            int affectedPositions1 = 0;
            //int affectedPositions2 = 0;
            int affectedPositions4 = 0;
            int affectedPositions3 = 0;
            bool isSuccessful = false;
            // Chunk Size picked from App.config
            int chunkSize = int.Parse(ConfigurationManager.AppSettings["CAToSaveGroupChunkSize"]);

            Logger.LoggerWrite("Corporate Action (Stock Dividend) data saving starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            Logger.LoggerWrite("Corporate Action (Stock Dividend): saving in Security Master DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            affectedPositions1 = SecMasterDataManager.SaveCorporateAction(caOnProcessObject.CorporateActionListString, caOnProcessObject.IsApplied);
            isSuccessful = (affectedPositions1 > 0);

            Logger.LoggerWrite("Corporate Action (Stock Dividend): saved in Security Master DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            //EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock Dividend): Taxlots affected due to Corporate action, saving in Tables PM_Taxlots and PM_CorpActionsTaxlots client DB...", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);
            //DataSet ds = AllocationDataManager.SavStockDividendForTaxlots(caOnProcessObject.Taxlots);
            //EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock Dividend): Taxlots affected due to Corporate action, saved in Tables PM_Taxlots and PM_CorpActionsTaxlots client DB", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);

            // Save Groups in Chunks  
            int chunkNo = 1;
            List<List<AllocationGroup>> taxlotsChunk = ChunkingManager.CreateChunks<AllocationGroup>(caOnProcessObject.NewGeneratedTaxlots, chunkSize);
            foreach (List<AllocationGroup> taxlotSave in taxlotsChunk)
            {
                Logger.LoggerWrite("Corporate Action (Stock Dividend): New groups (Fractional shares generated in Cash In Lieu) from Chunk " + chunkNo + " saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                affectedPositions3 = allocationServices.SaveGroups(taxlotSave, userID);
                chunkNo++;
            }
            if (affectedPositions3 <= 0)
                isSuccessful = false;

            Logger.LoggerWrite("Corporate Action (Stock Dividend): Taxlots affected due to Corporate action, saved in Tables PM_Taxlots and PM_CorpActionsTaxlots client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            CACloseData caCloseData = new CACloseData();
            caCloseData.CAID = caOnProcessObject.CorporateActionID.ToString();

            if (caOnProcessObject.ClosingData != null && caOnProcessObject.ClosingData.Taxlots.Count > 0)
            {
                Logger.LoggerWrite("Corporate Action (Stock Dividend): Closed data of fractional shares saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                affectedPositions_CashInLieu = closingServices.SaveCloseTradesData(caOnProcessObject.ClosingData);
                if (affectedPositions_CashInLieu <= 0)
                    isSuccessful = false;

                Logger.LoggerWrite("Corporate Action (Stock Dividend): Closed data of fractional shares saved in client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                foreach (Position position in caOnProcessObject.ClosingData.ClosedPositions)
                {
                    ClosingInfo closingInfo = new ClosingInfo();
                    closingInfo.ClosingID = position.TaxLotClosingId;
                    closingInfo.PositionalTaxlotID = position.ID;
                    closingInfo.ClosingTaxlotID = position.ClosingID;
                    caCloseData.CAClosingList.Add(closingInfo);
                }

                /// Save closing taxlots only for PM_Corpactiontaxlots
                Logger.LoggerWrite("Corporate Action (Stock Dividend): Taxlots affected due to Corporate action, saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                affectedPositions4 = AllocationDataManager.SaveCAWiseCloseData(caCloseData);
                if (affectedPositions4 <= 0)
                    isSuccessful = false;
            }

            //EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock Dividend) Generate Cash Activity", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //        dr.SetAdded();

            //    List<CashActivity> lsCashActivity = activityService.GenerateCashActivity(ds, CashTransactionType.CorpAction);
            //    affectedPositions2 = ds.Tables[0].Rows.Count;
            //    if (affectedPositions2 <= 0)
            //        isSuccessful = false;
            //}
            //EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock Dividend) data saving completed", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);

            if (isSuccessful)
            {
                List<TaxLot> taxlotList = GetTaxlotsForPublishing(caOnProcessObject.Taxlots, false);
                PublishAllocationData(taxlotList, proxyPublishing);
            }
            return isSuccessful;
        }

        internal static bool UndoStockDividend(CAOnProcessObjects caOnProcessObject, TaxlotBaseCollection taxlots, IClosingServices closingServices, IAllocationServices allocationServices, ProxyBase<IPublishing> proxyPublishing, IActivityServices activityService)
        {

            //int affectedPositions2 = 0;

            string caIds = caOnProcessObject.CorporateActionIDs;
            Logger.LoggerWrite("Corporate Action (Stock-Dividend) Undo data starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            //EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock-Dividend): Undoing taxlots...", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);
            //DataSet undoStockDividends = AllocationDataManager.UndoStockDividendCA(caIds);
            //EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock-Dividend): Deleting allocation group created from CA...", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);

            List<ClosingInfo> closingInfoList = FetchClosingInfo(caIds);
            // Unwind if there's any closing done
            if (closingInfoList.Count > 0)
            {
                Logger.LoggerWrite("Corporate Action (Stock-Dividend): Unwinding positions...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                bool isUnwinded = UnWindPositions(caIds, allocationServices);
                if (!isUnwinded)
                    return false;
            }
            Logger.LoggerWrite("Corporate Action (Stock-Dividend): Deleting allocation group created from CA...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions0 = allocationServices.DeleteGroupsFromCA(FetchGroupsToDeleteForCAs(caIds));
            if (affectedPositions0 <= 0)
            {
                return false;
            }

            Logger.LoggerWrite("Corporate Action (Split):  Deleting CA information from CA management on client side...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions3 = DeleteNameChangeData(caIds);

            int affectedPositions1 = SecMasterDataManager.UndoCA(caIds);

            bool isSuccessful = (affectedPositions1 > 0);

            //if (undoStockDividends != null && undoStockDividends.Tables.Count > 0 && undoStockDividends.Tables[0] != null && undoStockDividends.Tables[0].Rows.Count > 0)
            //{
            //    //so that rows flow with deleted state
            //    foreach (DataRow dr in undoStockDividends.Tables[0].Rows)
            //        dr.Delete();

            //    EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock-Dividend): Undoing Cash Activity generated from CA...", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);
            //    List<CashActivity> lsCashActivity = activityService.GenerateCashActivity(undoStockDividends.GetChanges(), CashTransactionType.CorpAction);
            //    affectedPositions2 = undoStockDividends.Tables[0].Rows.Count;
            //    if (affectedPositions2 <= 0)
            //        return false;
            //}
            //EnterpriseLibraryManager.LoggerWrite("Corporate Action (Stock-Dividend): Undo data completed", EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING);

            if (isSuccessful)
            {
                List<TaxLot> taxlotList = GetTaxlotsForPublishing(taxlots, false);
                PublishAllocationData(taxlotList, proxyPublishing);
                PublishStockDividendData(proxyPublishing);
            }
            return isSuccessful;
        }

        public static int DeleteNameChangeData(string caIDs)
        {
            int rowsAffected = 0;
            try
            {
                object[] parameter = new object[3];

                parameter[0] = caIDs;
                parameter[1] = string.Empty;
                parameter[2] = 0;

                string spName = "P_DeleteNameChangeData";
                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        // Unwinding is done with one server call
        private static bool UnWindPositions(string caIds, IAllocationServices allocationServices)
        {
            bool isSuccessful = false;
            List<ClosingInfo> closingInfoList = FetchClosingInfo(caIds);
            if (closingInfoList.Count == 0 || closingInfoList == null)
                return false;
            StringBuilder sbClosingID = new StringBuilder();
            StringBuilder sbPositionalandClosingTaxlotID = new StringBuilder();
            StringBuilder taxlotClosingIDWithClosingDate = new StringBuilder();
            CAHelperClass.GetInstance().GetClosingTaxlotFormatedID(closingInfoList, ref sbClosingID, ref sbPositionalandClosingTaxlotID, ref taxlotClosingIDWithClosingDate);
            ClosingData closingData = allocationServices.UnWindClosing(sbClosingID.ToString(), sbPositionalandClosingTaxlotID.ToString(), taxlotClosingIDWithClosingDate.ToString());
            if (closingData != null)
            {
                isSuccessful = true;
            }
            return isSuccessful;

        }

        private static DataTable FetchGroupsToDeleteForCAs(string caIds)
        {

            DataSet dsGroupAccountStrategyIDAndQty = new DataSet();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = caIds;

                dsGroupAccountStrategyIDAndQty = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetGroupsToDeleteForCA", parameter);
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
            return dsGroupAccountStrategyIDAndQty.Tables[0];
        }


        private static List<ClosingInfo> FetchClosingInfo(string caIds)
        {
            List<ClosingInfo> closingInfoList = new List<ClosingInfo>();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = caIds;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetClosingInfoForCA", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        closingInfoList.Add(FillClosingInfo(row));
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
            return closingInfoList;

        }

        private static ClosingInfo FillClosingInfo(object[] row)
        {
            int i = 0;
            ClosingInfo f = new ClosingInfo();
            try
            {
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ClosingID = row[i].ToString().Trim();
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.PositionalTaxlotID = row[i].ToString().Trim();
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.ClosingTaxlotID = row[i].ToString().Trim();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ClosingTradeDate = row[i].ToString().Trim();
                }
                i++;
            }

            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return f;
        }

        internal static List<TaxLot> GetTaxlotsForPublishing(TaxlotBaseCollection taxlots, bool isStockDividendApply)
        {
            List<TaxLot> taxlotList = new List<TaxLot>();
            foreach (TaxlotBase taxlotBase in taxlots)
            {
                TaxLot taxlot = CARulesHelper.GetTaxlotFromTaxlotBase(taxlotBase, isStockDividendApply);
                if (taxlot != null)
                {
                    taxlot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    taxlotList.Add(taxlot);
                }
            }
            return taxlotList;
        }

        private static void PublishAllocationData(List<TaxLot> taxlotsList, ProxyBase<IPublishing> proxyPublishing)
        {
            MessageData e = new MessageData();
            e.EventData = taxlotsList;
            e.TopicName = Topics.Topic_Allocation;
            proxyPublishing.InnerChannel.Publish(e, Topics.Topic_Allocation);
        }

        private static void PublishStockDividendData(ProxyBase<IPublishing> proxyPublishing)
        {
            MessageData e = new MessageData();
            e.EventData = null;
            e.TopicName = Topics.Topic_Split;

            proxyPublishing.InnerChannel.Publish(e, Topics.Topic_Split);
        }
    }
}
