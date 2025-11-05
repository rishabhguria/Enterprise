using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.PM.Common;
using Nirvana.PM.BLL;
using Nirvana.ServerClientCommon;
using Csla;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Nirvana.PM.DAL;

namespace Nirvana.PM.Client.UI.Controls
{
    public partial class CtrlCloseTrades : UserControl
    {
        private CloseTradeInterface _closeTradeInterfaceData = new CloseTradeInterface();

        private AllocatedTradesList _allocatedTradeList = new AllocatedTradesList();
        UltraGridBand _gridBandShortPositions = null;
        UltraGridBand _gridBandLongPositions = null;        
        UltraGridBand _grdBandNetPositions = null;
        UltraGridBand _grdBandPositionsTaxlot = null;

        bool _isNetPositionsGridInitialized = false;
        bool _isShortPositionGridInitialized = false;
        bool _isLongPositionGridInitialized = false;

        const string GRIDSHORTPOSITIONNAME = "grdShortPosition";
        const string GRIDLONGPOSITIONNAME = "grdLongPosition";
        const string GRIDNETPOSITIONNAME = "grdNetPosition";


        #region Allocated Trades Grid columns

        const string COL_AllocationID = "ID";
        const string COL_TradeDate = "TradeDate";
        const string COL_Side = "Side";
        const string COL_Symbol = "Symbol";
        const string COL_ExecutedQuantity = "Quantity";
        const string COL_AveragePrice = "AveragePrice";       
        const string COL_FundValue = "FundValue";
        const string COL_SideID = "SideID";
        const string COL_IsPosition = "IsPosition";
        const string COL_AUEC = "AUECID";
        const string COL_PositionID = "PositionID";        
        #endregion

        #region Grid Columns for Net Positions

        const string COL_ID = "ID";
        const string COL_ClosingQuantity = "ClosedQty";
        const string COL_OpenQuantity = "OpenQty";
        const string COL_StartDate = "StartDate";
        const string COL_LastActivityDate = "LastActivityDate";
        const string COL_PositionType = "PositionType";
        const string COL_Quantity = "Quantity";
        const string COL_GeneratedPNL = "GeneratedPNL";
        const string COL_StartTaxLotID = "StartTaxLotID";
        

        #endregion

        //UltraGridBand _gridBandPositionsEligibleForClosing = null;
        //private SortableSearchableList<PositionEligibleForClosing> _positionEligibleForClosing = new SortableSearchableList<PositionEligibleForClosing>();
        

        Forms.CloseTradeMatchedTradeReport _frmCloseTradeMatchedTradeReport = null;


       

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlCloseTrades"/> class.
        /// </summary>
        public CtrlCloseTrades()
        {
            InitializeComponent();

            //ctrlCloseTradePreferences1.InitControl();
            ctrlCloseTradePreferences1.SelectedAUECListChanged += new MethodInvoker(ctrlCloseTradePreferences1_SelectedAUECListChanged);
            ctrlCloseTradePreferences1.SelectedFundListChanged += new MethodInvoker(ctrlCloseTradePreferences1_SelectedFundListChanged);
            ctrlCloseTradePreferences1.SelectedCloseMethodologyChanged += new MethodInvoker(ctrlCloseTradePreferences1_SelectedCloseMethodologyChanged);
            ctrlCloseTradePreferences1.SelectedCloseAlgorithmChanged += new MethodInvoker(ctrlCloseTradePreferences1_SelectedCloseAlgorithmChanged);
            
            ///Databinding of check box list (selected items )not working, hence work arround applied. 
            ///TODO : Need to use these datasource and datamember properties later on.
            //ctrlCloseTradePreferences1.DataSource = _closeTradeInterfaceData;
            //ctrlCloseTradePreferences1.DataMember = _closeTradeInterfaceData.CloseTradeFilter;
 
            //ctrlCloseTradePreferences1.ComboDefaultMethodologyEnabled = false;
        }

        /// <summary>
        /// CTRLs the close trade preferences1_ selected close algorithm changed.
        /// </summary>
        void ctrlCloseTradePreferences1_SelectedCloseAlgorithmChanged()
        {
            _closeTradeInterfaceData.CloseTradeFilter.Algorithm  =  ctrlCloseTradePreferences1.CloseTradePreferences.Algorithm;
        }

        /// <summary>
        /// On manual closing, we don't need the services of run button
        /// </summary>
        void ctrlCloseTradePreferences1_SelectedCloseMethodologyChanged()
        {
            _closeTradeInterfaceData.CloseTradeFilter.DefaultMethodology = ctrlCloseTradePreferences1.CloseTradePreferences.DefaultMethodology;
            if (_closeTradeInterfaceData.CloseTradeFilter.DefaultMethodology == CloseTradeMethodology.Manual)
            {
                btnRun.Enabled = false;
                grdLongPosition.AllowDrop = true;
                grdShortPosition.AllowDrop = true;
            }
            else
            {
                btnRun.Enabled = true;
                //grdLongPosition.Rows.dis
                //grdShortPosition.Enabled = false;
                //grdLongPosition.Enabled = false;
                grdLongPosition.AllowDrop = false;
                grdShortPosition.AllowDrop = false;
            }
        }

        /// <summary>
        /// CTRLs the close trade preferences1_ selected fund list changed.
        /// </summary>
        void ctrlCloseTradePreferences1_SelectedFundListChanged()
        {
            _closeTradeInterfaceData.CloseTradeFilter.Funds = ctrlCloseTradePreferences1.CloseTradePreferences.Funds;
        }

        /// <summary>
        /// CTRLs the closetradepreferences1_selectedAUEClistchanged event.
        /// </summary>
        void ctrlCloseTradePreferences1_SelectedAUECListChanged()
        {
            _closeTradeInterfaceData.CloseTradeFilter.AUECListValues = ctrlCloseTradePreferences1.CloseTradePreferences.AUECListValues ;
        }

        #region Net position grid events
        /// <summary>
        /// Handles the InitializeLayout event of the grdNetPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdNetPosition_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

            if (bool.Equals(_isNetPositionsGridInitialized, false))
            {
                UltraGridLayout gridLayout = grdNetPosition.DisplayLayout;
                SetGridAppearanceAndLayout(ref gridLayout);

                grdLongPosition.DisplayLayout.Override.RowAppearance.BackColor = Color.Gray;
                // deactivate the grid - so that is cannot be edited
                grdNetPosition.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                _grdBandNetPositions = grdNetPosition.DisplayLayout.Bands[0];

                _grdBandNetPositions.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                UltraGridColumn colAllocationID = _grdBandNetPositions.Columns[COL_ID];
                colAllocationID.Header.Caption = "Position ID";
                colAllocationID.Header.VisiblePosition = 1;

                UltraGridColumn colStartTaxLotID = _grdBandNetPositions.Columns[COL_StartTaxLotID];
                colStartTaxLotID.Header.Caption = "Starting TaxLot ID";
                colStartTaxLotID.Header.VisiblePosition = 2;
                

                UltraGridColumn colFund = _grdBandNetPositions.Columns[COL_FundValue];
                colFund.Header.Caption = "Fund";
                colFund.Header.VisiblePosition = 3;

                UltraGridColumn colTradeDate = _grdBandNetPositions.Columns[COL_StartDate];
                colTradeDate.Header.Caption = "Trade Date";
                colTradeDate.Header.VisiblePosition = 4;

                //UltraGridColumn ColSide = _grdBandNetPositions.Columns[COL_Side];
                ////ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //ColSide.Header.Caption = "Side";
                //ColSide.Header.VisiblePosition = 4;


                UltraGridColumn colSymbol = _grdBandNetPositions.Columns[COL_Symbol];
                colSymbol.Header.Caption = "Symbol";
                colSymbol.Header.VisiblePosition = 5;

                UltraGridColumn colClosingQuantity = _grdBandNetPositions.Columns[COL_ClosingQuantity];
                colClosingQuantity.Header.Caption = "Closing Quantity";
                colClosingQuantity.Header.VisiblePosition = 6;


                UltraGridColumn colOpenQuantity = _grdBandNetPositions.Columns[COL_OpenQuantity];
                colOpenQuantity.Header.Caption = "Open Quantity";
                colOpenQuantity.Header.VisiblePosition = 7;

                UltraGridColumn colAveragePrice = _grdBandNetPositions.Columns[COL_AveragePrice];
                colAveragePrice.Header.Caption = "Average Price";
                colAveragePrice.Header.VisiblePosition = 8;

                UltraGridColumn colGeneratedPNL = _grdBandNetPositions.Columns[COL_GeneratedPNL];
                colGeneratedPNL.Header.Caption = "Generated PNL";
                colGeneratedPNL.Header.VisiblePosition = 9;

                #region Child Band's - PositionTaxLots

                 _grdBandPositionsTaxlot = grdNetPosition.DisplayLayout.Bands[1];
                _grdBandPositionsTaxlot.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                UltraGridColumn colIDTaxlot = _grdBandPositionsTaxlot.Columns[COL_ID];
                colIDTaxlot.Header.Caption = "Taxlot ID";
                colIDTaxlot.Header.VisiblePosition = 1;

                UltraGridColumn colPositionID = _grdBandPositionsTaxlot.Columns[COL_PositionID];
                colPositionID.Width = 40;
                colPositionID.Header.Caption = "Parent Position ID";
                colPositionID.Header.VisiblePosition = 2;
                
                UltraGridColumn colFundValueTaxlot = _grdBandPositionsTaxlot.Columns[COL_FundValue];
                colFundValueTaxlot.Header.Caption = "Fund";
                colFundValueTaxlot.Header.VisiblePosition = 3;

                UltraGridColumn colTradeDateTaxlot = _grdBandPositionsTaxlot.Columns[COL_TradeDate];
                colTradeDateTaxlot.Header.Caption = "Trade Date";
                colTradeDateTaxlot.Header.VisiblePosition = 4;

                UltraGridColumn colSideTaxlot = _grdBandPositionsTaxlot.Columns[COL_Side];
                colSideTaxlot.Header.Caption = "Side";
                colSideTaxlot.Header.VisiblePosition = 5;

                UltraGridColumn colSymbolTaxlot = _grdBandPositionsTaxlot.Columns[COL_Symbol];
                colSymbolTaxlot.Header.Caption = "Symbol";
                colSymbolTaxlot.Header.VisiblePosition = 6;

                UltraGridColumn colQuantityTaxlot = _grdBandPositionsTaxlot.Columns[COL_Quantity];
                colQuantityTaxlot.Header.Caption = "Quantity";
                colQuantityTaxlot.Header.VisiblePosition = 7;

                UltraGridColumn colAveragePriceTaxlot = _grdBandPositionsTaxlot.Columns[COL_AveragePrice];
                colAveragePriceTaxlot.Header.Caption = "Average Price";
                colAveragePriceTaxlot.Header.VisiblePosition = 8;

                UltraGridColumn colSideIDTaxlot = _grdBandPositionsTaxlot.Columns[COL_SideID];
                colSideIDTaxlot.Hidden = true;

                UltraGridColumn colAUECTaxlot = _grdBandPositionsTaxlot.Columns[COL_AUEC];
                colAUECTaxlot.Hidden = true;

                UltraGridColumn colIsPosition = _grdBandPositionsTaxlot.Columns[COL_IsPosition];
                colIsPosition.Hidden = true;

                

	            #endregion

                //UltraGridColumn colAUEC = _grdBandNetPositions.Columns[COL_AUEC];
                //colAUEC.Hidden = true;


                //UltraGridColumn ColSideID = _grdBandNetPositions.Columns[COL_SideID];
                //ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //ColSideID.Hidden = true;

                _isNetPositionsGridInitialized = true;
            }
        } 
        #endregion

