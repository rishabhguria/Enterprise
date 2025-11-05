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
    class BlotterToolBarAction : BlotterUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                MaximizeBlotter();
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(dr[TestDataConstants.Refresh_Data].ToString()))
                    {
                        RefreshData1.DoubleClick(MouseButtons.Left);
                    }

                    if (!string.IsNullOrEmpty(dr[TestDataConstants.AddWorkingTab].ToString()))
                    {
                        AddTabWorking.Click(MouseButtons.Left);
                        Wait(1000);
                        InputBox1.Click(MouseButtons.Left);
                        if (TextBox11.IsEnabled)
                        {
                            TextBox11.Click(MouseButtons.Left);
                            TextBox11.Properties["Text"] = dr[TestDataConstants.TAB_NAME].ToString();
                        }
                        BtnOK1.Click(MouseButtons.Left);

                        if (Error.IsVisible)
                        {
                            ButtonOK6.Click(MouseButtons.Left);
                            KeyboardUtilities.CloseWindow(ref InputBox_UltraFormManager_Dock_Area_Top);
                        }
                    }


                    if (!string.IsNullOrEmpty(dr[TestDataConstants.AddOrderTab].ToString()))
                    {
                        AddTabOrder.Click(MouseButtons.Left);
                        if (InputBox2.IsVisible)
                        {
                            InputBox2.Click(MouseButtons.Left);
                            if (TextBox12.IsEnabled)
                            {
                                TextBox12.Click(MouseButtons.Left);
                                TextBox12.Properties["Text"] = dr[TestDataConstants.TAB_NAME].ToString();
                            }
                            BtnOK2.Click(MouseButtons.Left);
                        }
                        if (InputBox1.IsVisible)
                        {
                            InputBox1.Click(MouseButtons.Left);
                            if (TextBox11.IsEnabled)
                            {
                                TextBox11.Click(MouseButtons.Left);
                                TextBox11.Properties["Text"] = dr[TestDataConstants.TAB_NAME].ToString();
                            }
                            BtnOK1.Click(MouseButtons.Left);
                        }
                        if (Error.IsVisible)
                        {
                            ButtonOK6.Click(MouseButtons.Left);
                            KeyboardUtilities.CloseWindow(ref InputBox_UltraFormManager_Dock_Area_Top);
                        }
                    }

                    if (!string.IsNullOrEmpty(dr[TestDataConstants.BTN_PREF].ToString()))
                    {
                        Preferences.Click(MouseButtons.Left);
                    }

                    if (!string.IsNullOrEmpty(dr[TestDataConstants.SaveLayout].ToString()))
                    {
                        SaveAllLayout.Click(MouseButtons.Left);
                    }

                    if (!string.IsNullOrEmpty(dr[TestDataConstants.RemoveOrders].ToString()))
                    {
                        RemoveOrders.Click(MouseButtons.Left);
                        if (NirvanaBlotter.IsVisible)
                        {

                            ButtonYes.Click(MouseButtons.Left);
                        }
                    }
                    if (!string.IsNullOrEmpty(dr[TestDataConstants.CancelAllSubs].ToString()))
                    {
                        CancelAllSubs1.Click(MouseButtons.Left);
                        if (NirvanaBlotter.IsVisible)
                            ButtonYes.Click(MouseButtons.Left);
                    }

                    if (!string.IsNullOrEmpty(dr[TestDataConstants.Rollover_All_Subs].ToString()))
                    {
                        RolloverAllSubs.Click(MouseButtons.Left);
                        if (Confirmation.IsVisible)
                        {
                            ButtonYes1.Click(MouseButtons.Left);
                        }
                    }

                    if (!string.IsNullOrEmpty(dr[TestDataConstants.LinkTab].ToString()))
                    {
                        LinkTab.Click(MouseButtons.Left);
                    }

                    if (dr.Table.Columns.Contains("Merge Orders") && !string.IsNullOrEmpty(dr["Merge Orders"].ToString()))
                    {
                        MergeOrders1.Click(MouseButtons.Left);
                    }

                    if (dr.Table.Columns.Contains("Toolbar : BlotterToolBar") && !string.IsNullOrEmpty(dr["Toolbar : BlotterToolBar"].ToString()))
                    {
                        IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                        TreeScope.TreeScope_Children,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                        IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Toolbar : BlotterToolBar"));
                        IUIAutomationElementArray childElement = gridElement.FindAll(
                        TreeScope.TreeScope_Children,
                        automation.CreateTrueCondition());

                        for (int i = 0; i < childElement.Length; i++)
                        {
                            IUIAutomationElement child = childElement.GetElement(i);
                            if (child.CurrentName.ToString().Equals(dr["Toolbar : BlotterToolBar"].ToString()))
                            {
                                double left = child.CurrentBoundingRectangle.left;
                                double top = child.CurrentBoundingRectangle.top;
                                double right = child.CurrentBoundingRectangle.right;
                                double bottom = child.CurrentBoundingRectangle.bottom;

                                int centerX = (int)((left + right) / 2);
                                int centerY = (int)((top + bottom) / 2);

                                SetCursorPos(centerX, centerY);
                                System.Threading.Thread.Sleep(1000);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                                break;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}
