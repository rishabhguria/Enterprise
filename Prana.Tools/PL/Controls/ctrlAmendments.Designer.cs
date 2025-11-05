namespace Prana.Tools
{
    partial class ctrlAmendments
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnPostTransaction = new Infragistics.Win.Misc.UltraButton();
            this.lblTemplate = new Infragistics.Win.Misc.UltraLabel();
            this.lblReconType = new Infragistics.Win.Misc.UltraLabel();
            this.lblClient = new Infragistics.Win.Misc.UltraLabel();
            this.tbClient = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.tbReconType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.tbTemplate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ctrlReconOutput1 = new Prana.Tools.ctrlReconOutput();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbReconType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(704, 412);
            this.ultraPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ultraGroupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ctrlReconOutput1);
            this.splitContainer1.Size = new System.Drawing.Size(704, 412);
            this.splitContainer1.SplitterDistance = 59;
            this.splitContainer1.TabIndex = 0;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.tbTemplate);
            this.ultraGroupBox1.Controls.Add(this.tbReconType);
            this.ultraGroupBox1.Controls.Add(this.tbClient);
            this.ultraGroupBox1.Controls.Add(this.lblTemplate);
            this.ultraGroupBox1.Controls.Add(this.lblReconType);
            this.ultraGroupBox1.Controls.Add(this.lblClient);
            this.ultraGroupBox1.Controls.Add(this.btnPostTransaction);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(704, 59);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "Amendments";
            // 
            // btnPostTransaction
            // 
            this.btnPostTransaction.Location = new System.Drawing.Point(590, 21);
            this.btnPostTransaction.Name = "btnPostTransaction";
            this.btnPostTransaction.Size = new System.Drawing.Size(108, 23);
            this.btnPostTransaction.TabIndex = 0;
            this.btnPostTransaction.Text = "Post Transaction";
            this.btnPostTransaction.Click += new System.EventHandler(this.btnPostTransaction_Click);
            // 
            // lblTemplate
            // 
            this.lblTemplate.Location = new System.Drawing.Point(388, 23);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(68, 18);
            this.lblTemplate.TabIndex = 10;
            this.lblTemplate.Text = "Template";
            // 
            // lblReconType
            // 
            this.lblReconType.Location = new System.Drawing.Point(182, 23);
            this.lblReconType.Name = "lblReconType";
            this.lblReconType.Size = new System.Drawing.Size(76, 18);
            this.lblReconType.TabIndex = 7;
            this.lblReconType.Text = "Recon Type";
            // 
            // lblClient
            // 
            this.lblClient.Location = new System.Drawing.Point(6, 23);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(46, 18);
            this.lblClient.TabIndex = 6;
            this.lblClient.Text = "Client";
            // 
            // tbClient
            // 
            this.tbClient.Enabled = false;
            this.tbClient.Location = new System.Drawing.Point(47, 19);
            this.tbClient.Name = "tbClient";
            this.tbClient.Size = new System.Drawing.Size(100, 21);
            this.tbClient.TabIndex = 11;
            this.tbClient.Text = "Client";
            // 
            // tbReconType
            // 
            this.tbReconType.Enabled = false;
            this.tbReconType.Location = new System.Drawing.Point(252, 19);
            this.tbReconType.Name = "tbReconType";
            this.tbReconType.Size = new System.Drawing.Size(100, 21);
            this.tbReconType.TabIndex = 12;
            this.tbReconType.Text = "Recon Type";
            // 
            // tbTemplateName
            // 
            this.tbTemplate.Enabled = false;
            this.tbTemplate.Location = new System.Drawing.Point(446, 19);
            this.tbTemplate.Name = "tbTemplate";
            this.tbTemplate.Size = new System.Drawing.Size(100, 21);
            this.tbTemplate.TabIndex = 13;
            this.tbTemplate.Text = "Template Name";
            // 
            // ctrlReconOutput1
            // 
            this.ctrlReconOutput1.BackColor = System.Drawing.SystemColors.Control;
            this.ctrlReconOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlReconOutput1.Location = new System.Drawing.Point(0, 0);
            this.ctrlReconOutput1.Name = "ctrlReconOutput1";
            this.ctrlReconOutput1.Size = new System.Drawing.Size(704, 349);
            this.ctrlReconOutput1.TabIndex = 0;
            // 
            // cntrlReconAmendments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "cntrlReconAmendments";
            this.Size = new System.Drawing.Size(704, 412);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbReconType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTemplate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraButton btnPostTransaction;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor tbTemplate;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor tbReconType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor tbClient;
        private Infragistics.Win.Misc.UltraLabel lblTemplate;
        private Infragistics.Win.Misc.UltraLabel lblReconType;
        private Infragistics.Win.Misc.UltraLabel lblClient;
        private ctrlReconOutput ctrlReconOutput1;
    }
}
