using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;

namespace Prana.ServerClientCommon
{
    /// <summary>
    /// Serializes Order or Basket Objects in string format  .. while creating a serilize method
    /// ensure that First Field is Message Type and Second field is Trading Account
    /// </summary>
    public class PranaMessageFormatter
    {
        #region Order Related Methods
        public static string CreateOrderSingle(Order order)
        {
            
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(FIXConstants.MSGOrderSingle);
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//2
            string orderSideTagValue = order.OrderSideTagValue;            
            //if (orderSideTagValue == FIXConstants.SIDE_Buy_Closed)
            //{
            //    orderSideTagValue = FIXConstants.SIDE_Buy;
            //    order.OpenClose = FIXConstants.Close;
            //}
            sb.Append(orderSideTagValue);
            sb.Append(Seperators.SEPERATOR_2);//3
             
            sb.Append(order.OrderTypeTagValue.Trim());
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(order.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(order.Quantity.ToString());
            sb.Append(Seperators.SEPERATOR_2);//6
            sb.Append(order.Price.ToString());
            sb.Append(Seperators.SEPERATOR_2);//7
            sb.Append(order.DisplayQuantity);
            sb.Append(Seperators.SEPERATOR_2);//8
            sb.Append(order.HandlingInstruction);
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(order.TargetSubID);
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(order.TargetCompID);
            sb.Append(Seperators.SEPERATOR_2);//10
            sb.Append(order.OrderID);
            sb.Append(Seperators.SEPERATOR_2);//11
            sb.Append(order.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//12
            sb.Append(order.StopPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);//13
            sb.Append(order.TIF);
            sb.Append(Seperators.SEPERATOR_2);//14
            sb.Append(order.ExecutionInstruction);
            sb.Append(Seperators.SEPERATOR_2);//15
            sb.Append(order.PNP);
            sb.Append(Seperators.SEPERATOR_2);//16
            sb.Append(order.ClientTime);
            sb.Append(Seperators.SEPERATOR_2);//17
            sb.Append(order.OrderStatusTagValue);
            sb.Append(Seperators.SEPERATOR_2);//18
            sb.Append(order.OrigClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//19
            sb.Append(order.DiscretionInst);
            sb.Append(Seperators.SEPERATOR_2);//20
            sb.Append(order.DiscretionOffset.ToString());
            sb.Append(Seperators.SEPERATOR_2);//21
            sb.Append(order.PegDifference.ToString());
            sb.Append(Seperators.SEPERATOR_2);//22
            
            sb.Append(order.ParentClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//23
           
           
         
            sb.Append(order.CompanyUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//24
            sb.Append(order.AssetID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//25
            sb.Append(order.UnderlyingID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//26
            sb.Append(order.CounterPartyID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//27
            sb.Append(order.CounterPartyName);
            sb.Append(Seperators.SEPERATOR_2);//28
            sb.Append(order.VenueID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//29
            sb.Append(order.AUECID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//30
            sb.Append(order.StagedOrderID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//31
            sb.Append(order.AvgPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);//32
            sb.Append(order.CumQty.ToString());
            sb.Append(Seperators.SEPERATOR_2);//33
            sb.Append(order.TransactionTime);
            sb.Append(Seperators.SEPERATOR_2);//34
            sb.Append(order.PranaMsgType);
            sb.Append(Seperators.SEPERATOR_2);//35
            sb.Append(order.Text);
            sb.Append(Seperators.SEPERATOR_2);//36
            sb.Append(order.ListID);
            sb.Append(Seperators.SEPERATOR_2);//37
            sb.Append(order.WaveID);
            sb.Append(Seperators.SEPERATOR_2);//38
            sb.Append(order.BasketSequenceNumber);
           
            //********** Added Ashish 29th Sept, 2006
            
            sb.Append(Seperators.SEPERATOR_2);//39
            sb.Append(order.LocateReqd);
            sb.Append(Seperators.SEPERATOR_2);//40
            sb.Append(order.ShortRebate);
            sb.Append(Seperators.SEPERATOR_2);//41
            sb.Append(order.FundID);
            sb.Append(Seperators.SEPERATOR_2);//42
            sb.Append(order.StrategyID);
            sb.Append(Seperators.SEPERATOR_2);//43
            sb.Append( order.BorrowerID);
            sb.Append(Seperators.SEPERATOR_2);//44
            sb.Append( order.SecurityType);
            sb.Append(Seperators.SEPERATOR_2);//45
            sb.Append( order.PutOrCall);
            sb.Append(Seperators.SEPERATOR_2);//46
            sb.Append( order.StrikePrice);
            sb.Append(Seperators.SEPERATOR_2);//47
            sb.Append( order.MaturityMonthYear);
            sb.Append(Seperators.SEPERATOR_2);//48
            //sb.Append(order.OpenClose);
            sb.Append(Seperators.SEPERATOR_2);//49
            sb.Append(order.Venue);
            sb.Append(Seperators.SEPERATOR_2);//50
            sb.Append(order.ClientOrderID);
            sb.Append(Seperators.SEPERATOR_2);//51
            sb.Append(order.ParentClientOrderID);
            sb.Append(Seperators.SEPERATOR_2);//52
            sb.Append(order.OrderSeqNumber.ToString());
            sb.Append(Seperators.SEPERATOR_2);//53         
            sb.Append(order.IsInternalOrder.ToString());
            sb.Append(Seperators.SEPERATOR_2);//54
            sb.Append(order.CMTAID);
            sb.Append(Seperators.SEPERATOR_2);//55
            sb.Append(order.CMTA.ToString());
            sb.Append(Seperators.SEPERATOR_2);//56
            sb.Append(order.UnderlyingSymbol.ToString());
            sb.Append(Seperators.SEPERATOR_2);//57
            sb.Append(order.GiveUpID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//58
            sb.Append(order.StrikePrice);
            sb.Append(Seperators.SEPERATOR_2);//59
            sb.Append(order.ExpirationDate.ToString());
            sb.Append(Seperators.SEPERATOR_2);//60
            sb.Append(order.SettlementDate.ToString());
            str = sb.ToString();
            return str;
        }
        public static Order FromOrderSingle(string message)
        {
            Order order = new Order();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);


                order.MsgType = str[0];
                order.TradingAccountID = Convert.ToInt32(str[1]);
                order.OrderSideTagValue = str[2];
                order.OrderTypeTagValue = str[3];
                order.Symbol = str[4];
                order.Quantity = Convert.ToDouble(str[5]);
                order.Price = double.Parse(str[6], System.Globalization.NumberStyles.Float);
                order.DisplayQuantity = double.Parse(str[7], System.Globalization.NumberStyles.Float);

                order.HandlingInstruction = str[8];
                order.TargetSubID = str[9];
                order.TargetCompID = str[10];
                order.OrderID = str[11];
                order.ClOrderID = str[12];
                order.StopPrice = double.Parse(str[13], System.Globalization.NumberStyles.Float);// str[12];
                order.TIF = str[14];
                order.ExecutionInstruction = str[15];
                order.PNP = str[16];
                order.ClientTime = str[17];
                order.OrderStatusTagValue = str[18];
                order.OrigClOrderID = str[19];
                order.DiscretionInst = str[20];
                order.DiscretionOffset = double.Parse(str[21], System.Globalization.NumberStyles.Float);
                order.PegDifference = double.Parse(str[22], System.Globalization.NumberStyles.Float);
                //order.isStaged = str[22];
                order.ParentClOrderID = str[23];
                //order.isManual = str[24];
                //order.isSubOrder = str[25];

                order.CompanyUserID = Convert.ToInt32(str[24]);
                order.AssetID = Convert.ToInt32(str[25]);
                order.UnderlyingID = Convert.ToInt32(str[26]);
                order.CounterPartyID = Convert.ToInt32(str[27]);
                order.CounterPartyName = str[28];
                order.VenueID = Convert.ToInt32(str[29]);
                order.AUECID = Convert.ToInt32(str[30]);
                order.StagedOrderID = str[31];
                order.AvgPrice = double.Parse(str[32], System.Globalization.NumberStyles.Float);
                order.CumQty = Convert.ToDouble(str[33]);
                order.TransactionTime = str[34];
                order.PranaMsgType = int.Parse(str[35].ToString());
                order.Text = str[36];
                order.ListID = str[37];
                order.WaveID = str[38];
                order.BasketSequenceNumber = int.Parse(str[39]);
                //********** Added Ashish 29th Sept

                order.LocateReqd = bool.Parse(str[40]);
                order.ShortRebate = double.Parse(str[41]);
                order.FundID = int.Parse(str[42]);
                order.StrategyID = int.Parse(str[43]);
                order.BorrowerID = str[44];
                order.SecurityType = str[45];
                order.PutOrCall = int.Parse(str[46]);
                order.StrikePrice = double.Parse(str[47]);
                order.MaturityMonthYear = str[48];
                //order.OpenClose = str[49];
                order.Venue = str[50];
                order.ClientOrderID = str[51];
                order.ParentClientOrderID = str[52];
                order.OrderSeqNumber  = Int64.Parse(str[53]);
                order.IsInternalOrder = bool.Parse(str[54]);
                //commenting it as it's not working on my system !! ram !
                order.CMTAID  = int.Parse(str[55]);
                order.CMTA = str[56];
                order.UnderlyingSymbol = str[57];
                order.GiveUpID = int.Parse(str[58].ToString());
                order.StrikePrice = double.Parse(str[59].ToString());
                order.ExpirationDate = str[60].ToString();
                order.SettlementDate = Convert.ToDateTime(str[61].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return order;
        }

        public static string CreateReplaceRequest(Order order)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(FIXConstants.MSGOrderCancelReplaceRequest);
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append(order.OrderSideTagValue.Trim());
            sb.Append(Seperators.SEPERATOR_2);//2

            sb.Append(order.OrderTypeTagValue.Trim());
            sb.Append(Seperators.SEPERATOR_2);//3
            sb.Append(order.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(order.Quantity.ToString());
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(order.Price.ToString());
            sb.Append(Seperators.SEPERATOR_2);//6
            sb.Append(order.Venue);
            sb.Append(Seperators.SEPERATOR_2);//7
            sb.Append(order.HandlingInstruction);
            sb.Append(Seperators.SEPERATOR_2);//8
            sb.Append(order.TargetSubID);
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(order.TargetCompID);
            sb.Append(Seperators.SEPERATOR_2);//10
            sb.Append(order.OrigClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//11
            sb.Append(order.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//12
            sb.Append(order.StopPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);//13
            sb.Append(order.DiscretionInst);
            sb.Append(Seperators.SEPERATOR_2);//14
            sb.Append(order.DiscretionOffset.ToString());
            sb.Append(Seperators.SEPERATOR_2);//15
            sb.Append(order.PegDifference.ToString());
            sb.Append(Seperators.SEPERATOR_2);//16

            sb.Append(order.ParentClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//17



            sb.Append(order.CompanyUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//19

            sb.Append(order.CounterPartyID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//20
            sb.Append(order.VenueID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//21
            sb.Append(order.AssetID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//22
            sb.Append(order.UnderlyingID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//23
            sb.Append(order.AUECID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//24
            sb.Append(order.StagedOrderID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//25
            sb.Append(order.TransactionTime);
            sb.Append(Seperators.SEPERATOR_2);//26
            sb.Append(order.PranaMsgType);
            sb.Append(Seperators.SEPERATOR_2);//27
            sb.Append(order.Text);
            sb.Append(Seperators.SEPERATOR_2);//28

            //********** Added Ashish 29th Sept, 2006
            sb.Append(order.SecurityType);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.PutOrCall);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.StrikePrice);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.MaturityMonthYear);
            sb.Append(Seperators.SEPERATOR_2);//

            sb.Append(order.TIF);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.ExecutionInstruction);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.PNP);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.ClientTime);
            sb.Append(Seperators.SEPERATOR_2);//

            sb.Append(order.LocateReqd);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.ShortRebate);
            sb.Append(Seperators.SEPERATOR_2);//
            sb.Append(order.BorrowerID);
            sb.Append(Seperators.SEPERATOR_2);
            //sb.Append(order.OpenClose);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderSeqNumber);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderStatusTagValue);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.ParentClientOrderID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.ClientOrderID);
            //sb.Append(Seperators.SEPERATOR_2);
            //sb.Append(order.CMTAID);
            //sb.Append(Seperators.SEPERATOR_2);
            //sb.Append(order.CMTA);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderID);
            //sb.Append(Seperators.SEPERATOR_2);
            //sb.Append(order.GiveUp.ToString());

            str = sb.ToString();
            return str;
        }
        public static Order FromReplaceRequest(string message)
        {
            Order order = new Order();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);


                order.MsgType = str[0];
                order.TradingAccountID = Convert.ToInt32(str[1]);
                order.OrderSideTagValue = str[2];
                order.OrderTypeTagValue = str[3];
                order.Symbol = str[4];
                order.Quantity = Convert.ToDouble(str[5]);
                order.Price = double.Parse(str[6], System.Globalization.NumberStyles.Float);
                order.Venue = str[7];

                order.HandlingInstruction = str[8];
                order.TargetSubID = str[9];
                order.TargetCompID = str[10];
                order.OrigClOrderID = str[11];
                order.ClOrderID = str[12];
                order.StopPrice = double.Parse(str[13], System.Globalization.NumberStyles.Float);// str[12];

                order.DiscretionInst = str[14];
                order.DiscretionOffset = double.Parse(str[15], System.Globalization.NumberStyles.Float);
                order.PegDifference = double.Parse(str[16], System.Globalization.NumberStyles.Float);
                order.ParentClOrderID = str[17];

                order.CompanyUserID = Convert.ToInt32(str[18]);
                order.CounterPartyID = Convert.ToInt32(str[19]);
                order.VenueID = Convert.ToInt32(str[20]);

                order.AssetID = Convert.ToInt32(str[21]);
                order.UnderlyingID = Convert.ToInt32(str[22]);
                order.AUECID = Convert.ToInt32(str[23]);
                order.StagedOrderID = str[24];

                order.TransactionTime = str[25];
                order.PranaMsgType = int.Parse(str[26].ToString());
                order.Text = str[27];
                //********** Added Ashish 29th Sept
                order.SecurityType = str[28];
                order.PutOrCall = int.Parse(str[29]);
                order.StrikePrice = double.Parse(str[30]);
                order.MaturityMonthYear = str[31];

                order.TIF = str[32];
                order.ExecutionInstruction = str[33];
                order.PNP = str[34];
                order.ClientTime = str[35];

                order.LocateReqd = bool.Parse(str[36]);
                order.ShortRebate = double.Parse(str[37]);
                order.BorrowerID = str[38];
                //order.OpenClose = str[39];
                order.OrderSeqNumber = Convert.ToInt64(str[40]);
                order.OrderStatusTagValue = str[41];
                order.ParentClientOrderID = str[42];
                order.ClientOrderID = str[43];
                //Modified by Sandeep
                //order.CMTAID =int.Parse(str[44]);
                //order.CMTA = str[45];
                order.OrderID = str[44];
                //order.GiveUp = str[45];
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return order;
        }

        public static string CreateFillReport(Order order)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(FIXConstants.MSGExecutionReport);
            sb.Append(Seperators.SEPERATOR_2); //0
            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(order.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append(order.OrigClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//3
            sb.Append(order.OrderID);
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(order.ListID);
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(order.WaveID);
            sb.Append(Seperators.SEPERATOR_2);//6
            sb.Append(order.CumQty);
            sb.Append(Seperators.SEPERATOR_2);//7
            sb.Append(order.Price);
            sb.Append(Seperators.SEPERATOR_2);//8
            sb.Append(order.AvgPrice);
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(order.OrderStatusTagValue);
            sb.Append(Seperators.SEPERATOR_2);//10
            sb.Append(order.LeavesQty);
            sb.Append(Seperators.SEPERATOR_2);//11
            sb.Append(order.LastShares);
            sb.Append(Seperators.SEPERATOR_2);//12 // repeat field .. to be removed
            sb.Append(order.LastShares);
            sb.Append(Seperators.SEPERATOR_2);//13
            sb.Append(order.LastPrice);
            sb.Append(Seperators.SEPERATOR_2);//14
            sb.Append(order.LastMarket);
            sb.Append(Seperators.SEPERATOR_2);//15
            sb.Append(order.BasketSequenceNumber);

            sb.Append(Seperators.SEPERATOR_2);//16
            sb.Append(order.ExecID);
            sb.Append(Seperators.SEPERATOR_2);//17
            sb.Append(order.PranaMsgType);
            sb.Append(Seperators.SEPERATOR_2);//18

            sb.Append(order.StagedOrderID);
            sb.Append(Seperators.SEPERATOR_2);//19
            sb.Append(order.TransactionTime);
            sb.Append(Seperators.SEPERATOR_2);//20
            sb.Append(order.ParentClOrderID);

            //The following fields are added for testing purposes. TO be removed when done
            sb.Append(Seperators.SEPERATOR_2);//21
            sb.Append(order.OrderSideTagValue.Trim());
            sb.Append(Seperators.SEPERATOR_2);//22
            sb.Append(order.OrderTypeTagValue.Trim());
            sb.Append(Seperators.SEPERATOR_2);//23
            sb.Append(order.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//24
            sb.Append(order.Quantity.ToString());
            sb.Append(Seperators.SEPERATOR_2);//25
            sb.Append(order.Price.ToString());

            sb.Append(Seperators.SEPERATOR_2);//26
            sb.Append(order.StopPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);//27
            sb.Append(order.TIF);
            sb.Append(Seperators.SEPERATOR_2);//28
            sb.Append(order.ExecutionInstruction);
            sb.Append(Seperators.SEPERATOR_2);//29
            sb.Append(order.DiscretionInst);
            sb.Append(Seperators.SEPERATOR_2);//30
            sb.Append(order.DiscretionOffset.ToString());
            sb.Append(Seperators.SEPERATOR_2);//31
            sb.Append(order.PegDifference.ToString());

            sb.Append(Seperators.SEPERATOR_2);//32
            sb.Append(order.AssetID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//33
            sb.Append(order.UnderlyingID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//34
            sb.Append(order.CounterPartyID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//35
            sb.Append(order.CounterPartyName);
            sb.Append(Seperators.SEPERATOR_2);//36
            sb.Append(order.VenueID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//37
            sb.Append(order.AUECID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//38
            sb.Append(order.CompanyUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2); //39
            sb.Append(order.PutOrCall);
            sb.Append(Seperators.SEPERATOR_2); //40
            sb.Append(order.StrikePrice);
            sb.Append(Seperators.SEPERATOR_2); //41
            sb.Append(order.SecurityType);
            sb.Append(Seperators.SEPERATOR_2); //42
            sb.Append(order.MaturityMonthYear);
            sb.Append(Seperators.SEPERATOR_2); //43
            sb.Append(order.DisplayQuantity);
            sb.Append(Seperators.SEPERATOR_2); //44
            sb.Append(order.OrderSeqNumber);
            sb.Append(Seperators.SEPERATOR_2); //45
            //sb.Append(order.OpenClose);
            sb.Append(Seperators.SEPERATOR_2); //46
            sb.Append(order.ExecType);
            sb.Append(Seperators.SEPERATOR_2); //47
            sb.Append(order.ClientOrderID);
            sb.Append(Seperators.SEPERATOR_2); //48
            sb.Append(order.ParentClientOrderID);
            sb.Append(Seperators.SEPERATOR_2); //49
            sb.Append(order.HandlingInstruction);
             //50

            str = sb.ToString();
            return str;
        }
        public static Order FromFillReport(string message)
        {
            Order order = new Order();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                order.MsgType = str[0];
                order.TradingAccountID = int.Parse(str[1].ToString());
                order.ClOrderID = str[2];
                order.OrigClOrderID = str[3];
                order.OrderID = str[4];
                order.ListID = str[5];
                order.WaveID = str[6];
                order.CumQty = Convert.ToDouble(str[7]);
                //order.ExeQty = Int64.Parse(str[6]);
                order.Price = double.Parse(str[8], System.Globalization.NumberStyles.Float);
                order.AvgPrice = double.Parse(str[9], System.Globalization.NumberStyles.Float);
                order.OrderStatusTagValue = str[10];
                order.LeavesQty = double.Parse(str[11], System.Globalization.NumberStyles.Float);
                order.LastShares = double.Parse(str[12], System.Globalization.NumberStyles.Float);
                order.LastShares = Convert.ToDouble(str[13]);
                order.LastPrice = double.Parse(str[14], System.Globalization.NumberStyles.Float);
                order.LastMarket = str[15];
                order.BasketSequenceNumber = int.Parse(str[16]);

                order.ExecID = str[17];
                order.PranaMsgType = int.Parse(str[18].ToString());

                order.StagedOrderID = str[19].ToString();
                order.TransactionTime = str[20].ToString();
                order.ParentClOrderID = str[21].ToString();

                //The following fields are added for testing purposes. TO be removed when done
                order.OrderSideTagValue = str[22];
                order.OrderTypeTagValue = str[23];
                order.Symbol = str[24];
                order.Quantity = Convert.ToDouble(str[25]);
                order.Price = double.Parse(str[26], System.Globalization.NumberStyles.Float);

                order.StopPrice = double.Parse(str[27], System.Globalization.NumberStyles.Float);// str[12];
                order.TIF = str[28];
                order.ExecutionInstruction = str[29];
                order.DiscretionInst = str[30];
                order.DiscretionOffset = double.Parse(str[31], System.Globalization.NumberStyles.Float);
                order.PegDifference = double.Parse(str[32], System.Globalization.NumberStyles.Float);

                order.AssetID = Convert.ToInt32(str[33]);
                order.UnderlyingID = Convert.ToInt32(str[34]);
                order.CounterPartyID = Convert.ToInt32(str[35]);
                order.CounterPartyName = str[36];
                order.VenueID = Convert.ToInt32(str[37]);
                order.AUECID = Convert.ToInt32(str[38]);
                order.CompanyUserID = Convert.ToInt32(str[39]);

                order.PutOrCall = int.Parse(str[40]);
                order.StrikePrice = double.Parse(str[41]);
                order.SecurityType = str[42];
                order.MaturityMonthYear = str[43];
                order.DisplayQuantity = double.Parse(str[44], System.Globalization.NumberStyles.Float);
                order.OrderSeqNumber = Convert.ToInt64(str[45]);
                //order.OpenClose = str[46];
                order.ExecType = str[47];
                order.ClientOrderID = str[48];
                order.ParentClientOrderID = str[49];
                order.HandlingInstruction = str[50];

                //Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Prana: Message Format Error.", ex);

            }

            return order;


        }

        public static string CreateCancelRequest(Order order)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(FIXConstants.MSGOrderCancelRequest);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderSideTagValue.Trim());
            sb.Append(Seperators.SEPERATOR_2);

            sb.Append(order.Symbol);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderID);
            sb.Append(Seperators.SEPERATOR_2);
            //  order.OrigClOrderID = order.ClOrderID;
            sb.Append(order.OrigClOrderID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.ListID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.WaveID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.BasketSequenceNumber);

            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.TransactionTime);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.Quantity);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.PutOrCall);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.StrikePrice);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.SecurityType);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.MaturityMonthYear);
            sb.Append(Seperators.SEPERATOR_2);
          
            sb.Append(order.ParentClOrderID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.CompanyUserID);
            sb.Append(Seperators.SEPERATOR_2);
            //sb.Append(order.OpenClose);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderSeqNumber);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderStatusTagValue);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.PranaMsgType);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.OrderTypeTagValue);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.Text);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(order.StagedOrderID);
            sb.Append(Seperators.SEPERATOR_2);


            str = sb.ToString();
            return str;
        }
        public static Order FromCancelRequest(string message)
        {
            Order order = new Order();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                order.MsgType = str[0];
                order.TradingAccountID  = int.Parse(str[1]);
                order.OrderSideTagValue = str[2];
                order.Symbol = str[3];
                order.OrderID = str[4];
                order.OrigClOrderID = str[5];
                order.ListID = str[6];
                order.WaveID = str[7];
                order.BasketSequenceNumber = int.Parse(str[8]);
                order.TransactionTime = str[9];

                order.Quantity = Convert.ToDouble(str[10]);
                order.PutOrCall = int.Parse(str[11]);

                order.StrikePrice = double.Parse(str[12]);
                order.SecurityType = str[13];
                order.MaturityMonthYear = str[14];
                order.ParentClOrderID = str[15];
                order.CompanyUserID = int.Parse(str[16]);
                //order.OpenClose = str[17];
                order.OrderSeqNumber = Convert.ToInt64(str[18]);
                order.OrderStatusTagValue = str[19];
                order.PranaMsgType = int.Parse(str[20]);
                order.ClOrderID = str[21];
                order.OrderTypeTagValue = str[22];
                order.Text = str[23];
                order.StagedOrderID = str[24];
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return order;

        }

        public static string CreateCancelRejectResponse(Order order)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(FIXConstants.MSGOrderCancelReject);
            sb.Append(Seperators.SEPERATOR_2);//0
            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(order.OrderID);
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append(order.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//3
            sb.Append(order.OrigClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(order.OrderSideTagValue);
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(order.ListID);
            sb.Append(Seperators.SEPERATOR_2);//6

            sb.Append(order.TransactionTime);
            sb.Append(Seperators.SEPERATOR_2);//7
            sb.Append(order.ParentClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//8
            sb.Append(order.OrderStatusTagValue);
            sb.Append(Seperators.SEPERATOR_2);//9

            sb.Append(order.ExecutionInstruction);
            sb.Append(Seperators.SEPERATOR_2);//10
            sb.Append(order.HandlingInstruction);
            sb.Append(Seperators.SEPERATOR_2);//11
            sb.Append(order.Quantity.ToString());
            sb.Append(Seperators.SEPERATOR_2);//12
            sb.Append(order.Price.ToString());
            sb.Append(Seperators.SEPERATOR_2);//13
            sb.Append(order.StopPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);//14
            sb.Append(order.OrderTypeTagValue);
            sb.Append(Seperators.SEPERATOR_2);//15
            sb.Append(order.CumQty.ToString());
            sb.Append(Seperators.SEPERATOR_2);//16
            sb.Append(order.AvgPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);//17
            sb.Append(order.LastPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);//18
            sb.Append(order.LastShares.ToString());
            sb.Append(Seperators.SEPERATOR_2);//19
            sb.Append(order.DiscretionOffset.ToString());
            sb.Append(Seperators.SEPERATOR_2);//20
            sb.Append(order.PegDifference.ToString());
            sb.Append(Seperators.SEPERATOR_2);//21
            sb.Append(order.DisplayQuantity.ToString());
            sb.Append(Seperators.SEPERATOR_2);//22
            sb.Append(order.TIF);
            sb.Append(Seperators.SEPERATOR_2);//23
            sb.Append(order.PranaMsgType.ToString());
            sb.Append(Seperators.SEPERATOR_2);//24
            sb.Append(order.StagedOrderID);
            sb.Append(Seperators.SEPERATOR_2);//25
            sb.Append(order.OrderSeqNumber.ToString());
            sb.Append(Seperators.SEPERATOR_2);//26
            sb.Append(order.ClientOrderID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//27
            sb.Append(order.ParentClientOrderID.ToString());
            //25
            str = sb.ToString();
            return str;
        }
        public static Order FromCancelRejectResponse(string message)
        {
            Order order = new Order();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                order.MsgType = str[0];
                order.TradingAccountID = int.Parse(str[1]);
                order.OrderID = str[2];
                order.ClOrderID = str[3];
                order.OrigClOrderID = str[4];
                order.OrderSideTagValue = str[5];
                order.ListID = str[6];
                order.TransactionTime = str[7];
                order.ParentClOrderID = str[8];
                order.OrderStatusTagValue = str[9];

                order.ExecutionInstruction = str[10];
                order.HandlingInstruction = str[11];
                order.Quantity=  double.Parse(str[12]);
                order.Price= double.Parse(str[13]);
                order.StopPrice= double.Parse(str[14]);
                order.OrderTypeTagValue= str[15];
                order.CumQty= double.Parse(str[16]);
                order.AvgPrice= double.Parse(str[17]);
                order.LastPrice= double.Parse(str[18]);
                order.LastShares= double.Parse(str[19]);
                order.DiscretionOffset= double.Parse(str[20]);
                order.PegDifference= double.Parse(str[21]);
                order.DisplayQuantity= double.Parse(str[22]);
                order.TIF= str[23];
                order.PranaMsgType = int.Parse( str[24]);
                order.StagedOrderID = str[25];
                order.OrderSeqNumber = Int64.Parse(str[26]);
                order.ClientOrderID = str[27];
                order.ParentClientOrderID = str[28];
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return order;

        }

        public static string CreateTransferUserRequest(Order order)
        {
            StringBuilder sb = new StringBuilder();
            string str = string.Empty;
            sb.Append(FIXConstants.MSGTransferUser);
            sb.Append(Seperators.SEPERATOR_2);//0
            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(order.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append(order.ParentClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//3
            sb.Append(order.TransactionTime);
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(order.CompanyUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(order.StagedOrderID);
            sb.Append(Seperators.SEPERATOR_2);//6
            sb.Append(order.PranaMsgType.ToString());
            //4
            
            //sb.Append(Seperators.SEPERATOR_2);//4
            //sb.Append(order.OrderSideTagValue);
            //sb.Append(Seperators.SEPERATOR_2);//5
            //sb.Append(order.ListID);
            //sb.Append(Seperators.SEPERATOR_2);//6
            str = sb.ToString();
            return str;
        }
        public static Order FromTransferUserRequest(string message)
        {
            Order order = new Order();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                order.MsgType = str[0];
                order.TradingAccountID = int.Parse(str[1]);
                order.ClOrderID = str[2];
                order.ParentClOrderID = str[3];
                order.TransactionTime = str[4];
                order.CompanyUserID = int.Parse(str[5]);
                order.StagedOrderID = str[6];
                order.PranaMsgType = int.Parse( str[7]);
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return order;

        }


        public static string GetFormattedMsg(Order order)
        {
            //TODO: For PranaMsgType = 8(Transfer user) make a separate Formatting method and call that ... 
            //Can be done similarly for any custom requests.
            switch (order.MsgType)
            {
                case FIXConstants.MSGOrder:
                    return CreateOrderSingle(order);

                case FIXConstants.MSGOrderCancelRequest:
                    return CreateCancelRequest(order);

                case FIXConstants.MSGExecutionReport:
                    return CreateFillReport(order);

                case FIXConstants.MSGOrderCancelReplaceRequest:
                    return CreateReplaceRequest(order);

                case FIXConstants.MSGOrderCancelReject:
                    return CreateCancelRejectResponse(order);

                case FIXConstants.MSGTransferUser:
                    return CreateTransferUserRequest(order);

                default:
                    return CreateOrderSingle(order);

            }

        }
        public static Order GetOrder(string message)
        {
            Order order = new Order();
            string msgType = GetMessageType(message);
            switch (msgType)
            {
                case FIXConstants.MSGOrderSingle:
                    order = PranaMessageFormatter.FromOrderSingle(message);
                    break;

                case FIXConstants.MSGOrderCancelRequest:
                    order = PranaMessageFormatter.FromCancelRequest(message);
                    break;

                case FIXConstants.MSGOrderCancelReplaceRequest:
                    order = PranaMessageFormatter.FromReplaceRequest(message);
                    break;
                case FIXConstants.MSGExecutionReport:
                    order = PranaMessageFormatter.FromFillReport(message);
                    break;

                case FIXConstants.MSGTransferUser:
                    order = PranaMessageFormatter.FromTransferUserRequest(message);
                    break;
                //case PranaMessageConstants.MSGTradingInstInternal:
                //    order = PranaMessageFormatter.GetOrderFromInternalTI(message);
                //    break;

            }
            return order;
        }

        #endregion
        
        #region Trading Instruction Related Methods
        //public static string CreateInternalTI(Order order)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string str = string.Empty;
        //    sb.Append(order.MsgType);
        //    sb.Append(Seperators.SEPERATOR_2);//0
        //    sb.Append(order.TargetCompID);
        //    sb.Append(Seperators.SEPERATOR_2);//1
        //    sb.Append(order.TradingAccountID);
        //    sb.Append(Seperators.SEPERATOR_2);//2
        //    sb.Append(order.ClOrderID);
        //    sb.Append(Seperators.SEPERATOR_2);//3
        //    sb.Append(order.Symbol);
        //    sb.Append(Seperators.SEPERATOR_2);//4
        //    sb.Append(order.OrderSideTagValue);
        //    sb.Append(Seperators.SEPERATOR_2);//5
        //    sb.Append(order.Quantity.ToString());
        //    sb.Append(Seperators.SEPERATOR_2);//6
        //    sb.Append(order.Text);
        //    sb.Append(Seperators.SEPERATOR_2);//7
        //    sb.Append(order.SenderCompID);
        //    sb.Append(Seperators.SEPERATOR_2);//7
        //    sb.Append(order.CompanyUserID.ToString());
        //    sb.Append(Seperators.SEPERATOR_2);//2
        //    sb.Append(order.ClientOrderID);


        //    //sb.Append(Seperators.SEPERATOR_2);//9
        //    //sb.Append(order.OrderTypeTagValue.ToString());

        //    str = sb.ToString();
        //    return str;
        //}

        /// <summary>
        /// Returns a string corresponding to a TradingInstruction with msgType TradingInstructionInternal
        /// </summary>
        /// <param name="tradingInstruction"></param>
        /// <returns></returns>
        
        public static string CreateInternalTI(TradingInstruction tradingInstruction )
        {
            //NOTE: For any change in the order of the fields change the methods FromInternalTI and GetOrderFromInternalTI
            StringBuilder sb = new StringBuilder();
            string str = string.Empty;
            sb.Append(tradingInstruction.MsgType);
            sb.Append(Seperators.SEPERATOR_2);//0
            sb.Append(tradingInstruction.TargetUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(tradingInstruction.TradingAccID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append(tradingInstruction.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//3
            sb.Append(tradingInstruction.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(tradingInstruction.Side);
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(tradingInstruction.Quantity.ToString());
            sb.Append(Seperators.SEPERATOR_2);//6
            sb.Append(tradingInstruction.Instructions);
            sb.Append(Seperators.SEPERATOR_2);//7
            sb.Append(tradingInstruction.SenderUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//8
            sb.Append(tradingInstruction.UserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(tradingInstruction.DeskID.ToString());

            str = sb.ToString();
            return str;
        }
       
        //public static Order GetOrderFromInternalTI(string message)
        //{
        //    Order tradingInst = new Order();
        //    try
        //    {
        //        string[] str = message.Split(Seperators.SEPERATOR_2);
        //        tradingInst.MsgType = str[0];
        //        tradingInst.TargetCompID = str[1];
        //        tradingInst.TradingAccountID = int.Parse(str[2]);
        //        tradingInst.ClOrderID = str[3];
        //        tradingInst.Symbol = str[4];
        //        tradingInst.OrderSideTagValue = str[5];
        //        tradingInst.Quantity = Double.Parse(str[6]);
        //        tradingInst.Text = str[7];
        //        tradingInst.SenderCompID = str[8];
        //        tradingInst.CompanyUserID = int.Parse(str[9]);
        //        tradingInst.ClientOrderID = str[10];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }
        //    return tradingInst;

        //}
    
        public static TradingInstruction FromInternalTI(string message)
        {
            TradingInstruction tradingInst = new TradingInstruction();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                tradingInst.MsgType = str[0];
                tradingInst.TargetUserID = int.Parse(str[1]);
                tradingInst.TradingAccID = int.Parse(str[2]);
                tradingInst.ClOrderID = str[3];
                tradingInst.Symbol = str[4];
                tradingInst.Side = str[5];
                tradingInst.Quantity = Double.Parse(str[6]);
                tradingInst.Instructions = str[7];
                tradingInst.SenderUserID = int.Parse(str[8]);
                tradingInst.UserID = int.Parse(str[9]);
                tradingInst.DeskID = str[10];
                int intStatus = -1;
                bool isSuccessfullyConverted = false;
                if (str.Length > 11)
                {
                    isSuccessfullyConverted = int.TryParse(str[11], out intStatus);
                    if (isSuccessfullyConverted && intStatus != -1)
                    {
                        tradingInst.Status = (TradingInstructionEnums.TradingInstStatus)intStatus;
                    }
                }
                
                
                
                //tradingInst.OrderTypeTagValue = str[10];

            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return tradingInst;

        }

        //public static string CreateInternalTIAccept(Order tradingInstruction)
        //{
        //    //NOTE: For any change in the order of the fields change the methods FromInternalTI and GetOrderFromInternalTI
        //    StringBuilder sb = new StringBuilder();
        //    string str = string.Empty;
        //    sb.Append(tradingInstruction.MsgType);
        //    sb.Append(Seperators.SEPERATOR_2);//0
        //    sb.Append(tradingInstruction.TargetCompID);
        //    sb.Append(Seperators.SEPERATOR_2);//1
        //    sb.Append(tradingInstruction.ClOrderID);
        //    sb.Append(Seperators.SEPERATOR_2);//2
        //    sb.Append(tradingInstruction.SenderCompID);
        //    sb.Append(Seperators.SEPERATOR_2);//3
        //    sb.Append(tradingInstruction.IsAccepted.ToString());
        //    sb.Append(Seperators.SEPERATOR_2);//4
        //    sb.Append(tradingInstruction.Symbol);
        //    sb.Append(Seperators.SEPERATOR_2);//5
        //    sb.Append(tradingInstruction.Quantity);
        //    sb.Append(Seperators.SEPERATOR_2);//6
        //    sb.Append(tradingInstruction.OrderSideTagValue);
        //    sb.Append(Seperators.SEPERATOR_2);//7
        //    sb.Append(tradingInstruction.TradingAccountID);
        //    sb.Append(Seperators.SEPERATOR_2);//8
        //    sb.Append(tradingInstruction.Text);
        //    sb.Append(Seperators.SEPERATOR_2);//9
        //    sb.Append(tradingInstruction.CompanyUserID.ToString());
        //    sb.Append(Seperators.SEPERATOR_2);//10
        //    sb.Append(tradingInstruction.ClientOrderID.ToString());

        //    str = sb.ToString();
        //    return str;
        //}

        public static string CreateInternalTIAccept(TradingInstruction tradingInstruction)
        {
            //NOTE: For any change in the order of the fields change the methods FromInternalTI and GetOrderFromInternalTI
            StringBuilder sb = new StringBuilder();
            string str = string.Empty;
            sb.Append(tradingInstruction.MsgType);
            sb.Append(Seperators.SEPERATOR_2);//0
            sb.Append(tradingInstruction.TargetUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(tradingInstruction.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append(tradingInstruction.SenderUserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//3
            sb.Append( ((int)tradingInstruction.Status).ToString());
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(tradingInstruction.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(tradingInstruction.Quantity);
            sb.Append(Seperators.SEPERATOR_2);//6
            sb.Append(tradingInstruction.Side);
            sb.Append(Seperators.SEPERATOR_2);//7
            sb.Append(tradingInstruction.TradingAccID);
            sb.Append(Seperators.SEPERATOR_2);//8
            sb.Append(tradingInstruction.Instructions);
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(tradingInstruction.UserID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(tradingInstruction.DeskID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(((int)tradingInstruction.AssetCategory).ToString());
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(((int)tradingInstruction.UnderLying).ToString());
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(tradingInstruction.ExchangeID.ToString());
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(tradingInstruction.AUECID.ToString());
            //sb.Append(tradingInstruction.DeskID);
            //sb.Append(Seperators.SEPERATOR_2);//9

            str = sb.ToString();
            return str;
        }
       
        //public static Order GetOrderFromInternalTIAccept(string message)
        //{
        //    Order tradingInst = new Order();
        //    try
        //    {
        //        string[] str = message.Split(Seperators.SEPERATOR_2);
        //        tradingInst.MsgType = str[0];
        //        tradingInst.TargetCompID = str[1];
        //        tradingInst.ClOrderID = str[2];
        //        tradingInst.SenderCompID = str[3];
        //        //tradingInst.IsAccepted = bool.Parse( str[4]);
                
        //        //tradingInst.Status = (TradingInstructionEnums.TradingInstStatus)int.Parse(str[4]);
        //        tradingInst.Symbol = str[5];
        //        tradingInst.Quantity = double.Parse(str[6]);
        //        tradingInst.OrderSideTagValue = str[7];
        //        tradingInst.TradingAccountID = int.Parse(str[8]);
        //        tradingInst.Text = str[9];
        //        tradingInst.CompanyUserID = int.Parse(str[10]);
        //        tradingInst.ClientOrderID = str[11];
        //        //tradingInst.ClientOrderID = str[9];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }
        //    return tradingInst;

        //}

        public static TradingInstruction FromInternalTIAccept(string message)
        {
            TradingInstruction tradingInst = new TradingInstruction();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                tradingInst.MsgType = str[0];
                tradingInst.TargetUserID = int.Parse(str[1]);
                tradingInst.ClOrderID = str[2];
                tradingInst.SenderUserID = int.Parse(str[3]);
                tradingInst.Status = (TradingInstructionEnums.TradingInstStatus)int.Parse(str[4]);
                tradingInst.Symbol = str[5];
                tradingInst.Quantity = double.Parse(str[6]);
                tradingInst.Side = str[7];
                tradingInst.TradingAccID = int.Parse(str[8]);
                tradingInst.Instructions = str[9];
                tradingInst.UserID = int.Parse(str[10]);
                tradingInst.DeskID = str[11];
                tradingInst.AssetCategory = (AssetCategory)int.Parse(str[12]);
                tradingInst.UnderLying = (Underlying)int.Parse(str[13]);
                tradingInst.ExchangeID = int.Parse(str[14]);
                tradingInst.AUECID = int.Parse(str[15]);
                //tradingInst.DeskID = str[10];
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return tradingInst;

        }

        #endregion

        #region EXposure PNL Related Methods

        public static string CreateExPnlCtrlMsg(string ctrlMsg, long sequenceNumber, ExPNLData exPNLDataType)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(PranaMessageConstants.MSG_ExpPNLCtrl);
            sb.Append(Seperators.SEPERATOR_2);//0

            //sb.Append(tradingAccountID);
            //sb.Append(Seperators.SEPERATOR_2);//1                                  
            sb.Append(ctrlMsg); //1
            sb.Append(Seperators.SEPERATOR_2);//1                                 
            sb.Append(sequenceNumber);//2  
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append((int)exPNLDataType); //3
                               
            str = sb.ToString();

            return str;

        }

        public static string[] FromExPnlCtrlMsg(string message)
        {
            string[] result = new string[3];

            String[] str;

            try
            {
                str = message.Split(Seperators.SEPERATOR_2);
                result[0] = str[1]; //ctrlMsg
                result[1] = str[2]; //sequenceNumber
                result[2] = str[3]; //exPNLDataType
               
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }

            return result;

        }

        public static string CreateExPnlSubscriptionMsg(string userID, ExPNLSubscriptionType subscriptionType, ExPNLData exPNLDataType)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(PranaMessageConstants.MSG_ExpPNLSubscription);
            sb.Append(Seperators.SEPERATOR_2); //0
            sb.Append(userID);
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append((int) subscriptionType);
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append((int)exPNLDataType); //3

            return sb.ToString();
        }

        public static void FromExPnlSubscriptionMsg(string message, out string userID, out int subscriptionType, out int exPNLDataType)
        {
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                userID = str[1];
                subscriptionType = int.Parse(str[2]);
                exPNLDataType = int.Parse(str[3]);
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
        }

        public static string CreateExPnlRefreshDataMsg()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(PranaMessageConstants.MSG_ExpPNLRefreshData);
            sb.Append(Seperators.SEPERATOR_2);//1
            return sb.ToString();
        }

        //public static ExPNLSubscriptionType FromExPnlRefreshDataMsg(string message, out string userID)
        //{
        //    ExPNLSubscriptionType subscriptionType = ExPNLSubscriptionType.None;

        //    try
        //    {
        //        string[] str = message.Split(Seperators.SEPERATOR_2);
        //        userID = str[1];
        //        subscriptionType = (ExPNLSubscriptionType)int.Parse(str[2]);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }
        //    return subscriptionType;

        //}

        public static string CreateExposureAndPnlOrderClearMsg(string tradingAccountID, List<string> IDsToBeCleared)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(PranaMessageConstants.MSG_ExpPNLClr);
            sb.Append(Seperators.SEPERATOR_2);//0

            sb.Append(tradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//1                      

            foreach (string id in IDsToBeCleared)
            {
                sb.Append(id + "$");
            }
            
            str = sb.ToString();

            return str;
        }

        public static List<string> FromExposureAndPnlOrderClearMsg (string message)
        {
            List<string> result;
            String[] str;

            try
            {
                str = message.Split(Seperators.SEPERATOR_2)[2].Split('$');

                result = new List<string>(str);
                //remove empty strings
                result.Remove(string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return result;
        }

        public static string CreateExposureAndPnlOrderSingle(ExposureAndPnlOrder order)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(PranaMessageConstants.MSG_ExpPNLCalc);
            sb.Append(Seperators.SEPERATOR_2);//0

            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//1                      

            sb.Append(order.ID);
            sb.Append(Seperators.SEPERATOR_2);//2

            sb.Append(order.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//3

            sb.Append(order.AUECID);
            sb.Append(Seperators.SEPERATOR_2);//4

            sb.Append((int)order.AssetCategory);
            sb.Append(Seperators.SEPERATOR_2);//5  

            sb.Append(order.OrderSideTagValue);
            sb.Append(Seperators.SEPERATOR_2);//6

            sb.Append(order.OrderStatusTagValue);
            sb.Append(Seperators.SEPERATOR_2);//7

            sb.Append(((int)order.OrderType).ToString());
            sb.Append(Seperators.SEPERATOR_2);//8

            sb.Append(order.AvgPrice);
            sb.Append(Seperators.SEPERATOR_2);//9

            sb.Append(order.CompanyUserID);
            sb.Append(Seperators.SEPERATOR_2);//10

            sb.Append(order.CumQty);
            sb.Append(Seperators.SEPERATOR_2);//11           

            sb.Append(order.Quantity);
            sb.Append(Seperators.SEPERATOR_2);//12 


            sb.Append(order.NetExposure);
            sb.Append(Seperators.SEPERATOR_2);//13

         
            sb.Append(order.NetNotionalValue);
            sb.Append(Seperators.SEPERATOR_2);//14

           
            sb.Append(order.DayPnL);
            sb.Append(Seperators.SEPERATOR_2);//15

         
            sb.Append(order.SideMultiplier);
            sb.Append(Seperators.SEPERATOR_2);//16                    

            sb.Append(order.FxRate);
            sb.Append(Seperators.SEPERATOR_2);//17

            sb.Append(order.AskPrice);
            sb.Append(Seperators.SEPERATOR_2);//18

            sb.Append(order.BidPrice);
            sb.Append(Seperators.SEPERATOR_2);//19

            sb.Append(order.LastPrice);
            sb.Append(Seperators.SEPERATOR_2);//20

            sb.Append(order.ClosingPrice);
            sb.Append(Seperators.SEPERATOR_2);//21

            sb.Append(order.HighPrice);
            sb.Append(Seperators.SEPERATOR_2);//22

            sb.Append(order.LowPrice);
            sb.Append(Seperators.SEPERATOR_2);//23
            

            //sb.Append(order.UserMark);
            //sb.Append(Seperators.SEPERATOR_2);//24

            sb.Append(order.Multiplier);
            sb.Append(Seperators.SEPERATOR_2);//24

            sb.Append(order.Delta);
            sb.Append(Seperators.SEPERATOR_2);//25

            sb.Append((int) order.SelectedFeedPrice);
            sb.Append(Seperators.SEPERATOR_2);//26

            sb.Append(order.FundID);
            sb.Append(Seperators.SEPERATOR_2);//27

            sb.Append(order.StrategyID);
            sb.Append(Seperators.SEPERATOR_2);//28

            sb.Append(order.TransactionDate);
            sb.Append(Seperators.SEPERATOR_2);//29

            sb.Append(order.SettlementDate);
            sb.Append(Seperators.SEPERATOR_2);//30

            sb.Append(order.StartOfDayPosition);
            sb.Append(Seperators.SEPERATOR_2);//31

            sb.Append(order.PercentagePositionLong);
            sb.Append(Seperators.SEPERATOR_2);//32

            sb.Append(order.PercentagePositionShort);
            sb.Append(Seperators.SEPERATOR_2);//33

            sb.Append(order.CostBasisRealizedPNL);
            sb.Append(Seperators.SEPERATOR_2);//34

            sb.Append(order.Sector);
            sb.Append(Seperators.SEPERATOR_2);//35

            //sb.Append(order.OpenClose);
            sb.Append(Seperators.SEPERATOR_2);//36

            sb.Append(order.UnderlyingSymbol);
            sb.Append(Seperators.SEPERATOR_2);//37
            
            sb.Append(order.MTDUnrealizedPnL);
            sb.Append(Seperators.SEPERATOR_2);//38

            //sb.Append(order.YTDUnrealizedPnL);
            //sb.Append(Seperators.SEPERATOR_2);//39

            sb.Append(order.MTDRealizedPnL);
            sb.Append(Seperators.SEPERATOR_2);//40

            sb.Append(order.MonthMarkPrice);
            sb.Append(Seperators.SEPERATOR_2);//41

            sb.Append(order.YesterdayMarkPrice);
            sb.Append(Seperators.SEPERATOR_2);//42

            sb.Append(order.Gamma);
            sb.Append(Seperators.SEPERATOR_2);//43

            sb.Append(order.Theta);
            sb.Append(Seperators.SEPERATOR_2);//44

            sb.Append(order.Kappa);
            sb.Append(Seperators.SEPERATOR_2);//45

            sb.Append(order.Rho);
            sb.Append(Seperators.SEPERATOR_2);//46

            sb.Append(order.DeltaAdjustedExposure);
            sb.Append(Seperators.SEPERATOR_2);//47

            sb.Append(order.DeltaAdjustedPosition);
            sb.Append(Seperators.SEPERATOR_2);//48

            sb.Append(order.GammaAdjustedExposure);
            sb.Append(Seperators.SEPERATOR_2);//49

            sb.Append(order.GammaAdjustedPosition);
            sb.Append(Seperators.SEPERATOR_2);//50

            sb.Append(order.ThetaAdjustedExposure);
            sb.Append(Seperators.SEPERATOR_2);//51

            sb.Append(order.ThetaAdjustedPosition);
            sb.Append(Seperators.SEPERATOR_2);//52
            
            sb.Append(order.DayPnlInCompanyBaseCurrency);
            sb.Append(Seperators.SEPERATOR_2);//53

            sb.Append(order.NetExposureInCompnayBaseCurrency);
            sb.Append(Seperators.SEPERATOR_2);//54

            sb.Append(order.UnderlyingStockPrice);
            //sb.Append(Seperators.SEPERATOR_2);//55

            str = sb.ToString();
            return str;
        }

        public static string[] CreateExposureAndPnlOrderChunk(ExposureAndPnlOrderCollection orders, int chunkSize)
        {

            int numberOfChunks = (int) Math.Ceiling((double)orders.Count / chunkSize);

            string[] result = new string[numberOfChunks];                   
            
            for (int i = 0; i < numberOfChunks; i++)
			{
                StringBuilder chunk = new StringBuilder();

                chunk.Append(PranaMessageConstants.MSG_ExpPNLCalc);
                chunk.Append(Seperators.SEPERATOR_2);//0

               // chunk.Append(tradingAccountID);
                chunk.Append(",^");//1 

                for (int j = chunkSize * (i); j < chunkSize * (i+1) && j< orders.Count; j++)
                {
                    ExposureAndPnlOrder order = orders[j];

                    if (order.Quantity > 0)
                    {
                        chunk.Append(CreateTrimmedExPNLOrderSingle(order));
                        chunk.Append("~");
                    }
                }

                result[i] = chunk.ToString();
			}
            
            return result;
        }

        public static string CreateTrimmedExPNLOrderSingle(ExposureAndPnlOrder order)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(PranaMessageConstants.MSG_ExpPNLCalc);
            sb.Append(Seperators.SEPERATOR_2);//0

            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//1                      

            sb.Append(order.ID);
            sb.Append(Seperators.SEPERATOR_2);//2

            sb.Append(order.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//3

            sb.Append(order.AUECID);
            sb.Append(Seperators.SEPERATOR_2);//4

            sb.Append((int)order.AssetCategory);
            sb.Append(Seperators.SEPERATOR_2);//5  

            sb.Append(order.OrderSideTagValue);
            sb.Append(Seperators.SEPERATOR_2);//6

            sb.Append(order.AvgPrice);
            sb.Append(Seperators.SEPERATOR_2);//7

            sb.Append(order.CompanyUserID);
            sb.Append(Seperators.SEPERATOR_2);//8

            sb.Append(order.CumQty);
            sb.Append(Seperators.SEPERATOR_2);//9           

            sb.Append(order.Quantity);
            sb.Append(Seperators.SEPERATOR_2);//10 

            sb.Append(order.NetExposure);
            sb.Append(Seperators.SEPERATOR_2);//11

            //sb.Append(order.OrderStatusTagValue);
            //sb.Append(Seperators.SEPERATOR_2);//7

            //sb.Append(((int)order.OrderType).ToString());
            //sb.Append(Seperators.SEPERATOR_2);//8


            sb.Append(order.NetNotionalValue);
            sb.Append(Seperators.SEPERATOR_2);//12

            sb.Append(order.DayPnL);
            sb.Append(Seperators.SEPERATOR_2);//13

            sb.Append(order.SideMultiplier);
            sb.Append(Seperators.SEPERATOR_2);//14                    

            sb.Append(order.FxRate);
            sb.Append(Seperators.SEPERATOR_2);//15


            sb.Append(order.LastPrice);
            sb.Append(Seperators.SEPERATOR_2);//16

            sb.Append(order.ClosingPrice);
            sb.Append(Seperators.SEPERATOR_2);//17


            sb.Append(order.Multiplier);
            sb.Append(Seperators.SEPERATOR_2);//18

            sb.Append((int)order.SelectedFeedPrice);
            sb.Append(Seperators.SEPERATOR_2);//19

            sb.Append(order.FundID);
            sb.Append(Seperators.SEPERATOR_2);//20

            sb.Append(order.StrategyID);
            sb.Append(Seperators.SEPERATOR_2);//21

            sb.Append(order.CostBasisRealizedPNL);
            sb.Append(Seperators.SEPERATOR_2);//22

            //sb.Append(order.OpenClose);
            sb.Append(Seperators.SEPERATOR_2);//23

            sb.Append(order.UnderlyingSymbol);
            sb.Append(Seperators.SEPERATOR_2);//24

            sb.Append(order.MonthMarkPrice);
            sb.Append(Seperators.SEPERATOR_2);//25

            sb.Append(order.YesterdayMarkPrice);
            sb.Append(Seperators.SEPERATOR_2);//26

            sb.Append(order.MTDUnrealizedPnL);
            sb.Append(Seperators.SEPERATOR_2);//27

            sb.Append(order.DayPnL);
            sb.Append(Seperators.SEPERATOR_2);//28

            sb.Append(order.MTDRealizedPnL);
            sb.Append(Seperators.SEPERATOR_2);//29

            sb.Append(order.Delta);
            sb.Append(Seperators.SEPERATOR_2);//30        

            sb.Append(((int)order.OrderType).ToString());
            sb.Append(Seperators.SEPERATOR_2);//31

            sb.Append(Math.Round(((double)order.PercentageChange), 4).ToString());
            sb.Append(Seperators.SEPERATOR_2);//32
            //string[] str1 = sb.ToString().Split(Seperators.SEPERATOR_2);

            //sb.Append(order.HighPrice);
            //sb.Append(Seperators.SEPERATOR_2);//18

            //sb.Append(order.LowPrice);
            //sb.Append(Seperators.SEPERATOR_2);//23


            //sb.Append(order.UserMark);
            //sb.Append(Seperators.SEPERATOR_2);//24

            //sb.Append(order.TransactionDate);
            //sb.Append(Seperators.SEPERATOR_2);//29

            //sb.Append(order.SettlementDate);
            //sb.Append(Seperators.SEPERATOR_2);//30

            //sb.Append(order.StartOfDayPosition);
            //sb.Append(Seperators.SEPERATOR_2);//31

            //sb.Append(order.PercentagePositionLong);
            //sb.Append(Seperators.SEPERATOR_2);//32

            //sb.Append(order.PercentagePositionShort);
            //sb.Append(Seperators.SEPERATOR_2);//33       

            //sb.Append(order.Sector);

            //sb.Append(Seperators.SEPERATOR_2);//35

            //sb.Append(order.Gamma);
            //sb.Append(Seperators.SEPERATOR_2);//43

            //sb.Append(order.Theta);
            //sb.Append(Seperators.SEPERATOR_2);//44

            //sb.Append(order.Kappa);
            //sb.Append(Seperators.SEPERATOR_2);//45

            //sb.Append(order.Rho);
            //sb.Append(Seperators.SEPERATOR_2);//46

            //sb.Append(order.DeltaAdjustedExposure);
            //sb.Append(Seperators.SEPERATOR_2);//47

            //sb.Append(order.DeltaAdjustedPosition);
            //sb.Append(Seperators.SEPERATOR_2);//48

            //sb.Append(order.GammaAdjustedExposure);
            //sb.Append(Seperators.SEPERATOR_2);//49

            //sb.Append(order.GammaAdjustedPosition);
            //sb.Append(Seperators.SEPERATOR_2);//50

            //sb.Append(order.ThetaAdjustedExposure);
            //sb.Append(Seperators.SEPERATOR_2);//51

            //sb.Append(order.ThetaAdjustedPosition);
            ////sb.Append(Seperators.SEPERATOR_2);//52

            sb.Append(order.TransactionDate);
            sb.Append(Seperators.SEPERATOR_2);//33

            sb.Append(order.DayPnlInCompanyBaseCurrency);
            sb.Append(Seperators.SEPERATOR_2);//34

            sb.Append(order.NetExposureInCompnayBaseCurrency);
            sb.Append(Seperators.SEPERATOR_2);//35

            sb.Append(order.UnderlyingStockPrice);
            sb.Append(Seperators.SEPERATOR_2);//36

            sb.Append(order.NetNotionalValueInCompanyBaseCurrency);
            sb.Append(Seperators.SEPERATOR_2);//37

            sb.Append(order.AskPrice);
            sb.Append(Seperators.SEPERATOR_2);//38

            sb.Append(order.BidPrice);
            sb.Append(Seperators.SEPERATOR_2);//39

            sb.Append(order.MidPrice);
            sb.Append(Seperators.SEPERATOR_2);//40

            sb.Append(order.FullSecurityName);
            //sb.Append(Seperators.SEPERATOR_2);//41

            str = sb.ToString();
            return str;
        }

        public static ExposurePnlCacheItem GetCacheItemFromExPNLOrderSingle(string message)
        {
            ExposurePnlCacheItem order = new ExposurePnlCacheItem();

            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);

                order.MsgType = str[0];

                order.TradingAccountID = Convert.ToInt32(str[1]);

                order.ID = str[2];

                order.Symbol = str[3];

                order.AUECID = Convert.ToInt32(str[4]);

                //NOTE: We do not need Asset Category as we will find it out using the AUECHelper Class
                //order.AssetCategory = (AssetCategory)Convert.ToInt32(str[5]);

                order.OrderSideTagValue = str[6];

                //order.OrderStatusTagValue = str[7];


                //order.OrderType = (ConsolidationInfoType)Convert.ToInt32(str[8]);

                order.AvgPrice = Convert.ToDouble(str[7]);

                order.CompanyUserID = Convert.ToInt32(str[8]);

                order.ExecutedQty = Convert.ToDouble(str[9]);

                if (Convert.ToDouble(str[10]) != double.Epsilon)
                {
                    order.Quantity = Convert.ToInt64(str[10]);
                }
                order.NetExposure = Convert.ToDouble(str[11]);

                order.NetNotionalValue = Convert.ToDouble(str[12]);

                order.DayPnL = Convert.ToDouble(str[13]);

                ///SideMultiplier should be set before ConsolidatioInfoType 
                ///so that PositionType can be set properly!
                order.SideMultiplier = Convert.ToInt32(str[14]);

                order.FxRate = Convert.ToDouble(str[15]);

                //order.AskPrice = Convert.ToDouble(str[18]);

                //order.BidPrice = Convert.ToDouble(str[19]);
                double lastPrice = 0;
                double closePrice = 0;
                double askPrice = 0;
                double bidPrice = 0;
                double midPrice = 0;
                double.TryParse(str[16], out lastPrice);
                double.TryParse(str[17], out closePrice);
                double.TryParse(str[38], out askPrice);
                double.TryParse(str[39], out bidPrice);
                double.TryParse(str[40], out midPrice);

                order.LastPrice = lastPrice; // Convert.ToDouble(str[16]);

                order.ClosingPrice = closePrice;

                //order.HighPrice = Convert.ToDouble(str[22]);

                //order.LowPrice = Convert.ToDouble(str[23]);

                //order.UserMark = Convert.ToDouble(str[24]);

                order.Multiplier = Convert.ToDouble(str[18]);

                //order.Delta = Convert.ToDouble(str[25]);

                order.SelectedFeedPrice = (SelectedFeedPrice)Convert.ToInt32(str[19]);

                order.FundID = Convert.ToInt32(str[20]);

                order.StrategyID = Convert.ToInt32(str[21]);

                //DateTime tranDate;
                //if (DateTime.TryParse(str[29].ToString(), out tranDate))
                //{

                //    order.TransactionDate = tranDate;
                //}

                //DateTime settlementDate;
                //if (DateTime.TryParse(str[30], out settlementDate))
                //{
                //    order.SettlementDate = settlementDate;
                //}

                //order.StartOfDayPosition = Convert.ToInt64(str[31]);

                //order.PercentagePositionLong = Convert.ToDouble(str[32]);

                //order.PercentagePositionShort = Convert.ToDouble(str[33]);

                order.CostBasisRealizedPNL = Convert.ToDouble(str[22]);

                //order.Sector = str[35];

                //order.OpenClose = str[23];

                //if (order.OpenClose == null)
                //{
                //    order.OpenClose = "";
                //}


                order.UnderlyingSymbol = str[24];

                order.MonthMarkPrice = Convert.ToDouble(str[25]);

                //order.YesterdayMarkPrice = Convert.ToDouble(str[26]);
                order.YesterdayMarkPrice = str[26];

                order.MTDUnrealizedPnL = Convert.ToDouble(str[27]);

                //order.YTDUnrealizedPnL = Convert.ToDouble(str[28]);

                order.MTDRealizedPnL = Convert.ToDouble(str[29]);

                order.Delta = Convert.ToDouble(str[30]);

                order.ConsolidationInfoType = (ConsolidationInfoType)Convert.ToInt32(str[31]);

                order.PercentageChange = Convert.ToDouble(str[32]);

                order.TradeDate = Convert.ToDateTime(str[33]);
                //order.Gamma = Convert.ToDouble(str[43]);

                //order.Theta = Convert.ToDouble(str[44]);

                //order.Kappa = Convert.ToDouble(str[45]);

                //order.Rho = Convert.ToDouble(str[46]);

                //order.DeltaAdjustedExposure = Convert.ToDouble(str[47]);

                //order.DeltaAdjustedPosition = Convert.ToDouble(str[48]);

                //order.GammaAdjustedExposure = Convert.ToDouble(str[49]);

                //order.GammaAdjustedPosition = Convert.ToDouble(str[50]);

                //order.ThetaAdjustedExposure = Convert.ToDouble(str[51]);

                //order.ThetaAdjustedPosition = Convert.ToDouble(str[52]);

                order.DayPnLInCompnayBaseCurrency = Convert.ToDouble(str[34]);//done from time being from date time to double: abhishek
                order.NetExposureInCompnayBaseCurrency = Convert.ToDouble(str[35]);
                order.UnderlyingStockPrice = Convert.ToDouble(str[36]);
                order.NetNotionalValueInCompnayBaseCurrency = Convert.ToDouble(str[37]);
                order.AskPrice = askPrice;
                order.BidPrice = bidPrice;
                order.MidPrice = midPrice;
                order.FullSecurityName = Convert.ToString(str[41]);
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }
            return order;
        }

        public static string CreateExposureAndPnlOrderSummary(ExposureAndPnlOrderSummary summaryOrder)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(PranaMessageConstants.MSG_ExpPNLCalcSummary);
            sb.Append(Seperators.SEPERATOR_2);//0

            sb.Append(summaryOrder.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//1

            sb.Append(summaryOrder.LongExposure);
            sb.Append(Seperators.SEPERATOR_2);//2

            sb.Append(summaryOrder.ShortExposure);
            sb.Append(Seperators.SEPERATOR_2);//3

            sb.Append(summaryOrder.NetExposure);
            sb.Append(Seperators.SEPERATOR_2);//4

            sb.Append(summaryOrder.DayPnLLong);
            sb.Append(Seperators.SEPERATOR_2);//5

            sb.Append(summaryOrder.DayPnLShort);
            sb.Append(Seperators.SEPERATOR_2);//6

            sb.Append(summaryOrder.DayPnL);
            sb.Append(Seperators.SEPERATOR_2);//7

            sb.Append(summaryOrder.CashInflow);
            sb.Append(Seperators.SEPERATOR_2);//8

            sb.Append(summaryOrder.CashOutflow);
            sb.Append(Seperators.SEPERATOR_2);//9

            sb.Append(summaryOrder.NetCashflow);
            sb.Append(Seperators.SEPERATOR_2);//10

            sb.Append(summaryOrder.LongNotionalValue);
            sb.Append(Seperators.SEPERATOR_2);//11

            sb.Append(summaryOrder.ShortNotionalValue);
            sb.Append(Seperators.SEPERATOR_2);//12

            sb.Append(summaryOrder.CashPosition);
            sb.Append(Seperators.SEPERATOR_2);//13

            sb.Append(summaryOrder.MTDUnrealizedPnL);
            sb.Append(Seperators.SEPERATOR_2);//14

            sb.Append(summaryOrder.CostBasisRealizedPNL);
            sb.Append(Seperators.SEPERATOR_2);//15

            sb.Append(summaryOrder.MTDRealizedPNL);
            sb.Append(Seperators.SEPERATOR_2);//16

            ///If strategy id is int.MinValue in the order then it is a global strategy summary
            sb.Append(summaryOrder.StrategyID);
            sb.Append(Seperators.SEPERATOR_2);//17

            //sb.Append(summaryOrder.BookDelta);
            //sb.Append(Seperators.SEPERATOR_2);//18
            
            //sb.Append(summaryOrder.BookGamma);
            //sb.Append(Seperators.SEPERATOR_2);//19

            //sb.Append(summaryOrder.BookKappa);
            //sb.Append(Seperators.SEPERATOR_2);//20

            //sb.Append(summaryOrder.BookRho);
            //sb.Append(Seperators.SEPERATOR_2);//21

            //sb.Append(summaryOrder.BookTheta);
            //sb.Append(Seperators.SEPERATOR_2);//22

            ///If fund id is int.MinValue in the order then it is a global fund summary
            sb.Append(summaryOrder.FundID);
            sb.Append(Seperators.SEPERATOR_2);//18

            sb.Append(summaryOrder.NetNotionalValue);
            //sb.Append(Seperators.SEPERATOR_2);//19

            return sb.ToString();

        }

        //public static ExposureAndPnlOrderSummary FromExposureAndPnlOrderSummary(string message)
        public static ExposureAndPnlOrderSummary FromExposureAndPnlOrderSummary(string message)
        {
            ExposureAndPnlOrderSummary orderSummary = new ExposureAndPnlOrderSummary();

            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);

                orderSummary.MsgType = str[0];

                orderSummary.TradingAccountID = Convert.ToInt64(str[1]);

                orderSummary.LongExposure = Convert.ToDouble(str[2]);

                orderSummary.ShortExposure = Convert.ToDouble(str[3]);

                orderSummary.NetExposure = Convert.ToDouble(str[4]);

                orderSummary.DayPnLLong = Convert.ToDouble(str[5]);

                orderSummary.DayPnLShort = Convert.ToDouble(str[6]);

                orderSummary.DayPnL = Convert.ToDouble(str[7]);

                orderSummary.CashInflow = Convert.ToDouble(str[8]);

                orderSummary.CashOutflow = Convert.ToDouble(str[9]);

                orderSummary.NetCashflow = Convert.ToDouble(str[10]);

                orderSummary.LongNotionalValue = Convert.ToDouble(str[11]);

                orderSummary.ShortNotionalValue = Convert.ToDouble(str[12]);

                orderSummary.CashPosition = Convert.ToDouble(str[13]);

                orderSummary.MTDUnrealizedPnL = Convert.ToDouble(str[14]);

                //orderSummary.YTDUnrealizedPnL = Convert.ToDouble(str[15]);

                orderSummary.CostBasisRealizedPNL = Convert.ToDouble(str[15]);

                orderSummary.MTDRealizedPNL = Convert.ToDouble(str[16]);

                orderSummary.StrategyID = Convert.ToInt32(str[17]);

                orderSummary.FundID = Convert.ToInt32(str[18]);

                orderSummary.NetNotionalValue = Convert.ToDouble(str[19]);

                //orderSummary.BookDelta = Convert.ToDouble(str[18]);

                //orderSummary.BookGamma = Convert.ToDouble(str[19]);

                //orderSummary.BookKappa = Convert.ToDouble(str[20]);

                //orderSummary.BookRho = Convert.ToDouble(str[21]);

                //orderSummary.BookTheta = Convert.ToDouble(str[22]);

                //orderSummary.NetAssetValue = Convert.ToDouble(str[11]);

                //orderSummary.OneDayPerformanceInBasisPoint = Convert.ToDouble(str[12]);

                //orderSummary.Leverage = Convert.ToDouble(str[13]);

                
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }

            return orderSummary;
        }
        
        #region Commented
        ///// <summary>
        ///// Froms the Exposure and PNL order from Message received from Server.
        ///// Works in similar fashion as of FromOrderSingle except it has less fields !!
        ///// </summary>
        ///// <remarks>In case of any change in order format this function will not work !!</remarks>
        ///// <param name="message">The message.</param>
        ///// <returns></returns>
        //public static ExposureAndPnlOrder FromExposureAndPnlOrderSingleForServerMessages(string message)
        //{
        //    ExposureAndPnlOrder order = new ExposureAndPnlOrder();
        //    try
        //    {
        //        string[] str = message.Split(Seperators.SEPERATOR_2);

        //        order.ClOrderID = str[11];

        //        order.CompanyUserID = Convert.ToInt32(str[24]);

        //        order.TradingAccountID = Convert.ToInt32(str[23]);

        //        order.AssetID = (AssetCategory) Convert.ToInt32(str[25]);

        //        order.UnderlyingID = Convert.ToInt32(str[26]);

        //        order.AUECID = Convert.ToInt32(str[30]);

        //        order.CumQty = Convert.ToDouble(str[33]);

        //        order.AvgPrice = double.Parse(str[32], System.Globalization.NumberStyles.Float);

        //        order.Symbol = str[3];

        //        order.OrderSideTagValue = str[1];                                               

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }
        //    return order;
        //}

        //public static OrderFill FromFillReportInOrderFill(string message)
        //{
        //    OrderFill order = new OrderFill();
        //    try
        //    {
        //        string[] str = message.Split(Seperators.SEPERATOR_2);
        //        order.MsgType = str[0];
        //        order.ClOrderID = str[1];
        //        order.OrigClOrderID = str[2];
        //        order.OrderID = str[3];
        //        order.ListID = str[4];
        //        order.WaveID = str[5];
        //        order.CumQty = Convert.ToDouble(str[6]);
        //        order.Price = double.Parse(str[7], System.Globalization.NumberStyles.Float);
        //        order.AvgPrice = double.Parse(str[8], System.Globalization.NumberStyles.Float);
        //        order.OrderStatusTagValue = str[9];
        //        order.LeavesQty = double.Parse(str[10], System.Globalization.NumberStyles.Float);
        //        order.LastShares = double.Parse(str[11], System.Globalization.NumberStyles.Float);
        //        order.LastShares = Convert.ToDouble(str[12]);
        //        order.LastPrice = double.Parse(str[13], System.Globalization.NumberStyles.Float);
        //        order.LastMarket = str[14];
        //        order.BasketSequenceNumber = int.Parse(str[15]);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }

        //    return order;

        //}
        #endregion
        #endregion

        #region Ex PNL Short Methods

        //public static ExposureAndPnlOrderCollection FromExposureAndPnlOrderChunk(string message)
        //{
        //    ExposureAndPnlOrderCollection orders = new ExposureAndPnlOrderCollection();

        //    try
        //    {
        //        string[] str = message.Split('^')[1].Split('~');

        //        for (int i = 0; i < str.Length -1; i++)
        //        {
        //            ExposureAndPnlOrder order = FromTrimmedExPNLOrderSingle(str[i]);

        //            orders.Add(order);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }

        //    return orders;
        //}

        //public static ExposureAndPnlOrder FromExposureAndPnlOrderSingle(string message)
        //{
        //    ExposureAndPnlOrder order = new ExposureAndPnlOrder();

        //    try
        //    {
        //        string[] str = message.Split(Seperators.SEPERATOR_2);

        //        order.MsgType = str[0];

        //        order.TradingAccountID = Convert.ToInt32(str[1]);

        //        order.ID = str[2];

        //        order.Symbol = str[3];

        //        order.AUECID = Convert.ToInt64(str[4]);

        //        order.AssetCategory = (AssetCategory)Convert.ToInt32(str[5]);

        //        order.OrderSideTagValue = str[6];

        //        order.OrderStatusTagValue = str[7];

        //        ///SideMultiplier should be set before OrderType 
        //        ///so that PositionType can be set properly!
        //        order.SideMultiplier = Convert.ToInt32(str[16]);

        //        order.OrderType = (ConsolidationInfoType)Convert.ToInt32(str[8]);

        //        order.AvgPrice = Convert.ToDouble(str[9]);

        //        order.CompanyUserID = Convert.ToInt32(str[10]);

        //        order.CumQty = Convert.ToDouble(str[11]);

        //        order.Quantity = Convert.ToInt64(str[12]);

        //        order.NetExposure = Convert.ToDouble(str[13]);

        //        order.NetNotionalValue = Convert.ToDouble(str[14]);

        //        order.DayPnL = Convert.ToDouble(str[15]);


        //        order.FxRate = Convert.ToDouble(str[17]);

        //        order.AskPrice = Convert.ToDouble(str[18]);

        //        order.BidPrice = Convert.ToDouble(str[19]);

        //        order.LastPrice = Convert.ToDouble(str[20]);

        //        order.ClosingPrice = Convert.ToDouble(str[21]);

        //        order.HighPrice = Convert.ToDouble(str[22]);

        //        order.LowPrice = Convert.ToDouble(str[23]);

        //        //order.UserMark = Convert.ToDouble(str[24]);

        //        order.Multiplier = Convert.ToDouble(str[24]);

        //        order.Delta = Convert.ToDouble(str[25]);

        //        order.SelectedFeedPrice = (SelectedFeedPrice)Convert.ToInt32(str[26]);

        //        order.FundID = Convert.ToInt32(str[27]);

        //        order.StrategyID = Convert.ToInt32(str[28]);

        //        DateTime tranDate;
        //        if (DateTime.TryParse(str[29].ToString(), out tranDate))
        //        {

        //            order.TransactionDate = tranDate;
        //        }

        //        DateTime settlementDate;
        //        if (DateTime.TryParse(str[30], out settlementDate))
        //        {
        //            order.SettlementDate = settlementDate;
        //        }

        //        order.StartOfDayPosition = Convert.ToInt64(str[31]);

        //        //order.PositionPNL = Convert.ToDouble(str[32]);


        //        order.PercentagePositionLong = Convert.ToDouble(str[32]);

        //        order.PercentagePositionShort = Convert.ToDouble(str[33]);

        //        order.CostBasisRealizedPNL = Convert.ToDouble(str[34]);

        //        order.Sector = str[35];

        //        //order.OpenClose = str[36];

        //        //if (order.OpenClose == null)
        //        //{
        //        //    order.OpenClose = "";
        //        //}

        //        order.UnderlyingSymbol = str[37];

        //        order.MTDUnrealizedPnL = Convert.ToDouble(str[38]);

        //        order.DayPnL = Convert.ToDouble(str[39]);

        //        order.MTDRealizedPnL = Convert.ToDouble(str[40]);

        //        order.MonthMarkPrice = Convert.ToDouble(str[41]);

        //        order.YesterdayMarkPrice = Convert.ToDouble(str[42]);

        //        order.Kappa = Convert.ToDouble(str[44]);

        //        order.Rho = Convert.ToDouble(str[45]);

        //        order.DeltaAdjustedExposure = Convert.ToDouble(str[46]);

        //        order.DeltaAdjustedPosition = Convert.ToDouble(str[47]);

        //        order.GammaAdjustedExposure = Convert.ToDouble(str[48]);

        //        order.GammaAdjustedPosition = Convert.ToDouble(str[49]);

        //        order.ThetaAdjustedExposure = Convert.ToDouble(str[50]);

        //        order.ThetaAdjustedPosition = Convert.ToDouble(str[51]);

        //        order.DayPnlInCompanyBaseCurrency = Convert.ToDouble(str[52]);

        //        order.NetExposureInCompnayBaseCurrency = Convert.ToDouble(str[53]);
        //        order.UnderlyingStockPrice = Convert.ToDouble(str[54]);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }
        //return order;
        //}

        /// <summary>
        /// Froms the trimmed ex PNL order single.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        //public static ExposureAndPnlOrder FromTrimmedExPNLOrderSingle(string message)
        //{
        //    ExposureAndPnlOrder order = new ExposureAndPnlOrder();

        //    try
        //    {
        //        string[] str = message.Split(Seperators.SEPERATOR_2);

        //        order.MsgType = str[0];

        //        order.TradingAccountID = Convert.ToInt32(str[1]);

        //        order.ID = str[2];

        //        order.Symbol = str[3];

        //        order.AUECID = Convert.ToInt64(str[4]);

        //        order.AssetCategory = (AssetCategory)Convert.ToInt32(str[5]);

        //        order.OrderSideTagValue = str[6];

        //        //order.OrderStatusTagValue = str[7];


        //        //order.OrderType = (ConsolidationInfoType)Convert.ToInt32(str[8]);

        //        order.AvgPrice = Convert.ToDouble(str[7]);

        //        order.CompanyUserID = Convert.ToInt32(str[8]);

        //        order.CumQty = Convert.ToDouble(str[9]);

        //        order.Quantity = Convert.ToInt64(str[10]);

        //        order.NetExposure = Convert.ToDouble(str[11]);

        //        order.NetNotionalValue = Convert.ToDouble(str[12]);

        //        order.DayPnL = Convert.ToDouble(str[13]);
                
        //        ///SideMultiplier should be set before ConsolidatioInfoType 
        //        ///so that PositionType can be set properly!
        //        order.SideMultiplier = Convert.ToInt32(str[14]);


        //        order.FxRate = Convert.ToDouble(str[15]);

        //        order.LastPrice = Convert.ToDouble(str[16]);

        //        order.ClosingPrice = Convert.ToDouble(str[17]);

               

        //        //order.HighPrice = Convert.ToDouble(str[22]);

        //        //order.LowPrice = Convert.ToDouble(str[23]);

        //        //order.UserMark = Convert.ToDouble(str[24]);

        //        order.Multiplier = Convert.ToDouble(str[18]);

        //        //order.Delta = Convert.ToDouble(str[25]);

        //        order.SelectedFeedPrice = (SelectedFeedPrice)Convert.ToInt32(str[19]);

        //        order.FundID = Convert.ToInt32(str[20]);

        //        order.StrategyID = Convert.ToInt32(str[21]);

        //        //DateTime tranDate;
        //        //if (DateTime.TryParse(str[29].ToString(), out tranDate))
        //        //{

        //        //    order.TransactionDate = tranDate;
        //        //}

        //        //DateTime settlementDate;
        //        //if (DateTime.TryParse(str[30], out settlementDate))
        //        //{
        //        //    order.SettlementDate = settlementDate;
        //        //}

        //        //order.StartOfDayPosition = Convert.ToInt64(str[31]);

        //        //order.PercentagePositionLong = Convert.ToDouble(str[32]);

        //        //order.PercentagePositionShort = Convert.ToDouble(str[33]);

        //        order.CostBasisRealizedPNL = Convert.ToDouble(str[22]);

        //        //order.Sector = str[35];

        //        //order.OpenClose = str[23];

        //        //if (order.OpenClose == null)
        //        //{
        //        //    order.OpenClose = "";
        //        //}
        //        order.UnderlyingSymbol = str[24];
                
        //        order.MonthMarkPrice = Convert.ToDouble(str[25]);

        //        order.YesterdayMarkPrice = Convert.ToDouble(str[26]);

        //        order.MTDUnrealizedPnL = Convert.ToDouble(str[27]);

        //        order.DayPnL = Convert.ToDouble(str[28]);

        //        order.MTDRealizedPnL = Convert.ToDouble(str[29]);

        //        order.Delta = Convert.ToDouble(str[30]);

        //        order.OrderType = (ConsolidationInfoType)Convert.ToInt32(str[31]);

        //        //order.PercentageChange = Convert.ToDouble(str[32]);

        //        order.TransactionDate = Convert.ToDateTime(str[33]);

               

        //        //order.Gamma = Convert.ToDouble(str[43]);

        //        //order.Theta = Convert.ToDouble(str[44]);

        //        //order.Kappa = Convert.ToDouble(str[45]);

        //        //order.Rho = Convert.ToDouble(str[46]);

        //        //order.DeltaAdjustedExposure = Convert.ToDouble(str[47]);

        //        //order.DeltaAdjustedPosition = Convert.ToDouble(str[48]);

        //        //order.GammaAdjustedExposure = Convert.ToDouble(str[49]);

        //        //order.GammaAdjustedPosition = Convert.ToDouble(str[50]);

        //        //order.ThetaAdjustedExposure = Convert.ToDouble(str[51]);

        //        //order.ThetaAdjustedPosition = Convert.ToDouble(str[52]);
        //        order.DayPnlInCompanyBaseCurrency = Convert.ToDouble(str[34]);// done from time being from date time to double: abhishek

        //        order.NetExposureInCompnayBaseCurrency = Convert.ToDouble(str[35]);

        //        order.UnderlyingStockPrice = Convert.ToDouble(str[36]);
        //        order.NetNotionalValueInCompanyBaseCurrency = Convert.ToDouble(str[37]);
        //        order.AskPrice = Convert.ToDouble(str[38]);
        //        order.BidPrice = Convert.ToDouble(str[39]);
        //        order.MidPrice = Convert.ToDouble(str[40]);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Prana: Message Format Error.", ex);

        //    }
        //    return order;
        //}

        #endregion 

        #region Basket Related Methods

        public static string CreateOrderForBasketResponse(Order order)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(FIXConstants.MSGOrder);
            sb.Append(Seperators.SEPERATOR_2);//0
            sb.Append(order.ParentClientOrderID);
            sb.Append(Seperators.SEPERATOR_2);//1
            sb.Append(order.ClientOrderID);
            sb.Append(Seperators.SEPERATOR_2);//2
            sb.Append(order.ClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//3
            if (order.MsgType == FIXConstants.MSGOrder)
            {
                order.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
            }
            else if (order.MsgType == FIXConstants.MSGOrderCancelRequest)
            {
                order.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingCancel;
            }
            else if (order.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
            {
                order.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingReplace;
            }
            sb.Append(order.OrderStatusTagValue);
            sb.Append(Seperators.SEPERATOR_2);//4
            sb.Append(order.ParentClOrderID);
            sb.Append(Seperators.SEPERATOR_2);//5
            sb.Append(order.Quantity);
            sb.Append(Seperators.SEPERATOR_2);//6
            sb.Append(order.LeavesQty);
            sb.Append(Seperators.SEPERATOR_2);//7
            sb.Append(order.OrderSideTagValue);
            sb.Append(Seperators.SEPERATOR_2);//8
            sb.Append(order.Symbol);
            sb.Append(Seperators.SEPERATOR_2);//9
            sb.Append(order.TradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);//10
            sb.Append(order.TransactionTime);
            sb.Append(Seperators.SEPERATOR_2);//11
            sb.Append(order.AUECID);
            sb.Append(Seperators.SEPERATOR_2);//12
            sb.Append(order.AssetID);
            sb.Append(Seperators.SEPERATOR_2);//13
            sb.Append(order.CompanyUserID);
            sb.Append(Seperators.SEPERATOR_2);//14

            str = sb.ToString();
            return str;
        }
        public static Order FromOrderForBasketResponse(string message)
        {
            Order order= new Order();
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                order.MsgType = str[0];
                order.ParentClientOrderID = str[1];
                order.ClientOrderID = str[2];
                order.ClOrderID = str[3];
                order.OrderStatusTagValue = str[4];
                order.ParentClOrderID = str[5];
                order.Quantity = double.Parse(str[6]);
                order.LeavesQty = double.Parse(str[7]);
                if (order.LeavesQty == double.Epsilon)
                {
                    order.LeavesQty = 0.0;
                }
                order.OrderSideTagValue = str[8];
                order.Symbol = str[9];
                order.TradingAccountID = int.Parse(str[10]);
                order.TransactionTime = str[11];
                order.AUECID = int.Parse(str[12]);
                order.AssetID = int.Parse(str[13]);
                order.CompanyUserID = int.Parse(str[14]);
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);

            }

            return order;
        }

        public static string CreateBasketForTrading(BasketDetail basket,OrderCollection selectedOrders)
        {

            StringBuilder strBasketOrder = new StringBuilder();
            //Message Type----------------- First Field
            strBasketOrder.Append(FIXConstants.MSGOrderList);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//0
            strBasketOrder.Append(basket.TradingAccountID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//1
            strBasketOrder.Append(basket.BasketID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//2           
            strBasketOrder.Append(basket.CurrentWaveID);
            strBasketOrder.Append(Seperators.SEPERATOR_2); //3                    
            strBasketOrder.Append(basket.CurrentGroupID);
            strBasketOrder.Append(Seperators.SEPERATOR_2); //4             
            strBasketOrder.Append(basket.UserID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//5

            //Start of Orders
            strBasketOrder.Append("|");
           
            foreach (Order order in selectedOrders)
            {
                    order.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
                   
                    strBasketOrder.Append(CreateOrderSingle(order));
                    strBasketOrder.Append("|");
            }
            return strBasketOrder.ToString();
        }
        public static BasketDetail FromBasketForTrading(string message)
        {
            BasketDetail basket = new BasketDetail();
            try
            {
               
                string[] basketRelatedDetail = message.Split(Seperators.SEPERATOR_2);
                basket.MsgType = basketRelatedDetail[0];
                basket.TradingAccountID = int.Parse(basketRelatedDetail[1]);
                basket.BasketID = basketRelatedDetail[2];
                basket.CurrentWaveID = basketRelatedDetail[3];
                basket.CurrentGroupID =basketRelatedDetail[4];
                basket.UserID = int.Parse(basketRelatedDetail[5]);
                string[] orderCollectionMessage = message.Split('|');
                int count = orderCollectionMessage.Length;
                
                for (int i = 1; i < count - 1; i++)
                {

                    Order order =FromOrderSingle(orderCollectionMessage[i]);
                    order.TradingAccountID = basket.TradingAccountID;
                    order.CompanyUserID = basket.UserID;
                    order.WaveID = basket.CurrentWaveID;
                    basket.BasketOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return basket;
        }

        public static string CreateBasketResponse(BasketDetail basket)
        {
            
            StringBuilder strBasketOrder = new StringBuilder();

            //Message Type----------------- First Field
            strBasketOrder.Append(FIXConstants.MSGOrderList);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//0

            strBasketOrder.Append(basket.TradingAccountID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//1


            strBasketOrder.Append(basket.BasketID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//2


            strBasketOrder.Append(basket.TradedBasketID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//3

            strBasketOrder.Append(basket.CurrentWaveID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//4

            strBasketOrder.Append(basket.CurrentGroupID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);//5

            //Start of Orders
            strBasketOrder.Append("|");
            foreach (Order order in basket.BasketOrders)
            {
                strBasketOrder.Append(CreateOrderForBasketResponse(order));
                strBasketOrder.Append("|");

            }

           
            return strBasketOrder.ToString();

        }
        public static BasketDetail FromBasketResponse(string message)
        {
            BasketDetail basket = new BasketDetail();
            try
            {
                //basket.BasketOrders = new OrderCollection();
                string[] basketRelatedDetail = message.Split(Seperators.SEPERATOR_2);
                basket.MsgType = basketRelatedDetail[0];
                basket.TradingAccountID  = int.Parse(basketRelatedDetail[1]);
                basket.BasketID = basketRelatedDetail[2];
                basket.TradedBasketID = basketRelatedDetail[3];
                basket.CurrentWaveID = basketRelatedDetail[4];
                basket.CurrentGroupID = basketRelatedDetail[5];

                // Order Details
                string[] orderCollectionMessage = message.Split('|');
                int count = orderCollectionMessage.Length;
                for (int i = 1; i < count - 1; i++)
                {
                    Order order = FromOrderForBasketResponse(orderCollectionMessage[i]);
                    basket.BasketOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return basket;

        }

        public static string CreateBasketCancel(BasketDetail basket,OrderCollection selectedOrders)
        {
            // string strBasketOrder = string.Empty;
            StringBuilder strBasketOrder = new StringBuilder();

            //Message Type----------------- First Field

            strBasketOrder.Append(FIXConstants.MSGListCancelRequest);
            strBasketOrder.Append(Seperators.SEPERATOR_2);

            strBasketOrder.Append(basket.TradingAccountID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);

            strBasketOrder.Append(basket.BasketID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);

            strBasketOrder.Append(basket.TradedBasketID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);
            strBasketOrder.Append(basket.CurrentWaveID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);



            //Start of Orders
            strBasketOrder.Append("|");
            // int seqNumber = 1;
            int incompleteOrdCount = 0;
            foreach (Order order in selectedOrders)
            {
                strBasketOrder.Append(CreateCancelRequest(order));
                strBasketOrder.Append("|");
                incompleteOrdCount++;
                
            
            }
            if (incompleteOrdCount == 0)
            {
                strBasketOrder = new StringBuilder();
            }
          
            return strBasketOrder.ToString();
        }
        public static BasketDetail FromBasketCancel(string message)
        {
            BasketDetail basket = new BasketDetail();
            try
            {
                //basket.BasketOrders = new OrderCollection();
                string[] basketRelatedDetail = message.Split(Seperators.SEPERATOR_2);
                basket.MsgType = basketRelatedDetail[0];
                basket.TradingAccountID = int.Parse(basketRelatedDetail[1]);
                basket.BasketID = basketRelatedDetail[2];
                basket.TradedBasketID = basketRelatedDetail[3];
                basket.CurrentWaveID = basketRelatedDetail[4];

                string[] orderCollectionMessage = message.Split('|');
                int count = orderCollectionMessage.Length;

                for (int i = 1; i < count - 1; i++)
                {
                    Order order = FromCancelRequest(orderCollectionMessage[i]);
                    order.TradingAccountID = basket.TradingAccountID;
                    order.ListID = basket.TradedBasketID;
                    order.WaveID = basket.CurrentWaveID;
                    basket.BasketOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return basket;
        }

        public static string CreateBasketReplace(BasketDetail basket, OrderCollection selectedOrders)
        {
            // string strBasketOrder = string.Empty;
            StringBuilder strBasketOrder = new StringBuilder();

            //Message Type----------------- First Field
            strBasketOrder.Append(FIXConstants.MSGListReplace);
            strBasketOrder.Append(Seperators.SEPERATOR_2);
            strBasketOrder.Append(basket.TradingAccountID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);
            strBasketOrder.Append(basket.BasketID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);
            strBasketOrder.Append(basket.TradedBasketID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);

            strBasketOrder.Append(basket.CurrentWaveID);
            strBasketOrder.Append(Seperators.SEPERATOR_2);



            //Start of Orders
            strBasketOrder.Append("|");
            // int seqNumber = 1;
            int incompleteOrdCount = 0;
            foreach (Order order in selectedOrders)
            {
                strBasketOrder.Append(CreateReplaceRequest(order));
                strBasketOrder.Append("|");
                incompleteOrdCount++;


            }
            if (incompleteOrdCount == 0)
            {
                strBasketOrder = new StringBuilder();
            }

            return strBasketOrder.ToString();
        }
        public static BasketDetail FromBasketReplace(string message)
        {
            BasketDetail basket = new BasketDetail();
            try
            {
               // basket.BasketOrders = new OrderCollection();
                string[] basketRelatedDetail = message.Split(Seperators.SEPERATOR_2);
                basket.MsgType = basketRelatedDetail[0];
                basket.TradingAccountID = int.Parse(basketRelatedDetail[1]);
                basket.BasketID = basketRelatedDetail[2];
                basket.TradedBasketID = basketRelatedDetail[3];
                basket.CurrentWaveID = basketRelatedDetail[4];

                string[] orderCollectionMessage = message.Split('|');
                int count = orderCollectionMessage.Length;

                for (int i = 1; i < count - 1; i++)
                {
                    Order order = FromReplaceRequest(orderCollectionMessage[i]);
                    order.TradingAccountID = basket.TradingAccountID;
                    order.ListID = basket.TradedBasketID;
                    order.WaveID = basket.CurrentWaveID;
                    basket.BasketOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return basket;
        }

        /// <summary>
        /// ApplicationConstants method for Getting Basket frim string message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static BasketDetail Getbasket(string message)
        {
            string msgType = GetMessageType(message);

            BasketDetail basket = new BasketDetail();
            switch (PranaMessageFormatter.GetMessageType(message))
            {
                case FIXConstants.MSGOrderList:
                    basket = FromBasketForTrading(message);
                    break;
                case FIXConstants.MSGListReplace:
                    basket = FromBasketReplace(message);
                    break;
                case FIXConstants.MSGListCancelRequest:
                    basket = FromBasketCancel(message);
                    break;
            }
            return basket;
        }

        #endregion

        /// <summary>
        /// Returns Message Type
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public  static string GetMessageType(string message)
        {
            return message.Substring(0, message.IndexOf(Seperators.SEPERATOR_2));

        }
       
        public static string GetSecondIndexField(string msg)
        {
            string secondMsgField = string.Empty;
            int indexofFirstSeperator = msg.IndexOf(Seperators.SEPERATOR_2);
            int length = msg.Length;
            int lengthofSubString = msg.Length - indexofFirstSeperator - 1;
            string firstSubString = msg.Substring(indexofFirstSeperator + 1, lengthofSubString);
            secondMsgField = firstSubString.Substring(0, firstSubString.IndexOf(Seperators.SEPERATOR_2));
            return secondMsgField;
        }
        
        #region Administrative Messages

        public static string CreateLogOnMessage(CompanyUser user)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FIXConstants.MSGLogon);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(user.CompanyUserID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(user.FirstName+" "+user.LastName);

            for (int i = 0; i < user.TradingAccounts.Count; i++)
            {
                sb.Append(Seperators.SEPERATOR_2);
                string tradingacc = ((TradingAccount)user.TradingAccounts[i]).TradingAccountID.ToString();
                sb.Append(tradingacc);
                //if (i != user.TradingAccounts.Count - 1)
                //    sb.Append(Seperators.SEPERATOR_2);

            }
            return sb.ToString();


        }
        public static string CreateLogOnMessage(string identifier,string identifierName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FIXConstants.MSGLogon);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(identifier);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(identifierName);
            sb.Append(Seperators.SEPERATOR_2);
            return sb.ToString();

        }
        public static string CreateHeartBeat(CompanyUser user)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FIXConstants.MSGHeartbeat);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(user.CompanyUserID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(user.FirstName + " " + user.LastName);
           
            return sb.ToString();


        }
        public static string CreateHeartBeatForUser(string userID)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FIXConstants.MSGHeartbeat);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(userID );
           

            return sb.ToString();


        }
        public static string CreateLogOutMessage(CompanyUser user)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FIXConstants.MSGLogout);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(user.CompanyUserID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(user.FirstName + " " + user.LastName);

            
            return sb.ToString();


        }
        public static string CreateLogOutMessageForUser(string userID)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FIXConstants.MSGLogout);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(userID);
            return sb.ToString();
        }
        ///This method is crap an is not loger required :P
        
        ////public static string CreateLogOutMessageForUser(string userID)
        ////{
        ////    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        ////    sb.Append(FIXConstants.MSGLogout);
        ////    sb.Append(Seperators.SEPERATOR_2);
        ////    sb.Append(userID);
        ////    sb.Append(Seperators.SEPERATOR_2);
        ////    sb.Append(userName);
        ////    return sb.ToString();


        ////}
        public static string CreateLogOutMessage(string  userID,string userName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FIXConstants.MSGLogout);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(userID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(userName);


            return sb.ToString();


        }
        public static string CreateCounterPartyStatusReport(int counterPartyID, int status)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PranaMessageConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(counterPartyID.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(status.ToString());
            sb.Append(Seperators.SEPERATOR_2);

            return sb.ToString();
        }
        public static string CreateCounterPartyStatusReqest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PranaMessageConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REQUEST);
            sb.Append(Seperators.SEPERATOR_2);
            return sb.ToString();
        }
        public static string CreateQueqedOrder(Order order, string messageType)
        {
            string message=CreateOrderSingle(order);
            int indexofFirstComma = message.IndexOf(Seperators.SEPERATOR_2);
            message=message.Substring(indexofFirstComma, message.Length - (indexofFirstComma ));
            message = messageType +  message;

            return message;
            
        }
        #endregion
    }
}
