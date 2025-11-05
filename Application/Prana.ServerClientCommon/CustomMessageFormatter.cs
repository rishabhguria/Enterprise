//not in use any more instead of custom we are using Nirvana Message Formatter
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Prana.BusinessObjects;
//using Prana.Global;

//namespace Prana.ServerClientCommon
//{
//    public class CustomMessageFormatter
//    {
       
//        //static string EXCHANGETIMEZONESTRING = "(GMT-05:00) Eastern Time (US & Canada)";
//        //static private TimeZoneInfo[] s_zones = TimeZoneInfo.GetTimeZonesFromRegistry();
//        //static TimeZoneInfo exchangeTimezone = TimeZoneInfo.FindTimeZone(s_zones, EXCHANGETIMEZONESTRING);
//        //Changed Rajat 13 July 2007
//        //TODO : In case of any other exchange, we can use FindTimeZone method of TimeZoneInfo or create similar properties for
//        //TODO : other time zones.
//        static Prana.BusinessObjects.TimeZone exchangeTimezone = TimeZoneInfo.EasternTimeZone;
//        static Prana.BusinessObjects.TimeZone currenttimezone = TimeZoneInfo.CurrentTimeZone;
        
//        //Global.TimeZone currentTimeZone = BusinessLogic.CachedData.GetInstance().CurrentTimeZone;
//        #region OrderSingle Related Methods
//        public static string CreateOrderSingle(OrderSingle order)
//        {            
//            string str = string.Empty;
//            StringBuilder sb = new StringBuilder();
//            //System.TimeZone.CurrentTimeZone.
//            //Global.TimeZone currenttimezone = TimeZoneInfo.FindTimeZone(TimeZoneInformation.CurrentTimeZone.DisplayName);
//            sb.Append(FIXConstants.MSGOrderSingle);
//            sb.Append(",");//1
//            sb.Append(order.TradingAccountID);
//            sb.Append(",");//2
//            sb.Append(order.OrderSideTagValue.Trim());
//            sb.Append(",");//3

//            sb.Append(order.OrderTypeTagValue.Trim());
//            sb.Append(",");//4
//            sb.Append(order.Symbol);
//            sb.Append(",");//5
//            sb.Append(order.Quantity.ToString());
//            sb.Append(",");//6
//            sb.Append(order.Price.ToString());
//            sb.Append(",");//7
//            sb.Append(order.DisplayQuantity);
//            sb.Append(",");//8
//            sb.Append(order.HandlingInstruction);
//            sb.Append(",");//9
//            sb.Append(order.TargetSubID);
//            sb.Append(",");//9
//            sb.Append(order.TargetCompID);
//            sb.Append(",");//10
//            sb.Append(order.OrderID);
//            sb.Append(",");//11
//            sb.Append(order.ClOrderID);
//            sb.Append(",");//12
//            sb.Append(order.StopPrice.ToString());
//            sb.Append(",");//13
//            sb.Append(order.TIF);
//            sb.Append(",");//14
//            sb.Append(order.ExecutionInstruction);
//            sb.Append(",");//15
//            sb.Append(order.PNP);
//            sb.Append(",");//16
//            sb.Append(order.ClientTime);
//            sb.Append(",");//17
//            sb.Append(order.OrderStatusTagValue);
//            sb.Append(",");//18
//            sb.Append(order.OrigClOrderID);
//            sb.Append(",");//19
//            sb.Append(order.DiscretionInst);
//            sb.Append(",");//20
//            sb.Append(order.DiscretionOffset.ToString());
//            sb.Append(",");//21
//            sb.Append(order.PegDifference.ToString());
//            sb.Append(",");//22

//            sb.Append(order.ParentClOrderID);
//            sb.Append(",");//23



//            sb.Append(order.CompanyUserID.ToString());
//            sb.Append(",");//24
//            sb.Append(order.AssetID.ToString());
//            sb.Append(",");//25
//            sb.Append(order.UnderlyingID.ToString());
//            sb.Append(",");//26
//            sb.Append(order.CounterPartyID.ToString());
//            sb.Append(",");//27
//            sb.Append(order.CounterPartyName);
//            sb.Append(",");//28
//            sb.Append(order.VenueID.ToString());
//            sb.Append(",");//29
//            sb.Append(order.AUECID.ToString());
//            sb.Append(",");//30
//            sb.Append(order.StagedOrderID.ToString());
//            sb.Append(",");//31
//            sb.Append(order.AvgPrice.ToString());
//            sb.Append(",");//32
//            sb.Append(order.CumQty.ToString());
//            sb.Append(",");//33
//            sb.Append(order.TransactionTime);
//            sb.Append(",");//34
//            sb.Append(order.PranaMsgType);
//            sb.Append(",");//35
//            sb.Append(order.Text);
//            sb.Append(",");//36
//            sb.Append(order.ListID);
//            sb.Append(",");//37
//            sb.Append(order.WaveID);
//            sb.Append(",");//38
//            sb.Append(order.BasketSequenceNumber);

