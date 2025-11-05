using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;

namespace Prana.BasketTrading
{
    class BasketSummaryParts
    {
        private string _side = string.Empty;
        private Int64 _numberOfSymbols;
        private double _numberOfShares;
        private double _absoluteBasketValue;
        private double _absoluteExecutedValue;
        private double _numberofSharesExecuted;
        private double _numberofSharesCommited;
        private double _percentageCommited;
        private double _percentageExecuted;
        private double _basketPNL;
        public double PercentageExecuted
        {
            get
            {
                return _percentageExecuted;
            }
            set
            {
                _percentageExecuted = value;
            }

        }

        public string Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
            }

        }
        public Int64 NumberOfSymbols
        {
            get
            {
                return _numberOfSymbols;
            }
            set
            {
                _numberOfSymbols = value;
            }

        }
        public double NumberOfShares
        {
            get
            {
                return _numberOfShares;
            }
            set
            {
                _numberOfShares = value;
            }

        }
        public double AbsoluteBasketValue
        {
            get
            {
                return _absoluteBasketValue;
            }
            set
            {
                _absoluteBasketValue = value;
            }

        }
        public double AbsoluteExecutedValue
        {
            get
            {
                return _absoluteExecutedValue;
            }
            set
            {
                _absoluteExecutedValue = value;
            }

        }
        public double NumberofSharesExecuted
        {
            get
            {
                return _numberofSharesExecuted;
            }
            set
            {
                _numberofSharesExecuted = value;
            }

        }
        public double NumberofSharesCommited
        {
            get
            {
                return _numberofSharesCommited;
            }
            set
            {
                _numberofSharesCommited = value;
            }

        }
        public double PercentageCommited
        {
            get
            {
                return _percentageCommited;
            }
            set
            {
                _percentageCommited = value;
            }

        }
        public double BasketPNL
        {
            get
            {
                return _basketPNL;
            }
            set
            {
                _basketPNL = value;
            }

        }
    }
}
