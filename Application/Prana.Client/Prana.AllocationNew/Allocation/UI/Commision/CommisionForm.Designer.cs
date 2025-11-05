using Prana.Global;
using System.Windows.Forms;
namespace Prana.AllocationNew
{
    partial class CommissionForm
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
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedBottom, new System.Guid("3d20b7c6-547a-4206-a133-10779b319a0f"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("e1b335d6-bc08-4275-8919-50f03777df05"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("3d20b7c6-547a-4206-a133-10779b319a0f"), -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommissionForm));
            this.ctrlRecalculate1 = new Prana.AllocationNew.CtrlRecalculate();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblAllocationType = new System.Windows.Forms.Label();
            this.lblAllocationTypeValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._CommissionAllocationUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._CommissionAllocationUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._CommissionAllocationUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._CommissionAllocationUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._CommissionAllocationAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.ctrlAmendmend1 = new Prana.AllocationNew.CtrlAmendmend();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this._CommissionAllocationAutoHideControl.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctrlRecalculate1
            // 
            this.ctrlRecalculate1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ctrlRecalculate1.CompanyUserID = -2147483648;
            this.ctrlRecalculate1.Location = new System.Drawing.Point(0, 18);
            this.ctrlRecalculate1.Name = "ctrlRecalculate1";
            this.ctrlRecalculate1.Size = new System.Drawing.Size(873, 185);
            this.ctrlRecalculate1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.lblAllocationType);
            this.groupBox1.Controls.Add(this.lblAllocationTypeValue);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(592, 469);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 40);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // lblAllocationType
            // 
            this.lblAllocationType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAllocationType.AutoSize = true;
            this.lblAllocationType.Location = new System.Drawing.Point(49, 13);
            this.lblAllocationType.Name = "lblAllocationType";
            this.lblAllocationType.Size = new System.Drawing.Size(127, 13);
            this.lblAllocationType.TabIndex = 4;
            this.lblAllocationType.Text = "Commission Methodology";
            // 
            // lblAllocationTypeValue
            // 
            this.lblAllocationTypeValue.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAllocationTypeValue.AutoSize = true;
            this.lblAllocationTypeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllocationTypeValue.Location = new System.Drawing.Point(203, 13);
            this.lblAllocationTypeValue.Name = "lblAllocationTypeValue";
            this.lblAllocationTypeValue.Size = new System.Drawing.Size(0, 13);
            this.lblAllocationTypeValue.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Commission Rule";
            this.label1.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(237, 11);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(90, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "PostAllocated";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(115, 11);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "PreAllocated";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Visible = false;
            // 
            // ultraDockManager1
            // 
            this.ultraDockManager1.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.DoubleClickAction = Infragistics.Win.UltraWinDock.PaneDoubleClickAction.None;
            dockableControlPane1.Control = this.ctrlRecalculate1;
            dockableControlPane1.FlyoutSize = new System.Drawing.Size(-1, 330);
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(4, 388, 466, 261);
            dockableControlPane1.Pinned = false;
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            //dockableControlPane1.Text = "Commission Recalculate";
            dockableControlPane1.Text = "Bulk Changes";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(873, 330);
            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.ultraDockManager1.HostControl = this;
            // 
            // _CommissionAllocationUnpinnedTabAreaLeft
            // 
            this._CommissionAllocationUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._CommissionAllocationUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._CommissionAllocationUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._CommissionAllocationUnpinnedTabAreaLeft.Name = "_CommissionAllocationUnpinnedTabAreaLeft";
            this._CommissionAllocationUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._CommissionAllocationUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 646);
            this._CommissionAllocationUnpinnedTabAreaLeft.TabIndex = 5;
            // 
            // _CommissionAllocationUnpinnedTabAreaRight
            // 
            this._CommissionAllocationUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._CommissionAllocationUnpinnedTabAreaRight.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._CommissionAllocationUnpinnedTabAreaRight.Location = new System.Drawing.Point(873, 0);
            this._CommissionAllocationUnpinnedTabAreaRight.Name = "_CommissionAllocationUnpinnedTabAreaRight";
            this._CommissionAllocationUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._CommissionAllocationUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 646);
            this._CommissionAllocationUnpinnedTabAreaRight.TabIndex = 6;
            // 
            // _CommissionAllocationUnpinnedTabAreaTop
            // 
            this._CommissionAllocationUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._CommissionAllocationUnpinnedTabAreaTop.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._CommissionAllocationUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._CommissionAllocationUnpinnedTabAreaTop.Name = "_CommissionAllocationUnpinnedTabAreaTop";
            this._CommissionAllocationUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._CommissionAllocationUnpinnedTabAreaTop.Size = new System.Drawing.Size(873, 0);
            this._CommissionAllocationUnpinnedTabAreaTop.TabIndex = 7;
            // 
            // _CommissionAllocationUnpinnedTabAreaBottom
            // 
            this._CommissionAllocationUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._CommissionAllocationUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._CommissionAllocationUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 625);
            this._CommissionAllocationUnpinnedTabAreaBottom.Name = "_CommissionAllocationUnpinnedTabAreaBottom";
            this._CommissionAllocationUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._CommissionAllocationUnpinnedTabAreaBottom.Size = new System.Drawing.Size(873, 21);
            this._CommissionAllocationUnpinnedTabAreaBottom.TabIndex = 8;
            // 
            // _CommissionAllocationAutoHideControl
            // 
            this._CommissionAllocationAutoHideControl.Controls.Add(this.dockableWindow1);
            this._CommissionAllocationAutoHideControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._CommissionAllocationAutoHideControl.Location = new System.Drawing.Point(0, 607);
            this._CommissionAllocationAutoHideControl.Name = "_CommissionAllocationAutoHideControl";
            this._CommissionAllocationAutoHideControl.Owner = this.ultraDockManager1;
            this._CommissionAllocationAutoHideControl.Size = new System.Drawing.Size(873, 18);
            this._CommissionAllocationAutoHideControl.TabIndex = 9;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.ctrlRecalculate1);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 5);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(873, 203);
            this.dockableWindow1.TabIndex = 12;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.windowDockingArea1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.windowDockingArea1.Location = new System.Drawing.Point(0, 421);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(873, 204);
            this.windowDockingArea1.TabIndex = 10;
            // 
            // ctrlAmendmend1
            // 
            this.ctrlAmendmend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlAmendmend1.Location = new System.Drawing.Point(0, 0);
            this.ctrlAmendmend1.Name = "ctrlAmendmend1";
            this.ctrlAmendmend1.Size = new System.Drawing.Size(873, 625);
            this.ctrlAmendmend1.TabIndex = 11;
            // 
            // CommissionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 646);
            this.Controls.Add(this._CommissionAllocationAutoHideControl);
            this.Controls.Add(this.ctrlAmendmend1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._CommissionAllocationUnpinnedTabAreaLeft);
            this.Controls.Add(this._CommissionAllocationUnpinnedTabAreaTop);
            this.Controls.Add(this._CommissionAllocationUnpinnedTabAreaRight);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._CommissionAllocationUnpinnedTabAreaBottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(751, 522);
            this.Name = "CommissionForm";
            this.Text = "Edit Trades/Commission";
            this.Load += new System.EventHandler(this.CommissionForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CommissionForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommissionForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this._CommissionAllocationAutoHideControl.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label lblAllocationType;
        private System.Windows.Forms.Label lblAllocationTypeValue;
        private System.Windows.Forms.Label label1;
        private CtrlRecalculate ctrlRecalculate1;
        private Infragistics.Win.UltraWinDock.AutoHideControl _CommissionAllocationAutoHideControl;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CommissionAllocationUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CommissionAllocationUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CommissionAllocationUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CommissionAllocationUnpinnedTabAreaRight;
        private Prana.AllocationNew.CtrlAmendmend ctrlAmendmend1;
        //private Infragistics.Win.Misc.UltraButton btnScreenshot;
        
        
    }
}