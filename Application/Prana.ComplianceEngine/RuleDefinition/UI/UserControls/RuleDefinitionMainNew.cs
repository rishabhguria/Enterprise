using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.RuleDefinition.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    public partial class RuleDefinitionMainNew : UserControl
    {
        Boolean _isOperationComplete = true;
        public event UpdateStatusBarHandler UpdateStatusBar;

        private volatile bool _isMsgBoxBeingSelected = false;

        public RuleDefinitionMainNew()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load basic tree nodes on rule rule navigator load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ruleNavigator_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    LoadBasicTreeNode();
                    //RaiseUpdateStatusBarEvent(RuleOperations.LoadRules, "Started");
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
        /// Creates Tree nodes for package and category.
        /// </summary>
        private void LoadBasicTreeNode()
        {
            try
            {
                RaiseUpdateStatusBarEvent(RuleOperations.None, "Loading Tree");
                foreach (RulePackage package in Enum.GetValues(typeof(RulePackage)))
                {
                    if (!package.Equals(RulePackage.None))
                    {
                        ruleNavigator.AddTreeNode(package, RuleCategory.None, null);
                        foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                        {
                            if (!category.Equals(RuleCategory.None))
                                ruleNavigator.AddTreeNode(package, category, null);
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
        /// Sends request for intialization request to RuleManager.
        /// Discard if any other operation is in process.
        /// </summary>
        private void InitializeRules()
        {
            try
            {
                if (!_isOperationComplete)
                {
                    MessageBox.Show(this, "Previous Operation in progress Discarded", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                RaiseUpdateStatusBarEvent(RuleOperations.LoadRules, "Started");
                _isOperationComplete = false;
                RuleManager.GetInstance().LoadRules();
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
        /// Binds all events.
        /// </summary>
        private void BindAllEvents()
        {
            try
            {
                RuleManager.GetInstance().AllRulesLoaded += RuleDefinitionMain_AllRulesLoaded;
                ruleNavigator.OpenSelectedRule += ruleNavigator_OpenSelectedRule;
                ruleNavigator.OperateOnRule += ruleNavigator_OperationsOnRule;
                RuleManager.GetInstance().RuleOperationCompleted += RuleDefinitionMain_RuleOperationCompleted;
                RuleManager.GetInstance().RuleOperationFromDifferentClient += RuleDefinitionMainNew_RuleOperationFromDifferentClient;
                ruleDefViewer.RuleDefBrowserSaveCompleteEvent += ruleDefViewer_RuleDefBrowserSaveCompleteEvent;

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
        /// Event raised when guvnor completes save rule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ruleDefViewer_RuleDefBrowserSaveCompleteEvent(object sender, BrowserLoadCompletedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { ruleDefViewer_RuleDefBrowserSaveCompleteEvent(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    //If document completed is raised after Load.
                    if (!e.IsPostBack)
                    {
                        RaiseUpdateStatusBarEvent(RuleOperations.None, "Load : Completed", true);
                    }
                    //if Document complete is raised after save.
                    else
                    {
                        List<String> ruleIdList = ruleNavigator.GetRuleId();
                        List<RuleBase> ruleList = new List<RuleBase>();
                        foreach (String ruleId in ruleIdList)
                        {
                            RuleBase rule = RuleManager.GetInstance().GetRule(ruleId);
                            if (e.Tag != null)
                                (rule as CustomRuleDefinition).ConstantsDefinationAsJSon = e.Tag.ToString();
                            ruleList.Add(rule);
                        }
                        ultraBtnSave.Enabled = true;
                        //RaiseUpdateStatusBarEvent(RuleOperations.None, "Saved Rule and Notification Setting for " + ruleList.Count + " rules", true);
                        //MessageBox.Show(this, "Save : Completed", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        if (e.IsSuccess)
                        {
                            notificationSettings.GetNotificationSetting(ref ruleList);
                            RuleManager.GetInstance().SaveUpdateRule(ruleList);
                            foreach (RuleBase rule in ruleList)
                            {
                                // if (rule.Category == RuleCategory.UserDefined)
                                //{
                                RuleManager.GetInstance().OperateOnRule(rule.Package, rule.Category, rule.RuleName, RuleOperations.Build, rule.RuleId, "", e.Tag);
                                //}
                                //else
                                //{
                                //    RaiseUpdateStatusBarEvent(RuleOperations.None, "Saved Rule and Notification Setting for " + ruleList.Count + " rules", true);
                                //    MessageBox.Show(this, "Save : Completed", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //}
                            }
                            ultraBtnSave.Enabled = true;
                            //RaiseUpdateStatusBarEvent(RuleOperations.None, "Saved Rule and Notification Setting for " + ruleList.Count + " rules", true);
                            //MessageBox.Show(this, "Save : Completed", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            ultraBtnSave.Enabled = true;
                            RaiseUpdateStatusBarEvent(RuleOperations.None, "Error in saving Rule and Notification Setting for " + ruleList.Count + " rules", true);
                            MessageBox.Show(this, "Error in saving rules", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Updates tree node when rule operation is done by other user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RuleDefinitionMainNew_RuleOperationFromDifferentClient(object sender, RuleOperationEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleDefinitionMainNew_RuleOperationFromDifferentClient(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    Dictionary<String, bool> selectedRules = new Dictionary<string, bool>();
                    List<String> selectedRuleIds = new List<string>();
                    if (e.OperationType == RuleOperations.RenameRule)
                    {
                        if (ruleNavigator.GetIfRuleSelectedFromOldValue(e.OldValue))
                            selectedRules.Add(e.RuleList[0].RuleId, true);
                        else
                        {
                            selectedRuleIds = ruleNavigator.GetRuleId();
                            if (selectedRuleIds.Count > 0)
                                selectedRules.Add(selectedRuleIds[0], true);
                        }
                    }
                    else
                    {
                        selectedRules = ruleNavigator.GetIfRuleSelected(e.RuleList);
                    }
                    List<RuleBase> ruleList = new List<RuleBase>();
                    ruleList.AddRange(e.RuleList);
                    StringBuilder builder = new StringBuilder();

                    if (selectedRules.Count > 0)
                    {
                        foreach (RuleBase rule in e.RuleList)
                        {
                            if (selectedRules.ContainsKey(rule.RuleId))
                            {
                                builder.Append("Rule Name: ");
                                builder.AppendLine(rule.RuleName);
                                builder.AppendLine("Selected Rule " + "'" + rule.RuleName + "'" + " is being modified by other user");
                                ruleList.Remove(rule);
                            }
                        }
                        builder.Append("Operation: ");
                        builder.AppendLine(e.OperationType.ToString());
                    }
                    if (ruleList.Count > 0)
                    {
                        builder.AppendLine("Following other rules are edited by other User: ");

                        foreach (RuleBase rule in ruleList)
                        {
                            builder.AppendLine(rule.RuleName);
                        }
                    }

                    string saveMsg = Application.StartupPath + "\\ComplianceTemp\\" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                    string fileName = "Operations_" + string.Format("text-{0:yyyy-MM-dd_hh}.txt", DateTime.Now);
                    if (!Directory.Exists(saveMsg))
                    {
                        Directory.CreateDirectory(saveMsg);
                    }
                    string saveMsgFile = saveMsg + fileName;   // Saving Msg in a File at Specific Loacation

                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(saveMsgFile, FileMode.Append, FileAccess.Write);  // Writing Messages in a File
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            fs = null;
                            sw.WriteLine("on");
                            sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss"));
                            sw.WriteLine(builder);
                        }
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Dispose();
                    }

                    System.Threading.Tasks.Task.Run(() => // Create a New Task for MessageBox
                    {
                        if (!_isMsgBoxBeingSelected)
                        {
                            _isMsgBoxBeingSelected = true;
                            DialogResult res = MessageBox.Show(this, "Selected rule is being modified by other user, your changes will be discarded. Do You Want To See The Changes", "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (res == DialogResult.Yes)  //Checking User Response after showing msgBox
                            {
                                Process.Start("notepad.exe", saveMsgFile);
                            }
                            _isMsgBoxBeingSelected = false;
                        }
                    });

                    RaiseUpdateStatusBarEvent(e.OperationType, builder.ToString(), true);
                    ruleDefViewer.UpdateDictionary(e.RuleList, e.OldValue);

                    notificationSettings.UpdateNotification(e.RuleList);
                    //If only one rule is selected then that rule is open so need to open updated browser.
                    string reseletionRequiredRuleId = string.Empty;
                    if (selectedRules.Count == 1)
                    {
                        //MessageBox.Show(this, builder.ToString(), "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        foreach (RuleBase rule in e.RuleList)
                        {
                            if (selectedRules.ContainsKey(rule.RuleId))
                            {
                                ruleDefViewer.LoadDocument(rule.RuleURL);
                                notificationSettings.SetNotificationSettings(rule, true);
                                reseletionRequiredRuleId = rule.RuleId;
                            }
                        }
                    }
                    ruleNavigator.ReloadTree(e.RuleList, e.OperationType, e.OldValue, reseletionRequiredRuleId);
                    RaiseUpdateStatusBarEvent(e.OperationType, "Done by other User", true);
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
        /// On Rule operation completed updates tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void RuleDefinitionMain_RuleOperationCompleted(object sender, RuleOperationEventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleDefinitionMain_RuleOperationCompleted(sender, args); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (args.HasError)
                    {
                        Dictionary<String, bool> selectedRules = new Dictionary<string, bool>();
                        _isOperationComplete = true;
                        ruleNavigator.UpdateTreeNode(args.RuleList, args.FailedRuleList, args.OperationType);
                        ruleDefViewer.UpdateDictionary(args.FailedRuleList, args.OldValue);
                        selectedRules = ruleNavigator.GetIfRuleSelected(args.FailedRuleList);
                        //foreach (RuleBase rule in args.FailedRuleList)
                        //{
                        //    ruleDefViewer.LoadDocument(rule.RuleURL, false);
                        //}
                        if (selectedRules.Count == 1)
                        {
                            foreach (RuleBase rule in args.FailedRuleList)
                            {
                                if (selectedRules.ContainsKey(rule.RuleId))
                                    ruleDefViewer.LoadDocument(rule.RuleURL);
                            }
                        }
                        RaiseUpdateStatusBarEvent(args.OperationType, " failed", true);
                        MessageBox.Show(this, args.OperationType + ": failed \nPlease check your recent changes\n\n" + args.ErrorMessage, "NirvanaCompliance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        _isOperationComplete = true;
                        RaiseUpdateStatusBarEvent(args.OperationType, " Completed", true);
                        if (args.OperationType == RuleOperations.ExportRule)
                        {
                            string str = string.Join(", ", from item in args.RuleList select item.RuleName);
                            Logger.LoggerWrite(str + " Rule(Count-" + args.RuleList.Count + ") Exported by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                            MessageBox.Show(this, args.RuleList.Count + " Rules Exported.", "NirvanaCompliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        else if (args.OperationType == RuleOperations.Build)
                        {
                            ruleDefViewer.UpdateDictionary(args.RuleList, args.OldValue);
                            //Loading rule in control.
                            //Need to change when multiple rule save will be implemented
                            if (args.RuleList.Count == 1)
                                ruleDefViewer.LoadDocument(args.RuleList[0].RuleURL);
                            ruleNavigator.UpdateTreeNode(args.RuleList, args.FailedRuleList, args.OperationType);
                            RaiseUpdateStatusBarEvent(RuleOperations.None, "Saved Rule and Notification Setting for " + args.RuleList.Count + " rules", true);
                            MessageBox.Show(this, "Save : Completed", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //MessageBox.Show(this, "Rules are Live Now", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (args.OperationType == RuleOperations.ImportRule)
                        {
                            ruleNavigator.UpdateTreeNode(args.RuleList, args.FailedRuleList, args.OperationType);
                            RaiseUpdateStatusBarEvent(args.OperationType, " : Completed for " + args.RuleList.Count + " rules", true);
                            // MessageBox.Show(this, args.OperationType + " : Completed", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        #region unused
                        /*else if (args.OperationType == RuleOperations.RenameRule)
                        {
                            ruleDefViewer.UpdateDictionary(args.RuleList,args.OldValue);
                            ruleNavigator.UpdateTreeNode(args.RuleList, args.OperationType);
                            MessageBox.Show(this, "Rules are Live Now", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (args.OperationType == RuleOperations.DeleteRule)
                        {
                            ruleDefViewer.UpdateDictionary(args.RuleList, args.OldValue);
                            ruleNavigator.UpdateTreeNode(args.RuleList, args.OperationType);
                            MessageBox.Show(this, "Rules are Live Now", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }*/
                        #endregion
                        else
                        {
                            ruleDefViewer.UpdateDictionary(args.RuleList, args.OldValue);
                            if (args.RuleList.Count == 1)
                                ruleDefViewer.LoadDocument(args.RuleList[0].RuleURL);
                            ruleNavigator.UpdateTreeNode(args.RuleList, args.FailedRuleList, args.OperationType);
                            RaiseUpdateStatusBarEvent(args.OperationType, " : Completed for " + args.RuleList.Count + " rules", true);
                            MessageBox.Show(this, args.OperationType + " : Completed", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// On event from from Rule navigator ask rule manager to operate on rules.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void ruleNavigator_OperationsOnRule(object sender, OperationsOnRuleEventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { ruleNavigator_OperationsOnRule(sender, args); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (args.OperationType == RuleOperations.LoadRules)
                    {
                        LoadBasicTreeNode();
                        InitializeRules();
                    }
                    else
                    {
                        if (_isOperationComplete)
                        {
                            RaiseUpdateStatusBarEvent(args.OperationType, "Started");
                            // _isOperationComplete = false;
                            switch (args.OperationType)
                            {
                                case RuleOperations.AddRule:
                                case RuleOperations.DisableAllRules:
                                case RuleOperations.DisableRule:
                                case RuleOperations.EnableAllRules:
                                case RuleOperations.EnableRule:
                                case RuleOperations.RenameRule:
                                    _isOperationComplete = false;
                                    RuleManager.GetInstance().OperateOnRule(args.PackageName, args.Category, args.RuleName, args.OperationType, args.RuleId, args.OldRuleName, null);
                                    break;
                                case RuleOperations.DeleteRule:
                                    if (IsExportPathExists())
                                    {
                                        _isOperationComplete = false;
                                        RuleManager.GetInstance().OperateOnRule(args.PackageName, args.Category, args.RuleName, args.OperationType, args.RuleId, args.OldRuleName, null);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, "Delete Stopped.\nPath for Exporting rules is not set. Please verify.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        RaiseUpdateStatusBarEvent(args.OperationType, "Stopped", true);
                                    }
                                    break;
                                /*Export
                                  package name, category,ruleName
                                 If selected node is package node then category and rule name is none and empty respectively
                                  else is category node then rule name is empty
                                  else if rule node then all parameters contains value.*/
                                case RuleOperations.ExportRule:
                                    //If Export path is null or Empty then Show message to set path else process Export. 
                                    if (IsExportPathExists())
                                    {
                                        RuleManager.GetInstance().ExportRule(args.PackageName, args.Category, args.RuleId);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, "Export Stopped.\nPath for Exporting rules is not set. Please verify.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        RaiseUpdateStatusBarEvent(args.OperationType, "Stopped", true);
                                    }
                                    break;
                                case RuleOperations.ImportRule:
                                    ImportRule(args.PackageName, args.Category);
                                    break;
                                    //case RuleOperations.LoadRules:
                                    //    LoadBasicTreeNode();
                                    //    InitializeRules();
                                    //    break;
                            }

                        }
                        else
                        {
                            MessageBox.Show(this, "Previous Operation in progress", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RaiseUpdateStatusBarEvent(RuleOperations.None, "Previous Operation in progress", true);
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
        /// Checks if the export path exists, If not then try to create the path and return
        /// </summary>
        /// <returns></returns>
        private bool IsExportPathExists()
        {
            try
            {
                string importExportPath = ComplianceCacheManager.GetImportExportPath();
                if (string.IsNullOrEmpty(importExportPath))
                {
                    return false;
                }
                else
                {
                    if (System.IO.Directory.Exists(importExportPath))
                    {
                        return true;
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(importExportPath);
                        return true;
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
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="ruleCategory"></param>
        private void ImportRule(RulePackage rulePackage, RuleCategory ruleCategory)
        {
            try
            {
                ImportRuleSelection selection = new ImportRuleSelection(rulePackage, ruleCategory);
                DialogResult dr = selection.ShowDialog(this.FindForm());
                if (dr == DialogResult.OK)
                {
                    Dictionary<String, ImportDefinition> importDefList = selection.GetImportListDefinition();
                    if (importDefList != null && importDefList.Count > 0)
                    {
                        RuleManager.GetInstance().ImportRules(importDefList);
                        _isOperationComplete = false;
                    }
                    else
                    {
                        _isOperationComplete = true;
                        RaiseUpdateStatusBarEvent(RuleOperations.ImportRule, "Cancelled", true);
                    }
                }
                else
                {
                    _isOperationComplete = true;
                    RaiseUpdateStatusBarEvent(RuleOperations.ImportRule, "Cancelled", true);
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
            //selection.Dispose();
        }

        /// <summary>
        /// open selected rule and notification settings on navigator on rule def viewer and notification control.
        /// if selected node is rule open rule definition
        /// else open startup page.
        /// And enable disable save button and notification control accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void ruleNavigator_OpenSelectedRule(object sender, SelectedNodeEventArgs args)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { ruleNavigator_OpenSelectedRule(sender, args); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (_isOperationComplete)
                    {
                        String uri = Application.StartupPath + "\\HtmlFiles\\Startup.Htm";
                        if (args.Level >= 3)
                        {
                            RaiseUpdateStatusBarEvent(RuleOperations.None, "Opening Rule");
                            RuleBase rule = RuleManager.GetInstance().GetRule(args.RuleId);

                            if (rule != null)
                            {
                                uri = rule.RuleURL;
                                notificationSettings.SetGroupDetails(RuleManager.GetInstance().GetGroupForId(rule.GroupId));
                                notificationSettings.SetNotificationSettings(rule, false);
                            }
                            notificationSettings.Visible = true;
                            notificationSettings.Enabled = true;
                            ultraBtnSave.Visible = true;
                            ultraBtnReload.Visible = true;
                            SetUpdatePermissions(args.PackageName, CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        }
                        else
                        {
                            RaiseUpdateStatusBarEvent(RuleOperations.None, "Ready");
                            ultraBtnSave.Visible = false;
                            ultraBtnReload.Visible = false;
                            notificationSettings.Visible = false;
                            notificationSettings.Enabled = false;
                        }
                        ruleDefViewer.LoadDocument(uri);
                        RaiseUpdateStatusBarEvent(RuleOperations.None, "Ready");
                    }
                    //RaiseUpdateStatusBarEvent(RuleOperations.None, "Open Rule : Completed", true); 
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
        /// open browser notification setting in disabled mode is only there is no create permission from admin
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="userId"></param>
        private void SetUpdatePermissions(RulePackage rulePackage, int userId)
        {
            try
            {
                if (ComplianceCacheManager.GetCreatePermission(rulePackage.ToString(), userId))
                {
                    ruleDefViewer.Enabled = true;
                    ultraBtnSave.Enabled = true;
                    notificationSettings.Enabled = true;
                }
                else
                {
                    ruleDefViewer.Enabled = false;
                    ultraBtnSave.Enabled = false;
                    notificationSettings.Enabled = false;
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
        /// Add rules to navigator tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void RuleDefinitionMain_AllRulesLoaded(object sender, RuleLoadEventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleDefinitionMain_AllRulesLoaded(sender, args); };
                    this.BeginInvoke(del);
                }
                else
                {
                    _isOperationComplete = true;
                    ruleNavigator.AddTreeNode(args.RuleList);
                    RaiseUpdateStatusBarEvent(RuleOperations.LoadRules, "Complete", true);
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
        /// Initialize notification controls.
        /// Load alert frequencies from DB.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notificationSettings_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    Dictionary<int, String> allNotificationFrequency = new Dictionary<int, string>();
                    allNotificationFrequency = RuleManager.GetInstance().LoadNotificationFrequency();
                    notificationSettings.InitializeNotificationFrequency(allNotificationFrequency);
                    notificationSettings.Enabled = false;
                    notificationSettings.Visible = false;
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
        /// Save changes in rule and notification settings on drools and DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ultraBtnSave.Enabled = false;
                List<String> ruleIdList = ruleNavigator.GetRuleId();
                List<RuleBase> ruleList = new List<RuleBase>();
                foreach (String ruleId in ruleIdList)
                {
                    RuleBase rule = RuleManager.GetInstance().GetRule(ruleId);
                    ruleList.Add(rule);
                }
                //DialogResult dr=MessageBox.Show(this,"Do you want to save rule","Nirvana Compliance",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                //if (dr == DialogResult.Yes)
                //{
                if (notificationSettings.VerifySettings())
                {
                    RaiseUpdateStatusBarEvent(RuleOperations.None, "Saving Rule and Notification Setting for " + ruleList.Count + " rules");
                    ruleDefViewer.SaveRule(ruleList);
                }
                else
                {
                    MessageBox.Show(this, "Please enter valid Email Id for the rule", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ultraBtnSave.Enabled = true; ;
                }
                // notificationSettings.GetNotificationSetting(ref ruleList);
                // RuleManager.GetInstance().SaveUpdateRule(ruleList);
                //foreach (RuleBase rule in ruleList)
                //{
                //    if (rule.Category == RuleCategory.UserDefined)
                //    {
                //        RuleManager.GetInstance().OperateOnRule(rule.Package, rule.Category, rule.RuleName, RuleOperations.Build, rule.RuleId, "");

                //    }
                //}
                //}
                //else
                //{
                //RaiseUpdateStatusBarEvent(RuleOperations.None, "Saving Rule Canceled for " + ruleList.Count + " rules", true);
                //}
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
        /// Unbinds all Events.
        /// </summary>
        /// <returns></returns>
        internal bool CloseStart()
        {
            try
            {
                RaiseUpdateStatusBarEvent(RuleOperations.None, "Closing Form");
                RuleManager.GetInstance().AllRulesLoaded -= RuleDefinitionMain_AllRulesLoaded;
                ruleNavigator.OpenSelectedRule -= ruleNavigator_OpenSelectedRule;
                ruleNavigator.OperateOnRule -= ruleNavigator_OperationsOnRule;
                RuleManager.GetInstance().RuleOperationCompleted -= RuleDefinitionMain_RuleOperationCompleted;
                RuleManager.GetInstance().RuleOperationFromDifferentClient -= RuleDefinitionMainNew_RuleOperationFromDifferentClient;
                ruleDefViewer.RuleDefBrowserSaveCompleteEvent -= ruleDefViewer_RuleDefBrowserSaveCompleteEvent;
                //RuleManager.GetInstance().Dispose();
                //RuleManager.DisposeInstance();
                ruleNavigator.Dispose();
                notificationSettings.Dispose();
                ruleDefViewer.Dispose();
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

        /// <summary>
        /// Binds all event Initializes control when control is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleDefinitionMainNew_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    BindAllEvents();
                    InitializeRules();
                    ultraBtnSave.Visible = false;
                    ultraBtnReload.Visible = false;
                    RaiseUpdateStatusBarEvent(RuleOperations.None, "Loading Form");
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

        private void SetAppearanceWithoutTheme()
        {
            try
            {
                Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();

                appearance1.BackColor = System.Drawing.Color.Silver;
                this.ultraPnlMain.Appearance = appearance1;
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
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                ultraBtnReload.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnReload.ForeColor = System.Drawing.Color.White;
                ultraBtnReload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnReload.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnReload.UseAppStyling = false;
                ultraBtnReload.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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


        /// <summary>
        /// Raises Event to Compliance Engine form for updating Status bar.
        /// </summary>
        /// <param name="ruleOperations"></param>
        /// <param name="status"></param>
        private void RaiseUpdateStatusBarEvent(RuleOperations ruleOperations, string status, bool isCompleteState = false)
        {
            try
            {
                StringBuilder statusBuilder = new StringBuilder();
                if (ruleOperations != RuleOperations.None)
                {
                    statusBuilder.Append(ruleNavigator.SplitCamelCase(ruleOperations.ToString()));
                    statusBuilder.Append(" - ");
                }
                statusBuilder.Append(status);
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new StatusBarEventArgs { Status = statusBuilder.ToString(), IsCompleteState = isCompleteState });
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
        /// Reloads rule browser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnReload_Click(object sender, EventArgs e)
        {
            try
            {
                List<String> ruleIdList = ruleNavigator.GetRuleId();
                List<RuleBase> ruleList = new List<RuleBase>();
                foreach (String ruleId in ruleIdList)
                {
                    RuleBase rule = RuleManager.GetInstance().GetRule(ruleId);
                    ruleList.Add(rule);
                }
                ruleDefViewer.UpdateDictionary(ruleList, "");
                //Loading rule in control.
                //Need to change when multiple rule save will be implemented
                if (ruleList.Count == 1)
                    ruleDefViewer.LoadDocument(ruleList[0].RuleURL, true);
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

        public void ExportData(string gridName, string filePath)
        {
            try
            {
                this.ruleNavigator.ExportUltraTreeToExcel(gridName, filePath);
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
