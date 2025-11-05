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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    [UITestFixture]
    public partial class BlotterUIMap : UIMap
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        /// <summary>
        /// Initializes a new instance of the <see cref="BlotterUIMap"/> class.
        /// </summary>
        public BlotterUIMap()
        {
            InitializeComponent();
        }

        public static void MouseDoubleClick(int x, int y)
        {
            SetCursorPos(x, y);

            // First Click
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, UIntPtr.Zero);

            // Small delay to simulate a real double-click
            System.Threading.Thread.Sleep(100);

            // Second Click
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, UIntPtr.Zero);
        }

        public static bool GetExpandedState(IUIAutomationElement element)
        {
            object expandCollapsePatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
            if (expandCollapsePatternObj != null)
            {
                IUIAutomationExpandCollapsePattern expandCollapsePattern =
                    expandCollapsePatternObj as IUIAutomationExpandCollapsePattern;

                // Get the current ExpandCollapse state
                ExpandCollapseState state = (ExpandCollapseState)expandCollapsePattern.CurrentExpandCollapseState;

                if(state.ToString().Contains("Collapsed")){
                    return false;
                }
            }
            return true;
        }

        public static string GetValue(IUIAutomationElement element)
        {
            object valuePatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
            if (valuePatternObj != null)
            {
                IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                // Get the current Value property
                string currentValue = valuePattern.CurrentValue;
                return currentValue;
            }
            return string.Empty;

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BlotterUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public BlotterUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public Boolean SelectBlotterTrades(UIUltraGrid dgBlotter, DataRow testDataRow)
        {

            try
            {
                DataTable dtBlotter =
                        CSVHelper.CSVAsDataTable(dgBlotter.Properties[ExcelStructureConstants.COL_DESCRIPTION].ToString());
                var msaaObj = dgBlotter.MsaaObject;
                string matchcolumns = @"" + TestDataConstants.COL_SYMBOL + " = '{0}' AND " + TestDataConstants.COL_QUANTITY +
                                      " = '{1}'  AND " + TestDataConstants.COL_ORDERSIDE + "='{2}' AND " +
                                      TestDataConstants.COL_ORDERSTATUS + "='{3}'";
                DataRow[] dtRow =
                    dtBlotter.Select(String.Format(matchcolumns, testDataRow[TestDataConstants.COL_SYMBOL],
                        testDataRow[TestDataConstants.COL_QUANTITY], testDataRow[TestDataConstants.COL_ORDERSIDE],
                        testDataRow[TestDataConstants.COL_ORDERSTATUS]));
                if (dtRow.Length > 0)
                {
                    int index = dtBlotter.Rows.IndexOf(dtRow[dtRow.Length - 1]);
                    dgBlotter.InvokeMethod("ScrollToRow", index);

                    msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Left);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return false;
            }
        }

        private void BlotterMain_AttachFailing(object sender, AttachFailingEventArgs e)
        {
            try
            {
                if (e.CurrentRetryCount > 2)
                    BlotterMain2.AttachFailing -= BlotterMain_AttachFailing;
                else
                {
                    uiWindow1.MatchedIndex = 1;
                    BlotterMain2.MatchedIndex = 1;
                   // Wait(1000);
                    uiWindow1.MatchedIndex = 0;
                    BlotterMain2.MatchedIndex = 0;
                   // Wait(1000);
                    e.Action = AttachFailingAction.Retry;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the blotter.
        /// </summary>
        protected void OpenBlotter()
        {
            try
            {
                //  Shortcut to open blotter module(CTRL + SHIFT + B)
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_BLOTTER"]);
                ExtentionMethods.WaitForVisible(ref BlotterMain2, 15);
                DgBlotter.BringToFront();
                //Trade.Click(MouseButtons.Left);
                //Blotter.Click(MouseButtons.Left);
               // Wait(6000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the blotter.
        /// </summary>
        protected void MinimizeBlotter()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
              //  Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected void ClickAddDivideModifyFills()
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected void ClickAllocateOnSubOrder()
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Minimize the View PTT Allocation Window.
        /// </summary>
        protected void MinimizeViewPTTAllocation()
        {
            try
            {
                PercentTradingTool.Click(MouseButtons.Left);
                PercentTradingTool.Click(MouseButtons.Right);
                KeyboardUtilities.MinimizeWindow(ref PercentTradingTool);
               // Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Opens the blotter.
        /// </summary>
        protected void CloseBlotter()
        {
            try
            {
               // Wait(3000);
                BlotterMain2.BringToFront();
                OrdersTab.Click(MouseButtons.Left);
                KeyboardUtilities.CloseWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Maximize the blotter.
        /// </summary>
        protected void MaximizeBlotter()
        {
            try
            {
                KeyboardUtilities.MaximizeWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
               // Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// method to choose the tab
        /// </summary>
        /// <param name="testData"></param>
        protected void ChooseTab(string tabName)
        {
            try
            {
                WorkingSubsTab.MsaaName = tabName;
                WorkingSubsTab.WaitForVisible();
                if (WorkingSubsTab.IsValid)
                    WorkingSubsTab.Click();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To get all required columns on grid.
        /// </summary>
        /// <param name="dTable"></param>
        protected void ViewAllColumnsOnGrid(DataTable dTable)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in dTable.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                this.DgBlotter1.InvokeMethod("AddColumnsToGrid", columns);
                SaveAllLayout.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }
        protected void HandlePopup(DataTable dTable)
        {
            try
            {
                DataRow dr = dTable.Rows[0];
                String Allowmerge = (dr[TestDataConstants.COL_ALLOWMERGE].ToString());
                Wait(5000);
                if (Allowmerge.ToUpper() == "YES")
                {
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                
                if (Allowmerge.Equals(String.Empty))
                {
                    Console.WriteLine("ALLOW MERGE COLUMN IS EMPTY");
                    throw new ApplicationException("ALLOW MERGE COLUMN IS EMPTY");
                }
                if (Allowmerge.ToUpper() == "NO")
                {
                    Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }


              
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }


        protected bool pickFromMenuItem(UIWindow PopupMenuContext, string itemToSelect, UIWindow futureExpectedWindow= null)
        {
            bool isExpectedResultAchieved = false;
            try
            {
               // List<string> li = new List<string>();
                if (PopupMenuContext.IsVisible)
                {
                    var popUpMenuContext = PopupMenuContext.MsaaObject;

                    int itemsCount = popUpMenuContext.ChildCount;
                    for (int i = 0; i < itemsCount; i++)
                    {
                        var menuItem = popUpMenuContext.CachedChildren[i];
                       //  
                        //string itemText = menuItem.Value;

                        if (!string.IsNullOrEmpty(menuItem.Name))
                        {
                            //li.Add(menuItem.Name.ToString());
                            string itemName = menuItem.Name.ToString();
                            if (string.Equals(itemName, itemToSelect, StringComparison.OrdinalIgnoreCase))
                            {

                                menuItem.Click();

                                try
                                {
                                    if (futureExpectedWindow != null)
                                    {
                                        if (futureExpectedWindow.IsAttached)
                                        {
                                            isExpectedResultAchieved = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        isExpectedResultAchieved = true;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "Expected window not achieved after selecting menu item");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return isExpectedResultAchieved;
        }


        public void SamsaraTestDataHandler(string StepName, DataTable UIData, DataTable ExcelData, List<String> columns)
        {
            try
            {
                DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref  UIData);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occur on SamsaraTestDataHandler :" + ex.Message);
            }
        }

    }
}
