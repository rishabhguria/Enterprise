using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
namespace Prana.DataManager
{
    public class CacheManagerDAL
    {
        private static CacheManagerDAL _cacheManagerDAL;

        //MultiDay Execution cache
        static Dictionary<string, MultiBrokersSubsCollection> _stagedSubCollection = new Dictionary<string, MultiBrokersSubsCollection>();
        private static readonly int _heavyGetTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavyGetTimeout"]);
        static CacheManagerDAL()
        {
            _cacheManagerDAL = new CacheManagerDAL();
        }
        public static CacheManagerDAL GetInstance()
        {
            return _cacheManagerDAL;
        }

        public PranaMessage GetOrderDetailsByOrderID(string _clOrderID)
        {
            Order order = null;
            PranaMessage pranaMessage = null;
            object[] parameter = new object[1];
            try
            {
                parameter[0] = _clOrderID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderSummaryByOrderID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        order = FillOrderDetails(row, 0);
                    }
                }
                if (order != null)
                    pranaMessage = Transformer.CreatePranaMessageThroughReflection(order);
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
            return pranaMessage;
        }

        /// <summary>
        /// Update DayWise MultiDay Fields
        /// </summary>
        /// <param name="pranaMessage"></param>
        public void UpdateDayWiseMultiDayFields(PranaMessage pranaMessage)
        {
            try
            {
                string clOrderID = String.Empty;
                string transactionTime = string.Empty;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                    clOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;


                if (!string.IsNullOrEmpty(clOrderID) && _stagedSubCollection.ContainsKey(clOrderID))
                {
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTransactTime))
                        transactionTime = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime].Value;
                    else
                    {
                        transactionTime = DateTimeConstants.GetCurrentTimeInFixFormat();
                    }
                    if (!transactionTime.Contains("/"))
                        transactionTime = DateTime.ParseExact(transactionTime, DateTimeConstants.NirvanaDateTimeFormat, null).ToString("MM/dd/yyyy HH:mm:ss tt");
                    DateTime auecLocalDate = Convert.ToDateTime(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(Convert.ToDateTime(transactionTime), CachedDataManager.GetInstance.GetAUECTimeZone(_stagedSubCollection[clOrderID].AUECID)));

