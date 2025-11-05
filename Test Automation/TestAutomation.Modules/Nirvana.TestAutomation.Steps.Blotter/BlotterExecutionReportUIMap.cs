using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    [UITestFixture]
    public partial class BlotterExecutionReportUIMap : UIMap
    {
        public BlotterExecutionReportUIMap()
        {
            InitializeComponent();
        }

        public BlotterExecutionReportUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Open Blotter ExecutionReport
        /// </summary>
        public void OpenBlotterExecutionReport()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open blotter execution report module(CTRL + ALT + B)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_BLOTTER_EXE_REP"]); ;
                Wait(5000);
                //Trade.Click();
                //BlotterDivideExecutionReport.Click();
                //Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
           
        }

        /// <summary>
        /// Minimize Blotter ExecutionReport
        /// </summary>
        public void MinimizeBlotterExecutionReport()
        {
            try
            {
                BlotterReports_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Right);
                Keyboard.SendKeys("[DOWN][DOWN][DOWN][DOWN][ENTER]");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
          
        }

        public void ExecutionReportCommonDateRange(DataRow dr)
        {
            try
            {
                String fromdate = "1/1/1800";
                String todate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Today.ToString("MM/dd/yyyy"));
                if (dr != null)
                {
                    if (!string.IsNullOrWhiteSpace(dr[TestDataConstants.COL_FROM_DATE].ToString()))
                    {
                        string tempDate = DataUtilities.DateHandler(dr[TestDataConstants.COL_FROM_DATE].ToString());
                        fromdate = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                    }
                    if (!string.IsNullOrWhiteSpace(dr[TestDataConstants.COL_TO_DATE].ToString()))
                    {
                        string tempDate = DataUtilities.DateHandler(dr[TestDataConstants.COL_TO_DATE].ToString());
                        todate = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));

                    }

                }
                FromDate.Click(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(fromdate, string.Empty, true);
                ToDate.Click(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(todate, string.Empty, true);
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
