using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.ComplianceEngine.RuleDefinition.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.RuleDefinition.RulePluggins.UserControls
{
    public partial class RuleGroupingMain : UserControl
    {
        public event UpdateStatusBarHandler UpdateGroupStatusBar;

        public RuleGroupingMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Control Load Event Binds all evensts
        /// Initializes notification frequency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleGroupingMain_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    BindAllEvents();
                    Dictionary<int, String> allNotificationFrequency = new Dictionary<int, string>();
                    allNotificationFrequency = RuleManager.GetInstance().LoadNotificationFrequency();
                    notificationSettings1.InitializeNotificationFrequency(allNotificationFrequency);
                    notificationSettings1.Visible = false;
                    RaiseUpdateStatusBarEvent("Loading: ", "");
                }
                if (!CustomThemeHelper.ApplyTheme)
                {
                    SetAppearanceWithoutTheme();

                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                ultraBtnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                ultraBtnSave.ForeColor = System.Drawing.Color.White;
                ultraBtnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnSave.UseAppStyling = false;
                ultraBtnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        private void SetAppearanceWithoutTheme()
        {
            try
            {
                Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();

                appearance2.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraPnlButtons.Appearance = appearance2;
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

        /// <summary>
        /// Binds all evennnnts.
        /// </summary>
        private void BindAllEvents()
        {
            try
            {
                RuleManager.GetInstance().AllRulesLoaded += RuleGroupingMain_AllRulesLoaded;
                RuleManager.GetInstance().RuleOperationCompleted += RuleGroupingMain_RuleOperationCompleted;
                RuleManager.GetInstance().RuleOperationFromDifferentClient += RuleGroupingMain_RuleOperationFromDifferentClient;
                ruleGroupingControl1.GroupOperationEvent += ruleGroupingControl1_GroupOperationEvent;
                RuleManager.GetInstance().RenameRuleOperationCompleted += RuleGroupingMain_RenameRuleOperationCompleted;
                RuleManager.GetInstance().LoadGroups += RuleGroupingMain_LoadGroups;
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
        /// On rename rule operation, rule should be updated in group control
        /// This event is raised if rule is renamed by same user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RuleGroupingMain_RenameRuleOperationCompleted(object sender, RuleOperationEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleGroupingMain_RenameRuleOperationCompleted(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (e.OperationType == RuleOperations.RenameRule)
                    {
                        ApplyRuleOperations(e.RuleList, e.OperationType, e.OldValue);
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

        bool _isPreviousOperationComplete = true;

        /// <summary>
        /// When rule operation is done by other user then group control updates rules.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RuleGroupingMain_RuleOperationFromDifferentClient(object sender, RuleOperationEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleGroupingMain_RuleOperationFromDifferentClient(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    ApplyRuleOperations(e.RuleList, e.OperationType, e.OldValue);
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
        /// Rule operation is done by same user then this event is raised.
        /// if operation is rename then nothing is done as there is separate event for that.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RuleGroupingMain_RuleOperationCompleted(object sender, RuleOperationEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleGroupingMain_RuleOperationCompleted(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (e.OperationType != RuleOperations.RenameRule)
                    {
                        ApplyRuleOperations(e.RuleList, e.OperationType, e.OldValue);
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
        /// This operation opens group details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ruleGroupingControl1_GroupOperationEvent(object sender, GroupOperationsEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { ruleGroupingControl1_GroupOperationEvent(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    GroupOperations operation = e.Operation;
                    switch (operation)
                    {
                        case GroupOperations.Open:
                            notificationSettings1.SetNotificationSettings(e.GroupId, e.Notification);
                            notificationSettings1.Visible = true;
                            break;
                        case GroupOperations.DeleteGroup:
                            if (e.Notification == null)
                                notificationSettings1.Visible = false;
                            break;
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
        /// When all rules are loaded by rule manager then it raises this event to load group and rule in group UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RuleGroupingMain_AllRulesLoaded(object sender, RuleLoadEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleGroupingMain_AllRulesLoaded(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (CommonDataCache.ComplianceCacheManager.GetPostTradeModuleEnabledForUser(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                    {
                        if (CommonDataCache.ComplianceCacheManager.GetCreatePermission(RulePackage.PostTrade.ToString(), CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                        {
                            ruleGroupingControl1.Enabled = true;
                            ruleGroupingControl1.InitializeCache(e.GroupList, e.RuleList);
                            RaiseUpdateStatusBarEvent("Rules Loaded: ", "", true);
                        }
                        else
                        {
                            if (ruleGroupingControl1.Enabled)
                            {
                                ruleGroupingControl1.Enabled = false;
                                MessageBox.Show(this, "User does not have Create and Update permission for Post Trade, so can not create or assign rules to group. Please Contact Administrator", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
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
        /// On save button click sends save request for all groups and there notification settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RaiseUpdateStatusBarEvent("Save Group", "Started");
                ultraBtnSave.Enabled = false;
                List<String> unassignedRuleList = new List<String>();
                Dictionary<String, GroupBase> groupList = new Dictionary<string, GroupBase>();
                Dictionary<String, String> renamedDict = new Dictionary<string, string>();
                Dictionary<String, GroupBase> addedDict = new Dictionary<string, GroupBase>();
                List<String> deletedList = new List<string>();
                String selectedGroupId = ruleGroupingControl1.GetSelectedGroupId();

                unassignedRuleList = ruleGroupingControl1.GetUnAssignedRules();
                groupList = ruleGroupingControl1.GetUpdatedGroupList();
                renamedDict = ruleGroupingControl1.GetRenamedGroups();
                addedDict = ruleGroupingControl1.GetAddedGroupList();
                deletedList = ruleGroupingControl1.GetDeletedGroupLIst();

                // There will be an alert for specifying valid email address (if no email address defined and check-box is checked)

                foreach (String key in groupList.Keys)
                {
                    NotificationSetting setting = new NotificationSetting();
                    if (key == selectedGroupId)
                        setting = notificationSettings1.GetCurrentNotificationSettings();
                    else
                        setting = notificationSettings1.GetNotificationSetting(key);
                    groupList[key].Notification = (setting == null) ? groupList[key].Notification : setting;
                }
                // E-mail id check for each group created
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-11304
                string group;
                bool result = VerifyNotificationSetting(groupList, out group);
                if (result)
                {
                    bool status = RuleManager.GetInstance().SaveGroupSettings(unassignedRuleList, groupList, renamedDict, addedDict, deletedList);
                    //RuleManager.GetInstance().SaveGroupSettings(unassignedRuleList, groupList, renamedDict, addedDict, deletedList);
                    if (status)
                    {
                        ultraBtnSave.Enabled = true;
                        ruleGroupingControl1.ClearAllCache();
                        MessageBox.Show(this, "All group saved.", "Nirvan Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        bool isSelected = ruleGroupingControl1.SelectGroup(selectedGroupId);
                        if (isSelected)
                            notificationSettings1.Visible = true;
                        else
                            notificationSettings1.Visible = false;
                        RaiseUpdateStatusBarEvent("Save Group", " Completed", true);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please enter valid Email Id for the Group: " + group, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ultraBtnSave.Enabled = true; ;
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
            //RuleManager.GetInstance().OperateOnGroup(group,GroupOperations.None,String.Empty);
        }

        /// <summary>
        /// Here, E-mail ID checkbox will be verified that if it is checked and any email id is not entered then it will show an error.
        /// </summary>
        /// <param name="groupList"></param>
        /// <returns></returns>
        private bool VerifyNotificationSetting(Dictionary<string, GroupBase> groupList, out string group)
        {
            try
            {
                group = String.Empty;
                foreach (string groupKey in groupList.Keys)
                {
                    if (groupList[groupKey].Notification.EmailEnabled && (string.IsNullOrEmpty(groupList[groupKey].Notification.EmailCCList) && string.IsNullOrEmpty(groupList[groupKey].Notification.EmailToList)))
                    {
                        group = groupList[groupKey].GroupName;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                group = String.Empty;
                return false;
            }
        }

        /// <summary>
        /// When group is updated by other user  then this event is raised.
        /// for updating groups.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleGroupingMain_LoadGroups(object sender, RuleLoadEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleGroupingMain_LoadGroups(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (CommonDataCache.ComplianceCacheManager.GetCreatePermission(RulePackage.PostTrade.ToString(), CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                    {
                        if (e.GroupList != null && e.RuleList != null)
                        {
                            MessageBox.Show(this, "Groups Updated by other user. Need to update", "Nirvan Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ruleGroupingControl1.Enabled = true;
                            notificationSettings1.Enabled = true;
                            ruleGroupingControl1.RefreshAll();
                            ruleGroupingControl1.InitializeCache(e.GroupList, e.RuleList);
                            String selectedGroupId = ruleGroupingControl1.GetSelectedGroupId();
                            notificationSettings1.UpdateNotification(e.GroupList, selectedGroupId);
                            MessageBox.Show(this, "All group updated.", "Nirvan Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            ruleGroupingControl1.RefreshAll();
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
        /// Apply rule operations on group ui 
        /// if previous operation is complete.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ruleOperations"></param>
        /// <param name="oldVlaue"></param>
        private void ApplyRuleOperations(List<RuleBase> list, RuleOperations ruleOperations, String oldVlaue)
        {

            try
            {
                if (_isPreviousOperationComplete)
                {
                    _isPreviousOperationComplete = false;
                    _isPreviousOperationComplete = ruleGroupingControl1.UpdateOnRuleOperations(list, ruleOperations, oldVlaue);
                    _isPreviousOperationComplete = true;
                    RaiseUpdateStatusBarEvent(ruleOperations.ToString(), " Completed", true);
                }
                else
                {
                    Thread.Sleep(5000);
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
            // String selectedGroupId = ruleGroupingControl1.GetSelectedGroupId();


        }

        /// <summary>
        /// Raise status bar events.
        /// </summary>
        /// <param name="ruleOperations"></param>
        /// <param name="status"></param>
        /// <param name="isCompleteState"></param>
        private void RaiseUpdateStatusBarEvent(String ruleOperations, string status, bool isCompleteState = false)
        {
            try
            {
                StringBuilder statusBuilder = new StringBuilder();

                statusBuilder.Append(ruleOperations);
                statusBuilder.Append(" - ");

                statusBuilder.Append(status);
                if (UpdateGroupStatusBar != null)
                    UpdateGroupStatusBar(this, new StatusBarEventArgs { Status = statusBuilder.ToString(), IsCompleteState = isCompleteState });
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
        /// Dispose UI.
        /// </summary>
        /// <returns></returns>
        internal bool CloseStart()
        {

            try
            {
                RuleManager.GetInstance().AllRulesLoaded -= RuleGroupingMain_AllRulesLoaded;
                RuleManager.GetInstance().RuleOperationCompleted -= RuleGroupingMain_RuleOperationCompleted;
                RuleManager.GetInstance().RuleOperationFromDifferentClient -= RuleGroupingMain_RuleOperationFromDifferentClient;
                ruleGroupingControl1.GroupOperationEvent -= ruleGroupingControl1_GroupOperationEvent;
                RuleManager.GetInstance().RenameRuleOperationCompleted -= RuleGroupingMain_RenameRuleOperationCompleted;
                RuleManager.GetInstance().LoadGroups -= RuleGroupingMain_LoadGroups;
                //RuleManager.GetInstance().DisposeGroup();
                RuleManager.DisposeInstance();
                ruleGroupingControl1.Close();
                return true;
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
                return false;
            }
        }
    }
}
