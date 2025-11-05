namespace Nirvana.AdminForms
{
    partial class CommissionRulesNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommissionRulesNew));
            this.trvCommissionRule = new System.Windows.Forms.TreeView();
            this.grpButtons = new System.Windows.Forms.GroupBox();
            this.btnSaveCommissionRule = new System.Windows.Forms.Button();
            this.btnAddNewRule = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDeleteCommissionRule = new System.Windows.Forms.Button();
            this.ctrlCommissionRuleobj = new Nirvana.Admin.Controls.ctrlCommissionRule();
            this.grpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvCommissionRule
            // 
            this.trvCommissionRule.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvCommissionRule.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.trvCommissionRule.FullRowSelect = true;
            this.trvCommissionRule.HideSelection = false;
            this.trvCommissionRule.HotTracking = true;
            this.trvCommissionRule.Location = new System.Drawing.Point(4, 0);
            this.trvCommissionRule.Name = "trvCommissionRule";
            this.trvCommissionRule.Size = new System.Drawing.Size(154, 533);
            this.trvCommissionRule.TabIndex = 0;
            this.trvCommissionRule.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvCommissionRule_AfterSelect);
            this.trvCommissionRule.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvCommissionRule_BeforeSelect);
            // 
            // grpButtons
            // 
            this.grpButtons.Controls.Add(this.btnSaveCommissionRule);
            this.grpButtons.Controls.Add(this.btnAddNewRule);
            this.grpButtons.Controls.Add(this.btnClose);
            this.grpButtons.Controls.Add(this.btnDeleteCommissionRule);
            this.grpButtons.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpButtons.Location = new System.Drawing.Point(4, 539);
            this.grpButtons.Name = "grpButtons";
            this.grpButtons.Size = new System.Drawing.Size(642, 48);
            this.grpButtons.TabIndex = 4;
            this.grpButtons.TabStop = false;
            // 
            // btnSaveCommissionRule
            // 
            this.btnSaveCommissionRule.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveCommissionRule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btnSaveCommissionRule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveCommissionRule.BackgroundImage")));
            this.btnSaveCommissionRule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveCommissionRule.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSaveCommissionRule.Location = new System.Drawing.Point(315, 15);
            this.btnSaveCommissionRule.Name = "btnSaveCommissionRule";
            this.btnSaveCommissionRule.Size = new System.Drawing.Size(75, 23);
            this.btnSaveCommissionRule.TabIndex = 89;
            this.btnSaveCommissionRule.UseVisualStyleBackColor = false;
            this.btnSaveCommissionRule.Click += new System.EventHandler(this.btnSaveCommissionRule_Click);
            // 
            // btnAddNewRule
            // 
            this.btnAddNewRule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAddNewRule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddNewRule.BackgroundImage")));
            this.btnAddNewRule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewRule.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAddNewRule.Location = new System.Drawing.Point(9, 15);
            this.btnAddNewRule.Name = "btnAddNewRule";
            this.btnAddNewRule.Size = new System.Drawing.Size(72, 23);
            this.btnAddNewRule.TabIndex = 71;
            this.btnAddNewRule.UseVisualStyleBackColor = false;
            this.btnAddNewRule.Click += new System.EventHandler(this.btnAddNewRule_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(396, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 23);
            this.btnClose.TabIndex = 72;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDeleteCommissionRule
            // 
            this.btnDeleteCommissionRule.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteCommissionRule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDeleteCommissionRule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteCommissionRule.BackgroundImage")));
            this.btnDeleteCommissionRule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeleteCommissionRule.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDeleteCommissionRule.Location = new System.Drawing.Point(86, 15);
            this.btnDeleteCommissionRule.Name = "btnDeleteCommissionRule";
            this.btnDeleteCommissionRule.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteCommissionRule.TabIndex = 70;
            this.btnDeleteCommissionRule.UseVisualStyleBackColor = false;
            this.btnDeleteCommissionRule.Click += new System.EventHandler(this.btnDeleteCommissionRule_Click);
            // 
            // ctrlCommissionRuleobj
            // 
            this.ctrlCommissionRuleobj.Location = new System.Drawing.Point(161, -1);
            this.ctrlCommissionRuleobj.Name = "ctrlCommissionRuleobj";
            this.ctrlCommissionRuleobj.Size = new System.Drawing.Size(495, 545);
            this.ctrlCommissionRuleobj.TabIndex = 5;
            // 
            // CommissionRulesNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 592);
            this.Controls.Add(this.ctrlCommissionRuleobj);
            this.Controls.Add(this.grpButtons);
            this.Controls.Add(this.trvCommissionRule);
            this.MaximizeBox = false;
            this.Name = "CommissionRulesNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Commission Rules";            
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommissionRulesNew_FormClosing);
            this.grpButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trvCommissionRule;
        private System.Windows.Forms.GroupBox grpButtons;
        private System.Windows.Forms.Button btnDeleteCommissionRule;
        private System.Windows.Forms.Button btnAddNewRule;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveCommissionRule;
        private Nirvana.Admin.Controls.ctrlCommissionRule ctrlCommissionRuleobj;
      
    }
}