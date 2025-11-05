using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.Behaviours
{
    class XamDataGridClearFilters : Behavior<XamDataGrid>
    {
        #region OnAttach Events

        /// <summary>
        /// Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            try
            {
                base.OnAttached();
                AssociatedObject.FieldPositionChanged += FieldPositionChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The is filters cleared property
        /// </summary>
        public static readonly DependencyProperty IsFiltersClearedProperty = DependencyProperty.Register("IsFiltersCleared", typeof(bool), typeof(XamDataGridClearFilters), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ClearFilters));

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refresh after get data.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is refresh after get data; otherwise, <c>false</c>.
        /// </value>
        public bool IsFiltersCleared
        {
            get { return (bool)GetValue(IsFiltersClearedProperty); }
            set { SetValue(IsFiltersClearedProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clears the filters.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ClearFilters(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                XamDataGridClearFilters gridExtender = (XamDataGridClearFilters)d;
                XamDataGrid dataGrid = gridExtender.AssociatedObject;
                dataGrid.ClearCustomizations(CustomizationType.RecordFilters);
                gridExtender.IsFiltersCleared = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Fields the position changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FieldPositionChangedEventArgs"/> instance containing the event data.</param>
        private void FieldPositionChanged(object sender, FieldPositionChangedEventArgs e)
        {
            try
            {
                if (e.ChangeReason == FieldPositionChangeReason.Hidden)
                    ((XamDataGrid)sender).FieldLayouts[0].RecordFilters[e.Field].Clear();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods

        #region OnDetach Events

        /// <summary>
        /// Called when [detaching].
        /// </summary>
        protected override void OnDetaching()
        {
            try
            {
                AssociatedObject.FieldPositionChanged -= FieldPositionChanged;
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
