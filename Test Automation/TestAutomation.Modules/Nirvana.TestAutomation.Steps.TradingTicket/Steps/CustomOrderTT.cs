using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.BussinessObjects;
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
using System.Xml.Linq;
using System.Xml;
using System.Runtime.InteropServices;
using UIAutomationClient;



namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class CustomOrderTT : TradingTicketUIMap, ITestStep
    {
        private bool flag = false;
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenManualTradingTicket();
                string sheetName = sheetIndexToName[0];
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    _res.ErrorMessage = CustomClick(testData, sheetName, dr);
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CustomOrderTT");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                if (!flag)
                {
                    CloseTradingTicket();
                }

            }
            return _res;
        }
        private string CustomClick(DataSet testData, string sheetName, DataRow dr)
        {
            try
            {
                String TT_Control = string.Empty;
                String PopUp_Control = string.Empty;
                String OrderButton = string.Empty;
                String PopUp_Qty = string.Empty;
                bool db = true;
                bool isSuggestionsDone = false;
                db = dbcheck(testData, sheetName);
                if (db == true)
                {
                    string query = "SELECT * FROM T_SMSymbolLookUpTable WHERE TickerSymbol = " + "'" + dr[TestDataConstants.COL_SYMBOL].ToString() + "'";
                    DataTable data = SqlUtilities.GetDataFromQuery(query);
                    if (data == null)
                    {
                        return "Symbol does not exist in Database";
                    }
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_MASTERFUNDS))
                {
                    if (dr[TestDataConstants.COL_MASTERFUNDS].ToString() != String.Empty && CmbFunds.IsEnabled)
                    {
                        Wait(1000);
                        CmbFunds.Click(MouseButtons.Left);
                        CmbFunds.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_MASTERFUNDS].ToString();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                }

                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_SYMBOL].ToString()) && TxtSymbol.IsEnabled)
                {
                    TxtSymbol.DoubleClick(MouseButtons.Left);
                    if (dr.Table.Columns.Contains(TestDataConstants.COL_VERIFYSUGGESTIONS))
                    {
                        if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_VERIFYSUGGESTIONS].ToString()))
                        {
                            isSuggestionsDone = true;
                            if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_pick].ToString()))
                            {
                                Keyboard.SendKeys(dr[TestDataConstants.COL_SYMBOL].ToString());
                                ExtentionMethods.EnsureTextWindowLength(TxtSymbol, dr[TestDataConstants.COL_SYMBOL].ToString());
                                Wait(6000);
                                bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_SYMBOL].ToString(), ref TxtSymbol, ref TestDataConstants.TT, string.Equals(dr[TestDataConstants.COL_TypedSymbolPresent].ToString(), "YES", StringComparison.OrdinalIgnoreCase), !string.IsNullOrEmpty(dr[TestDataConstants.COL_TypedSymbolPresent].ToString()), dr[TestDataConstants.COL_pick].ToString());
                                if (isverificationsuceeded == false)
                                {
                                    throw new Exception("VerifySuggestionList failed");
                                }

                            }
                        }

                    }
                    if (!isSuggestionsDone)
                    {
                        ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_SYMBOL].ToString(), KeyboardConstants.ENTERKEY, true);
                    }

                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }


                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_ORDERBUTTON))
                {
                    OrderButton = dr[TestDataConstants.COL_ORDERBUTTON].ToString();
                    OrderButton = OrderButton.ToUpper();
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.Col_Strike_Price)) 
                {
                    IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                        TreeScope.TreeScope_Children,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                    if (dr["Option"].ToString().Equals("ToggleState_On"))
                    {
                        Wait(1500);
                        IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "tblPnlSymbolControl"));
                        IUIAutomationElement option = gridElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "chkBoxOption"));
                        object valuePatternObj = option.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                        if (valuePatternObj != null)
                        {
                            IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                            string value = valuePattern.CurrentValue;
                            if (value.ToUpper().Equals("FALSE")) {
                                click(option);                                
                            }
                            Wait(1000);
                        }
                        if(!string.IsNullOrEmpty(dr["Underlying Symbol"].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "txtUnderlyingSymbol"));
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(dr["Underlying Symbol"].ToString());
                            Wait(1000);

                        }

                        if (!string.IsNullOrEmpty(dr[TestDataConstants.Col_Strike_Price].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "txtStrikePrice"));
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(dr[TestDataConstants.Col_Strike_Price].ToString());
                            Wait(1000);
                        }

                        if (!string.IsNullOrEmpty(dr["Option Type"].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbOptionType"));
                            click(option);
                            Keyboard.SendKeys(dr["Option Type"].ToString());
                            Wait(1000);
                        }

                        if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_Expiration_Date].ToString()))
                        {
                            string tempDate = DataUtilities.DateHandler(dr[TestDataConstants.COL_Expiration_Date].ToString());
                            string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtExpirationDate"));
                            Wait(1000);
                            valuePatternObj = option.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                            if (valuePatternObj != null)
                            {
                                IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                                string value = valuePattern.CurrentValue;
                                Console.WriteLine(value);
                            }
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(date);
                            Wait(1000);
                        }
                        option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "txtStrikePrice"));
                        click(option);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    else if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_INTEREST_RATE].ToString()) || !string.IsNullOrEmpty(dr[TestDataConstants.COL_SPREAD].ToString()) || !string.IsNullOrEmpty(dr["Original Cost Basis"].ToString()) || !string.IsNullOrEmpty(dr["Notional Value"].ToString()) || !string.IsNullOrEmpty(dr["Book as Swap"].ToString())) // Reverting change as used for ACC cases
                    {
                        IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "tblPnlSymbolControl"));
                        IUIAutomationElement option = gridElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "chkBoxSwap"));
                        object valuePatternObj = option.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                        if (valuePatternObj != null)
                        {
                            IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                            string value = valuePattern.CurrentValue;
                            if (value.ToUpper().Equals("FALSE"))
                            {
                                click(option);
                            }
                        }
                        Swap.Click(MouseButtons.Left);
                        gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "grpBxSwap"));
                        if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_INTEREST_RATE].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "spnrBenchMarkRate"));
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(dr[TestDataConstants.COL_INTEREST_RATE].ToString());
                        }

                        if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_SPREAD].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "spnrDifferential"));
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(dr[TestDataConstants.COL_SPREAD].ToString());
                        }

                        if (!string.IsNullOrEmpty(dr["Notional Value"].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "spnrNotional"));
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(dr["Notional Value"].ToString());
                        }

                        if (!string.IsNullOrEmpty(dr["Original Cost Basis"].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "spnrCostBasis"));
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(dr["Original Cost Basis"].ToString());
                        }

                        if (!string.IsNullOrEmpty(dr["Day Count"].ToString()))
                        {
                            option = gridElement.FindFirst(
                            TreeScope.TreeScope_Descendants,
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "spnrDayCount"));
                            click(option);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(dr["Day Count"].ToString());
                        }
                    }
                
                }
                if (testData.Tables[0].Columns.Contains("BookAsSwap") && !string.IsNullOrEmpty(dr["BookAsSwap"].ToString()) && dr["BookAsSwap"].ToString().Equals("Yes")) 
                {
                    Wait(2000);
                    IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                       TreeScope.TreeScope_Children,
                       automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                    
                    IUIAutomationElement gridElement = appWindow.FindFirst(
                           TreeScope.TreeScope_Descendants,
                           automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "tblPnlSymbolControl"));
                    IUIAutomationElement option = gridElement.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "chkBoxSwap"));
                    object valuePatternObj = option.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    if (valuePatternObj != null)
                    {
                        IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                        string value = valuePattern.CurrentValue;
                        if (value.ToUpper().Equals("FALSE"))
                        {
                            click(option);
                        }
                    }
                }
                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_ORDER_SIDE))
                {
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_ORDER_SIDE].ToString()))
                    {
                        try
                        {
                            CmbOrderSide.WaitForEnabled();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception occurred while waiting for CmbOrderSide to be enabled: " + ex.Message);
                            Console.WriteLine("Stack Trace: " + ex.StackTrace);
                        }
                        CmbOrderSide.DoubleClick(MouseButtons.Left);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        if (TradingTicket2.IsVisible)
                        {
                            ButtonYes1.Click(MouseButtons.Left);
                            CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY] = string.Empty;
                            Keyboard.SendKeys(dr[TestDataConstants.COL_ORDER_SIDE].ToString());
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                        else
                        {
                            CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_SIDE].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }

                    }
                }
                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_QUANTITY))
                {
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_QUANTITY].ToString()))
                    {
                        NmrcQuantity.WaitForEnabled();
                        NmrcQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_QUANTITY].ToString();
                    }
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_PRICE))
                {
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_PRICE].ToString()))
                    {
                        NmrcPrice.Click(MouseButtons.Left);
                        NmrcPrice.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_PRICE].ToString();
                        Console.WriteLine(NmrcPrice.Properties[TestDataConstants.TEXT_PROPERTY].ToString());
                    }
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_Fx_Rate))
                {
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_Fx_Rate].ToString()) && UpDownEdit4.IsEnabled)
                    {
                        UpDownEdit4.Click(MouseButtons.Left);
                        UpDownEdit4.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_Fx_Rate].ToString();
                    }
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_Fx_Opertor))
                {
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_Fx_Opertor].ToString()) && CmbFxOperator.IsEnabled)
                    {
                        CmbFxOperator.Click(MouseButtons.Left);
                        CmbFxOperator.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_Fx_Opertor].ToString();
                    }
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_ALLOCATION_METHOD))
                {
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_ALLOCATION_METHOD].ToString()))
                    {
                        CmbAllocation.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ALLOCATION_METHOD].ToString();
                    }
                }
                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_CUSTOM_ALLOCATION_ACCOUNT_VALUE))
                {
                    String[] Accounts = dr[TestDataConstants.COL_CUSTOM_ALLOCATION_ACCOUNT_VALUE].ToString().Split(',');
                    String[] AllocationPercentage = dr[TestDataConstants.COL_CUSTOM_ALLOCATION_ACCOUNT_PERCENT_VALUE].ToString().Split(',');
                    if (Accounts.Length == 1)
                    {
                        if (!CmbAllocation.Text.Equals(dr[TestDataConstants.COL_CUSTOM_ALLOCATION_ACCOUNT_VALUE].ToString(), StringComparison.InvariantCultureIgnoreCase) && dr[TestDataConstants.COL_CUSTOM_ALLOCATION_ACCOUNT_VALUE].ToString() != string.Empty) // change cmbobox only if value needs to be changed
                        {
                            //Timer added to prevent the test case to get struck in loop
                            //Changes required as test cases failed when struck in this loop
                            Stopwatch timer = new Stopwatch();
                            timer.Start();
                            while (!(CmbAllocation.Text.Equals(dr[TestDataConstants.COL_CUSTOM_ALLOCATION_ACCOUNT_VALUE].ToString()) || timer.ElapsedMilliseconds >= 50000))
                            {
                                CmbAllocation.Click(MouseButtons.Left);
                                Keyboard.SendKeys("[END][SHIFT+HOME]");
                                Keyboard.SendKeys(dr[TestDataConstants.COL_CUSTOM_ALLOCATION_ACCOUNT_VALUE].ToString());
                            }
                            timer.Stop();
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                    }
                    else
                    {
                        BtnAccountQty.Click(MouseButtons.Left);
                        Wait(1000);

                        int rowCount = 0, msaaId = 0;
                        const int colCount = 4;


                        foreach (String Account in Accounts)
                        {
                            int isNewRow = 0;
                            if (rowCount == 1)
                                msaaId = 1;

                            var gridMssaObject = GrdAccounts.MsaaObject;
                            for (int i = 1; i < gridMssaObject.CachedChildren[msaaId].ChildCount - 1; i++)
                            {

                                if (gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Value != null && gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Value.Equals(Account))
                                {
                                    gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Click(MouseButtons.Left);

                                    for (int j = 1; j <= colCount; j++)
                                        Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                    Keyboard.SendKeys(AllocationPercentage[rowCount]);
                                    rowCount++;
                                    isNewRow++;
                                    break;
                                }
                            }
                            if (isNewRow == 0)
                            {
                                for (int k = 1; k < gridMssaObject.CachedChildren[msaaId].ChildCount - 1; k++)
                                {
                                    if (gridMssaObject.CachedChildren[msaaId].CachedChildren[k].Name.Equals("Template Add Row"))
                                    {
                                        gridMssaObject.CachedChildren[msaaId].CachedChildren[k].CachedChildren[0].Click(MouseButtons.Left);
                                        Keyboard.SendKeys(Account);

                                        for (int j = 1; j <= colCount; j++)
                                            Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                        Keyboard.SendKeys(AllocationPercentage[rowCount]);
                                        rowCount++;
                                        break;
                                    }
                                }
                            }
                        }
                        BtnOK.Click(MouseButtons.Left);
                    }

                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_ACCOUNT))
                {
                    String[] Accounts = dr[TestDataConstants.COL_ACCOUNT].ToString().Split(',');
                    String[] AllocationPercentage = dr[TestDataConstants.COL_ACCOUNT_PERCENTAGE_VALUE].ToString().Split(',');
                    if (Accounts.Length == 1)
                    {
                        if (!CmbAllocation.Text.Equals(dr[TestDataConstants.COL_ACCOUNT].ToString(), StringComparison.InvariantCultureIgnoreCase) && dr[TestDataConstants.COL_ACCOUNT].ToString() != string.Empty) // change cmbobox only if value needs to be changed
                        {
                            //Timer added to prevent the test case to get struck in loop
                            //Changes required as test cases failed when struck in this loop
                            Stopwatch timer = new Stopwatch();
                            timer.Start();
                            while (!(CmbAllocation.Text.Equals(dr[TestDataConstants.COL_ACCOUNT].ToString()) || timer.ElapsedMilliseconds >= 50000))
                            {
                                CmbAllocation.Click(MouseButtons.Left);
                                Keyboard.SendKeys("[END][SHIFT+HOME]");
                                Keyboard.SendKeys(dr[TestDataConstants.COL_ACCOUNT].ToString());
                            }
                            timer.Stop();
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                    }
                    else
                    {
                        BtnAccountQty.Click(MouseButtons.Left);
                        Wait(1000);

                        int rowCount = 0, msaaId = 0;
                        const int colCount = 4;


                        foreach (String Account in Accounts)
                        {
                            int isNewRow = 0;
                            if (rowCount == 1)
                                msaaId = 1;

                            var gridMssaObject = GrdAccounts.MsaaObject;
                            for (int i = 1; i < gridMssaObject.CachedChildren[msaaId].ChildCount - 1; i++)
                            {

                                if (gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Value != null && gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Value.Equals(Account))
                                {
                                    gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Click(MouseButtons.Left);

                                    for (int j = 1; j <= colCount; j++)
                                        Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                    Keyboard.SendKeys(AllocationPercentage[rowCount]);
                                    rowCount++;
                                    isNewRow++;
                                    break;
                                }
                            }
                            if (isNewRow == 0)
                            {
                                for (int k = 1; k < gridMssaObject.CachedChildren[msaaId].ChildCount - 1; k++)
                                {
                                    if (gridMssaObject.CachedChildren[msaaId].CachedChildren[k].Name.Equals("Template Add Row"))
                                    {
                                        gridMssaObject.CachedChildren[msaaId].CachedChildren[k].CachedChildren[0].Click(MouseButtons.Left);
                                        Keyboard.SendKeys(Account);

                                        for (int j = 1; j <= colCount; j++)
                                            Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                        Keyboard.SendKeys(AllocationPercentage[rowCount]);
                                        rowCount++;
                                        break;
                                    }
                                }
                            }
                        }
                        BtnOK.Click(MouseButtons.Left);
                    }

                }


                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_TIF))
                {
                    if (dr[TestDataConstants.COL_TIF].ToString() != String.Empty && CmbTIF.IsEnabled)
                    {
                        CmbTIF.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TIF].ToString();
                        if (dr[TestDataConstants.COL_TIF].ToString().Equals("Good Till Date"))
                        {
                            if (BtnExpireTime.IsVisible)
                            {
                                BtnExpireTime.Click(MouseButtons.Left);
                                Wait(1500);
                                try
                                {
                                    if (dr[TestDataConstants.COL_Expiration_Date].ToString().ToUpper().Contains("TODAY"))
                                    {
                                        string tempDate = DataUtilities.DateHandler(dr[TestDataConstants.COL_Expiration_Date].ToString());
                                        string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                                        dr[TestDataConstants.COL_Expiration_Date] = date;
                                    }
                                    GTD(dr);
                                }
                                catch(Exception ex) {
                                    BtnExpireTime.Click(MouseButtons.Left);
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }

                    if (dr[TestDataConstants.COL_ORDER_TYPE].ToString() != String.Empty && CmbOrderType.IsEnabled)
                    {
                        CmbOrderType.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_TYPE].ToString();
                    }

                    if (dr[TestDataConstants.COL_STOP].ToString() != String.Empty && NmrcStop.IsEnabled)
                    {
                        NmrcStop.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_STOP].ToString();
                    }

                    if (dr[TestDataConstants.COL_LIMIT].ToString() != String.Empty && NmrcLimit.IsEnabled)
                    {
                        NmrcLimit.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_LIMIT].ToString();
                    }

                }

                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_TRADE_DATE))
                {
                    if (!string.IsNullOrWhiteSpace(dr[TestDataConstants.COL_TRADE_DATE].ToString()) && DtTradeDate.IsEnabled)
                    {
                        try
                        {
                            string tempDate = DataUtilities.DateHandler(dr[TestDataConstants.COL_TRADE_DATE].ToString());
                            string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                            DtTradeDate.Click(MouseButtons.Left);
                            DtTradeDate.Properties[TestDataConstants.TEXT_PROPERTY] = date;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_BROKER))
                {
                    if (testData.Tables[0].Columns.Contains(TestDataConstants.PadLock_Visiblity))
                    {
                        if (!string.IsNullOrEmpty(dr[TestDataConstants.PadLock_Visiblity].ToString())) {
                            if (!BtnPadlock.IsValid.ToString().ToUpper().Equals(dr[TestDataConstants.PadLock_Visiblity].ToString().ToUpper()))
                            {
                                throw new Exception("Padlock visiblity is " + BtnPadlock.IsValid + " but excel is showing " + dr[TestDataConstants.PadLock_Visiblity].ToString());
                            }
                        }

                        if (!string.IsNullOrEmpty(dr[TestDataConstants.Padlock_Unlock].ToString())) {
                            if (dr[TestDataConstants.Padlock_Unlock].ToString().ToUpper().Equals("TRUE")) {
                                BtnPadlock.Click(MouseButtons.Left);
                                if (!CmbBroker.IsEnabled) {
                                    BtnPadlock.Click(MouseButtons.Left);
                                }
                            }
                        }
                    }


                    if (dr[TestDataConstants.COL_BROKER].ToString() != String.Empty && CmbBroker.IsEnabled)
                    {
                        CmbBroker.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_BROKER].ToString();
                    }
                    
                }

                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_VENUE))
                {
                    if (dr[TestDataConstants.COL_VENUE].ToString() != String.Empty && CmbVenue.IsEnabled)
                    {
                        if (dr[TestDataConstants.COL_VENUE].ToString() == "BLANK")
                        {
                            while (CmbVenue.Text.Length > 0)
                            {

                                CmbVenue.DoubleClick(MouseButtons.Left);

                                Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                            }


                        }
                        else
                        {
                            CmbVenue.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_VENUE].ToString();
                        }
                    }
                }
                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_TARGET_QUANTITY_IN))
                {
                    if (NmrcTargetQuantity.IsVisible)
                    {
                        if (dr[TestDataConstants.COL_TARGET_QUANTITY_IN].ToString() != String.Empty && NmrcTargetQuantity.IsEnabled)
                        {
                            NmrcTargetQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TARGET_QUANTITY_IN].ToString();
                        }
                    }
                }

                Commision.Click(MouseButtons.Left);
                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_COMMISSION_BASIS))
                {
                    if (dr[TestDataConstants.COL_COMMISSION_BASIS].ToString() != String.Empty && CmbCommissionBasis.IsEnabled)
                    {
                        CmbCommissionBasis.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_COMMISSION_BASIS].ToString();
                    }
                    if (dr[TestDataConstants.COL_COMMISSION_RATE].ToString() != String.Empty && NmrcCommissionRate.IsEnabled)
                    {
                        NmrcCommissionRate.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_COMMISSION_RATE].ToString();
                    }
                    if (dr[TestDataConstants.COL_SOFT_BASIS].ToString() != String.Empty && CmbSoftCommissionBasis.IsEnabled)
                    {
                        CmbSoftCommissionBasis.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SOFT_BASIS].ToString();
                    }
                    if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_SOFT_RATE))
                    {
                        if (dr[TestDataConstants.COL_SOFT_RATE].ToString() != String.Empty && NmrcSoftRate.IsEnabled)
                        {
                            NmrcSoftRate.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SOFT_RATE].ToString();
                        }
                    }

                }



                if (!string.IsNullOrEmpty(OrderButton.ToUpper()))
                {

                    if (OrderButton == "SEND")
                    {
                        if (string.IsNullOrEmpty(dr[TestDataConstants.COL_ORDER_SIDE].ToString()))
                        {
                            dr[TestDataConstants.COL_ORDER_SIDE] = CmbOrderSide.Text.ToString();
                            testData.AcceptChanges();
                        }
                        BtnSend.DoubleClick(MouseButtons.Left);
                        if (dr.Table.Columns.Contains("FixDisconnectionPopup") && !string.IsNullOrEmpty(dr["FixDisconnectionPopup"].ToString())) {
                            if (uiWindow1.IsVisible)
                                ButtonOK2.Click(MouseButtons.Left);
                        }
                        SaveData(testData, dr);
                    }
                    if (OrderButton == "DONEAWAY" || OrderButton == "DONE AWAY")
                    {
                        BtnDoneAway.DoubleClick(MouseButtons.Left);
                        if (Warning.IsVisible)
                        {
                            ButtonOK3.Click(MouseButtons.Left);
                        }
                       


                    }
                    if (OrderButton == "CREATEORDER" || OrderButton == "CREATE ORDER")
                    {
                        BtnCreateOrder.DoubleClick(MouseButtons.Left);
                    }

                    if (OrderButton == "REPLACE")
                    {
                        BtnReplace.DoubleClick(MouseButtons.Left);
                    }
                    if (OrderButton == "REPLACELIVE") // handling in case the user wants  to perform sim actions on the replaced trade
                    {
                        BtnReplace.DoubleClick(MouseButtons.Left);
                        SaveData(testData, dr);
                    }
                    if (string.Equals(OrderButton,"View Allocation",StringComparison.OrdinalIgnoreCase))
                    {
                        BtnViewAllocationDetails.DoubleClick(MouseButtons.Left);
                    }

                }
                if (TradingTicket2.IsVisible)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }

                if (uiWindow1.IsVisible)
                {
                    ButtonOK2.Click(MouseButtons.Left);
                }

                if (MessageWithGridPopup.IsVisible) 
                {
                    UltraButtonYes.Click(MouseButtons.Left);
                }
				
                if (!ApplicationArguments.SkipCompliance)
                {
                    if (ComplianceAlertPopUp.IsVisible)
                    {
                        if (!testData.Tables[0].Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                        {
                            // Wrong Button calling - code updated by yash
                            if (ResponseButton1.IsVisible || ResponseButton1.IsEnabled ) //Preventing random failures by adding is enabled
                            {
                                ResponseButton1.DoubleClick(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper().Equals("YES"))
                            {
                                if (ResponseButton1.IsVisible || ResponseButton1.IsEnabled )
                                {
                                    ResponseButton1.DoubleClick(MouseButtons.Left);
                                }
                            }
                            else if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper().Equals("NO"))
                            {
                                if (CancelButton2.IsVisible || CancelButton2.IsEnabled)
                                {
                                    CancelButton2.DoubleClick(MouseButtons.Left);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No action performed on compliance popup");
                            }

                        }
                    }
                    else if (ComplianceAlertPopupUC2.IsVisible || ComplianceAlertPopupUC2.IsEnabled)
                    {
                        if (!testData.Tables[0].Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                        {
                            /*Changing ResponseButton3 to ResponseButton as ResponseButton3 is not a part of ComplianceAlertPopupUC2 
                             Modified by Yash Gupta
                             https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/60621
                             */
                            if (ResponseButton.IsVisible || ResponseButton.IsEnabled) 
                            {
                                ResponseButton.DoubleClick(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper().Equals("YES"))
                            {
                                if (ResponseButton.IsVisible || ResponseButton.IsEnabled) 
                                {
                                    ResponseButton.DoubleClick(MouseButtons.Left);
                                }
                            }
                            else if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper().Equals("NO"))
                            {
                                if (CancelButton1.IsVisible || CancelButton1.IsEnabled)
                                {
                                    CancelButton1.Click(MouseButtons.Left);
                                }
                                else
                                {
                                    CancelButton.Click(MouseButtons.Left);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No action performed on compliance popup");
                            }

                        }
                    }
                    else if (ComplianceAlertPopupUC1.IsVisible || ComplianceAlertPopupUC1.IsEnabled)
                    {
                        if (!testData.Tables[0].Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                        {
                            if (ResponseButton3.IsVisible || ResponseButton3.IsEnabled)
                            {
                                ResponseButton3.DoubleClick(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper().Equals("YES"))
                            {
                                if (ResponseButton3.IsVisible || ResponseButton3.IsEnabled)
                                {
                                    ResponseButton3.Click(MouseButtons.Left);
                                }
                            }
                            else if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper().Equals("NO"))
                            {
                                if (CancelButton3.IsVisible || CancelButton3.IsEnabled)
                                {
                                    CancelButton3.Click(MouseButtons.Left);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No action performed on compliance popup");
                            }

                        }
                    }
                }
                else 
                    if (ComplianceAlertPopUp.IsVisible)
                    {
                        //handling for cases which runs on Compliance release with "None" dependancy
                        if (ResponseButton1.IsVisible || ResponseButton1.IsEnabled) //Preventing random failures by adding is enabled
                        {
                            ResponseButton1.DoubleClick(MouseButtons.Left);
                        }
                    }

                if (NirvanaCompliance.IsEnabled || NirvanaCompliance.IsVisible)
                {
                    NirvanaCompliance.BringToFront(); //bringing this popup to from to prevent random failures ,modified by Yash
                }
                if (dr.Table.Columns.Contains(TestDataConstants.Col_PendingApprovalPopUp) && !String.IsNullOrWhiteSpace(dr[TestDataConstants.Col_PendingApprovalPopUp].ToString()))
                {
                    if (NirvanaCompliance.IsVisible && dr[TestDataConstants.Col_PendingApprovalPopUp].ToString().Equals("Yes"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    else if (NirvanaCompliance.IsVisible && dr[TestDataConstants.Col_PendingApprovalPopUp].ToString().Equals("No"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }
                else{

                    if (NirvanaCompliance.IsVisible)
                    {
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }

                if (testData.Tables[0].Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp_Qty))
                {
                    PopUp_Qty = dr[TestDataConstants.Col_Shares_Outstanding_PopUp_Qty].ToString();
                }


                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_TT_Control))
                {
                    TT_Control = testData.Tables[0].Rows[0][TestDataConstants.COL_TT_Control].ToString();
                }
                else {
                    flag = true;
                }


                if (testData.Tables[0].Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp))
                {
                    PopUp_Control = dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString();
                }


                if (!String.IsNullOrEmpty(PopUp_Control) && PromptWindow_Fill_Panel.IsVisible)
                {
                    if (!String.IsNullOrEmpty(PopUp_Qty))
                    {
                        NmrcTargetQuantity1.Properties[TestDataConstants.TEXT_PROPERTY] = PopUp_Qty;
                        Wait(2000);
                    }

                    if (PopUp_Control.ToUpper() == "TRUE")
                    {
                        BtnPlace.Click(MouseButtons.Left);

                    }
                    else if (PopUp_Control.ToUpper() == "FALSE")
                    {
                        BtnEdit.Click(MouseButtons.Left);

                    }
                    else
                    {
                        throw new Exception("You can only Enter 'TRUE' or 'FALSE' in SharesOutstandingPopUp Column");
                    }
                }

                if (!String.IsNullOrEmpty(PopUp_Control))
                {

                    if (PopUp_Control.ToUpper() == "TRUE")
                    {
                        Wait(500);
                        if (TradingRulesViolatedPopUp.IsVisible)
                        {
                            UltraButtonYes.Click(MouseButtons.Left);
                        }

                    }
                    else if (PopUp_Control.ToUpper() == "FALSE")
                    {
                        if (TradingRulesViolatedPopUp.IsVisible)
                        {
                            UltraButtonNo.Click(MouseButtons.Left);
                        }

                    }
                    else
                    {
                        throw new Exception("You can only Enter 'TRUE' or 'FALSE' in SharesOutstandingPopUp Column");
                    }
                }



                if (!String.IsNullOrEmpty(TT_Control))
                {
                    if (TT_Control.ToUpper() == "TRUE")
                    {
                        flag = true;
                    }
                    else if (TT_Control.ToUpper() == "FALSE")
                    {
                        flag = false;
                    }
                    else
                    {
                        throw new Exception("You can only Enter 'TRUE' or 'FALSE' in TT_Open Column");
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            //
            return null;
        }

        protected void GTD(DataRow dr)
        {

            string str = dr[TestDataConstants.COL_Expiration_Date].ToString();
            string[] dates = str.Split('/');
            var nextDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[0]), int.Parse(dates[1]));
            var today = DateTime.Now;
            var diffOfDates = nextDate.Day - today.Day;
            Wait(2000);
            for (int i = 0; i < diffOfDates; i++)
            {
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
            }
            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            return;
        }


        private bool dbcheck(DataSet testData, string sheetName)
        {
            if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_DB_Check))
            {
                if (testData.Tables[sheetName].Rows[0][TestDataConstants.COL_DB_Check].ToString().ToUpper() == "TRUE")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void click(IUIAutomationElement option)
        {
            double left = option.CurrentBoundingRectangle.left;
            double top = option.CurrentBoundingRectangle.top;
            double right = option.CurrentBoundingRectangle.right;
            double bottom = option.CurrentBoundingRectangle.bottom;

            int centerX = (int)((left + right) / 2);
            int centerY = (int)((top + bottom) / 2);

            SetCursorPos(centerX, centerY);
            Wait(1000);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
            Wait(1000);
        }

        public void SaveData(DataSet LiveData, DataRow row)
        {
            try
            {

                LiveData.Tables[0].TableName = "CustomOrderTT";
                DataTable singleRowTable = LiveData.Tables[0].Clone();
                singleRowTable.ImportRow(row);

                DataSet singleRowDataSet = new DataSet();
                singleRowDataSet.Tables.Add(singleRowTable);

                if (!Directory.Exists("SimulatorData"))
                    Directory.CreateDirectory("SimulatorData");
                if (File.Exists(@"SimulatorData/LiveTrades.xml"))
                {
                    try
                    {
                        XmlDocument xml1 = new XmlDocument();
                        XmlDocument xml2 = new XmlDocument();
                        xml1.Load(@"SimulatorData/LiveTrades.xml");

                        if (xml1.SelectSingleNode("NewDataSet/CustomOrderTT") != null || xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateReplaceLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/SendOrderUsingMTT") != null)
                        {
                            xml2.LoadXml(singleRowDataSet.GetXml());
                            foreach (XmlNode list in xml2.SelectSingleNode("NewDataSet").ChildNodes)
                                xml1.SelectSingleNode("NewDataSet").AppendChild(xml1.ImportNode(list, true));
                            xml1.Save(@"SimulatorData/LiveTrades.xml");
                        }
                        else
                        {
                            singleRowDataSet.WriteXml(@"SimulatorData/LiveTrades.xml");
                        }
                    }
                    catch { singleRowDataSet.WriteXml(@"SimulatorData/LiveTrades.xml"); }
                }
                else
                    singleRowDataSet.WriteXml(@"SimulatorData/LiveTrades.xml");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}

