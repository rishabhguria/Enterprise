using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Business Object StageImportData
    /// </summary>
    sealed public class StageImportData : IDisposable
    {

        private static readonly object syncRoot = new Object();
        #region Members

        /// <summary>
        /// The _symbology
        /// </summary>
        private int _symbology;

        /// <summary>
        /// The _symbol
        /// </summary>
        private string _symbol;

        /// <summary>
        /// contains all the orders for a particular symbol and symbology
        /// </summary>
        private List<OrderSingle> _orderSingleList;

        /// <summary>
        /// DataTable for Scheme Creation
        /// </summary>
        private DataTable _allocationScheme;


        bool _isGroupingReq_BlotterStageImport = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsGroupingReqBlotterStageImport"]);

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        /// <summary>
        /// Gets or sets the symbology.
        /// </summary>
        /// <value>
        /// The symbology.
        /// </value>
        public int Symbology
        {
            get { return _symbology; }
            set { _symbology = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor for a new StageImportData, all properties initialized in this default constructor only
        /// </summary>
        public StageImportData()
        {
            _symbology = int.MinValue;
            _symbol = string.Empty;
            _orderSingleList = new List<OrderSingle>();
        }

        /// <summary>
        /// makes StageImportData from Symbology,Symbol and OrderSingle, calling this.default constructor to initialize fields
        /// </summary>
        /// <param name="symbology"></param>
        /// <param name="symbol"></param>
        /// <param name="order"></param>
        public StageImportData(int symbology, string symbol, OrderSingle order)
            : this()
        {
            _symbology = symbology;
            _symbol = symbol;
            _orderSingleList.Add(order);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds a new OrderSingle to OrderSingleList
        /// </summary>
        /// <param name="order"></param>
        public void AddToList(OrderSingle order, AccountCollection accountCollection)
        {
            try
            {
                lock (syncRoot)
                {
                    if (_isGroupingReq_BlotterStageImport.Equals(true) || order.TransactionSource == TransactionSource.Rebalancer)
                    {
                        //If orderSingleList contains any orderSingles then check if required conditions are matched to group the orders together
                        if (_orderSingleList.Any())
                        {

                            for (int i = 0; i < _orderSingleList.Count; i++)
                            {
                                OrderSingle ordersingle = _orderSingleList[i];
                                //If these conditions are matched then the quantity for provided orderSingle will be added to underlying orderSingle
                                if (ordersingle.OrderSideTagValue.Equals(order.OrderSideTagValue) && ordersingle.TIF.Equals(order.TIF) && ordersingle.OrderTypeTagValue.Equals(order.OrderTypeTagValue)
                                    && (order.OrderTypeTagValue != "2" || order.Price.Equals(ordersingle.Price)) && ordersingle.HandlingInstruction.Equals(order.HandlingInstruction)
                                    && ordersingle.ExecutionInstruction.Equals(order.ExecutionInstruction) && ordersingle.CounterPartyID.Equals(order.CounterPartyID)
                                    && ordersingle.CurrencyID.Equals(order.CurrencyID)
                                    && ordersingle.TransactionTime.Date.Equals(order.TransactionTime.Date))
                                {
                                    if (order.Level1ID.Equals(ordersingle.Level1ID))
                                    {
                                        _orderSingleList[i].Quantity += order.Quantity;
                                        return;
                                    }
                                    else if (order.Level1ID.Equals(int.MinValue) || ordersingle.Level1ID.Equals(int.MinValue))
                                    {
                                        continue;
                                    }
                                    else if (accountCollection.Contains(order.Level1ID) && (accountCollection.Contains(ordersingle.Level1ID)))
                                    {
                                        //Allocationscheme to be created here
                                        if (_allocationScheme == null)
                                        {
                                            GetAllocationSchemeDataTable();
                                        }
                                        AddRow(ordersingle, true);
                                        AddRow(order, false);
                                        ordersingle.Level1ID = -1;
                                        //Setting TransactionSource as TradeImport 
                                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-25709 
                                        ordersingle.TransactionSourceTag = 3;
                                        ordersingle.TransactionSource = TransactionSource.TradeImport;
                                        _orderSingleList[i].Quantity += order.Quantity;
                                        return;
                                    }
                                    else if (accountCollection.Contains(order.Level1ID) && ordersingle.Level1ID == -1)
                                    {
                                        //Allocationscheme to be modified here
                                        AddRow(order, false);
                                        _orderSingleList[i].Quantity += order.Quantity;
                                        return;
                                    }

                                }
                            }
                        }
                    }
                    _orderSingleList.Add(order);
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
        /// Creates and returns a clone of orderSingleList
        /// </summary>
        public List<OrderSingle> getOrderSingleList()
        {
            lock (syncRoot)
            {
                return new List<OrderSingle>(_orderSingleList);
            }
        }

        /// <summary>
        /// Gets the symbol allocation scheme.
        /// </summary>
        /// <returns></returns>
        public DataTable GetSymbolAllocationScheme()
        {
            try
            {
                lock (syncRoot)
                {
                    if (_allocationScheme != null)
                        return _allocationScheme.Copy();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// creates new Datatable and enters the order as first row
        /// </summary>
        /// <param name="order">ordersingle.</param>
        /// <returns>The Datatable</returns>
        private void GetAllocationSchemeDataTable()
        {
            try
            {
                _allocationScheme = new DataTable();
                _allocationScheme.TableName = "PositionMaster";

                _allocationScheme.Columns.Add("Symbol", typeof(string));
                _allocationScheme.Columns.Add("FundName", typeof(string));
                _allocationScheme.Columns.Add("Quantity", typeof(string));
                _allocationScheme.Columns.Add("AllocationBasedOn", typeof(string));
                _allocationScheme.Columns.Add("OrderSideTagValue", typeof(string));
                _allocationScheme.Columns.Add("AllocationSchemeKey", typeof(string));
                _allocationScheme.Columns.Add("RIC", typeof(string));
                _allocationScheme.Columns.Add("ISIN", typeof(string));
                _allocationScheme.Columns.Add("SEDOL", typeof(string));
                _allocationScheme.Columns.Add("CUSIP", typeof(string));
                _allocationScheme.Columns.Add("Bloomberg", typeof(string));
                _allocationScheme.Columns.Add("OSIOptionSymbol", typeof(string));
                _allocationScheme.Columns.Add("IDCOOptionSymbol", typeof(string));
                _allocationScheme.Columns.Add("LongName", typeof(string));
                _allocationScheme.Columns.Add("FundID", typeof(int));
                _allocationScheme.Columns.Add("RoundLot", typeof(int));
                _allocationScheme.Columns.Add("Percentage", typeof(double));
                _allocationScheme.Columns.Add("Side", typeof(string));
                _allocationScheme.Columns.Add("TotalQty", typeof(int));
                _allocationScheme.Columns.Add("TradeType", typeof(string));
                _allocationScheme.Columns.Add("Currency", typeof(string));
                _allocationScheme.Columns.Add("PB", typeof(string));
                _allocationScheme.Columns.Add("RowIndex", typeof(int));
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
        /// Adds a new row to the scheme datatable.
        /// </summary>
        /// <param name="data">The Scheme DataTable.</param>
        /// <param name="order">The order.</param>
        /// <param name="isFirst">if set to <c>true</c> [is first].</param>
        private void AddRow(OrderSingle order, bool isFirst)
        {
            try
            {
                bool found = false;
                if (!isFirst)
                {
                    foreach (DataRow Row in _allocationScheme.Rows)
                    {
                        if (!found && Row["FundID"].ToString().Equals(order.Level1ID.ToString()) && Row["OrderSideTagValue"].Equals(order.OrderSideTagValue.ToString()))
                        {
                            Row["Quantity"] = (Int32.Parse(Row["Quantity"].ToString()) + (int)order.Quantity);
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    DataRow row = _allocationScheme.NewRow();
                    row["Symbol"] = order.Symbol;
                    row["FundName"] = string.Empty;
                    row["Quantity"] = order.Quantity;
                    row["AllocationBasedOn"] = "Symbol";
                    row["OrderSideTagValue"] = order.OrderSideTagValue;
                    row["AllocationSchemeKey"] = "SymbolSide";
                    row["RIC"] = string.Empty;
                    row["ISIN"] = string.Empty; ;
                    row["SEDOL"] = string.Empty; ;
                    row["CUSIP"] = string.Empty; ;
                    row["Bloomberg"] = string.Empty; ;
                    row["OSIOptionSymbol"] = string.Empty; ;
                    row["IDCOOptionSymbol"] = string.Empty; ;
                    row["LongName"] = string.Empty; ;
                    row["FundID"] = order.Level1ID;
                    row["RoundLot"] = order.RoundLot;
                    row["Percentage"] = 0.0;
                    row["Side"] = string.Empty; ;
                    row["TotalQty"] = 0; ;
                    row["TradeType"] = string.Empty; ;
                    row["Currency"] = string.Empty; ;
                    row["PB"] = string.Empty; ;
                    row["RowIndex"] = 0;
                    _allocationScheme.Rows.Add(row);
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

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            _allocationScheme.Dispose();
        }

        #endregion  IDisposable Members
    }
}
