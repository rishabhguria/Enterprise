using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BasketTrading
{
    class BasketPNL
    {       

        private int _numberOfSymbols=0;
        public int NumberOFSymbols
        {
            get { return _numberOfSymbols; }
            set { _numberOfSymbols = value; }
        }

        private int _numberOfOrders=0;
        public int NumberOfOrders
        {
            get { return _numberOfOrders; }
            set { _numberOfOrders = value; }
        }

        private double _currentValue=0.0;
        public double CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }

        private double _benchmarkValue=0.0;
        public double BenchMarkValue
        {
            get { return _benchmarkValue; }
            set { _benchmarkValue = value; }
        }

        private float _percentageChange=0;
        public float PercenageChange
        {
            get { return _percentageChange; }
            set { _percentageChange = value; }
        }

        private double _executionValue=0.0;
        public double ExecutionValue
        {
            get { return _executionValue; }
            set { _executionValue = value; }
        }

        private double _commitedValue=0.0;
        public double CommitedValue
        {
            get { return _commitedValue; }
            set { _commitedValue = value; }
        }

        private float _percentageExecuted=0;
        public float PercentageExecuted
        {
            get { return _percentageExecuted; }
            set { _percentageExecuted = value; }
        }

        private float _percentageCommited=0;
        public float PercentageCommited
        {
            get { return _percentageCommited; }
            set { _percentageCommited = value; }
        }
        private double _pnl;

        public double PNL
        {
            get { return _pnl; }
            set { _pnl = value; }
        }
        private double _commitedQty = 0.0;
        public double CommitedQty
        {
            get { return _commitedQty; }
            set { _commitedQty = value; }
        }
	
    }
}
