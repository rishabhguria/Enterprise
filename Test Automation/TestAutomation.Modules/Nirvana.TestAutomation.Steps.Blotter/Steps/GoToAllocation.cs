using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public partial class GoToAllocation : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                PranaApplication.BringToFrontOnAttach = false;
                //Wait(6000);
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
        private void InputEnter(DataRow dr)
        {
            try
            {
                var msaaObj = DgBlotter2.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                DgBlotter2.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.GoToAllocation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                    if (GotoAllocation.IsVisible)
                    {
                        GotoAllocation.Click(MouseButtons.Left);
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", GotoAllocation.MsaaName);
                    }
                }
                
                
                //GotoAllocation.Click(MouseButtons.Left);
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
