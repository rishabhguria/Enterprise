namespace Prana.Admin.Controls
{
    partial class SymbolLevelAccrualUtility
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
            this.MsgListBox = new System.Windows.Forms.ListBox();
            this.finishButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MsgListBox
            // 
            this.MsgListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MsgListBox.FormattingEnabled = true;
            this.MsgListBox.Location = new System.Drawing.Point(13, 13);
            this.MsgListBox.Name = "MsgListBox";
            this.MsgListBox.Size = new System.Drawing.Size(559, 197);
            this.MsgListBox.TabIndex = 0;
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.Enabled = false;
            this.finishButton.Location = new System.Drawing.Point(240, 226);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(120, 23);
            this.finishButton.TabIndex = 1;
            this.finishButton.Text = "Finish";
            this.finishButton.UseVisualStyleBackColor = true;
            this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
            // 
            // SymbolLevelAccrualUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.ControlBox = false;
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.MsgListBox);
            this.MaximumSize = new System.Drawing.Size(600, 300);
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "SymbolLevelAccrualUtility";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Symbol Level Accrual Utility Running Details";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox MsgListBox;
        private System.Windows.Forms.Button finishButton;
    }
}