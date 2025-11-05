using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.MvvmDialogs;
using System;
using System.Text;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class CommissionRuleFormViewModel : ViewModelBase, IModalDialogViewModel
    {
        #region Events

        /// <summary>
        /// Occurs when [on form close button event].
        /// </summary>
        internal event EventHandler OnFormCloseButtonEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _LBL assets
        /// </summary>
        private string _lblAssets;

        /// <summary>
        /// The _LBL rule applied
        /// </summary>
        private string _lblRuleApplied;

        /// <summary>
        /// The _LBL commission criteria
        /// </summary>
        private string _lblCommissionCriteria;

        /// <summary>
        /// The _LBL soft commission
        /// </summary>
        private string _lblSoftCommission;

        /// <summary>
        /// The _LBL description
        /// </summary>
        private string _lblDescription;

        /// <summary>
        /// The _LBL fees rate
        /// </summary>
        private string _lblFeesRate;

        /// <summary>
        /// The _LBL clearing broker fee rate
        /// </summary>
        private string _lblClearingBrokerFeeRate;

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>
        /// The assets.
        /// </value>
        public string Assets
        {
            get { return _lblAssets; }
            set
            {
                _lblAssets = value;
                RaisePropertyChangedEvent("Assets");
            }
        }

        /// <summary>
        /// Gets or sets the clearing broker fee rate.
        /// </summary>
        /// <value>
        /// The clearing broker fee rate.
        /// </value>
        public string ClearingBrokerFeeRate
        {
            get { return _lblClearingBrokerFeeRate; }
            set
            {
                _lblClearingBrokerFeeRate = value;
                RaisePropertyChangedEvent("ClearingBrokerFeeRate");
            }
        }

        /// <summary>
        /// Gets or sets the commission criteria.
        /// </summary>
        /// <value>
        /// The commission criteria.
        /// </value>
        public string CommissionCriteria
        {
            get { return _lblCommissionCriteria; }
            set
            {
                _lblCommissionCriteria = value;
                RaisePropertyChangedEvent("CommissionCriteria");
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return _lblDescription; }
            set
            {
                _lblDescription = value;
                RaisePropertyChangedEvent("Description");
            }
        }

        /// <summary>
        /// Gets or sets the fees rate.
        /// </summary>
        /// <value>
        /// The fees rate.
        /// </value>
        public string FeesRate
        {
            get { return _lblFeesRate; }
            set
            {
                _lblFeesRate = value;
                RaisePropertyChangedEvent("FeesRate");
            }
        }

        /// <summary>
        /// Gets or sets the rule applied.
        /// </summary>
        /// <value>
        /// The rule applied.
        /// </value>
        public string RuleApplied
        {
            get { return _lblRuleApplied; }
            set
            {
                _lblRuleApplied = value;
                RaisePropertyChangedEvent("RuleApplied");
            }
        }

        /// <summary>
        /// Gets or sets the soft commission.
        /// </summary>
        /// <value>
        /// The soft commission.
        /// </value>
        public string SoftCommission
        {
            get { return _lblSoftCommission; }
            set
            {
                _lblSoftCommission = value;
                RaisePropertyChangedEvent("OtherFees");
            }
        }

        /// <summary>
        /// Gets or sets the bring to front.
        /// </summary>
        /// <value>
        /// The bring to front.
        /// </value>
        public WindowState BringToFront
        {
            get { return _bringToFront; }
            set
            {
                if (_bringToFront == WindowState.Minimized)
                    _bringToFront = value;
                else
                {
                    if (value == WindowState.Minimized)
                        _bringToFront = value;
                    else
                    {
                        WindowState currentState = _bringToFront;
                        _bringToFront = WindowState.Minimized;
                        RaisePropertyChangedEvent("BringToFront");
                        _bringToFront = currentState;
                    }
                }
                RaisePropertyChangedEvent("BringToFront");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the form close button.
        /// </summary>
        /// <value>
        /// The form close button.
        /// </value>
        public RelayCommand<object> FormCloseButton { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommissionRuleFormViewModel"/> class.
        /// </summary>
        public CommissionRuleFormViewModel()
        {
            try
            {
                FormCloseButton = new RelayCommand<object>((parameter) => OnCloseButton(parameter));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the rule applied string.
        /// </summary>
        /// <param name="calculationBasis">The calculation basis.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Commission rule basis not set. It should depend either of Shares,Contracts , Notional values,Commissionor NotionalPlusCommission.</exception>
        private string GetRuleAppliedString(Prana.BusinessObjects.AppConstants.CalculationBasis calculationBasis)
        {
            try
            {
                switch (calculationBasis)
                {
                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Shares:
                        return AllocationUIConstants.CAPTION_PerShare;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Notional:
                        return AllocationUIConstants.CAPTION_BasisPoints;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Contracts:
                        return AllocationUIConstants.CAPTION_PerContract;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.NotionalPlusCommission:
                        return AllocationUIConstants.CAPTION_BasisPoints;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Commission:
                        return AllocationUIConstants.CAPTION_BasisPoints;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.AvgPrice:
                        return AllocationUIConstants.CAPTION_PerShare;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.FlatAmount:
                        return AllocationUIConstants.CAPTION_FlatAmount;
                    default:
                        throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts , Notional values,Commissionor NotionalPlusCommission.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Called when [close button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseButton(object parameter)
        {
            try
            {
                if (OnFormCloseButtonEvent != null)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the commission rule.
        /// </summary>
        /// <param name="commissionRule">The commission rule.</param>
        internal void SetCommissionRule(CommissionRule commissionRule)
        {
            try
            {
                string ruleAppiledOnCommission = GetRuleAppliedString(commissionRule.Commission.RuleAppliedOn);
                string ruleAppliedOnSoftCommission = GetRuleAppliedString(commissionRule.SoftCommission.RuleAppliedOn);

                StringBuilder commissionDetails = new StringBuilder();
                commissionDetails.Append("The Permitted Assets : ");
                commissionDetails.AppendLine();
                commissionDetails.Append(string.Join("\n", commissionRule.AssetIdList.ToArray()));

                Assets = commissionDetails.ToString();

                if (commissionRule.RuleDescription != string.Empty)
                    Description = "Rule Description is " + commissionRule.RuleDescription;

                RuleApplied = "Rule Applied to " + Enum.GetName(typeof(BusinessObjects.AppConstants.TradeType), commissionRule.ApplyRuleForTrade);

                commissionDetails.Clear();

                commissionDetails.Append("Commission rate : ");
                commissionDetails.AppendLine();
                if (commissionRule.Commission.IsCriteriaApplied)
                {
                    foreach (
                        CommissionRuleCriteria commissionRuleCriteria in
                            commissionRule.Commission.CommissionRuleCriteiaList)
                    {
                        commissionDetails.Append(commissionRuleCriteria.CommissionRate.ToString());
                        commissionDetails.Append("\t");
                        commissionDetails.Append(ruleAppiledOnCommission);
                        commissionDetails.Append(" for < ");
                        commissionDetails.Append(commissionRuleCriteria.ValueGreaterThan.ToString());
                        commissionDetails.Append(" and <");
                        commissionDetails.Append(commissionRuleCriteria.ValueLessThanOrEqual.ToString());
                        commissionDetails.AppendLine();
                    }
                    CommissionCriteria = commissionDetails.ToString(0, (commissionDetails.Length - 8));
                }
                else
                {
                    commissionDetails.Append(commissionRule.Commission.CommissionRate.ToString());
                    commissionDetails.Append(" ");
                    commissionDetails.Append(ruleAppiledOnCommission);
                    CommissionCriteria = commissionDetails.ToString();
                }

                commissionDetails.Clear();

                commissionDetails.Append("Soft Commission rate : ");
                commissionDetails.AppendLine();
                if (commissionRule.SoftCommission.IsCriteriaApplied)
                {
                    foreach (
                        CommissionRuleCriteria commissionRuleCriteria in
                            commissionRule.SoftCommission.CommissionRuleCriteiaList)
                    {
                        commissionDetails.Append(commissionRuleCriteria.CommissionRate.ToString());
                        commissionDetails.Append("\t");
                        commissionDetails.Append(ruleAppiledOnCommission);
                        commissionDetails.Append(" for < ");
                        commissionDetails.Append(commissionRuleCriteria.ValueGreaterThan.ToString());
                        commissionDetails.Append(" and <");
                        commissionDetails.Append(commissionRuleCriteria.ValueLessThanOrEqual.ToString());
                        commissionDetails.AppendLine();
                    }
                    SoftCommission = commissionDetails.ToString(0, (commissionDetails.Length - 8));
                }
                else
                {
                    commissionDetails.Append(commissionRule.SoftCommission.CommissionRate.ToString());
                    commissionDetails.Append(" ");
                    commissionDetails.Append(ruleAppliedOnSoftCommission);
                    SoftCommission = commissionDetails.ToString();
                }

                commissionDetails.Clear();

                if (commissionRule.IsClearingFeeApplied)
                {
                    commissionDetails.Append("OtherBrokerFees Rate : ");
                    commissionDetails.AppendLine();
                    if (commissionRule.ClearingFeeObj.IsCriteriaApplied)
                    {
                        foreach (ClearingFeeCriteria clearingFeeCriteria in commissionRule.ClearingFeeObj.ClearingFeeRuleCriteiaList)
                        {
                            commissionDetails.Append(clearingFeeCriteria.ClearingFeeRate.ToString());
                            commissionDetails.Append("\t");
                            commissionDetails.Append(GetRuleAppliedString(commissionRule.ClearingFeeObj.RuleAppliedOn));
                            commissionDetails.Append(" for < ");
                            commissionDetails.Append(clearingFeeCriteria.ValueGreaterThan.ToString());
                            commissionDetails.Append(" and <");
                            commissionDetails.Append(clearingFeeCriteria.ValueLessThanOrEqual.ToString());
                            commissionDetails.AppendLine();
                        }
                        FeesRate = commissionDetails.ToString(0, (commissionDetails.Length - 8));
                    }
                    else
                    {
                        commissionDetails.Append(commissionRule.ClearingFeeObj.ClearingFeeRate.ToString());
                        commissionDetails.Append(" ");
                        commissionDetails.Append(GetRuleAppliedString(commissionRule.ClearingFeeObj.RuleAppliedOn));
                        FeesRate = commissionDetails.ToString();
                    }
                }

                commissionDetails.Clear();

                if (commissionRule.IsClearingBrokerFeeApplied)
                {
                    commissionDetails.Append("ClearingBrokerFee Rate : ");
                    commissionDetails.AppendLine();
                    if (commissionRule.ClearingBrokerFeeObj.IsCriteriaApplied)
                    {
                        foreach (ClearingFeeCriteria clearingFeeCriteria in commissionRule.ClearingBrokerFeeObj.ClearingFeeRuleCriteiaList)
                        {
                            commissionDetails.Append(clearingFeeCriteria.ClearingFeeRate.ToString());
                            commissionDetails.Append("\t");
                            commissionDetails.Append(GetRuleAppliedString(commissionRule.ClearingFeeObj.RuleAppliedOn));
                            commissionDetails.Append(" for < ");
                            commissionDetails.Append(clearingFeeCriteria.ValueGreaterThan.ToString());
                            commissionDetails.Append(" and <");
                            commissionDetails.Append(clearingFeeCriteria.ValueLessThanOrEqual.ToString());
                            commissionDetails.AppendLine();
                        }
                        ClearingBrokerFeeRate = commissionDetails.ToString(0, (commissionDetails.Length - 8));
                    }
                    else
                    {
                        commissionDetails.Append(commissionRule.ClearingBrokerFeeObj.ClearingFeeRate.ToString());
                        commissionDetails.Append(" ");
                        commissionDetails.Append(GetRuleAppliedString(commissionRule.ClearingBrokerFeeObj.RuleAppliedOn));
                        ClearingBrokerFeeRate = commissionDetails.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the rule details.
        /// </summary>
        /// <param name="commissionRule">The commission rule.</param>
        internal void SetRuleDetails(CommissionRule commissionRule)
        {
            try
            {
                // TODO: Complete member initialization
                SetCommissionRule(commissionRule);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods

        #region IModalDialogViewModel Members

        public bool? DialogResult
        {
            get { return true; }
        }

        #endregion
    }
}
