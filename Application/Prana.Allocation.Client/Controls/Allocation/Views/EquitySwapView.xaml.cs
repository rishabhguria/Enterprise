using System.Windows.Controls;

namespace Prana.Allocation.Client.Controls.Allocation.Views
{
    /// <summary>
    /// Interaction logic for EquitySwapView.xaml
    /// </summary>
    public partial class EquitySwapView : UserControl
    {
        public EquitySwapView()
        {
            InitializeComponent();

            DataContext = new Prana.Allocation.Client.Controls.Allocation.ViewModels.EquitySwapViewModel();
        }
    }
}
