using Infragistics.Windows.Editors;
using System.Windows.Controls;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for ModelPortfolioView.xaml
    /// </summary>
    public partial class ModelPortfolioView : UserControl
    {
        public ModelPortfolioView()
        {
            InitializeComponent();
        }

        private void XamNumericEditor_EditModeStarted(object sender, Infragistics.Windows.Editors.Events.EditModeStartedEventArgs e)
        {
            ((XamNumericEditor)sender).SelectAll();
        }

        private void XamNumericEditor_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text) && e.Text.Contains("-"))
            {
                e.Handled = true;
            }
        }
    }
}
