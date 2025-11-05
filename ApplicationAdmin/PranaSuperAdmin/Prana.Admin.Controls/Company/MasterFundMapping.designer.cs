
using System.Windows.Forms;
using Infragistics.Win.UltraWinListView;
namespace Prana.Admin.Controls.Company
{
    partial class MasterFundMapping
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
                if (txtRenameAdd != null)
                {
                    txtRenameAdd.Dispose();
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.uLblMasterFund = new Infragistics.Win.Misc.UltraLabel();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMasterFundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameMasterFundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMasterFundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uLstViewAssignedAccounts = new Infragistics.Win.UltraWinListView.UltraListView();
            this.UBtnUnSelectAssignedAccounts = new Infragistics.Win.Misc.UltraButton();
            this.uBtnAllUnSelectAssignedAccounts = new Infragistics.Win.Misc.UltraButton();
            this.uBtnSelectUnassignedAccounts = new Infragistics.Win.Misc.UltraButton();
            this.uBtnAllUnSelectUnassignedAccounts = new Infragistics.Win.Misc.UltraButton();
            this.uLstViewUnAssignedAccounts = new Infragistics.Win.UltraWinListView.UltraListView();
            this.uLblAssignedAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.uLblUnAssignAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.uLblAssignedTradingAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uTxtAssignedAccounts = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uTxtMasterFund = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.listMasterFund = new Infragistics.Win.UltraWinListView.UltraListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.uTxtUnassignedAccounts = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton3 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton4 = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.chkBoxManyToMany = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ChkBoxShowmasterFundAsClient = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBoxShowMasterFundonTT = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cmbTradingAccounts = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewAssignedAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewUnAssignedAccounts)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listMasterFund)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxManyToMany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkBoxShowmasterFundAsClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxShowMasterFundonTT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // uLblMasterFund
            // 
            this.uLblMasterFund.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblMasterFund.Location = new System.Drawing.Point(6, 10);
            this.uLblMasterFund.Name = "uLblMasterFund";
            this.uLblMasterFund.Size = new System.Drawing.Size(90, 19);
            this.uLblMasterFund.TabIndex = 0;
            this.uLblMasterFund.Text = "Master Fund";
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMasterFundToolStripMenuItem,
            this.renameMasterFundToolStripMenuItem,
            this.deleteMasterFundToolStripMenuItem});
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(209, 70);
            // 
            // addMasterFundToolStripMenuItem
            // 
            this.addMasterFundToolStripMenuItem.Name = "addMasterFundToolStripMenuItem";
            this.addMasterFundToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addMasterFundToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.addMasterFundToolStripMenuItem.Text = "Add Master Fund";
            this.addMasterFundToolStripMenuItem.Click += new System.EventHandler(this.addMasterFundToolStripMenuItem_Click);
            // 
            // renameMasterFundToolStripMenuItem
            // 
            this.renameMasterFundToolStripMenuItem.Name = "renameMasterFundToolStripMenuItem";
            this.renameMasterFundToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.renameMasterFundToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.renameMasterFundToolStripMenuItem.Text = "Rename Master Fund";
            this.renameMasterFundToolStripMenuItem.Click += new System.EventHandler(this.renameMasterFundToolStripMenuItem_Click);
            // 
            // deleteMasterFundToolStripMenuItem
            // 
            this.deleteMasterFundToolStripMenuItem.Name = "deleteMasterFundToolStripMenuItem";
            this.deleteMasterFundToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteMasterFundToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.deleteMasterFundToolStripMenuItem.Text = "Delete Master Fund";
            this.deleteMasterFundToolStripMenuItem.Click += new System.EventHandler(this.deleteMasterFundToolStripMenuItem_Click);
            // 
            // uLstViewAssignedAccounts
            // 
            this.uLstViewAssignedAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uLstViewAssignedAccounts.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.uLstViewAssignedAccounts.ItemSettings.HideSelection = false;
            this.uLstViewAssignedAccounts.Location = new System.Drawing.Point(6, 42);
            this.uLstViewAssignedAccounts.Name = "uLstViewAssignedAccounts";
            this.uLstViewAssignedAccounts.Size = new System.Drawing.Size(152, 262);
            this.uLstViewAssignedAccounts.TabIndex = 0;
            this.uLstViewAssignedAccounts.Text = "ultraListView1";
            this.uLstViewAssignedAccounts.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.uLstViewAssignedAccounts.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewAssignedAccounts.ViewSettingsDetails.NullText = "Not available";
            this.uLstViewAssignedAccounts.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewAssignedAccounts.ViewSettingsList.MultiColumn = false;
            this.uLstViewAssignedAccounts.ViewSettingsTiles.Alignment = Infragistics.Win.UltraWinListView.ItemAlignment.TopToBottom;
            this.uLstViewAssignedAccounts.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.uLstViewAssignedAccounts_ItemSelectionChanged);
            // 
            // UBtnUnSelectAssignedAccounts
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance1.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.UBtnUnSelectAssignedAccounts.Appearance = appearance1;
            this.UBtnUnSelectAssignedAccounts.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.UBtnUnSelectAssignedAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UBtnUnSelectAssignedAccounts.Location = new System.Drawing.Point(19, 97);
            this.UBtnUnSelectAssignedAccounts.Name = "UBtnUnSelectAssignedAccounts";
            this.UBtnUnSelectAssignedAccounts.ShowOutline = false;
            this.UBtnUnSelectAssignedAccounts.Size = new System.Drawing.Size(45, 23);
            this.UBtnUnSelectAssignedAccounts.TabIndex = 3;
            this.UBtnUnSelectAssignedAccounts.Text = ">";
            this.UBtnUnSelectAssignedAccounts.UseAppStyling = false;
            this.UBtnUnSelectAssignedAccounts.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.UBtnUnSelectAssignedAccounts.Click += new System.EventHandler(this.UBtnUnSelectAssignedAccounts_Click);
            // 
            // uBtnAllUnSelectAssignedAccounts
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance2.BorderColor = System.Drawing.Color.White;
            appearance2.BorderColor2 = System.Drawing.Color.White;
            appearance2.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnAllUnSelectAssignedAccounts.Appearance = appearance2;
            this.uBtnAllUnSelectAssignedAccounts.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnAllUnSelectAssignedAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnAllUnSelectAssignedAccounts.Location = new System.Drawing.Point(19, 124);
            this.uBtnAllUnSelectAssignedAccounts.Name = "uBtnAllUnSelectAssignedAccounts";
            this.uBtnAllUnSelectAssignedAccounts.Size = new System.Drawing.Size(45, 23);
            this.uBtnAllUnSelectAssignedAccounts.TabIndex = 4;
            this.uBtnAllUnSelectAssignedAccounts.Text = ">>";
            this.uBtnAllUnSelectAssignedAccounts.UseAppStyling = false;
            this.uBtnAllUnSelectAssignedAccounts.Click += new System.EventHandler(this.uBtnAllUnSelectAssignedAccounts_Click);
            // 
            // uBtnSelectUnassignedAccounts
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnSelectUnassignedAccounts.Appearance = appearance3;
            this.uBtnSelectUnassignedAccounts.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnSelectUnassignedAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnSelectUnassignedAccounts.Location = new System.Drawing.Point(19, 181);
            this.uBtnSelectUnassignedAccounts.Name = "uBtnSelectUnassignedAccounts";
            this.uBtnSelectUnassignedAccounts.Size = new System.Drawing.Size(45, 23);
            this.uBtnSelectUnassignedAccounts.TabIndex = 5;
            this.uBtnSelectUnassignedAccounts.Text = "<";
            this.uBtnSelectUnassignedAccounts.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.uBtnSelectUnassignedAccounts.UseAppStyling = false;
            this.uBtnSelectUnassignedAccounts.Click += new System.EventHandler(this.uBtnSelectUnassignedAccounts_Click);
            // 
            // uBtnAllUnSelectUnassignedAccounts
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnAllUnSelectUnassignedAccounts.Appearance = appearance4;
            this.uBtnAllUnSelectUnassignedAccounts.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnAllUnSelectUnassignedAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnAllUnSelectUnassignedAccounts.Location = new System.Drawing.Point(19, 208);
            this.uBtnAllUnSelectUnassignedAccounts.Name = "uBtnAllUnSelectUnassignedAccounts";
            this.uBtnAllUnSelectUnassignedAccounts.Size = new System.Drawing.Size(45, 23);
            this.uBtnAllUnSelectUnassignedAccounts.TabIndex = 6;
            this.uBtnAllUnSelectUnassignedAccounts.Text = "<<";
            this.uBtnAllUnSelectUnassignedAccounts.UseAppStyling = false;
            this.uBtnAllUnSelectUnassignedAccounts.Click += new System.EventHandler(this.uBtnAllUnSelectUnassignedAccounts_Click);
            // 
            // uLstViewUnAssignedAccounts
            // 
            this.uLstViewUnAssignedAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uLstViewUnAssignedAccounts.ItemSettings.HideSelection = false;
            this.uLstViewUnAssignedAccounts.Location = new System.Drawing.Point(14, 42);
            this.uLstViewUnAssignedAccounts.Name = "uLstViewUnAssignedAccounts";
            this.uLstViewUnAssignedAccounts.Size = new System.Drawing.Size(172, 262);
            this.uLstViewUnAssignedAccounts.TabIndex = 0;
            this.uLstViewUnAssignedAccounts.Text = "ultraListView2";
            this.uLstViewUnAssignedAccounts.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.uLstViewUnAssignedAccounts.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewUnAssignedAccounts.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewUnAssignedAccounts.ViewSettingsList.MultiColumn = false;
            this.uLstViewUnAssignedAccounts.ViewSettingsTiles.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewUnAssignedAccounts.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.uLstViewUnAssignedAccounts_ItemSelectionChanged);
            // 
            // uLblAssignedAccounts
            // 
            this.uLblAssignedAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblAssignedAccounts.Location = new System.Drawing.Point(3, 10);
            this.uLblAssignedAccounts.Name = "uLblAssignedAccounts";
            this.uLblAssignedAccounts.Size = new System.Drawing.Size(105, 20);
            this.uLblAssignedAccounts.TabIndex = 0;
            this.uLblAssignedAccounts.Text = "Assigned Accounts";
            // 
            // uLblUnAssignAccounts
            // 
            this.uLblUnAssignAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblUnAssignAccounts.Location = new System.Drawing.Point(3, 10);
            this.uLblUnAssignAccounts.Name = "uLblUnAssignAccounts";
            this.uLblUnAssignAccounts.Size = new System.Drawing.Size(121, 20);
            this.uLblUnAssignAccounts.TabIndex = 0;
            this.uLblUnAssignAccounts.Text = "Unassigned Accounts";
            // 
            // uLblAssignedTradingAccounts
            // 
            this.uLblAssignedTradingAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblAssignedTradingAccounts.Location = new System.Drawing.Point(3, 18);
            this.uLblAssignedTradingAccounts.Name = "uLblAssignedTradingAccounts";
            this.uLblAssignedTradingAccounts.Size = new System.Drawing.Size(171, 20);
            this.uLblAssignedTradingAccounts.TabIndex = 0;
            this.uLblAssignedTradingAccounts.Text = "Mapped Trading Account";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.uLstViewAssignedAccounts);
            this.groupBox1.Controls.Add(this.uTxtAssignedAccounts);
            this.groupBox1.Controls.Add(this.uLblAssignedAccounts);
            this.groupBox1.Location = new System.Drawing.Point(0, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 316);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // uTxtAssignedAccounts
            // 
            this.uTxtAssignedAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtAssignedAccounts.ForeColor = System.Drawing.Color.Gray;
            this.uTxtAssignedAccounts.Location = new System.Drawing.Point(115, 10);
            this.uTxtAssignedAccounts.Name = "uTxtAssignedAccounts";
            this.uTxtAssignedAccounts.Size = new System.Drawing.Size(43, 20);
            this.uTxtAssignedAccounts.TabIndex = 1;
            this.uTxtAssignedAccounts.Text = "Search";
            this.uTxtAssignedAccounts.WatermarkActive = true;
            this.uTxtAssignedAccounts.WatermarkText = "Search";
            this.uTxtAssignedAccounts.TextChanged += new System.EventHandler(this.uTxtAssignedAccounts_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.uTxtMasterFund);
            this.groupBox2.Controls.Add(this.listMasterFund);
            this.groupBox2.Controls.Add(this.uLblMasterFund);
            this.groupBox2.Location = new System.Drawing.Point(0, -2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(172, 316);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // uTxtMasterFund
            // 
            this.uTxtMasterFund.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtMasterFund.ForeColor = System.Drawing.Color.Gray;
            this.uTxtMasterFund.Location = new System.Drawing.Point(98, 10);
            this.uTxtMasterFund.Name = "uTxtMasterFund";
            this.uTxtMasterFund.Size = new System.Drawing.Size(66, 20);
            this.uTxtMasterFund.TabIndex = 1;
            this.uTxtMasterFund.Text = "Search";
            this.uTxtMasterFund.WatermarkActive = true;
            this.uTxtMasterFund.WatermarkText = "Search";
            this.uTxtMasterFund.TextChanged += new System.EventHandler(this.uTxtMasterFund_TextChanged);
            // 
            // listMasterFund
            // 
            this.listMasterFund.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMasterFund.ContextMenuStrip = this.menu;
            this.listMasterFund.ItemSettings.HideSelection = false;
            this.listMasterFund.ItemSettings.HotTracking = true;
            this.listMasterFund.Location = new System.Drawing.Point(6, 42);
            this.listMasterFund.Name = "listMasterFund";
            this.listMasterFund.Size = new System.Drawing.Size(161, 262);
            this.listMasterFund.TabIndex = 1;
            this.listMasterFund.ItemSettings.SelectionType = SelectionType.Single;
            this.listMasterFund.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.listMasterFund.ViewSettingsDetails.FullRowSelect = true;
            this.listMasterFund.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.listMasterFund.ViewSettingsList.MultiColumn = false;
            this.listMasterFund.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.listMasterFund_ItemActivated);
            this.listMasterFund.ItemDoubleClick += new Infragistics.Win.UltraWinListView.ItemDoubleClickEventHandler(this.listMasterFund_ItemDoubleClick);
            this.listMasterFund.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.listMasterFund_ItemSelectionChanged);
            this.listMasterFund.ItemEnteringEditMode += new Infragistics.Win.UltraWinListView.ItemEnteringEditModeEventHandler(this.listMasterFund_ItemEnteringEditMode);
            this.listMasterFund.ItemExitedEditMode += new Infragistics.Win.UltraWinListView.ItemExitedEditModeEventHandler(this.listMasterFund_ItemExitedEditMode);
            this.listMasterFund.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listMasterFund_MouseDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.UBtnUnSelectAssignedAccounts);
            this.groupBox3.Controls.Add(this.uBtnAllUnSelectAssignedAccounts);
            this.groupBox3.Controls.Add(this.uBtnAllUnSelectUnassignedAccounts);
            this.groupBox3.Controls.Add(this.uBtnSelectUnassignedAccounts);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(91, 316);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.uTxtUnassignedAccounts);
            this.groupBox4.Controls.Add(this.uLstViewUnAssignedAccounts);
            this.groupBox4.Controls.Add(this.uLblUnAssignAccounts);
            this.groupBox4.Location = new System.Drawing.Point(8, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(199, 316);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // uTxtUnassignedAccounts
            // 
            this.uTxtUnassignedAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtUnassignedAccounts.ForeColor = System.Drawing.Color.Gray;
            this.uTxtUnassignedAccounts.Location = new System.Drawing.Point(125, 10);
            this.uTxtUnassignedAccounts.Name = "uTxtUnassignedAccounts";
            this.uTxtUnassignedAccounts.Size = new System.Drawing.Size(61, 20);
            this.uTxtUnassignedAccounts.TabIndex = 0;
            this.uTxtUnassignedAccounts.Text = "Search";
            this.uTxtUnassignedAccounts.WatermarkActive = true;
            this.uTxtUnassignedAccounts.WatermarkText = "Search";
            this.uTxtUnassignedAccounts.TextChanged += new System.EventHandler(this.uTxtUnassignedAccounts_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.uLblAssignedTradingAccounts);
            this.groupBox6.Controls.Add(this.cmbTradingAccounts);
            this.groupBox6.Location = new System.Drawing.Point(8, 320);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(655, 50);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(5, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1MinSize = 8;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2MinSize = 8;
            this.splitContainer1.Size = new System.Drawing.Size(342, 318);
            this.splitContainer1.SplitterDistance = 173;
            this.splitContainer1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox5);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer2.Panel1MinSize = 0;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer2.Panel2MinSize = 0;
            this.splitContainer2.Size = new System.Drawing.Size(306, 318);
            this.splitContainer2.SplitterDistance = 91;
            this.splitContainer2.TabIndex = 23;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.ultraButton1);
            this.groupBox5.Controls.Add(this.ultraButton2);
            this.groupBox5.Controls.Add(this.ultraButton3);
            this.groupBox5.Controls.Add(this.ultraButton4);
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(91, 316);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            // 
            // ultraButton1
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance5.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraButton1.Appearance = appearance5;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.ultraButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton1.Location = new System.Drawing.Point(19, 97);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.ShowOutline = false;
            this.ultraButton1.Size = new System.Drawing.Size(45, 23);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = ">";
            this.ultraButton1.UseAppStyling = false;
            this.ultraButton1.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraButton1.Click += new System.EventHandler(this.UBtnUnSelectAssignedAccounts_Click);
            // 
            // ultraButton2
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance6.BorderColor = System.Drawing.Color.White;
            appearance6.BorderColor2 = System.Drawing.Color.White;
            appearance6.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraButton2.Appearance = appearance6;
            this.ultraButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.ultraButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton2.Location = new System.Drawing.Point(19, 124);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(45, 23);
            this.ultraButton2.TabIndex = 1;
            this.ultraButton2.Text = ">>";
            this.ultraButton2.UseAppStyling = false;
            this.ultraButton2.Click += new System.EventHandler(this.uBtnAllUnSelectAssignedAccounts_Click);
            // 
            // ultraButton3
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraButton3.Appearance = appearance7;
            this.ultraButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.ultraButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton3.Location = new System.Drawing.Point(19, 208);
            this.ultraButton3.Name = "ultraButton3";
            this.ultraButton3.Size = new System.Drawing.Size(45, 23);
            this.ultraButton3.TabIndex = 3;
            this.ultraButton3.Text = "<<";
            this.ultraButton3.UseAppStyling = false;
            this.ultraButton3.Click += new System.EventHandler(this.uBtnAllUnSelectUnassignedAccounts_Click);
            // 
            // ultraButton4
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance8.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraButton4.Appearance = appearance8;
            this.ultraButton4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.ultraButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton4.Location = new System.Drawing.Point(19, 181);
            this.ultraButton4.Name = "ultraButton4";
            this.ultraButton4.Size = new System.Drawing.Size(45, 23);
            this.ultraButton4.TabIndex = 2;
            this.ultraButton4.Text = "<";
            this.ultraButton4.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ultraButton4.UseAppStyling = false;
            this.ultraButton4.Click += new System.EventHandler(this.uBtnSelectUnassignedAccounts_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.MinimumSize = new System.Drawing.Size(600, 300);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainer3.Panel1MinSize = 0;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer3.Panel2MinSize = 20;
            this.splitContainer3.Size = new System.Drawing.Size(672, 325);
            this.splitContainer3.SplitterDistance = 357;
            this.splitContainer3.TabIndex = 24;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Location = new System.Drawing.Point(0, 2);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.chkBoxManyToMany);
            this.splitContainer4.Panel1.Controls.Add(this.ChkBoxShowmasterFundAsClient);
            this.splitContainer4.Panel1.Controls.Add(this.chkBoxShowMasterFundonTT);

            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer4.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer4.Size = new System.Drawing.Size(700, 700);
            this.splitContainer4.SplitterDistance = 75;
            this.splitContainer4.TabIndex = 0;
            // 
            // chkBoxManyToMany
            // 
            this.chkBoxManyToMany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxManyToMany.Enabled = false;
            this.chkBoxManyToMany.Location = new System.Drawing.Point(6, 15);
            this.chkBoxManyToMany.Name = "chkBoxManyToMany";
            this.chkBoxManyToMany.Size = new System.Drawing.Size(154, 14);
            this.chkBoxManyToMany.TabIndex = 1;
            this.chkBoxManyToMany.Text = "Many To Many Mapping";
            this.chkBoxManyToMany.CheckedChanged += new System.EventHandler(this.chkBoxManyToMany_CheckedChanged);
            // 
            // ChkBoxShowmasterFundAsClient
            // 
            this.ChkBoxShowmasterFundAsClient.Location = new System.Drawing.Point(6, 35);
            this.ChkBoxShowmasterFundAsClient.Name = "ChkBoxShowmasterFundAsClient";
            this.ChkBoxShowmasterFundAsClient.Size = new System.Drawing.Size(300, 14);
            this.ChkBoxShowmasterFundAsClient.TabIndex = 0;
            this.ChkBoxShowmasterFundAsClient.Text = "Show Master Fund as Client";
            this.ChkBoxShowmasterFundAsClient.CheckedChanged += new System.EventHandler(this.ChkBoxShowmasterFundAsClient_CheckedChanged);
            // 
            // chkBoxShowMasterFundonTT
            // 
            this.chkBoxShowMasterFundonTT.Location = new System.Drawing.Point(6, 55);
            this.chkBoxShowMasterFundonTT.Name = "chkBoxShowMasterFundonTT";
            this.chkBoxShowMasterFundonTT.Size = new System.Drawing.Size(300, 14);
            this.chkBoxShowMasterFundonTT.TabIndex = 1;
            this.chkBoxShowMasterFundonTT.Text = "Show Master Fund on TT";
            this.chkBoxShowMasterFundonTT.CheckedChanged += new System.EventHandler(this.chkBoxShowMasterFundonTT_CheckedChanged);
            // 
            // cmbClosing
            // 
            this.cmbTradingAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbTradingAccounts.Location = new System.Drawing.Point(182, 16);
            this.cmbTradingAccounts.Name = "cmbClosing";
            this.cmbTradingAccounts.Size = new System.Drawing.Size(136, 22);
            this.cmbTradingAccounts.TabIndex = 4;
            this.cmbTradingAccounts.ValueChanged += cmbTradingAccounts_ValueChanged;
            // 
            // MasterFundMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.splitContainer4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.MinimumSize = new System.Drawing.Size(660, 383);
            this.Name = "MasterFundMapping";
            this.Size = new System.Drawing.Size(675, 383);
            this.menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewAssignedAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewUnAssignedAccounts)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listMasterFund)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxManyToMany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkBoxShowmasterFundAsClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxShowMasterFundonTT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccounts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel uLblMasterFund;
        private Infragistics.Win.UltraWinListView.UltraListView uLstViewAssignedAccounts;
        private Infragistics.Win.Misc.UltraButton UBtnUnSelectAssignedAccounts;
        private Infragistics.Win.Misc.UltraButton uBtnAllUnSelectAssignedAccounts;
        private Infragistics.Win.Misc.UltraButton uBtnSelectUnassignedAccounts;
        private Infragistics.Win.Misc.UltraButton uBtnAllUnSelectUnassignedAccounts;
        private Infragistics.Win.UltraWinListView.UltraListView uLstViewUnAssignedAccounts;
        private Infragistics.Win.Misc.UltraLabel uLblAssignedAccounts;
        private Infragistics.Win.Misc.UltraLabel uLblUnAssignAccounts;
        private Infragistics.Win.Misc.UltraLabel uLblAssignedTradingAccounts;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private WatermarkTextBox uTxtMasterFund;
        private WatermarkTextBox uTxtAssignedAccounts;
        private WatermarkTextBox uTxtUnassignedAccounts;
        private System.Windows.Forms.ContextMenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem addMasterFundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameMasterFundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMasterFundToolStripMenuItem;
        private SplitContainer splitContainer4;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxManyToMany;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxShowMasterFundonTT;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ChkBoxShowmasterFundAsClient;
        private UltraListView listMasterFund;
        private GroupBox groupBox5;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private Infragistics.Win.Misc.UltraButton ultraButton3;
        private Infragistics.Win.Misc.UltraButton ultraButton4;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTradingAccounts;

    }
}
