using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Infragistics.Controls.Editors;

namespace Nirvana.TestAutomation.TestExecutor
{
    internal class XamComboEditorSelectedItemsBehavior : Behavior<XamComboEditor>
    {
        /// <summary>
        /// Sets the selected items in the view model, ensure two way binding of data
        /// </summary>

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
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// The selected items property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("UserItems", typeof(ObservableCollection<object>), typeof(XamComboEditorSelectedItemsBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The selected values property
        /// </summary>
        public static readonly DependencyProperty SelectedValuesProperty = DependencyProperty.Register("SelectedValues", typeof(string), typeof(XamComboEditorSelectedItemsBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>
        /// The selected items.
        /// </value>
        public ObservableCollection<object> UserItems
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

        /// <summary>
        /// Handles the Loaded event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                XamComboEditor comboEditor = (sender as XamComboEditor);
                if (UserItems != null)
                    comboEditor.SelectedItems = UserItems;
            }
            catch
            {
                throw;
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
                UserItems = comboEditor.SelectedItems;

                SelectedValues = GetStringFromCollection(UserItems);
            }
            catch
            {
                throw;
            }
        }

        public static string GetStringFromCollection(ObservableCollection<object> selectedValue)
        {
            try
            {
                if (selectedValue.Count > 0)
                {
                    List<string> values = new List<string>();

                    if (selectedValue[0] is KeyValuePair<int, string>)
                    {
                        foreach (KeyValuePair<int, string> kvp in selectedValue)
                            values.Add(kvp.Value);
                    }
                    else if (selectedValue[0] is KeyValuePair<string, string>)
                    {
                        foreach (KeyValuePair<string, string> kvp in selectedValue)
                            values.Add(kvp.Value);
                    }
                    else if (selectedValue[0] is string)
                    {
                        foreach (string kvp in selectedValue)
                            values.Add(kvp);
                    }
                    return string.Join(",", values.ToArray());
                }
            }
            catch
            {
                throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Handles the IsMouseDirectlyOverChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(e.NewValue))
                {
                    (sender as XamComboEditor).IsDropDownOpen = false;
                }
            }
            catch
            {
                //bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                // if (rethrow)
                throw;
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
                AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.IsMouseDirectlyOverChanged -= AssociatedObject_IsMouseDirectlyOverChanged;
                base.OnDetaching();
            }
            catch
            {
                throw;
            }
        }
    }
}