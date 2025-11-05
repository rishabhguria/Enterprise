using Csla;
using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public abstract class MarkPriceBase : BusinessBase<MarkPriceBase>
    {
        public MarkPriceBase()
        {
            MarkAsChild();
        }

        private int _markPriceID = int.MinValue;
        [Browsable(false)]
        public int MarkPriceID
        {
            get { return _markPriceID; }
            set
            {
                _markPriceID = value;
                PropertyHasChanged();
            }
        }

        private double _appMarkPrice = 0.0;
        public double AppMarkPrice
        {
            get { return _appMarkPrice; }
            set
            {
                _appMarkPrice = value;
                PropertyHasChanged();
            }
        }

        private double _primeBrokerMarkPrice = 0.0;
        public double PrimeBrokerMarkPrice
        {
            get { return _primeBrokerMarkPrice; }
            set
            {
                _primeBrokerMarkPrice = value;
                PropertyHasChanged();
            }
        }

        private double _finalMarkPrice = 0.0;
        public double FinalMarkPrice
        {
            get { return _finalMarkPrice; }
            set
            {
                _finalMarkPrice = value;
                PropertyHasChanged();
            }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                PropertyHasChanged();
            }
        }

        private bool _useMarkPrice = true;
        public bool UseMarkPrice
        {
            get { return _useMarkPrice; }
            set
            {
                _useMarkPrice = value;
                PropertyHasChanged();
            }
        }





        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// Need to have some id which uniquely identify this.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _markPriceID;
        }

        #region OldCode
        //private int _markPositionID = int.MinValue;
        //[Browsable(false)]
        //public int MarkPositionID
        //{
        //    get { return _markPositionID; }
        //    set
        //    {
        //        _markPositionID = value;
        //        PropertyHasChanged();
        //    }
        //}

        //private double _markPrice = 0.0;
        //[Browsable(true)]
        //public double MarkPrice
        //{
        //    get { return _markPrice; }
        //    set
        //    {
        //        _markPrice = value;
        //        PropertyHasChanged();
        //    }
        //}

        //private DateTime _markDateTime = DateTime.Today.Date;
        //[Browsable(true)]
        //public DateTime MarkDateTime
        //{
        //    get { return _markDateTime; }
        //    set
        //    {
        //        _markDateTime = value;
        //        PropertyHasChanged();
        //    }
        //}

        //private string _symbol = string.Empty;
        //[Browsable(true)]
        //public string Symbol
        //{
        //    get { return _symbol; }
        //    set
        //    {
        //        _symbol = value;
        //        PropertyHasChanged();
        //    }
        //}

        //private string _isLiveFeedAvailable = "No";
        //[Browsable(true)]
        //public string IsLiveFeedAvailable
        //{
        //    get { return _isLiveFeedAvailable; }
        //    set
        //    {
        //        _isLiveFeedAvailable = value;
        //        PropertyHasChanged();
        //    }
        //}

        //private bool _includeSymbol = false;
        //[Browsable(true)]
        //public bool IncludeSymbol
        //{
        //    get { return _includeSymbol; }
        //    set
        //    {
        //        _includeSymbol = value;
        //        PropertyHasChanged();
        //    }
        //}
        //private string _isLiveFeedAvailable = "No";
        //[Browsable(true)]
        //public string IsLiveFeedAvailable
        //{
        //    get { return _isLiveFeedAvailable; }
        //    set
        //    {
        //        _isLiveFeedAvailable = value;
        //        PropertyHasChanged();
        //    }
        //}

        //private bool _includeSymbol = false;
        //[Browsable(true)]
        //public bool IncludeSymbol
        //{
        //    get { return _includeSymbol; }
        //    set
        //    {
        //        _includeSymbol = value;
        //        PropertyHasChanged();
        //    }
        //}
        //private DateTime _markDateTime = DateTime.Today.Date;
        //[Browsable(true)]
        //public DateTime MarkDateTime
        //{
        //    get { return _markDateTime; }
        //    set
        //    {
        //        _markDateTime = value;
        //        PropertyHasChanged();
        //    }
        //}
        #endregion
    }
}
