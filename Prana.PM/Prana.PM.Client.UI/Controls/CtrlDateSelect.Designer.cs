namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlDateSelect
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton5 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            this.rdHistoricalDate = new System.Windows.Forms.RadioButton();
            this.rdCurrentDate = new System.Windows.Forms.RadioButton();
            this.lblCloseDate = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCloseDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCloseDate)).BeginInit();
            this.SuspendLayout();
            // 
            // rdHistoricalDate
            // 
            this.rdHistoricalDate.AutoSize = true;
            this.rdHistoricalDate.Location = new System.Drawing.Point(113, 36);
            this.rdHistoricalDate.Name = "rdHistoricalDate";
            this.rdHistoricalDate.Size = new System.Drawing.Size(68, 17);
            this.rdHistoricalDate.TabIndex = 24;
            this.rdHistoricalDate.TabStop = true;
            this.rdHistoricalDate.Text = "Historical";
            this.rdHistoricalDate.UseVisualStyleBackColor = true;
            this.rdHistoricalDate.CheckedChanged += new System.EventHandler(this.rdHistoricalDate_CheckedChanged);
            // 
            // rdCurrentDate
            // 
            this.rdCurrentDate.AutoSize = true;
            this.rdCurrentDate.Location = new System.Drawing.Point(113, 13);
            this.rdCurrentDate.Name = "rdCurrentDate";
            this.rdCurrentDate.Size = new System.Drawing.Size(59, 17);
            this.rdCurrentDate.TabIndex = 25;
            this.rdCurrentDate.TabStop = true;
            this.rdCurrentDate.Text = "Current";
            this.rdCurrentDate.UseVisualStyleBackColor = true;
            this.rdCurrentDate.CheckedChanged += new System.EventHandler(this.rdCurrentDate_CheckedChanged);
            // 
            // lblCloseDate
            // 
            this.lblCloseDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCloseDate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCloseDate.Location = new System.Drawing.Point(21, 13);
            this.lblCloseDate.Name = "lblCloseDate";
            this.lblCloseDate.Size = new System.Drawing.Size(60, 15);
            this.lblCloseDate.TabIndex = 22;
            this.lblCloseDate.Text = "Close Date";
            // 
            // cmbCloseDate
            // 
            this.cmbCloseDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbCloseDate.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCloseDate.DateButtons.Add(dateButton5);
            this.cmbCloseDate.Location = new System.Drawing.Point(185, 32);
            this.cmbCloseDate.Name = "cmbCloseDate";
            this.cmbCloseDate.NonAutoSizeHeight = 21;
            this.cmbCloseDate.Size = new System.Drawing.Size(99, 21);
            this.cmbCloseDate.TabIndex = 23;
            // 
            // CtrlDateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdHistoricalDate);
            this.Controls.Add(this.rdCurrentDate);
            this.Controls.Add(this.lblCloseDate);
            this.Controls.Add(this.cmbCloseDate);
            this.Name = "CtrlDateSelect";
            this.Size = new System.Drawing.Size(287, 59);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCloseDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdHistoricalDate;
        private System.Windows.Forms.RadioButton rdCurrentDate;
        private Infragistics.Win.Misc.UltraLabel lblCloseDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo cmbCloseDate;
    }
}
