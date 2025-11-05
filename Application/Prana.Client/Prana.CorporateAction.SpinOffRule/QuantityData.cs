using Prana.BusinessObjects;
using System;

namespace Prana.CorporateAction.SpinOffRule
{
    public class QuantityData
    {
        private string _groupID;
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private string _taxLotID;
        public string TaxLotID
        {
            get { return _taxLotID; }

            set { _taxLotID = value; }
        }

        private DateTime _originalPurchaseDate = DateTimeConstants.MinValue;
        public DateTime OriginalPurchaseDate
        {
            get { return _originalPurchaseDate; }
            set { _originalPurchaseDate = value; }
        }

        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private double _integralPart;
        public double IntegralPart
        {
            get { return _integralPart; }
            set { _integralPart = value; }
        }

        private double _fractionalPart;
        public double FractionalPart
        {
            get { return _fractionalPart; }
            set { _fractionalPart = value; }
        }

        private double _avgPrice;
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        public QuantityData(string groupID, string taxlotID, DateTime originalPurchaseDate, double enteredQuantity, double avgPrice)
        {
            _groupID = groupID;
            _taxLotID = taxlotID;
            _originalPurchaseDate = originalPurchaseDate;
            _quantity = enteredQuantity;
            _integralPart = Math.Floor(enteredQuantity);
            _fractionalPart = _quantity - _integralPart;
            _avgPrice = avgPrice;
        }

        public void MoveToCeiling()
        {
            if (_fractionalPart != 0)
            {
                _fractionalPart = 0;
                _quantity = Math.Ceiling(_quantity);
                _integralPart++;
            }
        }

        public void MoveToFloor()
        {
            if (_fractionalPart != 0)
            {
                _fractionalPart = 0;
                _quantity = Math.Floor(_quantity);
            }
        }
    }
}
