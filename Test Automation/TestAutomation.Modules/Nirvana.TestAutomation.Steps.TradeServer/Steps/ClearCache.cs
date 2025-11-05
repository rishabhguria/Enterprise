using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.UIAutomation;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.Runtime.InteropServices;
using UIAutomationClient;


namespace Nirvana.TestAutomation.Steps.TradeServer
{
    class ClearCache : TradeServerUIMap, ITestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                PranaTradeServiceUIApplication.Start();
                if (PranaTradeServiceUIApplication.IsEnabled)
                {
                    Wait(5000);
                    PranaTradeServiceUIApplication.BringToFront();
                    IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                    TreeScope.TreeScope_Children,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TradeServiceUI"));
                    IUIAutomationElement gridElement = appWindow.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "menuStrip1"));

                    if (gridElement != null)
                    {
                        IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, "Refresh Cache");

                        IUIAutomationElement refreshElement = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            nameCondition);
                        GridDataProvider.click(refreshElement);
                        Wait(2000);
                        IUIAutomationCondition closingNameCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, "Closing");

                        IUIAutomationElement closingElement = refreshElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            closingNameCondition);
                        GridDataProvider.click(closingElement);
                        Warning1.BringToFront();
                        ButtonYes2.Click(MouseButtons.Left);
                        Wait(2000);
                        GridDataProvider.click(refreshElement);
                        Wait(2000);
                        IUIAutomationCondition PreferenceCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, "Allocation Preference");

                        IUIAutomationElement Preference = refreshElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            PreferenceCondition);
                        GridDataProvider.click(Preference);
                        Wait(2000);
                        Warning1.BringToFront();
                        ButtonYes2.Click(MouseButtons.Left);
                        if (closingElement != null)
                        {
                            Marshal.ReleaseComObject(closingElement);
                            closingElement = null;
                        }

                        if (refreshElement != null)
                        {
                            Marshal.ReleaseComObject(refreshElement);
                            refreshElement = null;
                        }
                        if (Preference != null)
                        {
                            Marshal.ReleaseComObject(Preference);
                            Preference = null;
                        }
                        if (gridElement != null)
                        {
                            Marshal.ReleaseComObject(gridElement);
                            gridElement = null;
                        }
                        if (appWindow != null)
                        {
                            Marshal.ReleaseComObject(appWindow);
                            appWindow = null;
                        }
                    }
                    else
                    {
                        if (RefreshCache.IsEnabled)
                        {
                            ExtentionMethods.WaitForMenuItemEnabled(ref RefreshCache, 20);
                            RefreshCache.Click(MouseButtons.Left);
                        }
                        if (Closing.IsEnabled)
                        {
                            ExtentionMethods.WaitForMenuItemEnabled(ref Closing, 10);
                            Closing.Click(MouseButtons.Left);
                            Wait(1000);
                            Warning1.BringToFront();
                            ButtonYes2.Click(MouseButtons.Left);
                        }
                        else
                        {
                            RefreshCache.Click(MouseButtons.Left);
                            Closing.Click(MouseButtons.Left);
                            Warning1.BringToFront();
                            ButtonYes2.Click(MouseButtons.Left);
                        }
                        Wait(2000);
                        if (RefreshCache.IsEnabled)
                        {
                            ExtentionMethods.WaitForMenuItemEnabled(ref RefreshCache, 20);
                            RefreshCache.Click(MouseButtons.Left);
                        }
                        if (AllocationPreference.IsEnabled)
                        {
                            ExtentionMethods.WaitForMenuItemEnabled(ref AllocationPreference, 10);
                            AllocationPreference.Click(MouseButtons.Left);
                            Wait(1000);
                            Warning1.BringToFront();
                            ButtonYes2.Click(MouseButtons.Left);

                        }
                        else
                        {
                            RefreshCache.Click(MouseButtons.Left);
                            AllocationPreference.Click(MouseButtons.Left);
                            Warning1.BringToFront();
                            ButtonYes2.Click(MouseButtons.Left);
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
            finally
            {
                try
                {
                    Process[] processes = Process.GetProcessesByName("Prana.TradeServiceUI");
                    foreach (Process process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error killing the 'Prana.TradeServiceUI' process: " + ex.Message);
                    throw;
                }
            }
            return _result;
        }
    }
}

