using System.ComponentModel;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CounterPartyVenueAcceptedOrderTypes.
    /// </summary>
    public class uctCounterPartyVenueAcceptedOrderTypes : System.Windows.Forms.UserControl
    {
        public uctCounterPartyVenueAcceptedOrderTypes()
        {
            InitializeComponent();
        }
        private const string FORM_NAME = "uctCounterPartyVenueAcceptedOrderTypes : ";
        const string C_COMBO_SELECT = "- Select -";
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ListBox lstSide;
        private System.Windows.Forms.CheckedListBox checkedlstSide;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbFollowCompliance;
        private System.Windows.Forms.Label lblFollowCompliance;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbShortSellConfirmation;
        private System.Windows.Forms.Label lblShortSellConfirmation;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbIdentifier;
        private System.Windows.Forms.Label lblIdentifier;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblForeignID;
        private System.Windows.Forms.TextBox txtForeignID;
        private System.Windows.Forms.Label lblAUEC;
        private System.Windows.Forms.GroupBox grpCompliance;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAdvancedOrders;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAlgos;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAUEC;
        private IContainer components;



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
                if (label42 != null)
                {
                    label42.Dispose();
                }
                if (label41 != null)
                {
                    label41.Dispose();
                }
                if (label36 != null)
                {
                    label36.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (lstSide != null)
                {
                    lstSide.Dispose();
                }
                if (checkedlstSide != null)
                {
                    checkedlstSide.Dispose();
                }
                if (cmbFollowCompliance != null)
                {
                    cmbFollowCompliance.Dispose();
                }
                if (lblFollowCompliance != null)
                {
                    lblFollowCompliance.Dispose();
                }
                if (cmbShortSellConfirmation != null)
                {
                    cmbShortSellConfirmation.Dispose();
                }
                if (lblShortSellConfirmation != null)
                {
                    lblShortSellConfirmation.Dispose();
                }
                if (cmbIdentifier != null)
                {
                    cmbIdentifier.Dispose();
                }
                if (lblIdentifier != null)
                {
                    lblIdentifier.Dispose();
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
                if (lblForeignID != null)
                {
                    lblForeignID.Dispose();
                }
                if (txtForeignID != null)
                {
                    txtForeignID.Dispose();
                }
                if (lblAUEC != null)
                {
                    lblAUEC.Dispose();
                }
                if (grpCompliance != null)
                {
                    grpCompliance.Dispose();
                }
                if (cmbAdvancedOrders != null)
                {
                    cmbAdvancedOrders.Dispose();
                }
                if (cmbAlgos != null)
                {
                    cmbAlgos.Dispose();
                }
                if (cmbAUEC != null)
                {
                    cmbAUEC.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientIdentifierName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryKey", 3);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 1);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.grpCompliance = new System.Windows.Forms.GroupBox();
            this.cmbAUEC = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbAlgos = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbAdvancedOrders = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtForeignID = new System.Windows.Forms.TextBox();
            this.lblForeignID = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbIdentifier = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblIdentifier = new System.Windows.Forms.Label();
            this.cmbShortSellConfirmation = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblShortSellConfirmation = new System.Windows.Forms.Label();
            this.cmbFollowCompliance = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblFollowCompliance = new System.Windows.Forms.Label();
            this.lblAUEC = new System.Windows.Forms.Label();
            this.checkedlstSide = new System.Windows.Forms.CheckedListBox();
            this.lstSide = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpCompliance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlgos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAdvancedOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShortSellConfirmation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFollowCompliance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label42
            // 
            this.label42.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label42.Location = new System.Drawing.Point(12, 514);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(36, 16);
            this.label42.TabIndex = 20;
            this.label42.Text = "Algos";
            this.label42.Visible = false;
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label41.Location = new System.Drawing.Point(12, 494);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(104, 16);
            this.label41.TabIndex = 19;
            this.label41.Text = "Advanced Orders";
            this.label41.Visible = false;
            // 
            // label36
            // 
            this.label36.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label36.Location = new System.Drawing.Point(12, 158);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(32, 16);
            this.label36.TabIndex = 14;
            this.label36.Text = "Side";
            // 
            // grpCompliance
            // 
            this.grpCompliance.Controls.Add(this.cmbAUEC);
            this.grpCompliance.Controls.Add(this.cmbAlgos);
            this.grpCompliance.Controls.Add(this.cmbAdvancedOrders);
            this.grpCompliance.Controls.Add(this.txtForeignID);
            this.grpCompliance.Controls.Add(this.lblForeignID);
            this.grpCompliance.Controls.Add(this.label12);
            this.grpCompliance.Controls.Add(this.label11);
            this.grpCompliance.Controls.Add(this.label10);
            this.grpCompliance.Controls.Add(this.label9);
            this.grpCompliance.Controls.Add(this.cmbIdentifier);
            this.grpCompliance.Controls.Add(this.lblIdentifier);
            this.grpCompliance.Controls.Add(this.cmbShortSellConfirmation);
            this.grpCompliance.Controls.Add(this.lblShortSellConfirmation);
            this.grpCompliance.Controls.Add(this.cmbFollowCompliance);
            this.grpCompliance.Controls.Add(this.lblFollowCompliance);
            this.grpCompliance.Controls.Add(this.lblAUEC);
            this.grpCompliance.Controls.Add(this.checkedlstSide);
            this.grpCompliance.Controls.Add(this.lstSide);
            this.grpCompliance.Controls.Add(this.label8);
            this.grpCompliance.Controls.Add(this.label7);
            this.grpCompliance.Controls.Add(this.label4);
            this.grpCompliance.Controls.Add(this.label42);
            this.grpCompliance.Controls.Add(this.label41);
            this.grpCompliance.Controls.Add(this.label36);
            this.grpCompliance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCompliance.Location = new System.Drawing.Point(4, -2);
            this.grpCompliance.Name = "grpCompliance";
            this.grpCompliance.Size = new System.Drawing.Size(352, 518);
            this.grpCompliance.TabIndex = 28;
            this.grpCompliance.TabStop = false;
            this.grpCompliance.Text = "Compliance";
            //this.grpCompliance.Enter += new System.EventHandler(this.cmbCompliance_Enter);
            // 
            // cmbAUEC
            // 
            this.cmbAUEC.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAUEC.DisplayLayout.Appearance = appearance1;
            this.cmbAUEC.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbAUEC.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbAUEC.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAUEC.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbAUEC.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAUEC.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbAUEC.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAUEC.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAUEC.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAUEC.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAUEC.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbAUEC.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAUEC.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbAUEC.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbAUEC.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAUEC.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbAUEC.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbAUEC.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAUEC.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbAUEC.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAUEC.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAUEC.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAUEC.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAUEC.DropDownWidth = 0;
            this.cmbAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAUEC.Location = new System.Drawing.Point(156, 20);
            this.cmbAUEC.MaxDropDownItems = 12;
            this.cmbAUEC.Name = "cmbAUEC";
            this.cmbAUEC.Size = new System.Drawing.Size(176, 21);
            this.cmbAUEC.TabIndex = 1;
            this.cmbAUEC.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            //this.cmbAUEC.ValueChanged += new System.EventHandler(this.cmbAUEC_ValueChanged);
            //this.cmbAUEC.GotFocus += new System.EventHandler(this.cmbAUEC_GotFocus);
            // 
            // cmbAlgos
            // 
            this.cmbAlgos.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn3.Width = 170;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn4.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            this.cmbAlgos.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbAlgos.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAlgos.DropDownWidth = 0;
            this.cmbAlgos.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAlgos.Location = new System.Drawing.Point(156, 512);
            this.cmbAlgos.MaxDropDownItems = 12;
            this.cmbAlgos.Name = "cmbAlgos";
            this.cmbAlgos.Size = new System.Drawing.Size(176, 21);
            this.cmbAlgos.TabIndex = 91;
            this.cmbAlgos.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAlgos.Visible = false;
            //this.cmbAlgos.GotFocus += new System.EventHandler(this.cmbAlgos_GotFocus);
            //this.cmbAlgos.LostFocus += new System.EventHandler(this.cmbAlgos_LostFocus);
            // 
            // cmbAdvancedOrders
            // 
            this.cmbAdvancedOrders.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn5.Width = 170;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridColumn6.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6});
            this.cmbAdvancedOrders.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbAdvancedOrders.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAdvancedOrders.DropDownWidth = 0;
            this.cmbAdvancedOrders.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAdvancedOrders.Location = new System.Drawing.Point(156, 490);
            this.cmbAdvancedOrders.MaxDropDownItems = 12;
            this.cmbAdvancedOrders.Name = "cmbAdvancedOrders";
            this.cmbAdvancedOrders.Size = new System.Drawing.Size(176, 21);
            this.cmbAdvancedOrders.TabIndex = 90;
            this.cmbAdvancedOrders.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAdvancedOrders.Visible = false;
            //this.cmbAdvancedOrders.GotFocus += new System.EventHandler(this.cmbAdvancedOrders_GotFocus);
            //this.cmbAdvancedOrders.LostFocus += new System.EventHandler(this.cmbAdvancedOrders_LostFocus);
            // 
            // txtForeignID
            // 
            this.txtForeignID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtForeignID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtForeignID.Location = new System.Drawing.Point(156, 108);
            this.txtForeignID.MaxLength = 20;
            this.txtForeignID.Name = "txtForeignID";
            this.txtForeignID.Size = new System.Drawing.Size(176, 21);
            this.txtForeignID.TabIndex = 5;
            //this.txtForeignID.GotFocus += new System.EventHandler(this.txtForeignID_GotFocus);
            //this.txtForeignID.LostFocus += new System.EventHandler(this.txtForeignID_LostFocus);
            // 
            // lblForeignID
            // 
            this.lblForeignID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblForeignID.Location = new System.Drawing.Point(12, 110);
            this.lblForeignID.Name = "lblForeignID";
            this.lblForeignID.Size = new System.Drawing.Size(64, 16);
            this.lblForeignID.TabIndex = 88;
            this.lblForeignID.Text = "Foreign ID";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(50, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 8);
            this.label12.TabIndex = 87;
            this.label12.Text = "*";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(120, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(12, 8);
            this.label11.TabIndex = 86;
            this.label11.Text = "*";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(142, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 6);
            this.label10.TabIndex = 85;
            this.label10.Text = "*";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(70, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 8);
            this.label9.TabIndex = 84;
            this.label9.Text = "*";
            // 
            // cmbIdentifier
            // 
            this.cmbIdentifier.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridColumn7.Header.VisiblePosition = 0;
            ultraGridColumn7.Width = 170;
            ultraGridColumn8.Header.VisiblePosition = 1;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.Header.VisiblePosition = 2;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.Header.VisiblePosition = 3;
            ultraGridColumn10.Hidden = true;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10});
            this.cmbIdentifier.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbIdentifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbIdentifier.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbIdentifier.DropDownWidth = 0;
            this.cmbIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbIdentifier.Location = new System.Drawing.Point(156, 86);
            this.cmbIdentifier.MaxDropDownItems = 12;
            this.cmbIdentifier.Name = "cmbIdentifier";
            this.cmbIdentifier.Size = new System.Drawing.Size(176, 21);
            this.cmbIdentifier.TabIndex = 4;
            this.cmbIdentifier.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            //this.cmbIdentifier.GotFocus += new System.EventHandler(this.cmbIdentifier_GotFocus);
            //this.cmbIdentifier.LostFocus += new System.EventHandler(this.cmbIdentifier_LostFocus);
            // 
            // lblIdentifier
            // 
            this.lblIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIdentifier.Location = new System.Drawing.Point(12, 88);
            this.lblIdentifier.Name = "lblIdentifier";
            this.lblIdentifier.Size = new System.Drawing.Size(58, 16);
            this.lblIdentifier.TabIndex = 82;
            this.lblIdentifier.Text = "Identifier";
            // 
            // cmbShortSellConfirmation
            // 
            this.cmbShortSellConfirmation.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand5.ColHeadersVisible = false;
            ultraGridColumn11.Header.VisiblePosition = 0;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.Header.VisiblePosition = 1;
            ultraGridColumn12.Width = 170;
            ultraGridBand5.Columns.AddRange(new object[] {
            ultraGridColumn11,
            ultraGridColumn12});
            this.cmbShortSellConfirmation.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbShortSellConfirmation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbShortSellConfirmation.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbShortSellConfirmation.DropDownWidth = 0;
            this.cmbShortSellConfirmation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbShortSellConfirmation.Location = new System.Drawing.Point(156, 64);
            this.cmbShortSellConfirmation.MaxDropDownItems = 12;
            this.cmbShortSellConfirmation.Name = "cmbShortSellConfirmation";
            this.cmbShortSellConfirmation.Size = new System.Drawing.Size(176, 21);
            this.cmbShortSellConfirmation.TabIndex = 3;
            this.cmbShortSellConfirmation.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            //this.cmbShortSellConfirmation.GotFocus += new System.EventHandler(this.cmbShortSellConfirmation_GotFocus);
            //this.cmbShortSellConfirmation.LostFocus += new System.EventHandler(this.cmbShortSellConfirmation_LostFocus);
            // 
            // lblShortSellConfirmation
            // 
            this.lblShortSellConfirmation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblShortSellConfirmation.Location = new System.Drawing.Point(12, 66);
            this.lblShortSellConfirmation.Name = "lblShortSellConfirmation";
            this.lblShortSellConfirmation.Size = new System.Drawing.Size(136, 16);
            this.lblShortSellConfirmation.TabIndex = 80;
            this.lblShortSellConfirmation.Text = "ShortSell Confirmation";
            // 
            // cmbFollowCompliance
            // 
            this.cmbFollowCompliance.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand6.ColHeadersVisible = false;
            ultraGridColumn13.Header.VisiblePosition = 0;
            ultraGridColumn13.Width = 170;
            ultraGridColumn14.Header.VisiblePosition = 1;
            ultraGridColumn14.Hidden = true;
            ultraGridBand6.Columns.AddRange(new object[] {
            ultraGridColumn13,
            ultraGridColumn14});
            this.cmbFollowCompliance.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbFollowCompliance.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFollowCompliance.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFollowCompliance.DropDownWidth = 0;
            this.cmbFollowCompliance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbFollowCompliance.Location = new System.Drawing.Point(156, 42);
            this.cmbFollowCompliance.MaxDropDownItems = 12;
            this.cmbFollowCompliance.Name = "cmbFollowCompliance";
            this.cmbFollowCompliance.Size = new System.Drawing.Size(176, 21);
            this.cmbFollowCompliance.TabIndex = 2;
            this.cmbFollowCompliance.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            //this.cmbFollowCompliance.ValueChanged += new System.EventHandler(this.cmbFollowCompliance_ValueChanged);
            //this.cmbFollowCompliance.GotFocus += new System.EventHandler(this.cmbFollowCompliance_GotFocus);
            //this.cmbFollowCompliance.LostFocus += new System.EventHandler(this.cmbFollowCompliance_LostFocus);
            // 
            // lblFollowCompliance
            // 
            this.lblFollowCompliance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFollowCompliance.Location = new System.Drawing.Point(12, 44);
            this.lblFollowCompliance.Name = "lblFollowCompliance";
            this.lblFollowCompliance.Size = new System.Drawing.Size(112, 16);
            this.lblFollowCompliance.TabIndex = 78;
            this.lblFollowCompliance.Text = "Follow Compliance";
            // 
            // lblAUEC
            // 
            this.lblAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAUEC.Location = new System.Drawing.Point(12, 22);
            this.lblAUEC.Name = "lblAUEC";
            this.lblAUEC.Size = new System.Drawing.Size(38, 16);
            this.lblAUEC.TabIndex = 54;
            this.lblAUEC.Text = "AUEC";
            // 
            // checkedlstSide
            // 
            this.checkedlstSide.CheckOnClick = true;
            this.checkedlstSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstSide.Location = new System.Drawing.Point(156, 132);
            this.checkedlstSide.Name = "checkedlstSide";
            this.checkedlstSide.Size = new System.Drawing.Size(176, 68);
            this.checkedlstSide.TabIndex = 6;
            //this.checkedlstSide.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlstSide_ItemCheck);
            //this.checkedlstSide.SelectedIndexChanged += new System.EventHandler(this.checkedlstSide_SelectedIndexChanged);
            //this.checkedlstSide.SelectedValueChanged += new System.EventHandler(this.checkedlstSide_SelectedValueChanged);
            //this.checkedlstSide.DoubleClick += new System.EventHandler(this.checkedlstSide_DoubleClick);
            //this.checkedlstSide.KeyUp += new System.Windows.Forms.KeyEventHandler(this.checkedlstSide_KeyUp);
            // 
            // lstSide
            // 
            this.lstSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lstSide.Location = new System.Drawing.Point(156, 130);
            this.lstSide.Name = "lstSide";
            this.lstSide.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSide.Size = new System.Drawing.Size(42, 69);
            this.lstSide.TabIndex = 44;
            this.lstSide.Visible = false;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(48, 514);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 10);
            this.label8.TabIndex = 43;
            this.label8.Text = "*";
            this.label8.Visible = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(116, 494);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 12);
            this.label7.TabIndex = 42;
            this.label7.Text = "*";
            this.label7.Visible = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(44, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 10);
            this.label4.TabIndex = 40;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(16, 528);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 16);
            this.label3.TabIndex = 34;
            this.label3.Text = "* Required Field";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // uctCounterPartyVenueAcceptedOrderTypes
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpCompliance);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "uctCounterPartyVenueAcceptedOrderTypes";
            this.Size = new System.Drawing.Size(358, 554);
            //this.Load += new System.EventHandler(this.CounterPartyVenueAcceptedOrderTypes_Load);
            this.grpCompliance.ResumeLayout(false);
            this.grpCompliance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlgos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAdvancedOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShortSellConfirmation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFollowCompliance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        //        private StatusBar _statusBar = null;
        //        public StatusBar ParentStatusBar
        //        {
        //            set{_statusBar = value;}
        //        }

        //        public CounterPartyVenue _objCounterPartyVenue = new CounterPartyVenue();
        //        public CounterPartyVenue CounterPartyProperty
        //        {
        //            get 
        //            {
        //                CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
        //                GetCounterPartyVenueAcceptedOrderTypes(counterPartyVenue);
        //                return counterPartyVenue;
        //            }
        //            set
        //            {
        //                //SetCounterPartyVenueAcceptedOrderTypes(value);
        //                //_objCounterPartyVenue = value;
        //                //BindCVAUEC();
        //            }
        //        }

        //        public void SetupControl(CounterPartyVenue counterPartyVenue)
        //        {
        //            BindCVAUEC();
        //            BindIdentifiers();
        //            BindShortSellConfirmation();
        //            BindSide();
        //            BindFollowCompliance();
        //            SetCounterPartyVenueAcceptedOrderTypes(counterPartyVenue);
        //            _objCounterPartyVenue = counterPartyVenue;
        //            BindCVAUEC();
        //        }

        //        public void GetCounterPartyVenueAcceptedOrderTypes(CounterPartyVenue counterPartyVenue)
        //        {			
        ////			counterPartyVenue.SideID = int.Parse(lstSide.SelectedValue.ToString());
        ////			counterPartyVenue.OrderTypesID = int.Parse(lstOrderTypes.SelectedValue.ToString());
        ////			counterPartyVenue.TimeInForceID = int.Parse(lstTIF.SelectedValue.ToString());
        ////			counterPartyVenue.HandlingInstructionsID = int.Parse(lstHandlingInstructions.SelectedValue.ToString());
        ////			counterPartyVenue.ExecutionInstructionsID = int.Parse(lstExecutionInstr.SelectedValue.ToString());
        ////			counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
        //        }

        //        public int GetCounterPartyVenueAcceptedOrderTypesForSave(CounterPartyVenue counterPartyVenue)
        //        {	
        //            int result = int.MinValue;

        //            errorProvider1.SetError(cmbAUEC, "");
        //            errorProvider1.SetError(cmbFollowCompliance, "");
        //            errorProvider1.SetError(cmbShortSellConfirmation, "");
        //            errorProvider1.SetError(cmbIdentifier, "");
        //            errorProvider1.SetError(checkedlstSide, "");
        //            errorProvider1.SetError(checkedlstOrderTypes, "");
        //            errorProvider1.SetError(checkedlstHandlingInstructions, "");
        //            errorProvider1.SetError(cmbAdvancedOrders, "");
        //            errorProvider1.SetError(checkedlstTIF, "");
        //            errorProvider1.SetError(checkedlstExecutionInstr, "");

        //            #region Check if the fields have atleast one item selected
        //            if(int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
        //            {
        //                errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
        //                cmbAUEC.Focus();
        //                return result;
        //            }
        //            else if(int.Parse(cmbFollowCompliance.Value.ToString()) == int.MinValue)
        //            {
        //                errorProvider1.SetError(cmbFollowCompliance, "Please select Follow Compliance!");
        //                cmbFollowCompliance.Focus();
        //                return result;
        //            }


        //            else if(_followCompliance == false)
        //            {
        //                if(int.Parse(cmbShortSellConfirmation.Value.ToString()) == int.MinValue)
        //                {
        //                    errorProvider1.SetError(cmbShortSellConfirmation, "Please select ShortSell Confirmation!");
        //                    cmbShortSellConfirmation.Focus();
        //                    return result;
        //                }
        //                else if(int.Parse(cmbIdentifier.Value.ToString()) == int.MinValue)
        //                {
        //                    errorProvider1.SetError(cmbIdentifier, "Please select Identifier!");
        //                    cmbShortSellConfirmation.Focus();
        //                    return result;
        //                }
        //                else if(checkedlstSide.CheckedIndices.Count == 0)
        //                {
        //                    errorProvider1.SetError(checkedlstSide, "Please select some Side!");
        //                    checkedlstSide.Focus();
        //                    return result;
        //                }
        //                

        //            }
        //            
        //                #endregion
        //            else
        //            {



        //                int auecID = int.Parse(cmbAUEC.Value.ToString());
        //                CounterPartyVenue auecCounterPartyVenue = new CounterPartyVenue();
        //                auecCounterPartyVenue = CounterPartyManager.GetCVAUECDetails(counterPartyVenue.CounterPartyVenueID, auecID);
        //                int cvAUECID = int.Parse(auecCounterPartyVenue.CVAUECID.ToString());
        //                counterPartyVenue.CVAUECID = cvAUECID;

        //                counterPartyVenue.FollowCompliance = int.Parse(cmbFollowCompliance.Value.ToString());
        //                counterPartyVenue.ShortSellConfirmation = int.Parse(cmbShortSellConfirmation.Value.ToString());
        //                counterPartyVenue.IdentifierID = int.Parse(cmbIdentifier.Value.ToString());
        //                counterPartyVenue.ForeignID = txtForeignID.Text.ToString();

        //                int sideID = int.MinValue; 
        //                Sides sides = new Sides();
        //                Prana.Admin.BLL.Side side = new Prana.Admin.BLL.Side();

        //                for(int i=0, count = checkedlstSide.CheckedItems.Count; i<count; i++)
        //                {
        //                    sideID = int.Parse((((Prana.Admin.BLL.Side)checkedlstSide.CheckedItems[i]).SideID.ToString()));
        //                    sides.Add(new Prana.Admin.BLL.Side(sideID, "", ""));
        //                }
        //                if (cmbFollowCompliance.Text == "No")
        //                {
        //                    CounterPartyManager.SaveCVAUECSide(cvAUECID, sides);
        //                }

        //                //counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
        //                result = 1;
        //            }
        //            return result;
        //        }

        //        public void SetCounterPartyVenueAcceptedOrderTypes(CounterPartyVenue counterPartyVenue)
        //        {
        //            //These loops are used to unslect the previous seleced values.
        //            for ( int j=0; j< checkedlstExecutionInstr.Items.Count ; j++)
        //            {
        //                checkedlstExecutionInstr.SetItemChecked(j,false);
        //            }
        //            for (int j=0; j< checkedlstSide.Items.Count ; j++)
        //            {
        //                checkedlstSide.SetItemChecked(j,false);
        //            }
        //            for (int j=0; j< checkedlstOrderTypes.Items.Count ; j++)
        //            {
        //                checkedlstOrderTypes.SetItemChecked(j,false);
        //            }
        //            for (int j=0; j< checkedlstTIF.Items.Count ; j++)
        //            {
        //                checkedlstTIF.SetItemChecked(j,false);
        //            }
        //            for (int j=0; j< checkedlstHandlingInstructions.Items.Count ; j++)
        //            {
        //                checkedlstHandlingInstructions.SetItemChecked(j,false);
        //            }


        //            if(counterPartyVenue != null)
        //            {
        //                string SideID = counterPartyVenue.SideID.ToString();
        //                char[] sep = {','};
        //                Array a = SideID.Split(sep);

        //                lstSide.SelectedIndex = -1;

        //                if(counterPartyVenue.SideID.ToString() != "")
        //                {
        //                    for(int i=1;i<(a.Length-1);i++)
        //                    {
        //                        lstSide.SelectedValue = int.Parse(a.GetValue(i).ToString());
        //                        for (int j=0; j< checkedlstSide.Items.Count ; j++)
        //                        {
        //                            if (((Side)checkedlstSide.Items[j]).SideID == int.Parse(a.GetValue(i).ToString()))
        //                            {
        //                                checkedlstSide.SetItemChecked(j,true);
        //                            }
        //                        }
        //                    }
        //                }
        //                if(lstSide.SelectedIndices.Count == 0)
        //                {
        //                    lstSide.SelectedValue = int.MinValue;
        //                }

        //                string OrderTypesID = counterPartyVenue.OrderTypesID.ToString();
        //                Array b = OrderTypesID.Split(sep);

        //                lstOrderTypes.SelectedIndex = -1;
        //                if(counterPartyVenue.OrderTypesID.ToString() != "")
        //                {

        //                    for(int i=1;i<(b.Length-1);i++)
        //                    {
        //                        lstOrderTypes.SelectedValue = int.Parse(b.GetValue(i).ToString());
        //                        for (int j=0; j< checkedlstOrderTypes.Items.Count ; j++)
        //                        {
        //                            if (((OrderType)checkedlstOrderTypes.Items[j]).OrderTypesID == int.Parse(b.GetValue(i).ToString()))
        //                            {
        //                                checkedlstOrderTypes.SetItemChecked(j,true);
        //                            }
        //                        }
        //                    }
        //                }
        //                if(lstOrderTypes.SelectedIndices.Count == 0)
        //                {
        //                    lstOrderTypes.SelectedValue = int.MinValue;
        //                }

        //                string TimeInForceID = counterPartyVenue.TimeInForceID.ToString();
        //                Array c = TimeInForceID.Split(sep);

        //                lstTIF.SelectedIndex = -1;
        //                if(counterPartyVenue.TimeInForceID.ToString() != "")
        //                {

        //                    for(int i=1;i<(c.Length-1);i++)
        //                    {
        //                        lstTIF.SelectedValue = int.Parse(c.GetValue(i).ToString());
        //                        for (int j=0; j< checkedlstTIF.Items.Count ; j++)
        //                        {
        //                            if (((TimeInForce)checkedlstTIF.Items[j]).TimeInForceID == int.Parse(c.GetValue(i).ToString()))
        //                            {
        //                                checkedlstTIF.SetItemChecked(j,true);
        //                            }
        //                        }
        //                    }
        //                }
        //                if(lstTIF.SelectedIndices.Count == 0)
        //                {
        //                    lstTIF.SelectedValue = int.MinValue;
        //                }

        //                string HandlingInstructions = counterPartyVenue.HandlingInstructionsID.ToString();
        //                Array d = HandlingInstructions.Split(sep);

        //                lstHandlingInstructions.SelectedIndex = -1;
        //                if(counterPartyVenue.HandlingInstructionsID.ToString() != "")
        //                {

        //                    for(int i=1;i<(d.Length-1);i++)
        //                    {
        //                        lstHandlingInstructions.SelectedValue = int.Parse(d.GetValue(i).ToString());
        //                        for (int j=0; j< checkedlstHandlingInstructions.Items.Count ; j++)
        //                        {
        //                            if (((HandlingInstruction)checkedlstHandlingInstructions.Items[j]).HandlingInstructionID== int.Parse(d.GetValue(i).ToString()))
        //                            {
        //                                checkedlstHandlingInstructions.SetItemChecked(j,true);
        //                            }
        //                        }
        //                    }
        //                }
        //                if(lstHandlingInstructions.SelectedIndices.Count == 0)
        //                {
        //                    lstHandlingInstructions.SelectedValue = int.MinValue;
        //                }
        //                string ExecutionInstructions = counterPartyVenue.ExecutionInstructionsID.ToString();
        //                Array e = ExecutionInstructions.Split(sep);

        //                lstExecutionInstr.SelectedIndex = -1;
        //                if(counterPartyVenue.ExecutionInstructionsID.ToString() != "")
        //                {

        //                    for(int i=1;i<(e.Length-1);i++)
        //                    {

        //                        lstExecutionInstr.SelectedValue = int.Parse(e.GetValue(i).ToString());

        //                            for (int j=0; j< checkedlstExecutionInstr.Items.Count ; j++)
        //                            {

        //                                if (((ExecutionInstruction)checkedlstExecutionInstr.Items[j]).ExecutionInstructionsID== int.Parse(e.GetValue(i).ToString()))
        //                                {
        //                                    checkedlstExecutionInstr.SetItemChecked(j,true);
        //                                }
        //                            }

        //                    }
        //                }
        //                if(lstExecutionInstr.SelectedIndices.Count == 0)
        //                {
        //                    lstExecutionInstr.SelectedValue = int.MinValue;
        //                }

        //                //cmbAdvancedOrders.SelectedValue = counterPartyVenue.AdvancedOrdersID;
        //            }
        //        }

        //        /// <summary>
        //        /// This method binds all the auec's related to the selected counterpartyvenue.
        //        /// </summary>
        //        private void BindCVAUEC()
        //        {
        //            //GetCVAUECs method fetches the existing auecs from the database.
        //            AUECs aUECs = CounterPartyManager.GetCVAUECs(_objCounterPartyVenue.CounterPartyVenueID);

        //            System.Data.DataTable dtauec = new System.Data.DataTable();
        //            dtauec.Columns.Add("Data");
        //            dtauec.Columns.Add("Value");
        //            object[] row = new object[2]; 
        //            row[0] = C_COMBO_SELECT;
        //            row[1] = int.MinValue;
        //            dtauec.Rows.Add(row);

        ////			if (aUECs.Count > 0 )
        ////			{
        //                foreach(AUEC auec in aUECs)
        //                {
        //                    //Currency currency = new Currency();
        //                    //string Data = auec.Asset.Name.ToString() + " : " + auec.Exchange.Name.ToString() + " : " + auec.Currency.CurrencyName.ToString();
        //                    //currency = AUECManager.GetCurrency(auec.Exchange.Currency);
        //                    //SK 20061009 REmoved Compliance Class
        //                    //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
        //                    //
        //                    //string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();
        //                    string Data = auec.AUECString;
        //                    int Value = auec.AUECID;

        //                    row[0] = Data;
        //                    row[1] = Value;
        //                    dtauec.Rows.Add(row);
        //                }
        //                cmbAUEC.DataSource = null;
        //                cmbAUEC.DataSource = dtauec;
        //                cmbAUEC.DisplayMember = "Data";
        //                cmbAUEC.ValueMember = "Value";
        //                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbAUEC.DisplayLayout.Bands[0].Columns)
        //                {
        //                    if (column.Key.Equals("Data"))
        //                    {
        //                        column.Hidden = false;
        //                    }
        //                    else
        //                    {
        //                        column.Hidden = true;
        //                    }
        //                }
        //                cmbAUEC.DisplayLayout.Bands[0].ColHeadersVisible = false;

        //            if(aUECs.Count > 0)
        //            {
        //                cmbAUEC.Value = int.Parse(((AUEC)aUECs[0]).AUECID.ToString());
        //            }
        //            else
        //            {
        //                cmbAUEC.Value = int.MinValue;
        //            }
        ////			}

        //        }

        //        /// <summary>
        //        /// This method fills the datable with a default row and two boolean rows and then binds the datatable to 
        //        /// the compliance combo.
        //        /// </summary>
        //        private void BindFollowCompliance()
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            object[]  row = new object[2];
        //            dt.Columns.Add("Data");
        //            dt.Columns.Add("Value");

        //            row[0] = C_COMBO_SELECT;
        //            row[1] = int.MinValue;
        //            dt.Rows.Add(row);

        //            row = new object[2]; 
        //            row[0] = "Yes";
        //            row[1] = "1";
        //            dt.Rows.Add(row);
        //            row[0] = "No";
        //            row[1] = "0";
        //            dt.Rows.Add(row);
        //            cmbFollowCompliance.DataSource = null;
        //            cmbFollowCompliance.DataSource = dt;
        //            cmbFollowCompliance.DisplayMember = "Data";
        //            cmbFollowCompliance.ValueMember = "Value";
        //            cmbFollowCompliance.Value = int.MinValue;
        //            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbFollowCompliance.DisplayLayout.Bands[0].Columns)
        //            {
        //                if (column.Key.Equals("Data"))
        //                {
        //                    column.Hidden = false;
        //                }
        //                else
        //                {
        //                    column.Hidden = true;
        //                }
        //            }
        //            cmbFollowCompliance.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //        }

        //        /// <summary>
        //        /// This method fills the datable with a default row and two boolean rows and then binds the datatable to 
        //        /// the ShortSellConfirmation combo.
        //        /// </summary>
        //        private void BindShortSellConfirmation()
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            object[]  row = new object[2];
        //            dt.Columns.Add("Data");
        //            dt.Columns.Add("Value");

        //            row[0] = C_COMBO_SELECT;
        //            row[1] = int.MinValue;
        //            dt.Rows.Add(row);

        //            row = new object[2]; 
        //            row[0] = "Yes";
        //            row[1] = "1";
        //            dt.Rows.Add(row);
        //            row[0] = "No";
        //            row[1] = "0";
        //            dt.Rows.Add(row);
        //            cmbShortSellConfirmation.DataSource = null;
        //            cmbShortSellConfirmation.DataSource = dt;
        //            cmbShortSellConfirmation.DisplayMember = "Data";
        //            cmbShortSellConfirmation.ValueMember = "Value";
        //            cmbShortSellConfirmation.Value = int.MinValue;
        //            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbShortSellConfirmation.DisplayLayout.Bands[0].Columns)
        //            {
        //                if (column.Key.Equals("Data"))
        //                {
        //                    column.Hidden = false;
        //                }
        //                else
        //                {
        //                    column.Hidden = true;
        //                }
        //            }
        //            cmbShortSellConfirmation.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //        }

        //        /// <summary>
        //        /// This method binds the listbox with all the available sides in the database.
        //        /// </summary>
        //        private void BindSide()
        //        {
        //            Sides sides = OrderManager.GetSides();
        //            if (sides.Count > 0 )
        //            {
        //                //sides.Insert(0, new Side(int.MinValue, C_COMBO_SELECT));
        //                lstSide.DataSource = sides;	
        //                checkedlstSide.DataSource = sides;
        //                lstSide.DisplayMember = "Name";
        //                lstSide.ValueMember = "SideID";
        //                checkedlstSide.DisplayMember = "Name";
        //                checkedlstSide.ValueMember = "SideID";
        //            }
        //        }


        //        private void BindAdvancedOrders()
        //        {
        //            AdvancedOrders advancedOrders = OrderManager.GetAdvancedOrders();
        //            if (advancedOrders.Count > 0 )
        //            {
        //                advancedOrders.Insert(0, new AdvancedOrder(int.MinValue, C_COMBO_SELECT));
        //                cmbAdvancedOrders.DataSource = null;
        //                cmbAdvancedOrders.DataSource = advancedOrders;				
        //                cmbAdvancedOrders.DisplayMember = "AdvancedOrders";
        //                cmbAdvancedOrders.ValueMember = "AdvancedOrdersID";
        //            }
        //        }

        //        /// <summary>
        //        /// This method binds the existing <see cref="Identifiers"/> in the ComboBox control by assigning the 
        //        /// identifiers object to its datasource property.
        //        /// </summary>
        //        private void BindIdentifiers()
        //        {
        //            //GetIdentifiers method fetches the existing symbols from the database.
        //            Identifiers identifiers = AUECManager.GetIdentifiers();
        //            //Checking the object for the null value before assigning it to the combo box.
        ////			if (identifiers.Count > 0 )
        ////			{
        //                //Inserting the - Select - option in the Combo Box at the top.
        //                identifiers.Insert(0, new Prana.Admin.BLL.Identifier(int.MinValue, C_COMBO_SELECT));
        //                cmbIdentifier.DataSource = null;
        //                cmbIdentifier.DataSource = identifiers;				
        //                cmbIdentifier.DisplayMember = "IdentifierName";
        //                cmbIdentifier.ValueMember = "IdentifierID";
        //                cmbIdentifier.Value = int.MinValue;
        ////			}
        //                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbIdentifier.DisplayLayout.Bands[0].Columns)
        //                {
        //                    if (column.Key.Equals("IdentifierName"))
        //                    {
        //                        column.Hidden = false;
        //                    }
        //                    else
        //                    {
        //                        column.Hidden = true;
        //                    }
        //                }
        //                cmbIdentifier.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //        }

        //        private void CounterPartyVenueAcceptedOrderTypes_Load(object sender, System.EventArgs e)
        //        {
        //            //BindCVAUEC();
        //            //BindIdentifiers();
        //            //BindShortSellConfirmation();
        //            //BindSide();
        //            //BindOrderTypes();
        //            //BindTimeInForce();
        //            //BindHandlingInstructions();
        //            //BindExecutionInstructions();
        //            //BindFollowCompliance();
        ////			//BindAdvancedOrders();
        //        }

        //        //private int _counterPartyVenueID = int.MinValue;
        //        public int CounterPartyVenueID 
        //        {
        //            set
        //            {
        //                //_counterPartyVenueID = value;
        //            }
        //        }

        //        public void SetCVAUEC()
        //        {
        //            BindCVAUEC();
        //        }

        //        private void lstSide_SelectedIndexChanged(object sender, System.EventArgs e)
        //        {

        //        }

        //# region Controls Focus Colors

        //        private void cmbAdvancedOrders_GotFocus(object sender, System.EventArgs e)
        //        {
        //            cmbAdvancedOrders.Appearance.BackColor = Color.LemonChiffon;
        //        }
        //        private void cmbAdvancedOrders_LostFocus(object sender, System.EventArgs e)
        //        {
        //            cmbAdvancedOrders.Appearance.BackColor = Color.White;
        //        }
        //        private void cmbAlgos_GotFocus(object sender, System.EventArgs e)
        //        {
        //            cmbAlgos.Appearance.BackColor = Color.LemonChiffon;
        //        }
        //        private void cmbAlgos_LostFocus(object sender, System.EventArgs e)
        //        {
        //            cmbAlgos.Appearance.BackColor = Color.White;
        //        }
        //        private void lstExecutionInstr_GotFocus(object sender, System.EventArgs e)
        //        {
        //            lstExecutionInstr.BackColor = Color.LemonChiffon;
        //        }
        //        private void lstExecutionInstr_LostFocus(object sender, System.EventArgs e)
        //        {
        //            lstExecutionInstr.BackColor = Color.White;
        //        }
        //        private void lstHandlingInstructions_GotFocus(object sender, System.EventArgs e)
        //        {
        //            lstHandlingInstructions.BackColor = Color.LemonChiffon;
        //        }
        //        private void lstHandlingInstructions_LostFocus(object sender, System.EventArgs e)
        //        {
        //            lstHandlingInstructions.BackColor = Color.White;
        //        }
        //        private void lstOrderTypes_GotFocus(object sender, System.EventArgs e)
        //        {
        //            lstOrderTypes.BackColor = Color.LemonChiffon;
        //        }
        //        private void lstOrderTypes_LostFocus(object sender, System.EventArgs e)
        //        {
        //            lstOrderTypes.BackColor = Color.White;
        //        }
        //        private void lstSide_GotFocus(object sender, System.EventArgs e)
        //        {
        //            lstSide.BackColor = Color.LemonChiffon;
        //        }
        //        private void lstSide_LostFocus(object sender, System.EventArgs e)
        //        {
        //            lstSide.BackColor = Color.White;
        //        }
        //        private void lstTIF_GotFocus(object sender, System.EventArgs e)
        //        {
        //            lstTIF.BackColor = Color.LemonChiffon;
        //        }
        //        private void lstTIF_LostFocus(object sender, System.EventArgs e)
        //        {
        //            lstTIF.BackColor = Color.White;
        //        }
        //        private void txtForeignID_GotFocus(object sender, System.EventArgs e)
        //        {
        //            txtForeignID.BackColor = Color.LemonChiffon;
        //        }
        //        private void txtForeignID_LostFocus(object sender, System.EventArgs e)
        //        {
        //            txtForeignID.BackColor = Color.White;
        //        }
        //        private void cmbAUEC_GotFocus(object sender, System.EventArgs e)
        //        {
        //            cmbAUEC.Appearance.BackColor = Color.LemonChiffon;
        //        }
        //        private void cmbAUEC_LostFocus(object sender, System.EventArgs e)
        //        {
        //            cmbAUEC.Appearance.BackColor = Color.White;
        //        }
        //        private void cmbFollowCompliance_GotFocus(object sender, System.EventArgs e)
        //        {
        //            cmbFollowCompliance.Appearance.BackColor = Color.LemonChiffon;
        //        }
        //        private void cmbFollowCompliance_LostFocus(object sender, System.EventArgs e)
        //        {
        //            cmbFollowCompliance.Appearance.BackColor = Color.White;
        //        }
        //        private void cmbShortSellConfirmation_GotFocus(object sender, System.EventArgs e)
        //        {
        //            cmbShortSellConfirmation.Appearance.BackColor = Color.LemonChiffon;
        //        }
        //        private void cmbShortSellConfirmation_LostFocus(object sender, System.EventArgs e)
        //        {
        //            cmbShortSellConfirmation.Appearance.BackColor = Color.White;
        //        }
        //        private void cmbIdentifier_GotFocus(object sender, System.EventArgs e)
        //        {
        //            cmbIdentifier.Appearance.BackColor = Color.LemonChiffon;
        //        }
        //        private void cmbIdentifier_LostFocus(object sender, System.EventArgs e)
        //        {
        //            cmbIdentifier.Appearance.BackColor = Color.White;
        //        }

        //        #endregion

        //        private void cmbCompliance_Enter(object sender, System.EventArgs e)
        //        {

        //        }

        //        public bool _followCompliance = false;
        //        private void cmbFollowCompliance_ValueChanged(object sender, System.EventArgs e)
        //        {
        //            //If the selected AUEC follow the compliance, then fill the AUEC details in the corresponding
        //            //controls in the CV Controls.
        //            if(cmbFollowCompliance.Value != null)
        //            {
        //                // to come out from the unhandled exception
        //               // if  AUEC type is not selected and Follow Compliance is selected
        //                if (int.Parse(cmbAUEC.Value.ToString()) == int.MinValue  && cmbFollowCompliance.Text == "Yes")
        //                {
        //                    return;
        //                }

        //                if(cmbFollowCompliance.Text == "Yes")
        //                {
        //                    _followCompliance = true;

        //                    int cvAUECID = int.Parse(cmbAUEC.Value.ToString());
        //                    //SK 2061009 removed Compliance class
        //                    AUEC auec = AUECManager.GetAUEC(cvAUECID);
        //                    cmbShortSellConfirmation.Value = int.Parse(auec.IsShortSaleConfirmation.ToString());
        //                    cmbIdentifier.Value = int.Parse(auec.IdentifierID.ToString());
        //                    //

        //                    //ToDo Ishan
        //                    Sides auecSides = new Sides();
        //                    auecSides = new Sides();

        //                    OrderTypes auecOrderTypes = new OrderTypes();
        //                    auecOrderTypes = AUECManager.GetAUECOrderTypes(cvAUECID);

        //                    for (int j=0; j< checkedlstSide.Items.Count ; j++)
        //                    {
        //                        checkedlstSide.SetItemChecked(j,false);
        //                    }
        //                    if(auecSides.Count > 0)
        //                    {
        //                        foreach(Prana.Admin.BLL.Side side in auecSides)
        //                        {
        //                            for (int j=0; j< checkedlstSide.Items.Count ; j++)
        //                            {
        //                                if (((Prana.Admin.BLL.Side)checkedlstSide.Items[j]).SideID == int.Parse(side.SideID.ToString()))
        //                                {
        //                                    checkedlstSide.SetItemChecked(j,true);
        //                                }
        //                            }
        //                        }
        //                    }


        //                    cmbShortSellConfirmation.Enabled = false;
        //                    cmbIdentifier.Enabled = false;
        //                    //checkedlstSide.Enabled = false;
        //                }
        //                else
        //                {
        //                    _followCompliance = false; 

        //                    cmbShortSellConfirmation.Enabled = true;
        //                    cmbIdentifier.Enabled = true;
        //                    //checkedlstSide.Enabled = true;
        //                }
        //            }
        //        }

        //        private void cmbAUEC_ValueChanged(object sender, System.EventArgs e)
        //        {
        //            int counterPartyVenueID = int.Parse(_objCounterPartyVenue.CounterPartyVenueID.ToString());
        //            if(cmbAUEC.Value != null)
        //            {                
        //                    int aUECID = int.Parse(cmbAUEC.Value.ToString());
        //                    CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCVAUECDetails(counterPartyVenueID, aUECID);
        //                    int cvAUECID = int.Parse(counterPartyVenue.CVAUECID.ToString());
        //                    SetCVAUECCompliance(cvAUECID);                 
        //            }
        //        }     

        //        private void SetCVAUECCompliance(int cvAUECID)
        //        {                     
        //                CounterPartyVenue cvAUECCounterPartyVenue = CounterPartyManager.GetCVAUECCompliance(cvAUECID);
        //                cmbFollowCompliance.Value = int.Parse(cvAUECCounterPartyVenue.FollowCompliance.ToString());
        //                cmbShortSellConfirmation.Value = int.Parse(cvAUECCounterPartyVenue.ShortSellConfirmation.ToString());
        //                cmbIdentifier.Value = int.Parse(cvAUECCounterPartyVenue.IdentifierID.ToString());
        //                txtForeignID.Text = cvAUECCounterPartyVenue.ForeignID.ToString();

        //                Sides cvAUECsides=null;
        //                OrderTypes cvAUECOrderTypes=null;
        //                int intAUECID = int.Parse(cmbAUEC.Value.ToString());

        //                if (cmbFollowCompliance.Text == "Yes")
        //                {
        //                    //ToDo Ishan
        //                     cvAUECsides = new Sides();
        //                   cvAUECOrderTypes = AUECManager.GetAUECOrderTypes(intAUECID);
        //                }
        //                else
        //                {
        //                     cvAUECsides = CounterPartyManager.GetCVAUECSides(cvAUECID);
        //                    cvAUECOrderTypes = CounterPartyManager.GetCVAUECOrderTypes(cvAUECID);
        //                }
        //                TimeInForces cvAUECTimeInForces = CounterPartyManager.GetCVAUECTimeInForce(cvAUECID);
        //                HandlingInstructions cvAUECHandlingInstructions = CounterPartyManager.GetCVAUECHandlingInstructions(cvAUECID);
        //                ExecutionInstructions cvAUECExecutionInstructions = CounterPartyManager.GetCVAUECExecutionInstructions(cvAUECID);


        //            for (int j=0; j< checkedlstSide.Items.Count ; j++)
        //            {
        //                checkedlstSide.SetItemChecked(j,false);
        //            }
        //            if(cvAUECsides.Count > 0)
        //            {
        //                foreach(Prana.Admin.BLL.Side side in cvAUECsides)
        //                {
        //                    for (int j=0; j< checkedlstSide.Items.Count ; j++)
        //                    {
        //                        if (((Prana.Admin.BLL.Side)checkedlstSide.Items[j]).SideID == int.Parse(side.SideID.ToString()))
        //                        {
        //                            checkedlstSide.SetItemChecked(j,true);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        private void checkedlstSide_SelectedValueChanged(object sender, System.EventArgs e)
        //        {
        //            if(_followCompliance == true)
        //            {
        //                if(cSide == CheckState.Checked)
        //                {
        //                    if(checkSide != true)
        //                    {
        //                        checkedlstSide.SetItemChecked(currentIndexSide, false);
        //                        checkSide = true;
        //                    }
        //                }
        //                else
        //                {
        //                    if(checkSide != true)
        //                    {
        //                        checkedlstSide.SetItemChecked(currentIndexSide, true); 
        //                        checkSide = true;
        //                    }
        //                }
        //            }
        //        }

        //        public System.Windows.Forms.CheckState cSide = new CheckState();
        //        public int currentIndexSide = 0;
        //        bool checkSide = false;
        //        private int _currentSideSelectedIndex = 0;
        //        private void checkedlstSide_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        //        {
        //            //System.Windows.Forms.CheckState cS = new CheckState();
        //            cSide = e.NewValue;
        //            currentIndexSide = e.Index;
        //            checkSide = false;

        ////			MessageBox.Show("OldValue = " + oldvalue + "NewValue = " + newValue);
        //            _currentSideSelectedIndex = e.Index;
        //        }

        //        private void checkedlstSide_DoubleClick(object sender, System.EventArgs e)
        //        {
        //            if(cSide == CheckState.Checked)
        //            {
        //                checkedlstSide.SetItemChecked(currentIndexSide, false);
        //                //checkedlstSide.Items[currentIndex]. 
        //            }
        //            else
        //            {
        //                checkedlstSide.SetItemChecked(currentIndexSide, true); 
        //            }
        //        }


        //        private void checkedlstOrderTypes_SelectedValueChanged(object sender, System.EventArgs e)
        //        {
        //            if(_followCompliance == true)
        //            {
        //                if(cOrderType == CheckState.Checked)
        //                {
        //                    if(checkOrderType != true)
        //                    {
        //                        checkedlstOrderTypes.SetItemChecked(currentIndexOrderTypes, false);
        //                        checkOrderType = true;
        //                    }
        //                }
        //                else
        //                {
        //                    if(checkOrderType != true)
        //                    {
        //                        checkedlstOrderTypes.SetItemChecked(currentIndexOrderTypes, true); 
        //                        checkOrderType = true;
        //                    }
        //                }
        //            }
        //        }

        //        public System.Windows.Forms.CheckState cOrderType = new CheckState();
        //        public int currentIndexOrderTypes = 0;
        //        bool checkOrderType = false;
        //        private int _currentOrderTypesSelectedIndex = 0;
        //        private void checkedlstOrderTypes_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        //        {
        //            cOrderType = e.NewValue;
        //            currentIndexOrderTypes = e.Index;
        //            checkOrderType = false;
        //            _currentOrderTypesSelectedIndex = e.Index;
        //        }

        //        private void checkedlstOrderTypes_DoubleClick(object sender, System.EventArgs e)
        //        {
        //            if(cOrderType == CheckState.Checked)
        //            {
        //                checkedlstOrderTypes.SetItemChecked(currentIndexOrderTypes, false);
        //            }
        //            else
        //            {
        //                checkedlstOrderTypes.SetItemChecked(currentIndexOrderTypes, true); 
        //            }
        //        }

        //        private void checkedlstSide_KeyUp(object sender, KeyEventArgs e)
        //        {
        //            if (_followCompliance == true)
        //            {
        //                if (cSide == CheckState.Checked)
        //                {
        //                    if (checkSide != true)
        //                    {
        //                        checkedlstSide.SetItemChecked(currentIndexSide, false);
        //                        checkSide = true;
        //                    }
        //                }
        //                else
        //                {
        //                    if (checkSide != true)
        //                    {
        //                        checkedlstSide.SetItemChecked(currentIndexSide, true);
        //                        checkSide = true;
        //                    }
        //                }
        //            }
        //        }

        //        private void checkedlstOrderTypes_KeyUp(object sender, KeyEventArgs e)
        //        {
        //            if (_followCompliance == true)
        //            {
        //                if (cOrderType == CheckState.Checked)
        //                {
        //                    if (checkOrderType != true)
        //                    {
        //                        checkedlstOrderTypes.SetItemChecked(currentIndexOrderTypes, false);
        //                        checkOrderType = true;
        //                    }
        //                }
        //                else
        //                {
        //                    if (checkOrderType != true)
        //                    {
        //                        checkedlstOrderTypes.SetItemChecked(currentIndexOrderTypes, true);
        //                        checkOrderType = true;
        //                    }
        //                }
        //            }
        //        }

        //        private void checkedlstSide_SelectedIndexChanged(object sender, EventArgs e)
        //        {
        //            checkedlstSide.SelectedIndex = _currentSideSelectedIndex;
        //        }

        //        private void checkedlstOrderTypes_SelectedIndexChanged(object sender, EventArgs e)
        //        {
        //            checkedlstOrderTypes.SelectedIndex = _currentOrderTypesSelectedIndex;
        //        }



        //    }
    }
}
