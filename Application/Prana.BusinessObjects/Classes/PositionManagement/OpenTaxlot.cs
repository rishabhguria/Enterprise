using Csla;
using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class OpenTaxlot : BusinessBase<OpenTaxlot>
    {
        private int _ID = -1;
        private PositionType _positionType;

        public PositionType PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private long _quantity;

        public long Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        private double _notionalValue;
        public double NotionalValue
        {
            get { return _notionalValue; }
            set { _notionalValue = value; }
        }
        private double _lastPrice;
        public double LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }
        private int _assetID;

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }
        private string _assetName;
        public string AssetName
        {
            get { return _assetName; }
            set { _assetName = value; }
        }

        private int _underlyingID;

        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        private int _accountID;

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private string _accountName;

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }
        protected override object GetIdValue()
        {
            return _ID;
        }

    }
}
