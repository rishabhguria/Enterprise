using Prana.Admin.BLL;
using Prana.Admin.Controls;
using Prana.BusinessObjects.Enums;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin
{
    public class CounterPartyVenueMaster : Form
    {
        #region Constants definitions

        private const string FORM_NAME = "CounterPartyVenueMaster : ";

        private const int C_TAB_COUNTERPARTY = 0;
        private const int C_TAB_VENUE = 1;
        private const int C_TAB_COUNTERPARTY_VENUE = 2;

        private const int C_TAB_COUNTERPARTYDETAIL = 0;
        private const int C_TAB_TRADINGINFORMATION = 1;

        private const int C_TAB_COUNTERPARTYVENUEDETAIL = 0;
        private const int C_TAB_ACCEPTEDORDERTYPES = 3;
        private const int C_TAB_FIX = 1;

        private const int C_TREE_COUNTERPARTY = 0;
        private const int C_TREE_VENUE = 1;
        private const int C_TREE_COUNTERPARTY_VENUE = 2;

        private const int VENUE_EXCHANGES = 1;
        private const int VENUE_ATS = 2;
        private const int VENUE_DESKS = 3;
        private const int VENUE_ROUTER = 4;
        private const int VENUE_ALGO = 5;

        private const string C_COMBO_SELECT = "- Select -";
        private const int OTHER = 3;

        #endregion Constants definitions

        #region Private and protected members

        private StatusBar stbCounterParty;
        private ErrorProvider errorProvider1;

        private TreeView trvCounterPartyVenue;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcCounterPartyVenue;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Button btnVenuesSave;
        private Button btnVenuesClose;
        private Button btnCounterPartyVenueClose;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCounterPartyVenueTabs;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        private Button btnAdd;
        private Button btnDelete;

        private Label label15;
        private GroupBox groupBox1;
        private Label label1;
        private Label label11;
        private Label label35;
        private Label label16;
        private TextBox txtPhone;
        private TextBox txtFax;
        private TextBox txtAddress1;
        private TextBox txtShortName;
        private TextBox txtFullName;
        private Label label13;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label33;
        private Label label34;
        private Button btmCVDetailClose;
        private Button btnSave;
        private Button btnCVDetailSave;
        private CounterPartyVenueDetails uctCounterPartyVenueDetails;
        private Controls.Fix uctFix;
        private Controls.SymbolMapping uctSymbolMapping;
        private CounterPartyVenueVenues uctcounterPartyVenueVenues;
        private uctCounterPartyVenueAcceptedOrderTypes uctCounterPartyVenueAcceptedOrderTypes;
        private GroupBox groupBox2;
        private Label label18;
        private Label label17;
        private TextBox txtEmail1;
        private TextBox txtContactName1;
        private TextBox txtTitle1;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label19;
        private TextBox txtAddress2;
        private Label lblAddress2;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCountry;
        private Label label25;
        private Label lblZip;
        private TextBox txtZip;
        private Label lblStateTerritory;
        private Label label26;
        private Label lblCountry;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbState;
        private TextBox txtPCLastName;
        private Label lblPCLastName;
        private TextBox txtPCCell;
        private Label lblCell;
        private Label label12;
        private TextBox txtPCWorkTel;
        private Label lblPCWorkTel;
        private Label label21;
        private GroupBox groupBox3;
        private TextBox txtTitle2;
        private TextBox txtEmail2;
        private TextBox txtContactName2;
        private Label label14;
        private Label label10;
        private Label label9;
        private TextBox txtSCLastName;
        private Label label22;
        private TextBox txtSCCell;
        private Label lblSCCell;
        private Label label23;
        private TextBox txtSCWorkTel;
        private Label lblSCWorkTel;
        private TextBox textBox2;
        private Label labelPCLastName;
        private TextBox textPCLastName;
        private Label label20;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterPartyType;
        private Label label43;
        private TextBox txtCity;
        private Label lblCity;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsAlgoBroker;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsOTDorEMS;
        private IContainer components;

        #endregion Private and protected members

        public CounterPartyVenueMaster()
        {
            InitializeComponent();

            SetUpMenuPermissions();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    uctCounterPartyVenueDetails.ApplyTheme();
                    uctSymbolMapping.ApplyTheme();
                    uctcounterPartyVenueVenues.BackColor = Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
                    uctcounterPartyVenueVenues.ForeColor = Color.White;

                    uctSymbolMapping.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    uctSymbolMapping.ForeColor = Color.White;

                    uctCounterPartyVenueAcceptedOrderTypes.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    uctCounterPartyVenueAcceptedOrderTypes.ForeColor = Color.White;

                    trvCounterPartyVenue.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    trvCounterPartyVenue.ForeColor = Color.White;
                    tbcCounterPartyVenue.Appearance.BackColor = Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                    tbcCounterPartyVenue.Appearance.ForeColor = Color.White;
                    tabCounterPartyVenueTabs.Appearance.BackColor = Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                    tabCounterPartyVenueTabs.Appearance.ForeColor = Color.White;

                    tabCounterPartyVenueTabs.ActiveTabAppearance.BackColor = Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                    tabCounterPartyVenueTabs.ActiveTabAppearance.ForeColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                    tbcCounterPartyVenue.ActiveTabAppearance.BackColor = Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                    tbcCounterPartyVenue.ActiveTabAppearance.ForeColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                    BackColor = Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));

                    groupBox1.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    groupBox1.ForeColor = Color.White;

                    groupBox2.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    groupBox2.ForeColor = Color.White;

                    groupBox3.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    groupBox3.ForeColor = Color.White;

                    MaximizeBox = true;
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

        private void PaintGradient()
        {
            try
            {
                System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
                gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(Width, Height), Color.Black, Color.LightGray);
                Bitmap bmp = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(gradBrush, new Rectangle(0, 0, Width, Height));

                BackgroundImage = bmp;
                BackgroundImageLayout = ImageLayout.Stretch;

                groupBox1.BackColor = Color.Transparent;
                groupBox1.BackgroundImage = bmp;
                groupBox1.BackgroundImageLayout = ImageLayout.Stretch;
                groupBox1.ForeColor = Color.White;

                groupBox2.BackColor = Color.Transparent;
                groupBox2.BackgroundImage = bmp;
                groupBox2.BackgroundImageLayout = ImageLayout.Stretch;
                groupBox2.ForeColor = Color.White;

                groupBox3.BackColor = Color.Transparent;
                groupBox3.BackgroundImage = bmp;
                groupBox3.BackgroundImageLayout = ImageLayout.Stretch;
                groupBox3.ForeColor = Color.White;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if(stbCounterParty != null)
                {
                    stbCounterParty.Dispose();
                }
                if(errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if(trvCounterPartyVenue != null)
                {
                    trvCounterPartyVenue.Dispose();
                }
                if(tbcCounterPartyVenue != null)
                {
                    tbcCounterPartyVenue.Dispose();
                }
                if(ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if(ultraTabSharedControlsPage3 != null)
                {
                    ultraTabSharedControlsPage3.Dispose();
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
                if(ultraTabPageControl5 != null)
                {
                    ultraTabPageControl5.Dispose();
                }
                if(ultraTabPageControl6 != null)
                {
                    ultraTabPageControl6.Dispose();
                }
                if(ultraTabPageControl7 != null)
                {
                    ultraTabPageControl7.Dispose();
                }
                if(ultraTabPageControl8 != null)
                {
                    ultraTabPageControl8.Dispose();
                }
                if(btnCounterPartyVenueClose != null)
                {
                    btnCounterPartyVenueClose.Dispose();
                }
                if(btnVenuesClose != null)
                {
                    btnVenuesClose.Dispose();
                }
                if(btnVenuesSave != null)
                {
                    btnVenuesSave.Dispose();
                }
                if(tabCounterPartyVenueTabs != null)
                {
                    tabCounterPartyVenueTabs.Dispose();
                }
                if(btnAdd != null)
                {
                    btnAdd.Dispose();
                }
                if(btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if(label1 != null)
                {
                    label1.Dispose();
                }
                if(label10 != null)
                {
                    label10.Dispose();
                }
                if(label11 != null)
                {
                    label11.Dispose();
                }
                if(label12 != null)
                {
                    label12.Dispose();
                }
                if(label13 != null)
                {
                    label13.Dispose();
                }
                if(label14 != null)
                {
                    label14.Dispose();
                }
                if(label15 != null)
                {
                    label15.Dispose();
                }
                if(label16 != null)
                {
                    label16.Dispose();
                }
                if(label17 != null)
                {
                    label17.Dispose();
                }
                if(label18 != null)
                {
                    label18.Dispose();
                }
                if(label19 != null)
                {
                    label19.Dispose();
                }
                if(label2 != null)
                {
                    label2.Dispose();
                }
                if(label21 != null)
                {
                    label21.Dispose();
                }
                if(label20 != null)
                {
                    label20.Dispose();
                }
                if(label22 != null)
                {
                    label22.Dispose();
                }
                if(label23 != null)
                {
                    label23.Dispose();
                }
                if(label25 != null)
                {
                    label25.Dispose();
                }
                if(label26 != null)
                {
                    label26.Dispose();
                }
                if(label3 != null)
                {
                    label3.Dispose();
                }
                if(label33 != null)
                {
                    label33.Dispose();
                }
                if(label34 != null)
                {
                    label34.Dispose();
                }
                if(label35 != null)
                {
                    label35.Dispose();
                }
                if(label4 != null)
                {
                    label4.Dispose();
                }
                if(label43 != null)
                {
                    label43.Dispose();
                }
                if(label5 != null)
                {
                    label5.Dispose();
                }
                if(label6 != null)
                {
                    label6.Dispose();
                }
                if(label7 != null)
                {
                    label7.Dispose();
                }
                if(label8 != null)
                {
                    label8.Dispose();
                }
                if(label9 != null)
                {
                    label9.Dispose();
                }
                if(labelPCLastName != null)
                {
                    labelPCLastName.Dispose();
                }
                if(txtAddress1 != null)
                {
                    txtAddress1.Dispose();
                }
                if(txtAddress2 != null)
                {
                    txtAddress2.Dispose();
                }
                if(txtCity != null)
                {
                    txtCity.Dispose();
                }
                if(txtContactName1 != null)
                {
                    txtContactName1.Dispose();
                }
                if(txtContactName2 != null)
                {
                    txtContactName2.Dispose();
                }
                if(txtEmail1 != null)
                {
                    txtEmail1.Dispose();
                }
                if(txtEmail2 != null)
                {
                    txtEmail2.Dispose();
                }
                if(txtFax != null)
                {
                    txtFax.Dispose();
                }
                if(txtFullName != null)
                {
                    txtFullName.Dispose();
                }
                if(txtPCCell != null)
                {
                    txtPCCell.Dispose();
                }
                if(txtPCLastName != null)
                {
                    txtPCLastName.Dispose();
                }
                if(txtPCWorkTel != null)
                {
                    txtPCWorkTel.Dispose();
                }
                if(txtPhone != null)
                {
                    txtPhone.Dispose();
                }
                if(txtSCCell != null)
                {
                    txtSCCell.Dispose();
                }
                if(txtSCLastName != null)
                {
                    txtSCLastName.Dispose();
                }
                if(txtSCWorkTel != null)
                {
                    txtSCWorkTel.Dispose();
                }
                if(txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if(txtTitle1 != null)
                {
                    txtTitle1.Dispose();
                }
                if(txtTitle2 != null)
                {
                    txtTitle2.Dispose();
                }
                if(txtZip != null)
                {
                    txtZip.Dispose();
                }
                if(btnSave != null)
                {
                    btnSave.Dispose();
                }
                if(btnCVDetailSave != null)
                {
                    btnCVDetailSave.Dispose();
                }
                if(btmCVDetailClose != null)
                {
                    btmCVDetailClose.Dispose();
                }
                if(uctcounterPartyVenueVenues != null)
                {
                    uctcounterPartyVenueVenues.Dispose();
                }
                if(uctCounterPartyVenueDetails != null)
                {
                    uctCounterPartyVenueDetails.Dispose();
                }
                if(uctCounterPartyVenueAcceptedOrderTypes != null)
                {
                    uctCounterPartyVenueAcceptedOrderTypes.Dispose();
                }
                if(uctFix != null)
                {
                    uctFix.Dispose();
                }
                if(uctSymbolMapping != null)
                {
                    uctSymbolMapping.Dispose();
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
                if(lblAddress2 != null)
                {
                    lblAddress2.Dispose();
                }
                if(lblCell != null)
                {
                    lblCell.Dispose();
                }
                if(lblCity != null)
                {
                    lblCity.Dispose();
                }
                if(lblCountry != null)
                {
                    lblCountry.Dispose();
                }
                if(lblSCCell != null)
                {
                    lblSCCell.Dispose();
                }
                if(lblPCLastName != null)
                {
                    lblPCLastName.Dispose();
                }
                if(lblPCWorkTel != null)
                {
                    lblPCWorkTel.Dispose();
                }
                if(lblSCWorkTel != null)
                {
                    lblSCWorkTel.Dispose();
                }
                if(lblStateTerritory != null)
                {
                    lblStateTerritory.Dispose();
                }
                if(lblZip != null)
                {
                    lblZip.Dispose();
                }
                if(cmbCounterPartyType != null)
                {
                    cmbCounterPartyType.Dispose();
                }
                if(cmbCountry != null)
                {
                    cmbCountry.Dispose();
                }
                if(cmbState != null)
                {
                    cmbState.Dispose();
                }
                if(textBox2 != null)
                {
                    textBox2.Dispose();
                }
                if(textPCLastName != null)
                {
                    textPCLastName.Dispose();
                }
                if(chkIsAlgoBroker != null)
                {
                    chkIsAlgoBroker.Dispose();
                }
                if(chkIsOTDorEMS != null)
                {
                    chkIsOTDorEMS.Dispose();
                }
                if (frmaddCounterPartyVenue != null)
                {
                    frmaddCounterPartyVenue.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private bool chkAddCVMaster = false;
        private bool chkDeleteCVMaster = false;
        private bool chkEditCVMaster = false;

        private void SetUpMenuPermissions()
        {
            ModuleResources module = ModuleResources.CV_Master;
            AuthAction action = AuthAction.Write;
            var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);
            if (hasAccess)
            {
                chkAddCVMaster = true;
                chkDeleteCVMaster = true;
                chkEditCVMaster = true;
            }

            if (chkAddCVMaster == false)
            {
                btnAdd.Enabled = false;
            }
            if (chkDeleteCVMaster == false)
            {
                btnDelete.Enabled = false;
            }
            if (chkEditCVMaster == false)
            {
                btnSave.Enabled = false;
                btnVenuesSave.Enabled = false;
                btnCVDetailSave.Enabled = false;
            }
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Prana.Admin.BLL.CounterPartyVenue counterPartyVenue2 = new Prana.Admin.BLL.CounterPartyVenue();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyTypeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CounterPartyVenueMaster));
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctCounterPartyVenueDetails = new Prana.Admin.Controls.CounterPartyVenueDetails();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctFix = new Prana.Admin.Controls.Fix();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctSymbolMapping = new Prana.Admin.Controls.SymbolMapping();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctCounterPartyVenueAcceptedOrderTypes = new Prana.Admin.Controls.uctCounterPartyVenueAcceptedOrderTypes();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtSCCell = new System.Windows.Forms.TextBox();
            this.lblSCCell = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtSCWorkTel = new System.Windows.Forms.TextBox();
            this.lblSCWorkTel = new System.Windows.Forms.Label();
            this.txtSCLastName = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtTitle2 = new System.Windows.Forms.TextBox();
            this.txtEmail2 = new System.Windows.Forms.TextBox();
            this.txtContactName2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtPCCell = new System.Windows.Forms.TextBox();
            this.lblCell = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPCWorkTel = new System.Windows.Forms.TextBox();
            this.lblPCWorkTel = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtPCLastName = new System.Windows.Forms.TextBox();
            this.lblPCLastName = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEmail1 = new System.Windows.Forms.TextBox();
            this.txtContactName1 = new System.Windows.Forms.TextBox();
            this.txtTitle1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsAlgoBroker = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkIsOTDorEMS = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label43 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.cmbCounterPartyType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbState = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label25 = new System.Windows.Forms.Label();
            this.lblZip = new System.Windows.Forms.Label();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.lblStateTerritory = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btmCVDetailClose = new System.Windows.Forms.Button();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctcounterPartyVenueVenues = new Prana.Admin.Controls.CounterPartyVenueVenues();
            this.btnVenuesClose = new System.Windows.Forms.Button();
            this.btnVenuesSave = new System.Windows.Forms.Button();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabCounterPartyVenueTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnCounterPartyVenueClose = new System.Windows.Forms.Button();
            this.btnCVDetailSave = new System.Windows.Forms.Button();
            this.stbCounterParty = new System.Windows.Forms.StatusBar();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.trvCounterPartyVenue = new System.Windows.Forms.TreeView();
            this.tbcCounterPartyVenue = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.labelPCLastName = new System.Windows.Forms.Label();
            this.textPCLastName = new System.Windows.Forms.TextBox();
            this.ultraTabPageControl5.SuspendLayout();
            this.ultraTabPageControl6.SuspendLayout();
            this.ultraTabPageControl7.SuspendLayout();
            this.ultraTabPageControl8.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsAlgoBroker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOTDorEMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCounterPartyVenueTabs)).BeginInit();
            this.tabCounterPartyVenueTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcCounterPartyVenue)).BeginInit();
            this.tbcCounterPartyVenue.SuspendLayout();
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
            this.uctCounterPartyVenueDetails.AutoScroll = true;
            this.uctCounterPartyVenueDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uctCounterPartyVenueDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctCounterPartyVenueDetails.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.uctCounterPartyVenueDetails.Location = new System.Drawing.Point(4, 6);
            this.uctCounterPartyVenueDetails.Name = "uctCounterPartyVenueDetails";
            this.uctCounterPartyVenueDetails.Size = new System.Drawing.Size(366, 540);
            this.uctCounterPartyVenueDetails.TabIndex = 0;
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Controls.Add(this.uctFix);
            this.ultraTabPageControl6.Enabled = false;
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(374, 547);
            // 
            // uctFix
            // 
            this.uctFix.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.uctFix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            counterPartyVenue2.Acronym = "";
            counterPartyVenue2.AdvancedOrdersID = -2147483648;
            counterPartyVenue2.AUECID = -2147483648;
            counterPartyVenue2.BaseCurrencyID = -2147483648;
            counterPartyVenue2.CompanyCounterPartyCVID = -2147483648;
            counterPartyVenue2.CounterPartyID = -2147483648;
            counterPartyVenue2.CounterPartyVenueDetailsID = -2147483648;
            counterPartyVenue2.CounterPartyVenueID = -2147483648;
            counterPartyVenue2.CurrencyTypeID = -2147483648;
            counterPartyVenue2.CVAUECComplianceID = -2147483648;
            counterPartyVenue2.CVAUECID = -2147483648;
            counterPartyVenue2.CVFIXID = -2147483648;
            counterPartyVenue2.DeliverToCompID = "";
            counterPartyVenue2.DeliverToSubID = "";
            counterPartyVenue2.DisplayName = "";
            counterPartyVenue2.ExecutionInstructionsID = "";
            counterPartyVenue2.FixIdentifier = "";
            counterPartyVenue2.FixVersionID = -2147483648;
            counterPartyVenue2.FollowCompliance = -2147483648;
            counterPartyVenue2.ForeignID = "";
            counterPartyVenue2.HandlingInstructionsID = "";
            counterPartyVenue2.IdentifierID = -2147483648;
            counterPartyVenue2.IsElectronic = -2147483648;
            counterPartyVenue2.OatsIdentifier = "";
            counterPartyVenue2.OrderTypesID = "";
            counterPartyVenue2.OtherCurrencyID = -2147483648;
            counterPartyVenue2.ShortSellConfirmation = -2147483648;
            counterPartyVenue2.SideID = "";
            counterPartyVenue2.SymbolConventionID = -2147483648;
            counterPartyVenue2.TargetCompID = "";
            counterPartyVenue2.TimeInForceID = "";
            counterPartyVenue2.VenueID = -2147483648;
            this.uctFix.CounterPartyProperty = counterPartyVenue2;
            this.uctFix.CounterPartyVenueID = -2147483648;
            this.uctFix.Location = new System.Drawing.Point(46, 6);
            this.uctFix.Name = "uctFix";
            this.uctFix.Size = new System.Drawing.Size(300, 170);
            this.uctFix.TabIndex = 0;
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Controls.Add(this.uctSymbolMapping);
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(374, 547);
            // 
            // uctSymbolMapping
            // 
            this.uctSymbolMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.uctSymbolMapping.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
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
            // uctCounterPartyVenueAcceptedOrderTypes
            // 
            this.uctCounterPartyVenueAcceptedOrderTypes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.uctCounterPartyVenueAcceptedOrderTypes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctCounterPartyVenueAcceptedOrderTypes.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.uctCounterPartyVenueAcceptedOrderTypes.Location = new System.Drawing.Point(12, 2);
            this.uctCounterPartyVenueAcceptedOrderTypes.Name = "uctCounterPartyVenueAcceptedOrderTypes";
            this.uctCounterPartyVenueAcceptedOrderTypes.Size = new System.Drawing.Size(350, 554);
            this.uctCounterPartyVenueAcceptedOrderTypes.TabIndex = 0;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.groupBox3);
            this.ultraTabPageControl1.Controls.Add(this.groupBox2);
            this.ultraTabPageControl1.Controls.Add(this.label15);
            this.ultraTabPageControl1.Controls.Add(this.groupBox1);
            this.ultraTabPageControl1.Controls.Add(this.btnSave);
            this.ultraTabPageControl1.Controls.Add(this.btmCVDetailClose);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(388, 668);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtSCCell);
            this.groupBox3.Controls.Add(this.lblSCCell);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.txtSCWorkTel);
            this.groupBox3.Controls.Add(this.lblSCWorkTel);
            this.groupBox3.Controls.Add(this.txtSCLastName);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.txtTitle2);
            this.groupBox3.Controls.Add(this.txtEmail2);
            this.groupBox3.Controls.Add(this.txtContactName2);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox3.Location = new System.Drawing.Point(40, 465);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(308, 157);
            this.groupBox3.TabIndex = 97;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Secondary Contact";
            // 
            // txtSCCell
            // 
            this.txtSCCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSCCell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtSCCell.Location = new System.Drawing.Point(135, 131);
            this.txtSCCell.MaxLength = 50;
            this.txtSCCell.Name = "txtSCCell";
            this.txtSCCell.Size = new System.Drawing.Size(150, 21);
            this.txtSCCell.TabIndex = 26;
            this.txtSCCell.GotFocus += new System.EventHandler(this.txtSCCell_GotFocus);
            this.txtSCCell.LostFocus += new System.EventHandler(this.txtSCCell_LostFocus);
            // 
            // lblSCCell
            // 
            this.lblSCCell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSCCell.Location = new System.Drawing.Point(7, 136);
            this.lblSCCell.Name = "lblSCCell";
            this.lblSCCell.Size = new System.Drawing.Size(32, 16);
            this.lblSCCell.TabIndex = 108;
            this.lblSCCell.Text = "Cell";
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label23.Location = new System.Drawing.Point(7, 116);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(100, 16);
            this.label23.TabIndex = 107;
            this.label23.Text = "(1-111-111111)";
            // 
            // txtSCWorkTel
            // 
            this.txtSCWorkTel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSCWorkTel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtSCWorkTel.Location = new System.Drawing.Point(135, 102);
            this.txtSCWorkTel.MaxLength = 50;
            this.txtSCWorkTel.Name = "txtSCWorkTel";
            this.txtSCWorkTel.Size = new System.Drawing.Size(150, 21);
            this.txtSCWorkTel.TabIndex = 25;
            this.txtSCWorkTel.GotFocus += new System.EventHandler(this.txtSCWorkTel_GotFocus);
            this.txtSCWorkTel.LostFocus += new System.EventHandler(this.txtSCWorkTel_LostFocus);
            // 
            // lblSCWorkTel
            // 
            this.lblSCWorkTel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSCWorkTel.Location = new System.Drawing.Point(7, 102);
            this.lblSCWorkTel.Name = "lblSCWorkTel";
            this.lblSCWorkTel.Size = new System.Drawing.Size(48, 16);
            this.lblSCWorkTel.TabIndex = 104;
            this.lblSCWorkTel.Text = "Work #";
            // 
            // txtSCLastName
            // 
            this.txtSCLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSCLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtSCLastName.Location = new System.Drawing.Point(135, 36);
            this.txtSCLastName.MaxLength = 50;
            this.txtSCLastName.Name = "txtSCLastName";
            this.txtSCLastName.Size = new System.Drawing.Size(150, 21);
            this.txtSCLastName.TabIndex = 22;
            this.txtSCLastName.GotFocus += new System.EventHandler(this.txtSCLastName_GotFocus);
            this.txtSCLastName.LostFocus += new System.EventHandler(this.txtSCLastName_LostFocus);
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label22.Location = new System.Drawing.Point(7, 38);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(65, 16);
            this.label22.TabIndex = 98;
            this.label22.Text = "Last Name";
            // 
            // txtTitle2
            // 
            this.txtTitle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtTitle2.Location = new System.Drawing.Point(135, 58);
            this.txtTitle2.MaxLength = 20;
            this.txtTitle2.Name = "txtTitle2";
            this.txtTitle2.Size = new System.Drawing.Size(150, 21);
            this.txtTitle2.TabIndex = 23;
            this.txtTitle2.GotFocus += new System.EventHandler(this.txtTitle2_GotFocus);
            this.txtTitle2.LostFocus += new System.EventHandler(this.txtTitle2_LostFocus);
            // 
            // txtEmail2
            // 
            this.txtEmail2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtEmail2.Location = new System.Drawing.Point(135, 80);
            this.txtEmail2.MaxLength = 20;
            this.txtEmail2.Name = "txtEmail2";
            this.txtEmail2.Size = new System.Drawing.Size(150, 21);
            this.txtEmail2.TabIndex = 24;
            this.txtEmail2.GotFocus += new System.EventHandler(this.txtEmail2_GotFocus);
            this.txtEmail2.LostFocus += new System.EventHandler(this.txtEmail2_LostFocus);
            // 
            // txtContactName2
            // 
            this.txtContactName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContactName2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtContactName2.Location = new System.Drawing.Point(135, 14);
            this.txtContactName2.MaxLength = 50;
            this.txtContactName2.Name = "txtContactName2";
            this.txtContactName2.Size = new System.Drawing.Size(150, 21);
            this.txtContactName2.TabIndex = 21;
            this.txtContactName2.GotFocus += new System.EventHandler(this.txtContactName2_GotFocus);
            this.txtContactName2.LostFocus += new System.EventHandler(this.txtContactName2_LostFocus);
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label14.Location = new System.Drawing.Point(7, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 16);
            this.label14.TabIndex = 83;
            this.label14.Text = "First Name";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.Location = new System.Drawing.Point(7, 82);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 82;
            this.label10.Text = "E-mail";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(7, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 16);
            this.label9.TabIndex = 81;
            this.label9.Text = "Title";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.txtPCCell);
            this.groupBox2.Controls.Add(this.lblCell);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtPCWorkTel);
            this.groupBox2.Controls.Add(this.lblPCWorkTel);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.txtPCLastName);
            this.groupBox2.Controls.Add(this.lblPCLastName);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.txtEmail1);
            this.groupBox2.Controls.Add(this.txtContactName1);
            this.groupBox2.Controls.Add(this.txtTitle1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox2.Location = new System.Drawing.Point(40, 305);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 157);
            this.groupBox2.TabIndex = 96;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Primary Contact";
            // 
            // label20
            // 
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(38, 133);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(8, 16);
            this.label20.TabIndex = 104;
            this.label20.Text = "*";
            // 
            // txtPCCell
            // 
            this.txtPCCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPCCell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtPCCell.Location = new System.Drawing.Point(135, 131);
            this.txtPCCell.MaxLength = 50;
            this.txtPCCell.Name = "txtPCCell";
            this.txtPCCell.Size = new System.Drawing.Size(150, 21);
            this.txtPCCell.TabIndex = 20;
            this.txtPCCell.GotFocus += new System.EventHandler(this.txtPCCell_GotFocus);
            this.txtPCCell.LostFocus += new System.EventHandler(this.txtPCCell_LostFocus);
            // 
            // lblCell
            // 
            this.lblCell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCell.Location = new System.Drawing.Point(7, 136);
            this.lblCell.Name = "lblCell";
            this.lblCell.Size = new System.Drawing.Size(32, 16);
            this.lblCell.TabIndex = 102;
            this.lblCell.Text = "Cell";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(7, 116);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 16);
            this.label12.TabIndex = 101;
            this.label12.Text = "(1-111-111111)";
            // 
            // txtPCWorkTel
            // 
            this.txtPCWorkTel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPCWorkTel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtPCWorkTel.Location = new System.Drawing.Point(135, 102);
            this.txtPCWorkTel.MaxLength = 50;
            this.txtPCWorkTel.Name = "txtPCWorkTel";
            this.txtPCWorkTel.Size = new System.Drawing.Size(150, 21);
            this.txtPCWorkTel.TabIndex = 19;
            this.txtPCWorkTel.GotFocus += new System.EventHandler(this.txtPCWorkTel_GotFocus);
            this.txtPCWorkTel.LostFocus += new System.EventHandler(this.txtPCWorkTel_LostFocus);
            // 
            // lblPCWorkTel
            // 
            this.lblPCWorkTel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCWorkTel.Location = new System.Drawing.Point(7, 102);
            this.lblPCWorkTel.Name = "lblPCWorkTel";
            this.lblPCWorkTel.Size = new System.Drawing.Size(48, 16);
            this.lblPCWorkTel.TabIndex = 98;
            this.lblPCWorkTel.Text = "Work #";
            // 
            // label21
            // 
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(55, 102);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(8, 16);
            this.label21.TabIndex = 100;
            this.label21.Text = "*";
            // 
            // txtPCLastName
            // 
            this.txtPCLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPCLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtPCLastName.Location = new System.Drawing.Point(135, 36);
            this.txtPCLastName.MaxLength = 50;
            this.txtPCLastName.Name = "txtPCLastName";
            this.txtPCLastName.Size = new System.Drawing.Size(150, 21);
            this.txtPCLastName.TabIndex = 16;
            this.txtPCLastName.GotFocus += new System.EventHandler(this.txtPCLastName_GotFocus);
            this.txtPCLastName.LostFocus += new System.EventHandler(this.txtPCLastName_LostFocus);
            // 
            // lblPCLastName
            // 
            this.lblPCLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCLastName.Location = new System.Drawing.Point(7, 38);
            this.lblPCLastName.Name = "lblPCLastName";
            this.lblPCLastName.Size = new System.Drawing.Size(65, 16);
            this.lblPCLastName.TabIndex = 96;
            this.lblPCLastName.Text = "Last Name";
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(47, 88);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(8, 16);
            this.label18.TabIndex = 95;
            this.label18.Text = "*";
            // 
            // label17
            // 
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(73, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(8, 16);
            this.label17.TabIndex = 94;
            this.label17.Text = "*";
            // 
            // txtEmail1
            // 
            this.txtEmail1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtEmail1.Location = new System.Drawing.Point(135, 80);
            this.txtEmail1.MaxLength = 50;
            this.txtEmail1.Name = "txtEmail1";
            this.txtEmail1.Size = new System.Drawing.Size(150, 21);
            this.txtEmail1.TabIndex = 18;
            this.txtEmail1.GotFocus += new System.EventHandler(this.txtEmail1_GotFocus);
            this.txtEmail1.LostFocus += new System.EventHandler(this.txtEmail1_LostFocus);
            // 
            // txtContactName1
            // 
            this.txtContactName1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContactName1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtContactName1.Location = new System.Drawing.Point(135, 14);
            this.txtContactName1.MaxLength = 50;
            this.txtContactName1.Name = "txtContactName1";
            this.txtContactName1.Size = new System.Drawing.Size(150, 21);
            this.txtContactName1.TabIndex = 15;
            this.txtContactName1.GotFocus += new System.EventHandler(this.txtContactName1_GotFocus);
            this.txtContactName1.LostFocus += new System.EventHandler(this.txtContactName1_LostFocus);
            // 
            // txtTitle1
            // 
            this.txtTitle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtTitle1.Location = new System.Drawing.Point(135, 58);
            this.txtTitle1.MaxLength = 50;
            this.txtTitle1.Name = "txtTitle1";
            this.txtTitle1.Size = new System.Drawing.Size(150, 21);
            this.txtTitle1.TabIndex = 17;
            this.txtTitle1.GotFocus += new System.EventHandler(this.txtTitle1_GotFocus);
            this.txtTitle1.LostFocus += new System.EventHandler(this.txtTitle1_LostFocus);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(7, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 16);
            this.label8.TabIndex = 90;
            this.label8.Text = "E-mail";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(7, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 16);
            this.label7.TabIndex = 89;
            this.label7.Text = "Title";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(7, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 16);
            this.label6.TabIndex = 88;
            this.label6.Text = "First Name";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(7, 643);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 16);
            this.label15.TabIndex = 89;
            this.label15.Text = "* Required Field";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkIsAlgoBroker);
            this.groupBox1.Controls.Add(this.chkIsOTDorEMS);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.txtCity);
            this.groupBox1.Controls.Add(this.lblCity);
            this.groupBox1.Controls.Add(this.cmbCounterPartyType);
            this.groupBox1.Controls.Add(this.cmbState);
            this.groupBox1.Controls.Add(this.cmbCountry);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.lblZip);
            this.groupBox1.Controls.Add(this.txtZip);
            this.groupBox1.Controls.Add(this.lblStateTerritory);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.lblCountry);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.txtAddress2);
            this.groupBox1.Controls.Add(this.lblAddress2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtPhone);
            this.groupBox1.Controls.Add(this.txtFax);
            this.groupBox1.Controls.Add(this.txtAddress1);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Controls.Add(this.txtFullName);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Controls.Add(this.label34);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(40, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 292);
            this.groupBox1.TabIndex = 88;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contact Details";
            // 
            // chkIsAlgoBroker
            // 
            this.chkIsAlgoBroker.AutoSize = true;
            this.chkIsAlgoBroker.Location = new System.Drawing.Point(5, 269);
            this.chkIsAlgoBroker.Name = "chkIsAlgoBroker";
            this.chkIsAlgoBroker.Size = new System.Drawing.Size(103, 18);
            this.chkIsAlgoBroker.TabIndex = 210;
            this.chkIsAlgoBroker.Text = "Is Algo Broker";
            // 
            // chkIsOTDorEMS
            // 
            this.chkIsOTDorEMS.AutoSize = true;
            this.chkIsOTDorEMS.Location = new System.Drawing.Point(110, 269);
            this.chkIsOTDorEMS.Name = "chkIsOTDorEMS";
            this.chkIsOTDorEMS.Size = new System.Drawing.Size(196, 18);
            this.chkIsOTDorEMS.TabIndex = 210;
            this.chkIsOTDorEMS.Text = "Hide from Trade entry tools";
            // 
            // label43
            // 
            this.label43.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(34, 170);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(8, 8);
            this.label43.TabIndex = 209;
            this.label43.Text = "*";
            // 
            // txtCity
            // 
            this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCity.Location = new System.Drawing.Point(136, 168);
            this.txtCity.MaxLength = 50;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(150, 21);
            this.txtCity.TabIndex = 11;
            this.txtCity.GotFocus += new System.EventHandler(this.txtCity_GotFocus);
            this.txtCity.LostFocus += new System.EventHandler(this.txtCity_LostFocus);
            // 
            // lblCity
            // 
            this.lblCity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCity.Location = new System.Drawing.Point(8, 170);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(28, 16);
            this.lblCity.TabIndex = 208;
            this.lblCity.Text = "City";
            // 
            // cmbCounterPartyType
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterPartyType.DisplayLayout.Appearance = appearance1;
            this.cmbCounterPartyType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbCounterPartyType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbCounterPartyType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterPartyType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterPartyType.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterPartyType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbCounterPartyType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterPartyType.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbCounterPartyType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterPartyType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterPartyType.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterPartyType.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbCounterPartyType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterPartyType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterPartyType.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterPartyType.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbCounterPartyType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterPartyType.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterPartyType.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbCounterPartyType.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbCounterPartyType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterPartyType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterPartyType.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbCounterPartyType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterPartyType.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbCounterPartyType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterPartyType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterPartyType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterPartyType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterPartyType.DropDownWidth = 0;
            this.cmbCounterPartyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCounterPartyType.Location = new System.Drawing.Point(136, 58);
            this.cmbCounterPartyType.Name = "cmbCounterPartyType";
            this.cmbCounterPartyType.Size = new System.Drawing.Size(150, 21);
            this.cmbCounterPartyType.TabIndex = 6;
            this.cmbCounterPartyType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterPartyType.GotFocus += new System.EventHandler(this.cmbCounterPartyType_GotFocus);
            this.cmbCounterPartyType.LostFocus += new System.EventHandler(this.cmbCounterPartyType_LostFocus);
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
            this.cmbState.Location = new System.Drawing.Point(136, 146);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(150, 21);
            this.cmbState.TabIndex = 10;
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
            this.cmbCountry.Location = new System.Drawing.Point(136, 124);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(150, 21);
            this.cmbCountry.TabIndex = 9;
            this.cmbCountry.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
            this.cmbCountry.GotFocus += new System.EventHandler(this.cmbCountry_GotFocus);
            this.cmbCountry.LostFocus += new System.EventHandler(this.cmbCountry_LostFocus);
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(93, 145);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(12, 8);
            this.label25.TabIndex = 203;
            this.label25.Text = "*";
            // 
            // lblZip
            // 
            this.lblZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblZip.Location = new System.Drawing.Point(8, 187);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(32, 22);
            this.lblZip.TabIndex = 202;
            this.lblZip.Text = "Zip";
            this.lblZip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtZip
            // 
            this.txtZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtZip.Location = new System.Drawing.Point(136, 190);
            this.txtZip.MaxLength = 50;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(150, 21);
            this.txtZip.TabIndex = 12;
            this.txtZip.GotFocus += new System.EventHandler(this.txtZip_GotFocus);
            this.txtZip.LostFocus += new System.EventHandler(this.txtZip_LostFocus);
            // 
            // lblStateTerritory
            // 
            this.lblStateTerritory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStateTerritory.Location = new System.Drawing.Point(8, 145);
            this.lblStateTerritory.Name = "lblStateTerritory";
            this.lblStateTerritory.Size = new System.Drawing.Size(88, 22);
            this.lblStateTerritory.TabIndex = 201;
            this.lblStateTerritory.Text = "State/Territory";
            this.lblStateTerritory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(61, 124);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(12, 12);
            this.label26.TabIndex = 200;
            this.label26.Text = "*";
            // 
            // lblCountry
            // 
            this.lblCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCountry.Location = new System.Drawing.Point(8, 122);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(54, 22);
            this.lblCountry.TabIndex = 199;
            this.lblCountry.Text = "Country";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(64, 80);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(8, 16);
            this.label19.TabIndex = 97;
            this.label19.Text = "*";
            // 
            // txtAddress2
            // 
            this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtAddress2.Location = new System.Drawing.Point(136, 102);
            this.txtAddress2.MaxLength = 50;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(150, 21);
            this.txtAddress2.TabIndex = 8;
            this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
            this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
            // 
            // lblAddress2
            // 
            this.lblAddress2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAddress2.Location = new System.Drawing.Point(8, 104);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(64, 16);
            this.lblAddress2.TabIndex = 95;
            this.lblAddress2.Text = "Address 2";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(112, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 16);
            this.label1.TabIndex = 93;
            this.label1.Text = "*";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(8, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 16);
            this.label11.TabIndex = 91;
            this.label11.Text = "Broker Type";
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label35.Location = new System.Drawing.Point(8, 226);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(100, 16);
            this.label35.TabIndex = 90;
            this.label35.Text = "(1-111-111111)";
            // 
            // label16
            // 
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(48, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(8, 16);
            this.label16.TabIndex = 85;
            this.label16.Text = "*";
            // 
            // txtPhone
            // 
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(136, 212);
            this.txtPhone.MaxLength = 50;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(150, 21);
            this.txtPhone.TabIndex = 13;
            this.txtPhone.GotFocus += new System.EventHandler(this.txtPhone_GotFocus);
            this.txtPhone.LostFocus += new System.EventHandler(this.txtPhone_LostFocus);
            // 
            // txtFax
            // 
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtFax.Location = new System.Drawing.Point(136, 242);
            this.txtFax.MaxLength = 50;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(150, 21);
            this.txtFax.TabIndex = 14;
            this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
            this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
            // 
            // txtAddress1
            // 
            this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtAddress1.Location = new System.Drawing.Point(136, 80);
            this.txtAddress1.MaxLength = 50;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(150, 21);
            this.txtAddress1.TabIndex = 7;
            this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
            this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtShortName.Location = new System.Drawing.Point(136, 36);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(150, 21);
            this.txtShortName.TabIndex = 5;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // txtFullName
            // 
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtFullName.Location = new System.Drawing.Point(136, 14);
            this.txtFullName.MaxLength = 50;
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(150, 21);
            this.txtFullName.TabIndex = 4;
            this.txtFullName.GotFocus += new System.EventHandler(this.txtFullName_GotFocus);
            this.txtFullName.LostFocus += new System.EventHandler(this.txtFullName_LostFocus);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(8, 82);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 16);
            this.label13.TabIndex = 68;
            this.label13.Text = "Address 1";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(8, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 60;
            this.label5.Text = "Fax";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(8, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 59;
            this.label4.Text = "Work #";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(8, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 58;
            this.label3.Text = "Short Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 16);
            this.label2.TabIndex = 57;
            this.label2.Text = "Name";
            // 
            // label33
            // 
            this.label33.ForeColor = System.Drawing.Color.Red;
            this.label33.Location = new System.Drawing.Point(80, 38);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(8, 16);
            this.label33.TabIndex = 88;
            this.label33.Text = "*";
            // 
            // label34
            // 
            this.label34.ForeColor = System.Drawing.Color.Red;
            this.label34.Location = new System.Drawing.Point(56, 212);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(8, 16);
            this.label34.TabIndex = 89;
            this.label34.Text = "*";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(116, 638);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btmCVDetailClose
            // 
            this.btmCVDetailClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btmCVDetailClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btmCVDetailClose.BackgroundImage")));
            this.btmCVDetailClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btmCVDetailClose.Location = new System.Drawing.Point(196, 638);
            this.btmCVDetailClose.Name = "btmCVDetailClose";
            this.btmCVDetailClose.Size = new System.Drawing.Size(75, 23);
            this.btmCVDetailClose.TabIndex = 31;
            this.btmCVDetailClose.UseVisualStyleBackColor = false;
            this.btmCVDetailClose.Click += new System.EventHandler(this.btmCVDetailClose_Click);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.uctcounterPartyVenueVenues);
            this.ultraTabPageControl2.Controls.Add(this.btnVenuesClose);
            this.ultraTabPageControl2.Controls.Add(this.btnVenuesSave);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(388, 668);
            this.ultraTabPageControl2.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl2_Paint);
            // 
            // uctcounterPartyVenueVenues
            // 
            this.uctcounterPartyVenueVenues.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctcounterPartyVenueVenues.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.uctcounterPartyVenueVenues.Location = new System.Drawing.Point(54, 10);
            this.uctcounterPartyVenueVenues.Name = "uctcounterPartyVenueVenues";
            this.uctcounterPartyVenueVenues.Size = new System.Drawing.Size(300, 125);
            this.uctcounterPartyVenueVenues.TabIndex = 2;
            // 
            // btnVenuesClose
            // 
            this.btnVenuesClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVenuesClose.BackgroundImage")));
            this.btnVenuesClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVenuesClose.Location = new System.Drawing.Point(196, 564);
            this.btnVenuesClose.Name = "btnVenuesClose";
            this.btnVenuesClose.Size = new System.Drawing.Size(75, 23);
            this.btnVenuesClose.TabIndex = 1;
            this.btnVenuesClose.Click += new System.EventHandler(this.btnVenuesClose_Click);
            // 
            // btnVenuesSave
            // 
            this.btnVenuesSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVenuesSave.BackgroundImage")));
            this.btnVenuesSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVenuesSave.Location = new System.Drawing.Point(116, 564);
            this.btnVenuesSave.Name = "btnVenuesSave";
            this.btnVenuesSave.Size = new System.Drawing.Size(75, 23);
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
            this.ultraTabPageControl3.Size = new System.Drawing.Size(388, 668);
            // 
            // tabCounterPartyVenueTabs
            // 
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance37.BackColor2 = System.Drawing.Color.White;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabCounterPartyVenueTabs.ActiveTabAppearance = appearance37;
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
            ultraTab8.Key = "tabCounterPartyVenueDetails";
            ultraTab8.TabPage = this.ultraTabPageControl5;
            ultraTab8.Text = "Details";
            ultraTab9.Key = "tabFix";
            ultraTab9.TabPage = this.ultraTabPageControl6;
            ultraTab9.Text = "Fix";
            ultraTab10.Key = "tabMappings";
            ultraTab10.TabPage = this.ultraTabPageControl7;
            ultraTab10.Text = "Mappings";
            ultraTab11.Key = "tabCompliance";
            ultraTab11.TabPage = this.ultraTabPageControl8;
            ultraTab11.Text = " Compliance";
            ultraTab11.Visible = false;
            this.tabCounterPartyVenueTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab8,
            ultraTab9,
            ultraTab10,
            ultraTab11});
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
            this.btnCounterPartyVenueClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCounterPartyVenueClose.Location = new System.Drawing.Point(196, 570);
            this.btnCounterPartyVenueClose.Name = "btnCounterPartyVenueClose";
            this.btnCounterPartyVenueClose.Size = new System.Drawing.Size(75, 23);
            this.btnCounterPartyVenueClose.TabIndex = 1;
            this.btnCounterPartyVenueClose.Click += new System.EventHandler(this.btnCounterPartyVenueClose_Click);
            // 
            // btnCVDetailSave
            // 
            this.btnCVDetailSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCVDetailSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCVDetailSave.BackgroundImage")));
            this.btnCVDetailSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCVDetailSave.Location = new System.Drawing.Point(116, 570);
            this.btnCVDetailSave.Name = "btnCVDetailSave";
            this.btnCVDetailSave.Size = new System.Drawing.Size(75, 23);
            this.btnCVDetailSave.TabIndex = 0;
            this.btnCVDetailSave.Click += new System.EventHandler(this.btnCVDetailSave_Click);
            // 
            // stbCounterParty
            // 
            this.stbCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.stbCounterParty.Location = new System.Drawing.Point(0, 711);
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
            this.trvCounterPartyVenue.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvCounterPartyVenue.FullRowSelect = true;
            this.trvCounterPartyVenue.HideSelection = false;
            this.trvCounterPartyVenue.HotTracking = true;
            this.trvCounterPartyVenue.Location = new System.Drawing.Point(2, 2);
            this.trvCounterPartyVenue.Name = "trvCounterPartyVenue";
            this.trvCounterPartyVenue.ShowLines = false;
            this.trvCounterPartyVenue.Size = new System.Drawing.Size(165, 665);
            this.trvCounterPartyVenue.TabIndex = 1;
            this.trvCounterPartyVenue.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvCounterPartyVenue_AfterSelect);
            // 
            // tbcCounterPartyVenue
            // 
            appearance38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tbcCounterPartyVenue.ActiveTabAppearance = appearance38;
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
            this.tbcCounterPartyVenue.Size = new System.Drawing.Size(390, 689);
            this.tbcCounterPartyVenue.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcCounterPartyVenue.TabIndex = 6;
            ultraTab1.Key = "CounterPartyTab";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Broker";
            ultraTab2.Key = "tabVenues";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Venues";
            ultraTab3.Key = "tabCounterPartyVenue";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Broker Venue";
            this.tbcCounterPartyVenue.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.tbcCounterPartyVenue.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcCounterPartyVenue_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(388, 668);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Location = new System.Drawing.Point(0, 671);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Location = new System.Drawing.Point(78, 671);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(135, 36);
            this.textBox2.MaxLength = 50;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(150, 21);
            this.textBox2.TabIndex = 99;
            // 
            // labelPCLastName
            // 
            this.labelPCLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelPCLastName.Location = new System.Drawing.Point(7, 38);
            this.labelPCLastName.Name = "labelPCLastName";
            this.labelPCLastName.Size = new System.Drawing.Size(65, 16);
            this.labelPCLastName.TabIndex = 96;
            this.labelPCLastName.Text = "Last Name";
            // 
            // textPCLastName
            // 
            this.textPCLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPCLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textPCLastName.Location = new System.Drawing.Point(135, 36);
            this.textPCLastName.MaxLength = 50;
            this.textPCLastName.Name = "textPCLastName";
            this.textPCLastName.Size = new System.Drawing.Size(150, 21);
            this.textPCLastName.TabIndex = 97;
            // 
            // CounterPartyVenueMaster
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(558, 713);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbcCounterPartyVenue);
            this.Controls.Add(this.trvCounterPartyVenue);
            this.Controls.Add(this.stbCounterParty);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(566, 700);
            this.Name = "CounterPartyVenueMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Broker Venue";
            this.Load += new System.EventHandler(this.CounterPartyVenueMaster_Load);
            this.ultraTabPageControl5.ResumeLayout(false);
            this.ultraTabPageControl6.ResumeLayout(false);
            this.ultraTabPageControl7.ResumeLayout(false);
            this.ultraTabPageControl8.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsAlgoBroker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOTDorEMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCounterPartyVenueTabs)).EndInit();
            this.tabCounterPartyVenueTabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcCounterPartyVenue)).EndInit();
            this.tbcCounterPartyVenue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        private void CounterPartyVenueMaster_Load(object sender, EventArgs e)
        {
            try
            {
                BindCounterPartyType();
                BindCountries();
                BindDataGrid();
                BindCounterPartyVenueMasterTree();
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("CounterPartyVenueMaster_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "CounterPartyVenueMaster_Load", null);


                #endregion LogEntry
            }
        }

        private void BindDataGrid()
        {
        }

        private void BindCounterPartyVenueMasterTree()
        {
            try
            {
                trvCounterPartyVenue.Nodes.Clear();

                Font font = new Font("Tahoma", 11, FontStyle.Bold, GraphicsUnit.Pixel);

                TreeNode treeNodeCounterPartyRoot = new TreeNode("Broker");

                treeNodeCounterPartyRoot.NodeFont = font;

                NodeDetails counterPartyNode = new NodeDetails(NodeType.CounterParty, int.MinValue);
                treeNodeCounterPartyRoot.Tag = counterPartyNode;

                CounterParties counterParties = CounterPartyManager.GetCounterParties();

                foreach (CounterParty counterParty in counterParties)
                {
                    TreeNode treeNodeCounterParty = new TreeNode(counterParty.ShortName);
                    counterPartyNode = new NodeDetails(NodeType.CounterParty, counterParty.CounterPartyID);
                    treeNodeCounterParty.Tag = counterPartyNode;

                    treeNodeCounterPartyRoot.Nodes.Add(treeNodeCounterParty);
                }

                trvCounterPartyVenue.Nodes.Add(treeNodeCounterPartyRoot);

                TreeNode treeNodeVenueRoot = new TreeNode("Venue");
                treeNodeVenueRoot.NodeFont = font;
                NodeDetails venueNode = new NodeDetails(NodeType.Venue, "VenueMasterRoot", int.MinValue, int.MinValue, int.MinValue);
                treeNodeVenueRoot.Tag = venueNode;

                TreeNode treeNodeVenueExchangeRoot = new TreeNode("Exchanges");
                treeNodeVenueExchangeRoot.NodeFont = font;
                NodeDetails venueExchangeNode = new NodeDetails(NodeType.Venue, int.MinValue, int.MinValue, int.MinValue);
                treeNodeVenueExchangeRoot.Tag = venueExchangeNode;

                treeNodeVenueRoot.Nodes.Add(treeNodeVenueExchangeRoot);

                Exchanges exchanges = ExchangeManager.GetAUECCommonExchanges();
                foreach (Exchange exchange in exchanges)
                {
                    TreeNode treeNodeExchange = new TreeNode(exchange.DisplayName);
                    venueNode = new NodeDetails(NodeType.Venue, exchange.ExchangeID, int.MinValue, exchange.ExchangeID);
                    treeNodeExchange.Tag = venueNode;
                    treeNodeVenueExchangeRoot.Nodes.Add(treeNodeExchange);
                }

                VenueTypes venueTypes = VenueManager.GetVenueTypes();
                TreeNode treeNodeVenueType = new TreeNode("");
                Venues venues = VenueManager.GetVenues();
                foreach (BLL.VenueType venueType in venueTypes)
                {
                    treeNodeVenueType = new TreeNode(venueType.Type);
                    treeNodeVenueType.NodeFont = font;
                    NodeDetails venueTypeNodeDetails = new NodeDetails(NodeType.Venue, venueType.Type, int.MinValue, venueType.VenueTypeID, int.MinValue);
                    treeNodeVenueType.Tag = venueTypeNodeDetails;

                    foreach (Venue venue in venues)
                    {
                        if (venue.ExchangeID <= 0)
                        {
                            TreeNode treeNodeVenue = new TreeNode(venue.VenueName);

                            if (venue.VenueTypeID == venueType.VenueTypeID)
                            {
                                venueNode = new NodeDetails(NodeType.Venue, venueType.Type, venue.VenueID, venue.VenueTypeID, venue.ExchangeID);
                                treeNodeVenue.Tag = venueNode;
                                treeNodeVenueType.Nodes.Add(treeNodeVenue);
                            }
                        }
                    }

                    treeNodeVenueRoot.Nodes.Add(treeNodeVenueType);
                }

                trvCounterPartyVenue.Nodes.Add(treeNodeVenueRoot);

                TreeNode treeNodeCounterPartyVenueRoot = new TreeNode("Broker-Venue");
                treeNodeCounterPartyVenueRoot.NodeFont = font;

                NodeDetails counterPartyVenueNode = new NodeDetails(int.MinValue, NodeType.CounterPartyVenue, -22);
                treeNodeCounterPartyVenueRoot.Tag = counterPartyVenueNode;

                CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCounterPartyVenues();
                if (cvSave == false)
                {
                    if (frmaddCounterPartyVenue != null)
                    {
                        int location = 1;
                        CounterPartyVenue newCounterPartyVenue = new CounterPartyVenue();
                        if (frmaddCounterPartyVenue.SavedCounterPartyVenueID == 1)
                        {
                            newCounterPartyVenue.CounterPartyID = frmaddCounterPartyVenue.CounterPartyID;
                            newCounterPartyVenue.VenueID = frmaddCounterPartyVenue.VenueID;

                            int index = 1;
                            foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
                            {
                                if (counterPartyVenue.CounterPartyID == frmaddCounterPartyVenue.CounterPartyID)
                                {
                                    location = index;
                                }
                                index++;
                            }
                            if (location == 1)
                            {
                                counterPartyVenues.Add(newCounterPartyVenue);
                            }
                            else
                            {
                                counterPartyVenues.Insert(location, newCounterPartyVenue);
                            }
                        }
                    }
                }

                string prevCounterPartyName = "debu";

                TreeNode treeNodeCP = null;
                if (counterPartyVenues.Count > 0)
                {
                    foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
                    {
                        if (prevCounterPartyName != counterPartyVenue.CounterPartyName)
                        {
                            if (treeNodeCP != null)
                            {
                                treeNodeCounterPartyVenueRoot.Nodes.Add(treeNodeCP);
                            }
                            prevCounterPartyName = counterPartyVenue.CounterPartyName;
                            treeNodeCP = new TreeNode(counterPartyVenue.CounterPartyName);
                            treeNodeCP.NodeFont = font;
                            NodeDetails nodeCP = new NodeDetails(-1, NodeType.CounterPartyVenue, counterPartyVenue.CounterPartyID);
                            treeNodeCP.Tag = nodeCP;
                            TreeNode treeNodeCPV = new TreeNode(counterPartyVenue.VenueName);
                            NodeDetails nodeCPV = new NodeDetails(NodeType.CounterPartyVenue, counterPartyVenue.CounterPartyVenueID);
                            treeNodeCPV.Tag = nodeCPV;
                            treeNodeCP.Nodes.Add(treeNodeCPV);
                        }
                        else
                        {
                            TreeNode treeNodeCPV = new TreeNode(counterPartyVenue.VenueName);
                            NodeDetails nodeCPV = new NodeDetails(NodeType.CounterPartyVenue, counterPartyVenue.CounterPartyVenueID);
                            treeNodeCPV.Tag = nodeCPV;
                            treeNodeCP.Nodes.Add(treeNodeCPV);
                        }
                    }

                    if (treeNodeCP != null)
                    {
                        treeNodeCounterPartyVenueRoot.Nodes.Add(treeNodeCP);
                    }

                    trvCounterPartyVenue.Nodes.Add(treeNodeCounterPartyVenueRoot);
                }
                else
                {
                    trvCounterPartyVenue.Nodes.Add(treeNodeCounterPartyVenueRoot);
                }

                trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TAB_COUNTERPARTY];
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("BindCounterPartyVenueMasterTree",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "BindCounterPartyVenueMasterTree", null);


                #endregion LogEntry
            }
        }

        private void BindCounterPartyType()
        {
            CounterPartyTypes counterPartyTypes = CounterPartyManager.GetCounterPartyTypes();
            counterPartyTypes.Insert(0, new BLL.CounterPartyType(int.MinValue, C_COMBO_SELECT));
            cmbCounterPartyType.DataSource = null;
            cmbCounterPartyType.DataSource = counterPartyTypes;
            cmbCounterPartyType.DisplayMember = "Type";
            cmbCounterPartyType.ValueMember = "CounterPartyTypeID";
            cmbCounterPartyType.Text = C_COMBO_SELECT;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCounterPartyType.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("Type"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            cmbCounterPartyType.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        private void BindCountries()
        {
            Countries countries = GeneralManager.GetCountries();

            countries.Insert(0, new Country(int.MinValue, C_COMBO_SELECT));
            cmbCountry.DisplayMember = "Name";
            cmbCountry.ValueMember = "CountryID";
            cmbCountry.DataSource = null;
            cmbCountry.DataSource = countries;
            cmbCountry.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCountry.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("Name"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            cmbCountry.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        private void BindStates()
        {
            States states = GeneralManager.GetStates();

            states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("StateName"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        private void BindEmptyStates()
        {
            States states = new States();

            states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("StateName"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCounterPartyDetails();
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion LogEntry
            }
        }

        private int SaveCounterPartyDetails()
        {
            int result = int.MinValue;

            string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex emailRegex = new Regex(emailCheck);
            Match emailMatch = emailRegex.Match(txtEmail1.Text);

            errorProvider1.SetError(txtFullName, "");
            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(txtPhone, "");
            errorProvider1.SetError(txtContactName1, "");
            errorProvider1.SetError(txtEmail1, "");
            errorProvider1.SetError(cmbCounterPartyType, "");

            errorProvider1.SetError(txtAddress1, "");
            errorProvider1.SetError(cmbCountry, "");
            errorProvider1.SetError(cmbState, "");
            errorProvider1.SetError(txtPCWorkTel, "");
            errorProvider1.SetError(txtPCCell, "");
            errorProvider1.SetError(txtCity, "");
            if (txtFullName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtFullName, "Please enter Full name!");
                txtFullName.Focus();
            }
            else if (txtShortName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtShortName, "Please enter Short name!");
                txtShortName.Focus();
            }
            else if (int.Parse(cmbCounterPartyType.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCounterPartyType, "Please select Broker type!");
                cmbCounterPartyType.Focus();
            }
            else if (txtAddress1.Text.Trim() == "")
            {
                errorProvider1.SetError(txtAddress1, "Please enter Address !");
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
                errorProvider1.SetError(txtCity, "Please enter City!");
                txtCity.Focus();
            }
            else if (txtPhone.Text.Trim() == "")
            {
                errorProvider1.SetError(txtPhone, "Please enter Telephone #!");
                txtPhone.Focus();
            }
            else if (txtContactName1.Text.Trim() == "")
            {
                errorProvider1.SetError(txtContactName1, "Please enter Contact Name 1!");
                txtContactName1.Focus();
            }
            else if (!emailMatch.Success)
            {
                errorProvider1.SetError(txtEmail1, "Please enter valid Email address!");
                txtEmail1.Focus();
            }
            else if (txtPCWorkTel.Text.Trim() == "")
            {
                errorProvider1.SetError(txtPCWorkTel, "Please enter Work Telephone #!");
                txtPCWorkTel.Focus();
            }
            else if (txtPCCell.Text.Trim() == "")
            {
                errorProvider1.SetError(txtPCCell, "Please enter Cell #!");
                txtPCCell.Focus();
            }
            else
            {
                NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;

                CounterParty counterParty = new CounterParty();
                counterParty.CounterPartyID = ((NodeDetails)trvCounterPartyVenue.SelectedNode.Tag).NodeID;
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

                counterParty.Address2 = txtAddress2.Text.Trim();
                counterParty.CountryID = int.Parse(cmbCountry.Value.ToString());
                counterParty.StateID = int.Parse(cmbState.Value.ToString());
                counterParty.Zip = txtZip.Text.Trim();
                counterParty.ContactName1LastName = txtPCLastName.Text.Trim();
                counterParty.ContactName1WorkPhone = txtPCWorkTel.Text.Trim();
                counterParty.ContactName1CellPhone = txtPCCell.Text.Trim();
                counterParty.ContactName2LastName = txtSCLastName.Text.Trim();
                counterParty.ContactName2WorkPhone = txtSCWorkTel.Text.Trim();
                counterParty.ContactName2CellPhone = txtSCCell.Text.Trim();

                counterParty.City = txtCity.Text.Trim();
                counterParty.IsAlgoBroker = chkIsAlgoBroker.Checked;
                counterParty.IsOTDorEMS = chkIsOTDorEMS.Checked;
                int newcounterPartyID = CounterPartyManager.SaveCounterParty(counterParty);
                if (newcounterPartyID == -1)
                {
                    MessageBox.Show("Broker already exists.", "Prana Alert", MessageBoxButtons.OK);
                }
                else
                {
                    if (newcounterPartyID > 0)
                    {
                        MessageBox.Show("Record saved.", "Prana Alert", MessageBoxButtons.OK);
                    }

                    BindCounterPartyVenueMasterTree();

                    NodeDetails selectNodeDetails = new NodeDetails(NodeType.CounterParty, newcounterPartyID);
                    SelectTreeNode(selectNodeDetails);
                }
                result = newcounterPartyID;
            }

            return result;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvCounterPartyVenue.SelectedNode.Nodes.Count == 0 && trvCounterPartyVenue.SelectedNode.Parent != null)
                {
                    bool result = false;
                    if (trvCounterPartyVenue.SelectedNode == null)
                    {
                    }
                    else
                    {
                        NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
                        NodeDetails prevNodeDetails = new NodeDetails();
                        if (nodeDetails.Type == NodeType.CounterPartyVenue)
                        {
                            if (trvCounterPartyVenue.SelectedNode.PrevNode != null)
                            {
                                prevNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.PrevNode.Tag;
                            }
                            else
                            {
                                prevNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Parent.Parent.Tag;
                            }
                        }
                        else
                        {
                            if (trvCounterPartyVenue.SelectedNode.PrevNode != null)
                            {
                                prevNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.PrevNode.Tag;
                            }
                            else
                            {
                                prevNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Parent.Tag;
                            }
                        }
                        switch (nodeDetails.Type)
                        {
                            case NodeType.CounterParty:

                                int counterPartyID = nodeDetails.NodeID;

                                if (MessageBox.Show(this, "Do you want to delete selected Broker?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (!(CounterPartyManager.DeleteCounterParty(counterPartyID)))
                                    {
                                        MessageBox.Show(this, "Broker is Referenced in Client Boker Venue, can not be deleted.", "Prana Alert", MessageBoxButtons.OK);
                                    }
                                    else
                                    {
                                        BindCounterPartyVenueMasterTree();
                                        SelectTreeNode(prevNodeDetails);
                                    }
                                }
                                break;

                            case NodeType.Venue:
                                int venueID = nodeDetails.NodeID;
                                prevNodeDetails.VenueTypeID = nodeDetails.VenueTypeID;

                                if (nodeDetails.VenueTypeID == int.MinValue)
                                {
                                    MessageBox.Show("You can not delete the selected Vanue.", "Nievane Alert", MessageBoxButtons.OK);
                                }
                                else if (nodeDetails.VenueTypeID >= VENUE_EXCHANGES)
                                {
                                    if (MessageBox.Show(this, "Do you want to delete selected Venue?", "Nirvala Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        if (!(VenueManager.DeleteVenue(venueID)))
                                        {
                                            MessageBox.Show(this, "Venue is Referenced in Client Broker Venue, can not be delete.", "Prana Alert", MessageBoxButtons.OK);
                                        }
                                        else
                                        {
                                            BindCounterPartyVenueMasterTree();
                                            SelectTreeNode(prevNodeDetails);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, "Venue is Referenced in Client Broker Venue,can not be deleted.", "Prana Alert");
                                }
                                break;

                            case NodeType.CounterPartyVenue:
                                int nodeID = nodeDetails.NodeID;

                                if (MessageBox.Show(this, "Do you want to delete selected BrokerVenue?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (nodeID > 0)
                                    {
                                        result = CounterPartyManager.DeleteCounterPartyVenue(nodeID);
                                        if (result == false)
                                        {
                                            MessageBox.Show(this, "BrokerVenue is Referenced in Client Broker Venue,can not be deleted.", "Prana Alert", MessageBoxButtons.OK);
                                        }
                                        else
                                        {
                                            BindCounterPartyVenueMasterTree();
                                            SelectTreeNode(prevNodeDetails);
                                        }
                                    }
                                    else
                                    {
                                        frmaddCounterPartyVenue = null;
                                        BindCounterPartyVenueMasterTree();
                                        SelectTreeNode(prevNodeDetails);
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                }
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnDelete_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnDelete_Click", null);


                #endregion LogEntry
            }
        }

        private AddCounterPartyVenue frmaddCounterPartyVenue = null;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvCounterPartyVenue.SelectedNode == null)
                {
                }
                else
                {
                    NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
                    switch (nodeDetails.Type)
                    {
                        case NodeType.CounterParty:
                            stbCounterParty.Text = "Enter Broker details.";
                            RefreshCounterPartyForm();

                            tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY];

                            if (nodeDetails.NodeID != int.MinValue)
                            {
                                trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.SelectedNode.Parent;
                            }
                            break;

                        case NodeType.Venue:
                            stbCounterParty.Text = "Enter Venue Details.";
                            uctcounterPartyVenueVenues.VenueRefresh();
                            tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_VENUE];
                            tabCounterPartyVenueTabs.Show();

                            if (nodeDetails.NodeID != int.MinValue)
                            {
                                trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.SelectedNode.Parent;
                            }
                            break;

                        case NodeType.CounterPartyVenue:
                            int counterPartyID = int.MinValue;
                            if (nodeDetails.NodeID == int.MinValue)
                            {
                            }
                            else
                            {
                                TreeNode parentToParentNode = trvCounterPartyVenue.SelectedNode.Parent.Parent;
                                if (parentToParentNode != null)
                                {
                                    NodeDetails counterPartyNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Parent.Tag;
                                    counterPartyID = counterPartyNodeDetails.NodeID;
                                }
                                else
                                {
                                    TreeNode parentNode = trvCounterPartyVenue.SelectedNode.Parent;
                                    if (parentNode != null)
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
                            stbCounterParty.Text = "Enter Broker Venue Details.";
                            uctCounterPartyVenueDetails.Refresh();
                            tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY_VENUE];

                            if (frmaddCounterPartyVenue == null)
                            {
                                frmaddCounterPartyVenue = new AddCounterPartyVenue();
                            }

                            if (nodeDetails.NodeID != int.MinValue)
                            {
                                trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.SelectedNode.Parent;
                            }
                            frmaddCounterPartyVenue.CounterPartyID = counterPartyID;
                            frmaddCounterPartyVenue.ShowDialog(this);

                            if (frmaddCounterPartyVenue.SavedCounterPartyVenueID == 1)
                            {
                                cvSave = false;
                                BindCounterPartyVenueMasterTree();

                                NodeDetails selectedNodeDetails = new NodeDetails(int.MinValue, NodeType.CounterPartyVenue, frmaddCounterPartyVenue.CounterPartyID);

                                SelectTreeNode(selectedNodeDetails);
                            }
                            break;
                    }
                }
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnAdd_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnAdd_Click", null);


                #endregion LogEntry
            }
        }

        private void RefreshCounterPartyForm()
        {
            txtFullName.Text = "";

            txtAddress1.Text = "";
            txtContactName1.Text = "";
            txtContactName2.Text = "";

            txtEmail1.Text = "";
            txtEmail2.Text = "";
            txtFax.Text = "";
            txtPhone.Text = "";
            txtShortName.Text = "";
            txtTitle1.Text = "";
            txtTitle2.Text = "";
            txtCity.Text = "";
        }

        private void trvCounterPartyVenue_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;

                if (trvCounterPartyVenue.SelectedNode == null)
                {
                }
                else
                {
                    switch (nodeDetails.Type)
                    {
                        case NodeType.CounterParty:

                            if (chkAddCVMaster == false)
                            {
                                if (nodeDetails.NodeID == int.MinValue)
                                {
                                    tbcCounterPartyVenue.Tabs["CounterPartyTab"].Enabled = false;
                                }
                                else
                                {
                                    tbcCounterPartyVenue.Tabs["CounterPartyTab"].Enabled = true;
                                }
                            }

                            tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY];
                            int counterPartyID = nodeDetails.NodeID;
                            CounterParty counterParty = CounterPartyManager.GetCounterParty(counterPartyID);

                            CounterPartyDetails(counterParty);

                            break;

                        case NodeType.Venue:

                            if (chkAddCVMaster == false)
                            {
                                if (nodeDetails.NodeID == int.MinValue)
                                {
                                    tbcCounterPartyVenue.Tabs["tabVenues"].Enabled = false;
                                }
                                else
                                {
                                    tbcCounterPartyVenue.Tabs["tabVenues"].Enabled = true;
                                }
                            }

                            tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_VENUE];
                            int exchangeTypeID = nodeDetails.VenueTypeID;
                            string exchangeType = nodeDetails.Type.ToString();
                            string venueType = nodeDetails.VenueType;
                            int exchangeID = nodeDetails.ExchangeID;

                            int venueID = nodeDetails.NodeID;

                            uctcounterPartyVenueVenues.SetupControl(exchangeTypeID, venueType, exchangeID, venueID);

                            Venue venue = new Venue();

                            if (nodeDetails.VenueType != "")
                            {
                                venue = VenueManager.GetVenue(venueID);
                            }
                            else
                            {
                                exchangeID = venueID;
                                if (exchangeID != int.MinValue)
                                {
                                    venue = VenueManager.GetVenueByExchangeID(exchangeID);
                                }
                            }
                            uctcounterPartyVenueVenues.SetVenueDetails(venue);
                            break;

                        case NodeType.CounterPartyVenue:

                            if ((nodeDetails.CounterPartyVenueID != -1) && (nodeDetails.CounterPartyID != -22))
                            {
                                int counterPartyVenueID = nodeDetails.NodeID;

                                tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_COUNTERPARTY_VENUE];
                                tbcCounterPartyVenue.SelectedTab = tbcCounterPartyVenue.Tabs[C_TREE_COUNTERPARTY_VENUE];

                                tabCounterPartyVenueTabs.Enabled = true;

                                CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCounterPartyVenue(counterPartyVenueID);

                                uctCounterPartyVenueDetails.SetupControl(counterPartyVenue);

                                uctCounterPartyVenueDetails.GetSelectedCounterPartyVenueID(counterPartyVenue.CounterPartyVenueID);
                                if (frmaddCounterPartyVenue != null)
                                {
                                    if (frmaddCounterPartyVenue.SavedCounterPartyVenueID == 1)
                                    {
                                        uctCounterPartyVenueDetails.CounterPartyID = frmaddCounterPartyVenue.CounterPartyID;
                                        uctCounterPartyVenueDetails.VenueID = frmaddCounterPartyVenue.VenueID;
                                    }
                                }
                                else
                                {
                                    uctCounterPartyVenueDetails.CounterPartyID = counterPartyVenue.CounterPartyID;
                                    uctCounterPartyVenueDetails.VenueID = counterPartyVenue.VenueID;
                                }

                                uctSymbolMapping.SetupControl(counterPartyVenueID);
                            }
                            else
                            {
                                int counterPartyVenueID = nodeDetails.NodeID;
                                CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCounterPartyVenue(counterPartyVenueID);

                                uctCounterPartyVenueDetails.SetupControl(counterPartyVenue);

                                uctCounterPartyVenueDetails.GetSelectedCounterPartyVenueID(counterPartyVenue.CounterPartyVenueID);

                                uctFix.SetupControl(counterPartyVenueID);
                                tabCounterPartyVenueTabs.Enabled = false;
                            }
                            break;
                    }

                    if (chkAddCVMaster == false && nodeDetails.NodeID == int.MinValue && nodeDetails.Type != NodeType.CounterPartyVenue)
                    {
                    }
                    else
                    {
                        tbcCounterPartyVenue.Enabled = true;
                    }
                }
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("trvCounterPartyVenue_AfterSelect",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "trvCounterPartyVenue_AfterSelect", null);


                #endregion LogEntry
            }
        }

        private void CounterPartyDetails(CounterParty counterParty)
        {
            if (counterParty != null)
            {
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
                cmbCounterPartyType.Value = int.Parse(counterParty.CounterPartyTypeID.ToString());

                txtAddress2.Text = counterParty.Address2;
                cmbCountry.Value = int.Parse(counterParty.CountryID.ToString());
                cmbState.Value = int.Parse(counterParty.StateID.ToString());
                txtZip.Text = counterParty.Zip;
                txtPCLastName.Text = counterParty.ContactName1LastName;
                txtPCWorkTel.Text = counterParty.ContactName1WorkPhone;
                txtPCCell.Text = counterParty.ContactName1CellPhone;
                txtSCLastName.Text = counterParty.ContactName2LastName;
                txtSCWorkTel.Text = counterParty.ContactName2WorkPhone;
                txtSCCell.Text = counterParty.ContactName2CellPhone;

                txtCity.Text = counterParty.City;
                chkIsAlgoBroker.Checked = counterParty.IsAlgoBroker;
                chkIsOTDorEMS.Checked = counterParty.IsOTDorEMS;
            }
            else
            {
            }
        }

        private bool cvSave = false;

        private void btnCVDetailSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvCounterPartyVenue.SelectedNode == null)
                {
                    stbCounterParty.Text = "Please select any Broker Venue to be saved";
                }
                else
                {
                    NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
                    int counterPartyVenueID = int.MinValue;
                    if (tabCounterPartyVenueTabs.Tabs[0].Selected == true || tabCounterPartyVenueTabs.Tabs[1].Selected == true)
                    {
                        uctCounterPartyVenueDetails.ParentStatusBar = stbCounterParty;
                        CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
                        counterPartyVenue.CounterPartyVenueID = nodeDetails.NodeID;

                        int counterPartyID = int.MinValue;
                        if (trvCounterPartyVenue.SelectedNode.Parent != null)
                        {
                            NodeDetails parentNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Parent.Tag;
                            counterPartyID = parentNodeDetails.NodeID;
                        }

                        counterPartyVenueID = uctCounterPartyVenueDetails.GetCounterPartyVenueDetailsForSave(counterPartyVenue);

                        if (counterPartyVenueID > 0)
                        {
                            MessageBox.Show(this, "Broker Venue information saved !", "Prana Alert", MessageBoxButtons.OK);
                        }
                        else if (counterPartyVenueID == -2)
                        {
                            MessageBox.Show(this, "Counter Party - Venue - AUEC is referenced in Client Setup\n  First delete the references!", "Prana Alert", MessageBoxButtons.OK);
                        }
                        else if (counterPartyVenueID == -1)
                        {
                            MessageBox.Show(this, "CounterPartyVenue with same Display Name already exists!", "Prana Alert", MessageBoxButtons.OK);
                        }
                        else
                        {
                            tabCounterPartyVenueTabs.SelectedTab = tabCounterPartyVenueTabs.Tabs[C_TAB_COUNTERPARTYVENUEDETAIL];
                        }
                    }
                    else
                    {
                    }
                    if (frmaddCounterPartyVenue != null)
                    {
                        if (counterPartyVenueID != int.MinValue && frmaddCounterPartyVenue.SavedCounterPartyVenueID == 1)
                        {
                            cvSave = true;
                            BindCounterPartyVenueMasterTree();
                            NodeDetails selectnodeDetails = new NodeDetails(counterPartyVenueID, NodeType.CounterPartyVenue, frmaddCounterPartyVenue.CounterPartyID);
                            SelectTreeNode(selectnodeDetails);
                        }
                    }
                }
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnCounterPartyVenueSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnCounterPartyVenueSave_Click", null);


                #endregion LogEntry
            }
        }

        private void SaveCompliance()
        {
            NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;

            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
            counterPartyVenue.CounterPartyVenueID = nodeDetails.NodeID;

            int counterPartyID = int.MinValue;
            if (trvCounterPartyVenue.SelectedNode.Parent != null)
            {
                NodeDetails parentNodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Parent.Tag;
                counterPartyID = parentNodeDetails.NodeID;
            }

            if (true)
            {
                CounterPartyManager.SaveCVAUECCompliance(counterPartyVenue);

                SymbolMappings symbolMappings = uctSymbolMapping.CurrentSymbolMappings;

                int resultSymbolMapping = CounterPartyManager.SaveCVSymbolMapping(symbolMappings);
                if (resultSymbolMapping == -1)
                {
                }
                else
                {
                    uctSymbolMapping.CounterPartyVenueID = nodeDetails.NodeID;
                    MessageBox.Show(this, "CV Symbol Mapping and Compliance information saved !", "Prana Alert", MessageBoxButtons.OK);
                }
            }

            tabCounterPartyVenueTabs.SelectedTab = tabCounterPartyVenueTabs.Tabs[C_TAB_ACCEPTEDORDERTYPES];
        }

        private void btnVenuesSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = int.MinValue;
                int venueTypeID = int.MinValue;
                uctcounterPartyVenueVenues.ParentStatusBar = stbCounterParty;

                NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
                Venue venue = new Venue();

                if (nodeDetails.VenueTypeID == 1)
                {
                    uctcounterPartyVenueVenues.ExchangeID = ((NodeDetails)trvCounterPartyVenue.SelectedNode.Tag).NodeID;
                }
                else
                {
                    uctcounterPartyVenueVenues.ExchangeID = int.MinValue;
                }

                result = uctcounterPartyVenueVenues.SaveVenues();

                if (result != int.MinValue)
                {
                    if (result == -1)
                    {
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

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnVenuesSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnVenuesSave_Click", null);


                #endregion LogEntry
            }
        }

        private void SelectTreeNode(NodeDetails nodeDetails)
        {
            int countIndex = int.MinValue;
            switch (nodeDetails.Type)
            {
                case NodeType.CounterParty:
                    foreach (TreeNode node in trvCounterPartyVenue.Nodes[C_TAB_COUNTERPARTY].Nodes)
                    {
                        if (((NodeDetails)node.Tag).NodeID == nodeDetails.NodeID)
                        {
                            trvCounterPartyVenue.SelectedNode = node;
                            break;
                        }
                    }
                    break;

                case NodeType.Venue:
                    int venueType = int.Parse(nodeDetails.VenueTypeID.ToString());

                    countIndex = -1;
                    foreach (TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_VENUE].Nodes)
                    {
                        countIndex++;
                        if (((NodeDetails)node.Tag).VenueTypeID == nodeDetails.VenueTypeID)
                        {
                            foreach (TreeNode venueNode in trvCounterPartyVenue.Nodes[C_TREE_VENUE].Nodes[countIndex].Nodes)
                            {
                                if (((NodeDetails)venueNode.Tag).NodeID == nodeDetails.NodeID)
                                {
                                    trvCounterPartyVenue.SelectedNode = venueNode;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case NodeType.CounterPartyVenue:
                    countIndex = -1;
                    foreach (TreeNode node in trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY_VENUE].Nodes)
                    {
                        countIndex++;

                        if (((NodeDetails)node.Tag).CounterPartyID == nodeDetails.CounterPartyID)
                        {
                            foreach (TreeNode cpvNode in trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY_VENUE].Nodes[countIndex].Nodes)
                            {
                                if (((NodeDetails)cpvNode.Tag).NodeID == nodeDetails.CounterPartyVenueID)
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

        private void btnVenuesClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCounterPartyVenueClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btmCVDetailClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CounterPartyDetailsTab_Click(object sender, EventArgs e)
        {
        }

        private void tbcCounterPartyVenue_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (trvCounterPartyVenue.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;

                    if ((nodeDetails.Type != NodeType.CounterParty) && (tbcCounterPartyVenue.SelectedTab == tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY]))
                    {
                        trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY];
                    }

                    if ((nodeDetails.Type != NodeType.Venue) && (tbcCounterPartyVenue.SelectedTab == tbcCounterPartyVenue.Tabs[C_TAB_VENUE]))
                    {
                        trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TREE_VENUE];
                    }

                    if ((nodeDetails.Type != NodeType.CounterPartyVenue) && (tbcCounterPartyVenue.SelectedTab == tbcCounterPartyVenue.Tabs[C_TAB_COUNTERPARTY_VENUE]))
                    {
                        trvCounterPartyVenue.SelectedNode = trvCounterPartyVenue.Nodes[C_TREE_COUNTERPARTY_VENUE];
                    }
                }
            }

            #region Catch

            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace;
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

            #endregion Catch

            finally
            {
                #region LogEntry

                Logger.LoggerWrite("tbcCounterPartyVenue_SelectedIndexChanged",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "tbcCounterPartyVenue_SelectedIndexChanged", null);


                #endregion LogEntry
            }
        }

        #region Controls Focus Color

        private void txtFullName_GotFocus(object sender, EventArgs e)
        {
            txtFullName.BackColor = Color.LemonChiffon;
        }

        private void txtFullName_LostFocus(object sender, EventArgs e)
        {
            txtFullName.BackColor = Color.White;
        }

        private void txtShortName_GotFocus(object sender, EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }

        private void txtShortName_LostFocus(object sender, EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }

        private void txtAddress1_GotFocus(object sender, EventArgs e)
        {
            txtAddress1.BackColor = Color.LemonChiffon;
        }

        private void txtAddress1_LostFocus(object sender, EventArgs e)
        {
            txtAddress1.BackColor = Color.White;
        }

        private void txtContactName1_GotFocus(object sender, EventArgs e)
        {
            txtContactName1.BackColor = Color.LemonChiffon;
        }

        private void txtContactName1_LostFocus(object sender, EventArgs e)
        {
            txtContactName1.BackColor = Color.White;
        }

        private void txtContactName2_GotFocus(object sender, EventArgs e)
        {
            txtContactName2.BackColor = Color.LemonChiffon;
        }

        private void txtContactName2_LostFocus(object sender, EventArgs e)
        {
            txtContactName2.BackColor = Color.White;
        }

        private void txtEmail1_GotFocus(object sender, EventArgs e)
        {
            txtEmail1.BackColor = Color.LemonChiffon;
        }

        private void txtEmail1_LostFocus(object sender, EventArgs e)
        {
            txtEmail1.BackColor = Color.White;
        }

        private void txtEmail2_GotFocus(object sender, EventArgs e)
        {
            txtEmail2.BackColor = Color.LemonChiffon;
        }

        private void txtEmail2_LostFocus(object sender, EventArgs e)
        {
            txtEmail2.BackColor = Color.White;
        }

        private void txtFax_GotFocus(object sender, EventArgs e)
        {
            txtFax.BackColor = Color.LemonChiffon;
        }

        private void txtFax_LostFocus(object sender, EventArgs e)
        {
            txtFax.BackColor = Color.White;
        }

        private void txtPhone_GotFocus(object sender, EventArgs e)
        {
            txtPhone.BackColor = Color.LemonChiffon;
        }

        private void txtPhone_LostFocus(object sender, EventArgs e)
        {
            txtPhone.BackColor = Color.White;
        }

        private void txtTitle1_GotFocus(object sender, EventArgs e)
        {
            txtTitle1.BackColor = Color.LemonChiffon;
        }

        private void txtTitle1_LostFocus(object sender, EventArgs e)
        {
            txtTitle1.BackColor = Color.White;
        }

        private void txtTitle2_GotFocus(object sender, EventArgs e)
        {
            txtTitle2.BackColor = Color.LemonChiffon;
        }

        private void txtTitle2_LostFocus(object sender, EventArgs e)
        {
            txtTitle2.BackColor = Color.White;
        }

        private void cmbCounterPartyType_GotFocus(object sender, EventArgs e)
        {
            cmbCounterPartyType.Appearance.BackColor = Color.LemonChiffon;
        }

        private void cmbCounterPartyType_LostFocus(object sender, EventArgs e)
        {
            cmbCounterPartyType.Appearance.BackColor = Color.White;
        }

        private void txtSCCell_GotFocus(object sender, EventArgs e)
        {
            txtSCCell.BackColor = Color.LemonChiffon;
        }

        private void txtSCCell_LostFocus(object sender, EventArgs e)
        {
            txtSCCell.BackColor = Color.White;
        }

        private void txtSCWorkTel_GotFocus(object sender, EventArgs e)
        {
            txtSCWorkTel.BackColor = Color.LemonChiffon;
        }

        private void txtSCWorkTel_LostFocus(object sender, EventArgs e)
        {
            txtSCWorkTel.BackColor = Color.White;
        }

        private void txtSCLastName_GotFocus(object sender, EventArgs e)
        {
            txtSCLastName.BackColor = Color.LemonChiffon;
        }

        private void txtSCLastName_LostFocus(object sender, EventArgs e)
        {
            txtSCLastName.BackColor = Color.White;
        }

        private void txtPCCell_GotFocus(object sender, EventArgs e)
        {
            txtPCCell.BackColor = Color.LemonChiffon;
        }

        private void txtPCCell_LostFocus(object sender, EventArgs e)
        {
            txtPCCell.BackColor = Color.White;
        }

        private void txtPCWorkTel_GotFocus(object sender, EventArgs e)
        {
            txtPCWorkTel.BackColor = Color.LemonChiffon;
        }

        private void txtPCWorkTel_LostFocus(object sender, EventArgs e)
        {
            txtPCWorkTel.BackColor = Color.White;
        }

        private void txtPCLastName_GotFocus(object sender, EventArgs e)
        {
            txtPCLastName.BackColor = Color.LemonChiffon;
        }

        private void txtPCLastName_LostFocus(object sender, EventArgs e)
        {
            txtPCLastName.BackColor = Color.White;
        }

        private void cmbState_GotFocus(object sender, EventArgs e)
        {
            cmbState.Appearance.BackColor = Color.LemonChiffon;
        }

        private void cmbState_LostFocus(object sender, EventArgs e)
        {
            cmbState.Appearance.BackColor = Color.White;
        }

        private void cmbCountry_GotFocus(object sender, EventArgs e)
        {
            cmbCountry.Appearance.BackColor = Color.LemonChiffon;
        }

        private void cmbCountry_LostFocus(object sender, EventArgs e)
        {
            cmbCountry.Appearance.BackColor = Color.White;
        }

        private void txtZip_GotFocus(object sender, EventArgs e)
        {
            txtZip.BackColor = Color.LemonChiffon;
        }

        private void txtZip_LostFocus(object sender, EventArgs e)
        {
            txtZip.BackColor = Color.White;
        }

        private void txtAddress2_GotFocus(object sender, EventArgs e)
        {
            txtAddress2.BackColor = Color.LemonChiffon;
        }

        private void txtAddress2_LostFocus(object sender, EventArgs e)
        {
            txtAddress2.BackColor = Color.White;
        }

        private void txtCity_GotFocus(object sender, EventArgs e)
        {
            txtCity.BackColor = Color.LemonChiffon;
        }

        private void txtCity_LostFocus(object sender, EventArgs e)
        {
            txtCity.BackColor = Color.White;
        }

        #endregion Controls Focus Color

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CounterPartyVenueSymbolMapping frmCounterPartyVenueSymbolMapping = new CounterPartyVenueSymbolMapping();
            frmCounterPartyVenueSymbolMapping.ShowDialog(this);
        }

        private void symbolMapping1_Load(object sender, EventArgs e)
        {
        }

        private void tbcCounterPartyVenue_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font f;
            Brush backBrush;
            Brush foreBrush;

            if (e.Index == tbcCounterPartyVenue.SelectedTab.Index)
            {
                f = new Font(e.Font, FontStyle.Regular);
                backBrush = new SolidBrush(Color.Brown);
                backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                foreBrush = Brushes.Black;
            }
            else
            {
                f = e.Font;
                backBrush = new SolidBrush(e.BackColor);
                foreBrush = new SolidBrush(e.ForeColor);
            }

            string tabName = tbcCounterPartyVenue.Tabs[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            Rectangle r = e.Bounds;
            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
            e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

            sf.Dispose();
            if (e.Index == tbcCounterPartyVenue.SelectedTab.Index)
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

        private void tabCounterPartyVenueTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font f;
            Brush backBrush;
            Brush foreBrush;

            if (e.Index == tabCounterPartyVenueTabs.SelectedTab.Index)
            {
                f = new Font(e.Font, FontStyle.Regular);
                backBrush = new SolidBrush(Color.Brown);
                backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                foreBrush = Brushes.Black;
            }
            else
            {
                f = e.Font;
                backBrush = new SolidBrush(e.BackColor);
                foreBrush = new SolidBrush(e.ForeColor);
            }

            string tabName = tabCounterPartyVenueTabs.Tabs[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            Rectangle r = e.Bounds;
            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
            e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

            sf.Dispose();
            if (e.Index == tabCounterPartyVenueTabs.SelectedTab.Index)
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

        private void tabRouteDetail_DrawItem(object sender, DrawItemEventArgs e)
        {
        }

        private void cmbTypeCounterParty_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabCounterPartyDetailTradingInfo_DrawItem(object sender, DrawItemEventArgs e)
        {
        }

        #endregion Highlight Selected Tab

        private void tabCounterPartyVenueTabs_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (tabCounterPartyVenueTabs.Tabs[3].Selected == true)
            {
                CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
                NodeDetails nodeDetails = (NodeDetails)trvCounterPartyVenue.SelectedNode.Tag;
                counterPartyVenue.CounterPartyVenueID = int.Parse(nodeDetails.NodeID.ToString());
            }
        }

        private void ultraTabPageControl8_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnVenuesSave_Click_1(object sender, EventArgs e)
        {
        }

        private void ultraTabPageControl2_Paint(object sender, PaintEventArgs e)
        {
        }

        #region NodeDetails

        private class NodeDetails
        {
            private NodeType _type = NodeType.CounterParty;
            private int _venueTypeID = int.MinValue;
            private int _counterPartyVenueID = int.MinValue;
            private int _counterPartyID = int.MinValue;
            private int _nodeID = int.MinValue;
            private int _exchangeID = int.MinValue;

            private string _venueType = string.Empty;

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

            public NodeDetails(NodeType type, int nodeID, int venueTypeID, int exchangeID)
            {
                _type = type;
                _nodeID = nodeID;
                _venueTypeID = venueTypeID;
                _exchangeID = exchangeID;
            }

            public NodeDetails(NodeType type, string venueType, int nodeID, int venueTypeID, int exchangeID)
            {
                _type = type;
                _venueType = venueType;
                _nodeID = nodeID;
                _venueTypeID = venueTypeID;
                _exchangeID = exchangeID;
            }

            public NodeDetails(int counterPartyVenueID, NodeType type, int counterPartyID)
            {
                _type = type;
                _counterPartyVenueID = counterPartyVenueID;
                _counterPartyID = counterPartyID;
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

            public int VenueTypeID
            {
                get { return _venueTypeID; }
                set { _venueTypeID = value; }
            }

            public int CounterPartyID
            {
                get { return _counterPartyID; }
                set { _counterPartyID = value; }
            }

            public int CounterPartyVenueID
            {
                get { return _counterPartyVenueID; }
                set { _counterPartyVenueID = value; }
            }

            public string VenueType
            {
                get { return _venueType; }
                set { _venueType = value; }
            }

            public int ExchangeID
            {
                get { return _exchangeID; }
                set { _exchangeID = value; }
            }
        }

        private enum NodeType
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

        #endregion NodeDetails

        private void cmbCountry_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCountry.Value != null)
            {
                int countryID = int.Parse(cmbCountry.Value.ToString());
                if (countryID > 0)
                {
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
                            if (column.Key.Equals("StateName"))
                            {
                                column.Hidden = false;
                            }
                            else
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
    }
}