using ExportGridsData;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.ComplianceEngine
{
    /// <summary>
    /// Main form for  Rule UI.
    /// </summary>
    public partial class ComplianceEngine : Form, IPluggableTools, IExportGridData
    {
        Object timerLocker = new object();
        System.Timers.Timer statusBarRefreshTimer = new System.Timers.Timer(5000);
        bool _isCompleteStatus = false;


        //RuleDefinitionMain ruleDefinition1;
        public ComplianceEngine()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    // Hide Rule Group UI if user does not have permission
                    if (ComplianceCacheManager.GetPostComplianceModuleEnabled() && ComplianceCacheManager.GetPostTradeModuleEnabledForUser(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                        ultraTabControl1.Tabs["groupRules"].Visible = true;
                    else
                        ultraTabControl1.Tabs["groupRules"].Visible = false;
                    SetComplianceActivatedTab();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //  ruleDefinitionMainNew1.InitializeRules();
        }

        /// <summary>
        /// Hides the compliance tabs which are not permitted to user
        /// </summary>
        /// <param name="TabPermitted"></param>
       public void SetTabPermissioning(Dictionary<string,bool> TabPermitted)
        {
            try
            {
                if (!TabPermitted.ContainsKey(ApplicationConstants.CONST_COMPLIANCE_RULE_DEFINITION))
                {
                    ultraTabControl1.Tabs["ruleDefinition"].Visible =false;
                    ultraTabControl1.Tabs["groupRules"].Visible = false;
                }                
                if (!TabPermitted.ContainsKey(ApplicationConstants.CONST_COMPLIANCE_ALERT_HISTORY))
                    ultraTabControl1.Tabs["alertHistory"].Visible = false;
                if (!TabPermitted.ContainsKey(ApplicationConstants.CONST_COMPLIANCE_PENDING_APPROVAL))
                    ultraTabControl1.Tabs["pendingApproval"].Visible = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Status bar after timer Elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void statusBarRefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { statusBarRefreshTimer_Tick(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    lock (timerLocker)
                    {
                        if (_isCompleteStatus)
                            ultraStatusBar1.Text = "Ready";
                        else
                            ultraStatusBar1.Text += ".";
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region IPluggableTools Members

        /// <summary>
        /// Sets Timer for status bar
        /// </summary>
        public void SetUP()
        {
            try
            {
                statusBarRefreshTimer.Elapsed += statusBarRefreshTimer_Tick;
                statusBarRefreshTimer.AutoReset = true;
                statusBarRefreshTimer.Start();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //throw new NotImplementedException();
        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set {; }
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        /// <summary>
        /// Dispose all form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComlianceEngine_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // stop the timer so that the items tha are about to be disposed are not acessed
                statusBarRefreshTimer.Stop();
                Boolean doClose = ruleDefinitionMainNew1.CloseStart() && alertHistoryMainNew1.CloseStart() && ruleGroupingMain1.CloseStart();
                CommonDataCache.ComplianceCacheManager.SetComplianceUITabSelected(ApplicationConstants.CONST_COMPLIANCE_RULE_DEFINITION);
                if (doClose)
                {

                    ruleDefinitionMainNew1.UpdateStatusBar -= ruleDefinitionMainNew1_UpdateStatusBar;
                    ruleGroupingMain1.UpdateGroupStatusBar -= ruleGroupingMain1_UpdateGroupStatusBar;
                    statusBarRefreshTimer.Elapsed -= statusBarRefreshTimer_Tick;

                    statusBarRefreshTimer.Dispose();
                    if (PluggableToolsClosed != null)
                        PluggableToolsClosed(this, null);

                }
                /*  else
                        e.Cancel = true;*/
                InstanceManager.ReleaseInstance(typeof(ComplianceEngine));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ComplianceEngineOld_Shown(object sender, EventArgs e)
        {
            try
            {
                ultraStatusBar1.Text = "Loading Compliance Engine...";
                ultraStatusBar2.Text = "Loading Compliance Engine...";
                ruleDefinitionMainNew1.UpdateStatusBar += ruleDefinitionMainNew1_UpdateStatusBar;
                ruleGroupingMain1.UpdateGroupStatusBar += ruleGroupingMain1_UpdateGroupStatusBar;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates group status bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ruleGroupingMain1_UpdateGroupStatusBar(object sender, StatusBarEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { ruleDefinitionMainNew1_UpdateStatusBar(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    lock (timerLocker)
                    {
                        ResetTimer();
                        ultraStatusBar2.Text = e.Status;
                        _isCompleteStatus = e.IsCompleteState;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Status bar when event raised from Controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void ruleDefinitionMainNew1_UpdateStatusBar(object sender, StatusBarEventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { ruleDefinitionMainNew1_UpdateStatusBar(sender, args); };
                    this.BeginInvoke(del);
                }
                else
                {
                    lock (timerLocker)
                    {
                        ResetTimer();
                        ultraStatusBar1.Text = args.Status;
                        _isCompleteStatus = args.IsCompleteState;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Resets timer after operations
        /// </summary>
        private void ResetTimer()
        {
            try
            {
                this.statusBarRefreshTimer.Stop();
                this.statusBarRefreshTimer.Start();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ComplianceEngine_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_COMPLIANCE_ENGINE);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ufmDefault.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ufmDefault.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                InstanceManager.RegisterInstance(this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the pending approval UI data UI open.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        public void UpdatePendingApprovalUI(DataSet dataSet, bool isBringToFrontRequired)
        {
            try
            {
                if (isBringToFrontRequired)
                    ultraTabControl1.Tabs[ApplicationConstants.CONST_COMPLIANCE_PENDING_APPROVAL].Selected = true;
                pendingApprovalMain1.UpdatePengindApprovalGridUIOpen(dataSet);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the pending frozen or unfrozen..
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        public void UpdatePendingFrozenUnfrozen(DataSet dataSet, bool isFrozen)
        {
            try
            {
                pendingApprovalMain1.UpdatePendingFrozenUnfrozen(dataSet, isFrozen);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the compliance activated tab.
        /// </summary>
        public void SetComplianceActivatedTab()
        {
            try
            {
                string selectedTab = CommonDataCache.ComplianceCacheManager.GetComplianceUITabSelect();
                ultraTabControl1.Tabs[selectedTab].Selected = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                if (gridName == "ultraAlertGrid")
                {
                    this.alertHistoryMainNew1.ExportData(gridName, filePath);
                }
                else if (gridName == "ultraPendingApprovalGrid")
                {
                    this.pendingApprovalMain1.ExportData(gridName, filePath);
                }
                else if (gridName == "ultraRuleTree")
                {
                    this.ruleDefinitionMainNew1.ExportData(gridName, filePath);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
