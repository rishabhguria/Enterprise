using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.Views
{
    /// <summary>
    /// Interaction logic for BulkChangeControl.xaml
    /// </summary>
    public partial class BulkChangeControl : Window
    {
        public BulkChangeControl()
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
