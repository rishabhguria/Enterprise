using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommissionRules;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyThirdPartyCommissionRules.
    /// </summary>
    public class CompanyThirdPartyCommissionRules : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "CompanyThirdPartyCommissionRules : ";
        const string C_COMBO_SELECT = "- Select -";
        const int MIN_VALUE = int.MinValue;

        const int SINGLE_RULEID = 1;
        const int BASKET_RULEID = 2;

        const int ASSET_EQUITY = 1;
        const int ASSET_EQUITYOPTION = 2;
        const int ASSET_FUTURE = 3;
        const int ASSET_FUTUREOPTION = 4;
        const int ASSET_FOREIGNEXCHANGE = 5;
        const int ASSET_CASH = 6;
        const int ASSET_INDICES = 7;
        const int ASSET_FIXEDINCOME = 8;
        const int ASSET_CONVERTIBLEBOND = 13;

        private System.Windows.Forms.GroupBox grpModifyCommissionRules;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCommissionRules;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCompanyAccounts;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCompanyCounterPartyVenues;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCVAUEC;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownSingleRule;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownBasketRule;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCVAUECNew;
        private GroupBox grpBoxAddRule;
        private Label lblSelectCommissionRule;
        private CheckedListBox checkedlstCompanyCVAUEC;
        private Label lblBasketRule;
        private Label lblSingleRule;
        private Button btnUpdateCommissionRules;
        private RadioButton radioButtonAddRule;
        private RadioButton radioButtonModifyRule;
        private GroupBox grpBoxCommissionCalculationType;
        private RadioButton radioButtonPostAllocation;
        private RadioButton radioButtonPreAllocation;
        private Label label2;
        private CheckedListBox checkedListBoxAccounts;
        private UltraCombo cmbBasketRule;
        private UltraCombo cmbSingleRule;
        private CheckBox chkBoxSelectAllAccounts;
        private Button btnExportCommissionRules;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private IContainer components;

        public CompanyThirdPartyCommissionRules()
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
                if (grpModifyCommissionRules != null)
                {
                    grpModifyCommissionRules.Dispose();
                }
                if (grdCommissionRules != null)
                {
                    grdCommissionRules.Dispose();
                }
                if (ultraDropDownCompanyAccounts != null)
                {
                    ultraDropDownCompanyAccounts.Dispose();
                }
                if (ultraDropDownCompanyCounterPartyVenues != null)
                {
                    ultraDropDownCompanyCounterPartyVenues.Dispose();
                }
                if (ultraDropDownCVAUEC != null)
                {
                    ultraDropDownCVAUEC.Dispose();
                }
                if (ultraDropDownSingleRule != null)
                {
                    ultraDropDownSingleRule.Dispose();
                }
                if (ultraDropDownBasketRule != null)
                {
                    ultraDropDownBasketRule.Dispose();
                }
                if (ultraDropDownCVAUECNew != null)
                {
                    ultraDropDownCVAUECNew.Dispose();
                }
                if (grpBoxAddRule != null)
                {
                    grpBoxAddRule.Dispose();
                }
                if (lblSelectCommissionRule != null)
                {
                    lblSelectCommissionRule.Dispose();
                }
                if (checkedlstCompanyCVAUEC != null)
                {
                    checkedlstCompanyCVAUEC.Dispose();
                }
                if (lblBasketRule != null)
                {
                    lblBasketRule.Dispose();
                }
                if (lblSingleRule != null)
                {
                    lblSingleRule.Dispose();
                }
                if (btnUpdateCommissionRules != null)
                {
                    btnUpdateCommissionRules.Dispose();
                }
                if (btnUpdateCommissionRules != null)
                {
                    btnUpdateCommissionRules.Dispose();
                }
                if (radioButtonAddRule != null)
                {
                    radioButtonAddRule.Dispose();
                }
                if (radioButtonModifyRule != null)
                {
                    radioButtonModifyRule.Dispose();
                }
                if (grpBoxCommissionCalculationType != null)
                {
                    grpBoxCommissionCalculationType.Dispose();
                }
                if (radioButtonPostAllocation != null)
                {
                    radioButtonPostAllocation.Dispose();
                }
                if (radioButtonPreAllocation != null)
                {
                    radioButtonPreAllocation.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (checkedListBoxAccounts != null)
                {
                    checkedListBoxAccounts.Dispose();
                }
                if (cmbBasketRule != null)
                {
                    cmbBasketRule.Dispose();
                }
                if (cmbSingleRule != null)
                {
                    cmbSingleRule.Dispose();
                }
                if (chkBoxSelectAllAccounts != null)
                {
                    chkBoxSelectAllAccounts.Dispose();
                }
                if (btnExportCommissionRules != null)
                {
                    btnExportCommissionRules.Dispose();
                }
                if(ultraGridExcelExporter1 != null)
                {
                    ultraGridExcelExporter1.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeleteButton", 0);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyThirdPartyCommissionRules));
            this.grpModifyCommissionRules = new System.Windows.Forms.GroupBox();
            this.grdCommissionRules = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDropDownCompanyAccounts = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownCompanyCounterPartyVenues = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownCVAUEC = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownSingleRule = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownBasketRule = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownCVAUECNew = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.grpBoxAddRule = new System.Windows.Forms.GroupBox();
            this.chkBoxSelectAllAccounts = new System.Windows.Forms.CheckBox();
            this.cmbBasketRule = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbSingleRule = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.checkedListBoxAccounts = new System.Windows.Forms.CheckedListBox();
            this.btnUpdateCommissionRules = new System.Windows.Forms.Button();
            this.lblBasketRule = new System.Windows.Forms.Label();
            this.lblSingleRule = new System.Windows.Forms.Label();
            this.lblSelectCommissionRule = new System.Windows.Forms.Label();
            this.checkedlstCompanyCVAUEC = new System.Windows.Forms.CheckedListBox();
            this.radioButtonAddRule = new System.Windows.Forms.RadioButton();
            this.radioButtonModifyRule = new System.Windows.Forms.RadioButton();
            this.grpBoxCommissionCalculationType = new System.Windows.Forms.GroupBox();
            this.radioButtonPostAllocation = new System.Windows.Forms.RadioButton();
            this.radioButtonPreAllocation = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExportCommissionRules = new System.Windows.Forms.Button();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.grpModifyCommissionRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyCounterPartyVenues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCVAUEC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownSingleRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownBasketRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCVAUECNew)).BeginInit();
            this.grpBoxAddRule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBasketRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSingleRule)).BeginInit();
            this.grpBoxCommissionCalculationType.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpModifyCommissionRules
            // 
            this.grpModifyCommissionRules.Controls.Add(this.grdCommissionRules);
            this.grpModifyCommissionRules.Location = new System.Drawing.Point(19, 252);
            this.grpModifyCommissionRules.Name = "grpModifyCommissionRules";
            this.grpModifyCommissionRules.Size = new System.Drawing.Size(614, 175);
            this.grpModifyCommissionRules.TabIndex = 0;
            this.grpModifyCommissionRules.TabStop = false;
            // 
            // grdCommissionRules
            // 
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance1.Image = global::Prana.Admin.Controls.Properties.Resources.delete;
            appearance1.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            ultraGridColumn1.CellButtonAppearance = appearance1;
            ultraGridColumn1.Header.Caption = "Delete";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1});
            this.grdCommissionRules.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCommissionRules.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCommissionRules.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCommissionRules.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCommissionRules.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCommissionRules.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdCommissionRules.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCommissionRules.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCommissionRules.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCommissionRules.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCommissionRules.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCommissionRules.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCommissionRules.Location = new System.Drawing.Point(8, 14);
            this.grdCommissionRules.Name = "grdCommissionRules";
            this.grdCommissionRules.Size = new System.Drawing.Size(603, 154);
            this.grdCommissionRules.TabIndex = 0;
            this.grdCommissionRules.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCommissionRules.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCommissionRules_InitializeLayout);
            this.grdCommissionRules.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdCommissionRules_InitializeRow);
            this.grdCommissionRules.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_CellChange);
            this.grdCommissionRules.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_ClickCellButton);
            // 
            // ultraDropDownCompanyAccounts
            // 
            this.ultraDropDownCompanyAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCompanyAccounts.Location = new System.Drawing.Point(679, 399);
            this.ultraDropDownCompanyAccounts.Name = "ultraDropDownCompanyAccounts";
            this.ultraDropDownCompanyAccounts.Size = new System.Drawing.Size(106, 36);
            this.ultraDropDownCompanyAccounts.TabIndex = 11;
            this.ultraDropDownCompanyAccounts.Text = "ultraDropDown1";
            this.ultraDropDownCompanyAccounts.Visible = false;
            // 
            // ultraDropDownCompanyCounterPartyVenues
            // 
            this.ultraDropDownCompanyCounterPartyVenues.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCompanyCounterPartyVenues.Location = new System.Drawing.Point(679, 370);
            this.ultraDropDownCompanyCounterPartyVenues.Name = "ultraDropDownCompanyCounterPartyVenues";
            this.ultraDropDownCompanyCounterPartyVenues.Size = new System.Drawing.Size(106, 36);
            this.ultraDropDownCompanyCounterPartyVenues.TabIndex = 12;
            this.ultraDropDownCompanyCounterPartyVenues.Text = "ultraDropDown1";
            this.ultraDropDownCompanyCounterPartyVenues.Visible = false;
            // 
            // ultraDropDownCVAUEC
            // 
            this.ultraDropDownCVAUEC.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraDropDownCVAUEC.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCVAUEC.DropDownWidth = 0;
            this.ultraDropDownCVAUEC.Location = new System.Drawing.Point(679, 339);
            this.ultraDropDownCVAUEC.Name = "ultraDropDownCVAUEC";
            this.ultraDropDownCVAUEC.Size = new System.Drawing.Size(106, 36);
            this.ultraDropDownCVAUEC.TabIndex = 13;
            this.ultraDropDownCVAUEC.Text = "ultraDropDown1";
            this.ultraDropDownCVAUEC.Visible = false;
            // 
            // ultraDropDownSingleRule
            // 
            this.ultraDropDownSingleRule.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownSingleRule.Location = new System.Drawing.Point(679, 275);
            this.ultraDropDownSingleRule.Name = "ultraDropDownSingleRule";
            this.ultraDropDownSingleRule.Size = new System.Drawing.Size(106, 36);
            this.ultraDropDownSingleRule.TabIndex = 14;
            this.ultraDropDownSingleRule.Text = "ultraDropDown2";
            this.ultraDropDownSingleRule.Visible = false;
            this.ultraDropDownSingleRule.TextChanged += new System.EventHandler(this.ultraDropDownSingleRule_TextChanged);
            // 
            // ultraDropDownBasketRule
            // 
            this.ultraDropDownBasketRule.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownBasketRule.Location = new System.Drawing.Point(679, 243);
            this.ultraDropDownBasketRule.Name = "ultraDropDownBasketRule";
            this.ultraDropDownBasketRule.Size = new System.Drawing.Size(106, 36);
            this.ultraDropDownBasketRule.TabIndex = 15;
            this.ultraDropDownBasketRule.Text = "ultraDropDown2";
            this.ultraDropDownBasketRule.Visible = false;
            // 
            // ultraDropDownCVAUECNew
            // 
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraDropDownCVAUECNew.DisplayLayout.Appearance = appearance2;
            this.ultraDropDownCVAUECNew.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraDropDownCVAUECNew.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownCVAUECNew.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownCVAUECNew.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.ultraDropDownCVAUECNew.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownCVAUECNew.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.ultraDropDownCVAUECNew.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraDropDownCVAUECNew.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.CellAppearance = appearance9;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlignAsString = "Left";
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.RowAppearance = appearance12;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraDropDownCVAUECNew.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.ultraDropDownCVAUECNew.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCVAUECNew.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraDropDownCVAUECNew.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraDropDownCVAUECNew.Location = new System.Drawing.Point(679, 307);
            this.ultraDropDownCVAUECNew.Name = "ultraDropDownCVAUECNew";
            this.ultraDropDownCVAUECNew.Size = new System.Drawing.Size(104, 36);
            this.ultraDropDownCVAUECNew.TabIndex = 16;
            this.ultraDropDownCVAUECNew.Text = "ultraDropDown1";
            this.ultraDropDownCVAUECNew.Visible = false;
            // 
            // grpBoxAddRule
            // 
            this.grpBoxAddRule.Controls.Add(this.chkBoxSelectAllAccounts);
            this.grpBoxAddRule.Controls.Add(this.cmbBasketRule);
            this.grpBoxAddRule.Controls.Add(this.cmbSingleRule);
            this.grpBoxAddRule.Controls.Add(this.checkedListBoxAccounts);
            this.grpBoxAddRule.Controls.Add(this.btnUpdateCommissionRules);
            this.grpBoxAddRule.Controls.Add(this.lblBasketRule);
            this.grpBoxAddRule.Controls.Add(this.lblSingleRule);
            this.grpBoxAddRule.Controls.Add(this.lblSelectCommissionRule);
            this.grpBoxAddRule.Controls.Add(this.checkedlstCompanyCVAUEC);
            this.grpBoxAddRule.Location = new System.Drawing.Point(19, 64);
            this.grpBoxAddRule.Name = "grpBoxAddRule";
            this.grpBoxAddRule.Size = new System.Drawing.Size(614, 170);
            this.grpBoxAddRule.TabIndex = 4;
            this.grpBoxAddRule.TabStop = false;
            // 
            // chkBoxSelectAllAccounts
            // 
            this.chkBoxSelectAllAccounts.AutoSize = true;
            this.chkBoxSelectAllAccounts.Location = new System.Drawing.Point(172, 142);
            this.chkBoxSelectAllAccounts.Name = "chkBoxSelectAllAccounts";
            this.chkBoxSelectAllAccounts.Size = new System.Drawing.Size(101, 17);
            this.chkBoxSelectAllAccounts.TabIndex = 6;
            this.chkBoxSelectAllAccounts.Text = "Select All Accounts";
            this.chkBoxSelectAllAccounts.UseVisualStyleBackColor = true;
            this.chkBoxSelectAllAccounts.CheckedChanged += new System.EventHandler(this.chkBoxSelectAllAccounts_CheckedChanged);
            // 
            // cmbBasketRule
            // 
            this.cmbBasketRule.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBasketRule.DisplayLayout.Appearance = appearance14;
            this.cmbBasketRule.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn2.Header.VisiblePosition = 0;
            ultraGridColumn3.Header.VisiblePosition = 1;
            ultraGridColumn3.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn2,
            ultraGridColumn3});
            this.cmbBasketRule.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbBasketRule.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBasketRule.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBasketRule.DisplayLayout.GroupByBox.Appearance = appearance15;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBasketRule.DisplayLayout.GroupByBox.BandLabelAppearance = appearance16;
            this.cmbBasketRule.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBasketRule.DisplayLayout.GroupByBox.PromptAppearance = appearance17;
            this.cmbBasketRule.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBasketRule.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBasketRule.DisplayLayout.Override.ActiveCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.SystemColors.Highlight;
            appearance19.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBasketRule.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            this.cmbBasketRule.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBasketRule.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBasketRule.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBasketRule.DisplayLayout.Override.CellAppearance = appearance21;
            this.cmbBasketRule.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBasketRule.DisplayLayout.Override.CellPadding = 0;
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBasketRule.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            appearance23.TextHAlignAsString = "Left";
            this.cmbBasketRule.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.cmbBasketRule.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBasketRule.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            this.cmbBasketRule.DisplayLayout.Override.RowAppearance = appearance24;
            this.cmbBasketRule.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBasketRule.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.cmbBasketRule.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBasketRule.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBasketRule.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBasketRule.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbBasketRule.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBasketRule.DropDownWidth = 0;
            this.cmbBasketRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbBasketRule.Location = new System.Drawing.Point(44, 74);
            this.cmbBasketRule.Name = "cmbBasketRule";
            this.cmbBasketRule.Size = new System.Drawing.Size(121, 21);
            this.cmbBasketRule.TabIndex = 3;
            this.cmbBasketRule.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbBasketRule.ValueChanged += new System.EventHandler(this.cmbBasketRule_ValueChanged);
            // 
            // cmbSingleRule
            // 
            this.cmbSingleRule.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSingleRule.DisplayLayout.Appearance = appearance26;
            this.cmbSingleRule.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn5.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5});
            this.cmbSingleRule.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbSingleRule.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSingleRule.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSingleRule.DisplayLayout.GroupByBox.Appearance = appearance27;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSingleRule.DisplayLayout.GroupByBox.BandLabelAppearance = appearance28;
            this.cmbSingleRule.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance29.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance29.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSingleRule.DisplayLayout.GroupByBox.PromptAppearance = appearance29;
            this.cmbSingleRule.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSingleRule.DisplayLayout.MaxRowScrollRegions = 1;
            appearance30.BackColor = System.Drawing.SystemColors.Window;
            appearance30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSingleRule.DisplayLayout.Override.ActiveCellAppearance = appearance30;
            appearance31.BackColor = System.Drawing.SystemColors.Highlight;
            appearance31.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSingleRule.DisplayLayout.Override.ActiveRowAppearance = appearance31;
            this.cmbSingleRule.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSingleRule.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSingleRule.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BorderColor = System.Drawing.Color.Silver;
            appearance33.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSingleRule.DisplayLayout.Override.CellAppearance = appearance33;
            this.cmbSingleRule.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSingleRule.DisplayLayout.Override.CellPadding = 0;
            appearance34.BackColor = System.Drawing.SystemColors.Control;
            appearance34.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance34.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance34.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSingleRule.DisplayLayout.Override.GroupByRowAppearance = appearance34;
            appearance35.TextHAlignAsString = "Left";
            this.cmbSingleRule.DisplayLayout.Override.HeaderAppearance = appearance35;
            this.cmbSingleRule.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSingleRule.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance36.BackColor = System.Drawing.SystemColors.Window;
            appearance36.BorderColor = System.Drawing.Color.Silver;
            this.cmbSingleRule.DisplayLayout.Override.RowAppearance = appearance36;
            this.cmbSingleRule.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance37.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSingleRule.DisplayLayout.Override.TemplateAddRowAppearance = appearance37;
            this.cmbSingleRule.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSingleRule.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSingleRule.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSingleRule.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbSingleRule.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSingleRule.DropDownWidth = 0;
            this.cmbSingleRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbSingleRule.Location = new System.Drawing.Point(44, 44);
            this.cmbSingleRule.Name = "cmbSingleRule";
            this.cmbSingleRule.Size = new System.Drawing.Size(121, 21);
            this.cmbSingleRule.TabIndex = 2;
            this.cmbSingleRule.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbSingleRule.ValueChanged += new System.EventHandler(this.cmbSingleRule_ValueChanged);
            // 
            // checkedListBoxAccounts
            // 
            this.checkedListBoxAccounts.CheckOnClick = true;
            this.checkedListBoxAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedListBoxAccounts.Location = new System.Drawing.Point(172, 19);
            this.checkedListBoxAccounts.Name = "checkedListBoxAccounts";
            this.checkedListBoxAccounts.Size = new System.Drawing.Size(118, 116);
            this.checkedListBoxAccounts.TabIndex = 4;
            // 
            // btnUpdateCommissionRules
            // 
            this.btnUpdateCommissionRules.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdateCommissionRules.BackgroundImage")));
            this.btnUpdateCommissionRules.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdateCommissionRules.Location = new System.Drawing.Point(364, 142);
            this.btnUpdateCommissionRules.Name = "btnUpdateCommissionRules";
            this.btnUpdateCommissionRules.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateCommissionRules.TabIndex = 7;
            this.btnUpdateCommissionRules.UseVisualStyleBackColor = false;
            this.btnUpdateCommissionRules.Click += new System.EventHandler(this.btnUpdateCommissionRules_Click);
            // 
            // lblBasketRule
            // 
            this.lblBasketRule.AutoSize = true;
            this.lblBasketRule.Location = new System.Drawing.Point(3, 76);
            this.lblBasketRule.Name = "lblBasketRule";
            this.lblBasketRule.Size = new System.Drawing.Size(39, 13);
            this.lblBasketRule.TabIndex = 46;
            this.lblBasketRule.Text = "Basket";
            // 
            // lblSingleRule
            // 
            this.lblSingleRule.AutoSize = true;
            this.lblSingleRule.Location = new System.Drawing.Point(3, 44);
            this.lblSingleRule.Name = "lblSingleRule";
            this.lblSingleRule.Size = new System.Drawing.Size(35, 13);
            this.lblSingleRule.TabIndex = 44;
            this.lblSingleRule.Text = "Single";
            // 
            // lblSelectCommissionRule
            // 
            this.lblSelectCommissionRule.AutoSize = true;
            this.lblSelectCommissionRule.Location = new System.Drawing.Point(3, 17);
            this.lblSelectCommissionRule.Name = "lblSelectCommissionRule";
            this.lblSelectCommissionRule.Size = new System.Drawing.Size(122, 13);
            this.lblSelectCommissionRule.TabIndex = 1;
            this.lblSelectCommissionRule.Text = "Select Commission Rule:";
            // 
            // checkedlstCompanyCVAUEC
            // 
            this.checkedlstCompanyCVAUEC.CheckOnClick = true;
            this.checkedlstCompanyCVAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstCompanyCVAUEC.Location = new System.Drawing.Point(297, 20);
            this.checkedlstCompanyCVAUEC.Name = "checkedlstCompanyCVAUEC";
            this.checkedlstCompanyCVAUEC.Size = new System.Drawing.Size(314, 116);
            this.checkedlstCompanyCVAUEC.TabIndex = 5;
            this.checkedlstCompanyCVAUEC.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlstCompanyCVAUEC_ItemCheck);
            // 
            // radioButtonAddRule
            // 
            this.radioButtonAddRule.AutoSize = true;
            this.radioButtonAddRule.Location = new System.Drawing.Point(19, 46);
            this.radioButtonAddRule.Name = "radioButtonAddRule";
            this.radioButtonAddRule.Size = new System.Drawing.Size(68, 17);
            this.radioButtonAddRule.TabIndex = 3;
            this.radioButtonAddRule.TabStop = true;
            this.radioButtonAddRule.Text = "Add Rule";
            this.radioButtonAddRule.UseVisualStyleBackColor = true;
            this.radioButtonAddRule.CheckedChanged += new System.EventHandler(this.radioButtonAddRule_CheckedChanged);
            // 
            // radioButtonModifyRule
            // 
            this.radioButtonModifyRule.AutoSize = true;
            this.radioButtonModifyRule.Location = new System.Drawing.Point(19, 238);
            this.radioButtonModifyRule.Name = "radioButtonModifyRule";
            this.radioButtonModifyRule.Size = new System.Drawing.Size(81, 17);
            this.radioButtonModifyRule.TabIndex = 5;
            this.radioButtonModifyRule.TabStop = true;
            this.radioButtonModifyRule.Text = "Modify Rule";
            this.radioButtonModifyRule.UseVisualStyleBackColor = true;
            this.radioButtonModifyRule.CheckedChanged += new System.EventHandler(this.radioButtonModifyRule_CheckedChanged);
            // 
            // grpBoxCommissionCalculationType
            // 
            this.grpBoxCommissionCalculationType.Controls.Add(this.radioButtonPostAllocation);
            this.grpBoxCommissionCalculationType.Controls.Add(this.radioButtonPreAllocation);
            this.grpBoxCommissionCalculationType.Controls.Add(this.label2);
            this.grpBoxCommissionCalculationType.Location = new System.Drawing.Point(19, 3);
            this.grpBoxCommissionCalculationType.Name = "grpBoxCommissionCalculationType";
            this.grpBoxCommissionCalculationType.Size = new System.Drawing.Size(614, 38);
            this.grpBoxCommissionCalculationType.TabIndex = 1;
            this.grpBoxCommissionCalculationType.TabStop = false;
            // 
            // radioButtonPostAllocation
            // 
            this.radioButtonPostAllocation.AutoSize = true;
            this.radioButtonPostAllocation.Location = new System.Drawing.Point(344, 12);
            this.radioButtonPostAllocation.Name = "radioButtonPostAllocation";
            this.radioButtonPostAllocation.Size = new System.Drawing.Size(95, 17);
            this.radioButtonPostAllocation.TabIndex = 2;
            this.radioButtonPostAllocation.TabStop = true;
            this.radioButtonPostAllocation.Text = "Post Allocation";
            this.radioButtonPostAllocation.UseVisualStyleBackColor = true;
            this.radioButtonPostAllocation.CheckedChanged += new System.EventHandler(this.radioButtonPostAllocation_CheckedChanged);
            // 
            // radioButtonPreAllocation
            // 
            this.radioButtonPreAllocation.AutoSize = true;
            this.radioButtonPreAllocation.Location = new System.Drawing.Point(207, 12);
            this.radioButtonPreAllocation.Name = "radioButtonPreAllocation";
            this.radioButtonPreAllocation.Size = new System.Drawing.Size(90, 17);
            this.radioButtonPreAllocation.TabIndex = 1;
            this.radioButtonPreAllocation.TabStop = true;
            this.radioButtonPreAllocation.Text = "Pre Allocation";
            this.radioButtonPreAllocation.UseVisualStyleBackColor = true;
            this.radioButtonPreAllocation.CheckedChanged += new System.EventHandler(this.radioButtonPreAllocation_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Commission Calculation:";
            // 
            // btnExportCommissionRules
            // 
            this.btnExportCommissionRules.Location = new System.Drawing.Point(555, 433);
            this.btnExportCommissionRules.Name = "btnExportCommissionRules";
            this.btnExportCommissionRules.Size = new System.Drawing.Size(75, 23);
            this.btnExportCommissionRules.TabIndex = 6;
            this.btnExportCommissionRules.Text = "Export Rules";
            this.btnExportCommissionRules.UseVisualStyleBackColor = true;
            this.btnExportCommissionRules.Click += new System.EventHandler(this.btnExportCommissionRules_Click);
            // 
            // CompanyThirdPartyCommissionRules
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnExportCommissionRules);
            this.Controls.Add(this.grpBoxCommissionCalculationType);
            this.Controls.Add(this.radioButtonModifyRule);
            this.Controls.Add(this.radioButtonAddRule);
            this.Controls.Add(this.grpBoxAddRule);
            this.Controls.Add(this.ultraDropDownCVAUECNew);
            this.Controls.Add(this.ultraDropDownBasketRule);
            this.Controls.Add(this.ultraDropDownSingleRule);
            this.Controls.Add(this.ultraDropDownCVAUEC);
            this.Controls.Add(this.ultraDropDownCompanyCounterPartyVenues);
            this.Controls.Add(this.ultraDropDownCompanyAccounts);
            this.Controls.Add(this.grpModifyCommissionRules);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CompanyThirdPartyCommissionRules";
            this.Size = new System.Drawing.Size(757, 466);
            this.grpModifyCommissionRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCompanyCounterPartyVenues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCVAUEC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownSingleRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownBasketRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCVAUECNew)).EndInit();
            this.grpBoxAddRule.ResumeLayout(false);
            this.grpBoxAddRule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBasketRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSingleRule)).EndInit();
            this.grpBoxCommissionCalculationType.ResumeLayout(false);
            this.grpBoxCommissionCalculationType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        int _companyID = int.MinValue;
        List<CVAUECAccountCommissionRule> oldCVAUECCommissionRules = new List<CVAUECAccountCommissionRule>();
        Dictionary<int, List<CVAuecs>> cvAUECsStringNames = new Dictionary<int, List<CVAuecs>>();
        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            cvAUECsStringNames = CounterPartyManager.GetAllCompanyCVAUECsStringName(_companyID); ;
            BindComboAndListBox();
            BindDataGrid();
            radioButtonAddRule.Checked = true;
            bool isPreallocationRule = GetCommissionCalculationTime();
            if (isPreallocationRule)
            {
                radioButtonPreAllocation.Checked = false;
                radioButtonPostAllocation.Checked = true;
            }
            else
            {
                radioButtonPreAllocation.Checked = true;
                radioButtonPostAllocation.Checked = false;
            }
            //BindComboAndListBox();
            //BindDataGrid();

            CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();
            List<CVAUECAccountCommissionRule> tempCVAUECCommissionRules = new List<CVAUECAccountCommissionRule>();
            tempCVAUECCommissionRules = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();

            //CVAUECAccountCommissionRule[] a = new CVAUECAccountCommissionRule[1];
            oldCVAUECCommissionRules = new List<CVAUECAccountCommissionRule>(tempCVAUECCommissionRules);
        }

        private void BindComboAndListBox()
        {
            //Bind Account List box.
            Accounts companyAccounts = CompanyManager.GetAccount(_companyID);
            if (companyAccounts.Count > 0)
            {
                checkedListBoxAccounts.DataSource = companyAccounts;
                checkedListBoxAccounts.ValueMember = "CompanyAccountID";
                checkedListBoxAccounts.DisplayMember = "AccountName";
            }

            //BindSingleRules Combo
            List<AssetCategory> assetIdList = new List<AssetCategory>();
            assetIdList.Add(AssetCategory.Equity);
            assetIdList.Add(AssetCategory.EquityOption);

            CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();
            List<Prana.BusinessObjects.CommissionRule> allSingleCommissionRules = new List<Prana.BusinessObjects.CommissionRule>();
            List<Prana.BusinessObjects.CommissionRule> allBasketCommissionRules = new List<Prana.BusinessObjects.CommissionRule>();

            Prana.BusinessObjects.CommissionRule tempCommRuleSelect = new Prana.BusinessObjects.CommissionRule();
            tempCommRuleSelect.RuleID = Guid.Empty;
            tempCommRuleSelect.RuleName = "-Select-";

            //allSingleCommissionRules.Add(tempCommRuleSelect);
            //allBasketCommissionRules.Add(tempCommRuleSelect);

            CommissionDBManager.GetAllSavedCommissionRules();
            allSingleCommissionRules = commissionRuleCacheManager.GetAllSingleCommissionRules();
            allBasketCommissionRules = commissionRuleCacheManager.GetAllBasketCommissionRules();

            allSingleCommissionRules.Insert(0, tempCommRuleSelect);
            allBasketCommissionRules.Insert(0, tempCommRuleSelect);

            //allSingleCommissionRules = commissionRuleCacheManager.GetAllSingleCommissionRules();

            //Prana.BusinessObjects.CommissionRule tempCommRule1 = new Prana.BusinessObjects.CommissionRule();
            //tempCommRule1.RuleID = Guid.NewGuid();
            //tempCommRule1.RuleName = "SingleRule-1";
            //tempCommRule1.ApplyRuleForTrade = Prana.BusinessObjects.AppConstants.TradeType.SingleTrade;
            //tempCommRule1.AssetIdList = assetIdList;
            //allSingleCommissionRules.Add(tempCommRule1);

            //Prana.BusinessObjects.CommissionRule tempCommRule2 = new Prana.BusinessObjects.CommissionRule();
            //tempCommRule2.RuleID = Guid.NewGuid();
            //tempCommRule2.RuleName = "BothRule-1";
            //tempCommRule2.ApplyRuleForTrade = Prana.BusinessObjects.AppConstants.TradeType.Both;
            //tempCommRule2.AssetIdList = assetIdList;
            //allSingleCommissionRules.Add(tempCommRule2);

            //allSingleCommissionRules.Add(
            cmbSingleRule.DataSource = allSingleCommissionRules;
            cmbSingleRule.DisplayMember = "RuleName";
            cmbSingleRule.ValueMember = "RuleID";
            cmbSingleRule.Value = Guid.Empty;

            ColumnsCollection columnsSingleRule = cmbSingleRule.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsSingleRule)
            {
                if (column.Key != "RuleName")
                {
                    column.Hidden = true;
                }
            }
            //ultraDropDownCompanyAccounts.DisplayLayout.Bands[0].ColHeadersVisible = false;



            //BindBasketRules Combo

            //allBasketCommissionRules = commissionRuleCacheManager.GetAllBasketCommissionRules();
            //allBasketCommissionRules.Add(tempCommRuleSelect);
            //allBasketCommissionRules.Add(tempCommRule2);
            cmbBasketRule.DataSource = allBasketCommissionRules;
            cmbBasketRule.DisplayMember = "RuleName";
            cmbBasketRule.ValueMember = "RuleID";
            cmbBasketRule.Value = Guid.Empty;

            ColumnsCollection columnsBasketRule = cmbBasketRule.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsBasketRule)
            {
                if (column.Key != "RuleName")
                {
                    column.Hidden = true;
                }
            }

            //Bind CVAUEC List box.
            CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(_companyID);
            CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(_companyID);

            int loopCounterPartyID = int.MinValue;
            int passThroughCounterPartyOuter = int.MinValue;
            int passThroughCounterPartyInner = int.MinValue;
            System.Data.DataTable tempDataTable = new System.Data.DataTable();
            tempDataTable.Columns.Add("DisplayData");
            tempDataTable.Columns.Add("CounterPartyID");
            tempDataTable.Columns.Add("VenueID");
            tempDataTable.Columns.Add("CounterPartyVenueID");
            tempDataTable.Columns.Add("AUECID");
            foreach (CounterPartyVenue companyCounterPartyVenue in counterPartyVenues)
            {
                passThroughCounterPartyInner = int.MinValue;

                loopCounterPartyID = companyCounterPartyVenue.CounterPartyID;
                System.Data.DataRow row = tempDataTable.NewRow();
                if (!companyCounterPartyVenue.CounterPartyID.Equals(passThroughCounterPartyOuter))
                {
                    row["DisplayData"] = companyCounterPartyVenue.CounterPartyName;
                    row["CounterPartyID"] = companyCounterPartyVenue.CounterPartyID;
                    row["CounterPartyVenueID"] = companyCounterPartyVenue.CounterPartyVenueID;
                    row["VenueID"] = int.MinValue;
                    row["AUECID"] = int.MinValue;
                    tempDataTable.Rows.Add(row);
                    passThroughCounterPartyOuter = companyCounterPartyVenue.CounterPartyID;
                }
                foreach (CounterPartyVenue companyCounterPartyVenueLoop in counterPartyVenues)
                {
                    if (companyCounterPartyVenueLoop.CounterPartyID.Equals(loopCounterPartyID) && companyCounterPartyVenueLoop.CounterPartyID != passThroughCounterPartyInner)
                    {
                        if (CounterPartyManager.CheckExistingUserCounterPartyVenue(companyCounterPartyVenue.CounterPartyID, companyCounterPartyVenue.VenueID, _companyID) == true)
                        {
                            row = tempDataTable.NewRow();
                            row["DisplayData"] = "      " + companyCounterPartyVenue.DisplayName;
                            row["CounterPartyID"] = companyCounterPartyVenue.CounterPartyID;
                            row["VenueID"] = companyCounterPartyVenue.VenueID;
                            row["CounterPartyVenueID"] = companyCounterPartyVenue.CounterPartyVenueID;
                            row["AUECID"] = int.MinValue;
                            tempDataTable.Rows.Add(row);


                            //Adding CompanyCV AUEC's
                            ValueList companyCVAUECList = GetCVAndAUECValueList(companyCounterPartyVenue.CompanyCounterPartyCVID, _companyID);

                            foreach (ValueListItem valueListItem in companyCVAUECList.ValueListItems)
                            {
                                if (int.Parse(valueListItem.DataValue.ToString()) > 0)
                                {
                                    DataRow rowcompanyCVAUEC = tempDataTable.NewRow();

                                    rowcompanyCVAUEC["DisplayData"] = "            " + valueListItem.DisplayText.ToString();
                                    rowcompanyCVAUEC["CounterPartyID"] = companyCounterPartyVenue.CounterPartyID;
                                    rowcompanyCVAUEC["VenueID"] = companyCounterPartyVenue.VenueID;
                                    rowcompanyCVAUEC["CounterPartyVenueID"] = companyCounterPartyVenue.CounterPartyVenueID;
                                    rowcompanyCVAUEC["AUECID"] = int.Parse(valueListItem.DataValue.ToString());
                                    tempDataTable.Rows.Add(rowcompanyCVAUEC);

                                }
                            }
                            passThroughCounterPartyInner = companyCounterPartyVenueLoop.CounterPartyID;
                        }
                    }
                }
            }

            if (counterPartyVenues.Count > 0)
            {
                checkedlstCompanyCVAUEC.DataSource = tempDataTable;// CounterPartyVenues;
                checkedlstCompanyCVAUEC.DisplayMember = "DisplayData";//"CounterPartyVenueFullName";
                checkedlstCompanyCVAUEC.ValueMember = "CounterPartyID";//"CounterPartyVenueID";						
            }
            //checkedlstCompanyCVAUEC.
        }


        public void SaveCVAUECAccountCommissionRulesList()
        {
            int result = int.MinValue;
            if (cancelApplied.Equals(true))
            {
                List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRulesList = new List<CVAUECAccountCommissionRule>();
                cvAUECAccountCommissionRulesList = (List<CVAUECAccountCommissionRule>)grdCommissionRules.DataSource;

                int index = 1;
                CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();
                List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRuleListCache = new List<CVAUECAccountCommissionRule>();

                cvAUECAccountCommissionRuleListCache = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();

                cvAUECAccountCommissionRuleListCache = oldCVAUECCommissionRules;
                //commissionRuleCacheManager.
                bool validRule = true;
                //cvAUECAccountCommissionRuleListCache.Clear();
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {
                    int accountID = int.Parse(dr.Cells["AccountID"].Value.ToString());
                    int counterPartyVenueID = int.Parse(dr.Cells["CVID"].Value.ToString());
                    int auecID = int.Parse(dr.Cells["AUECID"].Value.ToString());
                    Prana.BusinessObjects.CommissionRule singleRule = (Prana.BusinessObjects.CommissionRule)dr.Cells["SingleRule"].Value;
                    Prana.BusinessObjects.CommissionRule basketRule = (Prana.BusinessObjects.CommissionRule)dr.Cells["BasketRule"].Value;
                    if (counterPartyVenueID != MIN_VALUE || auecID != MIN_VALUE || singleRule.RuleID != Guid.Empty || basketRule.RuleID != Guid.Empty)
                    {
                        if (radioButtonPostAllocation.Checked.Equals(true))
                        {
                            if (accountID == MIN_VALUE)
                            {
                                MessageBox.Show(this, "Please select the account in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                validRule = false;
                                break;
                            }
                        }
                        if (counterPartyVenueID == MIN_VALUE)
                        {
                            MessageBox.Show(this, "Please select the Broker Venue in the row: " + index, "PranaAlert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            validRule = false;
                            break;
                        }
                        if (auecID == MIN_VALUE)
                        {
                            MessageBox.Show(this, "Please select the AUEC in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            validRule = false;
                            break;
                        }
                        if (singleRule.RuleID == Guid.Empty)
                        {
                            MessageBox.Show(this, "Please select the Single Rule in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            validRule = false;
                            break;
                        }
                        if (basketRule.RuleID == Guid.Empty)
                        {
                            if (basketRule.ApplyRuleForTrade == TradeType.Both || basketRule.ApplyRuleForTrade == TradeType.BasketTrade)
                            {
                                MessageBox.Show(this, "Please select the Basket Rule in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                validRule = false;
                            }
                            break;
                        }
                    }
                    index += 1;
                    //cvAUECAccountCommissionRuleListCache.Add((CVAUECAccountCommissionRule)dr.ListObject);
                }
                if (validRule.Equals(true))
                {
                    CommissionDBManager.SaveCompanyCommissionRulesForCVAUEC(oldCVAUECCommissionRules, _companyID);
                    //commissionRuleCacheManager.SaveCommissionRulesForCVAUEC();
                    CommissionDBManager.GetAllCommissionRulesForCVAUEC(_companyID);
                    MessageBox.Show(this, "Rules Saved.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                cancelApplied = false;
                //return cvAUECAccountCommissionRulesList;
            }
            else
            {
                List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRulesList = new List<CVAUECAccountCommissionRule>();
                cvAUECAccountCommissionRulesList = (List<CVAUECAccountCommissionRule>)grdCommissionRules.DataSource;

                int index = 1;
                CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();
                List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRuleListCache = new List<CVAUECAccountCommissionRule>();

                cvAUECAccountCommissionRuleListCache = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();

                bool validRule = true;
                //cvAUECAccountCommissionRuleListCache.Clear();
                int innerCounter = 0;
                int nestedCounter = 0;
                bool foundDuplicateRow = false;
                UltraGridRow[] individualRows = grdCommissionRules.Rows.GetFilteredInNonGroupByRows();
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in individualRows)
                {
                    nestedCounter = 0;
                    int accountID = int.Parse(dr.Cells["AccountID"].Value.ToString());
                    int counterPartyVenueID = int.Parse(dr.Cells["CVID"].Value.ToString());
                    int auecID = int.Parse(dr.Cells["AUECID"].Value.ToString());
                    Prana.BusinessObjects.CommissionRule singleRule = (Prana.BusinessObjects.CommissionRule)dr.Cells["SingleRule"].Value;
                    Prana.BusinessObjects.CommissionRule basketRule = (Prana.BusinessObjects.CommissionRule)dr.Cells["BasketRule"].Value;
                    if (counterPartyVenueID != MIN_VALUE || auecID != MIN_VALUE || singleRule.RuleID != Guid.Empty || basketRule.RuleID != Guid.Empty)
                    {
                        if (radioButtonPostAllocation.Checked.Equals(true))
                        {
                            if (accountID == MIN_VALUE)
                            {
                                MessageBox.Show(this, "Please select the account in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                validRule = false;
                                break;
                            }
                        }
                        if (counterPartyVenueID == MIN_VALUE)
                        {
                            MessageBox.Show(this, "Please select the Broker Venue in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            validRule = false;
                            break;
                        }
                        if (auecID == MIN_VALUE)
                        {
                            MessageBox.Show(this, "Please select the AUEC in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            validRule = false;
                            break;
                        }
                        if (singleRule.RuleID == Guid.Empty)
                        {
                            MessageBox.Show(this, "Please select the Single Rule in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            validRule = false;
                            break;
                        }
                        if (basketRule.RuleID == Guid.Empty)
                        {
                            if (basketRule.ApplyRuleForTrade == TradeType.Both || basketRule.ApplyRuleForTrade == TradeType.BasketTrade)
                            {
                                MessageBox.Show(this, "Please select the Basket Rule in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                validRule = false;
                            }
                            break;
                        }
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow drCheck in individualRows)
                        {
                            int checkCounterPartyVenueID = int.Parse(drCheck.Cells["CVID"].Value.ToString());
                            int checkAUECID = int.Parse(drCheck.Cells["AUECID"].Value.ToString());
                            int checkAccountID = int.Parse(drCheck.Cells["AccountID"].Value.ToString());

                            if (radioButtonPostAllocation.Checked.Equals(true))
                            {
                                if (counterPartyVenueID == checkCounterPartyVenueID && auecID == checkAUECID && accountID == checkAccountID && innerCounter != nestedCounter)
                                {
                                    foundDuplicateRow = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (counterPartyVenueID == checkCounterPartyVenueID && auecID == checkAUECID && innerCounter != nestedCounter)
                                {
                                    foundDuplicateRow = true;
                                    break;
                                }
                            }
                            nestedCounter++;

                        }
                        innerCounter++;
                        if (foundDuplicateRow.Equals(true))
                        {
                            break;
                        }
                    }
                    index += 1;
                    //cvAUECAccountCommissionRuleListCache.Add((CVAUECAccountCommissionRule)dr.ListObject);
                }
                if (validRule.Equals(true))
                {
                    if (foundDuplicateRow.Equals(false))
                    {
                        //commissionRuleCacheManager.SaveCommissionRulesForCVAUEC();
                        result = CommissionDBManager.SaveCompanyCommissionRulesForCVAUEC(cvAUECAccountCommissionRulesList, _companyID);
                        if (result > 0)
                        {
                            bool commissionCalculationTime = false;
                            if (radioButtonPostAllocation.Checked.Equals(true))
                            {
                                commissionCalculationTime = true;
                            }
                            CommissionDBManager.SaveCommissionCalculationTime(commissionCalculationTime);
                        }

                        oldCVAUECCommissionRules = cvAUECAccountCommissionRulesList; //Added on 23rd Oct.
                        CommissionDBManager.GetAllCommissionRulesForCVAUEC(_companyID);
                        MessageBox.Show(this, "Rules Saved.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, "There are duplicate rows in the grid. Please remove them first before saving.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                //return cvAUECAccountCommissionRulesList;
            }
        }


        ThirdPartyAccountCommissionRules _companyThirdPartyAccountCommissionRules = new ThirdPartyAccountCommissionRules();
        public ThirdPartyAccountCommissionRules CompanyThirdPartyAccountCommissionRules
        {
            get
            {
                _companyThirdPartyAccountCommissionRules = (ThirdPartyAccountCommissionRules)grdCommissionRules.DataSource;
                ThirdPartyAccountCommissionRules validThirdPartyAccountCommissionRules = new ThirdPartyAccountCommissionRules();

                int index = 1;
                StringBuilder AccountIDs = new StringBuilder();
                ArrayList alAccountIDs = new ArrayList();

                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {
                    int companyAccountID = int.Parse(dr.Cells["CompanyAccountID"].Value.ToString());
                    AccountIDs.Append(companyAccountID);
                    alAccountIDs.Add(companyAccountID);
                }
                //                Accounts companyAccounts = (Accounts) ultraDropDownCompanyAccounts.DataSource;
                //                //companyAccounts.RemoveAt(0);
                //                StringBuilder comparerAccountID = new StringBuilder();
                //                ArrayList alComparerAccountID = new ArrayList();

                //                //Commented as there is no account id is replaced by company accountID.
                //                //foreach(Account account in companyAccounts) 
                //                //{
                //                //    if(int.Parse(account.AccountID.ToString()) != int.MinValue)
                //                //    {
                //                //        comparerAccountID.Append(account.AccountID);
                //                //        alComparerAccountID.Add(account.AccountID);
                //                //    }
                //                //}

                //                foreach (Account account in companyAccounts)
                //                {
                //                    if (int.Parse(account.CompanyAccountID.ToString()) != int.MinValue)
                //                    {
                //                        comparerAccountID.Append(account.CompanyAccountID);
                //                        alComparerAccountID.Add(account.CompanyAccountID);
                //                    }
                //                }

                //                bool found = false;
                //                int countArrayList = alAccountIDs.Count;
                ////				while(countArrayList > 0)
                ////				{
                //                    found = false;
                //                    for(int i = 0; i < alComparerAccountID.Count; i++)
                //                    {
                //                        if(alAccountIDs.Contains(alComparerAccountID[i]) == true)
                //                        {
                //                            found = true;
                //                            //break;
                //                        }
                //                        else
                //                        {
                //                            found = false;
                //                            break;
                //                        }
                //                    }
                //                    countArrayList -= 1;
                ////				}

                //                string sAccount = AccountIDs.ToString();
                //                string sComparerAccount  = comparerAccountID.ToString();
                //                int result = sAccount.IndexOf(sComparerAccount);
                //                //if(result < 0)
                //                if(found == false)
                //                {
                //                    MessageBox.Show("Please select the all the company accounts atleast for one time.");
                //                    validThirdPartyAccountCommissionRules = null;
                //                    return validThirdPartyAccountCommissionRules;
                //                }


                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {
                    int companyAccountID = int.Parse(dr.Cells["CompanyAccountID"].Value.ToString());
                    int companyCounterPartyCVID = int.Parse(dr.Cells["CompanyCounterPartyCVID"].Value.ToString());
                    int cvAUECID = int.Parse(dr.Cells["CVAUECID"].Value.ToString());
                    int singleRuleID = int.Parse(dr.Cells["SingleRuleID"].Value.ToString());
                    int basketRuleID = int.Parse(dr.Cells["BasketRuleID"].Value.ToString());
                    //if(companyAccountID != MIN_VALUE && companyCounterPartyCVID != MIN_VALUE && cvAUECID != MIN_VALUE && singleRuleID != MIN_VALUE && basketRuleID != MIN_VALUE)
                    //{
                    if (companyAccountID != MIN_VALUE && (companyCounterPartyCVID == MIN_VALUE || cvAUECID == MIN_VALUE || singleRuleID == MIN_VALUE || basketRuleID == MIN_VALUE))
                    //if (companyAccountID != MIN_VALUE )
                    {
                        if (companyCounterPartyCVID != MIN_VALUE || cvAUECID != MIN_VALUE || singleRuleID != MIN_VALUE || basketRuleID != MIN_VALUE)
                        {
                            //if(companyAccountID == MIN_VALUE)
                            //{
                            //    MessageBox.Show("Please select the account in the row: " + index);
                            //    validThirdPartyAccountCommissionRules = null;
                            //    return validThirdPartyAccountCommissionRules;
                            //}
                            if (companyCounterPartyCVID == MIN_VALUE)
                            {
                                MessageBox.Show(this, "Please select the Broker Venue in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                validThirdPartyAccountCommissionRules = null;
                                return validThirdPartyAccountCommissionRules;
                            }
                            if (cvAUECID == MIN_VALUE)
                            {
                                MessageBox.Show(this, "Please select the AUEC in the row: " + index, "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                validThirdPartyAccountCommissionRules = null;
                                return validThirdPartyAccountCommissionRules;
                            }
                            if (singleRuleID == MIN_VALUE)
                            {
                                MessageBox.Show("Please select the Single Rule in the row: " + index);
                                validThirdPartyAccountCommissionRules = null;
                                return validThirdPartyAccountCommissionRules;
                            }
                            if (basketRuleID == MIN_VALUE)
                            {
                                MessageBox.Show("Please select the Basket Rule in the row: " + index);
                                validThirdPartyAccountCommissionRules = null;
                                return validThirdPartyAccountCommissionRules;
                            }
                            //index += 1;
                        }
                    }
                    index += 1;
                    //}
                }

                foreach (ThirdPartyAccountCommissionRule thirdPartyAccountCommissionRule in _companyThirdPartyAccountCommissionRules)
                {
                    if (thirdPartyAccountCommissionRule.CompanyAccountID != MIN_VALUE && thirdPartyAccountCommissionRule.CompanyCounterPartyCVID != MIN_VALUE && thirdPartyAccountCommissionRule.CVAUECID != MIN_VALUE && thirdPartyAccountCommissionRule.SingleRuleID != MIN_VALUE && thirdPartyAccountCommissionRule.BasketRuleID != MIN_VALUE)
                    {
                        validThirdPartyAccountCommissionRules.Add(thirdPartyAccountCommissionRule);
                    }
                }

                return validThirdPartyAccountCommissionRules;
            }
            set
            {
                _companyThirdPartyAccountCommissionRules = value;
                if (_companyThirdPartyAccountCommissionRules.Count > 0)
                {
                    grdCommissionRules.DataSource = _companyThirdPartyAccountCommissionRules;
                    AddNewRow();
                    SetRuleDescription();
                }
                else
                {
                    AddNewTempRow();
                }
                RefreshGrid();
            }
        }

        private void SetRuleDescription()
        {
            string ruleDescription = string.Empty;
            ThirdPartyAccountCommissionRules companyThirdPartyAccountCommissionRules = (ThirdPartyAccountCommissionRules)grdCommissionRules.DataSource;

            foreach (ThirdPartyAccountCommissionRule tpfc in companyThirdPartyAccountCommissionRules)
            {

            }
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
            {
                int singleRuleID = int.Parse(dr.Cells["SingleRuleID"].Value.ToString());
                int basketRuleID = int.Parse(dr.Cells["BasketRuleID"].Value.ToString());

                ruleDescription = GetRuleDescription(singleRuleID);
                dr.Cells["SingleRuleDescription"].Value = ruleDescription.ToString();
                ruleDescription = GetRuleDescription(basketRuleID);
                dr.Cells["BasketRuleDescription"].Value = ruleDescription.ToString();
            }

        }

        private void grdCommissionRules_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CounterPartyId"].Hidden = true; ;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["VenueId"].Hidden = true;

            Accounts companyAccounts = CompanyManager.GetAccount(_companyID);
            companyAccounts.Insert(0, new Prana.Admin.BLL.Account(MIN_VALUE, C_COMBO_SELECT));
            ultraDropDownCompanyAccounts.DisplayMember = "AccountName";
            ultraDropDownCompanyAccounts.ValueMember = "CompanyAccountID";
            ultraDropDownCompanyAccounts.DataSource = null;
            ultraDropDownCompanyAccounts.DataSource = companyAccounts;


            ultraDropDownCompanyAccounts.Text = C_COMBO_SELECT;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].ValueList = ultraDropDownCompanyAccounts;

            ColumnsCollection columnsCF = ultraDropDownCompanyAccounts.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCF)
            {
                if (column.Key != "AccountName")
                {
                    column.Hidden = true;
                }
            }
            ultraDropDownCompanyAccounts.DisplayLayout.Bands[0].ColHeadersVisible = false;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].Header.Caption = "Account";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].Header.VisiblePosition = 5;


            CounterPartyVenues companyCounterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(_companyID);

            companyCounterPartyVenues.Insert(0, new CounterPartyVenue(int.MinValue, C_COMBO_SELECT));
            ultraDropDownCompanyCounterPartyVenues.DataSource = null;
            ultraDropDownCompanyCounterPartyVenues.DataSource = companyCounterPartyVenues;
            ultraDropDownCompanyCounterPartyVenues.ValueMember = "CounterPartyVenueID";//ultraDropDownCompanyCounterPartyVenues.ValueMember = "CompanyCounterPartyCVID";
            ultraDropDownCompanyCounterPartyVenues.DisplayMember = "DisplayName";
            ultraDropDownCompanyCounterPartyVenues.Text = C_COMBO_SELECT;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["CVID"].ValueList = ultraDropDownCompanyCounterPartyVenues;
            ColumnsCollection columnsCounterPartyVenues = ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCounterPartyVenues)
            {
                if (column.Key != "DisplayName")
                {
                    column.Hidden = true;
                }
            }
            ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].ColHeadersVisible = false;
            ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].Header.Caption = "Broker Venue";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CVID"].Header.VisiblePosition = 3;

            System.Data.DataTable dtCVAUEC = new System.Data.DataTable();
            dtCVAUEC.Columns.Add("Data");
            dtCVAUEC.Columns.Add("Value");
            object[] row = new object[2];
            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            dtCVAUEC.Rows.Add(row);

            if (cvAUECsStringNames.Count > 0)
            {
                foreach (KeyValuePair<int, List<CVAuecs>> lstCVAuecs in cvAUECsStringNames)
                {
                    foreach (CVAuecs item in lstCVAuecs.Value)
                    {
                        row[0] = item.AuecStringName;
                        row[1] = item.AuecID;
                        dtCVAUEC.Rows.Add(row);
                    }
                }
                ultraDropDownCVAUEC.DataSource = null;
                ultraDropDownCVAUEC.DataSource = dtCVAUEC;
                ultraDropDownCVAUEC.DisplayMember = "Data";
                ultraDropDownCVAUEC.ValueMember = "Value";
                ultraDropDownCompanyCounterPartyVenues.Text = C_COMBO_SELECT;
                ultraDropDownCVAUEC.Width = 240;
            }
            if (radioButtonPreAllocation.Checked.Equals(true))
            {
                grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].CellActivation = Activation.NoEdit;
            }
            else
            {
                grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].CellActivation = Activation.AllowEdit;
            }


            grdCommissionRules.DisplayLayout.Bands[0].Columns["AUECID"].ValueList = ultraDropDownCVAUEC;
            ColumnsCollection columnsCVAUECs = ultraDropDownCVAUEC.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCVAUECs)
            {
                if (column.Key != "Data")
                {
                    column.Hidden = true;
                }
            }
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AUECID"].Width = 240;
            ultraDropDownCVAUEC.DisplayLayout.Bands[0].ColHeadersVisible = false;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AUECID"].Header.Caption = "AUEC";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AUECID"].Header.VisiblePosition = 4;



            CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();
            List<Prana.BusinessObjects.CommissionRule> allSingleCommissionRules = new List<Prana.BusinessObjects.CommissionRule>();
            List<Prana.BusinessObjects.CommissionRule> allBasketCommissionRules = new List<Prana.BusinessObjects.CommissionRule>();

            Prana.BusinessObjects.CommissionRule tempCommRuleSelect = new Prana.BusinessObjects.CommissionRule();
            tempCommRuleSelect.RuleID = Guid.Empty;
            tempCommRuleSelect.RuleName = "-Select-";

            CommissionDBManager.GetAllSavedCommissionRules();
            allSingleCommissionRules = commissionRuleCacheManager.GetAllSingleCommissionRules();
            allBasketCommissionRules = commissionRuleCacheManager.GetAllBasketCommissionRules();

            allSingleCommissionRules.Insert(0, tempCommRuleSelect);
            allBasketCommissionRules.Insert(0, tempCommRuleSelect);
            ultraDropDownSingleRule.DataSource = null;
            ultraDropDownSingleRule.DataSource = allSingleCommissionRules;
            ultraDropDownSingleRule.ValueMember = "RuleID";
            ultraDropDownSingleRule.DisplayMember = "RuleName";
            ultraDropDownSingleRule.Text = C_COMBO_SELECT;
            ultraDropDownSingleRule.Width = 140;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["SingleRule"].ValueList = ultraDropDownSingleRule;
            ColumnsCollection columnsCommissionRules = ultraDropDownSingleRule.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCommissionRules)
            {
                if (column.Key != "RuleName")
                {
                    column.Hidden = true;
                }
            }
            grdCommissionRules.DisplayLayout.Bands[0].Columns["SingleRule"].Width = 140;
            ultraDropDownSingleRule.DisplayLayout.Bands[0].ColHeadersVisible = false;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["SingleRule"].Header.Caption = "Single Rule";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["SingleRule"].Header.VisiblePosition = 1;
            ultraDropDownBasketRule.DataSource = null;
            ultraDropDownBasketRule.DataSource = allBasketCommissionRules;
            ultraDropDownBasketRule.ValueMember = "RuleID";
            ultraDropDownBasketRule.DisplayMember = "RuleName";
            ultraDropDownBasketRule.Text = C_COMBO_SELECT;
            ultraDropDownBasketRule.Width = 140;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["BasketRule"].ValueList = ultraDropDownBasketRule;
            ColumnsCollection columnsBasketCommissionRules = ultraDropDownBasketRule.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsBasketCommissionRules)
            {
                if (column.Key != "RuleName")
                {
                    column.Hidden = true;
                }
            }
            grdCommissionRules.DisplayLayout.Bands[0].Columns["BasketRule"].Width = 140;
            ultraDropDownBasketRule.DisplayLayout.Bands[0].ColHeadersVisible = false;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["BasketRule"].Header.Caption = "Basket Rule";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["BasketRule"].Header.VisiblePosition = 2;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["CVName"].Hidden = true;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AUECName"].Hidden = true;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CVAUECRuleID"].Hidden = true;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountName"].Hidden = true;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;

            //ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].Columns["DisplayName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            //ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].Columns["CompanyCounterPartyCVID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

            //Added By sandeep as on 22-Oct-2007
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Header.VisiblePosition = 8;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Width = 75;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Hidden = false;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

        }

        public void BindDataGrid()
        {
            try
            {
                CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();

                CommissionDBManager.GetAllCommissionRulesForCVAUEC(_companyID);
                List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRulesList = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();


                //ThirdPartyAccountCommissionRules thirdPartyAccountCommissionRules = CompanyManager.GetCompanyThirdPartyAccountCommissionRules(_companyID);
                //ThirdPartyAccountCommissionRule nullThirdPartyAccountCommissionRule = new ThirdPartyAccountCommissionRule();
                //if(thirdPartyAccountCommissionRules.Count <= 0)
                //{
                //    thirdPartyAccountCommissionRules.Add(nullThirdPartyAccountCommissionRule);
                //}
                grdCommissionRules.DataSource = cvAUECAccountCommissionRulesList;
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

        private bool AccountCVAUECMapping()
        {
            bool result = false;
            int accountID = int.Parse(grdCommissionRules.ActiveRow.Cells["AccountID"].Value.ToString());
            int cvID = int.Parse(grdCommissionRules.ActiveRow.Cells["CVID"].Value.ToString());
            int cvAUECID = int.Parse(grdCommissionRules.ActiveRow.Cells["AUECID"].Value.ToString());
            Guid singleRuleID = (Guid)((Prana.BusinessObjects.CommissionRule)(grdCommissionRules.ActiveRow.Cells["SingleRule"].Value)).RuleID;
            int currentIndex = grdCommissionRules.ActiveRow.Index;
            int checkIndex = int.MinValue;
            if (radioButtonPostAllocation.Checked.Equals(true))
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {
                    int dAccountID = int.Parse(dr.Cells["AccountID"].Value.ToString());
                    int dcvID = int.Parse(dr.Cells["CVID"].Value.ToString());
                    int dcvAUECID = int.Parse(dr.Cells["AUECID"].Value.ToString());
                    Guid dSingleRuleID = (Guid)((Prana.BusinessObjects.CommissionRule)(grdCommissionRules.ActiveRow.Cells["SingleRule"].Value)).RuleID;
                    checkIndex = dr.Index;
                    if (accountID == dAccountID && cvID == dcvID && cvAUECID == dcvAUECID && singleRuleID == dSingleRuleID && checkIndex != currentIndex)
                    {
                        result = true;
                        MessageBox.Show(this, "This Account, Broker Venue & CVAUEC combination already exists for the single rule selected, you cannot choose this combination again.", "Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {
                    int dcvID = int.Parse(dr.Cells["CVID"].Value.ToString());
                    int dcvAUECID = int.Parse(dr.Cells["AUECID"].Value.ToString());
                    Guid dSingleRuleID = (Guid)((Prana.BusinessObjects.CommissionRule)(grdCommissionRules.ActiveRow.Cells["SingleRule"].Value)).RuleID;
                    checkIndex = dr.Index;
                    if (cvID == dcvID && cvAUECID == dcvAUECID && singleRuleID == dSingleRuleID && checkIndex != currentIndex)
                    {
                        result = true;
                        MessageBox.Show(this, "This Broker Venue & CVAUEC combination already exists for the single rule selected, you cannot choose this combination again.", "Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            return result;
        }

        public int updatedCVID = int.MinValue;
        public int updatedCompanyAccountID = int.MinValue;
        public int updatedCVAUECID = int.MinValue;
        public Guid updatedSingleRuleID = Guid.Empty;
        public Guid updatedBasketRuleID = Guid.Empty;
        CommissionRulesCacheManager _commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();
        private void grdCommissionRules_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {

                Prana.BusinessObjects.CommissionRule singleComRule = new Prana.BusinessObjects.CommissionRule();
                Prana.BusinessObjects.CommissionRule basketComRule = new Prana.BusinessObjects.CommissionRule();
                if (grdCommissionRules.ActiveRow.Cells["SingleRule"].IsActiveCell == true)
                {
                    singleComRule = (Prana.BusinessObjects.CommissionRule)ultraDropDownSingleRule.SelectedRow.ListObject;
                    grdCommissionRules.ActiveRow.Cells["SingleRule"].Value = singleComRule;
                }
                //else
                //{
                //    singleComRule = (Prana.BusinessObjects.CommissionRule)grdCommissionRules.ActiveRow.Cells["SingleRule"].Value;
                //}
                if (grdCommissionRules.ActiveRow.Cells["BasketRule"].IsActiveCell == true)
                {
                    basketComRule = (Prana.BusinessObjects.CommissionRule)ultraDropDownBasketRule.SelectedRow.ListObject;
                    grdCommissionRules.ActiveRow.Cells["BasketRule"].Value = basketComRule;
                }

                string ruleDescription = string.Empty;
                int oldCompanyAccountID = int.Parse(grdCommissionRules.ActiveRow.Cells["AccountID"].Value.ToString());
                int oldCVID = int.Parse(grdCommissionRules.ActiveRow.Cells["CVID"].Value.ToString());
                int oldCVAUECID = int.Parse(grdCommissionRules.ActiveRow.Cells["AUECID"].Value.ToString());
                Guid oldSingleRuleID = (Guid)((Prana.BusinessObjects.CommissionRule)(grdCommissionRules.ActiveRow.Cells["SingleRule"].Value)).RuleID;
                Guid oldBasketRuleID = (Guid)((Prana.BusinessObjects.CommissionRule)(grdCommissionRules.ActiveRow.Cells["BasketRule"].Value)).RuleID;

                Prana.BusinessObjects.CommissionRule singleCommissionRule = _commissionRuleCacheManager.GetCommissionRuleByRuleId(oldSingleRuleID);
                //SelectBasketRuleForSingleRule(singleCommissionRule);
                grdCommissionRules.UpdateData();

                if (grdCommissionRules.ActiveRow.Cells["SingleRule"].IsActiveCell == true)
                {
                    SelectBasketRuleForSingleRule(singleCommissionRule);
                }

                //SetCompanyCVAUECForSelectedSingleRule(singleComRule.AssetIdList, oldCVID);

                updatedCVID = int.Parse(grdCommissionRules.ActiveRow.Cells["CVID"].Value.ToString());

                //if (grdCommissionRules.ActiveRow.Cells["SingleRule"].IsActiveCell == true || grdCommissionRules.ActiveRow.Cells["CVID"].IsActiveCell == true)
                //{
                //    SetCompanyCVAUECForSelectedSingleRule(singleComRule.AssetIdList, updatedCVID);
                //}

                if (grdCommissionRules.ActiveRow.Cells["SingleRule"].IsActiveCell == true || grdCommissionRules.ActiveRow.Cells["CVID"].IsActiveCell == true)
                {
                    if (singleCommissionRule != null)
                    {
                        SetCompanyCVAUECForSelectedSingleRule(singleCommissionRule.AssetIdList, updatedCVID);
                    }
                }

                bool result = false;
                updatedCVID = int.Parse(grdCommissionRules.ActiveRow.Cells["CVID"].Value.ToString());
                if (oldCVID != updatedCVID)
                {
                    result = AccountCVAUECMapping();
                    if (result == true)
                    {
                        grdCommissionRules.ActiveRow.Cells["CVID"].Value = oldCVID;
                        grdCommissionRules.UpdateData();
                    }
                }



                result = false;
                updatedCVAUECID = int.Parse(grdCommissionRules.ActiveRow.Cells["AUECID"].Value.ToString());
                if (oldCVAUECID != updatedCVAUECID)
                {
                    result = AccountCVAUECMapping();
                    if (result == true)
                    {
                        grdCommissionRules.ActiveRow.Cells["AUECID"].Value = oldCVAUECID;
                        grdCommissionRules.UpdateData();
                    }
                }

                result = false;
                updatedSingleRuleID = (Guid)((Prana.BusinessObjects.CommissionRule)(grdCommissionRules.ActiveRow.Cells["SingleRule"].Value)).RuleID;
                if (oldSingleRuleID != updatedSingleRuleID)
                {
                    result = AccountCVAUECMapping();
                    if (result == true)
                    {
                        grdCommissionRules.ActiveRow.Cells["SingleRule"].Value = oldSingleRuleID;
                        grdCommissionRules.UpdateData();
                    }
                }

                if (radioButtonPostAllocation.Checked.Equals(true))
                {
                    result = false;
                    updatedCompanyAccountID = int.Parse(grdCommissionRules.ActiveRow.Cells["AccountID"].Value.ToString());
                    if (oldCompanyAccountID != updatedCompanyAccountID)
                    {
                        result = AccountCVAUECMapping();
                        if (result == true)
                        {
                            grdCommissionRules.ActiveRow.Cells["AccountID"].Value = oldCompanyAccountID;
                            grdCommissionRules.UpdateData();
                        }
                    }
                }


                //To Make use of:
                //if(oldCVID != updatedCVID)
                //{
                //    GetCompanyCVAUEC(updatedCVID);
                //    //AUECs companyCVAUECs = CompanyManager.GetCompanyCVAUECs(_companyID);			
                //}

                //updatedCVAUECID = int.Parse(grdCommissionRules.ActiveRow.Cells["AUECID"].Value.ToString());
                //if(oldCVAUECID != updatedCVAUECID)
                //{
                //    GetAUECCommissionRules(updatedCVAUECID);
                //}

                //int singleRuleID = int.Parse(grdCommissionRules.ActiveRow.Cells["SingleRule"].Value.ToString());
                //if(oldSingleRuleID != singleRuleID)
                //{
                //    ruleDescription = GetRuleDescription(singleRuleID);
                //    grdCommissionRules.ActiveRow.Cells["SingleRuleDescription"].Value = ruleDescription.ToString();
                //}

                //int basketRuleID = int.Parse(grdCommissionRules.ActiveRow.Cells["BasketRule"].Value.ToString());
                //if(oldBasketRuleID != basketRuleID)
                //{
                //    ruleDescription = GetRuleDescription(basketRuleID);
                //    grdCommissionRules.ActiveRow.Cells["BasketRuleDescription"].Value = ruleDescription.ToString();
                //}

                //AddNewRow(); -- Commented as per the new changes for Oct End release 07.

                //this.ultraDropDownCVAUEC.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                //this.grdCommissionRules.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                this.ultraDropDownCVAUEC.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

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

        private void GetCompanyCVAUEC(int cvID)
        {
            grdCommissionRules.ActiveRow.Cells["CVAUECID"].Value = int.MinValue;

            ValueList vlCVAUEC = GetCVAndCVAUECValueList(updatedCVID, _companyID);
            grdCommissionRules.ActiveRow.Cells["CVAUECID"].ValueList = vlCVAUEC;

        }

        private void GetAUECCommissionRules(int _cvAUECID)
        {
            grdCommissionRules.ActiveRow.Cells["SingleRuleID"].Value = int.MinValue;
            grdCommissionRules.ActiveRow.Cells["BasketRuleID"].Value = int.MinValue;

            ValueList vlCVAUECSingleCommissionRules = GetCVAUECSingleCommissoinRules(_cvAUECID);
            grdCommissionRules.ActiveRow.Cells["SingleRuleID"].ValueList = vlCVAUECSingleCommissionRules;


            ValueList vlCVAUECBasketCommissionRules = GetCVAUECBasketCommissoinRules(_cvAUECID);
            grdCommissionRules.ActiveRow.Cells["BasketRuleID"].ValueList = vlCVAUECBasketCommissionRules;

        }

        //private string GetRuleDescription(int ruleID)
        //{
        //    StringBuilder description = new StringBuilder();
        //    AllCommissionRules commissionRuleDescriptions = CommissionRuleManager.GetCommissionRuleDescription(ruleID);
        //    foreach(AllCommissionRule commissionRuleDescription in commissionRuleDescriptions)
        //    {
        //        if(int.Parse(commissionRuleDescription.ApplyCriteria.ToString()) == 1)
        //        {
        //            string rateCents = commissionRuleDescription.CommisionRate1.ToString();
        //            string commissionRateType = commissionRuleDescription.CommissionRateType.ToString();
        //            string operatorName = commissionRuleDescription.OperatorName.ToString();
        //            string valueK = commissionRuleDescription.Value1.ToString();

        //            description.Append(rateCents);
        //            description.Append("/");
        //            description.Append(commissionRateType);
        //            description.Append(" if ");
        //            description.Append(commissionRateType);
        //            description.Append(operatorName);
        //            description.Append(valueK);
        //            description.Append("K");
        //            description.Append(", ");
        //        }
        //        else
        //        {
        //            string CaluculationType = commissionRuleDescription.CaluculationType.ToString();
        //            string CurrencySymbol = commissionRuleDescription.CurrencySymbol.ToString();
        //            string Commission = commissionRuleDescription.Commission.ToString();

        //            description.Append(CurrencySymbol);
        //            description.Append(" ");
        //            description.Append(Commission);
        //            description.Append("/");
        //            description.Append(CaluculationType);
        //            break;
        //        }

        //    }
        //    return description.ToString();
        //}

        private string GetRuleDescription(int ruleID)
        {
            StringBuilder description = new StringBuilder();
            AllCommissionRules commissionRuleDescriptions = CommissionRuleManager.GetCommissionRuleDescriptionUpdated(ruleID);
            foreach (AllCommissionRule commissionRuleDescription in commissionRuleDescriptions)
            {
                if (int.Parse(commissionRuleDescription.ApplyCriteria.ToString()) == 1)
                {
                    string rateCents = commissionRuleDescription.CommisionRate1.ToString();
                    string commissionRateType = commissionRuleDescription.CommissionRateType.ToString();
                    //string operatorName = commissionRuleDescription.OperatorName.ToString();
                    string valuefrom = commissionRuleDescription.ValueFrom.ToString();
                    string valueto = commissionRuleDescription.ValueTo.ToString();

                    description.Append(rateCents);
                    description.Append("/");
                    description.Append(commissionRateType);
                    description.Append(" if ");
                    description.Append(commissionRateType);
                    description.Append(" >= ");
                    description.Append(valuefrom);
                    description.Append("K");
                    description.Append(" And ");
                    description.Append(" <= ");
                    description.Append(valueto);
                    description.Append("K");
                    description.Append(", ");
                }
                else
                {
                    string CaluculationType = commissionRuleDescription.CaluculationType.ToString();
                    string CurrencySymbol = commissionRuleDescription.CurrencySymbol.ToString();
                    string Commission = commissionRuleDescription.Commission.ToString();

                    description.Append(CurrencySymbol);
                    description.Append(" ");
                    description.Append(Commission);
                    description.Append("/");
                    description.Append(CaluculationType);
                    break;
                }

            }
            return description.ToString();
        }

        private void AddNewRow()
        {
            ColumnsCollection columnsCounterPartyVenues = ultraDropDownCompanyCounterPartyVenues.DisplayLayout.Bands[0].Columns;

            int rowsCount = grdCommissionRules.Rows.Count;
            UltraGridRow dr = grdCommissionRules.Rows[rowsCount - 1];

            ThirdPartyAccountCommissionRules tpfcrs = (ThirdPartyAccountCommissionRules)grdCommissionRules.DataSource;

            ThirdPartyAccountCommissionRule tpfcr = new ThirdPartyAccountCommissionRule();

            //The below varriables are taken from the last row of the grid befor adding the new row.
            int companyAccountID = int.Parse(dr.Cells["CompanyAccountID"].Value.ToString());
            int companyCounterPartyCVID = int.Parse(dr.Cells["CompanyCounterPartyCVID"].Value.ToString());
            int cvAUECID = int.Parse(dr.Cells["CVAUECID"].Value.ToString());
            int singleRuleID = int.Parse(dr.Cells["SingleRuleID"].Value.ToString());
            int basketRuleID = int.Parse(dr.Cells["BasketRuleID"].Value.ToString());

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (companyAccountID != MIN_VALUE || companyCounterPartyCVID != MIN_VALUE || cvAUECID != MIN_VALUE || singleRuleID != MIN_VALUE || basketRuleID != MIN_VALUE)
            {
                tpfcr.CompanyCounterPartyCVID = int.MinValue;
                tpfcr.CVAUECID = int.MinValue;
                tpfcr.SingleRuleID = int.MinValue;
                tpfcr.BasketRuleID = int.MinValue;
                tpfcrs.Add(tpfcr);
                grdCommissionRules.DataSource = tpfcrs;
                grdCommissionRules.DataBind();
            }
        }

        /// <summary>
        /// Method created to add the empty row in the case where no rows for it are stored in the DB.
        /// </summary>
        private void AddNewTempRow()
        {
            ThirdPartyAccountCommissionRules tpfcrs = new ThirdPartyAccountCommissionRules();

            ThirdPartyAccountCommissionRule tpfcr = new ThirdPartyAccountCommissionRule();
            tpfcr.CompanyCounterPartyCVID = int.MinValue;
            tpfcr.CVAUECID = int.MinValue;
            tpfcr.SingleRuleID = int.MinValue;
            tpfcr.BasketRuleID = int.MinValue;
            tpfcrs.Add(tpfcr);
            grdCommissionRules.DataSource = tpfcrs;
            //grdCommissionRules.DataBind();

        }

        private void grdCommissionRules_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
        }

        private ValueList GetCVAndCVAUECValueList(int rcdUpdatedCVID, int rcdCompanyID)
        {
            ValueList cvAUECValueList = new ValueList();
            List<CVAuecs> cvAUECs = new List<CVAuecs>();
            if (cvAUECs.Count > 0 && cvAUECsStringNames.ContainsKey(rcdUpdatedCVID))
            {
                cvAUECs = cvAUECsStringNames[rcdUpdatedCVID];
                foreach (CVAuecs auec in cvAUECs)
                {
                    cvAUECValueList.ValueListItems.Add(auec.AuecID, auec.AuecStringName);
                }
            }
            return cvAUECValueList;
        }


        private ValueList GetCVAndAUECValueList(int rcdUpdatedCVID, int rcdCompanyID)
        {
            ValueList cvAUECValueList = new ValueList();

            List<CVAuecs> dtCVAUEC = new List<CVAuecs>();
            if (cvAUECsStringNames.ContainsKey(rcdUpdatedCVID))
            {
                dtCVAUEC = cvAUECsStringNames[rcdUpdatedCVID];
                if (dtCVAUEC.Count > 0)
                {
                    foreach (CVAuecs objAUEC in dtCVAUEC)
                    {
                        cvAUECValueList.ValueListItems.Add(objAUEC.AuecID, objAUEC.AuecStringName);
                    }
                }
            }
            return cvAUECValueList;

        }


        private ValueList GetCVAUECSingleCommissoinRules(int cvAUECID)
        {
            ValueList cvAUECSingleCommissionValueList = new ValueList();

            AUECCommissionRules auecCommissionRules = CommissionRuleManager.GetCVAUECCommissionRules(cvAUECID);
            AUECCommissionRules singleAUECCommissionRules = new AUECCommissionRules();
            foreach (AUECCommissionRule auecCommissionRule in auecCommissionRules)
            {
                if (auecCommissionRule.ApplyRuleID == SINGLE_RULEID)
                {
                    singleAUECCommissionRules.Add(auecCommissionRule);
                }
            }
            singleAUECCommissionRules.Insert(0, new AUECCommissionRule(int.MinValue, C_COMBO_SELECT));
            foreach (AUECCommissionRule auecCommissionRule in singleAUECCommissionRules)
            {
                cvAUECSingleCommissionValueList.ValueListItems.Add(int.Parse(auecCommissionRule.RuleID.ToString()), auecCommissionRule.RuleName.ToString());
            }

            return cvAUECSingleCommissionValueList;
        }

        private ValueList GetCVAUECBasketCommissoinRules(int cvAUECID)
        {
            ValueList cvAUECBasketCommissionValueList = new ValueList();

            AUECCommissionRules auecCommissionRules = CommissionRuleManager.GetCVAUECCommissionRules(cvAUECID);
            AUECCommissionRules basketAUECCommissionRules = new AUECCommissionRules();
            foreach (AUECCommissionRule auecCommissionRule in auecCommissionRules)
            {
                if (auecCommissionRule.ApplyRuleID == BASKET_RULEID)
                {
                    basketAUECCommissionRules.Add(auecCommissionRule);
                }
            }
            basketAUECCommissionRules.Insert(0, new AUECCommissionRule(int.MinValue, C_COMBO_SELECT));
            foreach (AUECCommissionRule auecCommissionRule in basketAUECCommissionRules)
            {
                cvAUECBasketCommissionValueList.ValueListItems.Add(int.Parse(auecCommissionRule.RuleID.ToString()), auecCommissionRule.RuleName.ToString());
            }

            return cvAUECBasketCommissionValueList;
        }

        private void RefreshGrid()
        {
            if (grdCommissionRules.Rows.Count > 0)
            {
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CompanyAccountID"].Header.VisiblePosition = 0;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CompanyCounterPartyCVID"].Header.VisiblePosition = 1;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CVAUECID"].Header.VisiblePosition = 2;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["SingleRuleID"].Header.VisiblePosition = 3;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["BasketRuleID"].Header.VisiblePosition = 5;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountName"].Header.VisiblePosition = 7;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["SingleRuleDescription"].Header.VisiblePosition = 4;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["BasketRuleDescription"].Header.VisiblePosition = 6;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountName"].Hidden = true;

                //Added By sandeep as on 22-Oct-2007
                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Header.VisiblePosition = 8;

            }
        }

        public int DeleteCommissionRule()
        {
            int result = 0;
            try
            {
                //Check whether any commission rule is there to delete
                if (grdCommissionRules.Rows.Count > 0)
                {
                    string strAccountName = grdCommissionRules.ActiveRow.Cells["AccountName"].Text.ToString();
                    string strCounterPartyVenue = grdCommissionRules.ActiveRow.Cells["CompanyCounterPartyCVID"].Text.ToString();
                    string strAUECName = grdCommissionRules.ActiveRow.Cells["CVAUECID"].Text.ToString();
                    // get all the Ids 
                    int intAccountId = int.Parse(grdCommissionRules.ActiveRow.Cells["CompanyAccountID"].Value.ToString());
                    int intCounterPartyVenue = int.Parse(grdCommissionRules.ActiveRow.Cells["CompanyCounterPartyCVID"].Value.ToString());
                    int intAUECId = int.Parse(grdCommissionRules.ActiveRow.Cells["CVAUECID"].Value.ToString());
                    if (strAccountName != "- Select -" && strCounterPartyVenue != "- Select -" && strAUECName != "- Select -")
                    {
                        //Asking the user to be sure about deleting the Commission Rule .
                        if (MessageBox.Show(this, "Do you want to delete this Commission Rule ?", "Prana Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            result = CommissionRuleManager.DeleteSelectedCommissionRule(intAccountId, intCounterPartyVenue, intAUECId);
                            if (result == 1)
                            {
                                MessageBox.Show(this, "Commission Rule deleted.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (result == -1)
                            {
                                MessageBox.Show(this, "Commission Rule can not be deleted, it is referenced in the Third Party Flat Files.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Please select the Commission Rule to delete.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        result = 0;
                    }
                }
                else
                {
                    //Showing the message: No Data Available.
                    MessageBox.Show(this, "Please select the Commission Rule to delete.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    result = 0;
                    return result;
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
            return result;
        }

        private void btnUpdateCommissionRules_Click(object sender, EventArgs e)
        {
            if (cmbSingleRule.Value != null)
            {
                if (!cmbSingleRule.Value.Equals(Guid.Empty))
                {
                    List<CVAUECAccountCommissionRule> existingCVAUECAccountCommissionRule = new List<CVAUECAccountCommissionRule>();
                    List<CVAUECAccountCommissionRule> totalBoxCVAUECAccountCommissionRuleList = new List<CVAUECAccountCommissionRule>();
                    List<CVAUECAccountCommissionRule> grossCVAUECAccountCommissionRuleList = new List<CVAUECAccountCommissionRule>();

                    CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();
                    existingCVAUECAccountCommissionRule = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();
                    bool foundExistingRule = false;
                    bool checkForUpdate = false;

                    if (radioButtonPostAllocation.Checked.Equals(true))
                    {
                        if (checkedListBoxAccounts.CheckedItems.Count > 0)
                        {
                            if (checkedlstCompanyCVAUEC.CheckedItems.Count > 0)
                            {
                                int len = checkedlstCompanyCVAUEC.CheckedItems.Count;
                                bool foundAUECID = false;
                                for (int i = 0; i < len; i++)
                                {
                                    System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstCompanyCVAUEC.CheckedItems[i]).Row;
                                    int auecID = int.Parse(selectedRow["AUECID"].ToString());
                                    if (auecID > 0)
                                    {
                                        foundAUECID = true;
                                        break;
                                    }
                                }
                                if (foundAUECID.Equals(true))
                                {
                                    //CommissionRuleCacheManager commissionRuleCacheManager = CommissionRuleCacheManager.GetInstance();
                                    int lenAUECs = checkedlstCompanyCVAUEC.CheckedItems.Count;
                                    int auecID = int.MinValue;
                                    int cvID = int.MinValue;
                                    for (int i = 0; i < lenAUECs; i++)
                                    {
                                        CVAUECAccountCommissionRule cvAUECAccountCommissionRule = new CVAUECAccountCommissionRule();
                                        System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstCompanyCVAUEC.CheckedItems[i]).Row;
                                        auecID = int.Parse(selectedRow["AUECID"].ToString());
                                        if (auecID > 0)
                                        {
                                            cvID = cvAUECAccountCommissionRule.CVID = int.Parse(selectedRow["CounterPartyVenueID"].ToString());
                                            if (auecID > 0)
                                            {
                                                cvAUECAccountCommissionRule.AUECID = auecID;
                                                cvAUECAccountCommissionRule.CVID = cvID; //int.Parse(selectedRow["CounterPartyVenueID"].ToString());
                                            }

                                            int lenAccounts = checkedListBoxAccounts.CheckedItems.Count;
                                            int accountID = int.MinValue;
                                            for (int iAccount = 0; iAccount < lenAccounts; iAccount++)
                                            {
                                                cvAUECAccountCommissionRule = new CVAUECAccountCommissionRule();
                                                cvAUECAccountCommissionRule.AUECID = auecID;
                                                cvAUECAccountCommissionRule.CVID = cvID;
                                                accountID = ((Prana.Admin.BLL.Account)checkedListBoxAccounts.CheckedItems[iAccount]).CompanyAccountID;
                                                cvAUECAccountCommissionRule.AccountID = accountID;

                                                cvAUECAccountCommissionRule.SingleRule = (Prana.BusinessObjects.CommissionRule)cmbSingleRule.SelectedRow.ListObject;
                                                cvAUECAccountCommissionRule.BasketRule = (Prana.BusinessObjects.CommissionRule)cmbBasketRule.SelectedRow.ListObject;
                                                cvAUECAccountCommissionRule.CompanyID = _companyID;

                                                foundExistingRule = false;
                                                foreach (CVAUECAccountCommissionRule lookupCVAUECAccountCommissionRule in existingCVAUECAccountCommissionRule)
                                                {
                                                    //if (lookupCVAUECAccountCommissionRule.AUECID == cvAUECAccountCommissionRule.AUECID && lookupCVAUECAccountCommissionRule.CVID == cvAUECAccountCommissionRule.CVID && lookupCVAUECAccountCommissionRule.SingleRule.RuleID == cvAUECAccountCommissionRule.SingleRule.RuleID && lookupCVAUECAccountCommissionRule.BasketRule.RuleID == cvAUECAccountCommissionRule.BasketRule.RuleID && lookupCVAUECAccountCommissionRule.AccountID == cvAUECAccountCommissionRule.AccountID)
                                                    if (lookupCVAUECAccountCommissionRule.AUECID == cvAUECAccountCommissionRule.AUECID && lookupCVAUECAccountCommissionRule.CVID == cvAUECAccountCommissionRule.CVID && lookupCVAUECAccountCommissionRule.AccountID == cvAUECAccountCommissionRule.AccountID)
                                                    {
                                                        checkForUpdate = true;
                                                        foundExistingRule = true;
                                                        //totalBoxCVAUECAccountCommissionRuleList.Add(lookupCVAUECAccountCommissionRule);

                                                    }
                                                    else
                                                    {
                                                        //totalBoxCVAUECAccountCommissionRuleList.Add(lookupCVAUECAccountCommissionRule);
                                                    }
                                                }

                                                if (foundExistingRule == false)
                                                {
                                                    totalBoxCVAUECAccountCommissionRuleList.Add(cvAUECAccountCommissionRule);
                                                    grossCVAUECAccountCommissionRuleList.Add(cvAUECAccountCommissionRule);
                                                }
                                                else
                                                {
                                                    grossCVAUECAccountCommissionRuleList.Add(cvAUECAccountCommissionRule);
                                                }

                                                //commissionRuleCacheManager.AddCVAUECRule(cvAUECAccountCommissionRule);
                                                //int counterPartyVenueID = int.Parse(selectedRow["CounterPartyVenueID"].ToString());
                                            }

                                        }
                                    }
                                    foreach (CVAUECAccountCommissionRule lookupCVAUECAccountCommissionRule in totalBoxCVAUECAccountCommissionRuleList)
                                    {
                                        existingCVAUECAccountCommissionRule.Add(lookupCVAUECAccountCommissionRule);
                                    }


                                    if (checkForUpdate.Equals(true))
                                    {
                                        if (DialogResult.Yes.Equals(MessageBox.Show(this, "There are some rules which have been assigned to the same CV-AUEC combination. Do You want to override them ?", "Prana Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Error)))
                                        {
                                            foreach (CVAUECAccountCommissionRule grossCVAUECAccountCommissionRule in grossCVAUECAccountCommissionRuleList)
                                            {
                                                foreach (CVAUECAccountCommissionRule existCVAUECAccountCommissionRule in existingCVAUECAccountCommissionRule)
                                                {
                                                    if (grossCVAUECAccountCommissionRule.AUECID == existCVAUECAccountCommissionRule.AUECID && grossCVAUECAccountCommissionRule.CVID == existCVAUECAccountCommissionRule.CVID && grossCVAUECAccountCommissionRule.AccountID == existCVAUECAccountCommissionRule.AccountID)
                                                    {
                                                        existCVAUECAccountCommissionRule.SingleRule = grossCVAUECAccountCommissionRule.SingleRule;
                                                        existCVAUECAccountCommissionRule.BasketRule = grossCVAUECAccountCommissionRule.BasketRule;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //grossCVAUECAccountCommissionRuleList = existingCVAUECAccountCommissionRule;
                                        }
                                    }


                                    //commissionRuleCacheManager.SaveCommissionRulesForCVAUEC();
                                    //radioButtonModifyRule.Checked = true;
                                    //grpModifyCommissionRules.Enabled = true;
                                    //grpBoxAddRule.Enabled = false;
                                    //grdCommissionRules.DataSource = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();
                                    List<CVAUECAccountCommissionRule> tempRules = new List<CVAUECAccountCommissionRule>();
                                    grdCommissionRules.DataSource = tempRules;

                                    grdCommissionRules.DataSource = existingCVAUECAccountCommissionRule;

                                }
                                else
                                {
                                    MessageBox.Show(this, "Please select some AUEC's under a Broker Venue.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                            }
                            else
                            {
                                MessageBox.Show(this, "Please select some AUEC's.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please select some accounts.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (checkedlstCompanyCVAUEC.CheckedItems.Count > 0)
                        {
                            int len = checkedlstCompanyCVAUEC.CheckedItems.Count;
                            bool foundAUECID = false;
                            for (int i = 0; i < len; i++)
                            {
                                System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstCompanyCVAUEC.CheckedItems[i]).Row;
                                int auecID = int.Parse(selectedRow["AUECID"].ToString());
                                if (auecID > 0)
                                {
                                    foundAUECID = true;
                                    break;
                                }
                            }



                            if (foundAUECID.Equals(true))
                            {


                                int lenAUECs = checkedlstCompanyCVAUEC.CheckedItems.Count;
                                int auecID = int.MinValue;


                                for (int i = 0; i < lenAUECs; i++)
                                {
                                    CVAUECAccountCommissionRule cvAUECAccountCommissionRule = new CVAUECAccountCommissionRule();
                                    System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstCompanyCVAUEC.CheckedItems[i]).Row;
                                    auecID = int.Parse(selectedRow["AUECID"].ToString());
                                    if (auecID < 0)
                                    {
                                        continue;
                                    }
                                    if (auecID > 0)
                                    {
                                        cvAUECAccountCommissionRule.AUECID = auecID;
                                        cvAUECAccountCommissionRule.CVID = int.Parse(selectedRow["CounterPartyVenueID"].ToString());
                                    }


                                    cvAUECAccountCommissionRule.SingleRule = (Prana.BusinessObjects.CommissionRule)cmbSingleRule.SelectedRow.ListObject;
                                    cvAUECAccountCommissionRule.BasketRule = (Prana.BusinessObjects.CommissionRule)cmbBasketRule.SelectedRow.ListObject;
                                    cvAUECAccountCommissionRule.CompanyID = _companyID;


                                    foundExistingRule = false;
                                    foreach (CVAUECAccountCommissionRule lookupCVAUECAccountCommissionRule in existingCVAUECAccountCommissionRule)
                                    {
                                        //if (lookupCVAUECAccountCommissionRule.AUECID == cvAUECAccountCommissionRule.AUECID && lookupCVAUECAccountCommissionRule.CVID == cvAUECAccountCommissionRule.CVID && lookupCVAUECAccountCommissionRule.SingleRule.RuleID == cvAUECAccountCommissionRule.SingleRule.RuleID && lookupCVAUECAccountCommissionRule.BasketRule.RuleID == cvAUECAccountCommissionRule.BasketRule.RuleID)
                                        if (lookupCVAUECAccountCommissionRule.AUECID == cvAUECAccountCommissionRule.AUECID && lookupCVAUECAccountCommissionRule.CVID == cvAUECAccountCommissionRule.CVID)
                                        {
                                            checkForUpdate = true;
                                            foundExistingRule = true;
                                            //totalBoxCVAUECAccountCommissionRuleList.Add(lookupCVAUECAccountCommissionRule);

                                        }
                                        else
                                        {
                                            //totalBoxCVAUECAccountCommissionRuleList.Add(lookupCVAUECAccountCommissionRule);
                                        }
                                    }

                                    if (foundExistingRule == false)
                                    {
                                        totalBoxCVAUECAccountCommissionRuleList.Add(cvAUECAccountCommissionRule);
                                        grossCVAUECAccountCommissionRuleList.Add(cvAUECAccountCommissionRule);
                                    }
                                    else
                                    {
                                        grossCVAUECAccountCommissionRuleList.Add(cvAUECAccountCommissionRule);
                                    }
                                }
                                foreach (CVAUECAccountCommissionRule lookupCVAUECAccountCommissionRule in totalBoxCVAUECAccountCommissionRuleList)
                                {
                                    existingCVAUECAccountCommissionRule.Add(lookupCVAUECAccountCommissionRule);
                                }

                                if (checkForUpdate.Equals(true))
                                {
                                    if (DialogResult.Yes.Equals(MessageBox.Show(this, "There are some rules which have been assigned to the same CV-AUEC combination. Do You want to override them ?", "Prana Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Error)))
                                    {
                                        foreach (CVAUECAccountCommissionRule grossCVAUECAccountCommissionRule in grossCVAUECAccountCommissionRuleList)
                                        {
                                            foreach (CVAUECAccountCommissionRule existCVAUECAccountCommissionRule in existingCVAUECAccountCommissionRule)
                                            {
                                                if (grossCVAUECAccountCommissionRule.AUECID == existCVAUECAccountCommissionRule.AUECID && grossCVAUECAccountCommissionRule.CVID == existCVAUECAccountCommissionRule.CVID)
                                                {
                                                    existCVAUECAccountCommissionRule.SingleRule = grossCVAUECAccountCommissionRule.SingleRule;
                                                    existCVAUECAccountCommissionRule.BasketRule = grossCVAUECAccountCommissionRule.BasketRule;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //grossCVAUECAccountCommissionRuleList = existingCVAUECAccountCommissionRule;
                                    }
                                }

                                //commissionRuleCacheManager.SaveCommissionRulesForCVAUEC();
                                //radioButtonModifyRule.Checked = true;
                                //grpModifyCommissionRules.Enabled = true;
                                //grpBoxAddRule.Enabled = false;
                                //grdCommissionRules.DataSource = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();
                                List<CVAUECAccountCommissionRule> tempRules = new List<CVAUECAccountCommissionRule>();
                                grdCommissionRules.DataSource = tempRules;


                                grdCommissionRules.DataSource = existingCVAUECAccountCommissionRule;
                            }

                        }
                        else
                        {
                            MessageBox.Show(this, "Please select some AUEC's.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select some single commission rule.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(this, "Please select some single commission rule.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void radioButtonAddRule_CheckedChanged(object sender, EventArgs e)
        {
            grpBoxAddRule.Enabled = true;
            if (radioButtonModifyRule.Checked.Equals(true))
            {
                btnUpdateCommissionRules.Enabled = false;
            }
            else
            {
                btnUpdateCommissionRules.Enabled = true;
            }
            grpModifyCommissionRules.Enabled = false;
        }

        private void radioButtonModifyRule_CheckedChanged(object sender, EventArgs e)
        {
            grpModifyCommissionRules.Enabled = true;
            grpBoxAddRule.Enabled = false;
            if (radioButtonModifyRule.Checked.Equals(true))
            {
                btnUpdateCommissionRules.Enabled = false;
            }
            else
            {
                btnUpdateCommissionRules.Enabled = true;
            }
        }

        private void radioButtonPreAllocation_CheckedChanged(object sender, EventArgs e)
        {
            if (_companyID != int.MinValue)
            {
                checkedListBoxAccounts.Enabled = false;
                HideAccountColumn();
                if (radioButtonPreAllocation.Checked.Equals(true))
                {
                    if (grdCommissionRules.DisplayLayout.Bands[0].Columns.Count > 1)
                    {
                        grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].CellActivation = Activation.NoEdit;
                    }
                    UnSelectAllAccounts();
                }
                else
                {
                    if (grdCommissionRules.DisplayLayout.Bands[0].Columns.Count > 1)
                    {
                        grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].CellActivation = Activation.AllowEdit;
                    }
                }
            }
        }

        private void radioButtonPostAllocation_CheckedChanged(object sender, EventArgs e)
        {
            if (_companyID != int.MinValue)
            {
                checkedListBoxAccounts.Enabled = true;
                ShowAccountColumn();
                if (radioButtonPreAllocation.Checked.Equals(true))
                {
                    if (grdCommissionRules.DisplayLayout.Bands[0].Columns.Count > 1)
                    {
                        grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].CellActivation = Activation.NoEdit;
                    }
                }
                else
                {
                    if (grdCommissionRules.DisplayLayout.Bands[0].Columns.Count > 1)
                    {
                        grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].CellActivation = Activation.AllowEdit;
                    }
                    UnSelectAllAccounts();
                }
            }
        }

        private void HideAccountColumn()
        {
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].Hidden = true;
        }

        private void ShowAccountColumn()
        {
            grdCommissionRules.DisplayLayout.Bands[0].Columns["AccountID"].Hidden = false;
        }

        private void cmbSingleRule_ValueChanged(object sender, EventArgs e)
        {
            if (cmbSingleRule.Value != null)
            {
                Prana.BusinessObjects.CommissionRule singleCommissionRule = (Prana.BusinessObjects.CommissionRule)cmbSingleRule.SelectedRow.ListObject;

                if (singleCommissionRule.ApplyRuleForTrade.Equals(TradeType.Both))
                {
                    cmbBasketRule.Value = singleCommissionRule.RuleID;
                    cmbBasketRule.Enabled = false;
                }
                else
                {
                    cmbBasketRule.Enabled = true;
                    cmbBasketRule.Value = Guid.Empty;
                }
                SetCompanyCVAUECForSelectedRules(singleCommissionRule.AssetIdList);
                if (singleCommissionRule.RuleID == Guid.Empty)
                {
                    checkedlstCompanyCVAUEC.Enabled = false;
                }
                else
                {
                    checkedlstCompanyCVAUEC.Enabled = true;
                }

                int itemsCount = checkedlstCompanyCVAUEC.Items.Count;
                for (int i = 0; i < itemsCount; i++)
                {
                    checkedlstCompanyCVAUEC.SetItemChecked(i, false);
                }
            }
        }

        private void cmbBasketRule_ValueChanged(object sender, EventArgs e)
        {
            int itemsCount = checkedlstCompanyCVAUEC.Items.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                checkedlstCompanyCVAUEC.SetItemChecked(i, false);
            }
        }

        StringBuilder _sbAUECsNotPermited = new StringBuilder();
        private void SetCompanyCVAUECForSelectedRules(List<AssetCategory> assetList)
        {
            _sbAUECsNotPermited = new StringBuilder();
            StringBuilder sbAssetList = new StringBuilder();
            List<int> lstCVAUECs = new List<int>();
            if (assetList != null)
            {
                foreach (AssetCategory assetCategory in assetList)
                {
                    sbAssetList.Append("'");
                    switch (assetCategory)
                    {
                        case AssetCategory.Equity:
                            sbAssetList.Append(ASSET_EQUITY);
                            break;
                        case AssetCategory.EquityOption:
                            sbAssetList.Append(ASSET_EQUITYOPTION);
                            break;
                        case AssetCategory.Future:
                            sbAssetList.Append(ASSET_FUTURE);
                            break;
                        case AssetCategory.FutureOption:
                            sbAssetList.Append(ASSET_FUTUREOPTION);
                            break;
                        case AssetCategory.FX:
                            sbAssetList.Append(ASSET_FOREIGNEXCHANGE);
                            break;
                        case AssetCategory.FixedIncome:
                            sbAssetList.Append(ASSET_FIXEDINCOME);
                            break;
                        case AssetCategory.Cash:
                            sbAssetList.Append(ASSET_CASH);
                            break;
                        case AssetCategory.Indices:
                            sbAssetList.Append(ASSET_INDICES);
                            break;
                    }
                    //sbAssetList.Append((AssetCategory)assetCategory);
                    //sbAssetList.Append(1);    
                    sbAssetList.Append("',");
                }
                int len = sbAssetList.Length;
                if (sbAssetList.Length > 0)
                {
                    sbAssetList.Remove((len - 1), 1);
                }
                lstCVAUECs = CounterPartyManager.GetAllCVAUECsForAssetList(sbAssetList.ToString());
            }


            CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(_companyID);
            CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(_companyID);

            int loopCounterPartyID = int.MinValue;
            int passThroughCounterPartyOuter = int.MinValue;
            int passThroughCounterPartyInner = int.MinValue;
            System.Data.DataTable tempDataTable = new System.Data.DataTable();
            tempDataTable.Columns.Add("DisplayData");
            tempDataTable.Columns.Add("CounterPartyID");
            tempDataTable.Columns.Add("VenueID");
            tempDataTable.Columns.Add("CounterPartyVenueID");
            tempDataTable.Columns.Add("AUECID");
            foreach (CounterPartyVenue companyCounterPartyVenue in counterPartyVenues)
            {
                //foreach (CounterParty counterParty in counterParties)
                //{
                passThroughCounterPartyInner = int.MinValue;

                loopCounterPartyID = companyCounterPartyVenue.CounterPartyID;
                System.Data.DataRow row = tempDataTable.NewRow();
                if (!companyCounterPartyVenue.CounterPartyID.Equals(passThroughCounterPartyOuter))
                {
                    //System.Data.DataRow row = tempDataTable.NewRow();
                    row["DisplayData"] = companyCounterPartyVenue.CounterPartyName;
                    row["CounterPartyID"] = companyCounterPartyVenue.CounterPartyID;
                    row["VenueID"] = int.MinValue;
                    row["CounterPartyVenueID"] = companyCounterPartyVenue.CounterPartyVenueID;
                    row["AUECID"] = int.MinValue;
                    tempDataTable.Rows.Add(row);
                    passThroughCounterPartyOuter = companyCounterPartyVenue.CounterPartyID;
                }
                foreach (CounterPartyVenue companyCounterPartyVenueLoop in counterPartyVenues)
                {
                    //foreach (Venue venue in counterParty.Venues)
                    //{
                    if (companyCounterPartyVenueLoop.CounterPartyID.Equals(loopCounterPartyID) && companyCounterPartyVenueLoop.CounterPartyID != passThroughCounterPartyInner)
                    {
                        if (CounterPartyManager.CheckExistingUserCounterPartyVenue(companyCounterPartyVenue.CounterPartyID, companyCounterPartyVenue.VenueID, _companyID) == true)
                        {
                            row = tempDataTable.NewRow();
                            row["DisplayData"] = "      " + companyCounterPartyVenue.DisplayName;
                            row["CounterPartyID"] = companyCounterPartyVenue.CounterPartyID;
                            row["VenueID"] = companyCounterPartyVenue.VenueID;
                            row["CounterPartyVenueID"] = companyCounterPartyVenue.CounterPartyVenueID;
                            row["AUECID"] = int.MinValue;
                            tempDataTable.Rows.Add(row);


                            //Adding CompanyCV AUEC's
                            ValueList companyCVAUECList = GetCVAndAUECValueList(companyCounterPartyVenue.CompanyCounterPartyCVID, _companyID);
                            //System.Data.DataTable dtCompanyCVAUEC = new System.Data.DataTable();
                            //dtCompanyCVAUEC.Columns.Add("Data");
                            //dtCompanyCVAUEC.Columns.Add("Value");
                            foreach (ValueListItem valueListItem in companyCVAUECList.ValueListItems)
                            {
                                if (int.Parse(valueListItem.DataValue.ToString()) > 0)
                                {
                                    DataRow rowcompanyCVAUEC = tempDataTable.NewRow();
                                    rowcompanyCVAUEC["DisplayData"] = "            " + valueListItem.DisplayText.ToString();
                                    rowcompanyCVAUEC["CounterPartyID"] = companyCounterPartyVenue.CounterPartyID;
                                    rowcompanyCVAUEC["VenueID"] = companyCounterPartyVenue.VenueID;
                                    rowcompanyCVAUEC["CounterPartyVenueID"] = companyCounterPartyVenue.CounterPartyVenueID;
                                    rowcompanyCVAUEC["AUECID"] = int.Parse(valueListItem.DataValue.ToString());

                                    if (!lstCVAUECs.Contains(int.Parse(valueListItem.DataValue.ToString())))
                                    {
                                        rowcompanyCVAUEC["DisplayData"] = "            " + valueListItem.DisplayText.ToString() + "     (N/A)";
                                        _sbAUECsNotPermited.Append(valueListItem.DataValue + ",");
                                    }
                                    tempDataTable.Rows.Add(rowcompanyCVAUEC);
                                }
                            }
                            passThroughCounterPartyInner = companyCounterPartyVenueLoop.CounterPartyID;
                        }
                    }
                }
            }

            if (counterPartyVenues.Count > 0)
            {
                checkedlstCompanyCVAUEC.DataSource = tempDataTable;// CounterPartyVenues;
                checkedlstCompanyCVAUEC.DisplayMember = "DisplayData";//"CounterPartyVenueFullName";
                checkedlstCompanyCVAUEC.ValueMember = "CounterPartyID";//"CounterPartyVenueID";						
            }
        }

        private void checkedlstCompanyCVAUEC_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            StringBuilder sbAUECsNotPermitedNew = new StringBuilder(_sbAUECsNotPermited.ToString());

            int len = sbAUECsNotPermitedNew.Length;
            if (sbAUECsNotPermitedNew.Length > 0)
            {
                sbAUECsNotPermitedNew.Remove((len - 1), 1);
            }
            string sbAUECsNotPermited = sbAUECsNotPermitedNew.ToString();
            string[] sbAUECsNotPermitedArray = sbAUECsNotPermited.Split(',');

            if (len > 0)
            {
                int lenArray = sbAUECsNotPermitedArray.Length;
                for (int i = 0; i < (lenArray); i++)
                {
                    if (int.Parse(sbAUECsNotPermitedArray[i].ToString()) == int.Parse(((System.Data.DataRowView)(((System.Windows.Forms.ListBox)(sender)).SelectedItem)).Row.ItemArray[4].ToString()))
                    {
                        e.NewValue = e.CurrentValue;
                        //MessageBox.Show("This AUEC is not permitted for the rules selected.", "Prana Alert", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void ultraDropDownSingleRule_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Combo Cell change occured");
        }


        private void SetCompanyCVAUECForSelectedSingleRule(List<AssetCategory> assetList, int cvID)
        {
            //_sbAUECsNotPermited = new StringBuilder();
            StringBuilder sbAssetList = new StringBuilder();
            Prana.Admin.BLL.AUECs cvAUECs = new Prana.Admin.BLL.AUECs();
            if (assetList != null)
            {
                foreach (AssetCategory assetCategory in assetList)
                {
                    sbAssetList.Append("'");
                    switch (assetCategory)
                    {
                        case AssetCategory.Equity:
                            sbAssetList.Append(ASSET_EQUITY);
                            break;
                        case AssetCategory.EquityOption:
                            sbAssetList.Append(ASSET_EQUITYOPTION);
                            break;
                        case AssetCategory.Future:
                            sbAssetList.Append(ASSET_FUTURE);
                            break;
                        case AssetCategory.FutureOption:
                            sbAssetList.Append(ASSET_FUTUREOPTION);
                            break;
                        case AssetCategory.FX:
                            sbAssetList.Append(ASSET_FOREIGNEXCHANGE);
                            break;
                        case AssetCategory.FixedIncome:
                            sbAssetList.Append(ASSET_FIXEDINCOME);
                            break;
                        case AssetCategory.Cash:
                            sbAssetList.Append(ASSET_CASH);
                            break;
                        case AssetCategory.Indices:
                            sbAssetList.Append(ASSET_INDICES);
                            break;
                    }
                    //sbAssetList.Append((AssetCategory)assetCategory);
                    //sbAssetList.Append(1);    
                    sbAssetList.Append("',");
                }
                int len = sbAssetList.Length;
                if (sbAssetList.Length > 0)
                {
                    sbAssetList.Remove((len - 1), 1);
                }
                cvAUECs = CounterPartyManager.GetAllCVAUECsForCVAndAssetList(sbAssetList.ToString(), cvID);

                ValueList cvCommissionRuleAUECValueList = new ValueList();
                string data = string.Empty;
                foreach (Prana.Admin.BLL.AUEC auec in cvAUECs)
                {
                    data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + auec.Currency.CurrencySymbol.ToString();
                    cvCommissionRuleAUECValueList.ValueListItems.Add(auec.AUECID, data);
                }

                ValueListItem valueListItem = new ValueListItem();
                valueListItem.DataValue = int.MinValue;
                valueListItem.DisplayText = "-Select-";
                cvCommissionRuleAUECValueList.ValueListItems.Insert(0, valueListItem);
                grdCommissionRules.ActiveRow.Cells["AUECID"].ValueList = cvCommissionRuleAUECValueList;
                grdCommissionRules.ActiveRow.Cells["AUECID"].Value = int.MinValue;
            }
        }

        private void SelectBasketRuleForSingleRule(Prana.BusinessObjects.CommissionRule singleRule)
        {
            if (singleRule != null)
            {
                if (singleRule.ApplyRuleForTrade == TradeType.Both)
                {
                    grdCommissionRules.ActiveRow.Cells["BasketRule"].Value = singleRule;
                    grdCommissionRules.ActiveRow.Cells["BasketRule"].Activation = Activation.Disabled;
                }
                else
                {
                    grdCommissionRules.ActiveRow.Cells["BasketRule"].Activation = Activation.AllowEdit;
                    Prana.BusinessObjects.CommissionRule tempCommRuleSelect = new Prana.BusinessObjects.CommissionRule();
                    tempCommRuleSelect.RuleID = Guid.Empty;
                    tempCommRuleSelect.RuleName = "-Select-";
                    grdCommissionRules.ActiveRow.Cells["BasketRule"].Value = tempCommRuleSelect;
                }
            }
        }

        bool cancelApplied = false;
        public void CancelCVAUECCommissionRules()
        {
            cancelApplied = true;
            //grdCommissionRules.DataSource = oldCVAUECCommissionRules;

            CommissionRulesCacheManager commissionRuleCacheManager = CommissionRulesCacheManager.GetInstance();

            CommissionDBManager.GetAllCommissionRulesForCVAUEC(_companyID);
            List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRulesList = commissionRuleCacheManager.GetAllCVAUECAccountCommissionRules();
            grdCommissionRules.DataSource = cvAUECAccountCommissionRulesList;
            grdCommissionRules.DataBind();
        }

        // added by Sandeep as on 22-Oct-2007
        private void grdCommissionRules_ClickCellButton(object sender, CellEventArgs e)
        {
            grdCommissionRules.UpdateData();
            //if (grdCommissionRules.Rows.Count == 1)
            //{             
            //    return;
            //}
            if (e.Cell.Column.Key.Equals("DeleteButton"))
            {
                if (grdCommissionRules.ActiveRow.Cells["AccountID"].Text.Equals("- Select -") && grdCommissionRules.ActiveRow.Cells["CVID"].Text.Equals("- Select -") && grdCommissionRules.ActiveRow.Cells["AUECID"].Text.Equals("- Select -") && grdCommissionRules.ActiveRow.Cells["SingleRule"].Text.Equals("-Select-") && grdCommissionRules.ActiveRow.Cells["BasketRule"].Text.Equals("-Select-") || grdCommissionRules.ActiveRow.Cells["SingleRule"].Text.Equals(""))
                {

                }
                else
                {
                    grdCommissionRules.ActiveRow.Delete();
                }

            }
        }

        private void chkBoxSelectAllAccounts_CheckedChanged(object sender, EventArgs e)
        {
            int accountsCount = checkedListBoxAccounts.Items.Count;
            if (chkBoxSelectAllAccounts.Checked.Equals(true))
            {
                if (checkedListBoxAccounts.Enabled.Equals(true))
                {
                    for (int i = 0; i < accountsCount; i++)
                    {
                        checkedListBoxAccounts.SetItemChecked(i, true);
                    }
                }
            }
            else
            {
                if (checkedListBoxAccounts.Enabled.Equals(true))
                {
                    for (int i = 0; i < accountsCount; i++)
                    {
                        checkedListBoxAccounts.SetItemChecked(i, false);
                    }
                }
            }
        }

        //private void SelectAllAccounts()
        //{
        //    int accountsCount = checkedListBoxAccounts.Items.Count;
        //    for (int i = 0; i < accountsCount; i++)
        //    {
        //        checkedListBoxAccounts.SetItemChecked(i, true);
        //    }
        //}

        private void UnSelectAllAccounts()
        {
            int accountsCount = checkedListBoxAccounts.Items.Count;
            for (int i = 0; i < accountsCount; i++)
            {
                checkedListBoxAccounts.SetItemChecked(i, false);
            }
        }

        private string OpenSaveDialogBox()
        {
            // File open dialog , ask user to select the File
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = initialDirectory;
            saveFileDialog.Filter = "Excel files (.xlsx)|*.xlsx"; //"Excel|*.xls|Excel 2010|*.xlsx";
            saveFileDialog.FileName = "Commission Rules";
            string strFilePath = string.Empty;
            saveFileDialog.RestoreDirectory = true;
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFilePath = saveFileDialog.FileName;
            }
            else if (result == DialogResult.Cancel)
            {
                strFilePath = string.Empty;
            }
            return strFilePath;
        }

        private void btnExportCommissionRules_Click(object sender, EventArgs e)
        {
            try
            {
                string fileNameWithPath = OpenSaveDialogBox();
                if (!string.IsNullOrEmpty(fileNameWithPath))
                {
                    this.ultraGridExcelExporter1.Export(grdCommissionRules, fileNameWithPath);
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

        public static bool GetCommissionCalculationTime()
        {
            bool result = false;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCommissionCalculationTime";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    if (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int offSet = 0;
                        if (row[offSet] != System.DBNull.Value)
                        {
                            result = bool.Parse(row[offSet].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }
    }
}
