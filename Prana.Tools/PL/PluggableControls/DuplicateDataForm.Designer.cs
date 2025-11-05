namespace Prana.Tools
{
    partial class DuplicateDataForm
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
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            this.grdMessages = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnXml = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // grdMessages
            // 
            this.grdMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance80.BackColor = System.Drawing.Color.Black;
            appearance80.BackColor2 = System.Drawing.Color.Black;
            appearance80.BorderColor = System.Drawing.Color.Black;
            appearance80.FontData.BoldAsString = "True";
            appearance80.FontData.Name = "Tahoma";
            appearance80.FontData.SizeInPoints = 8.25F;
            appearance80.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.grdMessages.DisplayLayout.Appearance = appearance80;
            this.grdMessages.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdMessages.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance81.BackColor = System.Drawing.Color.White;
            this.grdMessages.DisplayLayout.CaptionAppearance = appearance81;
            this.grdMessages.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.GroupByBox.Hidden = true;
            this.grdMessages.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMessages.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdMessages.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup;
            this.grdMessages.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdMessages.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdMessages.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            appearance82.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance82.FontData.BoldAsString = "True";
            appearance82.FontData.Name = "Tahoma";
            appearance82.FontData.SizeInPoints = 8.25F;
            this.grdMessages.DisplayLayout.Override.HeaderAppearance = appearance82;
            this.grdMessages.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdMessages.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdMessages.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdMessages.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdMessages.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMessages.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMessages.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdMessages.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdMessages.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grdMessages.Location = new System.Drawing.Point(-1, 2);
            this.grdMessages.Name = "grdMessages";
            this.grdMessages.Size = new System.Drawing.Size(573, 245);
            this.grdMessages.TabIndex = 96;
            this.grdMessages.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExcel.BackColor = System.Drawing.Color.SeaGreen;
            this.btnExcel.Location = new System.Drawing.Point(182, 253);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 104;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnXml
            // 
            this.btnXml.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnXml.BackColor = System.Drawing.Color.SeaGreen;
            this.btnXml.Location = new System.Drawing.Point(275, 253);
            this.btnXml.Name = "btnXml";
            this.btnXml.Size = new System.Drawing.Size(75, 23);
            this.btnXml.TabIndex = 105;
            this.btnXml.Text = "Xml";
            this.btnXml.UseVisualStyleBackColor = false;
            this.btnXml.Click += new System.EventHandler(this.btnXml_Click);
            // 
            // DuplicateDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 278);
            this.Controls.Add(this.btnXml);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.grdMessages);
            this.Name = "DuplicateDataForm";
            this.Text = "DuplicateDataForm";
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdMessages;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnXml;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}