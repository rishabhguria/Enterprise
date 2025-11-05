using Infragistics.Windows.DataPresenter;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.Controls.Preferences.Behaviours
{
    class AllocationFixedPreferencesGridBehavior : Behavior<XamDataGrid>
    {
        #region OnAttaching

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            try
            {
                AssociatedObject.CellChanged += AssociatedObject_CellChanged;
                AssociatedObject.FieldLayoutInitialized += AssociatedObject_FieldLayoutInitialized;
                base.OnAttached();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// The is modified property
        /// </summary>
        public static readonly DependencyProperty IsModifiedProperty = DependencyProperty.Register("IsModified", typeof(bool), typeof(AllocationFixedPreferencesGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The _total qty
        /// </summary>
        private double _totalQty = 0;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get { return (bool)GetValue(IsModifiedProperty); }
            set { SetValue(IsModifiedProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the CellChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.DataPresenter.Events.CellChangedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_CellChanged(object sender, Infragistics.Windows.DataPresenter.Events.CellChangedEventArgs e)
        {
            try
            {
                List<string> _currencyListForAlloScheme = AllocationSchemeDataManager.GetInstance.GetCurrencyLIstForAllocationScheme();
                FieldLayout fieldLayout = AssociatedObject.FieldLayouts.First();

                //Changes are made because there was no handling of 0 and empty string according to PRANA-15383
                double result;
                if (!double.TryParse(e.Editor.Text, out result))
                {
                    e.Editor.Text = "0";
                    e.Editor.Value = "0";
                }


                if (e.Cell.Field.Name.Equals(AllocationUIConstants.TOTAL_QUANTITY) && e.Editor.Value.ToString() != string.Empty)
                {
                    if (fieldLayout.Fields.IndexOf(AllocationUIConstants.TOTAL_QTY) != -1 && fieldLayout.Fields.IndexOf(AllocationUIConstants.TOTAL_PERCENTAGE) != -1)
                    {
                        if (double.TryParse(e.Editor.Text, out _totalQty))
                        {
                            double quantity = double.Parse(e.Editor.Text);
                            if (quantity >= 0)
                            {
                                _totalQty = double.Parse(e.Cell.Record.Cells[AllocationUIConstants.TOTAL_QTY].Value.ToString());
                                _totalQty -= double.Parse(e.Cell.Value.ToString());
                                e.Cell.Value = double.Parse(e.Editor.Text);
                                string allocSchemeKeyName = e.Cell.Record.Cells[AllocationUIConstants.ALLOCATION_SCHEME_KEY].Value.ToString();
                                AllocationSchemeKey allocSchemeKey = (AllocationSchemeKey)Enum.Parse(typeof(AllocationSchemeKey), allocSchemeKeyName);

                                string symbol = e.Cell.Record.Cells[AllocationUIConstants.SYMBOL].Value.ToString();
                                string orderSideTagValue = e.Cell.Record.Cells[AllocationUIConstants.ORDERSIDE_TAGVALUE].Value.ToString();
                                string primeBroker = e.Cell.Record.Cells[AllocationUIConstants.CAPTION_PB].Value.ToString();
                                string tradeType = e.Cell.Record.Cells[AllocationUIConstants.TRADE_TYPE].Value.ToString();
                                string currency = e.Cell.Record.Cells[AllocationUIConstants.CAPTION_CURRENCY_NAME].Value.ToString();

                                _totalQty += quantity;
                                foreach (DataRecord row in AssociatedObject.RecordManager.GetFilteredInDataRecords())
                                {
                                    switch (allocSchemeKey)
                                    {
                                        case AllocationSchemeKey.Symbol:
                                            UpdatePercentageAndQuantity(row, symbol, null, null);
                                            break;

                                        case AllocationSchemeKey.SymbolSide:
                                            UpdatePercentageAndQuantity(row, symbol, orderSideTagValue, null);
                                            break;

                                        case AllocationSchemeKey.PBSymbolSide:
                                            if (_currencyListForAlloScheme != null && _currencyListForAlloScheme.Count > 0 && !_currencyListForAlloScheme.Contains(currency) && tradeType.ToLower().Equals("swap"))
                                            {
                                                UpdatePercentageAndQuantity(row, symbol, orderSideTagValue, primeBroker);
                                            }
                                            else
                                                UpdatePercentageAndQuantity(row, symbol, orderSideTagValue, null);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                IsModified = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the FieldLayoutInitialized event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.DataPresenter.Events.FieldLayoutInitializedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_FieldLayoutInitialized(object sender, Infragistics.Windows.DataPresenter.Events.FieldLayoutInitializedEventArgs e)
        {
            try
            {
                FieldLayout fieldLayout = AssociatedObject.FieldLayouts.First();
                XamDataGrid dataGrid = (sender as XamDataGrid);

                if (AssociatedObject != null)
                    AssociatedObject.FieldSettings.AllowEdit = false;

                if (fieldLayout.Fields.IndexOf(AllocationUIConstants.TOTAL_QUANTITY) != -1)
                    fieldLayout.Fields[AllocationUIConstants.TOTAL_QUANTITY].AllowEdit = true;

                ColumnsCustomization(dataGrid);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Columnses the customization.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        private void ColumnsCustomization(XamDataGrid dataGrid)
        {
            try
            {
                FieldLayout fieldLayout = dataGrid.FieldLayouts.FirstOrDefault(x => x.Description.ToString().Equals(AllocationClientConstants.POSITION_MASTER));
                if (dataGrid.FieldLayouts.Any(x => x.Description.ToString().Equals(AllocationClientConstants.POSITION_MASTER)))
                {
                    foreach (Field field in fieldLayout.Fields)
                        field.Visibility = System.Windows.Visibility.Collapsed;

                    Dictionary<string, string> positionMasterFields = new Dictionary<string, string>();
                    positionMasterFields.Add(AllocationUIConstants.FUND_NAME, AllocationUIConstants.CAPTION_ACCOUNT_NAME);
                    positionMasterFields.Add(AllocationUIConstants.SYMBOL, AllocationUIConstants.CAPTION_TICKER_SYMBOL);
                    positionMasterFields.Add(AllocationUIConstants.LONG_NAME, AllocationUIConstants.CAPTION_DESCRIPTION);
                    positionMasterFields.Add(AllocationUIConstants.SEDOL, AllocationUIConstants.CAPTION_SEDOL_SYMBOL);
                    positionMasterFields.Add(AllocationUIConstants.BLOOMBERG, AllocationUIConstants.CAPTION_BLOOMBERG_WITH_SYMBOL);
                    positionMasterFields.Add(AllocationUIConstants.SIDE, AllocationUIConstants.SIDE);
                    positionMasterFields.Add(AllocationUIConstants.TOTAL_QUANTITY, AllocationUIConstants.TOTAL_QUANTITY);
                    positionMasterFields.Add(AllocationUIConstants.TOTAL_QTY, AllocationUIConstants.CAPTION_TOTAL_QUANTITY);
                    positionMasterFields.Add(AllocationUIConstants.TOTAL_PERCENTAGE, AllocationUIConstants.CAPTION_ALLOCATION_PERCENTAGE);
                    positionMasterFields.Add(AllocationUIConstants.ROUNDLOT, AllocationUIConstants.CAPTION_ROUNDLOTS);
                    positionMasterFields.Add(AllocationUIConstants.TRADE_TYPE, AllocationUIConstants.CAPTION_TRADE_TYPE);
                    positionMasterFields.Add(AllocationUIConstants.CAPTION_CURRENCY_NAME, AllocationUIConstants.CAPTION_CURRENCY_NAME);
                    positionMasterFields.Add(AllocationUIConstants.CAPTION_PB, AllocationUIConstants.CAPTION_PB);
                    positionMasterFields.Add(AllocationUIConstants.ALLOCATION_SCHEME_KEY, AllocationUIConstants.CAPTION_ALLOCATION_SCHEME_KEY);

                    int i = 0;
                    foreach (string field in positionMasterFields.Keys)
                    {
                        if (fieldLayout.Fields.IndexOf(field) != -1)
                        {
                            fieldLayout.Fields[field].Label = positionMasterFields[field];
                            fieldLayout.Fields[field].Visibility = Visibility.Visible;
                            fieldLayout.Fields[field].ActualPosition = new FieldPosition(i++, 0, 0, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the percentage and total quantity.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        /// <param name="primeBroker">The prime broker.</param>
        private void UpdatePercentageAndQuantity(DataRecord row, string symbol, string orderSideTagValue, string primeBroker)
        {
            try
            {
                bool primeBrokerCheck = primeBroker == null || row.Cells[AllocationUIConstants.CAPTION_PB].Value.ToString().Equals(primeBroker);
                bool orderSideTagCheck = orderSideTagValue == null || row.Cells[AllocationUIConstants.ORDERSIDE_TAGVALUE].Value.ToString().Equals(orderSideTagValue);
                if (row.Cells[AllocationUIConstants.SYMBOL].Value.ToString().Equals(symbol) && orderSideTagCheck && primeBrokerCheck)
                {
                    row.Cells[AllocationUIConstants.TOTAL_QTY].Value = _totalQty;
                    double quantity = double.Parse(row.Cells[AllocationUIConstants.TOTAL_QUANTITY].Value.ToString());
                    row.Cells[AllocationUIConstants.TOTAL_PERCENTAGE].Value = quantity == 0.0 ? 0 : (quantity / _totalQty) * 100;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods

        #region OnDetaching

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            try
            {
                AssociatedObject.CellChanged -= AssociatedObject_CellChanged;
                AssociatedObject.FieldLayoutInitialized -= AssociatedObject_FieldLayoutInitialized;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion
    }
}
