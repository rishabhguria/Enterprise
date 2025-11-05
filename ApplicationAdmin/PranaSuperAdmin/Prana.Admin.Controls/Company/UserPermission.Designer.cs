namespace Prana.Admin.Controls.Company
{
    partial class UserPermission
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.grpPermission = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.uTxtSearchUserGroups = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.uLstUserGroups = new Infragistics.Win.UltraWinListView.UltraListView();
            this.uLblUserGroups = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.UBtnUnSelectAvailableGroups = new Infragistics.Win.Misc.UltraButton();
            this.uBtnAllUnSelectAvailableGroups = new Infragistics.Win.Misc.UltraButton();
            this.uBtnAllUnSelectUserGroups = new Infragistics.Win.Misc.UltraButton();
            this.uBtnUnSelectUserGroups = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uTxtSearchGroups = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.listAvailableGroups = new Infragistics.Win.UltraWinListView.UltraListView();
            this.uLblAvailableGroups = new Infragistics.Win.Misc.UltraLabel();
            this.grpPermission.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uLstUserGroups)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listAvailableGroups)).BeginInit();
            this.SuspendLayout();
            // 
            // grpPermission
            // 
            this.grpPermission.Controls.Add(this.groupBox4);
            this.grpPermission.Controls.Add(this.groupBox3);
            this.grpPermission.Controls.Add(this.groupBox2);
            this.grpPermission.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPermission.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpPermission.Location = new System.Drawing.Point(0, 0);
            this.grpPermission.Name = "grpPermission";
            this.grpPermission.Size = new System.Drawing.Size(589, 270);
            this.grpPermission.TabIndex = 0;
            this.grpPermission.TabStop = false;
            this.grpPermission.Text = "Permissioning";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.uTxtSearchUserGroups);
            this.groupBox4.Controls.Add(this.uLstUserGroups);
            this.groupBox4.Controls.Add(this.uLblUserGroups);
            this.groupBox4.Location = new System.Drawing.Point(360, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(187, 251);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            // 
            // uTxtSearchUserGroups
            // 
            this.uTxtSearchUserGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtSearchUserGroups.ForeColor = System.Drawing.Color.Gray;
            this.uTxtSearchUserGroups.Location = new System.Drawing.Point(111, 9);
            this.uTxtSearchUserGroups.Name = "uTxtSearchUserGroups";
            this.uTxtSearchUserGroups.Size = new System.Drawing.Size(69, 21);
            this.uTxtSearchUserGroups.TabIndex = 0;
            this.uTxtSearchUserGroups.Text = "Search";
            this.uTxtSearchUserGroups.WatermarkActive = true;
            this.uTxtSearchUserGroups.WatermarkText = "Search";
            this.uTxtSearchUserGroups.TextChanged += new System.EventHandler(this.uTxtSearchUserGroups_TextChanged);
            // 
            // uLstUserGroups
            // 
            this.uLstUserGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uLstUserGroups.ItemSettings.HideSelection = false;
            this.uLstUserGroups.Location = new System.Drawing.Point(6, 42);
            this.uLstUserGroups.Name = "uLstUserGroups";
            this.uLstUserGroups.Size = new System.Drawing.Size(175, 197);
            this.uLstUserGroups.TabIndex = 1;
            this.uLstUserGroups.Text = "ultraListView2";
            this.uLstUserGroups.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.uLstUserGroups.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstUserGroups.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstUserGroups.ViewSettingsList.MultiColumn = false;
            this.uLstUserGroups.ViewSettingsTiles.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstUserGroups.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.uLstUserGroups_ItemSelectionChanged);
            // 
            // uLblUserGroups
            // 
            this.uLblUserGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblUserGroups.Location = new System.Drawing.Point(3, 10);
            this.uLblUserGroups.Name = "uLblUserGroups";
            this.uLblUserGroups.Size = new System.Drawing.Size(121, 20);
            this.uLblUserGroups.TabIndex = 0;
            this.uLblUserGroups.Text = "User Groups";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.UBtnUnSelectAvailableGroups);
            this.groupBox3.Controls.Add(this.uBtnAllUnSelectAvailableGroups);
            this.groupBox3.Controls.Add(this.uBtnAllUnSelectUserGroups);
            this.groupBox3.Controls.Add(this.uBtnUnSelectUserGroups);
            this.groupBox3.Location = new System.Drawing.Point(251, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(85, 251);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // UBtnUnSelectAvailableGroups
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance1.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.UBtnUnSelectAvailableGroups.Appearance = appearance1;
            this.UBtnUnSelectAvailableGroups.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.UBtnUnSelectAvailableGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UBtnUnSelectAvailableGroups.Location = new System.Drawing.Point(19, 71);
            this.UBtnUnSelectAvailableGroups.Name = "UBtnUnSelectAvailableGroups";
            this.UBtnUnSelectAvailableGroups.ShowOutline = false;
            this.UBtnUnSelectAvailableGroups.Size = new System.Drawing.Size(45, 23);
            this.UBtnUnSelectAvailableGroups.TabIndex = 0;
            this.UBtnUnSelectAvailableGroups.Text = ">";
            this.UBtnUnSelectAvailableGroups.UseAppStyling = false;
            this.UBtnUnSelectAvailableGroups.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.UBtnUnSelectAvailableGroups.Click += new System.EventHandler(this.UBtnUnSelectAvailableGroups_Click);
            // 
            // uBtnAllUnSelectAvailableGroups
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance2.BorderColor = System.Drawing.Color.White;
            appearance2.BorderColor2 = System.Drawing.Color.White;
            appearance2.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnAllUnSelectAvailableGroups.Appearance = appearance2;
            this.uBtnAllUnSelectAvailableGroups.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnAllUnSelectAvailableGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnAllUnSelectAvailableGroups.Location = new System.Drawing.Point(19, 98);
            this.uBtnAllUnSelectAvailableGroups.Name = "uBtnAllUnSelectAvailableGroups";
            this.uBtnAllUnSelectAvailableGroups.Size = new System.Drawing.Size(45, 23);
            this.uBtnAllUnSelectAvailableGroups.TabIndex = 1;
            this.uBtnAllUnSelectAvailableGroups.Text = ">>";
            this.uBtnAllUnSelectAvailableGroups.UseAppStyling = false;
            this.uBtnAllUnSelectAvailableGroups.Click += new System.EventHandler(this.uBtnAllUnSelectAvailableGroups_Click);
            // 
            // uBtnAllUnSelectUserGroups
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnAllUnSelectUserGroups.Appearance = appearance3;
            this.uBtnAllUnSelectUserGroups.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnAllUnSelectUserGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnAllUnSelectUserGroups.Location = new System.Drawing.Point(19, 182);
            this.uBtnAllUnSelectUserGroups.Name = "uBtnAllUnSelectUserGroups";
            this.uBtnAllUnSelectUserGroups.Size = new System.Drawing.Size(45, 23);
            this.uBtnAllUnSelectUserGroups.TabIndex = 3;
            this.uBtnAllUnSelectUserGroups.Text = "<<";
            this.uBtnAllUnSelectUserGroups.UseAppStyling = false;
            this.uBtnAllUnSelectUserGroups.Click += new System.EventHandler(this.uBtnAllUnSelectUserGroups_Click);
            // 
            // uBtnUnSelectUserGroups
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnUnSelectUserGroups.Appearance = appearance4;
            this.uBtnUnSelectUserGroups.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnUnSelectUserGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnUnSelectUserGroups.Location = new System.Drawing.Point(19, 155);
            this.uBtnUnSelectUserGroups.Name = "uBtnUnSelectUserGroups";
            this.uBtnUnSelectUserGroups.Size = new System.Drawing.Size(45, 23);
            this.uBtnUnSelectUserGroups.TabIndex = 2;
            this.uBtnUnSelectUserGroups.Text = "<";
            this.uBtnUnSelectUserGroups.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.uBtnUnSelectUserGroups.UseAppStyling = false;
            this.uBtnUnSelectUserGroups.Click += new System.EventHandler(this.uBtnSelectUserGroups_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uTxtSearchGroups);
            this.groupBox2.Controls.Add(this.listAvailableGroups);
            this.groupBox2.Controls.Add(this.uLblAvailableGroups);
            this.groupBox2.Location = new System.Drawing.Point(41, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 251);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // uTxtSearchGroups
            // 
            this.uTxtSearchGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtSearchGroups.ForeColor = System.Drawing.Color.Gray;
            this.uTxtSearchGroups.Location = new System.Drawing.Point(112, 9);
            this.uTxtSearchGroups.Name = "uTxtSearchGroups";
            this.uTxtSearchGroups.Size = new System.Drawing.Size(69, 21);
            this.uTxtSearchGroups.TabIndex = 0;
            this.uTxtSearchGroups.Text = "Search";
            this.uTxtSearchGroups.WatermarkActive = true;
            this.uTxtSearchGroups.WatermarkText = "Search";
            this.uTxtSearchGroups.TextChanged += new System.EventHandler(this.uTxtSearchGroups_TextChanged);
            // 
            // listAvailableGroups
            // 
            this.listAvailableGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listAvailableGroups.ItemSettings.HideSelection = false;
            this.listAvailableGroups.ItemSettings.HotTracking = true;
            this.listAvailableGroups.Location = new System.Drawing.Point(6, 42);
            this.listAvailableGroups.Name = "listAvailableGroups";
            this.listAvailableGroups.Size = new System.Drawing.Size(175, 197);
            this.listAvailableGroups.TabIndex = 1;
            this.listAvailableGroups.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.listAvailableGroups.ViewSettingsDetails.FullRowSelect = true;
            this.listAvailableGroups.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.listAvailableGroups.ViewSettingsList.MultiColumn = false;
            this.listAvailableGroups.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.listAvailableGroups_ItemSelectionChanged);
            // 
            // uLblAvailableGroups
            // 
            this.uLblAvailableGroups.AutoSize = true;
            this.uLblAvailableGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblAvailableGroups.Location = new System.Drawing.Point(6, 10);
            this.uLblAvailableGroups.Name = "uLblAvailableGroups";
            this.uLblAvailableGroups.Size = new System.Drawing.Size(100, 15);
            this.uLblAvailableGroups.TabIndex = 2;
            this.uLblAvailableGroups.Text = "Available Groups";
            // 
            // UserPermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpPermission);
            this.MinimumSize = new System.Drawing.Size(589, 270);
            this.Name = "UserPermission";
            this.Size = new System.Drawing.Size(589, 270);
            this.grpPermission.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uLstUserGroups)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listAvailableGroups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPermission;
        private System.Windows.Forms.GroupBox groupBox2;
        private WatermarkTextBox uTxtSearchGroups;
        private Infragistics.Win.UltraWinListView.UltraListView listAvailableGroups;
        private Infragistics.Win.Misc.UltraLabel uLblAvailableGroups;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraButton UBtnUnSelectAvailableGroups;
        private Infragistics.Win.Misc.UltraButton uBtnAllUnSelectAvailableGroups;
        private Infragistics.Win.Misc.UltraButton uBtnAllUnSelectUserGroups;
        private Infragistics.Win.Misc.UltraButton uBtnUnSelectUserGroups;
        private System.Windows.Forms.GroupBox groupBox4;
        private WatermarkTextBox uTxtSearchUserGroups;
        private Infragistics.Win.UltraWinListView.UltraListView uLstUserGroups;
        private Infragistics.Win.Misc.UltraLabel uLblUserGroups;
    }
}
