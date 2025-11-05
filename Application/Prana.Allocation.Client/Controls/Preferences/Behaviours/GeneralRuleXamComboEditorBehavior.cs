using Infragistics.Controls.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.LogManager;
using System;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.Controls.Preferences.Behaviours
{
    class GeneralRuleXamComboEditorBehavior : Behavior<XamComboEditor>
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
                AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The is enabled cell property
        /// </summary>
        public static readonly DependencyProperty IsEnabledCellProperty = DependencyProperty.Register("IsEnabledCell", typeof(bool), typeof(GeneralRuleXamComboEditorBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled cell.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled cell; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledCell
        {
            get { return (bool)GetValue(IsEnabledCellProperty); }
            set { SetValue(IsEnabledCellProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the SelectionChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                XamComboEditor comboEditor = (sender as XamComboEditor);
                var selectedItemCaption = ((Infragistics.Windows.DataPresenter.DataItemPresenter)(((System.Windows.FrameworkElement)(comboEditor)).TemplatedParent)).Field.Label;
                if (selectedItemCaption.ToString().Equals(AllocationUIConstants.CAPTION_EXCHANGE_SELECTOR) || selectedItemCaption.ToString().Equals(AllocationUIConstants.CAPTION_ASSET_SELECTOR) || selectedItemCaption.ToString().Equals(AllocationUIConstants.CAPTION_PR_SELECTOR) || selectedItemCaption.ToString().Equals(AllocationUIConstants.CAPTION_ORDER_SIDE_SELECTOR))
                    IsEnabledCell = !IsEnabledCell;
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
                AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
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