//            //********** Added Ashish 29th Sept, 2006

//            sb.Append(",");//39
//            sb.Append(order.LocateReqd);
//            sb.Append(",");//40
//            sb.Append(order.ShortRebate);
//            sb.Append(",");//41
//            sb.Append(order.FundID);
//            sb.Append(",");//42
//            sb.Append(order.StrategyID);
//            sb.Append(",");//43
//            sb.Append(order.BorrowerID);
//            sb.Append(",");//44
//            sb.Append(order.SecurityType);
//            sb.Append(",");//45
//            sb.Append(order.PutOrCall);
//            sb.Append(",");//46
//            sb.Append(order.StrikePrice);
//            sb.Append(",");//47
//            sb.Append(order.MaturityMonthYear);
//            sb.Append(",");//48
//            //sb.Append("order.OpenClose");
//            sb.Append(",");//49
//            sb.Append(order.Venue);
//            sb.Append(",");//50
//            sb.Append(order.ClientOrderID);
//            sb.Append(",");//51
//            sb.Append(order.ParentClientOrderID);
//            sb.Append(",");//52
//            sb.Append(order.OrderSeqNumber.ToString());
//            sb.Append(",");//53
//            sb.Append(order.IsInternalOrder.ToString());
//            sb.Append(",");//54
//            sb.Append(order.CMTAID.ToString());
//            sb.Append(",");//55
//            sb.Append(order.CMTA.ToString());
//            sb.Append(",");//56
//            sb.Append(order.UnderlyingSymbol.ToString());
//            sb.Append(",");//57
//            sb.Append(order.GiveUpID.ToString());
//            sb.Append(",");//58
//            sb.Append(order.StrikePrice.ToString());
//            sb.Append(",");//59
//            sb.Append(order.ExpirationDate.ToString());
//            sb.Append(",");//60
//            sb.Append(order.SettlementDate.ToString());
//            str = sb.ToString();
//            return str;
//        }
//        public static OrderSingle FromOrderSingle(string message)
//        {
//            OrderSingle order = new OrderSingle();
//            try
//            {
//                string[] str = message.Split(',');


//                order.MsgType = str[0];
//                order.TradingAccountID = Convert.ToInt32(str[1]);
//                order.OrderSideTagValue = str[2];
//                order.OrderTypeTagValue = str[3];
//                order.Symbol = str[4];
//                order.Quantity = Convert.ToDouble(str[5]);
//                order.Price = double.Parse(str[6], System.Globalization.NumberStyles.Float);
//                order.DisplayQuantity = double.Parse(str[7], System.Globalization.NumberStyles.Float);

//                order.HandlingInstruction = str[8];
//                order.TargetSubID = str[9];
//                order.TargetCompID = str[10];
//                order.OrderID = str[11];
//                order.ClOrderID = str[12];
//                order.StopPrice = double.Parse(str[13], System.Globalization.NumberStyles.Float);// str[12];
//                order.TIF = str[14];
//                order.ExecutionInstruction = str[15];
//                order.PNP = str[16];
//                order.ClientTime = str[17];
//                order.OrderStatusTagValue = str[18];
//                order.OrigClOrderID = str[19];
//                order.DiscretionInst = str[20];
//                order.DiscretionOffset = double.Parse(str[21], System.Globalization.NumberStyles.Float);
//                order.PegDifference = double.Parse(str[22], System.Globalization.NumberStyles.Float);
//                //order.isStaged = str[22];
//                order.ParentClOrderID = str[23];
//                //order.isManual = str[24];
//                //order.isSubOrder = str[25];

//                order.CompanyUserID = Convert.ToInt32(str[24]);
//                order.AssetID = Convert.ToInt32(str[25]);
//                order.UnderlyingID = Convert.ToInt32(str[26]);
//                order.CounterPartyID = Convert.ToInt32(str[27]);
//                order.CounterPartyName = str[28];
//                order.VenueID = Convert.ToInt32(str[29]);
//                order.AUECID = Convert.ToInt32(str[30]);
//                order.StagedOrderID = str[31];
//                order.AvgPrice = double.Parse(str[32], System.Globalization.NumberStyles.Float);
//                order.CumQty = Convert.ToDouble(str[33]);

//                order.TransactionTime = str[34];
//                //DateTime dt = DateTime.ParseExact(str[34],Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat,null);
//                //order.TransactionTime = TimeZoneInfo.ConvertUtcToTimeZone(dt, currenttimezone).ToString();
                
//                order.PranaMsgType = int.Parse(str[35].ToString());
//                order.Text = str[36];
//                order.ListID = str[37];
//                order.WaveID = str[38];
//                order.BasketSequenceNumber = int.Parse(str[39]);
//                //********** Added Ashish 29th Sept

