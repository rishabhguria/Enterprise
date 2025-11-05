using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.ThirdPartyUI;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls.ThirdParty
{
    partial class TimedBatchSchedulingForm
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
            this._TimedBatch_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TimedBatch_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TimedBatch_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraGridBagLayoutManager1 = new Infragistics.Win.Misc.UltraGridBagLayoutManager(this.components);
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grdTimedBatch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTimedBatch)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _TimedBatch_UltraFormManager_Dock_Area_Left
            // 
            this._TimedBatch_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TimedBatch_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TimedBatch_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._TimedBatch_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TimedBatch_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._TimedBatch_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._TimedBatch_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._TimedBatch_UltraFormManager_Dock_Area_Left.Name = "_TimedBatch_UltraFormManager_Dock_Area_Left";
            this._TimedBatch_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 327);
            // 
            // _TimedBatch_UltraFormManager_Dock_Area_Right
            // 
            this._TimedBatch_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TimedBatch_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TimedBatch_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._TimedBatch_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TimedBatch_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._TimedBatch_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._TimedBatch_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(445, 32);
            this._TimedBatch_UltraFormManager_Dock_Area_Right.Name = "_TimedBatch_UltraFormManager_Dock_Area_Right";
            this._TimedBatch_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 327);
            // 
            // _TimedBatch_UltraFormManager_Dock_Area_Top
            // 
            this._TimedBatch_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TimedBatch_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TimedBatch_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._TimedBatch_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TimedBatch_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._TimedBatch_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._TimedBatch_UltraFormManager_Dock_Area_Top.Name = "_TimedBatch_UltraFormManager_Dock_Area_Top";
            this._TimedBatch_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(453, 32);
            // 
            // _TimedBatch_UltraFormManager_Dock_Area_Bottom
            // 
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 359);
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.Name = "_TimedBatch_UltraFormManager_Dock_Area_Bottom";
            this._TimedBatch_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(453, 8);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnAdd.BackgroundImage = global::Prana.Admin.Controls.Properties.Resources.btn_add;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.Location = new System.Drawing.Point(73, 295);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(212, 225, 208);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(112, 122, 113);
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(190, 295);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "OK";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCancel.BackgroundImage = global::Prana.Admin.Controls.Properties.Resources.btn_cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCancel.Location = new System.Drawing.Point(300, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += (sender, e) => this.Close();
            // 
            // grdTimedBatch
            // 
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.grdTimedBatch.DisplayLayout.Appearance = appearance1;
            this.grdTimedBatch.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdTimedBatch.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdTimedBatch.DisplayLayout.Override.DefaultRowHeight = 25;
            this.grdTimedBatch.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            this.grdTimedBatch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdTimedBatch.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            this.grdTimedBatch.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdTimedBatch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdTimedBatch.Location = new System.Drawing.Point(7, 9);
            this.grdTimedBatch.Name = "grdTimedBatch";
            this.grdTimedBatch.Size = new System.Drawing.Size(422, 275);
            this.grdTimedBatch.TabIndex = 5;
            this.grdTimedBatch.UseAppStyling = false;
            this.grdTimedBatch.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.grdTimedBatch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.GrdTimedBatch_InitializeRow);
            this.grdTimedBatch.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.GrdTimedBatch_ClickCellButton);
            // 
            // TimedBatchSchedulingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(453, 367);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grdTimedBatch);
            this.Controls.Add(this._TimedBatch_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._TimedBatch_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._TimedBatch_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._TimedBatch_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(453, 367);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(453, 367);
            this.Name = "TimedBatchSchedulingForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Timed Batch Scheduling FIX";
            this.Load += new System.EventHandler(this.TimedBatch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTimedBatch)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TimedBatch_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TimedBatch_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TimedBatch_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TimedBatch_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraGridBagLayoutManager ultraGridBagLayoutManager1;
        private Button btnAdd;
        private Button btnSave;
        private Button btnCancel;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdTimedBatch;
    }
}

