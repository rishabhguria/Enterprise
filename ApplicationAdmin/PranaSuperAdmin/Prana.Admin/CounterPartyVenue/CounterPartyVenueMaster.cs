using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Controls;
using System.Text;
using Nirvana.Admin.Utility;
using System.Text.RegularExpressions;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for CounterPartyVenueMaster.
	/// </summary>
	public class CounterPartyVenueMaster : System.Windows.Forms.Form
	{
		#region Constants definitions
		private const string FORM_NAME = "CounterPartyVenueMaster : ";
		//Constants defined by the user.
		//Tab Constants
		const int C_TAB_COUNTERPARTY = 0;
		const int C_TAB_VENUE = 1;
		const int C_TAB_COUNTERPARTY_VENUE = 2;

		const int C_TAB_COUNTERPARTYDETAIL = 0;
		const int C_TAB_TRADINGINFORMATION = 1;

		const int C_TAB_COUNTERPARTYVENUEDETAIL = 0;
		const int C_TAB_ACCEPTEDORDERTYPES = 3;
		const int C_TAB_FIX = 1;	

		//Tree Node Constants
		const int C_TREE_COUNTERPARTY = 0;
		const int C_TREE_VENUE = 1;
		const int C_TREE_COUNTERPARTY_VENUE = 2;

		const int VENUE_EXCHANGES = 1;
		const int VENUE_ATS = 2;
		const int VENUE_DESKS = 3;
		const int VENUE_ROUTER = 4;
		const int VENUE_ALGO = 5;
		
		const string C_COMBO_SELECT = "- Select -";
		const int OTHER = 3;

		#endregion

		#region Private and protected members

		private System.Windows.Forms.StatusBar stbCounterParty;
		private System.Windows.Forms.ErrorProvider errorProvider1;
//		private Nirvana.Admin.Controls.Fix uctFix;
//		private Nirvana.Admin.Controls.SymbolMapping uctSymbolMapping;
//		private Nirvana.Admin.Controls.CounterPartyVenueDetails uctCounterPartyVenueDetails;
//		private Nirvana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes uctCounterPartyVenueAcceptedOrderTypes1;
		private System.Windows.Forms.TreeView trvCounterPartyVenue;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCounterPartyVenue;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private System.Windows.Forms.Button btnVenuesSave;
		private System.Windows.Forms.Button btnVenuesClose;
		private System.Windows.Forms.Button btnCounterPartyVenueClose;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCounterPartyVenueTabs;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
//		private Nirvana.Admin.Controls.CounterPartyVenueVenues uctcounterPartyVenueVenues;
//		private Nirvana.Admin.Controls.CounterPartyVenueDetails uctCounterPartyVenueDetails;
//		private Nirvana.Admin.Controls.Fix uctFix;
//		private Nirvana.Admin.Controls.SymbolMapping uctSymbolMapping;
//		private Nirvana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes uctCounterPartyVenueAcceptedOrderTypes;
//		private Nirvana.Admin.Controls.CounterPartyVenueDetails counterPartyVenueDetails1;
//		private Nirvana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes uctCounterPartyVenueAcceptedOrderTypes1;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TextBox txtTitle2;
		private System.Windows.Forms.TextBox txtEmail2;
		private System.Windows.Forms.TextBox txtEmail1;
		private System.Windows.Forms.TextBox txtContactName2;
		private System.Windows.Forms.TextBox txtContactName1;
		private System.Windows.Forms.TextBox txtTitle1;
		private System.Windows.Forms.TextBox txtPhone;
		private System.Windows.Forms.TextBox txtFax;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.TextBox txtFullName;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Button btmCVDetailClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCVDetailSave;
		private Nirvana.Admin.Controls.CounterPartyVenueDetails uctCounterPartyVenueDetails;
		private Nirvana.Admin.Controls.Fix uctFix;
		private Nirvana.Admin.Controls.SymbolMapping uctSymbolMapping;
		private Nirvana.Admin.Controls.CounterPartyVenueVenues uctcounterPartyVenueVenues;
		private Nirvana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes uctCounterPartyVenueAcceptedOrderTypes;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterPartyType;

//		private Nirvana.Admin.Controls.CounterPartyVenueDetails uctCounterPartyVenueDetails;
//		private Nirvana.Admin.Controls.Fix uctFix;
//		private Nirvana.Admin.Controls.SymbolMapping uctSymbolMapping;
//		private Nirvana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes uctCounterPartyVenueAcceptedOrderTypes;

		
//		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterPartyType;
//		private Nirvana.Admin.Controls.SymbolMapping symbolMapping1;
//		private Nirvana.Admin.Controls.CounterPartyVenueVenues uctcounterPartyVenueVenues;
//		private Nirvana.Admin.Controls.CounterPartyVenueDetails uctCounterPartyVenueDetails;
//		private Nirvana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes uctCounterPartyVenueAcceptedOrderTypes;
//		private Nirvana.Admin.Controls.SymbolMapping uctSymbolMapping;

		//private System.Windows.Forms.TextBox txtCompIDOutgoingd;
		

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


#endregion

		public CounterPartyVenueMaster()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			 SetUpMenuPermissions();
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

		//This method fetches the user permissions from the database.
		private void SetUpMenuPermissions()
		{	
			Preferences preferences = Preferences.Instance;	
			bool chkCounterPartyVenue = preferences.Maintain_Counter_Parties;
			bool chkSetUpCompanies = preferences.Set_Up_Company;
			//If the user doesnt have the permissions to maintain CounterParty then the Add & Delete buttons are
			//disabled so that he/she can't add or delete the CounterParty.
			if(chkCounterPartyVenue == false)
			{
				btnAdd.Enabled = false;
				btnDelete.Enabled = false;
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CounterPartyVenueMaster));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyTypeID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1);
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.uctCounterPartyVenueDetails = new Nirvana.Admin.Controls.CounterPartyVenueDetails();
			this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.uctFix = new Nirvana.Admin.Controls.Fix();
			this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.uctSymbolMapping = new Nirvana.Admin.Controls.SymbolMapping();
			this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.label1 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.txtTitle2 = new System.Windows.Forms.TextBox();
			this.txtEmail2 = new System.Windows.Forms.TextBox();
			this.txtEmail1 = new System.Windows.Forms.TextBox();
			this.txtContactName2 = new System.Windows.Forms.TextBox();
			this.txtContactName1 = new System.Windows.Forms.TextBox();
			this.txtTitle1 = new System.Windows.Forms.TextBox();
			this.txtPhone = new System.Windows.Forms.TextBox();
			this.txtFax = new System.Windows.Forms.TextBox();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.txtFullName = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btmCVDetailClose = new System.Windows.Forms.Button();
			this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.uctcounterPartyVenueVenues = new Nirvana.Admin.Controls.CounterPartyVenueVenues();
			this.btnVenuesClose = new System.Windows.Forms.Button();
			this.btnVenuesSave = new System.Windows.Forms.Button();
			this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.tabCounterPartyVenueTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.btnCounterPartyVenueClose = new System.Windows.Forms.Button();
			this.btnCVDetailSave = new System.Windows.Forms.Button();
			this.stbCounterParty = new System.Windows.Forms.StatusBar();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.trvCounterPartyVenue = new System.Windows.Forms.TreeView();
			this.tbcCounterPartyVenue = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.uctCounterPartyVenueAcceptedOrderTypes = new Nirvana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes();
			this.cmbCounterPartyType = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.ultraTabPageControl5.SuspendLayout();
			this.ultraTabPageControl6.SuspendLayout();
			this.ultraTabPageControl7.SuspendLayout();
			this.ultraTabPageControl8.SuspendLayout();
			this.ultraTabPageControl1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.ultraTabPageControl2.SuspendLayout();
			this.ultraTabPageControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabCounterPartyVenueTabs)).BeginInit();
			this.tabCounterPartyVenueTabs.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcCounterPartyVenue)).BeginInit();
			this.tbcCounterPartyVenue.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyType)).BeginInit();
			this.SuspendLayout();
			// 
			// ultraTabPageControl5
			// 
			this.ultraTabPageControl5.Controls.Add(this.uctCounterPartyVenueDetails);
			this.ultraTabPageControl5.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl5.Name = "ultraTabPageControl5";
			this.ultraTabPageControl5.Size = new System.Drawing.Size(374, 547);
			// 
			// uctCounterPartyVenueDetails
			// 
			this.uctCounterPartyVenueDetails.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.uctCounterPartyVenueDetails.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCounterPartyVenueDetails.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCounterPartyVenueDetails.Location = new System.Drawing.Point(4, 6);
			this.uctCounterPartyVenueDetails.Name = "uctCounterPartyVenueDetails";
			this.uctCounterPartyVenueDetails.Size = new System.Drawing.Size(366, 460);
			this.uctCounterPartyVenueDetails.TabIndex = 0;
			// 
			// ultraTabPageControl6
			// 
			this.ultraTabPageControl6.Controls.Add(this.uctFix);
			this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl6.Name = "ultraTabPageControl6";
			this.ultraTabPageControl6.Size = new System.Drawing.Size(374, 547);
			// 
			// uctFix
			// 
			this.uctFix.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.uctFix.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctFix.CounterPartyVenueID = -2147483648;
			this.uctFix.Location = new System.Drawing.Point(46, 6);
			this.uctFix.Name = "uctFix";
			this.uctFix.Size = new System.Drawing.Size(300, 142);
			this.uctFix.TabIndex = 0;
			// 
			// ultraTabPageControl7
			// 
			this.ultraTabPageControl7.Controls.Add(this.uctSymbolMapping);
			this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl7.Name = "ultraTabPageControl7";
			this.ultraTabPageControl7.Size = new System.Drawing.Size(374, 547);
			this.ultraTabPageControl7.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl7_Paint);
			// 
			// uctSymbolMapping
			// 
			this.uctSymbolMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.uctSymbolMapping.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctSymbolMapping.CounterPartyVenueID = -2147483648;
			this.uctSymbolMapping.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctSymbolMapping.Location = new System.Drawing.Point(4, 6);
			this.uctSymbolMapping.Name = "uctSymbolMapping";
			this.uctSymbolMapping.Size = new System.Drawing.Size(366, 202);
			this.uctSymbolMapping.SymbolMappingID = -2147483648;
			this.uctSymbolMapping.TabIndex = 0;
			// 
			// ultraTabPageControl8
			// 
			this.ultraTabPageControl8.Controls.Add(this.uctCounterPartyVenueAcceptedOrderTypes);
			this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl8.Name = "ultraTabPageControl8";
			this.ultraTabPageControl8.Size = new System.Drawing.Size(374, 547);
			this.ultraTabPageControl8.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl8_Paint);
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Controls.Add(this.label15);
			this.ultraTabPageControl1.Controls.Add(this.groupBox1);
			this.ultraTabPageControl1.Controls.Add(this.btnSave);
			this.ultraTabPageControl1.Controls.Add(this.btmCVDetailClose);
			this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			this.ultraTabPageControl1.Size = new System.Drawing.Size(388, 595);
			this.ultraTabPageControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl1_Paint_1);
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label15.ForeColor = System.Drawing.Color.Red;
			this.label15.Location = new System.Drawing.Point(42, 326);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(104, 16);
			this.label15.TabIndex = 89;
			this.label15.Text = "* Required Field";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.groupBox1.Controls.Add(this.cmbCounterPartyType);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label35);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.txtTitle2);
			this.groupBox1.Controls.Add(this.txtEmail2);
			this.groupBox1.Controls.Add(this.txtEmail1);
			this.groupBox1.Controls.Add(this.txtContactName2);
			this.groupBox1.Controls.Add(this.txtContactName1);
			this.groupBox1.Controls.Add(this.txtTitle1);
			this.groupBox1.Controls.Add(this.txtPhone);
			this.groupBox1.Controls.Add(this.txtFax);
			this.groupBox1.Controls.Add(this.txtAddress1);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Controls.Add(this.txtFullName);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label33);
			this.groupBox1.Controls.Add(this.label34);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox1.Location = new System.Drawing.Point(40, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(308, 320);
			this.groupBox1.TabIndex = 88;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Contact Details";
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(112, 296);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 16);
			this.label1.TabIndex = 93;
			this.label1.Text = "*";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label11.Location = new System.Drawing.Point(8, 298);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(112, 16);
			this.label11.TabIndex = 91;
			this.label11.Text = "Counterparty Type";
			// 
			// label35
			// 
			this.label35.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label35.Location = new System.Drawing.Point(8, 118);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(100, 16);
			this.label35.TabIndex = 90;
			this.label35.Text = "(1-111-111111)";
			// 
			// label18
			// 
			this.label18.ForeColor = System.Drawing.Color.Red;
			this.label18.Location = new System.Drawing.Point(48, 208);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(8, 16);
			this.label18.TabIndex = 87;
			this.label18.Text = "*";
			// 
			// label17
			// 
			this.label17.ForeColor = System.Drawing.Color.Red;
			this.label17.Location = new System.Drawing.Point(88, 154);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(8, 16);
			this.label17.TabIndex = 86;
			this.label17.Text = "*";
			// 
			// label16
			// 
			this.label16.ForeColor = System.Drawing.Color.Red;
			this.label16.Location = new System.Drawing.Point(56, 30);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(8, 16);
			this.label16.TabIndex = 85;
			this.label16.Text = "*";
			// 
			// txtTitle2
			// 
			this.txtTitle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTitle2.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtTitle2.Location = new System.Drawing.Point(136, 248);
			this.txtTitle2.MaxLength = 50;
			this.txtTitle2.Name = "txtTitle2";
			this.txtTitle2.Size = new System.Drawing.Size(150, 21);
			this.txtTitle2.TabIndex = 79;
			this.txtTitle2.Text = "";
			this.txtTitle2.LostFocus += new System.EventHandler(this.txtTitle2_LostFocus);
			this.txtTitle2.GotFocus += new System.EventHandler(this.txtTitle2_GotFocus);
			// 
			// txtEmail2
			// 
			this.txtEmail2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEmail2.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtEmail2.Location = new System.Drawing.Point(136, 272);
			this.txtEmail2.MaxLength = 50;
			this.txtEmail2.Name = "txtEmail2";
			this.txtEmail2.Size = new System.Drawing.Size(150, 21);
			this.txtEmail2.TabIndex = 80;
			this.txtEmail2.Text = "";
			this.txtEmail2.LostFocus += new System.EventHandler(this.txtEmail2_LostFocus);
			this.txtEmail2.GotFocus += new System.EventHandler(this.txtEmail2_GotFocus);
			// 
			// txtEmail1
			// 
			this.txtEmail1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEmail1.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtEmail1.Location = new System.Drawing.Point(136, 200);
			this.txtEmail1.MaxLength = 50;
			this.txtEmail1.Name = "txtEmail1";
			this.txtEmail1.Size = new System.Drawing.Size(150, 21);
			this.txtEmail1.TabIndex = 77;
			this.txtEmail1.Text = "";
			this.txtEmail1.LostFocus += new System.EventHandler(this.txtEmail1_LostFocus);
			this.txtEmail1.GotFocus += new System.EventHandler(this.txtEmail1_GotFocus);
			// 
			// txtContactName2
			// 
			this.txtContactName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtContactName2.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtContactName2.Location = new System.Drawing.Point(136, 224);
			this.txtContactName2.MaxLength = 50;
			this.txtContactName2.Name = "txtContactName2";
			this.txtContactName2.Size = new System.Drawing.Size(150, 21);
			this.txtContactName2.TabIndex = 78;
			this.txtContactName2.Text = "";
			this.txtContactName2.LostFocus += new System.EventHandler(this.txtContactName2_LostFocus);
			this.txtContactName2.GotFocus += new System.EventHandler(this.txtContactName2_GotFocus);
			// 
			// txtContactName1
			// 
			this.txtContactName1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtContactName1.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtContactName1.Location = new System.Drawing.Point(136, 152);
			this.txtContactName1.MaxLength = 50;
			this.txtContactName1.Name = "txtContactName1";
			this.txtContactName1.Size = new System.Drawing.Size(150, 21);
			this.txtContactName1.TabIndex = 75;
			this.txtContactName1.Text = "";
			this.txtContactName1.LostFocus += new System.EventHandler(this.txtContactName1_LostFocus);
			this.txtContactName1.GotFocus += new System.EventHandler(this.txtContactName1_GotFocus);
			// 
			// txtTitle1
			// 
			this.txtTitle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTitle1.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtTitle1.Location = new System.Drawing.Point(136, 176);
			this.txtTitle1.MaxLength = 50;
			this.txtTitle1.Name = "txtTitle1";
			this.txtTitle1.Size = new System.Drawing.Size(150, 21);
			this.txtTitle1.TabIndex = 76;
			this.txtTitle1.Text = "";
			this.txtTitle1.LostFocus += new System.EventHandler(this.txtTitle1_LostFocus);
			this.txtTitle1.GotFocus += new System.EventHandler(this.txtTitle1_GotFocus);
			// 
			// txtPhone
			// 
			this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPhone.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtPhone.Location = new System.Drawing.Point(136, 104);
			this.txtPhone.MaxLength = 20;
			this.txtPhone.Name = "txtPhone";
			this.txtPhone.Size = new System.Drawing.Size(150, 21);
			this.txtPhone.TabIndex = 73;
			this.txtPhone.Text = "";
			this.txtPhone.LostFocus += new System.EventHandler(this.txtPhone_LostFocus);
			this.txtPhone.GotFocus += new System.EventHandler(this.txtPhone_GotFocus);
			// 
			// txtFax
			// 
			this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFax.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtFax.Location = new System.Drawing.Point(136, 128);
			this.txtFax.MaxLength = 20;
			this.txtFax.Name = "txtFax";
			this.txtFax.Size = new System.Drawing.Size(150, 21);
			this.txtFax.TabIndex = 74;
			this.txtFax.Text = "";
			this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
			this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
			// 
			// txtAddress1
			// 
			this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress1.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtAddress1.Location = new System.Drawing.Point(136, 76);
			this.txtAddress1.MaxLength = 100;
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(150, 21);
			this.txtAddress1.TabIndex = 72;
			this.txtAddress1.Text = "";	
			this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
			this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtShortName.Location = new System.Drawing.Point(136, 52);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(150, 21);
			this.txtShortName.TabIndex = 71;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// txtFullName
			// 
			this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFullName.Font = new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.txtFullName.Location = new System.Drawing.Point(136, 28);
			this.txtFullName.MaxLength = 50;
			this.txtFullName.Name = "txtFullName";
			this.txtFullName.Size = new System.Drawing.Size(150, 21);
			this.txtFullName.TabIndex = 70;
			this.txtFullName.Text = "";
			this.txtFullName.LostFocus += new System.EventHandler(this.txtFullName_LostFocus);
			this.txtFullName.GotFocus += new System.EventHandler(this.txtFullName_GotFocus);
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label14.Location = new System.Drawing.Point(8, 226);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 16);
			this.label14.TabIndex = 69;
			this.label14.Text = "Contact Name 2";
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label13.Location = new System.Drawing.Point(8, 78);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(48, 16);
			this.label13.TabIndex = 68;
			this.label13.Text = "Address";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label10.Location = new System.Drawing.Point(8, 274);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(48, 16);
			this.label10.TabIndex = 65;
			this.label10.Text = "E-mail";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label9.Location = new System.Drawing.Point(8, 256);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(32, 16);
			this.label9.TabIndex = 64;
			this.label9.Text = "Title";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label8.Location = new System.Drawing.Point(8, 208);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 16);
			this.label8.TabIndex = 63;
			this.label8.Text = "E-mail";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label7.Location = new System.Drawing.Point(8, 178);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(32, 16);
			this.label7.TabIndex = 62;
			this.label7.Text = "Title";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label6.Location = new System.Drawing.Point(8, 154);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 61;
			this.label6.Text = "Contact Name 1";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label5.Location = new System.Drawing.Point(8, 136);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(24, 16);
			this.label5.TabIndex = 60;
			this.label5.Text = "Fax";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label4.Location = new System.Drawing.Point(8, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 59;
			this.label4.Text = "Work #";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.Location = new System.Drawing.Point(8, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 58;
			this.label3.Text = "Short Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.Location = new System.Drawing.Point(8, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 16);
			this.label2.TabIndex = 57;
			this.label2.Text = "Full Name";
			// 
			// label33
			// 
			this.label33.ForeColor = System.Drawing.Color.Red;
			this.label33.Location = new System.Drawing.Point(80, 54);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(8, 16);
			this.label33.TabIndex = 88;
			this.label33.Text = "*";
			// 
			// label34
			// 
			this.label34.ForeColor = System.Drawing.Color.Red;
			this.label34.Location = new System.Drawing.Point(56, 104);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(8, 16);
			this.label34.TabIndex = 89;
			this.label34.Text = "*";
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(116, 564);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 95;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btmCVDetailClose
			// 
			this.btmCVDetailClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btmCVDetailClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btmCVDetailClose.BackgroundImage")));
			this.btmCVDetailClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btmCVDetailClose.Location = new System.Drawing.Point(196, 564);
			this.btmCVDetailClose.Name = "btmCVDetailClose";
			this.btmCVDetailClose.TabIndex = 7;
			this.btmCVDetailClose.Click += new System.EventHandler(this.btmCVDetailClose_Click);
			// 
			// ultraTabPageControl2
			// 
			this.ultraTabPageControl2.Controls.Add(this.uctcounterPartyVenueVenues);
			this.ultraTabPageControl2.Controls.Add(this.btnVenuesClose);
			this.ultraTabPageControl2.Controls.Add(this.btnVenuesSave);
			this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl2.Name = "ultraTabPageControl2";
			this.ultraTabPageControl2.Size = new System.Drawing.Size(388, 595);
			this.ultraTabPageControl2.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl2_Paint);
			// 
			// uctcounterPartyVenueVenues
			// 
			this.uctcounterPartyVenueVenues.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.uctcounterPartyVenueVenues.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctcounterPartyVenueVenues.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctcounterPartyVenueVenues.Location = new System.Drawing.Point(54, 10);
			this.uctcounterPartyVenueVenues.Name = "uctcounterPartyVenueVenues";
			this.uctcounterPartyVenueVenues.Size = new System.Drawing.Size(286, 108);
			this.uctcounterPartyVenueVenues.TabIndex = 2;
			// 
			// btnVenuesClose
			// 
			this.btnVenuesClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVenuesClose.BackgroundImage")));
			this.btnVenuesClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVenuesClose.Location = new System.Drawing.Point(196, 564);
			this.btnVenuesClose.Name = "btnVenuesClose";
			this.btnVenuesClose.TabIndex = 1;
			this.btnVenuesClose.Click += new System.EventHandler(this.btnVenuesClose_Click);
			// 
			// btnVenuesSave
			// 
			this.btnVenuesSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVenuesSave.BackgroundImage")));
			this.btnVenuesSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVenuesSave.Location = new System.Drawing.Point(116, 564);
			this.btnVenuesSave.Name = "btnVenuesSave";
			this.btnVenuesSave.TabIndex = 0;
			this.btnVenuesSave.Click += new System.EventHandler(this.btnVenuesSave_Click);
			// 
			// ultraTabPageControl3
			// 
			this.ultraTabPageControl3.Controls.Add(this.tabCounterPartyVenueTabs);
			this.ultraTabPageControl3.Controls.Add(this.btnCounterPartyVenueClose);
			this.ultraTabPageControl3.Controls.Add(this.btnCVDetailSave);
			this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl3.Name = "ultraTabPageControl3";
			this.ultraTabPageControl3.Size = new System.Drawing.Size(388, 595);
			// 
			// tabCounterPartyVenueTabs
			// 
			appearance1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance1.BackColor2 = System.Drawing.Color.White;
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tabCounterPartyVenueTabs.ActiveTabAppearance = appearance1;
			this.tabCounterPartyVenueTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabCounterPartyVenueTabs.Controls.Add(this.ultraTabSharedControlsPage3);
			this.tabCounterPartyVenueTabs.Controls.Add(this.ultraTabPageControl5);
			this.tabCounterPartyVenueTabs.Controls.Add(this.ultraTabPageControl6);
			this.tabCounterPartyVenueTabs.Controls.Add(this.ultraTabPageControl7);
			this.tabCounterPartyVenueTabs.Controls.Add(this.ultraTabPageControl8);
			this.tabCounterPartyVenueTabs.Location = new System.Drawing.Point(4, 0);
			this.tabCounterPartyVenueTabs.Name = "tabCounterPartyVenueTabs";
			this.tabCounterPartyVenueTabs.SharedControlsPage = this.ultraTabSharedControlsPage3;
			this.tabCounterPartyVenueTabs.Size = new System.Drawing.Size(376, 568);
			this.tabCounterPartyVenueTabs.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tabCounterPartyVenueTabs.TabIndex = 2;
			ultraTab1.Key = "tabCounterPartyVenueDetails";
			ultraTab1.TabPage = this.ultraTabPageControl5;
			ultraTab1.Text = "Details";
			ultraTab2.Key = "tabFix";
			ultraTab2.TabPage = this.ultraTabPageControl6;
			ultraTab2.Text = "Fix";
			ultraTab3.Key = "tabMappings";
			ultraTab3.TabPage = this.ultraTabPageControl7;
			ultraTab3.Text = "Mappings";
			ultraTab4.Key = "tabCompliance";
			ultraTab4.TabPage = this.ultraTabPageControl8;
			ultraTab4.Text = " Compliance";
			this.tabCounterPartyVenueTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																											   ultraTab1,
																											   ultraTab2,
																											   ultraTab3,
																											   ultraTab4});
			this.tabCounterPartyVenueTabs.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabCounterPartyVenueTabs_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage3
			// 
			this.ultraTabSharedControlsPage3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage3.Name = "ultraTabSharedControlsPage3";
			this.ultraTabSharedControlsPage3.Size = new System.Drawing.Size(374, 547);
			// 
			// btnCounterPartyVenueClose
			// 
			this.btnCounterPartyVenueClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCounterPartyVenueClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCounterPartyVenueClose.BackgroundImage")));
			this.btnCounterPartyVenueClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCounterPartyVenueClose.Location = new System.Drawing.Point(196, 570);
			this.btnCounterPartyVenueClose.Name = "btnCounterPartyVenueClose";
			this.btnCounterPartyVenueClose.TabIndex = 1;
			this.btnCounterPartyVenueClose.Click += new System.EventHandler(this.btnCounterPartyVenueClose_Click);
			// 
			// btnCVDetailSave
			// 
			this.btnCVDetailSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCVDetailSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCVDetailSave.BackgroundImage")));
			this.btnCVDetailSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCVDetailSave.Location = new System.Drawing.Point(116, 570);
			this.btnCVDetailSave.Name = "btnCVDetailSave";
			this.btnCVDetailSave.TabIndex = 0;
			this.btnCVDetailSave.Click += new System.EventHandler(this.btnCVDetailSave_Click);
			// 
			// stbCounterParty
			// 
			this.stbCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.stbCounterParty.Location = new System.Drawing.Point(0, 615);
			this.stbCounterParty.Name = "stbCounterParty";
			this.stbCounterParty.ShowPanels = true;
			this.stbCounterParty.Size = new System.Drawing.Size(558, 2);
			this.stbCounterParty.TabIndex = 3;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// trvCounterPartyVenue
			// 
			this.trvCounterPartyVenue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.trvCounterPartyVenue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.trvCounterPartyVenue.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.trvCounterPartyVenue.FullRowSelect = true;
			this.trvCounterPartyVenue.HideSelection = false;
			this.trvCounterPartyVenue.HotTracking = true;
			this.trvCounterPartyVenue.ImageIndex = -1;
			this.trvCounterPartyVenue.Location = new System.Drawing.Point(2, 2);
			this.trvCounterPartyVenue.Name = "trvCounterPartyVenue";
			this.trvCounterPartyVenue.SelectedImageIndex = -1;
			this.trvCounterPartyVenue.ShowLines = false;
			this.trvCounterPartyVenue.Size = new System.Drawing.Size(158, 586);
			this.trvCounterPartyVenue.TabIndex = 5;
			this.trvCounterPartyVenue.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvCounterPartyVenue_AfterSelect);
			// 
			// tbcCounterPartyVenue
			// 
			appearance2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance2.BackColor2 = System.Drawing.Color.White;
			appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcCounterPartyVenue.ActiveTabAppearance = appearance2;
			this.tbcCounterPartyVenue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbcCounterPartyVenue.Controls.Add(this.ultraTabSharedControlsPage1);
			this.tbcCounterPartyVenue.Controls.Add(this.ultraTabPageControl1);
			this.tbcCounterPartyVenue.Controls.Add(this.ultraTabPageControl2);
			this.tbcCounterPartyVenue.Controls.Add(this.ultraTabPageControl3);
			this.tbcCounterPartyVenue.Location = new System.Drawing.Point(162, 2);
			this.tbcCounterPartyVenue.Name = "tbcCounterPartyVenue";
			this.tbcCounterPartyVenue.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.tbcCounterPartyVenue.Size = new System.Drawing.Size(390, 610);
			this.tbcCounterPartyVenue.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcCounterPartyVenue.TabIndex = 6;
			ultraTab5.Key = "CounterPartyTab";
			ultraTab5.TabPage = this.ultraTabPageControl1;
			ultraTab5.Text = "CounterParty";
			ultraTab6.Key = "tabVenues";
			ultraTab6.TabPage = this.ultraTabPageControl2;
			ultraTab6.Text = "Venues";
			ultraTab7.Key = "tabCounterPartyVenue";
			ultraTab7.TabPage = this.ultraTabPageControl3;
			ultraTab7.Text = "Counterparty Venue";
			this.tbcCounterPartyVenue.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																										   ultraTab5,
																										   ultraTab6,
																										   ultraTab7});
			this.tbcCounterPartyVenue.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcCounterPartyVenue_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(388, 595);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Location = new System.Drawing.Point(0, 592);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 7;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Location = new System.Drawing.Point(78, 592);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 8;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// uctCounterPartyVenueAcceptedOrderTypes
			// 
			this.uctCounterPartyVenueAcceptedOrderTypes.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.uctCounterPartyVenueAcceptedOrderTypes.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCounterPartyVenueAcceptedOrderTypes.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCounterPartyVenueAcceptedOrderTypes.Location = new System.Drawing.Point(12, 2);
			this.uctCounterPartyVenueAcceptedOrderTypes.Name = "uctCounterPartyVenueAcceptedOrderTypes";
			this.uctCounterPartyVenueAcceptedOrderTypes.Size = new System.Drawing.Size(350, 554);
			this.uctCounterPartyVenueAcceptedOrderTypes.TabIndex = 0;
			// 
			// cmbCounterPartyType
			// 
			this.cmbCounterPartyType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance3.BackColor = System.Drawing.SystemColors.Window;
			appearance3.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCounterPartyType.DisplayLayout.Appearance = appearance3;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2});
			this.cmbCounterPartyType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.cmbCounterPartyType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCounterPartyType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance4.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCounterPartyType.DisplayLayout.GroupByBox.Appearance = appearance4;
			appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCounterPartyType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance5;
			this.cmbCounterPartyType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance6.BackColor2 = System.Drawing.SystemColors.Control;
			appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCounterPartyType.DisplayLayout.GroupByBox.PromptAppearance = appearance6;
			this.cmbCounterPartyType.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCounterPartyType.DisplayLayout.MaxRowScrollRegions = 1;
			appearance7.BackColor = System.Drawing.SystemColors.Window;
			appearance7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCounterPartyType.DisplayLayout.Override.ActiveCellAppearance = appearance7;
			appearance8.BackColor = System.Drawing.SystemColors.Highlight;
			appearance8.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCounterPartyType.DisplayLayout.Override.ActiveRowAppearance = appearance8;
			this.cmbCounterPartyType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCounterPartyType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance9.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCounterPartyType.DisplayLayout.Override.CardAreaAppearance = appearance9;
			appearance10.BorderColor = System.Drawing.Color.Silver;
			appearance10.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCounterPartyType.DisplayLayout.Override.CellAppearance = appearance10;
			this.cmbCounterPartyType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCounterPartyType.DisplayLayout.Override.CellPadding = 0;
			appearance11.BackColor = System.Drawing.SystemColors.Control;
			appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance11.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCounterPartyType.DisplayLayout.Override.GroupByRowAppearance = appearance11;
			appearance12.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCounterPartyType.DisplayLayout.Override.HeaderAppearance = appearance12;
			this.cmbCounterPartyType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCounterPartyType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance13.BackColor = System.Drawing.SystemColors.Window;
			appearance13.BorderColor = System.Drawing.Color.Silver;
			this.cmbCounterPartyType.DisplayLayout.Override.RowAppearance = appearance13;
			this.cmbCounterPartyType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCounterPartyType.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
			this.cmbCounterPartyType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCounterPartyType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCounterPartyType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCounterPartyType.DisplayMember = "";
			this.cmbCounterPartyType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbCounterPartyType.FlatMode = true;
			this.cmbCounterPartyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbCounterPartyType.Location = new System.Drawing.Point(136, 294);
			this.cmbCounterPartyType.Name = "cmbCounterPartyType";
			this.cmbCounterPartyType.Size = new System.Drawing.Size(150, 20);
			this.cmbCounterPartyType.TabIndex = 94;
			this.cmbCounterPartyType.Text = "ultraCombo1";
			this.cmbCounterPartyType.ValueMember = "";
			this.cmbCounterPartyType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			this.cmbCounterPartyType.LostFocus += new System.EventHandler(this.cmbCounterPartyType_LostFocus);
			this.cmbCounterPartyType.GotFocus += new System.EventHandler(this.cmbCounterPartyType_GotFocus);
			// 
			// CounterPartyVenueMaster
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(558, 617);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.tbcCounterPartyVenue);
			this.Controls.Add(this.trvCounterPartyVenue);
			this.Controls.Add(this.stbCounterParty);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(566, 644);
			this.Name = "CounterPartyVenueMaster";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Counter Party Venue";
			this.Load += new System.EventHandler(this.CounterPartyVenueMaster_Load);
			this.ultraTabPageControl5.ResumeLayout(false);
			this.ultraTabPageControl6.ResumeLayout(false);
			this.ultraTabPageControl7.ResumeLayout(false);
			this.ultraTabPageControl8.ResumeLayout(false);
			this.ultraTabPageControl1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ultraTabPageControl2.ResumeLayout(false);
			this.ultraTabPageControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabCounterPartyVenueTabs)).EndInit();
			this.tabCounterPartyVenueTabs.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcCounterPartyVenue)).EndInit();
			this.tbcCounterPartyVenue.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyType)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		
		/// <summary>
		/// Various Bind methods are called on the on Load event of the AddCounterPartyVenue form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CounterPartyVenueMaster_Load(object sender, System.EventArgs e)
		{
			try
			{
				BindCounterPartyType();
				BindDataGrid();		
				BindCounterPartyVenueMasterTree();
//				BindBaseCurrencies();
//				InitializeTradingInformation();
//				BindFix();
//				BindFixCapabilities();
				//BindCompIDOutgoingIncoming();       
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("CounterPartyVenueMaster_Load", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "CounterPartyVenueMaster_Load"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void BindDataGrid()
		{
			//
		}
		
		/// <summary>
		/// Binds left tree with relevent data.
		/// </summary>
		private void BindCounterPartyVenueMasterTree()
		{
			try
			{
				//To clear the tree of any node before binding it afresh.
				trvCounterPartyVenue.Nodes.Clear();
				//Add CounterParty Node to the Tree

				//Font font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
				Font font = new Font("Tahoma", 11, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
													
				TreeNode treeNodeCounterPartyRoot = new TreeNode("Counter Party");
				//Making the root node to bold by assigning it to the font object defined above. 
				treeNodeCounterPartyRoot.NodeFont = font;
			
				NodeDetails counterPartyNode = new NodeDetails(NodeType.CounterParty, int.MinValue); 
				treeNodeCounterPartyRoot.Tag = counterPartyNode;

				//GetCounterParties method is used to fetch the existing CounterParties in the database.
				CounterParties counterParties = CounterPartyManager.GetCounterParties();

				//Loop through all the counterparties assigning each node an id corresponding to its unique 
				//value in the database.
				foreach(CounterParty counterParty in counterParties)
				{					
					TreeNode treeNodeCounterParty = new TreeNode(counterParty.ShortName);
					counterPartyNode = new NodeDetails(NodeType.CounterParty, counterParty.CounterPartyID); 
					treeNodeCounterParty.Tag = counterPartyNode;

					treeNodeCounterPartyRoot.Nodes.Add(treeNodeCounterParty);
				}

				trvCounterPartyVenue.Nodes.Add(treeNodeCounterPartyRoot);
		
				//Add Venue Node to the Tree

				//Adding Venue tree
				TreeNode treeNodeVenueRoot = new TreeNode("Venue");
				treeNodeVenueRoot.NodeFont = font;
				NodeDetails venueNode = new NodeDetails(NodeType.Venue, int.MinValue); 
				treeNodeVenueRoot.Tag = venueNode;

				//Adding Sub Venue Roots i.e Exchange, Algo etc
				//Adding Exchange Sub Root
				TreeNode treeNodeVenueExchangeRoot = new TreeNode("Exchanges");
				treeNodeVenueExchangeRoot.NodeFont = font;
				NodeDetails venueExchangeNode = new NodeDetails(NodeType.Exchangex, int.MinValue); 
				treeNodeVenueExchangeRoot.Tag = venueExchangeNode;
	
				treeNodeVenueRoot.Nodes.Add(treeNodeVenueExchangeRoot);

				//Adding ATS Sub Root
				TreeNode treeNodeVenueATSRoot = new TreeNode("ATS");
				treeNodeVenueATSRoot.NodeFont = font;
				NodeDetails venueATSNode = new NodeDetails(NodeType.ATS, int.MinValue); 
				treeNodeVenueATSRoot.Tag = venueATSNode;

				treeNodeVenueRoot.Nodes.Add(treeNodeVenueATSRoot);

				//Adding Desks Sub Root
				TreeNode treeNodeVenueDesksRoot = new TreeNode("Desks");
				treeNodeVenueDesksRoot.NodeFont = font;
				NodeDetails venueDesksNode = new NodeDetails(NodeType.ATS, int.MinValue); 
				treeNodeVenueDesksRoot.Tag = venueDesksNode;

				treeNodeVenueRoot.Nodes.Add(treeNodeVenueDesksRoot);

				//Adding Router Sub Root
				TreeNode treeNodeVenueRouterRoot = new TreeNode("Router");
				treeNodeVenueRouterRoot.NodeFont = font;
				NodeDetails venueRouterNode = new NodeDetails(NodeType.Router , int.MinValue); 
				treeNodeVenueRouterRoot.Tag = venueRouterNode;

				treeNodeVenueRoot.Nodes.Add(treeNodeVenueRouterRoot);

				//Adding Algo Sub Root
				TreeNode treeNodeVenueAlgoRoot = new TreeNode("Algo");
				treeNodeVenueAlgoRoot.NodeFont = font;
				NodeDetails venueAlgoNode = new NodeDetails(NodeType.Algo, int.MinValue); 
				treeNodeVenueAlgoRoot.Tag = venueAlgoNode;

				treeNodeVenueRoot.Nodes.Add(treeNodeVenueAlgoRoot);
			
				Venues venues = VenueManager.GetVenues();

				foreach(Venue venue in venues)
				{					
					TreeNode treeNodeVenue = new TreeNode(venue.VenueName);
					switch(venue.VenueTypeID)
					{
					
						case VENUE_EXCHANGES:				
							venueNode = new NodeDetails(NodeType.Venue, venue.VenueID, 1); 
							treeNodeVenue.Tag = venueNode;
							treeNodeVenueExchangeRoot.Nodes.Add(treeNodeVenue);
							break;	
			
						case VENUE_ATS:				
							venueNode = new NodeDetails(NodeType.Venue, venue.VenueID, 2); 
							treeNodeVenue.Tag = venueNode;
							treeNodeVenueATSRoot.Nodes.Add(treeNodeVenue);
							break;
					
						case VENUE_DESKS:				
							venueNode = new NodeDetails(NodeType.Venue, venue.VenueID, 3); 
							treeNodeVenue.Tag = venueNode;
							treeNodeVenueDesksRoot.Nodes.Add(treeNodeVenue);
							break;

						case VENUE_ROUTER:				
							venueNode = new NodeDetails(NodeType.Venue, venue.VenueID, 4); 
							treeNodeVenue.Tag = venueNode;
							treeNodeVenueRouterRoot.Nodes.Add(treeNodeVenue);
							break;

						case VENUE_ALGO:				
							venueNode = new NodeDetails(NodeType.Venue, venue.VenueID, 5); 
							treeNodeVenue.Tag = venueNode;
							treeNodeVenueAlgoRoot.Nodes.Add(treeNodeVenue);
							break;
					}

				}

				trvCounterPartyVenue.Nodes.Add(treeNodeVenueRoot);

				//Add CounterPartyVenue Node to the tree Tree


				TreeNode treeNodeCounterPartyVenueRoot = new TreeNode("CounterParty-Venue");
				treeNodeCounterPartyVenueRoot.NodeFont = font;
				NodeDetails counterPartyVenueNode = new NodeDetails(NodeType.CounterPartyVenue, int.MinValue); 
				treeNodeCounterPartyVenueRoot.Tag = counterPartyVenueNode;

				CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCounterPartyVenues();			
				string prevCounterPartyName = "debu";//string.Empty;	

				//Loop through all the counterparty and counterpartyvenues assigning each node an id corresponding to its unique 
				//value in the database.
				TreeNode treeNodeCP = null;
				if(counterPartyVenues.Count > 0)
				{
					foreach(CounterPartyVenue counterPartyVenue in counterPartyVenues)
					{
						if (prevCounterPartyName != counterPartyVenue.CounterPartyName)	
						{
							if(treeNodeCP != null)
							{
								treeNodeCounterPartyVenueRoot.Nodes.Add(treeNodeCP);
							}
							prevCounterPartyName = counterPartyVenue.CounterPartyName;
							treeNodeCP = new TreeNode(counterPartyVenue.CounterPartyName);
							treeNodeCP.NodeFont = font;
							NodeDetails nodeCP = new NodeDetails(-1, NodeType.CounterPartyVenue, counterPartyVenue.CounterPartyID); //-1 set for counterpartyvenueID so that it is uniquely identified from other nodes in CounterPartyVenue so that no data is entered against it in the screen. 
							treeNodeCP.Tag = nodeCP;					
							TreeNode treeNodeCPV = new TreeNode(counterPartyVenue.VenueName);
							NodeDetails nodeCPV = new NodeDetails(NodeType.CounterPartyVenue, counterPartyVenue.CounterPartyVenueID);
							treeNodeCPV.Tag = nodeCPV;
							treeNodeCP.Nodes.Add(treeNodeCPV);
							//treeNodeCounterPartyVenueRoot.Nodes.Add(treeNodeCP);
						}
						else
						{
							TreeNode treeNodeCPV = new TreeNode(counterPartyVenue.VenueName);
							NodeDetails nodeCPV = new NodeDetails(NodeType.CounterPartyVenue, counterPartyVenue.CounterPartyVenueID);
							treeNodeCPV.Tag = nodeCPV;
							treeNodeCP.Nodes.Add(treeNodeCPV);
						}
					}
					if(treeNodeCP != null)
					{
						treeNodeCounterPartyVenueRoot.Nodes.Add(treeNodeCP);
					}
					trvCounterPartyVenue.Nodes.Add(treeNodeCounterPartyVenueRoot);
				}
				else 
				{
					trvCounterPartyVenue.Nodes.Add(treeNodeCounterPartyVenueRoot);
				}

				//Select Root 1
				trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TAB_COUNTERPARTY];
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("BindCounterPartyVenueMasterTree", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "BindCounterPartyVenueMasterTree"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

//		/// <summary>
//		/// This method binds the existing <see cref="Currency"/> in the ComboBox control by assigning the 
//		/// currencies object to its datasource property.
//		/// </summary>
//		private void BindBaseCurrencies()
//		{
//			Currencies currencies = AUECManager.GetCurrencies();	
//			currencies.Insert(0, new Currency(int.MinValue, C_COMBO_SELECT,C_COMBO_SELECT));
//			cmbBaseCurrency.DataSource = currencies;						
//			cmbBaseCurrency.DisplayMember = "CurrencySymbol";
//			cmbBaseCurrency.ValueMember = "CurencyID";
//            cmbBaseCurrency.SelectedValue = int.MinValue;			
//		}

//
//		/// <summary>
//		/// This method binds the existing <see cref="CompanyTypes"/> in the ComboBox control by assigning the 
//		/// companyTypes object to its datasource property.
//		/// Binds Fixs combo.
//		/// </summary>
//		private void BindFix()
//		{
//			Fixs fixs = Fixmanager.GetFixs();
//			cmbFIXVersion.DataSource = fixs;
//			fixs.Insert(0, new Nirvana.Admin.BLL.Fix(int.MinValue, C_COMBO_SELECT));
//			cmbFIXVersion.DisplayMember = "FixVersion";
//			cmbFIXVersion.ValueMember = "FixID";
//		}
//
//		/// <summary>
//		/// This method binds the existing <see cref="FixCapabilities"/> in the ComboBox control by assigning the 
//		/// fixCapabilities object to its datasource property.
//		/// </summary>
//		private void BindFixCapabilities()
//		{
//			FixCapabilities fixCapabilities = Fixmanager.GetFixCapabilities();
//			cmbFIXCapabilities.DataSource = fixCapabilities;
//			fixCapabilities.Insert(0, new Nirvana.Admin.BLL.FixCapability(int.MinValue, C_COMBO_SELECT));
//			cmbFIXCapabilities.DisplayMember = "Description";
//			cmbFIXCapabilities.ValueMember = "FixCapabilityID";
//		}

		
		/// <summary>
		/// This method binds the existing <see cref="CounterPartyType"/> in the ComboBox control by assigning the 
		/// counterPartyTypes object to its datasource property.
		/// </summary>
		private void BindCounterPartyType()
		{
			CounterPartyTypes counterPartyTypes = CounterPartyManager.GetCounterPartyTypes();
			cmbCounterPartyType.DataSource = counterPartyTypes;
			counterPartyTypes.Insert(0, new Nirvana.Admin.BLL.CounterPartyType(int.MinValue, C_COMBO_SELECT));
			cmbCounterPartyType.DisplayMember = "Type";
			cmbCounterPartyType.ValueMember = "CounterPartyTypeID";
		}
		
		/// <summary>
		/// Saves the application on the click of the button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveCounterPartyDetails();
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSave_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		///This method saves the counterpartyvene details while checking for the validations in the required field.
		/// </summary>
		/// <returns></returns>
		private int SaveCounterPartyDetails()
		{			
			int result = int.MinValue;
			Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
			Match emailMatch = emailRegex.Match(txtEmail1.Text.ToString());

			errorProvider1.SetError(txtFullName, "");
			errorProvider1.SetError(txtShortName, "");
			errorProvider1.SetError(txtPhone, "");
			errorProvider1.SetError(txtContactName1, "");
			errorProvider1.SetError(txtEmail1, "");
			errorProvider1.SetError(cmbCounterPartyType, "");
//			errorProvider1.SetError(cmbBaseCurrency, "");
//			errorProvider1.SetError(cmbElectronic, "");
//			errorProvider1.SetError(cmbFIXVersion, "");
//			errorProvider1.SetError(txtDescriptionCounterParty, "");
//			errorProvider1.SetError(cmbFIXCapabilities, "");
//			errorProvider1.SetError(txtCompIDOutgoing, "");
//			errorProvider1.SetError(txtCompIDIncoming, "");
			if(txtFullName.Text.Trim() == "")
			{
				//tabCounterPartyDetailTradingInfo.SelectedTab = tabCounterPartyDetailTradingInfo.Tabs[C_TAB_COUNTERPARTYDETAIL];
				//tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTYDETAIL];
				errorProvider1.SetError(txtFullName, "Please enter Full name!");
				txtFullName.Focus();
			}
			else if(txtShortName.Text.Trim() == "")
			{
				//tabCounterPartyDetailTradingInfo.SelectedTab = tabCounterPartyDetailTradingInfo.Tabs[C_TAB_COUNTERPARTYDETAIL];
				errorProvider1.SetError(txtShortName, "Please enter Short name!");
				txtShortName.Focus();
			}
			else if(txtPhone.Text.Trim() == "")
			{
				//tabCounterPartyDetailTradingInfo.SelectedTab = tabCounterPartyDetailTradingInfo.Tabs[C_TAB_COUNTERPARTYDETAIL];
				errorProvider1.SetError(txtPhone, "Please enter Telephone #!");
				txtPhone.Focus();
			}
			else if(txtContactName1.Text.Trim() == "")
			{
				//tabCounterPartyDetailTradingInfo.SelectedTab = tabCounterPartyDetailTradingInfo.Tabs[C_TAB_COUNTERPARTYDETAIL];
				errorProvider1.SetError(txtContactName1, "Please enter Contact Name 1!");
				txtContactName1.Focus();
			}
			//else if(txtEmail1.Text.Trim() == "")
			else if (!emailMatch.Success)
			{
				//tabCounterPartyDetailTradingInfo.SelectedTab = tabCounterPartyDetailTradingInfo.Tabs[C_TAB_COUNTERPARTYDETAIL];
				errorProvider1.SetError(txtEmail1, "Please enter valid Email1 address!");
				txtEmail1.Focus();
			}
			else if(int.Parse(cmbCounterPartyType.Value.ToString()) == int.MinValue)
			{
				//tabCounterPartyDetailTradingInfo.SelectedTab = tabCounterPartyDetailTradingInfo.Tabs[C_TAB_COUNTERPARTYDETAIL];
				errorProvider1.SetError(cmbCounterPartyType, "Please select Counter Party type!");
				cmbCounterPartyType.Focus();
			}
			else
			{
				NodeDetails nodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Tag;
				
				Nirvana.Admin.BLL.CounterParty counterParty  = new Nirvana.Admin.BLL.CounterParty();
				counterParty.CounterPartyID = ((NodeDetails) trvCounterPartyVenue.SelectedNode.Tag).NodeID;
				counterParty.CounterPartyFullName = txtFullName.Text.Trim();
				counterParty.ShortName = txtShortName.Text.Trim();
				counterParty.Address = txtAddress1.Text.Trim();			
				counterParty.Phone = txtPhone.Text.Trim();
				counterParty.Fax = txtFax.Text.Trim();
				counterParty.ContactName1 = txtContactName1.Text.Trim();
				counterParty.Title1 = txtTitle1.Text.Trim();
				counterParty.Email1 = txtEmail1.Text.Trim();
				counterParty.contactName2 = txtContactName2.Text.Trim();
				counterParty.Title2 = txtTitle2.Text.Trim();
				counterParty.Email2 = txtEmail2.Text.Trim();
				counterParty.CounterPartyTypeID = int.Parse(cmbCounterPartyType.Value.ToString());

				
				int newcounterPartyID = CounterPartyManager.SaveCounterParty(counterParty);
				if(newcounterPartyID == -1)
				{
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Counter Party already exists with given name. Please select other login name.");
				}	
				else
				{
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Stored!");
					BindCounterPartyVenueMasterTree();
					
					NodeDetails selectNodeDetails = new NodeDetails(NodeType.CounterParty, newcounterPartyID);
					SelectTreeNode(selectNodeDetails);
				}
				result = newcounterPartyID;
			}		
					
			return result;
		}

		/// <summary>
		/// Closes the application on click of the button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Deleting the particular node of the tree on the click event of the delete button in the form. The
		/// node is checked whether it is selected or not. Also it is checked for its type ie. CounterParty, 
		/// Venue or CounterPartyVenue and then calling particular delete method depending upon its type.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				//Check whether any node is selected or not.
				if (trvCounterPartyVenue.SelectedNode.Parent != null)
				{
			
					bool result = false;
					if(trvCounterPartyVenue.SelectedNode == null)
					{
						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
						Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Please select CounteParty/Venue/CounterPartyVenue to be deleted!");
					}
					else
					{
						NodeDetails prevNodeDetails = new NodeDetails();
						if(trvCounterPartyVenue.SelectedNode.PrevNode != null)
						{
							prevNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.PrevNode.Tag;
						}
						else
						{
							prevNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Parent.Tag;
						}

						NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
						switch(nodeDetails.Type)
						{
							//CounterParty is selected now.
							case NodeType.CounterParty:
								//prevSelectedID = trvCounterPartyVenue.SelectedNode.PrevNode.Tag;
								int counterPartyID = nodeDetails.NodeID;
								if(MessageBox.Show(this, "Do you want to delete selected Counter Party?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{	
									if(!(CounterPartyManager.DeleteCounterParty(counterPartyID, false)))
									{
										MessageBox.Show(this, "CounterParty Venue is referenced in /Counterparty Venue.\n You can not delete it.", "Nirvana Alert");
									}
									else
									{
										BindCounterPartyVenueMasterTree();		
										SelectTreeNode(prevNodeDetails);
									}
								}
								break;
					
							//Venue is selected now.
							case NodeType.Venue:
								int venueID = nodeDetails.NodeID;
								prevNodeDetails.VenueTypeID = nodeDetails.VenueTypeID;								
								//prevSelectedID = trvCounterPartyVenue.SelectedNode.PrevNode;
								if(MessageBox.Show(this, "Do you want to delete selected Venue?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{
									if(!(VenueManager.DeleteVenue(venueID, false)))
									{
										MessageBox.Show(this, "Venue is referenced in /Counterparty Venue.\n You can not delete it.", "Nirvana Alert");
									}
									else
									{
										BindCounterPartyVenueMasterTree();		
										SelectTreeNode(prevNodeDetails);
									}
								}
								break;

							//CounterPartyVenue is selected.
							case NodeType.CounterPartyVenue:
								int nodeID = nodeDetails.NodeID;
								//prevNodeDetails.CounterPartyVenueID = trvCounterPartyVenue.SelectedNode.PrevNode;
								if(MessageBox.Show(this, "Do you want to delete selected CounterPartyVenue?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{
									result = CounterPartyManager.DeleteCounterPartyVenue(nodeID);					
									BindCounterPartyVenueMasterTree();
									Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
									Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "CounterPartyVenue deleted");
									BindCounterPartyVenueMasterTree();		
									SelectTreeNode(prevNodeDetails);
								}
								break;
						}					

					}
				}
				else
				{
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "No Data Available!");
				}
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnDelete_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnDelete_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private AddCounterPartyVenue frmaddCounterPartyVenue = null;

		//private NodeType selectedNodeType = NodeType.CounterParty;

		/// <summary>
		/// This method adds a node in the tree on click event of the add button in the form. After clicking 
		/// the add button a pop up opens asking the user to select particular counterparty, venue
		/// to add in the tree. After selecting the valid options from the popup, when the Save button is 
		/// clicked the particular counterparty venue combination is added in the tree. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(trvCounterPartyVenue.SelectedNode == null)
				{
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Please select CounterParty/Venue/CounterPartyVenue to be added!");
				}
				else
				{
					NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
					switch(nodeDetails.Type)
					{
						case NodeType.CounterParty:
							stbCounterParty.Text = "Enter CounterParty details.";					
							RefreshCounterPartyForm();

							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY];
							//tabCounterPartyDetailTradingInfo.Show();					

							if(nodeDetails.NodeID != int.MinValue)
							{
								trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.SelectedNode.Parent;
							}
							break;

						case NodeType.Venue:
							stbCounterParty.Text = "Enter Venue Details.";
							uctcounterPartyVenueVenues.VenueRefresh();
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_VENUE];
							tabCounterPartyVenueTabs.Show();

							if(nodeDetails.NodeID != int.MinValue)
							{
								trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.SelectedNode.Parent;
							}
							break;

						case NodeType.CounterPartyVenue:
							int counterPartyID = int.MinValue;
							if(nodeDetails.NodeID == int.MinValue)
							{
								//
							}
							else
							{
								TreeNode parentToParentNode = trvCounterPartyVenue.SelectedNode.Parent.Parent;
								if(parentToParentNode != null)
								{
									NodeDetails counterPartyNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Parent.Tag;
									counterPartyID = counterPartyNodeDetails.NodeID;
								}
								else
								{
									TreeNode parentNode = trvCounterPartyVenue.SelectedNode.Parent;
									if(parentNode != null)
									{
										NodeDetails counterPartyNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
										counterPartyID = counterPartyNodeDetails.NodeID;
									}
									else
									{
										counterPartyID = nodeDetails.NodeID;
									}
								}
							}
							stbCounterParty.Text = "Enter Counter Party Venue Details.";
							uctCounterPartyVenueDetails.Refresh();
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY_VENUE];

							if(frmaddCounterPartyVenue == null)
							{
								frmaddCounterPartyVenue = new AddCounterPartyVenue();
							}

							if(nodeDetails.NodeID != int.MinValue)
							{
								trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.SelectedNode.Parent;
							}
							frmaddCounterPartyVenue.CounterPartyID = counterPartyID;
							frmaddCounterPartyVenue.ShowDialog(this);
							//The popup dialog when disposed, returns the counterpartyvenueid.
							
							//Binding the tree to include the latest added counterpartyvenue
							BindCounterPartyVenueMasterTree();
							NodeDetails selectedNodeDetails = new NodeDetails(NodeType.CounterPartyVenue, frmaddCounterPartyVenue.SavedCounterPartyVenueID);
							//The newly added counterpartyvenue is shown selected.
							SelectTreeNode(selectedNodeDetails);
							break;
					}				
				
				}			
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnAdd_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnAdd_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}			
		}

		/// <summary>
		/// This method makes the textboxes blank.
		/// </summary>
		private void RefreshCounterPartyForm()
		{
			txtFullName.Text = "";
//			txtAcronym.Text = "";
		
			txtAddress1.Text = "";
			txtContactName1.Text = "";
			txtContactName2.Text = "";
			//txtDescriptionCounterParty = "";
			txtEmail1.Text = "";
			txtEmail2.Text = "";
			txtFax.Text = "";
			txtPhone.Text = "";
			txtShortName.Text = "";
			txtTitle1.Text = "";
			txtTitle2.Text = "";
			
		}

		/// <summary>
		/// This method fills corresponding tabs according to the counterparty/venue/counterpartyvenue selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trvCounterPartyVenue_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{			
			try
			{
				tabCounterPartyVenueTabs.Tabs[2].Enabled = false;
				Preferences preferences = Preferences.Instance;	
				bool chkCounterPartyVenue = preferences.Maintain_Counter_Parties;
								
				NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;

				if (chkCounterPartyVenue == false && nodeDetails.NodeID < 0)
				{
					tbcCounterPartyVenue.Enabled = false;
				}
				else
				{
					tbcCounterPartyVenue.Enabled = true;
				}
				if(trvCounterPartyVenue.SelectedNode == null)
				{
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Please select CounterParty/Venue/CounterPartyVenue to be shown with the details!");
				}
				else
				{
					switch(nodeDetails.Type)
					{
						case NodeType.CounterParty:
							//Counter Party is selected to be shown with details.
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY];
							int counterPartyID = nodeDetails.NodeID;
							Nirvana.Admin.BLL.CounterParty counterParty = CounterPartyManager.GetCounterParty(counterPartyID);
							//AdminUserDetails(user);
							
							//
							CounterPartyDetails(counterParty);						
							//tabCounterPartyDetailTradingInfo.Show();
						
							break;

						//Venue is selected to be shown with details.
						case NodeType.Venue:
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_VENUE];
							int venueID = nodeDetails.NodeID;
							Nirvana.Admin.BLL.Venue venue = VenueManager.GetVenue(venueID);
							uctcounterPartyVenueVenues.SetVenueDetails(venue);						
							break;

						case NodeType.Exchangex:
							//Counter Party Venue is selected to be shown with details.
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_VENUE];
							uctcounterPartyVenueVenues.VenueRefresh();					
							break;

						case NodeType.Algo:
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_VENUE];
							uctcounterPartyVenueVenues.VenueRefresh();					
							break;

						case NodeType.ATS:
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_VENUE];
							uctcounterPartyVenueVenues.VenueRefresh();					
							break;

						case NodeType.Desks:
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_VENUE];
							uctcounterPartyVenueVenues.VenueRefresh();						
							break;

						case NodeType.Router:
							tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_VENUE];
							uctcounterPartyVenueVenues.VenueRefresh();					
							break;

						case NodeType.CounterPartyVenue:
							if(nodeDetails.NodeID != int.MinValue && nodeDetails.CounterPartyVenueID != -1)
							{
								int counterPartyVenueID = nodeDetails.NodeID;
								//MessageBox.Show("no: " + nodeDetails.NodeID);
								tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_COUNTERPARTY_VENUE];
								if(counterPartyVenueID != int.MinValue)
								{
									tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_COUNTERPARTY_VENUE];						
									if(counterPartyVenueID != int.MinValue)
									{
										tabCounterPartyVenueTabs.Enabled = true;
									}
									else
									{
										//tabCounterPartyVenueTabs.Enabled = false;
									}
								
								}
								Nirvana.Admin.BLL.CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCounterPartyVenue(counterPartyVenueID);
								uctCounterPartyVenueDetails.CounterPartyProperty = counterPartyVenue;
								uctCounterPartyVenueAcceptedOrderTypes.CounterPartyProperty = counterPartyVenue;
								uctSymbolMapping.CounterPartyVenueID = counterPartyVenueID;
								uctFix.CounterPartyVenueID = counterPartyVenueID;
							}
							else
							{
								int counterPartyVenueID = nodeDetails.NodeID;
								Nirvana.Admin.BLL.CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCounterPartyVenue(counterPartyVenueID);
								uctCounterPartyVenueDetails.CounterPartyProperty = counterPartyVenue;
								uctCounterPartyVenueAcceptedOrderTypes.CounterPartyProperty = counterPartyVenue;
								uctSymbolMapping.CounterPartyVenueID = counterPartyVenueID;
								uctFix.CounterPartyVenueID = counterPartyVenueID;

								tabCounterPartyVenueTabs.Enabled = false;
							}
							break;
					}				
				}
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("trvCounterPartyVenue_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "trvCounterPartyVenue_AfterSelect"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void CounterPartyDetails(Nirvana.Admin.BLL.CounterParty counterParty)
		{
			//CounterParty detail to be shown; 
			
			
			if(counterParty != null)
			{				
//				txtAcronym.Text = counterParty.Acronym;
				txtAddress1.Text = counterParty.Address;
				txtContactName1.Text = counterParty.ContactName1;
				txtContactName2.Text = counterParty.contactName2;
				txtEmail1.Text = counterParty.Email1;
				txtEmail2.Text = counterParty.Email2;
				txtFax.Text = counterParty.Fax;
				txtFullName.Text = counterParty.CounterPartyFullName;
				txtPhone.Text = counterParty.Phone;
				txtShortName.Text = counterParty.ShortName;
				txtTitle1.Text = counterParty.Title1;
				txtTitle2.Text = counterParty.Title2;
//				txtDescriptionCounterParty.Text = counterParty.Description;
//				
//				cmbBaseCurrency.SelectedValue = int.Parse(counterParty.BaseCurrency.ToString());
//
//				cmbBaseCurrency.SelectedValue = int.Parse(counterParty.BaseCurrency.ToString());
//				cmbElectronic.SelectedValue = int.Parse(counterParty.ElectronicID.ToString());
//				cmbFIXCapabilities.SelectedValue = int.Parse(counterParty.FixCapabilitiesID.ToString());
//				cmbFIXVersion.SelectedValue = int.Parse(counterParty.FixVersionID.ToString());
//				txtCompIDIncoming.Text = counterParty.CompanyIncomingID.ToString();
//				txtCompIDOutgoing.Text = counterParty.CompanyOutgoingID.ToString();
				cmbCounterPartyType.Value = int.Parse(counterParty.CounterPartyTypeID.ToString());
				
			}
			else
			{
				//
			}
				
		}
//		/// <summary>
//		/// This method initializes a datatable to be allocated as a source to the combo boxes
//		/// </summary>
//		private void InitializeTradingInformation()
//		{
//			//ToDo: Remove this thing and put original things.
//			System.Data.DataTable dt = new System.Data.DataTable();
//			
//			//CurrencyConvention
//			dt.Columns.Add("Data");
//			dt.Columns.Add("Value");
//			object[] row = new object[2]; 
//			row[0] = C_COMBO_SELECT;
//			row[1] = int.MinValue;
//			dt.Rows.Add(row);
//			row[0] = "Yes";
//			row[1] = "1";
//			dt.Rows.Add(row);
//			row[0] = "No";
//			row[1] = "0";
//			dt.Rows.Add(row);
//			cmbElectronic.DataSource = dt;
//			cmbElectronic.DisplayMember = "Data";
//			cmbElectronic.ValueMember = "Value";
//		}

		/// <summary>
		/// This method saves the CounterPartyVenue details by calling their respective methods in the user 
		/// controls. These all check for the validation also.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCVDetailSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (trvCounterPartyVenue.SelectedNode == null) 
				{
					stbCounterParty.Text = "Please select any Counter Party Venue to be saved";
				}
				else
				{
					NodeDetails nodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Tag;

					if(tabCounterPartyVenueTabs.Tabs[0].Selected == true || tabCounterPartyVenueTabs.Tabs[1].Selected == true)
					{
						int counterPartyVenueID = int.MinValue;
						//int resultSymbolMapping = int.MinValue;
						uctCounterPartyVenueDetails.ParentStatusBar= stbCounterParty;
						CounterPartyVenue counterPartyVenue = new CounterPartyVenue();			
						counterPartyVenue.CounterPartyVenueID = nodeDetails.NodeID;
										
						int counterPartyVenueDetailID = int.MinValue;
						int counterPartyID = int.MinValue;
						if(trvCounterPartyVenue.SelectedNode.Parent != null)
						{
							NodeDetails parentNodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Parent.Tag;
							counterPartyID = parentNodeDetails.NodeID;
						}
				
						//Check for getting the complete details of the counterpartyvenuedetails so that it can
						//go ahead to save the details regarding CounterPartyVenueAcceptedOrderTypes control.
						if(uctCounterPartyVenueDetails.GetCounterPartyVenueDetailsForSave(counterPartyVenue) != int.MinValue)
						{
							//Check for getting the complete details of the Fix related detils so 
							//that it can go ahead to save the details regarding CounterParty Venue control.
							if(uctFix.GetCounterPartyVenueFix(counterPartyVenue) != int.MinValue)
							{
								//Now saves the complete details regarding the couterpartyvenuedetails.
								counterPartyVenueDetailID = CounterPartyManager.SaveCounterPartyVenue(counterPartyVenue);
												
								//							SymbolMappings symbolMappings = uctSymbolMapping.CurrentSymbolMappings;
								//							int resultSymbolMapping = CounterPartyManager.SaveSymbolMapping(symbolMappings, counterPartyVenue.CounterPartyVenueID);

								int resultCVFIX = CounterPartyManager.SaveCVFIX(counterPartyVenue);
								//
								if(resultCVFIX > 0)
								{
									Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
									Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Counter Party already exists. Please select other Counter Party.");
								}	
								else
								{
									Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
									Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Counter Party Stored!!.");
									BindCounterPartyVenueMasterTree();	
									counterPartyVenueID = counterPartyVenueDetailID;
									//NodeDetails selectnodeDetails = new NodeDetails(NodeType.CounterPartyVenue, counterPartyID);
									NodeDetails selectnodeDetails = new NodeDetails(counterPartyVenueID, NodeType.CounterPartyVenue, counterPartyID);
									SelectTreeNode(selectnodeDetails);
								}
							}
							else
							{
								tabCounterPartyVenueTabs.SelectedTab = tabCounterPartyVenueTabs.Tabs[C_TAB_FIX];
							}
												
						}		
						else
						{
							tabCounterPartyVenueTabs.SelectedTab = tabCounterPartyVenueTabs.Tabs[C_TAB_COUNTERPARTYVENUEDETAIL];
						}
					}
					else
					{
						SaveCompliance();
					}
				}	
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnCounterPartyVenueSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnCounterPartyVenueSave_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void SaveCompliance()
		{
			//TODO: Save the symbol mapping as per the AUEC. Now on hold.
			//							SymbolMappings symbolMappings = uctSymbolMapping.CurrentSymbolMappings;
			//							int resultSymbolMapping = CounterPartyManager.SaveSymbolMapping(symbolMappings, counterPartyVenue.CounterPartyVenueID);

			
			NodeDetails nodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Tag;
			//				int counterPartyVenueID = int.MinValue;
			CounterPartyVenue counterPartyVenue = new CounterPartyVenue();			
			counterPartyVenue.CounterPartyVenueID = nodeDetails.NodeID;

			int counterPartyID = int.MinValue;
			if(trvCounterPartyVenue.SelectedNode.Parent != null)
			{
				NodeDetails parentNodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Parent.Tag;
				counterPartyID = parentNodeDetails.NodeID;
			}
				
			//Check for getting the complete details of the Compliance so 
			//that it can go ahead to save the details regarding SymbolMappings & Fix control.
			if(uctCounterPartyVenueAcceptedOrderTypes.GetCounterPartyVenueAcceptedOrderTypesForSave(counterPartyVenue) != int.MinValue)
			{
				//					if(uctCounterPartyVenueAcceptedOrderTypes.GetCounterPartyVenueAcceptedOrderTypesForSave(counterPartyVenue) != int.MinValue)
				//					{
				//Now save the complete details regarding the selected AUEC.
				//						int counterPartyVenueDetailID = CounterPartyManager.SaveCounterPartyVenue(counterPartyVenue);
				int CVAUECComplianceID = CounterPartyManager.SaveCVAUECCompliance(counterPartyVenue);
									
				SymbolMappings symbolMappings = uctSymbolMapping.CurrentSymbolMappings;
				int resultSymbolMapping = CounterPartyManager.SaveSymbolMapping(symbolMappings, counterPartyVenue.CounterPartyVenueID);

				if(resultSymbolMapping == -1)
				{
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Counter Party already exists. Please select other Counter Party.");
				}	
				else
				{
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Counter Party Stored!!.");
					BindCounterPartyVenueMasterTree();	
					//							counterPartyVenueID = counterPartyVenueDetailID;
							
					//							NodeDetails selectnodeDetails = new NodeDetails(counterPartyVenueID, NodeType.CounterPartyVenue, counterPartyID);
					//							SelectTreeNode(selectnodeDetails);
				}
				//					}
				
			}
			else
			{
				tabCounterPartyVenueTabs.SelectedTab = tabCounterPartyVenueTabs.Tabs[C_TAB_ACCEPTEDORDERTYPES];
			}
		}

		//TODO: Remove this button's click event.
		/// <summary>
		/// This method saves the CounterPartyVenue compliance, symbol mapping & fix by calling their 
		/// respective methods in the user controls. These all check for the validation also.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCounterPartyVenueSave_Click(object sender, System.EventArgs e)
		{
			// This code is commented as this button click is never called as this event's function is handled
			// in the btnCVDetailSave click.

//			if (trvCounterPartyVenue.SelectedNode == null) 
//			{
//				stbCounterParty.Text = "Please select any Counter Party Venue to be saved";
//			}
//			else
//			{
//				NodeDetails nodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Tag;
////				int counterPartyVenueID = int.MinValue;
//				CounterPartyVenue counterPartyVenue = new CounterPartyVenue();			
//				counterPartyVenue.CounterPartyVenueID = nodeDetails.NodeID;
//
//				int counterPartyID = int.MinValue;
//				if(trvCounterPartyVenue.SelectedNode.Parent != null)
//				{
//					NodeDetails parentNodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Parent.Tag;
//					counterPartyID = parentNodeDetails.NodeID;
//				}
//				
//				//Check for getting the complete details of the Compliance so 
//				//that it can go ahead to save the details regarding SymbolMappings & Fix control.
//				if(uctCounterPartyVenueAcceptedOrderTypes1.GetCounterPartyVenueAcceptedOrderTypesForSave(counterPartyVenue) != int.MinValue)
//				{
////					if(uctCounterPartyVenueAcceptedOrderTypes.GetCounterPartyVenueAcceptedOrderTypesForSave(counterPartyVenue) != int.MinValue)
////					{
//						//Now save the complete details regarding the selected AUEC.
////						int counterPartyVenueDetailID = CounterPartyManager.SaveCounterPartyVenue(counterPartyVenue);
//						int CVAUECComplianceID = CounterPartyManager.SaveCVAUECCompliance(counterPartyVenue);
//									
//						SymbolMappings symbolMappings = uctSymbolMapping.CurrentSymbolMappings;
//						int resultSymbolMapping = CounterPartyManager.SaveSymbolMapping(symbolMappings, counterPartyVenue.CounterPartyVenueID);
//
//						if(resultSymbolMapping == -1)
//						{
//							Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
//							Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Counter Party already exists. Please select other Counter Party.");
//						}	
//						else
//						{
//							Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
//							Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Counter Party Stored!!.");
//							BindCounterPartyVenueMasterTree();	
////							counterPartyVenueID = counterPartyVenueDetailID;
//							
////							NodeDetails selectnodeDetails = new NodeDetails(counterPartyVenueID, NodeType.CounterPartyVenue, counterPartyID);
////							SelectTreeNode(selectnodeDetails);
//						}
////					}
//				
//				}
//				else
//				{
//					tabCounterPartyVenueTabs.SelectedTab = tabCounterPartyVenueTabs.Tabs[C_TAB_ACCEPTEDORDERTYPES];
//				}
//			}
		}
		

		/// <summary>
		/// This method saves the venue details by getting the venue details from user control while checking 
		/// for the validations.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnVenuesSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				int result = int.MinValue;
				int venueTypeID = int.MinValue;
				uctcounterPartyVenueVenues.ParentStatusBar = stbCounterParty;
			
				NodeDetails nodeDetails = (NodeDetails) trvCounterPartyVenue.SelectedNode.Tag;				
				Nirvana.Admin.BLL.Venue venue  = new Nirvana.Admin.BLL.Venue();
								
				uctcounterPartyVenueVenues.VenueID = ((NodeDetails) trvCounterPartyVenue.SelectedNode.Tag).NodeID;
			
				result = uctcounterPartyVenueVenues.SaveVenues();// SaveVenues();

				if(result != int.MinValue)
				{
					if(result == -1)
					{
						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCounterParty);
						Nirvana.Admin.Utility.Common.SetStatusPanel(stbCounterParty, "Venue already exists. Please select other venue.");
					}	
					else
					{
						venueTypeID = uctcounterPartyVenueVenues.VenueTypeID;
						stbCounterParty.Text = "Venue Stored!!.";					
					}
				
					BindCounterPartyVenueMasterTree();
					NodeDetails selectnodeDetails = new NodeDetails(NodeType.Venue, result, venueTypeID);
					SelectTreeNode(selectnodeDetails);
				}
			}
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnVenuesSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnVenuesSave_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
		/// </summary>
		/// <param name="nodeDetails"></param>
		private void SelectTreeNode(NodeDetails nodeDetails)
		{
			int countIndex = int.MinValue;
			switch(nodeDetails.Type)
			{
				case NodeType.CounterParty:
					foreach(TreeNode node in trvCounterPartyVenue.Nodes[C_TAB_COUNTERPARTY].Nodes)
					{
						if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
						{
							trvCounterPartyVenue.SelectedNode = node;
							break;
						}
					}
					break;

				case NodeType.Venue:
					int venueType = int.Parse(nodeDetails.VenueTypeID.ToString());
					switch(venueType)
					{
						case 1:
						foreach(TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_VENUE].Nodes[0].Nodes)
						{
							if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
							{
								trvCounterPartyVenue.SelectedNode = node;
								break;
							}	
						}
						break;

						case 2:
							foreach(TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_VENUE].Nodes[1].Nodes)
							{
								if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
								{
									trvCounterPartyVenue.SelectedNode = node;
									break;
								}	
							}
							break;

						case 3:
							foreach(TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_VENUE].Nodes[2].Nodes)
							{
								if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
								{
									trvCounterPartyVenue.SelectedNode = node;
									break;
								}	
							}
							break;

						case 4:
							foreach(TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_VENUE].Nodes[3].Nodes)
							{
								if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
								{
									trvCounterPartyVenue.SelectedNode = node;
									break;
								}	
							}
							break;

						case 5:
							foreach(TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_VENUE].Nodes[4].Nodes)
							{
								if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
								{
									trvCounterPartyVenue.SelectedNode = node;
									break;
								}	
							}
							break;
					}
					break;

				case NodeType.CounterPartyVenue:
					countIndex = -1;
					foreach(TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY_VENUE].Nodes)
					{
						countIndex++;
						if(((NodeDetails) node.Tag).NodeID == nodeDetails.CounterPartyID)
						{
							foreach(TreeNode cpvNode in trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY_VENUE].Nodes[countIndex].Nodes)
							{
								if(((NodeDetails) cpvNode.Tag).NodeID == nodeDetails.CounterPartyVenueID)
								{
									trvCounterPartyVenue.SelectedNode = cpvNode;
									break;
								}
							}
						}
					}
					break;
			}
		}

		private void btnVenuesClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnCounterPartyVenueClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btmCVDetailClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void CounterPartyDetailsTab_Click(object sender, System.EventArgs e)
		{
		
		}

		/// <summary>
		/// This methods selects the respected node as per the selected tab on click event of the tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tbcCounterPartyVenue_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{		
			try
			{
				if(trvCounterPartyVenue.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;

			
					if ((nodeDetails.Type != NodeType.CounterParty) &&  (tbcCounterPartyVenue.SelectedTab == tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY]))
					{
						trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY];
					}
				
					if ((nodeDetails.Type != NodeType.Venue) &&  (tbcCounterPartyVenue.SelectedTab == tbcCounterPartyVenue.Tabs[C_TAB_VENUE]))
					{
						trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TREE_VENUE];
					}
				
					if ((nodeDetails.Type != NodeType.CounterPartyVenue) &&  (tbcCounterPartyVenue.SelectedTab == tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY_VENUE]))
					{
						trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY_VENUE];
					}
				}
			}		
			#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("tbcCounterPartyVenue_SelectedIndexChanged", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "tbcCounterPartyVenue_SelectedIndexChanged"); 
				Logger.Write(logEntry); 

				#endregion
			}
			
		}

		#region Controls Focus Color
		private void txtFullName_GotFocus(object sender, System.EventArgs e)
		{
			txtFullName.BackColor = Color.LemonChiffon;
		}
		private void txtFullName_LostFocus(object sender, System.EventArgs e)
		{
			txtFullName.BackColor = Color.White;
		}
		private void txtShortName_GotFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.LemonChiffon;
		}
		private void txtShortName_LostFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.White;
		}
		private void txtAddress1_GotFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.LemonChiffon;
		}
		private void txtAddress1_LostFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.White;
		}
		private void txtContactName1_GotFocus(object sender, System.EventArgs e)
		{
			txtContactName1.BackColor = Color.LemonChiffon;
		}
		private void txtContactName1_LostFocus(object sender, System.EventArgs e)
		{
			txtContactName1.BackColor = Color.White;
		}
		private void txtContactName2_GotFocus(object sender, System.EventArgs e)
		{
			txtContactName2.BackColor = Color.LemonChiffon;
		}
		private void txtContactName2_LostFocus(object sender, System.EventArgs e)
		{
			txtContactName2.BackColor = Color.White;
		}
		private void txtEmail1_GotFocus(object sender, System.EventArgs e)
		{
			txtEmail1.BackColor = Color.LemonChiffon;
		}
		private void txtEmail1_LostFocus(object sender, System.EventArgs e)
		{
			txtEmail1.BackColor = Color.White;
		}
		private void txtEmail2_GotFocus(object sender, System.EventArgs e)
		{
			txtEmail2.BackColor = Color.LemonChiffon;
		}
		private void txtEmail2_LostFocus(object sender, System.EventArgs e)
		{
			txtEmail2.BackColor = Color.White;
		}
		private void txtFax_GotFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.LemonChiffon;
		}
		private void txtFax_LostFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.White;
		}
		private void txtPhone_GotFocus(object sender, System.EventArgs e)
		{
			txtPhone.BackColor = Color.LemonChiffon;
		}
		private void txtPhone_LostFocus(object sender, System.EventArgs e)
		{
			txtPhone.BackColor = Color.White;
		}
		private void txtTitle1_GotFocus(object sender, System.EventArgs e)
		{
			txtTitle1.BackColor = Color.LemonChiffon;
		}
		private void txtTitle1_LostFocus(object sender, System.EventArgs e)
		{
			txtTitle1.BackColor = Color.White;
		}
		private void txtTitle2_GotFocus(object sender, System.EventArgs e)
		{
			txtTitle2.BackColor = Color.LemonChiffon;
		}
		private void txtTitle2_LostFocus(object sender, System.EventArgs e)
		{
			txtTitle2.BackColor = Color.White;
		}
		private void cmbCounterPartyType_GotFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyType.Appearance.BackColor = Color.LemonChiffon;
		}
		private void cmbCounterPartyType_LostFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyType.Appearance.BackColor = Color.White;
		}
		
		
