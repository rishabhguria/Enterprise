using Csla;
using Csla.Validation;
using Prana.Global;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public class DividendYieldImport : BusinessBase<DividendYieldImport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DividendYieldImport"/> class.
        /// </summary>
        public DividendYieldImport()
        {
        }

        /// <summary>
        /// The _symbology
        /// </summary>
        private string _symbology;

        /// <summary>
        /// Gets or sets the symbology.
        /// </summary>
        /// <value>
        /// The symbology.
        /// </value>
        public string Symbology
        {
            get { return _symbology; }
            set { _symbology = value; }
        }

        /// <summary>
        /// The _ date
        /// </summary>
        private string _Date = string.Empty;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public string Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                PropertyHasChanged("Date");
            }
        }

        /// <summary>
        /// The _symbol
        /// </summary>
        private string _symbol = string.Empty;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                PropertyHasChanged("Symbol");
            }
        }

        /// <summary>
        /// The _auec identifier
        /// </summary>
        private int _auecID = 0;

        /// <summary>
        /// Gets or sets the auecid.
        /// </summary>
        /// <value>
        /// The auecid.
        /// </value>
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

        /// <summary>
        /// The approved
        /// </summary>
        public const string APPROVED = "Approved";
        /// <summary>
        /// The unapproved
        /// </summary>
        public const string UNAPPROVED = "UnApproved";


        /// <summary>
        /// The _validation status
        /// </summary>
        private string _validationStatus = ApplicationConstants.ValidationStatus.None.ToString();

        /// <summary>
        /// Gets or sets the validation status.
        /// </summary>
        /// <value>
        /// The validation status.
        /// </value>
        public string ValidationStatus
        {
            get { return _validationStatus; }
            set { _validationStatus = value; }
        }

        /// <summary>
        /// The _validation error
        /// </summary>
        private string _validationError = string.Empty;

        /// <summary>
        /// Gets or sets the validation error.
        /// </summary>
        /// <value>
        /// The validation error.
        /// </value>
        public string ValidationError
        {
            get { return _validationError; }
            set { _validationError = value; }
        }

        /// <summary>
        /// will act as a key to the Dividend Yield import
        /// </summary>
        private int _rowIndex;

        /// <summary>
        /// Gets or sets the index of the row.
        /// </summary>
        /// <value>
        /// The index of the row.
        /// </value>
        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// Sets the import status of the Dividend Yield import object
        /// </summary>
        private string _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();

        /// <summary>
        /// Gets or sets the import status.
        /// </summary>
        /// <value>
        /// The import status.
        /// </value>
        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        /// <summary>
        /// The _dividend yield price
        /// </summary>
        private double _dividendYieldPrice = 0;

        /// <summary>
        /// Gets or sets the dividend yield.
        /// </summary>
        /// <value>
        /// The dividend yield.
        /// </value>
        public double DividendYield
        {
            get { return _dividendYieldPrice; }
            set { _dividendYieldPrice = value; }
        }

        /// <summary>
        /// The _cusip
        /// </summary>
        private string _cusip = string.Empty;

        /// <summary>
        /// Gets or sets the cusip.
        /// </summary>
        /// <value>
        /// The cusip.
        /// </value>
        [Browsable(false)]
        public string CUSIP
        {
            get { return _cusip; }
            set { _cusip = value; }
        }

        /// <summary>
        /// The _sedol
        /// </summary>
        private string _sedol = string.Empty;

        /// <summary>
        /// Gets or sets the sedol.
        /// </summary>
        /// <value>
        /// The sedol.
        /// </value>
        [Browsable(false)]
        public string SEDOL
        {
            get { return _sedol; }
            set { _sedol = value; }
        }

        /// <summary>
        /// The _isin
        /// </summary>
        private string _isin = string.Empty;

        /// <summary>
        /// Gets or sets the isin.
        /// </summary>
        /// <value>
        /// The isin.
        /// </value>
        [Browsable(false)]
        public string ISIN
        {
            get { return _isin; }
            set { _isin = value; }
        }

        /// <summary>
        /// The _ric
        /// </summary>
        private string _ric = string.Empty;

        /// <summary>
        /// Gets or sets the ric.
        /// </summary>
        /// <value>
        /// The ric.
        /// </value>
        [Browsable(false)]
        public string RIC
        {
            get { return _ric; }
            set { _ric = value; }
        }

        /// <summary>
        /// The _bloomberg
        /// </summary>
        private string _bloomberg = string.Empty;

        /// <summary>
        /// Gets or sets the bloomberg.
        /// </summary>
        /// <value>
        /// The bloomberg.
        /// </value>
        [Browsable(false)]
        public string Bloomberg
        {
            get { return _bloomberg; }
            set { _bloomberg = value; }
        }

        /// <summary>
        /// The _osi option symbol
        /// </summary>
        private string _osiOptionSymbol;

        /// <summary>
        /// Gets or sets the osi option symbol.
        /// </summary>
        /// <value>
        /// The osi option symbol.
        /// </value>
        [Browsable(false)]
        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        /// <summary>
        /// The _idco option symbol
        /// </summary>
        private string _idcoOptionSymbol;

        /// <summary>
        /// Gets or sets the idco option symbol.
        /// </summary>
        /// <value>
        /// The idco option symbol.
        /// </value>
        [Browsable(false)]
        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        /// <summary>
        /// The _opra option symbol
        /// </summary>
        private string _opraOptionSymbol = string.Empty;

        /// <summary>
        /// Gets or sets the opra option symbol.
        /// </summary>
        /// <value>
        /// The opra option symbol.
        /// </value>
        [Browsable(false)]
        public string OpraOptionSymbol
        {
            get { return _opraOptionSymbol; }
            set { _opraOptionSymbol = value; }
        }

        /// <summary>
        /// The _p b symbol
        /// </summary>
        private string _pBSymbol = string.Empty;

        /// <summary>
        /// Gets or sets the pb symbol.
        /// </summary>
        /// <value>
        /// The pb symbol.
        /// </value>
        public string PBSymbol
        {
            get { return _pBSymbol; }
            set { _pBSymbol = value; }
        }

        /// <summary>
        /// The _mismatch type
        /// </summary>
        private string _mismatchType = string.Empty;

        /// <summary>
        /// Gets or sets the type of the mismatch.
        /// </summary>
        /// <value>
        /// The type of the mismatch.
        /// </value>
        public string MismatchType
        {
            get { return _mismatchType; }
            set { _mismatchType = value; }
        }

        /// <summary>
        /// The _mis match details
        /// </summary>
        private string _misMatchDetails;

        /// <summary>
        /// Gets or sets the mis match details.
        /// </summary>
        /// <value>
        /// The mis match details.
        /// </value>
        public string MisMatchDetails
        {
            get { return _misMatchDetails; }
            set { _misMatchDetails = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
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
            return _symbol;
        }

        /// <summary>
        /// Adds the business rules.
        /// </summary>
        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.SymbolCheck, "Symbol");
            ValidationRules.AddRule(CustomRules.DateCheck, "Date");
            ValidationRules.AddRule(CustomRules.AUECIDCheck, "AUECID");
        }

        /// <summary>
        /// 
        /// </summary>
        public class CustomRules : RuleArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CustomRules"/> class.
            /// </summary>
            /// <param name="validation">The validation.</param>
            public CustomRules(string validation)
                : base(validation)
            {
            }


            /// <summary>
            /// Checks the validation on all fileds.
            /// </summary>
            /// <param name="finalTarget">The final target.</param>
            public static void CheckValidationOnAllFileds(DividendYieldImport finalTarget)
            {
                if (string.IsNullOrEmpty(finalTarget.Symbol) ||
                    string.IsNullOrEmpty(finalTarget.Date) ||
                    finalTarget.AUECID <= 0)
                {
                    if (finalTarget.AUECID <= 0)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NotExists.ToString();
                    }

                    else
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                    }
                }
                else
                {
                    finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                }
            }

            /// <summary>
            /// Symbols the check.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <param name="e">The e.</param>
            /// <returns></returns>
            public static bool SymbolCheck(object target, RuleArgs e)
            {
                DividendYieldImport finalTarget = target as DividendYieldImport;
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

            /// <summary>
            /// Dates the check.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <param name="e">The e.</param>
            /// <returns></returns>
            public static bool DateCheck(object target, RuleArgs e)
            {
                DividendYieldImport finalTarget = target as DividendYieldImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (string.IsNullOrEmpty(finalTarget.Date))
                    {
                        e.Description = "Date required";
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

            /// <summary>
            /// Auecids the check.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <param name="e">The e.</param>
            /// <returns></returns>
            public static bool AUECIDCheck(object target, RuleArgs e)
            {
                DividendYieldImport finalTarget = target as DividendYieldImport;
                if (finalTarget != null)
                {
                    CheckValidationOnAllFileds(finalTarget);

                    if (finalTarget.AUECID <= 0)
                    {
                        e.Description = "Invalid AUECID";
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
