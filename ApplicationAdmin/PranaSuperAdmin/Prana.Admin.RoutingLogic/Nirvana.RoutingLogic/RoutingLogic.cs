using Prana.Admin.RoutingLogic.MisclFunctions;
using System;
using System.Data;
using System.Windows.Forms;


namespace Prana.Admin.RoutingLogic.Forms
{
    /// <summary>
    /// Summary description for RoutingLogic.
    /// </summary>
    public class CompanyMaster : System.Windows.Forms.Form
    {
        //		private Infragistics.Win.UltraWinTree.UltraTree treeMain;
        private System.Windows.Forms.TreeView treeMain;
        private System.Windows.Forms.Panel panelControls;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabctrlMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage tabsharedpage;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabpageRL;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabpageClient;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabctrlRoutingLogic;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage tabsharedpageRL;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabpageRoutingLogic;
        private Infragistics.Win.Misc.UltraButton btnAdd;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Controls.RLogic ucRLogic;
        private Controls.Client ucClient;
        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        //		private System.Data.DataTable dtMemoryRL;
        //		private System.Data.DataTable dtMemoryGroupClient;
        private System.Data.DataTable dtClientList;
        private System.Data.DataTable dtRLList;
        private System.Data.DataTable dtParameters;
        private System.Data.DataTable dtAUEC;
        private System.Data.DataTable dtCounterPartyVenue;
        private System.Data.DataTable dtTradingAccount;
        private System.Data.DataTable dtTree;
        private int ipkCompanyID = Functions.MinValue;
        private string strTabID = "client";
        private bool bUserTriggered = true;
        private const string strTabIDRL = "RL";
        private const string strTabIDGroup = "group";
        private const string strTabIDClient = "client";
        //		private System.Windows.Forms.NodeTree nodeMain;// =  new NodeTree();
        private System.Windows.Forms.NodeTree nodeMain;
        //private object objMessageLoad ;

        //		private 		System.Data.DataSet dsData ;

        public CompanyMaster()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //			this.nodeMain =  new NodeTree();
            //			this.CompanyMaster(new System.Windows.Forms.NodeTree() , 20);

        }

