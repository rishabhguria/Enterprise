using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.ThirdPartyManager.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyThirdPartyMappingDetails.
    /// </summary>
    public class CompanyThirdPartyMappingDetails : System.Windows.Forms.UserControl
    {

        const string C_COMBO_SELECT = "- Select -";
        const string NONE = "None";
        const int MIN_VALUE = int.MinValue;
        const string CAP_BrokerVenue = "Broker Venue";
        const string NULLSTRING = "";

        #region Wizard Stuff
        private System.Windows.Forms.GroupBox grpAccountInformation;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdAccountInformation;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCompanyAccounts;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownAccountType;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCAccounts;
        private Infragistics.Win.Misc.UltraLabel lblThirdparty;
        private Infragistics.Win.Misc.UltraLabel lblThirdPartyName;
        private Infragistics.Win.Misc.UltraGroupBox grpBxThirdParty;
        private Infragistics.Win.Misc.UltraLabel lblTPType;
        private Infragistics.Win.Misc.UltraLabel lblCompanyIdentifier;
        private Infragistics.Win.Misc.UltraLabel lblType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCompanyIdentifier;
        private UltraDropDown ultraDropDownCompanyThirdParties;
        private UltraDropDown ultraDropDownCompanyCounterPartyVenues;
        private Infragistics.Win.Misc.UltraGroupBox grpSaveFlatFile;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSavePath;
        private Infragistics.Win.Misc.UltraLabel lblNamingConvention;
        private Infragistics.Win.Misc.UltraLabel lblSaveGeneratedFile;
        private Infragistics.Win.Misc.UltraButton btnBrowse;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtNamingConvention;
        private GroupBox grpCVIdentifier;
        private UltraGrid grdCVIdentifier;
        private ErrorProvider errorProvider1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private IContainer components;
        //private CompanyThirdPartyCVIdentifier companyThirdPartyCVIdentifier1;
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyPrimeBrokerClearerID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCustodianID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAdministratorID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountTypeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountTypeName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyPrimeBrokerClearerID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCustodianID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAdministratorID", 6);
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContactPerson", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CellPhone", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkTelephone", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Email", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyThirdPartyID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyCVID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCounterPartyVenueID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVIdentifier", 17);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueDetailsID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DisplayName", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsElectronic", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixIdentifier", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECID", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SymbolConventionID", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SideID", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTypesID", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeInForceID", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstructionsID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructionsID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn51 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AdvancedOrdersID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OatsIdentifier", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("BaseCurrencyID", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OtherCurrencyID", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn55 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 20);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn56 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECComplianceID", 21);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn57 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 22);
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            this.grpAccountInformation = new System.Windows.Forms.GroupBox();
            this.grdAccountInformation = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDropDownCompanyAccounts = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownAccountType = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ultraDropDownCAccounts = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.lblThirdparty = new Infragistics.Win.Misc.UltraLabel();
            this.lblThirdPartyName = new Infragistics.Win.Misc.UltraLabel();
            this.grpBxThirdParty = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtCompanyIdentifier = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTPType = new Infragistics.Win.Misc.UltraLabel();
            this.lblCompanyIdentifier = new Infragistics.Win.Misc.UltraLabel();
            this.lblType = new Infragistics.Win.Misc.UltraLabel();
            this.ultraDropDownCompanyThirdParties = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownCompanyCounterPartyVenues = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.grpSaveFlatFile = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnBrowse = new Infragistics.Win.Misc.UltraButton();
            this.txtNamingConvention = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSavePath = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblNamingConvention = new Infragistics.Win.Misc.UltraLabel();
            this.lblSaveGeneratedFile = new Infragistics.Win.Misc.UltraLabel();
            this.grpCVIdentifier = new System.Windows.Forms.GroupBox();
            this.grdCVIdentifier = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpAccountInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountInformation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownAccountType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxThirdParty)).BeginInit();
            this.grpBxThirdParty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyThirdParties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyCounterPartyVenues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSaveFlatFile)).BeginInit();
            this.grpSaveFlatFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamingConvention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSavePath)).BeginInit();
            this.grpCVIdentifier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCVIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpAccountInformation
            // 
            this.grpAccountInformation.Controls.Add(this.grdAccountInformation);
            this.grpAccountInformation.Location = new System.Drawing.Point(4, 58);
            this.grpAccountInformation.Name = "grpAccountInformation";
            this.grpAccountInformation.Size = new System.Drawing.Size(648, 195);
            this.grpAccountInformation.TabIndex = 0;
            this.grpAccountInformation.TabStop = false;
            this.grpAccountInformation.Text = "Mapping Details";
            // 
            // grdAccountInformation
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdAccountInformation.DisplayLayout.Appearance = appearance1;
            this.grdAccountInformation.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountInformation.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdAccountInformation.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccountInformation.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdAccountInformation.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAccountInformation.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdAccountInformation.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccountInformation.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAccountInformation.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdAccountInformation.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccountInformation.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdAccountInformation.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdAccountInformation.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdAccountInformation.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdAccountInformation.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdAccountInformation.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdAccountInformation.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdAccountInformation.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdAccountInformation.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdAccountInformation.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdAccountInformation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdAccountInformation.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdAccountInformation.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountInformation.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdAccountInformation.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdAccountInformation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccountInformation.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccountInformation.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccountInformation.Location = new System.Drawing.Point(0, 16);
            this.grdAccountInformation.Name = "grdAccountInformation";
            this.grdAccountInformation.Size = new System.Drawing.Size(642, 174);
            this.grdAccountInformation.TabIndex = 0;
            this.grdAccountInformation.Text = "ultraGrid1";
            this.grdAccountInformation.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountInformation.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAccountInformation_InitializeLayout);
            // 
            // ultraDropDownCompanyAccounts
            // 
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7});
            this.ultraDropDownCompanyAccounts.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.ultraDropDownCompanyAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCompanyAccounts.Location = new System.Drawing.Point(693, 295);
            this.ultraDropDownCompanyAccounts.Name = "ultraDropDownCompanyAccounts";
            this.ultraDropDownCompanyAccounts.Size = new System.Drawing.Size(106, 18);
            this.ultraDropDownCompanyAccounts.TabIndex = 10;
            this.ultraDropDownCompanyAccounts.Text = "ultraDropDown1";
            this.ultraDropDownCompanyAccounts.Visible = false;
            // 
            // ultraDropDownAccountType
            // 
            ultraGridColumn8.Header.VisiblePosition = 0;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.Header.VisiblePosition = 1;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn8,
            ultraGridColumn9});
            this.ultraDropDownAccountType.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.ultraDropDownAccountType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownAccountType.Location = new System.Drawing.Point(693, 246);
            this.ultraDropDownAccountType.Name = "ultraDropDownAccountType";
            this.ultraDropDownAccountType.Size = new System.Drawing.Size(106, 19);
            this.ultraDropDownAccountType.TabIndex = 11;
            this.ultraDropDownAccountType.Text = "ultraDropDown1";
            this.ultraDropDownAccountType.Visible = false;
            // 
            // ultraDropDownCAccounts
            // 
            ultraGridColumn10.Header.VisiblePosition = 0;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.Header.VisiblePosition = 1;
            ultraGridColumn12.Header.VisiblePosition = 2;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 3;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn14.Header.VisiblePosition = 4;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn15.Header.VisiblePosition = 5;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn16.Header.VisiblePosition = 6;
            ultraGridColumn16.Hidden = true;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16});
            this.ultraDropDownCAccounts.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.ultraDropDownCAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCAccounts.Location = new System.Drawing.Point(693, 271);
            this.ultraDropDownCAccounts.Name = "ultraDropDownCAccounts";
            this.ultraDropDownCAccounts.Size = new System.Drawing.Size(106, 18);
            this.ultraDropDownCAccounts.TabIndex = 12;
            this.ultraDropDownCAccounts.Text = "ultraDropDown1";
            this.ultraDropDownCAccounts.Visible = false;
            // 
            // lblThirdparty
            // 
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.lblThirdparty.Appearance = appearance13;
            this.lblThirdparty.Location = new System.Drawing.Point(6, 14);
            this.lblThirdparty.Name = "lblThirdparty";
            this.lblThirdparty.Size = new System.Drawing.Size(67, 15);
            this.lblThirdparty.TabIndex = 13;
            this.lblThirdparty.Text = "Third Party :";
            // 
            // lblThirdPartyName
            // 
            appearance14.ForeColor = System.Drawing.Color.Black;
            appearance14.TextHAlignAsString = "Center";
            appearance14.TextVAlignAsString = "Middle";
            this.lblThirdPartyName.Appearance = appearance14;
            this.lblThirdPartyName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblThirdPartyName.Location = new System.Drawing.Point(79, 13);
            this.lblThirdPartyName.Name = "lblThirdPartyName";
            this.lblThirdPartyName.Size = new System.Drawing.Size(80, 17);
            this.lblThirdPartyName.TabIndex = 14;
            this.lblThirdPartyName.Text = "MorganStanley";
            // 
            // grpBxThirdParty
            // 
            this.grpBxThirdParty.Controls.Add(this.txtCompanyIdentifier);
            this.grpBxThirdParty.Controls.Add(this.lblTPType);
            this.grpBxThirdParty.Controls.Add(this.lblCompanyIdentifier);
            this.grpBxThirdParty.Controls.Add(this.lblType);
            this.grpBxThirdParty.Controls.Add(this.lblThirdparty);
            this.grpBxThirdParty.Controls.Add(this.lblThirdPartyName);
            this.grpBxThirdParty.Location = new System.Drawing.Point(4, 7);
            this.grpBxThirdParty.Name = "grpBxThirdParty";
            this.grpBxThirdParty.Size = new System.Drawing.Size(647, 48);
            this.grpBxThirdParty.TabIndex = 3;
            // 
            // txtCompanyIdentifier
            // 
            this.txtCompanyIdentifier.Location = new System.Drawing.Point(495, 10);
            this.txtCompanyIdentifier.MaxLength = 15;
            this.txtCompanyIdentifier.Name = "txtCompanyIdentifier";
            this.txtCompanyIdentifier.Size = new System.Drawing.Size(129, 22);
            this.txtCompanyIdentifier.TabIndex = 3;
            this.txtCompanyIdentifier.Enter += new System.EventHandler(this.txtCompanyIdentifier_Enter);
            this.txtCompanyIdentifier.Leave += new System.EventHandler(this.txtCompanyIdentifier_Leave);
            // 
            // lblTPType
            // 
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Center";
            appearance15.TextVAlignAsString = "Middle";
            this.lblTPType.Appearance = appearance15;
            this.lblTPType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblTPType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTPType.Location = new System.Drawing.Point(257, 13);
            this.lblTPType.Name = "lblTPType";
            this.lblTPType.Size = new System.Drawing.Size(71, 17);
            this.lblTPType.TabIndex = 19;
            this.lblTPType.Text = "Prime Broker";
            // 
            // lblCompanyIdentifier
            // 
            appearance16.TextHAlignAsString = "Center";
            appearance16.TextVAlignAsString = "Middle";
            this.lblCompanyIdentifier.Appearance = appearance16;
            this.lblCompanyIdentifier.Location = new System.Drawing.Point(382, 14);
            this.lblCompanyIdentifier.Name = "lblCompanyIdentifier";
            this.lblCompanyIdentifier.Size = new System.Drawing.Size(107, 15);
            this.lblCompanyIdentifier.TabIndex = 18;
            this.lblCompanyIdentifier.Text = "Client Identifier :";
            // 
            // lblType
            // 
            appearance17.TextHAlignAsString = "Center";
            appearance17.TextVAlignAsString = "Middle";
            this.lblType.Appearance = appearance17;
            this.lblType.Location = new System.Drawing.Point(215, 14);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(36, 15);
            this.lblType.TabIndex = 15;
            this.lblType.Text = "Type :";
            // 
            // ultraDropDownCompanyThirdParties
            // 
            ultraGridColumn17.Header.VisiblePosition = 0;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn18.Header.VisiblePosition = 1;
            ultraGridColumn19.Header.VisiblePosition = 2;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn20.Header.VisiblePosition = 3;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn21.Header.VisiblePosition = 4;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn22.Header.VisiblePosition = 5;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn23.Header.VisiblePosition = 6;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn24.Header.VisiblePosition = 7;
            ultraGridColumn24.Hidden = true;
            ultraGridColumn25.Header.VisiblePosition = 8;
            ultraGridColumn25.Hidden = true;
            ultraGridColumn26.Header.VisiblePosition = 9;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.Header.VisiblePosition = 10;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn28.Header.VisiblePosition = 11;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn29.Header.VisiblePosition = 12;
            ultraGridColumn29.Hidden = true;
            ultraGridColumn30.Header.VisiblePosition = 13;
            ultraGridColumn30.Hidden = true;
            ultraGridColumn31.Header.VisiblePosition = 14;
            ultraGridColumn31.Hidden = true;
            ultraGridColumn32.Header.VisiblePosition = 15;
            ultraGridColumn32.Hidden = true;
            ultraGridColumn33.Header.VisiblePosition = 16;
            ultraGridColumn33.Hidden = true;
            ultraGridColumn34.Header.VisiblePosition = 17;
            ultraGridColumn34.Hidden = true;
            ultraGridBand5.Columns.AddRange(new object[] {
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
            ultraGridColumn34});
            this.ultraDropDownCompanyThirdParties.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.ultraDropDownCompanyThirdParties.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCompanyThirdParties.Location = new System.Drawing.Point(693, 319);
            this.ultraDropDownCompanyThirdParties.Name = "ultraDropDownCompanyThirdParties";
            this.ultraDropDownCompanyThirdParties.Size = new System.Drawing.Size(106, 24);
            this.ultraDropDownCompanyThirdParties.TabIndex = 18;
            this.ultraDropDownCompanyThirdParties.Text = "ultraDropDown1";
            this.ultraDropDownCompanyThirdParties.Visible = false;
            // 
            // ultraDropDownCompanyCounterPartyVenues
            // 
            ultraGridColumn35.Header.VisiblePosition = 0;
            ultraGridColumn35.Hidden = true;
            ultraGridColumn36.Header.VisiblePosition = 1;
            ultraGridColumn36.Hidden = true;
            ultraGridColumn37.Header.VisiblePosition = 2;
            ultraGridColumn37.Hidden = true;
            ultraGridColumn38.Header.VisiblePosition = 3;
            ultraGridColumn38.Hidden = true;
            ultraGridColumn39.Header.VisiblePosition = 4;
            ultraGridColumn39.Hidden = true;
            ultraGridColumn40.Header.VisiblePosition = 5;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn41.Header.VisiblePosition = 6;
            ultraGridColumn42.Header.VisiblePosition = 7;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn43.Header.VisiblePosition = 8;
            ultraGridColumn43.Hidden = true;
            ultraGridColumn44.Header.VisiblePosition = 9;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn45.Header.VisiblePosition = 10;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn46.Header.VisiblePosition = 11;
            ultraGridColumn46.Hidden = true;
            ultraGridColumn47.Header.VisiblePosition = 12;
            ultraGridColumn47.Hidden = true;
            ultraGridColumn48.Header.VisiblePosition = 13;
            ultraGridColumn48.Hidden = true;
            ultraGridColumn49.Header.VisiblePosition = 14;
            ultraGridColumn49.Hidden = true;
            ultraGridColumn50.Header.VisiblePosition = 15;
            ultraGridColumn50.Hidden = true;
            ultraGridColumn51.Header.VisiblePosition = 16;
            ultraGridColumn51.Hidden = true;
            ultraGridColumn52.Header.VisiblePosition = 17;
            ultraGridColumn52.Hidden = true;
            ultraGridColumn53.Header.VisiblePosition = 18;
            ultraGridColumn53.Hidden = true;
            ultraGridColumn54.Header.VisiblePosition = 19;
            ultraGridColumn54.Hidden = true;
            ultraGridColumn55.Header.VisiblePosition = 20;
            ultraGridColumn55.Hidden = true;
            ultraGridColumn56.Header.VisiblePosition = 21;
            ultraGridColumn56.Hidden = true;
            ultraGridColumn57.Header.VisiblePosition = 22;
            ultraGridColumn57.Hidden = true;
            ultraGridBand6.Columns.AddRange(new object[] {
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
            ultraGridColumn57});
            this.ultraDropDownCompanyCounterPartyVenues.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.ultraDropDownCompanyCounterPartyVenues.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCompanyCounterPartyVenues.Location = new System.Drawing.Point(690, 349);
            this.ultraDropDownCompanyCounterPartyVenues.Name = "ultraDropDownCompanyCounterPartyVenues";
            this.ultraDropDownCompanyCounterPartyVenues.Size = new System.Drawing.Size(109, 13);
            this.ultraDropDownCompanyCounterPartyVenues.TabIndex = 17;
            this.ultraDropDownCompanyCounterPartyVenues.Text = "ultraDropDown1";
            this.ultraDropDownCompanyCounterPartyVenues.Visible = false;
            // 
            // grpSaveFlatFile
            // 
            this.grpSaveFlatFile.Controls.Add(this.ultraLabel1);
            this.grpSaveFlatFile.Controls.Add(this.btnBrowse);
            this.grpSaveFlatFile.Controls.Add(this.txtNamingConvention);
            this.grpSaveFlatFile.Controls.Add(this.txtSavePath);
            this.grpSaveFlatFile.Controls.Add(this.lblNamingConvention);
            this.grpSaveFlatFile.Controls.Add(this.lblSaveGeneratedFile);
            this.grpSaveFlatFile.Location = new System.Drawing.Point(398, 258);
            this.grpSaveFlatFile.Name = "grpSaveFlatFile";
            this.grpSaveFlatFile.Size = new System.Drawing.Size(248, 176);
            this.grpSaveFlatFile.TabIndex = 5;
            this.grpSaveFlatFile.Text = "Save Details";
            // 
            // ultraLabel1
            // 
            appearance18.TextHAlignAsString = "Left";
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance18;
            this.ultraLabel1.Location = new System.Drawing.Point(115, 131);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(102, 15);
            this.ultraLabel1.TabIndex = 5;
            this.ultraLabel1.Text = "eg : {MMddyyyy}";
            this.ultraLabel1.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColorInternal = System.Drawing.Color.White;
            this.btnBrowse.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.btnBrowse.Location = new System.Drawing.Point(169, 71);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(73, 19);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtNamingConvention
            // 
            appearance19.BackColor = System.Drawing.Color.White;
            this.txtNamingConvention.Appearance = appearance19;
            this.txtNamingConvention.BackColor = System.Drawing.Color.White;
            this.txtNamingConvention.Location = new System.Drawing.Point(115, 105);
            this.txtNamingConvention.MaxLength = 50;
            this.txtNamingConvention.Name = "txtNamingConvention";
            this.txtNamingConvention.Size = new System.Drawing.Size(127, 20);
            this.txtNamingConvention.TabIndex = 4;
            this.txtNamingConvention.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtNamingConvention.Visible = false;
            // 
            // txtSavePath
            // 
            appearance20.BackColor = System.Drawing.Color.White;
            this.txtSavePath.Appearance = appearance20;
            this.txtSavePath.BackColor = System.Drawing.Color.White;
            this.txtSavePath.Location = new System.Drawing.Point(115, 45);
            this.txtSavePath.MaxLength = 50;
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.ReadOnly = true;
            this.txtSavePath.Size = new System.Drawing.Size(127, 20);
            this.txtSavePath.TabIndex = 3;
            this.txtSavePath.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtSavePath.Enter += new System.EventHandler(this.txtSavePath_Enter);
            this.txtSavePath.Leave += new System.EventHandler(this.txtSavePath_Leave);
            // 
            // lblNamingConvention
            // 
            appearance21.TextHAlignAsString = "Center";
            appearance21.TextVAlignAsString = "Middle";
            this.lblNamingConvention.Appearance = appearance21;
            this.lblNamingConvention.Location = new System.Drawing.Point(6, 108);
            this.lblNamingConvention.Name = "lblNamingConvention";
            this.lblNamingConvention.Size = new System.Drawing.Size(102, 15);
            this.lblNamingConvention.TabIndex = 1;
            this.lblNamingConvention.Text = "Naming Convention";
            this.lblNamingConvention.Visible = false;
            // 
            // lblSaveGeneratedFile
            // 
            appearance22.TextHAlignAsString = "Center";
            appearance22.TextVAlignAsString = "Middle";
            this.lblSaveGeneratedFile.Appearance = appearance22;
            this.lblSaveGeneratedFile.Location = new System.Drawing.Point(6, 48);
            this.lblSaveGeneratedFile.Name = "lblSaveGeneratedFile";
            this.lblSaveGeneratedFile.Size = new System.Drawing.Size(104, 15);
            this.lblSaveGeneratedFile.TabIndex = 0;
            this.lblSaveGeneratedFile.Text = "Save Generated File";
            // 
            // grpCVIdentifier
            // 
            this.grpCVIdentifier.Controls.Add(this.grdCVIdentifier);
            this.grpCVIdentifier.Location = new System.Drawing.Point(17, 261);
            this.grpCVIdentifier.Name = "grpCVIdentifier";
            this.grpCVIdentifier.Size = new System.Drawing.Size(366, 173);
            this.grpCVIdentifier.TabIndex = 4;
            this.grpCVIdentifier.TabStop = false;
            this.grpCVIdentifier.Text = "CV Identifier";
            // 
            // grdCVIdentifier
            // 
            this.grdCVIdentifier.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCVIdentifier.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand7.Header.Enabled = false;
            ultraGridBand7.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCVIdentifier.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.grdCVIdentifier.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCVIdentifier.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCVIdentifier.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCVIdentifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCVIdentifier.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCVIdentifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCVIdentifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCVIdentifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCVIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCVIdentifier.Location = new System.Drawing.Point(6, 26);
            this.grdCVIdentifier.Name = "grdCVIdentifier";
            this.grdCVIdentifier.Size = new System.Drawing.Size(354, 147);
            this.grdCVIdentifier.TabIndex = 0;
            this.grdCVIdentifier.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCVIdentifier.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCVIdentifier_InitializeLayout);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CompanyThirdPartyMappingDetails
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpCVIdentifier);
            this.Controls.Add(this.grpSaveFlatFile);
            this.Controls.Add(this.ultraDropDownCompanyThirdParties);
            this.Controls.Add(this.ultraDropDownCompanyCounterPartyVenues);
            this.Controls.Add(this.grpBxThirdParty);
            this.Controls.Add(this.ultraDropDownCAccounts);
            this.Controls.Add(this.ultraDropDownAccountType);
            this.Controls.Add(this.ultraDropDownCompanyAccounts);
            this.Controls.Add(this.grpAccountInformation);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "CompanyThirdPartyMappingDetails";
            this.Size = new System.Drawing.Size(657, 444);
            this.grpAccountInformation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountInformation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownAccountType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxThirdParty)).EndInit();
            this.grpBxThirdParty.ResumeLayout(false);
            this.grpBxThirdParty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyThirdParties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyCounterPartyVenues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSaveFlatFile)).EndInit();
            this.grpSaveFlatFile.ResumeLayout(false);
            this.grpSaveFlatFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamingConvention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSavePath)).EndInit();
            this.grpCVIdentifier.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCVIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

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
                if (grpAccountInformation != null)
                {
                    grpAccountInformation.Dispose();
                }
                if (grdAccountInformation != null)
                {
                    grdAccountInformation.Dispose();
                }
                if (ultraDropDownCompanyAccounts != null)
                {
                    ultraDropDownCompanyAccounts.Dispose();
                }
                if (ultraDropDownAccountType != null)
                {
                    ultraDropDownAccountType.Dispose();
                }
                if (saveFileDialog1 != null)
                {
                    saveFileDialog1.Dispose();
                }
                if (ultraDropDownCAccounts != null)
                {
                    ultraDropDownCAccounts.Dispose();
                }
                if (lblThirdparty != null)
                {
                    lblThirdparty.Dispose();
                }
                if (lblThirdPartyName != null)
                {
                    lblThirdPartyName.Dispose();
                }
                if (grpBxThirdParty != null)
                {
                    grpBxThirdParty.Dispose();
                }
                if (lblTPType != null)
                {
                    lblTPType.Dispose();
                }
                if (lblCompanyIdentifier != null)
                {
                    lblCompanyIdentifier.Dispose();
                }
                if (lblType != null)
                {
                    lblType.Dispose();
                }
                if (txtCompanyIdentifier != null)
                {
                    txtCompanyIdentifier.Dispose();
                }
                if (ultraDropDownCompanyThirdParties != null)
                {
                    ultraDropDownCompanyThirdParties.Dispose();
                }
                if (ultraDropDownCompanyCounterPartyVenues != null)
                {
                    ultraDropDownCompanyCounterPartyVenues.Dispose();
                }
                if (grpSaveFlatFile != null)
                {
                    grpSaveFlatFile.Dispose();
                }
                if (txtSavePath != null)
                {
                    txtSavePath.Dispose();
                }
                if (lblNamingConvention != null)
                {
                    lblNamingConvention.Dispose();
                }
                if (lblSaveGeneratedFile != null)
                {
                    lblSaveGeneratedFile.Dispose();
                }
                if (btnBrowse != null)
                {
                    btnBrowse.Dispose();
                }
                if (txtNamingConvention != null)
                {
                    txtNamingConvention.Dispose();
                }
                if (grpCVIdentifier != null)
                {
                    grpCVIdentifier.Dispose();
                }
                if (grdCVIdentifier != null)
                {
                    grdCVIdentifier.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public CompanyThirdPartyMappingDetails()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }
        #endregion Wizard Stuff

        // This sets the selected companyID and thirdpartyId for the Third Party usercontrol.
        private int _companyID = int.MinValue;
        private int _companyThirdPartyID = int.MinValue;

        /// <summary>
        /// set property of the company id selected in the tree in the main form.
        /// </summary>
        public int CompanyID
        {
            set { _companyID = value; }
        }

        /// <summary>
        /// the set property for the selected thirdparty id in the main form tree.
        /// </summary>
        public int ThirdPartyID
        {
            set { _companyThirdPartyID = value; }
        }

        /// <summary>
        /// the set property for the account mapping details grid data.
        /// </summary>
        public ThirdPartyAccounts SetThirdParty
        {
            set
            { SetThirdPartyMappingDetail(value); }
        }

        /// <summary>
        /// the set method called in the set property of the account mapping details grid .
        /// </summary>
        /// <param name="thirdPartyAccounts"></param>
        private void SetThirdPartyMappingDetail(Prana.Admin.BLL.ThirdPartyAccounts thirdPartyAccounts)
        {
            errorProvider1.SetError(txtCompanyIdentifier, "");
            //errorProvider1.SetError(txtNamingConvention, "");
            errorProvider1.SetError(txtSavePath, "");
            if (_companyThirdPartyID != int.MinValue)
            {
                Prana.BusinessObjects.ThirdParty thirdParty = ThirdPartyDataManager.GetCompanyThirdParty(_companyThirdPartyID);
                lblThirdPartyName.Text = thirdParty.ShortName;
                lblThirdPartyName.AutoSize = true;
                Prana.BusinessObjects.ThirdPartyType thirdPartyType = ThirdPartyDataManager.GetThirdPartyType(ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty));
                lblTPType.Text = thirdPartyType.ThirdPartyTypeName;
                lblTPType.AutoSize = true;

                BindMappingDetailsGrid();
            }
            else
            {

                lblThirdPartyName.Text = "";
                lblTPType.Text = "";
                BindThirdPartyAccounts();
            }
        }

        /// <summary>
        /// the set property for the flat file save details of the thirdparty.
        /// </summary>
        public ThirdPartyFlatFileSaveDetail SetTPFFSaveDetail
        {
            set
            { SettingTPFFSaveDetail(value); }
        }

        /// <summary>
        /// the set method used to set the save details of the third party flat file.
        /// </summary>
        /// <param name="thirdPartyFlatFileSaveDetail"></param>
        private void SettingTPFFSaveDetail(ThirdPartyFlatFileSaveDetail thirdPartyFlatFileSaveDetail)
        {
            if (_companyThirdPartyID != int.MinValue)
            {
                if (thirdPartyFlatFileSaveDetail != null)
                {
                    txtCompanyIdentifier.Text = thirdPartyFlatFileSaveDetail.CompanyIdentifier;
                    txtSavePath.Text = thirdPartyFlatFileSaveDetail.SaveGeneratedFileIn;
                    txtNamingConvention.Text = thirdPartyFlatFileSaveDetail.NamingConvention;
                }
                else
                {
                    RefreshSaveDetails();
                }

            }
            else
            {
                RefreshSaveDetails();
            }
        }

        /// <summary>
        /// this is the set property of the cv identifier of the third party.
        /// </summary>
        public Prana.Admin.BLL.ThirdPartyCVIdentifiers SetCVIdentifier
        {
            set
            { SettingCVIdentifier(value); }
        }

        /// <summary>
        /// this method is used to set the CV identifier data of the selected third party.
        /// </summary>
        /// <param name="thirdPartyCVIdentifiers"></param>
        private void SettingCVIdentifier(ThirdPartyCVIdentifiers thirdPartyCVIdentifiers)
        {
            if (_companyThirdPartyID != int.MinValue)
            {
                //if (thirdPartyCVIdentifiers.Count > 0)
                //{
                BindCVIdentifierGrid();
                //}
                //else
                //{
                //    BindCVs();
                //}
            }
            else
            {
                BindCVs();
            }
        }

        /// <summary>
        /// To bind an empty grid in case of no third party selected. 
        /// </summary>
        private void BindThirdPartyAccounts()
        {
            DataTable dtEmpty = new DataTable();

            dtEmpty.Columns.Add("Internal Account Name");
            dtEmpty.Columns.Add("Mapped Account Name");
            dtEmpty.Columns.Add("Account A/C #");
            dtEmpty.Columns.Add("Account Type");

            object[] row = new object[4];

            row[0] = "";
            row[1] = "";
            row[2] = "";
            row[3] = "";

            dtEmpty.Rows.Add(row);
            grdAccountInformation.DataSource = dtEmpty;

        }

        /// <summary>
        /// To bind the Grid with the the CV mapping details of the selected third party.
        /// </summary>
        private void BindCVIdentifierGrid()
        {
            DataTable dtCV = new DataTable();
            dtCV.Columns.Add("CounterPartyVenueID");
            dtCV.Columns.Add("CounterParty Venue");
            dtCV.Columns.Add("CV Identifier");

            object[] row = new object[3];

            CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(_companyID);
            ThirdPartyCVIdentifiers thirdPartyCVIdentifiers = CompanyManager.GetCompanyThirdPartyCVIdentifiers(_companyThirdPartyID);

            if (counterPartyVenues.Count > 0)
            {
                if (thirdPartyCVIdentifiers.Count > 0)
                {
                    foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
                    {
                        int cVID = counterPartyVenue.CompanyCounterPartyCVID;
                        string cVName = counterPartyVenue.DisplayName;
                        string cVIdentifier = "";
                        foreach (ThirdPartyCVIdentifier thirdPartyCVIdentifier in thirdPartyCVIdentifiers)
                        {

                            if (counterPartyVenue.CompanyCounterPartyCVID == thirdPartyCVIdentifier.CompanyCounterPartyVenueID)
                            {
                                //int cVID = thirdPartyCVIdentifier.CompanyCounterPartyVenueID;
                                //CompanyCounterPartyVenueDetail cv = CompanyManager.GetCompanyCounterPartyVenueDetail(cVID);
                                ////CounterPartyVenue cv = CounterPartyManager.GetCounterPartyVenue .GetCompanyCounterPartyVenue(cVID);
                                //string cVName = thirdPartyCVIdentifier.CVName;
                                cVIdentifier = thirdPartyCVIdentifier.CVIdentifier;
                                break;
                            }
                        }
                        row[0] = cVID;
                        row[1] = cVName;
                        row[2] = cVIdentifier;

                        dtCV.Rows.Add(row);
                    }


                }
                else
                {
                    //CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(_companyID);

                    if (counterPartyVenues.Count > 0)
                    {
                        foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
                        {
                            int cVID = counterPartyVenue.CompanyCounterPartyCVID;
                            string cVName = counterPartyVenue.DisplayName;
                            string cVIdentifier = "";

                            row[0] = cVID;
                            row[1] = cVName;
                            row[2] = cVIdentifier;

                            dtCV.Rows.Add(row);
                        }

                    }
                    else
                    {
                        ThirdPartyCVIdentifier thirdPartyCVIdentifier = new ThirdPartyCVIdentifier();
                        int cVID = thirdPartyCVIdentifier.CompanyCounterPartyVenueID;
                        CompanyCounterPartyVenueDetail cv = CompanyManager.GetCompanyCounterPartyVenueDetail(cVID);
                        string cVName = cv.CompanyCounterPartyVenueName;
                        string cVIdentifier = thirdPartyCVIdentifier.CVIdentifier;

                        row[0] = cVID;
                        row[1] = cVName;
                        row[2] = cVIdentifier;

                        dtCV.Rows.Add(row);
                    }

                }
            }
            //grdCVIdentifier.DataSource = thirdPartyCVIdentifiers;
            grdCVIdentifier.DataSource = dtCV;
        }

        /// <summary>
        /// to bind an empty grid in case of no third party selected.
        /// </summary>
        private void BindCVs()
        {
            DataTable dtEmpty = new DataTable();
            dtEmpty.Columns.Add("CounterParty Venue");
            dtEmpty.Columns.Add("CV Identifier");

            object[] row = new object[2];

            row[0] = "";
            row[1] = "";

            dtEmpty.Rows.Add(row);
            grdCVIdentifier.DataSource = dtEmpty;

        }

        /// <summary>
        /// to refresh the save details of flat files for the thirdparty.
        /// </summary>
        private void RefreshSaveDetails()
        {
            txtCompanyIdentifier.Text = "";
            txtNamingConvention.Text = "";
            txtSavePath.Text = "";
        }

        /// <summary>
        /// To bind the grid of mapping details with the Third Party Account mapping details.
        /// </summary>
        private void BindMappingDetailsGrid()
        {
            DataTable dtTPaccounts = new DataTable();
            dtTPaccounts.Columns.Add("Account ID");
            dtTPaccounts.Columns.Add("Internal Account Name");
            dtTPaccounts.Columns.Add("Mapped Account Name");
            dtTPaccounts.Columns.Add("Account A/C #");
            dtTPaccounts.Columns.Add("Account Type");
            dtTPaccounts.Columns.Add("AccountTypeID");


            object[] row = new object[6];

            ThirdPartyAccounts companyThirdPartyAccounts = CompanyManager.GetCompanyThirdPartyMappingDetails(_companyThirdPartyID);
            List<ThirdPartyPermittedAccount> thirdPartyPermittedAccounts = ThirdPartyDataManager.GetThirdPartyPermittedAccounts(_companyThirdPartyID);

            if (thirdPartyPermittedAccounts.Count > 0)
            {
                if (companyThirdPartyAccounts.Count > 0)
                {
                    foreach (ThirdPartyPermittedAccount thirdPartyPermittedAccount in thirdPartyPermittedAccounts)
                    {
                        int accountID = thirdPartyPermittedAccount.CompanyAccountID;
                        string accountName = thirdPartyPermittedAccount.AccountName;
                        int accountTypeId = thirdPartyPermittedAccount.AccountTypeID;
                        AccountType accountType = CompanyManager.GetsAccountType(accountTypeId);
                        string accountTypeName = accountType.AccountTypeName;
                        string mappedName = "";
                        string accntNo = "";
                        foreach (ThirdPartyAccount thirdPartyAccount in companyThirdPartyAccounts)
                        {
                            if (thirdPartyAccount.InternalAccountID == thirdPartyPermittedAccount.CompanyAccountID)
                            {
                                //int accountID = thirdPartyAccount.InternalAccountID;
                                //string accountName = thirdPartyAccount.AccountName;
                                mappedName = thirdPartyAccount.MappedAccountName;
                                accntNo = thirdPartyAccount.Account;
                                //int accountTypeId = thirdPartyAccount.AccountTypeID;
                                //AccountType accountType = CompanyManager.GetsAccountType(accountTypeId);
                                //string accountTypeName = accountType.AccountTypeName;
                                break;

                            }

                        }
                        row[0] = accountID;
                        row[1] = accountName;
                        row[2] = mappedName;
                        row[3] = accntNo;
                        row[4] = accountTypeName;
                        row[5] = accountTypeId;
                        dtTPaccounts.Rows.Add(row);
                    }

                }
                else
                {

                    //ThirdPartyPermittedAccounts thirdPartyPermittedAccounts = ThirdPartyManager.GetThirdPartyPermittedAccounts(_thirdPartyID);
                    if (thirdPartyPermittedAccounts.Count > 0)
                    {
                        foreach (ThirdPartyPermittedAccount thirdPartyPermittedAccount in thirdPartyPermittedAccounts)
                        {
                            int accountID = thirdPartyPermittedAccount.CompanyAccountID;
                            string accountName = thirdPartyPermittedAccount.AccountName;
                            string mappedName = "";
                            string accntNo = "";
                            int accountTypeID = thirdPartyPermittedAccount.AccountTypeID;
                            AccountType accountType = CompanyManager.GetsAccountType(accountTypeID);
                            string accountTypeName = accountType.AccountTypeName;

                            row[0] = accountID;
                            row[1] = accountName;
                            row[2] = mappedName;
                            row[3] = accntNo;
                            row[4] = accountTypeName;
                            row[5] = accountTypeID;

                            dtTPaccounts.Rows.Add(row);
                        }
                    }
                    else
                    {
                        int accountID = int.MinValue;
                        string accountName = "";
                        string mappedName = "";
                        string accntNo = "";
                        int accountTypeId = int.MinValue;
                        AccountType accountType = CompanyManager.GetsAccountType(accountTypeId);
                        string accountTypeName = "";

                        row[0] = accountID;
                        row[1] = accountName;
                        row[2] = mappedName;
                        row[3] = accntNo;
                        row[4] = accountTypeName;
                        row[5] = accountTypeId;

                        dtTPaccounts.Rows.Add(row);
                    }

                }
            }
            grdAccountInformation.DataSource = dtTPaccounts;
        }

        /// <summary>
        /// this is to open up a dailog box on click of browse button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            Control btn = sender as Control;

            if (folderDialog.ShowDialog(btn.FindForm()) == DialogResult.OK)
            {
                txtSavePath.Text = folderDialog.SelectedPath;
                return;
            }

            //string _fullName = string.Empty;
            //string _shortName = string.Empty;

            //saveFileDialog1.InitialDirectory = "DeskTop";
            //saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
            //saveFileDialog1.ShowDialog();

            //string strFn = saveFileDialog1.FileName;
            //if (strFn.ToString() != "")
            //{

            //    FileInfo fiImage = new FileInfo(strFn);

            //    _fullName = fiImage.FullName;
            //    _shortName = fiImage.Name;

            //    FileStream fs = new FileStream(strFn, FileMode.Create,
            //        FileAccess.ReadWrite, FileShare.ReadWrite);


            //    fs.Close();

            //    txtNamingConvention.Text = _shortName;
            //    txtSavePath.Text = _fullName;
            //}
        }

        /// <summary>
        /// the method is used to validate the data entered in the mapping details usercontrol.
        /// </summary>
        /// <returns></returns>
        public bool MappingDetailsValidation()
        {
            bool validationSuccess = true;

            errorProvider1.SetError(txtCompanyIdentifier, "");
            errorProvider1.SetError(txtSavePath, "");
            // errorProvider1.SetError(txtNamingConvention, "");

            if (txtCompanyIdentifier.Text.Trim() == "")
            {
                errorProvider1.SetError(txtCompanyIdentifier, "Please enter the client identifier !");
                txtCompanyIdentifier.Focus();
                validationSuccess = false;
                return validationSuccess;
            }
            else if (txtSavePath.Text.Trim() == "")
            {
                errorProvider1.SetError(txtSavePath, "Please enter the save destination!");
                txtSavePath.Focus();
                validationSuccess = false;
                return validationSuccess;
            }
            //else if (txtNamingConvention.Text.Trim() == "")
            //{
            //    errorProvider1.SetError(txtNamingConvention, "PLease enter a name for the file to be saved !");
            //    txtNamingConvention.Focus();
            //    validationSuccess = false;
            //    return validationSuccess;
            //}
            //else if (txtNamingConvention.Text.Trim() != "")
            //{
            //    string FileName = txtNamingConvention.Text.Trim().ToString();              
            //    int lengthofFile = FileName.Length;
            //    int startIndex = FileName.IndexOf("{");
            //    int lastIndex = FileName.LastIndexOf("}");
            //    string FileNameSubString = string.Empty;
            //    if (startIndex != -1 && lastIndex != -1)
            //    {
            //        FileNameSubString = FileName.Substring(startIndex + 1, (lastIndex - startIndex) - 1);
            //    }            

            //    if (FileNameSubString == string.Empty || FileNameSubString.Length < 8)
            //    {
            //        MessageBox.Show("Please enter the proper Date Format eg : {MMddyyyy}","Admin Info");
            //        txtNamingConvention.Focus();
            //        validationSuccess = false;
            //        return validationSuccess;
            //    }             

            //}
            else if (grdAccountInformation.Rows.Count == 0)
            {
                MessageBox.Show("There are no accounts permitted for the selected Third Party. Please set account permission before saving Mapping Details.");
                validationSuccess = false;
                return validationSuccess;
            }
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdAccountInformation.Rows)
            {
                string mappedName = string.Empty;
                string accountAccntNo = string.Empty;
                string accountName = string.Empty;
                mappedName = dr.Cells["Mapped Account Name"].Text.ToString();
                accountAccntNo = dr.Cells["Account A/C #"].Text.ToString();
                accountName = dr.Cells["Internal Account Name"].Text;
                int index = int.MinValue;
                index = dr.Index;
                if (mappedName == "")
                {
                    MessageBox.Show("Please enter a Mapping name for the account!");
                    validationSuccess = false;
                    return validationSuccess;
                }
                else if (mappedName != "")
                {
                    bool mappedNameValid = MappedNameRepeatChk(mappedName, index);
                    if (!mappedNameValid)
                    {
                        MessageBox.Show("The Mapped Account Name for the Account '" + accountName + "' has already been assigned to some other Account, please enter a new mapped AccountName.");
                        validationSuccess = false;
                        return validationSuccess;
                    }
                }
                else if (accountAccntNo == "")
                {
                    MessageBox.Show("Please enter the Account Account No. !");
                    validationSuccess = false;
                    return validationSuccess;
                }
                else if (accountAccntNo != "")
                {
                    bool accountNoValid = AccountAccntNoRepeatChk(accountAccntNo, index);
                    if (!accountNoValid)
                    {
                        MessageBox.Show("The Account A/C No for the account '" + accountName + "' has already been assigned to some other Account, please enter a new Account A/C No.");
                        validationSuccess = false;
                        return validationSuccess;
                    }
                }


            }
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow cvdr in grdCVIdentifier.Rows)
            {
                string cVIdentifier = string.Empty;
                string cVName = string.Empty;
                cVIdentifier = cvdr.Cells["CV Identifier"].Text.ToString();
                cVName = cvdr.Cells["CounterParty Venue"].Text.ToString();
                int index = cvdr.Index;
                //if (cVIdentifier == "")
                //{
                //    MessageBox.Show("Please enter an identifier for the CV !");
                //    validationSuccess = false;
                //    return validationSuccess;
                //}
                //else 
                if (cVIdentifier != "")
                {
                    bool cVIDValid = CVIdentifierRepeatChk(cVIdentifier, index);
                    if (!cVIDValid)
                    {
                        MessageBox.Show("The CVIdentifier set for the CV '" + cVName + "' has already been assigned to some other CV, please enter a new cv Identifier.");
                        validationSuccess = false;
                        return validationSuccess;
                    }
                }
            }


            return validationSuccess;
        }

        /// <summary>
        /// the method is used to check whether entered cv identifier for repetition.
        /// </summary>
        /// <param name="cVIdentifier"></param>
        /// <returns></returns>
        private bool CVIdentifierRepeatChk(string cVIdentifier, int index)
        {

            bool notRepeated = true;
            if (grdCVIdentifier.Rows.Count > 1)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCVIdentifier.Rows)
                {
                    string selectedCVID = dr.Cells["CV Identifier"].Value.ToString();
                    int checkedindex = dr.Index;
                    if (selectedCVID == cVIdentifier && checkedindex != index)
                    {
                        notRepeated = false;
                        break;
                    }
                }
            }
            return notRepeated;
        }

        /// <summary>
        /// the method is used to check for the repetition of the account account no.
        /// </summary>
        /// <param name="accountAccntNo"></param>
        /// <returns></returns>
        private bool AccountAccntNoRepeatChk(string accountAccntNo, int index)
        {
            bool notRepeated = true;
            if (grdAccountInformation.Rows.Count > 1)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdAccountInformation.Rows)
                {
                    string selectedAccountNo = dr.Cells["Account A/C "].Value.ToString();
                    int checkedindex = dr.Index;
                    if (selectedAccountNo == accountAccntNo && checkedindex != index)
                    {
                        notRepeated = false;
                        break;
                    }
                }
            }
            return notRepeated;
        }

        /// <summary>
        /// the method is used to check for the repetition of the mapped account name
        /// </summary>
        /// <param name="mappedName"></param>
        /// <returns></returns>
        private bool MappedNameRepeatChk(string mappedName, int index)
        {
            bool notRepeated = true;
            if (grdAccountInformation.Rows.Count > 1)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdAccountInformation.Rows)
                {
                    string selectedMappedName = dr.Cells["Mapped Account Name"].Value.ToString();
                    int checkedindex = dr.Index;
                    if (selectedMappedName == mappedName && checkedindex != index)
                    {
                        notRepeated = false;
                        break;
                    }
                }
            }
            return notRepeated;
        }

        /// <summary>
        /// this event is raised to set the not required columns as hidden and to set the text alignmnt correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCVIdentifier_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            ColumnsCollection columns = grdCVIdentifier.DisplayLayout.Bands[0].Columns;
            columns["CounterParty Venue"].Header.Caption = CAP_BrokerVenue;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key == "CounterPartyVenueID")
                {
                    column.Hidden = true;
                }
                else
                {
                    column.Hidden = false;
                    column.Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                    column.Header.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;
                }

            }

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCVIdentifier.Rows)
            {
                dr.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                dr.CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

                dr.Cells["CV Identifier"].Activation = Activation.AllowEdit;
                dr.Cells["CounterParty Venue"].Activation = Activation.NoEdit;
            }
        }

        /// <summary>
        ///  this event is raised to set the not required columns as hidden and to set the text alignmnt correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAccountInformation_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            ColumnsCollection columns = grdAccountInformation.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key == "Account ID")
                {
                    column.Hidden = true;

                }
                else
                {
                    if (column.Key == "AccountTypeID")
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        column.Hidden = false;
                    }


                }

                column.Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                column.Header.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            }
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdAccountInformation.Rows)
            {
                dr.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                dr.CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

                dr.Cells["Internal Account Name"].Activation = Activation.NoEdit;
                dr.Cells["Mapped Account Name"].Activation = Activation.AllowEdit;
                dr.Cells["Account A/C #"].Activation = Activation.AllowEdit;
                dr.Cells["Account Type"].Activation = Activation.NoEdit;
            }
        }

        /// <summary>
        /// the method is used to save the mapping details entered in the grid.
        /// </summary>
        public int SaveMappingDetails()
        {
            int result = int.MinValue;
            ThirdPartyAccounts thirdPartyAccounts = new ThirdPartyAccounts();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdAccountInformation.Rows)
            {
                ThirdPartyAccount thirdPartyAccount = new ThirdPartyAccount();

                thirdPartyAccount.InternalAccountID = Convert.ToInt32(dr.Cells["Account ID"].Value);
                thirdPartyAccount.MappedAccountName = dr.Cells["Mapped Account Name"].Text;
                thirdPartyAccount.Account = dr.Cells["Account A/C #"].Text;
                thirdPartyAccount.AccountTypeID = Convert.ToInt32(dr.Cells["AccountTypeID"].Value);
                thirdPartyAccount.CompanyID_FK = _companyID;
                thirdPartyAccount.CompanyThirdPartyID_FK = _companyThirdPartyID;

                thirdPartyAccounts.Add(thirdPartyAccount);

            }

            result = CompanyManager.SaveCompanyThirdMappingDetails(thirdPartyAccounts, _companyThirdPartyID);
            return result;
        }

        /// <summary>
        /// the method is used to save all the cv identifier details entered in the grid by the user.
        /// </summary>
        public int SaveCVIdentifier()
        {
            int result = int.MinValue;
            ThirdPartyCVIdentifiers thirdPartyCVIdentifiers = new ThirdPartyCVIdentifiers();

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCVIdentifier.Rows)
            {
                ThirdPartyCVIdentifier thirdPartyCVIdentifier = new ThirdPartyCVIdentifier();

                thirdPartyCVIdentifier.CompanyThirdPartyID = _companyThirdPartyID;
                thirdPartyCVIdentifier.CompanyCounterPartyVenueID = Convert.ToInt32(dr.Cells["CounterPartyVenueID"].Value);
                thirdPartyCVIdentifier.CVIdentifier = dr.Cells["CV Identifier"].Text;

                thirdPartyCVIdentifiers.Add(thirdPartyCVIdentifier);

            }
            result = CompanyManager.SaveCompanyThirdPartyCVIdentifiers(thirdPartyCVIdentifiers, _companyThirdPartyID);
            return result;
        }

        /// <summary>
        /// the method is used to save the save details if the thirdparty flat file .
        /// </summary>
        public int SaveSaveDetail()
        {
            int result = int.MinValue;
            ThirdPartyFlatFileSaveDetail thirdPartyFlatFileSaveDetail = new ThirdPartyFlatFileSaveDetail();
            thirdPartyFlatFileSaveDetail.CompanyIdentifier = txtCompanyIdentifier.Text;
            thirdPartyFlatFileSaveDetail.CompanyThirdPartyID = _companyThirdPartyID;
            thirdPartyFlatFileSaveDetail.NamingConvention = txtNamingConvention.Text;
            thirdPartyFlatFileSaveDetail.SaveGeneratedFileIn = txtSavePath.Text;

            result = CompanyManager.SaveCompanyThirdPartyFileSaveDetail(thirdPartyFlatFileSaveDetail);
            return result;
        }

        #region Focus Property
        private void txtCompanyIdentifier_Enter(object sender, EventArgs e)
        {

        }

        private void txtCompanyIdentifier_Leave(object sender, EventArgs e)
        {

        }

        private void txtSavePath_Enter(object sender, EventArgs e)
        {

        }

        private void txtSavePath_Leave(object sender, EventArgs e)
        {

        }


        #endregion Focus Property

        #region not req
        //private void grdCVIdentifier_CellChange(object sender, CellEventArgs e)
        //{
        //    ValidateCVIDRepetition();

        //}

        //private void ValidateCVIDRepetition()
        //{
        //    string enteredData = grdCVIdentifier.ActiveRow.Cells["CV Identifier"].Value.ToString();
        //    string blank = string.Empty;
        //    if (enteredData != "")
        //    {
        //        string cVID = grdCVIdentifier.ActiveRow.Cells["CV Identifier"].Text;

        //        int currentIndex = grdCVIdentifier.ActiveRow.Index;

        //        int checkIndex = int.MinValue;
        //        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCVIdentifier.Rows)
        //        {
        //            string exitingCVID = dr.Cells["CV Identifier"].Value.ToString();
        //            checkIndex = dr.Index;
        //            if (cVID == exitingCVID && checkIndex != currentIndex)
        //            {
        //                MessageBox.Show(this, "This CV Identifier has already been assigned to some CV, you cannot assign it to this CV.", "CounterParty Venue", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                grdCVIdentifier.Rows[currentIndex].Cells["CV Identifier"].Value = blank;
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void MappedNameRepetitionChk()
        //{
        //    string enteredData = grdAccountInformation.ActiveRow.Cells["Mapped Account Name"].Value.ToString();
        //    string blank = string.Empty;
        //    if (enteredData != "")
        //    {
        //        string cVID = grdAccountInformation.ActiveRow.Cells["Mapped Account Name"].Text;

        //        int currentIndex = grdAccountInformation.ActiveRow.Index;

        //        int checkIndex = int.MinValue;
        //        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdAccountInformation.Rows)
        //        {
        //            string exitingCVID = dr.Cells["Mapped Account Name"].Value.ToString();
        //            checkIndex = dr.Index;
        //            if (cVID == exitingCVID && checkIndex != currentIndex)
        //            {
        //                MessageBox.Show(this, "This Mapped Account Name has already been assigned to some Account, you cannot assign it to this Account.", "Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                grdAccountInformation.Rows[currentIndex].Cells["Mapped Account Name"].Value = blank;
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void AccountRepetitionChk()
        //{
        //    string enteredData = grdAccountInformation.ActiveRow.Cells["Account A/C #"].Value.ToString();
        //    string blank = string.Empty;
        //    if (enteredData != "")
        //    {
        //        string cVID = grdAccountInformation.ActiveRow.Cells["Account A/C #"].Text;

        //        int currentIndex = grdAccountInformation.ActiveRow.Index;

        //        int checkIndex = int.MinValue;
        //        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdAccountInformation.Rows)
        //        {
        //            string exitingCVID = dr.Cells["Account A/C #"].Value.ToString();
        //            checkIndex = dr.Index;
        //            if (cVID == exitingCVID && checkIndex != currentIndex)
        //            {
        //                MessageBox.Show(this, "This Account A/C # has already been assigned to some Account, you cannot assign it to this Account.", "Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                grdAccountInformation.Rows[currentIndex].Cells["Account A/C #"].Value = blank;
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void grdAccountInformation_CellChange(object sender, CellEventArgs e)
        //{        
        //    if (grdAccountInformation.ActiveCell.Column.Key == "Mapped Account Name")
        //    {
        //       MappedNameRepetitionChk();     
        //    }
        //    else if (grdAccountInformation.ActiveCell.Column.Key == "Account A/C #")
        //    {
        //        AccountRepetitionChk();
        //    }

        //}
        #endregion not req

    }
}
