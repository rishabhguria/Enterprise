namespace Prana.Import.Controls
{
    partial class CtrlArchivedData
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_frmUploadedFile != null)
                {
                    _frmUploadedFile.Dispose();
                }
                if (_frmReport != null)
                {
                    _frmReport.Dispose();
                }
                if (_grdArchivalReport != null)
                {
                    _grdArchivalReport.Dispose();
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grdArchiveDashBoard = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdArchiveDashBoard)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnDelete);
            this.splitContainer1.Panel1.Controls.Add(this.btnView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdArchiveDashBoard);
            this.splitContainer1.Size = new System.Drawing.Size(742, 331);
            this.splitContainer1.SplitterDistance = 39;
            this.splitContainer1.TabIndex = 31;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(160, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(130, 25);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.Text = "Delete Selected Items";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(7, 8);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(127, 25);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "View Archived Data";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // grdArchiveDashBoard
            // 
            this.grdArchiveDashBoard.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdArchiveDashBoard.DisplayLayout.Appearance = appearance1;
            this.grdArchiveDashBoard.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdArchiveDashBoard.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdArchiveDashBoard.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdArchiveDashBoard.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdArchiveDashBoard.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdArchiveDashBoard.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdArchiveDashBoard.DisplayLayout.MaxColScrollRegions = 1;
            this.grdArchiveDashBoard.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdArchiveDashBoard.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdArchiveDashBoard.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdArchiveDashBoard.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdArchiveDashBoard.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdArchiveDashBoard.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdArchiveDashBoard.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdArchiveDashBoard.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdArchiveDashBoard.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdArchiveDashBoard.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdArchiveDashBoard.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdArchiveDashBoard.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdArchiveDashBoard.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.FontData.BoldAsString = "False";
            this.grdArchiveDashBoard.DisplayLayout.Override.RowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdArchiveDashBoard.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdArchiveDashBoard.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdArchiveDashBoard.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdArchiveDashBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdArchiveDashBoard.Location = new System.Drawing.Point(0, 0);
            this.grdArchiveDashBoard.Name = "grdArchiveDashBoard";
            this.grdArchiveDashBoard.Size = new System.Drawing.Size(742, 288);
            this.grdArchiveDashBoard.TabIndex = 31;
            this.grdArchiveDashBoard.Text = "ultraGrid1";
            this.grdArchiveDashBoard.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdArchiveDashBoard_InitializeLayout);
            this.grdArchiveDashBoard.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdArchiveDashBoard_InitializeRow);
            this.grdArchiveDashBoard.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdArchiveDashBoard_ClickCellButton);
            this.grdArchiveDashBoard.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdArchiveDashBoard_BeforeRowsDeleted);
            this.grdArchiveDashBoard.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdArchiveDashBoard_BeforeCustomRowFilterDialog);
            this.grdArchiveDashBoard.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdArchiveDashBoard_BeforeColumnChooserDisplayed);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // CtrlArchivedData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CtrlArchivedData";
            this.Size = new System.Drawing.Size(742, 331);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdArchiveDashBoard)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdArchiveDashBoard;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

    }
}
