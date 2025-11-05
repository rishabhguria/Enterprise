using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Data;
using System.Collections.Generic;
using System;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class AddCalculatedPreferences : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// begins the test execution process
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                OpenAllocation();
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "AllocationClientWindow"));
                IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    "btnEditAllocationPreferences"
                );

                IUIAutomationElement buttonElement = gridElement.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    buttonCondition
                );

                if (buttonElement != null)
                {
                    IUIAutomationInvokePattern invokePattern = buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                }
                try{
                    Records1.Click(MouseButtons.Left);
                    AddMFPreference(testData, sheetIndexToName);                 
                }catch {
                    Records.Click(MouseButtons.Left);
                    AddCalculatedPreference(testData, sheetIndexToName);
                }
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddCalculatedPreferences");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }

        private void AddMFPreference(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    string prefname = row[TestDataConstants.COL_PREFERENCE_NAME].ToString();
                    Records1.Click(MouseButtons.Left);
                    Records1.ClickRightBound(MouseButtons.Right);
                    Add2.Click(MouseButtons.Left);
                    Keyboard.SendKeys(prefname);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    if (NirvanaPreferences.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// adds a calculated preferecne
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void AddCalculatedPreference(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {

                    string prefname = row[TestDataConstants.COL_PREFERENCE_NAME].ToString();
                    Records.Click(MouseButtons.Left);
                    Dictionary<string, int> indexToName = GetIndexNameMap();
                    if (indexToName.ContainsKey(prefname))
                    {
                        DeleteExistingPref(prefname);
                        //delete existing pref
                    }
                    Records.ClickRightBound(MouseButtons.Right);
                    Add2.Click(MouseButtons.Left);
                    Keyboard.SendKeys(prefname);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    if (NirvanaPreferences.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
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
        public void DeleteExistingPref(string prefname)
        {
            try
            {
                Records.AutomationElementWrapper.FindDescendantByName(prefname).WpfClick();
                Delete.Click(MouseButtons.Left);
                if (NirvanaPreferences.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);

                }
            }
            catch (Exception)
            { throw; }
        }
    }
}