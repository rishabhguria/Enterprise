#region Using

using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Controls;
using Nirvana.Admin.Utility;
using System.Text.RegularExpressions;
using Infragistics.Win;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

#endregion 

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for CompanyMaster.
	/// </summary>
	public class CompanyMaster : System.Windows.Forms.Form
	{
		#region Constants definitions
		
		const string C_COMBO_SELECT = "- Select -";
		private const string FORM_NAME = "CompanyMaster : ";

		//Tab Constants defined by user.
		const int C_TAB_COMPANYSETUP = 0;
		const int C_TAB_COMPANYCOUNTERPARTIES = 1;
		const int C_TAB_COMPANYUSER = 2;
		const int C_TAB_COMPANYCLIENTS = 3;
		const int C_TAB_COMPANYTHIRDPARTIES = 4;

		const int C_TAB_COMPANYDETAIL = 0;
		const int C_TAB_PERMISSIONLEVEL = 1;
		const int C_TAB_INTERNALACCOUNTS = 2;	

		const int C_TAB_USERDETAIL = 0;
		const int C_TAB_USERPERMISSION = 1;

		const int C_TAB_CLIENTCOMPANY = 0;
		const int C_TAB_CLIENTFUNDS = 1;
		const int C_TAB_CLIENTTRADERS = 2;

		const int C_TAB_COUNTERPARTYCOMPANYLEVELTAG = 1;

		const int C_TYPE_PRIMEBROKERCLEARER = 1;
		const int C_TYPE_VENDOR = 2;
		const int C_TYPE_CUSTODIAN = 3;
		const int C_TYPE_ADMINISTRATOR = 4;

		#endregion
		
		#region Private and Protected


		
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.TreeView trvCompanyMaster;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		
		private System.Windows.Forms.ErrorProvider errorProvider1;
		//private Nirvana.Admin.Controls.GridClientFix uctGridClientFix;
		//private Nirvana.Admin.Controls.CompanyCompliance uctCompanyCompliance;
		//private Nirvana.Admin.Controls.CompanyUser uctCompanyUserDetail;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCompanyMaster;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCompanySetup;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCompanyCounterParties;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCompanyUser;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage4;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl13;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCompanyClients;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage5;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl14;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl15;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl16;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl17;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl18;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl19;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl20;
		private System.Windows.Forms.GroupBox gpbDetails;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox txtTelephone;
		private System.Windows.Forms.TextBox txtFax;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtCompanyName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gpbSecondaryContact;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.TextBox txtPC2Telephone;
		private System.Windows.Forms.TextBox txtPC2Cell;
		private System.Windows.Forms.TextBox txtPC2Title;
		private System.Windows.Forms.TextBox txtPC2LastName;
		private System.Windows.Forms.TextBox txtPC2FirstName;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.TextBox txtPC2Email;
		private System.Windows.Forms.GroupBox gpbPrimaryContact;
		private System.Windows.Forms.TextBox txtPC1FirstName;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox txtPC1LastName;
		private System.Windows.Forms.TextBox txtPC1Title;
		private System.Windows.Forms.TextBox txtPC1Cell;
		private System.Windows.Forms.TextBox txtPC1Telephone;
		private System.Windows.Forms.TextBox txtPC1Email;
		private System.Windows.Forms.GroupBox gpbTechnologyContact;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.TextBox txtTCTelephone;
		private System.Windows.Forms.TextBox txtTCCell;
		private System.Windows.Forms.TextBox txtTCTitle;
		private System.Windows.Forms.TextBox txtTCFirstName;
		private System.Windows.Forms.TextBox txtTCLastName;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox txtTCEmail;
		private System.Windows.Forms.GroupBox grpCompliance;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckedListBox checkedlstCounterParties;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckedListBox checkedlstThirdPartyComponents;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckedListBox checkedlstAssetsUnderlyings;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.CheckedListBox checkedlstApplicationComponents;
//		private Nirvana.Admin.Controls.GridClientTradingAccounts gridClientTradingAccounts1;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCompanyTYpe;
		private System.Windows.Forms.Button btnCompanyCouterPartiesSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnCompanyCouterPartiesClose;
		private System.Windows.Forms.Button btnSaveCompanySetup;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdMPID;
		private System.Windows.Forms.Button btnSaveCompanyUser;
		private System.Windows.Forms.Button btnCloseCompanyUser;
		private System.Windows.Forms.Button btnCloseCompanyClients;
		private System.Windows.Forms.Button btnCompanyClientsSave;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCompanyThirdParty;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage6;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl21;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl23;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl24;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
		private System.Windows.Forms.Button btnCloseCompanyThirdParty;
		private System.Windows.Forms.Button btnSaveCompanyThirdParty;
		private System.Windows.Forms.GroupBox grpCompanyInternalAccounts;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.GroupBox grpCompanyCompliance;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.GroupBox grpCompanyVenue;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.GroupBox grpCompanyCounterParties;
		private System.Windows.Forms.GroupBox grpCompanyUser;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.GroupBox grpCompanyUserPermission;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.GroupBox grpCompanyClientDetails;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.GroupBox grpCompanyClientFunds;
		private System.Windows.Forms.Label label44;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.GroupBox grpCompanyClientTradingAccounts;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.GroupBox grpCompanyClientClearer;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Label label48;
		private Nirvana.Admin.Controls.GridStrategy uctCompanyStrategy;
		private Nirvana.Admin.Controls.GridClearingFirmsPrimeBrokers uctClearingFirmsPrimeBrokers;
		private Nirvana.Admin.Controls.GridTradingAccounts uctCompanyTradingAccounts;
		private Nirvana.Admin.Controls.GridFunds uctCompanyFunds;
		//private Nirvana.Admin.Controls.CompanyCompliance uctCompanyCompliance;
		//private Nirvana.Admin.Controls.CompanyVenue uctCompanyVenue;
		//private Nirvana.Admin.Controls.CompanyUser uctCompanyUserDetail;
		private Nirvana.Admin.Controls.CompanyUserPermissions uctCompanyUserPermissions;
		private Nirvana.Admin.Controls.ClientCompany uctClientCompanyControl;
		private Nirvana.Admin.Controls.GridClientFunds uctGridClientFunds1;
		private Nirvana.Admin.Controls.GridClientTraders uctClientTradersDetail;
		private Nirvana.Admin.Controls.GridClientTradingAccounts uctClientTradingAccounts;
		private Nirvana.Admin.Controls.ClientClearer uctClientClearer;
		private Nirvana.Admin.Controls.GridClientFix uctGridClientFix;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.Label label49;
		private Nirvana.Admin.Controls.ClientPermission uctClientPermission;
		//private Nirvana.Admin.Controls.GridCompanyCounterPartiesCompanyLevelTag uctGridCompanyCountePartiesCompanyLevelTags;
		private Nirvana.Admin.Controls.CompanyCompliance uctCompanyCompliance;
		private Nirvana.Admin.Controls.CompanyVenue uctCompanyVenue;
		private Nirvana.Admin.Controls.CompanyUser uctCompanyUserDetail;
		private Nirvana.Admin.Controls.GridCompanyCounterPartiesCompanyLevelTag uctGridCompanyCountePartiesCompanyLevelTags;
		private System.Windows.Forms.GroupBox groupBox10;
		private Nirvana.Admin.Controls.CompanyThirdPartyFileFormats uctCompanyThirdPartyFileFormats;
		private System.Windows.Forms.GroupBox groupBox11;
		private Nirvana.Admin.Controls.CompanyThirdPartyCVIdentifier uctCompanyThirdPartyCVIdentifier;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl22;
		private System.Windows.Forms.GroupBox grpThirdPartyFileFormat;
		private Nirvana.Admin.Controls.CompanyThirdPartyMappingDetails uctCompanyThirdPartyMappingDetails;
		private System.Windows.Forms.GroupBox grpThirdPartyCommissionRules;
		private System.Windows.Forms.GroupBox groupBox12;
		private Nirvana.Admin.Controls.CompanyThirdPartyCommissionRules uctCompanyThirdPartyCommissionRules;
		//private Nirvana.Admin.Controls.CompanyThirdPartyCommissionRules uctCompanyThirdPartyFundCommissonRules;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		#endregion

		public CompanyMaster()
		{			
			InitializeComponent();
			//This method is called to setup the menu permissions when the constructor is called to know 
			//beforehand that whether the current user has the rights to manitain the company module or
			//just look the information in this.
			SetUpMenuPermissions();
			BindCompanyType();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance("CurrencyAdd");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyMPID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MPID", 2, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyTypeID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1);
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CompanyMaster));
			Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab13 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab14 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab15 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab16 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab17 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab18 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab19 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab20 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab21 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab22 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab23 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab24 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompliance = new System.Windows.Forms.GroupBox();
			this.grdMPID = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.gpbSecondaryContact = new System.Windows.Forms.GroupBox();
			this.label38 = new System.Windows.Forms.Label();
			this.txtPC2Telephone = new System.Windows.Forms.TextBox();
			this.txtPC2Cell = new System.Windows.Forms.TextBox();
			this.txtPC2Title = new System.Windows.Forms.TextBox();
			this.txtPC2LastName = new System.Windows.Forms.TextBox();
			this.txtPC2FirstName = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.txtPC2Email = new System.Windows.Forms.TextBox();
			this.gpbPrimaryContact = new System.Windows.Forms.GroupBox();
			this.txtPC1FirstName = new System.Windows.Forms.TextBox();
			this.label37 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.txtPC1LastName = new System.Windows.Forms.TextBox();
			this.txtPC1Title = new System.Windows.Forms.TextBox();
			this.txtPC1Cell = new System.Windows.Forms.TextBox();
			this.txtPC1Telephone = new System.Windows.Forms.TextBox();
			this.txtPC1Email = new System.Windows.Forms.TextBox();
			this.gpbTechnologyContact = new System.Windows.Forms.GroupBox();
			this.label39 = new System.Windows.Forms.Label();
			this.txtTCTelephone = new System.Windows.Forms.TextBox();
			this.txtTCCell = new System.Windows.Forms.TextBox();
			this.txtTCTitle = new System.Windows.Forms.TextBox();
			this.txtTCFirstName = new System.Windows.Forms.TextBox();
			this.txtTCLastName = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.txtTCEmail = new System.Windows.Forms.TextBox();
			this.gpbDetails = new System.Windows.Forms.GroupBox();
			this.label36 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.txtTelephone = new System.Windows.Forms.TextBox();
			this.txtFax = new System.Windows.Forms.TextBox();
			this.txtAddress2 = new System.Windows.Forms.TextBox();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtCompanyName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbCompanyTYpe = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkedlstThirdPartyComponents = new System.Windows.Forms.CheckedListBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.checkedlstAssetsUnderlyings = new System.Windows.Forms.CheckedListBox();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.checkedlstApplicationComponents = new System.Windows.Forms.CheckedListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkedlstCounterParties = new System.Windows.Forms.CheckedListBox();
			this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyInternalAccounts = new System.Windows.Forms.GroupBox();
			this.uctCompanyStrategy = new Nirvana.Admin.Controls.GridStrategy();
			this.uctClearingFirmsPrimeBrokers = new Nirvana.Admin.Controls.GridClearingFirmsPrimeBrokers();
			this.uctCompanyTradingAccounts = new Nirvana.Admin.Controls.GridTradingAccounts();
			this.uctCompanyFunds = new Nirvana.Admin.Controls.GridFunds();
			this.label25 = new System.Windows.Forms.Label();
			this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyCompliance = new System.Windows.Forms.GroupBox();
			this.uctCompanyCompliance = new Nirvana.Admin.Controls.CompanyCompliance();
			this.label26 = new System.Windows.Forms.Label();
			this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyVenue = new System.Windows.Forms.GroupBox();
			this.uctCompanyVenue = new Nirvana.Admin.Controls.CompanyVenue();
			this.label27 = new System.Windows.Forms.Label();
			this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyCounterParties = new System.Windows.Forms.GroupBox();
			this.uctGridCompanyCountePartiesCompanyLevelTags = new Nirvana.Admin.Controls.GridCompanyCounterPartiesCompanyLevelTag();
			this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyUser = new System.Windows.Forms.GroupBox();
			this.uctCompanyUserDetail = new Nirvana.Admin.Controls.CompanyUser();
			this.label41 = new System.Windows.Forms.Label();
			this.ultraTabPageControl13 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyUserPermission = new System.Windows.Forms.GroupBox();
			this.uctCompanyUserPermissions = new Nirvana.Admin.Controls.CompanyUserPermissions();
			this.label42 = new System.Windows.Forms.Label();
			this.ultraTabPageControl14 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyClientDetails = new System.Windows.Forms.GroupBox();
			this.uctClientCompanyControl = new Nirvana.Admin.Controls.ClientCompany();
			this.label43 = new System.Windows.Forms.Label();
			this.ultraTabPageControl15 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyClientFunds = new System.Windows.Forms.GroupBox();
			this.uctGridClientFunds1 = new Nirvana.Admin.Controls.GridClientFunds();
			this.label44 = new System.Windows.Forms.Label();
			this.ultraTabPageControl17 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.uctClientTradersDetail = new Nirvana.Admin.Controls.GridClientTraders();
			this.label45 = new System.Windows.Forms.Label();
			this.ultraTabPageControl16 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyClientTradingAccounts = new System.Windows.Forms.GroupBox();
			this.uctClientTradingAccounts = new Nirvana.Admin.Controls.GridClientTradingAccounts();
			this.label46 = new System.Windows.Forms.Label();
			this.ultraTabPageControl18 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpCompanyClientClearer = new System.Windows.Forms.GroupBox();
			this.uctClientClearer = new Nirvana.Admin.Controls.ClientClearer();
			this.label47 = new System.Windows.Forms.Label();
			this.ultraTabPageControl19 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.uctGridClientFix = new Nirvana.Admin.Controls.GridClientFix();
			this.label48 = new System.Windows.Forms.Label();
			this.ultraTabPageControl20 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.uctClientPermission = new Nirvana.Admin.Controls.ClientPermission();
			this.label49 = new System.Windows.Forms.Label();
			this.ultraTabPageControl21 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.uctCompanyThirdPartyFileFormats = new Nirvana.Admin.Controls.CompanyThirdPartyFileFormats();
			this.ultraTabPageControl22 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpThirdPartyFileFormat = new System.Windows.Forms.GroupBox();
			this.uctCompanyThirdPartyMappingDetails = new Nirvana.Admin.Controls.CompanyThirdPartyMappingDetails();
			this.ultraTabPageControl23 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.uctCompanyThirdPartyCVIdentifier = new Nirvana.Admin.Controls.CompanyThirdPartyCVIdentifier();
			this.ultraTabPageControl24 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.tbcCompanySetup = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.btnSaveCompanySetup = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.tbcCompanyCounterParties = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.btnCompanyCouterPartiesSave = new System.Windows.Forms.Button();
			this.btnCompanyCouterPartiesClose = new System.Windows.Forms.Button();
			this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.btnCloseCompanyUser = new System.Windows.Forms.Button();
			this.btnSaveCompanyUser = new System.Windows.Forms.Button();
			this.tbcCompanyUser = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage4 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.btnCloseCompanyClients = new System.Windows.Forms.Button();
			this.btnCompanyClientsSave = new System.Windows.Forms.Button();
			this.tbcCompanyClients = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage5 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.btnCloseCompanyThirdParty = new System.Windows.Forms.Button();
			this.btnSaveCompanyThirdParty = new System.Windows.Forms.Button();
			this.tbcCompanyThirdParty = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage6 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.grpThirdPartyCommissionRules = new System.Windows.Forms.GroupBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.trvCompanyMaster = new System.Windows.Forms.TreeView();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.tbcCompanyMaster = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.uctCompanyThirdPartyCommissionRules = new Nirvana.Admin.Controls.CompanyThirdPartyCommissionRules();
			this.ultraTabPageControl6.SuspendLayout();
			this.grpCompliance.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdMPID)).BeginInit();
			this.gpbSecondaryContact.SuspendLayout();
			this.gpbPrimaryContact.SuspendLayout();
			this.gpbTechnologyContact.SuspendLayout();
			this.gpbDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTYpe)).BeginInit();
			this.ultraTabPageControl7.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.ultraTabPageControl8.SuspendLayout();
			this.grpCompanyInternalAccounts.SuspendLayout();
			this.ultraTabPageControl9.SuspendLayout();
			this.grpCompanyCompliance.SuspendLayout();
			this.ultraTabPageControl10.SuspendLayout();
			this.grpCompanyVenue.SuspendLayout();
			this.ultraTabPageControl11.SuspendLayout();
			this.grpCompanyCounterParties.SuspendLayout();
			this.ultraTabPageControl12.SuspendLayout();
			this.grpCompanyUser.SuspendLayout();
			this.ultraTabPageControl13.SuspendLayout();
			this.grpCompanyUserPermission.SuspendLayout();
			this.ultraTabPageControl14.SuspendLayout();
			this.grpCompanyClientDetails.SuspendLayout();
			this.ultraTabPageControl15.SuspendLayout();
			this.grpCompanyClientFunds.SuspendLayout();
			this.ultraTabPageControl17.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.ultraTabPageControl16.SuspendLayout();
			this.grpCompanyClientTradingAccounts.SuspendLayout();
			this.ultraTabPageControl18.SuspendLayout();
			this.grpCompanyClientClearer.SuspendLayout();
			this.ultraTabPageControl19.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.ultraTabPageControl20.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.ultraTabPageControl21.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.ultraTabPageControl22.SuspendLayout();
			this.grpThirdPartyFileFormat.SuspendLayout();
			this.ultraTabPageControl23.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.ultraTabPageControl24.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.ultraTabPageControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanySetup)).BeginInit();
			this.tbcCompanySetup.SuspendLayout();
			this.ultraTabPageControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyCounterParties)).BeginInit();
			this.tbcCompanyCounterParties.SuspendLayout();
			this.ultraTabPageControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyUser)).BeginInit();
			this.tbcCompanyUser.SuspendLayout();
			this.ultraTabPageControl4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyClients)).BeginInit();
			this.tbcCompanyClients.SuspendLayout();
			this.ultraTabPageControl5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyThirdParty)).BeginInit();
			this.tbcCompanyThirdParty.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyMaster)).BeginInit();
			this.tbcCompanyMaster.SuspendLayout();
			this.SuspendLayout();
			// 
			// ultraTabPageControl6
			// 
			this.ultraTabPageControl6.Controls.Add(this.grpCompliance);
			this.ultraTabPageControl6.Controls.Add(this.gpbSecondaryContact);
			this.ultraTabPageControl6.Controls.Add(this.gpbPrimaryContact);
			this.ultraTabPageControl6.Controls.Add(this.gpbTechnologyContact);
			this.ultraTabPageControl6.Controls.Add(this.gpbDetails);
			this.ultraTabPageControl6.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl6.Name = "ultraTabPageControl6";
			this.ultraTabPageControl6.Size = new System.Drawing.Size(672, 499);
			this.ultraTabPageControl6.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl6_Paint);
			// 
			// grpCompliance
			// 
			this.grpCompliance.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.grpCompliance.Controls.Add(this.grdMPID);
			this.grpCompliance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpCompliance.Location = new System.Drawing.Point(341, 374);
			this.grpCompliance.Name = "grpCompliance";
			this.grpCompliance.Size = new System.Drawing.Size(321, 122);
			this.grpCompliance.TabIndex = 31;
			this.grpCompliance.TabStop = false;
			this.grpCompliance.Text = "MPID";
			// 
			// grdMPID
			// 
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
			this.grdMPID.DisplayLayout.AddNewBox.ButtonAppearance = appearance1;
			this.grdMPID.DisplayLayout.AddNewBox.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
			this.grdMPID.DisplayLayout.AddNewBox.Prompt = "Add new Row";
			appearance2.BackColor = System.Drawing.Color.White;
			appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.grdMPID.DisplayLayout.Appearance = appearance2;
			this.grdMPID.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Hidden = true;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Width = 113;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3});
			appearance3.BackColor = System.Drawing.SystemColors.Control;
			appearance3.ForeColor = System.Drawing.SystemColors.Control;
			ultraGridBand1.Header.Appearance = appearance3;
			appearance4.BackColor = System.Drawing.Color.LemonChiffon;
			ultraGridBand1.Override.ActiveCellAppearance = appearance4;
			appearance5.BackColor = System.Drawing.Color.LemonChiffon;
			ultraGridBand1.Override.ActiveRowAppearance = appearance5;
			ultraGridBand1.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.FixedAddRowOnTop;
			ultraGridBand1.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
			ultraGridBand1.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
			ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
			ultraGridBand1.Override.TemplateAddRowPrompt = "Add an MPID.";
			this.grdMPID.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdMPID.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.grdMPID.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			this.grdMPID.DisplayLayout.GroupByBox.Hidden = true;
			this.grdMPID.DisplayLayout.MaxColScrollRegions = 1;
			this.grdMPID.DisplayLayout.MaxRowScrollRegions = 1;
			appearance6.BackColor = System.Drawing.SystemColors.Highlight;
			appearance6.ForeColor = System.Drawing.Color.Black;
			this.grdMPID.DisplayLayout.Override.ActiveCellAppearance = appearance6;
			appearance7.BackColor = System.Drawing.SystemColors.Highlight;
			appearance7.ForeColor = System.Drawing.Color.Black;
			this.grdMPID.DisplayLayout.Override.ActiveRowAppearance = appearance7;
			this.grdMPID.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
			this.grdMPID.DisplayLayout.Override.CellPadding = 0;
			appearance8.TextHAlign = Infragistics.Win.HAlign.Left;
			this.grdMPID.DisplayLayout.Override.HeaderAppearance = appearance8;
			this.grdMPID.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdMPID.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdMPID.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdMPID.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdMPID.FlatMode = true;
			this.grdMPID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
			this.grdMPID.Location = new System.Drawing.Point(141, 16);
			this.grdMPID.Name = "grdMPID";
			this.grdMPID.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
			this.grdMPID.Size = new System.Drawing.Size(150, 100);
			this.grdMPID.TabIndex = 33;
			this.grdMPID.TabStop = false;
			this.grdMPID.Text = "Currency Grid";
			// 
			// gpbSecondaryContact
			// 
			this.gpbSecondaryContact.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gpbSecondaryContact.Controls.Add(this.label38);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Telephone);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Cell);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Title);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2LastName);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2FirstName);
			this.gpbSecondaryContact.Controls.Add(this.label30);
			this.gpbSecondaryContact.Controls.Add(this.label31);
			this.gpbSecondaryContact.Controls.Add(this.label32);
			this.gpbSecondaryContact.Controls.Add(this.label33);
			this.gpbSecondaryContact.Controls.Add(this.label34);
			this.gpbSecondaryContact.Controls.Add(this.label35);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Email);
			this.gpbSecondaryContact.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.gpbSecondaryContact.Location = new System.Drawing.Point(341, 6);
			this.gpbSecondaryContact.Name = "gpbSecondaryContact";
			this.gpbSecondaryContact.Size = new System.Drawing.Size(321, 182);
			this.gpbSecondaryContact.TabIndex = 15;
			this.gpbSecondaryContact.TabStop = false;
			this.gpbSecondaryContact.Text = "Secondary Contact";
			// 
			// label38
			// 
			this.label38.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label38.Location = new System.Drawing.Point(29, 138);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(98, 14);
			this.label38.TabIndex = 39;
			this.label38.Text = "(1-111-111111)";
			// 
			// txtPC2Telephone
			// 
			this.txtPC2Telephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC2Telephone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC2Telephone.Location = new System.Drawing.Point(141, 118);
			this.txtPC2Telephone.MaxLength = 50;
			this.txtPC2Telephone.Name = "txtPC2Telephone";
			this.txtPC2Telephone.Size = new System.Drawing.Size(150, 21);
			this.txtPC2Telephone.TabIndex = 22;
			this.txtPC2Telephone.Text = "34";
			this.txtPC2Telephone.LostFocus += new System.EventHandler(this.txtPC2Telephone_LostFocus);
			this.txtPC2Telephone.GotFocus += new System.EventHandler(this.txtPC2Telephone_GotFocus);
			// 
			// txtPC2Cell
			// 
			this.txtPC2Cell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC2Cell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC2Cell.Location = new System.Drawing.Point(141, 150);
			this.txtPC2Cell.MaxLength = 50;
			this.txtPC2Cell.Name = "txtPC2Cell";
			this.txtPC2Cell.Size = new System.Drawing.Size(150, 21);
			this.txtPC2Cell.TabIndex = 23;
			this.txtPC2Cell.Text = "34";
			this.txtPC2Cell.LostFocus += new System.EventHandler(this.txtPC2Cell_LostFocus);
			this.txtPC2Cell.GotFocus += new System.EventHandler(this.txtPC2Cell_GotFocus);
			// 
			// txtPC2Title
			// 
			this.txtPC2Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC2Title.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC2Title.Location = new System.Drawing.Point(141, 70);
			this.txtPC2Title.MaxLength = 50;
			this.txtPC2Title.Name = "txtPC2Title";
			this.txtPC2Title.Size = new System.Drawing.Size(150, 21);
			this.txtPC2Title.TabIndex = 20;
			this.txtPC2Title.Text = "34";
			this.txtPC2Title.LostFocus += new System.EventHandler(this.txtPC2Title_LostFocus);
			this.txtPC2Title.GotFocus += new System.EventHandler(this.txtPC2Title_GotFocus);
			// 
			// txtPC2LastName
			// 
			this.txtPC2LastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC2LastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC2LastName.Location = new System.Drawing.Point(141, 46);
			this.txtPC2LastName.MaxLength = 50;
			this.txtPC2LastName.Name = "txtPC2LastName";
			this.txtPC2LastName.Size = new System.Drawing.Size(150, 21);
			this.txtPC2LastName.TabIndex = 19;
			this.txtPC2LastName.Text = "34";
			this.txtPC2LastName.LostFocus += new System.EventHandler(this.txtPC2LastName_LostFocus);
			this.txtPC2LastName.GotFocus += new System.EventHandler(this.txtPC2LastName_GotFocus);
			// 
			// txtPC2FirstName
			// 
			this.txtPC2FirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC2FirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC2FirstName.Location = new System.Drawing.Point(141, 21);
			this.txtPC2FirstName.MaxLength = 50;
			this.txtPC2FirstName.Name = "txtPC2FirstName";
			this.txtPC2FirstName.Size = new System.Drawing.Size(150, 21);
			this.txtPC2FirstName.TabIndex = 18;
			this.txtPC2FirstName.Text = "34";
			this.txtPC2FirstName.LostFocus += new System.EventHandler(this.txtPC2FirstName_LostFocus);
			this.txtPC2FirstName.GotFocus += new System.EventHandler(this.txtPC2FirstName_GotFocus);
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label30.Location = new System.Drawing.Point(29, 96);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(44, 16);
			this.label30.TabIndex = 15;
			this.label30.Text = "EMail";
			// 
			// label31
			// 
			this.label31.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label31.Location = new System.Drawing.Point(29, 124);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(54, 12);
			this.label31.TabIndex = 16;
			this.label31.Text = "Work #";
			// 
			// label32
			// 
			this.label32.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label32.Location = new System.Drawing.Point(29, 154);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(40, 14);
			this.label32.TabIndex = 17;
			this.label32.Text = "Cell #";
			// 
			// label33
			// 
			this.label33.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label33.Location = new System.Drawing.Point(29, 25);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(76, 15);
			this.label33.TabIndex = 12;
			this.label33.Text = "First Name";
			// 
			// label34
			// 
			this.label34.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label34.Location = new System.Drawing.Point(29, 48);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(76, 16);
			this.label34.TabIndex = 13;
			this.label34.Text = "Last Name";
			// 
			// label35
			// 
			this.label35.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label35.Location = new System.Drawing.Point(29, 74);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(34, 14);
			this.label35.TabIndex = 14;
			this.label35.Text = "Title";
			// 
			// txtPC2Email
			// 
			this.txtPC2Email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC2Email.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC2Email.Location = new System.Drawing.Point(141, 94);
			this.txtPC2Email.MaxLength = 50;
			this.txtPC2Email.Name = "txtPC2Email";
			this.txtPC2Email.Size = new System.Drawing.Size(150, 21);
			this.txtPC2Email.TabIndex = 21;
			this.txtPC2Email.Text = "34";
			this.txtPC2Email.LostFocus += new System.EventHandler(this.txtPC2Email_LostFocus);
			this.txtPC2Email.GotFocus += new System.EventHandler(this.txtPC2Email_GotFocus);
			// 
			// gpbPrimaryContact
			// 
			this.gpbPrimaryContact.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gpbPrimaryContact.Controls.Add(this.txtPC1FirstName);
			this.gpbPrimaryContact.Controls.Add(this.label37);
			this.gpbPrimaryContact.Controls.Add(this.label29);
			this.gpbPrimaryContact.Controls.Add(this.label28);
			this.gpbPrimaryContact.Controls.Add(this.label19);
			this.gpbPrimaryContact.Controls.Add(this.label13);
			this.gpbPrimaryContact.Controls.Add(this.label14);
			this.gpbPrimaryContact.Controls.Add(this.label15);
			this.gpbPrimaryContact.Controls.Add(this.label16);
			this.gpbPrimaryContact.Controls.Add(this.label17);
			this.gpbPrimaryContact.Controls.Add(this.label18);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1LastName);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Title);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Cell);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Telephone);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Email);
			this.gpbPrimaryContact.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.gpbPrimaryContact.Location = new System.Drawing.Point(8, 186);
			this.gpbPrimaryContact.Name = "gpbPrimaryContact";
			this.gpbPrimaryContact.Size = new System.Drawing.Size(330, 182);
			this.gpbPrimaryContact.TabIndex = 13;
			this.gpbPrimaryContact.TabStop = false;
			this.gpbPrimaryContact.Text = "Primary Contact";
			// 
			// txtPC1FirstName
			// 
			this.txtPC1FirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC1FirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC1FirstName.Location = new System.Drawing.Point(150, 18);
			this.txtPC1FirstName.MaxLength = 50;
			this.txtPC1FirstName.Name = "txtPC1FirstName";
			this.txtPC1FirstName.Size = new System.Drawing.Size(150, 21);
			this.txtPC1FirstName.TabIndex = 40;
			this.txtPC1FirstName.Text = "34";
			this.txtPC1FirstName.LostFocus += new System.EventHandler(this.txtPC1FirstName_LostFocus);
			this.txtPC1FirstName.GotFocus += new System.EventHandler(this.txtPC1FirstName_GotFocus);
			// 
			// label37
			// 
			this.label37.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label37.Location = new System.Drawing.Point(30, 122);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(84, 14);
			this.label37.TabIndex = 39;
			this.label37.Text = "(1-111-111111)";
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label29.ForeColor = System.Drawing.Color.Red;
			this.label29.Location = new System.Drawing.Point(74, 108);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(12, 10);
			this.label29.TabIndex = 22;
			this.label29.Text = "*";
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label28.ForeColor = System.Drawing.Color.Red;
			this.label28.Location = new System.Drawing.Point(62, 86);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(12, 8);
			this.label28.TabIndex = 21;
			this.label28.Text = "*";
			// 
			// label19
			// 
			this.label19.ForeColor = System.Drawing.Color.Red;
			this.label19.Location = new System.Drawing.Point(94, 22);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(12, 9);
			this.label19.TabIndex = 18;
			this.label19.Text = "*";
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label13.Location = new System.Drawing.Point(30, 21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(64, 14);
			this.label13.TabIndex = 12;
			this.label13.Text = "First Name";
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label14.Location = new System.Drawing.Point(30, 42);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(60, 16);
			this.label14.TabIndex = 13;
			this.label14.Text = "Last Name";
			// 
			// label15
			// 
			this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label15.Location = new System.Drawing.Point(30, 64);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(32, 14);
			this.label15.TabIndex = 14;
			this.label15.Text = "Title";
			// 
			// label16
			// 
			this.label16.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label16.Location = new System.Drawing.Point(30, 86);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(32, 16);
			this.label16.TabIndex = 15;
			this.label16.Text = "EMail";
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label17.Location = new System.Drawing.Point(30, 108);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(46, 14);
			this.label17.TabIndex = 16;
			this.label17.Text = "Work #";
			// 
			// label18
			// 
			this.label18.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label18.Location = new System.Drawing.Point(30, 138);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(42, 14);
			this.label18.TabIndex = 17;
			this.label18.Text = "Cell #";
			// 
			// txtPC1LastName
			// 
			this.txtPC1LastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC1LastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC1LastName.Location = new System.Drawing.Point(150, 40);
			this.txtPC1LastName.MaxLength = 50;
			this.txtPC1LastName.Name = "txtPC1LastName";
			this.txtPC1LastName.Size = new System.Drawing.Size(150, 21);
			this.txtPC1LastName.TabIndex = 13;
			this.txtPC1LastName.Text = "34";
			this.txtPC1LastName.LostFocus += new System.EventHandler(this.txtPC1LastName_LostFocus);
			this.txtPC1LastName.GotFocus += new System.EventHandler(this.txtPC1LastName_GotFocus);
			// 
			// txtPC1Title
			// 
			this.txtPC1Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC1Title.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC1Title.Location = new System.Drawing.Point(150, 62);
			this.txtPC1Title.MaxLength = 50;
			this.txtPC1Title.Name = "txtPC1Title";
			this.txtPC1Title.Size = new System.Drawing.Size(150, 21);
			this.txtPC1Title.TabIndex = 14;
			this.txtPC1Title.Text = "34";
			this.txtPC1Title.LostFocus += new System.EventHandler(this.txtPC1Title_LostFocus);
			this.txtPC1Title.GotFocus += new System.EventHandler(this.txtPC1Title_GotFocus);
			// 
			// txtPC1Cell
			// 
			this.txtPC1Cell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC1Cell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC1Cell.Location = new System.Drawing.Point(150, 136);
			this.txtPC1Cell.MaxLength = 50;
			this.txtPC1Cell.Name = "txtPC1Cell";
			this.txtPC1Cell.Size = new System.Drawing.Size(150, 21);
			this.txtPC1Cell.TabIndex = 17;
			this.txtPC1Cell.Text = "34";
			this.txtPC1Cell.LostFocus += new System.EventHandler(this.txtPC1Cell_LostFocus);
			this.txtPC1Cell.GotFocus += new System.EventHandler(this.txtPC1Cell_GotFocus);
			// 
			// txtPC1Telephone
			// 
			this.txtPC1Telephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC1Telephone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC1Telephone.Location = new System.Drawing.Point(150, 106);
			this.txtPC1Telephone.MaxLength = 50;
			this.txtPC1Telephone.Name = "txtPC1Telephone";
			this.txtPC1Telephone.Size = new System.Drawing.Size(150, 21);
			this.txtPC1Telephone.TabIndex = 16;
			this.txtPC1Telephone.Text = "34";
			this.txtPC1Telephone.LostFocus += new System.EventHandler(this.txtPC1Telephone_LostFocus);
			this.txtPC1Telephone.GotFocus += new System.EventHandler(this.txtPC1Telephone_GotFocus);
			// 
			// txtPC1Email
			// 
			this.txtPC1Email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPC1Email.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPC1Email.Location = new System.Drawing.Point(150, 84);
			this.txtPC1Email.MaxLength = 50;
			this.txtPC1Email.Name = "txtPC1Email";
			this.txtPC1Email.Size = new System.Drawing.Size(150, 21);
			this.txtPC1Email.TabIndex = 15;
			this.txtPC1Email.Text = "34";
			this.txtPC1Email.LostFocus += new System.EventHandler(this.txtPC1Email_LostFocus);
			this.txtPC1Email.GotFocus += new System.EventHandler(this.txtPC1Email_GotFocus);
			// 
			// gpbTechnologyContact
			// 
			this.gpbTechnologyContact.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gpbTechnologyContact.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.gpbTechnologyContact.Controls.Add(this.label39);
			this.gpbTechnologyContact.Controls.Add(this.txtTCTelephone);
			this.gpbTechnologyContact.Controls.Add(this.txtTCCell);
			this.gpbTechnologyContact.Controls.Add(this.txtTCTitle);
			this.gpbTechnologyContact.Controls.Add(this.txtTCFirstName);
			this.gpbTechnologyContact.Controls.Add(this.txtTCLastName);
			this.gpbTechnologyContact.Controls.Add(this.label7);
			this.gpbTechnologyContact.Controls.Add(this.label8);
			this.gpbTechnologyContact.Controls.Add(this.label9);
			this.gpbTechnologyContact.Controls.Add(this.label10);
			this.gpbTechnologyContact.Controls.Add(this.label11);
			this.gpbTechnologyContact.Controls.Add(this.label12);
			this.gpbTechnologyContact.Controls.Add(this.txtTCEmail);
			this.gpbTechnologyContact.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.gpbTechnologyContact.Location = new System.Drawing.Point(341, 186);
			this.gpbTechnologyContact.Name = "gpbTechnologyContact";
			this.gpbTechnologyContact.Size = new System.Drawing.Size(321, 182);
			this.gpbTechnologyContact.TabIndex = 14;
			this.gpbTechnologyContact.TabStop = false;
			this.gpbTechnologyContact.Text = "Technology Contact";
			// 
			// label39
			// 
			this.label39.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label39.Location = new System.Drawing.Point(29, 128);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(88, 18);
			this.label39.TabIndex = 39;
			this.label39.Text = "(1-111-111111)";
			// 
			// txtTCTelephone
			// 
			this.txtTCTelephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTCTelephone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTCTelephone.Location = new System.Drawing.Point(141, 112);
			this.txtTCTelephone.MaxLength = 50;
			this.txtTCTelephone.Name = "txtTCTelephone";
			this.txtTCTelephone.Size = new System.Drawing.Size(150, 21);
			this.txtTCTelephone.TabIndex = 28;
			this.txtTCTelephone.Text = "34";
			this.txtTCTelephone.LostFocus += new System.EventHandler(this.txtTCTelephone_LostFocus);
			this.txtTCTelephone.GotFocus += new System.EventHandler(this.txtTCTelephone_GotFocus);
			// 
			// txtTCCell
			// 
			this.txtTCCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTCCell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTCCell.Location = new System.Drawing.Point(141, 144);
			this.txtTCCell.MaxLength = 50;
			this.txtTCCell.Name = "txtTCCell";
			this.txtTCCell.Size = new System.Drawing.Size(150, 21);
			this.txtTCCell.TabIndex = 29;
			this.txtTCCell.Text = "34";
			this.txtTCCell.LostFocus += new System.EventHandler(this.txtTCCell_LostFocus);
			this.txtTCCell.GotFocus += new System.EventHandler(this.txtTCCell_GotFocus);
			// 
			// txtTCTitle
			// 
			this.txtTCTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTCTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTCTitle.Location = new System.Drawing.Point(141, 64);
			this.txtTCTitle.MaxLength = 50;
			this.txtTCTitle.Name = "txtTCTitle";
			this.txtTCTitle.Size = new System.Drawing.Size(150, 21);
			this.txtTCTitle.TabIndex = 26;
			this.txtTCTitle.Text = "34";
			this.txtTCTitle.LostFocus += new System.EventHandler(this.txtTCTitle_LostFocus);
			this.txtTCTitle.GotFocus += new System.EventHandler(this.txtTCTitle_GotFocus);
			// 
			// txtTCFirstName
			// 
			this.txtTCFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTCFirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTCFirstName.Location = new System.Drawing.Point(141, 16);
			this.txtTCFirstName.MaxLength = 50;
			this.txtTCFirstName.Name = "txtTCFirstName";
			this.txtTCFirstName.Size = new System.Drawing.Size(150, 21);
			this.txtTCFirstName.TabIndex = 24;
			this.txtTCFirstName.Text = "34";
			this.txtTCFirstName.LostFocus += new System.EventHandler(this.txtTCFirstName_LostFocus);
			this.txtTCFirstName.GotFocus += new System.EventHandler(this.txtTCFirstName_GotFocus);
			// 
			// txtTCLastName
			// 
			this.txtTCLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTCLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTCLastName.Location = new System.Drawing.Point(141, 40);
			this.txtTCLastName.MaxLength = 50;
			this.txtTCLastName.Name = "txtTCLastName";
			this.txtTCLastName.Size = new System.Drawing.Size(150, 21);
			this.txtTCLastName.TabIndex = 25;
			this.txtTCLastName.Text = "34";
			this.txtTCLastName.LostFocus += new System.EventHandler(this.txtTCLastName_LostFocus);
			this.txtTCLastName.GotFocus += new System.EventHandler(this.txtTCLastName_GotFocus);
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label7.Location = new System.Drawing.Point(29, 144);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(76, 18);
			this.label7.TabIndex = 17;
			this.label7.Text = "Cell #";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label8.Location = new System.Drawing.Point(29, 20);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 16);
			this.label8.TabIndex = 12;
			this.label8.Text = "First Name";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label9.Location = new System.Drawing.Point(29, 48);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(64, 16);
			this.label9.TabIndex = 13;
			this.label9.Text = "Last Name";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label10.Location = new System.Drawing.Point(29, 64);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 12);
			this.label10.TabIndex = 14;
			this.label10.Text = "Title";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label11.Location = new System.Drawing.Point(29, 88);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 18);
			this.label11.TabIndex = 15;
			this.label11.Text = "EMail";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label12.Location = new System.Drawing.Point(29, 112);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(76, 18);
			this.label12.TabIndex = 16;
			this.label12.Text = "Work #";
			// 
			// txtTCEmail
			// 
			this.txtTCEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTCEmail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTCEmail.Location = new System.Drawing.Point(141, 88);
			this.txtTCEmail.MaxLength = 50;
			this.txtTCEmail.Name = "txtTCEmail";
			this.txtTCEmail.Size = new System.Drawing.Size(150, 21);
			this.txtTCEmail.TabIndex = 27;
			this.txtTCEmail.Text = "34";
			this.txtTCEmail.LostFocus += new System.EventHandler(this.txtTCEmail_LostFocus);
			this.txtTCEmail.GotFocus += new System.EventHandler(this.txtTCEmail_GotFocus);
			// 
			// gpbDetails
			// 
			this.gpbDetails.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gpbDetails.Controls.Add(this.label36);
			this.gpbDetails.Controls.Add(this.label24);
			this.gpbDetails.Controls.Add(this.label23);
			this.gpbDetails.Controls.Add(this.label22);
			this.gpbDetails.Controls.Add(this.label21);
			this.gpbDetails.Controls.Add(this.label20);
			this.gpbDetails.Controls.Add(this.txtTelephone);
			this.gpbDetails.Controls.Add(this.txtFax);
			this.gpbDetails.Controls.Add(this.txtAddress2);
			this.gpbDetails.Controls.Add(this.txtAddress1);
			this.gpbDetails.Controls.Add(this.txtCompanyName);
			this.gpbDetails.Controls.Add(this.label6);
			this.gpbDetails.Controls.Add(this.label5);
			this.gpbDetails.Controls.Add(this.label4);
			this.gpbDetails.Controls.Add(this.label3);
			this.gpbDetails.Controls.Add(this.label2);
			this.gpbDetails.Controls.Add(this.label1);
			this.gpbDetails.Controls.Add(this.cmbCompanyTYpe);
			this.gpbDetails.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.gpbDetails.Location = new System.Drawing.Point(8, 6);
			this.gpbDetails.Name = "gpbDetails";
			this.gpbDetails.Size = new System.Drawing.Size(330, 182);
			this.gpbDetails.TabIndex = 12;
			this.gpbDetails.TabStop = false;
			this.gpbDetails.Text = "Details";
			// 
			// label36
			// 
			this.label36.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label36.Location = new System.Drawing.Point(30, 134);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(102, 16);
			this.label36.TabIndex = 39;
			this.label36.Text = "(1-111-111111)";
			// 
			// label24
			// 
			this.label24.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label24.ForeColor = System.Drawing.Color.Red;
			this.label24.Location = new System.Drawing.Point(100, 44);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(12, 10);
			this.label24.TabIndex = 16;
			this.label24.Text = "*";
			// 
			// label23
			// 
			this.label23.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label23.ForeColor = System.Drawing.Color.Red;
			this.label23.Location = new System.Drawing.Point(128, 94);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(12, 10);
			this.label23.TabIndex = 15;
			this.label23.Text = "*";
			// 
			// label22
			// 
			this.label22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label22.ForeColor = System.Drawing.Color.Red;
			this.label22.Location = new System.Drawing.Point(82, 120);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(12, 8);
			this.label22.TabIndex = 14;
			this.label22.Text = "*";
			// 
			// label21
			// 
			this.label21.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label21.ForeColor = System.Drawing.Color.Red;
			this.label21.Location = new System.Drawing.Point(98, 70);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(8, 10);
			this.label21.TabIndex = 13;
			// 
			// label20
			// 
			this.label20.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label20.ForeColor = System.Drawing.Color.Red;
			this.label20.Location = new System.Drawing.Point(132, 22);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(8, 9);
			this.label20.TabIndex = 12;
			this.label20.Text = "*";
			// 
			// txtTelephone
			// 
			this.txtTelephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTelephone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTelephone.Location = new System.Drawing.Point(150, 116);
			this.txtTelephone.MaxLength = 20;
			this.txtTelephone.Name = "txtTelephone";
			this.txtTelephone.Size = new System.Drawing.Size(150, 21);
			this.txtTelephone.TabIndex = 10;
			this.txtTelephone.Text = "34";
			this.txtTelephone.LostFocus += new System.EventHandler(this.txtTelephone_LostFocus);
			this.txtTelephone.GotFocus += new System.EventHandler(this.txtTelephone_GotFocus);
			// 
			// txtFax
			// 
			this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtFax.Location = new System.Drawing.Point(150, 152);
			this.txtFax.MaxLength = 20;
			this.txtFax.Name = "txtFax";
			this.txtFax.Size = new System.Drawing.Size(150, 21);
			this.txtFax.TabIndex = 11;
			this.txtFax.Text = "34";
			this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
			this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
			// 
			// txtAddress2
			// 
			this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtAddress2.Location = new System.Drawing.Point(150, 66);
			this.txtAddress2.MaxLength = 50;
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(150, 21);
			this.txtAddress2.TabIndex = 8;
			this.txtAddress2.Text = "34";
			this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
			this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
			// 
			// txtAddress1
			// 
			this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtAddress1.Location = new System.Drawing.Point(150, 42);
			this.txtAddress1.MaxLength = 50;
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(150, 21);
			this.txtAddress1.TabIndex = 7;
			this.txtAddress1.Text = "34";
			this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
			this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
			// 
			// txtCompanyName
			// 
			this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCompanyName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtCompanyName.Location = new System.Drawing.Point(150, 18);
			this.txtCompanyName.MaxLength = 50;
			this.txtCompanyName.Name = "txtCompanyName";
			this.txtCompanyName.Size = new System.Drawing.Size(150, 21);
			this.txtCompanyName.TabIndex = 6;
			this.txtCompanyName.Text = "34";
			this.txtCompanyName.LostFocus += new System.EventHandler(this.txtCompanyName_LostFocus);
			this.txtCompanyName.GotFocus += new System.EventHandler(this.txtCompanyName_GotFocus);
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label6.Location = new System.Drawing.Point(30, 155);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Fax #";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label5.Location = new System.Drawing.Point(30, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(52, 14);
			this.label5.TabIndex = 4;
			this.label5.Text = "Work #";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label4.Location = new System.Drawing.Point(30, 94);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(98, 14);
			this.label4.TabIndex = 3;
			this.label4.Text = "Company Type";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.Location = new System.Drawing.Point(30, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 14);
			this.label3.TabIndex = 2;
			this.label3.Text = "Address 2";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.Location = new System.Drawing.Point(30, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Address 1";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(30, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Company Name";
			// 
			// cmbCompanyTYpe
			// 
			this.cmbCompanyTYpe.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance9.BackColor = System.Drawing.SystemColors.Window;
			appearance9.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCompanyTYpe.DisplayLayout.Appearance = appearance9;
			this.cmbCompanyTYpe.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand2.ColHeadersVisible = false;
			ultraGridColumn4.Header.Enabled = false;
			ultraGridColumn4.Header.VisiblePosition = 0;
			ultraGridColumn4.Hidden = true;
			ultraGridColumn5.Header.Enabled = false;
			ultraGridColumn5.Header.VisiblePosition = 1;
			ultraGridBand2.Columns.AddRange(new object[] {
															 ultraGridColumn4,
															 ultraGridColumn5});
			this.cmbCompanyTYpe.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
			this.cmbCompanyTYpe.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCompanyTYpe.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance10.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance10.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCompanyTYpe.DisplayLayout.GroupByBox.Appearance = appearance10;
			appearance11.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCompanyTYpe.DisplayLayout.GroupByBox.BandLabelAppearance = appearance11;
			this.cmbCompanyTYpe.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance12.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance12.BackColor2 = System.Drawing.SystemColors.Control;
			appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance12.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCompanyTYpe.DisplayLayout.GroupByBox.PromptAppearance = appearance12;
			this.cmbCompanyTYpe.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCompanyTYpe.DisplayLayout.MaxRowScrollRegions = 1;
			appearance13.BackColor = System.Drawing.SystemColors.Window;
			appearance13.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCompanyTYpe.DisplayLayout.Override.ActiveCellAppearance = appearance13;
			appearance14.BackColor = System.Drawing.SystemColors.Highlight;
			appearance14.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCompanyTYpe.DisplayLayout.Override.ActiveRowAppearance = appearance14;
			this.cmbCompanyTYpe.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCompanyTYpe.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance15.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCompanyTYpe.DisplayLayout.Override.CardAreaAppearance = appearance15;
			appearance16.BorderColor = System.Drawing.Color.Silver;
			appearance16.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCompanyTYpe.DisplayLayout.Override.CellAppearance = appearance16;
			this.cmbCompanyTYpe.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCompanyTYpe.DisplayLayout.Override.CellPadding = 0;
			appearance17.BackColor = System.Drawing.SystemColors.Control;
			appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance17.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance17.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCompanyTYpe.DisplayLayout.Override.GroupByRowAppearance = appearance17;
			appearance18.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCompanyTYpe.DisplayLayout.Override.HeaderAppearance = appearance18;
			this.cmbCompanyTYpe.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCompanyTYpe.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance19.BackColor = System.Drawing.SystemColors.Window;
			appearance19.BorderColor = System.Drawing.Color.Silver;
			this.cmbCompanyTYpe.DisplayLayout.Override.RowAppearance = appearance19;
			this.cmbCompanyTYpe.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance20.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCompanyTYpe.DisplayLayout.Override.TemplateAddRowAppearance = appearance20;
			this.cmbCompanyTYpe.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCompanyTYpe.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCompanyTYpe.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCompanyTYpe.DisplayMember = "";
			this.cmbCompanyTYpe.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbCompanyTYpe.DropDownWidth = 0;
			this.cmbCompanyTYpe.FlatMode = true;
			this.cmbCompanyTYpe.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbCompanyTYpe.Location = new System.Drawing.Point(150, 90);
			this.cmbCompanyTYpe.Name = "cmbCompanyTYpe";
			this.cmbCompanyTYpe.Size = new System.Drawing.Size(150, 20);
			this.cmbCompanyTYpe.TabIndex = 40;
			this.cmbCompanyTYpe.ValueMember = "";
			this.cmbCompanyTYpe.LostFocus += new System.EventHandler(this.cmbCompanyTYpe_LostFocus);
			this.cmbCompanyTYpe.GotFocus += new System.EventHandler(this.cmbCompanyTYpe_GotFocus);
			// 
			// ultraTabPageControl7
			// 
			this.ultraTabPageControl7.Controls.Add(this.groupBox3);
			this.ultraTabPageControl7.Controls.Add(this.groupBox4);
			this.ultraTabPageControl7.Controls.Add(this.groupBox7);
			this.ultraTabPageControl7.Controls.Add(this.groupBox2);
			this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl7.Name = "ultraTabPageControl7";
			this.ultraTabPageControl7.Size = new System.Drawing.Size(672, 499);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox3.Controls.Add(this.checkedlstThirdPartyComponents);
			this.groupBox3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox3.Location = new System.Drawing.Point(337, 250);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(325, 242);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Third Party Components";
			// 
			// checkedlstThirdPartyComponents
			// 
			this.checkedlstThirdPartyComponents.CheckOnClick = true;
			this.checkedlstThirdPartyComponents.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.checkedlstThirdPartyComponents.Location = new System.Drawing.Point(38, 20);
			this.checkedlstThirdPartyComponents.Name = "checkedlstThirdPartyComponents";
			this.checkedlstThirdPartyComponents.Size = new System.Drawing.Size(248, 180);
			this.checkedlstThirdPartyComponents.TabIndex = 1;
			this.checkedlstThirdPartyComponents.SelectedIndexChanged += new System.EventHandler(this.checkedlstThirdPartyComponents_SelectedIndexChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox4.Controls.Add(this.checkedlstAssetsUnderlyings);
			this.groupBox4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox4.Location = new System.Drawing.Point(337, 6);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(325, 242);
			this.groupBox4.TabIndex = 5;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "AUEC";
			// 
			// checkedlstAssetsUnderlyings
			// 
			this.checkedlstAssetsUnderlyings.CheckOnClick = true;
			this.checkedlstAssetsUnderlyings.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.checkedlstAssetsUnderlyings.Location = new System.Drawing.Point(38, 22);
			this.checkedlstAssetsUnderlyings.Name = "checkedlstAssetsUnderlyings";
			this.checkedlstAssetsUnderlyings.Size = new System.Drawing.Size(248, 180);
			this.checkedlstAssetsUnderlyings.TabIndex = 1;
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox7.Controls.Add(this.checkedlstApplicationComponents);
			this.groupBox7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox7.Location = new System.Drawing.Point(8, 250);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(328, 242);
			this.groupBox7.TabIndex = 4;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Modules";
			// 
			// checkedlstApplicationComponents
			// 
			this.checkedlstApplicationComponents.CheckOnClick = true;
			this.checkedlstApplicationComponents.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.checkedlstApplicationComponents.Location = new System.Drawing.Point(40, 20);
			this.checkedlstApplicationComponents.Name = "checkedlstApplicationComponents";
			this.checkedlstApplicationComponents.Size = new System.Drawing.Size(248, 180);
			this.checkedlstApplicationComponents.TabIndex = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.checkedlstCounterParties);
			this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox2.Location = new System.Drawing.Point(8, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(325, 242);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "CounterPartyVenues";
			// 
			// checkedlstCounterParties
			// 
			this.checkedlstCounterParties.CheckOnClick = true;
			this.checkedlstCounterParties.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.checkedlstCounterParties.Location = new System.Drawing.Point(40, 22);
			this.checkedlstCounterParties.Name = "checkedlstCounterParties";
			this.checkedlstCounterParties.Size = new System.Drawing.Size(248, 180);
			this.checkedlstCounterParties.TabIndex = 1;
			this.checkedlstCounterParties.SelectedIndexChanged += new System.EventHandler(this.checkedlstCounterParties_SelectedIndexChanged);
			// 
			// ultraTabPageControl8
			// 
			this.ultraTabPageControl8.Controls.Add(this.grpCompanyInternalAccounts);
			this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl8.Name = "ultraTabPageControl8";
			this.ultraTabPageControl8.Size = new System.Drawing.Size(672, 499);
			// 
			// grpCompanyInternalAccounts
			// 
			this.grpCompanyInternalAccounts.Controls.Add(this.uctCompanyStrategy);
			this.grpCompanyInternalAccounts.Controls.Add(this.uctClearingFirmsPrimeBrokers);
			this.grpCompanyInternalAccounts.Controls.Add(this.uctCompanyTradingAccounts);
			this.grpCompanyInternalAccounts.Controls.Add(this.uctCompanyFunds);
			this.grpCompanyInternalAccounts.Controls.Add(this.label25);
			this.grpCompanyInternalAccounts.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyInternalAccounts.Name = "grpCompanyInternalAccounts";
			this.grpCompanyInternalAccounts.Size = new System.Drawing.Size(654, 490);
			this.grpCompanyInternalAccounts.TabIndex = 6;
			this.grpCompanyInternalAccounts.TabStop = false;
			// 
			// uctCompanyStrategy
			// 
			this.uctCompanyStrategy.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctCompanyStrategy.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyStrategy.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCompanyStrategy.Location = new System.Drawing.Point(332, 270);
			this.uctCompanyStrategy.Name = "uctCompanyStrategy";
			this.uctCompanyStrategy.Size = new System.Drawing.Size(314, 195);
			this.uctCompanyStrategy.TabIndex = 10;
			// 
			// uctClearingFirmsPrimeBrokers
			// 
			this.uctClearingFirmsPrimeBrokers.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctClearingFirmsPrimeBrokers.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctClearingFirmsPrimeBrokers.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(10, 270);
			this.uctClearingFirmsPrimeBrokers.Name = "uctClearingFirmsPrimeBrokers";
			this.uctClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(314, 204);
			this.uctClearingFirmsPrimeBrokers.TabIndex = 9;
			// 
			// uctCompanyTradingAccounts
			// 
			this.uctCompanyTradingAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctCompanyTradingAccounts.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyTradingAccounts.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCompanyTradingAccounts.Location = new System.Drawing.Point(332, 48);
			this.uctCompanyTradingAccounts.Name = "uctCompanyTradingAccounts";
			this.uctCompanyTradingAccounts.Size = new System.Drawing.Size(314, 204);
			this.uctCompanyTradingAccounts.TabIndex = 8;
			// 
			// uctCompanyFunds
			// 
			this.uctCompanyFunds.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctCompanyFunds.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyFunds.CompanyID = -2147483648;
			this.uctCompanyFunds.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCompanyFunds.Location = new System.Drawing.Point(10, 48);
			this.uctCompanyFunds.Name = "uctCompanyFunds";
			this.uctCompanyFunds.Size = new System.Drawing.Size(314, 204);
			this.uctCompanyFunds.TabIndex = 7;
			// 
			// label25
			// 
			this.label25.ForeColor = System.Drawing.Color.Red;
			this.label25.Location = new System.Drawing.Point(10, 472);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(216, 16);
			this.label25.TabIndex = 6;
			this.label25.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl9
			// 
			this.ultraTabPageControl9.Controls.Add(this.grpCompanyCompliance);
			this.ultraTabPageControl9.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl9.Name = "ultraTabPageControl9";
			this.ultraTabPageControl9.Size = new System.Drawing.Size(672, 499);
			// 
			// grpCompanyCompliance
			// 
			this.grpCompanyCompliance.Controls.Add(this.uctCompanyCompliance);
			this.grpCompanyCompliance.Controls.Add(this.label26);
			this.grpCompanyCompliance.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyCompliance.Name = "grpCompanyCompliance";
			this.grpCompanyCompliance.Size = new System.Drawing.Size(654, 490);
			this.grpCompanyCompliance.TabIndex = 7;
			this.grpCompanyCompliance.TabStop = false;
			// 
			// uctCompanyCompliance
			// 
			this.uctCompanyCompliance.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyCompliance.Location = new System.Drawing.Point(189, 32);
			this.uctCompanyCompliance.Name = "uctCompanyCompliance";
			this.uctCompanyCompliance.Size = new System.Drawing.Size(276, 210);
			this.uctCompanyCompliance.TabIndex = 7;
			// 
			// label26
			// 
			this.label26.ForeColor = System.Drawing.Color.Red;
			this.label26.Location = new System.Drawing.Point(10, 472);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(216, 16);
			this.label26.TabIndex = 6;
			this.label26.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl10
			// 
			this.ultraTabPageControl10.Controls.Add(this.grpCompanyVenue);
			this.ultraTabPageControl10.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl10.Name = "ultraTabPageControl10";
			this.ultraTabPageControl10.Size = new System.Drawing.Size(672, 499);
			// 
			// grpCompanyVenue
			// 
			this.grpCompanyVenue.Controls.Add(this.uctCompanyVenue);
			this.grpCompanyVenue.Controls.Add(this.label27);
			this.grpCompanyVenue.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyVenue.Name = "grpCompanyVenue";
			this.grpCompanyVenue.Size = new System.Drawing.Size(654, 490);
			this.grpCompanyVenue.TabIndex = 7;
			this.grpCompanyVenue.TabStop = false;
			// 
			// uctCompanyVenue
			// 
			this.uctCompanyVenue.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyVenue.Location = new System.Drawing.Point(138, 32);
			this.uctCompanyVenue.Name = "uctCompanyVenue";
			this.uctCompanyVenue.Size = new System.Drawing.Size(378, 412);
			this.uctCompanyVenue.TabIndex = 7;
			// 
			// label27
			// 
			this.label27.ForeColor = System.Drawing.Color.Red;
			this.label27.Location = new System.Drawing.Point(10, 472);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(216, 16);
			this.label27.TabIndex = 6;
			this.label27.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl11
			// 
			this.ultraTabPageControl11.Controls.Add(this.grpCompanyCounterParties);
			this.ultraTabPageControl11.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl11.Name = "ultraTabPageControl11";
			this.ultraTabPageControl11.Size = new System.Drawing.Size(674, 499);
			// 
			// grpCompanyCounterParties
			// 
			this.grpCompanyCounterParties.Controls.Add(this.uctGridCompanyCountePartiesCompanyLevelTags);
			this.grpCompanyCounterParties.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyCounterParties.Name = "grpCompanyCounterParties";
			this.grpCompanyCounterParties.Size = new System.Drawing.Size(654, 490);
			this.grpCompanyCounterParties.TabIndex = 7;
			this.grpCompanyCounterParties.TabStop = false;
			// 
			// uctGridCompanyCountePartiesCompanyLevelTags
			// 
			this.uctGridCompanyCountePartiesCompanyLevelTags.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctGridCompanyCountePartiesCompanyLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctGridCompanyCountePartiesCompanyLevelTags.Location = new System.Drawing.Point(37, 32);
			this.uctGridCompanyCountePartiesCompanyLevelTags.Name = "uctGridCompanyCountePartiesCompanyLevelTags";
			this.uctGridCompanyCountePartiesCompanyLevelTags.Size = new System.Drawing.Size(580, 468);
			this.uctGridCompanyCountePartiesCompanyLevelTags.TabIndex = 0;
			// 
			// ultraTabPageControl12
			// 
			this.ultraTabPageControl12.Controls.Add(this.grpCompanyUser);
			this.ultraTabPageControl12.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl12.Name = "ultraTabPageControl12";
			this.ultraTabPageControl12.Size = new System.Drawing.Size(672, 497);
			// 
			// grpCompanyUser
			// 
			this.grpCompanyUser.Controls.Add(this.uctCompanyUserDetail);
			this.grpCompanyUser.Controls.Add(this.label41);
			this.grpCompanyUser.Location = new System.Drawing.Point(9, 6);
			this.grpCompanyUser.Name = "grpCompanyUser";
			this.grpCompanyUser.Size = new System.Drawing.Size(654, 490);
			this.grpCompanyUser.TabIndex = 7;
			this.grpCompanyUser.TabStop = false;
			// 
			// uctCompanyUserDetail
			// 
			this.uctCompanyUserDetail.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyUserDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCompanyUserDetail.Location = new System.Drawing.Point(36, 32);
			this.uctCompanyUserDetail.Name = "uctCompanyUserDetail";
			this.uctCompanyUserDetail.Size = new System.Drawing.Size(590, 298);
			this.uctCompanyUserDetail.TabIndex = 7;
			// 
			// label41
			// 
			this.label41.ForeColor = System.Drawing.Color.Red;
			this.label41.Location = new System.Drawing.Point(10, 472);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(216, 16);
			this.label41.TabIndex = 6;
			this.label41.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl13
			// 
			this.ultraTabPageControl13.Controls.Add(this.grpCompanyUserPermission);
			this.ultraTabPageControl13.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl13.Name = "ultraTabPageControl13";
			this.ultraTabPageControl13.Size = new System.Drawing.Size(672, 497);
			// 
			// grpCompanyUserPermission
			// 
			this.grpCompanyUserPermission.Controls.Add(this.uctCompanyUserPermissions);
			this.grpCompanyUserPermission.Controls.Add(this.label42);
			this.grpCompanyUserPermission.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyUserPermission.Name = "grpCompanyUserPermission";
			this.grpCompanyUserPermission.Size = new System.Drawing.Size(654, 488);
			this.grpCompanyUserPermission.TabIndex = 7;
			this.grpCompanyUserPermission.TabStop = false;
			// 
			// uctCompanyUserPermissions
			// 
			this.uctCompanyUserPermissions.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctCompanyUserPermissions.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyUserPermissions.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCompanyUserPermissions.Location = new System.Drawing.Point(66, 48);
			this.uctCompanyUserPermissions.Name = "uctCompanyUserPermissions";
			this.uctCompanyUserPermissions.Size = new System.Drawing.Size(522, 410);
			this.uctCompanyUserPermissions.TabIndex = 7;
			// 
			// label42
			// 
			this.label42.ForeColor = System.Drawing.Color.Red;
			this.label42.Location = new System.Drawing.Point(10, 468);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(216, 16);
			this.label42.TabIndex = 6;
			this.label42.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl14
			// 
			this.ultraTabPageControl14.Controls.Add(this.grpCompanyClientDetails);
			this.ultraTabPageControl14.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl14.Name = "ultraTabPageControl14";
			this.ultraTabPageControl14.Size = new System.Drawing.Size(672, 490);
			// 
			// grpCompanyClientDetails
			// 
			this.grpCompanyClientDetails.Controls.Add(this.uctClientCompanyControl);
			this.grpCompanyClientDetails.Controls.Add(this.label43);
			this.grpCompanyClientDetails.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyClientDetails.Name = "grpCompanyClientDetails";
			this.grpCompanyClientDetails.Size = new System.Drawing.Size(654, 480);
			this.grpCompanyClientDetails.TabIndex = 7;
			this.grpCompanyClientDetails.TabStop = false;
			// 
			// uctClientCompanyControl
			// 
			this.uctClientCompanyControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctClientCompanyControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctClientCompanyControl.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctClientCompanyControl.Location = new System.Drawing.Point(34, 48);
			this.uctClientCompanyControl.Name = "uctClientCompanyControl";
			this.uctClientCompanyControl.Size = new System.Drawing.Size(586, 348);
			this.uctClientCompanyControl.TabIndex = 7;
			// 
			// label43
			// 
			this.label43.ForeColor = System.Drawing.Color.Red;
			this.label43.Location = new System.Drawing.Point(10, 460);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(216, 16);
			this.label43.TabIndex = 6;
			this.label43.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl15
			// 
			this.ultraTabPageControl15.Controls.Add(this.grpCompanyClientFunds);
			this.ultraTabPageControl15.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl15.Name = "ultraTabPageControl15";
			this.ultraTabPageControl15.Size = new System.Drawing.Size(672, 490);
			// 
			// grpCompanyClientFunds
			// 
			this.grpCompanyClientFunds.Controls.Add(this.uctGridClientFunds1);
			this.grpCompanyClientFunds.Controls.Add(this.label44);
			this.grpCompanyClientFunds.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyClientFunds.Name = "grpCompanyClientFunds";
			this.grpCompanyClientFunds.Size = new System.Drawing.Size(654, 480);
			this.grpCompanyClientFunds.TabIndex = 7;
			this.grpCompanyClientFunds.TabStop = false;
			// 
			// uctGridClientFunds1
			// 
			this.uctGridClientFunds1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctGridClientFunds1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctGridClientFunds1.CurrentCompanyClientFundID = -2147483648;
			this.uctGridClientFunds1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctGridClientFunds1.Location = new System.Drawing.Point(110, 48);
			this.uctGridClientFunds1.Name = "uctGridClientFunds1";
			this.uctGridClientFunds1.Size = new System.Drawing.Size(434, 202);
			this.uctGridClientFunds1.TabIndex = 7;
			// 
			// label44
			// 
			this.label44.ForeColor = System.Drawing.Color.Red;
			this.label44.Location = new System.Drawing.Point(10, 462);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(216, 16);
			this.label44.TabIndex = 6;
			this.label44.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl17
			// 
			this.ultraTabPageControl17.Controls.Add(this.groupBox1);
			this.ultraTabPageControl17.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl17.Name = "ultraTabPageControl17";
			this.ultraTabPageControl17.Size = new System.Drawing.Size(672, 490);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.uctClientTradersDetail);
			this.groupBox1.Controls.Add(this.label45);
			this.groupBox1.Location = new System.Drawing.Point(8, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(654, 480);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// uctClientTradersDetail
			// 
			this.uctClientTradersDetail.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctClientTradersDetail.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctClientTradersDetail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctClientTradersDetail.Location = new System.Drawing.Point(25, 48);
			this.uctClientTradersDetail.Name = "uctClientTradersDetail";
			this.uctClientTradersDetail.Size = new System.Drawing.Size(604, 228);
			this.uctClientTradersDetail.TabIndex = 7;
			// 
			// label45
			// 
			this.label45.ForeColor = System.Drawing.Color.Red;
			this.label45.Location = new System.Drawing.Point(10, 462);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(216, 16);
			this.label45.TabIndex = 6;
			this.label45.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl16
			// 
			this.ultraTabPageControl16.Controls.Add(this.grpCompanyClientTradingAccounts);
			this.ultraTabPageControl16.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl16.Name = "ultraTabPageControl16";
			this.ultraTabPageControl16.Size = new System.Drawing.Size(672, 490);
			// 
			// grpCompanyClientTradingAccounts
			// 
			this.grpCompanyClientTradingAccounts.Controls.Add(this.uctClientTradingAccounts);
			this.grpCompanyClientTradingAccounts.Controls.Add(this.label46);
			this.grpCompanyClientTradingAccounts.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyClientTradingAccounts.Name = "grpCompanyClientTradingAccounts";
			this.grpCompanyClientTradingAccounts.Size = new System.Drawing.Size(654, 480);
			this.grpCompanyClientTradingAccounts.TabIndex = 7;
			this.grpCompanyClientTradingAccounts.TabStop = false;
			// 
			// uctClientTradingAccounts
			// 
			this.uctClientTradingAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctClientTradingAccounts.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctClientTradingAccounts.Location = new System.Drawing.Point(135, 48);
			this.uctClientTradingAccounts.Name = "uctClientTradingAccounts";
			this.uctClientTradingAccounts.Size = new System.Drawing.Size(384, 168);
			this.uctClientTradingAccounts.TabIndex = 7;
			// 
			// label46
			// 
			this.label46.ForeColor = System.Drawing.Color.Red;
			this.label46.Location = new System.Drawing.Point(10, 460);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(216, 16);
			this.label46.TabIndex = 6;
			this.label46.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl18
			// 
			this.ultraTabPageControl18.Controls.Add(this.grpCompanyClientClearer);
			this.ultraTabPageControl18.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl18.Name = "ultraTabPageControl18";
			this.ultraTabPageControl18.Size = new System.Drawing.Size(672, 490);
			// 
			// grpCompanyClientClearer
			// 
			this.grpCompanyClientClearer.Controls.Add(this.uctClientClearer);
			this.grpCompanyClientClearer.Controls.Add(this.label47);
			this.grpCompanyClientClearer.Location = new System.Drawing.Point(8, 6);
			this.grpCompanyClientClearer.Name = "grpCompanyClientClearer";
			this.grpCompanyClientClearer.Size = new System.Drawing.Size(654, 480);
			this.grpCompanyClientClearer.TabIndex = 7;
			this.grpCompanyClientClearer.TabStop = false;
			// 
			// uctClientClearer
			// 
			this.uctClientClearer.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctClientClearer.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctClientClearer.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctClientClearer.Location = new System.Drawing.Point(190, 48);
			this.uctClientClearer.Name = "uctClientClearer";
			this.uctClientClearer.Size = new System.Drawing.Size(274, 102);
			this.uctClientClearer.TabIndex = 7;
			// 
			// label47
			// 
			this.label47.ForeColor = System.Drawing.Color.Red;
			this.label47.Location = new System.Drawing.Point(10, 462);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(216, 16);
			this.label47.TabIndex = 6;
			this.label47.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl19
			// 
			this.ultraTabPageControl19.Controls.Add(this.groupBox8);
			this.ultraTabPageControl19.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl19.Name = "ultraTabPageControl19";
			this.ultraTabPageControl19.Size = new System.Drawing.Size(672, 490);
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.uctGridClientFix);
			this.groupBox8.Controls.Add(this.label48);
			this.groupBox8.Location = new System.Drawing.Point(8, 6);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(654, 480);
			this.groupBox8.TabIndex = 7;
			this.groupBox8.TabStop = false;
			// 
			// uctGridClientFix
			// 
			this.uctGridClientFix.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctGridClientFix.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctGridClientFix.clientFix = null;
			this.uctGridClientFix.Location = new System.Drawing.Point(84, 48);
			this.uctGridClientFix.Name = "uctGridClientFix";
			this.uctGridClientFix.Size = new System.Drawing.Size(486, 320);
			this.uctGridClientFix.TabIndex = 7;
			// 
			// label48
			// 
			this.label48.ForeColor = System.Drawing.Color.Red;
			this.label48.Location = new System.Drawing.Point(10, 462);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(216, 16);
			this.label48.TabIndex = 6;
			this.label48.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl20
			// 
			this.ultraTabPageControl20.Controls.Add(this.groupBox9);
			this.ultraTabPageControl20.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl20.Name = "ultraTabPageControl20";
			this.ultraTabPageControl20.Size = new System.Drawing.Size(672, 490);
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.uctClientPermission);
			this.groupBox9.Controls.Add(this.label49);
			this.groupBox9.Location = new System.Drawing.Point(8, 6);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(654, 480);
			this.groupBox9.TabIndex = 8;
			this.groupBox9.TabStop = false;
			// 
			// uctClientPermission
			// 
			this.uctClientPermission.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uctClientPermission.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctClientPermission.CompanyVenue = null;
			this.uctClientPermission.Location = new System.Drawing.Point(19, 48);
			this.uctClientPermission.Name = "uctClientPermission";
			this.uctClientPermission.Size = new System.Drawing.Size(616, 276);
			this.uctClientPermission.TabIndex = 7;
			// 
			// label49
			// 
			this.label49.ForeColor = System.Drawing.Color.Red;
			this.label49.Location = new System.Drawing.Point(10, 462);
			this.label49.Name = "label49";
			this.label49.Size = new System.Drawing.Size(216, 16);
			this.label49.TabIndex = 6;
			this.label49.Text = "*Click Save below to save the data above.";
			// 
			// ultraTabPageControl21
			// 
			this.ultraTabPageControl21.Controls.Add(this.groupBox10);
			this.ultraTabPageControl21.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl21.Name = "ultraTabPageControl21";
			this.ultraTabPageControl21.Size = new System.Drawing.Size(675, 498);
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.uctCompanyThirdPartyFileFormats);
			this.groupBox10.Location = new System.Drawing.Point(8, 6);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(654, 480);
			this.groupBox10.TabIndex = 1;
			this.groupBox10.TabStop = false;
			// 
			// uctCompanyThirdPartyFileFormats
			// 
			this.uctCompanyThirdPartyFileFormats.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyThirdPartyFileFormats.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctCompanyThirdPartyFileFormats.Location = new System.Drawing.Point(88, 32);
			this.uctCompanyThirdPartyFileFormats.Name = "uctCompanyThirdPartyFileFormats";
			this.uctCompanyThirdPartyFileFormats.Size = new System.Drawing.Size(472, 222);
			this.uctCompanyThirdPartyFileFormats.TabIndex = 0;
			// 
			// ultraTabPageControl22
			// 
			this.ultraTabPageControl22.Controls.Add(this.grpThirdPartyFileFormat);
			this.ultraTabPageControl22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraTabPageControl22.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl22.Name = "ultraTabPageControl22";
			this.ultraTabPageControl22.Size = new System.Drawing.Size(674, 497);
			// 
			// grpThirdPartyFileFormat
			// 
			this.grpThirdPartyFileFormat.Controls.Add(this.uctCompanyThirdPartyMappingDetails);
			this.grpThirdPartyFileFormat.Location = new System.Drawing.Point(8, 6);
			this.grpThirdPartyFileFormat.Name = "grpThirdPartyFileFormat";
			this.grpThirdPartyFileFormat.Size = new System.Drawing.Size(654, 480);
			this.grpThirdPartyFileFormat.TabIndex = 0;
			this.grpThirdPartyFileFormat.TabStop = false;
			// 
			// uctCompanyThirdPartyMappingDetails
			// 
			this.uctCompanyThirdPartyMappingDetails.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyThirdPartyMappingDetails.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctCompanyThirdPartyMappingDetails.Location = new System.Drawing.Point(4, 32);
			this.uctCompanyThirdPartyMappingDetails.Name = "uctCompanyThirdPartyMappingDetails";
			this.uctCompanyThirdPartyMappingDetails.Size = new System.Drawing.Size(654, 252);
			this.uctCompanyThirdPartyMappingDetails.TabIndex = 0;
			// 
			// ultraTabPageControl23
			// 
			this.ultraTabPageControl23.Controls.Add(this.groupBox11);
			this.ultraTabPageControl23.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl23.Name = "ultraTabPageControl23";
			this.ultraTabPageControl23.Size = new System.Drawing.Size(674, 497);
			// 
			// groupBox11
			// 
			this.groupBox11.Controls.Add(this.uctCompanyThirdPartyCVIdentifier);
			this.groupBox11.Location = new System.Drawing.Point(8, 6);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(654, 480);
			this.groupBox11.TabIndex = 0;
			this.groupBox11.TabStop = false;
			// 
			// uctCompanyThirdPartyCVIdentifier
			// 
			this.uctCompanyThirdPartyCVIdentifier.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyThirdPartyCVIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctCompanyThirdPartyCVIdentifier.Location = new System.Drawing.Point(76, 32);
			this.uctCompanyThirdPartyCVIdentifier.Name = "uctCompanyThirdPartyCVIdentifier";
			this.uctCompanyThirdPartyCVIdentifier.Size = new System.Drawing.Size(502, 260);
			this.uctCompanyThirdPartyCVIdentifier.TabIndex = 0;
			// 
			// ultraTabPageControl24
			// 
			this.ultraTabPageControl24.Controls.Add(this.groupBox12);
			this.ultraTabPageControl24.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl24.Name = "ultraTabPageControl24";
			this.ultraTabPageControl24.Size = new System.Drawing.Size(674, 497);
			// 
			// groupBox12
			// 
			this.groupBox12.Controls.Add(this.uctCompanyThirdPartyCommissionRules);
			this.groupBox12.Location = new System.Drawing.Point(8, 6);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(654, 480);
			this.groupBox12.TabIndex = 0;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "groupBox12";
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Controls.Add(this.tbcCompanySetup);
			this.ultraTabPageControl1.Controls.Add(this.btnSaveCompanySetup);
			this.ultraTabPageControl1.Controls.Add(this.btnClose);
			this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			this.ultraTabPageControl1.Size = new System.Drawing.Size(674, 545);
			// 
			// tbcCompanySetup
			// 
			appearance21.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance21.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcCompanySetup.ActiveTabAppearance = appearance21;
			this.tbcCompanySetup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbcCompanySetup.Controls.Add(this.ultraTabSharedControlsPage2);
			this.tbcCompanySetup.Controls.Add(this.ultraTabPageControl6);
			this.tbcCompanySetup.Controls.Add(this.ultraTabPageControl7);
			this.tbcCompanySetup.Controls.Add(this.ultraTabPageControl8);
			this.tbcCompanySetup.Controls.Add(this.ultraTabPageControl9);
			this.tbcCompanySetup.Controls.Add(this.ultraTabPageControl10);
			this.tbcCompanySetup.Location = new System.Drawing.Point(0, 0);
			this.tbcCompanySetup.Name = "tbcCompanySetup";
			this.tbcCompanySetup.SharedControlsPage = this.ultraTabSharedControlsPage2;
			this.tbcCompanySetup.Size = new System.Drawing.Size(674, 520);
			this.tbcCompanySetup.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcCompanySetup.TabIndex = 0;
			appearance22.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance22.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab1.Appearance = appearance22;
			ultraTab1.Key = "tbpCompanyDetails";
			ultraTab1.TabPage = this.ultraTabPageControl6;
			ultraTab1.Text = "Details";
			appearance23.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance23.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab2.Appearance = appearance23;
			ultraTab2.Key = "tbpPermissionLevel";
			ultraTab2.TabPage = this.ultraTabPageControl7;
			ultraTab2.Text = "Permission Level";
			appearance24.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance24.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab3.Appearance = appearance24;
			ultraTab3.Key = "tbpInternalAccounts";
			ultraTab3.TabPage = this.ultraTabPageControl8;
			ultraTab3.Text = "Internal Accounts";
			appearance25.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance25.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab4.Appearance = appearance25;
			ultraTab4.Key = "tbpCompanyCompliance";
			ultraTab4.TabPage = this.ultraTabPageControl9;
			ultraTab4.Text = "Compliance";
			appearance26.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance26.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab5.Appearance = appearance26;
			ultraTab5.Key = "tbpCompanyVenue";
			ultraTab5.TabPage = this.ultraTabPageControl10;
			ultraTab5.Text = "Venue";
			this.tbcCompanySetup.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									  ultraTab1,
																									  ultraTab2,
																									  ultraTab3,
																									  ultraTab4,
																									  ultraTab5});
			this.tbcCompanySetup.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			this.tbcCompanySetup.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcCompanySetup_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage2
			// 
			this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
			this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(672, 499);
			// 
			// btnSaveCompanySetup
			// 
			this.btnSaveCompanySetup.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSaveCompanySetup.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnSaveCompanySetup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveCompanySetup.BackgroundImage")));
			this.btnSaveCompanySetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSaveCompanySetup.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnSaveCompanySetup.Location = new System.Drawing.Point(261, 522);
			this.btnSaveCompanySetup.Name = "btnSaveCompanySetup";
			this.btnSaveCompanySetup.TabIndex = 33;
			this.btnSaveCompanySetup.Click += new System.EventHandler(this.btnSaveCompanySetup_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnClose.Location = new System.Drawing.Point(339, 522);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 32;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// ultraTabPageControl2
			// 
			this.ultraTabPageControl2.Controls.Add(this.tbcCompanyCounterParties);
			this.ultraTabPageControl2.Controls.Add(this.btnCompanyCouterPartiesSave);
			this.ultraTabPageControl2.Controls.Add(this.btnCompanyCouterPartiesClose);
			this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl2.Name = "ultraTabPageControl2";
			this.ultraTabPageControl2.Size = new System.Drawing.Size(674, 545);
			// 
			// tbcCompanyCounterParties
			// 
			appearance27.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance27.BackColor2 = System.Drawing.Color.White;
			appearance27.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcCompanyCounterParties.ActiveTabAppearance = appearance27;
			this.tbcCompanyCounterParties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbcCompanyCounterParties.Controls.Add(this.ultraTabSharedControlsPage3);
			this.tbcCompanyCounterParties.Controls.Add(this.ultraTabPageControl11);
			this.tbcCompanyCounterParties.Location = new System.Drawing.Point(0, 0);
			this.tbcCompanyCounterParties.Name = "tbcCompanyCounterParties";
			this.tbcCompanyCounterParties.SharedControlsPage = this.ultraTabSharedControlsPage3;
			this.tbcCompanyCounterParties.Size = new System.Drawing.Size(676, 520);
			this.tbcCompanyCounterParties.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcCompanyCounterParties.TabIndex = 0;
			appearance28.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance28.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab6.Appearance = appearance28;
			ultraTab6.Key = "tabCompanyCounterPartiesCompanyLevelTags";
			ultraTab6.TabPage = this.ultraTabPageControl11;
			ultraTab6.Text = "Details";
			this.tbcCompanyCounterParties.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																											   ultraTab6});
			this.tbcCompanyCounterParties.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			// 
			// ultraTabSharedControlsPage3
			// 
			this.ultraTabSharedControlsPage3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage3.Name = "ultraTabSharedControlsPage3";
			this.ultraTabSharedControlsPage3.Size = new System.Drawing.Size(674, 499);
			// 
			// btnCompanyCouterPartiesSave
			// 
			this.btnCompanyCouterPartiesSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCompanyCouterPartiesSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnCompanyCouterPartiesSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCompanyCouterPartiesSave.BackgroundImage")));
			this.btnCompanyCouterPartiesSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCompanyCouterPartiesSave.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnCompanyCouterPartiesSave.Location = new System.Drawing.Point(261, 520);
			this.btnCompanyCouterPartiesSave.Name = "btnCompanyCouterPartiesSave";
			this.btnCompanyCouterPartiesSave.TabIndex = 37;
			this.btnCompanyCouterPartiesSave.Click += new System.EventHandler(this.btnCompanyCouterPartiesSave_Click);
			// 
			// btnCompanyCouterPartiesClose
			// 
			this.btnCompanyCouterPartiesClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCompanyCouterPartiesClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnCompanyCouterPartiesClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCompanyCouterPartiesClose.BackgroundImage")));
			this.btnCompanyCouterPartiesClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCompanyCouterPartiesClose.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnCompanyCouterPartiesClose.Location = new System.Drawing.Point(339, 520);
			this.btnCompanyCouterPartiesClose.Name = "btnCompanyCouterPartiesClose";
			this.btnCompanyCouterPartiesClose.TabIndex = 36;
			this.btnCompanyCouterPartiesClose.Click += new System.EventHandler(this.btnCompanyCouterPartiesClose_Click);
			// 
			// ultraTabPageControl3
			// 
			this.ultraTabPageControl3.Controls.Add(this.btnCloseCompanyUser);
			this.ultraTabPageControl3.Controls.Add(this.btnSaveCompanyUser);
			this.ultraTabPageControl3.Controls.Add(this.tbcCompanyUser);
			this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl3.Name = "ultraTabPageControl3";
			this.ultraTabPageControl3.Size = new System.Drawing.Size(674, 545);
			// 
			// btnCloseCompanyUser
			// 
			this.btnCloseCompanyUser.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCloseCompanyUser.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnCloseCompanyUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCloseCompanyUser.BackgroundImage")));
			this.btnCloseCompanyUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCloseCompanyUser.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnCloseCompanyUser.Location = new System.Drawing.Point(340, 518);
			this.btnCloseCompanyUser.Name = "btnCloseCompanyUser";
			this.btnCloseCompanyUser.TabIndex = 39;
			this.btnCloseCompanyUser.Click += new System.EventHandler(this.btnCloseCompanyUser_Click);
			// 
			// btnSaveCompanyUser
			// 
			this.btnSaveCompanyUser.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSaveCompanyUser.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnSaveCompanyUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveCompanyUser.BackgroundImage")));
			this.btnSaveCompanyUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSaveCompanyUser.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnSaveCompanyUser.Location = new System.Drawing.Point(260, 518);
			this.btnSaveCompanyUser.Name = "btnSaveCompanyUser";
			this.btnSaveCompanyUser.TabIndex = 38;
			this.btnSaveCompanyUser.Click += new System.EventHandler(this.btnSaveCompanyUser_Click);
			// 
			// tbcCompanyUser
			// 
			appearance29.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance29.BackColor2 = System.Drawing.Color.White;
			appearance29.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcCompanyUser.ActiveTabAppearance = appearance29;
			this.tbcCompanyUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbcCompanyUser.Controls.Add(this.ultraTabSharedControlsPage4);
			this.tbcCompanyUser.Controls.Add(this.ultraTabPageControl12);
			this.tbcCompanyUser.Controls.Add(this.ultraTabPageControl13);
			this.tbcCompanyUser.Location = new System.Drawing.Point(0, 0);
			this.tbcCompanyUser.Name = "tbcCompanyUser";
			this.tbcCompanyUser.SharedControlsPage = this.ultraTabSharedControlsPage4;
			this.tbcCompanyUser.Size = new System.Drawing.Size(674, 518);
			this.tbcCompanyUser.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcCompanyUser.TabIndex = 0;
			appearance30.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance30.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab7.Appearance = appearance30;
			ultraTab7.Key = "tbpUserDetails";
			ultraTab7.TabPage = this.ultraTabPageControl12;
			ultraTab7.Text = "Details";
			appearance31.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance31.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab8.Appearance = appearance31;
			ultraTab8.Key = "tbpPermission";
			ultraTab8.TabPage = this.ultraTabPageControl13;
			ultraTab8.Text = "Permission";
			this.tbcCompanyUser.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									 ultraTab7,
																									 ultraTab8});
			this.tbcCompanyUser.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			this.tbcCompanyUser.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcCompanyUser_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage4
			// 
			this.ultraTabSharedControlsPage4.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage4.Name = "ultraTabSharedControlsPage4";
			this.ultraTabSharedControlsPage4.Size = new System.Drawing.Size(672, 497);
			// 
			// ultraTabPageControl4
			// 
			this.ultraTabPageControl4.Controls.Add(this.btnCloseCompanyClients);
			this.ultraTabPageControl4.Controls.Add(this.btnCompanyClientsSave);
			this.ultraTabPageControl4.Controls.Add(this.tbcCompanyClients);
			this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl4.Name = "ultraTabPageControl4";
			this.ultraTabPageControl4.Size = new System.Drawing.Size(674, 545);
			// 
			// btnCloseCompanyClients
			// 
			this.btnCloseCompanyClients.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCloseCompanyClients.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnCloseCompanyClients.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCloseCompanyClients.BackgroundImage")));
			this.btnCloseCompanyClients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCloseCompanyClients.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnCloseCompanyClients.Location = new System.Drawing.Point(340, 518);
			this.btnCloseCompanyClients.Name = "btnCloseCompanyClients";
			this.btnCloseCompanyClients.TabIndex = 41;
			this.btnCloseCompanyClients.Click += new System.EventHandler(this.btnCloseCompanyClients_Click);
			// 
			// btnCompanyClientsSave
			// 
			this.btnCompanyClientsSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCompanyClientsSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnCompanyClientsSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCompanyClientsSave.BackgroundImage")));
			this.btnCompanyClientsSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCompanyClientsSave.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnCompanyClientsSave.Location = new System.Drawing.Point(260, 518);
			this.btnCompanyClientsSave.Name = "btnCompanyClientsSave";
			this.btnCompanyClientsSave.TabIndex = 40;
			this.btnCompanyClientsSave.Click += new System.EventHandler(this.btnCompanyClientsSave_Click);
			// 
			// tbcCompanyClients
			// 
			appearance32.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance32.BackColor2 = System.Drawing.Color.White;
			appearance32.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcCompanyClients.ActiveTabAppearance = appearance32;
			this.tbcCompanyClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbcCompanyClients.Controls.Add(this.ultraTabPageControl17);
			this.tbcCompanyClients.Controls.Add(this.ultraTabSharedControlsPage5);
			this.tbcCompanyClients.Controls.Add(this.ultraTabPageControl14);
			this.tbcCompanyClients.Controls.Add(this.ultraTabPageControl15);
			this.tbcCompanyClients.Controls.Add(this.ultraTabPageControl16);
			this.tbcCompanyClients.Controls.Add(this.ultraTabPageControl18);
			this.tbcCompanyClients.Controls.Add(this.ultraTabPageControl19);
			this.tbcCompanyClients.Controls.Add(this.ultraTabPageControl20);
			this.tbcCompanyClients.Location = new System.Drawing.Point(0, 0);
			this.tbcCompanyClients.Name = "tbcCompanyClients";
			this.tbcCompanyClients.SharedControlsPage = this.ultraTabSharedControlsPage5;
			this.tbcCompanyClients.Size = new System.Drawing.Size(674, 511);
			this.tbcCompanyClients.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcCompanyClients.TabIndex = 0;
			appearance33.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance33.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab9.Appearance = appearance33;
			ultraTab9.Key = "tbpClientCompany";
			ultraTab9.TabPage = this.ultraTabPageControl14;
			ultraTab9.Text = "Details";
			appearance34.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance34.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab10.Appearance = appearance34;
			ultraTab10.Key = "tbpClientFunds";
			ultraTab10.TabPage = this.ultraTabPageControl15;
			ultraTab10.Text = "Funds";
			appearance35.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance35.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab11.Appearance = appearance35;
			ultraTab11.Key = "tbpClientTraders";
			ultraTab11.TabPage = this.ultraTabPageControl17;
			ultraTab11.Text = "Trader";
			appearance36.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance36.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab12.Appearance = appearance36;
			ultraTab12.Key = "tbpClientTradingAccounts";
			ultraTab12.TabPage = this.ultraTabPageControl16;
			ultraTab12.Text = "TradingAccounts";
			appearance37.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance37.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab13.Appearance = appearance37;
			ultraTab13.Key = "tbpClientClearer";
			ultraTab13.TabPage = this.ultraTabPageControl18;
			ultraTab13.Text = "Clearer";
			appearance38.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance38.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab14.Appearance = appearance38;
			ultraTab14.Key = "tbpFix";
			ultraTab14.TabPage = this.ultraTabPageControl19;
			ultraTab14.Text = "Fix";
			appearance39.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance39.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab15.Appearance = appearance39;
			ultraTab15.Key = "tbpClientPermissions";
			ultraTab15.TabPage = this.ultraTabPageControl20;
			ultraTab15.Text = "Permissions";
			this.tbcCompanyClients.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																										ultraTab9,
																										ultraTab10,
																										ultraTab11,
																										ultraTab12,
																										ultraTab13,
																										ultraTab14,
																										ultraTab15});
			this.tbcCompanyClients.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			this.tbcCompanyClients.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcCompanyClients_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage5
			// 
			this.ultraTabSharedControlsPage5.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage5.Name = "ultraTabSharedControlsPage5";
			this.ultraTabSharedControlsPage5.Size = new System.Drawing.Size(672, 490);
			// 
			// ultraTabPageControl5
			// 
			this.ultraTabPageControl5.Controls.Add(this.btnCloseCompanyThirdParty);
			this.ultraTabPageControl5.Controls.Add(this.btnSaveCompanyThirdParty);
			this.ultraTabPageControl5.Controls.Add(this.tbcCompanyThirdParty);
			this.ultraTabPageControl5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraTabPageControl5.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl5.Name = "ultraTabPageControl5";
			this.ultraTabPageControl5.Size = new System.Drawing.Size(674, 545);
			this.ultraTabPageControl5.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl5_Paint);
			// 
			// btnCloseCompanyThirdParty
			// 
			this.btnCloseCompanyThirdParty.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCloseCompanyThirdParty.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnCloseCompanyThirdParty.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCloseCompanyThirdParty.BackgroundImage")));
			this.btnCloseCompanyThirdParty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCloseCompanyThirdParty.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnCloseCompanyThirdParty.Location = new System.Drawing.Point(340, 518);
			this.btnCloseCompanyThirdParty.Name = "btnCloseCompanyThirdParty";
			this.btnCloseCompanyThirdParty.TabIndex = 43;
			this.btnCloseCompanyThirdParty.Click += new System.EventHandler(this.btnCloseCompanyThirdParty_Click);
			// 
			// btnSaveCompanyThirdParty
			// 
			this.btnSaveCompanyThirdParty.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSaveCompanyThirdParty.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnSaveCompanyThirdParty.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveCompanyThirdParty.BackgroundImage")));
			this.btnSaveCompanyThirdParty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSaveCompanyThirdParty.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnSaveCompanyThirdParty.Location = new System.Drawing.Point(260, 518);
			this.btnSaveCompanyThirdParty.Name = "btnSaveCompanyThirdParty";
			this.btnSaveCompanyThirdParty.TabIndex = 42;
			this.btnSaveCompanyThirdParty.Click += new System.EventHandler(this.btnSaveCompanyThirdParty_Click);
			// 
			// tbcCompanyThirdParty
			// 
			appearance40.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance40.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance40.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcCompanyThirdParty.ActiveTabAppearance = appearance40;
			this.tbcCompanyThirdParty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbcCompanyThirdParty.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.tbcCompanyThirdParty.Controls.Add(this.ultraTabSharedControlsPage6);
			this.tbcCompanyThirdParty.Controls.Add(this.ultraTabPageControl21);
			this.tbcCompanyThirdParty.Controls.Add(this.ultraTabPageControl22);
			this.tbcCompanyThirdParty.Controls.Add(this.ultraTabPageControl23);
			this.tbcCompanyThirdParty.Controls.Add(this.ultraTabPageControl24);
			this.tbcCompanyThirdParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.tbcCompanyThirdParty.Location = new System.Drawing.Point(0, 0);
			this.tbcCompanyThirdParty.Name = "tbcCompanyThirdParty";
			this.tbcCompanyThirdParty.SharedControlsPage = this.ultraTabSharedControlsPage6;
			this.tbcCompanyThirdParty.Size = new System.Drawing.Size(676, 518);
			this.tbcCompanyThirdParty.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcCompanyThirdParty.TabIndex = 0;
			appearance41.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance41.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab16.Appearance = appearance41;
			ultraTab16.Key = "tbpCompanyThirdPartyFileFormat";
			ultraTab16.TabPage = this.ultraTabPageControl21;
			ultraTab16.Text = "File Formats";
			appearance42.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance42.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab17.Appearance = appearance42;
			ultraTab17.Key = "tbpCompanyThirdPartyMappingDetails";
			ultraTab17.TabPage = this.ultraTabPageControl22;
			ultraTab17.Text = "Mapping Details";
			appearance43.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance43.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab18.Appearance = appearance43;
			ultraTab18.Key = "tbpCompanyThirdPartyCVIdentifier";
			ultraTab18.TabPage = this.ultraTabPageControl23;
			ultraTab18.Text = "Company CVIdentifier";
			appearance44.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance44.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab19.Appearance = appearance44;
			ultraTab19.Key = "tbpCompanyThirdPartyCommissionRules";
			ultraTab19.TabPage = this.ultraTabPageControl24;
			ultraTab19.Text = "Add Commission Rules";
			this.tbcCompanyThirdParty.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																										   ultraTab16,
																										   ultraTab17,
																										   ultraTab18,
																										   ultraTab19});
			this.tbcCompanyThirdParty.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			// 
			// ultraTabSharedControlsPage6
			// 
			this.ultraTabSharedControlsPage6.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage6.Name = "ultraTabSharedControlsPage6";
			this.ultraTabSharedControlsPage6.Size = new System.Drawing.Size(674, 497);
			// 
			// grpThirdPartyCommissionRules
			// 
			this.grpThirdPartyCommissionRules.Location = new System.Drawing.Point(0, 0);
			this.grpThirdPartyCommissionRules.Name = "grpThirdPartyCommissionRules";
			this.grpThirdPartyCommissionRules.TabIndex = 0;
			this.grpThirdPartyCommissionRules.TabStop = false;
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox6.Controls.Add(this.trvCompanyMaster);
			this.groupBox6.Location = new System.Drawing.Point(0, 0);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(160, 541);
			this.groupBox6.TabIndex = 1;
			this.groupBox6.TabStop = false;
			// 
			// trvCompanyMaster
			// 
			this.trvCompanyMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.trvCompanyMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.trvCompanyMaster.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.trvCompanyMaster.FullRowSelect = true;
			this.trvCompanyMaster.HideSelection = false;
			this.trvCompanyMaster.HotTracking = true;
			this.trvCompanyMaster.ImageIndex = -1;
			this.trvCompanyMaster.Location = new System.Drawing.Point(5, 51);
			this.trvCompanyMaster.Name = "trvCompanyMaster";
			this.trvCompanyMaster.SelectedImageIndex = -1;
			this.trvCompanyMaster.ShowLines = false;
			this.trvCompanyMaster.Size = new System.Drawing.Size(159, 484);
			this.trvCompanyMaster.TabIndex = 0;
			this.trvCompanyMaster.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvCompanyMaster_AfterSelect);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnAdd.Location = new System.Drawing.Point(3, 544);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 6;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.btnDelete.Location = new System.Drawing.Point(83, 544);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 6;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// tbcCompanyMaster
			// 
			appearance45.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance45.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcCompanyMaster.ActiveTabAppearance = appearance45;
			this.tbcCompanyMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			appearance46.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance46.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.tbcCompanyMaster.Appearance = appearance46;
			this.tbcCompanyMaster.Controls.Add(this.ultraTabSharedControlsPage1);
			this.tbcCompanyMaster.Controls.Add(this.ultraTabPageControl1);
			this.tbcCompanyMaster.Controls.Add(this.ultraTabPageControl2);
			this.tbcCompanyMaster.Controls.Add(this.ultraTabPageControl3);
			this.tbcCompanyMaster.Controls.Add(this.ultraTabPageControl4);
			this.tbcCompanyMaster.Controls.Add(this.ultraTabPageControl5);
			this.tbcCompanyMaster.Location = new System.Drawing.Point(160, 2);
			this.tbcCompanyMaster.Name = "tbcCompanyMaster";
			this.tbcCompanyMaster.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.tbcCompanyMaster.Size = new System.Drawing.Size(676, 566);
			this.tbcCompanyMaster.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcCompanyMaster.TabIndex = 8;
			appearance47.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			ultraTab20.Appearance = appearance47;
			ultraTab20.Key = "SetUpCompany ";
			ultraTab20.TabPage = this.ultraTabPageControl1;
			ultraTab20.Text = "Set Up Company";
			ultraTab21.Key = "CounterParties";
			ultraTab21.TabPage = this.ultraTabPageControl2;
			ultraTab21.Text = "Company CounterParties";
			ultraTab22.Key = "CompanyUser";
			ultraTab22.TabPage = this.ultraTabPageControl3;
			ultraTab22.Text = "Company User";
			ultraTab23.Key = "Clients";
			ultraTab23.TabPage = this.ultraTabPageControl4;
			ultraTab23.Text = "Company Clients";
			ultraTab24.Key = "CompanyThirdPaties";
			ultraTab24.TabPage = this.ultraTabPageControl5;
			ultraTab24.Text = "Company Third Parties";
			this.tbcCompanyMaster.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									   ultraTab20,
																									   ultraTab21,
																									   ultraTab22,
																									   ultraTab23,
																									   ultraTab24});
			this.tbcCompanyMaster.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			this.tbcCompanyMaster.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcCompanyMaster_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(674, 545);
			// 
			// groupBox5
			// 
			this.groupBox5.Location = new System.Drawing.Point(328, 224);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(262, 215);
			this.groupBox5.TabIndex = 3;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Third Party Components";
			// 
			// uctCompanyThirdPartyCommissionRules
			// 
			this.uctCompanyThirdPartyCommissionRules.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCompanyThirdPartyCommissionRules.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctCompanyThirdPartyCommissionRules.Location = new System.Drawing.Point(12, 32);
			this.uctCompanyThirdPartyCommissionRules.Name = "uctCompanyThirdPartyCommissionRules";
			this.uctCompanyThirdPartyCommissionRules.Size = new System.Drawing.Size(630, 222);
			this.uctCompanyThirdPartyCommissionRules.TabIndex = 0;
			// 
			// CompanyMaster
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(850, 617);
			this.Controls.Add(this.tbcCompanyMaster);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.btnDelete);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(856, 642);
			this.Name = "CompanyMaster";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Company Master";
			this.Load += new System.EventHandler(this.CompanyMaster_Load);
			this.ultraTabPageControl6.ResumeLayout(false);
			this.grpCompliance.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdMPID)).EndInit();
			this.gpbSecondaryContact.ResumeLayout(false);
			this.gpbPrimaryContact.ResumeLayout(false);
			this.gpbTechnologyContact.ResumeLayout(false);
			this.gpbDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTYpe)).EndInit();
			this.ultraTabPageControl7.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ultraTabPageControl8.ResumeLayout(false);
			this.grpCompanyInternalAccounts.ResumeLayout(false);
			this.ultraTabPageControl9.ResumeLayout(false);
			this.grpCompanyCompliance.ResumeLayout(false);
			this.ultraTabPageControl10.ResumeLayout(false);
			this.grpCompanyVenue.ResumeLayout(false);
			this.ultraTabPageControl11.ResumeLayout(false);
			this.grpCompanyCounterParties.ResumeLayout(false);
			this.ultraTabPageControl12.ResumeLayout(false);
			this.grpCompanyUser.ResumeLayout(false);
			this.ultraTabPageControl13.ResumeLayout(false);
			this.grpCompanyUserPermission.ResumeLayout(false);
			this.ultraTabPageControl14.ResumeLayout(false);
			this.grpCompanyClientDetails.ResumeLayout(false);
			this.ultraTabPageControl15.ResumeLayout(false);
			this.grpCompanyClientFunds.ResumeLayout(false);
			this.ultraTabPageControl17.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ultraTabPageControl16.ResumeLayout(false);
			this.grpCompanyClientTradingAccounts.ResumeLayout(false);
			this.ultraTabPageControl18.ResumeLayout(false);
			this.grpCompanyClientClearer.ResumeLayout(false);
			this.ultraTabPageControl19.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.ultraTabPageControl20.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			this.ultraTabPageControl21.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			this.ultraTabPageControl22.ResumeLayout(false);
			this.grpThirdPartyFileFormat.ResumeLayout(false);
			this.ultraTabPageControl23.ResumeLayout(false);
			this.groupBox11.ResumeLayout(false);
			this.ultraTabPageControl24.ResumeLayout(false);
			this.groupBox12.ResumeLayout(false);
			this.ultraTabPageControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanySetup)).EndInit();
			this.tbcCompanySetup.ResumeLayout(false);
			this.ultraTabPageControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyCounterParties)).EndInit();
			this.tbcCompanyCounterParties.ResumeLayout(false);
			this.ultraTabPageControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyUser)).EndInit();
			this.tbcCompanyUser.ResumeLayout(false);
			this.ultraTabPageControl4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyClients)).EndInit();
			this.tbcCompanyClients.ResumeLayout(false);
			this.ultraTabPageControl5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyThirdParty)).EndInit();
			this.tbcCompanyThirdParty.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcCompanyMaster)).EndInit();
			this.tbcCompanyMaster.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		
		//This method fetches the user permissions from the database.
		private void SetUpMenuPermissions()
		{	
			Preferences preferences = Preferences.Instance;	
			bool chkMaintainCompanies = preferences.Maintain_Companies;
			bool chkSetUpCompanies = preferences.Set_Up_Company;
			//If the user doesnt have the permissions to maintain companies then the Add & Delete buttons are
			//disabled so that he/she can't add or delete the companies.
			if((chkMaintainCompanies == true) && (chkSetUpCompanies == false))
			{
				btnAdd.Enabled = false;
				btnDelete.Enabled = false;
			}
		}
		
		/// <summary>
		/// This method saves the company on the click event of the button in the form while applying the
		/// validations on the required fields by checking for the null values or some other validation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSaveCompanySetup_Click(object sender, System.EventArgs e)
		{
			int result = int.MinValue;
			int selCompanyID = int.MinValue;
			bool isCompanyValue = false;
			Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
			try
			{
				if(trvCompanyMaster.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails) trvCompanyMaster.SelectedNode.Tag;

				
					//Save Company details.
					//-----------------------------------------------------------------------------
					Company company = new Company();
					//Match emailMatch = new Match();
					Match emailMatch = emailRegex.Match(txtPC1Email.Text.ToString());
				
					errorProvider1.SetError(txtCompanyName, "");
					errorProvider1.SetError(txtAddress1, "");
					errorProvider1.SetError(txtAddress2, "");
					errorProvider1.SetError(cmbCompanyTYpe, "");
					errorProvider1.SetError(txtTelephone, "");
					errorProvider1.SetError(txtPC1FirstName, "");
					errorProvider1.SetError(txtPC1Email, "");
					errorProvider1.SetError(txtPC1Telephone, "");
					if (txtCompanyName.Text == "")
					{
						errorProvider1.SetError(txtCompanyName, "Please enter Company Name!");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						txtCompanyName.Focus();
					}
					else if (txtAddress1.Text == "")
					{
						errorProvider1.SetError(txtAddress1, "Please enter Address1!");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						txtAddress1.Focus();
					}
					
					else if(cmbCompanyTYpe.Value == null)
					{
						errorProvider1.SetError(cmbCompanyTYpe, "Please select Company Type!");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						cmbCompanyTYpe.Focus();
					}
					else if (int.Parse(cmbCompanyTYpe.Value.ToString()) == int.MinValue)
					{
						errorProvider1.SetError(cmbCompanyTYpe, "Please select Company Type!");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						cmbCompanyTYpe.Focus();
					}

					else if (txtTelephone.Text == "")
					{
						errorProvider1.SetError(txtTelephone, "Please enter Company Telephone!");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						txtTelephone.Focus();
					}
					
					else if (txtPC1FirstName.Text == "")
					{
						errorProvider1.SetError(txtPC1FirstName, "Please enter Personal Contact1 First name!");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						txtPC1FirstName.Focus();
					}
					
					//else if (txtPC1Email.Text == "")
					
					else if (!emailMatch.Success)
					{
						//errorProvider1.SetError(txtPC1Email, "Please enter Personal Contact1 Email!");
						errorProvider1.SetError(txtPC1Email, "Please enter valid Email address !");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						txtPC1Email.Focus();
					}
					else if (txtPC1Telephone.Text == "")
					{
						errorProvider1.SetError(txtPC1Telephone, "Please enter Personal Contact1 Telephone No!");
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
						txtPC1Telephone.Focus();
					}
					
					
					else
					{
						company.CompanyID = nodeDetails.NodeID;
						company.Address1 = txtAddress1.Text;
						company.Address2 = txtAddress2.Text;
						company.CompanyTypeID = int.Parse(cmbCompanyTYpe.Value.ToString());
						company.Telephone = txtTelephone.Text;
						company.Fax = txtFax.Text;
						company.Name = txtCompanyName.Text;			
						company.PrimaryContactCell = txtPC1Cell.Text;
						company.PrimaryContactFirstName = txtPC1FirstName.Text;
						company.PrimaryContactLastName = txtPC1LastName.Text;
						company.PrimaryContactEMail = txtPC1Email.Text;
						company.PrimaryContactTelephone = txtPC1Telephone.Text;
						company.PrimaryContactTitle = txtPC1Title.Text;			
						company.SecondaryContactCell = txtPC2Cell.Text;
						company.SecondaryContactFirstName = txtPC2FirstName.Text;
						company.SecondaryContactLastName = txtPC2LastName.Text;
						company.SecondaryContactEMail = txtPC2Email.Text;
						company.SecondaryContactTelephone = txtPC2Telephone.Text;
						company.SecondaryContactTitle = txtPC2Title.Text;			
						company.TechnologyContactCell = txtTCCell.Text;
						company.TechnologyContactFirstName = txtTCFirstName.Text;
						company.TechnologyContactLastName = txtTCLastName.Text;
						company.TechnologyContactEMail = txtTCEmail.Text;
						company.TechnologyContactTelephone = txtTCTelephone.Text;
						company.TechnologyContactTitle = txtTCTitle.Text;

					
						result = CompanyManager.SaveCompany(company); //result now storing the value of stored companyID.
						//MPIDs mpids = (MPIDs)grdMPID.DataSource;
						MPIDCollection mpids = (MPIDCollection)grdMPID.DataSource;
						bool resultMPID = CompanyManager.SaveCompanyMPID(mpids, result);
						selCompanyID = result;

						int testCompanyID = nodeDetails.NodeID;
						if (result > int.MinValue)
						{
							if(testCompanyID != result)
							{
								BindTree();
								//Binding the tree to include the currently added or updated company in the tree.
								NodeDetails selectNodeDetails = new NodeDetails(NodeType.Company, selCompanyID);
								//Select the currently added or updated company in the tree. 
								SelectTreeNode(selectNodeDetails);
							}
						}
					}
					if (result != int.MinValue)
					{
						//-----------------------------------------------------------------------------

						//Save Company CounterParty
						//-----------------------------------------------------------------------------
						Nirvana.Admin.BLL.CounterParties counterParties = new CounterParties();
						CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
						//Loop through the checked items in the CounterParties listbox.
						for(int i=0, count = checkedlstCounterParties.CheckedItems.Count; i<count; i++)
						{					
							//Row selectedRow created to include the definition for the checked row in the listbox. 
							System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstCounterParties.CheckedItems[i]).Row;
							int counterPartyID = int.Parse(selectedRow["CounterPartyID"].ToString());
							int venueID = int.Parse(selectedRow["VenueID"].ToString());
							CounterParty cp = new CounterParty(counterPartyID, "");
							//Keep adding the selected CounterParty to the CounterParty collection if it
							//doesnt include its definition already.
							if(!counterParties.Contains(cp))
							{
								counterParties.Add(cp);
							}
							//Keep adding the selected CounterPartyVenue to the CounterPartyVenue collection 
							//if it doesnt include its definition already by checking the venueID to positive value.
							if(venueID > 0)
							{
								CounterPartyVenue cpv = new CounterPartyVenue();
								cpv.CounterPartyID = counterPartyID;
								cpv.VenueID = venueID;
								counterPartyVenues.Add(cpv);
							}
						}
						//SaveCompanyCounterParties method saves the CompanyCounterParties in the database.
						CounterPartyManager.SaveCompanyCounterParties(company.CompanyID, counterParties);
						
						//SaveCompanyVenues method saves the CompanyVenues in the database.
//						if(counterPartyVenues.Count > 0)
//						{
							CompanyManager.SaveCompanyCounterPartyVenues(company.CompanyID, counterPartyVenues);
//						}
						

						//Save selected Company AUEC's in the database.
						//-----------------------------------------------------------------------------
						int auecID = int.MinValue;
						AUECs aUECs = new AUECs();
						AUEC aUEC = new AUEC();
						for(int i=0, count = checkedlstAssetsUnderlyings.CheckedItems.Count; i<count; i++)
						{
							auecID = int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAssetsUnderlyings.CheckedItems[i]))).Row)).ItemArray[1].ToString());
							aUECs.Add(new Nirvana.Admin.BLL.AUEC(auecID));
						}
						CompanyManager.SaveCompanyAUECs(company.CompanyID, aUECs);
						//-----------------------------------------------------------------------------

						//Save selected Company Modules in the database.
						//-----------------------------------------------------------------------------
						Modules modules = new Modules();
						for(int i=0, count = checkedlstApplicationComponents.CheckedItems.Count; i<count; i++)
						{				
							modules.Add((Nirvana.Admin.BLL.Module)checkedlstApplicationComponents.CheckedItems[i]);
						}
						CompanyManager.SaveCompanyModules(company.CompanyID, modules);
						//-----------------------------------------------------------------------------

						//Save selected Company ThirdParty in the database.
						//-----------------------------------------------------------------------------
						ThirdParties thirdParties = new ThirdParties();
						for(int i=0, count = checkedlstThirdPartyComponents.CheckedItems.Count; i<count; i++)
						{				
							System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstThirdPartyComponents.CheckedItems[i]).Row;
							int thirdPartyID = int.Parse(selectedRow["ThirdPartyID"].ToString());
							if(thirdPartyID != int.MinValue) //So that no ThirdParty Type head is added to the collection.
							{
								thirdParties.Add(new ThirdParty(thirdPartyID, ""));
								//thirdParties.Add((ThirdParty)checkedlstThirdPartyComponents.CheckedItems[i]);
							}
						}
						CompanyManager.SaveCompanyThirdParty(company.CompanyID, thirdParties);
						//-----------------------------------------------------------------------------
