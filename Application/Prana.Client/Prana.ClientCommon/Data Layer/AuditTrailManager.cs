using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Data;


namespace Prana.ClientCommon
{
    /// <summary>
    /// Summary description for AuditTrailManager.
    /// </summary>
    public class AuditTrailManager
    {
        private static readonly AuditTrailManager instance = new AuditTrailManager();

        private AuditTrailManager()
        {
        }

        public static AuditTrailManager GetInstance()
        {

            return instance;
        }

        OrderCollection _orderCollection = null;
        #region GetTrailByOrderID
        public OrderCollection GetTrailByOrderID(string _clOrderID)
        {
            try
            {
                _orderCollection = new OrderCollection();
                object[] parameter = new object[1];
                parameter[0] = _clOrderID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTrailByOrderID", parameter))
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
                    int SUBCLORDERID = offset + 0;
                    int SYMBOL = offset + 1;
                    int ORIGCLORDERID = offset + 2;
                    int QUANTITY = offset + 3;
                    int PRICE = offset + 4;
                    int SIDE = offset + 5;
                    int TEXT = offset + 6;
                    int CLIENTTIME = offset + 7;
                    int ORDERTYPETAGVALUE = offset + 8;
                    int COMPANY_USER_ID = offset + 9;
                    int AUECID = offset + 10;
                    int PranaMsgType = offset + 11;
                    int BorrowerBroker = offset + 12;
                    int BorrowerID = offset + 13;
                    int ShortRebate = offset + 14;

                    _order.PranaMsgType = (int)(int.Parse(row[PranaMsgType].ToString()));
                    _order.AUECID = int.Parse(row[AUECID].ToString());
                    _order.Symbol = row[SYMBOL].ToString();
                    _order.OrderSideTagValue = row[SIDE].ToString();
                    _order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(row[SIDE].ToString());
                    _order.Quantity = Convert.ToDouble(row[QUANTITY].ToString());
                    _order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.ClOrderID = row[SUBCLORDERID].ToString();
                    _order.OrigClOrderID = row[ORIGCLORDERID].ToString();
                    _order.Text = row[TEXT].ToString();
                    _order.TransactionTime = Convert.ToDateTime(row[CLIENTTIME]);
                    //if (_order.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDManual ||
                    //        _order.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDManualSub)
                    //{
                    //    _order.ClientTime = row[CLIENTTIME].ToString();
                    //    _order.TransactionTime = row[CLIENTTIME].ToString();
                    //}
                    //else
                    //{
                    //    DateTime dtClient = DateTime.ParseExact(row[CLIENTTIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                    //    _order.ClientTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dtClient, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID)).ToString();
                    //    DateTime dt = DateTime.ParseExact(row[CLIENTTIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                    //    _order.TransactionTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dt, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID)).ToString();
                    //}
                    _order.OrderTypeTagValue = row[ORDERTYPETAGVALUE].ToString();
                    _order.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(row[ORDERTYPETAGVALUE].ToString());
                    _order.CompanyUserID = int.Parse(row[COMPANY_USER_ID].ToString());
                    _order.CompanyUserName = CachedDataManager.GetInstance.GetUserText(_order.CompanyUserID);
                    _order.BorrowerBroker = row[BorrowerBroker].ToString();
                    _order.BorrowerID = row[BorrowerID].ToString();
                    _order.ShortRebate = Convert.ToDouble(row[ShortRebate]);
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

        #region GetFillsByOrderID
        public OrderCollection GetFillsByOrderID(string _clOrderID, Order order)
        {
            _orderCollection = new OrderCollection();
            object[] parameter = new object[1];
            try
            {
                if (_clOrderID != string.Empty)
                {
                    parameter[0] = _clOrderID;

                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFillsByOrderID", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            _orderCollection.Add(FillOrderFills(row, 0, order));

                        }
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
        public Order FillOrderFills(object[] row, int offset, Order order)
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
                    int LAST_SHARES = offset + 0;
                    int TEXT = offset + 1;
                    int PRICE = offset + 2;
                    int TRANSACT_TIME = offset + 3;
                    int CLORDERID = offset + 4;
                    int EXECUTIONID = offset + 5;
                    int ORDER_STATUS = offset + 6;
                    int CUM_QTY = offset + 7;
                    int AVG_PRICE = offset + 8;
                    DateTime dt = DateTime.ParseExact(row[TRANSACT_TIME].ToString(), "yyyyMMdd-HH:mm:ss", null);

                    _order.TransactionTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dt,
                        CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));

                    _order.LastShares = double.Parse(row[LAST_SHARES].ToString(), System.Globalization.NumberStyles.Float);
                    _order.LastPrice = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.Text = row[TEXT].ToString();
                    _order.ClOrderID = row[CLORDERID].ToString();
                    _order.ExecID = row[EXECUTIONID].ToString();
                    if (row[ORDER_STATUS] != DBNull.Value)
                    {
                        _order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(row[ORDER_STATUS].ToString());
                    }
                    _order.CumQty = double.Parse(row[CUM_QTY].ToString());
                    _order.AvgPrice = float.Parse(row[AVG_PRICE].ToString());
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

        #region GetSingleOrderDetails
        public Order GetSingleOrderDetails(string _clOrderID)
        {
            Order _order = new Order();
            object[] parameter = new object[1];
            parameter[0] = _clOrderID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSubOrderDetailByOrderID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        _order.ClOrderID = row[0].ToString();
                        _order.Symbol = row[1].ToString();
                        _order.OrderSideTagValue = row[2].ToString();
                        _order.Price = double.Parse(row[3].ToString(), System.Globalization.NumberStyles.Float);
                        _order.Quantity = Convert.ToDouble(row[4]);

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
            return _order;
        }
        #endregion

        #region GetSubOrderDetails
        public Order GetSubOrderDetails(string _clOrderID)
        {
            Order _order = new Order();
            object[] parameter = new object[1];
            parameter[0] = _clOrderID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSubOrderDetailByOrderID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int offset = 0;
                        int TIME = offset + 0;
                        int SYMBOL = offset + 1;
                        int SIDE = offset + 2;
                        int QUANTITY = offset + 3;
                        int ORDER_TYPE = offset + 4;
                        int TIF = offset + 6;
                        int EXEC_INST = offset + 7;
                        int HANDLING_INST = offset + 8;
                        int CLORDERID = offset + 9;
                        int TRADING_AC = offset + 10;
                        int TRADING_ACOUNT_NAME = offset + 11;
                        int PRICE = offset + 12;
                        int AUECID = offset + 13;


                        _order.AUECID = int.Parse(row[AUECID].ToString());

                        _order.TransactionTime = Convert.ToDateTime(row[TIME]);

                        _order.Symbol = row[SYMBOL].ToString();
                        _order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(row[SIDE].ToString());
                        _order.Quantity = double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);
                        _order.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(row[ORDER_TYPE].ToString());
                        _order.TIF = row[TIF].ToString();
                        _order.ExecutionInstruction = row[EXEC_INST].ToString();
                        _order.HandlingInstruction = row[HANDLING_INST].ToString();
                        _order.ClOrderID = row[CLORDERID].ToString();
                        _order.TradingAccountID = Int32.Parse(row[TRADING_AC].ToString());
                        _order.TradingAccountName = row[TRADING_ACOUNT_NAME].ToString();
                        _order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
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
            return _order;
        }
        #endregion

    }
}
