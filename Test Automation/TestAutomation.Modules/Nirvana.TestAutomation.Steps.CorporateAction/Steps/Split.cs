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
    class Split : CorporateActionUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenCorporateActionsUI();
                InsertSplitData(testData, sheetIndexToName);
                Preview.DoubleClick(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.MinimizeWindow(ref FrmCorporateActionNew_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        public void InsertSplitData(DataSet testData, Dictionary<int, String> sheetIndexToName)
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
                    Wait(2000);
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
