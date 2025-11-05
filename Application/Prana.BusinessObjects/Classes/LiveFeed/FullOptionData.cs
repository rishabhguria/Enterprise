using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;


namespace Prana.BusinessObjects.LiveFeed
{
    /// <summary>
    /// This class contains the Full option realted data for a given month
    /// and some extra data as well. We will bind the collection of this class to the DataGrid
    /// </summary>
    [Serializable]
    public class FullOptionData : INotifyPropertyChanged //  : System.Collections.CollectionBase
    {
        bool _isObjectChanged = false;

        #region Commented Event Declarations for propertyChanged 

        #region Put

        //		public event EventHandler PSymbolChanged ;
        //		public event EventHandler PBidPriceChanged ;
        //		public event EventHandler PBidSizeChanged ;
        //		public event EventHandler PExchangeBidChanged ;
        //		public event EventHandler PAskPriceChanged ;
        //		public event EventHandler PAskSizeChanged ;
        //		public event EventHandler PExchangeAskChanged ;
        //		public event EventHandler PChangeChanged ;
        //		public event EventHandler PLastChanged ;
        //		public event EventHandler PHighChanged ;
        //		public event EventHandler PLowChanged ;
        //		public event EventHandler PVolumeChanged ;
        //		public event EventHandler POpenChanged;
        //		public event EventHandler PPreviousChanged ;

        #endregion

        //		StrikePrice and ExpirationMonth change doesn't matter much 
        //		public event EventHandler StrikePriceChanged;
        //		public event EventHandler ExpirationMonthChanged;

        #region Call

        //		public event EventHandler CSymbolChanged ;
        //		public event EventHandler CBidPriceChanged ;
        //		public event EventHandler CBidSizeChanged ;
        //		public event EventHandler CExchangeBidChanged ;
        //		public event EventHandler CAskPriceChanged ;
        //		public event EventHandler CAskSizeChanged ;
        //		public event EventHandler CExchangeAskChanged ;
        //		public event EventHandler CChangeChanged ;
        //		public event EventHandler CLastChanged ;
        //		public event EventHandler CHighChanged ;
        //		public event EventHandler CLowChanged ;
        //		public event EventHandler CVolumeChanged ;
        //		public event EventHandler COpenChanged;
        //		public event EventHandler CPreviousChanged ;

        #endregion

        #endregion

        public FullOptionDataCollection parent;

        public FullOptionData()
        {
        }

        /// <summary>
        /// We will not show this PutStruct, Instead we will assign it into individual properties
        /// </summary>

        OptionsUpdateStruct _putStruct;
        [BrowsableAttribute(false)]	///This attribute make the column invisible while binding to grid
		public OptionsUpdateStruct PutStruct
        {
            get
            {
                return _putStruct;
            }
            set
            {
                _putStruct = value;

                #region Assign Put Struct Properties

                if (_putStruct != null)
                {
                    P_symbol = _putStruct.Symbol;
                    P_bidPrice = _putStruct.BidPrice;
                    P_bidSize = _putStruct.BidSize;
                    P_exchangeBid = _putStruct.ExchangeBid;
                    P_askPrice = _putStruct.AskPrice;
                    P_askSize = _putStruct.AskSize;
                    P_exchangeAsk = _putStruct.ExchangeAsk;
                    P_exchangeListed = _putStruct.ExchangeListed;
                    P_change = _putStruct.Change;
                    P_last = _putStruct.Last;
                    P_high = _putStruct.High;
                    P_low = _putStruct.Low;
                    P_volume = _putStruct.Volume;
                    P_open = _putStruct.Open;
                    P_previous = _putStruct.Previous;
                    P_categoryCode = _putStruct.AssetCategoryCode;
                    P_updateTime = _putStruct.UpdateTime;
                }

                #endregion

                _isObjectChanged = true;
            }
        }

        #region Put Struct properties

        string P_symbol = string.Empty;
        public string PSymbol
        {
            get { return P_symbol; }
            set
            {
                P_symbol = value;
                //				PSymbolChanged(this,EventArgs.Empty);
            }
        }

