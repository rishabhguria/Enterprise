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
using System.Threading;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    class VerifySMGrid : IUIAutomationTestStep
    {
         private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ColScrollRegion: 0, RowScrollRegion: 0"));
                DataTable dt = new DataTable();
                var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                for (int i = 0; i < rawChildren.Length; i++)
                {
                    var child = rawChildren.GetElement(i);

                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderItemControlTypeId)
                    {
                        if (!string.IsNullOrEmpty(child.CurrentName.ToString()))
                        {
                            dt.Columns.Add(child.CurrentName.ToString());
                        }
                    }
                }
                var dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                IUIAutomationElementArray dataItems = gridElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);

                for (int i = 0; i < dataItems.Length; i++)
                {
                    try
                    {
                        IUIAutomationElement dataItem = dataItems.GetElement(i);
                        DataRow row = dt.NewRow();

                        IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        for (int j = 0; j < dataItemChildren.Length; j++)
                        {
                            IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                            string columnName = cellElement.CurrentName;
                            bool resulttemp = false;
                            string cellValue = ControlTypeHandler.getValueOfElement(cellElement, ref resulttemp);
                            if (dt.Columns.Contains(columnName))
                            {
                                row[columnName] = cellValue;
                            }
                        }

                        dt.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing data item " + i + ": " + ex.Message);
                    }
                }
                List<string> columns = new List<string>();
                var errors = Recon.RunRecon(testData.Tables[0], dt, columns, 0.01);
                if (errors.Count > 0)
                    _result.ErrorMessage = String.Join("\n\r", errors);
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
