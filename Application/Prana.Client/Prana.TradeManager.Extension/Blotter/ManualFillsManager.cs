using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Data;
namespace Prana.TradeManager.Extension
{
    /// <summary>
    /// Summary description for ManualFillsManager.
    /// </summary>
    public class ManualFillsManager
    {
        private static readonly ManualFillsManager instance = new ManualFillsManager();
        private ManualFillsManager()
        {
        }

        public static ManualFillsManager GetInstance()
        {
            return instance;
        }

        #region Manual Fills for Blotter New

        public OrderBindingList GetBlotterNewManualFills(OrderSingle _order)
        {
            OrderBindingList _orderCollection = new OrderBindingList();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = _order.ParentClOrderID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetManualFillsNew", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        _orderCollection.Add(FillBlotterNewOrder(row, 0));
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
            return _orderCollection;
        }
        private OrderSingle FillBlotterNewOrder(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            OrderSingle _order = null;
            try
            {
                if (row != null)
                {
                    _order = new OrderSingle();
                    int EXECUTION_ID = offset + 0;
                    int PRICE = offset + 2;
                    int LAST_SHARES = offset + 3;
                    int TRANSACT_TIME = offset + 5;
                    int CLORDERID = offset + 6;
                    int ORDERSTATUSTAGVALUE = offset + 7;
                    int AUECID = offset + 8;
                    int AvgPrice = offset + 9;
                    int NotionalValue = offset + 10;
                    int FXRateForFill = offset + 12;
                    int FXRateCalc = offset + 13;
                    int SettlCurrency = offset + 14;

                    _order.AUECID = int.Parse(row[AUECID].ToString());
                    if (row[EXECUTION_ID] != DBNull.Value && row[EXECUTION_ID].ToString() != string.Empty)
                    {
                        _order.ExecID = row[EXECUTION_ID].ToString();
                        _order.LastPrice = Math.Round(double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float), 4);
                        _order.LastShares = double.Parse(row[LAST_SHARES].ToString(), System.Globalization.NumberStyles.Float);
                        _order.TransactionTime = DateTime.ParseExact(row[TRANSACT_TIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                        _order.ClOrderID = row[CLORDERID].ToString();
                        _order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(row[ORDERSTATUSTAGVALUE].ToString());

                        if (row[AvgPrice] != DBNull.Value)
                        {
                            _order.AvgPrice = double.Parse(row[AvgPrice].ToString());
                        }
                        if (row[NotionalValue] != DBNull.Value)
                        {
                            _order.NotionalValue = double.Parse(row[NotionalValue].ToString());
                        }
                        if (row[FXRateForFill] != DBNull.Value)
                        {
                            _order.FXRate = double.Parse(row[FXRateForFill].ToString());
                        }
                        if (row[FXRateCalc] != DBNull.Value)
                        {
                            _order.FXConversionMethodOperator = row[FXRateCalc].ToString();
                        }
                        if (row[SettlCurrency] != DBNull.Value)
                        {
                            _order.SettlementCurrencyID = int.Parse(row[SettlCurrency].ToString());
                        }
                        return _order;
                    }
                    else
                    {
                        return null;
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
        #endregion
    }
}