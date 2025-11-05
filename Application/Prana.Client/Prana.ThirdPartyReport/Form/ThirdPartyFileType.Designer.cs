namespace Prana.ThirdPartyReport
{
    partial class ThirdPartyFileType
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThirdPartyFileType));
            this.chkUserDefind = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblFileFormat = new Infragistics.Win.Misc.UltraLabel();
            this.grpFileFormat = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkEXL = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.ThirdPartyFileType_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.chkUserDefind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpFileFormat)).BeginInit();
            this.grpFileFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkEXL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.ThirdPartyFileType_Fill_Panel.ClientArea.SuspendLayout();
            this.ThirdPartyFileType_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkUserDefind
            // 
            appearance1.FontData.BoldAsString = "False";
            this.chkUserDefind.Appearance = appearance1;
            this.chkUserDefind.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.chkUserDefind.Location = new System.Drawing.Point(15, 30);
            this.chkUserDefind.Name = "chkUserDefind";
            this.chkUserDefind.Size = new System.Drawing.Size(185, 20);
            this.chkUserDefind.TabIndex = 0;
            this.chkUserDefind.Tag = "";
            this.chkUserDefind.Text = "user defind";
            this.chkUserDefind.CheckedChanged += new System.EventHandler(this.chkUserDefind_CheckedChanged);
            // 
            // lblFileFormat
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblFileFormat.Appearance = appearance2;
            this.lblFileFormat.Location = new System.Drawing.Point(7, 9);
            this.lblFileFormat.Name = "lblFileFormat";
            this.lblFileFormat.Size = new System.Drawing.Size(175, 14);
            this.lblFileFormat.TabIndex = 2;
            this.lblFileFormat.Text = "Please select the File format !";
            // 
            // grpFileFormat
            // 
            this.grpFileFormat.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Center;
            this.grpFileFormat.Controls.Add(this.chkEXL);
            this.grpFileFormat.Controls.Add(this.lblFileFormat);
            this.grpFileFormat.Controls.Add(this.chkUserDefind);
            this.grpFileFormat.Location = new System.Drawing.Point(4, 8);
            this.grpFileFormat.Name = "grpFileFormat";
            this.grpFileFormat.Size = new System.Drawing.Size(216, 86);
            this.grpFileFormat.TabIndex = 3;
            // 
            // chkEXL
            // 
            appearance3.FontData.BoldAsString = "False";
            this.chkEXL.Appearance = appearance3;
            this.chkEXL.Location = new System.Drawing.Point(15, 56);
            this.chkEXL.Name = "chkEXL";
            this.chkEXL.Size = new System.Drawing.Size(142, 20);
            this.chkEXL.TabIndex = 3;
            this.chkEXL.Text = "EXCEL Format";
            this.chkEXL.CheckedChanged += new System.EventHandler(this.chkEXL_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(61, 98);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // ThirdPartyFileType_Fill_Panel
            // 
            // 
            // ThirdPartyFileType_Fill_Panel.ClientArea
            // 
            this.ThirdPartyFileType_Fill_Panel.ClientArea.Controls.Add(this.btnOK);
            this.ThirdPartyFileType_Fill_Panel.ClientArea.Controls.Add(this.grpFileFormat);
            this.ThirdPartyFileType_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ThirdPartyFileType_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThirdPartyFileType_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.ThirdPartyFileType_Fill_Panel.Name = "ThirdPartyFileType_Fill_Panel";
            this.ThirdPartyFileType_Fill_Panel.Size = new System.Drawing.Size(237, 139);
            this.ThirdPartyFileType_Fill_Panel.TabIndex = 0;
            // 
            // _ThirdPartyFileType_UltraFormManager_Dock_Area_Left
            // 
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.Name = "_ThirdPartyFileType_UltraFormManager_Dock_Area_Left";
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 139);
            // 
            // _ThirdPartyFileType_UltraFormManager_Dock_Area_Right
            // 
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(241, 27);
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.Name = "_ThirdPartyFileType_UltraFormManager_Dock_Area_Right";
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 139);
            // 
            // _ThirdPartyFileType_UltraFormManager_Dock_Area_Top
            // 
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.Name = "_ThirdPartyFileType_UltraFormManager_Dock_Area_Top";
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(245, 27);
            // 
            // _ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom
            // 
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 166);
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.Name = "_ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom";
            this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(245, 4);
            // 
            // ThirdPartyFileType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 170);
            this.Controls.Add(this.ThirdPartyFileType_Fill_Panel);
            this.Controls.Add(this._ThirdPartyFileType_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ThirdPartyFileType_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ThirdPartyFileType_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ThirdPartyFileType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Third Party File Type";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ThirdPartyFileType_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.chkUserDefind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpFileFormat)).EndInit();
            this.grpFileFormat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkEXL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ThirdPartyFileType_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ThirdPartyFileType_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkUserDefind;
        private Infragistics.Win.Misc.UltraLabel lblFileFormat;
        private Infragistics.Win.Misc.UltraGroupBox grpFileFormat;
        private Infragistics.Win.Misc.UltraButton btnOK;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkEXL;
        private Infragistics.Win.Misc.UltraPanel ThirdPartyFileType_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyFileType_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyFileType_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyFileType_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyFileType_UltraFormManager_Dock_Area_Bottom;
    }
}