        double P_bidPrice = 0.0;
        public double PBidPrice
        {
            get { return P_bidPrice; }
            set
            {
                P_bidPrice = value;
                //				PBidPriceChanged(this,EventArgs.Empty);
            }
        }

        long P_bidSize = 0;
        public long PBidSize
        {
            get { return P_bidSize; }
            set
            {
                P_bidSize = value;
                //				PBidSizeChanged(this,EventArgs.Empty);
            }
        }

        string P_exchangeBid = string.Empty;
        public string PExchangeBid
        {
            get { return P_exchangeBid; }
            set
            {
                P_exchangeBid = value;
                //				PExchangeBidChanged(this,EventArgs.Empty);
            }
        }

        double P_askPrice = 0.0;
        public double PAskPrice
        {
            get { return P_askPrice; }
            set
            {
                P_askPrice = value;
                //				PAskPriceChanged(this,EventArgs.Empty);
            }
        }

        //double P_midPrice = 0.0;
        public double PMidPrice
        {
            get { return (P_askPrice + P_bidPrice) / 2.0; }
        }

        //double P_imidPrice = 0.0;
        public double PiMidPrice
        {
            get
            {
                double minVal = (P_bidPrice < P_askPrice ? P_bidPrice : P_askPrice);
                double maxVal = (P_bidPrice > P_askPrice ? P_bidPrice : P_askPrice);
                if (minVal <= P_last && P_last <= maxVal)
                {
                    return P_last;
                }
                else
                {
                    return (P_askPrice + P_bidPrice) / 2.0;
                }
            }
        }

        long P_askSize = 0;
        public long PAskSize
        {
            get { return P_askSize; }
            set
            {
                P_askSize = value;
                //				PAskSizeChanged(this,EventArgs.Empty);
            }
        }

        string P_exchangeAsk = string.Empty;
        public string PExchangeAsk
        {
            get { return P_exchangeAsk; }
            set
            {
                P_exchangeAsk = value;
                //				PExchangeAskChanged(this,EventArgs.Empty);
            }
        }

        private string P_exchangeListed = string.Empty;

        public string PExchangeListed
        {
            get { return P_exchangeListed; }
            set
            {
                P_exchangeListed = value;
            }
        }



        double P_change = 0.0;
        public double PChange
        {
            get { return P_change; }
            set
            {
                P_change = value;
                //				PChangeChanged(this,EventArgs.Empty);
            }
        }

        double P_last = 0.0;
        public double PLast
        {
            get { return P_last; }
            set
            {
                P_last = value;
                //				PLastChanged(this,EventArgs.Empty);
            }
        }

        double P_high = 0.0;
        public double PHigh
        {
            get { return P_high; }
            set
            {
                P_high = value;
                //				PHighChanged(this,EventArgs.Empty);
            }
        }

        double P_low = 0.0;
        public double PLow
        {
            get { return P_low; }
            set
            {
                P_low = value;
                //				PLowChanged(this,EventArgs.Empty);
            }
        }

        long P_volume = 0;      // (Cumulative)
        public long PVolume // (Cumulative)
        {
            get { return P_volume; }
            set
            {
                P_volume = value;
                //				PVolumeChanged(this,EventArgs.Empty);
            }
        }

        double P_open = 0.0;
        public double POpen
        {
            get { return P_open; }
            set
            {
                P_open = value;
                //				POpenChanged(this,EventArgs.Empty);
            }
        }

        double P_previous = 0.0;
        public double PPrevious //Same as close
        {
            get { return P_previous; }
            set
            {
                P_previous = value;
                //				PPreviousChanged(this,EventArgs.Empty);
            }
        }

        // TODO :  need to remove [Browsable(false)], comment after this release
        private double P_delta = 0.0;
        [Browsable(false)]
        public double PDelta
        {
            get { return P_delta; }
            set
            {
                P_delta = value;
            }

        }

