using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Prana.Rebalancer.PercentTradingTool.DAL
{
    /// <summary>
    /// Connects to database and is responsible for executing CRUD operations on data
    /// </summary>
    internal class PTTDataManager
    {

        /// <summary>
        /// Saves the PTT Preference details.
        /// </summary>
        /// <param name="pttRequestObject"></param>
        /// <param name="pttResponseObjects">The PTT response objects.</param>
        /// <param name="AllocationPrefID">The unique identifier key.</param>
        internal static void SavePTTPreferenceDetails(PTTRequestObject pttRequestObject, List<PTTResponseObject> pttResponseObjects, int AllocationPrefID)
        {
            //int result = 0;
            DataTable dtDataTable = null;
            StringWriter writer = null;
            //for more than one side, we set it to true #44394
            try
            {
                object[] parameter = new object[11];
                parameter[0] = AllocationPrefID;
                parameter[1] = pttRequestObject.TickerSymbol;
                parameter[2] = pttRequestObject.Target;
                parameter[3] = pttRequestObject.Type.Value;
                parameter[4] = pttRequestObject.AddOrSet.Value;
                parameter[5] = pttRequestObject.MasterFundOrAccount.Value;
                parameter[6] = (PTTCombineAccountTotalValue)pttRequestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.Yes ? true : false;
                parameter[7] = pttRequestObject.SelectedFeedPrice;

                dtDataTable = AddPTTColumnsToDataTable();

                dtDataTable = GeneralUtilities.CreateTableFromCollection(dtDataTable, pttResponseObjects);
                dtDataTable.TableName = "PTTDetails";
                writer = new StringWriter();
                DataColumn col = dtDataTable.Columns["PTTId"];
                foreach (DataRow row in dtDataTable.Rows)
                    row[col] = AllocationPrefID;
                dtDataTable.WriteXml(writer, true);
                parameter[8] = writer.ToString().Replace("OrderSide", PTTConstants.COL_ORDERSIDEID);

                parameter[9] = pttRequestObject.IsUseRoundLot;
                parameter[10] = DBNull.Value;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SavePTTDetails", parameter);
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
                if (writer != null)
                {
                    writer.Dispose();
                }
                if (dtDataTable != null)
                {
                    dtDataTable.Dispose();
                }
            }
        }

        /// <summary>
        /// Adds the PTT columns to data table.
        /// </summary>
        /// <returns></returns>
        private static DataTable AddPTTColumnsToDataTable()
        {
            DataTable table = new DataTable();
            try
            {
                table.Columns.Add(new DataColumn(PTTConstants.COL_PTTID, typeof(string)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_ACCOUNTID, typeof(int)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_STARTINGPOSITION, typeof(decimal)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_STARTINGVALUE, typeof(decimal)));

                table.Columns.Add(new DataColumn(PTTConstants.COL_ACCOUNTNAV, typeof(decimal)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_STARTINGPERCENTAGE, typeof(decimal)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_PERCENTAGETYPE, typeof(decimal)));

                table.Columns.Add(new DataColumn(PTTConstants.COL_TRADEQUANTITY, typeof(decimal)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_ENDINGPERCENTAGE, typeof(decimal)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_ENDINGPOSITION, typeof(decimal)));

                table.Columns.Add(new DataColumn(PTTConstants.COL_ENDINGVALUE, typeof(decimal)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_PERCENTAGEALLOCATION, typeof(decimal)));
                table.Columns.Add(new DataColumn(PTTConstants.COL_ORDERSIDE, typeof(string)));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            finally
            {
                table.Dispose();
            }
            return table;
        }

        /// <summary>
        /// Gets the order side data table.
        /// </summary>
        /// <param name="auecID">The auec identifier.</param>
        /// <returns></returns>
        internal static DataTable GetOrderSideDataTable(int assetID)
        {
            DataTable sides = null;
            try
            {
                sides = CachedDataManager.GetInstance.GetOrderSides(assetID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (sides != null)
                {
                    sides.Dispose();
                }
            }
            return sides;
        }

        /// <summary>
        /// Gets the PTT Preference details.
        /// </summary>
        /// <param name="pttRequestObject">The PTT request object.</param>
        /// <param name="pttResponseObjects">The PTT response objects.</param>
        /// <param name="allocationPrefID">The unique allocationPrefID.</param>
        /// <param name="errorMessage">errorMessage.</param>
        internal static void GetPTTPreferenceDetails(PTTAllocDetailsRequest pttRequestObject, List<PTTResponseObject> pttResponseObjects, int allocationPrefID, string symbol, string OrderSideId, StringBuilder errorMessage)
        {
            DataSet result = null;
            try
            {
                object[] parameter = new object[2];
                parameter[0] = allocationPrefID;
                parameter[1] = OrderSideId;

                result = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetPTTDetails", parameter);
                if (result != null && result.Tables.Count > 1)
                {
                    DataTable requestDataTable = result.Tables[0];
                    pttRequestObject.ExtractRequestObjectFromDataTable(requestDataTable, symbol, errorMessage);

                    DataTable resposeDataTable = result.Tables[1];
                    foreach (DataRow responseDataRow in resposeDataTable.Rows)
                    {
                        PTTResponseObject pttResponseObject = new PTTResponseObject();
                        pttResponseObject.ExtractResponseObjectFromDataRow(responseDataRow);
                        pttResponseObjects.Add(pttResponseObject);
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
    }
}