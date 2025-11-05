#region Using
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.Admin.BLL;
using Prana.BusinessLogic;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
#endregion Using

namespace Prana.ThirdPartyReport
{
    /// <summary>
    /// Summary description for ThirdPartyReportControl.
    /// </summary>
    public class ThirdPartyReportControl : System.Windows.Forms.UserControl, IComparer
    {
        #region Wizard Stuff

        private Infragistics.Win.Misc.UltraLabel lblThirdParty;
        private Infragistics.Win.Misc.UltraLabel lblAccounts;
        private System.Windows.Forms.DateTimePicker txtDayLightSaving;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbThirdParty;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdThirdParty;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnGenerate;
        //private Infragistics.Win.Misc.UltraButton btnScreenshot;
        private Infragistics.Win.Misc.UltraButton btnView;
        private CheckedListBox chkLstThirdPartyAccounts;
        private ErrorProvider errorProvider1;
        private Infragistics.Win.Misc.UltraGroupBox groupBox1;
        private UltraGridExcelExporter ultraGridExcelExporter1;
        private UltraCombo cmbFormat;
        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkL2Data;
        private CheckedListBox chkLstAuec;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSelectAllAccounts;
        private UltraGrid grdExportToExcel;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem IgnoreToolStripMenuItem;
        private ToolStripMenuItem BackToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSelectAllAUECs;
        private Infragistics.Win.Misc.UltraLabel label3;
        private UltraCombo cmbThirdPartyType;
        private Infragistics.Win.Misc.UltraLabel label4;
        private UltraCombo cmbDateType;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private CtrlImageListButtons ctrlImageListButtons1;
        private IContainer components;
        private System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIncluedSent;
        private int toolTipIndex;
        CheckBoxOnHeader_CreationFilter _headerCheckBoxUnallocated = new CheckBoxOnHeader_CreationFilter();
        Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> _taxLotWithStateDict = new Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState>();
        Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> _taxLotIgnoreStateDict = new Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState>();
        Dictionary<string, string> _deletedToIgnoreDict = new Dictionary<string, string>();
        DataSet _datasetFromFilteredRowsDs = null;
        StringBuilder _auecIDStringBuilder = null;

        StringBuilder _accountIDStringBuilder = null;
        DataSet _dsXML = new DataSet();
        public DataSet _dsForXMLFile = null;
        public delegate void ThirdPartyObjHandler(Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection thirdPartyCollObj);
        BackgroundWorker _bgFetchDataAsycWithDirectSPCall = null;
        bool _blnAUEClstTest = false;

