using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class GenericDayEndData
    {
        private Dictionary<int, double> _startOfDayAccountWiseCash = new Dictionary<int, double>();
        private Dictionary<int, Dictionary<int, double>> _startOfDayAccountWiseAccruals = new Dictionary<int, Dictionary<int, double>>();
        private Dictionary<int, Tuple<double, double>> _dayAccountWiseCash = new Dictionary<int, Tuple<double, double>>();
        private Dictionary<int, Dictionary<int, double>> _dayAccountWiseAccruals = new Dictionary<int, Dictionary<int, double>>();

        public Dictionary<int, double> StartOfDayAccountWiseCash
        {
            get { return _startOfDayAccountWiseCash; }
            set { _startOfDayAccountWiseCash = value; }
        }

        public Dictionary<int, Dictionary<int, double>> StartOfDayAccountWiseAccruals
        {
            get { return _startOfDayAccountWiseAccruals; }
            set { _startOfDayAccountWiseAccruals = value; }
        }

        public Dictionary<int, Tuple<double, double>> DayAccountWiseCash
        {
            get { return _dayAccountWiseCash; }
            set { _dayAccountWiseCash = value; }
        }

        public Dictionary<int, Dictionary<int, double>> DayAccountWiseAccruals
        {
            get { return _dayAccountWiseAccruals; }
            set { _dayAccountWiseAccruals = value; }
        }
    }
}