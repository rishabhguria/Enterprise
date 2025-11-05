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
using System.Threading;
namespace Prana.CorporateAction.SplitRule
{
    static class DBManager
    {
        internal static bool SaveCAForSplits(CAOnProcessObjects caOnProcessObject, ProxyBase<IPublishing> proxyPublishing, IAllocationServices allocationServices, IClosingServices closingServices, int userID)
        {
            int affectedPositions_CashInLieu = 0;
            int affectedPositions1 = 0;
            int affectedPositions2 = 0;
            int affectedPositions4 = 0;
            int affectedPositions3 = 0;

            // Chunk Size picked from App.config
            int chunkSize = int.Parse(ConfigurationManager.AppSettings["CAToSaveGroupChunkSize"]);

            Logger.LoggerWrite("Corporate Action (Split) data saving starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            Logger.LoggerWrite("Corporate Action (Split): saving in Security Master DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            affectedPositions1 = SecMasterDataManager.SaveCorporateAction(caOnProcessObject.CorporateActionListString, caOnProcessObject.IsApplied);
            Logger.LoggerWrite("Corporate Action (Split): saved in Security Master DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            Logger.LoggerWrite("Corporate Action (Split): Taxlots affected due to Corporate action, saving in Tables PM_Taxlots and PM_CorpActionsTaxlots client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            affectedPositions2 = AllocationDataManager.SaveAllTaxLotsPostCorporateAction(caOnProcessObject.Taxlots);

            bool isSuccessful = (affectedPositions1 > 0) && (affectedPositions2 > 0);

            if (caOnProcessObject.NewGeneratedTaxlots != null && caOnProcessObject.NewGeneratedTaxlots.Count > 0)
            {
                // Save Groups in Chunks  
                int chunkNo = 1;
                List<List<AllocationGroup>> taxlotsChunk = ChunkingManager.CreateChunks<AllocationGroup>(caOnProcessObject.NewGeneratedTaxlots, chunkSize);
                foreach (List<AllocationGroup> taxlotSave in taxlotsChunk)
                {
                    Logger.LoggerWrite("Corporate Action (Split): New groups (Fractional shares generated in Cash In Lieu) from Chunk " + chunkNo + " saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    affectedPositions3 = allocationServices.SaveGroups(taxlotSave, userID);
                    chunkNo++;
                }
                if (affectedPositions3 <= 0)
                {
                    isSuccessful = false;
                }
            }

            Logger.LoggerWrite("Corporate Action (Split): Taxlots affected due to Corporate action, saved in Tables PM_Taxlots and PM_CorpActionsTaxlots client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            CACloseData caCloseData = new CACloseData();
            caCloseData.CAID = caOnProcessObject.CorporateActionID.ToString();

            if (caOnProcessObject.ClosingData != null && caOnProcessObject.ClosingData.Taxlots.Count > 0)
            {
                Logger.LoggerWrite("Corporate Action (Split): Closed data of fractional shares saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                affectedPositions_CashInLieu = closingServices.SaveCloseTradesData(caOnProcessObject.ClosingData);
                if (affectedPositions_CashInLieu <= 0)
                    isSuccessful = false;

                Logger.LoggerWrite("Corporate Action (Split): Closed data of fractional shares saved in client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                foreach (Position position in caOnProcessObject.ClosingData.ClosedPositions)
                {
                    ClosingInfo closingInfo = new ClosingInfo();
                    closingInfo.ClosingID = position.TaxLotClosingId;
                    closingInfo.PositionalTaxlotID = position.ID;
                    closingInfo.ClosingTaxlotID = position.ClosingID;
                    caCloseData.CAClosingList.Add(closingInfo);
                }

                /// Save closing taxlots only for PM_Corpactiontaxlots
                Logger.LoggerWrite("Corporate Action (Split): Taxlots affected due to Corporate action, saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                affectedPositions4 = AllocationDataManager.SaveCAWiseCloseData(caCloseData);
                if (affectedPositions4 <= 0)
                    isSuccessful = false;
            }

            Logger.LoggerWrite("Corporate Action (Split) data saving completed", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            if (isSuccessful)
            {
                // when we adjust fractional shares with cash in lieu, we generate fractionals shares new group(s) and close them
                // as closing done, closing publish data, so updated every where. After closing is done, we pulish all taxlots and these taxlots override the updated data as 
                // closing already published. So closed taxlots should not be published again. So we have collected these closed taxlots and excluded from the other taxlots list
                List<TaxLot> excludeTaxlots = new List<TaxLot>();
                if (caOnProcessObject.ClosingData != null && caOnProcessObject.ClosingData.Taxlots != null && caOnProcessObject.ClosingData.Taxlots.Count > 0)
                {
                    excludeTaxlots = caOnProcessObject.ClosingData.Taxlots;
                }

                List<TaxLot> taxlotList = GetTaxlotsForPublishing(caOnProcessObject.Taxlots, excludeTaxlots, true);
                PublishAllocationData(taxlotList, proxyPublishing);
                PublishSplitData(proxyPublishing);
            }

            return isSuccessful;
        }

        internal static bool UndoSplit(CAOnProcessObjects caOnProcessObject, TaxlotBaseCollection taxlots, IClosingServices closingServices, IAllocationServices allocationServices, ProxyBase<IPublishing> proxyPublishing)
        {
            bool isSuccessful = false;
            int affectedPositions2 = 0;

            string caIds = caOnProcessObject.CorporateActionIDs;
            Logger.LoggerWrite("Corporate Action (Split) Undo data starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            Logger.LoggerWrite("Corporate Action (Split): Undoing taxlots...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions1 = AllocationDataManager.UndoSplitCA(caIds);
            isSuccessful = (affectedPositions1 > 0);

            DataTable dtGroupsToDelete = FetchGroupsToDeleteForCAs(caIds);
            if (dtGroupsToDelete.Rows.Count > 0)
            {
                Logger.LoggerWrite("Corporate Action (Split): Unwinding positions...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                isSuccessful = UnWindPositions(caIds, allocationServices);

                Logger.LoggerWrite("Corporate Action (Split): Deleting allocation group created from CA...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                affectedPositions2 = allocationServices.DeleteGroupsFromCA(dtGroupsToDelete);
                if (affectedPositions2 <= 0)
                {
                    isSuccessful = false;
                }
            }

            Logger.LoggerWrite("Corporate Action (Split):  Deleting CA information from CA management on client side...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions3 = DeleteNameChangeData(caIds);
            Logger.LoggerWrite("Corporate Action (Split): Deleting CA information from CA management on SM side...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions4 = SecMasterDataManager.UndoCA(caIds);

            Logger.LoggerWrite("Corporate Action (Split): Undo data completed", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            isSuccessful = (affectedPositions3 > 0) && (affectedPositions4 > 0) && isSuccessful;

            if (isSuccessful)
            {
                List<TaxLot> taxlotList = GetTaxlotsForPublishing(taxlots, null, false);
                PublishAllocationData(taxlotList, proxyPublishing);
                PublishSplitData(proxyPublishing);
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

        // Unwinding is done with one server call
        private static bool UnWindPositions(string caIds, IAllocationServices allocationServices)
        {

            bool isSuccessful = true;
            List<ClosingInfo> closingInfoList = FetchClosingInfo(caIds);
            StringBuilder sbClosingID = new StringBuilder();
            StringBuilder sbPositionalandClosingTaxlotID = new StringBuilder();
            StringBuilder taxlotClosingIDWithClosingDate = new StringBuilder();
            CAHelperClass.GetInstance().GetClosingTaxlotFormatedID(closingInfoList, ref sbClosingID, ref sbPositionalandClosingTaxlotID, ref taxlotClosingIDWithClosingDate);
            if (closingInfoList.Count > 0 && sbClosingID.Length > 0)
            {
                ClosingData closingData = allocationServices.UnWindClosing(sbClosingID.ToString(), sbPositionalandClosingTaxlotID.ToString(), taxlotClosingIDWithClosingDate.ToString());
                if (closingData != null)
                {
                    isSuccessful = isSuccessful && true;
                }
            }
            return isSuccessful;

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

        internal static List<TaxLot> GetTaxlotsForPublishing(TaxlotBaseCollection taxlots, List<TaxLot> excludeTaxlots, bool isSplitApply)
        {
            List<TaxLot> taxlotList = new List<TaxLot>();

            Dictionary<string, TaxLot> dictExcludeTaxlots = new Dictionary<string, TaxLot>();

            if (excludeTaxlots != null && excludeTaxlots.Count > 0)
            {
                foreach (TaxLot excludeTaxlot in excludeTaxlots)
                {
                    if (!dictExcludeTaxlots.ContainsKey(excludeTaxlot.TaxLotID))
                    {
                        dictExcludeTaxlots.Add(excludeTaxlot.TaxLotID, excludeTaxlot);
                    }
                }
            }

            foreach (TaxlotBase taxlotBase in taxlots)
            {
                if (!dictExcludeTaxlots.ContainsKey(taxlotBase.L2TaxlotID))
                {
                    TaxLot taxlot = CARulesHelper.GetTaxlotFromTaxlotBase(taxlotBase, isSplitApply);

                    taxlot.SideMultiplier = Prana.BusinessLogic.Calculations.GetSideMultilpier(taxlot.OrderSideTagValue);

                    if (taxlot != null)
                    {
                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                        taxlotList.Add(taxlot);
                    }
                }
            }
            return taxlotList;
        }

        private static void PublishAllocationData(List<TaxLot> taxlotsList, ProxyBase<IPublishing> proxyPublishing)
        {
            MessageData e = new MessageData();
            e.EventData = taxlotsList;
            e.TopicName = Topics.Topic_Allocation;
            // proxyPublishing.InnerChannel.Publish(e, Topics.Topic_Allocation);
            CentralizePublish(e, proxyPublishing);
        }

        private static void PublishSplitData(ProxyBase<IPublishing> proxyPublishing)
        {
            MessageData e = new MessageData();
            e.EventData = null;
            e.TopicName = Topics.Topic_Split;

            // proxyPublishing.InnerChannel.Publish(e, Topics.Topic_Split);
            CentralizePublish(e, proxyPublishing);
        }
        private static readonly object _publishLock = new object();
        private static void CentralizePublish(MessageData msgData, ProxyBase<IPublishing> proxyPublishing)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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

    }
}
