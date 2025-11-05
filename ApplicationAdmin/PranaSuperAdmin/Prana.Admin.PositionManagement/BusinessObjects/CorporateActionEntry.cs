using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CorporateActionEntry
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }


        private CorporateActionNameID _corporateActionNameID;

        /// <summary>
        /// Gets or sets the corporate action name ID.
        /// </summary>
        /// <value>The corporate action name ID.</value>
        public CorporateActionNameID CorporateActionNameID
        {
            get
            {
                if (_corporateActionNameID == null)
                {
                    _corporateActionNameID = new CorporateActionNameID();  
                }
                return _corporateActionNameID;
            }
            set { _corporateActionNameID = value; }
        }
	

        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new DataSourceNameID();
                }
                return _dataSourceNameID; }
            set { _dataSourceNameID = value; }
        }


        private Fund _fund;

        /// <summary>
        /// Gets or sets the fund.
        /// </summary>
        /// <value>The fund.</value>
        public Fund Fund
        {
            get {
                if (_fund == null)
                {
                    _fund = new Fund();
                }
                return _fund; }
            set { _fund = value; }
        }

        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get {
                //if (_symbol ==null)
                //{
                //    _symbol = new Symbol();
                //}
                return _symbol;
            }
            set { _symbol = value; }
        }
	
        private Currency _currency;

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public Currency Currency
        {
            get 
            {
                if (_currency == null)
                {
                    _currency = new Currency();
                }
                return _currency; 
            }
            set { _currency = value; }
        }

        private double _amount= double.MinValue;

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private CorporateActionType _corporateActionType;

        /// <summary>
        /// Gets or sets the type of the corporate action.
        /// </summary>
        /// <value>The type of the corporate action.</value>
        public CorporateActionType CorporateActionType
        {
            get 
            {
                if (_corporateActionType == null)
                {
                    _corporateActionType = new CorporateActionType();
                }
                return _corporateActionType; 
            }
            set { _corporateActionType = value; }
        }

        private DateTime _paymentDate;

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        /// <value>The payment date.</value>
        public DateTime PaymentDate
        {
            get { return _paymentDate; }
            set { _paymentDate = value; }
        }

        private DateTime _transactionDate;

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        /// <value>The transaction date.</value>
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
        }

        private ImpactOnCash _impactOnCash;

        /// <summary>
        /// Gets or sets a value indicating whether [impact on cash].
        /// </summary>
        /// <value><c>true</c> if [impact on cash]; otherwise, <c>false</c>.</value>
        public ImpactOnCash ImpactOnCash
        {
            get { return _impactOnCash; }
            set { _impactOnCash = value; }
        }
	
    }
}
