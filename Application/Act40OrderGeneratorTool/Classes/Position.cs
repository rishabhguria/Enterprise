using System;

namespace Act40OrderGeneratorTool.Classes
{
    /// <summary>
    /// Defines a position in the portfolio
    /// </summary>
    class Position
    {
        private String _account;

        public String Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private String _symbol;

        public String Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private Side _positionSide;

        public Side PositionSide
        {
            get { return _positionSide; }
            set { _positionSide = value; }
        }

        private Double _quantity;

        public Double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private Double _selectedFeedPrice;

        public Double Price
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }

        private String _sector;

        public String Sector
        {
            get { return _sector; }
            set { _sector = value; }
        }

        private Double _dollarDelta;

        public Double DollarDelta
        {
            get { return _dollarDelta; }
            set { _dollarDelta = value; }
        }


        public override string ToString()
        {
            return " Ticker Symbol=" + Symbol + " Side=" + PositionSide;
        }

        internal Position(String account, String symbol, Side positionSide, Double quantity, Double markPrice, String sector, Double dollarDelta)
        {
            Account = account;
            Symbol = symbol;
            PositionSide = positionSide;
            Quantity = quantity;
            Price = markPrice;
            Sector = sector;
            DollarDelta = dollarDelta;
        }
    }
}
