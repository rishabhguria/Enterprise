using Prana.Admin.BLL;
using Prana.BusinessObjects.Enums;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin
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

        const int SUPER_USER = 1;
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
        private Panel pnlUserPerrmissions;
        private TextBox txtCity;
        private Label lblCity;
        private Label label43;
        private IContainer components;


        //private Infragistics.Win.UltraWinTree.UltraTree trvUserRights;

        public ThirdPartyVendor()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            BindCountries();
            this.tabThirdPartyVendor.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabThirdPartyVendor_SelectedTabChanged);

            SetUpMenuPermissions();
        }

        private bool chkAddSLSU = false;
        private bool chkDeleteSLSU = false;
        private bool chkEditSLSU = false;
        //This method fetches the user permissions from the database.
        private void SetUpMenuPermissions()
        {
            ModuleResources module = ModuleResources.SLSU;
            AuthAction action = AuthAction.Write;
            var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);
            if (hasAccess)
            {
                chkAddSLSU = true;
                chkDeleteSLSU = true;
                chkEditSLSU = true;
            }

            Preferences preferences = Preferences.Instance;
            //chkAddSLSU = preferences.Add_SLSU;
            //chkDeleteSLSU = preferences.Delete_SLSU;
            //chkEditSLSU = preferences.Edit_SLSU;
            //If the user doesnt have the permissions to add or delete SLSU then the respecive Add or Delete buttons are
            //disabled so that he/she can't add or delete the SLSU.
            if (chkAddSLSU == false)
            {
                btnAdd.Enabled = false;
            }
            if (chkDeleteSLSU == false)
            {
                btnDelete.Enabled = false;
            }
            if (chkEditSLSU == false)
            {
                btnSave.Enabled = false;
            }
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
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (label15 != null)
                {
                    label15.Dispose();
                }
                if (label16 != null)
                {
                    label16.Dispose();
                }
                if (label17 != null)
                {
                    label17.Dispose();
                }
                if (label18 != null)
                {
                    label18.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label21 != null)
                {
                    label21.Dispose();
                }
                if (label20 != null)
                {
                    label20.Dispose();
                }
                if (label22 != null)
                {
                    label22.Dispose();
                }
                if (label23 != null)
                {
                    label23.Dispose();
                }
                if(label24 != null)
                {
                    label24.Dispose();
                }    
                if (label25 != null)
                {
                    label25.Dispose();
                }
                if (label26 != null)
                {
                    label26.Dispose();
                }
                if(label27 != null)
                {
                    label27.Dispose();
                }
                if(label28 != null)
                {
                    label28.Dispose();
                }
                if(label29 != null)
                {
                    label29.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if(label30 != null)
                {
                    label30.Dispose();
                }
                if(label31 != null)
                {
                    label31.Dispose();
                }
                if(label32 != null)
                {
                    label32.Dispose();
                }
                if (label33 != null)
                {
                    label33.Dispose();
                }
                if (label34 != null)
                {
                    label34.Dispose();
                }
                if (label35 != null)
                {
                    label35.Dispose();
                }
                if(label36 != null)
                {
                    label36.Dispose();
                }
                if(label37 != null)
                {
                    label37.Dispose();
                }
                if(label38 != null)
                {
                    label38.Dispose();
                }
                if(label39 != null)
                {
                    label39.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if(label40 != null)
                {
                    label40.Dispose();
                }
                if(label41 != null)
                {
                    label41.Dispose();
                }
                if(label42 != null)
                {
                    label42.Dispose();
                }
                if (label43 != null)
                {
                    label43.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if(lblCity != null)
                {
                    lblCity.Dispose();
                }
                if(lblStateTerrirtory != null)
                {
                    lblStateTerrirtory.Dispose();
                }
                if(lblZip != null)
                {
                    lblZip.Dispose();
                }
                if (btnAdd != null)
                {
                    btnAdd.Dispose();
                }
                if(btnClose != null)
                {
                    btnClose.Dispose();
                }
                if(btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if(btnSave != null)
                {
                    btnSave.Dispose();
                }
                if(txtAddress1 != null)
                {
                    txtAddress1.Dispose();
                }
                if(txtAddress1A != null)
                {
                    txtAddress1A.Dispose();
                }
                if(txtAddress2 != null)
                {
                    txtAddress2.Dispose();
                }
                if(txtAddress2A != null)
                {
                    txtAddress2A.Dispose();
                }
                if(txtCellTele != null)
                {
                    txtCellTele.Dispose();
                }
                if(txtCellTeleA != null)
                {
                    txtCellTeleA.Dispose();
                }
                if(txtCity != null)
                {
                    txtCity.Dispose();
                }
                if(txtComment != null)
                {
                    txtComment.Dispose();
                }
                if(txtEmail != null)
                {
                    txtEmail.Dispose();
                }
                if(txtEmailA != null)
                {
                    txtEmailA.Dispose();
                }
                if(txtFaxA != null)
                {
                    txtFaxA.Dispose();
                }
                if(txtFaxTele != null)
                {
                    txtFaxTele.Dispose();
                }
                if(txtFirstName != null)
                {
                    txtFirstName.Dispose();
                }
                if(txtFirstNameA != null)
                {
                    txtFirstNameA.Dispose();
                }
                if(txtHomeTele != null)
                {
                    txtHomeTele.Dispose();
                }
                if(txtHomeTeleA != null)
                {
                    txtHomeTeleA.Dispose();
                }
                if(txtLastName != null)
                {
                    txtLastName.Dispose();
                }
                if(txtLastNameA != null)
                {
                    txtLastNameA.Dispose();
                }
                if(txtLoginNameA != null)
                {
                    txtLoginNameA.Dispose();
                }
                if(txtMailingAddressT != null)
                {
                    txtMailingAddressT.Dispose();
                }
                if(txtPagerTele != null)
                {
                    txtPagerTele.Dispose();
                }
                if(txtPagerTeleA != null)
                {
                    txtPagerTeleA.Dispose();
                }
                if(txtPasswordA != null)
                {
                    txtPasswordA.Dispose();
                }
                if(txtProductName != null)
                {
                    txtProductName.Dispose();
                }
                if(txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if(txtShortNameA != null)
                {
                    txtShortNameA.Dispose();
                }
                if(txtTitle != null)
                {
                    txtTitle.Dispose();
                }
                if(txtTitleA != null)
                {
                    txtTitleA.Dispose();
                }
                if(txtVendorName != null)
                {
                    txtVendorName.Dispose();
                }
                if(txtWorkTele != null)
                {
                    txtWorkTele.Dispose();
                }
                if(txtWorkTeleA != null)
                {
                    txtWorkTeleA.Dispose();
                }
                if(txtZip != null)
                {
                    txtZip.Dispose();
                }
                if(groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if(groupBox2 != null)
                {
                    groupBox2.Dispose();
                }
                if(groupBox3 != null)
                {
                    groupBox3.Dispose();
                }
                if(groupBox4 != null)
                {
                    groupBox4.Dispose();
                }
                if(ultraTabPageControl1 != null)
                {
                    ultraTabPageControl1.Dispose();
                }
                if(ultraTabPageControl2 != null)
                {
                    ultraTabPageControl2.Dispose();
                }
                if(ultraTabPageControl3 != null)
                {
                    ultraTabPageControl3.Dispose();
                }
                if(ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if(tabThirdPartyVendor != null)
                {
                    tabThirdPartyVendor.Dispose();
                }
                if(trvUserRights != null)
                {
                    trvUserRights.Dispose();
                }
                if(cmbCountry != null)
                {
                    cmbCountry.Dispose();
                }
                if(cmbState != null)
                {
                    cmbState.Dispose();
                }
                if(pnlUserPerrmissions != null)
                {
                    pnlUserPerrmissions.Dispose();
                }
                if(chkPermissionAUEC != null)
                {
                    chkPermissionAUEC.Dispose();
                }
                if(chkPermissionCompany != null)
                {
                    chkPermissionCompany.Dispose();
                }
                if(chkPermissionCounterParties != null)
                {
                    chkPermissionCounterParties.Dispose();
                }
                if(chkPermissionMaintainCompanis != null)
                {
                    chkPermissionMaintainCompanis.Dispose();
                }
                if(errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateName", 2);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThirdPartyVendor));
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label43 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
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
            this.pnlUserPerrmissions = new System.Windows.Forms.Panel();
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
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ultraTabPageControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.pnlUserPerrmissions.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabThirdPartyVendor)).BeginInit();
            this.tabThirdPartyVendor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.groupBox2);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(456, 427);
            this.ultraTabPageControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl1_Paint);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label43);
            this.groupBox2.Controls.Add(this.txtCity);
            this.groupBox2.Controls.Add(this.lblCity);
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
            this.groupBox2.Location = new System.Drawing.Point(2, -3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 425);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // label43
            // 
            this.label43.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(32, 239);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(8, 8);
            this.label43.TabIndex = 170;
            this.label43.Text = "*";
            // 
            // txtCity
            // 
            this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCity.Location = new System.Drawing.Point(120, 237);
            this.txtCity.MaxLength = 50;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(200, 21);
            this.txtCity.TabIndex = 11;
            this.txtCity.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtCity.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // lblCity
            // 
            this.lblCity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCity.Location = new System.Drawing.Point(4, 239);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(28, 16);
            this.lblCity.TabIndex = 169;
            this.lblCity.Text = "City";
            // 
            // cmbState
            // 
            this.cmbState.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 0;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.cmbState.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance1.BorderColor = System.Drawing.Color.Silver;
            this.cmbState.DisplayLayout.Override.RowAppearance = appearance1;
            this.cmbState.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbState.DropDownWidth = 0;
            this.cmbState.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbState.LimitToList = true;
            this.cmbState.Location = new System.Drawing.Point(120, 214);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(200, 21);
            this.cmbState.TabIndex = 10;
            this.cmbState.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbState.GotFocus += new System.EventHandler(this.cmbState_GotFocus);
            this.cmbState.LostFocus += new System.EventHandler(this.cmbState_LostFocus);
            // 
            // cmbCountry
            // 
            this.cmbCountry.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn5.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5});
            this.cmbCountry.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance2.BorderColor = System.Drawing.Color.Silver;
            this.cmbCountry.DisplayLayout.Override.RowAppearance = appearance2;
            this.cmbCountry.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCountry.DropDownWidth = 0;
            this.cmbCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCountry.LimitToList = true;
            this.cmbCountry.Location = new System.Drawing.Point(120, 191);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(200, 21);
            this.cmbCountry.TabIndex = 9;
            this.cmbCountry.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
            this.cmbCountry.GotFocus += new System.EventHandler(this.cmbCountry_GotFocus);
            this.cmbCountry.LostFocus += new System.EventHandler(this.cmbCountry_LostFocus);
            // 
            // label42
            // 
            this.label42.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label42.ForeColor = System.Drawing.Color.Red;
            this.label42.Location = new System.Drawing.Point(102, 215);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(8, 8);
            this.label42.TabIndex = 165;
            this.label42.Text = "*";
            // 
            // label40
            // 
            this.label40.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label40.Location = new System.Drawing.Point(4, 317);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(102, 16);
            this.label40.TabIndex = 162;
            this.label40.Text = "(1-111-111111)";
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label34.ForeColor = System.Drawing.Color.Red;
            this.label34.Location = new System.Drawing.Point(42, 337);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(8, 8);
            this.label34.TabIndex = 161;
            this.label34.Text = "*";
            // 
            // txtZip
            // 
            this.txtZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtZip.Location = new System.Drawing.Point(120, 259);
            this.txtZip.MaxLength = 50;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(200, 21);
            this.txtZip.TabIndex = 12;
            this.txtZip.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtZip.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // lblZip
            // 
            this.lblZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblZip.Location = new System.Drawing.Point(4, 261);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(28, 16);
            this.lblZip.TabIndex = 159;
            this.lblZip.Text = "Zip";
            // 
            // lblStateTerrirtory
            // 
            this.lblStateTerrirtory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStateTerrirtory.Location = new System.Drawing.Point(4, 215);
            this.lblStateTerrirtory.Name = "lblStateTerrirtory";
            this.lblStateTerrirtory.Size = new System.Drawing.Size(98, 16);
            this.lblStateTerrirtory.TabIndex = 157;
            this.lblStateTerrirtory.Text = "State/Territory";
            this.lblStateTerrirtory.Click += new System.EventHandler(this.lblStateTerrirtory_Click);
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label33.Location = new System.Drawing.Point(4, 191);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(54, 16);
            this.label33.TabIndex = 156;
            this.label33.Text = "Country";
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label41.ForeColor = System.Drawing.Color.Red;
            this.label41.Location = new System.Drawing.Point(44, 283);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(8, 8);
            this.label41.TabIndex = 154;
            this.label41.Text = "*";
            // 
            // label39
            // 
            this.label39.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label39.ForeColor = System.Drawing.Color.Red;
            this.label39.Location = new System.Drawing.Point(56, 305);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(8, 8);
            this.label39.TabIndex = 152;
            this.label39.Text = "*";
            // 
            // label38
            // 
            this.label38.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label38.ForeColor = System.Drawing.Color.Red;
            this.label38.Location = new System.Drawing.Point(58, 191);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(8, 8);
            this.label38.TabIndex = 151;
            this.label38.Text = "*";
            // 
            // label37
            // 
            this.label37.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label37.ForeColor = System.Drawing.Color.Red;
            this.label37.Location = new System.Drawing.Point(64, 147);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(8, 8);
            this.label37.TabIndex = 150;
            this.label37.Text = "*";
            // 
            // label36
            // 
            this.label36.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label36.ForeColor = System.Drawing.Color.Red;
            this.label36.Location = new System.Drawing.Point(62, 125);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(8, 8);
            this.label36.TabIndex = 149;
            this.label36.Text = "*";
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label35.ForeColor = System.Drawing.Color.Red;
            this.label35.Location = new System.Drawing.Point(36, 103);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(8, 8);
            this.label35.TabIndex = 148;
            this.label35.Text = "*";
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(74, 59);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(8, 8);
            this.label18.TabIndex = 146;
            this.label18.Text = "*";
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(70, 15);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(8, 8);
            this.label17.TabIndex = 145;
            this.label17.Text = "*";
            // 
            // txtFaxA
            // 
            this.txtFaxA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFaxA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFaxA.Location = new System.Drawing.Point(120, 401);
            this.txtFaxA.MaxLength = 50;
            this.txtFaxA.Name = "txtFaxA";
            this.txtFaxA.Size = new System.Drawing.Size(200, 21);
            this.txtFaxA.TabIndex = 18;
            this.txtFaxA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtFaxA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // label32
            // 
            this.label32.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label32.Location = new System.Drawing.Point(4, 403);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(38, 16);
            this.label32.TabIndex = 141;
            this.label32.Text = "Fax #";
            // 
            // txtHomeTeleA
            // 
            this.txtHomeTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHomeTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtHomeTeleA.Location = new System.Drawing.Point(120, 379);
            this.txtHomeTeleA.MaxLength = 50;
            this.txtHomeTeleA.Name = "txtHomeTeleA";
            this.txtHomeTeleA.Size = new System.Drawing.Size(200, 21);
            this.txtHomeTeleA.TabIndex = 17;
            this.txtHomeTeleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtHomeTeleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtPagerTeleA
            // 
            this.txtPagerTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPagerTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPagerTeleA.Location = new System.Drawing.Point(120, 357);
            this.txtPagerTeleA.MaxLength = 50;
            this.txtPagerTeleA.Name = "txtPagerTeleA";
            this.txtPagerTeleA.Size = new System.Drawing.Size(200, 21);
            this.txtPagerTeleA.TabIndex = 16;
            this.txtPagerTeleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtPagerTeleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtCellTeleA
            // 
            this.txtCellTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCellTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCellTeleA.Location = new System.Drawing.Point(120, 335);
            this.txtCellTeleA.MaxLength = 50;
            this.txtCellTeleA.Name = "txtCellTeleA";
            this.txtCellTeleA.Size = new System.Drawing.Size(200, 21);
            this.txtCellTeleA.TabIndex = 15;
            this.txtCellTeleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtCellTeleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtWorkTeleA
            // 
            this.txtWorkTeleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWorkTeleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtWorkTeleA.Location = new System.Drawing.Point(120, 303);
            this.txtWorkTeleA.MaxLength = 50;
            this.txtWorkTeleA.Name = "txtWorkTeleA";
            this.txtWorkTeleA.Size = new System.Drawing.Size(200, 21);
            this.txtWorkTeleA.TabIndex = 14;
            this.txtWorkTeleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtWorkTeleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtEmailA
            // 
            this.txtEmailA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEmailA.Location = new System.Drawing.Point(120, 281);
            this.txtEmailA.MaxLength = 50;
            this.txtEmailA.Name = "txtEmailA";
            this.txtEmailA.Size = new System.Drawing.Size(200, 21);
            this.txtEmailA.TabIndex = 13;
            this.txtEmailA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtEmailA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtAddress2A
            // 
            this.txtAddress2A.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress2A.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress2A.Location = new System.Drawing.Point(120, 167);
            this.txtAddress2A.MaxLength = 50;
            this.txtAddress2A.Name = "txtAddress2A";
            this.txtAddress2A.Size = new System.Drawing.Size(200, 21);
            this.txtAddress2A.TabIndex = 8;
            this.txtAddress2A.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtAddress2A.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtAddress1A
            // 
            this.txtAddress1A.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress1A.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress1A.Location = new System.Drawing.Point(120, 145);
            this.txtAddress1A.MaxLength = 50;
            this.txtAddress1A.Name = "txtAddress1A";
            this.txtAddress1A.Size = new System.Drawing.Size(200, 21);
            this.txtAddress1A.TabIndex = 7;
            this.txtAddress1A.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtAddress1A.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtPasswordA
            // 
            this.txtPasswordA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPasswordA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPasswordA.Location = new System.Drawing.Point(120, 123);
            this.txtPasswordA.MaxLength = 20;
            this.txtPasswordA.Name = "txtPasswordA";
            this.txtPasswordA.Size = new System.Drawing.Size(200, 21);
            this.txtPasswordA.TabIndex = 6;
            this.txtPasswordA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtPasswordA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtLoginNameA
            // 
            this.txtLoginNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLoginNameA.Location = new System.Drawing.Point(120, 101);
            this.txtLoginNameA.MaxLength = 20;
            this.txtLoginNameA.Name = "txtLoginNameA";
            this.txtLoginNameA.Size = new System.Drawing.Size(200, 21);
            this.txtLoginNameA.TabIndex = 5;
            this.txtLoginNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtLoginNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtTitleA
            // 
            this.txtTitleA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitleA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTitleA.Location = new System.Drawing.Point(120, 79);
            this.txtTitleA.MaxLength = 50;
            this.txtTitleA.Name = "txtTitleA";
            this.txtTitleA.Size = new System.Drawing.Size(200, 21);
            this.txtTitleA.TabIndex = 4;
            this.txtTitleA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtTitleA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtShortNameA
            // 
            this.txtShortNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortNameA.Location = new System.Drawing.Point(120, 57);
            this.txtShortNameA.MaxLength = 50;
            this.txtShortNameA.Name = "txtShortNameA";
            this.txtShortNameA.Size = new System.Drawing.Size(200, 21);
            this.txtShortNameA.TabIndex = 3;
            this.txtShortNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtShortNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtLastNameA
            // 
            this.txtLastNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLastNameA.Location = new System.Drawing.Point(120, 35);
            this.txtLastNameA.MaxLength = 50;
            this.txtLastNameA.Name = "txtLastNameA";
            this.txtLastNameA.Size = new System.Drawing.Size(200, 21);
            this.txtLastNameA.TabIndex = 2;
            this.txtLastNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtLastNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtFirstNameA
            // 
            this.txtFirstNameA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFirstNameA.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFirstNameA.Location = new System.Drawing.Point(120, 13);
            this.txtFirstNameA.MaxLength = 50;
            this.txtFirstNameA.Name = "txtFirstNameA";
            this.txtFirstNameA.Size = new System.Drawing.Size(200, 21);
            this.txtFirstNameA.TabIndex = 1;
            this.txtFirstNameA.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtFirstNameA.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label19.Location = new System.Drawing.Point(4, 381);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 16);
            this.label19.TabIndex = 127;
            this.label19.Text = "Home #";
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label20.Location = new System.Drawing.Point(4, 359);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(52, 16);
            this.label20.TabIndex = 126;
            this.label20.Text = "Pager #";
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label21.Location = new System.Drawing.Point(4, 337);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(40, 16);
            this.label21.TabIndex = 125;
            this.label21.Text = "Cell #";
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label22.Location = new System.Drawing.Point(4, 305);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(52, 16);
            this.label22.TabIndex = 124;
            this.label22.Text = "Work #";
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label23.Location = new System.Drawing.Point(4, 283);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 16);
            this.label23.TabIndex = 123;
            this.label23.Text = "E-Mail";
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label24.Location = new System.Drawing.Point(4, 169);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(64, 16);
            this.label24.TabIndex = 122;
            this.label24.Text = "Address 2";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label25.Location = new System.Drawing.Point(4, 147);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(64, 16);
            this.label25.TabIndex = 121;
            this.label25.Text = "Address 1";
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label26.Location = new System.Drawing.Point(4, 125);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(62, 16);
            this.label26.TabIndex = 120;
            this.label26.Text = "Password";
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label27.Location = new System.Drawing.Point(4, 103);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(36, 16);
            this.label27.TabIndex = 119;
            this.label27.Text = "Login";
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label28.Location = new System.Drawing.Point(4, 81);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(30, 16);
            this.label28.TabIndex = 118;
            this.label28.Text = "Title";
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label29.Location = new System.Drawing.Point(4, 59);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(74, 16);
            this.label29.TabIndex = 117;
            this.label29.Text = "Short Name";
            // 
            // label30
            // 
            this.label30.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label30.Location = new System.Drawing.Point(4, 37);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(66, 16);
            this.label30.TabIndex = 116;
            this.label30.Text = "Last Name";
            // 
            // label31
            // 
            this.label31.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label31.Location = new System.Drawing.Point(4, 15);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(66, 16);
            this.label31.TabIndex = 115;
            this.label31.Text = "First Name";
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.AutoScroll = true;
            this.ultraTabPageControl2.Controls.Add(this.pnlUserPerrmissions);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(456, 427);
            // 
            // pnlUserPerrmissions
            // 
            this.pnlUserPerrmissions.Controls.Add(this.groupBox4);
            this.pnlUserPerrmissions.Location = new System.Drawing.Point(5, 3);
            this.pnlUserPerrmissions.Name = "pnlUserPerrmissions";
            this.pnlUserPerrmissions.Size = new System.Drawing.Size(428, 700);
            this.pnlUserPerrmissions.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkPermissionCounterParties);
            this.groupBox4.Controls.Add(this.chkPermissionMaintainCompanis);
            this.groupBox4.Controls.Add(this.chkPermissionCompany);
            this.groupBox4.Controls.Add(this.chkPermissionAUEC);
            this.groupBox4.Location = new System.Drawing.Point(298, 328);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(10, 18);
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
            this.chkPermissionCompany.Text = "Set up Client";
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
            this.ultraTabPageControl3.Size = new System.Drawing.Size(493, 447);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
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
            this.txtMailingAddressT.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtMailingAddressT.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtComment
            // 
            this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComment.Location = new System.Drawing.Point(200, 349);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(232, 20);
            this.txtComment.TabIndex = 156;
            this.txtComment.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtComment.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // txtTitle
            // 
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Location = new System.Drawing.Point(200, 325);
            this.txtTitle.MaxLength = 50;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(232, 21);
            this.txtTitle.TabIndex = 155;
            this.txtTitle.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtTitle.LostFocus += new System.EventHandler(this.ControlLostFocus);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(24, 370);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 23);
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
            this.txtEmail.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtEmail.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtFaxTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtFaxTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtHomeTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtHomeTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtPagerTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtPagerTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtCellTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtCellTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtWorkTele.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtWorkTele.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtLastName.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtLastName.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtFirstName.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtFirstName.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtAddress2.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtAddress2.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtAddress1.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtAddress1.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtShortName.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtProductName.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtProductName.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            this.txtVendorName.GotFocus += new System.EventHandler(this.ControlGotFocus);
            this.txtVendorName.LostFocus += new System.EventHandler(this.ControlLostFocus);
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
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabThirdPartyVendor.ActiveTabAppearance = appearance3;
            this.tabThirdPartyVendor.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.tabThirdPartyVendor.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl1);
            this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl2);
            this.tabThirdPartyVendor.Controls.Add(this.ultraTabPageControl3);
            this.tabThirdPartyVendor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tabThirdPartyVendor.Location = new System.Drawing.Point(174, 0);
            this.tabThirdPartyVendor.Name = "tabThirdPartyVendor";
            this.tabThirdPartyVendor.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabThirdPartyVendor.Size = new System.Drawing.Size(458, 448);
            this.tabThirdPartyVendor.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabThirdPartyVendor.TabIndex = 0;
            appearance4.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ActiveAppearance = appearance4;
            ultraTab1.Key = "AdminUserDetails";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Details";
            appearance5.ForeColor = System.Drawing.Color.Black;
            ultraTab2.ActiveAppearance = appearance5;
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
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(456, 427);
            this.ultraTabSharedControlsPage1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabSharedControlsPage1_Paint);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(276, 453);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trvUserRights);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 448);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // trvUserRights
            // 
            this.trvUserRights.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvUserRights.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvUserRights.HideSelection = false;
            this.trvUserRights.Location = new System.Drawing.Point(8, 16);
            this.trvUserRights.Name = "trvUserRights";
            this.trvUserRights.ShowLines = false;
            this.trvUserRights.Size = new System.Drawing.Size(152, 426);
            this.trvUserRights.TabIndex = 0;
            this.trvUserRights.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvUserRights_AfterSelect);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Location = new System.Drawing.Point(14, 453);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Location = new System.Drawing.Point(92, 453);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Location = new System.Drawing.Point(354, 453);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ThirdPartyVendor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(639, 481);
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
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.pnlUserPerrmissions.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabThirdPartyVendor)).EndInit();
            this.tabThirdPartyVendor.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
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
                NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;

                if (nodeDetails.Type == NodeType.User)
                {
                    int nodeID = SaveUserDetails();
                    if (nodeID > 0)
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
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);

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
            NodeDetails node = (NodeDetails)trvUserRights.SelectedNode.Tag;
            //Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
            //Match emailMatch = emailRegex.Match(txtEmailA.Text.ToString());
            string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex emailRe = new Regex(emailCheck);
            Match emailMatch = emailRe.Match(txtEmailA.Text.ToString());

            errorProvider1.SetError(txtFirstNameA, "");
            errorProvider1.SetError(txtShortNameA, "");
            errorProvider1.SetError(txtLoginNameA, "");
            errorProvider1.SetError(txtPasswordA, "");
            errorProvider1.SetError(txtAddress1A, "");
            errorProvider1.SetError(cmbCountry, "");
            errorProvider1.SetError(cmbState, "");
            errorProvider1.SetError(txtEmailA, "");
            errorProvider1.SetError(txtWorkTeleA, "");
            errorProvider1.SetError(txtCellTeleA, "");
            errorProvider1.SetError(txtCity, "");

            if (txtFirstNameA.Text.Trim() == "")
            {
                errorProvider1.SetError(txtFirstNameA, "Please enter First Name!");
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                txtFirstNameA.Focus();
            }
            else if (txtShortNameA.Text.Trim() == "")
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtShortNameA, "Please enter Short Name!");
                txtShortNameA.Focus();
            }
            else if (txtLoginNameA.Text.Trim() == "")
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtLoginNameA, "Please enter Login Name!");
                txtLoginNameA.Focus();
            }
            else if (txtPasswordA.Text.Trim() == "")
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtPasswordA, "Please enter Password!");
                txtPasswordA.Focus();
            }
            else if (int.Parse(txtPasswordA.Text.Length.ToString()) < 4)
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtPasswordA, "Please enter password having at least four characters !");
                txtPasswordA.Focus();
            }
            else if (txtAddress1A.Text.Trim() == "")
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtAddress1A, "Please enter Address1!");
                txtAddress1A.Focus();
            }
            else if (int.Parse(cmbCountry.Value.ToString()) == int.MinValue)
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(cmbCountry, "Please select Country!");
                cmbCountry.Focus();
            }
            else if (int.Parse(cmbState.Value.ToString()) == int.MinValue)
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(cmbState, "Please select State!");
                cmbState.Focus();
            }
            else if (txtCity.Text.Trim() == "")
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtCity, "Please enter City!");
                txtCity.Focus();
            }
            //else if(txtEmailA.Text.Trim() == "")
            else if (!emailMatch.Success)
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtEmailA, "Please enter valid Email address!");
                txtEmailA.Focus();
            }
            else if (txtWorkTeleA.Text.Trim() == "")
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtWorkTeleA, "Please enter Work Telephone!");
                txtWorkTeleA.Focus();
            }
            else if (txtCellTeleA.Text.Trim() == "")
            {
                tabThirdPartyVendor.SelectedTab = tabThirdPartyVendor.Tabs[C_TAB_USER];
                errorProvider1.SetError(txtCellTeleA, "Please enter Mobile No!");
                txtCellTeleA.Focus();
            }
            else
            {
                Prana.Admin.BLL.User user = new Prana.Admin.BLL.User();

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
                user.City = txtCity.Text.Trim();

                user.UserID = node.NodeID;

                int newUserID = UserManager.SaveUser(user);
                if (newUserID == -1)
                {
                    MessageBox.Show("User with the same short name or login name already exists.", "Prana Alert", MessageBoxButtons.OK);
                }
                else
                {
                    SaveUserPermission(newUserID);
                    //					Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                    //					Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Saved!");
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
            //permissions.Add(new Permission(int.Parse(chkPermissionAUEC.Tag.ToString()), chkPermissionAUEC.Checked));
            //permissions.Add(new Permission(int.Parse(chkPermissionCompany.Tag.ToString()), chkPermissionCompany.Checked));
            //permissions.Add(new Permission(int.Parse(chkPermissionMaintainCompanis.Tag.ToString()), chkPermissionMaintainCompanis.Checked));
            //permissions.Add(new Permission(int.Parse(chkPermissionCounterParties.Tag.ToString()), chkPermissionCounterParties.Checked));

            //UserManager.AddPermissions(userID, permissions);
            CheckBox chkCtr = new CheckBox();
            foreach (Control ctr in pnlUserPerrmissions.Controls)
            {
                //MessageBox.Show(ctr.Name);
                Type type = ctr.GetType();
                switch (type.ToString())
                {
                    case "System.Windows.Forms.CheckBox":
                        //MessageBox.Show(ty.ToString());
                        chkCtr = (CheckBox)ctr;
                        permissions.Add(new Permission(int.Parse(ctr.Tag.ToString()), chkCtr.Checked));
                        break;

                }
                //    MessageBox.Show(ty.ToString());
            }
            if (_loggedUser.UserID == SUPER_USER || (UserManager.IsAdmin(_loggedUser.UserID)))
            {
                UserManager.AddPermissions(userID, permissions);
            }
        }

        /// <summary>
        /// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
        /// </summary>
        /// <param name="nodeDetails"></param>
        private void SelectNode(NodeType nodeType, int nodeID)
        {
            foreach (TreeNode node in trvUserRights.Nodes)
            {
                NodeDetails nodeDetails = (NodeDetails)node.Tag;
                if (nodeDetails.Type == nodeType)
                {
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        NodeDetails subNodeDetails = (NodeDetails)subNode.Tag;
                        if (subNodeDetails.NodeID == nodeID)
                        {
                            trvUserRights.SelectedNode = subNode;
                        }
                    }
                }
            }
        }

        Prana.Admin.BLL.User _loggedUser = null;
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
            _loggedUser = UserManager.GetUser(Preferences.Instance.UserID);
            LoadUserPermissions();

            DisablePermissionSelection();
        }


        private void DisablePermissionSelection()
        {
            //Prana.Admin.BLL.User loggedUser = UserManager.GetUser(Preferences.Instance.UserID);
            if (_loggedUser.UserID != SUPER_USER && !(UserManager.IsAdmin(_loggedUser.UserID)))
            {
                CheckBox chkCtr = new CheckBox();
                foreach (Control ctr in pnlUserPerrmissions.Controls)
                {
                    Type type = ctr.GetType();
                    switch (type.ToString())
                    {
                        case "System.Windows.Forms.CheckBox":
                            chkCtr = (CheckBox)ctr;
                            chkCtr.Enabled = false;
                            break;
                    }
                }
            }
        }

        private void LoadUserPermissions()
        {
            //Label lblTest = new Label();
            //lblTest.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            //lblTest.Location = new System.Drawing.Point(50, 115);
            //lblTest.Name = "lblTestName";
            //lblTest.Size = new System.Drawing.Size(100, 50);
            //lblTest.Text = "Monu";
            //if (lblTest != null)
            //{
            //    //lblTest = new Label();
            //    pnlUserPerrmissions.Controls.Add(lblTest);
            //}

            //CheckBox chkSLSU = new CheckBox();
            //chkSLSU.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            //chkSLSU.Location = new System.Drawing.Point(50, 115);
            //chkSLSU.Name = "ckhSLSU";
            //chkSLSU.Size = new System.Drawing.Size(100, 50);
            //chkSLSU.Text = "Monu";
            //if (chkSLSU != null)
            //{
            //    //lblTest = new Label();
            //    pnlUserPerrmissions.Controls.Add(chkSLSU);
            //}

            Permissions userPermissions = new Permissions();
            userPermissions = PermissionManager.GetPermissions();

            int locXModule = 1;
            int locYModule = 20;
            int locXPermissionType = 170;
            int locYPermissionType = 3;

            int index = 1;
            string moduleName = string.Empty;
            string checkModuleName = string.Empty;

            string permissionName = string.Empty;
            string checkPermissionName = string.Empty;
            string validPermissionName = string.Empty;
            string lblModuleName = string.Empty;
            foreach (Permission permission in userPermissions)
            {
                checkModuleName = permission.ModuleName;
                break;
            }

            CheckBox chkSLSU1 = new CheckBox();
            foreach (Permission permission in userPermissions)
            {
                validPermissionName = permission.ModuleName.Replace(' ', '_');
                Label lblPermissionName = new Label();
                lblPermissionName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
                lblPermissionName.Location = new System.Drawing.Point(locXModule, locYModule);
                lblPermissionName.Name = "lbl" + validPermissionName;
                lblPermissionName.Size = new System.Drawing.Size(100, 50);
                lblPermissionName.Text = permission.ModuleName;
                pnlUserPerrmissions.Controls.Add(lblPermissionName);

                string permissionTypeName = permission.PermissionTypeName;



                if (checkModuleName != permission.ModuleName && index != 1)
                {
                    locYModule += 50;
                    //locYPermissionType += 50;

                    locXPermissionType = 170;
                    locYPermissionType += 50;

                    //CheckBox chkSLSU1 = new CheckBox();
                    chkSLSU1 = new CheckBox();
                    chkSLSU1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
                    chkSLSU1.Location = new System.Drawing.Point(locXPermissionType, locYPermissionType);
                    chkSLSU1.Name = "chk" + permissionTypeName + validPermissionName;
                    chkSLSU1.Size = new System.Drawing.Size(60, 50);
                    chkSLSU1.Tag = permission.PermissionID;
                    chkSLSU1.Text = permissionTypeName;
                    pnlUserPerrmissions.Controls.Add(chkSLSU1);

                    locXPermissionType += 60;
                    chkSLSU1.CheckStateChanged += new System.EventHandler(chkSLSU1_CheckStateChanged);
                }
                else
                {
                    //CheckBox chkSLSU1 = new CheckBox();
                    chkSLSU1 = new CheckBox();
                    chkSLSU1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
                    chkSLSU1.Location = new System.Drawing.Point(locXPermissionType, locYPermissionType);
                    chkSLSU1.Name = "chk" + permissionTypeName + validPermissionName;
                    chkSLSU1.Size = new System.Drawing.Size(60, 50);
                    chkSLSU1.Tag = permission.PermissionID;
                    chkSLSU1.Text = permissionTypeName;
                    pnlUserPerrmissions.Controls.Add(chkSLSU1);

                    locXPermissionType += 60;
                    chkSLSU1.CheckStateChanged += new System.EventHandler(chkSLSU1_CheckStateChanged);

                }





                moduleName = permission.ModuleName;
                checkModuleName = moduleName;

                //if (checkPermissionName != permission.PermissionTypeName && index != 0)
                //{
                //    locXPermissionType += 50;
                //}
                //permissionTypeName = permission.PermissionTypeName;
                //checkPermissionName = permissionTypeName;
                if (index % 4 == 0)
                {
                    //locXPermissionType = 170;
                    //locYPermissionType += 50;
                }
                index++;
                if (locYModule >= 600)
                {
                    pnlUserPerrmissions.Height += 50;
                }
                //chkSLSU1.CheckStateChanged -= new System.EventHandler(this.chkSLSU1_CheckStateChanged);
            }
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
            cmbCountry.DataSource = null;
            cmbCountry.DataSource = countries;
            cmbCountry.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCountry.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("Name"))
                {
                    column.Hidden = true;
                }
            }
            cmbCountry.DisplayLayout.Bands[0].ColHeadersVisible = false;

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
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("StateName"))
                {
                    column.Hidden = true;
                }
            }
            cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("StateName"))
                {
                    column.Hidden = true;
                }
            }
            cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        /// <summary>
        /// Bind left tree to th User and Vendor data.
        /// </summary>
        private void BindUserTree()
        {
            try
            {
                int parentNodeID = 0;
                //int selectedNodeID = 0;
                bool gotFirstNode = false;
                Font font = new Font("Vedana", 10, System.Drawing.FontStyle.Bold);

                trvUserRights.Nodes.Clear();

                //if(UserManager.IsAdmin(Preferences.Instance.UserID))
                //{
                //Add Users			
                TreeNode treeNodeUserRoot = new TreeNode("Users");
                treeNodeUserRoot.NodeFont = font;
                NodeDetails userNodeRoot = new NodeDetails(NodeType.User, int.MinValue);
                treeNodeUserRoot.Tag = userNodeRoot;
                Users users = UserManager.GetUsers();
                foreach (User user in users)
                {
                    if (gotFirstNode == false)
                    {
                        gotFirstNode = true;
                        //selectedNodeID = 0;
                    }

                    TreeNode treeNodeUser;
                    if (UserManager.IsAdmin(user.UserID))
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
                //}
                //else
                //{
                //    DisableUserTabs();
                //}

                trvUserRights.ExpandAll();
                trvUserRights.SelectedNode = trvUserRights.Nodes[parentNodeID];
                //if(gotFirstNode == true)
                //{
                //    trvUserRights.SelectedNode = trvUserRights.Nodes[parentNodeID].Nodes[selectedNodeID];
                //}
                //else
                //{
                //    //trvUserRights.SelectedNode = trvUserRights.Nodes[parentNodeID];
                //}
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("BindUserTree",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "BindUserTree", null);

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
                if (trvUserRights.SelectedNode.Parent != null)
                {
                    if (trvUserRights.SelectedNode == null)
                    {
                        //						Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                        //						Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Please select User/Vendor to be deleted!");
                    }
                    else
                    {
                        NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;
                        NodeDetails prevNodeDetails = new NodeDetails();
                        if (trvUserRights.SelectedNode.PrevNode != null)
                        {
                            prevNodeDetails = (NodeDetails)trvUserRights.SelectedNode.PrevNode.Tag;
                        }
                        else
                        {
                            prevNodeDetails = (NodeDetails)trvUserRights.SelectedNode.Parent.Tag;
                        }
                        Prana.Admin.BLL.User loggedUser = UserManager.GetUser(Preferences.Instance.UserID);


                        int nodeID = nodeDetails.NodeID;
                        Prana.Admin.BLL.User user = UserManager.GetUser(nodeID);

                        switch (loggedUser.SuperUser)
                        {
                            case SUPER_USER:
                                if (user.SuperUser == SUPER_USER)
                                {
                                }
                                else
                                {
                                    if (MessageBox.Show(this, "Do you want to delete selected User?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        result = UserManager.DeleteUser(nodeID);
                                        BindUserTree();
                                        SelectNode(NodeType.User, prevNodeDetails.NodeID);
                                    }
                                }
                                break;

                            default:
                                if (UserManager.IsAdmin(nodeDetails.NodeID) || user.SuperUser == SUPER_USER || nodeDetails.NodeID == loggedUser.UserID)
                                {
                                }
                                else
                                {
                                    if (MessageBox.Show(this, "Do you want to delete selected User?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        result = UserManager.DeleteUser(nodeID);
                                        BindUserTree();
                                        SelectNode(NodeType.User, prevNodeDetails.NodeID);
                                    }
                                }
                                break;
                        }

                        //if(nodeDetails.Type == NodeType.User)
                        //{
                        //    //If the user is admin user then it cant be deleted.
                        //    //SuperAdminMain superAdminMain = (SuperAdminMain) this.Owner;
                        //    //if(superAdminMain.UserPermissions.UserID == nodeID || UserManager.IsAdmin(nodeID))
                        //    //if(Preferences.Instance.UserID == nodeDetails.NodeID)
                        //    //if(UserManager.IsAdmin(nodeID))
                        //    //if (Preferences.Instance.UserID == nodeDetails.NodeID)
                        //    //if(true)
                        //    if (user.SuperUser != SUPER_USER)
                        //    {
                        //        if (UserManager.IsAdmin(nodeDetails.NodeID) || user.SuperUser == SUPER_USER)
                        //        {
                        //            //								Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                        //            //								Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User Can't be deleted!");
                        //        }
                        //        else
                        //        {
                        //            if (MessageBox.Show(this, "Do you want to delete selected User?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //            {
                        //                result = UserManager.DeleteUser(nodeID);
                        //                //									Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                        //                //									Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User deleted");

                        //                BindUserTree();
                        //                SelectNode(NodeType.User, prevNodeDetails.NodeID);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (user.SuperUser == SUPER_USER)
                        //        {
                        //            //								Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                        //            //								Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User Can't be deleted!");
                        //        }
                        //        else
                        //        {
                        //            if (MessageBox.Show(this, "Do you want to delete selected User?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //            {
                        //                result = UserManager.DeleteUser(nodeID);
                        //                //									Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                        //                //									Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "User deleted");

                        //                BindUserTree();
                        //                SelectNode(NodeType.User, prevNodeDetails.NodeID);
                        //            }
                        //        }
                        //    }
                        //}				

                    }
                }
                else
                {
                    //					Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                    //					Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Can't Delete Root Node");

                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnDelete_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnDelete_Click", null);

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
                if (trvUserRights.SelectedNode == null)
                {
                    //					Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                    //					Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Please select User/Vendor to be shown with the details!");
                }
                else
                {
                    if (nodeDetails.Type == NodeType.User)
                    {   //User is selected.					

                        int userID = nodeDetails.NodeID;
                        Prana.Admin.BLL.User user = UserManager.GetUser(userID);
                        AdminUserDetails(user);

                        Prana.Admin.BLL.User loggedUser = UserManager.GetUser(Preferences.Instance.UserID);

                        tabThirdPartyVendor.Tabs[0].Selected = true;


                        //ultraTabPageControl1.Show();
                        //if((((SuperAdminMain) this.ParentForm).UserPermissions.UserID) == nodeDetails.NodeID)
                        //if((((SuperAdminMain) this.Owner).UserPermissions.UserID) == nodeDetails.NodeID)

                        //if (Preferences.Instance.UserID == nodeDetails.NodeID || UserManager.IsAdmin(nodeDetails.NodeID))
                        //if (UserManager.IsAdmin(nodeDetails.NodeID) || user.SuperUser == SUPER_USER)
                        ////if(true)
                        //{
                        //    DisableUserTabs();
                        //    //MessageBox.Show("This is Admin User");
                        //}
                        //else
                        //{
                        //    EnableUserTabs();  //MessageBox.Show("You have no permission to edit Users.");
                        //}

                        switch (loggedUser.SuperUser)
                        {
                            case SUPER_USER:
                                if (user.SuperUser == SUPER_USER)
                                {
                                    DisableUserTabs();
                                }
                                break;

                            default:
                                if ((UserManager.IsAdmin(user.UserID) || user.SuperUser == SUPER_USER) && loggedUser.UserID != user.UserID)
                                {
                                    DisableUserTabs();
                                }
                                else
                                {
                                    EnableUserTabs();
                                }

                                if (!(UserManager.IsAdmin(loggedUser.UserID)))
                                {
                                    if (loggedUser.UserID == user.UserID)
                                    {
                                        EnableUserTabs();
                                    }
                                    else
                                    {
                                        DisableUserTabs();
                                    }
                                }

                                break;
                        }
                    }

                    if (chkAddSLSU == false && nodeDetails.NodeID == int.MinValue)
                    {
                        DisableUserTabs();
                    }

                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("trvUserRights_AfterSelect",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "trvUserRights_AfterSelect", null);

                #endregion
            }
        }

        /// <summary>
        /// Showing the user details and the associated permissions with it.
        /// </summary>
        /// <param name="user"></param>
        private void AdminUserDetails(Prana.Admin.BLL.User user)
        {
            //User userdetail; 

            if (user != null)
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
                    BindEmptyStates();
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
                txtCity.Text = user.City;

                //Fill permission form.
                Permissions permissions = PermissionManager.GetPermissions(user.UserID);
                StringBuilder permissionIDString = new StringBuilder();
                permissionIDString.Append(",");
                foreach (Permission permission in permissions)
                {
                    permissionIDString.Append(permission.PermissionID);
                    permissionIDString.Append(",");
                }

                //chkPermissionAUEC.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionAUEC.Tag.ToString().Trim() + ",") >= 0?true:false);

                //chkPermissionCompany.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionCompany.Tag.ToString().Trim() + ",") >= 0?true:false);

                //chkPermissionCounterParties.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionCounterParties.Tag.ToString().Trim() + ",") >= 0?true:false);

                //chkPermissionMaintainCompanis.Checked = (permissionIDString.ToString().IndexOf("," + chkPermissionMaintainCompanis.Tag.ToString().Trim() + ",") >= 0?true:false);


                StringBuilder permissionIDStringTemp = new StringBuilder();
                permissionIDStringTemp.Append(",");
                CheckBox chkCtr1 = new CheckBox();
                foreach (Control ctr in pnlUserPerrmissions.Controls)
                {
                    Type type = ctr.GetType();
                    switch (type.ToString())
                    {
                        case "System.Windows.Forms.CheckBox":
                            chkCtr1 = (CheckBox)ctr;
                            chkCtr1.Checked = (permissionIDStringTemp.ToString().IndexOf("," + chkCtr1.Tag.ToString().Trim() + ",") >= 0 ? true : false);
                            break;
                    }
                }

                LoadUserPermissions();

                CheckBox chkCtr = new CheckBox();
                foreach (Control ctr in pnlUserPerrmissions.Controls)
                {
                    Type type = ctr.GetType();
                    switch (type.ToString())
                    {
                        case "System.Windows.Forms.CheckBox":
                            chkCtr = (CheckBox)ctr;
                            chkCtr.Checked = (permissionIDString.ToString().IndexOf("," + chkCtr.Tag.ToString().Trim() + ",") >= 0 ? true : false);
                            break;
                    }
                }
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
                if (trvUserRights.SelectedNode == null)
                {
                    //					Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                    //					Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Please select User/Vendor to be added!");
                }
                else
                {
                    NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;
                    selectedNodeType = nodeDetails.Type;
                    if (selectedNodeType == NodeType.User)
                    {
                        //						Prana.Admin.Utility.Common.ResetStatusPanel(stbSLAU);
                        //						Prana.Admin.Utility.Common.SetStatusPanel(stbSLAU, "Enter User Details.");
                        RefreshUserForm();

                        tabThirdPartyVendor.Tabs[0].Selected = true;
                        ultraTabPageControl1.Show();
                    }

                    //Set Focus to parent node.
                    if (nodeDetails.NodeID != int.MinValue)
                    {
                        trvUserRights.SelectedNode = trvUserRights.SelectedNode.Parent;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnAdd_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnAdd_Click", null);

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
                get { return _type; }
                set { _type = value; }
            }
            public int NodeID
            {
                get { return _nodeID; }
                set { _nodeID = value; }
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
                if (trvUserRights.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;

                    if ((nodeDetails.Type != NodeType.User) && (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_USER]))
                    {
                        trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_USER];
                    }
                    if ((nodeDetails.Type != NodeType.User) && (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_PERMISSION]))
                    {
                        trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_USER];
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("tabThirdPartyVendor_SelectedTabChanged",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "tabThirdPartyVendor_SelectedTabChanged", null);

                #endregion
            }
        }

        private void tabThirdPartyVendor_TabIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                NodeDetails nodeDetails = (NodeDetails)trvUserRights.SelectedNode.Tag;

                if ((nodeDetails.Type != NodeType.User) && (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_USER]))
                {
                    trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_USER];
                }
                if ((nodeDetails.Type != NodeType.Vendor) && (tabThirdPartyVendor.SelectedTab == tabThirdPartyVendor.Tabs[C_TAB_VENDOR]))
                {
                    trvUserRights.SelectedNode = trvUserRights.Nodes[C_TREE_VENDOR];
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("tabThirdPartyVendor_TabIndexChanged",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "tabThirdPartyVendor_TabIndexChanged", null);

                #endregion
            }
        }

        private void cmbCountry_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCountry.Value != null)
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
                        cmbState.DataSource = null;
                        cmbState.DataSource = states;
                        cmbState.Text = C_COMBO_SELECT;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
                        {
                            if (!column.Key.Equals("StateName"))
                            {
                                column.Hidden = true;
                            }
                        }
                        cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    }
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

        #region Focus Colors
        private void cmbCountry_GotFocus(object sender, System.EventArgs e)
        {
            cmbCountry.Appearance.BackColor = Color.LemonChiffon;
        }

        private void cmbCountry_LostFocus(object sender, System.EventArgs e)
        {
            cmbCountry.Appearance.BackColor = Color.White;
        }

        private void cmbState_GotFocus(object sender, System.EventArgs e)
        {
            cmbState.Appearance.BackColor = Color.LemonChiffon;
        }

        private void cmbState_LostFocus(object sender, System.EventArgs e)
        {
            cmbState.Appearance.BackColor = Color.White;
        }


        #endregion

        private void chkSLSU1_CheckStateChanged(object sender, System.EventArgs e)
        {

            //cmbCountry_ValueChanged
            //((TextBox)sender).BackColor = Color.LemonChiffon;
            //MessageBox.Show(sender.ToString());
            int chkCount = 0;
            int ptY = ((System.Windows.Forms.Control)(sender)).Location.Y;

            bool chkEdit = false;
            bool chkAdd = false;
            bool chkDelete = false;
            string currentCheckBoxName = ((System.Windows.Forms.Control)(sender)).Name.ToString();
            bool isChecked = ((CheckBox)(System.Windows.Forms.Control)(sender)).Checked;
            //bool exitLoop = false;
            if (currentCheckBoxName != "View")
            {
                CheckBox chkCtr = new CheckBox();
                foreach (Control ctr in pnlUserPerrmissions.Controls)
                {
                    //MessageBox.Show(ctr.Name);
                    Type type = ctr.GetType();
                    switch (type.ToString())
                    {
                        case "System.Windows.Forms.CheckBox":

                            if (chkCount < 4)
                            {
                                if (int.Parse(ctr.Location.Y.ToString()) == ptY)
                                {
                                    //if ((ctr.Text.ToString() == "View") && (isChecked == false))
                                    //{
                                    //    chkEdit = false;
                                    //    break;
                                    //}
                                    if ((ctr.Text.ToString() == "Edit") && ((CheckBox)ctr).Checked == true)
                                    {
                                        chkEdit = true;
                                        break;
                                    }
                                    if ((ctr.Text.ToString() == "Add") && ((CheckBox)ctr).Checked == true)
                                    {
                                        chkAdd = true;
                                        break;
                                    }
                                    if ((ctr.Text.ToString() == "Delete") && ((CheckBox)ctr).Checked == true)
                                    {
                                        chkDelete = true;
                                        break;
                                    }
                                    //else
                                    //{
                                    //    chkEditAddDelete = false;
                                    //}
                                    chkCount++;
                                }
                            }
                            //MessageBox.Show(ty.ToString());
                            //chkCtr = (CheckBox)ctr;
                            //permissions.Add(new Permission(int.Parse(ctr.Tag.ToString()), chkCtr.Checked));
                            break;
                    }
                }
                foreach (Control ctr in pnlUserPerrmissions.Controls)
                {
                    //MessageBox.Show(ctr.Name);
                    Type type = ctr.GetType();
                    switch (type.ToString())
                    {
                        case "System.Windows.Forms.CheckBox":
                            if ((chkEdit == false && chkAdd == false && chkDelete == false && isChecked == false) && (ctr.Text.ToString() == "View") && (int.Parse(ctr.Location.Y.ToString()) == ptY))
                            {
                                ((CheckBox)ctr).Checked = false;
                                chkEdit = false;
                                chkAdd = false;
                                chkDelete = false;
                                break;
                            }
                            if ((chkEdit == true || chkAdd == true || chkDelete == true || isChecked == true) && (ctr.Text.ToString() == "View") && (int.Parse(ctr.Location.Y.ToString()) == ptY))
                            {
                                ((CheckBox)ctr).Checked = true;
                                //chkEdit = true;
                                //chkAdd = true;
                                //chkDelete = true;
                                break;
                            }

                            if (chkAdd == true && (ctr.Text.ToString() == "Edit") && (int.Parse(ctr.Location.Y.ToString()) == ptY))
                            {
                                ((CheckBox)ctr).Checked = true;
                                chkAdd = true;
                                break;
                            }

                            if (chkDelete == true && (ctr.Text.ToString() == "Edit") && (int.Parse(ctr.Location.Y.ToString()) == ptY))
                            {
                                ((CheckBox)ctr).Checked = true;
                                chkDelete = true;
                                break;
                            }

                            break;
                    }
                }

            }
        }
    }
}