//
//						//Save Company Funds to the database by getting the exising rows in the Funds Grid.
//						//These funds are feched from the control by accessing the CurrentFunds property of
//						//the user control.
						Funds funds = uctCompanyFunds.CurrentFunds;
						int resultFund = CompanyManager.SaveFund(funds, company.CompanyID);
//
//						//Save Company TradingAccounts to the database by getting the exising rows in the 
//						//TradingAccounts Grid.
//						//These tradingAccounts are feched from the control by accessing the 
//						//CurrentTradingAccounts property of the user control.
						TradingAccounts tradingAccounts = uctCompanyTradingAccounts.CurrentTradingAccounts;
						int resultTradingAccount = CompanyManager.SaveTradingAccount(tradingAccounts, company.CompanyID);

						//Save Company Strategies to the database by getting the exising rows in the Strategies 
						//Grid.
						//These strategies are feched from the control by accessing the CurrentStrategies 
						//property of the user control.
						Strategies strategies = uctCompanyStrategy.CurrentStrategies;
						int resultStrategy = CompanyManager.SaveStrategy(strategies, company.CompanyID);

						//Save Company ClearingFirmsPrimeBrokers to the database by getting the exising rows 
						//in the ClearingFirmsPrimeBrokers Grid.
						//These clearingFirmsPrimeBrokers are feched from the control by accessing the 
						//CurrentClearingFirmsPrimeBrokers property of the user control.
						Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = uctClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers;
						int resultClearingFirmPrimeBroker = CompanyManager.SaveClearingFirmPrimeBroker(clearingFirmsPrimeBrokers, company.CompanyID);

						//Save Company Compliance & Borrower details to the database by getting these details
						//from the Compliance control by accessing its Company property.
						Company companyCompliance = uctCompanyCompliance.CompanyComplaince;
						if(companyCompliance != null)
						{
							companyCompliance.CompanyID = company.CompanyID; //The assignment is done to copy the 
							//selected companyid to the tobe saved company compliance & borrower details.
						
							int resultCompanyCompliance = CompanyManager.SaveCompanyCompliance(companyCompliance);
							int resultCompanyBorrower = CompanyManager.SaveCompanyBorrower(companyCompliance);

							//Save Company Venue details to the database by getting these details
							//from the Company Venue control by accessing its CompanyVenue property.
							Company companyVenue = uctCompanyVenue.GetCompanyVenueDetails();
							if(companyVenue != null)
							{
								companyVenue.CompanyID = company.CompanyID; //The assignment is done to copy the 
								//selected companyid to the tobe saved company venue details.
						
								int resultCompanyVenue = CompanyManager.SaveCompanyVenueDetails(companyVenue);


								BindTree();
								//Binding the tree to include the currently added or updated company in the tree.
								NodeDetails selectNodeDetails = new NodeDetails(NodeType.Company, selCompanyID);
								//Select the currently added or updated company in the tree. 
								SelectTreeNode(selectNodeDetails);
							}

							else
							{
								tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[4];
							}
					
							
						}
						else
						{
							tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[3];
						}
						
										
					}
					else
					{
//						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCompany);
//						Nirvana.Admin.Utility.Common.SetStatusPanel(stbCompany, "Please Fill the details");						
						tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
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

				LogEntry logEntry = new LogEntry("btnSaveCompanySetup_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSaveCompanySetup_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// Binds left tree with relevent data.
		/// </summary>
		private void BindTree()
		{
			//To clear the tree of any node before binding it afresh.
			trvCompanyMaster.Nodes.Clear();

			//Create Copmpany nodes.
			//Add label Company.

			//Font font = new Font("Vedana", 8.25F , System.Drawing.FontStyle.Bold);
			Font font = new Font("Tahoma", 11, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

			TreeNode treeNodeCompanies = new TreeNode("Company");
			//Making the root node to bold by assigning it to the font object defined above. 
			treeNodeCompanies.NodeFont = font;
			NodeDetails companiesNode = new NodeDetails(NodeType.Company, int.MinValue, int.MinValue); 
			treeNodeCompanies.Tag = companiesNode;

			//GetCompanies method is used to fetch the existing companies in the database.
			Companies companies = CompanyManager.GetCompanies();
			try
			{
				//Loop through all the companies, users, counterparties and clients, assigning each node an id 
				//corresponding to its unique value in the database.
				foreach(Company company in companies)
				{
					TreeNode treeNodeCompany = new TreeNode(company.Name);
					//Making the root node to bold by assigning it to the font object defined above. 
					treeNodeCompany.NodeFont = font;
					NodeDetails companyNode = new NodeDetails(NodeType.Company, company.CompanyID, company.CompanyID); 
					treeNodeCompany.Tag = companyNode;

					//Create copmpany counterparties' nodes.
					// --------------------------------------------------------------------------
				
					//Add label Counter Parties.
					TreeNode treeNodeCounterParties = new TreeNode("CounterParties");
					//Making the root node to bold by assigning it to the font object defined above. 
					treeNodeCounterParties.NodeFont = font;
					NodeDetails counterPartiesNode = new NodeDetails(NodeType.CounterParties, int.MinValue, company.CompanyID); 
					treeNodeCounterParties.Tag = counterPartiesNode;	
				
					//GetCompanyCounterParties method is used to fetch the existing CounterParties in the 
					//database to a given company.
					CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(company.CompanyID);
					foreach(CounterParty counterParty in counterParties)
					{
						TreeNode treeNodeCounterParty = new TreeNode(counterParty.ShortName);
						NodeDetails counterPartyNode = new NodeDetails(NodeType.CounterParties, counterParty.CounterPartyID, company.CompanyID); 
						treeNodeCounterParty.Tag = counterPartyNode;	

						//Add label Counter Parties Venues.
						TreeNode treeNodeCounterPartyVenues = new TreeNode("CounterPartyVenues");
						//Making the root node to bold by assigning it to the font object defined above. 
						treeNodeCounterPartyVenues.NodeFont = font;
						NodeDetails counterPartyVenuesNode = new NodeDetails(NodeType.CounterPartyVenues, int.MinValue, company.CompanyID); 
						treeNodeCounterPartyVenues.Tag = counterPartyVenuesNode;	

						//GetCompanyCounterPartyVenues method is used to fetch the existing CounterPartyVenues in the 
						//database to a given company.
						CompanyCounterPartyVenues companyCounterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneusChanged(company.CompanyID, counterParty.CounterPartyID);
						foreach(CompanyCounterPartyVenue companyCounterPartyVenue in companyCounterPartyVenues)
						{
							TreeNode treeNodeCounterPartyVenue = new TreeNode(companyCounterPartyVenue.CounterPartyVenueDisplayName);
							NodeDetails counterPartyVenueNode = new NodeDetails(NodeType.CounterPartyVenues, companyCounterPartyVenue.CompanyCounterPartyCVID, company.CompanyID); 
							treeNodeCounterPartyVenue.Tag = counterPartyVenueNode;	

							treeNodeCounterPartyVenues.Nodes.Add(treeNodeCounterPartyVenue);
						}
						treeNodeCounterParty.Nodes.Add(treeNodeCounterPartyVenues);

						treeNodeCounterParties.Nodes.Add(treeNodeCounterParty);
					}

					treeNodeCompany.Nodes.Add(treeNodeCounterParties);
					
					
					
					// --------------------------------------------------------------------------

					//Create copmpany user nodes.
					// --------------------------------------------------------------------------
				
					//Add label USER.
					TreeNode treeNodeUsers = new TreeNode("Users");
					//Making the root node to bold by assigning it to the font object defined above. 
					treeNodeUsers.NodeFont = font;
					NodeDetails usersNode = new NodeDetails(NodeType.Users, int.MinValue, company.CompanyID); 
					treeNodeUsers.Tag = usersNode;	
				
					//GetUsers method is used to fetch the existing users in the database to a given company.
					Users users = UserManager.GetUsers(company.CompanyID);
					foreach(User user in users)
					{
						TreeNode treeNodeUser = new TreeNode(user.ShortName + " " + user.FirstName);
						NodeDetails userNode = new NodeDetails(NodeType.Users, user.UserID, company.CompanyID); 
						treeNodeUser.Tag = userNode;	

						treeNodeUsers.Nodes.Add(treeNodeUser);
					}

					treeNodeCompany.Nodes.Add(treeNodeUsers);
					// --------------------------------------------------------------------------

					

					//Create copmpany clients' nodes.
					// --------------------------------------------------------------------------
				
					//Add label Clients
					TreeNode treeNodeClients = new TreeNode("Clients");
					//Making the root node to bold by assigning it to the font object defined above. 
					treeNodeClients.NodeFont = font;
					NodeDetails clientsNode = new NodeDetails(NodeType.Clients, int.MinValue, company.CompanyID); 
					treeNodeClients.Tag = clientsNode;	
				
					//GetClients method is used to fetch the existing Clients in the database to a given company.
					Nirvana.Admin.BLL.Clients clients = CompanyManager.GetClients(company.CompanyID);
					foreach(Client client in clients)
					{
						TreeNode treeNodeClient = new TreeNode(client.ClientName);
						NodeDetails clientNode = new NodeDetails(NodeType.Clients, client.ClientID, company.CompanyID); 
						treeNodeClient.Tag = clientNode;	

						treeNodeClients.Nodes.Add(treeNodeClient);
					}

					treeNodeCompany.Nodes.Add(treeNodeClients);
					// --------------------------------------------------------------------------

					//Add label ThirdParties
					TreeNode treeNodeThirdParties = new TreeNode("ThirdParties");
					//Making the root node to bold by assigning it to the font object defined above. 
					treeNodeThirdParties.NodeFont = font;
					NodeDetails thirdPartiesNode = new NodeDetails(NodeType.ThirdParty, int.MinValue, company.CompanyID); 
					treeNodeThirdParties.Tag = thirdPartiesNode;

					//GetThirdParties method is used to fetch the existing thirdparties in the database to a given company.
					Nirvana.Admin.BLL.ThirdParties thirdParties = ThirdPartyManager.GetCompanyThirdParties(company.CompanyID);
					foreach(ThirdParty thirdParty in thirdParties)
					{
						TreeNode treeNodeThirdParty = new TreeNode(thirdParty.ThirdPartyName);
						NodeDetails thirdPartyNode = new NodeDetails(NodeType.ThirdParty, thirdParty.ThirdPartyID, company.CompanyID); 
						treeNodeThirdParty.Tag = thirdPartyNode;	

						treeNodeThirdParties.Nodes.Add(treeNodeThirdParty);
					}

					treeNodeCompany.Nodes.Add(treeNodeThirdParties);
					// --------------------------------------------------------------------------
				
					treeNodeCompanies.Nodes.Add(treeNodeCompany);
				}
				trvCompanyMaster.Nodes.Add(treeNodeCompanies);

				trvCompanyMaster.TopNode.Expand();
				if(trvCompanyMaster.Nodes.Count > 0)
				{				
					if(trvCompanyMaster.Nodes[0].Nodes.Count > 0)
					{
						//trvCompanyMaster.Nodes[0].Nodes[0].ExpandAll();
					}
				}
				
				//The code below fetches the user permissions from the database.
				Preferences preferences = Preferences.Instance;	
				bool chkMaintainCompanies = preferences.Maintain_Companies;
				bool chkSetUpCompanies = preferences.Set_Up_Company;
				
				//This check is done to ensure that the user cant add the new company as on binding
				//the tree, existing company is made seleced thus eliminating the add possibility. 
				if((chkMaintainCompanies == true) && (chkSetUpCompanies == false))
				{
					if(trvCompanyMaster.Nodes[0].Nodes.Count > 0)
					{
						trvCompanyMaster.SelectedNode = trvCompanyMaster.Nodes[0].Nodes[0];
					}
				}
				else
				{
					trvCompanyMaster.SelectedNode = trvCompanyMaster.Nodes[0];
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

				LogEntry logEntry = new LogEntry("BindTree", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "BindTree"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		//Constant definitions by user for tab index.
		const int TAB_INDEX_COMPANY_SETUP = 0;
		const int TAB_INDEX_COMPANY_USER = 1;
		const int TAB_INDEX_COMPANY_COUNTERPARTIES = 2;
		const int TAB_INDEX_COMPANY_CLIENTS = 3;

		/// <summary>
		/// On the load event of this form some binding functions are called and some initialization for a 
		/// company is done. Also by default the company tab is shown selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CompanyMaster_Load(object sender, System.EventArgs e)
		{
			try
			{
				//TODO: Select the comapny's first tab.
				//tbcCompanyMaster.SelectedTab.Index = TAB_INDEX_COMPANY_SETUP;
				BindTree();
				//Set up Company.
				InitializeSetUpCompany();			
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

				LogEntry logEntry = new LogEntry("CompanyMaster_Load", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "CompanyMaster_Load"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// This method initializes a company controls by binding combo boxes, getting all the existing 
		/// underlyings, assets, modules, thirdparties, counterparties, counterpartyvenues. 
		/// </summary>
		private void InitializeSetUpCompany()
		{
			//Initializes the compliance combo.
			InitializeCompliance();
			//Binds the company type combo box.
			BindCompanyType();
			
			try
			{
				//Bind AUEC
				AUECs auecs = AUECManager.GetAUEC();

				System.Data.DataTable dtauec = new System.Data.DataTable();
				dtauec.Columns.Add("Data");
				dtauec.Columns.Add("Value");
				object[] rowAUEC = new object[2]; 
				if (auecs.Count > 0 )
				{
					foreach(Nirvana.Admin.BLL.AUEC auec in auecs)
					{
						Currency currency = new Currency();
						//currency = AUECManager.GetCurrency(auec.Exchange.Currency);
						currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
						string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();
						int Value = auec.AUECID;
					
						rowAUEC[0] = Data;
						rowAUEC[1] = Value;
						dtauec.Rows.Add(rowAUEC);
					}
					checkedlstAssetsUnderlyings.DataSource = dtauec;
					checkedlstAssetsUnderlyings.DisplayMember = "Data";
					checkedlstAssetsUnderlyings.ValueMember = "Value";
				}
			
				//Bind Module list
				Modules modules = ModuleManager.GetModules();
				if(modules.Count > 0)
				{
//					lstApplicationComponents.DataSource = modules;
//					lstApplicationComponents.DisplayMember = "ModuleName";
//					lstApplicationComponents.ValueMember = "ModuleID";

					checkedlstApplicationComponents.DataSource = modules;
					checkedlstApplicationComponents.DisplayMember = "ModuleName";
					checkedlstApplicationComponents.ValueMember = "ModuleID";
				}
			
				//Bind Thirdparty
				ThirdPartyTypes thirdPartyTypes = ThirdPartyManager.GetThirdPartyTypes();
				ThirdParties thirdParties = ThirdPartyManager.GetThirdParties();

				System.Data.DataTable tempDataTableThirdParty = new System.Data.DataTable();
				tempDataTableThirdParty.Columns.Add("DisplayData");
				tempDataTableThirdParty.Columns.Add("ThirdPartyTypeID");
				tempDataTableThirdParty.Columns.Add("ThirdPartyID");

				foreach(ThirdPartyType thirdPartyType in thirdPartyTypes)
				{
					System.Data.DataRow row = tempDataTableThirdParty.NewRow();
					row = tempDataTableThirdParty.NewRow();
					//row.Table.Columns[0].Caption
					row["DisplayData"] = thirdPartyType.ThirdPartyTypeName;
					row["ThirdPartyTypeID"] = thirdPartyType.ThirdPartyTypeID;
					row["ThirdPartyID"] = int.MinValue;						
					tempDataTableThirdParty.Rows.Add(row);
					foreach(ThirdParty thirdParty in thirdPartyType.ThirdParties)
					{
//						System.Data.DataRow row = tempDataTableThirdParty.NewRow();
//						row = tempDataTableThirdParty.NewRow();
//						row["DisplayData"] = thirdParty.ThirdPartyTypeName;
//						row["ThirdPartyTypeID"] = thirdParty.ThirdPartyTypeID;
//						row["ThirdPartyID"] = int.MinValue;						
//						tempDataTableThirdParty.Rows.Add(row);
	
						switch(thirdParty.ThirdPartyTypeID)	
						{
							case C_TYPE_PRIMEBROKERCLEARER:
								System.Data.DataRow rowTPPB = tempDataTableThirdParty.NewRow();
								rowTPPB["DisplayData"] = "      " + thirdParty.ThirdPartyName;
								rowTPPB["ThirdPartyTypeID"] = thirdParty.ThirdPartyTypeID;
								rowTPPB["ThirdPartyID"] = thirdParty.ThirdPartyID;					
								tempDataTableThirdParty.Rows.Add(rowTPPB);
								break;

							case C_TYPE_VENDOR:
								System.Data.DataRow rowTPV = tempDataTableThirdParty.NewRow();
								rowTPV["DisplayData"] = "      " + thirdParty.ThirdPartyName;
								rowTPV["ThirdPartyTypeID"] = thirdParty.ThirdPartyTypeID;
								rowTPV["ThirdPartyID"] = thirdParty.ThirdPartyID;					
								tempDataTableThirdParty.Rows.Add(rowTPV);
								break;

							case C_TYPE_CUSTODIAN:
								System.Data.DataRow rowCUS = tempDataTableThirdParty.NewRow();
								rowCUS["DisplayData"] = "      " + thirdParty.ThirdPartyName;
								rowCUS["ThirdPartyTypeID"] = thirdParty.ThirdPartyTypeID;
								rowCUS["ThirdPartyID"] = thirdParty.ThirdPartyID;					
								tempDataTableThirdParty.Rows.Add(rowCUS);
								break;

							case C_TYPE_ADMINISTRATOR:
								System.Data.DataRow rowADM = tempDataTableThirdParty.NewRow();
								rowADM["DisplayData"] = "      " + thirdParty.ThirdPartyName;
								rowADM["ThirdPartyTypeID"] = thirdParty.ThirdPartyTypeID;
								rowADM["ThirdPartyID"] = thirdParty.ThirdPartyID;					
								tempDataTableThirdParty.Rows.Add(rowADM);
								break;
						}
					}
				}

				if(thirdParties.Count > 0)
				{
					checkedlstThirdPartyComponents.DataSource = tempDataTableThirdParty;
					checkedlstThirdPartyComponents.DisplayMember = "DisplayData";
					checkedlstThirdPartyComponents.ValueMember = "ThirdPartyID";
				}

//				ThirdParties thirdParties = ThirdPartyManager.GetThirdParties();
//								
//				if(thirdParties.Count > 0)
//				{
//					lstThirdPartyComponents.DataSource = thirdParties;
//					lstThirdPartyComponents.DisplayMember = "ThirdPartyName";
//					lstThirdPartyComponents.ValueMember = "ThirdPartyID";
//
//					checkedlstThirdPartyComponents.DataSource = thirdParties;
//					checkedlstThirdPartyComponents.DisplayMember = "ThirdPartyName";
//					checkedlstThirdPartyComponents.ValueMember = "ThirdPartyID";
//				}

				//Creating a datatable tempDataTable to add into it the counterParty and venue to be binded to 
				//the listbox later.
				CounterParties counterParties = CounterPartyManager.GetCounterParties();
				System.Data.DataTable tempDataTable = new System.Data.DataTable();
				tempDataTable.Columns.Add("DisplayData");
				tempDataTable.Columns.Add("CounterPartyID");
				tempDataTable.Columns.Add("VenueID");				
				foreach(CounterParty counterParty in counterParties)
				{
					System.Data.DataRow row = tempDataTable.NewRow();
					row["DisplayData"] = counterParty.CounterPartyFullName;
					row["CounterPartyID"] = counterParty.CounterPartyID;
					row["VenueID"] = int.MinValue;					
					tempDataTable.Rows.Add(row);
					foreach(Venue venue in counterParty.Venues)
					{
						row = tempDataTable.NewRow();
						row["DisplayData"] = "      " + venue.VenueName;
						row["CounterPartyID"] = counterParty.CounterPartyID;
						row["VenueID"] = venue.VenueID;						
						tempDataTable.Rows.Add(row);
					}
				}

				if(counterParties.Count > 0)
				{
//					lstCounterParties.DataSource = null;
//					lstCounterParties.Items.Clear();
//					lstCounterParties.DataSource = tempDataTable;// counterParties;
//					lstCounterParties.DisplayMember = "DisplayData";//"CounterPartyFullName";
//					lstCounterParties.ValueMember = "CounterPartyID";//"CounterPartyID";	
					
					checkedlstCounterParties.DataSource = null;
					checkedlstCounterParties.Items.Clear();
					checkedlstCounterParties.DataSource = tempDataTable;// counterParties;
					checkedlstCounterParties.DisplayMember = "DisplayData";//"CounterPartyFullName";
					checkedlstCounterParties.ValueMember = "CounterPartyID";//"CounterPartyID";	
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

				LogEntry logEntry = new LogEntry("InitializeSetUpCompany", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "InitializeSetUpCompany"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		///This method adds the new company, user, counterparty, client on the click event of the button. 
		/// It adds the company, user, counterparty or client based on the tree selection before the add 
		/// button is clicked. If the selection was on the company root or child then the company is added.
		/// Similarly others can be added depending upon the nodes selection in the tree.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			NodeDetails nodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Tag;			
			
			try
			{
				//If no node is selected in the tree.
				if(nodeDetails == null)
				{
					MessageBox.Show("Select Some node!");
					tbcCompanyMaster.Enabled = false;
				}
				else
				{
					switch(nodeDetails.Type)
					{
						case NodeType.Company:
							//Select Company main node.
							trvCompanyMaster.SelectedNode = trvCompanyMaster.Nodes[0];
							//Select company Tab.
							//tbcCompanyMaster.SelectedTab = tbcCompanyMaster.TabPages[0];
							tbcCompanySetup.SelectedTab = tbcCompanySetup.Tabs[0];
							break;

						case NodeType.Clients:
							//Select client node for that company.
							if(nodeDetails.NodeID != int.MinValue)
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent;								
							}
							//Select client Tab.
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[3];
							//uctGridClientFunds1.CompanyClientID = int.MinValue;
							uctGridClientFunds1.SetupControl(int.MinValue);

							//uctClientTradingAccounts.CompanyClientID = int.MinValue;
							uctClientTradingAccounts.SetUp(nodeDetails.CompanyID, int.MinValue);
							tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[0];
							break;

						case NodeType.CounterParties:
							//Select client node for that company.
							if(nodeDetails.NodeID != int.MinValue)
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent;
							}
							//Select counterparties Tab.
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[1];
							
							break;
						
						case NodeType.Users:
							//Select client node for that company.
							if(nodeDetails.NodeID != int.MinValue)
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent;
							}
							//Select User Tab.
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[2];
							//uctCompanyUserPermissions.CompanyID = nodeDetails.CompanyID;
							//uctCompanyUserDetail.CompanyID = nodeDetails.CompanyID;
							uctCompanyUserDetail.SetupControl(nodeDetails.CompanyID, nodeDetails.NodeID);
							tbcCompanyUser.SelectedTab = tbcCompanyUser.Tabs[0];

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
		/// This method binds the existing <see cref="CompanyTypes"/> in the ComboBox control by assigning the 
		/// companyTypes object to its datasource property.
		/// </summary>
		private void BindCompanyType()
		{
			CompanyTypes companyTypes = CompanyManager.GetCompanyTypes();
			cmbCompanyTYpe.DataSource = companyTypes;
			companyTypes.Insert(0, new CompanyType(int.MinValue, C_COMBO_SELECT));
			cmbCompanyTYpe.DisplayMember = "Type";
			cmbCompanyTYpe.ValueMember = "CompanyTypeID";
			cmbCompanyTYpe.Text = C_COMBO_SELECT;
		}

		/// <summary>
		/// This method fills the compliance combo boxes with the some default values at the begining of form
		/// load.
		/// </summary>
		private void InitializeCompliance()
		{
			//ToDo: Remove this thing and put original things.
			System.Data.DataTable dt = new System.Data.DataTable();
			
			//CurrencyConvention
			dt.Columns.Add("Data");
			dt.Columns.Add("Value");
			object[] row = new object[2]; 
			row[0] = "AAPP";
			row[1] = "1";
			dt.Rows.Add(row);			
//			cmbMPID.DataSource = dt;
//			cmbMPID.DisplayMember = "Data";
//			cmbMPID.ValueMember = "Value";
		}

		/// <summary>
		/// This method deletes the selected node when clicking the delete button in the form. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(trvCompanyMaster.SelectedNode.Parent != null)
				{
					if(trvCompanyMaster.SelectedNode == null)
					{
						//Nothing is selecte in Node tree.
						MessageBox.Show("Please select Company/User/Counter Party/!");
					}
					else
					{
					
						//Getting the info about the previous node so as to select it after deleting the 
						//currently selected node.
						NodeDetails prevNodeDetails = new NodeDetails();
						if(trvCompanyMaster.SelectedNode.PrevNode != null)
						{
							prevNodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.PrevNode.Tag;
						}
						else
						{
							prevNodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Parent.Tag;
						}
					
						//Getting the info about the currently selected node to be deleted.
						NodeDetails nodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Tag;
						switch(nodeDetails.Type)
						{
							case NodeType.Company:
								int companyID = nodeDetails.NodeID;
								if(MessageBox.Show(this, "Do you want to delete selected Company?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{
									//Deleting the selected company before checking it for reference in other 
									//nodes or forms.
									bool chkVarraible = CompanyManager.DeleteCompany(companyID, false);
									if(!(chkVarraible))
									{
										MessageBox.Show(this, "Company is referenced in /Company User/Clients.\n You can not delete it.", "Nirvana Alert");
									}
									else
									{
										//Binding the tree after deleting the selected node and selecting its
										//previous node.
										BindTree();
										SelectTreeNode(prevNodeDetails);
									}
								}
								break;
							case NodeType.Clients:
								int clientID = nodeDetails.NodeID;
								if(MessageBox.Show(this, "Do you want to delete selected Client?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{
									//Deleting the selected Client before checking it for reference in other 
									//nodes or forms.
									bool chkVarraible = CompanyManager.DeleteCompanyClient(nodeDetails.CompanyID, clientID, true);
									if(!(chkVarraible))
									{
										MessageBox.Show(this, "Client is referenced in /Client Funds/Client Traders.\n You cant delete it?", "Nirvana Alert");
									}
									else
									{
										//Binding the tree after deleting the selected node and selecting its
										//previous node.
										BindTree();
										SelectTreeNode(prevNodeDetails);
									}
								}
								break;

							case NodeType.CounterPartyVenues:
								int companyCounterPartyVenueID = nodeDetails.NodeID;
								if(MessageBox.Show(this, "Do you want to delete selected CounterParty?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{
									//Delete CounterParties with its details.																
									if(companyCounterPartyVenueID != int.MinValue)
									{
										CompanyManager.DeleteCompanyCounterPartyVenueDetail(companyCounterPartyVenueID);
										//Binding the tree after deleting the selected node and selecting its
										//previous node.
										BindTree();
										SelectTreeNode(prevNodeDetails);
									}

								}
								break;

							case NodeType.Users:
								int userID = nodeDetails.NodeID;
								if(MessageBox.Show(this, "Do you want to delete selected User?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{
									bool chkVarraible = UserManager.DeleteCompanyUser(nodeDetails.CompanyID, userID);
									if(!(chkVarraible))
									{
										//User not deleted.	
									}
									else
									{
										//Binding the tree after deleting the selected node and selecting its
										//previous node.
										BindTree();
										SelectTreeNode(prevNodeDetails);
									}
								}
								break;
						}
					
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

				LogEntry logEntry = new LogEntry("btnDelete_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnDelete_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// This method closes the form when clicked the close button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
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

				LogEntry logEntry = new LogEntry("btnClose_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnClose_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// This methods selects the respected node as per the selected tab on click event of the tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tbcCompanyMaster_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				NodeDetails nodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Tag;
			
				if(nodeDetails.NodeID == int.MinValue && nodeDetails.CompanyID == int.MinValue)
				{
					return;
				}
				if(nodeDetails.NodeID == int.MinValue) //Have to c for the excption here when we click on the tab while selecting the tree in CVDetail.
				{//Either of Users/Counterparties/Clients nodes is selected.
				
					if((nodeDetails.Type != NodeType.Company) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYSETUP])
					{
						trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent;
					}								
					if((nodeDetails.Type != NodeType.Users) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYUSER])
					{
						trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[0];
						
					}				
					if((nodeDetails.Type != NodeType.CounterParties) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCOUNTERPARTIES])
					{
						if(trvCompanyMaster.SelectedNode.Parent.Nodes[1].IsVisible == true)
						{
							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[0];
						}
					}				
					if((nodeDetails.Type != NodeType.Clients) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCLIENTS])
					{
						trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[2];
					}				
				}
				else
				{//Either of Users/Counterparties/Clients child nodes is selected or Company node is selected.
					if(nodeDetails.NodeID == nodeDetails.CompanyID)
					{
						if((nodeDetails.Type != NodeType.Users) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYUSER])
						{
							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Nodes[0];
						}				
//						if((nodeDetails.Type != NodeType.CounterParties) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.TabPages[C_TAB_COMPANYCOUNTERPARTIES])
//						{
//							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Nodes[1];
//						}				
						if((nodeDetails.Type != NodeType.Clients) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCLIENTS])
						{
					
							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Nodes[2];
						}

					}
					else
					{
						if((nodeDetails.Type != NodeType.Company) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYSETUP])
						{
							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent;
						}								
						if((nodeDetails.Type != NodeType.Users) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYUSER])
						{
							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[1];
						}				
//						if((nodeDetails.Type != NodeType.CounterParties) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.TabPages[C_TAB_COMPANYCOUNTERPARTIES])
//						{
//							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[1];
//						}				
						if((nodeDetails.Type != NodeType.Clients) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCLIENTS])
						{
							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[2];
						}
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

				LogEntry logEntry = new LogEntry("tbcCompanyMaster_SelectedIndexChanged", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "tbcCompanyMaster_SelectedIndexChanged"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// Disables all the tabs in the form.
		/// </summary>
		private void DisableCompanyTabs()
		{
			tbcCompanyCounterParties.Enabled = false;
			tbcCompanyMaster.Enabled = false;
			tbcCompanyUser.Enabled = false;
			tbcCompanyClients.Enabled = false;
		}

		/// <summary>
		/// Enables all the tabs in the form.
		/// </summary>
		private void EnableCompanyTabs()
		{
			tbcCompanyCounterParties.Enabled = true;
			tbcCompanyMaster.Enabled = true;
			tbcCompanyUser.Enabled = true;
			tbcCompanyClients.Enabled = true;
		}

		/// <summary>
		/// This method shows the details of the selected node on the click event of the tree. It fetches the
		/// details of the selected node from the database by sending the nodeID.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trvCompanyMaster_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			NodeDetails nodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Tag;			
			try
			{
			
				if(nodeDetails == null)
				{
					MessageBox.Show("Select Some node!");
					tbcCompanyMaster.Enabled = false;
				}
				else
				{
					switch(nodeDetails.Type)
					{
						//Showing all the details pertaining to the selected company from the database..
						case NodeType.Company:
							
							//Nirvana.Admin.Controls.CompanyUser[] uctCompanyUser ;
							//uctCompanyUser = new CompanyUser[] {new Nirvana.Admin.Controls.CompanyUser()};
							
							if(nodeDetails.NodeID == int.MinValue && nodeDetails.CompanyID == int.MinValue)
							{
								//uctGridCompanyCountePartiesCompanyLevelTags.CompanyCounterPartyVenueID = int.MinValue;
								uctGridCompanyCountePartiesCompanyLevelTags.SetupControl(int.MinValue, int.MinValue, nodeDetails.NodeID);

								//uctCompanyUserDetail.RefreshUserDetail();
								uctCompanyUserDetail.SetupControl(int.MinValue, int.MinValue);
								
								//uctClientClearer.CompanyClientClearer =new Nirvana.Admin.BLL.CompanyClientClearer();
								uctClientClearer.SetupControl(int.MinValue);

								//uctClientTradingAccounts.CompanyClientID = int.MinValue;
								uctClientTradingAccounts.SetUp(int.MinValue, int.MinValue);

								//uctGridClientFix.ClientID = int.MinValue;
								uctGridClientFix.SetUp(int.MinValue);
								
								//uctClientPermission.ClientID = int.MinValue;
								uctClientPermission.Setup(int.MinValue, int.MinValue);
				
								uctClientTradersDetail.SetupControl(int.MinValue);
								uctClientCompanyControl.RefreshCompanyClientDetail();
								uctGridClientFunds1.SetupControl(int.MinValue);
								uctGridClientFix.SetUp(int.MinValue);
							}
							
							//Selecting the company tab.
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[0];
							int companyID = nodeDetails.NodeID;
						
							CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(companyID);
							CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
							Nirvana.Admin.BLL.Company company = CompanyManager.GetCompany(companyID);

							//MPIDs companyMPIDs = CompanyManager.GetCompanyMPIDs(companyID);
							MPIDCollection companyMPIDs = CompanyManager.GetCompanyMPIDs(companyID);
							
							grdMPID.DataSource = companyMPIDs;
						
							Modules modules = ModuleManager.GetCompanyModules(companyID);

							ThirdParties thirdParties = ThirdPartyManager.GetCompanyThirdParties(companyID);

//							Assets assets = AssetManager.GetCompanyAssets(companyID);
//
//							UnderLyings underLyings = AssetManager.GetCompanyUnderLyings(companyID);

							AUECs companyAUECs = CompanyManager.GetCompanyAUECs(companyID);
							CompanyDetails(company, counterParties, counterPartyVenues, modules, thirdParties, companyAUECs);
							
							//Passing the companyID to the user control insances having the CompanyID property
							//which fetches and shows the details in the form.
							//uctCompanyFunds.CompanyID = companyID;
							uctCompanyFunds.SetupControl(companyID);

							//uctClearingFirmsPrimeBrokers.CompanyID = companyID;
							uctClearingFirmsPrimeBrokers.SetupControl(companyID);

							//uctCompanyStrategy.CompanyID = companyID;
							uctCompanyStrategy.SetupControl(companyID);

							//uctCompanyTradingAccounts.CompanyID = companyID;
							uctCompanyTradingAccounts.SetupControl(companyID);

							//Company companyCompliance = CompanyManager.GetCompanyCompliance(companyID);
							//uctCompanyCompliance.SetCompanyComplianceDetails(companyCompliance);
							uctCompanyCompliance.SetupControl(companyID);
							
//							Company companyBorrower = CompanyManager.GetCompanyBorrower(companyID);
//							uctCompanyCompliance.SetCompanyBorrowerDetails(companyBorrower);
							
//							Company companyVenueDetail = new Company();
//							companyVenueDetail = CompanyManager.GetCompanyVenueDetails(companyID);
//							uctCompanyVenue.CompanyVenueProperty = companyVenueDetail;
							uctCompanyVenue.SetupControl(companyID);

//							uctCompanyThirdPartyFileFormats.CompanyID = companyID;
							uctCompanyThirdPartyFileFormats.SetupControl(companyID);
							Funds fileFormatFunds = CompanyManager.GetCompanyThirdPartyFileFormats(companyID);
							uctCompanyThirdPartyFileFormats.FileFormatsFunds = fileFormatFunds;

							uctCompanyThirdPartyCVIdentifier.SetupControl(companyID);
							ThirdParties companyThirdPartyCVIdentifiers = CompanyManager.GetCompanyThirdPartyCVIdentifiers(companyID);
							uctCompanyThirdPartyCVIdentifier.ThirdPartiesCVIdentifiers = companyThirdPartyCVIdentifiers;

							uctCompanyThirdPartyMappingDetails.SetupControl(companyID);
							ThirdPartyFunds companyThirdPartyFunds = CompanyManager.GetCompanyThirdPartyMappingDetails(companyID);
							uctCompanyThirdPartyMappingDetails.CompanyThirdPartyFunds = companyThirdPartyFunds;

							uctCompanyThirdPartyCommissionRules.SetupControl(companyID);
							ThirdPartyFundCommissionRules companyThirdPartyFundCommissionRules = CompanyManager.GetCompanyThirdPartyFundCommissionRules(companyID);
							uctCompanyThirdPartyCommissionRules.CompanyThirdPartyFundCommissionRules = companyThirdPartyFundCommissionRules;

							break;

						//Showing all the details pertaining to the selected client from the database..
						case NodeType.Clients:
							int clientID = nodeDetails.NodeID;
							NodeDetails nodeDetailsParentClient = (NodeDetails)trvCompanyMaster.SelectedNode.Parent.Parent.Tag;			
							int CompanyIDForClient = nodeDetailsParentClient.NodeID;
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[3];
							
							//Nirvana.Admin.BLL.CompanyClient companyClient = CompanyManager.GetCompanyClient(CompanyIDForClient, clientID);
							//uctClientCompanyControl.CompanyClient = companyClient;
							uctClientCompanyControl.SetupControl(CompanyIDForClient, clientID);
							
							//CompanyClientClearer companyClientClearer = CompanyClientClearerManager.GetCompanyClientClearer(nodeDetails.NodeID);
							//uctClientClearer.CompanyClientClearer = companyClientClearer;
							uctClientClearer.SetupControl(nodeDetails.NodeID);
							
							
							//Passing the clientID to the user control insances having the CompanyClientID 
							//property which fetches and shows the details in the form.
							//uctGridClientFunds1.CompanyClientID = clientID;
							uctGridClientFunds1.SetupControl(clientID);
							
							//uctClientTradersDetail.CompanyClientID = clientID;
							uctClientTradersDetail.SetupControl(clientID);

							//uctClientTradingAccounts.CompanyClientID = clientID ;
							//uctClientTradingAccounts.CompanyID=nodeDetails.CompanyID;
							//uctClientTradingAccounts.CompanyClientID = clientID;
							uctClientTradingAccounts.SetUp(nodeDetails.CompanyID, clientID);
							
							//uctGridClientFix.client=CliuctGridClientFix.SetUp();

							//ClientFix Area
							//uctGridClientFix.ClientID=clientID;
							uctGridClientFix.SetUp(clientID);
							//ClientFixManager.GetClientIdentifiers(clientID);							
							
							//uctClientPermission.CompanyID=nodeDetails.CompanyID;
							//uctClientPermission.ClientID =nodeDetails.NodeID;
							uctClientPermission.Setup(nodeDetails.CompanyID, nodeDetails.NodeID);
							
							break;

						//Showing all the details pertaining to the selected counterparty from the database..
						case NodeType.CounterParties:
//							GridCompanyCounterPartiesFundLevelTags passCompanyCounterPartiesFundLevelTags = new GridCompanyCounterPartiesFundLevelTags();
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[1];
							int counterPartyID = nodeDetails.NodeID;
							int companyIDCompanyLevelTag = nodeDetails.CompanyID;
							
							//Passing the companyID to the user control insances having the CompanyID 
							//property which fetches and shows the CounterPartyVenue permitted for the selected company.
							//uctGridCompanyCountePartiesCompanyLevelTags.CompanyID = nodeDetails.CompanyID;
							
							//Passing the counterPartyID to the user control insances having the CompanyCounterPartyID 
							//property which fetches and shows the details in the form.
							//uctGridCompanyCountePartiesCompanyLevelTags.CompanyCounterPartyID = counterPartyID;

							uctGridCompanyCountePartiesCompanyLevelTags.SetupControl(int.MinValue, counterPartyID, companyIDCompanyLevelTag);

							
							break;

						case NodeType.CounterPartyVenues:
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[1];
							NodeDetails nodeDetailCounterParty = (NodeDetails)trvCompanyMaster.SelectedNode.Parent.Parent.Tag;			

							int companyCounterPartyVenueID = nodeDetails.NodeID;
							//MessageBox.Show("Company Counter Party Venue ID: " + companyCounterPartyVenueID);
							companyIDCompanyLevelTag = nodeDetails.CompanyID;
							
							//Passing the companyID to the user control insances having the CompanyID 
							//property which fetches and shows the CounterPartyVenue permitted for the selected company.
							//uctGridCompanyCountePartiesCompanyLevelTags.CompanyID = nodeDetails.CompanyID;
							uctGridCompanyCountePartiesCompanyLevelTags.SetupControl(companyCounterPartyVenueID, companyIDCompanyLevelTag, nodeDetails.CompanyID);
							
							//Passing the counterPartyID to the user control insances having the CompanyCounterPartyID 
							//property which fetches and shows the details in the form.
							if(nodeDetails.NodeID == int.MinValue)
							{
								NodeDetails nodeDetailCounterPartyVenue = (NodeDetails)trvCompanyMaster.SelectedNode.Parent.Tag;
								//uctGridCompanyCountePartiesCompanyLevelTags.CompanyCounterPartyID = nodeDetailCounterPartyVenue.NodeID;
								uctGridCompanyCountePartiesCompanyLevelTags.SetupControl(nodeDetails.NodeID, nodeDetailCounterPartyVenue.NodeID, nodeDetails.CompanyID);
							}
							else
							{
								//uctGridCompanyCountePartiesCompanyLevelTags.CompanyCounterPartyID = nodeDetailCounterParty.NodeID;
								uctGridCompanyCountePartiesCompanyLevelTags.SetupControl(companyCounterPartyVenueID, nodeDetailCounterParty.NodeID, nodeDetails.CompanyID);
							}

							//uctGridCompanyCountePartiesCompanyLevelTags.CompanyCounterPartyVenueID = companyCounterPartyVenueID;
							uctGridCompanyCountePartiesCompanyLevelTags.SetupControl(companyCounterPartyVenueID, nodeDetailCounterParty.NodeID, nodeDetails.CompanyID);

							//							uctGridCompanyCounterPartiesFundLevelTags.CompanyCounterPartyID = counterPartyID;
							
							//							uctGridCompanyCounterPartiesUserLevelTags.CompanyCounterPartyID = counterPartyID;
							
							break;


					
						//Showing all the details pertaining to the selected user from the database..
						case NodeType.Users:
							int userID = nodeDetails.NodeID;
							NodeDetails nodeDetailsParentUser = (NodeDetails)trvCompanyMaster.SelectedNode.Parent.Parent.Tag;			
							int CompanyIDForUser = nodeDetailsParentUser.NodeID;
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[2];
							//Nirvana.Admin.BLL.User user = UserManager.GetCompanyUserBoth(CompanyIDForUser, userID);
							
							//Passing the user object to the user control insances having the User 
							//property which fetches and shows the details in the form.
							
							//uctCompanyUserDetail.User = user;
							uctCompanyUserDetail.SetupControl(CompanyIDForUser, userID);
					
							//Calling the BindData method in the user control which shows the details of the 
							//selected user
							//uctCompanyUserPermissions.BindData(nodeDetails.CompanyID, userID);							
							uctCompanyUserPermissions.SetupControl(CompanyIDForUser, userID);
						
							break;

							//Showing the Third Party Tab.
						case NodeType.ThirdParty:
							tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[4];

							uctCompanyThirdPartyFileFormats.SetupControl(nodeDetails.CompanyID);
							Funds thirdPartiesFileFormatFunds = CompanyManager.GetCompanyThirdPartyFileFormats(nodeDetails.CompanyID);
							uctCompanyThirdPartyFileFormats.FileFormatsFunds = thirdPartiesFileFormatFunds;

							uctCompanyThirdPartyCVIdentifier.SetupControl(nodeDetails.CompanyID);
							ThirdParties companyThirdPartiesCVIdentifiers = CompanyManager.GetCompanyThirdPartyCVIdentifiers(nodeDetails.CompanyID);
							uctCompanyThirdPartyCVIdentifier.ThirdPartiesCVIdentifiers = companyThirdPartiesCVIdentifiers;

							uctCompanyThirdPartyMappingDetails.SetupControl(nodeDetails.CompanyID);
							ThirdPartyFunds companyThirdPartiesFunds = CompanyManager.GetCompanyThirdPartyMappingDetails(nodeDetails.CompanyID);
							uctCompanyThirdPartyMappingDetails.CompanyThirdPartyFunds = companyThirdPartiesFunds;

							uctCompanyThirdPartyCommissionRules.SetupControl(nodeDetails.CompanyID);
							ThirdPartyFundCommissionRules companyThirdPartyCommissionRules = CompanyManager.GetCompanyThirdPartyFundCommissionRules(nodeDetails.CompanyID);
							uctCompanyThirdPartyCommissionRules.CompanyThirdPartyFundCommissionRules = companyThirdPartyCommissionRules;
							
							break;

					}
				}
				//If the user doesnt have the permision to maintain companies and selected node doesnt have any 
				//id, then the all the company tabs are disabled.
				Preferences preferences = Preferences.Instance;	
				bool chkMaintainCompanies = preferences.Maintain_Companies;
				bool chkSetUpCompanies = preferences.Set_Up_Company;
				if((chkMaintainCompanies == true) && (chkSetUpCompanies == false))
				{
					if(nodeDetails.NodeID < 0 && nodeDetails.Type == NodeType.Company)
					{
						DisableCompanyTabs();
					}
					else
					{
						EnableCompanyTabs();
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

				LogEntry logEntry = new LogEntry("trvCompanyMaster_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "trvCompanyMaster_AfterSelect"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// This method saves the company user form related informations on its click event in the form. This
		/// includes user's basic details and the permissions.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSaveCompanyUser_Click(object sender, System.EventArgs e)
		{
			try
			{
				//TODO: Add transaction.
				NodeDetails selNodeDetails = new NodeDetails();
				int selUserID = int.MinValue;
				if(trvCompanyMaster.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails) trvCompanyMaster.SelectedNode.Tag;
					
					//Save User details.
					//-----------------------------------------------------------------------------
//					uctCompanyUserDetail.ParentStatusBar = stbCompany;
					User user = uctCompanyUserDetail.User;
					if(user != null)
					{
						user.UserID = nodeDetails.NodeID;

						int companyid = nodeDetails.CompanyID;
						int userID = UserManager.SaveCompanyUser(companyid, user);
						selUserID = userID;
						if(userID > 0 )
						{
							//Save Company User permissions
							uctCompanyUserPermissions.Save(userID, nodeDetails.CompanyID);						
						}
						BindTree();		
						int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
						NodeDetails selectNodeDetails = new NodeDetails(NodeType.Users, selUserID, selCompanyID);
						SelectTreeNode(selectNodeDetails);
					}
					else
					{
						tbcCompanyUser.SelectedTab = tbcCompanyUser.Tabs[0];
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

				LogEntry logEntry = new LogEntry("btnSaveCompanyUser_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSaveCompanyUser_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void CompanyDetails(Nirvana.Admin.BLL.Company company, CounterParties counterParties, CounterPartyVenues counterPartyVenues, Modules modules, ThirdParties thirdParties, AUECs companyAUECs)
		{
			//Deselecting the company permissions list boxes before selecting them again.
			for ( int j=0; j< checkedlstApplicationComponents.Items.Count ; j++)
			{
				checkedlstApplicationComponents.SetItemChecked(j,false);
			}
			for ( int j=0; j< checkedlstAssetsUnderlyings.Items.Count ; j++)
			{
				checkedlstAssetsUnderlyings.SetItemChecked(j,false);
			}
			for ( int j=0; j< checkedlstCounterParties.Items.Count ; j++)
			{
				checkedlstCounterParties.SetItemChecked(j,false);
			}
			for ( int j=0; j< checkedlstThirdPartyComponents.Items.Count ; j++)
			{
				checkedlstThirdPartyComponents.SetItemChecked(j,false);
			}
			//Showing Company details 
			
			if(company != null)
			{
				
				//Details
				txtCompanyName.Text = company.Name;
				txtAddress1.Text = company.Address1;
				txtAddress2.Text = company.Address2;
				cmbCompanyTYpe.Value = int.Parse(company.CompanyTypeID.ToString());
				txtTelephone.Text = company.Telephone;
				txtFax.Text = company.Fax;

				//PrimaryContact
				txtPC1FirstName.Text = company.PrimaryContactFirstName;
				txtPC1LastName.Text = company.PrimaryContactLastName;
				txtPC1Title.Text  = company.PrimaryContactTitle;
				txtPC1Email.Text = company.PrimaryContactEMail;
				txtPC1Telephone.Text = company.PrimaryContactTelephone;
				txtPC1Cell.Text = company.PrimaryContactCell;

				//SecondryContact
				txtPC2FirstName.Text = company.SecondaryContactFirstName;
				txtPC2LastName.Text = company.SecondaryContactLastName;
				txtPC2Title.Text  = company.SecondaryContactTitle;
				txtPC2Email.Text = company.SecondaryContactEMail;
				txtPC2Telephone.Text = company.SecondaryContactTelephone;
				txtPC2Cell.Text = company.SecondaryContactCell;

				//TechnologyContact
				txtTCFirstName.Text = company.TechnologyContactFirstName;
				txtTCLastName.Text = company.TechnologyContactLastName;
				txtTCTitle.Text = company.TechnologyContactTitle;
				txtTCEmail.Text = company.TechnologyContactEMail;
				txtTCTelephone.Text = company.TechnologyContactTelephone;
				txtTCCell.Text = company.TechnologyContactCell;
				

				//Deselecting the company permissions list boxes before selecting them again.		
//				lstCounterParties.SelectedIndex = -1;
//				checkedlstCounterParties.ClearSelected();
//				foreach(CounterParty counterParty in counterParties)
//				{
//					lstCounterParties.SelectedValue = counterParty.CounterPartyID;	
//					for (int j=0; j < checkedlstCounterParties.Items.Count ; j++)
//					{
//						if ((int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstCounterParties.Items[j]))).Row)).ItemArray[1].ToString())== counterParty.CounterPartyID) &&
//							(int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstCounterParties.Items[j]))).Row)).ItemArray[2].ToString())== int.MinValue))
//						{
//							checkedlstCounterParties.SetItemChecked(j,true);
//						}
//					}
//				}				

				System.Data.DataTable dt = new System.Data.DataTable();
//				if (lstCounterParties.Items.Count > 0)
//				{
//					dt = (System.Data.DataTable) lstCounterParties.DataSource;
//				}
				if (checkedlstCounterParties.Items.Count > 0)
				{
					dt = (System.Data.DataTable)checkedlstCounterParties.DataSource;
				}


				int rowIndex = 0;
				counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(company.CompanyID);
				foreach(CounterPartyVenue counterPartyVenue in counterPartyVenues)
				{
					rowIndex = 0;
					foreach(System.Data.DataRow row in dt.Rows)
					{
						if((int.Parse(row["CounterPartyID"].ToString()) == counterPartyVenue.CounterPartyID && int.Parse(row["VenueID"].ToString()) == counterPartyVenue.VenueID) || (int.Parse(row["CounterPartyID"].ToString()) == counterPartyVenue.CounterPartyID && int.Parse(row["VenueID"].ToString()) == int.MinValue))
						{
//							lstCounterParties.SelectedIndex = rowIndex;
							checkedlstCounterParties.SetItemChecked(rowIndex, true);
						}
						rowIndex++;
					}
					
				}				
				
//				lstApplicationComponents.SelectedIndex = -1;
				checkedlstApplicationComponents.ClearSelected();
				foreach(Nirvana.Admin.BLL.Module module  in modules)
				{
//					lstApplicationComponents.SelectedValue = module.ModuleID;
					for (int j=0; j < checkedlstApplicationComponents.Items.Count ; j++)
					{
						if (((Nirvana.Admin.BLL.Module)checkedlstApplicationComponents.Items[j]).ModuleID == module.ModuleID)
						{
							checkedlstApplicationComponents.SetItemChecked(j,true);
						}
					}
				}
							
				
				
//				System.Data.DataTable dt = new System.Data.DataTable();
//				if (checkedlstThirdPartyComponents.Items.Count > 0)
//				{
//					dt = (System.Data.DataTable)checkedlstThirdPartyComponents.DataSource;
//				}			

//				bool checkThirdParty = false; //A varriable declared to used to check whether a thirdparty is to be selected or not depending upon the availablity of thirdparty.
//				lstThirdPartyComponents.SelectedIndex = -1;
				checkedlstThirdPartyComponents.ClearSelected();
				foreach(ThirdParty thirdParty in thirdParties)
				{
//					lstThirdPartyComponents.SelectedValue = thirdParty.ThirdPartyID;
					for (int j=0; j< checkedlstThirdPartyComponents.Items.Count ; j++)
					{
						//if (((ThirdParty)checkedlstThirdPartyComponents.Items[j]).ThirdPartyID == thirdParty.ThirdPartyID)
						if (int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstThirdPartyComponents.Items[j]))).Row)).ItemArray[2].ToString()) == int.Parse(thirdParty.ThirdPartyID.ToString()))
						{
							checkedlstThirdPartyComponents.SetItemChecked(j,true);
						}
						if (int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstThirdPartyComponents.Items[j]))).Row)).ItemArray[1].ToString()) == int.Parse(thirdParty.ThirdPartyTypeID.ToString()))
						{
							checkedlstThirdPartyComponents.SetItemChecked(j,true);
						}
						
					}
				}
//				lstAssetsUnderlyings.SelectedIndex = -1;
//				checkedlstAssetsUnderlyings.ClearSelected();
//				foreach(UnderLying underLying in underLyings)
//				{
//					lstAssetsUnderlyings.SelectedValue = underLying.UnderlyingID;
//					for (int j = 0; j < checkedlstAssetsUnderlyings.Items.Count; j++)
//					{
//					if (int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAssetsUnderlyings.Items[j]))).Row)).ItemArray[1].ToString()) == underLying.UnderlyingID)
//						{
//							checkedlstAssetsUnderlyings.SetItemChecked(j,true);
//						}
//					}
//				}

				checkedlstAssetsUnderlyings.SelectedIndex = -1;
				checkedlstAssetsUnderlyings.ClearSelected();
				foreach(Nirvana.Admin.BLL.AUEC companyAUEC in companyAUECs)
				{
					checkedlstAssetsUnderlyings.SelectedValue = companyAUEC.AUECID;
					for (int j=0; j< checkedlstAssetsUnderlyings.Items.Count ; j++)
					{
						if (int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAssetsUnderlyings.Items[j]))).Row)).ItemArray[1].ToString()) == int.Parse(companyAUEC.AUECID.ToString()))
						{
							checkedlstAssetsUnderlyings.SetItemChecked(j,true);
						}
					}
				}
			}			
		}
		
		/// <summary>
		/// This method saves the company client form related informations on its click event in the form. This
		/// includes client's basic details, funds and the traders.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCompanyClientsSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				int selCompanyID = int.MinValue;
				if (trvCompanyMaster.SelectedNode == null) 
				{
					//stbCompany.Text = "Please select any Client Company to be saved";
				}
				else
				{   
					int companyClientID = int.MinValue;
					int result = int.MinValue;
					int selCompanyClient = int.MinValue;
					NodeDetails nodeDetails = (NodeDetails) trvCompanyMaster.SelectedNode.Tag;
					
//					uctClientCompanyControl.ParentStatusBar = stbCompany;
					int companyID = nodeDetails.CompanyID;
					selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
					
					//Getting the client details info from the user control and saving them to the database.
					
					CompanyClient companyClient = uctClientCompanyControl.CompanyClient;
					
					if(companyClient != null)
					{
						companyClient.CompanyClientID = nodeDetails.NodeID;	
					}

					ClientFix clientFix= uctGridClientFix.clientFix;
				    Identifiers  identifiers= uctGridClientFix.clientIdentifiers;

					Nirvana.Admin.BLL.ClientFunds clientCompanyFunds = uctGridClientFunds1.CurrentCompanyClientFunds;
					
					Nirvana.Admin.BLL.Traders traders = uctClientTradersDetail.CurrentTraders;
					
					CompanyClientClearer companyClientClearer= uctClientClearer.CompanyClientClearer;
					if(companyClientClearer !=null)
					companyClientClearer.CompanyClientID = nodeDetails.NodeID;

					//uctClientTradingAccounts.CompanyID=nodeDetails.CompanyID;
					//uctClientTradingAccounts.SetUp(nodeDetails.CompanyID, nodeDetails.NodeID);
					
					if(companyClient == null)
					{
					
						tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[0];
					}
					
					else if (clientFix==null )
					{
							
						
						tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[5];
						//result = int.MinValue;
					}
					else if (identifiers==null )
					{
							

						tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[5];
						//result = int.MinValue;
					}
						
					else if (clientCompanyFunds==null)	
					{
							
						
						tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[1];
						//result = int.MinValue;
					}
					
							
					else if(traders==null)
					{

						tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[2];
					}

					else if(companyClientClearer==null)
					{

						tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[4];
					}
					else if(uctClientTradingAccounts.CompanyClientTradingAccounts.Count==0)
					{
//						stbCompany.Text = "Please fill the client details";
						tbcCompanyClients.SelectedTab = tbcCompanyClients.Tabs[3];
					}
					
				
						
						//save to database
					
					else
					{		
						
								
						companyClientID = CompanyManager.SaveCompanyClient(companyID, companyClient);
						companyClientClearer.CompanyClientID =companyClientID;
						ClientFundManager.SaveClientFund(clientCompanyFunds, companyClientID);
						ClientFixManager.SaveCompanyClientFix(clientFix, companyClientID);
						TraderManager.SaveTrader(companyClientID, traders);
						//Gettng Traders
						Traders savedtraders = TraderManager.GetTraders(companyClientID);
						CompanyClientTradingAccounts companyClientTradingAccounts=uctClientTradingAccounts.CompanyClientTradingAccounts;
						foreach(CompanyClientTradingAccount companyClientTradingAccount in companyClientTradingAccounts)
						{
							foreach(Trader trader in savedtraders)
							{
								if(companyClientTradingAccount.ClientTraderShortName==trader.ShortName  )
								{
									companyClientTradingAccount.ClientTraderID=trader.TraderID ;
									break;
								}
							}
						}

						
						CompanyClientTradingAccountManager.SaveCompanyClientTradingAccount(companyClientTradingAccounts, companyClientID);
						CompanyManager.SaveClientAUECs(companyClientID,uctClientPermission.ClientAUECS);						
						ClientFixManager.SaveCompanyIdentifiers(identifiers,companyClientID);
						CompanyClientClearerManager.SaveCompanyClientClearer(companyClientClearer);					
						BindTree();											
						NodeDetails selectNodeDetails = new NodeDetails(NodeType.Clients, selCompanyClient, selCompanyID);
						SelectTreeNode(selectNodeDetails);
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

				LogEntry logEntry = new LogEntry("btnCompanyClientsSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnCompanyClientsSave_Click"); 
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
			int compIndex = int.MinValue;
			//Selects the node based on the nodedetail type ie. company/user/counterparty/client.
			switch(nodeDetails.Type)
			{
				case NodeType.Company:
					if (trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes.Count > 0)
					{
						foreach(TreeNode node in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes)
						{
							if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
							{
								trvCompanyMaster.SelectedNode = node;
								break;
							}
						}
					}
					break;

				case NodeType.Users:
					compIndex = -1;
					foreach(TreeNode node in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes)
					{
						compIndex++;
						if(((NodeDetails) node.Tag).NodeID == nodeDetails.CompanyID)
						{
							if (trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[C_TAB_USERDETAIL].Nodes.Count > 0)
							{
								foreach(TreeNode userNode in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[C_TAB_USERDETAIL].Nodes)
								{
									if(((NodeDetails) userNode.Tag).NodeID == nodeDetails.NodeID)
									{
										trvCompanyMaster.SelectedNode = userNode;
										break;
									}
								}
							}
							else
							{
								trvCompanyMaster.SelectedNode = node;
							}
						}
					}
					break;
			
				case NodeType.CounterParties:
					compIndex = -1;
					foreach(TreeNode node in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes)
					{
						compIndex++;
						if(((NodeDetails) node.Tag).NodeID == nodeDetails.CompanyID)
						{
							if (trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[C_TAB_COUNTERPARTYCOMPANYLEVELTAG].Nodes.Count > 0)
							{
								foreach(TreeNode counterPartyNode in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[C_TAB_COUNTERPARTYCOMPANYLEVELTAG].Nodes)
								{
									if(((NodeDetails) counterPartyNode.Tag).NodeID == nodeDetails.NodeID)
									{
										trvCompanyMaster.SelectedNode = counterPartyNode;
										break;
									}
								}
							}
							else
							{
								trvCompanyMaster.SelectedNode = node;
							}
						}
					}
					break;

				case NodeType.Clients:
					compIndex = -1;
					foreach(TreeNode node in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes)
					{
						compIndex++;
						if(((NodeDetails) node.Tag).NodeID == nodeDetails.CompanyID)
						{
							if (trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[2].Nodes.Count > 0)
							{
								foreach(TreeNode clientsNode in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[2].Nodes)
								{
									if(((NodeDetails) clientsNode.Tag).NodeID == nodeDetails.NodeID)
									{
										trvCompanyMaster.SelectedNode = clientsNode;
										break;
									}
								}
							}
							else
							{
								trvCompanyMaster.SelectedNode = node;
							}
						}
					}
					break;

				case NodeType.ThirdParty:
					compIndex = -1;
					foreach(TreeNode node in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes)
					{
						compIndex++;
						if(((NodeDetails) node.Tag).NodeID == nodeDetails.CompanyID)
						{
							if (trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[3].Nodes.Count > 0)
							{
								foreach(TreeNode thirdPartyNode in trvCompanyMaster.Nodes[C_TAB_COMPANYSETUP].Nodes[compIndex].Nodes[3].Nodes)
								{
									if(((NodeDetails) thirdPartyNode.Tag).NodeID == nodeDetails.NodeID)
									{
										trvCompanyMaster.SelectedNode = thirdPartyNode;
										break;
									}
								}
							}
							else
							{
								trvCompanyMaster.SelectedNode = node;
							}
						}
					}
					break;
			}
		}
		
		/// <summary>
		/// This method closes the application when clicked the close button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCloseCompanyUser_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
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

				LogEntry logEntry = new LogEntry("btnCloseCompanyUser_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnCloseCompanyUser_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// This method closes the application when clicked the close button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCloseCompanyClients_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		#region NodeDetails

		//Creating class NodeDetail to be used for the purpose of tree giving it some methods & properties.
		class NodeDetails
		{
			private NodeType _type = NodeType.Company;
			private int _nodeID = int.MinValue;
			private int _companyID = int.MinValue;			

			public NodeDetails()
			{
			}

			public NodeDetails(NodeType type, int nodeID)
			{
				_type = type;
				_nodeID = nodeID;
			}

			public NodeDetails(NodeType type, int nodeID, int companyID)
			{
				_type = type;
				_nodeID = nodeID;
				_companyID = companyID; 
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
			public int CompanyID
			{
				get{return _companyID;}
				set{_companyID = value;}
			}
		}

		//Creating enumeration to be used to distinguish tree nodetype on the basis of Company/Users/CounterParties/Clients
		enum NodeType
		{
			Company = 1,
			Users = 2,
			CounterParties = 3,
			Clients = 4,
			CounterPartyVenues = 5,
			ThirdParty = 6
		}

		#endregion

		#region Focus Colors
		private void txtCompanyName_GotFocus(object sender, System.EventArgs e)
		{
			txtCompanyName.BackColor = Color.LemonChiffon;
		}
		private void txtCompanyName_LostFocus(object sender, System.EventArgs e)
		{
			txtCompanyName.BackColor = Color.White;
		}
		private void txtAddress1_GotFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.LemonChiffon;
		}
		private void txtAddress1_LostFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.White;
		}
		private void txtAddress2_GotFocus(object sender, System.EventArgs e)
		{
			txtAddress2.BackColor = Color.LemonChiffon;
		}
		private void txtAddress2_LostFocus(object sender, System.EventArgs e)
		{
			txtAddress2.BackColor = Color.White;
		}
		private void cmbCompanyTYpe_GotFocus(object sender, System.EventArgs e)
		{
			cmbCompanyTYpe.Appearance.BackColor = Color.LemonChiffon;
		}
		private void cmbCompanyTYpe_LostFocus(object sender, System.EventArgs e)
		{
			cmbCompanyTYpe.Appearance.BackColor = Color.White;
		}
		private void txtTelephone_GotFocus(object sender, System.EventArgs e)
		{
			txtTelephone.BackColor = Color.LemonChiffon;
		}
		private void txtTelephone_LostFocus(object sender, System.EventArgs e)
		{
			txtTelephone.BackColor = Color.White;
		}
		private void txtFax_GotFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.LemonChiffon;
		}
		private void txtFax_LostFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.White;
		}
		private void txtPC1FirstName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1FirstName.BackColor = Color.LemonChiffon;
		}
		private void txtPC1FirstName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1FirstName.BackColor = Color.White;
		}
		private void txtPC1LastName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1LastName.BackColor = Color.LemonChiffon;
		}
		private void txtPC1LastName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1LastName.BackColor = Color.White;
		}
		private void txtPC1Title_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Title.BackColor = Color.LemonChiffon;
		}
		private void txtPC1Title_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Title.BackColor = Color.White;
		}
		private void txtPC1Email_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Email.BackColor = Color.LemonChiffon;
			}
		private void txtPC1Email_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Email.BackColor = Color.White;
		}
		private void txtPC1Telephone_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Telephone.BackColor = Color.LemonChiffon;
		}
		private void txtPC1Telephone_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Telephone.BackColor = Color.White;
		}
		private void txtPC1Cell_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Cell.BackColor = Color.LemonChiffon;
		}
		private void txtPC1Cell_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Cell.BackColor = Color.White;
		}
		private void txtPC2FirstName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2FirstName.BackColor = Color.LemonChiffon;
		}
		private void txtPC2FirstName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2FirstName.BackColor = Color.White;
		}
		private void txtPC2LastName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2LastName.BackColor = Color.LemonChiffon;
		}
		private void txtPC2LastName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2LastName.BackColor = Color.White;
		}
		private void txtPC2Title_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Title.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Title_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Title.BackColor = Color.White;
		}
		private void txtPC2Email_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Email.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Email_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Email.BackColor = Color.White;
		}
		private void txtPC2Telephone_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Telephone.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Telephone_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Telephone.BackColor = Color.White;
		}
		private void txtPC2Cell_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Cell.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Cell_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Cell.BackColor = Color.White;
		}
		private void txtTCFirstName_GotFocus(object sender, System.EventArgs e)
		{
			txtTCFirstName.BackColor = Color.LemonChiffon;
		}
		private void txtTCFirstName_LostFocus(object sender, System.EventArgs e)
		{
			txtTCFirstName.BackColor = Color.White;
		}
		private void txtTCLastName_GotFocus(object sender, System.EventArgs e)
		{
			txtTCLastName.BackColor = Color.LemonChiffon;
		}
		private void txtTCLastName_LostFocus(object sender, System.EventArgs e)
		{
			txtTCLastName.BackColor = Color.White;
		}
		private void txtTCTitle_GotFocus(object sender, System.EventArgs e)
		{
			txtTCTitle.BackColor = Color.LemonChiffon;
		}
		private void txtTCTitle_LostFocus(object sender, System.EventArgs e)
		{
			txtTCTitle.BackColor = Color.White;
		}
		private void txtTCEmail_GotFocus(object sender, System.EventArgs e)
		{
			txtTCEmail.BackColor = Color.LemonChiffon;
		}
		private void txtTCEmail_LostFocus(object sender, System.EventArgs e)
		{
			txtTCEmail.BackColor = Color.White;
		}
		private void txtTCTelephone_GotFocus(object sender, System.EventArgs e)
		{
			txtTCTelephone.BackColor = Color.LemonChiffon;
		}
		private void txtTCTelephone_LostFocus(object sender, System.EventArgs e)
		{
			txtTCTelephone.BackColor = Color.White;
		}
		private void txtTCCell_GotFocus(object sender, System.EventArgs e)
		{
			txtTCCell.BackColor = Color.LemonChiffon;
		}
		private void txtTCCell_LostFocus(object sender, System.EventArgs e)
		{
			txtTCCell.BackColor = Color.White;
		}
	
	
		#endregion

		/// <summary>
		/// This method closes the application when clicked the close button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCompanyCouterPartiesClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Saves the Company Counter Parties having the tags ie. company level, fund level & user level. It
		/// saves the all the info taken together from the tags at a time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCompanyCouterPartiesSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				int resultCompanyLevelTag = int.MinValue;
				int selCompanyLevelTag = int.MinValue;
				NodeDetails selNodeDetails = new NodeDetails();
				
				int selCounterPartyVenueDetails = int.MinValue;
				NodeDetails counterNodeDetails = (NodeDetails) trvCompanyMaster.SelectedNode.Tag;
				selCounterPartyVenueDetails = counterNodeDetails.NodeID;
				if(trvCompanyMaster.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails) trvCompanyMaster.SelectedNode.Tag;
					
					//Saving the company level tag info to the database by getting the data from the user control's 
					//CurrentCompanyCounterPartyVenueDetails property.
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetailsCompany = uctGridCompanyCountePartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails;
					if(uctGridCompanyCountePartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails != null)
					{
						resultCompanyLevelTag = CompanyManager.SaveCompanyCounterPartyCompanyLevelTags(companyCounterPartyVenueDetailsCompany);
						int resultCompanyCVIdentifier = CompanyManager.SaveCompanyCounterPartyVenueIdentifier(companyCounterPartyVenueDetailsCompany);
						selCompanyLevelTag = resultCompanyLevelTag;
					}


					BindTree();
					int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
					NodeDetails selectNodeDetails = new NodeDetails(NodeType.CounterParties, selCounterPartyVenueDetails, selCompanyID);
					//Selecting the newly added or updated node in the tree.
					SelectTreeNode(selectNodeDetails);
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

				LogEntry logEntry = new LogEntry("btnCompanyCouterPartiesSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnCompanyCouterPartiesSave_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}
		
		#region Highlight Selected Tab
		//To highlight and show the currently selected node in a different color.
		private void tbcCompanyMaster_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tbcCompanyMaster.SelectedTab.Index)
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

			string tabName = this.tbcCompanyMaster.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcCompanyMaster.SelectedTab.Index)
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

		private void tbcCompanySetup_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tbcCompanySetup.SelectedTab.Index)
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

			string tabName = this.tbcCompanySetup.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcCompanySetup.SelectedTab.Index)
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
		private void tbcCompanyUser_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tbcCompanyUser.SelectedTab.Index)
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

			string tabName = this.tbcCompanyUser.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcCompanyUser.SelectedTab.Index)
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
		private void tbcCompanyClients_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tbcCompanyClients.SelectedTab.Index)
			{
				f = new Font(e.Font, FontStyle.Regular);
				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);				
				foreBrush = Brushes.Black;
			}
			else
			{
				f = e.Font;
				backBrush = new SolidBrush(e.BackColor); 
				foreBrush = new SolidBrush(e.ForeColor);
			}

			string tabName = this.tbcCompanyClients.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcCompanyClients.SelectedTab.Index)
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
		private void tabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
            			
			if(e.Index == this.tbcCompanyCounterParties.SelectedTab.Index)
			{
				f = new Font(e.Font, FontStyle.Regular);
				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
				foreBrush = Brushes.Black;
			}
			else
			{
				f = e.Font;
				backBrush = new SolidBrush(e.BackColor); 
				foreBrush = new SolidBrush(e.ForeColor);
			}

			string tabName = this.tbcCompanyCounterParties.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcCompanyCounterParties.SelectedTab.Index)
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

		private void tbcCompanyThirdParty_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tbcCompanyThirdParty.SelectedTab.Index)
			{
				f = new Font(e.Font, FontStyle.Regular);
				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);				
				foreBrush = Brushes.Black;
			}
			else
			{
				f = e.Font;
				backBrush = new SolidBrush(e.BackColor); 
				foreBrush = new SolidBrush(e.ForeColor);
			}

			string tabName = this.tbcCompanyThirdParty.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcCompanyThirdParty.SelectedTab.Index)
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

		#endregion

		private void btnMPIDAdd_Click(object sender, System.EventArgs e)
		{
//			//Currencies currencies = ((Currencies) grdMPID.DataSource);
//			Currencies currencies = new Currencies();
//			currencies.Add(new Currency(int.MinValue, "", ""));
//			grdMPID.DataSource = null;
//			grdMPID.DataSource = currencies;
//			grdMPID.Refresh();
//			MPIDCollection mpids = (MPIDCollection)grdMPID.DataSource;
//			if(mpids.Count > 0)
//			{
//				mpids.Add(new MPID(int.MinValue, int.MinValue, ""));
//				grdMPID.DataSource = mpids;
//				grdMPID.Refresh();
//			}
//			else
//			{
//				//MPIDs mpids = new MPIDs();
//				mpids.AddNew();
//				grdMPID.DataSource = null;
//				grdMPID.DataSource = mpids;
//				grdMPID.Refresh();
//			}
		}

		private void tbpCompanyCompliance_Click(object sender, System.EventArgs e)
		{
		
		}

		private void grdMPID_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
