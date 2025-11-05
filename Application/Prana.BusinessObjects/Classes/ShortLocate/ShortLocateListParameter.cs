using Prana.LogManager;
using System;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ShortLocateListParameter
    {
        #region members

        //  private bool _check;

        private string _broker;

        private double _borrowQuantity = 0;

        private double _replaceQuantity = 0;

        private double _borrowSharesAvailable = 0;

        private string _borrowerId;

        private double _borrowRate;

        private int _nirvanaLocateID;

        #endregion

        #region Properties

        //public bool Check
        //{
        //    get { return _check; }
        //    set { _check = value; }
        //}

        public int NirvanaLocateID
        {
            get { return _nirvanaLocateID; }
            set { _nirvanaLocateID = value; }
        }

        public string Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }

        public double ReplaceQuantity
        {
            get { return _replaceQuantity; }
            set { _replaceQuantity = value; }
        }
        public double BorrowQuantity
        {
            get { return _borrowQuantity; }
            set { _borrowQuantity = value; }
        }

        public double BorrowSharesAvailable
        {
            get { return _borrowSharesAvailable; }
            set { _borrowSharesAvailable = value; }
        }

        public string BorrowerId
        {
            get { return _borrowerId; }
            set { _borrowerId = value; }
        }

        public double BorrowRate
        {
            get { return _borrowRate; }
            set { _borrowRate = value; }
        }

        #endregion
        public ShortLocateListParameter()
        {
            _broker = string.Empty;
            _borrowQuantity = 0;
            _borrowSharesAvailable = 0;
            _borrowerId = string.Empty;
            _borrowRate = 0;
            _nirvanaLocateID = 0;
            _replaceQuantity = 0;
        }

        public ShortLocateListParameter(string shortLocateParameterString) : this()
        {
            try
            {
                string[] externList = shortLocateParameterString.Split(Seperators.SEPERATOR_5);
                _broker = externList[0].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                _borrowQuantity = double.Parse(externList[1].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _borrowSharesAvailable = double.Parse(externList[2].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _borrowerId = externList[3].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                _borrowRate = double.Parse(externList[4].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _nirvanaLocateID = Int32.Parse(externList[5].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                _replaceQuantity = Int32.Parse(externList[6].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw new Exception("shortLocate string not in correct format", ex);
                }
            }

        }

        public override string ToString()
        {
            StringBuilder shortLocateParameterstr = new StringBuilder();

            shortLocateParameterstr.Append(CustomFIXConstants.CUST_TAG_BorrowBroker);
            shortLocateParameterstr.Append(Seperators.SEPERATOR_6);
            shortLocateParameterstr.Append(this._broker.ToString());
            shortLocateParameterstr.Append(Seperators.SEPERATOR_5);

            shortLocateParameterstr.Append(CustomFIXConstants.CUST_TAG_BorrowQuantity);
            shortLocateParameterstr.Append(Seperators.SEPERATOR_6);
            shortLocateParameterstr.Append(this._borrowQuantity.ToString());
            shortLocateParameterstr.Append(Seperators.SEPERATOR_5);

            shortLocateParameterstr.Append(CustomFIXConstants.CUST_TAG_BorrowSharesAvailable);
            shortLocateParameterstr.Append(Seperators.SEPERATOR_6);
            shortLocateParameterstr.Append(this._borrowSharesAvailable.ToString());
            shortLocateParameterstr.Append(Seperators.SEPERATOR_5);

            shortLocateParameterstr.Append(CustomFIXConstants.CUST_TAG_BorrowerID);
            shortLocateParameterstr.Append(Seperators.SEPERATOR_6);
            shortLocateParameterstr.Append(this._borrowerId.ToString());
            shortLocateParameterstr.Append(Seperators.SEPERATOR_5);

            shortLocateParameterstr.Append(CustomFIXConstants.CUST_TAG_BorrowRate);
            shortLocateParameterstr.Append(Seperators.SEPERATOR_6);
            shortLocateParameterstr.Append(this._borrowRate.ToString());
            shortLocateParameterstr.Append(Seperators.SEPERATOR_5);

            shortLocateParameterstr.Append(CustomFIXConstants.CUST_TAG_NirvanaLocateID);
            shortLocateParameterstr.Append(Seperators.SEPERATOR_6);
            shortLocateParameterstr.Append(this._nirvanaLocateID.ToString());
            shortLocateParameterstr.Append(Seperators.SEPERATOR_5);

            shortLocateParameterstr.Append(CustomFIXConstants.CUST_TAG_ReplaceQuantity);
            shortLocateParameterstr.Append(Seperators.SEPERATOR_6);
            shortLocateParameterstr.Append(this._replaceQuantity.ToString());
            shortLocateParameterstr.Append(Seperators.SEPERATOR_5);

            return shortLocateParameterstr.ToString();
        }
    }
}
