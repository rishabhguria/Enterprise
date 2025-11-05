using Infragistics.Win;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Interface;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridStrategy.
    /// </summary>
    public class GridStrategy : System.Windows.Forms.UserControl, IAuditSource
    {
        private const string FORM_NAME = "GridStrategy : ";
        private Infragistics.Win.UltraWinGrid.UltraGrid grdStrategy;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Button btnCreate;
        private Button btnDelete;
        private Button btnEdit;
        private IContainer components;
        public static Dictionary<int, List<int>> _strategyAuditStatus = new Dictionary<int, List<int>>();

        [AuditManager.Attributes.AuditSourceConstAttri]
        public GridStrategy()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

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
                if (grdStrategy != null)
                {
                    grdStrategy.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (btnCreate != null)
                {
                    btnCreate.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (createCompanyStrategyEdit != null)
                {
                    createCompanyStrategyEdit.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridStrategy));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyName", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyShortName", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyStrategyID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyUserStrategyID", 5);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.grdStrategy = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCreate);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.grdStrategy);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(7, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 237);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCreate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreate.BackgroundImage")));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCreate.Location = new System.Drawing.Point(52, 205);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(208, 205);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(130, 205);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // grdStrategy
            // 
            this.grdStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdStrategy.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 75;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 94;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 57;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Width = 75;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdStrategy.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdStrategy.DisplayLayout.GroupByBox.Hidden = true;
            this.grdStrategy.DisplayLayout.MaxColScrollRegions = 1;
            this.grdStrategy.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdStrategy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdStrategy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdStrategy.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdStrategy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdStrategy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdStrategy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdStrategy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdStrategy.Location = new System.Drawing.Point(6, 22);
            this.grdStrategy.Name = "grdStrategy";
            this.grdStrategy.Size = new System.Drawing.Size(322, 171);
            this.grdStrategy.TabIndex = 3;
            this.grdStrategy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdStrategy.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdStrategy_InitializeLayout);
            this.grdStrategy.AfterRowActivate += new System.EventHandler(this.grdStrategy_AfterRowActivate);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GridStrategy
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "GridStrategy";
            this.Size = new System.Drawing.Size(346, 247);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;


        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1863
            CreateCompanyStrategy createCompanyStrategy = new CreateCompanyStrategy();
            int countBeforeEnd = createCompanyStrategy.CurrentCompanyStrategies.Count;
            Strategies strategies = new Strategies();
            createCompanyStrategy.CurrentCompanyStrategies = (Strategies)grdStrategy.DataSource;

            foreach (Prana.Admin.BLL.Strategy strategy in createCompanyStrategy.CurrentCompanyStrategies)
            {
                if (strategy.StrategyName.ToString() == "")
                {
                    createCompanyStrategy.NoData = 1;
                }
                else
                {
                    createCompanyStrategy.NoData = 0;
                    break;
                }
            }
            createCompanyStrategy.ShowDialog(this.Parent);
            strategies = createCompanyStrategy.CurrentCompanyStrategies;
            if (countBeforeEnd < strategies.Count)
            {
                foreach (Prana.Admin.BLL.Strategy strategy in createCompanyStrategy.CurrentCompanyStrategies)
                {
                    if (strategy.StrategyID < 0)
                    {
                        if (!_strategyAuditStatus.ContainsKey(1))
                            _strategyAuditStatus.Add(1, new List<int>());
                        _strategyAuditStatus[1].Add(strategy.StrategyID);
                    }
                }
            }
            //grdStrategy.DataSource = createCompanyStrategy.CurrentCompanyStrategies; 
            grdStrategy.DataSource = null;
            grdStrategy.DataSource = strategies;
            if (strategies.Count > 0)
            {
                RefreshGrid();
            }
            else
            {
                BindDataGrid();
            }

            //            if(createCompanyStrategy.CurrentCompanyStrategies.Count > 0 )
            //            {
            ////				grdStrategy.Select(0);
            //                _nullRow = false;
            //            }
            //            else
            //            {
            //                _nullRow = true;
            //            }
            foreach (Prana.Admin.BLL.Strategy strategy in createCompanyStrategy.CurrentCompanyStrategies)
            {
                if (strategy.StrategyName.ToString() != "")
                {
                    _nullRow = false;
                    break;
                }
                else
                {
                    _nullRow = true;
                    break;
                }
            }
            createCompanyStrategy.CurrentCompanyStrategies = null;
            createCompanyStrategy.StrategyEdit = null;
        }
        private void RefreshGrid()
        {
            grdStrategy.DisplayLayout.Bands[0].Columns["StrategyID"].Hidden = true;
            grdStrategy.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
            grdStrategy.DisplayLayout.Bands[0].Columns["CompanyStrategyID"].Hidden = true;
            grdStrategy.DisplayLayout.Bands[0].Columns["CompanyUserStrategyID"].Hidden = true;

            grdStrategy.DisplayLayout.Bands[0].Columns["StrategyName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
            grdStrategy.DisplayLayout.Bands[0].Columns["StrategyShortName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;

            grdStrategy.DisplayLayout.Bands[0].Columns["StrategyName"].Header.Caption = "Name";
            grdStrategy.DisplayLayout.Bands[0].Columns["StrategyShortName"].Header.Caption = "Short Name";
        }

        public Prana.Admin.BLL.Strategies CurrentStrategies
        {
            get
            {
                foreach (Prana.Admin.BLL.Strategy strategy in (Prana.Admin.BLL.Strategies)grdStrategy.DataSource)
                {
                    if (strategy.StrategyName.ToString() == "")
                    {
                        _nullRow = true;
                        //						Prana.Admin.BLL.Strategies nullStrategies  = new Strategies();
                        //						grdStrategy.DataSource = nullStrategies;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                //return (Prana.Admin.BLL.Strategies) grdStrategy.DataSource; 
                Strategies currentStrategies = (Prana.Admin.BLL.Strategies)grdStrategy.DataSource;
                if (currentStrategies.Count > 0)
                {
                    Strategies saveStrategies = new Strategies();
                    Strategies companyStrategies = (Prana.Admin.BLL.Strategies)grdStrategy.DataSource;
                    foreach (Strategy companyStrategy in companyStrategies)
                    {
                        if (companyStrategy.StrategyName != "")
                        {
                            saveStrategies.Add(companyStrategy);
                        }
                    }

                    return saveStrategies;
                    //return (Prana.Admin.BLL.ClientAccounts) grdClientAccounts.DataSource; 
                }
                else
                {
                    currentStrategies = null;
                    return currentStrategies;
                }
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            get { return _companyID; }
            //			set
            //			{
            //				_companyID = value;
            //				BindDataGrid();
            //			}
        }

        private int _strategyID = int.MinValue;
        public int StrategyID
        {
            get { return _strategyID; }
            //			set
            //			{
            //				_strategyID = value;
            //				BindDataGrid();
            //			}
        }

        public Strategy StrategyProperty
        {
            get
            {
                Strategy strategy = new Strategy();
                GetStrategy(strategy);
                return strategy;
            }
            //			set 
            //			{
            //				SetStrategy(value);
            //					
            //			}
        }

        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            BindDataGrid();
        }

        public void GetStrategy(Strategy strategy)
        {
            strategy = (Strategy)grdStrategy.DataSource;
        }

        public void SetStrategy(Strategy strategy)
        {
            grdStrategy.DataSource = strategy;
        }

        private void BindDataGrid()
        {
            try
            {
                Strategy nullStrategy = new Strategy(int.MinValue, "");
                Prana.Admin.BLL.Strategies strategies = CompanyManager.GetStrategy(_companyID);
                if (strategies.Count <= 0)
                {
                    strategies.Add(nullStrategy);
                    _nullRow = true;
                }
                else
                {
                    _nullRow = false;
                }
                grdStrategy.DataSource = strategies;
                RefreshGrid();
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnLogin_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnLogin_Click", null);
                #endregion
            }
        }

        private bool _nullRow = false;
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            errorProvider1.SetError(btnDelete, "");
            bool result = false;
            CreateCompanyStrategy createCompanyStrategy = new CreateCompanyStrategy();
            if (grdStrategy.Rows.Count > 0)
            {
                string strategyName = grdStrategy.ActiveRow.Cells["StrategyName"].Text.ToString();
                if (strategyName != "")
                {
                    if (MessageBox.Show(this, "Do you want to delete this Strategy?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int strategyID = int.Parse(grdStrategy.ActiveRow.Cells["StrategyID"].Text.ToString());

                        Prana.Admin.BLL.Strategies strategies = (Strategies)grdStrategy.DataSource;
                        Prana.Admin.BLL.Strategy strategy = new Prana.Admin.BLL.Strategy();

                        strategy.StrategyID = int.Parse(grdStrategy.ActiveRow.Cells["StrategyID"].Text.ToString());
                        strategy.StrategyName = grdStrategy.ActiveRow.Cells["StrategyName"].Text.ToString();
                        strategy.StrategyShortName = grdStrategy.ActiveRow.Cells["StrategyShortName"].Text.ToString();
                        strategy.CompanyID = int.Parse(grdStrategy.ActiveRow.Cells["CompanyID"].Text.ToString());

                        //strategies.RemoveAt(strategies.IndexOf(strategy));

                        bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                        if (strategyID != int.MinValue)
                        {
                            result = CompanyManager.DeleteStrategy(strategyID, isPermanentDeletion);
                            if (result == false)
                            {
                                MessageBox.Show("The selected strategy is referred by some Client Broker Venue. Please unallocate that first before deleting it !",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            if (result == true)
                            {
                                strategies.RemoveAt(strategies.IndexOf(strategy));
                                if (!_strategyAuditStatus.ContainsKey(2))
                                {
                                    _strategyAuditStatus.Add(2, new List<int>());
                                }
                                _strategyAuditStatus[2].Add(StrategyID);
                            }
                        }
                        else
                        {
                            strategies.RemoveAt(strategies.IndexOf(strategy));
                            //errorProvider1.SetError(btnDelete, "There is nothing do delete in the Grid!");
                        }

                        Strategies newStrategies = new Strategies();
                        foreach (Strategy tempStrategy in strategies)
                        {
                            newStrategies.Add(tempStrategy);
                        }
                        if (strategies.Count <= 0)
                        {
                            newStrategies.Add(new Strategy(int.MinValue, ""));
                            createCompanyStrategy.NoData = 1;
                            _nullRow = true;
                        }
                        else
                        {
                            createCompanyStrategy.NoData = 0;
                            _nullRow = false;
                        }
                        grdStrategy.DataSource = newStrategies;
                    }
                }
            }
        }

        private CreateCompanyStrategy createCompanyStrategyEdit = null;

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if ((createCompanyStrategyEdit == null) && (_nullRow == false))
            {
                if (grdStrategy.Rows.Count > 0)
                {
                    createCompanyStrategyEdit = new CreateCompanyStrategy();
                    createCompanyStrategyEdit.StrategyEdit = (Prana.Admin.BLL.Strategy)((Prana.Admin.BLL.Strategies)grdStrategy.DataSource)[grdStrategy.ActiveRow.Index];
                    createCompanyStrategyEdit.CurrentCompanyStrategies = (Prana.Admin.BLL.Strategies)grdStrategy.DataSource;
                    createCompanyStrategyEdit.ShowDialog(this.Parent);
                    if (createCompanyStrategyEdit.validStrategy)
                    {
                        if (!_strategyAuditStatus.ContainsKey(3))
                        {
                            _strategyAuditStatus.Add(3, new List<int>());
                        }
                        _strategyAuditStatus[3].Add(createCompanyStrategyEdit.StrategyEdit.StrategyID);

                    }
                    createCompanyStrategyEdit = null;
                }
            }
        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        //added by: Bharat raturi, 25 apr 2014
        //purpose: allow row filtering
        /// <summary>
        /// Allow row filtering on layout initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStrategy_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            grdStrategy.DisplayLayout.Bands[0].Override.AllowRowFiltering = DefaultableBoolean.True;
        }

        internal event EventHandler<int> grdStrategyRowActivated;

        /// <summary>
        /// Event for active row in strategy grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStrategy_AfterRowActivate(object sender, EventArgs e)
        {
            if (grdStrategy.Rows.Count > 0)
            {
                if (grdStrategy.ActiveRow != null && !string.IsNullOrEmpty(grdStrategy.ActiveRow.Cells["StrategyID"].Text))
                {
                    int strategyID = (int)grdStrategy.ActiveRow.Cells["StrategyID"].Value;
                    if (grdStrategyRowActivated != null)
                    {
                        grdStrategyRowActivated(sender, strategyID);
                    }
                }
            }
        }
    }
}
