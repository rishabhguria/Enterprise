using Prana.Utilities.UI.UIUtilities;

namespace Prana.Admin.Controls.PmPrefs
{
    partial class PMViewCopyCtrl
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.PanelBase = new Infragistics.Win.Misc.UltraPanel();
            this.checkboxExcludeCurrentUser = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkboxCreateNewTabIfNotExists = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkboxCopyViewsForAllUsers = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblCopyTo = new Infragistics.Win.Misc.UltraLabel();
            this.comboCopyTo = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.checkboxIncludeFilers = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkboxIncludeGrouping = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.buttonRun = new Infragistics.Win.Misc.UltraButton();
            this.comboCopyFrom = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblCopyFrom = new Infragistics.Win.Misc.UltraLabel();
            this.txtFolderBrowser = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.lblToUsername = new Infragistics.Win.Misc.UltraLabel();
            this.lblFromUsername = new Infragistics.Win.Misc.UltraLabel();
            this.comboToUsername = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.comboFromUsername = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.msgStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.preferenceBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.PanelBase.ClientArea.SuspendLayout();
            this.PanelBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxExcludeCurrentUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxCreateNewTabIfNotExists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxCopyViewsForAllUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeFilers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeGrouping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboCopyFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolderBrowser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboToUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFromUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.msgStatusBar)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelBase
            // 
            // 
            // PanelBase.ClientArea
            // 
            this.PanelBase.ClientArea.Controls.Add(this.checkboxExcludeCurrentUser);
            this.PanelBase.ClientArea.Controls.Add(this.checkboxCreateNewTabIfNotExists);
            this.PanelBase.ClientArea.Controls.Add(this.checkboxCopyViewsForAllUsers);
            this.PanelBase.ClientArea.Controls.Add(this.lblCopyTo);
            this.PanelBase.ClientArea.Controls.Add(this.comboCopyTo);
            this.PanelBase.ClientArea.Controls.Add(this.checkboxIncludeFilers);
            this.PanelBase.ClientArea.Controls.Add(this.checkboxIncludeGrouping);
            this.PanelBase.ClientArea.Controls.Add(this.buttonRun);
            this.PanelBase.ClientArea.Controls.Add(this.comboCopyFrom);
            this.PanelBase.ClientArea.Controls.Add(this.lblCopyFrom);
            this.PanelBase.ClientArea.Controls.Add(this.txtFolderBrowser);
            this.PanelBase.ClientArea.Controls.Add(this.buttonBrowse);
            this.PanelBase.ClientArea.Controls.Add(this.lblToUsername);
            this.PanelBase.ClientArea.Controls.Add(this.lblFromUsername);
            this.PanelBase.ClientArea.Controls.Add(this.comboToUsername);
            this.PanelBase.ClientArea.Controls.Add(this.comboFromUsername);
            this.PanelBase.ClientArea.Controls.Add(this.msgStatusBar);
            this.PanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelBase.Location = new System.Drawing.Point(0, 0);
            this.PanelBase.Name = "PanelBase";
            this.PanelBase.Size = new System.Drawing.Size(648, 222);
            this.PanelBase.TabIndex = 0;
            // 
            // checkboxExcludeCurrentUser
            // 
            this.checkboxExcludeCurrentUser.Checked = true;
            this.checkboxExcludeCurrentUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxExcludeCurrentUser.Location = new System.Drawing.Point(181, 131);
            this.checkboxExcludeCurrentUser.Name = "checkboxExcludeCurrentUser";
            this.checkboxExcludeCurrentUser.Size = new System.Drawing.Size(145, 20);
            this.checkboxExcludeCurrentUser.TabIndex = 26;
            this.checkboxExcludeCurrentUser.Text = "Exclude Current User";
            this.checkboxExcludeCurrentUser.Visible = false;
            // 
            // checkboxCreateNewTabIfNotExists
            // 
            this.checkboxCreateNewTabIfNotExists.Location = new System.Drawing.Point(358, 105);
            this.checkboxCreateNewTabIfNotExists.Name = "checkboxCreateNewTabIfNotExists";
            this.checkboxCreateNewTabIfNotExists.Size = new System.Drawing.Size(178, 20);
            this.checkboxCreateNewTabIfNotExists.TabIndex = 25;
            this.checkboxCreateNewTabIfNotExists.Text = "Create New Tab If Not Exists";
            // 
            // checkboxCopyViewsForAllUsers
            // 
            this.checkboxCopyViewsForAllUsers.Location = new System.Drawing.Point(7, 130);
            this.checkboxCopyViewsForAllUsers.Name = "checkboxCopyViewsForAllUsers";
            this.checkboxCopyViewsForAllUsers.Size = new System.Drawing.Size(145, 20);
            this.checkboxCopyViewsForAllUsers.TabIndex = 22;
            this.checkboxCopyViewsForAllUsers.Text = "Copy views for all users";
            this.checkboxCopyViewsForAllUsers.CheckedChanged += new System.EventHandler(this.checkboxCopyViewsForAllUsers_CheckedChanged);
            // 
            // lblCopyTo
            // 
            this.lblCopyTo.AutoSize = true;
            this.lblCopyTo.Location = new System.Drawing.Point(296, 68);
            this.lblCopyTo.Name = "lblCopyTo";
            this.lblCopyTo.Size = new System.Drawing.Size(47, 14);
            this.lblCopyTo.TabIndex = 16;
            this.lblCopyTo.Text = "Copy To";
            // 
            // comboCopyTo
            // 
            this.comboCopyTo.Location = new System.Drawing.Point(381, 65);
            this.comboCopyTo.Name = "comboCopyTo";
            this.comboCopyTo.Size = new System.Drawing.Size(158, 21);
            this.comboCopyTo.TabIndex = 17;
            this.comboCopyTo.TitleText = "View";
            // 
            // checkboxIncludeFilers
            // 
            this.checkboxIncludeFilers.Location = new System.Drawing.Point(181, 105);
            this.checkboxIncludeFilers.Name = "checkboxIncludeFilers";
            this.checkboxIncludeFilers.Size = new System.Drawing.Size(102, 20);
            this.checkboxIncludeFilers.TabIndex = 19;
            this.checkboxIncludeFilers.Text = "Include Filters";
            // 
            // checkboxIncludeGrouping
            // 
            this.checkboxIncludeGrouping.Checked = true;
            this.checkboxIncludeGrouping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxIncludeGrouping.Location = new System.Drawing.Point(7, 105);
            this.checkboxIncludeGrouping.Name = "checkboxIncludeGrouping";
            this.checkboxIncludeGrouping.Size = new System.Drawing.Size(111, 20);
            this.checkboxIncludeGrouping.TabIndex = 18;
            this.checkboxIncludeGrouping.Text = "Include Grouping";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(550, 103);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 20;
            this.buttonRun.Text = "Run";
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // comboCopyFrom
            // 
            this.comboCopyFrom.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.comboCopyFrom.Location = new System.Drawing.Point(104, 65);
            this.comboCopyFrom.Name = "comboCopyFrom";
            this.comboCopyFrom.NullText = "Select Tab Name";
            appearance1.ForeColor = System.Drawing.Color.Gray;
            this.comboCopyFrom.NullTextAppearance = appearance1;
            this.comboCopyFrom.Size = new System.Drawing.Size(158, 21);
            this.comboCopyFrom.TabIndex = 15;
            // 
            // lblCopyFrom
            // 
            this.lblCopyFrom.AutoSize = true;
            this.lblCopyFrom.Location = new System.Drawing.Point(7, 68);
            this.lblCopyFrom.Name = "lblCopyFrom";
            this.lblCopyFrom.Size = new System.Drawing.Size(60, 14);
            this.lblCopyFrom.TabIndex = 14;
            this.lblCopyFrom.Text = "Copy From";
            // 
            // txtFolderBrowser
            // 
            this.txtFolderBrowser.Location = new System.Drawing.Point(7, 38);
            this.txtFolderBrowser.Name = "txtFolderBrowser";
            this.txtFolderBrowser.NullText = "Browse File Path up to Prana Preferences Folder";
            appearance2.ForeColor = System.Drawing.Color.Gray;
            this.txtFolderBrowser.NullTextAppearance = appearance2;
            this.txtFolderBrowser.Size = new System.Drawing.Size(534, 21);
            this.txtFolderBrowser.TabIndex = 12;
            this.txtFolderBrowser.Leave += new System.EventHandler(this.txtFolderBrowser_Leave);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(550, 37);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 13;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // lblToUsername
            // 
            this.lblToUsername.AutoSize = true;
            this.lblToUsername.Location = new System.Drawing.Point(296, 15);
            this.lblToUsername.Name = "lblToUsername";
            this.lblToUsername.Size = new System.Drawing.Size(73, 14);
            this.lblToUsername.TabIndex = 10;
            this.lblToUsername.Text = "To Username";
            // 
            // lblFromUsername
            // 
            this.lblFromUsername.AutoSize = true;
            this.lblFromUsername.Location = new System.Drawing.Point(7, 15);
            this.lblFromUsername.Name = "lblFromUsername";
            this.lblFromUsername.Size = new System.Drawing.Size(86, 14);
            this.lblFromUsername.TabIndex = 8;
            this.lblFromUsername.Text = "From Username";
            // 
            // comboToUsername
            // 
            this.comboToUsername.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.comboToUsername.Location = new System.Drawing.Point(381, 12);
            this.comboToUsername.Name = "comboToUsername";
            this.comboToUsername.NullText = "Select Username";
            appearance3.ForeColor = System.Drawing.Color.Gray;
            this.comboToUsername.NullTextAppearance = appearance3;
            this.comboToUsername.Size = new System.Drawing.Size(158, 21);
            this.comboToUsername.TabIndex = 11;
            this.comboToUsername.Leave += new System.EventHandler(this.comboToUser_Leave);
            // 
            // comboFromUsername
            // 
            this.comboFromUsername.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.comboFromUsername.Location = new System.Drawing.Point(104, 12);
            this.comboFromUsername.Name = "comboFromUsername";
            this.comboFromUsername.NullText = "Select Username";
            appearance4.ForeColor = System.Drawing.Color.Gray;
            this.comboFromUsername.NullTextAppearance = appearance4;
            this.comboFromUsername.Size = new System.Drawing.Size(158, 21);
            this.comboFromUsername.TabIndex = 9;
            this.comboFromUsername.Leave += new System.EventHandler(this.comboFromUser_Leave);
            // 
            // msgStatusBar
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.msgStatusBar.Appearance = appearance5;
            this.msgStatusBar.Location = new System.Drawing.Point(0, 199);
            this.msgStatusBar.Name = "msgStatusBar";
            this.msgStatusBar.Size = new System.Drawing.Size(648, 23);
            this.msgStatusBar.TabIndex = 21;
            // 
            // PMViewCopyCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelBase);
            this.MinimumSize = new System.Drawing.Size(648, 210);
            this.Name = "PMViewCopyCtrl";
            this.Size = new System.Drawing.Size(648, 222);
            this.PanelBase.ClientArea.ResumeLayout(false);
            this.PanelBase.ClientArea.PerformLayout();
            this.PanelBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkboxExcludeCurrentUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxCreateNewTabIfNotExists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxCopyViewsForAllUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeFilers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeGrouping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboCopyFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolderBrowser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboToUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFromUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.msgStatusBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel PanelBase;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar msgStatusBar;
        private Infragistics.Win.Misc.UltraLabel lblToUsername;
        private Infragistics.Win.Misc.UltraLabel lblFromUsername;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor comboToUsername;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor comboFromUsername;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFolderBrowser;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.FolderBrowserDialog preferenceBrowser;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor comboCopyFrom;
        private Infragistics.Win.Misc.UltraLabel lblCopyFrom;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkboxIncludeFilers;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkboxIncludeGrouping;
        private Infragistics.Win.Misc.UltraButton buttonRun;
        private MultiSelectDropDown comboCopyTo;
        private Infragistics.Win.Misc.UltraLabel lblCopyTo;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkboxCopyViewsForAllUsers;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkboxCreateNewTabIfNotExists;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkboxExcludeCurrentUser;
    }
}