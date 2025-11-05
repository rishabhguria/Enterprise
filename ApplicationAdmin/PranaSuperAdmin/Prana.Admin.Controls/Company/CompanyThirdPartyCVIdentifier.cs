using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.ThirdPartyManager.DataAccess;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyThirdPartyCVIdentifier.
    /// </summary>
    public class CompanyThirdPartyCVIdentifier : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "CompanyThirdPartyCVIdentifier : ";
        const string C_COMBO_SELECT = "- Select -";
        const int MIN_VALUE = int.MinValue;

        private System.Windows.Forms.GroupBox grpCVIdentifier;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCVIdentifier;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCompanyCounterPartyVenues;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCompanyThirdParties;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CompanyThirdPartyCVIdentifier()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

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
                if (grpCVIdentifier != null)
                {
                    grpCVIdentifier.Dispose();
                }
                if (grdCVIdentifier != null)
                {
                    grdCVIdentifier.Dispose();
                }
                if (ultraDropDownCompanyCounterPartyVenues != null)
                {
                    ultraDropDownCompanyCounterPartyVenues.Dispose();
                }
                if (ultraDropDownCompanyThirdParties != null)
                {
                    ultraDropDownCompanyThirdParties.Dispose();
                }
                if (nextActCell != null)
                {
                    nextActCell.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContactPerson", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CellPhone", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkTelephone", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Email", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyThirdPartyID", 14, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyCVID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCounterPartyVenueID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVIdentifier", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Zip", 20);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactLastName", 21);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactTitle", 22);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactWorkTelephone", 23);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactFax", 24);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueDetailsID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DisplayName", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsElectronic", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixIdentifier", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECID", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SymbolConventionID", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SideID", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTypesID", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeInForceID", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstructionsID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructionsID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AdvancedOrdersID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OatsIdentifier", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("BaseCurrencyID", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OtherCurrencyID", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 20);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECComplianceID", 21);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 22);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn51 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContactPerson", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn55 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn56 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn57 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn58 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CellPhone", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn59 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkTelephone", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn60 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn61 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Email", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn62 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn63 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyThirdPartyID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn64 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyCVID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn65 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCounterPartyVenueID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn66 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVIdentifier", 17);
            this.grpCVIdentifier = new System.Windows.Forms.GroupBox();
            this.grdCVIdentifier = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDropDownCompanyCounterPartyVenues = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownCompanyThirdParties = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.grpCVIdentifier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCVIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyCounterPartyVenues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyThirdParties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCVIdentifier
            // 
            this.grpCVIdentifier.Controls.Add(this.grdCVIdentifier);
            this.grpCVIdentifier.Location = new System.Drawing.Point(4, 6);
            this.grpCVIdentifier.Name = "grpCVIdentifier";
            this.grpCVIdentifier.Size = new System.Drawing.Size(366, 173);
            this.grpCVIdentifier.TabIndex = 0;
            this.grpCVIdentifier.TabStop = false;
            this.grpCVIdentifier.Text = "CV Identifier";
            // 
            // grdCVIdentifier
            // 
            this.grdCVIdentifier.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCVIdentifier.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 31;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.Width = 12;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 12;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 12;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn5.Width = 12;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.Width = 15;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn7.Width = 17;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn8.Width = 14;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn9.Width = 15;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn10.Width = 19;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn11.Width = 22;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn12.Width = 26;
            ultraGridColumn13.Header.VisiblePosition = 12;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn13.Width = 34;
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.Width = 33;
            ultraGridColumn15.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn15.Header.Caption = "ThirdParty";
            ultraGridColumn15.Header.VisiblePosition = 14;
            ultraGridColumn15.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn15.Width = 80;
            ultraGridColumn16.Header.VisiblePosition = 15;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn16.Width = 47;
            ultraGridColumn17.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn17.Header.Caption = "BrokerVenue";
            ultraGridColumn17.Header.VisiblePosition = 16;
            ultraGridColumn17.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn17.Width = 96;
            ultraGridColumn18.Header.VisiblePosition = 17;
            ultraGridColumn18.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridColumn18.Width = 157;
            ultraGridColumn19.Header.VisiblePosition = 18;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn19.Width = 61;
            ultraGridColumn20.Header.VisiblePosition = 19;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn20.Width = 72;
            ultraGridColumn21.Header.VisiblePosition = 20;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn21.Width = 84;
            ultraGridColumn22.Header.VisiblePosition = 21;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn22.Width = 50;
            ultraGridColumn23.Header.VisiblePosition = 22;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn23.Width = 57;
            ultraGridColumn24.Header.VisiblePosition = 23;
            ultraGridColumn24.Hidden = true;
            ultraGridColumn24.Width = 72;
            ultraGridColumn25.Header.VisiblePosition = 24;
            ultraGridColumn25.Hidden = true;
            ultraGridColumn25.Width = 84;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
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
            ultraGridColumn25});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCVIdentifier.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCVIdentifier.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCVIdentifier.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCVIdentifier.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCVIdentifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCVIdentifier.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCVIdentifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCVIdentifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCVIdentifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCVIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCVIdentifier.Location = new System.Drawing.Point(6, 20);
            this.grdCVIdentifier.Name = "grdCVIdentifier";
            this.grdCVIdentifier.Size = new System.Drawing.Size(354, 147);
            this.grdCVIdentifier.TabIndex = 68;
            this.grdCVIdentifier.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCVIdentifier.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCVIdentifier_AfterCellUpdate);
            this.grdCVIdentifier.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCVIdentifier_InitializeLayout);
            this.grdCVIdentifier.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCVIdentifier_AfterCellChange);
            this.grdCVIdentifier.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grdCVIdentifier_KeyPress);
            this.grdCVIdentifier.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdCVIdentifier_MouseDown);
            this.grdCVIdentifier.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdCVIdentifier_MouseUp);
            // 
            // ultraDropDownCompanyCounterPartyVenues
            // 
            ultraGridColumn26.Header.VisiblePosition = 0;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.Header.VisiblePosition = 1;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn28.Header.VisiblePosition = 2;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn29.Header.VisiblePosition = 3;
            ultraGridColumn29.Hidden = true;
            ultraGridColumn30.Header.VisiblePosition = 4;
            ultraGridColumn30.Hidden = true;
            ultraGridColumn31.Header.VisiblePosition = 5;
            ultraGridColumn31.Hidden = true;
            ultraGridColumn32.Header.VisiblePosition = 6;
            ultraGridColumn33.Header.VisiblePosition = 7;
            ultraGridColumn33.Hidden = true;
            ultraGridColumn34.Header.VisiblePosition = 8;
            ultraGridColumn34.Hidden = true;
            ultraGridColumn35.Header.VisiblePosition = 9;
            ultraGridColumn35.Hidden = true;
            ultraGridColumn36.Header.VisiblePosition = 10;
            ultraGridColumn36.Hidden = true;
            ultraGridColumn37.Header.VisiblePosition = 11;
            ultraGridColumn37.Hidden = true;
            ultraGridColumn38.Header.VisiblePosition = 12;
            ultraGridColumn38.Hidden = true;
            ultraGridColumn39.Header.VisiblePosition = 13;
            ultraGridColumn39.Hidden = true;
            ultraGridColumn40.Header.VisiblePosition = 14;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn41.Header.VisiblePosition = 15;
            ultraGridColumn41.Hidden = true;
            ultraGridColumn42.Header.VisiblePosition = 16;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn43.Header.VisiblePosition = 17;
            ultraGridColumn43.Hidden = true;
            ultraGridColumn44.Header.VisiblePosition = 18;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn45.Header.VisiblePosition = 19;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn46.Header.VisiblePosition = 20;
            ultraGridColumn46.Hidden = true;
            ultraGridColumn47.Header.VisiblePosition = 21;
            ultraGridColumn47.Hidden = true;
            ultraGridColumn48.Header.VisiblePosition = 22;
            ultraGridColumn48.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
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
            ultraGridColumn48});
            this.ultraDropDownCompanyCounterPartyVenues.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.ultraDropDownCompanyCounterPartyVenues.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCompanyCounterPartyVenues.DisplayMember = "";
            this.ultraDropDownCompanyCounterPartyVenues.Location = new System.Drawing.Point(32, 228);
            this.ultraDropDownCompanyCounterPartyVenues.Name = "ultraDropDownCompanyCounterPartyVenues";
            this.ultraDropDownCompanyCounterPartyVenues.Size = new System.Drawing.Size(109, 39);
            this.ultraDropDownCompanyCounterPartyVenues.TabIndex = 6;
            this.ultraDropDownCompanyCounterPartyVenues.Text = "ultraDropDown1";
            this.ultraDropDownCompanyCounterPartyVenues.ValueMember = "";
            this.ultraDropDownCompanyCounterPartyVenues.Visible = false;
            this.ultraDropDownCompanyCounterPartyVenues.AfterCloseUp += new Infragistics.Win.UltraWinGrid.DropDownEventHandler(this.ultraDropDownCompanyCounterPartyVenues_AfterCloseUp);
            this.ultraDropDownCompanyCounterPartyVenues.TextChanged += new System.EventHandler(this.ultraDropDownCompanyCounterPartyVenues_TextChanged);
            // 
            // ultraDropDownCompanyThirdParties
            // 
            ultraGridColumn49.Header.VisiblePosition = 0;
            ultraGridColumn49.Hidden = true;
            ultraGridColumn50.Header.VisiblePosition = 1;
            ultraGridColumn51.Header.VisiblePosition = 2;
            ultraGridColumn51.Hidden = true;
            ultraGridColumn52.Header.VisiblePosition = 3;
            ultraGridColumn52.Hidden = true;
            ultraGridColumn53.Header.VisiblePosition = 4;
            ultraGridColumn53.Hidden = true;
            ultraGridColumn54.Header.VisiblePosition = 5;
            ultraGridColumn54.Hidden = true;
            ultraGridColumn55.Header.VisiblePosition = 6;
            ultraGridColumn55.Hidden = true;
            ultraGridColumn56.Header.VisiblePosition = 7;
            ultraGridColumn56.Hidden = true;
            ultraGridColumn57.Header.VisiblePosition = 8;
            ultraGridColumn57.Hidden = true;
            ultraGridColumn58.Header.VisiblePosition = 9;
            ultraGridColumn58.Hidden = true;
            ultraGridColumn59.Header.VisiblePosition = 10;
            ultraGridColumn59.Hidden = true;
            ultraGridColumn60.Header.VisiblePosition = 11;
            ultraGridColumn60.Hidden = true;
            ultraGridColumn61.Header.VisiblePosition = 12;
            ultraGridColumn61.Hidden = true;
            ultraGridColumn62.Header.VisiblePosition = 13;
            ultraGridColumn62.Hidden = true;
            ultraGridColumn63.Header.VisiblePosition = 14;
            ultraGridColumn63.Hidden = true;
            ultraGridColumn64.Header.VisiblePosition = 15;
            ultraGridColumn64.Hidden = true;
            ultraGridColumn65.Header.VisiblePosition = 16;
            ultraGridColumn65.Hidden = true;
            ultraGridColumn66.Header.VisiblePosition = 17;
            ultraGridColumn66.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
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
            ultraGridColumn66});
            this.ultraDropDownCompanyThirdParties.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.ultraDropDownCompanyThirdParties.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCompanyThirdParties.Location = new System.Drawing.Point(312, 214);
            this.ultraDropDownCompanyThirdParties.Name = "ultraDropDownCompanyThirdParties";
            this.ultraDropDownCompanyThirdParties.Size = new System.Drawing.Size(106, 34);
            this.ultraDropDownCompanyThirdParties.TabIndex = 8;
            this.ultraDropDownCompanyThirdParties.Text = "ultraDropDown1";
            this.ultraDropDownCompanyThirdParties.Visible = false;
            this.ultraDropDownCompanyThirdParties.AfterCloseUp += new Infragistics.Win.UltraWinGrid.DropDownEventHandler(this.ultraDropDownCompanyThirdParties_AfterCloseUp);
            // 
            // CompanyThirdPartyCVIdentifier
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraDropDownCompanyThirdParties);
            this.Controls.Add(this.grpCVIdentifier);
            this.Controls.Add(this.ultraDropDownCompanyCounterPartyVenues);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CompanyThirdPartyCVIdentifier";
            this.Size = new System.Drawing.Size(373, 182);
            this.grpCVIdentifier.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCVIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyCounterPartyVenues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyThirdParties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void grdCVIdentifier_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            CounterPartyVenues companyCounterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(_companyID);

            companyCounterPartyVenues.Insert(0, new CounterPartyVenue(int.MinValue, C_COMBO_SELECT));
            ultraDropDownCompanyCounterPartyVenues.DataSource = null;
            ultraDropDownCompanyCounterPartyVenues.DataSource = companyCounterPartyVenues;
            ultraDropDownCompanyCounterPartyVenues.ValueMember = "CompanyCounterPartyCVID";
            ultraDropDownCompanyCounterPartyVenues.DisplayMember = "DisplayName";
            ultraDropDownCompanyCounterPartyVenues.Text = C_COMBO_SELECT;

            grdCVIdentifier.DisplayLayout.Bands[0].Columns["CompanyCounterPartyVenueID"].ValueList = ultraDropDownCompanyCounterPartyVenues;
            ColumnsCollection columnsCounterPartyVenues = ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCounterPartyVenues)
            {
                if (column.Key != "DisplayName")
                {
                    column.Hidden = true;
                }
            }

            ThirdParties companyThirdParties = ThirdPartyDataManager.GetCompanyThirdParties(_companyID);
            ThirdParties validCompanyThirdParties = new ThirdParties();
            //			if(companyThirdParties.Count > 0)
            //			{
            companyThirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, C_COMBO_SELECT));
            int vendor = 2;
            foreach (Prana.BusinessObjects.ThirdParty tempThirdParty in companyThirdParties)
            {
                if (int.Parse(ThirdPartyDataManager.GetThirdPartyTypeId(tempThirdParty).ToString()) != vendor)
                {
                    validCompanyThirdParties.Add(tempThirdParty);
                }
            }
            //			}
            ultraDropDownCompanyThirdParties.DataSource = null;
            ultraDropDownCompanyThirdParties.DataSource = validCompanyThirdParties;
            ultraDropDownCompanyThirdParties.DisplayMember = "ThirdPartyName";
            ultraDropDownCompanyThirdParties.ValueMember = "CompanyThirdPartyID";
            ultraDropDownCompanyThirdParties.Text = C_COMBO_SELECT;

            grdCVIdentifier.DisplayLayout.Bands[0].Columns["CompanyThirdPartyID"].ValueList = ultraDropDownCompanyThirdParties;
            ColumnsCollection columnsCTP = ultraDropDownCompanyThirdParties.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCTP)
            {
                if (column.Key != "ThirdPartyName")
                {
                    column.Hidden = true;
                }
            }

            //grdCVIdentifier.DisplayLayout.Bands[0].Columns["IdentifierID"].ValueList = ultraDropDownThirdParties;
            //			ColumnsCollection columnsIdenifiers = ultraDropDownThirdParties.DisplayLayout.Bands[0].Columns;
            //			foreach (UltraGridColumn column in columnsIdenifiers)
            //			{
            //				if(column.Key != "CounterPartyName")
            //				{
            //					column.Hidden = true;
            //				}
            //			}
            //AddNewRow();
        }

        int _companyID = int.MinValue;


        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            BindDataGrid();
            //BindJJ();
        }

        //Not required
        private void BindJJ()
        {
            //    CounterPartyVenues companyCounterPartyVenues = Prana.Admin.BLL.CounterPartyManager.GetCompanyCounterPartyVeneus(_companyID);

            //    companyCounterPartyVenues.Insert(0, new Prana.Admin.BLL.CounterPartyVenue(int.MinValue, C_COMBO_SELECT));

            //    ultraDropDownCompanyCounterPartyVenues.DataSource = companyCounterPartyVenues;
            //    ultraDropDownCompanyCounterPartyVenues.ValueMember = "CounterPartyID";
            //    ultraDropDownCompanyCounterPartyVenues.DisplayMember = "CounterPartyName";
            //    ultraDropDownCompanyCounterPartyVenues.Text = C_COMBO_SELECT;

            //    cmbCounterPartyVenue.DataSource = companyCounterPartyVenues;
            //    cmbCounterPartyVenue.ValueMember = "CounterPartyID";
            //    cmbCounterPartyVenue.DisplayMember = "CounterPartyName";
            //    cmbCounterPartyVenue.Text = C_COMBO_SELECT;


            //    comboBox2.DataSource = companyCounterPartyVenues;
            //    comboBox2.ValueMember = "CompanyCounterPartyCVID";
            //    comboBox2.DisplayMember = "DisplayName";
            //    comboBox2.Text = C_COMBO_SELECT;

            //    ThirdParties companyThirdParties = ThirdPartyManager.GetCompanyThirdParties(_companyID);
            //    companyThirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, C_COMBO_SELECT));

            //    comboBox1.DataSource = companyThirdParties;				
            //    comboBox1.DisplayMember = "ThirdPartyName";
            //    comboBox1.ValueMember = "ThirdPartyID";
            //    comboBox1.Text = C_COMBO_SELECT; 

            //    ultraDropDownCompanyThirdParties.DataSource = companyThirdParties;
            //    ultraDropDownCompanyThirdParties.ValueMember = "ThirdPartyID";
            //    ultraDropDownCompanyThirdParties.DisplayMember = "ThirdPartyName";
            //    ultraDropDownCompanyThirdParties.Text = C_COMBO_SELECT;
        }

        ThirdParties _thirdPartyCVIdentifiers = new ThirdParties();
        public ThirdParties ThirdPartiesCVIdentifiers
        {
            get
            {
                ThirdParties validCompanyThirdPartyCVIdentifiers = null;
                int index = 1;
                //if(_illegalCombination == false)
                //{
                _thirdPartyCVIdentifiers = (ThirdParties)grdCVIdentifier.DataSource;
                validCompanyThirdPartyCVIdentifiers = new ThirdParties();

                foreach (Prana.BusinessObjects.ThirdParty thirdPartyCVIdentifier in _thirdPartyCVIdentifiers)
                {
                    if (thirdPartyCVIdentifier.CompanyThirdPartyID != MIN_VALUE || thirdPartyCVIdentifier.CompanyCounterPartyVenueID != int.MinValue || thirdPartyCVIdentifier.CVIdentifier != "")
                    {
                        if (thirdPartyCVIdentifier.CompanyThirdPartyID == MIN_VALUE)
                        {
                            MessageBox.Show("Please select the ThirdParty in the row: " + index);
                            validCompanyThirdPartyCVIdentifiers = null;
                            return validCompanyThirdPartyCVIdentifiers;
                        }
                        if (thirdPartyCVIdentifier.CompanyCounterPartyVenueID == MIN_VALUE)
                        {
                            MessageBox.Show("Please select the CounterPartyVenue in the row: " + index);
                            validCompanyThirdPartyCVIdentifiers = null;
                            return validCompanyThirdPartyCVIdentifiers;
                        }
                        if (thirdPartyCVIdentifier.CVIdentifier == "")
                        {
                            MessageBox.Show("Please select the Identifier in the row: " + index);
                            validCompanyThirdPartyCVIdentifiers = null;
                            return validCompanyThirdPartyCVIdentifiers;
                        }
                        validCompanyThirdPartyCVIdentifiers.Add(thirdPartyCVIdentifier);
                        index++;
                    }
                }
                //}
                return validCompanyThirdPartyCVIdentifiers;

            }
            set
            {
                //				Accounts fileFormatAccounts = CompanyManager.GetCompanyThirdPartyFileFormats(_companyID);
                _thirdPartyCVIdentifiers = value;
                if (_thirdPartyCVIdentifiers.Count > 0)
                {
                    grdCVIdentifier.DataSource = _thirdPartyCVIdentifiers;
                    AddNewRow();
                }
            }
        }

        public void BindDataGrid()
        {
            try
            {
                ThirdParties companyThirdParties = ThirdPartyDataManager.GetCompanyThirdParties(_companyID);
                Prana.BusinessObjects.ThirdParty nullCompanyThirdParty = new Prana.BusinessObjects.ThirdParty();
                if (companyThirdParties.Count <= 0)
                {
                    companyThirdParties.Add(nullCompanyThirdParty);
                }

                bool foundVendor = false;
                if (companyThirdParties.Count > 0)
                {
                    int vendor = 2;
                    int index = 0;
                    foreach (Prana.BusinessObjects.ThirdParty tempThirdParty in companyThirdParties)
                    {
                        if (int.Parse(ThirdPartyDataManager.GetThirdPartyTypeId(tempThirdParty).ToString()) == vendor)
                        {
                            foundVendor = true;
                            break;
                        }
                        index++;
                    }
                    if (foundVendor == true)
                    {
                        companyThirdParties.RemoveAt(index);
                    }
                }
                grdCVIdentifier.DataSource = companyThirdParties;
                grdCVIdentifier.ActiveRow = grdCVIdentifier.Rows[0];
                //AddNewRow();
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnLogin_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnLogin_Click", null);
                #endregion
            }
        }

        private void AddNewRow()
        {
            UltraGridCell prevActiveCell = grdCVIdentifier.Rows[0].Cells[0];
            string cellText = string.Empty;
            int len = int.MinValue;
            //TextBoxTool tBL = new TextBoxTool("As");
            if (grdCVIdentifier.ActiveCell != null)
            {
                prevActiveCell = grdCVIdentifier.ActiveCell;
                cellText = prevActiveCell.Text;
                len = cellText.Length;
                //tBL = (TextBoxTool)prevActiveCell;
            }

            ColumnsCollection columnsCounterPartyVenues = ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].Columns;
            //columnsCounterPartyVenues = (ColumnsCollection)grdCVIdentifier.DisplayLayout.Bands[0].Columns.All;
            //System.Data.DataRow dr = grdCVIdentifier.ActiveRow;

            int rowsCount = grdCVIdentifier.Rows.Count;
            UltraGridRow dr = grdCVIdentifier.Rows[rowsCount - 1];
            //UltraGridRow dr = grdCVIdentifier.ActiveRow;

            ThirdParties tps = (ThirdParties)grdCVIdentifier.DataSource;

            //Prana.BusinessObjects.ThirdParty tp = (Prana.BusinessObjects.ThirdParty)dr;
            Prana.BusinessObjects.ThirdParty tp = new Prana.BusinessObjects.ThirdParty();

            //The below varriables are taken from the last row of the grid befor adding the new row.
            int companyThirdPartyID = int.Parse(dr.Cells["CompanyThirdPartyID"].Value.ToString());
            int companyCounterPartyVenueID = int.Parse(dr.Cells["CompanyCounterPartyVenueID"].Value.ToString());
            string cvIdentifier = dr.Cells["CVIdentifier"].Text.ToString();

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (companyThirdPartyID != MIN_VALUE || companyCounterPartyVenueID != MIN_VALUE || cvIdentifier != "")
            {
                tp.CompanyThirdPartyID = int.MinValue;
                tp.CompanyCounterPartyVenueID = int.MinValue;
                tp.CVIdentifier = "";
                tps.Add(tp);
                grdCVIdentifier.DataSource = null;
                grdCVIdentifier.DataSource = tps;
                grdCVIdentifier.DataBind();

                grdCVIdentifier.ActiveCell = prevActiveCell;
                grdCVIdentifier.Focus();
                grdCVIdentifier.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                if (len != int.MinValue)
                {
                    prevActiveCell.SelLength = 0;
                    prevActiveCell.SelStart = len + 1;
                }
            }

        }

        private bool _illegalCombination = false;
        public void ThirdPartyCVJoinValidate()
        {
            _illegalCombination = false;
            ThirdParties companyThirdPartyCVIdentifiers = (ThirdParties)grdCVIdentifier.DataSource;
            int companyThirdPartyID = int.Parse(grdCVIdentifier.ActiveRow.Cells["CompanyThirdPartyID"].Value.ToString());
            int companyCounterPartyVenueID = int.Parse(grdCVIdentifier.ActiveRow.Cells["CompanyCounterPartyVenueID"].Value.ToString());
            int indexActiveRow = grdCVIdentifier.ActiveRow.Index;
            int index = 0;
            foreach (Prana.BusinessObjects.ThirdParty thirdParty in companyThirdPartyCVIdentifiers)
            {
                if (companyThirdPartyID == thirdParty.CompanyThirdPartyID && companyCounterPartyVenueID == thirdParty.CompanyCounterPartyVenueID && indexActiveRow != index && thirdParty.CompanyThirdPartyID != MIN_VALUE)
                {
                    MessageBox.Show("Combination already exists !");
                    _illegalCombination = true;
                    break;
                }
                index += 1;
            }

        }

        private void grdCVIdentifier_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //AddNewRow();
        }

        private void ultraDropDownCompanyCounterPartyVenues_TextChanged(object sender, System.EventArgs e)
        {
            //MessageBox.Show("Hi");
        }

        private void grdCVIdentifier_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            //MessageBox.Show("Hi");
            //AddNewRow();
        }

        private void ValueList_ListChanged(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            MessageBox.Show("Identifier changed");
        }

        string oldText = string.Empty;
        UltraGridCell nextActCell = null;
        private void grdCVIdentifier_AfterCellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            UltraGridCell prevActCell = grdCVIdentifier.ActiveCell;

            IDataObject iData = new DataObject();
            iData = Clipboard.GetDataObject();
            string str = iData.GetData(System.Windows.Forms.DataFormats.Text).ToString();
            string updatedText = e.Cell.Text.ToString();
            int lenUpdatedText = updatedText.Length;
            int lenOldText = oldText.Length;

            int companyThirdPartyID = int.Parse(grdCVIdentifier.ActiveRow.Cells["CompanyThirdPartyID"].Value.ToString());
            int companyCVID = int.Parse(grdCVIdentifier.ActiveRow.Cells["CompanyCounterPartyVenueID"].Value.ToString());
            grdCVIdentifier.UpdateData();
            ThirdPartyCVJoinValidate();
            if (_illegalCombination == true)
            {
                grdCVIdentifier.ActiveRow.Cells["CompanyCounterPartyVenueID"].Value = companyCVID;
                grdCVIdentifier.ActiveRow.Cells["CompanyThirdPartyID"].Value = companyThirdPartyID;
                grdCVIdentifier.UpdateData();
                _illegalCombination = false;
            }
            if (lenUpdatedText > 1 && (prevActCell != nextActCell))
            {
                //Do nothing.
            }
            else
            {
                AddNewRow();
            }
        }

        private void grdCVIdentifier_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //AddNewRow();
            int selectedIndex = int.Parse(grdCVIdentifier.ActiveRow.Index.ToString());
            this.grdCVIdentifier.Rows[selectedIndex].Selected = true;
            this.grdCVIdentifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Default;

        }

        private void grdCVIdentifier_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //			UIElement objUIElement;
            //			UltraGridCell objUltraGridCell; 
            //			objUIElement = grdUnallocated.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            //			if(objUIElement == null)
            //				return;
            //			objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            //			if(objUltraGridCell == null)
            //				return;
            //
            //			if(objUltraGridCell.Value.ToString()=="True" || objUltraGridCell.Value.ToString()=="False")
            //			{
            //				grdCVIdentifier.UpdateData();
            //			}
        }

        private void ultraDropDownCompanyThirdParties_AfterCloseUp(object sender, Infragistics.Win.UltraWinGrid.DropDownEventArgs e)
        {
            int selectedIndex = int.Parse(grdCVIdentifier.ActiveRow.Index.ToString());
            this.grdCVIdentifier.Rows[selectedIndex].Selected = true;
            this.grdCVIdentifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
        }

        private void ultraDropDownCompanyCounterPartyVenues_AfterCloseUp(object sender, Infragistics.Win.UltraWinGrid.DropDownEventArgs e)
        {
            int selectedIndex = int.Parse(grdCVIdentifier.ActiveRow.Index.ToString());
            this.grdCVIdentifier.Rows[selectedIndex].Selected = true;
            this.grdCVIdentifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
        }

    }
}