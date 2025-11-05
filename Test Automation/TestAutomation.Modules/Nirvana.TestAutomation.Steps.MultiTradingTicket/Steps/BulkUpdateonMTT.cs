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
    class BulkUpdateonMTT: MultiTradingTicketUIMap,IUIAutomationTestStep
    {
        private static CUIAutomation Automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                MultiTradingTicket.BringToFront();
                var dict = SamsaraHelperClass.GetDict(ApplicationArguments.IUIAutomationMappingTables["MultiTradingTicket"]);
                DataRow dr = testData.Tables[0].Rows[0];
                IUIAutomationElement appWindow = Automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                TreeScope.TreeScope_Descendants,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "MultiTradingTicket"));
                foreach (DataColumn col in testData.Tables[0].Columns) 
                {
                    if (!string.IsNullOrEmpty(dr[col].ToString()))
                    {
                        try
                        {
                            IUIAutomationCondition buttonCondition = Automation.CreatePropertyCondition(
                                UIA_PropertyIds.UIA_AutomationIdPropertyId,
                                dict[col.ToString()].ToString()
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
                                else
                                {
                                    object selectObject = buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                    if (selectObject != null)
                                    {
                                        IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                        selectObjectPattern.Select();
                                        Wait(1500);
                                        DataUtilities.clearTextData(15);
                                        Keyboard.SendKeys(dr[col].ToString());
                                    }
                                    else
                                    {
                                        GridDataProvider.click(buttonElement);
                                        Wait(1000);
                                        DataUtilities.clearTextData(15);
                                        Keyboard.SendKeys(dr[col].ToString());
                                    }
                                    if (dr[col].ToString().Equals("Good Till Date")) 
                                    {
                                         buttonCondition = Automation.CreatePropertyCondition(
                                            UIA_PropertyIds.UIA_AutomationIdPropertyId,
                                            "btnExpireTime"
                                        );
                                         buttonElement = gridElement.FindFirst(
                                             TreeScope.TreeScope_Descendants,
                                             buttonCondition
                                         );
                                         invokePattern = buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                                         if (invokePattern != null)
                                         {
                                             invokePattern.Invoke();
                                         }
                                         Wait(1000);
                                        GTD(dr);
                                    }
                                }
                            }
                        }
                        catch { }
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
        protected void GTD(DataRow dr)
        {

            string tempDate = DataUtilities.DateHandler(dr[TestDataConstants.COL_Expiration_Date].ToString());
            string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
            string str = date;
            string[] dates = str.Split('/');
            var nextDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[0]), int.Parse(dates[1]));
            var today = DateTime.Now;
            var diffOfDates = nextDate.Day - today.Day;
            Wait(1000);
            for (int i = 0; i < diffOfDates; i++)
            {
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
            }
            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            return;
        }

    }
}
