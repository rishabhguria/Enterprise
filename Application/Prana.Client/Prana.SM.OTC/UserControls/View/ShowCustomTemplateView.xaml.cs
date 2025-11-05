using System.Windows.Controls;

namespace Prana.SM.OTC.View
{
    /// <summary>
    /// Interaction logic for ShowCustomTemplateView.xaml
    /// </summary>
    public partial class ShowCustomTemplateView : UserControl
    {
        public ShowCustomTemplateView()
        {
            InitializeComponent();
            DataContext = new ShowCustomTemplateViewModel();
        }
    }
}