//                order.LocateReqd = bool.Parse(str[40]);
//                order.ShortRebate = double.Parse(str[41]);
//                order.FundID = int.Parse(str[42]);
//                order.StrategyID = int.Parse(str[43]);
//                order.BorrowerID = str[44];
//                order.SecurityType = str[45];
//                order.PutOrCall = int.Parse(str[46]);
//                order.StrikePrice = double.Parse(str[47]);
//                order.MaturityMonthYear = str[48];
//                //order.OpenClose = str[49];
//                order.Venue = str[50];
//                order.ClientOrderID = str[51];
//                order.ParentClientOrderID = str[52];
//                order.OrderSeqNumber = Int64.Parse(str[53]);
//                order.IsInternalOrder = bool.Parse(str[54]);           
//                order.CMTAID = int.Parse(str[55]);
//                order.CMTA = str[56];
//                order.UnderlyingSymbol  = str[57];
//                order.GiveUpID = int.Parse(str[58].ToString());

//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Prana: Message Format Error.", ex);

//            }
//            return order;
//        }

//        public static string CreateReplaceRequest(OrderSingle order)
//        {
//            string str = string.Empty;
//            StringBuilder sb = new StringBuilder();

//            sb.Append(FIXConstants.MSGOrderCancelReplaceRequest);
//            sb.Append(",");//1
//            sb.Append(order.TradingAccountID);
//            sb.Append(",");//2
//            sb.Append(order.OrderSideTagValue.Trim());
//            sb.Append(",");//2

//            sb.Append(order.OrderTypeTagValue.Trim());
//            sb.Append(",");//3
//            sb.Append(order.Symbol);
//            sb.Append(",");//4
//            sb.Append(order.Quantity.ToString());
//            sb.Append(",");//5
//            sb.Append(order.Price.ToString());
//            sb.Append(",");//6
//            sb.Append(order.Venue);
//            sb.Append(",");//7
//            sb.Append(order.HandlingInstruction);
//            sb.Append(",");//8
//            sb.Append(order.TargetSubID);
//            sb.Append(",");//9
//            sb.Append(order.TargetCompID);
//            sb.Append(",");//10
//            sb.Append(order.OrigClOrderID);
//            sb.Append(",");//11
//            sb.Append(order.ClOrderID);
//            sb.Append(",");//12
//            sb.Append(order.StopPrice.ToString());
//            sb.Append(",");//13
//            sb.Append(order.DiscretionInst);
//            sb.Append(",");//14
//            sb.Append(order.DiscretionOffset.ToString());
//            sb.Append(",");//15
//            sb.Append(order.PegDifference.ToString());
//            sb.Append(",");//16

//            sb.Append(order.ParentClOrderID);
//            sb.Append(",");//17



//            sb.Append(order.CompanyUserID.ToString());
//            sb.Append(",");//19

//            sb.Append(order.CounterPartyID.ToString());
//            sb.Append(",");//20
//            sb.Append(order.VenueID.ToString());
//            sb.Append(",");//21
//            sb.Append(order.AssetID.ToString());
//            sb.Append(",");//22
//            sb.Append(order.UnderlyingID.ToString());
//            sb.Append(",");//23
//            sb.Append(order.AUECID.ToString());
//            sb.Append(",");//24
//            sb.Append(order.StagedOrderID.ToString());
//            sb.Append(",");//25
//            sb.Append(order.TransactionTime);
//            sb.Append(",");//26
//            sb.Append(order.PranaMsgType);
//            sb.Append(",");//27
//            sb.Append(order.Text);
//            sb.Append(",");//28

//            //********** Added Ashish 29th Sept, 2006
//            sb.Append(order.SecurityType);
//            sb.Append(",");//
//            sb.Append(order.PutOrCall);
//            sb.Append(",");//
//            sb.Append(order.StrikePrice);
//            sb.Append(",");//
//            sb.Append(order.MaturityMonthYear);
//            sb.Append(",");//

//            sb.Append(order.TIF);
//            sb.Append(",");//
//            sb.Append(order.ExecutionInstruction);
//            sb.Append(",");//
//            sb.Append(order.PNP);
//            sb.Append(",");//
//            sb.Append(order.ClientTime);
//            sb.Append(",");//

//            sb.Append(order.LocateReqd);
//            sb.Append(",");//
//            sb.Append(order.ShortRebate);
//            sb.Append(",");//
//            sb.Append(order.BorrowerID);
//            sb.Append(",");
//            //sb.Append(order.OpenClose);
//            sb.Append(",");
//            sb.Append(order.OrderSeqNumber);
//            sb.Append(",");
//            sb.Append(order.OrderStatusTagValue);
//            sb.Append(",");
//            sb.Append(order.ParentClientOrderID);
//            sb.Append(",");
//            sb.Append(order.ClientOrderID);
//            //sb.Append(",");
//            //sb.Append(order.CMTAID);
//            //sb.Append(",");
//            //sb.Append(order.CMTA);
//            sb.Append(",");
//            sb.Append(order.OrderID);
//            //sb.Append(",");
//            //sb.Append(order.GiveUp.ToString());

//            str = sb.ToString();
//            return str;
//        }
//        public static OrderSingle FromReplaceRequest(string message)
//        {
//            OrderSingle order = new OrderSingle();
//            try
//            {
//                string[] str = message.Split(',');


