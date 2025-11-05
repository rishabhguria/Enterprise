using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using Prana.RuleEngine.BussinessObjects;
using Newtonsoft.Json;

using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Prana.RuleEngine.Utility;
using Prana.Interfaces;
using Prana.BusinessObjects;
using Prana.AmqpAdapter;
using Prana.Global;
using Prana.RuleEngine.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Prana.RuleEngine.DataAccessControl;
using Prana.RuleEngine.BusinessLogic;
using Infragistics.Win.UltraWinListView;
using System.Text.RegularExpressions;
using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Interfaces;
using Prana.CommonDataCache;
using Infragistics.Win.UltraWinGrid;
using System.Xml.Serialization;
using Prana.Utilities.MiscUtilities;
using Prana.RuleEngine.ImportExport;
using Prana.BusinessObjects.Classes.CompliancePref;
using Prana.RuleEngine.UserControls;



namespace Prana.RuleEngine
{
    /// <summary>
    /// Rule engine main form
    /// </summary>
    public partial class RuleEngineWin : Form, IPluggableTools
    {

        #region Private properties
        delegate void MainThreadDelegate(DataSet dsReceived, String mediaName, MediaType mediaType, String routingKey);
        delegate void MainThreadDelegateTreeView(object sender, RunWorkerCompletedEventArgs e);
        delegate void MainThreadDelegateNotify(DataSet dsReceived, String mediaName, MediaType mediaType, String routingKey);
       
        /// <summary>
        /// Currently logged in user
        /// </summary>
        CompanyUser _companyUser = new CompanyUser();

        /// <summary>
        /// Permissions for each user
        /// </summary>
        UserPermission _userPermissions = new UserPermission();

        private ISecurityMasterServices _secMasterServices;

        /// <summary>
        /// List of sixteenB rules in form of dictionary
        /// </summary>
        Dictionary<String, List<DateTime>> _sixteenBRuleList = new Dictionary<string, List<DateTime>>();
        List<IAmqpReceiver> receiverInstances = new List<IAmqpReceiver>();
        //String _amqpHostName;
        String _ruleBuildRequestExchange;
        String _ruleBuildResponseExchange;

        String _selectedPackageName = "";
        String _lastSelectedPackageName = "";

        String _selectedRuleText = "";
        String _newRuleNameText = "";

        //String _selectedCompressionLevel = "";
        //int _selectedCompressionLevelID = -1;
        //String _oldCompressionLevel = "";
        // Boolean _isUpdateCompressionRequest = false;
        Boolean _isCreateUserDefinedRuleRequest = false;
        Boolean _isRenameUserDefinedRuleRequest = false;
        Boolean _isSaveUserDefineRuleRequest = false;
        // Boolean _isDeleteUserDefinedRuleRequest = false;
        Boolean _isBuildPackageRequest = false;
        bool _isPowerUser = false;
        private TreeNode _selectedTreeNode = null;
        private TreeNode lastSelectedNode = null;
        TreeNode _userDefinedRulesNode = null;
        TreeNode _customRuleNode = null;

        //**
        bool _isWashSalePresent = false;
        bool _isWashSaleEnabled = false;
        String _notificationExchange;
        private DataSet _ruleHistDS = new DataSet();
        Dictionary<String, Dictionary<String, RulesAsset>> _userDefinedRulesDict = new Dictionary<string, Dictionary<string, RulesAsset>>();
       
        SixtenBControl _sbCntrl = new SixtenBControl();
        IWebBrowserControl _webBrowserCntrl = null;
        NotificationControl _notificationCntrl = new NotificationControl();
        ImageList _myImageList = new ImageList();
        String _oldRuleName = "";
        public object _lockerObject = new object();
        CompliancePref _compliancePref = new CompliancePref();
        #endregion

        

        #region constructor
        public RuleEngineWin()
        {
            //**
            _isWashSalePresent = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // String xulrunnerPath = ConfigurationHelper.Instance.GetAppSettingValueByKey("XulRunnerPath");
                // Skybound.Gecko.Xpcom.Initialize(xulrunnerPath);

                _companyUser = Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
                InitializeComponent();
               

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
                //MessageBox.Show("Problem in Rule Engine!!");
            }
        }

        #endregion

        #region private Methods
        /// <summary>
        /// New alert is recieved and added to alert history grid.
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        void Amqp_Alert_received(DataSet dsReceived, String mediaName, MediaType mediaType, String routingKey)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                if (this.InvokeRequired)
                {
                    MainThreadDelegateNotify del = this.Amqp_Alert_received;
                    this.Invoke(del, new object[] { dsReceived, mediaName, mediaType, routingKey });
                }
                else
                {
                    if (dsReceived.Tables.Count > 0)
                    {
                        if (rdCurrent.Checked)
                        {
                            AddRowToAlert(dsReceived);
                        }

                    }
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
        /// <summary>
        /// Adding new alert to AlertHistory Grid. 
        /// </summary>
        /// <param name="dsReceived"></param>
        private void AddRowToAlert(DataSet dsReceived)
        {
            try
            {
                String defaultFieldsValue = "N/A";

                foreach (DataRow dataRow in dsReceived.Tables[0].Rows)
                {
                    String compressionLevel = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("CompressionLevel"))
                        compressionLevel = dataRow["CompressionLevel"].ToString();

                    String alertComment = dataRow["Summary"].ToString();
                    String validationTime = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("ValidationTime"))
                        validationTime = dataRow["ValidationTime"].ToString();

                    String ruleType = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("RuleType"))
                        ruleType = dataRow["RuleType"].ToString();


                    String ruleName = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("name"))
                        ruleName = MessageFormatter.FormatRuleNameForDisplay(dataRow["name"].ToString());

