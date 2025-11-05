using System;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    [Serializable]
    public class FutureRootData
    {
        private string _rootsymbol = string.Empty;

        public string RootSymbol
        {
            get { return _rootsymbol; }
            set { _rootsymbol = value; }
        }
        private double _multiplier;

        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        private string _psRootSymbol = string.Empty;

        public string PSRootSymbol
        {
            get { return _psRootSymbol; }
            set { _psRootSymbol = value; }
        }


        private string _proxyRootSymbol = string.Empty;

        public string ProxyRootSymbol
        {
            get { return _proxyRootSymbol; }
            set { _proxyRootSymbol = value; }
        }
        private string _cutoffTime = string.Empty;
        /// <summary>
        /// Cut off time for the future root on the basis of which process date is determined. This is used only
        /// when _isFutureCutOffTimeUsed is true in Secmasterdatacache. This is further copied into the respective future
        /// sybmol's data.
        /// </summary>
        public string CutoffTime
        {
            get { return _cutoffTime; }
            set { _cutoffTime = value; }
        }

        /// <summary>
        /// As root symbol can be same for different exchanges,we have to keep a pair of symbol and exchange as unique instead of  unique symbol.
        /// </summary>
        private string _exchange = string.Empty;
        public string Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }
        //private string _underlyingSymbol;

        //public string UnderlyingSymbol
        //{
        //    get { return _underlyingSymbol; }
        //    set { _underlyingSymbol = value; }
        //}

        #region UDA DATA related fields - omshiv, Nov 2013
        // UDA now can be add/edit from root ui


        private int _udaAssetClassID = int.MinValue;
        private int _udaSecurityTypeID = int.MinValue;
        private int _udaSubSectorID = int.MinValue;
        private int _udaSectorID = int.MinValue;
        private int _udaCountryID = int.MinValue;


        public int UDAAssetClassID
        {
            get { return _udaAssetClassID; }
            set { _udaAssetClassID = value; }
        }
        public int UDASecurityTypeID
        {
            get { return _udaSecurityTypeID; }
            set { _udaSecurityTypeID = value; }
        }
        public int UDASectorID
        {
            get { return _udaSectorID; }
            set { _udaSectorID = value; }
        }
        public int UDASubSectorID
        {
            get { return _udaSubSectorID; }
            set { _udaSubSectorID = value; }
        }
        public int UDACountryID
        {
            get { return _udaCountryID; }
            set { _udaCountryID = value; }
        }

        #endregion

        private bool _isCurrencyFuture;
        public bool IsCurrencyFuture
        {
            get { return _isCurrencyFuture; }
            set { _isCurrencyFuture = value; }
        }

        private string _bbgYellowKey;
        public string BBGYellowKey
        {
            get { return _bbgYellowKey; }
            set { _bbgYellowKey = value; }
        }

        private string _bbgRoot;
        public string BBGRoot
        {
            get { return _bbgRoot; }
            set { _bbgRoot = value; }
        }


        private SerializableDictionary<String, Object> _dynamicUDA = new SerializableDictionary<string, object>();
        public SerializableDictionary<String, Object> DynamicUDA { get { return _dynamicUDA; } }
    }
}
