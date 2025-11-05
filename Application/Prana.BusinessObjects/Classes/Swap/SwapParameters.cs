using Prana.LogManager;
using System;
using System.Data;
using System.Text;

namespace Prana.BusinessObjects
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


        public SwapParameters()
        {
            _firstResetDate = DateTimeConstants.MinValue;
            _resetFrequency = string.Empty;
            _origTransDate = DateTimeConstants.MinValue;
            _swapDescription = string.Empty;
            _closingDate = DateTimeConstants.MinValue;
            _transDate = DateTimeConstants.MinValue;
        }
        public SwapParameters(DataRow dr)
        {
            _groupID = dr["GroupID"].ToString();
            _notionalValue = Convert.ToDouble(dr["NotionalValue"]);
            _dayCount = Convert.ToInt32(dr["DayCount"]);
            _benchMarkRate = Convert.ToDouble(dr["BenchMarkRate"]);
            _differential = Convert.ToDouble(dr["Differential"]);
            _firstResetDate = Convert.ToDateTime(dr["FirstResetDate"]);
            _resetFrequency = dr["ResetFrequency"].ToString();
            _origTransDate = Convert.ToDateTime(dr["OrigTransDate"]);
            _origCostBasis = Convert.ToDouble(dr["OrigCostBasis"]);
            _swapDescription = dr["SwapDescription"].ToString();
            _closingPrice = Convert.ToDouble(dr["ClosingPrice"]);
            _closingDate = Convert.ToDateTime(dr["ClosingDate"]);
            _transDate = Convert.ToDateTime(dr["TransDate"]);
        }

        public SwapParameters(string swapString) : this()
        {
            try
            {
                string[] externList = swapString.Split(Seperators.SEPERATOR_5);
                _notionalValue = double.Parse(externList[0].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _dayCount = int.Parse(externList[1].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _benchMarkRate = double.Parse(externList[2].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _differential = double.Parse(externList[3].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _firstResetDate = DateTime.Parse(externList[4].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _resetFrequency = externList[5].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                _origTransDate = DateTime.Parse(externList[6].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _origCostBasis = double.Parse(externList[7].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _swapDescription = externList[8].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                _closingDate = DateTime.Parse(externList[9].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _closingPrice = double.Parse(externList[10].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _transDate = DateTime.Parse(externList[11].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw new Exception("swap string not in correct format", ex);
                }
            }

        }


        public override string ToString()
        {
            StringBuilder swapstr = new StringBuilder();

            swapstr.Append(CustomFIXConstants.CUST_TAG_NotionalValue);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._notionalValue.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_DayCount);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._dayCount.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_BenchMarkRate);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._benchMarkRate.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_Differential);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._differential.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_FirstResetDate);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._firstResetDate.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_ResetFrequency);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._resetFrequency.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_OrigTransDate);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._origTransDate.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_OrigCostBasis);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._origCostBasis.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_SwapDescription);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._swapDescription.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_SwapClosingDate);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._closingDate.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_SwapClosingPrice);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._closingPrice.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            swapstr.Append(CustomFIXConstants.CUST_TAG_SwapTransDate);
            swapstr.Append(Seperators.SEPERATOR_6);
            swapstr.Append(this._transDate.ToString());
            swapstr.Append(Seperators.SEPERATOR_5);

            return swapstr.ToString();
        }

        public virtual SwapParameters Clone()
        {
            SwapParameters clonedSwapParams = new SwapParameters();
            clonedSwapParams.NotionalValue = this._notionalValue;
            clonedSwapParams.DayCount = this._dayCount;
            clonedSwapParams.BenchMarkRate = this._benchMarkRate;
            clonedSwapParams.Differential = this._differential;
            clonedSwapParams.FirstResetDate = this._firstResetDate;
            clonedSwapParams.ResetFrequency = this._resetFrequency;
            clonedSwapParams.OrigTransDate = this._origTransDate;
            clonedSwapParams.OrigCostBasis = this._origCostBasis;
            clonedSwapParams.SwapDescription = this._swapDescription;
            clonedSwapParams.ClosingPrice = this._closingPrice;
            clonedSwapParams.ClosingDate = this._closingDate;
            clonedSwapParams.TransDate = this._transDate;
            //update group ID for clone object, PRANA-13092
            clonedSwapParams.GroupID = this._groupID;
            return clonedSwapParams;
        }

    }
}
