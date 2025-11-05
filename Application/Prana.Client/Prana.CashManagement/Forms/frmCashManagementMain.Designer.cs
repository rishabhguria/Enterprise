using Prana.Global;
namespace Prana.CashManagement
{
    partial class frmCashManagementMain
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
                if (_dailyCalculationControl != null)
                {
                    _dailyCalculationControl.Dispose();
                }
                if (_cashJournalControl != null)
                {
                    _cashJournalControl.Dispose();
                }
                if (_ctrlAccountsChart != null)
                {
                    _ctrlAccountsChart.Dispose();
                }
                if (_ctrlActivity != null)
                {
                    _ctrlActivity.Dispose();
                }
                if (_cashControl != null)
                {
                    _cashControl.Dispose();
                }
                if (_ctrlJournalException != null)
                {
                    _ctrlJournalException.Dispose();
                }
                if (_ctrlActivityException != null)
                {
                    _ctrlActivityException.Dispose();
                }
                if (_cashTransaction != null)
                {
                    _cashTransaction.Dispose();
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AccountSetup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Snapshot");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AccountSetup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Snapshot");
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.frmCashManagementMain_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.tbCtrlMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.frmCashManagementMain_Fill_Panel.ClientArea.SuspendLayout();
            this.frmCashManagementMain_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCtrlMain)).BeginInit();
            this.tbCtrlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(1314, 540);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(1322, 548);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1322, 548);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1322, 548);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1322, 548);
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(1322, 548);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(1322, 548);
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(1314, 540);
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(1314, 540);
            // 
            // frmCashManagementMain_Fill_Panel
            // 
            // 
            // frmCashManagementMain_Fill_Panel.ClientArea
            // 
            this.frmCashManagementMain_Fill_Panel.ClientArea.Controls.Add(this.tbCtrlMain);
            this.frmCashManagementMain_Fill_Panel.ClientArea.Controls.Add(this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left);
            this.frmCashManagementMain_Fill_Panel.ClientArea.Controls.Add(this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right);
            this.frmCashManagementMain_Fill_Panel.ClientArea.Controls.Add(this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top);
            this.frmCashManagementMain_Fill_Panel.ClientArea.Controls.Add(this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom);
            this.frmCashManagementMain_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.frmCashManagementMain_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frmCashManagementMain_Fill_Panel.Location = new System.Drawing.Point(8, 31);
            this.frmCashManagementMain_Fill_Panel.Name = "frmCashManagementMain_Fill_Panel";
            this.frmCashManagementMain_Fill_Panel.Size = new System.Drawing.Size(1316, 586);
            this.frmCashManagementMain_Fill_Panel.TabIndex = 0;
            // 
            // tbCtrlMain
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tbCtrlMain.ActiveTabAppearance = appearance1;
            appearance2.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance2.FontData.SizeInPoints = 9F;
            this.tbCtrlMain.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance3.FontData.SizeInPoints = 9F;
            this.tbCtrlMain.ClientAreaAppearance = appearance3;
            this.tbCtrlMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl1);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl2);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl4);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl3);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl5);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl6);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl7);
            this.tbCtrlMain.Controls.Add(this.ultraTabPageControl8);
            this.tbCtrlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtrlMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCtrlMain.Location = new System.Drawing.Point(0, 25);
            this.tbCtrlMain.Name = "tbCtrlMain";
            appearance4.FontData.SizeInPoints = 9F;
            this.tbCtrlMain.SelectedTabAppearance = appearance4;
            this.tbCtrlMain.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.tbCtrlMain.Size = new System.Drawing.Size(1316, 561);
            this.tbCtrlMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbCtrlMain.TabIndex = 0;
            ultraTab7.Key = "tbActivity";
            ultraTab7.TabPage = this.ultraTabPageControl7;
            ultraTab7.Text = "Activity";
            ultraTab4.Key = "tbCashJrnl";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Cash Journal";
            ultraTab1.Key = "tbDailyCalc";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Daily Calculations";
            ultraTab2.Key = "tbDayEndCash";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Day End Cash";
            ultraTab3.Key = "tbAccountsChart";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Chart of CashAccounts";
            ultraTab6.Key = "tbActivityExceptions";
            ultraTab6.TabPage = this.ultraTabPageControl6;
            ultraTab6.Text = "Activity Exceptions";
            ultraTab5.Key = "tbJournalExceptions";
            ultraTab5.TabPage = this.ultraTabPageControl5;
            ultraTab5.Text = "Journal Exceptions";
            ultraTab8.Key = "tbCashTransaction";
            ultraTab8.TabPage = this.ultraTabPageControl8;
            ultraTab8.Text = "Cash Transactions";
            this.tbCtrlMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab7,
            ultraTab4,
            ultraTab1,
            ultraTab2,
            ultraTab3,
            ultraTab6,
            ultraTab5,
            ultraTab8});
            this.tbCtrlMain.ActiveTabChanging += new Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventHandler(this.tbCtrlMain_ActiveTabChanging);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(833, 554);
            // 
            // _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left
            // 
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.Name = "_frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left";
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 561);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            appearance5.FontData.SizeInPoints = 9F;
            this.ultraToolbarsManager1.Appearance = appearance5;
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.frmCashManagementMain_Fill_Panel;
            this.ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            this.ultraToolbarsManager1.ImageTransparentColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ultraToolbarsManager1.IsGlassSupported = false;
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.SaveSettings = true;
            this.ultraToolbarsManager1.SettingsKey = "frmCashManagementMain.ultraToolbarsManager1";
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager1.ShowShortcutsInToolTips = true;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.FloatingSize = new System.Drawing.Size(100, 42);
            ultraToolbar1.IsMainMenuBar = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            buttonTool5});
            ultraToolbar1.Text = "Preferences";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            buttonTool4.SharedPropsInternal.Caption = "Account Setup";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool1.SharedPropsInternal.Caption = "Snapshot";
            buttonTool1.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            buttonTool1});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right
            // 
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1316, 25);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.Name = "_frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right";
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 561);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top
            // 
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.Name = "_frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top";
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1316, 25);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom
            // 
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 586);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.Name = "_frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom";
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1316, 0);
            this._frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _frmCashManagementMain_UltraFormManager_Dock_Area_Left
            // 
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.Name = "_frmCashManagementMain_UltraFormManager_Dock_Area_Left";
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 586);
            // 
            // _frmCashManagementMain_UltraFormManager_Dock_Area_Right
            // 
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1324, 31);
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.Name = "_frmCashManagementMain_UltraFormManager_Dock_Area_Right";
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 586);
            // 
            // _frmCashManagementMain_UltraFormManager_Dock_Area_Top
            // 
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.Name = "_frmCashManagementMain_UltraFormManager_Dock_Area_Top";
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1332, 31);
            // 
            // _frmCashManagementMain_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 617);
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.Name = "_frmCashManagementMain_UltraFormManager_Dock_Area_Bottom";
            this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1332, 8);
            // 
            // frmCashManagementMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1332, 625);
            this.Controls.Add(this.frmCashManagementMain_Fill_Panel);
            this.Controls.Add(this._frmCashManagementMain_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmCashManagementMain_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmCashManagementMain_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmCashManagementMain_UltraFormManager_Dock_Area_Bottom);
            this.Name = "frmCashManagementMain";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "General Ledger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCashManagementMain_FormClosing);
            this.Load += frmCashManagementMain_Load;
            this.Resize += new System.EventHandler(this.frmCashManagementMain_Resize);
            this.frmCashManagementMain_Fill_Panel.ClientArea.ResumeLayout(false);
            this.frmCashManagementMain_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbCtrlMain)).EndInit();
            this.tbCtrlMain.ResumeLayout(false);
            ((System.Configuration.IPersistComponentSettings)(this.ultraToolbarsManager1)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel frmCashManagementMain_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmCashManagementMain_Fill_Panel_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbCtrlMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCashManagementMain_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCashManagementMain_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCashManagementMain_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCashManagementMain_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;

    }
}