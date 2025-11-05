using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public class ShortLocateOrder : INotifyPropertyChanged
    {

        #region members

        private int _nirvanaLocateID;

        private string _ticker;

        private string _broker;

        private double _lastPx = 0;

        private double _tradeQuantity = 0;

        private string _clientMasterfund;

        private double _borrowSharesAvailable = 0;

        private double _borrowRate;

        private double _totalBorrowAmount = 0;

        private string _borrowerId;

        private double _borrowedShare = 0;

        private double _borrowedRate;

        private double _totalBorrowedAmount = 0;

        private double _sODBorrowshareAvailable = 0;

        private double _sODBorrowRate;

        private string _statusSource;

        #endregion

        #region Properties

        public int NirvanaLocateID
        {
            get { return _nirvanaLocateID; }
            set { _nirvanaLocateID = value; }
        }

        public string Ticker
        {
            get { return _ticker; }
            set { _ticker = value; }
        }

        public string Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }

        public string ClientMasterfund
        {
            get { return _clientMasterfund; }
            set { _clientMasterfund = value; }
        }

        public double LastPx
        {
            get { return _lastPx; }
            set { _lastPx = value; }
        }

        public double TradeQuantity
        {
            get { return _tradeQuantity; }
            set { _tradeQuantity = value; }
        }

        public double BorrowSharesAvailable
        {
            get { return _borrowSharesAvailable; }
            set { _borrowSharesAvailable = value; }
        }

        public double BorrowRate
        {
            get { return _borrowRate; }
            set { _borrowRate = value; }
        }

        public double TotalBorrowAmount
        {
            get { return _totalBorrowAmount; }
            set { _totalBorrowAmount = value; }
        }

        public string BorrowerId
        {
            get { return _borrowerId; }
            set { _borrowerId = value; }
        }

        public double BorrowedShare
        {
            get { return _borrowedShare; }
            set { _borrowedShare = value; }
        }

        public double BorrowedRate
        {
            get { return _borrowedRate; }
            set { _borrowedRate = value; }
        }

        public double TotalBorrowedAmount
        {
            get { return _totalBorrowedAmount; }
            set { _totalBorrowedAmount = value; }
        }

        public double SODBorrowshareAvailable
        {
            get { return _sODBorrowshareAvailable; }
            set { _sODBorrowshareAvailable = value; }
        }

        public double SODBorrowRate
        {
            get { return _sODBorrowRate; }
            set { _sODBorrowRate = value; }
        }


        public string StatusSource
        {
            get { return _statusSource; }
            set { _statusSource = value; }
        }

        #endregion

        #region INotifyPropertyChanged Members
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, null);
            }
        }
        #endregion INotifyPropertyChanged Members
    }
}
