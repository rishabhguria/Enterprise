using Infragistics.Windows.DataPresenter.Events;
using Infragistics.Windows.Editors;
using Prana.Rebalancer.RebalancerNew.ViewModels;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for CustomCashFlowView.xaml
    /// </summary>
    public partial class CustomCashFlowView : Window
    {
        public CustomCashFlowView()
        {
            InitializeComponent();
        }

        private void XamNumericEditor_EditModeStarted(object sender, Infragistics.Windows.Editors.Events.EditModeStartedEventArgs e)
        {
            ((XamNumericEditor)sender).SelectAll();
        }

        private void CustomCashFlow_OnSummaryResultChanged(object sender, SummaryResultChangedEventArgs e)
        {
            CashFlowTextBlock.Text = e.SummaryResult.Value.ToString();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            CustomCashFlowViewModel customCashFlowViewModel = this.DataContext as CustomCashFlowViewModel;
            if (customCashFlowViewModel != null)
                customCashFlowViewModel.IsOkButtonClicked = true;
            this.Close();
        }
    }
}
