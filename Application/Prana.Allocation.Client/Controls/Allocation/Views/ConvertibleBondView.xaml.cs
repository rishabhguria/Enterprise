using Prana.Allocation.Client.Controls.Allocation.ViewModels;
using System.Windows.Controls;

namespace Prana.Allocation.Client.Controls.Allocation.Views
{
    /// <summary>
    /// Interaction logic for ConvertibleBondView.xaml
    /// </summary>
    public partial class ConvertibleBondView : UserControl
    {
        public ConvertibleBondView()
        {
            InitializeComponent();
            DataContext = new ConvertibleBondViewModel();
        }
    }
}
