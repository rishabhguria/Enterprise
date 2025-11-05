using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.TradeManager.Extension
{
    // TODO: In future refractoring will be done to handle enterprise PTT from here 24dec2024
    public class PstTradeManager
    {
        private readonly IAllocationManager allocation;

        public PstTradeManager(IAllocationManager allocation)
        {
            this.allocation = allocation ?? throw new NullReferenceException("IAllocationManager cannot be null");
        }


        /// <summary>
        /// Saves the PTT Preference details.
        /// </summary>
        /// <param name="pttRequestObject"></param>
        /// <param name="pttResponseObjects">The PTT response objects.</param>
        /// <param name="AllocationPrefID">The unique identifier key.</param>
        /// #TODO: can use this method of PTT
        /// use this after order creaed or send
        public void SavePTTPreferenceDetails(PTTRequestObject pttRequestObject,
           List<PTTResponseObject> pttResponseObjects,
           int AllocationPrefID,
           string selectedFundIds)
        {
            //int result = 0;
            DataTable dtDataTable = null;
            StringWriter writer = null;

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
                parameter[10] = selectedFundIds;
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


        //TODO: Use businObject PTTConstant
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


        public int GetMasterFundIDfromAccounts(PTTMasterFundOrAccount mfOrAccount, List<int> accountList)
        {
            int mfID = int.MinValue;

            try
            {
                if (mfOrAccount == PTTMasterFundOrAccount.MasterFund && accountList.Count == 1)
                {
                    mfID = accountList.First();
                }
                else if (mfOrAccount == PTTMasterFundOrAccount.Account && accountList.Count >= 1)
                {
                    List<int> funds = new List<int>();
                    foreach (var account in accountList)
                    {
                        int fund = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(account);
                        if (!funds.Contains(fund))
                            funds.Add(fund);
                    }
                    if (funds.Count == 1)
                        mfID = funds.First();
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
            return mfID;
        }

        public Dictionary<string, List<PTTResponseObject>> CreateDictionaryOnOrderSides(List<PTTResponseObject> responseList)
        {
            try
            {
                Dictionary<string, List<PTTResponseObject>> orderSideWiseResponseList = new Dictionary<string, List<PTTResponseObject>>();
                foreach (PTTResponseObject responseObject in responseList)
                {
                    if (responseObject.OrderSide != null)
                    {
                        if (orderSideWiseResponseList.ContainsKey(responseObject.OrderSide))
                        {
                            orderSideWiseResponseList[responseObject.OrderSide].Add(responseObject);
                        }
                        else
                        {
                            List<PTTResponseObject> listResponseObject = new List<PTTResponseObject>();
                            listResponseObject.Add(responseObject);
                            orderSideWiseResponseList.Add(responseObject.OrderSide, listResponseObject);
                        }
                    }
                }
                return orderSideWiseResponseList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public string GetMultiTradeName()
        {
            try
            {
                DateTime dateTimeValue = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                return dateTimeValue.ToString("MM-dd-yyyy") + "-" + dateTimeValue.ToString("HH:mm:ss.fff"); //ms
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return String.Empty;
        }
    }
}