using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Infragistics.Win.UltraWinGrid;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCloseTrades : UserControl
    {


        private CloseTradeInterface _closeTradeInterfaceData = new CloseTradeInterface();

        private SortableSearchableList<AllocatedTrades> _AllocatedTradesData = new SortableSearchableList<AllocatedTrades>();
        UltraGridBand _gridBandAllocatedTrades = null;
        bool _isAllocatedTradesGridInitialized;
        #region Allocated Trades Grid columns

        const string COL_Side = "Side";
        const string COL_AllocationID = "ID";
        const string COL_Symbol = "Symbol";
        const string COL_ExecutedQuantity = "Quantity";
        const string COL_FundValue = "FundValue";
        const string COL_AveragePrice = "AveragePrice";       
        #endregion

        bool _isPositionsEligibleForClosingGridInitialized;
        UltraGridBand _gridBandPositionsEligibleForClosing = null;
        private SortableSearchableList<PositionEligibleForClosing> _positionEligibleForClosing = new SortableSearchableList<PositionEligibleForClosing>();
        #region Additional Grid Columns for Eligible Open Positions

        const string COL_ID = "ID";
        const string COL_ClosingQuantity = "ClosingQuantity";
        const string COL_LastActivityDate = "LastActivityDate";
        const string COL_PositionType = "PositionType";
        const string COL_Quantity = "Quantity";        
       

        #endregion

        Forms.CloseTradeMatchedTradeReport _frmCloseTradeMatchedTradeReport = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlCloseTrades"/> class.
        /// </summary>
        public CtrlCloseTrades()
        {
            InitializeComponent();
            ctrlCloseTradePreferences1.ComboDefaultMethodologyEnabled = false;
        }

        /// <summary>
        /// Handles the InitializeLayout event of the grdTodaysAllocatedTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdTodaysAllocatedTrades_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            if (bool.Equals(_isAllocatedTradesGridInitialized, false))
            {
                grdTodaysAllocatedTrades.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdTodaysAllocatedTrades.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdTodaysAllocatedTrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
                grdTodaysAllocatedTrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdTodaysAllocatedTrades.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdTodaysAllocatedTrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdTodaysAllocatedTrades.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdTodaysAllocatedTrades.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;


                _gridBandAllocatedTrades = grdTodaysAllocatedTrades.DisplayLayout.Bands[0];
                _gridBandAllocatedTrades.Override.AllowColSwapping = AllowColSwapping.NotAllowed;


                UltraGridColumn ColSide = _gridBandAllocatedTrades.Columns[COL_Side];
                //ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                ColSide.Header.Caption = "Side";
                ColSide.Header.VisiblePosition = 1;

                UltraGridColumn colAllocationID = _gridBandAllocatedTrades.Columns[COL_AllocationID];
                colAllocationID.Header.Caption = "ID";
                colAllocationID.Header.VisiblePosition = 2;
                //_gridBandAllocatedTrades.Columns[colSymbol].Hidden = true;
                //colDataSourceNameID.Header.Caption = "Source";
                //colDataSourceNameID.Header.VisiblePosition = 2;

                UltraGridColumn colSymbol = _gridBandAllocatedTrades.Columns[COL_Symbol];
                colSymbol.Header.Caption = "Symbol";
                colSymbol.Header.VisiblePosition = 3;


                UltraGridColumn colExecutedQuantity = _gridBandAllocatedTrades.Columns[COL_ExecutedQuantity];
                colExecutedQuantity.Header.Caption = "Executed Quantity";
                colExecutedQuantity.Header.VisiblePosition = 4;

                UltraGridColumn colFund = _gridBandAllocatedTrades.Columns[COL_FundValue];
                colFund.Header.Caption = "Fund";
                colFund.Header.VisiblePosition = 5;

                UltraGridColumn colAveragePrice = _gridBandAllocatedTrades.Columns[COL_AveragePrice];
                colAveragePrice.Header.Caption = "Average Price";
                colAveragePrice.Header.VisiblePosition = 6;               

                _isAllocatedTradesGridInitialized = true;
            }
        }

        /// <summary>
        /// Handles the InitializeLayout event of the grdEligiblePositions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdEligiblePositions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            if (bool.Equals(_isPositionsEligibleForClosingGridInitialized, false))
            {
                grdEligiblePositions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdEligiblePositions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdEligiblePositions.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
                grdEligiblePositions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdEligiblePositions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdEligiblePositions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdEligiblePositions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdEligiblePositions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;


                _gridBandPositionsEligibleForClosing = grdEligiblePositions.DisplayLayout.Bands[0];
                _gridBandPositionsEligibleForClosing.Override.AllowColSwapping = AllowColSwapping.NotAllowed;


                UltraGridColumn ColID = _gridBandPositionsEligibleForClosing.Columns[COL_ID];
                //ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                ColID.Header.Caption = "ID";
                ColID.Header.VisiblePosition = 1;

                UltraGridColumn colClosingQuantity = _gridBandPositionsEligibleForClosing.Columns[COL_ClosingQuantity];
                colClosingQuantity.Header.Caption = "Closing Quantity";
                colClosingQuantity.Header.VisiblePosition = 2;
                

                UltraGridColumn colTradeDate = _gridBandPositionsEligibleForClosing.Columns[COL_LastActivityDate];
                colTradeDate.Header.Caption = "Trade Date";
                colTradeDate.Header.VisiblePosition = 3;


                UltraGridColumn colPositionType = _gridBandPositionsEligibleForClosing.Columns[COL_PositionType];
                colPositionType.Header.Caption = "Position Type";
                colPositionType.Header.VisiblePosition = 4;

                UltraGridColumn colSymbol = _gridBandPositionsEligibleForClosing.Columns[COL_Symbol];
                colSymbol.Header.Caption = "Symbol";
                colSymbol.Header.VisiblePosition = 5;

                UltraGridColumn colQuantity = _gridBandPositionsEligibleForClosing.Columns[COL_Quantity];
                colQuantity.Header.Caption = "Quantity";
                colQuantity.Header.VisiblePosition = 6; 

                UltraGridColumn colFund = _gridBandPositionsEligibleForClosing.Columns[COL_FundValue];
                colFund.Header.Caption = "Fund";
                colFund.Header.VisiblePosition = 7;

                UltraGridColumn colAveragePrice = _gridBandPositionsEligibleForClosing.Columns[COL_AveragePrice];
                colAveragePrice.Header.VisiblePosition = 8;
                colAveragePrice.Header.Caption = "Cost Basis";               

                _isPositionsEligibleForClosingGridInitialized = true;
            }
        }

        /// <summary>
        /// Populates the close trades interface.
        /// input parameters to be added in some time...
        /// </summary>
        /// <param name="isInternal">if set to <c>true</c> [populates the default initial dummy values].</param>
        public void PopulateCloseTradesInterface(bool isInternal)
        {
            _closeTradeInterfaceData = CloseTradesManager.GetCloseTradeInterfaceData(DateTime.Now, isInternal);

            grdEligiblePositions.DataMember = "PositionsEligibleForClosing";
            grdEligiblePositions.DataSource = _closeTradeInterfaceData;

            grdTodaysAllocatedTrades.DataMember = "AllocatedTradesData";
            grdTodaysAllocatedTrades.DataSource = _closeTradeInterfaceData;

            //Initialize the preferences Control.
            this.ctrlCloseTradePreferences1.InitControl();
        }

        /// <summary>
        /// Handles the Click event of the btnRun control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            PopulateCloseTradesInterface(true);
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        /// <summary>
        /// Handles the Click event of the btnViewReport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnViewReport_Click(object sender, EventArgs e)
        {
            if (_frmCloseTradeMatchedTradeReport == null)
            {
                _frmCloseTradeMatchedTradeReport = new Nirvana.Admin.PositionManagement.Forms.CloseTradeMatchedTradeReport();
            }

            _frmCloseTradeMatchedTradeReport.ShowDialog();
            //_frmCloseTradeMatchedTradeReport.Activate();
            _frmCloseTradeMatchedTradeReport.Disposed += new EventHandler(_frmCloseTradeMatchedTradeReport_Disposed);
        }

        /// <summary>
        /// Handles the Disposed event of the _frmCloseTradeMatchedTradeReport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmCloseTradeMatchedTradeReport_Disposed(object sender, EventArgs e)
        {
            _frmCloseTradeMatchedTradeReport = null;
        }

        

        
    }
}
