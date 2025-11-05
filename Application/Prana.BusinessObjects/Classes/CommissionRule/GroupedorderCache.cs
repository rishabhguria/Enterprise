using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for UnGrouped Order.
    /// </summary>
    public class GroupedorderCache
    {
        private int _groupID;
        //private int _accountID;
        string _symbol = string.Empty;
        private int _auecID;
        private int _cvID;
        private double _avgPrice;
        private double _quantity;
        private TradeType _tradeTypeValue;
        private int _assetId;


        private double _multiplier;
        private int _orderSideTagValue;

        private int _sideMultiplier;





        public GroupedorderCache()
        {
            //constructor logic
        }
        public GroupedorderCache(int groupID, string symbol, int auecID, int cvID, double avgPrice, long quantity, TradeType tradeType, int assetId, double multiplier, int sideMultiplier)
        {
            _groupID = groupID;
            _symbol = symbol;
            _auecID = auecID;
            _cvID = cvID;
            _avgPrice = avgPrice;
            _quantity = quantity;
            _tradeTypeValue = tradeType;
            _assetId = assetId;
            _multiplier = multiplier;
            _sideMultiplier = sideMultiplier;
        }
        public int GroupID
        {
            set { _groupID = value; }
            get { return _groupID; }

        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
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
        public double Quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }

        public TradeType TradeType
        {
            get { return _tradeTypeValue; }
            set { _tradeTypeValue = value; }
        }

        public int AssetId
        {
            get { return _assetId; }
            set { _assetId = value; }
        }


        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

        public int OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }


        public int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }

        private double _commission;

        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        private double _fees;

        public double Fees
        {
            get { return _fees; }
            set { _fees = value; }
        }


    }
}