#endregion

		
		/// <summary>
		/// This method creates the symbol mapping by opening up a dialog box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			CounterPartyVenueSymbolMapping frmCounterPartyVenueSymbolMapping = new CounterPartyVenueSymbolMapping();
			frmCounterPartyVenueSymbolMapping.ShowDialog(this);
		}

		private void symbolMapping1_Load(object sender, System.EventArgs e)
		{
			//BindDataDrid();
		}

		
		//To highlight and show the currently selected node in a different color.
		private void tbcCounterPartyVenue_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tbcCounterPartyVenue.SelectedTab.Index)
			{
				f = new Font(e.Font, FontStyle.Regular);
				backBrush = new System.Drawing.SolidBrush(Color.Brown);
				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
				foreBrush = Brushes.Black;
			}
			else
			{
				f = e.Font;
				backBrush = new SolidBrush(e.BackColor); 
				foreBrush = new SolidBrush(e.ForeColor);
			}

			string tabName = this.tbcCounterPartyVenue.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcCounterPartyVenue.SelectedTab.Index)
			{
				f.Dispose();
				backBrush.Dispose();
			}
			else
			{
				backBrush.Dispose();
				foreBrush.Dispose();
			}
		}
		
		#region Highlight Selected Tab
		//To highlight and show the currently selected node in a different color.
		private void tabCounterPartyVenueTabs_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tabCounterPartyVenueTabs.SelectedTab.Index)
			{
				f = new Font(e.Font, FontStyle.Regular);
				backBrush = new System.Drawing.SolidBrush(Color.Brown);
				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
				foreBrush = Brushes.Black;
			}
			else
			{
				f = e.Font;
				backBrush = new SolidBrush(e.BackColor); 
				foreBrush = new SolidBrush(e.ForeColor);
			}

			string tabName = this.tabCounterPartyVenueTabs.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tabCounterPartyVenueTabs.SelectedTab.Index)
			{
				f.Dispose();
				backBrush.Dispose();
			}
			else
			{
				backBrush.Dispose();
				foreBrush.Dispose();
			}
		}
		
		

		private void tabRouteDetail_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
