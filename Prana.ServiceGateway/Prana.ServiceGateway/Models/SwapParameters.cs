using System.Data;
using System.Text;

namespace Prana.ServiceGateway.Models
{
    [Serializable]
    public class SwapParameters
    {
        #region properties
        private double _notionalValue;
        private int _dayCount;
        private double _benchMarkRate;
        private double _differential;
        private DateTime _firstResetDate;
        private string _resetFrequency;
        private DateTime _origTransDate;
        private double _origCostBasis;
        private string _swapDescription;
        private double _closingPrice;
        private DateTime _closingDate;
        private DateTime _transDate;
        private int _swapPK;
        string _groupID = string.Empty;

        public virtual int SwapPK
        {
            set { _swapPK = value; }
            get { return _swapPK; }
        }

        public virtual string GroupID
        {
            set { _groupID = value; }
            get { return _groupID; }
        }

        public virtual double NotionalValue
        {
            get { return _notionalValue; }
            set { _notionalValue = value; }
        }
        public virtual int DayCount
        {
            get { return _dayCount; }
            set { _dayCount = value; }
        }
        public virtual double BenchMarkRate
        {
            get { return _benchMarkRate; }
            set { _benchMarkRate = value; }
        }
        public virtual double Differential
        {
            get { return _differential; }
            set { _differential = value; }
        }
        public virtual DateTime FirstResetDate
        {
            get { return _firstResetDate; }
            set { _firstResetDate = value; }
        }
        public virtual string ResetFrequency
        {
            get { return _resetFrequency; }
            set { _resetFrequency = value; }
        }
        /// <summary>
        /// The creation date of original Swap of which this is a rollover
        /// </summary>
        public virtual DateTime OrigTransDate
        {
            get { return _origTransDate; }
            set { _origTransDate = value; }
        }
        public virtual double OrigCostBasis
        {
            get { return _origCostBasis; }
            set { _origCostBasis = value; }
        }
        public virtual string SwapDescription
        {
            get { return _swapDescription; }
            set { _swapDescription = value; }
        }
        public virtual double ClosingPrice
        {
            get { return _closingPrice; }
            set { _closingPrice = value; }
        }
        public virtual DateTime ClosingDate
        {
            get { return _closingDate; }
            set { _closingDate = value; }
        }
        /// <summary>
        /// The creation date for this Swap, On reset date this == Reset Date
        /// </summary>
        public virtual DateTime TransDate
        {
            get { return _transDate; }
            set { _transDate = value; }
        }
        #endregion
    }
}
