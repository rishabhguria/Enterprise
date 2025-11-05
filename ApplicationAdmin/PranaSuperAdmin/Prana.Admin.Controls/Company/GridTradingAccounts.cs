using Infragistics.Win;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridTradingAccounts.
    /// </summary>
    public class GridTradingAccounts : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "GridTradingAccounts : ";
        private Infragistics.Win.UltraWinGrid.UltraGrid grdTradingAccounts;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private IContainer components;
        private bool _isApplyCHTheme = false;

        public GridTradingAccounts()
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
                if (grdTradingAccounts != null)
                {
                    grdTradingAccounts.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (btnCreate != null)
                {
                    btnCreate.Dispose();
                }
                if (createCompanyTradingAccounts != null)
                {
                    createCompanyTradingAccounts.Dispose();
                }
                if (createCompanyTradingAccountsEdit != null)
                {
                    createCompanyTradingAccountsEdit.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountsID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountName", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingShortName", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridTradingAccounts));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdTradingAccounts = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTradingAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdTradingAccounts);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(4, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 154);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trading Accounts";
            // 
            // grdTradingAccounts
            // 
            this.grdTradingAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTradingAccounts.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 139;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 154;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdTradingAccounts.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdTradingAccounts.DisplayLayout.GroupByBox.Hidden = true;
            this.grdTradingAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTradingAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdTradingAccounts.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdTradingAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdTradingAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdTradingAccounts.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdTradingAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdTradingAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdTradingAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdTradingAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdTradingAccounts.Location = new System.Drawing.Point(6, 22);
            this.grdTradingAccounts.Name = "grdTradingAccounts";
            this.grdTradingAccounts.Size = new System.Drawing.Size(314, 128);
            this.grdTradingAccounts.TabIndex = 68;
            this.grdTradingAccounts.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdTradingAccounts.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdTradingAccounts_InitializeLayout);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(211, 156);
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
            this.btnEdit.Location = new System.Drawing.Point(133, 156);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCreate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreate.BackgroundImage")));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCreate.Location = new System.Drawing.Point(55, 156);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GridTradingAccounts
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "GridTradingAccounts";
            this.Size = new System.Drawing.Size(340, 184);
            this.Load += new System.EventHandler(this.GridTradingAccounts_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTradingAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCreate;


        public Prana.Admin.BLL.TradingAccounts CurrentTradingAccounts
        {
            get
            {
                foreach (Prana.Admin.BLL.TradingAccount tradingAccount in (Prana.Admin.BLL.TradingAccounts)grdTradingAccounts.DataSource)
                {
                    if (tradingAccount.TradingAccountName.ToString() == "")
                    {
                        _nullRow = true;
                        //						Prana.Admin.BLL.TradingAccounts nullTradingAccounts = new TradingAccounts();
                        //						grdTradingAccounts.DataSource = nullTradingAccounts;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                //return (Prana.Admin.BLL.TradingAccounts) grdTradingAccounts.DataSource; 
                TradingAccounts currentTradingAccounts = (Prana.Admin.BLL.TradingAccounts)grdTradingAccounts.DataSource;
                if (currentTradingAccounts.Count > 0)
                {
                    TradingAccounts saveTradingAccounts = new TradingAccounts();
                    TradingAccounts companyTradingAccounts = (Prana.Admin.BLL.TradingAccounts)grdTradingAccounts.DataSource;
                    foreach (TradingAccount companyTradingAccount in companyTradingAccounts)
                    {
                        if (companyTradingAccount.TradingAccountName != "")
                        {
                            saveTradingAccounts.Add(companyTradingAccount);
                        }
                    }

                    return saveTradingAccounts;
                    //return (Prana.Admin.BLL.ClientAccounts) grdClientAccounts.DataSource; 
                }
                else
                {
                    currentTradingAccounts = null;
                    return currentTradingAccounts;
                }

            }
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                _isApplyCHTheme = true;
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

        private CreateCompanyTradingAccounts createCompanyTradingAccounts = null;
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (createCompanyTradingAccounts == null)
            {
                createCompanyTradingAccounts = new CreateCompanyTradingAccounts();
                // Applying theme for CH Release
                if (_isApplyCHTheme)
                {
                    this.createCompanyTradingAccounts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    this.createCompanyTradingAccounts.ForeColor = System.Drawing.Color.White;
                }
            }
            TradingAccounts tradingAccounts = new TradingAccounts();
            createCompanyTradingAccounts.CurrentCompanyTradingAccounts = (TradingAccounts)grdTradingAccounts.DataSource;
            //CHANGED CODE
            foreach (Prana.Admin.BLL.TradingAccount tradingAccount in createCompanyTradingAccounts.CurrentCompanyTradingAccounts)
            {
                if (tradingAccount.TradingAccountName.ToString() == "")
                {
                    createCompanyTradingAccounts.NoData = 1;
                }
                else
                {
                    createCompanyTradingAccounts.NoData = 0;
                    break;
                }
            }

            createCompanyTradingAccounts.ShowDialog(this.Parent);
            grdTradingAccounts.DataSource = null;
            grdTradingAccounts.Refresh();
            tradingAccounts = createCompanyTradingAccounts.CurrentCompanyTradingAccounts;
            //grdTradingAccounts.DataSource = createCompanyTradingAccounts.CurrentCompanyTradingAccounts;
            grdTradingAccounts.DataSource = tradingAccounts;
            if (tradingAccounts.Count > 0)
            {
                RefreshGrid();
            }
            else
            {
                BindDataGrid();
            }

            //            if(createCompanyTradingAccounts.CurrentCompanyTradingAccounts.Count > 0 )
            //            {
            ////				grdTradingAccounts.Select(0);
            //                _nullRow = false;
            //            }
            foreach (Prana.Admin.BLL.TradingAccount tradingAccount in createCompanyTradingAccounts.CurrentCompanyTradingAccounts)
            {
                if (tradingAccount.TradingAccountName.ToString() != "")
                {
                    _nullRow = false;
                    break;
                }
            }
        }

        private void RefreshGrid()
        {
            grdTradingAccounts.DisplayLayout.Bands[0].Columns["TradingAccountsID"].Hidden = true;
            grdTradingAccounts.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;

            grdTradingAccounts.DisplayLayout.Bands[0].Columns["TradingAccountName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
            grdTradingAccounts.DisplayLayout.Bands[0].Columns["TradingShortName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;

            grdTradingAccounts.DisplayLayout.Bands[0].Columns["TradingAccountName"].Header.Caption = "Name";
            grdTradingAccounts.DisplayLayout.Bands[0].Columns["TradingShortName"].Header.Caption = "Short Name";
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

        private int _tradingAccountID = int.MinValue;
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            //			set
            //			{
            //				_tradingAccountID = value;
            //				BindDataGrid();
            //			}
        }

        public TradingAccount TradingAccountProperty
        {
            get
            {
                TradingAccount tradingAccount = new TradingAccount();
                GetTradingAccount(tradingAccount);
                return tradingAccount;
            }
            //			set 
            //			{
            //				SetTradingAccount(value);
            //					
            //			}
        }

        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            BindDataGrid();
        }

        public void GetTradingAccount(TradingAccount tradingAccount)
        {
            tradingAccount = (TradingAccount)grdTradingAccounts.DataSource;
        }

        public void SetTradingAccount(TradingAccount tradingAccount)
        {
            grdTradingAccounts.DataSource = tradingAccount;
        }

        private void BindDataGrid()
        {
            try
            {
                TradingAccount nullTradingAccount = new TradingAccount(int.MinValue, "");
                Prana.Admin.BLL.TradingAccounts tradingAccounts = CompanyManager.GetTradingAccount(_companyID);
                if (tradingAccounts.Count <= 0)
                {
                    tradingAccounts.Add(nullTradingAccount);
                    _nullRow = true;
                }
                else
                {
                    _nullRow = false;
                }
                grdTradingAccounts.DataSource = tradingAccounts;
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
            CreateCompanyTradingAccounts createCompanyTradingAccounts = new CreateCompanyTradingAccounts();
            if (grdTradingAccounts.Rows.Count > 0)
            {
                string tradingAccountName = grdTradingAccounts.ActiveRow.Cells["TradingAccountName"].Text.ToString();
                if (tradingAccountName != "")
                {
                    if (MessageBox.Show(this, "Do you want to delete this TradingAccount?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int tradingAccountID = int.Parse(grdTradingAccounts.ActiveRow.Cells["TradingAccountsID"].Text.ToString());

                        Prana.Admin.BLL.TradingAccounts tradingAccounts = (TradingAccounts)grdTradingAccounts.DataSource;
                        Prana.Admin.BLL.TradingAccount tradingAccount = new Prana.Admin.BLL.TradingAccount();

                        tradingAccount.TradingAccountsID = int.Parse(grdTradingAccounts.ActiveRow.Cells["TradingAccountsID"].Text.ToString());
                        tradingAccount.TradingAccountName = grdTradingAccounts.ActiveRow.Cells["TradingAccountName"].Text.ToString();
                        tradingAccount.TradingShortName = grdTradingAccounts.ActiveRow.Cells["TradingShortName"].Text.ToString();
                        tradingAccount.CompanyID = int.Parse(grdTradingAccounts.ActiveRow.Cells["CompanyID"].Text.ToString());

                        //tradingAccounts.RemoveAt(tradingAccounts.IndexOf(tradingAccount));
                        if (tradingAccountID != int.MinValue)
                        {
                            result = AccountMasterFundMappingManager.IsTradingAccountMappedToMF(tradingAccountID);
                            if (result)
                            {
                                errorProvider1.SetError(btnDelete, "The Selected trading Account is mapped to some MasterFund. You cannot delete it!");
                            }
                            else
                            {
                                result = CompanyManager.DeleteTradingAccount(tradingAccountID);
                                if (result == false)
                                {
                                    errorProvider1.SetError(btnDelete, "The Selected trading Account is referred by some Client. You cannot delete it!");
                                }
                                else
                                {
                                    tradingAccounts.RemoveAt(tradingAccounts.IndexOf(tradingAccount));
                                }

                            }
                        }
                        else
                        {
                            //errorProvider1.SetError(btnDelete, "There is nothing do delete in the Grid!");
                            tradingAccounts.RemoveAt(tradingAccounts.IndexOf(tradingAccount));
                        }

                        TradingAccounts newTradingAccounts = new TradingAccounts();
                        foreach (TradingAccount tempTradingAccount in tradingAccounts)
                        {
                            newTradingAccounts.Add(tempTradingAccount);
                        }
                        if (tradingAccounts.Count <= 0)
                        {
                            newTradingAccounts.Add(new TradingAccount(int.MinValue, ""));
                            createCompanyTradingAccounts.NoData = 1;
                            _nullRow = true;
                        }
                        else
                        {
                            createCompanyTradingAccounts.NoData = 0;
                            _nullRow = false;
                        }
                        grdTradingAccounts.DataSource = newTradingAccounts;
                    }
                }
            }

        }

        private CreateCompanyTradingAccounts createCompanyTradingAccountsEdit = null;
        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if ((createCompanyTradingAccountsEdit == null) && (_nullRow == false))
            {
                if (grdTradingAccounts.Rows.Count > 0)
                {
                    createCompanyTradingAccountsEdit = new CreateCompanyTradingAccounts();

                    // Applying theme for CH Release
                    if (_isApplyCHTheme)
                    {
                        this.createCompanyTradingAccountsEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                        this.createCompanyTradingAccountsEdit.ForeColor = System.Drawing.Color.White;
                    }

                    createCompanyTradingAccountsEdit.TradingAccountEdit = (TradingAccount)((TradingAccounts)grdTradingAccounts.DataSource)[grdTradingAccounts.ActiveRow.Index];

                    createCompanyTradingAccountsEdit.CurrentCompanyTradingAccounts = (Prana.Admin.BLL.TradingAccounts)grdTradingAccounts.DataSource;
                    createCompanyTradingAccountsEdit.ShowDialog(this.Parent);
                    createCompanyTradingAccountsEdit = null;

                    //Modified By: Bharat Raturi, 17 apr 2014
                    //purpose: Reflect the changes in the grid without needing a click on it 
                    grdTradingAccounts.Focus();
                }



            }
        }

        private void GridTradingAccounts_Load(object sender, System.EventArgs e)
        {

        }

        //added by: Bharat raturi, 25 apr 2014
        //purpose: allow row filtering
        /// <summary>
        /// Allow row filtering on the layout initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTradingAccounts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            grdTradingAccounts.DisplayLayout.Bands[0].Override.AllowRowFiltering = DefaultableBoolean.True;
        }

        /// <summary>
        /// added by: Bharat Raturi, 16 jun 2014
        /// make the controls read only if the user does not have write permission
        /// </summary>
        /// <param name="isActive"></param>
        public void SetGridAccess(bool isActive)
        {
            try
            {
                if (!isActive)
                {
                    grdTradingAccounts.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowAddNew = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                    btnCreate.Enabled = btnDelete.Enabled = btnEdit.Enabled = false;
                }
                else
                {
                    grdTradingAccounts.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    btnCreate.Enabled = btnDelete.Enabled = btnEdit.Enabled = true;
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
    }
}