//			Font f;
//			Brush backBrush;
//			Brush foreBrush;
//			
//			if(e.Index == this.tabRouteDetail.SelectedTab.Index)
//			{
//				f = new Font(e.Font, FontStyle.Regular);
//				backBrush = new System.Drawing.SolidBrush(Color.Brown);
//				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
//				foreBrush = Brushes.Black;
//			}
//			else
//			{
//				f = e.Font;
//				backBrush = new SolidBrush(e.BackColor); 
//				foreBrush = new SolidBrush(e.ForeColor);
//			}
//
//			string tabName = this.tabRouteDetail.Tabs[e.Index].Text;
//			StringFormat sf = new StringFormat();
//			sf.Alignment = StringAlignment.Center;
//			e.Graphics.FillRectangle(backBrush, e.Bounds);
//			Rectangle r = e.Bounds;
//			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
//			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);
//
//			sf.Dispose();
//			if(e.Index == this.tabRouteDetail.SelectedTab.Index)
//			{
//				f.Dispose();
//				backBrush.Dispose();
//			}
//			else
//			{
//				backBrush.Dispose();
//				foreBrush.Dispose();
//			}
		}

		private void cmbTypeCounterParty_SelectedIndexChanged(object sender, System.EventArgs e)
		{
//			if(int.Parse(cmbTypeCounterParty.SelectedValue.ToString()) == OTHER)
//			{
//				txtDescriptionCounterParty.Enabled = true;
//			}
//			else
//			{
//				txtDescriptionCounterParty.Enabled = false;
//			}
		}


		private void tabCounterPartyDetailTradingInfo_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
