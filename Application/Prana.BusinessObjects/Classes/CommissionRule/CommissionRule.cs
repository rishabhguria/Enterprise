using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Prana.BusinessObjects
{
    //[Serializable]
    public class CommissionRule : INotifyPropertyChanged
    {
        #region Public properties

        /// <summary>
        /// The _rule identifier
        /// </summary>
        private Guid _ruleId = Guid.Empty;

        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        /// <value>
        /// The rule identifier.
        /// </value>
        public Guid RuleID
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        /// <summary>
        /// This commission rule can be applied to multiple asset classes. Asset Category can be Equity or EquityOption or Future or FutureOption etc....
        /// </summary>
        private List<AssetCategory> _assetIdList;

        /// <summary>
        /// Gets or sets the asset identifier list.
        /// </summary>
        /// <value>
        /// The asset identifier list.
        /// </value>
        public List<AssetCategory> AssetIdList
        {
            get { return _assetIdList; }
            set { _assetIdList = value; }
        }

        /// <summary>
        /// The _rule name
        /// </summary>
        private string _ruleName = string.Empty;

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        /// <value>
        /// The name of the rule.
        /// </value>
        public string RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }

        /// <summary>
        /// The _rule description
        /// </summary>
        private string _ruleDescription = string.Empty;

        /// <summary>
        /// Gets or sets the rule description.
        /// </summary>
        /// <value>
        /// The rule description.
        /// </value>
        public string RuleDescription
        {
            get { return _ruleDescription; }
            set { _ruleDescription = value; }
        }

        /// <summary>
        /// TradeType can be Single Trade or Basket Trade or Both option
        /// </summary>
        private TradeType _applyRuleForTrade;

        /// <summary>
        /// Gets or sets the apply rule for trade.
        /// </summary>
        /// <value>
        /// The apply rule for trade.
        /// </value>
        public TradeType ApplyRuleForTrade
        {
            get { return _applyRuleForTrade; }
            set { _applyRuleForTrade = value; }
        }

        /// <summary>
        /// The _is commission applied
        /// </summary>
        private bool _isCommissionApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is commission applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is commission applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsCommissionApplied
        {
            get { return _isCommissionApplied; }
            set
            {
                _isCommissionApplied = value;
                RaisePropertyChangedEvent("IsCommissionApplied");
            }
        }

        /// <summary>
        /// The _commission
        /// </summary>
        private Commission _commission = new Commission();

        /// <summary>
        /// Gets or sets the commission.
        /// </summary>
        /// <value>
        /// The commission.
        /// </value>
        public Commission Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        /// <summary>
        /// The _is soft commission applied
        /// </summary>
        private bool _isSoftCommissionApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is soft commission applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is soft commission applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsSoftCommissionApplied
        {
            get { return _isSoftCommissionApplied; }
            set
            {
                _isSoftCommissionApplied = value;
                RaisePropertyChangedEvent("IsSoftCommissionApplied");
            }
        }

        /// <summary>
        /// The _soft commission
        /// </summary>
        private Commission _softCommission = new Commission();

        /// <summary>
        /// Gets or sets the soft commission.
        /// </summary>
        /// <value>
        /// The soft commission.
        /// </value>
        public Commission SoftCommission
        {
            get { return _softCommission; }
            set { _softCommission = value; }
        }

        /// <summary>
        /// The _is clearing fee applied
        /// </summary>
        private bool _isClearingFeeApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clearing fee applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clearing fee applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearingFeeApplied
        {
            get { return _isClearingFeeApplied; }
            set
            {
                _isClearingFeeApplied = value;
                RaisePropertyChangedEvent("IsClearingFeeApplied");
            }
        }

        /// <summary>
        /// The _is clearing broker fee applied
        /// </summary>
        private bool _isClearingBrokerFeeApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clearing broker fee applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clearing broker fee applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearingBrokerFeeApplied
        {
            get { return _isClearingBrokerFeeApplied; }
            set
            {
                _isClearingBrokerFeeApplied = value;
                RaisePropertyChangedEvent("IsClearingBrokerFeeApplied");
            }
        }

        /// <summary>
        /// The _is stamp duty applied
        /// </summary>
        private bool _isStampDutyApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is stamp duty applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is stamp duty applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsStampDutyApplied
        {
            get { return _isStampDutyApplied; }
            set
            {
                _isStampDutyApplied = value;
                RaisePropertyChangedEvent("IsStampDutyApplied");
            }
        }

        /// <summary>
        /// The _stamp duty
        /// </summary>
        private double _stampDuty;

        /// <summary>
        /// Gets or sets the stamp duty.
        /// </summary>
        /// <value>
        /// The stamp duty.
        /// </value>
        public double StampDuty
        {
            get { return _stampDuty; }
            set { _stampDuty = value; }
        }

        /// <summary>
        /// The _stamp duty calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Stamp Duty
        /// </summary>
        private CalculationBasis _stampDutyCalculationBasedOn;

        /// <summary>
        /// Gets or sets the stamp duty calculation based on.
        /// </summary>
        /// <value>
        /// The stamp duty calculation based on.
        /// </value>
        public CalculationBasis StampDutyCalculationBasedOn
        {
            get { return _stampDutyCalculationBasedOn; }
            set { _stampDutyCalculationBasedOn = value; }
        }

        /// <summary>
        /// The _is taxon commissions applied
        /// </summary>
        private bool _isTaxonCommissionsApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is taxon commissions applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is taxon commissions applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsTaxonCommissionsApplied
        {
            get { return _isTaxonCommissionsApplied; }
            set
            {
                _isTaxonCommissionsApplied = value;
                RaisePropertyChangedEvent("IsTaxonCommissionsApplied");
            }
        }

        /// <summary>
        /// The _taxon commissions
        /// </summary>
        private double _taxonCommissions;

        /// <summary>
        /// Gets or sets the taxon commissions.
        /// </summary>
        /// <value>
        /// The taxon commissions.
        /// </value>
        public double TaxonCommissions
        {
            get { return _taxonCommissions; }
            set { _taxonCommissions = value; }
        }

        /// <summary>
        /// The tax on commissions calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Tax on Commission
        /// </summary>
        private CalculationBasis _taxonCommissionsCalculationBasedOn;

        /// <summary>
        /// Gets or sets the taxon commissions calculation based on.
        /// </summary>
        /// <value>
        /// The taxon commissions calculation based on.
        /// </value>
        public CalculationBasis TaxonCommissionsCalculationBasedOn
        {
            get { return _taxonCommissionsCalculationBasedOn; }
            set { _taxonCommissionsCalculationBasedOn = value; }
        }

        /// <summary>
        /// The _is transaction levy applied
        /// </summary>
        private bool _isTransactionLevyApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is transaction levy applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is transaction levy applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsTransactionLevyApplied
        {
            get { return _isTransactionLevyApplied; }
            set
            {
                _isTransactionLevyApplied = value;
                RaisePropertyChangedEvent("IsTransactionLevyApplied");
            }
        }

        /// <summary>
        /// The _transaction levy
        /// </summary>
        private double _transactionLevy;

        /// <summary>
        /// Gets or sets the transaction levy.
        /// </summary>
        /// <value>
        /// The transaction levy.
        /// </value>
        public double TransactionLevy
        {
            get { return _transactionLevy; }
            set { _transactionLevy = value; }
        }

        /// <summary>
        /// The _transaction levy calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Transaction Levy
        /// </summary>
        private CalculationBasis _transactionLevyCalculationBasedOn;

        /// <summary>
        /// Gets or sets the transaction levy calculation based on.
        /// </summary>
        /// <value>
        /// The transaction levy calculation based on.
        /// </value>
        public CalculationBasis TransactionLevyCalculationBasedOn
        {
            get { return _transactionLevyCalculationBasedOn; }
            set { _transactionLevyCalculationBasedOn = value; }
        }

        /// <summary>
        /// The _is misc fees applied
        /// </summary>
        private bool _isMiscFeesApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is misc fees applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is misc fees applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsMiscFeesApplied
        {
            get { return _isMiscFeesApplied; }
            set
            {
                _isMiscFeesApplied = value;
                RaisePropertyChangedEvent("IsMiscFeesApplied");
            }
        }

        /// <summary>
        /// The _misc fees
        /// </summary>
        private double _miscFees;

        /// <summary>
        /// Gets or sets the misc fees.
        /// </summary>
        /// <value>
        /// The misc fees.
        /// </value>
        public double MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        /// <summary>
        /// The _misc fees calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Misc Fees
        /// </summary>
        private CalculationBasis _miscFeesCalculationBasedOn;

        /// <summary>
        /// Gets or sets the misc fees calculation based on.
        /// </summary>
        /// <value>
        /// The misc fees calculation based on.
        /// </value>
        public CalculationBasis MiscFeesCalculationBasedOn
        {
            get { return _miscFeesCalculationBasedOn; }
            set { _miscFeesCalculationBasedOn = value; }
        }

        /// <summary>
        /// The _is clearing fee_ a applied
        /// </summary>
        private bool _isClearingFee_AApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clearing fee_ a applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clearing fee_ a applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearingFee_AApplied
        {
            get { return _isClearingFee_AApplied; }
            set
            {
                _isClearingFee_AApplied = value;
                RaisePropertyChangedEvent("IsClearingFee_AApplied");
            }
        }

        /// <summary>
        /// The _clearing fee_ a
        /// </summary>
        private double _clearingFee_A;

        /// <summary>
        /// Gets or sets the clearing fee_ a.
        /// </summary>
        /// <value>
        /// The clearing fee_ a.
        /// </value>
        public double ClearingFee_A
        {
            get { return _clearingFee_A; }
            set { _clearingFee_A = value; }
        }

        /// <summary>
        /// The _clearing fee calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Misc Fees
        /// </summary>
        private CalculationBasis _clearingFeeCalculationBasedOn_A;

        /// <summary>
        /// Gets or sets the clearing fee calculation based on_ a.
        /// </summary>
        /// <value>
        /// The clearing fee calculation based on_ a.
        /// </value>
        public CalculationBasis ClearingFeeCalculationBasedOn_A
        {
            get { return _clearingFeeCalculationBasedOn_A; }
            set { _clearingFeeCalculationBasedOn_A = value; }
        }

        /// <summary>
        /// The _is commission rule selected
        /// </summary>
        private bool _isCommissionRuleSelected = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is commission rule selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is commission rule selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsCommissionRuleSelected
        {
            get { return _isCommissionRuleSelected; }
            set { _isCommissionRuleSelected = value; }
        }

        /// <summary>
        /// The _account i ds
        /// </summary>
        private List<int> _accountIDs = null;

        /// <summary>
        /// Gets or sets the account i ds.
        /// </summary>
        /// <value>
        /// The account i ds.
        /// </value>
        public List<int> AccountIDs
        {
            get { return _accountIDs; }
            set { _accountIDs = value; }
        }


        /// <summary>
        /// The _is modified
        /// </summary>
        private bool _isModified = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
                if (_isModified)
                {
                    if (CommissionRuleChanged != null)
                    {
                        CommissionRuleChanged(this, new EventArgs<CommissionRule>(this));
                    }
                }
            }
        }

        /// <summary>
        /// The _is sec fee applied
        /// </summary>
        private bool _isSecFeeApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sec fee applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is sec fee applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsSecFeeApplied
        {
            get { return _isSecFeeApplied; }
            set
            {
                _isSecFeeApplied = value;
                RaisePropertyChangedEvent("IsSecFeeApplied");
            }
        }

        /// <summary>
        /// The _sec fee
        /// </summary>
        private double _secFee;

        /// <summary>
        /// Gets or sets the sec fee.
        /// </summary>
        /// <value>
        /// The sec fee.
        /// </value>
        public double SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }

        /// <summary>
        /// The _is occ fee applied
        /// </summary>
        private bool _isOccFeeApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is occ fee applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is occ fee applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsOccFeeApplied
        {
            get { return _isOccFeeApplied; }
            set
            {
                _isOccFeeApplied = value;
                RaisePropertyChangedEvent("IsOccFeeApplied");
            }
        }

        /// <summary>
        /// The _occ fee
        /// </summary>
        private double _occFee;

        /// <summary>
        /// Gets or sets the occ fee.
        /// </summary>
        /// <value>
        /// The occ fee.
        /// </value>
        public double OccFee
        {
            get { return _occFee; }
            set { _occFee = value; }
        }

        /// <summary>
        /// The _is orf fee applied
        /// </summary>
        private bool _isOrfFeeApplied = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is orf fee applied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is orf fee applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsOrfFeeApplied
        {
            get { return _isOrfFeeApplied; }
            set
            {
                _isOrfFeeApplied = value;
                RaisePropertyChangedEvent("IsOrfFeeApplied");
            }
        }

        /// <summary>
        /// The _orf fee
        /// </summary>
        private double _orfFee;

        /// <summary>
        /// Gets or sets the orf fee.
        /// </summary>
        /// <value>
        /// The orf fee.
        /// </value>
        public double OrfFee
        {
            get { return _orfFee; }
            set { _orfFee = value; }
        }

        /// <summary>
        /// The _sec fee calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Sec Fee
        /// </summary>
        private CalculationBasis _secFeeCalculationBasedOn;

        /// <summary>
        /// Gets or sets the sec fee calculation based on.
        /// </summary>
        /// <value>
        /// The sec fee calculation based on.
        /// </value>
        public CalculationBasis SecFeeCalculationBasedOn
        {
            get { return _secFeeCalculationBasedOn; }
            set { _secFeeCalculationBasedOn = value; }
        }

        /// <summary>
        /// The _occ fee calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Occ Fees
        /// </summary>
        private CalculationBasis _occFeeCalculationBasedOn;

        /// <summary>
        /// Gets or sets the occ fee calculation based on.
        /// </summary>
        /// <value>
        /// The occ fee calculation based on.
        /// </value>
        public CalculationBasis OccFeeCalculationBasedOn
        {
            get { return _occFeeCalculationBasedOn; }
            set { _occFeeCalculationBasedOn = value; }
        }

        /// <summary>
        /// The _orf fee calculation basis. Calculation basis is Shares, Notional and Contracts, it is for Orf Fees
        /// </summary>
        private CalculationBasis _orfFeeCalculationBasedOn;

        /// <summary>
        /// Gets or sets the orf fee calculation based on.
        /// </summary>
        /// <value>
        /// The orf fee calculation based on.
        /// </value>
        public CalculationBasis OrfFeeCalculationBasedOn
        {
            get { return _orfFeeCalculationBasedOn; }
            set { _orfFeeCalculationBasedOn = value; }
        }

        /// <summary>
        /// The _option premium adjustment
        /// </summary>
        private double _optionPremiumAdjustment;

        /// <summary>
        /// Gets or sets the option premium adjustment.
        /// </summary>
        /// <value>
        /// The option premium adjustment.
        /// </value>
        public double OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }

        ClearingFee _clearingFeeObj = new ClearingFee();

        public ClearingFee ClearingFeeObj
        {
            get { return _clearingFeeObj; }
            set { _clearingFeeObj = value; }
        }

        ClearingFee _clearingBrokerFeeObj = new ClearingFee();

        public ClearingFee ClearingBrokerFeeObj
        {
            get { return _clearingBrokerFeeObj; }
            set { _clearingBrokerFeeObj = value; }
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _ruleName;
        }

        /// <summary>
        /// Occurs when [commission rule changed].
        /// </summary>
        public event CommissionRuleChangeHandler CommissionRuleChanged;

        #region Constructor

        /// <summary>
        /// The constructor
        /// </summary>
        public CommissionRule()
        {
        }

        /// <summary>
        /// The paramaterized constructor
        /// </summary>
        /// <param name="isApplied">Is all rule applies</param>
        /// <param name="calculationBasis">The default calculation basis</param>
        /// <param name="defaultValue">The default value</param>
        public CommissionRule(bool isApplied, CalculationBasis calculationBasis, double defaultValue)
        {
            try
            {
                SetAppliedCommissionAndFees(isApplied);

                Commission.RuleAppliedOn = calculationBasis;
                SoftCommission.RuleAppliedOn = calculationBasis;
                ClearingFeeObj.RuleAppliedOn = calculationBasis;
                ClearingBrokerFeeObj.RuleAppliedOn = calculationBasis;
                TransactionLevyCalculationBasedOn = calculationBasis;
                TaxonCommissionsCalculationBasedOn = calculationBasis;
                StampDutyCalculationBasedOn = calculationBasis;
                ClearingFeeCalculationBasedOn_A = calculationBasis;
                MiscFeesCalculationBasedOn = calculationBasis;
                SecFeeCalculationBasedOn = calculationBasis;
                OccFeeCalculationBasedOn = calculationBasis;
                OrfFeeCalculationBasedOn = calculationBasis;

                Commission.CommissionRate = defaultValue;
                SoftCommission.CommissionRate = defaultValue;
                ClearingFeeObj.ClearingFeeRate = defaultValue;
                ClearingBrokerFeeObj.ClearingFeeRate = defaultValue;
                TransactionLevy = defaultValue;
                TaxonCommissions = defaultValue;
                StampDuty = defaultValue;
                ClearingFee_A = defaultValue;
                MiscFees = defaultValue;
                SecFee = defaultValue;
                OccFee = defaultValue;
                OrfFee = defaultValue;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sets is applied value for commission and fees
        /// </summary>
        /// <param name="isApplied">if set to <c>true</c> [is applied].</param>
        public void SetAppliedCommissionAndFees(bool isApplied)
        {
            try
            {
                IsCommissionApplied = isApplied;
                IsSoftCommissionApplied = isApplied;
                IsStampDutyApplied = isApplied;
                IsClearingBrokerFeeApplied = isApplied;
                IsClearingFeeApplied = isApplied;
                IsClearingFee_AApplied = isApplied;
                IsMiscFeesApplied = isApplied;
                IsOccFeeApplied = isApplied;
                IsOrfFeeApplied = isApplied;
                IsSecFeeApplied = isApplied;
                IsTaxonCommissionsApplied = isApplied;
                IsTransactionLevyApplied = isApplied;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// checks if commission rule is valid or not for applying bulk changes
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <returns>
        /// true if user has selected valid values for bulk change, false oherwise
        /// </returns>
        public bool IsValid(out string errorMessage)
        {
            bool isValid = true;
            StringBuilder errorBuilder = new StringBuilder();
            try
            {
                if (IsCommissionApplied && (Commission.CommissionRate < 0 || Commission.RuleAppliedOn.Equals(CalculationBasis.Auto) || (Commission.CommissionRate == 0 && !Commission.RuleAppliedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- Commission ");
                }
                if (IsSoftCommissionApplied && (SoftCommission.CommissionRate < 0 || SoftCommission.RuleAppliedOn.Equals(CalculationBasis.Auto) || (SoftCommission.CommissionRate == 0 && !SoftCommission.RuleAppliedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- Soft Commission ");
                }
                if (IsClearingBrokerFeeApplied && (ClearingBrokerFeeObj.ClearingFeeRate < 0 || ClearingBrokerFeeObj.RuleAppliedOn.Equals(CalculationBasis.Auto) || (ClearingBrokerFeeObj.ClearingFeeRate == 0 && !ClearingBrokerFeeObj.RuleAppliedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- Clearing Broker Fee ");
                }
                if (IsClearingFeeApplied && (ClearingFeeObj.ClearingFeeRate < 0 || ClearingFeeObj.RuleAppliedOn.Equals(CalculationBasis.Auto) || (ClearingFeeObj.ClearingFeeRate == 0 && !ClearingFeeObj.RuleAppliedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- Other Broker Fee ");
                }
                if (IsTransactionLevyApplied && (TransactionLevy < 0 || TransactionLevyCalculationBasedOn.Equals(CalculationBasis.Auto) || (TransactionLevy == 0 && !TransactionLevyCalculationBasedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- Transaction Levy ");
                }
                if (IsTaxonCommissionsApplied && (TaxonCommissions < 0 || TaxonCommissionsCalculationBasedOn.Equals(CalculationBasis.Auto) || (TaxonCommissions == 0 && !TaxonCommissionsCalculationBasedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- Tax on Commissions ");
                }
                if (IsStampDutyApplied && (StampDuty < 0 || StampDutyCalculationBasedOn.Equals(CalculationBasis.Auto) || (StampDuty == 0 && !StampDutyCalculationBasedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- Stamp Duty ");
                }
                if (IsClearingFee_AApplied && (ClearingFee_A < 0 || ClearingFeeCalculationBasedOn_A.Equals(CalculationBasis.Auto) || (ClearingFee_A == 0 && !ClearingFeeCalculationBasedOn_A.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- AUEC Fee1 ");
                }
                if (IsMiscFeesApplied && (MiscFees < 0 || MiscFeesCalculationBasedOn.Equals(CalculationBasis.Auto) || (MiscFees == 0 && !MiscFeesCalculationBasedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- AUEC Fee2 ");
                }
                if (IsSecFeeApplied && (SecFee < 0 || SecFeeCalculationBasedOn.Equals(CalculationBasis.Auto) || (SecFee == 0 && !SecFeeCalculationBasedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- SEC Fee ");
                }
                if (IsOccFeeApplied && (OccFee < 0 || OccFeeCalculationBasedOn.Equals(CalculationBasis.Auto) || (OccFee == 0 && !OccFeeCalculationBasedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- OCC Fee ");
                }
                if (IsOrfFeeApplied && (OrfFee < 0 || OrfFeeCalculationBasedOn.Equals(CalculationBasis.Auto) || (OrfFee == 0 && !OrfFeeCalculationBasedOn.Equals(CalculationBasis.FlatAmount))))
                {
                    errorBuilder.AppendLine("- ORF Fee ");
                }
                if (!string.IsNullOrWhiteSpace(errorBuilder.ToString()))
                {
                    errorBuilder.Insert(0, "Either Calculation Basis is not selected or Rate is less than 0 or Choose 'Flat Amount' Calculation Basis for 0 rate for following : \n");
                    isValid = false;
                }
                else
                    isValid = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                isValid = false;
            }
            errorMessage = errorBuilder.ToString();
            return isValid;
        }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs{CommissionRule}"/> instance containing the event data.</param>
    public delegate void CommissionRuleChangeHandler(object sender, EventArgs<CommissionRule> e);
}
