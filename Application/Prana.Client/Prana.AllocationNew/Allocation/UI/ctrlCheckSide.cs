using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.UIUtilities;
using Infragistics.Win;

namespace Prana.AllocationNew.Allocation.UI
{
    public partial class ctrlCheckSide : UserControl
    {
        public ctrlCheckSide()
        {
            InitializeComponent();
        }

        UltraGridBand _gridBandShortPositions = null;
        UltraGridBand _gridBandLongPositions = null;

        const string SIDEBUY = "BUY";
        const string SIDEBUYTOCLOSE = "BUY TO CLOSE";
        const string SIDESELL = "SELL";
        const string SIDESELLSHORT = "SELL SHORT";

        const string _currencyColumnFormat = "#,#.00";

        internal void BindGrid(List<TaxLot> taxlots)
        {
            //grdLong.DataSource = null;
            //grdShort.DataSource = null;
            grdLong.DataSource = taxlots;
            grdLong.DataBind();
            grdShort.DataSource = taxlots;
            grdShort.DataBind();

            SetDefaultFilters();
        }

        #region Column Captions

        const string CAP_TaxlotId = "Taxlot ID";
        const string CAP_Account = "Account";
        const string CAP_Strategy = "Strategy";
        const string CAP_TradeDate = "Trade Date";
        const string CAP_Symbol = "Symbol";
        const string CAP_StartQty = "Start Quantity";
        const string CAP_OpenQty = "Open Qty";
        const string CAP_AvgPrice = "Opening Price";
        const string CAP_Side = "Side";
        const string CAP_AssetCategory = "Asset";
        const string CAP_SecurityFullName = "Security Name";
        #endregion

        #region Allocated Trades Grid columns

        const string COL_AllocationID = "TaxLotID";
        const string COL_TradeDate = "AUECLocalDate";
        const string COL_ClosingTradeDate = "ClosingTradeDate";
        const string COL_TradeDateUTC = "TradeDateUTC";
        const string COL_Side = "OrderSide";
        const string COL_ClosingSide = "ClosingSide";
        const string COL_Symbol = "Symbol";
        const string COL_SecurityFullName = "CompanyName";
        const string COL_OpenQuantity = "TaxLotQty";
        const string COL_ClosedQty = "ClosedQty";

        const string COL_AveragePrice = "AvgPrice";
        const string COL_OpenAveragePrice = "OpenAveragePrice";
        const string COL_ClosedAveragePrice = "ClosedAveragePrice";
        const string COL_Account = "Level1Name";

        const string COL_SideID = "OrderSideTagValue";
        const string COL_IsPosition = "IsPosition";
        const string COL_AUEC = "AUECID";
        const string COL_PositionTaxlotID = "PositionTaxlotID";

        const string COL_OpenCommission = "OpenTotalCommissionandFees";
        const string COL_PositionCommission = "PositionTotalCommissionandFees";
        const string COL_OpenFees = "OtherBrokerOpenFees";
        const string COL_PositionFees = "PositionOtherBrokerFees";
        const string COL_ClosedCommission = "ClosedTotalCommissionandFees";
        const string COL_ClosingTotalCommissionandFees = "ClosingTotalCommissionandFees";
        const string COL_NetNotionalValue = "NetNotionalValue";
        const string COL_StrategyValue = "Level2Name";
        const string COL_SettledQty = "SettledQty";
        const string COL_CashSettledPrice = "CashSettledPrice";
        const string COL_ClosingMode = "ClosingMode";
        const string COL_IsExpired_Settled = "IsExpired_Settled";
        const string COL_AssetCategoryValue = "AssetCategoryValue";
        const string COL_ExpiryDate = "ExpirationDate";
        const string COL_Underlying = "UnderlyingName";
        const string COL_UnitCost = "UnitCost";
        const string COL_PositionTagValue = "PositionTag";
        const string COL_IsSwap = "ISSwap";

        #endregion

        public List<string> OpenTaxlotGridColumns
        {
            get
            {
                List<string> TaxlotGridColumns = new List<string>();
                TaxlotGridColumns.Add(COL_AllocationID);
                TaxlotGridColumns.Add(COL_TradeDate);
                TaxlotGridColumns.Add(COL_Side);
                TaxlotGridColumns.Add(COL_Symbol);
                TaxlotGridColumns.Add(COL_OpenQuantity);
                TaxlotGridColumns.Add(COL_AveragePrice);
                TaxlotGridColumns.Add(COL_StrategyValue);
                TaxlotGridColumns.Add(COL_Account);
                return TaxlotGridColumns;
            }
        }

