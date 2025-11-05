using Csla;
using Csla.Validation;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class DividendImport : BusinessBase<DividendImport>
    {

        public DividendImport()
        {
        }

        private string _symbology;

        public string Symbology
        {
            get { return _symbology; }
            set { _symbology = value; }
        }

        public const string VALID = "Validated";

        public const string INVALID = "NotValidated";


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

        private double _Amount = 0;
        public double Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                PropertyHasChanged("Amount");
            }
        }

        /// <summary>
        /// Fx Rate
        /// </summary>
        private double _fxRate = 0;
        public double FXRate
        {
            get { return _fxRate; }
            set
            {
                _fxRate = value;
                PropertyHasChanged("FXRate");
            }
        }

        /// <summary>
        /// FX Conversion Method Operator
        /// </summary>
        private string _fXConversionMethodOperator = Operator.M.ToString();
        public string FXConversionMethodOperator
        {
            get { return _fXConversionMethodOperator; }
            set
            {
                _fXConversionMethodOperator = value;
                PropertyHasChanged("FXConversionMethodOperator");
            }
        }

        private int _fundID = 0;
        [Browsable(false)]
        public int FundID
        {
            get { return _fundID; }
            set
            {
                _fundID = value;
                PropertyHasChanged("FundID");
            }
        }

        private string _accountName = string.Empty;
        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                PropertyHasChanged(ApplicationConstants.CONST_FUNDNAME);
            }
        }

        private string _payoutDate = string.Empty;
        public string PayoutDate
        {
            get { return _payoutDate; }
            set
            {
                _payoutDate = value;
                PropertyHasChanged("PayoutDate");
            }
        }

        private string _exDate = string.Empty;
        public string ExDate
        {
            get { return _exDate; }
            set
            {
                _exDate = value;
                PropertyHasChanged("ExDate");
            }
        }

        private string _recordDate = string.Empty;
        public string RecordDate
        {
            get { return _recordDate; }
            set
            {
                _recordDate = value;
                PropertyHasChanged("RecordDate");
            }
        }

        private string _declarationDate = string.Empty;
        public string DeclarationDate
        {
            get { return _declarationDate; }
            set
            {
                _declarationDate = value;
                PropertyHasChanged("DeclarationDate");
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                PropertyHasChanged("Description");
            }
        }

        private int _level2ID = 0;
        public int Level2ID
        {
            get { return _level2ID; }
            set
            {
                _level2ID = value;
                PropertyHasChanged("Level2Id");
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

        private string _currencyName = string.Empty;

        public string CurrencyName
        {
            get { return _currencyName; }
            set
            {
                _currencyName = value;
                PropertyHasChanged("CurrencyName");
            }
        }

        private int _currencyID = 0;

        public int CurrencyID
        {
            get { return _currencyID; }
            set
            {
                _currencyID = value;
                PropertyHasChanged("CurrencyID");
            }
        }

        private int _userID = 0;
        public int UserId
        {
            get { return _userID; }
            set
            {
                _userID = value;
                PropertyHasChanged("UserId");
            }
        }

        private string _pbSymbol = string.Empty;
        public string PBSymbol
        {
            get { return _pbSymbol; }
            set
            {
                _pbSymbol = value;
            }
        }

        private string _activityType = string.Empty;
        public string ActivityType
        {
            get { return _activityType; }
            set
            {
                _activityType = value;
            }
        }

        private int _activityTypeId = 0;
        [Browsable(false)]
        public int ActivityTypeId
        {
            get { return _activityTypeId; }
            set
            {
                _activityTypeId = value;
                PropertyHasChanged("ActivityTypeId");
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

        private string _validated = INVALID;
        public string Validated
        {
            get { return _validated; }
            set
            {
                _validated = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        protected override object GetIdValue()
        {
            return _symbol;
        }

        /// <summary>
        /// Add Business Rules
        /// </summary>
        protected override void AddBusinessRules()
        {
            try
            {
                //Narendra Kumar Jangir, Sept 11,2013
                //Check for AUECID and symbol removed because now other cash transactions will be imported using dividend import which does not contain symbol

                //ValidationRules.AddRule(CustomRules.SymbolCheck, "Symbol");
                //ValidationRules.AddRule(CustomRules.AUECIDCheck, "AUECID");
                ValidationRules.AddRule(CustomRules.ExDateCheck, "ExDate");
                ValidationRules.AddRule(CustomRules.AccountIDCheck, "FundID");
                ValidationRules.AddRule(CustomRules.ActivityTypeIDCheck, "ActivityTypeId");
                ValidationRules.AddRule(CustomRules.CurrencyCheck, "CurrencyID");
                //ValidationRules.AddRule(CustomRules.FXRateCheck, "FXRate");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }

            public static void CheckValidationOnAllFileds(DividendImport finalTarget)
            {
                if (string.IsNullOrEmpty(finalTarget.ExDate) || (finalTarget.FundID <= 0) || (finalTarget.ActivityTypeId <= 0) || finalTarget.CurrencyID <= 0 ||
                            !NAVLockDateRule.ValidateNAVLockDate(finalTarget.ExDate))
                {
                    finalTarget.Validated = INVALID;
                }
                else
                {
                    finalTarget.Validated = VALID;
                }
            }

            /// <summary>
            /// AUECID Check
            /// </summary>
            /// <param name="target"></param>
            /// <param name="e"></param>
            /// <returns></returns>
            public static bool AUECIDCheck(object target, RuleArgs e)
            {
                try
                {
                    DividendImport finalTarget = target as DividendImport;
                    if (finalTarget != null)
                    {
                        if (finalTarget.AUECID <= 0)
                        {
                            finalTarget.Validated = INVALID;
                        }
                        else
                        {
                            finalTarget.Validated = VALID;
                        }

                        if (finalTarget.AUECID <= 0)
                        {
                            e.Description = "AUECID required";
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
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }


            /// <summary>
            /// FXRate Check
            /// </summary>
            /// <param name="target"></param>
            /// <param name="e"></param>
            /// <returns></returns>
            public static bool FXRateCheck(object target, RuleArgs e)
            {
                try
                {
                    DividendImport finalTarget = target as DividendImport;
                    if (finalTarget != null)
                    {
                        if (finalTarget.FXRate <= 0)
                        {
                            finalTarget.Validated = INVALID;
                            e.Description = "FXRate required";
                            return false;
                        }
                        else
                        {
                            finalTarget.Validated = VALID;
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }

            /// <summary>
            /// Symbol Check
            /// </summary>
            /// <param name="target"></param>
            /// <param name="e"></param>
            /// <returns></returns>
            public static bool SymbolCheck(object target, RuleArgs e)
            {
                try
                {
                    DividendImport finalTarget = target as DividendImport;
                    if (finalTarget != null)
                    {
                        if (string.IsNullOrEmpty(finalTarget.Symbol))
                        {
                            finalTarget.Validated = INVALID;
                        }
                        else
                        {
                            finalTarget.Validated = VALID;
                        }

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
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }

            /// <summary>
            /// ExDate Check
            /// </summary>
            /// <param name="target"></param>
            /// <param name="e"></param>
            /// <returns></returns>
            public static bool ExDateCheck(object target, RuleArgs e)
            {
                try
                {
                    DividendImport finalTarget = target as DividendImport;
                    if (finalTarget != null)
                    {
                        CheckValidationOnAllFileds(finalTarget);

                        if (string.IsNullOrEmpty(finalTarget.ExDate))
                        {
                            e.Description = "Ex Date required";
                            return false;
                        }else if (!NAVLockDateRule.ValidateNAVLockDate(finalTarget.ExDate))
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
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }

            /// <summary>
            /// AccountID Check
            /// </summary>
            /// <param name="target"></param>
            /// <param name="e"></param>
            /// <returns></returns>
            public static bool AccountIDCheck(object target, RuleArgs e)
            {
                try
                {
                    DividendImport finalTarget = target as DividendImport;
                    if (finalTarget != null)
                    {
                        CheckValidationOnAllFileds(finalTarget);

                        if (finalTarget.FundID <= 0)
                        {
                            e.Description = "Account Name not validated";
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
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }

            /// <summary>
            /// ActivityTypeID Check
            /// </summary>
            /// <param name="target"></param>
            /// <param name="e"></param>
            /// <returns></returns>
            public static bool ActivityTypeIDCheck(object target, RuleArgs e)
            {
                try
                {
                    DividendImport finalTarget = target as DividendImport;
                    if (finalTarget != null)
                    {
                        CheckValidationOnAllFileds(finalTarget);

                        if (finalTarget.ActivityTypeId <= 0)
                        {
                            e.Description = "ActivityType not validated";
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
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }

            /// <summary>
            /// Currencies the check.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <param name="e">The e.</param>
            /// <returns></returns>
            public static bool CurrencyCheck(object target, RuleArgs e)
            {
                try
                {
                    DividendImport finalTarget = target as DividendImport;
                    if (finalTarget != null)
                    {
                        CheckValidationOnAllFileds(finalTarget);

                        if (finalTarget.CurrencyID <= 0)
                        {
                            e.Description = "Currency not validated";
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
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }

        }

    }
}
