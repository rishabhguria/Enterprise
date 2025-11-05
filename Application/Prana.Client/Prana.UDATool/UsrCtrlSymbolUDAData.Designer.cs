using Prana.Global;
namespace Prana.UDATool
{
    partial class UsrCtrlSymbolUDAData
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
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this.grdSymbolData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnScreenShot = SnapShotManager.GetInstance().ultraButton;
            this.btnExcelExport = new Infragistics.Win.Misc.UltraButton();
            this.lblView = new System.Windows.Forms.Label();
            this.rdobtnCurrent = new System.Windows.Forms.RadioButton();
            this.rdobtnHistorical = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolData)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdSymbolData
            // 
            this.grdSymbolData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdSymbolData.DisplayLayout.Appearance = appearance1;
            this.grdSymbolData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdSymbolData.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSymbolData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSymbolData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSymbolData.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSymbolData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdSymbolData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSymbolData.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdSymbolData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSymbolData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdSymbolData.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdSymbolData.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdSymbolData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdSymbolData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdSymbolData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdSymbolData.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdSymbolData.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdSymbolData.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSymbolData.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdSymbolData.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdSymbolData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSymbolData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.ForeColor = System.Drawing.Color.White;
            this.grdSymbolData.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdSymbolData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdSymbolData.DisplayLayout.Override.SortComparisonType = Infragistics.Win.UltraWinGrid.SortComparisonType.CaseInsensitive;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdSymbolData.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdSymbolData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSymbolData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSymbolData.Location = new System.Drawing.Point(0, 31);
            this.grdSymbolData.Name = "grdSymbolData";
            this.grdSymbolData.Size = new System.Drawing.Size(696, 323);
            this.grdSymbolData.TabIndex = 2;
            this.grdSymbolData.Text = "ultraGrid1";
            this.grdSymbolData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSymbolData_InitializeLayout);
            this.grdSymbolData.CellListSelect += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSymbolData_CellListSelect);
            this.grdSymbolData.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSymbolData_CellChange);
            // 
            // btnGetData
            // 
            appearance13.BackColor = System.Drawing.Color.AliceBlue;
            appearance13.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.btnGetData.Appearance = appearance13;
            this.btnGetData.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetData.Location = new System.Drawing.Point(237, 3);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(86, 23);
            this.btnGetData.TabIndex = 8;
            this.btnGetData.Text = "GetData";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // btnSave
            // 
            appearance14.BackColor = System.Drawing.Color.AliceBlue;
            appearance14.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.btnSave.Appearance = appearance14;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(333, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnScreenShot
            //            
            this.btnScreenShot.Appearance = appearance14;
            this.btnScreenShot.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScreenShot.Location = new System.Drawing.Point(490, 3);
            this.btnScreenShot.Name = "btnScreenShot";
            this.btnScreenShot.Size = new System.Drawing.Size(82, 23);
            this.btnScreenShot.Click += new System.EventHandler(this.btnScreenShot_Click);
            // 
            // btnExcelExport
            // 
            appearance15.BackColor = System.Drawing.Color.AliceBlue;
            appearance15.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.btnExcelExport.Appearance = appearance15;
            this.btnExcelExport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcelExport.Location = new System.Drawing.Point(421, 3);
            this.btnExcelExport.Name = "btnExcelExport";
            this.btnExcelExport.Size = new System.Drawing.Size(65, 23);
            this.btnExcelExport.TabIndex = 10;
            this.btnExcelExport.Text = "Export";
            this.btnExcelExport.Click += new System.EventHandler(this.btnExcelExport_Click);
            // 
            // lblView
            // 
            this.lblView.AutoSize = true;
            this.lblView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblView.Location = new System.Drawing.Point(10, 8);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(87, 13);
            this.lblView.TabIndex = 6;
            this.lblView.Text = "View Symbols:";
            // 
            // rdobtnCurrent
            // 
            this.rdobtnCurrent.AutoSize = true;
            this.rdobtnCurrent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdobtnCurrent.Location = new System.Drawing.Point(100, 6);
            this.rdobtnCurrent.Name = "rdobtnCurrent";
            this.rdobtnCurrent.Size = new System.Drawing.Size(62, 17);
            this.rdobtnCurrent.TabIndex = 7;
            this.rdobtnCurrent.Text = "Current";
            this.rdobtnCurrent.UseVisualStyleBackColor = true;
            this.rdobtnCurrent.Click += new System.EventHandler(this.rdobtnAll_Click);
           
            // 
            // rdobtnHistorical
            // 
            this.rdobtnHistorical.AutoSize = true;
            this.rdobtnHistorical.Checked = true;
            this.rdobtnHistorical.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdobtnHistorical.Location = new System.Drawing.Point(166, 6);
            this.rdobtnHistorical.Name = "rdobtnHistorical";
            this.rdobtnHistorical.Size = new System.Drawing.Size(68, 17);
            this.rdobtnHistorical.TabIndex = 7;
            this.rdobtnHistorical.TabStop = true;
            this.rdobtnHistorical.Text = "Historical";
            this.rdobtnHistorical.UseVisualStyleBackColor = true;
            this.rdobtnHistorical.Click += new System.EventHandler(this.rdobtnAll_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 357);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(696, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(681, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UsrCtrlSymbolUDAData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.rdobtnHistorical);
            this.Controls.Add(this.rdobtnCurrent);
            this.Controls.Add(this.lblView);
            this.Controls.Add(this.btnExcelExport);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnScreenShot);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.grdSymbolData);
            this.Name = "UsrCtrlSymbolUDAData";
            this.Size = new System.Drawing.Size(696, 379);
            this.Load += new System.EventHandler(this.UsrCtrlSymbolUDAData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolData)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdSymbolData;
        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnScreenShot;
        private Infragistics.Win.Misc.UltraButton btnExcelExport;
        private System.Windows.Forms.Label lblView;
        private System.Windows.Forms.RadioButton rdobtnCurrent;
        private System.Windows.Forms.RadioButton rdobtnHistorical;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}
