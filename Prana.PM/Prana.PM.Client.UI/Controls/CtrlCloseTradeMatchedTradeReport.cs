using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlCloseTradeMatchedTradeReport : UserControl
    {
        #region Grid Column Names

        const string CAPTION_Symbol = "Symbol";
        const string CAPTION_TradeDate = "TradeDate";
        const string CAPTION_TotalQty = "TotalQty";
        const string CAPTION_ClosedQty = "ClosedQty";
        const string CAPTION_OpenQty = "OpenQty";
        const string CAPTION_Account = "Account";
        const string CAPTION_RealizedPNL = "RealizedPNL";

        #endregion Grid Column Names

        BindingSource _formBindingSource;
        private CloseTradeMatchedTradeReport _closeTradeMatchedTradeReport = new CloseTradeMatchedTradeReport();

        public CtrlCloseTradeMatchedTradeReport()
        {
            _formBindingSource = new BindingSource();
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
            try
            {

                _formBindingSource.DataSource = RetrieveCloseTradeMatchedTradeReport();

                //create a binding object
                Binding reportDateBinding = new System.Windows.Forms.Binding("Value", _formBindingSource, "ReportDate");
                //add new binding
                cmbDate.DataBindings.Add(reportDateBinding);

                grdMatchedTradeReport.DataSource = null;
                grdMatchedTradeReport.DataMember = "ReportItems";
                grdMatchedTradeReport.DataSource = _formBindingSource;

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

        private CloseTradeMatchedTradeReport RetrieveCloseTradeMatchedTradeReport()
        {
            //_closeTradeMatchedTradeReport.DataSourceNameID = dataSourceNameID;
            //_closeTradeMatchedTradeReport.MappingItemList = MappingItemList.RetrieveAccountMappings(dataSourceNameID);

            _closeTradeMatchedTradeReport.ReportDate = DateTime.Today;
            _closeTradeMatchedTradeReport.ReportItems = GetReportItems();
            return _closeTradeMatchedTradeReport;
        }

        private SortableSearchableList<CloseTradeMatchedTradeReportItem> GetReportItems()
        {
            SortableSearchableList<CloseTradeMatchedTradeReportItem> list = new SortableSearchableList<CloseTradeMatchedTradeReportItem>();

            CloseTradeMatchedTradeReportItem item = new CloseTradeMatchedTradeReportItem();

            item.Account.ID = 1;
            item.Account.ShortName = "Long";
            item.ClosedQty = -2000;
            item.OpenQty = 98000;
            item.Symbol = "MSFT";
            item.TradeDate = new DateTime(2006, 1, 1);
            item.TotalQty = 100000;
            item.RealizedPNL = 5100;

            list.Add(item);

            item = new CloseTradeMatchedTradeReportItem();

            item.Account.ID = 2;
            item.Account.ShortName = "Short";
            item.ClosedQty = 1000;
            item.OpenQty = 49000;
            item.Symbol = "DELL";
            item.TradeDate = new DateTime(2005, 12, 20);
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

            UltraGridColumn colSymbol = band.Columns[CAPTION_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 1;

            UltraGridColumn colTradeDate = band.Columns[CAPTION_TradeDate];
            colTradeDate.Header.Caption = "Trade Date";
            colTradeDate.Header.VisiblePosition = 2;

            UltraGridColumn colTotalQty = band.Columns[CAPTION_TotalQty];
            colTotalQty.Header.Caption = "Total Qty";
            colTotalQty.Header.VisiblePosition = 3;

            UltraGridColumn colAccount = band.Columns[CAPTION_Account];
            colAccount.Header.Caption = "Account";
            colAccount.Header.VisiblePosition = 4;

            UltraGridColumn colClosedQty = band.Columns[CAPTION_ClosedQty];
            colClosedQty.Header.Caption = "Closed Qty";
            colClosedQty.Header.VisiblePosition = 5;

            UltraGridColumn colOpenQty = band.Columns[CAPTION_OpenQty];
            colOpenQty.Header.Caption = "Open Qty";
            colOpenQty.Header.VisiblePosition = 6;

            UltraGridColumn colRealizedPNL = band.Columns[CAPTION_RealizedPNL];
            colRealizedPNL.Header.Caption = "Realized PNL";
            colRealizedPNL.Header.VisiblePosition = 7;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.FindForm().Close();
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
