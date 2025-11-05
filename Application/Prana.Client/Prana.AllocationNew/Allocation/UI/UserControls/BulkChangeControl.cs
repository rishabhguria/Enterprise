using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prana.Allocation.Common.Enums;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Prana.Allocation.Common.Definitions;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UIUtilities;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class BulkChangeControl : UserControl
    {

        public event ApplyBulkChangeHandler ApplyBulkChangeEvent;
        public BulkChangeControl()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Enable Disable combo box for allocation base on the basis of check box checked status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraChckAllocationBase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraCmbAllocationBase.Enabled = ultraChckAllocationBase.Checked;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Enable Disable combo box for Match Rule on the basis of check box checked status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraChckMatchRule_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraCmbMatchRule.Enabled = ultraChckMatchRule.Checked;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Enable Disable combo box for Preference account on the basis of check box checked status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraChckPrefAccount_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraCmbPrefAccount.Enabled = ultraChckPrefAccount.Checked;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// On load Event Binds data source to all combo boxes.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BulkChangeControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    ultraCmbAllocationBase.DataSource = EnumHelper.ToList(typeof(AllocationBaseType));
                    ultraCmbAllocationBase.DisplayMember = "Value";
                    ultraCmbAllocationBase.ValueMember = "Key";

                    ultraCmbMatchRule.DataSource = EnumHelper.ToList(typeof(MatchingRuleType));
                    ultraCmbMatchRule.DisplayMember = "Value";
                    ultraCmbMatchRule.ValueMember = "Key";

                    Dictionary<int, string> accountList = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();

                    Dictionary<int, string> preferenceAccountList = new Dictionary<int, string>();
                    preferenceAccountList.Add(int.MinValue, "Select");
                    foreach (int id in accountList.Keys)
                    {
                        preferenceAccountList.Add(id, accountList[id]);
                    }


                ultraCmbPrefAccount.DataSource = new BindingSource(preferenceAccountList, null); 
                    ultraCmbPrefAccount.DisplayMember = "Value";
                    ultraCmbPrefAccount.ValueMember = "Key";
                    ultraCmbPrefAccount.SelectedIndex = 0;

                    ValueList matchPositionList = new ValueList();
                    foreach (DefaultableBoolean op in Enum.GetValues(typeof(DefaultableBoolean)))
                    {
                        if (op != DefaultableBoolean.Default)
                            matchPositionList.ValueListItems.Add(op.ToString());
                    }
                    ultraCmbMatchPosition.ValueList = matchPositionList;
                    //     ultraCmbMatchPosition.DataSource = System.Enum.GetValues(typeof(DefaultableBoolean));                
                ultraCmbMatchPosition.SelectedIndex = 0;                  

                    ultraCmbAllocationBase.SelectedIndex = 0;
                    ultraCmbMatchRule.SelectedIndex = 0;

                    ultraChckBoxPosition.Checked = false;
                    ultraChckAllocationBase.Checked = false;
                    ultraChckMatchRule.Checked = false;
                    ultraChckPrefAccount.Checked = false;
                    ultraCmbAllocationBase.Enabled = false;
                    ultraCmbMatchRule.Enabled = false;
                    ultraCmbPrefAccount.Enabled = false;
                    ultraCmbMatchPosition.Enabled = false;

                    //Binding account list with ProrataAccountCombo
                    ultraCmbAccounts.DataSource = new BindingSource(accountList, null);
                    ultraCmbAccounts.DisplayMember = "Value";
                    ultraCmbAccounts.ValueMember = "Key";
                    ultraCmbAccounts.SelectedIndex = 0;
                    if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                ultraBtnApply.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                ultraBtnApply.ForeColor = System.Drawing.Color.White;
                ultraBtnApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnApply.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnApply.UseAppStyling = false;
                ultraBtnApply.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// On apply button click raises event to apply changes to general rule control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> selectedPreference = new List<int>();
                foreach(ValueListItem item in ultraCmbSelectPreferences.CheckedItems.All)
                {
                    selectedPreference.Add(int.Parse(item.DataValue.ToString()));
                }
                
                ApplyBulkChangeEventArgs evntArgs = new ApplyBulkChangeEventArgs()
                {
                    Rule = new AllocationRule()
                    {
                        BaseType = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), ultraCmbAllocationBase.SelectedItem.DataValue.ToString()),
                        MatchPortfolioPosition = Convert.ToBoolean(ultraCmbMatchPosition.SelectedItem.DataValue.ToString()),
                        PreferenceAccountId = Convert.ToInt32(ultraCmbPrefAccount.SelectedItem.DataValue.ToString()),
                        RuleType = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), ultraCmbMatchRule.SelectedItem.DataValue.ToString()),
                        ProrataDaysBack = Convert.ToInt32(ultraNumEditorDate.Value),
                        ProrataAccountList = ControlHelper.GetListId(ultraCmbAccounts.Text, "Account")
                    },
                    allocationBaseChecked = ultraChckAllocationBase.Checked,
                    preferencedAccountChecked = ultraChckPrefAccount.Checked,
                    matchingRuleChecked = ultraChckMatchRule.Checked,
                    matchPortfolioPostionChecked = ultraChckBoxPosition.Checked,
                    ApplyOnDefaultRule = ultraChckApplyDefaultRule.Checked,
                    ApplyOnSelectedPref = ultraChckSelectPreference.Checked,
                    PreferenceList = new List<int>(selectedPreference),
                    
                };
                if (!(evntArgs.matchingRuleChecked && evntArgs.Rule.RuleType == MatchingRuleType.Prorata && evntArgs.Rule.ProrataAccountList == null))
                {
                    if (ApplyBulkChangeEvent != null)
                        ApplyBulkChangeEvent(this, evntArgs);
                }
                else
                    MessageBox.Show(this, "Bulk change not applied, as Prorata account list not selected.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //ApplyBulkChangeEventArgs evntArgs = new ApplyBulkChangeEventArgs();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Enable Disable combo box for Match Position on the basis of check box checked status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraChckBoxPosition_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraCmbMatchPosition.Enabled = ultraChckBoxPosition.Checked;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Enable Disable combo box for to Select Preferenceon the basis of check box checked status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraChckSelectPreference_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ultraCmbSelectPreferences.Items.Count > 0)
                {
                    ultraCmbSelectPreferences.Enabled = ultraChckSelectPreference.Checked;
                }
                else
                {
                    ultraChckSelectPreference.Checked = false;
                    MessageBox.Show(this, "No Preference to select", "Nirvana Preference", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Bind Preference list to dropdown.
        /// </summary>
        /// <param name="preferenceList"></param>
        internal void BindPreferenceList(Dictionary<int, string> preferenceList)
        {
            try
            {
                ultraCmbSelectPreferences.DataSource = new BindingSource(preferenceList, null);
                ultraCmbSelectPreferences.DisplayMember = "Value";
                ultraCmbSelectPreferences.ValueMember = "Key";
                ultraCmbSelectPreferences.SelectedIndex = 0;
                ultraCmbSelectPreferences.Enabled = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// When prorata is selected enable Comboboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraCmbMatchRule_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (MatchingRuleType.Prorata == (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), ultraCmbMatchRule.Value.ToString()))
                {
                    ultraCmbAccounts.Enabled = true;
                    ultraNumEditorDate.Enabled = true;
                    ultralblAccounts.Enabled = true;
                    ultraLblDateUpTo.Enabled = true;                    
                    ultraCmbAllocationBase.Value = AllocationBaseType.CumQuantity;
                    ultraCmbAllocationBase.Enabled = false;
                }
                else
                {
                    ultraCmbAccounts.Enabled = false;
                    ultraNumEditorDate.Enabled = false;
                    ultralblAccounts.Enabled = false;
                    ultraLblDateUpTo.Enabled = false;
                    if (ultraChckAllocationBase.Checked)
                        ultraCmbAllocationBase.Enabled = true;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
