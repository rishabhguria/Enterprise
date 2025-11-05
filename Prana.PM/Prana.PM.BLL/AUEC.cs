using Csla;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class AUEC : BusinessBase<AUEC>
    {
        public AUEC()
        {
            MarkAsChild();
        }

        public AUEC(int auecID, string auecName) : this()
        {
            _auecID = auecID;
            _auecName = auecName;
        }
        private int _auecID;

        public int AUECID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
                PropertyHasChanged();
            }
        }

        private string _auecName = string.Empty;

        public string AUECName
        {
            get { return _auecName; }
            set
            {
                _auecName = value;
                PropertyHasChanged();
            }
        }


        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        protected override object GetIdValue()
        {
            return _auecID;
        }

        private int _assetID;

        public int AssetID
        {
            get { return _assetID; }
            set
            {
                _assetID = value;
                PropertyHasChanged();
            }
        }

        private int _underlyingID;

        public int UnderlyingID
        {
            get { return _underlyingID; }
            set
            {
                _underlyingID = value;
                PropertyHasChanged();
            }
        }

        private int _exchangeID;

        public int ExchangeID
        {
            get { return _exchangeID; }
            set
            {
                _exchangeID = value;
                PropertyHasChanged();
            }
        }

        private int _currencyID;

        public int CurrencyID
        {
            get { return _currencyID; }
            set
            {
                _currencyID = value;
                PropertyHasChanged();
            }
        }

        //public override string ToString()
        //{
        //    return Asset.Name + "/" + Underlying.Name + "/" + Exchange.Name + "/" + Currency.Symbol;
        //}

    }
}
