using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class CtrlSwapClosing : UserControl
    {
        public CtrlSwapClosing()
        {
            InitializeComponent();
        }

        /// <summary>
        /// returns Swap string
        /// </summary>
        /// <returns></returns>
        public SwapParameters GetSelectedParams(SwapValidate swapValidate)
        {
            try
            {
                if (ValidateValues())
                {
                    SwapParameters swapParams = new SwapParameters();
                    swapParams.ClosingDate = dtPkrClosingDate.Value.Date;
                    swapParams.ClosingPrice = spnrClosingCostBasis.Value;
                    return swapParams = ctrlSwapParameters1.GetSelectedParams(swapParams, swapValidate);

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

        #region Set Overloads
        public void Set(SwapParameters swapParams, SwapValidate swapValidate)
        {
            try
            {
                spnrClosingCostBasis.Value = swapParams.ClosingPrice;
                dtPkrClosingDate.Value = swapParams.FirstResetDate;
                ctrlSwapParameters1.Set(swapParams, swapValidate);
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

        /// <summary>
        /// returns Swap string
        /// </summary>
        /// <returns></returns>
        public void ResetControl()
        {
            try
            {
                dtPkrClosingDate.Value = DateTime.Now.Date;
                spnrClosingCostBasis.Value = 0;
                ctrlSwapParameters1.ResetControl();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private bool ValidateValues()
        {
            try
            {
                if (spnrClosingCostBasis.Value < 0)
                {
                    MessageBox.Show("Enter valid Closing Cost Basis", "Information");
                    return false;
                }
                //if (dtPkrClosingDate.Value == null )
                //{
                //    MessageBox.Show("Enter valid Swap Closing Date","Information");
                //    return false;
                //}               

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

