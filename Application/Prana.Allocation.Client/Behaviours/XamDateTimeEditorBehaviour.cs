using Infragistics.Windows.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;

namespace Prana.Allocation.Client.Behaviours
{
    class XamDateTimeEditorBehaviour : Behavior<XamDateTimeEditor>
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
                AssociatedObject.EditModeEnded += AssociatedObject_EditModeEnded;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Methods

        /// <summary>
        /// Handles the EditModeEnded event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.Editors.Events.EditModeEndedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_EditModeEnded(object sender, Infragistics.Windows.Editors.Events.EditModeEndedEventArgs e)
        {
            try
            {
                XamDateTimeEditor dateTimeEditor = (sender as XamDateTimeEditor);
                if (dateTimeEditor.DisplayText == string.Empty)
                    dateTimeEditor.Value = DateTime.Now.ToString();
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
                AssociatedObject.EditModeEnded -= AssociatedObject_EditModeEnded;
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
