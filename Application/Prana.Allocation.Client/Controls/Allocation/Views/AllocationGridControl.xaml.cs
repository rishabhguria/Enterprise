using Infragistics.Windows.DataPresenter;
using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prana.Allocation.Client.Controls.Allocation.Views
{
    /// <summary>
    /// Interaction logic for AllocationGrid.xaml
    /// </summary>
    public partial class AllocationGridControl : UserControl
    {
        public AllocationGridControl()
        {
            InitializeComponent();
            AddInfragisticsSourceDictionary();

        }

        private void AddInfragisticsSourceDictionary()
        {
            try
            {
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    ResourceDictionary rd = new ResourceDictionary();
                    rd.Source = new Uri(@"/Prana.Allocation.Client;component/Themes/IG/IG.xamDataPresenter.xaml", UriKind.Relative);
                    this.Resources.MergedDictionaries.Add(rd);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #region Field Chooser
        //We cannot remove this code from here. Because logic is little bit tricky. We will remove this code after getting best way to search in Field chooser   

        /// <summary>
        /// The filtered un allocated grid fields
        /// </summary>
        private ReadOnlyObservableCollection<FieldChooserEntry> _filteredUnAllocatedGridFields;

        /// <summary>
        /// The filtered allocated fields
        /// </summary>
        private ReadOnlyObservableCollection<FieldChooserEntry> _filteredAllocatedFields;

        /// <summary>
        /// </summary>
        ListBox _listBox;

        /// <summary>
        /// The filter text
        /// </summary>
        string _filterText;

        /// <summary>
        /// Handles the TextChanged event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox textBox = (sender as TextBox);
                var dependencyProperty = textBox.Parent;
                _listBox = Infragistics.Windows.Utilities.GetDescendantFromType(dependencyProperty, typeof(ListBox), false) as ListBox;
                _filterText = textBox.Text;
                this.FilterItems(_listBox, _filterText, textBox.Name);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the 1 event of the TextBox_Loaded control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var grid = (sender as TextBox).Parent;
                var lb = Infragistics.Windows.Utilities.GetDescendantFromType(grid, typeof(ListBox), false) as ListBox;
                if (lb.Items.Count > 0)
                    lb.ScrollIntoView(lb.Items[0]);
                (sender as TextBox).Text = String.Empty;
                (sender as TextBox).Focus();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the GridUnAllocatedListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GridUnAllocatedListBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _filteredUnAllocatedGridFields = ((sender as ListBox).TemplatedParent as FieldChooser).CurrentFields;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the 2 event of the PART_FieldsListBox_Loaded control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GridAllocatedListBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                _filteredAllocatedFields = ((sender as ListBox).TemplatedParent as FieldChooser).CurrentFields;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Filters the items.
        /// </summary>
        /// <param name="lb">The lb.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="textBoxName">Name of the text box.</param>
        private void FilterItems(ListBox lb, string filter, string textBoxName)
        {
            try
            {
                if (lb == null || lb.ItemsSource == null)
                    return;

                IEnumerable<FieldChooserEntry> filteredItems = new List<FieldChooserEntry>();

                if (textBoxName.Equals(AllocationUIConstants.FIELD_CHOOSER_ALLOCATED_TEXTBOX))
                    filteredItems = _filteredAllocatedFields.Where(f => f.Field.Label.ToString().ToLower().Contains(filter.ToLower()));
                else
                    filteredItems = _filteredUnAllocatedGridFields.Where(f => f.Field.Label.ToString().ToLower().Contains(filter.ToLower()));

                lb.ItemsSource = filteredItems;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the SelectedItemChanged event of the fieldGroupSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedPropertyChangedEventArgs{System.Object}"/> instance containing the event data.</param>
        private void fieldGroupSelector_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                _filteredAllocatedFields = ((sender as Infragistics.Windows.Editors.XamComboEditor).TemplatedParent as FieldChooser).CurrentFields;
                this.FilterItems(_listBox, _filterText, AllocationUIConstants.FIELD_CHOOSER_ALLOCATED_TEXTBOX);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion

        /// <summary>
        /// Handles the MouseRightButtonDown event of the DataRecordPresenter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void DataRecordPresenter_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((sender as DataRecordPresenter).Record.IsDataRecord)
                {
                    if ((sender as DataRecordPresenter).DataRecord.DataItem != null)
                        (sender as DataRecordPresenter).DataPresenter.ActiveDataItem = (sender as DataRecordPresenter).DataRecord.DataItem;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the DataRecordPresenter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void DataRecordPresenter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRecordPresenter presenter = sender as DataRecordPresenter;
                if (presenter != null && presenter.Record.IsDataRecord && presenter.DataPresenter.ActiveCell != null)
                {
                    if (presenter.DataPresenter.ActiveDataItem is TaxLot && presenter.DataPresenter.ActiveCell.Field.Name.Equals(AllocationUIConstants.EXTERNAL_TRANS_ID))
                    {
                        var viewModel = this.DataContext;
                        if (((ViewModels.AllocationGridControlViewModel)(viewModel)).AllowEditGrid)
                            ((ViewModels.AllocationGridControlViewModel)(viewModel)).IsOpenAddAndUpdateExtenalTransactionUi = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
