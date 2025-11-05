using System;
using System.IO;
using System.Text;
using Prana.BusinessObjects;

namespace GenericLogging.Utility
{
   public class DataWriter
    {
        // Method to write Data into file
        public static void WriteDataToFile(StreamWriter sw, StringBuilder sb, TaxLot taxlot)
        {
            try
            {
                sb.Append("Published taxlot received with details.");
                sb.Append(" Symbol = " + taxlot.Symbol);
                sb.Append(", Group ID = " + taxlot.GroupID);
                sb.Append(", Taxlot ID = " + taxlot.TaxLotID);
                sb.Append(", Is Manual Order = " + taxlot.IsManualOrder);
                sb.Append(", Average Px = " + taxlot.AvgPrice);
                sb.Append(", Delta = " + taxlot.Delta);
                sb.Append(", Transaction Source = " + taxlot.TransactionSource);
                sb.Append(", Fx Rate = " + taxlot.FXRate);
                sb.Append(", Strike Price = " + taxlot.StrikePrice);
                sb.Append(", Put or Call = " + taxlot.PutOrCall);
                sb.Append(", Master Fund = " + taxlot.MasterFund);
                sb.Append(", TotalCommissionandFees = " + taxlot.TotalCommissionandFees);
                sb.Append(", Is Swapped = " + taxlot.ISSwap);
                sb.Append(", Net Notional Value = " + taxlot.NetNotionalValue);
                sb.Append(", UnRealized PNL = " + taxlot.UnRealizedPNL);
                sb.Append(", Side Multiplier = " + taxlot.SideMultiplier);
                sb.Append(", Long or Short = " + taxlot.LongOrShort);
                sb.Append(", AUEC ID = " + taxlot.AUECID);
                sb.Append(", Asset ID = " + taxlot.AssetID);
                sb.Append(", Underlying ID = " + taxlot.UnderlyingID);
                sb.Append(", Exchange = " + taxlot.ExchangeID);
                sb.Append(", Currency ID = " + taxlot.CurrencyID);
                sb.Append(", Trading Account ID  = " + taxlot.TradingAccountID);
                sb.Append(", Venue ID  = " + taxlot.VenueID);
                sb.Append(", CounterParty ID = " + taxlot.CounterPartyID);
                sb.Append(", Order Side = " + taxlot.OrderSideTagValue);
                sb.Append(", Order Type = " + taxlot.OrderTypeTagValue);
                sb.Append(", Account ID = " + taxlot.Level1ID);
                sb.Append(", Strategy ID = " + taxlot.Level2ID);
                sb.Append(", Quantity = " + taxlot.Quantity);
                sb.Append(", Cum Quantity = " + taxlot.CumQty);
                sb.Append(", TaxLot Quantity = " + taxlot.TaxLotQty);
                sb.Append(", Executed Quantity = " + taxlot.ExecutedQty);
                sb.Append(", Original Cum Quantity = " + taxlot.OriginalCumQty);
                sb.Append(", AUEC Local Date = " + taxlot.AUECLocalDate);
                sb.Append(", AUEC Modified Date = " + taxlot.AUECModifiedDate);
                sb.Append(", Process Date = " + taxlot.ProcessDate);
                sb.Append(", Original Purchase Date = " + taxlot.OriginalPurchaseDate);
                sb.Append(", Closing Date = " + taxlot.ClosingDate);
                sb.Append(", Settlement Date = " + taxlot.SettlementDate);
                sb.Append(", Closing Status = " + taxlot.ClosingStatus);
                sb.Append(", Closing Mode = " + taxlot.ClosingMode);
                sb.Append(", Position Tag = " + taxlot.PositionTag);
                sb.Append(", TaxLot State = " + taxlot.TaxLotState);

                sw.WriteLine(sb.ToString());
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