                    //In case the AUEC-date change happens for any Multi-Day trade during the time when Trade Server is UP so need to reset the cache
                    if (_stagedSubCollection[clOrderID].CurrentAuecDate.Date < auecLocalDate.Date)
                    {
                        _stagedSubCollection[clOrderID].CurrentAuecDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(_stagedSubCollection[clOrderID].AUECID));
                        CalculateMultiDayFieldsInCache(_stagedSubCollection[clOrderID]);
                    }

                    double currentDayCumQty = 0;
                    if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagDayCumQty) && pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                    {
                        currentDayCumQty = (Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value) - _stagedSubCollection[clOrderID].StartOfDayCumQty);
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagDayCumQty, currentDayCumQty.ToString());
                    }

                    if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagDayAvgPx) && pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAvgPx) && currentDayCumQty != 0)
                    {
                        double grossNotional = Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value) * Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value);
                        double startOfDayNotional = _stagedSubCollection[clOrderID].StartOfDayCumQty * _stagedSubCollection[clOrderID].StartOfDayAveragePrice;
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagDayAvgPx, ((grossNotional - startOfDayNotional) / currentDayCumQty).ToString());
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

        public List<PranaMessage> GetTodaysOrders()
        {
            List<Order> listOrders = new List<Order>();
            List<PranaMessage> listPranaMessage = new List<PranaMessage>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetTodaysOrderSummary";
                queryData.CommandTimeout = _heavyGetTimeout;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        listOrders.Add(FillOrderDetails(row, 0));
                    }
                }
                if (listOrders != null && listOrders.Count > 0)
                {
                    foreach (Order order in listOrders)
                        listPranaMessage.Add(Transformer.CreatePranaMessageThroughReflection(order));
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
            return listPranaMessage;
        }

        public Order FillOrderDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Order _order = null;
            try
            {
                if (row != null)
                {
                    _order = new Order();
                    int PARENTID = offset + 0;
                    int CLORDERID = offset + 1;
                    int PARENTCLORDERID = offset + 2;
                    int ISSUBORDER = offset + 3;
                    int LAST_PRICE = offset + 4;
                    int AVGPRICE = offset + 5;
                    int ISSTAGED = offset + 6;
                    int LEAVESQTY = offset + 7;
                    int CUMQTY = offset + 8;
                    int ORDERSTATUS = offset + 9;
                    int LASTSHARES = offset + 10;
                    int QUANTITY = offset + 11;
                    int SYMBOL = offset + 12;
                    int ORDERSIDE = offset + 13;
                    int ORDERTYPE = offset + 14;
                    int PRICE = offset + 15;
                    int TRADINGACCOUNTNAME = offset + 16;
                    int ORIGCLORDERID = offset + 17;
                    int ISMANUAL = offset + 18;
                    int CLIENTTIME = offset + 19;
                    int COUNTERPARTYID = offset + 20;
                    int VENUEID = offset + 21;
                    int AUECID = offset + 22;
                    int ASSETID = offset + 23;
                    int UNDERLYINGID = offset + 24;
                    int FLAG = offset + 25;
                    int STAGEDORDERID = offset + 26;
                    int TRADINGACCOUNTID = offset + 27;
                    int USERID = offset + 28;
                    int Prana_MSG_TYPE = offset + 29;
                    int DISCR_OFFSET = offset + 30;
                    int PEG_DIFF = offset + 31;
                    int STOP_PRICE = offset + 32;
                    int CLEARANCE_TIME = offset + 33;
                    int MATURITY_YEARMONTH = offset + 34;
                    int STRIKE_PRICE = offset + 35;
                    int PUT_CALL = offset + 36;
                    int SECURITY_TYPE = offset + 37;
                    int OPEN_CLOSE = offset + 38;
                    int ORDER_SEQ_NUMBER = offset + 39;
                    int FUNDID = offset + 40;
                    int STRATEGYID = offset + 41;
                    int AUECLocalDate = offset + 42;
                    int SETTLEMENTDATE = offset + 43;
                    int PROCESSDATE = offset + 44;
                    int CHANGETYPE = offset + 45;
                    int SettlementCurrencyID = offset + 46;
                    int currencyID = offset + 47;
                    int avgFxRateForTrade = offset + 48;
                    int swapParameters = offset + 49;
                    int TIME_IN_FORCE = offset + 50;
                    int EXCHANGE_ID = offset + 51;
                    int HANDLING_INST = offset + 52;
                    int EXPIRETIME = offset + 53;
                    int CALCBASIS = offset + 54;
                    int SOFTCACLBASIS = offset + 55;
                    int COMMRATE = offset + 56;
                    int SOFTCOMMRATE = offset + 57;
                    int ROUNDLOT = offset + 58;

                    _order.CalcBasis = (BusinessObjects.AppConstants.CalculationBasis)Enum.Parse(typeof(BusinessObjects.AppConstants.CalculationBasis), row[CALCBASIS].ToString());
                    _order.SoftCommissionCalcBasis = (BusinessObjects.AppConstants.CalculationBasis)Enum.Parse(typeof(BusinessObjects.AppConstants.CalculationBasis), row[SOFTCACLBASIS].ToString());
                    _order.CommissionRate = double.Parse(row[COMMRATE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.SoftCommissionRate = double.Parse(row[SOFTCOMMRATE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.RoundLot = decimal.Parse(row[ROUNDLOT].ToString(), System.Globalization.NumberStyles.Float);
                    _order.ClOrderID = row[CLORDERID].ToString();
                    _order.ParentClOrderID = row[PARENTCLORDERID].ToString();

                    _order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.LastPrice = double.Parse(row[LAST_PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.LeavesQty = double.Parse(row[LEAVESQTY].ToString(), System.Globalization.NumberStyles.Float);
                    _order.CumQty = double.Parse(row[CUMQTY].ToString(), System.Globalization.NumberStyles.Float);
                    _order.LastShares = double.Parse(row[LASTSHARES].ToString(), System.Globalization.NumberStyles.Float);

                    _order.OrderStatusTagValue = row[ORDERSTATUS].ToString().Trim();

                    _order.AvgPrice = float.Parse(row[AVGPRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.Quantity = Double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);
                    _order.Symbol = row[SYMBOL].ToString();

                    _order.OrderSideTagValue = row[ORDERSIDE].ToString().Trim();
                    _order.OrderTypeTagValue = row[ORDERTYPE].ToString().Trim();

                    _order.TradingAccountName = row[TRADINGACCOUNTNAME].ToString();
                    // Kuldeep A.: ORIGCLORDERID is a string field, so whenever it comes as a string value parsing into INT creates error here.
                    // So comparing to int.MinValue as a string.
                    if (row[ORIGCLORDERID].ToString() != int.MinValue.ToString())
                    {
                        _order.OrigClOrderID = row[ORIGCLORDERID].ToString();
                    }

                    _order.CounterPartyID = int.Parse(row[COUNTERPARTYID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.VenueID = int.Parse(row[VENUEID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.AUECID = int.Parse(row[AUECID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.ClientTime = row[CLIENTTIME].ToString();
                    _order.TransactionTime = DateTime.ParseExact(row[CLIENTTIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                    _order.AssetID = int.Parse(row[ASSETID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString(), System.Globalization.NumberStyles.Integer);
                    if (row[FLAG].ToString() != string.Empty)
                    {
                        _order.Flag = (byte[])(row[FLAG]);
                    }
                    _order.StagedOrderID = row[STAGEDORDERID].ToString();
                    _order.TradingAccountID = Int32.Parse(row[TRADINGACCOUNTID].ToString());
                    _order.CompanyUserID = Int32.Parse(row[USERID].ToString());
                    _order.PranaMsgType = Int32.Parse(row[Prana_MSG_TYPE].ToString());
                    if (row[DISCR_OFFSET] != DBNull.Value)
                    {
                        _order.DiscretionOffset = Convert.ToDouble(row[DISCR_OFFSET]);
                    }
                    if (row[PEG_DIFF] != DBNull.Value)
                    {
                        _order.PegDifference = Convert.ToDouble(row[PEG_DIFF]);
                    }
                    if (row[STOP_PRICE] != DBNull.Value)
                    {
                        _order.StopPrice = Convert.ToDouble(row[STOP_PRICE]);
                    }

                    _order.MaturityMonthYear = row[MATURITY_YEARMONTH].ToString();
                    if (row[STRIKE_PRICE] != DBNull.Value)
                    {
                        _order.StrikePrice = Convert.ToDouble(row[STRIKE_PRICE]);
                    }
                    _order.SecurityType = row[SECURITY_TYPE].ToString();
                    _order.PutOrCall = row[PUT_CALL].ToString();
                    _order.OpenClose = row[OPEN_CLOSE].ToString();
                    if (row[ORDER_SEQ_NUMBER] != DBNull.Value)
                    {
                        _order.OrderSeqNumber = int.Parse(row[ORDER_SEQ_NUMBER].ToString());
                    }
                    _order.Level1ID = int.Parse(row[FUNDID].ToString());
                    _order.Level2ID = int.Parse(row[STRATEGYID].ToString());
                    _order.AUECLocalDate = DateTime.Parse(row[AUECLocalDate].ToString());
                    _order.SettlementDate = DateTime.Parse(row[SETTLEMENTDATE].ToString());
                    _order.ProcessDate = DateTime.Parse(row[PROCESSDATE].ToString());
                    _order.OriginalPurchaseDate = DateTime.Parse(row[PROCESSDATE].ToString());
                    _order.ChangeType = int.Parse(row[CHANGETYPE].ToString());
                    _order.SettlementCurrencyID = int.Parse(row[SettlementCurrencyID].ToString());

                    //Updated currency ID for incoming order when details are updated from database, PRANA-11607
                    if (row[currencyID] != DBNull.Value)
                    {
                        _order.CurrencyID = int.Parse(row[currencyID].ToString());
                    }

                    //Updated FX Rate for incoming order when details are updated from database, PRANA-11597
                    if (row[avgFxRateForTrade] != DBNull.Value)
                    {
                        _order.FXRate = Convert.ToDouble(row[avgFxRateForTrade]);
                    }

                    //KashishG., CI-2013
                    //Filling Swap Parameters
                    if (row[swapParameters] != DBNull.Value)
                    {
                        _order.SwapParameters = new SwapParameters(row[swapParameters].ToString());
                    }

                    if (row[TIME_IN_FORCE] != DBNull.Value)
                    {
                        _order.TIF = row[TIME_IN_FORCE].ToString();
                    }
                    if (row[EXCHANGE_ID] != DBNull.Value)
                    {
                        _order.ExchangeID = int.Parse(row[EXCHANGE_ID].ToString());
                    }
                    if (row[HANDLING_INST] != DBNull.Value)
                    {
                        _order.HandlingInstruction = row[HANDLING_INST].ToString();
                    }
                    if (row[EXPIRETIME] != DBNull.Value && !string.IsNullOrEmpty(row[EXPIRETIME].ToString()))
                    {
                        DateTime _expireTime = DateTime.ParseExact(row[EXPIRETIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                        _order.ExpireTime = _expireTime.ToString();
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
            return _order;
        }


        public PranaMessage GetTradingInstByOrderID(string clOrderID)
        {
            Order _order = new Order();
            object[] parameter = new object[1];
            try
            {
                parameter[0] = clOrderID;//Convert.ToInt64( _date);

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingInstByClOrderID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        _order = FillTradingInstDetails(row, 0);
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
            //return _order;
            PranaMessage PranaMsg = new PranaMessage(_order);
            return PranaMsg;
        }

        public Order FillTradingInstDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Order _order = null;
            try
            {
                if (row != null)
                {
                    _order = new Order();
                    int CLORDERID = offset + 0;
                    int QUANTITY = offset + 1;
                    int SYMBOL = offset + 2;
                    int ORDERSIDE = offset + 3;
                    int TRADINGACCOUNTID = offset + 4;
                    int USERID = offset + 5;
                    int TEXT = offset + 6;
                    int CLIENT_ORDERID = offset + 7;
                    int ORDERSTATUS = offset + 8;
                    int MSG_TYPE = offset + 9;
                    int ON_BEHALF_OF_COMPID = offset + 10;

                    _order.ClOrderID = row[CLORDERID].ToString();

                    _order.Quantity = Double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);
                    _order.Symbol = row[SYMBOL].ToString();
                    _order.OrderSideTagValue = row[ORDERSIDE].ToString().Trim();

                    _order.TradingAccountID = Int32.Parse(row[TRADINGACCOUNTID].ToString());
                    _order.CompanyUserID = Int32.Parse(row[USERID].ToString());
                    _order.Text = row[TEXT].ToString();

                    _order.ClientOrderID = row[CLIENT_ORDERID].ToString();
                    _order.MsgType = row[MSG_TYPE].ToString();
                    _order.OnBehalfOfCompID = row[ON_BEHALF_OF_COMPID].ToString();
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
            return _order;
        }

        public void DeleteOldManualFills(string clOrderID)
        {
            int result = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = clOrderID.Equals(string.Empty) ? Int64.MinValue.ToString() : clOrderID;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteManualFills", parameter).ToString());
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
        public static Dictionary<string, MultiBrokersSubsCollection> FillStagedSubsCollection()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMultiBrokerStagedSubs";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow)
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        FillSubsIntoCollection(row, 0);
                    }
                }

                List<string> IDsToDelete = new List<string>();
                foreach (KeyValuePair<String, MultiBrokersSubsCollection> entry in _stagedSubCollection)
                {
                    if (!String.IsNullOrEmpty(entry.Value.parentClOrderID) && entry.Value.clOrderID != entry.Value.parentClOrderID)
                    {
                        if (_stagedSubCollection.ContainsKey(entry.Value.parentClOrderID))
                        {
                            _stagedSubCollection[entry.Value.clOrderID].dictOrderIDWiseNewCLOrderIDs = _stagedSubCollection[entry.Value.parentClOrderID].dictOrderIDWiseNewCLOrderIDs;
                            if (!IDsToDelete.Contains(entry.Value.origClOrderID))
                            {
                                IDsToDelete.Add(entry.Value.origClOrderID);
                            }
                        }
                    }
                    CalculateMultiDayFieldsInCache(entry.Value);
                }
                foreach (string clorderID in IDsToDelete)
                {
                    if (_stagedSubCollection.ContainsKey(clorderID))
                    {
                        _stagedSubCollection.Remove(clorderID);
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
            return _stagedSubCollection;
        }

        /// <summary>
        /// Updates the DayCumQty and DayAvgPrice in the Multi-Child cache of Sub orders on the basis of AuecDate
        /// </summary>
        /// <param name="parentorder">Main Sub order whose Day Fields need to be set</param>
        public static void CalculateMultiDayFieldsInCache(MultiBrokersSubsCollection parentorder)
        {
            try
            {
                double notionalAvg = 0;
                parentorder.StartOfDayCumQty = 0;
                parentorder.StartOfDayAveragePrice = 0;
                foreach (var item in parentorder.dictOrderIDWiseNewCLOrderIDs)
                {
                    if (item.Value.AuecLocalDate.Date < parentorder.CurrentAuecDate.Date)
                    {
                        parentorder.StartOfDayCumQty += item.Value.CumQty;
                        notionalAvg += item.Value.AveragePrice * item.Value.CumQty;
                    }
                }
                if (parentorder.StartOfDayCumQty != 0)
                    parentorder.StartOfDayAveragePrice = notionalAvg / parentorder.StartOfDayCumQty;
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

        private static void FillSubsIntoCollection(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            try
            {
                if (row != null)
                {
                    int clorderID = offset + 0;
                    int orderID = offset + 1;
                    int newCLOrderID = offset + 2;
                    int newOrderID = offset + 3;
                    int parentclOrderID = offset + 4;
                    int origclOrderID = offset + 5;
                    int childParentclOrderID = offset + 6;
                    int childOrigclOrderID = offset + 7;
                    int AuecLocalDate = offset + 8;
                    int AUECID = offset + 9;
                    int CumQty = offset + 11;
                    int AveragePrice = offset + 12;

                    MultiBrokerChildOrders multiBrokerChildOrders = new MultiBrokerChildOrders();
                    multiBrokerChildOrders.clOrderID = row[newCLOrderID].ToString();
                    multiBrokerChildOrders.origClOrderID = row[childOrigclOrderID].ToString();
                    multiBrokerChildOrders.parentClOrderID = row[childParentclOrderID].ToString();
                    multiBrokerChildOrders.CumQty = Convert.ToDouble(row[CumQty].ToString());
                    multiBrokerChildOrders.AveragePrice = Convert.ToDouble(row[AveragePrice].ToString());
                    multiBrokerChildOrders.AuecLocalDate = Convert.ToDateTime(row[AuecLocalDate].ToString());

                    if (_stagedSubCollection != null)
                    {
                        if (!_stagedSubCollection.ContainsKey(row[clorderID].ToString()))
                        {
                            MultiBrokersSubsCollection multiBrokerSubsCollection = new MultiBrokersSubsCollection();
                            multiBrokerSubsCollection.clOrderID = row[clorderID].ToString();
                            multiBrokerSubsCollection.OrderID = row[orderID].ToString();
                            multiBrokerSubsCollection.AUECID = Convert.ToInt32(row[AUECID].ToString());
                            multiBrokerSubsCollection.CurrentAuecDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(multiBrokerSubsCollection.AUECID));

                            if (!row[origclOrderID].ToString().Equals(int.MinValue.ToString()))
                            {
                                multiBrokerSubsCollection.parentClOrderID = row[parentclOrderID].ToString();
                                multiBrokerSubsCollection.origClOrderID = row[origclOrderID].ToString();
                                multiBrokerSubsCollection.dictOrderIDWiseNewCLOrderIDs = new Dictionary<string, MultiBrokerChildOrders>();
                                multiBrokerSubsCollection.dictOrderIDWiseNewCLOrderIDs.Add(row[newOrderID].ToString(), multiBrokerChildOrders);
                            }
                            _stagedSubCollection.Add(row[clorderID].ToString(), multiBrokerSubsCollection);
                        }
                        else
                        {
                            if (_stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs == null)
                            {
                                _stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs = new Dictionary<string, MultiBrokerChildOrders>();
                                _stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs.Add(row[newOrderID].ToString(), multiBrokerChildOrders);
                            }
                            else if (!_stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs.ContainsKey(row[newOrderID].ToString()))
                            {
                                _stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs.Add(row[newOrderID].ToString(), multiBrokerChildOrders);
                            }
                            else
                            {
                                _stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs[row[newOrderID].ToString()].clOrderID = row[newCLOrderID].ToString();
                                _stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs[row[newOrderID].ToString()].parentClOrderID = row[childParentclOrderID].ToString();
                                _stagedSubCollection[row[clorderID].ToString()].dictOrderIDWiseNewCLOrderIDs[row[newOrderID].ToString()].origClOrderID = row[childOrigclOrderID].ToString();
                            }
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
        }

        /// <summary>
        /// Finding ClOrderIds for MultiDay trades
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetMultiDayClOrderIdCache()
        {
            HashSet<string> multiDayClOrderIdsCache = new HashSet<string>();
            try
            {
                QueryData queryData = new QueryData
                {
                    StoredProcedureName = "P_GetClOrderIdsOfMultiDayTrades"
                };
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string attrb = dr[0].ToString();
                        if (!string.IsNullOrEmpty(attrb))
                        {
                            multiDayClOrderIdsCache.Add(attrb);
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
            return multiDayClOrderIdsCache;
        }

        /// <summary>
        /// Checking if row with given clOrderId is present in T_Fills or not
        /// </summary>
        /// <param name="multiDayClOrderIdsCache"></param>
        /// <param name="clOrderID"></param>
        /// <returns></returns>
        public bool IsFillForClOrderIDPresent(string clOrderID)
        {
            try
            {
                if (clOrderID != string.Empty)
                {
                    object[] parameter = new object[1];
                    parameter[0] = clOrderID;
                    DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetFillsCountForClOrderId", parameter);
                    if (ds != null && ds.Tables != null)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (int.Parse(dr[0].ToString()) > 0)
                        {
                            return true;
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
            return false;
        }

        /// <summary>
        /// This method is to get multi day order allocations
        /// </summary>
        /// <returns>Dictionary<string, int></returns>
        public Dictionary<string, string> GetMultiDayOrderAllocations()
        {
            Dictionary<string, string> dictMultiDayOrderAllocation = new Dictionary<string, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMultiDayOrderAllocations";
                queryData.CommandTimeout = 200;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        var clorderId = row[0].ToString();
                        var groupId = row[1].ToString();
                        if (!string.IsNullOrEmpty(groupId) && !string.IsNullOrEmpty(clorderId) && !dictMultiDayOrderAllocation.ContainsKey(clorderId))
                        {
                            dictMultiDayOrderAllocation.Add(clorderId, groupId);
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
            return dictMultiDayOrderAllocation;
        }
        /// <summary>
        /// This method is to get groupId of parentClOrderId
        /// </summary>
        /// <param name="parentClOrderId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParentGroupIdAndParentClOrderId()
        {
            Dictionary<string, string> dictMultiDayGroupIdAndParentClOrderIdMapping = new Dictionary<string, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetParentGroupIdAndParentClOrderIdFromTradedOrders";
                queryData.CommandTimeout = 200;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        var GroupId = row[0].ToString();
                        var ParentClOrderID = row[1].ToString();
                        if (!string.IsNullOrEmpty(GroupId) && !string.IsNullOrEmpty(ParentClOrderID) && !dictMultiDayGroupIdAndParentClOrderIdMapping.ContainsKey(ParentClOrderID))
                        {
                            dictMultiDayGroupIdAndParentClOrderIdMapping.Add(ParentClOrderID, GroupId);
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
            return dictMultiDayGroupIdAndParentClOrderIdMapping;
        }
        /// <summary>
        /// This method is to get ClOrderId mapping for MultiDay orders.
        /// </summary>
        /// <returns>Dictionary<string, string></returns>
        public Dictionary<string, string> GetMultiDayClOrderIdMapping()
        {
            Dictionary<string, string> dictMultiDayClOrderIds = new Dictionary<string, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMultiDayClOrderIDMapping";
                queryData.CommandTimeout = 200;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        var latesClorderId = row[0].ToString();
                        var parentClOrderId = row[1].ToString();
                        if (!string.IsNullOrEmpty(parentClOrderId) && !string.IsNullOrEmpty(latesClorderId) && !dictMultiDayClOrderIds.ContainsKey(latesClorderId))
                        {
                            dictMultiDayClOrderIds.Add(latesClorderId, parentClOrderId);
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
            return dictMultiDayClOrderIds;
        }

        /// <summary>
        /// This method is for saving multi day order allocation mapping
        /// </summary>
        /// <param name="clOrderId"></param>
        /// <param name="allocationPrefId"></param>
        /// <returns>No of rows affected</returns>
        public int SaveMultiDayOrderAllocation(string clOrderId, string groupId)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = clOrderId;
                parameter[1] = groupId;
                return DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveMultiDayOrderAllocation", parameter);
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
            return int.MinValue;
        }

       /// <summary>
       /// To save ClOrderIds mapping for multiDay orders.
       /// </summary>
       /// <param name="latestClOrderId"></param>
       /// <param name="parentClOrderId"></param>
       /// <returns>no of rows affected</returns>
        public int SaveMultiDayClOrderIdMapping(string latestClOrderId, string parentClOrderId)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = latestClOrderId;
                parameter[1] = parentClOrderId;
                return DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveMultiDaylatestCLOrderId", parameter);
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
            return int.MinValue;
        }

        /// <summary>
        /// This method is to get multi day order replaced clorderids
        /// </summary>
        /// <returns>Dictionary<string, string></returns>
        public Dictionary<string, string> GetMultiDayOrderReplacedClOrderIds(out Dictionary<string, string> dictMultiDayOrderOriginalClOrderIdAfterReplace)
        {
            Dictionary<string, string> dictMultiDayOrderClOrderId = new Dictionary<string, string>();
            dictMultiDayOrderOriginalClOrderIdAfterReplace = new Dictionary<string, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMultiDayOrderReplacedCLOrderIds";
                queryData.CommandTimeout = 200;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        var originalClOrderId = row[0].ToString();
                        var clOrderid = row[1].ToString();
                        if (!string.IsNullOrEmpty(originalClOrderId) && !string.IsNullOrEmpty(clOrderid) && !dictMultiDayOrderClOrderId.ContainsKey(originalClOrderId))
                        {
                            dictMultiDayOrderClOrderId.Add(originalClOrderId, clOrderid);
                            dictMultiDayOrderOriginalClOrderIdAfterReplace.Add(clOrderid, originalClOrderId);
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
            return dictMultiDayOrderClOrderId;
        }

       /// <summary>
       /// This method is to save replaced ClOrderId for multi day orders
       /// </summary>
       /// <param name="originalClOrderId"></param>
       /// <param name="clOrderid"></param>
       /// <returns>No. of rows affected</returns>
        public int SaveMultiDayOrderReplacedClOrderId(string originalClOrderId, string clOrderid)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = originalClOrderId;
                parameter[1] = clOrderid;
                return DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveMultiDayOrderReplacedCLOrderId", parameter);
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
            return int.MinValue;
        }

        /// <summary>
        /// This method is to save multi broker trade details to Database
        /// </summary>
        /// <param name="parentCLOrderId"></param>
        /// <param name="counterPartyId"></param>
        /// <param name="clOrderId"></param>
        /// <returns></returns>
        public int SaveMultiBrokerTradeDetails(string parentCLOrderId, int counterPartyId, string clOrderId)
        {
            try
            {
                object[] parameter = new object[3];
                parameter[0] = parentCLOrderId;
                parameter[1] = counterPartyId;
                parameter[2] = clOrderId;
                return DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveMultiBrokerTradeDetails", parameter);
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
            return int.MinValue;
        }

        /// <summary>
        /// This method is to get multi broker trade mapping from Database
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetMultiBrokerTradeMapping()
        {
            Dictionary<string, Dictionary<string, string>> multiBrokerTradeMapping = new Dictionary<string, Dictionary<string, string>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMultiBrokerTradeDetails";
                queryData.CommandTimeout = 200;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        var parentClOrderId = row[0].ToString();
                        var counterPartyId = row[1].ToString();
                        var clOrderid = row[2].ToString();
                        if (!string.IsNullOrEmpty(parentClOrderId) && !string.IsNullOrEmpty(clOrderid) && !string.IsNullOrEmpty(counterPartyId))
                        {
                            if (!multiBrokerTradeMapping.ContainsKey(parentClOrderId))
                            {
                                multiBrokerTradeMapping.Add(parentClOrderId, new Dictionary<string, string>());
                            }
                            if (!multiBrokerTradeMapping[parentClOrderId].ContainsKey(counterPartyId))
                            {
                                multiBrokerTradeMapping[parentClOrderId].Add(counterPartyId, clOrderid);
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
            return multiBrokerTradeMapping;
        }

        public void  UpdateMultiBrokerTradeDetailsForCurrentDay(string parentCLOrderId)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = parentCLOrderId;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateMultiBrokerTradeDetailsForCurrentDay", parameter);
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
