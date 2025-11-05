using Infragistics.Windows.DockManager;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Controls.Allocation.Views;
using Prana.LogManager;
using System;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Forms.Behaviours
{
    class AllocationClientDockPanelBehavior : Behavior<ContentPane>
    {
        #region OnAttach Events

        /// <summary>
        /// Attached event property
        /// </summary>
        protected override void OnAttached()
        {
            try
            {
                base.OnAttached();
                AssociatedObject.Closing += AssociatedObject_Closing;
                AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }       

        #endregion OnAttach Events

        #region Methods

        /// <summary>
        /// If user click on close button of the content pane, then unpin the selected content pane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Closing(object sender, Infragistics.Windows.DockManager.Events.PaneClosingEventArgs e)
        {
            try
            {
                e.Cancel = true;
                if (((ContentPane)(((RoutedEventArgs)e).Source)).IsPinned)
                    ((ContentPane)(((RoutedEventArgs)e).Source)).IsPinned = false;
                else
                {
                    XamDockManager dockManager = (XamDockManager)((UnpinnedTabArea)((ContentPane)(((RoutedEventArgs)(e)).Source)).Parent).TemplatedParent;
                    if (dockManager != null)
                    {
                        ContentPane allocatedGridPane = dockManager.GetPanes(PaneNavigationOrder.ActivationOrder).FirstOrDefault(x => x.Name == "AllocationGrids");
                        if (allocatedGridPane != null)
                            allocatedGridPane.Activate();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the visibility change event of the associated ContentPane.      
        /// </summary>
        /// <param name="sender">The source of the event, expected to be a ContentPane.</param>
        /// <param name="e">Provides data about the visibility change.</param>
        private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (sender is ContentPane contentPane && contentPane.IsVisible && !contentPane.IsPinned && contentPane.Content is TradeAttributeBulkChangeControl tradeControl)
                {
                    // Reset scroll position to the top-left when the pane becomes visible (and is not pinned)
                    tradeControl.TradeAttributeScrollViewer?.ScrollToTop();
                    tradeControl.TradeAttributeScrollViewer?.ScrollToLeftEnd();
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
                AssociatedObject.Closing -= AssociatedObject_Closing;
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
