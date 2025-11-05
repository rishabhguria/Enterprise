using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Prana.Rebalancer.PercentTradingTool.Behavior
{
    /// <summary>
    /// Reposition the error popup according to window position and size
    /// </summary>
    public class RepositionPopupBehavior : Behavior<Popup>
    {
        #region Protected Methods
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
                var window = Window.GetWindow(AssociatedObject.PlacementTarget);
                if (window == null) { return; }
                window.LocationChanged += OnLocationChanged;
                window.SizeChanged += OnSizeChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
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
                base.OnDetaching();
                var window = Window.GetWindow(AssociatedObject.PlacementTarget);
                if (window == null) { return; }
                window.LocationChanged -= OnLocationChanged;
                window.SizeChanged -= OnSizeChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Called when [location changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnLocationChanged(object sender, EventArgs e)
        {
            try
            {

                var offset = AssociatedObject.HorizontalOffset;
                AssociatedObject.HorizontalOffset = offset + 1;
                AssociatedObject.HorizontalOffset = offset;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [size changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                var offset = AssociatedObject.HorizontalOffset;
                AssociatedObject.HorizontalOffset = offset + 1;
                AssociatedObject.HorizontalOffset = offset;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Private Methods
    }
}
