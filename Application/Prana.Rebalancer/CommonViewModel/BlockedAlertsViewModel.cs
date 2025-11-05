using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Utilities.MiscUtilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;

namespace Prana.Rebalancer.CommonViewModel
{
    public class BlockedAlertsViewModel : BindableBase, IModalDialogViewModel
    {
        private ObservableDictionary<string, string> _alerts = new ObservableDictionary<string, string>();
        private string _ruleMessageText = string.Empty;
        private bool _isConfirmationVisible;
        private bool _isRejectionVisible;
        private string _confirmationText;
        private string _rejectionText;
        private AlertPopUpType _alertPopUpType;
        private bool _canBindAlerts;
        private bool? _dialogResult = null;
        private string _alertComplianceType = "Pre Trade Compliance";

        private const string COMPLIANCE_TRADE_BLOCK_MESSAGE_OVERRIDE = "Trade Blocked by Pre Trade Compliance. The following rules are blocking the trade. Do you want to allow the trade ?";
        private const string BASKET_COMPLIANCE_TRADE_BLOCK_MESSAGE_OVERRIDE = "Trade Blocked by Basket Compliance. The following rules are blocking the trade. Do you want to allow the trade ?";
        private const string COMPLIANCE_TRADE_BLOCK_MESSAGE_BLOCK = "Trade Blocked by Pre Trade Compliance. The following rules are blocking the trade. Contact your compliance office for instructions.";
        private const string BASKET_COMPLIANCE_TRADE_BLOCK_MESSAGE_BLOCK = "Following alerts were triggered by basket compliance.";
        private const string COMPLIANCE_CHECK_MESSAGE = "Following alerts were triggered by pre trade compliance.";
        private const string COMPLIANCE_TRADE_APPROVAL_MESSAGE = "The trade that was entered would breach compliance that you do not have full permission to override. Would you like to send this trade to the Pending Approval UI for a permitted user to allow the trade?";

        public ObservableDictionary<string, string> Alerts
        {
            get { return _alerts; }
            set { SetProperty(ref _alerts, value); }
        }

        public string RuleMessageText
        {
            get { return _ruleMessageText; }
            set { SetProperty(ref _ruleMessageText, value); }
        }

        public bool IsConfirmationVisible
        {
            get { return _isConfirmationVisible; }
            set { SetProperty(ref _isConfirmationVisible, value); }
        }

        public bool IsRejectionVisible
        {
            get { return _isRejectionVisible; }
            set { SetProperty(ref _isRejectionVisible, value); }
        }

        public string ConfirmationText
        {
            get { return _confirmationText; }
            set { SetProperty(ref _confirmationText, value); }
        }

        public string RejectionText
        {
            get { return _rejectionText; }
            set { SetProperty(ref _rejectionText, value); }
        }

        public AlertPopUpType AlertPopUpType
        {
            get { return _alertPopUpType; }
            set { SetProperty(ref _alertPopUpType, value); }
        }

        public bool CanBindAlerts
        {
            get { return _canBindAlerts; }
            set { SetProperty(ref _canBindAlerts, value); }
        }

        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { SetProperty(ref _dialogResult, value); }
        }

        public string AlertComplianceType
        {
            get { return _alertComplianceType; }
            set { SetProperty(ref _alertComplianceType, value); }
        }

        public ICommand AddAlertsCommand { get; set; }
        public ICommand BlockedAlertsViewerLoaded { get; set; }
        public ICommand ConfirmationClicked { get; set; }
        public ICommand RejectionClicked { get; set; }

