using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class SymbolDataLiveFeedEventArgs : EventArgs
    {
        private SymbolData _snapshotLevel1Data = null;
        /// <summary>
        /// Gets or sets the snapshot of level1 data.
        /// </summary>
        /// <value>The snapshot level1 data.</value>
        public SymbolData SnapshotLevel1Data
        {
            get { return _snapshotLevel1Data; }
            set { _snapshotLevel1Data = value; }
        }

        private List<SymbolData> _symbolListContinuousLevel1Data = null;

        /// <summary>
        /// Gets or sets the continuous level1 data for a symbol list 
        /// </summary>
        /// <value>The symbol list continuous level1 data.</value>
        public List<SymbolData> SymbolListContinuousLevel1Data
        {
            get { return _symbolListContinuousLevel1Data; }
            set { _symbolListContinuousLevel1Data = value; }
        }

        //private Dictionary<string, double> _currencyPairConversionHash;

        ///// <summary>
        ///// Gets or sets the forex continuous data.
        ///// </summary>
        ///// <value>The forex continuous data.</value>
        //public Dictionary<string, double> CurrencyPairConversionHash
        //{
        //    get { return _currencyPairConversionHash; }
        //    set { _currencyPairConversionHash = value; }
        //}

        private ICollection<CurrencyConversion> _currencyConversions = null;

        /// <summary>
        /// Gets or sets the forex continuous data.
        /// </summary>
        /// <value>The forex continuous data.</value>
        public ICollection<CurrencyConversion> CurrencyConversions
        {
            get { return _currencyConversions; }
            set { _currencyConversions = value; }
        }

        private CurrencyConversion _snapshotForexdata;

        /// <summary>
        /// Gets or sets the snapshot of forex data.
        /// </summary>
        /// <value>The snapshot forex data.</value>
        public CurrencyConversion SnapshotForexData
        {
            get { return _snapshotForexdata; }
            set { _snapshotForexdata = value; }
        }



    }
}
