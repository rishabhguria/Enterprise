using System.Windows.Controls;

namespace Prana.SM.OTC.View
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
