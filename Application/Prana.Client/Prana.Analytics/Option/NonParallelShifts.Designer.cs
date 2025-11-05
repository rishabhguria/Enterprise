namespace Prana.Analytics
{
    partial class NonParallelShiftsUI
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveNonParallelShifts = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
           // this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ctrlSettings1 = new Prana.Analytics.CtrlSettings();
            this.ctrlVolShockAdjustment1 = new Prana.Analytics.CtrlVolShockAdjustment();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraTabPageControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.ultraTabSharedControlsPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlSettings1);
            this.ultraTabPageControl1.Controls.Add(this.panel1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 25);
            this.ultraTabPageControl1.Margin = new System.Windows.Forms.Padding(4);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(666, 404);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveNonParallelShifts);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 376);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 28);
            this.inboxControlStyler1.SetStyleSettings(this.panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panel1.TabIndex = 0;
            // 
            // btnSaveNonParallelShifts
            // 
            this.btnSaveNonParallelShifts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnSaveNonParallelShifts.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.btnSaveNonParallelShifts.Location = new System.Drawing.Point(283, 4);
            this.btnSaveNonParallelShifts.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveNonParallelShifts.Name = "btnSaveNonParallelShifts";
            this.btnSaveNonParallelShifts.Size = new System.Drawing.Size(100, 23);
            this.btnSaveNonParallelShifts.TabIndex = 3;
            this.btnSaveNonParallelShifts.Text = "Save";
            this.btnSaveNonParallelShifts.Click += new System.EventHandler(this.btnSaveNonParallelShifts_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(666, 27);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ctrlVolShockAdjustment1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Margin = new System.Windows.Forms.Padding(4);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(666, 404);
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTabControl1.Location = new System.Drawing.Point(4, 27);
            this.ultraTabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.panel1});
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(670, 432);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab1.Key = "Settings";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Settings";
            ultraTab2.Key = "VolShockAdjustment";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Vol. Shock Adjustment";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Controls.Add(this.panel1);
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Margin = new System.Windows.Forms.Padding(4);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(666, 404);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _NonParallelShiftsUI_UltraFormManager_Dock_Area_Left
            // 
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(4);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.Name = "_NonParallelShiftsUI_UltraFormManager_Dock_Area_Left";
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 432);
            // 
            // _NonParallelShiftsUI_UltraFormManager_Dock_Area_Right
            // 
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(674, 27);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(4);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.Name = "_NonParallelShiftsUI_UltraFormManager_Dock_Area_Right";
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 432);
            // 
            // _NonParallelShiftsUI_UltraFormManager_Dock_Area_Top
            // 
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(4);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.Name = "_NonParallelShiftsUI_UltraFormManager_Dock_Area_Top";
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(678, 27);
            // 
            // _NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom
            // 
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 459);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(4);
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.Name = "_NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom";
            this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(678, 4);
            // 
            // ctrlSettings1
            // 
            this.ctrlSettings1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSettings1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlSettings1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSettings1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlSettings1.Name = "ctrlSettings1";
            this.ctrlSettings1.Size = new System.Drawing.Size(666, 376);
            this.ctrlSettings1.TabIndex = 1;
            // 
            // ctrlVolShockAdjustment1
            // 
            this.ctrlVolShockAdjustment1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlVolShockAdjustment1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlVolShockAdjustment1.Location = new System.Drawing.Point(0, 0);
            this.ctrlVolShockAdjustment1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlVolShockAdjustment1.Name = "ctrlVolShockAdjustment1";
            this.ctrlVolShockAdjustment1.Size = new System.Drawing.Size(666, 404);
            this.ctrlVolShockAdjustment1.TabIndex = 0;
            // 
            // NonParallelShiftsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 463);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "NonParallelShiftsUI";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Non-Parallel Shifts";
            this.Load += new System.EventHandler(this.NonParallelShiftsUI_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ultraTabSharedControlsPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private CtrlSettings ctrlSettings1;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private CtrlVolShockAdjustment ctrlVolShockAdjustment1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private Infragistics.Win.Misc.UltraButton btnSaveNonParallelShifts;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NonParallelShiftsUI_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NonParallelShiftsUI_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NonParallelShiftsUI_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NonParallelShiftsUI_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}