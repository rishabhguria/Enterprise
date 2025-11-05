using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateCompanyClientTradingAccount1.
    /// </summary>
    public class CreateCompanyClientTradingAccount : System.Windows.Forms.Form
    {
        private Traders _traders;
        private string FORM_NAME = "CreateCompanyClientTradingAccount";
        private string strTraderPreviousStName = string.Empty;
        #region Properties
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblExchangeLogo;
        private System.Windows.Forms.Label lblFlag;
        private Infragistics.Win.Misc.UltraLabel ultraLabel39;
        private IContainer components;
        private int _companyID;
        private int _companyClientID;
        private CompanyClientTradingAccount _companyClientTradingAccount;
        private bool _isAddition = true;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCompanyTradingAccount;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTraderShortName;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtClientTradingAccount;
        const string C_COMBO_SELECT = "- Select -";

        #endregion
        #region Constructor
        public CreateCompanyClientTradingAccount()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public CreateCompanyClientTradingAccount(int companyID, int companyClientID)
        {
            try
            {
                InitializeComponent();
                //	_traders=traders;
                _companyClientID = companyClientID;
                _companyID = companyID;
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

        public CreateCompanyClientTradingAccount(CompanyClientTradingAccount companyClientTradingAccount, int companyID)
        {
            InitializeComponent();
            _companyClientTradingAccount = companyClientTradingAccount;
            _companyClientID = _companyClientTradingAccount.CompanyClientID;
            _companyID = companyID;
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
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnAdd != null)
                {
                    btnAdd.Dispose();
                }
                if (lblExchangeLogo != null)
                {
                    lblExchangeLogo.Dispose();
                }
                if (lblFlag != null)
                {
                    lblFlag.Dispose();
                }
                if (ultraLabel39 != null)
                {
                    ultraLabel39.Dispose();
                }
                if (cmbCompanyTradingAccount != null)
                {
                    cmbCompanyTradingAccount.Dispose();
                }
                if (cmbTraderShortName != null)
                {
                    cmbTraderShortName.Dispose();
                }
                if (errorProvider != null)
                {
                    errorProvider.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtClientTradingAccount != null)
                {
                    txtClientTradingAccount.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCompanyClientTradingAccount));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyTradingAccountsID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountsID", 4);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TraderID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FirstName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Title", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EMail", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneWork", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneCell", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Pager", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneHome", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientID", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 12);
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblExchangeLogo = new System.Windows.Forms.Label();
            this.lblFlag = new System.Windows.Forms.Label();
            this.ultraLabel39 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCompanyTradingAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbTraderShortName = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClientTradingAccount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTradingAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTraderShortName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(164, 80);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.Location = new System.Drawing.Point(86, 80);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblExchangeLogo
            // 
            this.lblExchangeLogo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblExchangeLogo.Location = new System.Drawing.Point(4, 56);
            this.lblExchangeLogo.Name = "lblExchangeLogo";
            this.lblExchangeLogo.Size = new System.Drawing.Size(128, 14);
            this.lblExchangeLogo.TabIndex = 120;
            this.lblExchangeLogo.Text = "Client Trader Short Name";
            // 
            // lblFlag
            // 
            this.lblFlag.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFlag.Location = new System.Drawing.Point(4, 34);
            this.lblFlag.Name = "lblFlag";
            this.lblFlag.Size = new System.Drawing.Size(132, 14);
            this.lblFlag.TabIndex = 119;
            this.lblFlag.Text = "CompanyTradingAccount";
            // 
            // ultraLabel39
            // 
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel39.Appearance = appearance1;
            this.ultraLabel39.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel39.Location = new System.Drawing.Point(4, 5);
            this.ultraLabel39.Name = "ultraLabel39";
            this.ultraLabel39.Size = new System.Drawing.Size(122, 21);
            this.ultraLabel39.TabIndex = 118;
            this.ultraLabel39.Text = "Client Trading Account";
            // 
            // cmbCompanyTradingAccount
            // 
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCompanyTradingAccount.DisplayLayout.Appearance = appearance2;
            this.cmbCompanyTradingAccount.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.cmbCompanyTradingAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbCompanyTradingAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCompanyTradingAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.cmbCompanyTradingAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCompanyTradingAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.CellAppearance = appearance9;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlignAsString = "Left";
            this.cmbCompanyTradingAccount.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.RowAppearance = appearance12;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCompanyTradingAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.cmbCompanyTradingAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCompanyTradingAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCompanyTradingAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCompanyTradingAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCompanyTradingAccount.DropDownWidth = 0;
            this.cmbCompanyTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCompanyTradingAccount.Location = new System.Drawing.Point(164, 29);
            this.cmbCompanyTradingAccount.Name = "cmbCompanyTradingAccount";
            this.cmbCompanyTradingAccount.Size = new System.Drawing.Size(162, 21);
            this.cmbCompanyTradingAccount.TabIndex = 2;
            this.cmbCompanyTradingAccount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCompanyTradingAccount.GotFocus += new System.EventHandler(this.cmbCompanyTradingAccount_GotFocus);
            this.cmbCompanyTradingAccount.LostFocus += new System.EventHandler(this.cmbCompanyTradingAccount_LostFocus);
            // 
            // cmbTraderShortName
            // 
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTraderShortName.DisplayLayout.Appearance = appearance14;
            this.cmbTraderShortName.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn6.Header.VisiblePosition = 0;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn8.Header.VisiblePosition = 2;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.Header.VisiblePosition = 3;
            ultraGridColumn10.Header.VisiblePosition = 4;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.Header.VisiblePosition = 5;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.Header.VisiblePosition = 6;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 7;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn14.Header.VisiblePosition = 8;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn15.Header.VisiblePosition = 9;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn16.Header.VisiblePosition = 10;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn17.Header.VisiblePosition = 11;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn18.Header.VisiblePosition = 12;
            ultraGridColumn18.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
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
            ultraGridColumn18});
            this.cmbTraderShortName.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbTraderShortName.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTraderShortName.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTraderShortName.DisplayLayout.GroupByBox.Appearance = appearance15;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTraderShortName.DisplayLayout.GroupByBox.BandLabelAppearance = appearance16;
            this.cmbTraderShortName.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTraderShortName.DisplayLayout.GroupByBox.PromptAppearance = appearance17;
            this.cmbTraderShortName.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTraderShortName.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTraderShortName.DisplayLayout.Override.ActiveCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.SystemColors.Highlight;
            appearance19.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTraderShortName.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            this.cmbTraderShortName.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTraderShortName.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTraderShortName.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTraderShortName.DisplayLayout.Override.CellAppearance = appearance21;
            this.cmbTraderShortName.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTraderShortName.DisplayLayout.Override.CellPadding = 0;
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTraderShortName.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            appearance23.TextHAlignAsString = "Left";
            this.cmbTraderShortName.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.cmbTraderShortName.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTraderShortName.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            this.cmbTraderShortName.DisplayLayout.Override.RowAppearance = appearance24;
            this.cmbTraderShortName.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTraderShortName.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.cmbTraderShortName.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTraderShortName.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTraderShortName.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTraderShortName.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTraderShortName.DropDownWidth = 0;
            this.cmbTraderShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbTraderShortName.Location = new System.Drawing.Point(164, 51);
            this.cmbTraderShortName.Name = "cmbTraderShortName";
            this.cmbTraderShortName.Size = new System.Drawing.Size(162, 21);
            this.cmbTraderShortName.TabIndex = 3;
            this.cmbTraderShortName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTraderShortName.GotFocus += new System.EventHandler(this.cmbTraderShortName_GotFocus);
            this.cmbTraderShortName.LostFocus += new System.EventHandler(this.cmbTraderShortName_LostFocus);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(138, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 8);
            this.label6.TabIndex = 129;
            this.label6.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(139, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 8);
            this.label1.TabIndex = 130;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(139, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 8);
            this.label2.TabIndex = 131;
            this.label2.Text = "*";
            // 
            // txtClientTradingAccount
            // 
            this.txtClientTradingAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClientTradingAccount.Location = new System.Drawing.Point(164, 5);
            this.txtClientTradingAccount.MaxLength = 50;
            this.txtClientTradingAccount.Name = "txtClientTradingAccount";
            this.txtClientTradingAccount.Size = new System.Drawing.Size(162, 21);
            this.txtClientTradingAccount.TabIndex = 1;
            this.txtClientTradingAccount.GotFocus += new System.EventHandler(this.txtClientTradingAccount_GotFocus);
            this.txtClientTradingAccount.LostFocus += new System.EventHandler(this.txtClientTradingAccount_LostFocus);
            // 
            // CreateCompanyClientTradingAccount
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(340, 103);
            this.Controls.Add(this.txtClientTradingAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbTraderShortName);
            this.Controls.Add(this.cmbCompanyTradingAccount);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblExchangeLogo);
            this.Controls.Add(this.lblFlag);
            this.Controls.Add(this.ultraLabel39);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(312, 126);
            this.Name = "CreateCompanyClientTradingAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Trading Account Mapping";
            this.Load += new System.EventHandler(this.CreateCompanyClientTradingAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTradingAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTraderShortName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        #region Focus Colors
        private void txtClientTradingAccount_GotFocus(object sender, System.EventArgs e)
        {
            txtClientTradingAccount.BackColor = Color.LemonChiffon;
        }
        private void txtClientTradingAccount_LostFocus(object sender, System.EventArgs e)
        {
            txtClientTradingAccount.BackColor = Color.White;
        }
        private void cmbCompanyTradingAccount_GotFocus(object sender, System.EventArgs e)
        {
            cmbCompanyTradingAccount.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCompanyTradingAccount_LostFocus(object sender, System.EventArgs e)
        {
            cmbCompanyTradingAccount.Appearance.BackColor = Color.White;
        }
        private void cmbTraderShortName_GotFocus(object sender, System.EventArgs e)
        {
            cmbTraderShortName.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbTraderShortName_LostFocus(object sender, System.EventArgs e)
        {
            cmbTraderShortName.Appearance.BackColor = Color.White;
        }
        #endregion



        #region Private Methods
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider.SetError(txtClientTradingAccount, "");
                errorProvider.SetError(cmbTraderShortName, "");
                errorProvider.SetError(cmbCompanyTradingAccount, "");
                if (txtClientTradingAccount.Text.Trim().Equals(string.Empty))
                {
                    errorProvider.SetError(txtClientTradingAccount, "Please Enter A Value");
                    return;
                }
                if (cmbCompanyTradingAccount.Text.Equals(C_COMBO_SELECT))
                {
                    errorProvider.SetError(cmbCompanyTradingAccount, "Please Select a Trading Account");
                    return;
                }
                if (cmbTraderShortName.Text.Equals(C_COMBO_SELECT))
                {
                    errorProvider.SetError(cmbTraderShortName, "Please Select a Trading Account");
                    return;
                }
                string tradingAccountName = cmbCompanyTradingAccount.SelectedRow.Cells["TradingAccountName"].Text;
                string clientTraderShortName = cmbTraderShortName.SelectedRow.Cells["ShortName"].Text;
                int tradingAccountID = int.Parse(cmbCompanyTradingAccount.Value.ToString());
                int traderID = int.Parse(cmbTraderShortName.Value.ToString());

                foreach (Trader trader in _traders)
                {
                    if (trader.ShortName == strTraderPreviousStName)
                    {
                        trader.SetReference(false);

                    }
                    if (trader.ShortName == clientTraderShortName)
                    {
                        trader.SetReference(true);

                    }

                }
                CompanyClientTradingAccount companyClientTradingAccount = new CompanyClientTradingAccount(txtClientTradingAccount.Text.Trim(), tradingAccountID, tradingAccountName, _companyClientID, traderID, clientTraderShortName);




                _companyClientTradingAccount = companyClientTradingAccount;
                txtClientTradingAccount.Enabled = true;
                this.Hide();

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

        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            txtClientTradingAccount.Enabled = true;
            this.Hide();
        }

        private void CreateCompanyClientTradingAccount_Load(object sender, System.EventArgs e)
        {
            try
            {
                BindCompanyTradingAccount();
                BindCompanyTradersShortNames();
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



        }

        private void BindCompanyTradingAccount()
        {
            try
            {
                TradingAccounts tradingAccounts = CompanyManager.GetTradingAccount(_companyID);
                tradingAccounts.Insert(0, new TradingAccount(int.MinValue, C_COMBO_SELECT));


                if (tradingAccounts.Count > 0)
                {




                    cmbCompanyTradingAccount.DisplayMember = "TradingAccountName";
                    cmbCompanyTradingAccount.ValueMember = "TradingAccountsID";
                    cmbCompanyTradingAccount.DataSource = null;
                    cmbCompanyTradingAccount.DataSource = tradingAccounts;
                    cmbCompanyTradingAccount.Value = int.MinValue;
                    //cmbCompanyTradingAccount.Text = C_COMBO_SELECT;


                    if (_isAddition == false)
                    {
                        txtClientTradingAccount.Text = _companyClientTradingAccount.CompClientTradingAccount;
                        txtClientTradingAccount.Enabled = false;

                        cmbCompanyTradingAccount.Value = _companyClientTradingAccount.CompanyTradingAccountID;
                    }
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

        private void BindCompanyTradersShortNames()

        {
            try
            {

                _traders.Insert(0, new Trader(int.MinValue));
                cmbTraderShortName.DisplayMember = "ShortName";
                cmbTraderShortName.ValueMember = "TraderID";
                cmbTraderShortName.DataSource = null;
                cmbTraderShortName.DataSource = _traders;
                cmbTraderShortName.Value = int.MinValue;
                if (_isAddition == false)
                {

                    cmbTraderShortName.Value = _companyClientTradingAccount.ClientTraderID;
                    strTraderPreviousStName = cmbTraderShortName.Text;
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
        //		private void EditTradingAccount()
        //		{
        //			try
        //			{
        //				txtClientTradingAccount.Text=_companyClientTradingAccount.CompClientTradingAccount;
        //				txtClientTradingAccount.Enabled=false;
        //				string tradingAccountName=((TradingAccount)cmbCompanyTradingAccount.Value).TradingAccountName;
        //				string clientTraderShortName= ((Trader) cmbTraderShortName.Value ).ShortName;
        //				_companyClientTradingAccount.CompanyClientTradingAccountID=int.Parse(cmbCompanyTradingAccount.Value.ToString());
        //				_companyClientTradingAccount.ClientTraderID=int.Parse(cmbTraderShortName.Value.ToString());
        //			}
        //			catch(Exception ex)
        //			{
        //				throw ex;
        //			}
        //		
        //		}
        #endregion


        #region Get Set Properties

        public int CompanyClientID


        {
            set
            {
                _companyClientID = value;

            }
            get
            {
                return _companyClientID;
            }

        }


        public Traders Traders


        {
            set
            {
                _traders = value;

            }
            get
            {
                return _traders;
            }

        }
        public bool ISAddition
        {
            get { return _isAddition; }
            set { _isAddition = value; }
        }


        public CompanyClientTradingAccount companyClientTradingAccount
        {

            set
            {
                _companyClientTradingAccount = value;
            }

            get
            {
                return _companyClientTradingAccount;
            }
        }
        #endregion





    }
}
