namespace Prana.Tools
{
    partial class ReconPrefForm
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
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSaveTemplates = new Infragistics.Win.Misc.UltraButton();
            this.btnSaveTemplate = new Infragistics.Win.Misc.UltraButton();
            this.btnCancelUpdate = new Infragistics.Win.Misc.UltraButton();
            this.ctrlReconTemplate1 = new Prana.Tools.ctrlReconTemplate();
            this.btnRunRecon = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(617, 619);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Apply";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(536, 619);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // btnSaveTemplates
            // 
            this.btnSaveTemplates.Location = new System.Drawing.Point(317, 619);
            this.btnSaveTemplates.Name = "btnSaveTemplates";
            this.btnSaveTemplates.Size = new System.Drawing.Size(99, 23);
            this.btnSaveTemplates.TabIndex = 16;
            this.btnSaveTemplates.Text = "Save Template";
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveTemplate.Location = new System.Drawing.Point(589, 4);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnSaveTemplate.TabIndex = 1;
            this.btnSaveTemplate.Text = "Save";
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // btnCancelUpdate
            // 
            this.btnCancelUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelUpdate.Location = new System.Drawing.Point(461, 4);
            this.btnCancelUpdate.Name = "btnCancelUpdate";
            this.btnCancelUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnCancelUpdate.TabIndex = 2;
            this.btnCancelUpdate.Text = "Cancel";
            this.btnCancelUpdate.Click += new System.EventHandler(this.btnCancelUpdate_Click);
            // 
            // ctrlReconTemplate1
            // 
            this.ctrlReconTemplate1.AllowDrop = true;
            this.ctrlReconTemplate1.BackColor = System.Drawing.Color.Transparent;
            this.ctrlReconTemplate1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlReconTemplate1.Location = new System.Drawing.Point(0, 0);
            this.ctrlReconTemplate1.MinimumSize = new System.Drawing.Size(1125, 615);
            this.ctrlReconTemplate1.Name = "ctrlReconTemplate1";
            this.ctrlReconTemplate1.Size = new System.Drawing.Size(1125, 615);
            this.ctrlReconTemplate1.TabIndex = 3;
            // 
            // btnRunRecon
            // 
            this.btnRunRecon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunRecon.Location = new System.Drawing.Point(737, 5);
            this.btnRunRecon.Name = "btnRunRecon";
            this.btnRunRecon.Size = new System.Drawing.Size(75, 23);
            this.btnRunRecon.TabIndex = 3;
            this.btnRunRecon.Text = "Run Now";
            this.btnRunRecon.Visible = false;
            this.btnRunRecon.Click += new System.EventHandler(this.btnRunRecon_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancelUpdate);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSaveTemplate);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnRunRecon);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(4, 642);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1125, 31);
            this.ultraPanel1.TabIndex = 4;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.ctrlReconTemplate1);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(4, 27);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(1125, 615);
            this.ultraPanel2.TabIndex = 5;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ReconPrefForm_UltraFormManager_Dock_Area_Left
            // 
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.Name = "_ReconPrefForm_UltraFormManager_Dock_Area_Left";
            this._ReconPrefForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 646);
            // 
            // _ReconPrefForm_UltraFormManager_Dock_Area_Right
            // 
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1129, 27);
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.Name = "_ReconPrefForm_UltraFormManager_Dock_Area_Right";
            this._ReconPrefForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 646);
            // 
            // _ReconPrefForm_UltraFormManager_Dock_Area_Top
            // 
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.Name = "_ReconPrefForm_UltraFormManager_Dock_Area_Top";
            this._ReconPrefForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1133, 27);
            // 
            // _ReconPrefForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 673);
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.Name = "_ReconPrefForm_UltraFormManager_Dock_Area_Bottom";
            this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1133, 4);
            // 
            // ReconPrefForm
            // 
            this.ShowIcon = false;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1133, 677);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._ReconPrefForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ReconPrefForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ReconPrefForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ReconPrefForm_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(1066, 660);
            this.Name = "ReconPrefForm";
            this.Text = "Recon Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReconPrefForm_FormClosing);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnSave;
      //  private ctrlMatchingRules ctrlMatchingRules1;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnSaveTemplates;
        private Infragistics.Win.Misc.UltraButton btnSaveTemplate;
        private Infragistics.Win.Misc.UltraButton btnCancelUpdate;
        private Prana.Tools.ctrlReconTemplate ctrlReconTemplate1;
        private Infragistics.Win.Misc.UltraButton btnRunRecon;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReconPrefForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReconPrefForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReconPrefForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReconPrefForm_UltraFormManager_Dock_Area_Bottom;       
        //private ctrlReconXSLTMapping ctrlReconXSLTMapping1;

    }
}