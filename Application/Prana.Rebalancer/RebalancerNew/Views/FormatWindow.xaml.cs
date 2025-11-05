using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for FormatWindow.xaml
    /// </summary>
    public partial class FormatWindow : Window
    {
        public FormatWindow(RebalancerEnums.ImportType importFormatType)
        {
            InitializeComponent();
            if (importFormatType.Equals(RebalancerEnums.ImportType.CustomGroupsImport))
            {
                FormatWindowName.Title = "CustomGroup Import Format";
                FormatTextBlock.Text = "CustomGroup Import Format";
                FormatImage.Source = new BitmapImage(new Uri("..\\..\\RebalancerNew\\Resources\\Images\\CustomGroupsImportFormat.PNG", UriKind.Relative));
            }
            else if (importFormatType.Equals(RebalancerEnums.ImportType.CashFlowImport))
            {
                FormatWindowName.Title = "CashFlow Import Format";
                FormatTextBlock.Text = "CashFlow Import Format";
                FormatImage.Source = new BitmapImage(new Uri("..\\..\\RebalancerNew\\Resources\\Images\\CashFlowImportFormatNew.PNG", UriKind.Relative));
            }
            else if (importFormatType.Equals(RebalancerEnums.ImportType.ModelPortfolioImport))
            {
                FormatWindowName.Title = "Model Portfolio Import Format";
                FormatTextBlock.Text = "Model Portfolio Import Format";
            }
        }

        /// <summary>
        /// This Method sets the Format Image as per Tolerance Functionality
        /// </summary>
        public void SetFormatImage(KeyValueItem selectedUseTolerance, KeyValueItem selectedToleranceFactor, KeyValueItem selectedModelType)
        {
            try
            {
                if((selectedModelType != null && selectedModelType.Key == (int)RebalancerEnums.ModelType.TargetCash) ||
                    (selectedUseTolerance != null && selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.No))
                {
                    FormatImage.Source = new BitmapImage(new Uri("..\\..\\RebalancerNew\\Resources\\Images\\ModelPortfolioExampleNew.PNG", UriKind.Relative));
                }
                else
                {
                    if (selectedToleranceFactor != null && selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InPercentage)
                    {
                        FormatImage.Source = new BitmapImage(new Uri("..\\..\\RebalancerNew\\Resources\\Images\\ToleranceInPercentage.png", UriKind.Relative));
                    }
                    else
                    {
                        FormatImage.Source = new BitmapImage(new Uri("..\\..\\RebalancerNew\\Resources\\Images\\ToleranceInBPS.png", UriKind.Relative));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
