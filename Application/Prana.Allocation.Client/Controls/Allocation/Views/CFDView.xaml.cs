using Prana.Allocation.Client.Controls.Allocation.ViewModels;
using System.Windows.Controls;

namespace Prana.Allocation.Client.Controls.Allocation.Views
{
    /// <summary>
    /// Interaction logic for CFDView.xaml
    /// </summary>
    public partial class CFDView : UserControl
    {
        public CFDView()
        {
            InitializeComponent();
            DataContext = new CFDViewModel();
        }
    }
}
