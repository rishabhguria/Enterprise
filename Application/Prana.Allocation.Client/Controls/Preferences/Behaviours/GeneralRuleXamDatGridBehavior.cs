using Infragistics.Windows.DataPresenter;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.LogManager;
using System;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.Behaviours
{
    class GeneralRuleXamDatGridBehavior : Behavior<XamDataGrid>
    {
        #region OnAttach Events

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
                AssociatedObject.InitializeRecord += AssociatedObject_InitializeRecord;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The disable cells
        /// </summary>
        public static readonly DependencyProperty DisableCell = DependencyProperty.Register("IsDisableCell", typeof(bool), typeof(GeneralRuleXamDatGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DisableCells));

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is load layout.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is load layout; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisableCell
        {
            get { return (bool)GetValue(DisableCell); }
            set { SetValue(DisableCell, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the InitializeRecord event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.DataPresenter.Events.InitializeRecordEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_InitializeRecord(object sender, Infragistics.Windows.DataPresenter.Events.InitializeRecordEventArgs e)
        {
            try
            {
                if (e.Record != null)
                    DisableCellOfRecord(e.Record);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Disables the cell general rule.
        /// </summary>
        private static void DisableCellOfRecord(Record record)
        {
            try
            {
                if (record.IsDataRecord)
                {
                    DataRecord dataRecord = (DataRecord)record;
                    if (dataRecord.Cells[AllocationUIConstants.EXCHANGE_OPERATOR].Value.ToString().Equals(AllocationUIConstants.CONST_ALL))
                        dataRecord.Cells[AllocationUIConstants.EXCHANGE_LIST].IsEnabled = false;
                    else
                        dataRecord.Cells[AllocationUIConstants.EXCHANGE_LIST].IsEnabled = true;

                    if (dataRecord.Cells[AllocationUIConstants.ORDER_SIDE_OPERATOR].Value.ToString().Equals(AllocationUIConstants.CONST_ALL))
                        dataRecord.Cells[AllocationUIConstants.ORDER_SIDE_LIST].IsEnabled = false;
                    else
                        dataRecord.Cells[AllocationUIConstants.ORDER_SIDE_LIST].IsEnabled = true;

                    if (dataRecord.Cells[AllocationUIConstants.ASSET_OPERATOR].Value.ToString().Equals(AllocationUIConstants.CONST_ALL))
                        dataRecord.Cells[AllocationUIConstants.ASSET_LIST].IsEnabled = false;
                    else
                        dataRecord.Cells[AllocationUIConstants.ASSET_LIST].IsEnabled = true;

                    if (dataRecord.Cells[AllocationUIConstants.PR_OPERATOR].Value.ToString().Equals(AllocationUIConstants.CONST_ALL))
                        dataRecord.Cells[AllocationUIConstants.PR_LIST].IsEnabled = false;
                    else
                        dataRecord.Cells[AllocationUIConstants.PR_LIST].IsEnabled = true;
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
        /// Disables the cells.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DisableCells(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                GeneralRuleXamDatGridBehavior gridExtender = obj as GeneralRuleXamDatGridBehavior;
                if (gridExtender.AssociatedObject != null)
                {
                    XamDataGrid dataGrid = gridExtender.AssociatedObject;
                    if (dataGrid.ActiveRecord != null)
                        DisableCellOfRecord(dataGrid.ActiveRecord);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods

        #region OnDetach Events

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
                AssociatedObject.InitializeRecord -= AssociatedObject_InitializeRecord;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnDetach Events
    }
}
