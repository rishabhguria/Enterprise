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
    class VerifyModelPortfolio : RebalancerUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                
                OpenRebalancer();
                Wait(4000);
                ModelPortfolio.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);

                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                    
                              automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "RebalancerWindow"));


                string values = ApplicationArguments.mapdictionary["btnViewMultipleIDs"].AutomationUniqueValue;
                string[] automationIds = values.Split(',');

                foreach (string value in automationIds)
                {
                    IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        value
                    );

                    IUIAutomationElement buttonElement = gridElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        buttonCondition
                    );

                    if (buttonElement != null)
                    {
                        IUIAutomationInvokePattern invokePattern = buttonElement.GetCurrentPattern(
                            UIA_PatternIds.UIA_InvokePatternId
                        ) as IUIAutomationInvokePattern;

                        if (invokePattern != null)
                        {
                            invokePattern.Invoke();
                        }
                        Wait(2000);
                    }
                }

               
                gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ModelPortfolioGrid"));
                Wait(2000);                

                if (gridElement == null)
                {
                    throw new Exception("Grid not found!");
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("Symbol Validity");
                dt.Columns.Add("Symbol");
                dt.Columns.Add("Target %");
                dt.Columns.Add("Tolerance %");

                var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                for (int i = 0; i < rawChildren.Length; i++)
                {
                    var child = rawChildren.GetElement(i);

                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                    {
                        DataRow dr = dt.NewRow();
                        var dataItemChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        for (int j = 1; j < dataItemChildren.Length; j++)
                        {
                            var dataItemChild = dataItemChildren.GetElement(j);
                            if (j > 4)
                                continue;
                            dr[j - 1] = dataItemChild.CurrentName.ToString();
                        }
                        dt.Rows.Add(dr);
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
