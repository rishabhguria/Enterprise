using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class ReviewExistingPositions : AllocationUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenAllocation();
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                ReviewExistingPositions3.Click(MouseButtons.Left);
                Wait(5000);
                GetState1.Click(MouseButtons.Left);
                Wait(2000);
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "CurrentStatePane"));
                IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    "btnGetState"
                );

                IUIAutomationElement buttonElement = appWindow.FindFirst(
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
                Wait(2000);
                if (gridElement == null)
                {
                    throw new Exception("Grid not found!");
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("Symbol");
                dt.Columns.Add("AccountName");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Notional");

                var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                for (int i = 0; i < rawChildren.Length; i++)
                {
                    var child = rawChildren.GetElement(i);

                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataGridControlTypeId)
                    {
                        var dataItemChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        for (int j = 0; j < dataItemChildren.Length; j++)
                        {
                            DataRow dr = dt.NewRow();
                            var dataItemChild = dataItemChildren.GetElement(j);
                            var dataItemDesc = dataItemChild.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                            for (int k = 0; k < dataItemDesc.Length; k++)
                            {
                                var ele = dataItemDesc.GetElement(k);
                                dr[k] = ele.CurrentName.ToString();
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                dt = DataUtilities.RemoveTrailingZeroes(DataUtilities.RemoveCommas(DataUtilities.RemovePercent(dt)));
                List<string> errors = Recon.RunRecon(dt, testData.Tables[0], new List<string>(), 0.01, false, false);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage = String.Join("\n\r", errors);
                    _result.IsPassed = false;
                }

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }
    }
}
