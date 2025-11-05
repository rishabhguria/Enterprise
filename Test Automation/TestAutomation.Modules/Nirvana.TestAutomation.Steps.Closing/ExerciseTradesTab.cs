using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Closing
{
    [UITestFixture]
    public partial class ExerciseTradesTab : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseTradesTab"/> class.
        /// </summary>
        public ExerciseTradesTab()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseTradesTab"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ExerciseTradesTab(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Swap Expire Closing
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        protected bool SwapExpireClosing(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];
                foreach (DataRow dt in excelFileData.Rows)
                {
                    if (DtPkrClosingDate.IsEnabled)
                        DtPkrClosingDate.Properties[TestDataConstants.CONST_VALUE] = Convert.ToDateTime(dt[TestDataConstants.COL_CLOSING_DATE].ToString());

                    if (ClosingCostBasis.IsEnabled)
                        ClosingCostBasis.Properties[TestDataConstants.CONST_VALUE] = dt[TestDataConstants.COL_CLOSING_COST_BASIS].ToString();

                    if (InterestRate.IsEnabled)
                        InterestRate.Properties[TestDataConstants.CONST_VALUE] = dt[TestDataConstants.COL_INTEREST_RATE].ToString();

                    if (Spread.IsEnabled)
                        Spread.Properties[TestDataConstants.CONST_VALUE] = dt[TestDataConstants.COL_SPREAD].ToString();

                    if (FirstResetDate.IsEnabled)
                        FirstResetDate.Properties[TestDataConstants.CONST_VALUE] = dt[TestDataConstants.COL_FIRST_RESET_DATE].ToString();

                    if (ShowDescription.IsEnabled)
                        ShowDescription.Properties[TestDataConstants.CONST_VALUE] = dt[TestDataConstants.COL_SHOW_DESCRIPTION].ToString();

                    if (DayCount.IsEnabled)
                        DayCount.Properties[TestDataConstants.CONST_VALUE] = dt[TestDataConstants.COL_DAY_COUNT].ToString();

                    if (OriginalTradeDate.IsEnabled)
                        OriginalTradeDate.Properties[TestDataConstants.CONST_VALUE] = dt[TestDataConstants.COL_ORIGINAL_TRADE_DATE].ToString();
                }
                BtnSave.Click(MouseButtons.Left);
                if (ButtonOK.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return true;
        }

        /// <summary>
        /// Open Closing UI
        /// </summary>
        internal void OpenClosingUI()
        {
            try
            {
                //Shortcut to open closing module(CTRL + ALT + C )
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_CLOSING"]);
                ExtentionMethods.WaitForVisible(ref CloseTrade, 15);
                //Wait(5000);
                //PortfolioManagement.Click(MouseButtons.Left);
                //ClosePositions.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Minimize Closing
        /// </summary>
        internal void MinimizeClosing()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref CloseTrade_UltraFormManager_Dock_Area_Top);
                TitleBar.WaitForVisible();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
