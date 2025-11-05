namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlIdentifyCash
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cash Re-con Columns", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Classification", 2);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlIdentifyCash));
            this.ctrlSourceName1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSourceName();
            this.grdIdentifyCash = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdIdentifyCash)).BeginInit();
            this.SuspendLayout();
            // 
            // ctrlSourceName1
            // 
            this.ctrlSourceName1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSourceName1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSourceName1.Location = new System.Drawing.Point(3, 0);
            this.ctrlSourceName1.Name = "ctrlSourceName1";
            this.ctrlSourceName1.Size = new System.Drawing.Size(243, 26);
            this.ctrlSourceName1.TabIndex = 0;
            // 
            // grdIdentifyCash
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdIdentifyCash.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.grdIdentifyCash.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdIdentifyCash.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdIdentifyCash.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdIdentifyCash.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdIdentifyCash.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdIdentifyCash.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdIdentifyCash.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdIdentifyCash.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdIdentifyCash.DisplayLayout.MaxColScrollRegions = 1;
            this.grdIdentifyCash.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdIdentifyCash.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdIdentifyCash.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdIdentifyCash.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdIdentifyCash.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdIdentifyCash.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdIdentifyCash.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdIdentifyCash.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdIdentifyCash.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdIdentifyCash.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.grdIdentifyCash.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdIdentifyCash.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdIdentifyCash.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdIdentifyCash.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdIdentifyCash.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdIdentifyCash.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdIdentifyCash.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdIdentifyCash.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdIdentifyCash.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdIdentifyCash.Location = new System.Drawing.Point(1, 28);
            this.grdIdentifyCash.Name = "grdIdentifyCash";
            this.grdIdentifyCash.Size = new System.Drawing.Size(329, 169);
            this.grdIdentifyCash.TabIndex = 1;
            this.grdIdentifyCash.Text = "ultraGrid1";
            // 
            // btnCancel
            // 
            appearance13.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_cancel;
            this.btnCancel.Appearance = appearance13;
            this.btnCancel.ImageSize = new System.Drawing.Size(75, 23);
            this.btnCancel.Location = new System.Drawing.Point(187, 203);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 37;
            // 
            // btnSave
            // 
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            this.btnSave.Appearance = appearance14;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(87, 203);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 36;
            // 
            // CtrlIdentifyCash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdIdentifyCash);
            this.Controls.Add(this.ctrlSourceName1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlIdentifyCash";
            this.Size = new System.Drawing.Size(331, 234);
            ((System.ComponentModel.ISupportInitialize)(this.grdIdentifyCash)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlSourceName ctrlSourceName1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdIdentifyCash;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnSave;


    }
}
