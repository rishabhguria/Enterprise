using Csla;
using Csla.Validation;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class DailyCreditLimit : BusinessBase<DailyCreditLimit>
    {
        public const string VALID = "Validated";
        public const string INVALID = "NotValidated";

        private int _accountID = 0;
        [Browsable(false)]
        public int AccountID
        {
            get { return _accountID; }
            set
            {
                _accountID = value;
                PropertyHasChanged("AccountID");
            }
        }
        private string _accountName = string.Empty;
        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                PropertyHasChanged("AccountName");
            }
        }
        private double _longDebitLimit = 0;
        [Browsable(false)]
        public double LongDebitLimit
        {
            get { return _longDebitLimit; }
            set { _longDebitLimit = value; }
        }
        private double _shortCreditLimit = 0;
        [Browsable(false)]
        public double ShortCreditLimit
        {
            get { return _shortCreditLimit; }
            set { _shortCreditLimit = value; }
        }
        private double _longDebitBalance = 0;
        public double LongDebitBalance
        {
            get { return _longDebitBalance; }
            set { _longDebitBalance = value; }
        }
        private double _shortCreditBalance = 0;
        public double ShortCreditBalance
        {
            get { return _shortCreditBalance; }
            set { _shortCreditBalance = value; }
        }

        private string _validated = INVALID;
        public string Validated
        {
            get { return _validated; }
            set { _validated = value; }
        }

        private string _validationError = string.Empty;
        public string ValidationError
        {
            get { return _validationError; }
            set
            {
                _validationError = value;
            }
        }

        /// <summary>
        /// will act as a key to the daily credit limit
        /// </summary>
        private int _rowIndex;
        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// Sets the import status of the daily credit limit object
        /// </summary>
        private string _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();

        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        /// <summary>
        /// This Object's hashcode will be the unique id for the object.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _accountID;
        }

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.AccountCheck, "FundName");
            ValidationRules.AddRule(CustomRules.AccountIDCheck, "FundID");
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }

            public static bool AccountCheck(object target, RuleArgs e)
            {
                DailyCreditLimit finalTarget = target as DailyCreditLimit;
                if (finalTarget != null)
                {
                    if (finalTarget.AccountID <= 0 || string.IsNullOrEmpty(finalTarget.AccountName))
                        finalTarget.Validated = INVALID;
                    else
                        finalTarget.Validated = VALID;

                    if (string.IsNullOrEmpty(finalTarget.AccountName))
                    {
                        e.Description = "Account Name required";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool AccountIDCheck(object target, RuleArgs e)
            {
                DailyCreditLimit finalTarget = target as DailyCreditLimit;
                if (finalTarget != null)
                {
                    if (finalTarget.AccountID <= 0 || string.IsNullOrEmpty(finalTarget.AccountName))
                        finalTarget.Validated = INVALID;
                    else
                        finalTarget.Validated = VALID;

                    if (finalTarget.AccountID <= 0)
                    {
                        e.Description = "Account Name Not Validated";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}