namespace Prana.Admin.BLL
{
    public class MarketDataType
    {
        private int _marketDataTypeID = int.MinValue;
        public int MarketDataTypeID
        {
            get { return _marketDataTypeID; }
            set { _marketDataTypeID = value; }
        }

        private string _marketDataTypeName = string.Empty;
        public string MarketDataTypeName
        {
            get { return _marketDataTypeName; }
            set { _marketDataTypeName = value; }
        }
    }
}
