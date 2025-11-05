using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.BusinessBaseClass;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Prana.TradeService
{
    public class ServerDbManager
    {
        private static readonly int _miscellanousTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["MiscellanousTimeout"]);
        /// <summary>
        /// Gets all order from DB when Non executed quantity is greater than 0
        /// </summary>
        /// <param name="isAllowMutlidayStagedOrders"></param>
        /// <returns></returns>
        internal static List<PranaMessage> GetBlotterLaunchData(bool isAllowMutlidayStagedOrders)
        {
            List<PranaMessage> orderCollection = new List<PranaMessage>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetAllOrdersInTrade";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow)
                });
                queryData.DictionaryDatabaseParameter.Add("@isAllowMultidayStagedOrders", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@isAllowMultidayStagedOrders",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isAllowMutlidayStagedOrders
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderCollection.Add(Transformer.CreatePranaMessageThroughReflection(FillOrderDetails(row, 0)));
                    }
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
            return orderCollection;
        }

        /// <summary>
        /// Create order from row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static OrderSingle FillOrderDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            OrderSingle order = null;
            try
            {
                if (row != null)
                {
                    order = new OrderSingle();
                    int CLORDERID = offset + 0;
                    int PARENTCLORDERID = offset + 1;
                    int LAST_PRICE = offset + 2;
                    int AVGPRICE = offset + 3;
                    int LEAVESQTY = offset + 4;
                    int CUMQTY = offset + 5;
                    int ORDERSTATUS = offset + 6;
                    int LASTSHARES = offset + 7;
                    int QUANTITY = offset + 8;
                    int SYMBOL = offset + 9;
                    int ORDERSIDE = offset + 10;
                    int ORDERTYPE = offset + 11;
                    int PRICE = offset + 12;
                    int ORIGCLORDERID = offset + 13;
                    int EXECUTION_ID = offset + 14;
                    int CLIENTTIME = offset + 15;
                    int COUNTERPARTYID = offset + 16;
                    int VENUEID = offset + 17;
                    int AUECID = offset + 18;
                    int ASSETID = offset + 19;
                    int UNDERLYINGID = offset + 20;
                    int STAGEDORDERID = offset + 21;
                    int TRADINGACCOUNTID = offset + 22;
                    int USERID = offset + 23;
                    int Prana_MSG_TYPE = offset + 24;
                    int DISCR_OFFSET = offset + 25;
                    int PEG_DIFF = offset + 26;
                    int STOP_PRICE = offset + 27;
                    //int CLEARANCE_TIME = offset + 28;
                    int MATURITY_YEARMONTH = offset + 29;
                    int STRIKE_PRICE = offset + 30;
                    int PUT_CALL = offset + 31;
                    int SECURITY_TYPE = offset + 32;
                    //int OPEN_CLOSE = offset + 33;
                    int EXEC_INST = offset + 34;
                    int TIMEINFORCE = offset + 35;
                    int HANDLINGINST = offset + 36;
                    int MESSAGETYPE = offset + 37;
                    int CMTA = offset + 38;
                    int GIVEUPID = offset + 39;
                    int UNDERLYINGSYMBOL = offset + 40;
                    int ORDER_ID = offset + 41;
                    int ALGOSTRATEGYID = offset + 42;
                    int ALGOSTRATEGYPARAMETERS = offset + 43;
                    int OriginatorTypeID = offset + 44;
                    int clientOrderID = offset + 45;
                    int AUECLocalDate = offset + 46;
                    int SettlementDate = offset + 47;
                    int SenderSubID = offset + 48;
                    int CurrencyID = offset + 49;
                    int AvgFxRateForTrade = offset + 50;
                    int Multiplier = offset + 51;
                    int ProcessDate = offset + 52;
                    int FundID = offset + 53;
                    int StategyId = offset + 54;
                    int OrderSeqNumber = offset + 55;
                    //int Calcbasis = offset + 56;
                    int CommissionRate = offset + 57;
                    int CommissionAmt = offset + 58;
                    int ImportFileName = offset + 59;
                    int ImportFileID = offset + 60;
                    int BloombergSymbol = offset + 61;
                    //int SoftCommissionCalcBasis = offset + 62;
                    int SoftCommissionRate = offset + 63;
                    int SoftCommissionAmt = offset + 64;
                    int TradeAttribute1 = offset + 65;
                    int TradeAttribute2 = offset + 66;
                    int TradeAttribute3 = offset + 67;
                    int TradeAttribute4 = offset + 68;
                    int TradeAttribute5 = offset + 69;
                    int TradeAttribute6 = offset + 70;
                    int InternalComments = offset + 71;
                    int settlementCurrency = offset + 72;
                    int FxRate = offset + 73;
                    int FxRateCalc = offset + 74;
                    int OriginalAllocationPreferenceID = offset + 75;
                    int additionalTradeAttributes = offset + 78;

                    if (!row[CommissionAmt].ToString().Equals(string.Empty))
                    {
                        order.CommissionAmt = double.Parse(row[CommissionAmt].ToString(), System.Globalization.NumberStyles.Float);
                    }
                    else
                    {
                        order.CommissionAmt = 0.0;
                    }
                    order.CommissionRate = double.Parse(row[CommissionRate].ToString(), System.Globalization.NumberStyles.Float);

                    if (!row[SoftCommissionAmt].ToString().Equals(string.Empty))
                    {
                        order.SoftCommissionAmt = double.Parse(row[SoftCommissionAmt].ToString(), System.Globalization.NumberStyles.Float);
                    }
                    else
                    {
                        order.SoftCommissionAmt = 0.0;
                    }
                    order.SoftCommissionRate = double.Parse(row[SoftCommissionRate].ToString(), System.Globalization.NumberStyles.Float);

                    order.ClOrderID = row[CLORDERID].ToString();
                    order.ParentClOrderID = row[PARENTCLORDERID].ToString();

                    order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    order.LastPrice = double.Parse(row[LAST_PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    order.LeavesQty = double.Parse(row[LEAVESQTY].ToString(), System.Globalization.NumberStyles.Float);
                    order.CumQty = double.Parse(row[CUMQTY].ToString(), System.Globalization.NumberStyles.Float);
                    order.LastShares = double.Parse(row[LASTSHARES].ToString(), System.Globalization.NumberStyles.Float);

                    order.OrderStatusTagValue = row[ORDERSTATUS].ToString().Trim();

                    order.AvgPrice = Double.Parse(row[AVGPRICE].ToString(), System.Globalization.NumberStyles.Float);//Convert.ToDouble((row[AVGPRICE]));
                    order.Quantity = Double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);
                    order.Symbol = row[SYMBOL].ToString();
                    order.ExecID = row[EXECUTION_ID].ToString();
                    order.OrderSideTagValue = row[ORDERSIDE].ToString().Trim();
                    order.OrderTypeTagValue = row[ORDERTYPE].ToString().Trim();

                    order.OrigClOrderID = row[ORIGCLORDERID].ToString();

                    order.CounterPartyID = int.Parse(row[COUNTERPARTYID].ToString(), System.Globalization.NumberStyles.Integer);
                    order.VenueID = int.Parse(row[VENUEID].ToString(), System.Globalization.NumberStyles.Integer);
                    order.AUECID = int.Parse(row[AUECID].ToString(), System.Globalization.NumberStyles.Integer);
                    order.ClientTime = row[CLIENTTIME].ToString();
                    //DateTime dt = DateTime.ParseExact(row[CLIENTTIME].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                    DateTime transTime = DateTime.Parse(row[AUECLocalDate].ToString());
                    order.TransactionTime = transTime;
                    order.AssetID = int.Parse(row[ASSETID].ToString(), System.Globalization.NumberStyles.Integer);
                    order.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString(), System.Globalization.NumberStyles.Integer);
                    order.StagedOrderID = row[STAGEDORDERID].ToString();
                    order.TradingAccountID = Int32.Parse(row[TRADINGACCOUNTID].ToString());
                    order.CompanyUserID = Int32.Parse(row[USERID].ToString());
                    order.PranaMsgType = Int32.Parse(row[Prana_MSG_TYPE].ToString());
                    if (row[DISCR_OFFSET] != DBNull.Value)
                    {
                        order.DiscretionOffset = Convert.ToDouble(row[DISCR_OFFSET]);
                    }
                    if (row[PEG_DIFF] != DBNull.Value)
                    {
                        order.PegDifference = Convert.ToDouble(row[PEG_DIFF]);
                    }
                    if (row[STOP_PRICE] != DBNull.Value)
                    {
                        order.StopPrice = Convert.ToDouble(row[STOP_PRICE]);
                    }

                    order.MaturityMonthYear = row[MATURITY_YEARMONTH].ToString();
                    if (row[STRIKE_PRICE] != DBNull.Value)
                    {
                        order.StrikePrice = Convert.ToDouble(row[STRIKE_PRICE]);
                    }
                    order.SecurityType = row[SECURITY_TYPE].ToString();

                    order.PutOrCalls = row[PUT_CALL].ToString();
                    order.ExecutionInstruction = row[EXEC_INST].ToString().Trim();
                    order.TIF = row[TIMEINFORCE].ToString().Trim();
                    order.HandlingInstruction = row[HANDLINGINST].ToString().Trim();
                    order.MsgType = row[MESSAGETYPE].ToString();
                    order.Level1ID = int.Parse(row[FundID].ToString(), System.Globalization.NumberStyles.Integer);
                    if (row[StategyId] != DBNull.Value)
                    {
                        order.Level2ID = int.Parse(row[StategyId].ToString(), System.Globalization.NumberStyles.Integer);
                    }
                    if (row[CMTA].ToString() != string.Empty)
                    {
                        order.CMTAID = int.Parse(row[CMTA].ToString());
                    }
                    if (row[GIVEUPID].ToString() != string.Empty)
                    {
                        order.GiveUpID = int.Parse(row[GIVEUPID].ToString());
                    }
                    if (row[UNDERLYINGSYMBOL] != DBNull.Value)
                    {
                        order.UnderlyingSymbol = row[UNDERLYINGSYMBOL].ToString();
                    }

                    if (row[ORDER_ID] != DBNull.Value)
                    {
                        order.OrderID = row[ORDER_ID].ToString();
                    }
                    if (row[ALGOSTRATEGYID] != DBNull.Value && row[ALGOSTRATEGYID].ToString() != string.Empty)
                    {
                        order.AlgoStrategyID = row[ALGOSTRATEGYID].ToString();
                    }
                    if (row[ALGOSTRATEGYPARAMETERS] != DBNull.Value)
                    {
                        order.AlgoProperties = new OrderAlgoStartegyParameters(row[ALGOSTRATEGYPARAMETERS].ToString());
                    }
                    if (row[OriginatorTypeID] != DBNull.Value)
                    {
                        order.OriginatorType = int.Parse(row[OriginatorTypeID].ToString());
                    }
                    if (row[clientOrderID] != DBNull.Value)
                    {
                        order.ClientOrderID = row[clientOrderID].ToString();
                    }
                    if (row[AUECLocalDate] != DBNull.Value)
                    {
                        order.AUECLocalDate = Convert.ToDateTime(row[AUECLocalDate]);
                    }

                    if (row[OrderSeqNumber] != DBNull.Value)
                    {
                        order.OrderSeqNumber = Convert.ToInt64(row[OrderSeqNumber]);
                    }

                    if (row[SettlementDate] != DBNull.Value)
                    {
                        order.SettlementDate = Convert.ToDateTime(row[SettlementDate]);
                    }
                    if (row[SenderSubID] != DBNull.Value)
                    {
                        order.SenderSubID = row[SenderSubID].ToString();
                    }
                    if (row[CurrencyID] != DBNull.Value)
                    {
                        order.CurrencyID = int.Parse(row[CurrencyID].ToString());
                    }
                    if (row[AvgFxRateForTrade] != DBNull.Value)
                    {
                        order.FXRate = Convert.ToDouble(row[AvgFxRateForTrade]);
                    }
                    if (row[Multiplier] != DBNull.Value)
                    {
                        order.ContractMultiplier = Convert.ToDouble((row[Multiplier]));
                    }
                    if (row[ProcessDate] != DBNull.Value)
                    {
                        order.ProcessDate = Convert.ToDateTime(row[ProcessDate].ToString());
                    }
                    if (row[ImportFileName] != DBNull.Value)
                    {
                        order.ImportFileName = row[ImportFileName].ToString();
                    }
                    if (row[ImportFileID] != DBNull.Value)
                    {
                        order.ImportFileID = int.Parse(row[ImportFileID].ToString());
                    }
                    if (row[BloombergSymbol] != DBNull.Value)
                    {
                        order.BloombergSymbol = row[BloombergSymbol].ToString();
                    }
                    if (row[TradeAttribute1] != DBNull.Value)
                    {
                        order.TradeAttribute1 = row[TradeAttribute1].ToString();
                    }
                    if (row[TradeAttribute2] != DBNull.Value)
                    {
                        order.TradeAttribute2 = row[TradeAttribute2].ToString();
                    }
                    if (row[TradeAttribute3] != DBNull.Value)
                    {
                        order.TradeAttribute3 = row[TradeAttribute3].ToString();
                    }
                    if (row[TradeAttribute4] != DBNull.Value)
                    {
                        order.TradeAttribute4 = row[TradeAttribute4].ToString();
                    }
                    if (row[TradeAttribute5] != DBNull.Value)
                    {
                        order.TradeAttribute5 = row[TradeAttribute5].ToString();
                    }
                    if (row[TradeAttribute6] != DBNull.Value)
                    {
                        order.TradeAttribute6 = row[TradeAttribute6].ToString();
                    }
                    if (row[InternalComments] != DBNull.Value)
                    {
                        order.InternalComments = row[InternalComments].ToString();
                    }
                    if (row[settlementCurrency] != DBNull.Value)
                    {
                        order.SettlementCurrencyID = Int32.Parse(row[settlementCurrency].ToString());
                    }
                    if (row[FxRate] != DBNull.Value)
                    {
                        order.FXRate = Convert.ToDouble(row[FxRate]);
                    }
                    if (row[FxRateCalc] != DBNull.Value)
                    {
                        order.FXConversionMethodOperator = row[FxRateCalc].ToString();
                    }
                    if (row[OriginalAllocationPreferenceID] != DBNull.Value)
                    {
                        order.OriginalAllocationPreferenceID = Convert.ToInt32(row[OriginalAllocationPreferenceID]);
                    }
                    if (row[additionalTradeAttributes] != DBNull.Value && !string.IsNullOrEmpty(row[additionalTradeAttributes].ToString()))
                    {
                        string json = row[additionalTradeAttributes].ToString();
                        order.SetTradeAttribute(json);
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
            return order;
        }

        /// <summary>
        /// Deletes the obsolete allocation preference.
        /// </summary>
        /// <returns></returns>
        internal static void DeleteObsoleteAllocationPreference()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_DeleteObsoleteAllocatinoPreference";
                queryData.CommandTimeout = _miscellanousTimeout;
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}