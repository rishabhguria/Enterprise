using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Prana.DataManager
{
    public class ClientSubsAndFillsManager
    {
        #region Private Variables

        private static readonly ClientSubsAndFillsManager instance = null;
        private static DateTime _currentDate = DateTime.Now.Date;
        private readonly int _heavySaveTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavySaveTimeout"]);

        #endregion Private Variables

        #region Instantiation

        static ClientSubsAndFillsManager()
        {
            instance = new ClientSubsAndFillsManager();
        }

        public static ClientSubsAndFillsManager GetInstance()
        {
            return instance;
        }

        #endregion Instantiation

        #region Public Methods
        public bool SaveClientResponse(Order order)
        {
            bool result = false;
            object[] parameter = new object[29];
            try
            {
                parameter[0] = order.AvgPrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.AvgPrice);
                /// Now save the order id's as integers!!
                parameter[1] = order.ClOrderID;
                parameter[2] = order.CumQty.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.CumQty);
                parameter[3] = order.ExecID;
                parameter[4] = order.ExecTransType.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.ExecTransType.Trim());
                parameter[5] = order.ExecType.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.ExecType.Trim());
                parameter[6] = order.LastMarket;
                parameter[7] = order.LastPrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.LastPrice);
                parameter[8] = order.LastShares.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.LastShares);
                parameter[9] = order.LeavesQty.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.LeavesQty);
                parameter[10] = order.MsgSeqNum;
                parameter[11] = order.MsgType.Trim();
                parameter[12] = order.OrderID;
                parameter[13] = order.OrderStatusTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderStatusTagValue.Trim());
                parameter[14] = order.OrderTypeTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderTypeTagValue.Trim());
                parameter[15] = order.OrigClOrderID;
                parameter[16] = order.Price.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.Price);
                parameter[17] = order.Quantity.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.Quantity);
                parameter[18] = order.SenderCompID;
                parameter[19] = order.SendingTime;
                parameter[20] = order.OrderSideTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderSideTagValue.Trim());
                parameter[21] = order.Symbol;
                parameter[22] = order.TargetCompID;
                parameter[23] = order.Text;
                parameter[24] = order.TIF.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.TIF.Trim());
                parameter[25] = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                parameter[26] = order.SendingTime;
                parameter[27] = order.OrderSeqNumber;
                parameter[28] = order.SenderSubID;

                int ret = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveClientOrderResponse", parameter, string.Empty, _heavySaveTimeout);

                if (ret > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (SqlException sqlEx)
            {
                bool rethrow = Logger.HandleException(sqlEx, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        public bool SaveClientRequest(Order order)
        {
            bool result = false;
            object[] parameter = new object[49];
            try
            {
                parameter[0] = order.ClOrderID;
                parameter[1] = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                if (order.DiscretionOffset != double.Epsilon)
                {
                    parameter[2] = order.DiscretionOffset;//.Equals(string.Empty) ? float.Epsilon: Convert.ToDouble( order.DiscretionOffset);
                }
                else
                {
                    parameter[2] = 0;
                }
                parameter[3] = order.ExecutionInstruction;
                parameter[4] = order.HandlingInstruction.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.HandlingInstruction.Trim());
                parameter[5] = order.MsgType.Trim();//.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.MsgType.Trim());

                parameter[6] = order.OrderID;
                parameter[7] = order.OrderSideTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderSideTagValue.Trim());
                parameter[8] = order.OrderStatusTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderStatusTagValue.Trim());
                parameter[9] = order.OrderTypeTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderTypeTagValue.Trim());
                parameter[10] = order.OrigClOrderID;

                parameter[11] = order.PegDifference.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.PegDifference);
                parameter[12] = order.PNP.Equals(string.Empty) ? int.MinValue : Convert.ToInt32(order.PNP);
                parameter[13] = order.Price.Equals(double.Epsilon) ? 0 : Convert.ToDouble(order.Price);

                parameter[14] = order.Quantity.Equals(double.Epsilon) ? 0 : Convert.ToDouble(order.Quantity);
                parameter[15] = order.StopPrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.StopPrice);
                parameter[16] = order.Symbol;
                parameter[17] = order.TargetCompID;

                parameter[18] = order.TargetSubID;
                parameter[19] = order.TIF.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.TIF.Trim());

                parameter[20] = order.VenueID;
                parameter[21] = order.ParentClOrderID;
                //TODO: Decrease this parameter or send some other parameter instead.
                parameter[22] = order.LocateReqd; //string.Empty; //order.IsSubOrder;
                parameter[23] = order.BorrowerID;// string.Empty;// order.IsStaged;
                parameter[24] = order.ShortRebate.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.ShortRebate);// string.Empty;// order.IsManual;
                parameter[25] = order.TradingAccountID;
                parameter[26] = order.CompanyUserID;
                parameter[27] = order.CounterPartyID;
                parameter[28] = order.AUECID;
                parameter[29] = order.StagedOrderID.Equals(string.Empty) ? int.MinValue.ToString() : order.StagedOrderID;
                parameter[30] = order.PranaMsgType;
                parameter[31] = order.Text;
                parameter[32] = order.Level2ID;
                parameter[33] = order.Level1ID;
                parameter[34] = order.ListID;
                parameter[35] = order.WaveID;
                parameter[36] = order.SecurityType;
                parameter[37] = order.PutOrCall;
                parameter[38] = order.MaturityMonthYear;
                parameter[39] = order.StrikePrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.StrikePrice);
                parameter[40] = order.OpenClose;
                parameter[41] = order.ParentClientOrderID;
                parameter[42] = order.ClientOrderID;
                parameter[43] = order.CMTAID;
                parameter[44] = order.GiveUpID;
                parameter[45] = order.UnderlyingSymbol;
                parameter[46] = order.ExpirationDate;
                //parameter[47] = order.SettlementDate;
                parameter[47] = order.AlgoStrategyID;
                parameter[48] = order.AlgoProperties.ToString();

                int recordsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveClientOrderRequest", parameter, string.Empty, _heavySaveTimeout);

                if (recordsAffected > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            #region Catch
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
            #endregion
            return result;
        }

        #endregion Public Methods
    }
}
