using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prana.Rebalancer.RebalancerNew.ViewModels;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for AddModelPortfolioView.xaml
    /// </summary>
    public partial class AddModelPortfolioView : UserControl
    {
        public AddModelPortfolioView()
        {
            InitializeComponent();
            UserControl.DataContext = new AddModelPortfolioViewModel();
        }

        private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            AddButtonsPanel.Visibility = Visibility.Collapsed;
            InfoPanel.Visibility = Visibility.Visible;
        }

        private void OnNewButtonClicked(object sender, RoutedEventArgs e)
        {
            AddButtonsPanel.Visibility = Visibility.Visible;
            InfoPanel.Visibility = Visibility.Collapsed;
        }
    }
}
