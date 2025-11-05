namespace Prana.CashManagement
{
    partial class ctrlAccounts
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
                if (_dataSetMasterCategory != null)
                {
                    _dataSetMasterCategory.Dispose();
                }
                if (_mouseNode != null)
                {
                    _mouseNode.Dispose();
                }
                if (_transactionType != null)
                {
                    _transactionType.Dispose();
                    _transactionType = null;
                }
                if (_accountTypes != null)
                {
                    _accountTypes.Dispose();
                    _accountTypes = null;
                }
                if (_masterCategoryType != null)
                {
                    _masterCategoryType.Dispose();
                    _masterCategoryType = null;
                }
                if (_vlsubAccountType != null)
                {
                    _vlsubAccountType.Dispose();
                    _vlsubAccountType = null;
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel5 = new Infragistics.Win.Misc.UltraPanel();
            this.grdData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.accTree = new Infragistics.Win.UltraWinTree.UltraTree();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemAddAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemAddSubAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemDeleteAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemDeleteSubAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFilterAccount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.lblActivityJournalMapping = new Infragistics.Win.Misc.UltraLabel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel4.ClientArea.SuspendLayout();
            this.ultraPanel4.SuspendLayout();
            this.ultraPanel5.ClientArea.SuspendLayout();
            this.ultraPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accTree)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilterAccount)).BeginInit();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPanel4);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPanel3);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(882, 371);
            this.ultraPanel1.TabIndex = 0;
            // 
            // ultraPanel4
            // 
            // 
            // ultraPanel4.ClientArea
            // 
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraPanel5);
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraPanel2);
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel4.Location = new System.Drawing.Point(0, 25);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(882, 346);
            this.ultraPanel4.TabIndex = 27;
            // 
            // ultraPanel5
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            this.ultraPanel5.Appearance = appearance1;
            // 
            // ultraPanel5.ClientArea
            // 
            this.ultraPanel5.ClientArea.Controls.Add(this.grdData);
            this.ultraPanel5.ClientArea.Controls.Add(this.ultraLabel2);
            this.ultraPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel5.Location = new System.Drawing.Point(201, 0);
            this.ultraPanel5.Name = "ultraPanel5";
            this.ultraPanel5.Size = new System.Drawing.Size(681, 346);
            this.ultraPanel5.TabIndex = 24;
            // 
            // grdData
            // 
            appearance2.BackColor = System.Drawing.Color.Black;
            appearance2.BackColor2 = System.Drawing.Color.Black;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdData.DisplayLayout.Appearance = appearance2;
            ultraGridBand1.CardSettings.CardScrollbars = Infragistics.Win.UltraWinGrid.CardScrollbars.None;
            ultraGridBand1.CardSettings.ShowCaption = false;
            ultraGridBand1.Override.CellSpacing = 10;
            this.grdData.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdData.DisplayLayout.Override.BorderStyleCardArea = Infragistics.Win.UIElementBorderStyle.None;
            this.grdData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.grdData.DisplayLayout.Override.CardAreaAppearance = appearance3;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            this.grdData.DisplayLayout.Override.SelectedRowAppearance = appearance4;
            this.grdData.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdData.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Location = new System.Drawing.Point(0, 24);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(681, 322);
            this.grdData.TabIndex = 2;
            this.grdData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdData_InitializeLayout);
            this.grdData.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_CellChange);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabel2.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(681, 24);
            this.ultraLabel2.TabIndex = 23;
            this.ultraLabel2.Text = "Account Details";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.Enabled = false;
            this.ultraSplitter1.Location = new System.Drawing.Point(195, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 0;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 346);
            this.ultraSplitter1.TabIndex = 23;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.accTree);
            this.ultraPanel2.ClientArea.Controls.Add(this.txtFilterAccount);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(195, 346);
            this.ultraPanel2.TabIndex = 22;
            // 
            // accTree
            // 
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.ForeColor = System.Drawing.Color.White;
            this.accTree.Appearance = appearance5;
            this.accTree.ContextMenuStrip = this.contextMenuStrip1;
            this.accTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accTree.Location = new System.Drawing.Point(0, 24);
            this.accTree.Name = "accTree";
            _override1.BorderStyleNode = Infragistics.Win.UIElementBorderStyle.None;
            this.accTree.Override = _override1;
            this.accTree.Size = new System.Drawing.Size(195, 322);
            this.accTree.TabIndex = 21;
            this.accTree.UpdateMode = Infragistics.Win.UltraWinTree.UpdateMode.OnActiveCellChangeOrLostFocus;
            this.accTree.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.accTree_AfterSelect);
            this.accTree.ColumnSetGenerated += new Infragistics.Win.UltraWinTree.ColumnSetGeneratedEventHandler(this.accTree_ColumnSetGenerated);
            this.accTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.accTree_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemAddAccount,
            this.mnuItemAddSubAccount,
            this.mnuItemDeleteAccount,
            this.mnuItemDeleteSubAccount});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 92);
            this.inboxControlStyler1.SetStyleSettings(this.contextMenuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // mnuItemAddAccount
            // 
            this.mnuItemAddAccount.Name = "mnuItemAddAccount";
            this.mnuItemAddAccount.Size = new System.Drawing.Size(181, 22);
            this.mnuItemAddAccount.Text = "Add Sub Category";
            this.mnuItemAddAccount.Click += new System.EventHandler(this.mnuItemAddAccount_Click);
            // 
            // mnuItemAddSubAccount
            // 
            this.mnuItemAddSubAccount.Name = "mnuItemAddSubAccount";
            this.mnuItemAddSubAccount.Size = new System.Drawing.Size(181, 22);
            this.mnuItemAddSubAccount.Text = "Add Sub Account";
            this.mnuItemAddSubAccount.Click += new System.EventHandler(this.mnuItemAddSubAccount_Click);
            // 
            // mnuItemDeleteAccount
            // 
            this.mnuItemDeleteAccount.Name = "mnuItemDeleteAccount";
            this.mnuItemDeleteAccount.Size = new System.Drawing.Size(181, 22);
            this.mnuItemDeleteAccount.Text = "Delete Sub Category";
            this.mnuItemDeleteAccount.Click += new System.EventHandler(this.mnuItemDeleteAccount_Click);
            // 
            // mnuItemDeleteSubAccount
            // 
            this.mnuItemDeleteSubAccount.Name = "mnuItemDeleteSubAccount";
            this.mnuItemDeleteSubAccount.Size = new System.Drawing.Size(181, 22);
            this.mnuItemDeleteSubAccount.Text = "Delete Sub Account";
            this.mnuItemDeleteSubAccount.Click += new System.EventHandler(this.mnuItemDeleteSubAccount_Click);
            // 
            // txtFilterAccount
            // 
            this.txtFilterAccount.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFilterAccount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtFilterAccount.Location = new System.Drawing.Point(0, 0);
            this.txtFilterAccount.Name = "txtFilterAccount";
            this.txtFilterAccount.NullText = "Filter Sub Account";
            appearance6.ForeColor = System.Drawing.Color.DimGray;
            this.txtFilterAccount.NullTextAppearance = appearance6;
            this.txtFilterAccount.Size = new System.Drawing.Size(195, 24);
            this.txtFilterAccount.TabIndex = 22;
            this.txtFilterAccount.ValueChanged += new System.EventHandler(this.txtFilterAccount_ValueChanged);
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.lblActivityJournalMapping);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(882, 25);
            this.ultraPanel3.TabIndex = 26;
            // 
            // lblActivityJournalMapping
            // 
            this.lblActivityJournalMapping.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblActivityJournalMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActivityJournalMapping.Location = new System.Drawing.Point(0, 0);
            this.lblActivityJournalMapping.Name = "lblActivityJournalMapping";
            this.lblActivityJournalMapping.Size = new System.Drawing.Size(882, 25);
            this.lblActivityJournalMapping.TabIndex = 24;
            this.lblActivityJournalMapping.Text = "Cash Accounts";
            // 
            // ctrlAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "ctrlAccounts";
            this.Size = new System.Drawing.Size(882, 371);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ctrlAccounts_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel4.ClientArea.ResumeLayout(false);
            this.ultraPanel4.ResumeLayout(false);
            this.ultraPanel5.ClientArea.ResumeLayout(false);
            this.ultraPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accTree)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFilterAccount)).EndInit();
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAddAccount;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAddSubAccount;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDeleteAccount;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDeleteSubAccount;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinTree.UltraTree accTree;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdData;
        private Infragistics.Win.Misc.UltraLabel lblActivityJournalMapping;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.Misc.UltraPanel ultraPanel5;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFilterAccount;
    }
}
