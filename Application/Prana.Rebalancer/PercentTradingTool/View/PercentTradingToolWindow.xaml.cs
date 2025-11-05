using Prana.LogManager;
using Prana.Rebalancer.PercentTradingTool.CustomControls;
using Prana.Rebalancer.PercentTradingTool.ViewModel;
using Prana.Utilities.UI.WpfUIUtilities;
using System;
using System.Windows;

namespace Prana.Rebalancer
{
    /// <summary>
    /// Interaction logic for PercentTradingToolWindow.xaml
    /// </summary>
    public partial class PercentTradingToolWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PercentTradingToolWindow"/> class.
        /// </summary>
        public PercentTradingToolWindow()
        {
            this.InitializeComponent();
            this.Closed += PercentTradingToolWindow_Closed;
            // Insert code required on object creation below this point.
        }

        private void PercentTradingToolWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (PercentTradingToolViewModel != null)
                {
                    PercentTradingToolViewModel.Dispose();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets or sets the percent trading tool view model.
        /// </summary>
        /// <value>
        /// The percent trading tool view model.
        /// </value>
        public PercentTradingToolViewModel PercentTradingToolViewModel
        {
            get { return DataContext as PercentTradingToolViewModel; }
            set { DataContext = value; }
        }

        /// <summary>
        /// Handles the OnStateChanged event of the PercentTradingToolWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PercentTradingToolWindow_OnStateChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.WindowState)
                {
                    case WindowState.Maximized:
                    case WindowState.Normal:
                        tbNirvana.Text = RebalancerConstants.CAP_NIRVANACAPTION;
                        break;
                    case WindowState.Minimized:
                        tbNirvana.Text = RebalancerConstants.CAP_PERCENTTRADINGTOOL;
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void PercentTradingToolWindow_OnClosed(object sender, EventArgs e)
        {
            try
            {
                PranaSymbolWPFControl foundTextBox = GetChildControl.FindChild<PranaSymbolWPFControl>(this, "pranaSymbolControl");
                if (foundTextBox != null)
                {
                    foundTextBox.Dispose();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}