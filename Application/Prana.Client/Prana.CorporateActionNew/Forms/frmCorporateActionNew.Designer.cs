using Infragistics.Win.UltraWinToolbars;
using Prana.Global;
namespace Prana.CorporateActionNew.Forms
{
    partial class frmCorporateActionNew
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
            if (_caServicesProxy != null)
                _caServicesProxy.Dispose();
            
            if (_proxy != null)
                _proxy.Dispose();

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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool11 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("CorporateActionApplied");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool12 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("FromdateApplied");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool13 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ToDateApplied");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnGetCAUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreviewUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnClearUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExportUndo");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool14 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("CorporateActionApplied");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool15 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("FromdateApplied");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool16 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ToDateApplied");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnGetCAUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreviewUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnClearUndo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExportUndo");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool7 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("CorporateActions");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("FromDateContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool2 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ToDateContainer");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnGetCARedo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnGetCARedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnPreviewRedo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreviewRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnSaveRedo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnClearRedo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnClearRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnImportRedo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnImportRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnUpdateCAs = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnUpdateCAs");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnDeleteRedo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnDeleteRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool btnExportRedo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExportRedo");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool5 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("AccountsContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool9 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("BrokerContainer");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cmbCounterPartyUnApplied");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnDeleteRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExportRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnGetCARedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnImportRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreviewRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnClearRedo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnUpdateCAs");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("multiSelectDropDownUnApplied");
            Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool3 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cmbCATypeRedo");
            Infragistics.Win.ValueList valueList3 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("FromDateContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool4 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ToDateContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool6 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("AccountsContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool8 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("CorporateActions");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool10 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("BrokerContainer");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar3 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool17 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("CorporateActionNew");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreview");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnApply");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveCorpAction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnClear");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSymbolLookUp");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool18 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("AccountsContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool19 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("BrokerContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool20 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("CorporateActionNew");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreview");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnApply");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveCorpAction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnClear");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSymbolLookUp");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool21 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("AccountsContainer");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool22 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("BrokerContainer");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.cmbCATypeUndo = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.dtFromDateUndo = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtToDateUndo = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.cmbCATypeRedo = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.dtFromDateRedo = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtToDateRedo = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.multiSelectDropDownUnApplied = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.cmbCounterPartyUnApplied = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCATypeApply = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.multiSelectDropDownNew = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grpPositionUndoRedo = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel4 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.pctBoxApplied = new System.Windows.Forms.PictureBox();
            this.ctrlPositionsUndo = new Prana.CorporateActionNew.Controls.ctrlPositions();
            this.spltUndo = new System.Windows.Forms.Splitter();
            this.grpCorpEntryUndoRedo = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel3 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ctrlCAEntryUndo = new Prana.CorporateActionNew.Controls.ctrlCAEntry();
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager2 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel5 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.pctBoxUnApplied = new System.Windows.Forms.PictureBox();
            this.ctrlPositionsRedo = new Prana.CorporateActionNew.Controls.ctrlPositions();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel6 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ctrlCAEntryRedo = new Prana.CorporateActionNew.Controls.ctrlCAEntry();
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grpPositionsApply = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.pctBoxNew = new System.Windows.Forms.PictureBox();
            this.ctrlPositionsApply = new Prana.CorporateActionNew.Controls.ctrlPositions();
            this.spltApply = new System.Windows.Forms.Splitter();
            this.grpCorpActionApply = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ctrlCAEntryApply = new Prana.CorporateActionNew.Controls.ctrlCAEntry();
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager3 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabCtrlUndoCA = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.tbUndoShareArea = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.fromDateControl = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.toDateControl = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.tabCtrlMainCA = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.tbSharedMain = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerClear = new System.Windows.Forms.Timer(this.components);
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCATypeUndo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDateUndo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDateUndo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCATypeRedo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDateRedo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDateRedo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyUnApplied)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCATypeApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpPositionUndoRedo)).BeginInit();
            this.grpPositionUndoRedo.SuspendLayout();
            this.ultraExpandableGroupBoxPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxApplied)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCorpEntryUndoRedo)).BeginInit();
            this.grpCorpEntryUndoRedo.SuspendLayout();
            this.ultraExpandableGroupBoxPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager2)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            this.ultraExpandableGroupBoxPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxUnApplied)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            this.ultraExpandableGroupBoxPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpPositionsApply)).BeginInit();
            this.grpPositionsApply.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCorpActionApply)).BeginInit();
            this.grpCorpActionApply.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager3)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlUndoCA)).BeginInit();
            this.tabCtrlUndoCA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fromDateControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toDateControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlMainCA)).BeginInit();
            this.tabCtrlMainCA.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCATypeUndo
            // 
            this.cmbCATypeUndo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCATypeUndo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCATypeUndo.Location = new System.Drawing.Point(128, 5);
            this.cmbCATypeUndo.Name = "cmbCATypeUndo";
            this.cmbCATypeUndo.Size = new System.Drawing.Size(110, 22);
            this.cmbCATypeUndo.TabIndex = 28;
            this.cmbCATypeUndo.ValueChanged += new System.EventHandler(this.cmbCATypeUndo_ValueChanged);
            // 
            // dtFromDateUndo
            // 
            this.dtFromDateUndo.Location = new System.Drawing.Point(360, 5);
            this.dtFromDateUndo.Name = "dtFromDateUndo";
            this.dtFromDateUndo.Size = new System.Drawing.Size(109, 21);
            this.dtFromDateUndo.TabIndex = 27;
            // 
            // dtToDateUndo
            // 
            this.dtToDateUndo.Location = new System.Drawing.Point(527, 5);
            this.dtToDateUndo.Name = "dtToDateUndo";
            this.dtToDateUndo.Size = new System.Drawing.Size(109, 21);
            this.dtToDateUndo.TabIndex = 26;
            // 
            // cmbCATypeRedo
            // 
            this.cmbCATypeRedo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCATypeRedo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCATypeRedo.Location = new System.Drawing.Point(5, 5);
            this.cmbCATypeRedo.Name = "cmbCATypeRedo";
            this.cmbCATypeRedo.Size = new System.Drawing.Size(114, 22);
            this.cmbCATypeRedo.TabIndex = 28;
            this.cmbCATypeRedo.ValueChanged += new System.EventHandler(this.cmbCATypeRedo_ValueChanged);
            // 
            // dtFromDateRedo
            // 
            this.dtFromDateRedo.Location = new System.Drawing.Point(5, 5);
            this.dtFromDateRedo.Name = "dtFromDateRedo";
            this.dtFromDateRedo.Size = new System.Drawing.Size(90, 21);
            this.dtFromDateRedo.TabIndex = 0;
            // 
            // dtToDateRedo
            // 
            this.dtToDateRedo.Location = new System.Drawing.Point(5, 5);
            this.dtToDateRedo.Name = "dtToDateRedo";
            this.dtToDateRedo.Size = new System.Drawing.Size(90, 21);
            this.dtToDateRedo.TabIndex = 0;
            // 
            // multiSelectDropDownUnApplied
            // 
            this.multiSelectDropDownUnApplied.Location = new System.Drawing.Point(5, 5);
            this.multiSelectDropDownUnApplied.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.multiSelectDropDownUnApplied.Name = "multiSelectDropDownUnApplied";
            this.multiSelectDropDownUnApplied.Size = new System.Drawing.Size(160, 22);
            this.multiSelectDropDownUnApplied.TabIndex = 36;
            this.multiSelectDropDownUnApplied.TitleText = "";
            // 
            // cmbCounterPartyUnApplied
            // 
            this.cmbCounterPartyUnApplied.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterPartyUnApplied.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterPartyUnApplied.Location = new System.Drawing.Point(5, 5);
            this.cmbCounterPartyUnApplied.Name = "cmbCounterPartyUnApplied";
            this.cmbCounterPartyUnApplied.Size = new System.Drawing.Size(100, 21);
            this.cmbCounterPartyUnApplied.TabIndex = 10;
            // 
            // cmbCATypeApply
            // 
            this.cmbCATypeApply.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCATypeApply.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCATypeApply.Location = new System.Drawing.Point(5, 5);
            this.cmbCATypeApply.Name = "cmbCATypeApply";
            this.cmbCATypeApply.Size = new System.Drawing.Size(123, 22);
            this.cmbCATypeApply.TabIndex = 0;
            this.cmbCATypeApply.ValueChanged += new System.EventHandler(this.cmbCATypeApply_ValueChanged);
            // 
            // multiSelectDropDownNew
            // 
            this.multiSelectDropDownNew.Location = new System.Drawing.Point(5, 5);
            this.multiSelectDropDownNew.Name = "multiSelectDropDownNew";
            this.multiSelectDropDownNew.Size = new System.Drawing.Size(160, 23);
            this.multiSelectDropDownNew.TabIndex = 9;
            this.multiSelectDropDownNew.TitleText = "";
            // 
            // cmbCounterParty
            // 
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.Location = new System.Drawing.Point(5, 5);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(100, 21);
            this.cmbCounterParty.TabIndex = 10;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.grpPositionUndoRedo);
            this.ultraTabPageControl3.Controls.Add(this.spltUndo);
            this.ultraTabPageControl3.Controls.Add(this.grpCorpEntryUndoRedo);
            this.ultraTabPageControl3.Controls.Add(this._ultraTabPageControl3_Toolbars_Dock_Area_Left);
            this.ultraTabPageControl3.Controls.Add(this._ultraTabPageControl3_Toolbars_Dock_Area_Right);
            this.ultraTabPageControl3.Controls.Add(this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom);
            this.ultraTabPageControl3.Controls.Add(this._ultraTabPageControl3_Toolbars_Dock_Area_Top);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(1, 22);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1227, 518);
            // 
            // grpPositionUndoRedo
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.grpPositionUndoRedo.Appearance = appearance1;
            this.grpPositionUndoRedo.Controls.Add(this.ultraExpandableGroupBoxPanel4);
            this.grpPositionUndoRedo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPositionUndoRedo.ExpandedSize = new System.Drawing.Size(1227, 362);
            this.grpPositionUndoRedo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPositionUndoRedo.Location = new System.Drawing.Point(0, 156);
            this.grpPositionUndoRedo.Name = "grpPositionUndoRedo";
            this.grpPositionUndoRedo.Size = new System.Drawing.Size(1227, 362);
            this.grpPositionUndoRedo.TabIndex = 14;
            this.grpPositionUndoRedo.TabStop = false;
            this.grpPositionUndoRedo.Text = "Modified Positions";
            // 
            // ultraExpandableGroupBoxPanel4
            // 
            this.ultraExpandableGroupBoxPanel4.AutoSize = true;
            this.ultraExpandableGroupBoxPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.pctBoxApplied);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.ctrlPositionsUndo);
            this.ultraExpandableGroupBoxPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraExpandableGroupBoxPanel4.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel4.Name = "ultraExpandableGroupBoxPanel4";
            this.ultraExpandableGroupBoxPanel4.Size = new System.Drawing.Size(1221, 339);
            this.ultraExpandableGroupBoxPanel4.TabIndex = 0;
            // 
            // pctBoxApplied
            // 
            this.pctBoxApplied.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctBoxApplied.BackColor = System.Drawing.Color.White;
            this.pctBoxApplied.Image = global::Prana.CorporateActionNew.Properties.Resources.circle;
            this.pctBoxApplied.Location = new System.Drawing.Point(495, 167);
            this.pctBoxApplied.Name = "pctBoxApplied";
            this.pctBoxApplied.Size = new System.Drawing.Size(180, 34);
            this.inboxControlStyler1.SetStyleSettings(this.pctBoxApplied, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.pctBoxApplied.TabIndex = 1;
            this.pctBoxApplied.TabStop = false;
            this.pctBoxApplied.Visible = false;
            this.pctBoxApplied.Paint += new System.Windows.Forms.PaintEventHandler(this.pctBox_Paint);
            // 
            // ctrlPositionsUndo
            // 
            this.ctrlPositionsUndo.AutoScroll = true;
            this.ctrlPositionsUndo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlPositionsUndo.Location = new System.Drawing.Point(0, 0);
            this.ctrlPositionsUndo.Name = "ctrlPositionsUndo";
            this.ctrlPositionsUndo.Size = new System.Drawing.Size(1221, 339);
            this.ctrlPositionsUndo.TabIndex = 0;
            // 
            // spltUndo
            // 
            this.spltUndo.Dock = System.Windows.Forms.DockStyle.Top;
            this.spltUndo.Location = new System.Drawing.Point(0, 153);
            this.spltUndo.Name = "spltUndo";
            this.spltUndo.Size = new System.Drawing.Size(1227, 3);
            this.inboxControlStyler1.SetStyleSettings(this.spltUndo, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.spltUndo.TabIndex = 13;
            this.spltUndo.TabStop = false;
            // 
            // grpCorpEntryUndoRedo
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grpCorpEntryUndoRedo.Appearance = appearance2;
            this.grpCorpEntryUndoRedo.Controls.Add(this.ultraExpandableGroupBoxPanel3);
            this.grpCorpEntryUndoRedo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCorpEntryUndoRedo.ExpandedSize = new System.Drawing.Size(1227, 128);
            this.grpCorpEntryUndoRedo.Location = new System.Drawing.Point(0, 25);
            this.grpCorpEntryUndoRedo.Name = "grpCorpEntryUndoRedo";
            this.grpCorpEntryUndoRedo.Size = new System.Drawing.Size(1227, 128);
            this.grpCorpEntryUndoRedo.TabIndex = 12;
            this.grpCorpEntryUndoRedo.TabStop = false;
            this.grpCorpEntryUndoRedo.Text = "Corporate Action Details";
            // 
            // ultraExpandableGroupBoxPanel3
            // 
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.ctrlCAEntryUndo);
            this.ultraExpandableGroupBoxPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel3.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel3.Name = "ultraExpandableGroupBoxPanel3";
            this.ultraExpandableGroupBoxPanel3.Size = new System.Drawing.Size(1221, 105);
            this.ultraExpandableGroupBoxPanel3.TabIndex = 0;
            // 
            // ctrlCAEntryUndo
            // 
            this.ctrlCAEntryUndo.AutoScroll = true;
            this.ctrlCAEntryUndo.AutoSize = true;
            this.ctrlCAEntryUndo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCAEntryUndo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ctrlCAEntryUndo.Location = new System.Drawing.Point(0, 0);
            this.ctrlCAEntryUndo.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlCAEntryUndo.Name = "ctrlCAEntryUndo";
            this.ctrlCAEntryUndo.Size = new System.Drawing.Size(1221, 105);
            this.ctrlCAEntryUndo.TabIndex = 0;
            this.ctrlCAEntryUndo.Click += new System.EventHandler(this.OnPreviewUndoClick);
            // 
            // _ultraTabPageControl3_Toolbars_Dock_Area_Left
            // 
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.Name = "_ultraTabPageControl3_Toolbars_Dock_Area_Left";
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 493);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager2;
            // 
            // ultraToolbarsManager2
            // 
            this.ultraToolbarsManager2.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
            this.ultraToolbarsManager2.DesignerFlags = 1;
            this.ultraToolbarsManager2.DockWithinContainer = this.ultraTabPageControl3;
            this.ultraToolbarsManager2.LockToolbars = true;
            this.ultraToolbarsManager2.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager2.ShowToolTips = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            controlContainerTool11.Control = this.cmbCATypeUndo;
            controlContainerTool11.InstanceProps.Width = 221;
            controlContainerTool12.Control = this.dtFromDateUndo;
            controlContainerTool13.Control = this.dtToDateUndo;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool11,
            controlContainerTool12,
            controlContainerTool13,
            buttonTool9,
            buttonTool10,
            buttonTool11,
            buttonTool12,
            buttonTool13});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.ToolSpacing = 5;
            ultraToolbar1.ShowInToolbarList = false;
            ultraToolbar1.Text = "UltraToolbarApplied";
            this.ultraToolbarsManager2.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            controlContainerTool14.Control = this.cmbCATypeUndo;
            controlContainerTool14.SharedPropsInternal.Caption = "Corporate Action";
            controlContainerTool14.SharedPropsInternal.CustomizerCaption = "Corpo";
            controlContainerTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool14.SharedPropsInternal.Width = 221;
            controlContainerTool15.Control = this.dtFromDateUndo;
            controlContainerTool15.SharedPropsInternal.Caption = "From Date";
            controlContainerTool15.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool16.Control = this.dtToDateUndo;
            controlContainerTool16.SharedPropsInternal.Caption = "To Date";
            controlContainerTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool14.SharedPropsInternal.Caption = "Get";
            buttonTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool15.SharedPropsInternal.Caption = "Preview";
            buttonTool15.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool16.SharedPropsInternal.Caption = "Undo";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool17.SharedPropsInternal.Caption = "Clear";
            buttonTool17.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool18.SharedPropsInternal.Caption = "Export";
            buttonTool18.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.ultraToolbarsManager2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool14,
            controlContainerTool15,
            controlContainerTool16,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18});
            this.ultraToolbarsManager2.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager2_ToolClick);
            // 
            // _ultraTabPageControl3_Toolbars_Dock_Area_Right
            // 
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1227, 25);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.Name = "_ultraTabPageControl3_Toolbars_Dock_Area_Right";
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 493);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager2;
            // 
            // _ultraTabPageControl3_Toolbars_Dock_Area_Bottom
            // 
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 518);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.Name = "_ultraTabPageControl3_Toolbars_Dock_Area_Bottom";
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1227, 0);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager2;
            // 
            // _ultraTabPageControl3_Toolbars_Dock_Area_Top
            // 
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.Name = "_ultraTabPageControl3_Toolbars_Dock_Area_Top";
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1227, 25);
            this._ultraTabPageControl3_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager2;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.ultraExpandableGroupBox1);
            this.ultraTabPageControl4.Controls.Add(this.splitter1);
            this.ultraTabPageControl4.Controls.Add(this.ultraExpandableGroupBox2);
            this.ultraTabPageControl4.Controls.Add(this._ultraTabPageControl4_Toolbars_Dock_Area_Left);
            this.ultraTabPageControl4.Controls.Add(this._ultraTabPageControl4_Toolbars_Dock_Area_Right);
            this.ultraTabPageControl4.Controls.Add(this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom);
            this.ultraTabPageControl4.Controls.Add(this._ultraTabPageControl4_Toolbars_Dock_Area_Top);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(1227, 518);
            // 
            // ultraExpandableGroupBox1
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraExpandableGroupBox1.Appearance = appearance3;
            this.ultraExpandableGroupBox1.Controls.Add(this.ultraExpandableGroupBoxPanel5);
            this.ultraExpandableGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(1227, 362);
            this.ultraExpandableGroupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(0, 156);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(1227, 362);
            this.ultraExpandableGroupBox1.TabIndex = 14;
            this.ultraExpandableGroupBox1.TabStop = false;
            this.ultraExpandableGroupBox1.Text = "Modified Positions";
            // 
            // ultraExpandableGroupBoxPanel5
            // 
            this.ultraExpandableGroupBoxPanel5.Controls.Add(this.pctBoxUnApplied);
            this.ultraExpandableGroupBoxPanel5.Controls.Add(this.ctrlPositionsRedo);
            this.ultraExpandableGroupBoxPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel5.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel5.Name = "ultraExpandableGroupBoxPanel5";
            this.ultraExpandableGroupBoxPanel5.Size = new System.Drawing.Size(1221, 339);
            this.ultraExpandableGroupBoxPanel5.TabIndex = 0;
            // 
            // pctBoxUnApplied
            // 
            this.pctBoxUnApplied.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctBoxUnApplied.BackColor = System.Drawing.Color.White;
            this.pctBoxUnApplied.Image = global::Prana.CorporateActionNew.Properties.Resources.circle;
            this.pctBoxUnApplied.Location = new System.Drawing.Point(498, 167);
            this.pctBoxUnApplied.Name = "pctBoxUnApplied";
            this.pctBoxUnApplied.Size = new System.Drawing.Size(180, 34);
            this.inboxControlStyler1.SetStyleSettings(this.pctBoxUnApplied, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.pctBoxUnApplied.TabIndex = 1;
            this.pctBoxUnApplied.TabStop = false;
            this.pctBoxUnApplied.Visible = false;
            this.pctBoxUnApplied.Paint += new System.Windows.Forms.PaintEventHandler(this.pctBox_Paint);
            // 
            // ctrlPositionsRedo
            // 
            this.ctrlPositionsRedo.AutoScroll = true;
            this.ctrlPositionsRedo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlPositionsRedo.Location = new System.Drawing.Point(0, 0);
            this.ctrlPositionsRedo.Name = "ctrlPositionsRedo";
            this.ctrlPositionsRedo.Size = new System.Drawing.Size(1221, 339);
            this.ctrlPositionsRedo.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 153);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1227, 3);
            this.inboxControlStyler1.SetStyleSettings(this.splitter1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            // 
            // ultraExpandableGroupBox2
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraExpandableGroupBox2.Appearance = appearance4;
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel6);
            this.ultraExpandableGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(1227, 128);
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 25);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(1227, 128);
            this.ultraExpandableGroupBox2.TabIndex = 12;
            this.ultraExpandableGroupBox2.TabStop = false;
            this.ultraExpandableGroupBox2.Text = "Corporate Action Details";
            // 
            // ultraExpandableGroupBoxPanel6
            // 
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.ctrlCAEntryRedo);
            this.ultraExpandableGroupBoxPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel6.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel6.Name = "ultraExpandableGroupBoxPanel6";
            this.ultraExpandableGroupBoxPanel6.Size = new System.Drawing.Size(1221, 105);
            this.ultraExpandableGroupBoxPanel6.TabIndex = 0;
            // 
            // ctrlCAEntryRedo
            // 
            this.ctrlCAEntryRedo.AutoScroll = true;
            this.ctrlCAEntryRedo.AutoSize = true;
            this.ctrlCAEntryRedo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCAEntryRedo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlCAEntryRedo.Location = new System.Drawing.Point(0, 0);
            this.ctrlCAEntryRedo.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlCAEntryRedo.Name = "ctrlCAEntryRedo";
            this.ctrlCAEntryRedo.Size = new System.Drawing.Size(1221, 105);
            this.ctrlCAEntryRedo.TabIndex = 0;
            // 
            // _ultraTabPageControl4_Toolbars_Dock_Area_Left
            // 
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.Name = "_ultraTabPageControl4_Toolbars_Dock_Area_Left";
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 493);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.ultraTabPageControl4;
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager1.ShowToolTips = false;
            ultraToolbar2.DockedColumn = 0;
            ultraToolbar2.DockedRow = 0;
            controlContainerTool7.Control = this.cmbCATypeRedo;
            controlContainerTool7.InstanceProps.Width = 225;
            controlContainerTool1.Control = this.dtFromDateRedo;
            controlContainerTool2.Control = this.dtToDateRedo;
            controlContainerTool5.Control = this.multiSelectDropDownUnApplied;
            controlContainerTool5.InstanceProps.Width = 166;
            controlContainerTool9.Control = this.cmbCounterPartyUnApplied;
            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool7,
            controlContainerTool1,
            controlContainerTool2,
            btnGetCARedo,
            btnPreviewRedo,
            btnSaveRedo,
            btnClearRedo,
            btnImportRedo,
            btnUpdateCAs,
            btnDeleteRedo,
            btnExportRedo,
            controlContainerTool5,
            controlContainerTool9});
            ultraToolbar2.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.ToolSpacing = 5;
            ultraToolbar2.ShowInToolbarList = false;
            ultraToolbar2.Text = "UltraToolbar1";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar2});
            comboBoxTool1.ValueList = valueList1;
            buttonTool1.SharedPropsInternal.Caption = "Delete";
            buttonTool1.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool2.SharedPropsInternal.Caption = "Export";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool3.SharedPropsInternal.Caption = "Get";
            buttonTool3.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool4.SharedPropsInternal.Caption = "Import";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool5.SharedPropsInternal.Caption = "Preview";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool6.SharedPropsInternal.Caption = "Apply";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool7.SharedPropsInternal.Caption = "Clear";
            buttonTool7.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool8.SharedPropsInternal.Caption = "Save";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            comboBoxTool2.ValueList = valueList2;
            comboBoxTool3.SharedPropsInternal.Caption = "Corporate Action";
            comboBoxTool3.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool3.ValueList = valueList3;
            controlContainerTool3.Control = this.dtFromDateRedo;
            controlContainerTool3.SharedPropsInternal.Caption = "From Date";
            controlContainerTool3.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool4.Control = this.dtToDateRedo;
            controlContainerTool4.SharedPropsInternal.Caption = "To Date";
            controlContainerTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool6.Control = this.multiSelectDropDownUnApplied;
            controlContainerTool6.SharedPropsInternal.Caption = "AccountsContainer";
            controlContainerTool6.SharedPropsInternal.Width = 166;
            controlContainerTool8.Control = this.cmbCATypeRedo;
            controlContainerTool8.SharedPropsInternal.Caption = "Corporate Action";
            controlContainerTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool8.SharedPropsInternal.Width = 225;
            controlContainerTool10.Control = this.cmbCounterPartyUnApplied;
            controlContainerTool10.SharedPropsInternal.Caption = "BrokerContainer";
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            comboBoxTool1,
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            comboBoxTool2,
            comboBoxTool3,
            controlContainerTool3,
            controlContainerTool4,
            controlContainerTool6,
            controlContainerTool8,
            controlContainerTool10});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _ultraTabPageControl4_Toolbars_Dock_Area_Right
            // 
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1227, 25);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.Name = "_ultraTabPageControl4_Toolbars_Dock_Area_Right";
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 493);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ultraTabPageControl4_Toolbars_Dock_Area_Bottom
            // 
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 518);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.Name = "_ultraTabPageControl4_Toolbars_Dock_Area_Bottom";
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1227, 0);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ultraTabPageControl4_Toolbars_Dock_Area_Top
            // 
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(5);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.Name = "_ultraTabPageControl4_Toolbars_Dock_Area_Top";
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1227, 25);
            this._ultraTabPageControl4_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.grpPositionsApply);
            this.ultraTabPageControl1.Controls.Add(this.spltApply);
            this.ultraTabPageControl1.Controls.Add(this.grpCorpActionApply);
            this.ultraTabPageControl1.Controls.Add(this._ultraTabPageControl1_Toolbars_Dock_Area_Left);
            this.ultraTabPageControl1.Controls.Add(this._ultraTabPageControl1_Toolbars_Dock_Area_Right);
            this.ultraTabPageControl1.Controls.Add(this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom);
            this.ultraTabPageControl1.Controls.Add(this._ultraTabPageControl1_Toolbars_Dock_Area_Top);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 22);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1229, 541);
            // 
            // grpPositionsApply
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.grpPositionsApply.Appearance = appearance5;
            this.grpPositionsApply.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.grpPositionsApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPositionsApply.ExpandedSize = new System.Drawing.Size(1229, 368);
            this.grpPositionsApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPositionsApply.Location = new System.Drawing.Point(0, 173);
            this.grpPositionsApply.Name = "grpPositionsApply";
            this.grpPositionsApply.Size = new System.Drawing.Size(1229, 368);
            this.grpPositionsApply.TabIndex = 9;
            this.grpPositionsApply.TabStop = false;
            this.grpPositionsApply.Text = "Modified Positions";
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.pctBoxNew);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.ctrlPositionsApply);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(1223, 345);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // pctBoxNew
            // 
            this.pctBoxNew.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctBoxNew.BackColor = System.Drawing.Color.White;
            this.pctBoxNew.Image = global::Prana.CorporateActionNew.Properties.Resources.circle;
            this.pctBoxNew.Location = new System.Drawing.Point(501, 147);
            this.pctBoxNew.Name = "pctBoxNew";
            this.pctBoxNew.Size = new System.Drawing.Size(180, 34);
            this.inboxControlStyler1.SetStyleSettings(this.pctBoxNew, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.pctBoxNew.TabIndex = 1;
            this.pctBoxNew.TabStop = false;
            this.pctBoxNew.Visible = false;
            this.pctBoxNew.Paint += new System.Windows.Forms.PaintEventHandler(this.pctBox_Paint);
            // 
            // ctrlPositionsApply
            // 
            this.ctrlPositionsApply.AutoScroll = true;
            this.ctrlPositionsApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlPositionsApply.Location = new System.Drawing.Point(0, 0);
            this.ctrlPositionsApply.Name = "ctrlPositionsApply";
            this.ctrlPositionsApply.Size = new System.Drawing.Size(1223, 345);
            this.ctrlPositionsApply.TabIndex = 0;
            // 
            // spltApply
            // 
            this.spltApply.Dock = System.Windows.Forms.DockStyle.Top;
            this.spltApply.Location = new System.Drawing.Point(0, 170);
            this.spltApply.Name = "spltApply";
            this.spltApply.Size = new System.Drawing.Size(1229, 3);
            this.inboxControlStyler1.SetStyleSettings(this.spltApply, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.spltApply.TabIndex = 8;
            this.spltApply.TabStop = false;
            // 
            // grpCorpActionApply
            // 
            appearance6.FontData.SizeInPoints = 9F;
            appearance6.ForeColor = System.Drawing.Color.White;
            this.grpCorpActionApply.Appearance = appearance6;
            this.grpCorpActionApply.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.grpCorpActionApply.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCorpActionApply.ExpandedSize = new System.Drawing.Size(1229, 145);
            this.grpCorpActionApply.Location = new System.Drawing.Point(0, 25);
            this.grpCorpActionApply.Name = "grpCorpActionApply";
            this.grpCorpActionApply.Size = new System.Drawing.Size(1229, 145);
            this.grpCorpActionApply.TabIndex = 0;
            this.grpCorpActionApply.TabStop = false;
            this.grpCorpActionApply.Text = "Corporate Action Details";
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.ctrlCAEntryApply);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(1223, 122);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // ctrlCAEntryApply
            // 
            this.ctrlCAEntryApply.AutoScroll = true;
            this.ctrlCAEntryApply.AutoSize = true;
            this.ctrlCAEntryApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCAEntryApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlCAEntryApply.Location = new System.Drawing.Point(0, 0);
            this.ctrlCAEntryApply.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlCAEntryApply.Name = "ctrlCAEntryApply";
            this.ctrlCAEntryApply.Size = new System.Drawing.Size(1223, 122);
            this.ctrlCAEntryApply.TabIndex = 0;
            // 
            // _ultraTabPageControl1_Toolbars_Dock_Area_Left
            // 
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.Name = "_ultraTabPageControl1_Toolbars_Dock_Area_Left";
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 516);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager3;
            // 
            // ultraToolbarsManager3
            // 
            this.ultraToolbarsManager3.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
            this.ultraToolbarsManager3.DesignerFlags = 1;
            this.ultraToolbarsManager3.DockWithinContainer = this.ultraTabPageControl1;
            this.ultraToolbarsManager3.LockToolbars = true;
            this.ultraToolbarsManager3.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager3.ShowToolTips = false;
            ultraToolbar3.DockedColumn = 0;
            ultraToolbar3.DockedRow = 0;
            controlContainerTool17.Control = this.cmbCATypeApply;
            controlContainerTool17.InstanceProps.Width = 223;
            controlContainerTool18.Control = this.multiSelectDropDownNew;
            controlContainerTool19.Control = this.cmbCounterParty;
            ultraToolbar3.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool17,
            buttonTool19,
            buttonTool20,
            buttonTool21,
            buttonTool22,
            buttonTool23,
            controlContainerTool18,
            controlContainerTool19});
            ultraToolbar3.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar3.Settings.ToolSpacing = 5;
            ultraToolbar3.ShowInToolbarList = false;
            ultraToolbar3.Text = "UltraToolbar1";
            this.ultraToolbarsManager3.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar3});
            controlContainerTool20.Control = this.cmbCATypeApply;
            controlContainerTool20.SharedPropsInternal.Caption = "Corporate Action";
            controlContainerTool20.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool24.SharedPropsInternal.Caption = "Preview";
            buttonTool24.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool25.SharedPropsInternal.Caption = "Apply";
            buttonTool25.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool26.SharedPropsInternal.Caption = "Save Corporate Action";
            buttonTool26.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool27.SharedPropsInternal.Caption = "Clear";
            buttonTool27.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool28.SharedPropsInternal.Caption = "Security Master";
            buttonTool28.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            controlContainerTool21.Control = this.multiSelectDropDownNew;
            controlContainerTool21.SharedPropsInternal.Caption = "AccountsContainer";
            controlContainerTool22.Control = this.cmbCounterParty;
            controlContainerTool22.SharedPropsInternal.Caption = "BrokerContainer";
            this.ultraToolbarsManager3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool20,
            buttonTool24,
            buttonTool25,
            buttonTool26,
            buttonTool27,
            buttonTool28,
            controlContainerTool21,
            controlContainerTool22});
            this.ultraToolbarsManager3.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager3_ToolClick);
            // 
            // _ultraTabPageControl1_Toolbars_Dock_Area_Right
            // 
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1229, 25);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.Name = "_ultraTabPageControl1_Toolbars_Dock_Area_Right";
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 516);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager3;
            // 
            // _ultraTabPageControl1_Toolbars_Dock_Area_Bottom
            // 
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 541);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.Name = "_ultraTabPageControl1_Toolbars_Dock_Area_Bottom";
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1229, 0);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager3;
            // 
            // _ultraTabPageControl1_Toolbars_Dock_Area_Top
            // 
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.Name = "_ultraTabPageControl1_Toolbars_Dock_Area_Top";
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1229, 25);
            this._ultraTabPageControl1_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager3;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.tabCtrlUndoCA);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1229, 541);
            // 
            // tabCtrlUndoCA
            // 
            this.tabCtrlUndoCA.Controls.Add(this.tbUndoShareArea);
            this.tabCtrlUndoCA.Controls.Add(this.ultraTabPageControl3);
            this.tabCtrlUndoCA.Controls.Add(this.ultraTabPageControl4);
            this.tabCtrlUndoCA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlUndoCA.Font = new System.Drawing.Font("Segoe UI", 9F);
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.BackwardDiagonal;
            this.tabCtrlUndoCA.HotTrackAppearance = appearance7;
            this.tabCtrlUndoCA.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlUndoCA.Name = "tabCtrlUndoCA";
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabCtrlUndoCA.SelectedTabAppearance = appearance8;
            this.tabCtrlUndoCA.SharedControlsPage = this.tbUndoShareArea;
            this.tabCtrlUndoCA.Size = new System.Drawing.Size(1229, 541);
            this.tabCtrlUndoCA.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabCtrlUndoCA.TabIndex = 2;
            ultraTab1.Key = "tbApplied";
            ultraTab1.TabPage = this.ultraTabPageControl3;
            ultraTab1.Text = "Applied";
            ultraTab2.Key = "tbUnApplied";
            ultraTab2.TabPage = this.ultraTabPageControl4;
            ultraTab2.Text = "Un-Applied";
            this.tabCtrlUndoCA.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // tbUndoShareArea
            // 
            this.tbUndoShareArea.Location = new System.Drawing.Point(-10000, -10000);
            this.tbUndoShareArea.Name = "tbUndoShareArea";
            this.tbUndoShareArea.Size = new System.Drawing.Size(1227, 518);
            // 
            // fromDateControl
            // 
            this.fromDateControl.Location = new System.Drawing.Point(0, 0);
            this.fromDateControl.Name = "fromDateControl";
            this.fromDateControl.Size = new System.Drawing.Size(144, 21);
            this.fromDateControl.TabIndex = 0;
            // 
            // toDateControl
            // 
            this.toDateControl.Location = new System.Drawing.Point(0, 0);
            this.toDateControl.Name = "toDateControl";
            this.toDateControl.Size = new System.Drawing.Size(144, 21);
            this.toDateControl.TabIndex = 0;
            // 
            // tabCtrlMainCA
            // 
            appearance9.FontData.SizeInPoints = 9F;
            this.tabCtrlMainCA.Appearance = appearance9;
            this.tabCtrlMainCA.Controls.Add(this.tbSharedMain);
            this.tabCtrlMainCA.Controls.Add(this.ultraTabPageControl1);
            this.tabCtrlMainCA.Controls.Add(this.ultraTabPageControl2);
            this.tabCtrlMainCA.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.BackwardDiagonal;
            this.tabCtrlMainCA.HotTrackAppearance = appearance10;
            this.tabCtrlMainCA.Location = new System.Drawing.Point(8, 31);
            this.tabCtrlMainCA.Name = "tabCtrlMainCA";
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabCtrlMainCA.SelectedTabAppearance = appearance11;
            this.tabCtrlMainCA.SharedControlsPage = this.tbSharedMain;
            this.tabCtrlMainCA.Size = new System.Drawing.Size(1231, 564);
            this.tabCtrlMainCA.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabCtrlMainCA.TabIndex = 0;
            ultraTab3.Key = "tbApply";
            ultraTab3.TabPage = this.ultraTabPageControl1;
            ultraTab3.Text = "Apply New";
            ultraTab4.Key = "tbUndo";
            ultraTab4.TabPage = this.ultraTabPageControl2;
            ultraTab4.Text = "Historical Corp Actions";
            this.tabCtrlMainCA.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab4});
            this.tabCtrlMainCA.TabStop = false;
            // 
            // tbSharedMain
            // 
            this.tbSharedMain.Location = new System.Drawing.Point(-10000, -10000);
            this.tbSharedMain.Name = "tbSharedMain";
            this.tbSharedMain.Size = new System.Drawing.Size(1229, 541);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(8, 595);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1231, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // timerClear
            // 
            this.timerClear.Interval = 6000;
            this.timerClear.Tick += new System.EventHandler(this.timerClear_Tick);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _frmCorporateActionNew_UltraFormManager_Dock_Area_Left
            // 
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.Name = "_frmCorporateActionNew_UltraFormManager_Dock_Area_Left";
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 586);
            // 
            // _frmCorporateActionNew_UltraFormManager_Dock_Area_Right
            // 
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1239, 31);
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.Name = "_frmCorporateActionNew_UltraFormManager_Dock_Area_Right";
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 586);
            // 
            // _frmCorporateActionNew_UltraFormManager_Dock_Area_Top
            // 
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.Name = "_frmCorporateActionNew_UltraFormManager_Dock_Area_Top";
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1247, 31);
            // 
            // _frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 617);
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.Name = "_frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom";
            this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1247, 8);
            // 
            // frmCorporateActionNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1247, 625);
            this.Controls.Add(this.tabCtrlMainCA);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._frmCorporateActionNew_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmCorporateActionNew_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmCorporateActionNew_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MinimumSize = new System.Drawing.Size(1200, 625);
            this.Name = "frmCorporateActionNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Corporate Action";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCorporateActionNew_FormClosing);
            this.Load += new System.EventHandler(this.frmCorporateActionNew_Load);
            this.Disposed += new System.EventHandler(this.frmCorporateActionNew_Disposed);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCATypeUndo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDateUndo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDateUndo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCATypeRedo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDateRedo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDateRedo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyUnApplied)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCATypeApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpPositionUndoRedo)).EndInit();
            this.grpPositionUndoRedo.ResumeLayout(false);
            this.grpPositionUndoRedo.PerformLayout();
            this.ultraExpandableGroupBoxPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxApplied)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCorpEntryUndoRedo)).EndInit();
            this.grpCorpEntryUndoRedo.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel3.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager2)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxUnApplied)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel6.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpPositionsApply)).EndInit();
            this.grpPositionsApply.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCorpActionApply)).EndInit();
            this.grpCorpActionApply.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager3)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlUndoCA)).EndInit();
            this.tabCtrlUndoCA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fromDateControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toDateControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlMainCA)).EndInit();
            this.tabCtrlMainCA.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCtrlMainCA;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage tbSharedMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCATypeApply;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpCorpActionApply;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpPositionsApply;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private System.Windows.Forms.Splitter spltApply;
        private Prana.CorporateActionNew.Controls.ctrlPositions ctrlPositionsApply;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCtrlUndoCA;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage tbUndoShareArea;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpPositionUndoRedo;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel4;
        private Prana.CorporateActionNew.Controls.ctrlPositions ctrlPositionsUndo;
        private System.Windows.Forms.Splitter spltUndo;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpCorpEntryUndoRedo;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel3;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCATypeUndo;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDateUndo;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDateUndo;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel5;
        private Prana.CorporateActionNew.Controls.ctrlPositions ctrlPositionsRedo;
        private System.Windows.Forms.Splitter splitter1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel6;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCATypeRedo;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterPartyUnApplied;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDateRedo;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDateRedo;
        private Prana.CorporateActionNew.Controls.ctrlCAEntry ctrlCAEntryUndo;
        private Prana.CorporateActionNew.Controls.ctrlCAEntry ctrlCAEntryRedo;
        private Prana.CorporateActionNew.Controls.ctrlCAEntry ctrlCAEntryApply;
        //private Infragistics.Win.Misc.UltraButton btnScreenshot;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timerClear;
        private System.Windows.Forms.PictureBox pctBoxNew;
        private System.Windows.Forms.PictureBox pctBoxApplied;
        private System.Windows.Forms.PictureBox pctBoxUnApplied;
        private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDownNew;
        private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDownUnApplied;        
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterParty;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionNew_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionNew_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionNew_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionNew_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor fromDateControl;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor toDateControl;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ultraTabPageControl4_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ultraTabPageControl4_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ultraTabPageControl4_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ultraTabPageControl4_Toolbars_Dock_Area_Top;
        private UltraToolbarsDockArea _ultraTabPageControl3_Toolbars_Dock_Area_Left;
        private UltraToolbarsManager ultraToolbarsManager2;
        private UltraToolbarsDockArea _ultraTabPageControl3_Toolbars_Dock_Area_Right;
        private UltraToolbarsDockArea _ultraTabPageControl3_Toolbars_Dock_Area_Bottom;
        private UltraToolbarsDockArea _ultraTabPageControl3_Toolbars_Dock_Area_Top;
        private UltraToolbarsDockArea _ultraTabPageControl1_Toolbars_Dock_Area_Left;
        private UltraToolbarsManager ultraToolbarsManager3;
        private UltraToolbarsDockArea _ultraTabPageControl1_Toolbars_Dock_Area_Right;
        private UltraToolbarsDockArea _ultraTabPageControl1_Toolbars_Dock_Area_Bottom;
        private UltraToolbarsDockArea _ultraTabPageControl1_Toolbars_Dock_Area_Top;
       // private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}

