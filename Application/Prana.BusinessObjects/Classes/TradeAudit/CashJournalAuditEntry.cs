using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.Classes.TradeAudit
{
    [Serializable]
    public class CashJournalAuditEntry
    {
        #region Members

        /// <summary>
        /// The transaction date
        /// </summary>
        private DateTime _transactionDate = DateTimeConstants.MinValue;

        /// <summary>
        /// The currency name
        /// </summary>
        private string _currencyName = string.Empty;

        /// <summary>
        /// The account identifier
        /// </summary>
        private int _accountId = 0;

        /// <summary>
        /// The comments
        /// </summary>
        private string _comments = string.Empty;

        /// <summary>
        /// The user identifier
        /// </summary>
        private int _userId = 0;

        /// <summary>
        /// The action date
        /// </summary>
        private DateTime _actionDate = DateTimeConstants.MinValue;

        /// <summary>
        /// The cash account identifier
        /// </summary>
        private int _cashAccountId = 0;

        /// <summary>
        /// The symbol
        /// </summary>
        private string _symbol = string.Empty;

        /// <summary>
        /// The c r
        /// </summary>
        private decimal _cR = 0.0M;

        /// <summary>
        /// The d r
        /// </summary>
        private decimal _dR = 0.0M;

        /// <summary>
        /// The f x rate
        /// </summary>
        private double _fXRate = 0.0;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        /// <summary>
        /// Gets or sets the action date.
        /// </summary>
        /// <value>
        /// The action date.
        /// </value>
        public DateTime ActionDate
        {
            get { return _actionDate; }
            set { _actionDate = value; }
        }

        /// <summary>
        /// Gets or sets the cash account identifier.
        /// </summary>
        /// <value>
        /// The cash account identifier.
        /// </value>
        public int CashAccountId
        {
            get { return _cashAccountId; }
            set { _cashAccountId = value; }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        /// <summary>
        /// Gets or sets the cr.
        /// </summary>
        /// <value>
        /// The cr.
        /// </value>
        public decimal CR
        {
            get { return _cR; }
            set { _cR = value; }
        }

        /// <summary>
        /// Gets or sets the name of the currency.
        /// </summary>
        /// <value>
        /// The name of the currency.
        /// </value>
        public string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }

        /// <summary>
        /// Gets or sets the dr.
        /// </summary>
        /// <value>
        /// The dr.
        /// </value>
        public decimal DR
        {
            get { return _dR; }
            set { _dR = value; }
        }

        /// <summary>
        /// Gets or sets the fx rate.
        /// </summary>
        /// <value>
        /// The fx rate.
        /// </value>
        public double FXRate
        {
            get { return _fXRate; }
            set { _fXRate = value; }
        }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        /// <value>
        /// The transaction date.
        /// </value>
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CashJournalAuditEntry"/> class.
        /// </summary>
        /// <param name="transactionDate">The transaction date.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="comments">The comments.</param>
        /// <param name="userId">The user identifier.</param>
        public CashJournalAuditEntry(DateTime transactionDate, string currency, int accountId, string comments, int userId, int cashAccountId, string symbol, decimal cr, decimal dr, double fxRate)
        {
            try
            {
                _transactionDate = transactionDate;
                _currencyName = currency;
                _accountId = accountId;
                _comments = comments;
                _actionDate = DateTime.Now;
                _userId = userId;
                _cashAccountId = cashAccountId;
                _symbol = symbol;
                _cR = cr;
                _dR = dr;
                _fXRate = fxRate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors
    }
}
