using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;


namespace Prana.ClientCommon
{
    public partial class CtrlSwapParameters : UserControl
    {
        private bool _isPreTradeSwap = false;

        public bool IsPreTradeSwap
        {
            get { return _isPreTradeSwap; }
            set { _isPreTradeSwap = value; }
        }


        public CtrlSwapParameters()
        {

            InitializeComponent();
            spnrNotional.DecimalPoints = 2;
            spnrNotional.ValueChanged += new EventHandler(spnrNotional_ValueChanged);
            if (IsPreTradeSwap)
            {
                ShowPriceUpdateCaption();
            }
        }

        public void UnWireEvents()
        {
            if (spnrNotional != null)
            {
                spnrNotional.ValueChanged -= new EventHandler(spnrNotional_ValueChanged);
                spnrNotional.ValueChanged -= new EventHandler(spnrNotional_ValueChanged);
                spnrNotional.Dispose();
                spnrNotional = null;
            }
        }
        public CtrlSwapParameters(bool isPreTradeSwap)
        {

            InitializeComponent();
            spnrNotional.DecimalPoints = 2;
            spnrNotional.ValueChanged += new EventHandler(spnrNotional_ValueChanged);
            if (isPreTradeSwap)
            {
                ShowPriceUpdateCaption();
            }
        }


        private void ShowPriceUpdateCaption()
        {
            lblNotionalUpdate.Visible = true;
            lblDisclaimer.Visible = true;
        }




