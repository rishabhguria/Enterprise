namespace Prana.BusinessObjects
{
    public class RiskStressTest
    {
        private string _name;

        public string GroupingName
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _Beta;

        public double Beta
        {
            get { return _Beta; }
            set { _Beta = value; }
        }

        private double _NewPortfolioValue;

        public double NewPortfolioValue
        {
            get { return _NewPortfolioValue; }
            set { _NewPortfolioValue = value; }
        }

        private double _PercentageChangeinPortfolioValue;

        public double PercentageChangeinPortfolioValue
        {
            get { return _PercentageChangeinPortfolioValue; }
            set { _PercentageChangeinPortfolioValue = value; }
        }


        private double _OldPortfolioValue;

        public double OldPortfolioValue
        {
            get { return _OldPortfolioValue; }
            set { _OldPortfolioValue = value; }
        }


        private double _Correlation;

        public double Correlation
        {
            get { return _Correlation; }
            set { _Correlation = value; }
        }

        private double _NewBenchmarkValue;

        public double NewBenchmarkValue
        {
            get { return _NewBenchmarkValue; }
            set { _NewBenchmarkValue = value; }
        }

        private double _OldBenchmarkValue;

        public double OldBenchmarkValue
        {
            get { return _OldBenchmarkValue; }
            set { _OldBenchmarkValue = value; }
        }




    }
}