//                order.MsgType = str[0];
//                order.TradingAccountID = Convert.ToInt32(str[1]);
//                order.OrderSideTagValue = str[2];
//                order.OrderTypeTagValue = str[3];
//                order.Symbol = str[4];
//                order.Quantity = Convert.ToDouble(str[5]);
//                order.Price = double.Parse(str[6], System.Globalization.NumberStyles.Float);
//                order.Venue = str[7];

//                order.HandlingInstruction = str[8];
//                order.TargetSubID = str[9];
//                order.TargetCompID = str[10];
//                order.OrigClOrderID = str[11];
//                order.ClOrderID = str[12];
//                order.StopPrice = double.Parse(str[13], System.Globalization.NumberStyles.Float);// str[12];

//                order.DiscretionInst = str[14];
//                order.DiscretionOffset = double.Parse(str[15], System.Globalization.NumberStyles.Float);
//                order.PegDifference = double.Parse(str[16], System.Globalization.NumberStyles.Float);
//                order.ParentClOrderID = str[17];

//                order.CompanyUserID = Convert.ToInt32(str[18]);
//                order.CounterPartyID = Convert.ToInt32(str[19]);
//                order.VenueID = Convert.ToInt32(str[20]);

//                order.AssetID = Convert.ToInt32(str[21]);
//                order.UnderlyingID = Convert.ToInt32(str[22]);
//                order.AUECID = Convert.ToInt32(str[23]);
//                order.StagedOrderID = str[24];
//                //DateTime dt = DateTime.ParseExact(str[25], Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat, null);
//                //order.TransactionTime = TimeZoneInfo.ConvertUtcToTimeZone(dt, currenttimezone).ToString();
//                order.TransactionTime = str[25];
//                order.PranaMsgType = int.Parse(str[26].ToString());
//                order.Text = str[27];
//                //********** Added Ashish 29th Sept
//                order.SecurityType = str[28];
//                order.PutOrCall = int.Parse(str[29]);
//                order.StrikePrice = double.Parse(str[30]);
//                order.MaturityMonthYear = str[31];

//                order.TIF = str[32];
//                order.ExecutionInstruction = str[33];
//                order.PNP = str[34];
//                order.ClientTime = str[35];

//                order.LocateReqd = bool.Parse(str[36]);
//                order.ShortRebate = double.Parse(str[37]);
//                order.BorrowerID = str[38];
//                //order.OpenClose = str[39];
//                order.OrderSeqNumber = Convert.ToInt64(str[40]);
//                order.OrderStatusTagValue = str[41];
//                order.ParentClientOrderID = str[42];
//                order.ClientOrderID = str[43];
//                //Modified by Sandeep
//                //order.CMTAID = int.Parse(str[44]);
//                //order.CMTA = str[45];
//                order.OrderID = str[44];
//                //order.GiveUp = str[45];
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Prana: Message Format Error.", ex);

//            }
//            return order;
//        }

//        public static string CreateFillReport(OrderSingle order)
//        {
//            string str = string.Empty;
//            StringBuilder sb = new StringBuilder();

//            sb.Append(FIXConstants.MSGExecutionReport);
//            sb.Append(","); //0
//            sb.Append(order.TradingAccountID);
//            sb.Append(",");//1
//            sb.Append(order.ClOrderID);
//            sb.Append(",");//2
//            sb.Append(order.OrigClOrderID);
//            sb.Append(",");//3
//            sb.Append(order.OrderID);
//            sb.Append(",");//4
//            sb.Append(order.ListID);
//            sb.Append(",");//5
//            sb.Append(order.WaveID);
//            sb.Append(",");//6
//            sb.Append(order.CumQty);
//            sb.Append(",");//7
//            sb.Append(order.Price);
//            sb.Append(",");//8
//            sb.Append(order.AvgPrice);
//            sb.Append(",");//9
//            sb.Append(order.OrderStatusTagValue);
//            sb.Append(",");//10
//            sb.Append(order.LeavesQty);
//            sb.Append(",");//11
//            sb.Append(order.LastShares);
//            sb.Append(",");//12 // repeat field .. to be removed
//            sb.Append(order.LastShares);
//            sb.Append(",");//13
//            sb.Append(order.LastPrice);
//            sb.Append(",");//14
//            sb.Append(order.LastMarket);
//            sb.Append(",");//15
//            sb.Append(order.BasketSequenceNumber);

//            sb.Append(",");//16
//            sb.Append(order.ExecID);
//            sb.Append(",");//17
//            sb.Append(order.PranaMsgType);
//            sb.Append(",");//18

//            sb.Append(order.StagedOrderID);
//            sb.Append(",");//19
//            sb.Append(order.TransactionTime);
//            sb.Append(",");//20
//            sb.Append(order.ParentClOrderID);

//            //The following fields are added for testing purposes. TO be removed when done
//            sb.Append(",");//21
//            sb.Append(order.OrderSideTagValue.Trim());
//            sb.Append(",");//22
//            sb.Append(order.OrderTypeTagValue.Trim());
//            sb.Append(",");//23
//            sb.Append(order.Symbol);
//            sb.Append(",");//24
//            sb.Append(order.Quantity.ToString());
//            sb.Append(",");//25
//            sb.Append(order.Price.ToString());

