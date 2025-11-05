using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;
using Prana.Admin.BLL;
using Prana.BusinessObjects.Compliance;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.Permissions;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.StringUtilities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class ComplianceAlertUserPermission : UserControl
    {
        #region Members

        /// <summary>
        /// </summary>
        const string COMPLIANCEPRETRADEMSG = "* User doesn't have Compliance Pre Trade Permission";

        /// <summary>
        /// </summary>
        const string BASKETCOMPLIANCEMSG = "* User doesn't have Basket Compliance Permission";

        /// <summary>
        /// </summary>
        const string PRETRADECHECKMSG = "* User doesn't have Pre Trade Check Permission";

        /// <summary>
        /// </summary>
        const string CHECKPRETRADEONSTAGINGMSG = "* User doesn't have Pre Trade Check on Staging Permission";

        /// <summary>
        /// </summary>
        const int BASKETCOMPLIANCE = 45;

        /// <summary>
        /// </summary>
        const int COMPLIANCEPRETRADE = 4;

        /// <summary>
        /// </summary>
        const int COMPLIANCEPOSTTRADE = 5;

        /// <summary>
        /// Check basketCompliance at company level
        /// </summary>
        bool basketCompliancePermission;

        #endregion Members

        #region Constructors

        /// <summary>
        /// </summary>
        public ComplianceAlertUserPermission()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="preRuleLevelPermissions"></param>
        private void BindDataSourceToGrid(DataTable preRuleLevelPermissions)
        {
            try
            {
                grdPrePermission.DataSource = preRuleLevelPermissions;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Binds data source to post dropdown
        /// </summary>
        /// <param name="postRuleLevelPermissions"></param>
        private void BindDataSourceToPostDropdown(DataTable postRuleLevelPermissions)
        {
            try
            {
                cmbPostRules.DataSource = null;
                cmbPostRules.ValueList = null;
                cmbPostRules.DataSource = postRuleLevelPermissions;
                cmbPostRules.DisplayMember = "RuleName";
                cmbPostRules.ValueMember = "RuleId";
                cmbPostRules.DataBind();

                foreach (ValueListItem item in cmbPostRules.Items)
                {
                    item.CheckState = postRuleLevelPermissions.AsEnumerable()
                        .Where(p => p.Field<string>("RuleId") == item.DataValue.ToString())
                        .Select(p => p.Field<bool>("PopUp")).First() ? CheckState.Checked : CheckState.Unchecked;
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbPreTradeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                PreTradeCheckChanged();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxTrading control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkBoxTrading_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TradingCheckChanged();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbOverrideType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmbOverrideType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as UltraComboEditor).Text.Equals(RuleOverrideType.Hard.ToString()))
                    cmbPreDefaultPop.Value = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the MouseEnterElement event of the cmbPostRules control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UIElementEventArgs"/> instance containing the event data.</param>
        private void cmbPostRules_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            try
            {
                UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbPostRules);
                toolTipInfo.ToolTipText = cmbPostRules.Text;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Creates compliance perrmission object for user id
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        private CompliancePermissions GetCompliancePermissions(int companyID, int companyUserID)
        {
            try
            {
                CompliancePermissions compliancePermissions = new CompliancePermissions();
                compliancePermissions.CompanyId = companyID;
                compliancePermissions.CompanyUserId = companyUserID;
                compliancePermissions.IsPowerUser = chbPowerUser.Checked;
                compliancePermissions.EnableBasketComplianceCheck = radioButtonBasket.Checked;

                compliancePermissions.complianceUIPermissions.Add(RuleType.PostTrade, new ComplianceUIPermissions());
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsCreate = chckPostCreate.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsExport = chckPostExport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsRename = chckPostRename.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsEnable = chckPostEnable.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsImport = chckPostImport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsDelete = chckPostDelete.Checked;

                compliancePermissions.complianceUIPermissions.Add(RuleType.PreTrade, new ComplianceUIPermissions());
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsCreate = chckPreCreate.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsExport = chckPreExport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsRename = chckPreRename.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsEnable = chckPreEnable.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsImport = chckPreImport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsDelete = chckPreDelete.Checked;

                compliancePermissions.RuleCheckPermission.IsApplyToManual = chbApplyToManual.Checked;
                // compliancePermissions.RuleCheckPermission.IsOverridePermission = chbOverridePermission.Checked;
                compliancePermissions.RuleCheckPermission.IsPreTradeEnabled = chbPreTradeEnabled.Checked;
                compliancePermissions.RuleCheckPermission.IsTrading = checkBoxTrading.Checked;
                compliancePermissions.RuleCheckPermission.IsStaging = checkBoxStaging.Checked;
                compliancePermissions.RuleCheckPermission.DefaultOverRideType = (RuleOverrideType)Enum.Parse(typeof(RuleOverrideType), cmbOverrideType.Value == null ? RuleOverrideType.Soft.ToString() : cmbOverrideType.Value.ToString());
                compliancePermissions.RuleCheckPermission.DefaultPostPopUpEnabled = Convert.ToBoolean(cmbPostDefaultPopup.Value);
                compliancePermissions.RuleCheckPermission.DefaultPrePopUpEnabled = Convert.ToBoolean(cmbPreDefaultPop.Value);

                return compliancePermissions;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Handles the CellChange event of the grdPrePermission control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdPrePermission_CellChange(object sender, CellEventArgs e)
        {
            // TODO: As popup column is not displaying on the grid so this code is not required.
            /*try
            {
                if (e.Cell.Column.Key.Equals("AlertTypePermission") && e.Cell.Text.Equals(RuleOverrideType.Hard.ToString()))
                    e.Cell.Row.Cells["PopUp"].Value = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }*/
        }

        /// <summary>
        /// Handles the InitializeLayout event of the grdPrePermission control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdPrePermission_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridLayout layout = e.Layout;
                UltraGridBand band = layout.Bands[0];

                band.Columns["AlertTypePermission"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ValueList ruleOverrideTypeList = new ValueList();
                foreach (RuleOverrideType op in Enum.GetValues(typeof(RuleOverrideType)))
                {
                    ruleOverrideTypeList.ValueListItems.Add(op, EnumHelper.GetDescription((op)));
                }
                band.Columns["AlertTypePermission"].ValueList = ruleOverrideTypeList;

                band.Columns["PopUp"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ValueList popUpList = new ValueList();
                popUpList.ValueListItems.Add(true, "Yes");
                popUpList.ValueListItems.Add(false, "No");
                band.Columns["PopUp"].ValueList = popUpList;

                e.Layout.Override.ActiveAppearancesEnabled = DefaultableBoolean.False;
                band.Columns["RuleName"].CellActivation = Activation.NoEdit;
                band.Columns["RuleId"].Hidden = true;
                band.Columns["PackageName"].Hidden = true;
                band.Columns["PopUp"].Hidden = true;

                foreach (UltraGridColumn column in band.Columns)
                    column.Header.Caption = StringUtilities.SplitCamelCase(column.Header.Caption);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Pres the trade check changed.
        /// </summary>
        private void PreTradeCheckChanged()
        {
            try
            {
                grpPreCheck.Enabled = chbPreTradeEnabled.Checked;
                cmbOverrideType.Enabled = chbPreTradeEnabled.Checked;
                if (grdPrePermission.DisplayLayout.Bands.Count > 0)
                {
                    UltraGridBand band = grdPrePermission.DisplayLayout.Bands[0];
                    if (band.Columns.Exists("AlertTypePermission"))
                        band.Columns["AlertTypePermission"].CellActivation = (chbPreTradeEnabled.Checked) ? Activation.AllowEdit : Activation.Disabled;
                }

                //if (chbPreTradeEnabled.Checked)
                //{
                //    if (checkBoxStaging.Checked)
                //        radioButtonBasket.Enabled = true;
                //}
                //else if (!chbPreTradeEnabled.Checked )
                //{
                //    radioButtonBasket.Enabled= false;
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool SaveCompliancePermissions(int companyUserID, int companyID)
        {
            try
            {
                foreach (ValueListItem item in cmbPostRules.Items)
                {
                    (cmbPostRules.DataSource as DataTable).AsEnumerable()
                        .Where(p => p.Field<string>("RuleId") == item.DataValue.ToString()).ToList()
                        .ForEach(r => r["PopUp"] = item.CheckState == CheckState.Checked ? true : false);
                }

                DataTable table = new DataTable();
                DataTable postRulesTable = (cmbPostRules.DataSource as DataTable);
                if (postRulesTable != null && postRulesTable.Rows != null && postRulesTable.Rows.Count > 0)
                    table.Merge(postRulesTable);
                DataTable preRulesTable = (grdPrePermission.DataSource as DataTable);
                if (preRulesTable != null && preRulesTable.Rows != null && preRulesTable.Rows.Count > 0)
                    table.Merge(preRulesTable);
                return CompanyManager.SaveCompliancePermissions(GetCompliancePermissions(companyID, companyUserID)) &&
                   CompanyManager.SaveOverRiddenRulePermission(companyUserID, table);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="compliancePermissions"></param>
        public void SetPermissionForUser(CompliancePermissions compliancePermissions)
        {
            try
            {
                if (compliancePermissions != null)
                {
                    chbPowerUser.Checked = compliancePermissions.IsPowerUser;
                    //chbOverridePermission.Checked = dict.RuleCheckPermission.IsOverridePermission;
                    cmbOverrideType.Value = (int)compliancePermissions.RuleCheckPermission.DefaultOverRideType;
                    cmbPreDefaultPop.Value = compliancePermissions.RuleCheckPermission.DefaultPrePopUpEnabled;
                    cmbPostDefaultPopup.Value = compliancePermissions.RuleCheckPermission.DefaultPostPopUpEnabled;
                    chbPreTradeEnabled.Checked = compliancePermissions.RuleCheckPermission.IsPreTradeEnabled;
                    chbApplyToManual.Checked = compliancePermissions.RuleCheckPermission.IsApplyToManual;
                    checkBoxTrading.Checked = compliancePermissions.RuleCheckPermission.IsTrading;
                    checkBoxStaging.Checked = compliancePermissions.RuleCheckPermission.IsStaging;
                    radioButtonBasket.Checked = compliancePermissions.EnableBasketComplianceCheck;
                    if (checkBoxStaging.Checked && !radioButtonBasket.Checked)
                        rabioButtonRegular.Checked = true;

                    if (compliancePermissions.complianceUIPermissions.ContainsKey(RuleType.PostTrade))
                    {
                        chckPostCreate.Checked = compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsCreate;
                        chckPostDelete.Checked = compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsDelete;
                        chckPostEnable.Checked = compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsEnable;
                        chckPostExport.Checked = compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsExport;
                        chckPostImport.Checked = compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsImport;
                        chckPostRename.Checked = compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsRename;
                    }

                    if (compliancePermissions.complianceUIPermissions.ContainsKey(RuleType.PreTrade))
                    {
                        chckPreCreate.Checked = compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsCreate;
                        chckPreDelete.Checked = compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsDelete;
                        chckPreEnable.Checked = compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsEnable;
                        chckPreExport.Checked = compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsExport;
                        chckPreImport.Checked = compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsImport;
                        chckPreRename.Checked = compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsRename;
                    }
                    BindDataSourceToGrid(compliancePermissions.PreRuleLevelPermissions);
                    BindDataSourceToPostDropdown(compliancePermissions.PostRuleLevelPermissions);

                    PreTradeCheckChanged();
                    TradingCheckChanged();
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
        /// </summary>
        /// <param name="companyIDForUser"></param>
        /// <param name="userID"></param>
        /// <param name="companyUserModules"></param>
        public void SetUpControl(int companyIDForUser, int userID, Modules companyUserModules)
        {
            try
            {
                CompliancePermissions compliancePermissions = userID == int.MinValue ? null : CompanyManager.GetPermmissionForUser(userID, companyIDForUser);
                grpBxPreTrade.Enabled = companyUserModules.Cast<Module>().SingleOrDefault(x => x.ModuleID == COMPLIANCEPRETRADE) != null;
                grpBxPostTrade.Enabled = companyUserModules.Cast<Module>().SingleOrDefault(x => x.ModuleID == COMPLIANCEPOSTTRADE) != null;

                ValueList ruleOverrideTypeValueList = new ValueList();
                foreach (RuleOverrideType op in Enum.GetValues(typeof(RuleOverrideType)))
                {
                    ruleOverrideTypeValueList.ValueListItems.Add(op, EnumHelper.GetDescription((op)));
                }
                cmbOverrideType.ValueList = ruleOverrideTypeValueList;

                if (compliancePermissions != null)
                {
                    if (!compliancePermissions.RuleCheckPermission.IsPreTradeEnabled)
                        compliancePermissions.EnableBasketComplianceCheck = false;
                    else if (!compliancePermissions.RuleCheckPermission.IsStaging)
                        compliancePermissions.EnableBasketComplianceCheck = false;
                }
                basketCompliancePermission = ComplianceCacheManager.GetIsBasketComplianceEnabledCompany();
                StagingCheckChanged();
                chbPowerUser.Enabled = grpBxPreTrade.Enabled || grpBxPostTrade.Enabled;

                SetPermissionForUser(compliancePermissions);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Tradings the check changed.
        /// </summary>
        private void TradingCheckChanged()
        {
            try
            {
                chbApplyToManual.Enabled = checkBoxTrading.Checked;
                if (!checkBoxTrading.Checked)
                    chbApplyToManual.Checked = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        private void StagingCheckChanged()
        {
            try
            {
                if (basketCompliancePermission && checkBoxStaging.Checked)
                {
                    rabioButtonRegular.Enabled = true;
                    rabioButtonRegular.Checked = true;
                    radioButtonBasket.Enabled = true;
                }
                else if (!basketCompliancePermission && checkBoxStaging.Checked)
                {
                    rabioButtonRegular.Enabled = false;
                    rabioButtonRegular.Checked = true;
                    radioButtonBasket.Enabled = false;
                }
                else
                {
                    rabioButtonRegular.Enabled = false;
                    rabioButtonRegular.Checked = false;
                    radioButtonBasket.Enabled = false;
                    radioButtonBasket.Checked = false;
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
        /// Handles the Click event of the setPreNotification control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void setPreNotification_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in grdPrePermission.Rows)
                {
                    row.Cells["PopUp"].Value = Convert.ToBoolean(cmbPreDefaultPop.Value);
                    row.Cells["AlertTypePermission"].Value = cmbOverrideType.Value;
                }
                (grdPrePermission.DataSource as DataTable).AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the Click event of the setPostNotification control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void setPostNotification_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ValueListItem item in cmbPostRules.Items)
                    item.CheckState = Convert.ToBoolean(cmbPostDefaultPopup.Value) ? CheckState.Checked : CheckState.Unchecked;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods

        private void checkBoxStaging_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                StagingCheckChanged();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
