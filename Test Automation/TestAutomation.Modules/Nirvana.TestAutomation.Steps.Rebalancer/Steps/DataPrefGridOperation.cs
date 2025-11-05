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
using Nirvana.TestAutomation.UIAutomation;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class DataPrefGridOperation : IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0].Columns.Contains("Cash"))
                {
                    testData.Tables[0].Columns.Remove("Cash");
                }
                if (testData.Tables[0].Columns.Contains("Accruals"))
                {
                    testData.Tables[0].Columns.Remove("Accruals");
                }
                if (testData.Tables[0].Columns.Contains("Other Assets  Market Value"))
                {
                    testData.Tables[0].Columns.Remove("Other Assets  Market Value");
                }
                if (testData.Tables[0].Columns.Contains("Swap NAV Adjustment"))
                {
                    testData.Tables[0].Columns.Remove("Swap NAV Adjustment");
                }
                if (testData.Tables[0].Columns.Contains("Unrealized P&L of Swaps"))
                {
                    testData.Tables[0].Columns.Remove("Unrealized P&L of Swaps");
                }
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["PranaMain"].AutomationUniqueValue);
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                uiAutomationHelper.FindAndClickElement(ApplicationArguments.mapdictionary["DataPreferenceTab"].AutomationUniqueValue);
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                System.Threading.Thread.Sleep(2000);
                IUIAutomationElement gridElement = appWindow.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "RebalancerWindow"));
                IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        "PageDown"
                    );

                IUIAutomationElement buttonElement = gridElement.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    buttonCondition
                );
                if (buttonElement != null)
                {
                    GridDataProvider.click(buttonElement);
                }
                System.Threading.Thread.Sleep(2000);
                gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["DataPrefGrid"].AutomationUniqueValue));

                var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                int row = 0;
                for (int i = 0; i < rawChildren.Length; i++)
                {
                    var child = rawChildren.GetElement(i);

                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                    {
                        var dataItemChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        DataRow dr = testData.Tables[0].Rows[row];
                        row++;
                        int column = 0;
                        for (int j = 0; j < dataItemChildren.Length; j++)
                        {
                            var dataItemChild = dataItemChildren.GetElement(j);
                            if (j == 1)
                                continue;
                            if (dr[column].ToString().Contains("ToggleState"))
                            {
                                IUIAutomationCondition checkBoxCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_ControlTypePropertyId,
                                    UIA_ControlTypeIds.UIA_CheckBoxControlTypeId
                                    );
                                IUIAutomationElement checkBoxElement = dataItemChild.FindFirst(TreeScope.TreeScope_Descendants, checkBoxCondition);
                                IUIAutomationTogglePattern togglePattern = checkBoxElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
                                if (!dr[column].ToString().Equals(togglePattern.CurrentToggleState.ToString()))
                                {
                                    togglePattern.Toggle();
                                    System.Threading.Thread.Sleep(1000);
                                }
                            }
                            column++;
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                buttonCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        "PageUp"
                    );

                buttonElement = appWindow.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    buttonCondition
                );
                if (buttonElement != null)
                {
                    GridDataProvider.click(buttonElement);
                }
                System.Threading.Thread.Sleep(2000);
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
