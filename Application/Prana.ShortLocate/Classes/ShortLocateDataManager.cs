using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.ShortLocate.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace Prana.ShortLocate.Classes
{
    /// <summary>
    /// ShortLocateDataManager is a singelton manager class.
    /// </summary>
    public class ShortLocateDataManager : IDisposable
    {

        private static ShortLocateDataManager _shortLocateManager = null;
        private static ShortLocateUIPreferences _shortLocatePreferences;
        // cache for store the live response. 
        private Dictionary<string, SymbolData> _symbolLiveFeedResponseCache = new Dictionary<string, SymbolData>();
        private List<string> _listRequestedSymbols = new List<string>();

        // this object is use to send request for live feed and wire the method for take response. 
        private MarketDataHelper _marketDataHelperInstance = null;
        // these members are contains the start due time and interval for a periodic method.
        int _level1TimerStartDueTime = Convert.ToInt32(ConfigurationManager.AppSettings["Level1TimerStartDueTime"]);
        int _timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]);
        public event EventHandler<EventArgs<int>> ColorChangeonRow;


        // this member is the object of Timer which is use to triger a method periodicly.
        System.Threading.Timer _timer = null;

        static object _locker = new object();

        private ShortLocateDataManager()
        {
            ShortLocateDataManager.BrokerAccountMapping = SetBorrowerAccountMappingDict();
            _marketDataHelperInstance = MarketDataHelper.GetInstance();
            _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
            _shortLocatePreferences = ctrlShortLocatePrefDataManager.GetShortLocatePrefs(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
        }

        public static ShortLocateDataManager GetInstance
        {
            get
            {
                if (_shortLocateManager == null)
                {
                    lock (_locker)
                    {
                        if (_shortLocateManager == null)
                        {
                            _shortLocateManager = new ShortLocateDataManager();
                        }
                    }
                }
                return _shortLocateManager;
            }
        }

        private void LOne_OnResponse(object sender, EventArgs<SymbolData> args)
        {
            try
            {
                SymbolData data = args.Value;
                if (_symbolLiveFeedResponseCache.ContainsKey(data.Symbol))
                {
                    lock (_symbolLiveFeedResponseCache)
                    {
                        _symbolLiveFeedResponseCache[data.Symbol] = data;
                    }
                }
                else
                {
                    _symbolLiveFeedResponseCache.Add(data.Symbol, data);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static Dictionary<string, string> _brokerAccountMappingDict;
        public static Dictionary<string, string> BrokerAccountMapping
        {
            get { return _brokerAccountMappingDict; }
            set { _brokerAccountMappingDict = value; }
        }

        private static BindingList<ShortLocateOrder> _shortLocateCollection;
        public static BindingList<ShortLocateOrder> ShortLocateCollection
        {
            get { return _shortLocateCollection; }
            set { _shortLocateCollection = value; }
        }

        private static Dictionary<string, BindingList<ShortLocateListParameter>> _symbolWiseShortLocateParameter;
        public static Dictionary<string, BindingList<ShortLocateListParameter>> SymbolWiseShortLocateParameter
        {
            get { return _symbolWiseShortLocateParameter; }
            set { _symbolWiseShortLocateParameter = value; }
        }

        public void SaveShortLocateData(BindingList<ShortLocateOrder> slOrder, TransactionSource source)
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
                parameter[1] = source == TransactionSource.TradingTicket ? false : CachedDataManager.GetInstance.IsImportOverrideOnShortLocate;
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

        public void UpdateShortLocateData(ShortLocateListParameter shortlocateparameter)
        {
            // bool result = false;

            object[] parameter = new object[6];
            try
            {
                if (ShortLocate.TradeCheck != false)
                {
                    if (ShortLocateCollection == null || ShortLocateCollection.Count == 0)
                        return;
                    var order = ShortLocateCollection.FirstOrDefault(x => x.NirvanaLocateID == shortlocateparameter.NirvanaLocateID);

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
                        if (ColorChangeonRow != null)
                        {
                            ColorChangeonRow(this, new EventArgs<int>(shortlocateparameter.NirvanaLocateID));
                        }
                    }
                }
                else
                {
                    ShortLocate.TradeCheck = true;
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

        public BindingList<ShortLocateOrder> GetShortLocateCollection(string ClientMasterFund, bool isLivePriceRequired = true)
        {
            DataSet result = null;
            BindingList<ShortLocateOrder> resultCollection = new BindingList<ShortLocateOrder>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = ClientMasterFund;

                result = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetShortLocateDetails", parameter);
                if (result != null && result.Tables.Count > 0)
                {
                    DataTable requestDataTable = result.Tables[0];
                    resultCollection = GetShortLocateOrderCollection(requestDataTable);
                    ShortLocateCollection = resultCollection;
                }
                List<string> symbols = ShortLocateCollection.Select(x => x.Ticker).Distinct().Except(_listRequestedSymbols).ToList();
                if (symbols.Count > 0 && isLivePriceRequired)
                {
                    if (_marketDataHelperInstance != null)
                    {
                        _marketDataHelperInstance.RequestMultipleSymbols(symbols, false);
                        _listRequestedSymbols.AddRange(symbols);
                    }
                    _timer = new System.Threading.Timer(new TimerCallback(PeriodicTask), null, _level1TimerStartDueTime, _timerInterval);
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
            return ShortLocateCollection;
        }

        /// <summary>
        /// This method takes serially form live feed cache and update or add on the grid.
        /// </summary>
        /// <param name="state">object</param>
        private void PeriodicTask(object state)
        {
            try
            {
                foreach (var order in ShortLocateCollection)
                {
                    if (_symbolLiveFeedResponseCache.ContainsKey(order.Ticker))
                    {
                        if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            order.LastPx = _symbolLiveFeedResponseCache[order.Ticker].LastPrice;
                            order.TotalBorrowAmount = order.LastPx * order.BorrowSharesAvailable;
                            order.TotalBorrowedAmount = order.LastPx * order.BorrowedShare;
                        }
                        else
                        {
                            order.LastPx = order.TotalBorrowAmount = order.TotalBorrowedAmount = 0;
                        }
                        order.PropertyHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        public Dictionary<string, BindingList<ShortLocateListParameter>> GetSymbolWiseShortLocateParameter(string companyMasterFund)
        {
            Dictionary<string, BindingList<ShortLocateListParameter>> dictSymbolWiseParameter = new Dictionary<string, BindingList<ShortLocateListParameter>>();
            try
            {
                if (ShortLocateCollection == null || ShortLocateCollection.Count == 0)
                    ShortLocateCollection = GetShortLocateCollection(companyMasterFund, false);

                List<ShortLocateOrder> SlCollection = ShortLocateCollection.ToList();
                if (!string.IsNullOrEmpty(companyMasterFund))
                {
                    SlCollection = SlCollection.Where(x => x.ClientMasterfund == companyMasterFund).ToList();
                }
                foreach (var slOrder in SlCollection)
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
                SymbolWiseShortLocateParameter = dictSymbolWiseParameter;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dictSymbolWiseParameter;
        }

        public bool IsSymbolAvailableForShortLocate(string symbol, string broker, string borrowerid, string companyMasterFund, int NirvanaLocateID)
        {
            Dictionary<string, BindingList<ShortLocateListParameter>> check = GetSymbolWiseShortLocateParameter(companyMasterFund);
            try
            {
                if (borrowerid.Equals(string.Empty))
                {
                    if (check.ContainsKey(symbol))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else if (check != null && check.Count > 0 && check.ContainsKey(symbol))
                {
                    var list = check[symbol].Where(x => x.BorrowerId == borrowerid && x.Broker == broker).ToList();
                    if (list.Count > 0)
                    {
                        if (NirvanaLocateID == 0)
                            return false;
                        return true;
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
            return false;
        }


        private BindingList<ShortLocateOrder> GetShortLocateOrderCollection(DataTable dTable)
        {
            try
            {
                BindingList<ShortLocateOrder> shortLocateOrderCollection = new BindingList<ShortLocateOrder>();

                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    ShortLocateOrder slOrder = new ShortLocateOrder();
                    slOrder.NirvanaLocateID = Convert.ToInt32(dTable.Rows[i][ShortLocateConstants.COL_NirvanaLocateID]);
                    slOrder.Ticker = dTable.Rows[i][ShortLocateConstants.COL_Ticker].ToString();
                    slOrder.Broker = dTable.Rows[i][ShortLocateConstants.COL_Broker].ToString();
                    slOrder.StatusSource = dTable.Rows[i][ShortLocateConstants.COL_StatusSource].ToString();
                    slOrder.BorrowSharesAvailable = Convert.ToDouble(dTable.Rows[i][ShortLocateConstants.COL_BorrowSharesAvailable]);
                    slOrder.SODBorrowshareAvailable = Convert.ToDouble(dTable.Rows[i][ShortLocateConstants.COL_SODBorrowshareAvailable]);
                    slOrder.SODBorrowRate = Convert.ToDouble(dTable.Rows[i][ShortLocateConstants.COL_SODBorrowRate]);
                    slOrder.BorrowerId = dTable.Rows[i][ShortLocateConstants.COL_BorrowerId].ToString();
                    slOrder.BorrowRate = Convert.ToDouble(dTable.Rows[i][ShortLocateConstants.COL_BorrowRate]);
                    slOrder.BorrowedShare = Convert.ToDouble(dTable.Rows[i][ShortLocateConstants.COL_BorrowedShare]);
                    slOrder.BorrowedRate = Convert.ToDouble(dTable.Rows[i][ShortLocateConstants.COL_BorrowedRate]);
                    slOrder.ClientMasterfund = dTable.Rows[i][ShortLocateConstants.COL_ClientMasterfund].ToString();
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

        private static DataTable AddShortLocateColumnsToDataTable()
        {
            DataTable table = new DataTable();
            try
            {
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_BorrowerId, typeof(string)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_ClientMasterfund, typeof(string)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_Ticker, typeof(string)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_Broker, typeof(string)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_BorrowSharesAvailable, typeof(double)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_BorrowRate, typeof(double)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_BorrowedShare, typeof(double)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_BorrowedRate, typeof(double)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_SODBorrowshareAvailable, typeof(double)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_SODBorrowRate, typeof(double)));
                table.Columns.Add(new DataColumn(ShortLocateConstants.COL_StatusSource, typeof(string)));
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

        private static Dictionary<string, string> BorrowerAccountMappingDict = new Dictionary<string, string>();

        private static Dictionary<string, string> SetBorrowerAccountMappingDict()
        {

            Prana.DatabaseManager.QueryData queryData = new Prana.DatabaseManager.QueryData();
            queryData.StoredProcedureName = "GetShortLocateGridParametersDB";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        BorrowerAccountMappingDict[row[1].ToString()] = row[2].ToString();
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
            return BorrowerAccountMappingDict;
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }
                    if (_marketDataHelperInstance != null)
                    {
                        _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                        _marketDataHelperInstance.RemoveMultipleSymbols(_listRequestedSymbols);
                        _marketDataHelperInstance.Dispose();
                        _marketDataHelperInstance = null;
                        _shortLocatePreferences = null;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
