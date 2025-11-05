using Prana.CashManagement.Controls;
using Prana.Global;
namespace Prana.CashManagement
{
    partial class CashAccountsUI
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
                if (_dsActivities != null)
                {
                    _dsActivities.Dispose();
                }
                if (_vlCashActivityType != null)
                {
                    _vlCashActivityType.Dispose();
                    _vlCashActivityType = null;
                }
                if (_vlSubAccount != null)
                {
                    _vlSubAccount.Dispose();
                    _vlSubAccount = null;
                }
                if (_proxy != null)
                {
                    _proxy.Dispose();
                    _proxy = null;
                }
                if (_vlActivityType != null)
                {
                    _vlActivityType.Dispose();
                    _vlActivityType = null;
                }
                if (_vlAmountType != null)
                {
                    _vlAmountType.Dispose();
                    _vlAmountType = null;
                }
                if (_vlCashTransactionType != null)
                {
                    _vlCashTransactionType.Dispose();
                    _vlCashTransactionType = null;
                }
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            //this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemAddAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemAddSubAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemDeleteAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemDeleteSubAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemAddJournalCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.tabCtrlActivities = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.btnRestoreDefault = new Infragistics.Win.Misc.UltraButton();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.CashAccountsUI_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ctrlAccounts1 = new Prana.CashManagement.ctrlAccounts();
            this.ctrlActivityType1 = new Prana.CashManagement.ctrlActivityType();
            this.ctrlActivityJournalMapping1 = new Prana.CashManagement.ctrlActivityJournalMapping();
            this.ctrlSubAccountType = new ctrlSubAccountType();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            this.ultraTabPageControl4.SuspendLayout();
            this.ultraTabPageControl5.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlActivities)).BeginInit();
            this.tabCtrlActivities.SuspendLayout();
            this.Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.CashAccountsUI_Fill_Panel.ClientArea.SuspendLayout();
            this.CashAccountsUI_Fill_Panel.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlAccounts1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 25);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1048, 329);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ctrlActivityType1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1048, 329);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.ctrlActivityJournalMapping1);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(1048, 329);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.ctrlSubAccountType);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(1048, 329);
            // 
            // ultraLabel1
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(53)))), ((int)(((byte)(134)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(46)))), ((int)(((byte)(109)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.SizeInPoints = 14F;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabel1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(1070, 29);
            this.ultraLabel1.TabIndex = 14;
            this.ultraLabel1.Text = "CashAccounts";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.inboxControlStyler1.SetStyleSettings(this.contextMenuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // mnuItemAddAccount
            // 
            this.mnuItemAddAccount.Name = "mnuItemAddAccount";
            this.mnuItemAddAccount.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuItemAddSubAccount
            // 
            this.mnuItemAddSubAccount.Name = "mnuItemAddSubAccount";
            this.mnuItemAddSubAccount.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuItemDeleteAccount
            // 
            this.mnuItemDeleteAccount.Name = "mnuItemDeleteAccount";
            this.mnuItemDeleteAccount.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuItemDeleteSubAccount
            // 
            this.mnuItemDeleteSubAccount.Name = "mnuItemDeleteSubAccount";
            this.mnuItemDeleteSubAccount.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuItemAddJournalCodeToolStripMenuItem
            // 
            this.mnuItemAddJournalCodeToolStripMenuItem.Name = "mnuItemAddJournalCodeToolStripMenuItem";
            this.mnuItemAddJournalCodeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.mnuItemAddJournalCodeToolStripMenuItem.Text = "Add Journal Code";
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(0, 0);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(200, 100);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Location = new System.Drawing.Point(0, 0);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(200, 100);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.tabCtrlActivities);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1052, 357);
            this.ultraPanel1.TabIndex = 1;
            // 
            // tabCtrlActivities
            // 
            appearance2.FontData.SizeInPoints = 9F;
            this.tabCtrlActivities.Appearance = appearance2;
            this.tabCtrlActivities.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabCtrlActivities.Controls.Add(this.ultraTabPageControl1);
            this.tabCtrlActivities.Controls.Add(this.ultraTabPageControl3);
            this.tabCtrlActivities.Controls.Add(this.ultraTabPageControl4);
            this.tabCtrlActivities.Controls.Add(this.ultraTabPageControl5);
            this.tabCtrlActivities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlActivities.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrlActivities.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlActivities.Name = "tabCtrlActivities";
            this.tabCtrlActivities.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabCtrlActivities.Size = new System.Drawing.Size(1052, 357);
            this.tabCtrlActivities.TabIndex = 0;
            ultraTab1.Key = "tabCashAccounts";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "CashAccounts";
            ultraTab3.Key = "tabActivityTypes";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Activity Types";
            ultraTab4.Key = "tabActivityJournalMapping";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Activity Journal Mapping";
            ultraTab5.Key = "tabSubAccountType";
            ultraTab5.TabPage = this.ultraTabPageControl5;
            ultraTab5.Text = "Sub Account Type";
            this.tabCtrlActivities.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab3,
            ultraTab4,
            ultraTab5});
            this.tabCtrlActivities.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabCtrlActivities_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1048, 329);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance3.FontData.SizeInPoints = 9F;
            this.ultraButton1.Appearance = appearance3;
            this.ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton1.Location = new System.Drawing.Point(572, 12);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(75, 23);
            this.ultraButton1.TabIndex = 19;
            this.ultraButton1.Text = "Save";
            this.ultraButton1.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance4.FontData.SizeInPoints = 9F;
            this.btnDelete.Appearance = appearance4;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(406, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance5.FontData.SizeInPoints = 9F;
            this.btnAdd.Appearance = appearance5;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(489, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRestoreDefault
            // 
            this.btnRestoreDefault.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance6.FontData.SizeInPoints = 9F;
            this.btnRestoreDefault.Appearance = appearance6;
            this.btnRestoreDefault.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreDefault.Location = new System.Drawing.Point(89, 12);
            this.btnRestoreDefault.Name = "btnRestoreDefault";
            this.btnRestoreDefault.Size = new System.Drawing.Size(104, 23);
            this.btnRestoreDefault.TabIndex = 20;
            this.btnRestoreDefault.Text = "Restore Default";
            this.btnRestoreDefault.Click += new System.EventHandler(this.btnRestoreDefault_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance7.FontData.SizeInPoints = 9F;
            this.btnExport.Appearance = appearance7;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(3, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 23);
            this.btnExport.TabIndex = 21;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.Status.Location = new System.Drawing.Point(4, 430);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(1052, 22);
            this.inboxControlStyler1.SetStyleSettings(this.Status, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Status.TabIndex = 9;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1037, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // CashAccountsUI_Fill_Panel
            // 
            // 
            // CashAccountsUI_Fill_Panel.ClientArea
            // 
            this.CashAccountsUI_Fill_Panel.ClientArea.Controls.Add(this.ultraPanel1);
            this.CashAccountsUI_Fill_Panel.ClientArea.Controls.Add(this.ultraPanel2);
            this.CashAccountsUI_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.CashAccountsUI_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CashAccountsUI_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.CashAccountsUI_Fill_Panel.Name = "CashAccountsUI_Fill_Panel";
            this.CashAccountsUI_Fill_Panel.Size = new System.Drawing.Size(1052, 403);
            this.CashAccountsUI_Fill_Panel.TabIndex = 10;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.btnRefresh);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraButton1);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnDelete);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnAdd);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnRestoreDefault);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnExport);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 357);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(1052, 46);
            this.ultraPanel2.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance8.FontData.SizeInPoints = 9F;
            this.btnRefresh.Appearance = appearance8;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(655, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // _CashAccountsUI_UltraFormManager_Dock_Area_Left
            // 
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.Name = "_CashAccountsUI_UltraFormManager_Dock_Area_Left";
            this._CashAccountsUI_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 425);
            // 
            // _CashAccountsUI_UltraFormManager_Dock_Area_Right
            // 
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1056, 27);
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.Name = "_CashAccountsUI_UltraFormManager_Dock_Area_Right";
            this._CashAccountsUI_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 425);
            // 
            // _CashAccountsUI_UltraFormManager_Dock_Area_Top
            // 
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.Name = "_CashAccountsUI_UltraFormManager_Dock_Area_Top";
            this._CashAccountsUI_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1060, 27);
            // 
            // _CashAccountsUI_UltraFormManager_Dock_Area_Bottom
            // 
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 452);
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.Name = "_CashAccountsUI_UltraFormManager_Dock_Area_Bottom";
            this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1060, 4);
            // 
            // ctrlAccounts1
            // 
            this.ctrlAccounts1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlAccounts1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlAccounts1.Location = new System.Drawing.Point(0, 0);
            this.ctrlAccounts1.Name = "ctrlAccounts1";
            this.ctrlAccounts1.Size = new System.Drawing.Size(1048, 329);
            this.ctrlAccounts1.TabIndex = 0;
            // 
            // ctrlActivityType1
            // 
            this.ctrlActivityType1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlActivityType1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlActivityType1.Location = new System.Drawing.Point(0, 0);
            this.ctrlActivityType1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ctrlActivityType1.Name = "ctrlActivityType1";
            this.ctrlActivityType1.Size = new System.Drawing.Size(1048, 329);
            this.ctrlActivityType1.TabIndex = 0;
            // 
            // ctrlActivityJournalMapping1
            // 
            this.ctrlActivityJournalMapping1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlActivityJournalMapping1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlActivityJournalMapping1.Location = new System.Drawing.Point(0, 0);
            this.ctrlActivityJournalMapping1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ctrlActivityJournalMapping1.Name = "ctrlActivityJournalMapping1";
            this.ctrlActivityJournalMapping1.Size = new System.Drawing.Size(1048, 329);
            this.ctrlActivityJournalMapping1.TabIndex = 0;
            // 
            // ctrlSubAccountType
            // 
            this.ctrlSubAccountType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSubAccountType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlSubAccountType.Location = new System.Drawing.Point(0, 0);
            this.ctrlSubAccountType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ctrlSubAccountType.Name = "ctrlSubAccountType";
            this.ctrlSubAccountType.Size = new System.Drawing.Size(1048, 329);
            this.ctrlSubAccountType.TabIndex = 0;
            // 
            // CashAccountsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 456);
            this.Controls.Add(this.CashAccountsUI_Fill_Panel);
            this.Controls.Add(this.Status);
            this.Controls.Add(this._CashAccountsUI_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._CashAccountsUI_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._CashAccountsUI_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._CashAccountsUI_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(1000, 450);
            this.Name = "CashAccountsUI";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Cash Accounts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CashAccountsUI_FormClosing);
            this.Load += new System.EventHandler(this.CashAccountsUI_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl4.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraTabPageControl5.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlActivities)).EndInit();
            this.tabCtrlActivities.ResumeLayout(false);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.CashAccountsUI_Fill_Panel.ClientArea.ResumeLayout(false);
            this.CashAccountsUI_Fill_Panel.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAddAccount;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAddSubAccount;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDeleteAccount;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDeleteSubAccount;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraButton btnAdd;
        private Infragistics.Win.Misc.UltraButton btnRestoreDefault;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAddJournalCodeToolStripMenuItem;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCtrlActivities;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        //private Infragistics.Win.UltraWinGrid.UltraGrid grdData;
        //private Infragistics.Win.UltraWinTree.UltraTree accTree;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        //private Infragistics.Win.UltraWinListView.UltraListView listViewActivities;
        //private Infragistics.Win.UltraWinGrid.UltraGrid grdActivitiesData;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        //private Infragistics.Win.UltraWinGrid.UltraGrid grdCashTransactionData;
        //private Infragistics.Win.UltraWinListView.UltraListView listViewCashTransactionType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private ctrlActivityJournalMapping ctrlActivityJournalMapping1;
        private ctrlAccounts ctrlAccounts1;
        private ctrlActivityType ctrlActivityType1;
        private ctrlSubAccountType ctrlSubAccountType;
        //private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel CashAccountsUI_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashAccountsUI_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashAccountsUI_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashAccountsUI_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashAccountsUI_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        //private System.Windows.Forms.Button btnScreenshot;
    }
}