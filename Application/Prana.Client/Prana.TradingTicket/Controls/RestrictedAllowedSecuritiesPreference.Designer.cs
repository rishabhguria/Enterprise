using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.TradingTicket.Controls
{
    partial class RestrictedAllowedSecuritiesPreference
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
                _securityMaster.SecMstrDataSymbolSearcResponse -= _securityMaster_SecMstrDataSymbolSearcResponse; 
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
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.ultraPanelSymbologyType = new Infragistics.Win.Misc.UltraPanel();
            this.radioButtonBloomberg = new System.Windows.Forms.RadioButton();
            this.radioButtonTicker = new System.Windows.Forms.RadioButton();
            this.ImportSecuritiesButton = new Infragistics.Win.Misc.UltraButton();
            this.DeleteSecuritiesButton = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.pranaUltraGrid1 = new PranaUltraGrid();
            this.removeButton = new Infragistics.Win.Misc.UltraButton();
            this.addButton = new Infragistics.Win.Misc.UltraButton();
            this.symbolValue = new Prana.Utilities.UI.UIUtilities.PranaSymbolCtrl();
            this.exportButton = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelSymbologyType.ClientArea.SuspendLayout();
            this.ultraPanelSymbologyType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pranaUltraGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanelSymbologyType
            // 
            // 
            // ultraPanelSymbologyType.ClientArea
            // 
            this.ultraPanelSymbologyType.ClientArea.Controls.Add(this.radioButtonBloomberg);
            this.ultraPanelSymbologyType.ClientArea.Controls.Add(this.radioButtonTicker);
            this.ultraPanelSymbologyType.Location = new System.Drawing.Point(6, 59);
            this.ultraPanelSymbologyType.Name = "ultraPanelSymbologyType";
            this.ultraPanelSymbologyType.Size = new System.Drawing.Size(193, 28);
            this.ultraPanelSymbologyType.TabIndex = 0;
            // 
            // radioButtonBloomberg
            // 
            this.radioButtonBloomberg.AutoSize = true;
            this.radioButtonBloomberg.Location = new System.Drawing.Point(81, 6);
            this.radioButtonBloomberg.Name = "radioButtonBloomberg";
            this.radioButtonBloomberg.Size = new System.Drawing.Size(75, 17);
            this.radioButtonBloomberg.TabIndex = 1;
            this.radioButtonBloomberg.TabStop = true;
            this.radioButtonBloomberg.Text = "Bloomberg";
            this.radioButtonBloomberg.UseVisualStyleBackColor = true;
            // 
            // radioButtonTicker
            // 
            this.radioButtonTicker.AutoSize = true;
            this.radioButtonTicker.Checked = true;
            this.radioButtonTicker.Location = new System.Drawing.Point(3, 6);
            this.radioButtonTicker.Name = "radioButtonTicker";
            this.radioButtonTicker.Size = new System.Drawing.Size(55, 17);
            this.radioButtonTicker.TabIndex = 0;
            this.radioButtonTicker.TabStop = true;
            this.radioButtonTicker.Text = "Ticker";
            this.radioButtonTicker.UseVisualStyleBackColor = true;
            this.radioButtonTicker.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // ImportSecuritiesButton
            // 
            this.ImportSecuritiesButton.Location = new System.Drawing.Point(6, 36);
            this.ImportSecuritiesButton.Name = "ImportSecuritiesButton";
            this.ImportSecuritiesButton.Size = new System.Drawing.Size(133, 23);
            this.ImportSecuritiesButton.TabIndex = 1;
            this.ImportSecuritiesButton.Text = "Import Securities List";
            this.ImportSecuritiesButton.Click += new System.EventHandler(this.ImportSecuritiesButton_Click);
            // 
            // DeleteSecuritiesButton
            // 
            this.DeleteSecuritiesButton.Location = new System.Drawing.Point(155, 36);
            this.DeleteSecuritiesButton.Name = "DeleteSecuritiesButton";
            this.DeleteSecuritiesButton.Size = new System.Drawing.Size(138, 23);
            this.DeleteSecuritiesButton.TabIndex = 2;
            this.DeleteSecuritiesButton.Text = "Delete Securities List";
            this.DeleteSecuritiesButton.Click += new System.EventHandler(this.DeleteSecuritiesButton_Click);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.pranaUltraGrid1);
            this.ultraGroupBox1.Controls.Add(this.removeButton);
            this.ultraGroupBox1.Controls.Add(this.addButton);
            this.ultraGroupBox1.Controls.Add(this.symbolValue);
            this.ultraGroupBox1.Controls.Add(this.exportButton);
            this.ultraGroupBox1.Controls.Add(this.ultraPanelSymbologyType);
            this.ultraGroupBox1.Controls.Add(this.DeleteSecuritiesButton);
            this.ultraGroupBox1.Controls.Add(this.ImportSecuritiesButton);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(689, 463);
            this.ultraGroupBox1.TabIndex = 3;
            // 
            // pranaUltraGrid1
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.pranaUltraGrid1.DisplayLayout.Appearance = appearance1;
            this.pranaUltraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.pranaUltraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.pranaUltraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.pranaUltraGrid1.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.pranaUltraGrid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.pranaUltraGrid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.pranaUltraGrid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.pranaUltraGrid1.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.pranaUltraGrid1.DisplayLayout.MaxColScrollRegions = 1;
            this.pranaUltraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pranaUltraGrid1.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.pranaUltraGrid1.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.pranaUltraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.pranaUltraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.pranaUltraGrid1.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.pranaUltraGrid1.DisplayLayout.Override.CellAppearance = appearance8;
            this.pranaUltraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.pranaUltraGrid1.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.pranaUltraGrid1.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.pranaUltraGrid1.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.pranaUltraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.pranaUltraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.pranaUltraGrid1.DisplayLayout.Override.RowAppearance = appearance11;
            this.pranaUltraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pranaUltraGrid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.pranaUltraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.pranaUltraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.pranaUltraGrid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.pranaUltraGrid1.Location = new System.Drawing.Point(6, 121);
            this.pranaUltraGrid1.Name = "pranaUltraGrid1";
            this.pranaUltraGrid1.Size = new System.Drawing.Size(353, 370);
            this.pranaUltraGrid1.TabIndex = 8;
            this.pranaUltraGrid1.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(232, 93);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(70, 22);
            this.removeButton.TabIndex = 6;
            this.removeButton.Text = "Remove";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(168, 93);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(58, 22);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // symbolValue
            // 
            this.symbolValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.symbolValue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.symbolValue.Location = new System.Drawing.Point(6, 94);
            this.symbolValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.symbolValue.MaxLength = 32767;
            this.symbolValue.Name = "symbolValue";
            this.symbolValue.PrevSymbolEntered = "";
            this.symbolValue.Size = new System.Drawing.Size(156, 22);
            this.symbolValue.TabIndex = 1;
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(308, 93);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(51, 22);
            this.exportButton.TabIndex = 4;
            this.exportButton.Text = "Export";
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // RestrictedAllowedSecuritiesPreference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "RestrictedAllowedSecuritiesPreference";
            this.Size = new System.Drawing.Size(689, 463);
            this.ultraPanelSymbologyType.ClientArea.ResumeLayout(false);
            this.ultraPanelSymbologyType.ClientArea.PerformLayout();
            this.ultraPanelSymbologyType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pranaUltraGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanelSymbologyType;
        private System.Windows.Forms.RadioButton radioButtonBloomberg;
        private System.Windows.Forms.RadioButton radioButtonTicker;
        private Infragistics.Win.Misc.UltraButton ImportSecuritiesButton;
        private Infragistics.Win.Misc.UltraButton DeleteSecuritiesButton;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraButton exportButton;
        private Prana.Utilities.UI.UIUtilities.PranaSymbolCtrl symbolValue;
        private Infragistics.Win.Misc.UltraButton removeButton;
        private Infragistics.Win.Misc.UltraButton addButton;
        private PranaUltraGrid pranaUltraGrid1;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
    }
}
