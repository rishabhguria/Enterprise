using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public struct CAPreviewResult
    {
        private string _closingIDs;
        private string _noPositionSymbols;
        /// <summary>
        /// Closing taxlot ids
        /// </summary>
        public string ClosingIDs
        {
            get { return _closingIDs; }
            set { _closingIDs = value; }
        }

        private string _caIDs;
        /// <summary>
        /// If CA applied in future date then it shows the ca applied taxlotids.
        /// </summary>
        public string CAIDs
        {
            get { return _caIDs; }
            set { _caIDs = value; }
        }

        public string NoPositionSymbols
        {
            get { return _noPositionSymbols; }
            set { _noPositionSymbols = value; }
        }
        private bool _isBoxedPositioin;

        public bool IsBoxedPositioin
        {
            get { return _isBoxedPositioin; }
            set { _isBoxedPositioin = value; }
        }

        private string _boxedPositionTaxlotIds;
        /// <summary>
        /// Boxed Position taxlot ids
        /// </summary>
        public string BoxedPositionTaxlotIds
        {
            get { return _boxedPositionTaxlotIds; }
            set { _boxedPositionTaxlotIds = value; }
        }

        /// <summary>
        /// Error Message
        /// </summary>
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

    }
}
