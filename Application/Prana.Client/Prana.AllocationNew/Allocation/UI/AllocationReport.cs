using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.CommonDataCache;
using Infragistics.Documents.Excel;
using Infragistics.Win.UltraWinGrid.ExcelExport;
//using Prana.PostTrade;
using Prana.BusinessObjects;
using Prana.Utilities.XMLUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UIUtilities;
using System.Xml.Serialization;
using System.IO;
using Prana.BusinessObjects.AppConstants;
using Infragistics.Win.UltraWinForm;
using Prana.Utilities;

namespace Prana.AllocationNew
{
    /// <summary>
    /// Summary description for AllocationReport.
    /// </summary>
    public class AllocationReport : System.Windows.Forms.Form
    {
        #region Private Variables
        private Color rowForeColor = Color.FromArgb(192, 192, 255);
        CompanyUser _loginUser = null;
        #endregion

        #region Windows Variables

        private UltraGridExcelExporter ExcelExporter;
        private Infragistics.Win.Misc.UltraButton btn_GenerateReport;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Infragistics.Win.Misc.UltraPanel AllocationReport_Fill_Panel;
        private System.Windows.Forms.ImageList imlBlotterDetails;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker colPkrRowColor;
        private Infragistics.Win.Misc.UltraLabel label9;
        private System.ComponentModel.IContainer components;
        #endregion
        private UltraGrid grdAllocationReport;
        private ContextMenu contextMenu1;
        private MenuItem menuItem1;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;
        private Infragistics.Win.Misc.UltraPanel AllocationReport_Fill_Panel_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationReport_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationReport_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationReport_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationReport_UltraFormManager_Dock_Area_Bottom;
        private static AllocationReport _allocationReport = null;
        private AllocationReport()
        {
            //
            // Required for Windows Form Designer support
            //
            try
            {
                InitializeComponent();
                InitGrid();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }




            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static AllocationReport GetInstance
        {
            get
            {
                if (_allocationReport == null)
                {
                    _allocationReport = new AllocationReport();
                }
                return _allocationReport;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _allocationReport = null;
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public Prana.BusinessObjects.CompanyUser loginUser
        {
            get
            {
                return _loginUser;
            }
            set
            {
                _loginUser = value;
            }
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportToExcel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SnapShotManager.GetInstance().buttonTool;");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportToExcel");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SnapShotManager.GetInstance().buttonTool;");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationReport));
            this.btn_GenerateReport = new Infragistics.Win.Misc.UltraButton();
            this.ExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.AllocationReport_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.AllocationReport_Fill_Panel_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.grdAllocationReport = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.label9 = new Infragistics.Win.Misc.UltraLabel();
            this.colPkrRowColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.imlBlotterDetails = new System.Windows.Forms.ImageList(this.components);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._AllocationReport_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationReport_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationReport_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            this.AllocationReport_Fill_Panel.ClientArea.SuspendLayout();
            this.AllocationReport_Fill_Panel.SuspendLayout();
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.SuspendLayout();
            this.AllocationReport_Fill_Panel_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocationReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colPkrRowColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_GenerateReport
            // 
            this.btn_GenerateReport.Location = new System.Drawing.Point(273, 8);
            this.btn_GenerateReport.Name = "btn_GenerateReport";
            this.btn_GenerateReport.Size = new System.Drawing.Size(99, 22);
            this.btn_GenerateReport.TabIndex = 31;
            this.btn_GenerateReport.Text = "Generate";
            this.btn_GenerateReport.Click += new System.EventHandler(this.btn_GenerateReport_Click);
            // 
            // dtFromDate
            // 
            this.dtFromDate.Location = new System.Drawing.Point(53, 9);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(86, 21);
            this.dtFromDate.TabIndex = 33;
            this.dtFromDate.ValueChanged += new System.EventHandler(this.dtPicker_ValueChanged);
            // 
            // AllocationReport_Fill_Panel
            // 
            // 
            // AllocationReport_Fill_Panel.ClientArea
            // 
            this.AllocationReport_Fill_Panel.ClientArea.Controls.Add(this.AllocationReport_Fill_Panel_Fill_Panel);
            this.AllocationReport_Fill_Panel.ClientArea.Controls.Add(this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left);
            this.AllocationReport_Fill_Panel.ClientArea.Controls.Add(this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right);
            this.AllocationReport_Fill_Panel.ClientArea.Controls.Add(this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom);
            this.AllocationReport_Fill_Panel.ClientArea.Controls.Add(this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top);
            this.AllocationReport_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AllocationReport_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllocationReport_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.AllocationReport_Fill_Panel.Name = "AllocationReport_Fill_Panel";
            this.AllocationReport_Fill_Panel.Size = new System.Drawing.Size(862, 338);
            this.AllocationReport_Fill_Panel.TabIndex = 0;
            // 
            // AllocationReport_Fill_Panel_Fill_Panel
            // 
            // 
            // AllocationReport_Fill_Panel_Fill_Panel.ClientArea
            // 
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.lblTo);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.lblFrom);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.dtToDate);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.grdAllocationReport);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.label9);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.colPkrRowColor);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.dtFromDate);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.Controls.Add(this.btn_GenerateReport);
            this.AllocationReport_Fill_Panel_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AllocationReport_Fill_Panel_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllocationReport_Fill_Panel_Fill_Panel.Location = new System.Drawing.Point(0, 27);
            this.AllocationReport_Fill_Panel_Fill_Panel.Name = "AllocationReport_Fill_Panel_Fill_Panel";
            this.AllocationReport_Fill_Panel_Fill_Panel.Size = new System.Drawing.Size(862, 311);
            this.AllocationReport_Fill_Panel_Fill_Panel.TabIndex = 0;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(145, 12);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(17, 14);
            this.lblTo.TabIndex = 41;
            this.lblTo.Text = "To";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(12, 12);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(31, 14);
            this.lblFrom.TabIndex = 40;
            this.lblFrom.Text = "From";
            // 
            // dtToDate
            // 
            this.dtToDate.Location = new System.Drawing.Point(171, 9);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(86, 21);
            this.dtToDate.TabIndex = 39;
            // 
            // grdAllocationReport
            // 
            this.grdAllocationReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAllocationReport.ContextMenu = this.contextMenu1;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdAllocationReport.DisplayLayout.Appearance = appearance1;
            this.grdAllocationReport.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grdAllocationReport.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance2.BackColor = System.Drawing.Color.White;
            this.grdAllocationReport.DisplayLayout.CaptionAppearance = appearance2;
            this.grdAllocationReport.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAllocationReport.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAllocationReport.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdAllocationReport.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdAllocationReport.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdAllocationReport.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdAllocationReport.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAllocationReport.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8.25F;
            this.grdAllocationReport.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdAllocationReport.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAllocationReport.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdAllocationReport.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAllocationReport.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAllocationReport.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAllocationReport.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAllocationReport.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdAllocationReport.Location = new System.Drawing.Point(1, 35);
            this.grdAllocationReport.Name = "grdAllocationReport";
            this.grdAllocationReport.Size = new System.Drawing.Size(859, 274);
            this.grdAllocationReport.TabIndex = 38;
            this.grdAllocationReport.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.grdAllocationReport.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAllocationReport_BeforeCustomRowFilterDialog);
            this.grdAllocationReport.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAllocationReport_BeforeColumnChooserDisplayed);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Save Layout";
            this.menuItem1.Click += new System.EventHandler(this.menuSaveLayout_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.Transparent;
            this.label9.Appearance = appearance4;
            this.label9.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(589, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 19);
            this.label9.TabIndex = 37;
            this.label9.Text = "Select Row Color";
            // 
            // colPkrRowColor
            // 
            this.colPkrRowColor.AllowEmpty = false;
            this.colPkrRowColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.BorderColor = System.Drawing.Color.Black;
            appearance5.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.colPkrRowColor.Appearance = appearance5;
            this.colPkrRowColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            appearance6.BorderColor = System.Drawing.Color.Black;
            this.colPkrRowColor.ButtonAppearance = appearance6;
            this.colPkrRowColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.colPkrRowColor.Color = System.Drawing.SystemColors.ActiveCaption;
            this.colPkrRowColor.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.colPkrRowColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.colPkrRowColor.Location = new System.Drawing.Point(698, 9);
            this.colPkrRowColor.Name = "colPkrRowColor";
            this.colPkrRowColor.Size = new System.Drawing.Size(134, 20);
            this.colPkrRowColor.TabIndex = 36;
            this.colPkrRowColor.Text = "ActiveCaption";
            this.colPkrRowColor.ColorChanged += new System.EventHandler(this.colPkrRowColor_ColorChanged);
            // 
            // _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left
            // 
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.Name = "_AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left";
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 311);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.AllocationReport_Fill_Panel;
            this.ultraToolbarsManager1.ImageListSmall = this.imlBlotterDetails;
            this.ultraToolbarsManager1.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool3});
            ultraToolbar1.Text = "UltraToolbar1";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance7.Image = 3;
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance7;
            buttonTool2.SharedPropsInternal.Caption = "ExportToExcel";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            buttonTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance8;
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            buttonTool4});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // imlBlotterDetails
            // 
            this.imlBlotterDetails.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlBlotterDetails.ImageStream")));
            this.imlBlotterDetails.TransparentColor = System.Drawing.Color.Transparent;
            this.imlBlotterDetails.Images.SetKeyName(0, "");
            this.imlBlotterDetails.Images.SetKeyName(1, "");
            this.imlBlotterDetails.Images.SetKeyName(2, "");
            this.imlBlotterDetails.Images.SetKeyName(3, "");
            this.imlBlotterDetails.Images.SetKeyName(4, "");
            this.imlBlotterDetails.Images.SetKeyName(5, "");
            this.imlBlotterDetails.Images.SetKeyName(6, "");
            this.imlBlotterDetails.Images.SetKeyName(7, "");
            // 
            // _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right
            // 
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(862, 27);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.Name = "_AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right";
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 311);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom
            // 
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 338);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.Name = "_AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom";
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(862, 0);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top
            // 
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.Name = "_AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top";
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(862, 27);
            this._AllocationReport_Fill_Panel_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _AllocationReport_UltraFormManager_Dock_Area_Left
            // 
            this._AllocationReport_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationReport_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AllocationReport_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AllocationReport_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._AllocationReport_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._AllocationReport_UltraFormManager_Dock_Area_Left.Name = "_AllocationReport_UltraFormManager_Dock_Area_Left";
            this._AllocationReport_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 338);
            // 
            // _AllocationReport_UltraFormManager_Dock_Area_Right
            // 
            this._AllocationReport_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationReport_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AllocationReport_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AllocationReport_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._AllocationReport_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(866, 27);
            this._AllocationReport_UltraFormManager_Dock_Area_Right.Name = "_AllocationReport_UltraFormManager_Dock_Area_Right";
            this._AllocationReport_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 338);
            // 
            // _AllocationReport_UltraFormManager_Dock_Area_Top
            // 
            this._AllocationReport_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationReport_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AllocationReport_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AllocationReport_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AllocationReport_UltraFormManager_Dock_Area_Top.Name = "_AllocationReport_UltraFormManager_Dock_Area_Top";
            this._AllocationReport_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(870, 27);
            // 
            // _AllocationReport_UltraFormManager_Dock_Area_Bottom
            // 
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 365);
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.Name = "_AllocationReport_UltraFormManager_Dock_Area_Bottom";
            this._AllocationReport_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(870, 4);
            // 
            // AllocationReport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(870, 369);
            this.Controls.Add(this.AllocationReport_Fill_Panel);
            this.Controls.Add(this._AllocationReport_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AllocationReport_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AllocationReport_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AllocationReport_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 200);
            this.Name = "AllocationReport";
            this.Text = "AllocationReport";
            this.Load += new System.EventHandler(this.AllocationReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            this.AllocationReport_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AllocationReport_Fill_Panel.ResumeLayout(false);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AllocationReport_Fill_Panel_Fill_Panel.ClientArea.PerformLayout();
            this.AllocationReport_Fill_Panel_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocationReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colPkrRowColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void InitGrid()
        {
            try
            {
                // AddColumns();
                //grdAllocationReport.DataSource = null;
                //grdAllocationReport.DataSource = _allocatedGroups;
                //grdAllocationReport.DataBind();
                //HideColumns();
                //SetColumnDataFormat(grdAllocationReport, "AvgPrice", "F2");
                //colPkrRowColor.Color = rowForeColor;
                //SetGridColors();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void SetGridColors()
        {
            grdAllocationReport.DisplayLayout.Override.RowAppearance.ForeColor = rowForeColor;
            grdAllocationReport.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
        }
        private void SetColumnDataFormat(UltraGrid grid, string columnName, string format)
        {

            if (grid.DisplayLayout.Bands[0].Columns.Exists(columnName))
                grid.DisplayLayout.Bands[0].Columns["AvgPrice"].Format = format;


        }
        private void btn_GenerateReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                DateTime FromDate = dtFromDate.DateTime;
                DateTime ToDate = dtToDate.DateTime;
                //DateTime date = new DateTime(dtPicker.DateTime.Year, dtPicker.DateTime.Month, dtPicker.DateTime.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                //date = Prana.BusinessObjects.TimeZoneInfo.ConvertTimeZoneToUtc(date, Prana.BusinessObjects.TimeZoneInfo.CurrentTimeZone);
                //AllocatedGroups filteredAllocatedgroup= new AllocatedGroups();

                string AllFromAUECDatesString = string.Empty;
                string AllToAUECDatesString = string.Empty;
                if (FromDate.Date.Equals(ToDate.Date))
                {
                    AllFromAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(FromDate);
                    AllToAUECDatesString = AllFromAUECDatesString;
                }
                else
                {
                    AllFromAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(FromDate);
                    AllToAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(ToDate);
                }


                grdAllocationReport.DataSource = PostTradeDataManager.GetTaxLots(AllFromAUECDatesString, AllToAUECDatesString);
                grdAllocationReport.DataBind();
                //if (filteredAllocatedgroup.Count == 0)
                //     AddColumns();
                // grdAllocationReport.DataBind();

                LoadPreferences(_loginUser);            
                // FilterAccountStrategyColumns();
                //   SetColumnDataFormat(grdAllocationReport, "AvgPrice", "F2");
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }


            }

        }

        private void cmbbxSelect_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {

                //AddColumns();				
                ////AllocatedGroups allocatedGroups=new AllocatedGroups();
                //grdAllocationReport.DataSource = null;
                ////grdAllocationReport.DataSource=allocatedGroups;
                //grdAllocationReport.DataBind();
                //HideColumns();
                //FilterAccountStrategyColumns();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }


            }
        }

        private void SetGridColumns(bool isPrefSaved)
        {
            try
            {
                ColumnsCollection columns = grdAllocationReport.DisplayLayout.Bands[0].Columns;

                List<string> ColumnNames = AllocationConstants.AllocatedReportColumns;


                if (isPrefSaved)
                {
                    foreach (string caption in ColumnNames)
                    {
                        string columnName = caption;

                        if (OrderFields.ColumnNameHeaderCollection.ContainsKey(caption))
                        {
                            columnName = OrderFields.ColumnNameHeaderCollection[caption];
                        }

                        if (columns.Exists(columnName))
                        {
                            columns[columnName].Header.Caption = caption;

                            if (caption.Equals(OrderFields.CAPTION_AUECLOCALDATE))
                            {

                                columns[columnName].Header.Caption = "Allocation Date";

                                columns[columnName].CellActivation = Activation.NoEdit;
                            }
                            if (caption.Equals(OrderFields.CAPTION_EXECUTED_QTY))
                            {
                                // Kuldeep A.: http://jira.nirvanasolutions.com:8080/browse/PRANA-7020
                                // The grid is being binded from taxlotCollection which has 2 fields for CumQty (1 from own and 1 from Base),so I am excluding one of the duplicate field here from showing on to Grid.
                                columns[columnName].Hidden = true;
                                columns[columnName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                            }
                        }
                    }
                }
                else
                {
                    foreach (UltraGridColumn column in columns)
                    {
                        column.Hidden = true;
                    }

                    foreach (string caption in ColumnNames)
                    {
                        string columnName = caption;

                        if (OrderFields.ColumnNameHeaderCollection.ContainsKey(caption))
                        {
                            columnName = OrderFields.ColumnNameHeaderCollection[caption];
                        }

                        if (columns.Exists(columnName))
                        {
                            columns[columnName].Header.Caption = caption;
                            columns[columnName].Hidden = false;

                            if (caption.Equals(OrderFields.CAPTION_AUECLOCALDATE))
                            {
                                columns[columnName].Hidden = false;
                                columns[columnName].Header.Caption = "Allocation Date";

                                columns[columnName].CellActivation = Activation.NoEdit;
                            }
                            if (caption.Equals(OrderFields.CAPTION_EXECUTED_QTY))
                            {
                                columns[columnName].Hidden = true;
                                columns[columnName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;                                
                            }
                            if (caption.Equals(OrderFields.CAPTION_Level2Name))
                            {
                                columns[columnName].Hidden = true;
                            }
                        }
                        else
                        {

                        }
                    }
                    int visiblePosition = 1;
                    columns[OrderFields.PROPERTY_Level1Name].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_ASSET_NAME].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_SYMBOL].Header.VisiblePosition = visiblePosition++;
                    columns["CompanyName"].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_ORDER_SIDE].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_AUECLOCALDATE].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_AVGPRICE].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_VENUE].Header.VisiblePosition = visiblePosition++;
                    columns[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Header.VisiblePosition = visiblePosition++;

                }
                UltraGridColumn colSettlementCurrency = grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID];
                colSettlementCurrency.Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCY;

                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                ValueList currencies = new ValueList();
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    currencies.ValueListItems.Add(item.Key, item.Value);
                }
                colSettlementCurrency.ValueList = currencies;
                colSettlementCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

                UltraGridColumn colSettlementCurrencyAmount = grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT];
                colSettlementCurrencyAmount.Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCYAMOUNT;

                UltraGridColumn colSettlementCurrentFxRate = grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SettCurrFXRate];
                colSettlementCurrentFxRate.Header.Caption = OrderFields.CAPTION_SettCurrFXRate;

                UltraGridColumn colSettlementCurrentFxRateCalc = grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SettCurrFXRateCalc];
                colSettlementCurrentFxRateCalc.Header.Caption = OrderFields.CAPTION_SettCurrFXRateCalc;

                foreach (UltraGridColumn column in grdAllocationReport.DisplayLayout.Bands[0].Columns)
                {
                    column.CellActivation = Activation.NoEdit;
                }
                UltraGridColumn colSettlementUnitCost = grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTUNITCOST];
                colSettlementUnitCost.Hidden = true;
                colSettlementUnitCost.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                //Added to hide Settled quantity, PRANA-10815
                columns["SettledQty"].Hidden = true;
                columns["SettledQty"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        AllocationReportPreferences _allocationReportPref = null;


        string _allocationReportPrefFilePath = string.Empty;
        string _allocationReportPrefDirectoryPath = string.Empty;
        static CustomXmlSerializer _Xml = new CustomXmlSerializer();

        private void SavePreferences()
        {
            _Xml.WriteFile(_allocationReportPref, _allocationReportPrefFilePath, true);
        }

        static string _startPath = string.Empty;

        internal void LoadPreferences(CompanyUser user)
        {
            _startPath = System.Windows.Forms.Application.StartupPath;
            _allocationReportPrefDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + user.CompanyUserID.ToString();
            _allocationReportPrefFilePath = _allocationReportPrefDirectoryPath + @"\AllocationReportPreferences.xml";
            _allocationReportPref = new AllocationReportPreferences();

            try
            {
                if (!Directory.Exists(_allocationReportPrefDirectoryPath))
                {
                    Directory.CreateDirectory(_allocationReportPrefDirectoryPath);
                }

                if (File.Exists(_allocationReportPrefFilePath))
                {
                    _allocationReportPref = (AllocationReportPreferences)_Xml.ReadXml(_allocationReportPrefFilePath, _allocationReportPref);
                }

                List<string> AllocationReportColumns = GeneralUtilities.GetListFromString(_allocationReportPref.GrdAllocationReportColumns, ',');

                bool isPrefSaved = false;
                if (AllocationReportColumns.Count != 0)
                {
                    UltraWinGridUtils.SetColumns(AllocationReportColumns, grdAllocationReport);

                    isPrefSaved = true;

                }

                SetGridColumns(isPrefSaved);

            }

            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }



        private void ExportToExcel()
        {
            try
            {
                int _count = 0;
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                //				workBook = null;
                string pathName = null;
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return;

                }

                workBook = OnExportToExcel(_count, workBook, pathName);
                _count++;

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
                try
                {
                    // Infragistics.Documents.Excel.BIFF8Writer.WriteWorkbookToFile(workBook,pathName);
                    workBook.Save(pathName);
                }
                catch (Exception)
                {
                    MessageBox.Show("File is Open, Please Close the File then Save it.");
                }


            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }
        }

        public Workbook OnExportToExcel(int key, Workbook workBook, string fileName)
        {
            try
            {
                if (workBook == null)
                {
                    workBook = this.ExcelExporter.Export(grdAllocationReport, fileName);
                }

                workBook.Worksheets.Add(this.grdAllocationReport.Name + key);
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[grdAllocationReport.Name + key];
                ExcelExporter.Export(this.grdAllocationReport, workBook);
                return workBook;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }
            return null;
        }







        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key.ToLower())
                {
                    case "exporttoexcel":
                        ExportToExcel();
                        break;
                    case "screenshot":
                        SnapShotManager.GetInstance().TakeSnapshot(this);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void dtPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = dtFromDate.DateTime.Date;
            DateTime currentDate = DateTime.Now.Date;
            //if (date.CompareTo(currentDate) > 0)
            //{
            //  // MessageBox.Show("Invalid Date ! Selected Date should not be Greater than Current date.");
            //    dtPicker.Value = DateTime.Now.Date;

            //}

        }

        private void colPkrRowColor_ColorChanged(object sender, EventArgs e)
        {
            rowForeColor = colPkrRowColor.Color;
            SetGridColors();

        }

        private void grdAllocationReport_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                //If Data Source of grdAllocationReport is not null then column chooser will open, Otherwise it will be disable
                if (grdAllocationReport.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.grdAllocationReport);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void menuSaveLayout_Click(object sender, EventArgs e)
        {

            _allocationReportPref.GrdAllocationReportColumns = UltraWinGridUtils.GetColumnsString(grdAllocationReport);

            SavePreferences();
        }

        private void AllocationReport_Load(object sender, EventArgs e)
        {
            try
            {
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                {
                    CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                }
                else
                {
                    SetButtonsColor();
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_MAIN);
                        this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                        colPkrRowColor.Visible = false;
                        label9.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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
                btn_GenerateReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btn_GenerateReport.ForeColor = System.Drawing.Color.White;
                btn_GenerateReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn_GenerateReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btn_GenerateReport.UseAppStyling = false;
                btn_GenerateReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAllocationReport_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
    }

    }

    [XmlRoot("AllocationReportPreferences")]

    public class AllocationReportPreferences
    {
        private string _grdAllocationReportColumns = string.Empty;

        public string GrdAllocationReportColumns
        {
            get { return _grdAllocationReportColumns; }
            set { _grdAllocationReportColumns = value; }
        }
    }
}
