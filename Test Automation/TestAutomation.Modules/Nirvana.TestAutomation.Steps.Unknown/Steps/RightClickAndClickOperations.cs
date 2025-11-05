using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using System.Configuration;
using System.Runtime.InteropServices;
using UIAutomationClient;


namespace Nirvana.TestAutomation.Steps.Unknown
{
    public class RightClickAndClickOperations : BlotterUIMap, IUIAutomationTestStep
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                MaximizeBlotter();
                string gridName = testData.Tables[0].Rows[0]["GridName"].ToString().ToLower();
                if (gridName.Contains("suborder"))
                {
                    Orders.Click(MouseButtons.Left);
                    if (DgBlotter1.IsVisible)
                    {
                        DgBlotter1.WaitForVisible();
                        DgBlotter1.Click(MouseButtons.Right);
                    }
                }
                else if (gridName.Contains("order"))
                {
                    Orders.Click(MouseButtons.Left);
                    if (DgBlotter.IsVisible)
                    {
                        DgBlotter.WaitForVisible();
                        DgBlotter.Click(MouseButtons.Right);
                    }
                }
                else if (gridName.Contains("working"))
                {
                    WorkingSubs.Click(MouseButtons.Left);
                    if (DgBlotter2.IsVisible)
                    {
                        DgBlotter2.WaitForVisible();
                        DgBlotter2.Click(MouseButtons.Right);
                    }
                }
                else if (gridName.Contains("summary"))
                {
                    Summary.Click(MouseButtons.Left);
                    if (DgBlotter3.IsVisible)
                    {
                        DgBlotter3.WaitForVisible();
                        DgBlotter3.Click(MouseButtons.Right);
                    }
                }
                else
                {
                    IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                    TreeScope.TreeScope_Children,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                    IUIAutomationElement gridElement = appWindow.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ColScrollRegion: 0, RowScrollRegion: 0"));
                    double left = gridElement.CurrentBoundingRectangle.left;
                    double top = gridElement.CurrentBoundingRectangle.top;
                    double right = gridElement.CurrentBoundingRectangle.right;
                    double bottom = gridElement.CurrentBoundingRectangle.bottom;

                    int centerX = (int)((left + right) / 2);
                    int centerY = (int)((top + bottom) / 2);

                    SetCursorPos(centerX, centerY);
                    Wait(1000);
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
                    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);

                }
                if (PopupMenuContext.IsVisible)
                {
                    PopupMenuContext.WaitForVisible();
                    Wait(1000);
                    pickFromMenuItem(PopupMenuContext, testData.Tables[0].Rows[0]["Action"].ToString());
                    if (testData.Tables[0].Rows[0]["Action"].ToString().Equals("Transfer to User"))
                    {
                        Wait(1000);
                        string user = testData.Tables[0].Rows[0]["TransfertoUserName"].ToString();
                        var msaa = PopupMenuTransfertoUser.MsaaObject;
                        msaa.Click(user);

                    }
                }

                if (testData.Tables[0].Columns.Contains("VerifyNotAvailableMenuItems")
                    && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["VerifyNotAvailableMenuItems"].ToString()))
                {
                    string itemToVerify = testData.Tables[0].Rows[0]["VerifyNotAvailableMenuItems"].ToString();
                    bool verificationSucceeded = DataUtilities.VerifyNotAvailableMenuItem(PopupMenuContext, itemToVerify);

                    if (!verificationSucceeded)
                    {
                        throw new Exception("Verification failed: The menu item " + itemToVerify + " is available, but it should not be.");
                    }
                }
                if (!testData.Tables[0].Columns.Contains("IgnorePopup"))
                {
                    if (NirvanaBlotter.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    if (Warning.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "RightClickAndClickOperations");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        protected void OpenBlotter()
        {
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_BLOTTER"]);
                ExtentionMethods.WaitForVisible(ref BlotterMain, 15);
                DgBlotter.BringToFront();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected bool pickFromMenuItem(UIWindow PopupMenuContext, string itemToSelect, UIWindow futureExpectedWindow = null)
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
    }
}
