using Infragistics.Win;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;

namespace Prana.TradingTicket.Forms
{
    partial class QuickTradingTicket
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
                if(validationTokenSource != null)
                {
                    validationTokenSource.Dispose();
                    validationTokenSource = null;
                }
                if (_secMasterSyncService != null)
                {
                    _secMasterSyncService.Dispose();
                }
                if (qttPresenter != null)
                {
                    qttPresenter.Dispose();
                }
                if (_algoControlPopUp != null)
                {
                    _algoControlPopUp.Dispose();
                    _algoControlPopUp = null;
                }
                if (components != null)
                {
                    components.Dispose();
                }
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate -= QuickTradingTicket_CounterPartyStatusUpdate;
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
            Infragistics.Win.Appearance appearanceAlloc = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearanceBroker = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearanceVenue = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearanceTIF = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearanceOrderType = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearanceCustomBtn = new Infragistics.Win.Appearance();
            this.components = new System.ComponentModel.Container();
            this.tblPnlDetails = new System.Windows.Forms.TableLayoutPanel();
            this.pranaSymbolCtrl = new Prana.Utilities.UI.UIUtilities.PranaSymbolCtrl();
            this.pnlSymbol = new Infragistics.Win.Misc.UltraPanel();
            this.btnSymbolLookup = new System.Windows.Forms.Button();
            this.lblSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblSplitter1 = new System.Windows.Forms.Label();
            this.lblSplitter2 = new System.Windows.Forms.Label();
            this.qttL1Strip = new Prana.LiveFeed.UI.Controls.QTTL1Strip(); 
            this.tblPnlMainControls = new System.Windows.Forms.TableLayoutPanel();
            this.tblBottomStrip = new System.Windows.Forms.TableLayoutPanel();
            this.pnlPrice = new Infragistics.Win.Misc.UltraPanel();
            this.pnlPriceButtons = new Infragistics.Win.Misc.UltraPanel();
            this.btnAsk = new Infragistics.Win.Misc.UltraButton();
            this.btnMid = new Infragistics.Win.Misc.UltraButton();
            this.btnBid = new Infragistics.Win.Misc.UltraButton();
            this.nmrcPrice = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.lblPrice = new Infragistics.Win.Misc.UltraLabel();
            this.pnlStop = new Infragistics.Win.Misc.UltraPanel();
            this.nmrcStop = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.lblStop = new Infragistics.Win.Misc.UltraLabel();
            this.pnlOrderType = new Infragistics.Win.Misc.UltraPanel();
            this.cmbOrderType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblOrderType = new Infragistics.Win.Misc.UltraLabel();
            this.pnlTIF = new Infragistics.Win.Misc.UltraPanel();
            this.cmbTIF = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblTIF = new Infragistics.Win.Misc.UltraLabel();
            this.lblNotionLoc = new System.Windows.Forms.Label();
            this.lblNotionLocCalc = new System.Windows.Forms.Label();
            this.lblNotionBase = new System.Windows.Forms.Label();
            this.lblNotionBaseCalc = new System.Windows.Forms.Label();
            this.lblSplitter3 = new System.Windows.Forms.Label();
            this.lblRoundLot = new System.Windows.Forms.Label();
            this.lblRoundLotValue = new System.Windows.Forms.Label();
            this.pnlRoundLot = new Infragistics.Win.Misc.UltraPanel();
            this.toggleSwitchRoundLot = new Prana.Utilities.UI.ToggleSwitch();
            this.pnlVenue = new Infragistics.Win.Misc.UltraPanel();
            this.btnAlgo = new Infragistics.Win.Misc.UltraButton();
            this.lblVenue = new Infragistics.Win.Misc.UltraLabel();
            this.cmbVenue = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.pnlBroker = new Infragistics.Win.Misc.UltraPanel();
            this.btnBroker = new Infragistics.Win.Misc.UltraButton();
            this.lblBroker = new Infragistics.Win.Misc.UltraLabel();
            this.cmbBroker = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.pnlAllocation = new Infragistics.Win.Misc.UltraPanel();
            this.btnCustomAllocation = new Infragistics.Win.Misc.UltraButton();
            this.lblAllocation = new Infragistics.Win.Misc.UltraLabel();
            this.cmbAllocation = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.pnlQuantity = new Infragistics.Win.Misc.UltraPanel();
            this.btnHotQty3 = new Infragistics.Win.Misc.UltraButton();
            this.btnHotQty2 = new Infragistics.Win.Misc.UltraButton();
            this.btnPlusMinus = new Infragistics.Win.Misc.UltraButton();
            this.btnHotQty1 = new Infragistics.Win.Misc.UltraButton();
            this.btnPosition = new Infragistics.Win.Misc.UltraButton();
            this.nmrcQuantity = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.lblQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.lblErrorMsg = new Infragistics.Win.Misc.UltraLabel();
            this.tblPnlButtonControls = new System.Windows.Forms.TableLayoutPanel();
            this.btnBuy = new Infragistics.Win.Misc.UltraButton();
            this.btnSell = new Infragistics.Win.Misc.UltraButton();
            this.btnSellShort = new Infragistics.Win.Misc.UltraButton();
            this.btnBuyToCover = new Infragistics.Win.Misc.UltraButton();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.QuickTradingTicket_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.tblPnlDetails.SuspendLayout();
            this.tblBottomStrip.SuspendLayout();
            this.pnlSymbol.ClientArea.SuspendLayout();
            this.pnlSymbol.SuspendLayout();
            this.tblPnlMainControls.SuspendLayout();
            this.pnlPrice.ClientArea.SuspendLayout();
            this.pnlPrice.SuspendLayout();
            this.pnlRoundLot.SuspendLayout();
            this.pnlRoundLot.ClientArea.SuspendLayout();
            this.pnlPriceButtons.ClientArea.SuspendLayout();
            this.pnlPriceButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcPrice)).BeginInit();
            this.pnlStop.ClientArea.SuspendLayout();
            this.pnlStop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcStop)).BeginInit();
            this.pnlOrderType.ClientArea.SuspendLayout();
            this.pnlOrderType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).BeginInit();
            this.pnlTIF.ClientArea.SuspendLayout();
            this.pnlTIF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).BeginInit();
            this.pnlVenue.ClientArea.SuspendLayout();
            this.pnlVenue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            this.pnlBroker.ClientArea.SuspendLayout();
            this.pnlBroker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBroker)).BeginInit();
            this.pnlAllocation.ClientArea.SuspendLayout();
            this.pnlAllocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocation)).BeginInit();
            this.pnlQuantity.ClientArea.SuspendLayout();
            this.pnlQuantity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcQuantity)).BeginInit();
            this.tblPnlButtonControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.QuickTradingTicket_Fill_Panel.ClientArea.SuspendLayout();
            this.QuickTradingTicket_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tblPnlDetails
            // 
            this.tblPnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblPnlDetails.ColumnCount = 3;
            this.tblPnlDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21F));
            this.tblPnlDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tblPnlDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78F));
            this.tblPnlDetails.Controls.Add(this.pranaSymbolCtrl, 0, 1);
            this.tblPnlDetails.Controls.Add(this.pnlSymbol, 0, 0);
            this.tblPnlDetails.Controls.Add(this.lblSplitter1, 1, 0);
            this.tblPnlDetails.Controls.Add(this.qttL1Strip, 2, 0);
            this.tblPnlDetails.Location = new System.Drawing.Point(2, 3);
            this.tblPnlDetails.Name = "tblPnlDetails";
            this.tblPnlDetails.RowCount = 2;
            this.tblPnlDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlDetails.Size = new System.Drawing.Size(682, 53);
            this.tblPnlDetails.TabIndex = 0;
            // 
            // pranaSymbolCtrl
            // 
            this.pranaSymbolCtrl.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.pranaSymbolCtrl.Location = new System.Drawing.Point(3, 28);
            this.pranaSymbolCtrl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pranaSymbolCtrl.MaxLength = 32767;
            this.pranaSymbolCtrl.Name = "pranaSymbolCtrl";
            this.pranaSymbolCtrl.PrevSymbolEntered = "";
            this.pranaSymbolCtrl.Size = new System.Drawing.Size(130, 21);
            this.pranaSymbolCtrl.TabIndex = 0;
            // 
            // pnlSymbol
            // 
            // 
            // pnlSymbol.ClientArea
            // 
            this.pnlSymbol.ClientArea.Controls.Add(this.btnSymbolLookup);
            this.pnlSymbol.ClientArea.Controls.Add(this.lblSymbol);
            this.pnlSymbol.Location = new System.Drawing.Point(3, 3);
            this.pnlSymbol.Name = "pnlSymbol";
            this.pnlSymbol.Size = new System.Drawing.Size(130, 23);
            this.pnlSymbol.TabIndex = 1;
            // 
            // btnSymbolLookup
            // 
            this.btnSymbolLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.FlatAppearance.BorderSize = 0;
            this.btnSymbolLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.ForeColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.UseVisualStyleBackColor = false;
            this.btnSymbolLookup.Location = new System.Drawing.Point(110, 1);
            this.btnSymbolLookup.Name = "btnSymbolLookup";
            this.btnSymbolLookup.Size = new System.Drawing.Size(20, 20);
            this.btnSymbolLookup.TabIndex = 0;
            this.btnSymbolLookup.BackgroundImage = global::Prana.TradingTicket.Properties.Resources.SecurityMaster;
            this.btnSymbolLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSymbolLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSymbolLookup.Click += BtnSymbolLookup_Click;
            this.btnSymbolLookup.MouseEnter += BtnSymbolLookup_MouseEnter;
            this.btnSymbolLookup.MouseLeave += BtnSymbolLookup_MouseLeave;
            this.btnSymbolLookup.GotFocus += BtnSymbolLookup_MouseEnter;
            this.btnSymbolLookup.LostFocus += BtnSymbolLookup_MouseLeave;
            // 
            // lblSymbol
            // 
            this.lblSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(3, 0);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(61, 23);
            this.lblSymbol.Text = "Symbol";
            // 
            // lblSplitter1
            // 
            this.lblSplitter1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSplitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSplitter1.Location = new System.Drawing.Point(146, 1);
            this.lblSplitter1.Name = "lblSplitter1";
            this.tblPnlDetails.SetRowSpan(this.lblSplitter1, 2);
            this.lblSplitter1.Size = new System.Drawing.Size(1, 50);
            this.lblSplitter1.TabStop = false;
            // 
            // qttL1Strip
            // 
            this.qttL1Strip.AutoSize = true;
            this.qttL1Strip.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.qttL1Strip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.qttL1Strip.Location = new System.Drawing.Point(151, 3);
            this.qttL1Strip.Name = "qttL1Strip";
            this.tblPnlDetails.SetRowSpan(this.qttL1Strip, 2);
            this.qttL1Strip.Size = new System.Drawing.Size(528, 47);
            this.qttL1Strip.Symbol = "";
            this.qttL1Strip.TabStop = false;
            // 
            // tblPnlMainControls
            // 
            this.tblPnlMainControls.ColumnCount = 4;
            this.tblPnlMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlMainControls.Controls.Add(this.pnlPrice, 3, 1);
            this.tblPnlMainControls.Controls.Add(this.pnlStop, 2, 1);
            this.tblPnlMainControls.Controls.Add(this.pnlOrderType, 1, 1);
            this.tblPnlMainControls.Controls.Add(this.pnlTIF, 0, 1);
            this.tblPnlMainControls.Controls.Add(this.pnlVenue, 3, 0);
            this.tblPnlMainControls.Controls.Add(this.pnlBroker, 2, 0);
            this.tblPnlMainControls.Controls.Add(this.pnlAllocation, 1, 0);
            this.tblPnlMainControls.Controls.Add(this.pnlQuantity, 0, 0); 
            this.tblPnlMainControls.Controls.Add(this.lblErrorMsg, 0, 3);
            this.tblPnlMainControls.Controls.Add(this.tblBottomStrip, 0, 2);
            this.tblPnlMainControls.Location = new System.Drawing.Point(-2, 57);
            this.tblPnlMainControls.Name = "tblPnlMainControls";
            this.tblPnlMainControls.RowCount = 3;
            this.tblPnlMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.tblPnlMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26F));
            this.tblPnlMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tblPnlMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tblPnlMainControls.Size = new System.Drawing.Size(585, 185);
            this.tblPnlMainControls.TabIndex = 1;
            // 
            // tblBottomStrip
            // 
            this.tblPnlMainControls.SetColumnSpan(this.tblBottomStrip, 4);
            this.tblBottomStrip.Margin = new System.Windows.Forms.Padding(0);
            this.tblBottomStrip.ColumnCount = 7;
            this.tblBottomStrip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tblBottomStrip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tblBottomStrip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tblBottomStrip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tblBottomStrip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tblBottomStrip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tblBottomStrip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblBottomStrip.Controls.Add(this.lblSplitter2, 2, 0);
            this.tblBottomStrip.Controls.Add(this.lblNotionBase, 1, 0);
            this.tblBottomStrip.Controls.Add(this.lblNotionBaseCalc, 1, 1);
            this.tblBottomStrip.Controls.Add(this.lblNotionLoc, 3, 0);
            this.tblBottomStrip.Controls.Add(this.lblNotionLocCalc, 3, 1);
            this.tblBottomStrip.Controls.Add(this.lblSplitter3, 4, 0);
            this.tblBottomStrip.Controls.Add(this.lblRoundLotValue, 5, 1);
            this.tblBottomStrip.Controls.Add(this.pnlRoundLot, 5, 0);
            this.tblBottomStrip.Controls.Add(this.pnlPriceButtons, 7, 0);
            this.tblBottomStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBottomStrip.Name = "tblBottomStrip";
            this.tblBottomStrip.RowCount = 2;
            this.tblBottomStrip.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblBottomStrip.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));

            // 
            // lblSplitter2
            // 
            this.lblSplitter2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblSplitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSplitter2.Location = new System.Drawing.Point(1, 6);
            this.lblSplitter2.Name = "lblSplitter2";
            this.tblBottomStrip.SetRowSpan(this.lblSplitter2, 2);
            this.lblSplitter2.Size = new System.Drawing.Size(1, 28);
            this.lblSplitter2.TabStop = false;
            // 
            // lblSplitter3
            // 
            this.lblSplitter3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblSplitter3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSplitter3.Location = new System.Drawing.Point(0, 0);
            this.tblBottomStrip.SetRowSpan(this.lblSplitter3, 2);
            this.lblSplitter3.Name = "lblSplitter3";
            this.lblSplitter3.Size = new System.Drawing.Size(1, 28);
            this.lblSplitter3.TabStop = false;
            // 
            // lblRoundLot
            // 
            this.lblRoundLot.TextAlign = ContentAlignment.MiddleCenter;
            this.lblRoundLot.Margin = new System.Windows.Forms.Padding(0);
            this.lblRoundLot.Name = "lblRoundLot";
            this.lblRoundLot.AutoSize = true;
            this.lblRoundLot.TabStop = false;
            this.lblRoundLot.Text = "Round Lot";
            lblRoundLot.Location = new System.Drawing.Point(24, 2);
            // 
            // lblRoundLotValue
            // 
            this.lblRoundLotValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRoundLotValue.AutoSize = true;
            this.lblRoundLotValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoundLotValue.Name = "lblRoundLotValue";
            this.lblRoundLotValue.TabStop = false;
            this.lblRoundLotValue.Text = "";
            this.lblRoundLotValue.MouseHover += LblRoundLotValue_MouseHover;
            // 
            // toggleSwitchRoundLot
            // 
            this.toggleSwitchRoundLot.AllowCheckChangedDuringLoad = false;
            this.toggleSwitchRoundLot.Location = new System.Drawing.Point(81,3);
            this.toggleSwitchRoundLot.Name = "toggleSwitchRoundLot";
            this.toggleSwitchRoundLot.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 6.95F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleSwitchRoundLot.OffText = "Off";
            this.toggleSwitchRoundLot.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 6.95F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleSwitchRoundLot.OnText = "On";
            this.toggleSwitchRoundLot.Size = new System.Drawing.Size(35, 14);
            this.toggleSwitchRoundLot.Style = Prana.Utilities.UI.ToggleSwitch.ToggleSwitchStyle.IOS5;
            this.toggleSwitchRoundLot.Click += new System.EventHandler(this.toggleSwitchRoundLot_Click);
            // 
            // pnlRoundLot
            // 
            this.pnlRoundLot.Anchor = System.Windows.Forms.AnchorStyles.Top;
            // 
            // pnlRoundLot.ClientArea
            // 
            this.pnlRoundLot.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRoundLot.ClientArea.Controls.Add(this.lblRoundLot);
            this.pnlRoundLot.ClientArea.Controls.Add(this.toggleSwitchRoundLot);
            this.pnlRoundLot.Location = new System.Drawing.Point(0, 0);
            this.pnlRoundLot.Name = "pnlPriceButtons";
            this.pnlRoundLot.Size = new System.Drawing.Size(139, 20);
            //    
            // pnlPriceButtons
            // 
            this.pnlPriceButtons.Anchor = System.Windows.Forms.AnchorStyles.Top;
            // 
            // pnlPriceButtons.ClientArea
            // 
            this.pnlPriceButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPriceButtons.ClientArea.Controls.Add(this.btnAsk);
            this.pnlPriceButtons.ClientArea.Controls.Add(this.btnMid);
            this.pnlPriceButtons.ClientArea.Controls.Add(this.btnBid);
            this.pnlPriceButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlPriceButtons.Name = "pnlPriceButtons";
            this.pnlPriceButtons.Size = new System.Drawing.Size(139, 20);
            // 
            // pnlPrice
            // 
            this.pnlPrice.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            // 
            // pnlPrice.ClientArea
            // 
            this.pnlPrice.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPrice.ClientArea.Controls.Add(this.nmrcPrice);
            this.pnlPrice.ClientArea.Controls.Add(this.lblPrice);
            this.pnlPrice.Location = new System.Drawing.Point(442, 0);
            this.pnlPrice.Name = "pnlPrice";
            this.pnlPrice.Size = new System.Drawing.Size(139, 50);
            this.pnlPrice.TabIndex = 7;
            // 
            // btnAsk
            // 
            this.btnAsk.Margin = new System.Windows.Forms.Padding(0);
            this.btnAsk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAsk.Location = new System.Drawing.Point(92, 0);
            this.btnAsk.Name = "btnAsk";
            this.btnAsk.Size = new System.Drawing.Size(44, 19);
            this.btnAsk.TabStop = false;
            this.btnAsk.Text = "ASK";
            this.btnAsk.Click += BtnAsk_Click;
            // 
            // btnMid
            // 
            this.btnMid.Margin = new System.Windows.Forms.Padding(0);
            this.btnMid.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMid.Location = new System.Drawing.Point(47, 0);
            this.btnMid.Name = "btnMid";
            this.btnMid.Size = new System.Drawing.Size(45, 19);
            this.btnMid.TabStop = false;
            this.btnMid.Text = "MID";
            this.btnMid.Click += BtnMid_Click;
            // 
            // btnBid
            // 
            this.btnBid.Margin = new System.Windows.Forms.Padding(0);
            this.btnBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBid.Location = new System.Drawing.Point(3, 0);
            this.btnBid.Name = "btnBid";
            this.btnBid.Size = new System.Drawing.Size(44, 19);
            this.btnBid.TabStop = false;
            this.btnBid.Text = "BID";
            this.btnBid.Click += BtnBid_Click;
            // 
            // nmrcPrice
            // 
            this.nmrcPrice.Increment = 0.01m;
            this.nmrcPrice.Location = new System.Drawing.Point(3, 26);
            this.nmrcPrice.Maximum = 999999999;
            this.nmrcPrice.Minimum = 0;
            this.nmrcPrice.Name = "nmrcPrice";
            this.nmrcPrice.Size = new System.Drawing.Size(133, 20);
            this.nmrcPrice.AutoSelect = true;
            this.nmrcPrice.TabIndex = 0;
            this.nmrcPrice.Value = 0m;
            this.nmrcPrice.RemoveThousandSeperatorOnEdit = true;
            this.nmrcPrice.AllowThousandSeperator = true;
            this.nmrcPrice.ShowCommaSeperatorOnEditing = true;
            this.nmrcPrice.ValueChanged += new System.EventHandler(this.nmrcPrice_ValueChanged);
            // 
            // lblPrice
            // 
            this.lblPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.Location = new System.Drawing.Point(3, 8);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(100, 23);
            this.lblPrice.TabStop = false;
            this.lblPrice.Text = "Price";
            // 
            // pnlStop
            // 
            this.pnlStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            // 
            // pnlStop.ClientArea
            // 
            this.pnlStop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStop.ClientArea.Controls.Add(this.nmrcStop);
            this.pnlStop.ClientArea.Controls.Add(this.lblStop);
            this.pnlStop.Location = new System.Drawing.Point(296, 0);
            this.pnlStop.Name = "pnlStop";
            this.pnlStop.Size = new System.Drawing.Size(138, 50);
            this.pnlStop.TabIndex = 6;
            // 
            // nmrcStop
            // 
            this.nmrcStop.Increment = 0.01m;
            this.nmrcStop.Location = new System.Drawing.Point(3, 26);
            this.nmrcStop.Maximum = 999999999;
            this.nmrcStop.Minimum = 0;
            this.nmrcStop.Name = "nmrcStop";
            this.nmrcStop.Size = new System.Drawing.Size(132, 20);
            this.nmrcStop.TabIndex = 0;
            this.nmrcStop.AutoSelect = true;
            this.nmrcStop.RemoveThousandSeperatorOnEdit = true;
            this.nmrcStop.AllowThousandSeperator = true;
            this.nmrcStop.ShowCommaSeperatorOnEditing = true;
            this.nmrcStop.Value = 0m;
            // 
            // lblStop
            // 
            this.lblStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStop.Location = new System.Drawing.Point(3, 8);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(100, 23);
            this.lblStop.TabStop = false;
            this.lblStop.Text = "Stop";
            // 
            // pnlOrderType
            // 
            this.pnlOrderType.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            // 
            // pnlOrderType.ClientArea
            // 
            this.pnlOrderType.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOrderType.ClientArea.Controls.Add(this.cmbOrderType);
            this.pnlOrderType.ClientArea.Controls.Add(this.lblOrderType);
            this.pnlOrderType.Location = new System.Drawing.Point(151, 0);
            this.pnlOrderType.Name = "pnlOrderType";
            this.pnlOrderType.Size = new System.Drawing.Size(135, 50);
            this.pnlOrderType.TabIndex = 5;
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbOrderType.DropDownListWidth = -1;
            this.cmbOrderType.NullText = "Select OrderType";
            appearanceOrderType.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbOrderType.NullTextAppearance = appearanceOrderType;
            this.cmbOrderType.Location = new System.Drawing.Point(5, 26);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(127, 20);
            this.cmbOrderType.TabIndex = 0;
            this.cmbOrderType.ValueChanged += CmbOrderType_ValueChanged;
            this.cmbOrderType.Leave += cmb_Leave;
            // 
            // lblOrderType
            // 
            this.lblOrderType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderType.Location = new System.Drawing.Point(3, 8);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(100, 23);
            this.lblOrderType.TabStop = false;
            this.lblOrderType.Text = "Order Type";
            // 
            // pnlTIF
            // 
            this.pnlTIF.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            // 
            // pnlTIF.ClientArea
            // 
            this.pnlTIF.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTIF.ClientArea.Controls.Add(this.cmbTIF);
            this.pnlTIF.ClientArea.Controls.Add(this.lblTIF);
            this.pnlTIF.Location = new System.Drawing.Point(3, 0);
            this.pnlTIF.Name = "pnlTIF";
            this.pnlTIF.Size = new System.Drawing.Size(140, 50);
            this.pnlTIF.TabIndex = 4;
            // 
            // cmbTIF
            // 
            this.cmbTIF.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTIF.DropDownListWidth = -1;
            this.cmbTIF.NullText = "Select TIF";
            appearanceTIF.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTIF.NullTextAppearance = appearanceTIF;
            this.cmbTIF.Location = new System.Drawing.Point(3, 26);
            this.cmbTIF.Name = "cmbTIF";
            this.cmbTIF.Size = new System.Drawing.Size(134, 20);
            this.cmbTIF.TabIndex = 0;
            this.cmbTIF.Leave += cmb_Leave;
            // 
            // lblTIF
            // 
            this.lblTIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTIF.Location = new System.Drawing.Point(3, 8);
            this.lblTIF.Name = "lblTIF";
            this.lblTIF.Size = new System.Drawing.Size(100, 23);
            this.lblTIF.TabStop = false;
            this.lblTIF.Text = "TIF";
            // 
            // lblNotionBase
            // 
            this.lblNotionBase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNotionBase.Margin = new System.Windows.Forms.Padding(0);
            this.lblNotionBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotionBase.Name = "lblNotionBase";
            this.lblNotionBase.Size = new System.Drawing.Size(100, 13);
            this.lblNotionBase.TabStop = false;
            this.lblNotionBase.Text = "Notional Base";
            // 
            // lblNotionBaseCalc
            // 
            this.lblNotionBaseCalc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNotionBaseCalc.Margin = new System.Windows.Forms.Padding(0);
            this.lblNotionBaseCalc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotionBaseCalc.Name = "lblNotionBaseCalc";
            this.lblNotionBaseCalc.Size = new System.Drawing.Size(100, 13);
            this.lblNotionBaseCalc.TabStop = false;
            this.lblNotionBaseCalc.Text = "";
            this.lblNotionBaseCalc.MouseHover += LblNotionBaseCalc_MouseHover;
            // 
            // lblNotionLoc
            // 
            this.lblNotionLoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNotionLoc.Margin = new System.Windows.Forms.Padding(0);
            this.lblNotionLoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotionLoc.Name = "lblNotionLoc";
            this.lblNotionLoc.Size = new System.Drawing.Size(100, 13);
            this.lblNotionLoc.TabStop = false;
            this.lblNotionLoc.Text = "Notional Local";
            // 
            // lblNotionLocCalc
            // 
            this.lblNotionLocCalc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNotionLocCalc.Margin = new System.Windows.Forms.Padding(0);
            this.lblNotionLocCalc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotionLocCalc.Name = "lblNotionLocCalc";
            this.lblNotionLocCalc.Size = new System.Drawing.Size(100, 13);
            this.lblNotionLocCalc.TabStop = false;
            this.lblNotionLocCalc.Text = "";
            this.lblNotionLocCalc.MouseHover += LblNotionLocCalc_MouseHover;
            // 
            // pnlVenue
            // 
            // 
            // pnlVenue.ClientArea
            // 
            this.pnlVenue.ClientArea.Controls.Add(this.btnAlgo);
            this.pnlVenue.ClientArea.Controls.Add(this.lblVenue);
            this.pnlVenue.ClientArea.Controls.Add(this.cmbVenue);
            this.pnlVenue.Location = new System.Drawing.Point(441, 3);
            this.pnlVenue.Name = "pnlVenue";
            this.pnlVenue.Size = new System.Drawing.Size(139, 72);
            this.pnlVenue.TabIndex = 3;
            // 
            // btnAlgo
            // 
            this.btnAlgo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlgo.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlgo.Location = new System.Drawing.Point(74, 6);
            this.btnAlgo.Name = "btnAlgo";
            this.btnAlgo.Size = new System.Drawing.Size(61, 19);
            this.btnAlgo.Text = "NONE";
            this.btnAlgo.TabStop = false;
            this.btnAlgo.Visible = false;
            this.btnAlgo.Click += BtnAlgo_Click;
            // 
            // lblVenue
            // 
            this.lblVenue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVenue.Location = new System.Drawing.Point(8, 6);
            this.lblVenue.Name = "lblVenue";
            this.lblVenue.Size = new System.Drawing.Size(61, 18);
            this.lblVenue.TabStop = false;
            this.lblVenue.Text = "Venue";
            // 
            // cmbVenue
            // 
            this.cmbVenue.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbVenue.DropDownListWidth = -1;
            this.cmbVenue.NullText = "Select Venue";
            appearanceVenue.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbVenue.NullTextAppearance = appearanceVenue;
            this.cmbVenue.Location = new System.Drawing.Point(3, 28);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(133, 20);
            this.cmbVenue.TabIndex = 0;
            this.cmbVenue.ValueChanged += CmbVenue_ValueChanged;
            this.cmbVenue.Leave += cmb_Leave;
            // 
            // pnlBroker
            // 
            // 
            // pnlBroker.ClientArea
            // 
            this.pnlBroker.ClientArea.Controls.Add(this.btnBroker);
            this.pnlBroker.ClientArea.Controls.Add(this.lblBroker);
            this.pnlBroker.ClientArea.Controls.Add(this.cmbBroker);
            this.pnlBroker.Location = new System.Drawing.Point(295, 3);
            this.pnlBroker.Name = "pnlBroker";
            this.pnlBroker.Size = new System.Drawing.Size(138, 72);
            this.pnlBroker.TabIndex = 2;
            // 
            // btnBroker
            // 
            this.btnBroker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBroker.Margin = new System.Windows.Forms.Padding(0);
            this.btnBroker.Appearance.BorderAlpha = Alpha.Transparent;
            this.btnBroker.Enabled = false;
            this.btnBroker.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBroker.Location = new System.Drawing.Point(116, 5);
            this.btnBroker.Name = "btnBroker";
            this.btnBroker.Size = new System.Drawing.Size(19, 19);
            this.btnBroker.TabStop = false;
            // 
            // lblBroker
            // 
            this.lblBroker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBroker.Location = new System.Drawing.Point(4, 5);
            this.lblBroker.Name = "lblBroker";
            this.lblBroker.Size = new System.Drawing.Size(61, 18);
            this.lblBroker.TabStop = false;
            this.lblBroker.Text = "Broker";
            // 
            // cmbBroker
            // 
            this.cmbBroker.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbBroker.DropDownListWidth = -1;
            this.cmbBroker.NullText = "Select Broker";
            appearanceBroker.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbBroker.NullTextAppearance = appearanceBroker;
            this.cmbBroker.Location = new System.Drawing.Point(3, 28);
            this.cmbBroker.Name = "cmbBroker";
            this.cmbBroker.Size = new System.Drawing.Size(132, 20);
            this.cmbBroker.TabIndex = 0;
            this.cmbBroker.ValueChanged += CmbBroker_ValueChanged;
            this.cmbBroker.Leave += cmb_Leave;
            // 
            // pnlAllocation
            // 
            // 
            // pnlAllocation.ClientArea
            // 
            this.pnlAllocation.ClientArea.Controls.Add(this.btnCustomAllocation);
            this.pnlAllocation.ClientArea.Controls.Add(this.lblAllocation);
            this.pnlAllocation.ClientArea.Controls.Add(this.cmbAllocation);
            this.pnlAllocation.Location = new System.Drawing.Point(149, 3);
            this.pnlAllocation.Name = "pnlAllocation";
            this.pnlAllocation.Size = new System.Drawing.Size(135, 72);
            this.pnlAllocation.TabIndex = 1;
            // 
            // btnCustomAllocation
            // 
            appearanceCustomBtn.BorderColor = System.Drawing.Color.Black;
            appearanceCustomBtn.Image = global::Prana.TradingTicket.Properties.Resources.level1;
            appearanceCustomBtn.TextHAlignAsString = "Center";
            appearanceCustomBtn.TextVAlignAsString = "Top";
            this.btnCustomAllocation.Appearance = appearanceCustomBtn;
            this.btnCustomAllocation.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnCustomAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustomAllocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomAllocation.Location = new System.Drawing.Point(113, 5);
            this.btnCustomAllocation.Name = "btnCustomAllocation";
            this.btnCustomAllocation.Size = new System.Drawing.Size(19, 19);
            this.btnCustomAllocation.TabStop = false;
            this.btnCustomAllocation.Click += BtnCustomAllocation_Click;
            // 
            // lblAllocation
            // 
            this.lblAllocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllocation.Location = new System.Drawing.Point(7, 6);
            this.lblAllocation.Name = "lblAllocation";
            this.lblAllocation.Size = new System.Drawing.Size(81, 18);
            this.lblAllocation.TabStop = false;
            this.lblAllocation.Text = "Allocation";
            // 
            // cmbAllocation
            // 
            this.cmbAllocation.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbAllocation.DropDownListWidth = -1;
            this.cmbAllocation.Location = new System.Drawing.Point(5, 28);
            this.cmbAllocation.NullText = "Select Account";
            this.cmbAllocation.Name = "cmbAllocation";
            appearanceAlloc.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbAllocation.NullTextAppearance = appearanceAlloc;
            this.cmbAllocation.Size = new System.Drawing.Size(127, 20);
            this.cmbAllocation.TabIndex = 0;
            this.cmbAllocation.ValueChanged += cmbAllocation_ValueChanged;
            this.cmbAllocation.Leave += cmb_Leave;
            // 
            // pnlQuantity
            // 
            // 
            // pnlQuantity.ClientArea
            // 
            this.pnlQuantity.ClientArea.Controls.Add(this.btnHotQty3);
            this.pnlQuantity.ClientArea.Controls.Add(this.btnHotQty2);
            this.pnlQuantity.ClientArea.Controls.Add(this.btnPlusMinus);
            this.pnlQuantity.ClientArea.Controls.Add(this.btnHotQty1);
            this.pnlQuantity.ClientArea.Controls.Add(this.btnPosition);
            this.pnlQuantity.ClientArea.Controls.Add(this.nmrcQuantity);
            this.pnlQuantity.ClientArea.Controls.Add(this.lblQuantity);
            this.pnlQuantity.Location = new System.Drawing.Point(3, 3);
            this.pnlQuantity.Name = "pnlQuantity";
            this.pnlQuantity.Size = new System.Drawing.Size(140, 70);
            this.pnlQuantity.TabIndex = 0;
            // 
            // btnHotQty3
            // 
            this.btnHotQty3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHotQty3.Location = new System.Drawing.Point(100, 51);
            this.btnHotQty3.Name = "btnHotQty3";
            this.btnHotQty3.Size = new System.Drawing.Size(38, 19);
            this.btnHotQty3.TabStop = false;
            this.btnHotQty3.Text = "10K";
            this.btnHotQty3.Click += BtnHotQty3_Click;
            // 
            // btnHotQty2
            // 
            this.btnHotQty2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHotQty2.Location = new System.Drawing.Point(62, 51);
            this.btnHotQty2.Name = "btnHotQty2";
            this.btnHotQty2.Size = new System.Drawing.Size(38, 19);
            this.btnHotQty2.TabStop = false;
            this.btnHotQty2.Text = "1000";
            this.btnHotQty2.Click += BtnHotQty2_Click;
            // 
            // btnPlusMinus
            // 
            this.btnPlusMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlusMinus.Location = new System.Drawing.Point(3, 51);
            this.btnPlusMinus.Name = "btnPlusMinus";
            this.btnPlusMinus.Size = new System.Drawing.Size(19, 19);
            this.btnPlusMinus.TabStop = false;
            this.btnPlusMinus.Text = "+";
            this.btnPlusMinus.Click += BtnPlusMinus_Click;
            // 
            // btnHotQty1
            // 
            this.btnHotQty1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHotQty1.Location = new System.Drawing.Point(24, 51);
            this.btnHotQty1.Name = "btnHotQty1";
            this.btnHotQty1.Size = new System.Drawing.Size(38, 19);
            this.btnHotQty1.TabStop = false;
            this.btnHotQty1.Text = "100";
            this.btnHotQty1.Click += BtnHotQty1_Click;
            // 
            // btnPosition
            // 
            this.btnPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPosition.Location = new System.Drawing.Point(80, 5);
            this.btnPosition.Name = "btnPosition";
            this.btnPosition.Size = new System.Drawing.Size(57, 19);
            this.btnPosition.TabStop = false;
            this.btnPosition.Text = "= Position";
            this.btnPosition.Click += BtnPosition_Click;
            // 
            // nmrcQuantity
            // 
            this.nmrcQuantity.Increment = 0.0001m;
            this.nmrcQuantity.Location = new System.Drawing.Point(5, 28);
            this.nmrcQuantity.Maximum = 999999999;
            this.nmrcQuantity.Minimum = 0;
            this.nmrcQuantity.Name = "nmrcQuantity";
            this.nmrcQuantity.Size = new System.Drawing.Size(132, 20);
            this.nmrcQuantity.TabIndex = 0;
            this.nmrcQuantity.Value = 1;
            this.nmrcQuantity.RemoveThousandSeperatorOnEdit = true;
            this.nmrcQuantity.AllowThousandSeperator = true;
            this.nmrcQuantity.ShowCommaSeperatorOnEditing = true;
            this.nmrcQuantity.AutoSelect = true;
            this.nmrcQuantity.ValueChanged += new System.EventHandler(this.nmrcQuantity_ValueChanged);
            // 
            // lblQuantity
            // 
            this.lblQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.Location = new System.Drawing.Point(7, 6);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(61, 18);
            this.lblQuantity.Text = "Quantity";
            // 
            // lblErrorMsg
            // 
            this.lblErrorMsg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tblPnlMainControls.SetColumnSpan(this.lblErrorMsg, 4);
            this.lblErrorMsg.Location = new System.Drawing.Point(3, 159);
            this.lblErrorMsg.Name = "lblErrorMsg";
            this.lblErrorMsg.Size = new System.Drawing.Size(579, 13);
            this.lblErrorMsg.TabIndex = 11;
            this.lblErrorMsg.Text = "Error Message";
            // 
            // tblPnlButtonControls
            // 
            this.tblPnlButtonControls.ColumnCount = 1;
            this.tblPnlButtonControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPnlButtonControls.Controls.Add(this.btnBuy, 0, 0);
            this.tblPnlButtonControls.Controls.Add(this.btnSell, 0, 1);
            this.tblPnlButtonControls.Controls.Add(this.btnSellShort, 0, 2);
            this.tblPnlButtonControls.Controls.Add(this.btnBuyToCover, 0, 3);
            this.tblPnlButtonControls.Location = new System.Drawing.Point(584, 57);
            this.tblPnlButtonControls.Name = "tblPnlButtonControls";
            this.tblPnlButtonControls.RowCount = 4;
            this.tblPnlButtonControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlButtonControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlButtonControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlButtonControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlButtonControls.Size = new System.Drawing.Size(102, 175);
            this.tblPnlButtonControls.TabIndex = 2;
            // 
            // btnBuy
            // 
            this.btnBuy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBuy.Location = new System.Drawing.Point(3, 6);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(96, 30);
            this.btnBuy.TabIndex = 0;
            this.btnBuy.Text = "Buy";
            this.btnBuy.Click += BtnBuy_Click;
            // 
            // btnSell
            // 
            this.btnSell.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSell.Location = new System.Drawing.Point(3, 49);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(96, 30);
            this.btnSell.TabIndex = 1;
            this.btnSell.Text = "Sell";
            this.btnSell.Click += BtnSell_Click;
            // 
            // btnSellShort
            // 
            this.btnSellShort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSellShort.Location = new System.Drawing.Point(3, 92);
            this.btnSellShort.Name = "btnSellShort";
            this.btnSellShort.Size = new System.Drawing.Size(96, 30);
            this.btnSellShort.TabIndex = 2;
            this.btnSellShort.Text = "Sell Short";
            this.btnSellShort.Click += BtnSellShort_Click;
            // 
            // btnBuyToCover
            // 
            this.btnBuyToCover.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBuyToCover.Location = new System.Drawing.Point(3, 137);
            this.btnBuyToCover.Name = "btnBuyToCover";
            this.btnBuyToCover.Size = new System.Drawing.Size(96, 30);
            this.btnBuyToCover.TabIndex = 3;
            this.btnBuyToCover.Text = "Buy To Cover";
            this.btnBuyToCover.Click += BtnBuyToCover_Click;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            this.ultraFormManager1.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.UltraFormManager1_MouseEnterElement);
            this.ultraFormManager1.MouseLeaveElement += new Infragistics.Win.UIElementEventHandler(this.ultraFormManager1_MouseLeaveElement);
            // 
            // QuickTradingTicket_Fill_Panel
            // 
            // 
            // QuickTradingTicket_Fill_Panel.ClientArea
            // 
            this.QuickTradingTicket_Fill_Panel.ClientArea.Controls.Add(this.tblPnlButtonControls);
            this.QuickTradingTicket_Fill_Panel.ClientArea.Controls.Add(this.tblPnlMainControls);
            this.QuickTradingTicket_Fill_Panel.ClientArea.Controls.Add(this.tblPnlDetails);
            this.QuickTradingTicket_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.QuickTradingTicket_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickTradingTicket_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.QuickTradingTicket_Fill_Panel.Name = "QuickTradingTicket_Fill_Panel";
            this.QuickTradingTicket_Fill_Panel.Size = new System.Drawing.Size(690, 237);
            // 
            // _QuickTradingTicket_UltraFormManager_Dock_Area_Left
            // 
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.Name = "_QuickTradingTicket_UltraFormManager_Dock_Area_Left";
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 237);
            // 
            // _QuickTradingTicket_UltraFormManager_Dock_Area_Right
            // 
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(698, 32);
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.Name = "_QuickTradingTicket_UltraFormManager_Dock_Area_Right";
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 237);
            // 
            // _QuickTradingTicket_UltraFormManager_Dock_Area_Top
            // 
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.Name = "_QuickTradingTicket_UltraFormManager_Dock_Area_Top";
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(706, 32);
            // 
            // _QuickTradingTicket_UltraFormManager_Dock_Area_Bottom
            // 
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 269);
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.Name = "_QuickTradingTicket_UltraFormManager_Dock_Area_Bottom";
            this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(706, 8);
            // 
            // toolTipManager
            // 
            this.toolTipManager.ContainingControl = this;
            this.toolTipManager.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.Default;
            // 
            // QuickTradingTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 277);
            this.Controls.Add(this.QuickTradingTicket_Fill_Panel);
            this.Controls.Add(this._QuickTradingTicket_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._QuickTradingTicket_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._QuickTradingTicket_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._QuickTradingTicket_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.ControlBox = true;
            this.MaximumSize = new System.Drawing.Size(706, 277);
            this.MinimumSize = new System.Drawing.Size(706, 277);
            this.Name = "QuickTradingTicket";
            this.Load += new System.EventHandler(this.QuickTradingTicket_Load);
            this.FormClosing += QuickTradingTicket_FormClosing;
            this.tblPnlDetails.ResumeLayout(false);
            this.pnlSymbol.ClientArea.ResumeLayout(false);
            this.pnlSymbol.ResumeLayout(false);
            this.tblPnlMainControls.ResumeLayout(false);
            this.tblBottomStrip.ResumeLayout(false);
            this.pnlPrice.ClientArea.ResumeLayout(false);
            this.pnlPrice.ResumeLayout(false);
            this.pnlPriceButtons.ClientArea.ResumeLayout(false);
            this.pnlRoundLot.ResumeLayout(false);
            this.pnlRoundLot.ClientArea.ResumeLayout(false);
            this.pnlPriceButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcPrice)).EndInit();
            this.pnlStop.ClientArea.ResumeLayout(false);
            this.pnlStop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcStop)).EndInit();
            this.pnlOrderType.ClientArea.ResumeLayout(false);
            this.pnlOrderType.ClientArea.PerformLayout();
            this.pnlOrderType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).EndInit();
            this.pnlTIF.ClientArea.ResumeLayout(false);
            this.pnlTIF.ClientArea.PerformLayout();
            this.pnlTIF.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).EndInit();
            this.pnlVenue.ClientArea.ResumeLayout(false);
            this.pnlVenue.ClientArea.PerformLayout();
            this.pnlVenue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            this.pnlBroker.ClientArea.ResumeLayout(false);
            this.pnlBroker.ClientArea.PerformLayout();
            this.pnlBroker.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBroker)).EndInit();
            this.pnlAllocation.ClientArea.ResumeLayout(false);
            this.pnlAllocation.ClientArea.PerformLayout();
            this.pnlAllocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocation)).EndInit();
            this.pnlQuantity.ClientArea.ResumeLayout(false);
            this.pnlQuantity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcQuantity)).EndInit();
            this.tblPnlButtonControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.QuickTradingTicket_Fill_Panel.ClientArea.ResumeLayout(false);
            this.QuickTradingTicket_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblPnlDetails;
        private System.Windows.Forms.TableLayoutPanel tblPnlMainControls;
        private System.Windows.Forms.TableLayoutPanel tblPnlButtonControls;
        private System.Windows.Forms.TableLayoutPanel tblBottomStrip;
        private Infragistics.Win.Misc.UltraLabel lblSymbol;
        private Infragistics.Win.Misc.UltraLabel lblQuantity;
        private Infragistics.Win.Misc.UltraPanel pnlStop;
        private Infragistics.Win.Misc.UltraLabel lblStop;
        private Infragistics.Win.Misc.UltraPanel pnlOrderType;
        private Infragistics.Win.Misc.UltraLabel lblOrderType;
        private Infragistics.Win.Misc.UltraPanel pnlTIF;
        private Infragistics.Win.Misc.UltraLabel lblTIF;
        private System.Windows.Forms.Label lblNotionLoc;
        private System.Windows.Forms.Label lblNotionBase;
        private System.Windows.Forms.Label lblNotionLocCalc;
        private System.Windows.Forms.Label lblNotionBaseCalc;
        private System.Windows.Forms.Label lblRoundLot;
        private System.Windows.Forms.Label lblRoundLotValue;
        private Infragistics.Win.Misc.UltraPanel pnlRoundLot;
        private Utilities.UI.ToggleSwitch toggleSwitchRoundLot;
        private System.Windows.Forms.Label lblSplitter3;
        private Infragistics.Win.Misc.UltraPanel pnlBroker;
        private Infragistics.Win.Misc.UltraPanel pnlQuantity;
        private Infragistics.Win.Misc.UltraPanel pnlAllocation;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAllocation;
        private Infragistics.Win.Misc.UltraButton btnBuyToCover;
        private Infragistics.Win.Misc.UltraButton btnSellShort;
        private Infragistics.Win.Misc.UltraButton btnSell;
        private Infragistics.Win.Misc.UltraButton btnBuy;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOrderType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTIF;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbBroker;
        private Utilities.UI.UIUtilities.PranaNumericUpDown nmrcQuantity;
        private Utilities.UI.UIUtilities.PranaSymbolCtrl pranaSymbolCtrl;
        private Utilities.UI.UIUtilities.PranaNumericUpDown nmrcStop;
        private Infragistics.Win.Misc.UltraPanel pnlSymbol;
        private Infragistics.Win.Misc.UltraButton btnPlusMinus;
        private Infragistics.Win.Misc.UltraButton btnHotQty1;
        private Infragistics.Win.Misc.UltraButton btnPosition;
        private Infragistics.Win.Misc.UltraButton btnHotQty2;
        private Infragistics.Win.Misc.UltraButton btnHotQty3;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel QuickTradingTicket_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _QuickTradingTicket_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _QuickTradingTicket_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _QuickTradingTicket_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _QuickTradingTicket_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraPanel pnlPrice;
        private Infragistics.Win.Misc.UltraPanel pnlPriceButtons;
        private Infragistics.Win.Misc.UltraButton btnMid;
        private Infragistics.Win.Misc.UltraButton btnBid;
        private Utilities.UI.UIUtilities.PranaNumericUpDown nmrcPrice;
        private Infragistics.Win.Misc.UltraLabel lblPrice;
        private Infragistics.Win.Misc.UltraPanel pnlVenue;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbVenue;
        private Infragistics.Win.Misc.UltraLabel lblErrorMsg;
        private System.Windows.Forms.Label lblSplitter1;
        private System.Windows.Forms.Label lblSplitter2;
        private Infragistics.Win.Misc.UltraButton btnAlgo;
        private Infragistics.Win.Misc.UltraLabel lblVenue;
        private Infragistics.Win.Misc.UltraButton btnBroker;
        private Infragistics.Win.Misc.UltraLabel lblBroker;
        private Infragistics.Win.Misc.UltraButton btnCustomAllocation;
        private Infragistics.Win.Misc.UltraLabel lblAllocation;
        private Infragistics.Win.Misc.UltraButton btnAsk;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btnSymbolLookup;
        private LiveFeed.UI.Controls.QTTL1Strip qttL1Strip;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager toolTipManager;
    }
}