//			MessageBox.Show("hi");
		}

		private void gpbDetails_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void tbcCompanyUser_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			 
		
		}

		private void ultraTabPageControl6_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void checkedlstCounterParties_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void checkedlstThirdPartyComponents_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void cmbCompanyTYpe_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		

		}

		private void tbcCompanyUser_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			//MessageBox.Show("Hi");			
			try
			{
				if(trvCompanyMaster.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Tag;			
					if(nodeDetails == null)
					{
						MessageBox.Show("Select Some node!");
						tbcCompanyMaster.Enabled = false;
					}
					else
					{
						int userID = nodeDetails.NodeID;
						uctCompanyUserPermissions.BindData(nodeDetails.CompanyID, userID);

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

				LogEntry logEntry = new LogEntry("trvCompanyMaster_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "trvCompanyMaster_AfterSelect"); 
				Logger.Write(logEntry); 

				#endregion
			}
		
		}
		
		private void tbcCompanySetup_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			//MessageBox.Show("Hi");			
			try
			{
				if(trvCompanyMaster.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Tag;			
					if(nodeDetails == null)
					{
						MessageBox.Show("Select Some node!");
						tbcCompanyMaster.Enabled = false;
					}
					else
					{
						int companyID = nodeDetails.NodeID;

						Nirvana.Admin.BLL.Company company = CompanyManager.GetCompany(companyID);
						CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(companyID);
						CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);

						Modules modules = ModuleManager.GetCompanyModules(companyID);

						ThirdParties thirdParties = ThirdPartyManager.GetCompanyThirdParties(companyID);

						AUECs companyAUECs = CompanyManager.GetCompanyAUECs(companyID);
						CompanyDetails(company, counterParties, counterPartyVenues, modules, thirdParties, companyAUECs);
											
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

				LogEntry logEntry = new LogEntry("trvCompanyMaster_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "trvCompanyMaster_AfterSelect"); 
				Logger.Write(logEntry); 

				#endregion
			}
		
		}

		private void tbcCompanyMaster_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			try
			{
				if(trvCompanyMaster.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails)trvCompanyMaster.SelectedNode.Tag;
			
					if(nodeDetails.NodeID == int.MinValue && nodeDetails.CompanyID == int.MinValue)
					{
						return;
					}
					if(nodeDetails.NodeID == int.MinValue) //Have to c for the excption here when we click on the tab while selecting the tree in CVDetail.
					{//Either of Users/Counterparties/Clients nodes is selected.
				
						if((nodeDetails.Type != NodeType.Company) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYSETUP])
						{
							if(nodeDetails.Type == NodeType.CounterPartyVenues)
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent;
							}
							else
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent;
							}
						}								
						if((nodeDetails.Type != NodeType.Users) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYUSER])
						{
							if(nodeDetails.Type == NodeType.CounterPartyVenues)
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Nodes[1];
							}
							else
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[1];
							}
						
						}				
						if(nodeDetails.Type == NodeType.CounterPartyVenues)
						{
							trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[0];
						}
						else
						{
							if((nodeDetails.Type != NodeType.CounterParties) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCOUNTERPARTIES])
							{
								if(trvCompanyMaster.SelectedNode.Parent.Nodes[1] != null)
								{
									//if(trvCompanyMaster.SelectedNode.Parent.Nodes[1].IsVisible == true)
									//{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[0];
									//}
								}
							}
						}
						
						
						if((nodeDetails.Type != NodeType.Clients) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCLIENTS])
						{
							if(nodeDetails.Type == NodeType.CounterPartyVenues)
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Nodes[2];
							}
