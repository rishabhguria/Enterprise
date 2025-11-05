using Csla;
using Csla.Validation;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class ForexPriceImport : BusinessBase<ForexPriceImport>
    {
        public ForexPriceImport()
        {

        }

        public const string VALID = "Validated";

        public const string INVALID = "NotValidated";

        private string _Date = string.Empty;
        public string Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                PropertyHasChanged("Date");
            }
        }


        private string _baseCurrency = string.Empty;

        public string BaseCurrency
        {
            get { return _baseCurrency; }
            set
            {
                _baseCurrency = value;
            }
        }

        private int _baseCurrencyID = 0;
        [Browsable(false)]
        public int BaseCurrencyID
        {
            get { return _baseCurrencyID; }
            set
            {
                _baseCurrencyID = value;
                PropertyHasChanged("BaseCurrencyID");
            }
        }

        private string _settlementCurrency = string.Empty;

        public string SettlementCurrency
        {
            get { return _settlementCurrency; }
            set
            {
                _settlementCurrency = value;
            }
        }

        private int _settlementCurrencyID = 0;
        [Browsable(false)]
        public int SettlementCurrencyID
        {
            get { return _settlementCurrencyID; }
            set
            {
                _settlementCurrencyID = value;
                PropertyHasChanged(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
            }
        }

        private double _forexPrice = 0;
        public double ForexPrice
        {
            get { return _forexPrice; }
            set
            {
                _forexPrice = value;
            }
        }

        private int _accountID = 0;
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

        // for saving pricing source
        private int _source = 0;
        public int Source
        {
            get { return _source; }
            set
            {
                _source = value;
                PropertyHasChanged("Source");
            }
        }

        private Prana.BusinessObjects.AppConstants.Operator _fxConversionMethodOperator = Prana.BusinessObjects.AppConstants.Operator.M;
        //string _fxConversionMethodOperator = Prana.BusinessObjects.AppConstants.Operator.M.ToString();

        public Prana.BusinessObjects.AppConstants.Operator FXConversionMethodOperator
        {
            get { return _fxConversionMethodOperator; }
            set
            {
                _fxConversionMethodOperator = value;
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
        /// will act as a key to the forex import
        /// </summary>
        private int _rowIndex;
        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// Sets the import status of the forex import object
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
        /// This Object's hashcode will be the unique id for the object.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _baseCurrencyID;
        }

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.BaseCurrencyCheck, "BaseCurrencyID");
            ValidationRules.AddRule(CustomRules.SettlementCurrencyCheck, OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
            ValidationRules.AddRule(CustomRules.DateCheck, "Date");
            ValidationRules.AddRule(CustomRules.ForexPriceCheck, "Date");
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {

            }

            public static void CheckValidationOnAllFileds(ForexPriceImport finalTarget)
            {
                if (finalTarget.BaseCurrencyID <= 0 ||
                        finalTarget.SettlementCurrencyID <= 0 ||
                       finalTarget.ForexPrice.Equals(ApplicationConstants.CONST_ZERO) ||
                       string.IsNullOrEmpty(finalTarget.Date) ||
                       !NAVLockDateRule.ValidateNAVLockDate(finalTarget.Date))
                {
                    finalTarget.Validated = INVALID;
                }
                else
                {
                    finalTarget.Validated = VALID;
                }
            }

            public static bool BaseCurrencyCheck(object target, RuleArgs e)
            {
                ForexPriceImport finalTarget = target as ForexPriceImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.BaseCurrencyID <= 0)
                    {
                        e.Description = "From Currency Not Validated";
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
            public static bool SettlementCurrencyCheck(object target, RuleArgs e)
            {
                ForexPriceImport finalTarget = target as ForexPriceImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.SettlementCurrencyID <= 0)
                    {
                        e.Description = "To Currency Not Validated";
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

            public static bool ForexPriceCheck(object target, RuleArgs e)
            {
                ForexPriceImport finalTarget = target as ForexPriceImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.ForexPrice == 0)
                    {
                        e.Description = "Invalid Forex Price";
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
                ForexPriceImport finalTarget = target as ForexPriceImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (string.IsNullOrEmpty(finalTarget.Date))
                    {
                        e.Description = "Date required";
                        return false;
                    }
                    else if (!NAVLockDateRule.ValidateNAVLockDate(finalTarget.Date))
                    {
                        e.Description = "The date you’ve chosen for this action precedes your NAV Lock date (" + NAVLockDateRule.NAVLockDate.Value.ToShortDateString() + "). Please reach out to your Support Team for further assistance.";
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

        //added by: Bharat raturi, 28 may 2013
        //purpose: add new property for getting validation status
        private string _validationStatus = ApplicationConstants.ValidationStatus.None.ToString();
        public string ValidationStatus
        {
            get { return _validationStatus; }
            set
            {
                _validationStatus = value;
            }
        }
    }
}
