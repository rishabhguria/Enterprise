using System.Windows.Forms;
using Prana.Global;
namespace Prana.PM.Client.UI.Forms
{
    partial class MarkPriceAndForexConversion
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
                if (_bgSaveData != null)
                {
                    _bgSaveData.Dispose();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab20 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab14 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab15 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab13 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab16 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab17 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlMarkPriceAndForexConversion = new Prana.PM.Client.UI.Controls.CtrlMarkPriceAndForexConversion();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl16 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl17 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl13 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl14 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl15 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl20 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.tabControlDailyValuation = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.btnRemove = new Infragistics.Win.Misc.UltraButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.MarkPriceAndForexConversion_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlDailyValuation)).BeginInit();
            this.tabControlDailyValuation.SuspendLayout();
            this.ultraTabSharedControlsPage1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.MarkPriceAndForexConversion_Fill_Panel.ClientArea.SuspendLayout();
            this.MarkPriceAndForexConversion_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlMarkPriceAndForexConversion);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 25);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1080, 422);
            // 
            // ctrlMarkPriceAndForexConversion
            // 
            this.ctrlMarkPriceAndForexConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlMarkPriceAndForexConversion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlMarkPriceAndForexConversion.Location = new System.Drawing.Point(0, 0);
            this.ctrlMarkPriceAndForexConversion.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ctrlMarkPriceAndForexConversion.Name = "ctrlMarkPriceAndForexConversion";
            this.ctrlMarkPriceAndForexConversion.Size = new System.Drawing.Size(1080, 422);
            this.ctrlMarkPriceAndForexConversion.TabIndex = 0;
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl16
            // 
            this.ultraTabPageControl16.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl16.Name = "ultraTabPageControl16";
            this.ultraTabPageControl16.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl17
            // 
            this.ultraTabPageControl17.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl17.Name = "ultraTabPageControl17";
            this.ultraTabPageControl17.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl9
            // 
            this.ultraTabPageControl9.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl9.Name = "ultraTabPageControl9";
            this.ultraTabPageControl9.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl12
            // 
            this.ultraTabPageControl12.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl12.Name = "ultraTabPageControl12";
            this.ultraTabPageControl12.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl13
            // 
            this.ultraTabPageControl13.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl13.Name = "ultraTabPageControl13";
            this.ultraTabPageControl13.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl14
            // 
            this.ultraTabPageControl14.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl14.Name = "ultraTabPageControl14";
            this.ultraTabPageControl14.Size = new System.Drawing.Size(1088, 430);
            // 
            // ultraTabPageControl15
            // 
            this.ultraTabPageControl15.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl15.Name = "ultraTabPageControl15";
            this.ultraTabPageControl15.Size = new System.Drawing.Size(1088, 430);
            //
            //ultraTabPageControl20
            //
            this.ultraTabPageControl20.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl20.Name = "ultraTabPageControl20";
            this.ultraTabPageControl20.Size = new System.Drawing.Size(1088, 430);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance1.FontData.SizeInPoints = 9F;
            this.btnClear.Appearance = appearance1;
            this.btnClear.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(705, 457);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.ShowOutline = false;
            this.btnClear.Size = new System.Drawing.Size(79, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance2.FontData.SizeInPoints = 9F;
            this.btnSave.Appearance = appearance2;
            this.btnSave.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(502, 457);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(79, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabMarkPriceForexConvertor
            // 
            this.tabControlDailyValuation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlDailyValuation.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl1);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl2);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl3);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl4);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl5);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl16);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl17);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl6);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl7);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl8);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl9);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl10);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl11);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl12);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl13);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl14);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl15);
            this.tabControlDailyValuation.Controls.Add(this.ultraTabPageControl20);
            this.tabControlDailyValuation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlDailyValuation.Location = new System.Drawing.Point(3, 2);
            this.tabControlDailyValuation.Name = "tabControlDailyValuation";
            this.tabControlDailyValuation.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.ctrlMarkPriceAndForexConversion});
            this.tabControlDailyValuation.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabControlDailyValuation.Size = new System.Drawing.Size(1084, 450);
            this.tabControlDailyValuation.TabIndex = 0;
            ultraTab1.Key = "tabPageMarkPrice";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "MarkPrice";
            ultraTab10.Key = "tabPagefxmarkPrices";
            ultraTab10.TabPage = this.ultraTabPageControl10;
            ultraTab10.Text = "Forward Points";
            ultraTab2.Key = "tabPageForexConversion";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Forex Conversion";
            ultraTab3.Key = "tabPageNAV";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "NAV";
            ultraTab4.Key = "tabPageDailyCash";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Daily Cash";
            ultraTab20.Key = "tabPageCollateralInterest";
            ultraTab20.TabPage = this.ultraTabPageControl20;
            ultraTab20.Text = "Collateral Interest";
            ultraTab5.Key = "tabPageDailyBeta";
            ultraTab5.TabPage = this.ultraTabPageControl5;
            ultraTab5.Text = "Daily Beta";
            ultraTab16.Key = "tabPageVWAP";
            ultraTab16.TabPage = this.ultraTabPageControl16;
            ultraTab16.Text = "Daily VWAP";
            ultraTab17.Key = "tabPageCollateralPrice";
            ultraTab17.TabPage = this.ultraTabPageControl17;
            ultraTab17.Text = "Collateral Price";
            ultraTab14.Key = "tabPageDailyVolatility";
            ultraTab14.TabPage = this.ultraTabPageControl14;
            ultraTab14.Text = "Daily Volatility";
            ultraTab15.Key = "tabPageDailyDividendYield";
            ultraTab15.TabPage = this.ultraTabPageControl15;
            ultraTab15.Text = "Daily Dividend Yield";
            ultraTab6.Key = "tabPageDailyTradingVol";
            ultraTab6.TabPage = this.ultraTabPageControl6;
            ultraTab6.Text = "Daily TradingVolume";
            ultraTab7.Key = "tabPageDailyDelta";
            ultraTab7.TabPage = this.ultraTabPageControl7;
            ultraTab7.Text = "Daily Delta";
            ultraTab8.Key = "tabPageDailyOutstandings";
            ultraTab8.TabPage = this.ultraTabPageControl8;
            ultraTab8.Text = "Daily Outstandings";
            ultraTab9.Key = "tabPagePerformanceNumbers";
            ultraTab9.TabPage = this.ultraTabPageControl9;
            ultraTab9.Text = "Performance Numbers";
            ultraTab11.Key = "tabPageStartOfMonthCapitalAccount";
            ultraTab11.TabPage = this.ultraTabPageControl11;
            ultraTab11.Text = "Start of Month Capital Account";
            ultraTab12.Key = "tabPageUserDefinedMTDPnL";
            ultraTab12.TabPage = this.ultraTabPageControl12;
            ultraTab12.Text = "User Defined MTD PnL";
            ultraTab13.Key = "tabPageDailyCreditLimit";
            ultraTab13.TabPage = this.ultraTabPageControl13;
            ultraTab13.Text = "Daily Credit Limit";
            this.tabControlDailyValuation.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab10,
            ultraTab2,
            ultraTab3,
            ultraTab4,
            ultraTab20,
            ultraTab5,
            ultraTab16,
            ultraTab14,
            ultraTab15,
            ultraTab6,
            ultraTab7,
            ultraTab8,
            ultraTab9,
            ultraTab11,
            ultraTab12,
            ultraTab13,
            ultraTab17});
            this.tabControlDailyValuation.SelectedTabChanging += new Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventHandler(this.tabControlDailyValuation_SelectedTabChanging);
            this.tabControlDailyValuation.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabControlDailyValuation_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Controls.Add(this.ctrlMarkPriceAndForexConversion);
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1080, 422);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance3.FontData.SizeInPoints = 9F;
            this.btnAdd.Appearance = appearance3;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ImageSize = new System.Drawing.Size(75, 23);
            this.btnAdd.Location = new System.Drawing.Point(418, 457);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ShowFocusRect = false;
            this.btnAdd.ShowOutline = false;
            this.btnAdd.Size = new System.Drawing.Size(77, 23);
            this.btnAdd.TabIndex = 122;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAddToCloseTrade_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance4.FontData.SizeInPoints = 9F;
            this.btnRemove.Appearance = appearance4;
            this.btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ImageSize = new System.Drawing.Size(75, 23);
            this.btnRemove.Location = new System.Drawing.Point(586, 457);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.ShowFocusRect = false;
            this.btnRemove.ShowOutline = false;
            this.btnRemove.Size = new System.Drawing.Size(74, 23);
            this.btnRemove.TabIndex = 123;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "Nirvana Help.chm";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(8, 520);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1084, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 124;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // MarkPriceAndForexConversion_Fill_Panel
            // 
            appearance5.FontData.SizeInPoints = 9F;
            this.MarkPriceAndForexConversion_Fill_Panel.Appearance = appearance5;
            // 
            // MarkPriceAndForexConversion_Fill_Panel.ClientArea
            // 
            this.MarkPriceAndForexConversion_Fill_Panel.ClientArea.Controls.Add(this.btnClear);
            this.MarkPriceAndForexConversion_Fill_Panel.ClientArea.Controls.Add(this.btnRemove);
            this.MarkPriceAndForexConversion_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.MarkPriceAndForexConversion_Fill_Panel.ClientArea.Controls.Add(this.tabControlDailyValuation);
            this.MarkPriceAndForexConversion_Fill_Panel.ClientArea.Controls.Add(this.btnAdd);
            this.MarkPriceAndForexConversion_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MarkPriceAndForexConversion_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MarkPriceAndForexConversion_Fill_Panel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MarkPriceAndForexConversion_Fill_Panel.Location = new System.Drawing.Point(8, 31);
            this.MarkPriceAndForexConversion_Fill_Panel.Name = "MarkPriceAndForexConversion_Fill_Panel";
            this.MarkPriceAndForexConversion_Fill_Panel.Size = new System.Drawing.Size(1084, 489);
            this.MarkPriceAndForexConversion_Fill_Panel.TabIndex = 125;
            // 
            // _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left
            // 
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.Name = "_MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left";
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 511);
            // 
            // _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right
            // 
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1092, 31);
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.Name = "_MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right";
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 511);
            // 
            // _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top
            // 
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.Name = "_MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top";
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1100, 31);
            // 
            // _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom
            // 
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 542);
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.Name = "_MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom";
            this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1100, 8);
            // 
            // MarkPriceAndForexConversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 550);
            this.Controls.Add(this.MarkPriceAndForexConversion_Fill_Panel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpProvider1.SetHelpKeyword(this, "EnteringMarkPrices.html");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.MinimumSize = new System.Drawing.Size(825, 497);
            this.Name = "MarkPriceAndForexConversion";
            this.helpProvider1.SetShowHelp(this, true);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Daily Valuation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MarkPriceAndForexConversion_FormClosing);
            this.Load += new System.EventHandler(this.MarkPriceAndForexConversion_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MarkPriceAndForexConversion_KeyUp);
            this.Disposed += new System.EventHandler(this.MarkPriceAndForexConversion_Disposed);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlDailyValuation)).EndInit();
            this.tabControlDailyValuation.ResumeLayout(false);
            this.ultraTabSharedControlsPage1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.MarkPriceAndForexConversion_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MarkPriceAndForexConversion_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabControlDailyValuation;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Prana.PM.Client.UI.Controls.CtrlMarkPriceAndForexConversion ctrlMarkPriceAndForexConversion;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnAdd;
        private Infragistics.Win.Misc.UltraButton btnRemove;
        //private Infragistics.Win.Misc.UltraButton btnScreenshot;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl16;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl17;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        //private System.Windows.Forms.Label lblLiveFeedEngineConnection;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl13;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel MarkPriceAndForexConversion_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl14;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl15;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl20;
    }
}