using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientPreferences;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using System.IO;

namespace Prana.Blotter
{
    /// <summary>
    /// Summary description for ExecutionReport.
    /// </summary>
    public class ExecutionReport : System.Windows.Forms.UserControl
    {

        public ExecutionReport()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            TradingTicketPrefManager.GetInstance.Initialise(TradingTicketPreferenceType.Company, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, CachedDataManager.GetInstance.LoggedInUser.CompanyID);
            TradingTicketPrefManager.GetInstance.GetPreferenceBindingData(false, false);
            bool? isShowTargetQty = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IsShowTargetQTY;
            if (isShowTargetQty.HasValue)
                _isShowTargetQuantity = (bool)isShowTargetQty;
        }

        #region Private Variables
        private IContainer components;
        private const string FORM_NAME = "ExecutionReport";
        private PranaUltraGrid executionReportGrid;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPickerlower;
        private Infragistics.Win.Misc.UltraButton btnBlotterReport;
        private Infragistics.Win.Misc.UltraButton btnGetDetailedReport;
        private Infragistics.Win.Misc.UltraButton btnExportToExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPickerUpper;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraPanel upnlBlotterReportsHeader;
        private Infragistics.Win.Misc.UltraPanel upnlBlotterReportsBody;
        private CompanyUser _loginUser;
        private bool _isShowTargetQuantity = false;
        #endregion
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
                if (executionReportGrid != null)
                {
                    executionReportGrid.Dispose();
                }
                if (dtPickerlower != null)
                {
                    dtPickerlower.Dispose();
                }
                if (btnBlotterReport != null)
                {
                    btnBlotterReport.Dispose();
                }
                if (btnGetDetailedReport != null)
                {
                    btnGetDetailedReport.Dispose();
                }
                if (btnExportToExcel != null)
                {
                    btnExportToExcel.Dispose();
                }
                if (saveFileDialog1 != null)
                {
                    saveFileDialog1.Dispose();
                }
                if (ultraGridExcelExporter1 != null)
                {
                    ultraGridExcelExporter1.Dispose();
                }
                if (dtPickerUpper != null)
                {
                    dtPickerUpper.Dispose();
                }
                if (lblFrom != null)
                {
                    lblFrom.Dispose();
                }
                if (lblTo != null)
                {
                    lblTo.Dispose();
                }
                if (upnlBlotterReportsHeader != null)
                {
                    upnlBlotterReportsHeader.Dispose();
                }
                if (upnlBlotterReportsBody != null)
                {
                    upnlBlotterReportsBody.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
            }
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this.executionReportGrid = new PranaUltraGrid();
            this.dtPickerlower = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnBlotterReport = new Infragistics.Win.Misc.UltraButton();
            this.btnGetDetailedReport = new Infragistics.Win.Misc.UltraButton();
            this.btnExportToExcel = new Infragistics.Win.Misc.UltraButton();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.dtPickerUpper = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.upnlBlotterReportsHeader = new Infragistics.Win.Misc.UltraPanel();
            this.upnlBlotterReportsBody = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.executionReportGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerlower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerUpper)).BeginInit();
            this.upnlBlotterReportsHeader.ClientArea.SuspendLayout();
            this.upnlBlotterReportsHeader.SuspendLayout();
            this.upnlBlotterReportsBody.ClientArea.SuspendLayout();
            this.upnlBlotterReportsBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // executionReportGrid
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            this.executionReportGrid.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Override.DefaultColWidth = 80;
            this.executionReportGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.executionReportGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.executionReportGrid.DisplayLayout.InterBandSpacing = 1;
            appearance2.BackColor = System.Drawing.Color.Gold;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.executionReportGrid.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.executionReportGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.executionReportGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.executionReportGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.executionReportGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.executionReportGrid.DisplayLayout.Override.CellPadding = 0;
            this.executionReportGrid.DisplayLayout.Override.DefaultColWidth = 50;
            this.executionReportGrid.DisplayLayout.Override.GroupByRowInitialExpansionState = Infragistics.Win.UltraWinGrid.GroupByRowInitialExpansionState.Expanded;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.executionReportGrid.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.executionReportGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.executionReportGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.Black;
            appearance4.ForeColor = System.Drawing.Color.Orange;
            this.executionReportGrid.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.executionReportGrid.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.executionReportGrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.executionReportGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.executionReportGrid.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.executionReportGrid.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows)
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance6.BackColor = System.Drawing.SystemColors.Info;
            this.executionReportGrid.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance6;
            this.executionReportGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.executionReportGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance7;
            this.executionReportGrid.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.executionReportGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.executionReportGrid.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.executionReportGrid.Location = new System.Drawing.Point(0, 0);
            this.executionReportGrid.Name = "executionReportGrid";
            this.executionReportGrid.Size = new System.Drawing.Size(599, 394);
            this.executionReportGrid.TabIndex = 0;
            this.executionReportGrid.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.executionReportGrid.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.executionReportGrid_BeforeRowExpanded);
            // 
            // dtPickerlower
            // 
            appearance8.FontData.SizeInPoints = 10F;
            this.dtPickerlower.Appearance = appearance8;
            this.dtPickerlower.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerlower.Location = new System.Drawing.Point(52, 13);
            this.dtPickerlower.Name = "dtPickerlower";
            this.dtPickerlower.Size = new System.Drawing.Size(109, 25);
            this.dtPickerlower.TabIndex = 1;
            // 
            // btnBlotterReport
            // 
            appearance9.FontData.SizeInPoints = 10F;
            this.btnBlotterReport.Appearance = appearance9;
            this.btnBlotterReport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBlotterReport.Location = new System.Drawing.Point(299, 14);
            this.btnBlotterReport.Name = "btnBlotterReport";
            this.btnBlotterReport.Size = new System.Drawing.Size(102, 23);
            this.btnBlotterReport.TabIndex = 2;
            this.btnBlotterReport.Text = "Get Orders";
            this.btnBlotterReport.Click += new System.EventHandler(this.btnBlotterReport_Click);
            // 
            // btnGetDetailedReport
            // 
            appearance10.FontData.SizeInPoints = 10F;
            this.btnGetDetailedReport.Appearance = appearance10;
            this.btnGetDetailedReport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetDetailedReport.Location = new System.Drawing.Point(407, 14);
            this.btnGetDetailedReport.Name = "btnGetDetailedReport";
            this.btnGetDetailedReport.Size = new System.Drawing.Size(93, 23);
            this.btnGetDetailedReport.TabIndex = 3;
            this.btnGetDetailedReport.Text = "Get Details";
            this.btnGetDetailedReport.Click += new System.EventHandler(this.btnGetDetailedReport_Click);
            // 
            // btnExportToExcel
            // 
            appearance11.FontData.SizeInPoints = 10F;
            this.btnExportToExcel.Appearance = appearance11;
            this.btnExportToExcel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportToExcel.Location = new System.Drawing.Point(506, 14);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(65, 23);
            this.btnExportToExcel.TabIndex = 4;
            this.btnExportToExcel.Text = "Export";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // dtPickerUpper
            // 
            appearance12.FontData.SizeInPoints = 10F;
            this.dtPickerUpper.Appearance = appearance12;
            this.dtPickerUpper.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerUpper.Location = new System.Drawing.Point(187, 13);
            this.dtPickerUpper.Name = "dtPickerUpper";
            this.dtPickerUpper.Size = new System.Drawing.Size(109, 25);
            this.dtPickerUpper.TabIndex = 5;
            // 
            // lblFrom
            // 
            appearance13.FontData.SizeInPoints = 10F;
            appearance13.TextHAlignAsString = "Left";
            appearance13.TextVAlignAsString = "Middle";
            this.lblFrom.Appearance = appearance13;
            this.lblFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(7, 15);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(50, 21);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "From";
            // 
            // lblTo
            // 
            appearance14.FontData.SizeInPoints = 10F;
            appearance14.TextHAlignAsString = "Left";
            appearance14.TextVAlignAsString = "Middle";
            this.lblTo.Appearance = appearance14;
            this.lblTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(167, 18);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 15);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "to";
            // 
            // upnlBlotterReportsHeader
            // 
            // 
            // upnlBlotterReportsHeader.ClientArea
            // 
            this.upnlBlotterReportsHeader.ClientArea.Controls.Add(this.dtPickerUpper);
            this.upnlBlotterReportsHeader.ClientArea.Controls.Add(this.dtPickerlower);
            this.upnlBlotterReportsHeader.ClientArea.Controls.Add(this.lblTo);
            this.upnlBlotterReportsHeader.ClientArea.Controls.Add(this.btnBlotterReport);
            this.upnlBlotterReportsHeader.ClientArea.Controls.Add(this.lblFrom);
            this.upnlBlotterReportsHeader.ClientArea.Controls.Add(this.btnGetDetailedReport);
            this.upnlBlotterReportsHeader.ClientArea.Controls.Add(this.btnExportToExcel);
            this.upnlBlotterReportsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.upnlBlotterReportsHeader.Location = new System.Drawing.Point(0, 0);
            this.upnlBlotterReportsHeader.Name = "upnlBlotterReportsHeader";
            this.upnlBlotterReportsHeader.Size = new System.Drawing.Size(599, 50);
            this.upnlBlotterReportsHeader.TabIndex = 8;
            // 
            // upnlBlotterReportsBody
            // 
            // 
            // upnlBlotterReportsBody.ClientArea
            // 
            this.upnlBlotterReportsBody.ClientArea.Controls.Add(this.executionReportGrid);
            this.upnlBlotterReportsBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upnlBlotterReportsBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upnlBlotterReportsBody.Location = new System.Drawing.Point(0, 50);
            this.upnlBlotterReportsBody.Name = "upnlBlotterReportsBody";
            this.upnlBlotterReportsBody.Size = new System.Drawing.Size(599, 394);
            this.upnlBlotterReportsBody.TabIndex = 9;
            // 
            // ExecutionReport
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.upnlBlotterReportsBody);
            this.Controls.Add(this.upnlBlotterReportsHeader);
            this.Name = "ExecutionReport";
            this.Size = new System.Drawing.Size(599, 444);
            this.Load += new System.EventHandler(this.ExecutionReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.executionReportGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerlower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerUpper)).EndInit();
            this.upnlBlotterReportsHeader.ClientArea.ResumeLayout(false);
            this.upnlBlotterReportsHeader.ClientArea.PerformLayout();
            this.upnlBlotterReportsHeader.ResumeLayout(false);
            this.upnlBlotterReportsBody.ClientArea.ResumeLayout(false);
            this.upnlBlotterReportsBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void btnBlotterReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                DateTime lowerdate = this.dtPickerlower.DateTime;
                DateTime upperdate = this.dtPickerUpper.DateTime;

                //Get Time in current TimeZone
                //GetLocalDateTime(ref lowerdate,ref upperdate);

                if (upperdate < lowerdate)
                {
                    MessageBox.Show("End Date cannot be lower than Start Date! Please choose another date.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.dtPickerUpper.Focus();
                    return;
                }

                GetSummaryReport(lowerdate, upperdate);
            }
            #region Catch Exception
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion


        }

        private delegate void DataSourceUpdated(OrderCollection oc);

        private async void GetReportDetails(DateTime lowerdate, DateTime upperdate)
        {
            try
            {
                this.executionReportGrid.DataSource = null;
                OrderCollection dayOrders = await System.Threading.Tasks.Task.Run(() => GetDayOrders(lowerdate, upperdate));

                //PratikR.
                //PRANA-27065
                if (UIValidation.GetInstance().validate(executionReportGrid))
                {
                    if (executionReportGrid.InvokeRequired)
                    {
                        DataSourceUpdated mi = new DataSourceUpdated(SetDataSource);
                        executionReportGrid.BeginInvoke(mi, new object[] { dayOrders });
                    }
                    else
                    {
                        SetDataSource(dayOrders);
                    }
                }
                else
                {
                    return;
                }

                HideColumnsBand1();
                HideColumnsBand2();
            }
            #region Catch Exception
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        public OrderCollection GetDayOrders(DateTime lowerdate, DateTime upperdate)
        {
            try
            {
                Dictionary<string, OrderCollection> dictOrderCollectionOrderIdWise = new Dictionary<string, OrderCollection>();
                OrderCollection dayOrders = new OrderCollection();
                Dictionary<string, string> dictStagedOrdIdsTif = new Dictionary<string, string>();
                dictStagedOrdIdsTif = BlotterReportManager.GetInstance().GetStagedOrderIdsAndTif(lowerdate, upperdate);
                dayOrders = BlotterReportManager.GetInstance().GetOrdersByDate(lowerdate, upperdate, _loginUser.CompanyUserID);
                foreach (Order order in dayOrders)
                {
                    bool isGtcGtd = (order.TIF == FIXConstants.TIF_GTC || order.TIF == FIXConstants.TIF_GTD);
                    if (isGtcGtd)
                    {
                        if (!string.IsNullOrEmpty(order.OrderID) && (!dictStagedOrdIdsTif.ContainsKey(order.StagedOrderID) ))
                        {
                            if (order.OrderID.Contains('_'))
                            {
                                string[] parts = order.OrderID.Split('_');
                                order.OrderID = parts[0];
                            }
                            order.SubOrders = BlotterReportManager.GetInstance().GetTrailByOrderID(order.OrderID, order.AUECID, order.PranaMsgType, _loginUser.CompanyUserID, isGtcGtd);
                            if (order.OrderID.Length < 16)
                            {
                                order.OrderID = order.StagedOrderID;
                            }
                            if (!dictOrderCollectionOrderIdWise.ContainsKey(order.OrderID))
                            {
                                dictOrderCollectionOrderIdWise.Add(order.OrderID, order.SubOrders);
                            }
                            else
                            {
                                foreach (Order subOrder in order.SubOrders)
                                {
                                    dictOrderCollectionOrderIdWise[order.OrderID].Add(subOrder);
                                }
                            }
                        }
                        else
                        {
                            order.SubOrders = BlotterReportManager.GetInstance().GetTrailByOrderID(order.ClOrderID, order.AUECID, order.PranaMsgType, _loginUser.CompanyUserID);
                            bool hasSingleDayHistory = (dictStagedOrdIdsTif.ContainsKey(order.StagedOrderID) && (dictStagedOrdIdsTif[order.StagedOrderID] != FIXConstants.TIF_GTD || dictStagedOrdIdsTif[order.StagedOrderID] != FIXConstants.TIF_GTC));
                            if (order.SubOrders.Count <= 1 || hasSingleDayHistory)
                            {
                                dictOrderCollectionOrderIdWise.Add(order.ClOrderID, order.SubOrders);
                                order.OrderID = order.ClOrderID;
                            }
                        }
                    }
                    else
                        order.SubOrders = BlotterReportManager.GetInstance().GetTrailByOrderID(order.ClOrderID, order.AUECID, order.PranaMsgType, _loginUser.CompanyUserID);
                }
                dayOrders = UpdateDayOrders(dayOrders, dictOrderCollectionOrderIdWise, dictStagedOrdIdsTif);
                return dayOrders;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// To update and sort the subOrders in case of gtc/gtd.
        /// </summary>
        private OrderCollection UpdateDayOrders(OrderCollection dayOrders, Dictionary<string, OrderCollection> dictOrderCollectionOrderIdWise, Dictionary<string,string> dictStagedOrdIdsTif)
        {
            OrderCollection updatedDayOrders = new OrderCollection();
            List<string> lstMultidayHistory = new List<string>();
            try
            {
                lstMultidayHistory = dictOrderCollectionOrderIdWise.Keys.ToList();
                foreach(Order order in dayOrders)
                {
                    bool hasMultiDayHistory = (order.TIF == FIXConstants.TIF_Day && (dictStagedOrdIdsTif.ContainsKey(order.StagedOrderID) && ((dictStagedOrdIdsTif[order.StagedOrderID] == FIXConstants.TIF_GTC) || dictStagedOrdIdsTif[order.StagedOrderID] == FIXConstants.TIF_GTD)));
                    if (dictOrderCollectionOrderIdWise.ContainsKey(order.OrderID))
                    {
                        List<Order> sortedOrders = new List<Order>();
                        List<string> listToRemoveDuplicacy = new List<string>();
                        string latestClOrdId = order.ClOrderID;
                        order.SubOrders = dictOrderCollectionOrderIdWise[order.OrderID];
                        int orderStatusNewCount = 0;
                        int orderStatusExpiredCount = 0;
                        foreach (Order subOrder in order.SubOrders)
                        {
                            if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired && orderStatusExpiredCount == 0)
                            {
                                orderStatusExpiredCount++;
                            }
                            else if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired && orderStatusExpiredCount >= 1)
                                continue;

                            if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New && orderStatusNewCount == 0)
                            {
                                orderStatusNewCount++;
                                order.CurrentUser = subOrder.CurrentUser;
                            }
                            else if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New && orderStatusNewCount >= 1)
                                continue;
                            string key = subOrder.TransactionTime.ToString() + subOrder.ClOrderID + subOrder.MsgSeqNum.ToString() + subOrder.OrderStatus;
                            if (!listToRemoveDuplicacy.Contains(key))
                            {
                                sortedOrders.Add(subOrder);
                                listToRemoveDuplicacy.Add(key);
                            }
                        }
                        sortedOrders = sortedOrders.OrderBy(t => t.TransactionTime).ThenBy(t => t.CumQty).ToList();
                        OrderCollection updatedOrder = new OrderCollection();
                        double prevQty = 0;
                        string prevStatus = string.Empty;
                        long prevMsgSeqNum = 0;
                        foreach (Order suborder in sortedOrders)
                        {
                            if (suborder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced && prevStatus == FIXConstants.ORDSTATUS_PartiallyFilled)
                                continue;
                            if (suborder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PartiallyFilled && prevStatus == FIXConstants.ORDSTATUS_Replaced
                               && prevMsgSeqNum == suborder.MsgSeqNum)
                            {
                                continue;
                            }
                            if (suborder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced || suborder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace ||
                                suborder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled || suborder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel)
                            {
                                suborder.CumQty = prevQty;
                            }
                            updatedOrder.Add(suborder);
                            prevQty = suborder.CumQty;
                            order.Price = suborder.AvgPrice;
                            prevStatus = suborder.OrderStatusTagValue;
                            prevMsgSeqNum = suborder.MsgSeqNum;
                        }
                        order.SubOrders = updatedOrder;
                        updatedDayOrders.Add(order);
                        dictOrderCollectionOrderIdWise.Remove(order.OrderID);
                    }
                    else if(!hasMultiDayHistory && order.TIF != FIXConstants.TIF_GTC && order.TIF != FIXConstants.TIF_GTD && !lstMultidayHistory.Contains(order.OrderID))
                    {
                        updatedDayOrders.Add(order);
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
            return updatedDayOrders;
        }

        private async void GetSummaryReport(DateTime lowerdate, DateTime upperdate)
        {
            try
            {
                this.executionReportGrid.DataSource = null;

                OrderCollection dayOrders = await System.Threading.Tasks.Task.Run(() => BlotterReportManager.GetInstance().GetTradeSummaryOfDay(lowerdate, _loginUser.CompanyUserID, upperdate));
                dayOrders = UpdateOrderCollection(dayOrders,lowerdate,upperdate);
                //PratikR.
                //PRANA-27065
                if (UIValidation.GetInstance().validate(executionReportGrid))
                {
                    if (executionReportGrid.InvokeRequired)
                    {
                        DataSourceUpdated mi = new DataSourceUpdated(SetDataSource);
                        executionReportGrid.BeginInvoke(mi, new object[] { dayOrders });
                    }
                    else
                    {
                        SetDataSource(dayOrders);
                    }
                }
                else
                {
                    return;
                }

                HideColumnsSummaryBand();
                this.executionReportGrid.DisplayLayout.Rows.ExpandAll(true);// = GroupByRowInitialExpansionState.Expanded;
            }
            #region Catch Exception
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }

        /// <summary>
        /// Update order collection for GTC/GTD order.
        /// </summary>
        private OrderCollection UpdateOrderCollection(OrderCollection orders, DateTime lowerdate, DateTime upperdate)
        {
            OrderCollection updatedOrders = new OrderCollection();
            Dictionary<string, OrderCollection> dictOrderCollectionParentClOrderIdWise = new Dictionary<string, OrderCollection>();
            Dictionary<string, string> dictStagedOrdIdsTif = new Dictionary<string, string>();
            Dictionary<string,string> dictMultiDayStagedOrdIds = new Dictionary<string, string>();
            dictStagedOrdIdsTif = BlotterReportManager.GetInstance().GetStagedOrderIdsAndTif(lowerdate, upperdate);
            try
            {
                foreach(Order order in orders)
                {
                    bool hasMultiDayHistory = (order.TIF == FIXConstants.TIF_Day && (dictStagedOrdIdsTif.ContainsKey(order.StagedOrderID) && ((dictStagedOrdIdsTif[order.StagedOrderID] == FIXConstants.TIF_GTC) || dictStagedOrdIdsTif[order.StagedOrderID] == FIXConstants.TIF_GTD)));
                    if (order.OrderID.Contains('_'))
                    {
                        string[] parts = order.OrderID.Split('_');
                        order.OrderID = parts[0];
                    }
                    if (order.TIF == FIXConstants.TIF_GTC || order.TIF == FIXConstants.TIF_GTD || dictOrderCollectionParentClOrderIdWise.ContainsKey(order.OrderID) || hasMultiDayHistory)
                    {
                        if (dictOrderCollectionParentClOrderIdWise.ContainsKey(order.OrderID))
                        {
                            dictOrderCollectionParentClOrderIdWise[order.OrderID].Add(order);
                        }
                        else if(dictMultiDayStagedOrdIds.ContainsKey(order.StagedOrderID) && dictOrderCollectionParentClOrderIdWise.ContainsKey(dictMultiDayStagedOrdIds[order.StagedOrderID])) 
                        {
                            dictOrderCollectionParentClOrderIdWise[dictMultiDayStagedOrdIds[order.StagedOrderID]].Add(order);
                        }
                        else
                        {
                            OrderCollection orderCollection = new OrderCollection();
                            orderCollection.Add(order);
                            string orderId = order.ParentClOrderID;
                            if(!string.IsNullOrWhiteSpace(order.OrderID))
                                orderId = order.OrderID;
                            dictOrderCollectionParentClOrderIdWise.Add(orderId, orderCollection);
                            dictMultiDayStagedOrdIds.Add(order.ParentClOrderID, order.OrderID);
                        }
                    }
                    else
                    {
                        updatedOrders.Add(order);
                    }
                }
                updatedOrders = UpdateGtcGtdOrder(dictOrderCollectionParentClOrderIdWise,updatedOrders);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return updatedOrders;
        }

        /// <summary>
        /// Update order collection for GTC/GTD order.
        /// </summary>
        /// <param name="dictOrderCollectionParentClOrderIdWise"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        private OrderCollection UpdateGtcGtdOrder(Dictionary<string, OrderCollection> dictOrderCollectionParentClOrderIdWise,OrderCollection orders)
        {
            try
            {
                foreach (KeyValuePair<string, OrderCollection> kvpOrderCollection in dictOrderCollectionParentClOrderIdWise)
                {
                    Order mergedOrder = null;
                    foreach (Order order in kvpOrderCollection.Value)
                    {
                        if (mergedOrder == null)
                            mergedOrder = order;
                        else
                        {
                            if (order.AvgPrice != 0)
                            {
                                mergedOrder.AvgPrice = order.AvgPrice;
                            }
                            mergedOrder.CumQty += order.CumQty;
                            if (order.TransactionTime > mergedOrder.TransactionTime || (order.TransactionTime == mergedOrder.TransactionTime && mergedOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New))
                            {
                                if (mergedOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_New)
                                    mergedOrder.CurrentUser = order.CurrentUser;
                                mergedOrder.OrderStatus = order.OrderStatus;
                            }
                        }
                    }
                    orders.Add(mergedOrder);
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
            return orders;
        }

        //This method is created to do marshaling
        private void SetDataSource(OrderCollection oc)
        {
            try
            {
                this.executionReportGrid.DataSource = oc;
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

        #region SetColumnsVisibility and Properties
        private void HideColumnsBand2()
        {
            if (executionReportGrid.DisplayLayout.Bands.Count > 1)
            {
                executionReportGrid.DisplayLayout.Bands[1].Override.HeaderClickAction = HeaderClickAction.Select;
                ColumnsCollection columns = executionReportGrid.DisplayLayout.Bands[1].Columns;
                foreach (UltraGridColumn column in columns)
                {

                    if (column.Key != OrderFields.PROPERTY_LAST_SHARES
                        && column.Key != OrderFields.PROPERTY_TRANSACTION_TIME
                        && column.Key != OrderFields.PROPERTY_LASTPRICE
                        && column.Key != OrderFields.PROPERTY_ORDER_STATUS
                        && column.Key != OrderFields.PROPERTY_AVGPRICE
                       && column.Key != OrderFields.PROPERTY_CANCEL_ORDER_ID
                        && column.Key != OrderFields.PROPERTY_EXECUTED_QTY
                        && column.Key != OrderFields.PROPERTY_TEXT
                        && column.Key != OrderFields.PROPERTY_COUNTERPARTY_NAME
                        && column.Key != OrderFields.PROPERTY_EXCHANGE)
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        column.Hidden = false;
                        column.Width = 64;
                    }
                }

                //define visible positions of columns
                columns[OrderFields.PROPERTY_EXECUTED_QTY].Header.VisiblePosition = 0;
                columns[OrderFields.PROPERTY_EXECUTED_QTY].Header.Caption = OrderFields.CAPTION_EXECUTED_QTY;

                columns[OrderFields.PROPERTY_AVGPRICE].Header.VisiblePosition = 1;
                columns[OrderFields.PROPERTY_AVGPRICE].Header.Caption = OrderFields.CAPTION_AVGPRICE;

                columns[OrderFields.PROPERTY_LAST_SHARES].Header.VisiblePosition = 2;
                columns[OrderFields.PROPERTY_LAST_SHARES].Header.Caption = OrderFields.CAPTION_GRID_LAST_SHARES;

                columns[OrderFields.PROPERTY_LASTPRICE].Header.VisiblePosition = 3;
                columns[OrderFields.PROPERTY_LASTPRICE].Header.Caption = OrderFields.CAPTION_LASTPRICE_BLOTTER;

                columns[OrderFields.PROPERTY_ORDER_STATUS].Header.VisiblePosition = 4;
                columns[OrderFields.PROPERTY_ORDER_STATUS].Header.Caption = OrderFields.CAPTION_ORDER_STATUS;

                columns[OrderFields.PROPERTY_CANCEL_ORDER_ID].Header.VisiblePosition = 6;
                columns[OrderFields.PROPERTY_CANCEL_ORDER_ID].Header.Caption = OrderFields.PROPERTY_CANCEL_ORDER_ID;

                columns[OrderFields.PROPERTY_TRANSACTION_TIME].Header.VisiblePosition = 7;
                columns[OrderFields.PROPERTY_TRANSACTION_TIME].Header.Caption = OrderFields.CAPTION_TRANSACTION_TIME;
                columns[OrderFields.PROPERTY_TRANSACTION_TIME].Format = DateTimeConstants.NirvanaDateTimeFormat;

                columns[OrderFields.PROPERTY_TEXT].Header.VisiblePosition = 8;
                columns[OrderFields.PROPERTY_TEXT].Header.Caption = OrderFields.CAPTION_TEXT;

                columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.VisiblePosition = 9;
                columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.Caption = OrderFields.CAPTION_COUNTERPARTY_NAME;

                columns[OrderFields.PROPERTY_EXCHANGE].Header.VisiblePosition = 10;
                columns[OrderFields.PROPERTY_EXCHANGE].Header.Caption = OrderFields.CAPTION_EXCHANGE;

                if (columns.Exists(OrderFields.PROPERTY_PROCESSDATE))
                    columns[OrderFields.PROPERTY_PROCESSDATE].Header.Caption = OrderFields.CAPTION_PROCESS_DATE;

                //define text align of numeric fields to right align
                columns[OrderFields.PROPERTY_EXECUTED_QTY].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                columns[OrderFields.PROPERTY_AVGPRICE].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                columns[OrderFields.PROPERTY_LAST_SHARES].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                columns[OrderFields.PROPERTY_LASTPRICE].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                //set format to 4 decimal places
                columns[OrderFields.PROPERTY_LASTPRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                columns[OrderFields.PROPERTY_AVGPRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                columns[OrderFields.PROPERTY_LAST_SHARES].Format = ApplicationConstants.FORMAT_COSTBASIS;
                columns[OrderFields.PROPERTY_EXECUTED_QTY].Format = ApplicationConstants.FORMAT_COSTBASIS;
            }
        }

        private void HideColumnsBand1()
        {
            executionReportGrid.DisplayLayout.Bands[0].Override.RowAppearance.BackColor = System.Drawing.Color.Black;
            executionReportGrid.DisplayLayout.Bands[1].Override.RowAlternateAppearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            executionReportGrid.DisplayLayout.Bands[0].Override.RowAppearance.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0); ;
            executionReportGrid.DisplayLayout.Bands[1].Override.RowAppearance.ForeColor = System.Drawing.Color.DeepSkyBlue;

            ColumnsCollection columns = executionReportGrid.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != OrderFields.PROPERTY_CANCEL_ORDER_ID
                    && column.Key != OrderFields.PROPERTY_ORDER_SIDE
                    && column.Key != OrderFields.PROPERTY_ORDER_TYPE
                    && column.Key != OrderFields.PROPERTY_SYMBOL
                    && column.Key != OrderFields.PROPERTY_QUANTITY
                    && column.Key != OrderFields.PROPERTY_PRICE
                    && column.Key != OrderFields.PROPERTY_TRADING_ACCOUNT
                    && column.Key != OrderFields.PROPERTY_AVGFXRATE
                    && column.Key != OrderFields.PROPERTY_USER
                    && column.Key != OrderFields.PROPERTY_COUNTERPARTY_NAME
                    && column.Key != OrderFields.PROPERTY_PROCESSDATE
                    && column.Key != OrderFields.PROPERTY_LEVEL1NAME
                    && column.Key != OrderFields.PROPERTY_MODIFIED_USER
                    && column.Key != OrderFields.PROPERTY_CURRENT_USER
                    )
                {
                    column.Hidden = true;
                }
                else
                {
                    column.Hidden = false;
                    column.Width = 65;
                }
            }
            //define visible positions of columns
            columns[OrderFields.PROPERTY_SYMBOL].Header.VisiblePosition = 0;

            columns[OrderFields.PROPERTY_ORDER_SIDE].Header.VisiblePosition = 1;
            columns[OrderFields.PROPERTY_ORDER_SIDE].Header.Caption = OrderFields.CAPTION_ORDER_SIDE;

            columns[OrderFields.PROPERTY_ORDER_TYPE].Header.VisiblePosition = 2;
            columns[OrderFields.PROPERTY_ORDER_TYPE].Header.Caption = OrderFields.CAPTION_ORDER_TYPE;

            columns[OrderFields.PROPERTY_QUANTITY].Header.VisiblePosition = 3;
            if (_isShowTargetQuantity)
                columns[OrderFields.PROPERTY_QUANTITY].Header.Caption = OrderFields.CAPTION_TARGET_QUANTITY;
            else
                columns[OrderFields.PROPERTY_QUANTITY].Header.Caption = OrderFields.PROPERTY_QUANTITY;

            columns[OrderFields.PROPERTY_PRICE].Header.VisiblePosition = 4;
            columns[OrderFields.PROPERTY_PRICE].Header.Caption = OrderFields.CAPTION_PRICE;

            columns[OrderFields.PROPERTY_CANCEL_ORDER_ID].Header.VisiblePosition = 5;
            columns[OrderFields.PROPERTY_CANCEL_ORDER_ID].Header.Caption = OrderFields.PROPERTY_CANCEL_ORDER_ID;

            columns[OrderFields.PROPERTY_USER].Header.VisiblePosition = 6;
            columns[OrderFields.PROPERTY_USER].Header.Caption = OrderFields.CAPTION_ORIGINAL_USER;

            columns[OrderFields.PROPERTY_TRADING_ACCOUNT].Header.VisiblePosition = 7;
            columns[OrderFields.PROPERTY_TRADING_ACCOUNT].Header.Caption = OrderFields.CAPTION_TRADER;

            columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.VisiblePosition = 8;
            columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.Caption = OrderFields.CAPTION_COUNTERPARTY_NAME;

            columns[OrderFields.PROPERTY_MODIFIED_USER].Header.VisiblePosition = 9;
            columns[OrderFields.PROPERTY_MODIFIED_USER].Header.Caption = OrderFields.CAPTION_MODIFIED_USER;

            columns[OrderFields.PROPERTY_CURRENT_USER].Header.VisiblePosition = 10;
            columns[OrderFields.PROPERTY_CURRENT_USER].Header.Caption = OrderFields.CAPTION_CURRENT_USER;

            //http://jira.nirvanasolutions.com:8080/browse/PRANA-8992
            if (columns.Exists(OrderFields.PROPERTY_AVGFXRATE))
            {
                columns[OrderFields.PROPERTY_AVGFXRATE].Header.VisiblePosition = 11;
                columns[OrderFields.PROPERTY_AVGFXRATE].Header.Caption = OrderFields.CAPTION_AVG_FX_RATE;
            }

            columns[OrderFields.PROPERTY_PROCESSDATE].Header.Caption = OrderFields.CAPTION_PROCESS_DATE;
            columns[OrderFields.PROPERTY_LEVEL1NAME].Header.Caption = OrderFields.CAPTION_LEVEL1NAME;

            //define text align of numeric fields to right align
            columns[OrderFields.PROPERTY_QUANTITY].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_PRICE].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_USER].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_MODIFIED_USER].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_CURRENT_USER].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //set format to 4 decimal places
            columns[OrderFields.PROPERTY_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
            columns[OrderFields.PROPERTY_QUANTITY].Format = ApplicationConstants.FORMAT_COSTBASIS;
        }

        private void HideColumnsSummaryBand()
        {
            ColumnsCollection columns = executionReportGrid.DisplayLayout.Bands[0].Columns;
            columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.Caption = OrderFields.CAPTION_COUNTERPARTY_NAME;

            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != OrderFields.PROPERTY_PARENT_CL_ORDERID
                    && column.Key != OrderFields.PROPERTY_ORDER_SIDE
                    && column.Key != OrderFields.PROPERTY_SYMBOL
                    && column.Key != OrderFields.PROPERTY_ORDER_STATUS
                    && column.Key != OrderFields.PROPERTY_AVGPRICE
                    && column.Key != OrderFields.PROPERTY_COUNTERPARTY_NAME
                    && column.Key != OrderFields.PROPERTY_EXECUTED_QTY
                    && column.Key != OrderFields.PROPERTY_USER
                    && column.Key != OrderFields.PROPERTY_PROCESSDATE
                    && column.Key != OrderFields.PROPERTY_LEVEL1NAME
                    && column.Key != OrderFields.PROPERTY_MODIFIED_USER
                    && column.Key != OrderFields.PROPERTY_CURRENT_USER
                    )
                {
                    column.Hidden = true;
                }
                else
                {
                    column.Hidden = false;
                    column.Width = 80;
                }
            }
            //define visible positions of columns
            columns[OrderFields.PROPERTY_SYMBOL].Header.VisiblePosition = 0;

            columns[OrderFields.PROPERTY_ORDER_SIDE].Header.VisiblePosition = 1;
            columns[OrderFields.PROPERTY_ORDER_SIDE].Header.Caption = OrderFields.CAPTION_ORDER_SIDE;

            columns[OrderFields.PROPERTY_EXECUTED_QTY].Header.VisiblePosition = 3;
            columns[OrderFields.PROPERTY_EXECUTED_QTY].Header.Caption = OrderFields.CAPTION_EXECUTED_QTY;

            columns[OrderFields.PROPERTY_AVGPRICE].Header.VisiblePosition = 4;
            columns[OrderFields.PROPERTY_AVGPRICE].Header.Caption = OrderFields.CAPTION_AVGPRICE;

            columns[OrderFields.PROPERTY_ORDER_STATUS].Header.VisiblePosition = 5;
            columns[OrderFields.PROPERTY_ORDER_STATUS].Header.Caption = OrderFields.CAPTION_ORDER_STATUS;

            columns[OrderFields.PROPERTY_PARENT_CL_ORDERID].Header.VisiblePosition = 6;
            columns[OrderFields.PROPERTY_PARENT_CL_ORDERID].Header.Caption = OrderFields.CAPTION_ORDER_ID;

            columns[OrderFields.PROPERTY_USER].Header.VisiblePosition = 7;
            columns[OrderFields.PROPERTY_USER].Header.Caption = OrderFields.CAPTION_ORIGINAL_USER;

            columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.VisiblePosition = 8;
            columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.Caption = OrderFields.CAPTION_COUNTERPARTY_NAME;

            columns[OrderFields.PROPERTY_LEVEL1NAME].Header.VisiblePosition = 9;
            columns[OrderFields.PROPERTY_LEVEL1NAME].Header.Caption = OrderFields.CAPTION_LEVEL1NAME;

            columns[OrderFields.PROPERTY_PROCESSDATE].Header.VisiblePosition = 10;
            columns[OrderFields.PROPERTY_PROCESSDATE].Header.Caption = OrderFields.CAPTION_PROCESS_DATE;

            columns[OrderFields.PROPERTY_MODIFIED_USER].Header.VisiblePosition = 11;
            columns[OrderFields.PROPERTY_MODIFIED_USER].Header.Caption = OrderFields.CAPTION_MODIFIED_USER;

            columns[OrderFields.PROPERTY_CURRENT_USER].Header.VisiblePosition = 12;
            columns[OrderFields.PROPERTY_CURRENT_USER].Header.Caption = OrderFields.CAPTION_CURRENT_USER;

            //define text align of numeric fields to right align
            columns[OrderFields.PROPERTY_EXECUTED_QTY].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_AVGPRICE].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_USER].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_MODIFIED_USER].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columns[OrderFields.PROPERTY_CURRENT_USER].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;


            //set format to 4 decimal places
            columns[OrderFields.PROPERTY_AVGPRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
            columns[OrderFields.PROPERTY_EXECUTED_QTY].Format = ApplicationConstants.FORMAT_COSTBASIS;
        }
        #endregion

        private void btnGetDetailedReport_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime lowerdate = this.dtPickerlower.DateTime;
                DateTime upperdate = this.dtPickerUpper.DateTime;

                //GetLocalDateTime(ref lowerdate, ref upperdate);

                if (upperdate < lowerdate)
                {
                    MessageBox.Show("End Date cannot be lower than Start Date! Please choose another date.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.dtPickerUpper.Focus();
                    return;
                }
                GetReportDetails(lowerdate, upperdate);

            }
            #region Catch Exception
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (ExportToExcel())
            {
                MessageBox.Show("Report Succesfully saved.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ExportToExcel()
        {
            bool result = false;
            try
            {
                this.ultraGridExcelExporter1.InitializeColumn += ultragridExcelExporter_InitializeColumn;
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook(
                    Infragistics.Documents.Excel.WorkbookFormat.Excel2007
                    );
                string pathName = null;
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return result;

                }
                string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                workBook = this.ultraGridExcelExporter1.Export(this.executionReportGrid, workBook.Worksheets[workbookName]);
                // Infragistics.Documents.Excel.BIFF8Writer.WriteWorkbookToFile(workBook, pathName);
                workBook.Save(pathName);
                this.ultraGridExcelExporter1.InitializeColumn -= ultragridExcelExporter_InitializeColumn;
                result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }

            return result;
        }
        private bool checkNumeric(Type dataType)
        {
            if (dataType == null)
                return false;

            return (dataType == typeof(int)
                    || dataType == typeof(double)
                    || dataType == typeof(long)
                    || dataType == typeof(short)
                    || dataType == typeof(float)
                    || dataType == typeof(Int16)
                    || dataType == typeof(Int32)
                    || dataType == typeof(Int64)
                    || dataType == typeof(uint)
                    || dataType == typeof(UInt16)
                    || dataType == typeof(UInt32)
                    || dataType == typeof(UInt64)
                    || dataType == typeof(byte)
                    || dataType == typeof(sbyte)
                    || dataType == typeof(Single)
                   );
        }

        private void ultragridExcelExporter_InitializeColumn(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.InitializeColumnEventArgs e)
        {
            if (checkNumeric(e.Column.DataType))
                e.ExcelFormatStr = "#0.0000";

        }

        protected void ultraGridExcelExporter1_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.ExcelExportInitializeRowEventArgs e)
        {

        }

        //private void GetLocalDateTime(ref DateTime lowerTime, ref DateTime upperTime)
        //{
        //    //get local time from Current TimeZone, CurrentTimeZone available in BusinessLogic
        //    //lowerTime = Prana.BusinessLogic.TimeZoneChangeHelper.GetLocalTime(lowerTime);
        //    //upperTime = Prana.BusinessLogic.TimeZoneChangeHelper.GetLocalTime(upperTime);

        //    lowerTime = Prana.BusinessObjects.TimeZoneInfo.ConvertTimeZoneToUtc(lowerTime, Prana.BusinessObjects.TimeZoneInfo.CurrentTimeZone);
        //    upperTime = Prana.BusinessObjects.TimeZoneInfo.ConvertTimeZoneToUtc(upperTime, Prana.BusinessObjects.TimeZoneInfo.CurrentTimeZone);

        //    //required as we need upperDate Included
        //    upperTime = upperTime.Add(new TimeSpan(1, 0, 0, 0));
        //}

        private void executionReportGrid_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            UltraGridRow currentRow = e.Row;
            if (currentRow.ChildBands != null)
            {
                foreach (UltraGridRow childRow in currentRow.ChildBands[0].Rows)
                {
                    childRow.ExpansionIndicator = ShowExpansionIndicator.Never;
                }
            }

        }

        private void ExecutionReport_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnBlotterReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBlotterReport.ForeColor = System.Drawing.Color.White;
                btnBlotterReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBlotterReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBlotterReport.UseAppStyling = false;
                btnBlotterReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetDetailedReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetDetailedReport.ForeColor = System.Drawing.Color.White;
                btnGetDetailedReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetDetailedReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetDetailedReport.UseAppStyling = false;
                btnGetDetailedReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExportToExcel.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExportToExcel.ForeColor = System.Drawing.Color.White;
                btnExportToExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExportToExcel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExportToExcel.UseAppStyling = false;
                btnExportToExcel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        public void ExportData(string gridName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "executionReportGrid")
                {
                    exporter.Export(executionReportGrid, filePath);
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

    }
}
