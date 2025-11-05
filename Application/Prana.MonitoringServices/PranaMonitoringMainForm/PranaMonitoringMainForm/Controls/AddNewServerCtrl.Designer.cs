namespace Prana.MonitoringServices
{
    partial class AddNewServerCtrl
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbxIP = new System.Windows.Forms.TextBox();
            this.txtbxPorts = new System.Windows.Forms.TextBox();
            this.lblPorts = new System.Windows.Forms.Label();
            this.txtbxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSeviceNames = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(82, 138);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IPAddress";
            // 
            // txtbxIP
            // 
            this.txtbxIP.Location = new System.Drawing.Point(109, 12);
            this.txtbxIP.Name = "txtbxIP";
            this.txtbxIP.Size = new System.Drawing.Size(100, 20);
            this.txtbxIP.TabIndex = 1;
            this.txtbxIP.Tag = "0";
            this.txtbxIP.Text = "192.168.1.144";
            // 
            // txtbxPorts
            // 
            this.txtbxPorts.Location = new System.Drawing.Point(109, 94);
            this.txtbxPorts.Name = "txtbxPorts";
            this.txtbxPorts.Size = new System.Drawing.Size(303, 20);
            this.txtbxPorts.TabIndex = 4;
            this.txtbxPorts.Tag = "3";
            this.txtbxPorts.Text = "5000,5001,5002";
            // 
            // lblPorts
            // 
            this.lblPorts.AutoSize = true;
            this.lblPorts.Location = new System.Drawing.Point(22, 94);
            this.lblPorts.Name = "lblPorts";
            this.lblPorts.Size = new System.Drawing.Size(31, 13);
            this.lblPorts.TabIndex = 3;
            this.lblPorts.Text = "Ports";
            // 
            // txtbxName
            // 
            this.txtbxName.Location = new System.Drawing.Point(109, 38);
            this.txtbxName.Name = "txtbxName";
            this.txtbxName.Size = new System.Drawing.Size(100, 20);
            this.txtbxName.TabIndex = 2;
            this.txtbxName.Tag = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "CustomerName";
            // 
            // txtSeviceNames
            // 
            this.txtSeviceNames.Location = new System.Drawing.Point(109, 67);
            this.txtSeviceNames.Name = "txtSeviceNames";
            this.txtSeviceNames.Size = new System.Drawing.Size(303, 20);
            this.txtSeviceNames.TabIndex = 3;
            this.txtSeviceNames.Tag = "2";
            this.txtSeviceNames.Text = "TradeServer,ExpnlServer,PricingServer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "ServiceNames";
            // 
            // AddNewServerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 196);
            this.Controls.Add(this.txtSeviceNames);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtbxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbxPorts);
            this.Controls.Add(this.lblPorts);
            this.Controls.Add(this.txtbxIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAdd);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(266, 158);
            this.Name = "AddNewServerCtrl";
            this.Text = "Prana: AddServer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbxIP;
        private System.Windows.Forms.TextBox txtbxPorts;
        private System.Windows.Forms.Label lblPorts;
        private System.Windows.Forms.TextBox txtbxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSeviceNames;
        private System.Windows.Forms.Label label3;
    }
}
