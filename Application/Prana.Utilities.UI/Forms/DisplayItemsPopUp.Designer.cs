using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Infragistics.Win;

namespace Prana.Utilities.UI.Forms
{
    partial class DisplayItemsPopUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayItemsPopUp));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.ultraPanelMain = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGridData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraLabelInformation = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButtonNo = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonYes = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonOK = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelBottom = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._PopUp_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PopUp_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PopUp_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PopUp_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraPanelTop = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanelMain.ClientArea.SuspendLayout();
            this.ultraPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridData)).BeginInit();
            this.ultraPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.ultraPanelTop.ClientArea.SuspendLayout();
            this.ultraPanelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanelMain
            // 
            this.ultraPanelMain.AutoSize = true;
            this.ultraPanelMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // 
            // ultraPanelMain.ClientArea
            // 
            this.ultraPanelMain.ClientArea.Controls.Add(this.ultraGridData);
            this.ultraPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelMain.Location = new System.Drawing.Point(8, 72);
            this.ultraPanelMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ultraPanelMain.Name = "ultraPanelMain";
            this.ultraPanelMain.Size = new System.Drawing.Size(734, 139);
            // 
            // ultraGridData
            // 
            this.ultraGridData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridData.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGridData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridData.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridData.DisplayLayout.UseFixedHeaders = true;
            this.ultraGridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridData.Location = new System.Drawing.Point(0, 0);
            this.ultraGridData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ultraGridData.Name = "ultraGrid_IncorrectQty";
            this.ultraGridData.Size = new System.Drawing.Size(728, 160);
            this.ultraGridData.TabIndex = 3;
            // 
            // ultraLabelInformation
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Top";
            this.ultraLabelInformation.Appearance = appearance2;
            this.ultraLabelInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabelInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.ultraLabelInformation.Location = new System.Drawing.Point(0, 0);
            this.ultraLabelInformation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ultraLabelInformation.Name = "ultraLabelInformation";
            this.ultraLabelInformation.Padding = new System.Drawing.Size(2, 2);
            this.ultraLabelInformation.Size = new System.Drawing.Size(734, 30);
            this.ultraLabelInformation.UseAppStyling = false;
            // 
            // ultraButtonNo
            // 
            this.ultraButtonNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonNo.Location = new System.Drawing.Point(575, 5);
            this.ultraButtonNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraButtonNo.Name = "ultraButtonNo";
            this.ultraButtonNo.Size = new System.Drawing.Size(133, 23);
            this.ultraButtonNo.TabIndex = 1;
            this.ultraButtonNo.Text = "No";
            this.ultraButtonNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // ultraButtonYes
            // 
            this.ultraButtonYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonYes.Location = new System.Drawing.Point(415, 5);
            this.ultraButtonYes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraButtonYes.Name = "ultraButtonYes";
            this.ultraButtonYes.Size = new System.Drawing.Size(133, 23);
            this.ultraButtonYes.TabIndex = 0;
            this.ultraButtonYes.Text = "Yes";
            this.ultraButtonYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // ultraButtonOK
            // 
            this.ultraButtonOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ultraButtonOK.Name = "ultraButtonOK";
            this.ultraButtonOK.Size = new System.Drawing.Size(133, 23);
            this.ultraButtonOK.TabIndex = 0;
            this.ultraButtonOK.Text = "OK";
            this.ultraButtonOK.Location = new Point(300, 5);
            this.ultraButtonOK.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // ultraPanelBottom
            // 
            this.ultraPanelBottom.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelBottom.Location = new System.Drawing.Point(8, 211);
            this.ultraPanelBottom.Margin = new System.Windows.Forms.Padding(2);
            this.ultraPanelBottom.Name = "ultraPanelBottom";
            this.ultraPanelBottom.Size = new System.Drawing.Size(733, 45);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _PopUp_UltraFormManager_Dock_Area_Left
            // 
            this._PopUp_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PopUp_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PopUp_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._PopUp_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PopUp_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._PopUp_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._PopUp_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._PopUp_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._PopUp_UltraFormManager_Dock_Area_Left.Name = "_PopUp_UltraFormManager_Dock_Area_Left";
            this._PopUp_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 114);
            // 
            // _PopUp_UltraFormManager_Dock_Area_Right
            // 
            this._PopUp_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PopUp_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PopUp_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._PopUp_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PopUp_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._PopUp_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._PopUp_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(742, 32);
            this._PopUp_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._PopUp_UltraFormManager_Dock_Area_Right.Name = "_PopUp_UltraFormManager_Dock_Area_Right";
            this._PopUp_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 114);
            // 
            // _PopUp_UltraFormManager_Dock_Area_Top
            // 
            this._PopUp_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PopUp_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PopUp_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._PopUp_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PopUp_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._PopUp_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PopUp_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._PopUp_UltraFormManager_Dock_Area_Top.Name = "_PopUp_UltraFormManager_Dock_Area_Top";
            this._PopUp_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(750, 32);
            // 
            // _PopUp_UltraFormManager_Dock_Area_Bottom
            // 
            this._PopUp_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PopUp_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PopUp_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._PopUp_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PopUp_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._PopUp_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._PopUp_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 146);
            this._PopUp_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._PopUp_UltraFormManager_Dock_Area_Bottom.Name = "_PopUp_UltraFormManager_Dock_Area_Bottom";
            this._PopUp_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(750, 8);
            // 
            // ultraPanelTop
            // 
            // 
            // ultraPanelTop.ClientArea
            // 
            this.ultraPanelTop.ClientArea.Controls.Add(this.ultraLabelInformation);
            this.ultraPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanelTop.Location = new System.Drawing.Point(8, 32);
            this.ultraPanelTop.Name = "ultraPanelTop";
            this.ultraPanelTop.Size = new System.Drawing.Size(734, 30);
            // 
            // DisplayItemsPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(750, 204);
            this.Controls.Add(this.ultraPanelMain);
            this.Controls.Add(this.ultraPanelBottom);
            this.Controls.Add(this.ultraPanelTop);
            this.Controls.Add(this._PopUp_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._PopUp_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._PopUp_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._PopUp_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(750, 204);
            this.Name = "DisplayItemsPopUp";
            this.ShowIcon = false;
            this.ultraPanelMain.ClientArea.ResumeLayout(false);
            this.ultraPanelMain.ResumeLayout(false);
            this.ultraPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridData)).EndInit();
            this.ultraPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ultraPanelTop.ClientArea.ResumeLayout(false);
            this.ultraPanelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanelMain;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridData;
        private Infragistics.Win.Misc.UltraButton ultraButtonNo;
        private Infragistics.Win.Misc.UltraButton ultraButtonYes;
        private Infragistics.Win.Misc.UltraButton ultraButtonOK;
        private Infragistics.Win.Misc.UltraLabel ultraLabelInformation;
        private Infragistics.Win.Misc.UltraPanel ultraPanelBottom;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PopUp_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PopUp_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PopUp_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PopUp_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraPanel ultraPanelTop;
    }
}