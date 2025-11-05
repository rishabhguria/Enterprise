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


namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class SelectTradesMTT : MultiTradingTicketUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation Automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                MultiTradingTicket.BringToFront();
                var dict = SamsaraHelperClass.GetDict(ApplicationArguments.IUIAutomationMappingTables["MultiTradingTicket"]);
                IUIAutomationElement appWindow = Automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, dict["MTTgrid"].ToString()));
                DataUtilities.RemoveTrailingZeroes(testData.Tables[0]);
                testData.Tables[0].Columns.Remove("Order Type");
                DataTable dt = GridDataProvider.GetMTTGrid(dict["MTTgrid"].ToString());
                foreach (DataRow dr in testData.Tables[0].Rows) 
                {
                    string toggle = dr["CheckBox"].ToString();
                    dr["CheckBox"] = string.Empty;
                    
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dt), dr);
                    int index = dt.Rows.IndexOf(dtRow);
                    IUIAutomationCondition parentCondition = Automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        index.ToString()
                    );
                    IUIAutomationElement parentElement = appWindow.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            parentCondition
                        );
                    if (parentElement != null)
                    {
                        
                        IUIAutomationCondition checkboxCondition = Automation.CreatePropertyCondition(
                            UIA_PropertyIds.UIA_AutomationIdPropertyId,
                            "172" // Checkbox AutomationId
                        );

                        IUIAutomationElement checkboxElement = parentElement.FindFirst(
                            TreeScope.TreeScope_Children, // Search only in the parent's children
                            checkboxCondition
                        );

                        if (checkboxElement != null)
                        {
                            object togglePatternObj = checkboxElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);

                            if (togglePatternObj != null)
                            {
                                IUIAutomationTogglePattern togglePattern = togglePatternObj as IUIAutomationTogglePattern;
                                ToggleState currentState = (ToggleState)togglePattern.CurrentToggleState;
                                if (!currentState.ToString().Equals(toggle))
                                {
                                    togglePattern.Toggle();
                                }
                                else
                                {
                                    checkboxCondition = Automation.CreatePropertyCondition(
                                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                                        "100" // Checkbox AutomationId
                                    );

                                    checkboxElement = parentElement.FindFirst(
                                        TreeScope.TreeScope_Children, // Search only in the parent's children
                                        checkboxCondition
                                    );
                                    GridDataProvider.click(checkboxElement);

                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Checkbox not found. on MTT grid");
                        }
                        Wait(2000);
                    }
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
