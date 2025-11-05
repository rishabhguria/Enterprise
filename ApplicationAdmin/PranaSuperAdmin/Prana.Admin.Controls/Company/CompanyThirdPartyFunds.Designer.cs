namespace Prana.Admin.Controls
{
    partial class CompanyThirdPartyAccounts
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (dtCompanyaccounts != null)
                {
                    dtCompanyaccounts.Dispose();
                }
                if (dtSelectedAccounts != null)
                {
                    dtSelectedAccounts.Dispose();
                }
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.lblThirdParty = new Infragistics.Win.Misc.UltraLabel();
            this.lblThirdPartyType = new Infragistics.Win.Misc.UltraLabel();
            this.lblThirdPartyName = new Infragistics.Win.Misc.UltraLabel();
            this.lblTPType = new Infragistics.Win.Misc.UltraLabel();
            this.lblCompanyAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.lblThirdPartyuAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.grpTPAccounts = new Infragistics.Win.Misc.UltraGroupBox();
            this.grpTPAccountMap = new Infragistics.Win.Misc.UltraGroupBox();
            this.listThirdPartyAccounts = new System.Windows.Forms.ListBox();
            this.listCompanyAccounts = new System.Windows.Forms.ListBox();
            this.btnAllUnSelect = new Infragistics.Win.Misc.UltraButton();
            this.btnAllSelect = new Infragistics.Win.Misc.UltraButton();
            this.btnSingleUnselect = new Infragistics.Win.Misc.UltraButton();
            this.btnSingleSelect = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.grpTPAccounts)).BeginInit();
            this.grpTPAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpTPAccountMap)).BeginInit();
            this.grpTPAccountMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblThirdParty
            // 
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblThirdParty.Appearance = appearance1;
            this.lblThirdParty.Location = new System.Drawing.Point(15, 15);
            this.lblThirdParty.Name = "lblThirdParty";
            this.lblThirdParty.Size = new System.Drawing.Size(66, 14);
            this.lblThirdParty.TabIndex = 0;
            this.lblThirdParty.Text = "Third Party :";
            // 
            // lblThirdPartyType
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblThirdPartyType.Appearance = appearance2;
            this.lblThirdPartyType.Location = new System.Drawing.Point(258, 17);
            this.lblThirdPartyType.Name = "lblThirdPartyType";
            this.lblThirdPartyType.Size = new System.Drawing.Size(94, 14);
            this.lblThirdPartyType.TabIndex = 1;
            this.lblThirdPartyType.Text = "Third Party Type : ";
            // 
            // lblThirdPartyName
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.lblThirdPartyName.Appearance = appearance3;
            this.lblThirdPartyName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblThirdPartyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThirdPartyName.Location = new System.Drawing.Point(87, 14);
            this.lblThirdPartyName.Name = "lblThirdPartyName";
            this.lblThirdPartyName.Size = new System.Drawing.Size(35, 14);
            this.lblThirdPartyName.TabIndex = 2;
            this.lblThirdPartyName.Text = "Name";
            // 
            // lblTPType
            // 
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.lblTPType.Appearance = appearance4;
            this.lblTPType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblTPType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTPType.Location = new System.Drawing.Point(358, 15);
            this.lblTPType.Name = "lblTPType";
            this.lblTPType.Size = new System.Drawing.Size(30, 14);
            this.lblTPType.TabIndex = 3;
            this.lblTPType.Text = "Type";
            // 
            // lblCompanyAccounts
            // 
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.lblCompanyAccounts.Appearance = appearance5;
            this.lblCompanyAccounts.Location = new System.Drawing.Point(9, 11);
            this.lblCompanyAccounts.Name = "lblCompanyAccounts";
            this.lblCompanyAccounts.Size = new System.Drawing.Size(93, 14);
            this.lblCompanyAccounts.TabIndex = 4;
            this.lblCompanyAccounts.Text = "Client Accounts :";
            // 
            // lblThirdPartyuAccounts
            // 
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.lblThirdPartyuAccounts.Appearance = appearance6;
            this.lblThirdPartyuAccounts.Location = new System.Drawing.Point(260, 11);
            this.lblThirdPartyuAccounts.Name = "lblThirdPartyuAccounts";
            this.lblThirdPartyuAccounts.Size = new System.Drawing.Size(100, 14);
            this.lblThirdPartyuAccounts.TabIndex = 6;
            this.lblThirdPartyuAccounts.Text = "Third Party Accounts :";
            // 
            // grpTPAccounts
            // 
            this.grpTPAccounts.Controls.Add(this.lblThirdPartyName);
            this.grpTPAccounts.Controls.Add(this.lblThirdParty);
            this.grpTPAccounts.Controls.Add(this.lblTPType);
            this.grpTPAccounts.Controls.Add(this.lblThirdPartyType);
            this.grpTPAccounts.Location = new System.Drawing.Point(4, 6);
            this.grpTPAccounts.Name = "grpTPAccounts";
            this.grpTPAccounts.Size = new System.Drawing.Size(481, 40);
            this.grpTPAccounts.TabIndex = 6;
            // 
            // grpTPAccountMap
            // 
            this.grpTPAccountMap.Controls.Add(this.listThirdPartyAccounts);
            this.grpTPAccountMap.Controls.Add(this.listCompanyAccounts);
            this.grpTPAccountMap.Controls.Add(this.btnAllUnSelect);
            this.grpTPAccountMap.Controls.Add(this.btnAllSelect);
            this.grpTPAccountMap.Controls.Add(this.btnSingleUnselect);
            this.grpTPAccountMap.Controls.Add(this.btnSingleSelect);
            this.grpTPAccountMap.Controls.Add(this.lblCompanyAccounts);
            this.grpTPAccountMap.Controls.Add(this.lblThirdPartyuAccounts);
            this.grpTPAccountMap.Location = new System.Drawing.Point(47, 52);
            this.grpTPAccountMap.Name = "grpTPAccountMap";
            this.grpTPAccountMap.Size = new System.Drawing.Size(425, 249);
            this.grpTPAccountMap.TabIndex = 9;
            // 
            // listThirdPartyAccounts
            // 
            this.listThirdPartyAccounts.FormattingEnabled = true;
            this.listThirdPartyAccounts.Location = new System.Drawing.Point(260, 33);
            this.listThirdPartyAccounts.Name = "listThirdPartyAccounts";
            this.listThirdPartyAccounts.Size = new System.Drawing.Size(129, 212);
            this.listThirdPartyAccounts.TabIndex = 3;
            // 
            // listCompanyAccounts
            // 
            this.listCompanyAccounts.FormattingEnabled = true;
            this.listCompanyAccounts.Location = new System.Drawing.Point(9, 33);
            this.listCompanyAccounts.Name = "listCompanyAccounts";
            this.listCompanyAccounts.Size = new System.Drawing.Size(129, 212);
            this.listCompanyAccounts.TabIndex = 0;
            // 
            // btnAllUnSelect
            // 
            this.btnAllUnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllUnSelect.Location = new System.Drawing.Point(180, 177);
            this.btnAllUnSelect.Name = "btnAllUnSelect";
            this.btnAllUnSelect.Size = new System.Drawing.Size(35, 23);
            this.btnAllUnSelect.TabIndex = 5;
            this.btnAllUnSelect.Text = "<<";
            this.btnAllUnSelect.Click += new System.EventHandler(this.btnAllUnSelect_Click_1);
            // 
            // btnAllSelect
            // 
            this.btnAllSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllSelect.Location = new System.Drawing.Point(180, 101);
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.Size = new System.Drawing.Size(35, 23);
            this.btnAllSelect.TabIndex = 2;
            this.btnAllSelect.Text = ">>";
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click_1);
            // 
            // btnSingleUnselect
            // 
            this.btnSingleUnselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSingleUnselect.Location = new System.Drawing.Point(180, 148);
            this.btnSingleUnselect.Name = "btnSingleUnselect";
            this.btnSingleUnselect.Size = new System.Drawing.Size(35, 23);
            this.btnSingleUnselect.TabIndex = 4;
            this.btnSingleUnselect.Text = "<";
            this.btnSingleUnselect.Click += new System.EventHandler(this.btnSingleUnselect_Click);
            // 
            // btnSingleSelect
            // 
            this.btnSingleSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSingleSelect.Location = new System.Drawing.Point(180, 72);
            this.btnSingleSelect.Name = "btnSingleSelect";
            this.btnSingleSelect.Size = new System.Drawing.Size(35, 23);
            this.btnSingleSelect.TabIndex = 1;
            this.btnSingleSelect.Text = ">";
            this.btnSingleSelect.Click += new System.EventHandler(this.btnSingleSelect_Click_1);
            // 
            // CompanyThirdPartyAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpTPAccountMap);
            this.Controls.Add(this.grpTPAccounts);
            this.Name = "CompanyThirdPartyAccounts";
            this.Size = new System.Drawing.Size(488, 306);
            ((System.ComponentModel.ISupportInitialize)(this.grpTPAccounts)).EndInit();
            this.grpTPAccounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpTPAccountMap)).EndInit();
            this.grpTPAccountMap.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblThirdParty;
        private Infragistics.Win.Misc.UltraLabel lblThirdPartyType;
        private Infragistics.Win.Misc.UltraLabel lblThirdPartyName;
        private Infragistics.Win.Misc.UltraLabel lblTPType;
        private Infragistics.Win.Misc.UltraLabel lblCompanyAccounts;
        private Infragistics.Win.Misc.UltraLabel lblThirdPartyuAccounts;
        private Infragistics.Win.Misc.UltraGroupBox grpTPAccounts;
        private Infragistics.Win.Misc.UltraGroupBox grpTPAccountMap;
        private Infragistics.Win.Misc.UltraButton btnAllUnSelect;
        private Infragistics.Win.Misc.UltraButton btnAllSelect;
        private Infragistics.Win.Misc.UltraButton btnSingleUnselect;
        private Infragistics.Win.Misc.UltraButton btnSingleSelect;
        private System.Windows.Forms.ListBox listThirdPartyAccounts;
        private System.Windows.Forms.ListBox listCompanyAccounts;
    }
}
