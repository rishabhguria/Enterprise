namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for MarketFees.
    /// </summary>
    public class MarketFee
    {
        #region Private members

        private double _purchaseSecFees = 0;
        private double _saleSecFees = 0;
        private double _purchaseStamp = 0;
        private double _saleStamp = 0;
        private double _purchaseLevy = 0;
        private double _saleLevy = 0;
        private int _auecExchangeID = int.MinValue;

        #endregion

        #region Constructors

        public MarketFee()
        {
        }

        public MarketFee(double purchaseSecFees, double saleSecFees, double purchaseStamp,
            double saleStamp, double purchaseLevy, double saleLevy)
        {
            _purchaseSecFees = purchaseSecFees;
            _saleSecFees = saleSecFees;
            _purchaseStamp = purchaseStamp;
            _saleStamp = saleStamp;
            _purchaseLevy = purchaseLevy;
            _saleLevy = saleLevy;
        }

        #endregion

        #region Properties

        public double PurchaseSecFees
        {
            get { return _purchaseSecFees; }
            set { _purchaseSecFees = value; }
        }

        public double SaleSecFees
        {
            get { return _saleSecFees; }
            set { _saleSecFees = value; }
        }

        public double PurchaseStamp
        {
            get { return _purchaseStamp; }
            set { _purchaseStamp = value; }
        }

        public double SaleStamp
        {
            get { return _saleStamp; }
            set { _saleStamp = value; }
        }

        public double PurchaseLevy
        {
            get { return _purchaseLevy; }
            set { _purchaseLevy = value; }
        }

        public double SaleLevy
        {
            get { return _saleLevy; }
            set { _saleLevy = value; }
        }

        public int AUECExchangeID
        {
            get { return _auecExchangeID; }
            set { _auecExchangeID = value; }
        }
        #endregion
    }
}
