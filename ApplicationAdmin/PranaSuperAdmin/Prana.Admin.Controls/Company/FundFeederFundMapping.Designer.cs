namespace Prana.Admin.Controls.Company
{
    partial class AccountFeederAccountMapping
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
                _currencyList.Dispose();
                _feederList.Dispose();


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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.lblAccount = new System.Windows.Forms.Label();
            this.lblFeederAccounts = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.ulvAccounts = new Infragistics.Win.UltraWinListView.UltraListView();
            this.ugvFeederAccounts = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.ulvAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugvFeederAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccount.Location = new System.Drawing.Point(17, 18);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(39, 15);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Account";
            // 
            // lblFeederAccounts
            // 
            this.lblFeederAccounts.AutoSize = true;
            this.lblFeederAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeederAccounts.Location = new System.Drawing.Point(170, 18);
            this.lblFeederAccounts.Name = "lblFeederAccounts";
            this.lblFeederAccounts.Size = new System.Drawing.Size(95, 15);
            this.lblFeederAccounts.TabIndex = 3;
            this.lblFeederAccounts.Text = "Feeder Accounts";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnAdd.BackgroundImage = global::Prana.Admin.Controls.Properties.Resources.btn_add;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.Location = new System.Drawing.Point(173, 254);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnRemove.BackgroundImage = global::Prana.Admin.Controls.Properties.Resources.btn_remove;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemove.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnRemove.Location = new System.Drawing.Point(253, 254);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ulvAccounts
            // 
            this.ulvAccounts.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ulvAccounts.ItemSettings.HideSelection = false;
            this.ulvAccounts.ItemSettings.HotTracking = true;
            this.ulvAccounts.Location = new System.Drawing.Point(7, 36);
            this.ulvAccounts.Name = "ulvAccounts";
            this.ulvAccounts.Size = new System.Drawing.Size(117, 212);
            this.ulvAccounts.TabIndex = 1;
            this.ulvAccounts.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ulvAccounts.ViewSettingsDetails.FullRowSelect = true;
            this.ulvAccounts.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.ulvAccounts.ViewSettingsList.MultiColumn = false;
            this.ulvAccounts.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.ulvAccounts_ItemActivated);
            this.ulvAccounts.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.ulvAccounts_ItemSelectionChanged);
            // 
            // ugvFeederAccounts
            // 
            this.ugvFeederAccounts.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugvFeederAccounts.DisplayLayout.Appearance = appearance1;
            this.ugvFeederAccounts.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugvFeederAccounts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ugvFeederAccounts.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugvFeederAccounts.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ugvFeederAccounts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugvFeederAccounts.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ugvFeederAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.ugvFeederAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugvFeederAccounts.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ugvFeederAccounts.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ugvFeederAccounts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugvFeederAccounts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ugvFeederAccounts.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugvFeederAccounts.DisplayLayout.Override.CellAppearance = appearance8;
            this.ugvFeederAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugvFeederAccounts.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ugvFeederAccounts.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ugvFeederAccounts.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ugvFeederAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugvFeederAccounts.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ugvFeederAccounts.DisplayLayout.Override.RowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugvFeederAccounts.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ugvFeederAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugvFeederAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugvFeederAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugvFeederAccounts.Location = new System.Drawing.Point(156, 36);
            this.ugvFeederAccounts.Name = "ugvFeederAccounts";
            this.ugvFeederAccounts.Size = new System.Drawing.Size(352, 212);
            this.ugvFeederAccounts.TabIndex = 2;
            this.ugvFeederAccounts.Text = "ultraGrid1";
            this.ugvFeederAccounts.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugvFeederAccounts_AfterCellUpdate);
            this.ugvFeederAccounts.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugvFeederAccounts_InitializeLayout);
            this.ugvFeederAccounts.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugvFeederAccounts_CellChange);
            this.ugvFeederAccounts.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugvFeederAccounts_BeforeCellListDropDown);
            // 
            // AccountFeederAccountMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ulvAccounts);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lblFeederAccounts);
            this.Controls.Add(this.ugvFeederAccounts);
            this.Controls.Add(this.lblAccount);
            this.Name = "AccountFeederAccountMapping";
            this.Size = new System.Drawing.Size(559, 295);
            this.Load += new System.EventHandler(this.AccountFeederAccountMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ulvAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugvFeederAccounts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.Label lblFeederAccounts;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private Infragistics.Win.UltraWinListView.UltraListView ulvAccounts;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugvFeederAccounts;
    }
}
