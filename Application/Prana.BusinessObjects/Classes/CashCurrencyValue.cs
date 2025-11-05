using Csla;
using Csla.Validation;
using Prana.BusinessObjects.Classes;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class CashCurrencyValue : BusinessBase<PositionMaster>
    {

        public CashCurrencyValue()
        {

        }

        public const string VALID = "Validated";

        public const string INVALID = "NotValidated";

        #region Cash and Currency

        private string _baseCurrency = string.Empty;

        public string BaseCurrency
        {
            get { return _baseCurrency; }
            set
            {
                _baseCurrency = value;
            }
        }

        private string _localCurrency = string.Empty;

        public string LocalCurrency
        {
            get { return _localCurrency; }
            set
            {
                _localCurrency = value;
            }
        }

        private double _cashValueBase = 0;

        public double CashValueBase
        {
            get { return _cashValueBase; }
            set
            {
                _cashValueBase = value;
                //PropertyHasChanged("CashValueBase");
            }
        }

        private double _cashValueLocal = 0;

        public double CashValueLocal
        {
            get { return _cashValueLocal; }
            set
            {
                _cashValueLocal = value;
                //PropertyHasChanged("CashValueLocal");
            }
        }

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


        private int _localCurrencyID = 0;
        [Browsable(false)]
        public int LocalCurrencyID
        {
            get { return _localCurrencyID; }
            set
            {
                _localCurrencyID = value;
                PropertyHasChanged("LocalCurrencyID");
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

        string _dataAdjustReq = string.Empty;
        /// <summary>
        /// it is a special case,whenever we need to adjust Base and Local currency values , it is require to add in the 
        /// XSLT file else the work flow is as usual,its value Yes or No
        /// </summary>
        [Browsable(false)]
        public string DataAdjustReq
        {
            get { return _dataAdjustReq; }
            set { _dataAdjustReq = value; }
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
            return _baseCurrencyID;
        }

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.BaseCurrencyCheck, "BaseCurrencyID");
            ValidationRules.AddRule(CustomRules.LocalCurrencyCheck, "LocalCurrencyID");
            //ValidationRules.AddRule(CustomRules.CashValueBaseCheck, "CashValueBase");
            //ValidationRules.AddRule(CustomRules.CashValueLocalCheck, "CashValueLocal");
            ValidationRules.AddRule(CustomRules.DateCheck, "Date");
            ValidationRules.AddRule(CustomRules.ForexConversionRateCheck, "ForexConversionRate");
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

            public static void CheckValidationOnAllFileds(CashCurrencyValue finalTarget)
            {
                if (finalTarget.BaseCurrencyID <= 0 ||
                        finalTarget.LocalCurrencyID <= 0 ||
                        finalTarget.AccountID <= 0 ||
                        finalTarget.AccountName.Equals(string.Empty) ||
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
                CashCurrencyValue finalTarget = target as CashCurrencyValue;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.BaseCurrencyID <= 0)
                    {
                        e.Description = "Base Currency Not Validated";
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
                CashCurrencyValue finalTarget = target as CashCurrencyValue;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.LocalCurrencyID <= 0)
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
                CashCurrencyValue finalTarget = target as CashCurrencyValue;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

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
            //public static bool CashValueBaseCheck(object target, RuleArgs e)
            //{
            //    CashCurrencyValue finalTarget = target as CashCurrencyValue;
            //    if (finalTarget != null)
            //    {
            //        if (finalTarget.BaseCurrencyID <= 0 ||
            //            finalTarget.LocalCurrencyID <= 0 || 
            //            finalTarget.AccountName.Equals(string.Empty))
            //        {
            //            finalTarget.Validated = INVALID;
            //        }
            //        else
            //        {
            //            finalTarget.Validated = VALID;
            //        }

            //        if (finalTarget.CashValueBase == 0)
            //        {
            //            e.Description = "Invalid Cash Value Base";
            //            return false;
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //public static bool CashValueLocalCheck(object target, RuleArgs e)
            //{
            //    CashCurrencyValue finalTarget = target as CashCurrencyValue;
            //    if (finalTarget != null)
            //    {
            //        if (finalTarget.BaseCurrencyID <= 0 ||
            //            finalTarget.LocalCurrencyID <= 0 || 
            //            finalTarget.CashValueBase.Equals(ApplicationConstants.CONST_ZERO) ||
            //            finalTarget.CashValueLocal.Equals(ApplicationConstants.CONST_ZERO) || 
            //            finalTarget.AccountName.Equals(string.Empty))
            //        {
            //            finalTarget.Validated = INVALID;
            //        }
            //        else
            //        {
            //            finalTarget.Validated = VALID;
            //        }

            //        if (finalTarget.CashValueLocal == 0)
            //        {
            //            e.Description = "Invalid Cash Value Local";
            //            return false;
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            public static bool DateCheck(object target, RuleArgs e)
            {
                CashCurrencyValue finalTarget = target as CashCurrencyValue;
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
            public static bool AccountCheck(object target, RuleArgs e)
            {
                CashCurrencyValue finalTarget = target as CashCurrencyValue;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

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
                CashCurrencyValue finalTarget = target as CashCurrencyValue;
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