        public BlockedAlertsViewModel()
        {
            try
            {

                AddAlertsCommand = new DelegateCommand<object>(AddAlertsAction);
                BlockedAlertsViewerLoaded = new DelegateCommand<object>(parameter => BlockedAlertsViewerLoadedAction());
                ConfirmationClicked = new DelegateCommand<object>(parameter => { DialogResult = true; });
                RejectionClicked = new DelegateCommand<object>(parameter => { DialogResult = false; });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BlockedAlertsViewerLoadedAction()
        {
            try
            {
                switch (AlertPopUpType)
                {
                    case AlertPopUpType.Override:
                        RuleMessageText = COMPLIANCE_TRADE_BLOCK_MESSAGE_OVERRIDE;
                        IsConfirmationVisible = true;
                        IsRejectionVisible = true;
                        ConfirmationText = "Yes";
                        RejectionText = "No";
                        break;
                    case AlertPopUpType.ComplianceCheck:
                        RuleMessageText = COMPLIANCE_CHECK_MESSAGE;
                        IsConfirmationVisible = false;
                        IsRejectionVisible = true;
                        RejectionText = "OK";
                        break;
                    case AlertPopUpType.Inform:
                        RuleMessageText = COMPLIANCE_TRADE_BLOCK_MESSAGE_BLOCK;
                        IsConfirmationVisible = false;
                        IsRejectionVisible = true;
                        RejectionText = "Dismiss";
                        break;
                    case AlertPopUpType.PendingApproval:
                        RuleMessageText = COMPLIANCE_TRADE_APPROVAL_MESSAGE;
                        IsConfirmationVisible = true;
                        IsRejectionVisible = true;
                        ConfirmationText = "Send";
                        RejectionText = "Cancel";
                        break;
                    case AlertPopUpType.BasketComplianceCheck:
                        RuleMessageText = BASKET_COMPLIANCE_TRADE_BLOCK_MESSAGE_BLOCK;
                        IsConfirmationVisible = false;
                        IsRejectionVisible = true;
                        RejectionText = "OK";
                        break;
                    case AlertPopUpType.BasketOverride:
                        RuleMessageText = BASKET_COMPLIANCE_TRADE_BLOCK_MESSAGE_OVERRIDE;
                        IsConfirmationVisible = true;
                        IsRejectionVisible = true;
                        ConfirmationText = "Yes";
                        RejectionText = "No";
                        break;
                }
                CanBindAlerts = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddAlertsAction(object parameter)
        {
            try
            {
                Dictionary<string, int> violatedAlterCount = new Dictionary<string, int>();
                Dictionary<string, string> tempAlerts = new Dictionary<string, string>();
                if (parameter is DataSet)
                {
                    DataSet ds = parameter as DataSet;
                    if (ds != null)
                    {
                        foreach (DataTable table in ds.Tables)
                        {
                            String message = MessageFormatter.FormatToOverrideMessage(table.Rows[0]);
                            String ruleAndDimention = MessageFormatter.FormatRuleNameForDisplay(String.Format("{0} ({1})", table.Rows[0]["RuleName"].ToString(), table.Rows[0]["Dimension"].ToString()));
                            if (!violatedAlterCount.ContainsKey(ruleAndDimention))
                                violatedAlterCount.Add(ruleAndDimention, 1);
                            else
                                violatedAlterCount[ruleAndDimention] = violatedAlterCount[ruleAndDimention] + 1;
                            if (!tempAlerts.ContainsKey(ruleAndDimention))
                                tempAlerts.Add(ruleAndDimention, message);
                        }

                        foreach (string ruleAndDimension in tempAlerts.Keys)
                        {
                            string key = ruleAndDimension + " (" + violatedAlterCount[ruleAndDimension].ToString() + ")";
                            Alerts.Add(key, tempAlerts[ruleAndDimension]);
                        }
                    }
                }
                else if (parameter is List<Alert>)
                {
                    List<Alert> alerts = parameter as List<Alert>;
                    if (alerts != null)
                    {
                        foreach (Alert alert in alerts)
                        {
                            if (alert.PackageName == RulePackage.Basket && AlertComplianceType == "Pre Trade Compliance")
                                AlertComplianceType = "Basket Compliance";
                            String message = MessageFormatter.FormatToOverrideMessage(alert);
                            String ruleAndDimention = MessageFormatter.FormatRuleNameForDisplay(String.Format("{0} ({1})", alert.RuleName, alert.Dimension));
                            if (!violatedAlterCount.ContainsKey(ruleAndDimention))
                                violatedAlterCount.Add(ruleAndDimention, 1);
                            else
                                violatedAlterCount[ruleAndDimention] = violatedAlterCount[ruleAndDimention] + 1;
                            if (!tempAlerts.ContainsKey(ruleAndDimention))
                                tempAlerts.Add(ruleAndDimention, message);
                        }

                        foreach (string ruleAndDimension in tempAlerts.Keys)
                        {
                            string key = ruleAndDimension + " (" + violatedAlterCount[ruleAndDimension].ToString() + ")";
                            Alerts.Add(key, tempAlerts[ruleAndDimension]);
                        }
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
        }
    }
}
