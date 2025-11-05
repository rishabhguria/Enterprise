//using Prana.InstanceCreator;
//using Prana.ServerClientCommon;
using Infragistics.Win.Misc;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlMainConsolidationViewSummary : UserControl
    {


        ExposureAndPnlOrderSummary _consolidationSummary = null;
        private const string FORMAT_PNL_EXPOSURE_CASH = "{0:#,##,##0}";
        private const string FORMAT_PNL_CONTRIBUTION = "{0:#,0.00}";
        private const string FORMAT_NETASSETVALUE = "{0:#,#}";
        private const string FORMAT_PERCENTAGE_1 = "{0:#,0.00%}";

        public CtrlMainConsolidationViewSummary()
        {
            InitializeComponent();
        }


        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        /// <summary>
        /// Initialize the control.
        /// This function won't be called, as it's not binding the object but just updating the values.
        /// </summary>
        public void InitControl(ExposureAndPnlOrderSummary consolidationSummary)
        {
            try
            {
                _consolidationSummary = consolidationSummary;

                if (!_isInitialized)
                {

                    try
                    {
                        SetupBinding();

                    }
                    catch (Exception ex)
                    {

                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }


        #endregion

        //This function won't be called, as it's not binding the object but just updating the values
        private void SetupBinding()
        {
            UpdateSummaryValues(_consolidationSummary);

        }

        delegate void SummaryUpdater(ExposureAndPnlOrderSummary textValuetoUpdate);

        private void UpdateSummary(ExposureAndPnlOrderSummary summary)
        {
            try
            {
                this.tlpConsolidationDashboard.SuspendLayout();
                this.lblLongExposureTotal.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.LongExposure);
                ColorLabel(summary.LongExposure, this.lblLongExposureTotal);


                this.lblShortExposureTotal.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.ShortExposure);

                ColorLabel(summary.ShortExposure, this.lblShortExposureTotal);

                this.lblNetExposureTotal.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.NetExposure);
                ColorLabel(summary.NetExposure, this.lblNetExposureTotal);

                this.lblDayPNLTotal.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.DayPnL);
                ColorLabel(summary.DayPnL, this.lblDayPNLTotal);


                this.lblNetAssetValue.Text = string.Format(FORMAT_NETASSETVALUE, summary.NetAssetValue);
                ColorLabel(summary.NetAssetValue, this.lblNetAssetValue);

                string lblPNLContributionText = string.Format(FORMAT_PNL_CONTRIBUTION, summary.PNLContributionPercentageSummary);
                this.lblPNLContribution.Text = lblPNLContributionText + "%";

                ColorLabel(summary.PNLContributionPercentageSummary, this.lblPNLContribution);


                this.lblLongMktValue.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.LongMarketValue);
                ColorLabel(summary.LongMarketValue, this.lblLongMktValue);

                this.lblShortMktValue.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.ShortMarketValue);
                ColorLabel(summary.ShortMarketValue, this.lblShortMktValue);

                this.lblNetMktValue.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.NetMarketValue);
                ColorLabel(summary.NetMarketValue, this.lblNetMktValue);

                this.lblCashProjected.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.CashProjected);
                ColorLabel(summary.CashProjected, this.lblCashProjected);

                this.lblCostBasisPNLValue.Text = string.Format(FORMAT_PNL_EXPOSURE_CASH, summary.CostBasisPNL);
                ColorLabel(summary.CostBasisPNL, this.lblCostBasisPNLValue);

                this.lblPercentNetExposure.Text = string.Format(FORMAT_PERCENTAGE_1, summary.NetPercentExposure / 100);
                ColorLabel(summary.NetPercentExposure, this.lblPercentNetExposure);

                this.lblPercentNetMktValue.Text = string.Format(FORMAT_PERCENTAGE_1, summary.PercentNetMarketValue);
                ColorLabel(summary.PercentNetMarketValue, this.lblPercentNetMktValue);

                this.lblPercentNetExposureGross.Text = string.Format(FORMAT_PERCENTAGE_1, summary.NetPercentExposureGross);
                ColorLabel(summary.NetPercentExposureGross, this.lblPercentNetExposureGross);

                this.tlpConsolidationDashboard.ResumeLayout();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public void UpdateSummaryValues(ExposureAndPnlOrderSummary consolidationSummary)
        {
            SummaryUpdater stringHandler = new SummaryUpdater(UpdateSummary);
            this.BeginInvoke(stringHandler, new object[1] { consolidationSummary });
        }

        private void ColorLabel(double summaryValue, UltraLabel label)
        {

            if (summaryValue > 0)
            {
                label.Appearance.ForeColor = Color.FromArgb(177, 216, 64);
            }
            else if (summaryValue < 0)
            {
                label.Appearance.ForeColor = Color.FromArgb(255, 91, 71);
            }
            else
            {
                //in case of zero
                label.Appearance.ForeColor = Color.White;
            }
        }

    }
}
