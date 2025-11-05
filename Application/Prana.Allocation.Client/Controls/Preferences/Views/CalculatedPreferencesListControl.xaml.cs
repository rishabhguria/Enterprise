using Infragistics.Windows.DataPresenter;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prana.Allocation.Client.Controls.Preferences.Views
{
    /// <summary>
    /// Interaction logic for CalculatedPreferencesListControl.xaml
    /// </summary>
    public partial class CalculatedPreferencesListControl : UserControl
    {
        public CalculatedPreferencesListControl()
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
    }
}
