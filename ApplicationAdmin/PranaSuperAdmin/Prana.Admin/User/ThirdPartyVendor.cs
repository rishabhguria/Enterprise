using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using System.Text;
using System.Text.RegularExpressions;

using Nirvana.Admin.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for thdpartyvendor.
	/// </summary>
	public class ThirdPartyVendor : System.Windows.Forms.Form
	{
		#region Constant Definitions
		//Constants defined by the user.
		private const string FORM_NAME = "ThirdPartyVendor: ";
		const string C_COMBO_SELECT = "- Select -";
		const int C_TAB_USER = 0;
		const int C_TAB_PERMISSION = 1;
		const int C_TAB_VENDOR = 2;

		const int C_TREE_USER = 0;
		const int C_TREE_VENDOR = 1;
		#endregion
		#region Private and Protectd members

		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabThirdPartyVendor;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TreeView trvUserRights;

		#endregion

		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox txtFaxA;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.TextBox txtHomeTeleA;
		private System.Windows.Forms.TextBox txtPagerTeleA;
		private System.Windows.Forms.TextBox txtCellTeleA;
		private System.Windows.Forms.TextBox txtWorkTeleA;
		private System.Windows.Forms.TextBox txtEmailA;
		private System.Windows.Forms.TextBox txtAddress2A;
		private System.Windows.Forms.TextBox txtAddress1A;
		private System.Windows.Forms.TextBox txtPasswordA;
		private System.Windows.Forms.TextBox txtLoginNameA;
		private System.Windows.Forms.TextBox txtTitleA;
		private System.Windows.Forms.TextBox txtShortNameA;
		private System.Windows.Forms.TextBox txtLastNameA;
		private System.Windows.Forms.TextBox txtFirstNameA;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox txtMailingAddressT;
		private System.Windows.Forms.TextBox txtComment;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.TextBox txtFaxTele;
		private System.Windows.Forms.TextBox txtHomeTele;
		private System.Windows.Forms.TextBox txtPagerTele;
		private System.Windows.Forms.TextBox txtCellTele;
		private System.Windows.Forms.TextBox txtWorkTele;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.TextBox txtProductName;
		private System.Windows.Forms.TextBox txtVendorName;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkPermissionCounterParties;
		private System.Windows.Forms.CheckBox chkPermissionMaintainCompanis;
		private System.Windows.Forms.CheckBox chkPermissionCompany;
		private System.Windows.Forms.CheckBox chkPermissionAUEC;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label lblStateTerrirtory;
		private System.Windows.Forms.TextBox txtZip;
		private System.Windows.Forms.Label lblZip;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Label label42;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCountry;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbState;


		//private Infragistics.Win.UltraWinTree.UltraTree trvUserRights;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ThirdPartyVendor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			BindCountries();
			this.tabThirdPartyVendor.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabThirdPartyVendor_SelectedTabChanged);
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateName", 1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 2);
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
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
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ThirdPartyVendor));
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cmbState = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.label42 = new System.Windows.Forms.Label();
			this.label40 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.txtZip = new System.Windows.Forms.TextBox();
			this.lblZip = new System.Windows.Forms.Label();
			this.lblStateTerrirtory = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.label39 = new System.Windows.Forms.Label();
			this.label38 = new System.Windows.Forms.Label();
			this.label37 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.txtFaxA = new System.Windows.Forms.TextBox();
			this.label32 = new System.Windows.Forms.Label();
			this.txtHomeTeleA = new System.Windows.Forms.TextBox();
			this.txtPagerTeleA = new System.Windows.Forms.TextBox();
			this.txtCellTeleA = new System.Windows.Forms.TextBox();
			this.txtWorkTeleA = new System.Windows.Forms.TextBox();
			this.txtEmailA = new System.Windows.Forms.TextBox();
			this.txtAddress2A = new System.Windows.Forms.TextBox();
			this.txtAddress1A = new System.Windows.Forms.TextBox();
			this.txtPasswordA = new System.Windows.Forms.TextBox();
			this.txtLoginNameA = new System.Windows.Forms.TextBox();
			this.txtTitleA = new System.Windows.Forms.TextBox();
			this.txtShortNameA = new System.Windows.Forms.TextBox();
			this.txtLastNameA = new System.Windows.Forms.TextBox();
			this.txtFirstNameA = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.chkPermissionCounterParties = new System.Windows.Forms.CheckBox();
			this.chkPermissionMaintainCompanis = new System.Windows.Forms.CheckBox();
			this.chkPermissionCompany = new System.Windows.Forms.CheckBox();
			this.chkPermissionAUEC = new System.Windows.Forms.CheckBox();
			this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.txtMailingAddressT = new System.Windows.Forms.TextBox();
			this.txtComment = new System.Windows.Forms.TextBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtFaxTele = new System.Windows.Forms.TextBox();
			this.txtHomeTele = new System.Windows.Forms.TextBox();
			this.txtPagerTele = new System.Windows.Forms.TextBox();
			this.txtCellTele = new System.Windows.Forms.TextBox();
			this.txtWorkTele = new System.Windows.Forms.TextBox();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.txtAddress2 = new System.Windows.Forms.TextBox();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.txtProductName = new System.Windows.Forms.TextBox();
			this.txtVendorName = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabThirdPartyVendor = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.btnSave = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.trvUserRights = new System.Windows.Forms.TreeView();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.ultraTabPageControl1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
			this.ultraTabPageControl2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.ultraTabPageControl3.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabThirdPartyVendor)).BeginInit();
			this.tabThirdPartyVendor.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Controls.Add(this.groupBox2);
			this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			this.ultraTabPageControl1.Size = new System.Drawing.Size(346, 421);
			this.ultraTabPageControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl1_Paint);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cmbState);
			this.groupBox2.Controls.Add(this.cmbCountry);
			this.groupBox2.Controls.Add(this.label42);
			this.groupBox2.Controls.Add(this.label40);
			this.groupBox2.Controls.Add(this.label34);
			this.groupBox2.Controls.Add(this.txtZip);
			this.groupBox2.Controls.Add(this.lblZip);
			this.groupBox2.Controls.Add(this.lblStateTerrirtory);
			this.groupBox2.Controls.Add(this.label33);
			this.groupBox2.Controls.Add(this.label41);
			this.groupBox2.Controls.Add(this.label39);
			this.groupBox2.Controls.Add(this.label38);
			this.groupBox2.Controls.Add(this.label37);
			this.groupBox2.Controls.Add(this.label36);
			this.groupBox2.Controls.Add(this.label35);
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.label17);
			this.groupBox2.Controls.Add(this.txtFaxA);
			this.groupBox2.Controls.Add(this.label32);
			this.groupBox2.Controls.Add(this.txtHomeTeleA);
			this.groupBox2.Controls.Add(this.txtPagerTeleA);
			this.groupBox2.Controls.Add(this.txtCellTeleA);
			this.groupBox2.Controls.Add(this.txtWorkTeleA);
			this.groupBox2.Controls.Add(this.txtEmailA);
			this.groupBox2.Controls.Add(this.txtAddress2A);
			this.groupBox2.Controls.Add(this.txtAddress1A);
			this.groupBox2.Controls.Add(this.txtPasswordA);
			this.groupBox2.Controls.Add(this.txtLoginNameA);
			this.groupBox2.Controls.Add(this.txtTitleA);
			this.groupBox2.Controls.Add(this.txtShortNameA);
			this.groupBox2.Controls.Add(this.txtLastNameA);
			this.groupBox2.Controls.Add(this.txtFirstNameA);
			this.groupBox2.Controls.Add(this.label19);
			this.groupBox2.Controls.Add(this.label20);
			this.groupBox2.Controls.Add(this.label21);
			this.groupBox2.Controls.Add(this.label22);
			this.groupBox2.Controls.Add(this.label23);
			this.groupBox2.Controls.Add(this.label24);
			this.groupBox2.Controls.Add(this.label25);
			this.groupBox2.Controls.Add(this.label26);
			this.groupBox2.Controls.Add(this.label27);
			this.groupBox2.Controls.Add(this.label28);
			this.groupBox2.Controls.Add(this.label29);
			this.groupBox2.Controls.Add(this.label30);
			this.groupBox2.Controls.Add(this.label31);
			this.groupBox2.Location = new System.Drawing.Point(2, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(340, 412);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
			// 
			// cmbState
			// 
			this.cmbState.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance1.BackColor = System.Drawing.SystemColors.Window;
			appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbState.DisplayLayout.Appearance = appearance1;
			this.cmbState.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand1.ColHeadersVisible = false;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Hidden = true;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3});
			this.cmbState.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.cmbState.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbState.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance2.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbState.DisplayLayout.GroupByBox.Appearance = appearance2;
			appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbState.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
			this.cmbState.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance4.BackColor2 = System.Drawing.SystemColors.Control;
			appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbState.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
			this.cmbState.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbState.DisplayLayout.MaxRowScrollRegions = 1;
			appearance5.BackColor = System.Drawing.SystemColors.Window;
			appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbState.DisplayLayout.Override.ActiveCellAppearance = appearance5;
			appearance6.BackColor = System.Drawing.SystemColors.Highlight;
			appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbState.DisplayLayout.Override.ActiveRowAppearance = appearance6;
			this.cmbState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance7.BackColor = System.Drawing.SystemColors.Window;
			this.cmbState.DisplayLayout.Override.CardAreaAppearance = appearance7;
			appearance8.BorderColor = System.Drawing.Color.Silver;
			appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbState.DisplayLayout.Override.CellAppearance = appearance8;
			this.cmbState.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbState.DisplayLayout.Override.CellPadding = 0;
			appearance9.BackColor = System.Drawing.SystemColors.Control;
			appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance9.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbState.DisplayLayout.Override.GroupByRowAppearance = appearance9;
			appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbState.DisplayLayout.Override.HeaderAppearance = appearance10;
			this.cmbState.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbState.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance11.BackColor = System.Drawing.SystemColors.Window;
			appearance11.BorderColor = System.Drawing.Color.Silver;
			this.cmbState.DisplayLayout.Override.RowAppearance = appearance11;
			this.cmbState.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbState.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
			this.cmbState.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbState.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbState.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbState.DisplayMember = "";
			this.cmbState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbState.DropDownWidth = 0;
			this.cmbState.FlatMode = true;
			this.cmbState.Location = new System.Drawing.Point(120, 222);
			this.cmbState.Name = "cmbState";
			this.cmbState.Size = new System.Drawing.Size(200, 20);
			this.cmbState.TabIndex = 167;
			this.cmbState.ValueMember = "";
			// 
			// cmbCountry
			// 
			this.cmbCountry.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance13.BackColor = System.Drawing.SystemColors.Window;
			appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCountry.DisplayLayout.Appearance = appearance13;
			this.cmbCountry.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand2.ColHeadersVisible = false;
			ultraGridColumn4.Header.VisiblePosition = 0;
			ultraGridColumn4.Hidden = true;
			ultraGridColumn5.Header.VisiblePosition = 1;
			ultraGridBand2.Columns.AddRange(new object[] {
															 ultraGridColumn4,
															 ultraGridColumn5});
			this.cmbCountry.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
			this.cmbCountry.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCountry.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance14.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCountry.DisplayLayout.GroupByBox.Appearance = appearance14;
			appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCountry.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
			this.cmbCountry.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance16.BackColor2 = System.Drawing.SystemColors.Control;
			appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCountry.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
			this.cmbCountry.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCountry.DisplayLayout.MaxRowScrollRegions = 1;
			appearance17.BackColor = System.Drawing.SystemColors.Window;
			appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCountry.DisplayLayout.Override.ActiveCellAppearance = appearance17;
			appearance18.BackColor = System.Drawing.SystemColors.Highlight;
			appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCountry.DisplayLayout.Override.ActiveRowAppearance = appearance18;
			this.cmbCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance19.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCountry.DisplayLayout.Override.CardAreaAppearance = appearance19;
			appearance20.BorderColor = System.Drawing.Color.Silver;
			appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCountry.DisplayLayout.Override.CellAppearance = appearance20;
			this.cmbCountry.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCountry.DisplayLayout.Override.CellPadding = 0;
			appearance21.BackColor = System.Drawing.SystemColors.Control;
			appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance21.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCountry.DisplayLayout.Override.GroupByRowAppearance = appearance21;
			appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCountry.DisplayLayout.Override.HeaderAppearance = appearance22;
			this.cmbCountry.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCountry.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance23.BackColor = System.Drawing.SystemColors.Window;
			appearance23.BorderColor = System.Drawing.Color.Silver;
			this.cmbCountry.DisplayLayout.Override.RowAppearance = appearance23;
			this.cmbCountry.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCountry.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
			this.cmbCountry.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCountry.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCountry.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCountry.DisplayMember = "";
			this.cmbCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbCountry.DropDownWidth = 0;
			this.cmbCountry.FlatMode = true;
			this.cmbCountry.Location = new System.Drawing.Point(120, 198);
			this.cmbCountry.Name = "cmbCountry";
			this.cmbCountry.Size = new System.Drawing.Size(200, 20);
			this.cmbCountry.TabIndex = 166;
			this.cmbCountry.ValueMember = "";
			this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
			// 
			// label42
			// 
			this.label42.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label42.ForeColor = System.Drawing.Color.Red;
			this.label42.Location = new System.Drawing.Point(102, 222);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(8, 8);
			this.label42.TabIndex = 165;
			this.label42.Text = "*";
			// 
			// label40
			// 
			this.label40.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label40.Location = new System.Drawing.Point(4, 302);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(102, 16);
			this.label40.TabIndex = 162;
			this.label40.Text = "(1-111-111111)";
			// 
			// label34
			// 
			this.label34.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label34.ForeColor = System.Drawing.Color.Red;
			this.label34.Location = new System.Drawing.Point(42, 322);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(8, 8);
			this.label34.TabIndex = 161;
			this.label34.Text = "*";
			// 
			// txtZip
			// 
			this.txtZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtZip.Location = new System.Drawing.Point(120, 244);
			this.txtZip.MaxLength = 50;
			this.txtZip.Name = "txtZip";
			this.txtZip.Size = new System.Drawing.Size(200, 21);
			this.txtZip.TabIndex = 160;
			this.txtZip.Text = "";
			// 
			// lblZip
			// 
			this.lblZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblZip.Location = new System.Drawing.Point(4, 246);
			this.lblZip.Name = "lblZip";
			this.lblZip.Size = new System.Drawing.Size(28, 16);
			this.lblZip.TabIndex = 159;
			this.lblZip.Text = "Zip";
			// 
			// lblStateTerrirtory
			// 
			this.lblStateTerrirtory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblStateTerrirtory.Location = new System.Drawing.Point(4, 222);
			this.lblStateTerrirtory.Name = "lblStateTerrirtory";
			this.lblStateTerrirtory.Size = new System.Drawing.Size(98, 16);
			this.lblStateTerrirtory.TabIndex = 157;
			this.lblStateTerrirtory.Text = "State/Territory";
			this.lblStateTerrirtory.Click += new System.EventHandler(this.lblStateTerrirtory_Click);
			// 
			// label33
			// 
			this.label33.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label33.Location = new System.Drawing.Point(4, 198);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(54, 16);
			this.label33.TabIndex = 156;
			this.label33.Text = "Country";
			// 
			// label41
			// 
			this.label41.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label41.ForeColor = System.Drawing.Color.Red;
			this.label41.Location = new System.Drawing.Point(44, 268);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(8, 8);
			this.label41.TabIndex = 154;
			this.label41.Text = "*";
			// 
			// label39
			// 
			this.label39.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label39.ForeColor = System.Drawing.Color.Red;
			this.label39.Location = new System.Drawing.Point(56, 290);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(8, 8);
			this.label39.TabIndex = 152;
			this.label39.Text = "*";
			// 
			// label38
			// 
			this.label38.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label38.ForeColor = System.Drawing.Color.Red;
			this.label38.Location = new System.Drawing.Point(58, 198);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(8, 8);
			this.label38.TabIndex = 151;
			this.label38.Text = "*";
			// 
			// label37
			// 
			this.label37.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label37.ForeColor = System.Drawing.Color.Red;
			this.label37.Location = new System.Drawing.Point(64, 154);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(8, 8);
			this.label37.TabIndex = 150;
			this.label37.Text = "*";
			// 
			// label36
			// 
			this.label36.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label36.ForeColor = System.Drawing.Color.Red;
			this.label36.Location = new System.Drawing.Point(62, 132);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(8, 8);
			this.label36.TabIndex = 149;
			this.label36.Text = "*";
			// 
			// label35
			// 
			this.label35.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label35.ForeColor = System.Drawing.Color.Red;
			this.label35.Location = new System.Drawing.Point(36, 110);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(8, 8);
			this.label35.TabIndex = 148;
			this.label35.Text = "*";
			// 
			// label18
			// 
			this.label18.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label18.ForeColor = System.Drawing.Color.Red;
			this.label18.Location = new System.Drawing.Point(74, 66);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(8, 8);
			this.label18.TabIndex = 146;
			this.label18.Text = "*";
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.label17.ForeColor = System.Drawing.Color.Red;
			this.label17.Location = new System.Drawing.Point(70, 22);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(8, 8);
			this.label17.TabIndex = 145;
			this.label17.Text = "*";
			// 
			// txtFaxA
			// 
			this.txtFaxA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFaxA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtFaxA.Location = new System.Drawing.Point(120, 386);
			this.txtFaxA.MaxLength = 50;
			this.txtFaxA.Name = "txtFaxA";
			this.txtFaxA.Size = new System.Drawing.Size(200, 21);
			this.txtFaxA.TabIndex = 143;
			this.txtFaxA.Text = "";
			this.txtFaxA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtFaxA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// label32
			// 
			this.label32.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label32.Location = new System.Drawing.Point(4, 388);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(38, 16);
			this.label32.TabIndex = 141;
			this.label32.Text = "Fax #";
			// 
			// txtHomeTeleA
			// 
			this.txtHomeTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtHomeTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtHomeTeleA.Location = new System.Drawing.Point(120, 364);
			this.txtHomeTeleA.MaxLength = 50;
			this.txtHomeTeleA.Name = "txtHomeTeleA";
			this.txtHomeTeleA.Size = new System.Drawing.Size(200, 21);
			this.txtHomeTeleA.TabIndex = 142;
			this.txtHomeTeleA.Text = "";
			this.txtHomeTeleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtHomeTeleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtPagerTeleA
			// 
			this.txtPagerTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPagerTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPagerTeleA.Location = new System.Drawing.Point(120, 342);
			this.txtPagerTeleA.MaxLength = 50;
			this.txtPagerTeleA.Name = "txtPagerTeleA";
			this.txtPagerTeleA.Size = new System.Drawing.Size(200, 21);
			this.txtPagerTeleA.TabIndex = 140;
			this.txtPagerTeleA.Text = "";
			this.txtPagerTeleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtPagerTeleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtCellTeleA
			// 
			this.txtCellTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCellTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtCellTeleA.Location = new System.Drawing.Point(120, 320);
			this.txtCellTeleA.MaxLength = 50;
			this.txtCellTeleA.Name = "txtCellTeleA";
			this.txtCellTeleA.Size = new System.Drawing.Size(200, 21);
			this.txtCellTeleA.TabIndex = 139;
			this.txtCellTeleA.Text = "";
			// 
			// txtWorkTeleA
			// 
			this.txtWorkTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtWorkTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtWorkTeleA.Location = new System.Drawing.Point(120, 288);
			this.txtWorkTeleA.MaxLength = 50;
			this.txtWorkTeleA.Name = "txtWorkTeleA";
			this.txtWorkTeleA.Size = new System.Drawing.Size(200, 21);
			this.txtWorkTeleA.TabIndex = 138;
			this.txtWorkTeleA.Text = "";
			this.txtWorkTeleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtWorkTeleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtEmailA
			// 
			this.txtEmailA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEmailA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtEmailA.Location = new System.Drawing.Point(120, 266);
			this.txtEmailA.MaxLength = 50;
			this.txtEmailA.Name = "txtEmailA";
			this.txtEmailA.Size = new System.Drawing.Size(200, 21);
			this.txtEmailA.TabIndex = 136;
			this.txtEmailA.Text = "";
			this.txtEmailA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtEmailA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtAddress2A
			// 
			this.txtAddress2A.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress2A.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtAddress2A.Location = new System.Drawing.Point(120, 174);
			this.txtAddress2A.MaxLength = 50;
			this.txtAddress2A.Name = "txtAddress2A";
			this.txtAddress2A.Size = new System.Drawing.Size(200, 21);
			this.txtAddress2A.TabIndex = 135;
			this.txtAddress2A.Text = "";
			this.txtAddress2A.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtAddress2A.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtAddress1A
			// 
			this.txtAddress1A.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress1A.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtAddress1A.Location = new System.Drawing.Point(120, 152);
			this.txtAddress1A.MaxLength = 50;
			this.txtAddress1A.Name = "txtAddress1A";
			this.txtAddress1A.Size = new System.Drawing.Size(200, 21);
			this.txtAddress1A.TabIndex = 134;
			this.txtAddress1A.Text = "";
			this.txtAddress1A.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtAddress1A.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtPasswordA
			// 
			this.txtPasswordA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPasswordA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPasswordA.Location = new System.Drawing.Point(120, 130);
			this.txtPasswordA.MaxLength = 50;
			this.txtPasswordA.Name = "txtPasswordA";
			this.txtPasswordA.Size = new System.Drawing.Size(200, 21);
			this.txtPasswordA.TabIndex = 133;
			this.txtPasswordA.Text = "";
			this.txtPasswordA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtPasswordA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtLoginNameA
			// 
			this.txtLoginNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLoginNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtLoginNameA.Location = new System.Drawing.Point(120, 108);
			this.txtLoginNameA.MaxLength = 50;
			this.txtLoginNameA.Name = "txtLoginNameA";
			this.txtLoginNameA.Size = new System.Drawing.Size(200, 21);
			this.txtLoginNameA.TabIndex = 132;
			this.txtLoginNameA.Text = "";
			this.txtLoginNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtLoginNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtTitleA
			// 
			this.txtTitleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTitleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTitleA.Location = new System.Drawing.Point(120, 86);
			this.txtTitleA.MaxLength = 50;
			this.txtTitleA.Name = "txtTitleA";
			this.txtTitleA.Size = new System.Drawing.Size(200, 21);
			this.txtTitleA.TabIndex = 131;
			this.txtTitleA.Text = "";
			this.txtTitleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtTitleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtShortNameA
			// 
			this.txtShortNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtShortNameA.Location = new System.Drawing.Point(120, 64);
			this.txtShortNameA.MaxLength = 50;
			this.txtShortNameA.Name = "txtShortNameA";
			this.txtShortNameA.Size = new System.Drawing.Size(200, 21);
			this.txtShortNameA.TabIndex = 130;
			this.txtShortNameA.Text = "";
			this.txtShortNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtShortNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtLastNameA
			// 
			this.txtLastNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLastNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtLastNameA.Location = new System.Drawing.Point(120, 42);
			this.txtLastNameA.MaxLength = 50;
			this.txtLastNameA.Name = "txtLastNameA";
			this.txtLastNameA.Size = new System.Drawing.Size(200, 21);
			this.txtLastNameA.TabIndex = 129;
			this.txtLastNameA.Text = "";
			this.txtLastNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtLastNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtFirstNameA
			// 
			this.txtFirstNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFirstNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtFirstNameA.Location = new System.Drawing.Point(120, 20);
			this.txtFirstNameA.MaxLength = 50;
			this.txtFirstNameA.Name = "txtFirstNameA";
			this.txtFirstNameA.Size = new System.Drawing.Size(200, 21);
			this.txtFirstNameA.TabIndex = 128;
			this.txtFirstNameA.Text = "";
			this.txtFirstNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtFirstNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// label19
			// 
			this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label19.Location = new System.Drawing.Point(4, 366);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(56, 16);
			this.label19.TabIndex = 127;
			this.label19.Text = "Home #";
			// 
			// label20
			// 
			this.label20.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label20.Location = new System.Drawing.Point(4, 344);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(52, 16);
			this.label20.TabIndex = 126;
			this.label20.Text = "Pager #";
			// 
			// label21
			// 
			this.label21.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label21.Location = new System.Drawing.Point(4, 322);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(40, 16);
			this.label21.TabIndex = 125;
			this.label21.Text = "Cell #";
			// 
			// label22
			// 
			this.label22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label22.Location = new System.Drawing.Point(4, 290);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(52, 16);
			this.label22.TabIndex = 124;
			this.label22.Text = "Work #";
			// 
			// label23
			// 
			this.label23.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label23.Location = new System.Drawing.Point(4, 268);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(40, 16);
			this.label23.TabIndex = 123;
			this.label23.Text = "E-Mail";
			// 
			// label24
			// 
			this.label24.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label24.Location = new System.Drawing.Point(4, 176);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(64, 16);
			this.label24.TabIndex = 122;
			this.label24.Text = "Address 2";
			// 
			// label25
			// 
			this.label25.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label25.Location = new System.Drawing.Point(4, 154);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(64, 16);
			this.label25.TabIndex = 121;
			this.label25.Text = "Address 1";
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label26.Location = new System.Drawing.Point(4, 132);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(62, 16);
			this.label26.TabIndex = 120;
			this.label26.Text = "Password";
			// 
			// label27
			// 
			this.label27.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label27.Location = new System.Drawing.Point(4, 110);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(36, 16);
			this.label27.TabIndex = 119;
			this.label27.Text = "Login";
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label28.Location = new System.Drawing.Point(4, 88);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(30, 16);
			this.label28.TabIndex = 118;
			this.label28.Text = "Title";
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label29.Location = new System.Drawing.Point(4, 66);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(74, 16);
			this.label29.TabIndex = 117;
			this.label29.Text = "Short Name";
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label30.Location = new System.Drawing.Point(4, 44);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(66, 16);
			this.label30.TabIndex = 116;
			this.label30.Text = "Last Name";
			// 
			// label31
			// 
			this.label31.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label31.Location = new System.Drawing.Point(4, 22);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(66, 16);
			this.label31.TabIndex = 115;
			this.label31.Text = "First Name";
			// 
			// ultraTabPageControl2
			// 
			this.ultraTabPageControl2.Controls.Add(this.groupBox4);
			this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl2.Name = "ultraTabPageControl2";
			this.ultraTabPageControl2.Size = new System.Drawing.Size(346, 421);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.chkPermissionCounterParties);
			this.groupBox4.Controls.Add(this.chkPermissionMaintainCompanis);
			this.groupBox4.Controls.Add(this.chkPermissionCompany);
			this.groupBox4.Controls.Add(this.chkPermissionAUEC);
			this.groupBox4.Location = new System.Drawing.Point(-2, -2);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(332, 154);
			this.groupBox4.TabIndex = 0;
			this.groupBox4.TabStop = false;
			// 
			// chkPermissionCounterParties
			// 
			this.chkPermissionCounterParties.Checked = true;
			this.chkPermissionCounterParties.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkPermissionCounterParties.Location = new System.Drawing.Point(8, 124);
			this.chkPermissionCounterParties.Name = "chkPermissionCounterParties";
			this.chkPermissionCounterParties.Size = new System.Drawing.Size(150, 24);
			this.chkPermissionCounterParties.TabIndex = 12;
			this.chkPermissionCounterParties.Tag = "4";
			this.chkPermissionCounterParties.Text = "Maintain Counter parties";
			// 
			// chkPermissionMaintainCompanis
			// 
			this.chkPermissionMaintainCompanis.Checked = true;
			this.chkPermissionMaintainCompanis.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkPermissionMaintainCompanis.Location = new System.Drawing.Point(8, 92);
			this.chkPermissionMaintainCompanis.Name = "chkPermissionMaintainCompanis";
			this.chkPermissionMaintainCompanis.Size = new System.Drawing.Size(144, 24);
			this.chkPermissionMaintainCompanis.TabIndex = 11;
			this.chkPermissionMaintainCompanis.Tag = "3";
			this.chkPermissionMaintainCompanis.Text = "Maintain Companies";
			// 
			// chkPermissionCompany
			// 
			this.chkPermissionCompany.Checked = true;
			this.chkPermissionCompany.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkPermissionCompany.Location = new System.Drawing.Point(8, 60);
			this.chkPermissionCompany.Name = "chkPermissionCompany";
			this.chkPermissionCompany.Size = new System.Drawing.Size(120, 24);
			this.chkPermissionCompany.TabIndex = 10;
			this.chkPermissionCompany.Tag = "2";
			this.chkPermissionCompany.Text = "Set up company";
			// 
			// chkPermissionAUEC
			// 
			this.chkPermissionAUEC.Checked = true;
			this.chkPermissionAUEC.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkPermissionAUEC.Location = new System.Drawing.Point(8, 28);
			this.chkPermissionAUEC.Name = "chkPermissionAUEC";
			this.chkPermissionAUEC.Size = new System.Drawing.Size(106, 24);
			this.chkPermissionAUEC.TabIndex = 9;
			this.chkPermissionAUEC.Tag = "1";
			this.chkPermissionAUEC.Text = "Maintain AUEC";
			// 
			// ultraTabPageControl3
			// 
			this.ultraTabPageControl3.Controls.Add(this.groupBox3);
			this.ultraTabPageControl3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl3.Name = "ultraTabPageControl3";
			this.ultraTabPageControl3.Size = new System.Drawing.Size(492, 446);
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.groupBox3.Controls.Add(this.txtMailingAddressT);
			this.groupBox3.Controls.Add(this.txtComment);
			this.groupBox3.Controls.Add(this.txtTitle);
			this.groupBox3.Controls.Add(this.label16);
			this.groupBox3.Controls.Add(this.label15);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.txtEmail);
			this.groupBox3.Controls.Add(this.txtFaxTele);
			this.groupBox3.Controls.Add(this.txtHomeTele);
			this.groupBox3.Controls.Add(this.txtPagerTele);
			this.groupBox3.Controls.Add(this.txtCellTele);
			this.groupBox3.Controls.Add(this.txtWorkTele);
			this.groupBox3.Controls.Add(this.txtLastName);
			this.groupBox3.Controls.Add(this.txtFirstName);
			this.groupBox3.Controls.Add(this.txtAddress2);
			this.groupBox3.Controls.Add(this.txtAddress1);
			this.groupBox3.Controls.Add(this.txtShortName);
			this.groupBox3.Controls.Add(this.txtProductName);
			this.groupBox3.Controls.Add(this.txtVendorName);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Location = new System.Drawing.Point(0, 8);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(456, 408);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			// 
			// txtMailingAddressT
			// 
			this.txtMailingAddressT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMailingAddressT.Location = new System.Drawing.Point(200, 373);
			this.txtMailingAddressT.MaxLength = 50;
			this.txtMailingAddressT.Name = "txtMailingAddressT";
			this.txtMailingAddressT.Size = new System.Drawing.Size(232, 21);
			this.txtMailingAddressT.TabIndex = 157;
			this.txtMailingAddressT.Text = "";
			this.txtMailingAddressT.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtMailingAddressT.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtComment
			// 
			this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtComment.Location = new System.Drawing.Point(200, 349);
			this.txtComment.Multiline = true;
			this.txtComment.Name = "txtComment";
			this.txtComment.Size = new System.Drawing.Size(232, 20);
			this.txtComment.TabIndex = 156;
			this.txtComment.Text = "";
			this.txtComment.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtComment.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtTitle
			// 
			this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTitle.Location = new System.Drawing.Point(200, 325);
			this.txtTitle.MaxLength = 50;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(232, 21);
			this.txtTitle.TabIndex = 155;
			this.txtTitle.Text = "";
			this.txtTitle.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtTitle.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(24, 370);
			this.label16.Name = "label16";
			this.label16.TabIndex = 154;
			this.label16.Text = "Mailing Address";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(24, 353);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(100, 16);
			this.label15.TabIndex = 153;
			this.label15.Text = "Comment";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(24, 329);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 16);
			this.label14.TabIndex = 152;
			this.label14.Text = "Title";
			// 
			// txtEmail
			// 
			this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEmail.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtEmail.Location = new System.Drawing.Point(200, 301);
			this.txtEmail.MaxLength = 50;
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(232, 21);
			this.txtEmail.TabIndex = 151;
			this.txtEmail.Text = "";
			this.txtEmail.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtEmail.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtFaxTele
			// 
			this.txtFaxTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFaxTele.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFaxTele.Location = new System.Drawing.Point(200, 277);
			this.txtFaxTele.MaxLength = 50;
			this.txtFaxTele.Name = "txtFaxTele";
			this.txtFaxTele.Size = new System.Drawing.Size(232, 21);
			this.txtFaxTele.TabIndex = 150;
			this.txtFaxTele.Text = "";
			this.txtFaxTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtFaxTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtHomeTele
			// 
			this.txtHomeTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtHomeTele.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtHomeTele.Location = new System.Drawing.Point(200, 253);
			this.txtHomeTele.MaxLength = 50;
			this.txtHomeTele.Name = "txtHomeTele";
			this.txtHomeTele.Size = new System.Drawing.Size(232, 21);
			this.txtHomeTele.TabIndex = 149;
			this.txtHomeTele.Text = "";
			this.txtHomeTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtHomeTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtPagerTele
			// 
			this.txtPagerTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPagerTele.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPagerTele.Location = new System.Drawing.Point(200, 229);
			this.txtPagerTele.MaxLength = 50;
			this.txtPagerTele.Name = "txtPagerTele";
			this.txtPagerTele.Size = new System.Drawing.Size(232, 21);
			this.txtPagerTele.TabIndex = 148;
			this.txtPagerTele.Text = "";
			this.txtPagerTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtPagerTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtCellTele
			// 
			this.txtCellTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCellTele.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtCellTele.Location = new System.Drawing.Point(200, 205);
			this.txtCellTele.MaxLength = 50;
			this.txtCellTele.Name = "txtCellTele";
			this.txtCellTele.Size = new System.Drawing.Size(232, 21);
			this.txtCellTele.TabIndex = 147;
			this.txtCellTele.Text = "";
			this.txtCellTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtCellTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtWorkTele
			// 
			this.txtWorkTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtWorkTele.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtWorkTele.Location = new System.Drawing.Point(200, 181);
			this.txtWorkTele.MaxLength = 50;
			this.txtWorkTele.Name = "txtWorkTele";
			this.txtWorkTele.Size = new System.Drawing.Size(232, 21);
			this.txtWorkTele.TabIndex = 146;
			this.txtWorkTele.Text = "";
			this.txtWorkTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtWorkTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtLastName
			// 
			this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLastName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtLastName.Location = new System.Drawing.Point(200, 157);
			this.txtLastName.MaxLength = 50;
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(232, 21);
			this.txtLastName.TabIndex = 145;
			this.txtLastName.Text = "";
			this.txtLastName.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtLastName.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtFirstName
			// 
			this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFirstName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFirstName.Location = new System.Drawing.Point(200, 133);
			this.txtFirstName.MaxLength = 50;
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(232, 21);
			this.txtFirstName.TabIndex = 144;
			this.txtFirstName.Text = "";
			this.txtFirstName.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtFirstName.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtAddress2
			// 
			this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress2.Location = new System.Drawing.Point(200, 109);
			this.txtAddress2.MaxLength = 50;
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(232, 21);
			this.txtAddress2.TabIndex = 143;
			this.txtAddress2.Text = "";
			this.txtAddress2.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtAddress2.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtAddress1
			// 
			this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress1.Location = new System.Drawing.Point(200, 85);
			this.txtAddress1.MaxLength = 50;
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(232, 21);
			this.txtAddress1.TabIndex = 142;
			this.txtAddress1.Text = "";
			this.txtAddress1.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtAddress1.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(200, 61);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(232, 21);
			this.txtShortName.TabIndex = 141;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtProductName
			// 
			this.txtProductName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtProductName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtProductName.Location = new System.Drawing.Point(200, 37);
			this.txtProductName.MaxLength = 50;
			this.txtProductName.Name = "txtProductName";
			this.txtProductName.Size = new System.Drawing.Size(232, 21);
			this.txtProductName.TabIndex = 140;
			this.txtProductName.Text = "";
			this.txtProductName.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtProductName.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// txtVendorName
			// 
			this.txtVendorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtVendorName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtVendorName.Location = new System.Drawing.Point(200, 13);
			this.txtVendorName.MaxLength = 50;
			this.txtVendorName.Name = "txtVendorName";
			this.txtVendorName.Size = new System.Drawing.Size(232, 21);
			this.txtVendorName.TabIndex = 139;
			this.txtVendorName.Text = "";
			this.txtVendorName.LostFocus += new System.EventHandler(this.ControlLostFocus);
			this.txtVendorName.GotFocus += new System.EventHandler(this.ControlGotFocus);
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label13.Location = new System.Drawing.Point(24, 306);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(56, 16);
			this.label13.TabIndex = 138;
			this.label13.Text = "E - Mail";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label12.Location = new System.Drawing.Point(24, 282);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(64, 16);
			this.label12.TabIndex = 137;
			this.label12.Text = "Fax #";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label11.Location = new System.Drawing.Point(24, 258);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(114, 16);
			this.label11.TabIndex = 136;
			this.label11.Text = "Home TelePhone #";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label10.Location = new System.Drawing.Point(24, 234);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(118, 16);
			this.label10.TabIndex = 135;
			this.label10.Text = "Pager TelePhone #";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label9.Location = new System.Drawing.Point(24, 210);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 134;
			this.label9.Text = "Mobile TelePhone #";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label8.Location = new System.Drawing.Point(24, 186);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(116, 16);
			this.label8.TabIndex = 133;
			this.label8.Text = "Work TelePhone #";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label7.Location = new System.Drawing.Point(24, 162);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(122, 16);
			this.label7.TabIndex = 132;
			this.label7.Text = "Contact - Last Name";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label6.Location = new System.Drawing.Point(24, 138);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(126, 16);
			this.label6.TabIndex = 131;
			this.label6.Text = "Contact - First Name";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label5.Location = new System.Drawing.Point(24, 114);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 130;
			this.label5.Text = "Address2";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(24, 90);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 129;
			this.label4.Text = "Address1";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(24, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 128;
			this.label3.Text = "Short Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(24, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 127;
			this.label2.Text = "Product";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(24, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 126;
			this.label1.Text = "Full Name of Vendor";
			// 
			// tabThirdPartyVendor
			// 
			appearance25.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance25.BackColor2 = System.Drawing.Color.White;
			appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tabThirdPartyVendor.ActiveTabAppearance = appearance25;
			this.tabThirdPartyVendor.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabSharedControlsPage1);
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl1);
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl2);
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl3);
			this.tabThirdPartyVendor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.tabThirdPartyVendor.Location = new System.Drawing.Point(176, 0);
			this.tabThirdPartyVendor.Name = "tabThirdPartyVendor";
			this.tabThirdPartyVendor.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.tabThirdPartyVendor.Size = new System.Drawing.Size(348, 442);
			this.tabThirdPartyVendor.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tabThirdPartyVendor.TabIndex = 0;
			appearance26.ForeColor = System.Drawing.Color.Black;
			ultraTab1.ActiveAppearance = appearance26;
			ultraTab1.Key = "AdminUserDetails";
			ultraTab1.TabPage = this.ultraTabPageControl1;
			ultraTab1.Text = "Details";
			appearance27.ForeColor = System.Drawing.Color.Black;
			ultraTab2.ActiveAppearance = appearance27;
			ultraTab2.Key = "PermissionLevels";
			ultraTab2.TabPage = this.ultraTabPageControl2;
			ultraTab2.Text = "Permission Levels";
			this.tabThirdPartyVendor.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																										  ultraTab1,
																										  ultraTab2});
			this.tabThirdPartyVendor.ActiveTabChanged += new Infragistics.Win.UltraWinTabControl.ActiveTabChangedEventHandler(this.tabThirdPartyVendor_ActiveTabChanged);
			this.tabThirdPartyVendor.TabIndexChanged += new System.EventHandler(this.tabThirdPartyVendor_TabIndexChanged);
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(346, 421);
			this.ultraTabSharedControlsPage1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabSharedControlsPage1_Paint);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(276, 444);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 5;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.trvUserRights);
			this.groupBox1.Location = new System.Drawing.Point(8, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(168, 442);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// trvUserRights
			// 
			this.trvUserRights.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.trvUserRights.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.trvUserRights.HideSelection = false;
			this.trvUserRights.ImageIndex = -1;
			this.trvUserRights.Location = new System.Drawing.Point(8, 16);
			this.trvUserRights.Name = "trvUserRights";
			this.trvUserRights.SelectedImageIndex = -1;
			this.trvUserRights.ShowLines = false;
			this.trvUserRights.Size = new System.Drawing.Size(152, 420);
			this.trvUserRights.TabIndex = 0;
			this.trvUserRights.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvUserRights_AfterSelect);
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Location = new System.Drawing.Point(14, 444);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Location = new System.Drawing.Point(92, 444);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(354, 444);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 7;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// ThirdPartyVendor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(528, 489);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.tabThirdPartyVendor);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnAdd);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ThirdPartyVendor";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SLSU";
			this.Load += new System.EventHandler(this.ThirdPartyVendor_Load);
			this.ultraTabPageControl1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
			this.ultraTabPageControl2.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ultraTabPageControl3.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabThirdPartyVendor)).EndInit();
			this.tabThirdPartyVendor.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ultraTabSharedControlsPage1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void ultraTabPageControl1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		/// <summary>
		/// This method saves the User details as per the selected node in the tree by calling the
		/// SaveUserDetails method.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				NodeDetails nodeDetails = (NodeDetails) trvUserRights.SelectedNode.Tag;

				if(nodeDetails.Type == NodeType.User)
				{				
					int nodeID = SaveUserDetails();						
					if(nodeID > 0)
					{
						BindUserTree();
						SelectNode(NodeType.User, nodeID);
					}
					else
					{
						SelectNode(NodeType.User, nodeDetails.NodeID);
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
				LogEntry logEntry = new LogEntry("btnSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSave_Click"); 
				Logger.Write(logEntry); 
				#endregion
			}
		}

		/// <summary>
		/// This method saves user details while checking for the validations.
		/// </summary>
		/// <returns></returns>
		private int SaveUserDetails()
		{	
			int result = int.MinValue;
			NodeDetails node = (NodeDetails) trvUserRights.SelectedNode.Tag;
			Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
			Match emailMatch = emailRegex.Match(txtEmailA.Text.ToString());
			
			errorProvider1.SetError(txtFirstNameA, "");
			errorProvider1.SetError(txtShortNameA, "");
			errorProvider1.SetError(txtLoginNameA, "");
			errorProvider1.SetError(txtPasswordA, "");
			errorProvider1.SetError(txtAddress1A, "");
			errorProvider1.SetError(cmbCountry, "");
			errorProvider1.SetError(cmbState, "");
			errorProvider1.SetError(txtEmailA, "");
			errorProvider1.SetError(txtWorkTeleA, "");
			errorProvider1.SetError(txtCellTele, "");
			
			if(txtFirstNameA.Text.Trim() == "")
			{
				errorProvider1.SetError(txtFirstNameA, "Please enter First Name!");
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				txtFirstNameA.Focus();
			}
			else if(txtShortNameA.Text.Trim() == "")
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtShortNameA, "Please enter Short Name!");
				txtShortNameA.Focus();
			}
			else if(txtLoginNameA.Text.Trim() == "")
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtLoginNameA, "Please enter Login Name!");
				txtLoginNameA.Focus();
			}
			else if(txtPasswordA.Text.Trim() == "")
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtPasswordA, "Please enter Password!");
				txtPasswordA.Focus();				
			}
			else if(int.Parse(txtPasswordA.Text.Length.ToString()) < 4)
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtPasswordA, "Please enter password having at least four characters !");
				txtPasswordA.Focus();				
			}
			else if(txtAddress1A.Text.Trim() == "")
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtAddress1A, "Please enter Address1!");
				txtAddress1A.Focus();
			}
			else if(int.Parse(cmbCountry.Value.ToString()) == int.MinValue)
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(cmbCountry, "Please select Country!");
				cmbCountry.Focus();
			}
			
			//else if(txtEmailA.Text.Trim() == "")
			else if (!emailMatch.Success)
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtEmailA, "Please enter valid Email address!");
				txtEmailA.Focus();
			}
			else if(txtWorkTeleA.Text.Trim() == "")
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtWorkTeleA, "Please enter Work Telephone!");
				txtWorkTeleA.Focus();
			}
			else if(txtCellTeleA.Text.Trim() == "")
			{
				tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
				errorProvider1.SetError(txtCellTeleA, "Please enter Mobile No!");
				txtCellTeleA.Focus();
			}
			else
			{
				Nirvana.Admin.BLL.User user  = new Nirvana.Admin.BLL.User();
			
				user.FirstName = txtFirstNameA.Text.Trim();
				user.LastName = txtLastNameA.Text.Trim();
				user.ShortName = txtShortNameA.Text.Trim();			
				user.Title = txtTitleA.Text.Trim();
				user.LoginName = txtLoginNameA.Text.Trim();
				user.Password = txtPasswordA.Text.Trim();
				user.Address1 = txtAddress1A.Text.Trim();
				user.Address2 = txtAddress2A.Text.Trim();
				user.CountryID = int.Parse(cmbCountry.Value.ToString());
				user.StateID = int.Parse(cmbState.Value.ToString());
				user.Zip = txtZip.Text.Trim();
				user.EMail = txtEmailA.Text.Trim();
				user.TelephoneWork = txtWorkTeleA.Text.Trim();
				user.TelephoneMobile = txtCellTeleA.Text.Trim();
				user.TelephonePager = txtPagerTeleA.Text.Trim();
				user.TelephoneHome = txtHomeTeleA.Text.Trim();
				user.Fax = txtFaxA.Text.Trim();

				
				user.UserID = node.NodeID;

				int newUserID = UserManager.SaveUser(user);
				if(newUserID == -1)
				{
//					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//					Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User already exists with given login name. Please select other login name.");
				}	
				else
				{
					SaveUserPermission(newUserID);
//					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//					Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Saved!");
				}
				result = newUserID;
			}			
			return result;
		}

		/// <summary>
		/// This method saves the user permissions whose id is passed to it as a parameter.
		/// </summary>
		/// <param name="userID"></param>
		private void SaveUserPermission(int userID)
		{
			Permissions permissions = new Permissions();
			permissions.Add(new Permission(int.Parse(chkPermissionAUEC.Tag.ToString()), chkPermissionAUEC.Checked));
			permissions.Add(new Permission(int.Parse(chkPermissionCompany.Tag.ToString()), chkPermissionCompany.Checked));
			permissions.Add(new Permission(int.Parse(chkPermissionMaintainCompanis.Tag.ToString()), chkPermissionMaintainCompanis.Checked));
			permissions.Add(new Permission(int.Parse(chkPermissionCounterParties.Tag.ToString()), chkPermissionCounterParties.Checked));

			UserManager.AddPermissions(userID, permissions);
//			Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//			Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User saved.");
		}

		/// <summary>
		/// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
		/// </summary>
		/// <param name="nodeDetails"></param>
		private void SelectNode(NodeType nodeType, int nodeID)
		{
			foreach(TreeNode node in trvUserRights.Nodes)
			{
				NodeDetails nodeDetails = (NodeDetails) node.Tag;
				if(nodeDetails.Type == nodeType)
				{
					foreach(TreeNode subNode in node.Nodes)
					{
						NodeDetails subNodeDetails = (NodeDetails) subNode.Tag;
						if(subNodeDetails.NodeID == nodeID)
						{
							trvUserRights.SelectedNode = subNode;
						}
					}
				}
			}
		}

		/// <summary>
		/// Binds the tree as this form is loaded. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ThirdPartyVendor_Load(object sender, System.EventArgs e)
		{
			BindUserTree();
			BindCountries();
			BindStates();
		}

		/// <summary>
		/// This method binds the existing <see cref="Countries"/> in the ComboBox control by assigning the 
		/// countries object to its datasource property.
		/// </summary>
		private void BindCountries()
		{
			//GetCountries method fetches the existing countries from the database.
			Countries countries = GeneralManager.GetCountries();
			//Inserting the - Select - option in the Combo Box at the top.
			countries.Insert(0, new Country(int.MinValue, C_COMBO_SELECT));
			cmbCountry.DisplayMember = "Name";
			cmbCountry.ValueMember = "CountryID";
			cmbCountry.DataSource = countries;			
			cmbCountry.Value = int.MinValue;
		}

		/// <summary>
		/// This method binds the existing <see cref="States"/> in the ComboBox control by assigning the 
		/// states object to its datasource property.
		/// </summary>
		private void BindStates()
		{
			//GetStates method fetches the existing states from the database.
			States states = GeneralManager.GetStates();
			//Inserting the - Select - option in the Combo Box at the top.
			states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
			cmbState.DisplayMember = "StateName";
			cmbState.ValueMember = "StateID";
			cmbState.DataSource = states;			
			cmbState.Value = int.MinValue;
		}

		/// <summary>
		/// This method empties the ComboBox from any state by assigning the states object to null value.
		/// </summary>
		private void BindEmptyStates()
		{
			//GetStates method fetches the existing states from the database.
			States states = new States();
			//Inserting the - Select - option in the Combo Box at the top.
			states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
			cmbState.DisplayMember = "StateName";
			cmbState.ValueMember = "StateID";
			cmbState.DataSource = states;			
			cmbState.Value = int.MinValue;
		}
		
		/// <summary>
		/// Bind left tree to th User and Vendor data.
		/// </summary>
		private void BindUserTree()		
		{
			try
			{
				int parentNodeID = 0;
				int selectedNodeID = 0;
				bool gotFirstNode = false;
				Font font = new Font("Vedana", 10, System.Drawing.FontStyle.Bold);

				trvUserRights.Nodes.Clear();
			
				//if(UserManager.IsAdmin(((SuperAdminMain) this.ParentForm).UserPermissions.UserID))
				if(UserManager.IsAdmin(((SuperAdminMain) this.Owner).UserPermissions.UserID))
				{
					//Add Users			
					TreeNode treeNodeUserRoot = new TreeNode("Users");
					treeNodeUserRoot.NodeFont = font;
					NodeDetails userNodeRoot = new NodeDetails(NodeType.User, int.MinValue); 
					treeNodeUserRoot.Tag = userNodeRoot;
					Users users = UserManager.GetUsers();			
					foreach(User user in users)
					{
						if(gotFirstNode == false)
						{
							gotFirstNode = true;
							selectedNodeID = 0;
						}				
					
						TreeNode treeNodeUser;
						if(UserManager.IsAdmin(user.UserID))
						{
							treeNodeUser = new TreeNode(user.ShortName + "[A]");
						}
						else
						{
							treeNodeUser = new TreeNode(user.ShortName);
						}
						NodeDetails userNode = new NodeDetails(NodeType.User, user.UserID); 
						treeNodeUser.Tag = userNode;
					
						treeNodeUserRoot.Nodes.Add(treeNodeUser);
					}
					trvUserRights.Nodes.Add(treeNodeUserRoot);
				}
				else
				{
					DisableUserTabs();
				}
		
				
				if(gotFirstNode == true)
				{
					trvUserRights.SelectedNode = trvUserRights.Nodes[parentNodeID].Nodes[selectedNodeID];
				}
				else
				{
					//trvUserRights.SelectedNode = trvUserRights.Nodes[parentNodeID];
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
				LogEntry logEntry = new LogEntry("BindUserTree", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "BindUserTree"); 
				Logger.Write(logEntry); 
				#endregion
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
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
				bool result = false;
				if(trvUserRights.SelectedNode.Parent != null)
				{
					if(trvUserRights.SelectedNode == null)
					{
//						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//						Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Please select User/Vendor to be deleted!");
					}
					else
					{
						NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;
						int nodeID = nodeDetails.NodeID;
						if(nodeDetails.Type == NodeType.User)
						{
							//If the user is admin user then it cant be deleted.
							SuperAdminMain superAdminMain = (SuperAdminMain) this.Owner;
							if(superAdminMain.UserPermissions.UserID == nodeID || UserManager.IsAdmin(nodeID))
							{
//								Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//								Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User Can't be deleted!");
							}
							else
							{
								if(MessageBox.Show(this, "Do you want to delete selected User?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
								{
									result = UserManager.DeleteUser(nodeID);					
//									Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//									Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User deleted");
								}
							}					
						}				
						BindUserTree();				
					}			
				}
				else 
				{
//					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//					Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Can't Delete Root Node");

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
		/// This method shows the details of the selected node on the click event of the tree. It fetches the
		/// details of the selected node from the database by sending the nodeID.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trvUserRights_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			try
			{
				EnableUserTabs();
				NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;
				if(trvUserRights.SelectedNode == null)
				{
//					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//					Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Please select User/Vendor to be shown with the details!");
				}
				else
				{
					if(nodeDetails.Type == NodeType.User)
					{	//User is selected.					
					
						int userID = nodeDetails.NodeID;
						Nirvana.Admin.BLL.User user = UserManager.GetUser(userID);
						AdminUserDetails(user);

						tabThirdPartyVendor.Tabs[0].Selected = true;
						//ultraTabPageControl1.Show();
						//if((((SuperAdminMain) this.ParentForm).UserPermissions.UserID) == nodeDetails.NodeID)
						if((((SuperAdminMain) this.Owner).UserPermissions.UserID) == nodeDetails.NodeID)
						{
							DisableUserTabs();
							//MessageBox.Show("This is Admin User");
						}
						else
						{
							EnableUserTabs();  //MessageBox.Show("You have no permission to edit Users.");
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
				LogEntry logEntry = new LogEntry("trvUserRights_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "trvUserRights_AfterSelect"); 
				Logger.Write(logEntry); 
				#endregion
			}
		}

		/// <summary>
		/// Showing the user details and the associated permissions with it.
		/// </summary>
		/// <param name="user"></param>
		private void AdminUserDetails(Nirvana.Admin.BLL.User user)
		{
			//User userdetail; 
				
			if(user != null)
			{				
				txtFirstNameA.Text = user.FirstName;
				txtLastNameA.Text = user.LastName;
				txtShortNameA.Text = user.ShortName;
				txtTitleA.Text = user.Title;
				txtLoginNameA.Text = user.LoginName;
				txtPasswordA.Text = user.Password;
				//txtPasswordA.Enabled = false;
				txtAddress1A.Text = user.Address1;
				txtAddress2A.Text = user.Address2;
				if (int.Parse(user.CountryID.ToString()) <= 0)
				{
					cmbCountry.Value = int.MinValue;
				}
				else
				{
					cmbCountry.Value = int.Parse(user.CountryID.ToString());
				}
				if (int.Parse(user.StateID.ToString()) <= 0)
				{
					cmbState.Value = int.MinValue;
				}
				else
				{
					cmbState.Value = int.Parse(user.StateID.ToString());
				}
				txtZip.Text = user.Zip;
				txtEmailA.Text = user.EMail;
				txtWorkTeleA.Text = user.TelephoneWork;
				txtCellTeleA.Text = user.TelephoneMobile;
				txtPagerTeleA.Text = user.TelephonePager;
				txtHomeTeleA.Text = user.TelephoneHome;
				txtFaxA.Text = user.Fax;
				
				//Fill permission form.
				Permissions permissions = PermissionManager.GetPermissions(user.UserID);
				StringBuilder permissionIDString = new StringBuilder();   
				permissionIDString.Append(",");
				foreach(Permission permission in permissions)
				{
					permissionIDString.Append(permission.PermissionID);
					permissionIDString.Append(",");
				}

				chkPermissionAUEC.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionAUEC.Tag.ToString().Trim() + ",") >= 0?true:false);

				chkPermissionCompany.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionCompany.Tag.ToString().Trim() + ",") >= 0?true:false);

				chkPermissionCounterParties.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionCounterParties.Tag.ToString().Trim() + ",") >= 0?true:false);

				chkPermissionMaintainCompanis.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionMaintainCompanis.Tag.ToString().Trim() + ",") >= 0?true:false);
			}
			else
			{
				//
			}
		}

		private void DisableUserTabs()
		{
			tabThirdPartyVendor.Tabs[0].Enabled = false;
			tabThirdPartyVendor.Tabs[1].Enabled = false;
		}

		private void EnableUserTabs()
		{
			tabThirdPartyVendor.Tabs[0].Enabled = true;
			tabThirdPartyVendor.Tabs[1].Enabled = true;
		}

		private NodeType selectedNodeType = NodeType.User;
		/// <summary>
		///This method adds the new User on the click event of the Add Button. 
		/// It adds the User based on the tree selection before the add button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(trvUserRights.SelectedNode == null)
				{
//					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//					Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Please select User/Vendor to be added!");
				}
				else
				{
					NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;
					selectedNodeType = nodeDetails.Type;
					if(selectedNodeType == NodeType.User)
					{
//						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
//						Nirvana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Enter User Details.");
						RefreshUserForm();

						tabThirdPartyVendor.Tabs[0].Selected = true;
						ultraTabPageControl1.Show();					
					}
						
					//Set Focus to parent node.
					if(nodeDetails.NodeID != int.MinValue)
					{
						trvUserRights.SelectedNode = trvUserRights.SelectedNode.Parent;
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

		private void RefreshUserForm()
		{
			txtFirstNameA.Text = "";
			txtLastNameA.Text = "";
			txtShortNameA.Text = "";
			txtTitleA.Text = "";
			txtLoginNameA.Text = "";
			txtPasswordA.Text = "";
			//txtPasswordA.Enabled = true;
			
			txtAddress1A.Text = "";
			txtAddress2A.Text = "";
			txtEmailA.Text = "";			
			txtZip.Text = "";
			txtWorkTeleA.Text = "";
			txtCellTeleA.Text = "";
			txtPagerTeleA.Text = "";
			txtHomeTeleA.Text = "";
			txtFaxA.Text = "";
		}
		

		private void txtAddress1_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void label23_Click(object sender, System.EventArgs e)
		{
		
		}

		private void label22_Click(object sender, System.EventArgs e)
		{
		
		}

		private void label14_Click(object sender, System.EventArgs e)
		{
		
		}

		#region NodeDetails

		class NodeDetails
		{
			private NodeType _type = NodeType.User;
			private int _nodeID = int.MinValue;

			public NodeDetails()
			{
			}

			public NodeDetails(NodeType type, int nodeID)
			{
				_type = type;
				_nodeID = nodeID;
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
		}

		enum NodeType
		{
			User = 1,
			Vendor = 2			
		}		
		#endregion

		#region Focus Colors

		private void ControlGotFocus(object sender, System.EventArgs e)
		{			
			((TextBox)sender).BackColor = Color.LemonChiffon;						
			//txtAddress1A.BackColor = Color.LemonChiffon;
		}
		private void ControlLostFocus(object sender, System.EventArgs e)
		{
			((TextBox)sender).BackColor = Color.White;
			//txtAddress1A.BackColor = Color.White;
		}
		#endregion

		private void tabThirdPartyVendor_ActiveTabChanged(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangedEventArgs e)
		{
		}

		private void tabThirdPartyVendor_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			try
			{
				if(trvUserRights.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;
					
					if ((nodeDetails.Type != NodeType.User) &&  (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_USER]))
					{
						trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_USER];
					}
					if ((nodeDetails.Type != NodeType.User) &&  (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_PERMISSION]))
					{
						trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_USER];
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
				LogEntry logEntry = new LogEntry("tabThirdPartyVendor_SelectedTabChanged", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "tabThirdPartyVendor_SelectedTabChanged"); 
				Logger.Write(logEntry); 
				#endregion
			}
		}

		private void tabThirdPartyVendor_TabIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;
					
				if ((nodeDetails.Type != NodeType.User) &&  (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_USER]))
				{
					trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_USER];
				}
				if ((nodeDetails.Type != NodeType.Vendor) &&  (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_VENDOR]))
				{
					trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_VENDOR];
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
				LogEntry logEntry = new LogEntry("tabThirdPartyVendor_TabIndexChanged", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "tabThirdPartyVendor_TabIndexChanged"); 
				Logger.Write(logEntry); 
				#endregion
			}
		}

		private void cmbCountry_ValueChanged(object sender, System.EventArgs e)
		{
			int countryID = int.Parse(cmbCountry.Value.ToString());
			if (countryID > 0)
			{
				//GetStates method fetches the existing states from the database.
				States states = GeneralManager.GetStates(countryID);
				if (states.Count > 0)
				{
					states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
					cmbState.DisplayMember = "StateName";
					cmbState.ValueMember = "StateID";
					cmbState.DataSource = states;			
					cmbState.Text = C_COMBO_SELECT;
				}
				else
				{
					BindEmptyStates();
				}
			}
			
		}

		private void groupBox2_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void lblStateTerrirtory_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
