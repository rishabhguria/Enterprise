using Infragistics.Windows.DataPresenter;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Behaviours
{
    internal class ComplianceAlertsTabBehavior : Behavior<XamDataGrid>
    {
        #region Properties

        /// <summary>
        /// RemoveFiltersProperty
        /// </summary>
        public readonly static DependencyProperty RemoveFiltersProperty = DependencyProperty.Register("RemoveFilters", typeof(bool), typeof(ComplianceAlertsTabBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RemoveFiltersAction));

        /// <summary>
        /// RemoveFilters
        /// </summary>
        public bool RemoveFilters
        {
            get { return (bool)GetValue(RemoveFiltersProperty); }
            set { SetValue(RemoveFiltersProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes the filters.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RemoveFiltersAction(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                ComplianceAlertsTabBehavior gridExtender = (ComplianceAlertsTabBehavior)d;
                XamDataGrid dataGrid = gridExtender.AssociatedObject;

                if (dataGrid != null)
                    dataGrid.ClearCustomizations(CustomizationType.RecordFilters);

                gridExtender.RemoveFilters = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion
    }
}
