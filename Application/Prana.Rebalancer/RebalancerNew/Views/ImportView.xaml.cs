using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.ViewModels;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : Window
    {
        public ImportView(RebalancerEnums.ImportType importType)
        {
            InitializeComponent();
            if (importType.Equals(RebalancerEnums.ImportType.CustomGroupsImport))
            {
                ImportWindow.Title = "CustomGroup Import";
                ImportTextBlock.Text = "CustomGroup Import";
            }
            else if (importType.Equals(RebalancerEnums.ImportType.CashFlowImport))
            {
                ImportWindow.Title = "CashFlow Import";
                ImportTextBlock.Text = "CashFlow Import";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ImportViewModel customCashFlowViewModel = this.DataContext as ImportViewModel;
            if (customCashFlowViewModel != null)
                customCashFlowViewModel.IsSaveClicked = true;
            this.Close();
        }
    }
}
