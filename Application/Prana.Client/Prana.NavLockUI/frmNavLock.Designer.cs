namespace Prana.NavLockUI
{
    partial class frmNavLock
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
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pranaUltraGrid1 = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.dtLockDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnAddLock = new Infragistics.Win.Misc.UltraButton();
            this._AboutPrana_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pranaUltraGrid1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtLockDate)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.pranaUltraGrid1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 32);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(494, 410);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pranaUltraGrid1
            // 
            this.pranaUltraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.pranaUltraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.pranaUltraGrid1.DisplayLayout.MaxColScrollRegions = 1;
            this.pranaUltraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
            this.pranaUltraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.pranaUltraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.pranaUltraGrid1.DisplayLayout.Override.CellPadding = 0;
            this.pranaUltraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.pranaUltraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.pranaUltraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.pranaUltraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.pranaUltraGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pranaUltraGrid1.Location = new System.Drawing.Point(0, 41);
            this.pranaUltraGrid1.Margin = new System.Windows.Forms.Padding(0);
            this.pranaUltraGrid1.Name = "pranaUltraGrid1";
            this.pranaUltraGrid1.Size = new System.Drawing.Size(494, 369);
            this.pranaUltraGrid1.TabIndex = 0;
            this.pranaUltraGrid1.Text = "pranaUltraGrid1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.Controls.Add(this.ultraLabel1);
            this.flowLayoutPanel1.Controls.Add(this.dtLockDate);
            this.flowLayoutPanel1.Controls.Add(this.btnAddLock);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(488, 33);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraLabel1.Location = new System.Drawing.Point(3, 3);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(148, 23);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Lock all accounts as of Date: ";
            // 
            // dtLockDate
            // 
            this.dtLockDate.Location = new System.Drawing.Point(157, 3);
            this.dtLockDate.Name = "dtLockDate";
            this.dtLockDate.Size = new System.Drawing.Size(124, 21);
            this.dtLockDate.TabIndex = 26;
            // 
            // btnAddLock
            // 
            this.btnAddLock.Location = new System.Drawing.Point(287, 3);
            this.btnAddLock.Name = "btnAddLock";
            this.btnAddLock.Size = new System.Drawing.Size(75, 23);
            this.btnAddLock.TabIndex = 27;
            this.btnAddLock.Text = "Add Lock";
            this.btnAddLock.Click += new System.EventHandler(this.btnAddLock_Click);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Left
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Name = "_AboutPrana_UltraFormManager_Dock_Area_Left";
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 410);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Right
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(502, 32);
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Name = "_AboutPrana_UltraFormManager_Dock_Area_Right";
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 410);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Top
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Name = "_AboutPrana_UltraFormManager_Dock_Area_Top";
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(510, 32);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Bottom
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 442);
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Name = "_AboutPrana_UltraFormManager_Dock_Area_Bottom";
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(510, 8);
            // 
            // frmNavLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(510, 650);
            this.MinimumSize = new System.Drawing.Size(510, 450);
            this.Name = "frmNavLock";
            this.Text = "NAV Lock";
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pranaUltraGrid1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtLockDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Utilities.UI.UIUtilities.PranaUltraGrid pranaUltraGrid1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtLockDate;
        private Infragistics.Win.Misc.UltraButton btnAddLock;
    }
}