                    String orderId = defaultFieldsValue;
                    if (ruleType.Equals(Constants.PRE_TRADE))
                    {
                        if (dsReceived.Tables[0].Columns.Contains("OrderId"))
                            orderId = dataRow["OrderId"].ToString();
                    }
                    String userName = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("UserId"))
                    {
                        int id = Convert.ToInt32(dataRow["UserId"].ToString());
                        if (id != 0)
                            userName = CachedDataManager.GetInstance.GetUserText(id);
                        if (string.IsNullOrEmpty(userName))
                        {
                            userName = defaultFieldsValue;
                        }

                    }


                    String curParameter = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("Parameters"))
                        curParameter = dataRow["Parameters"].ToString();
                    String status = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("Status"))
                        status = dataRow["Status"].ToString();
                    String alertDescription = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("Description"))
                        alertDescription = dataRow["Description"].ToString();
                    String dimension = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("dimension"))
                        dimension = dataRow["dimension"].ToString();
                    _ruleHistDS.Tables[0].Rows.Add(new object[] { ruleName, userName, ruleType, alertComment, compressionLevel, curParameter, validationTime, orderId, status, alertDescription, dimension });
                    //Refreshing ultragrid to sort according to new alert. 
                    //http://jira.nirvanasolutions.com:8080/browse/COMPALERT-235
                    ultraAlertGrid.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                    ultraAlertGrid.Refresh();

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



        ///// <summary>
        ///// initialise treeview control for each package and rule type
        ///// </summary>
        //private void AddRulesToTreeView()
        //{
        //    try
        //    {
        //      //  _bgWorker.DoWork += new DoWorkEventHandler(_bgWorker_DoWork);
        //     //   _bgWorker.RunWorkerAsync();



        //    }
        //    catch (Exception ex)
        //    {
        //        //// Invoke our policy that is responsible for making sure no secure information
        //        //// gets out of our layer.

        //        //bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

        //        //if (rethrow)
        //        //{
        //        //    throw;
        //        //}

        //        MessageBox.Show("Please check for Rule Server started!!", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}



        /// <summary>
        /// Loads app settings from config file
        /// </summary>
        private void LoadAppSettings()
        {
            try
            {
                //_amqpHostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_AmqpServer);
                _ruleBuildRequestExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RuleRequestExchange);
                _ruleBuildResponseExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RuleResponseExchange);
                _isWashSaleEnabled = Convert.ToBoolean(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_IsWashSaleEnabled));
                _notificationExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);
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
        /// Initialize listener for AMQP instances
        /// </summary>
        private void InitializeAMQPListeners()
        {
            try
            {
                //Sender instance will be used to send request for rebuilding of rule package
                AmqpHelper.InitializeSender("Build",  _ruleBuildRequestExchange, MediaType.Exchange_Fanout);

                //Initialize Amqp listener so that rebuild response will be used to refresh controls
                AmqpHelper.Started += new ListenerStarted(amqpHelperSummary_started);
                AmqpHelper.InitializeListenerForExchange( _ruleBuildResponseExchange, MediaType.Exchange_Fanout, null);
                List<String> key = new List<string>();
                key.Add(_companyUser.CompanyUserID.ToString());
                key.Add("PostAlertFromNotificationManager");
                AmqpHelper.InitializeListenerForExchange( _notificationExchange, MediaType.Exchange_Direct, key);
                AmqpHelper.InitializeSender("RuleSaveSender",  _notificationExchange, MediaType.Exchange_Direct);

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



        void amqpHelperSummary_started(IAmqpReceiver amqpReceiver)
        {
            try
            {
                if (amqpReceiver.MediaName == _ruleBuildResponseExchange)
                {
                    amqpReceiver.AmqpDataReceived += new DataReceived(amqpHelperSummary_amqpDataRecieved);
                    lock (receiverInstances)
                    {
                        receiverInstances.Add(amqpReceiver);
                    }
                }
                else if (amqpReceiver.MediaName == _notificationExchange)
                {
                    amqpReceiver.AmqpDataReceived += new DataReceived(Amqp_Alert_received);
                    lock (receiverInstances)
                    {
                        receiverInstances.Add(amqpReceiver);
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



        #endregion

        #region rules tree private methods
        private void SetRuleNodeViewAndContextMenu(String PackageName, String RuleName)
        {
            try
            {
                if (_selectedTreeNode.Parent == null)
                    return;

                if (_selectedTreeNode.Parent.Tag.Equals(Constants.CUSTOM_RULE_NODE_TAG))
                {
                    CustomRule rule = CustomRuleManager.GetInstance().GetRuleByRuleId(PackageName, RuleName);
                    if (rule == null)
                    {
                        _selectedTreeNode.Remove();
                        return;
                    }
                    enableRuleToolStripMenuItem.Visible = !(rule.Enabled);
                    disableRuleToolStripMenuItem.Visible = rule.Enabled;
                    if (!rule.Enabled)
                    {
                        _selectedTreeNode.ImageIndex = 5;
                        _selectedTreeNode.SelectedImageIndex = 5;

                    }
                    else
                    {
                        _selectedTreeNode.ImageIndex = 1;
                        _selectedTreeNode.SelectedImageIndex = 1;
                    }
                }
                else
                {
                    if (_userDefinedRulesDict.ContainsKey(PackageName))
                    {
                        if (_userDefinedRulesDict[PackageName].ContainsKey(RuleName))
                        {
                            RulesAsset rule = _userDefinedRulesDict[PackageName][RuleName];
                            enableRuleToolStripMenuItem.Visible = rule.metadata.disabled;
                            disableRuleToolStripMenuItem.Visible = !rule.metadata.disabled;

                            if (rule.metadata.disabled)
                            {
                                _selectedTreeNode.ImageIndex = 5;
                                _selectedTreeNode.SelectedImageIndex = 5;

                            }
                            else
                            {
                                //if (_selectedPackageName.Equals(Constants.POST_TRADE_COMPLIANCE))
                                //{
                                //    _selectedTreeNode.ImageIndex = 2;
                                //    _selectedTreeNode.SelectedImageIndex = 2;
                                //}
                                //else
                                //{
                                _selectedTreeNode.ImageIndex = 1;
                                _selectedTreeNode.SelectedImageIndex = 1;
                                //}
                            }

                        }

                    }
                }
                exportRuleToolStripMenuItem.Visible = true;
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
        private void AddUserDefinedRulesToTree(string PackageName, Dictionary<String, RulesAsset> rules)
        {
            try
            {
                TreeNodeCollection rootNodes1 = treeView1.Nodes[0].Nodes;

                TreeNode packageNode = FindPackageNodeInTreeView(PackageName, rootNodes1);
                // treeView1.SelectedNode = FindPackageNodeInTreeView(PackageName, rootNodes1);

                if (packageNode == null)
                    return;
                foreach (TreeNode node in packageNode.Nodes)
                {
                    if (node.Tag == Constants.USER_DEFINED_RULES_TAG.ToString())
                    {
                        node.Remove();
                    }
                }
                //packageNode.Nodes.Clear();
                TreeNode userRuleNode = new TreeNode();
                userRuleNode.Name = "UserDefinedRules";
                userRuleNode.Text = "User Defined Rules";
                userRuleNode.Tag = Constants.USER_DEFINED_RULES_TAG;
                userRuleNode.ImageIndex = 0;
                userRuleNode.SelectedImageIndex = 0;

                packageNode.Nodes.Add(userRuleNode);
                TreeNode selectedNode = null;

                foreach (KeyValuePair<String, RulesAsset> asset in rules)
                {

                    if (asset.Value.metadata.format.Equals(Constants.RULE_FILE_FORMATE))
                    {
                        String category = asset.Value.description;
                        String ruleName = asset.Value.title;
                        TreeNodeCollection rootNodes = treeView1.Nodes[0].Nodes;
                        //treeView1.SelectedNode = FindPackageNodeInTreeView(PackageName, rootNodes);
                        // treeView1.SelectedNode = userRuleNode;
                        if (userRuleNode != null)
                        {
                            TreeNode nod = new TreeNode();
                            nod.Name = asset.Value.metadata.uuid;
                            // = asset.Value.metadata.disabled.ToString();
                            nod.Text = MessageFormatter.FormatRuleNameForDisplay(ruleName);
                            //nod.Text = ruleName;
                            nod.Tag = Constants.RULE_TAG;

                            if (asset.Value.metadata.disabled)
                            {
                                nod.ToolTipText = "Disabled";
                                nod.ImageIndex = 5;
                                nod.SelectedImageIndex = 5;
                            }
                            else
                            {
                                nod.ToolTipText = "Enabled";
                                nod.ImageIndex = 1;
                                nod.SelectedImageIndex = 1;
                            }


                            userRuleNode.Nodes.Add(nod);
                            if (_selectedTreeNode != null && ruleName.Equals(_selectedTreeNode.Text))
                            {
                                selectedNode = nod;
                            }
                            //else if (_selectedPackageName!="" && _selectedPackageName != PackageName )
                            //{
                            //    treeView1.SelectedNode = userRuleNode;
                            //}
                        }

                    }
                }
                if (selectedNode != null && _selectedPackageName == PackageName)
                {
                    treeView1.SelectedNode = selectedNode;
                }
                else if (_selectedPackageName == "")
                    treeView1.SelectedNode = treeView1.Nodes[0];
                    //treeView1.SelectedNode = userRuleNode;
                if (!treeView1.IsDisposed)
                    treeView1.Sort();
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
            treeView1.ExpandAll();
        }

         
        private TreeNode FindPackageNodeInTreeView(string PackageName, TreeNodeCollection Nodes)
        {
            TreeNode foundNode = null;
            bool isFoundNode = false;
            try
            {
                foreach (TreeNode node in Nodes)
                {
                    if (node.Name.Equals(PackageName))
                    {
                        foundNode = node;
                        isFoundNode = true;
                        break;
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
                //MessageBox.Show("Problem in findin node !!");
            }
            return foundNode;
        }

        private void SetPackagePermissions(String packageName)
        {
            try
            {
                bool canAccess = RulesManager.CheckPermissionsOnPackage(packageName, _companyUser.CompanyUserID);
                if (canAccess)
                {
                    //categoryContextMenuStrip.Enabled = true;
                    enableAllToolStripMenuItem.Enabled = true;
                    disableAllToolStripMenuItem.Enabled = true;
                    addCategoriesToolStripMenuItem.Enabled = true;

                    deleteRuleToolStripMenuItem1.Enabled = true;
                    openRuleToolStripMenuItem.Visible = true;
                    renameRuleToolStripMenuItem.Visible = true;

                    enableRuleToolStripMenuItem.Enabled = true;
                    disableRuleToolStripMenuItem.Enabled = true;
                    //editComlianceLevelToolStripMenuItem.Visible = true;
                    panelWebControl.Enabled = true;
                    ultraExpandableGroupBox1.Enabled = true;
                    exportAllRulesToolStripMenuItem.Visible = true;
                    importRuleToolStripMenuItem.Visible = true;
                    importRuleToolStripMenuItem.Enabled = true;
                    //ApplyRuleBtn.Enabled = true;
                }
                else
                {
                    //categoryContextMenuStrip.Enabled = false;
                    enableAllToolStripMenuItem.Enabled = false;
                    disableAllToolStripMenuItem.Enabled = false;
                    addCategoriesToolStripMenuItem.Enabled = false;

                    deleteRuleToolStripMenuItem1.Enabled = false;
                    renameRuleToolStripMenuItem.Visible = false;
                    //editComlianceLevelToolStripMenuItem.Visible = false;
                    openRuleToolStripMenuItem.Visible = true;
                    ultraExpandableGroupBox1.Enabled = false;
                    panelWebControl.Enabled = false;
                    enableRuleToolStripMenuItem.Enabled = false;
                    disableRuleToolStripMenuItem.Enabled = false;
                    exportAllRulesToolStripMenuItem.Visible = true;
                    exportAllRulesToolStripMenuItem.Enabled = true;
                    importRuleToolStripMenuItem.Visible = true;
                    importRuleToolStripMenuItem.Enabled = false;
                    //ApplyRuleBtn.Enabled = false;


                }
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
                //MessageBox.Show("Problem in setting permissions on project level!!");
            }
        }

        /// <summary>
        /// Adding basic Package Level Structure of tree view. 
        /// </summary>
        private void InitializeTreeViewStuct()
        {
            try
            {
                //Adding icon to treeView nodes.
                //ico files added to properties.resources

                Size size = new Size();
                size.Height = 12;
                size.Width = 12;
                _myImageList.ImageSize = size;

                _myImageList.Images.Add(Properties.Resources.Category);

                _myImageList.Images.Add(Properties.Resources.PreEnable);
                _myImageList.Images.Add(Properties.Resources.PreEnable);

                _myImageList.Images.Add(Properties.Resources.RootNode);
                _myImageList.Images.Add(Properties.Resources.riskicon);
                _myImageList.Images.Add(Properties.Resources.Disable);
                treeView1.ImageList = _myImageList;

                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new TreeNode("Compliance Rules", 4, 4));
                TreeNode inTreeNode = treeView1.Nodes[0];
                inTreeNode.Tag = "RootNode";

                TreeNode nodPreTrade = new TreeNode();
                nodPreTrade.Name = "PreTradeCompliance";
                nodPreTrade.Text = "Pre Trade Compliance";
                nodPreTrade.Tag = Constants.CATEGORY_TAG;
                inTreeNode.Nodes.Add(nodPreTrade);

                TreeNode nodPostTrade = new TreeNode();
                nodPostTrade.Name = "PostTradeCompliance";
                nodPostTrade.Text = "Post Trade Compliance";
                nodPostTrade.Tag = Constants.CATEGORY_TAG;
                inTreeNode.Nodes.Add(nodPostTrade);

                

             
                LoadRulesFromGuvnor();
                //LoadCustomRule();

                if (!ComplianceCacheManager.GetPostComplianceModuleEnabled())
                    nodPostTrade.Remove();
                if (!ComplianceCacheManager.GetPreComplianceModuleEnabled())
                    nodPreTrade.Remove();


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

        private void LoadCustomRule()
        {
            try
            {


                try
                {
                    AddCustomRuleNode("PreTrade", CustomRuleManager.GetInstance().GetAllRules("PreTradeCompliance"));
                    AddCustomRuleNode("PostTrade", CustomRuleManager.GetInstance().GetAllRules("PostTradeCompliance"));
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

                //Dictionary<String, Dictionary<String, CustomRule>> customRuleCache = new Dictionary<string, Dictionary<string, CustomRule>>();
                //Dictionary<String, CustomRule> preCustomRuleCache = CustomRuleManager.GetInstance().GetAllRules("PreTrade");
                //Dictionary<String, CustomRule> postCustomRuleCache = CustomRuleManager.GetInstance().GetAllRules("PostTrade");

                //customRuleCache.Add("PreTradeCompliance", preCustomRuleCache);
                //customRuleCache.Add("PostTradeCompliance", postCustomRuleCache);

                //foreach (String key in customRuleCache.Keys)
                //{
                //    if (customRuleCache[key].Values.Count > 0)
                //        AddCustomRuleNode(key, customRuleCache[key]);
                //}
                

                //foreach (String key in postCustomRuleCache.Keys)
                //{
                //    AddCustomRuleNode(key, postCustomRuleCache[key]);
                //}

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

        private void AddCustomRuleNode(string key, Dictionary<String, CustomRule> customRuleDict)
        {

            try
            {
                TreeNodeCollection rootNodes1 = treeView1.Nodes[0].Nodes;
                String packageName = key + "Compliance";
                TreeNode packageNode = FindPackageNodeInTreeView(packageName, rootNodes1);
                if (packageNode == null)
                    return;


                Boolean isCustomRuleAlreadyPresent = false;
                TreeNode customRuleNode = null;
                foreach (TreeNode node in packageNode.Nodes)
                {
                    if (node.Tag == Constants.CUSTOM_RULE_NODE_TAG.ToString())
                    {
                        isCustomRuleAlreadyPresent = true;
                        customRuleNode = node;
                        customRuleNode.Nodes.Clear();
                        break;
                    }
                }
                //TreeNode customRuleNode;
                //if (isCustomRuleAlreadyPresent)
                //    customRuleNode = packageNode.Nodes[Constants.CUSTOM_RULE_NODE_TAG];
                if (customRuleNode == null)
                {
                    customRuleNode = new TreeNode();
                    customRuleNode.Name = "CustomRules";
                    customRuleNode.Text = "Custom Rules";
                    customRuleNode.Tag = Constants.CUSTOM_RULE_NODE_TAG;
                    customRuleNode.ImageIndex = 0;
                    customRuleNode.SelectedImageIndex = 0;
                }
                foreach (KeyValuePair<String, CustomRule> customRule in customRuleDict)
                {
                    if (customRuleNode != null)
                    {
                        TreeNode nod = new TreeNode();
                        nod.Name = customRule.Value.RuleId;
                        nod.Text = customRule.Value.Name;
                        nod.Tag = Constants.CUSTOM_RULE_TAG;

                        if (!customRule.Value.Enabled)
                        {
                            nod.ToolTipText = "Disabled";
                            nod.ImageIndex = 5;
                            nod.SelectedImageIndex = 5;
                        }
                        else
                        {
                            nod.ToolTipText = "Enabled";
                            nod.ImageIndex = 1;
                            nod.SelectedImageIndex = 1;
                        }

                        customRuleNode.Nodes.Add(nod);
                    }
                }

                //Boolean isCustomRuleAlreadyPresent = false;
                //foreach (TreeNode node in packageNode.Nodes)
                //{
                //    if (node.Tag == customRuleNode.Tag)
                //    {
                //        isCustomRuleAlreadyPresent = true;
                //        break;
                //    }
                //}

                if (!isCustomRuleAlreadyPresent)
                    packageNode.Nodes.Add(customRuleNode);
                treeView1.Sort();
                _notificationCntrl.SynchronizeCustomRules(key+"Compliance",customRuleDict);
                Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                savedMessage.Add("ApplicationStatus", "Saved");
                AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
                
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

        private void LoadRulesFromGuvnor()
        {
            try
            {
                BackgroundWorker bgLoadRules = new BackgroundWorker();
                bgLoadRules.DoWork += new DoWorkEventHandler(bgLoadRules_DoWork);
                bgLoadRules.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgLoadRules_RunWorkerCompleted);
                bgLoadRules.RunWorkerAsync();
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

        void bgLoadRules_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    _userDefinedRulesDict = e.Result as Dictionary<String, Dictionary<String, RulesAsset>>;
                    // if(_userDefinedRulesDict

                    if (_userDefinedRulesDict.ContainsKey(Constants.POST_TRADE_COMPLIANCE))
                    {
                        AddUserDefinedRulesToTree(Constants.POST_TRADE_COMPLIANCE, _userDefinedRulesDict[Constants.POST_TRADE_COMPLIANCE]);
                        _notificationCntrl.SynchronizeUserDefinedRules(Constants.POST_TRADE_COMPLIANCE, _userDefinedRulesDict[Constants.POST_TRADE_COMPLIANCE]);
                    }
                    if (_userDefinedRulesDict.ContainsKey(Constants.PRE_TRADE_COMPLIANCE))
                    {
                        AddUserDefinedRulesToTree(Constants.PRE_TRADE_COMPLIANCE, _userDefinedRulesDict[Constants.PRE_TRADE_COMPLIANCE]);
                        _notificationCntrl.SynchronizeUserDefinedRules(Constants.PRE_TRADE_COMPLIANCE, _userDefinedRulesDict[Constants.PRE_TRADE_COMPLIANCE]);
                    }
                    //_notificationCntrl.SynchronizeUserDefinedRules(_userDefinedRulesDict);
                    Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                    savedMessage.Add("ApplicationStatus", "Saved");
                    AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");

                }
                else
                    throw e.Error;
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

        void bgLoadRules_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                e.Result = RulesManager.GetRulesListFromWebRepo();
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

        private void CreateUserDefinedRule(String ruleNameText)
        {
            try
            {
                treeView1.SelectedNode.Tag = Constants.RULE_TAG;
                _selectedTreeNode = treeView1.SelectedNode;
                Boolean isRuleCreated = false;
                if (ruleNameText != "")
                {
                    _selectedTreeNode.Text = ruleNameText;
                    Cursor.Current = Cursors.WaitCursor;
                    isRuleCreated = RulesManager.CreateRulewithTemplate(_selectedPackageName, ruleNameText);

                    if (isRuleCreated)
                    {
                        
                        OpenUserDefinedRule();
                        _selectedRuleText = ruleNameText;
                        EnableDisableRule(true);
                        RulesAsset asset = RulesManager.GetAssetMetaData(_newRuleNameText, _selectedPackageName);
                        if (_userDefinedRulesDict.ContainsKey(_selectedPackageName))
                        {
                            if (!_userDefinedRulesDict[_selectedPackageName].ContainsKey(ruleNameText))
                                _userDefinedRulesDict[_selectedPackageName].Add(ruleNameText, asset);
                        }

                        String ruleId = System.Guid.NewGuid().ToString();
                        //_notificationCntrl.CreateNotificationSettings(_selectedPackageName, _newRuleNameText, asset.metadata.uuid, ruleId);
                        ultraExpandableGroupBox1.Enabled = true;
                        treeView1.Focus();
                    }
                    Cursor.Current = Cursors.Default;
                    
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

        private void RedirectPage(string Url)
        {
            try
            {
                _webBrowserCntrl.Navigate(Url);

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

        private void OpenUserDefinedRule()
        {
            try
            {
                String ruleName =MessageFormatter.FormatRuleNameForGuvnor(_selectedTreeNode.Text);
                String uuid = _selectedTreeNode.Name;
                if (uuid == "" || uuid == null)
                {
                    uuid = RulesManager.GetUUIDOfAssets(ruleName, _selectedPackageName);
                }
                if (uuid != "")
                {
                    String clientName = "admin";//Constants.CLIENT_NAME;

                    String param = "client=" + clientName + "&assetsUUIDs=" + uuid + "&isPowerUser=" + _isPowerUser;
                    RedirectPage(Constants.GUVNOR_STANDALONE_BASE_URL + param);

                }
                ultraExpandableGroupBoxPanel1.Enabled = true;
                if (_selectedPackageName != "PreTradeCompliance")
                {
                    _notificationCntrl.ToggleFrequencyVisibility(true);
                }
                else
                {
                    _notificationCntrl.ToggleFrequencyVisibility(false);
                }
                _notificationCntrl.SetNotificationSettings(ruleName, _selectedPackageName);
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

        #endregion


        #region IPluggableTools Members

        public void SetUP()
        {
            try
            {
                LoadAppSettings();
                InitializeAMQPListeners();
                /// InitializeWebControl("Gecko");
                InitializeWebControl("OpenWebkit");
                Constants.CLIENT_NAME = _companyUser.FirstName;
                Constants.CLIENT_PASSWORD = _companyUser.FirstName;// "a";
                _isPowerUser = ComplianceCacheManager.GetPowerUserCheck(_companyUser.CompanyUserID);
                ultraExpandableGroupBoxPanel1.Controls.Add(_notificationCntrl);
                ultraExpandableGroupBoxPanel1.Visible = true;

                /// Seems to be ultraExpandableGroupBox1 issue that we have to expand it at least once so that it start showing up the contents.
                ultraExpandableGroupBox1.Expanded = false;
                ultraExpandableGroupBox1.Expanded = true;
                //ultraExpandableGroupBox1.Expanded = true;
                ultraExpandableGroupBoxPanel1.Enabled = false;

                _notificationCntrl.Dock = DockStyle.Fill;
                _sbCntrl.cancelClick += new EventHandler(SBCntrl_cancelClick);
                _sbCntrl.submitClick += new SixtenBEventDelegate(SBCntrl_submitClick);
                //NotificationHelper.alertEvent += new AlertDelegate(notify_alertEvent);
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

        private void InitializeWebControl(String ControlName)
        {

            try
            {
                switch (ControlName)
                {
                    case "Gecko":
                        //  _webBrowserCntrl = new GeckoWebControl();
                        break;

                    case "OpenWebkit":
                        OpenWebkitWebControl openWebkit = new OpenWebkitWebControl();
                        openWebkit.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
                        _webBrowserCntrl = openWebkit;

                        break;
                    default:
                        break;


                }
                UserControl webControl = (UserControl)_webBrowserCntrl;

                webControl.Dock = DockStyle.Fill;
                panelWebControl.Controls.Add(webControl);
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

        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (_selectedTreeNode != null)
                {
                    if (_selectedTreeNode.Tag.ToString().Equals(Constants.RULE_TAG))
                    {
                        btnSaveRule.Enabled = true;
                        if (_isSaveUserDefineRuleRequest)
                        {
                            _isBuildPackageRequest = true;

                            //get row of selecte rule.
                            String ruleId = _notificationCntrl.GetRuleId();
                            if (ruleId != "")
                            {
                                AmqpHelper.SendObject("UpdateNotification," + ruleId, "Build", null);

                            }
                            AmqpHelper.SendObject("Build", "Build", null);
                            ////ApplyRuleBtn.Enabled = false;
                            _isSaveUserDefineRuleRequest = false;
                        }
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
        /// <summary>
        /// Grid View added.
        /// http://jira.nirvanasolutions.com:8080/browse/COMPALERT-226
        /// </summary>
        private void InitializeAlertHistView()
        {
            try
            {
                dTPFrom.Value = DateTime.Today;
                dTPTill.Value = DateTime.Today;
                _ruleHistDS = RulesDAO.GetComplianceAlertHist(DateTime.Today, DateTime.Today);
                ultraAlertGrid.DataSource = _ruleHistDS;
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].Format = "MM/dd/yyyy HH:mm:ss";
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["Description"].MinWidth = 300;
                ultraAlertGrid.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].SortIndicator = SortIndicator.Descending;
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["Compression Level"].Header.Caption = "Compliance Level";
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["Dimension"].Header.Caption = "Compliance Level Value";
                //ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].SortIndicator = SortIndicator.Descending;
                //DataTable tempDT = ruleHistDS.Tables[0];
                //if (tempDT.Rows.Count > 0)
                //    AddAlertToAlerthistView(tempDT);
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

        void SBCntrl_submitClick(object sender, string symbol, DateTime date)
        {
            try
            {
                if (symbol != String.Empty)
                {

                    SixteenBDataHelper.SaveAndSendSymbolToExchange(symbol, date, _companyUser.CompanyUserID.ToString());
                    SixteenBDataHelper.AddSixteenBNodeToTree(ref treeView1);
                    //MessageBox.Show(symbol + " rule " + date);
                }
                else
                    MessageBox.Show(this, "Please enter symbol", "Nirvana Compliance");
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
            //throw new Exception("The method or operation is not implemented.");
        }

        //private void SaveAndSendSymbolToExchange(string symbol, DateTime date)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("RuleId");
        //        dt.Columns.Add("Symbol");
        //        dt.Columns.Add("StartDate");
        //        dt.Columns.Add("CreatedBy");
        //        dt.Rows.Add(new object[] { Guid.NewGuid(), symbol, date, companyUser.CompanyUserID });
        //        ds.Tables.Add(dt);
        //        int res = SaveSixteenBRules(ds);
        //        AmqpAdapter.AmqpHelper helper = new AmqpHelper(ConfigurationHelper.Instance.GetAppSettingValueByKey("AmqpServer"));
        //        helper.SendDataSet(ds, ConfigurationHelper.Instance.GetAppSettingValueByKey("RuleSaveQueueName"));
        //        if (res > 0)
        //            MessageBox.Show("Rule saved and applied");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}


        void SBCntrl_cancelClick(object sender, EventArgs e)
        {
            try
            {
                _sbCntrl.Visible = false;
                panelWebControl.Visible = true;
                Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                RedirectPage(urlPath.AbsoluteUri);
                if (treeView1.Nodes.Count > 0)
                    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
                ultraExpandableGroupBoxPanel1.Enabled = false;
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

        void amqpHelperSummary_amqpDataRecieved(DataSet dsReceived, String mediaName, MediaType mediaType, String routingKey)
        {
            try
            {
                if (mediaName == _ruleBuildResponseExchange)
                {
                    String response = (dsReceived.Tables[0].Rows[0][0] as String);
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            MainThreadDelegate del = this.amqpHelperSummary_amqpDataRecieved;
                            this.Invoke(del, new object[] { dsReceived, mediaName, mediaType, routingKey });
                        }
                        else
                        {
                            if (response.Contains("Build"))
                            {
                                if (_isBuildPackageRequest)
                                {

                                    if (response.Equals("Build:true"))
                                    {
                                        MessageBox.Show(this, "Rules are now live.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                        if (_isPowerUser)
                                            MessageBox.Show(this, response + "\nPlease check your recently created/modified rule !!", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        else
                                            MessageBox.Show(this, "Error while creating/updating the rule. Please contact Nirvana Support !!", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Error);


                                }
                                _isBuildPackageRequest = false;
                                LoadRulesFromGuvnor();
                            }

                            else if (response.Contains("Rename"))
                            {
                                lock (_lockerObject)
                                {
                                    String[] strArray = response.Split(',');

                                    if (strArray[0].Equals("Rename:true"))
                                    {
                                        if (_isRenameUserDefinedRuleRequest)
                                        {
                                            String infoMessage = "";
                                            infoMessage = "Successfuly renamed the rule from \"" + MessageFormatter.FormatRuleNameForDisplay(_oldRuleName) + "\" to \"" + MessageFormatter.FormatRuleNameForDisplay(_newRuleNameText) + "\"";
                                            _isRenameUserDefinedRuleRequest = false;


                                            RulesAsset rule = RulesManager.GetAssetMetaData(_newRuleNameText, _selectedPackageName);
                                            if (_userDefinedRulesDict.ContainsKey(_selectedPackageName))
                                            {
                                                Boolean isDisabled = true;
                                                if (_userDefinedRulesDict[_selectedPackageName].ContainsKey(_oldRuleName))
                                                {
                                                    RulesAsset oldRule = _userDefinedRulesDict[_selectedPackageName][_oldRuleName];
                                                    isDisabled = oldRule.metadata.disabled;

                                                    _userDefinedRulesDict[_selectedPackageName].Remove(_oldRuleName);

                                                }
                                                bool isDeleted = RulesManager.DeleteUserDefinedRule(_oldRuleName, _selectedPackageName);

                                                RulesManager.EnableDisableUserDefnedRule(_selectedPackageName, _newRuleNameText, isDisabled);
                                                if (!_userDefinedRulesDict[_selectedPackageName].ContainsKey(_newRuleNameText))
                                                {
                                                    rule.metadata.disabled = isDisabled;
                                                    _userDefinedRulesDict[_selectedPackageName].Add(_newRuleNameText, rule);

                                                }

                                            }

                                            _selectedTreeNode.Name = rule.metadata.uuid;
                                            ultraExpandableGroupBox1.Enabled = true;

                                            if (_notificationCntrl.isNotificationExists(_selectedPackageName, _oldRuleName))
                                            {
                                                _notificationCntrl.UpdateNotificationSettings(_newRuleNameText, rule.metadata.uuid);
                                            }
                                            else
                                            {
                                                String ruleId = System.Guid.NewGuid().ToString();
                                                //String ruleId = "R" + DateTime.Now.ToString("yyyyddMMHHmmssfff");
                                                _notificationCntrl.CreateNotificationSettings(_selectedPackageName, _newRuleNameText, rule.metadata.uuid, ruleId);
                                            }
                                            MessageBox.Show(this, infoMessage, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                                            savedMessage.Add("ApplicationStatus", "Saved");
                                            AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
                                            AmqpHelper.SendObject("Build", "Build", null);
                                            //OpenUserDefinedRule();
                                        }


                                    }
                                    else
                                        MessageBox.Show(this, "Error while Renaming the rule. Please contact Nirvana support !!", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //  OpenUserDefinedRule();
                                    // LoadRulesFromGuvnor();
                                }
                            }
                        }
                        else if (response.Contains("UpdateNotification"))
                        {
                            String[] strArray = response.Split(':');
                            _notificationCntrl.UpdateNotificationDS(strArray[1]);
                        }
                        if (treeView1.SelectedNode != null && _selectedTreeNode != null)
                        {
                            //LoadRulesFromGuvnor();
                            //if (_selectedTreeNode.Tag.ToString() == Constants.RULE_TAG)
                            //{
                            //    //OpenUserDefinedRule();

                            //}
                            //else
                            if (_selectedTreeNode.Tag.ToString() != Constants.RULE_TAG)
                            {
                                Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                                RedirectPage(urlPath.AbsoluteUri);
                            }
                        }
                        treeView1.Focus();
                        }
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

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set { _secMasterServices = value; }
        }

        public IPostTradeServices PostTradeServices
        {
            set { ; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set { ; }
        }

        public IContainerServices ContainerServices
        {
            set { ; }
        }

        #endregion

        #region Events Listeners
        private void RuleEngineWin_Load(object sender, EventArgs e)
        {
            // Cursor.Current = Cursors.WaitCursor;
            Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
            RedirectPage(urlPath.AbsoluteUri);
            //ultraExpandableGroupBox1.Controls.Add(_notificationCntrl);
        }
        private void RuleEngineWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                AmqpHelper.Started -= new ListenerStarted(amqpHelperSummary_started);

                foreach (IAmqpReceiver amqpReceiver in receiverInstances)
                {
                    if (amqpReceiver.MediaName == _ruleBuildResponseExchange)
                    {
                        amqpReceiver.AmqpDataReceived -= new DataReceived(amqpHelperSummary_amqpDataRecieved);
                    }
                    else if (amqpReceiver.MediaName == _notificationExchange)
                    {
                        amqpReceiver.AmqpDataReceived -= new DataReceived(Amqp_Alert_received);
                    }
                }
                CustomRuleManager.GetInstance().AllRuleReceived -= new AllRuleReceivedHandler(RuleEngineWin_AllRuleReceived);
                CustomRuleManager.GetInstance().RuleOperationReceived -= new RuleOperationReceivedHandler(RuleEngineWin_RuleOperationReceived);
                CustomRuleManager.GetInstance().CloseAllConnection();
                CustomRuleManager.DisposeSingletonInstance();


                ImportExportManager.GetInstance().ImportComplete -= new ImportCompleteHandler(RuleEngineWin_ImportComplete);
                ImportExportManager.GetInstance().ExportComplete -= new ExportCompleteHandler(RuleEngineWin_ExportComplete);
                ImportExportManager.DestroySingletonInstance();
               // ImportExportManager.GetInstance().ImportComplete -= new ImportCompleteHandler(RuleEngineWin_ImportComplete);
                //NotificationHelper.alertEvent -= new AlertDelegate(notify_alertEvent);
                PluggableToolsClosed(this, null);
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
        /// Events for rule window start shown.
        /// Used to redirect to default page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleEngineWin_Shown(object sender, EventArgs e)
        {
            try
            {

                CustomRuleManager.GetInstance().AllRuleReceived += new AllRuleReceivedHandler(RuleEngineWin_AllRuleReceived);
                CustomRuleManager.GetInstance().RuleOperationReceived += new RuleOperationReceivedHandler(RuleEngineWin_RuleOperationReceived);

                CustomRuleManager.GetInstance().Initialise(_companyUser.CompanyUserID);
                ImportExportManager.GetInstance().ImportComplete += RuleEngineWin_ImportComplete;             
                ImportExportManager.GetInstance().ExportComplete+=RuleEngineWin_ExportComplete;

                GetCompliancePref();
                InitializeAlertHistView();
                //GetUserPermissions();
                Cursor.Current = Cursors.Default;
                ultraExpandableGroupBoxPanel1.Enabled = false;
                
                InitializeTreeViewStuct();

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

        private void RuleEngineWin_ExportComplete(string message, bool isSuccessful)
        {
            try
            {
            if (isSuccessful)
                MessageBox.Show(this,message, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this,message, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Event triggered from ImportExportManager when rule is imported.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="ruleName"></param>
        /// <param name="notification"></param>
        /// <param name="category"></param>
        /// <param name="ruleId"></param>
        private void RuleEngineWin_ImportComplete(string packageName, string ruleName, NotificationSettings notification, String category, String ruleId, int remainingImports)
        {
            try
            {
                if (!String.IsNullOrEmpty(ruleId))
                {
                    if (category.Equals(Constants.USER_DEFINED_RULES_TAG))
                    {
                        String uuid = RulesManager.GetUUIDOfAssets(ruleName, packageName);
                        ruleId = System.Guid.NewGuid().ToString();
                        RulesAsset rule = RulesManager.GetAssetMetaData(ruleName, packageName);
                        _notificationCntrl.SaveNotificationSettings(packageName, ruleName, notification, uuid, ruleId);
                        _userDefinedRulesDict[packageName][ruleName] = rule;

                        Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                        savedMessage.Add("ApplicationStatus", "Saved");
                        AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
                        //AmqpHelper.SendObject("UpdateNotification," + ruleId, "Build", null);
                        AmqpHelper.SendObject("Build", "Build", null);
                    }
                    else if (category.Equals(Constants.CUSTOM_RULE_NODE_TAG))
                    {

                        _notificationCntrl.SaveNotificationSettings(packageName, ruleName, notification, ruleId, ruleId);
                        CustomRuleManager.GetInstance().SendCustomRuleRequest("InitialisationRequest", null, "Import");


                    }
                    if (remainingImports == 0)
                    MessageBox.Show(this, "Rule Import Complete", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show(this, "Could not import rule " + ruleName + ".", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


       
        #endregion

        #region Tree view Events Listeners
        /// <summary>
        /// add 16B rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add16BRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                panelWebControl.Visible = false;
                //splitContainerRuleView.Panel1Collapsed = true;
                _sbCntrl.Visible = true;
                panel1.Controls.Add(_sbCntrl);
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

        private void categoryContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                TreeNode node = treeView1.SelectedNode;
                if (node != null)
                {

                    switch (node.Name)
                    {
                        case "PreTradeCompliance":
                            //addNewRuleToolStripMenuItem.Visible = true;
                            add16BRuleToolStripMenuItem.Visible = false;
                            add16BRuleToolStripMenuItem.Enabled = false;
                            //add16BRuleToolStripMenuItem.Enabled = false;
                            avoidWashSaleToolStripMenuItem.Visible = false;
                            if ((!_isWashSalePresent) && _isWashSaleEnabled)
                            {
                                avoidWashSaleToolStripMenuItem.Enabled = true;
                                // _isWashSalePresent = true;

                            }
                            else
                                avoidWashSaleToolStripMenuItem.Enabled = false;

                            break;
                        case "PostTradeCompliance":
                            // addNewRuleToolStripMenuItem.Visible = true;
                            add16BRuleToolStripMenuItem.Visible = false;
                            avoidWashSaleToolStripMenuItem.Visible = false;
                            break;
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

        private void avoidWashSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wash sale");
            _isWashSalePresent = true;
            //TODO: Save that wash sale is active.
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        void RuleEngineWin_AllRuleReceived()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        AllRuleReceivedHandler del = this.RuleEngineWin_AllRuleReceived;
                        this.Invoke(del);
                    }
                    else
                    {
                        LoadCustomRule();
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
        void RuleEngineWin_RuleOperationReceived(DataSet dsRecieved)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        RuleOperationReceivedHandler del = this.RuleEngineWin_RuleOperationReceived;
                        this.Invoke(del, new object[] { dsRecieved });
                    }
                    else
                    {
                        String operationType = dsRecieved.Tables[0].Rows[0]["OperationType"].ToString();
                        String infoMessage;
                        String ruleName = null;
                        switch (operationType)
                        {
                            case "Enable":
                                ruleName = _selectedRuleText;
                                break;
                            case "Disable":
                                ruleName = _selectedRuleText;
                                break;
                            case "Delete":

                                infoMessage = "Rule Deleted";
                                MessageBox.Show(this, infoMessage, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //LoadCustomRule();
                                break;
                            case "Rename":
                                String ruleId = dsRecieved.Tables[0].Rows[0]["RuleId"].ToString();
                                if (_notificationCntrl.isNotificationExists(_selectedPackageName, _selectedRuleText))
                                {
                                    _notificationCntrl.UpdateNotificationSettings(_newRuleNameText, ruleId);
                                }
                                else
                                {

                                    //String ruleId = "R" + DateTime.Now.ToString("yyyyddMMHHmmssfff");
                                    _notificationCntrl.CreateNotificationSettings(_selectedPackageName, _newRuleNameText, ruleId, ruleId);
                                }
                                ruleName = _newRuleNameText;
                                infoMessage = "Successfuly renamed the rule from \"" + _selectedRuleText + "\" to \"" + _newRuleNameText + "\"";
                                MessageBox.Show(this, infoMessage, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadCustomRule(_selectedPackageName, ruleName);
                                break;
                        }


                        if (_selectedTreeNode != null)
                            SetRuleNodeViewAndContextMenu(_selectedPackageName, _selectedRuleText);
                        //_selectedTreeNode.EndEdit(false);
                        //e.Node.EndEdit(false);
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

        private void LoadCustomRule(String packageName, String ruleName)
        {
            try
            {
                CustomRule rule = CustomRuleManager.GetInstance().GetRuleByRuleId(packageName, ruleName);
                AddCustomRuleNode(rule.RuleType, CustomRuleManager.GetInstance().GetAllRules(packageName));
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
        private void addRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _isCreateUserDefinedRuleRequest = true;
                int i = 0;
                while (i <= _userDefinedRulesNode.Nodes.Count)
                {
                    i++;
                    Boolean isFoundNode = false;

                    foreach (TreeNode node in _userDefinedRulesNode.Nodes)
                    {
                        if (node.Text.Equals("Rule" + i))
                        {
                            isFoundNode = true;
                            // break;
                        }
                    }

                    if (!isFoundNode)
                    {

                        TreeNode TempNode = new TreeNode("Rule" + i);
                        TempNode.Name = "";
                        //if (_selectedPackageName.Equals("PostTradeCompliance"))
                        //{
                        //    TempNode.ImageIndex = 2;
                        //    TempNode.SelectedImageIndex = 2;
                        //}
                        //else
                        //{
                        TempNode.ImageIndex = 5;
                        TempNode.SelectedImageIndex = 5;
                        // }
                        _userDefinedRulesNode.Nodes.Add(TempNode);
                        _userDefinedRulesNode.ExpandAll();
                        treeView1.SelectedNode = TempNode;
                        treeView1.LabelEdit = true;
                        TempNode.BeginEdit();
                        break;
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

        private void deleteRuleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                // _isDeleteUserDefinedRuleRequest = true;
                String ruleName = MessageFormatter.FormatRuleNameForGuvnor(treeView1.SelectedNode.Text);
                String packageName = treeView1.SelectedNode.Parent.Parent.Name;
                DialogResult dr = MessageBox.Show(this, "Are you sure you want to delete rule " + _selectedRuleText + " from " + packageName + "?", "Nirvana Compliance Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    TreeNode selectedNode = treeView1.SelectedNode;
                    bool isDeleted = false;
                    string nodeTag = selectedNode.Tag.ToString();
                    switch (nodeTag)
                    {
                        case Constants.RULE_TAG:
                            #region delete Rule handeling
                            lock (_lockerObject)
                            {

                                if (_notificationCntrl.isNotificationExists(_selectedPackageName, ruleName))
                                    _notificationCntrl.DeleteNotificationSettings(packageName, ruleName);

                                isDeleted = RulesManager.DeleteUserDefinedRule(ruleName, packageName);

                                if (isDeleted)
                                {
                                    if (_userDefinedRulesDict.ContainsKey(_selectedPackageName))
                                        if (_userDefinedRulesDict[_selectedPackageName].ContainsKey(ruleName))
                                            _userDefinedRulesDict[_selectedPackageName].Remove(ruleName);

                                    AmqpHelper.SendObject("Build", "Build", null);
                                }
                                ultraExpandableGroupBox1.Enabled = false;


                            }
                            #endregion
                            break;
                        case Constants.SIXTEEN_B_SYMBOL:
                            #region delete SixteenB symbols rule handeling
                            String symbol = selectedNode.Text;
                            isDeleted = SixteenBDataHelper.DeleteSixteenBRule(symbol, "", _companyUser.CompanyUserID);
                            #endregion
                            break;
                        case Constants.SIXTEEN_B_DATE:
                            #region delete SixteenB date rule handeling
                            String date = selectedNode.Text;
                            String symbol_Date = selectedNode.Parent.Text;
                            isDeleted = SixteenBDataHelper.DeleteSixteenBRule(symbol_Date, date, _companyUser.CompanyUserID);
                            #endregion
                            break;
                        case Constants.CUSTOM_RULE_TAG:
                        #region delete Custom Rule

                            if (_notificationCntrl.isNotificationExists(_selectedPackageName, _selectedRuleText))
                                _notificationCntrl.DeleteNotificationSettings(packageName, ruleName);

                            CustomRule rule = CustomRuleManager.GetInstance().GetRuleByRuleId(_selectedPackageName, _selectedRuleText);
                            String response = CustomRuleManager.GetInstance().SendCustomRuleRequest("Delete", rule, String.Empty);
                            if (String.IsNullOrEmpty(response))
                            {
                                treeView1.SelectedNode.Remove();
                                //_selectedTreeNode.Name = "";
                                //_selectedTreeNode.Tag = "";
                                //_selectedTreeNode.Text = "";
                                Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                                RedirectPage(urlPath.AbsoluteUri);
                                ultraExpandableGroupBoxPanel1.Enabled = false;
                            }
                            else
                                MessageBox.Show("Delete operation failed.\n" + response);
                            break;   
                                
                               
                          
                        #endregion
                    }
                    if (isDeleted)
                    {
                        if (nodeTag.Equals(Constants.SIXTEEN_B_DATE))
                        {
                            if (selectedNode.Parent.Nodes.Count == 1)
                                treeView1.SelectedNode = selectedNode.Parent;
                        }
                        treeView1.SelectedNode.Remove();
                        LoadRulesFromGuvnor();
                        //Selection is going to user defined node but in _selectedTreeNode contains deleted rule value.
                        _selectedTreeNode.Name = "";
                        _selectedTreeNode.Tag = "";
                        _selectedTreeNode.Text = "";
                        Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                        RedirectPage(urlPath.AbsoluteUri);
                        ultraExpandableGroupBoxPanel1.Enabled = false;
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

        private void openRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String nodeTag = "";
                if (treeView1.SelectedNode.Tag != null)
                    nodeTag = treeView1.SelectedNode.Tag.ToString();
                switch (nodeTag)
                {
                    case Constants.RULE_TAG:
                        #region Open drools rule click handeling

                        OpenUserDefinedRule();

                        #endregion
                        break;

                    case Constants.SIXTEEN_B_DATE:
                        #region open SixteenB rule click handeling
                        panelWebControl.Visible = false;
                        //splitContainerRuleView.Panel1Collapsed = true;
                        _sbCntrl.Visible = true;
                        panel1.Controls.Add(_sbCntrl);
                        String symbol = treeView1.SelectedNode.Parent.Text;
                        DateTime date = DateTime.Parse(treeView1.SelectedNode.Text);
                        _sbCntrl.SetSixteenBRule(symbol, date);
                        #endregion
                        break;
                    case Constants.CUSTOM_RULE_TAG:
                        OpenCustomRule();
                        break;

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


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode.Tag.ToString().Equals(Constants.RULE_TAG))
                {
                    OpenUserDefinedRule();
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

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            try
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
                _selectedTreeNode = treeView1.SelectedNode;
                _isSaveUserDefineRuleRequest = false;
                _isBuildPackageRequest = false;
                //_isCreateUserDefinedRuleRequest = false;

                string nodeTag = "";
                if (treeView1.SelectedNode.Tag != null)
                    nodeTag = treeView1.SelectedNode.Tag.ToString();
                ultraExpandableGroupBox1.Enabled = true;
                ultraExpandableGroupBox1.Expanded = true;
                btnSaveRule.Enabled = false;
                // ApplyRuleBtn.Enabled = false;
                Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                switch (nodeTag)
                {
                    case "RootNode":
                        //Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                        RedirectPage(urlPath.AbsoluteUri);
                        ultraExpandableGroupBoxPanel1.Enabled = false;
                        if (lastSelectedNode != null)
                        {
                            lastSelectedNode.BackColor = Color.Empty;
                        }
                        break;

                    case Constants.CATEGORY_TAG:
                        #region category node click handeling
                        SetPackagePermissions(treeView1.SelectedNode.Name);
                        _selectedPackageName = treeView1.SelectedNode.Name;
                        foreach (TreeNode node in _selectedTreeNode.Nodes)
                        {
                            if (node.Tag.ToString() == Constants.USER_DEFINED_RULES_TAG)
                            {
                                _userDefinedRulesNode = node;
                                
                            }
                            else if (node.Tag.ToString() == Constants.CUSTOM_RULE_NODE_TAG)
                            {
                                _customRuleNode = node;
                               
                            }

                        }
                        
                        if (e.Button == MouseButtons.Right)
                        {

                            categoryContextMenuStrip.Show(treeView1, e.Location);
                            importRuleToolStripMenuItem.Visible = false;
                            addCategoriesToolStripMenuItem.Visible = true;

                        }
                        else if (e.Button == MouseButtons.Left)
                        {
                            //Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                            RedirectPage(urlPath.AbsoluteUri);
                            ultraExpandableGroupBoxPanel1.Enabled = false;

                        }
                        if (lastSelectedNode != null)
                        {
                            lastSelectedNode.BackColor = Color.Empty;
                        }


                        #endregion
                        break;
                    case Constants.USER_DEFINED_RULES_TAG:
                        #region category node click handeling
                        SetPackagePermissions(treeView1.SelectedNode.Parent.Name);
                        _selectedPackageName = treeView1.SelectedNode.Parent.Name;
                        _userDefinedRulesNode = _selectedTreeNode;
                        //foreach (TreeNode node in _selectedTreeNode.Nodes)
                        //{
                        //    if (node.Tag.ToString() == Constants.USER_DEFINED_RULES_TAG)
                        //    {
                        //        _userDefinedRulesNode = node;

                        //    }
                        //}
                        
                        if (e.Button == MouseButtons.Right)
                        {

                            categoryContextMenuStrip.Show(treeView1, e.Location);
                            addCategoriesToolStripMenuItem.Visible = true;
                            add16BRuleToolStripMenuItem.Visible = false;
                            avoidWashSaleToolStripMenuItem.Visible = false;

                        }
                        else if (e.Button == MouseButtons.Left)
                        {
                            //Uri urlPath = new Uri(Application.StartupPath + "/Startup.htm");
                            RedirectPage(urlPath.AbsoluteUri);
                            ultraExpandableGroupBoxPanel1.Enabled = false;

                        }
                        if (lastSelectedNode != null)
                        {
                            lastSelectedNode.BackColor = Color.Empty;
                        }


                        #endregion
                        break;
                    case Constants.RULE_TAG:
                        lock (_lockerObject)
                        {
                            #region rule node click handeling
                            btnSaveRule.Enabled = true;
                            //ultraExpandableGroupBox1.Enabled = true;

                            SetPackagePermissions(treeView1.SelectedNode.Parent.Parent.Name);
                            _selectedPackageName = treeView1.SelectedNode.Parent.Parent.Name;
                            _selectedRuleText = treeView1.SelectedNode.Text;

                            //OpenUserDefinedRule();
                            SetRuleNodeViewAndContextMenu(_selectedPackageName, MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText));
                            //if (lastSelectedNode != null && lastSelectedNode.Tag.ToString().Equals(Constants.RULE_TAG))
                            //{
                            //    SetRuleNodeViewAndContextMenu(_selectedPackageName,lastSelectedNode.Text);
                            //}

                            if (e.Button == MouseButtons.Right)
                            {

                                ruleOnlycontextMenuStrip.Show(treeView1, e.Location);
                                deleteRuleToolStripMenuItem1.Visible = true;
                            }
                            else if (e.Button == MouseButtons.Left)
                            {
                                OpenUserDefinedRule();
                            }
                            //if (lastSelectedNode != null)
                            //{
                            //    lastSelectedNode.BackColor = Color.Empty;
                            //    if (lastSelectedNode.ToolTipText.Equals("Enabled"))
                            //        lastSelectedNode.ForeColor = Color.White;
                            //    else
                            //        lastSelectedNode.ForeColor = Color.Black;

                            //}
                            lastSelectedNode = _selectedTreeNode;
                            _lastSelectedPackageName = _selectedPackageName;
                            #endregion
                            break;
                        }
                    case Constants.SIXTEEN_B_SYMBOL:
                        #region SixteenB symbol click handeling
                        SetPackagePermissions(treeView1.SelectedNode.Parent.Parent.Text);

                        if (e.Button == MouseButtons.Right)
                        {
                            ruleOnlycontextMenuStrip.Show(treeView1, e.Location);
                            openRuleToolStripMenuItem.Visible = false;
                        }
                        else if (e.Button == MouseButtons.Left)
                        {
                            RedirectPage(Application.StartupPath + "\\Startup.htm");
                            ultraExpandableGroupBoxPanel1.Enabled = false;
                        }

                        if (lastSelectedNode != null)
                        {
                            lastSelectedNode.BackColor = Color.Empty;
                        }

                        #endregion
                        break;
                    case Constants.SIXTEEN_B_DATE:
                        #region SixteenB date node click handeling
                        SetPackagePermissions(treeView1.SelectedNode.Parent.Parent.Parent.Text);
                        if (e.Button == MouseButtons.Right)
                            ruleOnlycontextMenuStrip.Show(treeView1, e.Location);

                        else if (e.Button == MouseButtons.Left)
                            panelWebControl.Visible = false;
                        //splitContainerRuleView.Panel1Collapsed = true;
                        _sbCntrl.Visible = true;
                        panel1.Controls.Add(_sbCntrl);
                        String symbol = treeView1.SelectedNode.Parent.Text;
                        DateTime date = DateTime.Parse(treeView1.SelectedNode.Text);
                        _sbCntrl.SetSixteenBRule(symbol, date);

                        if (lastSelectedNode != null)
                        {
                            lastSelectedNode.BackColor = Color.Empty;
                        }

                        #endregion
                        break;
                    case Constants.CUSTOM_RULE_TAG:
                        #region Custom rule handling

                        SetPackagePermissions(treeView1.SelectedNode.Parent.Parent.Name);
                        _selectedPackageName = treeView1.SelectedNode.Parent.Parent.Name;
                        _selectedRuleText = treeView1.SelectedNode.Text;
                        SetRuleNodeViewAndContextMenu(_selectedPackageName, _selectedRuleText);
                        btnSaveRule.Enabled = true;
                        if (e.Button == MouseButtons.Right)
                        {
                            
                            ruleOnlycontextMenuStrip.Show(treeView1, e.Location);
                            deleteRuleToolStripMenuItem1.Visible = false;
                        }
                        else if (e.Button == MouseButtons.Left)
                        {
                            OpenCustomRule();
                        }
                        lastSelectedNode = _selectedTreeNode;
                        _lastSelectedPackageName = _selectedPackageName;
                        #endregion
                        break;
                    case Constants.CUSTOM_RULE_NODE_TAG:
                        SetPackagePermissions(treeView1.SelectedNode.Parent.Name);
                        _selectedPackageName = treeView1.SelectedNode.Parent.Name;
                        _customRuleNode = _selectedTreeNode;
                        ultraExpandableGroupBoxPanel1.Enabled = false;
                        if (e.Button == MouseButtons.Right)
                        {

                            categoryContextMenuStrip.Show(treeView1, e.Location);
                            addCategoriesToolStripMenuItem.Visible = false;
                            add16BRuleToolStripMenuItem.Visible = false;
                            avoidWashSaleToolStripMenuItem.Visible = false;

                        }
                        else if (e.Button == MouseButtons.Left)
                        {
                            RedirectPage(urlPath.AbsoluteUri);
                        }
                        break;

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

        private void OpenCustomRule()
        {
            try
            {
                ultraExpandableGroupBoxPanel1.Enabled = true;
                if (_selectedPackageName != "PreTradeCompliance")
                {
                    _notificationCntrl.ToggleFrequencyVisibility(true);
                }
                else
                {
                    _notificationCntrl.ToggleFrequencyVisibility(false);
                }
                _notificationCntrl.SetNotificationSettings(_selectedRuleText, _selectedPackageName);
                RedirectPage(new Uri(CustomRuleManager.GetInstance().GetRuleByRuleId(_selectedPackageName, _selectedRuleText).HtmlPath).AbsoluteUri);
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



        private void renameRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                lock (_lockerObject)
                {

                    treeView1.LabelEdit = true;
                    treeView1.SelectedNode.BeginEdit();
                    _isRenameUserDefinedRuleRequest = true;
                    _selectedRuleText = treeView1.SelectedNode.Text;

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

        void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (_selectedTreeNode.Parent.Tag.Equals(Constants.CUSTOM_RULE_NODE_TAG))                
                {

                    CustomRule rule = CustomRuleManager.GetInstance().GetRuleByRuleId(_selectedPackageName, e.Node.Text);
                    if (e.Label==null || e.Label.Equals(e.Node.Text))
                    {
                        treeView1.LabelEdit = false;
                        return;
                    }
                    if (_userDefinedRulesDict[_selectedPackageName].ContainsKey(e.Label))
                    {
                        MessageBox.Show(this, "Rule Name already exist", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        treeView1.LabelEdit = true;
                        e.CancelEdit = true;
                        treeView1.SelectedNode.BeginEdit();
                        //treeView1.SelectedNode.Remove();
                    }
                    else
                    {

                        String response = CustomRuleManager.GetInstance().SendCustomRuleRequest("Rename", rule, e.Label);
                        if (String.IsNullOrEmpty(response))
                        {

                            _newRuleNameText = e.Label;
                            treeView1.LabelEdit = false;
                        }
                        else
                        {
                            e.Node.Text = _selectedRuleText;
                            treeView1.LabelEdit = true;
                            e.CancelEdit = true;
                            
                            //treeView1.SelectedNode.BeginEdit();
                            MessageBox.Show(this, "Rename operation failed.\n" + response, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        //String infoMessage = "";
                        //infoMessage = "Successfuly renamed the rule from \"" + e.Node.Text + "\" to \"" + e.Label + "\"";

                        //_selectedTreeNode.EndEdit(false);
                        //e.Node.EndEdit(false);

                        //MessageBox.Show(this, infoMessage, "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                }
                else
                {
                    lock (_lockerObject)
                    {
                        treeView1.LabelEdit = false;
                        errProviderRuleEngine.Clear();
                        _oldRuleName = MessageFormatter.FormatRuleNameForGuvnor(e.Node.Text);
                        String ruleName = e.Node.Text;
                        if (e.Label != null)
                            ruleName = e.Label;
                        if (MessageFormatter.FormatRuleNameForDisplay(_oldRuleName).Equals(ruleName) && _isRenameUserDefinedRuleRequest)
                        {
                            _isRenameUserDefinedRuleRequest = false;
                            return;
                        }

                        if (ruleName != "" && ruleName != null)
                        {
                            //Regex regex = new Regex(@"^[a-zA-Z0-9][[[$$]|[\s]|[a-zA-Z0-9@%&_.-]]]*[a-zA-Z0-9]$");
                            Regex regex = new Regex(@"^[a-zA-Z0-9][-@\$%&_.a-zA-Z0-9 ]*[a-zA-Z0-9]$");
                            // errProviderRuleEngine.Clear();
                            if (!regex.IsMatch(ruleName))
                            {
                                //assetNameTxtBox.Text = "";
                                MessageBox.Show("Rule name can only have alphanumeric characters, space and @ _ . - & $ % special characters and can not start/end with special characters.", "Nirvana Compliance ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //  errProviderRuleEngine.SetError(treeView1, "Name can only have alphanumeric characters as well as _ . - special characters.");
                                treeView1.LabelEdit = true;
                                e.CancelEdit = true;
                                treeView1.SelectedNode.BeginEdit();
                            }
                            else
                            {
                                _newRuleNameText = MessageFormatter.FormatRuleNameForGuvnor(ruleName);

                                if (_userDefinedRulesDict[_selectedPackageName].ContainsKey(_newRuleNameText) || CustomRuleManager.GetInstance().IsCustomRuleNamePresent(_selectedPackageName, _newRuleNameText))
                                {
                                    MessageBox.Show(this, "Rule Name already exist", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    treeView1.LabelEdit = true;
                                    e.CancelEdit = true;
                                    treeView1.SelectedNode.BeginEdit();
                                    //treeView1.SelectedNode.Remove();
                                }
                                else
                                {
                                    treeView1.SelectedNode.EndEdit(false);
                                    if (_isCreateUserDefinedRuleRequest)
                                    {
                                        _isCreateUserDefinedRuleRequest = false;
                                        //comboCompressionLevel.Enabled = false;
                                        //DialogResult dr = MessageBox.Show("Are you sure to create rule \"" + ruleName + "\" with \"" + _selectedCompressionLevel + "\" compliance level?", "Nirvana Compliance ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        DialogResult dr = MessageBox.Show("Are you sure you want to create rule " + ruleName + ".", "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (dr == DialogResult.Yes)
                                        {
                                            CreateUserDefinedRule(_newRuleNameText);
                                        }
                                        else
                                        {
                                            treeView1.SelectedNode.Remove();
                                        }
                                        // _newRuleNameText = e.Node.Name;

                                    }
                                    else if (_isRenameUserDefinedRuleRequest)
                                    {
                                        if (!_oldRuleName.Equals(_newRuleNameText))
                                        {
                                            //_selectedTreeNode.Name = "";

                                            //String newRuleFileName = _selectedCompressionLevel + "_" + ruleName + ".brl";
                                            
                                            //String newRuleFileName = ruleName + ".brl";
                                            //String oldRuleFileName = _oldRuleName + ".brl";
                                            //String uuid = _selectedTreeNode.Name;
                                            RulesManager.RenameRule(_newRuleNameText, _oldRuleName, _selectedPackageName);
                                            
                                            //_newRuleNameText = ruleName;

                                        }
                                        //_amqpHelperSender.SendObject("Rename," + _selectedPackageName + ":" + oldRuleFileName + ":" + newRuleFileName);
                                    }
                                    //_newRuleNameValue = _selectedCompressionLevel + "_" + ruleName;
                                }
                            }
                        }
                        else
                        {

                            e.CancelEdit = true;
                            treeView1.SelectedNode.EndEdit(true);
                            if (_isCreateUserDefinedRuleRequest)
                            {
                                _isCreateUserDefinedRuleRequest = false;
                                treeView1.SelectedNode.Remove();

                            }
                            else if (_isRenameUserDefinedRuleRequest)
                            {
                                // treeView1.SelectedNode.text= 
                                treeView1.SelectedNode.Text = MessageFormatter.FormatRuleNameForDisplay(_oldRuleName);
                            }

                            //_isUpdateCompressionRequest = false;
                            _isRenameUserDefinedRuleRequest = false;
                        }
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


        private void btnSaveRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedTreeNode.Parent.Tag.Equals(Constants.CUSTOM_RULE_NODE_TAG))
                {
                    if (_notificationCntrl.isNotificationExists(_selectedPackageName, _selectedRuleText))
                    {
                        _notificationCntrl.UpdateNotificationSettings();
                    }
                    else
                    {
                        CustomRule rule=CustomRuleManager.GetInstance().GetRuleByRuleId(_selectedPackageName,_selectedRuleText);
                        String ruleId = rule.RuleId;

                        _notificationCntrl.CreateNotificationSettings(_selectedPackageName, _selectedRuleText, ruleId, ruleId);
                    }
                }
                else
                {
                    lock (_lockerObject)
                    {
                        _isSaveUserDefineRuleRequest = true;
                        _webBrowserCntrl.InvokeSaveBtnOfGuvnor();

                        string uuid = _selectedTreeNode.Name;
                        if (_notificationCntrl.isNotificationExists(_selectedPackageName, MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText)))
                        {
                            _notificationCntrl.UpdateNotificationSettings();
                        }
                        else
                        {
                            String ruleId = System.Guid.NewGuid().ToString();
                            //String ruleId = "R" + DateTime.Now.ToString("yyyyddMMHHmmssfff");
                            _notificationCntrl.CreateNotificationSettings(_selectedPackageName, MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText), uuid, ruleId);
                        }
                       
                        //btnSaveRule.Enabled = false;
                        //RuleSaveSender
                        //ApplyRuleBtn.Enabled = true; 
                    }
                }
                Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                savedMessage.Add("ApplicationStatus", "Saved");
                AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
                _webBrowserCntrl.FocusOnBrowser();
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

        private void disableRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                lock (_lockerObject)
                {
                    Boolean isDisabled = true;
                    EnableDisableRule(isDisabled);

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

        private void enableRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                lock (_lockerObject)
                {
                    Boolean isDisabled = false;
                    EnableDisableRule(isDisabled);
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


        private void enableAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (_selectedTreeNode.Name.Equals(_selectedPackageName))
                {
                    EnableAllRules(Constants.CUSTOM_RULE_NODE_TAG);
                    EnableAllRules(Constants.USER_DEFINED_RULES_TAG);
                }
                else if (_selectedTreeNode.Tag.ToString().Equals(Constants.USER_DEFINED_RULES_TAG))
                {
                     EnableAllRules(Constants.USER_DEFINED_RULES_TAG);
                    
                }
                else if (_selectedTreeNode.Tag.ToString().Equals(Constants.CUSTOM_RULE_NODE_TAG))
                {
                    EnableAllRules(Constants.CUSTOM_RULE_NODE_TAG);
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

        private void EnableAllRules(string nodeTag)
        {
            try
            {
                Boolean isDisabled = false;
                if (nodeTag.Equals(Constants.CUSTOM_RULE_NODE_TAG))
                {
                    CustomRuleManager.GetInstance().ChangeStateOfAllRules(_selectedPackageName, isDisabled);
                    foreach (TreeNode node in _customRuleNode.Nodes)
                    {
                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                        node.ToolTipText = "Enabled";
                    }
                }
                else if (nodeTag.Equals(Constants.USER_DEFINED_RULES_TAG))
                {

                    if (_userDefinedRulesDict.ContainsKey(_selectedPackageName))
                    {
                        Dictionary<String, RulesAsset> packageRuels = _userDefinedRulesDict[_selectedPackageName];
                        foreach (KeyValuePair<String, RulesAsset> rule in packageRuels)
                        {

                            RulesManager.EnableDisableUserDefnedRule(_selectedPackageName, rule.Key, isDisabled);
                            rule.Value.metadata.disabled = isDisabled;

                        }
                        foreach (TreeNode node in _userDefinedRulesNode.Nodes)
                        {
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            node.ToolTipText = "Enabled";
                        }
                        _isBuildPackageRequest = true;
                        AmqpHelper.SendObject("Build", "Build", null);
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

        private void disableAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedTreeNode.Name.Equals(_selectedPackageName))
                {
                    DisableAllRules(Constants.CUSTOM_RULE_NODE_TAG);
                    DisableAllRules(Constants.USER_DEFINED_RULES_TAG);
                }
                else if (_selectedTreeNode.Tag.ToString().Equals(Constants.USER_DEFINED_RULES_TAG))
                {
                    DisableAllRules(Constants.USER_DEFINED_RULES_TAG);

                }
                else if (_selectedTreeNode.Tag.ToString().Equals(Constants.CUSTOM_RULE_NODE_TAG))
                {
                    DisableAllRules(Constants.CUSTOM_RULE_NODE_TAG);
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

        private void DisableAllRules(string nodeTag)
        {
            try
            {
                Boolean isDisabled = true;
                if (nodeTag.Equals(Constants.CUSTOM_RULE_NODE_TAG))
                {
                    CustomRuleManager.GetInstance().ChangeStateOfAllRules(_selectedPackageName, isDisabled);

                    foreach (TreeNode node in _customRuleNode.Nodes)
                    {
                        node.ImageIndex = 5;
                        node.SelectedImageIndex = 5;
                        node.ToolTipText = "Disabled";
                    }
                }
                else if (nodeTag.Equals(Constants.USER_DEFINED_RULES_TAG))
                {
                    Dictionary<String, RulesAsset> packageRuels = _userDefinedRulesDict[_selectedPackageName];
                    foreach (KeyValuePair<String, RulesAsset> rule in packageRuels)
                    {
                        RulesManager.EnableDisableUserDefnedRule(_selectedPackageName, rule.Key, isDisabled);
                        rule.Value.metadata.disabled = isDisabled;
                    }
                    foreach (TreeNode node in _userDefinedRulesNode.Nodes)
                    {
                        node.ImageIndex = 5;
                        node.SelectedImageIndex = 5;
                        node.ToolTipText = "Disabled";
                    }
                    _isBuildPackageRequest = true;
                    AmqpHelper.SendObject("Build", "Build", null);
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

        private void EnableDisableRule(bool isDisabled)
        {


            try
            {
                if (_selectedTreeNode.Parent.Tag.Equals(Constants.CUSTOM_RULE_NODE_TAG))
                {
                    CustomRule rule = CustomRuleManager.GetInstance().GetRuleByRuleId(_selectedPackageName, _selectedRuleText);

                    if (isDisabled)
                    {
                        String response = CustomRuleManager.GetInstance().SendCustomRuleRequest("Disable", rule, String.Empty);
                        if (!String.IsNullOrEmpty(response))
                        {
                            MessageBox.Show("Disable operation failed.\n" + response);
                        }
                    }
                    else
                    {
                        String response = CustomRuleManager.GetInstance().SendCustomRuleRequest("Enable", rule, String.Empty);
                        if (!String.IsNullOrEmpty(response))
                        {
                            MessageBox.Show("Disable operation failed.\n" + response);
                        }
                    }
                    if (isDisabled)
                        _selectedTreeNode.ToolTipText = "Disabled";
                    else
                        _selectedTreeNode.ToolTipText = "Enabled";
                    SetRuleNodeViewAndContextMenu(_selectedPackageName, _selectedRuleText);
                }
                else
                {
                    String response = RulesManager.EnableDisableUserDefnedRule(_selectedPackageName,MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText), isDisabled);
                    if (isDisabled == Boolean.Parse(response))
                    {
                        _isBuildPackageRequest = true;
                        AmqpHelper.SendObject("Build", "Build", null);
                        if (isDisabled)
                            _selectedTreeNode.ToolTipText = "Disabled";
                        else
                            _selectedTreeNode.ToolTipText = "Enabled";
                        if (_userDefinedRulesDict.ContainsKey(_selectedPackageName))
                        {
                            if (_userDefinedRulesDict[_selectedPackageName].ContainsKey(MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText)))
                            {
                                RulesAsset rule = _userDefinedRulesDict[_selectedPackageName][MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText)];
                                rule.metadata.disabled = isDisabled;
                            }
                        }
                        SetRuleNodeViewAndContextMenu(_selectedPackageName, MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText));
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

         #endregion

        private void btGetHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdCurrent.Checked)
                {
                    _ruleHistDS = RulesDAO.GetComplianceAlertHist(DateTime.Today, DateTime.Today);
                    ultraAlertGrid.DataSource = _ruleHistDS;
                    ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].Format = "MM/dd/yyyy HH:mm:ss";
                    ultraAlertGrid.DisplayLayout.Bands[0].Columns["Description"].MinWidth = 300;
                    ultraAlertGrid.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                    ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].SortIndicator = SortIndicator.Descending;
                }
                else
                {
                    _ruleHistDS = RulesDAO.GetComplianceAlertHist(dTPFrom.Value, dTPTill.Value);
                    ultraAlertGrid.DataSource = _ruleHistDS;
                    ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].Format = "MM/dd/yyyy HH:mm:ss";
                    ultraAlertGrid.DisplayLayout.Bands[0].Columns["Description"].MinWidth = 300;
                    ultraAlertGrid.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                    ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].SortIndicator = SortIndicator.Descending;
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
        private void rdCurrent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _ruleHistDS.Tables[0].Clear();
                if (rdCurrent.Checked)
                    rdHistorical.Checked = false;
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

        private void rdHistorical_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _ruleHistDS.Tables[0].Clear();
                if (rdHistorical.Checked)
                {
                    rdCurrent.Checked = false;
                    dTPFrom.Enabled = true;
                    dTPTill.Enabled = true;
                }
                else
                {
                    dTPFrom.Enabled = false;
                    dTPTill.Enabled = false;
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

        private void exportAllRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, ExportDefinition> exportDefCache = new Dictionary<string, ExportDefinition>();
            if (_selectedTreeNode.Tag.Equals(Constants.CATEGORY_TAG))
            {
                foreach (TreeNode nodes in _selectedTreeNode.Nodes)
                {
                    foreach (TreeNode node in nodes.Nodes)
                    {
                            String key="";
                            if (node.Parent.Tag.ToString() == Constants.CUSTOM_RULE_NODE_TAG)
                                key = _selectedPackageName + "_" + node.Text;
                            else
                                key = _selectedPackageName + "_" + MessageFormatter.FormatRuleNameForGuvnor(node.Text);

                            NotificationSettings notification = _notificationCntrl.GetNotificationSetting(_selectedPackageName, MessageFormatter.FormatRuleNameForGuvnor(node.Text));
                            if (exportDefCache.ContainsKey(key))
                            {
                                exportDefCache[key].PackageName = _selectedPackageName;
                                exportDefCache[key].Notification = notification;
                                exportDefCache[key].DirectoryPath = _compliancePref.ImportExportPath;
                                exportDefCache[key].RuleCategory = node.Parent.Tag.ToString();
                                if (exportDefCache[key].RuleCategory == Constants.CUSTOM_RULE_NODE_TAG)
                                    exportDefCache[key].RuleName = node.Text;
                                else
                                    exportDefCache[key].RuleName = MessageFormatter.FormatRuleNameForGuvnor(node.Text);
                                exportDefCache[key].RuleId = node.Name;
                            }
                            else 
                            {
                                ExportDefinition exportDef=new ExportDefinition();
                                exportDef.PackageName = _selectedPackageName;
                                exportDef.Notification = notification;
                                exportDef.DirectoryPath = _compliancePref.ImportExportPath;
                                exportDef.RuleCategory = node.Parent.Tag.ToString();
                                if (exportDef.RuleCategory == Constants.CUSTOM_RULE_NODE_TAG)
                                    exportDef.RuleName = node.Text;
                                else
                                    exportDef.RuleName = MessageFormatter.FormatRuleNameForGuvnor(node.Text);
                                exportDef.RuleId = node.Name;
                                exportDefCache.Add(key, exportDef);
                            }
                           // ImportExportManager.GetInstance().Export(_selectedPackageName, node.Parent.Tag.ToString(), MessageFormatter.FormatRuleNameForGuvnor(node.Text), _compliancePref.ImportExportPath, node.Name, notification);
                    }
                }
            }
            else
            {
                foreach (TreeNode node in _selectedTreeNode.Nodes)
                {
                        String key = "";
                        if (node.Parent.Tag.ToString() == Constants.CUSTOM_RULE_NODE_TAG)
                            key = _selectedPackageName + "_" + node.Text;
                        else
                            key = _selectedPackageName + "_" + MessageFormatter.FormatRuleNameForGuvnor(node.Text);

                        NotificationSettings notification = _notificationCntrl.GetNotificationSetting(_selectedPackageName, MessageFormatter.FormatRuleNameForGuvnor(node.Text));
                        if (exportDefCache.ContainsKey(key))
                        {
                            exportDefCache[key].PackageName = _selectedPackageName;
                           exportDefCache[key].Notification = notification;
                            exportDefCache[key].DirectoryPath = _compliancePref.ImportExportPath;
                            exportDefCache[key].RuleCategory = node.Parent.Tag.ToString();
                            if (exportDefCache[key].RuleCategory == Constants.CUSTOM_RULE_NODE_TAG)
                                exportDefCache[key].RuleName = node.Text;
                            else
                                exportDefCache[key].RuleName = MessageFormatter.FormatRuleNameForGuvnor(node.Text);
                            exportDefCache[key].RuleId = node.Name;
                        }
                        else
                        {
                            ExportDefinition exportDef = new ExportDefinition();
                            exportDef.PackageName = _selectedPackageName;
                            exportDef.Notification = notification;
                            exportDef.DirectoryPath = _compliancePref.ImportExportPath;
                            exportDef.RuleCategory = node.Parent.Tag.ToString();
                            if (exportDef.RuleCategory == Constants.CUSTOM_RULE_NODE_TAG)
                                exportDef.RuleName = node.Text;
                            else
                                exportDef.RuleName = MessageFormatter.FormatRuleNameForGuvnor(node.Text);
                            exportDef.RuleId = node.Name;
                            exportDefCache.Add(key, exportDef);
                        }
                       // ImportExportManager.GetInstance().Export(_selectedPackageName, node.Parent.Tag.ToString(), MessageFormatter.FormatRuleNameForGuvnor(node.Text), _compliancePref.ImportExportPath, node.Name, notification);
                    }
                }
                ImportExportManager.GetInstance().Export(exportDefCache);
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

        private void importRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, ImportDefinition> importDefCache = new Dictionary<string, ImportDefinition>();
                List<String> path = GetRuleDirectoryFromUser();

                List<String> inValidPath = new List<string>();
                List<String> validPath = new List<string>();
                foreach (String rulePath in path)
                {
                    if (String.IsNullOrEmpty(rulePath) || !ValidatePath(rulePath))
                        inValidPath.Add(rulePath);
                    else
                        validPath.Add(rulePath);
                }

                List<string> newInvalidPath = new List<string>();
                foreach (String valPath in validPath)
                {
                    ImportDefinition def = GetImportDefinitionFromPath(valPath);
                    if (def != null && ImportExportManager.GetInstance().CheckValidImport(def, _selectedPackageName, _selectedTreeNode.Tag.ToString()))
                    {
                        def.PackageName = _selectedPackageName;
                        if (importDefCache.ContainsKey(def.PackageName + "_" + def.NewRuleName))
                            def.NewRuleName = GetnewRuleNameIfRuleExists(def.NewRuleName);

                        importDefCache.Add(def.PackageName + "_" + def.NewRuleName, def);
                    }
                    else
                        {
                        newInvalidPath.Add(valPath);
                        inValidPath.Add(valPath);
                    }
                    //else
                }
                foreach (String newInpath in newInvalidPath)
                    validPath.Remove(newInpath);


                if (validPath.Count == 0)
                            {
                    MessageBox.Show("No valid rules found", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //Asking if user wants to continue with partial selection
                if (inValidPath.Count > 0)
                                {
                    StringBuilder message = new StringBuilder();
                    message.Append(inValidPath.Count);
                    message.AppendLine(" rule path are invalid.");
                    message.AppendLine("Invalid rules are:");

                    int i = 0;
                    foreach (String invapath in inValidPath)
                                    {
                        if (i > 10)
                                        {
                            message.AppendLine("And some more");
                                            break;
                                        }
                        else
                        {
                            message.AppendLine(invapath);
                            i++;
                                    }
                                }
                    message.AppendLine("\n\nDo you want to continue with valid rules?");
                    DialogResult dr = MessageBox.Show(message.ToString(), "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.No)
                        return;

                            }


                ImportRuleSelection selection = new ImportRuleSelection(ref importDefCache);
                DialogResult drSelection=  selection.ShowDialog(this);
                if (drSelection == DialogResult.OK)
                {
                    if (importDefCache.Count > 0)
                    ImportExportManager.GetInstance().Import(importDefCache);
                            else
                        MessageBox.Show("No rule to import.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private ImportDefinition GetImportDefinitionFromPath(string rulePath)
        {
            try
            {
                ImportDefinition definition = new ImportDefinition();
                DataTable dtTemp = ImportExportManager.GetInstance().GetImportExportRuleMetadata(rulePath);
                definition.PackageName = dtTemp.Rows[0]["PackageName"].ToString();
                definition.DirectoryPath = rulePath;
                definition.RuleName = dtTemp.Rows[0]["RuleName"].ToString();
                definition.NewRuleName = GetnewRuleNameIfRuleExists(definition.RuleName);
                definition.RuleCategory = dtTemp.Rows[0]["RuleCategory"].ToString();
                dtTemp.Rows[0]["NewRuleName"] = definition.NewRuleName;
                definition.MetaData = dtTemp;
                //definition.NewRuleName = GetnewRuleNameIfRuleExists(definition.RuleName);                
                

                definition.PrePostCrossImportAllowed = _compliancePref.PrePostCrossImportAllowed;
                //definition.RuleCategory = _selectedTreeNode.Tag.ToString();
                if (definition.RuleCategory == Constants.CUSTOM_RULE_NODE_TAG && definition.NewRuleName != definition.RuleName)
                    return null;
                    else
                    return definition;
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
                return null;
                    }

                    }


        long _newRuleCounter = 1000;

        private String GetnewRuleNameIfRuleExists(String ruleName)
        {
            try
            {
                if (_userDefinedRulesDict[_selectedPackageName].ContainsKey(ruleName) || CustomRuleManager.GetInstance().IsCustomRuleNamePresent(_selectedPackageName, ruleName))
                {
                    String newRuleName = "";
                    bool newRuleNameFound = false;
                    while (!newRuleNameFound)
                    {
                        newRuleName = ruleName + "_" + _newRuleCounter++;
                        if (_userDefinedRulesDict[_selectedPackageName].ContainsKey(newRuleName) || CustomRuleManager.GetInstance().IsCustomRuleNamePresent(_selectedPackageName, newRuleName))
                            newRuleNameFound = false;
                        else
                            newRuleNameFound = true;
                }
                    return newRuleName;
                
                    //int i = 0;
                    //Boolean isFoundNode = false;
                    //while (i < _userDefinedRulesDict[_selectedPackageName].Count)
                    //{
                    //    i++;
                    //    isFoundNode = false;

                    //    foreach (String key in _userDefinedRulesDict[_selectedPackageName].Keys)
                    //    {
                    //        if (key.Equals(ruleName + "_" + i))
                    //        {
                    //            isFoundNode = true;
                    //            break;
                    //        }
                    //    }
                    //    if (!isFoundNode)
                    //        break;
                    //}
                    //return ruleName + "_" + i;

                }
                else
                    return ruleName;
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
                return null;
            }
        }

        private bool ValidatePath(string path)
        {
            return File.Exists(path + "\\Metadata.xml");
        }

        private List<string> GetRuleDirectoryFromUser()
        {
            List<string> rulePathList = new List<string>();
            try
            {
                FolderBrowserDialog diag = new FolderBrowserDialog();
                if (Directory.Exists(_compliancePref.ImportExportPath))
                    diag.SelectedPath = _compliancePref.ImportExportPath + "\\" + _selectedPackageName+"\\";

                diag.ShowNewFolderButton = false;
                if (diag.ShowDialog(this) == DialogResult.OK)
                {
                    //String[] probPath = Directory.GetDirectories(diag.SelectedPath);
                    rulePathList.AddRange(Directory.GetDirectories(diag.SelectedPath));
                }
                return rulePathList;
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
            return null;
        }

        private void exportRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, ExportDefinition> exportDefCache = new Dictionary<string, ExportDefinition>();
                NotificationSettings notification = _notificationCntrl.GetNotificationSetting(_selectedPackageName, MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText));


                String key = "";
                if (_selectedTreeNode.Parent.Tag.ToString() == Constants.CUSTOM_RULE_NODE_TAG)
                    key = _selectedPackageName + "_" + _selectedTreeNode.Text;
                else
                    key = _selectedPackageName + "_" + MessageFormatter.FormatRuleNameForGuvnor(_selectedTreeNode.Text);


                if (exportDefCache.ContainsKey(key))
                {
                    exportDefCache[key].PackageName = _selectedPackageName;
                    exportDefCache[key].Notification = notification;
                    exportDefCache[key].DirectoryPath = _compliancePref.ImportExportPath;
                    exportDefCache[key].RuleCategory = _selectedTreeNode.Parent.Tag.ToString();
                    if (exportDefCache[key].RuleCategory == Constants.CUSTOM_RULE_NODE_TAG)
                        exportDefCache[key].RuleName = _selectedTreeNode.Text;
                    else
                        exportDefCache[key].RuleName = MessageFormatter.FormatRuleNameForGuvnor(_selectedTreeNode.Text);
                    exportDefCache[key].RuleId = _selectedTreeNode.Name;
                }
                else
                {
                    ExportDefinition exportDef = new ExportDefinition();
                    exportDef.PackageName = _selectedPackageName;
                   exportDef.Notification = notification;
                    exportDef.DirectoryPath = _compliancePref.ImportExportPath;
                    exportDef.RuleCategory = _selectedTreeNode.Parent.Tag.ToString();
                    if (exportDef.RuleCategory == Constants.CUSTOM_RULE_NODE_TAG)
                        exportDef.RuleName = _selectedTreeNode.Text;
                    else
                        exportDef.RuleName = MessageFormatter.FormatRuleNameForGuvnor(_selectedTreeNode.Text);
                    exportDef.RuleId = _selectedTreeNode.Name;
                    exportDefCache.Add(key, exportDef);
                }
                ImportExportManager.GetInstance().Export(exportDefCache);
                //ImportExportManager.GetInstance().Export(_selectedPackageName, _selectedTreeNode.Parent.Tag.ToString(), MessageFormatter.FormatRuleNameForGuvnor(_selectedRuleText), _compliancePref.ImportExportPath, _selectedTreeNode.Name, notification);

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
        private void GetCompliancePref()
        {
            try
            {
                DataSet dsCompliancePref = RulesDAO.GetCompliancePreferences(_companyUser.CompanyID);
                if (dsCompliancePref != null)
                {
                    //Putting a check if default path has not been set from admin
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3245
                    if (dsCompliancePref.Tables.Count > 0 && dsCompliancePref.Tables[0].Rows.Count > 0)
                    {
                    _compliancePref.ImportExportPath = dsCompliancePref.Tables[0].Rows[0]["ImportExportPath"].ToString();
                    _compliancePref.PrePostCrossImportAllowed = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["PrePostCrossImport"].ToString());
                }
                    else
                    {
                        throw new Exception("Import/Export preferences are not set from admin. Please set it then restart the client");
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
    }
}