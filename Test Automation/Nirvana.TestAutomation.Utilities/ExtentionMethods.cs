using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.Core.UIAutomationSupport;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Utilities
{
    public static class ExtentionMethods
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        static HighlightRectangle highlightRectangle = null;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static HighlightRectangle GetInstance()
        {
            try
            {
                if (highlightRectangle == null)
                {
                    highlightRectangle = new HighlightRectangle();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Highlightlight the rectangle is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return highlightRectangle;
        }
        /// <summary>
        /// Clicks the specified button.
        /// </summary>
        /// <param name="msaa">The msaa.</param>
        /// <param name="button">The button.</param>
        public static void Click(this MsaaObject msaa, MouseButtons button)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on button is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Double the specified button.
        /// </summary>
        /// <param name="msaa">The msaa.</param>
        /// <param name="button">The button.</param>
        public static void DoubleClick(this MsaaObject msaa, MouseButtons button)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.DoubleClick(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on button is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks the specified target.
        /// </summary>
        /// <param name="uiObject">The UI object.</param>
        /// <param name="target">The target string.</param>
        public static void Click(this MsaaObject msaa, string target)
        {
            try
            {
                for (int i = 0; i < msaa.CachedChildren.Count; i++)
                {
                    if (msaa.CachedChildren[i].Name == target)
                    {
                        msaa.CachedChildren[i].Click(MouseButtons.Left);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on specified object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        //Click on Msaa Object according to Mouse button
        public static void Click(this UIObject uiObject, MouseButtons button)
        {
            try
            {
                Rectangle bounds = uiObject.Bounds;
                TestStatusLoggerManager.ReportStatus(string.Format("Clicking on Object {0} ", uiObject.Name));
                if (bounds.X < 0 || bounds.Y < 0)
                {
                    //throw new Exception("Button not available in visible area");
                }
                HighlightRectangle clickArea = GetInstance();
                clickArea.Bounds = bounds;
                clickArea.Color = Color.Red;
                clickArea.Visible = true;
                MouseController.MoveTo(uiObject.ClickPoint.X, uiObject.ClickPoint.Y, TestAutomationFX.Core.UI.MousePath.Straight);
                MouseController.Click(button);
                clickArea.Color = Color.DeepSkyBlue;
                clickArea.Opacity = 0.40;
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on UI object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        //Click on Msaa Object according to Mouse button
        public static void DoubleClick(this UIObject uiObject, MouseButtons button)
        {
            try
            {
                Rectangle bounds = uiObject.Bounds;
                TestStatusLoggerManager.ReportStatus(string.Format("Clicking on Object {0} ", uiObject.Name));
                if (bounds.X < 0 || bounds.Y < 0)
                {
                    //throw new Exception("Button not available in visible area");
                }
                HighlightRectangle clickArea = GetInstance();
                clickArea.Bounds = bounds;
                clickArea.Color = Color.Red;
                clickArea.Visible = true;
                MouseController.MoveTo(uiObject.ClickPoint.X, uiObject.ClickPoint.Y, TestAutomationFX.Core.UI.MousePath.Straight);
                MouseController.DoubleClick(button);
                clickArea.Color = Color.DeepSkyBlue;
                clickArea.Opacity = 0.40;
            }
            catch (Exception ex)
            {
                Log.Error("Double click on UI object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        //Left click on Msaa Object  
        public static void Click(this MsaaObject msaa)
        {
            try
            {
                msaa.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                Log.Error("Left click on msaa object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        //Click on the mid of msaa object
        public static void Click(this MsaaObject msaa, MsaaObject targetMsaa)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                Rectangle targetBounds = targetMsaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Drag(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, targetBounds.Left + targetBounds.Width / 2, targetBounds.Top + targetBounds.Height / 2);
            }
            catch (Exception ex)
            {
                Log.Error("Click on the mid of msaa object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks on the mid bound of the object
        /// </summary>
        /// <param name="autoWrapper"></param>
        /// <param name="button"></param>
        public static void WpfClick(this AutomationElementWrapper autoWrapper, MouseButtons button)
        {
            try
            {
                Rectangle bounds = autoWrapper.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicks on the mid bound of the WPF object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;           
            }

        }

        /// <summary>
        /// Clicks on Left bound of the object
        /// </summary>
        /// <param name="autoWrapper"></param>
        /// <param name="button"></param>
        public static void WpfClickLeftBound(this AutomationElementWrapper autoWrapper, MouseButtons button)
        {
            try
            {
                Rectangle bounds = autoWrapper.Bounds;
                MouseController.MoveTo(bounds.Left + 1, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicks on the left bound of the WPF object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks at the specified distance from Left bound of the object
        /// </summary>
        /// <param name="autoWrapper"></param>
        /// <param name="button"></param>
        public static void WpfClickLeftBound(this AutomationElementWrapper autoWrapper, MouseButtons button,int distanceFromLeft)
        {
            try
            {
                Rectangle bounds = autoWrapper.Bounds;
                MouseController.MoveTo(bounds.Left + distanceFromLeft, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicks on the left bound at a specified distance of the WPF object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks at the specified distance from Bottom bound of the object
        /// </summary>
        /// <param name="autoWrapper"></param>
        /// <param name="button"></param>
        public static void WpfClickBottomBound(this AutomationElementWrapper autoWrapper, MouseButtons button, int distanceFromBottom)
        {
            try
            {
                Rectangle bounds = autoWrapper.Bounds;
                MouseController.MoveTo(bounds.Left + distanceFromBottom, bounds.Bottom - distanceFromBottom, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicks on the Bottom bound at a specified distance of the WPF object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks on Right bound of the object
        /// </summary>
        /// <param name="autoWrapper"></param>
        /// <param name="button"></param>
        public static void ClickRightBound(this UIObject uiObject, MouseButtons button)
        {
            try
            {
                Rectangle bounds = uiObject.Bounds;
                MouseController.MoveTo(bounds.Right - 20, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Straight);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicks on the right bound of the WPF object is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Wpf Click
        /// </summary>
        /// <param name="autoWrapper"></param>
        public static void WpfClick(this AutomationElementWrapper autoWrapper)
        {
            try
            {
                autoWrapper.WpfClick(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                Log.Error("WPF Clicks is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// *** Handle cell value blank and special character of input sheet ***
        ///- If the user doesn't enter any value in the cell of excel sheet data, then it means no need to change the particular column value in the control. (It will remain same as before) 
        ///- If the user enter a special character (i.e. "$#$") in the cell of excel sheet data, then it means send blank in the particular column value in the control. 
        /// </summary>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="appnedValue">The appned value.</param>
        /// <param name="isTextBoxControl">is Text Box Control</param>
        public static void CheckCellValueConditions(string cellValue, string appnedValue, bool isTextBoxControl)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    if (isTextBoxControl)
                    {
                        Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                        KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                    }

                    if (!cellValue.Equals(ExcelStructureConstants.BLANK_CONST))
                        Keyboard.SendKeys(cellValue + appnedValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Check cell value is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Deleting and updating cell value
        /// Deleting is done using shift+home
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="appnedValue"></param>
        /// <param name="isTextBoxControl"></param>
        public static void UpdateCellValueConditions(string cellValue, string appnedValue, bool isTextBoxControl)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    if (isTextBoxControl)
                    {
                        Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                        Keyboard.SendKeys(KeyboardConstants.SHIFTHOMEKEY);
                        Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    }

                    if (!cellValue.Equals(ExcelStructureConstants.BLANK_CONST))
                        Keyboard.SendKeys(cellValue + appnedValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update cell value is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Deleting and updating cell value it will handle case where Zero is left by default in textBox
        /// Deleting is done using shift+home
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="appnedValue"></param>
        /// <param name="isTextBoxControl"></param>
        public static void UpdateCellValueConditions(string cellValue, string appnedValue, UIAutomationElement TextBoxElement)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    Stopwatch tmr = new Stopwatch();
                    tmr.Start();

                    while (TextBoxElement.Text.Length > 0 && tmr.ElapsedMilliseconds <= 5000)
                    {
                        if (!TextBoxElement.Text.Equals("0"))
                        {
                            TextBoxElement.Click(MouseButtons.Left);
                            Keyboard.SendKeys("[HOME]");
                            MouseController.DoubleClick();
                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                        }
                        else
                            break;
                    }

                    tmr.Stop();
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    if (!cellValue.Equals(ExcelStructureConstants.BLANK_CONST))
                        Keyboard.SendKeys(cellValue + appnedValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update cell value for WPF grid is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Update value after deleting the text using timer
        /// Deleting is done using shift+home
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="appnedValue"></param>
        /// <param name="isTextBoxControl"></param>
        public static void UpdateCellValueConditions(string cellValue, string appnedValue)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    Stopwatch tmr = new Stopwatch();
                    tmr.Start();

                    while (tmr.ElapsedMilliseconds <= 5000)
                    {
                            Keyboard.SendKeys("[HOME]");
                            MouseController.DoubleClick();
                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    }

                    tmr.Stop();
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    if (!cellValue.Equals(ExcelStructureConstants.BLANK_CONST))
                        Keyboard.SendKeys(cellValue + appnedValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update cell value is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// select multiple items from combo
        /// </summary>
        /// <param name="items"></param>
        /// <param name="multiSelectCombo"></param>
        /// <param name="isMatchExactItem">matches exact item name in drop down</param>
        public static void SelectMultipleItemsFromCombo(List<string> items, UIWindow multiSelectCombo, bool isMatchExactItem = true)
        {
            try
            {
                Dictionary<int, string> allItems = (Dictionary<int, string>)multiSelectCombo.InvokeMethod("GetAllItemsInDictionary", null);
                foreach (int key in allItems.Keys.ToList())
                {
                    //this matches exact item name in drop down list
                    if(isMatchExactItem && !items.Contains(allItems[key]))
                    {
                        allItems.Remove(key);
                    }

                    //this matches if item name is contained in drop down list
                    if (!isMatchExactItem && !items.Any(x => allItems[key].Contains(x.Trim())))
                    {
                        allItems.Remove(key);
                    }
                }
                object[] parameters = new object[2];
                parameters[0] = allItems;
                parameters[1] = CheckState.Checked;
                multiSelectCombo.InvokeMethod("SelectUnselectItems", parameters);
            }
            catch (Exception ex)
            {
                Log.Error("Select multiple items from combo is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Get Column Index Mapping
        /// </summary>
        /// <param name="msaa"></param>
        /// <param name="gridTable"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetColumnIndexMaping(this MsaaObject msaa, DataTable gridTable)
        {
                Dictionary<string, int> columnToIndexMapping = new Dictionary<string, int>();
                try
                {
                    for (int index = 0; index < msaa.CachedChildren.Count; index++)
                    {
                        string temp = msaa.CachedChildren[index].Name.Trim();
                        if (gridTable.Columns.Contains(temp))
                        {
                            if (msaa.CachedChildren[index].Role == AccessibleRole.Cell && !columnToIndexMapping.ContainsKey(temp))
                            {
                                columnToIndexMapping.Add(temp, index);
                            }
                        }
                    }
                }
            catch (Exception ex)
                {
                    Log.Error("Get column index mapping failed :" + ex.Message);
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
            }
            return columnToIndexMapping;
        }
        /// <summary>
        /// Get Column Headers of Msaa Object
        /// </summary>
        /// <param name="msaaObject"></param>
        /// <returns></returns>
        public static MsaaObject GetColumnHeadersMsaa(this MsaaObject msaaObject)
        {
            MsaaObject columnHeaderObject = msaaObject;
            try
            {
                if (msaaObject.Name.Equals("Column Headers"))
                {
                    columnHeaderObject = msaaObject;
                }
                else
                {
                    for (int index = 0; index < msaaObject.ChildCount; index++)
                    {
                        columnHeaderObject = GetColumnHeadersMsaa(msaaObject.CachedChildren[index]);
                        if (columnHeaderObject.Name.Equals("Column Headers"))
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Get column header for msaa object failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return columnHeaderObject;
        }
        /// <summary>
        /// Trims the end.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="suffixToRemove">The suffix to remove.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType)
        {

            try
            {
                if (input != null && suffixToRemove != null
                      && input.EndsWith(suffixToRemove, comparisonType))
                {
                    return input.Substring(0, input.Length - suffixToRemove.Length);
                }
                else return input;
            }
            catch (Exception ex)
            {
                Log.Error("Trim from end failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }
        /// <summary>
        /// Clears the text.
        /// </summary>
        /// <param name="TextBoxElement">The text box element.</param>
        public static void ClearText(this UIObject TextBoxElement)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                TextBoxElement.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                while (TextBoxElement.Text.Length > 0 && tmr.ElapsedMilliseconds <= 15000)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                }
                tmr.Stop();
            }
            catch (Exception ex)
            {
                Log.Error("Clear texting failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Sends the key wait.
        /// </summary>
        /// <param name="input">The input.</param>
        public static void SendKeyWait(this string input)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
            }
            catch (Exception ex)
            {
                Log.Error("Get column header for msaa object failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }
        /// <summary>
        /// To check if the ui object is enabled or not and wait for it to get enabled
        /// Retry once more if it fails to enable within first timeout otherwise throw exception
        /// </summary>
        /// <param name="checkButton"></param>
        /// <param name="time">Time in seconds</param>
        public static void WaitForEnabled(ref UIWindow checkElement, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();
                bool retryFlag;
                if (!checkElement.IsEnabled)
                {
                    enableTimer.Start();
                    while (!checkElement.IsEnabled)
                    {
                        retryFlag = false;
                        if (enableTimer.ElapsedMilliseconds >= (time)*1000 && !retryFlag)
                        {
                            enableTimer.Reset();
                            enableTimer.Restart();
                            retryFlag = true;
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && retryFlag)
                        {
                            enableTimer.Stop();
                            //throw Exception here
                            //throw new Exception(MessageConstants.MSG_OBJ_NOT_FOUND);
                        }
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for enabled failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To check if the ui object is enabled or not and wait for it to get enabled
        /// Retry once more if it fails to enable within first timeout otherwise throw exception
        /// </summary>
        /// <param name="checkButton"></param>
        /// <param name="time">Time in seconds</param>
        public static void WaitForMenuItemEnabled(ref UIMenuItem checkElement, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();
                bool retryFlag;
                if (!checkElement.IsEnabled)
                {
                    enableTimer.Start();
                    while (!checkElement.IsEnabled)
                    {
                        retryFlag = false;
                        if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && !retryFlag)
                        {
                            enableTimer.Reset();
                            enableTimer.Restart();
                            retryFlag = true;
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && retryFlag)
                        {
                            enableTimer.Stop();
                            //throw Exception here
                            //throw new Exception(MessageConstants.MSG_OBJ_NOT_FOUND);
                        }
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for enabled failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To check if the ui element is enabled or not and wait for it to get enabled
        /// Retry once more if it fails to enable within first timeout otherwise throw exception
        /// </summary>
        /// <param name="checkButton"></param>
        /// <param name="time"></param>
        public static void WaitForUIElementEnable(ref UIAutomationElement checkElement, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();
                bool retryFlag;
                if (!checkElement.IsEnabled)
                {
                    enableTimer.Start();
                    while (!checkElement.IsEnabled)
                    {
                        retryFlag = false;
                        if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && !retryFlag)
                        {
                            enableTimer.Reset();
                            enableTimer.Restart();
                            retryFlag = true;
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && retryFlag)
                        {
                            enableTimer.Stop();
                            //throw Exception here
                            //throw new Exception(MessageConstants.MSG_OBJ_NOT_FOUND);
                        }
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for enabled failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To check if the ui element is visible or not and wait for it to get visible
        /// Retry once more if it fails to enable within first timeout otherwise throw exception
        /// </summary>
        /// <param name="checkElement"></param>
        /// <param name="time"></param>
        public static void WaitForVisibleUIElement(ref UIAutomationElement checkElement, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();
                bool retryFlag;
                if (!checkElement.IsVisible)
                {
                    enableTimer.Start();
                    Console.WriteLine("Total Timeout Time: " + (time) * 1000);
                    retryFlag = false;
                    while (!checkElement.IsVisible)
                    {
                        if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && !retryFlag)
                        {
                            enableTimer.Reset();
                            enableTimer.Restart();
                            retryFlag = true;
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && retryFlag)
                        {
                            enableTimer.Stop();
                            TimeSpan timeTaken = enableTimer.Elapsed;
                            string elapsedTime = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                            //throw Exception here
                            throw new Exception(MessageConstants.MSG_OBJ_NOT_FOUND + elapsedTime);
                        }
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for visible failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public static void WaitForVisibleUIApplication(ref UIApplication checkApplication, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();
                bool retryFlag;
                if (!checkApplication.IsVisible)
                {
                    enableTimer.Start();
                    Console.WriteLine("Total Timeout Time: " + (time) * 1000);
                    retryFlag = false;
                    while (!checkApplication.IsVisible)
                    {
                        if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && !retryFlag)
                        {
                            enableTimer.Reset();
                            enableTimer.Restart();
                            retryFlag = true;
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && retryFlag)
                        {
                            enableTimer.Stop();
                            TimeSpan timeTaken = enableTimer.Elapsed;
                            string elapsedTime = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                            //throw Exception here
                            throw new Exception(MessageConstants.MSG_OBJ_NOT_FOUND + elapsedTime);
                        }
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for UI Application Failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// To check if the ui object is visible or not and wait for it to get visible
        /// Retry once more if it fails to enable within first timeout otherwise throw exception
        /// </summary>
        /// <param name="checkUI"></param>
        /// <param name="time"></param>
        public static void WaitForVisible(ref UIWindow checkUI, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();
                bool retryFlag;
                if (!checkUI.IsVisible)
                {
                    enableTimer.Start();
                    Console.WriteLine("Total Timeout Time: " + (time) * 1000);
                    retryFlag = false;
                    while (!checkUI.IsVisible)
                    {
                        if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && !retryFlag)
                        {
                            enableTimer.Reset();
                            enableTimer.Restart();
                            retryFlag = true;
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && retryFlag)
                        {
                            enableTimer.Stop();
                            TimeSpan timeTaken = enableTimer.Elapsed;
                            string elapsedTime = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                            //throw Exception here
                            throw new Exception(MessageConstants.MSG_OBJ_NOT_FOUND +" "+ elapsedTime);
                        }
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for visible failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        ///  For wait to get enabled Done Away button
        /// </summary>
        /// <param name="checkButton"></param>
        /// <param name="time"></param>
        public static void WaitForEnabledForTTUseCase(ref UIWindow checkButton, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();

                if (!checkButton.IsEnabled)
                {
                    enableTimer.Start();
                    //bool retryFlag = false;
                    Console.WriteLine("Total Timeout Time: " + (time) * 1000);

                    while (checkButton.IsEnabled || enableTimer.ElapsedMilliseconds >= (time) * 1000)
                    {
                        Console.WriteLine(enableTimer.ElapsedMilliseconds);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for enabled failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }


        public static bool ClickObjectAndWaitForExpectedWindow(ref UIObject objectToClick, ref UIWindow futureExpectedWindow, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(10); 
            }
            bool isExpectedResultAchieved = false;

            try
            {
                objectToClick.Click();

                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime) < timeout)
                {
                    if (futureExpectedWindow.IsVisible)
                    {
                        isExpectedResultAchieved = true;
                        break;
                    }
                    Thread.Sleep(1000); 
                }

                if (!isExpectedResultAchieved)
                {
                    Console.WriteLine( "Timeout: "+futureExpectedWindow+"window did not appear within the specified time.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return isExpectedResultAchieved;
        }
        public static bool ClickWindowAndWaitForExpectedWindow(ref UIWindow objectToClick, ref UIWindow futureExpectedWindow, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(10);
            }
            bool isExpectedResultAchieved = false;

            try
            {
                objectToClick.Click();

                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime) < timeout)
                {
                    if (futureExpectedWindow.IsVisible)
                    {
                        isExpectedResultAchieved = true;
                        break;
                    }
                    Thread.Sleep(1000);
                }

                if (!isExpectedResultAchieved)
                {
                    Console.WriteLine("Timeout: " + futureExpectedWindow + "window did not appear within the specified time.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return isExpectedResultAchieved;
        }
       

        public static bool pickFromMenuItem(ref UIWindow PopupMenuContext, string itemToSelect,  UIWindow futureExpectedWindow = null)
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


        public static string GetActiveWindowTitle()
        {
            IntPtr hWnd = GetForegroundWindow();
            const int nChars = 256;
            StringBuilder title = new StringBuilder(nChars);

            if (GetWindowText(hWnd, title, nChars) > 0)
            {
                return title.ToString();
            }
            return null;
        }

        public static void SwitchToWindowTitle(string windowTitle)
        {
            IntPtr hWnd = FindWindow(null, windowTitle);

            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
            }
            else
            {
                Console.WriteLine("Window not found with title: " + windowTitle);
            }
        }

        public static void EnsureTextWindowLength(UIWindow windowtextBox, string text)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                while ((windowtextBox.Text.Length != text.Length) && tmr.ElapsedMilliseconds <= 15000)
                {
                    windowtextBox.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[HOME]");
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    Console.WriteLine(windowtextBox.Text);
                }
                tmr.Stop();
            }
            catch (Exception ex)
            {
                Log.Error("Clear texting failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public static void WaitForVisible(ref UIAutomationElement checkUI, int time)
        {
            try
            {
                Stopwatch enableTimer = new Stopwatch();
                bool retryFlag;
                if (!checkUI.IsVisible)
                {
                    enableTimer.Start();
                    Console.WriteLine("Total Timeout Time: " + (time) * 1000);
                    retryFlag = false;
                    while (!checkUI.IsVisible)
                    {
                        if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && !retryFlag)
                        {
                            enableTimer.Reset();
                            enableTimer.Restart();
                            retryFlag = true;
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else if (enableTimer.ElapsedMilliseconds >= (time) * 1000 && retryFlag)
                        {
                            enableTimer.Stop();
                            TimeSpan timeTaken = enableTimer.Elapsed;
                            string elapsedTime = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                            //throw Exception here
                            throw new Exception(MessageConstants.MSG_OBJ_NOT_FOUND + " " + elapsedTime);
                        }
                    }
                    enableTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wait for visible failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public static void SwitchToPartialWindowTitle(string partialWindowTitle)
        {
            try
            {
                IntPtr hWnd = IntPtr.Zero;

                // Enumerate all top-level windows to find a match with the partial title
                EnumWindows((IntPtr hwnd, IntPtr lParam) =>
                {
                    StringBuilder windowText = new StringBuilder(256);
                    GetWindowText(hwnd, windowText, windowText.Capacity);

                    if (windowText.ToString().Contains(partialWindowTitle))
                    {
                        hWnd = hwnd;
                        return false; // Stop enumerating
                    }

                    return true; // Continue enumerating
                }, IntPtr.Zero);

                if (hWnd != IntPtr.Zero)
                {
                    SetForegroundWindow(hWnd);
                    Console.WriteLine("Switched to Window containing title: " + partialWindowTitle);
                }
                else
                {
                    Console.WriteLine("Window not found with title containing: " + partialWindowTitle);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


        public static void CheckActiveUserBeforeAutomation()
        {
            string[] usernameToCheck = ConfigurationManager.AppSettings["-AutomationUser"].Split(',');
            string command = "quser";
            List<string> activeUser = new List<string>();
            string output = ExecuteCommand(command);
            foreach (string userToCheck in usernameToCheck)
            {
                if (IsUserActive(ref output, userToCheck, ref activeUser))
                {
                    Console.WriteLine(userToCheck + " is active.");
                    break;
                }
            }
            foreach (string userToCheck in usernameToCheck)
            {
                if (!IsUserActive(ref output, userToCheck, ref activeUser))
                {
                    bool systemRestarted = true;
                    Console.WriteLine(usernameToCheck + " is not active.");
                    Console.WriteLine("Active User Info: ");

                    foreach (string user in activeUser)
                    {
                        Console.WriteLine(user);
                    }

                    try
                    {

                        Console.WriteLine("Exiting Automation and Restarting System ");
                        ProcessStartInfo info = new ProcessStartInfo(@"E:\DistributedAutomation\RestartMachine.bat");
                        info.CreateNoWindow = true;
                        info.UseShellExecute = false;
                        Process.Start(info);
                        Thread.Sleep(6000);
                        //System.Environment.Exit(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error while Restarting Machine - Check E:\\DistributedAutomation\\RestartMachine.bat");
                        systemRestarted = false;
                    }
                    finally
                    {
                        if (!systemRestarted)
                        {
                            Console.WriteLine("Restarting System on Finally");
                            RestartSystem();
                            System.Environment.Exit(1);
                        }
                    }
                }
                else
                    break;
            }
        }
        static string ExecuteCommand(string command)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            StreamWriter sw = process.StandardInput;
            StreamReader sr = process.StandardOutput;

            sw.WriteLine(command + " 2>&1");
            sw.Close();

            string output = sr.ReadToEnd();
            process.WaitForExit();
            process.Close();

            return output;
        }

        static bool IsUserActive(ref string quserData, string username, ref List<string> activeUser)
        {
            string[] lines = quserData.Split('\n');
            bool ans = false;
            foreach (string line in lines)
            {
                if (line.Contains("Active"))
                {
                    activeUser.Add(line);

                }
                if ((line.Contains(username.ToLower()) || line.Contains(username.ToUpper()) || line.Contains(username)) && line.Contains("Active") && !line.Contains("Disc"))
                {
                    ans = true;
                }
            }

            return ans;
        }

       static void RestartSystem()
       {
           
            ProcessStartInfo startInfo = new ProcessStartInfo
           {
            FileName = "shutdown",
            Arguments = "/r /t 0", 
            CreateNoWindow = true,
            UseShellExecute = false
           };
           Process.Start(startInfo);
       }
 
        public static string VerifyAndControlSimulatorAction(string jarPath, string scriptFilePath, string action, string response)
        {
            string str = "";
            try
            {

                string batchFilePath = ConfigurationManager.AppSettings["-SimulatorActionsFilePath"].ToString() + "\\SimulatorActionsBatchScript.bat";

                string arguments = "\"" + jarPath + "\" \"" + action + "\" \"" + response + "\"";
                ExtentionMethods.SwitchToWindowTitle("MS");
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = batchFilePath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                int maxRetry = 3;
                int currentRetry = 0;


                do
                {
                    using (Process process = new Process { StartInfo = startInfo })
                    {
                        process.Start();

                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        str = output;
                        process.WaitForExit();


                        Console.WriteLine("Output: " + output);
                        Console.WriteLine("Error: " + error);


                        string resultFilePath = ConfigurationManager.AppSettings["-SimulatorActionsFilePath"].ToString() + "\\ScriptResult.txt";
                        process.WaitForExit();
                        if (File.Exists(resultFilePath))
                        {
                            string resultContent = File.ReadAllText(resultFilePath);


                            if (resultContent.Trim() == "0")
                            {
                                Console.WriteLine("ScriptResult run successfully.");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Script failed. Retry attempt " + (currentRetry + 1) / (maxRetry) + " .");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ScriptResult.txt not found.");
                        }
                    }

                    currentRetry++;

                } while (currentRetry < maxRetry);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while verifying SimulatorActions: " + ex.Message);
            }
            return str;
        }


    }
}
