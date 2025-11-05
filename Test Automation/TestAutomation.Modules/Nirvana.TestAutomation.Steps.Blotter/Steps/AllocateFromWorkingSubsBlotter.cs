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
    class AllocateFromWorkingSubsBlotter : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                bool isClicked = false;
                try
                {
                    DgBlotter.Click(MouseButtons.Right);
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Allocate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (testData.Tables[0].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString() != String.Empty)
                {
                    DataRow dr = testData.Tables[0].Rows[0];
                    UltraComboEditorSymbology.Click(MouseButtons.Left);
                    UltraComboEditorSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_PREFERENCE_NAME].ToString();
                    Button1.Click(MouseButtons.Left);
                    Wait(2000);
                }
                else
                {
                    InputEnter(testData, sheetIndexToName);
                    Button1.Click(MouseButtons.Left);
                    Wait(2000);
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
        private void InputEnter(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                int index = 0;
                var gridRow = GridViewAllocation1.MsaaObject.CachedChildren[2];
                Dictionary<string, int> indexToColumnMapDictionary = new Dictionary<string, int>();
                for (int colIndex = 0; colIndex < gridRow.CachedChildren[1].ChildCount; colIndex++)
                {
                    if (indexToColumnMapDictionary.ContainsKey(gridRow.CachedChildren[1].CachedChildren[colIndex].Name))
                    {
                        indexToColumnMapDictionary.Add(gridRow.CachedChildren[1].CachedChildren[colIndex].Name + '2', colIndex);
                    }
                    else
                    {
                        indexToColumnMapDictionary.Add(gridRow.CachedChildren[1].CachedChildren[colIndex].Name, colIndex);
                    }
                }
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    var row = GridViewAllocation1.MsaaObject.CachedChildren[2].CachedChildren[index + 1];
                    string value = row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_Account]].Value;
                    if (!string.IsNullOrWhiteSpace(value) && (index.Equals(0)))
                    {
                        row = GridViewAllocation1.MsaaObject.CachedChildren[2].CachedChildren[index + 2];
                        index++;
                    }
                    if (string.IsNullOrWhiteSpace(value) && (index.Equals(0)))
                    {
                        row = GridViewAllocation1.MsaaObject.CachedChildren[2].CachedChildren[index + 1];

                    }
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_Account]].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Account].ToString());
                    KeyboardUtilities.PressKey(4, KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString());
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
