using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlCloseTrade
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
                if (headerCheckBoxUnWind != null)
                {
                    headerCheckBoxUnWind.Dispose();
                }
                if (myerror != null)
                {
                    myerror.Dispose();
                }
                if (preferences != null)
                {
                    preferences.Dispose();
                    preferences = null;
                }
                if (_allocationServices != null)
                {
                    _allocationServices.Dispose();
                }
                if (_closingServices != null)
                {
                    _closingServices.Dispose();
                }
                if (_gridBandShortPositions != null)
                {
                    _gridBandShortPositions.Dispose();
                }
                if (_gridBandLongPositions != null)
                {
                    _gridBandLongPositions.Dispose();
                }
                if (_grdBandNetPositions != null)
                {
                    _grdBandNetPositions.Dispose();
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
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            this.cntxtMenuStripClosedPosition = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.unwindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearFilterStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToExcelStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer15 = new System.Windows.Forms.SplitContainer();
            this.cmbSecondarySort = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbClosingField = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblClosingField = new Infragistics.Win.Misc.UltraLabel();
            this.chkBoxSellWithBTC = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnClearFilter = new Infragistics.Win.Misc.UltraButton();
            this.chkBoxBuyAndBuyToCover = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cbSyncFilter = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbMethodology = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbAlgorithm = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnRun = new Infragistics.Win.Misc.UltraButton();
            this.grpOpenPosition = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel9 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.grpLongPosition = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel10 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdLong = new PranaUltraGrid();
            this.contextMenuShortLongPosition = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutShortLongtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToExcelShortLongStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearFilterShortLongStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpShortPosition = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel12 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdShort = new PranaUltraGrid();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.grpBoxNetPosition = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel8 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdNetPosition = new PranaUltraGrid();
            this.cntxtMenuStripSplit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSplitItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyTaxlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoSplitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraExpandableGroupBoxPanel7 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBoxPanel3 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraExpandableGroupBox3 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel6 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraExpandableGroupBoxPanel5 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraExpandableGroupBox4 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel11 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButton3 = new Infragistics.Win.Misc.UltraButton();
            this.ultraCombo1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraCombo2 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraExpandableGroupBox5 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel13 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBox6 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel14 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraExpandableGroupBox7 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel15 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBox8 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel16 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ultraButton4 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton5 = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButton6 = new Infragistics.Win.Misc.UltraButton();
            this.ultraCombo3 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraCombo4 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraExpandableGroupBox9 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel17 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer10 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBox10 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel18 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer11 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBox12 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel20 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer12 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ultraButton7 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton8 = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButton9 = new Infragistics.Win.Misc.UltraButton();
            this.ultraCombo5 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraCombo6 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraExpandableGroupBox13 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel21 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.splitContainer13 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBox14 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel22 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraGrid5 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraExpandableGroupBox15 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel23 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraGrid6 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.splitContainer14 = new System.Windows.Forms.SplitContainer();
            this.ultraExpandableGroupBox16 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel24 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
           // this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.chkBxIsAutoCloseStrategy = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkCopyOpeningTradeAttributes = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cntxtMenuStripClosedPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer15)).BeginInit();
            this.splitContainer15.Panel1.SuspendLayout();
            this.splitContainer15.Panel2.SuspendLayout();
            this.splitContainer15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecondarySort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClosingField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxSellWithBTC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxBuyAndBuyToCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSyncFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMethodology)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlgorithm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpOpenPosition)).BeginInit();
            this.grpOpenPosition.SuspendLayout();
            this.ultraExpandableGroupBoxPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpLongPosition)).BeginInit();
            this.grpLongPosition.SuspendLayout();
            this.ultraExpandableGroupBoxPanel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLong)).BeginInit();
            this.contextMenuShortLongPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpShortPosition)).BeginInit();
            this.grpShortPosition.SuspendLayout();
            this.ultraExpandableGroupBoxPanel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdShort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxNetPosition)).BeginInit();
            this.grpBoxNetPosition.SuspendLayout();
            this.ultraExpandableGroupBoxPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNetPosition)).BeginInit();
            this.cntxtMenuStripSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox3)).BeginInit();
            this.ultraExpandableGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox4)).BeginInit();
            this.ultraExpandableGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox5)).BeginInit();
            this.ultraExpandableGroupBox5.SuspendLayout();
            this.ultraExpandableGroupBoxPanel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox6)).BeginInit();
            this.ultraExpandableGroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox7)).BeginInit();
            this.ultraExpandableGroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox8)).BeginInit();
            this.ultraExpandableGroupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox9)).BeginInit();
            this.ultraExpandableGroupBox9.SuspendLayout();
            this.ultraExpandableGroupBoxPanel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).BeginInit();
            this.splitContainer10.Panel1.SuspendLayout();
            this.splitContainer10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox10)).BeginInit();
            this.ultraExpandableGroupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).BeginInit();
            this.splitContainer11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox12)).BeginInit();
            this.ultraExpandableGroupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).BeginInit();
            this.splitContainer12.Panel1.SuspendLayout();
            this.splitContainer12.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox13)).BeginInit();
            this.ultraExpandableGroupBox13.SuspendLayout();
            this.ultraExpandableGroupBoxPanel21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer13)).BeginInit();
            this.splitContainer13.Panel1.SuspendLayout();
            this.splitContainer13.Panel2.SuspendLayout();
            this.splitContainer13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox14)).BeginInit();
            this.ultraExpandableGroupBox14.SuspendLayout();
            this.ultraExpandableGroupBoxPanel22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox15)).BeginInit();
            this.ultraExpandableGroupBox15.SuspendLayout();
            this.ultraExpandableGroupBoxPanel23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer14)).BeginInit();
            this.splitContainer14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox16)).BeginInit();
            this.ultraExpandableGroupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBxIsAutoCloseStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCopyOpeningTradeAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // cntxtMenuStripClosedPosition
            // 
            this.cntxtMenuStripClosedPosition.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.unwindToolStripMenuItem,
                this.clearFilterStripMenuItem,
                this.saveLayoutStripMenuItem,
                this.exportToExcelStripMenuItem});
            this.cntxtMenuStripClosedPosition.Name = "cntxtMenuStripNetPositionsGrid";
            this.cntxtMenuStripClosedPosition.Size = new System.Drawing.Size(154, 70);
            this.cntxtMenuStripClosedPosition.Text = "Save Layout";
            this.cntxtMenuStripClosedPosition.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtMenuStripClosedPosition_Opening);
            // 
            // unwindToolStripMenuItem
            // 
            this.unwindToolStripMenuItem.Name = "unwindToolStripMenuItem";
            this.unwindToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.unwindToolStripMenuItem.Text = "Unwind";
            this.unwindToolStripMenuItem.Click += new System.EventHandler(this.unwindToolStripMenuItem_Click);
            // 
            // saveLayoutStripMenuItem
            // 
            this.saveLayoutStripMenuItem.Name = "saveLayoutStripMenuItem";
            this.saveLayoutStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveLayoutStripMenuItem.Text = "Save Layout";
            this.saveLayoutStripMenuItem.Click += new System.EventHandler(this.saveLayoutStripMenuItem_Click);
            // 
            // exportToExcelStripMenuItem
            // 
            this.exportToExcelStripMenuItem.Name = "exportToExcelStripMenuItem";
            this.exportToExcelStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.exportToExcelStripMenuItem.Text = "Export To Excel";
            this.exportToExcelStripMenuItem.Click += new System.EventHandler(this.exportToExcelStripMenuItem_Click);
            // 
            // clearFilterStripMenuItem
            // 
            this.clearFilterStripMenuItem.Name = "clearFilterStripMenuItem";
            this.clearFilterStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.clearFilterStripMenuItem.Text = "Clear Filters";
            this.clearFilterStripMenuItem.Click += new System.EventHandler(this.ClearNetPositionGridFiltersStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer15);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1108, 533);
            this.splitContainer2.SplitterDistance = 275;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 2;
            // 
            // splitContainer15
            // 
            this.splitContainer15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer15.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer15.Location = new System.Drawing.Point(0, 0);
            this.splitContainer15.Name = "splitContainer15";
            this.splitContainer15.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer15.Panel1
            // 
            this.splitContainer15.Panel1.Controls.Add(this.chkBxIsAutoCloseStrategy);
            this.splitContainer15.Panel1.Controls.Add(this.cmbSecondarySort);
            this.splitContainer15.Panel1.Controls.Add(this.label3);
            this.splitContainer15.Panel1.Controls.Add(this.chkBoxSellWithBTC);
            this.splitContainer15.Panel1.Controls.Add(this.btnClearFilter);
            this.splitContainer15.Panel1.Controls.Add(this.chkBoxBuyAndBuyToCover);
            this.splitContainer15.Panel1.Controls.Add(this.cbSyncFilter);
            this.splitContainer15.Panel1.Controls.Add(this.label2);
            this.splitContainer15.Panel1.Controls.Add(this.label1);
            this.splitContainer15.Panel1.Controls.Add(this.cmbMethodology);
            this.splitContainer15.Panel1.Controls.Add(this.cmbAlgorithm);
            this.splitContainer15.Panel1.Controls.Add(this.btnRun);
            this.splitContainer15.Panel1.Controls.Add(this.lblClosingField);
            this.splitContainer15.Panel1.Controls.Add(this.cmbClosingField);
            this.splitContainer15.Panel1.Controls.Add(this.chkCopyOpeningTradeAttributes); 
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer15.Panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // splitContainer15.Panel2
            // 
            this.splitContainer15.Panel2.Controls.Add(this.grpOpenPosition);
            this.splitContainer15.Size = new System.Drawing.Size(1108, 275);
            this.splitContainer15.SplitterDistance = 48;
            this.splitContainer15.SplitterWidth = 1;
            this.splitContainer15.TabIndex = 107;
            // 
            // chkCopyOpeningTradeAttributes
            // 
            this.chkCopyOpeningTradeAttributes.Location = new System.Drawing.Point(320, 28);
            this.chkCopyOpeningTradeAttributes.Name = "chkCopyOpeningTradeAttributes";
            this.chkCopyOpeningTradeAttributes.Size = new System.Drawing.Size(300, 17);
            this.chkCopyOpeningTradeAttributes.TabIndex = 117;
            this.chkCopyOpeningTradeAttributes.Text = "Copy Opening Trade Attributes to Closing Trades";
            // 
            // cmbSecondarySort
            // 
            this.cmbSecondarySort.AutoSize = false;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            appearance1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSecondarySort.DisplayLayout.Appearance = appearance1;
            this.cmbSecondarySort.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSecondarySort.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.Color.Silver;
            appearance2.ForeColor = System.Drawing.SystemColors.HighlightText;
            appearance2.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSecondarySort.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.Control;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSecondarySort.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbSecondarySort.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance4.TextHAlignAsString = "Left";
            this.cmbSecondarySort.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbSecondarySort.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSecondarySort.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.BorderColor = System.Drawing.Color.Silver;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSecondarySort.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSecondarySort.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbSecondarySort.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSecondarySort.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSecondarySort.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderColor = System.Drawing.SystemColors.Window;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSecondarySort.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbSecondarySort.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSecondarySort.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            appearance9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSecondarySort.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance10.BackColor2 = System.Drawing.SystemColors.Control;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance10.TextHAlignAsString = "Left";
            this.cmbSecondarySort.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbSecondarySort.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSecondarySort.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSecondarySort.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbSecondarySort.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.Highlight;
            appearance12.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSecondarySort.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbSecondarySort.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSecondarySort.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSecondarySort.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSecondarySort.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSecondarySort.Location = new System.Drawing.Point(470, 2);
            this.cmbSecondarySort.Name = "cmbSecondarySort";
            this.cmbSecondarySort.Size = new System.Drawing.Size(100, 22);
            this.cmbSecondarySort.TabIndex = 13;
            this.cmbSecondarySort.ValueChanged += new System.EventHandler(this.cmbSecondarySort_ValueChanged);
            this.cmbSecondarySort.MouseHover += new System.EventHandler(this.cmbSecondarySort_MouseHover);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(332, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 15);
            this.label3.TabIndex = 116;
            this.label3.Text = "Secondary Sort Criteria";

            // 
            // lblClosingField
            // 
            this.lblClosingField.AutoSize = true;
            this.lblClosingField.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblClosingField.Location = new System.Drawing.Point(571, 6);
            this.lblClosingField.Name = "lblClosingField";
            this.lblClosingField.Size = new System.Drawing.Size(137, 15);
            this.lblClosingField.TabIndex = 116;
            this.lblClosingField.Text = "Closing Field";

            // 
            // cmbClosingField
            // 
            this.cmbClosingField.AutoSize = false;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            appearance1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClosingField.DisplayLayout.Appearance = appearance1;
            this.cmbClosingField.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClosingField.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.Color.Silver;
            appearance2.ForeColor = System.Drawing.SystemColors.HighlightText;
            appearance2.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClosingField.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.Control;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClosingField.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbClosingField.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance4.TextHAlignAsString = "Left";
            this.cmbClosingField.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbClosingField.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClosingField.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.BorderColor = System.Drawing.Color.Silver;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClosingField.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClosingField.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbClosingField.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClosingField.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClosingField.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderColor = System.Drawing.SystemColors.Window;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClosingField.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbClosingField.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClosingField.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            appearance9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClosingField.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance10.BackColor2 = System.Drawing.SystemColors.Control;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance10.TextHAlignAsString = "Left";
            this.cmbClosingField.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbClosingField.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClosingField.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClosingField.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbClosingField.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.Highlight;
            appearance12.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClosingField.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbClosingField.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClosingField.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClosingField.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClosingField.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClosingField.Location = new System.Drawing.Point(650, 2);
            this.cmbClosingField.Name = "cmbClosingField";
            this.cmbClosingField.Size = new System.Drawing.Size(100, 22);
            this.cmbClosingField.TabIndex = 13;
            this.cmbClosingField.ValueChanged += new System.EventHandler(this.cmbClosingField_ValueChanged);
            this.cmbClosingField.MouseHover += new System.EventHandler(this.cmbClosingField_MouseHover);
            // 
            // chkBoxSellWithBTC
            // 
            this.chkBoxSellWithBTC.AutoSize = true;
            this.chkBoxSellWithBTC.Location = new System.Drawing.Point(838, 22);
            this.chkBoxSellWithBTC.Name = "chkBoxSellWithBTC";
            this.chkBoxSellWithBTC.Size = new System.Drawing.Size(188, 17);
            this.chkBoxSellWithBTC.TabIndex = 17;
            this.chkBoxSellWithBTC.Text = "Can close Sell with Buy To Close";
            this.chkBoxSellWithBTC.CheckedChanged += new System.EventHandler(this.chkboxSellWithBTC_CheckedChanged);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClearFilter.Location = new System.Drawing.Point(80, 27);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.ShowFocusRect = false;
            this.btnClearFilter.ShowOutline = false;
            this.btnClearFilter.Size = new System.Drawing.Size(79, 21);
            this.btnClearFilter.TabIndex = 19;
            this.btnClearFilter.Text = "Clear Filters";
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // chkBoxBuyAndBuyToCover
            // 
            this.chkBoxBuyAndBuyToCover.AutoSize = true;
            this.chkBoxBuyAndBuyToCover.Location = new System.Drawing.Point(838, 2);
            this.chkBoxBuyAndBuyToCover.Name = "chkBoxBuyAndBuyToCover";
            this.chkBoxBuyAndBuyToCover.Size = new System.Drawing.Size(257, 17);
            this.chkBoxBuyAndBuyToCover.TabIndex = 16;
            this.chkBoxBuyAndBuyToCover.Text = "Can close Sell Short with Buy and Buy to Open";
            this.chkBoxBuyAndBuyToCover.CheckedChanged += new System.EventHandler(this.chkBoxBuyAndBuyToCover_CheckedChanged);
            // 
            // cbSyncFilter
            // 
            this.cbSyncFilter.AutoSize = true;
            this.cbSyncFilter.Checked = true;
            this.cbSyncFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSyncFilter.Location = new System.Drawing.Point(6, 27);
            this.cbSyncFilter.Name = "cbSyncFilter";
            this.cbSyncFilter.Size = new System.Drawing.Size(75, 17);
            this.cbSyncFilter.TabIndex = 18;
            this.cbSyncFilter.Text = "Sync Filter";
            this.cbSyncFilter.CheckedChanged += new System.EventHandler(this.cbSyncFilter_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(165, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 14);
            this.label2.TabIndex = 108;
            this.label2.Text = "Algorithm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 107;
            this.label1.Text = "Method";
            // 
            // cmbMethodology
            // 
            this.cmbMethodology.AutoSize = false;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbMethodology.DisplayLayout.Appearance = appearance13;
            this.cmbMethodology.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbMethodology.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            appearance14.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbMethodology.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.BackColor = System.Drawing.SystemColors.Control;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMethodology.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbMethodology.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance16.TextHAlignAsString = "Left";
            this.cmbMethodology.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbMethodology.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbMethodology.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbMethodology.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderColor = System.Drawing.Color.Silver;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            appearance18.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbMethodology.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbMethodology.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbMethodology.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Control;
            appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance19.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance19.BorderColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMethodology.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbMethodology.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbMethodology.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbMethodology.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            appearance21.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_save;
            this.cmbMethodology.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            appearance22.TextHAlignAsString = "Left";
            this.cmbMethodology.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbMethodology.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbMethodology.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMethodology.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbMethodology.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance24.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMethodology.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbMethodology.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbMethodology.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbMethodology.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbMethodology.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbMethodology.Location = new System.Drawing.Point(63, 2);
            this.cmbMethodology.Name = "cmbMethodology";
            this.cmbMethodology.Size = new System.Drawing.Size(100, 22);
            this.cmbMethodology.TabIndex = 11;
            this.cmbMethodology.ValueChanged += new System.EventHandler(this.cmbMethodology_ValueChanged);
            this.cmbMethodology.MouseHover += new System.EventHandler(this.cmbMethodology_MouseHover);
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.AutoSize = false;
            this.cmbAlgorithm.DisplayLayout.Appearance = appearance17;
            this.cmbAlgorithm.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAlgorithm.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbAlgorithm.DisplayLayout.GroupByBox.Appearance = appearance18;
            this.cmbAlgorithm.DisplayLayout.GroupByBox.BandLabelAppearance = appearance19;
            this.cmbAlgorithm.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance25.TextHAlignAsString = "Left";
            this.cmbAlgorithm.DisplayLayout.GroupByBox.PromptAppearance = appearance25;
            this.cmbAlgorithm.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAlgorithm.DisplayLayout.MaxRowScrollRegions = 1;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.BorderColor = System.Drawing.Color.Silver;
            appearance26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAlgorithm.DisplayLayout.Override.ActiveCellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance27.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAlgorithm.DisplayLayout.Override.ActiveRowAppearance = appearance27;
            this.cmbAlgorithm.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAlgorithm.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance28.BackColor = System.Drawing.SystemColors.Window;
            appearance28.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAlgorithm.DisplayLayout.Override.CardAreaAppearance = appearance28;
            appearance29.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance29.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance29.BorderColor = System.Drawing.SystemColors.Window;
            appearance29.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAlgorithm.DisplayLayout.Override.CellAppearance = appearance29;
            this.cmbAlgorithm.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAlgorithm.DisplayLayout.Override.CellPadding = 0;
            appearance30.BackColor = System.Drawing.SystemColors.Control;
            appearance30.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance30.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.BorderColor = System.Drawing.SystemColors.Window;
            appearance30.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAlgorithm.DisplayLayout.Override.GroupByRowAppearance = appearance30;
            appearance31.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance31.BackColor2 = System.Drawing.SystemColors.Control;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance31.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance31.TextHAlignAsString = "Left";
            this.cmbAlgorithm.DisplayLayout.Override.HeaderAppearance = appearance31;
            this.cmbAlgorithm.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAlgorithm.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAlgorithm.DisplayLayout.Override.RowAppearance = appearance32;
            this.cmbAlgorithm.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance33.BackColor = System.Drawing.SystemColors.Highlight;
            appearance33.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAlgorithm.DisplayLayout.Override.TemplateAddRowAppearance = appearance33;
            this.cmbAlgorithm.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAlgorithm.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAlgorithm.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAlgorithm.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAlgorithm.Location = new System.Drawing.Point(230, 2);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(100, 22);
            this.cmbAlgorithm.TabIndex = 12;
            this.cmbAlgorithm.ValueChanged += new System.EventHandler(this.cmbAlgorithm_ValueChanged);
            this.cmbAlgorithm.MouseHover += new System.EventHandler(this.cmbAlgorithm_MouseHover);
            // 
            // btnRun
            // 
            this.btnRun.ImageSize = new System.Drawing.Size(75, 23);
            this.btnRun.Location = new System.Drawing.Point(753, 2);
            this.btnRun.Name = "btnRun";
            this.btnRun.ShowFocusRect = false;
            this.btnRun.ShowOutline = false;
            this.btnRun.Size = new System.Drawing.Size(79, 21);
            this.btnRun.TabIndex = 14;
            this.btnRun.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // grpOpenPosition
            // 
            this.grpOpenPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOpenPosition.Controls.Add(this.ultraExpandableGroupBoxPanel9);
            this.grpOpenPosition.ExpandedSize = new System.Drawing.Size(1108, 238);
            this.grpOpenPosition.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.grpOpenPosition.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.grpOpenPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpOpenPosition.Location = new System.Drawing.Point(0, -4);
            this.grpOpenPosition.Name = "grpOpenPosition";
            this.grpOpenPosition.Size = new System.Drawing.Size(1108, 238);
            this.grpOpenPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOpenPosition.TabIndex = 100;
            this.grpOpenPosition.Text = "Open Taxlots";
            // 
            // ultraExpandableGroupBoxPanel9
            // 
            this.ultraExpandableGroupBoxPanel9.Controls.Add(this.splitContainer6);
            this.ultraExpandableGroupBoxPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel9.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel9.Name = "ultraExpandableGroupBoxPanel9";
            this.ultraExpandableGroupBoxPanel9.Size = new System.Drawing.Size(1102, 216);
            this.ultraExpandableGroupBoxPanel9.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.grpLongPosition);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.grpShortPosition);
            this.splitContainer6.Size = new System.Drawing.Size(1102, 216);
            this.splitContainer6.SplitterDistance = 551;
            this.splitContainer6.SplitterWidth = 1;
            this.splitContainer6.TabIndex = 0;
            // 
            // grpLongPosition
            // 
            this.grpLongPosition.Controls.Add(this.ultraExpandableGroupBoxPanel10);
            this.grpLongPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLongPosition.ExpandedSize = new System.Drawing.Size(551, 216);
            this.grpLongPosition.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.RightArrow1;
            this.grpLongPosition.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.LeftArrow2;
            this.grpLongPosition.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.RightOnBorder;
            this.grpLongPosition.Location = new System.Drawing.Point(0, 0);
            this.grpLongPosition.Name = "grpLongPosition";
            this.grpLongPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grpLongPosition.Size = new System.Drawing.Size(551, 216);
            this.grpLongPosition.TabIndex = 0;
            this.grpLongPosition.Text = "Long Position";
            this.grpLongPosition.ExpandedStateChanged += new System.EventHandler(this.grdBoxLongPosition_ExpandedStateChanged);
            // 
            // ultraExpandableGroupBoxPanel10
            // 
            this.ultraExpandableGroupBoxPanel10.Controls.Add(this.grdLong);
            this.ultraExpandableGroupBoxPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel10.Location = new System.Drawing.Point(3, 3);
            this.ultraExpandableGroupBoxPanel10.Name = "ultraExpandableGroupBoxPanel10";
            this.ultraExpandableGroupBoxPanel10.Size = new System.Drawing.Size(529, 210);
            this.ultraExpandableGroupBoxPanel10.TabIndex = 0;
            // 
            // grdLong
            // 
            this.grdLong.ContextMenuStrip = this.contextMenuShortLongPosition;
            appearance34.BackColor = System.Drawing.Color.Black;
            this.grdLong.DisplayLayout.Appearance = appearance34;
            this.grdLong.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdLong.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdLong.DisplayLayout.GroupByBox.Hidden = true;
            this.grdLong.DisplayLayout.MaxBandDepth = 6;
            this.grdLong.DisplayLayout.MaxColScrollRegions = 1;
            this.grdLong.DisplayLayout.MaxRowScrollRegions = 1;
            appearance35.BackColor = System.Drawing.Color.LightSlateGray;
            appearance35.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance35.BorderColor = System.Drawing.Color.DimGray;
            appearance35.FontData.BoldAsString = "True";
            appearance35.ForeColor = System.Drawing.Color.White;
            this.grdLong.DisplayLayout.Override.ActiveRowAppearance = appearance35;
            this.grdLong.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdLong.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdLong.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdLong.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdLong.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdLong.DisplayLayout.Override.CellPadding = 0;
            this.grdLong.DisplayLayout.Override.CellSpacing = 0;
            appearance36.FontData.Name = "Tahoma";
            appearance36.FontData.SizeInPoints = 8F;
            appearance36.TextHAlignAsString = "Center";
            this.grdLong.DisplayLayout.Override.HeaderAppearance = appearance36;
            this.grdLong.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdLong.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance37.ForeColor = System.Drawing.Color.White;
            appearance37.TextHAlignAsString = "Right";
            appearance37.TextVAlignAsString = "Middle";
            this.grdLong.DisplayLayout.Override.RowAlternateAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.Black;
            appearance38.ForeColor = System.Drawing.Color.White;
            appearance38.TextHAlignAsString = "Right";
            appearance38.TextVAlignAsString = "Middle";
            this.grdLong.DisplayLayout.Override.RowAppearance = appearance38;
            this.grdLong.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdLong.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdLong.DisplayLayout.Override.RowSelectorWidth = 25;
            appearance39.BackColor = System.Drawing.Color.Transparent;
            appearance39.BorderColor = System.Drawing.Color.Transparent;
            appearance39.FontData.BoldAsString = "True";
            this.grdLong.DisplayLayout.Override.SelectedRowAppearance = appearance39;
            this.grdLong.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdLong.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdLong.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdLong.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdLong.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdLong.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdLong.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdLong.DisplayLayout.Override.TemplateAddRowAppearance = appearance40;
            this.grdLong.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdLong.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdLong.DisplayLayout.UseFixedHeaders = true;
            this.grdLong.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdLong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdLong.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdLong.Location = new System.Drawing.Point(0, 0);
            this.grdLong.Name = "grdLong";
            this.grdLong.Size = new System.Drawing.Size(529, 210);
            this.grdLong.SyncWithCurrencyManager = false;
            this.grdLong.TabIndex = 20;
            this.grdLong.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdLong_InitializeLayout);
            this.grdLong.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdLong_InitializeRow);
            this.grdLong.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdLong_AfterRowFilterChanged);
            this.grdLong.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdLong_BeforeCustomRowFilterDialog);
            this.grdLong.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdLong_BeforeColumnChooserDisplayed);
            this.grdLong.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdLong_DragDrop);
            this.grdLong.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdLong_DragEnter);
            this.grdLong.DragOver += new System.Windows.Forms.DragEventHandler(this.grdLong_DragEnter);
            this.grdLong.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdLong_MouseDown);
            this.grdLong.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdLong_MouseMove);
            this.grdLong.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdLong_MouseUp);
            this.grdLong.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            // 
            // contextMenuShortLongPosition
            // 
            this.contextMenuShortLongPosition.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.clearFilterShortLongStripMenuItem,
                this.saveLayoutShortLongtoolStripMenuItem,
                this.exportToExcelShortLongStripMenuItem});
            this.contextMenuShortLongPosition.Name = "cntxtMenuStripNetPositionsGrid";
            this.contextMenuShortLongPosition.Size = new System.Drawing.Size(154, 48);
            // 
            // saveLayoutShortLongtoolStripMenuItem
            // 
            this.saveLayoutShortLongtoolStripMenuItem.Name = "saveLayoutShortLongtoolStripMenuItem";
            this.saveLayoutShortLongtoolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveLayoutShortLongtoolStripMenuItem.Text = "Save Layout";
            this.saveLayoutShortLongtoolStripMenuItem.Click += new System.EventHandler(this.saveLayoutShortLongtoolStripMenuItem_Click);
            // 
            // exportToExcelShortLongStripMenuItem
            // 
            this.exportToExcelShortLongStripMenuItem.Name = "exportToExcelShortLongStripMenuItem";
            this.exportToExcelShortLongStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.exportToExcelShortLongStripMenuItem.Text = "Export To Excel";
            this.exportToExcelShortLongStripMenuItem.Click += new System.EventHandler(this.exportToExcelShortLongStripMenuItem_Click);
            // 
            // clearFilterShortLongStripMenuItem
            // 
            this.clearFilterShortLongStripMenuItem.Name = "clearFilterShortLongStripMenuItem";
            this.clearFilterShortLongStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.clearFilterShortLongStripMenuItem.Text = "Clear Filters";
            this.clearFilterShortLongStripMenuItem.Click += new System.EventHandler(this.ClearLongAndShortGridFiltersStripMenuItem_Click);
            // 
            // grpShortPosition
            // 
            this.grpShortPosition.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Far;
            this.grpShortPosition.Controls.Add(this.ultraExpandableGroupBoxPanel12);
            this.grpShortPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpShortPosition.ExpandedSize = new System.Drawing.Size(550, 216);
            this.grpShortPosition.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.LeftArrow2;
            this.grpShortPosition.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.RightArrow1;
            this.grpShortPosition.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.LeftOnBorder;
            this.grpShortPosition.Location = new System.Drawing.Point(0, 0);
            this.grpShortPosition.Name = "grpShortPosition";
            this.grpShortPosition.Size = new System.Drawing.Size(550, 216);
            this.grpShortPosition.TabIndex = 0;
            this.grpShortPosition.Text = "ShortPosition";
            this.grpShortPosition.ExpandedStateChanged += new System.EventHandler(this.grdBoxShortPosition_ExpandedStateChanged);
            // 
            // ultraExpandableGroupBoxPanel12
            // 
            this.ultraExpandableGroupBoxPanel12.Controls.Add(this.grdShort);
            this.ultraExpandableGroupBoxPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel12.Location = new System.Drawing.Point(19, 3);
            this.ultraExpandableGroupBoxPanel12.Name = "ultraExpandableGroupBoxPanel12";
            this.ultraExpandableGroupBoxPanel12.Size = new System.Drawing.Size(528, 210);
            this.ultraExpandableGroupBoxPanel12.TabIndex = 0;
            // 
            // grdShort
            // 
            this.grdShort.ContextMenuStrip = this.contextMenuShortLongPosition;
            appearance41.BackColor = System.Drawing.Color.Black;
            this.grdShort.DisplayLayout.Appearance = appearance41;
            this.grdShort.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdShort.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdShort.DisplayLayout.GroupByBox.Hidden = true;
            this.grdShort.DisplayLayout.MaxBandDepth = 6;
            this.grdShort.DisplayLayout.MaxColScrollRegions = 1;
            this.grdShort.DisplayLayout.MaxRowScrollRegions = 1;
            appearance42.BackColor = System.Drawing.Color.LightSlateGray;
            appearance42.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance42.BorderColor = System.Drawing.Color.DimGray;
            appearance42.FontData.BoldAsString = "True";
            appearance42.ForeColor = System.Drawing.Color.White;
            this.grdShort.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.grdShort.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdShort.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdShort.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdShort.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdShort.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdShort.DisplayLayout.Override.CellPadding = 0;
            this.grdShort.DisplayLayout.Override.CellSpacing = 0;
            appearance43.FontData.Name = "Tahoma";
            appearance43.FontData.SizeInPoints = 8F;
            appearance43.TextHAlignAsString = "Center";
            this.grdShort.DisplayLayout.Override.HeaderAppearance = appearance43;
            this.grdShort.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdShort.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance44.ForeColor = System.Drawing.Color.White;
            appearance44.TextHAlignAsString = "Right";
            appearance44.TextVAlignAsString = "Middle";
            this.grdShort.DisplayLayout.Override.RowAlternateAppearance = appearance44;
            appearance45.BackColor = System.Drawing.Color.Black;
            appearance45.ForeColor = System.Drawing.Color.White;
            appearance45.TextHAlignAsString = "Right";
            appearance45.TextVAlignAsString = "Middle";
            this.grdShort.DisplayLayout.Override.RowAppearance = appearance45;
            this.grdShort.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdShort.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdShort.DisplayLayout.Override.RowSelectorWidth = 25;
            this.grdShort.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance46.BackColor = System.Drawing.Color.Transparent;
            appearance46.BorderColor = System.Drawing.Color.Transparent;
            appearance46.FontData.BoldAsString = "True";
            this.grdShort.DisplayLayout.Override.SelectedRowAppearance = appearance46;
            this.grdShort.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdShort.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdShort.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdShort.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdShort.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdShort.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdShort.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance47.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdShort.DisplayLayout.Override.TemplateAddRowAppearance = appearance47;
            this.grdShort.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdShort.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdShort.DisplayLayout.UseFixedHeaders = true;
            this.grdShort.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdShort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdShort.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdShort.Location = new System.Drawing.Point(0, 0);
            this.grdShort.Name = "grdShort";
            this.grdShort.Size = new System.Drawing.Size(528, 210);
            this.grdShort.SyncWithCurrencyManager = false;
            this.grdShort.TabIndex = 21;
            this.grdShort.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdShort_InitializeLayout);
            this.grdShort.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdShort_InitializeRow);
            this.grdShort.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdShort_AfterRowFilterChanged);
            this.grdShort.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdShort_BeforeCustomRowFilterDialog);
            this.grdShort.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdShort_BeforeColumnChooserDisplayed);
            this.grdShort.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdShort_DragDrop);
            this.grdShort.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdShort_DragEnter);
            this.grdShort.DragOver += new System.Windows.Forms.DragEventHandler(this.grdShort_DragOver);
            this.grdShort.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdShort_MouseDown);
            this.grdShort.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdShort_MouseMove);
            this.grdShort.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdShort_MouseUp);
            this.grdShort.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.grpBoxNetPosition);
            this.splitContainer3.Size = new System.Drawing.Size(1108, 257);
            this.splitContainer3.SplitterDistance = 231;
            this.splitContainer3.SplitterWidth = 1;
            this.splitContainer3.TabIndex = 0;
            // 
            // grpBoxNetPosition
            // 
            this.grpBoxNetPosition.Controls.Add(this.ultraExpandableGroupBoxPanel8);
            this.grpBoxNetPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxNetPosition.ExpandedSize = new System.Drawing.Size(1108, 231);
            this.grpBoxNetPosition.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.grpBoxNetPosition.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.grpBoxNetPosition.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grpBoxNetPosition.Location = new System.Drawing.Point(0, 0);
            this.grpBoxNetPosition.Name = "grpBoxNetPosition";
            this.grpBoxNetPosition.Size = new System.Drawing.Size(1108, 231);
            this.grpBoxNetPosition.TabIndex = 83;
            this.grpBoxNetPosition.Text = "Closing History";
            this.grpBoxNetPosition.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grpBoxNetPosition.ExpandedStateChanged += new System.EventHandler(this.grpBoxNetPosition_ExpandedStateChanged);
            // 
            // ultraExpandableGroupBoxPanel8
            // 
            this.ultraExpandableGroupBoxPanel8.Controls.Add(this.grdNetPosition);
            this.ultraExpandableGroupBoxPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel8.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel8.Name = "ultraExpandableGroupBoxPanel8";
            this.ultraExpandableGroupBoxPanel8.Size = new System.Drawing.Size(1102, 209);
            this.ultraExpandableGroupBoxPanel8.TabIndex = 0;
            // 
            // grdNetPosition
            // 
            this.grdNetPosition.ContextMenuStrip = this.cntxtMenuStripClosedPosition;
            appearance48.BackColor = System.Drawing.Color.Black;
            this.grdNetPosition.DisplayLayout.Appearance = appearance48;
            this.grdNetPosition.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdNetPosition.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdNetPosition.DisplayLayout.GroupByBox.Hidden = true;
            this.grdNetPosition.DisplayLayout.GroupByBox.Style = Infragistics.Win.UltraWinGrid.GroupByBoxStyle.Compact;
            this.grdNetPosition.DisplayLayout.MaxBandDepth = 6;
            this.grdNetPosition.DisplayLayout.MaxColScrollRegions = 1;
            this.grdNetPosition.DisplayLayout.MaxRowScrollRegions = 1;
            appearance49.BackColor = System.Drawing.Color.LightSlateGray;
            appearance49.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance49.BorderColor = System.Drawing.Color.DimGray;
            appearance49.FontData.BoldAsString = "True";
            appearance49.ForeColor = System.Drawing.Color.White;
            this.grdNetPosition.DisplayLayout.Override.ActiveRowAppearance = appearance49;
            this.grdNetPosition.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdNetPosition.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdNetPosition.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdNetPosition.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdNetPosition.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdNetPosition.DisplayLayout.Override.CellPadding = 0;
            this.grdNetPosition.DisplayLayout.Override.CellSpacing = 0;
            appearance50.FontData.Name = "Tahoma";
            appearance50.FontData.SizeInPoints = 8F;
            appearance50.TextHAlignAsString = "Center";
            this.grdNetPosition.DisplayLayout.Override.HeaderAppearance = appearance50;
            this.grdNetPosition.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdNetPosition.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance51.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance51.ForeColor = System.Drawing.Color.White;
            appearance51.TextHAlignAsString = "Right";
            appearance51.TextVAlignAsString = "Middle";
            this.grdNetPosition.DisplayLayout.Override.RowAlternateAppearance = appearance51;
            appearance52.BackColor = System.Drawing.Color.Black;
            appearance52.ForeColor = System.Drawing.Color.White;
            appearance52.TextHAlignAsString = "Right";
            appearance52.TextVAlignAsString = "Middle";
            this.grdNetPosition.DisplayLayout.Override.RowAppearance = appearance52;
            this.grdNetPosition.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdNetPosition.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdNetPosition.DisplayLayout.Override.RowSelectorWidth = 25;
            appearance53.BackColor = System.Drawing.Color.Transparent;
            appearance53.BorderColor = System.Drawing.Color.Transparent;
            appearance53.FontData.BoldAsString = "True";
            this.grdNetPosition.DisplayLayout.Override.SelectedRowAppearance = appearance53;
            this.grdNetPosition.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdNetPosition.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdNetPosition.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdNetPosition.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdNetPosition.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdNetPosition.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdNetPosition.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance54.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdNetPosition.DisplayLayout.Override.TemplateAddRowAppearance = appearance54;
            this.grdNetPosition.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdNetPosition.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdNetPosition.DisplayLayout.UseFixedHeaders = true;
            this.grdNetPosition.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdNetPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdNetPosition.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdNetPosition.Location = new System.Drawing.Point(0, 0);
            this.grdNetPosition.Name = "grdNetPosition";
            this.grdNetPosition.Size = new System.Drawing.Size(1102, 209);
            this.grdNetPosition.SyncWithCurrencyManager = false;
            this.grdNetPosition.TabIndex = 23;
            this.grdNetPosition.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdNetPosition_InitializeLayout);
            this.grdNetPosition.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdNetPosition_InitializeRow);
            this.grdNetPosition.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdNetPosition_BeforeCustomRowFilterDialog);
            this.grdNetPosition.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdNetPosition_BeforeColumnChooserDisplayed);
            this.grdNetPosition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdNetPosition_MouseDown);
            this.grdNetPosition.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdNetPosition.AfterRowFilterChanged+=grdNetPosition_AfterRowFilterChanged;
            // 
            // cntxtMenuStripSplit
            // 
            this.cntxtMenuStripSplit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitItem1,
            this.modifyTaxlotToolStripMenuItem,
            this.undoSplitToolStripMenuItem});
            this.cntxtMenuStripSplit.Name = "cntxtMenuStripNetPositionsGrid";
            this.cntxtMenuStripSplit.Size = new System.Drawing.Size(148, 70);
            // 
            // toolStripSplitItem1
            // 
            this.toolStripSplitItem1.Name = "toolStripSplitItem1";
            this.toolStripSplitItem1.Size = new System.Drawing.Size(147, 22);
            this.toolStripSplitItem1.Text = "Split Taxlot";
            // 
            // modifyTaxlotToolStripMenuItem
            // 
            this.modifyTaxlotToolStripMenuItem.Name = "modifyTaxlotToolStripMenuItem";
            this.modifyTaxlotToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.modifyTaxlotToolStripMenuItem.Text = "Modify Taxlot";
            //this.modifyTaxlotToolStripMenuItem.Click += new System.EventHandler(this.modifyTaxlotToolStripMenuItem_Click);
            // 
            // undoSplitToolStripMenuItem
            // 
            this.undoSplitToolStripMenuItem.Name = "undoSplitToolStripMenuItem";
            this.undoSplitToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.undoSplitToolStripMenuItem.Text = "Undo";
            // 
            // ultraExpandableGroupBoxPanel7
            // 
            this.ultraExpandableGroupBoxPanel7.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel7.Name = "ultraExpandableGroupBoxPanel7";
            this.ultraExpandableGroupBoxPanel7.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel7.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Size = new System.Drawing.Size(150, 100);
            this.splitContainer4.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel3
            // 
            this.ultraExpandableGroupBoxPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel3.Name = "ultraExpandableGroupBoxPanel3";
            this.ultraExpandableGroupBoxPanel3.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel3.TabIndex = 0;
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpandableGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(869, 365);
            this.ultraExpandableGroupBox1.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.ultraExpandableGroupBox1.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.ultraExpandableGroupBox1.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(869, 365);
            this.ultraExpandableGroupBox1.TabIndex = 82;
            this.ultraExpandableGroupBox1.Text = "Allocated Trades and Open Positions";
            this.ultraExpandableGroupBox1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.splitContainer1);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 21);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(863, 341);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ultraExpandableGroupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraExpandableGroupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(863, 341);
            this.splitContainer1.SplitterDistance = 404;
            this.splitContainer1.TabIndex = 82;
            // 
            // ultraExpandableGroupBox2
            // 
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(0, 0);
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox2.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 17);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(194, 165);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // ultraExpandableGroupBox3
            // 
            this.ultraExpandableGroupBox3.Controls.Add(this.ultraExpandableGroupBoxPanel6);
            this.ultraExpandableGroupBox3.ExpandedSize = new System.Drawing.Size(0, 0);
            this.ultraExpandableGroupBox3.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox3.Name = "ultraExpandableGroupBox3";
            this.ultraExpandableGroupBox3.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox3.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel6
            // 
            this.ultraExpandableGroupBoxPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel6.Location = new System.Drawing.Point(3, 17);
            this.ultraExpandableGroupBoxPanel6.Name = "ultraExpandableGroupBoxPanel6";
            this.ultraExpandableGroupBoxPanel6.Size = new System.Drawing.Size(194, 165);
            this.ultraExpandableGroupBoxPanel6.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel5
            // 
            this.ultraExpandableGroupBoxPanel5.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel5.Name = "ultraExpandableGroupBoxPanel5";
            this.ultraExpandableGroupBoxPanel5.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel5.TabIndex = 0;
            // 
            // ultraExpandableGroupBox4
            // 
            this.ultraExpandableGroupBox4.Controls.Add(this.ultraExpandableGroupBoxPanel11);
            this.ultraExpandableGroupBox4.ExpandedSize = new System.Drawing.Size(0, 0);
            this.ultraExpandableGroupBox4.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox4.Name = "ultraExpandableGroupBox4";
            this.ultraExpandableGroupBox4.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox4.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel11
            // 
            this.ultraExpandableGroupBoxPanel11.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel11.Name = "ultraExpandableGroupBoxPanel11";
            this.ultraExpandableGroupBoxPanel11.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel11.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.panel2);
            this.splitContainer5.Panel1.Controls.Add(this.ultraExpandableGroupBox5);
            this.splitContainer5.Size = new System.Drawing.Size(150, 100);
            this.splitContainer5.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ultraButton1);
            this.panel2.Controls.Add(this.ultraButton2);
            this.panel2.Controls.Add(this.ultraLabel2);
            this.panel2.Controls.Add(this.ultraButton3);
            this.panel2.Controls.Add(this.ultraCombo1);
            this.panel2.Controls.Add(this.ultraCombo2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(148, 38);
            this.panel2.TabIndex = 101;
            // 
            // ultraButton1
            // 
            this.ultraButton1.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton1.Location = new System.Drawing.Point(311, 3);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.ShowFocusRect = false;
            this.ultraButton1.ShowOutline = false;
            this.ultraButton1.Size = new System.Drawing.Size(79, 23);
            this.ultraButton1.TabIndex = 95;
            // 
            // ultraButton2
            // 
            appearance55.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_cancel;
            this.ultraButton2.Appearance = appearance55;
            this.ultraButton2.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton2.Location = new System.Drawing.Point(482, 3);
            this.ultraButton2.Margin = new System.Windows.Forms.Padding(4);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.ShowFocusRect = false;
            this.ultraButton2.ShowOutline = false;
            this.ultraButton2.Size = new System.Drawing.Size(79, 23);
            this.ultraButton2.TabIndex = 96;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(3, 6);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraLabel2.Size = new System.Drawing.Size(84, 21);
            this.ultraLabel2.TabIndex = 99;
            this.ultraLabel2.Text = "Run Closing";
            // 
            // ultraButton3
            // 
            appearance56.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_save;
            this.ultraButton3.Appearance = appearance56;
            this.ultraButton3.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton3.Location = new System.Drawing.Point(399, 3);
            this.ultraButton3.Name = "ultraButton3";
            this.ultraButton3.ShowFocusRect = false;
            this.ultraButton3.ShowOutline = false;
            this.ultraButton3.Size = new System.Drawing.Size(74, 23);
            this.ultraButton3.TabIndex = 94;
            this.ultraButton3.Text = "Save";
            // 
            // ultraCombo1
            // 
            this.ultraCombo1.Location = new System.Drawing.Point(202, 5);
            this.ultraCombo1.Name = "ultraCombo1";
            this.ultraCombo1.Size = new System.Drawing.Size(100, 22);
            this.ultraCombo1.TabIndex = 98;
            // 
            // ultraCombo2
            // 
            this.ultraCombo2.Location = new System.Drawing.Point(93, 5);
            this.ultraCombo2.Name = "ultraCombo2";
            this.ultraCombo2.Size = new System.Drawing.Size(100, 22);
            this.ultraCombo2.TabIndex = 97;
            // 
            // ultraExpandableGroupBox5
            // 
            this.ultraExpandableGroupBox5.Controls.Add(this.ultraExpandableGroupBoxPanel13);
            this.ultraExpandableGroupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraExpandableGroupBox5.ExpandedSize = new System.Drawing.Size(148, 359);
            this.ultraExpandableGroupBox5.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.ultraExpandableGroupBox5.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.ultraExpandableGroupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraExpandableGroupBox5.Location = new System.Drawing.Point(0, -311);
            this.ultraExpandableGroupBox5.Name = "ultraExpandableGroupBox5";
            this.ultraExpandableGroupBox5.Size = new System.Drawing.Size(148, 359);
            this.ultraExpandableGroupBox5.TabIndex = 100;
            this.ultraExpandableGroupBox5.Text = "Open Taxlots";
            // 
            // ultraExpandableGroupBoxPanel13
            // 
            this.ultraExpandableGroupBoxPanel13.Controls.Add(this.splitContainer7);
            this.ultraExpandableGroupBoxPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel13.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel13.Name = "ultraExpandableGroupBoxPanel13";
            this.ultraExpandableGroupBoxPanel13.Size = new System.Drawing.Size(142, 337);
            this.ultraExpandableGroupBoxPanel13.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.ultraExpandableGroupBox6);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.ultraExpandableGroupBox7);
            this.splitContainer7.Size = new System.Drawing.Size(142, 337);
            this.splitContainer7.SplitterDistance = 68;
            this.splitContainer7.TabIndex = 0;
            // 
            // ultraExpandableGroupBox6
            // 
            this.ultraExpandableGroupBox6.Controls.Add(this.ultraExpandableGroupBoxPanel14);
            this.ultraExpandableGroupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox6.ExpandedSize = new System.Drawing.Size(68, 337);
            this.ultraExpandableGroupBox6.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.RightArrow1;
            this.ultraExpandableGroupBox6.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.LeftArrow2;
            this.ultraExpandableGroupBox6.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.RightOnBorder;
            this.ultraExpandableGroupBox6.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox6.Name = "ultraExpandableGroupBox6";
            this.ultraExpandableGroupBox6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraExpandableGroupBox6.Size = new System.Drawing.Size(68, 337);
            this.ultraExpandableGroupBox6.TabIndex = 0;
            this.ultraExpandableGroupBox6.Text = "Long Position";
            // 
            // ultraExpandableGroupBoxPanel14
            // 
            this.ultraExpandableGroupBoxPanel14.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel14.Name = "ultraExpandableGroupBoxPanel14";
            this.ultraExpandableGroupBoxPanel14.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel14.TabIndex = 0;
            // 
            // ultraExpandableGroupBox7
            // 
            this.ultraExpandableGroupBox7.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Far;
            this.ultraExpandableGroupBox7.Controls.Add(this.ultraExpandableGroupBoxPanel15);
            this.ultraExpandableGroupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox7.ExpandedSize = new System.Drawing.Size(70, 337);
            this.ultraExpandableGroupBox7.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.RightArrow1;
            this.ultraExpandableGroupBox7.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.LeftArrow2;
            this.ultraExpandableGroupBox7.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.LeftOutsideBorder;
            this.ultraExpandableGroupBox7.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox7.Name = "ultraExpandableGroupBox7";
            this.ultraExpandableGroupBox7.Size = new System.Drawing.Size(70, 337);
            this.ultraExpandableGroupBox7.TabIndex = 0;
            this.ultraExpandableGroupBox7.Text = "ShortPosition";
            // 
            // ultraExpandableGroupBoxPanel15
            // 
            this.ultraExpandableGroupBoxPanel15.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel15.Name = "ultraExpandableGroupBoxPanel15";
            this.ultraExpandableGroupBoxPanel15.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel15.TabIndex = 0;
            // 
            // splitContainer8
            // 
            this.splitContainer8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer8.Size = new System.Drawing.Size(150, 100);
            this.splitContainer8.TabIndex = 0;
            // 
            // ultraExpandableGroupBox8
            // 
            this.ultraExpandableGroupBox8.Controls.Add(this.ultraExpandableGroupBoxPanel16);
            this.ultraExpandableGroupBox8.ExpandedSize = new System.Drawing.Size(0, 0);
            this.ultraExpandableGroupBox8.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox8.Name = "ultraExpandableGroupBox8";
            this.ultraExpandableGroupBox8.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox8.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel16
            // 
            this.ultraExpandableGroupBoxPanel16.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel16.Name = "ultraExpandableGroupBoxPanel16";
            this.ultraExpandableGroupBoxPanel16.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel16.TabIndex = 0;
            // 
            // splitContainer9
            // 
            this.splitContainer9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.Location = new System.Drawing.Point(0, 0);
            this.splitContainer9.Name = "splitContainer9";
            this.splitContainer9.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.panel3);
            this.splitContainer9.Panel1.Controls.Add(this.ultraExpandableGroupBox9);
            this.splitContainer9.Size = new System.Drawing.Size(150, 100);
            this.splitContainer9.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ultraButton4);
            this.panel3.Controls.Add(this.ultraButton5);
            this.panel3.Controls.Add(this.ultraLabel3);
            this.panel3.Controls.Add(this.ultraButton6);
            this.panel3.Controls.Add(this.ultraCombo3);
            this.panel3.Controls.Add(this.ultraCombo4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(148, 38);
            this.panel3.TabIndex = 101;
            // 
            // ultraButton4
            // 
            this.ultraButton4.Location = new System.Drawing.Point(0, 0);
            this.ultraButton4.Name = "ultraButton4";
            this.ultraButton4.Size = new System.Drawing.Size(75, 23);
            this.ultraButton4.TabIndex = 0;
            // 
            // ultraButton5
            // 
            this.ultraButton5.Location = new System.Drawing.Point(0, 0);
            this.ultraButton5.Name = "ultraButton5";
            this.ultraButton5.Size = new System.Drawing.Size(75, 23);
            this.ultraButton5.TabIndex = 1;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(3, 6);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraLabel3.Size = new System.Drawing.Size(84, 21);
            this.ultraLabel3.TabIndex = 99;
            this.ultraLabel3.Text = "Run Closing";
            // 
            // ultraButton6
            // 
            this.ultraButton6.Appearance = appearance21;
            this.ultraButton6.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton6.Location = new System.Drawing.Point(399, 3);
            this.ultraButton6.Name = "ultraButton6";
            this.ultraButton6.ShowFocusRect = false;
            this.ultraButton6.ShowOutline = false;
            this.ultraButton6.Size = new System.Drawing.Size(74, 23);
            this.ultraButton6.TabIndex = 94;
            this.ultraButton6.Text = "Save";
            // 
            // ultraCombo3
            // 
            this.ultraCombo3.Location = new System.Drawing.Point(202, 5);
            this.ultraCombo3.Name = "ultraCombo3";
            this.ultraCombo3.Size = new System.Drawing.Size(100, 22);
            this.ultraCombo3.TabIndex = 98;
            // 
            // ultraCombo4
            // 
            this.ultraCombo4.Location = new System.Drawing.Point(93, 5);
            this.ultraCombo4.Name = "ultraCombo4";
            this.ultraCombo4.Size = new System.Drawing.Size(100, 22);
            this.ultraCombo4.TabIndex = 97;
            // 
            // ultraExpandableGroupBox9
            // 
            this.ultraExpandableGroupBox9.Controls.Add(this.ultraExpandableGroupBoxPanel17);
            this.ultraExpandableGroupBox9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraExpandableGroupBox9.ExpandedSize = new System.Drawing.Size(148, 359);
            this.ultraExpandableGroupBox9.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.ultraExpandableGroupBox9.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.ultraExpandableGroupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraExpandableGroupBox9.Location = new System.Drawing.Point(0, -311);
            this.ultraExpandableGroupBox9.Name = "ultraExpandableGroupBox9";
            this.ultraExpandableGroupBox9.Size = new System.Drawing.Size(148, 359);
            this.ultraExpandableGroupBox9.TabIndex = 100;
            this.ultraExpandableGroupBox9.Text = "Open Taxlots";
            // 
            // ultraExpandableGroupBoxPanel17
            // 
            this.ultraExpandableGroupBoxPanel17.Controls.Add(this.splitContainer10);
            this.ultraExpandableGroupBoxPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel17.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel17.Name = "ultraExpandableGroupBoxPanel17";
            this.ultraExpandableGroupBoxPanel17.Size = new System.Drawing.Size(142, 337);
            this.ultraExpandableGroupBoxPanel17.TabIndex = 0;
            // 
            // splitContainer10
            // 
            this.splitContainer10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer10.Location = new System.Drawing.Point(0, 0);
            this.splitContainer10.Name = "splitContainer10";
            // 
            // splitContainer10.Panel1
            // 
            this.splitContainer10.Panel1.Controls.Add(this.ultraExpandableGroupBox10);
            this.splitContainer10.Size = new System.Drawing.Size(142, 337);
            this.splitContainer10.SplitterDistance = 47;
            this.splitContainer10.TabIndex = 0;
            // 
            // ultraExpandableGroupBox10
            // 
            this.ultraExpandableGroupBox10.Controls.Add(this.ultraExpandableGroupBoxPanel18);
            this.ultraExpandableGroupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox10.ExpandedSize = new System.Drawing.Size(47, 337);
            this.ultraExpandableGroupBox10.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.RightArrow1;
            this.ultraExpandableGroupBox10.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.LeftArrow2;
            this.ultraExpandableGroupBox10.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.RightOnBorder;
            this.ultraExpandableGroupBox10.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox10.Name = "ultraExpandableGroupBox10";
            this.ultraExpandableGroupBox10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraExpandableGroupBox10.Size = new System.Drawing.Size(47, 337);
            this.ultraExpandableGroupBox10.TabIndex = 0;
            this.ultraExpandableGroupBox10.Text = "Long Position";
            // 
            // ultraExpandableGroupBoxPanel18
            // 
            this.ultraExpandableGroupBoxPanel18.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel18.Name = "ultraExpandableGroupBoxPanel18";
            this.ultraExpandableGroupBoxPanel18.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel18.TabIndex = 0;
            // 
            // splitContainer11
            // 
            this.splitContainer11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer11.Location = new System.Drawing.Point(0, 0);
            this.splitContainer11.Name = "splitContainer11";
            this.splitContainer11.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer11.Size = new System.Drawing.Size(150, 100);
            this.splitContainer11.TabIndex = 0;
            // 
            // ultraExpandableGroupBox12
            // 
            this.ultraExpandableGroupBox12.Controls.Add(this.ultraExpandableGroupBoxPanel20);
            this.ultraExpandableGroupBox12.ExpandedSize = new System.Drawing.Size(0, 0);
            this.ultraExpandableGroupBox12.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox12.Name = "ultraExpandableGroupBox12";
            this.ultraExpandableGroupBox12.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox12.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel20
            // 
            this.ultraExpandableGroupBoxPanel20.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel20.Name = "ultraExpandableGroupBoxPanel20";
            this.ultraExpandableGroupBoxPanel20.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel20.TabIndex = 0;
            // 
            // splitContainer12
            // 
            this.splitContainer12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer12.Location = new System.Drawing.Point(0, 0);
            this.splitContainer12.Name = "splitContainer12";
            this.splitContainer12.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer12.Panel1
            // 
            this.splitContainer12.Panel1.Controls.Add(this.panel4);
            this.splitContainer12.Panel1.Controls.Add(this.ultraExpandableGroupBox13);
            this.splitContainer12.Size = new System.Drawing.Size(150, 100);
            this.splitContainer12.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ultraButton7);
            this.panel4.Controls.Add(this.ultraButton8);
            this.panel4.Controls.Add(this.ultraLabel4);
            this.panel4.Controls.Add(this.ultraButton9);
            this.panel4.Controls.Add(this.ultraCombo5);
            this.panel4.Controls.Add(this.ultraCombo6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(148, 38);
            this.panel4.TabIndex = 101;
            // 
            // ultraButton7
            // 
            this.ultraButton7.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton7.Location = new System.Drawing.Point(311, 3);
            this.ultraButton7.Name = "ultraButton7";
            this.ultraButton7.ShowFocusRect = false;
            this.ultraButton7.ShowOutline = false;
            this.ultraButton7.Size = new System.Drawing.Size(79, 23);
            this.ultraButton7.TabIndex = 95;
            // 
            // ultraButton8
            // 
            appearance57.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_cancel;
            this.ultraButton8.Appearance = appearance57;
            this.ultraButton8.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton8.Location = new System.Drawing.Point(482, 3);
            this.ultraButton8.Margin = new System.Windows.Forms.Padding(4);
            this.ultraButton8.Name = "ultraButton8";
            this.ultraButton8.ShowFocusRect = false;
            this.ultraButton8.ShowOutline = false;
            this.ultraButton8.Size = new System.Drawing.Size(79, 23);
            this.ultraButton8.TabIndex = 96;
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(3, 6);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraLabel4.Size = new System.Drawing.Size(84, 21);
            this.ultraLabel4.TabIndex = 99;
            this.ultraLabel4.Text = "Run Closing";
            // 
            // ultraButton9
            // 
            appearance58.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_save;
            this.ultraButton9.Appearance = appearance58;
            this.ultraButton9.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton9.Location = new System.Drawing.Point(399, 3);
            this.ultraButton9.Name = "ultraButton9";
            this.ultraButton9.ShowFocusRect = false;
            this.ultraButton9.ShowOutline = false;
            this.ultraButton9.Size = new System.Drawing.Size(74, 23);
            this.ultraButton9.TabIndex = 94;
            this.ultraButton9.Text = "Save";
            // 
            // ultraCombo5
            // 
            this.ultraCombo5.Location = new System.Drawing.Point(202, 5);
            this.ultraCombo5.Name = "ultraCombo5";
            this.ultraCombo5.Size = new System.Drawing.Size(100, 22);
            this.ultraCombo5.TabIndex = 98;
            // 
            // ultraCombo6
            // 
            this.ultraCombo6.Location = new System.Drawing.Point(93, 5);
            this.ultraCombo6.Name = "ultraCombo6";
            this.ultraCombo6.Size = new System.Drawing.Size(100, 22);
            this.ultraCombo6.TabIndex = 97;
            // 
            // ultraExpandableGroupBox13
            // 
            this.ultraExpandableGroupBox13.Controls.Add(this.ultraExpandableGroupBoxPanel21);
            this.ultraExpandableGroupBox13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraExpandableGroupBox13.ExpandedSize = new System.Drawing.Size(148, 359);
            this.ultraExpandableGroupBox13.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.ultraExpandableGroupBox13.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.ultraExpandableGroupBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraExpandableGroupBox13.Location = new System.Drawing.Point(0, -311);
            this.ultraExpandableGroupBox13.Name = "ultraExpandableGroupBox13";
            this.ultraExpandableGroupBox13.Size = new System.Drawing.Size(148, 359);
            this.ultraExpandableGroupBox13.TabIndex = 100;
            this.ultraExpandableGroupBox13.Text = "Open Taxlots";
            // 
            // ultraExpandableGroupBoxPanel21
            // 
            this.ultraExpandableGroupBoxPanel21.Controls.Add(this.splitContainer13);
            this.ultraExpandableGroupBoxPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel21.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel21.Name = "ultraExpandableGroupBoxPanel21";
            this.ultraExpandableGroupBoxPanel21.Size = new System.Drawing.Size(142, 337);
            this.ultraExpandableGroupBoxPanel21.TabIndex = 0;
            // 
            // splitContainer13
            // 
            this.splitContainer13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer13.Location = new System.Drawing.Point(0, 0);
            this.splitContainer13.Name = "splitContainer13";
            // 
            // splitContainer13.Panel1
            // 
            this.splitContainer13.Panel1.Controls.Add(this.ultraExpandableGroupBox14);
            // 
            // splitContainer13.Panel2
            // 
            this.splitContainer13.Panel2.Controls.Add(this.ultraExpandableGroupBox15);
            this.splitContainer13.Size = new System.Drawing.Size(142, 337);
            this.splitContainer13.SplitterDistance = 68;
            this.splitContainer13.TabIndex = 0;
            // 
            // ultraExpandableGroupBox14
            // 
            this.ultraExpandableGroupBox14.Controls.Add(this.ultraExpandableGroupBoxPanel22);
            this.ultraExpandableGroupBox14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox14.ExpandedSize = new System.Drawing.Size(68, 337);
            this.ultraExpandableGroupBox14.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.RightArrow1;
            this.ultraExpandableGroupBox14.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.LeftArrow2;
            this.ultraExpandableGroupBox14.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.RightOnBorder;
            this.ultraExpandableGroupBox14.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox14.Name = "ultraExpandableGroupBox14";
            this.ultraExpandableGroupBox14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraExpandableGroupBox14.Size = new System.Drawing.Size(68, 337);
            this.ultraExpandableGroupBox14.TabIndex = 0;
            this.ultraExpandableGroupBox14.Text = "Long Position";
            // 
            // ultraExpandableGroupBoxPanel22
            // 
            this.ultraExpandableGroupBoxPanel22.Controls.Add(this.ultraGrid5);
            this.ultraExpandableGroupBoxPanel22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel22.Location = new System.Drawing.Point(3, 3);
            this.ultraExpandableGroupBoxPanel22.Name = "ultraExpandableGroupBoxPanel22";
            this.ultraExpandableGroupBoxPanel22.Size = new System.Drawing.Size(46, 331);
            this.ultraExpandableGroupBoxPanel22.TabIndex = 0;
            // 
            // ultraGrid5
            // 
            this.ultraGrid5.Location = new System.Drawing.Point(0, 0);
            this.ultraGrid5.Name = "ultraGrid5";
            this.ultraGrid5.Size = new System.Drawing.Size(550, 80);
            this.ultraGrid5.TabIndex = 0;
            // 
            // ultraExpandableGroupBox15
            // 
            this.ultraExpandableGroupBox15.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Far;
            this.ultraExpandableGroupBox15.Controls.Add(this.ultraExpandableGroupBoxPanel23);
            this.ultraExpandableGroupBox15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox15.ExpandedSize = new System.Drawing.Size(70, 337);
            this.ultraExpandableGroupBox15.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.RightArrow1;
            this.ultraExpandableGroupBox15.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.LeftArrow2;
            this.ultraExpandableGroupBox15.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.LeftOutsideBorder;
            this.ultraExpandableGroupBox15.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox15.Name = "ultraExpandableGroupBox15";
            this.ultraExpandableGroupBox15.Size = new System.Drawing.Size(70, 337);
            this.ultraExpandableGroupBox15.TabIndex = 0;
            this.ultraExpandableGroupBox15.Text = "ShortPosition";
            // 
            // ultraExpandableGroupBoxPanel23
            // 
            this.ultraExpandableGroupBoxPanel23.Controls.Add(this.ultraGrid6);
            this.ultraExpandableGroupBoxPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel23.Location = new System.Drawing.Point(24, 3);
            this.ultraExpandableGroupBoxPanel23.Name = "ultraExpandableGroupBoxPanel23";
            this.ultraExpandableGroupBoxPanel23.Size = new System.Drawing.Size(43, 331);
            this.ultraExpandableGroupBoxPanel23.TabIndex = 0;
            // 
            // ultraGrid6
            // 
            appearance59.BackColor = System.Drawing.SystemColors.Window;
            appearance59.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGrid6.DisplayLayout.Appearance = appearance59;
            this.ultraGrid6.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid6.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance60.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance60.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance60.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance60.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGrid6.DisplayLayout.GroupByBox.Appearance = appearance60;
            appearance61.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGrid6.DisplayLayout.GroupByBox.BandLabelAppearance = appearance61;
            this.ultraGrid6.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid6.DisplayLayout.GroupByBox.Hidden = true;
            appearance62.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance62.BackColor2 = System.Drawing.SystemColors.Control;
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance62.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGrid6.DisplayLayout.GroupByBox.PromptAppearance = appearance62;
            this.ultraGrid6.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGrid6.DisplayLayout.MaxRowScrollRegions = 1;
            appearance63.BackColor = System.Drawing.SystemColors.Window;
            appearance63.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGrid6.DisplayLayout.Override.ActiveCellAppearance = appearance63;
            appearance64.BackColor = System.Drawing.SystemColors.Highlight;
            appearance64.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGrid6.DisplayLayout.Override.ActiveRowAppearance = appearance64;
            this.ultraGrid6.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid6.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGrid6.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance65.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGrid6.DisplayLayout.Override.CardAreaAppearance = appearance65;
            appearance66.BorderColor = System.Drawing.Color.Silver;
            appearance66.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGrid6.DisplayLayout.Override.CellAppearance = appearance66;
            this.ultraGrid6.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGrid6.DisplayLayout.Override.CellPadding = 0;
            appearance67.BackColor = System.Drawing.SystemColors.Control;
            appearance67.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance67.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance67.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance67.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGrid6.DisplayLayout.Override.GroupByRowAppearance = appearance67;
            appearance68.TextHAlignAsString = "Left";
            this.ultraGrid6.DisplayLayout.Override.HeaderAppearance = appearance68;
            this.ultraGrid6.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGrid6.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance69.BackColor = System.Drawing.SystemColors.Window;
            appearance69.BorderColor = System.Drawing.Color.Silver;
            this.ultraGrid6.DisplayLayout.Override.RowAppearance = appearance69;
            this.ultraGrid6.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance70.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGrid6.DisplayLayout.Override.TemplateAddRowAppearance = appearance70;
            this.ultraGrid6.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGrid6.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGrid6.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraGrid6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGrid6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraGrid6.Location = new System.Drawing.Point(0, 0);
            this.ultraGrid6.Name = "ultraGrid6";
            this.ultraGrid6.Size = new System.Drawing.Size(43, 331);
            this.ultraGrid6.TabIndex = 87;
            this.ultraGrid6.Text = "ultraGrid1";
            // 
            // splitContainer14
            // 
            this.splitContainer14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer14.Location = new System.Drawing.Point(0, 0);
            this.splitContainer14.Name = "splitContainer14";
            this.splitContainer14.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer14.Size = new System.Drawing.Size(150, 100);
            this.splitContainer14.TabIndex = 0;
            // 
            // ultraExpandableGroupBox16
            // 
            this.ultraExpandableGroupBox16.Controls.Add(this.ultraExpandableGroupBoxPanel24);
            this.ultraExpandableGroupBox16.ExpandedSize = new System.Drawing.Size(0, 0);
            this.ultraExpandableGroupBox16.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox16.Name = "ultraExpandableGroupBox16";
            this.ultraExpandableGroupBox16.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox16.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel24
            // 
            this.ultraExpandableGroupBoxPanel24.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel24.Name = "ultraExpandableGroupBoxPanel24";
            this.ultraExpandableGroupBoxPanel24.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel24.TabIndex = 0;
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // chkBxIsAutoCloseStrategy
            // 
            this.chkBxIsAutoCloseStrategy.Location = new System.Drawing.Point(175, 28);
            this.chkBxIsAutoCloseStrategy.Name = "chkBxIsAutoCloseStrategy";
            this.chkBxIsAutoCloseStrategy.Size = new System.Drawing.Size(145, 17);
            this.chkBxIsAutoCloseStrategy.TabIndex = 117;
            this.chkBxIsAutoCloseStrategy.Text = "Auto Close Strategy";
            // 
            // CtrlCloseTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Name = "CtrlCloseTrade";
            this.Size = new System.Drawing.Size(1108, 533);
            this.Load += new System.EventHandler(this.CtrlCloseTrade_Load);
            this.cntxtMenuStripClosedPosition.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer15.Panel1.ResumeLayout(false);
            this.splitContainer15.Panel1.PerformLayout();
            this.splitContainer15.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer15)).EndInit();
            this.splitContainer15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecondarySort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClosingField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxSellWithBTC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxBuyAndBuyToCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSyncFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMethodology)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlgorithm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpOpenPosition)).EndInit();
            this.grpOpenPosition.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel9.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpLongPosition)).EndInit();
            this.grpLongPosition.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdLong)).EndInit();
            this.contextMenuShortLongPosition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpShortPosition)).EndInit();
            this.grpShortPosition.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdShort)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxNetPosition)).EndInit();
            this.grpBoxNetPosition.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdNetPosition)).EndInit();
            this.cntxtMenuStripSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox3)).EndInit();
            this.ultraExpandableGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox4)).EndInit();
            this.ultraExpandableGroupBox4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox5)).EndInit();
            this.ultraExpandableGroupBox5.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel13.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox6)).EndInit();
            this.ultraExpandableGroupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox7)).EndInit();
            this.ultraExpandableGroupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox8)).EndInit();
            this.ultraExpandableGroupBox8.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox9)).EndInit();
            this.ultraExpandableGroupBox9.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel17.ResumeLayout(false);
            this.splitContainer10.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).EndInit();
            this.splitContainer10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox10)).EndInit();
            this.ultraExpandableGroupBox10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).EndInit();
            this.splitContainer11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox12)).EndInit();
            this.ultraExpandableGroupBox12.ResumeLayout(false);
            this.splitContainer12.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).EndInit();
            this.splitContainer12.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox13)).EndInit();
            this.ultraExpandableGroupBox13.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel21.ResumeLayout(false);
            this.splitContainer13.Panel1.ResumeLayout(false);
            this.splitContainer13.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer13)).EndInit();
            this.splitContainer13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox14)).EndInit();
            this.ultraExpandableGroupBox14.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox15)).EndInit();
            this.ultraExpandableGroupBox15.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer14)).EndInit();
            this.splitContainer14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox16)).EndInit();
            this.ultraExpandableGroupBox16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBxIsAutoCloseStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCopyOpeningTradeAttributes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cntxtMenuStripClosedPosition;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel7;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel3;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox3;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel5;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpOpenPosition;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel9;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpBoxNetPosition;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel8;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpLongPosition;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel10;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpShortPosition;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel12;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox4;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel11;
        private System.Windows.Forms.ToolStripMenuItem unwindToolStripMenuItem;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel6;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox9;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel17;
        private System.Windows.Forms.SplitContainer splitContainer10;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox10;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel18;
        private System.Windows.Forms.Panel panel3;
        private Infragistics.Win.Misc.UltraButton ultraButton4;
        private Infragistics.Win.Misc.UltraButton ultraButton5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraButton ultraButton6;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo3;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo4;
        private System.Windows.Forms.SplitContainer splitContainer11;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox12;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel20;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraButton ultraButton3;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo1;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo2;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox5;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel13;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox6;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel14;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox7;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel15;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox8;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel16;
        private System.Windows.Forms.SplitContainer splitContainer15;
        private Infragistics.Win.Misc.UltraButton btnRun;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAlgorithm;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbMethodology;
        private System.Windows.Forms.SplitContainer splitContainer12;
        private System.Windows.Forms.Panel panel4;
        private Infragistics.Win.Misc.UltraButton ultraButton7;
        private Infragistics.Win.Misc.UltraButton ultraButton8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraButton ultraButton9;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo5;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo6;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox13;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel21;
        private System.Windows.Forms.SplitContainer splitContainer13;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox14;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel22;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid5;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox15;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel23;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid6;
        private System.Windows.Forms.SplitContainer splitContainer14;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox16;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel24;
        private PranaUltraGrid grdLong;
        private PranaUltraGrid grdShort;
        private PranaUltraGrid grdNetPosition;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor cbSyncFilter;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxBuyAndBuyToCover;

        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxSellWithBTC;
        private Infragistics.Win.Misc.UltraButton btnClearFilter;
        private System.Windows.Forms.ContextMenuStrip cntxtMenuStripSplit;
        private System.Windows.Forms.ToolStripMenuItem toolStripSplitItem1;
        private System.Windows.Forms.ToolStripMenuItem undoSplitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuShortLongPosition;
        private System.Windows.Forms.ToolStripMenuItem modifyTaxlotToolStripMenuItem;
       
       // private Infragistics.Win.Misc.UltraButton btnSaveACA;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutShortLongtoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelShortLongStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearFilterStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearFilterShortLongStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private Infragistics.Win.Misc.UltraLabel label3;
        private Infragistics.Win.Misc.UltraLabel lblClosingField;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSecondarySort;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClosingField;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBxIsAutoCloseStrategy;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkCopyOpeningTradeAttributes;
    }
}
