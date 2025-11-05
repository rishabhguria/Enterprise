using Csla;
using Csla.Validation;
using System.ComponentModel;
namespace Prana.BusinessObjects
{
    //Created By: Pooja Porwal
    //Date:12 Feb 2015
    //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-5820

    [System.Runtime.InteropServices.ComVisible(false)]
    public class SettlementDateCashCurrencyValue : BusinessBase<SettlementDateCashCurrencyValue>
    {

        public SettlementDateCashCurrencyValue()
        {

        }

        public const string VALID = "Validated";

        public const string INVALID = "NotValidated";

        #region Cash and Currency

        private string _settlementDateBaseCurrency = string.Empty;

        public string SettlementDateBaseCurrency
        {
            get
            {
                return _settlementDateBaseCurrency;
            }
            set
            {
                _settlementDateBaseCurrency = value;
            }
        }

        private string _settlementDateLocalCurrency = string.Empty;

        public string SettlementDateLocalCurrency
        {
            get { return _settlementDateLocalCurrency; }
            set
            {
                _settlementDateLocalCurrency = value;
            }
        }

        private double _settlementDateCashValueBase = 0;

        public double SettlementDateCashValueBase
        {
            get { return _settlementDateCashValueBase; }
            set
            {
                _settlementDateCashValueBase = value;
            }
        }

        private double _settlementDateCashValueLocal = 0;

        public double SettlementDateCashValueLocal
        {
            get { return _settlementDateCashValueLocal; }
            set
            {
                _settlementDateCashValueLocal = value;
            }
        }

        private string _settlementDate = string.Empty;
        public string SettlementDate
        {
            get { return _settlementDate; }
            set
            {
                _settlementDate = value;
                PropertyHasChanged("SettlementDate");
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


        private string _positionType = string.Empty;
        public string PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }


        private int __settlementDateLocalCurrencyID = 0;
        [Browsable(false)]
        public int SettlementDateLocalCurrencyID
        {
            get { return __settlementDateLocalCurrencyID; }
            set
            {
                __settlementDateLocalCurrencyID = value;
                PropertyHasChanged("SettlementDateLocalCurrencyID");
            }
        }


        private int _settlementDateBaseCurrencyID = 0;
        [Browsable(false)]
        public int SettlementDateBaseCurrencyID
        {
            get { return _settlementDateBaseCurrencyID; }
            set
            {
                _settlementDateBaseCurrencyID = value;
                PropertyHasChanged("SettlementDateBaseCurrencyID");
            }
        }

        private string _validated = INVALID;
        public string Validated
        {
            get { return _validated; }
            set
            {
                _validated = value;
            }
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
        /// will act as a key to the cash currency value
        /// </summary>
        private int _rowIndex;
        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// Sets the import status of the cash currency value object
        /// </summary>
        private string _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();

        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        private double _forexConversionRate = 0;
        [Browsable(false)]
        public double ForexConversionRate
        {
            get { return _forexConversionRate; }
            set
            {
                _forexConversionRate = value;
                PropertyHasChanged("ForexConversionRate");
            }
        }

        #endregion

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
            return _settlementDateBaseCurrencyID;
        }

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.BaseCurrencyCheck, "SettlementDateBaseCurrencyID");
            ValidationRules.AddRule(CustomRules.LocalCurrencyCheck, "SettlementDateLocalCurrencyID");
            ValidationRules.AddRule(CustomRules.DateCheck, "SettlementDate");
            ValidationRules.AddRule(CustomRules.ForexConversionRateCheck, "ForexConversionRate");
            ValidationRules.AddRule(CustomRules.AccountCheck, "AccountName");
            ValidationRules.AddRule(CustomRules.AccountIDCheck, "AccountID");
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }

            public static bool BaseCurrencyCheck(object target, RuleArgs e)
            {
                SettlementDateCashCurrencyValue finalTarget = target as SettlementDateCashCurrencyValue;
                if (finalTarget != null)
                {
                    if (finalTarget.SettlementDateBaseCurrencyID <= 0 ||
                        finalTarget.SettlementDateLocalCurrencyID <= 0 ||
                        finalTarget.AccountID <= 0 ||
                        finalTarget.AccountName.Equals(string.Empty) ||
                        string.IsNullOrEmpty(finalTarget.SettlementDate))
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (finalTarget.SettlementDateBaseCurrencyID <= 0)
                    {
                        e.Description = "Settlement Date Base Currency Not Validated";
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
            public static bool LocalCurrencyCheck(object target, RuleArgs e)
            {
                SettlementDateCashCurrencyValue finalTarget = target as SettlementDateCashCurrencyValue;
                if (finalTarget != null)
                {
                    if (finalTarget.SettlementDateBaseCurrencyID <= 0 ||
                        finalTarget.SettlementDateLocalCurrencyID <= 0 ||
                        finalTarget.AccountID <= 0 ||
                        finalTarget.AccountName.Equals(string.Empty) ||
                        string.IsNullOrEmpty(finalTarget.SettlementDate))
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (finalTarget.SettlementDateLocalCurrencyID <= 0)
                    {
                        e.Description = "Local Currency Not Validated";
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
                SettlementDateCashCurrencyValue finalTarget = target as SettlementDateCashCurrencyValue;
                if (finalTarget != null)
                {
                    if (finalTarget.SettlementDateBaseCurrencyID <= 0 ||
                        finalTarget.SettlementDateLocalCurrencyID <= 0 ||
                        finalTarget.AccountID <= 0 ||
                        finalTarget.AccountName.Equals(string.Empty) ||
                        string.IsNullOrEmpty(finalTarget.SettlementDate))
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

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

            public static bool DateCheck(object target, RuleArgs e)
            {
                SettlementDateCashCurrencyValue finalTarget = target as SettlementDateCashCurrencyValue;
                if (finalTarget != null)
                {
                    if (finalTarget.SettlementDateBaseCurrencyID <= 0 ||
                        finalTarget.SettlementDateLocalCurrencyID <= 0 ||
                        finalTarget.AccountID <= 0 ||
                        finalTarget.AccountName.Equals(string.Empty) ||
                        string.IsNullOrEmpty(finalTarget.SettlementDate))
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (string.IsNullOrEmpty(finalTarget.SettlementDate))
                    {
                        e.Description = "Settlement Date required";
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
            public static bool AccountCheck(object target, RuleArgs e)
            {
                SettlementDateCashCurrencyValue finalTarget = target as SettlementDateCashCurrencyValue;
                if (finalTarget != null)
                {
                    if (finalTarget.SettlementDateBaseCurrencyID <= 0 ||
                        finalTarget.SettlementDateLocalCurrencyID <= 0 ||
                        finalTarget.AccountID <= 0 ||
                        finalTarget.AccountName.Equals(string.Empty) ||
                        string.IsNullOrEmpty(finalTarget.SettlementDate))
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (finalTarget.AccountName.Equals(string.Empty))
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

            public static bool ForexConversionRateCheck(object target, RuleArgs e)
            {
                SettlementDateCashCurrencyValue finalTarget = target as SettlementDateCashCurrencyValue;
                if (finalTarget != null)
                {
                    if (finalTarget.ForexConversionRate <= 0)
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (finalTarget.ForexConversionRate <= 0)
                    {
                        e.Description = "Forex Conversion Rate required for the date";
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
