using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;



namespace Prana.CommonDataCache
{
    public class CARulesHelper
    {
        /// <summary>
        /// Fills the account and side information in the taxlots from the current values
        /// </summary>
        /// <param name="positionsCollection"></param>
        /// <returns></returns>
        public static void FillText(TaxlotBase taxlot)
        {
            try
            {
                taxlot.Account = CachedDataManager.GetInstance.GetAccountText(taxlot.Level1ID);
                taxlot.Side = TagDatabaseManager.GetInstance.GetOrderSideText(taxlot.OrderSideTagValue);
                taxlot.Strategy = CachedDataManager.GetInstance.GetStrategyText(taxlot.Level2ID);
                taxlot.Currency = CachedDataManager.GetInstance.GetCurrencyText(taxlot.CurrencyID);
                taxlot.Broker = CachedDataManager.GetInstance.GetCounterPartyText(taxlot.CounterPartyID);
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

        public static void FillDateInfo(TaxlotBase taxlot, DataRow corporateActionRow)
        {
            taxlot.UTCDate = corporateActionRow[CorporateActionConstants.CONST_EffectiveDate].ToString();
            //DateTime date = TimeZoneHelper.GetAUECLocalDateFromUTC(taxlot.AUECID, Convert.ToDateTime(taxlot.UTCDate));
            taxlot.AUECDate = taxlot.UTCDate;
            taxlot.AUECLocalDate = Convert.ToDateTime(taxlot.UTCDate);
        }

        public static DateTime AddDayStartTimeToDate(DateTime date)
        {
            int years = date.Year;
            int months = date.Month;
            int day = date.Day;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            DateTime dateOfCA = new DateTime(years, months, day, hours, minutes, seconds);

            return dateOfCA;
        }

        public static void ResetRowErrors(DataRow firstRow)
        {
            DataColumnCollection columns = firstRow.Table.Columns;
            foreach (DataColumn col in columns)
            {
                firstRow.SetColumnError(col, "");
            }
        }

        public static TaxLot GetTaxlotFromTaxlotBase(TaxlotBase taxlotBase, bool isSplitApply)
        {
            TaxLot taxlot = new TaxLot();
            taxlot.Symbol = taxlotBase.Symbol;
            taxlot.Level1ID = taxlotBase.Level1ID;
            taxlot.Level1Name = taxlotBase.Account;
            taxlot.Level2ID = taxlotBase.Level2ID;
            taxlot.Level2Name = taxlotBase.Strategy;
            taxlot.TaxLotID = taxlotBase.L2TaxlotID;
            taxlot.Quantity = taxlotBase.OpenQty;
            taxlot.GroupID = taxlotBase.GroupID;
            taxlot.ISSwap = false;
            taxlot.PositionTag = taxlotBase.PositionTag;
            taxlot.AUECModifiedDate = taxlotBase.AUECLocalDate;
            taxlot.OpenTotalCommissionandFees = taxlotBase.OpenTotalCommissionandFees;
            taxlot.ClosedTotalCommissionandFees = taxlotBase.ClosedTotalCommissionandFees;
            taxlot.Commission = taxlotBase.OpenTotalCommissionandFees;
            taxlot.CurrencyID = taxlotBase.CurrencyID;
            if (isSplitApply)
            {
                taxlot.TaxLotQty = Convert.ToDouble(taxlotBase.NewTaxlotOpenQty);
                taxlot.AvgPrice = Convert.ToDouble(taxlotBase.NewAvgPrice);
            }
            else
            {
                taxlot.TaxLotQty = taxlotBase.OpenQty;
                taxlot.AvgPrice = taxlotBase.AvgPrice;
            }

            taxlot.IsPosition = true;
            taxlot.UnderlyingID = Convert.ToInt32(taxlotBase.Underlying);
            taxlot.VenueID = taxlotBase.VenueID;
            taxlot.OrderSideTagValue = taxlotBase.OrderSideTagValue;
            taxlot.OrderType = taxlotBase.OrderTypeTagValue;
            taxlot.OrderSide = taxlotBase.Side;
            taxlot.OrderTypeTagValue = taxlotBase.OrderTypeTagValue;

            taxlot.AssetCategoryValue = taxlotBase.AssetCategory;
            taxlot.AUECID = taxlotBase.AUECID;

            if (taxlotBase.UTCDate != null)
                taxlot.TimeOfSaveUTC = Convert.ToDateTime(taxlotBase.UTCDate);
            taxlot.AUECLocalDate = taxlotBase.AUECLocalDate;
            taxlot.OriginalPurchaseDate = taxlotBase.OriginalPurchaseDate;
            taxlot.ProcessDate = taxlotBase.ProcessDate;
            taxlot.FXRate = taxlotBase.FXRate;
            taxlot.FXConversionMethodOperator = taxlotBase.FXConversionMethodOperator;
            taxlot.TransactionType = taxlotBase.TransactionType;

            taxlot.AssetID = (int)taxlotBase.AssetCategory;
            taxlot.ExchangeID = taxlotBase.ExchangeID;
            taxlot.TradeAttribute1 = taxlotBase.TradeAttribute1;
            taxlot.TradeAttribute2 = taxlotBase.TradeAttribute2;
            taxlot.TradeAttribute3 = taxlotBase.TradeAttribute3;
            taxlot.TradeAttribute4 = taxlotBase.TradeAttribute4;
            taxlot.TradeAttribute5 = taxlotBase.TradeAttribute5;
            taxlot.TradeAttribute6 = taxlotBase.TradeAttribute6;
            taxlot.PositionType = taxlotBase.PositionType.ToString();
            taxlot.CounterPartyID = taxlotBase.CounterPartyID;
            taxlot.TradingAccountID = taxlotBase.TradingAccountID;
            taxlot.InternalComments = taxlotBase.InternalComments;
            taxlot.Description = taxlotBase.Description;
            taxlot.ExternalTransId = taxlotBase.ExternalTransId;
            taxlot.CompanyUserID = taxlotBase.UserID;

            return taxlot;
        }
        public static TransactionEntry GetCashDataFromTaxlotBase(TaxlotBase taxlotBase)
        {
            TransactionEntry _cashDividend = new TransactionEntry();

            _cashDividend.CurrencyID = taxlotBase.CurrencyID;
            _cashDividend.AccountID = taxlotBase.Level1ID;
            _cashDividend.TaxLotId = taxlotBase.L2TaxlotID;
            _cashDividend.TaxLotState = ApplicationConstants.TaxLotState.New;
            return _cashDividend;

        }
    }
}
