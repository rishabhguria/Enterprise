using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using System.IO;
using TestAutomationFX.UI;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using Nirvana.TestAutomation.Interfaces.Enums;
using Nirvana.TestAutomation.UIAutomation;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UIAutomationClient;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    class CreateTradeWithAlgoBroker : TradingTicketUIMap, IUIAutomationTestStep
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                var dict = SamsaraHelperClass.GetDict(ApplicationArguments.IUIAutomationMappingTables["TradingTicket"]);
                OpenManualTradingTicket();
                for (int i = 0; i < testData.Tables[0].Rows.Count;  i++)
                {
                    if (TxtSymbol.IsEnabled)
                    {
                        TxtSymbol.Properties["Text"] = testData.Tables[0].Rows[0][TestDataConstants.COL_SYMBOL].ToString();
                        TxtSymbol.Click(MouseButtons.Left);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        if (TradingTicket2.IsVisible)
                        {
                            ButtonYes1.Click(MouseButtons.Left);
                        }
                    }
                    ExtentionMethods.WaitForEnabledForTTUseCase(ref BtnSend, 20);

                    if (!BtnSend.IsEnabled)
                    {
                        Console.WriteLine("Second try for enter symbol on TT");
                        KeyboardUtilities.CloseWindow(ref TradingTicket_UltraFormManager_Dock_Area_Top);
                        OpenManualTradingTicket();
                        string symbol = testData.Tables[0].Rows[0][TestDataConstants.COL_SYMBOL].ToString();
                        if (TxtSymbol.IsEnabled)
                        {
                            TxtSymbol.Properties["Text"] = testData.Tables[0].Rows[0][TestDataConstants.COL_SYMBOL].ToString();
                            TxtSymbol.Click(MouseButtons.Left);
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            Wait(4000);
                        }

                        ExtentionMethods.WaitForEnabledForTTUseCase(ref BtnSend, 20);

                        if (!BtnSend.IsEnabled)
                        {
                            throw new Exception("Symbol may not exist in SM." + symbol);
                        }
                    }
                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_ORDER_SIDE].ToString() != String.Empty && CmbOrderSide.IsEnabled)
                    {
                        CmbOrderSide.Click(MouseButtons.Left);
                        CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_ORDER_SIDE].ToString();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }

                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_QUANTITY].ToString() != String.Empty && NmrcQuantity.IsEnabled)
                    {
                        NmrcQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_QUANTITY].ToString();
                    }


                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_TIF].ToString() != String.Empty && CmbTIF.IsEnabled)
                    {
                        CmbTIF.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_TIF].ToString();
                    }

                    if (NmrcTargetQuantity.IsVisible)
                    {
                        if (testData.Tables[0].Rows[0][TestDataConstants.COL_TARGET_QUANTITY_IN].ToString() != String.Empty && NmrcTargetQuantity.IsEnabled)
                        {
                            NmrcTargetQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_TARGET_QUANTITY_IN].ToString();
                        }
                    }
                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_ORDER_TYPE].ToString() != String.Empty && CmbOrderType.IsEnabled)
                    {
                        CmbOrderType.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_ORDER_TYPE].ToString();
                    }

                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_BROKER].ToString() != String.Empty && CmbBroker.IsEnabled)
                    {
                        if (testData.Tables[0].Rows[0][TestDataConstants.COL_BROKER].ToString() == "BLANK")
                        {
                            while (CmbBroker.Text.Length > 0)
                            {

                                CmbBroker.DoubleClick(MouseButtons.Left);

                                Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                            }


                        }
                        else
                        {
                            CmbBroker.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_BROKER].ToString();
                        }
                    }

                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_STOP].ToString() != String.Empty && NmrcStop.IsEnabled)
                    {
                        NmrcStop.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_STOP].ToString();
                    }
                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_LIMIT].ToString() != String.Empty && NmrcLimit.IsEnabled)
                    {
                        NmrcLimit.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_LIMIT].ToString();
                    }
                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_PRICE].ToString() != String.Empty && NmrcPrice.IsEnabled)
                    {
                        // Wait(3000);
                        NmrcPrice.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0][TestDataConstants.COL_PRICE].ToString();
                    }
                    

                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_VENUE].ToString() != String.Empty && CmbVenue.IsEnabled)
                    {
                        if (testData.Tables[0].Rows[0][TestDataConstants.COL_VENUE].ToString() == "BLANK")
                        {
                            while (CmbVenue.Text.Length > 0)
                            {

                                CmbVenue.DoubleClick(MouseButtons.Left);

                                Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                            }


                        }
                        else
                        {
                            CmbVenue.Properties[TestDataConstants.TEXT_PROPERTY] = "Algo";
                        }
                    }
                    if (testData.Tables[0].Rows[0]["AlgoType"].ToString() != String.Empty && CmbbxStrategy.IsEnabled)
                    {
                        CmbbxStrategy.Click(MouseButtons.Left);
                        if (testData.Tables[0].Rows[0]["AlgoType"].ToString() != String.Empty && CmbbxStrategy.IsEnabled)
                        {
                            CmbbxStrategy.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[0].Rows[0]["AlgoType"].ToString();
                        }
                    }
                    Wait(2000);
                    DataTable ExcelData = testData.Tables[0].Copy();
                    try
                    {
                        List<String> columns = new List<string>();
                        string StepName = "CreateTradeWithAlgoBroker";
                        DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                        Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                        SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref ExcelData);
                    }
                    catch (Exception)
                    { }
                    IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                    TreeScope.TreeScope_Children,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                    IUIAutomationElement gridElement = appWindow.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "algoStrategyControl"));
                    if (gridElement == null)
                    {
                        throw new Exception("AlgoGrid not found!");
                    }

                    foreach (DataColumn col in ExcelData.Columns)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(ExcelData.Rows[i][col].ToString()))
                            {
                                if (col.ToString().Equals("Start Time") || col.ToString().Equals("End Time")) 
                                {
                                    var today = DateTime.Now;
                                    string[] start = ExcelData.Rows[0][col].ToString().Split('.');
                                    int hour;
                                    int min = 00;
                                    hour = Int32.Parse(start[0]) + today.Hour;

                                    if (start.Length > 1)
                                    {
                                        min = Int32.Parse(start[1]);
                                    }
                                    if (hour < 10)
                                    {
                                        ExcelData.Rows[0][col] = "0" + hour + ":" + min;
                                    }
                                    else
                                    {
                                        ExcelData.Rows[0][col] = hour + ":" + min;
                                    }
                                    
                                }
                                string IUIid = dict[col.ToString()].ToString();
                                Console.WriteLine("Finding element with id: " + IUIid);
                                IUIAutomationElement actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, IUIid));
                                if (actionItem != null)
                                {
                                    var rawChildren = actionItem.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                                    for (int j = 0; j < rawChildren.Length; j++)
                                    { 
                                        var child = rawChildren.GetElement(j);

                                        if (child.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
                                        {
                                            double left = child.CurrentBoundingRectangle.left;
                                            double top = child.CurrentBoundingRectangle.top;
                                            double right = child.CurrentBoundingRectangle.right;
                                            double bottom = child.CurrentBoundingRectangle.bottom;

                                            int centerX = (int)((left + right) / 2);
                                            int centerY = (int)((top + bottom) / 2);

                                            SetCursorPos(centerX, centerY);
                                            Wait(1000);
                                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                                            DataUtilities.clearTextData();
                                            Keyboard.SendKeys(ExcelData.Rows[i][col].ToString());
                                            Wait(1000);
                                            break;
                                        }
                                    }
                
                                }
                            }
                        }
                        catch (Exception ex) {
                            throw ex;
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
