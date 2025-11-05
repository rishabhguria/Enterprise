namespace Prana.CashManagement
{
    partial class ctrlCashAccounts
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
            this.components = new System.ComponentModel.Container();
            this.treeAccounts = new Infragistics.Win.UltraWinTree.UltraTree();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAcronym = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblSubAccType = new System.Windows.Forms.Label();
            this.grpBoxAccounts = new System.Windows.Forms.GroupBox();
            this.cmbAccountType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtBoxAcronym = new System.Windows.Forms.TextBox();
            this.txtBoxName = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treeAccounts)).BeginInit();
            this.grpBoxAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountType)).BeginInit();
            this.SuspendLayout();
            // 
            // treeAccounts
            // 
            this.treeAccounts.Location = new System.Drawing.Point(0, 3);
            this.treeAccounts.Name = "treeAccounts";
            this.treeAccounts.Size = new System.Drawing.Size(123, 201);
            this.treeAccounts.TabIndex = 0;
            this.treeAccounts.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.treeAccounts_AfterSelect);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(86, 49);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // lblAcronym
            // 
            this.lblAcronym.AutoSize = true;
            this.lblAcronym.Location = new System.Drawing.Point(73, 87);
            this.lblAcronym.Name = "lblAcronym";
            this.lblAcronym.Size = new System.Drawing.Size(48, 13);
            this.lblAcronym.TabIndex = 2;
            this.lblAcronym.Text = "Acronym";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(143, 94);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(25, 13);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "Info";
            // 
            // lblSubAccType
            // 
            this.lblSubAccType.AutoSize = true;
            this.lblSubAccType.Location = new System.Drawing.Point(56, 124);
            this.lblSubAccType.Name = "lblSubAccType";
            this.lblSubAccType.Size = new System.Drawing.Size(74, 13);
            this.lblSubAccType.TabIndex = 4;
            this.lblSubAccType.Text = "Account Type";
            // 
            // grpBoxAccounts
            // 
            this.grpBoxAccounts.Controls.Add(this.cmbAccountType);
            this.grpBoxAccounts.Controls.Add(this.txtBoxAcronym);
            this.grpBoxAccounts.Controls.Add(this.txtBoxName);
            this.grpBoxAccounts.Controls.Add(this.lblName);
            this.grpBoxAccounts.Controls.Add(this.lblSubAccType);
            this.grpBoxAccounts.Controls.Add(this.lblAcronym);
            this.grpBoxAccounts.Controls.Add(this.lblInfo);
            this.grpBoxAccounts.Location = new System.Drawing.Point(129, 3);
            this.grpBoxAccounts.Name = "grpBoxAccounts";
            this.grpBoxAccounts.Size = new System.Drawing.Size(310, 201);
            this.grpBoxAccounts.TabIndex = 5;
            this.grpBoxAccounts.TabStop = false;
            this.grpBoxAccounts.Text = "Accounts";
            // 
            // cmbAccountType
            // 
            this.cmbAccountType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbAccountType.Location = new System.Drawing.Point(136, 120);
            this.cmbAccountType.Name = "cmbAccountType";
            this.cmbAccountType.Size = new System.Drawing.Size(100, 21);
            this.cmbAccountType.TabIndex = 8;
            this.cmbAccountType.ValueChanged += new System.EventHandler(this.cmbAccountType_ValueChanged);
            // 
            // txtBoxAcronym
            // 
            this.txtBoxAcronym.Location = new System.Drawing.Point(136, 84);
            this.txtBoxAcronym.Name = "txtBoxAcronym";
            this.txtBoxAcronym.Size = new System.Drawing.Size(100, 20);
            this.txtBoxAcronym.TabIndex = 6;
            this.txtBoxAcronym.TextChanged += new System.EventHandler(this.txtBoxAcronym_TextChanged);
            // 
            // txtBoxName
            // 
            this.txtBoxName.Location = new System.Drawing.Point(136, 46);
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(100, 20);
            this.txtBoxName.TabIndex = 5;
            this.txtBoxName.TextChanged += new System.EventHandler(this.txtBoxName_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // ctrlCashAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBoxAccounts);
            this.Controls.Add(this.treeAccounts);
            this.Name = "ctrlCashAccounts";
            this.Size = new System.Drawing.Size(442, 207);
            ((System.ComponentModel.ISupportInitialize)(this.treeAccounts)).EndInit();
            this.grpBoxAccounts.ResumeLayout(false);
            this.grpBoxAccounts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTree.UltraTree treeAccounts;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAcronym;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblSubAccType;
        private System.Windows.Forms.GroupBox grpBoxAccounts;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txtBoxAcronym;
        private System.Windows.Forms.TextBox txtBoxName;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAccountType;
    }
}
