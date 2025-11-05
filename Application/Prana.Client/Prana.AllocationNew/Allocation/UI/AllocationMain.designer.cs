using Prana.AllocationNew.Allocation.UI.CostAdjustment;
using Prana.Global;
using Prana.Utilities.UIUtilities;

namespace Prana.AllocationNew
{
    partial class AllocationMain
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
                if (this._headerCheckBoxAllocated != null)
                    this._headerCheckBoxAllocated.Dispose();
                if (this._headerCheckBoxUnallocated != null)
                    this._headerCheckBoxUnallocated.Dispose();
            }
            this.cmbbxdefaults.ValueChanged -= new System.EventHandler(this.cmbbxdefaults_ValueChanged);
            this.cmbbxdefaults = null;
            this.btnReAllocate.Click -= new System.EventHandler(this.btnReAllocate_Click_1);
            this.btnAllocate.Click -= new System.EventHandler(this.btnAllocate_Click_1);
            this.btnClear.Click -= new System.EventHandler(this.btnClear_Click);
            this.btnSaveSwap.Click -= new System.EventHandler(this.btnSaveSwap_Click);
            this.btnSwapClose.Click -= new System.EventHandler(this.btnSwapClose_Click);
            if (grdAllocated != null)
            {
                this.grdAllocated.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAllocated_InitializeLayout);
                this.grdAllocated.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdAllocated_InitializeRow);
                this.grdAllocated.BeforeColumnChooserDisplayed -= new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAllocated_BeforeColumnChooserDisplayed);
                this.grdAllocated.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.grdAllocated_KeyDown);
                this.grdAllocated.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.grdAllocated_MouseClick);
                this.grdAllocated.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.grdAllocated_MouseDown);
            this.grdAllocated.AfterRowFilterChanged -= new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdAllocated_AfterRowFilterChanged);
            }
         
            this.contextMnuUnAllocatedGrid.Popup -= new System.EventHandler(this.contextMnuUnAllocatedGrid_Popup);
            this.mnuUnGroup.Click -= new System.EventHandler(this.mnuUnGroup_Click);
            this.mnuGroup.Click -= new System.EventHandler(this.mnuGroup_Click);
            this.mnuSwapDetailsUnAllocated.Click -= new System.EventHandler(this.mnuSwapDetails_Click);
            this.mnuSaveUnAllocatedColumns.Click -= new System.EventHandler(this.mnuSaveColumns_Click);
            this.mnuSaveAllocatedColumns.Click -= new System.EventHandler(this.mnuSaveAllocatedColumns_Click);
            this.mnuEditUnAllocatedColumns.Click -= new System.EventHandler(this.mnuUnallocateEdit_Click);
            this._defaults = null;
            if (dtToDatePickerAllocation != null)
            {
                dtToDatePickerAllocation.Dispose();
                dtToDatePickerAllocation = null;
            }
            if (dtFromDatePickerAllocation != null)
            {
                dtFromDatePickerAllocation.Dispose();
                dtFromDatePickerAllocation = null;
            }
            if (allocationCalculatorUsrControl1 != null)
            {
                allocationCalculatorUsrControl1.Dispose();
                allocationCalculatorUsrControl1 = null;
            }
            if (_strategyPergentage != null)
            {
                _strategyPergentage = null;
            }
            if (cmbAllocationScheme != null)
            {
                cmbAllocationScheme.Dispose();
                cmbAllocationScheme = null;
            }
            if (btnGetAllocationData != null)
            {
                btnGetAllocationData.Dispose();
                btnGetAllocationData = null;
            }
            if (cmbStrategySearch != null)
            {
                cmbStrategySearch.Dispose();
                cmbStrategySearch = null;
            }
            base.Dispose(disposing);
            _isEventAlive = false;
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
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
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("By Pass Side Check", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("AllocationMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recon Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool(" CalculateAllocationProrataPercent");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("FilterType");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("PrefetchFilter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterStatus_Enabled");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = SnapShotManager.GetInstance().buttonTool;
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Commission");
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Get Data");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Auto Group");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerTool1");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool4 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerTool2");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recon Report");
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterStatus_Enabled");
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("PrefetchFilter");
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("FilterType");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool(" CalculateAllocationProrataPercent");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Snapshot");
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();            
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationMain));
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.splitContainer2 = new Infragistics.Win.Misc.UltraSplitter();
            this.splitContainer2Panel2 = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer3Panel2 = new Infragistics.Win.Misc.UltraPanel();
            this.accountStrategyMapping1 = new Prana.AllocationNew.AccountStrategyMappingUserCtrlNew();
            this.panelReAllocateButtonAccountStrategy = new Infragistics.Win.Misc.UltraPanel();
            this.btnClearAllocated = new Infragistics.Win.Misc.UltraButton();
            this.btnReAllocate = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer3 = new Infragistics.Win.Misc.UltraSplitter();
            this.splitContainer3Panel1 = new Infragistics.Win.Misc.UltraPanel();
            this.panelAllocateFillArea = new Infragistics.Win.Misc.UltraPanel();
            this.ctrlSwapParameters1 = new Prana.ClientCommon.CtrlSwapParameters();
            this.allocationCalculatorUsrControl1 = new Prana.AllocationNew.AllocationCalculatorUsrControl();
            this.accountOnlyUserControl1 = new Prana.AllocationNew.AccountOnlyUserControl();
            this.panelAllocateButtonAccount = new Infragistics.Win.Misc.UltraPanel();
            this.lblStrategySearch = new Infragistics.Win.Misc.UltraLabel();
            this.cmbStrategySearch = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnAllocate = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnSaveSwap = new Infragistics.Win.Misc.UltraButton();
            this.btnSwapClose = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer2Panel1 = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer1Panel2 = new Infragistics.Win.Misc.UltraPanel();
            this.grdAllocated = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.splitContainer1 = new Infragistics.Win.Misc.UltraSplitter();
            this.splitContainer1Panel1 = new Infragistics.Win.Misc.UltraPanel();
            this.grdUnallocated = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlAmendmend1 = new Prana.AllocationNew.CtrlAmendmend();
            this.ctrlCostAdjustment = new Prana.AllocationNew.Allocation.UI.CostAdjustment.CostAdjustmentControlMain();
            this.panelReAllocateFillArea = new Infragistics.Win.Misc.UltraPanel();
            this.contextMnuUnAllocatedGrid = new System.Windows.Forms.ContextMenu();
            this.mnuGroup = new System.Windows.Forms.MenuItem();
            this.mnuUnGroup = new System.Windows.Forms.MenuItem();
            this.mnuSwapDetailsUnAllocated = new System.Windows.Forms.MenuItem();
            this.mnuSwapUnallocated = new System.Windows.Forms.MenuItem();
            this.mnuSaveUnAllocatedColumns = new System.Windows.Forms.MenuItem();
            this.mnuEditUnAllocatedColumns = new System.Windows.Forms.MenuItem();
            this.SMUnallocated = new System.Windows.Forms.MenuItem();
            this.mnuTradeAuditTrailUnallocated = new System.Windows.Forms.MenuItem();
            this.ExpandCollapseUnallocated = new System.Windows.Forms.MenuItem();
            this.mnuTradeAuditTrailAllocated = new System.Windows.Forms.MenuItem();
            this.contextMnuAllocatedGrid = new System.Windows.Forms.ContextMenu();
            this.mnuUnAllocate = new System.Windows.Forms.MenuItem();
            this.mnuSwapAllocated = new System.Windows.Forms.MenuItem();
            this.mnuSwapDetailsAllocated = new System.Windows.Forms.MenuItem();
            this.mnuSaveAllocatedColumns = new System.Windows.Forms.MenuItem();
            this.mnuCashTran = new System.Windows.Forms.MenuItem();
            this.mnuAllocatedEdit = new System.Windows.Forms.MenuItem();
            this.mnuCloseTrade = new System.Windows.Forms.MenuItem();
            this.SMAllocated = new System.Windows.Forms.MenuItem();
            this.ExpandCollapseAllocated = new System.Windows.Forms.MenuItem();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.chkbxAllocationCalculator = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnAutoGrp = new Infragistics.Win.Misc.UltraButton();
            this.btnGetAllocationData = new Infragistics.Win.Misc.UltraButton();
            this.label7 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbbxdefaults = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.AllocationMain_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.tabAllocation = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnCheckSide = new Infragistics.Win.Misc.UltraButton();
            this.btnClosing = new Infragistics.Win.Misc.UltraButton();
            this.btnCancelData = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ugbxHeaderFill = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbAllocationScheme = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.rbtnAllocationByAccount = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.chkboxForceAllocation = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.rbtnAllocationBySymbol = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ugbxHeaderLeft = new Infragistics.Win.Misc.UltraGroupBox();
            this.rbHistorical = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.rbCurrent = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.dtToDatePickerAllocation = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtFromDatePickerAllocation = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.timerClear = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLblDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusStripProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.upnlFilterGrid = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._AllocationMain_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationMain_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationMain_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ctrlImageListButtons1 = new Prana.Utilities.UIUtilities.CtrlImageListButtons(this.components);
            this.btnSaveWOState = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl1.SuspendLayout();
            this.splitContainer2Panel2.ClientArea.SuspendLayout();
            this.splitContainer2Panel2.SuspendLayout();
            this.splitContainer3Panel2.ClientArea.SuspendLayout();
            this.splitContainer3Panel2.SuspendLayout();
            this.accountStrategyMapping1.SuspendLayout();
            this.panelReAllocateButtonAccountStrategy.ClientArea.SuspendLayout();
            this.panelReAllocateButtonAccountStrategy.SuspendLayout();
            this.splitContainer3Panel1.ClientArea.SuspendLayout();
            this.splitContainer3Panel1.SuspendLayout();
            this.panelAllocateFillArea.ClientArea.SuspendLayout();
            this.panelAllocateFillArea.SuspendLayout();
            this.allocationCalculatorUsrControl1.SuspendLayout();
            this.accountOnlyUserControl1.SuspendLayout();
            this.panelAllocateButtonAccount.ClientArea.SuspendLayout();
            this.panelAllocateButtonAccount.SuspendLayout();
            this.splitContainer2Panel1.ClientArea.SuspendLayout();
            this.splitContainer2Panel1.SuspendLayout();
            this.splitContainer1Panel2.ClientArea.SuspendLayout();
            this.splitContainer1Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocated)).BeginInit();
            this.splitContainer1Panel1.ClientArea.SuspendLayout();
            this.splitContainer1Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnallocated)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            this.panelReAllocateFillArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxAllocationCalculator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxdefaults)).BeginInit();
            this.AllocationMain_Fill_Panel.ClientArea.SuspendLayout();
            this.AllocationMain_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabAllocation)).BeginInit();
            this.tabAllocation.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxHeaderFill)).BeginInit();
            this.ugbxHeaderFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocationScheme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnAllocationByAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkboxForceAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnAllocationBySymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxHeaderLeft)).BeginInit();
            this.ugbxHeaderLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbHistorical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDatePickerAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDatePickerAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.upnlFilterGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.splitContainer2);
            this.ultraTabPageControl1.Controls.Add(this.splitContainer2Panel2);
            this.ultraTabPageControl1.Controls.Add(this.splitContainer2Panel1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1263, 482);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(450, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.RestoreExtent = 0;
            this.splitContainer2.Size = new System.Drawing.Size(6, 482);
            this.splitContainer2.TabIndex = 1;
            // 
            // splitContainer2Panel2
            // 
            // 
            // splitContainer2Panel2.ClientArea
            // 
            this.splitContainer2Panel2.ClientArea.Controls.Add(this.splitContainer3Panel2);
            this.splitContainer2Panel2.ClientArea.Controls.Add(this.splitContainer3);
            this.splitContainer2Panel2.ClientArea.Controls.Add(this.splitContainer3Panel1);
            this.splitContainer2Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2Panel2.Location = new System.Drawing.Point(450, 0);
            this.splitContainer2Panel2.Name = "splitContainer2Panel2";
            this.splitContainer2Panel2.Size = new System.Drawing.Size(813, 482);
            this.splitContainer2Panel2.TabIndex = 2;
            // 
            // splitContainer3Panel2
            // 
            // 
            // splitContainer3Panel2.ClientArea
            // 
            this.splitContainer3Panel2.ClientArea.Controls.Add(this.accountStrategyMapping1);
            this.splitContainer3Panel2.ClientArea.Controls.Add(this.panelReAllocateButtonAccountStrategy);
            this.splitContainer3Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3Panel2.Location = new System.Drawing.Point(0, 232);
            this.splitContainer3Panel2.Name = "splitContainer3Panel2";
            this.splitContainer3Panel2.Size = new System.Drawing.Size(813, 250);
            this.splitContainer3Panel2.TabIndex = 0;
            // 
            // accountStrategyMapping1
            // 
            this.accountStrategyMapping1.AutoScroll = true;
            this.accountStrategyMapping1.AutoScrollMargin = new System.Drawing.Size(50, 20);
            this.accountStrategyMapping1.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.accountStrategyMapping1.BorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.accountStrategyMapping1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.accountStrategyMapping1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.accountStrategyMapping1.Location = new System.Drawing.Point(0, 0);
            this.accountStrategyMapping1.Name = "accountStrategyMapping1";
            this.accountStrategyMapping1.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.accountStrategyMapping1.Size = new System.Drawing.Size(813, 220);
            this.accountStrategyMapping1.TabIndex = 0;
            // 
            // panelReAllocateButtonAccountStrategy
            // 
            // 
            // panelReAllocateButtonAccountStrategy.ClientArea
            // 
            this.panelReAllocateButtonAccountStrategy.ClientArea.Controls.Add(this.btnClearAllocated);
            this.panelReAllocateButtonAccountStrategy.ClientArea.Controls.Add(this.btnReAllocate);
            this.panelReAllocateButtonAccountStrategy.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelReAllocateButtonAccountStrategy.Location = new System.Drawing.Point(0, 220);
            this.panelReAllocateButtonAccountStrategy.Name = "panelReAllocateButtonAccountStrategy";
            this.panelReAllocateButtonAccountStrategy.Size = new System.Drawing.Size(813, 30);
            this.panelReAllocateButtonAccountStrategy.TabIndex = 0;
            // 
            // btnReAllocate
            // 
            this.btnReAllocate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReAllocate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReAllocate.Location = new System.Drawing.Point(267, 4);
            this.btnReAllocate.Name = "btnReAllocate";
            this.btnReAllocate.Size = new System.Drawing.Size(75, 23);
            this.btnReAllocate.TabIndex = 1;
            this.btnReAllocate.Text = "Allocate";
            this.btnReAllocate.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnReAllocate.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnReAllocate.Click += new System.EventHandler(this.btnReAllocate_Click_1);
           

            // 
            // btnClearAllocated
            // 
            this.btnClearAllocated.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClearAllocated.Appearance = appearance1;
            this.btnClearAllocated.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearAllocated.Location = new System.Drawing.Point(530, 2);
            this.btnClearAllocated.Name = "btnClearAllocated";
            this.btnClearAllocated.Size = new System.Drawing.Size(75, 23);
            this.btnClearAllocated.TabIndex = 2;
            this.btnClearAllocated.Text = "Clear";
            this.btnClearAllocated.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnClearAllocated.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnClearAllocated.Click += new System.EventHandler(this.btnClearAllocated_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer3.Location = new System.Drawing.Point(0, 226);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.RestoreExtent = 0;
            this.splitContainer3.Size = new System.Drawing.Size(813, 6);
            this.splitContainer3.TabIndex = 1;
            // 
            // splitContainer3Panel1
            // 
            // 
            // splitContainer3Panel1.ClientArea
            // 
            this.splitContainer3Panel1.ClientArea.Controls.Add(this.panelAllocateFillArea);
            this.splitContainer3Panel1.ClientArea.Controls.Add(this.panelAllocateButtonAccount);
            this.splitContainer3Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer3Panel1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3Panel1.Name = "splitContainer3Panel1";
            this.splitContainer3Panel1.Size = new System.Drawing.Size(813, 226);
            this.splitContainer3Panel1.TabIndex = 2;
            // 
            // panelAllocateFillArea
            // 
            // 
            // panelAllocateFillArea.ClientArea
            // 
            this.panelAllocateFillArea.ClientArea.Controls.Add(this.ctrlSwapParameters1);
            this.panelAllocateFillArea.ClientArea.Controls.Add(this.allocationCalculatorUsrControl1);
            this.panelAllocateFillArea.ClientArea.Controls.Add(this.accountOnlyUserControl1);
            this.panelAllocateFillArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAllocateFillArea.Location = new System.Drawing.Point(0, 0);
            this.panelAllocateFillArea.Name = "panelAllocateFillArea";
            this.panelAllocateFillArea.Size = new System.Drawing.Size(813, 198);
            this.panelAllocateFillArea.TabIndex = 0;
            // 
            // ctrlSwapParameters1
            // 
            this.ctrlSwapParameters1.AutoSize = true;
            this.ctrlSwapParameters1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.ctrlSwapParameters1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSwapParameters1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlSwapParameters1.IsPreTradeSwap = false;
            this.ctrlSwapParameters1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSwapParameters1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlSwapParameters1.Name = "ctrlSwapParameters1";
            this.ctrlSwapParameters1.Size = new System.Drawing.Size(813, 198);
            this.ctrlSwapParameters1.TabIndex = 0;
            // 
            // allocationCalculatorUsrControl1
            // 
            this.allocationCalculatorUsrControl1.AutoScroll = true;
            this.allocationCalculatorUsrControl1.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.allocationCalculatorUsrControl1.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.allocationCalculatorUsrControl1.BorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.allocationCalculatorUsrControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationCalculatorUsrControl1.Location = new System.Drawing.Point(0, 0);
            this.allocationCalculatorUsrControl1.MultipleGroupSelected = false;
            this.allocationCalculatorUsrControl1.Name = "allocationCalculatorUsrControl1";
            this.allocationCalculatorUsrControl1.Size = new System.Drawing.Size(813, 198);
            this.allocationCalculatorUsrControl1.TabIndex = 1;
            // 
            // accountOnlyUserControl1
            // 
            //this.accountOnlyUserControl1.AutoScroll = true;
            this.accountOnlyUserControl1.AutoScrollMargin = new System.Drawing.Size(40, 40);
            this.accountOnlyUserControl1.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.accountOnlyUserControl1.BorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.accountOnlyUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountOnlyUserControl1.Location = new System.Drawing.Point(0, 0);
            this.accountOnlyUserControl1.MultipleGroupSelected = false;
            this.accountOnlyUserControl1.Name = "accountOnlyUserControl1";
            this.accountOnlyUserControl1.Size = new System.Drawing.Size(813, 198);
            this.accountOnlyUserControl1.TabIndex = 0;
            // 
            // panelAllocateButtonAccount
            // 
            // 
            // panelAllocateButtonAccount.ClientArea
            // 
            this.panelAllocateButtonAccount.ClientArea.Controls.Add(this.lblStrategySearch);
            this.panelAllocateButtonAccount.ClientArea.Controls.Add(this.cmbStrategySearch);
            this.panelAllocateButtonAccount.ClientArea.Controls.Add(this.btnAllocate);
            this.panelAllocateButtonAccount.ClientArea.Controls.Add(this.btnClear);
            this.panelAllocateButtonAccount.ClientArea.Controls.Add(this.btnSaveSwap);
            this.panelAllocateButtonAccount.ClientArea.Controls.Add(this.btnSwapClose);
            this.panelAllocateButtonAccount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAllocateButtonAccount.Location = new System.Drawing.Point(0, 198);
            this.panelAllocateButtonAccount.Name = "panelAllocateButtonAccount";
            this.panelAllocateButtonAccount.Size = new System.Drawing.Size(813, 28);
            this.panelAllocateButtonAccount.TabIndex = 1;
            // 
            // lblStrategySearch
            // 
            appearance67.FontData.SizeInPoints = 9F;
            this.lblStrategySearch.Appearance = appearance67;
            this.lblStrategySearch.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);            
            this.lblStrategySearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStrategySearch.Location = new System.Drawing.Point(25, 5);
            this.lblStrategySearch.Name = "lblStrategySearch";
            this.lblStrategySearch.Size = new System.Drawing.Size(75, 23);
            this.lblStrategySearch.TabIndex = 3;
            this.lblStrategySearch.Text = "Search Strategy";
            this.lblStrategySearch.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.lblStrategySearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.lblStrategySearch.AutoSize = true;
            // 
            // cmbStrategySearch
            // 
            appearance68.FontData.SizeInPoints = 9F;
            this.cmbStrategySearch.Appearance = appearance68;
            this.cmbStrategySearch.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
            this.cmbStrategySearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStrategySearch.Location = new System.Drawing.Point(120, 2);
            this.cmbStrategySearch.Name = "btnAllocate";
            this.cmbStrategySearch.Size = new System.Drawing.Size(100, 23);
            this.cmbStrategySearch.TabIndex = 3;
            this.cmbStrategySearch.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.cmbStrategySearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.cmbStrategySearch.AutoSize = true;
            this.cmbStrategySearch.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Automatic;
            this.cmbStrategySearch.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbStrategySearch.ValueChanged += new System.EventHandler(this.cmbStrategySearch_ValueChanged);    
            // 
            // btnAllocate
            // 
            this.btnAllocate.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);            
            this.btnAllocate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllocate.Location = new System.Drawing.Point(325, 2);
            this.btnAllocate.Name = "btnAllocate";
            this.btnAllocate.Size = new System.Drawing.Size(75, 23);
            this.btnAllocate.TabIndex = 3;
            this.btnAllocate.Text = "Allocate";
            this.btnAllocate.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnAllocate.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnAllocate.Click += new System.EventHandler(this.btnAllocate_Click_1);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(525, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSaveSwap
            // 
            this.btnSaveSwap.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.FontData.SizeInPoints = 9F;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.btnSaveSwap.Appearance = appearance4;
            this.btnSaveSwap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSwap.Location = new System.Drawing.Point(373, 2);
            this.btnSaveSwap.Name = "btnSaveSwap";
            this.btnSaveSwap.Size = new System.Drawing.Size(109, 23);
            this.btnSaveSwap.TabIndex = 4;
            this.btnSaveSwap.Text = "Swap Update";
            this.btnSaveSwap.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnSaveSwap.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnSaveSwap.Click += new System.EventHandler(this.btnSaveSwap_Click);
            // 
            // btnSwapClose
            // 
            this.btnSwapClose.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
            appearance5.BackColor = System.Drawing.Color.Red;
            appearance5.BackColor2 = System.Drawing.Color.Red;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance5.FontData.SizeInPoints = 9F;
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.btnSwapClose.Appearance = appearance5;
            this.btnSwapClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapClose.Location = new System.Drawing.Point(139, 2);
            this.btnSwapClose.Name = "btnSwapClose";
            this.btnSwapClose.Size = new System.Drawing.Size(105, 23);
            this.btnSwapClose.TabIndex = 2;
            this.btnSwapClose.Text = "Hide Details";
            this.btnSwapClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnSwapClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnSwapClose.Click += new System.EventHandler(this.btnSwapClose_Click);
            // 
            // splitContainer2Panel1
            // 
            // 
            // splitContainer2Panel1.ClientArea
            // 
            this.splitContainer2Panel1.ClientArea.Controls.Add(this.splitContainer1Panel2);
            this.splitContainer2Panel1.ClientArea.Controls.Add(this.splitContainer1);
            this.splitContainer2Panel1.ClientArea.Controls.Add(this.splitContainer1Panel1);
            this.splitContainer2Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.splitContainer2Panel1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2Panel1.Name = "splitContainer2Panel1";
            this.splitContainer2Panel1.Size = new System.Drawing.Size(450, 482);
            this.splitContainer2Panel1.TabIndex = 0;
            // 
            // splitContainer1Panel2
            // 
            // 
            // splitContainer1Panel2.ClientArea
            // 
            this.splitContainer1Panel2.ClientArea.Controls.Add(this.grdAllocated);
            this.splitContainer1Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1Panel2.Location = new System.Drawing.Point(0, 232);
            this.splitContainer1Panel2.Name = "splitContainer1Panel2";
            this.splitContainer1Panel2.Size = new System.Drawing.Size(450, 250);
            this.splitContainer1Panel2.TabIndex = 0;
            // 
            // grdAllocated
            // 
            appearance6.BackColor = System.Drawing.Color.Black;
            appearance6.BackColor2 = System.Drawing.Color.Black;
            appearance6.BorderColor = System.Drawing.Color.Black;
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.Name = "Segoe UI";
            appearance6.FontData.SizeInPoints = 9F;
            this.grdAllocated.DisplayLayout.Appearance = appearance6;
            this.grdAllocated.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance7.BackColor = System.Drawing.Color.White;
            this.grdAllocated.DisplayLayout.CaptionAppearance = appearance7;
            this.grdAllocated.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAllocated.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAllocated.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdAllocated.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdAllocated.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdAllocated.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdAllocated.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdAllocated.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAllocated.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance8.FontData.BoldAsString = "False";
            appearance8.FontData.Name = "Segoe UI";
            appearance8.FontData.SizeInPoints = 9F;
            this.grdAllocated.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.grdAllocated.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAllocated.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.RepeatOnBreak;
            this.grdAllocated.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdAllocated.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdAllocated.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdAllocated.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAllocated.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAllocated.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAllocated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAllocated.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdAllocated.Location = new System.Drawing.Point(0, 0);
            this.grdAllocated.Name = "grdAllocated";
            this.grdAllocated.Size = new System.Drawing.Size(450, 250);
            this.grdAllocated.TabIndex = 0;
            this.grdAllocated.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdAllocated.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAllocated_InitializeLayout);
            this.grdAllocated.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdAllocated_InitializeRow);
            this.grdAllocated.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdAllocated_AfterRowFilterChanged);
            this.grdAllocated.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAllocated_BeforeCustomRowFilterDialog);
            this.grdAllocated.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAllocated_BeforeColumnChooserDisplayed);
            this.grdAllocated.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdAllocated_KeyDown);
            this.grdAllocated.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdAllocated_MouseClick);
            this.grdAllocated.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdAllocated_MouseDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 226);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.RestoreExtent = 226;
            this.splitContainer1.Size = new System.Drawing.Size(450, 6);
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer1Panel1
            // 
            // 
            // splitContainer1Panel1.ClientArea
            // 
            this.splitContainer1Panel1.ClientArea.Controls.Add(this.grdUnallocated);
            this.splitContainer1Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1Panel1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1Panel1.Name = "splitContainer1Panel1";
            this.splitContainer1Panel1.Size = new System.Drawing.Size(450, 226);
            this.splitContainer1Panel1.TabIndex = 2;
            // 
            // grdUnallocated
            // 
            appearance9.BackColor = System.Drawing.Color.Black;
            appearance9.BackColor2 = System.Drawing.Color.Black;
            appearance9.BorderColor = System.Drawing.Color.Black;
            appearance9.FontData.BoldAsString = "False";
            appearance9.FontData.Name = "Segoe UI";
            appearance9.FontData.SizeInPoints = 9F;
            this.grdUnallocated.DisplayLayout.Appearance = appearance9;
            this.grdUnallocated.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance10.BackColor = System.Drawing.Color.White;
            this.grdUnallocated.DisplayLayout.CaptionAppearance = appearance10;
            this.grdUnallocated.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.DisplayLayout.GroupByBox.Hidden = true;
            this.grdUnallocated.DisplayLayout.MaxColScrollRegions = 1;
            this.grdUnallocated.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdUnallocated.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdUnallocated.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdUnallocated.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdUnallocated.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdUnallocated.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdUnallocated.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance11.FontData.BoldAsString = "False";
            appearance11.FontData.Name = "Segoe UI";
            appearance11.FontData.SizeInPoints = 9F;
            this.grdUnallocated.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdUnallocated.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdUnallocated.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdUnallocated.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdUnallocated.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdUnallocated.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdUnallocated.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdUnallocated.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdUnallocated.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdUnallocated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUnallocated.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdUnallocated.Location = new System.Drawing.Point(0, 0);
            this.grdUnallocated.Name = "grdUnallocated";
            this.grdUnallocated.Size = new System.Drawing.Size(450, 226);
            this.grdUnallocated.TabIndex = 0;
            this.grdUnallocated.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdUnallocated.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdUnallocated_InitializeLayout);
            this.grdUnallocated.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdUnallocated_InitializeRow);
            this.grdUnallocated.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdUnallocated_AfterSelectChange);
            this.grdUnallocated.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdUnallocated_AfterRowFilterChanged);
            this.grdUnallocated.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdUnallocated_BeforeCustomRowFilterDialog);
            this.grdUnallocated.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdUnallocated_BeforeColumnChooserDisplayed);
            this.grdUnallocated.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdUnallocated_KeyDown);
            this.grdUnallocated.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdUnallocated_MouseClick);
            this.grdUnallocated.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdUnallocated_MouseDown);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ctrlAmendmend1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1263, 482);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ctrlCostAdjustment);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1263, 482);
            // 
            // ctrlAmendmend1
            // 
            this.ctrlAmendmend1.AutoSize = true;
            this.ctrlAmendmend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlAmendmend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlAmendmend1.Location = new System.Drawing.Point(0, 0);
            this.ctrlAmendmend1.Name = "ctrlAmendmend1";
            this.ctrlAmendmend1.Size = new System.Drawing.Size(1263, 482);
            this.ctrlAmendmend1.TabIndex = 0;
            this.inboxControlStyler1.SetStyleSettings(this.ctrlAmendmend1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // ctrlCostAdjustment
            // 
            this.ctrlCostAdjustment.AutoSize = true;
            this.ctrlCostAdjustment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlCostAdjustment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCostAdjustment.Location = new System.Drawing.Point(0, 0);
            this.ctrlCostAdjustment.Name = "ctrlCostAdjustment";
            this.ctrlCostAdjustment.Size = new System.Drawing.Size(1263, 482);
            this.ctrlCostAdjustment.TabIndex = 0;
            this.inboxControlStyler1.SetStyleSettings(this.ctrlCostAdjustment, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // panelReAllocateFillArea
            // 
            this.panelReAllocateFillArea.Location = new System.Drawing.Point(0, 0);
            this.panelReAllocateFillArea.Name = "panelReAllocateFillArea";
            this.panelReAllocateFillArea.Size = new System.Drawing.Size(200, 100);
            this.panelReAllocateFillArea.TabIndex = 0;
            // 
            // contextMnuUnAllocatedGrid
            // 
            this.contextMnuUnAllocatedGrid.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGroup,
            this.mnuUnGroup,
            this.mnuSwapDetailsUnAllocated,
            this.mnuSwapUnallocated,
            this.mnuSaveUnAllocatedColumns,
            this.mnuEditUnAllocatedColumns,
            this.SMUnallocated,
            this.mnuTradeAuditTrailUnallocated,
            this.ExpandCollapseUnallocated});
            this.contextMnuUnAllocatedGrid.Popup += new System.EventHandler(this.contextMnuUnAllocatedGrid_Popup);
            // 
            // mnuGroup
            // 
            this.mnuGroup.Index = 0;
            this.mnuGroup.Text = "Group";
            this.mnuGroup.Click += new System.EventHandler(this.mnuGroup_Click);
            // 
            // mnuUnGroup
            // 
            this.mnuUnGroup.Index = 1;
            this.mnuUnGroup.Text = "UnGroup";
            this.mnuUnGroup.Click += new System.EventHandler(this.mnuUnGroup_Click);
            // 
            // mnuSwapDetailsUnAllocated
            // 
            this.mnuSwapDetailsUnAllocated.Index = 2;
            this.mnuSwapDetailsUnAllocated.Text = "Swap Details";
            this.mnuSwapDetailsUnAllocated.Click += new System.EventHandler(this.mnuSwapDetails_Click);
            // 
            // mnuSwapUnallocated
            // 
            this.mnuSwapUnallocated.Index = 3;
            this.mnuSwapUnallocated.Text = "Book as Swap";
            this.mnuSwapUnallocated.Click += new System.EventHandler(this.mnuSwap_Click);
            // 
            // mnuSaveUnAllocatedColumns
            // 
            this.mnuSaveUnAllocatedColumns.Index = 4;
            this.mnuSaveUnAllocatedColumns.Text = "Save Layout";
            this.mnuSaveUnAllocatedColumns.Click += new System.EventHandler(this.mnuSaveColumns_Click);
            // 
            // mnuEditUnAllocatedColumns
            // 
            this.mnuEditUnAllocatedColumns.Index = 5;
            this.mnuEditUnAllocatedColumns.Text = "Edit";
            this.mnuEditUnAllocatedColumns.Click += new System.EventHandler(this.mnuUnallocateEdit_Click);
            // 
            // SMUnallocated
            // 
            this.SMUnallocated.Index = 6;
            this.SMUnallocated.Text = "SymbolLookUp";
            this.SMUnallocated.Name = "SMUnallocated";
            this.SMUnallocated.Click += new System.EventHandler(this.mnuSymbolLookUp_Click);
            // 
            // mnuTradeAuditTrailUnallocated
            // 
            this.mnuTradeAuditTrailUnallocated.Index = 7;
            this.mnuTradeAuditTrailUnallocated.Text = "Audit Trail";
            this.mnuTradeAuditTrailUnallocated.Click += new System.EventHandler(this.mnuTradeAuditTrailUnallocated_Click);
            // 
            // ExpandCollapseUnallocated
            // 
            this.ExpandCollapseUnallocated.Index = 8;
            this.ExpandCollapseUnallocated.Text = "Expand/Collapse All";
            this.ExpandCollapseUnallocated.Name = "ExpandCollapseUnallocated";
            this.ExpandCollapseUnallocated.Click += new System.EventHandler(this.mnuExpandCollapseAll_Click);
            // 
            // mnuTradeAuditTrailAllocated
            // 
            this.mnuTradeAuditTrailAllocated.Index = 8;
            this.mnuTradeAuditTrailAllocated.Text = "Audit Trail";
            this.mnuTradeAuditTrailAllocated.Click += new System.EventHandler(this.mnuTradeAuditTrailAllocated_Click);
            // 
            // contextMnuAllocatedGrid
            // 
            this.contextMnuAllocatedGrid.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuUnAllocate,
            this.mnuSwapAllocated,
            this.mnuSwapDetailsAllocated,
            this.mnuSaveAllocatedColumns,
            this.mnuCashTran,
            this.mnuAllocatedEdit,
            this.mnuCloseTrade,
            this.SMAllocated,
            this.mnuTradeAuditTrailAllocated,
            this.ExpandCollapseAllocated});
            this.contextMnuAllocatedGrid.Popup += new System.EventHandler(this.contextMnuAllocatedGrid_Popup);
            // 
            // mnuUnAllocate
            // 
            this.mnuUnAllocate.Index = 0;
            this.mnuUnAllocate.Text = "UnAllocate";
            this.mnuUnAllocate.Click += new System.EventHandler(this.mnuUnAllocate_Click);
            // 
            // mnuSwapAllocated
            // 
            this.mnuSwapAllocated.Index = 1;
            this.mnuSwapAllocated.Text = "Book as Swap";
            this.mnuSwapAllocated.Click += new System.EventHandler(this.mnuSwap_Click);
            // 
            // mnuSwapDetailsAllocated
            // 
            this.mnuSwapDetailsAllocated.Index = 2;
            this.mnuSwapDetailsAllocated.Text = "Swap Details";
            this.mnuSwapDetailsAllocated.Click += new System.EventHandler(this.mnuSwapDetails_Click);
            // 
            // mnuSaveAllocatedColumns
            // 
            this.mnuSaveAllocatedColumns.Index = 3;
            this.mnuSaveAllocatedColumns.Text = "Save Layout";
            this.mnuSaveAllocatedColumns.Click += new System.EventHandler(this.mnuSaveAllocatedColumns_Click);
            // 
            // mnuCashTran
            // 
            this.mnuCashTran.Index = 4;
            this.mnuCashTran.Text = "Generate Cash Transaction";
            this.mnuCashTran.Click += new System.EventHandler(this.menuCashTran_Click);
            // 
            // mnuAllocatedEdit
            // 
            this.mnuAllocatedEdit.Index = 5;
            this.mnuAllocatedEdit.Text = "Edit";
            this.mnuAllocatedEdit.Click += new System.EventHandler(this.mnuAllocatedEdit_Click);
            // 
            // mnuCloseTrade
            // 
            this.mnuCloseTrade.Index = 6;
            this.mnuCloseTrade.Text = "Close Order";
            this.mnuCloseTrade.Click += new System.EventHandler(this.mnuCloseTrade_Click);
            //
            // SMAllocated
            // 
            this.SMAllocated.Index = 7;
            this.SMAllocated.Text = "SymbolLookUp";
            this.SMAllocated.Name = "SMAllocated";
            this.SMAllocated.Click += new System.EventHandler(this.mnuSymbolLookUp_Click);
            // 
            // ExpandCollapseAllocated
            // 
            this.ExpandCollapseAllocated.Index = 9;
            this.ExpandCollapseAllocated.Name = "ExpandCollapseAllocated";
            this.ExpandCollapseAllocated.Text = "Expand/Collapse All";
            this.ExpandCollapseAllocated.Click += new System.EventHandler(this.mnuExpandCollapseAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;            
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(306, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 24);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save (w/Status)";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkbxAllocationCalculator
            // 
            appearance13.FontData.SizeInPoints = 9F;
            this.chkbxAllocationCalculator.Appearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxAllocationCalculator.CheckedAppearance = appearance14;
            this.chkbxAllocationCalculator.Location = new System.Drawing.Point(121, 6);
            this.chkbxAllocationCalculator.Name = "chkbxAllocationCalculator";
            this.chkbxAllocationCalculator.Size = new System.Drawing.Size(89, 20);
            this.chkbxAllocationCalculator.TabIndex = 1;
            this.chkbxAllocationCalculator.Text = "Calculator";
            this.chkbxAllocationCalculator.CheckedChanged += new System.EventHandler(this.chkbxAllocationCalculator_CheckedChanged);
            // 
            // btnAutoGrp
            // 
            this.btnAutoGrp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoGrp.Location = new System.Drawing.Point(489, 6);
            this.btnAutoGrp.Name = "btnAutoGrp";
            this.btnAutoGrp.Size = new System.Drawing.Size(106, 21);
            this.btnAutoGrp.TabIndex = 5;
            this.btnAutoGrp.Text = "Auto Group";
            this.btnAutoGrp.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnAutoGrp.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnAutoGrp.Click += new System.EventHandler(this.btnAutoGrp_Click);
            // 
            // btnGetAllocationData
            // 
            this.btnGetAllocationData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetAllocationData.Location = new System.Drawing.Point(389, 6);
            this.btnGetAllocationData.Name = "btnGetAllocationData";
            this.btnGetAllocationData.Size = new System.Drawing.Size(97, 21);
            this.btnGetAllocationData.TabIndex = 4;
            this.btnGetAllocationData.Text = "Get Data";
            this.btnGetAllocationData.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnGetAllocationData.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnGetAllocationData.Click += new System.EventHandler(this.btnGetAllocationData_Click);
            // 
            // label7
            // 
            appearance17.FontData.SizeInPoints = 9F;
            this.label7.Appearance = appearance17;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(209, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 18);
            this.label7.TabIndex = 2;
            this.label7.Text = "Allocation By";
            // 
            // cmbbxdefaults
            // 
            appearance18.FontData.SizeInPoints = 9F;
            this.cmbbxdefaults.Appearance = appearance18;
            this.cmbbxdefaults.AutoSize = false;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxdefaults.DisplayLayout.Appearance = appearance19;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbbxdefaults.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxdefaults.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxdefaults.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxdefaults.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance20.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.Appearance = appearance20;
            appearance21.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.BandLabelAppearance = appearance21;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance22.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.PromptAppearance = appearance22;
            this.cmbbxdefaults.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxdefaults.DisplayLayout.MaxRowScrollRegions = 1;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxdefaults.DisplayLayout.Override.ActiveCellAppearance = appearance23;
            appearance24.BackColor = System.Drawing.SystemColors.Highlight;
            appearance24.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxdefaults.DisplayLayout.Override.ActiveRowAppearance = appearance24;
            this.cmbbxdefaults.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxdefaults.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxdefaults.DisplayLayout.Override.CardAreaAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Silver;
            appearance26.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxdefaults.DisplayLayout.Override.CellAppearance = appearance26;
            this.cmbbxdefaults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxdefaults.DisplayLayout.Override.CellPadding = 0;
            appearance27.BackColor = System.Drawing.SystemColors.Control;
            appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance27.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance27.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxdefaults.DisplayLayout.Override.GroupByRowAppearance = appearance27;
            appearance28.TextHAlignAsString = "Left";
            this.cmbbxdefaults.DisplayLayout.Override.HeaderAppearance = appearance28;
            this.cmbbxdefaults.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxdefaults.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxdefaults.DisplayLayout.Override.RowAppearance = appearance29;
            this.cmbbxdefaults.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance30.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxdefaults.DisplayLayout.Override.TemplateAddRowAppearance = appearance30;
            this.cmbbxdefaults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxdefaults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxdefaults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxdefaults.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxdefaults.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbbxdefaults.Location = new System.Drawing.Point(440, 5);
            this.cmbbxdefaults.Name = "cmbbxdefaults";
            this.cmbbxdefaults.Size = new System.Drawing.Size(150, 23);
            this.cmbbxdefaults.TabIndex = 5;
            this.cmbbxdefaults.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxdefaults.ValueChanged += new System.EventHandler(this.cmbbxdefaults_ValueChanged);
            // 
            // AllocationMain_Fill_Panel
            // 
            this.AllocationMain_Fill_Panel.AutoSize = true;
            this.AllocationMain_Fill_Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // 
            // AllocationMain_Fill_Panel.ClientArea
            // 
            this.AllocationMain_Fill_Panel.ClientArea.Controls.Add(this.tabAllocation);
            this.AllocationMain_Fill_Panel.ClientArea.Controls.Add(this.ultraPanel3);
            this.AllocationMain_Fill_Panel.ClientArea.Controls.Add(this.ultraPanel1);
            this.AllocationMain_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.AllocationMain_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.AllocationMain_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.AllocationMain_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.AllocationMain_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AllocationMain_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllocationMain_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.AllocationMain_Fill_Panel.Name = "AllocationMain_Fill_Panel";
            this.AllocationMain_Fill_Panel.Size = new System.Drawing.Size(1267, 599);
            this.AllocationMain_Fill_Panel.TabIndex = 0;
            // 
            // tabAllocation
            // 
            appearance31.BorderColor = System.Drawing.Color.Transparent;
            appearance31.BorderColor2 = System.Drawing.Color.Transparent;
            appearance31.FontData.SizeInPoints = 9F;
            this.tabAllocation.Appearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance32.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance32.BackColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance32.BackColorDisabled2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance32.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance32.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance32.FontData.SizeInPoints = 9F;
            this.tabAllocation.ClientAreaAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.RosyBrown;
            appearance33.BackColor2 = System.Drawing.Color.RosyBrown;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.ForeColorDisabled = System.Drawing.Color.Black;
            this.tabAllocation.CloseButtonAppearance = appearance33;
            this.tabAllocation.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabAllocation.Controls.Add(this.ultraTabPageControl1);
            this.tabAllocation.Controls.Add(this.ultraTabPageControl2);
            this.tabAllocation.Controls.Add(this.ultraTabPageControl3);
            this.tabAllocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAllocation.Location = new System.Drawing.Point(0, 62);
            this.tabAllocation.Name = "tabAllocation";
            this.tabAllocation.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabAllocation.Size = new System.Drawing.Size(1267, 508);
            this.tabAllocation.TabIndex = 106;
            ultraTab1.Key = "Allocation";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Allocation";
            ultraTab2.Key = "EditTrades";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Edit Trades";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Cost Adjustment";
            ultraTab3.Key = "CostAdjustment";
            this.tabAllocation.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.tabAllocation.SelectedTabChanging += new Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventHandler(this.tabAllocation_SelectedTabChanging);
            this.tabAllocation.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1263, 482);
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.btnSaveWOState);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnDelete);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnCheckSide);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnClosing);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnCancelData);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 570);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(1267, 29);
            this.ultraPanel3.TabIndex = 107;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(799, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 24);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCheckSide
            // 
            this.btnCheckSide.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCheckSide.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckSide.Location = new System.Drawing.Point(790, 2);
            this.btnCheckSide.Name = "btnCheckSide";
            this.btnCheckSide.Size = new System.Drawing.Size(115, 24);
            this.btnCheckSide.TabIndex = 8;
            this.btnCheckSide.Text = "Check Side";
            this.btnCheckSide.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnCheckSide.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnCheckSide.Click += new System.EventHandler(this.btnCheckSide_Click);
            // 
            // btnClosing
            // 
            this.btnClosing.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClosing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosing.Location = new System.Drawing.Point(669, 2);
            this.btnClosing.Name = "btnClosing";
            this.btnClosing.Size = new System.Drawing.Size(115, 24);
            this.btnClosing.TabIndex = 9;
            this.btnClosing.Text = "Close Data";
            this.btnClosing.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnClosing.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnClosing.Click += new System.EventHandler(this.btnClosing_Click);
            // 
            // btnCancelData
            // 
            this.btnCancelData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancelData.Appearance = appearance12;
            this.btnCancelData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelData.Location = new System.Drawing.Point(548, 2);
            this.btnCancelData.Name = "btnCancelData";
            this.btnCancelData.Size = new System.Drawing.Size(115, 24);
            this.btnCancelData.TabIndex = 10;
            this.btnCancelData.Text = "Cancel";
            this.btnCancelData.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancelData.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnCancelData.Click += new System.EventHandler(this.btnCancelData_Click_1);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxHeaderFill);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxHeaderLeft);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraPanel1.Location = new System.Drawing.Point(0, 27);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1267, 35);
            this.ultraPanel1.TabIndex = 109;
            // 
            // ugbxHeaderFill
            // 
            this.ugbxHeaderFill.Controls.Add(this.rbtnAllocationByAccount);
            this.ugbxHeaderFill.Controls.Add(this.chkboxForceAllocation);
            this.ugbxHeaderFill.Controls.Add(this.chkbxAllocationCalculator);
            this.ugbxHeaderFill.Controls.Add(this.label7);
            this.ugbxHeaderFill.Controls.Add(this.rbtnAllocationBySymbol);
            this.ugbxHeaderFill.Controls.Add(this.cmbbxdefaults);
            this.ugbxHeaderFill.Controls.Add(this.cmbAllocationScheme);
            this.ugbxHeaderFill.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugbxHeaderFill.Location = new System.Drawing.Point(609, 0);
            this.ugbxHeaderFill.Name = "ugbxHeaderFill";
            this.ugbxHeaderFill.Size = new System.Drawing.Size(677, 33);
            this.ugbxHeaderFill.TabIndex = 108;
            // 
            // cmbAllocationScheme
            // 
            appearance41.FontData.SizeInPoints = 9F;
            this.cmbAllocationScheme.Appearance = appearance41;
            this.cmbAllocationScheme.AutoSize = false;
            appearance42.BackColor = System.Drawing.SystemColors.Window;
            appearance42.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAllocationScheme.DisplayLayout.Appearance = appearance42;
            this.cmbAllocationScheme.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance43.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance43.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance43.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAllocationScheme.DisplayLayout.GroupByBox.Appearance = appearance43;
            appearance44.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAllocationScheme.DisplayLayout.GroupByBox.BandLabelAppearance = appearance44;
            this.cmbAllocationScheme.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance45.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance45.BackColor2 = System.Drawing.SystemColors.Control;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAllocationScheme.DisplayLayout.GroupByBox.PromptAppearance = appearance45;
            this.cmbAllocationScheme.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAllocationScheme.DisplayLayout.MaxRowScrollRegions = 1;
            appearance46.BackColor = System.Drawing.SystemColors.Window;
            appearance46.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAllocationScheme.DisplayLayout.Override.ActiveCellAppearance = appearance46;
            appearance47.BackColor = System.Drawing.SystemColors.Highlight;
            appearance47.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAllocationScheme.DisplayLayout.Override.ActiveRowAppearance = appearance47;
            this.cmbAllocationScheme.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAllocationScheme.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance48.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAllocationScheme.DisplayLayout.Override.CardAreaAppearance = appearance48;
            appearance49.BorderColor = System.Drawing.Color.Silver;
            appearance49.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAllocationScheme.DisplayLayout.Override.CellAppearance = appearance49;
            this.cmbAllocationScheme.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAllocationScheme.DisplayLayout.Override.CellPadding = 0;
            this.cmbAllocationScheme.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance50.BackColor = System.Drawing.SystemColors.Control;
            appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance50.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance50.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAllocationScheme.DisplayLayout.Override.GroupByRowAppearance = appearance50;
            appearance51.TextHAlignAsString = "Left";
            this.cmbAllocationScheme.DisplayLayout.Override.HeaderAppearance = appearance51;
            this.cmbAllocationScheme.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAllocationScheme.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance52.BackColor = System.Drawing.SystemColors.Window;
            appearance52.BorderColor = System.Drawing.Color.Silver;
            this.cmbAllocationScheme.DisplayLayout.Override.RowAppearance = appearance52;
            this.cmbAllocationScheme.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance53.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAllocationScheme.DisplayLayout.Override.TemplateAddRowAppearance = appearance53;
            this.cmbAllocationScheme.DisplayLayout.PriorityScrolling = true;
            this.cmbAllocationScheme.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            this.cmbAllocationScheme.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAllocationScheme.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAllocationScheme.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAllocationScheme.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAllocationScheme.Location = new System.Drawing.Point(440, 5);
            this.cmbAllocationScheme.Name = "cmbAllocationScheme";
            this.cmbAllocationScheme.Size = new System.Drawing.Size(150, 23);
            this.cmbAllocationScheme.TabIndex = 6;
            this.cmbAllocationScheme.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // rbtnAllocationByAccount
            // 
            appearance37.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance37.FontData.SizeInPoints = 9F;
            this.rbtnAllocationByAccount.Appearance = appearance37;
            this.rbtnAllocationByAccount.CheckedIndex = 0;
            valueListItem2.DataValue = "Account";
            valueListItem2.DisplayText = "Account";
            this.rbtnAllocationByAccount.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem2});
            this.rbtnAllocationByAccount.Location = new System.Drawing.Point(295, 6);
            this.rbtnAllocationByAccount.Name = "rbtnAllocationByAccount";
            this.rbtnAllocationByAccount.Size = new System.Drawing.Size(70, 21);
            this.rbtnAllocationByAccount.TabIndex = 109;
            this.rbtnAllocationByAccount.Text = "Account";
            this.rbtnAllocationByAccount.ValueChanged += new System.EventHandler(this.rbtnAllocationByAccount_ValueChanged);
            // 
            // chkboxForceAllocation
            // 
            appearance38.FontData.SizeInPoints = 9F;
            this.chkboxForceAllocation.Appearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance39.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance39.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance39.ForeColor = System.Drawing.Color.Black;
            appearance39.ForeColorDisabled = System.Drawing.Color.Black;
            this.chkboxForceAllocation.CheckedAppearance = appearance39;
            this.chkboxForceAllocation.Enabled = false;
            this.chkboxForceAllocation.Location = new System.Drawing.Point(6, 6);
            this.chkboxForceAllocation.Name = "chkboxForceAllocation";
            this.chkboxForceAllocation.Size = new System.Drawing.Size(112, 20);
            this.chkboxForceAllocation.TabIndex = 0;
            this.chkboxForceAllocation.Text = "Force Allocation";
            ultraToolTipInfo1.ToolTipText = "By Pass Side Check";
            this.ultraToolTipManager1.SetUltraToolTip(this.chkboxForceAllocation, ultraToolTipInfo1);
            // 
            // rbtnAllocationBySymbol
            // 
            appearance40.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance40.FontData.SizeInPoints = 9F;
            this.rbtnAllocationBySymbol.Appearance = appearance40;
            valueListItem1.DataValue = "Symbol";
            valueListItem1.DisplayText = "Symbol";
            this.rbtnAllocationBySymbol.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1});
            this.rbtnAllocationBySymbol.Location = new System.Drawing.Point(370, 6);
            this.rbtnAllocationBySymbol.Name = "rbtnAllocationBySymbol";
            this.rbtnAllocationBySymbol.Size = new System.Drawing.Size(65, 21);
            this.rbtnAllocationBySymbol.TabIndex = 110;
            this.rbtnAllocationBySymbol.ValueChanged += new System.EventHandler(this.ultraOptionSet2_ValueChanged);
            // 
            // ugbxHeaderLeft
            // 
            this.ugbxHeaderLeft.Controls.Add(this.rbHistorical);
            this.ugbxHeaderLeft.Controls.Add(this.rbCurrent);
            this.ugbxHeaderLeft.Controls.Add(this.dtToDatePickerAllocation);
            this.ugbxHeaderLeft.Controls.Add(this.btnAutoGrp);
            this.ugbxHeaderLeft.Controls.Add(this.btnGetAllocationData);
            this.ugbxHeaderLeft.Controls.Add(this.dtFromDatePickerAllocation);
            this.ugbxHeaderLeft.Location = new System.Drawing.Point(3, 0);
            this.ugbxHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.ugbxHeaderLeft.Name = "ugbxHeaderLeft";
            this.ugbxHeaderLeft.Size = new System.Drawing.Size(603, 33);
            this.ugbxHeaderLeft.TabIndex = 107;
            // 
            // rbHistorical
            // 
            appearance54.FontData.SizeInPoints = 9F;
            this.rbHistorical.Appearance = appearance54;
            this.rbHistorical.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance55.TextHAlignAsString = "Center";
            appearance55.TextVAlignAsString = "Middle";
            this.rbHistorical.ItemAppearance = appearance55;
            valueListItem3.DataValue = "Historical";
            valueListItem3.DisplayText = "Historical";
            this.rbHistorical.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3});
            this.rbHistorical.Location = new System.Drawing.Point(72, 6);
            this.rbHistorical.Name = "rbHistorical";
            this.rbHistorical.Size = new System.Drawing.Size(79, 21);
            this.rbHistorical.TabIndex = 7;
            this.rbHistorical.ValueChanged += new System.EventHandler(this.rbHistorical_ValueChanged);
            // 
            // rbCurrent
            // 
            appearance56.FontData.SizeInPoints = 9F;
            this.rbCurrent.Appearance = appearance56;
            this.rbCurrent.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.rbCurrent.CheckedIndex = 0;
            appearance57.TextHAlignAsString = "Center";
            appearance57.TextVAlignAsString = "Middle";
            this.rbCurrent.ItemAppearance = appearance57;
            valueListItem4.DataValue = "Current";
            valueListItem4.DisplayText = "Current";
            this.rbCurrent.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4});
            this.rbCurrent.Location = new System.Drawing.Point(6, 6);
            this.rbCurrent.Name = "rbCurrent";
            this.rbCurrent.Size = new System.Drawing.Size(63, 21);
            this.rbCurrent.TabIndex = 6;
            this.rbCurrent.Text = "Current";
            this.rbCurrent.ValueChanged += new System.EventHandler(this.rbCurrent_ValueChanged);
            // 
            // dtToDatePickerAllocation
            // 
            appearance58.FontData.SizeInPoints = 9F;
            this.dtToDatePickerAllocation.Appearance = appearance58;
            this.dtToDatePickerAllocation.Location = new System.Drawing.Point(268, 6);
            this.dtToDatePickerAllocation.Name = "dtToDatePickerAllocation";
            this.dtToDatePickerAllocation.Size = new System.Drawing.Size(114, 21);
            this.dtToDatePickerAllocation.TabIndex = 3;
            this.dtToDatePickerAllocation.Nullable = false;
            // 
            // dtFromDatePickerAllocation
            // 
            this.dtFromDatePickerAllocation.Location = new System.Drawing.Point(154, 6);
            this.dtFromDatePickerAllocation.Name = "dtFromDatePickerAllocation";
            this.dtFromDatePickerAllocation.Size = new System.Drawing.Size(107, 21);
            this.dtFromDatePickerAllocation.TabIndex = 2;
            this.dtFromDatePickerAllocation.Nullable = false;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 572);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            appearance60.FontData.SizeInPoints = 9F;
            this.ultraToolbarsManager1.Appearance = appearance60;
            this.ultraToolbarsManager1.DesignerFlags = 0;
            this.ultraToolbarsManager1.DockWithinContainer = this.AllocationMain_Fill_Panel.ClientArea;
            this.ultraToolbarsManager1.ImageTransparentColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.MenuSettings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager1.ShowShortcutsInToolTips = true;
            this.ultraToolbarsManager1.ToolbarSettings.ToolSpacing = 8;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool16,
            buttonTool17,
            buttonTool1,
            buttonTool3,
            labelTool1,
            comboBoxTool1,
            buttonTool5,
            buttonTool25});
            ultraToolbar1.Text = "Allocation Menu";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.ultraToolbarsManager1.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            buttonTool19.SharedPropsInternal.Caption = "Preferences";
            buttonTool19.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool20.SharedPropsInternal.Caption = "Report";
            buttonTool20.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            appearance59.Image = global::Prana.AllocationNew.Properties.Resources.pencil;
            buttonTool21.SharedPropsInternal.AppearancesSmall.Appearance = appearance59;
            buttonTool21.SharedPropsInternal.Caption = "Edit Trades/Commissions";
            buttonTool21.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool22.SharedPropsInternal.Caption = "ButtonTool1";
            buttonTool23.SharedPropsInternal.Caption = "Get Data";
            buttonTool23.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool24.SharedPropsInternal.Caption = "Auto Group";
            buttonTool24.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            controlContainerTool3.SharedPropsInternal.Caption = "ControlContainerTool1";
            controlContainerTool3.SharedPropsInternal.Width = 80;
            controlContainerTool4.SharedPropsInternal.Caption = "ControlContainerTool2";
            appearance60.Image = global::Prana.AllocationNew.Properties.Resources.abc;
            buttonTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance60;
            appearance61.Image = global::Prana.AllocationNew.Properties.Resources.abc;
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance61;
            buttonTool2.SharedPropsInternal.Caption = "Allocation Scheme Recon";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance62.Image = global::Prana.AllocationNew.Properties.Resources.Checked;
            buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance62;
            buttonTool6.SharedPropsInternal.Caption = "Filter Status";
            appearance63.Image = global::Prana.AllocationNew.Properties.Resources.icon_search_filter;
            comboBoxTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance63;
            appearance64.Image = global::Prana.AllocationNew.Properties.Resources.icon_search_filter;
            comboBoxTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance64;
            comboBoxTool2.SharedPropsInternal.Caption = "Prefetch Filter";
            comboBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
            comboBoxTool2.ValueList = valueList1;
            labelTool2.SharedPropsInternal.Caption = "Filter Mode ";
            labelTool2.SharedPropsInternal.CustomizerCaption = "FilterType";
            labelTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool4.SharedPropsInternal.Caption = " Prorata";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            appearance65.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance65.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance65.ForeColor = System.Drawing.Color.Black;
            appearance65.ForeColorDisabled = System.Drawing.Color.Black;
            appearance65.Image = ((object)(resources.GetObject("appearance65.Image")));
            buttonTool7.SharedPropsInternal.AppearancesSmall.Appearance = appearance65;
            buttonTool7.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool19,
            buttonTool20,
            buttonTool21,
            buttonTool22,
            buttonTool23,
            buttonTool24,
            controlContainerTool3,
            controlContainerTool4,
            buttonTool2,
            buttonTool6,
            comboBoxTool2,
            labelTool2,
            buttonTool4,
            buttonTool7});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1267, 27);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 572);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 599);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1267, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1267, 27);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // timerClear
            // 
            this.timerClear.Interval = 4000;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLblDateTime,
            this.lblStatusStrip,
            this.lblStatusStripProgress});
            this.statusStrip1.Location = new System.Drawing.Point(4, 626);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1267, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLblDateTime
            // 
            this.statusLblDateTime.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.statusLblDateTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusLblDateTime.Name = "statusLblDateTime";
            this.statusLblDateTime.Size = new System.Drawing.Size(112, 17);
            this.statusLblDateTime.Text = "statusLblDateTime";
            // 
            // lblStatusStrip
            // 
            this.lblStatusStrip.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lblStatusStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusStrip.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStatusStrip.Name = "lblStatusStrip";
            this.lblStatusStrip.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblStatusStrip.Size = new System.Drawing.Size(61, 17);
            this.lblStatusStrip.Text = "loading...";
            this.lblStatusStrip.TextChanged += new System.EventHandler(this.lblStatusStrip_TextChanged);
            // 
            // lblStatusStripProgress
            // 
            this.lblStatusStripProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusStripProgress.Margin = new System.Windows.Forms.Padding(-5, 3, 0, 2);
            this.lblStatusStripProgress.Name = "lblStatusStripProgress";
            this.lblStatusStripProgress.Size = new System.Drawing.Size(0, 17);
            this.lblStatusStripProgress.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 1000;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ultraButton1.Appearance = appearance61;
            this.ultraButton1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraButton1.Location = new System.Drawing.Point(683, 518);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 24);
            this.ultraButton1.TabIndex = 10;
            this.ultraButton1.Text = "Cancel";
            this.ultraButton1.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // upnlFilterGrid
            // 
            this.upnlFilterGrid.AutoSize = true;
            this.upnlFilterGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.upnlFilterGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.upnlFilterGrid.Location = new System.Drawing.Point(4, 27);
            this.upnlFilterGrid.Name = "upnlFilterGrid";
            this.upnlFilterGrid.Size = new System.Drawing.Size(1267, 0);
            this.upnlFilterGrid.TabIndex = 5;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _AllocationMain_UltraFormManager_Dock_Area_Left
            // 
            this._AllocationMain_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationMain_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AllocationMain_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AllocationMain_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._AllocationMain_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._AllocationMain_UltraFormManager_Dock_Area_Left.Name = "_AllocationMain_UltraFormManager_Dock_Area_Left";
            this._AllocationMain_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 621);
            // 
            // _AllocationMain_UltraFormManager_Dock_Area_Right
            // 
            this._AllocationMain_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationMain_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AllocationMain_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AllocationMain_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._AllocationMain_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1271, 27);
            this._AllocationMain_UltraFormManager_Dock_Area_Right.Name = "_AllocationMain_UltraFormManager_Dock_Area_Right";
            this._AllocationMain_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 621);
            // 
            // _AllocationMain_UltraFormManager_Dock_Area_Top
            // 
            this._AllocationMain_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationMain_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AllocationMain_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AllocationMain_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AllocationMain_UltraFormManager_Dock_Area_Top.Name = "_AllocationMain_UltraFormManager_Dock_Area_Top";
            this._AllocationMain_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1275, 27);
            // 
            // _AllocationMain_UltraFormManager_Dock_Area_Bottom
            // 
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 648);
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.Name = "_AllocationMain_UltraFormManager_Dock_Area_Bottom";
            this._AllocationMain_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1275, 4);
            // 
            // btnSaveWOState
            // 
            this.btnSaveWOState.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveWOState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveWOState.Location = new System.Drawing.Point(427, 2);
            this.btnSaveWOState.Name = "btnSaveWOState";
            this.btnSaveWOState.Size = new System.Drawing.Size(115, 24);
            this.btnSaveWOState.TabIndex = 12;
            this.btnSaveWOState.Text = "Save (w/o Status)";
            this.btnSaveWOState.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.btnSaveWOState.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnSaveWOState.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // AllocationMain
            // 
            this.AcceptButton = this.btnGetAllocationData;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(1275, 652);
            this.Controls.Add(this.AllocationMain_Fill_Panel);
            this.Controls.Add(this.upnlFilterGrid);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._AllocationMain_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AllocationMain_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AllocationMain_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AllocationMain_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AllocationMain";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Allocation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AllocationMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AllocationMain_FormClosed);
            this.Load += new System.EventHandler(this.AllocationMain_Load);
            this.Shown += new System.EventHandler(this.AllocationMain_Shown);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.splitContainer2Panel2.ClientArea.ResumeLayout(false);
            this.splitContainer2Panel2.ResumeLayout(false);
            this.splitContainer3Panel2.ClientArea.ResumeLayout(false);
            this.splitContainer3Panel2.ResumeLayout(false);
            this.accountStrategyMapping1.ResumeLayout(false);
            this.panelReAllocateButtonAccountStrategy.ClientArea.ResumeLayout(false);
            this.panelReAllocateButtonAccountStrategy.ResumeLayout(false);
            this.splitContainer3Panel1.ClientArea.ResumeLayout(false);
            this.splitContainer3Panel1.ResumeLayout(false);
            this.panelAllocateFillArea.ClientArea.ResumeLayout(false);
            this.panelAllocateFillArea.ClientArea.PerformLayout();
            this.panelAllocateFillArea.ResumeLayout(false);
            this.allocationCalculatorUsrControl1.ResumeLayout(false);
            this.accountOnlyUserControl1.ResumeLayout(false);
            this.panelAllocateButtonAccount.ClientArea.ResumeLayout(false);
            this.panelAllocateButtonAccount.ResumeLayout(false);
            this.splitContainer2Panel1.ClientArea.ResumeLayout(false);
            this.splitContainer2Panel1.ResumeLayout(false);
            this.splitContainer1Panel2.ClientArea.ResumeLayout(false);
            this.splitContainer1Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocated)).EndInit();
            this.splitContainer1Panel1.ClientArea.ResumeLayout(false);
            this.splitContainer1Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdUnallocated)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl2.PerformLayout();
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl3.PerformLayout();
            this.panelReAllocateFillArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkbxAllocationCalculator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxdefaults)).EndInit();
            this.AllocationMain_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AllocationMain_Fill_Panel.ResumeLayout(false);
            this.AllocationMain_Fill_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabAllocation)).EndInit();
            this.tabAllocation.ResumeLayout(false);
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxHeaderFill)).EndInit();
            this.ugbxHeaderFill.ResumeLayout(false);
            this.ugbxHeaderFill.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocationScheme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnAllocationByAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkboxForceAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnAllocationBySymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxHeaderLeft)).EndInit();
            this.ugbxHeaderLeft.ResumeLayout(false);
            this.ugbxHeaderLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbHistorical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDatePickerAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDatePickerAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.upnlFilterGrid.ResumeLayout(false);
            this.upnlFilterGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdUnallocated;
        private System.Windows.Forms.ContextMenu contextMnuUnAllocatedGrid;
        private System.Windows.Forms.MenuItem mnuGroup;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdAllocated;
        private System.Windows.Forms.ContextMenu contextMnuAllocatedGrid;
        private System.Windows.Forms.MenuItem mnuUnAllocate;
        private System.Windows.Forms.MenuItem mnuUnGroup;
        private System.Windows.Forms.MenuItem mnuTradeAuditTrailUnallocated;
        private System.Windows.Forms.MenuItem mnuTradeAuditTrailAllocated;
        private Prana.AllocationNew.AccountStrategyMappingUserCtrlNew accountStrategyMapping1;
        private AccountOnlyUserControl accountOnlyUserControl1;
        Infragistics.Win.Misc.UltraPanel splitContainer2Panel1;
        Infragistics.Win.Misc.UltraPanel splitContainer1Panel2;
        private Infragistics.Win.Misc.UltraSplitter splitContainer1;
        Infragistics.Win.Misc.UltraPanel splitContainer1Panel1;
        Infragistics.Win.Misc.UltraPanel splitContainer2Panel2;
        Infragistics.Win.Misc.UltraPanel splitContainer3Panel2;
        private Infragistics.Win.Misc.UltraSplitter splitContainer3;
        Infragistics.Win.Misc.UltraPanel splitContainer3Panel1;
        private Infragistics.Win.Misc.UltraSplitter splitContainer2;
        private Infragistics.Win.Misc.UltraPanel panelAllocateButtonAccount;
        private Infragistics.Win.Misc.UltraPanel panelAllocateFillArea;
        private Infragistics.Win.Misc.UltraPanel panelReAllocateButtonAccountStrategy;
        private Infragistics.Win.Misc.UltraPanel panelReAllocateFillArea;
        private Infragistics.Win.Misc.UltraLabel lblStrategySearch;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbStrategySearch;
        private Infragistics.Win.Misc.UltraButton btnAllocate;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnReAllocate;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnAutoGrp;
        private Infragistics.Win.Misc.UltraButton btnGetAllocationData;
        private Infragistics.Win.Misc.UltraLabel label7;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxdefaults;
        private Infragistics.Win.Misc.UltraPanel AllocationMain_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxAllocationCalculator;
        private AllocationCalculatorUsrControl allocationCalculatorUsrControl1;
        private System.Windows.Forms.MenuItem mnuSwapDetailsUnAllocated;
        private System.Windows.Forms.MenuItem mnuSwapAllocated;
        private Infragistics.Win.Misc.UltraButton btnSaveSwap;
        private Prana.ClientCommon.CtrlSwapParameters ctrlSwapParameters1;
        private System.Windows.Forms.MenuItem mnuSwapUnallocated;
        private System.Windows.Forms.MenuItem mnuSwapDetailsAllocated;
        private System.Windows.Forms.Timer timerClear;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDatePickerAllocation;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDatePickerAllocation;
        private Infragistics.Win.Misc.UltraButton btnSwapClose;
        private System.Windows.Forms.MenuItem mnuSaveUnAllocatedColumns;
        private System.Windows.Forms.MenuItem mnuSaveAllocatedColumns;
        private System.Windows.Forms.MenuItem mnuEditUnAllocatedColumns;
        private Infragistics.Win.Misc.UltraButton btnCheckSide;
        private Infragistics.Win.Misc.UltraButton btnClosing;
        private System.Windows.Forms.MenuItem mnuCashTran;
        private System.Windows.Forms.MenuItem mnuAllocatedEdit;
        private System.Windows.Forms.MenuItem mnuCloseTrade;
        //private System.Windows.Forms.MenuItem mnuSymbolLookUp;
        //private System.Windows.Forms.MenuItem mnuSymbolLookUpUnAllocated;
        //private System.Windows.Forms.MenuItem mnuExpandCollapseAllUnallocated;
        //private System.Windows.Forms.MenuItem mnuExpandCollapseAllAllocated;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAllocationScheme;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLblDateTime;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusStripProgress;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkboxForceAllocation;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabAllocation;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private CtrlAmendmend ctrlAmendmend1;
        private CostAdjustmentControlMain ctrlCostAdjustment;
        private Infragistics.Win.Misc.UltraButton btnCancelData;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private System.Windows.Forms.MenuItem SMUnallocated;
        private System.Windows.Forms.MenuItem ExpandCollapseUnallocated;
        private System.Windows.Forms.MenuItem SMAllocated;
        private System.Windows.Forms.MenuItem ExpandCollapseAllocated;
        private Infragistics.Win.Misc.UltraGroupBox ugbxHeaderFill;
        private Infragistics.Win.Misc.UltraGroupBox ugbxHeaderLeft;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet rbtnAllocationBySymbol;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet rbtnAllocationByAccount;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet rbHistorical;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet rbCurrent;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraPanel upnlFilterGrid;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationMain_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationMain_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationMain_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationMain_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.Misc.UltraButton btnClearAllocated;
        private Infragistics.Win.Misc.UltraButton btnSaveWOState;
    }
}