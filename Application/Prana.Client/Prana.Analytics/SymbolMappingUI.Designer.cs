namespace Prana.Analytics
{
    partial class SymbolMappingUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolMappingUI));
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grdSymbolMapping = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.upnlBody = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolMapping)).BeginInit();
            this.upnlBody.ClientArea.SuspendLayout();
            this.upnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.inboxControlStyler1.SetStyleSettings(this.panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            resources.ApplyResources(this.statusLabel, "statusLabel");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowToolStripMenuItem,
            this.deleteRowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.inboxControlStyler1.SetStyleSettings(this.contextMenuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            resources.ApplyResources(this.addRowToolStripMenuItem, "addRowToolStripMenuItem");
            this.addRowToolStripMenuItem.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // deleteRowToolStripMenuItem
            // 
            this.deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
            resources.ApplyResources(this.deleteRowToolStripMenuItem, "deleteRowToolStripMenuItem");
            this.deleteRowToolStripMenuItem.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grdSymbolMapping
            // 
            this.grdSymbolMapping.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            this.grdSymbolMapping.DisplayLayout.AddNewBox.Appearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Black;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdSymbolMapping.DisplayLayout.Appearance = appearance2;
            this.grdSymbolMapping.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSymbolMapping.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSymbolMapping.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSymbolMapping.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.grdSymbolMapping.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSymbolMapping.DisplayLayout.GroupByBox.Hidden = true;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSymbolMapping.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.grdSymbolMapping.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSymbolMapping.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.Color.LightYellow;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdSymbolMapping.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            this.grdSymbolMapping.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdSymbolMapping.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdSymbolMapping.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdSymbolMapping.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdSymbolMapping.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdSymbolMapping.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSymbolMapping.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            this.grdSymbolMapping.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSymbolMapping.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.Color.Black;
            appearance10.BackColor2 = System.Drawing.Color.Black;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            appearance10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdSymbolMapping.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdSymbolMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BackColor2 = System.Drawing.Color.Transparent;
            appearance11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdSymbolMapping.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            this.grdSymbolMapping.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSymbolMapping.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSymbolMapping.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSymbolMapping.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdSymbolMapping.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdSymbolMapping.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSymbolMapping.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSymbolMapping.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            resources.ApplyResources(this.grdSymbolMapping, "grdSymbolMapping");
            this.grdSymbolMapping.Name = "grdSymbolMapping";
            this.grdSymbolMapping.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSymbolMapping_AfterCellUpdate);
            this.grdSymbolMapping.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdInterestRate_InitializeLayout);
            // 
            // upnlBody
            // 
            // 
            // upnlBody.ClientArea
            // 
            this.upnlBody.ClientArea.Controls.Add(this.grdSymbolMapping);
            this.upnlBody.ClientArea.Controls.Add(this.panel1);
            resources.ApplyResources(this.upnlBody, "upnlBody");
            this.upnlBody.Name = "upnlBody";
            this.upnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _SymbolMappingUI_UltraFormManager_Dock_Area_Left
            // 
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            resources.ApplyResources(this._SymbolMappingUI_UltraFormManager_Dock_Area_Left, "_SymbolMappingUI_UltraFormManager_Dock_Area_Left");
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Left.Name = "_SymbolMappingUI_UltraFormManager_Dock_Area_Left";
            // 
            // _SymbolMappingUI_UltraFormManager_Dock_Area_Right
            // 
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            resources.ApplyResources(this._SymbolMappingUI_UltraFormManager_Dock_Area_Right, "_SymbolMappingUI_UltraFormManager_Dock_Area_Right");
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Right.Name = "_SymbolMappingUI_UltraFormManager_Dock_Area_Right";
            // 
            // _SymbolMappingUI_UltraFormManager_Dock_Area_Top
            // 
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            resources.ApplyResources(this._SymbolMappingUI_UltraFormManager_Dock_Area_Top, "_SymbolMappingUI_UltraFormManager_Dock_Area_Top");
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Top.Name = "_SymbolMappingUI_UltraFormManager_Dock_Area_Top";
            // 
            // _SymbolMappingUI_UltraFormManager_Dock_Area_Bottom
            // 
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            resources.ApplyResources(this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom, "_SymbolMappingUI_UltraFormManager_Dock_Area_Bottom");
            this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom.Name = "_SymbolMappingUI_UltraFormManager_Dock_Area_Bottom";
            // 
            // SymbolMappingUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.upnlBody);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._SymbolMappingUI_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._SymbolMappingUI_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._SymbolMappingUI_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._SymbolMappingUI_UltraFormManager_Dock_Area_Bottom);
            this.Name = "SymbolMappingUI";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SymbolMappingUI_FormClosing);
            this.Load += new System.EventHandler(this.SymbolMappingUI_Load);
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolMapping)).EndInit();
            this.upnlBody.ClientArea.ResumeLayout(false);
            this.upnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRowToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSymbolMapping;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private Infragistics.Win.Misc.UltraPanel upnlBody;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolMappingUI_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolMappingUI_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolMappingUI_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolMappingUI_UltraFormManager_Dock_Area_Bottom;
    }
}