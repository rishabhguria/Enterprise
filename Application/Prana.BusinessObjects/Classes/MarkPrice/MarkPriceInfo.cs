using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class MarkPriceInfo
    {
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _markPrice = 0;

        public double MarkPrice
        {
            get { return _markPrice / _splitFactor; }
            set { _markPrice = value; }
        }

        private double _splitFactor = 1;

        public double SplitFactor
        {
            get { return _splitFactor; }
            set { _splitFactor = value; }
        }

        private double _forwardPoints;

        public double ForwardPoints
        {
            get { return _forwardPoints; }
            set { _forwardPoints = value; }
        }
        private double _fxRate;//in case of fx?fxforward tab.. FxRate is shown in case of FxSpot and FxForward

        public double FxRate
        {
            get { return _fxRate; }
            set { _fxRate = value; }
        }

        private int _auecID;

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private string _auecIdentifier;

        public string AUECIdentifier
        {
            get { return _auecIdentifier; }
            set { _auecIdentifier = value; }
        }

        private DateTime _dateActual;
        /// <summary>
        /// The mark price is of actually this date.
        /// </summary>
        public DateTime DateActual
        {
            get { return _dateActual; }
            set { _dateActual = value; }
        }

        //private int _markPriceIndicator;
        /// <summary>
        /// The value is 0 if the markprice's DateActual is same as requested date, else 1
        /// </summary>
        public int MarkPriceIndicator
        {
            get
            {
                if (MarkPrice != 0)
                {
                    if (YesterdayBusinessAdjustedDate > DateActual)
                        return 1;
                    else
                        return 0;
                }
                return 1;
            }

        }


        private DateTime _yesterdayBusinessAdjustedDate;

        public DateTime YesterdayBusinessAdjustedDate
        {
            get { return _yesterdayBusinessAdjustedDate; }
            set { _yesterdayBusinessAdjustedDate = value; }
        }


        public string MarkPriceStr
        {
            get
            {
                //we may also apply check on MarkPriceIndicator 
                if (MarkPrice != 0)
                {
                    if (YesterdayBusinessAdjustedDate > DateActual)
                        return MarkPrice.ToString("#,####0.####") + "*";
                    else
                        return MarkPrice.ToString("#,####0.####");
                }
                //return string.Empty;
                //PRANA-2159 On Ungrouped Custom views,PM showing Blank Closing Mark in place of Undefined.
                return "Undefined";
            }
        }

        //[RG:20130125] Added to have this info for FX and FXForward symbols.
        private int _leadCurrencyID;
        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }

        private int _vsCurrencyID;
        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        private int _assetID;
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private string _bloombergSymbol;

        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }

            set { _bloombergSymbol = value; }

        }

        private string _ISINSymbol;

        public string ISINSymbol
        {
            get { return _ISINSymbol; }
            set { _ISINSymbol = value; }
        }

        private string _CUSIPSymbol;

        public string CUSIPSymbol
        {
            get { return _CUSIPSymbol; }
            set { _CUSIPSymbol = value; }
        }

        private string _currency;
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        //modifed by: Bharat raturi
        //purpose: to add extra fields for getting the complete details of the mark price
        public int AccountID { get; set; }
        public string PricingSource { get; set; }
        public string PricingType { get; set; }
        public string AccountName { get; set; }
    }
}
