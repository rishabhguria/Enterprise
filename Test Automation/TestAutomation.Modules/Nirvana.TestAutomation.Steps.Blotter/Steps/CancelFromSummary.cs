using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Steps.Blotter;
using System.Linq;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class CancelFromSummary : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                MaximizeBlotter();
                Symbol.Click(MouseButtons.Left);
                Summary.DoubleClick(MouseButtons.Left);
                Wait(2000);
                if (testData != null)
                {
                    _res.ErrorMessage = InputEnter(testData.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CancelFromSummary");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseBlotter();
            }
            return _res;
        }

        private string InputEnter(DataTable dTable)
        {
            string errorMessage = string.Empty;
            try
            {

                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dTable.Rows[0]);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dTable, new List<string>(), 0.01);
                        throw new Exception("Trade not found during CancelFromSummary step. [Symbol= " + dTable.Rows[0]["Symbol"] + "], Quantity = [" + dTable.Rows[0]["Target Qty"] + "] Side = [" + dTable.Rows[0]["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                for (int i = 0; i < index + 1; i++)
                {
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                BtnExpansion.WaitForVisible();
                BtnExpansion.Click(MouseButtons.Left);
                for (int i = 0; i < 2; i++)
                {
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                Wait(1000);
                Pageright.ClickRightBound(MouseButtons.Right);
                Cancel.Click(MouseButtons.Left);
                if (Confirmation.IsVisible)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }
                else if (Warning1.IsVisible)
                {
                    ButtonOK1.Click(MouseButtons.Left);
                }
                return errorMessage;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
