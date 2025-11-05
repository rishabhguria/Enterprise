using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class FxForwardContractSymbolData
    /// </summary>
    [Serializable]
    public class FxForwardContractSymbolData : FxSymbolData
    {
        /// <summary>
        /// Gets or sets the selected feed price.
        /// </summary>
        /// <value>The selected feed price.</value>
        public override double SelectedFeedPrice
        {
            get
            {
                return (base.SelectedFeedPrice);
            }
            set
            {
                base.SelectedFeedPrice = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FxForwardContractSymbolData"/> class.
        /// </summary>
        public FxForwardContractSymbolData()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FxForwardContractSymbolData"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public FxForwardContractSymbolData(string[] data, ref int i)
            : base(data, ref i)
        {
            //this.ForwardPoints = double.Parse(data[i++]);
            //this.ContractPrice = double.Parse(data[i++]);

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FxForwardContractSymbolData"/> class.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        /// <param name="ConversionMethod">The conversion method.</param>
        public FxForwardContractSymbolData(SymbolData symbolData, Operator ConversionMethod)
        {
            try
            {
                this.ConversionMethod = ConversionMethod;
                CategoryCode = symbolData.CategoryCode;
                AnnualDividend = symbolData.AnnualDividend;
                AskExchange = symbolData.AskExchange;
                AskSize = symbolData.AskSize;
                AUECID = symbolData.AUECID;
                AverageVolume20Day = symbolData.AverageVolume20Day;
                Beta_5yrMonthly = symbolData.Beta_5yrMonthly;

                Ask = symbolData.Ask;
                Bid = symbolData.Bid;
                High = symbolData.High;
                LastPrice = symbolData.LastPrice;
                Low = symbolData.Low;
                Previous = symbolData.Previous;
                SelectedFeedPrice = symbolData.SelectedFeedPrice;
                if (ConversionMethod == Operator.D)
                    base.AdjustPricesBasedOnConversionMethod();
                BidExchange = symbolData.BidExchange;
                BidSize = symbolData.BidSize;
                Change = symbolData.Change;
                CurencyCode = symbolData.CurencyCode;
                CusipNo = symbolData.CusipNo;
                DivDistributionDate = symbolData.DivDistributionDate;
                Dividend = symbolData.Dividend;
                DividendAmtRate = symbolData.DividendAmtRate;
                DividendInterval = symbolData.DividendInterval;
                DividendYield = symbolData.DividendYield;
                ExchangeID = symbolData.ExchangeID;
                FullCompanyName = symbolData.FullCompanyName;
                GapOpen = symbolData.GapOpen;

                IsChangedToHigherCurrency = symbolData.IsChangedToHigherCurrency;
                ISIN = symbolData.ISIN;

                LastTick = symbolData.LastTick;
                ListedExchange = symbolData.ListedExchange;

                Open = symbolData.Open;
                High52W = symbolData.High52W;
                Low52W = symbolData.Low52W;
                PctChange = symbolData.PctChange;

                Spread = symbolData.Spread;
                Symbol = symbolData.Symbol;
                TotalVolume = symbolData.TotalVolume;
                TradeVolume = symbolData.TradeVolume;
                UnderlyingCategory = symbolData.UnderlyingCategory;
                UnderlyingSymbol = symbolData.UnderlyingSymbol;
                UpdateTime = symbolData.UpdateTime;
                Volume10DAvg = symbolData.Volume10DAvg;
                VWAP = symbolData.VWAP;
                XDividendDate = symbolData.XDividendDate;

                MarkPrice = symbolData.MarkPrice;
                MarkPriceStr = symbolData.MarkPriceStr;
                PreferencedPrice = symbolData.PreferencedPrice;
                FinalDividendYield = symbolData.FinalDividendYield;
                StockBorrowCost = symbolData.StockBorrowCost;
                ForwardPoints = symbolData.ForwardPoints;
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
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(_splitter);
            //sb.Append(this.ForwardPoints);
            //sb.Append(_splitter);
            //sb.Append(this.ContractPrice);
            return sb.ToString();
        }
        /// <summary>
        /// Updates the continuous data.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        public override void UpdateContinuousData(SymbolData symbolData)
        {
            base.UpdateContinuousData(symbolData);
            //  base.AdjustPricesBasedOnConversionMethod();

        }
    }
}
