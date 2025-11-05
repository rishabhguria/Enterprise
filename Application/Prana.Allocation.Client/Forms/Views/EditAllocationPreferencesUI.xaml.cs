using Prana.Allocation.Client.Forms.ViewModels;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prana.Allocation.Client.Forms.Views
{
    /// <summary>
    /// Interaction logic for EditAllocationPreferencesUI.xaml
    /// </summary>
    public partial class EditAllocationPreferencesUI : Window
    {
        /// <summary>
        /// Gets or sets the edit allocation preferences UI view model.
        /// </summary>
        /// <value>
        /// The edit allocation preferences UI view model.
        /// </value>
        public EditAllocationPreferencesUIViewModel EditAllocationPreferencesUIViewModel
        {
            set { this.DataContext = value; }
            get { return (EditAllocationPreferencesUIViewModel)this.DataContext; }
        }

        public EditAllocationPreferencesUI()
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

    }
}
