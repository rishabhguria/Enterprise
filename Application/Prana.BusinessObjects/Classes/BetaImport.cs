using Csla;
using Csla.Validation;
using Prana.Global;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class BetaImport : BusinessBase<BetaImport>
    {
        public BetaImport()
        {

        }
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

        public const string APPROVED = "Approved";
        public const string UNAPPROVED = "UnApproved";


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
        /// will act as a key to the beta import
        /// </summary>
        private int _rowIndex;
        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// Sets the import status of the beta import object
        /// </summary>
        private string _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();

        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        private double _betaPrice = 0;
        public double Beta
        {
            get { return _betaPrice; }
            set
            {
                _betaPrice = value;
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
            return _symbol;
        }

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.SymbolCheck, "Symbol");
            ValidationRules.AddRule(CustomRules.DateCheck, "Date");
            ValidationRules.AddRule(CustomRules.AUECIDCheck, "AUECID");
        }

        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }


            public static void CheckValidationOnAllFileds(BetaImport finalTarget)
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

            public static bool SymbolCheck(object target, RuleArgs e)
            {
                BetaImport finalTarget = target as BetaImport;
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
                BetaImport finalTarget = target as BetaImport;
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

            public static bool AUECIDCheck(object target, RuleArgs e)
            {
                BetaImport finalTarget = target as BetaImport;
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
