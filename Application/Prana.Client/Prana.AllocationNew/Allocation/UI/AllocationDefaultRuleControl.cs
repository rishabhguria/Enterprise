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
using Prana.Allocation.Common.Definitions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.MiscUtilities;
using Infragistics.Win;
using Prana.Utilities.UIUtilities;

namespace Prana.AllocationNew.Allocation.UI
{
    public partial class AllocationDefaultRuleControl : UserControl
    {
        public event EventHandler ChangePreference;

        public AllocationDefaultRuleControl()
        {
            InitializeComponent();
        }

        private void AllocationDefaultRuleControl_Load(object sender, EventArgs e)
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
                    Dictionary<int, string> accountList;

                    if (_showDateTimeCombo)
                        accountList = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();
                    else
                        accountList = CommonDataCache.CachedDataManager.GetInstance.GetAccounts();

                    Dictionary<int, string> preferencedList = new Dictionary<int, string>();
                    if (!preferencedList.ContainsKey(-1))
                        preferencedList.Add(-1, "Select");
                    foreach (int id in accountList.Keys)
                    {
                        preferencedList.Add(id, accountList[id]);
                    }

                    ultraCmbAccounts.DataSource = new BindingSource(accountList, null);
                    ultraCmbAccounts.DisplayMember = "Value";
                    ultraCmbAccounts.ValueMember = "Key";
                    ultraCmbAccounts.SelectedIndex = 0;


                    ultraCmbPrefAccount.DataSource = new BindingSource(preferencedList, null); ;
                    ultraCmbPrefAccount.DisplayMember = "Value";
                    ultraCmbPrefAccount.ValueMember = "Key";
                    ultraCmbPrefAccount.Value = -1;
                    ultraCmbAllocationBase.SelectedIndex = 0;
                    ultraCmbMatchRule.SelectedIndex = 0;

                    ultraCmbMatchRule.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    ultraCmbPrefAccount.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    ultraCmbAllocationBase.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);

