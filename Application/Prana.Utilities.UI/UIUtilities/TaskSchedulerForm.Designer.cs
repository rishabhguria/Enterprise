namespace Prana.Utilities.UI.UIUtilities
{
    partial class TaskSchedulerForm
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
            this.spMain = new System.Windows.Forms.SplitContainer();
            this.taskScheduler1 = new Prana.Utilities.UI.UIUtilities.TaskScheduler();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.spMain)).BeginInit();
            this.spMain.Panel1.SuspendLayout();
            this.spMain.Panel2.SuspendLayout();
            this.spMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // spMain
            // 
            this.spMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spMain.IsSplitterFixed = true;
            this.spMain.Location = new System.Drawing.Point(4, 52);
            this.spMain.Name = "spMain";
            this.spMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spMain.Panel1
            // 
            this.spMain.Panel1.Controls.Add(this.taskScheduler1);
            // 
            // spMain.Panel2
            // 
            this.spMain.Panel2.Controls.Add(this.btnCancel);
            this.spMain.Panel2.Controls.Add(this.btnOk);
            this.spMain.Size = new System.Drawing.Size(617, 285);
            this.spMain.SplitterDistance = 250;
            this.inboxControlStyler1.SetStyleSettings(this.spMain, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.spMain.TabIndex = 0;
            // 
            // taskScheduler1
            // 
            this.taskScheduler1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.taskScheduler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskScheduler1.Location = new System.Drawing.Point(0, 0);
            this.taskScheduler1.MinimumSize = new System.Drawing.Size(605, 270);
            this.taskScheduler1.Name = "taskScheduler1";
            this.taskScheduler1.Size = new System.Drawing.Size(617, 270);
            this.taskScheduler1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(298, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnCancel, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(217, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnOk, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            // 
            // _TaskSchedulerForm_Toolbars_Dock_Area_Left
            // 
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.Name = "_TaskSchedulerForm_Toolbars_Dock_Area_Left";
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 285);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _TaskSchedulerForm_Toolbars_Dock_Area_Right
            // 
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(621, 52);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.Name = "_TaskSchedulerForm_Toolbars_Dock_Area_Right";
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 285);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _TaskSchedulerForm_Toolbars_Dock_Area_Top
            // 
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.Name = "_TaskSchedulerForm_Toolbars_Dock_Area_Top";
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(625, 52);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _TaskSchedulerForm_Toolbars_Dock_Area_Bottom
            // 
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 337);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.Name = "_TaskSchedulerForm_Toolbars_Dock_Area_Bottom";
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(625, 4);
            this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // TaskSchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(625, 341);
            this.Controls.Add(this.spMain);
            this.Controls.Add(this._TaskSchedulerForm_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._TaskSchedulerForm_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._TaskSchedulerForm_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._TaskSchedulerForm_Toolbars_Dock_Area_Top);
            this.MinimumSize = new System.Drawing.Size(625, 341);
            this.Name = "TaskSchedulerForm";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Task Scheduler";
            this.Load += new System.EventHandler(this.TaskSchedulerForm_Load);
            this.spMain.Panel1.ResumeLayout(false);
            this.spMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spMain)).EndInit();
            this.spMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spMain;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private TaskScheduler taskScheduler1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TaskSchedulerForm_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TaskSchedulerForm_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TaskSchedulerForm_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TaskSchedulerForm_Toolbars_Dock_Area_Top;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;

    }
}