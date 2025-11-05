namespace Prana.Tools
{
    partial class frmAccountLock
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
            _pranaPositionServices.Dispose();
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
            this.frmAccountLock_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.grdAccountLock = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this._frmAccountLock_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._frmAccountLock_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmAccountLock_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmAccountLock_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.frmAccountLock_Fill_Panel.ClientArea.SuspendLayout();
            this.frmAccountLock_Fill_Panel.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // frmAccountLock_Fill_Panel
            // 
            // 
            // frmAccountLock_Fill_Panel.ClientArea
            // 
            this.frmAccountLock_Fill_Panel.ClientArea.Controls.Add(this.ultraPanel1);
            this.frmAccountLock_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.frmAccountLock_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frmAccountLock_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.frmAccountLock_Fill_Panel.Name = "frmAccountLock_Fill_Panel";
            this.frmAccountLock_Fill_Panel.Size = new System.Drawing.Size(334, 449);
            this.frmAccountLock_Fill_Panel.TabIndex = 0;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(334, 449);
            this.ultraPanel1.TabIndex = 10;
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
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.grdAccountLock);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraStatusBar1);
            this.splitContainer1.Size = new System.Drawing.Size(334, 449);
            this.splitContainer1.SplitterDistance = 417;
            this.splitContainer1.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(251, 393);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnRefresh.Location = new System.Drawing.Point(122, 393);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grdAccountLock
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdAccountLock.DisplayLayout.Appearance = appearance1;
            this.grdAccountLock.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccountLock.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdAccountLock.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAccountLock.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdAccountLock.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAccountLock.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdAccountLock.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccountLock.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdAccountLock.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdAccountLock.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdAccountLock.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdAccountLock.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdAccountLock.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdAccountLock.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdAccountLock.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdAccountLock.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdAccountLock.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdAccountLock.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdAccountLock.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAccountLock.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdAccountLock.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdAccountLock.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdAccountLock.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdAccountLock.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccountLock.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccountLock.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccountLock.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdAccountLock.Location = new System.Drawing.Point(0, 0);
            this.grdAccountLock.Name = "grdAccountLock";
            this.grdAccountLock.Size = new System.Drawing.Size(334, 387);
            this.grdAccountLock.TabIndex = 0;
            this.grdAccountLock.Text = "ultraGrid1";
            this.grdAccountLock.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAccountLock_InitializeLayout);
            this.grdAccountLock.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccountLock_CellChange);
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 5);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Size = new System.Drawing.Size(334, 23);
            this.ultraStatusBar1.TabIndex = 0;
            // 
            // _frmAccountLock_Toolbars_Dock_Area_Left
            // 
            this._frmAccountLock_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmAccountLock_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmAccountLock_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._frmAccountLock_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmAccountLock_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._frmAccountLock_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._frmAccountLock_Toolbars_Dock_Area_Left.Name = "_frmAccountLock_Toolbars_Dock_Area_Left";
            this._frmAccountLock_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 449);
            this._frmAccountLock_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            this.ultraToolbarsManager1.IsGlassSupported = false;
            // 
            // _frmAccountLock_Toolbars_Dock_Area_Right
            // 
            this._frmAccountLock_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmAccountLock_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmAccountLock_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._frmAccountLock_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmAccountLock_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._frmAccountLock_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(338, 27);
            this._frmAccountLock_Toolbars_Dock_Area_Right.Name = "_frmAccountLock_Toolbars_Dock_Area_Right";
            this._frmAccountLock_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 449);
            this._frmAccountLock_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _frmAccountLock_Toolbars_Dock_Area_Top
            // 
            this._frmAccountLock_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmAccountLock_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmAccountLock_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._frmAccountLock_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmAccountLock_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmAccountLock_Toolbars_Dock_Area_Top.Name = "_frmAccountLock_Toolbars_Dock_Area_Top";
            this._frmAccountLock_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(342, 27);
            this._frmAccountLock_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _frmAccountLock_Toolbars_Dock_Area_Bottom
            // 
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 476);
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.Name = "_frmAccountLock_Toolbars_Dock_Area_Bottom";
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(342, 4);
            this._frmAccountLock_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // frmAccountLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 480);
            this.Controls.Add(this.frmAccountLock_Fill_Panel);
            this.Controls.Add(this._frmAccountLock_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._frmAccountLock_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._frmAccountLock_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._frmAccountLock_Toolbars_Dock_Area_Top);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(342, 480);
            this.MinimumSize = new System.Drawing.Size(342, 480);
            this.Name = "frmAccountLock";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Account Lock";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAccountLock_FormClosed);
            this.Load += new System.EventHandler(this.frmAccountLock_Load);
            this.frmAccountLock_Fill_Panel.ClientArea.ResumeLayout(false);
            this.frmAccountLock_Fill_Panel.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel frmAccountLock_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmAccountLock_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmAccountLock_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmAccountLock_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmAccountLock_Toolbars_Dock_Area_Top;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdAccountLock;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;
    }
}