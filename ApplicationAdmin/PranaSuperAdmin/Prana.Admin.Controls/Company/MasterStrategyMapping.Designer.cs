using System.Windows.Forms;
using Infragistics.Win.UltraWinListView;

namespace Prana.Admin.Controls.Company
{
    partial class MasterStrategyMapping
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
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMasterStrategyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameMasterStrategyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMasterStrategyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uLblMasterStrategy = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.UBtnUnSelectAssignedStrategies = new Infragistics.Win.Misc.UltraButton();
            this.uBtnAllUnSelectAssignedStrategies = new Infragistics.Win.Misc.UltraButton();
            this.uBtnAllSelectUnassignedStrategies = new Infragistics.Win.Misc.UltraButton();
            this.uBtnSelectUnassignedStrategies = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.uTxtUnassignedStrategies = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.uLstViewUnAssignedStrategies = new Infragistics.Win.UltraWinListView.UltraListView();
            this.uLblUnAssignStrategies = new Infragistics.Win.Misc.UltraLabel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uTxtMasterStrategy = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.listMasterStrategy = new Infragistics.Win.UltraWinListView.UltraListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uTxtAssignedStrategies = new Prana.Admin.Controls.Company.WatermarkTextBox();
            this.uLblAssignedStrategy = new Infragistics.Win.Misc.UltraLabel();
            this.uLstViewAssignedStrategies = new Infragistics.Win.UltraWinListView.UltraListView();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.grdStrategyList = new Prana.Admin.Controls.GridStrategy();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.chkBoxManyToMany = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.menu.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewUnAssignedStrategies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listMasterStrategy)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewAssignedStrategies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxManyToMany)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMasterStrategyToolStripMenuItem,
            this.renameMasterStrategyToolStripMenuItem,
            this.deleteMasterStrategyToolStripMenuItem});
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(225, 70);
            // 
            // addMasterStrategyToolStripMenuItem
            // 
            this.addMasterStrategyToolStripMenuItem.Name = "addMasterStrategyToolStripMenuItem";
            this.addMasterStrategyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addMasterStrategyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.addMasterStrategyToolStripMenuItem.Text = "Add Master Strategy";
            this.addMasterStrategyToolStripMenuItem.Click += new System.EventHandler(this.addMasterStrategyToolStripMenuItem_Click);
            // 
            // renameMasterStrategyToolStripMenuItem
            // 
            this.renameMasterStrategyToolStripMenuItem.Name = "renameMasterStrategyToolStripMenuItem";
            this.renameMasterStrategyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.renameMasterStrategyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.renameMasterStrategyToolStripMenuItem.Text = "Rename Master Strategy";
            this.renameMasterStrategyToolStripMenuItem.Click += new System.EventHandler(this.renameMasterStrategyToolStripMenuItem_Click);
            // 
            // deleteMasterStrategyToolStripMenuItem
            // 
            this.deleteMasterStrategyToolStripMenuItem.Name = "deleteMasterStrategyToolStripMenuItem";
            this.deleteMasterStrategyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteMasterStrategyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.deleteMasterStrategyToolStripMenuItem.Text = "Delete Master Strategy";
            this.deleteMasterStrategyToolStripMenuItem.Click += new System.EventHandler(this.deleteMasterStrategyToolStripMenuItem_Click);
            // 
            // uLblMasterStrategy
            // 
            this.uLblMasterStrategy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblMasterStrategy.Location = new System.Drawing.Point(6, 9);
            this.uLblMasterStrategy.Name = "uLblMasterStrategy";
            this.uLblMasterStrategy.Size = new System.Drawing.Size(114, 19);
            this.uLblMasterStrategy.TabIndex = 2;
            this.uLblMasterStrategy.Text = "Master Strategy";
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.UBtnUnSelectAssignedStrategies);
            this.groupBox3.Controls.Add(this.uBtnAllUnSelectAssignedStrategies);
            this.groupBox3.Controls.Add(this.uBtnAllSelectUnassignedStrategies);
            this.groupBox3.Controls.Add(this.uBtnSelectUnassignedStrategies);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(65, 217);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // UBtnUnSelectAssignedStrategies
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance1.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.UBtnUnSelectAssignedStrategies.Appearance = appearance1;
            this.UBtnUnSelectAssignedStrategies.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.UBtnUnSelectAssignedStrategies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UBtnUnSelectAssignedStrategies.Location = new System.Drawing.Point(6, 42);
            this.UBtnUnSelectAssignedStrategies.Name = "UBtnUnSelectAssignedStrategies";
            this.UBtnUnSelectAssignedStrategies.ShowOutline = false;
            this.UBtnUnSelectAssignedStrategies.Size = new System.Drawing.Size(45, 23);
            this.UBtnUnSelectAssignedStrategies.TabIndex = 4;
            this.UBtnUnSelectAssignedStrategies.Text = ">";
            this.UBtnUnSelectAssignedStrategies.UseAppStyling = false;
            this.UBtnUnSelectAssignedStrategies.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.UBtnUnSelectAssignedStrategies.Click += new System.EventHandler(this.UBtnUnSelectAssignedStrategies_Click);
            // 
            // uBtnAllUnSelectAssignedStrategies
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance2.BorderColor = System.Drawing.Color.White;
            appearance2.BorderColor2 = System.Drawing.Color.White;
            appearance2.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnAllUnSelectAssignedStrategies.Appearance = appearance2;
            this.uBtnAllUnSelectAssignedStrategies.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnAllUnSelectAssignedStrategies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnAllUnSelectAssignedStrategies.Location = new System.Drawing.Point(6, 71);
            this.uBtnAllUnSelectAssignedStrategies.Name = "uBtnAllUnSelectAssignedStrategies";
            this.uBtnAllUnSelectAssignedStrategies.Size = new System.Drawing.Size(45, 23);
            this.uBtnAllUnSelectAssignedStrategies.TabIndex = 0;
            this.uBtnAllUnSelectAssignedStrategies.Text = ">>";
            this.uBtnAllUnSelectAssignedStrategies.UseAppStyling = false;
            this.uBtnAllUnSelectAssignedStrategies.Click += new System.EventHandler(this.uBtnAllUnSelectAssignedStrategies_Click);
            // 
            // uBtnAllSelectUnassignedStrategies
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnAllSelectUnassignedStrategies.Appearance = appearance3;
            this.uBtnAllSelectUnassignedStrategies.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnAllSelectUnassignedStrategies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnAllSelectUnassignedStrategies.Location = new System.Drawing.Point(6, 157);
            this.uBtnAllSelectUnassignedStrategies.Name = "uBtnAllSelectUnassignedStrategies";
            this.uBtnAllSelectUnassignedStrategies.Size = new System.Drawing.Size(45, 23);
            this.uBtnAllSelectUnassignedStrategies.TabIndex = 7;
            this.uBtnAllSelectUnassignedStrategies.Text = "<<";
            this.uBtnAllSelectUnassignedStrategies.UseAppStyling = false;
            this.uBtnAllSelectUnassignedStrategies.Click += new System.EventHandler(this.uBtnAllUnSelectUnassignedStrategies_Click);
            // 
            // uBtnSelectUnassignedStrategies
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uBtnSelectUnassignedStrategies.Appearance = appearance4;
            this.uBtnSelectUnassignedStrategies.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.uBtnSelectUnassignedStrategies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uBtnSelectUnassignedStrategies.Location = new System.Drawing.Point(6, 128);
            this.uBtnSelectUnassignedStrategies.Name = "uBtnSelectUnassignedStrategies";
            this.uBtnSelectUnassignedStrategies.Size = new System.Drawing.Size(45, 23);
            this.uBtnSelectUnassignedStrategies.TabIndex = 6;
            this.uBtnSelectUnassignedStrategies.Text = "<";
            this.uBtnSelectUnassignedStrategies.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.uBtnSelectUnassignedStrategies.UseAppStyling = false;
            this.uBtnSelectUnassignedStrategies.Click += new System.EventHandler(this.uBtnSelectUnassignedStrategies_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer2.Panel1MinSize = 0;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer2.Panel2MinSize = 0;
            this.splitContainer2.Size = new System.Drawing.Size(275, 217);
            this.splitContainer2.SplitterDistance = 65;
            this.splitContainer2.TabIndex = 23;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.uTxtUnassignedStrategies);
            this.groupBox4.Controls.Add(this.uLstViewUnAssignedStrategies);
            this.groupBox4.Controls.Add(this.uLblUnAssignStrategies);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox4.Size = new System.Drawing.Size(206, 217);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // uTxtUnassignedStrategies
            // 
            this.uTxtUnassignedStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtUnassignedStrategies.ForeColor = System.Drawing.Color.Gray;
            this.uTxtUnassignedStrategies.Location = new System.Drawing.Point(144, 9);
            this.uTxtUnassignedStrategies.Margin = new System.Windows.Forms.Padding(0);
            this.uTxtUnassignedStrategies.Name = "uTxtUnassignedStrategies";
            this.uTxtUnassignedStrategies.Size = new System.Drawing.Size(51, 20);
            this.uTxtUnassignedStrategies.TabIndex = 1;
            this.uTxtUnassignedStrategies.Text = "Search";
            this.uTxtUnassignedStrategies.WatermarkActive = true;
            this.uTxtUnassignedStrategies.WatermarkText = "Search";
            this.uTxtUnassignedStrategies.TextChanged += new System.EventHandler(this.uTxtUnassignedStrategies_TextChanged);
            // 
            // uLstViewUnAssignedStrategies
            // 
            this.uLstViewUnAssignedStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uLstViewUnAssignedStrategies.ItemSettings.HideSelection = false;
            this.uLstViewUnAssignedStrategies.Location = new System.Drawing.Point(36, 40);
            this.uLstViewUnAssignedStrategies.Margin = new System.Windows.Forms.Padding(0);
            this.uLstViewUnAssignedStrategies.Name = "uLstViewUnAssignedStrategies";
            this.uLstViewUnAssignedStrategies.Size = new System.Drawing.Size(159, 167);
            this.uLstViewUnAssignedStrategies.TabIndex = 9;
            this.uLstViewUnAssignedStrategies.Text = "ultraListView2";
            this.uLstViewUnAssignedStrategies.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.uLstViewUnAssignedStrategies.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewUnAssignedStrategies.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewUnAssignedStrategies.ViewSettingsList.MultiColumn = false;
            this.uLstViewUnAssignedStrategies.ViewSettingsTiles.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewUnAssignedStrategies.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.uLstViewUnAssignedStrategies_ItemSelectionChanged);
            // 
            // uLblUnAssignStrategies
            // 
            this.uLblUnAssignStrategies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblUnAssignStrategies.Location = new System.Drawing.Point(7, 9);
            this.uLblUnAssignStrategies.Name = "uLblUnAssignStrategies";
            this.uLblUnAssignStrategies.Size = new System.Drawing.Size(135, 20);
            this.uLblUnAssignStrategies.TabIndex = 0;
            this.uLblUnAssignStrategies.Text = "Unassigned Strategy";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(0);
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
            this.splitContainer3.Size = new System.Drawing.Size(636, 217);
            this.splitContainer3.SplitterDistance = 357;
            this.splitContainer3.TabIndex = 25;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
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
            this.splitContainer1.Size = new System.Drawing.Size(357, 217);
            this.splitContainer1.SplitterDistance = 171;
            this.splitContainer1.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uTxtMasterStrategy);
            this.groupBox2.Controls.Add(this.listMasterStrategy);
            this.groupBox2.Controls.Add(this.uLblMasterStrategy);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(171, 217);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // uTxtMasterStrategy
            // 
            this.uTxtMasterStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtMasterStrategy.ForeColor = System.Drawing.Color.Gray;
            this.uTxtMasterStrategy.Location = new System.Drawing.Point(116, 9);
            this.uTxtMasterStrategy.Name = "uTxtMasterStrategy";
            this.uTxtMasterStrategy.Size = new System.Drawing.Size(46, 20);
            this.uTxtMasterStrategy.TabIndex = 0;
            this.uTxtMasterStrategy.Text = "Search";
            this.uTxtMasterStrategy.WatermarkActive = true;
            this.uTxtMasterStrategy.WatermarkText = "Search";
            this.uTxtMasterStrategy.TextChanged += new System.EventHandler(this.uTxtMasterStrategy_TextChanged);
            // 
            // listMasterStrategy
            // 
            this.listMasterStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMasterStrategy.ContextMenuStrip = this.menu;
            this.listMasterStrategy.ItemSettings.HideSelection = false;
            this.listMasterStrategy.ItemSettings.HotTracking = true;
            this.listMasterStrategy.Location = new System.Drawing.Point(6, 42);
            this.listMasterStrategy.Name = "listMasterStrategy";
            this.listMasterStrategy.Size = new System.Drawing.Size(156, 167);
            this.listMasterStrategy.TabIndex = 1;
            this.listMasterStrategy.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.listMasterStrategy.ViewSettingsDetails.FullRowSelect = true;
            this.listMasterStrategy.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.listMasterStrategy.ViewSettingsList.MultiColumn = false;
            this.listMasterStrategy.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.listMasterStrategy_ItemActivated);
            this.listMasterStrategy.ItemDoubleClick += new Infragistics.Win.UltraWinListView.ItemDoubleClickEventHandler(this.listMasterStrategy_ItemDoubleClick);
            this.listMasterStrategy.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.listMasterStrategy_ItemSelectionChanged);
            this.listMasterStrategy.ItemEnteringEditMode += new Infragistics.Win.UltraWinListView.ItemEnteringEditModeEventHandler(this.listMasterStrategy_ItemEnteringEditMode);
            this.listMasterStrategy.ItemExitedEditMode += new Infragistics.Win.UltraWinListView.ItemExitedEditModeEventHandler(this.listMasterStrategy_ItemExitedEditMode);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uTxtAssignedStrategies);
            this.groupBox1.Controls.Add(this.uLblAssignedStrategy);
            this.groupBox1.Controls.Add(this.uLstViewAssignedStrategies);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 217);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // uTxtAssignedStrategies
            // 
            this.uTxtAssignedStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uTxtAssignedStrategies.ForeColor = System.Drawing.Color.Gray;
            this.uTxtAssignedStrategies.Location = new System.Drawing.Point(123, 9);
            this.uTxtAssignedStrategies.Name = "uTxtAssignedStrategies";
            this.uTxtAssignedStrategies.Size = new System.Drawing.Size(49, 20);
            this.uTxtAssignedStrategies.TabIndex = 2;
            this.uTxtAssignedStrategies.Text = "Search";
            this.uTxtAssignedStrategies.WatermarkActive = true;
            this.uTxtAssignedStrategies.WatermarkText = "Search";
            this.uTxtAssignedStrategies.TextChanged += new System.EventHandler(this.uTxtAssignedStrategies_TextChanged);
            // 
            // uLblAssignedStrategy
            // 
            this.uLblAssignedStrategy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLblAssignedStrategy.Location = new System.Drawing.Point(3, 9);
            this.uLblAssignedStrategy.Name = "uLblAssignedStrategy";
            this.uLblAssignedStrategy.Size = new System.Drawing.Size(125, 20);
            this.uLblAssignedStrategy.TabIndex = 0;
            this.uLblAssignedStrategy.Text = "Assigned Strategy";
            // 
            // uLstViewAssignedStrategies
            // 
            this.uLstViewAssignedStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uLstViewAssignedStrategies.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.uLstViewAssignedStrategies.ItemSettings.HideSelection = false;
            this.uLstViewAssignedStrategies.Location = new System.Drawing.Point(14, 42);
            this.uLstViewAssignedStrategies.Name = "uLstViewAssignedStrategies";
            this.uLstViewAssignedStrategies.Size = new System.Drawing.Size(158, 167);
            this.uLstViewAssignedStrategies.TabIndex = 1;
            this.uLstViewAssignedStrategies.Text = "ultraListView1";
            this.uLstViewAssignedStrategies.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.uLstViewAssignedStrategies.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewAssignedStrategies.ViewSettingsDetails.NullText = "Not available";
            this.uLstViewAssignedStrategies.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.uLstViewAssignedStrategies.ViewSettingsList.MultiColumn = false;
            this.uLstViewAssignedStrategies.ViewSettingsTiles.Alignment = Infragistics.Win.UltraWinListView.ItemAlignment.TopToBottom;
            this.uLstViewAssignedStrategies.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.uLstViewAssignedStrategies_ItemSelectionChanged);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.grdStrategyList);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(647, 458);
            this.splitContainer4.SplitterDistance = 204;
            this.splitContainer4.TabIndex = 27;
            // 
            // grdStrategyList
            // 
            this.grdStrategyList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdStrategyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdStrategyList.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdStrategyList.Location = new System.Drawing.Point(0, 0);
            this.grdStrategyList.Name = "grdStrategyList";
            this.grdStrategyList.Size = new System.Drawing.Size(647, 204);
            this.grdStrategyList.TabIndex = 0;
            this.grdStrategyList.Leave += new System.EventHandler(this.grdStrategyList_Leave);
            // 
            // splitContainer5
            // 
            this.splitContainer5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer5.Location = new System.Drawing.Point(5, -4);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.chkBoxManyToMany);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer5.Size = new System.Drawing.Size(636, 247);
            this.splitContainer5.SplitterDistance = 26;
            this.splitContainer5.TabIndex = 0;
            // 
            // chkBoxManyToMany
            // 
            this.chkBoxManyToMany.Enabled = false;
            this.chkBoxManyToMany.Location = new System.Drawing.Point(0, 0);
            this.chkBoxManyToMany.Name = "chkBoxManyToMany";
            this.chkBoxManyToMany.Size = new System.Drawing.Size(147, 25);
            this.chkBoxManyToMany.TabIndex = 0;
            this.chkBoxManyToMany.Text = "Many To Many Mapping";
            this.chkBoxManyToMany.CheckedChanged += new System.EventHandler(this.chkBoxManyToMany_CheckedChanged);
            // 
            // MasterStrategyMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.splitContainer4);
            this.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.Name = "MasterStrategyMapping";
            this.Size = new System.Drawing.Size(647, 458);
            this.Load += MasterStrategyMapping_Load;
            this.menu.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewUnAssignedStrategies)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listMasterStrategy)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uLstViewAssignedStrategies)).EndInit();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxManyToMany)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WatermarkTextBox uTxtMasterStrategy;
        private ContextMenuStrip menu;
        private ToolStripMenuItem addMasterStrategyToolStripMenuItem;
        private ToolStripMenuItem renameMasterStrategyToolStripMenuItem;
        private ToolStripMenuItem deleteMasterStrategyToolStripMenuItem;
        private Infragistics.Win.Misc.UltraLabel uLblMasterStrategy;
        private GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraButton UBtnUnSelectAssignedStrategies;
        private Infragistics.Win.Misc.UltraButton uBtnAllUnSelectAssignedStrategies;
        private Infragistics.Win.Misc.UltraButton uBtnAllSelectUnassignedStrategies;
        private Infragistics.Win.Misc.UltraButton uBtnSelectUnassignedStrategies;
        private SplitContainer splitContainer2;
        private GroupBox groupBox4;
        private WatermarkTextBox uTxtUnassignedStrategies;
        private UltraListView uLstViewUnAssignedStrategies;
        private Infragistics.Win.Misc.UltraLabel uLblUnAssignStrategies;
        private SplitContainer splitContainer3;
        private SplitContainer splitContainer1;
        private GroupBox groupBox2;
        private UltraListView listMasterStrategy;
        private GroupBox groupBox1;
        private WatermarkTextBox uTxtAssignedStrategies;
        private Infragistics.Win.Misc.UltraLabel uLblAssignedStrategy;
        private UltraListView uLstViewAssignedStrategies;
        private GridStrategy grdStrategyList;
        private SplitContainer splitContainer4;
        private SplitContainer splitContainer5;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxManyToMany;


    }
}
