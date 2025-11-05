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
    public partial class CtrlCashBalanceManagement : UserControl
    {
        private CashBalanceManagement _cashBalanceSummaryData = new CashBalanceManagement();

        private SortableSearchableList<CashBalanceEntry> _cashBalanceData = new SortableSearchableList<CashBalanceEntry>();

        UltraGridBand _gridBandCashBalanceManagement = null;

        private DataSourceNameID _dataSourceNameID = new DataSourceNameID(0, "Not forwarding");


        #region Grid Column Names


        const string COL_IsSelected = "IsSelected";
        const string COL_DataSourceNameID = "DataSourceNameID";
        const string COL_Fund = "Fund";
        const string COL_Currency = "Currency";
        const string COL_OpeningCash = "OpeningCash";
        const string COL_TradingCashSpent = "TradingCashSpent"; 
        const string COL_TradingCashReceived = "TradingCashReceived";
        const string COL_Transactions = "Transactions";
        const string COL_NetAmount = "NetAmount";
        const string COL_ProjectedClosingCash = "ProjectedClosingCash";
        const string COL_Symbol = "Symbol";
        
        #endregion Grid Column Names


        //internal event EventHandler OpenCashReconDetails;

        public CtrlCashBalanceManagement()
        {
            InitializeComponent();
            
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Under Construction", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //if (OpenCashReconDetails != null)
            //{
            //    OpenCashReconDetails(this, EventArgs.Empty);
            //}

            Forms.CashReconDetails crd = new Nirvana.Admin.PositionManagement.Forms.CashReconDetails(new DataSourceNameID(1,"Goldman Sachs", "GS"));
            crd.ShowDialog();
        }


        /// <summary>
        /// Binds the combo boxes.
        /// </summary>
        public  void InitControl()
        {
            ctrlSourceName1.IsSelectItemRequired = false;
            ctrlSourceName1.IsAllDataSourceAvailable = true;
            ctrlSourceName1.InitControl();            
        }

        /// <summary>
        /// Populates the run upload details.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        public void PopulateCashManagementGridDetails(int dataSourceID)
        {
            _cashBalanceSummaryData = CashBalanceManager.GetCashBalanceDataForDataSource(_dataSourceNameID, Convert.ToDateTime(cmbDate.Value));


            _cashBalanceData = _cashBalanceSummaryData.CashBalanceManagementDataListItems;

              
            grdTrades.DataMember = "CashBalanceManagementDataListItems";
            grdTrades.DataSource = _cashBalanceSummaryData;
            
        }        
      

        bool _isGridInitialized = false;
        
        /// <summary>
        /// Handles the InitializeLayout event of the grdTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdTrades_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            if (bool.Equals(_isGridInitialized, false))
            {

                grdTrades.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdTrades.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdTrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
                grdTrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdTrades.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdTrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdTrades.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdTrades.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;


                _gridBandCashBalanceManagement = grdTrades.DisplayLayout.Bands[0];
                _gridBandCashBalanceManagement.Override.AllowColSwapping = AllowColSwapping.NotAllowed;


                UltraGridColumn ColIsSelected = _gridBandCashBalanceManagement.Columns[COL_IsSelected];                
                //ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                ColIsSelected.Header.Caption = "";
                ColIsSelected.Header.VisiblePosition = 1;

                UltraGridColumn colSymbol = _gridBandCashBalanceManagement.Columns[COL_Symbol];
                colSymbol.Hidden = true;
                //_gridBandCashBalanceManagement.Columns[colSymbol].Hidden = true;
                //colDataSourceNameID.Header.Caption = "Source";
                //colDataSourceNameID.Header.VisiblePosition = 2;

                UltraGridColumn colDataSourceNameID = _gridBandCashBalanceManagement.Columns[COL_DataSourceNameID];
                colDataSourceNameID.Header.Caption = "Source";
                colDataSourceNameID.Header.VisiblePosition = 2;

                
                UltraGridColumn colFund = _gridBandCashBalanceManagement.Columns[COL_Fund];
                colFund.Header.Caption = "Fund";
                colFund.Header.VisiblePosition = 3;

                UltraGridColumn colCurrency = _gridBandCashBalanceManagement.Columns[COL_Currency];
                colCurrency.Header.Caption = "Currency";                        
                colCurrency.Header.VisiblePosition = 4;

                UltraGridColumn colOpeningCash = _gridBandCashBalanceManagement.Columns[COL_OpeningCash];
                colOpeningCash.Header.Caption = "Opening Cash";
                colOpeningCash.Header.VisiblePosition = 5;

                UltraGridColumn colTradingCashSpent = _gridBandCashBalanceManagement.Columns[COL_TradingCashSpent];                
                colTradingCashSpent.Header.Caption = "Trading Cash Spent";
                colTradingCashSpent.Header.VisiblePosition = 6;

                UltraGridColumn colTradingCashReceived = _gridBandCashBalanceManagement.Columns[COL_TradingCashReceived];
                colTradingCashSpent.Header.Caption = "Trading Cash Recieved";
                colTradingCashSpent.Header.VisiblePosition = 7;

                UltraGridColumn colTransactions = _gridBandCashBalanceManagement.Columns[COL_Transactions];
                colTransactions.Header.VisiblePosition = 8;
                colTransactions.Header.Caption = "Transactions";
              
                UltraGridColumn colNetAmount = _gridBandCashBalanceManagement.Columns[COL_NetAmount];
                colNetAmount.Header.Caption = "NetAmount";
                colNetAmount.Header.VisiblePosition = 9;

                UltraGridColumn colProjectedClosingCash = _gridBandCashBalanceManagement.Columns[COL_ProjectedClosingCash];
                colProjectedClosingCash.Header.Caption = "Projected Closing Cash";
                colProjectedClosingCash.Header.VisiblePosition = 10;

                _isGridInitialized = true;
            }
        }

        /// <summary>
        /// TODO : Need to be replaced by some bindingsource change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {
            _dataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;
        }

        void grdTrades_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            //if(string.Equals(e.Cell.Column.Key, 
    
        }

        /// <summary>
        /// Handles the Click event of the btnRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            int dataSourceID = _dataSourceNameID.ID;
            if (!(dataSourceID < 0))
            {
                PopulateCashManagementGridDetails(dataSourceID);
            }
        }
    }
}
