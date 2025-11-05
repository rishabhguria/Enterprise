using System;

namespace Prana.BusinessObjects
{
    public class RiskGrouping
    {
        private String groupName;

        public String GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        private double _risk;
        public double Risk
        {
            get { return _risk; }
            set { _risk = value; }
        }


        private double _correlation;
        public double Correlation
        {
            get { return _correlation; }
            set { _correlation = value; }
        }

    }
}
