using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Configuration;
//using System.Threading;
namespace Prana.DataManager
{
    /// <summary>
    /// Summary description for SubsAndFillsManager.
    /// </summary>
    public class SubsAndFillsManager
    {
        private static readonly SubsAndFillsManager instance = null;
        private static DateTime _currentDate = DateTime.Now.Date;
        private readonly int _heavySaveTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavySaveTimeout"]);
        static SubsAndFillsManager()
        {
            instance = new SubsAndFillsManager();
        }


        public static SubsAndFillsManager GetInstance()
        {
            return instance;
        }

        public bool SaveResponse(Order order)
        {
            bool result = false;
            object[] parameter = new object[49];
            try
            {
                parameter[0] = order.AvgPrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.AvgPrice);
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
                parameter[24] = order.TIF.Trim();
                parameter[25] = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                parameter[26] = order.SendingTime;
                parameter[27] = order.OrderSeqNumber;
                parameter[28] = order.SenderSubID;
                parameter[29] = order.FXRate;
                parameter[30] = order.CommissionAmt;
                parameter[31] = order.SoftCommissionAmt;
                parameter[32] = order.TradeAttribute1;
                parameter[33] = order.TradeAttribute2;
                parameter[34] = order.TradeAttribute3;
                parameter[35] = order.TradeAttribute4;
                parameter[36] = order.TradeAttribute5;
                parameter[37] = order.TradeAttribute6;
                parameter[38] = order.SettlementCurrencyID;
                parameter[39] = order.FXRate;
                parameter[40] = order.FXConversionMethodOperator;
                parameter[41] = order.ChangeType;
                double fillsNotionalValue = calculateFillsNotionalValue(order.OrderSideTagValue, order.LastPrice, order.LastShares, order.NotionalValue, order.ContractMultiplier);
                parameter[42] = fillsNotionalValue;
                parameter[43] = calculateFillsNotionalValueBase(fillsNotionalValue, order.FXRate, order.FXConversionMethodOperator);
                parameter[44] = order.CounterPartyID;
                parameter[45] = order.ExchangeID != int.MinValue ? (object)order.ExchangeID : null;
                parameter[46] = order.DayCumQty;
                parameter[47] = order.DayAvgPx.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.DayAvgPx);
                parameter[48] = order.GetTradeAttributesAsJson();

                int ret = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveOrderResponse", parameter, string.Empty, _heavySaveTimeout);

                if (ret > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
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
            return result;
        }

        /// <summary>
        /// Method to calculate Notional Value for given fill.
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-11544
        /// </summary>
        /// <param name="OrderSideTagValue"></param>
        /// <param name="LastPrice"></param>
        /// <param name="lastShares"></param>
        /// <param name="notionalValue"></param>
        /// <param name="ContractMultiplier"></param>
        /// <returns></returns>
        public double calculateFillsNotionalValue(string OrderSideTagValue, double LastPrice, double lastShares, double notionalValue, double ContractMultiplier)
        {
            double fillsNotionalValue = 0.0;

            if (notionalValue == 0)
                fillsNotionalValue = notionalValue;
            else
            {
                if (string.Equals(OrderSideTagValue, FIXConstants.SIDE_Buy) || string.Equals(OrderSideTagValue, FIXConstants.SIDE_Buy_Closed) || string.Equals(OrderSideTagValue, FIXConstants.SIDE_Buy_Open) || string.Equals(OrderSideTagValue, FIXConstants.SIDE_Buy_Cover))
                    fillsNotionalValue = lastShares * LastPrice * ContractMultiplier;
                else
                    fillsNotionalValue = lastShares * LastPrice * ContractMultiplier * -1;
            }
            return fillsNotionalValue;
        }

        /// <summary>
        /// Method to calculate Notional Value Base for given fill.
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-11544
        /// </summary>
        /// <param name="fillsNotionalValue"></param>
        /// <param name="fXRate"></param>
        /// <param name="fXConversionMethodOperator"></param>
        /// <returns></returns>
        public double calculateFillsNotionalValueBase(double fillsNotionalValue, double fXRate, string fXConversionMethodOperator)
        {
            double fXRateTemp = 0.0;
            if (fXRate != 0)
            {
                fXRateTemp = fXConversionMethodOperator.Equals("M") ? fXRate : (1 / fXRate);
            }

            double fillsNotionalValueBase = fillsNotionalValue * fXRateTemp;
            return fillsNotionalValueBase;
        }

