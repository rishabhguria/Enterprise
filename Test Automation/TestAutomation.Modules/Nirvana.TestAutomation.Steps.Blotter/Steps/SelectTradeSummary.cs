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
    class SelectTradeSummary : BlotterUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenBlotter();
                Summary.Click(MouseButtons.Left);
                MaximizeBlotter();
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

                foreach (DataRow dr in testData.Tables[0].Rows)
                {

                    IUIAutomationElementArray childElement = gridElement.FindAll(
                    TreeScope.TreeScope_Children,
                    automation.CreateTrueCondition());
                    for (int i = 0; i < childElement.Length; i++)
                    {
                        IUIAutomationElement child = childElement.GetElement(i);
                        Console.WriteLine(child.CurrentName);
                        if (child.CurrentName.Contains(dr["Symbol"].ToString()))
                        {
                            if (!GetExpandedState(child))
                            {
                                tagPOINT pt;
                                int hr = child.GetClickablePoint(out pt);
                                MouseDoubleClick(pt.x, pt.y);
                            }
                            Wait(2000);
                            IUIAutomationElementArray orderchild = child.FindAll(
                            TreeScope.TreeScope_Children,
                            automation.CreateTrueCondition());
                            IUIAutomationElementArray orderside = orderchild.GetElement(1).FindAll(
                                                                TreeScope.TreeScope_Children,
                                                                automation.CreateTrueCondition());
                            for (int j = 0; j < orderside.Length; j++) 
                            {
                                if (orderside.GetElement(j).CurrentName.Contains(dr["Side"].ToString()))
                                {
                                    if (!GetExpandedState(orderside.GetElement(j)))
                                    {
                                        tagPOINT pt;
                                        int hr = orderside.GetElement(j).GetClickablePoint(out pt);
                                        MouseDoubleClick(pt.x, pt.y);
                                    }
                                    Wait(2000);
                                    IUIAutomationElementArray items = orderside.GetElement(j).FindAll(
                                                                        TreeScope.TreeScope_Children,
                                                                        automation.CreateTrueCondition());
                                    IUIAutomationElementArray itemDetails = items.GetElement(1).FindAll(
                                                                        TreeScope.TreeScope_Children,
                                                                        automation.CreateTrueCondition());
                                    for(int k = 0; k < itemDetails.Length; k++)
                                    {
                                        IUIAutomationElementArray itemUnder = itemDetails.GetElement(k).FindAll(
                                                                        TreeScope.TreeScope_Children,
                                                                        automation.CreateTrueCondition());
                                        bool flag = true;
                                        for (int l = 0; l < itemUnder.Length; l++)
                                        {
                                            if (!string.IsNullOrEmpty(dr["Status"].ToString()))
                                            {
                                                if (itemUnder.GetElement(l).CurrentName.Equals("Status") && !GetValue(itemUnder.GetElement(l)).Equals(dr["Status"].ToString()))
                                                {
                                                    flag = false;
                                                }

                                            }
                                            if (!string.IsNullOrEmpty(dr["Executed Qty"].ToString()))
                                            {
                                                if (itemUnder.GetElement(l).CurrentName.Equals("Executed Qty") && !GetValue(itemUnder.GetElement(l)).Equals(dr["Executed Qty"].ToString()))
                                                {
                                                    flag = false;
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(dr["Avg Fill Price (Local)"].ToString()))
                                            {
                                                if (itemUnder.GetElement(l).CurrentName.Equals("Avg Fill Price (Local)") && !GetValue(itemUnder.GetElement(l)).Equals(dr["Avg Fill Price (Local)"].ToString()))
                                                {
                                                    flag = false;
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(dr["Principal Amount (Local)"].ToString()))
                                            {
                                                if (itemUnder.GetElement(l).CurrentName.Equals("Principal Amount (Local)") && !GetValue(itemUnder.GetElement(l)).Equals(dr["Principal Amount (Local)"].ToString()))
                                                {
                                                    flag = false;
                                                }
                                            }
                                        }
                                        if (flag) {
                                            double left = itemUnder.GetElement(0).CurrentBoundingRectangle.left;
                                            double top = itemUnder.GetElement(0).CurrentBoundingRectangle.top;
                                            double right = itemUnder.GetElement(0).CurrentBoundingRectangle.right;
                                            double bottom = itemUnder.GetElement(0).CurrentBoundingRectangle.bottom;

                                            int centerX = (int)((left + right) / 2);
                                            int centerY = (int)((top + bottom) / 2);

                                            SetCursorPos(centerX, centerY);
                                            System.Threading.Thread.Sleep(1000);
                                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
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
