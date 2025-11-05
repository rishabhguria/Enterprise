using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.Controls;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Properties;
using Infragistics.Win.UltraWinTree;


namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class PMMain : Form
    {
        private AddDataSource frmAddDataSource = null;
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
                    if (frmAddDataSource == null)
                    {
                        frmAddDataSource = new AddDataSource();
                        frmAddDataSource.Owner = this;
                        frmAddDataSource.ShowInTaskbar = false;

                    }
                    frmAddDataSource.Show();
                    frmAddDataSource.Activate();

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

                if (frmAddDataSource != null)
                    frmAddDataSource.Disposed += new EventHandler(frmAddDataSource_Disposed);

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
            frmAddDataSource = null;
        }

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
            ctrlSetupTradeRecon1.InitControl();
        }

        SelectColumns _frmSetupColumns = null;

        /// <summary>
        /// Sets the up form.
        /// </summary>
        private void SetUpForm()
        {
         //   SetMapColumnsForm();

            BindTree();
            InitControl();
        }

        /// <summary>
        /// TODO : Need to send using the controller
        /// </summary>
        private void InitControl()
        {
            ctrlSetupTradeRecon1.CancelClicked += new EventHandler(ctrlSetupTradeRecon1_CancelClicked);
            //ctrlRunTradeRecon1.ViewExceptionReport += new EventHandler(ctrlRunTradeRecon1_ViewExceptionReport);

            //ctrlImportSetup1.OpenFundMapping += new EventHandler(ctrlImportSetup1_OpenFundMapping);
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

        
        void ctrlTransactionManagement1_OpenEnterTransactions(object sender, EventArgs e)
        {
            EnterTransactions enterTransactions = new EnterTransactions();
            enterTransactions.Show();
        }

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

        void ctrlImportSetup1_OpenColumnMapping(object sender, EventArgs e)
        {
            MapColumns mapColumns = new MapColumns(((DataSourceEventArgs)e).DataSourceNameID);
            mapColumns.Show();
        }

        void ctrlImportSetup1_OpenSetupColumns(object sender, EventArgs e)
        {
            DataSourceEventArgs dsArgs = (DataSourceEventArgs) e;

            SelectColumns setupColumns = new SelectColumns(dsArgs.DataSourceNameID);

            setupColumns.Show();
        }

        

        //void ctrlRunTradeRecon1_ViewExceptionReport(object sender, EventArgs e)
        //{
        //    ExceptionReport exceptionReport = new ExceptionReport();
        //    exceptionReport.Show();
        //}

        void ctrlImportSetup1_OpenFundMapping(object sender, EventArgs e)
        {
            MapFunds mapFunds = new MapFunds();
            mapFunds.Show();
        }
        

        private void SetMapColumnsForm()
        {
            if (_frmSetupColumns == null)
            {
                _frmSetupColumns = new SelectColumns();
                _frmSetupColumns.Owner = this;
                _frmSetupColumns.ShowInTaskbar = false;

                _frmSetupColumns.Show();
                _frmSetupColumns.Activate();
            }

           // ctrlImportSetup1.DataSourceEvent += new EventHandler(_frmSetupColumns.SetDataSourceName);
        }

        /// <summary>
        /// Binds the tree.
        /// </summary>
        private void BindTree()
        {
            //
            // Bind Tree to the form
            //
            treePMMain.Nodes.Clear();                  

            Font font = new Font("Tahoma", 11, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            UltraTreeNode treeNodeParentDataSources = new UltraTreeNode("DS", "DataSources");
            treeNodeParentDataSources.Tag = new DataSourceNameID();
           
            //UltraTreeNode treeNodeParentDataSources = new UltraTreeNode();
            //treeNodeParentDataSourceseFont = font;            
            //treeNodeParentDataSources.Tag = 0;

            SortableSearchableList<DataSourceNameID> dataSourceNamesList = DataSourceManager.GetAllDataSourceNames();
            
            foreach (DataSourceNameID dataSourceName in dataSourceNamesList)
            {
                UltraTreeNode treeNodeDataSource = new UltraTreeNode(Convert.ToString(dataSourceName.ID) + "DS", dataSourceName.ShortName);
                treeNodeDataSource.Tag = dataSourceName;
               // treeNodeDataSource.Tag = dataSourceName.ID.ToString() + "DS" ;

               // treeNodeDataSource.Tag = dataSource.ID;
                treeNodeParentDataSources.Nodes.Add(treeNodeDataSource);
                
            }
            treePMMain.Nodes.Add(treeNodeParentDataSources);
            
            
           // treePMMain.PerformAction(UltraTreeAction.ParentNode, false, false);                        



            UltraTreeNode treeNodeParentCompany = new UltraTreeNode("CM", "Companies");
            treeNodeParentCompany.Tag = new CompanyNameID();

            UltraTreeNode treeNodeParentRunUpload = new UltraTreeNode("RU", "Upload Client");
            treeNodeParentCompany.Tag = new CompanyNameID();
            //treeNodeParentCompany.NodeFont = font;
           // treeNodeParentCompany.Tag = 1;

            SortableSearchableList<CompanyNameID> companyNameIDList = CompanyManager.GetCompanyNameIDList();
            
            foreach (CompanyNameID companyNameID in companyNameIDList)
            {
                UltraTreeNode treeNodecompany = new UltraTreeNode(Convert.ToString(companyNameID.ID) + "CM", companyNameID.FullName);
                treeNodecompany.Tag = companyNameID;
                treeNodeParentCompany.Nodes.Add(treeNodecompany);

                UltraTreeNode treeNodeRunUpload = new UltraTreeNode(Convert.ToString(companyNameID.ID) + "RU", companyNameID.ShortName);
                treeNodeRunUpload.Tag = companyNameID;
                treeNodeParentRunUpload.Nodes.Add(treeNodeRunUpload);

            }
            treePMMain.Nodes.Add(treeNodeParentCompany);
            treePMMain.Nodes.Add(treeNodeParentRunUpload);
                        
            treePMMain.ExpandAll();

           // treePMMain.Nodes[0].Selected = true;
        }

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
            if (!isMethodCallFlagTree)
            {
                string key = treePMMain.SelectedNodes[0].Key;
                if (treePMMain.SelectedNodes[0].Level > 0)
                {
                    this.SelectedTreeNodeID = Convert.ToInt32(key.Substring(0, key.Length - 2));
                }
                if (treePMMain.Nodes["DS"] != null && (key.Equals("DS") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["DS"]))
                {
                    btnAdd.Visible = true;
                    isMethodCallFlagTab = true;
                    this.tabMainPMAdmin.Tabs["ImportAdmin"].Selected = true;
                    
                    this.tabDataSource.Tabs["DataSourceDetails"].Selected = true;
                    isMethodCallFlagTab = false;
                    if (treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["DS"])
                    {
                        if (bool.Equals(this.tabDataSource.Tabs["DataSourceDetails"].Selected, true))
                        {
                            this.tabMainPMAdmin.Tabs["ImportAdmin"].Selected = true;
                            this.tabDataSource.Tabs["DataSourceDetails"].Selected = true;
                            this.ctrlDataSourceDetails1.PopulateDataSourceDetails((DataSourceNameID)treePMMain.SelectedNodes[0].Tag);
                        }
                        else
                        {
                            this.ctrlImportSetup1.DataSourceNameIDValue = (DataSourceNameID)treePMMain.SelectedNodes[0].Tag;
                            this.ctrlImportSetup1.PopulateImportSetUpDetails(this.SelectedTreeNodeID);
                        }
                    }
                }
                else if (treePMMain.Nodes["CM"] != null && (treePMMain.SelectedNodes[0].Key.Equals("CM") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["CM"]))
                {
                    btnAdd.Visible = false;
                    isMethodCallFlagTab = true;
                        this.tabMainPMAdmin.Tabs["SetUpCompany"].Selected = true;
                        
                    this.ultraTabCompanyDetails.Tabs["CompanyDetails"].Selected = true;
                    isMethodCallFlagTab = false;
                    if (treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["CM"])
                    {
                        if (bool.Equals(this.ultraTabCompanyDetails.Tabs["CompanyDetails"].Selected, true))
                        {
                            this.ctrlCompanyDetails1.PopulateCompanyDetails(SelectedTreeNodeID);
                        }
                        else
                        {
                            this.ctrlCompanyApplicationDetails1.PopulateCompanyApplicationDetails(SelectedTreeNodeID);
                        }
                        // this.ctrlDataSourceDetails1.PopulateDataSourceDetails(selectedTreeNodeID);
                    }
                }
                else if (treePMMain.Nodes["RU"] != null && (treePMMain.SelectedNodes[0].Key.Equals("RU") || treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["RU"]))
                {

                    btnAdd.Visible = true;
                    isMethodCallFlagTab = true;
                    this.tabMainPMAdmin.Tabs["RunUpload"].Selected = true;                        
                    this.tabSetUpRunUpload.Tabs["SetUpRunUpload"].Selected = true;
                    isMethodCallFlagTab = false;
                    //if (treePMMain.SelectedNodes[0].Parent == treePMMain.Nodes["RU"])
                    //{
                    //    if (bool.Equals(this.tabSetUpRunUpload.Tabs["SetUpRunUpload"], true))
                    //    {
                            this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails(1);
                        //}
                        //else if (bool.Equals(this.tabSetUpRunUpload.Tabs["RunUploadRun"], true))
                        //{
                         //   this.ctrlRunUpload1.PopulateRunUploadDetails(1);
                        //}
                        // TO Do : code to populate controls for the country.
                        //this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails(SelectedTreeNodeID);                        
                    }

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
                        case "ImportAdmin":
                            treePMMain.Nodes["DS"].Selected = true;
                            isMethodCallFlagTab = true;
                            this.tabDataSource.Tabs["DataSourceDetails"].Selected = true;
                            this.ctrlDataSourceDetails1.ClearControl();
                            isMethodCallFlagTab = false;
                            break;
                        case "SetUpCompany":
                            treePMMain.Nodes["CM"].Selected = true;
                            isMethodCallFlagTab = true;
                            this.ultraTabCompanyDetails.Tabs["CompanyDetails"].Selected = true;
                            this.ctrlCompanyDetails1.ClearControl();
                            isMethodCallFlagTab = false;
                            break;
                        case "RunUpload":
                            treePMMain.Nodes["RU"].Selected = true;
                            isMethodCallFlagTab = true;
                            this.tabSetUpRunUpload.Tabs["SetUpRunUpload"].Selected = true;
                            this.ctrlRunUploadSetup1.ClearControl();
                            isMethodCallFlagTab = false;
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
                    if (!SelectedTreeNodeID.Equals(0))
                    {
                        if (e.Tab.Key.Equals("DataSourceDetails"))
                        {
                            this.ctrlDataSourceDetails1.PopulateDataSourceDetails((DataSourceNameID)treePMMain.SelectedNodes[0].Tag);
                        }
                        else if (e.Tab.Key.Equals("ImportSetUp"))
                        {

                            this.ctrlImportSetup1.DataSourceNameIDValue = (DataSourceNameID)treePMMain.SelectedNodes[0].Tag;
                            this.ctrlImportSetup1.PopulateImportSetUpDetails(this.SelectedTreeNodeID);
                        }
                    }

                }
            }
        } 
        #endregion

        #region InitControl on the tab change - Set Up Company
        /// <summary>
        /// Handles the SelectedTabChanged event of the tab for Company Details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        private void ultraTabCompanyDetails_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (!isMethodCallFlagTab)
            {
                if (!int.Equals(treePMMain.Nodes.Count, 0))
                {

                    //treePMMain.Nodes["CM"].Selected = true;
                    if (!SelectedTreeNodeID.Equals(0))
                    {
                        if (e.Tab.Key.Equals("CompanyDetails"))
                        {
                            this.ctrlCompanyDetails1.PopulateCompanyDetails(SelectedTreeNodeID);

                        }
                        else if (e.Tab.Key.Equals("ApplicationDetails"))
                        {

                            this.ctrlCompanyApplicationDetails1.PopulateCompanyApplicationDetails(SelectedTreeNodeID);
                        }
                    }
                }
            }

        }
        #endregion

        #region InitControl on the tab change - RunUpload
        /// <summary>
        /// Handles the SelectedTabChanged event of the tabSetUpRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        private void tabSetUpRunUpload_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (!isMethodCallFlagTab)
            {
                if (!int.Equals(treePMMain.Nodes.Count, 0))
                {

                   // treePMMain.Nodes["RU"].Selected = true;

                    if (!SelectedTreeNodeID.Equals(0))
                    {
                        if (e.Tab.Key.Equals("SetUpRunUpload"))
                        {
                            this.ctrlRunUploadSetup1.PopulateUploadSetUpDetails(1);
                        }
                        else if (e.Tab.Key.Equals("RunUploadRun"))
                        {
                            this.ctrlRunUpload1.PopulateRunUploadDetails(1);
                        }
                    }
                }
            }

        }
        #endregion

        #region InitControl on the tab change - Reconcilliation
        private void tabReconciliation_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            

            if (e.Tab.Key.Equals("SetUp Recon"))
            {
                ctrlSetupTradeRecon1.InitControl();
            }
            else if (e.Tab.Key.Equals("Run Re-con"))
            {
                ctrlRunTradeRecon1.InitControl();
            }
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
            if (e.Tab.Key.Equals("Reconciliation"))
            {
                ctrlCashRecon1.InitControl();
            }
            else if (string.Equals(e.Tab.Key, "BalanceManagement"))
            {
                ctrlCashBalanceManagement1.InitControl();
            }
            else if (string.Equals(e.Tab.Key, "TransactionManagement"))
            {
                ctrlTransactionManagement1.InitControl();
            }
            
        }
        #endregion







    }
}
