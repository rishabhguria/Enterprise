using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridClientTradingAccounts.
    /// </summary>
    public class GridClientTradingAccounts : System.Windows.Forms.UserControl
    {
        #region private members
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;

        private CompanyClientTradingAccounts _companyClientTradingAccounts;

        private bool bAlreadyCalled = false;
        CompanyClientTradingAccount nullcompanyClientTradingAccount;
        private const string FORM_NAME = "GridClientTradingAccounts : ";
        private int _companyClientID;
        private int _companyID;
        private CompanyClientTradingAccount _companyClientTradingAccount;
        CreateCompanyClientTradingAccount createCompanyClientTradingAccount = null;
        private Traders _traders;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdClientTradingAccounts;
        private string _addedTradersIDS;
        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
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
                if (createCompanyClientTradingAccount != null)
                {
                    createCompanyClientTradingAccount.Dispose();
                }
                if (grdClientTradingAccounts != null)
                {
                    grdClientTradingAccounts.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Constructor and  SetUp
        public GridClientTradingAccounts()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            nullcompanyClientTradingAccount = new CompanyClientTradingAccount(int.MinValue, "", int.MinValue, "", int.MinValue, int.MinValue, "");

            // TODO: Add any initialization after the InitializeComponent call

        }

        public void SetUp(int companyID, int companyClientID, Traders traders)
        {
            _traders = traders;
            _companyID = companyID;
            _companyClientID = companyClientID;

            _companyClientTradingAccounts = CompanyClientTradingAccountManager.GetCompanyClientTradingAccount(CompanyClientID);
            if (!bAlreadyCalled)
            {

                SetUpDataGrid();
                bAlreadyCalled = true;
            }

            BindDataGrid();



        }

        #endregion

        #region grid Setting
        private void SetUpDataGrid()
        {
            //			
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns.Add("CompClientTradingAccount", "Trading Account");
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns.Add("CompanyTradingAccountID", "CompanyTradingAccountID");
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns.Add("CompanyTradingAccountName", "Trading Account Name");
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns.Add("CompanyClientID", "CompanyClientID");
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns.Add("ClientTraderID", "ClientTraderID");
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns.Add("ClientTraderShortName", "Trader Short Name");
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns.Add("CompanyClientTradingAccountID", "CompanyClientTradingAccountID");
            //		    
            //			//Position
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns["CompClientTradingAccount"].Header.VisiblePosition = 0;
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns["ClientTraderShortName"].Header.VisiblePosition = 2;
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns["CompanyTradingAccountName"].Header.VisiblePosition = 1;
            //hiding
            HideColumns();


        }

        private void BindDataGrid()
        {
            try
            {
                if (_companyClientTradingAccounts.Count > 0
                    && _companyClientTradingAccounts.Contains(nullcompanyClientTradingAccount))
                    _companyClientTradingAccounts.Remove(nullcompanyClientTradingAccount);

                if (_companyClientTradingAccounts.Count == 0)
                    _companyClientTradingAccounts.Add(nullcompanyClientTradingAccount);
                grdClientTradingAccounts.DataSource = _companyClientTradingAccounts;
                grdClientTradingAccounts.DataBind();
                foreach (Trader trader in _traders)
                {
                    foreach (CompanyClientTradingAccount companyClientTradingAccount in _companyClientTradingAccounts)
                    {
                        if (trader.ShortName == companyClientTradingAccount.ClientTraderShortName)
                        {
                            trader.SetReference(true);
                            break;

                        }
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

        private void HideColumns()
        {
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns["CompanyTradingAccountID"].Hidden = true;
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns["CompanyClientID"].Hidden = true;
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns["CompanyClientTradingAccountID"].Hidden = true;
            grdClientTradingAccounts.DisplayLayout.Bands[0].Columns["ClientTraderID"].Hidden = true;


        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridClientTradingAccounts));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.grdClientTradingAccounts = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdClientTradingAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCreate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreate.BackgroundImage")));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCreate.Location = new System.Drawing.Point(78, 142);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
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
            this.btnDelete.Location = new System.Drawing.Point(232, 142);
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
            this.btnEdit.Location = new System.Drawing.Point(155, 142);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // grdClientTradingAccounts
            // 
            this.grdClientTradingAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdClientTradingAccounts.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdClientTradingAccounts.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdClientTradingAccounts.DisplayLayout.GroupByBox.Hidden = true;
            this.grdClientTradingAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.grdClientTradingAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdClientTradingAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdClientTradingAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdClientTradingAccounts.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdClientTradingAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdClientTradingAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdClientTradingAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdClientTradingAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdClientTradingAccounts.Location = new System.Drawing.Point(4, 8);
            this.grdClientTradingAccounts.Name = "grdClientTradingAccounts";
            this.grdClientTradingAccounts.Size = new System.Drawing.Size(374, 118);
            this.grdClientTradingAccounts.TabIndex = 0;
            this.grdClientTradingAccounts.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // GridClientTradingAccounts
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grdClientTradingAccounts);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Name = "GridClientTradingAccounts";
            this.Size = new System.Drawing.Size(384, 168);
            ((System.ComponentModel.ISupportInitialize)(this.grdClientTradingAccounts)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region  Events

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            try
            {

                createCompanyClientTradingAccount = new CreateCompanyClientTradingAccount(_companyID, _companyClientID);
                createCompanyClientTradingAccount.ISAddition = true;
                createCompanyClientTradingAccount.Traders = _traders;
                createCompanyClientTradingAccount.ShowDialog(this);
                _traders.RemoveAt(0);
                if (createCompanyClientTradingAccount.companyClientTradingAccount != null && !_companyClientTradingAccounts.Contains(createCompanyClientTradingAccount.companyClientTradingAccount))

                    _companyClientTradingAccounts.Add(createCompanyClientTradingAccount.companyClientTradingAccount);
                createCompanyClientTradingAccount.Dispose();
                BindDataGrid();
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

        }
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            int clientTraderID = int.MinValue;
            int companyClientID = int.MinValue;
            string clientTraderShortName = "";
            string companyTradingAccountName = "";
            int companyTradingAccountID = int.MinValue;
            string companyClientTradingAccount = string.Empty;

            if (grdClientTradingAccounts.ActiveRow == null)
            {
                clientTraderID = int.Parse(grdClientTradingAccounts.Rows[0].Cells["ClientTraderID"].Value.ToString().Trim());
                companyClientID = int.Parse(grdClientTradingAccounts.Rows[0].Cells["CompanyClientID"].Value.ToString().Trim());
                clientTraderShortName = grdClientTradingAccounts.Rows[0].Cells["ClientTraderShortName"].Value.ToString().Trim();
                companyTradingAccountName = grdClientTradingAccounts.Rows[0].Cells["CompanyTradingAccountName"].Value.ToString().Trim();
                companyTradingAccountID = int.Parse(grdClientTradingAccounts.Rows[0].Cells["CompanyTradingAccountID"].Value.ToString().Trim());
                companyClientTradingAccount = grdClientTradingAccounts.Rows[0].Cells["CompClientTradingAccount"].Value.ToString().Trim();

            }
            else
            {
                clientTraderID = int.Parse(grdClientTradingAccounts.ActiveRow.Cells["ClientTraderID"].Value.ToString().Trim());
                companyClientID = int.Parse(grdClientTradingAccounts.ActiveRow.Cells["CompanyClientID"].Value.ToString().Trim());
                clientTraderShortName = grdClientTradingAccounts.ActiveRow.Cells["ClientTraderShortName"].Value.ToString().Trim();
                companyTradingAccountName = grdClientTradingAccounts.ActiveRow.Cells["CompanyTradingAccountName"].Value.ToString().Trim();
                companyTradingAccountID = int.Parse(grdClientTradingAccounts.ActiveRow.Cells["CompanyTradingAccountID"].Value.ToString().Trim());
                companyClientTradingAccount = grdClientTradingAccounts.ActiveRow.Cells["CompClientTradingAccount"].Value.ToString().Trim();
            }
            //Blank Row 
            if (companyClientTradingAccount == string.Empty)
                return;
            //Prompt for Deletion
            if (MessageBox.Show(this, "Do you want to delete this Trading Account Mapping?", "Alert", MessageBoxButtons.YesNo) == DialogResult.No)
                return;




            CompanyClientTradingAccount tempcompanyClientTradingAccount = new CompanyClientTradingAccount(companyClientTradingAccount, companyTradingAccountID, companyTradingAccountName, companyClientID, clientTraderID, clientTraderShortName);
            foreach (Trader trader in _traders)
            {

                if (trader.ShortName == tempcompanyClientTradingAccount.ClientTraderShortName)
                {
                    trader.SetReference(false);

                }

            }
            _companyClientTradingAccounts.RemoveAt(_companyClientTradingAccounts.IndexOf(tempcompanyClientTradingAccount));
            BindDataGrid();


        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {

                if (grdClientTradingAccounts.ActiveRow == null || grdClientTradingAccounts.ActiveRow.Cells["CompClientTradingAccount"].Value.ToString() == "")
                {
                    return;
                }
                int clientTraderID = int.Parse(grdClientTradingAccounts.ActiveRow.Cells["ClientTraderID"].Value.ToString());
                int companyClientID = int.Parse(grdClientTradingAccounts.ActiveRow.Cells["CompanyClientID"].Value.ToString());
                string clientTraderShortName = grdClientTradingAccounts.ActiveRow.Cells["ClientTraderShortName"].Value.ToString();
                string companyTradingAccountName = grdClientTradingAccounts.ActiveRow.Cells["CompanyTradingAccountName"].Value.ToString();
                int companyTradingAccountID = int.Parse(grdClientTradingAccounts.ActiveRow.Cells["CompanyTradingAccountID"].Value.ToString());
                string companyClientTradingAccount = grdClientTradingAccounts.ActiveRow.Cells["CompClientTradingAccount"].Value.ToString();


                _companyClientTradingAccount = new CompanyClientTradingAccount(companyClientTradingAccount, companyTradingAccountID, companyTradingAccountName, companyClientID, clientTraderID, clientTraderShortName);

                createCompanyClientTradingAccount = new CreateCompanyClientTradingAccount(_companyClientTradingAccount, _companyID);
                createCompanyClientTradingAccount.Traders = _traders;
                createCompanyClientTradingAccount.ISAddition = false;
                createCompanyClientTradingAccount.Text = "Edit Client Trading Account Mapping";

                createCompanyClientTradingAccount.ShowDialog(this);
                _traders.RemoveAt(0);
                _companyClientTradingAccounts.RemoveAt(_companyClientTradingAccounts.IndexOf(_companyClientTradingAccount));
                _companyClientTradingAccount = null;
                if (!_companyClientTradingAccounts.Contains(createCompanyClientTradingAccount.companyClientTradingAccount))
                    _companyClientTradingAccounts.Add(createCompanyClientTradingAccount.companyClientTradingAccount);
                createCompanyClientTradingAccount.Dispose();
                BindDataGrid();
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
        }


        #endregion

        #region  Get Set Properties
        public int CompanyClientID
        {
            get { return _companyClientID; }
            //			set
            //			{
            //				_companyClientID = value;
            //				
            //			}
        }
        public int CompanyID
        {
            get { return _companyID; }
            //			set
            //			{
            //				_companyID = value;
            //				
            //			}
        }

        public string AddedTradersIDS
        {
            get { return _addedTradersIDS; ; }
            set { _addedTradersIDS = value; }

        }
        public CompanyClientTradingAccounts CompanyClientTradingAccounts
        {
            get
            {
                CompanyClientTradingAccounts companyClientTradingAccounts = new CompanyClientTradingAccounts();
                foreach (CompanyClientTradingAccount companyClientTradingAccount in _companyClientTradingAccounts)
                {
                    if (!companyClientTradingAccount.Equal(nullcompanyClientTradingAccount))
                        companyClientTradingAccounts.Add(companyClientTradingAccount);
                }

                return companyClientTradingAccounts;
            }
            set { _companyClientTradingAccounts = value; }

        }

        #endregion

    }
}
