using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Prana.ThirdPartyUI.Forms
{
    partial class FileLogs
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
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._FileLogs_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._FileLogs_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._FileLogs_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._FileLogs_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraGridBagLayoutManager1 = new Infragistics.Win.Misc.UltraGridBagLayoutManager(this.components);
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.grdFileLogs = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFileLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _FileLogs_UltraFormManager_Dock_Area_Left
            // 
            this._FileLogs_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FileLogs_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FileLogs_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._FileLogs_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FileLogs_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._FileLogs_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._FileLogs_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._FileLogs_UltraFormManager_Dock_Area_Left.Name = "_FileLogs_UltraFormManager_Dock_Area_Left";
            this._FileLogs_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 466);
            // 
            // _FileLogs_UltraFormManager_Dock_Area_Right
            // 
            this._FileLogs_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FileLogs_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FileLogs_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._FileLogs_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FileLogs_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._FileLogs_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._FileLogs_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(692, 31);
            this._FileLogs_UltraFormManager_Dock_Area_Right.Name = "_FileLogs_UltraFormManager_Dock_Area_Right";
            this._FileLogs_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 466);
            // 
            // _FileLogs_UltraFormManager_Dock_Area_Top
            // 
            this._FileLogs_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FileLogs_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FileLogs_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._FileLogs_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FileLogs_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._FileLogs_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._FileLogs_UltraFormManager_Dock_Area_Top.Name = "_FileLogs_UltraFormManager_Dock_Area_Top";
            this._FileLogs_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(700, 31);
            // 
            // _FileLogs_UltraFormManager_Dock_Area_Bottom
            // 
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 497);
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.Name = "_FileLogs_UltraFormManager_Dock_Area_Bottom";
            this._FileLogs_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(700, 8);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(67)))), ((int)(((byte)(85)))));
            this.btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(303, 450);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 33);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseAppStyling = false;
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // grdFileLogs
            // 
            this.grdFileLogs.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdFileLogs.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            appearance1.TextHAlignAsString = "Left";
            this.grdFileLogs.DisplayLayout.Override.CellAppearance = appearance1;
            this.grdFileLogs.DisplayLayout.Override.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
            this.grdFileLogs.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdFileLogs.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFree;
            this.grdFileLogs.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdFileLogs.Location = new System.Drawing.Point(8, 32);
            this.grdFileLogs.Name = "grdFileLogs";
            this.grdFileLogs.Size = new System.Drawing.Size(686, 415);
            this.grdFileLogs.TabIndex = 5;
            this.grdFileLogs.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdFileLogs_InitializeLayout);
            // 
            // FileLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 505);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grdFileLogs);
            this.Controls.Add(this._FileLogs_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._FileLogs_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._FileLogs_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._FileLogs_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 510);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 505);
            this.Name = "FileLogs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Log";
            this.Load += new System.EventHandler(this.FileLogs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFileLogs)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FileLogs_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FileLogs_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FileLogs_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FileLogs_UltraFormManager_Dock_Area_Bottom;
        private Utilities.UI.UIUtilities.PranaUltraGrid grdFileLogs;
        private Infragistics.Win.Misc.UltraGridBagLayoutManager ultraGridBagLayoutManager1;
        private Infragistics.Win.Misc.UltraButton btnOK;
    }
}