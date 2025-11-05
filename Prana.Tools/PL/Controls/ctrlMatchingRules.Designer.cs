namespace Prana.Tools
{
    partial class ctrlMatchingRules
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
                if (_template != null)
                {
                    _template.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grdMatchingRules = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grdMatchingRules)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdMatchingRules
            // 
            this.grdMatchingRules.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdMatchingRules.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdMatchingRules.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMatchingRules.DisplayLayout.GroupByBox.Hidden = true;
            this.grdMatchingRules.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMatchingRules.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdMatchingRules.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdMatchingRules.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdMatchingRules.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdMatchingRules.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdMatchingRules.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdMatchingRules.DisplayLayout.Override.CellPadding = 0;
            this.grdMatchingRules.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.grdMatchingRules.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdMatchingRules.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdMatchingRules.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMatchingRules.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMatchingRules.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdMatchingRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMatchingRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdMatchingRules.Location = new System.Drawing.Point(0, 0);
            this.grdMatchingRules.Name = "grdMatchingRules";
            this.grdMatchingRules.Size = new System.Drawing.Size(754, 356);
            this.grdMatchingRules.TabIndex = 2;
            this.grdMatchingRules.Text = "ultraGrid1";
            this.grdMatchingRules.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grdMatchingRules.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMatchingRules_AfterCellUpdate);
            this.grdMatchingRules.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdMatchingRules_InitializeLayout);
            this.grdMatchingRules.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdMatchingRules_InitializeRow);
            this.grdMatchingRules.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMatchingRules_CellChange);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.grdMatchingRules);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(754, 356);
            this.ultraPanel1.TabIndex = 3;
            // 
            // ctrlMatchingRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlMatchingRules";
            this.Size = new System.Drawing.Size(754, 356);
            ((System.ComponentModel.ISupportInitialize)(this.grdMatchingRules)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdMatchingRules;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;

    }
}
