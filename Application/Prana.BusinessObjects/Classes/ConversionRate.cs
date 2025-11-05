using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ConversionRate : ICloneable
    {

        private double _rateValue = double.MinValue;
        public double RateValue
        {
            get { return _rateValue; }
            set { _rateValue = value; }
        }

        private Operator _conversionMethod = Operator.M;
        public Operator ConversionMethod
        {
            get { return _conversionMethod; }
            set { _conversionMethod = value; }
        }

        private DateTime _date = DateTimeConstants.MinValue;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _fxeSignalSymbol = string.Empty;
        public string FXeSignalSymbol
        {
            get { return _fxeSignalSymbol; }
            set { _fxeSignalSymbol = value; }
        }

        //CHMW-3132	Account wise fx rate handling for expiration settlement
        private int _accountID = int.MinValue;
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        //#region ICloneable Members
        ///// <summary>
        ///// Returns deep copy of the passed object
        ///// </summary>
        ///// <returns></returns>
        //public ConversionRate Clone()
        //{
        //    ConversionRate clonedRate = new ConversionRate();
        //    clonedRate.RateValue = this.RateValue;
        //    clonedRate.ConversionMethod = this.ConversionMethod;
        //    clonedRate.Date = this.Date;

        //    return clonedRate;
        //}

        //#endregion

        #region ICloneable Members

        public object Clone()
        {
            ConversionRate clonedRate = new ConversionRate();
            clonedRate.RateValue = this.RateValue;
            clonedRate.ConversionMethod = this.ConversionMethod;
            clonedRate.Date = this.Date;
            clonedRate.FXeSignalSymbol = this.FXeSignalSymbol;
            clonedRate.AccountID = this.AccountID;
            return clonedRate;
        }

        #endregion
    }

    [Serializable]
    public class fxInfo
    {
        private ConversionRate _conversionRate;

        public ConversionRate ConversionRate
        {
            get { return _conversionRate; }
            set { _conversionRate = value; }
        }

        private string _pranaSymbol;

        public string PranaSymbol
        {
            get { return _pranaSymbol; }
            set { _pranaSymbol = value; }
        }

        private AssetCategory _categoryCode;

        public AssetCategory CategoryCode
        {
            get { return _categoryCode; }
            set { _categoryCode = value; }
        }


        private int _fromCurrencyID;

        public int FromCurrencyID
        {
            get { return _fromCurrencyID; }
            set { _fromCurrencyID = value; }
        }

        private int _toCurrencyID;

        public int ToCurrencyID
        {
            get { return _toCurrencyID; }
            set { _toCurrencyID = value; }
        }
    }
}