        void spnrNotional_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// returns Swap string
        /// </summary>
        /// <returns></returns>
        public SwapParameters GetSelectedParams(SwapValidate swapValidate)
        {
            try
            {
                if (ValidateValues(swapValidate))
                {
                    SwapParameters selectedParams = new SwapParameters();
                    selectedParams.SwapDescription = txtDescription.Text;
                    selectedParams.OrigCostBasis = spnrCostBasis.Value;
                    selectedParams.DayCount = int.Parse(spnrDayCount.Value.ToString());
                    selectedParams.Differential = spnrDifferential.Value / ApplicationConstants.BASISPOINTTOPERCENTAGE;
                    selectedParams.NotionalValue = spnrNotional.Value;
                    selectedParams.FirstResetDate = DateTime.Parse(dtPkrResetDate.Value.ToString());
                    selectedParams.OrigTransDate = DateTime.Parse(dtPkrTransDate.Value.ToString());
                    selectedParams.BenchMarkRate = spnrBenchMarkRate.Value;
                    selectedParams.ResetFrequency = "Quarterly";// cmbResetFrequency.Value.ToString();
                    return selectedParams;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        public SwapParameters GetSelectedParams(SwapParameters targetSwapParams, SwapValidate swapValidate)
        {
            try
            {
                if (ValidateValues(swapValidate))
                {
                    targetSwapParams.SwapDescription = txtDescription.Text;
                    targetSwapParams.OrigCostBasis = spnrCostBasis.Value;
                    targetSwapParams.DayCount = int.Parse(spnrDayCount.Value.ToString());
                    targetSwapParams.Differential = spnrDifferential.Value / ApplicationConstants.BASISPOINTTOPERCENTAGE;
                    targetSwapParams.NotionalValue = spnrNotional.Value;
                    targetSwapParams.FirstResetDate = DateTime.Parse(dtPkrResetDate.Value.ToString());
                    targetSwapParams.OrigTransDate = DateTime.Parse(dtPkrTransDate.Value.ToString());
                    targetSwapParams.BenchMarkRate = spnrBenchMarkRate.Value;
                    targetSwapParams.ResetFrequency = "Quarterly";// cmbResetFrequency.Value.ToString();
                    return targetSwapParams;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }
        /// <summary>
        /// Added to set swap parameter fields in TT, PRANA-10823
        /// </summary>
        /// <param name="os"></param>
        public void SetSwapParams(OrderSingle os)
        {
            try
            {
                txtDescription.Text = os.SwapParameters.SwapDescription;
                spnrCostBasis.Value = os.SwapParameters.OrigCostBasis;
                spnrDayCount.Value = os.SwapParameters.DayCount;
                spnrNotional.Value = os.SwapParameters.NotionalValue;
                dtPkrResetDate.Value = os.SwapParameters.FirstResetDate;
                dtPkrTransDate.Value = os.SwapParameters.OrigTransDate;
                spnrBenchMarkRate.Value = os.SwapParameters.BenchMarkRate;
                spnrDifferential.Value = os.SwapParameters.Differential * ApplicationConstants.BASISPOINTTOPERCENTAGE;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region Set Overloads
        public void Set(SwapParameters swapParams, SwapValidate swapValidate)
        {
            try
            {
                txtDescription.Text = swapParams.SwapDescription;
                spnrCostBasis.Value = swapParams.OrigCostBasis;
                spnrDayCount.Value = swapParams.DayCount;
                spnrDifferential.Value = swapParams.Differential * ApplicationConstants.BASISPOINTTOPERCENTAGE;
                spnrNotional.Value = swapParams.NotionalValue;
                if (swapParams.FirstResetDate == DateTimeConstants.MinValue)
                {
                    dtPkrResetDate.Value = DateTime.Now.Date;
                }
                else
                {
                    dtPkrResetDate.Value = swapParams.FirstResetDate;
                }
                if (swapParams.OrigTransDate == DateTimeConstants.MinValue)
                {
                    dtPkrTransDate.Value = DateTime.Now.Date;
                }
                else
                {
                    dtPkrTransDate.Value = swapParams.OrigTransDate;
                }
                spnrBenchMarkRate.Value = swapParams.BenchMarkRate;
                cmbResetFrequency.Value = swapParams.ResetFrequency;
                if (swapValidate == SwapValidate.Expire)
                {
                    spnrNotional.Enabled = false;
                    spnrBenchMarkRate.Enabled = false;
                    spnrDifferential.Enabled = false;
                    dtPkrResetDate.Enabled = false;
                    spnrDayCount.Enabled = false;
                    dtPkrTransDate.Enabled = false;
                    spnrCostBasis.Enabled = false;
                    cmbResetFrequency.Enabled = false;
                }
                else if (swapValidate == SwapValidate.ExpireAndRollover)
                {
                    spnrNotional.Enabled = false;
                    spnrBenchMarkRate.Enabled = true;
                    spnrDifferential.Enabled = true;
                    dtPkrResetDate.Enabled = true;
                    spnrDayCount.Enabled = true;
                    dtPkrTransDate.Enabled = true;
                    spnrCostBasis.Enabled = false;
                    cmbResetFrequency.Enabled = true;
                }
                else
                {
                    spnrNotional.Enabled = true;
                    spnrBenchMarkRate.Enabled = true;
                    spnrDifferential.Enabled = true;
                    dtPkrResetDate.Enabled = true;
                    spnrDayCount.Enabled = true;
                    dtPkrTransDate.Enabled = true;
                    spnrCostBasis.Enabled = true;
                    cmbResetFrequency.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void Set(string swapStr, SwapValidate swapValidate)
        {
            SwapParameters swapParams = new SwapParameters(swapStr);
            Set(swapParams, swapValidate);
        }
        #endregion

        public void UpdateNotionalValue(double notionalValue)
        {
            this.spnrNotional.Value = Math.Round(notionalValue, 2);
        }
        public void UpdateCostBasis(double costBasis)
        {
            this.spnrCostBasis.Value = costBasis;
        }
        public void UpdateOrigTransDate(DateTime startDate)
        {
            this.dtPkrTransDate.Value = startDate;
            //Kashish Goyal, 14/07/2015
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-9523
            this.dtPkrResetDate.Value = startDate.AddDays(1);
        }
        public void ResetControl()
        {
            try
            {
                txtDescription.Text = string.Empty;
                spnrCostBasis.Value = 0;
                spnrDayCount.Value = 0;
                spnrDifferential.Value = 0;
                spnrNotional.Value = 0;
                dtPkrResetDate.Value = DateTime.Now.Date;
                dtPkrTransDate.Value = DateTime.Now.Date;
                spnrBenchMarkRate.Value = 0;
                cmbResetFrequency.Visible = false;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private bool ValidateValues(SwapValidate swapValidate)
        {
            bool isValidated = false;
            try
            {

                switch (swapValidate)
                {
                    case SwapValidate.Trade:
                        isValidated = ValidateValuesForTrade();
                        break;
                    case SwapValidate.Allocate:
                        isValidated = ValidateValuesForTrade();
                        break;
                    case SwapValidate.Expire:
                        isValidated = ValidateValuesForExpire();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                isValidated = false;
            }

            return isValidated;
        }
        private bool ValidateValuesForTrade()
        {
            try
            {
                spnrNotional.Enabled = true;
                spnrCostBasis.Enabled = true;
                dtPkrTransDate.Enabled = true;
                if (spnrNotional.Value < 0)
                {
                    MessageBox.Show("Enter valid Swap Notional", "Information");
                    return false;
                }
                if (spnrBenchMarkRate.Value < 0)
                {
                    MessageBox.Show("Enter valid Swap Interest BemchMark Rate", "Information");
                    return false;
                }
                if (spnrCostBasis.Value < 0)
                {
                    MessageBox.Show("Enter valid Swap CostBasis", "Information");
                    return false;
                }
                if (spnrDayCount.Value < 1)
                {
                    MessageBox.Show("Enter valid Swap DayCount Convention", "Information");
                    return false;
                }
                if (dtPkrTransDate.Value == null)
                {
                    MessageBox.Show("Enter valid Swap Transaction Date", "Information");
                    return false;
                }
                if (dtPkrResetDate.Value == null || DateTime.Parse(dtPkrResetDate.Value.ToString()) <= DateTime.Parse(dtPkrTransDate.Value.ToString()))
                {
                    MessageBox.Show("Enter valid Swap Reset Date", "Information");
                    return false;
                }
                if (spnrBenchMarkRate.Value + spnrDifferential.Value / ApplicationConstants.BASISPOINTTOPERCENTAGE < 0)
                {
                    MessageBox.Show("Sum of Benchmark Rate and differential not positive !", "Information");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        //private bool ValidateValuesForAllocation()
        //{
        //    try
        //    {
        //        spnrNotional.Enabled = true ;
        //        spnrCostBasis.Enabled = true ;
        //        dtPkrTransDate.Enabled = true ;
        //        if (spnrNotional.Value <= 0)
        //        {
        //            MessageBox.Show("Enter valid Swap Notional", "Information");
        //            return false;
        //        }                
        //        if (spnrCostBasis.Value <= 0)
        //        {
        //            MessageBox.Show("Enter valid Swap CostBasis", "Information");
        //            return false;
        //        }
        //        if (spnrBenchMarkRate.Value < 0)
        //        {
        //            MessageBox.Show("Enter valid Swap Interest BemchMark Rate", "Information");
        //            return false;
        //        }  
        //        if (spnrDayCount.Value < 1)
        //        {
        //            MessageBox.Show("Enter valid Swap DayCount Convention", "Information");
        //            return false;
        //        }
        //        if (dtPkrTransDate.Value == null )
        //        {
        //            MessageBox.Show("Enter valid Swap Transaction Date", "Information");
        //            return false;
        //        }
        //        if (dtPkrResetDate.Value == null || dtPkrResetDate.Value <= dtPkrTransDate.Value )
        //        {
        //            MessageBox.Show("Enter valid Swap Reset Date", "Information");
        //            return false;
        //        }
        //        if (spnrBenchMarkRate.Value + spnrDifferential.Value / ApplicationConstants.BASISPOINTTOPERCENTAGE < 0)
        //        {
        //            MessageBox.Show("Sum of Benchmark Rate and differential not positive !", "Information");
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        throw;
        //    }
        //}

        private bool ValidateValuesForExpire()
        {
            try
            {
                if (spnrDayCount.Value < 1)
                {
                    MessageBox.Show("Enter valid Swap DayCount Convention", "Information");
                    return false;
                }
                if (dtPkrTransDate.Value == null)
                {
                    MessageBox.Show("Enter valid Swap Transaction Date", "Information");
                    return false;
                }
                if (dtPkrResetDate.Value == null || DateTime.Parse(dtPkrResetDate.Value.ToString()) <= DateTime.Parse(dtPkrTransDate.Value.ToString()))
                {
                    MessageBox.Show("Enter valid Swap Reset Date", "Information");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }



    }
}
