using System;
using System.Collections.Generic;
using System.Text;

namespace DataProcessor
{
    class PositionDetail
    {
        private string symbol = String.Empty;
        private double _quantity = 0.0;
        private double _avgPrice = 0.0;
        private int _subAccountID = 0;
        private int _tradeDate = 0;
        private int _fundID = 0;
        private double _commission= 0;

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public int SubAccountID
        {
            get { return _subAccountID; }
            set { _subAccountID = value; }
        }
        public int TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }
        public int FundID
        {
            get { return _fundID; }
            set { _fundID = value; }
        }
        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }
    }
}