        #region Private Declarations
        private bool c_blnGridLongLeftMousedown = false;
        private bool c_blnGridShortLeftMousedown = false;
        #endregion

        #region Short position grid events
        /// <summary>
        /// Handles the InitializeLayout event of the grdEligiblePositions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdShortPosition_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            if (bool.Equals(_isShortPositionGridInitialized, false))
            {

                UltraGridLayout gridLayout = grdShortPosition.DisplayLayout;
                SetGridAppearanceAndLayout(ref gridLayout);
                gridLayout.Override.RowAppearance.BackColor = Color.FromArgb(64, 64, 64);
                //grdShortPosition.DisplayLayout.Override.RowAlternateAppearance.BackColor = 
                
                GridInitializeLayout(sender, e);

                _gridBandShortPositions = grdShortPosition.DisplayLayout.Bands[0];
                SetGridColumns(_gridBandShortPositions);
                _isShortPositionGridInitialized = true;
            }
        }

        /// <summary>
        /// Sets the grid appearance and layout.
        /// </summary>
        /// <param name="grid">The grid.</param>
        private void SetGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
        {
            gridLayout.Appearance.BackColor = Color.Black;
            //gridLayout.Override.RowAppearance.BackColor = Color.Black;
            //gridLayout.Override.RowAlternateAppearance.BackColor = Color.FromArgb(64, 64, 64);           

            //gridLayout.Override.SelectedRowAppearance.BackColor = Color.Transparent;
            gridLayout.Override.SelectedRowAppearance.BorderColor = Color.White;
            gridLayout.Override.SelectedCellAppearance.BackColor = Color.Transparent;
            gridLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.Default;
            gridLayout.Override.ActiveRowAppearance.BackColor = Color.Transparent;
            gridLayout.Override.ActiveCellAppearance.BackColor = Color.Gold;
            //gridLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
            gridLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            gridLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
            gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;

            gridLayout.AutoFitStyle = AutoFitStyle.None;
            gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            gridLayout.Override.AllowAddNew = AllowAddNew.Yes;
            gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

            gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
            gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;

            
        }

        /// <summary>
        /// Handles the MouseDown event of the grdShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdShortPosition_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            GridMouseDown(sender, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the grdShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdShortPosition_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            c_blnGridShortLeftMousedown = false;
            GridMouseUp(sender);
        }

        /// <summary>
        /// Handles the MouseMove event of the grdShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdShortPosition_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UIElement elementEntered = this.grdShortPosition.DisplayLayout.UIElement.LastElementEntered;

            if (elementEntered == null) return;

            if ((elementEntered is RowUIElement) || (elementEntered.GetAncestor(typeof(RowUIElement)) != null))
                GridMouseMove(sender, e);
        }

        /// <summary>
        /// Handles the DragOver event of the grdShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdShortPosition_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            GridDragOver(sender, e);
        }

        /// <summary>
        /// Handles the DragEnter event of the grdShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdShortPosition_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            GridDragEnter(sender, e);
        }

        /// <summary>
        /// Handles the DragDrop event of the grdShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdShortPosition_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            GridDragDrop(sender, e);
        }



        /// <summary>
        /// Handles the InitializeRow event of the grdShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdShortPosition_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            SetGridsRowAppearance(e, GRIDSHORTPOSITIONNAME);         
            
        }
        #endregion

        #region Long position grid events
        /// <summary>
        /// Handles the InitializeLayout event of the grdTodaysAllocatedTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdLongPosition_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            if (bool.Equals(_isLongPositionGridInitialized, false))
            {
                UltraGridLayout gridLayout = grdLongPosition.DisplayLayout;
                SetGridAppearanceAndLayout(ref gridLayout);


                grdLongPosition.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                //grdLongPosition.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.FromArgb(64, 64, 64);

                GridInitializeLayout(sender, e);

                _gridBandLongPositions = grdLongPosition.DisplayLayout.Bands[0];
                SetGridColumns(_gridBandLongPositions);
                _isLongPositionGridInitialized = true;
            }
        }

        /// <summary>
        /// Handles the DragOver event of the grdLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdLongPosition_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            GridDragOver(sender, e);
        }

        /// <summary>
        /// Handles the DragEnter event of the grdLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdLongPosition_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            GridDragEnter(sender, e);
        }

        /// <summary>
        /// Handles the DragDrop event of the grdLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdLongPosition_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            GridDragDrop(sender, e);
        }

        /// <summary>
        /// Handles the MouseMove event of the grdLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdLongPosition_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UIElement elementEntered = this.grdLongPosition.DisplayLayout.UIElement.LastElementEntered;

            if (elementEntered == null) return;

            if ((elementEntered is RowUIElement) || (elementEntered.GetAncestor(typeof(RowUIElement)) != null))
                GridMouseMove(sender, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the grdLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdLongPosition_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            c_blnGridLongLeftMousedown = false;
            GridMouseUp(sender);
        }

        /// <summary>
        /// Handles the MouseDown event of the grdLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdLongPosition_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            GridMouseDown(sender, e);
        }

        /// <summary>
        /// Handles the InitializeRow event of the grdLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdLongPosition_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            SetGridsRowAppearance(e, GRIDLONGPOSITIONNAME);  
        }

        
        #endregion

        #region Private Methods

        /// <summary>
        /// Grids the initialize layout.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void GridInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //retrieve reference to grid
            UltraGrid ug = (UltraGrid)sender;

            //configure grid
            e.Layout.Override.SelectTypeCell = SelectType.Extended;
            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;

            ug.AllowDrop = true;
        }

        /// <summary>
        /// mouse down event of the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void GridMouseDown(object sender, MouseEventArgs e)
        {
            UltraGrid ug = sender as UltraGrid;
            if (ug == null)
            {
                return;
            }

            switch (ug.Name)
            {
                case GRIDSHORTPOSITIONNAME:
                    //if left mouse down, set property
                    if (e.Button == MouseButtons.Left)
                        c_blnGridShortLeftMousedown = true;
                    break;

                case GRIDLONGPOSITIONNAME :
                    //if left mouse down, set property
                    if (e.Button == MouseButtons.Left)
                        c_blnGridLongLeftMousedown = true;
                    break;
            }
        
        }

        /// <summary>
        /// mouse move event of the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void GridMouseMove(object sender, MouseEventArgs e)
        {
            UltraGrid ug = sender as UltraGrid;

            if (e.Button == MouseButtons.Left)
            {
                switch (ug.Name)
                {
                    case GRIDSHORTPOSITIONNAME:
                        //if left mouse down and mouse move then do drag drop
                        if (c_blnGridShortLeftMousedown == true)
                            if (GetSelectedData(this.grdLongPosition) != null)
                            {
                                this.grdShortPosition.DoDragDrop(GetSelectedData(this.grdShortPosition), DragDropEffects.Copy);
                            }
                        break;
                    case GRIDLONGPOSITIONNAME:
                        //if left mouse down and mouse move then do drag drop
                        if (c_blnGridLongLeftMousedown == true)
                            if (GetSelectedData(this.grdLongPosition) != null)
                            {
                                this.grdLongPosition.DoDragDrop(GetSelectedData(this.grdLongPosition), DragDropEffects.Copy);
                            }
                        break;
                }
           
            }
            
        }

        /// <summary>
        /// mouse up event on the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void GridMouseUp(object sender)
        {
            UltraGrid ug = sender as UltraGrid;

            switch (ug.Name)
            {
                case GRIDSHORTPOSITIONNAME:
                    //turn off mouse down property
                    c_blnGridShortLeftMousedown = false;
                    break;
                case GRIDLONGPOSITIONNAME:
                    //turn off mouse down property
                    c_blnGridLongLeftMousedown = false;
                    break;
            }
            
        }

        /// <summary>
        /// drag enter event on the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void GridDragEnter(object sender, DragEventArgs e)
        {
            //on drag enter, turn on copy drag drop effect
            DataObject doDrop = (DataObject)e.Data;
            if (doDrop.GetDataPresent(typeof(AllocatedTrade))== true)
                e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// drag over on the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void GridDragOver(object sender, DragEventArgs e)
        {
            //on drag over, turn on copy drag drop effect if over a cell

            //get reference to grid
            UltraGrid ug = (UltraGrid)sender;

            //retirieve drag drop data
            DataObject doDrop = (DataObject)e.Data;

            //only do this if there is CSV data
            if (doDrop.GetDataPresent(typeof(AllocatedTrade)) == true)
            {
                //retrieve reference to cell under mouse pointer
                UIElement uieleMouseOver = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(new Point(e.X, e.Y)));
                //UltraGridCell cellMouseOver = (UltraGridCell)uieleMouseOver.GetContext(typeof(UltraGridCell));
                ///Need to fetch for row
                UltraGridRow rowMouseOver = uieleMouseOver.GetContext(typeof(UltraGridRow)) as UltraGridRow;

                if (rowMouseOver != null)
                {
                    //Console.WriteLine(rowMouseOver.Column.Key);
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// This method initiate the postion closing manually on dropping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                //retrieve reference to grid
                UltraGrid ug = (UltraGrid)sender;

                //retrieve reference to cell
                UIElement uieleMouseOver = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(new Point(e.X, e.Y)));
                UltraGridRow rowMouseOver = uieleMouseOver.GetContext(typeof(UltraGridRow)) as UltraGridRow;
                AllocatedTrade draggedAllocatedTrade = null;
                if (rowMouseOver != null)
                {
                    AllocatedTrade targetAllocatedTrade = rowMouseOver.ListObject as AllocatedTrade;
                    //retrieve data
                    DataObject doClipboard = (DataObject)e.Data;

                    //print all available clipboard formats
                    //string[] arrayOfFormats = doClipboard.GetFormats(true);
                    //for (int i = 0; i < arrayOfFormats.Length; i++)
                    //{
                    //    Console.WriteLine(arrayOfFormats[i]);
                    //}

                    //test for CSV data available
                    if (doClipboard.GetDataPresent(typeof(AllocatedTrade)) == true)
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();

                        using (MemoryStream msClipBoard = (MemoryStream)doClipboard.GetData(typeof(AllocatedTrade))) //new MemoryStream(bytes,0,Convert.ToInt32(ms.Length)))
                        {
                            msClipBoard.Position = 0;
                            draggedAllocatedTrade = binaryFormatter.Deserialize(msClipBoard) as AllocatedTrade;
                        }

                        if (targetAllocatedTrade != null && draggedAllocatedTrade != null)
	                    {
                            //long draggedTaxLotID = draggedAllocatedTrade.ID;
                            //Getting the actual reference which is binded in the grid.
                            int indexOfdraggedTaxLotID = _closeTradeInterfaceData.AllocatedTrades.IndexOf(draggedAllocatedTrade);
                            draggedAllocatedTrade = _closeTradeInterfaceData.AllocatedTrades[indexOfdraggedTaxLotID];
		                    ClosePosition(targetAllocatedTrade, draggedAllocatedTrade);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = string.Empty;
            }
        }

        #region Close Position Business Logic
        bool _isSameFund = true;
        bool _isSameSymbol = true;
        bool _isShortWithBuyAndCover = true;
        bool _isShortWithCoverOnly = false;


        /// <summary>
        /// Closes the position.
        /// </summary>
        /// <param name="targetAllocatedTrade">The target allocated trade.</param>
        /// <param name="draggedAllocatedTrade">The dragged allocated trade.</param>
        private void ClosePosition(AllocatedTrade targetAllocatedTrade, AllocatedTrade draggedAllocatedTrade)
        {
            string rulesCheckString = CheckCloseRules(targetAllocatedTrade, draggedAllocatedTrade) ;
            if (rulesCheckString != string.Empty)
            {
                MessageBox.Show(rulesCheckString, "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            switch (targetAllocatedTrade.SideID)
            {
                case FIXConstants.SIDE_Buy:

                    if (draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_SellShort))
                    {
                        CloseTaxlot(draggedAllocatedTrade, targetAllocatedTrade, PositionType.Short);
                    }
                    else if (draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Sell))
                    {
                        CloseTaxlot(targetAllocatedTrade, draggedAllocatedTrade, PositionType.Long);
                    }

                    break;

                case FIXConstants.SIDE_Buy_Closed:
                    ///Only short sell allowed
                    CloseTaxlot(draggedAllocatedTrade, targetAllocatedTrade, PositionType.Short);
                    break;

                case FIXConstants.SIDE_SellShort:
                    if (draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy))
                    {
                        CloseTaxlot(targetAllocatedTrade, draggedAllocatedTrade, PositionType.Short);
                    }
                    else if (draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy_Closed))
                    {
                        CloseTaxlot(targetAllocatedTrade, draggedAllocatedTrade, PositionType.Short);
                    }
                    break;

                case FIXConstants.SIDE_Sell:
                    ///Only buy can be dragged here
                    CloseTaxlot(draggedAllocatedTrade, targetAllocatedTrade, PositionType.Long);
                    break;




                //    switch (draggedAllocatedTrade.SideID)
                //    {
                //        case FIXConstants.SIDE_Sell:

                //            if (targetAllocatedTrade.Quantity > draggedAllocatedTrade.Quantity)
                //            {
                //                targetAllocatedTrade.Quantity -= draggedAllocatedTrade.Quantity;
                //                ///Remove the row from close trade respective grid
                //                _closeTradeInterfaceData.AllocatedTrades.Remove(draggedAllocatedTrade);

                //                /// Generated PNL = (Sell price - purchase price)
                //                currentPosition.GeneratedPNL += (draggedAllocatedTrade.AveragePrice - targetAllocatedTrade.AveragePrice) * draggedAllocatedTrade.Quantity;
                //                currentPosition.ClosedQty += draggedAllocatedTrade.Quantity;
                //                currentPosition.OpenQty = targetAllocatedTrade.Quantity;

                //                ///TODO : does position has side, if not then remove
                //                //currentPosition.Side = 

                //                //newPosition.
                //                currentPosition.PositionTaxLots.Add(draggedAllocatedTrade);
                //            }
                //            else if (targetAllocatedTrade.Quantity < draggedAllocatedTrade.Quantity)
                //            {
                //                ///Get the reference of the same row's object and then update. 
                //                ///Changes in draggedAllocatedTrade will not reflect on the grid as this is a clone picked from 
                //                ///memorystream. We need to find the real AllocatedTrade reference which is binded to the grid
                //                AllocatedTrade draggedAllocatedTradeOriginal = _closeTradeInterfaceData.AllocatedTrades[draggedAllocatedTrade.ID];

                //                draggedAllocatedTradeOriginal.Quantity -= targetAllocatedTrade.Quantity;
                //                ///Remove the row from close trade respective grid
                //                _closeTradeInterfaceData.AllocatedTrades.Remove(targetAllocatedTrade);

                //                /// Generated PNL = (Sell price - purchase price)
                //                currentPosition.GeneratedPNL += (draggedAllocatedTrade.AveragePrice - targetAllocatedTrade.AveragePrice) * targetAllocatedTrade.Quantity;
                //                currentPosition.ClosedQty += targetAllocatedTrade.Quantity;
                //                currentPosition.OpenQty = 0;

                //                draggedAllocatedTrade.Quantity = targetAllocatedTrade.Quantity;
                //                currentPosition.PositionTaxLots.Add(draggedAllocatedTrade);

                //            }
                //            else
                //            {
                //                //Remove both of the rows from both of the grids
                //                _closeTradeInterfaceData.AllocatedTrades.Remove(draggedAllocatedTrade);
                //                _closeTradeInterfaceData.AllocatedTrades.Remove(targetAllocatedTrade);

                //                currentPosition.GeneratedPNL += (draggedAllocatedTrade.AveragePrice - targetAllocatedTrade.AveragePrice) * targetAllocatedTrade.Quantity;
                //                currentPosition.ClosedQty += targetAllocatedTrade.Quantity;
                //                currentPosition.OpenQty = 0;
                //                //newPosition.
                //                currentPosition.PositionTaxLots.Add(draggedAllocatedTrade);

                //                //currentPosition.ClosedQty = targetAllocatedTrade.Quantity;
                //                //currentPosition.OpenQty = draggedAllocatedTrade.Quantity;
                //            }
                //            break;

                //    }
                //    break;

                //case FIXConstants.SIDE_Sell:
                //    break;

                //case FIXConstants.SIDE_SellShort:
                //    break;

                //default:
                //    break;
            }


        }

        /// <summary>
        /// Checks the close rules and returns the error string or empty string. If empty string is returned
        /// then taxlots are successfully closed
        /// </summary>
        /// <param name="targetAllocatedTrade">The target allocated trade.</param>
        /// <param name="draggedAllocatedTrade">The dragged allocated trade.</param>
        /// <returns></returns>
        private string CheckCloseRules(AllocatedTrade targetAllocatedTrade, AllocatedTrade draggedAllocatedTrade)
        {
            ///TODO : Need to integrate with some rules engine.

            if (_isSameFund)
            {
                //Same fund rule
                if (targetAllocatedTrade.FundValue.ID != draggedAllocatedTrade.FundValue.ID)
                {
                    //MessageBox.Show("Taxlot/Position pair to close must belong to the same fund.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return "Taxlot/Position pair to close must belong to the same fund."; 
                }
            }

            if (_isSameSymbol)
            {
                //Same symbol rule
                if (targetAllocatedTrade.Symbol.ToUpper().Trim() != draggedAllocatedTrade.Symbol.ToUpper().Trim())
                {
                    //MessageBox.Show("Taxlot/Position pair to close must belong to same symbol.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return "Taxlot/Position pair to close must belong to same symbol.";
                }
            }

            ///Side specific rules

            //Opposite side rule
            if (targetAllocatedTrade.SideID.Equals(draggedAllocatedTrade.SideID))
            {
                //MessageBox.Show("Taxlot/Position pair to close must belong to opposite sides.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "Taxlot/Position pair to close must belong to opposite sides.";
            }

            ///Buy to cover and Buy cannot be closed
            if ((targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy) && draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy_Closed)) || (targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy_Closed) && draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy)))
            {
                //MessageBox.Show("Taxlot/Position pair to close must belong to opposite sides.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "Taxlot/Position pair to close must belong to opposite sides.";
            }

            ///Sell and SellShort cannot be closed
            if ((targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_SellShort) && draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Sell)) || (targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Sell) && draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_SellShort)))
            {
                //MessageBox.Show("Taxlot/Position pair to close must belong to opposite sides.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "Taxlot/Position pair to close must belong to opposite sides.";
            }

            ///Buy to Cover and Sell can not be closed
            if ((targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy_Closed) && draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Sell)) || (targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Sell) && draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_Buy_Closed)))
            {
                //MessageBox.Show("Taxlot/Position pair to close must belong to opposite sides.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "Taxlot/Position pair to close must belong to opposite sides.";
            }

            if (_isShortWithBuyAndCover)
            {
                if (targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_SellShort))
                {
                    switch (draggedAllocatedTrade.SideID)
                    {
                        case FIXConstants.SIDE_Buy: break;
                        case FIXConstants.SIDE_Buy_Closed: break;
                        default:
                            //MessageBox.Show("Sell Short can only be closed with 'Buy' and 'Buy To Close'.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return "Sell Short can only be closed with 'Buy' and 'Buy To Close'.";
                    }
                }
                else if (draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_SellShort))
                {
                    switch (targetAllocatedTrade.SideID)
                    {
                        case FIXConstants.SIDE_Buy: break;
                        case FIXConstants.SIDE_Buy_Closed: break;
                        case FIXConstants.SIDE_Sell:
                        default:
                            //MessageBox.Show("Sell Short can only be closed with 'Buy' and 'Buy To Close'.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return "Sell Short can only be closed with 'Buy' and 'Buy To Close'.";

                    }
                }
            }

            if (_isShortWithCoverOnly)
            {
                if (targetAllocatedTrade.SideID.Equals(FIXConstants.SIDE_SellShort))
                {
                    switch (draggedAllocatedTrade.SideID)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Sell:
                        default:
                            //MessageBox.Show("Sell Short can only be closed with 'Buy To Close'.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return "Sell Short can only be closed with 'Buy To Close'.";
                    }
                }
                else if (draggedAllocatedTrade.SideID.Equals(FIXConstants.SIDE_SellShort))
                {
                    switch (targetAllocatedTrade.SideID)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Sell:
                        default:
                            //MessageBox.Show("Sell Short can only be closed with 'Buy To Close'.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return "Sell Short can only be closed with 'Buy To Close'.";
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Closes the taxlot.
        /// </summary>
        /// <param name="positionalTaxLot">The positional tax lot. The TaxLot/Position on which the closing will be done.</param>
        /// <param name="taxLotToClose">The TaxLot, which will close the position</param>
        /// <param name="positionType">Type of the position(Long, or Short)</param>
        private void CloseTaxlot(AllocatedTrade positionalTaxLot, AllocatedTrade taxLotToClose, PositionType positionType)
        {
            Position currentPosition = null;
            AllocatedTrade taxLotToCloseClone = null;

            long closingQuantity ;
            long taxLotToCloseInitialQuantity ;

            if (positionalTaxLot.IsPosition)
            {
                Guid positionId = positionalTaxLot.PositionID;
                currentPosition = _closeTradeInterfaceData.NetPositions[positionId];
                if (currentPosition == null)
                {
                    MessageBox.Show("Error while fetching the earlier position details.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                currentPosition = new Position();
                currentPosition.ID = Guid.NewGuid();

                currentPosition.StartTaxLotID = positionalTaxLot.ID;
                currentPosition.PositionStartQuantity = positionalTaxLot.Quantity;

                currentPosition.PositionType = positionType;
                currentPosition.AUECID = positionalTaxLot.AUECID;
                currentPosition.StartDate = DateTime.Now;
                currentPosition.Symbol = positionalTaxLot.Symbol;
                currentPosition.FundValue = positionalTaxLot.FundValue;
                ///Do we need to change the average price
                currentPosition.AveragePrice = positionalTaxLot.AveragePrice;

                ///Mark tax lot as a position
                positionalTaxLot.IsPosition = true;
                
                positionalTaxLot.PositionID = currentPosition.ID;

               // Position currentPositionClonedItem = currentPosition.Clone();
                _closeTradeInterfaceData.NetPositions.Add(currentPosition);
                
            }

            currentPosition.ModifiedAt = DateTime.Now;
            //put user id, who has logged in 
            //currentPosition.ModifiedBy = 

            if (positionalTaxLot.Quantity > taxLotToClose.Quantity)
            {
                positionalTaxLot.Quantity -= taxLotToClose.Quantity;               
                
                taxLotToClose.PositionID = currentPosition.ID;

                

                /// Generated PNL = (Sell price - purchase price)
                currentPosition.GeneratedPNL += (taxLotToClose.AveragePrice - positionalTaxLot.AveragePrice) * taxLotToClose.Quantity;
                currentPosition.ClosedQty += taxLotToClose.Quantity;
                currentPosition.OpenQty = positionalTaxLot.Quantity;

                ///TODO : does position has side, if not then remove
                //currentPosition.Side = 


                //newPosition.
                //taxLotToCloseClone =                 
                taxLotToCloseClone = taxLotToClose.Clone();
                //taxLotToCloseClone.ID = taxLotToClose.ID;
                currentPosition.PositionTaxLots.Add(taxLotToCloseClone);                

                ///Remove the row from close trade respective grid
                _closeTradeInterfaceData.AllocatedTrades.Remove(taxLotToClose);
                
            }
            else if (positionalTaxLot.Quantity < taxLotToClose.Quantity)
            {
                ///Get the reference of the same row's object and then update. 
                ///Changes in draggedAllocatedTrade will not reflect on the grid as this is a clone picked from 
                ///memorystream. We need to find the real AllocatedTrade reference which is binded to the grid
               // AllocatedTrade dragged6AllocatedTradeOriginal = _closeTradeInterfaceData.AllocatedTrades[taxLotToClose.ID];

               // draggedAllocatedTradeOriginal.Quantity -= positionalTaxLot.Quantity;

                // this is the quantity which will be going into the position. that is, the position that is getting closed's total
                // quantity.
                closingQuantity = positionalTaxLot.Quantity;
                taxLotToCloseInitialQuantity = taxLotToClose.Quantity;
                /// Generated PNL = (Sell price - purchase price)
                currentPosition.GeneratedPNL += (taxLotToClose.AveragePrice - positionalTaxLot.AveragePrice) * positionalTaxLot.Quantity;
                currentPosition.ClosedQty += positionalTaxLot.Quantity;
                currentPosition.OpenQty = 0;

                
                taxLotToClose.PositionID = positionalTaxLot.PositionID;

                // setting the closing taxlot's quantity equal to positions total quantity, for the quantity of the 
                //closing tax lot be right.
                taxLotToClose.Quantity = closingQuantity;
                taxLotToCloseClone = taxLotToClose.Clone();                
                //taxLotToCloseClone.ID = taxLotToClose.ID;
                currentPosition.PositionTaxLots.Add(taxLotToCloseClone);
                //After Cloning setting the taxLottoClose's net quantity to (original - closing)
                taxLotToClose.Quantity = taxLotToCloseInitialQuantity - closingQuantity;

                positionalTaxLot.Quantity = 0;
                ///Remove the row from close trade respective grid
                _closeTradeInterfaceData.AllocatedTrades.Remove(positionalTaxLot);

            }
            else
            {                

                currentPosition.GeneratedPNL += (taxLotToClose.AveragePrice - positionalTaxLot.AveragePrice) * positionalTaxLot.Quantity;
                currentPosition.ClosedQty += positionalTaxLot.Quantity;
                currentPosition.OpenQty = 0;


                //newPosition.
                taxLotToCloseClone = taxLotToClose.Clone();
                //taxLotToCloseClone.ID = taxLotToClose.ID;
                currentPosition.PositionTaxLots.Add(taxLotToCloseClone);

                //Remove both of the rows from both of the grids
                _closeTradeInterfaceData.AllocatedTrades.Remove(taxLotToClose);
                _closeTradeInterfaceData.AllocatedTrades.Remove(positionalTaxLot);

                //currentPosition.ClosedQty = targetAllocatedTrade.Quantity;
                //currentPosition.OpenQty = draggedAllocatedTrade.Quantity;
            }
        }
        #endregion

        /// <summary>
        /// Gets the selected data.
        /// </summary>
        /// <param name="ugData">The ug data.</param>
        /// <returns></returns>
        private DataObject GetSelectedData(UltraGrid ugData)
        {
            ///If user wants to drag multiple rows, allow him to do so
            if (ugData != null && ugData.Selected.Rows.Count > 0)
            {
                //TODO : Make it selected later on
                UltraGridRow activeRow = ugData.ActiveRow;
                AllocatedTrade selectedAllocatedTrade = activeRow.ListObject as AllocatedTrade;
                if (selectedAllocatedTrade == null)
                {
                    MessageBox.Show("Error in getting the selected data of drag drop.");
                    return null;
                }

                BinaryFormatter binaryFormatter = new BinaryFormatter();
              
                //put memory stream into data object as csv format
                DataObject doClipboard = new DataObject();
                
                ///Need to keep the memorystream open until we retrieve the object on the drop,
                ///hence removed the using statement
                MemoryStream msData = new MemoryStream();
                //using ()
                //{
                    binaryFormatter.Serialize(msData, selectedAllocatedTrade);
                    //msData.Position = 0;
                    doClipboard.SetData(typeof(AllocatedTrade), msData);

                //}

                //put byte array into memory stream
                 //= new MemoryStream(byteData);

                return doClipboard; 
            }

            return null;

            #region commented
            //create a string builder to contain the output CSV text
            //StringBuilder sbCSV = new StringBuilder();

            //if (this.ultraGrid1.Selected.Cells.Count > 0)
            //{
            //    //put cell data into an array for sorting
            //    CellData[] cdCells = new CellData[ultraGrid1.Selected.Cells.Count];
            //    Int64 lngPtr = 0;
            //    Int64 lngLowColIndex = long.MaxValue;
            //    Int64 lngHighColIndex = 0;

            //    //pass and process selected cells collection
            //    foreach (UltraGridCell cellSelected in this.ultraGrid1.Selected.Cells)
            //    {
            //        //set row and column values
            //        cdCells[lngPtr].RowIndex = cellSelected.Row.Index;
            //        cdCells[lngPtr].ColIndex = cellSelected.Column.Index;

            //        //retrieve text from cell based on string type
            //        switch (cellSelected.Column.DataType.ToString())
            //        {
            //            case "System.String":
            //                {
            //                    cdCells[lngPtr].Text = cellSelected.Value.ToString();
            //                    break;
            //                }
            //            case "System.Decimal":
            //                {
            //                    decimal decValue = decimal.Parse(cellSelected.Value.ToString());
            //                    cdCells[lngPtr].Text = decValue.ToString();
            //                    break;
            //                }
            //        }

            //        //update low and high column index
            //        if (cdCells[lngPtr].ColIndex < lngLowColIndex)
            //            lngLowColIndex = cdCells[lngPtr].ColIndex;
            //        if (cdCells[lngPtr].ColIndex > lngHighColIndex)
            //            lngHighColIndex = cdCells[lngPtr].ColIndex;

            //        //increment array pointer
            //        lngPtr++;
            //    }

            //    //display cell values before sorting
            //    int intPtr;
            //    for (intPtr = cdCells.GetLowerBound(0); intPtr <= cdCells.GetUpperBound(0); intPtr++)
            //    {
            //        Console.WriteLine(cdCells[intPtr].RowIndex.ToString() + " " + cdCells[intPtr].ColIndex.ToString() + " " + cdCells[intPtr].Text);
            //    }
            //    Console.WriteLine();

            //    //sort the array on row and cell
            //    Array.Sort(cdCells, this);

            //    //display cell values after sorting
            //    for (intPtr = cdCells.GetLowerBound(0); intPtr <= cdCells.GetUpperBound(0); intPtr++)
            //    {
            //        Console.WriteLine(cdCells[intPtr].RowIndex.ToString() + " " + cdCells[intPtr].ColIndex.ToString() + " " + cdCells[intPtr].Text);
            //    }
            //    Console.WriteLine();

            //    //put into CSV format
            //    long lngCurrentRow = cdCells[0].RowIndex;
            //    long lngCurrentCol = lngLowColIndex;
            //    for (intPtr = cdCells.GetLowerBound(0); intPtr <= cdCells.GetUpperBound(0); intPtr++)
            //    {
            //        //test for row change
            //        if (cdCells[intPtr].RowIndex != lngCurrentRow)
            //        {
            //            sbCSV.Remove(sbCSV.Length - 1, 1); //remove trailing comma
            //            sbCSV.Append(Environment.NewLine);
            //            lngCurrentRow = cdCells[intPtr].RowIndex;
            //            lngCurrentCol = lngLowColIndex;
            //        }

            //        //test for padding needed to this column
            //        if (lngCurrentCol < cdCells[intPtr].ColIndex)
            //        {
            //            Int64 lngPads = cdCells[intPtr].ColIndex - lngCurrentCol;
            //            Int64 lngPtr2;
            //            for (lngPtr2 = 0; lngPtr2 < lngPads; lngPtr2++)
            //            {
            //                lngCurrentCol++;
            //                sbCSV.Append(",");
            //            }
            //        }

            //        //add value for this cell
            //        sbCSV.Append(cdCells[intPtr].Text + ",");
            //        lngCurrentCol++;
            //    }

            //    //add last new line
            //    sbCSV.Remove(sbCSV.Length - 1, 1); //remove trailing comma
            //    sbCSV.Append(Environment.NewLine);
            //}
            //else
            //{
            //    if (this.ultraGrid1.Selected.Rows.Count > 0)
            //    {
            //        foreach (UltraGridRow aRow in this.ultraGrid1.Selected.Rows)
            //        {
            //            foreach (UltraGridCell aCell in aRow.Cells)
            //            {
            //                switch (aCell.Column.DataType.ToString())
            //                {
            //                    case "System.String":
            //                        {
            //                            sbCSV.Append(aCell.Value + ",");
            //                            break;
            //                        }
            //                    case "System.Decimal":
            //                        {
            //                            decimal decValue = decimal.Parse(aCell.Value.ToString());
            //                            sbCSV.Append(decValue.ToString() + ",");
            //                            break;
            //                        }
            //                }
            //            }

            //            sbCSV.Append(Environment.NewLine);
            //        }
            //    }
            //}

            ////add a c-string terminator for those applilcations like Excel that like that kind of thing
            //sbCSV.Append((char)0);
            //Console.WriteLine(sbCSV.ToString());

            ////convert string to byte array
            //Byte[] byteData = Encoding.UTF8.GetBytes(sbCSV.ToString());

            ////put byte array into memory stream
            //MemoryStream msData = new MemoryStream(byteData);

            ////put memory stream into data object as csv format
            //DataObject doClipboard = new DataObject();
            //doClipboard.SetData(DataFormats.CommaSeparatedValue, true, msData);

            //return doClipboard; 
            #endregion
        }
        #endregion

        /// <summary>
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        private void SetGridColumns(UltraGridBand gridBand)
        {
            
            UltraGridColumn colAllocationID = gridBand.Columns[COL_AllocationID];
            colAllocationID.Width = 45;
            colAllocationID.Header.Caption = "ID";
            colAllocationID.Header.VisiblePosition = 1;

            UltraGridColumn colTradeDate = gridBand.Columns[COL_TradeDate];
            colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTime;
            colTradeDate.Width = 140;
            colTradeDate.Header.Caption = "Trade Date";
            colTradeDate.Header.VisiblePosition = 2;

            UltraGridColumn ColSide = gridBand.Columns[COL_Side];
            ColSide.Width = 65;
            //ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ColSide.Header.Caption = "Side";
            ColSide.Header.VisiblePosition = 3;

            UltraGridColumn colSymbol = gridBand.Columns[COL_Symbol];
            colSymbol.Width = 50;
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 4;

            UltraGridColumn colExecutedQuantity = gridBand.Columns[COL_ExecutedQuantity];
            colExecutedQuantity.Width = 65;
            colExecutedQuantity.Header.Caption = "Quantity";
            colExecutedQuantity.Header.VisiblePosition = 5;

            UltraGridColumn colAveragePrice = gridBand.Columns[COL_AveragePrice];
            colAveragePrice.Width = 80;
            colAveragePrice.Header.Caption = "Average Price";
            colAveragePrice.Header.VisiblePosition = 6;

            UltraGridColumn colFund = gridBand.Columns[COL_FundValue];
            colFund.Width = 80;
            colFund.Header.Caption = "Fund";
            colFund.Header.VisiblePosition = 7;

            UltraGridColumn ColSideID = gridBand.Columns[COL_SideID];
            ColSideID.Hidden = true;

            UltraGridColumn colAUEC = gridBand.Columns[COL_AUEC];
            colAUEC.Hidden = true;

            UltraGridColumn colIsPosition = gridBand.Columns[COL_IsPosition];
            colIsPosition.Hidden = true;

            UltraGridColumn colPositionID = gridBand.Columns[COL_PositionID];
            colPositionID.Hidden = true; 
            
            //UltraGridColumn ColIsPosition = gridBand.Columns[COL_IsPosition];
            ////ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ////ColIsPosition.Header.Caption = "Side";
            //ColIsPosition.Hidden = true;
        }

        /// <summary>
        /// Sets the allocated trade grids row appearance.
        /// </summary>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        /// <param name="gridName">Name of the grid.</param>
        private void SetGridsRowAppearance(InitializeRowEventArgs e, string gridName)
        {
            bool isPosition = false;
            string sideID = string.Empty;
            if (e.Row.Cells[COL_IsPosition] != null && e.Row.Cells[COL_SideID] != null)
            {
                isPosition = Convert.ToBoolean(e.Row.Cells[COL_IsPosition].Text);
                sideID = Convert.ToString(e.Row.Cells[COL_SideID].Text);
                if (string.Equals(gridName, GRIDLONGPOSITIONNAME))
                {
                    if (bool.Equals(isPosition, true))
                    {
                        e.Row.Appearance.ForeColor = Color.Green;
                        e.Row.Appearance.BackColor = Color.Lavender;
                        //e.Row.Appearance.BackColor = Color.Gray;
                    }
                    else
                    {
                        e.Row.Appearance.ForeColor = Color.Blue;
                    }
                }
                else
                {
                    if (bool.Equals(isPosition, true) || string.Equals(sideID,FIXConstants.SIDE_SellShort ))
                    {
                        e.Row.Appearance.ForeColor = Color.GreenYellow;
                        e.Row.Appearance.BackColor = Color.Chocolate;
                    }
                    else
                    {
                        e.Row.Appearance.ForeColor = Color.Red;
                    }
                }
            }
        }
                                  
             

        /// <summary>
        /// Populates the close trades interface.
        /// input parameters to be added in some time...
        /// </summary>
        /// <param name="isInternal">if set to <c>true</c> </param>
        public void PopulateCloseTradesInterface()
        {
            try
            {
                _closeTradeInterfaceData.CloseTradeFilter = ctrlCloseTradePreferences1.InitControl();
                //TODO : Hardcoding of filters, take it from filters
                _closeTradeInterfaceData.CloseTradeFilter.CloseTradeDate = DateTime.Now.AddDays(-6.0);
                //_closeTradeInterfaceData.CloseTradeFilter.DataSourceNameID = new DataSourceNameID();
                //_closeTradeInterfaceData.CloseTradeFilter.DataSourceNameID.ID = 1;

                CloseTradesManager.GetPositionsAndAllocatedTradeList(ref _closeTradeInterfaceData);

                
                //_closeTradeInterfaceData.AllocatedTrades = 
                //_closeTradeInterfaceData.NetPositions = CloseTradesManager.

                SetGridDataSources();

                if (int.Equals(_closeTradeInterfaceData.AllocatedTrades.Count, 0) && int.Equals(_closeTradeInterfaceData.NetPositions.Count, 0))
                {
                    throw (new Exception("No data exists for the filters selected."));
                }
                
                //Initialize the preferences Control.
                //this.ctrlCloseTradePreferences1.InitControl();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Nirvana.Global.Common.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the grid data sources.
        /// </summary>
        private void SetGridDataSources()
        {
            grdShortPosition.DataMember = "AllocatedTrades";
            grdShortPosition.DataSource = _closeTradeInterfaceData;

            grdLongPosition.DataMember = "AllocatedTrades";
            grdLongPosition.DataSource = _closeTradeInterfaceData;

            grdNetPosition.DataMember = "NetPositions";
            grdNetPosition.DataSource = _closeTradeInterfaceData;

            SetDefaultFilters();
        }

        /// <summary>
        /// Sets the default filters.
        /// </summary>
        private void SetDefaultFilters()
        {

            try
            {
                grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].LogicalOperator = FilterLogicalOperator.Or;
                grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, Nirvana.ServerClientCommon.FIXConstants.SIDE_Buy);
                grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, Nirvana.ServerClientCommon.FIXConstants.SIDE_Buy_Closed);

                grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].LogicalOperator = FilterLogicalOperator.Or;
                grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, Nirvana.ServerClientCommon.FIXConstants.SIDE_Sell);
                grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, Nirvana.ServerClientCommon.FIXConstants.SIDE_SellShort);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Nirvana.Global.Common.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            //IsPostion = 1 means that it is a position
            //grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_IsPosition].FilterConditions.Add(FilterComparisionOperator.Equals, true);
        }

        /// <summary>
        /// Sets the conditional filters.
        /// </summary>
        private void SetConditionalFilters()
        {
            grdLongPosition.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            grdShortPosition.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();

            SetDefaultFilters();

            #region Fund filtering

            FundList selectedFunds = _closeTradeInterfaceData.CloseTradeFilter.Funds;

            grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].LogicalOperator = FilterLogicalOperator.Or;
            grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].LogicalOperator = FilterLogicalOperator.Or;
            grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].LogicalOperator = FilterLogicalOperator.Or;

            if (selectedFunds.Count > 0)
            {
                foreach (Fund fund in selectedFunds)
                {
                    grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].FilterConditions.Add(FilterComparisionOperator.Equals, fund);
                    grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].FilterConditions.Add(FilterComparisionOperator.Equals, fund);
                    grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].FilterConditions.Add(FilterComparisionOperator.Equals, fund);
                }
            }
            else
            {
                ///Adding fund filters for a void fund, so no row will match this criteria
                grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].FilterConditions.Add(FilterComparisionOperator.Equals, new Fund());
                grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].FilterConditions.Add(FilterComparisionOperator.Equals, new Fund());
                grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_FundValue].FilterConditions.Add(FilterComparisionOperator.Equals, new Fund());
            }
            
            #endregion

            #region AUEC filtering

            AUECList selectedAUECs = _closeTradeInterfaceData.CloseTradeFilter.AUECListValues;
            if (selectedAUECs.Count > 0)
            {
                
                grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_AUEC].LogicalOperator = FilterLogicalOperator.Or;
                grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_AUEC].LogicalOperator = FilterLogicalOperator.Or;
                grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_AUEC].LogicalOperator = FilterLogicalOperator.Or;

                foreach (AUEC auec in selectedAUECs)
                {
                    grdLongPosition.DisplayLayout.Bands[0].ColumnFilters[COL_AUEC].FilterConditions.Add(FilterComparisionOperator.Equals, auec.AUECID);
                    grdShortPosition.DisplayLayout.Bands[0].ColumnFilters[COL_AUEC].FilterConditions.Add(FilterComparisionOperator.Equals, auec.AUECID);
                    grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_AUEC].FilterConditions.Add(FilterComparisionOperator.Equals, auec.AUECID);
                }
            } 
            #endregion
        }

        /// <summary>
        /// Handles the Click event of the btnUpdateFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnUpdateFilter_Click(object sender, System.EventArgs e)
        {
            
            GetFilteredData();
        }

        /// <summary>
        /// Gets the filtered data.
        /// </summary>
        private void GetFilteredData()
        {
            try
            {                
                CloseTradesManager.GetPositionsAndAllocatedTradeList(ref _closeTradeInterfaceData);
                if (int.Equals(_closeTradeInterfaceData.AllocatedTrades.Count, 0) && int.Equals(_closeTradeInterfaceData.NetPositions.Count, 0))
                {
                    throw (new Exception("No data exists for the filters selected."));
                }
                SetGridDataSources();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Nirvana.Global.Common.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //SetConditionalFilters();
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
        /// Handles the Click event of the btnRun control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            
            bool eligibleMatchingTaxLotsPresent = true;

            AllocatedTradesList buyTaxLotsAndPositions;
            AllocatedTradesList sellTaxLotsAndPositions;
            SortAndGetLongAndShortRecords(out buyTaxLotsAndPositions, out sellTaxLotsAndPositions);

            while (eligibleMatchingTaxLotsPresent)
            {                
                eligibleMatchingTaxLotsPresent = CloseTaxLotsAutomatically(buyTaxLotsAndPositions, sellTaxLotsAndPositions);                                                        
            }
            if (!eligibleMatchingTaxLotsPresent )
            {
                if(buyTaxLotsAndPositions.Count > 0)
                {
                    MakePositionOfRemainingBuyTaxLots(buyTaxLotsAndPositions);
                }
                this.grdLongPosition.Rows.Refresh(RefreshRow.FireInitializeRow);
                if (sellTaxLotsAndPositions.Count > 0)
                {
                    MakePositionOfRemainingSellShortTaxLots(sellTaxLotsAndPositions);
                }    
            
                this.grdShortPosition.Rows.Refresh(RefreshRow.FireInitializeRow);
            }
        }

        /// <summary>
        /// Sorts the and get long and short records.
        /// </summary>
        /// <param name="buyTaxLotsAndPositions">The buy tax lots and positions.</param>
        /// <param name="sellTaxLotsAndPositions">The sell tax lots and positions.</param>
        private void SortAndGetLongAndShortRecords(out AllocatedTradesList buyTaxLotsAndPositions, out AllocatedTradesList sellTaxLotsAndPositions)
        {
            CloseTradeMethodology methodology = _closeTradeInterfaceData.CloseTradeFilter.DefaultMethodology;
            CloseTradeAlogrithm algorithm = _closeTradeInterfaceData.CloseTradeFilter.Algorithm;

            SortedBindingList<AllocatedTrade> sortedTaxLots = new SortedBindingList<AllocatedTrade>(_closeTradeInterfaceData.AllocatedTrades);
            buyTaxLotsAndPositions = new AllocatedTradesList();
            sellTaxLotsAndPositions = new AllocatedTradesList();

            if (methodology == CloseTradeMethodology.Automatic)
            {
                if (algorithm == CloseTradeAlogrithm.LIFO)
                {
                    sortedTaxLots.ApplySort(COL_TradeDate, ListSortDirection.Descending);
                }
                else if (algorithm == CloseTradeAlogrithm.FIFO)
                {
                    sortedTaxLots.ApplySort(COL_TradeDate, ListSortDirection.Ascending);
                }
                else if (algorithm == CloseTradeAlogrithm.HIFO)
                {
                    sortedTaxLots.ApplySort(COL_AveragePrice, ListSortDirection.Descending);
                }
            }
            GetBuyAndSellTaxLots(sortedTaxLots, buyTaxLotsAndPositions, sellTaxLotsAndPositions);
        }



        /// <summary>
        /// Makes the position of remaining sell short tax lots.
        /// </summary>
        /// <param name="sellTaxLotsAndPositions">The sell tax lots and positions.</param>
        private void MakePositionOfRemainingSellShortTaxLots(AllocatedTradesList sellTaxLotsAndPositions)
        {
            Position shortPosition = null;
            for (int counter = 0; counter < sellTaxLotsAndPositions.Count; counter++)
            {
                AllocatedTrade taxLot = null;
                taxLot = sellTaxLotsAndPositions[counter];
                
                long taxLotQuantity = taxLot.Quantity;
                long taxLotID = taxLot.ID;                
                string sideID = sellTaxLotsAndPositions[counter].SideID;                   
                
                if (string.Equals(sideID, FIXConstants.SIDE_SellShort))
                {                    
                    if (!taxLot.IsPosition)
                    {
                        shortPosition = new Position();
                        shortPosition.PositionType = PositionType.Short;
                        
                        
                        shortPosition.ID = Guid.NewGuid();
                        shortPosition.StartTaxLotID = taxLotID;
                        shortPosition.PositionStartQuantity = taxLotQuantity;
                        shortPosition.ClosedQty = 0;
                        shortPosition.OpenQty = taxLotQuantity;                        
                        shortPosition.AUECID = taxLot.AUECID;
                        shortPosition.StartDate = DateTime.Now;
                        shortPosition.Symbol = taxLot.Symbol;
                        shortPosition.FundValue = taxLot.FundValue;
                        shortPosition.AveragePrice = taxLot.AveragePrice;

                        //Mark tax lot as a position
                        taxLot.IsPosition = true;                        
                        taxLot.PositionID = shortPosition.ID;

                        // Position shortPositionClonedItem = shortPosition.Clone();
                        _closeTradeInterfaceData.NetPositions.Add(shortPosition);
                    }                    
                }
                else if(string.Equals(sideID, FIXConstants.SIDE_Sell))
                {                    
                    MessageBox.Show("Remaining " + Convert.ToString(taxLotQuantity) + " shares for Tax Lot ID : " + Convert.ToString(taxLotID) + " on Sell Grid are orders which cannot be permitted by trading ticket", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }




        /// <summary>
        /// Makes the position of remaining buy tax lots.
        /// </summary>
        /// <param name="buyTaxLotsAndPositions">The buy tax lots and positions.</param>
        private void MakePositionOfRemainingBuyTaxLots(AllocatedTradesList buyTaxLotsAndPositions)
        {
            Position longPosition = null;
            for (int counter = 0; counter < buyTaxLotsAndPositions.Count; counter++)
			{               
                AllocatedTrade buyTaxLot = buyTaxLotsAndPositions[counter];
                if (!buyTaxLot.IsPosition)  
                {
                    longPosition = new Position();
                    longPosition.ID = Guid.NewGuid();

                    longPosition.StartTaxLotID = buyTaxLot.ID;
                    longPosition.PositionStartQuantity = buyTaxLot.Quantity;
                    longPosition.ClosedQty = 0;
                    longPosition.OpenQty = buyTaxLot.Quantity;
                    longPosition.PositionType = PositionType.Long;
                    longPosition.AUECID = buyTaxLot.AUECID;
                    longPosition.StartDate = DateTime.Now;
                    longPosition.Symbol = buyTaxLot.Symbol;
                    longPosition.FundValue = buyTaxLot.FundValue;
                    ///Do we need to change the average price
                    longPosition.AveragePrice = buyTaxLot.AveragePrice;

                    //Mark tax lot as a position
                    buyTaxLot.IsPosition = true;
                    
                    buyTaxLot.PositionID = longPosition.ID;

                    // Position buyPositionClonedItem = buyPosition.Clone();
                    _closeTradeInterfaceData.NetPositions.Add(longPosition);                    
                }                
			}            
        }



        /// <summary>
        /// Closes the tax lots automatically. Don't touch the code in this method without talking to Sugandh Jain
        /// Or Change at your own risk. [Sugandh Jain 02-Jan-2007]
        /// </summary>
        /// <param name="buyTaxLotsAndPositions">The buy tax lots and positions.</param>
        /// <param name="sellTaxLotsAndPositions">The sell tax lots and positions.</param>
        private bool CloseTaxLotsAutomatically(AllocatedTradesList buyTaxLotsAndPositions, AllocatedTradesList sellTaxLotsAndPositions)
        {

            //Closing Buy Taxlots
            // foreach (AllocatedTrade buyTaxLot in buyTaxLotsAndPositions)
            //int counter = 0;            
            //int countBuyItemsRemoved = 0;
            bool eligibleMatchingTaxLotsPresent = true;
            int totalTaxLotInitialCount = buyTaxLotsAndPositions.Count + sellTaxLotsAndPositions.Count;
            
            for (int counter = 0; counter < buyTaxLotsAndPositions.Count; counter++)
            {
                //while (buyTaxLotsAndPositions.Count > 0 && counter != buyTaxLotsAndPositions.Count)
                //{
                
                //countBuyItemsRemoved = 0;
                // for (int counter = 0; counter < buyTaxLotsAndPositions.Count; counter++)
                //  {
                AllocatedTrade buyTaxLot = new AllocatedTrade();
                //if(buyTaxLotsAndPositions.Count >= counter)
                //{
                buyTaxLot = buyTaxLotsAndPositions[counter];
                //}
                while (buyTaxLot.Quantity > 0)
                {
                    bool isClosingTaxLotSmaller = true;
                    AllocatedTrade matchingEligibleTaxLot = GetEligibleClosingTaxLot(sellTaxLotsAndPositions, buyTaxLot.Symbol, buyTaxLot.FundValue.ID, false);
                    if (matchingEligibleTaxLot.ID > 0)
                    {
                        if (matchingEligibleTaxLot.SideID != "5")
                        {
                            isClosingTaxLotSmaller = true;
                            if (matchingEligibleTaxLot.Quantity > buyTaxLot.Quantity)
                            {
                                isClosingTaxLotSmaller = false;
                            }
                            ClosePosition(buyTaxLot, matchingEligibleTaxLot);
                            if (isClosingTaxLotSmaller)
                            {
                                sellTaxLotsAndPositions.Remove(matchingEligibleTaxLot);
                            }
                        }
                        // i.e. this is hit,when the tax lot on sell grid is Sell Short.
                        else
                        {
                            isClosingTaxLotSmaller = true;
                            if (buyTaxLot.Quantity > matchingEligibleTaxLot.Quantity)
                            {
                                isClosingTaxLotSmaller = false;
                            }
                            if (!buyTaxLot.IsPosition)
                            {
                                ClosePosition(matchingEligibleTaxLot, buyTaxLot);
                                if (isClosingTaxLotSmaller)
                                {
                                    buyTaxLotsAndPositions.Remove(buyTaxLot);
                                }
                                else
                                {
                                    sellTaxLotsAndPositions.Remove(matchingEligibleTaxLot);
                                    continue;
                                }
                            }
                            AllocatedTrade sellTaxLot = matchingEligibleTaxLot;
                            while (sellTaxLot.Quantity > 0)
                            {
                                AllocatedTrade matchingBuyTaxLot = GetEligibleClosingTaxLot(buyTaxLotsAndPositions, sellTaxLot.Symbol, sellTaxLot.FundValue.ID, true);
                                if (matchingBuyTaxLot.ID > 0)
                                {
                                    isClosingTaxLotSmaller = true;
                                    if (matchingBuyTaxLot.Quantity > sellTaxLot.Quantity)
                                    {
                                        isClosingTaxLotSmaller = false;
                                    }
                                    ClosePosition(sellTaxLot, matchingBuyTaxLot);
                                    if (isClosingTaxLotSmaller)
                                    {
                                        buyTaxLotsAndPositions.Remove(matchingBuyTaxLot);
                                    }
                                    else
                                    {
                                        sellTaxLotsAndPositions.Remove(sellTaxLot);
                                    }
                                    
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            int totalTaxLotFinalCount = buyTaxLotsAndPositions.Count + sellTaxLotsAndPositions.Count;

            if(int.Equals(totalTaxLotInitialCount , totalTaxLotFinalCount))
            {
                eligibleMatchingTaxLotsPresent = false;
            }
            return eligibleMatchingTaxLotsPresent;
        }


        /// <summary>
        /// Gets the matching buy tax lot.
        /// </summary>
        /// <param name="buyTaxLotsAndPositions">The buy tax lots and positions.</param>
        /// <param name="matchingSymbol">The matching symbol.</param>
        /// <param name="matchingFundID">The matching fund ID.</param>
        /// <returns></returns>
        private AllocatedTrade GetMatchingBuyTaxLot(AllocatedTradesList buyTaxLotsAndPositions, string matchingSymbol, int matchingFundID)
        {
            AllocatedTrade result = new AllocatedTrade();
            foreach (AllocatedTrade taxLot in buyTaxLotsAndPositions)
            {
                if (string.Equals(taxLot.Symbol, matchingSymbol)
                        && int.Equals(taxLot.FundValue.ID, matchingFundID) )
                {
                    result = taxLot;
                    break;
                }
            }
            return result;

        }



        /// <summary>
        /// Gets the eligible closing tax lot.
        /// </summary>
        /// <param name="TaxLotsAndPositions">The tax lots and positions.</param>
        /// <param name="matchingSymbol">The matching symbol.</param>
        /// <param name="matchingFundID">The matching fund ID.</param>
        /// <returns></returns>
        private AllocatedTrade GetEligibleClosingTaxLot(AllocatedTradesList TaxLotsAndPositions, string matchingSymbol, int matchingFundID, bool isShort)
        {
            AllocatedTrade result = new AllocatedTrade();            
            foreach (AllocatedTrade taxLot in TaxLotsAndPositions)
            {
                if (!isShort)
                {
                    if (string.Equals(taxLot.Symbol, matchingSymbol)
                            && int.Equals(taxLot.FundValue.ID, matchingFundID))
                    {
                        result = taxLot;
                        break;
                    }
                }
                else
                {
                    if (string.Equals(taxLot.Symbol, matchingSymbol)
                            && int.Equals(taxLot.FundValue.ID, matchingFundID) && !taxLot.IsPosition)
                    {
                        result = taxLot;
                        break;
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// Gets the buy and sell tax lots.
        /// </summary>
        /// <param name="sortedTaxLots">The sorted tax lots.</param>
        /// <param name="buyTaxLotsAndPositions">The buy tax lots and positions.</param>
        /// <param name="sellTaxLotsAndPositions">The sell tax lots and positions.</param>
        private static void GetBuyAndSellTaxLots(SortedBindingList<AllocatedTrade> sortedTaxLots, AllocatedTradesList buyTaxLotsAndPositions, AllocatedTradesList sellTaxLotsAndPositions)
        {
            for (int counter = 0; counter < sortedTaxLots.Count; counter++)
            {
                if ((sortedTaxLots[counter].Side).ToUpper() == ("Buy").ToUpper())
                {

                    buyTaxLotsAndPositions.Add(sortedTaxLots[counter]);
                }
                else
                {
                    sellTaxLotsAndPositions.Add(sortedTaxLots[counter]);
                }
            }
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
                _frmCloseTradeMatchedTradeReport = new Nirvana.PM.Client.UI.Forms.CloseTradeMatchedTradeReport();
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

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            

            NetPositionList netPostionList = _closeTradeInterfaceData.NetPositions; 
            int dataSourceID = _closeTradeInterfaceData.CloseTradeFilter.DataSourceNameID.ID;            
            

            int numberOfRowsEffected; 
            try
            {
                numberOfRowsEffected = CloseTradesManager.SaveCloseTradesData(netPostionList, dataSourceID);

                if (numberOfRowsEffected > 0)
                {
                    int closeTradeReportID = CloseTradesManager.SaveCloseTradesFilterPreferences(_closeTradeInterfaceData.CloseTradeFilter);
                    //int result = CloseTradesManager.SaveCloseTradesFundPreferences(_closeTradeInterfaceData, closeTradeReportID);
                    //result = CloseTradesManager.SaveCloseTradesAUECPreferences(_closeTradeInterfaceData, closeTradeReportID);    
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Nirvana.Global.Common.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Handles the InitializeRow event of the grdNetPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdNetPosition_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Band.ParentBand == null)
            {
                if (e.Row.Cells[COL_PositionType] != null)
                {
                    if (string.Equals(e.Row.Cells[COL_PositionType].Text, "Long"))
                    {
                        e.Row.Appearance.ForeColor = Color.Green;
                        e.Row.Appearance.BackColor = Color.Lavender;
                    }
                    else
                    {
                        e.Row.Appearance.ForeColor = Color.GreenYellow;
                        e.Row.Appearance.BackColor = Color.Chocolate;
                    }
                }
            }
            else
            {
                if ((string.Equals(e.Row.ParentRow.Cells[COL_PositionType].Text, "Long")))
                {
                    e.Row.Appearance.ForeColor = Color.Red;
                    e.Row.Appearance.BackColor = Color.FromArgb(64, 64, 64);
                }
                else
                {
                    e.Row.Appearance.ForeColor = Color.Green;
                    e.Row.Appearance.BackColor = Color.Black;
                }
            }

            
        }


        /// <summary>
        /// Handles the ExpandedStateChanged event of the grpExpandableYetToClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void grpExpandableYetToClose_ExpandedStateChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the ExpandedStateChanged event of the grpExpandableNetPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void grpExpandableNetPosition_ExpandedStateChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the ExpandedStateChanged event of the grpExpandableCloseTradeFilters control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void grpExpandableCloseTradeFilters_ExpandedStateChanged(object sender, EventArgs e)
        {
            if (!grpExpandableCloseTradeFilters.Expanded)
            {

            }
        }

       

       

        
    }
}
