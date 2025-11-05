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
using System.Collections.Generic ;

using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Allocation.BLL;
using Prana.CommonDataCache;
namespace Prana.Allocation
{
	/// <summary>
	/// Summary description for AllocationReport.
	/// </summary>
	public class AllocationReport : System.Windows.Forms.Form
    {
        #region Private Variables
        AllocatedGroups _allocatedGroups;
        //private string FORM_NAME = "AllocationReport";
        private Color rowForeColor = Color.FromArgb(192, 192, 255);
        #endregion

        #region Windows Variables

        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ExcelExporter;
        private System.Windows.Forms.Button btn_GenerateReport;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPicker;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbbxSelect;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel AllocationReport_Fill_Panel;
        private System.Windows.Forms.ImageList imlBlotterDetails;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationReport_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker colPkrRowColor;
        private Label label9;
        private System.ComponentModel.IContainer components;
        #endregion	
        private UltraGrid grdAllocationReport;
        private static AllocationReport _allocationReport = null;
		private  AllocationReport()
		{
			//
			// Required for Windows Form Designer support
			//
			try
			{
				InitializeComponent();
				InitGrid();
				BindComboBox();
			}
			catch(Exception ex)
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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                _allocationReport = null;
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationReport));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportToExcel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportToExcel");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.btn_GenerateReport = new System.Windows.Forms.Button();
            this.ExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.dtPicker = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.cmbbxSelect = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.AllocationReport_Fill_Panel = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.colPkrRowColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.imlBlotterDetails = new System.Windows.Forms.ImageList(this.components);
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._AllocationReport_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AllocationReport_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AllocationReport_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AllocationReport_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.grdAllocationReport = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.dtPicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSelect)).BeginInit();
            this.AllocationReport_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colPkrRowColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocationReport)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_GenerateReport
            // 
            this.btn_GenerateReport.Image = ((System.Drawing.Image)(resources.GetObject("btn_GenerateReport.Image")));
            this.btn_GenerateReport.Location = new System.Drawing.Point(150, 8);
            this.btn_GenerateReport.Name = "btn_GenerateReport";
            this.btn_GenerateReport.Size = new System.Drawing.Size(70, 22);
            this.btn_GenerateReport.TabIndex = 31;
            this.btn_GenerateReport.Click += new System.EventHandler(this.btn_GenerateReport_Click);
            // 
            // dtPicker
            // 
            this.dtPicker.Location = new System.Drawing.Point(44, 8);
            this.dtPicker.Name = "dtPicker";
            this.dtPicker.Size = new System.Drawing.Size(86, 21);
            this.dtPicker.TabIndex = 33;
            this.dtPicker.ValueChanged += new System.EventHandler(this.dtPicker_ValueChanged);
            // 
            // cmbbxSelect
            // 
            this.cmbbxSelect.AutoComplete = true;
            this.cmbbxSelect.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbbxSelect.ButtonAppearance = appearance1;
            this.cmbbxSelect.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.cmbbxSelect.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbbxSelect.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxSelect.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbbxSelect.Location = new System.Drawing.Point(244, 8);
            this.cmbbxSelect.MaxMRUItems = 2;
            this.cmbbxSelect.Name = "cmbbxSelect";
            this.cmbbxSelect.Size = new System.Drawing.Size(66, 20);
            this.cmbbxSelect.TabIndex = 34;
            this.cmbbxSelect.ValueChanged += new System.EventHandler(this.cmbbxSelect_ValueChanged);
            // 
            // AllocationReport_Fill_Panel
            // 
            this.AllocationReport_Fill_Panel.Controls.Add(this.grdAllocationReport);
            this.AllocationReport_Fill_Panel.Controls.Add(this.label9);
            this.AllocationReport_Fill_Panel.Controls.Add(this.colPkrRowColor);
            this.AllocationReport_Fill_Panel.Controls.Add(this.cmbbxSelect);
            this.AllocationReport_Fill_Panel.Controls.Add(this.dtPicker);
            this.AllocationReport_Fill_Panel.Controls.Add(this.btn_GenerateReport);
            this.AllocationReport_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AllocationReport_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllocationReport_Fill_Panel.Location = new System.Drawing.Point(0, 24);
            this.AllocationReport_Fill_Panel.Name = "AllocationReport_Fill_Panel";
            this.AllocationReport_Fill_Panel.Size = new System.Drawing.Size(870, 345);
            this.AllocationReport_Fill_Panel.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(643, 11);
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
            this.colPkrRowColor.Location = new System.Drawing.Point(752, 9);
            this.colPkrRowColor.Name = "colPkrRowColor";
            this.colPkrRowColor.Size = new System.Drawing.Size(88, 20);
            this.colPkrRowColor.TabIndex = 36;
            this.colPkrRowColor.Text = "ActiveCaption";
            this.colPkrRowColor.ColorChanged += new System.EventHandler(this.colPkrRowColor_ColorChanged);
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
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.ImageListSmall = this.imlBlotterDetails;
            this.ultraToolbarsManager1.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.Text = "UltraToolbar1";
            ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1});
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance7.Image = 3;
            buttonTool2.SharedProps.AppearancesSmall.Appearance = appearance7;
            buttonTool2.SharedProps.Caption = "ExportToExcel";
            buttonTool2.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _AllocationReport_Toolbars_Dock_Area_Left
            // 
            this._AllocationReport_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._AllocationReport_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 24);
            this._AllocationReport_Toolbars_Dock_Area_Left.Name = "_AllocationReport_Toolbars_Dock_Area_Left";
            this._AllocationReport_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 345);
            this._AllocationReport_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _AllocationReport_Toolbars_Dock_Area_Right
            // 
            this._AllocationReport_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._AllocationReport_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(870, 24);
            this._AllocationReport_Toolbars_Dock_Area_Right.Name = "_AllocationReport_Toolbars_Dock_Area_Right";
            this._AllocationReport_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 345);
            this._AllocationReport_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _AllocationReport_Toolbars_Dock_Area_Top
            // 
            this._AllocationReport_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._AllocationReport_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AllocationReport_Toolbars_Dock_Area_Top.Name = "_AllocationReport_Toolbars_Dock_Area_Top";
            this._AllocationReport_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(870, 24);
            this._AllocationReport_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _AllocationReport_Toolbars_Dock_Area_Bottom
            // 
            this._AllocationReport_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationReport_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationReport_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._AllocationReport_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationReport_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 369);
            this._AllocationReport_Toolbars_Dock_Area_Bottom.Name = "_AllocationReport_Toolbars_Dock_Area_Bottom";
            this._AllocationReport_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(870, 0);
            this._AllocationReport_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // grdAllocationReport
            // 
            this.grdAllocationReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.Black;
            appearance2.BackColor2 = System.Drawing.Color.Black;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Tahoma";
            appearance2.FontData.SizeInPoints = 8.25F;
            this.grdAllocationReport.DisplayLayout.Appearance = appearance2;
            this.grdAllocationReport.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdAllocationReport.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance3.BackColor = System.Drawing.Color.White;
            this.grdAllocationReport.DisplayLayout.CaptionAppearance = appearance3;
            this.grdAllocationReport.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAllocationReport.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAllocationReport.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdAllocationReport.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdAllocationReport.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdAllocationReport.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdAllocationReport.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAllocationReport.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Tahoma";
            appearance4.FontData.SizeInPoints = 8.25F;
            this.grdAllocationReport.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdAllocationReport.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAllocationReport.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAllocationReport.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAllocationReport.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAllocationReport.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAllocationReport.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAllocationReport.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocationReport.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdAllocationReport.Location = new System.Drawing.Point(1, 35);
            this.grdAllocationReport.Name = "grdAllocationReport";
            this.grdAllocationReport.Size = new System.Drawing.Size(867, 308);
            this.grdAllocationReport.TabIndex = 38;
            this.grdAllocationReport.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            // 
            // AllocationReport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(870, 369);
            this.Controls.Add(this.AllocationReport_Fill_Panel);
            this.Controls.Add(this._AllocationReport_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._AllocationReport_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._AllocationReport_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._AllocationReport_Toolbars_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 200);
            this.Name = "AllocationReport";
            this.Text = "AllocationReport";
            ((System.ComponentModel.ISupportInitialize)(this.dtPicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSelect)).EndInit();
            this.AllocationReport_Fill_Panel.ResumeLayout(false);
            this.AllocationReport_Fill_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colPkrRowColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocationReport)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private void InitGrid()
		{
            try
            {
                 AddColumns();
                _allocatedGroups = new AllocatedGroups();
                grdAllocationReport.DataSource = null;
                grdAllocationReport.DataSource = _allocatedGroups;
                grdAllocationReport.DataBind();
                HideColumns();
                SetColumnDataFormat(grdAllocationReport, "AvgPrice", "F2");
                colPkrRowColor.Color = rowForeColor;
                SetGridColors();
            }
            catch (Exception ex)
            {
                throw ex;
            }
		
		}
        private void SetGridColors()
        {
            grdAllocationReport.DisplayLayout.Override.RowAppearance.ForeColor = rowForeColor;
            grdAllocationReport.DisplayLayout.Override.RowAppearance.BackColor  = Color.Black;
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
                DateTime date = dtPicker.DateTime;
                //DateTime date = new DateTime(dtPicker.DateTime.Year, dtPicker.DateTime.Month, dtPicker.DateTime.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                //date = Prana.BusinessObjects.TimeZoneInfo.ConvertTimeZoneToUtc(date, Prana.BusinessObjects.TimeZoneInfo.CurrentTimeZone);
				AllocatedGroups filteredAllocatedgroup= new AllocatedGroups();


                string AllAUECDatesString = string.Empty;
                if (date.Date.Equals(DateTime.Now.Date))
                {
                    AllAUECDatesString = TimeZoneHelper.GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);
                }
                else
                {
                    AllAUECDatesString = TimeZoneHelper.GetSameDateForAllAUEC(date);
                }

				if(cmbbxSelect.SelectedIndex==0)
				{
                    filteredAllocatedgroup = OrderAllocationDBManager.GetAllocatedFundsFromDB(AllAUECDatesString);
				}
				else if(cmbbxSelect.SelectedIndex==1)
				{
                    filteredAllocatedgroup = OrderAllocationDBManager.GetAllocatedStrategiesFromDb(AllAUECDatesString);
				}
				else 
				{
				}
                grdAllocationReport.DataSource = null;
                grdAllocationReport.DataSource = filteredAllocatedgroup;
               if (filteredAllocatedgroup.Count == 0)
                    AddColumns();
                grdAllocationReport.DataBind();
                HideColumns();
                FilterFundStrategyColumns();
                SetColumnDataFormat(grdAllocationReport, "AvgPrice", "F2");
			}
			catch(Exception ex)
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

                AddColumns();				
				AllocatedGroups allocatedGroups=new AllocatedGroups();
                grdAllocationReport.DataSource = null;
				grdAllocationReport.DataSource=allocatedGroups;
                grdAllocationReport.DataBind();
				HideColumns();
				FilterFundStrategyColumns();
			}
			catch(Exception ex)
			{

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				
				
			}		
		}
		private void HideColumns()
		{
            try
            {
                ColumnsCollection columns = grdAllocationReport.DisplayLayout.Bands[0].Columns;
                List<string> ColumnNames = OrderFields.AllocationReportColumns;
                foreach (UltraGridColumn column in columns)
                {
                    column.Hidden = true;
                }

                foreach (string caption in ColumnNames)
                {
                    string columnName = OrderFields.ColumnNameHeaderCollection[caption];
                    columns[columnName].Hidden = false;
                    columns[columnName].Header.Caption =caption;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
		private void AddColumns()
		{
			try
			{
                UltraGridBand allocatedGridBand = new UltraGridBand("", -1);
                //int count=newPreferences.UnAllocatedGridColumns.DisplayColumns.Length;
                string[] ColumnNames = Enum.GetNames(typeof(OrderFields.AllocationReportColumnsEnum));
                UltraGridColumn[] gridColumnList = new UltraGridColumn[ColumnNames.Length];
                int i = 0;

                foreach (string columnName in ColumnNames)
                {

                    gridColumnList[i] = new UltraGridColumn(columnName, i);
                    gridColumnList[i].Header.VisiblePosition = i;
                    i++;

                }
                //grdAllocationReport.DisplayLayout.Bands[0].Columns.AddRange(gridColumnList);

                allocatedGridBand.Columns.AddRange(gridColumnList);
                allocatedGridBand.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
                
               grdAllocationReport.DisplayLayout.BandsSerializer.Add(allocatedGridBand);
			
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
				
			}

		
		}

		private void BindComboBox()
		{
			Infragistics.Win.ValueListItem fundItem = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem strategyItem = new Infragistics.Win.ValueListItem();
//			Infragistics.Win.ValueListItem bothItem = new Infragistics.Win.ValueListItem();

			fundItem.DisplayText  = OrderFields.HEADCOL_ALLOCATIONFUND ;
			fundItem.DataValue = 1; 
			cmbbxSelect.Items.Add(fundItem); 

			strategyItem.DisplayText  = OrderFields.HEADCOL_STRATEGY;
			strategyItem.DataValue = 2; 
			cmbbxSelect.Items.Add(strategyItem); 

//			bothItem.DisplayText  = "Both";
//			bothItem.DataValue = 3; 
//			cmbbxSelect.Items.Add(bothItem); 

			cmbbxSelect.SelectedIndex=0;
		}
		
		private void FilterFundStrategyColumns()
		{
            try
            {
                if (cmbbxSelect.SelectedIndex == 0)
                {
                    grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.HEADCOL_ALLOCATIONFUND].Hidden = false;
                    grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.HEADCOL_STRATEGY].Hidden = true;

                }
                //			else if(cmbbxSelect.SelectedIndex==1)
                else
                {
                    grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.HEADCOL_STRATEGY].Hidden = false;
                    grdAllocationReport.DisplayLayout.Bands[0].Columns[OrderFields.HEADCOL_ALLOCATIONFUND].Hidden = true;

                }
                //			else
                //			{
                //				grdAllocationReport.DisplayLayout.Bands[0].Columns["Strategy"].Hidden=false;
                //				grdAllocationReport.DisplayLayout.Bands[0].Columns["AllocationFund"].Hidden=false;
                //
                //			}
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
		private void ExportToExcel()
		{
			try
			{
				int _count = 0;
				Infragistics.Excel.Workbook workBook = new Infragistics.Excel.Workbook();
				//				workBook = null;
				string pathName = null;
				saveFileDialog1.InitialDirectory  = Application.StartupPath ;
				saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*" ;
				saveFileDialog1.RestoreDirectory = true;
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					pathName = saveFileDialog1.FileName;
				}
				else
				{
					return;
			
				}
			
							workBook = OnExportToExcel(_count, workBook, pathName) ;
							_count++;	
			
				workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
				try
				{
                    //Infragistics.Excel.BIFF8Writer.WriteWorkbookToFile(workBook,pathName);
                    workBook.Save(pathName);
				}
				catch(Exception )
				{
					MessageBox.Show("File is Open, Please Close the File then Save it.");
				}
			

			}
			catch(Exception ex)
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

		public Infragistics.Excel.Workbook OnExportToExcel(int key, Infragistics.Excel.Workbook workBook, string fileName)
		{
			try
			{	
				if(workBook == null )
				{
					workBook = this.ExcelExporter.Export(grdAllocationReport , fileName);
				}
				
				workBook.Worksheets.Add(this.grdAllocationReport.Name + key);
				workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[grdAllocationReport.Name + key];
				ExcelExporter.Export(this.grdAllocationReport, workBook);
				return workBook;
			}
			catch(Exception ex)
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
				switch(e.Tool.Key.ToLower())
				{
					case  "exporttoexcel":
						ExportToExcel();
						break;
				}
			}
			catch(Exception ex)
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
            DateTime date= dtPicker.DateTime.Date;
            DateTime currentDate=DateTime.Now.Date;
            //if (date.CompareTo(currentDate) > 0)
            //{
            //  // MessageBox.Show("Invalid Date ! Selected Date should not be Greater than Current date.");
            //    dtPicker.Value = DateTime.Now.Date;

            //}
           
        }

        private void colPkrRowColor_ColorChanged(object sender, EventArgs e)
        {
            rowForeColor= colPkrRowColor.Color;
            SetGridColors();
            
        }

	
	}
}

