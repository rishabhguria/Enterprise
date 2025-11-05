using Infragistics.Windows.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;

namespace Prana.SM.OTC.Behaviours
{
    class XamNumericEditorBehaviour : Behavior<XamNumericEditor>
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
                AssociatedObject.TextChanged += AssociatedObject_TextChanged;
                AssociatedObject.EditModeStarted += AssociatedObject_EditModeStarted;
                AssociatedObject.IsEnabledChanged += AssociatedObject_IsEnabledChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Methods

        /// <summary>
        /// Handles the EditModeStarted event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.Editors.Events.EditModeStartedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_EditModeStarted(object sender, Infragistics.Windows.Editors.Events.EditModeStartedEventArgs e)
        {
            try
            {
                AssociatedObject.SelectAll();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the IsEnabledChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_IsEnabledChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (AssociatedObject.Value.ToString().Equals("0"))
                    AssociatedObject.Text = "0.0";
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void AssociatedObject_TextChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<string> e)
        {
            try
            {
                XamNumericEditor selectedTextBox = (XamNumericEditor)e.Source;
                if (string.IsNullOrWhiteSpace(selectedTextBox.Text))
                    selectedTextBox.Text = "0";
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
                AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
                AssociatedObject.EditModeStarted -= AssociatedObject_EditModeStarted;
                AssociatedObject.IsEnabledChanged -= AssociatedObject_IsEnabledChanged;
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
