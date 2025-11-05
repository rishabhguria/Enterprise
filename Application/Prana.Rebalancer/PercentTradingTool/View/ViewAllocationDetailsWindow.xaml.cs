using Prana.LogManager;
using Prana.Rebalancer.PercentTradingTool.ViewModel;
using System;
using System.Windows;

namespace Prana.Rebalancer
{
    /// <summary>
    /// Interaction logic for ViewAllocationDetailsWindow.xaml
    /// </summary>
	public partial class ViewAllocationDetailsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAllocationDetailsWindow"/> class.
        /// </summary>
        public ViewAllocationDetailsWindow()
        {
            this.InitializeComponent();
            this.Closed += ViewAllocationDetailsWindow_Closed;
            // Insert code required on object creation below this point.
        }

        private void ViewAllocationDetailsWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (ViewAllocationDetailsViewModel != null)
                {
                    ViewAllocationDetailsViewModel.Dispose();
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
        /// Gets or sets the view allocation details view model.
        /// </summary>
        /// <value>
        /// The view allocation details view model.
        /// </value>
        public ViewAllocationDetailsViewModel ViewAllocationDetailsViewModel
        {
            get { return DataContext as ViewAllocationDetailsViewModel; }
            set { DataContext = value; }
        }

        /// <summary>
        /// Handles the OnStateChanged event of the ViewAllocationDetailsWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ViewAllocationDetailsWindow_OnStateChanged(object sender, EventArgs e)
        {
            try
            {

                switch (this.WindowState)
                {
                    case WindowState.Maximized:
                        tbNirvana.Text = RebalancerConstants.CAP_NIRVANACAPTION;
                        break;
                    case WindowState.Minimized:
                        tbNirvana.Text = RebalancerConstants.CAP_VIEWALLOCATIONCAPTION;
                        break;
                    case WindowState.Normal:
                        tbNirvana.Text = RebalancerConstants.CAP_NIRVANACAPTION;
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
    }
}