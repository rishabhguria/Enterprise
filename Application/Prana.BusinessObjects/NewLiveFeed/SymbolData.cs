using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class SymbolData
    /// </summary>
    [Serializable]
    [KnownType(typeof(CashSymbolData))]
    [KnownType(typeof(EquitySymbolData))]
    [KnownType(typeof(OptionSymbolData))]
    [KnownType(typeof(IndexSymbolData))]
    [KnownType(typeof(FutureSymbolData))]
    [KnownType(typeof(FutureOptionSymbolData))]
    [KnownType(typeof(FxSymbolData))]
    [KnownType(typeof(FxContractSymbolData))]
    [KnownType(typeof(FxForwardContractSymbolData))]
    [KnownType(typeof(FixedIncomeSymbolData))]
    public class SymbolData
    {
        /// <summary>
        /// The _splitter
        /// </summary>
        internal readonly char _splitter = Seperators.SEPERATOR_3;
        /// <summary>
        /// The default delta
        /// </summary>
        // Kuldeep A.: changing it's default value as whenever the underlying symbol/underlying data is not available then it 
        // doesn't calculted and shows its default value as "-2" which is unexplainable.
        //private const double defaultDelta = 0.0;
        /// <summary>
        /// The default implied vol
        /// </summary>
        private const double defaultImpliedVol = -1.0;
        /// <summary>
        /// The default interest rate value
        /// </summary>
        // Kuldeep A.: changing it's default value as whenever the underlying symbol/underlying data is not available then it 
        // doesn't calculted and shows its default value as "-INFINITY".
        private const double defaultInterestRateValue = 0.25;

        #region Common
        public int State = 0;
        /// <summary>
        /// The symbol
        /// </summary>
        private string symbol = string.Empty;
        /// <summary>
        /// The full company name
        /// </summary>
        private string fullCompanyName = string.Empty;
        /// <summary>
        /// The category code
        /// </summary>
        private AssetCategory categoryCode = AssetCategory.None;
        /// <summary>
        /// The pricing source
        /// </summary>
        private PricingSource pricingSource = PricingSource.None;
        /// <summary>
        /// The Delta source
        /// </summary>
        private DeltaSource deltaSource = DeltaSource.Default;
        /// <summary>
        /// The pricing source
        /// </summary>
        private MarketDataProvider marketDataProvider = MarketDataProvider.Esignal;
        /// <summary>
        /// The underlying category
        /// </summary>
        private Underlying underlyingCategory = Underlying.None;
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>

        public virtual string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                if (symbol == value)
                    return;
                symbol = value;
            }
        }

        public string BBGID { get; set; }

        string _sedolSymbol = string.Empty;

        public string SedolSymbol
        {
            get { return _sedolSymbol; }
            set { _sedolSymbol = value; }
        }

        int _countryid = 0;
        public int CountryID
        {
            get { return _countryid; }
            set { _countryid = value; }
        }

        string _reuterSymbol = string.Empty;
        public virtual string ReuterSymbol
        {
            get { return _reuterSymbol; }
            set { _reuterSymbol = value; }
        }

        string _bloombergSymbol = string.Empty;
        public virtual string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set
            {
                if (_bloombergSymbol == value)
                    return;
                _bloombergSymbol = value;
            }
        }

        string _bloombergSymbolWithExchangeCode = string.Empty;
        public virtual string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set
            {
                _bloombergSymbolWithExchangeCode = value;
            }
        }
        string _factSetSymbol = string.Empty;
        public virtual string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }

        string _activSymbol = string.Empty;
        public virtual string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }

        /// <summary>
        /// Gets or sets the full name of the company.
        /// </summary>
        /// <value>The full name of the company.</value>
        public virtual string FullCompanyName
        {
            get
            {
                return fullCompanyName;
            }
            set
            {
                if (fullCompanyName == value)
                    return;
                fullCompanyName = value;
            }
        }
        /// <summary>
        /// Gets or sets the category code.
        /// </summary>
        /// <value>The category code.</value>
        public virtual AssetCategory CategoryCode
        {
            get
            {
                return categoryCode;
            }
            set
            {
                if (categoryCode == value)
                    return;
                categoryCode = value;
            }
        }
        /// <summary>
        /// Gets or sets the pricing source.
        /// </summary>
        /// <value>The pricing source.</value>
        public virtual PricingSource PricingSource
        {
            get
            {
                return pricingSource;
            }
            set
            {
                if (pricingSource == value)
                    return;
                pricingSource = value;
            }
        }
        /// <summary>
        /// Gets or sets the Delta source.
        /// </summary>
        /// <value>The Delta source.</value>
        public virtual DeltaSource DeltaSource
        {
            get
            {
                return deltaSource;
            }
            set
            {
                deltaSource = value;
            }
        }
        /// <summary>
        /// Gets or sets the pricing provider.
        /// </summary>
        /// <value>The pricing provider.</value>
        public MarketDataProvider MarketDataProvider
        {
            get
            {
                return marketDataProvider;
            }
            set
            {
                if (marketDataProvider == value)
                    return;
                marketDataProvider = value;
            }
        }

        /// <summary>
        /// Gets or sets the underlying category.
        /// </summary>
        /// <value>The underlying category.</value>
        public virtual Underlying UnderlyingCategory
        {
            get
            {
                return underlyingCategory;
            }
            set
            {
                if (underlyingCategory == value)
                    return;
                underlyingCategory = value;
            }
        }


        #endregion

        #region Quote (Bid/Ask)
        /// <summary>
        /// The bid price
        /// </summary>
        private double bidPrice = 0;
        /// <summary>
        /// The bid size
        /// </summary>
        private long bidSize = 0;
        /// <summary>
        /// The bid exchange
        /// </summary>
        private string bidExchange = string.Empty;
        /// <summary>
        /// The ask price
        /// </summary>
        private double askPrice = 0;
        /// <summary>
        /// The ask size
        /// </summary>
        private long askSize = 0;
        /// <summary>
        /// The ask exchange
        /// </summary>
        private string askExchange = string.Empty;
        /// <summary>
        /// The mid
        /// </summary>
        private double mid = 0;
        /// <summary>
        /// The i mid
        /// </summary>
        private double imid = 0;
        /// <summary>
        /// The delayed
        /// </summary>
        private PricingStatus _pricingStatus = PricingStatus.None;
        /// <summary>
        /// If symbol is delayed, what is the delay interval?
        /// </summary>
        private string delayInterval = "-";

        /// <summary>
        /// Gets or sets the bid.
        /// </summary>
        /// <value>The bid.</value>
        public virtual double Bid
        {
            get
            {
                return bidPrice;
            }
            set
            {
                if (bidPrice == value)
                    return;
                bidPrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the size of the bid.
        /// </summary>
        /// <value>The size of the bid.</value>
        public virtual long BidSize
        {
            get
            {
                return bidSize;
            }
            set
            {
                if (bidSize == value)
                    return;
                bidSize = value;
            }
        }
        /// <summary>
        /// Gets or sets the bid exchange.
        /// </summary>
        /// <value>The bid exchange.</value>
        public virtual string BidExchange
        {
            get
            {
                return bidExchange;
            }
            set
            {
                if (bidExchange == value)
                    return;
                bidExchange = value;
            }
        }
        /// <summary>
        /// Gets or sets the ask.
        /// </summary>
        /// <value>The ask.</value>
        public virtual double Ask
        {
            get
            {
                return askPrice;
            }
            set
            {
                if (askPrice == value)
                    return;
                askPrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the size of the ask.
        /// </summary>
        /// <value>The size of the ask.</value>
        public virtual long AskSize
        {
            get
            {
                return askSize;
            }
            set
            {
                if (askSize == value)
                    return;
                askSize = value;
            }
        }
        /// <summary>
        /// Gets or sets the ask exchange.
        /// </summary>
        /// <value>The ask exchange.</value>
        public virtual string AskExchange
        {
            get
            {
                return askExchange;
            }
            set
            {
                if (askExchange == value)
                    return;
                askExchange = value;
            }
        }
        /// <summary>
        /// Gets or sets the mid.
        /// </summary>
        /// <value>The mid.</value>
        public virtual double Mid
        {
            get
            {
                double tempMid = (bidPrice + askPrice) / 2.0;
                if (tempMid != 0)
                    return tempMid;
                else
                    return mid;
            }
            set
            {
                mid = value;
            }
        }
        /// <summary>
        /// Gets or sets the I mid.
        /// </summary>
        /// <value>The I mid.</value>
        public virtual double iMid
        {
            get
            {
                double minVal = (bidPrice < askPrice ? bidPrice : askPrice);
                double maxVal = (bidPrice > askPrice ? bidPrice : askPrice);
                if (minVal <= lastPrice && lastPrice <= maxVal)
                    imid = lastPrice;
                else
                    imid = (bidPrice + askPrice) / 2.0;
                return imid;
            }
            set
            {
                imid = value;
            }
        }

        public virtual PricingStatus PricingStatus
        {
            get
            {
                return _pricingStatus;
            }
            set
            {
                if (_pricingStatus == value)
                    return;
                _pricingStatus = value;
            }
        }

        /// <summary>
        /// If symbol is delayed, what is the delay interval?
        /// </summary>
        public virtual string DelayInterval
        {
            get
            {
                return delayInterval;
            }
            set
            {
                if (delayInterval == value)
                    return;
                delayInterval = value;
            }
        }
        #endregion


        #region Equities
        /// <summary>
        /// The market capitalization
        /// </summary>
        private double marketCapitalization = 0;
        /// <summary>
        /// The shares outstanding
        /// </summary>
        private long sharesOutstanding = 0;
        /// <summary>
        /// The dividend
        /// </summary>
        private double dividend = 0;
        /// <summary>
        /// The x dividend date
        /// </summary>
        private DateTime xDividendDate = DateTimeConstants.MinValue;
        /// <summary>
        /// The dividend interval
        /// </summary>
        private long dividendInterval = 0;
        /// <summary>
        /// The dividend amt rate
        /// </summary>
        private float dividendAmtRate = 0;
        /// <summary>
        /// The div distribution date
        /// </summary>
        private DateTime divDistributionDate = DateTimeConstants.MinValue;
        /// <summary>
        /// The annual dividend
        /// </summary>
        private double annualDividend = 0;
        /// <summary>
        /// The dividend yield
        /// </summary>
        private double dividendYield = 0;
        /// <summary>
        /// The final dividend yield
        /// </summary>
        private double finalDividendYield = 0;
        /// <summary>
        /// The stock borrow cost
        /// </summary>
        private double stockBorrowCost = 0;
        /// <summary>
        /// Gets or sets the market capitalization.
        /// </summary>
        /// <value>The market capitalization.</value>

        public virtual double MarketCapitalization
        {
            get
            {
                return marketCapitalization;
            }
            set
            {
                if (marketCapitalization == value)
                    return;
                marketCapitalization = value;
            }
        }



        /// <summary>
        /// Gets or sets the shares outstanding.
        /// </summary>
        /// <value>The shares outstanding.</value>
        public virtual long SharesOutstanding
        {
            get
            {
                return sharesOutstanding;
            }
            set
            {
                if (sharesOutstanding == value)
                    return;
                sharesOutstanding = value;
            }
        }
        /// <summary>
        /// Gets or sets the dividend.
        /// </summary>
        /// <value>The dividend.</value>
        public virtual double Dividend
        {
            get
            {
                return dividend;
            }
            set
            {
                if (dividend == value)
                    return;
                dividend = value;
            }
        }
        /// <summary>
        /// Gets or sets the X dividend date.
        /// </summary>
        /// <value>The X dividend date.</value>
        public virtual DateTime XDividendDate
        {
            get
            {
                return xDividendDate;
            }
            set
            {
                if (xDividendDate == value)
                    return;
                xDividendDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the dividend interval.
        /// </summary>
        /// <value>The dividend interval.</value>
        public virtual long DividendInterval
        {
            get
            {
                return dividendInterval;
            }
            set
            {
                if (dividendInterval == value)
                    return;
                dividendInterval = value;
            }
        }
        /// <summary>
        /// Gets or sets the dividend amt rate.
        /// </summary>
        /// <value>The dividend amt rate.</value>
        public virtual float DividendAmtRate
        {
            get
            {
                return dividendAmtRate;
            }
            set
            {
                if (dividendAmtRate == value)
                    return;
                dividendAmtRate = value;
            }
        }
        /// <summary>
        /// Gets or sets the div distribution date.
        /// </summary>
        /// <value>The div distribution date.</value>
        public virtual DateTime DivDistributionDate
        {
            get
            {
                return divDistributionDate;
            }
            set
            {
                if (divDistributionDate == value)
                    return;
                divDistributionDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the annual dividend.
        /// </summary>
        /// <value>The annual dividend.</value>
        public virtual double AnnualDividend
        {
            get
            {
                return annualDividend;
            }
            set
            {
                if (annualDividend == value)
                    return;
                annualDividend = value;
            }
        }
        /// <summary>
        /// Gets or sets the dividend yield.
        /// </summary>
        /// <value>The dividend yield.</value>
        public double DividendYield
        {
            get
            {
                return dividendYield;
            }
            set
            {
                if (dividendYield == value)
                    return;
                dividendYield = value;
            }
        }
        /// <summary>
        /// Gets or sets the final dividend yield.
        /// </summary>
        /// <value>The final dividend yield.</value>
        public double FinalDividendYield
        {
            get
            {
                return finalDividendYield;
            }
            set
            {
                if (finalDividendYield == value)
                    return;
                finalDividendYield = value;
            }
        }
        /// <summary>
        /// Gets or sets the stock borrow cost.
        /// </summary>
        /// <value>
        /// The stock borrow cost.
        /// </value>
        public double StockBorrowCost
        {
            get
            {
                return stockBorrowCost;
            }
            set
            {
                if (stockBorrowCost == value)
                    return;
                stockBorrowCost = value;
            }
        }
        #endregion

        #region Forex

        /// <summary>
        /// The conversion method
        /// </summary>
        private Operator conversionMethod = Operator.M;
        /// <summary>
        /// The forward points
        /// </summary>
        private double forwardPoints = 0.0;
        /// <summary>
        /// Gets or sets the conversion method.
        /// </summary>
        /// <value>The conversion method.</value>
        public virtual Operator ConversionMethod
        {
            get
            {
                return conversionMethod;
            }
            set
            {
                if (conversionMethod == value)
                    return;
                conversionMethod = value;
            }
        }
        /// <summary>
        /// Gets or sets the forward points.
        /// </summary>
        /// <value>The forward points.</value>
        public virtual double ForwardPoints
        {
            get
            {
                return forwardPoints;
            }
            set
            {
                if (forwardPoints == value)
                    return;
                forwardPoints = value;
            }
        }
        #endregion

        #region Options/Futures
        /// <summary>
        /// The put or call
        /// </summary>
        private OptionType putOrCall = OptionType.NONE;
        /// <summary>
        /// The theoretical price
        /// </summary>
        private double theoreticalPrice = 0;
        /// <summary>
        /// The final delta
        /// </summary>
        //private double finalDelta = 0;
        /// <summary>
        /// The theta
        /// </summary>
        private double theta = 0;
        /// <summary>
        /// The vega
        /// </summary>
        private double vega = 0;
        /// <summary>
        /// The rho
        /// </summary>
        private double rho = 0;
        /// <summary>
        /// The gamma
        /// </summary>
        private double gamma = 0;
        /// <summary>
        /// The expiration date
        /// </summary>
        private DateTime expirationDate = DateTimeConstants.MinValue;
        /// <summary>
        /// The strike price
        /// </summary>
        private double strikePrice = 0;
        /// <summary>
        /// The days to expiration
        /// </summary>
        private int daysToExpiration = 0;
        /// <summary>
        /// The implied vol
        /// </summary>
        private double impliedVol = 0;
        /// <summary>
        /// The final implied vol
        /// </summary>
        private double finalImpliedVol = defaultImpliedVol;
        /// <summary>
        /// The interest rate
        /// </summary>
        private double interestRate = defaultInterestRateValue;
        /// <summary>
        /// The final interest rate
        /// </summary>
        private double finalInterestRate = defaultInterestRateValue;
        /// <summary>
        /// The open interest
        /// </summary>
        private double openInterest = 0;
        /// <summary>
        /// The underlying data
        /// </summary>
        private SymbolData underlyingData;
        /// <summary>
        /// The cfi code
        /// ISO 10962 Classification of Financial Instruments : 6 character code
        /// http://www.onixs.biz/tools/fixdictionary/4.4/app_6_d.html
        /// </summary>
        private string cfiCode = string.Empty;
        /// <summary>
        /// The osi option symbol
        /// </summary>
        private string osiOptionSymbol = string.Empty;
        /// <summary>
        /// The idco option symbol
        /// </summary>
        private string idcoOptionSymbol = string.Empty;
        /// <summary>
        /// The opra symbol
        /// </summary>
        private string opraSymbol = string.Empty;
        /// <summary>
        /// The requested symbology
        /// </summary>
        private Prana.Global.ApplicationConstants.SymbologyCodes requestedSymbology = Prana.Global.ApplicationConstants.SymbologyCodes.TickerSymbol;
        /// <summary>
        /// The Multiplier
        /// </summary>
        private long multiplier;

        /// <summary>
        /// Gets or sets the put or call.
        /// </summary>
        /// <value>The put or call.</value>

        public virtual OptionType PutOrCall
        {
            get
            {
                return putOrCall;
            }
            set
            {
                if (putOrCall == value)
                    return;
                putOrCall = value;
            }
        }
        /// <summary>
        /// Gets or sets the theoretical price.
        /// </summary>
        /// <value>The theoretical price.</value>
        public virtual double TheoreticalPrice
        {
            get
            {
                return theoreticalPrice;
            }
            set
            {
                if (theoreticalPrice == value)
                    return;
                theoreticalPrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the final delta.
        /// </summary>
        /// <value>The final delta.</value>
        //public virtual double FinalDelta
        //{
        //    get
        //    {
        //        return finalDelta;
        //    }
        //    set
        //    {
        //        if (finalDelta == value)
        //            return;
        //        finalDelta = value;
        //    }
        //}
        /// <summary>
        /// Gets or sets the theta.
        /// </summary>
        /// <value>The theta.</value>
        public virtual double Theta
        {
            get
            {
                return theta;
            }
            set
            {
                if (theta == value)
                    return;
                theta = value;
            }
        }
        /// <summary>
        /// Gets or sets the vega.
        /// </summary>
        /// <value>The vega.</value>
        public virtual double Vega
        {
            get
            {
                return vega;
            }
            set
            {
                if (vega == value)
                    return;
                vega = value;
            }
        }
        /// <summary>
        /// Gets or sets the rho.
        /// </summary>
        /// <value>The rho.</value>
        public virtual double Rho
        {
            get
            {
                return rho;
            }
            set
            {
                if (rho == value)
                    return;
                rho = value;
            }
        }
        /// <summary>
        /// Gets or sets the gamma.
        /// </summary>
        /// <value>The gamma.</value>
        public virtual double Gamma
        {
            get
            {
                return gamma;
            }
            set
            {
                if (gamma == value)
                    return;
                gamma = value;
            }
        }
        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>The expiration date.</value>
        public virtual DateTime ExpirationDate
        {
            get
            {
                return expirationDate;
            }
            set
            {
                if (expirationDate == value)
                    return;
                expirationDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the strike price.
        /// </summary>
        /// <value>The strike price.</value>
        public virtual double StrikePrice
        {
            get
            {
                return strikePrice;
            }
            set
            {
                if (strikePrice == value)
                    return;
                strikePrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the days to expiration.
        /// </summary>
        /// <value>The days to expiration.</value>
        public virtual int DaysToExpiration
        {
            get
            {
                return daysToExpiration;
            }
            set
            {
                if (daysToExpiration == value)
                    return;
                daysToExpiration = value;
            }
        }
        /// <summary>
        /// Gets or sets the implied vol.
        /// </summary>
        /// <value>The implied vol.</value>
        public virtual double ImpliedVol
        {
            get
            {
                return impliedVol;
            }
            set
            {
                if (impliedVol == value)
                    return;
                impliedVol = value;
            }
        }
        /// <summary>
        /// Gets or sets the final implied vol.
        /// </summary>
        /// <value>The final implied vol.</value>
        public virtual double FinalImpliedVol
        {
            get
            {
                return finalImpliedVol;
            }
            set
            {
                if (finalImpliedVol == value)
                    return;
                finalImpliedVol = value;
            }
        }
        /// <summary>
        /// Gets or sets the interest rate.
        /// </summary>
        /// <value>The interest rate.</value>
        public virtual double InterestRate
        {
            get
            {
                return interestRate;
            }
            set
            {
                if (interestRate == value)
                    return;
                interestRate = value;
            }
        }
        /// <summary>
        /// Gets or sets the final interest rate.
        /// </summary>
        /// <value>The final interest rate.</value>
        public virtual double FinalInterestRate
        {
            get
            {
                return finalInterestRate;
            }
            set
            {
                if (finalInterestRate == value)
                    return;
                finalInterestRate = value;
            }
        }
        /// <summary>
        /// Gets or sets the open interest.
        /// </summary>
        /// <value>The open interest.</value>
        public virtual double OpenInterest
        {
            get
            {
                return openInterest;
            }
            set
            {
                if (openInterest == value)
                    return;
                openInterest = value;
            }
        }
        /// <summary>
        /// Gets or sets the underlying data.
        /// </summary>
        /// <value>The underlying data.</value>
        public virtual SymbolData UnderlyingData
        {
            get
            {
                return underlyingData;
            }
            set
            {
                if (underlyingData == value)
                    return;
                underlyingData = value;
            }
        }
        /// <summary>
        /// Gets or sets the cfi code.
        /// </summary>
        /// <value>The cfi code.</value>
        public virtual string CFICode
        {
            get
            {
                return cfiCode;
            }
            set
            {
                if (cfiCode == value)
                    return;
                cfiCode = value;
            }
        }
        /// <summary>
        /// Gets or sets the osi option symbol.
        /// </summary>
        /// <value>The osi option symbol.</value>
        public virtual string OSIOptionSymbol
        {
            get
            {
                return osiOptionSymbol;
            }
            set
            {
                if (osiOptionSymbol == value)
                    return;
                osiOptionSymbol = value;
            }
        }
        /// <summary>
        /// Gets or sets the idco option symbol.
        /// </summary>
        /// <value>The idco option symbol.</value>
        public virtual string IDCOOptionSymbol
        {
            get
            {
                return idcoOptionSymbol;
            }
            set
            {
                if (idcoOptionSymbol == value)
                    return;
                idcoOptionSymbol = value;
            }
        }
        /// <summary>
        /// Gets or sets the opra symbol.
        /// </summary>
        /// <value>The opra symbol.</value>
        public virtual string OpraSymbol
        {
            get
            {
                return opraSymbol;
            }
            set
            {
                if (opraSymbol == value)
                    return;
                opraSymbol = value;
            }
        }
        /// <summary>
        /// Gets or sets the requested symbology.
        /// </summary>
        /// <value>The requested symbology.</value>
        public virtual Prana.Global.ApplicationConstants.SymbologyCodes RequestedSymbology
        {
            get
            {
                return requestedSymbology;
            }
            set
            {
                if (requestedSymbology == value)
                    return;
                requestedSymbology = value;
            }
        }

        #endregion

        #region Trades
        /// <summary>
        /// The update time
        /// </summary>
        private DateTime updateTime = DateTimeConstants.MinValue;
        /// <summary>
        /// The last price
        /// </summary>
        private double lastPrice = 0;
        /// <summary>
        /// The last tick
        /// </summary>
        private string lastTick = string.Empty;
        /// <summary>
        /// The trade volume
        /// </summary>
        private long tradeVolume = 0;
        /// <summary>
        /// The change
        /// </summary>
        private double change = 0;
        /// <summary>
        /// The PCT change
        /// </summary>
        private double pctChange = 0;
        /// <summary>
        /// The mark price
        /// </summary>
        private double markPrice = 0;
        /// <summary>
        /// The vwap
        /// </summary>
        private double vwap = 0;
        /// <summary>
        /// The delta
        /// </summary>
        private double delta = 1;
        /// <summary>
        /// The previous
        /// </summary>
        private double previous = 0;
        /// <summary>
        /// The open
        /// </summary>
        private double open = 0;
        /// <summary>
        /// The high 52 w
        /// </summary>
        private double high52W = 0;
        /// <summary>
        /// The low 52 w
        /// </summary>
        private double low52W = 0;
        /// <summary>
        /// The high
        /// </summary>
        private double high = 0;
        /// <summary>
        /// The low
        /// </summary>
        private double low = 0;
        /// <summary>
        /// The total volume
        /// </summary>
        private long totalVolume = 0;
        /// <summary>
        /// The avg volume
        /// </summary>
        private double avgVolume = 0;
        /// <summary>
        /// Gets or sets the update time.
        /// </summary>
        /// <value>The update time.</value>

        public virtual DateTime UpdateTime
        {
            get
            {
                return updateTime;
            }
            set
            {
                if (updateTime == value)
                    return;
                updateTime = value;
            }
        }
        /// <summary>
        /// Gets or sets the last price.
        /// </summary>
        /// <value>The last price.</value>
        public virtual double LastPrice
        {
            get
            {
                return lastPrice;
            }
            set
            {
                if (lastPrice == value)
                    return;
                lastPrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the last tick.
        /// </summary>
        /// <value>The last tick.</value>
        public virtual string LastTick
        {
            get
            {
                return lastTick;
            }
            set
            {
                if (lastTick == value)
                    return;
                lastTick = value;
            }
        }
        /// <summary>
        /// Gets or sets the trade volume.
        /// </summary>
        /// <value>The trade volume.</value>
        public virtual long TradeVolume
        {
            get
            {
                return tradeVolume;
            }
            set
            {
                if (tradeVolume == value)
                    return;
                tradeVolume = value;
            }
        }
        /// <summary>
        /// Gets or sets the change.
        /// </summary>
        /// <value>The change.</value>
        public virtual double Change
        {
            get
            {
                return change;
            }
            set
            {
                if (change == value)
                    return;
                change = value;
            }
        }
        /// <summary>
        /// Gets or sets the PCT change.
        /// </summary>
        /// <value>The PCT change.</value>
        public virtual double PctChange
        {
            get
            {
                return pctChange;
            }
            set
            {
                if (pctChange == value)
                    return;
                pctChange = value;
            }
        }
        /// <summary>
        /// Gets or sets the mark price.
        /// </summary>
        /// <value>The mark price.</value>
        public virtual double MarkPrice
        {
            get
            {
                return markPrice;
            }
            set
            {
                if (markPrice == value)
                    return;
                markPrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the VWAP.
        /// </summary>
        /// <value>The VWAP.</value>
        public virtual double VWAP
        {
            get
            {
                return vwap;
            }
            set
            {
                if (vwap == value)
                    return;
                vwap = value;
            }
        }
        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>The delta.</value>
        public virtual double Delta
        {
            get
            {
                return delta;
            }
            set
            {
                if (delta == value)
                    return;
                delta = value;
            }
        }
        /// <summary>
        /// Gets or sets the previous.
        /// </summary>
        /// <value>The previous.</value>
        public virtual double Previous
        {
            get
            {
                return previous;
            }
            set
            {
                if (previous == value)
                    return;
                previous = value;
            }
        }
        /// <summary>
        /// Gets or sets the open.
        /// </summary>
        /// <value>The open.</value>
        public virtual double Open
        {
            get
            {
                return open;
            }
            set
            {
                if (open == value)
                    return;
                open = value;
            }
        }
        /// <summary>
        /// Gets or sets the high52 w.
        /// </summary>
        /// <value>
        /// The high52 w.
        /// </value>
        public virtual double High52W
        {
            get
            {
                return high52W;
            }
            set
            {
                if (high52W == value)
                    return;
                high52W = value;
            }
        }
        /// <summary>
        /// Gets or sets the low52 w.
        /// </summary>
        /// <value>
        /// The low52 w.
        /// </value>
        public virtual double Low52W
        {
            get
            {
                return low52W;
            }
            set
            {
                if (low52W == value)
                    return;
                low52W = value;
            }
        }
        /// <summary>
        /// Gets or sets the high.
        /// </summary>
        /// <value>The high.</value>
        public virtual double High
        {
            get
            {
                return high;
            }
            set
            {
                if (high == value)
                    return;
                high = value;
            }
        }
        /// <summary>
        /// Gets or sets the low.
        /// </summary>
        /// <value>The low.</value>
        public virtual double Low
        {
            get
            {
                return low;
            }
            set
            {
                if (low == value)
                    return;
                low = value;
            }
        }
        /// <summary>
        /// Gets or sets the total volume.
        /// </summary>
        /// <value>The total volume.</value>
        public virtual long TotalVolume
        {
            get
            {
                return totalVolume;
            }
            set
            {
                if (totalVolume == value)
                    return;
                totalVolume = value;
            }
        }
        /// <summary>
        /// Gets or sets the avg volume.
        /// </summary>
        /// <value>The avg volume.</value>
        public virtual double AvgVolume
        {
            get
            {
                return avgVolume;
            }
            set
            {
                if (avgVolume == value)
                    return;
                avgVolume = value;
            }
        }
        #endregion

        #region Misc
        /// <summary>
        /// The preferenced price
        /// </summary>
        private double preferencedPrice = 0;
        /// <summary>
        /// The auec ID
        /// </summary>
        private int auecID = 0;
        /// <summary>
        /// The exchange ID
        /// </summary>
        private int exchangeID = 0;
        /// <summary>
        /// The cusip no
        /// </summary>
        private string cusipNo = string.Empty;
        /// <summary>
        /// The isin no
        /// </summary>
        private string isinNo = string.Empty;
        /// <summary>
        /// The listed exchange
        /// </summary>
        private string listedExchange = string.Empty;
        /// <summary>
        /// The currency code
        /// </summary>
        private string currencyCode = string.Empty;
        /// <summary>
        /// The average volume20 day
        /// </summary>
        private double averageVolume20Day = 0;
        /// <summary>
        /// The underlying symbol
        /// </summary>
        private string underlyingSymbol = string.Empty;
        /// <summary>
        /// The volume10 D avg
        /// </summary>
        private double volume10DAvg = 0;
        /// <summary>
        /// The beta_5yr monthly
        /// </summary>
        private double beta_5yrMonthly = 0;
        /// <summary>
        /// The spread
        /// </summary>
        private double spread = 0;
        /// <summary>
        /// The gap open
        /// </summary>
        private double gapOpen = 0;
        /// <summary>
        /// The is changed to higher currency
        /// </summary>
        private bool isChangedToHigherCurrency = false;
        /// <summary>
        /// The selected feed price
        /// </summary>
        private double selectedFeedPrice = 0;
        /// <summary>
        /// The mark price STR
        /// </summary>
        private string markPriceStr = string.Empty;

        /// <summary>
        /// Gets or sets the preferenced price.
        /// </summary>
        /// <value>The preferenced price.</value>
        public virtual double PreferencedPrice
        {
            get
            {
                return preferencedPrice;
            }
            set
            {
                if (preferencedPrice == value)
                    return;
                preferencedPrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the AUECID.
        /// </summary>
        /// <value>The AUECID.</value>
        public virtual int AUECID
        {
            get
            {
                return auecID;
            }
            set
            {
                if (auecID == value)
                    return;
                auecID = value;
            }
        }
        /// <summary>
        /// Gets or sets the exchange ID.
        /// </summary>
        /// <value>The exchange ID.</value>
        public virtual int ExchangeID
        {
            get
            {
                return exchangeID;
            }
            set
            {
                if (exchangeID == value)
                    return;
                exchangeID = value;
            }
        }
        /// <summary>
        /// Gets or sets the cusip no.
        /// </summary>
        /// <value>The cusip no.</value>
        public virtual string CusipNo
        {
            get
            {
                return cusipNo;
            }
            set
            {
                if (cusipNo == value)
                    return;
                cusipNo = value;
            }
        }
        /// <summary>
        /// Gets or sets the ISIN.
        /// </summary>
        /// <value>The ISIN.</value>
        public virtual string ISIN
        {
            get
            {
                return isinNo;
            }
            set
            {
                if (isinNo == value)
                    return;
                isinNo = value;
            }
        }
        /// <summary>
        /// Gets or sets the listed exchange.
        /// </summary>
        /// <value>The listed exchange.</value>
        public virtual string ListedExchange
        {
            get
            {
                return listedExchange;
            }
            set
            {
                if (listedExchange == value)
                    return;
                listedExchange = value;
            }
        }
        /// <summary>
        /// Gets or sets the curency code.
        /// </summary>
        /// <value>The curency code.</value>
        public virtual string CurencyCode
        {
            get
            {
                return currencyCode;
            }
            set
            {
                if (currencyCode == value)
                    return;
                currencyCode = value;
            }
        }
        /// <summary>
        /// Gets or sets the average volume20 day.
        /// </summary>
        /// <value>The average volume20 day.</value>
        public virtual double AverageVolume20Day
        {
            get
            {
                return averageVolume20Day;
            }
            set
            {
                if (averageVolume20Day == value)
                    return;
                averageVolume20Day = value;
            }
        }
        /// <summary>
        /// Gets or sets the underlying symbol.
        /// </summary>
        /// <value>The underlying symbol.</value>
        public virtual string UnderlyingSymbol
        {
            get
            {
                return underlyingSymbol;
            }
            set
            {
                if (underlyingSymbol == value)
                    return;
                underlyingSymbol = value;
            }
        }
        /// <summary>
        /// Gets or sets the volume10 D avg.
        /// </summary>
        /// <value>The volume10 D avg.</value>
        public virtual double Volume10DAvg
        {
            get
            {
                return volume10DAvg;
            }
            set
            {
                if (volume10DAvg == value)
                    return;
                volume10DAvg = value;
            }
        }
        /// <summary>
        /// Gets or sets the beta_5yr monthly.
        /// </summary>
        /// <value>The beta_5yr monthly.</value>
        public virtual double Beta_5yrMonthly
        {
            get
            {
                return beta_5yrMonthly;
            }
            set
            {
                if (beta_5yrMonthly == value)
                    return;
                beta_5yrMonthly = value;
            }
        }
        /// <summary>
        /// Gets or sets the spread.
        /// </summary>
        /// <value>The spread.</value>
        public virtual double Spread
        {
            get
            {
                return spread;
            }
            set
            {
                if (spread == value)
                    return;
                spread = value;
            }
        }
        /// <summary>
        /// Gets or sets the gap open.
        /// </summary>
        /// <value>The gap open.</value>
        public virtual double GapOpen
        {
            get
            {
                return gapOpen;
            }
            set
            {
                if (gapOpen == value)
                    return;
                gapOpen = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is changed to higher currency.
        /// </summary>
        /// <value><c>true</c> if this instance is changed to higher currency; otherwise, <c>false</c>.</value>
        public virtual bool IsChangedToHigherCurrency
        {
            get
            {
                return isChangedToHigherCurrency;
            }
            set
            {
                if (isChangedToHigherCurrency == value)
                    return;
                isChangedToHigherCurrency = value;
            }
        }
        /// <summary>
        /// Gets or sets the selected feed price.
        /// </summary>
        /// <value>The selected feed price.</value>
        public virtual double SelectedFeedPrice
        {
            get
            {
                return selectedFeedPrice;
            }
            set
            {
                if (selectedFeedPrice == value)
                    return;
                selectedFeedPrice = value;
            }
        }
        /// <summary>
        /// Gets or sets the mark price STR.
        /// </summary>
        /// <value>The mark price STR.</value>
        public virtual string MarkPriceStr
        {
            get
            {
                return markPriceStr;
            }
            set
            {
                if (markPriceStr == value)
                    return;
                markPriceStr = value;
            }
        }

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>
        /// The multiplier.
        /// </value>
        public virtual long Multiplier
        {
            get
            {
                return multiplier;
            }
            set
            {
                multiplier = value;
            }
        }
        #endregion

        #region Fixed Income

        public AccrualBasis AccrualBasis { get; set; }

        public SecurityType BondType { get; set; }

        /// <summary>
        /// Coupon - decimal value of the annual coupon, e.g., 6.0 means 6%
        /// </summary>
        public double Coupon { get; set; }

        public CouponFrequency Frequency { get; set; }

        /// <summary>
        /// IssueDate - issue date of the security 
        /// </summary>
        public DateTime IssueDate { get; set; } = DateTimeConstants.MinValue;

        /// <summary>
        /// MaturityDate - security maturity date
        /// </summary>
        public DateTime MaturityDate { get; set; } = DateTimeConstants.MinValue;

        /// <summary>
        /// FirstCouponDate - first coupon date from the date of issue 
        /// </summary>
        public DateTime FirstCouponDate { get; set; } = DateTimeConstants.MinValue;

        public int BondTypeID { get; set; }

        public int AccrualBasisID { get; set; }

        public int CouponFrequencyID { get; set; }

        /// <summary>
        /// IsZero - set true if zero coupon bond, can also set coupon above to 0
        /// </summary>
        public bool IsZero { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"/> class.
        /// </summary>
        public SymbolData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public SymbolData(string[] data, ref int i)
        {
            try
            {
                i = 0;
                this.CategoryCode = (AssetCategory)Enum.Parse(typeof(AssetCategory), data[i++]);
                if (Convert.ToDouble(data[i]) != 0.0)
                    this.AnnualDividend = double.Parse(data[i++]);
                else
                {
                    this.AnnualDividend = 0;
                    i++;
                }
                this.Ask = Convert.ToDouble(data[i++]);
                this.AskExchange = data[i++];
                this.AskSize = long.Parse(data[i++]);
                this.AUECID = int.Parse(data[i++]);
                if (Convert.ToDouble(data[i]) != 0.0)
                    this.AverageVolume20Day = double.Parse(data[i++]);
                else
                {
                    this.AverageVolume20Day = 0;
                    i++;
                }
                if (Convert.ToDouble(data[i]) != 0.0)
                    this.Beta_5yrMonthly = double.Parse(data[i++]);
                else
                {
                    this.Beta_5yrMonthly = 0;
                    i++;
                }
                this.Bid = Convert.ToDouble(data[i++]);
                this.BidExchange = data[i++];
                this.BidSize = long.Parse(data[i++]);
                this.Change = Convert.ToDouble(data[i++]);
                this.CurencyCode = data[i++];
                this.CusipNo = data[i++];
                this.DivDistributionDate = DateTime.Parse(data[i++]);
                if (float.Parse(data[i]) != 0.0)
                    this.Dividend = double.Parse(data[i++]);
                else
                {
                    this.Dividend = 0;
                    i++;
                }
                if (float.Parse(data[i]) != 0.0F)
                    this.DividendAmtRate = float.Parse(data[i++]);
                else
                {
                    this.DividendAmtRate = 0;
                    i++;
                }
                if (long.Parse(data[i]) != 0)
                    this.DividendInterval = long.Parse(data[i++]);
                else
                {
                    this.DividendInterval = 0;
                    i++;
                }
                this.DividendYield = float.Parse(data[i++]);
                this.ExchangeID = int.Parse(data[i++]);
                this.FullCompanyName = data[i++];
                this.GapOpen = double.Parse(data[i++]);
                this.High = double.Parse(data[i++]);
                this.iMid = double.Parse(data[i++]);
                this.IsChangedToHigherCurrency = bool.Parse(data[i++]);
                this.ISIN = data[i++];
                this.LastPrice = double.Parse(data[i++]);
                this.LastTick = data[i++];
                this.ListedExchange = data[i++];
                this.Low = double.Parse(data[i++]);
                this.Mid = double.Parse(data[i++]);
                this.Open = double.Parse(data[i++]);
                this.PctChange = double.Parse(data[i++]);
                this.Previous = double.Parse(data[i++]);
                this.Spread = double.Parse(data[i++]);
                this.Symbol = data[i++];
                this.TotalVolume = long.Parse(data[i++]);
                if (long.Parse(data[i]) != 0)
                    this.TradeVolume = long.Parse(data[i++]);
                else
                {
                    this.TradeVolume = 0;
                    i++;
                }
                this.UnderlyingCategory = (Underlying)Enum.Parse(typeof(Underlying), data[i++]);
                this.UnderlyingSymbol = data[i++];
                this.UpdateTime = DateTime.Parse(data[i++]);
                if (double.Parse(data[i]) != 0.0)
                    this.Volume10DAvg = double.Parse(data[i++]);
                else
                {
                    this.Volume10DAvg = 0;
                    i++;
                }
                if (double.Parse(data[i]) != 0.0)
                    this.VWAP = double.Parse(data[i++]);
                else
                {
                    this.VWAP = 0;
                    i++;
                }
                this.XDividendDate = DateTime.Parse(data[i++]);
                this.SelectedFeedPrice = double.Parse(data[i++]);
                this.MarkPrice = double.Parse(data[i++]);
                this.MarkPriceStr = data[i++];
                this.PreferencedPrice = double.Parse(data[i++]);
                this.FinalDividendYield = double.Parse(data[i++]);
                this.StockBorrowCost = double.Parse(data[i++]);
                this.ForwardPoints = double.Parse(data[i++]);
                this.PricingSource = (PricingSource)Enum.Parse(typeof(PricingSource), data[i++]);
                this.MarketDataProvider = (MarketDataProvider)Enum.Parse(typeof(MarketDataProvider), data[i++]);

                this.BloombergSymbol = data[i++];
                this.DeltaSource = (DeltaSource)Enum.Parse(typeof(DeltaSource), data[i++]);
                this.High52W = double.Parse(data[i++]);
                this.Low52W = double.Parse(data[i++]);
                this.RequestedSymbology = (ApplicationConstants.SymbologyCodes)Enum.Parse(typeof(ApplicationConstants.SymbologyCodes), data[i++]);
                this.FactSetSymbol = data[i++];
                this.ActivSymbol = data[i++];
                this.CountryID = int.Parse(data[i++]);
                this.BloombergSymbolWithExchangeCode = data[i++];
                this.Multiplier = long.Parse(data[i++]);
                this.PricingStatus = (PricingStatus)Enum.Parse(typeof(PricingStatus), data[i++]);
                this.DelayInterval = (data[i++]);
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
        /// Updates the continuous data.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        public virtual void UpdateContinuousData(SymbolData symbolData)
        {
            if (symbolData.FullCompanyName != string.Empty)
                this.FullCompanyName = symbolData.FullCompanyName;
            //if (symbolData.Ask != 0.0)
            //{
            this.Ask = symbolData.Ask;
            //}
            //if (symbolData.AskSize != 0)
            //{
            this.AskSize = symbolData.AskSize;
            //}
            //if (symbolData.Bid != 0.0)
            //{
            this.Bid = symbolData.Bid;
            //}
            //if (symbolData.BidSize != 0)
            //{
            this.BidSize = symbolData.BidSize;
            //}
            //if (symbolData.Change != 0.0)
            //{
            this.Change = symbolData.Change;
            //}
            //if (symbolData.High != 0.0)
            //{
            this.High = symbolData.High;
            //}
            //if (symbolData.LastPrice != 0.0)
            //{
            this.LastPrice = symbolData.LastPrice;
            //}

            if (symbolData.LastTick != null)
                this.LastTick = symbolData.LastTick;
            //if (symbolData.Low != 0.0)
            //{
            this.Low = symbolData.Low;
            //}
            //if (symbolData.Open != 0.0)
            //{
            this.Open = symbolData.Open;
            this.High52W = symbolData.High52W;
            this.Low52W = symbolData.Low52W;
            //}
            //if (symbolData.Previous != 0.0)
            //{
            this.Previous = symbolData.Previous;
            //}
            if (!string.IsNullOrEmpty(symbolData.UnderlyingSymbol))
                this.UnderlyingSymbol = symbolData.UnderlyingSymbol;
            //if (symbolData.Dividend != 0.0)
            //{
            this.Dividend = symbolData.Dividend;
            //}
            //if (symbolData.TradeVolume != 0)
            //{
            this.TradeVolume = symbolData.TradeVolume;
            //}

            if (symbolData.UpdateTime != DateTimeConstants.MinValue)
                this.UpdateTime = symbolData.UpdateTime;
            //if (symbolData.TotalVolume != 0)
            //{
            this.TotalVolume = symbolData.TotalVolume;
            //}

            if (!string.IsNullOrEmpty(symbolData.CurencyCode))
            {
                this.CurencyCode = symbolData.CurencyCode;
            }
            // commented this as now currency conversion is handled by App.config file entries.

            // applied this check because sometimes there were multiple responses from throtteling and thus it was being converted to higher currency multiple times.
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-3161
            //if (!symbolData.IsChangedToHigherCurrency)
            //{
            //// in some cases -LON symbols were coming with EURO currency and due to this they were falsely coverted to higher currency. So added this check.
            //if ((!string.IsNullOrEmpty(CurencyCode) && (CurencyCode == "GBX" || CurencyCode == "ZAC"))|| (this.IsChangedToHigherCurrency==true && symbolData.IsChangedToHigherCurrency==false)
            //    || (string.IsNullOrEmpty(CurencyCode) && (Symbol.Contains("-LON") || Symbol.Contains("-JSE") || UnderlyingSymbol.Contains("-LON") || UnderlyingSymbol.Contains("-JSE"))))
            //{
            //    ConvertToHigherCurrency();
            //    symbolData.IsChangedToHigherCurrency = true;
            //}
            //}
            if (symbolData.ListedExchange != string.Empty)
                this.ListedExchange = symbolData.ListedExchange;
            if (symbolData.AverageVolume20Day != 0.0)
                this.AverageVolume20Day = symbolData.AverageVolume20Day;
            //if (symbolData.CategoryCode != AssetCategory.None)
            //{
            //    this.CategoryCode = symbolData.CategoryCode;
            //}

            if (symbolData.BidExchange != string.Empty)
                this.BidExchange = symbolData.BidExchange;
            if (symbolData.AskExchange != string.Empty)
                this.AskExchange = symbolData.AskExchange;
            if (symbolData.Volume10DAvg != 0.0)
                this.Volume10DAvg = symbolData.Volume10DAvg;
            if (symbolData.Beta_5yrMonthly != 0.0)
                this.Beta_5yrMonthly = symbolData.Beta_5yrMonthly;
            if (symbolData.CusipNo != string.Empty)
                this.CusipNo = symbolData.CusipNo;
            if (symbolData.ISIN != string.Empty)
                this.ISIN = symbolData.ISIN;
            if (symbolData.DividendYield != 0.0)
                this.DividendYield = symbolData.DividendYield;
            //if (symbolData.FinalDividendYield != 0.0)
            //{
            this.FinalDividendYield = symbolData.FinalDividendYield;
            this.StockBorrowCost = symbolData.StockBorrowCost;
            //}

            if (symbolData.XDividendDate != DateTimeConstants.MinValue)
                this.XDividendDate = symbolData.XDividendDate;
            //if (symbolData.DividendInterval != 0)
            //{
            this.DividendInterval = symbolData.DividendInterval;
            //}
            //if (symbolData.DividendAmtRate != 0.0F)
            //{
            this.DividendAmtRate = symbolData.DividendAmtRate;
            // }

            if (symbolData.DivDistributionDate != DateTimeConstants.MinValue)
                this.DivDistributionDate = symbolData.DivDistributionDate;
            //if (symbolData.AnnualDividend != 0.0)
            //{
            this.AnnualDividend = symbolData.AnnualDividend;
            //}

            //if (!String.IsNullOrEmpty(symbolData.Symbol))
            //{
            //    this.Symbol = symbolData.Symbol;
            //}

            if (symbolData.AUECID != int.MinValue)
                this.AUECID = symbolData.AUECID;
            if (!String.IsNullOrEmpty(symbolData.CurencyCode))
                this.CurencyCode = symbolData.CurencyCode;
            if (symbolData.ExchangeID != int.MinValue)
                this.ExchangeID = symbolData.ExchangeID;
            if (symbolData.UnderlyingCategory != Underlying.None)
                this.UnderlyingCategory = symbolData.UnderlyingCategory;
            this.ForwardPoints = symbolData.ForwardPoints;

            this.PricingSource = symbolData.PricingSource;
            this.MarketDataProvider = symbolData.MarketDataProvider;
            this.DeltaSource = symbolData.DeltaSource;

            if (symbolData.MarketDataProvider == MarketDataProvider.SAPI)
            {
                this.BloombergSymbol = symbolData.BloombergSymbol;
                this.BloombergSymbolWithExchangeCode = symbolData.BloombergSymbolWithExchangeCode;
            }
            this.Multiplier = symbolData.Multiplier;

            this.PricingStatus = symbolData.PricingStatus;
            this.DelayInterval = symbolData.DelayInterval;
        }

        /// <summary>
        /// Parses the specified symbol data.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        //public void ConvertToHigherCurrency()
        //{
        //    try
        //    {
        //        if (CurencyCode == "GBX")
        //        {
        //            CurencyCode = "GBP";
        //        }
        //        if (CurencyCode == "ZAC")
        //        {
        //            CurencyCode = "ZAR";
        //        }
        //        Ask = Ask / 100;
        //        Bid = Bid / 100;
        //        Change = Change / 100;
        //        Dividend = Dividend / 100;
        //        DividendAmtRate = DividendAmtRate / 100;
        //        High = High / 100;
        //        LastPrice = LastPrice / 100;
        //        Low = Low / 100;
        //        Open = Open / 100;
        //        Previous = Previous / 100;

        //        GapOpen = GapOpen / 100;
        //        Spread = Spread / 100;
        //        this.IsChangedToHigherCurrency = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);
        //        throw;
        //    }
        //}
        public static void Parse(SymbolData symbolData, string[] data, ref int i)
        {

            try
            {
                symbolData.CategoryCode = (AssetCategory)Enum.Parse(typeof(AssetCategory), data[i++]);
                if (Convert.ToDouble(data[i]) != 0.0)
                    symbolData.AnnualDividend = double.Parse(data[i++]);
                else
                {
                    symbolData.AnnualDividend = 0;
                    i++;
                }
                symbolData.Ask = Convert.ToDouble(data[i++]);
                symbolData.AskExchange = data[i++];
                symbolData.AskSize = long.Parse(data[i++]);
                symbolData.AUECID = int.Parse(data[i++]);
                if (Convert.ToDouble(data[i]) != 0.0)
                    symbolData.AverageVolume20Day = double.Parse(data[i++]);
                else
                {
                    symbolData.AverageVolume20Day = 0;
                    i++;
                }
                if (Convert.ToDouble(data[i]) != 0.0)
                    symbolData.Beta_5yrMonthly = double.Parse(data[i++]);
                else
                {
                    symbolData.Beta_5yrMonthly = 0;
                    i++;
                }
                symbolData.Bid = Convert.ToDouble(data[i++]);
                symbolData.BidExchange = data[i++];
                symbolData.BidSize = long.Parse(data[i++]);
                symbolData.Change = Convert.ToDouble(data[i++]);
                symbolData.CurencyCode = data[i++];
                symbolData.CusipNo = data[i++];
                symbolData.DivDistributionDate = DateTime.Parse(data[i++]);
                if (float.Parse(data[i]) != 0.0)
                    symbolData.Dividend = double.Parse(data[i++]);
                else
                {
                    symbolData.Dividend = 0;
                    i++;
                }
                if (float.Parse(data[i]) != 0.0F)
                    symbolData.DividendAmtRate = float.Parse(data[i++]);
                else
                {
                    symbolData.DividendAmtRate = 0;
                    i++;
                }
                if (long.Parse(data[i]) != 0)
                    symbolData.DividendInterval = long.Parse(data[i++]);
                else
                {
                    symbolData.DividendInterval = 0;
                    i++;
                }
                symbolData.DividendYield = float.Parse(data[i++]);
                symbolData.ExchangeID = int.Parse(data[i++]);
                symbolData.FullCompanyName = data[i++];
                symbolData.GapOpen = double.Parse(data[i++]);
                symbolData.High = double.Parse(data[i++]);
                symbolData.iMid = double.Parse(data[i++]);
                symbolData.IsChangedToHigherCurrency = bool.Parse(data[i++]);
                symbolData.ISIN = data[i++];
                symbolData.LastPrice = double.Parse(data[i++]);
                symbolData.LastTick = data[i++];
                symbolData.ListedExchange = data[i++];
                symbolData.Low = double.Parse(data[i++]);
                symbolData.Mid = double.Parse(data[i++]);
                symbolData.Open = double.Parse(data[i++]);
                symbolData.PctChange = double.Parse(data[i++]);
                symbolData.Previous = double.Parse(data[i++]);
                symbolData.Spread = double.Parse(data[i++]);
                symbolData.Symbol = data[i++];
                symbolData.TotalVolume = long.Parse(data[i++]);
                if (long.Parse(data[i]) != 0)
                    symbolData.TradeVolume = long.Parse(data[i++]);
                else
                {
                    symbolData.TradeVolume = 0;
                    i++;
                }
                symbolData.UnderlyingCategory = (Underlying)Enum.Parse(typeof(Underlying), data[i++]);
                symbolData.UnderlyingSymbol = data[i++];
                symbolData.UpdateTime = DateTime.Parse(data[i++]);
                if (double.Parse(data[i]) != 0.0)
                    symbolData.Volume10DAvg = double.Parse(data[i++]);
                else
                {
                    symbolData.Volume10DAvg = 0;
                    i++;
                }
                if (double.Parse(data[i]) != 0.0)
                    symbolData.VWAP = double.Parse(data[i++]);
                else
                {
                    symbolData.VWAP = 0;
                    i++;
                }
                symbolData.XDividendDate = DateTime.Parse(data[i++]);
                symbolData.SelectedFeedPrice = double.Parse(data[i++]);
                symbolData.MarkPrice = double.Parse(data[i++]);
                symbolData.MarkPriceStr = (data[i++]);
                symbolData.PreferencedPrice = double.Parse(data[i++]);
                symbolData.FinalDividendYield = double.Parse(data[i++]);
                symbolData.StockBorrowCost = double.Parse(data[i++]);
                symbolData.ForwardPoints = double.Parse(data[i++]);
                symbolData.PricingSource = (PricingSource)Enum.Parse(typeof(PricingSource), data[i++]);
                symbolData.MarketDataProvider = (MarketDataProvider)Enum.Parse(typeof(MarketDataProvider), data[i++]);
                symbolData.DeltaSource = (DeltaSource)Enum.Parse(typeof(DeltaSource), data[i++]);
                symbolData.High52W = double.Parse(data[i++]);
                symbolData.Low52W = double.Parse(data[i++]);
                symbolData.FactSetSymbol = data[i++];
                symbolData.ActivSymbol = data[i++];
                symbolData.CountryID = int.Parse(data[i++]);
                symbolData.BloombergSymbolWithExchangeCode = data[i++];
                symbolData.Multiplier = long.Parse(data[i++]);
                symbolData.PricingStatus = (PricingStatus)Enum.Parse(typeof(PricingStatus), data[i++]);
                symbolData.DelayInterval = (data[i++]);
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
        /// Adjusts the selected feed price.
        /// </summary>
        public void AdjustSelectedFeedPrice()
        {
            if (ForwardPoints != double.MinValue)
                SelectedFeedPrice = SelectedFeedPrice + ForwardPoints;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }

        /// <summary>
        /// Returns a <see cref="System.String"></see> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"></see> that represents this instance.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.CategoryCode.ToString());
            sb.Append(_splitter);
            sb.Append(this.AnnualDividend.ToString());
            sb.Append(_splitter);
            sb.Append(this.Ask.ToString());
            sb.Append(_splitter);
            sb.Append(this.AskExchange);
            sb.Append(_splitter);
            sb.Append(this.AskSize.ToString());
            sb.Append(_splitter);
            sb.Append(this.AUECID.ToString());
            sb.Append(_splitter);
            sb.Append(this.AverageVolume20Day.ToString());
            sb.Append(_splitter);
            sb.Append(this.Beta_5yrMonthly.ToString());
            sb.Append(_splitter);
            sb.Append(this.Bid.ToString());
            sb.Append(_splitter);
            sb.Append(this.BidExchange);
            sb.Append(_splitter);
            sb.Append(this.BidSize.ToString());
            sb.Append(_splitter);
            sb.Append(this.Change.ToString());
            sb.Append(_splitter);
            sb.Append(this.CurencyCode);
            sb.Append(_splitter);
            sb.Append(this.CusipNo);
            sb.Append(_splitter);
            sb.Append(this.DivDistributionDate.ToString());
            sb.Append(_splitter);
            sb.Append(this.Dividend.ToString());
            sb.Append(_splitter);
            sb.Append(this.DividendAmtRate.ToString());
            sb.Append(_splitter);
            sb.Append(this.DividendInterval.ToString());
            sb.Append(_splitter);
            sb.Append(this.DividendYield.ToString());
            sb.Append(_splitter);
            sb.Append(this.ExchangeID.ToString());
            sb.Append(_splitter);
            sb.Append(this.FullCompanyName);
            sb.Append(_splitter);
            sb.Append(this.GapOpen.ToString());
            sb.Append(_splitter);
            sb.Append(this.High.ToString());
            sb.Append(_splitter);
            sb.Append(this.iMid.ToString());
            sb.Append(_splitter);
            sb.Append(this.IsChangedToHigherCurrency.ToString());
            sb.Append(_splitter);
            sb.Append(this.ISIN);
            sb.Append(_splitter);
            sb.Append(this.LastPrice.ToString());
            sb.Append(_splitter);
            sb.Append(this.LastTick);
            sb.Append(_splitter);
            sb.Append(this.ListedExchange);
            sb.Append(_splitter);
            sb.Append(this.Low.ToString());
            sb.Append(_splitter);
            sb.Append(this.Mid.ToString());
            sb.Append(_splitter);
            sb.Append(this.Open.ToString());
            sb.Append(_splitter);
            sb.Append(this.PctChange.ToString());
            sb.Append(_splitter);
            sb.Append(this.Previous.ToString());
            sb.Append(_splitter);
            sb.Append(this.Spread.ToString());
            sb.Append(_splitter);
            sb.Append(this.Symbol);
            sb.Append(_splitter);
            sb.Append(this.TotalVolume.ToString());
            sb.Append(_splitter);
            sb.Append(this.TradeVolume.ToString());
            sb.Append(_splitter);
            sb.Append(this.UnderlyingCategory.ToString());
            sb.Append(_splitter);
            sb.Append(this.UnderlyingSymbol);
            sb.Append(_splitter);
            sb.Append(this.UpdateTime.ToString());
            sb.Append(_splitter);
            sb.Append(this.Volume10DAvg.ToString());
            sb.Append(_splitter);
            sb.Append(this.VWAP.ToString());
            sb.Append(_splitter);
            sb.Append(this.XDividendDate.ToString());
            sb.Append(_splitter);
            sb.Append(this.SelectedFeedPrice.ToString());
            sb.Append(_splitter);
            sb.Append(this.MarkPrice.ToString());
            sb.Append(_splitter);
            sb.Append(this.MarkPriceStr.ToString());
            sb.Append(_splitter);
            sb.Append(this.PreferencedPrice);
            sb.Append(_splitter);
            sb.Append(this.FinalDividendYield);
            sb.Append(_splitter);
            sb.Append(this.StockBorrowCost);
            sb.Append(_splitter);
            sb.Append(this.ForwardPoints);
            sb.Append(_splitter);
            sb.Append(this.PricingSource);
            sb.Append(_splitter);
            sb.Append(this.MarketDataProvider);
            sb.Append(_splitter);
            sb.Append(this.BloombergSymbol);
            sb.Append(_splitter);
            sb.Append(this.DeltaSource);
            sb.Append(_splitter);
            sb.Append(this.High52W.ToString());
            sb.Append(_splitter);
            sb.Append(this.Low52W.ToString());
            sb.Append(_splitter);
            sb.Append(this.RequestedSymbology.ToString());
            sb.Append(_splitter);
            sb.Append(this.FactSetSymbol);
            sb.Append(_splitter);
            sb.Append(this.ActivSymbol);
            sb.Append(_splitter);
            sb.Append(this.CountryID.ToString());
            sb.Append(_splitter);
            sb.Append(this.BloombergSymbolWithExchangeCode);
            sb.Append(_splitter);
            sb.Append(this.Multiplier.ToString());
            sb.Append(_splitter);
            sb.Append(this.PricingStatus);
            sb.Append(_splitter);
            sb.Append(this.DelayInterval);
            return sb.ToString();
        }
    }
}
