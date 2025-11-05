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
    class CustomOrderMTT : MultiTradingTicketUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation Automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                MultiTradingTicket.BringToFront();
                DataTable ExcelData = testData.Tables[0].Copy();
                try
                {
                    List<String> columns = new List<string>();
                    string StepName = "CustomOrderMTT";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref ExcelData);
                }
                catch (Exception)
                { }
                var dict = SamsaraHelperClass.GetDict(ApplicationArguments.IUIAutomationMappingTables["MultiTradingTicket"]);
                IUIAutomationElement appWindow = Automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                TreeScope.TreeScope_Descendants,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "MultiTradingTicket"));
                string buttonName = string.Empty;
                foreach (DataRow dr in ExcelData.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["ButtonRefreshPrice"].ToString())) 
                    {
                        buttonName = dict["ButtonRefreshPrice"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["ButtonDoneAway"].ToString()))
                    {
                        buttonName = dict["ButtonDoneAway"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["ButtonCreateOrder"].ToString()))
                    {
                        buttonName = dict["ButtonCreateOrder"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["ButtonSend"].ToString()))
                    {
                        buttonName = dict["ButtonSend"].ToString();
                    }
                    IUIAutomationCondition buttonCondition = Automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        buttonName
                    );

                    IUIAutomationElement buttonElement = gridElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        buttonCondition
                    );

                    if (buttonElement != null)
                    {
                        try
                        {
                            IUIAutomationInvokePattern invokePattern = buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                            if (invokePattern != null)
                            {
                                invokePattern.Invoke();
                            }
                        }
                        catch (Exception ex) {
                            Console.WriteLine(ex.Message);
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
