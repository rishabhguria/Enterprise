#region Using

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

#endregion

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CompanyUserPermissions.
	/// </summary>
	public class CompanyUserPermissions : System.Windows.Forms.UserControl
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
			set{_companyID = value;}
		}

		public CompanyUserPermissions()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
			this.grpCounterParties.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpCounterParties.Location = new System.Drawing.Point(6, 4);
			this.grpCounterParties.Name = "grpCounterParties";
			this.grpCounterParties.Size = new System.Drawing.Size(248, 192);
			this.grpCounterParties.SupportThemes = false;
			this.grpCounterParties.TabIndex = 0;
			this.grpCounterParties.Text = "CounterParties";
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
			this.grpApplicationComponent.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpApplicationComponent.Location = new System.Drawing.Point(270, 4);
			this.grpApplicationComponent.Name = "grpApplicationComponent";
			this.grpApplicationComponent.Size = new System.Drawing.Size(248, 192);
			this.grpApplicationComponent.SupportThemes = false;
			this.grpApplicationComponent.TabIndex = 0;
			this.grpApplicationComponent.Text = "Application Component";
			// 
			// lstApplicationComponent
			// 
			this.lstApplicationComponent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lstApplicationComponent.Enabled = false;
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
			this.grpAssetUnderlying.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpAssetUnderlying.Location = new System.Drawing.Point(6, 202);
			this.grpAssetUnderlying.Name = "grpAssetUnderlying";
			this.grpAssetUnderlying.Size = new System.Drawing.Size(248, 192);
			this.grpAssetUnderlying.SupportThemes = false;
			this.grpAssetUnderlying.TabIndex = 0;
			this.grpAssetUnderlying.Text = "Asset && Underlying";
			// 
			// lstAssetUnderLying
			// 
			this.lstAssetUnderLying.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lstAssetUnderLying.Enabled = false;
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
			this.ultraGroupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraGroupBox1.Location = new System.Drawing.Point(266, 202);
			this.ultraGroupBox1.Name = "ultraGroupBox1";
			this.ultraGroupBox1.Size = new System.Drawing.Size(248, 192);
			this.ultraGroupBox1.SupportThemes = false;
			this.ultraGroupBox1.TabIndex = 0;
			this.ultraGroupBox1.Text = "Trading Account";
			// 
			// lstTradingAccount
			// 
			this.lstTradingAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lstTradingAccount.Enabled = false;
			this.lstTradingAccount.Location = new System.Drawing.Point(8, 16);
			this.lstTradingAccount.Name = "lstTradingAccount";
			this.lstTradingAccount.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.lstTradingAccount.Size = new System.Drawing.Size(238, 160);
			this.lstTradingAccount.TabIndex = 0;
			// 
			// CompanyUserPermissions
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.grpCounterParties);
			this.Controls.Add(this.grpAssetUnderlying);
			this.Controls.Add(this.grpApplicationComponent);
			this.Controls.Add(this.ultraGroupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CompanyUserPermissions";
			this.Size = new System.Drawing.Size(528, 408);
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
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnLogin_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnLogin_Click"); 
				Logger.Write(logEntry); 

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
			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
			{
				counterPartyVenues.Add((CounterPartyVenue)lstCounterParties.SelectedItems[i]);
			}
			return counterPartyVenues;
		}

		public void SetCounterParties(int companyID, int companyUserID)
		{
			CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
			
			if(counterPartyVenues.Count > 0)
			{
				lstCounterParties.DataSource = counterPartyVenues;
				lstCounterParties.DisplayMember = "DisplayName";
				lstCounterParties.ValueMember = "CounterPartyVenueID";
				lstCounterParties.SelectedIndex = -1;
						
				if(companyUserID != int.MinValue)
				{
					CounterPartyVenues companyUserCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
					foreach(CounterPartyVenue counterPartyVenue  in companyUserCounterPartyVenues)
					{
						lstCounterParties.SelectedValue = counterPartyVenue.CounterPartyVenueID;
					}
				}
			}
		}

		public TradingAccounts GetTradingAccounts()
		{
			TradingAccounts tradingAccounts = new TradingAccounts();
			for(int i=0, count = lstTradingAccount.SelectedItems.Count; i<count; i++)
			{
				tradingAccounts.Add((TradingAccount)lstTradingAccount.SelectedItems[i]);
			}
			return tradingAccounts;
		}

		public void SetTradingAccounts(int companyID, int companyUserID)
		{
			TradingAccounts tradingaccounts = CompanyManager.GetTradingAccountsForCompany(companyID);
			if(tradingaccounts.Count > 0)
			{
				lstTradingAccount.DataSource = tradingaccounts;
				lstTradingAccount.DisplayMember = "TradingAccountName";
				lstTradingAccount.ValueMember = "TradingAccountsID";
			
				lstTradingAccount.SelectedIndex = -1;
				if(companyUserID != int.MinValue)
				{
					TradingAccounts companyUserTradingaccounts = CompanyManager.GetTradingAccountsForUser(companyUserID);
					foreach(TradingAccount tradingAccount in companyUserTradingaccounts)
					{
						lstTradingAccount.SelectedValue = tradingAccount.TradingAccountsID;
					}
				}
			}
		}

		public Modules GetModules()
		{
			Modules modules = new Modules();
			for(int i=0, count = lstApplicationComponent.SelectedItems.Count; i<count; i++)
			{				
				modules.Add((Module)lstApplicationComponent.SelectedItems[i]);
			}
			return modules;
		}

		public UnderLyings GetUnderlyings()
		{
			UnderLyings underLyings = new UnderLyings();
			for(int i=0, count = lstAssetUnderLying.SelectedItems.Count; i<count; i++)
			{
				underLyings.Add(new UnderLying(int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[1].ToString()), ((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[0].ToString()));
			}
			return underLyings;
		}

		public Assets GetAssets()
		{
			Assets assets = new Assets();
			for(int i=0, count = lstAssetUnderLying.SelectedItems.Count; i<count; i++)
			{
				assets.Add(new Asset(int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[1].ToString()), ((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[0].ToString()));
			}	
			return assets;
		}
	
		public void SetModules(int companyID, int companyUserID)
		{
			Modules modules = new Modules();
			modules = ModuleManager.GetModulesForCompany(companyID);
			if(modules.Count > 0)
			{
				lstApplicationComponent.DataSource = modules;
				lstApplicationComponent.DisplayMember = "ModuleName";
				lstApplicationComponent.ValueMember = "ModuleID";
			
				lstApplicationComponent.SelectedIndex = -1;
				if(companyUserID != int.MinValue)
				{
					Modules userModules = ModuleManager.GetModulesForCompanyUser(companyUserID);
					foreach(Module module in userModules)
					{
						lstApplicationComponent.SelectedValue = module.ModuleID;
					}
				}
			}
		}

		public void SetAssetsUnderLyings(int companyID, int companyUserID)
		{
			UnderLyings underLyings = AssetManager.GetCompanyUnderLyings(companyID);
			if(underLyings.Count > 0)
			{
				System.Data.DataTable dt = new System.Data.DataTable();
				dt.Columns.Add("Data");
				dt.Columns.Add("Value");
				foreach(UnderLying underLying in underLyings)
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
				if(companyUserID != int.MinValue)
				{
					UnderLyings companyUserUnderLyings = AssetManager.GetCompanyUserUnderLyings(companyUserID);
					foreach (UnderLying underLying in companyUserUnderLyings)
					{
						lstAssetUnderLying.SelectedValue = underLying.UnderlyingID;
					}
				}
			}
		}

		public bool Save(int userID)
		{
			bool result = false;
			if(CounterPartyManager.SaveCounterPartyVenuesForUser(userID, GetCounterPartyVenues())
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
