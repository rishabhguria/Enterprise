using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Prana.Blotter.BusinessObjects;
using Prana.Blotter.Classes;
using Prana.Blotter.Forms;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Blotter
{
    public partial class CtrlBlotterMain : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlBlotterMain"/> class.
        /// </summary>
        public CtrlBlotterMain()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// The trade manager
        /// </summary>
        static Prana.TradeManager.TradeManager _tradeManager;

        /// <summary>
        /// The login user
        /// </summary>
        private CompanyUser _loginUser = null;

        /// <summary>
        /// Occurs when Manual Blotter Import starts.
        /// </summary>
        public event EventHandler SetBeginImportText;

        /// <summary>
        /// Occurs when [enable disable button].
        /// </summary>
        public event EventHandler<EventArgs<bool>> EnableDisableButton;

        /// <summary>
        /// Occurs when [enable disable Merge button].
        /// </summary>
        public event EventHandler<EventArgs<bool>> EnableDisableMergeAndUploadButton;

        /// <summary>
        /// Occurs when [change link unlik BTN caption].
        /// </summary>
        public event EventHandler<EventArgs<string>> ChangeLinkUnlikBtnCaption;

        /// <summary>
        /// Occurs when [highlight symbol send].
        /// </summary>
        public event EventHandler<EventArgs<string>> HighlightSymbolSend = null;

        /// <summary>
        /// Occurs when [go to allocation clicked].
        /// </summary>
        public virtual event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked = null;

        /// <summary>
        /// The blotter preference data
        /// </summary>
        private BlotterPreferenceData _blotterPreferenceData = new BlotterPreferenceData();

        /// <summary>
        /// The blotter preference path
        /// </summary>
        private string _blotterPreferencePath = string.Empty;

        /// <summary>
        /// Tab wise prana grid details
        /// </summary>
        Dictionary<string, PranaUltraGrid> orderTabPranaGrids = new Dictionary<string, PranaUltraGrid>();

        /// <summary>
        /// The linked tab name
        /// </summary>
        private string _linkedTabName = string.Empty;

        /// <summary>
        /// To wire Security Master UI Launch event
        /// </summary>
        public event EventHandler LaunchSecurityMasterForm;

        public event EventHandler<EventArgs<string>> UpdateStatusBar;
        public event EventHandler<EventArgs<string>> UpdateCountStatusBar;

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                OrderBlotterGrid.SecurityMaster = value;
            }
        }

        public event EventHandler DisableRolloverButton;
        public event EventHandler UpdateOnRolloverComplete;

        /// <summary>
        /// Initializes the contol.
        /// </summary>
        /// <param name="loginUser">The login user.</param>
        /// <param name="blotterPreferenceData">The blotter preference data.</param>
        public void InitContol(CompanyUser loginUser, BlotterPreferenceData blotterPreferenceData)
        {
            try
            {
                _blotterPreferenceData = blotterPreferenceData;
                _loginUser = loginUser;
                _tradeManager = TradeManager.TradeManager.GetInstance();
                _blotterPreferencePath = System.Windows.Forms.Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID.ToString();

                //intialize tabs
                WorkingSubBlotterGrid.InitContol(_tradeManager.WorkingSubBlotterCollection, OrderFields.BlotterTypes.WorkingSubs.ToString(), _loginUser, _blotterPreferenceData);
                SummaryBlotterGrid.InitContol(_tradeManager.WorkingSubBlotterCollection, OrderFields.BlotterTypes.Summary.ToString(), _loginUser, _blotterPreferenceData);
                OrderBlotterGrid.InitContol(_tradeManager.OrderBlotterCollection, OrderFields.BlotterTypes.Orders.ToString(), _loginUser, _blotterPreferenceData);

                WorkingSubBlotterGrid.HighlightSymbolSendOnBloterMain += WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;
                SummaryBlotterGrid.HighlightSymbolSendOnBloterMainFromSummary += WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;
                OrderBlotterGrid.HighlightSymbolSendOnBloterMain += WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;

                if (Directory.Exists(_blotterPreferencePath))
                {
                    DirectoryInfo info = new DirectoryInfo(_blotterPreferencePath);
                    FileInfo[] files = info.GetFiles("*BlotterGridLayout.xml").OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo file in files)
                    {
                        string fileName = file.FullName;
                        string tabKey = Path.GetFileName(fileName).Replace("BlotterGridLayout.xml", "");

                        if (tabKey.StartsWith(BlotterConstants.TAB_NAME_Dynamic_Order))
                        {
                            //OrderBlotterGrid subControlWorkingSub = new Blotter.OrderBlotterGrid();
                            OrderBlotterGrid subControl = new Blotter.OrderBlotterGrid();
                            subControl.BlotterType = OrderFields.BlotterTypes.Orders;
                            subControl.InitContol(_tradeManager.OrderBlotterCollection, tabKey, _loginUser, _blotterPreferenceData);
                            subControl.BlotterType = OrderFields.BlotterTypes.DynamicTabOrders;
                            subControl.Dock = DockStyle.Fill;
                            subControl.TradeClick += new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
                            subControl.ReplaceOrEditOrderClicked += new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
                            subControl.LaunchAuditTrail += new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
                            subControl.LaunchAddFills += new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
                            subControl.VisibleChanged += new System.EventHandler(this.OrderBlotterGrid_VisibleChanged);
                            subControl.GoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
                            subControl.RemoveTab += this.OrderBlotterGrid_RemoveTab;
                            subControl.RenameTab += this.OrderBlotterGrid_RenameTab;
                            subControl.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
                            subControl.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
                            subControl.DisableRolloverButton += blotterGrid_DisableRolloverButton;
                            subControl.UpdateOnRolloverComplete += this.blotterGrid_UpdateOnRolloverComplete;
                            subControl.SubOrderBlotterLaunchAddFills += this.WorkingSubBlotterGrid_AddFillsClick;
                            subControl.SubOrderBlotterLaunchAuditTrail += this.WorkingSubBlotterGrid_AuditTrailClick;
                            subControl.SubOrderBlotterGridTradeClick += this.WorkingSubBlotterGrid_TradeClick;
                            subControl.SubOrderBloterGoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
                            subControl.SubOrderBloterReplaceOrEditOrderClicked += this.BlotterGrid_ReplaceOrEditClicked;
                            subControl.SubOrderBloterUpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
                            subControl.HighlightSymbolSendOnBloterMain += WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;

                            UltraTab ultraDynamicTab = this.BlotterTabControl.Tabs.Add(tabKey, tabKey.Replace(BlotterConstants.TAB_NAME_Dynamic_Order, ""));
                            ultraDynamicTab.TabPage.Controls.Add(subControl);
                            orderTabPranaGrids.Add(tabKey, subControl.dgBlotter);
                        }
                        else
                        {
                            if (tabKey.StartsWith("Dynamic_") && !tabKey.StartsWith("Dynamic_SubOrder_"))
                            {
                                WorkingSubBlotterGrid subControl = new Blotter.WorkingSubBlotterGrid();
                                subControl.InitContol(_tradeManager.WorkingSubBlotterCollection, tabKey, _loginUser, _blotterPreferenceData);
                                subControl.BlotterType = OrderFields.BlotterTypes.DynamicTab;
                                subControl.Dock = DockStyle.Fill;
                                subControl.TradeClick += new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
                                subControl.ReplaceOrEditOrderClicked += new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
                                subControl.LaunchAuditTrail += new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
                                subControl.LaunchAddFills += new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
                                subControl.VisibleChanged += new System.EventHandler(this.WorkingSubBlotterGrid_VisibleChanged);
                                subControl.GoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
                                subControl.RemoveTab += this.WorkingSubBlotterGrid_RemoveTab;
                                subControl.RenameTab += this.WorkingSubBlotterGrid_RenameTab;
                                subControl.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
                                subControl.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
                                subControl.DisableRolloverButton += blotterGrid_DisableRolloverButton;
                                subControl.UpdateOnRolloverComplete += this.blotterGrid_UpdateOnRolloverComplete;
                                subControl.HighlightSymbolSendOnBloterMain += WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;

                                UltraTab ultraDynamicTab = this.BlotterTabControl.Tabs.Add(tabKey, tabKey.Replace("Dynamic_", ""));
                                ultraDynamicTab.TabPage.Controls.Add(subControl);
                            }
                        }
                    }


                }
                string filePath = _blotterPreferencePath + "\\BlotterTabOrder.xml";
                UltraTabControlHelper.LoadTabOrder(filePath, this.BlotterTabControl);
                this.BlotterTabControl.FirstDisplayedTab.Selected = true;
                //apply theme
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    if (!_blotterPreferenceData.ApplyColorPreferencesInTheme)
                    {
                        CustomThemeHelper.SetThemeProperties(SummaryBlotterGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_SUMMARY_GRID);
                    }
                    else
                    {
                        CustomThemeHelper.SetThemeProperties(SummaryBlotterGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_SUMMARY_GRID_OVERRIDE);
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
        /// Add Tab Control
        /// </summary>
        /// <param name="key"></param>
        /// <param name="controls"></param>
        private void AddTabControl(string key, ControlCollection controls)
        {

            UltraTab ultraDynamicTab = this.BlotterTabControl.Tabs.Add(key, key.Replace("Dynamic_Order_", "").Replace("Dynamic_", ""));
            foreach (Control control in controls)
            {
                ultraDynamicTab.TabPage.Controls.Add(control);
            }
        }

        /// <summary>
        /// Add Items In Dict
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tab"></param>
        /// <param name="tabListDic"></param>
        private void AddItemsInDict(string key, UltraTab tab, Dictionary<string, Dictionary<string, ControlCollection>> tabListDict)
        {
            try
            {
                if (tabListDict.ContainsKey(key))
                {
                    if (!tabListDict[key].ContainsKey(tab.Key))
                        tabListDict[key].Add(tab.Key, tab.TabPage.Controls);
                }
                else
                {
                    Dictionary<string, ControlCollection> tabDict = new Dictionary<string, ControlCollection>();
                    tabDict.Add(tab.Key, tab.TabPage.Controls);
                    tabListDict.Add(key, tabDict);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void SaveAllLayout()
        {
            try
            {
                if (!Directory.Exists(_blotterPreferencePath))
                    Directory.CreateDirectory(_blotterPreferencePath);

                SaveTabControlLayout();
                foreach (UltraTab ultraTab in BlotterTabControl.Tabs)
                {
                    WorkingSubBlotterGrid.SaveLayoutBlotterGrid(((WorkingSubBlotterGrid)ultraTab.TabPage.Controls[0]).dgBlotter, ((WorkingSubBlotterGrid)ultraTab.TabPage.Controls[0]).Key, _blotterPreferencePath);
                    if (ultraTab.Key.StartsWith(BlotterConstants.TAB_NAME_Dynamic_Order))
                    {
                        OrderBlotterGrid.SaveLayoutSubOrderBlotterGrid(((OrderBlotterGrid)ultraTab.TabPage.Controls[0]).SubOrderBlotterGrid.dgBlotter, _blotterPreferencePath, ultraTab.Key.Replace(BlotterConstants.CAPTION_ORDER_SEPARATOR, BlotterConstants.CAPTION_SUBORDER_SEPARATOR));
                    }
                }
                OrderBlotterGrid.SaveLayoutSubOrderBlotterGrid(null, _blotterPreferencePath, BlotterConstants.TAB_NAME_SUBORDERS);
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string>("Save All Layout completed."));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void SaveTabControlLayout()
        {
            try
            {
                string filePath = _blotterPreferencePath + "\\BlotterTabOrder.xml";

                if (File.Exists(filePath))
                    File.Delete(filePath);

                UltraTabControlHelper.SaveTabOrder(_blotterPreferencePath, filePath, this.BlotterTabControl);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void CancelAllSubs()
        {
            try
            {
                if (OrderBlotterGrid.Visible)
                {
                    OrderBlotterGrid.CancelAllSubsSelectedOrders();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void RolloverAllSubs()
        {
            try
            {
                if (OrderBlotterGrid.Visible)
                {
                    OrderBlotterGrid.RolloverAllSubs();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        internal void MergeOrdersClick()
        {
            try
            {
                if (orderTabPranaGrids.ContainsKey(this.BlotterTabControl.ActiveTab.Key))
                    OrderBlotterGrid.MergeOrdersClick(orderTabPranaGrids[this.BlotterTabControl.ActiveTab.Key]);
                else if (this.BlotterTabControl.ActiveTab.Key.Equals(BlotterConstants.TAB_NAME_ORDERS))
                    OrderBlotterGrid.MergeOrdersClick(null);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        internal void UploadStageOrdersClick()
        {
            try
            {
                StageImport stageImportForm = new StageImport();
                stageImportForm.SetBeginImportText += SetBeginImportText;
                stageImportForm.SetSecurityMasterService(_securityMaster);
                stageImportForm.SetLaunchSecurityMasterForm(LaunchSecurityMasterForm);
                OrderBlotterGrid.Enabled = false;
                stageImportForm.FormClosed += (sender, e) =>
                {
                    OrderBlotterGrid.Enabled = true;
                };
                stageImportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        public void RemoveOrders()
        {
            try
            {
                if (orderTabPranaGrids.ContainsKey(this.BlotterTabControl.ActiveTab.Key))
                    OrderBlotterGrid.RemoveSelectedOrders(orderTabPranaGrids[this.BlotterTabControl.ActiveTab.Key]);
                else if (this.BlotterTabControl.ActiveTab.Key.Equals(BlotterConstants.TAB_NAME_ORDERS))
                    OrderBlotterGrid.RemoveSelectedOrders(null);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region Blotter Grid Events
        /// <summary>
        /// Occurs when [trade click].
        /// </summary>
        public event EventHandler TradeClick = null;

        /// <summary>
        /// Handles the TradeClick event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void WorkingSubBlotterGrid_TradeClick(object sender, System.EventArgs e)
        {
            if (TradeClick != null)
            {
                TradeClick(sender, e);
            }
        }

        /// <summary>
        /// Occurs when [launch add fills].
        /// </summary>
        public event EventHandler LaunchAddFills = null;

        /// <summary>
        /// Handles the AddFillsClick event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WorkingSubBlotterGrid_AddFillsClick(object sender, EventArgs e)
        {
            if (LaunchAddFills != null)
            {
                LaunchAddFills(sender, e);
            }
        }

        /// <summary>
        /// Occurs when [replace or edit order clicked].
        /// </summary>
        public event EventHandler ReplaceOrEditOrderClicked = null;

        /// <summary>
        /// Handles the ReplaceOrEditClicked event of the BlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BlotterGrid_ReplaceOrEditClicked(object sender, EventArgs e)
        {
            if (ReplaceOrEditOrderClicked != null)
            {
                ReplaceOrEditOrderClicked(sender, e);
            }
        }

        /// <summary>
        /// Occurs when [launch audit trail].
        /// </summary>
        public event EventHandler LaunchAuditTrail;

        /// <summary>
        /// Handles the AuditTrailClick event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WorkingSubBlotterGrid_AuditTrailClick(object sender, EventArgs e)
        {
            if (LaunchAuditTrail != null)
            {
                LaunchAuditTrail(sender, e);
            }
        }

        /// <summary>
        /// Handles the RenameTab event of the SubControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WorkingSubBlotterGrid_RenameTab(object sender, EventArgs<string> e)
        {
            try
            {
                string tabKey = e.Value;
                string tabName = InputBox.ShowInputBox("Enter tab name", CharacterCasing.Normal, BlotterTabControl.Tabs[tabKey].Text);
                if (!string.IsNullOrWhiteSpace(tabName) && !tabName.Equals(BlotterTabControl.Tabs[tabKey].Text))
                {
                    if (BlotterTabControl.Tabs.Exists(GetDynamicTabKey(tabName)) || BlotterTabControl.Tabs.Exists(tabName.ToLower()))
                    {
                        //MessageBox.Show(this, "Tab with same name already exists.", BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (UpdateStatusBar != null)
                            UpdateStatusBar(this, new EventArgs<string>("Tab with same name already exists."));
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(this, "Do you want to rename " + BlotterTabControl.Tabs[tabKey].Text + " tab to " + tabName + "?", BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            //Remove current tab from control
                            if (BlotterTabControl.Tabs.Exists(tabKey))
                                BlotterTabControl.Tabs.Remove(BlotterTabControl.Tabs[tabKey]);

                            //Add control with new tab name key
                            WorkingSubBlotterGrid subControl = (sender as WorkingSubBlotterGrid);
                            subControl.Key = GetDynamicTabKey(tabName);
                            subControl.Dock = DockStyle.Fill;
                            AddTabControl(GetDynamicTabKey(tabName), tabName, subControl);

                            this.BlotterTabControl.SelectedTab = this.BlotterTabControl.Tabs[GetDynamicTabKey(tabName)];

                            //Save tab and current tab name with new tab name
                            SaveTabControlLayout();

                            //Remove current tab layout file and add new layout file
                            string oldLayoutFile = _blotterPreferencePath + "\\" + tabKey + "BlotterGridLayout.xml";
                            string newLayoutFile = _blotterPreferencePath + "\\" + GetDynamicTabKey(tabName) + "BlotterGridLayout.xml";

                            System.Threading.Tasks.Task.Run(() => WindsorContainerManager.DeleteCustomViewPreference(_loginUser.CompanyUserID, tabKey + "BlotterGridLayout.xml"));
                            if (File.Exists(oldLayoutFile))
                                File.Move(oldLayoutFile, newLayoutFile);
                            else
                                subControl.SaveLayoutBlotterGrid(subControl.dgBlotter, GetDynamicTabKey(tabName), _blotterPreferencePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderBlotterGrid_RenameTab(object sender, EventArgs<string> e)
        {
            try
            {
                string tabKey = e.Value;
                string tabName = InputBox.ShowInputBox("Enter tab name", CharacterCasing.Normal, BlotterTabControl.Tabs[tabKey].Text);
                if (!string.IsNullOrWhiteSpace(tabName) && !tabName.Equals(BlotterTabControl.Tabs[tabKey].Text))
                {
                    if (BlotterTabControl.Tabs.Exists(GetDynamicOrderTabKey(tabName)) || BlotterTabControl.Tabs.Exists(tabName.ToLower()))
                    {
                        //MessageBox.Show(this, "Tab with same name already exists.", BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (UpdateStatusBar != null)
                            UpdateStatusBar(this, new EventArgs<string>("Tab with same name already exists."));
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(this, "Do you want to rename " + BlotterTabControl.Tabs[tabKey].Text + " tab to " + tabName + "?", BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            //Remove current tab from control
                            if (BlotterTabControl.Tabs.Exists(tabKey))
                                BlotterTabControl.Tabs.Remove(BlotterTabControl.Tabs[tabKey]);

                            //Add control with new tab name key
                            OrderBlotterGrid subControl = (sender as OrderBlotterGrid);
                            subControl.Key = GetDynamicOrderTabKey(tabName);
                            subControl.Dock = DockStyle.Fill;
                            AddTabControl(GetDynamicOrderTabKey(tabName), tabName, subControl);

                            this.BlotterTabControl.SelectedTab = this.BlotterTabControl.Tabs[GetDynamicOrderTabKey(tabName)];
                            //Save tab and current tab name with new tab name
                            SaveTabControlLayout();

                            //Remove current tab layout file and add new layout file
                            string oldLayoutFile = _blotterPreferencePath + "\\" + tabKey + "BlotterGridLayout.xml";
                            string newLayoutFile = _blotterPreferencePath + "\\" + GetDynamicOrderTabKey(tabName) + "BlotterGridLayout.xml";

                            DeleteOldPreferences(tabKey, GetDynamicOrderTabKey(tabName), subControl, oldLayoutFile, newLayoutFile);

                            tabKey = tabKey.Replace(BlotterConstants.CAPTION_ORDER_SEPARATOR, BlotterConstants.CAPTION_SUBORDER_SEPARATOR);
                            oldLayoutFile = oldLayoutFile.Replace(BlotterConstants.CAPTION_ORDER_SEPARATOR, BlotterConstants.CAPTION_SUBORDER_SEPARATOR);
                            newLayoutFile = newLayoutFile.Replace(BlotterConstants.CAPTION_ORDER_SEPARATOR, BlotterConstants.CAPTION_SUBORDER_SEPARATOR);

                            DeleteOldPreferences(tabKey, GetDynamicOrderTabKey(tabName), subControl, oldLayoutFile, newLayoutFile);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void DeleteOldPreferences(string tabKey, string tabName, OrderBlotterGrid subControl, string oldLayoutFile, string newLayoutFile)
        {

            System.Threading.Tasks.Task.Run(() => WindsorContainerManager.DeleteCustomViewPreference(_loginUser.CompanyUserID, tabKey + "BlotterGridLayout.xml"));
            if (File.Exists(oldLayoutFile))
                File.Move(oldLayoutFile, newLayoutFile);
            else
                subControl.SaveLayoutBlotterGrid(subControl.dgBlotter, tabName, _blotterPreferencePath);
        }
        /// <summary>
        /// Handles the RemoveTab event of the SubControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WorkingSubBlotterGrid_RemoveTab(object sender, EventArgs<string> e)
        {
            try
            {
                string tabKey = e.Value;
                DialogResult result = MessageBox.Show(this, "Do you want to remove " + BlotterTabControl.Tabs[tabKey].Text + " tab?", BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (BlotterTabControl.Tabs.Exists(tabKey))
                    {
                        BlotterTabControl.Tabs.Remove(BlotterTabControl.Tabs[tabKey]);
                    }
                    (sender as WorkingSubBlotterGrid).Dispose();

                    //Remove current tab layout file and save tab control layout
                    SaveTabControlLayout();
                    string layoutFile = _blotterPreferencePath + "\\" + tabKey + "BlotterGridLayout.xml";
                    DeleteCustomPreferences(tabKey, layoutFile);

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Handles the RemoveTab event of the SubControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OrderBlotterGrid_RemoveTab(object sender, EventArgs<string> e)
        {
            try
            {
                string tabKey = e.Value;
                DialogResult result = MessageBox.Show(this, "Do you want to remove " + BlotterTabControl.Tabs[tabKey].Text + " tab?", BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (BlotterTabControl.Tabs.Exists(tabKey))
                    {
                        BlotterTabControl.Tabs.Remove(BlotterTabControl.Tabs[tabKey]);
                    }
                    (sender as OrderBlotterGrid).Dispose();


                    //Remove current tab layout file and save tab control layout
                    SaveTabControlLayout();
                    string layoutFile = _blotterPreferencePath + "\\" + tabKey + "BlotterGridLayout.xml";
                    DeleteCustomPreferences(tabKey, layoutFile);

                    tabKey = tabKey.Replace(BlotterConstants.CAPTION_ORDER_SEPARATOR, BlotterConstants.CAPTION_SUBORDER_SEPARATOR);
                    layoutFile = layoutFile.Replace(BlotterConstants.CAPTION_ORDER_SEPARATOR, BlotterConstants.CAPTION_SUBORDER_SEPARATOR);

                    string SubGridlayoutFile = _blotterPreferencePath + "\\" + tabKey + "BlotterGridLayout.xml";
                    DeleteCustomPreferences(tabKey, layoutFile);


                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void DeleteCustomPreferences(string tabKey, string layoutFile)
        {

            System.Threading.Tasks.Task.Run(() => WindsorContainerManager.DeleteCustomViewPreference(_loginUser.CompanyUserID, tabKey + "BlotterGridLayout.xml"));
            if (File.Exists(layoutFile))
            {
                File.Delete(layoutFile);
            }
        }

        #endregion

        private void SummaryBlotterGrid_VisibleChanged(object sender, EventArgs e)
        {
            if (EnableDisableButton != null && EnableDisableMergeAndUploadButton != null)
            {
                if (((System.Windows.Forms.Control)sender).Visible)
                {
                    EnableDisableButton(this, new EventArgs<bool>(false));
                    EnableDisableMergeAndUploadButton(this, new EventArgs<bool>(false));
                }
            }
        }

        private void WorkingSubBlotterGrid_VisibleChanged(object sender, EventArgs e)
        {
            if (EnableDisableButton != null && EnableDisableMergeAndUploadButton != null)
            {
                if (((System.Windows.Forms.Control)sender).Visible)
                {
                    EnableDisableButton(this, new EventArgs<bool>(true));
                    EnableDisableMergeAndUploadButton(this, new EventArgs<bool>(false));
                }
            }
        }

        private void OrderBlotterGrid_VisibleChanged(object sender, EventArgs e)
        {
            if (EnableDisableButton != null && EnableDisableMergeAndUploadButton != null)
            {
                if (((System.Windows.Forms.Control)sender).Visible)
                {
                    EnableDisableButton(this, new EventArgs<bool>(true));
                    EnableDisableMergeAndUploadButton(this, new EventArgs<bool>(true));
                }
            }
        }

        private void BlotterGrid_GoToAllocationClicked(object sender, EventArgs<string, DateTime, DateTime> e)
        {
            try
            {
                if (GoToAllocationClicked != null)
                    GoToAllocationClicked(this, new EventArgs<string, DateTime, DateTime>(e.Value, e.Value2, e.Value3));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        internal void UpdateAllocationDetails(List<AllocationDetails> allocationDetails)
        {
            try
            {
                if (allocationDetails.Count > 0)
                {
                    List<string> updatedOrderParentClOrderIDs = new List<string>();

                    updatedOrderParentClOrderIDs.AddRange(BlotterUICommonMethods.UpdateAllocationDetails(allocationDetails, WorkingSubBlotterGrid.dgBlotter));
                    updatedOrderParentClOrderIDs.AddRange(OrderBlotterGrid.UpdateAllocationDetailsOrderSubOrders(allocationDetails));

                    //If the details has been updated in the grid the remove it from List
                    if (updatedOrderParentClOrderIDs.Count > 0)
                    {
                        List<AllocationDetails> detailsToRemove = allocationDetails.Where(x => updatedOrderParentClOrderIDs.Contains(x.ClOrderID)).ToList();

                        if (detailsToRemove.Count > 0)
                            detailsToRemove.ForEach(details => allocationDetails.Remove(details));
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

        internal void ClearSubOrderBlotterGrid()
        {
            try
            {
                OrderBlotterGrid.ClearSubOrdersGrid();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the manual sub order blotter grid.
        /// </summary>
        internal void ClearManualSubOrderBlotterGrid(string clOrderID)
        {
            try
            {
                OrderBlotterGrid.ClearManualSubOrdersGrid(clOrderID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }



        /// <summary>
        /// Set Grid Band Single Band/ MultiBand
        /// </summary>
        /// <param name="viewStyle"></param>
        internal void SetGridBand(Infragistics.Win.UltraWinGrid.ViewStyle viewStyleForOrdersGrid, bool isHiddenBand)
        {
            try
            {
                foreach (UltraTab tab in BlotterTabControl.Tabs)
                {
                    if (tab.Text != "Summary")
                        ((WorkingSubBlotterGrid)tab.TabPage.Controls[0]).SetGridBand(viewStyleForOrdersGrid, isHiddenBand);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        internal void UpdateSubOrderBlotterGrid(List<AllocationDetails> allocationDetails, List<OrderSingle> incomingOrders)
        {
            try
            {
                OrderBlotterGrid.UpdateSubOrderBlotterGrid(incomingOrders);
                foreach (UltraTab ultraTab in BlotterTabControl.Tabs)
                {
                    if (ultraTab.Key.StartsWith("Dynamic_Order_"))
                    {
                        ((OrderBlotterGrid)ultraTab.TabPage.Controls[0]).UpdateSubOrderBlotterGrid(incomingOrders);
                    }
                }
                UpdateAllocationDetails(allocationDetails);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// SaveAuditTrailMergedOrder
        /// </summary>
        /// <param name="incomingOrders"></param>
        internal void SaveAuditTrailMergedOrder(string incomingOrdersClOrderId)
        {
            try
            {
                OrderBlotterGrid.SaveAuditTrailData(true, incomingOrdersClOrderId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the tab.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        internal void AddTab(string tabName)
        {
            try
            {
                if (BlotterTabControl.Tabs.Exists(GetDynamicTabKey(tabName)) || BlotterTabControl.Tabs.Exists(tabName.ToLower()))
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Tab with same name already exists."));
                }
                else
                {
                    WorkingSubBlotterGrid subControl = new Blotter.WorkingSubBlotterGrid();
                    subControl.InitContol(_tradeManager.WorkingSubBlotterCollection, GetDynamicTabKey(tabName), _loginUser, _blotterPreferenceData);
                    subControl.BlotterType = OrderFields.BlotterTypes.DynamicTab;
                    subControl.Dock = DockStyle.Fill;
                    subControl.TradeClick += new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
                    subControl.ReplaceOrEditOrderClicked += new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
                    subControl.LaunchAuditTrail += new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
                    subControl.LaunchAddFills += new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
                    subControl.VisibleChanged += new System.EventHandler(this.WorkingSubBlotterGrid_VisibleChanged);
                    subControl.GoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
                    subControl.RemoveTab += this.WorkingSubBlotterGrid_RemoveTab;
                    subControl.RenameTab += this.WorkingSubBlotterGrid_RenameTab;
                    subControl.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
                    subControl.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
                    subControl.DisableRolloverButton += blotterGrid_DisableRolloverButton;
                    subControl.UpdateOnRolloverComplete += this.blotterGrid_UpdateOnRolloverComplete;
                    subControl.HighlightSymbolSendOnBloterMain += WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;
                    AddTabControl(GetDynamicTabKey(tabName), tabName, subControl);

                    CustomThemeHelper.SetThemeProperties(subControl, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_OVERRIDE);
                    //Save tab control and new tab layout
                    SaveTabControlLayout();
                    subControl.SaveLayoutBlotterGrid(subControl.dgBlotter, GetDynamicTabKey(tabName), _blotterPreferencePath);
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
        /// 
        /// </summary>
        /// <param name="tabName1"></param>
        internal void AddOrdersTab(string tabName)
        {
            try
            {
                if (BlotterTabControl.Tabs.Exists(GetDynamicOrderTabKey(tabName)) || BlotterTabControl.Tabs.Exists(tabName.ToLower()))
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Tab with same name already exists."));
                }
                else
                {
                    OrderBlotterGrid subControlWorkingSub = new Blotter.OrderBlotterGrid();
                    subControlWorkingSub.BlotterType = OrderFields.BlotterTypes.Orders;
                    subControlWorkingSub.InitContol(_tradeManager.OrderBlotterCollection, GetDynamicOrderTabKey(tabName), _loginUser, _blotterPreferenceData);
                    subControlWorkingSub.BlotterType = OrderFields.BlotterTypes.DynamicTabOrders;
                    subControlWorkingSub.Dock = DockStyle.Fill;

                    subControlWorkingSub.TradeClick += new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
                    subControlWorkingSub.ReplaceOrEditOrderClicked += new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
                    subControlWorkingSub.LaunchAuditTrail += new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
                    subControlWorkingSub.LaunchAddFills += new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
                    subControlWorkingSub.VisibleChanged += new System.EventHandler(this.OrderBlotterGrid_VisibleChanged);
                    subControlWorkingSub.GoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
                    subControlWorkingSub.RemoveTab += this.OrderBlotterGrid_RemoveTab;
                    subControlWorkingSub.RenameTab += this.OrderBlotterGrid_RenameTab;
                    subControlWorkingSub.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
                    subControlWorkingSub.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
                    subControlWorkingSub.DisableRolloverButton += blotterGrid_DisableRolloverButton;
                    subControlWorkingSub.UpdateOnRolloverComplete += this.blotterGrid_UpdateOnRolloverComplete;
                    subControlWorkingSub.SubOrderBlotterLaunchAddFills += this.WorkingSubBlotterGrid_AddFillsClick;
                    subControlWorkingSub.SubOrderBlotterLaunchAuditTrail += this.WorkingSubBlotterGrid_AuditTrailClick;
                    subControlWorkingSub.SubOrderBlotterGridTradeClick += this.WorkingSubBlotterGrid_TradeClick;
                    subControlWorkingSub.SubOrderBloterGoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
                    subControlWorkingSub.SubOrderBloterReplaceOrEditOrderClicked += this.BlotterGrid_ReplaceOrEditClicked;
                    subControlWorkingSub.SubOrderBloterUpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
                    subControlWorkingSub.HighlightSymbolSendOnBloterMain += WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;

                    AddTabControl(GetDynamicOrderTabKey(tabName), tabName, subControlWorkingSub);
                    //Save tab control and new tab layout
                    SaveTabControlLayout();
                    CustomThemeHelper.SetThemeProperties(subControlWorkingSub, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_OVERRIDE);
                    subControlWorkingSub.SaveLayoutBlotterGrid(subControlWorkingSub.dgBlotter, GetDynamicOrderTabKey(tabName), _blotterPreferencePath);
                    orderTabPranaGrids.Add(GetDynamicOrderTabKey(tabName), subControlWorkingSub.dgBlotter);
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
        /// Adds the new tab control.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="subControl">The sub control.</param>
        private void AddTabControl(string dynamicTabKeyName, string tabName, WorkingSubBlotterGrid subControl)
        {
            try
            {
                UltraTab ultraDynamicTab = this.BlotterTabControl.Tabs.Add(dynamicTabKeyName, tabName);
                ultraDynamicTab.TabPage.Controls.Add(subControl);
                ultraDynamicTab.Selected = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the dynamic tab key.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        /// <returns></returns>
        private static string GetDynamicTabKey(string tabName)
        {
            string tabKey = string.Empty;
            try
            {
                tabKey = "Dynamic_" + tabName.Trim();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tabKey;
        }


        private static string GetDynamicOrderTabKey(string tabName)
        {
            string tabKey = string.Empty;
            try
            {
                tabKey = "Dynamic_Order_" + tabName.Trim();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tabKey;
        }

        internal void BlotterGrid_UpdateStatusBar(object sender, EventArgs<string> e)
        {
            try
            {
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string>(e.Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }



        internal void BlotterGrid_UpdateCountStatusBar(object sender, EventArgs<string> e)
        {
            try
            {
                if (UpdateCountStatusBar != null)
                    UpdateCountStatusBar(this, new EventArgs<string>(e.Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        internal void blotterGrid_DisableRolloverButton(object sender, EventArgs e)
        {
            try
            {
                if (DisableRolloverButton != null)
                    DisableRolloverButton(this, new EventArgs());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// UpdateOnRolloverComplete value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void blotterGrid_UpdateOnRolloverComplete(object sender, EventArgs e)
        {
            try
            {
                if (UpdateOnRolloverComplete != null)
                    UpdateOnRolloverComplete(this, new EventArgs());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        internal void EnableDisableTab(bool isEnabled)
        {
            BlotterTabControl.Enabled = isEnabled;
        }

        internal UltraGridRow GetActiveRowOfOrderBlotter()
        {
            try
            {
                return OrderBlotterGrid.GetActiveRowOfOrderBlotter();
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
        /// Highlights the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        internal void HighlightSymbol(string symbol)
        {
            try
            {
                foreach (UltraTab ultraTab in BlotterTabControl.Tabs)
                {
                    if (ultraTab.Selected)
                    {
                        PranaUltraGrid grd = ((WorkingSubBlotterGrid)ultraTab.TabPage.Controls[0]).dgBlotter;
                        UltraGridRow row = null;
                        foreach (UltraGridRow oRow in grd.Rows)
                        {
                            if (ultraTab.Key.Equals("Summary"))
                            {
                                if (oRow.Description != null)
                                {
                                    string symb = oRow.Description.Split().Last();
                                    if (symb.Equals(symbol))
                                    {
                                        row = oRow;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                string strCellValue = oRow.Cells[OrderFields.PROPERTY_SYMBOL].Text;
                                if (strCellValue.Equals(symbol))
                                {
                                    row = oRow;
                                    break;
                                }
                            }
                        }
                        if (row != null)
                        {
                            grd.ActiveRow = row;
                            row.Selected = true;
                        }
                        break;
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

        /// <summary>
        /// Highlights the symbol from QTT.
        /// </summary>
        /// <param name="linkingData">The linking data.</param>
        internal void HighlightSymbolFromQTT(QTTBlotterLinkingData linkingData)
        {
            try
            {
                foreach (UltraTab ultraTab in BlotterTabControl.Tabs)
                {
                    if (!ultraTab.Key.Equals("Summary"))
                    {
                        ((WorkingSubBlotterGrid)ultraTab.TabPage.Controls[0]).AddUpdateLinkingFromQTT(linkingData);
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

        /// <summary>
        /// DeHighlights the symbol from QTT.
        /// </summary>
        /// <param name="linkingData">The linking data.</param>
        internal void DeHighlightSymbolFromQTT(QTTBlotterLinkingData linkingData)
        {
            try
            {
                foreach (UltraTab ultraTab in BlotterTabControl.Tabs)
                {
                    if (!ultraTab.Key.Equals("Summary"))
                    {
                        ((WorkingSubBlotterGrid)ultraTab.TabPage.Controls[0]).DeleteLinkingWithQTT(linkingData.Index);
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

        /// <summary>
        /// Unwire the events.
        /// </summary>
        internal void UnwireEvents()
        {
            foreach (UltraTab tab in this.BlotterTabControl.Tabs)
            {
                foreach (var control in tab.TabPage.Controls)
                {
                    if (control is WorkingSubBlotterGrid)
                    {
                        var subcontrol = (control as WorkingSubBlotterGrid);
                        subcontrol.TradeClick -= new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
                        subcontrol.ReplaceOrEditOrderClicked -= new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
                        subcontrol.LaunchAuditTrail -= new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
                        subcontrol.LaunchAddFills -= new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
                        subcontrol.VisibleChanged -= new System.EventHandler(this.WorkingSubBlotterGrid_VisibleChanged);
                        subcontrol.GoToAllocationClicked -= this.BlotterGrid_GoToAllocationClicked;
                        subcontrol.RemoveTab -= this.WorkingSubBlotterGrid_RemoveTab;
                        subcontrol.RenameTab -= this.WorkingSubBlotterGrid_RenameTab;
                        subcontrol.UpdateStatusBar -= this.BlotterGrid_UpdateStatusBar;
                        subcontrol.UpdateCountStatusBar -= this.BlotterGrid_UpdateCountStatusBar;
                        subcontrol.DisableRolloverButton -= blotterGrid_DisableRolloverButton;
                        subcontrol.UpdateOnRolloverComplete -= this.blotterGrid_UpdateOnRolloverComplete;
                        subcontrol.HighlightSymbolSendOnBloterMain -= WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;
                    }

                    if (control is OrderBlotterGrid)
                    {
                        var subcontrol = (control as OrderBlotterGrid);
                        subcontrol.TradeClick -= new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
                        subcontrol.ReplaceOrEditOrderClicked -= new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
                        subcontrol.LaunchAuditTrail -= new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
                        subcontrol.LaunchAddFills -= new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
                        subcontrol.VisibleChanged -= new System.EventHandler(this.OrderBlotterGrid_VisibleChanged);
                        subcontrol.GoToAllocationClicked -= this.BlotterGrid_GoToAllocationClicked;
                        subcontrol.RemoveTab -= this.OrderBlotterGrid_RemoveTab;
                        subcontrol.RenameTab -= this.OrderBlotterGrid_RenameTab;
                        subcontrol.UpdateStatusBar -= this.BlotterGrid_UpdateStatusBar;
                        subcontrol.UpdateCountStatusBar -= this.BlotterGrid_UpdateCountStatusBar;
                        subcontrol.DisableRolloverButton -= blotterGrid_DisableRolloverButton;
                        subcontrol.UpdateOnRolloverComplete -= this.blotterGrid_UpdateOnRolloverComplete;
                        subcontrol.HighlightSymbolSendOnBloterMain -= WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;
                    }

                    if (control is SummaryBlotterGrid)
                    {
                        var subcontrol = (control as SummaryBlotterGrid);
                        subcontrol.TradeClick -= new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
                        subcontrol.HighlightSymbolSendOnBloterMainFromSummary -= WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the TabIndexChanged event of the BlotterTabControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BlotterTabControl_TabIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (_linkedTabName == this.BlotterTabControl.Tabs[this.BlotterTabControl.ActiveTab.Index].Key)
                    ChangeLinkUnlikBtnCaption(null, new EventArgs<string>(BlotterConstants.CAPTION_UNLINK_TAB));
                else
                    ChangeLinkUnlikBtnCaption(null, new EventArgs<string>(BlotterConstants.CAPTION_LINK_TAB));

                string countStatusBarMessage = string.Empty;
                UltraTab ultraTab = this.BlotterTabControl.Tabs[this.BlotterTabControl.ActiveTab.Index];
                if (ultraTab.Key.StartsWith(BlotterConstants.TAB_NAME_Dynamic_Order) || ultraTab.Key.Equals(BlotterConstants.TAB_NAME_ORDERS))
                {
                    OrderBlotterGrid grid = ((OrderBlotterGrid)ultraTab.TabPage.Controls[0]);
                    countStatusBarMessage = grid.GetCheckedCount();
                }
                if (UpdateCountStatusBar != null)
                    UpdateCountStatusBar(this, new EventArgs<string>(countStatusBarMessage));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Sets the link unlink startup.
        /// </summary>
        internal void SetLinkUnlinkStartup()
        {
            try
            {
                _linkedTabName = _tradeManager.GetBlotterLinkedTabName();
                if (_linkedTabName == this.BlotterTabControl.Tabs[this.BlotterTabControl.ActiveTab.Index].Key)
                    ChangeLinkUnlikBtnCaption(null, new EventArgs<string>(BlotterConstants.CAPTION_UNLINK_TAB));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the HighlightSymbolSendOnBloterMain event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void WorkingSubBlotterGrid_HighlightSymbolSendOnBloterMain(object sender, EventArgs<string> e)
        {
            try
            {
                if (_linkedTabName == this.BlotterTabControl.Tabs[this.BlotterTabControl.ActiveTab.Index].Key)
                    HighlightSymbolSend(null, new EventArgs<string>(e.Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Links the unlik tab.
        /// </summary>
        public string LinkUnlikActiveTab()
        {
            string linkUnlinkCaption = BlotterConstants.CAPTION_LINK_TAB;
            try
            {
                if (_linkedTabName != this.BlotterTabControl.Tabs[this.BlotterTabControl.ActiveTab.Index].Key)
                {
                    linkUnlinkCaption = BlotterConstants.CAPTION_UNLINK_TAB;
                    _tradeManager.SetBlotterLinkedTabName(this.BlotterTabControl.Tabs[this.BlotterTabControl.ActiveTab.Index].Key);
                    linkUnlinkCaption = BlotterConstants.CAPTION_UNLINK_TAB;
                    _linkedTabName = this.BlotterTabControl.Tabs[this.BlotterTabControl.ActiveTab.Index].Key;
                }
                else
                {
                    _tradeManager.SetBlotterLinkedTabName(string.Empty);
                    _linkedTabName = string.Empty;
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return linkUnlinkCaption;
        }

        private void ExportBlotterTabToExcel(string tabName, string filePath)
        {
            SetGridBand(Infragistics.Win.UltraWinGrid.ViewStyle.MultiBand, false);
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
            // If tabName is empty, get the active tab
            if (string.IsNullOrEmpty(tabName))
            {
                var activeTab = BlotterTabControl.SelectedTab;
                if (activeTab != null)
                {
                    string sheetName = BlotterUICommonMethods.SplitCamelCase(activeTab.Key);
                    workBook = ((WorkingSubBlotterGrid)activeTab.TabPage.Controls[0])
                        .OnExportToExcel(workBook, filePath, sheetName);

                    workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
                    workBook.Save(filePath);
                }
                return;
            }
            // Try to find the tab using the name
            foreach (UltraTab ultraTab in BlotterTabControl.Tabs)
            {
                string key = ultraTab.Key;
                if (tabName.Contains(key))
                {
                    // Found the tab, do the export
                    string sheetName = BlotterUICommonMethods.SplitCamelCase(key);
                    workBook = ((WorkingSubBlotterGrid)ultraTab.TabPage.Controls[0])
                    .OnExportToExcel(workBook, filePath, sheetName);

                    workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
                    workBook.Save(filePath);
                    break;
                }
            }
        }


        public void ExportData(string tabName, string filePath)
        {
            ExportBlotterTabToExcel(tabName, filePath);
        }
    }

}