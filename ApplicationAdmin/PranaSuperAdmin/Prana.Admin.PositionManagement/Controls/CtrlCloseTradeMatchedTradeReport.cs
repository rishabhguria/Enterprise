using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;

using Infragistics.Win.UltraWinGrid;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCloseTradeMatchedTradeReport : UserControl
    {
        #region Grid Column Names

        const string COL_Symbol = "Symbol";
        const string COL_TradeDate = "TradeDate";
        const string COL_TotalQty = "TotalQty";
        const string COL_ClosedQty = "ClosedQty";
        const string COL_OpenQty = "OpenQty";
        const string COL_Fund = "Fund";
        const string COL_RealizedPNL = "RealizedPNL";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        private CloseTradeMatchedTradeReport _closeTradeMatchedTradeReport = new CloseTradeMatchedTradeReport();

        public CtrlCloseTradeMatchedTradeReport()
        {
            InitializeComponent();
        }

        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        /// <summary>
        /// Initialize the control.
        /// </summary>
        public void InitControl()
        {
            if (!_isInitialized)
            {
                SetupBinding();
                _isInitialized = true;
            }
        }

        #endregion

        private void SetupBinding()
        {
            _formBindingSource.DataSource = RetrieveCloseTradeMatchedTradeReport();

            //create a binding object
            Binding reportDateBinding = new System.Windows.Forms.Binding("Value", _formBindingSource, "ReportDate");
            //add new binding
            cmbDate.DataBindings.Add(reportDateBinding);            

            grdMatchedTradeReport.DataMember = "ReportItems";
            grdMatchedTradeReport.DataSource = _formBindingSource;

        }

        private CloseTradeMatchedTradeReport RetrieveCloseTradeMatchedTradeReport()
        {
            //_closeTradeMatchedTradeReport.DataSourceNameID = dataSourceNameID;
            //_closeTradeMatchedTradeReport.MappingItemList = MappingItemList.RetrieveFundMappings(dataSourceNameID);

            _closeTradeMatchedTradeReport.ReportDate = DateTime.Today;
            _closeTradeMatchedTradeReport.ReportItems = GetReportItems();
            return _closeTradeMatchedTradeReport;
        }

        private SortableSearchableList<CloseTradeMatchedTradeReportItem> GetReportItems()
        {
            SortableSearchableList<CloseTradeMatchedTradeReportItem> list = new SortableSearchableList<CloseTradeMatchedTradeReportItem>();

            CloseTradeMatchedTradeReportItem item = new CloseTradeMatchedTradeReportItem();

            item.Fund.ID = 1;
            item.Fund.Name = "Long";
            item.ClosedQty = -2000;
            item.OpenQty = 98000;
            item.Symbol = "MSFT";
            item.TradeDate = new DateTime(2006, 1, 1);
            item.TotalQty = 100000;
            item.RealizedPNL = 5100;

            list.Add(item);

            item = new CloseTradeMatchedTradeReportItem();

            item.Fund.ID = 2;
            item.Fund.Name = "Short";
            item.ClosedQty = 1000;
            item.OpenQty = 49000;
            item.Symbol = "DELL";
            item.TradeDate = new DateTime(2005, 12,20);
            item.TotalQty = 50000;
            item.RealizedPNL = -450;

            list.Add(item);

            return list;
        }

        private void grdMatchedTradeReport_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdMatchedTradeReport.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdMatchedTradeReport.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdMatchedTradeReport.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdMatchedTradeReport.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdMatchedTradeReport.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdMatchedTradeReport.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdMatchedTradeReport.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdMatchedTradeReport.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdMatchedTradeReport.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 1;

            UltraGridColumn colTradeDate = band.Columns[COL_TradeDate];
            colTradeDate.Header.Caption = "Trade Date";
            colTradeDate.Header.VisiblePosition = 2;

            UltraGridColumn colTotalQty = band.Columns[COL_TotalQty];
            colTotalQty.Header.Caption = "Total Qty";
            colTotalQty.Header.VisiblePosition = 3;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colFund.Header.Caption = "Fund";
            colFund.Header.VisiblePosition = 4;

            UltraGridColumn colClosedQty = band.Columns[COL_ClosedQty];
            colClosedQty.Header.Caption = "Closed Qty";
            colClosedQty.Header.VisiblePosition = 5;

            UltraGridColumn colOpenQty = band.Columns[COL_OpenQty];
            colOpenQty.Header.Caption = "Open Qty";
            colOpenQty.Header.VisiblePosition = 6;

            UltraGridColumn colRealizedPNL = band.Columns[COL_RealizedPNL];
            colRealizedPNL.Header.Caption = "Realized PNL";
            colRealizedPNL.Header.VisiblePosition = 7;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

    }
}
