using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.CreateTransaction
{
    class AddNewTransaction : CreateTransactionUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenCreateTransaction();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        BtnAddToCloseTrade.Click(MouseButtons.Left);
                        InputEnter(dr);
                        BtnSave.Click(MouseButtons.Left);
                        Wait(1000);
                        ButtonSaveYes.Click(MouseButtons.Left);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.MinimizeWindow(ref CreatePosition_UltraFormManager_Dock_Area_Top);
            } 
            return _result;
        }
        private void InputEnter(DataRow dr)
        {
            try
            {
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.GrdCreatePosition.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                var gridRow = GrdCreatePosition.MsaaObject.CachedChildren[0];
                Dictionary<string, int> indexToColumnMapDictionary = new Dictionary<string, int>();
                for (int colIndex = 0; colIndex < gridRow.CachedChildren[1].ChildCount; colIndex++)
                {
                    if (colIndex == 66 || colIndex == 73)
                    {
                        continue;
                    }
                    indexToColumnMapDictionary.Add(gridRow.CachedChildren[1].CachedChildren[colIndex].Name, colIndex);
                }
                int index = 0;
                var row = GrdCreatePosition.MsaaObject.CachedChildren[0].CachedChildren[1];
                foreach (string item in dr.ItemArray)
                {

                    if (item != "")
                    {
                        this.GrdCreatePosition.InvokeMethod("ScrollToColumnName", dr.Table.Columns[index].ToString());
                        row.CachedChildren[indexToColumnMapDictionary[dr.Table.Columns[index].ToString()]].Click(MouseButtons.Left);
                        Wait(1000);
                        Keyboard.SendKeys(item);
                        Keyboard.SendKeys("[TAB]");
                    }
                    index++;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
