namespace Prana.Tools.PL.SecMaster
{
    partial class AdvSearchFilterUI
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnAddNewGroup = new Infragistics.Win.Misc.UltraButton();
            this.panelConditions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.AdvSearchFilertUI_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
           //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.SuspendLayout();
            this.AdvSearchFilertUI_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddNewGroup
            // 
            this.btnAddNewGroup.AutoSize = true;
            this.btnAddNewGroup.Location = new System.Drawing.Point(8, 5);
            this.btnAddNewGroup.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddNewGroup.Name = "btnAddNewGroup";
            this.btnAddNewGroup.Size = new System.Drawing.Size(254, 28);
            this.btnAddNewGroup.TabIndex = 1;
            this.btnAddNewGroup.Text = "Add new logical group of conditions";
            this.btnAddNewGroup.Click += new System.EventHandler(this.btnAddNewGroup_Click);
            // 
            // panelConditions
            // 
            this.panelConditions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            //this.panelConditions.AutoScroll = true;
            this.panelConditions.AutoSize = true;
            this.panelConditions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelConditions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelConditions.Location = new System.Drawing.Point(8, 49);
            this.panelConditions.Margin = new System.Windows.Forms.Padding(4);
            this.panelConditions.Name = "panelConditions";
            this.panelConditions.Size = new System.Drawing.Size(0, 0);
            this.inboxControlStyler1.SetStyleSettings(this.panelConditions, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panelConditions.TabIndex = 2;
            this.panelConditions.WrapContents = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(624, 597);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 28);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ultraButton2
            // 
            this.ultraButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton2.Location = new System.Drawing.Point(732, 597);
            this.ultraButton2.Margin = new System.Windows.Forms.Padding(4);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(100, 28);
            this.ultraButton2.TabIndex = 4;
            this.ultraButton2.Text = "&Cancel";
            this.ultraButton2.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // AdvSearchFilertUI_Fill_Panel
            // 
            // 
            // AdvSearchFilertUI_Fill_Panel.ClientArea
            // 
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.panelConditions);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.ultraButton2);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.btnSearch);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.btnAddNewGroup);
            this.AdvSearchFilertUI_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AdvSearchFilertUI_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdvSearchFilertUI_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.AdvSearchFilertUI_Fill_Panel.Margin = new System.Windows.Forms.Padding(4);
            this.AdvSearchFilertUI_Fill_Panel.Name = "AdvSearchFilertUI_Fill_Panel";
            appearance1.FontData.SizeInPoints = 10F;
            scrollBarLook1.Appearance = appearance1;
            appearance2.FontData.SizeInPoints = 10F;
            scrollBarLook1.AppearanceHorizontal = appearance2;
            appearance3.FontData.SizeInPoints = 10F;
            scrollBarLook1.AppearanceVertical = appearance3;
            appearance4.FontData.SizeInPoints = 10F;
            scrollBarLook1.ButtonAppearance = appearance4;
            appearance5.FontData.SizeInPoints = 10F;
            scrollBarLook1.ButtonAppearanceHorizontal = appearance5;
            appearance6.FontData.SizeInPoints = 10F;
            scrollBarLook1.ButtonAppearanceVertical = appearance6;
            this.AdvSearchFilertUI_Fill_Panel.ScrollBarLook = scrollBarLook1;
            this.AdvSearchFilertUI_Fill_Panel.Size = new System.Drawing.Size(848, 629);
            this.AdvSearchFilertUI_Fill_Panel.TabIndex = 0; 
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _AdvSearchFilertUI_UltraFormManager_Dock_Area_Left
            // 
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(4);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.Name = "_AdvSearchFilertUI_UltraFormManager_Dock_Area_Left";
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 629);
            // 
            // _AdvSearchFilertUI_UltraFormManager_Dock_Area_Right
            // 
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(852, 27);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(4);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.Name = "_AdvSearchFilertUI_UltraFormManager_Dock_Area_Right";
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 629);
            // 
            // _AdvSearchFilertUI_UltraFormManager_Dock_Area_Top
            // 
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(4);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.Name = "_AdvSearchFilertUI_UltraFormManager_Dock_Area_Top";
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(856, 27);
            // 
            // _AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom
            // 
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 656);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(4);
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.Name = "_AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom";
            this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(856, 4);
            // 
            // AdvSearchFilterUI
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(856, 660);
            this.Controls.Add(this.AdvSearchFilertUI_Fill_Panel);
            this.Controls.Add(this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(856, 660);
            this.MinimumSize = new System.Drawing.Size(856, 660);
            this.Name = "AdvSearchFilterUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Advanced Search";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AdvSearchFilertUI_Load_1);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.PerformLayout();
            this.AdvSearchFilertUI_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnAddNewGroup;
        private System.Windows.Forms.FlowLayoutPanel panelConditions;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private Infragistics.Win.Misc.UltraPanel AdvSearchFilertUI_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AdvSearchFilertUI_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AdvSearchFilertUI_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AdvSearchFilertUI_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AdvSearchFilertUI_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}