using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.Text;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class CheckPreferences : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                AllocateUnallocatePinTab.Click(MouseButtons.Left);
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    Dictionary<string, int> prefMapDictionary = new Dictionary<string, int>();
                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_PREFERENCE_TYPE].ToString().Equals("Fixed"))
                    {
                        Fixed1.Click(MouseButtons.Left);
                        Fixed1.Click(MouseButtons.Left);
                        ComboBox13.ClickRightBound(MouseButtons.Left);
                        Wait(2000);
                        for (int i = 0; i < ComboBox13.AutomationElementWrapper.CachedChildren.Count; i++)
                        {
                            prefMapDictionary.Add(ComboBox13.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].Name, i);
                        }
                    }
                    else
                    {
                        Calculated.Click(MouseButtons.Left);
                        Calculated.Click(MouseButtons.Left);
                        ToggleButton12.Click(MouseButtons.Left);
                        Wait(2000);
                        for (int i = 0; i < XamComboEditor38.AutomationElementWrapper.CachedChildren.Count; i++)
                        {
                            prefMapDictionary.Add(XamComboEditor38.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].Name, i);
                        }
                    }
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    { 
                        bool isPresent = Convert.ToBoolean(dr[TestDataConstants.COL_Is_PrefPresent].ToString());
                        if ((prefMapDictionary.ContainsKey(dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()) && isPresent) || (!prefMapDictionary.ContainsKey(dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()) && !isPresent))
                        {
                            _res.IsPassed = true;
                        }
                        else
                        {
                            _res.IsPassed = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }
    }
}
