#region Using

//

using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyCompliance.
    /// </summary>
    public class CompanyCompliance : System.Windows.Forms.UserControl
    {
        const string C_COMBO_SELECT = "- Select -";
        const int YES = 1;

        private System.Windows.Forms.GroupBox grpCompliance;
        private System.Windows.Forms.GroupBox grpBorrower;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label43;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbBaseCurrency;
        private System.Windows.Forms.Label lblBaseCurrency;
        private System.Windows.Forms.Label lblSupportMultipleCurrency;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbMultipleCurrency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbFixVersion;
        private System.Windows.Forms.Label lblFixVersion;
        private System.Windows.Forms.Label label4;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbFixCapability;
        private System.Windows.Forms.Label lblFixCapability;
        private System.Windows.Forms.CheckedListBox checkedListBoxCurrency;
        private System.Windows.Forms.Label label7;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdBorrowers;
        private System.Windows.Forms.Button btnBorrowerDelete;
        private IContainer components;

        public CompanyCompliance()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();


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
                if (grpCompliance != null)
                {
                    grpCompliance.Dispose();
                }
                if (grpBorrower != null)
                {
                    grpBorrower.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label43 != null)
                {
                    label43.Dispose();
                }
                if (cmbBaseCurrency != null)
                {
                    cmbBaseCurrency.Dispose();
                }
                if (lblBaseCurrency != null)
                {
                    lblBaseCurrency.Dispose();
                }
                if (lblSupportMultipleCurrency != null)
                {
                    lblSupportMultipleCurrency.Dispose();
                }
                if (cmbMultipleCurrency != null)
                {
                    cmbMultipleCurrency.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (cmbFixVersion != null)
                {
                    cmbFixVersion.Dispose();
                }
                if (lblFixVersion != null)
                {
                    lblFixVersion.Dispose();
                }
                if (checkedListBoxCurrency != null)
                {
                    checkedListBoxCurrency.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (grdBorrowers != null)
                {
                    grdBorrowers.Dispose();
                }
                if (btnBorrowerDelete != null)
                {
                    btnBorrowerDelete.Dispose();
                }
                if (nextActCell != null)
                {
                    nextActCell.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (cmbFixCapability != null)
                {
                    cmbFixCapability.Dispose();
                }
                if (lblFixCapability != null)
                {
                    lblFixCapability.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixCapabilityID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixVersion", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixID", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 4);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyCompliance));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("BorrowerName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("BorrowerShortName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FirmID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyTypeID", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Telephone", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactFirstName", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactLastName", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactTitle", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactEMail", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactTelephone", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactCell", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactFirstName", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactLastName", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactTitle", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactEMail", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactTelephone", 20);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactCell", 21);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TechnologyContactFirstName", 22);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TechnologyContactLastName", 23);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TechnologyContactTitle", 24);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TechnologyContactEMail", 25);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TechnologyContactTelephone", 26);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TechnologyContactCell", 27);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MPID", 28);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyMPID", 29);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("BaseCurrencyID", 30);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SupportsMultipleCurrency", 31);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixVersionID", 32);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixCapabilityID", 33);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyComplianceID", 34);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyBorrowerID", 35);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MultipleCurrencyID", 36);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyVenueID", 37);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueName", 38);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn51 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueShortName", 39);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueType", 40);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeZone", 41);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RegularTradingStartTime", 42);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn55 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RegularTradingEndTime", 43);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn56 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PreMarketTradingStartTime", 44);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn57 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PreMarketTradingEndTime", 45);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn58 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LunchTimeStartTime", 46);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn59 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LunchTimeEndTime", 47);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn60 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PostMarketTradingStartTime", 48);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn61 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PostMarketTradingEndTime", 49);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn62 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PreMarketCheck", 50);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn63 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PostMarketCheck", 51);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn64 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RegularTimeCheck", 52);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn65 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LunchTimeCheck", 53);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn66 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 54);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn67 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LoginName", 55);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn68 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Password", 56);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn69 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 57);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn70 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 58);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn71 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Zip", 59);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn72 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAllCurrencyID", 60);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn73 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("City", 61);
            this.grpCompliance = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkedListBoxCurrency = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFixCapability = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblFixCapability = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFixVersion = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblFixVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbMultipleCurrency = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblSupportMultipleCurrency = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.cmbBaseCurrency = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblBaseCurrency = new System.Windows.Forms.Label();
            this.grpBorrower = new System.Windows.Forms.GroupBox();
            this.btnBorrowerDelete = new System.Windows.Forms.Button();
            this.grdBorrowers = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpCompliance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFixCapability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFixVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMultipleCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBaseCurrency)).BeginInit();
            this.grpBorrower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBorrowers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCompliance
            // 
            this.grpCompliance.Controls.Add(this.label7);
            this.grpCompliance.Controls.Add(this.checkedListBoxCurrency);
            this.grpCompliance.Controls.Add(this.label4);
            this.grpCompliance.Controls.Add(this.cmbFixCapability);
            this.grpCompliance.Controls.Add(this.lblFixCapability);
            this.grpCompliance.Controls.Add(this.label2);
            this.grpCompliance.Controls.Add(this.cmbFixVersion);
            this.grpCompliance.Controls.Add(this.lblFixVersion);
            this.grpCompliance.Controls.Add(this.label1);
            this.grpCompliance.Controls.Add(this.cmbMultipleCurrency);
            this.grpCompliance.Controls.Add(this.lblSupportMultipleCurrency);
            this.grpCompliance.Controls.Add(this.label43);
            this.grpCompliance.Controls.Add(this.cmbBaseCurrency);
            this.grpCompliance.Controls.Add(this.lblBaseCurrency);
            this.grpCompliance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCompliance.Location = new System.Drawing.Point(4, 6);
            this.grpCompliance.Name = "grpCompliance";
            this.grpCompliance.Size = new System.Drawing.Size(264, 164);
            this.grpCompliance.TabIndex = 0;
            this.grpCompliance.TabStop = false;
            this.grpCompliance.Text = "Compliance";
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(12, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 16);
            this.label7.TabIndex = 92;
            this.label7.Text = "Select Currency";
            // 
            // checkedListBoxCurrency
            // 
            this.checkedListBoxCurrency.CheckOnClick = true;
            this.checkedListBoxCurrency.Enabled = false;
            this.checkedListBoxCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedListBoxCurrency.HorizontalExtent = 500;
            this.checkedListBoxCurrency.Location = new System.Drawing.Point(119, 60);
            this.checkedListBoxCurrency.Name = "checkedListBoxCurrency";
            this.checkedListBoxCurrency.Size = new System.Drawing.Size(126, 100);
            this.checkedListBoxCurrency.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(91, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(8, 8);
            this.label4.TabIndex = 4;
            this.label4.Text = "*";
            this.label4.Visible = false;
            // 
            // cmbFixCapability
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbFixCapability.DisplayLayout.Appearance = appearance1;
            this.cmbFixCapability.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbFixCapability.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbFixCapability.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbFixCapability.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFixCapability.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFixCapability.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbFixCapability.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFixCapability.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbFixCapability.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbFixCapability.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbFixCapability.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbFixCapability.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbFixCapability.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbFixCapability.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbFixCapability.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbFixCapability.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbFixCapability.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbFixCapability.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFixCapability.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbFixCapability.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbFixCapability.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbFixCapability.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbFixCapability.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbFixCapability.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbFixCapability.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbFixCapability.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFixCapability.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbFixCapability.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbFixCapability.DisplayMember = "";
            this.cmbFixCapability.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFixCapability.DropDownWidth = 0;
            this.cmbFixCapability.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbFixCapability.Location = new System.Drawing.Point(119, 186);
            this.cmbFixCapability.Name = "cmbFixCapability";
            this.cmbFixCapability.Size = new System.Drawing.Size(126, 21);
            this.cmbFixCapability.TabIndex = 5;
            this.cmbFixCapability.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbFixCapability.ValueMember = "";
            this.cmbFixCapability.Visible = false;
            // 
            // lblFixCapability
            // 
            this.lblFixCapability.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFixCapability.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFixCapability.Location = new System.Drawing.Point(15, 188);
            this.lblFixCapability.Name = "lblFixCapability";
            this.lblFixCapability.Size = new System.Drawing.Size(76, 16);
            this.lblFixCapability.TabIndex = 88;
            this.lblFixCapability.Text = "Fix Capability";
            this.lblFixCapability.Visible = false;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(89, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 8);
            this.label2.TabIndex = 87;
            this.label2.Text = "*";
            this.label2.Visible = false;
            // 
            // cmbFixVersion
            // 
            this.cmbFixVersion.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbFixVersion.DisplayLayout.Appearance = appearance13;
            this.cmbFixVersion.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn4.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            this.cmbFixVersion.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbFixVersion.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbFixVersion.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFixVersion.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFixVersion.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbFixVersion.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFixVersion.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbFixVersion.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbFixVersion.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbFixVersion.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbFixVersion.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbFixVersion.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbFixVersion.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbFixVersion.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbFixVersion.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbFixVersion.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbFixVersion.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFixVersion.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbFixVersion.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbFixVersion.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbFixVersion.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbFixVersion.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbFixVersion.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbFixVersion.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbFixVersion.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFixVersion.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbFixVersion.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbFixVersion.DisplayMember = "";
            this.cmbFixVersion.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFixVersion.DropDownWidth = 0;
            this.cmbFixVersion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbFixVersion.Location = new System.Drawing.Point(119, 164);
            this.cmbFixVersion.Name = "cmbFixVersion";
            this.cmbFixVersion.Size = new System.Drawing.Size(126, 21);
            this.cmbFixVersion.TabIndex = 4;
            this.cmbFixVersion.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbFixVersion.ValueMember = "";
            this.cmbFixVersion.Visible = false;
            // 
            // lblFixVersion
            // 
            this.lblFixVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFixVersion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFixVersion.Location = new System.Drawing.Point(15, 166);
            this.lblFixVersion.Name = "lblFixVersion";
            this.lblFixVersion.Size = new System.Drawing.Size(74, 16);
            this.lblFixVersion.TabIndex = 85;
            this.lblFixVersion.Text = "Fix VersionID";
            this.lblFixVersion.Visible = false;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(105, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 3;
            this.label1.Text = "*";
            // 
            // cmbMultipleCurrency
            // 
            this.cmbMultipleCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbMultipleCurrency.DisplayLayout.Appearance = appearance25;
            this.cmbMultipleCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridColumn6.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6});
            this.cmbMultipleCurrency.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbMultipleCurrency.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbMultipleCurrency.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMultipleCurrency.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMultipleCurrency.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbMultipleCurrency.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMultipleCurrency.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbMultipleCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbMultipleCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbMultipleCurrency.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbMultipleCurrency.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbMultipleCurrency.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbMultipleCurrency.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbMultipleCurrency.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbMultipleCurrency.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbMultipleCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbMultipleCurrency.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMultipleCurrency.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbMultipleCurrency.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbMultipleCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbMultipleCurrency.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbMultipleCurrency.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbMultipleCurrency.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbMultipleCurrency.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbMultipleCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbMultipleCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbMultipleCurrency.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbMultipleCurrency.DisplayMember = "";
            this.cmbMultipleCurrency.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbMultipleCurrency.DropDownWidth = 0;
            this.cmbMultipleCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbMultipleCurrency.Location = new System.Drawing.Point(119, 38);
            this.cmbMultipleCurrency.Name = "cmbMultipleCurrency";
            this.cmbMultipleCurrency.Size = new System.Drawing.Size(126, 21);
            this.cmbMultipleCurrency.TabIndex = 1;
            this.cmbMultipleCurrency.ValueMember = "";
            this.cmbMultipleCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbMultipleCurrency.ValueChanged += new System.EventHandler(this.cmbMultipleCurrency_ValueChanged);
            // 
            // lblSupportMultipleCurrency
            // 
            this.lblSupportMultipleCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSupportMultipleCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSupportMultipleCurrency.Location = new System.Drawing.Point(15, 40);
            this.lblSupportMultipleCurrency.Name = "lblSupportMultipleCurrency";
            this.lblSupportMultipleCurrency.Size = new System.Drawing.Size(94, 16);
            this.lblSupportMultipleCurrency.TabIndex = 1;
            this.lblSupportMultipleCurrency.Text = "Mulitple Currency";
            // 
            // label43
            // 
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(97, 18);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(8, 8);
            this.label43.TabIndex = 81;
            this.label43.Text = "*";
            // 
            // cmbBaseCurrency
            // 
            this.cmbBaseCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbBaseCurrency.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBaseCurrency.DisplayLayout.Appearance = appearance37;
            this.cmbBaseCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridColumn7.Header.VisiblePosition = 0;
            ultraGridColumn8.Header.VisiblePosition = 1;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.Header.VisiblePosition = 2;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.Header.VisiblePosition = 3;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.Header.VisiblePosition = 4;
            ultraGridColumn11.Hidden = true;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11});
            this.cmbBaseCurrency.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbBaseCurrency.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBaseCurrency.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBaseCurrency.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBaseCurrency.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.cmbBaseCurrency.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBaseCurrency.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBaseCurrency.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.cmbBaseCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBaseCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBaseCurrency.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBaseCurrency.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.cmbBaseCurrency.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBaseCurrency.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBaseCurrency.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBaseCurrency.DisplayLayout.Override.CellAppearance = appearance44;
            this.cmbBaseCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBaseCurrency.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBaseCurrency.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbBaseCurrency.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.cmbBaseCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBaseCurrency.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.cmbBaseCurrency.DisplayLayout.Override.RowAppearance = appearance47;
            this.cmbBaseCurrency.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBaseCurrency.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.cmbBaseCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBaseCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBaseCurrency.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBaseCurrency.DisplayMember = "";
            this.cmbBaseCurrency.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBaseCurrency.DropDownWidth = 0;
            this.cmbBaseCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbBaseCurrency.Location = new System.Drawing.Point(119, 16);
            this.cmbBaseCurrency.MaxDropDownItems = 12;
            this.cmbBaseCurrency.Name = "cmbBaseCurrency";
            this.cmbBaseCurrency.Size = new System.Drawing.Size(126, 21);
            this.cmbBaseCurrency.TabIndex = 2;
            this.cmbBaseCurrency.ValueMember = "";
            this.cmbBaseCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbBaseCurrency.ValueChanged += new System.EventHandler(this.cmbBaseCurrency_ValueChanged);
            // 
            // lblBaseCurrency
            // 
            this.lblBaseCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblBaseCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBaseCurrency.Location = new System.Drawing.Point(15, 18);
            this.lblBaseCurrency.Name = "lblBaseCurrency";
            this.lblBaseCurrency.Size = new System.Drawing.Size(82, 16);
            this.lblBaseCurrency.TabIndex = 0;
            this.lblBaseCurrency.Text = "Base Currency";
            // 
            // grpBorrower
            // 
            this.grpBorrower.Controls.Add(this.btnBorrowerDelete);
            this.grpBorrower.Controls.Add(this.grdBorrowers);
            this.grpBorrower.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpBorrower.Location = new System.Drawing.Point(4, 178);
            this.grpBorrower.Name = "grpBorrower";
            this.grpBorrower.Size = new System.Drawing.Size(348, 156);
            this.grpBorrower.TabIndex = 0;
            this.grpBorrower.TabStop = false;
            this.grpBorrower.Text = "Borrower";
            // 
            // btnBorrowerDelete
            // 
            this.btnBorrowerDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBorrowerDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnBorrowerDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBorrowerDelete.BackgroundImage")));
            this.btnBorrowerDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBorrowerDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnBorrowerDelete.Location = new System.Drawing.Point(137, 124);
            this.btnBorrowerDelete.Name = "btnBorrowerDelete";
            this.btnBorrowerDelete.Size = new System.Drawing.Size(75, 23);
            this.btnBorrowerDelete.TabIndex = 0;
            this.btnBorrowerDelete.UseVisualStyleBackColor = false;
            this.btnBorrowerDelete.Click += new System.EventHandler(this.btnBorrowerDelete_Click);
            // 
            // grdBorrowers
            // 
            this.grdBorrowers.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn12.Header.Caption = "Name";
            ultraGridColumn12.Header.VisiblePosition = 0;
            ultraGridColumn12.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridColumn12.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn12.Width = 60;
            ultraGridColumn13.Header.Caption = "ShortName";
            ultraGridColumn13.Header.VisiblePosition = 1;
            ultraGridColumn13.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridColumn13.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn13.Width = 148;
            ultraGridColumn14.Header.VisiblePosition = 2;
            ultraGridColumn14.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridColumn14.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn14.Width = 99;
            ultraGridColumn15.Header.VisiblePosition = 3;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn15.Width = 8;
            ultraGridColumn16.Header.VisiblePosition = 4;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn16.Width = 8;
            ultraGridColumn17.Header.VisiblePosition = 5;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn17.Width = 8;
            ultraGridColumn18.Header.VisiblePosition = 6;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn18.Width = 8;
            ultraGridColumn19.Header.VisiblePosition = 7;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn19.Width = 8;
            ultraGridColumn20.Header.VisiblePosition = 8;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn20.Width = 8;
            ultraGridColumn21.Header.VisiblePosition = 9;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn21.Width = 8;
            ultraGridColumn22.Header.VisiblePosition = 10;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn22.Width = 8;
            ultraGridColumn23.Header.VisiblePosition = 11;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn23.Width = 8;
            ultraGridColumn24.Header.VisiblePosition = 12;
            ultraGridColumn24.Hidden = true;
            ultraGridColumn24.Width = 8;
            ultraGridColumn25.Header.VisiblePosition = 13;
            ultraGridColumn25.Hidden = true;
            ultraGridColumn25.Width = 8;
            ultraGridColumn26.Header.VisiblePosition = 14;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn26.Width = 8;
            ultraGridColumn27.Header.VisiblePosition = 15;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn27.Width = 8;
            ultraGridColumn28.Header.VisiblePosition = 16;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn28.Width = 8;
            ultraGridColumn29.Header.VisiblePosition = 17;
            ultraGridColumn29.Hidden = true;
            ultraGridColumn29.Width = 8;
            ultraGridColumn30.Header.VisiblePosition = 18;
            ultraGridColumn30.Hidden = true;
            ultraGridColumn30.Width = 8;
            ultraGridColumn31.Header.VisiblePosition = 19;
            ultraGridColumn31.Hidden = true;
            ultraGridColumn31.Width = 8;
            ultraGridColumn32.Header.VisiblePosition = 20;
            ultraGridColumn32.Hidden = true;
            ultraGridColumn32.Width = 8;
            ultraGridColumn33.Header.VisiblePosition = 21;
            ultraGridColumn33.Hidden = true;
            ultraGridColumn33.Width = 8;
            ultraGridColumn34.Header.VisiblePosition = 22;
            ultraGridColumn34.Hidden = true;
            ultraGridColumn34.Width = 8;
            ultraGridColumn35.Header.VisiblePosition = 23;
            ultraGridColumn35.Hidden = true;
            ultraGridColumn35.Width = 8;
            ultraGridColumn36.Header.VisiblePosition = 24;
            ultraGridColumn36.Hidden = true;
            ultraGridColumn36.Width = 8;
            ultraGridColumn37.Header.VisiblePosition = 25;
            ultraGridColumn37.Hidden = true;
            ultraGridColumn37.Width = 8;
            ultraGridColumn38.Header.VisiblePosition = 26;
            ultraGridColumn38.Hidden = true;
            ultraGridColumn38.Width = 8;
            ultraGridColumn39.Header.VisiblePosition = 27;
            ultraGridColumn39.Hidden = true;
            ultraGridColumn39.Width = 8;
            ultraGridColumn40.Header.VisiblePosition = 28;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn40.Width = 8;
            ultraGridColumn41.Header.VisiblePosition = 29;
            ultraGridColumn41.Hidden = true;
            ultraGridColumn41.Width = 8;
            ultraGridColumn42.Header.VisiblePosition = 30;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn42.Width = 8;
            ultraGridColumn43.Header.VisiblePosition = 31;
            ultraGridColumn43.Hidden = true;
            ultraGridColumn43.Width = 8;
            ultraGridColumn44.Header.VisiblePosition = 32;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn44.Width = 8;
            ultraGridColumn45.Header.VisiblePosition = 33;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn45.Width = 8;
            ultraGridColumn46.Header.VisiblePosition = 34;
            ultraGridColumn46.Hidden = true;
            ultraGridColumn46.Width = 8;
            ultraGridColumn47.Header.VisiblePosition = 35;
            ultraGridColumn47.Hidden = true;
            ultraGridColumn47.Width = 8;
            ultraGridColumn48.Header.VisiblePosition = 36;
            ultraGridColumn48.Hidden = true;
            ultraGridColumn48.Width = 8;
            ultraGridColumn49.Header.VisiblePosition = 37;
            ultraGridColumn49.Hidden = true;
            ultraGridColumn49.Width = 8;
            ultraGridColumn50.Header.VisiblePosition = 38;
            ultraGridColumn50.Hidden = true;
            ultraGridColumn50.Width = 8;
            ultraGridColumn51.Header.VisiblePosition = 39;
            ultraGridColumn51.Hidden = true;
            ultraGridColumn51.Width = 8;
            ultraGridColumn52.Header.VisiblePosition = 40;
            ultraGridColumn52.Hidden = true;
            ultraGridColumn52.Width = 8;
            ultraGridColumn53.Header.VisiblePosition = 41;
            ultraGridColumn53.Hidden = true;
            ultraGridColumn53.Width = 8;
            ultraGridColumn54.Header.VisiblePosition = 42;
            ultraGridColumn54.Hidden = true;
            ultraGridColumn54.Width = 8;
            ultraGridColumn55.Header.VisiblePosition = 43;
            ultraGridColumn55.Hidden = true;
            ultraGridColumn55.Width = 8;
            ultraGridColumn56.Header.VisiblePosition = 44;
            ultraGridColumn56.Hidden = true;
            ultraGridColumn56.Width = 8;
            ultraGridColumn57.Header.VisiblePosition = 45;
            ultraGridColumn57.Hidden = true;
            ultraGridColumn57.Width = 8;
            ultraGridColumn58.Header.VisiblePosition = 46;
            ultraGridColumn58.Hidden = true;
            ultraGridColumn58.Width = 8;
            ultraGridColumn59.Header.VisiblePosition = 47;
            ultraGridColumn59.Hidden = true;
            ultraGridColumn59.Width = 8;
            ultraGridColumn60.Header.VisiblePosition = 48;
            ultraGridColumn60.Hidden = true;
            ultraGridColumn60.Width = 8;
            ultraGridColumn61.Header.VisiblePosition = 49;
            ultraGridColumn61.Hidden = true;
            ultraGridColumn61.Width = 8;
            ultraGridColumn62.Header.VisiblePosition = 50;
            ultraGridColumn62.Hidden = true;
            ultraGridColumn62.Width = 8;
            ultraGridColumn63.Header.VisiblePosition = 51;
            ultraGridColumn63.Hidden = true;
            ultraGridColumn63.Width = 8;
            ultraGridColumn64.Header.VisiblePosition = 52;
            ultraGridColumn64.Hidden = true;
            ultraGridColumn64.Width = 8;
            ultraGridColumn65.Header.VisiblePosition = 53;
            ultraGridColumn65.Hidden = true;
            ultraGridColumn65.Width = 8;
            ultraGridColumn66.Header.VisiblePosition = 54;
            ultraGridColumn66.Hidden = true;
            ultraGridColumn66.Width = 20;
            ultraGridColumn67.Header.VisiblePosition = 55;
            ultraGridColumn67.Hidden = true;
            ultraGridColumn67.Width = 25;
            ultraGridColumn68.Header.VisiblePosition = 56;
            ultraGridColumn68.Hidden = true;
            ultraGridColumn68.Width = 32;
            ultraGridColumn69.Header.VisiblePosition = 57;
            ultraGridColumn69.Hidden = true;
            ultraGridColumn69.Width = 42;
            ultraGridColumn70.Header.VisiblePosition = 58;
            ultraGridColumn70.Hidden = true;
            ultraGridColumn70.Width = 53;
            ultraGridColumn71.Header.VisiblePosition = 59;
            ultraGridColumn71.Hidden = true;
            ultraGridColumn71.Width = 75;
            ultraGridColumn72.Header.VisiblePosition = 60;
            ultraGridColumn72.Hidden = true;
            ultraGridColumn72.Width = 75;
            ultraGridColumn73.Header.VisiblePosition = 61;
            ultraGridColumn73.Hidden = true;
            ultraGridColumn73.Width = 75;
            ultraGridBand5.Columns.AddRange(new object[] {
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
            ultraGridColumn27,
            ultraGridColumn28,
            ultraGridColumn29,
            ultraGridColumn30,
            ultraGridColumn31,
            ultraGridColumn32,
            ultraGridColumn33,
            ultraGridColumn34,
            ultraGridColumn35,
            ultraGridColumn36,
            ultraGridColumn37,
            ultraGridColumn38,
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
            ultraGridColumn72,
            ultraGridColumn73});
            ultraGridBand5.Header.Enabled = false;
            ultraGridBand5.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand5.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdBorrowers.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.grdBorrowers.DisplayLayout.GroupByBox.Hidden = true;
            this.grdBorrowers.DisplayLayout.MaxColScrollRegions = 1;
            this.grdBorrowers.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdBorrowers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdBorrowers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdBorrowers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdBorrowers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdBorrowers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdBorrowers.Location = new System.Drawing.Point(10, 20);
            this.grdBorrowers.Name = "grdBorrowers";
            this.grdBorrowers.Size = new System.Drawing.Size(328, 100);
            this.grdBorrowers.TabIndex = 1;
            this.grdBorrowers.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grdBorrowers.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdBorrowers.AfterCellActivate += new System.EventHandler(this.grdBorrowers_AfterCellActivate);
            this.grdBorrowers.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBorrowers_CellChange);
            this.grdBorrowers.Click += new System.EventHandler(this.grdBorrowers_Click);
            this.grdBorrowers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdBorrowers_KeyDown);
            this.grdBorrowers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdBorrowers_MouseClick);
            this.grdBorrowers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdBorrowers_MouseDown);
            this.grdBorrowers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdBorrowers_MouseUp);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CompanyCompliance
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpBorrower);
            this.Controls.Add(this.grpCompliance);
            this.Name = "CompanyCompliance";
            this.Size = new System.Drawing.Size(362, 339);
            this.grpCompliance.ResumeLayout(false);
            this.grpCompliance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFixCapability)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFixVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMultipleCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBaseCurrency)).EndInit();
            this.grpBorrower.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBorrowers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private int _complianceBaseCurrencyID = int.MinValue;
        public int ComplianceBaseCurrencyID
        {
            get
            {
                _complianceBaseCurrencyID = int.Parse(cmbBaseCurrency.Value.ToString());
                return _complianceBaseCurrencyID;
            }
            set
            {
                _complianceBaseCurrencyID = value;
            }
        }

        private int _supportsMultipleCurrencyID = int.MinValue;
        public int SupportsMultipleCurrencyID
        {
            get
            {
                _supportsMultipleCurrencyID = int.Parse(cmbMultipleCurrency.Value.ToString());
                return _supportsMultipleCurrencyID;
            }
            set
            {
                _supportsMultipleCurrencyID = value;
            }
        }

        /// <summary>
        /// This method binds the existing <see cref="Currencies"/> in the ComboBox control by assigning the 
        /// currencies object to its datasource property.
        /// </summary>
        private void BindBaseCurrency()
        {
            //GetCurrencies method fetches the existing currencies from the database.
            Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();
            //Inserting the - Select - option in the Combo Box at the top.
            currencies.Insert(0, new Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
            cmbBaseCurrency.DataSource = null;
            cmbBaseCurrency.DataSource = currencies;
            cmbBaseCurrency.DisplayMember = "CurrencySymbol";
            cmbBaseCurrency.ValueMember = "CurencyID";
            cmbBaseCurrency.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbBaseCurrency.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("CurrencySymbol"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            cmbBaseCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        /// <summary>
        /// This method binds the ComboBox control by assigning the detatable having default values yes/no.
        /// </summary>
        private void BindSupportsMultipleCurrency()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Data");
            dt.Columns.Add("Value");
            object[] row = new object[2];
            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            dt.Rows.Add(row);
            row[0] = "Yes";
            row[1] = "1";
            dt.Rows.Add(row);
            row[0] = "No";
            row[1] = "0";
            dt.Rows.Add(row);
            cmbMultipleCurrency.DataSource = null;
            cmbMultipleCurrency.DataSource = dt;
            cmbMultipleCurrency.DisplayMember = "Data";
            cmbMultipleCurrency.ValueMember = "Value";
            cmbMultipleCurrency.Text = C_COMBO_SELECT;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbMultipleCurrency.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("Data"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            cmbMultipleCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        /// <summary>
        /// This method binds the existing <see cref="Currencies"/> in the Checklist box control by assigning the 
        /// currencies object to its datasource property.
        /// </summary>
        private void BindMultipleCurrency()
        {
            if (cmbBaseCurrency.Value != null)
            {
                int currencyID = int.Parse(cmbBaseCurrency.Value.ToString());

                //GetCurrencies method fetches the existing currencies from the database.
                Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();
                if (currencyID != int.MinValue)
                {
                    int index = 0;
                    foreach (Currency baseCurrency in currencies)
                    {
                        if (baseCurrency.CurencyID == currencyID)
                        {
                            break;
                        }
                        index += 1;
                    }
                    currencies.RemoveAt(index);
                }

                checkedListBoxCurrency.DataSource = currencies;
                checkedListBoxCurrency.DisplayMember = "CurrencySymbol";
                checkedListBoxCurrency.ValueMember = "CurencyID";
            }
        }

        //User property gets the CompanyComplaince details from the form. 
        public Prana.Admin.BLL.Company CompanyComplaince
        {
            get { return GetCompanyComplianceDetails(); }
        }

        //Property gets the CompanyComplainceCurrency details from the form. 
        public Prana.Admin.BLL.Companies CompanyComplainceCurrency
        {
            get { return GetCompanyComplianceCurrencyDetails(); }
        }

        //Property gets the CompanyComplainceCurrency details from the form. 
        public Companies CompanyComplainceBorrowers
        {
            get { return GetCompanyBorrowerDetails(); }
        }

        public void RefreshComplianceDetail()
        {
            cmbBaseCurrency.Text = C_COMBO_SELECT;
            cmbMultipleCurrency.Text = C_COMBO_SELECT;
            Companies companyBorrowers = new Companies();
            companyBorrowers.Add(new Prana.Admin.BLL.Company("", "", ""));
            grdBorrowers.DataSource = companyBorrowers;
        }

        private Companies GetCompanyBorrowerDetails()
        {
            Companies companyBorrowerDetails = (Companies)grdBorrowers.DataSource;
            Companies validcompanyBorrowerDetails = new Companies();

            int count = companyBorrowerDetails.Count;
            int index = 0;
            string borrowerShortName = string.Empty;
            string borrowerShortNameTemp = string.Empty;
            foreach (Prana.Admin.BLL.Company companyBorrower in companyBorrowerDetails)
            {
                //				if(companyBorrower.BorrowerName != "" && companyBorrower.BorrowerShortName != "" && companyBorrower.FirmID != "")
                //				{
                //					validcompanyBorrowerDetails.Add(companyBorrower);
                //				}
                borrowerShortName = companyBorrower.BorrowerShortName.ToString();
                if (borrowerShortName == borrowerShortNameTemp && borrowerShortName != "")
                {
                    MessageBox.Show("Borrower with the short name already exists");
                    validcompanyBorrowerDetails = null;
                    return validcompanyBorrowerDetails;
                }
                if (companyBorrower.BorrowerName == "")
                {
                    MessageBox.Show("Please enter the Borrower Name in the row: " + (index + 1));
                    validcompanyBorrowerDetails = null;
                    return validcompanyBorrowerDetails;
                }
                else if (companyBorrower.BorrowerShortName == "")
                {
                    MessageBox.Show("Please enter the Borrower Short Name in the row: " + (index + 1));
                    validcompanyBorrowerDetails = null;
                    return validcompanyBorrowerDetails;
                }
                else if (companyBorrower.FirmID == "")
                {
                    MessageBox.Show("Please enter the Firm ID in the row: " + (index + 1));
                    validcompanyBorrowerDetails = null;
                    return validcompanyBorrowerDetails;
                }
                else
                {
                    validcompanyBorrowerDetails.Add(companyBorrower);
                }
                index++;
                if (index == (count - 1))
                {
                    break;
                }
                borrowerShortNameTemp = borrowerShortName;
            }
            return validcompanyBorrowerDetails;
        }


        private Prana.Admin.BLL.Company GetCompanyComplianceDetails()
        {
            Prana.Admin.BLL.Company companyCompliance = null;

            //companyCompliance = new Prana.Admin.BLL.Company();
            //companyCompliance.FixVersionID = int.Parse(cmbFixVersion.Value.ToString());
            //companyCompliance.FixCapabilityID = int.Parse(cmbFixCapability.Value.ToString());
            return companyCompliance;
        }

        private Companies GetCompanyComplianceCurrencyDetails()
        {
            errorProvider1.SetError(cmbBaseCurrency, "");
            errorProvider1.SetError(cmbMultipleCurrency, "");
            errorProvider1.SetError(checkedListBoxCurrency, "");

            Companies companyComplianceCurrencies = null;

            if (int.Parse(cmbBaseCurrency.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbBaseCurrency, "Please select base currency!");
                cmbBaseCurrency.Focus();
            }
            else if (int.Parse(cmbMultipleCurrency.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbMultipleCurrency, "Please select multiple currency!");
                cmbMultipleCurrency.Focus();
            }
            else if (int.Parse(cmbMultipleCurrency.Value.ToString()) == YES && checkedListBoxCurrency.CheckedIndices.Count == 0)
            {
                //				if(checkedListBoxCurrency.CheckedIndices.Count == 0)
                //				{
                errorProvider1.SetError(checkedListBoxCurrency, "Please select Multiple Currency!");
                checkedListBoxCurrency.Focus();
                //				}
            }
            else
            {
                companyComplianceCurrencies = new Companies();
                int baseCurrencyID = int.Parse(cmbBaseCurrency.Value.ToString());
                int supportsMultipleCurrency = int.Parse(cmbMultipleCurrency.Value.ToString());
                int multipleCurrencyID = int.MinValue;
                //companyComplianceCurrencies.Add(new Prana.Admin.BLL.Company(baseCurrencyID, supportsMultipleCurrency, multipleCurrencyID));
                companyComplianceCurrencies.Add(new Prana.Admin.BLL.Company(baseCurrencyID));

                if (supportsMultipleCurrency == YES)
                {
                    for (int i = 0, count = checkedListBoxCurrency.CheckedItems.Count; i < count; i++)
                    {
                        multipleCurrencyID = int.Parse(((Prana.Admin.BLL.Currency)(checkedListBoxCurrency.CheckedItems[i])).CurencyID.ToString());
                        companyComplianceCurrencies.Add(new Prana.Admin.BLL.Company((multipleCurrencyID)));
                    }
                }
            }

            return companyComplianceCurrencies;
        }

        private Companies CompanyComplianceBorrowers()
        {
            Companies companyComplianceBorrowers = (Companies)grdBorrowers.DataSource;
            return companyComplianceBorrowers;
        }

        public int _companyID = int.MinValue;
        public void SetupControl(int companyID)
        {
            _companyID = companyID;

            BindBaseCurrency();
            BindSupportsMultipleCurrency();
            BindMultipleCurrency();

            SetCompanyComplianceDetails();
            SetCompanyComplianceCurrencyDetails();
            SetCompanyBorrowerDetails();
        }

        public void SetCompanyComplianceDetails()
        {
            //Prana.Admin.BLL.Company companyCompliance = CompanyManager.GetCompanyCompliance(_companyID);
        }

        public void SetCompanyComplianceCurrencyDetails()
        {
            Prana.Admin.BLL.Companies companyComplianceCurrencies = CompanyManager.GetCompanyComplianceCurrencies(_companyID);

            //foreach(Prana.Admin.BLL.Company companyComplianceCurrency in companyComplianceCurrencies)
            //{
            //    if(int.Parse(companyComplianceCurrency.MultipleCurrencyID.ToString()) == int.MinValue)
            //    {
            //        cmbBaseCurrency.Value = int.Parse(companyComplianceCurrency.BaseCurrencyID.ToString());
            //        cmbMultipleCurrency.Value = int.Parse(companyComplianceCurrency.SupportsMultipleCurrency.ToString());	
            //    }
            //}
            cmbBaseCurrency.Value = _complianceBaseCurrencyID;
            cmbMultipleCurrency.Value = _supportsMultipleCurrencyID;

            if (companyComplianceCurrencies.Count >= 1)
            {
                bool flag = false;
                int location = 0;
                foreach (Prana.Admin.BLL.Company company in companyComplianceCurrencies)
                {
                    if (company.BaseCurrencyID == int.Parse(cmbBaseCurrency.Value.ToString()))
                    {
                        flag = true;
                        break;
                    }
                    location++;
                }
                if (flag == true)
                {
                    companyComplianceCurrencies.RemoveAt(location);
                }
            }

            for (int j = 0; j < checkedListBoxCurrency.Items.Count; j++)
            {
                checkedListBoxCurrency.SetItemChecked(j, false);
            }

            if (companyComplianceCurrencies.Count > 0)
            {
                foreach (Prana.Admin.BLL.Company companyComplianceCurrency in companyComplianceCurrencies)
                {
                    if (int.Parse(companyComplianceCurrency.MultipleCurrencyID.ToString()) != int.MinValue)
                    {
                        for (int j = 0; j < checkedListBoxCurrency.Items.Count; j++)
                        {
                            if (((Prana.Admin.BLL.Currency)checkedListBoxCurrency.Items[j]).CurencyID == int.Parse(companyComplianceCurrency.MultipleCurrencyID.ToString()))
                            {
                                checkedListBoxCurrency.SetItemChecked(j, true);
                            }
                        }
                    }
                }
            }
        }

        public void SetCompanyBorrowerDetails()
        {
            Companies companyComplianceBorrowers = CompanyManager.GetCompanyBorrowers(_companyID);
            if (companyComplianceBorrowers.Count == 0)
            {
                companyComplianceBorrowers.Add(new Prana.Admin.BLL.Company("", "", ""));
            }
            grdBorrowers.DataSource = companyComplianceBorrowers;
            AddNewRow();

            //ColumnsCollection columnsCBD = grdBorrowers.DisplayLayout.Bands[0].Columns;
            //foreach (UltraGridColumn column in columnsCBD)
            //{
            //    if (column.Key != "AccountName")
            //    {
            //        column.Hidden = true;
            //    }
            //}
        }

        private void cmbMultipleCurrency_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbMultipleCurrency.Value != null)
            {
                int supportsMutpleCurrency = int.Parse(cmbMultipleCurrency.Value.ToString());
                if (supportsMutpleCurrency == YES)
                {
                    checkedListBoxCurrency.Enabled = true;
                    BindMultipleCurrency();
                }
                else
                {
                    checkedListBoxCurrency.Enabled = false;
                }
            }
        }

        private void cmbBaseCurrency_ValueChanged(object sender, System.EventArgs e)
        {
            //			if(checkedListBoxCurrency.Enabled == true)
            //			{
            BindMultipleCurrency();
            //			}
        }

        private void btnBorrowerDelete_Click(object sender, System.EventArgs e)
        {
            int companyBorrowerID = int.MinValue;
            if (grdBorrowers.Rows.Count > 0)
            {
                companyBorrowerID = int.Parse(grdBorrowers.ActiveRow.Cells["CompanyBorrowerID"].Value.ToString());
                string companyBorrowerName = grdBorrowers.ActiveRow.Cells["BorrowerName"].Value.ToString();
                string companyBorrowerShortName = grdBorrowers.ActiveRow.Cells["BorrowerShortName"].Value.ToString();
                string firmID = grdBorrowers.ActiveRow.Cells["FirmID"].Value.ToString();
                if (companyBorrowerName != "" || companyBorrowerShortName != "" || firmID != "")
                {
                    if (MessageBox.Show(this, "Do you want to delete the selected Borrower?", "Borrower Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (companyBorrowerID != int.MinValue)
                        {

                            bool result = CompanyManager.DeleteCompanyBorrower(companyBorrowerID);
                            if (result == true)
                            {
                                //Company Borrower Deleted.
                                SetCompanyBorrowerDetails();
                            }
                            else
                            {
                                //Company Borrower not deleted.
                            }

                        }
                        else
                        {
                            Companies companyComplianceBorrowers = (Companies)grdBorrowers.DataSource;
                            Prana.Admin.BLL.Company companyComplianceBorrower = new Prana.Admin.BLL.Company();
                            companyComplianceBorrower.BorrowerName = grdBorrowers.ActiveRow.Cells["BorrowerName"].Text.ToString();
                            companyComplianceBorrower.BorrowerShortName = grdBorrowers.ActiveRow.Cells["BorrowerShortName"].Text.ToString();
                            companyComplianceBorrower.FirmID = grdBorrowers.ActiveRow.Cells["FirmID"].Text.ToString();
                            companyComplianceBorrower.CompanyID = int.Parse(grdBorrowers.ActiveRow.Cells["CompanyID"].Text.ToString());

                            companyComplianceBorrowers.RemoveAt(companyComplianceBorrowers.IndexOf(companyComplianceBorrower));
                            //companyComplianceBorrowers.Remove(companyComplianceBorrower);
                            Companies newCompanyComplianceBorrowers = new Companies();
                            foreach (Prana.Admin.BLL.Company tempCompanyComplianceBorrower in companyComplianceBorrowers)
                            {
                                newCompanyComplianceBorrowers.Add(tempCompanyComplianceBorrower);
                            }
                            if (companyComplianceBorrowers.Count <= 0)
                            {
                                newCompanyComplianceBorrowers.Add(new Prana.Admin.BLL.Company("", "", ""));
                            }
                            grdBorrowers.DataSource = newCompanyComplianceBorrowers;
                        }
                    }
                }
            }

        }

        string oldText = string.Empty;
        UltraGridCell nextActCell = null;
        private void grdBorrowers_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            UltraGridCell prevActCell = grdBorrowers.ActiveCell;

            IDataObject iData = new DataObject();
            iData = Clipboard.GetDataObject();
            //if( iData.GetDataPresent(System.Windows.Forms.DataFormats.Text))
            //{
            //    MessageBox.Show("Data present");
            //}
            //else
            //{
            //    MessageBox.Show("Not present");
            //}
            if ((iData.GetDataPresent(System.Windows.Forms.DataFormats.Text)))
            {
                string str = iData.GetData(System.Windows.Forms.DataFormats.Text).ToString();
            }
            //MessageBox.Show(str);
            string updatedText = e.Cell.Text.ToString();
            int lenUpdatedText = updatedText.Length;
            int lenOldText = oldText.Length;
            if (lenUpdatedText > 1 && (prevActCell != nextActCell))
            {
                //Do nothing.
            }
            else
            {
                AddNewRow();
            }
            oldText = updatedText;
            nextActCell = prevActCell;
        }

        private void AddNewRow()
        {
            UltraGridCell prevActiveCell = grdBorrowers.Rows[0].Cells[0];
            string cellText = string.Empty;
            int len = int.MinValue;
            //TextBoxTool tBL = new TextBoxTool("As");
            if (grdBorrowers.ActiveCell != null)
            {
                prevActiveCell = grdBorrowers.ActiveCell;
                cellText = prevActiveCell.Text;
                len = cellText.Length;
                //tBL = (TextBoxTool)prevActiveCell;
            }
            int rowsCount = grdBorrowers.Rows.Count;
            UltraGridRow dr = grdBorrowers.Rows[rowsCount - 1];

            Companies companyBorrowerDetails = (Companies)grdBorrowers.DataSource;

            Prana.Admin.BLL.Company companyBorrower = new Prana.Admin.BLL.Company();

            //The below varriables are taken from the last row of the grid befor adding the new row.
            string borrowerName = dr.Cells["BorrowerName"].Text.ToString();
            string borrowerShortName = dr.Cells["BorrowerShortName"].Text.ToString();
            string firmID = dr.Cells["FirmID"].Text.ToString();

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (borrowerName != "" || borrowerShortName != "" || firmID != "")
            {
                companyBorrowerDetails.Add(new Prana.Admin.BLL.Company("", "", ""));
                grdBorrowers.DataSource = companyBorrowerDetails;
                //grdBorrowers.UpdateData();
                grdBorrowers.DataBind();
                grdBorrowers.ActiveCell = prevActiveCell;
                grdBorrowers.Focus();
                grdBorrowers.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                if (len != int.MinValue)
                {
                    prevActiveCell.SelLength = 0;
                    prevActiveCell.SelStart = len + 1;
                }
            }
        }

        private void grdBorrowers_KeyDown(object sender, KeyEventArgs e)
        {
            //if (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control)
            //{
            //    IDataObject iData = new DataObject();
            //    iData = Clipboard.GetDataObject();
            //    if (iData.GetDataPresent(DataFormats.Text)) { e.Handled = true; }  //disable ctrlV  and shift+insert for paste   
            //}
        }

        private int _flag = int.MinValue;
        private void grdBorrowers_MouseClick(object sender, MouseEventArgs e)
        {
            MouseButtons rtButton = e.Button;
            _flag = rtButton.CompareTo(MouseButtons.Right);
            //MessageBox.Show("Flag :" + flag);

            //if (_flag == 0)
            //{
            //    IDataObject iData = new DataObject();
            //    iData = Clipboard.GetDataObject();
            //    if (iData.GetDataPresent(DataFormats.Text)) 
            //    //{ e.Handled = true; }  //disable ctrlV  and shift+insert for paste   
            //}
        }

        private void grdBorrowers_MouseDown(object sender, MouseEventArgs e)
        {
            MouseButtons rtButton = e.Button;
            _flag = rtButton.CompareTo(MouseButtons.Right);

        }

        private void grdBorrowers_MouseUp(object sender, MouseEventArgs e)
        {
            MouseButtons rtButton = e.Button;
            _flag = rtButton.CompareTo(MouseButtons.Right);
        }

        private void grdBorrowers_AfterCellActivate(object sender, EventArgs e)
        {
            //int a = 10;
            grdBorrowers.MouseDown -= new MouseEventHandler(grdBorrowers_MouseDown);
            grdBorrowers.MouseDown += new MouseEventHandler(grdBorrowers_MouseDown);
        }

        private void grdBorrowers_Click(object sender, EventArgs e)
        {
            //int a = 10;
        }

    }
}
