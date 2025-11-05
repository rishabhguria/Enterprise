using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ClientRiskPref
    {
        private string _benchMarkSymbol;

        public string BenchMarkSymbol
        {
            get { return _benchMarkSymbol; }
            set { _benchMarkSymbol = value; }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private DateTime _endDate;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        private string _groupingCriteria;

        public string GroupingCriteria
        {
            get { return _groupingCriteria; }
            set { _groupingCriteria = value; }
        }


        private string _viewCriteria = "";

        public string ViewCriteria
        {
            get { return _viewCriteria; }
            set { _viewCriteria = value; }
        }

        //private string _reportType = "";

        //public string ReportType
        //{
        //    get { return _reportType; }
        //    set { _reportType = value; }
        //}
        private double _percentageBenchMarkMove;

        public double PercentageBenchMarkMove
        {
            get { return _percentageBenchMarkMove; }
            set { _percentageBenchMarkMove = value; }
        }



        public override string ToString()
        {
            return " StartDate: " + _startDate + " EndDate: " + _endDate + " BenchMarkSymbol: " + _benchMarkSymbol;
        }

    }
}