//            sb.Append(",");//26
//            sb.Append(order.StopPrice.ToString());
//            sb.Append(",");//27
//            sb.Append(order.TIF);
//            sb.Append(",");//28
//            sb.Append(order.ExecutionInstruction);
//            sb.Append(",");//29
//            sb.Append(order.DiscretionInst);
//            sb.Append(",");//30
//            sb.Append(order.DiscretionOffset.ToString());
//            sb.Append(",");//31
//            sb.Append(order.PegDifference.ToString());

//            sb.Append(",");//32
//            sb.Append(order.AssetID.ToString());
//            sb.Append(",");//33
//            sb.Append(order.UnderlyingID.ToString());
//            sb.Append(",");//34
//            sb.Append(order.CounterPartyID.ToString());
//            sb.Append(",");//35
//            sb.Append(order.CounterPartyName);
//            sb.Append(",");//36
//            sb.Append(order.VenueID.ToString());
//            sb.Append(",");//37
//            sb.Append(order.AUECID.ToString());
//            sb.Append(",");//38
//            sb.Append(order.CompanyUserID.ToString());
//            sb.Append(","); //39
//            sb.Append(order.PutOrCall);
//            sb.Append(","); //40
//            sb.Append(order.StrikePrice);
//            sb.Append(","); //41
//            sb.Append(order.SecurityType);
//            sb.Append(","); //42
//            sb.Append(order.MaturityMonthYear);
//            sb.Append(","); //43
//            sb.Append(order.DisplayQuantity);
//            sb.Append(","); //44
//            sb.Append(order.OrderSeqNumber);
//            sb.Append(","); //45
//            //sb.Append(order.OpenClose);
//            sb.Append(","); //46
//            sb.Append(order.ExecType);
//            sb.Append(","); //47
//            sb.Append(order.ClientOrderID);
//            sb.Append(","); //48
//            sb.Append(order.ParentClientOrderID);
//            sb.Append(","); //49
//            sb.Append(order.HandlingInstruction);
//            //50

//            str = sb.ToString();
//            return str;
//        }
//        public static OrderSingle FromFillReport(string message)
//        {
//            OrderSingle order = new OrderSingle();
//            try
//            {
//                string[] str = message.Split(',');
//                order.MsgType = str[0];
//                order.TradingAccountID = int.Parse(str[1].ToString());
//                order.ClOrderID = str[2];
//                order.OrigClOrderID = str[3];
//                order.OrderID = str[4];
//                order.ListID = str[5];
//                order.WaveID = str[6];
//                order.CumQty = Convert.ToDouble(str[7]);
//                //order.ExeQty = Int64.Parse(str[6]);
//                order.Price = double.Parse(str[8], System.Globalization.NumberStyles.Float);
//                order.AvgPrice = double.Parse(str[9], System.Globalization.NumberStyles.Float);
//                order.OrderStatusTagValue = str[10];
//                order.LeavesQty = double.Parse(str[11], System.Globalization.NumberStyles.Float);
//                order.LastShares = double.Parse(str[12], System.Globalization.NumberStyles.Float);
//                order.LastShares = Convert.ToDouble(str[13]);
//                order.LastPrice = double.Parse(str[14], System.Globalization.NumberStyles.Float);
//                order.LastMarket = str[15];
//                order.BasketSequenceNumber = int.Parse(str[16]);

//                order.ExecID = str[17];
//                order.PranaMsgType = int.Parse(str[18].ToString());

//                order.StagedOrderID = str[19].ToString();
//                //DateTime dt = DateTime.ParseExact(str[20], Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat, null);
//                //order.TransactionTime = TimeZoneInfo.ConvertUtcToTimeZone(dt, currenttimezone).ToString();
//                order.TransactionTime = str[20].ToString();
//                order.ParentClOrderID = str[21].ToString();

//                //The following fields are added for testing purposes. TO be removed when done
//                order.OrderSideTagValue = str[22];
//                order.OrderTypeTagValue = str[23];
//                order.Symbol = str[24];
//                order.Quantity = Convert.ToDouble(str[25]);
//                order.Price = double.Parse(str[26], System.Globalization.NumberStyles.Float);

//                order.StopPrice = double.Parse(str[27], System.Globalization.NumberStyles.Float);// str[12];
//                order.TIF = str[28];
//                order.ExecutionInstruction = str[29];
//                order.DiscretionInst = str[30];
//                order.DiscretionOffset = double.Parse(str[31], System.Globalization.NumberStyles.Float);
//                order.PegDifference = double.Parse(str[32], System.Globalization.NumberStyles.Float);

