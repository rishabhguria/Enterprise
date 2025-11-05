namespace Prana.CommonDataCache
{
    /// <summary>
    /// Summary description for UnAllocatedCache.
    /// </summary>
    class UnAllocatedOrderCache
    {
        private int _orderID;
        // private int _accountID;
        private int _auecID;
        private int _cvID;
        private double _avgPrice;
        private int _quantity;

        public UnAllocatedOrderCache()
        {
            //constructor logic
        }
        public int AllocationId
        {
            set { _orderID = value; }
            get { return _orderID; }

        }
        //public int AccountID
        //{
        //    set { _accountID = value; }
        //    get { return _accountID; }

        //}
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }
        public int CvID
        {
            set { _cvID = value; }
            get { return _cvID; }

        }
        public double AvgPrice
        {
            set { _avgPrice = value; }
            get { return _avgPrice; }
        }
        public int Quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }

    }
}
