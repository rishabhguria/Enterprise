namespace Prana.ClientCommon
{
    partial class GetDate
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
            this.dtPositionDate = new System.Windows.Forms.DateTimePicker();
            this.lblSchemeDate = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnContinue = new Infragistics.Win.Misc.UltraButton();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.GetDate_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._GetDate_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._GetDate_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._GetDate_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._GetDate_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.GetDate_Fill_Panel.ClientArea.SuspendLayout();
            this.GetDate_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtPositionDate
            // 
            this.dtPositionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPositionDate.Location = new System.Drawing.Point(52, 18);
            this.dtPositionDate.Name = "dtPositionDate";
            this.dtPositionDate.Size = new System.Drawing.Size(91, 20);
            this.dtPositionDate.TabIndex = 20;
            this.dtPositionDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtPositionDate_KeyPress);
            // 
            // lblSchemeDate
            // 
            this.lblSchemeDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSchemeDate.AutoSize = true;
            this.lblSchemeDate.Location = new System.Drawing.Point(9, 21);
            this.lblSchemeDate.Name = "lblSchemeDate";
            this.lblSchemeDate.Size = new System.Drawing.Size(28, 14);
            this.lblSchemeDate.TabIndex = 21;
            this.lblSchemeDate.Text = "Date";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnContinue);
            this.groupBox1.Controls.Add(this.dtPositionDate);
            this.groupBox1.Controls.Add(this.lblSchemeDate);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 57);
            this.groupBox1.TabIndex = 22;
            // 
            // btnContinue
            // 
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnContinue.Location = new System.Drawing.Point(149, 17);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(84, 23);
            this.btnContinue.TabIndex = 22;
            this.btnContinue.Text = "Continue";
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // GetDate_Fill_Panel
            // 
            // 
            // GetDate_Fill_Panel.ClientArea
            // 
            this.GetDate_Fill_Panel.ClientArea.Controls.Add(this.groupBox1);
            this.GetDate_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.GetDate_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetDate_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.GetDate_Fill_Panel.Name = "GetDate_Fill_Panel";
            this.GetDate_Fill_Panel.Size = new System.Drawing.Size(249, 71);
            this.GetDate_Fill_Panel.TabIndex = 0;
            // 
            // _GetDate_UltraFormManager_Dock_Area_Left
            // 
            this._GetDate_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GetDate_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._GetDate_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._GetDate_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GetDate_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._GetDate_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._GetDate_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._GetDate_UltraFormManager_Dock_Area_Left.Name = "_GetDate_UltraFormManager_Dock_Area_Left";
            this._GetDate_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 71);
            // 
            // _GetDate_UltraFormManager_Dock_Area_Right
            // 
            this._GetDate_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GetDate_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._GetDate_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._GetDate_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GetDate_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._GetDate_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._GetDate_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(253, 27);
            this._GetDate_UltraFormManager_Dock_Area_Right.Name = "_GetDate_UltraFormManager_Dock_Area_Right";
            this._GetDate_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 71);
            // 
            // _GetDate_UltraFormManager_Dock_Area_Top
            // 
            this._GetDate_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GetDate_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._GetDate_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._GetDate_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GetDate_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._GetDate_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._GetDate_UltraFormManager_Dock_Area_Top.Name = "_GetDate_UltraFormManager_Dock_Area_Top";
            this._GetDate_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(257, 27);
            // 
            // _GetDate_UltraFormManager_Dock_Area_Bottom
            // 
            this._GetDate_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GetDate_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._GetDate_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._GetDate_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GetDate_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._GetDate_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._GetDate_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 98);
            this._GetDate_UltraFormManager_Dock_Area_Bottom.Name = "_GetDate_UltraFormManager_Dock_Area_Bottom";
            this._GetDate_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(257, 4);
            // 
            // GetDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 102);
            this.Controls.Add(this.GetDate_Fill_Panel);
            this.Controls.Add(this._GetDate_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._GetDate_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._GetDate_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._GetDate_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(257, 102);
            this.MinimumSize = new System.Drawing.Size(257, 102);
            this.Name = "GetDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Date";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetDate_FormClosing);
            this.Load += new System.EventHandler(this.GetDate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.GetDate_Fill_Panel.ClientArea.ResumeLayout(false);
            this.GetDate_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblSchemeDate;
        private Infragistics.Win.Misc.UltraGroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton btnContinue;
        public System.Windows.Forms.DateTimePicker dtPositionDate;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel GetDate_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _GetDate_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _GetDate_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _GetDate_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _GetDate_UltraFormManager_Dock_Area_Bottom;
    }
}