using Infragistics.Win.Misc;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana
{
    public partial class BlockedAlertsViewer : Form
    {
        private Dictionary<String, String> _alerts = new Dictionary<String, String>();

        private const String TRADE_BLOCK_MESSAGE_OVERRIDE = "Trade Blocked by Pre Trade Compliance. The following rules are blocking the trade. Do you want to allow the trade ?";
        private const String TRADE_BLOCK_MESSAGE_BLOCK = "Trade Blocked by Pre Trade Compliance. The following rules are blocking the trade. Contact your compliance office for instructions.";
        private const String COMPLIANCE_CHECK_MESSAGE = "Following alerts were triggered by pre trade compliance.";
        private const String TRADE_APPROVAL_MESSAGE = "The trade that was entered would breach compliance that you do not have full permission to override. Would you like to send this trade to the Pending Approval UI for a permitted user to allow the trade?";

        /// <summary>
        /// Create a pop up dialog and modify is as per the required type
        /// </summary>
        /// <param name="alertPopUpType"></param>
        public BlockedAlertsViewer(AlertPopUpType alertPopUpType)
        {
            InitializeComponent();
            switch (alertPopUpType)
            {
                case AlertPopUpType.Override:
                    ultraLabel1.Text = TRADE_BLOCK_MESSAGE_OVERRIDE;
                    break;
                case AlertPopUpType.ComplianceCheck:
                    btnYes.Visible = false;
                    btnNo.Text = "OK";
                    ultraLabel1.Text = COMPLIANCE_CHECK_MESSAGE;
                    break;
                case AlertPopUpType.Inform:
                    btnYes.Visible = false;
                    btnNo.Text = "Dismiss";
                    ultraLabel1.Text = TRADE_BLOCK_MESSAGE_BLOCK;
                    break;
                case AlertPopUpType.PendingApproval:
                    ultraLabel1.Text = TRADE_APPROVAL_MESSAGE;
                    //ultraLabel1.Size = new System.Drawing.Size(492, 60);
                    btnYes.Text = "Send";
                    btnNo.Text = "Cancel";
                    break;
            }
        }

        /// <summary>
        /// Add the alerts to the form
        /// </summary>
        /// <param name="ds"></param>
        public void AddAlerts(DataSet ds)
        {
            Dictionary<string, int> violatedAlterCount = new Dictionary<string, int>();
            Dictionary<string, string> tempAlerts = new Dictionary<string, string>();
            foreach (DataTable table in ds.Tables)
            {
                String message = MessageFormatter.FormatToOverrideMessage(table.Rows[0]);
                String ruleAndDimention;
                if (table.Rows.Count > 0 && String.IsNullOrEmpty(table.Rows[0]["Dimension"].ToString()))
                    ruleAndDimention = MessageFormatter.FormatRuleNameForDisplay(String.Format("{0}", table.Rows[0]["RuleName"].ToString()));
                else
                    ruleAndDimention = MessageFormatter.FormatRuleNameForDisplay(String.Format("{0} ({1})", table.Rows[0]["RuleName"].ToString(), table.Rows[0]["Dimension"].ToString()));
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
                _alerts.Add(key, tempAlerts[ruleAndDimension]);
            }
        }

        /// <summary>
        /// Add the alerts to the form
        /// </summary>
        /// <param name="alerts"></param>
        public void AddAlerts(List<Alert> alerts)
        {
            Dictionary<string, int> violatedAlterCount = new Dictionary<string, int>();
            Dictionary<string, string> tempAlerts = new Dictionary<string, string>();
            foreach (Alert alert in alerts)
            {
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
                _alerts.Add(key, tempAlerts[ruleAndDimension]);
            }
        }

        private void BlockedAlertsViewer_Load(object sender, EventArgs e)
        {
            bool expanded = true;
            foreach (KeyValuePair<String, String> alert in _alerts)
            {
                UltraExpandableGroupBox gpBx = new UltraExpandableGroupBox() { Text = alert.Key, Dock = DockStyle.Top, Expanded = expanded, Margin = new Padding(10), Size = new System.Drawing.Size(492, 205), ExpandedSize = new System.Drawing.Size(492, 205) };
                expanded = false;
                ultraAlertsPanel.ClientArea.Controls.Add(gpBx);
                gpBx.Panel.Controls.Add(new UltraLabel() { Text = alert.Value, Dock = DockStyle.Fill });
            }
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER);
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnYes.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnYes.ForeColor = System.Drawing.Color.White;
                btnYes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnYes.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnYes.UseAppStyling = false;
                btnYes.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnNo.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnNo.ForeColor = System.Drawing.Color.White;
                btnNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnNo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnNo.UseAppStyling = false;
                btnNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