        private double P_theta = 0.0;
        [Browsable(false)]
        public double PTheta
        {
            get { return P_theta; }
            set
            {
                P_theta = value;
            }
        }

        private double P_vega = 0.0;
        [Browsable(false)]
        public double PVega
        {
            get { return P_vega; }
            set
            {
                P_vega = value;
            }
        }

        private double P_rho = 0.0;
        [Browsable(false)]
        public double PRho
        {
            get { return P_rho; }
            set
            {
                P_rho = value;
            }
        }

        private double P_gamma = 0.0;
        [Browsable(false)]
        public double PGamma
        {
            get { return P_gamma; }
            set
            {
                P_gamma = value;
            }
        }

        private double P_impliedVol = 0.0;
        [Browsable(false)]
        public double PImpliedVol
        {
            get { return P_impliedVol; }
            set
            {
                P_impliedVol = value;
            }
        }

        // added by sandeep on 17/07/07
        AssetCategory P_categoryCode = AssetCategory.None;
        public AssetCategory PCategoryCode
        {
            get { return P_categoryCode; }
            set { P_categoryCode = value; }
        }

        DateTime P_updateTime = DateTimeConstants.MinValue;
        public DateTime PUpdateTime
        {
            get { return P_updateTime; }
            set { P_updateTime = value; }
        }

        #endregion



        private string _expirationDate = string.Empty;