//                order.AssetID = Convert.ToInt32(str[33]);
//                order.UnderlyingID = Convert.ToInt32(str[34]);
//                order.CounterPartyID = Convert.ToInt32(str[35]);
//                order.CounterPartyName = str[36];
//                order.VenueID = Convert.ToInt32(str[37]);
//                order.AUECID = Convert.ToInt32(str[38]);
//                order.CompanyUserID = Convert.ToInt32(str[39]);

//                order.PutOrCall = int.Parse(str[40]);
//                order.StrikePrice = double.Parse(str[41]);
//                order.SecurityType = str[42];
//                order.MaturityMonthYear = str[43];
//                order.DisplayQuantity = double.Parse(str[44], System.Globalization.NumberStyles.Float);
//                order.OrderSeqNumber = Convert.ToInt64(str[45]);
//                //order.OpenClose = str[46];
//                order.ExecType = str[47];
//                order.ClientOrderID = str[48];
//                order.ParentClientOrderID = str[49];
//                order.HandlingInstruction = str[50];

//                //Console.WriteLine(message);
//            }
//            catch (Exception ex)
//            {

//                throw new Exception("Prana: Message Format Error.", ex);

//            }

//            return order;


//        }

//        public static string CreateCancelRequest(OrderSingle order)
//        {
//            string str = string.Empty;
//            StringBuilder sb = new StringBuilder();

//            sb.Append(FIXConstants.MSGOrderCancelRequest);
//            sb.Append(",");
//            sb.Append(order.TradingAccountID);
//            sb.Append(",");
//            sb.Append(order.OrderSideTagValue.Trim());
//            sb.Append(",");

//            sb.Append(order.Symbol);
//            sb.Append(",");
//            sb.Append(order.OrderID);
//            sb.Append(",");
//            //  order.OrigClOrderID = order.ClOrderID;
//            sb.Append(order.OrigClOrderID);
//            sb.Append(",");
//            sb.Append(order.ListID);
//            sb.Append(",");
//            sb.Append(order.WaveID);
//            sb.Append(",");
//            sb.Append(order.BasketSequenceNumber);

//            sb.Append(",");
//            sb.Append(order.TransactionTime);
//            sb.Append(",");
//            sb.Append(order.Quantity);
//            sb.Append(",");
//            sb.Append(order.PutOrCall);
//            sb.Append(",");
//            sb.Append(order.StrikePrice);
//            sb.Append(",");
//            sb.Append(order.SecurityType);
//            sb.Append(",");
//            sb.Append(order.MaturityMonthYear);
//            sb.Append(",");

//            sb.Append(order.ParentClOrderID);
//            sb.Append(",");
//            sb.Append(order.CompanyUserID);
//            sb.Append(",");
//            //sb.Append(order.OpenClose);
//            sb.Append(",");
//            sb.Append(order.OrderSeqNumber);
//            sb.Append(",");
//            sb.Append(order.OrderStatusTagValue);
//            sb.Append(",");
//            sb.Append(order.PranaMsgType);
//            sb.Append(",");
//            sb.Append(order.ClOrderID);
//            sb.Append(",");
//            sb.Append(order.OrderTypeTagValue);
//            sb.Append(",");
//            sb.Append(order.Text);
//            sb.Append(",");
//            sb.Append(order.StagedOrderID);
//            sb.Append(",");


//            str = sb.ToString();
//            return str;
//        }
//        public static OrderSingle FromCancelRequest(string message)
//        {
//            OrderSingle order = new OrderSingle();
//            try
//            {
//                string[] str = message.Split(',');
//                order.MsgType = str[0];
//                order.TradingAccountID = int.Parse(str[1]);
//                order.OrderSideTagValue = str[2];
//                order.Symbol = str[3];
//                order.OrderID = str[4];
//                order.OrigClOrderID = str[5];
//                order.ListID = str[6];
//                order.WaveID = str[7];
//                order.BasketSequenceNumber = int.Parse(str[8]);
//                //DateTime dt = DateTime.ParseExact(str[9], Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat, null);
//                //order.TransactionTime = TimeZoneInfo.ConvertUtcToTimeZone(dt, currenttimezone).ToString();
//                order.TransactionTime = str[9];

//                order.Quantity = Convert.ToDouble(str[10]);
//                order.PutOrCall = int.Parse(str[11]);

//                order.StrikePrice = double.Parse(str[12]);
//                order.SecurityType = str[13];
//                order.MaturityMonthYear = str[14];
//                order.ParentClOrderID = str[15];
//                order.CompanyUserID = int.Parse(str[16]);
//                //order.OpenClose = str[17];
//                order.OrderSeqNumber = Convert.ToInt64(str[18]);
//                order.OrderStatusTagValue = str[19];
//                order.PranaMsgType = int.Parse(str[20]);
//                order.ClOrderID = str[21];
//                order.OrderTypeTagValue = str[22];
//                order.Text = str[23];
//                order.StagedOrderID = str[24];
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Prana: Message Format Error.", ex);

//            }
//            return order;

//        }

//        public static string CreateCancelRejectResponse(OrderSingle order)
//        {
//            string str = string.Empty;
//            StringBuilder sb = new StringBuilder();

