using System;

namespace Prana.BusinessObjects
{
    public class LastRevaluationCalculationDate
    {
        private int _ID;

        public virtual int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private DateTime _lastCalcDate;
        public virtual DateTime LastCalcDate
        {
            get { return _lastCalcDate; }
            set { _lastCalcDate = value; }
        }

        private DateTime _lastCalcDateMW;
        public virtual DateTime LastCalcDateMW
        {
            get { return _lastCalcDateMW; }
            set { _lastCalcDateMW = value; }
        }

        public virtual int AccountID { get; set; }

        private DateTime _lastRevalRunDate;
        public virtual DateTime LastRevalRunDate
        {
            get { return _lastRevalRunDate; }
            set { _lastRevalRunDate = value; }
        }

        public virtual int FundID { get; set; }

        public virtual bool UpdatedRevaluation { get; set; }
    }
}
