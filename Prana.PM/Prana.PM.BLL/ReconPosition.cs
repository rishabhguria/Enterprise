using System;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    [Serializable]
    public class ReconPosition : ICloneable
    {
        public ReconPosition()
        {

        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _side = string.Empty;
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private string _orderSideTagValue = string.Empty;
        [Browsable(false)]
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set
            {
                _orderSideTagValue = value;
                //_sideType = (Side)value;
            }
        }

        private double _quantity = 0;

        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private double _avgPX = 0;

        public double AvgPX
        {
            get { return _avgPX; }
            set { _avgPX = value; }
        }

        private double _fee = 0;

        public double Fee
        {
            get { return _fee; }
            set { _fee = value; }
        }

        private double _commission = 0;

        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        private string _accountName = string.Empty;
        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        private string _iSIN = string.Empty;
        //[Browsable(false)]
        public string ISIN
        {
            get { return _iSIN; }
            set { _iSIN = value; }
        }

        private string _cUSIP = string.Empty;
        //[Browsable(false)]
        public string CUSIP
        {
            get { return _cUSIP; }
            set { _cUSIP = value; }
        }

        private string _sEDOL = string.Empty;
        //[Browsable(false)]
        public string SEDOL
        {
            get { return _sEDOL; }
            set { _sEDOL = value; }
        }

        private string _rIC = string.Empty;
        //[Browsable(false)]
        public string RIC
        {
            get { return _rIC; }
            set { _rIC = value; }
        }

        private string _bloomberg = string.Empty;
        //[Browsable(false)]
        public string Bloomberg
        {
            get { return _bloomberg; }
            set { _bloomberg = value; }
        }

        private string _osiOptionSymbol;

        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol;

        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        private string _pbSymbol = string.Empty;

        public string PBSymbol
        {
            get { return _pbSymbol; }
            set { _pbSymbol = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            ReconPosition cloneReconPosition = new ReconPosition();
            cloneReconPosition.AvgPX = _avgPX;
            cloneReconPosition.Commission = _commission;
            cloneReconPosition.Fee = _fee;
            cloneReconPosition.AccountName = _accountName;
            cloneReconPosition.OrderSideTagValue = _orderSideTagValue;
            cloneReconPosition.Quantity = _quantity;
            cloneReconPosition.Side = _side;
            cloneReconPosition.Symbol = _symbol;
            cloneReconPosition.ISIN = _iSIN;
            cloneReconPosition.CUSIP = _cUSIP;
            cloneReconPosition.SEDOL = _sEDOL;
            cloneReconPosition.RIC = _rIC;
            cloneReconPosition.Bloomberg = _bloomberg;
            cloneReconPosition.PBSymbol = _pbSymbol;
            return cloneReconPosition;
        }

        #endregion
    }
}
