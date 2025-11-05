using System;

namespace Prana.WashSale.Forms
{
    partial class WashSaleUploadDesign
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
            this.pathLabel = new Infragistics.Win.Misc.UltraLabel();
            this.browseButton = new Infragistics.Win.Misc.UltraButton();
            this.okButton = new Infragistics.Win.Misc.UltraButton();
            this.cancelButton = new Infragistics.Win.Misc.UltraButton();
            this.uploadHeadLabel = new Infragistics.Win.Misc.UltraLabel();
            this.browsePath = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraFormUploadManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.WashSaleUploadDesign_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.browsePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormUploadManager)).BeginInit();
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.SuspendLayout();
            this.WashSaleUploadDesign_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pathLabel
            // 
            this.pathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathLabel.Location = new System.Drawing.Point(7, 97);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(42, 23);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "Path";
            this.pathLabel.UseAppStyling = false;
            // 
            // browseButton
            // 
            this.browseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseButton.Location = new System.Drawing.Point(372, 95);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(102, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "Browse";
            this.browseButton.UseAppStyling = false;
            this.browseButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(154, 209);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(89, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseAppStyling = false;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(249, 209);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(89, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseAppStyling = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // uploadHeadLabel
            // 
            this.uploadHeadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadHeadLabel.Location = new System.Drawing.Point(7, 49);
            this.uploadHeadLabel.Name = "uploadHeadLabel";
            this.uploadHeadLabel.Size = new System.Drawing.Size(371, 39);
            this.uploadHeadLabel.TabIndex = 4;
            this.uploadHeadLabel.Text = "Upload a file from local machine";
            this.uploadHeadLabel.UseAppStyling = false;
            // 
            // browsePath
            // 
            appearance1.BorderColor = System.Drawing.Color.DodgerBlue;
            this.browsePath.Appearance = appearance1;
            this.browsePath.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.browsePath.Location = new System.Drawing.Point(55, 97);
            this.browsePath.Name = "browsePath";
            this.browsePath.Size = new System.Drawing.Size(311, 19);
            this.browsePath.TabIndex = 5;
            this.browsePath.UseAppStyling = false;
            // 
            // ultraFormUploadManager
            // 
            this.ultraFormUploadManager.Form = this;
            // 
            // WashSaleUploadDesign_Fill_Panel
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            this.WashSaleUploadDesign_Fill_Panel.Appearance = appearance2;
            // 
            // WashSaleUploadDesign_Fill_Panel.ClientArea
            // 
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.Controls.Add(this.browsePath);
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.Controls.Add(this.uploadHeadLabel);
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.Controls.Add(this.cancelButton);
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.Controls.Add(this.okButton);
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.Controls.Add(this.browseButton);
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.Controls.Add(this.pathLabel);
            this.WashSaleUploadDesign_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.WashSaleUploadDesign_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WashSaleUploadDesign_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.WashSaleUploadDesign_Fill_Panel.Name = "WashSaleUploadDesign_Fill_Panel";
            this.WashSaleUploadDesign_Fill_Panel.Size = new System.Drawing.Size(483, 254);
            this.WashSaleUploadDesign_Fill_Panel.TabIndex = 0;
            // 
            // _WashSaleUploadDesign_UltraFormManager_Dock_Area_Left
            // 
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormUploadManager;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.Name = "_WashSaleUploadDesign_UltraFormManager_Dock_Area_Left";
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 254);
            // 
            // _WashSaleUploadDesign_UltraFormManager_Dock_Area_Right
            // 
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormUploadManager;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(491, 32);
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.Name = "_WashSaleUploadDesign_UltraFormManager_Dock_Area_Right";
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 254);
            // 
            // _WashSaleUploadDesign_UltraFormManager_Dock_Area_Top
            // 
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormUploadManager;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.Name = "_WashSaleUploadDesign_UltraFormManager_Dock_Area_Top";
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(499, 32);
            // 
            // _WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom
            // 
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormUploadManager;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 286);
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.Name = "_WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom";
            this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(499, 8);
            // 
            // WashSaleUploadDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 294);
            this.Controls.Add(this.WashSaleUploadDesign_Fill_Panel);
            this.Controls.Add(this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MaximumSize = new System.Drawing.Size(499, 294);
            this.MinimumSize = new System.Drawing.Size(499, 294);
            this.ControlBox = false;
            this.Name = "WashSaleUploadDesign";
            ((System.ComponentModel.ISupportInitialize)(this.browsePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormUploadManager)).EndInit();
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.ResumeLayout(false);
            this.WashSaleUploadDesign_Fill_Panel.ClientArea.PerformLayout();
            this.WashSaleUploadDesign_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       
        #endregion

        private Infragistics.Win.Misc.UltraLabel pathLabel;
        private Infragistics.Win.Misc.UltraButton browseButton;
        private Infragistics.Win.Misc.UltraButton okButton;
        private Infragistics.Win.Misc.UltraButton cancelButton;
        private Infragistics.Win.Misc.UltraLabel uploadHeadLabel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor browsePath;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormUploadManager;
        private Infragistics.Win.Misc.UltraPanel WashSaleUploadDesign_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleUploadDesign_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleUploadDesign_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleUploadDesign_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleUploadDesign_UltraFormManager_Dock_Area_Bottom;
    }
}