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

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class SelectAndVerifyTradeList : RebalancerUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName) {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["TradeBuySellListView"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                    TreeScope.TreeScope_Children,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                
                bool isTradeBuySellListViewOpened = false;
                isTradeBuySellListViewOpened = UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                int maxRetries = 2;
                int currentRetry = 0;
                while (!isTradeBuySellListViewOpened && currentRetry < maxRetries)
                {
                    try
                    {
                        isTradeBuySellListViewOpened = UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                        if (isTradeBuySellListViewOpened.Equals(false))
                        {
                            currentRetry++; //added counter increment to prevent from infinite looping , Modified by Yash Gupta
                        }
                    }
                    catch (Exception ex)
                    {
                        currentRetry++;
                        Console.WriteLine("Attempt " + currentRetry + " failed: " + ex.Message);

                        if (currentRetry >= maxRetries)
                        {
                            Console.WriteLine("Max retries reached. Could not find Trade Buy/Sell List View.");
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
                IUIAutomationElement gridElement = null;
                if (!isTradeBuySellListViewOpened)
                {
                    
                     gridElement = appWindow.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "RebalancerWindow"));
                    IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        "btnViewBuySellList"
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
                        Wait(2000);
                    }
                }
                 gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TradeListGrid"));
                DataTable dt = new DataTable();
                dt.Columns.Add("Account");
                dt.Columns.Add("Trading Group Name");
                dt.Columns.Add("Symbol");
                dt.Columns.Add("Price (Local)");
                dt.Columns.Add("Price (Base)");
                dt.Columns.Add("Previous Day Closing Price (Local)");
                dt.Columns.Add("% Change from Closing Price (Local)");
                dt.Columns.Add("Side");
                dt.Columns.Add("Current Market Value");
                dt.Columns.Add("Current %");
                dt.Columns.Add("Target Market Value");
                dt.Columns.Add("Target %");
                dt.Columns.Add("Current Position");
                dt.Columns.Add("Target Position");
                dt.Columns.Add("Buy/Sell Qty");
                dt.Columns.Add("Buy/Sell Value (Local)");
                dt.Columns.Add("Buy/Sell Value");
                dt.Columns.Add("Current FX Rate");
                dt.Columns.Add("Comments");
                
                var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                for (int i = 0; i < rawChildren.Length; i++)
                {
                    var child = rawChildren.GetElement(i);

                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                    {
                        var dataItemChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        DataRow dr = dt.NewRow();
                        if (dataItemChildren.Length < 15)
                            continue;
                        for (int j = 1; j < dataItemChildren.Length; j++)
                        {
                            var dataItemChild = dataItemChildren.GetElement(j);
                            if (j == 1)
                            {
                                IUIAutomationCondition checkBoxCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_ControlTypePropertyId,
                                    UIA_ControlTypeIds.UIA_CheckBoxControlTypeId
                                    );
                                IUIAutomationElement checkBoxElement = dataItemChild.FindFirst(TreeScope.TreeScope_Descendants, checkBoxCondition);
                                IUIAutomationTogglePattern togglePattern = checkBoxElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;

                                if (testData.Tables[0].Columns.Contains("CheckBox"))
                                {
                                    if(togglePattern != null && !testData.Tables[0].Rows[dt.Rows.Count]["CheckBox"].ToString().Equals("ToggleState_On"))
                                    {
                                        togglePattern.Toggle();
                                        Console.WriteLine("Checkbox UnToggled.");
                                    }
                                }
                                continue;
                            }
                            dr[j - 2] = dataItemChild.CurrentName;
                        }
                        dt.Rows.Add(dr);

                    }
                }
                if (testData.Tables[0].Columns.Contains("CheckBox"))
                {
                    testData.Tables[0].Columns.Remove("CheckBox");
                }
                DataUtilities.RemoveTrailingZeroes(DataUtilities.RemoveCommas(DataUtilities.RemovePercent(testData.Tables[0])));
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
