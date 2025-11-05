using System;
using System.ComponentModel;

namespace Act40OrderGeneratorTool.Classes
{
    /// <summary>
    /// Defines a position in the portfolio
    /// </summary>
    class Trade
    {
        private String _symbol;

        public String Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private String _sector;

        public String Sector
        {
            get { return _sector; }
            set { _sector = value; }
        }

        private String _orderSide;

        public String OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }
        private Double _originalQuantity;

        [Browsable(false)]
        public Double OriginalQuantity
        {
            get { return _originalQuantity; }
            set { _originalQuantity = value; }
        }
        private Double _targetQuantity;

        [Browsable(false)]
        public Double TargetQuantity
        {
            get { return _targetQuantity; }
            set { _targetQuantity = value; }
        }

        private Double _tradeQuantity;

        [Browsable(false)]
        public Double TradeQuantity
        {
            get { return _tradeQuantity; }
            set { _tradeQuantity = value; }
        }

        private Double _selectedFeedPrice;

        public Double SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }

        private Double _originalValue;

        [Browsable(false)]
        public Double OriginalValue
        {
            get { return _originalValue; }
            set { _originalValue = value; }
        }

        private Double _originalContribution;

        [Browsable(false)]
        public Double OriginalContribution
        {
            get { return _originalContribution; }
            set { _originalContribution = value; }
        }

        private Double _targetValue;

        [Browsable(false)]
        public Double TargetValue
        {
            get { return _targetValue; }
            set { _targetValue = value; }
        }

        private Double _TargetContribution;

        [Browsable(false)]
        public Double TargetContribution
        {
            get { return _TargetContribution; }
            set { _TargetContribution = value; }
        }

        public String OriginalValueString
        {
            get { return FormatDouble(_originalValue); }
        }

        public String OriginalContributionString
        {
            get { return FormatDouble(_originalContribution); }
        }

        public String TargetValueString
        {
            get { return FormatDouble(_targetValue); }
        }

        public String TargetContributionString
        {
            get { return FormatDouble(_TargetContribution); }
        }

        public String TargetQuantityString
        {
            get { return FormatDouble(_targetQuantity); }
        }

        public String TradeQuantityString
        {
            get { return FormatDouble(_tradeQuantity); }
        }

        public String OriginalQuantityString
        {
            get { return FormatDouble(_originalQuantity); }
        }

        internal static Trade Create(Position Old, Position New, Double originalContribution, Double targetContribution)
        {
            Trade T = new Trade();
            if (Old == null) // create a position
            {
                T.Symbol = New.Symbol;
                T.Sector = New.Sector;
                T.OrderSide = New.Quantity < 0 ? "SELL SHORT" : "BUY";
                T.OriginalQuantity = 0;
                T.TargetQuantity = Math.Truncate(New.Quantity);
                T.TradeQuantity = Math.Abs(Math.Truncate(New.Quantity));
                T.SelectedFeedPrice = New.Price;
                T.OriginalValue = 0.0;
            }
            else if (New == null) // remove the position
            {
                T.Symbol = Old.Symbol;
                T.Sector = Old.Sector;
                T.OrderSide = Old.Quantity < 0 ? "BUY TO CLOSE" : "SELL";
                T.OriginalQuantity = Old.Quantity;
                T.TargetQuantity = 0;
                T.TradeQuantity = Math.Abs(Math.Truncate(Old.Quantity));
                T.SelectedFeedPrice = Old.Price;
                T.OriginalValue = Old.DollarDelta;
            }
            else
            {
                T.Symbol = Old.Symbol;
                T.Sector = Old.Sector;
                if (New.PositionSide == Side.Long)
                    T.OrderSide = Old.Quantity > New.Quantity ? "SELL" : "BUY";
                else
                    T.OrderSide = Old.Quantity > New.Quantity ? "SELL SHORT" : "BUY TO CLOSE";
                T.OriginalQuantity = Old.Quantity;
                T.TargetQuantity = Math.Truncate(New.Quantity);
                T.TradeQuantity = Math.Abs(Math.Truncate(New.Quantity) - Old.Quantity);
                T.SelectedFeedPrice = New.Price;
                T.OriginalValue = Old.DollarDelta;
            }
            T.TargetValue = T.TargetQuantity * T.SelectedFeedPrice;
            T.TargetContribution = targetContribution;
            T.OriginalContribution = originalContribution;
            return T;
        }

        private String FormatDouble(Double d)
        {
            if (Double.IsInfinity(d) || Double.IsNaN(d))
                d = 0;
            return d.ToString("N");
        }

        public override string ToString()
        {
            return " Ticker Symbol=" + Symbol + " Sector=" + Sector + " Side=" + OrderSide;
        }
    }
}
