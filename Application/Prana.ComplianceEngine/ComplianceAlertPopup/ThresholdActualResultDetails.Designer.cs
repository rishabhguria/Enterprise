using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    partial class ThresholdActualResultDetails
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.gridViewThresholdActualResult = new PranaUltraGrid();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewThresholdActualResult)).BeginInit();

            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.gridViewThresholdActualResult);
            this.ultraPanel1.Location = new System.Drawing.Point(3, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(498, 282);
            this.ultraPanel1.TabIndex = 0;

            // 
            // gridViewThresholdActualResult
            // 
            this.gridViewThresholdActualResult.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.gridViewThresholdActualResult.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.gridViewThresholdActualResult.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.gridViewThresholdActualResult.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.gridViewThresholdActualResult.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.gridViewThresholdActualResult.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            this.gridViewThresholdActualResult.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridViewThresholdActualResult.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridViewThresholdActualResult.Location = new System.Drawing.Point(0, 5);
            this.gridViewThresholdActualResult.Name = "gridViewThresholdActualResult";
            this.gridViewThresholdActualResult.Size = new System.Drawing.Size(480, 200);
            this.gridViewThresholdActualResult.TabIndex = 5;
            this.gridViewThresholdActualResult.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridViewThresholdActualResult_InitializeLayout);

            // 
            // ThresholdActualResultDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ThresholdActualResultDetails";
            this.Size = new System.Drawing.Size(503, 283);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewThresholdActualResult)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private PranaUltraGrid gridViewThresholdActualResult;
    }
}
