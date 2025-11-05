namespace Prana.BusinessObjects
{
    public class DefAssetSide
    {
        private int? _asset;
        private int? _orderSide;

        public int? Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        public int? OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }
    }
}
