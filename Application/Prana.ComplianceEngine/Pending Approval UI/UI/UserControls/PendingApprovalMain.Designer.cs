using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;
using System.Drawing;
namespace Prana.ComplianceEngine.Pending_Approval_UI.UI
{
    partial class PendingApprovalMain
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
            if (pendingApprovalGrid1 != null)
                pendingApprovalGrid1 = null;
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bulkApproveButton = new Infragistics.Win.Misc.UltraButton();
            this.bulkBlockButton = new Infragistics.Win.Misc.UltraButton();
            this.lblMsg = new Infragistics.Win.Misc.UltraLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.pendingApprovalGrid1 = new Prana.ComplianceEngine.Pending_Approval_UI.PendingApprovalGrid();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMsg);
            this.ultraPanel1.ClientArea.Controls.Add(this.label1);
            this.ultraPanel1.ClientArea.Controls.Add(this.bulkApproveButton);
            this.ultraPanel1.ClientArea.Controls.Add(this.bulkBlockButton);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(389, 54);
            this.ultraPanel1.TabIndex = 0;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(10, 0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(300, 24);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Text = "Transactions Pending Compliance Approval";
            // 
            // label1
            // 
            this.label1.AutoSize = false;
            this.label1.Location = new System.Drawing.Point(0, 20);
            this.label1.Height = 2;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Size = new System.Drawing.Size(2000, 1);
            // 
            // bulkApproveButton
            //
            this.bulkApproveButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.bulkApproveButton.Location = new System.Drawing.Point(480, 25);
            this.bulkApproveButton.Name = "bulkApproveButton";
            this.bulkApproveButton.Size = new System.Drawing.Size(80, 25);
            this.bulkApproveButton.TabIndex = 1;
            this.bulkApproveButton.Text = "Approve";
            this.bulkApproveButton.UseAppStyling = false;
            this.bulkApproveButton.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.bulkApproveButton.Appearance.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
            this.bulkApproveButton.Appearance.ForeColor = System.Drawing.Color.White;
            this.bulkApproveButton.Click += new System.EventHandler(this.btn_bulkApproveButtonClick);
            this.bulkApproveButton.MouseHover += message_MouseHover;
            // 
            // bulkBlockButton
            //
            this.bulkBlockButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.bulkBlockButton.Location = new System.Drawing.Point(580, 25);
            this.bulkBlockButton.Name = "bulkBlockButton";
            this.bulkBlockButton.Size = new System.Drawing.Size(80, 25);
            this.bulkBlockButton.TabIndex = 2;
            this.bulkBlockButton.Text = "Block";
            this.bulkBlockButton.UseAppStyling = false;
            this.bulkBlockButton.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.bulkBlockButton.Appearance.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
            this.bulkBlockButton.Appearance.ForeColor = System.Drawing.Color.White;
            this.bulkBlockButton.Click += new System.EventHandler(this.btn_bulkBlockButtonClick);
            this.bulkApproveButton.MouseHover += message_MouseHover;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraPanel3);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 21);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(389, 305);
            this.ultraPanel2.TabIndex = 7;
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.pendingApprovalGrid1);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(389, 305);
            this.ultraPanel3.TabIndex = 3;
            // 
            // pendingApprovalGrid1
            // 
            this.pendingApprovalGrid1.AutoScroll = true;
            this.pendingApprovalGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pendingApprovalGrid1.Location = new System.Drawing.Point(0, 0);
            this.pendingApprovalGrid1.Name = "pendingApprovalGrid1";
            this.pendingApprovalGrid1.Size = new System.Drawing.Size(389, 305);
            this.pendingApprovalGrid1.TabIndex = 0;
            // 
            // PendingApprovalMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "PendingApprovalMain";
            this.Size = new System.Drawing.Size(389, 326);
            this.Load += new System.EventHandler(this.PendingApprovalMain_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private PendingApprovalGrid pendingApprovalGrid1;
        private Infragistics.Win.Misc.UltraButton bulkApproveButton;
        private Infragistics.Win.Misc.UltraButton bulkBlockButton;
        private Infragistics.Win.Misc.UltraLabel lblMsg;
        private System.Windows.Forms.Label label1;



    }
}
