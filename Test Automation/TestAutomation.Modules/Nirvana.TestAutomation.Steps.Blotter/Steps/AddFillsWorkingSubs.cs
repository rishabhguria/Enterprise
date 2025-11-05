using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class AddFillsWorkingSubs : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                WorkingSubsTab.Click(MouseButtons.Left);
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddFillsWorkingSubs");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return _res;
        }

        private void InputEnter(DataRow dr)
        {
            try
            {
                var msaaObj = DgBlotter.MsaaObject;
                DataTable dtBlotter = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.DgBlotter.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                DataRow dtRow = DataUtilities.GetMatchingDataRow(dtBlotter, dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                DgBlotter.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                //if (AddFills2.IsVisible) AddFills2.Click(MouseButtons.Left);
                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Add_Modify_Fills);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                    if (AddDivideModifyFills.IsVisible)
                    {
                        ClickAddDivideModifyFills();
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", AddDivideModifyFills.MsaaName);
                    }
                }
                
                
               


            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
