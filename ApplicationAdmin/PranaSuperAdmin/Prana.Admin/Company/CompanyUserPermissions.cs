#region Using
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Nirvana.Admin.BLL;

#endregion

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for CompanyUserPermissions.
	/// </summary>
	public class CompanyUserPermissions : System.Windows.Forms.UserControl
	{
		private Infragistics.Win.Misc.UltraGroupBox grpCounterParties;
		private Infragistics.Win.Misc.UltraGroupBox grpApplicationComponent;
		private Infragistics.Win.Misc.UltraGroupBox grpAssetUnderlying;
		private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
		private System.Windows.Forms.ListControl lstApplicationComponent;
		private System.Windows.Forms.ListControl lstAssetUnderLying;
		private System.Windows.Forms.ListControl lstTradingAccount;
		private System.Windows.Forms.ListControl lstCounterPartyVenue;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
			Infragistics.Win.UltraWinListBar.Group group1 = new Infragistics.Win.UltraWinListBar.Group(true);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinListBar.Group group2 = new Infragistics.Win.UltraWinListBar.Group(true);
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinListBar.Group group3 = new Infragistics.Win.UltraWinListBar.Group(true);
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinListBar.Group group4 = new Infragistics.Win.UltraWinListBar.Group(true);
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			this.grpCounterParties = new Infragistics.Win.Misc.UltraGroupBox();
			this.grpApplicationComponent = new Infragistics.Win.Misc.UltraGroupBox();
			this.grpAssetUnderlying = new Infragistics.Win.Misc.UltraGroupBox();
			this.lstCounterPartyVenue = new System.Windows.Forms.ListBox();
			this.lstApplicationComponent = new ListBox();
			this.lstAssetUnderLying = new ListBox();
			this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
			this.lstTradingAccount = new ListBox();
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
			this.grpCounterParties.Controls.Add(this.lstCounterPartyVenue);
			this.grpCounterParties.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpCounterParties.Location = new System.Drawing.Point(6, 4);
			this.grpCounterParties.Name = "grpCounterParties";
			this.grpCounterParties.Size = new System.Drawing.Size(248, 192);
			this.grpCounterParties.SupportThemes = false;
			this.grpCounterParties.TabIndex = 0;
			this.grpCounterParties.Text = "CounterParties";
			// 
			// grpApplicationComponent
			// 
			this.grpApplicationComponent.Controls.Add(this.lstApplicationComponent);
			this.grpApplicationComponent.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpApplicationComponent.Location = new System.Drawing.Point(270, 4);
			this.grpApplicationComponent.Name = "grpApplicationComponent";
			this.grpApplicationComponent.Size = new System.Drawing.Size(248, 192);
			this.grpApplicationComponent.SupportThemes = false;
			this.grpApplicationComponent.TabIndex = 0;
			this.grpApplicationComponent.Text = "Application Component";
			// 
			// grpAssetUnderlying
			// 
			this.grpAssetUnderlying.Controls.Add(this.lstAssetUnderLying);
			this.grpAssetUnderlying.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpAssetUnderlying.Location = new System.Drawing.Point(6, 202);
			this.grpAssetUnderlying.Name = "grpAssetUnderlying";
			this.grpAssetUnderlying.Size = new System.Drawing.Size(248, 192);
			this.grpAssetUnderlying.SupportThemes = false;
			this.grpAssetUnderlying.TabIndex = 0;
			this.grpAssetUnderlying.Text = "Asset && Underlying";
			// 
			// lstCounterPartyVenue
			// 
			this.lstCounterPartyVenue.Location = new System.Drawing.Point(6, 18);
			this.lstCounterPartyVenue.Name = "lstCounterPartyVenue";
			this.lstCounterPartyVenue.Size = new System.Drawing.Size(238, 166);
			// 
			// lstApplicationComponent
			// 
			this.lstApplicationComponent.Location = new System.Drawing.Point(6, 20);
			this.lstApplicationComponent.Name = "lstApplicationComponent";
			this.lstApplicationComponent.Size = new System.Drawing.Size(238, 166);
			// 
			// lstAssetUnderLying
			// 
			this.lstAssetUnderLying.Location = new System.Drawing.Point(4, 16);
			this.lstAssetUnderLying.Name = "lstAssetUnderLying";
			this.lstAssetUnderLying.Size = new System.Drawing.Size(238, 166);
			// 
			// ultraGroupBox1
			// 
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
			this.lstTradingAccount.Location = new System.Drawing.Point(8, 16);
			this.lstTradingAccount.Name = "lstTradingAccount";
			this.lstTradingAccount.Size = new System.Drawing.Size(238, 166);
			// 
			// CompanyUserPermissions
			// 
			this.Controls.Add(this.grpCounterParties);
			this.Controls.Add(this.grpAssetUnderlying);
			this.Controls.Add(this.grpApplicationComponent);
			this.Controls.Add(this.ultraGroupBox1);
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
			BindCounterPartyVenue();
			BindComponents();
			BindAssetUnderlying();
			BindTradingAccount();
		}

		private void BindCounterPartyVenue()
		{
			//TODO:
		}

		private void BindComponents()
		{
			Modules modules = ModuleManager.GetModules();
			if(modules.Count > 0)
			{
				lstApplicationComponent.DataSource = modules;
				lstApplicationComponent.DisplayMember = "ModuleName";
				lstApplicationComponent.ValueMember = "ModuleID";
			}			
		}

		private void BindAssetUnderlying()
		{
			UnderLyings underLyings = AssetManager.GetUnderLyings();
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
			}
		}

		private void BindTradingAccount()
		{			
			//TODO:
		}		
	}
}
