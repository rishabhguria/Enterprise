using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Microsoft.Xaml.Behaviors;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Rebalancer.PercentTradingTool.ViewModel;
using Prana.Utilities.UI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Prana.Rebalancer.PercentTradingTool.Behavior
{
    /// <summary>
    /// Aggregates all the data errors in the grid
    /// </summary>
    public class XamDataGridValidationErrorAggregator : Behavior<XamDataGrid>
    {
        /// <summary>
        /// The view model property
        /// </summary>
        public readonly static DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PercentTradingToolViewModel), typeof(XamDataGridValidationErrorAggregator), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public PercentTradingToolViewModel ViewModel
        {
            get { return GetValue(ViewModelProperty) as PercentTradingToolViewModel; }
            set { SetValue(ViewModelProperty, value); }
        }

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
                base.OnAttached();
                XamValidationHelper.AddXamDataErrorEventHandler(AssociatedObject, OnXamValidationError);
                AssociatedObject.InitializeRecord += OnInitializeRecord;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [initialize record].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="InitializeRecordEventArgs"/> instance containing the event data.</param>
        private void OnInitializeRecord(object sender, InitializeRecordEventArgs e)
        {
            try
            {
                foreach (var record in AssociatedObject.Records)
                {
                    if (record.IsDataRecord)
                    {
                        DataRecord dataRecord = (DataRecord)record;
                        if (dataRecord.HasDataError)
                            ViewModel.ValidationHelper.AddError(dataRecord);
                        else
                            ViewModel.ValidationHelper.RemoveError(dataRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [xam validation error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="XamValidationErrorEventArgs"/> instance containing the event data.</param>
        private void OnXamValidationError(object sender, XamValidationErrorEventArgs e)
        {
            try
            {
                if (ViewModel != null)
                {
                    WPFErrorObject wpfErrorObject = ViewModel.ErrorObject;
                    if (e.Action == ValidationErrorEventAction.Added)
                    {
                        wpfErrorObject.ErrorDescription = e.DataError;

                    }
                    else
                    {
                        wpfErrorObject.ErrorDescription = string.Empty;
                        wpfErrorObject.ErrorCount = 0;
                        wpfErrorObject.ErrorMsg = String.Empty;
                    }
                    AddRemoveCustomErrorsInMessage();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void AddRemoveCustomErrorsInMessage()
        {
            ViewModel.SetCustomErrorMessages("Calculation Service Disconnected", !ViewModel.IsExpnlServiceConnected);
            ViewModel.SetCustomErrorMessages("Non Equity Asset Category", !ViewModel.IsEquitySymbol);

        }

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
                XamValidationHelper.RemoveXamDataErrorEventHandler(AssociatedObject, OnXamValidationError);
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}