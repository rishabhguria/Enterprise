using Csla;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class CompanyStrategyDailyPNL : BusinessBase<CompanyStrategyDailyPNL>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyStrategyDailyPNL"/> class.
        /// </summary>
        public CompanyStrategyDailyPNL()
        {

        }

        private int _companyID = int.MinValue;

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        private int _strategyID = int.MinValue;

        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private string _date;

        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private double _applicationRealizedPNL = 0;

        public double ApplicationRealizedPNL
        {
            get { return _applicationRealizedPNL; }
            set { _applicationRealizedPNL = value; }
        }
        private double _pBRealizedPNL = int.MinValue;

        public double PBRealizedPNL
        {
            get { return _pBRealizedPNL; }
            set { _pBRealizedPNL = value; }
        }

        private double _applicationUnrealizedPNL = 0;

        public double ApplicationUnrealizedPNL
        {
            get { return _applicationUnrealizedPNL; }
            set { _applicationUnrealizedPNL = value; }
        }

        private double _pBUnrealizedPNL = double.MinValue;

        public double PBUnrealizedPNL
        {
            get { return _pBUnrealizedPNL; }
            set { _pBUnrealizedPNL = value; }
        }



        protected override object GetIdValue()
        {
            return _companyID;
        }
    }
}
