namespace Prana.ClientCommon
{
    partial class AllocationSchemes
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
            this.lstSchemes = new System.Windows.Forms.ListBox();
            this.lblError = new System.Windows.Forms.Label();
            this.btnContinue = new Infragistics.Win.Misc.UltraButton();
            this.txtScheme = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstSchemes
            // 
            this.lstSchemes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSchemes.ColumnWidth = 200;
            this.lstSchemes.FormattingEnabled = true;
            this.lstSchemes.Location = new System.Drawing.Point(2, 23);
            this.lstSchemes.MultiColumn = true;
            this.lstSchemes.Name = "lstSchemes";
            this.lstSchemes.ScrollAlwaysVisible = true;
            this.lstSchemes.Size = new System.Drawing.Size(298, 197);
            this.lstSchemes.TabIndex = 1;
            this.lstSchemes.SelectedValueChanged += new System.EventHandler(this.lstSchemes_SelectedValueChanged);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblError.Location = new System.Drawing.Point(2, 5);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(203, 13);
            this.lblError.TabIndex = 0;
            this.lblError.Text = "Allocation Schemes already exists.";
            // 
            // btnContinue
            // 
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnContinue.Location = new System.Drawing.Point(236, 231);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(62, 23);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "Continue";
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // txtScheme
            // 
            this.txtScheme.Location = new System.Drawing.Point(45, 232);
            this.txtScheme.MaxLength = 100;
            this.txtScheme.Name = "txtScheme";
            this.txtScheme.Size = new System.Drawing.Size(177, 20);
            this.txtScheme.TabIndex = 3;
            this.txtScheme.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtScheme_KeyPress);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblName.Location = new System.Drawing.Point(0, 235);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // AllocationSchemes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(304, 262);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtScheme);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lstSchemes);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(312, 300);
            this.MinimumSize = new System.Drawing.Size(312, 300);
            this.Name = "AllocationSchemes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allocation Schemes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstSchemes;
        private System.Windows.Forms.Label lblError;
        private Infragistics.Win.Misc.UltraButton btnContinue;
        private System.Windows.Forms.TextBox txtScheme;
        private System.Windows.Forms.Label lblName;
    }
}