namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    partial class AlertHistoryPaging
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
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.lblRecordStatus = new Infragistics.Win.Misc.UltraLabel();
            this.btnLast = new Infragistics.Win.Misc.UltraButton();
            this.btnNext = new Infragistics.Win.Misc.UltraButton();
            this.btnPrevious = new Infragistics.Win.Misc.UltraButton();
            this.btnFirst = new Infragistics.Win.Misc.UltraButton();
            this.ultraNumericEditorPageSize = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditorPageSize)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraNumericEditorPageSize);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblRecordStatus);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnLast);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnNext);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnPrevious);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnFirst);
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(308, 28);
            this.ultraPanel1.TabIndex = 0;
            // 
            // lblRecordStatus
            // 
            this.lblRecordStatus.Location = new System.Drawing.Point(161, 1);
            this.lblRecordStatus.Name = "lblRecordStatus";
            this.lblRecordStatus.Size = new System.Drawing.Size(71, 23);
            this.lblRecordStatus.TabIndex = 4;
            this.lblRecordStatus.Text = "lbl";
            // 
            // btnLast
            // 
            this.btnLast.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLast.Location = new System.Drawing.Point(271, 2);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(34, 21);
            this.btnLast.TabIndex = 3;
            this.btnLast.Text = ">>";
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(231, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(34, 20);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(128, 2);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(32, 20);
            this.btnPrevious.TabIndex = 1;
            this.btnPrevious.Text = "<";
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFirst.Location = new System.Drawing.Point(92, 1);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(30, 21);
            this.btnFirst.TabIndex = 0;
            this.btnFirst.Text = "<<";
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // ultraNumericEditorPageSize
            // 
            this.ultraNumericEditorPageSize.Location = new System.Drawing.Point(3, 4);
            this.ultraNumericEditorPageSize.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.ultraNumericEditorPageSize.MaskInput = "nnnnnnnnn";
            this.ultraNumericEditorPageSize.MinValue = 1;
            this.ultraNumericEditorPageSize.Name = "ultraNumericEditorPageSize";
            this.ultraNumericEditorPageSize.PromptChar = ' ';
            this.ultraNumericEditorPageSize.Size = new System.Drawing.Size(57, 21);
            this.ultraNumericEditorPageSize.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraNumericEditorPageSize.TabIndex = 5;
            this.ultraNumericEditorPageSize.Value = 20;
            this.ultraNumericEditorPageSize.ValueChanged += new System.EventHandler(this.ultraNumericEditorPageSize_ValueChanged);
            // 
            // AlertHistoryPaging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "AlertHistoryPaging";
            this.Size = new System.Drawing.Size(308, 32);
            this.Load += new System.EventHandler(this.AlertHistoryPaging_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditorPageSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraLabel lblRecordStatus;
        private Infragistics.Win.Misc.UltraButton btnLast;
        private Infragistics.Win.Misc.UltraButton btnNext;
        private Infragistics.Win.Misc.UltraButton btnPrevious;
        private Infragistics.Win.Misc.UltraButton btnFirst;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumericEditorPageSize;
    }
}
