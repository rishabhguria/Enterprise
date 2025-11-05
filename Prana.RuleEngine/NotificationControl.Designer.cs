namespace Prana.RuleEngine
{
    partial class NotificationControl
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
            this.chckbxPopUp = new System.Windows.Forms.CheckBox();
            this.chckbxEmail = new System.Windows.Forms.CheckBox();
            this.chckBxSound = new System.Windows.Forms.CheckBox();
            this.txtBxEmails = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chckBxSMS = new System.Windows.Forms.CheckBox();
            this.grpBoxFrequency = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboNotifyFreq = new System.Windows.Forms.ComboBox();
            this.chckBxManualTrade = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.grpBoxFrequency.SuspendLayout();
            this.SuspendLayout();
            // 
            // chckbxPopUp
            // 
            this.chckbxPopUp.AutoSize = true;
            this.chckbxPopUp.Location = new System.Drawing.Point(18, 28);
            this.chckbxPopUp.Name = "chckbxPopUp";
            this.chckbxPopUp.Size = new System.Drawing.Size(62, 17);
            this.chckbxPopUp.TabIndex = 0;
            this.chckbxPopUp.Text = "Pop-Up";
            this.chckbxPopUp.UseVisualStyleBackColor = true;
            // 
            // chckbxEmail
            // 
            this.chckbxEmail.AutoSize = true;
            this.chckbxEmail.Location = new System.Drawing.Point(89, 28);
            this.chckbxEmail.Name = "chckbxEmail";
            this.chckbxEmail.Size = new System.Drawing.Size(51, 17);
            this.chckbxEmail.TabIndex = 1;
            this.chckbxEmail.Text = "Email";
            this.chckbxEmail.UseVisualStyleBackColor = true;
            // 
            // chckBxSound
            // 
            this.chckBxSound.AutoSize = true;
            this.chckBxSound.Location = new System.Drawing.Point(89, 56);
            this.chckBxSound.Name = "chckBxSound";
            this.chckBxSound.Size = new System.Drawing.Size(57, 17);
            this.chckBxSound.TabIndex = 4;
            this.chckBxSound.Text = "Sound";
            this.chckBxSound.UseVisualStyleBackColor = true;
            this.chckBxSound.Visible = false;
            // 
            // txtBxEmails
            // 
            this.txtBxEmails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBxEmails.Location = new System.Drawing.Point(146, 24);
            this.txtBxEmails.Name = "txtBxEmails";
            this.txtBxEmails.Size = new System.Drawing.Size(306, 20);
            this.txtBxEmails.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(147, 52);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(305, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtBxEmails);
            this.groupBox2.Controls.Add(this.chckBxSMS);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.chckBxSound);
            this.groupBox2.Controls.Add(this.chckbxEmail);
            this.groupBox2.Controls.Add(this.chckbxPopUp);
            this.groupBox2.Location = new System.Drawing.Point(9, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 81);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notify Via :";
            // 
            // chckBxSMS
            // 
            this.chckBxSMS.AutoSize = true;
            this.chckBxSMS.Location = new System.Drawing.Point(18, 56);
            this.chckBxSMS.Name = "chckBxSMS";
            this.chckBxSMS.Size = new System.Drawing.Size(49, 17);
            this.chckBxSMS.TabIndex = 7;
            this.chckBxSMS.Text = "SMS";
            this.chckBxSMS.UseVisualStyleBackColor = true;
            this.chckBxSMS.Visible = false;
            // 
            // grpBoxFrequency
            // 
            this.grpBoxFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxFrequency.Controls.Add(this.label1);
            this.grpBoxFrequency.Controls.Add(this.comboNotifyFreq);
            this.grpBoxFrequency.Controls.Add(this.chckBxManualTrade);
            this.grpBoxFrequency.Location = new System.Drawing.Point(473, 5);
            this.grpBoxFrequency.Name = "grpBoxFrequency";
            this.grpBoxFrequency.Size = new System.Drawing.Size(361, 82);
            this.grpBoxFrequency.TabIndex = 10;
            this.grpBoxFrequency.TabStop = false;
            this.grpBoxFrequency.Text = "Preferences:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Alert Frequency";
            // 
            // comboNotifyFreq
            // 
            this.comboNotifyFreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboNotifyFreq.FormattingEnabled = true;
            this.comboNotifyFreq.Location = new System.Drawing.Point(108, 24);
            this.comboNotifyFreq.Name = "comboNotifyFreq";
            this.comboNotifyFreq.Size = new System.Drawing.Size(237, 21);
            this.comboNotifyFreq.TabIndex = 11;
            // 
            // chckBxManualTrade
            // 
            this.chckBxManualTrade.AutoSize = true;
            this.chckBxManualTrade.Location = new System.Drawing.Point(20, 56);
            this.chckBxManualTrade.Name = "chckBxManualTrade";
            this.chckBxManualTrade.Size = new System.Drawing.Size(150, 17);
            this.chckBxManualTrade.TabIndex = 3;
            this.chckBxManualTrade.Text = "Apply to manual trade also";
            this.chckBxManualTrade.UseVisualStyleBackColor = true;
            this.chckBxManualTrade.Visible = false;
            // 
            // NotificationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBoxFrequency);
            this.Controls.Add(this.groupBox2);
            this.Name = "NotificationControl";
            this.Size = new System.Drawing.Size(837, 96);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpBoxFrequency.ResumeLayout(false);
            this.grpBoxFrequency.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chckbxPopUp;
        private System.Windows.Forms.CheckBox chckbxEmail;
        private System.Windows.Forms.CheckBox chckBxSound;
        private System.Windows.Forms.TextBox txtBxEmails;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chckBxSMS;
        private System.Windows.Forms.GroupBox grpBoxFrequency;
        private System.Windows.Forms.CheckBox chckBxManualTrade;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboNotifyFreq;
    }
}
