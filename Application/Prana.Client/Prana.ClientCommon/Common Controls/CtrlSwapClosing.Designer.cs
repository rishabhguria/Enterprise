namespace Prana.ClientCommon
{
    partial class CtrlSwapClosing
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblSettlementDetails = new System.Windows.Forms.Label();
            this.pnlOriginalTDate = new System.Windows.Forms.Panel();
            this.dtPkrClosingDate = new System.Windows.Forms.DateTimePicker();
            this.lblClosingDate = new System.Windows.Forms.Label();
            this.pnlCostBasis = new System.Windows.Forms.Panel();
            this.lblClosingCostBasis = new System.Windows.Forms.Label();
            this.spnrClosingCostBasis = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.lblRolloverDetails = new System.Windows.Forms.Label();
            this.ctrlSwapParameters1 = new Prana.ClientCommon.CtrlSwapParameters();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlOriginalTDate.SuspendLayout();
            this.pnlCostBasis.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblSettlementDetails);
            this.splitContainer1.Panel1.Controls.Add(this.pnlOriginalTDate);
            this.splitContainer1.Panel1.Controls.Add(this.pnlCostBasis);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.lblRolloverDetails);
            this.splitContainer1.Panel2.Controls.Add(this.ctrlSwapParameters1);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(880, 251);
            this.splitContainer1.SplitterDistance = 98;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // lblSettlementDetails
            // 
            this.lblSettlementDetails.AutoSize = true;
            this.lblSettlementDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettlementDetails.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSettlementDetails.Location = new System.Drawing.Point(3, 12);
            this.lblSettlementDetails.Name = "lblSettlementDetails";
            this.lblSettlementDetails.Size = new System.Drawing.Size(114, 13);
            this.lblSettlementDetails.TabIndex = 10;
            this.lblSettlementDetails.Text = "Settlement Details:";
            // 
            // pnlOriginalTDate
            // 
            this.pnlOriginalTDate.Controls.Add(this.dtPkrClosingDate);
            this.pnlOriginalTDate.Controls.Add(this.lblClosingDate);
            this.pnlOriginalTDate.Location = new System.Drawing.Point(119, 12);
            this.pnlOriginalTDate.Name = "pnlOriginalTDate";
            this.pnlOriginalTDate.Size = new System.Drawing.Size(103, 39);
            this.pnlOriginalTDate.TabIndex = 8;
            // 
            // dtPkrClosingDate
            // 
            this.dtPkrClosingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPkrClosingDate.Location = new System.Drawing.Point(3, 16);
            this.dtPkrClosingDate.MinDate = new System.DateTime(1800, 1, 1, 0, 0, 0, 0);
            this.dtPkrClosingDate.Name = "dtPkrClosingDate";
            this.dtPkrClosingDate.Size = new System.Drawing.Size(99, 20);
            this.dtPkrClosingDate.TabIndex = 1;
            // 
            // lblClosingDate
            // 
            this.lblClosingDate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblClosingDate.ForeColor = System.Drawing.Color.Black;
            this.lblClosingDate.Location = new System.Drawing.Point(4, 1);
            this.lblClosingDate.Name = "lblClosingDate";
            this.lblClosingDate.Size = new System.Drawing.Size(84, 13);
            this.lblClosingDate.TabIndex = 0;
            this.lblClosingDate.Text = "Closing Date";
            this.lblClosingDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlCostBasis
            // 
            this.pnlCostBasis.Controls.Add(this.lblClosingCostBasis);
            this.pnlCostBasis.Controls.Add(this.spnrClosingCostBasis);
            this.pnlCostBasis.Location = new System.Drawing.Point(249, 11);
            this.pnlCostBasis.Name = "pnlCostBasis";
            this.pnlCostBasis.Size = new System.Drawing.Size(103, 39);
            this.pnlCostBasis.TabIndex = 9;
            // 
            // lblClosingCostBasis
            // 
            this.lblClosingCostBasis.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblClosingCostBasis.ForeColor = System.Drawing.Color.Black;
            this.lblClosingCostBasis.Location = new System.Drawing.Point(0, 1);
            this.lblClosingCostBasis.Name = "lblClosingCostBasis";
            this.lblClosingCostBasis.Size = new System.Drawing.Size(100, 13);
            this.lblClosingCostBasis.TabIndex = 0;
            this.lblClosingCostBasis.Text = "Closing Cost Basis";
            this.lblClosingCostBasis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spnrClosingCostBasis
            // 
            this.spnrClosingCostBasis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnrClosingCostBasis.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.Numeric;
            this.spnrClosingCostBasis.DecimalEntered = false;
            this.spnrClosingCostBasis.DecimalPoints = 2147483647;
            this.spnrClosingCostBasis.Increment = 0.01D;
            this.spnrClosingCostBasis.Location = new System.Drawing.Point(3, 17);
            this.spnrClosingCostBasis.MaxValue = 99999D;
            this.spnrClosingCostBasis.MinValue = 0D;
            this.spnrClosingCostBasis.Name = "spnrClosingCostBasis";
            this.spnrClosingCostBasis.Size = new System.Drawing.Size(97, 20);
            this.spnrClosingCostBasis.TabIndex = 2;
            this.spnrClosingCostBasis.Value = 0D;
            // 
            // lblRolloverDetails
            // 
            this.lblRolloverDetails.AutoSize = true;
            this.lblRolloverDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRolloverDetails.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblRolloverDetails.Location = new System.Drawing.Point(3, 4);
            this.lblRolloverDetails.Name = "lblRolloverDetails";
            this.lblRolloverDetails.Size = new System.Drawing.Size(101, 13);
            this.lblRolloverDetails.TabIndex = 11;
            this.lblRolloverDetails.Text = "Rollover Details:";
            // 
            // ctrlSwapParameters1
            // 
            this.ctrlSwapParameters1.BackColor = System.Drawing.Color.Transparent;
            this.ctrlSwapParameters1.IsPreTradeSwap = false;
            this.ctrlSwapParameters1.Location = new System.Drawing.Point(6, 20);
            this.ctrlSwapParameters1.Name = "ctrlSwapParameters1";
            this.ctrlSwapParameters1.Size = new System.Drawing.Size(871, 129);
            this.ctrlSwapParameters1.TabIndex = 0;
            // 
            // CtrlSwapClosing
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "CtrlSwapClosing";
            this.Size = new System.Drawing.Size(880, 251);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlOriginalTDate.ResumeLayout(false);
            this.pnlCostBasis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlOriginalTDate;
        private System.Windows.Forms.DateTimePicker dtPkrClosingDate;
        private System.Windows.Forms.Label lblClosingDate;
        private System.Windows.Forms.Panel pnlCostBasis;
        private System.Windows.Forms.Label lblClosingCostBasis;
        private Prana.Utilities.UI.UIUtilities.Spinner spnrClosingCostBasis;
        private System.Windows.Forms.Label lblSettlementDetails;
        private System.Windows.Forms.Label lblRolloverDetails;
        private CtrlSwapParameters ctrlSwapParameters1;
    }
}
