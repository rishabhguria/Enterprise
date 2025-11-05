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


namespace Nirvana.TestAutomation.Steps.Blotter
{
    class SelectTradeOnBlotter : BlotterUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable temp = testData.Tables[0].Copy();
                if (temp.Columns.Contains("CheckBox"))
                {
                    temp.Columns.Remove("CheckBox");
                }
                OpenBlotter();
                MaximizeBlotter();
                Wait(3000);
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ColScrollRegion: 0, RowScrollRegion: 0"));

                if (gridElement == null)
                {
                    throw new Exception("Grid not found!");
                }

                DataTable dt = new DataTable();
                IUIAutomationElement Headers = gridElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Band Headers"));

                if (Headers != null)
                {
                    IUIAutomationElementArray childElements = Headers.FindAll(
                        TreeScope.TreeScope_Children,
                        automation.CreateTrueCondition()
                    );

                    for (int i = 0; i < childElements.Length; i++)
                    {
                        IUIAutomationElement child = childElements.GetElement(i);
                        if (!string.IsNullOrEmpty(child.CurrentName.ToString()))
                        {
                            dt.Columns.Add(child.CurrentName);
                        }
                    }
                }
                IUIAutomationElementArray childElement = gridElement.FindAll(
                TreeScope.TreeScope_Children,
                automation.CreateTrueCondition());

                for (int i = 0; i < childElement.Length; i++)
                {
                    IUIAutomationElement child = childElement.GetElement(i);
                    if (child.CurrentAutomationId.ToString().Equals("Band Headers")) {
                        continue;
                    }
                    var rawChildren = child.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < rawChildren.Length; j++)
                    {
                        if (rawChildren.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
                        {
                            string columnName = rawChildren.GetElement(j).CurrentName;
                            if (string.IsNullOrEmpty(columnName))
                                continue;
                            object valuePatternObj = rawChildren.GetElement(j).GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                            if (valuePatternObj != null)
                            {
                                IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                                string value = valuePattern.CurrentValue;
                                dr[columnName] = value;
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }

                for (int i = 0; i < testData.Tables[0].Rows.Count; i++)
                {
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dt), temp.Rows[i]);
                    int index = dt.Rows.IndexOf(dtRow);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dt, temp, new List<string>(), 0.01);
                            throw new Exception("Trade not found during SelectTradeOnBlotter step. [Symbol= " + temp.Rows[i]["Symbol"] + "], Quantity = [" + temp.Rows[i]["Target Qty"] + "] Side = [" + temp.Rows[i]["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));

                        }
                    }
                    IUIAutomationCondition parentCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        index.ToString()
                    );
                    IUIAutomationElement parentElement = appWindow.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            parentCondition
                        );
                    Wait(2000);
                    if (testData.Tables[0].Columns.Contains("CheckBox") && testData.Tables[0].Rows[i]["CheckBox"].ToString().Equals("ToggleState_On"))
                    {
                        
                        if (parentElement != null)
                        {
                            IUIAutomationCondition checkboxCondition =  automation.CreatePropertyCondition
                                (UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "check box");

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
                                    if (!currentState.ToString().Equals("ToggleState_On"))
                                    {
                                        togglePattern.Toggle();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Element does not support TogglePattern.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Checkbox not found.");
                            }
                        }
                    }
                    else {
                        if (parentElement != null)
                        {
                            IUIAutomationCondition checkboxCondition = automation.CreatePropertyCondition(
                                UIA_PropertyIds.UIA_AutomationIdPropertyId,
                                "102" // Checkbox AutomationId
                            );

                            IUIAutomationElement checkboxElement = parentElement.FindFirst(
                                TreeScope.TreeScope_Children, // Search only in the parent's children
                                checkboxCondition
                            );
                            object patternObject = checkboxElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                            if (patternObject != null)
                            {
                                IUIAutomationScrollItemPattern scrollPattern = (IUIAutomationScrollItemPattern)patternObject;
                                scrollPattern.ScrollIntoView();
                            }
                            object expandCollapsePatternObj = checkboxElement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                            if (expandCollapsePatternObj != null)
                            {
                                IUIAutomationExpandCollapsePattern expandCollapsePattern = (IUIAutomationExpandCollapsePattern)expandCollapsePatternObj;
                                expandCollapsePattern.Expand();
                            }
                            
                            double left = checkboxElement.CurrentBoundingRectangle.left;
                            double top = checkboxElement.CurrentBoundingRectangle.top;
                            double right = checkboxElement.CurrentBoundingRectangle.right;
                            double bottom = checkboxElement.CurrentBoundingRectangle.bottom;

                            int centerX = (int)((left + right) / 2);
                            int centerY = (int)((top + bottom) / 2);

                            SetCursorPos(centerX, centerY);
                            System.Threading.Thread.Sleep(1000);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                            
                        }
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
