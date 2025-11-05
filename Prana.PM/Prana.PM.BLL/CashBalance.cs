using Csla;
using Prana.BusinessObjects.PositionManagement;
using System;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    [Serializable]
    public class CashBalance : BusinessBase<CashBalance>
    {
        public CashBalance()
        {
            MarkAsChild();
        }

        private int _accountID;
        [Browsable(false)]
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private Account _account;

        /// <summary>
        /// Gets or sets the account value.
        /// </summary>
        /// <value>The account value.</value>
        public Account AccountValue
        {
            get
            {
                if (_account == null)
                {
                    _account = new Account();
                }
                return _account;
            }
            set { _account = value; }
        }

        private double _previousBalance;

        //TODO: To remove the following property. BB
        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price.</value>
        public double PreviousBalance
        {
            get { return _previousBalance; }
            set { _previousBalance = value; }
        }

        private double _currentBalance;

        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price.</value>
        public double CurrentBalance
        {
            get { return _currentBalance; }
            set { _currentBalance = value; }
        }

        private DateTime _date;
        [Browsable(false)]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private ThirdPartyNameID _datasourceNameId;
        [Browsable(false)]
        public ThirdPartyNameID DatasourceNameId
        {
            get
            {
                if (_datasourceNameId == null)
                {
                    _datasourceNameId = new ThirdPartyNameID();
                }
                return _datasourceNameId;
            }
            set { _datasourceNameId = value; }
        }

        private Prana.BusinessObjects.Currency _currency;
        /// <summary>
        /// Gets or sets the currency value.
        /// </summary>
        /// <value>The currency value.</value>
        public Prana.BusinessObjects.Currency CurrencyValue
        {
            get
            {
                if (_currency == null)
                {
                    _currency = new Prana.BusinessObjects.Currency();
                }
                return _currency;
            }
            set { _currency = value; }
        }

        private string _comments;
        /// <summary>
        /// Gets or sets the comments value.
        /// </summary>
        /// <value>The comments value.</value>
        public string Comments
        {
            get
            {
                return _comments;
            }
            set { _comments = value; }
        }

        // dummy
        //int _id;
        protected override object GetIdValue()
        {
            return 0;
        }

    }
}
