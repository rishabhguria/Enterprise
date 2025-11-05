namespace Prana.CommonDataCache
{
    /// <summary>
    /// Summary description for Allocated Order Cache.
    /// </summary>
    class AllocatedOrderCache
    {
        private int _allocationID;
        private int _accountID;
        private int _auecID;
        private int _cvID;
        private double _avgPrice;
        private int _quantity;

        public AllocatedOrderCache()
        {
            //constructor logic
        }
        public int AllocationId
        {
            set { _allocationID = value; }
            get { return _allocationID; }

        }
        public int AccountID
        {
            set { _accountID = value; }
            get { return _accountID; }

        }
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
