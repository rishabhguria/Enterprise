#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridClientAccounts.
    /// </summary>
    public class GridClientAccounts : System.Windows.Forms.UserControl
    {
        #region Private members

        private const string FORM_NAME = "GridClientAccounts : ";
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdClientAccounts;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        public GridClientAccounts()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
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
                if (createCompanyClientAccount != null)
                {
                    createCompanyClientAccount.Dispose();
                }
                if (createCompanyClientAccountEdit != null)
                {
                    createCompanyClientAccountEdit.Dispose();
                }
                if (grdClientAccounts != null)
                {
                    grdClientAccounts.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridClientAccounts));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientAccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientAccountName", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientAccountShortName", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientID", 3);
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdClientAccounts = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClientAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCreate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreate.BackgroundImage")));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCreate.Location = new System.Drawing.Point(102, 170);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(258, 170);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(180, 170);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdClientAccounts);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(2, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Accounts";
            // 
            // grdClientAccounts
            // 
            this.grdClientAccounts.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdClientAccounts.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 299;
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 162;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 225;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 73;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdClientAccounts.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdClientAccounts.DisplayLayout.GroupByBox.Hidden = true;
            this.grdClientAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.grdClientAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdClientAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdClientAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdClientAccounts.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdClientAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdClientAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdClientAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdClientAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdClientAccounts.Location = new System.Drawing.Point(4, 19);
            this.grdClientAccounts.Name = "grdClientAccounts";
            this.grdClientAccounts.Size = new System.Drawing.Size(408, 136);
            this.grdClientAccounts.TabIndex = 53;
            this.grdClientAccounts.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // GridClientAccounts
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "GridClientAccounts";
            this.Size = new System.Drawing.Size(434, 202);
            this.Load += new System.EventHandler(this.GridClientAccounts_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClientAccounts)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private CreateCompanyClientAccount createCompanyClientAccount = null;
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (createCompanyClientAccount == null)
            {
                createCompanyClientAccount = new CreateCompanyClientAccount();
            }
            Prana.Admin.BLL.ClientAccounts clientAccounts = new Prana.Admin.BLL.ClientAccounts();
            createCompanyClientAccount.CurrentClientAccounts = (Prana.Admin.BLL.ClientAccounts)grdClientAccounts.DataSource;

            foreach (Prana.Admin.BLL.ClientAccount clientAccount in createCompanyClientAccount.CurrentClientAccounts)
            {
                if (clientAccount.CompanyClientAccountName.ToString() == "")
                {
                    createCompanyClientAccount.NoData = 1;
                }
                else
                {
                    createCompanyClientAccount.NoData = 0;
                    break;
                }
            }

            createCompanyClientAccount.ShowDialog(this.Parent);
            grdClientAccounts.Refresh();
            clientAccounts = createCompanyClientAccount.CurrentClientAccounts;
            //grdClientAccounts.DataSource = createCompanyClientAccount.CurrentClientAccounts; 
            ClientAccount nullClientAccount = new ClientAccount(int.MinValue, "");
            if (clientAccounts.Count == 0)
            {
                clientAccounts.Add(nullClientAccount);
            }
            grdClientAccounts.DataSource = clientAccounts;
            //			if(clientAccounts.Count > 0)
            //			{
            grdClientAccounts.DisplayLayout.Bands[0].Columns["CompanyClientAccountID"].Hidden = true;
            grdClientAccounts.DisplayLayout.Bands[0].Columns["CompanyClientID"].Hidden = true;
            grdClientAccounts.DisplayLayout.Bands[0].Columns["CompanyClientAccountName"].Header.Caption = "Client Account Name";
            grdClientAccounts.DisplayLayout.Bands[0].Columns["CompanyClientAccountShortName"].Header.Caption = "Client Account Short Name";

            //			}

            //            if(createCompanyClientAccount.CurrentClientAccounts.Count > 0 )
            //            {
            ////				grdClientAccounts.Select(0);
            //                _nullRow = false;
            //            }
            foreach (Prana.Admin.BLL.ClientAccount clientAccount in createCompanyClientAccount.CurrentClientAccounts)
            {
                if (clientAccount.CompanyClientAccountName.ToString() != "")
                {
                    _nullRow = false;
                    break;
                }
            }

        }

        public Prana.Admin.BLL.ClientAccounts CurrentCompanyClientAccounts
        {
            get
            {
                foreach (Prana.Admin.BLL.ClientAccount clientAccount in (Prana.Admin.BLL.ClientAccounts)grdClientAccounts.DataSource)
                {
                    if (clientAccount.CompanyClientAccountName.ToString() == "")
                    {
                        _nullRow = true;
                        //						Prana.Admin.BLL.ClientAccounts nullClientAccounts = new ClientAccounts();
                        //						grdClientAccounts.DataSource = nullClientAccounts;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                ClientAccounts currentClientAccounts = (Prana.Admin.BLL.ClientAccounts)grdClientAccounts.DataSource;
                if (currentClientAccounts.Count > 0)
                {
                    ClientAccounts saveClientAccounts = new ClientAccounts();
                    ClientAccounts clientAccounts = (Prana.Admin.BLL.ClientAccounts)grdClientAccounts.DataSource;
                    foreach (ClientAccount clientAccount in clientAccounts)
                    {
                        if (clientAccount.CompanyClientAccountName != "")
                        {
                            saveClientAccounts.Add(clientAccount);
                        }
                    }

                    return saveClientAccounts;
                    //return (Prana.Admin.BLL.ClientAccounts) grdClientAccounts.DataSource; 
                }
                else
                {
                    currentClientAccounts = null;
                    return currentClientAccounts;
                }
            }
        }

        public void SetupControl(int companyClientID)
        {
            _companyClientID = companyClientID;
            BindDataGrid();
        }

        private int _companyClientID = int.MinValue;
        public int CompanyClientID
        {
            get { return _companyClientID; }
            //			set
            //			{
            //				_companyClientID = value;
            //				BindDataGrid();
            //			}
        }

        private int _currentCompanyClientAccountID = int.MinValue;
        public int CurrentCompanyClientAccountID
        {
            get { return _currentCompanyClientAccountID; }
            set
            {
                _currentCompanyClientAccountID = value;
                //BindDataGrid();
            }
        }

        public ClientAccount CompanyClientAccountProperty
        {
            get
            {
                ClientAccount clientAccount = new ClientAccount();
                GetClientAccount(clientAccount);
                return clientAccount;
            }
            set
            {
                SetClientAccount(value);

            }
        }

        public void GetClientAccount(ClientAccount clientAccount)
        {
            clientAccount = (ClientAccount)grdClientAccounts.DataSource;
        }

        public void SetClientAccount(ClientAccount clientAccount)
        {
            //grdClientAccounts.DataSource = clientAccount;
        }

        private void BindDataGrid()
        {
            try
            {
                ClientAccount nullClientAccount = new ClientAccount(int.MinValue, "");
                Prana.Admin.BLL.ClientAccounts clientAccounts = ClientAccountManager.GetCompanyClientAccounts(_companyClientID);
                if (clientAccounts.Count <= 0)
                {
                    clientAccounts.Add(nullClientAccount);
                    _nullRow = true;
                }
                else
                {
                    _nullRow = false;
                }
                grdClientAccounts.DataSource = null;
                grdClientAccounts.DataSource = clientAccounts;

                ColumnsCollection columnsClientAccounts = grdClientAccounts.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columnsClientAccounts)
                {
                    if (column.Key == "CompanyClientID" || column.Key == "CompanyClientAccountID")
                    {
                        column.Hidden = true;
                    }
                }
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
            if (grdClientAccounts.ActiveRow == null)
            {
                return;
            }
            CreateCompanyClientAccount createCompanyClientAccount = new CreateCompanyClientAccount();
            if (grdClientAccounts.Rows.Count > 0)
            {
                string companyClientAccountName = grdClientAccounts.ActiveRow.Cells["CompanyClientAccountName"].Text.ToString();
                if (companyClientAccountName != "")
                {
                    if (MessageBox.Show(this, "Do you want to delete this Client Account?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int clientAccountID = int.Parse(grdClientAccounts.ActiveRow.Cells["CompanyClientAccountID"].Text.ToString());

                        Prana.Admin.BLL.ClientAccounts clientAccounts = (Prana.Admin.BLL.ClientAccounts)grdClientAccounts.DataSource;
                        Prana.Admin.BLL.ClientAccount clientAccount = new Prana.Admin.BLL.ClientAccount();

                        clientAccount.CompanyClientAccountID = int.Parse(grdClientAccounts.ActiveRow.Cells["CompanyClientAccountID"].Text.ToString());
                        clientAccount.CompanyClientAccountName = grdClientAccounts.ActiveRow.Cells["CompanyClientAccountName"].Text.ToString();
                        clientAccount.CompanyClientAccountShortName = grdClientAccounts.ActiveRow.Cells["CompanyClientAccountShortName"].Text.ToString();
                        clientAccount.CompanyClientID = int.Parse(grdClientAccounts.ActiveRow.Cells["CompanyClientID"].Text.ToString());

                        clientAccounts.RemoveAt(clientAccounts.IndexOf(clientAccount));
                        if (clientAccountID != int.MinValue)
                        {
                            ClientAccountManager.DeleteClientAccount(clientAccountID);
                        }

                        ClientAccounts newClientAccounts = new ClientAccounts();
                        foreach (ClientAccount tempClientAccount in clientAccounts)
                        {
                            newClientAccounts.Add(tempClientAccount);
                        }
                        if (clientAccounts.Count <= 0)
                        {
                            newClientAccounts.Add(new ClientAccount(int.MinValue, ""));
                            createCompanyClientAccount.NoData = 1;
                            _nullRow = true;
                        }
                        else
                        {
                            createCompanyClientAccount.NoData = 0;
                            _nullRow = false;
                        }
                        grdClientAccounts.DataSource = newClientAccounts;
                    }
                }
            }
        }

        private CreateCompanyClientAccount createCompanyClientAccountEdit = null;
        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (grdClientAccounts.ActiveRow == null)
            {
                return;
            }
            if (createCompanyClientAccountEdit == null)
            {
                if ((grdClientAccounts.Rows.Count > 0) && (_nullRow == false))
                {
                    createCompanyClientAccountEdit = new CreateCompanyClientAccount();
                    createCompanyClientAccountEdit.Text = "Edit Client Account";
                    createCompanyClientAccountEdit.ClientAccountEdit = (ClientAccount)((Prana.Admin.BLL.ClientAccounts)grdClientAccounts.DataSource)[grdClientAccounts.ActiveRow.Index];

                    createCompanyClientAccountEdit.CurrentClientAccounts = (Prana.Admin.BLL.ClientAccounts)grdClientAccounts.DataSource;
                    createCompanyClientAccountEdit.ShowDialog(this.Parent);
                    createCompanyClientAccountEdit = null;
                }
            }
        }

        private void GridClientAccounts_Load(object sender, System.EventArgs e)
        {

        }
    }
}
