namespace Prana.PricingService2UI.FactSet
{
    partial class FactSetMonitoringUI
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
            this.ultraPanelHeader = new Infragistics.Win.Misc.UltraPanel();
            this.ultraButtonSubscribedSymbols = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonConnectionProperties = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelSubscribedSymbols = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGridSubscribedSymbols = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStripSubscribedSymbols = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshFactSetSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraPanelHeader.ClientArea.SuspendLayout();
            this.ultraPanelHeader.SuspendLayout();
            this.ultraPanelSubscribedSymbols.ClientArea.SuspendLayout();
            this.ultraPanelSubscribedSymbols.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSubscribedSymbols)).BeginInit();
            this.contextMenuStripSubscribedSymbols.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanelHeader
            // 
            // 
            // ultraPanelHeader.ClientArea
            // 
            this.ultraPanelHeader.ClientArea.Controls.Add(this.ultraButtonSubscribedSymbols);
            this.ultraPanelHeader.ClientArea.Controls.Add(this.ultraButtonConnectionProperties);
            this.ultraPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelHeader.Name = "ultraPanelHeader";
            this.ultraPanelHeader.Size = new System.Drawing.Size(858, 47);
            this.ultraPanelHeader.TabIndex = 0;
            // 
            // ultraButtonSubscribedSymbols
            // 
            this.ultraButtonSubscribedSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonSubscribedSymbols.Location = new System.Drawing.Point(12, 12);
            this.ultraButtonSubscribedSymbols.Name = "ultraButtonSubscribedSymbols";
            this.ultraButtonSubscribedSymbols.Size = new System.Drawing.Size(153, 23);
            this.ultraButtonSubscribedSymbols.TabIndex = 0;
            this.ultraButtonSubscribedSymbols.Text = "View Subscribed Symbols";
            this.ultraButtonSubscribedSymbols.Click += new System.EventHandler(this.ultraButtonSubscribedSymbols_Click);
            // 
            // ultraButtonConnectionProperties
            // 
            this.ultraButtonConnectionProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonConnectionProperties.Location = new System.Drawing.Point(169, 12);
            this.ultraButtonConnectionProperties.Name = "ultraButtonConnectionProperties";
            this.ultraButtonConnectionProperties.Size = new System.Drawing.Size(126, 23);
            this.ultraButtonConnectionProperties.TabIndex = 1;
            this.ultraButtonConnectionProperties.Text = "Connection Properties";
            this.ultraButtonConnectionProperties.Click += new System.EventHandler(this.ultraButtonConnectionProperties_Click);
            // 
            // ultraPanelSubscribedSymbols
            // 
            // 
            // ultraPanelSubscribedSymbols.ClientArea
            // 
            this.ultraPanelSubscribedSymbols.ClientArea.Controls.Add(this.ultraGridSubscribedSymbols);
            this.ultraPanelSubscribedSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelSubscribedSymbols.Location = new System.Drawing.Point(0, 47);
            this.ultraPanelSubscribedSymbols.Name = "ultraPanelSubscribedSymbols";
            this.ultraPanelSubscribedSymbols.Size = new System.Drawing.Size(858, 204);
            this.ultraPanelSubscribedSymbols.TabIndex = 1;
            // 
            // ultraGridSubscribedSymbols
            // 
            this.ultraGridSubscribedSymbols.ContextMenuStrip = this.contextMenuStripSubscribedSymbols;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridSubscribedSymbols.DisplayLayout.Appearance = appearance1;
            this.ultraGridSubscribedSymbols.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridSubscribedSymbols.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridSubscribedSymbols.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridSubscribedSymbols.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridSubscribedSymbols.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraGridSubscribedSymbols.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridSubscribedSymbols.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraGridSubscribedSymbols.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridSubscribedSymbols.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridSubscribedSymbols.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraGridSubscribedSymbols.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridSubscribedSymbols.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridSubscribedSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridSubscribedSymbols.Location = new System.Drawing.Point(0, 0);
            this.ultraGridSubscribedSymbols.Name = "ultraGridSubscribedSymbols";
            this.ultraGridSubscribedSymbols.Size = new System.Drawing.Size(858, 204);
            this.ultraGridSubscribedSymbols.TabIndex = 0;
            this.ultraGridSubscribedSymbols.Text = "ultraGrid1";
            this.ultraGridSubscribedSymbols.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ultraGridSubscribedSymbols_MouseDown);
            // 
            // contextMenuStripSubscribedSymbols
            // 
            this.contextMenuStripSubscribedSymbols.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshFactSetSymbolToolStripMenuItem,
            this.copyToClipboardToolStripMenuItem});
            this.contextMenuStripSubscribedSymbols.Name = "contextMenuStripSubscribedSymbols";
            this.contextMenuStripSubscribedSymbols.Size = new System.Drawing.Size(245, 48);
            // 
            // refreshFactSetSymbolToolStripMenuItem
            // 
            this.refreshFactSetSymbolToolStripMenuItem.Name = "refreshFactSetSymbolToolStripMenuItem";
            this.refreshFactSetSymbolToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.refreshFactSetSymbolToolStripMenuItem.Text = "Refresh Selected FactSet Symbol";
            this.refreshFactSetSymbolToolStripMenuItem.Click += new System.EventHandler(this.refreshFactSetSymbolToolStripMenuItem_Click);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 251);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(858, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // FactSetMonitoringUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 273);
            this.Controls.Add(this.ultraPanelSubscribedSymbols);
            this.Controls.Add(this.ultraPanelHeader);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FactSetMonitoringUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FactSet Monitoring UI";
            this.ultraPanelHeader.ClientArea.ResumeLayout(false);
            this.ultraPanelHeader.ResumeLayout(false);
            this.ultraPanelSubscribedSymbols.ClientArea.ResumeLayout(false);
            this.ultraPanelSubscribedSymbols.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSubscribedSymbols)).EndInit();
            this.contextMenuStripSubscribedSymbols.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanelHeader;
        private Infragistics.Win.Misc.UltraButton ultraButtonSubscribedSymbols;
        private Infragistics.Win.Misc.UltraButton ultraButtonConnectionProperties;
        private Infragistics.Win.Misc.UltraPanel ultraPanelSubscribedSymbols;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridSubscribedSymbols;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSubscribedSymbols;
        private System.Windows.Forms.ToolStripMenuItem refreshFactSetSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}