//			Font f;
//			Brush backBrush;
//			Brush foreBrush;
//			
//			if(e.Index == this.tabCounterPartyDetailTradingInfo.SelectedTab.Index)
//			{
//				f = new Font(e.Font, FontStyle.Regular);
//				backBrush = new System.Drawing.SolidBrush(Color.Brown);
//				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
//				foreBrush = Brushes.Black;
//			}
//			else
//			{
//				f = e.Font;
//				backBrush = new SolidBrush(e.BackColor); 
//				foreBrush = new SolidBrush(e.ForeColor);
//			}
//
//			string tabName = this.tabCounterPartyDetailTradingInfo.Tabs[e.Index].Text;
//			StringFormat sf = new StringFormat();
//			sf.Alignment = StringAlignment.Center;
//			e.Graphics.FillRectangle(backBrush, e.Bounds);
//			Rectangle r = e.Bounds;
//			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
//			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);
//
//			sf.Dispose();
//			if(e.Index == this.tabCounterPartyDetailTradingInfo.SelectedTab.Index)
//			{
//				f.Dispose();
//				backBrush.Dispose();
//			}
//			else
//			{
//				backBrush.Dispose();
//				foreBrush.Dispose();
//			}
		}
		#endregion

		private void tabCounterPartyDetailTradingInfo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void ultraTabPageControl1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void ultraTabPageControl1_Paint_1(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void ultraTabPageControl7_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void tabCounterPartyVenueTabs_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			if(tabCounterPartyVenueTabs.Tabs[3].Selected == true)
			{
				uctCounterPartyVenueAcceptedOrderTypes.SetCVAUEC();
			}
		}

		private void ultraTabPageControl8_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void btnVenuesSave_Click_1(object sender, System.EventArgs e)
		{
		
		}

		private void ultraTabPageControl2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}
		
		#region NodeDetails
		//Creating class NodeDetail to be used for the purpose of tree giving it some methods & properties.
		class NodeDetails
		{
			private NodeType _type = NodeType.CounterParty;
			private int _venueTypeID = int.MinValue;
			private int _counterPartyVenueID = int.MinValue;
			private int _counterPartyID = int.MinValue;
			private int _nodeID = int.MinValue;

			public NodeDetails()
			{
			}

			public NodeDetails(NodeType type, int nodeID)
			{
				_type = type;
				_nodeID = nodeID;
			}

			public NodeDetails(NodeType type, int nodeID, int venueTypeID)
			{
				_type = type;
				_nodeID = nodeID;
				_venueTypeID = venueTypeID;
			}

			public NodeDetails(int counterPartyVenueID, NodeType type, int counterPartyID)
			{
				_type = type;
				_counterPartyVenueID = counterPartyVenueID;
				_counterPartyID = counterPartyID;
			}

			public NodeType Type
			{
				get{return _type;}
				set{_type = value;}
			}

			public int NodeID
			{
				get{return _nodeID;}
				set{_nodeID = value;}
			}

			public int VenueTypeID
			{
				get{return _venueTypeID;}
				set{_venueTypeID = value;}
			}

			public int CounterPartyID
			{
				get{return _counterPartyID;}
				set{_counterPartyID = value;}
			}

			public int CounterPartyVenueID
			{
				get{return _counterPartyVenueID;}
				set{_counterPartyVenueID = value;}
			}
		}

		//Creating enumeration to be used to distinguish tree nodetype on the basis of CounterParty/Venue/
		//CounterPartyVenue/Exchangex/ATS/Desks/Router/Algo
		enum NodeType
		{
			CounterParty = 1,
			Venue = 2,
			CounterPartyVenue = 3,
			Exchangex = 4,
			ATS = 5,
			Desks = 6,
			Router = 7,
			Algo = 8
		}		
		#endregion		

	}
}



