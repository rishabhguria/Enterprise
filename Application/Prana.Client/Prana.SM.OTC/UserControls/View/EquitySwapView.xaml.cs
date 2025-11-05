using System.Windows.Controls;

namespace Prana.SM.OTC.View
{
    /// <summary>
    /// Interaction logic for EquitySwapView.xaml
    /// </summary>
    public partial class EquitySwapView : UserControl
    {
        public EquitySwapView()
        {
            InitializeComponent();

            DataContext = new EquitySwapViewModel();
        }
    }
}
