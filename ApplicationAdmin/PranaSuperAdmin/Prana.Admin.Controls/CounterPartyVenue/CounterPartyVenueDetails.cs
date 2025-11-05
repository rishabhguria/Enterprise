using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Interface;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CounterPartyVenueDetails.
    /// </summary>
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueDeleted, ShowAuditUI = true)]
    public class CounterPartyVenueDetails : System.Windows.Forms.UserControl, IAuditSource
    {
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "Login : ";
        const int BASE_CURRENCY_TYPE = 1;
        const int OTHER_CURRENCY_TYPE = 2;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblOatsIdentifier;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label43;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCurrency;
        private System.Windows.Forms.Label lblBaseCurrency;
        private System.Windows.Forms.CheckedListBox checkedListSymbolConvention;
        private System.Windows.Forms.CheckedListBox checkedListBoxOtherCurrency;
        private System.Windows.Forms.Label lblOtherCurrency;
        private System.Windows.Forms.Label lblSymbolConvention;
        private System.Windows.Forms.TextBox txtOatsIdentifierID;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterPartyDetailsElectronic;
        private UltraCombo cmbSymbolConvention;
        private IContainer components;

        [AuditManager.Attributes.AuditSourceConstAttri]
        public CounterPartyVenueDetails()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                //PaintGradient();
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.groupBox1.BackColor = System.Drawing.Color.Transparent;
                this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox1.ForeColor = System.Drawing.Color.White;
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
        /// Applying gradient theme
        /// </summary>
        private void PaintGradient()
        {
            try
            {
                System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
                gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, this.Height), System.Drawing.Color.Black, System.Drawing.Color.White);
                Bitmap bmp = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(gradBrush, new Rectangle(0, 0, this.Width, this.Height));

                this.BackgroundImage = bmp;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                this.groupBox1.BackColor = System.Drawing.Color.Transparent;
                this.groupBox1.BackgroundImage = bmp;
                this.groupBox1.ForeColor = System.Drawing.Color.White;
                this.groupBox1.BackgroundImageLayout = ImageLayout.Stretch;
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
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label29 != null)
                {
                    label29.Dispose();
                }
                if (label28 != null)
                {
                    label28.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (txtDisplayName != null)
                {
                    txtDisplayName.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (lblOatsIdentifier != null)
                {
                    lblOatsIdentifier.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label43 != null)
                {
                    label43.Dispose();
                }
                if (cmbCurrency != null)
                {
                    cmbCurrency.Dispose();
                }
                if (lblBaseCurrency != null)
                {
                    lblBaseCurrency.Dispose();
                }
                if (checkedListSymbolConvention != null)
                {
                    checkedListSymbolConvention.Dispose();
                }
                if (checkedListBoxOtherCurrency != null)
                {
                    checkedListBoxOtherCurrency.Dispose();
                }
                if (lblOtherCurrency != null)
                {
                    lblOtherCurrency.Dispose();
                }
                if (lblSymbolConvention != null)
                {
                    lblSymbolConvention.Dispose();
                }
                if (txtOatsIdentifierID != null)
                {
                    txtOatsIdentifierID.Dispose();
                }
                if (cmbCounterPartyDetailsElectronic != null)
                {
                    cmbCounterPartyDetailsElectronic.Dispose();
                }
                if (cmbSymbolConvention != null)
                {
                    cmbSymbolConvention.Dispose();
                }
                if (_statusBar != null)
                {
                    _statusBar.Dispose();
                }
                base.Dispose(disposing);
            }
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SymbolConventionID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SymbolConventionName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 3);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 2);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
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
            this.label1 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbSymbolConvention = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtOatsIdentifierID = new System.Windows.Forms.TextBox();
            this.lblOtherCurrency = new System.Windows.Forms.Label();
            this.checkedListBoxOtherCurrency = new System.Windows.Forms.CheckedListBox();
            this.label43 = new System.Windows.Forms.Label();
            this.cmbCurrency = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblBaseCurrency = new System.Windows.Forms.Label();
            this.checkedListSymbolConvention = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSymbolConvention = new System.Windows.Forms.Label();
            this.lblOatsIdentifier = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCounterPartyDetailsElectronic = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbolConvention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyDetailsElectronic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(92, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 10);
            this.label1.TabIndex = 31;
            this.label1.Text = "*";
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label29.Location = new System.Drawing.Point(4, 42);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(64, 16);
            this.label29.TabIndex = 16;
            this.label29.Text = "Electronic";
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label28.Location = new System.Drawing.Point(4, 20);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(88, 16);
            this.label28.TabIndex = 15;
            this.label28.Text = "Display Name";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(18, 307);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 20);
            this.label3.TabIndex = 33;
            this.label3.Text = "* Required Field";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDisplayName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDisplayName.Location = new System.Drawing.Point(138, 18);
            this.txtDisplayName.MaxLength = 50;
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(202, 21);
            this.txtDisplayName.TabIndex = 1;
            this.txtDisplayName.GotFocus += new System.EventHandler(this.txtDisplayName_GotFocus);
            this.txtDisplayName.LostFocus += new System.EventHandler(this.txtDisplayName_LostFocus);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbSymbolConvention);
            this.groupBox1.Controls.Add(this.txtOatsIdentifierID);
            this.groupBox1.Controls.Add(this.lblOtherCurrency);
            this.groupBox1.Controls.Add(this.checkedListBoxOtherCurrency);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.cmbCurrency);
            this.groupBox1.Controls.Add(this.lblBaseCurrency);
            this.groupBox1.Controls.Add(this.checkedListSymbolConvention);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblSymbolConvention);
            this.groupBox1.Controls.Add(this.lblOatsIdentifier);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDisplayName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbCounterPartyDetailsElectronic);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 442);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Broker Venue Details";
            // 
            // cmbSymbolConvention
            // 
            this.cmbSymbolConvention.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSymbolConvention.DisplayLayout.Appearance = appearance1;
            this.cmbSymbolConvention.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            this.cmbSymbolConvention.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbSymbolConvention.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSymbolConvention.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbSymbolConvention.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSymbolConvention.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSymbolConvention.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSymbolConvention.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbSymbolConvention.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSymbolConvention.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSymbolConvention.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSymbolConvention.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbSymbolConvention.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSymbolConvention.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSymbolConvention.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbSymbolConvention.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbSymbolConvention.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSymbolConvention.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbSymbolConvention.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbSymbolConvention.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSymbolConvention.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbSymbolConvention.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSymbolConvention.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSymbolConvention.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSymbolConvention.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSymbolConvention.DropDownWidth = 0;
            this.cmbSymbolConvention.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbSymbolConvention.Location = new System.Drawing.Point(138, 243);
            this.cmbSymbolConvention.MaxDropDownItems = 12;
            this.cmbSymbolConvention.Name = "cmbSymbolConvention";
            this.cmbSymbolConvention.Size = new System.Drawing.Size(202, 21);
            this.cmbSymbolConvention.TabIndex = 6;
            this.cmbSymbolConvention.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // txtOatsIdentifierID
            // 
            this.txtOatsIdentifierID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOatsIdentifierID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtOatsIdentifierID.Location = new System.Drawing.Point(138, 62);
            this.txtOatsIdentifierID.MaxLength = 20;
            this.txtOatsIdentifierID.Name = "txtOatsIdentifierID";
            this.txtOatsIdentifierID.Size = new System.Drawing.Size(202, 21);
            this.txtOatsIdentifierID.TabIndex = 3;
            this.txtOatsIdentifierID.GotFocus += new System.EventHandler(this.txtOatsIdentifierID_GotFocus);
            this.txtOatsIdentifierID.LostFocus += new System.EventHandler(this.txtOatsIdentifierID_LostFocus);
            // 
            // lblOtherCurrency
            // 
            this.lblOtherCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblOtherCurrency.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblOtherCurrency.Location = new System.Drawing.Point(2, 146);
            this.lblOtherCurrency.Name = "lblOtherCurrency";
            this.lblOtherCurrency.Size = new System.Drawing.Size(94, 16);
            this.lblOtherCurrency.TabIndex = 80;
            this.lblOtherCurrency.Text = "Other Currency";
            this.lblOtherCurrency.Click += new System.EventHandler(this.lblOtherCurrency_Click);
            // 
            // checkedListBoxOtherCurrency
            // 
            this.checkedListBoxOtherCurrency.CheckOnClick = true;
            this.checkedListBoxOtherCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedListBoxOtherCurrency.HorizontalExtent = 500;
            this.checkedListBoxOtherCurrency.HorizontalScrollbar = true;
            this.checkedListBoxOtherCurrency.Location = new System.Drawing.Point(138, 104);
            this.checkedListBoxOtherCurrency.Name = "checkedListBoxOtherCurrency";
            this.checkedListBoxOtherCurrency.Size = new System.Drawing.Size(202, 100);
            this.checkedListBoxOtherCurrency.TabIndex = 5;
            this.checkedListBoxOtherCurrency.GotFocus += new System.EventHandler(this.checkedListBoxOtherCurrency_GotFocus);
            this.checkedListBoxOtherCurrency.LostFocus += new System.EventHandler(this.checkedListBoxOtherCurrency_LostFocus);
            // 
            // label43
            // 
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(94, 86);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(8, 8);
            this.label43.TabIndex = 78;
            this.label43.Text = "*";
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCurrency.DisplayLayout.Appearance = appearance13;
            this.cmbCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 2;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7});
            this.cmbCurrency.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbCurrency.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrency.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrency.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrency.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbCurrency.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrency.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrency.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCurrency.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCurrency.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbCurrency.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCurrency.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCurrency.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCurrency.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCurrency.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrency.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbCurrency.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCurrency.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbCurrency.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbCurrency.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCurrency.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCurrency.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCurrency.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCurrency.DropDownWidth = 0;
            this.cmbCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCurrency.Location = new System.Drawing.Point(138, 84);
            this.cmbCurrency.MaxDropDownItems = 12;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(202, 21);
            this.cmbCurrency.TabIndex = 4;
            this.cmbCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCurrency.ValueChanged += new System.EventHandler(this.cmbCurrency_ValueChanged);
            this.cmbCurrency.GotFocus += new System.EventHandler(this.cmbCurrency_GotFocus);
            this.cmbCurrency.LostFocus += new System.EventHandler(this.cmbCurrency_LostFocus);
            // 
            // lblBaseCurrency
            // 
            this.lblBaseCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblBaseCurrency.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblBaseCurrency.Location = new System.Drawing.Point(4, 86);
            this.lblBaseCurrency.Name = "lblBaseCurrency";
            this.lblBaseCurrency.Size = new System.Drawing.Size(92, 16);
            this.lblBaseCurrency.TabIndex = 76;
            this.lblBaseCurrency.Text = "Base Currency";
            // 
            // checkedListSymbolConvention
            // 
            this.checkedListSymbolConvention.CheckOnClick = true;
            this.checkedListSymbolConvention.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedListSymbolConvention.HorizontalExtent = 500;
            this.checkedListSymbolConvention.HorizontalScrollbar = true;
            this.checkedListSymbolConvention.Location = new System.Drawing.Point(138, 270);
            this.checkedListSymbolConvention.Name = "checkedListSymbolConvention";
            this.checkedListSymbolConvention.Size = new System.Drawing.Size(202, 20);
            this.checkedListSymbolConvention.TabIndex = 7;
            this.checkedListSymbolConvention.Visible = false;
            this.checkedListSymbolConvention.GotFocus += new System.EventHandler(this.checkedListSymbolConvention_GotFocus);
            this.checkedListSymbolConvention.LostFocus += new System.EventHandler(this.checkedListSymbolConvention_LostFocus);
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(116, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 8);
            this.label5.TabIndex = 48;
            this.label5.Text = "*";
            // 
            // lblSymbolConvention
            // 
            this.lblSymbolConvention.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblSymbolConvention.Location = new System.Drawing.Point(2, 248);
            this.lblSymbolConvention.Name = "lblSymbolConvention";
            this.lblSymbolConvention.Size = new System.Drawing.Size(120, 16);
            this.lblSymbolConvention.TabIndex = 47;
            this.lblSymbolConvention.Text = "Symbol Convention";
            // 
            // lblOatsIdentifier
            // 
            this.lblOatsIdentifier.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblOatsIdentifier.Location = new System.Drawing.Point(4, 64);
            this.lblOatsIdentifier.Name = "lblOatsIdentifier";
            this.lblOatsIdentifier.Size = new System.Drawing.Size(92, 16);
            this.lblOatsIdentifier.TabIndex = 43;
            this.lblOatsIdentifier.Text = "OATS Identifier";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(66, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 10);
            this.label7.TabIndex = 38;
            this.label7.Text = "*";
            // 
            // cmbCounterPartyDetailsElectronic
            // 
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Appearance = appearance25;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn8.Header.VisiblePosition = 0;
            ultraGridColumn9.Header.VisiblePosition = 1;
            ultraGridColumn9.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn8,
            ultraGridColumn9});
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterPartyDetailsElectronic.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterPartyDetailsElectronic.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterPartyDetailsElectronic.DropDownWidth = 0;
            this.cmbCounterPartyDetailsElectronic.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCounterPartyDetailsElectronic.Location = new System.Drawing.Point(138, 40);
            this.cmbCounterPartyDetailsElectronic.MaxDropDownItems = 12;
            this.cmbCounterPartyDetailsElectronic.Name = "cmbCounterPartyDetailsElectronic";
            this.cmbCounterPartyDetailsElectronic.Size = new System.Drawing.Size(202, 21);
            this.cmbCounterPartyDetailsElectronic.TabIndex = 2;
            this.cmbCounterPartyDetailsElectronic.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterPartyDetailsElectronic.GotFocus += new System.EventHandler(this.cmbCounterPartyDetailsElectronic_GotFocus);
            this.cmbCounterPartyDetailsElectronic.LostFocus += new System.EventHandler(this.cmbCounterPartyDetailsElectronic_LostFocus);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CounterPartyVenueDetails
            // 
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CounterPartyVenueDetails";
            this.Size = new System.Drawing.Size(366, 445);
            this.Load += new System.EventHandler(this.CounterPartyVenueDetails_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbolConvention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyDetailsElectronic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public CounterPartyVenue CounterPartyProperty
        {
            get
            {
                CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
                GetCounterPartyVenueDetails(counterPartyVenue);
                return counterPartyVenue;
            }
            set
            {
                //SetCounterPartyVenueDetails(value);
            }
        }

        private int _counterPartyID = int.MinValue;
        public int CounterPartyID
        {
            set
            {
                _counterPartyID = value;
            }
        }

        private int _venueID = int.MinValue;
        public int VenueID
        {
            set
            {
                _venueID = value;
            }
        }

        public void SetupControl(CounterPartyVenue counterPartyVenue)
        {
            BindIsElectronicID();
            BindCurrencies();
            BindOtherCurrencies();
            BindSymbolConventions();

            SetCounterPartyVenueDetails(counterPartyVenue);
            GetSelectedCounterPartyVenueID(counterPartyVenue.CounterPartyVenueID);
        }

        /// <summary>
        /// To pass selected counterPartyVenueID in Audit trail
        /// </summary>
        /// <param name="counterPartyVenueID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void GetSelectedCounterPartyVenueID(int counterPartyVenueID) { }

        public void GetCounterPartyVenueDetails(CounterPartyVenue counterPartyVenue)
        {
            //counterPartyVenue.CounterPartyVenueID = 1;
            counterPartyVenue.DisplayName = txtDisplayName.Text.Trim();
            counterPartyVenue.IsElectronic = int.Parse(cmbCounterPartyDetailsElectronic.Value.ToString());

            //return counterPartyVenue;
        }

        public int GetCounterPartyVenueDetailsForSave(CounterPartyVenue counterPartyVenue)
        {
            int result = int.MinValue;

            errorProvider1.SetError(txtDisplayName, "");
            errorProvider1.SetError(cmbCounterPartyDetailsElectronic, "");
            errorProvider1.SetError(txtOatsIdentifierID, "");
            errorProvider1.SetError(cmbCurrency, "");
            errorProvider1.SetError(checkedListBoxOtherCurrency, "");
            errorProvider1.SetError(checkedListSymbolConvention, "");
            errorProvider1.SetError(cmbSymbolConvention, "");

            if (txtDisplayName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtDisplayName, "Please enter display name!");
                txtDisplayName.Focus();
                return result;
            }
            else if (int.Parse(cmbCounterPartyDetailsElectronic.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCounterPartyDetailsElectronic, "Please select Electronic Details!");
                cmbCounterPartyDetailsElectronic.Focus();
                return result;
            }
            else if (int.Parse(cmbCurrency.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCurrency, "Please select Base Currency!");
                cmbCurrency.Focus();
                return result;
            }
            //else if(checkedListSymbolConvention.CheckedIndices.Count == 0)
            //{
            //    errorProvider1.SetError(checkedListSymbolConvention, "Please select Symbol Conventions!");
            //    checkedListSymbolConvention.Focus();
            //    return result;
            //}
            else if (int.Parse(cmbSymbolConvention.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbSymbolConvention, "Please select Symbol Convention!");
                cmbSymbolConvention.Focus();
                return result;
            }

            else
            {
                //counterPartyVenue.CounterPartyVenueID = 1;
                counterPartyVenue.CounterPartyID = _counterPartyID;
                counterPartyVenue.VenueID = _venueID;
                counterPartyVenue.DisplayName = txtDisplayName.Text.Trim();
                counterPartyVenue.IsElectronic = int.Parse(cmbCounterPartyDetailsElectronic.Value.ToString());
                counterPartyVenue.OatsIdentifier = txtOatsIdentifierID.Text.Trim();
                counterPartyVenue.SymbolConventionID = int.Parse(cmbSymbolConvention.Value.ToString());
                counterPartyVenue.BaseCurrencyID = int.Parse(cmbCurrency.Value.ToString());
                //counterPartyVenue.FixIdentifier = txtFixIdentifier.Text.Trim();
                System.Text.StringBuilder valueAUECs = new System.Text.StringBuilder(",");

                int resultantCounterPartyVenueID = CounterPartyManager.SaveCounterPartyVenue(counterPartyVenue);
                //Dont need now the following check as the venue is already binded depending upon the counter party id. 
                //if (resultantCounterPartyVenueID < 0)
                //{
                //    MessageBox.Show("Counter Party Venue with the same display name already exists.", "Alert", MessageBoxButtons.OK);
                //}

                if (resultantCounterPartyVenueID > 0)
                {
                    Currencies currencies = new Currencies();
                    Currency currency = new Currency();

                    int baseCurrencyID = int.Parse(cmbCurrency.Value.ToString());
                    //Following three lines are commented as no currency type is required as per the changes.
                    //int baseCurrencyTypeID = 1; //is hardcoded for now, have to change it to take the id from DB. 
                    //currencies.Add(new Currency(baseCurrencyID, baseCurrencyTypeID));
                    currencies.Add(new Currency(baseCurrencyID, int.MinValue));

                    int otherCurrencyID = int.MinValue;
                    for (int i = 0, count = checkedListBoxOtherCurrency.CheckedItems.Count; i < count; i++)
                    {
                        // int otherCurrencyTypeID = 2; //is hardcoded for now, have to change it to take the id from DB. 
                        otherCurrencyID = int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedListBoxOtherCurrency.CheckedItems[i]))).Row)).ItemArray[1].ToString());
                        currencies.Add(new Currency(otherCurrencyID, int.MinValue));
                    }
                    CounterPartyManager.SaveCVCurrency(currencies, resultantCounterPartyVenueID);

                    //int symbolConventionID = int.MinValue;
                    //SymbolConventions symbolConventions = new SymbolConventions();
                    //SymbolConvention symbolConvention = new SymbolConvention();

                    //for(int i=0, count = checkedListSymbolConvention.CheckedItems.Count; i<count; i++)
                    //{
                    //    symbolConventionID = int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedListSymbolConvention.CheckedItems[i]))).Row)).ItemArray[1].ToString());
                    //    symbolConventions.Add(new SymbolConvention(symbolConventionID, "", ""));
                    //}

                    //CounterPartyManager.SaveCVSymbolConvention(symbolConventions, counterPartyVenue.CounterPartyVenueID);

                    result = CounterPartyManager.SaveCVAUEC(resultantCounterPartyVenueID);

                    if (counterPartyVenue.CounterPartyVenueID == int.MinValue)
                    {
                        counterPartyVenue.CounterPartyVenueID = resultantCounterPartyVenueID;
                        AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, counterPartyVenue, AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueCreated);
                    }
                    else
                    {
                        AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, counterPartyVenue, AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueUpdated);
                    }
                    GetSelectedCounterPartyVenueID(counterPartyVenue.CounterPartyVenueID);
                }
                else
                {
                    result = resultantCounterPartyVenueID;
                }
            }
            return result;
        }

        public void SetCounterPartyVenueDetails(CounterPartyVenue counterPartyVenue)
        {
            if (counterPartyVenue.CounterPartyVenueID != int.MinValue)
            {


                if (counterPartyVenue != null)
                {
                    txtDisplayName.Text = counterPartyVenue.DisplayName;
                    cmbCounterPartyDetailsElectronic.Value = counterPartyVenue.IsElectronic;
                    txtOatsIdentifierID.Text = counterPartyVenue.OatsIdentifier.ToString();
                    cmbSymbolConvention.Value = counterPartyVenue.SymbolConventionID;
                    cmbCurrency.Value = counterPartyVenue.BaseCurrencyID;

                    string checkString = counterPartyVenue.AUECID.ToString();
                    char[] sep = { ',' };
                    Array a = checkString.Split(sep);

                    //				lstAUEC.SelectedIndex = -1;
                    //				if(counterPartyVenue.AUECID.ToString() != "")
                    //				{
                    //					for(int i=1;i<(a.Length-1);i++)
                    //					{
                    //
                    //						lstAUEC.SelectedValue = a.GetValue(i);
                    //						for (int j=0; j< checkedlstAUEC.Items.Count ; j++)
                    //						{
                    //								if (int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAUEC.Items[j]))).Row)).ItemArray[1].ToString()) == int.Parse(a.GetValue(i).ToString()))
                    //								{
                    //									checkedlstAUEC.SetItemChecked(j,true);
                    //								}
                    //						}
                    //					}
                    //				}
                    //				else
                    //				{
                    //					lstAUEC.SelectedValue = int.MinValue;
                    //				}



                    //New Code:
                    Currencies otherCurrencies = CounterPartyManager.GetCVCurrencies(counterPartyVenue.CounterPartyVenueID);
                    //SymbolConventions symbolConventions = CounterPartyManager.GetCVSymbolConventions(counterPartyVenue.CounterPartyVenueID);

                    if (otherCurrencies.Count >= 1)
                    {
                        bool flag = false;
                        int location = 0;
                        foreach (Currency currency in otherCurrencies)
                        {
                            if (currency.CurencyID == counterPartyVenue.BaseCurrencyID)
                            {
                                flag = true;
                                break;
                            }
                            location++;
                        }
                        if (flag == true)
                        {
                            otherCurrencies.RemoveAt(location);
                        }
                    }

                    for (int j = 0; j < checkedListBoxOtherCurrency.Items.Count; j++)
                    {
                        checkedListBoxOtherCurrency.SetItemChecked(j, false);
                    }

                    if (otherCurrencies.Count > 0)
                    {
                        foreach (Prana.Admin.BLL.Currency otherCurrency in otherCurrencies)
                        {
                            for (int j = 0; j < checkedListBoxOtherCurrency.Items.Count; j++)
                            {
                                if (int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedListBoxOtherCurrency.Items[j]))).Row)).ItemArray[1].ToString()) == int.Parse(otherCurrency.CurencyID.ToString()))
                                //if (((Prana.Admin.BLL.Currency)checkedListBoxOtherCurrency.Items[j]).CurencyID == int.Parse(otherCurrency.CurencyID.ToString()))
                                {
                                    checkedListBoxOtherCurrency.SetItemChecked(j, true);
                                }
                            }
                        }
                    }

                }
                ColumnsCollection columns = cmbCounterPartyDetailsElectronic.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Data")
                    {
                        column.Hidden = true;
                    }
                }

                columns = cmbCurrency.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "CurrencySymbol")
                    {
                        column.Hidden = true;
                    }
                }
            }
            else
            {
                RefreshForm();
            }
        }

        private void RefreshForm()
        {
            txtDisplayName.Text = "";
            cmbCounterPartyDetailsElectronic.Text = C_COMBO_SELECT;
            txtOatsIdentifierID.Text = "";
            cmbCurrency.Value = int.MinValue;
            cmbSymbolConvention.Value = int.MinValue;
            for (int j = 0; j < checkedListBoxOtherCurrency.Items.Count; j++)
            {
                checkedListBoxOtherCurrency.SetItemChecked(j, false);
            }
            //for (int j=0; j< checkedListSymbolConvention.Items.Count ; j++)
            //{
            //    checkedListSymbolConvention.SetItemChecked(j,false);
            //}


            ColumnsCollection columns = cmbCurrency.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "CurrencySymbol")
                {
                    column.Hidden = true;
                }
            }
        }

        private void BindIsElectronicID()
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
            cmbCounterPartyDetailsElectronic.DataSource = null;
            cmbCounterPartyDetailsElectronic.DataSource = dt;
            cmbCounterPartyDetailsElectronic.DisplayMember = "Data";
            cmbCounterPartyDetailsElectronic.ValueMember = "Value";
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCounterPartyDetailsElectronic.DisplayLayout.Bands[0].Columns)
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
            cmbCounterPartyDetailsElectronic.DisplayLayout.Bands[0].ColHeadersVisible = false;

        }

        /// <summary>
        /// This method binds the existing <see cref="Currencies"/> in the ComboBox control by assigning the 
        /// currencies object to its datasource property.
        /// </summary>
        private void BindCurrencies()
        {
            //GetCurrencies method fetches the existing currencies from the database.
            Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();
            //Inserting the - Select - option in the Combo Box at the top.
            currencies.Insert(0, new Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
            this.cmbCurrency.DataSource = null;
            this.cmbCurrency.DataSource = currencies;
            this.cmbCurrency.DisplayMember = "CurrencySymbol";
            this.cmbCurrency.ValueMember = "CurencyID";
            this.cmbCurrency.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCurrency.DisplayLayout.Bands[0].Columns)
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
            cmbCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        private void BindOtherCurrencies()
        {
            Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();

            int baseCurrencyID = int.Parse(cmbCurrency.Value.ToString());
            if (baseCurrencyID != int.MinValue)
            {
                int index = 0;
                foreach (Currency baseCurrency in currencies)
                {
                    if (baseCurrency.CurencyID == baseCurrencyID)
                    {
                        break;
                    }
                    index += 1;
                }
                currencies.RemoveAt(index);
            }

            System.Data.DataTable dtCurrencies = new System.Data.DataTable();
            dtCurrencies.Columns.Add("Data");
            dtCurrencies.Columns.Add("Value");
            //dtCurrencies.PrimaryKey = 
            object[] row = new object[2];

            Array arr = new Array[17];
            int i = 0;
            if (currencies.Count > 0)
            {
                foreach (Currency currency in currencies)
                {
                    string Data = currency.CurrencySymbol.ToString();
                    int Value = int.Parse(currency.CurencyID.ToString());

                    row[0] = Data;
                    row[1] = Value;
                    dtCurrencies.Rows.Add(row);
                    //arr = Value;
                    i++;
                }

                checkedListBoxOtherCurrency.DataSource = dtCurrencies;
                checkedListBoxOtherCurrency.DisplayMember = "Data";
                checkedListBoxOtherCurrency.ValueMember = "Value";
            }

        }

        private void BindSymbolConventions()
        {
            //old code
            SymbolConventions symbolConventions = SymbolManager.GetSymbolConventions();
            if (symbolConventions.Count > 0)
            {
                symbolConventions.Insert(0, new SymbolConvention(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
                cmbSymbolConvention.DataSource = null;
                cmbSymbolConvention.DataSource = symbolConventions;
                cmbSymbolConvention.DisplayMember = "SymbolConventionName";
                cmbSymbolConvention.ValueMember = "SymbolConventionID";
            }
            foreach (UltraGridColumn column in cmbSymbolConvention.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("SymbolConventionName"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            cmbSymbolConvention.DisplayLayout.Bands[0].ColHeadersVisible = false;
            //SymbolConventions symbolConventions = SymbolManager.GetSymbolConventions();
            //System.Data.DataTable dtSymbolConvention = new System.Data.DataTable();
            //dtSymbolConvention.Columns.Add("Data");
            //dtSymbolConvention.Columns.Add("Value");
            //object[] row = new object[2]; 

            //if(symbolConventions.Count > 0 )
            //{
            //    foreach(SymbolConvention symbolConvention in symbolConventions)
            //    {
            //        string Data = symbolConvention.SymbolConventionName.ToString();
            //        int Value = int.Parse(symbolConvention.SymbolConventionID.ToString());

            //        row[0] = Data;
            //        row[1] = Value;
            //        dtSymbolConvention.Rows.Add(row);
            //    }

            //    checkedListSymbolConvention.DataSource = dtSymbolConvention;
            //    checkedListSymbolConvention.DisplayMember = "Data";
            //    checkedListSymbolConvention.ValueMember = "Value";
            //}



        }



        private StatusBar _statusBar = null;
        public StatusBar ParentStatusBar
        {
            set { _statusBar = value; }
        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtDisplayName.Text = "";
        }

        private void CounterPartyVenueDetails_Load(object sender, System.EventArgs e)
        {
        }




        #region Controls Focus Colors


        private void cmbCounterPartyDetailsElectronic_GotFocus(object sender, System.EventArgs e)
        {
            cmbCounterPartyDetailsElectronic.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCounterPartyDetailsElectronic_LostFocus(object sender, System.EventArgs e)
        {
            cmbCounterPartyDetailsElectronic.Appearance.BackColor = Color.White;
        }
        private void checkedListBoxOtherCurrency_GotFocus(object sender, System.EventArgs e)
        {
            checkedListBoxOtherCurrency.BackColor = Color.LemonChiffon;
        }
        private void checkedListBoxOtherCurrency_LostFocus(object sender, System.EventArgs e)
        {
            checkedListBoxOtherCurrency.BackColor = Color.White;
        }
        private void checkedListSymbolConvention_GotFocus(object sender, System.EventArgs e)
        {
            checkedListSymbolConvention.BackColor = Color.LemonChiffon;
        }
        private void checkedListSymbolConvention_LostFocus(object sender, System.EventArgs e)
        {
            checkedListSymbolConvention.BackColor = Color.White;
        }


        private void txtDisplayName_GotFocus(object sender, System.EventArgs e)
        {
            txtDisplayName.BackColor = Color.LemonChiffon;
        }
        private void txtDisplayName_LostFocus(object sender, System.EventArgs e)
        {
            txtDisplayName.BackColor = Color.White;
        }
        private void txtOatsIdentifierID_GotFocus(object sender, System.EventArgs e)
        {
            txtOatsIdentifierID.BackColor = Color.LemonChiffon;
        }
        private void txtOatsIdentifierID_LostFocus(object sender, System.EventArgs e)
        {
            txtOatsIdentifierID.BackColor = Color.White;
        }
        private void cmbCurrency_GotFocus(object sender, System.EventArgs e)
        {
            cmbCurrency.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCurrency_LostFocus(object sender, System.EventArgs e)
        {
            cmbCurrency.Appearance.BackColor = Color.White;
        }

        #endregion

        private void lstAUEC_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        private void lstAUEC_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void lblOtherCurrency_Click(object sender, System.EventArgs e)
        {
        }

        private void cmbCurrency_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCurrency.Value != null)
            {
                if (int.Parse(cmbCurrency.Value.ToString()) > 0)
                {
                    BindOtherCurrencies();
                }
            }
        }



        private int _currentAUECSelectedIndex = 0;
        private void checkedlstAUEC_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _currentAUECSelectedIndex = e.Index;
        }

    }
}