        public string ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                _expirationDate = value;
            }
        }

        string _expirationMonth = string.Empty;
        public string ExpirationMonth
        {
            get { return _expirationMonth; }
            set
            {
                _expirationMonth = value;
                //				ExpirationMonthChanged(this,EventArgs.Empty);
            }
        }

        double _strikePrice = 0.0;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set
            {
                _strikePrice = value;
                //				StrikePriceChanged(this,EventArgs.Empty);
            }
        }

        /// <summary>
        /// This property is used to sort the data in the bindinglist.
        /// </summary>
        private string _sortingKey;
        [Browsable(false)]
        public string SortingKey
        {
            get { return _sortingKey; }
            set { _sortingKey = value; }
        }

        private int _daysToExpiration;
        [Browsable(false)]
        public int DaysToExpiration
        {
            get { return _daysToExpiration; }
            set { _daysToExpiration = value; }
        }

        private string _underlyingSymbol;
        [Browsable(false)]
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        #region Call Struct properties

        string C_symbol = string.Empty;
        public string CSymbol
        {
            get { return C_symbol; }
            set
            {
                C_symbol = value;
                //				CSymbolChanged(this,EventArgs.Empty);				
            }
        }

        double C_bidPrice = 0.0;
        public double CBidPrice
        {
            get { return C_bidPrice; }
            set
            {
                C_bidPrice = value;
                //				CBidPriceChanged(this,EventArgs.Empty);				
            }
        }

        long C_bidSize = 0;
        public long CBidSize
        {
            get { return C_bidSize; }
            set
            {
                C_bidSize = value;
                //				CBidSizeChanged(this,EventArgs.Empty);				
            }
        }

        string C_exchangeBid = string.Empty;
        public string CExchangeBid
        {
            get { return C_exchangeBid; }
            set
            {
                C_exchangeBid = value;
                //				CExchangeBidChanged(this,EventArgs.Empty);				
            }
        }

        double C_askPrice = 0.0;
        public double CAskPrice
        {
            get { return C_askPrice; }
            set
            {
                C_askPrice = value;
                //				CAskPriceChanged(this,EventArgs.Empty);				
            }
        }

        //double C_midPrice = 0.0;
        public double CMidPrice
        {
            get { return (C_askPrice + C_bidPrice) / 2.0; }
        }

        //double C_imidPrice = 0.0;
        public double CiMidPrice
        {
            get
            {
                double minVal = (C_bidPrice < C_askPrice ? C_bidPrice : C_askPrice);
                double maxVal = (C_bidPrice > C_askPrice ? C_bidPrice : C_askPrice);
                if (minVal <= C_last && C_last <= maxVal)
                {
                    return C_last;
                }
                else
                {
                    return (C_askPrice + C_bidPrice) / 2.0;
                }
            }
        }

        long C_askSize = 0;
        public long CAskSize
        {
            get { return C_askSize; }
            set
            {
                C_askSize = value;
                //				CAskSizeChanged(this,EventArgs.Empty);				
            }
        }

        string C_exchangeAsk = string.Empty;
        public string CExchangeAsk
        {
            get { return C_exchangeAsk; }
            set
            {
                C_exchangeAsk = value;
                //				CExchangeAskChanged(this,EventArgs.Empty);				
            }
        }

        private string C_exchangeListed = string.Empty;

        public string CExchangeListed
        {
            get { return C_exchangeListed; }
            set
            {
                C_exchangeListed = value;
            }
        }

        double C_change = 0.0;
        public double CChange
        {
            get { return C_change; }
            set
            {
                C_change = value;
                //				CChangeChanged(this,EventArgs.Empty);				
            }
        }

        double C_last = 0.0;
        public double CLast
        {
            get { return C_last; }
            set
            {
                C_last = value;
                //				CLastChanged(this,EventArgs.Empty);				
            }
        }

        double C_high = 0.0;
        public double CHigh
        {
            get { return C_high; }
            set
            {
                C_high = value;
                //				CHighChanged(this,EventArgs.Empty);				
            }
        }

        double C_low = 0.0;
        public double CLow
        {
            get { return C_low; }
            set
            {
                C_low = value;
                //				CLowChanged(this,EventArgs.Empty);				
            }
        }

        long C_volume = 0;      // (Cumulative)
        public long CVolume // (Cumulative)
        {
            get { return C_volume; }
            set
            {
                C_volume = value;
                //				CVolumeChanged(this,EventArgs.Empty);				
            }
        }

        double C_open = 0.0;
        public double COpen
        {
            get { return C_open; }
            set
            {
                C_open = value;
                //				COpenChanged(this,EventArgs.Empty);				
            }
        }

        double C_previous = 0.0;
        public double CPrevious //Same as close
        {
            get { return C_previous; }
            set
            {
                C_previous = value;
                //				CPreviousChanged(this,EventArgs.Empty);				
            }
        }

        // TODO :  need to remove [Browsable(false)], comment after this release
        private double C_delta = 0.0;
        [Browsable(false)]
        public double CDelta
        {
            get { return C_delta; }
            set
            {
                C_delta = value;
            }
        }

        private double C_theta = 0.0;
        [Browsable(false)]
        public double CTheta
        {
            get { return C_theta; }
            set
            {
                C_theta = value;
            }
        }

        private double C_vega = 0.0;
        [Browsable(false)]
        public double CVega
        {
            get { return C_vega; }
            set
            {
                C_vega = value;
            }
        }

        private double C_rho = 0.0;
        [Browsable(false)]
        public double CRho
        {
            get { return C_rho; }
            set
            {
                C_rho = value;
            }
        }

        private double C_gamma = 0.0;
        [Browsable(false)]
        public double CGamma
        {
            get { return C_gamma; }
            set
            {
                C_gamma = value;
            }
        }

        private double C_impliedVol = 0.0;
        [Browsable(false)]
        public double CImpliedVol
        {
            get { return C_impliedVol; }
            set
            {
                C_impliedVol = value;

            }
        }

        // added by sandeep on 17/07/07
        AssetCategory C_categoryCode = AssetCategory.None;
        public AssetCategory CCategoryCode
        {
            get { return C_categoryCode; }
            set { C_categoryCode = value; }
        }

        DateTime C_updateTime = DateTimeConstants.MinValue;
        public DateTime CUpdateTime
        {
            get { return C_updateTime; }
            set { C_updateTime = value; }
        }
        #endregion

        /// <summary>
        /// We will not show this PutStruct, Instead we will assign it into individual properties
        /// </summary>

        OptionsUpdateStruct _callStruct;
        [BrowsableAttribute(false)]   ///This attribute make the column invisible while binding to grid
		public OptionsUpdateStruct CallStruct
        {
            get
            {
                return _callStruct;
            }
            set
            {
                _callStruct = value;

                #region Assign Call Struct Properties

                if (_callStruct != null)
                {
                    C_symbol = _callStruct.Symbol;
                    C_bidPrice = _callStruct.BidPrice;
                    C_bidSize = _callStruct.BidSize;
                    C_exchangeBid = _callStruct.ExchangeBid;
                    C_askPrice = _callStruct.AskPrice;
                    C_askSize = _callStruct.AskSize;
                    C_exchangeAsk = _callStruct.ExchangeAsk;
                    C_exchangeListed = _callStruct.ExchangeListed;
                    C_change = _callStruct.Change;
                    C_last = _callStruct.Last;
                    C_high = _callStruct.High;
                    C_low = _callStruct.Low;
                    C_volume = _callStruct.Volume;
                    C_open = _callStruct.Open;
                    C_previous = _callStruct.Previous;
                    C_categoryCode = _callStruct.AssetCategoryCode;
                    C_updateTime = _callStruct.UpdateTime;
                }


                #endregion

                _isObjectChanged = true;
            }
        }

        public void ClearPrices()
        {
            ClearCallStruct();
            ClearPutStruct();
        }

        private void ClearCallStruct()
        {
            C_bidPrice = 0;
            C_bidSize = 0;
            C_exchangeBid = string.Empty;
            C_askPrice = 0;
            C_askSize = 0;
            C_exchangeAsk = string.Empty;
            C_exchangeListed = string.Empty;
            C_change = 0;
            C_last = 0;
            C_high = 0;
            C_low = 0;
            C_volume = 0;
            C_open = 0;
            C_previous = 0;
            C_categoryCode = AssetCategory.None;
            C_updateTime = DateTimeConstants.MinValue;
        }

        private void ClearPutStruct()
        {
            P_bidPrice = 0;
            P_bidSize = 0;
            P_exchangeBid = string.Empty;
            P_askPrice = 0;
            P_askSize = 0;
            P_exchangeAsk = string.Empty;
            P_exchangeListed = string.Empty;
            P_change = 0;
            P_last = 0;
            P_high = 0;
            P_low = 0;
            P_volume = 0;
            P_open = 0;
            P_previous = 0;
            P_categoryCode = AssetCategory.None;
            P_updateTime = DateTimeConstants.MinValue;
        }


        //internal FullOptionDataCollection Parents
        //{
        //    get
        //    {
        //        return parent;
        //    }
        //    set
        //    {
        //        parent = value;
        //    }
        //}

        //private void OnFullOptionDataChanged()
        //{
        //    if (!(parent == null))
        //    {
        //        parent.FullOptionDataChanged(this);
        //    }
        //}

        /// <summary>
        /// This provides the deep copy of present object
        /// </summary>
        /// <returns></returns>
        public FullOptionData Copy()
        {
            FullOptionData copiedFullOptionData = null;
            if (this != null)
            {
                copiedFullOptionData = new FullOptionData();
                if (this.PutStruct != null)
                {
                    copiedFullOptionData.PutStruct = this.PutStruct;
                }

                if (this.CallStruct != null)
                {
                    copiedFullOptionData.CallStruct = this.CallStruct;
                }

                copiedFullOptionData.ExpirationDate = this.ExpirationDate;
                copiedFullOptionData.ExpirationMonth = this.ExpirationMonth;
                copiedFullOptionData.SortingKey = this.SortingKey;
                copiedFullOptionData.StrikePrice = this.StrikePrice;
                copiedFullOptionData.UnderlyingSymbol = this.UnderlyingSymbol;
                copiedFullOptionData.DaysToExpiration = this.DaysToExpiration;
            }
            return copiedFullOptionData;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyHasChanged()
        {
            if (_isObjectChanged)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, null);
                }

            }
            _isObjectChanged = false;
        }


        #endregion

    }
}
