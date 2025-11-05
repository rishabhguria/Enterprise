using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.UI;
using TestAutomationFX.Core;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    public class SpinOff : CorporateActionUIMap,ITestStep
    {

        /// <summary>
        /// Run the Test
        /// </summary>
        /// <param name="testdata"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testdata, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenCorporateActionsUI();
               // Wait(2000);
                InsertSpinOffData(testdata, sheetIndexToName);
                Preview.DoubleClick(MouseButtons.Left);
                //Wait(2000);
                //Apply.DoubleClick(MouseButtons.Left);
                //if (CorporateAction1.IsVisible)
                //{
                //    ButtonYes.Click(MouseButtons.Left);
                //}
                //SaveCorporateAction.DoubleClick(MouseButtons.Left);
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


        /// <summary>
        /// Insert Spin off Data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        public void InsertSpinOffData(DataSet testData, Dictionary<int, String> sheetIndexToName)
        {
            try
            {
                foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    Dictionary<string, int> dictColumnToIndexMap = new Dictionary<string, int>();
                    var gridMssa = GrdCorporateActionEntry.MsaaObject;
                    for (int i = 0; i < gridMssa.CachedChildren[0].CachedChildren[0].ChildCount; i++)
                    {
                        dictColumnToIndexMap.Add(gridMssa.CachedChildren[0].CachedChildren[0].CachedChildren[i].Name, i);
                    }
                    CorporateActionStep(dictColumnToIndexMap, gridMssa, dtRow);
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
        /// Step used for handling the date format
        /// </summary>
        /// <param name="dictColumnToIndexMap"></param>
        /// <param name="gridMssa"></param>
        /// <param name="dtRow"></param>
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
