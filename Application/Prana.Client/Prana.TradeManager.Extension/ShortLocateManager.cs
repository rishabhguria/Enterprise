using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.TradeManager.Extension
{
    public class ShortLocateManager
    {
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singelton instance
        /// </summary>
        private static ShortLocateManager shortLocateManager = null;
        /// <summary>
        /// Singelton instance
        /// </summary>
        /// <returns></returns>
        public static ShortLocateManager GetInstance()
        {
            lock (_lock)
            {
                if (shortLocateManager == null)
                    shortLocateManager = new ShortLocateManager();
                return shortLocateManager;
            }
        }
        #endregion

        #region Short Locate constants
        public const string COL_Ticker = "Ticker";
        public const string COL_Broker = "Broker";
        public const string COL_ClientMasterfund = "ClientMasterfund";
        public const string COL_BorrowSharesAvailable = "BorrowSharesAvailable";
        public const string COL_BorrowRate = "BorrowRate";
        public const string COL_BorrowerId = "BorrowerId";
        public const string COL_BorrowedShare = "BorrowedShare";
        public const string COL_BorrowedRate = "BorrowedRate";
        public const string COL_SODBorrowshareAvailable = "SODBorrowshareAvailable";
        public const string COL_SODBorrowRate = "SODBorrowRate";
        public const string COL_StatusSource = "StatusSource";
        public const string COL_NirvanaLocateID = "NirvanaLocateID";
        #endregion

        private List<ShortLocateOrder> _shortLocateCollection;
        public  List<ShortLocateOrder> ShortLocateCollection
        {
            get { return _shortLocateCollection; }
            set { _shortLocateCollection = value; }
        }

        /// <summary>
        /// Gets the symbol wise ShortLocate Information
        /// </summary>
        /// <param name="symbol">The Symbol.</param>
        /// <returns></returns>
        public BindingList<ShortLocateListParameter> GetSymbolWiseShortLocateInformation(string symbol)
        {
            Dictionary<string, BindingList<ShortLocateListParameter>> dictSymbolWiseParameter = new Dictionary<string, BindingList<ShortLocateListParameter>>();
            try
            {
                List<ShortLocateOrder> symbolwiseFilteredOrders = null;
                if (_shortLocateCollection != null && _shortLocateCollection.Count>= 0)
                {
                    List<ShortLocateOrder> shortLocateOrders = new List<ShortLocateOrder>();
                    shortLocateOrders = _shortLocateCollection;                 
                    symbolwiseFilteredOrders = shortLocateOrders.FindAll(x => x.Ticker.Equals(symbol) && x.BorrowSharesAvailable>0);
                    foreach (var slOrder in symbolwiseFilteredOrders)
                    {
                        if (!dictSymbolWiseParameter.ContainsKey(slOrder.Ticker))
                        {
                            dictSymbolWiseParameter.Add(slOrder.Ticker, new BindingList<ShortLocateListParameter>());
                        }
                        ShortLocateListParameter param = new ShortLocateListParameter();
                        param.BorrowerId = slOrder.BorrowerId;
                        param.BorrowQuantity = slOrder.TradeQuantity;
                        param.BorrowRate = slOrder.BorrowRate;
                        param.BorrowSharesAvailable = slOrder.BorrowSharesAvailable;
                        param.Broker = slOrder.Broker;
                        param.NirvanaLocateID = slOrder.NirvanaLocateID;
                        dictSymbolWiseParameter[slOrder.Ticker].Add(param);
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
            return dictSymbolWiseParameter.ContainsKey(symbol)? dictSymbolWiseParameter[symbol]:new BindingList<ShortLocateListParameter>();
        }

        /// <summary>
        /// Updates the Short Locate Information in the database
        /// </summary>
        /// <param name="Symbol">The Symbol.</param>
        /// <param name="shortlocateparameter">The Short Locate Parameter.</param>
        /// <returns></returns>
        public void UpdateShortLocateData(ShortLocateListParameter shortlocateparameter)
        {
            object[] parameter = new object[6];
            try
            {
                var order = ShortLocateCollection.FirstOrDefault(x => x.NirvanaLocateID == shortlocateparameter.NirvanaLocateID);

                //Check if new Short Locate row is needed to be fetched
                if (order == null)
                {
                    var NewShortLocateCollection = FetchShortLocateDetailsForTrade();
                    order = NewShortLocateCollection.FirstOrDefault(x => x.NirvanaLocateID == shortlocateparameter.NirvanaLocateID);
                    if (order != null)
                        _shortLocateCollection.Add(order);
                }
                if (order != null)
                {
                    if (shortlocateparameter.ReplaceQuantity > 0)
                        order.BorrowedShare = shortlocateparameter.ReplaceQuantity;
                    else
                        order.BorrowedShare += shortlocateparameter.BorrowQuantity;

                    if (order.BorrowedShare >= order.SODBorrowshareAvailable)
                        order.BorrowSharesAvailable = 0;
                    else
                    {
                        order.BorrowSharesAvailable = order.SODBorrowshareAvailable - order.BorrowedShare;
                    }
                    order.BorrowedRate = shortlocateparameter.BorrowRate;
                    parameter[0] = order.NirvanaLocateID;
                    parameter[1] = order.BorrowerId;
                    parameter[2] = order.BorrowSharesAvailable;
                    parameter[3] = order.BorrowedShare;
                    parameter[4] = order.BorrowedRate;
                    parameter[5] = order.ClientMasterfund;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateShortLocateData", parameter);
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
        /// Saves the Short Locate Information in the database
        /// </summary>
        /// <param name="slOrder">The Short Locate Order.</param>
        /// <returns></returns>
        public void SaveShortLocateData(BindingList<ShortLocateOrder> slOrder)
        {
            try
            {
                DataTable dtDataTable = null;
                StringWriter writer = null;
                object[] parameter = new object[2];
                dtDataTable = AddShortLocateColumnsToDataTable();
                dtDataTable = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection(dtDataTable, slOrder.ToList());
                dtDataTable.TableName = "ShortLocateDetails";
                writer = new StringWriter();
                dtDataTable.WriteXml(writer, true);
                parameter[0] = writer.ToString();
                parameter[1] = false;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveShortLocateDetails", parameter);
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
        /// Fetch the symbol wise ShortLocate Information for the trade
        /// </summary>
        /// <param name="symbol">The Symbol.</param>
        /// <returns></returns>
        public List<ShortLocateOrder> FetchShortLocateDetailsForTrade()
        {
            List<ShortLocateOrder> symbolwiseFilteredOrders = null;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = string.Empty;

                DataSet result = null;
                result = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetShortLocateDetails", parameter);
                if (result != null && result.Tables.Count > 0)
                {
                    DataTable requestDataTable = result.Tables[0];
                    symbolwiseFilteredOrders = GetShortLocateOrderCollection(requestDataTable);
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
            return symbolwiseFilteredOrders;
        }

        /// <summary>
        /// Creates a Datatable for saving ShortLocateData
        /// </summary>
        /// <returns></returns>
        private static DataTable AddShortLocateColumnsToDataTable()
        {
            DataTable table = new DataTable();
            try
            {
                table.Columns.Add(new DataColumn(COL_BorrowerId, typeof(string)));
                table.Columns.Add(new DataColumn(COL_ClientMasterfund, typeof(string)));
                table.Columns.Add(new DataColumn(COL_Ticker, typeof(string)));
                table.Columns.Add(new DataColumn(COL_Broker, typeof(string)));
                table.Columns.Add(new DataColumn(COL_BorrowSharesAvailable, typeof(double)));
                table.Columns.Add(new DataColumn(COL_BorrowRate, typeof(double)));
                table.Columns.Add(new DataColumn(COL_BorrowedShare, typeof(double)));
                table.Columns.Add(new DataColumn(COL_BorrowedRate, typeof(double)));
                table.Columns.Add(new DataColumn(COL_SODBorrowshareAvailable, typeof(double)));
                table.Columns.Add(new DataColumn(COL_SODBorrowRate, typeof(double)));
                table.Columns.Add(new DataColumn(COL_StatusSource, typeof(string)));
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
        /// Creates a Short Locate Order collection for fetching ShortLocateData
        /// </summary>
        /// <returns></returns>
        private List<ShortLocateOrder> GetShortLocateOrderCollection(DataTable dTable)
        {
            try
            {
                List<ShortLocateOrder> shortLocateOrderCollection = new List<ShortLocateOrder>();

                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    ShortLocateOrder slOrder = new ShortLocateOrder();
                    slOrder.NirvanaLocateID = Convert.ToInt32(dTable.Rows[i][COL_NirvanaLocateID]);
                    slOrder.Ticker = dTable.Rows[i][COL_Ticker].ToString();
                    slOrder.Broker = dTable.Rows[i][COL_Broker].ToString();
                    slOrder.StatusSource = dTable.Rows[i][COL_StatusSource].ToString();
                    slOrder.BorrowSharesAvailable = Convert.ToDouble(dTable.Rows[i][COL_BorrowSharesAvailable]);
                    slOrder.SODBorrowshareAvailable = Convert.ToDouble(dTable.Rows[i][COL_SODBorrowshareAvailable]);
                    slOrder.SODBorrowRate = Convert.ToDouble(dTable.Rows[i][COL_SODBorrowRate]);
                    slOrder.BorrowerId = dTable.Rows[i][COL_BorrowerId].ToString();
                    slOrder.BorrowRate = Convert.ToDouble(dTable.Rows[i][COL_BorrowRate]);
                    slOrder.BorrowedShare = Convert.ToDouble(dTable.Rows[i][COL_BorrowedShare]);
                    slOrder.BorrowedRate = Convert.ToDouble(dTable.Rows[i][COL_BorrowedRate]);
                    slOrder.ClientMasterfund = dTable.Rows[i][COL_ClientMasterfund].ToString();
                    shortLocateOrderCollection.Add(slOrder);
                }
                return shortLocateOrderCollection;
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
    }
}
