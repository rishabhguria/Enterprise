using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Windows;
using System.Windows.Controls;
namespace Prana.Allocation.Client.Behaviours
{
    public class CustomValueComboBoxBehavior : Behavior<ComboBox>
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
                AssociatedObject.IsMouseDirectlyOverChanged += AssociatedObject_IsMouseDirectlyOverChanged;
                AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the LostFocus event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.SelectedItem == null || String.IsNullOrWhiteSpace(AssociatedObject.SelectedItem.ToString()))
            {
                string newItem = AssociatedObject.Text;
                AssociatedObject.SelectedItem = newItem;
            }
        }

        #endregion OnAttach Events

        #region Methods

        /// <summary>
        /// Handles the IsMouseDirectlyOverChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(e.NewValue))
                {
                    AssociatedObject.IsDropDownOpen = false;
                    AssociatedObject.ReleaseMouseCapture();
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
                AssociatedObject.IsMouseDirectlyOverChanged -= AssociatedObject_IsMouseDirectlyOverChanged;
                AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
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

