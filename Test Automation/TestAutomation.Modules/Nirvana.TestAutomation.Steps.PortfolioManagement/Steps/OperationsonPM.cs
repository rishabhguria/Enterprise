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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class OperationsonPM : PortfolioManagementUIMap, IUIAutomationTestStep
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private static CUIAutomation automation = new CUIAutomation();

        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable dt = SqlUtilities.GetDataFromQuery("SELECT * FROM T_CompanyMarketDataProvider where CompanyId>0", "Client");
                int ExportState = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["IsMarketDataBlocked"].ToString().ToUpper().Equals("TRUE"))
                    {
                        ExportState = 1;
                    }
                }
                OpenConsolidationView();
                Main.BringToFront();
                Wait(3000);
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                if (appWindow != null)
                {
                    IUIAutomationElement targetElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ColScrollRegion: 0, RowScrollRegion: 0"));

                    if (targetElement != null)
                    {
                        if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_ROWINDEXWISE) && !string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_ROWINDEXWISE].ToString()))
                        {
                            
                            GridDataProvider gridDataobj = new GridDataProvider();

                            int maxRetries = 2;
                            int attempt = 0;
                            bool success = false;

                            while (attempt <= maxRetries && !success)
                            {
                                try
                                {
                                    ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["PM"]);
                                    gridDataobj.OperationsonPM(testData.Tables[0], targetElement);
                                    success = true;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Attempt " + (attempt + 1).ToString() + " failed: " + ex.Message);
                                    attempt++;

                                    if (attempt > maxRetries)
                                    {
                                        Console.WriteLine("Maximum retry attempts reached. OperationsonPM failed.");
                                        throw new Exception("OperationsonPM failed. :" + ex.InnerException +","+ex.Message);
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(500); 
                                    }
                                }
                            }

                        }
                        else
                        {
                        var elementRect = targetElement.CurrentBoundingRectangle;

                        System.Drawing.Point clickPoint = new System.Drawing.Point(
                            (int)(elementRect.left + (elementRect.right - elementRect.left) / 2),
                            (int)(elementRect.top + (elementRect.bottom - elementRect.top) / 2));

                        Cursor.Position = clickPoint;
                        MouseRightClick();
                        
                            
                        Wait(2000);

                        IUIAutomationElement contextMenu = automation.GetRootElement().FindFirst(
                        TreeScope.TreeScope_Subtree,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_MenuControlTypeId));

                        if (contextMenu != null)
                        {
                            string action = string.Empty;
                            if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0]["RightClickOperations"].ToString()))
                            {
                                action = InsertSpaceBeforeSecondCapital(testData.Tables[0].Rows[0]["RightClickOperations"].ToString());
                                IUIAutomationElement menuItem = contextMenu.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, action));

                                if (menuItem != null)
                                {
                                    var legacyPatternObj = menuItem.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId) as IUIAutomationLegacyIAccessiblePattern;

                                    if (legacyPatternObj != null)
                                    {
                                        IUIAutomationLegacyIAccessiblePattern legacyPattern = (IUIAutomationLegacyIAccessiblePattern)legacyPatternObj;

                                        // Get the LegacyIAccessibleState
                                        var legacyState = legacyPattern.CurrentState;

                                        // Display the state value
                                        if (legacyState == 1 && ExportState == 1)
                                        {
                                            Console.WriteLine(action + " is Disabled as Expected");
                                        }
                                        else if(legacyState == 1)
                                        {
                                            throw new Exception(action + " is Disabled and not working according to admin");
                                        }
                                        else if (ExportState == 1 && action.ToUpper().Contains("EXPORT")) {
                                            throw new Exception(action + " is Enabled and not working according to admin");
                                        }
                                    }
                                    IUIAutomationInvokePattern invokePattern = menuItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                                    if (invokePattern != null)
                                    {
                                        invokePattern.Invoke();
                                    }
                                    Wait(2000);
                                }
                                else
                                {
                                    Console.WriteLine("Menu item not found.");
                                }
                            }


                        else
                        {
                            Console.WriteLine("Context menu not found.");
                        }
                    }

                        }
                    }

                    else
                    {
                        Console.WriteLine("Target element not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Application window not found.");
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
        private static void MouseRightClick()
        {
            // Simulate a right-click at the current cursor position
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, (UIntPtr)0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, (UIntPtr)0);
        }

        static string InsertSpaceBeforeSecondCapital(string input)
        {
            int capitalCount = 0;

            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    capitalCount++;
                    if (capitalCount == 1 && input[i - 1] != ' ')
                    {
                        return input.Insert(i, " ");
                    }
                }
            }
            return input;
        }

        
    }
}
