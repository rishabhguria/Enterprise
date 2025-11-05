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
using System.Data.SqlClient;
using System.Configuration;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class VerifyDelRASGrid : RebalancerUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        private static DataTable dt = new DataTable();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                dt = new DataTable();
                OpenRebalancer();
                Wait(2000);
                RebalancerTabButton.Click(MouseButtons.Left);
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "RebalancerWindow"));
                IUIAutomationElement ras = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["RASExpander"].AutomationUniqueValue));

                if (ras != null)
                {
                    var legacyPatternObj = ras.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId) as IUIAutomationLegacyIAccessiblePattern;

                    if (legacyPatternObj != null)
                    {
                        IUIAutomationLegacyIAccessiblePattern legacyPattern = (IUIAutomationLegacyIAccessiblePattern)legacyPatternObj;

                        var legacyState = legacyPattern.CurrentDefaultAction;
                        if (!legacyState.ToString().Contains("Collapse"))
                        {
                            GridDataProvider.click(ras);
                            Wait(2000);
                        }
                    }
                }

                IUIAutomationElement actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["ScrollerDown"].AutomationUniqueValue));

                if (actionItem != null)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        GridDataProvider.click(actionItem);
                        Wait(500);
                    }
                    
                }

                gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["DelRasGrid"].AutomationUniqueValue));
                
                if (gridElement != null)
                {
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

                    GetGridData(gridElement);
                }

                //btnRemoveAll
                dt = DataUtilities.RemoveTrailingZeroes(DataUtilities.RemoveCommas(DataUtilities.RemovePercent(dt)));
                if (testData.Tables[0].Columns.Contains("Action") && testData.Tables[0].Rows[0]["Action"].ToString().ToUpper().Equals("REMOVEALL"))
                {
                    actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["btnRemoveAll"].AutomationUniqueValue));
                    if (actionItem != null)
                    {
                        IUIAutomationInvokePattern invokePattern = actionItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                        if (invokePattern != null)
                        {
                            invokePattern.Invoke();
                        }
                    }
                    if (NirvanaAlert1.IsVisible)
                        ButtonYes2.Click(MouseButtons.Left);

                }
                else
                {
                    if (testData.Tables[0].Columns.Contains("Action")) 
                    {
                        ActionOnGrid(testData.Tables[0], dt, gridElement);
                        for (int i = testData.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            if (testData.Tables[0].Rows[i]["Action"].ToString().ToUpper().Equals("EDIT"))
                            {
                                testData.Tables[0].Rows[i].Delete();
                            }
                        }
                        testData.Tables[0].Columns.Remove("Action");

                    }
                    List<string> errors = Recon.RunRecon(dt, testData.Tables[0], new List<string>(), 0.01, false, false);
                    if (errors.Count > 0)
                    {
                        _result.ErrorMessage = String.Join("\n\r", errors);
                        _result.IsPassed = false;
                    }
                     
                }
                actionItem = appWindow.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["ScrollerUp"].AutomationUniqueValue));

                if (actionItem != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GridDataProvider.click(actionItem);
                        Wait(500);
                    }

                }
                RebalanceAcrossSecurities2.Click(MouseButtons.Left);
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

        private void GetGridData(IUIAutomationElement gridElement)
        {
            var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
            dt.Clear();
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
                        dr[j - 1] = dataItemChild.CurrentName;
                    }
                    dt.Rows.Add(dr);
                }
            }
        }

        private void ActionOnGrid(DataTable dataTable, DataTable UiDataTable, IUIAutomationElement gridElement)
        {
            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dataTable.Rows[i];
                int index = -1;
                if (!string.IsNullOrEmpty(dr["Action"].ToString()) && !dr["Action"].ToString().ToUpper().Equals("EDIT"))
                {
                    string action = dr["Action"].ToString();
                    dr["Action"] = string.Empty;
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(UiDataTable), dr);
                    index = UiDataTable.Rows.IndexOf(dtRow);
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(UiDataTable, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Row not found \nRecon Error: " + String.Join("\n\r", errors));
                    }
                    IUIAutomationElement childElement = gridElement.FindFirst(
                            TreeScope.TreeScope_Children,
                            automation.CreateTrueCondition()
                        );
                    IUIAutomationElementArray rawChildren = childElement.FindAll(
                        TreeScope.TreeScope_Children,
                        automation.CreateTrueCondition());
                    object selectObject = rawChildren.GetElement(index + 1).GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                    if (selectObject != null)
                    {
                        IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                        selectObjectPattern.Select();
                    }
                    IUIAutomationElementArray elementsToEdit = rawChildren.GetElement(index + 1).FindAll(
                        TreeScope.TreeScope_Children,
                        automation.CreateTrueCondition()
                    );
                    if (action.ToUpper().Equals("REMOVEROW"))
                    {
                        IUIAutomationElement ele = elementsToEdit.GetElement(elementsToEdit.Length - 1);
                        if (ele != null)
                            GridDataProvider.click(ele);
                        if (NirvanaAlert1.IsVisible)
                            ButtonYes2.Click(MouseButtons.Left);
                        UiDataTable.Rows[index].Delete();
                        dataTable.Rows[i].Delete();
                    }
                    else if (action.ToUpper().Equals("SELECT")) 
                    {
                        bool target = false;
                        bool price = false;
                        bool fx = false;
                        bool calculation = false;
                        for (int j = 2; j < elementsToEdit.Length; j++)
                        {
                            IUIAutomationElement ele = elementsToEdit.GetElement(j);
                            if (!calculation && (!string.IsNullOrEmpty(dataTable.Rows[i + 1]["+/-/="].ToString()) && ele.CurrentName.Equals(dr["+/-/="].ToString())))
                            {
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(dataTable.Rows[i + 1]["+/-/="].ToString());
                                dataTable.Rows[i]["+/-/="] = dataTable.Rows[i + 1]["+/-/="].ToString();
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                calculation = true;
                            }

                            if (!target && (!string.IsNullOrEmpty(dataTable.Rows[i + 1]["Target"].ToString()) && ele.CurrentName.Equals(dr["Target"].ToString())))
                            {
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(dataTable.Rows[i + 1]["Target"].ToString());
                                dataTable.Rows[i]["Target"] = dataTable.Rows[i + 1]["Target"].ToString();
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                target = true;
                            }
                            if (!price && (!string.IsNullOrEmpty(dataTable.Rows[i + 1]["Price"].ToString()) && ele.CurrentName.Equals(dr["Price"].ToString())))
                            {
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(dataTable.Rows[i + 1]["Price"].ToString());
                                dataTable.Rows[i]["Price"] = dataTable.Rows[i + 1]["Price"].ToString();
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                price = true;
                            }
                            if (!fx && (!string.IsNullOrEmpty(dataTable.Rows[i + 1]["Fx"].ToString()) && ele.CurrentName.Equals(dr["Fx"].ToString())))
                            {
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(dataTable.Rows[i + 1]["Fx"].ToString());
                                dataTable.Rows[i]["Fx"] = dataTable.Rows[i + 1]["Fx"].ToString();
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                fx = true;
                            }
                        }
                    
                    }
                    GetGridData(gridElement);
                }
            }
        }

    }
}