        //		public CompanyMaster(System.Windows.Forms.NodeTree _nodeMain, int _iCompanyID)
        public CompanyMaster(System.Windows.Forms.NodeTree _nodeMain, int _iCompanyID)
        {
            //= new MessageBox();
            //			MessageBox.Show( "Loading...","Starting...", System.Windows.Forms.MessageBoxButtons.OK);
            InitializeComponent();

            //			this.GetCompanyID();
            //			this.nodeMain = _nodeMain ;
            //			ipkCompanyID =_iCompanyID ;

            dataRL = BLL.DataRoutingLogicObjects.GetInstance;

            dsData = new DataSet("Data");

            Prana.Admin.RoutingLogic.BLL.DataCallFunctionsManager.InitializeDataCall(ref dsData, ref dataRL, _iCompanyID);
            //dtMemoryRL = new DataTable("Memory");
            //			dsData.Tables.Add(dtMemoryRL);
            //			dsData.Tables.Add(dtParameters);
            //			dsData.Tables.Add(dtAUEC);
            //			dsData.Tables.Add(dtCounterPartyVenue);
            //			dsData.Tables.Add(dtTradingAccount);
            //			dsData.Tables.Add(dtTree);
            //dsData.Tables.Add(dtMemoryRL);

            this.tabpageClient.Size = new System.Drawing.Size(1500, 374);

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            //	LoadDataToDataSet();
            //Data.DataHandelingManager

            //			this.treeMain.Leave += new System.EventHandler(Functions.object_LostFocus);
            //			this.treeMain.Enter += new System.EventHandler(Functions.object_GotFocus);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new System.Drawing.Point(50, 000);
            this.BringToFront();
            this.AutoScroll = true;


            LoadData(_nodeMain, _iCompanyID);


        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (treeMain != null)
                {
                    treeMain.Dispose();
                }
                if (panelControls != null)
                {
                    panelControls.Dispose();
                }
                if (tabctrlMain != null)
                {
                    tabctrlMain.Dispose();
                }
                if (tabsharedpage != null)
                {
                    tabsharedpage.Dispose();
                }
                if (tabpageRL != null)
                {
                    tabpageRL.Dispose();
                }
                if (tabpageClient != null)
                {
                    tabpageClient.Dispose();
                }
                if (tabctrlRoutingLogic != null)
                {
                    tabctrlRoutingLogic.Dispose();
                }
                if (tabsharedpageRL != null)
                {
                    tabsharedpageRL.Dispose();
                }
                if (tabpageRoutingLogic != null)
                {
                    tabpageRoutingLogic.Dispose();
                }
                if (btnAdd != null)
                {
                    btnAdd.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (ucRLogic != null)
                {
                    ucRLogic.Dispose();
                }
                if (ucClient != null)
                {
                    ucClient.Dispose();
                }
                if (dsData != null)
                {
                    dsData.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            //			Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
            //			System.Windows.Forms.NodeTree ultraTreeNode1 = new System.Windows.Forms.NodeTree();
            //			System.Windows.Forms.NodeTree ultraTreeNode2 = new System.Windows.Forms.NodeTree();
            //			System.Windows.Forms.NodeTree ultraTreeNode3 = new System.Windows.Forms.NodeTree();
            //			Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CompanyMaster));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();

            this.treeMain = new System.Windows.Forms.TreeView();
            this.tabpageRL = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabpageRoutingLogic = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ucRLogic = new Prana.Admin.RoutingLogic.Controls.RLogic();
            this.tabpageClient = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ucClient = new Prana.Admin.RoutingLogic.Controls.Client();
            //			this.treeMain = new Infragistics.Win.UltraWinTree.UltraTree();
            this.panelControls = new System.Windows.Forms.Panel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.tabctrlMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.tabsharedpage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tabctrlRoutingLogic = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.tabsharedpageRL = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tabpageRL.SuspendLayout();
            this.tabpageClient.SuspendLayout();
            this.tabpageRoutingLogic.SuspendLayout();

            //			((System.ComponentModel.ISupportInitialize)(this.treeMain)).BeginInit();
            this.panelControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlMain)).BeginInit();
            this.tabctrlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlRoutingLogic)).BeginInit();
            this.tabctrlRoutingLogic.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabpageRL
            // 
            this.tabpageRL.AutoScroll = true;
            this.tabpageRL.Controls.Add(this.ucRLogic);
            this.tabpageRL.Location = new System.Drawing.Point(1, 1);
            this.tabpageRL.Name = "tabpageRL";
            this.tabpageRL.Size = new System.Drawing.Size(701, 430);
            this.tabpageRL.Tag = "tabpageRL";
            // 
            // tabpageRoutingLogic
            // 
            this.tabpageRoutingLogic.AutoScroll = true;
            //			this.tabpageRoutingLogic.Controls.Add(this.ucRLogic);
            this.tabpageRoutingLogic.Location = new System.Drawing.Point(1, 1);
            this.tabpageRoutingLogic.Name = "tabpageRoutingLogic";
            this.tabpageRoutingLogic.Size = new System.Drawing.Size(701, 430);
            this.tabpageRoutingLogic.Tag = "tabpageRoutingLogic";
            this.tabpageRoutingLogic.AutoScroll = false;

            this.tabpageRoutingLogic.Controls.Add(this.tabctrlMain);
            // 
            // ucRLogic
            // 
            this.ucRLogic.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.ucRLogic.Location = new System.Drawing.Point(2, 4);
            this.ucRLogic.Name = "ucRLogic";
            this.ucRLogic.Size = new System.Drawing.Size(701, 430);
            this.ucRLogic.TabIndex = 0;
            // 
            // tabpageClient
            // 
            this.tabpageClient.AutoScroll = true;
            this.tabpageClient.Controls.Add(this.ucClient);
            this.tabpageClient.Location = new System.Drawing.Point(1, 1);
            this.tabpageClient.Name = "tabpageClient";
            this.tabpageClient.Size = new System.Drawing.Size(701, 430);
            // 
            // ucClient
            // 
            this.ucClient.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.ucClient.Location = new System.Drawing.Point(2, 4);
            this.ucClient.Name = "ucClient";
            this.ucClient.Size = new System.Drawing.Size(701, 430);
            this.ucClient.TabIndex = 0;
            this.ucClient.Tag = "Client";
            // 
            // treeMain
            // 
            this.treeMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)));
            //			appearance1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
            //			this.treeMain.Appearance = appearance1;
            //			ultraTreeColumnSet1.LabelStyle = Infragistics.Win.UltraWinTree.NodeLayoutLabelStyle.Separate;
            //			this.treeMain.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
            //			this.treeMain.UseFlatMode = DefaultableBoolean.True;
            this.treeMain.HideSelection = false;
            this.treeMain.Location = new System.Drawing.Point(0, 0);
            this.treeMain.Name = "treeMain";
            //			//			ultraTreeNode1.Key = "c:-2-";
            //			//			ultraTreeNode2.Key = "g:-3-";
            //			//			ultraTreeNode2.Text = "Groups";
            //			//			ultraTreeNode1.AddRange(new System.Windows.Forms.NodeTree[] {
            //			//																								ultraTreeNode2});
            //			//			ultraTreeNode1.Text = "Clients";
            //			//			ultraTreeNode3.Key = "r:-1-";
            //			//			ultraTreeNode3.Text = "RL Templates";
            //			//			this.treeMain.AddRange(new System.Windows.Forms.NodeTree[] {
            //			//																							   ultraTreeNode1,
            //			//																							   ultraTreeNode3});
            //			_override1.AllowAutoDragExpand = Infragistics.Win.UltraWinTree.AllowAutoDragExpand.Never;
            //			_override1.AllowCopy = Infragistics.Win.DefaultableBoolean.False;
            //			_override1.AllowCut = Infragistics.Win.DefaultableBoolean.False;
            //			_override1.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            //			_override1.AllowPaste = Infragistics.Win.DefaultableBoolean.False;
            //			_override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Single;
            //			_override1.Sort = Infragistics.Win.UltraWinTree.SortType.Ascending;
            //			this.treeMain.Override = _override1;
            this.treeMain.SelectedImageIndex = -1;
            this.treeMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeMain.PathSeparator = ":";
            this.treeMain.Size = new System.Drawing.Size(152, 480);
            this.treeMain.TabIndex = 0;
            this.treeMain.Scrollable = true;
            //			(this.treeView1_AfterSelect);
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);

            //			this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
            // 
            // panelControls
            // 
            this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControls.Controls.Add(this.btnClose);
            this.panelControls.Controls.Add(this.btnSave);
            this.panelControls.Controls.Add(this.btnDelete);
            this.panelControls.Controls.Add(this.btnAdd);
            this.panelControls.Location = new System.Drawing.Point(0, 480);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(982, 30);
            this.panelControls.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            appearance2.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance2.ImageBackground")));
            this.btnClose.Appearance = appearance2;
            this.btnClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.Location = new System.Drawing.Point(516, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 3;
            this.btnClose.Click += new System.EventHandler(this.CloseAll);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            appearance3.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance3.ImageBackground")));
            this.btnSave.Appearance = appearance3;
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.Location = new System.Drawing.Point(440, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.TabIndex = 2;
            this.btnSave.Tag = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.Saving);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)));
            appearance4.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance4.ImageBackground")));
            this.btnDelete.Appearance = appearance4;
            this.btnDelete.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnDelete.Location = new System.Drawing.Point(78, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Click += new System.EventHandler(this.Delete);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)));
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance5.ImageBackground")));
            this.btnAdd.Appearance = appearance5;
            this.btnAdd.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnAdd.Location = new System.Drawing.Point(2, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Click += new System.EventHandler(this.Add);
            // 
            // tabctrlMain
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.FontData.BoldAsString = "True";
            this.tabctrlMain.ActiveTabAppearance = appearance6;
            this.tabctrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)/**| System.Windows.Forms.AnchorStyles.Bottom) 
				**/| System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tabctrlMain.Controls.Add(this.tabsharedpage);
            this.tabctrlMain.Controls.Add(this.tabpageRL);
            this.tabctrlMain.Controls.Add(this.tabpageClient);
            this.tabctrlMain.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tabctrlMain.Location = new System.Drawing.Point(0, 0);
            this.tabctrlMain.Name = "tabctrlMain";
            this.tabctrlMain.SharedControlsPage = this.tabsharedpage;
            this.tabctrlMain.Size = new System.Drawing.Size(705, 480);
            this.tabctrlMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabctrlMain.TabIndex = 3;
            ultraTab1.Key = "RLogic";
            ultraTab1.TabPage = this.tabpageRL;
            ultraTab1.Text = "RL";
            ultraTab2.Key = "Client";
            ultraTab2.TabPage = this.tabpageClient;
            ultraTab2.Text = "Client";
            this.tabctrlMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
                                                                                                  ultraTab1,
                                                                                                  ultraTab2});
            this.tabctrlMain.Tag = "tabctrlMain";
            this.tabctrlMain.ScrollButtons = Infragistics.Win.UltraWinTabs.TabScrollButtons.Automatic; ;
            this.tabctrlMain.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.TabSelect);
            // 
            // tabctrlRoutingLogic
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.FontData.BoldAsString = "True";
            this.tabctrlRoutingLogic.ActiveTabAppearance = appearance6;
            this.tabctrlRoutingLogic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tabctrlRoutingLogic.Controls.Add(this.tabsharedpageRL);
            this.tabctrlRoutingLogic.Controls.Add(this.tabpageRoutingLogic);
            //				this.tabctrlRoutingLogic.Controls.Add(this.tabpageRL);
            //				this.tabctrlRoutingLogic.Controls.Add(this.tabpageClient);
            this.tabctrlRoutingLogic.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tabctrlRoutingLogic.Location = new System.Drawing.Point(155, 0);
            this.tabctrlRoutingLogic.Name = "tabctrlRoutingLogic";
            this.tabctrlRoutingLogic.SharedControlsPage = this.tabsharedpageRL;
            this.tabctrlRoutingLogic.Size = new System.Drawing.Size(707, 480);
            this.tabctrlRoutingLogic.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabctrlRoutingLogic.TabIndex = 3;
            ultraTab3.Key = "Routing Logic";
            ultraTab3.Text = "Routing Logic";
            ultraTab3.TabPage = this.tabpageRoutingLogic;
            //				ultraTab1.Text = "RoutingLogic";
            //				ultraTab2.Key = "Client";
            //				ultraTab2.TabPage = this.tabpageClient;
            //				ultraTab2.Text = "Client";
            this.tabctrlRoutingLogic.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
                                                                                                          ultraTab3});
            this.tabctrlRoutingLogic.Tag = "tabctrlRoutingLogic";
            this.tabctrlRoutingLogic.ScrollButtons = Infragistics.Win.UltraWinTabs.TabScrollButtons.Automatic;
            //				this.tabctrlRoutingLogic.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.TabResize);
            // 
            // tabsharedpage
            // 
            this.tabsharedpage.Location = new System.Drawing.Point(-10000, -10000);
            this.tabsharedpage.Name = "tabsharedpage";
            this.tabsharedpage.Size = new System.Drawing.Size(701, 430);
            // 
            // tabsharedpageRL
            // 
            this.tabsharedpageRL.Location = new System.Drawing.Point(-10000, -10000);
            this.tabsharedpageRL.Name = "tabsharedpageRL";
            this.tabsharedpageRL.Size = new System.Drawing.Size(701, 430);
            // 
            // RoutingLogic
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ClientSize = new System.Drawing.Size(860, 508);
            this.AutoScroll = true;
            //			this.Controls.Add(this.tabctrlMain);
            this.Controls.Add(this.tabctrlRoutingLogic);
            this.Controls.Add(this.treeMain);
            this.Controls.Add(this.panelControls);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RoutingLogic";
            this.Text = "RoutingLogic";
            this.tabpageRL.ResumeLayout(false);
            this.tabpageClient.ResumeLayout(false);
            this.tabpageRoutingLogic.ResumeLayout(false);
            //			((System.ComponentModel.ISupportInitialize)(this.treeMain)).EndInit();
            this.panelControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlMain)).EndInit();
            this.tabctrlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlRoutingLogic)).EndInit();
            this.tabctrlRoutingLogic.ResumeLayout(false);
            this.ResumeLayout(false);
            this.AutoScroll = true;

        }
        #endregion


        #region Load  Data to controls
        private void LoadData(System.Windows.Forms.NodeTree _nodeMain, int _iCompanyID)
        {

            //			bool _bResult=false;
            //
            //			Forms.DialogBox dlgbx = new Forms.DialogBox(" Are You Sure You Want to Load RL ? ");
            //			
            //			//			this.Hide();
            //			dlgbx.ShowDialog();
            //			dlgbx.BringToFront();
            //			dlgbx.DesktopLocation =	new System.Drawing.Point(-100,50);
            //			
            //			_bResult= ( dlgbx.DialogResult == DialogResult.Yes) ? true : false ;
            //			//			this.Show();
            //
            //			dlgbx.Close();
            //			this.BringToFront();
            //			if(!_bResult)
            //			{
            //				return;
            //			}





            this.nodeMain = _nodeMain;
            ipkCompanyID = _iCompanyID;

            //			System.Windows.Forms.NodeTree treeNode1 = new System.Windows.Forms.NodeTree();
            //			System.Windows.Forms.NodeTree treeNode2 = new System.Windows.Forms.NodeTree();
            //			System.Windows.Forms.NodeTree treeNode3 = new System.Windows.Forms.NodeTree();
            System.Windows.Forms.NodeTree treeNode1 = new NodeTree();
            System.Windows.Forms.NodeTree treeNode2 = new NodeTree();
            System.Windows.Forms.NodeTree treeNode3 = new NodeTree();



            nodeMain.Tag = "RL";
            nodeMain.Text = "RLogic";

            treeNode1.Tag = "c:-2";
            treeNode2.Tag = "g:-3";
            treeNode2.Text = "-Groups-";
            treeNode1.Nodes.AddRange(new System.Windows.Forms.NodeTree[] { treeNode2 });
            treeNode1.Text = "-Clients-";
            treeNode3.Tag = "r:-1";
            treeNode3.Text = "-RL Templates-";
            nodeMain.Nodes.AddRange(new System.Windows.Forms.NodeTree[] { treeNode3, treeNode1 });
            //			string[] h = "g:-3:".Split(':');
            this.treeMain.Nodes.Add(nodeMain);
            this.nodeMain.ExpandAll();
            LoadDataToDataSet();
            LoadTree();
            //        Data.DataHandelingManager.FillDefaultTable( ref dsData);

            //			MessageBox.
        }
        #endregion

        #region LoadDataToDataSet
        private void LoadDataToDataSet()
        {

            bool _bDataLoaded = false;

            _bDataLoaded = this.DataCall();
            bool _bReload = false;

            if (!_bDataLoaded)
            {

                Forms.DialogBox dlgbx = new Forms.DialogBox(" Unable to load data. Do you want to retry ? ");

                this.Hide();

                dlgbx.ShowDialog();
                _bReload = (dlgbx.DialogResult == DialogResult.Yes) ? true : false;
                dlgbx.Close();

                this.Show();

                if (_bReload)
                {
                    _bDataLoaded = this.DataCall();
                }


            }

            if (!_bDataLoaded)
            {
                if (_bReload)
                {
                    MessageBox.Show(" Still not able to load data. Quiting ! ");
                }
                this.Close();
            }

        }
        #endregion

        #region Datahandelingcalls
        private bool DataCall()
        {
            //try
            {
                //				System.Data.SqlClient.SqlParameter[][] _sqlParam= new System.Data.SqlClient.SqlParameter[8][];
                //				for(int i=0;i<8;i++)
                //				{
                //					_sqlParam[i] =  new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@CompanyID",ipkCompanyID)};
                //				}
                //				//dsData.Tables.Add(
                //				this.dtTree=BLL.DataHandelingManager.DataStoredProcedure("P_GetRLTree", null);
                //				this.dtTree.TableName="dtTree";
                //				this.dsData.Tables.Add(dtTree);
                //
                //				this.dtAUEC=BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllAUECAssetUnderLyingExchange", _sqlParam[0]);
                //				this.dtAUEC.TableName="dtAUEC";
                //				this.dsData.Tables.Add(dtAUEC);
                //
                //				this.dtParameters =BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllParameters", _sqlParam[1]);
                //				this.dtParameters.TableName="dtParameters";
                //				this.dsData.Tables.Add(dtParameters);
                //
                //				this.dtCounterPartyVenue=BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllCounterPartyVenues", _sqlParam[2]);
                //				this.dtCounterPartyVenue.TableName="dtCounterPartyVenue";
                //				this.dsData.Tables.Add(dtCounterPartyVenue);
                //
                //				this.dtTradingAccount=BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllTradingAccount", _sqlParam[3]);
                //				this.dtTradingAccount.TableName="dtTradingAccount";
                //				this.dsData.Tables.Add(dtTradingAccount);
                //				//dsData.Tables["dtTree]=;

                BLL.DataCallFunctionsManager.LoadDataTables(ipkCompanyID, out dtTree, out dtAUEC, out dtParameters, out dtCounterPartyVenue, out dtTradingAccount);

                if (this.dsData.Tables.Contains("dtTree"))
                {
                    this.dsData.Tables.Remove("dtTree");
                }
                this.dsData.Tables.Add(dtTree);
                if (this.dsData.Tables.Contains("dtAUEC"))
                {
                    this.dsData.Tables.Remove("dtAUEC");
                }
                this.dsData.Tables.Add(dtAUEC);
                if (this.dsData.Tables.Contains("dtParameters"))
                {
                    this.dsData.Tables.Remove("dtParameters");
                }
                this.dsData.Tables.Add(dtParameters);
                if (this.dsData.Tables.Contains("dtCounterPartyVenue"))
                {
                    this.dsData.Tables.Remove("dtCounterPartyVenue");
                }
                this.dsData.Tables.Add(dtCounterPartyVenue);
                if (this.dsData.Tables.Contains("dtTradingAccount"))
                {
                    this.dsData.Tables.Remove("dtTradingAccount");
                }
                this.dsData.Tables.Add(dtTradingAccount);

                return true;
            }
            //			catch(Exception ex)
            //			{
            //				string s = ex.Message + ex.Source + ex.StackTrace;
            //				return false;
            //			}


        }

        #endregion


        #region Load data to Tree

        private void LoadTree()
        {

            //			this.nodeMain.Clear();
            //
            //			System.Windows.Forms.NodeTree ultraTreeNode1 = new System.Windows.Forms.NodeTree();
            //			System.Windows.Forms.NodeTree ultraTreeNode2 = new System.Windows.Forms.NodeTree();
            //			System.Windows.Forms.NodeTree ultraTreeNode3 = new System.Windows.Forms.NodeTree();
            //			this.nodeMain.Name = "nodeMain";
            //			ultraTreeNode1.Key = "r:-1";
            //			ultraTreeNode1.Text = "RL";
            //			ultraTreeNode2.Key = "c:-2";
            //			ultraTreeNode3.Key = "g:-3";
            //			ultraTreeNode3.Text = "Groups";
            //			ultraTreeNode2.AddRange(new System.Windows.Forms.NodeTree[] {
            //																								ultraTreeNode3});
            //			ultraTreeNode2.Text = "Client";
            //			this.nodeMain.AddRange(new System.Windows.Forms.NodeTree[] {
            //																							   ultraTreeNode1,
            //																							   ultraTreeNode2});
            //			this.nodeMain.PathSeparator = ":";
            const char _cPrefixRL = 'r';
            const char _cPrefixClient = 'c';
            const char _cPrefixGroup = 'g';
            const string _strKeyHeadingRL = "r:-1";
            const string _strKeyHeadingClient = "c:-2";
            const string _strKeyHeadingGroup = "g:-3";

            //			System.Windows.Forms.NodeTree _nodeRL=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.GetItem(this.nodeMain.IndexOf(_strKeyHeadingRL)));
            //			System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.GetItem(this.nodeMain.IndexOf(_strKeyHeadingClient)));
            //			System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.GetItem(_nodeClient.IndexOf(_strKeyHeadingGroup)));
            //			//System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.All[1]);
            //			//System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.All[0]);
            //			System.Windows.Forms.NodeTree _node;

            //			System.Windows.Forms.NodeTree _nodeRL=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.GetItem(this.nodeMain.IndexOf(_strKeyHeadingRL)));
            //			System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.GetItem(this.nodeMain.IndexOf(_strKeyHeadingClient)));
            //			System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.GetItem(_nodeClient.IndexOf(_strKeyHeadingGroup)));
            //System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.All[1]);
            //System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.All[0]);
            System.Windows.Forms.NodeTree _node;
            System.Windows.Forms.NodeTree _nodeRL = this.nodeMain[_strKeyHeadingRL];
            System.Windows.Forms.NodeTree _nodeClient = this.nodeMain[_strKeyHeadingClient];
            System.Windows.Forms.NodeTree _nodeClientGrp = _nodeClient[_strKeyHeadingGroup];




            foreach (System.Data.DataRow _row in dsData.Tables["dtTree"].Rows)
            {
                switch ((_row["KeyID"].ToString().ToCharArray()[0]))
                {
                    case _cPrefixRL:
                        _node = _nodeRL;
                        break;
                    case _cPrefixClient:
                        _node = _nodeClient;
                        break;
                    case _cPrefixGroup:
                        _node = _nodeClientGrp;
                        break;
                    default:
                        _node = _nodeRL;
                        break;
                }

                _node.Add(_row["KeyID"].ToString().Trim(), _row["NodeName"].ToString());
            }


            //			//  Rl loading to tree
            //			System.Data.DataRow[] _rowarrayFoundRows=this.dsData.Tables["T_RoutingLogic"].Select("Name<>''","Name");
            //			System.Windows.Forms.NodeTree _nodeRL=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.All[0]);
            //			foreach( System.Data.DataRow _row in _rowarrayFoundRows)
            //			{
            //				_nodeRL.Add("r:"+_row["RLID"].ToString(),_row["Name"].ToString());
            //			}
            //
            //
            //
            //			//  Client loading to tree
            //	
            //			_rowarrayFoundRows=null;
            //			_rowarrayFoundRows=this.dsData.Tables["T_RoutingLogicCompanyClient"].Select();
            //			System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.All[1]);
            //			bool _bInGroup = false ;
            //			string _strClientName="";
            //			foreach(System.Data.DataRow _row in _rowarrayFoundRows)
            //			{
            //				_bInGroup =  this.dsData.Tables["T_RoutingLogicGroupClient"].Columns["CompanyClientID_FK"].ExtendedProperties.ContainsValue(_row["CompanyClientID_FK"]) ;
            //				if(!_bInGroup)
            //				{
            //					_strClientName=(this.dsData.Tables["T_CompanyClient"].Select("CompanyClientID = " +_row["CompanyClientID_FK"].ToString() )[0])["ClientName"].ToString();
            //					_nodeClient.Add("c:"+_row["CompanyClientID_FK"].ToString(),_strClientName);
            //				}
            //			}
            //
            //
            //			//  client-Groups loading to tree
            //			_rowarrayFoundRows=null;
            //			_rowarrayFoundRows=this.dsData.Tables["T_RoutingLogicClientGroup"].Select();
            //			System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.All[0]);
            //			foreach( System.Data.DataRow _row in _rowarrayFoundRows)
            //			{
            //				_nodeClientGrp.Add("g:"+_row["ClientGroupID"].ToString(),_row["Name"].ToString());
            //			}

        }

        #endregion



        #region Loading Selections
        private void LoadSelection(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            const string _cPrefixRL = "r";
            const string _cPrefixClient = "c";
            const string _cPrefixGroup = "g";
            const string _strKeyHeadingRL = "r:-1";
            const string _strKeyHeadingClient = "c:-2";
            const string _strKeyHeadingGroup = "g:-3";


            System.Windows.Forms.NodeTree _nodeRL = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingRL]);
            System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingClient]);
            System.Windows.Forms.NodeTree _nodeClientGrp = (System.Windows.Forms.NodeTree)(_nodeClient[_strKeyHeadingGroup]);


            //			const string _strMemoryIDRL = "RL";


            if (Functions.IsNull(this.Tree.SelectedNode))
            {
                return;
            }

            if (((System.Windows.Forms.NodeTree)(this.Tree.SelectedNode)).Key.ToString().Split(':').Length < 2)
            {

                //				if(tabctrlMain.ActiveTab == tabctrlMain.Tabs["RLogic"])
                //				{
                //
                //				}
                //				if(tab

                return;
            }


            //string[] _sSelectedPath = e.NewSelections[0].FullPath.Split(':') ;

            //Infragistics.Win.UltraWinTree.UltraTree _treeMain = (Infragistics.Win.UltraWinTree.UltraTree) sender;
            //string _sSelectedKey = ((Infragistics.Shared.SubObjectsCollectionBase)(((Infragistics.Win.UltraWinTree.SelectedNodesCollection)(e.NewSelections)))).All[0].Key;
            System.Windows.Forms.NodeTree _nodeToAdd = ((System.Windows.Forms.NodeTree)(this.Tree.SelectedNode));
            string _sSelectedKey = _nodeToAdd.Key;


            //_sqlCommand.Parameters.Add("@CategoryID", SqlDbType.Int).Value = 1;


            //			bool _bSelectedHeading = !(_sSelectedKey.StartsWith(_cPrefixRL) ||  _sSelectedKey.StartsWith(_cPrefixClient) ||  _sSelectedKey.StartsWith(_cPrefixGroup)) ;
            //
            //			if(_bSelectedHeading)
            //			{
            //				
            //	
            //			}


            //			int _iValue = Convert.ToInt32(_sSelectedKey.Substring((_cPrefixRL+":").Length));
            string[] _strKeySelectedArray = _sSelectedKey.Split(':');
            int _iValue = Convert.ToInt32(_strKeySelectedArray[1]);


            //		bool _bIsRL = _sSelectedKey.StartsWith(_cPrefixRL);//_sSelectedPath[0].StartsWith("R");

            // to add here the dynamic conterol to identiy the conrol added 
            //if any and  call their close function so that any changes are 
            //not lost w/o warning
            // close control would have update flag to help it.

            //			FillMemoryRecord(_sSelectedKey);

            //			System.Data.DataTable _dtMemoryRL;
            //
            //			if(this.dsData.Tables.Contains("dtMemoryRL"))
            //			{
            //				SaveToDBMemory();
            //			}


            //
            //			System.Data.SqlClient.SqlParameter[] _sqlParam ;//= new System.Data.SqlClient.SqlParameter[];
            //			DataColumn[] _dcPrimaryKey ;//= new DataColumn[];

            switch (_strKeySelectedArray[0])
            {
                case _cPrefixRL:
                    //					this.WindowState= FormWindowState.Normal;
                    //					this.tabctrlMain.Size = new System.Drawing.Size(846, 267);
                    this.strTabID = strTabIDRL;//"RL";

                    //					_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@RLID",_iValue), new System.Data.SqlClient.SqlParameter("@MemoryID",_strMemoryIDRL)};
                    //					this.dtMemoryRL= BLL.DataHandelingManager.DataStoredProcedure("P_GetRL",_sqlParam);
                    //					this.dtMemoryRL.TableName="dtMemoryRL";
                    //					
                    //					_dcPrimaryKey=  new DataColumn[] {this.dtMemoryRL.Columns["MemoryID"]};
                    //					this.dtMemoryRL.PrimaryKey = _dcPrimaryKey;
                    //					BLL.DataCallFunctionsManager.LoadRL(_strMemoryIDRL,  _iValue, out this.dtMemoryRL);

                    dataRL.RoutingPathCount(strTabID, this.ucRLogic.NumberOfRoutingPath);

                    for (int i = 0; i < dataRL.RoutingPathCount(strTabID); i++)
                    {
                        BLL.DataCallFunctionsManager.LoadRL(strTabID, _iValue, i);
                    }

                    //					if(this.dsData.Tables.Contains("dtMemoryRL"))
                    //					{
                    //						this.dsData.Tables["dtMemoryRL"].Dispose();
                    //						this.dsData.Tables.Remove("dtMemoryRL");
                    //					}
                    //					this.dsData.Tables.Add(dtMemoryRL) ;

                    //					this.tabctrlMain.ActiveTab = this.tabctrlMain.Tabs["RLogic"];
                    //					this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["RLogic"];
                    //					this.tabpageRL.Select();
                    //					this.tabpageRL.Focus();

                    this.ucRLogic.LoadData(ref dsData, ref dataRL, ref nodeMain);
                    this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["RLogic"];
                    break;



                case _cPrefixClient:

                    if (_iValue < 0)
                    {
                        return;
                    }

                    //					this.WindowState= FormWindowState.Maximized;
                    //					this.tabctrlMain.Size = new System.Drawing.Size(846, 660);
                    this.strTabID = strTabIDClient;//"client";
                    if (!_sSelectedKey.Equals(_cPrefixClient + ":" + this.dataRL.ID(strTabID).ToString() + ":" + this.dataRL.AUECID(strTabID).ToString()))
                    {
                        //rl list

                        foreach (System.Windows.Forms.NodeTree _nodeChildToClear in _nodeClientGrp.Nodes)
                        {
                            _nodeChildToClear.Clear();

                        }

                        LoadRLListData();
                        //mem   grp client 
                        LoadMemoryGroupClient(_iValue, Functions.MinValue, Convert.ToInt32(_strKeySelectedArray["c:1:2".Split(':').Length - 1]));

                        BLL.DataCallFunctionsManager.LoadClientList(ipkCompanyID, Functions.MinValue, out this.dtClientList);

                        //					_dcPrimaryKey= new DataColumn[] {this.dtClientList.Columns["ClientID"], this.dtClientList.Columns["AUECID"]};
                        //					this.dtClientList.PrimaryKey = _dcPrimaryKey;

                        if (this.dsData.Tables.Contains("dtClientList"))
                        {
                            this.dsData.Tables["dtClientList"].Dispose();
                            this.dsData.Tables.Remove("dtClientList");
                        }
                        int _iTempChecked = 1;
                        int _iTempApplyRL = 1;

                        object[] _objCurrentClient = new object[] { this.dataRL.ID(strTabID), this.dataRL.Name(strTabID), this.dataRL.AUECID(strTabID), _iTempApplyRL, _iTempChecked };
                        this.dtClientList.Rows.Add(_objCurrentClient);

                        this.dsData.Tables.Add(dtClientList);
                    }
                    //					_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@ClientID",_iValue), new System.Data.SqlClient.SqlParameter("@GroupID",Functions.MinValue) };
                    //					this.dtMemoryGroupClient = BLL.DataHandelingManager.DataStoredProcedure("P_GetRLMemoryGroupClient",_sqlParam);
                    //					this.dtMemoryGroupClient.TableName="dtMemoryGroupClient";
                    //					
                    //					_dcPrimaryKey= new DataColumn[] {this.dtMemoryGroupClient.Columns["ID"]};
                    //					this.dtMemoryGroupClient.PrimaryKey = _dcPrimaryKey;
                    //
                    //					if(this.dsData.Tables.Contains("dtMemoryGroupClient"))
                    //					{
                    //						this.dsData.Tables["dtMemoryGroupClient"].Dispose();
                    //						this.dsData.Tables.Remove("dtMemoryGroupClient");
                    //					}
                    //					
                    //					this.dsData.Tables.Add(dtMemoryGroupClient);
                    //


                    //chging tabs

                    //					this.tabctrlMain.ActiveTab = this.tabctrlMain.Tabs["Client"];
                    //					this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["Client"];
                    //					this.tabpageClient.Select();
                    //					this.tabpageClient.Focus();

                    this.ucClient.Tag = _sSelectedKey;
                    this.ucClient.SelectedGroupClientID = Functions.MinValue;
                    this.ucClient.LoadData(ref dsData, ref dataRL, ref strTabID, ref nodeMain);
                    this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["Client"];
                    break;



                case _cPrefixGroup:
                    //					this.WindowState= FormWindowState.Maximized;
                    //					this.tabctrlMain.Size = new System.Drawing.Size(846, 660);
                    this.strTabID = strTabIDGroup;//"group";


                    if (this.dataRL.ID(strTabID) != _iValue)
                    {
                        foreach (System.Windows.Forms.NodeTree _nodeChildToClear in _nodeClientGrp.Nodes)
                        {
                            _nodeChildToClear.Clear();

                        }



                        LoadRLListData();
                        //mem   grp client  ----------

                        LoadMemoryGroupClient(Functions.MinValue, _iValue, Functions.MinValue);
                        //  client list

                        //					_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@CompanyID",ipkCompanyID), new System.Data.SqlClient.SqlParameter("@GroupID",_iValue) };
                        //					this.dtClientList = BLL.DataHandelingManager.DataStoredProcedure("P_GetRLClientList",_sqlParam);
                        //					this.dtClientList.TableName="dtClientList";

                        BLL.DataCallFunctionsManager.LoadClientList(ipkCompanyID, _iValue, out this.dtClientList);

                        //					_dcPrimaryKey= new DataColumn[] {this.dtClientList.Columns["ClientID"], this.dtClientList.Columns["AUECID"]};
                        //					this.dtClientList.PrimaryKey = _dcPrimaryKey;

                        if (this.dsData.Tables.Contains("dtClientList"))
                        {
                            this.dsData.Tables["dtClientList"].Dispose();
                            this.dsData.Tables.Remove("dtClientList");
                        }

                        this.dsData.Tables.Add(dtClientList);

                    }
                    //chging tabs

                    //					this.tabctrlMain.ActiveTab = this.tabctrlMain.Tabs["Client"];
                    //					this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["Client"];
                    //					this.tabpageClient.Select();
                    //					this.tabpageClient.Focus();

                    this.ucClient.Tag = _sSelectedKey;

                    if (_strKeySelectedArray.Length < 3)
                    {
                        if (Convert.ToInt32(_strKeySelectedArray[1]) >= 0)
                        {
                            this.LoadGroupClient(_nodeToAdd);
                        }
                        this.ucClient.SelectedGroupClientID = Functions.MinValue;
                        //						this.ucClient.LoadData(ref dsData, ref dataRL, ref strTabID);
                    }
                    else
                    {
                        //string _strClientNameSelected = (this.dsData.Tables["dtClientList"].Select(" ClientID = " + _strKeySelectedArray[2] ) )[0]["ClientName"].ToString() ;
                        this.ucClient.SelectedGroupClientID = Convert.ToInt32(_strKeySelectedArray[2]);
                        //						this.ucClient.LoadData(ref dsData, ref dataRL, ref strTabID);//, Convert.ToInt32(_strKeySelectedArray[2]));//_strClientNameSelected );

                    }
                    this.ucClient.LoadData(ref dsData, ref dataRL, ref strTabID, ref nodeMain);
                    this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["Client"];



                    break;

                default:

                    break;
            }


            //			if( _bIsRL )
            //			{
            //				
            ////				int _iValue = 1;// (_sSelectedKey.Remove(1,2).StartsWith("-"))? ((-1)*(int.Parse(_sSelectedKey.Remove(1,3)))): ( int.Parse(_sSelectedKey.Remove(1,2)));
            //				//FillMemoryRecord(_sSelectedPath);
            //				System.Data.SqlClient.SqlParameter _sqlParam = new System.Data.SqlClient.SqlParameter("@RLID",_iValue);
            //				_dtMemoryRL = Data.DataHandelingManager.DataStoredProcedure("P_GetRL",_sqlParam);
            //
            //				this.tabctrlMain.ActiveTab = this.tabctrlMain.Tabs["RLogic"];
            //				this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["RLogic"];
            //				
            //				this.ucRLogic.LoadData(ref _dtMemoryRL);				
            //				
            //			}
            //			else
            //			{
            //				//FillMemoryRecord(_sSelectedPath);
            //				this.tabctrlMain.ActiveTab = this.tabctrlMain.Tabs["Client"];
            //				this.tabctrlMain.SelectedTab = this.tabctrlMain.Tabs["Client"];
            //
            //				this.ucClient.LoadData(ref _dtMemoryRL);
            //
            //			}
        }

        private void LoadGroupClient(System.Windows.Forms.NodeTree _nodeToAdd)
        {
            //			const char _cPrefixRL = 'r';
            //			const char _cPrefixClient = 'c';
            const char _cPrefixGroup = 'g';
            //			const string _strKeyHeadingRL = "r:-1";
            //			const string _strKeyHeadingClient = "c:-2";
            //			const string _strKeyHeadingGroup = "g:-3";

            if (Functions.IsNull(this.dsData) || Functions.IsNull(this.dataRL) || !this.dsData.Tables.Contains("dtClientList"))
            {
                return;
            }

            string _strSelectedKey = ((System.Windows.Forms.NodeTree)(this.Tree.SelectedNode)).Key.ToString();


            string _strKeyToAdd = _nodeToAdd.Key.ToString();

            //			_nodeToAdd.a
            string _strKeyClientPrefix = _cPrefixGroup + ":" + ((_strKeyToAdd.Split(':'))[1]) + ":";
            string _strKeyClient = "";

            _nodeToAdd.Clear();
            System.Windows.Forms.NodeTree _nodeNew;

            foreach (System.Data.DataRow _row in this.dsData.Tables["dtClientList"].Select("AUECID = " + this.dataRL.AUECID(strTabID).ToString() + " AND Checked = 1 "))
            {
                _strKeyClient = _strKeyClientPrefix + _row["ClientID"].ToString();
                _nodeNew = new NodeTree(_strKeyClient, _row["ClientName"].ToString());
                if (!_nodeToAdd.Contains(_strKeyClient))
                {
                    _nodeToAdd.Add(_nodeNew);
                }

                //				this.chklstClient.Items.Add(_row["ClientName"].ToString(),((int.Parse(_row["ApplyRL"].ToString()))==1?true:false));						
            }

            _nodeToAdd.Expanded = true;

            if (_strSelectedKey.Split(':').Length > 2)
            {

                this.Tree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                string _strSelectedID = _strSelectedKey.Split(':')[2];
                string _strSelectedName = this.dsData.Tables["dtClientList"].Select("ClientID = " + _strSelectedID)[0]["ClientName"].ToString();
                _strKeyClient = _strKeyClientPrefix + _strSelectedID;
                _nodeNew = new NodeTree(_strKeyClient, _strSelectedName);
                if (_nodeToAdd.Contains(_nodeNew))
                {
                    _nodeToAdd[_strKeyClient].Selected = true;
                }
                else
                {
                    _nodeToAdd.Selected = true;
                }

                this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
            }
        }



        private void LoadRLListData()
        {

            //  load RL list


            //			DataColumn[] _dcPrimaryKey = new DataColumn[1];
            //
            //			this.dtRLList = BLL.DataHandelingManager.DataStoredProcedure("P_GetRLList",null);
            //			this.dtRLList.TableName="dtRLList";
            //					
            //			_dcPrimaryKey[0]=this.dtRLList.Columns["RLID"];
            //			this.dtRLList.PrimaryKey = _dcPrimaryKey;

            BLL.DataCallFunctionsManager.LoadRLList(out this.dtRLList);

            if (this.dsData.Tables.Contains("dtRLList"))
            {
                this.dsData.Tables["dtRLList"].Dispose();
                this.dsData.Tables.Remove("dtRLList");
            }

            this.dsData.Tables.Add(this.dtRLList);


        }


        private void LoadMemoryGroupClient(int _iClientID, int _iGroupID, int _iAUECID)
        {
            //			System.Data.SqlClient.SqlParameter[] _sqlParam ;//= new System.Data.SqlClient.SqlParameter[];
            //			DataColumn[] _dcPrimaryKey ;
            //
            //			_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@ClientID",_iClientID), new System.Data.SqlClient.SqlParameter("@GroupID",_iGroupID) };
            //			this.dtMemoryGroupClient = BLL.DataHandelingManager.DataStoredProcedure("P_GetRLMemoryGroupClient",_sqlParam);
            //			this.dtMemoryGroupClient.TableName="dtMemoryGroupClient";
            //					
            //			_dcPrimaryKey= new DataColumn[] {this.dtMemoryGroupClient.Columns["ID"]};
            //			this.dtMemoryGroupClient.PrimaryKey = _dcPrimaryKey;

            //			BLL.DataCallFunctionsManager.LoadGroupClient(_iClientID,_iGroupID, out this.dtMemoryGroupClient);
            //
            //			if(this.dsData.Tables.Contains("dtMemoryGroupClient"))
            //			{
            //				this.dsData.Tables["dtMemoryGroupClient"].Dispose();
            //				this.dsData.Tables.Remove("dtMemoryGroupClient");
            //			}
            //					
            //			this.dsData.Tables.Add(this.dtMemoryGroupClient);





            BLL.DataCallFunctionsManager.LoadGroupClient(_iClientID, _iGroupID, strTabID, _iAUECID);




        }



        //		public void SaveToDBMemory()
        //		{
        //			SaveMemoryDataCall("RL");
        //			SaveMemoryDataCall("RL0");
        //			SaveMemoryDataCall("RL1");
        //			SaveMemoryDataCall("RL2");		
        //		}


        //		private void SaveMemoryDataCall(string _strMemoryID )
        //		{
        ////			object[] parameter = new object[this.dsData.Tables["dtMemoryRL"].Columns.Count];
        ////			System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[this.dsData.Tables["dtMemoryRL"].Columns.Count];
        ////				
        ////			System.Data.DataRow _row = this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryID);
        ////				
        ////			string _strValue="" ;
        ////			int _iValue=0;
        ////
        ////			for(int i=0;i<this.dsData.Tables["dtMemoryRL"].Columns.Count; i++)
        ////			{
        ////				if(_row[i].GetType().Equals( _strValue.GetType()))
        ////				{
        ////					_strValue = (IsNull(_row[i]))?"":_row[i].ToString();
        ////					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_strValue);
        ////				}
        ////				else if(_row[i].GetType().Equals( _iValue.GetType()))
        ////				{
        ////
        ////					_iValue = (IsNull(_row[i]))?(Functions.MinValue):Convert.ToInt32(_row[i].ToString());
        ////					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_iValue);
        ////				}
        ////				else   // null  system.dbnull
        ////				{
        ////					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),Functions.MinValue);
        ////				}
        ////			}
        ////
        ////			//actula saving call
        ////			BLL.DataHandelingManager.DataStoredProcedure("P_SaveRLMemory",_sqlParam);
        //
        //			BLL.DataCallFunctionsManager.SaveMemory(_strMemoryID );
        //
        //		}
        #endregion




        #region tab selestion event    //Resizing on Client/RL selection
        private void TabSelect(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //			if(Functions.IsNull(this.strTabID))
            //			{
            //				return;
            //			}
            //			bool _bClientActive=this.tabpageClient.TabControl.ContainsFocus;
            const string _cPrefixRL = "r";
            const string _cPrefixClient = "c";
            const string _cPrefixGroup = "g";
            const string _strKeyHeadingRL = "r:-1";
            const string _strKeyHeadingClient = "c:-2";
            const string _strKeyHeadingGroup = "g:-3";
            //

            System.Windows.Forms.NodeTree _nodeRL = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingRL]);
            System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingClient]);
            System.Windows.Forms.NodeTree _nodeClientGrp = (System.Windows.Forms.NodeTree)(_nodeClient[_strKeyHeadingGroup]);
            //
            //			System.Windows.Forms.NodeTree _nodeRL=(System.Windows.Forms.NodeTree)(this.treeMain.Nodes.GetItem(this.treeMain.IndexOf(_strKeyHeadingRL)));
            //			System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.treeMain.Nodes.GetItem(this.treeMain.IndexOf(_strKeyHeadingClient)));
            //			System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.GetItem(_nodeClient.IndexOf(_strKeyHeadingGroup)));
            //			//			
            bool _bSelected = this.tabctrlMain.SelectedTab.Equals(this.tabctrlMain.Tabs["Client"]);
            string _strKey = "";

            if (_bSelected)
            {
                //				this.WindowState= FormWindowState.Maximized;
                //				this.tabctrlMain.Size = new System.Drawing.Size(846, 660);
                if (!Functions.IsNull(this.strTabID) && this.strTabID.Equals(strTabIDRL))
                {
                    //					foreach(System.Windows.Forms.NodeTree _node in this.treeMain.SelectedNode)
                    //					{
                    //						_node.Selected=false;
                    //					}
                    this.Tree.SelectedNode = null;
                }
                this.strTabID = this.ucClient.TabID;

                this.Tree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);



                if (this.strTabID.Equals(strTabIDClient))
                {
                    if (this.dataRL.ID(strTabID) < 0)
                    {

                        _strKey = _strKeyHeadingClient;
                        nodeMain[_strKey].Selected = true;
                    }
                    else
                    {
                        _strKey = _cPrefixClient + ":" + this.dataRL.ID(strTabID).ToString() + ":" + this.dataRL.AUECID(strTabID);
                        if (_nodeClient.Contains(_strKey))
                        {
                            _nodeClient[_strKey].Selected = true;
                        }
                        else
                        {
                            _nodeClient.Selected = true;
                        }
                    }

                    //					nodeMain[_strKey].Selected=true;
                    //						_nodeRL[_strKey].Selected=true;
                }
                else
                {
                    if (this.dataRL.ID(strTabID) < 0)
                    {

                        //						_strKey = _strKeyHeadingGroup ;
                        //						_nodeClient[_strKey].Selected=true;
                        _nodeClient.Selected = true;
                    }
                    else
                    {
                        _strKey = _cPrefixGroup + ":" + this.dataRL.ID(strTabID).ToString();
                        if (_nodeClientGrp.Contains(_strKey))
                        {
                            _nodeClientGrp[_strKey].Selected = true;
                        }
                        else
                        {
                            _nodeClientGrp.Selected = true;
                        }
                    }

                }

                this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);

                this.ucClient.SelectedGroupClientID = Functions.MinValue;
                return;
            }

            //			_bSelected=this.tabpageRL.TabControl.ContainsFocus;
            _bSelected = this.tabctrlMain.SelectedTab.Equals(this.tabctrlMain.Tabs["RLogic"]);
            if (_bSelected)
            {
                //				this.WindowState= FormWindowState.Normal;
                //				this.tabctrlMain.Size = new System.Drawing.Size(846, 260);
                if (!Functions.IsNull(this.strTabID) && !this.strTabID.Equals(strTabIDRL))
                {
                    //					foreach(System.Windows.Forms.NodeTree _node in this.treeMain.SelectedNodes)
                    //					{
                    //						_node.Selected=false;
                    //					}
                    this.Tree.SelectedNode = null;
                }
                this.strTabID = strTabIDRL;


                this.Tree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                if (this.dataRL.ID(strTabID) < 0)
                {

                    _strKey = _strKeyHeadingRL;
                    nodeMain[_strKey].Selected = true;
                }
                else
                {
                    _strKey = _cPrefixRL + ":" + this.dataRL.ID(strTabID).ToString();
                    if (_nodeRL.Contains(_strKey))
                    {
                        _nodeRL[_strKey].Selected = true;
                    }
                    else
                    {
                        _nodeRL.Selected = true;
                    }
                }
                this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                return;
            }

        }


        //		private void TabResize(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangedEventArgs e)
        //		{
        //			bool _bClientActive=this.tabpageClient.TabControl.ContainsFocus;
        //			if(_bClientActive)
        //			{
        //				this.WindowState= FormWindowState.Maximized;
        //				return;
        //			}
        //			
        //			_bClientActive=this.tabpageRL.TabControl.ContainsFocus;
        //			if(_bClientActive)
        //			{
        //				this.WindowState= FormWindowState.Normal;
        //				return;
        //			}
        //		
        //		}
        #endregion

        //		#region companay id
        //		private void GetCompanyID()
        //		{
        //			ipkCompanyID =20;
        //		}
        //		#endregion



        #region Closing
        private void CloseAll(object sender, System.EventArgs e)
        {

            //			bool _bResult=false;
            //
            //			DialogBox dlgbx = new DialogBox(" Are You Sure You Want to Close ? ");
            //			
            //			this.Hide();
            //			dlgbx.ShowDialog();
            //			
            //			_bResult= ( dlgbx.DialogResult == DialogResult.Yes) ? true : false ;
            //			this.Show();
            //
            //			dlgbx.Close();
            //			if(_bResult)
            //			{
            this.Close();
            //			}

        }
        #endregion

        #region Saving

        private void Saving(object sender, System.EventArgs e)
        {

            if (Functions.IsNull(this.dataRL) || Functions.IsNull(this.dsData) || this.dsData.Tables.Count <= 0)
            {
                MessageBox.Show(" Nothing To Save ");
                return;
            }
            //			const string _cPrefixRL = "r";
            //			const string _cPrefixClient = "c";
            //			const string _cPrefixGroup = "g";
            const string _strKeyHeadingRL = "r:-1";
            const string _strKeyHeadingClient = "c:-2";
            const string _strKeyHeadingGroup = "g:-3";
            //			const string strTabIDRL="RL";
            //			const string strTabIDGroup="group";
            //			const string strTabIDClient="client";
            //			string _strTabID;

            System.Windows.Forms.NodeTree _nodeRL = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingRL]);
            System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingClient]);
            System.Windows.Forms.NodeTree _nodeClientGrp = (System.Windows.Forms.NodeTree)(_nodeClient[_strKeyHeadingGroup]);


            if (tabctrlMain.ActiveTab == tabctrlMain.Tabs["RLogic"])
            {


                // verification
                string _strIncompleteMessage = "";
                if (!VerificationRLComplete(strTabIDRL, out _strIncompleteMessage))
                {
                    MessageBox.Show("RL Form Incomplete for : " + _strIncompleteMessage);
                    return;
                }
                string _strKey = this.ucRLogic.SaveData();
                if (Convert.ToInt32(_strKey.Split(':')[1]) < 0)
                {
                    return;
                }

                this.Tree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                _nodeRL[_strKey].Selected = true;
                this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);


                //modifying tree
                //				int _iIDRetunedFromSave = this.ucRLogic.SaveData();
                //				if(_iIDRetunedFromSave <0)
                //				{
                //					return;
                //				}
                //				string _strKey = _cPrefixRL+":"+    _iIDRetunedFromSave.ToString();
                //				string _strName = this.dataRL.Name(strTabIDRL);//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString();
                //			
                //				int _iIndex = _nodeRL.IndexOf(_strKey);
                //				if(_iIndex>=0)
                //				{					
                //					_nodeRL.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeRL.Nodes.GetItem(_iIndex));
                //					_nodeRL.Nodes.Insert(_iIndex, _strKey, _strName);						
                //				}
                //				else
                //				{
                //					if(this.dataRL.ID(strTabIDRL)>=0)//! IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"]))
                //					{
                //						//remving older one
                //						
                //						string _strKeyOld = _cPrefixRL+":"+this.dataRL.ID(strTabIDRL).ToString();//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"].ToString();
                //						int _iIndexOld = _nodeRL.IndexOf(_strKeyOld);
                //						if(_iIndexOld >= 0)
                //						{
                //							_nodeRL.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeRL.Nodes.GetItem(_iIndexOld));
                //						}
                //					}
                //
                //					_nodeRL.Add(_strKey , _strName);
                //					
                //					
                //
                //					if(this.dsData.Tables.Contains("dtRLList"))
                //					{
                //						object[] _objNewRL = new object[] { this.dataRL.RoutingPathID(strTabIDRL,0),this.dataRL.RoutingPathName(strTabIDRL,0),this.dataRL.AUECID(strTabIDRL)};
                //						this.dsData.Tables["dtRLList"].Rows.Add(_objNewRL);
                //					}
                //					this.dataRL.ID(strTabIDRL,_iIDRetunedFromSave);
                //					//					this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"] = Convert.ToInt32(_strKey.Substring("r:".Length));
                //				}
                //
                //				this.treeMain.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                //				_nodeRL[_strKey].Selected=true;
                //				this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);


            }
            else if (tabctrlMain.ActiveTab == tabctrlMain.Tabs["Client"])//client
            {
                if (this.ucClient.TabID.Trim().Equals(strTabIDClient.Trim()))
                {

                    //					if( ! VerificationRLComplete(strTabIDClient))
                    //					{
                    //						MessageBox.Show("Client Form Incomplete");
                    //						return ;
                    //					}
                    string _strIncompleteMessage = "";
                    if (!VerificationRLComplete(strTabIDClient, out _strIncompleteMessage))
                    {
                        MessageBox.Show("Client Form Incomplete for : " + _strIncompleteMessage);
                        return;
                    }
                    string _strKey = this.ucClient.SaveData();
                    if (Convert.ToInt32(_strKey.Split(':')[1]) < 0)
                    {
                        return;
                    }



                    //modifying tree
                    //					int _iIDRetunedFromSave = this.ucClient.SaveData();
                    //					if(_iIDRetunedFromSave <0)
                    //					{
                    //						return;
                    //					}
                    //					string _strKey = _cPrefixClient+":"+    _iIDRetunedFromSave.ToString()+":"+ this.dataRL.AUECID(strTabID).ToString();
                    //					string _strName = this.dataRL.Name(strTabIDClient);//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString();
                    //			
                    //					int _iIndex = _nodeClient.IndexOf(_strKey);
                    //					if(_iIndex>=0)
                    //					{					
                    //						_nodeClient.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClient.Nodes.GetItem(_iIndex));
                    //						_nodeClient.Nodes.Insert(_iIndex, _strKey, _strName);						
                    //					}
                    //					else
                    //					{
                    //						if(this.dataRL.ID(strTabIDClient)>=0)
                    //						{
                    //							//remving older one
                    //						
                    //							string _strKeyOld = _cPrefixClient+":"+this.dataRL.ID(strTabIDClient).ToString()+":"+this.dataRL.AUECID(strTabIDClient).ToString();
                    //							int _iIndexOld = _nodeClient.IndexOf(_strKeyOld);
                    //							if(_iIndexOld >= 0)
                    //							{
                    //								_nodeClient.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClient.Nodes.GetItem(_iIndexOld));
                    //							}
                    //						}
                    //						_nodeClient.Add(_strKey , _strName);
                    //
                    //						
                    //						this.dataRL.ID(strTabIDClient,_iIDRetunedFromSave);
                    //					
                    //					}
                    //
                    this.treeMain.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                    _nodeClient[_strKey].Selected = true;
                    this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);



                }
                else//group
                {
                    //					if( ! VerificationRLComplete(strTabIDGroup))
                    //					{
                    //						MessageBox.Show("Group Form Incomplete");
                    //						return ;
                    //					}

                    this.ucClient.SaveClientList();

                    string _strIncompleteMessage = "";
                    if (!VerificationRLComplete(strTabIDGroup, out _strIncompleteMessage))
                    {
                        MessageBox.Show("Group Form Incomplete for : " + _strIncompleteMessage);
                        return;
                    }
                    string _strKey = this.ucClient.SaveData();
                    if (Convert.ToInt32(_strKey.Split(':')[1]) < 0)
                    {
                        return;
                    }



                    //					//modifying tree
                    //					int _iIDRetunedFromSave = this.ucClient.SaveData();
                    //					if(_iIDRetunedFromSave <0)
                    //					{
                    //						return;
                    //					}
                    //					string _strKey = _cPrefixGroup +":"+    _iIDRetunedFromSave.ToString();
                    //					string _strName = this.dataRL.Name(strTabIDGroup);//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString();
                    //			
                    //					int _iIndex = _nodeClientGrp.IndexOf(_strKey);
                    //					if(_iIndex>=0)
                    //					{					
                    //						_nodeClientGrp.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClientGrp.Nodes.GetItem(_iIndex));
                    //						_nodeClientGrp.Nodes.Insert(_iIndex, _strKey, _strName);						
                    //					}
                    //					else
                    //					{
                    //						if(this.dataRL.ID(strTabIDGroup)>=0)
                    //						{
                    //							//remving older one
                    //						
                    //							string _strKeyOld = _cPrefixGroup +":"+this.dataRL.ID(strTabIDGroup).ToString();
                    //							int _iIndexOld = _nodeClientGrp.IndexOf(_strKeyOld);
                    //							if(_iIndexOld >= 0)
                    //							{
                    //								_nodeClientGrp.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClientGrp.Nodes.GetItem(_iIndexOld));
                    //							}
                    //						}
                    //
                    //						_nodeClientGrp.Add(_strKey , _strName);
                    //						this.dataRL.ID(strTabIDGroup,_iIDRetunedFromSave);
                    //					
                    //					}

                    this.treeMain.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                    _nodeClientGrp[_strKey].Selected = true;
                    this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                    this.LoadGroupClient(_nodeClientGrp[_strKey]);


                }




            }
            else
            {
                MessageBox.Show(" some err in saving,  none of the expected tab active");
            }






            //			if(!( this.dsData.Tables.Contains("dtMemoryRL") && this.dsData.Tables["dtMemoryRL"].Rows.Count >0))
            //			{
            //				MessageBox.Show(" Nothing To Save " );
            //				return ;
            //			}
            //
            ////			MessageBox.Show("Saving : " + this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["MemoryID"].ToString());
            //			const string _cPrefixRL = "r";
            //			const string _cPrefixClient = "c";
            ////			const string _cPrefixGroup = "g";
            //			const string _strKeyHeadingRL = "r:-1";
            //			const string _strKeyHeadingClient = "c:-2";
            //			const string _strKeyHeadingGroup = "g:-3";
            //			const string _strMemoryIDRL = "RL";
            //			const string _strMemoryIDClient = "RL0";
            //
            //			System.Windows.Forms.NodeTree _nodeRL=(System.Windows.Forms.NodeTree)(this.treeMain.Nodes.GetItem(this.treeMain.IndexOf(_strKeyHeadingRL)));
            //			System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.treeMain.Nodes.GetItem(this.treeMain.IndexOf(_strKeyHeadingClient)));
            //			System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.GetItem(_nodeClient.IndexOf(_strKeyHeadingGroup)));
            //
            //			if(tabctrlMain.ActiveTab == tabctrlMain.Tabs["RLogic"]) // this.tabpageRL.ContainsFocus)
            //			{
            //				
            //
            //				// verification
            //				if( ! VerificationRLComplete(_strMemoryIDRL))
            //				{
            //					MessageBox.Show("Form Incomplete");
            //					return ;
            //				}
            //
            //
            //
            //				//modifying tree
            //				int _iRLIDRetunedFromSave = this.ucRLogic.SaveData();
            //				if(_iRLIDRetunedFromSave <0)
            //				{
            //					return;
            //				}
            //				string _strKey = _cPrefixRL+":"+    _iRLIDRetunedFromSave.ToString();
            //				string _strName = this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString();
            //			
            //				int _iIndex = _nodeRL.IndexOf(_strKey);
            //				if(_iIndex>=0)
            //				{					
            //					_nodeRL.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeRL.Nodes.GetItem(_iIndex));
            //					_nodeRL.Nodes.Insert(_iIndex, _strKey, _strName);						
            //				}
            //				else
            //				{
            //					if(! IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"]))
            //					{
            //						//remving older one
            //						
            //						string _strKeyOld = _cPrefixRL+":"+this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"].ToString();
            //						int _iIndexOld = _nodeRL.IndexOf(_strKeyOld);
            //						if(_iIndexOld >= 0)
            //						{
            //							_nodeRL.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeRL.Nodes.GetItem(_iIndexOld));
            //						}
            //					}
            //
            //					_nodeRL.Add(_strKey , _strName);
            //					this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"] = Convert.ToInt32(_strKey.Substring("r:".Length));
            //				}
            //
            //
            //
            //					//this.treeMain.Nodes
            ////				string _strRLID = (_dtTemp).Rows[0][0].ToString();
            ////				_strRLID;	
            //				//reloading tree
            ////				this.dtTree.Clear();
            ////				this.dtTree=BLL.DataHandelingManager.DataStoredProcedure("P_GetRLTree", null);
            ////				this.LoadTree();
            //
            //			}
            //			else if(tabctrlMain.ActiveTab == tabctrlMain.Tabs["Client"])
            //			{
            //
            //				string _strKey = _cPrefixClient+":"+    this.ucClient.SaveData().ToString();
            //				string _strName = this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDClient)["RLName"].ToString();
            //			
            //				int _iIndex = _nodeClient.IndexOf(_strKey);
            //				if(_iIndex>=0)
            //				{					
            //					_nodeClient.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClient.Nodes.GetItem(_iIndex));
            //					_nodeClient.Nodes.Insert(_iIndex, _strKey, _strName);						
            //				}
            //				else
            //				{
            //					if(! IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDClient)["RLID"]))
            //					{
            //						//remving older one
            //						
            //						string _strKeyOld = _cPrefixClient+":"+this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDClient)["ClientID"].ToString();
            //						int _iIndexOld = _nodeClient.IndexOf(_strKeyOld);
            //						_nodeClient.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClient.Nodes.GetItem(_iIndexOld));
            //					}
            //
            //					_nodeClient.Add(_strKey , _strName);
            //					this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDClient)["ClientID"] = Convert.ToInt32(_strKey.Substring("c:".Length));
            //				}
            //
            //
            //
            //			}
            //			else
            //			{
            //				MessageBox.Show(" some err in saving,  none of the expected tab active");
            //			}

            //this.dsData.Tables["dtMemoryRL"].Rows[this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[
        }



        private bool VerificationRLComplete(string _strTabID, out string strIncompleteMessage)
        {
            bool _bComplete = true;
            const string strTabIDClient = "client";
            const string strTabIDGroup = "group";
            string _strIncompleteMessage = "";

            if (this.dataRL.Name(_strTabID).Trim().Equals(""))
            {
                _strIncompleteMessage = " Name can't be blank ";
                _bComplete = false;
            }
            if (this.dataRL.AUECID(_strTabID) < 0 || this.dataRL.RoutingPathCount(_strTabID) <= 0)
            {
                _strIncompleteMessage = " AUEC ";
                _bComplete = false;
            }
            if (this.dataRL.RoutingPathCount(_strTabID) <= 0)
            {
                _strIncompleteMessage = " No Routing Logic Selected ";
                _bComplete = false;
            }

            for (int i = 0; i < this.dataRL.RoutingPathCount(_strTabID); i++)
            {
                if (this.dataRL.ConditionsCount(_strTabID, i) <= 0 || this.dataRL.TradingAccountIDDefault(_strTabID, i) < 0)
                {
                    _strIncompleteMessage = " No Condition Selected, and or , No Default Trading Account Selected ";
                    _bComplete = false;
                    break;
                }

                for (int m = 0; m <= this.dataRL.ConditionsCount(_strTabID, i) - 1; m++)
                {
                    if (this.dataRL.ParameterID(_strTabID, i, m) < 0 || this.dataRL.ParameterValue(_strTabID, i, m).Trim().Equals(""))
                    {
                        _strIncompleteMessage = " No Condition Selected ";
                        _bComplete = false;
                        break;
                    }

                    if ((this.dataRL.ParameterID(_strTabID, i, m) == (Convert.ToInt32((this.dsData.Tables["dtParameters"].Select("ParamName = 'Quantity'"))[0]["ParamID"].ToString()))) && this.dataRL.OperatorID(_strTabID, i, m) < 0)
                    {
                        _strIncompleteMessage = " No Condition's Parameter Not Complete ";
                        _bComplete = false;
                        break;
                    }


                }
                if (!_bComplete)
                {

                    break;
                }

                for (int m = 0; m < this.dataRL.TradingAccountCount(_strTabID, i); m++)
                {
                    if ((this.dataRL.CounterPartyID(_strTabID, i, m) >= 0 ^ this.dataRL.VenueID(_strTabID, i, m) >= 0))//this.dataRL.TradingAccountID(_strTabID,i,m)<0 &&  
                    {
                        _strIncompleteMessage = " Trading account or Counter Party -Venue, neither selected ";
                        _bComplete = false;
                        break;
                    }
                }

                if (!_bComplete)
                {
                    break;
                }
            }

            if (_strTabID.Equals(strTabIDClient) && this.dataRL.ID(_strTabID) < 0)
            {
                _strIncompleteMessage = " No Client Selection ";
                _bComplete = false;
            }

            if ((_strTabID.Equals(strTabIDClient) || _strTabID.Equals(strTabIDGroup)))
            {
                //				if( (this.dataRL.ClientCount(_strTabID)<=0))
                //				{
                //					_strIncompleteMessage=" No Client Selection ";
                //					_bComplete=false;
                //				}
                int _iRLID;
                for (int i = 0; i < this.dataRL.RoutingPathCount(_strTabID); i++)
                {
                    if (!_bComplete)
                    {
                        break;
                    }
                    _iRLID = this.dataRL.RoutingPathID(_strTabID, i);
                    if (_iRLID < 0)
                    {
                        continue;
                    }
                    for (int j = 0; j < this.dataRL.RoutingPathCount(_strTabID); j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if (_iRLID == this.dataRL.RoutingPathID(_strTabID, j))
                        {
                            _strIncompleteMessage = " Same RL can't be choosen ";
                            _bComplete = false;
                            break;
                        }
                    }

                }
            }

            strIncompleteMessage = _strIncompleteMessage;
            return _bComplete;


            //			if(IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"]) || IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["AUECID"]) ||  IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["TradingAccountDefaultID"]) ||  IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["ParameterID0"]) ||  IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["ParameterValue0"]) )
            //			{
            //			}
            //			else if(Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["AUECID"].ToString()) < 0) 
            //			{
            //			}
            //			else if(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString() == ""    )
            //			{
            //			}
            //			else if (Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["TradingAccountDefaultID"].ToString()) <0)
            //			{
            //			}
            //			else if(Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["ParameterID0"].ToString()) <0)
            //			{
            //			}
            //			else if(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["ParameterValue0"].ToString() =="")
            //			{
            //			}
            //			else if(Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["ParameterID0"].ToString()) == Convert.ToInt32((this.dsData.Tables["dtParameters"].Select(" ParamName = 'Quantity'"))[0]["ParamID"].ToString()))
            //			{
            //				if(IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["OperatorID0"]) || Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["OperatorID0"].ToString()) <0)
            //				{
            //				}
            //				else
            //				{
            //					_bComplete = true;
            //				}
            //			}
            //			else if(Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["ParameterID1"].ToString()) == Convert.ToInt32((this.dsData.Tables["dtParameters"].Select(" ParamName = 'Quantity'"))[0]["ParamID"].ToString()))
            //			{
            //				if(IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["OperatorID1"]) || Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["OperatorID1"].ToString()) <0)
            //				{
            //				}
            //				else
            //				{
            //					_bComplete = true;
            //				}
            //			}
            //			else if(Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["ParameterID2"].ToString()) == Convert.ToInt32((this.dsData.Tables["dtParameters"].Select(" ParamName = 'Quantity'"))[0]["ParamID"].ToString()))
            //			{
            //				if(IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["OperatorID2"]) || Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["OperatorID2"].ToString()) <0)
            //				{
            //				}
            //				else
            //				{
            //					_bComplete = true;
            //				}
            //			}
            //			else
            //			{
            //				_bComplete = true;
            //			}
            //	
            //			return _bComplete;

        }

        #endregion



        //		#region Fill the memeory table with the slected data
        //		//		private void FillMemoryRecord(string  _sSelectedKey)
        //		//		{
        //		//			System.Data.DataRow _row = dsData.Tables["T_RoutingLogicMemoryRL"].Rows[1];
        //		//
        //		//			System.Data.DataRow _rowTemp = (dsData.Tables["T_RoutingLogic"].Select("RLID = " + _sSelectedKey.Remove(0,2)))[0];
        //		//			_row["RLID"] = _rowTemp["RLID"];
        //		//			_row["RLName"] = _rowTemp["Name"];
        //		//			_row["AUECID"] = _rowTemp["AUECID_FK"];
        //		//
        //		//			_row["TradingAccountDefaultID"] = _rowTemp["DefaultCompanyTradingAccountID_FK"];
        //		//			_row["TradingAccountNameDefault"] = (dsData.Tables["T_CompanyTradingAccounts"].Select("CompanyTradingAccountsID = " + _row["TradingAccountDefaultID"].ToString()))[0]["TradingAccountName"];
        //		//
        //		//			_rowTemp = (dsData.Tables["T_AUEC"].Select("AUECID = " + _row["AUECID"].ToString()))[0];
        //		//			_row["AssetID"] = _rowTemp["AssetID"];
        //		//			_row["UnderLyingID"] = _rowTemp["UnderLyingID"];
        //		//			_row["AUECExchangeID"] = _rowTemp["AUECExchangeID"];
        //		//			
        //		//			_row["AssetName"] = (dsData.Tables["T_Asset"].Select("AssetID = " + _row["AssetID"].ToString()))[0]["AssetName"];
        //		//			_row["UnderLyingName"] = (dsData.Tables["T_UnderLying"].Select("UnderLyingID = " + _row["UnderLyingID"].ToString()))[0]["UnderLyingName"];
        //		//			_row["ExchangeName"] = (dsData.Tables["T_AUECExchange"].Select("AUECExchangeID = " + _row["AUECExchangeID"].ToString()))[0]["DisplayName"];
        //		//			
        //		//		
        //		//			foreach(System.Data.DataRow _rowTradeVenues in (dsData.Tables["T_RoutingLogicVenues"].Select("RLID_FK = " + _row["RLID"].ToString())))
        //		//			{
        //		//				
        //		//				if(_rowTradeVenues["CompanyTradingAccountID_FK"] != "" || _rowTradeVenues["CompanyTradingAccountID_FK"] != "")
        //		//				{
        //		//
        //		//					_row["TradingAccountID"+_rowTradeVenues["Rank"].ToString()]= _rowTradeVenues["CompanyTradingAccountID_FK"];
        //		//					_row["TradingAccountName"+_rowTradeVenues["Rank"].ToString()] = (dsData.Tables["T_CompanyTradingAccounts"].Select("CompanyTradingAccountsID = " + _rowTradeVenues["CompanyTradingAccountID_FK"].ToString()))[0]["TradingAccountName"];
        //		//				}
        //		//				else
        //		//				{
        //		//					_row["TradingAccountID"+_rowTradeVenues["Rank"].ToString()]= "" ;
        //		//					_row["TradingAccountName"+_rowTradeVenues["Rank"].ToString()] = "";
        //		//				}
        //		//
        //		//				if(_rowTradeVenues["CompanyCounterPartyVenueID_FK"] != "" || _rowTradeVenues["CompanyCounterPartyVenueID_FK"] != "")
        //		//				{
        //		//					_rowTemp = (dsData.Tables["T_CounterPartyVenue"].Select("CounterPartyVenueID = " + _rowTradeVenues["CompanyCounterPartyVenueID_FK"].ToString()))[0];
        //		//					
        //		//					_row["CounterPartyID"+_rowTradeVenues["Rank"].ToString()]= _rowTemp["CounterPartyID"];
        //		//					_row["CounterPartyName"+_rowTradeVenues["Rank"].ToString()] = (dsData.Tables["T_CounterParty"].Select("CounterPartyID = " + _rowTemp["CounterPartyID"].ToString()))[0]["FullName"];
        //		//
        //		//					_row["VenueID"+_rowTradeVenues["Rank"].ToString()]=_rowTemp["VenueID"];
        //		//					_row["VenueName"+_rowTradeVenues["Rank"].ToString()] = (dsData.Tables["T_Venue"].Select("VenueID = " + _rowTemp["VenueID"].ToString()))[0]["VenueName"];
        //		//
        //		//				}
        //		//				else
        //		//				{
        //		//					_row["CounterPartyID"+_rowTradeVenues["Rank"].ToString()]= "" ;
        //		//					_row["CounterPartyName"+_rowTradeVenues["Rank"].ToString()] = "";
        //		//					_row["VenueID"+_rowTradeVenues["Rank"].ToString()]= "" ;
        //		//					_row["VenueName"+_rowTradeVenues["Rank"].ToString()] = "";
        //		//
        //		//				}
        //		//
        //		//			}
        //		//
        //		//
        //		//			int _iParameterCount=0;
        //		//			foreach(System.Data.DataRow _rowParameter in (dsData.Tables["T_RoutingLogicCondition"].Select("RLID_FK = " + _row["RLID"].ToString())))
        //		//			{				
        //		//				_row["ParameterID"+_rowParameter["Sequence"].ToString()]= _rowParameter["ParameterID_FK"];
        //		//				_row["ParameterValue"+_rowParameter["Sequence"].ToString()] = _rowParameter["ParameterValue"];
        //		//				_row["JoinCondition"+_rowParameter["Sequence"].ToString()] = _rowParameter["JoinCondition"];
        //		//				_row["OperatorID"+_rowParameter["Sequence"].ToString()] = _rowParameter["OperatorID_FK"];
        //		//					
        //		//				_iParameterCount++;							
        //		//			}
        //		//			int _iMaxParameters = 3;
        //		//			
        //		//			for(;_iParameterCount<_iMaxParameters;_iParameterCount++)
        //		//			{
        //		//				_row["ParameterID"+_iParameterCount.ToString()]= "";
        //		//				_row["ParameterValue"+_iParameterCount.ToString()] = "";
        //		//				_row["JoinCondition"+_iParameterCount.ToString()] = "";
        //		//				_row["OperatorID"+_iParameterCount.ToString()] = "";
        //		//			}
        //		//
        //		//	}
        //		//
        //		#endregion


        //		#region is null
        //		private bool IsNull(Object _obj)
        //		{
        //			if(_obj==null)
        //			{
        //				return true;
        //			}
        //			else if( _obj.Equals(null))
        //			{
        //				return true;
        //			}
        //			else if (_obj.Equals(System.DBNull.Value))
        //			{
        //				return true;
        //			}
        //
        //			return false;
        //		}
        //		#endregion

        private void Delete(object sender, System.EventArgs e)
        {
            const string _cPrefixRL = "r";
            const string _cPrefixClient = "c";
            const string _cPrefixGroup = "g";
            const string _strKeyHeadingRL = "r:-1";
            const string _strKeyHeadingClient = "c:-2";
            const string _strKeyHeadingGroup = "g:-3";

            System.Windows.Forms.NodeTree _nodeRL = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingRL]);
            System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingClient]);
            System.Windows.Forms.NodeTree _nodeClientGrp = (System.Windows.Forms.NodeTree)(_nodeClient[_strKeyHeadingGroup]);

            int iDeleteForceFully = 1;

            if (Functions.IsNull(this.Tree.SelectedNode))
            {
                return;
            }



            string _sSelectedKey = ((System.Windows.Forms.NodeTree)(this.Tree.SelectedNode)).Key;
            string[] _strKeySelectedArray = _sSelectedKey.Split(':');
            int _iValue = Convert.ToInt32(_strKeySelectedArray[1]);
            string _strSelectedPrefix = _strKeySelectedArray[0];

            if (_strKeySelectedArray.Length > 2 && !(_strKeySelectedArray[0].Equals(_cPrefixClient)))
            {
                return;
            }
            //
            //			string _strSelectedPrefix = _sSelectedKey.Substring(0,_cPrefixRL.Length);
            //			int _iValue = Convert.ToInt32(_sSelectedKey.Substring((_cPrefixRL+":").Length));
            if (_iValue < 0)
            {
                return;
            }

            bool _bResult = false;

            Forms.DialogBox dlgbx = new Forms.DialogBox(" Are you sure you want to delete " + this.Tree.SelectedNode.Text + " ? ");

            //			this.Hide();
            dlgbx.ShowDialog();
            dlgbx.BringToFront();
            dlgbx.DesktopLocation = new System.Drawing.Point(-100, 50);

            _bResult = (dlgbx.DialogResult == DialogResult.Yes) ? true : false;
            //			this.Show();

            dlgbx.Close();
            this.BringToFront();
            if (!_bResult)
            {
                return;
            }

            //			System.Data.SqlClient.SqlParameter[] _sqlParam ;
            int _iIndexToRemove;


            //			_bResult=BLL.DataCallFunctionsManager.Delete(_strSelectedPrefix, _iValue, iDeleteForceFully);
            _bResult = BLL.DataCallFunctionsManager.Delete(_sSelectedKey, iDeleteForceFully);

            if (!_bResult)
            {
                MessageBox.Show(" Failed to delete ");
                return;

            }

            switch (_strSelectedPrefix)
            {
                case _cPrefixRL:

                    //					_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@RLIDID",_iValue), new System.Data.SqlClient.SqlParameter("@DeleteForceFully", iDeleteForceFully ) };
                    //					BLL.DataHandelingManager.DataStoredProcedure("P_DeleteRL",_sqlParam);

                    _iIndexToRemove = _nodeRL.IndexOf(_sSelectedKey);
                    if (_iIndexToRemove >= 0)
                    {
                        _nodeRL.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeRL[_iIndexToRemove]);
                    }

                    _nodeRL.Selected = true;

                    //					foreach( System.Data.DataRow _row  in  (this.dsData.Tables["dtRLList"].Select("RLID = " + _iValue.ToString())) )
                    //					{
                    //						_row.r
                    //
                    //					}

                    this.LoadRLListData();



                    break;
                case _cPrefixClient:

                    //					_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@ClientID",_iValue) };
                    //					BLL.DataHandelingManager.DataStoredProcedure("P_DeleteDataRoutingLogicCompanyClientObject",_sqlParam);

                    _iIndexToRemove = _nodeClient.IndexOf(_sSelectedKey);
                    if (_iIndexToRemove >= 0)
                    {
                        _nodeClient.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClient[_iIndexToRemove]);
                    }

                    _nodeClient.Selected = true;

                    //					this.lo

                    break;
                case _cPrefixGroup:
                    //					_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@GroupID",_iValue) };
                    //					BLL.DataHandelingManager.DataStoredProcedure("P_DeleteRLGroup",_sqlParam);


                    _iIndexToRemove = _nodeClientGrp.IndexOf(_sSelectedKey);
                    if (_iIndexToRemove >= 0)
                    {
                        _nodeClientGrp.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClientGrp[_iIndexToRemove]);
                    }

                    _nodeClientGrp.Selected = true;

                    break;
                default:

                    break;
            }




        }

        private void Add(object sender, System.EventArgs e)
        {
            const string _cPrefixRL = "r";
            const string _cPrefixClient = "c";
            const string _cPrefixGroup = "g";
            const string _strKeyHeadingRL = "r:-1";
            const string _strKeyHeadingClient = "c:-2";
            const string _strKeyHeadingGroup = "g:-3";

            System.Windows.Forms.NodeTree _nodeRL = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingRL]);
            System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingClient]);
            System.Windows.Forms.NodeTree _nodeClientGrp = (System.Windows.Forms.NodeTree)(_nodeClient[_strKeyHeadingGroup]);


            if (Functions.IsNull(this.Tree.SelectedNode))
            {
                return;
            }

            string _sSelectedKey = ((System.Windows.Forms.NodeTree)(this.Tree.SelectedNode)).Key;


            switch (_sSelectedKey.Substring(0, _cPrefixRL.Length))
            {
                case _cPrefixRL:

                    _nodeRL.Selected = true;

                    break;
                case _cPrefixClient:

                    _nodeClient.Selected = true;

                    break;
                case _cPrefixGroup:

                    _nodeClientGrp.Selected = true;

                    break;
                default:

                    break;
            }

        }



        public void ValueChanged(string _strTabID, int _iIndex, bool _bRoutingPath)
        {
            this.strTabID = _strTabID;


            if ((!Functions.IsNull(_strTabID)) && _bRoutingPath == false && _strTabID.Equals("group") && _iIndex < 0 && (!Functions.IsNull(this.Tree.SelectedNode)) && this.dataRL.ID(_strTabID) >= 0)
            {
                this.LoadNodeSelected();

                //				System.Windows.Forms.NodeTree _node = this.nodeMain.SelectedNodes[0] ;
                //				string[] _strKeyArray = _node.Key.Split(':');
                //				if(_strKeyArray[0].Equals(_cPrefixGroup))
                //				{
                //					if(_strKeyArray.Length == ("g:1").Split(':').Length )
                //					{
                //						LoadGroupClient(_node);
                //					}
                //					else
                //					{
                //						LoadGroupClient(_node.Parent);
                //					}
                //				}

            }

            if ((!Functions.IsNull(_strTabID)) && this.bUserTriggered == true && (!_strTabID.Equals("RL")))//(( _strTabID.Equals("group")) || ( _strTabID.Equals("client")))  )
            {
                this.ucClient.GroupToClient(_strTabID, _iIndex, _bRoutingPath);
                return;
            }

            this.bUserTriggered = true;
        }

        private void LoadNodeSelected()
        {
            const string _cPrefixGroup = "g";
            System.Windows.Forms.NodeTree _node = ((System.Windows.Forms.NodeTree)(this.Tree.SelectedNode));
            string[] _strKeyArray = _node.Key.Split(':');
            if (_strKeyArray[0].Equals(_cPrefixGroup) && Convert.ToInt32(_strKeyArray[1]) >= 0)
            {
                if (_strKeyArray.Length == ("g:1").Split(':').Length)
                {
                    LoadGroupClient(_node);
                }
                else
                {
                    LoadGroupClient(_node.Parent);
                }
            }
        }

        public bool UserTriggered
        {
            get
            {
                return bUserTriggered;
            }
            set
            {
                bUserTriggered = value;
            }

        }

        public bool SelectClientInTree
        {
            set
            {
                const string _cKeyClientHeading = "c:-2";

                const string _cPrefixClient = "c";




                //			const string strTabIDRL="RL";
                //			const string strTabIDGroup="group";
                //			const string strTabIDClient="client";
                //			string _strTabID;


                System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_cKeyClientHeading]);



                //				if(value==true)
                //				{

                //				this.LoadNodeSelected();


                const string _cPrefixGroup = "g";
                System.Windows.Forms.NodeTree _node = ((System.Windows.Forms.NodeTree)(this.Tree.SelectedNode));
                string[] _strKeyArray = _node.Key.Split(':');
                //				if(_strKeyArray[0].Equals(_cPrefixGroup) && Convert.ToInt32(_strKeyArray[1])>=0)
                //				{
                //					if(_strKeyArray.Length == ("g:1").Split(':').Length )
                //					{
                ////						_node.Clear();
                //						_node.Clear();
                //					}
                //					else
                //					{
                //						System.Windows.Forms.NodeTree _nodeParent = (System.Windows.Forms.NodeTree)_node.Parent;
                //						bool result = _nodeParent.Clear();
                ////						MessageBox.Show("Got after return");
                ////						_nodeParent.Clear();
                ////						_node.Parent.Clear();
                //
                //					}
                //				}
                string _strKeyClient = _cPrefixClient + ":" + this.dataRL.ID(strTabIDClient).ToString() + ":" + this.dataRL.AUECID(strTabIDClient).ToString();
                bool _bClientExists = _nodeClient.Contains(_strKeyClient);
                if (_bClientExists)
                {
                    this.Tree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                    _nodeClient[_strKeyClient].Selected = true;
                    this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);

                }
                else
                {
                    _nodeClient.Selected = value;
                }
                //
                //				}
                if (_strKeyArray[0].Equals(_cPrefixGroup) && Convert.ToInt32(_strKeyArray[1]) >= 0)
                {
                    if (_strKeyArray.Length == ("g:1").Split(':').Length)
                    {
                        //						_node.Clear();
                        _node.Clear();
                    }
                    else
                    {
                        System.Windows.Forms.NodeTree _nodeParent = (System.Windows.Forms.NodeTree)_node.Parent;
                        bool result = _nodeParent.Clear();
                        //						MessageBox.Show("Got after return");
                        //						_nodeParent.Clear();
                        //						_node.Parent.Clear();

                    }
                }

                this.ucClient.SelectedGroupClientID = Functions.MinValue;


            }
        }


        public int SelectGroupInTree
        {
            set
            {

                const string _cPrefixGroup = "g";

                const string _strKeyHeadingClient = "c:-2";
                const string _strKeyHeadingGroup = "g:-3";


                System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingClient]);
                System.Windows.Forms.NodeTree _nodeClientGrp = (System.Windows.Forms.NodeTree)(_nodeClient[_strKeyHeadingGroup]);

                this.Tree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                if (value >= 0)
                {
                    _nodeClientGrp[_cPrefixGroup + ":" + value.ToString()].Selected = true;

                }
                else
                {
                    _nodeClientGrp.Selected = true;
                }
                this.ucClient.SelectedGroupClientID = Functions.MinValue;
                this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);


            }
        }


        //		private Infragistics.Win.UltraWinTree.UltraTree Tree
        //		{
        //			get
        //			{
        //				return this.treeMain ;
        //			}
        //		}
        private System.Windows.Forms.TreeView Tree
        {
            get
            {
                return this.treeMain;
            }
        }


    }





}