                    ultraLblAllocationBase.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    ultraLblPrefAccount.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    ultraLblMatchRule.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    ultraChckMatchPosition.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);

                    ultraCmbAccounts.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    ultraNumEditorDate.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);

                    ultraCmbMatchRule.BackColor = Color.LemonChiffon;
                    ultraCmbPrefAccount.BackColor = Color.LemonChiffon;
                    ultraCmbAllocationBase.BackColor = Color.LemonChiffon;
                    ultraCmbAccounts.BackColor = Color.LemonChiffon;
                    ultraNumEditorDate.BackColor = Color.LemonChiffon;

                    //Sets visibility of controls accordingly.
                    if (_showDateTimeCombo)
                    {
                        ultraNumEditorDate.Visible = false; ;
                        ultraClndrDate.Visible = true; ;
                    }
                    else
                    {
                        ultraNumEditorDate.Visible = true;
                        ultraClndrDate.Visible = false;
                    }
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

        bool _isPublish = false;
        /// <summary>
        /// Set values according to rule in control
        /// </summary>
        /// <param name="allocationRule"></param>
        internal void SetValues(AllocationRule allocationRule)
        {
            try
            {
                _isPublish = true;
                ultraCmbAllocationBase.SelectedIndex = (int)allocationRule.BaseType == 0 ? 0 : (int)allocationRule.BaseType - 1;
                ultraCmbMatchRule.SelectedIndex = (int)allocationRule.RuleType == 0 ? 0 : (int)allocationRule.RuleType - 1;
                ////If user have more than one account permission
                if (CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Count() > 0)
                {
                    if (_showDateTimeCombo && !CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().ContainsKey(allocationRule.PreferenceAccountId))
                        ultraCmbPrefAccount.Value = -1;
                    else
                        ultraCmbPrefAccount.Value = allocationRule.PreferenceAccountId == 0 ? -1 : allocationRule.PreferenceAccountId;
                    
                    ultraChckMatchPosition.Checked = allocationRule.MatchPortfolioPosition;
                    foreach (ValueListItem item in ultraCmbAccounts.Items)
                    {
                        if (allocationRule != null && allocationRule.ProrataAccountList != null && allocationRule.ProrataAccountList.Contains(Convert.ToInt32(item.DataValue)))
                            item.CheckState = CheckState.Checked;
                        else
                            item.CheckState = CheckState.Unchecked;
                    }
                    
               
                    if (_showDateTimeCombo)
                        ultraClndrDate.Value = DateTime.Now.AddDays(-1 * allocationRule.ProrataDaysBack);
                    else
                        ultraNumEditorDate.Value = allocationRule.ProrataDaysBack;
                    if (ultraCmbMatchRule.Value != null && (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), ultraCmbMatchRule.Value.ToString()) == MatchingRuleType.Prorata)
                    {
                        ultraCmbAccounts.Enabled = true;
                        ultraNumEditorDate.Enabled = true;
                        ultraClndrDate.Enabled = true;
                        ultralblAccounts.Enabled = true;
                        ultraLblDateUpTo.Enabled = true;
                        ultraCmbAllocationBase.SelectedIndex = (int)AllocationBaseType.CumQuantity - 1;
                        ultraCmbAllocationBase.Enabled = false;
                    }
                    else
                    {
                        ultraCmbAccounts.Enabled = false;
                        ultraNumEditorDate.Enabled = false;
                        ultraClndrDate.Enabled = false;
                        ultralblAccounts.Enabled = false;
                        ultraLblDateUpTo.Enabled = false;
                        ultraCmbAllocationBase.Enabled = true;
                    }
                    _isPublish = false;
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

        /// <summary>
        /// Return current values from control
        /// </summary>
        /// <returns></returns>
        internal AllocationRule GetCurrentValues()
        {
            try
            {
                List<int> prorataAccountList = new List<int>();
                foreach (ValueListItem item in ultraCmbAccounts.CheckedItems.All)
                {
                    prorataAccountList.Add(int.Parse(item.DataValue.ToString()));
                }
                int prorataDays = 0;
                if (_showDateTimeCombo)
                    prorataDays = (DateTime.Now.Date - Convert.ToDateTime(ultraClndrDate.Value)).Days;
                else
                    prorataDays = Convert.ToInt32(ultraNumEditorDate.Value);
                return new AllocationRule()
                    {
                        BaseType = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), ultraCmbAllocationBase.Value.ToString()),
                        MatchPortfolioPosition = ultraChckMatchPosition.Checked,
                        RuleType = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), ultraCmbMatchRule.Value.ToString()),
                        PreferenceAccountId = int.Parse(ultraCmbPrefAccount.Value.ToString()),
                        ProrataDaysBack = prorataDays,
                        ProrataAccountList = prorataAccountList
                    };
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Raises Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraCmbAllocationBase_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isPublish)
                {
                    if (ChangePreference != null)
                        ChangePreference(this, new EventArgs());
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

        /// <summary>
        /// Raises Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraCmbPrefAccount_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isPublish)
                {
                    if (ChangePreference != null)
                        ChangePreference(this, new EventArgs());
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

        /// <summary>
        /// Raises Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraCmbMatchRule_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isPublish)
                {
                    if (MatchingRuleType.Prorata == (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), ultraCmbMatchRule.Value.ToString()))
                    {
                        ultraCmbAccounts.Enabled = true;
                        ultraNumEditorDate.Enabled = true;
                        ultraClndrDate.Enabled = true;
                        ultralblAccounts.Enabled = true;
                        ultraLblDateUpTo.Enabled = true;
                        ultraCmbAllocationBase.SelectedIndex = (int)AllocationBaseType.CumQuantity - 1;
                        ultraCmbAllocationBase.Enabled = false;
                    }
                    else
                    {
                        ultraCmbAccounts.Enabled = false;
                        ultraNumEditorDate.Enabled = false;
                        ultraClndrDate.Enabled = false;
                        ultralblAccounts.Enabled = false;
                        ultraLblDateUpTo.Enabled = false;
                        ultraCmbAllocationBase.Enabled = true;
                    }
                    if (ChangePreference != null)
                        ChangePreference(this, new EventArgs());
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

        /// <summary>
        /// Raises Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraChckMatchPosition_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isPublish)
                {
                    if (ChangePreference != null)
                        ChangePreference(this, new EventArgs());
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

        /// <summary>
        /// Boolean for showinf combo or numeric editor.
        /// </summary>
        bool _showDateTimeCombo = false;

        /// <summary>
        /// Set value for boolean variable
        /// </summary>
        /// <param name="showDateTimeCombo"></param>
        internal void ShowDateTimeCombo(bool showDateTimeCombo)
        {
            try
            {
                _showDateTimeCombo = showDateTimeCombo;
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
