using Infragistics.Controls.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Helper;
using Prana.LogManager;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.Behaviours
{
    class XamComboEditorBehavior : Behavior<XamComboEditor>
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
                AssociatedObject.Loaded += AssociatedObject_Loaded;
                AssociatedObject.IsMouseDirectlyOverChanged += AssociatedObject_IsMouseDirectlyOverChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The selected items property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(XamComboEditorBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The selected values property
        /// </summary>
        public static readonly DependencyProperty SelectedValuesProperty = DependencyProperty.Register("SelectedValues", typeof(string), typeof(XamComboEditorBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>
        /// The selected items.
        /// </value>
        public ObservableCollection<object> SelectedItems
        {
            get { return GetValue(SelectedItemsProperty) as ObservableCollection<object>; }
            set { SetValue(SelectedItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected values.
        /// </summary>
        /// <value>
        /// The selected values.
        /// </value>
        public string SelectedValues
        {
            get { return GetValue(SelectedValuesProperty) as string; }
            set { SetValue(SelectedValuesProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the Loaded event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                XamComboEditor comboEditor = (sender as XamComboEditor);
                if (SelectedItems != null)
                    comboEditor.SelectedItems = SelectedItems;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

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
                SelectedItems = comboEditor.SelectedItems;

                SelectedValues = CommonAllocationMethods.GetStringFromCollection(SelectedItems);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

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
                    (sender as XamComboEditor).IsDropDownOpen = false;
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
                AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.IsMouseDirectlyOverChanged -= AssociatedObject_IsMouseDirectlyOverChanged;
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
