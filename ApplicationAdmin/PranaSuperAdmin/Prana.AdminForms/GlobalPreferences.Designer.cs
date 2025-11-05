namespace Prana.AdminForms
{
    partial class GlobalPreferences
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
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.GlobalPreferences_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.chkBoxSwapCommission = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbAutoCalculateFields = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblAutoCalculate = new Infragistics.Win.Misc.UltraLabel();
            this.lbPricingSource = new Infragistics.Win.Misc.UltraLabel();
            this.btSave = new Infragistics.Win.Misc.UltraButton();
            this.cmbPricingSource = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this._GlobalPreferences_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._GlobalPreferences_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._GlobalPreferences_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.GlobalPreferences_Fill_Panel.ClientArea.SuspendLayout();
            this.GlobalPreferences_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxSwapCommission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAutoCalculateFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPricingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            // 
            // GlobalPreferences_Fill_Panel
            // 
            // 
            // GlobalPreferences_Fill_Panel.ClientArea
            // 
            this.GlobalPreferences_Fill_Panel.ClientArea.Controls.Add(this.chkBoxSwapCommission);
            this.GlobalPreferences_Fill_Panel.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.GlobalPreferences_Fill_Panel.ClientArea.Controls.Add(this.lbPricingSource);
            this.GlobalPreferences_Fill_Panel.ClientArea.Controls.Add(this.btSave);
            this.GlobalPreferences_Fill_Panel.ClientArea.Controls.Add(this.cmbPricingSource);
            this.GlobalPreferences_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.GlobalPreferences_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GlobalPreferences_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.GlobalPreferences_Fill_Panel.Name = "GlobalPreferences_Fill_Panel";
            this.GlobalPreferences_Fill_Panel.Size = new System.Drawing.Size(334, 182);
            this.GlobalPreferences_Fill_Panel.TabIndex = 0;
            // 
            // chkBoxSwapCommission
            // 
            this.chkBoxSwapCommission.Location = new System.Drawing.Point(24, 114);
            this.chkBoxSwapCommission.Name = "chkBoxSwapCommission";
            this.chkBoxSwapCommission.Size = new System.Drawing.Size(251, 31);
            this.chkBoxSwapCommission.TabIndex = 6;
            this.chkBoxSwapCommission.Text = "      No Commission/Fees for Swaps";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.cmbAutoCalculateFields);
            this.ultraGroupBox1.Controls.Add(this.lblAutoCalculate);
            this.ultraGroupBox1.Location = new System.Drawing.Point(12, 41);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(310, 67);
            this.ultraGroupBox1.TabIndex = 5;
            this.ultraGroupBox1.Text = "Settlement Fields Calculation Preference";
            // 
            // cmbAutoCalculateFields
            // 
            this.cmbAutoCalculateFields.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest;
            this.cmbAutoCalculateFields.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbAutoCalculateFields.Location = new System.Drawing.Point(138, 29);
            this.cmbAutoCalculateFields.Name = "cmbAutoCalculateFields";
            this.cmbAutoCalculateFields.Size = new System.Drawing.Size(144, 21);
            this.cmbAutoCalculateFields.TabIndex = 3;
            // 
            // lblAutoCalculate
            // 
            this.lblAutoCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblAutoCalculate.Location = new System.Drawing.Point(12, 33);
            this.lblAutoCalculate.Name = "lblAutoCalculate";
            this.lblAutoCalculate.Size = new System.Drawing.Size(107, 21);
            this.lblAutoCalculate.TabIndex = 4;
            this.lblAutoCalculate.Text = "Auto Calculate Field";
            // 
            // lbPricingSource
            // 
            this.lbPricingSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbPricingSource.Location = new System.Drawing.Point(24, 16);
            this.lbPricingSource.Name = "lbPricingSource";
            this.lbPricingSource.Size = new System.Drawing.Size(100, 23);
            this.lbPricingSource.TabIndex = 2;
            this.lbPricingSource.Text = "Pricing Source";
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(129, 149);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Save";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // cmbPricingSource
            // 
            this.cmbPricingSource.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest;
            this.cmbPricingSource.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbPricingSource.Location = new System.Drawing.Point(150, 12);
            this.cmbPricingSource.Name = "cmbPricingSource";
            this.cmbPricingSource.Size = new System.Drawing.Size(144, 21);
            this.cmbPricingSource.TabIndex = 0;
            // 
            // _GlobalPreferences_Toolbars_Dock_Area_Left
            // 
            this._GlobalPreferences_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GlobalPreferences_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._GlobalPreferences_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._GlobalPreferences_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GlobalPreferences_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._GlobalPreferences_Toolbars_Dock_Area_Left.Name = "_GlobalPreferences_Toolbars_Dock_Area_Left";
            this._GlobalPreferences_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 182);
            this._GlobalPreferences_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _GlobalPreferences_Toolbars_Dock_Area_Right
            // 
            this._GlobalPreferences_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GlobalPreferences_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._GlobalPreferences_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._GlobalPreferences_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GlobalPreferences_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(334, 0);
            this._GlobalPreferences_Toolbars_Dock_Area_Right.Name = "_GlobalPreferences_Toolbars_Dock_Area_Right";
            this._GlobalPreferences_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 182);
            this._GlobalPreferences_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _GlobalPreferences_Toolbars_Dock_Area_Top
            // 
            this._GlobalPreferences_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GlobalPreferences_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._GlobalPreferences_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._GlobalPreferences_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GlobalPreferences_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._GlobalPreferences_Toolbars_Dock_Area_Top.Name = "_GlobalPreferences_Toolbars_Dock_Area_Top";
            this._GlobalPreferences_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(334, 0);
            this._GlobalPreferences_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _GlobalPreferences_Toolbars_Dock_Area_Bottom
            // 
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 182);
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.Name = "_GlobalPreferences_Toolbars_Dock_Area_Bottom";
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(334, 0);
            this._GlobalPreferences_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // GlobalPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 182);
            this.Controls.Add(this.GlobalPreferences_Fill_Panel);
            this.Controls.Add(this._GlobalPreferences_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._GlobalPreferences_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._GlobalPreferences_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._GlobalPreferences_Toolbars_Dock_Area_Top);
            this.MaximumSize = new System.Drawing.Size(360, 220);
            this.MinimumSize = new System.Drawing.Size(350, 216);
            this.Name = "GlobalPreferences";
            this.ShowInTaskbar = false;
            this.Text = "GlobalPreferences";
            this.Load += new System.EventHandler(this.GlobalPreferences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.GlobalPreferences_Fill_Panel.ClientArea.ResumeLayout(false);
            this.GlobalPreferences_Fill_Panel.ClientArea.PerformLayout();
            this.GlobalPreferences_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxSwapCommission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAutoCalculateFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPricingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel GlobalPreferences_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _GlobalPreferences_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _GlobalPreferences_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _GlobalPreferences_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _GlobalPreferences_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbPricingSource;
        private Infragistics.Win.Misc.UltraButton btSave;
        private Infragistics.Win.Misc.UltraLabel lbPricingSource;
        private Infragistics.Win.Misc.UltraLabel lblAutoCalculate;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAutoCalculateFields;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxSwapCommission;
    }
}