using Prana.Allocation.Client.Controls.Allocation.ViewModels;
using System.Windows.Controls;

namespace Prana.Allocation.Client.Controls.Allocation.Views
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
