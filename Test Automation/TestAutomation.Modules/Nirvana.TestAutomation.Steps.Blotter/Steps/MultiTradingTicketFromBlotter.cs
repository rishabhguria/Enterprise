using System.Linq;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class MultiTradingTicketFromBlotter : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                OrdersTab.Click(MouseButtons.Left);
                if (testData != null)
                {
                    _res.ErrorMessage = InputEnter(testData.Tables[0]);
                }
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
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
               // Wait(5000);
                KeyboardUtilities.CloseWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
            }
            return _res;
        }
        private string InputEnter(DataTable dTable)
        {
            string errorMessage = string.Empty;
            try
            {
                var msaaObj = DgBlotter1.MsaaObject;
                List<String> columns = new List<String>();
                try
                {
                    string StepName = "MultiTradingTicketFromBlotter";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref dTable);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref dTable);
                }
                catch (Exception)
                { }
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] dRows = DataUtilities.GetMatchingMultipleDataRows(DataUtilities.RemoveTrailingZeroes(dtBlotter),DataUtilities.RemoveTrailingZeroes(dTable), errorMessage);
                
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    return errorMessage;

                int index = 0;
                foreach(var row in dRows)
                {
                    index = dtBlotter.Rows.IndexOf(row);
                    DgBlotter1.InvokeMethod("ScrollToRow", index);
					// For checking the check box  used FindDescendantByName()
                    msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                }
             
                msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Right);
                  bool isClicked = false;
               // Wait(3000);

                try
                 {
                        isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Trade_New_Sub);
                 }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                if (isClicked == false)
                {
                    msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Right);
                    TradeNewSub.Click(MouseButtons.Left);
                }
                //ButtonOK.Click(MouseButtons.Left);
                if (Warning2.IsVisible)
                {
                    ButtonOK2.Click(MouseButtons.Left);
                }
                
            }
            catch (Exception ex)
            {
				bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;               
            }
            return errorMessage;
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
