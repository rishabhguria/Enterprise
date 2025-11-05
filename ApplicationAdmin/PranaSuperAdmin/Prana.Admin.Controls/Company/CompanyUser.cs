#region Using

using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Interface;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;


#endregion

namespace Prana.Admin.Controls
{

    /// <summary>
    /// Summary description for CompanyUser.
    /// </summary>
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.UserCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.UserUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.UserApproved, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.UserDeleted, ShowAuditUI = true)]
    public class CompanyUser : System.Windows.Forms.UserControl, IAuditSource
    {
        const string C_COMBO_SELECT = "- Select -";
        #region private and protected members

        private int _companyID = int.MinValue;

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFirstName;
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
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtLoginName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.TextBox txtEMail;
        private System.Windows.Forms.TextBox txtTelephoneWork;
        private System.Windows.Forms.TextBox txtTelephoneCell;
        private System.Windows.Forms.TextBox txtPager;
        private System.Windows.Forms.TextBox txtTelephoneHome;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label lblStateTerritory;
        private System.Windows.Forms.Label lblZip;
        private System.Windows.Forms.TextBox txtZip;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbState;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCountry;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTradingPermission;
        private Label label26;
        private Label label29;
        private Label lblUserType;
        private Label lblUserID;
        private TextBox txtUserType;
        private TextBox txtUserID;
        private Label label30;
        private Label label43;
        private TextBox txtCity;
        private Label lblCity;
        private Label lblFactSetUsernameAndSerialNumber;
        private TextBox txtFactSetUsernameAndSerialNumber;
        private Label lblFactSetSupportUser;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraComboFactSetConnection;
        private Infragistics.Win.Misc.UltraPanel ultraPanelActiv;
        private Label labelActivPassword;
        private TextBox txtActivPassword;
        private Label labelActivUsername;
        private TextBox txtActivUsername;
        private Infragistics.Win.Misc.UltraPanel ultraPanelFactSet;
        private Infragistics.Win.Misc.UltraLabel lblWebAzureId;
        private TextBox txtWebAzureId;
        private Infragistics.Win.Misc.UltraPanel ultraPanelSapi;
        private Label labelSapiUsername;
        private TextBox txtSapiUsername;
        private IContainer components;

        //		public int CompanyID
        //		{
        //			//get{}
        //			//set{_companyID = value;}
        //		}

        [AuditManager.Attributes.AuditSourceConstAttri]
        public CompanyUser()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            //			BindTradingPermission();
            //			BindCountries();
            //			BindStates();

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
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (txtFirstName != null)
                {
                    txtFirstName.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
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
                if (txtLastName != null)
                {
                    txtLastName.Dispose();
                }
                if (txtLoginName != null)
                {
                    txtLoginName.Dispose();
                }
                if (txtPassword != null)
                {
                    txtPassword.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (txtTitle != null)
                {
                    txtTitle.Dispose();
                }
                if (txtAddress1 != null)
                {
                    txtAddress1.Dispose();
                }
                if (txtAddress2 != null)
                {
                    txtAddress2.Dispose();
                }
                if (txtEMail != null)
                {
                    txtEMail.Dispose();
                }
                if (txtTelephoneWork != null)
                {
                    txtTelephoneWork.Dispose();
                }
                if (txtTelephoneCell != null)
                {
                    txtTelephoneCell.Dispose();
                }
                if (txtPager != null)
                {
                    txtPager.Dispose();
                }
                if (txtTelephoneHome != null)
                {
                    txtTelephoneHome.Dispose();
                }
                if (txtFax != null)
                {
                    txtFax.Dispose();
                }
                if (label16 != null)
                {
                    label16.Dispose();
                }
                if (label18 != null)
                {
                    label18.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label20 != null)
                {
                    label20.Dispose();
                }
                if (label23 != null)
                {
                    label23.Dispose();
                }
                if (label24 != null)
                {
                    label24.Dispose();
                }
                if (label25 != null)
                {
                    label25.Dispose();
                }
                if (label27 != null)
                {
                    label27.Dispose();
                }
                if (label28 != null)
                {
                    label28.Dispose();
                }
                if (lblCountry != null)
                {
                    lblCountry.Dispose();
                }
                if (lblStateTerritory != null)
                {
                    lblStateTerritory.Dispose();
                }
                if (lblZip != null)
                {
                    lblZip.Dispose();
                }
                if (txtZip != null)
                {
                    txtZip.Dispose();
                }
                if (label21 != null)
                {
                    label21.Dispose();
                }
                if (label22 != null)
                {
                    label22.Dispose();
                }
                if (cmbState != null)
                {
                    cmbState.Dispose();
                }
                if (cmbCountry != null)
                {
                    cmbCountry.Dispose();
                }
                if (cmbTradingPermission != null)
                {
                    cmbTradingPermission.Dispose();
                }
                if (label26 != null)
                {
                    label26.Dispose();
                }
                if (label29 != null)
                {
                    label29.Dispose();
                }
                if (lblUserType != null)
                {
                    lblUserType.Dispose();
                }
                if (lblUserID != null)
                {
                    lblUserID.Dispose();
                }
                if (label30 != null)
                {
                    label30.Dispose();
                }
                if (label43 != null)
                {
                    label43.Dispose();
                }
                if (txtCity != null)
                {
                    txtCity.Dispose();
                }
                if (lblCity != null)
                {
                    lblCity.Dispose();
                }
                if (lblFactSetUsernameAndSerialNumber != null)
                {
                    lblFactSetUsernameAndSerialNumber.Dispose();
                }
                if (txtFactSetUsernameAndSerialNumber != null)
                {
                    txtFactSetUsernameAndSerialNumber.Dispose();
                }
                if (lblFactSetSupportUser != null)
                {
                    lblFactSetSupportUser.Dispose();
                }
                if (ultraComboFactSetConnection != null)
                {
                    ultraComboFactSetConnection.Dispose();
                }
                if (ultraPanelActiv != null)
                {
                    ultraPanelActiv.Dispose();
                }
                if (labelActivPassword != null)
                {
                    labelActivPassword.Dispose();
                }
                if (txtActivPassword != null)
                {
                    txtActivPassword.Dispose();
                }
                if (labelActivPassword != null)
                {
                    labelActivPassword.Dispose();
                }
                if (txtActivUsername != null)
                {
                    txtActivUsername.Dispose();
                }
                if (ultraPanelFactSet != null)
                {
                    ultraPanelFactSet.Dispose();
                }
                if (txtUserType != null)
                {
                    txtUserType.Dispose();
                }
                if (txtUserID != null)
                {
                    txtUserID.Dispose();
                }
                if (labelActivUsername != null)
                {
                    labelActivUsername.Dispose();
                }
                if (txtWebAzureId != null)
                {
                    txtWebAzureId.Dispose();
                }
                if (lblWebAzureId != null)
                {
                    lblWebAzureId.Dispose();
                }
                if (ultraPanelSapi != null)
                {
                    ultraPanelSapi.Dispose();
                }
                if (labelSapiUsername != null)
                {
                    labelSapiUsername.Dispose();
                }
                if (txtSapiUsername != null)
                {
                    txtSapiUsername.Dispose();
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
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 2);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblFactSetSupportUser = new System.Windows.Forms.Label();
            this.txtWebAzureId = new System.Windows.Forms.TextBox();
            this.lblWebAzureId = new Infragistics.Win.Misc.UltraLabel();
            this.label27 = new System.Windows.Forms.Label();
            this.ultraComboFactSetConnection = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label43 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.lblUserType = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtUserType = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.cmbTradingPermission = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbState = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label22 = new System.Windows.Forms.Label();
            this.lblZip = new System.Windows.Forms.Label();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.lblStateTerritory = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtLoginName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.txtEMail = new System.Windows.Forms.TextBox();
            this.txtTelephoneWork = new System.Windows.Forms.TextBox();
            this.txtTelephoneCell = new System.Windows.Forms.TextBox();
            this.txtPager = new System.Windows.Forms.TextBox();
            this.txtTelephoneHome = new System.Windows.Forms.TextBox();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.lblFactSetUsernameAndSerialNumber = new System.Windows.Forms.Label();
            this.txtFactSetUsernameAndSerialNumber = new System.Windows.Forms.TextBox();
            this.ultraPanelFactSet = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanelActiv = new Infragistics.Win.Misc.UltraPanel();
            this.labelActivUsername = new System.Windows.Forms.Label();
            this.txtActivUsername = new System.Windows.Forms.TextBox();
            this.labelActivPassword = new System.Windows.Forms.Label();
            this.txtActivPassword = new System.Windows.Forms.TextBox();
            this.ultraPanelSapi = new Infragistics.Win.Misc.UltraPanel();
            this.labelSapiUsername = new System.Windows.Forms.Label();
            this.txtSapiUsername = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboFactSetConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingPermission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
            this.ultraPanelFactSet.ClientArea.SuspendLayout();
            this.ultraPanelFactSet.SuspendLayout();
            this.ultraPanelActiv.ClientArea.SuspendLayout();
            this.ultraPanelActiv.SuspendLayout();
            this.ultraPanelSapi.ClientArea.SuspendLayout();
            this.ultraPanelSapi.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtWebAzureId);
            this.groupBox1.Controls.Add(this.lblWebAzureId);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.txtCity);
            this.groupBox1.Controls.Add(this.lblCity);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.lblUserType);
            this.groupBox1.Controls.Add(this.lblUserID);
            this.groupBox1.Controls.Add(this.txtUserType);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.cmbTradingPermission);
            this.groupBox1.Controls.Add(this.cmbState);
            this.groupBox1.Controls.Add(this.cmbCountry);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.lblZip);
            this.groupBox1.Controls.Add(this.txtZip);
            this.groupBox1.Controls.Add(this.lblStateTerritory);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.lblCountry);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.txtLoginName);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Controls.Add(this.txtTitle);
            this.groupBox1.Controls.Add(this.txtAddress1);
            this.groupBox1.Controls.Add(this.txtAddress2);
            this.groupBox1.Controls.Add(this.txtEMail);
            this.groupBox1.Controls.Add(this.txtTelephoneWork);
            this.groupBox1.Controls.Add(this.txtTelephoneCell);
            this.groupBox1.Controls.Add(this.txtPager);
            this.groupBox1.Controls.Add(this.txtTelephoneHome);
            this.groupBox1.Controls.Add(this.txtFax);
            this.groupBox1.Controls.Add(this.ultraPanelActiv);
            this.groupBox1.Controls.Add(this.ultraPanelFactSet);
            this.groupBox1.Controls.Add(this.ultraPanelSapi);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(0, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(629, 383);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Detail";
            // 
            // lblFactSetSupportUser
            // 
            this.lblFactSetSupportUser.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFactSetSupportUser.Location = new System.Drawing.Point(3, 31);
            this.lblFactSetSupportUser.Name = "lblFactSetSupportUser";
            this.lblFactSetSupportUser.Size = new System.Drawing.Size(107, 16);
            this.lblFactSetSupportUser.TabIndex = 181;
            this.lblFactSetSupportUser.Text = "FactSet Connection";
            // 
            // txtWebAzureId
            // 
            this.txtWebAzureId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWebAzureId.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtWebAzureId.Location = new System.Drawing.Point(462, 263);
            this.txtWebAzureId.Name = "txtWebAzureId";
            this.txtWebAzureId.Size = new System.Drawing.Size(150, 21);
            this.txtWebAzureId.TabIndex = 187;
            this.txtWebAzureId.GotFocus += new System.EventHandler(this.txtWebAzureId_GotFocus);
            this.txtWebAzureId.LostFocus += new System.EventHandler(this.txtWebAzureId_LostFocus);
            // 
            // lblWebAzureId
            // 
            this.lblWebAzureId.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblWebAzureId.Location = new System.Drawing.Point(318, 267);
            this.lblWebAzureId.Name = "lblWebAzureId";
            this.lblWebAzureId.Size = new System.Drawing.Size(142, 16);
            this.lblWebAzureId.TabIndex = 186;
            this.lblWebAzureId.Text = "Nirvāna ONE AzureID";
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label27.ForeColor = System.Drawing.Color.Red;
            this.label27.Location = new System.Drawing.Point(6, 245);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(98, 16);
            this.label27.TabIndex = 178;
            this.label27.Text = "* Required Field";
            // 
            // ultraComboFactSetConnection
            // 
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraComboFactSetConnection.DisplayLayout.Appearance = appearance37;
            this.ultraComboFactSetConnection.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridColumn10.Header.VisiblePosition = 0;
            ultraGridColumn11.Header.VisiblePosition = 1;
            ultraGridColumn11.Hidden = true;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn10,
            ultraGridColumn11});
            this.ultraComboFactSetConnection.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.ultraComboFactSetConnection.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraComboFactSetConnection.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraComboFactSetConnection.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraComboFactSetConnection.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.ultraComboFactSetConnection.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraComboFactSetConnection.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.ultraComboFactSetConnection.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraComboFactSetConnection.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraComboFactSetConnection.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraComboFactSetConnection.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.ultraComboFactSetConnection.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraComboFactSetConnection.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.ultraComboFactSetConnection.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraComboFactSetConnection.DisplayLayout.Override.CellAppearance = appearance44;
            this.ultraComboFactSetConnection.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraComboFactSetConnection.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraComboFactSetConnection.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlignAsString = "Left";
            this.ultraComboFactSetConnection.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.ultraComboFactSetConnection.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraComboFactSetConnection.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.ultraComboFactSetConnection.DisplayLayout.Override.RowAppearance = appearance47;
            this.ultraComboFactSetConnection.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraComboFactSetConnection.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.ultraComboFactSetConnection.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraComboFactSetConnection.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraComboFactSetConnection.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraComboFactSetConnection.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.ultraComboFactSetConnection.DropDownWidth = 0;
            this.ultraComboFactSetConnection.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraComboFactSetConnection.Location = new System.Drawing.Point(120, 26);
            this.ultraComboFactSetConnection.Name = "ultraComboFactSetConnection";
            this.ultraComboFactSetConnection.Size = new System.Drawing.Size(203, 21);
            this.ultraComboFactSetConnection.TabIndex = 183;
            this.ultraComboFactSetConnection.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // label43
            // 
            this.label43.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(345, 65);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(8, 8);
            this.label43.TabIndex = 173;
            this.label43.Text = "*";
            // 
            // txtCity
            // 
            this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCity.Location = new System.Drawing.Point(462, 64);
            this.txtCity.MaxLength = 50;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(150, 21);
            this.txtCity.TabIndex = 12;
            // 
            // lblCity
            // 
            this.lblCity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCity.Location = new System.Drawing.Point(318, 64);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(32, 16);
            this.lblCity.TabIndex = 172;
            this.lblCity.Text = "City";
            // 
            // label30
            // 
            this.label30.ForeColor = System.Drawing.Color.Red;
            this.label30.Location = new System.Drawing.Point(354, 154);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(12, 8);
            this.label30.TabIndex = 79;
            this.label30.Text = "*";
            // 
            // label26
            // 
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(49, 133);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(12, 8);
            this.label26.TabIndex = 78;
            this.label26.Text = "*";
            // 
            // label29
            // 
            this.label29.ForeColor = System.Drawing.Color.Red;
            this.label29.Location = new System.Drawing.Point(61, 110);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(12, 8);
            this.label29.TabIndex = 77;
            this.label29.Text = "*";
            // 
            // lblUserType
            // 
            this.lblUserType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblUserType.Location = new System.Drawing.Point(6, 107);
            this.lblUserType.Name = "lblUserType";
            this.lblUserType.Size = new System.Drawing.Size(60, 16);
            this.lblUserType.TabIndex = 75;
            this.lblUserType.Text = "User Type";
            this.lblUserType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUserID
            // 
            this.lblUserID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblUserID.Location = new System.Drawing.Point(6, 130);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(54, 16);
            this.lblUserID.TabIndex = 76;
            this.lblUserID.Text = "User ID";
            this.lblUserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUserType
            // 
            this.txtUserType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtUserType.Location = new System.Drawing.Point(123, 108);
            this.txtUserType.MaxLength = 50;
            this.txtUserType.Name = "txtUserType";
            this.txtUserType.ReadOnly = true;
            this.txtUserType.Size = new System.Drawing.Size(150, 21);
            this.txtUserType.TabIndex = 4;
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtUserID.Location = new System.Drawing.Point(123, 130);
            this.txtUserID.MaxLength = 50;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.PasswordChar = '*';
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(150, 21);
            this.txtUserID.TabIndex = 5;
            // 
            // cmbTradingPermission
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTradingPermission.DisplayLayout.Appearance = appearance1;
            this.cmbTradingPermission.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn8.Header.VisiblePosition = 0;
            ultraGridColumn9.Header.VisiblePosition = 1;
            ultraGridColumn9.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn8,
            ultraGridColumn9});
            this.cmbTradingPermission.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbTradingPermission.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTradingPermission.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTradingPermission.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTradingPermission.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbTradingPermission.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTradingPermission.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbTradingPermission.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTradingPermission.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTradingPermission.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTradingPermission.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbTradingPermission.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTradingPermission.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTradingPermission.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTradingPermission.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbTradingPermission.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTradingPermission.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTradingPermission.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbTradingPermission.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbTradingPermission.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTradingPermission.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbTradingPermission.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbTradingPermission.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTradingPermission.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbTradingPermission.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTradingPermission.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTradingPermission.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTradingPermission.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTradingPermission.DropDownWidth = 0;
            this.cmbTradingPermission.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbTradingPermission.Location = new System.Drawing.Point(462, 240);
            this.cmbTradingPermission.Name = "cmbTradingPermission";
            this.cmbTradingPermission.Size = new System.Drawing.Size(150, 21);
            this.cmbTradingPermission.TabIndex = 20;
            this.cmbTradingPermission.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTradingPermission.GotFocus += new System.EventHandler(this.cmbTradingPermission_GotFocus);
            this.cmbTradingPermission.LostFocus += new System.EventHandler(this.cmbTradingPermission_LostFocus);
            // 
            // cmbState
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbState.DisplayLayout.Appearance = appearance13;
            this.cmbState.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn5.Header.VisiblePosition = 2;
            ultraGridColumn5.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.cmbState.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbState.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbState.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbState.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbState.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbState.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbState.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbState.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbState.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbState.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbState.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbState.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbState.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbState.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbState.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbState.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbState.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbState.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbState.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbState.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbState.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbState.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbState.DropDownWidth = 0;
            this.cmbState.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbState.Location = new System.Drawing.Point(462, 42);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(150, 21);
            this.cmbState.TabIndex = 11;
            this.cmbState.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbState.GotFocus += new System.EventHandler(this.cmbState_GotFocus);
            this.cmbState.LostFocus += new System.EventHandler(this.cmbState_LostFocus);
            // 
            // cmbCountry
            // 
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCountry.DisplayLayout.Appearance = appearance25;
            this.cmbCountry.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn6.Header.VisiblePosition = 0;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn6,
            ultraGridColumn7});
            this.cmbCountry.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbCountry.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCountry.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCountry.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbCountry.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCountry.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbCountry.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCountry.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCountry.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCountry.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCountry.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbCountry.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCountry.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.cmbCountry.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbCountry.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCountry.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbCountry.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbCountry.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCountry.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbCountry.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCountry.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCountry.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCountry.DropDownWidth = 0;
            this.cmbCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCountry.Location = new System.Drawing.Point(462, 20);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(150, 21);
            this.cmbCountry.TabIndex = 10;
            this.cmbCountry.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
            this.cmbCountry.GotFocus += new System.EventHandler(this.cmbCountry_GotFocus);
            this.cmbCountry.LostFocus += new System.EventHandler(this.cmbCountry_LostFocus);
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label22.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(396, 42);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(12, 8);
            this.label22.TabIndex = 72;
            this.label22.Text = "*";
            // 
            // lblZip
            // 
            this.lblZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblZip.Location = new System.Drawing.Point(318, 86);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(32, 16);
            this.lblZip.TabIndex = 67;
            this.lblZip.Text = "Zip";
            this.lblZip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtZip
            // 
            this.txtZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtZip.Location = new System.Drawing.Point(462, 86);
            this.txtZip.MaxLength = 50;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(150, 21);
            this.txtZip.TabIndex = 13;
            this.txtZip.GotFocus += new System.EventHandler(this.txtZip_GotFocus);
            this.txtZip.LostFocus += new System.EventHandler(this.txtZip_LostFocus);
            // 
            // lblStateTerritory
            // 
            this.lblStateTerritory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStateTerritory.Location = new System.Drawing.Point(318, 41);
            this.lblStateTerritory.Name = "lblStateTerritory";
            this.lblStateTerritory.Size = new System.Drawing.Size(80, 16);
            this.lblStateTerritory.TabIndex = 64;
            this.lblStateTerritory.Text = "State/Territory";
            this.lblStateTerritory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label21.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(361, 23);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(12, 12);
            this.label21.TabIndex = 63;
            this.label21.Text = "*";
            // 
            // lblCountry
            // 
            this.lblCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCountry.Location = new System.Drawing.Point(318, 19);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(48, 16);
            this.lblCountry.TabIndex = 61;
            this.lblCountry.Text = "Country";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.ForeColor = System.Drawing.Color.Red;
            this.label28.Location = new System.Drawing.Point(416, 243);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(10, 8);
            this.label28.TabIndex = 60;
            this.label28.Text = "*";
            // 
            // label25
            // 
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(437, 131);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(12, 8);
            this.label25.TabIndex = 58;
            this.label25.Text = "*";
            // 
            // label24
            // 
            this.label24.ForeColor = System.Drawing.Color.Red;
            this.label24.Location = new System.Drawing.Point(355, 109);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(12, 8);
            this.label24.TabIndex = 57;
            this.label24.Text = "*";
            // 
            // label23
            // 
            this.label23.ForeColor = System.Drawing.Color.Red;
            this.label23.Location = new System.Drawing.Point(56, 200);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(12, 8);
            this.label23.TabIndex = 56;
            this.label23.Text = "*";
            // 
            // label20
            // 
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(68, 67);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(12, 8);
            this.label20.TabIndex = 53;
            this.label20.Text = "*";
            // 
            // label19
            // 
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(57, 176);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(12, 8);
            this.label19.TabIndex = 52;
            this.label19.Text = "*";
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(39, 154);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(12, 8);
            this.label18.TabIndex = 51;
            this.label18.Text = "*";
            // 
            // label16
            // 
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(63, 24);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(12, 8);
            this.label16.TabIndex = 49;
            this.label16.Text = "*";
            // 
            // txtFirstName
            // 
            this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFirstName.Location = new System.Drawing.Point(123, 20);
            this.txtFirstName.MaxLength = 50;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(150, 21);
            this.txtFirstName.TabIndex = 0;
            this.txtFirstName.GotFocus += new System.EventHandler(this.txtFirstName_GotFocus);
            this.txtFirstName.LostFocus += new System.EventHandler(this.txtFirstName_LostFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "First Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 27;
            this.label2.Text = "Last Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(6, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 29;
            this.label3.Text = "Login";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(6, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 16);
            this.label4.TabIndex = 31;
            this.label4.Text = "Password";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(6, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 30;
            this.label5.Text = "Short Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(6, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 16);
            this.label6.TabIndex = 26;
            this.label6.Text = "Title";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(6, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "Address1";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(6, 218);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "Address2";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(318, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 16);
            this.label9.TabIndex = 17;
            this.label9.Text = "E-Mail";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.Location = new System.Drawing.Point(318, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(128, 16);
            this.label10.TabIndex = 18;
            this.label10.Text = "Work # (1-111-111111)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(318, 151);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 16);
            this.label11.TabIndex = 21;
            this.label11.Text = "Cell #";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(318, 174);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 16);
            this.label12.TabIndex = 24;
            this.label12.Text = "Pager #";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(318, 196);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 16);
            this.label13.TabIndex = 25;
            this.label13.Text = "Home #";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label14.Location = new System.Drawing.Point(318, 218);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 16);
            this.label14.TabIndex = 22;
            this.label14.Text = "Fax #";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label15.Location = new System.Drawing.Point(318, 240);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(103, 16);
            this.label15.TabIndex = 23;
            this.label15.Text = "Trading Permission";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLastName
            // 
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLastName.Location = new System.Drawing.Point(123, 42);
            this.txtLastName.MaxLength = 50;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(150, 21);
            this.txtLastName.TabIndex = 1;
            this.txtLastName.GotFocus += new System.EventHandler(this.txtLastName_GotFocus);
            this.txtLastName.LostFocus += new System.EventHandler(this.txtLastName_LostFocus);
            // 
            // txtLoginName
            // 
            this.txtLoginName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLoginName.Location = new System.Drawing.Point(123, 152);
            this.txtLoginName.MaxLength = 50;
            this.txtLoginName.Name = "txtLoginName";
            this.txtLoginName.Size = new System.Drawing.Size(150, 21);
            this.txtLoginName.TabIndex = 6;
            this.txtLoginName.GotFocus += new System.EventHandler(this.txtLoginName_GotFocus);
            this.txtLoginName.LostFocus += new System.EventHandler(this.txtLoginName_LostFocus);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPassword.Location = new System.Drawing.Point(123, 174);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(150, 21);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.GotFocus += new System.EventHandler(this.txtPassword_GotFocus);
            this.txtPassword.LostFocus += new System.EventHandler(this.txtPassword_LostFocus);
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(123, 64);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(150, 21);
            this.txtShortName.TabIndex = 2;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // txtTitle
            // 
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTitle.Location = new System.Drawing.Point(123, 86);
            this.txtTitle.MaxLength = 50;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(150, 21);
            this.txtTitle.TabIndex = 3;
            this.txtTitle.GotFocus += new System.EventHandler(this.txtTitle_GotFocus);
            this.txtTitle.LostFocus += new System.EventHandler(this.txtTitle_LostFocus);
            // 
            // txtAddress1
            // 
            this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress1.Location = new System.Drawing.Point(123, 196);
            this.txtAddress1.MaxLength = 50;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(150, 21);
            this.txtAddress1.TabIndex = 8;
            this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
            this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
            // 
            // txtAddress2
            // 
            this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress2.Location = new System.Drawing.Point(123, 218);
            this.txtAddress2.MaxLength = 50;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(150, 21);
            this.txtAddress2.TabIndex = 9;
            this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
            this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
            // 
            // txtEMail
            // 
            this.txtEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEMail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEMail.Location = new System.Drawing.Point(462, 108);
            this.txtEMail.MaxLength = 50;
            this.txtEMail.Name = "txtEMail";
            this.txtEMail.Size = new System.Drawing.Size(150, 21);
            this.txtEMail.TabIndex = 14;
            this.txtEMail.GotFocus += new System.EventHandler(this.txtEMail_GotFocus);
            this.txtEMail.LostFocus += new System.EventHandler(this.txtEMail_LostFocus);
            // 
            // txtTelephoneWork
            // 
            this.txtTelephoneWork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephoneWork.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTelephoneWork.Location = new System.Drawing.Point(462, 130);
            this.txtTelephoneWork.MaxLength = 50;
            this.txtTelephoneWork.Name = "txtTelephoneWork";
            this.txtTelephoneWork.Size = new System.Drawing.Size(150, 21);
            this.txtTelephoneWork.TabIndex = 15;
            this.txtTelephoneWork.GotFocus += new System.EventHandler(this.txtTelephoneWork_GotFocus);
            this.txtTelephoneWork.LostFocus += new System.EventHandler(this.txtTelephoneWork_LostFocus);
            // 
            // txtTelephoneCell
            // 
            this.txtTelephoneCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephoneCell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTelephoneCell.Location = new System.Drawing.Point(462, 152);
            this.txtTelephoneCell.MaxLength = 50;
            this.txtTelephoneCell.Name = "txtTelephoneCell";
            this.txtTelephoneCell.Size = new System.Drawing.Size(150, 21);
            this.txtTelephoneCell.TabIndex = 16;
            this.txtTelephoneCell.GotFocus += new System.EventHandler(this.txtTelephoneCell_GotFocus);
            this.txtTelephoneCell.LostFocus += new System.EventHandler(this.txtTelephoneCell_LostFocus);
            // 
            // txtPager
            // 
            this.txtPager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPager.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPager.Location = new System.Drawing.Point(462, 174);
            this.txtPager.MaxLength = 50;
            this.txtPager.Name = "txtPager";
            this.txtPager.Size = new System.Drawing.Size(150, 21);
            this.txtPager.TabIndex = 17;
            this.txtPager.GotFocus += new System.EventHandler(this.txtPager_GotFocus);
            this.txtPager.LostFocus += new System.EventHandler(this.txtPager_LostFocus);
            // 
            // txtTelephoneHome
            // 
            this.txtTelephoneHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephoneHome.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTelephoneHome.Location = new System.Drawing.Point(462, 196);
            this.txtTelephoneHome.MaxLength = 50;
            this.txtTelephoneHome.Name = "txtTelephoneHome";
            this.txtTelephoneHome.Size = new System.Drawing.Size(150, 21);
            this.txtTelephoneHome.TabIndex = 18;
            this.txtTelephoneHome.GotFocus += new System.EventHandler(this.txtTelephoneHome_GotFocus);
            this.txtTelephoneHome.LostFocus += new System.EventHandler(this.txtTelephoneHome_LostFocus);
            // 
            // txtFax
            // 
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFax.Location = new System.Drawing.Point(462, 218);
            this.txtFax.MaxLength = 50;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(150, 21);
            this.txtFax.TabIndex = 19;
            this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
            this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
            // 
            // lblFactSetUsernameAndSerialNumber
            // 
            this.lblFactSetUsernameAndSerialNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFactSetUsernameAndSerialNumber.Location = new System.Drawing.Point(3, 2);
            this.lblFactSetUsernameAndSerialNumber.Name = "lblFactSetUsernameAndSerialNumber";
            this.lblFactSetUsernameAndSerialNumber.Size = new System.Drawing.Size(98, 29);
            this.lblFactSetUsernameAndSerialNumber.TabIndex = 6;
            this.lblFactSetUsernameAndSerialNumber.Text = "FactSet Username && Serial No";
            // 
            // txtFactSetUsernameAndSerialNumber
            // 
            this.txtFactSetUsernameAndSerialNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFactSetUsernameAndSerialNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFactSetUsernameAndSerialNumber.Location = new System.Drawing.Point(120, 3);
            this.txtFactSetUsernameAndSerialNumber.MaxLength = 200;
            this.txtFactSetUsernameAndSerialNumber.Name = "txtFactSetUsernameAndSerialNumber";
            this.txtFactSetUsernameAndSerialNumber.Size = new System.Drawing.Size(150, 21);
            this.txtFactSetUsernameAndSerialNumber.TabIndex = 180;
            // 
            // ultraPanelFactSet
            // 
            // 
            // ultraPanelFactSet.ClientArea
            // 
            this.ultraPanelFactSet.ClientArea.Controls.Add(this.lblFactSetUsernameAndSerialNumber);
            this.ultraPanelFactSet.ClientArea.Controls.Add(this.lblFactSetSupportUser);
            this.ultraPanelFactSet.ClientArea.Controls.Add(this.txtFactSetUsernameAndSerialNumber);
            this.ultraPanelFactSet.ClientArea.Controls.Add(this.ultraComboFactSetConnection);
            this.ultraPanelFactSet.Location = new System.Drawing.Point(2, 264);
            this.ultraPanelFactSet.Name = "ultraPanelFactSet";
            this.ultraPanelFactSet.Size = new System.Drawing.Size(328, 54);
            this.ultraPanelFactSet.TabIndex = 184;
            this.ultraPanelFactSet.Visible = false;
            // 
            // ultraPanelActiv
            // 
            // 
            // ultraPanelActiv.ClientArea
            // 
            this.ultraPanelActiv.ClientArea.Controls.Add(this.labelActivPassword);
            this.ultraPanelActiv.ClientArea.Controls.Add(this.txtActivPassword);
            this.ultraPanelActiv.ClientArea.Controls.Add(this.labelActivUsername);
            this.ultraPanelActiv.ClientArea.Controls.Add(this.txtActivUsername);
            this.ultraPanelActiv.Location = new System.Drawing.Point(2, 264);
            this.ultraPanelActiv.Name = "ultraPanelActiv";
            this.ultraPanelActiv.Size = new System.Drawing.Size(328, 54);
            this.ultraPanelActiv.TabIndex = 185;
            this.ultraPanelActiv.Visible = false;
            // 
            // labelActivUsername
            // 
            this.labelActivUsername.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelActivUsername.Location = new System.Drawing.Point(3, 3);
            this.labelActivUsername.Name = "labelActivUsername";
            this.labelActivUsername.Size = new System.Drawing.Size(98, 18);
            this.labelActivUsername.TabIndex = 6;
            this.labelActivUsername.Text = "ACTIV Username";
            // 
            // txtActivUsername
            // 
            this.txtActivUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtActivUsername.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtActivUsername.Location = new System.Drawing.Point(120, 3);
            this.txtActivUsername.MaxLength = 200;
            this.txtActivUsername.Name = "txtActivUsername";
            this.txtActivUsername.Size = new System.Drawing.Size(150, 21);
            this.txtActivUsername.TabIndex = 180;
            // 
            // labelActivPassword
            // 
            this.labelActivPassword.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelActivPassword.Location = new System.Drawing.Point(3, 29);
            this.labelActivPassword.Name = "labelActivPassword";
            this.labelActivPassword.Size = new System.Drawing.Size(98, 18);
            this.labelActivPassword.TabIndex = 181;
            this.labelActivPassword.Text = "ACTIV Password";
            // 
            // txtActivPassword
            // 
            this.txtActivPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtActivPassword.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtActivPassword.Location = new System.Drawing.Point(120, 29);
            this.txtActivPassword.MaxLength = 200;
            this.txtActivPassword.Name = "txtActivPassword";
            this.txtActivPassword.Size = new System.Drawing.Size(150, 21);
            this.txtActivPassword.TabIndex = 182;
            this.txtActivPassword.UseSystemPasswordChar = true;
            // 
            // ultraPanelSapi
            // 
            // 
            // ultraPanelSapi.ClientArea
            // 
            this.ultraPanelSapi.ClientArea.Controls.Add(this.labelSapiUsername);
            this.ultraPanelSapi.ClientArea.Controls.Add(this.txtSapiUsername);
            this.ultraPanelSapi.Location = new System.Drawing.Point(2, 264);
            this.ultraPanelSapi.Name = "ultraPanelSapi";
            this.ultraPanelSapi.Size = new System.Drawing.Size(328, 54);
            this.ultraPanelSapi.TabIndex = 185;
            this.ultraPanelSapi.Visible = false;
            // 
            // labelSapiUsername
            // 
            this.labelSapiUsername.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelSapiUsername.Location = new System.Drawing.Point(3, 3);
            this.labelSapiUsername.Name = "labelSapiUsername";
            this.labelSapiUsername.Size = new System.Drawing.Size(98, 18);
            this.labelSapiUsername.TabIndex = 6;
            this.labelSapiUsername.Text = "SAPI Username";
            // 
            // txtSapiUsername
            // 
            this.txtSapiUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSapiUsername.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSapiUsername.Location = new System.Drawing.Point(120, 3);
            this.txtSapiUsername.MaxLength = 200;
            this.txtSapiUsername.Name = "txtSapiUsername";
            this.txtSapiUsername.Size = new System.Drawing.Size(150, 21);
            this.txtSapiUsername.TabIndex = 180;
            // 
            // CompanyUser
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CompanyUser";
            this.Size = new System.Drawing.Size(632, 388);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboFactSetConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingPermission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
            this.ultraPanelFactSet.ClientArea.ResumeLayout(false);
            this.ultraPanelFactSet.ClientArea.PerformLayout();
            this.ultraPanelFactSet.ResumeLayout(false);
            this.ultraPanelActiv.ClientArea.ResumeLayout(false);
            this.ultraPanelActiv.ClientArea.PerformLayout();
            this.ultraPanelActiv.ResumeLayout(false);
            this.ultraPanelSapi.ClientArea.ResumeLayout(false);
            this.ultraPanelSapi.ClientArea.PerformLayout();
            this.ultraPanelSapi.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Public Properties and Methods

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

        /// <summary>
        /// This method binds the TradingPermission in the ComboBox control by assigning the 
        /// GetDataTable method which binds default data to its datasource property.
        /// </summary>
        private void BindTradingPermission()
        {
            cmbTradingPermission.DataSource = null;
            cmbTradingPermission.DataSource = GetDataTable();
            cmbTradingPermission.DisplayMember = "Data";
            cmbTradingPermission.ValueMember = "Value";
            cmbTradingPermission.Text = C_COMBO_SELECT;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbTradingPermission.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("Data"))
                {
                    column.Hidden = true;
                }
            }
            cmbTradingPermission.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }
        //User property sets the CompanyUser form and get the CompanyUser details from it. 
        public User User
        {
            get { return GetUserDetail(); }
            //set{SetUserDetail(value);}
        }

        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 1)]
        public void SetupControl(int companyID, int companyUserID)
        {
            _companyID = companyID;
            BindTradingPermission();
            BindCountries();
            BindStates();

            if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.FactSet)
            {
                ShowHideFactSetUserDetails(true);
            }
            else
            {
                ShowHideFactSetUserDetails(false);
            }

            if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.ACTIV)
            {
                ShowHideActivUserDetails(true);
            }
            else
            {
                ShowHideActivUserDetails(false);
            }

            if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI)
            {
                ShowHideSapiUserDetails(true);
            }
            else
            {
                ShowHideSapiUserDetails(false);
            }

            BindFactSetConnections();
            Prana.Admin.BLL.User user = UserManager.GetCompanyUserBoth(_companyID, companyUserID);
            SetUserDetail(user);
        }

        private void BindFactSetConnections()
        {
            Dictionary<string, int> factSetConnections = new Dictionary<string, int>();
            factSetConnections.Add("Client's server-level connection", 0);
            factSetConnections.Add("Support's server-level connection", 1);

            ultraComboFactSetConnection.DataSource = factSetConnections;
            ultraComboFactSetConnection.DisplayMember = "Key";
            ultraComboFactSetConnection.ValueMember = "Value";
            ultraComboFactSetConnection.DataBind();
            ultraComboFactSetConnection.Value = 0;

            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in ultraComboFactSetConnection.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("Key"))
                {
                    column.Hidden = true;
                }
            }

            ultraComboFactSetConnection.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        public void RefreshUserDetail()
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtEMail.Text = "";
            txtFax.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtLoginName.Text = "";
            txtLoginName.Tag = int.MinValue;
            txtPassword.Text = "";
            txtShortName.Text = "";
            txtTelephoneHome.Text = "";
            txtTelephoneCell.Text = "";
            txtPager.Text = "";
            txtTelephoneWork.Text = "";
            txtTitle.Text = "";
            cmbCountry.Text = C_COMBO_SELECT;
            cmbState.Text = C_COMBO_SELECT;
            cmbState.Text = "";
            txtZip.Text = "";
            cmbTradingPermission.Value = C_COMBO_SELECT;
            txtCity.Text = "";
            txtFactSetUsernameAndSerialNumber.Text = "";
            txtActivUsername.Text = "";
            txtActivPassword.Text = "";
            txtWebAzureId.Text = "";
            txtSapiUsername.Text = "";
        }

        //Bind Permission

        public int SaveCompanyUser()
        {
            int result = int.MinValue;

            if (txtFirstName.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter First name!");
                txtFirstName.Focus();
            }
            else if (txtLastName.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Last name!");
                txtLastName.Focus();
            }
            else if (txtLoginName.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Stored!");
                txtLoginName.Focus();
            }
            else if (txtPassword.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Password name!");
                txtPassword.Focus();
            }
            else if (txtShortName.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Short name!");
                txtShortName.Focus();
            }
            else if (txtTitle.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Title!");
                txtTitle.Focus();
            }

            else if (txtAddress1.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Address1!");
                txtAddress1.Focus();
            }
            else if (txtAddress2.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Address2!");
                txtAddress2.Focus();
            }
            else if (txtEMail.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Email!");
                txtEMail.Focus();
            }
            else if (txtTelephoneWork.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Work Telephone!");
                txtTelephoneWork.Focus();
            }
            else if (txtTelephoneCell.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Cell no!");
                txtTelephoneCell.Focus();
            }
            else if (txtPager.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Pager no!");
                txtPager.Focus();
            }
            else if (txtTelephoneHome.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Home Telephone!");
                txtTelephoneHome.Focus();
            }
            else if (txtFax.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Fax no!");
                txtFax.Focus();
            }
            else if (txtWebAzureId.Text == "")
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Short name!");
                txtWebAzureId.Focus();
            }
            //			else if(int.Parse(cmbTradingPermission.SelectedValue.ToString()) == int.MinValue)
            //			{
            //				//Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
            //				//Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please select Trading Permission!");
            //				cmbTradingPermission.Focus();
            //			}
            else
            {
                User user = new User();
                //user.UserID
                user.Address1 = txtAddress1.Text.Trim();
                user.Address2 = txtAddress2.Text.Trim();
                user.EMail = txtEMail.Text.Trim();
                user.Fax = txtFax.Text.Trim();
                user.FirstName = txtFirstName.Text.Trim();
                user.LastName = txtLastName.Text.Trim();
                user.LoginName = txtLoginName.Text.Trim();

                user.Password = txtPassword.Text.Trim();
                user.ShortName = txtShortName.Text.Trim();
                user.TelephoneHome = txtTelephoneHome.Text.Trim();
                user.TelephoneMobile = txtTelephoneCell.Text.Trim();
                user.TelephoneWork = txtTelephoneWork.Text.Trim();
                user.Title = txtTitle.Text.Trim();
                user.FactSetUsernameAndSerialNumber = txtFactSetUsernameAndSerialNumber.Text.Trim();
                user.IsFactSetSupportUser = Convert.ToBoolean(ultraComboFactSetConnection.Value);

                user.ActivUsername = txtActivUsername.Text.Trim();
                user.ActivPassword = txtActivPassword.Text.Trim();
                user.SamsaraAzureId = txtWebAzureId.Text.Trim();
                user.SapiUsername = txtSapiUsername.Text.Trim();

                result = 1;
            }
            return result;
        }
        #endregion

        #region Private methods
        private void SetUserDetail(User user)
        {
            txtAddress1.Text = user.Address1;
            txtAddress2.Text = user.Address2;
            cmbCountry.Value = int.Parse(user.CountryID.ToString());
            if (user.CountryID == int.MinValue)
            {
                BindEmptyStates();
            }
            cmbState.Value = int.Parse(user.StateID.ToString());
            txtEMail.Text = user.EMail;
            txtFax.Text = user.Fax;
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtLoginName.Text = user.LoginName;
            txtZip.Text = user.Zip;
            txtPassword.Text = string.IsNullOrEmpty(user.Password) ? user.Password : ApplicationConstants.DUMMY_PASSWORD;
            txtShortName.Text = user.ShortName;
            txtTelephoneHome.Text = user.TelephoneHome;
            txtTelephoneCell.Text = user.TelephoneMobile;
            txtPager.Text = user.TelephonePager;
            txtTelephoneWork.Text = user.TelephoneWork;
            txtTitle.Text = user.Title;
            txtFactSetUsernameAndSerialNumber.Text = user.FactSetUsernameAndSerialNumber;
            ultraComboFactSetConnection.Value = Convert.ToInt32(user.IsFactSetSupportUser);
            txtActivUsername.Text = user.ActivUsername;
            txtActivPassword.Text = user.ActivPassword;
            cmbTradingPermission.Value = int.Parse(user.TradingPermission.ToString());
            txtCity.Text = user.City;
            txtWebAzureId.Text = user.SamsaraAzureId;
            txtSapiUsername.Text = user.SapiUsername;
        }


        //[AuditSourceMethAttri(AuditMehodType.ReturnType,-1, AuditManager.Definitions.Enum.AuditAction.UserUpdated)]
        private User GetUserDetail()
        {
            User user = null;
            //Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
            string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex emailRegex = new Regex(emailCheck);
            Match emailMatch = emailRegex.Match(txtEMail.Text.ToString());
            Match samsaraEmailMatch = emailRegex.Match(txtWebAzureId.Text.ToString());

            errorProvider1.SetError(txtFirstName, "");
            errorProvider1.SetError(txtLoginName, "");
            errorProvider1.SetError(txtPassword, "");
            errorProvider1.SetError(txtFactSetUsernameAndSerialNumber, "");
            errorProvider1.SetError(txtActivUsername, "");
            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(txtZip, "");
            errorProvider1.SetError(txtAddress1, "");
            errorProvider1.SetError(cmbCountry, "");
            errorProvider1.SetError(cmbState, "");
            errorProvider1.SetError(txtEMail, "");
            errorProvider1.SetError(txtTelephoneWork, "");
            errorProvider1.SetError(cmbTradingPermission, "");
            errorProvider1.SetError(txtCity, "");
            errorProvider1.SetError(txtTelephoneCell, "");
            errorProvider1.SetError(txtWebAzureId, "");
            errorProvider1.SetError(txtSapiUsername, "");
            if (txtFirstName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtFirstName, "Please enter First Name in details!");
                txtFirstName.Focus();
            }
            else if (txtShortName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtShortName, "Please enter Short name!");
                txtShortName.Focus();
            }
            else if (txtLoginName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtLoginName, "Please enter Login Name!");
                txtLoginName.Focus();
            }
            else if (txtPassword.Text.Trim() == "")
            {
                errorProvider1.SetError(txtPassword, "Please enter Password!");
                txtPassword.Focus();
            }
            else if (int.Parse(txtPassword.Text.Trim().Length.ToString()) < 4)
            {
                errorProvider1.SetError(txtPassword, "Please enter password having at least four characters !");
                txtPassword.Focus();
            }
            else if (txtAddress1.Text.Trim() == "")
            {
                errorProvider1.SetError(txtAddress1, "Please enter Address1");
                txtAddress1.Focus();
            }
            else if (int.Parse(cmbCountry.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCountry, "Please select Country!");
                cmbCountry.Focus();
            }
            else if (int.Parse(cmbState.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbState, "Please select State!");
                cmbState.Focus();
            }
            else if (txtCity.Text.Trim() == "")
            {
                errorProvider1.SetError(txtCity, "Please enter City");
                txtCity.Focus();
            }
            else if (!emailMatch.Success)
            {
                errorProvider1.SetError(txtEMail, "Please enter valid Email address!");
                txtEMail.Focus();
            }
            else if (!string.IsNullOrEmpty(txtWebAzureId.Text) && !samsaraEmailMatch.Success)
            {
                errorProvider1.SetError(txtWebAzureId, "Please enter valid Email address!");
                txtWebAzureId.Focus();
            }
            else if (txtTelephoneWork.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTelephoneWork, "Please enter Work Telephone!");
                txtTelephoneWork.Focus();
            }
            else if (txtTelephoneCell.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTelephoneCell, "Please enter Cell# !");
                txtTelephoneCell.Focus();
            }

            else if (int.Parse(cmbTradingPermission.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbTradingPermission, "Please select Trading Permission!");
                cmbTradingPermission.Focus();
            }
            else if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.FactSet && CommonDataCache.CachedDataManager.CompanyFactSetContractType != BusinessObjects.AppConstants.FactSetContractType.Reseller && !string.IsNullOrWhiteSpace(txtFactSetUsernameAndSerialNumber.Text) && UserManager.CheckMarketDataProviderAccessIDDuplication(txtLoginName.Text.Trim(), txtFactSetUsernameAndSerialNumber.Text.Trim()) > 0)
            {
                errorProvider1.SetError(txtFactSetUsernameAndSerialNumber, "FactSet Username-Serial No. already exists!");
                txtFactSetUsernameAndSerialNumber.Focus();
            }
            else if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.ACTIV && !string.IsNullOrWhiteSpace(txtActivUsername.Text) && UserManager.CheckMarketDataProviderAccessIDDuplication(txtLoginName.Text.Trim(), txtActivUsername.Text.Trim()) > 0)
            {
                errorProvider1.SetError(txtActivUsername, "ACTIV Username already exists!");
                txtActivUsername.Focus();
            }
            else if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && !string.IsNullOrWhiteSpace(txtSapiUsername.Text) && UserManager.CheckMarketDataProviderAccessIDDuplication(txtLoginName.Text.Trim(), txtSapiUsername.Text.Trim()) > 0)
            {
                errorProvider1.SetError(txtSapiUsername, "SAPI Username already exists!");
                txtSapiUsername.Focus();
            }
            else
            {
                user = new User();
                user.Address1 = txtAddress1.Text;
                user.Address2 = txtAddress2.Text;
                user.CountryID = int.Parse(cmbCountry.Value.ToString());
                user.StateID = int.Parse(cmbState.Value.ToString());
                user.EMail = txtEMail.Text;
                user.Fax = txtFax.Text;
                user.FirstName = txtFirstName.Text;
                user.LastName = txtLastName.Text;
                user.LoginName = txtLoginName.Text;
                user.Password = txtPassword.Text;
                user.ShortName = txtShortName.Text;
                user.Zip = txtZip.Text;
                user.TelephoneHome = txtTelephoneHome.Text;
                user.TelephoneMobile = txtTelephoneCell.Text;
                user.TelephonePager = txtPager.Text;
                user.TelephoneWork = txtTelephoneWork.Text;
                user.Title = txtTitle.Text;
                user.TradingPermission = int.Parse(cmbTradingPermission.Value.ToString());
                user.City = txtCity.Text;
                user.FactSetUsernameAndSerialNumber = txtFactSetUsernameAndSerialNumber.Text.Trim();
                user.IsFactSetSupportUser = Convert.ToBoolean(ultraComboFactSetConnection.Value);

                user.ActivUsername = txtActivUsername.Text.Trim();
                user.ActivPassword = txtActivPassword.Text.Trim();
                user.SamsaraAzureId = txtWebAzureId.Text.Trim();
                user.SapiUsername = txtSapiUsername.Text.Trim();
            }
            return user;
        }

        #endregion

        private System.Data.DataTable GetDataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            object[] row = new object[2];
            dt.Columns.Add("Data");
            dt.Columns.Add("Value");

            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            dt.Rows.Add(row);

            row = new object[2];
            row[0] = "Yes";
            row[1] = "1";
            dt.Rows.Add(row);
            row[0] = "No";
            row[1] = "0";
            dt.Rows.Add(row);
            return dt;
        }

        #region Focus Colors
        private void txtFirstName_GotFocus(object sender, System.EventArgs e)
        {
            txtFirstName.BackColor = Color.LemonChiffon;
        }
        private void txtFirstName_LostFocus(object sender, System.EventArgs e)
        {
            txtFirstName.BackColor = Color.White;
        }
        private void txtLastName_GotFocus(object sender, System.EventArgs e)
        {
            txtLastName.BackColor = Color.LemonChiffon;
        }
        private void txtLastName_LostFocus(object sender, System.EventArgs e)
        {
            txtLastName.BackColor = Color.White;
        }
        private void txtLoginName_GotFocus(object sender, System.EventArgs e)
        {
            txtLoginName.BackColor = Color.LemonChiffon;
        }
        private void txtLoginName_LostFocus(object sender, System.EventArgs e)
        {
            txtLoginName.BackColor = Color.White;
        }
        private void txtPassword_GotFocus(object sender, System.EventArgs e)
        {
            txtPassword.BackColor = Color.LemonChiffon;
        }
        private void txtPassword_LostFocus(object sender, System.EventArgs e)
        {
            txtPassword.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        private void txtTitle_GotFocus(object sender, System.EventArgs e)
        {
            txtTitle.BackColor = Color.LemonChiffon;
        }
        private void txtTitle_LostFocus(object sender, System.EventArgs e)
        {
            txtTitle.BackColor = Color.White;
        }
        private void txtZip_GotFocus(object sender, System.EventArgs e)
        {
            txtZip.BackColor = Color.LemonChiffon;
        }
        private void txtZip_LostFocus(object sender, System.EventArgs e)
        {
            txtZip.BackColor = Color.White;
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
        private void txtEMail_GotFocus(object sender, System.EventArgs e)
        {
            txtEMail.BackColor = Color.LemonChiffon;
        }
        private void txtEMail_LostFocus(object sender, System.EventArgs e)
        {
            txtEMail.BackColor = Color.White;
        }
        private void txtTelephoneWork_GotFocus(object sender, System.EventArgs e)
        {
            txtTelephoneWork.BackColor = Color.LemonChiffon;
        }
        private void txtTelephoneWork_LostFocus(object sender, System.EventArgs e)
        {
            txtTelephoneWork.BackColor = Color.White;
        }
        private void txtTelephoneCell_GotFocus(object sender, System.EventArgs e)
        {
            txtTelephoneCell.BackColor = Color.LemonChiffon;
        }
        private void txtTelephoneCell_LostFocus(object sender, System.EventArgs e)
        {
            txtTelephoneCell.BackColor = Color.White;
        }
        private void txtPager_GotFocus(object sender, System.EventArgs e)
        {
            txtPager.BackColor = Color.LemonChiffon;
        }
        private void txtPager_LostFocus(object sender, System.EventArgs e)
        {
            txtPager.BackColor = Color.White;
        }
        private void txtTelephoneHome_GotFocus(object sender, System.EventArgs e)
        {
            txtTelephoneHome.BackColor = Color.LemonChiffon;
        }
        private void txtTelephoneHome_LostFocus(object sender, System.EventArgs e)
        {
            txtTelephoneHome.BackColor = Color.White;
        }
        private void txtFax_GotFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.LemonChiffon;
        }
        private void txtFax_LostFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.White;
        }
        private void cmbTradingPermission_GotFocus(object sender, System.EventArgs e)
        {
            cmbTradingPermission.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbTradingPermission_LostFocus(object sender, System.EventArgs e)
        {
            cmbTradingPermission.Appearance.BackColor = Color.White;
        }
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
        private void txtWebAzureId_GotFocus(object sender, System.EventArgs e)
        {
            txtWebAzureId.BackColor = Color.LemonChiffon;
        }
        private void txtWebAzureId_LostFocus(object sender, System.EventArgs e)
        {
            txtWebAzureId.BackColor = Color.White;
        }

        #endregion

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
                    }
                }
                else
                {
                    BindEmptyStates();
                }
            }
        }

        private void ShowHideFactSetUserDetails(bool isVisible)
        {
            ultraPanelFactSet.Visible = isVisible;
        }

        private void ShowHideActivUserDetails(bool isVisible)
        {
            ultraPanelActiv.Visible = isVisible;
        }

        /// <summary>
        /// Shows/hides Sapi user details.
        /// </summary>
        /// <param name="isVisible"></param>
        private void ShowHideSapiUserDetails(bool isVisible)
        {
            ultraPanelSapi.Visible = isVisible;
        }
    }
}
