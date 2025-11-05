using System.Windows;
using System.Windows.Controls;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for CustomGroupsView.xaml
    /// </summary>
    public partial class CustomGroupsView : UserControl
    {
        public CustomGroupsView()
        {
            InitializeComponent();
        }

        private void ContextMenu_OnOpened(object sender, RoutedEventArgs e)
        {
            (sender as ContextMenu).DataContext = CustomGroupUserControl.DataContext;
        }
    }
}
