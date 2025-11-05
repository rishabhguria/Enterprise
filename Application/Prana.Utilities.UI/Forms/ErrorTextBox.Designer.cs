namespace Prana.Utilities.UI
{
    partial class ErrorTextBox
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
            this.ErrorMessageBox = new System.Windows.Forms.TextBox();
            this.okbutton = new System.Windows.Forms.Button();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ErrorMessageBox
            // 
            this.ErrorMessageBox.Location = new System.Drawing.Point(15, 57);
            this.ErrorMessageBox.MaxLength = 999999999;
            this.ErrorMessageBox.Multiline = true;
            this.ErrorMessageBox.Name = "ErrorMessageBox";
            this.ErrorMessageBox.ReadOnly = true;
            this.ErrorMessageBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ErrorMessageBox.Size = new System.Drawing.Size(391, 154);
            this.ErrorMessageBox.TabIndex = 1;
            // 
            // okbutton
            // 
            this.okbutton.Location = new System.Drawing.Point(331, 222);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(75, 23);
            this.okbutton.TabIndex = 2;
            this.okbutton.Text = "OK";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.ForeColor = System.Drawing.Color.Red;
            this.MessageLabel.Location = new System.Drawing.Point(12, 41);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(79, 13);
            this.MessageLabel.TabIndex = 3;
            this.MessageLabel.Text = "Message Label";
            // 
            // ErrorTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 257);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.okbutton);
            this.Controls.Add(this.ErrorMessageBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorTextBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warning!";
            this.Load += new System.EventHandler(this.errortextbox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private System.Windows.Forms.TextBox ErrorMessageBox;
        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.Label MessageLabel;
    }
}
