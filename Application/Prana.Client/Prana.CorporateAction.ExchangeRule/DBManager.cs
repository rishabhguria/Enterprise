using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SecurityMasterNew;
using Prana.SecurityMasterNew.BLL;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace Prana.CorporateAction.ExchangeRule
{
    static class DBManager
    {
        internal static bool SaveCAExchangeWithDifferentSecurity(List<CAOnProcessObjects> caOnProcessObjectList, IAllocationServices allocationServices, IClosingServices closingServices, int userID)
        {
            bool isSaved = true;
            foreach (CAOnProcessObjects obj in caOnProcessObjectList)
            {
                int affectedPositions1 = 0;
                int affectedPositions2 = 0;
                int affectedPositions3 = 0;
                int affectedPositions4 = 0;
                // Chunk Size picked from App.config
                int chunkSize = int.Parse(ConfigurationManager.AppSettings["CAToSaveGroupChunkSize"]);

                string origSymbol = obj.Symbol;
                string newSymbol = obj.NewSymbol;

                ////We are generating the new symbolpks for all the options for which underlying symbol is the old symbol and
                Logger.LoggerWrite("Corporate Action (Exchange) data saving starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                Int64 symbol_PK_Underlying = SecurityMasterSymbolIDGenerator.GenerateSymbolPKID();

                //Saves data in securitymaster
                Logger.LoggerWrite("Corporate Action (Exchange): saving in Security Master DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                affectedPositions1 = SecMasterDataManager.SaveCorpActionWithSymbolAndCompanyNameChange(obj.CorporateActionListString, symbol_PK_Underlying);
                Logger.LoggerWrite("Corporate Action (Exchange): saved in Security Master DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                // Save Groups in Chunks  
                int chunkNo = 1;
                List<List<AllocationGroup>> taxlotsChunk = ChunkingManager.CreateChunks<AllocationGroup>(obj.NewGeneratedTaxlots, chunkSize);
                foreach (List<AllocationGroup> taxlotSave in taxlotsChunk)
                {
                    Logger.LoggerWrite("Corporate Action (Exchange): New groups from Chunk " + chunkNo + " saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    affectedPositions2 = allocationServices.SaveGroups(taxlotSave, userID);
                    chunkNo++;
                }
                Logger.LoggerWrite("Corporate Action (Exchange): New groups saved in client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                //Saves the closing data into PM_Taxlots and PM_Taxlotclosing
                Logger.LoggerWrite("Corporate Action (Exchange): Closed data saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                affectedPositions3 = closingServices.SaveCloseTradesData(obj.ClosingData);
                Logger.LoggerWrite("Corporate Action (Exchange): Closed data saved in client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                CACloseData caCloseData = new CACloseData();

                caCloseData.CAID = obj.CorporateActionID.ToString();
                foreach (Position position in obj.ClosingData.ClosedPositions)
                {
                    ClosingInfo closingInfo = new ClosingInfo();
                    closingInfo.ClosingID = position.TaxLotClosingId;
                    closingInfo.PositionalTaxlotID = position.ID;
                    closingInfo.ClosingTaxlotID = position.ClosingID;
                    caCloseData.CAClosingList.Add(closingInfo);
                }

                /// Saves data only for PM_Corpactiontaxlots
                Logger.LoggerWrite("Corporate Action (Exchange): Taxlots affected due to Corporate action, saving in client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                affectedPositions4 = AllocationDataManager.SaveCAWiseCloseData(caCloseData);
                Logger.LoggerWrite("Corporate Action (Exchange): Taxlots affected due to Corporate action, saved in client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                Logger.LoggerWrite("Corporate Action (Exchange) data saving completed", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                obj.IsSaved = (affectedPositions1 > 0);
                isSaved = isSaved && obj.IsSaved;
            }

            return isSaved;
        }

        internal static bool UndoExchange(CAOnProcessObjects caOnProcessObject, IClosingServices closingServices, IAllocationServices allocationServices)
        {
            string caIds = caOnProcessObject.CorporateActionIDs;

            Logger.LoggerWrite("Undo Corporate Action (Exchange) Delete Data from Security Master starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions1 = SecMasterDataManager.UndoNameChange(caIds);

            Logger.LoggerWrite("Undo Corporate Action (Exchange) Unwinding positions starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            bool isSuccessfullyUnwinded = UnWindPositions(caIds, allocationServices);

            Logger.LoggerWrite("Undo Corporate Action (Exchange) Delete Groups starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions2 = allocationServices.DeleteGroupsFromCA(FetchGroupsToDeleteForCAs(caIds));

            Logger.LoggerWrite("Undo Corporate Action (Exchange) Delete CA Information from client side starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions3 = DeleteNameChangeData(caIds);
            Logger.LoggerWrite("Corporate Action (Exchange): Undo data completed", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            if (affectedPositions1 > 0 && isSuccessfullyUnwinded && affectedPositions2 > 0 && affectedPositions3 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private static List<string> FetchGroupsToDeleteForCAs_Old(string caIds)
        {
            List<string> groupIds = new List<string>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = caIds;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetGroupsToDeleteForCA_OLD", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        groupIds.Add(row[0].ToString());
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
            return groupIds;
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
            ClosingData closingData = allocationServices.UnWindClosing(sbClosingID.ToString(), sbPositionalandClosingTaxlotID.ToString(), taxlotClosingIDWithClosingDate.ToString());
            if (closingData != null)
            {
                isSuccessful = isSuccessful && true;
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
    }
}
