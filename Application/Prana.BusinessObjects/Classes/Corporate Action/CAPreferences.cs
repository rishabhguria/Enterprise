using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CAPreferences
    {


        private bool _useNetNotional = true;
        public bool UseNetNotional
        {
            get { return _useNetNotional; }
            set { _useNetNotional = value; }
        }

        private bool _adjustFractionalSharesatAccountLevel = true;
        public bool AdjustFractionalSharesAtAccountPositionLevel
        {
            get { return _adjustFractionalSharesatAccountLevel; }
            set { _adjustFractionalSharesatAccountLevel = value; }
        }

        /// <summary>
        /// The Closing Algo selected like FIFO, HIFO etc..
        /// </summary>
        private PostTradeEnums.CloseTradeAlogrithm _closingAlgo;

        public PostTradeEnums.CloseTradeAlogrithm ClosingAlgo
        {
            get { return _closingAlgo; }
            set { _closingAlgo = value; }
        }


        /// <summary>
        /// The Secondary Sort Criteria like AvgPxAscending etc.
        /// Its only applicable when the primary criteria for sorting of taxlots is same...
        /// </summary>
        private PostTradeEnums.SecondarySortCriteria _secondarySort = PostTradeEnums.SecondarySortCriteria.None;

        public PostTradeEnums.SecondarySortCriteria SecondarySort
        {
            get { return _secondarySort; }
            set { _secondarySort = value; }
        }

    }
}
