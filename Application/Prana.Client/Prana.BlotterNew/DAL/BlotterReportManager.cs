using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Blotter
{
    /// <summary>
    /// Summary description for AuditTrailManager.
    /// </summary>
    public class BlotterReportManager : IDisposable
    {
        private static readonly BlotterReportManager instance = new BlotterReportManager();
        ProxyBase<IAllocationManager> _allocationProxy = null;

        private BlotterReportManager()
        {
            if (_allocationProxy == null)
            {
                _allocationProxy = new ProxyBase<IAllocationManager>(BlotterConstants.LIT_ALLOCATION_END_POINT_ADDRESS_NAME);
            }
        }

        public static BlotterReportManager GetInstance()
        {

            return instance;
        }
        /// <summary>
        /// To fetch and store the stagedOrdersIds and TIF of orders.
        /// </summary>
        /// <param name="lowerdate"></param>
        /// <param name="upperdate"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetStagedOrderIdsAndTif(DateTime lowerdate, DateTime upperdate)
        {
            Dictionary<string, string> StagedOrdersIdsAndTIF = new Dictionary<string, string>();
            object[] parameter = new object[2];
            parameter[0] = lowerdate;
            parameter[1] = upperdate;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetStagedOrderIDs", parameter))
                {
                    while (reader.Read())
                    {
                        string StagedOrdId = reader.GetString(0);
                        string TIF = reader.GetString(1);
                        StagedOrdersIdsAndTIF.Add(StagedOrdId,TIF);
                    }
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                #endregion
            }
            return StagedOrdersIdsAndTIF;
        }
        #region GetTrailByOrderID

        public OrderCollection GetTrailByOrderID(string _clOrderID, int auecID, int pranaMsgType,int userID, bool  isGtcGtd=false)
        {
            OrderCollection _orderCollection = new OrderCollection();
            object[] parameter = new object[3];
            parameter[0] = _clOrderID;
            parameter[1] = isGtcGtd;
            parameter[2] = userID;
            try
            {
                //TODO: Write SP
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderTrailwithFills", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        _orderCollection.Add(FillTrail(row, 0, auecID, pranaMsgType));

                    }
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                #endregion
            }
            return _orderCollection;
        }
        public Order FillTrail(object[] row, int offset, int auecID, int pranaMsgType)
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
                    int TIME = offset + 0;
                    int LAST_SHARES = offset + 1;
                    int LASTPRICE = offset + 2;
                    int TEXT = offset + 3;
                    int AVGPRICE = offset + 4;
                    int CUMQTY = offset + 5;
                    int ORDERSTATUS = offset + 6;
                    int CLORDERID = offset + 7;
                    int QUANTITY = offset + 8;
                    int PRICE = offset + 9;
                    int SEQNUM = offset + 10;
                    int COUNTERPARTYID = offset + 11;
                    int ExchangeID = offset + 12;
                    int userID = offset + 13;
                    int TIF = offset + 14;


                    // check added as for some fills transaction time field is not available

                    if (row[TIME].ToString() != string.Empty)
                    {
                        DateTime dt = DateTime.ParseExact(row[TIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);

                        _order.TransactionTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dt, CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                    }
                    else
                    {
                        _order.TransactionTime = Convert.ToDateTime(row[TIME]);
                    }

                    _order.LastShares = double.Parse(row[LAST_SHARES].ToString(), System.Globalization.NumberStyles.Float);//row[LAST_SHARES].ToString();
                    _order.LastPrice = double.Parse(row[LASTPRICE].ToString(), System.Globalization.NumberStyles.Float);//row[PRICE].ToString();
                    _order.Text = row[TEXT].ToString();
                    _order.AvgPrice = double.Parse(row[AVGPRICE].ToString());
                    _order.CumQty = double.Parse(row[CUMQTY].ToString());
                    _order.TIF = row[TIF].ToString();
                    string orderStatus = row[ORDERSTATUS].ToString();
                    if (_order.TIF == FIXConstants.TIF_GTD && orderStatus.Equals(FIXConstants.ORDSTATUS_RollOver))
                        orderStatus = FIXConstants.ORDSTATUS_Expired;
                    _order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderStatus);
                    _order.OrderStatusTagValue = orderStatus;
                    _order.MsgSeqNum = (long)row[SEQNUM];
                    _order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);//row[LAST_SHARES].ToString();
                    _order.Quantity = double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);//row[LAST_SHARES].ToString();
                    _order.ClOrderID = row[CLORDERID].ToString();
                    if (row[userID] != DBNull.Value)
                    {
                        _order.CurrentUser = CachedDataManager.GetInstance.GetUserText(Convert.ToInt32(row[userID].ToString()));
                    }
                    if (row[COUNTERPARTYID].ToString() != string.Empty)
                    {
                        _order.CounterPartyID = int.Parse(row[COUNTERPARTYID].ToString());
                        _order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(_order.CounterPartyID);
                    }
                    else
                    {
                        _order.CounterPartyName = string.Empty;
                    }
                    if (row[ExchangeID] != DBNull.Value)
                        _order.ExchangeName = CommonDataCache.CachedDataManager.GetInstance.GetExchangeText(int.Parse(row[ExchangeID].ToString()));
                    else
                        _order.ExchangeName = CommonDataCache.CachedDataManager.GetInstance.GetExchangeText(CommonDataCache.CachedDataManager.GetInstance.GetExchangeIdFromAUECId(auecID));
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
        #endregion

        #region GetOrdersByDate

        public OrderCollection GetOrdersByDate(DateTime lowerdate, DateTime upperdate, int companyUserID)
        {
            OrderCollection _orderCollection = new OrderCollection();
            object[] parameter = new object[3];
            parameter[0] = lowerdate;
            parameter[1] = companyUserID;
            parameter[2] = upperdate;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrdersbyDate", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        _orderCollection.Add(FillOrderDetails(row, 0));

                    }
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                #endregion
            }
            return _orderCollection;
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
                    int TIME = offset + 0;
                    int SYMBOL = offset + 1;
                    int SIDE = offset + 2;
                    int QUANTITY = offset + 3;
                    int ORDER_TYPE = offset + 4;
                    //int ORDER_STAUTUS = offset + 5;
                    int TIF = offset + 6;
                    int EXEC_INST = offset + 7;
                    int HANDLING_INST = offset + 8;
                    int CLORDERID = offset + 9;
                    int TRADING_AC = offset + 10;
                    int TRADING_ACOUNT_NAME = offset + 11;
                    int PRICE = offset + 12;

                    int COUNTERPARTYID = offset + 14;
                    int VENUEID = offset + 15;

                    int ASSETID = offset + 21;
                    int ASSETNAME = offset + 18;
                    int UNDERLYINGID = offset + 19;
                    int AUECID = offset + 13;
                    int avgFXRateForTrade = offset + 22;
                    int pranaMsgType = offset + 23;
                    int USERID = offset + 24;
                    int PROCESSDATE = offset + 25;
                    int FundID = offset + 26;
                    int MODIFIEDBY = offset + 27;
                    int CURRENTUSER = offset + 28;
                    int ORDERID = offset + 29;
                    int STAGEDORDID = offset + 30;

                    _order.AUECID = int.Parse(row[AUECID].ToString());
                    if (row[TIME].ToString() != string.Empty)
                    {
                        //DateTime dt = DateTime.ParseExact(row[TIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                        _order.TransactionTime = Convert.ToDateTime(row[TIME]);
                    }

                    _order.Symbol = row[SYMBOL].ToString();
                    _order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(row[SIDE].ToString());
                    _order.Quantity = double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);//Convert.ToDouble(row[QUANTITY]);
                    _order.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(row[ORDER_TYPE].ToString());
                    //_order.OrderStatus		= row[ORDER_STAUTUS].ToString();
                    _order.TIF = row[TIF].ToString();
                    _order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionText(row[EXEC_INST].ToString());
                    _order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionText(row[HANDLING_INST].ToString());
                    _order.ClOrderID = row[CLORDERID].ToString();
                    _order.TradingAccountID = Int32.Parse(row[TRADING_AC].ToString());
                    _order.TradingAccountName = row[TRADING_ACOUNT_NAME].ToString();
                    if (row[PRICE] != DBNull.Value)
                        _order.Price = double.Parse(row[PRICE].ToString());//row[PRICE].ToString();
                    //					_order.U			= row[SYMBOL].ToString();
                    //					_order.Price		= row[PRICE].ToString();
                    //					_order.Quantity		= row[QUANTITY].ToString();

                    _order.CounterPartyID = int.Parse(row[COUNTERPARTYID].ToString());
                    _order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(_order.CounterPartyID);
                    _order.VenueID = int.Parse(row[VENUEID].ToString());


                    _order.AssetID = int.Parse(row[ASSETID].ToString());
                    _order.AssetName = row[ASSETNAME].ToString();
                    _order.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString());
                    _order.ProcessDate = (!row[PROCESSDATE].Equals(System.DBNull.Value)) ? Convert.ToDateTime(row[PROCESSDATE].ToString()) : DateTimeConstants.MinValue;
                    //if (row[OPENCLOSE] != DBNull.Value)
                    //{
                    //    _order.OpenClose = TagDatabaseManager.GetInstance.GetOpenCloseText(row[OPENCLOSE].ToString());
                    //}
                    //else
                    //{

                    //}
                    if (row[avgFXRateForTrade] != DBNull.Value)
                    {
                        //Update the property from PranaBasicMessage
                        _order.FXRate = double.Parse(row[avgFXRateForTrade].ToString());
                    }
                    if (row[pranaMsgType] != DBNull.Value)
                    {
                        _order.PranaMsgType = int.Parse(row[pranaMsgType].ToString());
                    }
                    if (row[USERID] != DBNull.Value)
                    {
                        _order.CompanyUserID = int.Parse(row[USERID].ToString());
                        _order.CompanyUserName = CachedDataManager.GetInstance.GetUserText(_order.CompanyUserID);
                    }

                    //_order.CurrentUser = CachedDataManager.GetInstance.LoggedInUser.ShortName;

                    if (row[FundID] != DBNull.Value && Convert.ToInt32(row[FundID].ToString()) != int.MinValue)
                    {
                        string accountName = "";
                        accountName = CachedDataManager.GetCashAccountName(Convert.ToInt32(row[FundID].ToString()));
                        if (string.IsNullOrEmpty(accountName) && _allocationProxy != null)
                        {
                            string prefName = _allocationProxy.InnerChannel.GetAllocationPreferenceNameById(Convert.ToInt32(row[FundID].ToString()));
                            if (!(string.IsNullOrEmpty(prefName) || prefName.Equals("Manual")))
                                accountName = prefName;
                            else
                                accountName = "Pre-Allocated";
                        }
                        _order.Level1Name = accountName;
                    }
                    if (row[MODIFIEDBY] != DBNull.Value)
                    {
                        _order.ModifiedUser = CachedDataManager.GetInstance.GetUserText(Convert.ToInt32(row[MODIFIEDBY].ToString()));
                    }
                    if (row[CURRENTUSER] != DBNull.Value)
                    {
                        _order.CurrentUser = CachedDataManager.GetInstance.GetUserText(Convert.ToInt32(row[CURRENTUSER].ToString()));
                    }
                    if(row[ORDERID] != DBNull.Value)
                    {
                        _order.OrderID = row[ORDERID].ToString();
                    }
                    if (row[STAGEDORDID] != DBNull.Value)
                    {
                        _order.StagedOrderID = row[STAGEDORDID].ToString();
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
            return _order;

        }
        #endregion

        #region GetSummary in one Single Row
        public OrderCollection GetTradeSummaryOfDay(DateTime lowerdate, int companyUserID, DateTime upperdate)
        {
            OrderCollection _orderCollection = new OrderCollection();
            object[] parameter = new object[3];
            parameter[0] = lowerdate;
            parameter[1] = companyUserID;
            parameter[2] = upperdate;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderSummarybyDate", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        _orderCollection.Add(FillOrderSummaryDetails(row, 0));

                    }
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                #endregion
            }
            return _orderCollection;
        }

        public Order FillOrderSummaryDetails(object[] row, int offset)
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
                    int PARENT_CLORDERID = offset + 0;
                    int SIDE = offset + 1;
                    int SIDE_TAGVALUE = offset + 2;
                    int ORDER_TYPE = offset + 3;
                    int ORDER_TYPE_TAGVALUE = offset + 4;

                    int COUNTERPARTY_ID = offset + 5;
                    int COUNTERPARTY_NAME = offset + 6;
                    int VEUNE_ID = offset + 7;
                    int VENUE_NAME = offset + 8;
                    int SYMBOL = offset + 9;

                    int TRADING_AC = offset + 10;
                    int TRADING_ACOUNT_NAME = offset + 11;
                    int AVGPRICE = offset + 12;
                    int QUANTITY = offset + 13;
                    int CUMQTY = offset + 14;
                    int PRICE = offset + 15;

                    int ORDER_STAUTUS = offset + 16;
                    int TIME = offset + 17;
                    int ASSETID = offset + 18;
                    int ASSETName = offset + 19;
                    int UNDERLYINGID = offset + 20;
                    int UNDERLYINGNAME = offset + 21;
                    int AUECID = offset + 23;
                    int USERID = offset + 24;
                    int PROCESSDATE = offset + 25;
                    int FundID = offset + 26;
                    int MODIFIEDBY = offset + 27;
                    int CURRENTUSER = offset + 28;
                    int ORDER_ID = offset + 29;
                    int TIF = offset + 30;
                    int STAGEDORDID = offset + 31;

                    _order.AUECID = int.Parse(row[AUECID].ToString());

                    if (row[TIME].ToString() != string.Empty)
                    {
                        DateTime dt = DateTime.ParseExact(row[TIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                        _order.TransactionTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dt, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID));
                    }


                    _order.ParentClOrderID = row[PARENT_CLORDERID].ToString();

                    _order.OrderSide = row[SIDE].ToString();
                    _order.OrderSideTagValue = row[SIDE_TAGVALUE].ToString();

                    _order.OrderType = row[ORDER_TYPE].ToString();
                    _order.OrderTypeTagValue = row[ORDER_TYPE_TAGVALUE].ToString();

                    _order.CounterPartyID = int.Parse(row[COUNTERPARTY_ID].ToString());
                    _order.CounterPartyName = row[COUNTERPARTY_NAME].ToString();

                    _order.VenueID = int.Parse(row[VEUNE_ID].ToString());
                    _order.Venue = row[VENUE_NAME].ToString();

                    _order.Symbol = row[SYMBOL].ToString();
                    _order.TradingAccountID = int.Parse(row[TRADING_AC].ToString());
                    _order.TradingAccountName = row[TRADING_ACOUNT_NAME].ToString();
                    if (row[AVGPRICE] != System.DBNull.Value)
                    {
                        _order.AvgPrice = float.Parse(row[AVGPRICE].ToString());
                    }
                    if (row[CUMQTY] != System.DBNull.Value)
                    {
                        _order.CumQty = double.Parse(row[CUMQTY].ToString());
                    }
                    _order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);//row[LAST_SHARES].ToString();
                    _order.Quantity = double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);//row[LAST_SHARES].ToString();

                    _order.OrderStatusTagValue = row[ORDER_STAUTUS].ToString();
                    if (row[TIF].ToString() == FIXConstants.TIF_GTD && _order.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver)
                    {
                        _order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_Expired);
                    }
                    else
                        _order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(row[ORDER_STAUTUS].ToString());
                    // check added as for some fills transaction time field is not available


                    _order.AssetID = int.Parse(row[ASSETID].ToString());
                    _order.AssetName = row[ASSETName].ToString();
                    _order.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString());
                    _order.UnderlyingName = row[UNDERLYINGNAME].ToString();
                    if (row[USERID] != DBNull.Value)
                    {
                        _order.CompanyUserID = int.Parse(row[USERID].ToString());
                        _order.CompanyUserName = CachedDataManager.GetInstance.GetUserText(_order.CompanyUserID);
                    }
                    //_order.CurrentUser = CachedDataManager.GetInstance.LoggedInUser.ShortName;

                    if (row[PROCESSDATE].ToString() != string.Empty)
                    {
                        _order.ProcessDate = Convert.ToDateTime(row[PROCESSDATE].ToString());
                    }

                    if (row[FundID] != DBNull.Value && Convert.ToInt32(row[FundID].ToString()) != int.MinValue)
                    {
                        string accountName = "";
                        accountName = CachedDataManager.GetCashAccountName(Convert.ToInt32(row[FundID].ToString()));
                        if (string.IsNullOrEmpty(accountName) && _allocationProxy != null)
                        {
                            string prefName = _allocationProxy.InnerChannel.GetAllocationPreferenceNameById(Convert.ToInt32(row[FundID].ToString()));
                            if (!(string.IsNullOrEmpty(prefName) || prefName.Equals("Manual")))
                                accountName = prefName;
                            else
                                accountName = "Pre-Allocated";
                        }
                        _order.Level1Name = accountName;
                    }
                    if (row[MODIFIEDBY] != DBNull.Value)
                    {
                        _order.ModifiedUser = CachedDataManager.GetInstance.GetUserText(Convert.ToInt32(row[MODIFIEDBY].ToString()));
                    }
                    if (row[CURRENTUSER] != DBNull.Value)
                    {
                        _order.CurrentUser = CachedDataManager.GetInstance.GetUserText(Convert.ToInt32(row[CURRENTUSER].ToString()));
                    }
                    if (row[ORDER_ID] != DBNull.Value)
                    {
                        _order.OrderID = row[ORDER_ID].ToString();
                    }
                    if (row[TIF] != DBNull.Value)
                    {
                        _order.TIF = row[TIF].ToString();
                    }
                    _order.StagedOrderID = row[STAGEDORDID].ToString();
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


        #endregion GetSummary in one Single Row

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_allocationProxy != null)
                        _allocationProxy.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion
    }
}

