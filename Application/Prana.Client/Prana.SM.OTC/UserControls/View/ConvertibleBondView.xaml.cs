using System.Windows.Controls;

namespace Prana.SM.OTC.View
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
