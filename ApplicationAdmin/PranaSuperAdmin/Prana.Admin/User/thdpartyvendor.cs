using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;

namespace Nirvana.Admin.User
{
	/// <summary>
	/// Summary description for thdpartyvendor.
	/// </summary>
	public class ThirdPartyVendor : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox txtVendorName;
		private System.Windows.Forms.TextBox txtProductName;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.TextBox txtCellTele;
		private System.Windows.Forms.TextBox txtFaxTele;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox txtAUEC;
		private System.Windows.Forms.TextBox txtSetupCompany;
		private System.Windows.Forms.TextBox txtMaintainComp;
		private System.Windows.Forms.TextBox txtSetupCounter;
		private System.Windows.Forms.TextBox txtMaintainCounter;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabThirdPartyVendor;
		private Infragistics.Win.UltraWinTree.UltraTree trvUserRights;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtShortNameA;
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
		private System.Windows.Forms.TextBox txtHomeTele;
		private System.Windows.Forms.TextBox txtPagerTele;
		private System.Windows.Forms.TextBox txtWorkTele;
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
		private System.Windows.Forms.TextBox txtLastNameA;
		private System.Windows.Forms.TextBox txtFirstNameA;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.TextBox txtFaxA;
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
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
			Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode1 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
			Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode2 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.txtMaintainCounter = new System.Windows.Forms.TextBox();
			this.txtSetupCounter = new System.Windows.Forms.TextBox();
			this.txtMaintainComp = new System.Windows.Forms.TextBox();
			this.txtSetupCompany = new System.Windows.Forms.TextBox();
			this.txtAUEC = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
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
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.btnSave = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.trvUserRights = new Infragistics.Win.UltraWinTree.UltraTree();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
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
			this.label32 = new System.Windows.Forms.Label();
			this.txtFaxA = new System.Windows.Forms.TextBox();
			this.ultraTabPageControl1.SuspendLayout();
			this.ultraTabPageControl2.SuspendLayout();
			this.ultraTabPageControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabThirdPartyVendor)).BeginInit();
			this.tabThirdPartyVendor.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trvUserRights)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Controls.Add(this.groupBox2);
			this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 24);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			this.ultraTabPageControl1.Size = new System.Drawing.Size(460, 390);
			this.ultraTabPageControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl1_Paint);
			// 
			// ultraTabPageControl2
			// 
			this.ultraTabPageControl2.Controls.Add(this.txtMaintainCounter);
			this.ultraTabPageControl2.Controls.Add(this.txtSetupCounter);
			this.ultraTabPageControl2.Controls.Add(this.txtMaintainComp);
			this.ultraTabPageControl2.Controls.Add(this.txtSetupCompany);
			this.ultraTabPageControl2.Controls.Add(this.txtAUEC);
			this.ultraTabPageControl2.Controls.Add(this.label18);
			this.ultraTabPageControl2.Controls.Add(this.label17);
			this.ultraTabPageControl2.Controls.Add(this.label16);
			this.ultraTabPageControl2.Controls.Add(this.label15);
			this.ultraTabPageControl2.Controls.Add(this.label14);
			this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl2.Name = "ultraTabPageControl2";
			this.ultraTabPageControl2.Size = new System.Drawing.Size(460, 382);
			// 
			// txtMaintainCounter
			// 
			this.txtMaintainCounter.Location = new System.Drawing.Point(232, 128);
			this.txtMaintainCounter.Name = "txtMaintainCounter";
			this.txtMaintainCounter.TabIndex = 9;
			this.txtMaintainCounter.Text = "";
			// 
			// txtSetupCounter
			// 
			this.txtSetupCounter.Location = new System.Drawing.Point(232, 104);
			this.txtSetupCounter.Name = "txtSetupCounter";
			this.txtSetupCounter.TabIndex = 8;
			this.txtSetupCounter.Text = "";
			// 
			// txtMaintainComp
			// 
			this.txtMaintainComp.Location = new System.Drawing.Point(232, 80);
			this.txtMaintainComp.Name = "txtMaintainComp";
			this.txtMaintainComp.TabIndex = 7;
			this.txtMaintainComp.Text = "";
			// 
			// txtSetupCompany
			// 
			this.txtSetupCompany.Location = new System.Drawing.Point(232, 56);
			this.txtSetupCompany.Name = "txtSetupCompany";
			this.txtSetupCompany.TabIndex = 6;
			this.txtSetupCompany.Text = "";
			// 
			// txtAUEC
			// 
			this.txtAUEC.Location = new System.Drawing.Point(232, 32);
			this.txtAUEC.Name = "txtAUEC";
			this.txtAUEC.TabIndex = 5;
			this.txtAUEC.Text = "";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(16, 128);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(144, 23);
			this.label18.TabIndex = 4;
			this.label18.Text = "Maintain Counter parties";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(16, 104);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(128, 23);
			this.label17.TabIndex = 3;
			this.label17.Text = "Set up counter parties";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(16, 80);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(120, 23);
			this.label16.TabIndex = 2;
			this.label16.Text = "Maintain Companies";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(16, 56);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(96, 16);
			this.label15.TabIndex = 1;
			this.label15.Text = "Set up company";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(16, 32);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(96, 16);
			this.label14.TabIndex = 0;
			this.label14.Text = "Maintain AUEC";
			// 
			// ultraTabPageControl3
			// 
			this.ultraTabPageControl3.Controls.Add(this.txtEmail);
			this.ultraTabPageControl3.Controls.Add(this.txtFaxTele);
			this.ultraTabPageControl3.Controls.Add(this.txtHomeTele);
			this.ultraTabPageControl3.Controls.Add(this.txtPagerTele);
			this.ultraTabPageControl3.Controls.Add(this.txtCellTele);
			this.ultraTabPageControl3.Controls.Add(this.txtWorkTele);
			this.ultraTabPageControl3.Controls.Add(this.txtLastName);
			this.ultraTabPageControl3.Controls.Add(this.txtFirstName);
			this.ultraTabPageControl3.Controls.Add(this.txtAddress2);
			this.ultraTabPageControl3.Controls.Add(this.txtAddress1);
			this.ultraTabPageControl3.Controls.Add(this.txtShortName);
			this.ultraTabPageControl3.Controls.Add(this.txtProductName);
			this.ultraTabPageControl3.Controls.Add(this.txtVendorName);
			this.ultraTabPageControl3.Controls.Add(this.label13);
			this.ultraTabPageControl3.Controls.Add(this.label12);
			this.ultraTabPageControl3.Controls.Add(this.label11);
			this.ultraTabPageControl3.Controls.Add(this.label10);
			this.ultraTabPageControl3.Controls.Add(this.label9);
			this.ultraTabPageControl3.Controls.Add(this.label8);
			this.ultraTabPageControl3.Controls.Add(this.label7);
			this.ultraTabPageControl3.Controls.Add(this.label6);
			this.ultraTabPageControl3.Controls.Add(this.label5);
			this.ultraTabPageControl3.Controls.Add(this.label4);
			this.ultraTabPageControl3.Controls.Add(this.label3);
			this.ultraTabPageControl3.Controls.Add(this.label2);
			this.ultraTabPageControl3.Controls.Add(this.label1);
			this.ultraTabPageControl3.Font = new System.Drawing.Font("Verdana", 8F);
			this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl3.Name = "ultraTabPageControl3";
			this.ultraTabPageControl3.Size = new System.Drawing.Size(460, 382);
			// 
			// txtEmail
			// 
			this.txtEmail.Location = new System.Drawing.Point(232, 320);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.TabIndex = 25;
			this.txtEmail.Text = "";
			// 
			// txtFaxTele
			// 
			this.txtFaxTele.Location = new System.Drawing.Point(232, 296);
			this.txtFaxTele.Name = "txtFaxTele";
			this.txtFaxTele.TabIndex = 24;
			this.txtFaxTele.Text = "";
			// 
			// txtHomeTele
			// 
			this.txtHomeTele.Location = new System.Drawing.Point(232, 272);
			this.txtHomeTele.Name = "txtHomeTele";
			this.txtHomeTele.TabIndex = 23;
			this.txtHomeTele.Text = "";
			// 
			// txtPagerTele
			// 
			this.txtPagerTele.Location = new System.Drawing.Point(232, 248);
			this.txtPagerTele.Name = "txtPagerTele";
			this.txtPagerTele.TabIndex = 22;
			this.txtPagerTele.Text = "";
			// 
			// txtCellTele
			// 
			this.txtCellTele.Location = new System.Drawing.Point(232, 224);
			this.txtCellTele.Name = "txtCellTele";
			this.txtCellTele.TabIndex = 21;
			this.txtCellTele.Text = "";
			// 
			// txtWorkTele
			// 
			this.txtWorkTele.Location = new System.Drawing.Point(232, 200);
			this.txtWorkTele.Name = "txtWorkTele";
			this.txtWorkTele.TabIndex = 20;
			this.txtWorkTele.Text = "";
			// 
			// txtLastName
			// 
			this.txtLastName.Location = new System.Drawing.Point(232, 176);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.TabIndex = 19;
			this.txtLastName.Text = "";
			// 
			// txtFirstName
			// 
			this.txtFirstName.Location = new System.Drawing.Point(232, 152);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.TabIndex = 18;
			this.txtFirstName.Text = "";
			// 
			// txtAddress2
			// 
			this.txtAddress2.Location = new System.Drawing.Point(232, 128);
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.TabIndex = 17;
			this.txtAddress2.Text = "";
			// 
			// txtAddress1
			// 
			this.txtAddress1.Location = new System.Drawing.Point(232, 104);
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.TabIndex = 16;
			this.txtAddress1.Text = "";
			// 
			// txtShortName
			// 
			this.txtShortName.Location = new System.Drawing.Point(232, 80);
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.TabIndex = 15;
			this.txtShortName.Text = "";
			// 
			// txtProductName
			// 
			this.txtProductName.Location = new System.Drawing.Point(232, 56);
			this.txtProductName.Name = "txtProductName";
			this.txtProductName.TabIndex = 14;
			this.txtProductName.Text = "";
			// 
			// txtVendorName
			// 
			this.txtVendorName.Location = new System.Drawing.Point(232, 32);
			this.txtVendorName.Name = "txtVendorName";
			this.txtVendorName.TabIndex = 13;
			this.txtVendorName.Text = "";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(24, 320);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(56, 16);
			this.label13.TabIndex = 12;
			this.label13.Text = "E-mail";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(24, 296);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(64, 16);
			this.label12.TabIndex = 11;
			this.label12.Text = "Fax #";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(24, 272);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 16);
			this.label11.TabIndex = 10;
			this.label11.Text = "Home Telephone #";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(24, 248);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 16);
			this.label10.TabIndex = 9;
			this.label10.Text = "Pager Telephone #";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(24, 224);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 8;
			this.label9.Text = "Cell Telephone #";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(24, 200);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 16);
			this.label8.TabIndex = 7;
			this.label8.Text = " Work Telephone #";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(24, 176);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "Contact Last Name";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(24, 152);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Contact - First Name";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Address2";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Address1";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Short Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Product";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Full Name of Vendor";
			// 
			// tabThirdPartyVendor
			// 
			this.tabThirdPartyVendor.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabSharedControlsPage1);
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl1);
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl2);
			this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl3);
			this.tabThirdPartyVendor.Font = new System.Drawing.Font("Verdana", 8F);
			this.tabThirdPartyVendor.Location = new System.Drawing.Point(184, 8);
			this.tabThirdPartyVendor.Name = "tabThirdPartyVendor";
			this.tabThirdPartyVendor.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.tabThirdPartyVendor.Size = new System.Drawing.Size(464, 416);
			this.tabThirdPartyVendor.TabIndex = 0;
			ultraTab1.Key = "AdminUserDetails";
			ultraTab1.TabPage = this.ultraTabPageControl1;
			ultraTab1.Text = "Admin User Details";
			ultraTab2.Key = "PermissionLevels";
			ultraTab2.TabPage = this.ultraTabPageControl2;
			ultraTab2.Text = "Permission Levels";
			ultraTab3.Key = "ThirdPartyVendor";
			ultraTab3.TabPage = this.ultraTabPageControl3;
			ultraTab3.Text = "Third Party Vendor";
			this.tabThirdPartyVendor.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																										  ultraTab1,
																										  ultraTab2,
																										  ultraTab3});
			this.tabThirdPartyVendor.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabThirdPartyVendor_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(460, 390);
			this.ultraTabSharedControlsPage1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabSharedControlsPage1_Paint);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 463);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(656, 22);
			this.statusBar1.TabIndex = 4;
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.Location = new System.Drawing.Point(352, 424);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(56, 24);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.trvUserRights);
			this.groupBox1.Controls.Add(this.btnAdd);
			this.groupBox1.Controls.Add(this.btnDelete);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(168, 440);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.Location = new System.Drawing.Point(88, 416);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(56, 24);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "Delete";
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnAdd.Location = new System.Drawing.Point(24, 416);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(56, 24);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add";
			// 
			// trvUserRights
			// 
			this.trvUserRights.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
			this.trvUserRights.Font = new System.Drawing.Font("Verdana", 8F);
			this.trvUserRights.Location = new System.Drawing.Point(8, 24);
			this.trvUserRights.Name = "trvUserRights";
			ultraTreeNode1.Text = "Admin - Users";
			ultraTreeNode2.Text = "Third Party Partners";
			this.trvUserRights.Nodes.AddRange(new Infragistics.Win.UltraWinTree.UltraTreeNode[] {
																									ultraTreeNode1,
																									ultraTreeNode2});
			this.trvUserRights.Size = new System.Drawing.Size(152, 384);
			this.trvUserRights.TabIndex = 0;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.Location = new System.Drawing.Point(408, 424);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(56, 24);
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "Close";
			// 
			// groupBox2
			// 
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
			this.groupBox2.Location = new System.Drawing.Point(8, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(448, 368);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			// 
			// txtHomeTeleA
			// 
			this.txtHomeTeleA.Location = new System.Drawing.Point(280, 310);
			this.txtHomeTeleA.Name = "txtHomeTeleA";
			this.txtHomeTeleA.TabIndex = 51;
			this.txtHomeTeleA.Text = "";
			// 
			// txtPagerTeleA
			// 
			this.txtPagerTeleA.Location = new System.Drawing.Point(280, 286);
			this.txtPagerTeleA.Name = "txtPagerTeleA";
			this.txtPagerTeleA.TabIndex = 50;
			this.txtPagerTeleA.Text = "";
			// 
			// txtCellTeleA
			// 
			this.txtCellTeleA.Location = new System.Drawing.Point(280, 262);
			this.txtCellTeleA.Name = "txtCellTeleA";
			this.txtCellTeleA.TabIndex = 49;
			this.txtCellTeleA.Text = "";
			// 
			// txtWorkTeleA
			// 
			this.txtWorkTeleA.Location = new System.Drawing.Point(280, 238);
			this.txtWorkTeleA.Name = "txtWorkTeleA";
			this.txtWorkTeleA.TabIndex = 48;
			this.txtWorkTeleA.Text = "";
			// 
			// txtEmailA
			// 
			this.txtEmailA.Location = new System.Drawing.Point(280, 214);
			this.txtEmailA.Name = "txtEmailA";
			this.txtEmailA.TabIndex = 47;
			this.txtEmailA.Text = "";
			// 
			// txtAddress2A
			// 
			this.txtAddress2A.Location = new System.Drawing.Point(280, 190);
			this.txtAddress2A.Name = "txtAddress2A";
			this.txtAddress2A.TabIndex = 46;
			this.txtAddress2A.Text = "";
			// 
			// txtAddress1A
			// 
			this.txtAddress1A.Location = new System.Drawing.Point(280, 166);
			this.txtAddress1A.Name = "txtAddress1A";
			this.txtAddress1A.TabIndex = 45;
			this.txtAddress1A.Text = "";
			// 
			// txtPasswordA
			// 
			this.txtPasswordA.Location = new System.Drawing.Point(280, 142);
			this.txtPasswordA.Name = "txtPasswordA";
			this.txtPasswordA.TabIndex = 44;
			this.txtPasswordA.Text = "";
			// 
			// txtLoginNameA
			// 
			this.txtLoginNameA.Location = new System.Drawing.Point(280, 118);
			this.txtLoginNameA.Name = "txtLoginNameA";
			this.txtLoginNameA.TabIndex = 43;
			this.txtLoginNameA.Text = "";
			// 
			// txtTitleA
			// 
			this.txtTitleA.Location = new System.Drawing.Point(280, 94);
			this.txtTitleA.Name = "txtTitleA";
			this.txtTitleA.TabIndex = 42;
			this.txtTitleA.Text = "";
			// 
			// txtShortNameA
			// 
			this.txtShortNameA.Location = new System.Drawing.Point(280, 70);
			this.txtShortNameA.Name = "txtShortNameA";
			this.txtShortNameA.TabIndex = 41;
			this.txtShortNameA.Text = "";
			// 
			// txtLastNameA
			// 
			this.txtLastNameA.Location = new System.Drawing.Point(280, 46);
			this.txtLastNameA.Name = "txtLastNameA";
			this.txtLastNameA.TabIndex = 40;
			this.txtLastNameA.Text = "";
			// 
			// txtFirstNameA
			// 
			this.txtFirstNameA.Location = new System.Drawing.Point(280, 22);
			this.txtFirstNameA.Name = "txtFirstNameA";
			this.txtFirstNameA.TabIndex = 39;
			this.txtFirstNameA.Text = "";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(70, 310);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(98, 16);
			this.label19.TabIndex = 38;
			this.label19.Text = "Home Telephone #";
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(70, 286);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(98, 16);
			this.label20.TabIndex = 37;
			this.label20.Text = "Pager Telephone #";
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(70, 262);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(104, 16);
			this.label21.TabIndex = 36;
			this.label21.Text = "Cell Telephone #";
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(70, 238);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(104, 16);
			this.label22.TabIndex = 35;
			this.label22.Text = "Work Telephone #";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(70, 214);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(104, 16);
			this.label23.TabIndex = 34;
			this.label23.Text = "E-Mail";
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(70, 190);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(104, 16);
			this.label24.TabIndex = 33;
			this.label24.Text = "Address 2";
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(70, 166);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(112, 16);
			this.label25.TabIndex = 32;
			this.label25.Text = "Address 1";
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(70, 142);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(122, 16);
			this.label26.TabIndex = 31;
			this.label26.Text = "Password";
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(70, 118);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(72, 16);
			this.label27.TabIndex = 30;
			this.label27.Text = "Login Name";
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(70, 94);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(72, 16);
			this.label28.TabIndex = 29;
			this.label28.Text = "Title";
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(70, 70);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(80, 16);
			this.label29.TabIndex = 28;
			this.label29.Text = "Short Name";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(70, 46);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(80, 16);
			this.label30.TabIndex = 27;
			this.label30.Text = "Last Name";
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(70, 22);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(122, 16);
			this.label31.TabIndex = 26;
			this.label31.Text = "First Name";
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(72, 336);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(80, 16);
			this.label32.TabIndex = 52;
			this.label32.Text = "Fax #";
			// 
			// txtFaxA
			// 
			this.txtFaxA.Location = new System.Drawing.Point(280, 336);
			this.txtFaxA.Name = "txtFaxA";
			this.txtFaxA.TabIndex = 53;
			this.txtFaxA.Text = "";
			// 
			// ThirdPartyVendor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(656, 485);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.tabThirdPartyVendor);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "ThirdPartyVendor";
			this.Text = "SLAU";
			this.ultraTabPageControl1.ResumeLayout(false);
			this.ultraTabPageControl2.ResumeLayout(false);
			this.ultraTabPageControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabThirdPartyVendor)).EndInit();
			this.tabThirdPartyVendor.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trvUserRights)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ultraTabSharedControlsPage1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void ultraTabPageControl1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void tabThirdPartyVendor_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
		
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveAdminUserDetails();
		}

		private bool SaveAdminUserDetails()
		{
			//bool result = false;

			//int slauID = (int)trvUserRights.SelectedNodes.Tag;

			Nirvana.Admin.BLL.User admin  = new Nirvana.Admin.BLL.User();
			
			admin.FirstName = txtFirstNameA.Text.Trim();
			admin.LastName = txtLastNameA.Text.Trim();
			admin.ShortName = txtShortNameA.Text.Trim();
			admin.Title = txtTitleA.Text.Trim();
			admin.LoginName = txtLoginNameA.Text.Trim();
			admin.Password = txtPasswordA.Text.Trim();
			admin.Address1 = txtAddress1A.Text.Trim();
			admin.Address2 = txtAddress2A.Text.Trim();
			admin.EMail = txtEmailA.Text.Trim();
			admin.TelephoneWork = txtWorkTeleA.Text.Trim();
			admin.TelephoneMobile = txtCellTeleA.Text.Trim();
			admin.TelephonePager = txtPagerTeleA.Text.Trim();
			admin.TelephoneHome = txtHomeTeleA.Text.Trim();
			admin.Fax = txtFaxA.Text.Trim();
			
			UserManager.AdminUserDetails(admin);
			return false;
		}
	}
}
