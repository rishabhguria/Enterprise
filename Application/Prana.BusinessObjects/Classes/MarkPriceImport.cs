using Csla;
using Csla.Validation;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using System;
using System.ComponentModel;
using System.Text;
namespace Prana.BusinessObjects
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class MarkPriceImport : BusinessBase<MarkPriceImport>
    {
        public MarkPriceImport()
        {

        }

        //public const string VALID = "Validated";

        //public const string INVALID = "NotValidated";

        private string _symbology;

        public string Symbology
        {
            get { return _symbology; }
            set { _symbology = value; }
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

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                PropertyHasChanged("Symbol");
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

        private int _auecID = 0;
        [Browsable(false)]
        public int AUECID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
                PropertyHasChanged("AUECID");
            }
        }

        #region Is Security Approved Status

        public const string APPROVED = "Approved";
        public const string UNAPPROVED = "UnApproved";

        private bool _IsSecApproved = false;
        public bool IsSecApproved
        {
            get
            {
                return _IsSecApproved;
            }
            set
            {
                _IsSecApproved = value;
                if (_IsSecApproved)
                    SecApprovalStatus = APPROVED;
                else
                    SecApprovalStatus = UNAPPROVED;

                PropertyHasChanged(ApplicationConstants.CONST_SEC_APPROVED_STATUS);
            }
        }

        private String _secApprovalStatus;
        public String SecApprovalStatus
        {
            get
            {
                return _secApprovalStatus;
            }
            set
            {
                _secApprovalStatus = value;
                PropertyHasChanged(ApplicationConstants.CONST_SEC_APPROVED_STATUS);
            }
        }

        #endregion


        private string _validationStatus = ApplicationConstants.ValidationStatus.None.ToString();
        public string ValidationStatus
        {
            get { return _validationStatus; }
            set
            {
                _validationStatus = value;
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
        /// will act as a key to the mark price file.
        /// </summary>
        private int _rowIndex;
        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
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

        /// <summary>
        /// Sets the import status of the mark price object
        /// </summary>
        private string _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();

        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        private double _markPrice = 0;
        public double MarkPrice
        {
            get { return _markPrice; }
            set
            {
                _markPrice = value;
            }
        }

        private double _forwardPoints = 0;
        public double ForwardPoints
        {
            get { return _forwardPoints; }
            set
            {
                _forwardPoints = value;
            }
        }


        private string _isForexRequired = string.Empty;
        [Browsable(false)]
        public string IsForexRequired
        {
            get { return _isForexRequired; }
            set
            {
                _isForexRequired = value;
            }
        }

        private string _cusip = string.Empty;
        [Browsable(false)]
        public string CUSIP
        {
            get { return _cusip; }
            set
            {
                _cusip = value;
            }
        }

        private string _sedol = string.Empty;
        [Browsable(false)]
        public string SEDOL
        {
            get { return _sedol; }
            set
            {
                _sedol = value;
            }
        }

        private string _isin = string.Empty;
        [Browsable(false)]
        public string ISIN
        {
            get { return _isin; }
            set
            {
                _isin = value;
            }
        }

        private string _ric = string.Empty;
        [Browsable(false)]
        public string RIC
        {
            get { return _ric; }
            set
            {
                _ric = value;
            }
        }
        private string _bloomberg = string.Empty;
        [Browsable(false)]
        public string Bloomberg
        {
            get { return _bloomberg; }
            set
            {
                _bloomberg = value;
            }
        }

        private string _osiOptionSymbol;
        [Browsable(false)]
        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol;
        [Browsable(false)]
        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }
        private string _opraOptionSymbol = string.Empty;
        [Browsable(false)]
        public string OpraOptionSymbol
        {
            get { return _opraOptionSymbol; }
            set { _opraOptionSymbol = value; }
        }

        private string _pBSymbol = string.Empty;
        public string PBSymbol
        {
            get { return _pBSymbol; }
            set
            {
                _pBSymbol = value;
            }
        }

        string _markPriceImportType = Prana.BusinessObjects.AppConstants.MarkPriceImportType.P.ToString();
        [Browsable(false)]
        public string MarkPriceImportType
        {
            get { return _markPriceImportType; }
            set { _markPriceImportType = value; }
        }

        private string _mismatchType = string.Empty;
        public string MismatchType
        {
            get { return _mismatchType; }
            set
            {
                _mismatchType = value;
            }
        }

        private string _misMatchDetails;
        public string MisMatchDetails
        {
            get { return _misMatchDetails; }
            set { _misMatchDetails = value; }
        }

        //private string _validated = INVALID;
        //public string Validated
        //{
        //    get { return _validated; }
        //    set
        //    {
        //        _validated = value;
        //    }
        //}

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
            return _symbol;
        }

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.SymbolCheck, "Symbol");
            ValidationRules.AddRule(CustomRules.DateCheck, "Date");
            ValidationRules.AddRule(CustomRules.AUECIDCheck, "AUECID");
            ValidationRules.AddRule(CustomRules.IsSecurityApprovedCheck, ApplicationConstants.CONST_SEC_APPROVED_STATUS);
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }


            public static void CheckValidationOnAllFileds(MarkPriceImport finalTarget)
            {

                if (string.IsNullOrEmpty(finalTarget.Symbol) ||
                        string.IsNullOrEmpty(finalTarget.Date) ||
                         finalTarget.AUECID <= 0 || !finalTarget.IsSecApproved ||
                         !NAVLockDateRule.ValidateNAVLockDate(finalTarget.Date))
                {

                    if (finalTarget.AUECID <= 0)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NotExists.ToString();
                    }
                    else if (!finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.UnApproved.ToString();
                    }
                    else if (!NAVLockDateRule.ValidateNAVLockDate(finalTarget.Date))
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                    }
                    else
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();

                        if (string.IsNullOrEmpty(finalTarget.Symbol))
                        {
                            SetOrRemoveValidationError(finalTarget, "Symbol is missing", true);
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Symbol is missing", false);
                        }
                        if (finalTarget.Date.Equals(string.Empty) || finalTarget.Date.Equals(DateTimeConstants.DateTimeMinVal))
                        {
                            SetOrRemoveValidationError(finalTarget, "Date is missing", true);
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Date is missing", false);
                        }
                    }

                }
                else
                {
                    finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                }

            }


            public static bool SymbolCheck(object target, RuleArgs e)
            {
                MarkPriceImport finalTarget = target as MarkPriceImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (string.IsNullOrEmpty(finalTarget.Symbol))
                    {
                        e.Description = "Symbol not validated";
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
                MarkPriceImport finalTarget = target as MarkPriceImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (string.IsNullOrEmpty(finalTarget.Date))
                    {
                        e.Description = "Date required";
                        return false;
                    }else if (!NAVLockDateRule.ValidateNAVLockDate(finalTarget.Date))
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

            public static bool AUECIDCheck(object target, RuleArgs e)
            {
                MarkPriceImport finalTarget = target as MarkPriceImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.AUECID <= 0)
                    {
                        e.Description = "Invalid AUECID"; // "Symbol not validated";
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

            public static bool IsSecurityApprovedCheck(object target, RuleArgs e)
            {
                MarkPriceImport finalTarget = target as MarkPriceImport;
                Boolean isApproved = false;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.SecApprovalStatus.Equals(UNAPPROVED) && finalTarget.AUECID > 0)
                    {
                        e.Description = "Security not Approved";
                        isApproved = false;

                    }
                    else
                    {

                        isApproved = true;
                    }

                }

                return isApproved;
            }

            /// <summary>
            /// Sets or removes validation error in the 
            /// </summary>
            /// <param name="positionMaster"></param>
            /// <param name="message"></param>
            /// <param name="isAddOrRemoveErrorMessage"></param>
            private static void SetOrRemoveValidationError(MarkPriceImport markPrice, string message, bool isAddOrRemoveErrorMessage)
            {
                StringBuilder errorMessage = new StringBuilder(markPrice.ValidationError);
                if (isAddOrRemoveErrorMessage)
                {
                    if (!errorMessage.ToString().Contains(message))
                    {
                        errorMessage.Append(message + Seperators.SEPERATOR_8);
                    }
                }
                else
                {
                    errorMessage.Replace(message + Seperators.SEPERATOR_8, string.Empty);
                }

                markPrice.ValidationError = errorMessage.ToString();
            }

        }
    }
}