        public ThirdPartyReportControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

        }



        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_headerCheckBoxUnallocated != null)
                    _headerCheckBoxUnallocated.Dispose();

                if (lblThirdParty != null)
                {
                    lblThirdParty.Dispose();
                }
                if (lblAccounts != null)
                {
                    lblAccounts.Dispose();
                }
                if (txtDayLightSaving != null)
                {
                    txtDayLightSaving.Dispose();
                }
                if (cmbThirdParty != null)
                {
                    cmbThirdParty.Dispose();
                }
                if (grdThirdParty != null)
                {
                    grdThirdParty.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnGenerate != null)
                {
                    btnGenerate.Dispose();
                }
                if (btnView != null)
                {
                    btnView.Dispose();
                }
                if (chkLstThirdPartyAccounts != null)
                {
                    chkLstThirdPartyAccounts.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (ultraGridExcelExporter1 != null)
                {
                    ultraGridExcelExporter1.Dispose();
                }
                if (cmbFormat != null)
                {
                    cmbFormat.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (chkL2Data != null)
                {
                    chkL2Data.Dispose();
                }
                if (chkLstAuec != null)
                {
                    chkLstAuec.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (chkSelectAllAccounts != null)
                {
                    chkSelectAllAccounts.Dispose();
                }
                if (grdExportToExcel != null)
                {
                    grdExportToExcel.Dispose();
                }
                if (ultraTabControl1 != null)
                {
                    ultraTabControl1.Dispose();
                }
                if (ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if (ultraTabPageControl1 != null)
                {
                    ultraTabPageControl1.Dispose();
                }
                if (ultraTabPageControl2 != null)
                {
                    ultraTabPageControl2.Dispose();
                }
                if (contextMenuStrip1 != null)
                {
                    contextMenuStrip1.Dispose();
                }
                if (IgnoreToolStripMenuItem != null)
                {
                    IgnoreToolStripMenuItem.Dispose();
                }
                if (BackToolStripMenuItem != null)
                {
                    BackToolStripMenuItem.Dispose();
                }
                if (statusStrip1 != null)
                {
                    statusStrip1.Dispose();
                }
                if (toolStripStatusLabel1 != null)
                {
                    toolStripStatusLabel1.Dispose();
                }
                if (chkSelectAllAUECs != null)
                {
                    chkSelectAllAUECs.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (cmbThirdPartyType != null)
                {
                    cmbThirdPartyType.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (cmbDateType != null)
                {
                    cmbDateType.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (toolTip != null)
                {
                    toolTip.Dispose();
                }
                if (chkIncluedSent != null)
                {
                    chkIncluedSent.Dispose();
                }
                if (_datasetFromFilteredRowsDs != null)
                {
                    _datasetFromFilteredRowsDs.Dispose();
                }
                if (_dsXML != null)
                {
                    _dsXML.Dispose();
                }
                if (_dsForXMLFile != null)
                {
                    _dsForXMLFile.Dispose();
                }
                if (_bgFetchDataAsycWithDirectSPCall != null)
                {
                    _bgFetchDataAsycWithDirectSPCall.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(fetchDataAsycWithDirectSPCall_RunWorkerCompleted);
                    _bgFetchDataAsycWithDirectSPCall.DoWork -= new DoWorkEventHandler(fetchDataAsycWithDirectSPCall_DoWork);
                    _bgFetchDataAsycWithDirectSPCall.Dispose();
                }
                ThirdPartyClientManager.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThirdPartyReportControl));
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
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
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grdThirdParty = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.IgnoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.lblThirdParty = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.txtDayLightSaving = new System.Windows.Forms.DateTimePicker();
            this.cmbThirdParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnGenerate = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.chkLstThirdPartyAccounts = new System.Windows.Forms.CheckedListBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkIncluedSent = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cmbDateType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbThirdPartyType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.chkSelectAllAUECs = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.grdExportToExcel = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.chkSelectAllAccounts = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.chkLstAuec = new System.Windows.Forms.CheckedListBox();
            this.chkL2Data = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cmbFormat = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdThirdParty)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncluedSent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDateType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdPartyType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAllAUECs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.ultraTabSharedControlsPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExportToExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAllAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkL2Data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFormat)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.grdThirdParty);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(941, 346);
            // 
            // grdThirdParty
            // 
            this.grdThirdParty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdThirdParty.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdThirdParty.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand1.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.grdThirdParty.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdThirdParty.DisplayLayout.GroupByBox.Hidden = true;
            this.grdThirdParty.DisplayLayout.MaxColScrollRegions = 1;
            this.grdThirdParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.Gold;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.grdThirdParty.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdThirdParty.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdThirdParty.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdThirdParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdThirdParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdThirdParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdThirdParty.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdThirdParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.grdThirdParty.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdThirdParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.Black;
            appearance4.ForeColor = System.Drawing.Color.Orange;
            this.grdThirdParty.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance5.ForeColor = System.Drawing.Color.Orange;
            this.grdThirdParty.DisplayLayout.Override.RowAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdThirdParty.DisplayLayout.Override.RowSelectorAppearance = appearance6;
            this.grdThirdParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdThirdParty.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.grdThirdParty.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdThirdParty.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdThirdParty.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance8.BackColor = System.Drawing.SystemColors.Info;
            this.grdThirdParty.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance8;
            this.grdThirdParty.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdThirdParty.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance9;
            this.grdThirdParty.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdThirdParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdThirdParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdThirdParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdThirdParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.grdThirdParty.Location = new System.Drawing.Point(0, 0);
            this.grdThirdParty.Name = "grdThirdParty";
            this.grdThirdParty.Size = new System.Drawing.Size(944, 330);
            this.grdThirdParty.TabIndex = 0;
            this.grdThirdParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdThirdParty.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdThirdParty_InitializeLayout);
            this.grdThirdParty.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdThirdParty_Error);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.IgnoreToolStripMenuItem,
            this.BackToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(170, 48);
            this.inboxControlStyler1.SetStyleSettings(this.contextMenuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // IgnoreToolStripMenuItem
            // 
            this.IgnoreToolStripMenuItem.Name = "IgnoreToolStripMenuItem";
            this.IgnoreToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.IgnoreToolStripMenuItem.Text = "Ignore Taxlot";
            this.IgnoreToolStripMenuItem.Click += new System.EventHandler(this.IgnoreToolStripMenuItem_Click);
            // 
            // BackToolStripMenuItem
            // 
            this.BackToolStripMenuItem.Name = "BackToolStripMenuItem";
            this.BackToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.BackToolStripMenuItem.Text = "Re-Activate Taxlot";
            this.BackToolStripMenuItem.Click += new System.EventHandler(this.BackToolStripMenuItem_Click);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(941, 346);
            // 
            // lblThirdParty
            // 
            this.lblThirdParty.AutoSize = true;
            this.lblThirdParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblThirdParty.Location = new System.Drawing.Point(431, 29);
            this.lblThirdParty.Name = "lblThirdParty";
            this.lblThirdParty.Size = new System.Drawing.Size(92, 15);
            this.lblThirdParty.TabIndex = 4;
            this.lblThirdParty.Text = "Third Party Name";
            // 
            // lblAccounts
            // 
            this.lblAccounts.AutoSize = true;
            this.lblAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblAccounts.Location = new System.Drawing.Point(9, 56);
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new System.Drawing.Size(77, 15);
            this.lblAccounts.TabIndex = 9;
            this.lblAccounts.Text = "Account Name";
            // 
            // txtDayLightSaving
            // 
            this.txtDayLightSaving.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtDayLightSaving.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDayLightSaving.Location = new System.Drawing.Point(120, 26);
            this.txtDayLightSaving.MaxDate = new System.DateTime(2050, 11, 8, 0, 0, 0, 0);
            this.txtDayLightSaving.MinDate = new System.DateTime(2006, 1, 1, 0, 0, 0, 0);
            this.txtDayLightSaving.Name = "txtDayLightSaving";
            this.txtDayLightSaving.Size = new System.Drawing.Size(88, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtDayLightSaving, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtDayLightSaving.TabIndex = 1;
            this.txtDayLightSaving.Value = new System.DateTime(2006, 12, 9, 0, 0, 0, 0);
            this.txtDayLightSaving.CloseUp += new System.EventHandler(this.txtDayLightSaving_CloseUp);
            this.txtDayLightSaving.ValueChanged += new System.EventHandler(this.txtDayLightSaving_ValueChanged);
            // 
            // cmbThirdParty
            // 
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbThirdParty.DisplayLayout.Appearance = appearance10;
            this.cmbThirdParty.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.cmbThirdParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbThirdParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbThirdParty.DisplayLayout.GroupByBox.Appearance = appearance11;
            appearance12.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbThirdParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance12;
            this.cmbThirdParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance13.BackColor2 = System.Drawing.SystemColors.Control;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbThirdParty.DisplayLayout.GroupByBox.PromptAppearance = appearance13;
            this.cmbThirdParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbThirdParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbThirdParty.DisplayLayout.Override.ActiveCellAppearance = appearance14;
            appearance15.BackColor = System.Drawing.SystemColors.Highlight;
            appearance15.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbThirdParty.DisplayLayout.Override.ActiveRowAppearance = appearance15;
            this.cmbThirdParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbThirdParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            this.cmbThirdParty.DisplayLayout.Override.CardAreaAppearance = appearance16;
            appearance17.BorderColor = System.Drawing.Color.Silver;
            appearance17.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbThirdParty.DisplayLayout.Override.CellAppearance = appearance17;
            this.cmbThirdParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbThirdParty.DisplayLayout.Override.CellPadding = 0;
            appearance18.BackColor = System.Drawing.SystemColors.Control;
            appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance18.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance18.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbThirdParty.DisplayLayout.Override.GroupByRowAppearance = appearance18;
            appearance19.TextHAlignAsString = "Left";
            this.cmbThirdParty.DisplayLayout.Override.HeaderAppearance = appearance19;
            this.cmbThirdParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbThirdParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            this.cmbThirdParty.DisplayLayout.Override.RowAppearance = appearance20;
            this.cmbThirdParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance21.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbThirdParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance21;
            this.cmbThirdParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbThirdParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbThirdParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbThirdParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbThirdParty.DropDownWidth = 0;
            this.cmbThirdParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbThirdParty.Location = new System.Drawing.Point(527, 26);
            this.cmbThirdParty.Name = "cmbThirdParty";
            this.cmbThirdParty.Size = new System.Drawing.Size(120, 21);
            this.cmbThirdParty.TabIndex = 5;
            this.cmbThirdParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbThirdParty.ValueChanged += new System.EventHandler(this.cmbThirdParty_ValueChanged);
            this.cmbThirdParty.TextChanged += new System.EventHandler(this.cmbThirdParty_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Location = new System.Drawing.Point(527, 106);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGenerate.BackgroundImage")));
            this.btnGenerate.Location = new System.Drawing.Point(527, 135);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(72, 23);
            this.btnGenerate.TabIndex = 20;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnView
            // 
            this.btnView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnView.BackgroundImage")));
            this.btnView.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnView.Location = new System.Drawing.Point(527, 77);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 23);
            this.btnView.TabIndex = 18;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // chkLstThirdPartyAccounts
            // 
            this.chkLstThirdPartyAccounts.CheckOnClick = true;
            this.chkLstThirdPartyAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkLstThirdPartyAccounts.FormattingEnabled = true;
            this.chkLstThirdPartyAccounts.Location = new System.Drawing.Point(9, 77);
            this.chkLstThirdPartyAccounts.Name = "chkLstThirdPartyAccounts";
            this.chkLstThirdPartyAccounts.Size = new System.Drawing.Size(199, 116);
            this.chkLstThirdPartyAccounts.Sorted = true;
            this.inboxControlStyler1.SetStyleSettings(this.chkLstThirdPartyAccounts, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkLstThirdPartyAccounts.TabIndex = 11;
            this.chkLstThirdPartyAccounts.SelectedValueChanged += new System.EventHandler(this.chkLstThirdPartyAccounts_SelectedValueChanged);
            this.chkLstThirdPartyAccounts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkLstThirdPartyAccounts_MouseUp);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkIncluedSent);
            this.groupBox1.Controls.Add(this.cmbDateType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbThirdPartyType);
            this.groupBox1.Controls.Add(this.chkSelectAllAUECs);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ultraTabControl1);
            this.groupBox1.Controls.Add(this.grdExportToExcel);
            this.groupBox1.Controls.Add(this.chkSelectAllAccounts);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkLstAuec);
            this.groupBox1.Controls.Add(this.chkL2Data);
            this.groupBox1.Controls.Add(this.cmbFormat);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbThirdParty);
            this.groupBox1.Controls.Add(this.txtDayLightSaving);
            this.groupBox1.Controls.Add(this.chkLstThirdPartyAccounts);
            this.groupBox1.Controls.Add(this.lblThirdParty);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnView);
            this.groupBox1.Controls.Add(this.lblAccounts);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(945, 577);
            this.groupBox1.TabIndex = 0;
            // 
            // chkIncluedSent
            // 
            this.chkIncluedSent.AutoSize = true;
            this.chkIncluedSent.Location = new System.Drawing.Point(649, 111);
            this.chkIncluedSent.Name = "chkIncluedSent";
            this.chkIncluedSent.Size = new System.Drawing.Size(84, 18);
            this.chkIncluedSent.TabIndex = 23;
            this.chkIncluedSent.Text = "Include Sent";
            this.chkIncluedSent.CheckedChanged += new System.EventHandler(this.chkIncluedSent_CheckedChanged);
            // 
            // cmbDateType
            // 
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbDateType.DisplayLayout.Appearance = appearance22;
            this.cmbDateType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.cmbDateType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbDateType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance23.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbDateType.DisplayLayout.GroupByBox.Appearance = appearance23;
            appearance24.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbDateType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance24;
            this.cmbDateType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbDateType.DisplayLayout.GroupByBox.PromptAppearance = appearance25;
            this.cmbDateType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbDateType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbDateType.DisplayLayout.Override.ActiveCellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.SystemColors.Highlight;
            appearance27.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbDateType.DisplayLayout.Override.ActiveRowAppearance = appearance27;
            this.cmbDateType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbDateType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance28.BackColor = System.Drawing.SystemColors.Window;
            this.cmbDateType.DisplayLayout.Override.CardAreaAppearance = appearance28;
            appearance29.BorderColor = System.Drawing.Color.Silver;
            appearance29.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbDateType.DisplayLayout.Override.CellAppearance = appearance29;
            this.cmbDateType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbDateType.DisplayLayout.Override.CellPadding = 0;
            appearance30.BackColor = System.Drawing.SystemColors.Control;
            appearance30.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance30.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbDateType.DisplayLayout.Override.GroupByRowAppearance = appearance30;
            appearance31.TextHAlignAsString = "Left";
            this.cmbDateType.DisplayLayout.Override.HeaderAppearance = appearance31;
            this.cmbDateType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbDateType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            this.cmbDateType.DisplayLayout.Override.RowAppearance = appearance32;
            this.cmbDateType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance33.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbDateType.DisplayLayout.Override.TemplateAddRowAppearance = appearance33;
            this.cmbDateType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbDateType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbDateType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbDateType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbDateType.DropDownWidth = 0;
            this.cmbDateType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbDateType.Location = new System.Drawing.Point(9, 26);
            this.cmbDateType.Name = "cmbDateType";
            this.cmbDateType.Size = new System.Drawing.Size(103, 21);
            this.cmbDateType.TabIndex = 0;
            this.cmbDateType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(214, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Third Party Type";
            // 
            // cmbThirdPartyType
            // 
            appearance34.BackColor = System.Drawing.SystemColors.Window;
            appearance34.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbThirdPartyType.DisplayLayout.Appearance = appearance34;
            this.cmbThirdPartyType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.cmbThirdPartyType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbThirdPartyType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance35.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance35.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance35.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbThirdPartyType.DisplayLayout.GroupByBox.Appearance = appearance35;
            appearance36.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbThirdPartyType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance36;
            this.cmbThirdPartyType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance37.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance37.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbThirdPartyType.DisplayLayout.GroupByBox.PromptAppearance = appearance37;
            this.cmbThirdPartyType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbThirdPartyType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance38.BackColor = System.Drawing.SystemColors.Window;
            appearance38.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbThirdPartyType.DisplayLayout.Override.ActiveCellAppearance = appearance38;
            appearance39.BackColor = System.Drawing.SystemColors.Highlight;
            appearance39.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbThirdPartyType.DisplayLayout.Override.ActiveRowAppearance = appearance39;
            this.cmbThirdPartyType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbThirdPartyType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance40.BackColor = System.Drawing.SystemColors.Window;
            this.cmbThirdPartyType.DisplayLayout.Override.CardAreaAppearance = appearance40;
            appearance41.BorderColor = System.Drawing.Color.Silver;
            appearance41.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbThirdPartyType.DisplayLayout.Override.CellAppearance = appearance41;
            this.cmbThirdPartyType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbThirdPartyType.DisplayLayout.Override.CellPadding = 0;
            appearance42.BackColor = System.Drawing.SystemColors.Control;
            appearance42.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance42.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance42.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbThirdPartyType.DisplayLayout.Override.GroupByRowAppearance = appearance42;
            appearance43.TextHAlignAsString = "Left";
            this.cmbThirdPartyType.DisplayLayout.Override.HeaderAppearance = appearance43;
            this.cmbThirdPartyType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbThirdPartyType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance44.BackColor = System.Drawing.SystemColors.Window;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            this.cmbThirdPartyType.DisplayLayout.Override.RowAppearance = appearance44;
            this.cmbThirdPartyType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance45.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbThirdPartyType.DisplayLayout.Override.TemplateAddRowAppearance = appearance45;
            this.cmbThirdPartyType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbThirdPartyType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbThirdPartyType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbThirdPartyType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbThirdPartyType.DropDownWidth = 0;
            this.cmbThirdPartyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbThirdPartyType.Location = new System.Drawing.Point(307, 28);
            this.cmbThirdPartyType.Name = "cmbThirdPartyType";
            this.cmbThirdPartyType.Size = new System.Drawing.Size(123, 21);
            this.cmbThirdPartyType.TabIndex = 3;
            this.cmbThirdPartyType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbThirdPartyType.ValueChanged += new System.EventHandler(this.cmbThirdPartyType_ValueChanged);
            this.cmbThirdPartyType.TextChanged += new System.EventHandler(this.cmbThirdPartyType_TextChanged);
            // 
            // chkSelectAllAUECs
            // 
            this.chkSelectAllAUECs.AutoSize = true;
            this.chkSelectAllAUECs.Location = new System.Drawing.Point(335, 56);
            this.chkSelectAllAUECs.Name = "chkSelectAllAUECs";
            this.chkSelectAllAUECs.Size = new System.Drawing.Size(68, 18);
            this.chkSelectAllAUECs.TabIndex = 16;
            this.chkSelectAllAUECs.Text = "Select All";
            this.chkSelectAllAUECs.CheckedChanged += new System.EventHandler(this.chkSelectAllAUECs_CheckedChanged);
            this.chkSelectAllAUECs.CheckStateChanged += new System.EventHandler(this.chkSelectAllAUECs_CheckStateChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 25;
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 199);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.grdThirdParty});
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(945, 372);
            this.ultraTabControl1.TabIndex = 22;
            ultraTab1.Key = "activetaxlots";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Active - Taxlots";
            ultraTab2.Key = "ignoredtaxlots";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Ignored - Taxlots";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Controls.Add(this.grdThirdParty);
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(941, 346);
            // 
            // grdExportToExcel
            // 
            appearance46.BackColor = System.Drawing.SystemColors.Window;
            appearance46.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdExportToExcel.DisplayLayout.Appearance = appearance46;
            this.grdExportToExcel.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdExportToExcel.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance47.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance47.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance47.BorderColor = System.Drawing.SystemColors.Window;
            this.grdExportToExcel.DisplayLayout.GroupByBox.Appearance = appearance47;
            appearance48.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdExportToExcel.DisplayLayout.GroupByBox.BandLabelAppearance = appearance48;
            this.grdExportToExcel.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdExportToExcel.DisplayLayout.GroupByBox.Hidden = true;
            appearance49.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance49.BackColor2 = System.Drawing.SystemColors.Control;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance49.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdExportToExcel.DisplayLayout.GroupByBox.PromptAppearance = appearance49;
            this.grdExportToExcel.DisplayLayout.MaxColScrollRegions = 1;
            this.grdExportToExcel.DisplayLayout.MaxRowScrollRegions = 1;
            appearance50.BackColor = System.Drawing.SystemColors.Window;
            appearance50.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdExportToExcel.DisplayLayout.Override.ActiveCellAppearance = appearance50;
            appearance51.BackColor = System.Drawing.SystemColors.Highlight;
            appearance51.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExportToExcel.DisplayLayout.Override.ActiveRowAppearance = appearance51;
            this.grdExportToExcel.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdExportToExcel.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance52.BackColor = System.Drawing.SystemColors.Window;
            this.grdExportToExcel.DisplayLayout.Override.CardAreaAppearance = appearance52;
            appearance53.BorderColor = System.Drawing.Color.Silver;
            appearance53.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdExportToExcel.DisplayLayout.Override.CellAppearance = appearance53;
            this.grdExportToExcel.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdExportToExcel.DisplayLayout.Override.CellPadding = 0;
            appearance54.BackColor = System.Drawing.SystemColors.Control;
            appearance54.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance54.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance54.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance54.BorderColor = System.Drawing.SystemColors.Window;
            this.grdExportToExcel.DisplayLayout.Override.GroupByRowAppearance = appearance54;
            appearance55.TextHAlignAsString = "Left";
            this.grdExportToExcel.DisplayLayout.Override.HeaderAppearance = appearance55;
            this.grdExportToExcel.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdExportToExcel.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance56.BackColor = System.Drawing.SystemColors.Window;
            appearance56.BorderColor = System.Drawing.Color.Silver;
            this.grdExportToExcel.DisplayLayout.Override.RowAppearance = appearance56;
            this.grdExportToExcel.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance57.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdExportToExcel.DisplayLayout.Override.TemplateAddRowAppearance = appearance57;
            this.grdExportToExcel.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdExportToExcel.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdExportToExcel.Location = new System.Drawing.Point(788, 174);
            this.grdExportToExcel.Name = "grdExportToExcel";
            this.grdExportToExcel.Size = new System.Drawing.Size(38, 33);
            this.grdExportToExcel.TabIndex = 21;
            this.grdExportToExcel.Text = "ultraGrid1";
            this.grdExportToExcel.Visible = false;
            // 
            // chkSelectAllAccounts
            // 
            this.chkSelectAllAccounts.AutoSize = true;
            this.chkSelectAllAccounts.Location = new System.Drawing.Point(95, 56);
            this.chkSelectAllAccounts.Name = "chkSelectAllAccounts";
            this.chkSelectAllAccounts.Size = new System.Drawing.Size(68, 18);
            this.chkSelectAllAccounts.TabIndex = 10;
            this.chkSelectAllAccounts.Text = "Select All";
            this.chkSelectAllAccounts.CheckedChanged += new System.EventHandler(this.chkSelectAllAccounts_CheckedChanged);
            this.chkSelectAllAccounts.CheckStateChanged += new System.EventHandler(this.chkSelectAllAccounts_CheckStateChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(255, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "AUEC";
            // 
            // chkLstAuec
            // 
            this.chkLstAuec.CheckOnClick = true;
            this.chkLstAuec.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkLstAuec.FormattingEnabled = true;
            this.chkLstAuec.Location = new System.Drawing.Point(255, 77);
            this.chkLstAuec.Name = "chkLstAuec";
            this.chkLstAuec.Size = new System.Drawing.Size(204, 116);
            this.chkLstAuec.Sorted = true;
            this.inboxControlStyler1.SetStyleSettings(this.chkLstAuec, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkLstAuec.TabIndex = 17;
            this.chkLstAuec.SelectedIndexChanged += new System.EventHandler(this.chkLstAuec_SelectedIndexChanged);
            this.chkLstAuec.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chkLstAuec_MouseMove);
            this.chkLstAuec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkLstAuec_MouseUp);
            // 
            // chkL2Data
            // 
            this.chkL2Data.AutoSize = true;
            this.chkL2Data.Location = new System.Drawing.Point(649, 82);
            this.chkL2Data.Name = "chkL2Data";
            this.chkL2Data.Size = new System.Drawing.Size(82, 18);
            this.chkL2Data.TabIndex = 8;
            this.chkL2Data.Text = "Level2 Data";
            this.chkL2Data.CheckedChanged += new System.EventHandler(this.chkL2Data_CheckedChanged);
            this.chkL2Data.CheckStateChanged += new System.EventHandler(this.chkL2Data_CheckStateChanged);
            // 
            // cmbFormat
            // 
            appearance58.BackColor = System.Drawing.SystemColors.Window;
            appearance58.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbFormat.DisplayLayout.Appearance = appearance58;
            this.cmbFormat.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.cmbFormat.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbFormat.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance59.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance59.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance59.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance59.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFormat.DisplayLayout.GroupByBox.Appearance = appearance59;
            appearance60.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFormat.DisplayLayout.GroupByBox.BandLabelAppearance = appearance60;
            this.cmbFormat.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance61.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance61.BackColor2 = System.Drawing.SystemColors.Control;
            appearance61.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance61.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFormat.DisplayLayout.GroupByBox.PromptAppearance = appearance61;
            this.cmbFormat.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbFormat.DisplayLayout.MaxRowScrollRegions = 1;
            appearance62.BackColor = System.Drawing.SystemColors.Window;
            appearance62.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbFormat.DisplayLayout.Override.ActiveCellAppearance = appearance62;
            appearance63.BackColor = System.Drawing.SystemColors.Highlight;
            appearance63.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbFormat.DisplayLayout.Override.ActiveRowAppearance = appearance63;
            this.cmbFormat.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbFormat.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance64.BackColor = System.Drawing.SystemColors.Window;
            this.cmbFormat.DisplayLayout.Override.CardAreaAppearance = appearance64;
            appearance65.BorderColor = System.Drawing.Color.Silver;
            appearance65.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbFormat.DisplayLayout.Override.CellAppearance = appearance65;
            this.cmbFormat.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbFormat.DisplayLayout.Override.CellPadding = 0;
            appearance66.BackColor = System.Drawing.SystemColors.Control;
            appearance66.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance66.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance66.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFormat.DisplayLayout.Override.GroupByRowAppearance = appearance66;
            appearance67.TextHAlignAsString = "Left";
            this.cmbFormat.DisplayLayout.Override.HeaderAppearance = appearance67;
            this.cmbFormat.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbFormat.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance68.BackColor = System.Drawing.SystemColors.Window;
            appearance68.BorderColor = System.Drawing.Color.Silver;
            this.cmbFormat.DisplayLayout.Override.RowAppearance = appearance68;
            this.cmbFormat.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance69.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbFormat.DisplayLayout.Override.TemplateAddRowAppearance = appearance69;
            this.cmbFormat.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFormat.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbFormat.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbFormat.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFormat.DropDownWidth = 0;
            this.cmbFormat.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbFormat.Location = new System.Drawing.Point(724, 26);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(120, 21);
            this.cmbFormat.TabIndex = 7;
            this.cmbFormat.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbFormat.ValueChanged += new System.EventHandler(this.cmbFormat_ValueChanged);
            this.cmbFormat.TextChanged += new System.EventHandler(this.cmbFormat_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(649, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Format Type";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 555);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(945, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ThirdPartyReportControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "ThirdPartyReportControl";
            this.Size = new System.Drawing.Size(945, 577);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ThirdPartyReportControl_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdThirdParty)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncluedSent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDateType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdPartyType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAllAUECs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ultraTabSharedControlsPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExportToExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAllAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkL2Data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFormat)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #endregion Wizard Stuff

        #region Declared Variables & Properties

        private int _companyID = int.MinValue;
        private int _thirdPartyID = int.MinValue;
        private int _fileFormatID = int.MinValue;

        private int _thirdPartyTypeID = int.MinValue;

        private int _userID = int.MinValue;
        private int _fileType = int.MinValue;
        private string _startupPath = Application.StartupPath;

        // TO RESTORE THE PREVIOUS VALUES
        // PREVIOUS DATE
        DateTime _getprevDate;
        //PREVIOUS Third Party COMBO VALUE
        int _cmbThirdPartyValue = 0;
        //Third Party COMBO TEXT
        string _cmbThirdPartyText = string.Empty;
        //PREVIOUS Third Party Type COMBO VALUE
        int _cmbThirdPartyTypeValue = 0;
        //Third Party Type COMBO TEXT
        string _cmbThirdPartyTypeText = string.Empty;
        // Format Type Text
        string _cmbFormatText = string.Empty;
        // Format Type Value
        int _cmbFormatVal = 0;
        // List collection to store the current state of checkedlistbox
        List<int> _thirdPartyMainAccountColl = new List<int>();
        List<int> _AUECColl = new List<int>();
        // BOOL VARIABLE USED ON Third Party COMBO
        bool _blncmbvaluechange = false;

        bool _blncmbThirdPartyTypeValueChange = false;
        // BOOL VARIABLE USED ON File Format COMBO
        bool _blncmbFormatvaluechange = false;
        // 
        bool _blnChklistchange = false;
        bool _blnChkAllAccountChange = false;
        bool _blnnoForDate = false;
        bool _test = false;

        bool _blnChkL2Change = false;
        bool _chkL2Value = false;

        bool _chkIncluedSent = false;

        // boolean variable to check whether some changes are made in the grd data
        bool _blnchange = false;
        bool _blnSelectAllAUECChange = false;
        // on form closing check whether want to save the data ?
        int _mess = 0;

        //Date Type
        int _dateType = 0;
        //int _hashCode = 0;
        CheckState _chkStateForAllAccounts = CheckState.Unchecked;

        CheckState _chkStateForAllAUECs = CheckState.Unchecked;
        // xml is used to set order of taxlot state
        List<string> _thirdPartyTaxlotStateSortingOrder = new List<string>();

        #endregion Declared Variables & Properties

        private const string HEADCOL_GROUPENDS = "GROUPENDS";
        private const string HEADCOL_PBUNIQUEID = "PBUniqueID";
        private const string HEADCOL_ROWHEADER = "RowHeader";
        private const string HEADCOL_GroupAllocationReq = "GroupAllocationReq";
        private const string HEADCOL_FILEHEADER = "FileHeader";
        private const string HEADCOL_FILEFOOTER = "FileFooter";

        private const string HEADCOL_TAXLOTSTATE = "TAXLOTSTATE";

        private const string HEADCOL_EntityID = "EntityID";

        private const string HEADCOL_CheckBox = "checkBox";
        private const string HEADCOL_ALLOCQTY = "ALLOCQTY";
        private const string HEADCOL_INTERNALNETNOTIONAL = "InternalNetNotional";
        private const string HEADCOL_INTERNALGROSSAMOUNT = "InternalGrossAmount";
        private const string HEADCOL_ISCAPCHANGEREQ = "IsCaptionChangeRequired";
        private const string HEADCOL_XMLMAINTAG = "XMLMAINTAG";
        private const string HEADCOL_XMLCHILDTAG = "XMLCHILDTAG";
        private const string HEADCOL_XMLFOOTERMAINTAG = "XMLFOOTERMAINTAG";
        private const string HEADCOL_XMLFOOTERCHILDTAG = "XMLFOOTERCHILDTAG";
        private const string HEADCOL_XMLHEADERMAINTAG = "XMLHEADERMAINTAG";
        private const string HEADCOL_XMLHEADERCHILDTAG = "XMLHEADERCHILDTAG";


        private const int TYPE_VENDOR = 2;


        #region Public Properties

        //Property to set the companyID.
        public int CompanyID
        {
            set { _companyID = value; }
        }

        //Property to set the ThirdPartyID.
        public int ThirdPartyID
        {
            set { _thirdPartyID = value; }
        }

        //Propety to set the UserID.
        public int CompanyUserID
        {
            set { _userID = value; }
        }

        #endregion Public Properties

        #region Private Functions


        private void BindThirdPartyType()
        {
            try
            {
                Prana.BusinessObjects.ThirdPartyTypes thirdPartyTypes = new BusinessObjects.ThirdPartyTypes();
                thirdPartyTypes.AddAllThirdParties(ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyTypes());
                thirdPartyTypes.Insert(0, new Prana.BusinessObjects.ThirdPartyType(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbThirdPartyType.DataSource = null;
                cmbThirdPartyType.DataSource = thirdPartyTypes;
                cmbThirdPartyType.DisplayMember = "ThirdPartyTypeName";
                cmbThirdPartyType.ValueMember = "ThirdPartyTypeID";
                cmbThirdPartyType.Text = "PrimeBrokerClearer";
                ColumnsCollection columns = cmbThirdPartyType.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    // If the column is not "ThirdPartyName" Column , then it is set as hidden. 
                    if (column.Key != "ThirdPartyTypeName")
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        // The column headers are set as invisible.
                        cmbThirdPartyType.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Method to populate the Third party combo with the available third parties of the selected company.
        /// </summary>
        private void BindThirdParties()
        {
            try
            {

                //cmbThirdParty.DataSource = null;

                if (_companyID != int.MinValue && _thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                {
                    // enable third parties drop down
                    cmbThirdParty.Enabled = true;
                    //For a valid companyID, the thirdparties are fetched from the database.
                    Prana.BusinessObjects.ThirdParties thirdParties = new Prana.BusinessObjects.ThirdParties();  
                    thirdParties.AddRange(ThirdPartyClientManager.ServiceInnerChannel.GetCompanyThirdParties_DayEnd(_companyID));
                    // an extra row before the available data is added to add the "Select" .
                    thirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                    //The combo is assisgned the valuemember , display member and datasource as the above fetched collection.
                    this.cmbThirdParty.ValueChanged -= new System.EventHandler(this.cmbThirdParty_ValueChanged);
                    this.cmbThirdParty.TextChanged -= new System.EventHandler(this.cmbThirdParty_TextChanged);
                    cmbThirdParty.DataSource = null;
                    this.cmbThirdParty.ValueChanged += new System.EventHandler(this.cmbThirdParty_ValueChanged);
                    this.cmbThirdParty.TextChanged += new System.EventHandler(this.cmbThirdParty_TextChanged);
                    cmbThirdParty.DataSource = thirdParties;
                    cmbThirdParty.ValueMember = "CompanyThirdPartyID";
                    cmbThirdParty.DisplayMember = "ThirdPartyName";
                    //the combo selected text is set to select by default evry time it is freshly populated.
                    cmbThirdParty.Text = ApplicationConstants.C_COMBO_SELECT;
                    // While iterating through each column in in the datasource assigned to the Combo..
                    // certain properties are set for certain columns.
                    ColumnsCollection columns = cmbThirdParty.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        // If the column is not "ThirdPartyName" Column , then it is set as hidden. 
                        if (column.Key != "ThirdPartyName")
                        {
                            column.Hidden = true;
                        }
                        else
                        {
                            // The column headers are set as invisible.
                            cmbThirdParty.DisplayLayout.Bands[0].ColHeadersVisible = false;
                        }
                    }
                }
                else if (_companyID != int.MinValue && (_thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker || _thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties))
                {
                    //For a valid companyID, the thirdparties are fetched from the database.
                    Prana.BusinessObjects.ThirdParties thirdParties = new BusinessObjects.ThirdParties();
                    thirdParties.AddRange(ThirdPartyClientManager.ServiceInnerChannel.GetCompanyThirdPartyTypeParty(_companyID, _thirdPartyTypeID));
                    // an extra row before the available data is added to add the "Select" .
                    thirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                    //The combo is assisgned the valuemember , display member and datasource as the above fetched collection.
                    this.cmbThirdParty.ValueChanged -= new System.EventHandler(this.cmbThirdParty_ValueChanged);
                    this.cmbThirdParty.TextChanged -= new System.EventHandler(this.cmbThirdParty_TextChanged);
                    cmbThirdParty.DataSource = null;
                    this.cmbThirdParty.ValueChanged += new System.EventHandler(this.cmbThirdParty_ValueChanged);
                    this.cmbThirdParty.TextChanged += new System.EventHandler(this.cmbThirdParty_TextChanged);
                    cmbThirdParty.DataSource = thirdParties;
                    cmbThirdParty.ValueMember = "CompanyThirdPartyID";
                    cmbThirdParty.DisplayMember = "ThirdPartyName";
                    //the combo selected text is set to select by default evry time it is freshly populated.
                    cmbThirdParty.Text = ApplicationConstants.C_COMBO_SELECT;
                    // While iterating through each column in in the datasource assigned to the Combo..
                    // certain properties are set for certain columns.
                    ColumnsCollection columns = cmbThirdParty.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        // If the column is not "ThirdPartyName" Column , then it is set as hidden. 
                        if (column.Key != "ThirdPartyName")
                        {
                            column.Hidden = true;
                        }
                        else
                        {
                            // The column headers are set as invisible.
                            cmbThirdParty.DisplayLayout.Bands[0].ColHeadersVisible = false;
                        }
                    }
                }
                else
                {
                    Prana.BusinessObjects.ThirdParties thirdParties = new Prana.BusinessObjects.ThirdParties();

                    // an extra row before the available data is added to add the "Select" .
                    thirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                    this.cmbThirdParty.ValueChanged -= new System.EventHandler(this.cmbThirdParty_ValueChanged);
                    this.cmbThirdParty.TextChanged -= new System.EventHandler(this.cmbThirdParty_TextChanged);
                    cmbThirdParty.DataSource = null;
                    this.cmbThirdParty.ValueChanged += new System.EventHandler(this.cmbThirdParty_ValueChanged);
                    this.cmbThirdParty.TextChanged += new System.EventHandler(this.cmbThirdParty_TextChanged);
                    cmbThirdParty.DataSource = thirdParties;
                    cmbThirdParty.ValueMember = "CompanyThirdPartyID";
                    cmbThirdParty.DisplayMember = "ThirdPartyName";
                    cmbThirdParty.Text = ApplicationConstants.C_COMBO_SELECT;

                    ColumnsCollection columns = cmbThirdParty.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        // If the column is not "FileFormatName" Column , then it is set as hidden. 
                        if (column.Key != "FileFormatName")
                        {
                            column.Hidden = true;
                        }
                        else
                        {
                            // The column headers are set as invisible.
                            cmbFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindWorkingAUEC()
        {
            try
            {
                if (_companyID != int.MinValue)
                {
                    AUECs companyWorkingAUECs = ThirdPartyManager.GetCompanyWorkingAUECs(_companyID);

                    if (companyWorkingAUECs.Count > 0)
                    {
                        chkLstAuec.DataSource = null;
                        chkLstAuec.DataSource = companyWorkingAUECs;
                        chkLstAuec.ValueMember = "AUECID";
                        chkLstAuec.DisplayMember = "DisplayName";
                    }
                    else
                    {
                        chkLstAuec.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// method to populate the checklistbox with permitted ThirdParty Accounts for the selected third partyID .
        /// </summary>
        private void BindThirdPartyAccounts()
        {
            try
            {
                if (_thirdPartyID != int.MinValue && _thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                {
                    //If the valid third party is selected , then, the accounts permitted to it are fetched from the database.
                    Prana.BusinessObjects.ThirdPartyPermittedAccounts thirdPartyPermittedAccounts = new BusinessObjects.ThirdPartyPermittedAccounts();
                    thirdPartyPermittedAccounts.AddRange(ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyPermittedAccounts(_thirdPartyID));
                    Dictionary<int, string> dictUserAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();

                    // Added by Sunil Sharma, required as per http://jira.nirvanasolutions.com:8080/browse/PRANA-12432

                    List<Prana.BusinessObjects.ThirdPartyPermittedAccount> toRemove = new List<Prana.BusinessObjects.ThirdPartyPermittedAccount>();

                    foreach (Prana.BusinessObjects.ThirdPartyPermittedAccount account in thirdPartyPermittedAccounts)
                    {
                        if (!dictUserAccounts.ContainsKey(account.CompanyAccountID))
                        {
                            toRemove.Add(account);
                        }
                    }

                    foreach (Prana.BusinessObjects.ThirdPartyPermittedAccount account in toRemove)
                    {
                        thirdPartyPermittedAccounts.Remove(account);
                    }
                    //The fetched collection of thirdpartyaccounts is assigned as datasource to the checklistbox.
                    if (thirdPartyPermittedAccounts.Count > 0)
                    {
                        chkLstThirdPartyAccounts.DataSource = null;
                        chkLstThirdPartyAccounts.DataSource = thirdPartyPermittedAccounts;
                        chkLstThirdPartyAccounts.ValueMember = "CompanyAccountID";
                        chkLstThirdPartyAccounts.DisplayMember = "AccountName";
                    }
                    else
                    {
                        //MessageBox.Show("No Account mapped to the selected Third Party.", "Flat File Generation Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        chkLstThirdPartyAccounts.DataSource = null;
                        //return;
                    }
                }
                else if (_thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker || _thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties)
                {
                    Prana.BusinessObjects.ThirdPartyPermittedAccounts thirdPartyPermittedAccounts = new BusinessObjects.ThirdPartyPermittedAccounts();
                    thirdPartyPermittedAccounts.AddRange(ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyAccounts(_companyID, _userID));
                    //The fetched collection of thirdpartyaccounts is assigned as datasource to the checklistbox.
                    if (thirdPartyPermittedAccounts.Count > 0)
                    {
                        chkLstThirdPartyAccounts.DataSource = null;
                        chkLstThirdPartyAccounts.DataSource = thirdPartyPermittedAccounts;
                        chkLstThirdPartyAccounts.ValueMember = "CompanyAccountID";
                        chkLstThirdPartyAccounts.DisplayMember = "AccountName";
                    }
                    else
                    {
                        //MessageBox.Show("No Account mapped to the selected Third Party.", "Flat File Generation Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        chkLstThirdPartyAccounts.DataSource = null;
                        //return;
                    }

                }
                else
                {
                    chkLstThirdPartyAccounts.DataSource = null;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// method to populate the checklistbox with permitted ThirdParty Accounts for the selected third partyID .
        /// </summary>
        private void BindXSLTFormatType()
        {
            try
            {
                if (_thirdPartyID != int.MinValue && _thirdPartyTypeID != int.MinValue)
                {

                    if (_thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                    {
                        //If the valid third party is selected , then, the Format assigned to it are fetched from the database.                    
                        Prana.BusinessObjects.ThirdPartyFileFormats thirdPatyFileFormats = new BusinessObjects.ThirdPartyFileFormats();
                        thirdPatyFileFormats.AddRange(ThirdPartyClientManager.ServiceInnerChannel.GetCompanyThirdPartyFileFormats(_thirdPartyID));

                        // an extra row before the available data is added to add the "Select" .
                        thirdPatyFileFormats.Insert(0, new Prana.BusinessObjects.ThirdPartyFileFormat(int.MinValue, ApplicationConstants.C_COMBO_SELECT));

                        // While iterating through each column in in the datasource assigned to the Combo..
                        //certain properties are set for certain columns.             
                        //The fetched collection of FileOfrmatCollection is assigned as datasource to the combo box.
                        cmbFormat.DataSource = null;
                        cmbFormat.DataSource = thirdPatyFileFormats;
                        cmbFormat.ValueMember = "FileFormatId";
                        cmbFormat.DisplayMember = "FileFormatName";
                        cmbFormat.Text = ApplicationConstants.C_COMBO_SELECT;

                        ColumnsCollection columns = cmbFormat.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            // If the column is not "FileFormatName" Column , then it is set as hidden. 
                            if (column.Key != "FileFormatName")
                            {
                                column.Hidden = true;
                            }
                            else
                            {
                                // The column headers are set as invisible.
                                cmbFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            }
                        }
                    }
                    else if (_thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker || _thirdPartyTypeID == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties)
                    {
                        //If the valid third party is selected , then, the Format assigned to it are fetched from the database.                    
                        Prana.BusinessObjects.ThirdPartyFileFormats thirdPatyFileFormats = new BusinessObjects.ThirdPartyFileFormats();
                        thirdPatyFileFormats.AddRange(ThirdPartyClientManager.ServiceInnerChannel.GetCompanyThirdPartyTypeFileFormats(_thirdPartyID));

                        // an extra row before the available data is added to add the "Select" .
                        thirdPatyFileFormats.Insert(0, new Prana.BusinessObjects.ThirdPartyFileFormat(int.MinValue, ApplicationConstants.C_COMBO_SELECT));

                        // While iterating through each column in in the datasource assigned to the Combo..
                        //certain properties are set for certain columns.             
                        //The fetched collection of FileOfrmatCollection is assigned as datasource to the combo box.
                        cmbFormat.DataSource = null;
                        cmbFormat.DataSource = thirdPatyFileFormats;
                        cmbFormat.ValueMember = "FileFormatId";
                        cmbFormat.DisplayMember = "FileFormatName";
                        cmbFormat.Text = ApplicationConstants.C_COMBO_SELECT;

                        ColumnsCollection columns = cmbFormat.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            // If the column is not "FileFormatName" Column , then it is set as hidden. 
                            if (column.Key != "FileFormatName")
                            {
                                column.Hidden = true;
                            }
                            else
                            {
                                // The column headers are set as invisible.
                                cmbFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            }
                        }
                    }
                    else
                    {
                        Prana.BusinessObjects.ThirdPartyFileFormats thirdPatyFileFormats = new Prana.BusinessObjects.ThirdPartyFileFormats();

                        // an extra row before the available data is added to add the "Select" .
                        thirdPatyFileFormats.Insert(0, new Prana.BusinessObjects.ThirdPartyFileFormat(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                        cmbFormat.DataSource = null;
                        cmbFormat.DataSource = thirdPatyFileFormats;
                        cmbFormat.ValueMember = "FileFormatId";
                        cmbFormat.DisplayMember = "FileFormatName";
                        cmbFormat.Text = ApplicationConstants.C_COMBO_SELECT;

                        ColumnsCollection columns = cmbFormat.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            // If the column is not "FileFormatName" Column , then it is set as hidden. 
                            if (column.Key != "FileFormatName")
                            {
                                column.Hidden = true;
                            }
                            else
                            {
                                // The column headers are set as invisible.
                                cmbFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            }
                        }
                    }
                }
                else
                {
                    Prana.BusinessObjects.ThirdPartyFileFormats thirdPatyFileFormats = new Prana.BusinessObjects.ThirdPartyFileFormats();

                    // an extra row before the available data is added to add the "Select" .
                    thirdPatyFileFormats.Insert(0, new Prana.BusinessObjects.ThirdPartyFileFormat(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                    cmbFormat.DataSource = null;
                    cmbFormat.DataSource = thirdPatyFileFormats;
                    cmbFormat.ValueMember = "FileFormatId";
                    cmbFormat.DisplayMember = "FileFormatName";
                    cmbFormat.Text = ApplicationConstants.C_COMBO_SELECT;

                    ColumnsCollection columns = cmbFormat.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        // If the column is not "FileFormatName" Column , then it is set as hidden. 
                        if (column.Key != "FileFormatName")
                        {
                            column.Hidden = true;
                        }
                        else
                        {
                            // The column headers are set as invisible.
                            cmbFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The usercontrol load method contains the Third party Combo binding method for it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThirdPartyReportControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    //This sets the initial value of the date to the current and the maximum date for whhc the data can be generated to current date.
                    txtDayLightSaving.Value = System.DateTime.Now;
                    //txtDayLightSaving.MaxDate = System.DateTime.Now;

                    //Saving of ThirrdPartyFileFormat XSLTs to startup folder from DB
                    FileAndDbSyncManager.SyncFileWithDataBase(_startupPath, ApplicationConstants.MappingFileType.ThirdPartyXSLT);
                    //ThirdPartyManager.GetThirdPartyFileFormatXSLTsFromDB(_startupPath);
                    // Bind third party types
                    BindThirdPartyType();
                    BindDateType();

                    //This populates the thirdparty combo with the available thirdparty for the company. 
                    //BindThirdParties();
                    BindWorkingAUEC();
                    SetAUECChecked();
                    //Initially, the 'Save' and 'Generate' button are set to disable state,since, the user cannot save or generate the data without viewing it first.
                    btnSave.Enabled = false;
                    btnGenerate.Enabled = false;
                    SetDefaultFilters();
                    LoadThirdPartyTaxlotStateSortXML();
                    CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY);
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnView.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnView.ForeColor = System.Drawing.Color.White;
                btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnView.UseAppStyling = false;
                btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGenerate.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnGenerate.ForeColor = System.Drawing.Color.White;
                btnGenerate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGenerate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGenerate.UseAppStyling = false;
                btnGenerate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindDateType()
        {
            try
            {
                BindingList<string> lsDateType = new BindingList<string>();
                lsDateType.Add("Process Date");
                lsDateType.Add("Trade Date");
                cmbDateType.DataSource = null;
                cmbDateType.DataSource = lsDateType;
                cmbDateType.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbDateType.Text = "Process Date";
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void LoadThirdPartyTaxlotStateSortXML()
        {
            try
            {
                string filePath = Application.StartupPath + @"\MappingFiles\ThirdPartyXML\ThirdPartyTaxlotStateSortPreference.xml";
                if (File.Exists(filePath))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);

                    foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                    {
                        _thirdPartyTaxlotStateSortingOrder.Add(xmlNode.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The method is used to validate the thirdparty report selected criteria before further process.
        /// </summary>
        /// <returns></returns>
        private bool ValidateThirdPartyReport()
        {
            bool IsValid = true;
            try
            {
                errorProvider1.SetError(txtDayLightSaving, "");
                errorProvider1.SetError(cmbThirdPartyType, "");
                errorProvider1.SetError(cmbThirdParty, "");
                errorProvider1.SetError(cmbFormat, "");
                errorProvider1.SetError(chkLstThirdPartyAccounts, "");

                if (txtDayLightSaving.Text.Equals(string.Empty))
                {
                    errorProvider1.SetError(txtDayLightSaving, "Please set the  date for which you want to generate the Third Party Report data!");
                    IsValid = false;
                    return IsValid;
                }
                else if (Convert.ToInt32(cmbThirdPartyType.Value).Equals(int.MinValue))
                {
                    errorProvider1.SetError(cmbThirdPartyType, "Please select the Third Party Type for which you want to generate the Data!");
                    IsValid = false;
                    return IsValid;
                }
                else if (Convert.ToInt32(cmbThirdParty.Value).Equals(int.MinValue))
                {
                    errorProvider1.SetError(cmbThirdParty, "Please select the Third Party for which you want to generate the Data!");
                    IsValid = false;
                    return IsValid;
                }
                else if (Convert.ToInt32(cmbFormat.Value).Equals(int.MinValue))
                {
                    errorProvider1.SetError(cmbFormat, "Please select the Format!");
                    IsValid = false;
                    return IsValid;
                }
                else if (chkLstThirdPartyAccounts.Items.Count == 0)
                {
                    errorProvider1.SetError(chkLstThirdPartyAccounts, "No accounts are available for the selected Third Party.Please contact to the administrator!");
                    IsValid = false;
                    return IsValid;
                }
                else if (chkLstThirdPartyAccounts.CheckedItems.Count == 0)
                {
                    errorProvider1.SetError(chkLstThirdPartyAccounts, "Please select the Main Account(s) for which you want to generate the data.");
                    IsValid = false;
                    return IsValid;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return IsValid;
        }

        /// <summary>
        /// The method is used to set the properties of the data displayed in grid.
        /// </summary>
        private void SetColumnHidden()
        {
            try
            {
                int bandCount = grdThirdParty.DisplayLayout.Bands.Count;
                // This sets the selected columns as hidden in the grid.
                ColumnsCollection columns = grdThirdParty.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (ColumnExists(column.Key))
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        //    column.Hidden = false;
                        //    //column.Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Default;
                        //    //column.Header.Appearance.TextVAlign = Infragistics.Win.VAlign.Default;
                        //    //column.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Default;
                        //    //column.CellAppearance.TextVAlign = Infragistics.Win.VAlign.Default;
                        //    column.CellClickAction = CellClickAction.Edit;
                        //    column.CellActivation = Activation.AllowEdit;
                        column.CellActivation = Activation.NoEdit;
                    }
                }
                if (bandCount.Equals(2))
                {
                    ColumnsCollection childColumns = grdThirdParty.DisplayLayout.Bands[1].Columns;
                    foreach (UltraGridColumn column in childColumns)
                    {
                        if (ColumnExists(column.Key))
                        {
                            column.Hidden = true;
                        }
                        else
                        {
                            column.CellActivation = Activation.NoEdit;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// The method is used to fill the collection with the required data.
        /// </summary>
        private async Task<Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection> FillData()
        {
            //ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetails = null;
            Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetailCollection = null;
            try
            {
                Dictionary<Int64, int> pbUniqueIDDict = new Dictionary<Int64, int>();
                DateTime inputDate = txtDayLightSaving.Value;
                //int dateType = cmbDateType.Value.ToString().ToUpper() == "PROCESS DATE" ? 0 : 1;
                _taxLotWithStateDict.Clear();
                _taxLotIgnoreStateDict.Clear();
                _deletedToIgnoreDict.Clear();
                Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                if (_thirdPartyID != int.MinValue)
                {
                    thirdPartyFlatFileDetailCollection = new Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection();
                    if (!thirdPartyFileFormat.ExportOnly)
                    {
                        if (_chkL2Value.Equals(true))
                        {
                            if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, true, _auecIDStringBuilder, false, 0, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                            else if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker || _cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, true, _auecIDStringBuilder, false, 1, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                        }
                        else
                        {
                            if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, false, _auecIDStringBuilder, false, 0, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                            else if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker || _cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, false, _auecIDStringBuilder, false, 1, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                        }
                    }
                    else
                    {
                        if (_chkL2Value.Equals(true))
                        {
                            if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, true, _auecIDStringBuilder, true, 0, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                            else if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker || _cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, true, _auecIDStringBuilder, true, 1, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                        }
                        else
                        {
                            if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, false, _auecIDStringBuilder, true, 0, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                            else if (_cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker || _cmbThirdPartyTypeValue == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties)
                            {
                                thirdPartyFlatFileDetailCollection = await ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetailsFiltered(_thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, false, _auecIDStringBuilder, true, 1, _dateType, _fileFormatID, _chkIncluedSent);
                            }
                        }
                    }
                    if (thirdPartyFlatFileDetailCollection.Count == 0)
                    {
                        //MessageBox.Show("No data available for the selected account(s)","Information");                                
                    }
                    else
                    {
                        //for each row of data obtained for the selected account details, further details are filled .
                        foreach (Prana.BusinessObjects.ThirdPartyFlatFileDetail thirdPartyFlatFileDetail in thirdPartyFlatFileDetailCollection)
                        {

                            if (pbUniqueIDDict.ContainsKey(thirdPartyFlatFileDetail.PBUniqueID))
                            {
                                thirdPartyFlatFileDetail.AllocationSeqNo = pbUniqueIDDict[thirdPartyFlatFileDetail.PBUniqueID];

                                thirdPartyFlatFileDetail.AllocationSeqNo = thirdPartyFlatFileDetail.AllocationSeqNo + 1;
                                pbUniqueIDDict[thirdPartyFlatFileDetail.PBUniqueID] = thirdPartyFlatFileDetail.AllocationSeqNo;

                            }
                            else
                            {
                                thirdPartyFlatFileDetail.AllocationSeqNo = 0;
                                pbUniqueIDDict.Add(thirdPartyFlatFileDetail.PBUniqueID, thirdPartyFlatFileDetail.AllocationSeqNo);
                            }
                            thirdPartyFlatFileDetail.AccountName = CachedDataManager.GetInstance.GetAccountText(thirdPartyFlatFileDetail.CompanyAccountID);
                            thirdPartyFlatFileDetail.CounterParty = CachedDataManager.GetInstance.GetCounterPartyText(thirdPartyFlatFileDetail.CounterPartyID);
                            thirdPartyFlatFileDetail.VenueName = CachedDataManager.GetInstance.GetVenueText(thirdPartyFlatFileDetail.VenueID);
                            thirdPartyFlatFileDetail.Exchange = CachedDataManager.GetInstance.GetExchangeText(thirdPartyFlatFileDetail.ExchangeID);
                            thirdPartyFlatFileDetail.Asset = CachedDataManager.GetInstance.GetAssetText(thirdPartyFlatFileDetail.AssetID);
                            thirdPartyFlatFileDetail.UnderLying = CachedDataManager.GetInstance.GetUnderLyingText(thirdPartyFlatFileDetail.UnderLyingID);
                            thirdPartyFlatFileDetail.Strategy = CachedDataManager.GetInstance.GetStrategyText(thirdPartyFlatFileDetail.StrategyID);

                            thirdPartyFlatFileDetail.GrossAmount = Math.Round(thirdPartyFlatFileDetail.AllocatedQty * thirdPartyFlatFileDetail.AveragePrice * thirdPartyFlatFileDetail.AssetMultiplier, 6);

                            //by default the securityIDType is "Ticker" (and therefore, hardcoded).
                            thirdPartyFlatFileDetail.SecurityIDType = "Ticker";
                            thirdPartyFlatFileDetail.ThirdParty = cmbThirdParty.Text;
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.TradeDate))
                            {
                                thirdPartyFlatFileDetail.TradeDate = Convert.ToDateTime(thirdPartyFlatFileDetail.TradeDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.ExpirationDate))
                            {
                                thirdPartyFlatFileDetail.ExpirationDate = Convert.ToDateTime(thirdPartyFlatFileDetail.ExpirationDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.SettlementDate))
                            {
                                thirdPartyFlatFileDetail.SettlementDate = Convert.ToDateTime(thirdPartyFlatFileDetail.SettlementDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.ProcessDate))
                            {
                                thirdPartyFlatFileDetail.ProcessDate = Convert.ToDateTime(thirdPartyFlatFileDetail.ProcessDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.OriginalPurchaseDate))
                            {
                                thirdPartyFlatFileDetail.OriginalPurchaseDate = Convert.ToDateTime(thirdPartyFlatFileDetail.OriginalPurchaseDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.IssueDate))
                            {
                                thirdPartyFlatFileDetail.IssueDate = Convert.ToDateTime(thirdPartyFlatFileDetail.IssueDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.FirstCouponDate))
                            {
                                thirdPartyFlatFileDetail.FirstCouponDate = Convert.ToDateTime(thirdPartyFlatFileDetail.FirstCouponDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.FirstResetDate))
                            {
                                thirdPartyFlatFileDetail.FirstResetDate = Convert.ToDateTime(thirdPartyFlatFileDetail.FirstResetDate).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.RerateDateBusDayAdjusted1))
                            {
                                thirdPartyFlatFileDetail.RerateDateBusDayAdjusted1 = Convert.ToDateTime(thirdPartyFlatFileDetail.RerateDateBusDayAdjusted1).ToString("MM/dd/yyyy");
                            }
                            if (!String.IsNullOrEmpty(thirdPartyFlatFileDetail.RerateDateBusDayAdjusted2))
                            {
                                thirdPartyFlatFileDetail.RerateDateBusDayAdjusted2 = Convert.ToDateTime(thirdPartyFlatFileDetail.RerateDateBusDayAdjusted2).ToString("MM/dd/yyyy");
                            }
                            //CHMW-3132	Account wise fx rate handling for expiration settlement
                            Prana.BusinessObjects.ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrency(thirdPartyFlatFileDetail.CurrencyID, txtDayLightSaving.Value, thirdPartyFlatFileDetail.CompanyAccountID);
                            thirdPartyFlatFileDetail.ForexRate = conversionRate.RateValue;

                            if (thirdPartyFlatFileDetail.CommissionCharged == double.MinValue)
                            {
                                thirdPartyFlatFileDetail.CommissionCharged = 0;
                            }
                            if (thirdPartyFlatFileDetail.SoftCommissionCharged == double.MinValue)
                            {
                                thirdPartyFlatFileDetail.SoftCommissionCharged = 0;
                            }
                            if (thirdPartyFlatFileDetail.OtherBrokerFee == double.MinValue)
                            {
                                thirdPartyFlatFileDetail.OtherBrokerFee = 0;
                            }
                            if (thirdPartyFlatFileDetail.ClearingBrokerFee == double.MinValue)
                            {
                                thirdPartyFlatFileDetail.ClearingBrokerFee = 0;
                            }
                            int sideMultiplier = 1;
                            if (CachedDataManager.GetInstance.IsLongSide(thirdPartyFlatFileDetail.SideTag))
                            {
                                sideMultiplier = 1;
                            }
                            else
                            {
                                sideMultiplier = -1;
                            }

                            thirdPartyFlatFileDetail.NetAmount = Math.Round(((thirdPartyFlatFileDetail.GrossAmount) + (thirdPartyFlatFileDetail.CommissionCharged + thirdPartyFlatFileDetail.SoftCommissionCharged + thirdPartyFlatFileDetail.OtherBrokerFee + thirdPartyFlatFileDetail.ClearingBrokerFee + thirdPartyFlatFileDetail.StampDuty + thirdPartyFlatFileDetail.TransactionLevy + thirdPartyFlatFileDetail.ClearingFee + thirdPartyFlatFileDetail.TaxOnCommissions + thirdPartyFlatFileDetail.MiscFees + thirdPartyFlatFileDetail.SecFee + thirdPartyFlatFileDetail.OccFee + thirdPartyFlatFileDetail.OrfFee) * sideMultiplier), 6);

                            thirdPartyFlatFileDetail.CompanyID = _companyID;

                            if (!_taxLotWithStateDict.ContainsKey(thirdPartyFlatFileDetail.EntityID))
                            {
                                _taxLotWithStateDict.Add(thirdPartyFlatFileDetail.EntityID, thirdPartyFlatFileDetail.TaxLotState);
                            }

                            if (thirdPartyFlatFileDetail.TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore) && (!_taxLotIgnoreStateDict.ContainsKey(thirdPartyFlatFileDetail.EntityID)))
                            {
                                _taxLotIgnoreStateDict.Add(thirdPartyFlatFileDetail.EntityID, thirdPartyFlatFileDetail.TaxLotState);
                            }

                            if (thirdPartyFlatFileDetail.TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore) &&
                                thirdPartyFlatFileDetail.FromDeleted.ToLower().Equals("yes") && (!_deletedToIgnoreDict.ContainsKey(thirdPartyFlatFileDetail.EntityID)))
                            {
                                _deletedToIgnoreDict.Add(thirdPartyFlatFileDetail.EntityID, "yes");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyFlatFileDetailCollection;
        }

        private void GetFilteredRowDataSet(bool saveOnly)
        {
            try
            {
                _datasetFromFilteredRowsDs = null;
                ultraTabControl1.Tabs["activetaxlots"].Selected = true;
                grdThirdParty.UpdateData();
                UltraGridRow[] rows = grdThirdParty.Rows.GetFilteredInNonGroupByRows();
                DataSet ds = (DataSet)grdThirdParty.DataSource;
                DataTable dsTable = ds.Tables[0];

                DataTable dt = new DataTable();
                dt.TableName = "ThirdPartyFlatFileDetail";
                int iColCountHeader = dsTable.Columns.Count;
                for (int i = 0; i < iColCountHeader; i++)
                {
                    dt.Columns.Add(dsTable.Columns[i].ColumnName);
                }
                //DataRow dr;
                foreach (UltraGridRow row in rows)
                {
                    bool isSelected = (bool)row.Cells[HEADCOL_CheckBox].Value;
                    if (isSelected && !row.Cells["TaxLotState"].Value.ToString().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString()))
                    {
                        //dr = dt.NewRow();
                        DataRowView dtRowview = (DataRowView)row.ListObject;
                        //dr.ItemArray = dtRowview.Row.ItemArray;
                        //dt.Rows.Add(dr);
                        dt.ImportRow(dtRowview.Row);
                        if (row.HasChild())
                        {
                            //DataRow drChild;
                            foreach (UltraGridRow childrow in row.ChildBands[0].Rows)
                            {
                                //drChild = dt.NewRow();
                                DataRowView dtRowviewChild = (DataRowView)childrow.ListObject;
                                //drChild.ItemArray = dtRowviewChild.Row.ItemArray;
                                //dt.Rows.Add(drChild);
                                dt.ImportRow(dtRowviewChild.Row);
                            }
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    _datasetFromFilteredRowsDs = new DataSet();
                    _datasetFromFilteredRowsDs.DataSetName = "ThirdPartyFlatFileDetailCollection";
                    _datasetFromFilteredRowsDs.Tables.Add(dt);
                }

                if (!saveOnly && dt.Rows.Count <= 0)
                {
                    MessageBox.Show("Please select at least one record.", "Information");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The method is used to save the details to the Database.
        /// </summary>
        private bool SaveMethod(bool saveOnly)
        {
            bool isSaved = false;
            try
            {
                if (!saveOnly)
                {
                    GetFilteredRowDataSet(saveOnly);
                }
                if (_datasetFromFilteredRowsDs == null)
                {
                    isSaved = false;
                }
                else
                {
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return isSaved;
        }
        #endregion Private Functions

        #region Public Functions
        // on form closing check whether want to save the data ?
        public int OnClosingChangesSaved()
        {
            try
            {
                _mess = 0;
                if (grdThirdParty.Rows.Count > 0)
                {
                    if (_blnchange == true)
                    {
                        DialogResult diares;
                        diares = MessageBox.Show("Do you want to save the Third Party changes ?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        if (diares == DialogResult.No)
                        {
                            _mess = 1;
                        }
                        else if (diares == DialogResult.Yes)
                        {
                            UpdateTaxlotsToIgnoreState();
                            _blnchange = false;
                            _mess = 2;
                        }
                        else if (diares == DialogResult.Cancel)
                        {
                            _mess = 3;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return _mess;
        }

        #endregion Public Functions

        #region Button events

        /// <summary>
        /// The event is used to 1) fetch the data for flat file from the database.
        /// 2) and ,populate the grid with the required data for report generation on view button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // for previous values storing purpose
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                //Added to update the AUEC (if new AUEC trade is made) on click of "View" button : http://jira.nirvanasolutions.com:8080/browse/PRANA-12834
                RetainValues();
                bool isAllChecked = false;

                if (chkLstAuec.Items.Count.Equals(chkLstAuec.CheckedItems.Count))
                    isAllChecked = true;

                List<int> unCheckedAuec = new List<int>();

                if (chkSelectAllAUECs.CheckState == CheckState.Indeterminate)
                {

                    for (int i = 0; i < chkLstAuec.Items.Count; i++)
                    {
                        AUEC auec = (AUEC)chkLstAuec.Items[i];
                        if (!chkLstAuec.CheckedItems.Contains(chkLstAuec.Items[i]))
                            unCheckedAuec.Add(auec.AUECID);

                    }
                }


                BindWorkingAUEC();

                if (!isAllChecked)
                    SetCheckedAUECs(unCheckedAuec);
                else
                    SetAUECChecked();


                toolStripStatusLabel1.Text = string.Empty;
                RetainValues();
                //Check the validation of the ThirdParty report module before fetching the data for view.
                bool IsValid = ValidateThirdPartyReport();
                Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                //if the validation is successful, the data fetching is done.
                if (IsValid)
                {
                    bool IsUnallocated = ThirdPartyClientManager.ServiceInnerChannel.CheckForUnallocatedTrades(txtDayLightSaving.Value);
                    grdThirdParty.DataSource = null;
                    if (IsUnallocated)
                    {
                        if (MessageBox.Show(this, "There are some trades which are still unallocated.Do you still want to continue with Flat File generation?", "Flat File Generation Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            GetThirdPartyData(thirdPartyFileFormat);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        GetThirdPartyData(thirdPartyFileFormat);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                toolStripStatusLabel1.Text = "";
                btnView.Enabled = true;

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCheckedAUECs(List<int> unCheckedAuec)
        {
            try
            {
                for (int i = 0; i < chkLstAuec.Items.Count; i++)
                {
                    AUEC auec = (AUEC)chkLstAuec.Items[i];
                    if (_AUECColl.Contains(auec.AUECID))
                    {
                        chkLstAuec.SetItemChecked(i, true);
                    }
                }
                for (int i = 0; i < _AUECColl.Count; i++)
                {
                    AUEC auec = (AUEC)chkLstAuec.Items[i];
                    if (unCheckedAuec.Contains(auec.AUECID))
                    {
                        chkLstAuec.SetItemChecked(i, false);

                    }
                }
                if (_AUECColl.Count == 0)
                    chkSelectAllAUECs.CheckState = CheckState.Unchecked;
                else
                    chkSelectAllAUECs.CheckState = CheckState.Indeterminate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void GetThirdPartyData(Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat)
        {
            try
            {
                ToggleButtons(false);
                toolStripStatusLabel1.Text = "Getting data...";
                _dateType = cmbDateType.Value.ToString().ToUpper() == "PROCESS DATE" ? 0 : 1;
                if (string.IsNullOrEmpty(thirdPartyFileFormat.StoredProcName))
                {
                    CallBackGroundWorker();
                }
                else
                {
                    FetchDataAsycWithDirectSPCall(txtDayLightSaving.Value, thirdPartyFileFormat.StoredProcName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region stored procedure direct database call

        /// <summary>
        /// Fetch DataAsyc With DirectSPCall
        /// </summary>
        /// <param name="inputDate"></param>
        /// <param name="storedProcName"></param>
        private void FetchDataAsycWithDirectSPCall(DateTime inputDate, string storedProcName)
        {
            try
            {
                _bgFetchDataAsycWithDirectSPCall = new BackgroundWorker();
                _bgFetchDataAsycWithDirectSPCall.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fetchDataAsycWithDirectSPCall_RunWorkerCompleted);
                _bgFetchDataAsycWithDirectSPCall.DoWork += new DoWorkEventHandler(fetchDataAsycWithDirectSPCall_DoWork);
                //sending date and stored procedure name to background worker
                _bgFetchDataAsycWithDirectSPCall.RunWorkerAsync(new object[] { inputDate, storedProcName, _dateType });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void fetchDataAsycWithDirectSPCall_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Argument;

                DateTime inputDate = Convert.ToDateTime(data[0].ToString());
                string storedProcName = data[1].ToString();
                // fetch data on the basis of Process Date or AUECLocal Date
                // Process Date: 0 and AuecLocal Date:1 
                int dateType = Convert.ToInt32(data[2].ToString());

                DataSet ds = ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetails_SPCall(storedProcName, _thirdPartyID, _accountIDStringBuilder, inputDate, _companyID, _auecIDStringBuilder, 0, dateType, _fileFormatID);

                if (ds != null)
                {
                    e.Result = ds;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void fetchDataAsycWithDirectSPCall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((e.Cancelled == true))
                {
                    MessageBox.Show("Cancelled!", "Flat File Generation Information", MessageBoxButtons.OK);
                }
                else if (!(e.Error == null))
                {
                    MessageBox.Show("Error: " + e.Error.Message, "Flat File Generation Information", MessageBoxButtons.OK);
                }
                else
                {
                    DataSet ds = (DataSet)e.Result;
                    if (ds != null)
                    {
                        FillDataWithDirectSPCall(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                toolStripStatusLabel1.Text = "";
                btnView.Enabled = true;

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void FillDataWithDirectSPCall(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt = GeneralUtilities.ArrangeTable(dt, "ThirdPartyFlatFileDetail");

                    Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;

                    string xsltPath = thirdPartyFileFormat.PranaToThirdParty;

                    string xsltName = xsltPath.Substring(xsltPath.LastIndexOf("\\") + 1);

                    string xsltStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + xsltName;
                    string strSerializedXML = Application.StartupPath + @"\serializedThirdPartyFlatFileXml.xml";
                    dt.WriteXml(strSerializedXML);
                    string mappedxml = Application.StartupPath + @"\ConvertedThirdPartyNew.xml";

                    string mappedfilePath = XMLUtilities.GetTransformed(strSerializedXML, mappedxml, xsltStartUpPath);

                    if (!mappedfilePath.Equals(""))
                    {
                        _dsXML = new DataSet();
                        _dsXML.ReadXml(mappedfilePath);
                        if (_dsXML.Tables.Count <= 0)
                        {
                            MessageBox.Show("No data available for selected Format - " + cmbFormat.Text, "Information");
                            grdThirdParty.DataSource = null;
                            toolStripStatusLabel1.Text = string.Empty;
                            btnView.Enabled = true;
                            return;
                        }

                        GeneralUtilities.ReArrangeTable(_dsXML.Tables[0]);
                        if (!thirdPartyFileFormat.DelimiterName.ToUpper().ToString().Equals("XML"))
                            UpdateCaptions();
                        else
                        {
                            bool captionChangeRequiredExists = _dsXML.Tables[0].Columns.Contains("IsCaptionChangeRequired");
                            if (captionChangeRequiredExists && _dsXML.Tables[0].Rows[0]["TaxLotState"].ToString().ToUpper().Equals("TAXLOTSTATE"))
                            {
                                _dsForXMLFile = _dsXML.Copy();
                                _dsXML.Tables[0].Rows[0].Delete();
                            }
                        }
                        grdThirdParty.DataSource = null;
                        grdThirdParty.DataSource = _dsXML;
                        SetColumnHidden();
                        AddCheckBoxinGrid(grdThirdParty, _headerCheckBoxUnallocated);
                        SetDefaultFilters();
                        toolStripStatusLabel1.Text = "Success";
                        ToggleButtons(true);
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "";
                        btnView.Enabled = true;
                        MessageBox.Show("The XML could not be generated for the collection.Please contact the administrator!", "Information");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion stored procedure direct database call

        #region Fill third party business object from database

        private void CallBackGroundWorker()
        {
            try
            {
                BackgroundWorker backGroundWorker = new BackgroundWorker();
                backGroundWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
                backGroundWorker.RunWorkerAsync();
                backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_RunWorkerCompleted);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetails = null;
                Task<Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection> task = Task.Run<Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection>(async () => await FillData());
                thirdPartyFlatFileDetails = task.Result;
                if (thirdPartyFlatFileDetails != null)
                {
                    e.Result = thirdPartyFlatFileDetails;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void backGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetails = (Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection)e.Result;
                if (thirdPartyFlatFileDetails != null)
                {
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            ThirdPartyObjHandler thirdPartyCollHandler = new ThirdPartyObjHandler(GenerateXML);
                            this.BeginInvoke(thirdPartyCollHandler, new object[] { thirdPartyFlatFileDetails });
                        }
                        else
                        {
                            GenerateXML(thirdPartyFlatFileDetails);
                        }
                    }
                }
                else
                {
                    btnSave.Enabled = false;
                    btnGenerate.Enabled = false;
                    btnView.Enabled = true;
                    toolStripStatusLabel1.Text = string.Empty;
                    MessageBox.Show("No Data available for the selected account(s)", "Information");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion Fill third party business object from database

        private void GenerateXML(Prana.BusinessObjects.ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetails)
        {
            try
            {
                Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;

                //FillData();

                //check whether the collection of details for the selected accounts is null or not. 
                if (thirdPartyFlatFileDetails != null)
                {
                    if ((thirdPartyFileFormat.PranaToThirdParty).Equals(""))
                    {
                        MessageBox.Show("No XSLT is available for the selected ThirdParty. Please contact the administrator!", "Information");
                        return;
                    }
                    //In case of available xslt path, the path is given to the method called for tranformation.
                    //string xsltPath = Application.StartupPath + @"\XSLT\ThirdPartyXSL\" + thirdPartyFileFormat.PranaToThirdParty;
                    string xsltPath = thirdPartyFileFormat.PranaToThirdParty;

                    string xsltName = xsltPath.Substring(xsltPath.LastIndexOf("\\") + 1);

                    string xsltStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + xsltName;
                    string str = Application.StartupPath + @"\serializedThirdPartyFlatFileXml.xml";
                    string strFilePathNew = Application.StartupPath + @"\ConvertedThirdPartyNew.xml";

                    // if C# code used in the XSLT, so need to do these setting as done below
                    //XsltSettings settings = new XsltSettings(false, true);
                    //XslCompiledTransform xslt = new XslCompiledTransform();
                    //xslt.Load(xsltPath, settings, new XmlUrlResolver());
                    string mappedfilePath = XMLHelper.GetTransformed(str, strFilePathNew, xsltStartUpPath, thirdPartyFlatFileDetails);

                    if (!mappedfilePath.Equals(""))
                    {
                        _dsXML = new DataSet();
                        //_dsXML.ReadXml(mappedfilePath, XmlReadMode.InferTypedSchema);
                        _dsXML.ReadXml(mappedfilePath);
                        //reader.Close();  

                        RepositionPrimaryColumn();

                        if (_dsXML.Tables.Count <= 0)
                        {
                            MessageBox.Show("No data available for selected Format - " + cmbFormat.Text, "Information");
                            grdThirdParty.DataSource = null;
                            toolStripStatusLabel1.Text = string.Empty;
                            btnView.Enabled = true;
                            return;
                        }
                        if (!thirdPartyFileFormat.DelimiterName.ToUpper().ToString().Equals("XML"))
                            UpdateCaptions();
                        else
                        {
                            bool captionChangeRequiredExists = _dsXML.Tables[0].Columns.Contains("IsCaptionChangeRequired");
                            if (captionChangeRequiredExists && _dsXML.Tables[0].Rows[0]["TaxLotState"].ToString().ToUpper().Equals("TAXLOTSTATE"))
                            {
                                _dsForXMLFile = _dsXML.Copy();
                                _dsXML.Tables[0].Rows[0].Delete();
                            }
                        }
                        
                        grdThirdParty.DataSource = null;
                        if (chkSelectAllAUECs.CheckState == CheckState.Checked || chkSelectAllAUECs.CheckState == CheckState.Indeterminate)
                            grdThirdParty.DataSource = _dsXML;
                        else
                            grdThirdParty.DataSource = null;
                        SetColumnHidden();
                        AddCheckBoxinGrid(grdThirdParty, _headerCheckBoxUnallocated);
                        SetDefaultFilters();
                        toolStripStatusLabel1.Text = "Success";
                        //btnView.Enabled = true;
                        //btnSave.Enabled = true;
                        //btnGenerate.Enabled = true;
                        ToggleButtons(true);
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "";
                        btnView.Enabled = true;
                        MessageBox.Show("The XML could not be generated for the collection.Please contact the administrator!", "Information");
                        return;
                    }
                }
                else
                {
                    //MessageBox.Show("No Data available for the selected Third Party Accounts.");
                    //RefreshThirdPartyReport();
                    toolStripStatusLabel1.Text = "";
                    btnView.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                toolStripStatusLabel1.Text = "";
                btnView.Enabled = true;
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void RepositionPrimaryColumn()
        {
            try
            {
                if (_dsXML != null && (_dsXML.Tables.Count == 2))
                {
                    if (_dsXML.Tables[0].Columns.Contains("Group_Id"))
                    {
                        _dsXML.Tables[0].Columns["Group_Id"].SetOrdinal(0);
                    }

                    if (_dsXML.Tables[1].Columns.Contains("Group_Id"))
                    {
                        _dsXML.Tables[1].Columns["Group_Id"].SetOrdinal(0);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ToggleButtons(bool status)
        {
            try
            {
                btnView.Enabled = status;
                btnSave.Enabled = status;
                btnGenerate.Enabled = status;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateCaptions()
        {
            try
            {
                _dsForXMLFile = _dsXML.Copy();
                string captionChangeRequiredValue = string.Empty;
                bool captionChangeRequiredExists = _dsXML.Tables[0].Columns.Contains(HEADCOL_ISCAPCHANGEREQ);
                if (captionChangeRequiredExists)
                {
                    // Handling based on Taxlot State of Column Captions, Check If IsCaptionChangeRequired tag is added in xslt but Column captions are missing in xslt.
                    //Jira: - http://jira.nirvanasolutions.com:8080/browse/PRANA-4620
                    //Taxlots state for Trade will be somthing value(Like allocated,sent etc) but for caption it will be taxlotState.
                    if (_dsXML.Tables[0].Rows[0][HEADCOL_TAXLOTSTATE].ToString().ToUpper() == HEADCOL_TAXLOTSTATE || _dsXML.Tables[0].Rows[0][HEADCOL_TAXLOTSTATE].ToString().ToUpper() == "TRUE")
                    {
                        if (_dsXML.Tables[0].Rows.Count > 1)
                        {
                            captionChangeRequiredValue = _dsXML.Tables[0].Rows[1][HEADCOL_ISCAPCHANGEREQ].ToString();
                            if (!String.IsNullOrEmpty(captionChangeRequiredValue) && captionChangeRequiredValue.Trim().ToUpper().Equals("TRUE"))
                            {
                                DataTable dtCaption = _dsXML.Tables[0].Copy();
                                foreach (DataColumn col in _dsXML.Tables[0].Columns)
                                {
                                    DataRow rowCaption = dtCaption.Rows[0];
                                    foreach (DataColumn captionCol in dtCaption.Columns)
                                    {
                                        if (captionCol.ColumnName.Equals(col.ColumnName) && !ColumnExists(col.ColumnName) && !col.ColumnName.Equals("TaxlotState"))
                                        {
                                            if (!string.IsNullOrEmpty(rowCaption[captionCol.ColumnName].ToString()))
                                            {
                                                col.ColumnName = rowCaption[captionCol.ColumnName].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        _dsXML.Tables[0].Rows[0].Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void RetainValues()
        {
            try
            {
                _getprevDate = Convert.ToDateTime(txtDayLightSaving.Text);
                _cmbThirdPartyValue = Convert.ToInt32(cmbThirdParty.Value);
                _cmbThirdPartyText = cmbThirdParty.Text.ToString();
                _cmbThirdPartyTypeValue = Convert.ToInt32(cmbThirdPartyType.Value);
                _cmbThirdPartyTypeText = cmbThirdPartyType.Text.ToString();
                // save File Format values in the variables so that if we want to restore we can...
                Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                _cmbFormatText = cmbFormat.Text;
                _cmbFormatVal = thirdPartyFormat.FileFormatId;
                // use when main account check list state chnage
                _blnChklistchange = false;
                //clear the List collection
                _thirdPartyMainAccountColl.Clear();
                _accountIDStringBuilder = new StringBuilder();
                if (chkLstThirdPartyAccounts.CheckedItems.Count > 0)
                {
                    for (int i = 0, count = chkLstThirdPartyAccounts.CheckedItems.Count; i < count; i++)
                    {
                        // get the selected Third Party Account Id
                        Prana.BusinessObjects.ThirdPartyPermittedAccount thirdPartyPermittedAccount = (Prana.BusinessObjects.ThirdPartyPermittedAccount)chkLstThirdPartyAccounts.CheckedItems[i];
                        int thirdPartyAccountID = Convert.ToInt32(thirdPartyPermittedAccount.CompanyAccountID);

                        _accountIDStringBuilder.Append(thirdPartyAccountID);
                        _accountIDStringBuilder.Append(",");
                        // Add the contents in the List
                        _thirdPartyMainAccountColl.Add(thirdPartyAccountID);
                    }
                }

                int accountIDLen = _accountIDStringBuilder.Length;
                if (_accountIDStringBuilder.Length > 0)
                {
                    _accountIDStringBuilder.Remove((accountIDLen - 1), 1);
                }


                if (chkL2Data.Checked == true)
                {
                    _chkL2Value = true;
                }
                else
                {
                    _chkL2Value = false;
                }

                //_blnAUEClstChange = false;
                _AUECColl.Clear();

                _auecIDStringBuilder = new StringBuilder();
                if (chkLstAuec.CheckedItems.Count > 0)
                {
                    for (int i = 0, count = chkLstAuec.CheckedItems.Count; i < count; i++)
                    {
                        Prana.Admin.BLL.AUEC auec = (Prana.Admin.BLL.AUEC)chkLstAuec.CheckedItems[i];
                        int auecID = Convert.ToInt32(auec.AUECID);
                        _auecIDStringBuilder.Append(auecID);
                        _auecIDStringBuilder.Append(",");
                        _AUECColl.Add(auecID);
                    }
                }
                else
                {
                    for (int i = 0, count = chkLstAuec.Items.Count; i < count; i++)
                    {
                        Prana.Admin.BLL.AUEC auec = (Prana.Admin.BLL.AUEC)chkLstAuec.Items[i];
                        int auecID = Convert.ToInt32(auec.AUECID);
                        _auecIDStringBuilder.Append(auecID);
                        _auecIDStringBuilder.Append(",");
                    }
                }
                int len = _auecIDStringBuilder.Length;
                if (_auecIDStringBuilder.Length > 0)
                {
                    _auecIDStringBuilder.Remove((len - 1), 1);
                }

                _blnChkAllAccountChange = false;
                _chkStateForAllAccounts = chkSelectAllAccounts.CheckState;
                _chkStateForAllAUECs = chkSelectAllAUECs.CheckState;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            try
            {
                grid.CreationFilter = headerCheckBox;
                grid.DisplayLayout.Bands[0].Columns.Add(HEADCOL_CheckBox, "");
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].DataType = typeof(bool);
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].CellClickAction = CellClickAction.EditAndSelectText;
                SetCheckBoxAtFirstPosition(grid);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Width = 10;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool IsAnyRecordSelected()
        {
            bool isSelected = false;
            try
            {
                ultraTabControl1.Tabs["activetaxlots"].Selected = true;
                UltraGridRow[] rows = grdThirdParty.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if ((bool)row.Cells[HEADCOL_CheckBox].Value && !row.Cells["TaxLotState"].Value.ToString().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString()))
                    {
                        isSelected = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isSelected;
        }
        /// <summary>
        /// On click of generate button, the updated data in the datagrid is saved in the database, and 
        /// an xml for thirdParty is generated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnScreenshot_Click(object sender, EventArgs e)
        //{
        //try
        //{
        //if (ThirdPartyReportMain.ActiveForm != null)
        //SnapShotManager.TakeSnapshot(ThirdPartyReportMain.ActiveForm);
        //}
        //catch (Exception ex)
        //{
        //bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //if (rethrow)
        //{
        //throw;
        //}
        //}
        //}
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                //validate whether all the mandatory parameters are selected
                bool IsValid = ValidateThirdPartyReport();
                //check is any record selected to generate report
                bool isSelected = IsAnyRecordSelected();
                toolStripStatusLabel1.Text = string.Empty;

                if (!isSelected)
                {
                    MessageBox.Show("Please select at least one record.", "Information");
                    return;
                }

                if (IsValid && (grdThirdParty.Rows.Count > 0))
                {
                    Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                    string delimiterSymbol = thirdPartyFileFormat.Delimiter;
                    string delimiterName = thirdPartyFileFormat.DelimiterName;
                    string fileExtension = thirdPartyFileFormat.FileExtension;
                    if (string.IsNullOrEmpty(delimiterSymbol) || string.IsNullOrEmpty(delimiterName))
                    {
                        MessageBox.Show("Delimiter or Delimiter display name of the file not set.", "Information");
                        return;
                    }

                    //check if delimiter is tab or New line
                    if (delimiterSymbol.ToUpper().Equals("\\T"))
                    {
                        delimiterSymbol = "\t";
                    }
                    else if (delimiterSymbol.ToUpper().Equals("\\N"))
                    {
                        delimiterSymbol = "\n";
                    }

                    //on the basis of delimiter, ask user to generate a file
                    ThirdPartyFileType frmTPFileFormatType = new ThirdPartyFileType(delimiterName);
                    frmTPFileFormatType.ShowDialog();

                    if (frmTPFileFormatType.FileFormat != int.MinValue)
                    {
                        _fileType = int.MinValue;
                        _fileType = frmTPFileFormatType.FileFormat;
                    }
                    else
                    {
                        _fileType = int.MinValue;
                    }

                    if (_fileType != int.MinValue)
                    {
                        Prana.BusinessObjects.ThirdPartyFlatFileSaveDetail tPFFsaveDetail = CompanyManager.GetThirdPartyFlatFileSaveDetail(_thirdPartyID);

                        string filePath = tPFFsaveDetail.SaveGeneratedFileIn;

                        string fileName = thirdPartyFileFormat.FileDisplayName;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            MessageBox.Show("The Naming Convention of The File to be saved is not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }


                        string fileNameTobeSavedAndDisplay = string.Empty;
                        string pranaFilePath = string.Empty;
                        string strFileNameAfterClosingBraces = string.Empty;

                        //check if filename contains open and close braces then cast between value in the datetime format
                        int startIndex = fileName.IndexOf("{");
                        int lastIndex = fileName.LastIndexOf("}");
                        if (fileName.Contains("{") || fileName.Contains("}"))
                        {
                            if (startIndex == -1 || lastIndex == -1)
                            {
                                MessageBox.Show("The Naming Convention of The File to be saved is not correct.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            int lengthOfFile = (lastIndex - startIndex) - 1;

                            if (lengthOfFile <= 0)
                            {
                                MessageBox.Show("The Naming Convention of The File to be saved is not correct.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }


                            string FileDateFormat = fileName.Substring(startIndex + 1, lengthOfFile);

                            string strFileNameBeforeStartBraces = fileName.Substring(0, fileName.IndexOf("{"));
                            string strFileNameBeforeClosingBraces = fileName.Substring(0, fileName.IndexOf("}"));
                            strFileNameAfterClosingBraces = fileName.Substring(strFileNameBeforeClosingBraces.Length + 1);

                            // txtDayLightSaving.Value = System.DateTime.Now;
                            var dateWithCurrentTime = txtDayLightSaving.Value.Date.AddTicks(DateTime.Now.TimeOfDay.Ticks);
                            string DateFormat = dateWithCurrentTime.ToString(FileDateFormat);

                            fileNameTobeSavedAndDisplay = strFileNameBeforeStartBraces + DateFormat + strFileNameAfterClosingBraces;
                            pranaFilePath = filePath + "\\" + fileNameTobeSavedAndDisplay;
                        }
                        else
                        {
                            pranaFilePath = filePath + "\\" + fileName;
                            fileNameTobeSavedAndDisplay = fileName;
                        }

                        // For user defind _fileType=1
                        if (_fileType.Equals(1))
                        {
                            if (!string.IsNullOrEmpty(fileExtension.Trim()))
                            {
                                fileNameTobeSavedAndDisplay = fileNameTobeSavedAndDisplay + "." + fileExtension.Trim();

                                pranaFilePath = pranaFilePath + "." + fileExtension.Trim();
                            }

                            // check for the File Path i.e. Is Path Valid
                            bool isFilePathExists = System.IO.Directory.Exists(filePath);

                            if (isFilePathExists)
                            {
                                // check file already Existing ?
                                bool fileExists = System.IO.File.Exists(pranaFilePath);
                                if (fileExists)
                                {
                                    DialogResult dlgResult = new DialogResult();
                                    dlgResult = MessageBox.Show("File " + pranaFilePath + " already exist,\nDo you want to replace it ?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                    if (dlgResult.Equals(DialogResult.Yes))
                                    {
                                        if (fileExtension.ToUpper().Equals("XML"))
                                        {
                                            GenerateUserDefindFormatForXML(pranaFilePath, fileNameTobeSavedAndDisplay);
                                        }
                                        else
                                        {
                                            GenerateUserDefindFormat(pranaFilePath, fileNameTobeSavedAndDisplay, delimiterSymbol);
                                        }
                                    }
                                    else if (dlgResult.Equals(DialogResult.No))
                                    {
                                        string fileNameWithPath = OpenSaveDialogBox(pranaFilePath, delimiterName + "Files (*." + fileExtension + ")|*." + fileExtension);
                                        if (!String.IsNullOrEmpty(fileNameWithPath))
                                        {
                                            string fileNameToPass = fileNameWithPath.Substring(fileNameWithPath.LastIndexOf("\\") + 1);
                                            if (fileExtension.ToUpper().Equals("XML"))
                                            {
                                                GenerateUserDefindFormatForXML(fileNameWithPath, fileNameToPass);
                                            }
                                            else
                                            {
                                                GenerateUserDefindFormat(fileNameWithPath, fileNameToPass, delimiterSymbol);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (fileExtension.ToUpper().Equals("XML"))
                                    {
                                        GenerateUserDefindFormatForXML(pranaFilePath, fileNameTobeSavedAndDisplay);
                                    }
                                    else
                                    {
                                        GenerateUserDefindFormat(pranaFilePath, fileNameTobeSavedAndDisplay, delimiterSymbol);
                                    }
                                }
                            }
                            else
                            {
                                string fileNameWithPath = string.Empty;
                                if (string.IsNullOrEmpty(fileExtension.Trim()))
                                {
                                    fileNameWithPath = OpenSaveDialogBox(pranaFilePath, " All Files (*.*)|*.*");
                                }
                                else
                                {
                                    fileNameWithPath = OpenSaveDialogBox(pranaFilePath, delimiterName + " Files (*." + fileExtension + ")|*." + fileExtension);
                                }
                                if (!String.IsNullOrEmpty(fileNameWithPath))
                                {

                                    string fileNameToPass = fileNameWithPath.Substring(fileNameWithPath.LastIndexOf("\\") + 1);
                                    if (fileExtension.ToUpper().Equals("XML"))
                                    {
                                        GenerateUserDefindFormatForXML(fileNameWithPath, fileNameToPass);
                                    }
                                    else
                                    {
                                        GenerateUserDefindFormat(fileNameWithPath, fileNameToPass, delimiterSymbol);
                                    }
                                }
                            }

                        }
                        // For Excel _fileType=2
                        else if (_fileType == 2)
                        {
                            fileNameTobeSavedAndDisplay = fileNameTobeSavedAndDisplay + ".xls";
                            pranaFilePath = pranaFilePath + ".xls";
                            // check for the File Path i.e. Is Path Valid
                            bool isFilePathExists = System.IO.Directory.Exists(filePath);

                            if (isFilePathExists)
                            {
                                //check file already existing ?
                                bool fileExists = System.IO.File.Exists(pranaFilePath);

                                if (fileExists)
                                {
                                    DialogResult dlgResult = new DialogResult();
                                    dlgResult = MessageBox.Show("File " + pranaFilePath + " already exist,\nDo you want to replace it ?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                    if (dlgResult.Equals(DialogResult.Yes))
                                    {
                                        GenerateExcelFile(pranaFilePath, fileNameTobeSavedAndDisplay);
                                    }
                                    else if (dlgResult.Equals(DialogResult.No))
                                    {
                                        string fileNameWithPath = OpenSaveDialogBox(pranaFilePath, "Excel Files (*.xls)|*.xls");
                                        if (!String.IsNullOrEmpty(fileNameWithPath))
                                        {
                                            string fileNameToDisplay = fileNameWithPath.Substring(fileNameWithPath.LastIndexOf("\\") + 1);
                                            GenerateExcelFile(fileNameWithPath, fileNameToDisplay);
                                        }
                                    }
                                }
                                else
                                {
                                    GenerateExcelFile(pranaFilePath, fileNameTobeSavedAndDisplay);
                                }
                            }
                            else
                            {
                                string fileNameWithPath = OpenSaveDialogBox(pranaFilePath, "Excel Files (*.xls)|*.xls");
                                if (!String.IsNullOrEmpty(fileNameWithPath))
                                {
                                    string fileNameToDisplay = fileNameWithPath.Substring(fileNameWithPath.LastIndexOf("\\") + 1);
                                    GenerateExcelFile(fileNameWithPath, fileNameToDisplay);
                                }

                            }
                        }
                        //}
                    }
                }
                else if (grdThirdParty.Rows.Count <= 0)
                {
                    MessageBox.Show("Nothing to generate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (FormatException fex)
            {
                MessageBox.Show("Please correct Date Format.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bool rethrow = Logger.HandleException(fex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string OpenSaveDialogBox(string initialDirectory, string filter)
        {
            string strFilePath = string.Empty;
            // File open dialog , ask user to select the File
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = initialDirectory;
                saveFileDialog.Filter = filter;
                saveFileDialog.FileName = initialDirectory.Substring(initialDirectory.LastIndexOf("\\") + 1);
                saveFileDialog.RestoreDirectory = true;
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    strFilePath = saveFileDialog.FileName;
                }
                else if (result == DialogResult.Cancel)
                {
                    strFilePath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return strFilePath;
        }


        //Pooja Porwal 12/15/2014:  Generate XML file from Third Party Generation 

        /// <summary>
        /// Genrate XML file for Third Party generation  
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileNameToBeDisplayed"></param>
        /// <param name="fileExtension"></param>
        private void GenerateUserDefindFormatForXML(string filePath, string fileNameToBeDisplayed)
        {
            Prana.BusinessObjects.ThirdPartyFileFormat tPFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
            StreamWriter sw = null;
            string xmlString = null;
            DataSet ds = new DataSet();
            DataSet dsHeader = new DataSet();
            DataSet dsFooter = new DataSet();
            DataSet ds_Updated = new DataSet();

            try
            {
                Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                DateTime dateHeader = txtDayLightSaving.Value;
                string dateFormat = dateHeader.ToString("MM/dd/yyyy:HHmmss");
                int recordCount = 0;// to display in the footer section
                //int statusID = 1;
                bool isDataSaved = SaveMethod(false);
                if (!tPFileFormat.ExportOnly)
                {
                    UpdateTaxlotStateAndSaveData(_datasetFromFilteredRowsDs, fileNameToBeDisplayed);
                    UpdateTaxlotsToIgnoreState();
                }
                if (isDataSaved)
                {
                    DataSet dsGrid = _datasetFromFilteredRowsDs;
                    if (dsGrid == null)
                    {
                        return;
                    }

                    #region Footer File Fields Calculations

                    recordCount = dsGrid.Tables[0].Rows.Count;
                    bool blntotalQtyFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ALLOCQTY);
                    bool blnnetNotionalFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALNETNOTIONAL);
                    bool blngrossAmountFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALGROSSAMOUNT);
                    double totalQtytoSend = 0.0;
                    double internalNetNotionaltoSend = 0.0;
                    double internalGrossAmounttoSend = 0.0;
                    if (blntotalQtyFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            totalQtytoSend = totalQtytoSend + Convert.ToDouble(row[HEADCOL_ALLOCQTY]);
                        }
                    }

                    if (blnnetNotionalFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            internalNetNotionaltoSend = internalNetNotionaltoSend + Convert.ToDouble(row[HEADCOL_INTERNALNETNOTIONAL]);
                        }
                    }

                    if (blngrossAmountFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            internalGrossAmounttoSend = internalGrossAmounttoSend + Convert.ToDouble(row[HEADCOL_INTERNALGROSSAMOUNT]);
                        }
                    }

                    #endregion Footer File Fields Calculations

                    sw = new StreamWriter(filePath);

                    #region Header Code

                    string fileHeaderReq = string.Empty;
                    bool blnFileHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEHEADER);
                    if (blnFileHeaderContains)
                    {
                        fileHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEHEADER].ToString();
                    }
                    if (fileHeaderReq.ToUpper().Equals("TRUE"))
                    {
                        if (string.IsNullOrEmpty(thirdPartyFileFormat.HeaderFile))
                        {
                            MessageBox.Show("No Header XSLT is available for the selected ThirdParty. Please contact the administrator!", "Information");
                            return;
                        }
                        dsHeader = HeaderDataSet(dateFormat, recordCount, thirdPartyFileFormat);
                        if (dsHeader.Tables.Count <= 0)
                        {
                            MessageBox.Show("No data available for Header", "Information");
                            return;
                        }
                        else
                        {
                            string xmlHeaderMainTag = string.Empty;
                            bool blnHeaderMainTag = dsHeader.Tables[0].Columns.Contains(HEADCOL_XMLHEADERMAINTAG);
                            if (blnHeaderMainTag)
                            {
                                xmlHeaderMainTag = dsHeader.Tables[0].Rows[0][HEADCOL_XMLHEADERMAINTAG].ToString();
                            }

                            string xmlHeaderChildTag = string.Empty;
                            bool blnHeaderChildTag = dsHeader.Tables[0].Columns.Contains(HEADCOL_XMLHEADERCHILDTAG);
                            if (blnHeaderChildTag)
                            {
                                xmlHeaderChildTag = dsHeader.Tables[0].Rows[0][HEADCOL_XMLHEADERCHILDTAG].ToString();
                            }

                            dsHeader = RemoveColumn(dsHeader);
                            dsHeader = RenameDataSetAndTable(xmlHeaderMainTag, xmlHeaderChildTag, dsHeader);
                        }
                    }

                    #endregion Header Code

                    #region Footer Code

                    string fileFooterReq = string.Empty;

                    bool blnFileFooterContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEFOOTER);
                    if (blnFileFooterContains)
                    {
                        fileFooterReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEFOOTER].ToString();
                    }
                    if (fileFooterReq.ToUpper().Equals("TRUE"))
                    {
                        if (string.IsNullOrEmpty(thirdPartyFileFormat.FooterFile))
                        {
                            MessageBox.Show("No Footer XSLT is available for the selected ThirdParty. Please contact the administrator!", "Information");
                            return;
                        }
                        dsFooter = FileFooterDataSet(thirdPartyFileFormat, dateFormat, recordCount, totalQtytoSend, internalNetNotionaltoSend, internalGrossAmounttoSend);
                        if (dsFooter.Tables.Count <= 0)
                        {
                            MessageBox.Show("No data available for Footer", "Information");
                            return;
                        }
                        else
                        {
                            string xmlFooterMainTag = string.Empty;
                            bool blnFooterMainTag = dsFooter.Tables[0].Columns.Contains(HEADCOL_XMLFOOTERMAINTAG);
                            if (blnFooterMainTag)
                            {
                                xmlFooterMainTag = dsFooter.Tables[0].Rows[0][HEADCOL_XMLFOOTERMAINTAG].ToString();
                            }
                            string xmlFooterChildTag = string.Empty;
                            bool blnFooterChildTag = dsFooter.Tables[0].Columns.Contains(HEADCOL_XMLFOOTERMAINTAG);
                            if (blnFooterChildTag)
                            {
                                xmlFooterChildTag = dsFooter.Tables[0].Rows[0][HEADCOL_XMLFOOTERCHILDTAG].ToString();
                            }

                            dsFooter = RemoveColumn(dsFooter);
                            dsFooter = RenameDataSetAndTable(xmlFooterMainTag, xmlFooterChildTag, dsFooter);
                        }
                    }

                    #endregion Footer Code

                    #region general XML file generation code

                    string xmlMainTag = string.Empty;
                    bool blnMainTag = dsGrid.Tables[0].Columns.Contains(HEADCOL_XMLMAINTAG);
                    if (blnMainTag)
                    {
                        xmlMainTag = dsGrid.Tables[0].Rows[0][HEADCOL_XMLMAINTAG].ToString();
                    }

                    string xmlChildTag = string.Empty;
                    bool blnChildTag = dsGrid.Tables[0].Columns.Contains(HEADCOL_XMLCHILDTAG);
                    if (blnChildTag)
                    {
                        xmlChildTag = dsGrid.Tables[0].Rows[0][HEADCOL_XMLCHILDTAG].ToString();
                    }

                    ds = RemoveColumn(dsGrid.Copy());
                    ds = RenameDataSetAndTable(xmlMainTag, xmlChildTag, ds);

                    if (dsHeader.Tables.Count > 0)
                    {
                        ds_Updated.Tables.Add(dsHeader.Tables[0].Copy());
                    }
                    if (dsGrid.Tables.Count > 0)
                    {
                        ds_Updated.Tables.Add(ds.Tables[0].Copy());
                    }
                    if (dsFooter.Tables.Count > 0)
                    {
                        ds_Updated.Tables.Add(dsFooter.Tables[0].Copy());
                    }
                   
                    RenameDataSetAndTable(xmlMainTag, "", ds_Updated);

                    xmlString = ConvertDataSetIntoXML(ds_Updated);
                            sw.Write(xmlString.ToString());

                            xmlString = null;
                    dsHeader = null;
                    dsFooter = null;
                    ds_Updated = null;                    
                            ds = null;

                    #endregion general XML file generation code                  

                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                        toolStripStatusLabel1.Text = "Generated";
                        //set backup on startup path
                        SetEODFileBackUp(filePath);
                        if (!tPFileFormat.DoNotShowFileOpenDialogue)
                        {
                            ViewFileOpenDialogue(filePath, fileNameToBeDisplayed);
                        }
                        _blnchange = false;
                    }

                    ds_Updated = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Convert file Header Data into Data Set
        /// </summary>
        /// <param name="dateFormat">Date format to show in file footer</param>
        /// <param name="recordCount">Total record count </param>
        /// <param name="thirdPartyFileFormat">Third Party file format</param>
        /// <returns>DataSet of file Header</returns>

        private DataSet HeaderDataSet(string dateFormat, int recordCount, Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat)
        {
            DataSet dsHeader = new DataSet();
            try
            {
                string headerXsltPath = thirdPartyFileFormat.HeaderFile;
                string headerXsltName = headerXsltPath.Substring(headerXsltPath.LastIndexOf("\\") + 1);
                string xsltForHeaderStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + headerXsltName;
                Prana.BusinessObjects.ThirdPartyFlatFileHeader thirdPartyFlatFileHeader = new Prana.BusinessObjects.ThirdPartyFlatFileHeader();
                thirdPartyFlatFileHeader.Date = txtDayLightSaving.Text;
                thirdPartyFlatFileHeader.DateAndTime = dateFormat;
                thirdPartyFlatFileHeader.RecordCount = recordCount;
                string serializeXMLforHeader = XMLUtilities.SerializeToXML(thirdPartyFlatFileHeader);
                string convertedXMLforHeader = Application.StartupPath + @"\ConvertedThirdPartyXMLforHeader.xml";
                StringReader sr = new StringReader(serializeXMLforHeader);
                XmlTextReader xreader = new XmlTextReader(sr);
                string mappedfilePathforHeader = XMLUtilities.GetTransformed(xreader, convertedXMLforHeader, xsltForHeaderStartUpPath);
                if (!mappedfilePathforHeader.Equals(""))
                {
                    dsHeader.ReadXml(mappedfilePathforHeader);
                }
                return dsHeader;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dsHeader;
        }

        /// <summary>
        /// Rename Data Set name and Table name
        /// </summary>
        /// <param name="xmlMainTag">Data Set name to rename the dataSet </param>
        /// <param name="xmlChildTag">DataTable name to rename the datatable</param>
        /// <param name="ds">Data Set</param>
        /// <returns>DataSet after rename with its table table[0]</returns>

        private DataSet RenameDataSetAndTable(string xmlMainTag, string xmlChildTag, DataSet ds)
        {
            try
            {
                if (!String.IsNullOrEmpty(xmlMainTag))
                    ds.DataSetName = xmlMainTag;
                if (!String.IsNullOrEmpty(xmlChildTag))
                    ds.Tables[0].TableName = xmlChildTag;
                return ds;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }


        /// <summary>
        /// Convert file footer Data into Data Set
        /// </summary>
        /// <param name="thirdPartyFileFormat">Third Party file format</param>
        /// <param name="dateFormat">Date format to show in file footer</param>
        /// <param name="recordCount">Total record count </param>
        /// <param name="totalQtytoSend">Internal  totol qty to show in File footer</param>
        /// <param name="internalNetNotionaltoSend">Internal Net notional to show in File footer</param>
        /// <param name="internalGrossAmounttoSend">Internal  Gross amount to show in File footer</param>
        /// <returns>DataSet of file footer</returns>

        private DataSet FileFooterDataSet(Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat, string dateFormat, int recordCount, double totalQtytoSend, double internalNetNotionaltoSend, double internalGrossAmounttoSend)
        {
            DataSet dsFooter = new DataSet();
            try
            {
                string footerXsltPath = thirdPartyFileFormat.FooterFile;
                string footerXsltName = footerXsltPath.Substring(footerXsltPath.LastIndexOf("\\") + 1);
                string xsltForFooterStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + footerXsltName;
                Prana.BusinessObjects.ThirdPartyFlatFileFooter thirdPartyFlatFileFooter = new Prana.BusinessObjects.ThirdPartyFlatFileFooter();
                thirdPartyFlatFileFooter.RecordCount = recordCount;
                thirdPartyFlatFileFooter.TotalQty = totalQtytoSend;
                thirdPartyFlatFileFooter.Date = txtDayLightSaving.Text;
                thirdPartyFlatFileFooter.DateAndTime = dateFormat;
                thirdPartyFlatFileFooter.InternalNetNotional = internalNetNotionaltoSend;
                thirdPartyFlatFileFooter.InternalGrossAmount = internalGrossAmounttoSend;
                string serializeXMLforFooter = XMLUtilities.SerializeToXML(thirdPartyFlatFileFooter);
                string convertedXMLforFooter = Application.StartupPath + @"\ConvertedThirdPartyXMLforFooter.xml";
                StringReader sreader = new StringReader(serializeXMLforFooter);
                XmlTextReader xmlreader = new XmlTextReader(sreader);
                string mappedfilePathforFooter = XMLUtilities.GetTransformed(xmlreader, convertedXMLforFooter, xsltForFooterStartUpPath);
                dsFooter.ReadXml(mappedfilePathforFooter);
                return dsFooter;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dsFooter;
        }

        /// <summary>
        /// Convert Data Set into XML String
        /// </summary>
        /// <param name="ds">Data Set </param>
        /// <returns>xml String</returns>
        private string ConvertDataSetIntoXML(DataSet ds)
        {
            string xmlString = null;
            try
            {
                StringBuilder writer = new StringBuilder();
                BusinessObjects.ThirdPartyStringWriterWithEncoding stringWriter = new BusinessObjects.ThirdPartyStringWriterWithEncoding(writer, Encoding.UTF8);

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    ConformanceLevel = ConformanceLevel.Document,
                    OmitXmlDeclaration = false,
                    CloseOutput = true,
                    Indent = true,
                    IndentChars = "  ",
                    NewLineHandling = NewLineHandling.Replace
                };

                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);

                ds.WriteXml(xmlWriter);
                xmlWriter.Close();
                    xmlString = writer.ToString();
                writer = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return xmlString;
        }

        /// <summary>
        /// Rmove columns and data which are not required into ouput third party data
        /// </summary>
        /// <param name="ds">complate converted Data Set from xslt </param>
        /// <returns>Data Set after remove not required columns</returns>
        private DataSet RemoveColumn(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            try
            {
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    if (ColumnExists(dt.Columns[i].ColumnName) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLMAINTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLCHILDTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLFOOTERMAINTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLFOOTERCHILDTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLHEADERMAINTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLHEADERCHILDTAG))
                    {
                        dt.Columns.RemoveAt(i);
                        iColCount--;
                        i--;
                    }
                    dt.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }

        private void GenerateUserDefindFormat(string filePath, string fileNameToBeDisplayed, string delimiterSymbol)
        {
            Prana.BusinessObjects.ThirdPartyFileFormat tPFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
            try
            {
                StringBuilder s = new StringBuilder();

                DateTime dateHeader = txtDayLightSaving.Value;
                string dateFormat = dateHeader.ToString("MM/dd/yyyy:HHmmss");

                int recordCount = 0;// to display in the footer section
                //int statusID = 1;
                bool isDataSaved = SaveMethod(false);

                // Ankit Gupta 01/22/2014: Allowed AllDataParties and Executing Brokers to save TaxlotStates
                if (!tPFileFormat.ExportOnly)
                {
                    UpdateTaxlotStateAndSaveData(_datasetFromFilteredRowsDs, fileNameToBeDisplayed);
                    UpdateTaxlotsToIgnoreState();
                }
                if (isDataSaved)
                {
                    DataSet dsGrid = _datasetFromFilteredRowsDs;
                    if (dsGrid == null)
                    {
                        return;
                    }

                    recordCount = dsGrid.Tables[0].Rows.Count;

                    bool blntotalQtyFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ALLOCQTY);
                    bool blnnetNotionalFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALNETNOTIONAL);
                    bool blngrossAmountFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALGROSSAMOUNT);
                    double totalQtytoSend = 0.0;
                    double internalNetNotionaltoSend = 0.0;
                    double internalGrossAmounttoSend = 0.0;
                    if (blntotalQtyFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            totalQtytoSend = totalQtytoSend + Convert.ToDouble(row[HEADCOL_ALLOCQTY]);
                        }
                    }

                    if (blnnetNotionalFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            internalNetNotionaltoSend = internalNetNotionaltoSend + Convert.ToDouble(row[HEADCOL_INTERNALNETNOTIONAL]);
                        }
                    }

                    if (blngrossAmountFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            internalGrossAmounttoSend = internalGrossAmounttoSend + Convert.ToDouble(row[HEADCOL_INTERNALGROSSAMOUNT]);
                        }
                    }

                    StreamWriter sw = new StreamWriter(filePath);

                    #region Header Code

                    string fileHeaderReq = string.Empty;
                    bool blnFileHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEHEADER);
                    if (blnFileHeaderContains)
                    {
                        fileHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEHEADER].ToString();
                    }
                    if (fileHeaderReq.ToUpper().Equals("TRUE"))
                    {
                        Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                        if (string.IsNullOrEmpty(thirdPartyFileFormat.HeaderFile))
                        {
                            MessageBox.Show("No Header XSLT is available for the selected ThirdParty. Please contact the administrator!", "Information");
                            return;
                        }
                        string headerXsltPath = thirdPartyFileFormat.HeaderFile;
                        // get the XSLT Name
                        string headerXsltName = headerXsltPath.Substring(headerXsltPath.LastIndexOf("\\") + 1);
                        // complete the path from StartUp Path
                        string xsltForHeaderStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + headerXsltName;

                        Prana.BusinessObjects.ThirdPartyFlatFileHeader thirdPartyFlatFileHeader = new Prana.BusinessObjects.ThirdPartyFlatFileHeader();

                        thirdPartyFlatFileHeader.Date = txtDayLightSaving.Text;
                        thirdPartyFlatFileHeader.DateAndTime = dateFormat;
                        thirdPartyFlatFileHeader.RecordCount = recordCount;

                        string serializeXMLforHeader = XMLUtilities.SerializeToXML(thirdPartyFlatFileHeader);

                        string convertedXMLforHeader = Application.StartupPath + @"\ConvertedThirdPartyXMLforHeader.xml";
                        StringReader sr = new StringReader(serializeXMLforHeader);
                        XmlTextReader xreader = new XmlTextReader(sr);
                        string mappedfilePathforHeader = XMLUtilities.GetTransformed(xreader, convertedXMLforHeader, xsltForHeaderStartUpPath);

                        if (!mappedfilePathforHeader.Equals(""))
                        {
                            DataSet dsHeader = new DataSet();
                            dsHeader.ReadXml(mappedfilePathforHeader);
                            if (dsHeader.Tables.Count <= 0)
                            {
                                MessageBox.Show("No data available for Header", "Information");
                                return;
                            }
                            DataTable dtHeader = dsHeader.Tables[0];

                            string rowHeaderReq = string.Empty;
                            bool blnRowHeaderContains = dtHeader.Columns.Contains(HEADCOL_ROWHEADER);
                            if (blnRowHeaderContains)
                            {
                                rowHeaderReq = dtHeader.Rows[0][HEADCOL_ROWHEADER].ToString();
                            }

                            int iColCountHeader = dtHeader.Columns.Count;
                            // check row header requires, if true the write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                // First of all write the column headers.
                                for (int i = 0; i < iColCountHeader; i++)
                                {
                                    if (!ColumnExists(dtHeader.Columns[i].ColumnName) &&
                                        !dtHeader.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        s.Append(dtHeader.Columns[i]).Append(delimiterSymbol);
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                                sw.WriteLine(s.ToString());
                                s = new StringBuilder();
                            }
                            // Now write all the row data.
                            foreach (DataRow dr in dtHeader.Rows)
                            {
                                for (int i = 0; i < iColCountHeader; i++)
                                {
                                    if (!ColumnExists(dtHeader.Columns[i].ColumnName) &&
                                        !dtHeader.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        if (!Convert.IsDBNull(dr[i]))
                                        {
                                            s.Append(dr[i].ToString()).Append(delimiterSymbol);
                                        }
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                            }
                            sw.WriteLine(s.ToString());
                            s = new StringBuilder();
                        }
                    }

                    #endregion Header Code

                    string groupAllocationRequired = string.Empty;
                    // check group and allocation requires
                    bool blnGroupAlloReq = dsGrid.Tables[0].Columns.Contains(HEADCOL_GroupAllocationReq);
                    if (blnGroupAlloReq)
                    {
                        groupAllocationRequired = dsGrid.Tables[0].Rows[0][HEADCOL_GroupAllocationReq].ToString();
                    }
                    // general delimited file generation code
                    #region general delimited file generation code
                    if (groupAllocationRequired.Equals(string.Empty) || groupAllocationRequired.ToUpper().Equals("FALSE"))
                    {
                        string rowHeaderReq = string.Empty;
                        bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                        if (blnRowHeaderContains)
                        {
                            rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                        }

                        DataTable dt = dsGrid.Tables[0];
                        int iColCount = dt.Columns.Count;

                        // check column header requires, if true the write
                        if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                        {
                            //if required, First of all write the column header. 
                            for (int i = 0; i < iColCount; i++)
                            {
                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                    !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                {
                                    s.Append(dt.Columns[i]).Append(delimiterSymbol);
                                }
                            }
                            s.Remove(s.Length - 1, 1);
                            sw.WriteLine(s.ToString());
                            s = new StringBuilder();
                        }
                        // Now write all the data rows.
                        foreach (DataRow dr in dt.Rows)
                        {
                            for (int i = 0; i < iColCount; i++)
                            {

                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                     !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                {
                                    if (!Convert.IsDBNull(dr[i]))
                                    {
                                        s.Append(dr[i].ToString()).Append(delimiterSymbol);
                                    }
                                }
                            }
                            s.Remove(s.Length - 1, 1);
                            //NewLine does the job, and writing is done only once
                            s.Append(Environment.NewLine);
                        }
                        sw.Write(s.ToString());
                        s = new StringBuilder();
                    }

                    #endregion general delimited file generation code
                    // group  and allocation generation code
                    #region group  and allocation generation code
                    else if (groupAllocationRequired.ToUpper().Equals("TRUE"))
                    {
                        string rowHeaderReq = string.Empty;

                        // Ankit Gupta on 08222014: Record Count is reset as records are counted separately where GroupAllocationReq tag is set true
                        recordCount = 0;
                        bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                        if (blnRowHeaderContains)
                        {
                            rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                        }

                        List<string> groupHeadingColl = new List<string>();
                        List<string> allocationHeadingColl = new List<string>();

                        DataTable dt = dsGrid.Tables[0];
                        bool groupEndsPassed = false;
                        int iColCount = dt.Columns.Count;
                        // collect headers for Group and Allocations
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                            {
                                groupEndsPassed = true;
                            }
                            if (!dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEndsPassed.Equals(false))
                            {
                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                     !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                {
                                    if (!groupHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                    {
                                        groupHeadingColl.Add(dt.Columns[i].ToString());
                                    }
                                }
                            }
                            else
                            {
                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                     !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                {
                                    if (!allocationHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                    {
                                        allocationHeadingColl.Add(dt.Columns[i].ToString());
                                    }
                                }
                            }
                        }

                        Dictionary<long, DataRow> groupDataDict = new Dictionary<long, DataRow>();
                        Dictionary<long, List<DataRow>> allocationDataDict = new Dictionary<long, List<DataRow>>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            for (int i = 0; i < iColCount; i++)
                            {
                                if (dt.Columns[i].ColumnName.Equals(HEADCOL_PBUNIQUEID))
                                {
                                    if (!groupDataDict.ContainsKey(Convert.ToInt64(dr[i].ToString())))
                                    {
                                        groupDataDict.Add(Convert.ToInt64(dr[i].ToString()), dr);
                                        recordCount = recordCount + 1;
                                        List<DataRow> datarowcoll = new List<DataRow>();
                                        datarowcoll.Add(dr);
                                        allocationDataDict.Add(Convert.ToInt64(dr[i].ToString()), datarowcoll);
                                        recordCount = recordCount + 1;
                                    }
                                    else
                                    {
                                        List<DataRow> datarowList = allocationDataDict[Convert.ToInt64(dr[i].ToString())];
                                        datarowList.Add(dr);
                                        allocationDataDict[Convert.ToInt64(dr[i].ToString())] = datarowList;
                                        recordCount = recordCount + 1;
                                    }
                                }
                            }
                        }

                        bool groupEnds = false;
                        foreach (KeyValuePair<long, DataRow> kvp in groupDataDict)
                        {
                            List<DataRow> allocationList = new List<DataRow>();
                            groupEnds = false;
                            if (groupDataDict.ContainsKey(kvp.Key))
                            {
                                if (allocationDataDict.ContainsKey(kvp.Key))
                                {
                                    allocationList = allocationDataDict[kvp.Key];
                                }

                                // check row header requires, if true then write
                                if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                                {
                                    //write header for group  
                                    int grpHeadcolCount = groupHeadingColl.Count;
                                    for (int i = 0; i < grpHeadcolCount; i++)
                                    {
                                        s.Append(groupHeadingColl[i]).Append(delimiterSymbol);
                                    }
                                    s.Remove(s.Length - 1, 1);
                                    sw.WriteLine(s.ToString());
                                    s = new StringBuilder();
                                }
                                // write row data for group
                                DataRow groupRow = groupDataDict[kvp.Key];
                                int colCount = groupRow.Table.Columns.Count;
                                for (int i = 0; i < colCount; i++)
                                {
                                    if (groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                    {
                                        groupEnds = true;
                                    }
                                    if (!groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEnds.Equals(false))
                                    {
                                        if (!ColumnExists(groupRow.Table.Columns[i].ColumnName) &&
                                            !groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                        {
                                            s.Append(groupRow[i].ToString()).Append(delimiterSymbol);
                                        }
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                                sw.WriteLine(s.ToString());
                                s = new StringBuilder();

                                // check row header requires, if true then write
                                if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                                {
                                    //write header for allocation 
                                    int allHeadcolCount = allocationHeadingColl.Count;
                                    for (int i = 0; i < allHeadcolCount; i++)
                                    {
                                        s.Append(allocationHeadingColl[i]).Append(delimiterSymbol);
                                    }
                                    s.Remove(s.Length - 1, 1);
                                    sw.WriteLine(s.ToString());
                                    s = new StringBuilder();
                                }
                                //allocationList
                                foreach (DataRow dtRow in allocationList)
                                {
                                    bool alloends = false;
                                    int allColCount = dtRow.Table.Columns.Count;
                                    for (int i = 0; i < allColCount; i++)
                                    {
                                        if (dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                        {
                                            alloends = true;
                                        }
                                        if (!dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && alloends.Equals(false))
                                        {

                                        }
                                        else
                                        {
                                            if (!ColumnExists(dtRow.Table.Columns[i].ColumnName) &&
                                                !dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                            {
                                                s.Append(dtRow[i].ToString()).Append(delimiterSymbol);
                                            }
                                        }
                                    }

                                    s.Remove(s.Length - 1, 1);
                                    //NewLine does the job, and writing is done only once
                                    s.Append(Environment.NewLine);
                                }
                                sw.Write(s.ToString());
                                s = new StringBuilder();
                            }
                        }
                    }
                    #endregion group  and allocation generation code

                    #region Footer Code


                    string fileFooterReq = string.Empty;

                    bool blnFileFooterContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEFOOTER);
                    if (blnFileFooterContains)
                    {
                        fileFooterReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEFOOTER].ToString();
                    }
                    if (fileFooterReq.ToUpper().Equals("TRUE"))
                    {

                        Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                        if (string.IsNullOrEmpty(thirdPartyFileFormat.FooterFile))
                        {
                            MessageBox.Show("No Footer XSLT is available for the selected ThirdParty. Please contact the administrator!", "Information");
                            return;
                        }
                        string footerXsltPath = thirdPartyFileFormat.FooterFile;
                        // get the XSLT Name
                        string footerXsltName = footerXsltPath.Substring(footerXsltPath.LastIndexOf("\\") + 1);
                        // complete the path from StartUp Path
                        string xsltForFooterStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + footerXsltName;
                        //if (recordCount == 0)
                        //{
                        //    recordCount = dsGrid.Tables[0].Rows.Count;
                        //}

                        Prana.BusinessObjects.ThirdPartyFlatFileFooter thirdPartyFlatFileFooter = new Prana.BusinessObjects.ThirdPartyFlatFileFooter();
                        thirdPartyFlatFileFooter.RecordCount = recordCount;
                        thirdPartyFlatFileFooter.TotalQty = totalQtytoSend;
                        thirdPartyFlatFileFooter.Date = txtDayLightSaving.Text;
                        thirdPartyFlatFileFooter.DateAndTime = dateFormat;
                        thirdPartyFlatFileFooter.InternalNetNotional = internalNetNotionaltoSend;
                        thirdPartyFlatFileFooter.InternalGrossAmount = internalGrossAmounttoSend;

                        string serializeXMLforFooter = XMLUtilities.SerializeToXML(thirdPartyFlatFileFooter);

                        string convertedXMLforFooter = Application.StartupPath + @"\ConvertedThirdPartyXMLforFooter.xml";
                        StringReader sreader = new StringReader(serializeXMLforFooter);
                        XmlTextReader xmlreader = new XmlTextReader(sreader);
                        string mappedfilePathforFooter = XMLUtilities.GetTransformed(xmlreader, convertedXMLforFooter, xsltForFooterStartUpPath);

                        if (!mappedfilePathforFooter.Equals(""))
                        {
                            DataSet dsFooter = new DataSet();
                            dsFooter.ReadXml(mappedfilePathforFooter);
                            if (dsFooter.Tables.Count <= 0)
                            {
                                MessageBox.Show("No data available for Footer", "Information");
                                return;
                            }
                            DataTable dtFooter = dsFooter.Tables[0];

                            string rowHeaderReq = string.Empty;
                            bool blnRowHeaderContains = dtFooter.Columns.Contains(HEADCOL_ROWHEADER);
                            if (blnRowHeaderContains)
                            {
                                rowHeaderReq = dtFooter.Rows[0][HEADCOL_ROWHEADER].ToString();
                            }

                            int iColCountFooter = dtFooter.Columns.Count;
                            // check column header requires, if true the write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                // First we will write the headers.    
                                for (int i = 0; i < iColCountFooter; i++)
                                {
                                    if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                        !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        s.Append(dtFooter.Columns[i]).Append(delimiterSymbol);
                                    }
                                }

                                s.Remove(s.Length - 1, 1);
                                sw.WriteLine(s.ToString());
                                s = new StringBuilder();
                            }
                            // Now write all the rows.
                            foreach (DataRow dr in dtFooter.Rows)
                            {
                                for (int i = 0; i < iColCountFooter; i++)
                                {
                                    if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                        !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        if (!Convert.IsDBNull(dr[i]))
                                        {
                                            s.Append(dr[i].ToString()).Append(delimiterSymbol);
                                        }
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                            }
                            sw.Write(s.ToString());
                            s = new StringBuilder();
                        }
                    }

                    #endregion Footer Code

                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                        toolStripStatusLabel1.Text = "Generated";
                        //set backup on startup path
                        SetEODFileBackUp(filePath);
                        if (!tPFileFormat.DoNotShowFileOpenDialogue)
                        {
                            ViewFileOpenDialogue(filePath, fileNameToBeDisplayed);
                            //FileOpenDialogue frmTPOpenDialogue = new FileOpenDialogue(fileNameToBeDisplayed, filePath);
                            //frmTPOpenDialogue.ShowDialog();
                        }
                        _blnchange = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Keep generated file backup on startup path
        /// </summary>
        /// <param name="filePath"></param>
        private void SetEODFileBackUp(string filePath)
        {
            try
            {
                // Target path i.e. applcation startuppath + Third Party Type + Third Party Name + Date wise folder (i.e. as user will select from UI)
                string targetPath_PBName = Application.StartupPath + @"\EOD_BackUp" + @"\" + cmbThirdPartyType.Text + @"\" + cmbThirdParty.Text + @"\" + txtDayLightSaving.Value.ToString("yyyyMMdd");
                string dateWithTimeStamp = txtDayLightSaving.Value.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmsstt"); ;
                string fileExtension = Path.GetExtension(filePath);
                string fileNameWithOutExtension = Path.GetFileNameWithoutExtension(filePath);
                // file name with datetime stamp
                string fileName = fileNameWithOutExtension + "_" + dateWithTimeStamp + fileExtension;

                string destFile = System.IO.Path.Combine(targetPath_PBName, fileName);

                if (!System.IO.Directory.Exists(targetPath_PBName))
                {
                    System.IO.Directory.CreateDirectory(targetPath_PBName);
                }
                System.IO.File.Copy(filePath, destFile, true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateTaxlotStateAndSaveData(DataSet dsGrid, string fileName)
        {
            Prana.BusinessObjects.ThirdPartyFileFormat fileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
            try
            {
                if (dsGrid != null)
                {
                    string taxLotIDs = string.Empty;
                    string deletedtaxLotIDs = "<TaxLots>";
                    string taxlotXmlToInsert = "<TaxLots>";

                    if (chkL2Data.Checked == true)
                    {
                        // taxLotIDs = "<TaxLots IsL1Data=\"" + "True\"" + " " + "ThirdPartyFormatID=\"" + fileFormat.FileFormatId + "\">";
                        taxLotIDs = "<TaxLots IsL1Data=\"" + "True" + "\">";
                        taxlotXmlToInsert = "<TaxLots IsL1Data=\"" + "True" + "\">";
                    }
                    else
                    {
                        taxLotIDs = "<TaxLots IsL1Data=\"" + "False" + "\">";
                        taxlotXmlToInsert = "<TaxLots IsL1Data=\"" + "False" + "\">";
                    }
                    // taxLotIDs += "<ThirdPartyFormatID=\"" + fileFormat.FileFormatId  + "\">";
                    DataTable dtUpdateTaxLotState = dsGrid.Tables[0];
                    DataColumnCollection columns = dtUpdateTaxLotState.Columns;
                    //int columnCount = dtUpdateTaxLotState.Columns.Count;
                    int count = 0;
                    PranaTaxLotState myEnum; int stateID = 0;

                    foreach (DataRow dr in dtUpdateTaxLotState.Rows)
                    {
                        if (Regex.IsMatch(dr["EntityID"].ToString(), "^[0-9]+$", RegexOptions.Compiled))
                            taxLotIDs += "<TaxLot TaxLotID =\"" + dr["EntityID"].ToString() + "\"/>";

                        if (fileFormat.ClearExternalTransID && dr["TaxLotState"].Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Deleted.ToString()))
                        {
                            if (Regex.IsMatch(dr["EntityID"].ToString(), "^[0-9]+$", RegexOptions.Compiled))
                                deletedtaxLotIDs += "<TaxLot TaxLotID =\"" + dr["EntityID"].ToString() + "\"/>";
                            count++;
                        }

                        if (columns.Contains("EntityID") && columns.Contains("TaxLotState") && dr["TaxLotState"] != null)
                        {
                            stateID = 0;

                            if (!string.IsNullOrEmpty(dr["TaxLotState"].ToString()) && (Enum.TryParse<PranaTaxLotState>(dr["TaxLotState"].ToString(), out myEnum)))
                                stateID = (int)(PranaTaxLotState)Enum.Parse(typeof(PranaTaxLotState), dr["TaxLotState"].ToString());

                            if (Regex.IsMatch(dr["EntityID"].ToString(), "^[0-9]+$", RegexOptions.Compiled))
                                taxlotXmlToInsert +=
                                    //String.Format("<TaxLot TaxLotID =\"{0}\" TaxlotState =\"{1}\"/>", dr["EntityID"], dr["TaxLotStateID"]);
                                    String.Format("<TaxLot TaxLotID =\"{0}\" TaxlotState =\"{1}\"/>", dr["EntityID"], stateID);
                        }
                    }

                    taxLotIDs += "</TaxLots>";
                    taxlotXmlToInsert += "</TaxLots>";
                    deletedtaxLotIDs += "</TaxLots>";

                    ThirdPartyFlatFileManager.InsertPBWiseTaxlotState(_thirdPartyID, _fileFormatID, taxlotXmlToInsert);
                    ThirdPartyFlatFileManager.UpdateTaxlotState(_thirdPartyID, _fileFormatID, taxLotIDs, deletedtaxLotIDs, fileFormat.GenerateCancelNewForAmend);

                    if (count > 0)
                    {
                        MessageBox.Show("External Transaction Id(s) cleared, please refresh Allocation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    Int64 fileId = Int64.Parse(DateTime.Now.ToString("MMddHHmmss"));

                    ThirdPartyFlatFileManager.SaveThirdPartyDetails(_thirdPartyID, DateTime.Now, fileName, fileId, taxLotIDs);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateTaxlotsToIgnoreState()
        {
            try
            {

                string taxLotIDs = string.Empty;
                if (chkL2Data.Checked == true)
                {
                    taxLotIDs = "<TaxLots IsL1Data=\"" + "True" + "\">";
                }
                else
                {
                    taxLotIDs = "<TaxLots IsL1Data=\"" + "False" + "\">";
                }
                int tableCount = _dsXML.Tables.Count;
                DataTable dt = new DataTable();
                if (tableCount.Equals(1))
                {
                    dt = _dsXML.Tables[0];
                }
                else
                {
                    dt = _dsXML.Tables[1];
                }

                foreach (DataRow row in dt.Rows)
                {
                    if (row["TaxlotState"].ToString().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString()))
                    {
                        taxLotIDs += "<TaxLot TaxLotID =\"" + row[HEADCOL_EntityID].ToString() + "\"" + " TaxLotState =\"" + (int)Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore + "\"/>";
                    }
                }
                if (_taxLotIgnoreStateDict.Count > 0)
                {
                    foreach (KeyValuePair<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> kvp in _taxLotIgnoreStateDict)
                    {
                        string TaxlotId = kvp.Key;
                        if (_taxLotIgnoreStateDict[TaxlotId].Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated))
                        {
                            taxLotIDs += "<TaxLot TaxLotID =\"" + TaxlotId + "\"" + " TaxLotState =\"" + (int)Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated + "\"/>";
                        }
                    }
                }

                if (_deletedToIgnoreDict.Count > 0)
                {
                    ICollection<String> orig = new List<string>();

                    foreach (KeyValuePair<string, string> keyvalpair in _deletedToIgnoreDict)
                    {
                        orig.Add(keyvalpair.Key);
                    }

                    foreach (String key in orig)
                    {
                        if (_deletedToIgnoreDict.ContainsKey(key))
                        {
                            _deletedToIgnoreDict[key] = "yes";
                        }
                    }
                }


                taxLotIDs += "</TaxLots>";
                ThirdPartyFlatFileManager.UpdateTaxlotsToIgnoreState(_thirdPartyID, taxLotIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool ColumnExists(string colName)
        {
            if (colName.Equals("CompanyID") || colName.Equals("ThirdPartyID") || colName.Equals("CompanyAccountID") ||
                colName.Equals("AssetID") || colName.Equals("UnderLyingID") || colName.Equals("CurrencyID") ||
                colName.Equals("ExchangeID") || colName.Equals("AUECID") || colName.Equals("CompanyAccountTypeID") ||
                colName.Equals("CommissionRateTypeID") || colName.Equals("ThirdPartyTypeID") || colName.Equals("CompanyCVID") ||
                colName.Equals("VenueID") || colName.Equals(HEADCOL_EntityID) || colName.Equals("CounterPartyID") ||
                colName.Equals("TradAccntID") || colName.Equals("GroupEnds") || colName.Equals(HEADCOL_GroupAllocationReq) ||
                colName.Equals(HEADCOL_FILEHEADER) || colName.Equals(HEADCOL_FILEFOOTER) || colName.Equals(HEADCOL_PBUNIQUEID) ||
                colName.Equals(HEADCOL_ROWHEADER) || colName.Equals("TaxLotStateID") || colName.ToUpper().Equals(HEADCOL_ALLOCQTY) ||
                colName.Equals("TaxLots_Id") || colName.Equals("Group_Id") || colName.Equals("TaxLots_ThirdPartyFlatFileDetail") ||
                colName.Equals("TaxLotState1") || colName.Equals("IsCaptionChangeRequired") || colName.Equals("FromDeleted") ||
                colName.Equals(HEADCOL_INTERNALNETNOTIONAL) || colName.Equals(HEADCOL_INTERNALGROSSAMOUNT) ||
                colName.ToUpper().Equals(HEADCOL_XMLCHILDTAG) || colName.ToUpper().Equals(HEADCOL_XMLMAINTAG))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GenerateExcelFile(string filePath, string fileNameToBeDisplayed)
        {
            Prana.BusinessObjects.ThirdPartyFileFormat tPFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
            try
            {
                DateTime dateHeader = txtDayLightSaving.Value;
                string dateFormat = dateHeader.ToString("MM/dd/yyyy:HHmmss");

                int recordCount = 0;// to display in the footer section
                //int statusID = 1;
                bool isDataSaved = SaveMethod(false);
                if (!tPFileFormat.ExportOnly)
                {
                    UpdateTaxlotStateAndSaveData(_datasetFromFilteredRowsDs, fileNameToBeDisplayed);
                    UpdateTaxlotsToIgnoreState();
                }
                if (isDataSaved)
                {
                    DataSet dsGrid = _datasetFromFilteredRowsDs;
                    if (dsGrid == null)
                    {
                        return;
                    }

                    recordCount = dsGrid.Tables[0].Rows.Count;

                    bool blntotalQtyFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ALLOCQTY);
                    bool blnnetNotionalFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALNETNOTIONAL);
                    bool blngrossAmountFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALGROSSAMOUNT);

                    double internalNetNotionaltoSend = 0.0;
                    double internalGrossAmounttoSend = 0.0;

                    double totalQtytoSend = 0.0;
                    if (blntotalQtyFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            totalQtytoSend = totalQtytoSend + Convert.ToDouble(row[HEADCOL_ALLOCQTY]);
                        }
                    }


                    if (blnnetNotionalFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            internalNetNotionaltoSend = internalNetNotionaltoSend + Convert.ToDouble(row[HEADCOL_INTERNALNETNOTIONAL]);
                        }
                    }

                    if (blngrossAmountFieldContains)
                    {
                        foreach (DataRow row in dsGrid.Tables[0].Rows)
                        {
                            internalGrossAmounttoSend = internalGrossAmounttoSend + Convert.ToDouble(row[HEADCOL_INTERNALGROSSAMOUNT]);
                        }
                    }

                    DataTable dsToExport = new DataTable();
                    DataTable dtHeader = null;

                    #region Header Code

                    string fileHeaderReq = string.Empty;
                    bool blnFileHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEHEADER);
                    if (blnFileHeaderContains)
                    {
                        fileHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEHEADER].ToString();
                    }
                    if (fileHeaderReq.ToUpper().Equals("TRUE"))
                    {
                        Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                        if (string.IsNullOrEmpty(thirdPartyFileFormat.HeaderFile))
                        {
                            MessageBox.Show("No Header XSLT is available for the selected ThirdParty. Please contact the administrator!", "Information");
                            return;
                        }
                        string headerXsltPath = thirdPartyFileFormat.HeaderFile;
                        // get the XSLT Name
                        string headerXsltName = headerXsltPath.Substring(headerXsltPath.LastIndexOf("\\") + 1);
                        // complete the path from StartUp Path
                        string xsltForHeaderStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + headerXsltName;

                        Prana.BusinessObjects.ThirdPartyFlatFileHeader thirdPartyFlatFileHeader = new Prana.BusinessObjects.ThirdPartyFlatFileHeader();

                        thirdPartyFlatFileHeader.Date = txtDayLightSaving.Text;
                        thirdPartyFlatFileHeader.DateAndTime = dateFormat;
                        thirdPartyFlatFileHeader.RecordCount = recordCount;

                        string serializeXMLforHeader = XMLUtilities.SerializeToXML(thirdPartyFlatFileHeader);

                        string convertedXMLforHeader = Application.StartupPath + @"\ConvertedThirdPartyXMLforHeader.xml";
                        StringReader sr = new StringReader(serializeXMLforHeader);
                        XmlTextReader xreader = new XmlTextReader(sr);
                        string mappedfilePathforHeader = XMLUtilities.GetTransformed(xreader, convertedXMLforHeader, xsltForHeaderStartUpPath);

                        if (!mappedfilePathforHeader.Equals(""))
                        {
                            DataSet dsHeader = new DataSet();
                            dsHeader.ReadXml(mappedfilePathforHeader);
                            if (dsHeader.Tables.Count <= 0)
                            {
                                MessageBox.Show("No data available for Header", "Information");
                                return;
                            }
                            dtHeader = dsHeader.Tables[0];
                        }
                    }
                    #endregion Header Code

                    string groupAllocationRequired = string.Empty;
                    // check group and allocation requires
                    bool blnGroupAlloReq = dsGrid.Tables[0].Columns.Contains(HEADCOL_GroupAllocationReq);
                    if (blnGroupAlloReq)
                    {
                        groupAllocationRequired = dsGrid.Tables[0].Rows[0][HEADCOL_GroupAllocationReq].ToString();
                    }

                    // general delimited file generation code
                    #region general delimited file generation code
                    if (groupAllocationRequired.Equals(string.Empty) || groupAllocationRequired.ToUpper().Equals("FALSE"))
                    {
                        string rowHeaderReq = string.Empty;
                        bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                        if (blnRowHeaderContains)
                        {
                            rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                        }

                        DataTable dt = dsGrid.Tables[0];
                        int iColCount = dt.Columns.Count;
                        int ind = 0;
                        // check column header requires, if true the write
                        if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                        {
                            //if required, First of all write the column header. 
                            for (int i = 0; i < iColCount; i++)
                            {
                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                    !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                    !dsToExport.Columns.Contains(dt.Columns[i].ColumnName))
                                {
                                    dsToExport.Columns.Add(dt.Columns[i].ColumnName);
                                }
                            }
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            for (int i = 0; i < iColCount; i++)
                            {
                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                    !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                    !dsToExport.Columns.Contains(dt.Columns[i].ColumnName))
                                {
                                    dsToExport.Columns.Add(dt.Columns[i].ColumnName);
                                    dsToExport.Columns[dt.Columns[i].ColumnName].Caption = dr[dt.Columns[i].ColumnName].ToString();
                                }
                            }
                            ind = 1;
                        }

                        // Now write all the data rows.
                        for (; ind < dt.Rows.Count; ind++)
                        {
                            DataRow dr = dt.Rows[ind];
                            DataRow row = dsToExport.Rows.Add();
                            bool isValueInert = false;

                            for (int i = 0; i < iColCount; i++)
                            {

                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                     !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                     dsToExport.Columns.Contains(dt.Columns[i].ColumnName))
                                {
                                    if (!Convert.IsDBNull(dr[i]))
                                    {
                                        row[dt.Columns[i].ColumnName] = dr[i].ToString();
                                        isValueInert = true;
                                    }
                                }
                            }
                            if (!isValueInert)
                            {
                                dsToExport.Rows.Remove(row);
                            }
                        }
                    }

                    #endregion general delimited file generation code

                    // group  and allocation generation code
                    #region group  and allocation generation code
                    else if (groupAllocationRequired.ToUpper().Equals("TRUE"))
                    {
                        string rowHeaderReq = string.Empty;
                        bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                        if (blnRowHeaderContains)
                        {
                            rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                        }

                        List<string> groupHeadingColl = new List<string>();
                        List<string> allocationHeadingColl = new List<string>();

                        DataTable dt = dsGrid.Tables[0];
                        bool groupEndsPassed = false;
                        int iColCount = dt.Columns.Count;
                        // collect headers for Group and Allocations
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                            {
                                groupEndsPassed = true;
                            }
                            if (!dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEndsPassed.Equals(false))
                            {
                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                     !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                {
                                    if (!groupHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                    {
                                        groupHeadingColl.Add(dt.Columns[i].ToString());
                                    }
                                }
                            }
                            else
                            {
                                if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                     !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                {
                                    if (!allocationHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                    {
                                        allocationHeadingColl.Add(dt.Columns[i].ToString());
                                    }
                                }
                            }
                        }

                        Dictionary<long, DataRow> groupDataDict = new Dictionary<long, DataRow>();
                        Dictionary<long, List<DataRow>> allocationDataDict = new Dictionary<long, List<DataRow>>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            for (int i = 0; i < iColCount; i++)
                            {
                                if (dt.Columns[i].ColumnName.Equals(HEADCOL_PBUNIQUEID))
                                {
                                    if (!groupDataDict.ContainsKey(Convert.ToInt64(dr[i].ToString())))
                                    {
                                        groupDataDict.Add(Convert.ToInt64(dr[i].ToString()), dr);
                                        recordCount = recordCount + 1;
                                        List<DataRow> datarowcoll = new List<DataRow>();
                                        datarowcoll.Add(dr);
                                        allocationDataDict.Add(Convert.ToInt64(dr[i].ToString()), datarowcoll);
                                        recordCount = recordCount + 1;
                                    }
                                    else
                                    {
                                        List<DataRow> datarowList = allocationDataDict[Convert.ToInt64(dr[i].ToString())];
                                        datarowList.Add(dr);
                                        allocationDataDict[Convert.ToInt64(dr[i].ToString())] = datarowList;
                                        recordCount = recordCount + 1;
                                    }
                                }
                            }
                        }

                        bool groupEnds = false;
                        foreach (KeyValuePair<long, DataRow> kvp in groupDataDict)
                        {
                            List<DataRow> allocationList = new List<DataRow>();
                            groupEnds = false;
                            if (groupDataDict.ContainsKey(kvp.Key))
                            {
                                if (allocationDataDict.ContainsKey(kvp.Key))
                                {
                                    allocationList = allocationDataDict[kvp.Key];
                                }

                                // check row header requires, if true then write
                                if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                                {
                                    //write header for group  
                                    int grpHeadcolCount = groupHeadingColl.Count;
                                    for (int i = 0; i < grpHeadcolCount; i++)
                                    {
                                        if (!dsToExport.Columns.Contains(groupHeadingColl[i]))
                                            dsToExport.Columns.Add(groupHeadingColl[i]);
                                    }
                                }

                                // write row data for group
                                DataRow groupRow = groupDataDict[kvp.Key];
                                int colCount = groupRow.Table.Columns.Count;
                                for (int i = 0; i < colCount; i++)
                                {
                                    if (groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                    {
                                        groupEnds = true;
                                    }
                                    if (!groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEnds.Equals(false))
                                    {
                                        if (!ColumnExists(groupRow.Table.Columns[i].ColumnName) &&
                                            !groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                            !dsToExport.Columns.Contains(groupRow.Table.Columns[i].ColumnName))
                                        {
                                            dsToExport.Columns.Add(groupRow.Table.Columns[i].ColumnName);
                                        }
                                    }
                                }

                                // check row header requires, if true then write
                                if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                                {
                                    DataRow row = dsToExport.Rows.Add();
                                    //write header for allocation 
                                    int allHeadcolCount = allocationHeadingColl.Count;
                                    for (int i = 0; i < allHeadcolCount; i++)
                                    {
                                        row[i] = allocationHeadingColl[i];
                                    }
                                }

                                //allocationList
                                foreach (DataRow dtRow in allocationList)
                                {
                                    bool alloends = false;
                                    int allColCount = dtRow.Table.Columns.Count;
                                    DataRow row = dsToExport.Rows.Add();
                                    bool isValueInert = false;
                                    for (int i = 0; i < allColCount; i++)
                                    {
                                        if (dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                        {
                                            alloends = true;
                                        }
                                        if (!dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && alloends.Equals(false))
                                        {

                                        }
                                        else
                                        {
                                            if (!ColumnExists(dtRow.Table.Columns[i].ColumnName) &&
                                                !dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                                dsToExport.Columns.Contains(dtRow.Table.Columns[i].ColumnName))
                                            {
                                                row[dtRow.Table.Columns[i].ColumnName] = dtRow[i].ToString();
                                                isValueInert = true;
                                            }
                                        }
                                    }
                                    if (!isValueInert)
                                    {
                                        dsToExport.Rows.Remove(row);
                                    }
                                }
                            }
                        }
                    }
                    #endregion group  and allocation generation code

                    #region Footer Code

                    string fileFooterReq = string.Empty;

                    bool blnFileFooterContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEFOOTER);
                    if (blnFileFooterContains)
                    {
                        fileFooterReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEFOOTER].ToString();
                    }
                    if (fileFooterReq.ToUpper().Equals("TRUE"))
                    {

                        Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                        if (string.IsNullOrEmpty(thirdPartyFileFormat.FooterFile))
                        {
                            MessageBox.Show("No Footer XSLT is available for the selected ThirdParty. Please contact the administrator!", "Information");
                            return;
                        }
                        else
                        {
                            string footerXsltPath = thirdPartyFileFormat.FooterFile;
                            // get the XSLT Name
                            string footerXsltName = footerXsltPath.Substring(footerXsltPath.LastIndexOf("\\") + 1);
                            // complete the path from StartUp Path
                            string xsltForFooterStartUpPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() + @"\" + footerXsltName;
                            Prana.BusinessObjects.ThirdPartyFlatFileFooter thirdPartyFlatFileFooter = new Prana.BusinessObjects.ThirdPartyFlatFileFooter();
                            thirdPartyFlatFileFooter.RecordCount = recordCount;
                            thirdPartyFlatFileFooter.TotalQty = totalQtytoSend;
                            thirdPartyFlatFileFooter.Date = txtDayLightSaving.Text;
                            thirdPartyFlatFileFooter.DateAndTime = dateFormat;
                            thirdPartyFlatFileFooter.InternalNetNotional = internalNetNotionaltoSend;
                            thirdPartyFlatFileFooter.InternalGrossAmount = internalGrossAmounttoSend;
                            string serializeXMLforFooter = XMLUtilities.SerializeToXML(thirdPartyFlatFileFooter);
                            string convertedXMLforFooter = Application.StartupPath + @"\ConvertedThirdPartyXMLforFooter.xml";
                            StringReader sreader = new StringReader(serializeXMLforFooter);
                            XmlTextReader xmlreader = new XmlTextReader(sreader);
                            string mappedfilePathforFooter = XMLUtilities.GetTransformed(xmlreader, convertedXMLforFooter, xsltForFooterStartUpPath);

                            if (!mappedfilePathforFooter.Equals(""))
                            {
                                DataSet dsFooter = new DataSet();
                                dsFooter.ReadXml(mappedfilePathforFooter);
                                if (dsFooter.Tables.Count <= 0)
                                {
                                    MessageBox.Show("No data available for Footer", "Information");
                                    return;
                                }
                                DataTable dtFooter = dsFooter.Tables[0];

                                string rowHeaderReq = string.Empty;
                                bool blnRowHeaderContains = dtFooter.Columns.Contains(HEADCOL_ROWHEADER);
                                if (blnRowHeaderContains)
                                {
                                    rowHeaderReq = dtFooter.Rows[0][HEADCOL_ROWHEADER].ToString();
                                }

                                int iColCountFooter = dtFooter.Columns.Count;
                                int indColCounter = 0;

                                // check column header requires, if true the write
                                if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                                {
                                    DataRow row = dsToExport.Rows.Add();
                                    // First we will write the headers.    
                                    for (int i = 0; i < iColCountFooter; i++)
                                    {
                                        if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                            !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                        {
                                            row[indColCounter] = dtFooter.Columns[i].ColumnName;
                                            indColCounter++;
                                            if (indColCounter >= dsToExport.Columns.Count)
                                                dsToExport.Columns.Add();
                                        }
                                    }
                                }
                                // Now write all the rows.
                                foreach (DataRow dr in dtFooter.Rows)
                                {
                                    DataRow row = dsToExport.Rows.Add();
                                    indColCounter = 0;
                                    for (int i = 0; i < iColCountFooter; i++)
                                    {
                                        if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                            !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                        {
                                            if (!Convert.IsDBNull(dr[i]))
                                            {

                                                string value = dr[i].ToString();
                                                while (value.Length > 2 && (value.StartsWith("\n") || value.StartsWith("\t")))
                                                {
                                                    if (value.StartsWith("\n"))
                                                    {
                                                        row = dsToExport.Rows.Add();
                                                        indColCounter = 0;
                                                    }
                                                    value = value.Substring(1);
                                                }
                                                if (indColCounter >= dsToExport.Columns.Count)
                                                    dsToExport.Columns.Add();
                                                row[indColCounter] = value;
                                                indColCounter++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion Footer Code

                    if (dsToExport != null)
                    {
                        string rowHeaderReq = string.Empty;
                        bool blnRowHeaderContains = false;
                        if (dtHeader != null)
                        {
                            blnRowHeaderContains = dtHeader.Columns.Contains(HEADCOL_ROWHEADER);
                            if (blnRowHeaderContains)
                                rowHeaderReq = dtHeader.Rows[0][HEADCOL_ROWHEADER].ToString();
                        }
                        grdExportToExcel.DataSource = null;
                        grdExportToExcel.DataSource = dsToExport;
                        Infragistics.Documents.Excel.Workbook workBook = ultraGridExcelExporter1.Export(grdExportToExcel);
                        if (dtHeader != null && dtHeader.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtHeader.Columns.Count; i++)
                            {
                                string colName = dtHeader.Columns[i].ColumnName;
                                if (ColumnExists(colName) ||
                                    colName.ToUpper().Equals(HEADCOL_ROWHEADER) ||
                                    colName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                {
                                    dtHeader.Columns.Remove(colName);
                                    i--;
                                }
                            }
                            int rowCounter = 0;
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                workBook.Worksheets[0].Rows.Insert(0, dtHeader.Rows.Count + 1);
                                int colCounter = 0;
                                foreach (DataColumn col in dtHeader.Columns)
                                {
                                    workBook.Worksheets[0].Rows[rowCounter].Cells[colCounter].Value = col.ColumnName;
                                    colCounter++;
                                }
                                rowCounter++;
                            }
                            else
                                workBook.Worksheets[0].Rows.Insert(0, dtHeader.Rows.Count);
                            foreach (DataRow row in dtHeader.Rows)
                            {
                                int colCounter = 0;
                                foreach (DataColumn col in dtHeader.Columns)
                                {
                                    workBook.Worksheets[0].Rows[rowCounter].Cells[colCounter].Value = row[col].ToString();
                                    colCounter++;
                                }
                                rowCounter++;
                            }
                        }
                        workBook.Save(filePath);
                        toolStripStatusLabel1.Text = "Generated";
                        //set backup on startup path
                        SetEODFileBackUp(filePath);

                        if (!tPFileFormat.DoNotShowFileOpenDialogue)
                        {
                            ViewFileOpenDialogue(filePath, fileNameToBeDisplayed);
                        }
                        _blnchange = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ViewFileOpenDialogue(string filePath, string fileNameToBeDisplayed)
        {
            try
            {
                FileOpenDialogue frmTPOpenDialogue = new FileOpenDialogue(fileNameToBeDisplayed, filePath);
                frmTPOpenDialogue.ShowDialog();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// On the click of save button, the changes in the grid are save in the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsValid = ValidateThirdPartyReport();
                if ((IsValid) && (grdThirdParty.Rows.Count > 0))
                {
                    //int statusID = 0;
                    //string filePathName = string.Empty;
                    SaveMethod(true);
                    UpdateTaxlotsToIgnoreState();
                    _blnchange = false;
                    toolStripStatusLabel1.Text = "Data Saved";
                }
                else if (grdThirdParty.Rows.Count <= 0)
                {
                    MessageBox.Show("Nothing to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion Button events

        #region Combos, Checked  List , DateTime Picker and Check box events

        private void txtDayLightSaving_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (_blnnoForDate == false)
                //{
                //    if (grdThirdParty.DataSource != null)
                //    {
                //        DialogResult diares;
                //        diares = MessageBox.Show("Changing of Date may change the Data, Do you want to continue..", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                //        if (diares == DialogResult.No)
                //        {
                //            _blnnoForDate = true;
                //            return;
                //        }
                //        else
                //        {
                //            SetDefaultValues();
                //        }
                //    }
                //    else
                //    {
                //        SetDefaultValues();
                //    }
                //}
            }

            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        private void txtDayLightSaving_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (_blnnoForDate == true)
                {
                    txtDayLightSaving.Value = _getprevDate;
                }
                else
                {
                    if (grdThirdParty.DataSource != null)
                    {
                        DialogResult diares;
                        diares = MessageBox.Show("Changing of Date may change the Data, Do you want to continue..", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (diares == DialogResult.No)
                        {
                            _blnnoForDate = true;
                            return;
                        }
                        else
                        {
                            SetDefaultValues();
                        }
                    }
                    else
                    {
                        SetDefaultValues();
                    }
                }
                _blnnoForDate = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// the event is used to populate the chklistbox with the accounts of the selected thirdParty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmbThirdParty_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_blncmbvaluechange.Equals(false))
                {
                    if (grdThirdParty.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Changing the Third Party may change the Data, Do you want to continue...", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            //cmbThirdParty.Value = cmbvalue;
                            _blncmbvaluechange = true;
                            return;
                        }
                        else
                        {
                            //The thirdPartyID of the currently selected comboThirdParty is assigned to the variable.
                            _thirdPartyID = int.Parse(cmbThirdParty.Value.ToString());

                            //On change of thirdparty combo value, the chklist box for third party accounts is populated.
                            _blnChklistchange = true;
                            BindThirdPartyAccounts();
                            //Bind XSLT Format combo with the thirdpartyid
                            BindXSLTFormatType();
                            SetDefaultValues();
                            // set third party accounts selected all by default
                            SetThirdPartyAccountsChecked();
                        }
                    }
                    else
                    {
                        //The thirdPartyID of the currently selected comboThirdParty is assigned to the variable.
                        _thirdPartyID = int.Parse(cmbThirdParty.Value.ToString());
                        //On change of thirdparty combo value, the chklist box for third party accounts is populated.
                        BindThirdPartyAccounts();
                        //Bind XSLT Format combo with the thirdpartyid
                        BindXSLTFormatType();
                        _blnChklistchange = true;
                        SetDefaultValues();
                        SetThirdPartyAccountsChecked();
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetThirdPartyAccountsChecked()
        {
            try
            {
                if (chkLstThirdPartyAccounts.Items.Count > 0)
                {
                    chkSelectAllAccounts.CheckState = CheckState.Checked;
                }
                else
                {
                    chkSelectAllAccounts.CheckState = CheckState.Unchecked;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetAUECChecked()
        {
            try
            {
                if (chkLstAuec.Items.Count > 0)
                {
                    chkSelectAllAUECs.CheckState = CheckState.Checked;
                }
                else
                {
                    chkSelectAllAUECs.CheckState = CheckState.Unchecked;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetDefaultValues()
        {
            try
            {
                grdThirdParty.DataSource = null;
                // disable save and generate button
                btnSave.Enabled = false;
                btnGenerate.Enabled = false;
                toolStripStatusLabel1.Text = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbThirdParty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // if user says No to changing the Value of selected Third Party Value
                if (_blncmbvaluechange.Equals(true))
                {
                    // restore the third party value
                    cmbThirdParty.Value = _cmbThirdPartyValue;
                    cmbThirdParty.Text = _cmbThirdPartyText;
                    cmbThirdParty.Refresh();
                }
                _blncmbvaluechange = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbFormat_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_blncmbFormatvaluechange.Equals(false))
                {
                    if (grdThirdParty.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Changing the File Format may change the Data, Do you want to continue...", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            _blncmbFormatvaluechange = true;
                            return;
                        }
                        else
                        {
                            //The thirdPartyID of the currently selected comboThirdParty is assigned to the variable.
                            //_thirdPartyID = int.Parse(cmbThirdParty.Value.ToString());

                            ////On change of thirdparty combo value, the chklist box for third party accounts is populated.
                            //blnChklistchange = true;
                            //BindThirdPartyAccounts();
                            ////Bind XSLT Format combo with the thirdpartyid
                            //BindXSLTFormatType();
                            _fileFormatID = int.Parse(cmbFormat.Value.ToString());
                            SetDefaultValues();
                        }
                    }
                    else if (cmbFormat.Value != null)
                        _fileFormatID = int.Parse(cmbFormat.Value.ToString());
                }


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbFormat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_blncmbFormatvaluechange.Equals(true))
                {
                    // restore combo file format  value
                    cmbFormat.Value = _cmbFormatVal;
                    cmbFormat.Text = _cmbFormatText;
                    cmbFormat.Refresh();
                }
                _blncmbFormatvaluechange = false;
                if (cmbFormat.SelectedRow != null)
                {
                    Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat = (Prana.BusinessObjects.ThirdPartyFileFormat)cmbFormat.SelectedRow.ListObject;
                    if (!string.IsNullOrEmpty(thirdPartyFileFormat.StoredProcName))
                    {
                        chkL2Data.Enabled = false;
                        chkIncluedSent.Enabled = false;
                    }
                    else
                    {
                        chkL2Data.Enabled = true;
                        chkIncluedSent.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkLstThirdPartyAccounts_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdThirdParty.Rows.Count > 0)
                {
                    if (_blnChklistchange == false)
                    {
                        if (MessageBox.Show("Selection of different Account Account may change the Data, Do you want to continue..  ", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            _test = true;
                            return;
                        }
                    }
                }
                SetDefaultValues();
                if (chkLstThirdPartyAccounts.CheckedItems.Count == chkLstThirdPartyAccounts.Items.Count)
                {
                    chkSelectAllAccounts.CheckState = CheckState.Checked;
                }
                else if (chkLstThirdPartyAccounts.CheckedItems.Count < chkLstThirdPartyAccounts.Items.Count && chkLstThirdPartyAccounts.CheckedItems.Count != 0)
                {
                    chkSelectAllAccounts.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    chkSelectAllAccounts.CheckState = CheckState.Unchecked;
                }
                //_chkStateForAllAccounts = chkSelectAllAccounts.CheckState;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkLstThirdPartyAccounts_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                // check whether there is any Data or not
                if (grdThirdParty.Rows.Count > 0)
                {
                    // if user says 'No' to change the Data
                    if (_test == true)
                    {
                        // uncheck all the Items in the CheckedListBox
                        for (int i = 0, count = chkLstThirdPartyAccounts.Items.Count; i < count; i++)
                        {
                            chkLstThirdPartyAccounts.SetItemChecked(i, false);
                        }
                        // go for each item in the checked list box
                        for (int i = 0, count = chkLstThirdPartyAccounts.Items.Count; i < count; i++)
                        {
                            // get selected third party Account Id for comparision purpose
                            Prana.BusinessObjects.ThirdPartyPermittedAccount thirdPartyPermittedAccount = (Prana.BusinessObjects.ThirdPartyPermittedAccount)chkLstThirdPartyAccounts.Items[i];
                            int thirdPartyAccountID = Convert.ToInt32(thirdPartyPermittedAccount.CompanyAccountID);
                            //search in the stored values and compare the third party Account Ids,if found in the stored collection ,restore the state                       
                            for (int j = 0; j < _thirdPartyMainAccountColl.Count; j++)
                            {
                                if (_thirdPartyMainAccountColl[j] == thirdPartyAccountID)
                                {
                                    chkLstThirdPartyAccounts.SetItemChecked(i, true);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkL2Data_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_blnChkL2Change == false)
                {
                    if (grdThirdParty.DataSource != null)
                    {
                        DialogResult diares;
                        diares = MessageBox.Show("Changing of Level2 Data check box may change the Data, Do you want to continue..", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (diares == DialogResult.No)
                        {
                            _blnChkL2Change = true;
                            return;
                        }
                        else
                        {
                            SetDefaultValues();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkL2Data_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (_blnChkL2Change == true)
                {
                    chkL2Data.Checked = _chkL2Value;
                }
                _blnChkL2Change = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkSelectAllAccounts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdThirdParty.Rows.Count == 0)
                {
                    if (chkSelectAllAccounts.CheckState == CheckState.Checked)
                    {
                        for (int j = 0; j < chkLstThirdPartyAccounts.Items.Count; j++)
                        {
                            chkLstThirdPartyAccounts.SetItemChecked(j, true);
                        }
                    }
                    else if (chkSelectAllAccounts.CheckState == CheckState.Unchecked)
                    {
                        for (int j = 0; j < chkLstThirdPartyAccounts.Items.Count; j++)
                        {
                            chkLstThirdPartyAccounts.SetItemChecked(j, false);
                        }
                    }
                }
                else
                {
                    if (_blnChkAllAccountChange.Equals(false))
                    {
                        if (MessageBox.Show("Selection/Unselection of Account Account may change the Data, Do you want to continue..  ", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            for (int j = 0; j < chkLstThirdPartyAccounts.Items.Count; j++)
                            {
                                chkLstThirdPartyAccounts.SetItemChecked(j, false);
                            }
                            SetDefaultValues();
                            _blnChkAllAccountChange = false;
                        }
                        else
                        {
                            _blnChkAllAccountChange = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkSelectAllAccounts_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                // if user says 'No' to change the Data
                if (_blnChkAllAccountChange.Equals(true))
                {
                    if (grdThirdParty.Rows.Count > 0)
                    {
                        chkSelectAllAccounts.CheckState = _chkStateForAllAccounts;
                    }
                    _blnChkAllAccountChange = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void chkSelectAllAUECs_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdThirdParty.Rows.Count == 0)
                {
                    if (chkSelectAllAUECs.CheckState == CheckState.Checked)
                    {
                        for (int j = 0; j < chkLstAuec.Items.Count; j++)
                        {
                            chkLstAuec.SetItemChecked(j, true);
                        }
                    }
                    else if (chkSelectAllAUECs.CheckState == CheckState.Unchecked)
                    {
                        for (int j = 0; j < chkLstAuec.Items.Count; j++)
                        {
                            chkLstAuec.SetItemChecked(j, false);
                        }
                    }
                }
                else
                {
                    if (_blnSelectAllAUECChange.Equals(false))
                    {
                        if (MessageBox.Show("Selection/Unselection of AUEC may change the Data, Do you want to continue..  ", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            if (chkSelectAllAUECs.Checked.Equals(true))
                            {
                                for (int j = 0; j < chkLstAuec.Items.Count; j++)
                                {
                                    chkLstAuec.SetItemChecked(j, true);
                                }
                            }
                            else
                            {
                                for (int j = 0; j < chkLstAuec.Items.Count; j++)
                                {
                                    chkLstAuec.SetItemChecked(j, false);
                                }
                            }
                            SetDefaultValues();
                            _blnSelectAllAUECChange = false;
                        }
                        else
                        {
                            _blnSelectAllAUECChange = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkSelectAllAUECs_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                // if user says 'No' to change the Data
                if (_blnSelectAllAUECChange.Equals(true))
                {
                    if (grdThirdParty.Rows.Count > 0)
                    {
                        chkSelectAllAUECs.CheckState = _chkStateForAllAUECs;
                    }
                    _blnSelectAllAUECChange = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbThirdPartyType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_blncmbThirdPartyTypeValueChange.Equals(false))
                {
                    if (grdThirdParty.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Changing the Third Party Type may change the Data, Do you want to continue...", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            //cmbThirdParty.Value = cmbvalue;
                            _blncmbThirdPartyTypeValueChange = true;
                            return;
                        }
                        else
                        {
                            //The thirdPartyID of the currently selected comboThirdParty is assigned to the variable.
                            _thirdPartyTypeID = int.Parse(cmbThirdPartyType.Value.ToString());
                            grdThirdParty.DataSource = null;
                            //Bind Third Party
                            BindThirdParties();
                            //On change of thirdparty combo value, the chklist box for third party accounts is populated.
                            _blnChklistchange = true;
                            BindThirdPartyAccounts();

                            //Bind XSLT Format combo with the thirdpartyid
                            BindXSLTFormatType();
                            SetDefaultValues();
                            // set third party accounts selected all by default
                            SetThirdPartyAccountsChecked();
                        }
                    }
                    else
                    {
                        //The thirdPartyTypeID of the currently selected comboThirdPartyType is assigned to the variable.
                        _thirdPartyTypeID = int.Parse(cmbThirdPartyType.Value.ToString());
                        grdThirdParty.DataSource = null;
                        // Bind third parties
                        BindThirdParties();
                        //On change of thirdparty combo value, the chklist box for third party accounts is populated.
                        BindThirdPartyAccounts();
                        //Bind XSLT Format combo with the thirdpartyid
                        BindXSLTFormatType();
                        _blnChklistchange = true;
                        SetDefaultValues();
                        SetThirdPartyAccountsChecked();
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbThirdPartyType_TextChanged(object sender, EventArgs e)
        {

            try
            {
                // if user says No to changing the Value of selected Third Party Type Value
                if (_blncmbThirdPartyTypeValueChange.Equals(true))
                {
                    // restore the third party value
                    cmbThirdPartyType.Value = _cmbThirdPartyTypeValue;
                    cmbThirdPartyType.Text = _cmbThirdPartyTypeText;
                    cmbThirdPartyType.Refresh();
                }
                _blncmbThirdPartyTypeValueChange = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion Combos, Checked  List , DateTime Picker  events

        #region Grid events

        private void grdThirdParty_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            //  e.Cancel = true;
        }

        #endregion Grid events

        #region AUEC Related Stuff

        private void chkLstAuec_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetDefaultValues();
                if (chkLstAuec.CheckedItems.Count == chkLstAuec.Items.Count)
                {
                    chkSelectAllAUECs.CheckState = CheckState.Checked;
                }
                else if (chkLstAuec.CheckedItems.Count < chkLstAuec.Items.Count && chkLstAuec.CheckedItems.Count != 0)
                {
                    chkSelectAllAUECs.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    chkSelectAllAUECs.CheckState = CheckState.Unchecked;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkLstAuec_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                // check whether there is any Data or not
                if (grdThirdParty.Rows.Count > 0)
                {
                    // if user says 'No' to change the Data
                    if (_blnAUEClstTest == true)
                    {
                        // uncheck all the Items in the CheckedListBox
                        for (int i = 0, count = chkLstAuec.Items.Count; i < count; i++)
                        {
                            chkLstAuec.SetItemChecked(i, false);
                        }
                        // go for each item in the checked list box
                        for (int i = 0, count = chkLstAuec.Items.Count; i < count; i++)
                        {
                            // get selected third party Account Id for comparision purpose
                            Prana.Admin.BLL.AUEC auec = (Prana.Admin.BLL.AUEC)chkLstAuec.Items[i];
                            int auecID = Convert.ToInt32(auec.AUECID);
                            //search in the stored values and compare the AUEC Ids,if found in the stored collection ,restore the state                       
                            for (int j = 0; j < _AUECColl.Count; j++)
                            {
                                if (_AUECColl[j] == auecID)
                                {
                                    chkLstAuec.SetItemChecked(i, true);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        private void SetColourToRow()
        {
            try
            {
                //Out of memory related changes 
                //GroupByComparer is assigned this(Current object) instead of creating new object of ThirdPartReportControl each time
                UltraGridBand band = this.grdThirdParty.DisplayLayout.Bands[0];
                foreach (UltraGridRow row in grdThirdParty.Rows)
                {
                    string TaxLotState = row.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper();
                    if (TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated.ToString().ToUpper()))
                    {
                        row.Appearance.ForeColor = Color.Orange;
                    }
                    else if (TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Sent.ToString().ToUpper()))
                    {
                        row.Appearance.ForeColor = Color.LightGray;
                    }
                    else if (TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Amended.ToString().ToUpper()))
                    {
                        row.Appearance.ForeColor = Color.GreenYellow;
                    }
                    else if (TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Deleted.ToString().ToUpper()))
                    {
                        row.Appearance.ForeColor = Color.Red;
                    }
                    else if (TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString().ToUpper()))
                    {
                        row.Appearance.ForeColor = Color.HotPink;
                    }
                }

                band.SortedColumns.Add(HEADCOL_TAXLOTSTATE, false, true);
                band.Columns[HEADCOL_TAXLOTSTATE].GroupByComparer = this;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColourToRow(UltraGridRow activeRow)
        {
            try
            {
                UltraGridBand band = this.grdThirdParty.DisplayLayout.Bands[0];
                if (activeRow.Cells.Exists(HEADCOL_TAXLOTSTATE))
                {
                    band.SortedColumns.Add(HEADCOL_TAXLOTSTATE, false, true);
                    band.Columns[HEADCOL_TAXLOTSTATE].GroupByComparer = new ThirdPartyReportControl();
                }

                if (activeRow.Cells.Exists(HEADCOL_TAXLOTSTATE))
                {
                    if (activeRow.Cells[HEADCOL_TAXLOTSTATE] != null)
                    {
                        if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated.ToString().ToUpper()))
                        {
                            activeRow.Appearance.ForeColor = Color.Orange;
                        }
                        else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Sent.ToString().ToUpper()))
                        {
                            activeRow.Appearance.ForeColor = Color.LightGray;
                        }
                        else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Amended.ToString().ToUpper()))
                        {
                            activeRow.Appearance.ForeColor = Color.GreenYellow;
                        }
                        else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Deleted.ToString().ToUpper()))
                        {
                            activeRow.Appearance.ForeColor = Color.Red;
                        }
                        else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString().ToUpper()))
                        {
                            activeRow.Appearance.ForeColor = Color.HotPink;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdThirdParty_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                grdThirdParty.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                UltraGridBand band = this.grdThirdParty.DisplayLayout.Bands[0];
                if (band.Columns.Exists(HEADCOL_TAXLOTSTATE))
                {
                    grdThirdParty.DisplayLayout.Override.GroupByRowAppearance.ForeColor = Color.White;
                    SetColourToRow();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void IgnoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdThirdParty.UpdateData();
                DataTable dt = new DataTable();
                dt = _dsXML.Tables[0];

                UltraGridRow[] rows = grdThirdParty.Rows.GetFilteredInNonGroupByRows();

                foreach (UltraGridRow grdrow in rows)
                {
                    if ((bool)grdrow.Cells[HEADCOL_CheckBox].Value)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            if (dtrow[HEADCOL_EntityID].ToString().Equals(grdrow.Cells[HEADCOL_EntityID].Value.ToString()))
                            {
                                dtrow["TaxLotState"] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString();
                                _blnchange = true;

                                SetColourToRow(grdrow);

                                if (_taxLotIgnoreStateDict.ContainsKey(grdrow.Cells[HEADCOL_EntityID].Value.ToString()))
                                {
                                    _taxLotIgnoreStateDict[HEADCOL_EntityID] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore;
                                }

                                if (!_deletedToIgnoreDict.ContainsKey(grdrow.Cells[HEADCOL_EntityID].Value.ToString()) &&
                                    grdrow.Cells["FromDeleted"].Value.ToString().ToLower().Equals("yes"))
                                {
                                    _deletedToIgnoreDict.Add(grdrow.Cells[HEADCOL_EntityID].Value.ToString().ToLower(), "no");
                                }
                            }
                        }
                    }
                }

                int tableCount = _dsXML.Tables.Count;
                if (tableCount.Equals(2))
                {
                    dt = _dsXML.Tables[1];

                    foreach (UltraGridRow grdrow in rows)
                    {
                        if ((bool)grdrow.Cells[HEADCOL_CheckBox].Value)
                        {
                            if (grdrow.HasChild())
                            {
                                foreach (UltraGridRow childrow in grdrow.ChildBands[0].Rows)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        if (dtrow[HEADCOL_EntityID].ToString().Equals(childrow.Cells[HEADCOL_EntityID].Value.ToString()))
                                        {
                                            dtrow["TaxLotState"] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString();
                                            _blnchange = true;

                                            SetColourToRow(grdrow);

                                            if (_taxLotIgnoreStateDict.ContainsKey(grdrow.Cells[HEADCOL_EntityID].Value.ToString()))
                                            {
                                                _taxLotIgnoreStateDict[HEADCOL_EntityID] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                SetDefaultFilters();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strMessage = string.Empty;
            try
            {
                grdThirdParty.UpdateData();
                int tableCount = _dsXML.Tables.Count;
                DataTable dt = new DataTable();
                dt = _dsXML.Tables[0];

                UltraGridRow[] rows = grdThirdParty.Rows.GetFilteredInNonGroupByRows();

                foreach (UltraGridRow grdrow in rows)
                {
                    if ((bool)grdrow.Cells[HEADCOL_CheckBox].Value)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            if (dtrow[HEADCOL_EntityID].ToString().Equals(grdrow.Cells[HEADCOL_EntityID].Value.ToString()))
                            {
                                if (_deletedToIgnoreDict.ContainsKey(grdrow.Cells[HEADCOL_EntityID].Value.ToString()) && (_deletedToIgnoreDict[grdrow.Cells[HEADCOL_EntityID].Value.ToString()].Equals("yes")))
                                {
                                    strMessage = "Deleted Taxlots can not Re-Activated.";
                                    break;
                                }
                                if (_taxLotWithStateDict.ContainsKey(dtrow[HEADCOL_EntityID].ToString()))
                                {
                                    if (_taxLotWithStateDict[dtrow[HEADCOL_EntityID].ToString()].Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore))
                                    {
                                        dtrow["TaxLotState"] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated.ToString();
                                        _blnchange = true;
                                        SetColourToRow(grdrow);
                                    }
                                    else
                                    {
                                        dtrow["TaxLotState"] = _taxLotWithStateDict[dtrow[HEADCOL_EntityID].ToString()];
                                        _blnchange = true;
                                        SetColourToRow(grdrow);
                                    }
                                }
                                else
                                {
                                    dtrow["TaxLotState"] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated.ToString();
                                    _blnchange = true;
                                    SetColourToRow(grdrow);
                                }

                                if (_taxLotIgnoreStateDict.ContainsKey(dtrow[HEADCOL_EntityID].ToString()))
                                {
                                    _taxLotIgnoreStateDict[dtrow[HEADCOL_EntityID].ToString()] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated;
                                }
                            }
                        }
                    }
                }

                if (tableCount.Equals(2))
                {
                    dt = _dsXML.Tables[1];

                    foreach (UltraGridRow grdrow in rows)
                    {
                        if ((bool)grdrow.Cells[HEADCOL_CheckBox].Value)
                        {
                            if (grdrow.HasChild())
                            {
                                foreach (UltraGridRow childrow in grdrow.ChildBands[0].Rows)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        if (dtrow[HEADCOL_EntityID].ToString().Equals(childrow.Cells[HEADCOL_EntityID].Value.ToString()))
                                        {
                                            if (_taxLotWithStateDict.ContainsKey(dtrow[HEADCOL_EntityID].ToString()))
                                            {
                                                if (_taxLotWithStateDict[dtrow[HEADCOL_EntityID].ToString()].Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore))
                                                {
                                                    dtrow["TaxLotState"] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated.ToString();
                                                    _blnchange = true;
                                                    SetColourToRow(grdrow);
                                                }
                                                else
                                                {
                                                    dtrow["TaxLotState"] = _taxLotWithStateDict[dtrow[HEADCOL_EntityID].ToString()];
                                                    _blnchange = true;
                                                    SetColourToRow(grdrow);
                                                }
                                            }
                                            else
                                            {
                                                dtrow["TaxLotState"] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated.ToString();
                                                _blnchange = true;
                                                SetColourToRow(grdrow);
                                            }

                                            if (_taxLotIgnoreStateDict.ContainsKey(dtrow[HEADCOL_EntityID].ToString()))
                                            {
                                                _taxLotIgnoreStateDict[dtrow[HEADCOL_EntityID].ToString()] = Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                SetDefaultFilters();
                toolStripStatusLabel1.Text = strMessage;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetDefaultFilters()
        {
            try
            {
                if (ultraTabControl1.Tabs["activetaxlots"].Selected.Equals(true))
                {
                    if (grdThirdParty.Rows.Count > 0)
                    {
                        UltraGridBand band = grdThirdParty.DisplayLayout.Bands[0];
                        band.ColumnFilters.ClearAllFilters();
                        if (band.Columns.Exists(HEADCOL_TAXLOTSTATE))
                        {
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].LogicalOperator = FilterLogicalOperator.Or;
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.Equals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated);
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.Equals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Amended);
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.Equals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Deleted);
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.Equals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Sent);
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.NotEquals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore);

                            BackToolStripMenuItem.Visible = false;
                            IgnoreToolStripMenuItem.Visible = true;
                        }
                    }
                    else
                    {
                        BackToolStripMenuItem.Visible = false;
                        IgnoreToolStripMenuItem.Visible = false;
                    }
                }
                else if (ultraTabControl1.Tabs["ignoredtaxlots"].Selected.Equals(true))
                {
                    if (grdThirdParty.Rows.Count > 0)
                    {
                        UltraGridBand band = grdThirdParty.DisplayLayout.Bands[0];
                        if (band.Columns.Exists(HEADCOL_TAXLOTSTATE))
                        {
                            band.ColumnFilters.ClearAllFilters();
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].LogicalOperator = FilterLogicalOperator.Or;
                            band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.Equals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore);

                            BackToolStripMenuItem.Visible = true;
                            IgnoreToolStripMenuItem.Visible = false;
                        }
                    }
                    else
                    {
                        BackToolStripMenuItem.Visible = false;
                        IgnoreToolStripMenuItem.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                SetDefaultFilters();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #region IComparer Members
        /// <summary>
        /// this method is implemeted from icomparer interface in order to custom sort for taxlots. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            UltraGridGroupByRow xObj = (UltraGridGroupByRow)x;
            UltraGridGroupByRow yObj = (UltraGridGroupByRow)y;
            try
            {
                if (_thirdPartyTaxlotStateSortingOrder.Count > 0)
                {
                    if (xObj.Rows.Count > 0 && yObj.Rows.Count > 0)
                    {
                        string state1 = xObj.Rows[0].Cells["TaxLotState"].Text.ToUpper();
                        string state2 = yObj.Rows[0].Cells["TaxLotState"].Text.ToUpper();

                        int i = _thirdPartyTaxlotStateSortingOrder.IndexOf(state1);
                        int j = _thirdPartyTaxlotStateSortingOrder.IndexOf(state2);
                        if (i >= 0 && j >= 0)
                        {
                            if (i > j)
                                return 1;
                            else if (i < j)
                                return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            // Compare the group rows by the number of items they contain.
            return 0;
        }
        #endregion
        private void chkLstAuec_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (toolTipIndex != this.chkLstAuec.IndexFromPoint(e.Location))
                {
                    toolTipIndex = chkLstAuec.IndexFromPoint(chkLstAuec.PointToClient(MousePosition));
                    if (toolTipIndex > -1)
                    {
                        AUEC companyWorkingAUEC = (AUEC)chkLstAuec.Items[toolTipIndex];
                        string displayNameofAUEC = companyWorkingAUEC.DisplayName;
                        toolTip.SetToolTip(chkLstAuec, displayNameofAUEC);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkIncluedSent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdThirdParty.DataSource != null)
                {
                    DialogResult diares;
                    diares = MessageBox.Show("Changing of include sent check box may change the Data, Do you want to continue..", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (diares == DialogResult.No)
                        return;
                    else
                        SetDefaultValues();
                }
                if (chkIncluedSent.Checked)
                    _chkIncluedSent = true;
                else
                    _chkIncluedSent = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}

