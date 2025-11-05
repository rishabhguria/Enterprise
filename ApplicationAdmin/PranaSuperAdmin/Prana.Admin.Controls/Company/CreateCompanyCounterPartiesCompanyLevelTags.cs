using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.ThirdPartyManager.DataAccess;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyCounterPartiesCompanyLevelTags.
    /// </summary>
    public class CreateCompanyCounterPartiesCompanyLevelTags : System.Windows.Forms.UserControl
    {
        const string C_COMBO_SELECT = "- Select -";
        private const string VENDOR_TYPE = "PrimeBrokerClearer";

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbCounterParty;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtDeliverCompID;
        private System.Windows.Forms.TextBox txtSenderCompID;
        private System.Windows.Forms.Label lblCounterParty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblCompanyMPID;
        private System.Windows.Forms.Label lblCMTAGiveUp;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCMTAGiveUp;
        private System.Windows.Forms.Label lblIdentifier;
        private System.Windows.Forms.TextBox txtGiveUpIdentifier;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtClearingFirm;
        private System.Windows.Forms.Label lblClearingFirm;
        private System.Windows.Forms.GroupBox grpCompanyLevelDetails;
        private System.Windows.Forms.TextBox txtTargetCompID;
        private System.Windows.Forms.Label lblTargetCompID;
        private System.Windows.Forms.TextBox txtOnBehalfOfSubID;
        private System.Windows.Forms.Label lblOnBehalfOfSubID;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCompanyMPID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCompanyCounterPartyVenue;
        private UltraCombo cmbCompanyAccounts;
        private UltraCombo cmbCompanyStrategy;
        private UltraCombo cmbClearingFirmPrimeBrokers;
        private TextBox txtCMTAIdentifier;
        private Label label6;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private UltraGrid grdCMTA;
        private UltraGrid grdGiveUp;
        private Label label14;
        private Label label12;
        private IContainer components;

        public CreateCompanyCounterPartiesCompanyLevelTags()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //			
        }

        public void SetData()
        {
            BindClearingFirmPrimeBrokers();
            BindCounterParty();
            BindCompanyCounterPartyVenues();
            BindAccounts();
            BindStrategies();
            //BindIdentifiers();
            BindCompanyMPID();
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
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (cmbCounterParty != null)
                {
                    cmbCounterParty.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (txtDeliverCompID != null)
                {
                    txtDeliverCompID.Dispose();
                }
                if (txtSenderCompID != null)
                {
                    txtSenderCompID.Dispose();
                }
                if (lblCounterParty != null)
                {
                    lblCounterParty.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label17 != null)
                {
                    label17.Dispose();
                }
                if (lblCompanyMPID != null)
                {
                    lblCompanyMPID.Dispose();
                }
                if (lblCMTAGiveUp != null)
                {
                    lblCMTAGiveUp.Dispose();
                }
                if (cmbCMTAGiveUp != null)
                {
                    cmbCMTAGiveUp.Dispose();
                }
                if (lblIdentifier != null)
                {
                    lblIdentifier.Dispose();
                }
                if (txtGiveUpIdentifier != null)
                {
                    txtGiveUpIdentifier.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (txtClearingFirm != null)
                {
                    txtClearingFirm.Dispose();
                }
                if (lblClearingFirm != null)
                {
                    lblClearingFirm.Dispose();
                }
                if (grpCompanyLevelDetails != null)
                {
                    grpCompanyLevelDetails.Dispose();
                }
                if (txtTargetCompID != null)
                {
                    txtTargetCompID.Dispose();
                }
                if (lblTargetCompID != null)
                {
                    lblTargetCompID.Dispose();
                }
                if (txtOnBehalfOfSubID != null)
                {
                    txtOnBehalfOfSubID.Dispose();
                }
                if (lblOnBehalfOfSubID != null)
                {
                    lblOnBehalfOfSubID.Dispose();
                }
                if (cmbCompanyMPID != null)
                {
                    cmbCompanyMPID.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (cmbCompanyCounterPartyVenue != null)
                {
                    cmbCompanyCounterPartyVenue.Dispose();
                }
                if (cmbCompanyAccounts != null)
                {
                    cmbCompanyAccounts.Dispose();
                }
                if (cmbCompanyStrategy != null)
                {
                    cmbCompanyStrategy.Dispose();
                }
                if (cmbClearingFirmPrimeBrokers != null)
                {
                    cmbClearingFirmPrimeBrokers.Dispose();
                }
                if (txtCMTAIdentifier != null)
                {
                    txtCMTAIdentifier.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (groupBox2 != null)
                {
                    groupBox2.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (grdCMTA != null)
                {
                    grdCMTA.Dispose();
                }
                if (grdGiveUp != null)
                {
                    grdGiveUp.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (nextActCell != null)
                {
                    nextActCell.Dispose();
                }
                if (CMTAnextActCell != null)
                {
                    CMTAnextActCell.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCVCMTAIdentifierID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CMTAIdentifier", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCounterPartyVenueID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCVGiveupIdentifierID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("GiveUpIdentifier", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCounterPartyVenueId", 2);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContactPerson", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CellPhone", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkTelephone", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Email", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyThirdPartyID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyCVID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCounterPartyVenueID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVIdentifier", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Zip", 20);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyPrimeBrokerClearerID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCustodianID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAdministratorID", 6);
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueDetailsID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DisplayName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsElectronic", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixIdentifier", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECID", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SymbolConventionID", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SideID", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTypesID", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeInForceID", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn51 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstructionsID", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructionsID", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AdvancedOrdersID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OatsIdentifier", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn55 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("BaseCurrencyID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn56 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OtherCurrencyID", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn57 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn58 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECComplianceID", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn59 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 20);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn60 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FollowCompliance", 21);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn61 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortSellConfirmation", 22);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn62 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 23);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn63 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ForeignID", 24);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn64 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVFIXID", 25);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn65 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Acronym", 26);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn66 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixVersionID", 27);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn67 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TargetCompID", 28);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn68 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeliverToCompID", 29);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn69 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeliverToSubID", 30);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn70 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCounterPartyCVID", 31);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn71 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyName", 32);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn72 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueName", 33);
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
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn73 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyMPID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn74 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn75 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MPIDName", 2);
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCompanyCounterPartiesCompanyLevelTags));
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand8 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn76 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn77 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn78 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientIdentifierName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn79 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryKey", 3);
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            this.grpCompanyLevelDetails = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdCMTA = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdGiveUp = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmbClearingFirmPrimeBrokers = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCompanyStrategy = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCompanyAccounts = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCompanyCounterPartyVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbCompanyMPID = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtOnBehalfOfSubID = new System.Windows.Forms.TextBox();
            this.lblOnBehalfOfSubID = new System.Windows.Forms.Label();
            this.txtTargetCompID = new System.Windows.Forms.TextBox();
            this.lblTargetCompID = new System.Windows.Forms.Label();
            this.txtClearingFirm = new System.Windows.Forms.TextBox();
            this.lblClearingFirm = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCompanyMPID = new System.Windows.Forms.Label();
            this.lblCounterParty = new System.Windows.Forms.Label();
            this.txtDeliverCompID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbCounterParty = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSenderCompID = new System.Windows.Forms.TextBox();
            this.txtCMTAIdentifier = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblIdentifier = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtGiveUpIdentifier = new System.Windows.Forms.TextBox();
            this.cmbCMTAGiveUp = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblCMTAGiveUp = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grpCompanyLevelDetails.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCMTA)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGiveUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClearingFirmPrimeBrokers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyCounterPartyVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyMPID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCMTAGiveUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCompanyLevelDetails
            // 
            this.grpCompanyLevelDetails.Controls.Add(this.label14);
            this.grpCompanyLevelDetails.Controls.Add(this.label12);
            this.grpCompanyLevelDetails.Controls.Add(this.groupBox1);
            this.grpCompanyLevelDetails.Controls.Add(this.groupBox2);
            this.grpCompanyLevelDetails.Controls.Add(this.cmbClearingFirmPrimeBrokers);
            this.grpCompanyLevelDetails.Controls.Add(this.cmbCompanyStrategy);
            this.grpCompanyLevelDetails.Controls.Add(this.cmbCompanyAccounts);
            this.grpCompanyLevelDetails.Controls.Add(this.cmbCompanyCounterPartyVenue);
            this.grpCompanyLevelDetails.Controls.Add(this.label13);
            this.grpCompanyLevelDetails.Controls.Add(this.label8);
            this.grpCompanyLevelDetails.Controls.Add(this.cmbCompanyMPID);
            this.grpCompanyLevelDetails.Controls.Add(this.txtOnBehalfOfSubID);
            this.grpCompanyLevelDetails.Controls.Add(this.lblOnBehalfOfSubID);
            this.grpCompanyLevelDetails.Controls.Add(this.txtTargetCompID);
            this.grpCompanyLevelDetails.Controls.Add(this.lblTargetCompID);
            this.grpCompanyLevelDetails.Controls.Add(this.txtClearingFirm);
            this.grpCompanyLevelDetails.Controls.Add(this.lblClearingFirm);
            this.grpCompanyLevelDetails.Controls.Add(this.label11);
            this.grpCompanyLevelDetails.Controls.Add(this.label7);
            this.grpCompanyLevelDetails.Controls.Add(this.lblCompanyMPID);
            this.grpCompanyLevelDetails.Controls.Add(this.lblCounterParty);
            this.grpCompanyLevelDetails.Controls.Add(this.txtDeliverCompID);
            this.grpCompanyLevelDetails.Controls.Add(this.label1);
            this.grpCompanyLevelDetails.Controls.Add(this.label9);
            this.grpCompanyLevelDetails.Controls.Add(this.cmbCounterParty);
            this.grpCompanyLevelDetails.Controls.Add(this.label10);
            this.grpCompanyLevelDetails.Controls.Add(this.label3);
            this.grpCompanyLevelDetails.Controls.Add(this.label4);
            this.grpCompanyLevelDetails.Controls.Add(this.txtSenderCompID);
            this.grpCompanyLevelDetails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpCompanyLevelDetails.Location = new System.Drawing.Point(4, 4);
            this.grpCompanyLevelDetails.Name = "grpCompanyLevelDetails";
            this.grpCompanyLevelDetails.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grpCompanyLevelDetails.Size = new System.Drawing.Size(491, 415);
            this.grpCompanyLevelDetails.TabIndex = 0;
            this.grpCompanyLevelDetails.TabStop = false;
            this.grpCompanyLevelDetails.Text = "Client Level Details";
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(199, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(12, 10);
            this.label14.TabIndex = 6;
            this.label14.Text = "*";
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(174, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 8);
            this.label12.TabIndex = 3;
            this.label12.Text = "*";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdCMTA);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(239, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 135);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CMTA Identifier(s)";
            // 
            // grdCMTA
            // 
            ultraGridColumn1.DataType = typeof(long);
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.Caption = "CMTA Identifier";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn2.Width = 200;
            ultraGridColumn3.DataType = typeof(long);
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.grdCMTA.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCMTA.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCMTA.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCMTA.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCMTA.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCMTA.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCMTA.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCMTA.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCMTA.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCMTA.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCMTA.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCMTA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCMTA.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdCMTA.Location = new System.Drawing.Point(3, 17);
            this.grdCMTA.Name = "grdCMTA";
            this.grdCMTA.Size = new System.Drawing.Size(222, 115);
            this.grdCMTA.TabIndex = 0;
            this.grdCMTA.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCMTA.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCMTA_CellChange);
            this.grdCMTA.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdCMTA_BeforeRowsDeleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdGiveUp);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.MenuText;
            this.groupBox2.Location = new System.Drawing.Point(5, 274);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 135);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "GiveUp Identifier(s)";
            // 
            // grdGiveUp
            // 
            ultraGridColumn4.DataType = typeof(long);
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.Caption = "GiveUp Identifier";
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn5.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridColumn5.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn5.Width = 200;
            ultraGridColumn6.DataType = typeof(long);
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridColumn6.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand2.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            ultraGridBand2.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.grdGiveUp.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdGiveUp.DisplayLayout.GroupByBox.Hidden = true;
            this.grdGiveUp.DisplayLayout.MaxColScrollRegions = 1;
            this.grdGiveUp.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdGiveUp.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdGiveUp.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdGiveUp.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdGiveUp.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGiveUp.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdGiveUp.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdGiveUp.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdGiveUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGiveUp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdGiveUp.Location = new System.Drawing.Point(3, 17);
            this.grdGiveUp.Name = "grdGiveUp";
            this.grdGiveUp.Size = new System.Drawing.Size(222, 115);
            this.grdGiveUp.TabIndex = 0;
            this.grdGiveUp.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdGiveUp.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdGiveUp_CellChange);
            this.grdGiveUp.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdGiveUp_BeforeRowsDeleted);
            // 
            // cmbClearingFirmPrimeBrokers
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Appearance = appearance1;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn8.Header.VisiblePosition = 0;
            ultraGridColumn9.Header.VisiblePosition = 2;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.Header.VisiblePosition = 3;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.Header.VisiblePosition = 4;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.Header.VisiblePosition = 5;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 6;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn14.Header.VisiblePosition = 7;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn15.Header.VisiblePosition = 8;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn16.Header.VisiblePosition = 9;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn17.Header.VisiblePosition = 10;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn18.Header.VisiblePosition = 11;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn19.Header.VisiblePosition = 12;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn20.Header.VisiblePosition = 13;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn21.Header.VisiblePosition = 14;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn22.Header.VisiblePosition = 15;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn23.Header.VisiblePosition = 16;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn24.Header.VisiblePosition = 17;
            ultraGridColumn24.Hidden = true;
            ultraGridColumn25.Header.VisiblePosition = 18;
            ultraGridColumn25.Hidden = true;
            ultraGridColumn26.Header.VisiblePosition = 19;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.Header.VisiblePosition = 20;
            ultraGridColumn27.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24,
            ultraGridColumn25,
            ultraGridColumn26,
            ultraGridColumn27});
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClearingFirmPrimeBrokers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClearingFirmPrimeBrokers.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClearingFirmPrimeBrokers.DropDownWidth = 0;
            this.cmbClearingFirmPrimeBrokers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbClearingFirmPrimeBrokers.Location = new System.Drawing.Point(240, 69);
            this.cmbClearingFirmPrimeBrokers.Name = "cmbClearingFirmPrimeBrokers";
            this.cmbClearingFirmPrimeBrokers.Size = new System.Drawing.Size(224, 21);
            this.cmbClearingFirmPrimeBrokers.TabIndex = 2;
            this.cmbClearingFirmPrimeBrokers.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cmbCompanyStrategy
            // 
            this.cmbCompanyStrategy.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridColumn28.Header.VisiblePosition = 0;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn29.Header.Enabled = false;
            ultraGridColumn29.Header.VisiblePosition = 1;
            ultraGridColumn29.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn30.Header.VisiblePosition = 2;
            ultraGridColumn30.Hidden = true;
            ultraGridColumn31.Header.VisiblePosition = 3;
            ultraGridColumn31.Hidden = true;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn28,
            ultraGridColumn29,
            ultraGridColumn30,
            ultraGridColumn31});
            ultraGridBand4.Header.Enabled = false;
            ultraGridBand4.Hidden = true;
            this.cmbCompanyStrategy.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbCompanyStrategy.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCompanyStrategy.DropDownWidth = 0;
            this.cmbCompanyStrategy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCompanyStrategy.Location = new System.Drawing.Point(240, 224);
            this.cmbCompanyStrategy.Name = "cmbCompanyStrategy";
            this.cmbCompanyStrategy.Size = new System.Drawing.Size(224, 21);
            this.cmbCompanyStrategy.TabIndex = 9;
            this.cmbCompanyStrategy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCompanyStrategy.GotFocus += new System.EventHandler(this.cmbCompanyStrategy_GotFocus);
            this.cmbCompanyStrategy.LostFocus += new System.EventHandler(this.cmbCompanyStrategy_LostFocus);
            // 
            // cmbCompanyAccounts
            // 
            this.cmbCompanyAccounts.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand5.ColHeadersVisible = false;
            ultraGridColumn32.Header.VisiblePosition = 0;
            ultraGridColumn32.Hidden = true;
            ultraGridColumn33.Header.Enabled = false;
            ultraGridColumn33.Header.VisiblePosition = 1;
            ultraGridColumn33.Hidden = true;
            ultraGridColumn34.Header.VisiblePosition = 2;
            ultraGridColumn34.Hidden = true;
            ultraGridColumn35.Header.VisiblePosition = 3;
            ultraGridColumn35.Hidden = true;
            ultraGridColumn36.Header.VisiblePosition = 4;
            ultraGridColumn36.Hidden = true;
            ultraGridColumn37.Header.VisiblePosition = 5;
            ultraGridColumn37.Hidden = true;
            ultraGridColumn38.Header.VisiblePosition = 6;
            ultraGridColumn38.Hidden = true;
            ultraGridBand5.Columns.AddRange(new object[] {
            ultraGridColumn32,
            ultraGridColumn33,
            ultraGridColumn34,
            ultraGridColumn35,
            ultraGridColumn36,
            ultraGridColumn37,
            ultraGridColumn38});
            ultraGridBand5.HeaderVisible = true;
            ultraGridBand5.Hidden = true;
            this.cmbCompanyAccounts.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbCompanyAccounts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCompanyAccounts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance13.BorderColor = System.Drawing.Color.Silver;
            this.cmbCompanyAccounts.DisplayLayout.Override.RowAppearance = appearance13;
            this.cmbCompanyAccounts.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCompanyAccounts.DropDownWidth = 0;
            this.cmbCompanyAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCompanyAccounts.LimitToList = true;
            this.cmbCompanyAccounts.Location = new System.Drawing.Point(240, 202);
            this.cmbCompanyAccounts.Name = "cmbCompanyAccounts";
            this.cmbCompanyAccounts.Size = new System.Drawing.Size(224, 21);
            this.cmbCompanyAccounts.TabIndex = 8;
            this.cmbCompanyAccounts.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCompanyAccounts.GotFocus += new System.EventHandler(this.cmbCompanyAccounts_GotFocus);
            this.cmbCompanyAccounts.LostFocus += new System.EventHandler(this.cmbCompanyAccounts_LostFocus);
            // 
            // cmbCompanyCounterPartyVenue
            // 
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Appearance = appearance14;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand6.ColHeadersVisible = false;
            ultraGridColumn39.Header.VisiblePosition = 0;
            ultraGridColumn39.Hidden = true;
            ultraGridColumn40.Header.VisiblePosition = 1;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn41.Header.VisiblePosition = 2;
            ultraGridColumn41.Hidden = true;
            ultraGridColumn42.Header.VisiblePosition = 3;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn43.Header.VisiblePosition = 4;
            ultraGridColumn44.Header.VisiblePosition = 5;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn45.Header.VisiblePosition = 6;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn46.Header.VisiblePosition = 7;
            ultraGridColumn46.Hidden = true;
            ultraGridColumn47.Header.VisiblePosition = 8;
            ultraGridColumn47.Hidden = true;
            ultraGridColumn48.Header.VisiblePosition = 9;
            ultraGridColumn48.Hidden = true;
            ultraGridColumn49.Header.VisiblePosition = 10;
            ultraGridColumn49.Hidden = true;
            ultraGridColumn50.Header.VisiblePosition = 11;
            ultraGridColumn50.Hidden = true;
            ultraGridColumn51.Header.VisiblePosition = 12;
            ultraGridColumn51.Hidden = true;
            ultraGridColumn52.Header.VisiblePosition = 13;
            ultraGridColumn52.Hidden = true;
            ultraGridColumn53.Header.VisiblePosition = 14;
            ultraGridColumn53.Hidden = true;
            ultraGridColumn54.Header.VisiblePosition = 15;
            ultraGridColumn54.Hidden = true;
            ultraGridColumn55.Header.VisiblePosition = 16;
            ultraGridColumn55.Hidden = true;
            ultraGridColumn56.Header.VisiblePosition = 17;
            ultraGridColumn56.Hidden = true;
            ultraGridColumn57.Header.VisiblePosition = 18;
            ultraGridColumn57.Hidden = true;
            ultraGridColumn58.Header.VisiblePosition = 19;
            ultraGridColumn58.Hidden = true;
            ultraGridColumn59.Header.VisiblePosition = 20;
            ultraGridColumn59.Hidden = true;
            ultraGridColumn60.Header.VisiblePosition = 21;
            ultraGridColumn60.Hidden = true;
            ultraGridColumn61.Header.VisiblePosition = 22;
            ultraGridColumn61.Hidden = true;
            ultraGridColumn62.Header.VisiblePosition = 23;
            ultraGridColumn62.Hidden = true;
            ultraGridColumn63.Header.VisiblePosition = 24;
            ultraGridColumn63.Hidden = true;
            ultraGridColumn64.Header.VisiblePosition = 25;
            ultraGridColumn64.Hidden = true;
            ultraGridColumn65.Header.VisiblePosition = 26;
            ultraGridColumn65.Hidden = true;
            ultraGridColumn66.Header.VisiblePosition = 27;
            ultraGridColumn66.Hidden = true;
            ultraGridColumn67.Header.VisiblePosition = 28;
            ultraGridColumn67.Hidden = true;
            ultraGridColumn68.Header.VisiblePosition = 29;
            ultraGridColumn68.Hidden = true;
            ultraGridColumn69.Header.VisiblePosition = 30;
            ultraGridColumn69.Hidden = true;
            ultraGridColumn70.Header.VisiblePosition = 31;
            ultraGridColumn70.Hidden = true;
            ultraGridColumn71.Header.VisiblePosition = 32;
            ultraGridColumn71.Hidden = true;
            ultraGridColumn72.Header.VisiblePosition = 33;
            ultraGridColumn72.Hidden = true;
            ultraGridBand6.Columns.AddRange(new object[] {
            ultraGridColumn39,
            ultraGridColumn40,
            ultraGridColumn41,
            ultraGridColumn42,
            ultraGridColumn43,
            ultraGridColumn44,
            ultraGridColumn45,
            ultraGridColumn46,
            ultraGridColumn47,
            ultraGridColumn48,
            ultraGridColumn49,
            ultraGridColumn50,
            ultraGridColumn51,
            ultraGridColumn52,
            ultraGridColumn53,
            ultraGridColumn54,
            ultraGridColumn55,
            ultraGridColumn56,
            ultraGridColumn57,
            ultraGridColumn58,
            ultraGridColumn59,
            ultraGridColumn60,
            ultraGridColumn61,
            ultraGridColumn62,
            ultraGridColumn63,
            ultraGridColumn64,
            ultraGridColumn65,
            ultraGridColumn66,
            ultraGridColumn67,
            ultraGridColumn68,
            ultraGridColumn69,
            ultraGridColumn70,
            ultraGridColumn71,
            ultraGridColumn72});
            ultraGridBand6.GroupHeadersVisible = false;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbCompanyCounterPartyVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.GroupByBox.Appearance = appearance15;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance16;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance17;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.ActiveCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.SystemColors.Highlight;
            appearance19.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.CellAppearance = appearance21;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.CellPadding = 0;
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            appearance23.TextHAlignAsString = "Left";
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.RowAppearance = appearance24;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCompanyCounterPartyVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCompanyCounterPartyVenue.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCompanyCounterPartyVenue.DropDownWidth = 0;
            this.cmbCompanyCounterPartyVenue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCompanyCounterPartyVenue.Location = new System.Drawing.Point(240, 46);
            this.cmbCompanyCounterPartyVenue.Name = "cmbCompanyCounterPartyVenue";
            this.cmbCompanyCounterPartyVenue.Size = new System.Drawing.Size(224, 21);
            this.cmbCompanyCounterPartyVenue.TabIndex = 1;
            this.cmbCompanyCounterPartyVenue.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCompanyCounterPartyVenue.ValueChanged += new System.EventHandler(this.cmbCompanyCounterPartyVenue_ValueChanged);
            this.cmbCompanyCounterPartyVenue.GotFocus += new System.EventHandler(this.cmbCompanyCounterPartyVenue_GotFocus);
            this.cmbCompanyCounterPartyVenue.LostFocus += new System.EventHandler(this.cmbCompanyCounterPartyVenue_LostFocus);
            // 
            // label13
            // 
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(111, 119);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(12, 8);
            this.label13.TabIndex = 11;
            this.label13.Text = "*";
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(107, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 8);
            this.label8.TabIndex = 16;
            this.label8.Text = "*";
            // 
            // cmbCompanyMPID
            // 
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCompanyMPID.DisplayLayout.Appearance = appearance26;
            this.cmbCompanyMPID.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand7.ColHeadersVisible = false;
            ultraGridColumn73.Header.VisiblePosition = 0;
            ultraGridColumn73.Hidden = true;
            ultraGridColumn74.Header.VisiblePosition = 1;
            ultraGridColumn74.Hidden = true;
            ultraGridColumn75.Header.VisiblePosition = 2;
            ultraGridBand7.Columns.AddRange(new object[] {
            ultraGridColumn73,
            ultraGridColumn74,
            ultraGridColumn75});
            ultraGridBand7.GroupHeadersVisible = false;
            this.cmbCompanyMPID.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.cmbCompanyMPID.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCompanyMPID.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbCompanyMPID.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCompanyMPID.DisplayLayout.MaxRowScrollRegions = 1;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCompanyMPID.DisplayLayout.Override.ActiveCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.SystemColors.Highlight;
            appearance28.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCompanyMPID.DisplayLayout.Override.ActiveRowAppearance = appearance28;
            this.cmbCompanyMPID.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCompanyMPID.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyMPID.DisplayLayout.Override.CardAreaAppearance = appearance29;
            appearance30.BorderColor = System.Drawing.Color.Silver;
            appearance30.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCompanyMPID.DisplayLayout.Override.CellAppearance = appearance30;
            this.cmbCompanyMPID.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCompanyMPID.DisplayLayout.Override.CellPadding = 0;
            appearance31.BackColor = System.Drawing.SystemColors.Control;
            appearance31.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance31.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance31.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyMPID.DisplayLayout.Override.GroupByRowAppearance = appearance31;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            this.cmbCompanyMPID.DisplayLayout.Override.RowAppearance = appearance32;
            this.cmbCompanyMPID.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance33.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCompanyMPID.DisplayLayout.Override.TemplateAddRowAppearance = appearance33;
            this.cmbCompanyMPID.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCompanyMPID.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCompanyMPID.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCompanyMPID.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCompanyMPID.DropDownWidth = 0;
            this.cmbCompanyMPID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCompanyMPID.Location = new System.Drawing.Point(240, 92);
            this.cmbCompanyMPID.Name = "cmbCompanyMPID";
            this.cmbCompanyMPID.Size = new System.Drawing.Size(224, 21);
            this.cmbCompanyMPID.TabIndex = 3;
            this.cmbCompanyMPID.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCompanyMPID.GotFocus += new System.EventHandler(this.cmbCompanyMPID_GotFocus);
            this.cmbCompanyMPID.LostFocus += new System.EventHandler(this.cmbCompanyMPID_LostFocus);
            // 
            // txtOnBehalfOfSubID
            // 
            this.txtOnBehalfOfSubID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOnBehalfOfSubID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtOnBehalfOfSubID.Location = new System.Drawing.Point(240, 247);
            this.txtOnBehalfOfSubID.MaxLength = 50;
            this.txtOnBehalfOfSubID.Name = "txtOnBehalfOfSubID";
            this.txtOnBehalfOfSubID.Size = new System.Drawing.Size(224, 21);
            this.txtOnBehalfOfSubID.TabIndex = 10;
            this.txtOnBehalfOfSubID.GotFocus += new System.EventHandler(this.txtOnBehalfOfSubID_GotFocus);
            this.txtOnBehalfOfSubID.LostFocus += new System.EventHandler(this.txtOnBehalfOfSubID_LostFocus);
            // 
            // lblOnBehalfOfSubID
            // 
            this.lblOnBehalfOfSubID.AutoSize = true;
            this.lblOnBehalfOfSubID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOnBehalfOfSubID.Location = new System.Drawing.Point(18, 250);
            this.lblOnBehalfOfSubID.Name = "lblOnBehalfOfSubID";
            this.lblOnBehalfOfSubID.Size = new System.Drawing.Size(99, 13);
            this.lblOnBehalfOfSubID.TabIndex = 24;
            this.lblOnBehalfOfSubID.Text = "On Behalf of SubID";
            // 
            // txtTargetCompID
            // 
            this.txtTargetCompID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTargetCompID.Location = new System.Drawing.Point(240, 158);
            this.txtTargetCompID.MaxLength = 50;
            this.txtTargetCompID.Name = "txtTargetCompID";
            this.txtTargetCompID.Size = new System.Drawing.Size(224, 21);
            this.txtTargetCompID.TabIndex = 6;
            this.txtTargetCompID.GotFocus += new System.EventHandler(this.txtTargetCompID_GotFocus);
            this.txtTargetCompID.LostFocus += new System.EventHandler(this.txtTargetCompID_LostFocus);
            // 
            // lblTargetCompID
            // 
            this.lblTargetCompID.AutoSize = true;
            this.lblTargetCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTargetCompID.Location = new System.Drawing.Point(18, 160);
            this.lblTargetCompID.Name = "lblTargetCompID";
            this.lblTargetCompID.Size = new System.Drawing.Size(83, 13);
            this.lblTargetCompID.TabIndex = 15;
            this.lblTargetCompID.Text = "Target Comp ID";
            // 
            // txtClearingFirm
            // 
            this.txtClearingFirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClearingFirm.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClearingFirm.Location = new System.Drawing.Point(240, 180);
            this.txtClearingFirm.MaxLength = 50;
            this.txtClearingFirm.Name = "txtClearingFirm";
            this.txtClearingFirm.Size = new System.Drawing.Size(224, 21);
            this.txtClearingFirm.TabIndex = 7;
            this.txtClearingFirm.GotFocus += new System.EventHandler(this.txtClearingFirm_GotFocus);
            this.txtClearingFirm.LostFocus += new System.EventHandler(this.txtClearingFirm_LostFocus);
            // 
            // lblClearingFirm
            // 
            this.lblClearingFirm.AutoSize = true;
            this.lblClearingFirm.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClearingFirm.Location = new System.Drawing.Point(18, 183);
            this.lblClearingFirm.Name = "lblClearingFirm";
            this.lblClearingFirm.Size = new System.Drawing.Size(66, 13);
            this.lblClearingFirm.TabIndex = 18;
            this.lblClearingFirm.Text = "ClearingFirm";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(18, 205);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Client Accounts";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(18, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Client Strategy";
            // 
            // lblCompanyMPID
            // 
            this.lblCompanyMPID.AutoSize = true;
            this.lblCompanyMPID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCompanyMPID.Location = new System.Drawing.Point(18, 95);
            this.lblCompanyMPID.Name = "lblCompanyMPID";
            this.lblCompanyMPID.Size = new System.Drawing.Size(62, 13);
            this.lblCompanyMPID.TabIndex = 8;
            this.lblCompanyMPID.Text = "Client MPID";
            // 
            // lblCounterParty
            // 
            this.lblCounterParty.BackColor = System.Drawing.Color.White;
            this.lblCounterParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCounterParty.Location = new System.Drawing.Point(240, 20);
            this.lblCounterParty.Name = "lblCounterParty";
            this.lblCounterParty.Size = new System.Drawing.Size(224, 21);
            this.lblCounterParty.TabIndex = 0;
            // 
            // txtDeliverCompID
            // 
            this.txtDeliverCompID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeliverCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDeliverCompID.Location = new System.Drawing.Point(240, 136);
            this.txtDeliverCompID.MaxLength = 50;
            this.txtDeliverCompID.Name = "txtDeliverCompID";
            this.txtDeliverCompID.Size = new System.Drawing.Size(224, 21);
            this.txtDeliverCompID.TabIndex = 5;
            this.txtDeliverCompID.GotFocus += new System.EventHandler(this.txtDeliverCompID_GotFocus);
            this.txtDeliverCompID.LostFocus += new System.EventHandler(this.txtDeliverCompID_LostFocus);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = ApplicationConstants.CONST_BROKER;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(18, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Sender Comp ID";
            // 
            // cmbCounterParty
            // 
            this.cmbCounterParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCounterParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCounterParty.Location = new System.Drawing.Point(242, 18);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(224, 21);
            this.cmbCounterParty.TabIndex = 7;
            this.cmbCounterParty.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.Location = new System.Drawing.Point(18, 139);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Deliver to Comp ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(18, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "ClientBrokerVenue";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(18, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Client ClearingFirm/PrimeBroker";
            // 
            // txtSenderCompID
            // 
            this.txtSenderCompID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSenderCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSenderCompID.Location = new System.Drawing.Point(240, 114);
            this.txtSenderCompID.MaxLength = 50;
            this.txtSenderCompID.Name = "txtSenderCompID";
            this.txtSenderCompID.Size = new System.Drawing.Size(224, 21);
            this.txtSenderCompID.TabIndex = 4;
            this.txtSenderCompID.GotFocus += new System.EventHandler(this.txtSenderCompID_GotFocus);
            this.txtSenderCompID.LostFocus += new System.EventHandler(this.txtSenderCompID_LostFocus);
            // 
            // txtCMTAIdentifier
            // 
            this.txtCMTAIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCMTAIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCMTAIdentifier.Location = new System.Drawing.Point(400, 43);
            this.txtCMTAIdentifier.MaxLength = 50;
            this.txtCMTAIdentifier.Name = "txtCMTAIdentifier";
            this.txtCMTAIdentifier.Size = new System.Drawing.Size(55, 21);
            this.txtCMTAIdentifier.TabIndex = 10;
            this.txtCMTAIdentifier.Visible = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(397, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 14);
            this.label6.TabIndex = 205;
            this.label6.Text = "CMTA Identifier";
            this.label6.Visible = false;
            // 
            // lblIdentifier
            // 
            this.lblIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIdentifier.Location = new System.Drawing.Point(397, 72);
            this.lblIdentifier.Name = "lblIdentifier";
            this.lblIdentifier.Size = new System.Drawing.Size(98, 14);
            this.lblIdentifier.TabIndex = 184;
            this.lblIdentifier.Text = "GiveUp Identifier";
            this.lblIdentifier.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(400, 244);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(40, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtGiveUpIdentifier
            // 
            this.txtGiveUpIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGiveUpIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtGiveUpIdentifier.Location = new System.Drawing.Point(400, 94);
            this.txtGiveUpIdentifier.MaxLength = 50;
            this.txtGiveUpIdentifier.Name = "txtGiveUpIdentifier";
            this.txtGiveUpIdentifier.Size = new System.Drawing.Size(61, 21);
            this.txtGiveUpIdentifier.TabIndex = 11;
            this.txtGiveUpIdentifier.Visible = false;
            this.txtGiveUpIdentifier.GotFocus += new System.EventHandler(this.txtIdentifier_GotFocus);
            this.txtGiveUpIdentifier.LostFocus += new System.EventHandler(this.txtIdentifier_LostFocus);
            // 
            // cmbCMTAGiveUp
            // 
            appearance34.BackColor = System.Drawing.SystemColors.Window;
            appearance34.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCMTAGiveUp.DisplayLayout.Appearance = appearance34;
            this.cmbCMTAGiveUp.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand8.ColHeadersVisible = false;
            ultraGridColumn76.Header.VisiblePosition = 0;
            ultraGridColumn76.Hidden = true;
            ultraGridColumn77.Header.VisiblePosition = 1;
            ultraGridColumn78.Header.VisiblePosition = 2;
            ultraGridColumn78.Hidden = true;
            ultraGridColumn79.Header.VisiblePosition = 3;
            ultraGridColumn79.Hidden = true;
            ultraGridBand8.Columns.AddRange(new object[] {
            ultraGridColumn76,
            ultraGridColumn77,
            ultraGridColumn78,
            ultraGridColumn79});
            this.cmbCMTAGiveUp.DisplayLayout.BandsSerializer.Add(ultraGridBand8);
            this.cmbCMTAGiveUp.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCMTAGiveUp.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance35.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance35.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance35.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.Appearance = appearance35;
            appearance36.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.BandLabelAppearance = appearance36;
            this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance37.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance37.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.PromptAppearance = appearance37;
            this.cmbCMTAGiveUp.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCMTAGiveUp.DisplayLayout.MaxRowScrollRegions = 1;
            appearance38.BackColor = System.Drawing.SystemColors.Window;
            appearance38.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCMTAGiveUp.DisplayLayout.Override.ActiveCellAppearance = appearance38;
            appearance39.BackColor = System.Drawing.SystemColors.Highlight;
            appearance39.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCMTAGiveUp.DisplayLayout.Override.ActiveRowAppearance = appearance39;
            this.cmbCMTAGiveUp.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCMTAGiveUp.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance40.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCMTAGiveUp.DisplayLayout.Override.CardAreaAppearance = appearance40;
            appearance41.BorderColor = System.Drawing.Color.Silver;
            appearance41.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCMTAGiveUp.DisplayLayout.Override.CellAppearance = appearance41;
            this.cmbCMTAGiveUp.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCMTAGiveUp.DisplayLayout.Override.CellPadding = 0;
            appearance42.BackColor = System.Drawing.SystemColors.Control;
            appearance42.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance42.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance42.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCMTAGiveUp.DisplayLayout.Override.GroupByRowAppearance = appearance42;
            appearance43.TextHAlignAsString = "Left";
            this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderAppearance = appearance43;
            this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance44.BackColor = System.Drawing.SystemColors.Window;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            this.cmbCMTAGiveUp.DisplayLayout.Override.RowAppearance = appearance44;
            this.cmbCMTAGiveUp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance45.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCMTAGiveUp.DisplayLayout.Override.TemplateAddRowAppearance = appearance45;
            this.cmbCMTAGiveUp.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCMTAGiveUp.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCMTAGiveUp.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCMTAGiveUp.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCMTAGiveUp.DropDownWidth = 0;
            this.cmbCMTAGiveUp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCMTAGiveUp.Location = new System.Drawing.Point(403, 209);
            this.cmbCMTAGiveUp.Name = "cmbCMTAGiveUp";
            this.cmbCMTAGiveUp.Size = new System.Drawing.Size(58, 21);
            this.cmbCMTAGiveUp.TabIndex = 10;
            this.cmbCMTAGiveUp.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCMTAGiveUp.Visible = false;
            this.cmbCMTAGiveUp.GotFocus += new System.EventHandler(this.cmbCMTAGiveUp_GotFocus);
            this.cmbCMTAGiveUp.LostFocus += new System.EventHandler(this.cmbCMTAGiveUp_LostFocus);
            // 
            // lblCMTAGiveUp
            // 
            this.lblCMTAGiveUp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCMTAGiveUp.Location = new System.Drawing.Point(397, 179);
            this.lblCMTAGiveUp.Name = "lblCMTAGiveUp";
            this.lblCMTAGiveUp.Size = new System.Drawing.Size(79, 21);
            this.lblCMTAGiveUp.TabIndex = 181;
            this.lblCMTAGiveUp.Text = "CMTA/Give Up";
            this.lblCMTAGiveUp.Visible = false;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(6, 422);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(90, 16);
            this.label17.TabIndex = 1;
            this.label17.Text = "* Required Field";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(178, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 8);
            this.label2.TabIndex = 35;
            this.label2.Text = "*";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(205, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 8);
            this.label5.TabIndex = 36;
            this.label5.Text = "*";
            // 
            // CreateCompanyCounterPartiesCompanyLevelTags
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpCompanyLevelDetails);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCMTAIdentifier);
            this.Controls.Add(this.cmbCMTAGiveUp);
            this.Controls.Add(this.lblCMTAGiveUp);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.lblIdentifier);
            this.Controls.Add(this.txtGiveUpIdentifier);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CreateCompanyCounterPartiesCompanyLevelTags";
            this.Size = new System.Drawing.Size(499, 445);
            this.Load += new System.EventHandler(this.CreateCompanyCounterPartiesCompanyLevelTags_Load_1);
            this.grpCompanyLevelDetails.ResumeLayout(false);
            this.grpCompanyLevelDetails.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCMTA)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGiveUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClearingFirmPrimeBrokers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyCounterPartyVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyMPID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCMTAGiveUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void BindClearingFirmPrimeBrokers()
        {
            //cmbClearingFirmPrimeBrokers.DataSource = null;
            Prana.BusinessObjects.ThirdParties thirdParties = ThirdPartyDataManager.GetThirdParties();
            Prana.BusinessObjects.ThirdParties thirdPartyCFPBs = new Prana.BusinessObjects.ThirdParties();
            foreach (Prana.BusinessObjects.ThirdParty thirdParty in thirdParties)
            {
                if (ThirdPartyDataManager.GetThirdPartyTypeName(thirdParty) == VENDOR_TYPE)
                {
                    thirdPartyCFPBs.Add(thirdParty);
                }
            }
            //ClearingFirmsPrimeBrokers clearingFirmPrimeBrokers = CompanyManager.GetClearingFirmPrimeBroker(_companyID);
            //			if (clearingFirmPrimeBrokers.Count > 0 )
            //			{
            //clearingFirmPrimeBrokers.Insert(0, new ClearingFirmPrimeBroker(int.MinValue, C_COMBO_SELECT));
            thirdPartyCFPBs.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, C_COMBO_SELECT));
            cmbClearingFirmPrimeBrokers.DataSource = null;
            cmbClearingFirmPrimeBrokers.DataSource = thirdPartyCFPBs;
            cmbClearingFirmPrimeBrokers.DisplayMember = "ThirdPartyName";
            cmbClearingFirmPrimeBrokers.ValueMember = "ThirdPartyID";
            //cmbClearingFirmPrimeBrokers.SelectedValue = int.MinValue; 
            cmbClearingFirmPrimeBrokers.Text = C_COMBO_SELECT;
            //			

            if (_companyCounterPartyVenueDetailEdit != null)
            {
                cmbClearingFirmPrimeBrokers.Value = int.Parse(_companyCounterPartyVenueDetailEdit.ClearingFirmPrimeBrokerID.ToString());
            }
            ColumnsCollection columns3 = cmbClearingFirmPrimeBrokers.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns3)
            {
                if (column.Key != "ThirdPartyName")
                {
                    column.Hidden = true;
                }
            }
            cmbClearingFirmPrimeBrokers.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        private void BindCounterParty()
        {

            CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(_companyID);
            //			if (counterParties.Count > 0 )
            //			{
            counterParties.Insert(0, new CounterParty(int.MinValue, C_COMBO_SELECT));
            cmbCounterParty.DataSource = counterParties;
            cmbCounterParty.DisplayMember = "CounterPartyFullName";
            cmbCounterParty.ValueMember = "CounterPartyID";

            //cmbCounterParty.SelectedValue = _companyCounterPartyID;
            //lblCounterParty.Text = _companyCounterPartyID.ToString();

            this.cmbCounterParty.SelectedValueChanged += new System.EventHandler(this.cmbCounterParty_SelectedValueChanged);
            this.cmbCounterParty.SelectedIndexChanged += new System.EventHandler(this.cmbCounterParty_SelectedIndexChanged);
            //			}


            if (_companyCounterPartyVenueDetailEdit != null)
            {
                cmbCounterParty.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID.ToString());
            }
        }


        private void BindAccounts()
        {
            cmbCompanyAccounts.DataSource = null;
            Accounts accounts = CompanyManager.GetAccount(_companyID);
            //			if (accounts.Count > 0 )
            //			{
            accounts.Insert(0, new Account(int.MinValue, C_COMBO_SELECT));
            cmbCompanyAccounts.DataSource = accounts;
            cmbCompanyAccounts.DisplayMember = "AccountName";
            cmbCompanyAccounts.ValueMember = "CompanyAccountID";

            cmbCompanyAccounts.Value = int.MinValue;
            //			}			
            if (_companyCounterPartyVenueDetailEdit != null)
            {
                cmbCompanyAccounts.Value = _companyCounterPartyVenueDetailEdit.AccountID;
            }
            ColumnsCollection columns = cmbCompanyAccounts.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "AccountName")
                {
                    column.Hidden = true;
                }
            }
            cmbCompanyAccounts.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }
        private void BindStrategies()
        {
            cmbCompanyStrategy.DataSource = null;
            Strategies strategies = CompanyManager.GetStrategy(_companyID);
            //			if (strategies.Count > 0 )
            //			{
            strategies.Insert(0, new Strategy(int.MinValue, C_COMBO_SELECT));
            cmbCompanyStrategy.DataSource = strategies;
            cmbCompanyStrategy.DisplayMember = "StrategyName";
            cmbCompanyStrategy.ValueMember = "StrategyID";
            cmbCompanyStrategy.Value = int.MinValue;
            //			}


            if (_companyCounterPartyVenueDetailEdit != null)
            {
                cmbCompanyStrategy.Value = _companyCounterPartyVenueDetailEdit.StrategyID;
            }
            ColumnsCollection columns = cmbCompanyStrategy.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "StrategyName")
                {
                    column.Hidden = true;
                }
            }
            cmbCompanyStrategy.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        private void BindCompanyMPID()
        {
            //cmbCompanyMPID.DataSource = null;
            //MPIDs mpids = CompanyManager.GetCompanyMPIDs(_companyID);
            MPIDCollection mpids = CompanyManager.GetCompanyMPIDs(_companyID);
            //			if (mpids.Count > 0 )
            //			{
            mpids.Add(new MPID(int.MinValue, int.MinValue, C_COMBO_SELECT));
            //mpids.AddNew();	
            cmbCompanyMPID.DataSource = null;
            cmbCompanyMPID.DataSource = mpids;
            cmbCompanyMPID.DisplayMember = "MPIDName";
            cmbCompanyMPID.ValueMember = "CompanyMPID";

            cmbCompanyMPID.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCompanyMPID.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("MPIDName"))
                {
                    column.Hidden = true;
                }
            }
            cmbCompanyMPID.DisplayLayout.Bands[0].ColHeadersVisible = false;
            //			}

            if (_companyCounterPartyVenueDetailEdit != null)
            {
                cmbCompanyMPID.Value = _companyCounterPartyVenueDetailEdit.MPID;
            }
        }

        //private void BindIdentifiers()
        //{
        //    Identifiers identifiers = AUECManager.GetIdentifiers();
        //    if (identifiers.Count > 0 )
        //    {
        //        identifiers.Insert(0, new Identifier(int.MinValue, "None"));
        //        cmbCMTAGiveUp.DataSource = identifiers;				
        //        cmbCMTAGiveUp.DisplayMember = "IdentifierName";
        //        cmbCMTAGiveUp.ValueMember = "IdentifierID";

        //        cmbCMTAGiveUp.Value = int.MinValue; 
        //    }

        //    if(_companyCounterPartyVenueDetailEdit != null)
        //    {
        //        cmbCMTAGiveUp.Value = _companyCounterPartyVenueDetailEdit.CMTAGiveUp;
        //    }
        //}

        private void BindCompanyCounterPartyVenues()
        {
            CompanyCounterPartyVenues companyCounterPartyVenues = new CompanyCounterPartyVenues();
            int counterPartyVenueID = int.MinValue;
            cmbCompanyCounterPartyVenue.DataSource = null;
            companyCounterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneusChanged(_companyID, _companyCounterPartyID);
            //			if(companyCounterPartyVenues.Count > 0)
            //			{
            companyCounterPartyVenues.Insert(0, new CompanyCounterPartyVenue(counterPartyVenueID, C_COMBO_SELECT));
            cmbCompanyCounterPartyVenue.DataSource = companyCounterPartyVenues;
            cmbCompanyCounterPartyVenue.DisplayMember = "CounterPartyVenueDisplayName";
            cmbCompanyCounterPartyVenue.ValueMember = "CompanyCounterPartyCVID";
            cmbCompanyCounterPartyVenue.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCompanyCounterPartyVenue.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("CounterPartyVenueDisplayName"))
                {
                    column.Hidden = true;
                }
            }
            cmbCompanyCounterPartyVenue.DisplayLayout.Bands[0].ColHeadersVisible = false;

            int companyCounterPartyVenueID = int.Parse(cmbCompanyCounterPartyVenue.Value.ToString());
            CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = CompanyManager.GetCompanyCounterPartyVenueDetail(companyCounterPartyVenueID);
            //			}

            if (_companyCounterPartyVenueDetailEdit != null)
            {
                cmbCompanyCounterPartyVenue.Value = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID.ToString());
            }
            ColumnsCollection columns1 = cmbCompanyCounterPartyVenue.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns1)
            {
                if (column.Key != "CounterPartyVenueDisplayName")
                {
                    column.Hidden = true;
                }
            }
            cmbCompanyCounterPartyVenue.DisplayLayout.Bands[0].ColHeadersVisible = false;

        }

        private void BindCompanyCounterPartyVenuesGrid()
        {
            CompanyCounterPartyVenues companyCounterPartyVenues = (CompanyCounterPartyVenues)cmbCompanyCounterPartyVenue.DataSource;
            if (companyCounterPartyVenues.Count > 0)
            {
                int companyCounterPartyVenueID = int.Parse(cmbCompanyCounterPartyVenue.Value.ToString());
                CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = CompanyManager.GetCompanyCounterPartyVenueDetail(companyCounterPartyVenueID);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Refresh(sender, e);
        }

        Prana.Admin.BLL.CompanyCounterPartyVenueDetail _companyCounterPartyVenueDetailEdit = null;
        public Prana.Admin.BLL.CompanyCounterPartyVenueDetail CompanyCounterPartyVenueDetailEdit
        {
            set { _companyCounterPartyVenueDetailEdit = value; }
        }

        public void BindForEdit()
        {
            if (_companyCounterPartyVenueDetailEdit != null)
            {
                //cmbCounterParty.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID.ToString());

                cmbCompanyCounterPartyVenue.Value = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID.ToString());
                cmbClearingFirmPrimeBrokers.Value = int.Parse(_companyCounterPartyVenueDetailEdit.ClearingFirmPrimeBrokerID.ToString());

                cmbCompanyMPID.Value = int.Parse(_companyCounterPartyVenueDetailEdit.MPID.ToString());

                txtSenderCompID.Text = _companyCounterPartyVenueDetailEdit.SenderCompanyID.ToString();
                txtDeliverCompID.Text = _companyCounterPartyVenueDetailEdit.DeliverToCompanyID.ToString();
                txtTargetCompID.Text = _companyCounterPartyVenueDetailEdit.TargetCompID.ToString();
                txtClearingFirm.Text = _companyCounterPartyVenueDetailEdit.ClearingFirm.ToString();
                cmbCompanyAccounts.Value = int.Parse(_companyCounterPartyVenueDetailEdit.AccountID.ToString());
                cmbCompanyStrategy.Value = int.Parse(_companyCounterPartyVenueDetailEdit.StrategyID.ToString());
                //cmbCMTAGiveUp.Value = int.Parse(_companyCounterPartyVenueDetailEdit.CMTAGiveUp.ToString());
                //txtGiveUpIdentifier.Text = _companyCounterPartyVenueDetailEdit.IdentifierName.ToString();
                txtOnBehalfOfSubID.Text = _companyCounterPartyVenueDetailEdit.OnBehalfOfSubID.ToString();

                ColumnsCollection columns = cmbCompanyAccounts.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "AccountName")
                    {
                        column.Hidden = true;
                    }
                }

                columns = cmbCompanyStrategy.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "StrategyName")
                    {
                        column.Hidden = true;
                    }
                }
            }
        }

        public void SetupControl(int companyCounterPartyVenueID)
        {
            _companyCounterPartyVenueID = companyCounterPartyVenueID;
            SetCompanyCounterPartyVenueDetails(_companyCounterPartyVenueID);
        }

        private void SetCompanyCounterPartyVenueDetails(int companyCounterPartyVenueID)
        {
            CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = CompanyManager.GetCompanyCounterPartyVenueDetail(companyCounterPartyVenueID);
            if (companyCounterPartyVenueDetail != null)
            {
                if (companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID > int.MinValue)
                {
                    //cmbCounterParty.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID.ToString());

                    cmbCompanyCounterPartyVenue.Value = int.Parse(companyCounterPartyVenueDetail.CompanyCounterPartyVenueID.ToString());
                    cmbClearingFirmPrimeBrokers.Value = int.Parse(companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID.ToString());

                    cmbCompanyMPID.Value = int.Parse(companyCounterPartyVenueDetail.MPID.ToString());

                    txtSenderCompID.Text = companyCounterPartyVenueDetail.SenderCompanyID.ToString();
                    txtDeliverCompID.Text = companyCounterPartyVenueDetail.DeliverToCompanyID.ToString();
                    txtTargetCompID.Text = companyCounterPartyVenueDetail.TargetCompID.ToString();
                    txtClearingFirm.Text = companyCounterPartyVenueDetail.ClearingFirm.ToString();
                    cmbCompanyAccounts.Value = int.Parse(companyCounterPartyVenueDetail.AccountID.ToString());
                    cmbCompanyStrategy.Value = int.Parse(companyCounterPartyVenueDetail.StrategyID.ToString());
                    //cmbCMTAGiveUp.Value = int.Parse(companyCounterPartyVenueDetail.CMTAGiveUp.ToString());
                    //txtGiveUpIdentifier.Text = companyCounterPartyVenueDetail.IdentifierName.ToString();

                    txtCMTAIdentifier.Text = companyCounterPartyVenueDetail.CMTAIdentifier.ToString();
                    txtGiveUpIdentifier.Text = companyCounterPartyVenueDetail.GiveUpIdentifier.ToString();
                    txtOnBehalfOfSubID.Text = companyCounterPartyVenueDetail.OnBehalfOfSubID.ToString();

                    if (cmbClearingFirmPrimeBrokers.Text == "")
                    {
                        cmbClearingFirmPrimeBrokers.Value = int.MinValue;
                    }

                    ColumnsCollection columns = cmbCompanyCounterPartyVenue.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "CounterPartyVenueDisplayName")
                        {
                            column.Hidden = true;
                        }
                    }

                    columns = cmbClearingFirmPrimeBrokers.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "ThirdPartyName")
                        {
                            column.Hidden = true;
                        }
                    }

                    columns = cmbCompanyAccounts.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "AccountName")
                        {
                            column.Hidden = true;
                        }
                    }

                    columns = cmbCompanyStrategy.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "StrategyName")
                        {
                            column.Hidden = true;
                        }
                    }
                }
                else
                {
                    CounterPartyVenue cpvDetails = CounterPartyManager.GetCVFIXForCompanyCPVID(companyCounterPartyVenueID);
                    txtTargetCompID.Text = cpvDetails.TargetCompID.ToString();
                    txtDeliverCompID.Text = cpvDetails.DeliverToCompID.ToString();

                    cmbCompanyAccounts.Text = C_COMBO_SELECT;
                    txtSenderCompID.Text = "";
                    txtClearingFirm.Text = "";
                    cmbCompanyStrategy.Text = C_COMBO_SELECT;
                    txtGiveUpIdentifier.Text = "";
                    txtOnBehalfOfSubID.Text = "";
                    txtCMTAIdentifier.Text = "";
                    //
                }
            }
        }

        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }

        private int _companyCounterPartyVenueID = int.MinValue;
        public int CompanyCounterPartyVenueID
        {
            set
            {
                //				_companyCounterPartyVenueID = value;
                //				SetCompanyCounterPartyVenueDetails(_companyCounterPartyVenueID);
            }
            get
            {
                return _companyCounterPartyVenueID;
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            set
            {
                _companyID = value;
                BindCompanyCounterPartyVenues();
                BindClearingFirmPrimeBrokers();
                BindAccounts();
                BindStrategies();
                //BindIdentifiers();
                BindCompanyMPID();
            }
        }

        private int _companyCounterPartyID = int.MinValue;
        public int CompanyCounterPartyID
        {
            set
            {
                _companyCounterPartyID = value;
                CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail(_companyCounterPartyID);
                //lblCounterParty.Text = _companyCounterPartyID.ToString();
                lblCounterParty.Text = companyCounterPartyVenueDetail.CounterPartyFullName;
                BindCompanyCounterPartyVenues();
            }
        }

        public event EventHandler SaveData;
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            bool exists = false;
            if (_noData == 1)
            {
                _companyCounterPartyVenueDetails.Clear();
            }
            else
            {
                _noData = 0;
            }
            if (_companyCounterPartyVenueDetailEdit != null)
            {
                errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "");
                errorProvider1.SetError(cmbCompanyCounterPartyVenue, "");
                errorProvider1.SetError(txtSenderCompID, "");
                errorProvider1.SetError(txtTargetCompID, "");
                errorProvider1.SetError(txtGiveUpIdentifier, "");
                if (int.Parse(cmbCompanyCounterPartyVenue.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbCompanyCounterPartyVenue, "Please select Broker Venue!");
                    cmbCompanyCounterPartyVenue.Focus();
                }
                else if (int.Parse(cmbClearingFirmPrimeBrokers.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "Please select Clearing Firm Prime Brokers!");
                    cmbClearingFirmPrimeBrokers.Focus();
                }
                else if (txtSenderCompID.Text.Trim().ToString() == "")
                {
                    errorProvider1.SetError(txtSenderCompID, "Please enter Sender Comp ID!");
                    txtSenderCompID.Focus();
                }
                else if (txtTargetCompID.Text.Trim().ToString() == "")
                {
                    errorProvider1.SetError(txtTargetCompID, "Please enter Target Comp ID!");
                    txtTargetCompID.Focus();
                }


                else
                {
                    //_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
                    _companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = _companyCounterPartyID;
                    _companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID = int.Parse(cmbCompanyCounterPartyVenue.Value.ToString());
                    _companyCounterPartyVenueDetailEdit.ClearingFirmPrimeBrokerID = int.Parse(cmbClearingFirmPrimeBrokers.Value.ToString());

                    _companyCounterPartyVenueDetailEdit.MPID = int.Parse(cmbCompanyMPID.Value.ToString());

                    _companyCounterPartyVenueDetailEdit.SenderCompanyID = txtSenderCompID.Text.ToString();
                    _companyCounterPartyVenueDetailEdit.DeliverToCompanyID = txtDeliverCompID.Text.ToString();
                    _companyCounterPartyVenueDetailEdit.TargetCompID = txtTargetCompID.Text.ToString();
                    _companyCounterPartyVenueDetailEdit.ClearingFirm = txtClearingFirm.Text.ToString();
                    _companyCounterPartyVenueDetailEdit.AccountID = int.Parse(cmbCompanyAccounts.Value.ToString());
                    _companyCounterPartyVenueDetailEdit.StrategyID = int.Parse(cmbCompanyStrategy.Value.ToString());
                    if (int.Parse(cmbCMTAGiveUp.Value.ToString()) != int.MinValue)
                    {
                        _companyCounterPartyVenueDetailEdit.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.Value.ToString());
                    }
                    else
                    {
                        _companyCounterPartyVenueDetailEdit.CMTAGiveUp = int.Parse(System.DBNull.Value.ToString());
                    }

                    //Commented as per Shams comments
                    //_companyCounterPartyVenueDetailEdit.IdentifierName = txtGiveUpIdentifier.Text.ToString();

                    _companyCounterPartyVenueDetailEdit.OnBehalfOfSubID = txtOnBehalfOfSubID.Text.ToString();


                }
            }
            else
            {
                errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "");
                errorProvider1.SetError(cmbCompanyCounterPartyVenue, "");
                errorProvider1.SetError(txtSenderCompID, "");
                errorProvider1.SetError(txtTargetCompID, "");
                errorProvider1.SetError(txtGiveUpIdentifier, "");
                if (int.Parse(cmbCompanyCounterPartyVenue.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbCompanyCounterPartyVenue, "Please select Broker Venue!");
                    cmbCompanyCounterPartyVenue.Focus();
                }
                else if (int.Parse(cmbClearingFirmPrimeBrokers.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "Please select Clearing Firm Prime Brokers!");
                    cmbClearingFirmPrimeBrokers.Focus();
                }
                else if (txtSenderCompID.Text.Trim().ToString() == "")
                {
                    errorProvider1.SetError(txtSenderCompID, "Please enter Sender Comp ID!");
                    txtSenderCompID.Focus();
                }
                else if (txtTargetCompID.Text.Trim().ToString() == "")
                {
                    errorProvider1.SetError(txtTargetCompID, "Please enter Target Comp ID!");
                    txtTargetCompID.Focus();
                }


                else
                {
                    //					
                    Prana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Prana.Admin.BLL.CompanyCounterPartyVenueDetail();
                    //companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());

                    companyCounterPartyVenueDetail.CompanyCounterPartyID = _companyCounterPartyID;
                    companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(cmbCompanyCounterPartyVenue.Value.ToString());
                    companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(cmbClearingFirmPrimeBrokers.Value.ToString());

                    companyCounterPartyVenueDetail.MPID = int.Parse(cmbCompanyMPID.Value.ToString());

                    companyCounterPartyVenueDetail.SenderCompanyID = txtSenderCompID.Text.ToString();
                    companyCounterPartyVenueDetail.DeliverToCompanyID = txtDeliverCompID.Text.ToString();
                    companyCounterPartyVenueDetail.TargetCompID = txtTargetCompID.Text.ToString();
                    companyCounterPartyVenueDetail.ClearingFirm = txtClearingFirm.Text.ToString();
                    companyCounterPartyVenueDetail.AccountID = int.Parse(cmbCompanyAccounts.Value.ToString());
                    companyCounterPartyVenueDetail.StrategyID = int.Parse(cmbCompanyStrategy.Value.ToString());
                    companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.Value.ToString());
                    //Commented as per Shams comments
                    //companyCounterPartyVenueDetail.IdentifierName = txtGiveUpIdentifier.Text.ToString();

                    companyCounterPartyVenueDetail.OnBehalfOfSubID = txtOnBehalfOfSubID.Text.ToString();

                    int nullLocation = 0;
                    bool nullRow = false;
                    foreach (CompanyCounterPartyVenueDetail ckCompanyCounterPartyVenueDetail in _companyCounterPartyVenueDetails)
                    {
                        if (ckCompanyCounterPartyVenueDetail.CompanyCounterPartyVenueID == int.MinValue)
                        {
                            nullRow = true;
                            break;
                        }
                        nullLocation++;
                    }
                    if (nullRow == true)
                    {
                        _companyCounterPartyVenueDetails.RemoveAt(nullLocation);
                    }
                    foreach (CompanyCounterPartyVenueDetail checkCompanyCounterPartyVenueDetail in _companyCounterPartyVenueDetails)
                    {
                        if (checkCompanyCounterPartyVenueDetail.CompanyCounterPartyVenueID == companyCounterPartyVenueDetail.CompanyCounterPartyVenueID)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (exists == false)
                    {
                        _companyCounterPartyVenueDetails.Add(companyCounterPartyVenueDetail);
                        SaveData(_companyCounterPartyVenueDetails, e);

                        Refresh(sender, e);
                    }
                    else
                    {
                        errorProvider1.SetError(cmbCompanyCounterPartyVenue, "Details regarding selected CounterPartyVenue already exists in the grid.!");
                        cmbCompanyCounterPartyVenue.Focus();
                    }
                }
            }
            SaveData(_companyCounterPartyVenueDetails, e);
            //this.Hide();
        }

        public Prana.Admin.BLL.CompanyCounterPartyVenueDetail SaveCounterPartyVenueDetail()
        {
            CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = null;

            errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "");
            errorProvider1.SetError(cmbCompanyCounterPartyVenue, "");
            errorProvider1.SetError(txtSenderCompID, "");
            errorProvider1.SetError(txtTargetCompID, "");
            errorProvider1.SetError(txtGiveUpIdentifier, "");
            if (int.Parse(cmbCompanyCounterPartyVenue.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCompanyCounterPartyVenue, "Please select Broker Venue!");
                cmbCompanyCounterPartyVenue.Focus();
                return companyCounterPartyVenueDetail;
            }
            else if (int.Parse(cmbClearingFirmPrimeBrokers.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "Please select Clearing Firm Prime Brokers!");
                cmbClearingFirmPrimeBrokers.Focus();
                return companyCounterPartyVenueDetail;
            }
            else if (txtSenderCompID.Text.Trim().ToString() == "")
            {
                errorProvider1.SetError(txtSenderCompID, "Please enter Sender Comp ID!");
                txtSenderCompID.Focus();
                return companyCounterPartyVenueDetail;
            }
            else if (txtTargetCompID.Text.Trim().ToString() == "")
            {
                errorProvider1.SetError(txtTargetCompID, "Please enter Target Comp ID!");
                txtTargetCompID.Focus();
                return companyCounterPartyVenueDetail;
            }

            else
            {
                companyCounterPartyVenueDetail = new Prana.Admin.BLL.CompanyCounterPartyVenueDetail();

                companyCounterPartyVenueDetail.CompanyCounterPartyID = _companyCounterPartyID;
                companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(cmbCompanyCounterPartyVenue.Value.ToString());
                companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(cmbClearingFirmPrimeBrokers.Value.ToString());

                companyCounterPartyVenueDetail.MPID = int.Parse(cmbCompanyMPID.Value.ToString());

                companyCounterPartyVenueDetail.SenderCompanyID = txtSenderCompID.Text.ToString();
                companyCounterPartyVenueDetail.DeliverToCompanyID = txtDeliverCompID.Text.ToString();
                companyCounterPartyVenueDetail.TargetCompID = txtTargetCompID.Text.ToString();
                companyCounterPartyVenueDetail.ClearingFirm = txtClearingFirm.Text.ToString();
                companyCounterPartyVenueDetail.AccountID = int.Parse(cmbCompanyAccounts.Value.ToString());
                companyCounterPartyVenueDetail.StrategyID = int.Parse(cmbCompanyStrategy.Value.ToString());

                companyCounterPartyVenueDetail.CMTAIdentifier = txtCMTAIdentifier.Text.ToString();
                companyCounterPartyVenueDetail.GiveUpIdentifier = txtGiveUpIdentifier.Text.ToString();
                companyCounterPartyVenueDetail.OnBehalfOfSubID = txtOnBehalfOfSubID.Text.ToString();

            }
            return companyCounterPartyVenueDetail;
        }

        public void Refresh(object sender, System.EventArgs e)
        {
            cmbCounterParty.SelectedValue = int.MinValue;
            cmbCompanyCounterPartyVenue.Value = int.MinValue;
            cmbClearingFirmPrimeBrokers.Value = int.MinValue;
            txtDeliverCompID.Text = "";
            //txtDeliverSubID.Text = "";
            txtSenderCompID.Text = "";

            txtGiveUpIdentifier.Text = "";
            txtCMTAIdentifier.Text = "";
        }

        private void CreateCompanyCounterPartiesCompanyLevelTags_Load(object sender, System.EventArgs e)
        {
            if (_companyCounterPartyVenueDetailEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
            }
        }

        private Prana.Admin.BLL.CompanyCounterPartyVenueDetails _companyCounterPartyVenueDetails = new Prana.Admin.BLL.CompanyCounterPartyVenueDetails();

        private void cmbCounterParty_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _companyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
            //			CreateCompanyCounterPartyAccountLevelTags createCompanyCounterPartyAccountLevelTags = new  CreateCompanyCounterPartyAccountLevelTags();
            //			createCompanyCounterPartyAccountLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
            BindCompanyCounterPartyVenues();
        }

        private void cmbCounterParty_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCounterParty.SelectedValue != null)
            {
                _companyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
            }
        }

        public Prana.Admin.BLL.CompanyCounterPartyVenueDetails CurrentCompanyCounterPartyVenueDetails
        {
            get
            {
                return _companyCounterPartyVenueDetails;
            }
            set
            {
                if (value != null)
                {
                    _companyCounterPartyVenueDetails = value;
                    BindForEdit();
                }
            }
        }
        #region Focus Colors
        private void txtDeliverCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtDeliverCompID.BackColor = Color.LemonChiffon;
        }
        private void txtDeliverCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtDeliverCompID.BackColor = Color.White;
        }
        //		private void txtDeliverSubID_GotFocus(object sender, System.EventArgs e)
        //		{
        //			txtDeliverSubID.BackColor = Color.LemonChiffon;
        //		}
        //		private void txtDeliverSubID_LostFocus(object sender, System.EventArgs e)
        //		{
        //			txtDeliverSubID.BackColor = Color.White;
        //		}
        private void txtSenderCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtSenderCompID.BackColor = Color.LemonChiffon;
        }
        private void txtSenderCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtSenderCompID.BackColor = Color.White;
        }
        private void cmbClearingFirmPrimeBrokers_GotFocus(object sender, System.EventArgs e)
        {
            cmbClearingFirmPrimeBrokers.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbClearingFirmPrimeBrokers_LostFocus(object sender, System.EventArgs e)
        {
            cmbClearingFirmPrimeBrokers.Appearance.BackColor = Color.White;
        }
        private void cmbCompanyCounterPartyVenue_GotFocus(object sender, System.EventArgs e)
        {
            cmbCompanyCounterPartyVenue.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCompanyCounterPartyVenue_LostFocus(object sender, System.EventArgs e)
        {
            cmbCompanyCounterPartyVenue.Appearance.BackColor = Color.White;
        }

        private void cmbCompanyMPID_GotFocus(object sender, System.EventArgs e)
        {
            cmbCompanyMPID.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCompanyMPID_LostFocus(object sender, System.EventArgs e)
        {
            cmbCompanyMPID.Appearance.BackColor = Color.White;

        }
        private void cmbCompanyAccounts_GotFocus(object sender, System.EventArgs e)
        {
            cmbCompanyAccounts.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCompanyAccounts_LostFocus(object sender, System.EventArgs e)
        {
            cmbCompanyAccounts.Appearance.BackColor = Color.White;
        }

        private void cmbCompanyStrategy_GotFocus(object sender, System.EventArgs e)
        {
            cmbCompanyStrategy.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCompanyStrategy_LostFocus(object sender, System.EventArgs e)
        {
            cmbCompanyStrategy.Appearance.BackColor = Color.White;
        }

        private void cmbCMTAGiveUp_GotFocus(object sender, System.EventArgs e)
        {
            cmbCMTAGiveUp.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCMTAGiveUp_LostFocus(object sender, System.EventArgs e)
        {
            cmbCMTAGiveUp.Appearance.BackColor = Color.White;
        }
        private void txtTargetCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtTargetCompID.BackColor = Color.LemonChiffon;
        }
        private void txtTargetCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtTargetCompID.BackColor = Color.White;
        }
        private void txtClearingFirm_GotFocus(object sender, System.EventArgs e)
        {
            txtClearingFirm.BackColor = Color.LemonChiffon;
        }
        private void txtClearingFirm_LostFocus(object sender, System.EventArgs e)
        {
            txtClearingFirm.BackColor = Color.White;
        }
        private void txtIdentifier_GotFocus(object sender, System.EventArgs e)
        {
            txtGiveUpIdentifier.BackColor = Color.LemonChiffon;
        }
        private void txtIdentifier_LostFocus(object sender, System.EventArgs e)
        {
            txtGiveUpIdentifier.BackColor = Color.White;
        }
        private void txtOnBehalfOfSubID_GotFocus(object sender, System.EventArgs e)
        {
            txtOnBehalfOfSubID.BackColor = Color.LemonChiffon;
        }
        private void txtOnBehalfOfSubID_LostFocus(object sender, System.EventArgs e)
        {
            txtOnBehalfOfSubID.BackColor = Color.White;
        }
        #endregion

        private void CreateCompanyCounterPartiesCompanyLevelTags_Load_1(object sender, System.EventArgs e)
        {
            BindGiveUpGrid();
            BindCMTAGrid();
        }

        //int _companyCounterPartyCVID = int.MinValue;

        public void BindCMTAGrid()
        {
            CompanyCVCMTAIdentifiers companyCVCMTAIdentifiers = new CompanyCVCMTAIdentifiers();
            //companyCVCMTAIdentifiers = CompanyManager.GetCompanyCVCMTAIdentifiers(0);
            grdCMTA.DataSource = companyCVCMTAIdentifiers;
            AddNewCMTATempRow();
            RefreshCMTAGrid();
        }

        private void RefreshCMTAGrid()
        {
            if (grdCMTA.Rows.Count > 0)
            {
                grdCMTA.DisplayLayout.Bands[0].Columns["CMTAIdentifier"].Header.VisiblePosition = 0;
                grdCMTA.DisplayLayout.Bands[0].Columns["CMTAIdentifier"].Header.Caption = "CMTA Identifier";
                grdCMTA.DisplayLayout.Bands[0].Columns["CMTAIdentifier"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                grdCMTA.DisplayLayout.Bands[0].Columns["CMTAIdentifier"].MaxLength = 50;
                grdCMTA.DisplayLayout.Bands[0].Columns["CMTAIdentifier"].Width = 190;
                grdCMTA.DisplayLayout.Bands[0].Columns["CMTAIdentifier"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

                grdCMTA.DisplayLayout.Bands[0].Columns["CompanyCounterPartyVenueID"].Header.VisiblePosition = 1;
                grdCMTA.DisplayLayout.Bands[0].Columns["CompanyCounterPartyVenueID"].Hidden = true;

                grdCMTA.DisplayLayout.Bands[0].Columns["CompanyCVCMTAIdentifierID"].Header.VisiblePosition = 2;
                grdCMTA.DisplayLayout.Bands[0].Columns["CompanyCVCMTAIdentifierID"].Hidden = true;

            }
        }

        private void AddNewCMTATempRow()
        {
            CompanyCVCMTAIdentifiers companyCVCMTAIds = new CompanyCVCMTAIdentifiers();

            CompanyCVCMTAIdentifier comcvcmtaId = new CompanyCVCMTAIdentifier();
            comcvcmtaId.CompanyCounterPartyVenueId = int.MinValue;
            comcvcmtaId.CompanyCVCMTAIdentifierID = int.MinValue;
            comcvcmtaId.CMTAIdentifier = string.Empty;
            companyCVCMTAIds.Add(comcvcmtaId);
            grdCMTA.DataSource = companyCVCMTAIds;
        }


        public void BindGiveUpGrid()
        {
            CompanyCVGiveUpIdentifiers companyCVGiveUpIdentifiers = new CompanyCVGiveUpIdentifiers();
            //companyCVGiveUpIdentifiers = CompanyManager.GetCompanyCVGiveUpIdentifiers(0);
            grdGiveUp.DataSource = companyCVGiveUpIdentifiers;
            AddNewGiveUpTempRow();
            RefreshGiveUpGrid();
        }

        private void AddNewGiveUpTempRow()
        {
            CompanyCVGiveUpIdentifiers companyCVGiveUpIds = new CompanyCVGiveUpIdentifiers();

            CompanyCVGiveUpIdentifier comcvgiveupId = new CompanyCVGiveUpIdentifier();
            comcvgiveupId.CompanyCounterPartyVenueId = int.MinValue;
            comcvgiveupId.CompanyCVGiveUpIdentifierID = int.MinValue;
            comcvgiveupId.GiveUpIdentifier = string.Empty;
            companyCVGiveUpIds.Add(comcvgiveupId);
            grdGiveUp.DataSource = companyCVGiveUpIds;

        }

        private void RefreshGiveUpGrid()
        {
            if (grdGiveUp.Rows.Count > 0)
            {
                grdGiveUp.DisplayLayout.Bands[0].Columns["GiveUpIdentifier"].Header.VisiblePosition = 0;
                grdGiveUp.DisplayLayout.Bands[0].Columns["GiveUpIdentifier"].Header.Caption = "GiveUp Identifier";
                grdGiveUp.DisplayLayout.Bands[0].Columns["GiveUpIdentifier"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                grdGiveUp.DisplayLayout.Bands[0].Columns["GiveUpIdentifier"].MaxLength = 50;
                grdGiveUp.DisplayLayout.Bands[0].Columns["GiveUpIdentifier"].Width = 190;
                grdGiveUp.DisplayLayout.Bands[0].Columns["GiveUpIdentifier"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

                grdGiveUp.DisplayLayout.Bands[0].Columns["CompanyCounterPartyVenueID"].Header.VisiblePosition = 1;
                grdGiveUp.DisplayLayout.Bands[0].Columns["CompanyCounterPartyVenueID"].Hidden = true;

                grdGiveUp.DisplayLayout.Bands[0].Columns["CompanyCVGiveupIdentifierID"].Header.VisiblePosition = 2;
                grdGiveUp.DisplayLayout.Bands[0].Columns["CompanyCVGiveupIdentifierID"].Hidden = true;

            }
        }


        private void cmbCompanyCounterPartyVenue_ValueChanged(object sender, System.EventArgs e)
        {
            //			if(cmbCompanyCounterPartyVenue.Value != null)
            //			{
            //				_companyCounterPartyCVID = int.Parse(cmbCompanyCounterPartyVenue.Value.ToString());
            //				MessageBox.Show("CCPCVID..." + _companyCounterPartyCVID);
            //			}
        }
        string oldText = string.Empty;
        UltraGridCell nextActCell = null;
        private void grdGiveUp_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridCell prevActCell = grdGiveUp.ActiveCell;

                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();
                if ((iData.GetDataPresent(System.Windows.Forms.DataFormats.Text)))
                {
                    string str = iData.GetData(System.Windows.Forms.DataFormats.Text).ToString();
                }

                string updatedText = e.Cell.Text.ToString();
                int lenUpdatedText = updatedText.Length;
                int lenOldText = oldText.Length;
                if (lenUpdatedText > 1 && (prevActCell != nextActCell))
                {
                    //Do nothing.
                }
                else
                {
                    AddGiveUpNewRow();
                }
                oldText = updatedText;
                nextActCell = prevActCell;

            }
            catch (Exception)
            {


            }
        }

        private void AddGiveUpNewRow()
        {

            UltraGridCell prevActiveCell = grdGiveUp.Rows[0].Cells[0];
            string cellText = string.Empty;
            int len = int.MinValue;
            if (grdGiveUp.ActiveCell != null)
            {
                prevActiveCell = grdGiveUp.ActiveCell;
                cellText = prevActiveCell.Text;
                len = cellText.Length;
            }

            int rowsCount = grdGiveUp.Rows.Count;
            UltraGridRow dr = grdGiveUp.Rows[rowsCount - 1];

            CompanyCVGiveUpIdentifiers companygiveupidens = (CompanyCVGiveUpIdentifiers)grdGiveUp.DataSource;
            CompanyCVGiveUpIdentifier companygiveupiden = new CompanyCVGiveUpIdentifier();

            //The below varriable is taken from the last row of the grid before adding the new row.
            string strGiveupIdentifier = dr.Cells["GiveUpIdentifier"].Text.ToString();

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (strGiveupIdentifier != string.Empty)
            {
                companygiveupiden.CompanyCounterPartyVenueId = int.MinValue;
                companygiveupiden.CompanyCVGiveUpIdentifierID = int.MinValue;
                companygiveupiden.GiveUpIdentifier = string.Empty;

                companygiveupidens.Add(companygiveupiden);
                grdGiveUp.DataSource = companygiveupidens;
                grdGiveUp.DataBind();
                grdGiveUp.ActiveCell = prevActiveCell;
                grdGiveUp.Focus();
                grdGiveUp.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                if (len != int.MinValue)
                {
                    prevActiveCell.SelLength = 0;
                    prevActiveCell.SelStart = len + 1;
                }
            }

        }
        private void grdGiveUp_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            grdGiveUp.UpdateData();
            if (grdGiveUp.Rows.Count == 1)
            {
                //if ((grdGiveUp.ActiveRow.Cells["GiveUpIdentifier"].Text.ToString()) == string.Empty)
                //{
                //    MessageBox.Show("Please select atleast one record to delete.", "Prana Alert");
                e.Cancel = true;
                return;
                // }

            }
        }

        CompanyCVGiveUpIdentifiers _companyCVGiveUpIdentifiers = new CompanyCVGiveUpIdentifiers();
        public CompanyCVGiveUpIdentifiers companyCVGiveUpproperties
        {
            get
            {
                _companyCVGiveUpIdentifiers = (CompanyCVGiveUpIdentifiers)grdGiveUp.DataSource;

                CompanyCVGiveUpIdentifiers validGiveUpIdentifiers = new CompanyCVGiveUpIdentifiers();

                //Validation for in Case of Repeating Values       

                string strGiveUpIdentifier = string.Empty;
                for (int index3 = 0; index3 < grdGiveUp.Rows.Count; index3++)
                {
                    strGiveUpIdentifier = grdGiveUp.Rows[index3].Cells["GiveUpIdentifier"].Text.ToString();

                    int counter = 0;
                    for (int intTest = 0; intTest < grdGiveUp.Rows.Count; intTest++)
                    {
                        if (strGiveUpIdentifier != string.Empty)
                        {

                            if (strGiveUpIdentifier == grdGiveUp.Rows[intTest].Cells["GiveUpIdentifier"].Text.ToString())
                            {
                                counter = counter + 1;
                            }
                            if (counter > 1)
                            {
                                MessageBox.Show("GiveUp Identifier already exist.", "Prana Alert");
                                validGiveUpIdentifiers = null;
                                grdGiveUp.Focus();
                                return validGiveUpIdentifiers;
                            }
                        }
                    }


                }


                foreach (CompanyCVGiveUpIdentifier companyGiveUpIdentifier in _companyCVGiveUpIdentifiers)
                {
                    if (companyGiveUpIdentifier.GiveUpIdentifier != string.Empty)
                    {
                        validGiveUpIdentifiers.Add(companyGiveUpIdentifier);
                    }
                }
                return validGiveUpIdentifiers;
            }
            set
            {
                _companyCVGiveUpIdentifiers = value;

                if (_companyCVGiveUpIdentifiers.Count > 0)
                {
                    grdGiveUp.DataSource = _companyCVGiveUpIdentifiers;
                    AddGiveUpNewRow();
                }
                else
                {
                    grdGiveUp.DataSource = _companyCVGiveUpIdentifiers;
                    AddNewGiveUpTempRow();
                }
                RefreshGiveUpGrid();
            }

        }
        string CMTAoldText = string.Empty;
        UltraGridCell CMTAnextActCell = null;
        private void grdCMTA_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridCell prevActCell = grdCMTA.ActiveCell;

                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();
                if ((iData.GetDataPresent(System.Windows.Forms.DataFormats.Text)))
                {
                    string str = iData.GetData(System.Windows.Forms.DataFormats.Text).ToString();
                }

                string updatedText = e.Cell.Text.ToString();
                int lenUpdatedText = updatedText.Length;
                int lenOldText = oldText.Length;
                if (lenUpdatedText > 1 && (prevActCell != nextActCell))
                {
                    //Do nothing.
                }
                else
                {
                    AddCMTANewRow();
                }
                CMTAoldText = updatedText;
                CMTAnextActCell = prevActCell;

            }
            catch (Exception)
            {


            }
        }

        private void AddCMTANewRow()
        {

            UltraGridCell prevActiveCell = grdCMTA.Rows[0].Cells[0];
            string cellText = string.Empty;
            int len = int.MinValue;
            if (grdCMTA.ActiveCell != null)
            {
                prevActiveCell = grdCMTA.ActiveCell;
                cellText = prevActiveCell.Text;
                len = cellText.Length;
            }

            int rowsCount = grdCMTA.Rows.Count;
            UltraGridRow dr = grdCMTA.Rows[rowsCount - 1];

            CompanyCVCMTAIdentifiers companycmtaidens = (CompanyCVCMTAIdentifiers)grdCMTA.DataSource;
            CompanyCVCMTAIdentifier companycmtaiden = new CompanyCVCMTAIdentifier();

            //The below varriable is taken from the last row of the grid before adding the new row.
            string strCMTAIdentifier = dr.Cells["CMTAIdentifier"].Text.ToString();

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (strCMTAIdentifier != string.Empty)
            {
                companycmtaiden.CompanyCounterPartyVenueId = int.MinValue;
                companycmtaiden.CompanyCVCMTAIdentifierID = int.MinValue;
                companycmtaiden.CMTAIdentifier = string.Empty;

                companycmtaidens.Add(companycmtaiden);
                grdCMTA.DataSource = companycmtaidens;
                grdCMTA.DataBind();
                grdCMTA.ActiveCell = prevActiveCell;
                grdCMTA.Focus();
                grdCMTA.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                if (len != int.MinValue)
                {
                    prevActiveCell.SelLength = 0;
                    prevActiveCell.SelStart = len + 1;
                }
            }

        }

        private void grdCMTA_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            grdCMTA.UpdateData();
            if (grdCMTA.Rows.Count == 1)
            {
                //if ((grdGiveUp.ActiveRow.Cells["GiveUpIdentifier"].Text.ToString()) == string.Empty)
                //{
                //    MessageBox.Show("Please select atleast one record to delete.", "Prana Alert");
                e.Cancel = true;
                return;
                // }

            }
        }

        CompanyCVCMTAIdentifiers _companyCVCMTAIdentifiers = new CompanyCVCMTAIdentifiers();
        public CompanyCVCMTAIdentifiers companyCVCMTAproperties
        {
            get
            {
                _companyCVCMTAIdentifiers = (CompanyCVCMTAIdentifiers)grdCMTA.DataSource;

                CompanyCVCMTAIdentifiers validCMTAIdentifiers = new CompanyCVCMTAIdentifiers();

                //Validation for in Case of Repeating Values       

                string strCMTAIdentifier = string.Empty;
                for (int index3 = 0; index3 < grdCMTA.Rows.Count; index3++)
                {
                    strCMTAIdentifier = grdCMTA.Rows[index3].Cells["CMTAIdentifier"].Text.ToString();

                    int counter = 0;
                    for (int intTest = 0; intTest < grdCMTA.Rows.Count; intTest++)
                    {
                        if (strCMTAIdentifier != string.Empty)
                        {

                            if (strCMTAIdentifier == grdCMTA.Rows[intTest].Cells["CMTAIdentifier"].Text.ToString())
                            {
                                counter = counter + 1;
                            }
                            if (counter > 1)
                            {
                                MessageBox.Show("CMTA Identifier already exist. ", "Prana Alert");
                                validCMTAIdentifiers = null;
                                grdCMTA.Focus();
                                // return validCMTAIdentifiers;
                                return null;
                            }
                        }
                    }


                }


                foreach (CompanyCVCMTAIdentifier companyCMTAIdentifier in _companyCVCMTAIdentifiers)
                {
                    if (companyCMTAIdentifier.CMTAIdentifier != string.Empty)
                    {
                        validCMTAIdentifiers.Add(companyCMTAIdentifier);
                    }
                }
                return validCMTAIdentifiers;
            }
            set
            {
                _companyCVCMTAIdentifiers = value;
                grdCMTA.DataSource = _companyCVCMTAIdentifiers;
                if (_companyCVCMTAIdentifiers.Count > 0)
                {
                    AddCMTANewRow();
                }
                else
                {
                    grdCMTA.DataSource = null;
                    AddNewCMTATempRow();
                }
                RefreshCMTAGrid();
            }

        }




    }
}