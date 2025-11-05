using Infragistics.Windows.DockManager;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Behaviours
{
    class RebalancerDockPanelBehavior : Behavior<ContentPane>
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
                AssociatedObject.Closed += ContentPane_Closed;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        /// <summary>
        /// ClearButtonActionProp
        /// </summary>
        public static readonly DependencyProperty ClearButtonActionProp = DependencyProperty.Register("ClearButtonAction", typeof(bool), typeof(RebalancerDockPanelBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PaneCloseAction));

        /// <summary>
        /// ClearButtonAction
        /// </summary>
        public bool ClearButtonAction
        {
            get { return (bool)GetValue(ClearButtonActionProp); }
            set { SetValue(ClearButtonActionProp, value); }
        }

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
                ContentPane pane = (ContentPane)(e.Source);
                if (pane.IsPinned && pane.PaneLocation != PaneLocation.Floating)
                {
                    e.Cancel = true;
                    pane.IsPinned = false;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// ContentPane_Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentPane_Closed(object sender, Infragistics.Windows.DockManager.Events.PaneClosedEventArgs e)
        {
            try
            {
                ContentPane cp = e.OriginalSource as ContentPane;

                if (cp.PaneLocation == PaneLocation.FloatingOnly)
                {
                    cp.ExecuteCommand(ContentPaneCommands.ChangeToDockable);
                    cp.ExecuteCommand(ContentPaneCommands.ToggleDockedState);
                    cp.Visibility = Visibility.Visible;
                    return;
                }
                else if (cp.PaneLocation == PaneLocation.Floating || cp.PaneLocation == PaneLocation.Unpinned)
                {
                    cp.IsPinned = false;
                    cp.ExecuteCommand(ContentPaneCommands.ToggleDockedState);
                    cp.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// PaneCloseAction
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void PaneCloseAction(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                RebalancerDockPanelBehavior rebalancerDockPanelBehavior = d as RebalancerDockPanelBehavior;
                if (rebalancerDockPanelBehavior.AssociatedObject != null && rebalancerDockPanelBehavior.AssociatedObject.PaneLocation == PaneLocation.Floating && rebalancerDockPanelBehavior.ClearButtonAction)
                {
                    rebalancerDockPanelBehavior.AssociatedObject.IsPinned = false;
                    rebalancerDockPanelBehavior.AssociatedObject.ExecuteCommand(ContentPaneCommands.ToggleDockedState);
                    rebalancerDockPanelBehavior.AssociatedObject.Visibility = Visibility.Visible;
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
                AssociatedObject.Closed -= ContentPane_Closed;
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