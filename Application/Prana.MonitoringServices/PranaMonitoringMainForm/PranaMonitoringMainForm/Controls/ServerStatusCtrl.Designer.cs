namespace Prana.MonitoringServices
{
    partial class ServerStatusCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblConnectedTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCPUUses = new System.Windows.Forms.Label();
            this.lblAvailableMemory = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(185, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(27, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "LastConnectedTime";
            // 
            // lblConnectedTime
            // 
            this.lblConnectedTime.AutoSize = true;
            this.lblConnectedTime.Location = new System.Drawing.Point(185, 42);
            this.lblConnectedTime.Name = "lblConnectedTime";
            this.lblConnectedTime.Size = new System.Drawing.Size(27, 13);
            this.lblConnectedTime.TabIndex = 3;
            this.lblConnectedTime.Text = "N/A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "CPU Uses";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Available Memory";
            // 
            // lblCPUUses
            // 
            this.lblCPUUses.AutoSize = true;
            this.lblCPUUses.Location = new System.Drawing.Point(370, 9);
            this.lblCPUUses.Name = "lblCPUUses";
            this.lblCPUUses.Size = new System.Drawing.Size(27, 13);
            this.lblCPUUses.TabIndex = 6;
            this.lblCPUUses.Text = "N/A";
            // 
            // lblAvailableMemory
            // 
            this.lblAvailableMemory.AutoSize = true;
            this.lblAvailableMemory.Location = new System.Drawing.Point(357, 42);
            this.lblAvailableMemory.Name = "lblAvailableMemory";
            this.lblAvailableMemory.Size = new System.Drawing.Size(27, 13);
            this.lblAvailableMemory.TabIndex = 7;
            this.lblAvailableMemory.Text = "N/A";
            // 
            // ServerStatusCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAvailableMemory);
            this.Controls.Add(this.lblCPUUses);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblConnectedTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Name = "ServerStatusCtrl";
            this.Size = new System.Drawing.Size(463, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblConnectedTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCPUUses;
        private System.Windows.Forms.Label lblAvailableMemory;
    }
}
