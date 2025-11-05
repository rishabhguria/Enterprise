using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Admin.Controls.ThirdParty;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ThirdPartyManager.DataAccess;
using Prana.ThirdPartyUI;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for ThirdPartyForm.
    /// </summary>
    public class ThirdPartyForm : System.Windows.Forms.UserControl
    {
        //const string C_COMBO_SELECT = "- Select -";
        const int TYPE_VENDOR = 2;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPCWorkTele;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBrokerCode;
        private System.Windows.Forms.TextBox txtBrokerCode;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.TextBox txtContactPerson;
        private System.Windows.Forms.TextBox txtThirdPartyName;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbThirdPartyType;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtCellPhone;
        private System.Windows.Forms.Label lblPCCellPhone;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbState;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCountry;
        private Label lblZip;
        private TextBox txtZip;
        private Label lblStateTerritory;
        private Label lblCountry;
        private GroupBox grpPrimaryContact;
        private Label lblLastName;
        private TextBox txtLastName;
        private Label lblTitle;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTitle;
        private Label label20;
        private TextBox txtWorkTele;
        private Label label24;
        private TextBox txtPCFax;
        private Label lblPCFax;
        private SaveFileDialog saveFileDialog1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private GroupBox groupBox2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdFileFormat;
        private OpenFileDialog openFileDialog1;
        //private GroupBox groupBox3;
        private Button btnDeleteFileFormat;
        private Label label7;
        private UltraCombo cmbSecurityIdentifier;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;

        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;

        private Prana.Admin.CommonControls.CntrlBatchSetup cntrlBatchSetup1;
        private SplitContainer splitContainer1;
        private ThirdParty.FileSettingSetup ctrlFileSetting;
        private Infragistics.Win.Misc.UltraButton btnAdvance;
        private IContainer components;
        private Controls.ThirdParty.ThirdPartyAudit thirdPartyAudit;
        private GroupBox groupBox3;
        private Label label13;
        private UltraCombo cmbCounterParty;
        private Label label12;
        private Label label15;
        private bool _isExecutingBroker;

        private const string COLUMN_TIME_BATCHES_ENABLED = "TimeBatchesEnabled";
        private const string COLUMN_SET_EDIT_BATCH = "Set/Edit Batch";

        public ThirdPartyForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox1.ForeColor = System.Drawing.Color.White;
                this.grpPrimaryContact.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grpPrimaryContact.ForeColor = System.Drawing.Color.White;
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.ultraTabControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.ultraTabControl1.Appearance.ForeColor = System.Drawing.Color.White;

                this.ultraTabControl1.ActiveTabAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                this.ultraTabControl1.ActiveTabAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.ultraTabControl1.ActiveTabAppearance.ForeColor = System.Drawing.Color.White;
                this.ctrlFileSetting.ApplyTheme();
                this.thirdPartyAudit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.btnAdvance.Anchor = AnchorStyles.Right;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
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
                if (_headerCheckBoxUnallocated != null)
                {
                    _headerCheckBoxUnallocated.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (txtPCWorkTele != null)
                {
                    txtPCWorkTele.Dispose();
                }
                if (txtAddress2 != null)
                {
                    txtAddress2.Dispose();
                }
                if (txtAddress1 != null)
                {
                    txtAddress1.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (lblBrokerCode != null)
                {
                    lblBrokerCode.Dispose();
                }
                if (txtBrokerCode != null)
                {
                    txtBrokerCode.Dispose();
                }
                if (txtEmail != null)
                {
                    txtEmail.Dispose();
                }
                if (txtFax != null)
                {
                    txtFax.Dispose();
                }
                if (txtContactPerson != null)
                {
                    txtContactPerson.Dispose();
                }
                if (txtThirdPartyName != null)
                {
                    txtThirdPartyName.Dispose();
                }
                if (cmbThirdPartyType != null)
                {
                    cmbThirdPartyType.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (label18 != null)
                {
                    label18.Dispose();
                }
                if (txtCellPhone != null)
                {
                    txtCellPhone.Dispose();
                }
                if (lblPCCellPhone != null)
                {
                    lblPCCellPhone.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label17 != null)
                {
                    label17.Dispose();
                }
                if (cmbState != null)
                {
                    cmbState.Dispose();
                }
                if (cmbCountry != null)
                {
                    cmbCountry.Dispose();
                }
                if (lblZip != null)
                {
                    lblZip.Dispose();
                }
                if (txtZip != null)
                {
                    txtZip.Dispose();
                }
                if (lblStateTerritory != null)
                {
                    lblStateTerritory.Dispose();
                }
                if (lblCountry != null)
                {
                    lblCountry.Dispose();
                }
                if (grpPrimaryContact != null)
                {
                    grpPrimaryContact.Dispose();
                }
                if (lblLastName != null)
                {
                    lblLastName.Dispose();
                }
                if (txtLastName != null)
                {
                    txtLastName.Dispose();
                }
                if (lblTitle != null)
                {
                    lblTitle.Dispose();
                }
                if (cmbTitle != null)
                {
                    cmbTitle.Dispose();
                }
                if (label20 != null)
                {
                    label20.Dispose();
                }
                if (txtWorkTele != null)
                {
                    txtWorkTele.Dispose();
                }
                if (label24 != null)
                {
                    label24.Dispose();
                }
                if (txtPCFax != null)
                {
                    txtPCFax.Dispose();
                }
                if (lblPCFax != null)
                {
                    lblPCFax.Dispose();
                }
                if (saveFileDialog1 != null)
                {
                    saveFileDialog1.Dispose();
                }
                if (ultraTabControl1 != null)
                {
                    ultraTabControl1.Dispose();
                }
                if (ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if (ultraTabPageControl1 != null)
                {
                    ultraTabPageControl1.Dispose();
                }
                if (ultraTabPageControl2 != null)
                {
                    ultraTabPageControl2.Dispose();
                }
                if (groupBox2 != null)
                {
                    groupBox2.Dispose();
                }
                if (grdFileFormat != null)
                {
                    grdFileFormat.Dispose();
                }
                if (grdFileFormat != null)
                {
                    grdFileFormat.Dispose();
                }
                if (openFileDialog1 != null)
                {
                    openFileDialog1.Dispose();
                }
                if (btnDeleteFileFormat != null)
                {
                    btnDeleteFileFormat.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (cmbSecurityIdentifier != null)
                {
                    cmbSecurityIdentifier.Dispose();
                }
                if (ultraTabPageControl3 != null)
                {
                    ultraTabPageControl3.Dispose();
                }
                if (ultraTabPageControl4 != null)
                {
                    ultraTabPageControl4.Dispose();
                }
                if (cntrlBatchSetup1 != null)
                {
                    cntrlBatchSetup1.Dispose();
                }
                if (splitContainer1 != null)
                {
                    splitContainer1.Dispose();
                }
                if (ctrlFileSetting != null)
                {
                    ctrlFileSetting.Dispose();
                }
                if (btnAdvance != null)
                {
                    btnAdvance.Dispose();
                }
                if (thirdPartyAudit != null)
                {
                    thirdPartyAudit.Dispose();
                }
                if(cmbCounterParty != null)
                {
                    cmbCounterParty.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 2);
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
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("List`1", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value");
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FileFormatName", 0);
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn51 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PranaToThirdParty", 1);
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SelectPranaTo", 2);
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FileFormatId", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn55 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HeaderFile", 5);
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn56 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SelectHeaderFile", 6);
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn57 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FooterFile", 7);
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn58 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SelectFooterFile", 8);
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn59 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(COLUMN_SET_EDIT_BATCH, 9);
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbSecurityIdentifier = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label20 = new System.Windows.Forms.Label();
            this.txtWorkTele = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cmbState = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblZip = new System.Windows.Forms.Label();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.lblStateTerritory = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbThirdPartyType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.lblBrokerCode = new System.Windows.Forms.Label();
            this.txtBrokerCode = new System.Windows.Forms.TextBox();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.txtThirdPartyName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.grpPrimaryContact = new System.Windows.Forms.GroupBox();
            this.txtPCFax = new System.Windows.Forms.TextBox();
            this.lblPCFax = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.cmbTitle = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtContactPerson = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCellPhone = new System.Windows.Forms.TextBox();
            this.lblPCCellPhone = new System.Windows.Forms.Label();
            this.txtPCWorkTele = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.thirdPartyAudit = new Prana.Admin.Controls.ThirdParty.ThirdPartyAudit();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDeleteFileFormat = new System.Windows.Forms.Button();
            this.grdFileFormat = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnAdvance = new Infragistics.Win.Misc.UltraButton();
            this.ctrlFileSetting = new Prana.Admin.Controls.ThirdParty.FileSettingSetup();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.cntrlBatchSetup1 = new Prana.Admin.CommonControls.CntrlBatchSetup();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ultraTabPageControl1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecurityIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdPartyType)).BeginInit();
            this.grpPrimaryContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTitle)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFileFormat)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.groupBox3);
            this.ultraTabPageControl1.Controls.Add(this.groupBox1);
            this.ultraTabPageControl1.Controls.Add(this.label17);
            this.ultraTabPageControl1.Controls.Add(this.grpPrimaryContact);
            this.ultraTabPageControl1.Controls.Add(this.thirdPartyAudit);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(847, 446);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.cmbCounterParty);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox3.Location = new System.Drawing.Point(400, 250);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(327, 65);
            this.groupBox3.TabIndex = 180;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mapping";
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(123, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(8, 8);
            this.label12.TabIndex = 184;
            this.label12.Text = "*";
            // 
            // cmbCounterParty
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance1;
            this.cmbCounterParty.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn45.Header.VisiblePosition = 0;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn46.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn45,
            ultraGridColumn46});
            this.cmbCounterParty.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterParty.DropDownWidth = 0;
            this.cmbCounterParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCounterParty.Location = new System.Drawing.Point(144, 27);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(160, 21);
            this.cmbCounterParty.TabIndex = 183;
            this.cmbCounterParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(6, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 13);
            this.label13.TabIndex = 182;
            this.label13.Text = "Counterparty mapping";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbSecurityIdentifier);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtWorkTele);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.cmbState);
            this.groupBox1.Controls.Add(this.cmbCountry);
            this.groupBox1.Controls.Add(this.lblZip);
            this.groupBox1.Controls.Add(this.txtZip);
            this.groupBox1.Controls.Add(this.lblStateTerritory);
            this.groupBox1.Controls.Add(this.lblCountry);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmbThirdPartyType);
            this.groupBox1.Controls.Add(this.txtFax);
            this.groupBox1.Controls.Add(this.lblBrokerCode);
            this.groupBox1.Controls.Add(this.txtBrokerCode);
            this.groupBox1.Controls.Add(this.txtAddress2);
            this.groupBox1.Controls.Add(this.txtAddress1);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Controls.Add(this.txtThirdPartyName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(31, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 305);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contact Details";
            // 
            // label15
            // 
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(107, 254);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(8, 8);
            this.label15.TabIndex = 204;
            this.label15.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(10, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 203;
            this.label7.Text = "Security Identifier";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSecurityIdentifier
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSecurityIdentifier.DisplayLayout.Appearance = appearance13;
            this.cmbSecurityIdentifier.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn40.Header.VisiblePosition = 0;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn41.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn40,
            ultraGridColumn41});
            this.cmbSecurityIdentifier.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbSecurityIdentifier.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSecurityIdentifier.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSecurityIdentifier.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSecurityIdentifier.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbSecurityIdentifier.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSecurityIdentifier.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbSecurityIdentifier.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSecurityIdentifier.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSecurityIdentifier.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSecurityIdentifier.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbSecurityIdentifier.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSecurityIdentifier.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSecurityIdentifier.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSecurityIdentifier.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbSecurityIdentifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSecurityIdentifier.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSecurityIdentifier.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbSecurityIdentifier.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbSecurityIdentifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSecurityIdentifier.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbSecurityIdentifier.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbSecurityIdentifier.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSecurityIdentifier.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbSecurityIdentifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSecurityIdentifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSecurityIdentifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSecurityIdentifier.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSecurityIdentifier.DropDownWidth = 0;
            this.cmbSecurityIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbSecurityIdentifier.Location = new System.Drawing.Point(130, 251);
            this.cmbSecurityIdentifier.Name = "cmbSecurityIdentifier";
            this.cmbSecurityIdentifier.Size = new System.Drawing.Size(160, 21);
            this.cmbSecurityIdentifier.TabIndex = 11;
            this.cmbSecurityIdentifier.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbSecurityIdentifier.GotFocus += new System.EventHandler(this.cmbSecurityIdentifier_GotFocus);
            this.cmbSecurityIdentifier.LostFocus += new System.EventHandler(this.cmbSecurityIdentifier_LostFocus);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label20.Location = new System.Drawing.Point(10, 208);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(83, 13);
            this.label20.TabIndex = 201;
            this.label20.Text = "(1-111-111111)";
            // 
            // txtWorkTele
            // 
            this.txtWorkTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWorkTele.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtWorkTele.Location = new System.Drawing.Point(130, 194);
            this.txtWorkTele.MaxLength = 50;
            this.txtWorkTele.Name = "txtWorkTele";
            this.txtWorkTele.Size = new System.Drawing.Size(160, 21);
            this.txtWorkTele.TabIndex = 9;
            this.txtWorkTele.TextChanged += new System.EventHandler(this.txtWorkTele_TextChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label24.Location = new System.Drawing.Point(10, 196);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(43, 13);
            this.label24.TabIndex = 199;
            this.label24.Text = "Work #";
            // 
            // cmbState
            // 
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbState.DisplayLayout.Appearance = appearance25;
            this.cmbState.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn42.Header.VisiblePosition = 0;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn43.Header.VisiblePosition = 1;
            ultraGridColumn44.Header.VisiblePosition = 2;
            ultraGridColumn44.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn42,
            ultraGridColumn43,
            ultraGridColumn44});
            this.cmbState.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbState.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbState.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbState.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbState.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbState.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbState.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbState.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbState.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbState.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbState.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbState.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbState.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.cmbState.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbState.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbState.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbState.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbState.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbState.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbState.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbState.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbState.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbState.DropDownWidth = 0;
            this.cmbState.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbState.Location = new System.Drawing.Point(130, 150);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(160, 21);
            this.cmbState.TabIndex = 7;
            this.cmbState.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbState.GotFocus += new System.EventHandler(this.cmbState_GotFocus);
            this.cmbState.LostFocus += new System.EventHandler(this.cmbState_LostFocus);
            // 
            // cmbCountry
            // 
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCountry.DisplayLayout.Appearance = appearance37;
            this.cmbCountry.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbCountry.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbCountry.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCountry.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCountry.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.cmbCountry.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCountry.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.cmbCountry.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCountry.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCountry.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCountry.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.cmbCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCountry.DisplayLayout.Override.CellAppearance = appearance44;
            this.cmbCountry.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCountry.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlignAsString = "Left";
            this.cmbCountry.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.cmbCountry.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCountry.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.cmbCountry.DisplayLayout.Override.RowAppearance = appearance47;
            this.cmbCountry.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCountry.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.cmbCountry.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCountry.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCountry.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCountry.DropDownWidth = 0;
            this.cmbCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCountry.Location = new System.Drawing.Point(130, 128);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(160, 21);
            this.cmbCountry.TabIndex = 6;
            this.cmbCountry.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
            this.cmbCountry.GotFocus += new System.EventHandler(this.cmbCountry_GotFocus);
            this.cmbCountry.LostFocus += new System.EventHandler(this.cmbCountry_LostFocus);
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblZip.Location = new System.Drawing.Point(10, 169);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(21, 13);
            this.lblZip.TabIndex = 194;
            this.lblZip.Text = "Zip";
            this.lblZip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtZip
            // 
            this.txtZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtZip.Location = new System.Drawing.Point(130, 172);
            this.txtZip.MaxLength = 50;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(160, 21);
            this.txtZip.TabIndex = 8;
            this.txtZip.TextChanged += new System.EventHandler(this.txtZip_TextChanged);
            this.txtZip.GotFocus += new System.EventHandler(this.txtZip_GotFocus);
            this.txtZip.LostFocus += new System.EventHandler(this.txtZip_LostFocus);
            // 
            // lblStateTerritory
            // 
            this.lblStateTerritory.AutoSize = true;
            this.lblStateTerritory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStateTerritory.Location = new System.Drawing.Point(10, 147);
            this.lblStateTerritory.Name = "lblStateTerritory";
            this.lblStateTerritory.Size = new System.Drawing.Size(79, 13);
            this.lblStateTerritory.TabIndex = 193;
            this.lblStateTerritory.Text = "State/Territory";
            this.lblStateTerritory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCountry.Location = new System.Drawing.Point(10, 126);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(46, 13);
            this.lblCountry.TabIndex = 191;
            this.lblCountry.Text = "Country";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(74, 64);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(8, 8);
            this.label18.TabIndex = 178;
            this.label18.Text = "*";
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(42, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(8, 8);
            this.label14.TabIndex = 174;
            this.label14.Text = "*";
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(46, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(8, 8);
            this.label11.TabIndex = 171;
            this.label11.Text = "*";
            // 
            // cmbThirdPartyType
            // 
            this.cmbThirdPartyType.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cmbThirdPartyType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand5.ColHeadersVisible = false;
            ultraGridColumn47.Header.Caption = "";
            ultraGridColumn47.Header.Enabled = false;
            ultraGridColumn47.Header.VisiblePosition = 0;
            ultraGridColumn47.Width = 200;
            ultraGridColumn48.Header.VisiblePosition = 1;
            ultraGridColumn48.Hidden = true;
            ultraGridBand5.Columns.AddRange(new object[] {
            ultraGridColumn47,
            ultraGridColumn48});
            this.cmbThirdPartyType.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbThirdPartyType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.cmbThirdPartyType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbThirdPartyType.DropDownWidth = 0;
            this.cmbThirdPartyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbThirdPartyType.Location = new System.Drawing.Point(130, 39);
            this.cmbThirdPartyType.Name = "cmbThirdPartyType";
            this.cmbThirdPartyType.Size = new System.Drawing.Size(160, 21);
            this.cmbThirdPartyType.TabIndex = 2;
            this.cmbThirdPartyType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbThirdPartyType.ValueChanged += new System.EventHandler(this.cmbThirdPartyType_ValueChanged);
            this.cmbThirdPartyType.GotFocus += new System.EventHandler(this.cmbThirdPartyType_GotFocus);
            this.cmbThirdPartyType.LostFocus += new System.EventHandler(this.cmbThirdPartyType_LostFocus);
            // 
            // txtFax
            // 
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFax.Location = new System.Drawing.Point(130, 227);
            this.txtFax.MaxLength = 50;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(160, 21);
            this.txtFax.TabIndex = 10;
            this.txtFax.TextChanged += new System.EventHandler(this.txtFax_TextChanged);
            this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
            this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
            // 
            // lblBrokerCode
            // 
            this.lblBrokerCode.AutoSize = true;
            this.lblBrokerCode.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBrokerCode.Location = new System.Drawing.Point(10, 275);
            this.lblBrokerCode.Name = "lblBrokerCode";
            this.lblBrokerCode.Size = new System.Drawing.Size(66, 13);
            this.lblBrokerCode.TabIndex = 191;
            this.lblBrokerCode.Text = "Broker Code";
            this.lblBrokerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBrokerCode
            // 
            this.txtBrokerCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBrokerCode.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtBrokerCode.Location = new System.Drawing.Point(130, 275);
            this.txtBrokerCode.MaxLength = 50;
            this.txtBrokerCode.Name = "txtBrokerCode";
            this.txtBrokerCode.Size = new System.Drawing.Size(160, 21);
            this.txtBrokerCode.TabIndex = 12;
            // 
            // txtAddress2
            // 
            this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress2.Location = new System.Drawing.Point(130, 106);
            this.txtAddress2.MaxLength = 50;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(160, 21);
            this.txtAddress2.TabIndex = 5;
            this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
            this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
            // 
            // txtAddress1
            // 
            this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress1.Location = new System.Drawing.Point(130, 84);
            this.txtAddress1.MaxLength = 50;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(160, 21);
            this.txtAddress1.TabIndex = 4;
            this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
            this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(130, 62);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(160, 21);
            this.txtShortName.TabIndex = 3;
            this.txtShortName.TextChanged += new System.EventHandler(this.txtShortName_TextChanged);
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // txtThirdPartyName
            // 
            this.txtThirdPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtThirdPartyName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtThirdPartyName.Location = new System.Drawing.Point(130, 16);
            this.txtThirdPartyName.MaxLength = 50;
            this.txtThirdPartyName.Name = "txtThirdPartyName";
            this.txtThirdPartyName.Size = new System.Drawing.Size(160, 21);
            this.txtThirdPartyName.TabIndex = 1;
            this.txtThirdPartyName.TextChanged += new System.EventHandler(this.txtThirdPartyName_TextChanged);
            this.txtThirdPartyName.GotFocus += new System.EventHandler(this.txtThirdPartyName_GotFocus);
            this.txtThirdPartyName.LostFocus += new System.EventHandler(this.txtThirdPartyName_LostFocus);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(10, 227);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 158;
            this.label9.Text = "Fax #";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(10, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 154;
            this.label5.Text = "Address2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(10, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 153;
            this.label4.Text = "Address1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(10, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 152;
            this.label3.Text = "Short Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(10, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 151;
            this.label2.Text = "Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 150;
            this.label1.Text = "Name";
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(8, 318);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 11);
            this.label17.TabIndex = 178;
            this.label17.Text = "* Required Field";
            // 
            // grpPrimaryContact
            // 
            this.grpPrimaryContact.Controls.Add(this.txtPCFax);
            this.grpPrimaryContact.Controls.Add(this.lblPCFax);
            this.grpPrimaryContact.Controls.Add(this.lblTitle);
            this.grpPrimaryContact.Controls.Add(this.cmbTitle);
            this.grpPrimaryContact.Controls.Add(this.lblLastName);
            this.grpPrimaryContact.Controls.Add(this.txtLastName);
            this.grpPrimaryContact.Controls.Add(this.label6);
            this.grpPrimaryContact.Controls.Add(this.txtContactPerson);
            this.grpPrimaryContact.Controls.Add(this.label19);
            this.grpPrimaryContact.Controls.Add(this.txtEmail);
            this.grpPrimaryContact.Controls.Add(this.label10);
            this.grpPrimaryContact.Controls.Add(this.txtCellPhone);
            this.grpPrimaryContact.Controls.Add(this.lblPCCellPhone);
            this.grpPrimaryContact.Controls.Add(this.txtPCWorkTele);
            this.grpPrimaryContact.Controls.Add(this.label8);
            this.grpPrimaryContact.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpPrimaryContact.Location = new System.Drawing.Point(400, 15);
            this.grpPrimaryContact.Name = "grpPrimaryContact";
            this.grpPrimaryContact.Size = new System.Drawing.Size(327, 204);
            this.grpPrimaryContact.TabIndex = 179;
            this.grpPrimaryContact.TabStop = false;
            this.grpPrimaryContact.Text = "Primary Contact";
            this.grpPrimaryContact.Enter += new System.EventHandler(this.grpPrimaryContact_Enter);
            // 
            // txtPCFax
            // 
            this.txtPCFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPCFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPCFax.Location = new System.Drawing.Point(130, 158);
            this.txtPCFax.MaxLength = 50;
            this.txtPCFax.Name = "txtPCFax";
            this.txtPCFax.Size = new System.Drawing.Size(160, 21);
            this.txtPCFax.TabIndex = 21;
            this.txtPCFax.TextChanged += new System.EventHandler(this.txtPCFax_TextChanged);
            this.txtPCFax.GotFocus += new System.EventHandler(this.txtPCFax_GotFocus);
            this.txtPCFax.LostFocus += new System.EventHandler(this.txtPCFax_LostFocus);
            // 
            // lblPCFax
            // 
            this.lblPCFax.AutoSize = true;
            this.lblPCFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCFax.Location = new System.Drawing.Point(10, 160);
            this.lblPCFax.Name = "lblPCFax";
            this.lblPCFax.Size = new System.Drawing.Size(36, 13);
            this.lblPCFax.TabIndex = 181;
            this.lblPCFax.Text = "Fax #";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTitle.Location = new System.Drawing.Point(10, 61);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 178;
            this.lblTitle.Text = "Title";
            // 
            // cmbTitle
            // 
            ultraGridColumn49.Header.VisiblePosition = 0;
            ultraGridBand6.Columns.AddRange(new object[] {
            ultraGridColumn49});
            this.cmbTitle.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbTitle.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbTitle.Location = new System.Drawing.Point(126, 59);
            this.cmbTitle.MaxLength = 50;
            this.cmbTitle.Name = "cmbTitle";
            this.cmbTitle.Size = new System.Drawing.Size(160, 23);
            this.cmbTitle.TabIndex = 16;
            this.cmbTitle.GotFocus += new System.EventHandler(this.cmbTitle_GotFocus);
            this.cmbTitle.LostFocus += new System.EventHandler(this.cmbTitle_LostFocus);
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLastName.Location = new System.Drawing.Point(10, 39);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(57, 13);
            this.lblLastName.TabIndex = 175;
            this.lblLastName.Text = "Last Name";
            // 
            // txtLastName
            // 
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLastName.Location = new System.Drawing.Point(125, 37);
            this.txtLastName.MaxLength = 50;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(160, 21);
            this.txtLastName.TabIndex = 15;
            this.txtLastName.GotFocus += new System.EventHandler(this.txtLastName_GotFocus);
            this.txtLastName.LostFocus += new System.EventHandler(this.txtLastName_LostFocus);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(10, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 155;
            this.label6.Text = "First Name";
            // 
            // txtContactPerson
            // 
            this.txtContactPerson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContactPerson.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtContactPerson.Location = new System.Drawing.Point(124, 15);
            this.txtContactPerson.MaxLength = 50;
            this.txtContactPerson.Name = "txtContactPerson";
            this.txtContactPerson.Size = new System.Drawing.Size(160, 21);
            this.txtContactPerson.TabIndex = 14;
            this.txtContactPerson.GotFocus += new System.EventHandler(this.txtContactPerson_GotFocus);
            this.txtContactPerson.LostFocus += new System.EventHandler(this.txtContactPerson_LostFocus);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label19.Location = new System.Drawing.Point(10, 117);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 13);
            this.label19.TabIndex = 179;
            this.label19.Text = "(1-111-111111)";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEmail.Location = new System.Drawing.Point(127, 81);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(160, 21);
            this.txtEmail.TabIndex = 17;
            this.txtEmail.GotFocus += new System.EventHandler(this.txtEmail_GotFocus);
            this.txtEmail.LostFocus += new System.EventHandler(this.txtEmail_LostFocus);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.Location = new System.Drawing.Point(10, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 159;
            this.label10.Text = "Email";
            // 
            // txtCellPhone
            // 
            this.txtCellPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCellPhone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCellPhone.Location = new System.Drawing.Point(130, 136);
            this.txtCellPhone.MaxLength = 50;
            this.txtCellPhone.Name = "txtCellPhone";
            this.txtCellPhone.Size = new System.Drawing.Size(160, 21);
            this.txtCellPhone.TabIndex = 20;
            this.txtCellPhone.TextChanged += new System.EventHandler(this.txtCellPhone_TextChanged);
            this.txtCellPhone.GotFocus += new System.EventHandler(this.txtCellPhone_GotFocus);
            this.txtCellPhone.LostFocus += new System.EventHandler(this.txtCellPhone_LostFocus);
            // 
            // lblPCCellPhone
            // 
            this.lblPCCellPhone.AutoSize = true;
            this.lblPCCellPhone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCCellPhone.Location = new System.Drawing.Point(10, 138);
            this.lblPCCellPhone.Name = "lblPCCellPhone";
            this.lblPCCellPhone.Size = new System.Drawing.Size(68, 13);
            this.lblPCCellPhone.TabIndex = 156;
            this.lblPCCellPhone.Text = "Cell Phone #";
            // 
            // txtPCWorkTele
            // 
            this.txtPCWorkTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPCWorkTele.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPCWorkTele.Location = new System.Drawing.Point(128, 103);
            this.txtPCWorkTele.MaxLength = 50;
            this.txtPCWorkTele.Name = "txtPCWorkTele";
            this.txtPCWorkTele.Size = new System.Drawing.Size(160, 21);
            this.txtPCWorkTele.TabIndex = 19;
            this.txtPCWorkTele.TextChanged += new System.EventHandler(this.txtPCWorkTele_TextChanged);
            this.txtPCWorkTele.GotFocus += new System.EventHandler(this.txtWorkTele_GotFocus);
            this.txtPCWorkTele.LostFocus += new System.EventHandler(this.txtWorkTele_LostFocus);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(10, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 157;
            this.label8.Text = "Work #";
            // 
            // thirdPartyAudit
            // 
            this.thirdPartyAudit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.thirdPartyAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thirdPartyAudit.Location = new System.Drawing.Point(0, 0);
            this.thirdPartyAudit.MinimumSize = new System.Drawing.Size(675, 1);
            this.thirdPartyAudit.Name = "thirdPartyAudit";
            this.thirdPartyAudit.Size = new System.Drawing.Size(847, 446);
            this.thirdPartyAudit.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.groupBox2);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(847, 446);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDeleteFileFormat);
            this.groupBox2.Controls.Add(this.grdFileFormat);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(847, 446);
            this.groupBox2.TabIndex = 180;
            this.groupBox2.TabStop = false;
            // 
            // btnDeleteFileFormat
            // 
            this.btnDeleteFileFormat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDeleteFileFormat.Location = new System.Drawing.Point(379, 420);
            this.btnDeleteFileFormat.Name = "btnDeleteFileFormat";
            this.btnDeleteFileFormat.Size = new System.Drawing.Size(100, 24);
            this.btnDeleteFileFormat.TabIndex = 25;
            this.btnDeleteFileFormat.Text = "Delete Format";
            this.btnDeleteFileFormat.UseVisualStyleBackColor = false;
            this.btnDeleteFileFormat.Click += new System.EventHandler(this.btnDeleteFileFormat_Click);
            // 
            // grdFileFormat
            // 
            ultraGridColumn50.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn50.DefaultCellValue = "";
            appearance49.FontData.BoldAsString = "True";
            ultraGridColumn50.Header.Appearance = appearance49;
            ultraGridColumn50.Header.Caption = "Format Name";
            ultraGridColumn50.Header.ToolTipText = "Enter File Format Name";
            ultraGridColumn50.Header.VisiblePosition = 0;
            ultraGridColumn50.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            ultraGridColumn50.MaxLength = 50;
            ultraGridColumn50.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn50.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            ultraGridColumn50.Width = 120;
            ultraGridColumn51.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn51.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn51.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn51.DefaultCellValue = "";
            appearance50.FontData.BoldAsString = "True";
            ultraGridColumn51.Header.Appearance = appearance50;
            ultraGridColumn51.Header.Caption = "Prana To ThirdParty";
            ultraGridColumn51.Header.ToolTipText = "Select the File Name that Maps to Third Party";
            ultraGridColumn51.Header.VisiblePosition = 1;
            ultraGridColumn51.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RetainValueAndFocus;
            ultraGridColumn51.Width = 140;
            ultraGridColumn52.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance51.FontData.BoldAsString = "True";
            ultraGridColumn52.Header.Appearance = appearance51;
            ultraGridColumn52.Header.Caption = "Select File";
            ultraGridColumn52.Header.ToolTipText = "Select the File Name that Maps to Third Party";
            ultraGridColumn52.Header.VisiblePosition = 2;
            ultraGridColumn52.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn52.Width = 70;
            ultraGridColumn53.Header.VisiblePosition = 7;
            ultraGridColumn53.Hidden = true;
            ultraGridColumn54.Header.VisiblePosition = 8;
            ultraGridColumn54.Hidden = true;
            ultraGridColumn55.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn55.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn55.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance52.FontData.BoldAsString = "True";
            ultraGridColumn55.Header.Appearance = appearance52;
            ultraGridColumn55.Header.Caption = "Header File";
            ultraGridColumn55.Header.ToolTipText = "Header XSLT";
            ultraGridColumn55.Header.VisiblePosition = 3;
            ultraGridColumn55.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RetainValueAndFocus;
            ultraGridColumn55.Width = 140;
            ultraGridColumn56.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance53.FontData.BoldAsString = "True";
            ultraGridColumn56.Header.Appearance = appearance53;
            ultraGridColumn56.Header.Caption = "Select Header File";
            ultraGridColumn56.Header.ToolTipText = "Select File";
            ultraGridColumn56.Header.VisiblePosition = 4;
            ultraGridColumn56.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn56.Width = 131;
            ultraGridColumn57.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn57.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn57.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance54.FontData.BoldAsString = "True";
            ultraGridColumn57.Header.Appearance = appearance54;
            ultraGridColumn57.Header.Caption = "Footer File";
            ultraGridColumn57.Header.ToolTipText = "Footer File";
            ultraGridColumn57.Header.VisiblePosition = 5;
            ultraGridColumn57.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RetainValueAndFocus;
            ultraGridColumn57.Width = 140;
            ultraGridColumn58.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance55.FontData.BoldAsString = "True";
            ultraGridColumn58.Header.Appearance = appearance55;
            ultraGridColumn58.Header.Caption = "Select File";
            ultraGridColumn58.Header.ToolTipText = "Footer File";
            ultraGridColumn58.Header.VisiblePosition = 6;
            ultraGridColumn58.Width = 40;
            ultraGridColumn59.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance62.FontData.BoldAsString = "True";
            appearance62.TextHAlignAsString = "Center";
            ultraGridColumn59.CellAppearance = appearance62;
            ultraGridColumn59.Header.Appearance = appearance62;
            ultraGridColumn59.Header.VisiblePosition = 9;
            ultraGridColumn59.NullText = "Set/Edit";
            ultraGridColumn59.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn59.Width = 100;
            ultraGridBand7.Columns.AddRange(new object[] {
            ultraGridColumn50,
            ultraGridColumn51,
            ultraGridColumn52,
            ultraGridColumn53,
            ultraGridColumn54,
            ultraGridColumn55,
            ultraGridColumn56,
            ultraGridColumn57,
            ultraGridColumn58,
            ultraGridColumn59});
            ultraGridBand7.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand7.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.grdFileFormat.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.grdFileFormat.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdFileFormat.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance56.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance56.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance56.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance56.BorderColor = System.Drawing.SystemColors.Window;
            this.grdFileFormat.DisplayLayout.GroupByBox.Appearance = appearance56;
            appearance57.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdFileFormat.DisplayLayout.GroupByBox.BandLabelAppearance = appearance57;
            this.grdFileFormat.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdFileFormat.DisplayLayout.GroupByBox.Hidden = true;
            appearance58.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance58.BackColor2 = System.Drawing.SystemColors.Control;
            appearance58.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance58.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdFileFormat.DisplayLayout.GroupByBox.PromptAppearance = appearance58;
            this.grdFileFormat.DisplayLayout.MaxColScrollRegions = 1;
            this.grdFileFormat.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdFileFormat.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdFileFormat.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdFileFormat.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdFileFormat.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            appearance59.BackColor = System.Drawing.SystemColors.Control;
            appearance59.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance59.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance59.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance59.BorderColor = System.Drawing.SystemColors.Window;
            this.grdFileFormat.DisplayLayout.Override.GroupByRowAppearance = appearance59;
            appearance60.TextHAlignAsString = "Center";
            this.grdFileFormat.DisplayLayout.Override.HeaderAppearance = appearance60;
            this.grdFileFormat.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdFileFormat.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance61.BorderColor = System.Drawing.Color.Black;
            this.grdFileFormat.DisplayLayout.Override.RowAppearance = appearance61;
            this.grdFileFormat.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdFileFormat.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdFileFormat.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdFileFormat.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdFileFormat.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdFileFormat.Location = new System.Drawing.Point(6, 14);
            this.grdFileFormat.Name = "grdFileFormat";
            this.grdFileFormat.Size = new System.Drawing.Size(835, 400);
            this.grdFileFormat.TabIndex = 0;
            this.grdFileFormat.Text = "ultraGrid1";
            this.grdFileFormat.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdFileFormat.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdFileFormat_AfterCellUpdate);
            this.grdFileFormat.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdFileFormat_CellChange);
            this.grdFileFormat.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdFileFormat_ClickCellButton);
            this.grdFileFormat.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdFileFormat_Error);
            this.grdFileFormat.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdFileFormat_InitializeRow);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.splitContainer1);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(847, 446);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnAdvance);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ctrlFileSetting);
            this.splitContainer1.Size = new System.Drawing.Size(847, 446);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnAdvance
            // 
            this.btnAdvance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdvance.Location = new System.Drawing.Point(725, 1);
            this.btnAdvance.Name = "btnAdvance";
            this.btnAdvance.Size = new System.Drawing.Size(101, 25);
            this.btnAdvance.TabIndex = 0;
            this.btnAdvance.Text = "Advance Settings";
            this.btnAdvance.Click += new System.EventHandler(this.btnAdvance_Click);
            // 
            // ctrlFileSetting
            // 
            this.ctrlFileSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlFileSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlFileSetting.Location = new System.Drawing.Point(0, 0);
            this.ctrlFileSetting.Name = "ctrlFileSetting";
            this.ctrlFileSetting.Size = new System.Drawing.Size(847, 417);
            this.ctrlFileSetting.TabIndex = 1;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.cntrlBatchSetup1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(847, 446);
            this.ultraTabPageControl3.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl3_Paint);
            // 
            // cntrlBatchSetup1
            // 
            this.cntrlBatchSetup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntrlBatchSetup1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cntrlBatchSetup1.Location = new System.Drawing.Point(0, 0);
            this.cntrlBatchSetup1.Name = "cntrlBatchSetup1";
            this.cntrlBatchSetup1.Size = new System.Drawing.Size(847, 446);
            this.cntrlBatchSetup1.TabIndex = 0;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl4);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(849, 467);
            this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.ultraTabControl1.TabIndex = 180;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Detail Section";
            ultraTab2.Key = "tbpFixFileSection";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "FIX/File Section";
            ultraTab4.Key = "importFileSetup";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Import File Setup";
            ultraTab3.Key = "batchSetup";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Import Batch Setup";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab4,
            ultraTab3});
            this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(847, 446);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ThirdPartyForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraTabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "ThirdPartyForm";
            this.Size = new System.Drawing.Size(849, 467);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecurityIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdPartyType)).EndInit();
            this.grpPrimaryContact.ResumeLayout(false);
            this.grpPrimaryContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTitle)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFileFormat)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        #endregion

        public Prana.BusinessObjects.ThirdParty ThirdPartyProperty
        {
            get
            {
                Prana.BusinessObjects.ThirdParty thirdParty = new Prana.BusinessObjects.ThirdParty();
                GetThirdPartyDetails(thirdParty);
                return thirdParty;
            }
            set
            {
                SetThirdPartyDetails(value);
            }
        }

        public void SetupControl()
        {
            BindThirdPartyType();
            BindCountries();
            BindStates();
            BindFileFormatGrid(0);
            BindSecurityIdentifier();
            BindCounterParty();
        }

        //modified by: Bharat Raturi, march 2014
        //purpose: create method to initialize the batch setup control
        /// <summary>
        /// Method to initialize the batch setup control
        /// </summary>
        public void InitializeThirdPartyControls(int thirdPartyID, string thirdPartyName, Boolean IsAdminPermission)
        {
            try
            {
                cntrlBatchSetup1.InitializeControl(thirdPartyID);
                ctrlFileSetting.InitializeControl(thirdPartyID, thirdPartyName);
                thirdPartyAudit.InitializeControl(thirdPartyID);

                //TODO - set to false after testing- omshiv
                ultraTabControl1.Tabs["batchSetup"].Visible = true;
                ultraTabControl1.Tabs["importFileSetup"].Visible = true;
                ctrlFileSetting.SetGridAccess(true);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindSecurityIdentifier()
        {
            EnumerationValueList lstSecurityIdentifier = new EnumerationValueList();
            cmbSecurityIdentifier.DisplayMember = "DisplayText";
            cmbSecurityIdentifier.ValueMember = "Value";
            lstSecurityIdentifier = EnumHelper.ConvertEnumForBindingWithSelect(typeof(Prana.Global.ApplicationConstants.SymbologyCodes));
            cmbSecurityIdentifier.DataSource = null;
            cmbSecurityIdentifier.DataSource = lstSecurityIdentifier;
            Utils.UltraComboFilter(cmbSecurityIdentifier, "DisplayText");
            cmbSecurityIdentifier.Value = int.MinValue;
        }

        /// <summary>
        /// This method binds the existing <see cref="Countries"/> in the ComboBox control by assigning the 
        /// countries object to its datasource property.
        /// </summary>
        private void BindCountries()
        {
            Countries countries = GeneralManager.GetCountries();
            countries.Insert(0, new Country(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            cmbCountry.DisplayMember = "Name";
            cmbCountry.ValueMember = "CountryID";
            cmbCountry.DataSource = null;
            cmbCountry.DataSource = countries;
            cmbCountry.Value = int.MinValue;
            ColumnsCollection columns = cmbCountry.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "Name")
                {
                    column.Hidden = true;
                }
                else
                {
                    // The column headers are set as invisible.
                    cmbCountry.DisplayLayout.Bands[0].ColHeadersVisible = false;
                }
            }
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
            states.Insert(0, new State(int.MinValue, ApplicationConstants.C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            ColumnsCollection columns = cmbState.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                // If the column is not "ThirdPartyName" Column , then it is set as hidden. 
                if (column.Key != "StateName")
                {
                    column.Hidden = true;
                }
                else
                {
                    // The column headers are set as invisible.
                    cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
                }
            }
        }

        /// <summary>
        /// This method empties the ComboBox from any state by assigning the states object to null value.
        /// </summary>
        private void BindEmptyStates()
        {
            //GetStates method fetches the existing states from the database.
            States states = new States();
            //Inserting the - Select - option in the Combo Box at the top.
            states.Insert(0, new State(int.MinValue, ApplicationConstants.C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            ColumnsCollection columns = cmbState.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                // If the column is not "ThirdPartyName" Column , then it is set as hidden. 
                if (column.Key != "StateName")
                {
                    column.Hidden = true;
                }
                else
                {
                    // The column headers are set as invisible.
                    cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
                }
            }
        }

        public void GetThirdPartyDetails(Prana.BusinessObjects.ThirdParty thirdParty)
        {
            thirdParty.ThirdPartyName = txtThirdPartyName.Text.ToString();
            thirdParty.ThirdPartyTypeID = int.Parse(cmbThirdPartyType.Value.ToString());
            thirdParty.ShortName = txtShortName.Text.ToString();
            thirdParty.Address1 = txtAddress1.Text.ToString();
            thirdParty.Address2 = txtAddress2.Text.ToString();
            thirdParty.ContactPerson = txtContactPerson.Text.ToString();
            thirdParty.CellPhone = txtCellPhone.Text.ToString();
            thirdParty.WorkTelephone = txtPCWorkTele.ToString();
            thirdParty.Fax = txtFax.Text.ToString();
            thirdParty.Email = txtEmail.Text.ToString();
            thirdParty.CounterPartyID = int.Parse(cmbCounterParty.Value.ToString());
        }

        public bool GetThirdPartyDetailsForSave(Prana.BusinessObjects.ThirdParty thirdParty)
        {
            bool result = false;
            //Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
            string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex emailRegex = new Regex(emailCheck);
            Match emailMatch = emailRegex.Match(txtEmail.Text.ToString());

            errorProvider1.SetError(txtThirdPartyName, "");
            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(cmbThirdPartyType, "");
            errorProvider1.SetError(txtEmail, "");
            errorProvider1.SetError(cmbCounterParty, "");

            if (txtThirdPartyName.Text.Trim().Equals(string.Empty))
            {
                errorProvider1.SetError(txtThirdPartyName, "Please enter display name!");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtThirdPartyName.Focus();
                return result;
            }
            else if (int.Parse(cmbThirdPartyType.Value.ToString()).Equals(int.MinValue))
            {
                errorProvider1.SetError(cmbThirdPartyType, "Please select third party type!");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                cmbThirdPartyType.Focus();
                return result;
            }
            else if (txtShortName.Text.Trim().Equals(string.Empty))
            {
                errorProvider1.SetError(txtShortName, "Please enter short name!");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtShortName.Focus();
                return result;
            }
            else if (!Regex.IsMatch(txtShortName.Text.Trim(), "^[a-zA-Z0-9_]*$"))
            {
                errorProvider1.SetError(txtShortName, "Short name cannot have white spaces!");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtShortName.Focus();
                return result;
            }
            else if (!(String.IsNullOrEmpty(txtZip.Text)) && txtZip.Text.Any(Char.IsLetter))
            {
                errorProvider1.SetError(txtZip, "Please enter numeric values only.");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtZip.Focus();
                return result;
            }
            else if (!(String.IsNullOrEmpty(txtFax.Text)) && txtFax.Text.Any(Char.IsLetter))
            {
                errorProvider1.SetError(txtFax, "Please enter numeric values only.");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtFax.Focus();
                return result;
            }
            else if (!(String.IsNullOrEmpty(txtPCFax.Text)) && txtPCFax.Text.Any(Char.IsLetter))
            {
                errorProvider1.SetError(txtPCFax, "Please enter numeric values only.");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtPCFax.Focus();
                return result;
            }
            else if (!(String.IsNullOrEmpty(txtPCWorkTele.Text)) && txtPCWorkTele.Text.Any(Char.IsLetter))
            {
                errorProvider1.SetError(txtPCWorkTele, "Please enter numeric values only.");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtPCWorkTele.Focus();
                return result;
            }
            else if (!(String.IsNullOrEmpty(txtWorkTele.Text)) && txtWorkTele.Text.Any(Char.IsLetter))
            {
                errorProvider1.SetError(txtWorkTele, "Please enter numeric values only.");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtWorkTele.Focus();
                return result;
            }
            else if (!(String.IsNullOrEmpty(txtCellPhone.Text)) && txtCellPhone.Text.Any(Char.IsLetter))
            {
                errorProvider1.SetError(txtCellPhone, "Please enter numeric values only.");
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                txtCellPhone.Focus();
                return result;
            }

            if (txtEmail.Text.Trim().Length > 0 && !emailMatch.Success)
            {
                errorProvider1.SetError(txtEmail, "Please enter valid Email address!");
                txtEmail.Focus();
                return result;
            }

            if (cmbCounterParty.Value != null && int.Parse(cmbCounterParty.Value.ToString()).Equals(int.MinValue) && int.Parse(cmbThirdPartyType.Value.ToString()) == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker)
            {
                errorProvider1.SetError(cmbCounterParty, "Please select a counter party mapping!");
                cmbCounterParty.Focus();
                return result;
            }
            else
            {

                thirdParty.ThirdPartyName = txtThirdPartyName.Text.ToString();
                thirdParty.ThirdPartyTypeID = int.Parse(cmbThirdPartyType.Value.ToString());
                thirdParty.ShortName = txtShortName.Text.ToString();
                thirdParty.Address1 = txtAddress1.Text.ToString();
                thirdParty.Address2 = txtAddress2.Text.ToString();
                thirdParty.ContactPerson = txtContactPerson.Text.ToString();
                thirdParty.CellPhone = txtCellPhone.Text.ToString();
                thirdParty.WorkTelephone = txtWorkTele.Text.ToString();
                thirdParty.Fax = txtFax.Text.ToString();
                thirdParty.Email = txtEmail.Text.ToString();
                thirdParty.CountryID = int.Parse(cmbCountry.Value.ToString());
                if (cmbState.Value != null)
                {
                    thirdParty.StateID = int.Parse(cmbState.Value.ToString());
                }
                thirdParty.Zip = txtZip.Text.ToString();
                thirdParty.PrimaryContactLastName = txtLastName.Text.ToString();
                thirdParty.PrimaryContactTitle = cmbTitle.Text.ToString();
                thirdParty.PrimaryContactWorkTelephone = txtPCWorkTele.Text.ToString();
                thirdParty.PrimaryContactFax = txtPCFax.Text.ToString();
                thirdParty.SecurityIdentifierType = (ApplicationConstants.SymbologyCodes)cmbSecurityIdentifier.Value;
                thirdParty.BrokerCode = txtBrokerCode.Text.Trim();
                if (cmbCounterParty.Value != null)
                {
                    thirdParty.CounterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                }

                return result = true;
            }
        }

        public void SetThirdPartyDetails(Prana.BusinessObjects.ThirdParty thirdParty)
        {
            txtThirdPartyName.Text = thirdParty.ThirdPartyName;
            cmbThirdPartyType.Value = thirdParty.ThirdPartyTypeID;
            txtShortName.Text = thirdParty.ShortName;
            txtAddress1.Text = thirdParty.Address1;
            txtAddress2.Text = thirdParty.Address2;
            txtContactPerson.Text = thirdParty.ContactPerson;
            txtCellPhone.Text = thirdParty.CellPhone;
            txtWorkTele.Text = thirdParty.WorkTelephone;
            txtFax.Text = thirdParty.Fax;
            txtEmail.Text = thirdParty.Email;
            cmbCountry.Value = int.Parse(thirdParty.CountryID.ToString());
            if (int.Parse(cmbCountry.Value.ToString()) <= 0)
            {
                BindEmptyStates();
            }
            txtPCWorkTele.Text = thirdParty.PrimaryContactWorkTelephone;
            cmbState.Value = int.Parse(thirdParty.StateID.ToString());
            txtZip.Text = thirdParty.Zip;

            txtPCFax.Text = thirdParty.PrimaryContactFax;
            cmbTitle.Text = thirdParty.PrimaryContactTitle;
            txtLastName.Text = thirdParty.PrimaryContactLastName;
            cmbSecurityIdentifier.Value = thirdParty.SecurityIdentifierType;
            txtBrokerCode.Text = thirdParty.BrokerCode;
            _isExecutingBroker = thirdParty.ThirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker;
            cmbCounterParty.Value = thirdParty.CounterPartyID;
        }

        public void RefreshThirdPartyDetails()
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCellPhone.Text = "";
            txtContactPerson.Text = "";
            txtEmail.Text = "";
            txtFax.Text = "";
            txtShortName.Text = "";
            txtThirdPartyName.Text = "";
            txtPCWorkTele.Text = "";
            cmbCountry.Value = int.MinValue;
            cmbState.Value = int.MinValue;
            cmbSecurityIdentifier.Value = int.MinValue;
            txtZip.Text = "";
            txtPCFax.Text = "";
            cmbTitle.Text = "-Select-";
            cmbTitle.Value = "-Select-";
            txtLastName.Text = "";
            txtBrokerCode.Text = "";
            BindFileFormatGrid(0);
        }

        private void BindThirdPartyType()
        {
            List<ThirdPartyType> thirdPartyTypes = ThirdPartyDataManager.GetThirdPartyTypes();
            thirdPartyTypes.Insert(0, new ThirdPartyType(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            cmbThirdPartyType.DataSource = null;
            cmbThirdPartyType.DataSource = thirdPartyTypes;
            cmbThirdPartyType.DisplayMember = "ThirdPartyTypeName";
            cmbThirdPartyType.ValueMember = "ThirdPartyTypeID";
            ColumnsCollection columns = cmbThirdPartyType.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "ThirdPartyTypeName")
                {
                    column.Hidden = true;
                }
                else
                {
                    cmbThirdPartyType.DisplayLayout.Bands[0].ColHeadersVisible = false;
                }
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
                        states.Insert(0, new State(int.MinValue, ApplicationConstants.C_COMBO_SELECT, int.MinValue));
                        cmbState.DisplayMember = "StateName";
                        cmbState.ValueMember = "StateID";
                        cmbState.DataSource = null;
                        cmbState.DataSource = states;
                        cmbState.Text = ApplicationConstants.C_COMBO_SELECT;
                        ColumnsCollection columns = cmbState.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            // If the column is not "ThirdPartyName" Column , then it is set as hidden. 
                            if (column.Key != "StateName")
                            {
                                column.Hidden = true;
                            }
                            else
                            {
                                // The column headers are set as invisible.
                                cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            }
                        }

                    }
                }
                else
                {
                    BindEmptyStates();
                }

            }
        }

        public void DisableThirdPartyControls()
        {
            txtAddress1.Enabled = false;
            txtAddress2.Enabled = false;
            txtCellPhone.Enabled = false;
            txtContactPerson.Enabled = false;
            txtEmail.Enabled = false;
            txtFax.Enabled = false;
            txtShortName.Enabled = false;
            txtThirdPartyName.Enabled = false;
            txtPCWorkTele.Enabled = false;
            cmbCountry.Enabled = false;
            cmbState.Enabled = false;
            txtZip.Enabled = false;

            txtPCFax.Enabled = false;
            cmbTitle.Enabled = false;
            txtLastName.Enabled = false;
            grdFileFormat.Enabled = false;
        }

        public void EnableThirdPartyControls()
        {
            txtAddress1.Enabled = true;
            txtAddress2.Enabled = true;
            txtCellPhone.Enabled = true;
            txtContactPerson.Enabled = true;
            txtEmail.Enabled = true;
            txtFax.Enabled = true;
            txtShortName.Enabled = true;
            txtThirdPartyName.Enabled = true;
            txtPCWorkTele.Enabled = true;
            cmbCountry.Enabled = true;
            cmbState.Enabled = true;
            txtZip.Enabled = true;

            txtPCFax.Enabled = true;
            cmbTitle.Enabled = true;
            txtLastName.Enabled = true;
            grdFileFormat.Enabled = true;
        }

        #region Focus Colors
        private void cmbThirdPartyType_GotFocus(object sender, System.EventArgs e)
        {
            cmbThirdPartyType.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbSecurityIdentifier_GotFocus(object sender, System.EventArgs e)
        {
            // cmbSecurityIdentifier.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbThirdPartyType_LostFocus(object sender, System.EventArgs e)
        {
            cmbThirdPartyType.Appearance.BackColor = Color.White;
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
        private void txtCellPhone_GotFocus(object sender, System.EventArgs e)
        {
            txtCellPhone.BackColor = Color.LemonChiffon;
        }
        private void txtCellPhone_LostFocus(object sender, System.EventArgs e)
        {
            txtCellPhone.BackColor = Color.White;
        }
        private void txtContactPerson_GotFocus(object sender, System.EventArgs e)
        {
            txtContactPerson.BackColor = Color.LemonChiffon;
        }
        private void txtContactPerson_LostFocus(object sender, System.EventArgs e)
        {
            txtContactPerson.BackColor = Color.White;
        }
        private void txtEmail_GotFocus(object sender, System.EventArgs e)
        {
            txtEmail.BackColor = Color.LemonChiffon;
        }
        private void txtEmail_LostFocus(object sender, System.EventArgs e)
        {
            txtEmail.BackColor = Color.White;
        }
        private void txtFax_GotFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.LemonChiffon;
        }
        private void txtFax_LostFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        private void txtThirdPartyName_GotFocus(object sender, System.EventArgs e)
        {
            txtThirdPartyName.BackColor = Color.LemonChiffon;
        }
        private void txtThirdPartyName_LostFocus(object sender, System.EventArgs e)
        {
            txtThirdPartyName.BackColor = Color.White;
        }
        private void txtWorkTele_GotFocus(object sender, System.EventArgs e)
        {
            txtPCWorkTele.BackColor = Color.LemonChiffon;
        }
        private void txtWorkTele_LostFocus(object sender, System.EventArgs e)
        {
            txtPCWorkTele.BackColor = Color.White;
        }
        private void txtZip_GotFocus(object sender, System.EventArgs e)
        {
            txtZip.BackColor = Color.LemonChiffon;
        }
        private void txtZip_LostFocus(object sender, System.EventArgs e)
        {
            txtZip.BackColor = Color.White;
        }
        private void cmbCountry_GotFocus(object sender, System.EventArgs e)
        {
            cmbCountry.BackColor = Color.LemonChiffon;
        }
        private void cmbCountry_LostFocus(object sender, System.EventArgs e)
        {
            cmbCountry.BackColor = Color.White;
        }
        private void cmbState_GotFocus(object sender, System.EventArgs e)
        {
            cmbState.BackColor = Color.LemonChiffon;
        }
        private void cmbState_LostFocus(object sender, System.EventArgs e)
        {
            cmbState.BackColor = Color.White;
        }
        private void cmbSecurityIdentifier_LostFocus(object sender, System.EventArgs e)
        {
            cmbSecurityIdentifier.BackColor = Color.White;
        }
        void txtPCFax_GotFocus(object sender, EventArgs e)
        {
            txtPCFax.BackColor = Color.LemonChiffon;
        }
        void txtPCFax_LostFocus(object sender, EventArgs e)
        {
            txtPCFax.BackColor = Color.White;
        }
        void cmbTitle_GotFocus(object sender, EventArgs e)
        {
            cmbTitle.BackColor = Color.LemonChiffon;
        }
        void cmbTitle_LostFocus(object sender, EventArgs e)
        {
            cmbTitle.BackColor = Color.White;
        }
        void txtLastName_GotFocus(object sender, EventArgs e)
        {
            txtLastName.BackColor = Color.LemonChiffon;
        }
        void txtLastName_LostFocus(object sender, EventArgs e)
        {
            txtLastName.BackColor = Color.White;
        }

        #endregion

        private void grpPrimaryContact_Enter(object sender, EventArgs e)
        {

        }

        #region Tab 2 of File Format

        public void BindFileFormatGrid(int thirdPartyId)
        {
            ThirdPartyFileFormats thirdPartyFileFormats = new ThirdPartyFileFormats();
            thirdPartyFileFormats = ThirdPartyDataManager.GetThirdPartyFileFormats(thirdPartyId);
            Dictionary<int, List<ThirdPartyTimeBatch>> thirdPartyTimedBatches = ThirdPartyDataManager.GetThirdPartyTimedBatches(thirdPartyId);
            foreach (ThirdPartyFileFormat fileFormat in thirdPartyFileFormats)
            {
                if (thirdPartyTimedBatches.ContainsKey(fileFormat.FileFormatId))
                {
                    fileFormat.TimeBatches = thirdPartyTimedBatches[fileFormat.FileFormatId];
                }
            }

            grdFileFormat.DataSource = thirdPartyFileFormats;
            if (thirdPartyId > 0)
            {
                AddNewRow();
            }
            else
            {
                //grdFileFormat.DataSource = null;
                AddNewTempRow();
            }
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            if (grdFileFormat.Rows.Count > 0)
            {
                UltraGridBand band = grdFileFormat.DisplayLayout.Bands[0];
                band.Columns["FileFormatName"].Header.VisiblePosition = 0;
                band.Columns["FileFormatName"].Header.Caption = "Format Name";
                band.Columns["FileFormatName"].MaxLength = 50;
                band.Columns["FileFormatName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["FileFormatName"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["FileFormatName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                band.Columns["FileFormatName"].Width = 140;
                band.Columns["FileFormatName"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["FileFormatName"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["FIXEnabled"].Header.VisiblePosition = 1;
                band.Columns["FIXEnabled"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["FIXEnabled"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["FIXEnabled"].Header.Caption = "FIX Enabled";

                band.Columns["FIXStorProc"].Header.VisiblePosition = 2;
                band.Columns["FIXStorProc"].Header.Caption = "Stored Proc Name (FIX)";
                band.Columns["FIXStorProc"].MaxLength = 50;
                band.Columns["FIXStorProc"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["FIXStorProc"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["FIXStorProc"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                band.Columns["FIXStorProc"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["FIXStorProc"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns[COLUMN_TIME_BATCHES_ENABLED].Header.VisiblePosition = 3;
                band.Columns[COLUMN_TIME_BATCHES_ENABLED].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns[COLUMN_TIME_BATCHES_ENABLED].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns[COLUMN_TIME_BATCHES_ENABLED].Header.Caption = "FIX Timed Batch";

                band.Columns[COLUMN_SET_EDIT_BATCH].Header.VisiblePosition = 4;

                band.Columns["FileEnabled"].Header.VisiblePosition = 5;
                band.Columns["FileEnabled"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["FileEnabled"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["FileEnabled"].Header.Caption = "File Enabled";

                HideShowFIXColumns();

                band.Columns["PranaToThirdParty"].Header.VisiblePosition = 6;
                band.Columns["PranaToThirdParty"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["PranaToThirdParty"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["PranaToThirdParty"].CellActivation = Activation.NoEdit;
                band.Columns["PranaToThirdParty"].Header.Caption = "Prana To Third Party";
                band.Columns["PranaToThirdParty"].Width = 140;
                band.Columns["PranaToThirdParty"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["PranaToThirdParty"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["SelectPranaTo"].Header.VisiblePosition = 7;
                band.Columns["SelectPranaTo"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                band.Columns["SelectPranaTo"].Width = 70;
                band.Columns["SelectPranaTo"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["SelectPranaTo"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["SelectPranaTo"].Header.Caption = "Select File";
                band.Columns["SelectPranaTo"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["SelectPranaTo"].NullText = "Select File";

                band.Columns["HeaderFile"].Header.VisiblePosition = 8;
                band.Columns["HeaderFile"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
                band.Columns["HeaderFile"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["HeaderFile"].Header.Caption = "Header File";
                band.Columns["HeaderFile"].Width = 140;
                band.Columns["HeaderFile"].CellActivation = Activation.NoEdit;
                band.Columns["HeaderFile"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["HeaderFile"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["SelectHeaderFile"].Header.VisiblePosition = 9;
                band.Columns["SelectHeaderFile"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                band.Columns["SelectHeaderFile"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["SelectHeaderFile"].Width = 70;
                band.Columns["SelectHeaderFile"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["SelectHeaderFile"].Header.Caption = "Select File";
                band.Columns["SelectHeaderFile"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["SelectHeaderFile"].NullText = "Select File";

                band.Columns["FooterFile"].Header.VisiblePosition = 10;
                band.Columns["FooterFile"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["FooterFile"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["FooterFile"].Header.Caption = "Footer File";
                band.Columns["FooterFile"].Width = 140;
                band.Columns["FooterFile"].CellActivation = Activation.NoEdit;
                band.Columns["FooterFile"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["FooterFile"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["SelectFooterFile"].Header.VisiblePosition = 11;
                band.Columns["SelectFooterFile"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                band.Columns["SelectFooterFile"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["SelectFooterFile"].Width = 70;
                band.Columns["SelectFooterFile"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["SelectFooterFile"].Header.Caption = "Select File";
                band.Columns["SelectFooterFile"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["SelectFooterFile"].NullText = "Select File";

                band.Columns["FileDisplayName"].Header.VisiblePosition = 12;
                band.Columns["FileDisplayName"].Header.Caption = "File Display Name";
                band.Columns["FileDisplayName"].MaxLength = 50;
                band.Columns["FileDisplayName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["FileDisplayName"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["FileDisplayName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                band.Columns["FileDisplayName"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["FileDisplayName"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["Delimiter"].Header.VisiblePosition = 13;
                band.Columns["Delimiter"].Header.Caption = "Delimiter";
                band.Columns["Delimiter"].MaxLength = 2;
                band.Columns["Delimiter"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["Delimiter"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["Delimiter"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                band.Columns["Delimiter"].Width = 80;
                band.Columns["Delimiter"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["Delimiter"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["DelimiterName"].Header.VisiblePosition = 14;
                band.Columns["DelimiterName"].Header.Caption = "Delimiter Name";
                band.Columns["DelimiterName"].MaxLength = 15;
                band.Columns["DelimiterName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["DelimiterName"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["DelimiterName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                band.Columns["DelimiterName"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["DelimiterName"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["FileExtension"].Header.VisiblePosition = 15;
                band.Columns["FileExtension"].Header.Caption = "File Extension";
                band.Columns["FileExtension"].MaxLength = 15;
                band.Columns["FileExtension"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["FileExtension"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["FileExtension"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                band.Columns["FileExtension"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["FileExtension"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["ExportOnly"].Header.VisiblePosition = 16;
                band.Columns["ExportOnly"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["ExportOnly"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["ExportOnly"].Header.Caption = "Export Only";

                band.Columns["StoredProcName"].Header.VisiblePosition = 17;
                band.Columns["StoredProcName"].Header.Caption = "Stored Proc Name (File)";
                band.Columns["StoredProcName"].MaxLength = 50;
                band.Columns["StoredProcName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["StoredProcName"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Default;
                band.Columns["StoredProcName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                band.Columns["StoredProcName"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                band.Columns["StoredProcName"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                band.Columns["DoNotShowFileOpenDialogue"].Header.VisiblePosition = 18;
                band.Columns["DoNotShowFileOpenDialogue"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["DoNotShowFileOpenDialogue"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["DoNotShowFileOpenDialogue"].Header.Caption = "Do Not Show File Open Dialogue";

                band.Columns["ClearExternalTransID"].Header.VisiblePosition = 19;
                band.Columns["ClearExternalTransID"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["ClearExternalTransID"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["ClearExternalTransID"].Header.Caption = "Clear External Transaction ID";

                band.Columns["IncludeExercisedAssignedTransaction"].Header.VisiblePosition = 20;
                band.Columns["IncludeExercisedAssignedTransaction"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["IncludeExercisedAssignedTransaction"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["IncludeExercisedAssignedTransaction"].Header.Caption = "Include Exer/Expr/Assign Transaction";

                band.Columns["IncludeExercisedAssignedUnderlyingTransaction"].Header.VisiblePosition = 21;
                band.Columns["IncludeExercisedAssignedUnderlyingTransaction"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["IncludeExercisedAssignedUnderlyingTransaction"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["IncludeExercisedAssignedUnderlyingTransaction"].Header.Caption = "Include Exer/Assign Underlying Transaction";

                band.Columns["IncludeCATransaction"].Header.VisiblePosition = 22;
                band.Columns["IncludeCATransaction"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["IncludeCATransaction"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["IncludeCATransaction"].Header.Caption = "Include CA Transaction";

                band.Columns["ThirdPartyId"].Header.VisiblePosition = 23;
                band.Columns["ThirdPartyId"].Hidden = true;

                band.Columns["FileFormatId"].Header.VisiblePosition = 24;
                band.Columns["FileFormatId"].Hidden = true;

                band.Columns["GenerateCancelNewForAmend"].Header.VisiblePosition = 25;
                band.Columns["GenerateCancelNewForAmend"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                band.Columns["GenerateCancelNewForAmend"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                band.Columns["GenerateCancelNewForAmend"].Header.Caption = "GenerateCancelNewForAmend";
            }
        }

        /// <summary>
        /// This method is to hide/show FIX columns on FIX/File Format section
        /// </summary>
        private void HideShowFIXColumns()
        {
            try
            {
                if (grdFileFormat.Rows.Count > 0)
                {
                    UltraGridBand band = grdFileFormat.DisplayLayout.Bands[0];
                    Activation cellActivation = _isExecutingBroker ? Activation.AllowEdit : Activation.Disabled;
                    bool isHidden = _isExecutingBroker ? false : true;
                    band.Columns["FIXEnabled"].CellActivation = cellActivation;
                    band.Columns["FIXEnabled"].Hidden = isHidden;
                    band.Columns["FIXStorProc"].Hidden = isHidden;
                    band.Columns["FileEnabled"].CellActivation = cellActivation;
                    band.Columns["FileEnabled"].Hidden = isHidden;
                    band.Columns[COLUMN_TIME_BATCHES_ENABLED].CellActivation = cellActivation;
                    band.Columns[COLUMN_TIME_BATCHES_ENABLED].Hidden = isHidden;
                    band.Columns[COLUMN_SET_EDIT_BATCH].Hidden = isHidden;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add new row to grid
        /// </summary>
        private void AddNewTempRow()
        {
            ThirdPartyFileFormats thirdPartyFileFormats = new ThirdPartyFileFormats();
            ThirdPartyFileFormat thirdPartyFileFormat = new ThirdPartyFileFormat();
            thirdPartyFileFormats.Add(thirdPartyFileFormat);
            grdFileFormat.DataSource = thirdPartyFileFormats;
        }

        /// <summary>
        /// add new row to enter the values after entering in the existing row
        /// </summary>
        private void AddNewRow()
        {
            UltraGridCell prevActiveCell = grdFileFormat.Rows[0].Cells[0];
            string cellText = string.Empty;
            int len = int.MinValue;
            if (grdFileFormat.ActiveCell != null)
            {
                prevActiveCell = grdFileFormat.ActiveCell;
                cellText = prevActiveCell.Text;
                len = cellText.Length;
            }

            int rowsCount = grdFileFormat.Rows.Count;
            UltraGridRow dr = grdFileFormat.Rows[rowsCount - 1];

            ThirdPartyFileFormats thirdPartyFileFormats = (ThirdPartyFileFormats)grdFileFormat.DataSource;
            ThirdPartyFileFormat thirdPartyFileFormat = new ThirdPartyFileFormat();
            //The below varriables are taken from the last row of the grid before adding the new row.
            string fileFormatName = dr.Cells["FileFormatName"].Value.ToString();
            string PranaToThirdParty = dr.Cells["PranaToThirdParty"].Value.ToString();
            string FileDisplayName = dr.Cells["FileDisplayName"].Value.ToString();

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            //if (!String.IsNullOrEmpty(fileFormatName) && !String.IsNullOrEmpty(PranaToThirdParty) && !String.IsNullOrEmpty(thirdPartyToPrana))
            if (!String.IsNullOrEmpty(fileFormatName) && !String.IsNullOrEmpty(PranaToThirdParty) && !String.IsNullOrEmpty(FileDisplayName))
            {
                thirdPartyFileFormats.Add(thirdPartyFileFormat);
                grdFileFormat.DataSource = thirdPartyFileFormats;
                grdFileFormat.DataBind();
                grdFileFormat.ActiveCell = prevActiveCell;
                grdFileFormat.Focus();
                grdFileFormat.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                if (len != int.MinValue)
                {
                    //prevActiveCell.SelLength = 0;
                    //prevActiveCell.SelStart = len + 1;
                }
            }
        }

        private string GetFileName(string title)
        {
            openFileDialog1.InitialDirectory = "DeskTop";
            openFileDialog1.Title = title;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt";
            string strFileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileName = openFileDialog1.FileName;
            }
            return strFileName;

        }

        /// <summary>
        /// Handles the click event on a cell button in the grdFileFormat grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileFormat_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals("SelectPranaTo"))
                {
                    string title = "Select File that Maps Prana To Third Party";
                    string shortName = GetFileName(title);
                    if (!String.IsNullOrEmpty(shortName))
                    {
                        grdFileFormat.ActiveRow.Cells["PranaToThirdParty"].Value = shortName;
                    }
                }

                if (e.Cell.Column.Key.Equals("SelectHeaderFile"))
                {
                    string title = "Select Header File ";
                    string shortName = GetFileName(title);
                    if (!String.IsNullOrEmpty(shortName))
                    {
                        grdFileFormat.ActiveRow.Cells["HeaderFile"].Value = shortName;
                    }
                }
                else if (e.Cell.Column.Key.Equals("SelectFooterFile"))
                {
                    string title = "Select Footer File ";
                    string shortName = GetFileName(title);
                    if (!String.IsNullOrEmpty(shortName))
                    {
                        grdFileFormat.ActiveRow.Cells["FooterFile"].Value = shortName;
                    }
                }
                else if (e.Cell.Column.Key.Equals(COLUMN_SET_EDIT_BATCH))
                {
                    TimedBatchSchedulingForm timedBatchSchedulingForm = new TimedBatchSchedulingForm();
                    ThirdPartyFileFormat fileFormat = (ThirdPartyFileFormat)e.Cell.Row.ListObject;
                    timedBatchSchedulingForm.BindGridData(fileFormat.TimeBatches);
                    timedBatchSchedulingForm.ShowDialog();
                    if (timedBatchSchedulingForm.UpdatedTimeBatches != null)
                    {
                        fileFormat.TimeBatches = timedBatchSchedulingForm.UpdatedTimeBatches;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the InitializeRow event for the grid grdFileFormat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileFormat_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                bool isFixEnabled = Convert.ToBoolean(e.Row.Cells["FIXEnabled"].Value);
                bool isTimeBatchesEnabled = Convert.ToBoolean(e.Row.Cells[COLUMN_TIME_BATCHES_ENABLED].Value);

                e.Row.Cells[COLUMN_TIME_BATCHES_ENABLED].Activation = isFixEnabled ? Activation.AllowEdit : Activation.Disabled;
                e.Row.Cells[COLUMN_SET_EDIT_BATCH].Activation = isTimeBatchesEnabled ? Activation.AllowEdit : Activation.Disabled;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the CellChange event for the grdFileFormat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileFormat_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                // grdFileFormat.UpdateData();
                if (e.Cell.Column.Key == "FIXEnabled")
                {
                    bool isFixEnabled = Convert.ToBoolean(e.Cell.Text);
                    if (!isFixEnabled)
                    {
                        e.Cell.Row.Cells[COLUMN_TIME_BATCHES_ENABLED].Value = false;
                    }
                    e.Cell.Row.Cells[COLUMN_TIME_BATCHES_ENABLED].Activation = isFixEnabled ? Activation.AllowEdit : Activation.Disabled;
                }
                if (e.Cell.Column.Key == COLUMN_TIME_BATCHES_ENABLED)
                {
                    bool isTimeBatchesEnabled = Convert.ToBoolean(e.Cell.Text);
                    e.Cell.Row.Cells[COLUMN_SET_EDIT_BATCH].Activation = isTimeBatchesEnabled ? Activation.AllowEdit : Activation.Disabled;
                }
                AddNewRow();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdFileFormat_AfterCellUpdate(object sender, CellEventArgs e)
        {
            grdFileFormat.UpdateData();
            AddNewRow();
        }

        ThirdPartyFileFormats _thirdPartyFileFormats = new ThirdPartyFileFormats();
        public ThirdPartyFileFormats thirdPartyFileFormatProperties
        {
            get
            {
                _thirdPartyFileFormats = (ThirdPartyFileFormats)grdFileFormat.DataSource;

                ThirdPartyFileFormats validFileFotmats = new ThirdPartyFileFormats();

                if (grdFileFormat.Rows.Count.Equals(1))
                {
                    string strFileFormatName = grdFileFormat.ActiveRow.Cells["FileFormatName"].Value.ToString();
                    string strPranaToThirdparty = grdFileFormat.ActiveRow.Cells["PranaToThirdParty"].Value.ToString();
                    //string strThirdPartyToPrana = grdFileFormat.ActiveRow.Cells["ThirdPartyToPrana"].Value.ToString();

                    //if (String.IsNullOrEmpty(strFileFormatName) && String.IsNullOrEmpty(strPranaToThirdparty) && String.IsNullOrEmpty(strThirdPartyToPrana))
                    if (String.IsNullOrEmpty(strFileFormatName) && String.IsNullOrEmpty(strPranaToThirdparty))
                    {
                        MessageBox.Show("Please enter the values for, atleast one Format ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        validFileFotmats = null;
                        ultraTabControl1.Tabs[1].Selected = true;
                        grdFileFormat.Focus();
                        return validFileFotmats;
                    }
                }
                int index1 = 1;
                // Validation to check whether any field in not blank              
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdFileFormat.Rows)
                {
                    string strFileFormatName = dr.Cells["FileFormatName"].Value.ToString().Trim();
                    string strPranaToThirdparty = dr.Cells["PranaToThirdParty"].Value.ToString().Trim();
                    string strFileDisplayName = dr.Cells["FileDisplayName"].Value.ToString();
                    string strDelimiter = dr.Cells["Delimiter"].Value.ToString();
                    string strDelimiterName = dr.Cells["DelimiterName"].Value.ToString().Trim();
                    string strFileExtension = dr.Cells["FileExtension"].Value.ToString();
                    string strFixStorProc = dr.Cells["FIXStorProc"].Value.ToString();
                    bool isFIXEnabled = Convert.ToBoolean(dr.Cells["FIXEnabled"].Value.ToString());
                    bool isFileEnabled = Convert.ToBoolean(dr.Cells["FileEnabled"].Value.ToString());
                    //if (!String.IsNullOrEmpty(strFileFormatName) || !String.IsNullOrEmpty(strPranaToThirdparty) || !String.IsNullOrEmpty(strThirdPartyToPrana))

                    if (!isFIXEnabled && !isFileEnabled)
                    {
                        MessageBox.Show("Please select the enabled transmission mechanism in the row " + index1, "Alert");
                        validFileFotmats = null;
                        ultraTabControl1.Tabs[1].Selected = true;
                        grdFileFormat.Focus();
                        return validFileFotmats;
                    }
                    if (isFileEnabled)
                    {
                        if (!String.IsNullOrEmpty(strFileFormatName) || !String.IsNullOrEmpty(strPranaToThirdparty) || !String.IsNullOrEmpty(strFileDisplayName) || !String.IsNullOrEmpty(strDelimiter) || !String.IsNullOrEmpty(strDelimiterName))// || !String.IsNullOrEmpty(strFileExtension))
                        {
                            if (String.IsNullOrEmpty(strFileFormatName))
                            {
                                MessageBox.Show("Please enter the Format Name in the row " + index1, "Alert");
                                validFileFotmats = null;
                                ultraTabControl1.Tabs[1].Selected = true;
                                grdFileFormat.Focus();
                                return validFileFotmats;
                            }
                            else if (String.IsNullOrEmpty(strPranaToThirdparty))
                            {
                                MessageBox.Show("Please select File 'Prana To ThirdParty' in the row " + index1, "Alert");
                                validFileFotmats = null;
                                ultraTabControl1.Tabs[1].Selected = true;
                                grdFileFormat.Focus();
                                return validFileFotmats;
                            }
                            else if (String.IsNullOrEmpty(strFileDisplayName))
                            {
                                MessageBox.Show("Please enter the File Dispaly Name in the Format '' in the row " + index1, "Alert");
                                validFileFotmats = null;
                                ultraTabControl1.Tabs[1].Selected = true;
                                grdFileFormat.Focus();
                                return validFileFotmats;
                            }
                            else if (String.IsNullOrEmpty(strDelimiter))
                            {
                                MessageBox.Show("Please enter the Delimiter in the row " + index1, "Alert");
                                validFileFotmats = null;
                                ultraTabControl1.Tabs[1].Selected = true;
                                grdFileFormat.Focus();
                                return validFileFotmats;
                            }
                            else if (String.IsNullOrEmpty(strDelimiterName))
                            {
                                MessageBox.Show("Please enter the Delimiter Name in the row " + index1, "Alert");
                                validFileFotmats = null;
                                ultraTabControl1.Tabs[1].Selected = true;
                                grdFileFormat.Focus();
                                return validFileFotmats;
                            }

                            if (!String.IsNullOrEmpty(strFileDisplayName))
                            {
                                string FileName = strFileDisplayName.Trim().ToString();

                                if (FileName.Contains("{") || FileName.Contains("}"))
                                {
                                    int startIndex = FileName.IndexOf("{");
                                    int lastIndex = FileName.LastIndexOf("}");
                                    string FileNameSubString = string.Empty;
                                    if (startIndex != -1 && lastIndex != -1)
                                    {
                                        FileNameSubString = FileName.Substring(startIndex + 1, (lastIndex - startIndex) - 1);
                                    }

                                    if (FileNameSubString.Equals(string.Empty) || (!FileNameSubString.Contains("M") && !FileNameSubString.Contains("d") && !FileNameSubString.Contains("y") && !FileNameSubString.Contains("h") && !FileNameSubString.Contains("m") && !FileNameSubString.Contains("s")))
                                    {
                                        MessageBox.Show("Please enter the proper Date Format eg : {MMddyy}", "Info");
                                        validFileFotmats = null;
                                        ultraTabControl1.Tabs[1].Selected = true;
                                        grdFileFormat.Focus();
                                        return validFileFotmats;
                                    }
                                }
                            }

                        }
                    }
                    index1 = index1 + 1;
                }

                // check for Name is not repeating 
                for (int index = 0; index < grdFileFormat.Rows.Count - 1; index++)
                {
                    string strFormatName = grdFileFormat.Rows[index].Cells["FileFormatName"].Value.ToString();

                    for (int localindex = index + 1; localindex < grdFileFormat.Rows.Count - 1; localindex++)
                    {
                        string innerFormatName = grdFileFormat.Rows[localindex].Cells["FileFormatName"].Value.ToString();

                        if (innerFormatName.Equals(strFormatName))
                        {
                            MessageBox.Show(" Format Name already exists in the row  " + (index + 1), "Alert");
                            validFileFotmats = null;
                            ultraTabControl1.Tabs[1].Selected = true;
                            grdFileFormat.Focus();
                            return validFileFotmats;
                        }
                    }
                }

                foreach (ThirdPartyFileFormat thirdPartyFileFormat in _thirdPartyFileFormats)
                {
                    if (!string.IsNullOrEmpty(thirdPartyFileFormat.FileFormatName) && (thirdPartyFileFormat.FIXEnabled || (thirdPartyFileFormat.FileEnabled
                        && !string.IsNullOrEmpty(thirdPartyFileFormat.PranaToThirdParty) && !string.IsNullOrEmpty(thirdPartyFileFormat.FileDisplayName)
                        && !string.IsNullOrEmpty(thirdPartyFileFormat.Delimiter) && !string.IsNullOrEmpty(thirdPartyFileFormat.DelimiterName))))
                    {
                        if (!string.IsNullOrEmpty(thirdPartyFileFormat.FileExtension.Trim()))
                        {
                            string fileExtValSubStr = thirdPartyFileFormat.FileExtension.Trim().Substring(0, 1);
                            if (fileExtValSubStr.Equals("."))
                            {
                                thirdPartyFileFormat.FileExtension = thirdPartyFileFormat.FileExtension.Trim().Substring(1, (thirdPartyFileFormat.FileExtension.Trim().Length) - 1);
                            }
                        }
                        validFileFotmats.Add(thirdPartyFileFormat);
                    }
                }
                return validFileFotmats;
            }
            set
            {
                _thirdPartyFileFormats = value;

                grdFileFormat.DataSource = _thirdPartyFileFormats;
                if (_thirdPartyFileFormats.Count > 0)
                {
                    AddNewRow();
                }
                else
                {
                    AddNewTempRow();
                }
                RefreshGrid();
            }
        }

        private void grdFileFormat_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        CheckBoxOnHeader_CreationFilter _headerCheckBoxUnallocated = new CheckBoxOnHeader_CreationFilter();

        #endregion Tab 2 of File Format

        private void btnDeleteFileFormat_Click(object sender, EventArgs e)
        {
            int result = 0;

            //Check whether any File Format is there to delete
            int gridRowCount = grdFileFormat.Rows.Count;
            if (gridRowCount > 2)
            {
                string strFormatName = grdFileFormat.ActiveRow.Cells["FileFormatName"].Text.ToString();
                string strPranaToThirdParty = grdFileFormat.ActiveRow.Cells["PranaToThirdParty"].Text.ToString();
                //string strThirdPartyToPrana = grdFileFormat.ActiveRow.Cells["ThirdPartyToPrana"].Text.ToString();

                // get all the Ids 
                int thirdPartyIdofIstRow = int.Parse(grdFileFormat.Rows[0].Cells["ThirdPartyId"].Value.ToString());
                int intFileFormatId = int.Parse(grdFileFormat.ActiveRow.Cells["FileFormatId"].Value.ToString());
                int intThirdPartyId = int.Parse(grdFileFormat.ActiveRow.Cells["ThirdPartyId"].Value.ToString());

                //if (!String.IsNullOrEmpty(strFormatName)  && !String.IsNullOrEmpty(strPranaToThirdParty) && !String.IsNullOrEmpty(strThirdPartyToPrana))
                if (!String.IsNullOrEmpty(strFormatName) && !String.IsNullOrEmpty(strPranaToThirdParty))
                {
                    //Asking the user to be sure about deleting the Commission Rule .
                    if (MessageBox.Show(this, "Do you want to delete the selected Format ?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        result = ThirdPartyDataManager.DeleteSelectedFileFormat(intThirdPartyId, intFileFormatId);
                        if (result.Equals(1))
                        {
                            MessageBox.Show("Format deleted.", "Alert");
                            if (gridRowCount > 2)
                            {
                                BindFileFormatGrid(thirdPartyIdofIstRow);
                            }
                            else
                            {
                                BindFileFormatGrid(0);
                            }
                        }
                        else if (result.Equals(0))
                        {
                            MessageBox.Show("Format cannot be deleted, it is referenced in the Third Party Flat Files.", "Alert");
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Please select the Format to delete.", "Alert");
                    result = 0;
                }
            }
            else
            {
                //Showing the message: No Data Available.
                //MessageBox.Show("Please select the Format to delete.", "Alert");
                MessageBox.Show("Atleast one Format should be there , you can not delete.", "Alert");
                result = 0;
            }

        }

        /// <summary>
        /// When the tab is changed and the file settings are not saved, confirm from the user if the settings need to be saved
        /// save the settings if the user chooses to do so and load the new settings in the batch setup tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //Modified by: Bharat raturi, 31 may 2014
            //purpose: to show the save file setting prompt only if theere are changes and batch setup tab is selected
            if (ctrlFileSetting._isSaveRequired && ultraTabControl1.SelectedTab.Key.Equals("batchSetup"))
            {
                DialogResult dr = MessageBox.Show("File settings have been modified. Do you want to save them now?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    int thirdPartyID = ctrlFileSetting.SaveFileSetting();
                    if (thirdPartyID == -1 && !ctrlFileSetting._isValidData)
                    {
                        ultraTabControl1.SelectedTab = ultraTabControl1.Tabs["importFileSetup"];
                    }
                    else
                    {
                        cntrlBatchSetup1.InitializeControl(thirdPartyID);
                    }
                }
                else
                {
                    ctrlFileSetting.RollBackChanges();
                }
            }
        }

        private void ultraTabPageControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        //added By: Bharat raturi
        //purpose: save file and batch settings for the third party
        /// <summary>
        /// Save the details of the file and the batch
        /// </summary>
        /// <returns>Positive integer if the data is saved</returns>
        public int SaveThirdPartyDetails()
        {
            int i = 0, thirdPartyID = 0;
            try
            {
                //if (ctrlFileSetting._isSaveRequired)
                //{
                //thirdPartyID= ctrlFileSetting.SaveFileSetting();
                thirdPartyID = ctrlFileSetting.SaveFileSetting();
                if (thirdPartyID == -1 && !ctrlFileSetting._isValidData)
                {
                    ultraTabControl1.SelectedTab = ultraTabControl1.Tabs["importFileSetup"];
                    return -1;
                }
                else
                {
                    i = cntrlBatchSetup1.SaveBatchDetails();
                    //cntrlBatchSetup1.InitializeControl(thirdPartyID);
                }
                //}
                //i = cntrlBatchSetup1.SaveBatchDetails();
                //if (thirdPartyID > 0)
                //{
                //    //ctrlFileSetting.InitializeControl(thirdPartyID,);
                //    cntrlBatchSetup1.InitializeControl(thirdPartyID);
                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return i;
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            using (frmThirdPartyEditor editor = new frmThirdPartyEditor())
            {
                editor.ShowDialog();
                ctrlFileSetting.RefreshAdvanceSettings();
            }
        }

        /// <summary>
        /// To save third party audit details.
        /// </summary>
        /// <param name="thirdParty"></param>
        public void SaveThirdPartyAuditDetails(Prana.BusinessObjects.ThirdParty thirdParty, int newAddedThirdpartyID)
        {
            try
            {
                //modified by : sachin mishra 25-feb-2015 Purpose-For saving the delete detail of counterparty in T_AdminAuditTrail.
                if (newAddedThirdpartyID == int.MaxValue)
                {
                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(thirdPartyAudit, thirdParty, AuditManager.Definitions.Enum.AuditAction.ThirdPartyDeleted);
                }
                else if (thirdParty.ThirdPartyID == int.MinValue)
                {
                    thirdParty.ThirdPartyID = newAddedThirdpartyID;
                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(thirdPartyAudit, thirdParty, AuditManager.Definitions.Enum.AuditAction.ThirdPartyCreated);
                }
                else
                {
                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(thirdPartyAudit, thirdParty, AuditManager.Definitions.Enum.AuditAction.ThirdPartyUpdated);
                }

                thirdPartyAudit.InitializeControl(thirdParty.ThirdPartyID);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtThirdPartyName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtThirdPartyName.Text != null)
                {
                    errorProvider1.SetError(txtThirdPartyName, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtShortName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtShortName.Text != null)
                {
                    errorProvider1.SetError(txtShortName, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtZip_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtZip.Text == null || !(txtZip.Text.Any(Char.IsLetter)))
                {
                    errorProvider1.SetError(txtZip, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtWorkTele_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtWorkTele.Text == null || !(txtWorkTele.Text.Any(Char.IsLetter)))
                {
                    errorProvider1.SetError(txtWorkTele, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtFax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFax.Text == null || !(txtFax.Text.Any(Char.IsLetter)))
                {
                    errorProvider1.SetError(txtFax, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtPCWorkTele_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPCWorkTele.Text == null || !(txtPCWorkTele.Text.Any(Char.IsLetter)))
                {
                    errorProvider1.SetError(txtPCWorkTele, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtCellPhone_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtCellPhone.Text == null || !(txtCellPhone.Text.Any(Char.IsLetter)))
                {
                    errorProvider1.SetError(txtCellPhone, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtPCFax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPCFax.Text == null || !(txtPCFax.Text.Any(Char.IsLetter)))
                {
                    errorProvider1.SetError(txtPCFax, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbThirdPartyType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbThirdPartyType.Value != null && (!int.Parse(cmbThirdPartyType.Value.ToString()).Equals(int.MinValue)))
                {
                    errorProvider1.SetError(cmbThirdPartyType, "");
                    if (int.Parse(cmbThirdPartyType.Value.ToString()) == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker)
                    {
                        groupBox3.Visible = true;
                        _isExecutingBroker = true;
                    }
                    else
                    {
                        _isExecutingBroker = false;
                        groupBox3.Visible = false;
                        cmbCounterParty.Value = int.MinValue;
                    }
                    HideShowFIXColumns();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindCounterParty()
        {
            try
            {
                CounterParties counterParties = new CounterParties();
                counterParties.Add(new BLL.CounterParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT));

                foreach (var kvp in CachedDataManager.GetInstance.GetAllCounterParties())
                {
                    counterParties.Add(new BLL.CounterParty(kvp.Key, kvp.Value));
                }

                cmbCounterParty.DisplayMember = "CounterPartyFullName";
                cmbCounterParty.ValueMember = "CounterPartyID";
                cmbCounterParty.DataSource = counterParties;
                cmbCounterParty.Value = int.MinValue;
                ColumnsCollection columns = cmbCounterParty.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "CounterPartyFullName")
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        cmbCounterParty.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbCounterParty_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCounterParty.Value != null && (!int.Parse(cmbCounterParty.Value.ToString()).Equals(int.MinValue)))
                {
                    errorProvider1.SetError(cmbCounterParty, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
