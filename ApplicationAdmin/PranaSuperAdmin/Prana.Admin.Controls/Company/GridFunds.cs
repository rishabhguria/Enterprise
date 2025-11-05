using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridAccounts.
    /// </summary>
    public class GridAccounts : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "GridAccounts : ";
        private Infragistics.Win.UltraWinGrid.UltraGrid grdAccounts;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private IContainer components;

        public GridAccounts()
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
                if (grdAccounts != null)
                {
                    grdAccounts.Dispose();
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
                if (createClientAccount != null)
                {
                    createClientAccount.Dispose();
                }
                if (createClientAccount1 != null)
                {
                    createClientAccount1.Dispose();
                }
                if (createClientAccountEdit != null)
                {
                    createClientAccountEdit.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountName", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountShortName", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyPrimeBrokerClearerID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCustodianID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAdministratorID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAccountID", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyUserAccountID", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountTypeID", 9);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridAccounts));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdAccounts = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdAccounts);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(-2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounts";
            // 
            // grdAccounts
            // 
            this.grdAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAccounts.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 130;
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "False";
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.Caption = "Name";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 122;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "False";
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.Caption = "ShortName";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 173;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 71;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn5.Width = 74;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.Width = 54;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn7.Width = 87;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn8.Width = 75;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn9.Width = 73;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn10.Width = 75;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccounts.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdAccounts.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAccounts.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdAccounts.Location = new System.Drawing.Point(6, 20);
            this.grdAccounts.Name = "grdAccounts";
            this.grdAccounts.Size = new System.Drawing.Size(316, 148);
            this.grdAccounts.TabIndex = 66;
            this.grdAccounts.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(207, 176);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
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
            this.btnEdit.Location = new System.Drawing.Point(129, 176);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
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
            this.btnCreate.Location = new System.Drawing.Point(51, 176);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GridAccounts
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "GridAccounts";
            this.Size = new System.Drawing.Size(332, 204);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCreate;


        //private Prana.Admin.BLL.Accounts _accounts = new Accounts();
        public Prana.Admin.BLL.Accounts CurrentAccounts
        {
            get
            {
                foreach (Prana.Admin.BLL.Account account in (Prana.Admin.BLL.Accounts)grdAccounts.DataSource)
                {
                    if (account.AccountName.ToString() == "")
                    {
                        _nullRow = true;
                        //						Prana.Admin.BLL.Accounts nullAccounts = new Accounts();
                        //						grdAccounts.DataSource = nullAccounts;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                //return (Prana.Admin.BLL.Accounts) grdAccounts.DataSource; 
                Accounts currentAccounts = (Prana.Admin.BLL.Accounts)grdAccounts.DataSource;
                if (currentAccounts.Count > 0)
                {
                    Accounts saveAccounts = new Accounts();
                    Accounts companyAccounts = (Prana.Admin.BLL.Accounts)grdAccounts.DataSource;
                    foreach (Account companyAccount in companyAccounts)
                    {
                        if (companyAccount.AccountName != "")
                        {
                            saveAccounts.Add(companyAccount);
                        }
                    }

                    return saveAccounts;
                    //return (Prana.Admin.BLL.ClientAccounts) grdClientAccounts.DataSource; 
                }
                else
                {
                    currentAccounts = null;
                    return currentAccounts;
                }
            }
        }

        private CreateClientAccount createClientAccount = null;
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (createClientAccount == null)
            {
                createClientAccount = new CreateClientAccount();
            }
            Prana.Admin.BLL.Accounts accounts = new Prana.Admin.BLL.Accounts();
            createClientAccount.CurrentAccounts = (Prana.Admin.BLL.Accounts)grdAccounts.DataSource;
            foreach (Prana.Admin.BLL.Account account in createClientAccount.CurrentAccounts)
            {
                if (account.AccountName.ToString() == "")
                {
                    createClientAccount.NoData = 1;
                }
                else
                {
                    createClientAccount.NoData = 0;
                    break;
                }
            }
            createClientAccount.ShowDialog(this.Parent);
            accounts = createClientAccount.CurrentAccounts;
            //grdAccounts.DataSource = createClientAccount.CurrentAccounts; 
            grdAccounts.DataSource = null;
            grdAccounts.DataSource = accounts;
            grdAccounts.Refresh();
            if (accounts.Count > 0)
            {
                RefreshGrid();
            }
            else
            {
                BindDataGrid();
            }

            //            if(createClientAccount.CurrentAccounts.Count > 0 )
            //            {
            ////				grdAccounts.Select(0);
            //                _nullRow = false;
            //            }
            foreach (Prana.Admin.BLL.Account account in createClientAccount.CurrentAccounts)
            {
                if (account.AccountName.ToString() != "")
                {
                    _nullRow = false;
                    break;
                }
            }
        }

        private void RefreshGrid()
        {
            grdAccounts.DisplayLayout.Bands[0].Columns["AccountName"].Header.VisiblePosition = 0;
            grdAccounts.DisplayLayout.Bands[0].Columns["AccountShortName"].Header.VisiblePosition = 1;

            grdAccounts.DisplayLayout.Bands[0].Columns["AccountID"].Hidden = true;
            grdAccounts.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
            grdAccounts.DisplayLayout.Bands[0].Columns["CompanyPrimeBrokerClearerID"].Hidden = true;
            grdAccounts.DisplayLayout.Bands[0].Columns["CompanyCustodianID"].Hidden = true;
            grdAccounts.DisplayLayout.Bands[0].Columns["CompanyAdministratorID"].Hidden = true;
            grdAccounts.DisplayLayout.Bands[0].Columns["CompanyAccountID"].Hidden = true;
            grdAccounts.DisplayLayout.Bands[0].Columns["CompanyUserAccountID"].Hidden = true;
            grdAccounts.DisplayLayout.Bands[0].Columns["AccountTypeID"].Hidden = true;

            grdAccounts.DisplayLayout.Bands[0].Columns["AccountName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
            grdAccounts.DisplayLayout.Bands[0].Columns["AccountShortName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;

            grdAccounts.DisplayLayout.Bands[0].Columns["AccountName"].Header.Caption = "Name";
            grdAccounts.DisplayLayout.Bands[0].Columns["AccountShortName"].Header.Caption = "ShortName";
        }

        //Company id is used to pass itself as an argument to fetch the details 
        //corresponding to it from the database and bind the datagrid.
        private int _companyID = int.MinValue;
        public int CompanyID
        {
            get { return _companyID; }
            //            set
            //            {
            ////				_companyID = value;
            ////				BindDataGrid();
            //            }
        }

        private int _accountID = int.MinValue;
        public int AccountID
        {
            get { return _accountID; }
            //			set
            //			{
            //				_accountID = value;
            //				BindDataGrid();
            //			}
        }

        public Account AccountProperty
        {
            get
            {
                Account account = new Account();
                GetAccount(account);
                return account;
            }
            //			set 
            //			{
            //				SetAccount(value);
            //					
            //			}
        }

        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            //_accountID = accountID;
            BindDataGrid();
        }

        public void GetAccount(Account account)
        {
            account = (Account)grdAccounts.DataSource;
        }

        public void SetAccount(Account account)
        {
            grdAccounts.DataSource = account;
        }

        private CreateClientAccount createClientAccount1 = new CreateClientAccount();
        private void BindDataGrid()
        {
            try
            {
                Account nullAccount = new Account(int.MinValue, "");
                Prana.Admin.BLL.Accounts accounts = CompanyManager.GetAccount(_companyID);
                if (accounts.Count <= 0)
                {
                    accounts.Add(nullAccount);
                    _nullRow = true;
                }
                else
                {
                    _nullRow = false;
                }
                grdAccounts.DataSource = accounts;
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
            CreateClientAccount createClientAccount = new CreateClientAccount();
            if (grdAccounts.Rows.Count > 0)
            {
                string accountName = grdAccounts.ActiveRow.Cells["AccountName"].Text.ToString();
                if (accountName != "")
                {
                    if (MessageBox.Show(this, "Do you want to delete this Account?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //int accountID = int.Parse(grdAccounts.ActiveRow.Cells["AccountID"].Text.ToString());
                        int accountID = int.Parse(grdAccounts.ActiveRow.Cells["CompanyAccountID"].Text.ToString());
                        Prana.Admin.BLL.Accounts accounts = (Accounts)grdAccounts.DataSource;
                        Prana.Admin.BLL.Account account = new Prana.Admin.BLL.Account();

                        account.AccountID = int.Parse(grdAccounts.ActiveRow.Cells["AccountID"].Text.ToString());
                        account.AccountName = grdAccounts.ActiveRow.Cells["AccountName"].Text.ToString();
                        account.AccountShortName = grdAccounts.ActiveRow.Cells["AccountShortName"].Text.ToString();
                        account.CompanyID = int.Parse(grdAccounts.ActiveRow.Cells["CompanyID"].Text.ToString());

                        //accounts.RemoveAt(accounts.IndexOf(account));					
                        if (accountID != int.MinValue)
                        {
                            result = CompanyManager.DeleteAccount(accountID);
                            if (result == false)
                            {
                                MessageBox.Show("The Account is referenced in the Client Application, can not be deleted.", "Alert", MessageBoxButtons.OK);
                                errorProvider1.SetError(btnDelete, "The selected account is referred by some Client Broker Venue. Please unallocate that first before deleting it !");
                            }
                            else
                            {
                                accounts.RemoveAt(accounts.IndexOf(account));
                            }
                        }
                        else
                        {
                            accounts.RemoveAt(accounts.IndexOf(account));
                            //errorProvider1.SetError(btnDelete, "There is nothing do delete in the Grid!");
                        }

                        Accounts newAccounts = new Accounts();
                        foreach (Account tempAccount in accounts)
                        {
                            newAccounts.Add(tempAccount);
                        }
                        if (accounts.Count <= 0)
                        {
                            newAccounts.Add(new Account(int.MinValue, ""));
                            createClientAccount.NoData = 1;
                            _nullRow = true;
                        }
                        else
                        {
                            createClientAccount.NoData = 0;
                            _nullRow = false;
                        }
                        grdAccounts.DataSource = newAccounts;
                    }
                }
            }
        }

        private CreateClientAccount createClientAccountEdit = null;
        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (createClientAccountEdit == null)
            {
                if ((grdAccounts.Rows.Count > 0) && (_nullRow == false))
                {
                    createClientAccountEdit = new CreateClientAccount();

                    createClientAccountEdit.AccountEdit = (Account)((Accounts)grdAccounts.DataSource)[grdAccounts.ActiveRow.Index];

                    createClientAccountEdit.CurrentAccounts = (Prana.Admin.BLL.Accounts)grdAccounts.DataSource;
                    createClientAccountEdit.ShowDialog(this.Parent);
                    createClientAccountEdit = null;
                }
            }
        }
    }
}
