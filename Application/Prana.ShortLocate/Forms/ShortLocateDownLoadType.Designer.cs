namespace Prana.ShortLocate.Forms
{
    partial class ShortLocateDownLoadType
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.btn_OK = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkExcel = new Infragistics.Win.UltraWinEditors.UltraRadioButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.ShortLocateDownLoadType_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.chkCSV = new Infragistics.Win.UltraWinEditors.UltraRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.ShortLocateDownLoadType_Fill_Panel.ClientArea.SuspendLayout();
            this.ShortLocateDownLoadType_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkCSV)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(73, 72);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "OK";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Center;
            this.ultraGroupBox1.Controls.Add(this.chkCSV);
            this.ultraGroupBox1.Controls.Add(this.chkExcel);
            this.ultraGroupBox1.Controls.Add(this.btn_OK);
            this.ultraGroupBox1.Location = new System.Drawing.Point(23, 23);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(212, 109);
            this.ultraGroupBox1.TabIndex = 5;
            // 
            // chkExcel
            // 
            appearance2.FontData.BoldAsString = "True";
            this.chkExcel.Appearance = appearance2;
            this.chkExcel.Location = new System.Drawing.Point(6, 6);
            this.chkExcel.Name = "chkExcel";
            this.chkExcel.Size = new System.Drawing.Size(120, 31);
            this.chkExcel.TabIndex = 5;
            this.chkExcel.Text = "Excel Format";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // ShortLocateDownLoadType_Fill_Panel
            // 
            // 
            // ShortLocateDownLoadType_Fill_Panel.ClientArea
            // 
            this.ShortLocateDownLoadType_Fill_Panel.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ShortLocateDownLoadType_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ShortLocateDownLoadType_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShortLocateDownLoadType_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.ShortLocateDownLoadType_Fill_Panel.Name = "ShortLocateDownLoadType_Fill_Panel";
            this.ShortLocateDownLoadType_Fill_Panel.Size = new System.Drawing.Size(257, 149);
            this.ShortLocateDownLoadType_Fill_Panel.TabIndex = 0;
            // 
            // _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left
            // 
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.Name = "_ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left";
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 149);
            // 
            // _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right
            // 
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(265, 32);
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.Name = "_ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right";
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 149);
            // 
            // _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top
            // 
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.Name = "_ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top";
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(273, 32);
            // 
            // _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom
            // 
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 181);
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.Name = "_ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom";
            this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(273, 8);
            // 
            // chkCSV
            // 
            appearance1.FontData.BoldAsString = "True";
            this.chkCSV.Appearance = appearance1;
            this.chkCSV.Location = new System.Drawing.Point(6, 35);
            this.chkCSV.Name = "chkCSV";
            this.chkCSV.Size = new System.Drawing.Size(120, 31);
            this.chkCSV.TabIndex = 6;
            this.chkCSV.Text = "CSV Format";
            // 
            // ShortLocateDownLoadType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 189);
            this.Controls.Add(this.ShortLocateDownLoadType_Fill_Panel);
            this.Controls.Add(this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShortLocateDownLoadType";
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ShortLocateDownLoadType_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ShortLocateDownLoadType_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkCSV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btn_OK;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.Misc.UltraPanel ShortLocateDownLoadType_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShortLocateDownLoadType_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinEditors.UltraRadioButton chkExcel;
        private Infragistics.Win.UltraWinEditors.UltraRadioButton chkCSV;
    }
}