        private void SetDefaultFilters()
        {
            try
            {
                grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].LogicalOperator = FilterLogicalOperator.Or;
                grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy);
                grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Closed);
                grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Open);
                grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Cover);

                grdShort.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].LogicalOperator = FilterLogicalOperator.Or;
                grdShort.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Sell);
                grdShort.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_SellShort);
                grdShort.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Sell_Open);
                grdShort.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Sell_Closed);

                //grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_IsPosition].FilterConditions.Add(FilterComparisionOperator.Equals, true);


            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            //IsPostion = 1 means that it is a position
            //grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[COL_IsPosition].FilterConditions.Add(FilterComparisionOperator.Equals, true);
        }

        private void SetGridColumns(UltraGrid grid)
        {
            Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;

            UltraGridBand gridBand = grid.DisplayLayout.Bands[0];

            UltraWinGridUtils.SetColumns(OpenTaxlotGridColumns, grid);
            foreach (UltraGridColumn col in columns)
            {
                col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            foreach (string col in OpenTaxlotGridColumns)
            {

                if (columns.Exists(col))
                {
                    UltraGridColumn column = columns[col];
                    column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
            }

            UltraGridColumn colAllocationID = gridBand.Columns[COL_AllocationID];
            colAllocationID.Width = 45;
            colAllocationID.Header.Caption = CAP_TaxlotId;
            colAllocationID.Header.VisiblePosition = 1;

            UltraGridColumn colSymbol = gridBand.Columns[COL_Symbol];
            colSymbol.Width = 50;
            colSymbol.Header.Caption = CAP_Symbol;
            colSymbol.Header.VisiblePosition = 2;

            UltraGridColumn ColSide = gridBand.Columns[COL_Side];
            ColSide.Width = 65;
            //ColIsSelected.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ColSide.Header.Caption = CAP_Side;
            ColSide.Header.VisiblePosition = 3;

            UltraGridColumn colTradeDate = gridBand.Columns[COL_TradeDate];
            colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
            colTradeDate.Width = 140;
            colTradeDate.Header.Caption = CAP_TradeDate;
            colTradeDate.Header.VisiblePosition = 4;
            colTradeDate.CellClickAction = CellClickAction.Default;                

            UltraGridColumn colSecurityFullName = gridBand.Columns[COL_SecurityFullName];
            colSecurityFullName.Width = 100;
            colSecurityFullName.Header.Caption = CAP_SecurityFullName;
            colSecurityFullName.Header.VisiblePosition = 5;

            UltraGridColumn colOpenQuantity = gridBand.Columns[COL_OpenQuantity];
            colOpenQuantity.Width = 65;
            colOpenQuantity.Header.Caption = "Quantity";
            //colExecutedQuantity.CellClickAction = CellClickAction.Edit;
            colOpenQuantity.Header.VisiblePosition = 6;

            UltraGridColumn colAveragePrice = gridBand.Columns[COL_AveragePrice];
            colAveragePrice.Width = 80;
            colAveragePrice.Format = _currencyColumnFormat;
            colAveragePrice.Header.Caption = CAP_AvgPrice;
            //colAveragePrice.CellClickAction = CellClickAction.Edit;
            colAveragePrice.Header.VisiblePosition = 7;

            UltraGridColumn colAccount = gridBand.Columns[COL_Account];
            colAccount.Width = 80;
            colAccount.Header.Caption = CAP_Account;
            colAccount.Header.VisiblePosition = 8;
        }

        private void grdShort_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
              // Get Column Chooser for the grid
                e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                UltraGridLayout gridLayout = grdShort.DisplayLayout;
                gridLayout.Override.RowAppearance.BackColor = Color.FromArgb(64, 64, 64);

                GridInitializeLayout(sender, e);

                _gridBandShortPositions = grdShort.DisplayLayout.Bands[0];
                SetGridColumns(grdShort);
        }

        private void GridInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //retrieve reference to grid
            UltraGrid ug = (UltraGrid)sender;
            //configure grid
            e.Layout.Override.SelectTypeCell = SelectType.SingleAutoDrag;
            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;

            ug.AllowDrop = true;
        }

        private void grdLong_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.RowSelectors = DefaultableBoolean.True;
            e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                UltraGridLayout gridLayout = grdLong.DisplayLayout;
                grdLong.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                GridInitializeLayout(sender, e);
                _gridBandLongPositions = grdLong.DisplayLayout.Bands[0];
                SetGridColumns(grdLong);
        }

        private void SetGridsRowAppearance(InitializeRowEventArgs e, bool isLongAllocatedTradesGrid, UltraGrid grdName)
        {
            bool isPosition = false;
            string side = string.Empty;
            if (e.Row.Cells[COL_IsPosition] != null && e.Row.Cells[COL_Side] != null)
            {
                isPosition = Convert.ToBoolean(e.Row.Cells[COL_IsPosition].Text);
                side = Convert.ToString(e.Row.Cells[COL_Side].Text).ToUpperInvariant();
                if (isLongAllocatedTradesGrid)
                {
                    if (bool.Equals(isPosition, true))
                    {
                        e.Row.Appearance.ForeColor = Color.Green;
                        e.Row.Appearance.BackColor = Color.Lavender;
                        grdName.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                    }
                    else
                    {
                        e.Row.Appearance.ForeColor = Color.LightGreen;
                        //e.Row.Appearance.BackColor = Color.Black;
                    }
                }
                else
                {
                    if (bool.Equals(isPosition, true))
                    {
                        grdName.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                    }
                    if (bool.Equals(isPosition, true) || string.Equals(side, SIDESELLSHORT))
                    {
                        e.Row.Appearance.ForeColor = Color.GreenYellow;
                        e.Row.Appearance.BackColor = Color.Chocolate;
                    }
                    else
                    {
                        e.Row.Appearance.ForeColor = Color.Orange;
                    }
                }
            }
        }

        private void grdLong_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            SetGridsRowAppearance(e, true, grdLong);
        }      

        private void grdShort_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            SetGridsRowAppearance(e, false, grdShort);

        }
	

       
    }
}
