using Infragistics.Windows.Editors;
using Prana.LogManager;
using System;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for RASImportView.xaml
    /// </summary>
    public partial class RASImportView : Window
    {
        public RASImportView()
        {
            InitializeComponent();
        }

        private void XamNumericEditor_EditModeStarted(object sender, Infragistics.Windows.Editors.Events.EditModeStartedEventArgs e)
        {
            ((XamNumericEditor)sender).SelectAll();
        }

        /// <summary>
        /// This method is to set the visibility of Sedol Symbol Column's visibility
        /// </summary>
        /// <param name="show"></param>
        public void SetSedolSymbolColumnVisibility(bool show)
        {
            try
            {
                ValidSecurities.FieldLayouts[0].Fields["SEDOLSymbol"].Visibility = show ? Visibility.Visible : Visibility.Collapsed;
                InvalidSecurities.FieldLayouts[0].Fields["Symbol"].Label = show ? "SEDOL" : "Symbol";
            }
            catch(Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
