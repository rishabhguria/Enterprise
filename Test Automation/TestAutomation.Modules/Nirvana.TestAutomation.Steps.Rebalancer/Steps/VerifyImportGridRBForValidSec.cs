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


namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class VerifyImportGridRBForValidSec : RebalancerUIMap, IUIAutomationTestStep
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MAXIMIZE = 3;
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            var dict = SamsaraHelperClass.GetDict(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
            TestResult _result = new TestResult();
            try
            {
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, dict["ValidSecuritiesGrid"].ToString()));
                DataTable dt = new DataTable();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "RASImportWindow");
                IUIAutomationElement targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    ShowWindow(hWnd, SW_MAXIMIZE);
                }
                Wait(1000);
                var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                for (int i = 0; i < rawChildren.Length; i++)
                {
                    var child = rawChildren.GetElement(i);

                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderItemControlTypeId)
                    {
                        if (!string.IsNullOrEmpty(child.CurrentName.ToString())) {
                            dt.Columns.Add(child.CurrentName.ToString());
                        }
                    }
                }
                if (dt.Columns.Contains("+/-/="))
                    dt.Columns["+/-/="].ColumnName = "/-/=";
                if (testData.Tables[0].Columns.Contains("+/-/="))
                    testData.Tables[0].Columns["+/-/="].ColumnName = "/-/=";
                GetData(gridElement, dt);
                
                dt = DataUtilities.RemoveTrailingZeroes(DataUtilities.RemoveCommas(DataUtilities.RemovePercent(dt)));
                if (testData.Tables[0].Columns.Contains("Action")) {
                    
                    EditRow(testData.Tables[0], dt, dict["ValidSecuritiesGrid"].ToString());
                    dt.Clear();
                    GetData(gridElement, dt);
                }
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
