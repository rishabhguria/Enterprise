using Infragistics.Controls.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.Behaviours
{
    /// <summary>
    /// Behavior to add select all functionality to the combo control
    /// </summary>
    public class XamComboEditorSelectAllBehavior : Behavior<XamComboEditor>
    {
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
                AssociatedObject.DropDownOpening += AssociatedObject_DropDownOpening;
                AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        void AssociatedObject_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                SetupSelectAllItem();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
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
                ObservableCollection<Account> source = (ObservableCollection<Account>)AssociatedObject.ItemsSource;
                Account selectedAccount = ((Account)AssociatedObject.SelectedItem);
                if (selectedAccount != null && (selectedAccount.FullName.Equals(AllocationClientConstants.LIT_SELECT_ALL) || selectedAccount.FullName.Equals(AllocationClientConstants.LIT_UNSELECT_ALL)))
                {
                    AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
                    if (selectedAccount.FullName.Equals(AllocationClientConstants.LIT_SELECT_ALL))
                    {
                        selectedAccount.FullName = AllocationClientConstants.LIT_UNSELECT_ALL;
                        for (int i = 1; i < AssociatedObject.Items.Count; i++)
                        {
                            AssociatedObject.Items[i].IsSelected = true;
                        }
                    }
                    else
                    {
                        selectedAccount.FullName = AllocationClientConstants.LIT_SELECT_ALL;
                        for (int i = 1; i < AssociatedObject.Items.Count; i++)
                        {
                            AssociatedObject.Items[i].IsSelected = false;
                        }
                    }
                    AssociatedObject.Items[0].IsSelected = false;
                    AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
                }
                else
                {
                    SetupSelectAllItem();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetupSelectAllItem()
        {
            try
            {
                ObservableCollection<Account> source = (ObservableCollection<Account>)AssociatedObject.ItemsSource;
                if (AssociatedObject.SelectedItems.Count == source.Count - 1)
                {
                    source[0].FullName = AllocationClientConstants.LIT_UNSELECT_ALL;
                }
                else
                {
                    source[0].FullName = AllocationClientConstants.LIT_SELECT_ALL;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
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
                AssociatedObject.DropDownOpened -= AssociatedObject_DropDownOpening;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Updates the binding sources.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void UpdateBindingSources(DependencyObject obj)
        {
            try
            {
                foreach (DependencyProperty depProperty in EnumerateDependencyProperties(obj))
                {
                    //check whether the submitted object provides a bound property
                    //that matches the property parameters
                    BindingExpression be =
                      BindingOperations.GetBindingExpression(obj, depProperty);
                    if (be != null) be.UpdateSource();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Enumerates the dependency properties.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IEnumerable EnumerateDependencyProperties(DependencyObject element)
        {
            LocalValueEnumerator lve = element.GetLocalValueEnumerator();

            while (lve.MoveNext())
            {
                LocalValueEntry entry = lve.Current;
                if (BindingOperations.IsDataBound(element, entry.Property))
                {
                    yield return entry.Property;
                }
            }
        }
    }
}
