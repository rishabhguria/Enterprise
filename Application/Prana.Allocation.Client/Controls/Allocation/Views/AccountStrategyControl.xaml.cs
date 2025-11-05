using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Prana.Allocation.Client.Controls.Allocation.Views
{
    /// <summary>
    /// Interaction logic for AccountStrategyControl.xaml
    /// </summary>
    public partial class AccountStrategyControl : UserControl
    {
        public AccountStrategyControl()
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
