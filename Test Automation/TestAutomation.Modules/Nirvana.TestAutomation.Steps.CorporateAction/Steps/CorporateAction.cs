using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    public class CorporateAction : CorporateActionUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// 
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                OpenCorporateActions();
                InsertCorporateAction(testData, sheetIndexToName);
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
                KeyboardUtilities.CloseWindow(ref FrmCorporateActionNew_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        /// <summary>
        /// Inserts the corporate action.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void InsertCorporateAction(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    CmbCATypeApply1.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dtRow.Table.ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Dictionary<string, int> dictColumnToIndexMap = new Dictionary<string, int>();
                    var gridMssa = GrdCorporateActionEntry.MsaaObject;
                    for (int i = 0; i < gridMssa.CachedChildren[0].CachedChildren[0].ChildCount; i++)
                    {
                        dictColumnToIndexMap.Add(gridMssa.CachedChildren[0].CachedChildren[0].CachedChildren[i].Name, i);
                    }
                    CorporateActionStep(dictColumnToIndexMap, gridMssa, dtRow);
                    Preview.DoubleClick(MouseButtons.Left);
                    Apply.DoubleClick(MouseButtons.Left);
                    ButtonYes.Click(MouseButtons.Left);
                    SaveCorporateAction.DoubleClick(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the corporate actions.
        /// </summary>
        private void OpenCorporateActions()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open closing module(CTRL + ALT + I)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_CA"]);
                //Wait(7000);
                ExtentionMethods.WaitForVisible(ref FrmCorporateActionNew,15);
 
                //PortfolioManagement.Click(MouseButtons.Left);
                //CorporateActions.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Corporates the action step.
        /// </summary>
        /// <param name="dictColumnToIndexMap">The dictionary column to index map.</param>
        /// <param name="gridMssa">The grid mssa.</param>
        /// <param name="dtRow">The dt row.</param>
        private static void CorporateActionStep(Dictionary<string, int> dictColumnToIndexMap, MsaaObject gridMssa, DataRow dtRow)
        {
            try
            {
                foreach (string columnName in dictColumnToIndexMap.Keys)
                {
                    gridMssa.CachedChildren[0].CachedChildren[0].CachedChildren[dictColumnToIndexMap[columnName]].Click(MouseButtons.Left);
                    Wait(2000);
                    if (columnName.Contains(TestDataConstants.COL_DATE))
                    {
                        gridMssa.CachedChildren[0].CachedChildren[0].CachedChildren[dictColumnToIndexMap[columnName]].Click(MouseButtons.Left);
                        gridMssa.CachedChildren[0].CachedChildren[0].CachedChildren[dictColumnToIndexMap[columnName]].Click(MouseButtons.Left);
                        Keyboard.SendKeys(String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dtRow[columnName].ToString())));
                    }
                    else
                    {
                        gridMssa.CachedChildren[0].CachedChildren[0].CachedChildren[dictColumnToIndexMap[columnName]].Click(MouseButtons.Left);
                        gridMssa.CachedChildren[0].CachedChildren[0].CachedChildren[dictColumnToIndexMap[columnName]].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dtRow[columnName].ToString());
                    }
                }
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