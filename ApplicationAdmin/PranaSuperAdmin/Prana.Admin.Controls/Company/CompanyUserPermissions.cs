#region Using
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyUserPermissions.
    /// </summary>
    public class CompanyUserPermissionsOld : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "CompanyUserPermissions : ";
        #region private and protected members

        private Infragistics.Win.Misc.UltraGroupBox grpCounterParties;
        private Infragistics.Win.Misc.UltraGroupBox grpApplicationComponent;
        private Infragistics.Win.Misc.UltraGroupBox grpAssetUnderlying;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.ListBox lstApplicationComponent;
        private System.Windows.Forms.ListBox lstAssetUnderLying;
        private System.Windows.Forms.ListBox lstTradingAccount;

        private int _companyID = int.MinValue;

        #endregion
        private System.Windows.Forms.ListBox lstCounterParties;


        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public int CompanyID
        {
            set { _companyID = value; }
        }

        public CompanyUserPermissionsOld()
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
                if (grpCounterParties != null)
                {
                    grpCounterParties.Dispose();
                }
                if (grpApplicationComponent != null)
                {
                    grpApplicationComponent.Dispose();
                }
                if (grpAssetUnderlying != null)
                {
                    grpAssetUnderlying.Dispose();
                }
                if (ultraGroupBox1 != null)
                {
                    ultraGroupBox1.Dispose();
                }
                if (lstApplicationComponent != null)
                {
                    lstApplicationComponent.Dispose();
                }
                if (lstAssetUnderLying != null)
                {
                    lstAssetUnderLying.Dispose();
                }
                if (lstTradingAccount != null)
                {
                    lstTradingAccount.Dispose();
                }
                if (lstCounterParties != null)
                {
                    lstCounterParties.Dispose();
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
            this.grpCounterParties = new Infragistics.Win.Misc.UltraGroupBox();
            this.lstCounterParties = new System.Windows.Forms.ListBox();
            this.grpApplicationComponent = new Infragistics.Win.Misc.UltraGroupBox();
            this.lstApplicationComponent = new System.Windows.Forms.ListBox();
            this.grpAssetUnderlying = new Infragistics.Win.Misc.UltraGroupBox();
            this.lstAssetUnderLying = new System.Windows.Forms.ListBox();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lstTradingAccount = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.grpCounterParties)).BeginInit();
            this.grpCounterParties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpApplicationComponent)).BeginInit();
            this.grpApplicationComponent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpAssetUnderlying)).BeginInit();
            this.grpAssetUnderlying.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCounterParties
            // 
            this.grpCounterParties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCounterParties.Controls.Add(this.lstCounterParties);
            this.grpCounterParties.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCounterParties.Location = new System.Drawing.Point(6, 4);
            this.grpCounterParties.Name = "grpCounterParties";
            this.grpCounterParties.Size = new System.Drawing.Size(248, 194);
            this.grpCounterParties.TabIndex = 0;
            this.grpCounterParties.Text = "Brokers";
            // 
            // lstCounterParties
            // 
            this.lstCounterParties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCounterParties.Enabled = false;
            this.lstCounterParties.Location = new System.Drawing.Point(6, 18);
            this.lstCounterParties.Name = "lstCounterParties";
            this.lstCounterParties.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstCounterParties.Size = new System.Drawing.Size(238, 160);
            this.lstCounterParties.TabIndex = 0;
            // 
            // grpApplicationComponent
            // 
            this.grpApplicationComponent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpApplicationComponent.Controls.Add(this.lstApplicationComponent);
            this.grpApplicationComponent.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpApplicationComponent.Location = new System.Drawing.Point(270, 4);
            this.grpApplicationComponent.Name = "grpApplicationComponent";
            this.grpApplicationComponent.Size = new System.Drawing.Size(248, 194);
            this.grpApplicationComponent.TabIndex = 1;
            this.grpApplicationComponent.Text = "Application Component";
            // 
            // lstApplicationComponent
            // 
            this.lstApplicationComponent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstApplicationComponent.Location = new System.Drawing.Point(6, 20);
            this.lstApplicationComponent.Name = "lstApplicationComponent";
            this.lstApplicationComponent.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstApplicationComponent.Size = new System.Drawing.Size(238, 160);
            this.lstApplicationComponent.TabIndex = 0;
            // 
            // grpAssetUnderlying
            // 
            this.grpAssetUnderlying.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAssetUnderlying.Controls.Add(this.lstAssetUnderLying);
            this.grpAssetUnderlying.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAssetUnderlying.Location = new System.Drawing.Point(6, 202);
            this.grpAssetUnderlying.Name = "grpAssetUnderlying";
            this.grpAssetUnderlying.Size = new System.Drawing.Size(248, 194);
            this.grpAssetUnderlying.TabIndex = 2;
            this.grpAssetUnderlying.Text = "Asset && Underlying";
            // 
            // lstAssetUnderLying
            // 
            this.lstAssetUnderLying.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAssetUnderLying.Location = new System.Drawing.Point(4, 16);
            this.lstAssetUnderLying.Name = "lstAssetUnderLying";
            this.lstAssetUnderLying.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstAssetUnderLying.Size = new System.Drawing.Size(238, 160);
            this.lstAssetUnderLying.TabIndex = 0;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox1.Controls.Add(this.lstTradingAccount);
            this.ultraGroupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox1.Location = new System.Drawing.Point(266, 202);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(248, 194);
            this.ultraGroupBox1.TabIndex = 3;
            this.ultraGroupBox1.Text = "Trading Account";
            // 
            // lstTradingAccount
            // 
            this.lstTradingAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTradingAccount.Location = new System.Drawing.Point(8, 16);
            this.lstTradingAccount.Name = "lstTradingAccount";
            this.lstTradingAccount.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstTradingAccount.Size = new System.Drawing.Size(238, 160);
            this.lstTradingAccount.TabIndex = 0;
            // 
            // CompanyUserPermissionsOld
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpCounterParties);
            this.Controls.Add(this.grpAssetUnderlying);
            this.Controls.Add(this.grpApplicationComponent);
            this.Controls.Add(this.ultraGroupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CompanyUserPermissionsOld";
            this.Size = new System.Drawing.Size(528, 410);
            this.Load += new System.EventHandler(this.CompanyUserPermissions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpCounterParties)).EndInit();
            this.grpCounterParties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpApplicationComponent)).EndInit();
            this.grpApplicationComponent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpAssetUnderlying)).EndInit();
            this.grpAssetUnderlying.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void CompanyUserPermissions_Load(object sender, System.EventArgs e)
        {
            try
            {
                //				BindCounterParties();
                //				BindComponents();
                //				BindAssetUnderlying();
                //				BindTradingAccount();
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

        #region Private methods

        //		private void BindCounterParties()
        //		{
        //			CounterParties counterParties = CounterPartyManager.GetCounterPartiesForCompanies();
        //			if(counterParties.Count > 0)
        //			{
        //				lstCounterParties.DataSource = counterParties;
        //				lstCounterParties.DisplayMember = "CounterPartyFullName";
        //				lstCounterParties.ValueMember = "CounterPartyID";
        //			}
        //		}		
        //
        //		private void BindComponents()
        //		{
        //			Modules modules = ModuleManager.GetModules();
        //			if(modules.Count > 0)
        //			{
        //				lstApplicationComponent.DataSource = modules;
        //				lstApplicationComponent.DisplayMember = "ModuleName";
        //				lstApplicationComponent.ValueMember = "ModuleID";
        //			}			
        //		}
        //
        //		private void BindAssetUnderlying()
        //		{
        //			UnderLyings underLyings = AssetManager.GetUnderLyings();
        //			if(underLyings.Count > 0)
        //			{
        //				System.Data.DataTable dt = new System.Data.DataTable();
        //				dt.Columns.Add("Data");
        //				dt.Columns.Add("Value");
        //				foreach(UnderLying underLying in underLyings)
        //				{
        //					object[] row = new object[2]; 
        //					row[0] = underLying.Asset + " : " + underLying.Name;
        //					row[1] = underLying.UnderlyingID;
        //					dt.Rows.Add(row);
        //				}
        //				lstAssetUnderLying.DataSource = dt; //underLyings;
        //				lstAssetUnderLying.DisplayMember = "Data";			
        //				lstAssetUnderLying.ValueMember = "Value";
        //			}
        //		}
        //
        //		
        //
        //		private void BindTradingAccount()
        //		{			
        //			TradingAccounts tradingAccounts = CompanyManager.GetTradingAccounts();
        //			if(tradingAccounts.Count > 0)
        //			{
        //				lstTradingAccount.DataSource = tradingAccounts;
        //				lstTradingAccount.DisplayMember = "TradingAccountName";
        //				lstTradingAccount.ValueMember = "TradingAccountsID";
        //			}
        //
        //		}		

        #endregion

        #region Public Properties
        //		public CounterParties CounterParties
        //		{
        //			get
        //			{
        //				return GetCounterParties();
        //			}
        //			set
        //			{
        //				SetCounterParties(value);
        //			}
        //		}

        //		public TradingAccounts TradingAccounts
        //		{
        //			get
        //			{
        //				return GetTradingAccounts();
        //			}
        //			set
        //			{
        //				SetTradingAccounts(value);
        //			}
        //		}
        //
        //		public Modules Modules
        //		{
        //			get
        //			{
        //				return GetModules();
        //			}
        //			set
        //			{
        //				SetModules(value);
        //			}
        //		}
        #endregion

        public void BindData(int companyID, int companyUserID)
        {
            //			if (userID != int.MinValue)
            //			{
            //				uctCompanyUserPermissions1.SetCounterPartiesForUser(userID);
            //				uctCompanyUserPermissions1.SetTradingAccountsForUser(userID);
            //				uctCompanyUserPermissions1.SetModulesForUser(userID);								
            //			}
            //			else
            //			{
            SetCounterParties(companyID, companyUserID);
            SetTradingAccounts(companyID, companyUserID);
            SetModules(companyID, companyUserID);
            SetAssetsUnderLyings(companyID, companyUserID);
            //			}
        }

        public CounterPartyVenues GetCounterPartyVenues()
        {
            Prana.Admin.BLL.CounterParties counterParties = new CounterParties();
            CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            for (int i = 0, count = lstCounterParties.SelectedItems.Count; i < count; i++)
            {
                System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)lstCounterParties.SelectedItems[i]).Row;
                int counterPartyID = int.Parse(selectedRow["CounterPartyID"].ToString());
                int venueID = int.Parse(selectedRow["VenueID"].ToString());
                CounterParty cp = new CounterParty(counterPartyID, "");
                if (!counterParties.Contains(cp))
                {
                    counterParties.Add(cp);
                }
                if (venueID > 0)
                {
                    CounterPartyVenue cpv = new CounterPartyVenue();
                    cpv.CounterPartyID = counterPartyID;
                    cpv.VenueID = venueID;
                    counterPartyVenues.Add(cpv);
                }
            }

            //			if(counterParties.Count > 0)
            //			{
            //				CounterPartyManager.SaveCompanyUserCounterParties(company.CompanyID, counterParties, userID);
            //			}

            return counterPartyVenues;



            #region Oldcode
            //			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            //			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
            //			{
            //				counterPartyVenues.Add((CounterPartyVenue)lstCounterParties.SelectedItems[i]);
            //			}
            //			return counterPartyVenues;
            #endregion
        }

        public CounterParties GetCounterParties()
        {
            Prana.Admin.BLL.CounterParties counterParties = new CounterParties();
            CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            for (int i = 0, count = lstCounterParties.SelectedItems.Count; i < count; i++)
            {
                System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)lstCounterParties.SelectedItems[i]).Row;
                int counterPartyID = int.Parse(selectedRow["CounterPartyID"].ToString());
                int venueID = int.Parse(selectedRow["VenueID"].ToString());
                CounterParty cp = new CounterParty(counterPartyID, "");
                if (!counterParties.Contains(cp))
                {
                    counterParties.Add(cp);
                }
                if (venueID > 0)
                {
                    CounterPartyVenue cpv = new CounterPartyVenue();
                    cpv.CounterPartyID = counterPartyID;
                    cpv.VenueID = venueID;
                    counterPartyVenues.Add(cpv);
                }
            }

            //			if(counterParties.Count > 0)
            //			{
            //				CounterPartyManager.SaveCompanyUserCounterParties(company.CompanyID, counterParties, userID);
            //			}

            return counterParties;



            #region Oldcode
            //			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            //			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
            //			{
            //				counterPartyVenues.Add((CounterPartyVenue)lstCounterParties.SelectedItems[i]);
            //			}
            //			return counterPartyVenues;
            #endregion
        }

        public void SetCounterParties(int companyID, int companyUserID)
        {
            lstCounterParties.Refresh();
            CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(companyID);
            CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);

            //CounterParties counterParties = CounterPartyManager.GetCompanyUserCounterParties(companyID, companyUserID);
            //CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);

            System.Data.DataTable tempDataTable = new System.Data.DataTable();
            tempDataTable.Columns.Add("DisplayData");
            tempDataTable.Columns.Add("CounterPartyID");
            tempDataTable.Columns.Add("VenueID");
            foreach (CounterParty counterParty in counterParties)
            {
                System.Data.DataRow row = tempDataTable.NewRow();
                row["DisplayData"] = counterParty.CounterPartyFullName;
                row["CounterPartyID"] = counterParty.CounterPartyID;
                row["VenueID"] = int.MinValue;
                tempDataTable.Rows.Add(row);
                foreach (Venue venue in counterParty.Venues)
                {
                    if (CounterPartyManager.CheckExistingUserCounterPartyVenue(counterParty.CounterPartyID, venue.VenueID, companyID) == true)
                    {
                        row = tempDataTable.NewRow();
                        row["DisplayData"] = "      " + venue.VenueName;
                        row["CounterPartyID"] = counterParty.CounterPartyID;
                        row["VenueID"] = venue.VenueID;
                        tempDataTable.Rows.Add(row);
                    }
                }
            }

            if (counterParties.Count > 0)
            {
                lstCounterParties.DataSource = null;
                lstCounterParties.Items.Clear();
                lstCounterParties.DataSource = tempDataTable;// counterParties;
                lstCounterParties.DisplayMember = "DisplayData";//"CounterPartyFullName";
                lstCounterParties.ValueMember = "CounterPartyID";//"CounterPartyID";						
            }
            else
            {
                lstCounterParties.DataSource = null;
            }


            CounterParties userCounterParties = CounterPartyManager.GetCompanyUserCounterParties(companyID, companyUserID);
            CounterPartyVenues userCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);

            lstCounterParties.SelectedIndex = -1;
            foreach (CounterParty userCounterParty in userCounterParties)
            {
                lstCounterParties.SelectedValue = userCounterParty.CounterPartyID;
            }

            System.Data.DataTable dt = (System.Data.DataTable)lstCounterParties.DataSource;
            int rowIndex = 0;
            counterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
            //			if (dt.Rows.Count > 0)
            //			{
            foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
            {
                rowIndex = 0;
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    if (int.Parse(row["CounterPartyID"].ToString()) == counterPartyVenue.CounterPartyID && int.Parse(row["VenueID"].ToString()) == counterPartyVenue.VenueID)
                    {
                        lstCounterParties.SelectedIndex = rowIndex;
                    }
                    rowIndex++;
                }
            }
            //			}


            //			if(companyUserID != int.MinValue)
            //			{
            //				CounterPartyVenues companyUserCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
            //				foreach(CounterPartyVenue counterPartyVenue  in companyUserCounterPartyVenues)
            //				{
            //					lstCounterParties.SelectedValue = counterPartyVenue.CounterPartyVenueID;
            //				}
            //			}



            #region Old Code
            //			CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
            //			
            //			if(counterPartyVenues.Count > 0)
            //			{
            //				lstCounterParties.DataSource = counterPartyVenues;
            //				lstCounterParties.DisplayMember = "DisplayName";
            //				lstCounterParties.ValueMember = "CounterPartyVenueID";
            //				lstCounterParties.SelectedIndex = -1;
            //						
            //				if(companyUserID != int.MinValue)
            //				{
            //					CounterPartyVenues companyUserCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
            //					foreach(CounterPartyVenue counterPartyVenue  in companyUserCounterPartyVenues)
            //					{
            //						lstCounterParties.SelectedValue = counterPartyVenue.CounterPartyVenueID;
            //					}
            //				}
            //			}
            #endregion
        }

        public TradingAccounts GetTradingAccounts()
        {
            TradingAccounts tradingAccounts = new TradingAccounts();
            for (int i = 0, count = lstTradingAccount.SelectedItems.Count; i < count; i++)
            {
                tradingAccounts.Add((TradingAccount)lstTradingAccount.SelectedItems[i]);
            }
            return tradingAccounts;
        }

        public void SetTradingAccounts(int companyID, int companyUserID)
        {
            lstTradingAccount.Refresh();
            TradingAccounts tradingaccounts = CompanyManager.GetTradingAccountsForCompany(companyID);
            if (tradingaccounts.Count > 0)
            {
                lstTradingAccount.DataSource = tradingaccounts;
                lstTradingAccount.DisplayMember = "TradingAccountName";
                lstTradingAccount.ValueMember = "TradingAccountsID";

                lstTradingAccount.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    TradingAccounts companyUserTradingaccounts = CompanyManager.GetTradingAccountsForUser(companyUserID);
                    foreach (TradingAccount tradingAccount in companyUserTradingaccounts)
                    {
                        lstTradingAccount.SelectedValue = tradingAccount.TradingAccountsID;
                    }
                }
            }
            else
            {
                lstTradingAccount.DataSource = null;
            }
        }

        public Modules GetModules()
        {
            Modules modules = new Modules();
            for (int i = 0, count = lstApplicationComponent.SelectedItems.Count; i < count; i++)
            {
                modules.Add((Module)lstApplicationComponent.SelectedItems[i]);
            }
            return modules;
        }

        public UnderLyings GetUnderlyings()
        {
            UnderLyings underLyings = new UnderLyings();
            for (int i = 0, count = lstAssetUnderLying.SelectedItems.Count; i < count; i++)
            {
                underLyings.Add(new UnderLying(int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[1].ToString()), ((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[0].ToString()));
            }
            return underLyings;
        }

        public Assets GetAssets()
        {
            Assets assets = new Assets();
            for (int i = 0, count = lstAssetUnderLying.SelectedItems.Count; i < count; i++)
            {
                assets.Add(new Asset(int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[1].ToString()), ((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[0].ToString()));
            }
            return assets;
        }

        public void SetModules(int companyID, int companyUserID)
        {
            lstApplicationComponent.Refresh();
            Modules modules = new Modules();
            modules = ModuleManager.GetModulesForCompany(companyID);
            if (modules.Count > 0)
            {
                lstApplicationComponent.DataSource = modules;
                lstApplicationComponent.DisplayMember = "ModuleName";
                lstApplicationComponent.ValueMember = "ModuleID";

                lstApplicationComponent.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    Modules userModules = ModuleManager.GetModulesForCompanyUser(companyUserID);
                    foreach (Module module in userModules)
                    {
                        lstApplicationComponent.SelectedValue = module.ModuleID;
                    }
                }
            }
            else
            {
                lstApplicationComponent.DataSource = null;
            }
        }

        public void SetAssetsUnderLyings(int companyID, int companyUserID)
        {
            lstAssetUnderLying.Refresh();

            UnderLyings underLyings = AssetManager.GetCompanyUnderLyings(companyID);
            if (underLyings.Count > 0)
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Data");
                dt.Columns.Add("Value");
                foreach (UnderLying underLying in underLyings)
                {
                    object[] row = new object[2];
                    row[0] = underLying.Asset + " : " + underLying.Name;
                    row[1] = underLying.UnderlyingID;
                    dt.Rows.Add(row);
                }
                lstAssetUnderLying.DataSource = dt; //underLyings;
                lstAssetUnderLying.DisplayMember = "Data";
                lstAssetUnderLying.ValueMember = "Value";

                lstAssetUnderLying.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    UnderLyings companyUserUnderLyings = AssetManager.GetCompanyUserUnderLyings(companyUserID);
                    foreach (UnderLying underLying in companyUserUnderLyings)
                    {
                        lstAssetUnderLying.SelectedValue = underLying.UnderlyingID;
                    }
                }
            }
            else
            {
                lstAssetUnderLying.DataSource = null;
            }
        }

        public bool Save(int userID, int companyID)
        {
            bool result = false;
            if (CounterPartyManager.SaveCounterPartyVenuesForUser(companyID, userID, GetCounterPartyVenues())
                && CounterPartyManager.SaveCompanyUserCounterParties(companyID, GetCounterParties(), userID)
                && CompanyManager.SaveTradingAccountsForUser(userID, GetTradingAccounts())
                && CompanyManager.SaveCompanyModulesForUser(userID, GetModules())
                && CompanyManager.SaveCompanyUnderlyingsForUser(userID, GetUnderlyings())
                && CompanyManager.SaveCompanyAssetsForUser(userID, GetAssets()))
            {
                result = true;
            }
            return result;
        }
    }
}
