using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Blotter
{
    public class SummaryBlotterGrid : Prana.Blotter.WorkingSubBlotterGrid
    {
        private System.ComponentModel.IContainer components = null;
        private CompanyUser _loginUser;
        public CompanyUser LoginUSer
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        /// <summary>
        /// Occurs when [highlight symbol send on bloter main].
        /// </summary>
        public event EventHandler<EventArgs<string>> HighlightSymbolSendOnBloterMainFromSummary = null;

        SummarySettings summary;
        public SummaryBlotterGrid()
        {
            try
            {
                summary = new SummarySettings();
                InitializeComponent();
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

        protected override void Dispose(bool disposing)
        {
            if (summary != null)
            {
                summary.Dispose();
                summary = null;
            }

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes the contol.
        /// </summary>
        /// <param name="blotterOdrColl">The blotter odr coll.</param>
        /// <param name="key">The key.</param>
        /// <param name="loginUser">The login user.</param>
        /// <param name="blotterPreferenceData">The blotter color prefs.</param>
        public override void InitContol(OrderBindingList blotterOdrColl, string key, CompanyUser loginUser, BlotterPreferenceData blotterPreferenceData)
        {
            try
            {
                LoginUSer = loginUser;
                base.InitContol(blotterOdrColl, key, loginUser, blotterPreferenceData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Select All", 0);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            ((System.ComponentModel.ISupportInitialize)(this.dgBlotter)).BeginInit();
            this.SuspendLayout();
            // 
            // dgBlotter
            // 
            ultraGridColumn1.DataType = typeof(bool);
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1});
            this.dgBlotter.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.dgBlotter.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.dgBlotter.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.DisplayLayout.GroupByBox.Hidden = true;
            this.dgBlotter.DisplayLayout.MaxBandDepth = 2;
            this.dgBlotter.DisplayLayout.MaxColScrollRegions = 1;
            this.dgBlotter.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.Gold;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.dgBlotter.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.dgBlotter.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.dgBlotter.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.dgBlotter.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dgBlotter.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dgBlotter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.dgBlotter.DisplayLayout.Override.CellPadding = 0;
            this.dgBlotter.DisplayLayout.Override.DefaultColWidth = 50;
            this.dgBlotter.DisplayLayout.Override.DefaultRowHeight = 20;
            this.dgBlotter.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Like;
            this.dgBlotter.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.Hidden;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.dgBlotter.DisplayLayout.Override.GroupByRowAppearance = appearance3;
            this.dgBlotter.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value]";
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.dgBlotter.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.dgBlotter.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.dgBlotter.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.dgBlotter.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.dgBlotter.DisplayLayout.Override.MaxSelectedRows = 1;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.Lime;
            this.dgBlotter.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance6.ForeColor = System.Drawing.Color.Lime;
            this.dgBlotter.DisplayLayout.Override.RowAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.dgBlotter.DisplayLayout.Override.RowSelectorAppearance = appearance7;
            this.dgBlotter.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.dgBlotter.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance8.BackColor = System.Drawing.Color.Gold;
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.dgBlotter.DisplayLayout.Override.SelectedCellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.dgBlotter.DisplayLayout.Override.SelectedRowAppearance = appearance9;
            this.dgBlotter.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.dgBlotter.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.dgBlotter.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.dgBlotter.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows)
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance10.BackColor = System.Drawing.SystemColors.Info;
            this.dgBlotter.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance10;
            this.dgBlotter.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dgBlotter.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance11;
            this.dgBlotter.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.dgBlotter.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.dgBlotter.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.dgBlotter.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.dgBlotter.Size = new System.Drawing.Size(339, 244);
            this.dgBlotter.MouseDown += dgBlotter_MouseDown;
            // 
            // btnExpansion
            // 
            this.btnExpansion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExpansion.Size = new System.Drawing.Size(24, 16);
            // 
            // btnCollapse
            // 
            this.btnCollapse.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCollapse.Location = new System.Drawing.Point(126, 0);
            this.btnCollapse.Size = new System.Drawing.Size(15, 13);
            // 
            // lblCollapseALL
            // 
            this.lblCollapseALL.Location = new System.Drawing.Point(138, 0);
            this.lblCollapseALL.Size = new System.Drawing.Size(78, 13);
            // 
            // lblExpansion
            // 
            this.lblExpansion.Location = new System.Drawing.Point(45, 0);
            this.lblExpansion.Size = new System.Drawing.Size(75, 13);
            // 
            // SummaryBlotterGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "SummaryBlotterGrid";
            this.Size = new System.Drawing.Size(339, 333);
            ((System.ComponentModel.ISupportInitialize)(this.dgBlotter)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Handles the MouseDown event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void dgBlotter_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (dgBlotter.ActiveRow != null && dgBlotter.ActiveRow.Description != null)
                    {
                        //using substring to exclude "Symbol:" from description to get symbol name
                        var symbol = dgBlotter.ActiveRow.Description.Substring(9);
                        if (!string.IsNullOrEmpty(symbol) && HighlightSymbolSendOnBloterMainFromSummary != null)
                            HighlightSymbolSendOnBloterMainFromSummary(null, new EventArgs<string>(symbol));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }
        #endregion

        #region Custom Summary Calculator Class Position Summary [Position]
        private class PositionSummary : ICustomSummaryCalculator
        {
            private double totals = 0;
            internal PositionSummary()
            {
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (row != null)
                    {
                        OrderSingle oCurrent = (OrderSingle)row.ListObject;
                        if (oCurrent != null)
                        {
                            object quantity = oCurrent.CumQty;
                            object ordersidetagvalue = oCurrent.OrderSideTagValue;

                            if (quantity is DBNull)
                            {
                                return;
                            }
                            double nQuantity = 0;
                            if (Convert.ToDouble(quantity) == double.MinValue)
                            {
                                quantity = 0.0;
                            }

                            if (quantity.ToString() != string.Empty || quantity.ToString() != Double.Epsilon.ToString())
                            {
                                nQuantity = Convert.ToDouble(quantity);
                            }

                            switch (ordersidetagvalue.ToString())
                            {
                                case FIXConstants.SIDE_Buy:
                                case FIXConstants.SIDE_Buy_Closed:
                                case FIXConstants.SIDE_Buy_Open:
                                    this.totals += nQuantity;
                                    break;

                                case FIXConstants.SIDE_Sell:
                                case FIXConstants.SIDE_Sell_Open:
                                case FIXConstants.SIDE_Sell_Closed:
                                case FIXConstants.SIDE_SellPlus:
                                case FIXConstants.SIDE_SellShort:
                                    this.totals += nQuantity * (-1);
                                    break;
                            }
                        }
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                return this.totals;
            }
        }
        #endregion

        #region Custom Summary Calculator Class OrderTotalsSummary [avg price]
        private class OrderTotalsSummary : ICustomSummaryCalculator
        {
            private double totals = 0;
            private double totalQty = 0;

            internal OrderTotalsSummary()
            {
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                // Begins the summary for the SummarySettings object passed in. Implementation of  
                // this method should reset any state variables used for calculating the summary. 
                this.totals = 0;
                this.totalQty = 0;

            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {

                try
                { // Here is where we process each row that gets passed in. 
                    object unitPrice = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[Global.OrderFields.PROPERTY_AVGPRICE]);
                    object quantity = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY]);

                    // Handle null values 
                    if (unitPrice is DBNull || quantity is DBNull)
                    {
                        return;
                    }

                    //added for the minrow that appears on startup
                    if (Convert.ToDouble(quantity) == double.MinValue)
                    {
                        quantity = 0.0;
                    }

                    // Convert to double. 

                    double nUnitPrice = 0;
                    double nQuantity = 0;
                    if (unitPrice.ToString() != string.Empty || unitPrice.ToString() != Double.Epsilon.ToString())
                    {
                        nUnitPrice = Convert.ToDouble(unitPrice);
                    }
                    if (quantity.ToString() != string.Empty || quantity.ToString() != Double.Epsilon.ToString())
                    {
                        nQuantity = Convert.ToDouble(quantity);
                    }

                    this.totals += nQuantity * nUnitPrice;
                    this.totalQty += nQuantity;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                double avgPrice = 0;
                try
                {
                    // This gets called when the every row has been processed so here is where we 
                    // would return the calculated summary value. 
                    if (this.totalQty > 0)
                    {
                        avgPrice = this.totals / this.totalQty;
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
                return avgPrice;
            }

        }
        #endregion

        #region Custom Summary Calculator Class OrderExposure [Notional Exchange Currency]
        private class OrderExposureTradedCurrency : ICustomSummaryCalculator
        {
            private double totals = 0;
            private double totalQty = 0;

            internal OrderExposureTradedCurrency()
            {
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.totalQty = 0;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    OrderSingle oCurrent = (OrderSingle)row.ListObject;

                    object unitPrice = oCurrent.AvgPrice;
                    object quantity = oCurrent.CumQty;
                    object auec = oCurrent.AUECID;
                    object ordside = oCurrent.OrderSideTagValue;

                    if (unitPrice is DBNull || quantity is DBNull || auec is DBNull)
                    {
                        return;
                    }

                    if (Convert.ToDouble(quantity) == double.MinValue)
                    {
                        quantity = 0.0;
                    }

                    double nUnitPrice = 0;
                    double nQuantity = 0;
                    if (unitPrice.ToString() != string.Empty || unitPrice.ToString() != Double.Epsilon.ToString())
                    {
                        nUnitPrice = Convert.ToDouble(unitPrice);
                    }

                    if (quantity.ToString() != string.Empty || quantity.ToString() != Double.Epsilon.ToString())
                    {
                        nQuantity = Convert.ToDouble(quantity);
                    }

                    OrderSingle rowOrder = (OrderSingle)row.ListObject;
                    double multiplier = rowOrder.ContractMultiplier;

                    switch (ordside.ToString())
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Closed:
                        case FIXConstants.SIDE_Buy_Open:
                            this.totals += nQuantity * nUnitPrice * multiplier;
                            break;

                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_Sell_Closed:
                        case FIXConstants.SIDE_SellPlus:
                            this.totals += nQuantity * nUnitPrice * (-1) * multiplier;
                            break;

                        case FIXConstants.SIDE_SellShort:
                            this.totals += nQuantity * nUnitPrice * (-1) * multiplier;
                            break;
                    }
                    this.totalQty += nQuantity;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                double exposure = 0;
                try
                {
                    exposure = this.totals;
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return exposure;
            }
        }
        #endregion

        #region Custom Summary Calculator Class OrderExposure [Notional Base Currency]
        private class OrderExposureBaseCurrency : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private double totalQty = 0.0;
            private int companyID = int.MinValue;

            internal OrderExposureBaseCurrency(int _companyID)
            {
                companyID = _companyID;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.totalQty = 0;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    OrderSingle oCurrent = (OrderSingle)row.ListObject;

                    object unitPrice = oCurrent.AvgPrice;
                    object quantity = oCurrent.CumQty;
                    object auec = oCurrent.AUECID;
                    object ordside = oCurrent.OrderSideTagValue;

                    if (unitPrice is DBNull || quantity is DBNull || auec is DBNull)
                    {
                        return;
                    }

                    if (Convert.ToDouble(quantity) == double.MinValue)
                    {
                        quantity = 0.0;
                    }

                    double nUnitPrice = 0;
                    double nQuantity = 0;
                    if (unitPrice.ToString() != string.Empty || unitPrice.ToString() != Double.Epsilon.ToString())
                    {
                        nUnitPrice = Convert.ToDouble(unitPrice);
                    }

                    if (quantity.ToString() != string.Empty || quantity.ToString() != Double.Epsilon.ToString())
                    {
                        nQuantity = Convert.ToDouble(quantity);
                    }

                    OrderSingle rowOrder = (OrderSingle)row.ListObject;
                    double multiplier = rowOrder.ContractMultiplier;

                    switch (ordside.ToString())
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Closed:
                        case FIXConstants.SIDE_Buy_Open:
                            this.totals += Convert.ToDouble(nQuantity * nUnitPrice * multiplier);
                            break;

                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_Sell_Closed:
                        case FIXConstants.SIDE_SellPlus:
                            this.totals += Convert.ToDouble(nQuantity * nUnitPrice * (-1) * multiplier);
                            break;

                        case FIXConstants.SIDE_SellShort:
                            this.totals += nQuantity * nUnitPrice * (-1) * multiplier;
                            break;
                    }

                    string rowOrderCurrency = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(rowOrder.CurrencyID);
                    int companyBaseCurrID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    string companyBaseCurr = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(companyBaseCurrID);
                    ConversionRate conversionRate = new ConversionRate();
                    if (!rowOrderCurrency.Equals(companyBaseCurr))
                    {
                        if (rowOrder.FXRate != 0.0)
                        {
                            conversionRate.RateValue = rowOrder.FXRate;
                        }
                        else
                        {
                            conversionRate = ForexConverter.GetInstance(companyID).GetConversionRateForCurrencyToBaseCurrency(rowOrder.CurrencyID, rowOrder.Level1ID);
                        }

                        switch (conversionRate.ConversionMethod)
                        {
                            case Prana.BusinessObjects.AppConstants.Operator.M:
                                this.totals = this.totals * conversionRate.RateValue;
                                break;
                            case Prana.BusinessObjects.AppConstants.Operator.D:
                                this.totals = this.totals / conversionRate.RateValue;
                                break;
                            default:
                                break;
                        }
                    }
                    this.totalQty += nQuantity;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                double exposure = 0;
                try
                {
                    exposure = this.totals;
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return exposure;
            }
        }
        #endregion

        protected override void SummarySettings()
        {
            try
            {
                UltraGridBand band = this.dgBlotter.DisplayLayout.Bands[0];
                band.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;

                band.Indentation = 0;
                Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
                Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
                this.dgBlotter.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.SingleSummaryBasedOnDataType;
                this.dgBlotter.DisplayLayout.Override.BorderStyleSummaryValue = Infragistics.Win.UIElementBorderStyle.Solid;
                this.dgBlotter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                this.dgBlotter.DisplayLayout.Override.DefaultColWidth = 150;
                this.dgBlotter.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.CheckOnDisplay;
                appearance4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(211)), ((System.Byte)(207)), ((System.Byte)(172)));
                appearance4.BorderColor = System.Drawing.Color.FromArgb(((System.Byte)(157)), ((System.Byte)(151)), ((System.Byte)(84)));
                appearance4.ForeColor = System.Drawing.Color.Black;
                this.dgBlotter.DisplayLayout.Override.GroupByRowAppearance = appearance4;
                this.dgBlotter.DisplayLayout.Override.GroupByRowInitialExpansionState = Infragistics.Win.UltraWinGrid.GroupByRowInitialExpansionState.Collapsed;
                this.dgBlotter.DisplayLayout.Override.GroupByRowPadding = 0;
                this.dgBlotter.DisplayLayout.Override.GroupByRowSpacingAfter = 0;
                this.dgBlotter.DisplayLayout.Override.GroupByRowSpacingBefore = 0;
                this.dgBlotter.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
                appearance5.BackColor = System.Drawing.Color.Transparent;
                appearance5.ForeColor = System.Drawing.Color.Black;
                this.dgBlotter.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance5;
                this.dgBlotter.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
                this.dgBlotter.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                this.dgBlotter.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
                this.dgBlotter.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
                this.dgBlotter.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
                this.dgBlotter.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.Default;
                appearance10.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(192)), ((System.Byte)(255)));
                this.dgBlotter.DisplayLayout.Override.SummaryValueAppearance = appearance10;
                appearance11.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
                scrollBarLook1.Appearance = appearance11;
                this.dgBlotter.DisplayLayout.ScrollBarLook = scrollBarLook1;
                this.dgBlotter.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
                this.dgBlotter.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.dgBlotter.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
                this.dgBlotter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.GraphicsUnit.Pixel);
                this.dgBlotter.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;

                // Add the OrderTotals custom summary. 
                if (!band.Summaries.Exists("OrderTotals"))
                {
                    summary = band.Summaries.Add(
                        "OrderTotals",
                        SummaryType.Custom,
                        new OrderTotalsSummary(),
                        band.Columns[Global.OrderFields.PROPERTY_AVGPRICE],
                        SummaryPosition.UseSummaryPositionColumn,
                        band.Columns[Global.OrderFields.PROPERTY_AVGPRICE]
                        );
                    summary.DisplayFormat = "AvgPrice = {0:#,###0.0000}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    summary.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }

                if (!band.Summaries.Exists("OrderTotals1"))
                {
                    SummarySettings summary1 = band.Summaries.Add(
                        "OrderTotals1",
                        SummaryType.Custom,
                        new PositionSummary(),
                        band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY],
                        SummaryPosition.UseSummaryPositionColumn,
                        band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY]
                        );
                    summary1.DisplayFormat = "Position = {0:#,##,###.##}";
                    summary1.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }

                if (!band.Summaries.Exists("Notional"))
                {
                    SummarySettings summarynotional = band.Summaries.Add(
                        "Notional Exchange",
                        SummaryType.Custom,
                        new OrderExposureTradedCurrency(),
                        band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY],
                        SummaryPosition.UseSummaryPositionColumn,
                        band.Columns[Global.OrderFields.PROPERTY_ORDER_STATUS]
                        );
                    summarynotional.DisplayFormat = "Notional Value = {0:#,###0.00}";
                    summarynotional.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }

                if (!band.Summaries.Exists("Notional(Base)"))
                {
                    band.Columns[Global.OrderFields.PROPERTY_NOTIONALVALUE].HiddenWhenGroupBy = Infragistics.Win.DefaultableBoolean.False;
                    SummarySettings summary2 = band.Summaries.Add(
                        "Notional Base",
                        SummaryType.Custom,
                        new OrderExposureBaseCurrency(_loginUser.CompanyID),
                        band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY],
                        SummaryPosition.UseSummaryPositionColumn,
                        band.Columns[Global.OrderFields.PROPERTY_NOTIONALVALUE]
                        );
                    summary2.DisplayFormat = "Notional Value(Base) = {0:#,###0.00}";
                    summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }

                if (!band.Summaries.Exists("TargetQuantity"))
                {
                    SummarySettings summary3 = band.Summaries.Add(
                        "TargetQuantity",
                        SummaryType.Sum,
                        band.Columns[Global.OrderFields.PROPERTY_QUANTITY]
                        );
                    summary3.DisplayFormat = "Target Qty = {0:#,###0}";
                    summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    summary3.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
                dgBlotter.DisplayLayout.Override.GroupBySummaryValueAppearance.BackColor = System.Drawing.Color.Transparent;
                dgBlotter.DisplayLayout.Override.GroupBySummaryValueAppearance.BorderColor = System.Drawing.Color.Transparent;
                dgBlotter.DisplayLayout.Override.GroupBySummaryValueAppearance.ForeColor = System.Drawing.Color.DarkBlue;
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

        protected override void dgBlotter_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            base.dgBlotter_InitializeLayout(sender, e);
            try
            {
                e.Layout.Bands[0].SortedColumns.Clear();
                e.Layout.Bands[0].SortedColumns.Add(Global.OrderFields.PROPERTY_SYMBOL, false, true);
                e.Layout.Bands[0].SortedColumns.Add(Global.OrderFields.PROPERTY_ORDER_SIDE, false, true);
                if (!(e.Layout.Bands[0].Columns.Exists(Global.OrderFields.PROPERTY_NOTIONALVALUE)))
                {
                    e.Layout.Bands[0].Columns.Add(Global.OrderFields.PROPERTY_NOTIONALVALUE);
                }
                e.Layout.Bands[0].Columns[Global.OrderFields.PROPERTY_NOTIONALVALUE].Width = 300;
                e.Layout.Bands[0].Columns[Global.OrderFields.PROPERTY_ORDER_STATUS].Width = 300;
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    appearance1.BackColor = System.Drawing.Color.Black;
                    this.dgBlotter.DisplayLayout.Appearance = appearance1;
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
    }
}