//            sb.Append(FIXConstants.MSGOrderCancelReject);
//            sb.Append(",");//0
//            sb.Append(order.TradingAccountID);
//            sb.Append(",");//1
//            sb.Append(order.OrderID);
//            sb.Append(",");//2
//            sb.Append(order.ClOrderID);
//            sb.Append(",");//3
//            sb.Append(order.OrigClOrderID);
//            sb.Append(",");//4
//            sb.Append(order.OrderSideTagValue);
//            sb.Append(",");//5
//            sb.Append(order.ListID);
//            sb.Append(",");//6

//            sb.Append(order.TransactionTime);
//            sb.Append(",");//7
//            sb.Append(order.ParentClOrderID);
//            sb.Append(",");//8
//            sb.Append(order.OrderStatusTagValue);
//            sb.Append(",");//9

//            sb.Append(order.ExecutionInstruction);
//            sb.Append(",");//10
//            sb.Append(order.HandlingInstruction);
//            sb.Append(",");//11
//            sb.Append(order.Quantity.ToString());
//            sb.Append(",");//12
//            sb.Append(order.Price.ToString());
//            sb.Append(",");//13
//            sb.Append(order.StopPrice.ToString());
//            sb.Append(",");//14
//            sb.Append(order.OrderTypeTagValue);
//            sb.Append(",");//15
//            sb.Append(order.CumQty.ToString());
//            sb.Append(",");//16
//            sb.Append(order.AvgPrice.ToString());
//            sb.Append(",");//17
//            sb.Append(order.LastPrice.ToString());
//            sb.Append(",");//18
//            sb.Append(order.LastShares.ToString());
//            sb.Append(",");//19
//            sb.Append(order.DiscretionOffset.ToString());
//            sb.Append(",");//20
//            sb.Append(order.PegDifference.ToString());
//            sb.Append(",");//21
//            sb.Append(order.DisplayQuantity.ToString());
//            sb.Append(",");//22
//            sb.Append(order.TIF);
//            sb.Append(",");//23
//            sb.Append(order.PranaMsgType.ToString());
//            sb.Append(",");//24
//            sb.Append(order.StagedOrderID);
//            sb.Append(",");//24
//            sb.Append(order.OrderSeqNumber.ToString());
//            sb.Append(",");//26
//            sb.Append(order.ClientOrderID.ToString());
//            sb.Append(",");//27
//            sb.Append(order.ParentClientOrderID.ToString()); 
            
//            str = sb.ToString();
//            return str;
//        }
//        public static OrderSingle FromCancelRejectResponse(string message)
//        {
//            OrderSingle order = new OrderSingle();
//            try
//            {
//                string[] str = message.Split(',');
//                order.MsgType = str[0];
//                order.TradingAccountID = int.Parse(str[1]);
//                order.OrderID = str[2];
//                order.ClOrderID = str[3];
//                order.OrigClOrderID = str[4];
//                order.OrderSideTagValue = str[5];
//                order.ListID = str[6];
//                //DateTime dt = DateTime.ParseExact(str[7], Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat, null);
//                //order.TransactionTime = TimeZoneInfo.ConvertUtcToTimeZone(dt, currenttimezone).ToString();
//                order.TransactionTime = str[7];
//                order.ParentClOrderID = str[8];
//                order.OrderStatusTagValue = str[9];

//                order.ExecutionInstruction = str[10];
//                order.HandlingInstruction = str[11];
//                order.Quantity = double.Parse(str[12]);
//                order.Price = double.Parse(str[13]);
//                order.StopPrice = double.Parse(str[14]);
//                order.OrderTypeTagValue = str[15];
//                order.CumQty = double.Parse(str[16]);
//                order.AvgPrice = double.Parse(str[17]);
//                order.LastPrice = double.Parse(str[18]);
//                order.LastShares = double.Parse(str[19]);
//                order.DiscretionOffset = double.Parse(str[20]);
//                order.PegDifference = double.Parse(str[21]);
//                order.DisplayQuantity = double.Parse(str[22]);
//                order.TIF = str[23];
//                order.PranaMsgType = int.Parse(str[24]);
//                order.StagedOrderID = str[25];
//                order.OrderSeqNumber = Int64.Parse(str[26]);
//                order.ClientOrderID = str[27];
//                order.ParentClientOrderID = str[28];
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Prana: Message Format Error.", ex);

//            }
//            return order;

//        }

//        public static string CreateTransferUserRequest(OrderSingle order)
//        {
//            StringBuilder sb = new StringBuilder();
//            string str = string.Empty;
//            sb.Append(FIXConstants.MSGTransferUser);
//            sb.Append(",");//0
//            sb.Append(order.TradingAccountID);
//            sb.Append(",");//1
//            sb.Append(order.ClOrderID);
//            sb.Append(",");//2
//            sb.Append(order.ParentClOrderID);
//            sb.Append(",");//3
//            sb.Append(order.TransactionTime);
//            sb.Append(",");//4
//            sb.Append(order.CompanyUserID.ToString());
//            sb.Append(",");//5
//            sb.Append(order.StagedOrderID);
//            sb.Append(",");//6
//            sb.Append(order.PranaMsgType.ToString());
//            //4

