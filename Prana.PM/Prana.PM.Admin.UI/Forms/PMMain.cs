using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
//using Prana.PM.Common;
using Prana.PM.DAL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.PM.Admin.UI.Forms
{
    public partial class PMMain : Form
    {
        //private AddDataSource frmAddDataSource = null;
        private AddUploadClient frmAddUploadClient = null;

        private bool isMethodCallFlagTree = false;
        private bool isMethodCallFlagTab = false;

        private int _selectedTreeNodeID;


        /// <summary>
        /// Gets or sets the selected tree node ID.
        /// </summary>
        /// <value>The selected tree node ID.</value>
        public int SelectedTreeNodeID
        {
            get { return _selectedTreeNodeID; }
            set { _selectedTreeNodeID = value; }
        }

        //private MapAUEC frmMapAUEC = null;
        //private MapColumns frmMapColumns = null;
        //private MapSymbol frmMapSymbol = null;



        /// <summary>
        /// Initializes a new instance of the <see cref="PMMain"/> class.
        /// </summary>
        public PMMain()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!int.Equals(treePMMain.SelectedNodes.Count, 0))
            {
                string key = treePMMain.SelectedNodes[0].Key;


                if (treePMMain.SelectedNodes[0].Level > 0)
                {
                    this.SelectedTreeNodeID = Convert.ToInt32(key.Substring(0, key.Length - 2));
                }

                if (string.Equals(key, "DS") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["DS"])
                {
                    //if (frmAddDataSource == null)
                    //{
                    //frmAddDataSource = new AddDataSource();
                    // frmAddDataSource.Owner = this;
                    //frmAddDataSource.ShowInTaskbar = false;

                    // }
                    //frmAddDataSource.Show();
                    //frmAddDataSource.Activate();

                }
                else if (string.Equals(key, "RU") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["RU"])
                {
                    if (frmAddUploadClient == null)
                    {
                        frmAddUploadClient = new AddUploadClient();
                        frmAddUploadClient.Owner = this;
                        frmAddUploadClient.ShowInTaskbar = false;

                    }
                    frmAddUploadClient.Show();
                    frmAddUploadClient.Activate();
                    frmAddUploadClient.PopulateChildControls();

                }
                else
                {
                    MessageBox.Show("New Company Cannot be added from here.", "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                //if (frmAddDataSource != null)
                //    frmAddDataSource.Disposed += new EventHandler(frmAddDataSource_Disposed);

                if (frmAddUploadClient != null)
                    frmAddUploadClient.Disposed += new EventHandler(frmAddUploadClient_Disposed);
            }
            else
            {
                MessageBox.Show("Please select an item from DataSource Node, or Upload Client Node to Add", "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        /// <summary>
        /// Handles the Disposed event of the frmAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frmAddDataSource_Disposed(object sender, EventArgs e)
        {
            BindTree(true);

            // frmAddDataSource = null;
        }

        /// <summary>
        /// Handles the Disposed event of the frmAddUploadClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frmAddUploadClient_Disposed(object sender, EventArgs e)
        {
            frmAddUploadClient = null;
        }

        /// <summary>
        /// Handles the Load event of the PMMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PMMain_Load(object sender, EventArgs e)
        {
            SetUpForm();
            //ctrlSetupTradeRecon1.InitControl();
        }

        //SelectColumns _frmSetupColumns = null;

        /// <summary>
        /// Sets the up form.
        /// </summary>
        private void SetUpForm()
        {
            //   SetMapColumnsForm();

            BindTree(false);
            InitControl();
        }

        /// <summary>
        /// TODO : Need to send using the controller
        /// </summary>
        private void InitControl()
        {
            //ctrlSetupTradeRecon1.CancelClicked += new EventHandler(ctrlSetupTradeRecon1_CancelClicked);
            //ctrlRunTradeRecon1.ViewExceptionReport += new EventHandler(ctrlRunTradeRecon1_ViewExceptionReport);

            //ctrlImportSetup1.OpenAccountMapping += new EventHandler(ctrlImportSetup1_OpenAccountMapping);
            //ctrlImportSetup1.OpenSetupColumns += new EventHandler(ctrlImportSetup1_OpenSetupColumns);
            ////ctrlImportSetup1.OpenColumnMapping += new EventHandler(ctrlImportSetup1_OpenColumnMapping);
            //ctrlImportSetup1.OpenAuecMapping += new EventHandler(ctrlImportSetup1_OpenAuecMapping);
            //ctrlImportSetup1.OpenSymbolMapping += new EventHandler(ctrlImportSetup1_OpenSymbolMapping);

            //ctrlTransactionManagement1.OpenEnterTransactions += new EventHandler(ctrlTransactionManagement1_OpenEnterTransactions);
            // ctrlCashBalanceManagement1.OpenCashReconDetails += new EventHandler(ctrlCashBalanceManagement1_OpenCashReconDetails);
        }

        void ctrlSetupTradeRecon1_CancelClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        void ctrlCashBalanceManagement1_OpenCashReconDetails(object sender, EventArgs e)
        {
            //CashReconDetails cashReconDetails = new CashReconDetails();
            //cashReconDetails.Show();
        }


        //void ctrlTransactionManagement1_OpenEnterTransactions(object sender, EventArgs e)
        //{
        //    EnterTransactions enterTransactions = new EnterTransactions();
        //    enterTransactions.Show();
        //}

        void ctrlImportSetup1_OpenSymbolMapping(object sender, EventArgs e)
        {
            MapSymbol mapSymbol = new MapSymbol();
            mapSymbol.Show();
        }

        void ctrlImportSetup1_OpenAuecMapping(object sender, EventArgs e)
        {
            MapAUEC mapAUEC = new MapAUEC();
            mapAUEC.Show();
        }

        //void ctrlImportSetup1_OpenColumnMapping(object sender, EventArgs e)
        //{
        //    MapColumns mapColumns = new MapColumns(((DataSourceEventArgs)e).DataSourceNameID);
        //    mapColumns.Show();
        //}

        //void ctrlImportSetup1_OpenSetupColumns(object sender, EventArgs e)
        //{
        //    DataSourceEventArgs dsArgs = (DataSourceEventArgs)e;
        //    SelectColumns setupColumns = new SelectColumns(dsArgs.DataSourceNameID);
        //    setupColumns.Show();
        //}



        //void ctrlRunTradeRecon1_ViewExceptionReport(object sender, EventArgs e)
        //{
        //    ExceptionReport exceptionReport = new ExceptionReport();
        //    exceptionReport.Show();
        //}

        /// <summary>
        /// Handles the OpenAccountMapping event of the ctrlImportSetup1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //void ctrlImportSetup1_OpenAccountMapping(object sender, EventArgs e)
        //{
        //    MapAccounts mapAccounts = new MapAccounts();
        //    mapAccounts.Show();
        //}


        /// <summary>
        /// Sets the map columns form.
        /// </summary>
        //private void SetMapColumnsForm()
        //{
        //    if (_frmSetupColumns == null)
        //    {
        //        _frmSetupColumns = new SelectColumns();
        //        _frmSetupColumns.Owner = this;
        //        _frmSetupColumns.ShowInTaskbar = false;

        //        _frmSetupColumns.Show();
        //        _frmSetupColumns.Activate();
        //    }

        //    // ctrlImportSetup1.DataSourceEvent += new EventHandler(_frmSetupColumns.SetDataSourceName);
        //}

        /// <summary>
        /// Binds the tree.
        /// </summary>
        private void BindTree(bool isFromAddDataSource)
        {
            try
            {
                //
                // Bind Tree to the form
                //
                isMethodCallFlagTree = true;
                treePMMain.Nodes.Clear();
                isMethodCallFlagTree = false;

                //UltraTreeNode treeNodeParentDataSources = new UltraTreeNode("DS", "DataSources");
                //UltraTreeNode treeNodeParentCompany = new UltraTreeNode("CM", "Clients");
                UltraTreeNode treeNodeParentRunUpload = new UltraTreeNode("RU", "Upload Client");


                Font font = new Font("Tahoma", 11, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);


                //treeNodeParentDataSources.Tag = new Prana.BusinessObjects.PositionManagement.DataSourceNameID();
                //UltraTreeNode treeNodeParentDataSources = new UltraTreeNode();
                //treeNodeParentDataSourceseFont = font;            
                //treeNodeParentDataSources.Tag = 0;
                //String newDataSourceKey = string.Empty;
                //SortableSearchableList<Prana.BusinessObjects.PositionManagement.DataSourceNameID> dataSourceNamesList = new SortableSearchableList<Prana.BusinessObjects.PositionManagement.DataSourceNameID>();
                //try
                //{
                //    dataSourceNamesList = DataSourceManager.GetAllDataSourceNames();
                //}
                //catch (Exception ex)
                //{
                //    // Invoke our policy that is responsible for making sure no secure information
                //    // gets out of our layer.
                //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                //    if (rethrow)
                //    {
                //        throw;
                //    }
                //}
                //int loopCount = 0;
                //foreach (Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceName in dataSourceNamesList)
                //{
                //    loopCount = loopCount + 1;
                //    UltraTreeNode treeNodeDataSource = new UltraTreeNode(Convert.ToString(dataSourceName.ID) + "DS", dataSourceName.ShortName);
                //    treeNodeDataSource.Tag = dataSourceName;
                //    // treeNodeDataSource.Tag = dataSourceName.ID.ToString() + "DS" ;
                //    // treeNodeDataSource.Tag = dataSource.ID;
                //    //treeNodeParentDataSources.Nodes.Add(treeNodeDataSource);
                //    //if (isFromAddDataSource && int.Equals(dataSourceNamesList.Count, loopCount))
                //    //{
                //    //    newDataSourceKey = Convert.ToString(dataSourceName.ID) + "DS";
                //    //}
                //}

                //treePMMain.Nodes.Add(treeNodeParentDataSources);


                // treePMMain.PerformAction(UltraTreeAction.ParentNode, false, false);                        




                //treeNodeParentCompany.Tag = new Prana.BusinessObjects.PositionManagement.CompanyNameID();
                treeNodeParentRunUpload.Tag = new Prana.BusinessObjects.PositionManagement.CompanyNameID();

                //treeNodeParentCompany.NodeFont = font;
                // treeNodeParentCompany.Tag = 1;

                Prana.BusinessObjects.PositionManagement.CompanyNameIDList companyNameIDList = CompanyManager.GetCompanyNameIDList();

                foreach (Prana.BusinessObjects.PositionManagement.CompanyNameID companyNameID in companyNameIDList)
                {
                    //UltraTreeNode treeNodecompany = new UltraTreeNode(Convert.ToString(companyNameID.ID) + "CM", companyNameID.FullName);
                    //treeNodecompany.Tag = companyNameID;
                    ////treeNodeParentCompany.Nodes.Add(treeNodecompany);

                    UltraTreeNode treeNodeRunUpload = new UltraTreeNode(Convert.ToString(companyNameID.ID) + "RU", companyNameID.ShortName);
                    treeNodeRunUpload.Tag = companyNameID;
                    treeNodeParentRunUpload.Nodes.Add(treeNodeRunUpload);

                }
                //treePMMain.Nodes.Add(treeNodeParentCompany);
                treePMMain.Nodes.Add(treeNodeParentRunUpload);
                treePMMain.ExpandAll();

                if (isFromAddDataSource)
                {
                    // isMethodCallFlagTree = true;
                    //if (treePMMain.Nodes["DS"].Nodes.Count > 0)
                    //{
                    //    treePMMain.Nodes["DS"].Nodes[newDataSourceKey].Selected = true;
                    //}
                    //else
                    {
                        treePMMain.Nodes[0].Selected = true;
                    }
                    // isMethodCallFlagTree = false;
                }

                if (treePMMain.Nodes[0].Nodes.Count > 0)
                {
                    treePMMain.Nodes[0].Nodes[0].Selected = true;
                }
                else
                {
                    treePMMain.Nodes[0].Selected = true;
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
        /// Handles the SplitterMoved event of the splitContainer1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.SplitterEventArgs"/> instance containing the event data.</param>
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }


        /// <summary>
        /// Handles the AfterSelect event of the treePMMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTree.SelectEventArgs"/> instance containing the event data.</param>
        private void treePMMain_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                if (!isMethodCallFlagTree)
                {

                    string key = treePMMain.SelectedNodes[0].Key;
                    if (treePMMain.SelectedNodes[0].Level > 0)
                    {
                        this.SelectedTreeNodeID = Convert.ToInt32(key.Substring(0, key.Length - 2));
                    }
                    else
                    {
                        switch (key)
                        {
                            //case "DS":
                            //    //disable and select import admin tab
                            //    this.tabMainPMAdmin.Tabs["ImportAdmin"].Enabled = false;
                            //    this.tabMainPMAdmin.Tabs["ImportAdmin"].Selected = true;
                            //    //enable other tabs
                            //    this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = true;
                            //    this.tabMainPMAdmin.Tabs["RunUpload"].Enabled = true;
                            //    break;
                            //case "CM":
                            //    this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = false;
                            //    this.tabMainPMAdmin.Tabs["SetUpCompany"].Selected = true;
                            //    //this.ctrlCompanyDetails1.RefreshDetails();
                            //    //enable other tabs
                            //    this.tabMainPMAdmin.Tabs["ImportAdmin"].Enabled = true;
                            //    this.tabMainPMAdmin.Tabs["RunUpload"].Enabled = true;
                            //    break;
                            case "RU":
                                this.tabMainPMAdmin.Tabs["RunUpload"].Enabled = false;
                                this.tabMainPMAdmin.Tabs["RunUpload"].Selected = true;
                                this.ctrlRunUploadSetup1.RefreshControls();

                                //enable other tabs
                                this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = true;
                                this.tabMainPMAdmin.Tabs["ImportAdmin"].Enabled = true;
                                break;
                            default:
                                break;
                        }


                        return;

                    }
                    //if (treePMMain.Nodes["DS"] != null && (key.Equals("DS") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["DS"]))
                    //{
                    //    btnAdd.Visible = true;
                    //    isMethodCallFlagTab = true;
                    //    this.tabMainPMAdmin.Tabs["ImportAdmin"].Selected = true;
                    //    this.tabDataSource.Tabs["DataSourceDetails"].Selected = true;
                    //    isMethodCallFlagTab = false;
                    //    if (treePMMain.SelectedNodes[0].Level > 0)
                    //    {
                    //        foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in this.tabDataSource.Tabs)
                    //        {
                    //            tab.Enabled = true;
                    //        }
                    //        this.tabMainPMAdmin.Tabs["ImportAdmin"].Enabled = true;
                    //        if (bool.Equals(this.tabDataSource.Tabs["DataSourceDetails"].Selected, true))
                    //        {
                    //            this.tabMainPMAdmin.Tabs["ImportAdmin"].Selected = true;
                    //            this.tabDataSource.Tabs["DataSourceDetails"].Selected = true;
                    //            //this.ctrlDataSourceDetails1.PopulateDataSourceDetails((DataSourceNameID)treePMMain.SelectedNodes[0].Tag);
                    //        }
                    //        //else
                    //        //{
                    //        //    this.ctrlImportSetup1.PopulateImportSetUpDetails((DataSourceNameID)treePMMain.SelectedNodes[0].Tag);
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        //this.ctrlDataSourceDetails1.PopulateDataSourceDetails((DataSourceNameID)treePMMain.SelectedNodes[0].Tag);
                    //    }
                    //}
                    //else if (treePMMain.Nodes["CM"] != null && (treePMMain.SelectedNodes[0].Key.Equals("CM") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["CM"]))
                    //{
                    //    btnAdd.Visible = false;
                    //    isMethodCallFlagTab = true;
                    //    this.tabMainPMAdmin.Tabs["SetUpCompany"].Selected = true;
                    //    this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = true;
                    //    this.ultraTabCompanyDetails.Tabs["CompanyDetails"].Selected = true;
                    //    isMethodCallFlagTab = false;
                    //    if (treePMMain.SelectedNodes[0].Level > 0)
                    //    {
                    //        foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in this.ultraTabCompanyDetails.Tabs)
                    //        {
                    //            tab.Enabled = true;
                    //        }
                    //        this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = true;
                    //        //if (bool.Equals(this.ultraTabCompanyDetails.Tabs["CompanyDetails"].Selected, true))
                    //        //{
                    //        //    this.ctrlCompanyDetails1.PopulateCompanyDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                    //        //}
                    //        //if (bool.Equals(this.ultraTabCompanyDetails.Tabs["ApplicationDetails"].Selected, true))
                    //        //{
                    //        //    this.ctrlCompanyApplicationDetails1.PopulateCompanyApplicationDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                    //        //}
                    //        //else if (bool.Equals(this.ultraTabCompanyDetails.Tabs["MapAccounts"].Selected, true))
                    //        //{
                    //        //    this.ctrlMapAccounts1.InitControl((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                    //        //}
                    //        // this.ctrlDataSourceDetails1.PopulateDataSourceDetails(selectedTreeNodeID);
                    //    }
                    //    //else
                    //    //{
                    //    //    //this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = false;
                    //    //    this.ctrlCompanyDetails1.RefreshDetails();
                    //    //}
                    //}

                    if (treePMMain.Nodes["RU"] != null && (treePMMain.SelectedNodes[0].Key.Equals("RU") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["RU"]))
                    {


                        try
                        {
                            btnAdd.Visible = false;
                            isMethodCallFlagTab = true;
                            this.tabMainPMAdmin.Tabs["RunUpload"].Selected = true;
                            this.tabSetUpRunUpload.Tabs["SetUpRunUpload"].Selected = true;
                            isMethodCallFlagTab = false;
                            if (treePMMain.SelectedNodes[0].Level > 0)
                            {
                                foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in this.tabSetUpRunUpload.Tabs)
                                {
                                    tab.Enabled = true;
                                }
                                this.tabMainPMAdmin.Tabs["RunUpload"].Enabled = true;
                                if (bool.Equals(this.tabSetUpRunUpload.Tabs["SetUpRunUpload"].Selected, true))
                                {
                                    this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                                }
                                //else if (bool.Equals(this.tabSetUpRunUpload.Tabs["RunUploadRun"].Selected, true))
                                //{
                                //    bool isPermissionGiven = RunUploadManager.GetAllowDailyImportStatusForUploadClient(((CompanyNameID)treePMMain.SelectedNodes[0].Tag));
                                //    if (isPermissionGiven)
                                //        this.ctrlRunUpload1.PopulateRunUploadDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                                //    else
                                //        this.tabSetUpRunUpload.Tabs["RunUploadRun"].Enabled = false;
                                //}
                                // TO Do : code to populate controls for the country.
                                //this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails(SelectedTreeNodeID);

                            }
                            else
                            {
                                //this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag); 
                                //bool isPermissionGiven = RunUploadManager.GetAllowDailyImportStatusForUploadClient(((CompanyNameID)treePMMain.SelectedNodes[0].Tag));
                                //if (isPermissionGiven)
                                //    this.ctrlRunUpload1.PopulateRunUploadDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                                //else
                                //    this.tabSetUpRunUpload.Tabs["RunUploadRun"].Enabled = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "exception");
                //throw;
            }

        }



        #region InitControl on the tab change - TAB MAIN
        /// <summary>
        /// Handles the SelectedTabChanged event of the tabMainPMAdmin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        private void tabMainPMAdmin_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (!isMethodCallFlagTab)
            {
                if (!int.Equals(treePMMain.Nodes.Count, 0))
                {
                    string selectedTab = e.Tab.Key;
                    isMethodCallFlagTree = true;
                    switch (selectedTab)
                    {
                        //case "ImportAdmin":
                        //    treePMMain.Nodes["DS"].Selected = true;
                        //    isMethodCallFlagTab = true;
                        //    this.tabDataSource.Tabs["DataSourceDetails"].Selected = true;
                        //    //this.ctrlDataSourceDetails1.ClearControl();
                        //    isMethodCallFlagTab = false;
                        //    this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = true;
                        //    this.tabMainPMAdmin.Tabs["RunUpload"].Enabled = true;
                        //    foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in this.tabDataSource.Tabs)
                        //    {
                        //        tab.Enabled = false;
                        //    }
                        //    break;
                        //case "SetUpCompany":
                        //    treePMMain.Nodes["CM"].Selected = true;
                        //    isMethodCallFlagTab = true;
                        //    this.ultraTabCompanyDetails.Tabs["CompanyDetails"].Selected = true;
                        //    //this.ctrlCompanyDetails1.ClearControl();
                        //    isMethodCallFlagTab = false;
                        //    foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in this.ultraTabCompanyDetails.Tabs)
                        //    {
                        //        tab.Enabled = false;
                        //    }
                        //    this.tabMainPMAdmin.Tabs["ImportAdmin"].Enabled = true;
                        //    this.tabMainPMAdmin.Tabs["RunUpload"].Enabled = true;
                        //    break;
                        case "RunUpload":

                            try
                            {
                                treePMMain.Nodes["RU"].Selected = true;
                                isMethodCallFlagTab = true;
                                this.tabSetUpRunUpload.Tabs["SetUpRunUpload"].Selected = true;
                                if (treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["RU"])
                                {
                                    this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                                }
                                isMethodCallFlagTab = false;

                                foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in this.tabSetUpRunUpload.Tabs)
                                {
                                    tab.Enabled = false;
                                }
                                this.tabMainPMAdmin.Tabs["SetUpCompany"].Enabled = true;
                                this.tabMainPMAdmin.Tabs["ImportAdmin"].Enabled = true;
                            }
                            catch (Exception ex)
                            {
                                string s = ex.Message;
                                throw;
                            }
                            break;
                        default:
                            treePMMain.HideSelection = true;
                            break;
                    }

                    isMethodCallFlagTree = false;
                }

            }
        }
        #endregion

        #region InitControl on the tab change - Import Admin
        /// <summary>
        /// Handles the SelectedTabChanged event of the tabDataSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        private void tabDataSource_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (!isMethodCallFlagTab)
            {
                if (!int.Equals(treePMMain.Nodes.Count, 0))
                {
                    // DataSourceNameID selectedNode = (DataSourceNameID)treePMMain.SelectedNodes[0].Tag;
                    // if (!int.Equals(selectedNodeID, 0))
                    //  {

                    //   treePMMain.Nodes["DS"].Selected = true;

                    // }
                    // else
                    //  {
                    //      treePMMain.Nodes["DS"].
                    //  }
                    if (!SelectedTreeNodeID.Equals(0) && treePMMain.SelectedNodes[0].Tag != null)
                    {
                        if (e.Tab.Key.Equals("DataSourceDetails"))
                        {
                            //this.ctrlDataSourceDetails1.PopulateDataSourceDetails((DataSourceNameID)treePMMain.SelectedNodes[0].Tag);
                        }
                        //else if (e.Tab.Key.Equals("ImportSetUp"))
                        //{
                        //    this.ctrlImportSetup1.PopulateImportSetUpDetails((DataSourceNameID)treePMMain.SelectedNodes[0].Tag);
                        //}
                    }

                }
            }
        }
        #endregion

        #region InitControl on the tab change - Set Up Company -- Commented
        /// <summary>
        /// Handles the SelectedTabChanged event of the tab for Company Details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        //private void ultraTabCompanyDetails_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        //{
        //    if (!isMethodCallFlagTab)
        //    {
        //        if (!int.Equals(treePMMain.Nodes.Count, 0))
        //        {
        //            //treePMMain.Nodes["CM"].Selected = true;
        //            if (!SelectedTreeNodeID.Equals(0) && treePMMain.SelectedNodes[0].Tag != null)
        //            {
        //                if (e.Tab.Key.Equals("CompanyDetails"))
        //                {
        //                    this.ctrlCompanyDetails1.PopulateCompanyDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
        //                }
        //                else if (e.Tab.Key.Equals("ApplicationDetails"))
        //                {
        //                    this.ctrlCompanyApplicationDetails1.PopulateCompanyApplicationDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
        //                }
        //                else if(e.Tab.Key.Equals("MapAccounts"))
        //                {
        //                    this.ctrlMapAccounts1.InitControl((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        #region InitControl on the tab change - RunUpload
        /// <summary>
        /// Handles the SelectedTabChanged event of the tabSetUpRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        private void tabSetUpRunUpload_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (!isMethodCallFlagTab)
                {
                    if (!int.Equals(treePMMain.Nodes.Count, 0))
                    {

                        // treePMMain.Nodes["RU"].Selected = true;

                        if (!SelectedTreeNodeID.Equals(0) && treePMMain.SelectedNodes[0].Tag != null)
                        {
                            if (e.Tab.Key.Equals("SetUpRunUpload"))
                            {
                                this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                            }
                            else if (e.Tab.Key.Equals("RunUploadRun"))
                            {
                                //this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                                //bool isPermissionGiven = RunUploadManager.GetAllowDailyImportStatusForUploadClient(((CompanyNameID)treePMMain.SelectedNodes[0].Tag));
                                //if (isPermissionGiven)
                                //    this.ctrlRunUpload1.PopulateRunUploadDetails((CompanyNameID)treePMMain.SelectedNodes[0].Tag);
                                //else
                                //    this.tabSetUpRunUpload.Tabs["RunUploadRun"].Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }

        }
        #endregion

        #region InitControl on the tab change - Reconcilliation
        private void tabReconciliation_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {


            //if (e.Tab.Key.Equals("SetUp Recon"))
            //{
            //    ctrlSetupTradeRecon1.InitControl();
            //}
            //else if (e.Tab.Key.Equals("Run Re-con"))
            //{
            //    ctrlRunTradeRecon1.InitControl();
            //}
        }
        #endregion

        #region InitControl on the tab change - Cash Management
        /// <summary>
        /// Handles the SelectedTabChanged event of the tabCashManagement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        private void tabCashManagement_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //if (e.Tab.Key.Equals("Reconciliation"))
            //{
            //    ctrlCashRecon1.InitControl();
            //}
            //else if (string.Equals(e.Tab.Key, "BalanceManagement"))
            //{
            //    ctrlCashBalanceManagement1.InitControl();
            //}
            //else if (string.Equals(e.Tab.Key, "TransactionManagement"))
            //{
            //    ctrlTransactionManagement1.InitControl();
            //}

        }
        #endregion

        private void ctrlRunUpload1_MouseClick(object sender, MouseEventArgs e)
        {
            //bool isPermissionGiven = RunUploadManager.GetAllowDailyImportStatusForUploadClient(((CompanyNameID)treePMMain.SelectedNodes[0].Tag));
            //if (!isPermissionGiven)
            //{
            //    MessageBox.Show(this, "Please select the Allow daily upload option from Company Application details tab to run upload the data source files.", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void tabSetUpRunUpload_MouseClick(object sender, MouseEventArgs e)
        {
            //bool isPermissionGiven = RunUploadManager.GetAllowDailyImportStatusForUploadClient(((CompanyNameID)treePMMain.SelectedNodes[0].Tag));
            //if (!isPermissionGiven)
            //{
            //    if (tabSetUpRunUpload.Tabs["RunUploadRun"].Active == true)
            //    {
            //        MessageBox.Show(this, "Please select the Allow daily upload option from Company Application details tab to run upload the data source files.", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
        }







    }
}