//							else if(nodeDetails.Type == NodeType.CounterPartyVenues && nodeDetails.NodeID != int.MinValue)
//							{
//								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Parent.Nodes[2];
//							}
							else
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[2];
								//trvCompanyMaster.SelectedNode = trvCompanyMaster.Nodes[0].Nodes[2];
							}
						}
				
						if((nodeDetails.Type != NodeType.ThirdParty) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYTHIRDPARTIES])
						{
							if(nodeDetails.Type == NodeType.CounterPartyVenues)
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Nodes[3];
							}
							else
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Nodes[3];
								//trvCompanyMaster.SelectedNode = trvCompanyMaster.Nodes[0].Nodes[2];
							}
						}
					}
					else
					{//Either of Users/Counterparties/Clients child nodes is selected or Company node is selected.
						if(nodeDetails.NodeID == nodeDetails.CompanyID)
						{
							if((nodeDetails.Type != NodeType.Users) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYUSER])
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Nodes[0];
							}				
							if((nodeDetails.Type != NodeType.CounterParties) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCOUNTERPARTIES])
							{
								if(trvCompanyMaster.SelectedNode.Nodes.Count > 0)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Nodes[1];
								}
							}				
							if((nodeDetails.Type != NodeType.Clients) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCLIENTS])
							{
					
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Nodes[2];
							}
							if((nodeDetails.Type != NodeType.ThirdParty) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYTHIRDPARTIES])
							{
								trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Nodes[3];
							}

						}
						else
						{
							if((nodeDetails.Type != NodeType.Company) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYSETUP])
							{
								if(nodeDetails.Type == NodeType.CounterParties)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent;
								}
								else if(nodeDetails.Type == NodeType.CounterPartyVenues)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Parent;
								}
								else
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent;
								}
							}								
							if((nodeDetails.Type != NodeType.Users) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYUSER])
							{
								if(nodeDetails.Type == NodeType.CounterParties)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[1];
								}
								else if(nodeDetails.Type == NodeType.CounterPartyVenues)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Parent.Nodes[1];
								}
								else
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[1];
								}
							}	
			
							if((nodeDetails.Type != NodeType.CounterParties) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCOUNTERPARTIES])
							{
								//trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[1];
							}				
							
							if((nodeDetails.Type != NodeType.Clients) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYCLIENTS])
							{
								if(nodeDetails.Type == NodeType.CounterParties)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[2];
								}
								else if(nodeDetails.Type == NodeType.CounterPartyVenues)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Parent.Nodes[2];
								}
								else
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[2];
								}
							}
							
							if((nodeDetails.Type != NodeType.ThirdParty) &&  tbcCompanyMaster.SelectedTab == tbcCompanyMaster.Tabs[C_TAB_COMPANYTHIRDPARTIES])
							{
								if(nodeDetails.Type == NodeType.CounterParties)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[3];
								}
								else if(nodeDetails.Type == NodeType.CounterPartyVenues)
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Parent.Parent.Nodes[3];
								}
								else
								{
									trvCompanyMaster.SelectedNode = trvCompanyMaster.SelectedNode.Parent.Parent.Nodes[3];
								}
							}
						}
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

				LogEntry logEntry = new LogEntry("tbcCompanyMaster_SelectedIndexChanged", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "tbcCompanyMaster_SelectedIndexChanged"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void companyThirdPartyCommissionRules1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void btnSaveCompanyThirdParty_Click(object sender, System.EventArgs e)
		{			
			try
			{
				int result = int.MinValue;
				Nirvana.Admin.BLL.Funds fileFormatsFunds = new Funds();
				fileFormatsFunds = uctCompanyThirdPartyFileFormats.FileFormatsFunds;
				result = CompanyManager.SaveCompanyThirdPartyFileFormats(fileFormatsFunds);
				if(result == int.MinValue)
				{
					//FileFormats not Saved
				}
				else
				{
					//FileFormats Saved
				}

				Nirvana.Admin.BLL.ThirdParties thirdPartiesCVIdentifiers = new ThirdParties();
				thirdPartiesCVIdentifiers = uctCompanyThirdPartyCVIdentifier.ThirdPartiesCVIdentifiers;
				if(thirdPartiesCVIdentifiers != null)
				{
					result = CompanyManager.SaveCompanyThirdPartyCVIdentifiers(thirdPartiesCVIdentifiers);
					if(result == int.MinValue)
					{
						//CVIdentifiers not Saved
					}
					else
					{
						//CVIdentifiers Saved
					}
				}

				NodeDetails nodeDetails = (NodeDetails) trvCompanyMaster.SelectedNode.Tag;				
				Nirvana.Admin.BLL.ThirdPartyFunds companyThirdPartyFunds = new ThirdPartyFunds();
				companyThirdPartyFunds = uctCompanyThirdPartyMappingDetails.CompanyThirdPartyFunds;
				if(companyThirdPartyFunds != null)
				{
					result = CompanyManager.SaveCompanyThirdMappingDetails(companyThirdPartyFunds);
					if(result == int.MinValue)
					{
						//CVIdentifiers not Saved
					}
					else
					{
						//CVIdentifiers Saved
					}
				}

				Nirvana.Admin.BLL.ThirdPartyFundCommissionRules companyThirdPartyFundCommissionRules = new ThirdPartyFundCommissionRules();
				companyThirdPartyFundCommissionRules = uctCompanyThirdPartyCommissionRules.CompanyThirdPartyFundCommissionRules;
				if(companyThirdPartyFundCommissionRules != null)
				{
					result = CompanyManager.SaveCompanyThirdMappingFundCommissionRules(companyThirdPartyFundCommissionRules);
					if(result == int.MinValue)
					{
						//ThirdPartyFundCommissionRules not Saved
					}
					else
					{
						//ThirdPartyFundCommissionRules Saved
					}
				}
				else
				{
					NodeDetails selectNodeDetails = new NodeDetails(NodeType.ThirdParty, nodeDetails.NodeID, nodeDetails.CompanyID);
					BindTree();
					SelectTreeNode(selectNodeDetails);
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

				LogEntry logEntry = new LogEntry("btnSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSave_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void btnCloseCompanyThirdParty_Click(object sender, System.EventArgs e)
		{			
			try
			{
				this.Close();		
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

		private void ultraTabPageControl5_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}
		private void tbcCompanyClients_SelectedTabChanged(object sender,Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			if(tbcCompanyClients.SelectedTab.Text=="Permissions")
			//uctClientPermission.Setup();
			uctClientPermission.Setup(int.MinValue, int.MinValue);
		}
	}
}