//            //sb.Append(",");//4
//            //sb.Append(order.OrderSideTagValue);
//            //sb.Append(",");//5
//            //sb.Append(order.ListID);
//            //sb.Append(",");//6
//            str = sb.ToString();
//            return str;
//        }
//        public static OrderSingle FromTransferUserRequest(string message)
//        {
//            OrderSingle order = new OrderSingle();
//            try
//            {
//                string[] str = message.Split(',');
//                order.MsgType = str[0];
//                order.TradingAccountID = int.Parse(str[1]);
//                order.ClOrderID = str[2];
//                order.ParentClOrderID = str[3];
//                //DateTime dt = DateTime.ParseExact(str[4], Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat, null);
//                //order.TransactionTime = TimeZoneInfo.ConvertUtcToTimeZone(dt, currenttimezone).ToString();
//                order.TransactionTime = str[4];
//                order.CompanyUserID = int.Parse(str[5]);
//                order.StagedOrderID = str[6];
//                order.PranaMsgType = int.Parse(str[7]);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Prana: Message Format Error.", ex);

//            }
//            return order;

//        }

//        public static string CreateTradingInstruction(OrderSingle order)
//        {
//            StringBuilder sb = new StringBuilder();
//            string str = string.Empty;
//            sb.Append(order.MsgType);
//            sb.Append(",");//0
//            sb.Append(order.TradingAccountID);
//            sb.Append(",");//1
//            sb.Append(order.ClOrderID);
//            sb.Append(",");//2
//            sb.Append(order.CompanyUserID.ToString());
//            sb.Append(",");//3
//            sb.Append(order.Symbol);
//            sb.Append(",");//4
//            sb.Append(order.OrderSideTagValue);
//            sb.Append(",");//5
//            sb.Append(order.Quantity.ToString());


//            //sb.Append(",");//9
//            //sb.Append(order.OrderTypeTagValue.ToString());

//            str = sb.ToString();
//            return str;
//        }
//        public static TradingInstruction FromTradinginstruction(string message)
//        {
//            TradingInstruction tradingInst = new TradingInstruction();
//            try
//            {
//                string[] str = message.Split(',');
//                tradingInst.MsgType = str[0];
//                tradingInst.TradingAccID = int.Parse(str[1]);
//                tradingInst.ClOrderID = str[2];
//                tradingInst.UserID = int.Parse(str[3]);
//                tradingInst.Symbol = str[4];
//                tradingInst.Side = str[5];
//                tradingInst.Quantity = Double.Parse(str[6]);
//                //tradingInst.OrderTypeTagValue = str[10];

//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Prana: Message Format Error.", ex);

//            }
//            return tradingInst;

//        }

//        public static string GetFormattedMsg(OrderSingle order)
//        {
//            //TODO: For PranaMsgType = 8(Transfer user) make a separate Formatting method and call that ... 
//            //Can be done similarly for any custom requests.
//            switch (order.MsgType)
//            {
//                case FIXConstants.MSGOrder:
//                    return CreateOrderSingle(order);

//                case FIXConstants.MSGOrderCancelRequest:
//                    return CreateCancelRequest(order);

//                case FIXConstants.MSGExecutionReport:
//                    return CreateFillReport(order);

//                case FIXConstants.MSGOrderCancelReplaceRequest:
//                    return CreateReplaceRequest(order);

//                case FIXConstants.MSGOrderCancelReject:
//                    return CreateCancelRejectResponse(order);

//                case FIXConstants.MSGTransferUser:
//                    return CreateTransferUserRequest(order);

//                default:
//                    return CreateOrderSingle(order);

//            }

//        }
//        public static OrderSingle GetOrder(string message)
//        {
//            OrderSingle order = new OrderSingle();
//            string msgType = GetMessageType(message);
//            switch (msgType)
//            {
//                case FIXConstants.MSGOrderSingle:
//                    order = FromOrderSingle(message);
//                    break;

//                case FIXConstants.MSGOrderCancelRequest:
//                    order = FromCancelRequest(message);
//                    break;

//                case FIXConstants.MSGOrderCancelReplaceRequest:
//                    order = FromReplaceRequest(message);
//                    break;
//                case FIXConstants.MSGExecutionReport:
//                    order = FromFillReport(message);
//                    break;

//                case FIXConstants.MSGTransferUser:
//                    order = FromTransferUserRequest(message);
//                    break;
//                //case FIXConstants.MSGTradingInstClient:
//                //case FIXConstants.MSGTradingInstInternal:
//                //    order = PranaMessageFormatter.FromTradinginstruction(message);
//                //    break;

//            }
//            return order;
//        }

//        /// <summary>
//        /// Returns Message Type
//        /// </summary>
//        /// <param name="message"></param>
//        /// <returns></returns>
//        public static string GetMessageType(string message)
//        {
//            return message.Substring(0, message.IndexOf(','));

//        }
//        #endregion

//    }
//}
