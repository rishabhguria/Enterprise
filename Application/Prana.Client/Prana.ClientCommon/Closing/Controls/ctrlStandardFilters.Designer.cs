namespace Prana.ClientCommon
{
    partial class ctrlStandardFilters
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lstBoxAsset = new System.Windows.Forms.CheckedListBox();
            this.cbAsset = new System.Windows.Forms.CheckBox();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lstBoxAccount = new System.Windows.Forms.CheckedListBox();
            this.cbAccount = new System.Windows.Forms.CheckBox();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lstBoxMasterFund = new System.Windows.Forms.CheckedListBox();
            this.cbMasterFund = new System.Windows.Forms.CheckBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.ultraGroupBox4 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraCheckEditor1 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).BeginInit();
            this.ultraGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.lstBoxAsset);
            this.ultraGroupBox2.Location = new System.Drawing.Point(70, 63);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(239, 180);
            this.ultraGroupBox2.TabIndex = 21;
            // 
            // lstBoxAsset
            // 
            this.lstBoxAsset.BackColor = System.Drawing.Color.White;
            this.lstBoxAsset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstBoxAsset.CheckOnClick = true;
            this.lstBoxAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBoxAsset.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxAsset.ForeColor = System.Drawing.Color.Black;
            this.lstBoxAsset.FormattingEnabled = true;
            this.lstBoxAsset.Location = new System.Drawing.Point(3, 3);
            this.lstBoxAsset.Name = "lstBoxAsset";
            this.lstBoxAsset.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstBoxAsset.Size = new System.Drawing.Size(233, 174);
            this.inboxControlStyler1.SetStyleSettings(this.lstBoxAsset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lstBoxAsset.TabIndex = 3;
            this.lstBoxAsset.Click += new System.EventHandler(this.lstBoxAsset_Click);
            // 
            // cbAsset
            // 
            this.cbAsset.AutoSize = true;
            this.cbAsset.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAsset.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbAsset.Location = new System.Drawing.Point(70, 40);
            this.cbAsset.Name = "cbAsset";
            this.cbAsset.Size = new System.Drawing.Size(119, 17);
            this.inboxControlStyler1.SetStyleSettings(this.cbAsset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cbAsset.TabIndex = 4;
            this.cbAsset.Tag = "cbAssets";
            this.cbAsset.Text = "Select All Assets";
            this.cbAsset.UseVisualStyleBackColor = true;
            this.cbAsset.CheckStateChanged += new System.EventHandler(this.cbAsset_CheckStateChanged);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.lstBoxAccount);
            this.ultraGroupBox1.ForeColor = System.Drawing.Color.Silver;
            this.ultraGroupBox1.Location = new System.Drawing.Point(400, 66);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(241, 180);
            this.ultraGroupBox1.TabIndex = 22;
            // 
            // lstBoxAccount
            // 
            this.lstBoxAccount.BackColor = System.Drawing.Color.White;
            this.lstBoxAccount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstBoxAccount.CheckOnClick = true;
            this.lstBoxAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBoxAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxAccount.ForeColor = System.Drawing.Color.Black;
            this.lstBoxAccount.FormattingEnabled = true;
            this.lstBoxAccount.Location = new System.Drawing.Point(3, 3);
            this.lstBoxAccount.Name = "lstBoxAccount";
            this.lstBoxAccount.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstBoxAccount.Size = new System.Drawing.Size(235, 174);
            this.inboxControlStyler1.SetStyleSettings(this.lstBoxAccount, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lstBoxAccount.TabIndex = 6;
            this.lstBoxAccount.SelectedIndexChanged += new System.EventHandler(this.lstBoxAccount_SelectedIndexChanged);
            // 
            // cbAccount
            // 
            this.cbAccount.AutoSize = true;
            this.cbAccount.BackColor = System.Drawing.Color.Transparent;
            this.cbAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAccount.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbAccount.Location = new System.Drawing.Point(400, 40);
            this.cbAccount.Name = "cbAccount";
            this.cbAccount.Size = new System.Drawing.Size(114, 17);
            this.inboxControlStyler1.SetStyleSettings(this.cbAccount, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cbAccount.TabIndex = 7;
            this.cbAccount.Tag = "cbAccounts";
            this.cbAccount.Text = "Select All Accounts";
            this.cbAccount.UseVisualStyleBackColor = false;
            this.cbAccount.CheckStateChanged += new System.EventHandler(this.cbAccount_CheckStateChanged);
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.lstBoxMasterFund);
            this.ultraGroupBox3.Location = new System.Drawing.Point(70, 292);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(239, 183);
            this.ultraGroupBox3.TabIndex = 23;
            // 
            // lstBoxMasterFund
            // 
            this.lstBoxMasterFund.BackColor = System.Drawing.Color.White;
            this.lstBoxMasterFund.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstBoxMasterFund.CheckOnClick = true;
            this.lstBoxMasterFund.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBoxMasterFund.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxMasterFund.ForeColor = System.Drawing.Color.Black;
            this.lstBoxMasterFund.FormattingEnabled = true;
            this.lstBoxMasterFund.Location = new System.Drawing.Point(3, 3);
            this.lstBoxMasterFund.Name = "lstBoxMasterFund";
            this.lstBoxMasterFund.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstBoxMasterFund.Size = new System.Drawing.Size(233, 177);
            this.inboxControlStyler1.SetStyleSettings(this.lstBoxMasterFund, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lstBoxMasterFund.TabIndex = 6;
           this.lstBoxMasterFund.SelectedIndexChanged += new System.EventHandler(this.lstBoxMasterFund_SelectedIndexChanged);
            // 
            // cbMasterFund
            // 
            this.cbMasterFund.AutoSize = true;
            this.cbMasterFund.BackColor = System.Drawing.Color.White;
            this.cbMasterFund.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbMasterFund.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMasterFund.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbMasterFund.Location = new System.Drawing.Point(73, 269);
            this.cbMasterFund.Name = "cbMasterFund";
            this.cbMasterFund.Size = new System.Drawing.Size(160, 17);
            this.inboxControlStyler1.SetStyleSettings(this.cbMasterFund, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cbMasterFund.TabIndex = 7;
            this.cbMasterFund.Tag = "cbMasterFunds";
            this.cbMasterFund.Text = "Select All Master Funds";
            this.cbMasterFund.UseVisualStyleBackColor = false;
            this.cbMasterFund.CheckStateChanged += new System.EventHandler(this.cbMasterFund_CheckStateChanged);
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.SteelBlue;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            this.ultraLabel3.Appearance = appearance1;
            this.ultraLabel3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(830, 20);
            this.ultraLabel3.TabIndex = 24;
            this.ultraLabel3.Text = "Standard Filters";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(6, 30);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.inboxControlStyler1.SetStyleSettings(this.dateTimePicker1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.dateTimePicker1.TabIndex = 25;
            // 
            // ultraGroupBox4
            // 
            this.ultraGroupBox4.Controls.Add(this.ultraCheckEditor1);
            this.ultraGroupBox4.Controls.Add(this.label3);
            this.ultraGroupBox4.Controls.Add(this.label2);
            this.ultraGroupBox4.Controls.Add(this.dtToDate);
            this.ultraGroupBox4.Controls.Add(this.dtFromDate);
            this.ultraGroupBox4.Controls.Add(this.checkedListBox1);
            this.ultraGroupBox4.Controls.Add(this.dateTimePicker1);
            this.ultraGroupBox4.ForeColor = System.Drawing.Color.Silver;
            this.ultraGroupBox4.Location = new System.Drawing.Point(398, 292);
            this.ultraGroupBox4.Name = "ultraGroupBox4";
            this.ultraGroupBox4.Size = new System.Drawing.Size(243, 183);
            this.ultraGroupBox4.TabIndex = 26;
            // 
            // ultraCheckEditor1
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.ultraCheckEditor1.Appearance = appearance2;
            this.ultraCheckEditor1.BackColor = System.Drawing.Color.White;
            this.ultraCheckEditor1.BackColorInternal = System.Drawing.Color.White;
            this.ultraCheckEditor1.Location = new System.Drawing.Point(11, 125);
            this.ultraCheckEditor1.Name = "ultraCheckEditor1";
            this.ultraCheckEditor1.Size = new System.Drawing.Size(120, 20);
            this.ultraCheckEditor1.TabIndex = 30;
            this.ultraCheckEditor1.Text = "Use Current Date";
            this.ultraCheckEditor1.CheckedChanged += new System.EventHandler(this.ultraCheckEditor1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(7, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label3.TabIndex = 29;
            this.label3.Text = "To Date :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label2.TabIndex = 28;
            this.label2.Text = "From Date :";
            // 
            // dtToDate
            // 
            this.dtToDate.Location = new System.Drawing.Point(83, 82);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(144, 21);
            this.dtToDate.TabIndex = 27;
            this.dtToDate.Leave += new System.EventHandler(this.OnDateValueChanged);
            // 
            // dtFromDate
            // 
            this.dtFromDate.Location = new System.Drawing.Point(83, 30);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(144, 21);
            this.dtFromDate.TabIndex = 26;
            this.dtFromDate.Leave += new System.EventHandler(this.OnDateValueChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.Color.White;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox1.ForeColor = System.Drawing.Color.Black;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 3);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox1.Size = new System.Drawing.Size(237, 177);
            this.inboxControlStyler1.SetStyleSettings(this.checkedListBox1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedListBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(406, 270);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label1.TabIndex = 27;
            this.label1.Text = "Select Date Range :";
            // 
            // ctrlStandardFilters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ultraGroupBox4);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.cbMasterFund);
            this.Controls.Add(this.cbAccount);
            this.Controls.Add(this.cbAsset);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.ultraGroupBox2);
            this.Name = "ctrlStandardFilters";
            this.Size = new System.Drawing.Size(830, 493);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).EndInit();
            this.ultraGroupBox4.ResumeLayout(false);
            this.ultraGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.CheckBox cbAsset;
        private System.Windows.Forms.CheckedListBox lstBoxAsset;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.CheckBox cbAccount;
        private System.Windows.Forms.CheckedListBox lstBoxAccount;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.CheckBox cbMasterFund;
        private System.Windows.Forms.CheckedListBox lstBoxMasterFund;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox4;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditor1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
