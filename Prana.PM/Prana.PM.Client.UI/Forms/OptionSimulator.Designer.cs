//namespace Prana.PM.Client.UI.Forms
//{
//    partial class OptionSimulator
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionSimulator));
//            this.ctrlOptionBooks1 = new Prana.PM.Client.UI.Controls.CtrlOptionBooks();
//            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
//            this.lblValues = new System.Windows.Forms.Label();
//            this.lblParameters = new System.Windows.Forms.Label();
//            this.lblVolatility = new System.Windows.Forms.Label();
//            this.lblInterestRate = new System.Windows.Forms.Label();
//            this.lblAssetPrice = new System.Windows.Forms.Label();
//            this.lblExpirationDate = new System.Windows.Forms.Label();
//            this.numEditorInterestRate = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
//            this.numEditorAssetPrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
//            this.numEditorDayToExpire = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
//            this.numEditorVolatility = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
//            this.btnClear = new Infragistics.Win.Misc.UltraButton();
//            this.btnReCalc = new Infragistics.Win.Misc.UltraButton();
//            this.btnImportPositions = new Infragistics.Win.Misc.UltraButton();
//            this.tableLayoutPanel1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorInterestRate)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorAssetPrice)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorDayToExpire)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorVolatility)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // ctrlOptionBooks1
//            // 
//            this.ctrlOptionBooks1.BackColor = System.Drawing.Color.Black;
//            this.ctrlOptionBooks1.Location = new System.Drawing.Point(12, 176);
//            this.ctrlOptionBooks1.Name = "ctrlOptionBooks1";
//            this.ctrlOptionBooks1.Size = new System.Drawing.Size(710, 409);
//            this.ctrlOptionBooks1.TabIndex = 0;
//            // 
//            // tableLayoutPanel1
//            // 
//            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetPartial;
//            this.tableLayoutPanel1.ColumnCount = 2;
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.5F));
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.5F));
//            this.tableLayoutPanel1.Controls.Add(this.lblValues, 1, 0);
//            this.tableLayoutPanel1.Controls.Add(this.lblParameters, 0, 0);
//            this.tableLayoutPanel1.Controls.Add(this.lblVolatility, 0, 1);
//            this.tableLayoutPanel1.Controls.Add(this.lblInterestRate, 0, 2);
//            this.tableLayoutPanel1.Controls.Add(this.lblAssetPrice, 0, 3);
//            this.tableLayoutPanel1.Controls.Add(this.lblExpirationDate, 0, 4);
//            this.tableLayoutPanel1.Controls.Add(this.numEditorInterestRate, 1, 2);
//            this.tableLayoutPanel1.Controls.Add(this.numEditorAssetPrice, 1, 3);
//            this.tableLayoutPanel1.Controls.Add(this.numEditorDayToExpire, 1, 4);
//            this.tableLayoutPanel1.Controls.Add(this.numEditorVolatility, 1, 1);
//            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 2);
//            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
//            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(1);
//            this.tableLayoutPanel1.RowCount = 6;
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
//            this.tableLayoutPanel1.Size = new System.Drawing.Size(488, 148);
//            this.tableLayoutPanel1.TabIndex = 1;
//            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
//            // 
//            // lblValues
//            // 
//            this.lblValues.AutoSize = true;
//            this.lblValues.Location = new System.Drawing.Point(250, 4);
//            this.lblValues.Name = "lblValues";
//            this.lblValues.Size = new System.Drawing.Size(39, 13);
//            this.lblValues.TabIndex = 1;
//            this.lblValues.Text = "Values";
//            // 
//            // lblParameters
//            // 
//            this.lblParameters.AutoSize = true;
//            this.lblParameters.Location = new System.Drawing.Point(7, 4);
//            this.lblParameters.Name = "lblParameters";
//            this.lblParameters.Size = new System.Drawing.Size(60, 13);
//            this.lblParameters.TabIndex = 0;
//            this.lblParameters.Text = "Parameters";
//            // 
//            // lblVolatility
//            // 
//            this.lblVolatility.AutoSize = true;
//            this.lblVolatility.Location = new System.Drawing.Point(7, 27);
//            this.lblVolatility.Name = "lblVolatility";
//            this.lblVolatility.Size = new System.Drawing.Size(45, 13);
//            this.lblVolatility.TabIndex = 2;
//            this.lblVolatility.Text = "Volatility";
//            // 
//            // lblInterestRate
//            // 
//            this.lblInterestRate.AutoSize = true;
//            this.lblInterestRate.Location = new System.Drawing.Point(7, 56);
//            this.lblInterestRate.Name = "lblInterestRate";
//            this.lblInterestRate.Size = new System.Drawing.Size(68, 13);
//            this.lblInterestRate.TabIndex = 3;
//            this.lblInterestRate.Text = "Interest Rate";
//            // 
//            // lblAssetPrice
//            // 
//            this.lblAssetPrice.AutoSize = true;
//            this.lblAssetPrice.Location = new System.Drawing.Point(7, 85);
//            this.lblAssetPrice.Name = "lblAssetPrice";
//            this.lblAssetPrice.Size = new System.Drawing.Size(60, 13);
//            this.lblAssetPrice.TabIndex = 4;
//            this.lblAssetPrice.Text = "Asset Price";
//            // 
//            // lblExpirationDate
//            // 
//            this.lblExpirationDate.AutoSize = true;
//            this.lblExpirationDate.Location = new System.Drawing.Point(7, 114);
//            this.lblExpirationDate.Name = "lblExpirationDate";
//            this.lblExpirationDate.Size = new System.Drawing.Size(92, 13);
//            this.lblExpirationDate.TabIndex = 5;
//            this.lblExpirationDate.Text = "Days to Expiration";
//            // 
//            // numEditorInterestRate
//            // 
//            this.numEditorInterestRate.Location = new System.Drawing.Point(250, 59);
//            this.numEditorInterestRate.Name = "numEditorInterestRate";
//            this.numEditorInterestRate.Size = new System.Drawing.Size(173, 21);
//            this.numEditorInterestRate.TabIndex = 7;
//            // 
//            // numEditorAssetPrice
//            // 
//            this.numEditorAssetPrice.Location = new System.Drawing.Point(250, 88);
//            this.numEditorAssetPrice.Name = "numEditorAssetPrice";
//            this.numEditorAssetPrice.Size = new System.Drawing.Size(173, 21);
//            this.numEditorAssetPrice.TabIndex = 8;
//            // 
//            // numEditorDayToExpire
//            // 
//            this.numEditorDayToExpire.Location = new System.Drawing.Point(250, 117);
//            this.numEditorDayToExpire.Name = "numEditorDayToExpire";
//            this.numEditorDayToExpire.Size = new System.Drawing.Size(173, 21);
//            this.numEditorDayToExpire.TabIndex = 9;
//            // 
//            // numEditorVolatility
//            // 
//            this.numEditorVolatility.Location = new System.Drawing.Point(250, 30);
//            this.numEditorVolatility.Name = "numEditorVolatility";
//            this.numEditorVolatility.Size = new System.Drawing.Size(173, 21);
//            this.numEditorVolatility.TabIndex = 6;
//            // 
//            // btnClear
//            // 
//            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
//            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
//            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
//            this.btnClear.Appearance = appearance1;
//            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
//            this.btnClear.Location = new System.Drawing.Point(178, 150);
//            this.btnClear.Name = "btnClear";
//            this.btnClear.ShowFocusRect = false;
//            this.btnClear.Size = new System.Drawing.Size(75, 23);
//            this.btnClear.TabIndex = 10;
//            // 
//            // btnReCalc
//            // 
//            this.btnReCalc.Location = new System.Drawing.Point(262, 150);
//            this.btnReCalc.Name = "btnReCalc";
//            this.btnReCalc.Size = new System.Drawing.Size(75, 23);
//            this.btnReCalc.TabIndex = 11;
//            this.btnReCalc.Text = "Re-Calc";
//            // 
//            // btnImportPositions
//            // 
//            this.btnImportPositions.Location = new System.Drawing.Point(594, 90);
//            this.btnImportPositions.Name = "btnImportPositions";
//            this.btnImportPositions.Size = new System.Drawing.Size(95, 23);
//            this.btnImportPositions.TabIndex = 13;
//            this.btnImportPositions.Text = "Import Positions";
//            this.btnImportPositions.Click += new System.EventHandler(this.btnImportPositions_Click);
//            // 
//            // OptionSimulator
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(736, 613);
//            this.Controls.Add(this.btnImportPositions);
//            this.Controls.Add(this.btnReCalc);
//            this.Controls.Add(this.tableLayoutPanel1);
//            this.Controls.Add(this.ctrlOptionBooks1);
//            this.Controls.Add(this.btnClear);
//            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
//            this.Name = "OptionSimulator";
//            this.Text = "OptionSimulator";
//            this.tableLayoutPanel1.ResumeLayout(false);
//            this.tableLayoutPanel1.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorInterestRate)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorAssetPrice)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorDayToExpire)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numEditorVolatility)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private Prana.PM.Client.UI.Controls.CtrlOptionBooks ctrlOptionBooks1;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
//        private System.Windows.Forms.Label lblValues;
//        private System.Windows.Forms.Label lblParameters;
//        private System.Windows.Forms.Label lblVolatility;
//        private System.Windows.Forms.Label lblInterestRate;
//        private System.Windows.Forms.Label lblAssetPrice;
//        private System.Windows.Forms.Label lblExpirationDate;
//        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numEditorVolatility;
//        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numEditorInterestRate;
//        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numEditorAssetPrice;
//        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numEditorDayToExpire;
//        private Infragistics.Win.Misc.UltraButton btnClear;
//        private Infragistics.Win.Misc.UltraButton btnReCalc;
//        private Infragistics.Win.Misc.UltraButton btnImportPositions;
//    }
//}