        public bool SaveRequest(Order order)
        {
            bool result = false;
            object[] parameter = new object[77];
            try
            {
                parameter[0] = order.ClOrderID;
                parameter[1] = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                if (order.DiscretionOffset != double.Epsilon)
                {
                    parameter[2] = order.DiscretionOffset;
                }
                else
                {
                    parameter[2] = 0;
                }
                parameter[3] = order.ExecutionInstruction;
                parameter[4] = order.HandlingInstruction.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.HandlingInstruction.Trim());
                parameter[5] = order.MsgType.Trim();
                parameter[6] = order.OrderID;
                parameter[7] = order.OrderSideTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderSideTagValue.Trim());
                parameter[8] = order.OrderStatusTagValue;
                parameter[9] = order.OrderTypeTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderTypeTagValue.Trim());
                parameter[10] = order.OrigClOrderID.Equals(string.Empty) ? int.MinValue.ToString() : order.OrigClOrderID;
                parameter[11] = order.PegDifference.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.PegDifference);
                parameter[12] = order.PNP.Equals(string.Empty) ? int.MinValue : Convert.ToInt32(order.PNP);
                parameter[13] = order.Price.Equals(double.Epsilon) ? 0 : Convert.ToDouble(order.Price);
                parameter[14] = order.Quantity.Equals(double.Epsilon) ? 0 : Convert.ToDouble(order.Quantity);
                parameter[15] = order.StopPrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.StopPrice);
                parameter[16] = order.Symbol;
                parameter[17] = order.TargetCompID;
                parameter[18] = order.TargetSubID;
                parameter[19] = order.TIF.Trim();
                parameter[20] = order.VenueID;
                parameter[21] = order.ParentClOrderID;
                parameter[22] = order.LocateReqd;
                parameter[23] = order.BorrowerID;
                parameter[24] = order.ShortRebate.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.ShortRebate);// string.Empty;// order.IsManual;
                parameter[25] = order.TradingAccountID;
                parameter[26] = order.CompanyUserID;
                parameter[27] = order.CounterPartyID;
                parameter[28] = order.AUECID;
                parameter[29] = order.StagedOrderID;
                parameter[30] = order.PranaMsgType;
                parameter[31] = order.Text;
                parameter[32] = order.Level2ID;
                parameter[33] = order.Level1ID;
                parameter[34] = order.ListID;
                parameter[35] = order.WaveID;
                parameter[36] = order.SecurityType;
                parameter[37] = order.OpenClose;
                parameter[38] = order.ParentClientOrderID;
                parameter[39] = order.ClientOrderID;
                parameter[40] = order.CMTAID;
                parameter[41] = order.GiveUpID;
                parameter[42] = order.UnderlyingSymbol;
                parameter[43] = order.SettlementDate;
                parameter[44] = order.AlgoStrategyID;
                parameter[45] = order.AlgoProperties.ToString();
                parameter[46] = order.OriginatorType;
                parameter[47] = order.AUECLocalDate;
                parameter[48] = order.CurrencyID;
                parameter[49] = order.ProcessDate;
                parameter[50] = order.OrderSeqNumber;
                CalculationBasis cal = order.CalcBasis;
                parameter[51] = (int)cal;
                parameter[52] = order.CommissionRate;
                parameter[53] = order.ImportFileID == 0 ? DBNull.Value : (object)order.ImportFileID;
                parameter[54] = order.MultiTradeName;
                CalculationBasis softCommissionCal = order.SoftCommissionCalcBasis;
                parameter[55] = (int)softCommissionCal;
                parameter[56] = order.SoftCommissionRate;
                parameter[57] = order.TradeAttribute1;
                parameter[58] = order.TradeAttribute2;
                parameter[59] = order.TradeAttribute3;
                parameter[60] = order.TradeAttribute4;
                parameter[61] = order.TradeAttribute5;
                parameter[62] = order.TradeAttribute6;
                parameter[63] = order.InternalComments;
                parameter[64] = order.SettlementCurrencyID;
                parameter[65] = order.FXRate;
                parameter[66] = order.FXConversionMethodOperator;
                parameter[67] = order.ChangeType;
                parameter[68] = (order.OriginalAllocationPreferenceID == 0) ? 0 : (object)order.OriginalAllocationPreferenceID;
                parameter[69] = order.ActualCompanyUserID;
                parameter[70] = order.ShortLocateParameter != null ? (object)order.ShortLocateParameter.Broker : string.Empty;
                parameter[71] = order.ShortLocateParameter != null ? (object)order.ShortLocateParameter.NirvanaLocateID : 0;
                parameter[72] = order.IsManualOrder;
                string stringConvertedDate = null;
                if (!string.IsNullOrEmpty(order.ExpireTime) && !order.ExpireTime.Equals("N/A"))
                {
                    if (order.ExpireTime.Contains("/"))
                    {
                        DateTime converteddate = Convert.ToDateTime(order.ExpireTime);
                        stringConvertedDate = converteddate.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                    }
                    else if(order.ExpireTime.Contains("-"))
                    {
                        stringConvertedDate = order.ExpireTime;
                    }
                }
                parameter[73] = stringConvertedDate;
                parameter[74] = order.IsUseCustodianBroker;
                parameter[75] = order.GetTradeAttributesAsJson();
                parameter[76] = order.TradeApplicationSource;
                int recordsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveOrderRequest", parameter, string.Empty, _heavySaveTimeout);

                if (recordsAffected > 0)
                {
                    result = true;
                    InformationReporter.GetInstance.Write($"Order with ClOrderID {order.ClOrderID} is executed from {Enum.GetName(typeof(TradeApplicationSource), order.TradeApplicationSource)} application");
                }
                else
                {
                    result = false;
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
            return result;
        }

        public bool SaveUserChangeRequest(Order order)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = order.ClOrderID;
                parameter[1] = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                parameter[2] = order.CompanyUserID;

                int recordsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveOrderUserChangeRequest", parameter);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return result;
        }

        internal bool SaveTradingInstruction(TradingInstruction tradingInst)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[11];
                parameter[0] = tradingInst.ClOrderID;
                parameter[1] = tradingInst.Symbol;
                parameter[2] = tradingInst.Quantity;
                parameter[3] = tradingInst.Instructions;
                parameter[4] = tradingInst.DeskID;
                parameter[5] = tradingInst.UserID;
                parameter[6] = tradingInst.TradingAccID;
                parameter[7] = tradingInst.Side;
                parameter[8] = (int)tradingInst.Status;
                parameter[9] = tradingInst.MsgType;
                parameter[10] = tradingInst.ClientName;

                int recordsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveTradingInstruction", parameter);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return result;
        }

        internal bool UpdateTradingInstruction(TradingInstruction tradingInst)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = tradingInst.ClOrderID;
                parameter[1] = tradingInst.UserID;
                parameter[2] = (int)tradingInst.Status;

                int recordsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateTradingInstruction", parameter);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return result;
        }

        internal bool SaveAlgoSyntheticReplaceOrder(Order order)
        {
            bool result = false;
            object[] parameter = new object[49];
            try
            {
                parameter[0] = order.ClOrderID.Equals(string.Empty) ? int.MinValue : Convert.ToInt64(order.ClOrderID);
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
                parameter[8] = order.OrderStatusTagValue;
                //.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderStatusTagValue.Trim());
                parameter[9] = order.OrderTypeTagValue.Equals(string.Empty) ? Char.MinValue : Convert.ToChar(order.OrderTypeTagValue.Trim());
                parameter[10] = order.OrigClOrderID.Equals(string.Empty) ? int.MinValue.ToString() : order.OrigClOrderID;
                parameter[11] = order.PegDifference.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.PegDifference);
                parameter[12] = order.PNP.Equals(string.Empty) ? int.MinValue : Convert.ToInt32(order.PNP);
                parameter[13] = order.Price.Equals(double.Epsilon) ? 0 : Convert.ToDouble(order.Price);
                parameter[14] = order.Quantity.Equals(double.Epsilon) ? 0 : Convert.ToDouble(order.Quantity);
                parameter[15] = order.StopPrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.StopPrice);
                parameter[16] = order.Symbol;
                parameter[17] = order.TargetCompID;
                parameter[18] = order.TargetSubID;
                parameter[19] = order.TIF.Trim();
                parameter[20] = order.VenueID;
                parameter[21] = order.ParentClOrderID.Equals(string.Empty) ? int.MinValue : Convert.ToInt64(order.ParentClOrderID);
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
                //parameter[34] = order.ListID;
                //parameter[35] = order.WaveID;
                parameter[34] = order.SecurityType;
                parameter[35] = order.PutOrCall;
                parameter[36] = order.MaturityMonthYear;
                parameter[37] = order.StrikePrice.Equals(double.Epsilon) ? 0.0 : Convert.ToDouble(order.StrikePrice);
                parameter[38] = order.OpenClose;
                parameter[39] = order.ParentClientOrderID;
                parameter[40] = order.ClientOrderID;
                parameter[41] = order.CMTAID;
                parameter[42] = order.GiveUpID;
                parameter[43] = order.UnderlyingSymbol;
                parameter[44] = order.ExpirationDate;
                parameter[45] = order.SettlementDate;
                parameter[46] = order.AlgoStrategyID;
                parameter[47] = order.AlgoProperties.ToString();
                if (order.AlgoSyntheticRPLParent != null)
                {
                    parameter[48] = order.AlgoSyntheticRPLParent.ToString();
                }
                else
                {
                    parameter[48] = string.Empty;
                }

                int recordsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveAlgoSyntheticReplaceOrderRequest", parameter);

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
    }
}
