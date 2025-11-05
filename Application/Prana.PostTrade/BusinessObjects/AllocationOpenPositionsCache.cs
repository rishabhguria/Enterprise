namespace Prana.PostTrade
{
    public class AllocationOpenPositionsCache
    {

        //AllocationOpenPositionsCache
        public AllocationOpenPositionsCache()
        {

        }

        private string _symbol = string.Empty;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private int _accountID = 0;

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private string _account = string.Empty;

        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private decimal _quantity = 0;

        /// <summary>
        /// Initial Quantity when the cache is created. This will be same as Open Position when cache is created
        /// </summary>
        public decimal Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        //_grossQty
        private decimal _grossQty = 0;

        public decimal GrossQty
        {
            get { return _grossQty; }
            set { _grossQty = value; }
        }

        private decimal _netQty = 0;

        public decimal NetQty
        {
            get { return _netQty; }
            set { _netQty = value; }
        }


        private string _sideID = string.Empty;

        public string SideID
        {
            get { return _sideID; }
            set { _sideID = value; }
        }

        private string _side = string.Empty;

        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        //openPosition
        private decimal _openPosition = 0;

        public decimal OpenPositions
        {
            get { return _openPosition; }
            set { _openPosition = value; }
        }

        private string _positionType_AccountLevel = string.Empty;

        public string PositionType_AccountLevel
        {
            get { return _positionType_AccountLevel; }
            set { _positionType_AccountLevel = value; }
        }

        private string _positionType_ConsolidatedLevel = string.Empty;

        public string PositionType_ConsolidatedLevel
        {
            get { return _positionType_ConsolidatedLevel; }
            set { _positionType_ConsolidatedLevel = value; }
        }

        private bool _isBoxedPosition = false;

        public bool IsBoxedPosition
        {
            get { return _isBoxedPosition; }
            set { _isBoxedPosition = value; }
        }
    }
}
