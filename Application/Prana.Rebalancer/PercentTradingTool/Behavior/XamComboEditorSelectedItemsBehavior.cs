using Infragistics.Controls.Editors;
using Infragistics.Windows.DataPresenter;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Prana.Rebalancer.PercentTradingTool.Behavior
{

    /// <summary>
    /// Sets the selected items in the view model, ensure two way binding of data
    /// </summary>
    class XamComboEditorSelectedItemsBehavior : Behavior<XamComboEditor>
    {

        /// <summary>
        /// The selected items property
        /// </summary>
        public readonly static DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(XamComboEditorSelectedItemsBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

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

        private bool _isAccountSelected = false;
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
                AssociatedObject.DropDownClosed += AssociatedObjectOnDropDownClosed;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }


        /// <summary>
        /// Finds the parent.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        private XamDataGrid FindParent(DependencyObject child)
        {
            try
            {
                //get parent item
                DependencyObject parentObject = VisualTreeHelper.GetParent(child);

                //we've reached the end of the tree
                if (parentObject == null) return null;

                //check if the parent matches the type we're looking for
                XamDataGrid parent = parentObject as XamDataGrid;
                if (parent != null)
                    return parent;
                else
                    return FindParent(parentObject);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }


        /// <summary>
        /// Associateds the object on drop down closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AssociatedObjectOnDropDownClosed(object sender, EventArgs eventArgs)
        {
            try
            {
                if (_isAccountSelected)
                {
                    XamDataGrid grid = FindParent(AssociatedObject);
                    if (grid.Records[0].IsDataRecord)
                    {
                        DataRecord dataRecord = (DataRecord)(grid.Records[0]);
                        dataRecord.RefreshCellValues();
                    }
                    _isAccountSelected = false;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
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
                if (comboEditor != null)
                {
                    if (SelectedItems != null)
                        comboEditor.SelectedItems = SelectedItems;
                }
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
                if (comboEditor != null) SelectedItems = comboEditor.SelectedItems;
                _isAccountSelected = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
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
                AssociatedObject.DropDownClosed -= AssociatedObjectOnDropDownClosed;
                AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}

