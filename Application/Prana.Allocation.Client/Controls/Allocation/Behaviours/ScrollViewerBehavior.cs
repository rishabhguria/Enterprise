using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Windows.Controls;

namespace Prana.Allocation.Client.Controls.Allocation.Behaviours
{
    class ScrollViewerBehavior : Behavior<ScrollViewer>
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
                AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Methods

        /// <summary>
        /// Handles the PreviewMouseWheel event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {
                ScrollViewer scrollviewer = sender as ScrollViewer;
                if (e.Delta > 0)
                    scrollviewer.LineUp();
                else
                    scrollviewer.LineDown();
                e.Handled = true;
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
                AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
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
