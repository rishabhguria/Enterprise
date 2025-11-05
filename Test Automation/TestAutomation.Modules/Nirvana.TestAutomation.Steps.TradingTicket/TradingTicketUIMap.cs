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


namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    [UITestFixture]
    public partial class TradingTicketUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TradingTicketUIMap"/> class.
        /// </summary>
        public TradingTicketUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TradingTicketUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public TradingTicketUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private void NumericInputHandle(TestAutomationFX.UI.UIWindow numeric, string input)
        {
            try
            {
                numeric.Click(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(input, KeyboardConstants.ENTERKEY, true);
                Wait(1000);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                //Wait(2000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;

        private void ComboBoxInputHandle(TestAutomationFX.UI.UIWindow comboBox, string input)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(input))
                    if (!comboBox.Text.Equals(input, StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                    {
                        comboBox.Click(MouseButtons.Left);
                        ClearText(comboBox);
                        Keyboard.SendKeys(input);
                    }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public void InputDetails(DataRow dr, UIWindow btn)
        {
            try
            {
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

                if (TxtSymbol.IsEnabled)
                {
                    //NumericInputHandle(TxtSymbol, dr[TestDataConstants.COL_SYMBOL].ToString());
                    TxtSymbol.Properties["Text"] = dr[TestDataConstants.COL_SYMBOL].ToString();
                    TxtSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    if (TradingTicket2.IsVisible)
                    {
                        ButtonYes1.Click(MouseButtons.Left);
                    }
                    Wait(4000);
                }

                ExtentionMethods.WaitForEnabledForTTUseCase(ref btn, 20);

                if (!btn.IsEnabled)
                {
                    Console.WriteLine("Second try for enter symbol on TT");
                    //close TT code
                    KeyboardUtilities.CloseWindow(ref TradingTicket_UltraFormManager_Dock_Area_Top);
                    //open ka code
                    OpenManualTradingTicket();
                    string symbol = dr[TestDataConstants.COL_SYMBOL].ToString();
                    if (TxtSymbol.IsEnabled)
                    {
                        TxtSymbol.Properties["Text"] = dr[TestDataConstants.COL_SYMBOL].ToString();
                        TxtSymbol.Click(MouseButtons.Left);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Wait(4000);
                    }

                    ExtentionMethods.WaitForEnabledForTTUseCase(ref btn, 20);

                    if (!btn.IsEnabled)
                    {
                        throw new Exception("Symbol may not exist in SM." + symbol);
                    }
                }

                if (dr[TestDataConstants.COL_DEAL_IN].ToString() != String.Empty && CmbDealIn.IsVisible)
                {
                    if (!CmbDealIn.Text.Equals(dr[TestDataConstants.COL_DEAL_IN].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                    {
                        CmbDealIn.Click(MouseButtons.Left);
                        CmbDealIn.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_DEAL_IN].ToString();
                        //ClearText(CmbDealIn);
                        //Keyboard.SendKeys(dr[TestDataConstants.COL_DEAL_IN].ToString());
                    }
                }

                if (dr[TestDataConstants.COL_ORDER_SIDE].ToString() != String.Empty && CmbOrderSide.IsEnabled)
                {
                    CmbOrderSide.Click(MouseButtons.Left);
                    CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_SIDE].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }


                if (dr.Table.Columns.Contains(TestDataConstants.COL_IsShortLocate))
                {
                    string booln = dr[TestDataConstants.COL_IsShortLocate].ToString();
                    if (booln.Equals("True"))
                    {
                        BtnShortLocateList.Click(MouseButtons.Left);
                    }
                }


                if (ShortLocateListGrid.IsVisible)
                {
                    ShortLocateListGrid.BringToFront();
                    if (dr[TestDataConstants.COL_Borrow_QUANTITY].ToString() != String.Empty)
                        UpdateShortLocate(dr);
                }
                else
                {

                    if (dr[TestDataConstants.COL_QUANTITY].ToString() != String.Empty && NmrcQuantity.IsEnabled)
                    {
                        NmrcQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_QUANTITY].ToString();

                        if (dr.Table.Columns.Contains(TestDataConstants.COL_INC_DEC_QUANTITY) && dr[TestDataConstants.COL_INC_DEC_QUANTITY].ToString() != String.Empty)
                        {
                            if (dr[TestDataConstants.COL_INC_DEC_QUANTITY].ToString() != String.Empty && dr[TestDataConstants.COL_INC_DEC_QUANTITY].ToString() == "Increase")
                            {
                                NmrcQuantity.Click(MouseButtons.Left);
                                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                            }
                            else if (dr[TestDataConstants.COL_INC_DEC_QUANTITY].ToString() != String.Empty && dr[TestDataConstants.COL_INC_DEC_QUANTITY].ToString() == "Decrease")
                            {
                                NmrcQuantity.Click(MouseButtons.Left);
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            }
                        }
                    }
                }

                if (dr[TestDataConstants.COL_TIF].ToString() != String.Empty && CmbTIF.IsEnabled)
                {
                    CmbTIF.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TIF].ToString();
                }

                if (NmrcTargetQuantity.IsVisible)
                {
                    if (dr[TestDataConstants.COL_TARGET_QUANTITY_IN].ToString() != String.Empty && NmrcTargetQuantity.IsEnabled)
                    {
                        NmrcTargetQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TARGET_QUANTITY_IN].ToString();
                    }
                }

                if (dr[TestDataConstants.COL_ORDER_TYPE].ToString() != String.Empty && CmbOrderType.IsEnabled)
                {
                    CmbOrderType.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_TYPE].ToString();
                }

                if (dr.Table.Columns.Contains(TestDataConstants.Broker_PadLock) && dr[TestDataConstants.Broker_PadLock].ToString().ToUpper() == "TRUE")
                {
                    BtnPadlock.Click(MouseButtons.Left);
                }

                if (dr[TestDataConstants.COL_BROKER].ToString() != String.Empty && CmbBroker.IsEnabled)
                {
                    if (dr[TestDataConstants.COL_BROKER].ToString() == "BLANK")
                    {
                        while (CmbBroker.Text.Length > 0)
                        {

                            CmbBroker.DoubleClick(MouseButtons.Left);

                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                        }


                    }
                    else
                    {
                        CmbBroker.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_BROKER].ToString();
                    }
                }
                if (dr[TestDataConstants.COL_NOTES].ToString() != String.Empty && TxtNotes.IsEnabled)
                {
                    TxtNotes.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_NOTES].ToString();
                }

                if (dr[TestDataConstants.COL_BROKER_NOTES].ToString() != String.Empty && TxtBrokerNotes.IsEnabled)
                {
                    TxtBrokerNotes.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_BROKER_NOTES].ToString();
                }
                if (dr[TestDataConstants.COL_FXRATE].ToString() != String.Empty && NmrcFXRate.IsEnabled)
                {
                    NmrcFXRate.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_FXRATE].ToString();
                }
                if (dr[TestDataConstants.COL_SETTLEMENT_FX_OPERATOR].ToString() != String.Empty && CmbFxOperator.IsEnabled)
                {
                    CmbFxOperator.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SETTLEMENT_FX_OPERATOR].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dr[TestDataConstants.COL_TRADE_DATE].ToString()) && DtTradeDate.IsEnabled)
                {
                    string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(dr[TestDataConstants.COL_TRADE_DATE].ToString()));
                    DtTradeDate.Click(MouseButtons.Left);
                    DtTradeDate.Properties[TestDataConstants.TEXT_PROPERTY] = date;
                }
                if (dr[TestDataConstants.COL_STRATEGY].ToString() != String.Empty && CmbStrategy.IsEnabled)
                {
                    CmbStrategy.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_STRATEGY].ToString();
                }
                if (dr[TestDataConstants.COL_STOP].ToString() != String.Empty && NmrcStop.IsEnabled)
                {
                    NmrcStop.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_STOP].ToString();
                }
                if (dr[TestDataConstants.COL_LIMIT].ToString() != String.Empty && NmrcLimit.IsEnabled)
                {
                    NmrcLimit.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_LIMIT].ToString();
                }
                if (dr[TestDataConstants.COL_PRICE].ToString() != String.Empty && NmrcPrice.IsEnabled)
                {
                    // Wait(3000);
                    NmrcPrice.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_PRICE].ToString();
                }

                String[] Accounts = dr["Account"].ToString().Split(',');
                String[] AllocationPercentage = dr["Account %"].ToString().Split(',');
                String[] AllocationQuantity = { };
                if (dr.Table.Columns.Contains("Account QTY"))
                {
                    AllocationQuantity = dr["Account QTY"].ToString().Split(',');
                }

                if (dr[TestDataConstants.COL_ALLOCATION_METHOD].ToString() != String.Empty && CmbAllocation.IsEnabled)
                {
                    //Timer added to prevent the test case to get struck in loop
                    //Changes required as test cases failed when struck in this loop
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    while (!(CmbAllocation.Text.Equals(dr[TestDataConstants.COL_ALLOCATION_METHOD].ToString()) || timer.ElapsedMilliseconds >= 50000))
                    {
                        /*CmbAllocation.Click();
                        Keyboard.SendKeys("[END][SHIFT+HOME]");
                        Keyboard.SendKeys(dr[TestDataConstants.COL_ALLOCATION_METHOD].ToString());*/
                        CmbAllocation.Properties["Text"] = dr[TestDataConstants.COL_ALLOCATION_METHOD].ToString();
                    }
                    timer.Stop();
                }
                else if (Accounts.Length > 1)
                {
                    BtnAccountQty.Click(MouseButtons.Left);
                    Wait(1000);

                    //  var gridMssaObject = GrdAccounts.MsaaObject;
                    //Dictionary that stores mapping of column names with it's index
                    ////Dictionary<string, int> colToIndexMappingDictionary = new Dictionary<string, int>();
                    // for (int i = 1; i < gridMssaObject.CachedChildren[0].ChildCount; i++)
                    // {
                    //   colToIndexMappingDictionary.Add(gridMssaObject.CachedChildren[0].CachedChildren[i].CachedChildren[0].Value, i);
                    //}
                    if (dr.Table.Columns.Contains(TestDataConstants.CLEAROLDCA) && btn.Equals(BtnReplace))
                    {

                        if (dr[TestDataConstants.CLEAROLDCA].ToString().ToUpper() == "YES")
                        {
                            GrdAccounts.Click(MouseButtons.Left);
                            BtnClear.Click(MouseButtons.Left);
                            // Wait(6000);

                        }
                        else
                        {
                            // no action needed 
                        }

                    }
                    else if (dr.Table.Columns.Contains(TestDataConstants.CLEAROLDCA))
                    {
                        if (dr[TestDataConstants.CLEAROLDCA].ToString().ToUpper() == "YES")
                        {
                            GrdAccounts.Click(MouseButtons.Left);
                            BtnClear.Click(MouseButtons.Left);
                            // Wait(6000);

                        }
                    }

                    int rowCount = 0, msaaId = 0;
                    //const int colCount = 4;
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

                                /*for (int j = 1; j <= colCount; j++)
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);*/
                                if (dr.Table.Columns.Contains("Account QTY") && !dr["Account QTY"].Equals(""))
                                {
                                    gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[4].Click(MouseButtons.Left);
                                    Keyboard.SendKeys(AllocationQuantity[rowCount]);
                                }
                                else
                                {
                                    gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[5].Click(MouseButtons.Left);
                                    Keyboard.SendKeys(AllocationPercentage[rowCount]);
                                }
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
                                    /*for (int j = 1; j <= colCount; j++)
                                        Keyboard.SendKeys(KeyboardConstants.TABKEY);*/

                                    if (dr.Table.Columns.Contains("Account QTY") && !dr["Account QTY"].Equals(""))
                                    {
                                        gridMssaObject.CachedChildren[msaaId].CachedChildren[k].CachedChildren[4].Click(MouseButtons.Left);
                                        Keyboard.SendKeys(AllocationQuantity[rowCount]);

                                    }
                                    else
                                    {
                                        gridMssaObject.CachedChildren[msaaId].CachedChildren[k].CachedChildren[5].Click(MouseButtons.Left);
                                        Keyboard.SendKeys(AllocationPercentage[rowCount]);

                                    }
                                    rowCount++;
                                    break;
                                }
                            }
                        }
                    }

                    BtnOK.Click(MouseButtons.Left);
                }
                Wait(3000);
                if (uiWindow1.IsVisible || uiWindow1.IsEnabled)
                {
                    ButtonOK2.Click(MouseButtons.Left);
                }

                //   Wait(3000);
                if (dr.Table.Columns.Contains(TestDataConstants.COL_ALGO_STRATEGY) && dr[TestDataConstants.COL_ALGO_STRATEGY].ToString() != String.Empty && CmbbxStrategy.IsVisible)
                {
                    CmbbxStrategy.Click(MouseButtons.Left);
                    if (dr[TestDataConstants.COL_ALGO_STRATEGY].ToString() != String.Empty && CmbbxStrategy.IsEnabled)
                    {
                        CmbbxStrategy.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ALGO_STRATEGY].ToString();
                    }
                }

                Commision.Click(MouseButtons.Left);
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
                if (dr[TestDataConstants.COL_SOFT_RATE].ToString() != String.Empty && NmrcSoftRate.IsEnabled)
                {
                    NmrcSoftRate.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SOFT_RATE].ToString();
                }
                Wait(1000);
                TradeAttribute.Click(MouseButtons.Left);
                if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString() != String.Empty && CmbTradeAttribute1.IsEnabled)
                {
                    CmbTradeAttribute1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString();
                }
                if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString() != String.Empty && CmbTradeAttribute2.IsEnabled)
                {
                    CmbTradeAttribute2.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString();
                }
                if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString() != String.Empty && CmbTradeAttribute3.IsEnabled)
                {
                    CmbTradeAttribute3.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString();
                }
                if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString() != String.Empty && CmbTradeAttribute4.IsEnabled)
                {
                    CmbTradeAttribute4.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString();
                }
                if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString() != String.Empty && CmbTradeAttribute5.IsEnabled)
                {
                    CmbTradeAttribute5.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString();
                }
                if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString() != String.Empty && CmbTradeAttribute6.IsEnabled)
                {
                    CmbTradeAttribute6.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString();
                }
                Wait(1000);
                if (Settlement.IsVisible == true)
                {
                    Settlement.Click(MouseButtons.Left);
                    if (dr[TestDataConstants.COL_SETTLEMENT_CURRENCY].ToString() != String.Empty && CmbSettlementCurrency.IsEnabled)
                    {
                        CmbSettlementCurrency.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SETTLEMENT_CURRENCY].ToString();
                    }

                    if (dr[TestDataConstants.COL_SETTLEMENT_FX_RATE].ToString() != String.Empty && NmrcSettelmentCurrencyFxRate.IsEnabled)
                    {
                        NmrcSettelmentCurrencyFxRate.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SETTLEMENT_FX_RATE].ToString();
                    }
                }

                Other.Click(MouseButtons.Left);
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
                if (dr[TestDataConstants.COL_EXECUTION_INSTRUCTIONS].ToString() != String.Empty && CmbExecutionInstructions.IsEnabled)
                {
                    CmbExecutionInstructions.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_EXECUTION_INSTRUCTIONS].ToString();
                }
                if (dr[TestDataConstants.COL_HANDLING_INSTRUCTIONS].ToString() != String.Empty && CmbHandlingInstructions.IsEnabled)
                {
                    CmbHandlingInstructions.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_HANDLING_INSTRUCTIONS].ToString();
                }
                if (dr[TestDataConstants.COL_TRADER].ToString() != String.Empty && CmbTradingAccount.IsEnabled)
                {
                    CmbTradingAccount.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADER].ToString();
                }
                Wait(3000);
                bool bookswap = false;
                if (dr.Table.Columns.Contains(TestDataConstants.COL_CHKBOX_BOOK_AS_SWAP))
                {
                    Boolean.TryParse(dr[TestDataConstants.COL_CHKBOX_BOOK_AS_SWAP].ToString(), out bookswap);
                    if (bookswap)
                    {
                        if (ChkBoxOption1.IsChecked)
                        {
                            ChkBoxOption1.Click(MouseButtons.Left);
                        }
                        ChkBoxSwap1.Click(MouseButtons.Left);
                        Swap.Click(MouseButtons.Left);

                        Txtbx2.Click(MouseButtons.Left);
                        if (dr.Table.Columns.Contains(TestDataConstants.COL_INTEREST_RATE))
                        {
                            if (dr[TestDataConstants.COL_INTEREST_RATE].ToString() != String.Empty)
                            {
                                ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_INTEREST_RATE].ToString(), string.Empty, true);
                            }
                        }
                    }
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
        /// Clears the text.
        /// </summary>
        /// <param name="cmb">The CMB.</param>
        private void ClearText(UIWindow cmb)
        {
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                while (cmb.Text.Length > 0 && timer.ElapsedMilliseconds < 50000)
                {
                    cmb.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[END][SHIFT+HOME][BACKSPACE]");
                }
                timer.Stop();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        void UpdateShortLocate(DataRow dr)
        {
            var gridRow = GrdShortLocateList.MsaaObject;
            Dictionary<string, int> indexToColumnMapDictionary = new Dictionary<string, int>();
            Wait(2000);
            for (int colIndex = 0; colIndex < gridRow.FindDescendantByName("BindingList`1", 3000).FindDescendantByName("Column Headers", 3000).ChildCount; colIndex++)
            {
                if (indexToColumnMapDictionary.ContainsKey(gridRow.FindDescendantByName("BindingList`1", 3000).FindDescendantByName("Column Headers", 3000).CachedChildren[colIndex].Name))
                {
                    indexToColumnMapDictionary.Add(gridRow.FindDescendantByName("BindingList`1", 3000).FindDescendantByName("Column Headers", 3000).CachedChildren[colIndex].Name + '2', colIndex);
                }
                else
                {
                    indexToColumnMapDictionary.Add(gridRow.FindDescendantByName("BindingList`1", 3000).FindDescendantByName("Column Headers", 3000).CachedChildren[colIndex].Name, colIndex);
                }
            }
            if (gridRow.FindDescendantByName("BindingList`1", 3000).ChildCount > 3)
            {
                gridRow.FindDescendantByName("BindingList`1", 3000).FindDescendantByName("BindingList`1 row 1", 3000).CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_QUANTITY]].Click(MouseButtons.Left);
                if (dr[TestDataConstants.COL_QUANTITY].ToString() != String.Empty)
                {
                    Keyboard.SendKeys(dr[TestDataConstants.COL_QUANTITY].ToString());
                }
                BtnSubmit.Click(MouseButtons.Left);
            }
            else if (gridRow.FindDescendantByName("BindingList`1", 3000).ChildCount <= 3)
            {
                var gridRow1 = GrdShortLocateList.MsaaObject;
                gridRow.FindDescendantByName("BindingList`1", 3000).FindDescendantByName("Template Add Row", 3000).CachedChildren[indexToColumnMapDictionary[TestDataConstants.Col_Broker]].Click(MouseButtons.Left);
                if (dr[TestDataConstants.COL_BROKER].ToString() != String.Empty)
                {

                    Keyboard.SendKeys(dr[TestDataConstants.COL_BROKER].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_QUANTITY].ToString() != String.Empty)
                {
                    Keyboard.SendKeys(dr[TestDataConstants.COL_QUANTITY].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_QUANTITY].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Rate].ToString() != String.Empty)
                {
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Rate].ToString());
                }
                BtnSubmit.Click(MouseButtons.Left);
            }

            else
            {
                KeyboardUtilities.CloseWindow(ref ShortLocateListPopUp_UltraFormManager_Dock_Area_Top);
                if (dr[TestDataConstants.COL_QUANTITY].ToString() != String.Empty && NmrcQuantity.IsEnabled)
                {
                    NmrcQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_QUANTITY].ToString();
                }
            }

        }

        /// <summary>
        /// Inputs the trade.
        /// </summary>
        /// <param name="dr">The dr.</param>
        public void InputTrade(DataRow dr)
        {
            try
            {
                if (TxtSymbol.IsEnabled)
                {
                    TxtSymbol.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_SYMBOL].ToString(), KeyboardConstants.ENTERKEY, true);
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }
                if (dr[TestDataConstants.COL_DEAL_IN].ToString() != String.Empty)
                {
                    if (!CmbDealIn.Text.Equals(dr[TestDataConstants.COL_DEAL_IN].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                    {
                        CmbDealIn.Click(MouseButtons.Left);
                        ClearText(CmbDealIn);
                        Keyboard.SendKeys(dr[TestDataConstants.COL_DEAL_IN].ToString());
                    }
                }
                if (!dr[TestDataConstants.COL_SIDE].ToString().Equals(String.Empty) && !CmbOrderSide.Text.Equals(dr[TestDataConstants.COL_SIDE].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                {
                    CmbOrderSide.Click(MouseButtons.Left);
                    ClearText(CmbOrderSide);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SIDE].ToString());
                }

                if (dr[TestDataConstants.COL_TARGET_QUANTITY_IN].ToString() != String.Empty && !BtnTargetQuantityPercentage.Text.Equals(dr[TestDataConstants.COL_TARGET_QUANTITY_IN].ToString()))
                {
                    BtnTargetQuantityPercentage.Click(MouseButtons.Left);
                }

                NmrcQuantity.Click(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_QUANTITY].ToString(), string.Empty, true);

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

                    /*  var gridMssaObject = GrdAccounts.MsaaObject;
                      //Dictionary that stores mapping of column names with it's index
                      Dictionary<string, int> colToIndexMappingDictionary = new Dictionary<string, int>();
                      for (int i = 1; i < gridMssaObject.CachedChildren[0].ChildCount; i++)
                      {
                          colToIndexMappingDictionary.Add(gridMssaObject.CachedChildren[0].CachedChildren[i].CachedChildren[0].Value, i);
                      }
                      int temp = 0;
                      foreach (String Account in Accounts)
                      {
                          gridMssaObject.CachedChildren[0].CachedChildren[colToIndexMappingDictionary[Account]].CachedChildren[4].Click(MouseButtons.Left);
                          Keyboard.SendKeys(AllocationPercentage[temp]);
                          temp++;
                      }*/
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
                if (dr[TestDataConstants.COL_EXECUTED_QUANTITY].ToString() != string.Empty)
                {
                    NmrcTargetQuantity.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_EXECUTED_QUANTITY].ToString(), string.Empty, true);
                }
                if (dr[TestDataConstants.COL_AVERAGE_PRICE].ToString() != string.Empty)
                {
                    NmrcPrice.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_AVERAGE_PRICE].ToString(), string.Empty, true);
                }
                String inputCommissionBasis = dr[TestDataConstants.COL_COMMISSION_BASIS].ToString();
                String inputCommission = dr[TestDataConstants.COL_COMMISSION].ToString();
                if ((inputCommissionBasis != String.Empty) && (inputCommission != String.Empty))
                {
                    Commision.Click(MouseButtons.Left);
                    CmbCommissionBasis.Click(MouseButtons.Left);
                    ClearText(CmbCommissionBasis);
                    Keyboard.SendKeys(inputCommissionBasis);
                    NmrcCommissionRate.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(inputCommission, string.Empty, true);
                }

                Wait(1000);
                bool bookswap = false;
                Boolean.TryParse(dr[TestDataConstants.COL_CHKBOX_BOOK_AS_SWAP].ToString(), out bookswap);
                if (bookswap)
                {
                    if (ChkBoxOption1.IsChecked)
                    {
                        ChkBoxOption1.Click(MouseButtons.Left);
                    }
                    ChkBoxSwap1.Click(MouseButtons.Left);
                    Swap.Click(MouseButtons.Left);

                    Txtbx2.Click(MouseButtons.Left);
                    if (dr[TestDataConstants.COL_INTEREST_RATE].ToString() != String.Empty)
                    {
                        ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_INTEREST_RATE].ToString(), string.Empty, true);
                    }

                    DtPkrResetDate.Click(MouseButtons.Left);
                    if (dr[TestDataConstants.COL_FIRST_RESET_DATE].ToString() != String.Empty)
                    {
                        ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_FIRST_RESET_DATE].ToString(), string.Empty, true);
                    }

                }


                String action = dr[TestDataConstants.COL_ACTION].ToString();
                if (action.Equals("Done Away") || action.Equals("Replace"))
                {
                    BtnDoneAway.Click(MouseButtons.Left);
                }
                else if (action.Equals("Create Order"))
                {
                    BtnCreateOrder.Click(MouseButtons.Left);
                }
                else if (action.Equals("Send"))
                {
                    BtnSend.Click(MouseButtons.Left);
                    if (Warning.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                }

                if (bookswap && ChkBoxSwap.IsChecked)
                {
                    ChkBoxSwap.Click(MouseButtons.Left);
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
        /// Opens the manual trading ticket.
        /// </summary>
        protected void OpenManualTradingTicket()
        {
            try
            {
                //Shortcut to open Trading Ticket (CTRL + SHIFT + T)
                // Wait(5000);
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_TT"]);
                if (!TradingTicket1.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref TradingTicket1, 20);
                }
                //Trade.Click(MouseButtons.Left);
                //TradingTicket.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception("TradingTicket is not opened " + ex);
            }
        }

        /// <summary>
        /// Closes the trading ticket.
        /// </summary>
        public void CloseTradingTicket()
        {
            try
            {
                if (TradingTicket1.IsValid)
                {
                    KeyboardUtilities.CloseWindow(ref TradingTicket_UltraFormManager_Dock_Area_Top);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

        }
        /// <summary>
        /// Handles the AttachFailing event of the TradingTicket2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AttachFailingEventArgs"/> instance containing the event data.</param>
        /// ToDo: Add a condition for detach the TradingTicket2 pop up 
        private void TradingTicket2_AttachFailing(object sender, AttachFailingEventArgs e)
        {

            try
            {
                if (e.CurrentRetryCount > 2)
                    TradingTicket2.AttachFailing -= TradingTicket2_AttachFailing;

                else
                {
                    CustomMessageBox.MatchedIndex = 1;
                    TableLayoutPanel1.MatchedIndex = 1;
                    TradingTicket1.MatchedIndex = 1;
                    TradingTicket.MatchedIndex = 1;
                    Wait(1000);
                    CustomMessageBox.MatchedIndex = 0;
                    TableLayoutPanel1.MatchedIndex = 0;
                    TradingTicket1.MatchedIndex = 0;
                    TradingTicket.MatchedIndex = 0;
                    Wait(1000);
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
        protected void OpenGeneralPreferences()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open Preferences under Tools (CTRL + ALT + F)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
                // Wait(5000);
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 15);
                //Tools.Click(MouseButtons.Left);
                //Preferences.Click(MouseButtons.Left);
                Trading.Click(MouseButtons.Left);
                GeneralPreferences.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public void CheckCompliance(DataRow dr)
        {
            try
            {
                try {
                    ComplianceAlertPopupUC1.WaitForEnabled();
                }
                catch { ComplianceAlertPopupUC2.WaitForEnabled(); }
                string allowTrade = string.Empty;
                string alertt = string.Empty;
                //condition to check(mainly for replace as tt closes)
                bool ResponseAttachedflag = false;

                if (ComplianceAlertPopupUC2.IsVisible || ComplianceAlertPopupUC2.IsEnabled)
                {
                    if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_ALLOW_TRADE].ToString()))
                        allowTrade = dr[TestDataConstants.COL_ALLOW_TRADE].ToString();

                    if (allowTrade.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (ResponseButton.IsVisible || ResponseButton.IsEnabled)
                        {
                            ResponseButton.Click(MouseButtons.Left);
                            ResponseAttachedflag = true;
                        }
                        else
                        {
                            ResponseButton.Click(MouseButtons.Left);
                            ResponseAttachedflag = true;
                        }
                        Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                    }
                    else if (allowTrade.Equals("No", StringComparison.CurrentCultureIgnoreCase) || allowTrade.Equals(String.Empty, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (CancelButton1.IsVisible || CancelButton1.IsEnabled)
                        {
                            CancelButton1.Click(MouseButtons.Left);
                            ResponseAttachedflag = true;
                        }
                        else
                        {
                            CancelButton.Click(MouseButtons.Left);
                            ResponseAttachedflag = true;
                        }
                        Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                    }
                }

                if (ResponseAttachedflag == false)
                {
                    if (ComplianceAlertPopupUC1.IsVisible || ComplianceAlertPopupUC1.IsEnabled)
                    {
                        if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_ALLOW_TRADE].ToString()))
                            allowTrade = dr[TestDataConstants.COL_ALLOW_TRADE].ToString();

                        if (allowTrade.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ResponseButton3.Click(MouseButtons.Left);
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                        else if (allowTrade.Equals("No", StringComparison.CurrentCultureIgnoreCase) || allowTrade.Equals(String.Empty, StringComparison.CurrentCultureIgnoreCase))
                        {
                            CancelButton3.Click(MouseButtons.Left);
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                    }
                }
                if (NirvanaCompliance.IsVisible)
                {
                    ButtonNo.Click(MouseButtons.Left);
                }
            }



            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected void CopyTTGeneralPref()
        {
            try
            {
                string DefaultSymbologySourceOldFile = ConfigurationManager.AppSettings["DefaultSymbologySourceOldFile"];
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                File.Copy(DefaultSymbologySourceOldFile, DefaultSymbologySourceNewFile);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected void RevertTTGenPref()
        {
            try
            {
                string DefaultSymbologySourceOldFile = ConfigurationManager.AppSettings["DefaultSymbologySourceOldFile"];
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                Console.WriteLine("Reverting Pref.");
                File.Delete(DefaultSymbologySourceOldFile);
                File.Move(DefaultSymbologySourceNewFile, DefaultSymbologySourceOldFile);
                Console.WriteLine("Pref Reverted Successfully");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }


        public HashSet<string> provideAllPossibleSuggestionValues(ref UIWindow cmb, string uniqueDescendantByName)
        {

            HashSet<string> hs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var msaaObj = cmb.MsaaObject;
                var childmsaaObj = msaaObj.FindDescendantByName(uniqueDescendantByName, 5000).CachedChildren;
                int itemsCount = childmsaaObj.Count;
                ConcurrentHashSet<string> concurrentHashSet = new ConcurrentHashSet<string>();

                Parallel.For(0, itemsCount, i =>
                {
                    var item = childmsaaObj[i];

                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        string itemName = item.Name.ToString();
                        concurrentHashSet.Add(itemName);
                    }
                });
                foreach (var value in concurrentHashSet)
                {
                    hs.Add(value);
                }

                return hs;
            }
            catch (Exception ex)
            {
                throw new Exception("provideAllPossibleSuggestionValues failed:" + ex.Message);
            }
            return hs;

        }
        public bool VerifySuggestionList(string Value, ref UIWindow targetwindow, ref string MainWindow, bool verifyValue = true, bool alsoSelectElement = false, string newSymbolValue = "")
        {
            bool verified = false;
            if (!verifyValue)
            {
                verified = true;
            }

            try
            {
                // string venueValue = dr[TestDataConstants.COL_BROKER].ToString();
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                // Wait(1000);

                HashSet<string> hs = provideAllPossibleSuggestionValues(ref targetwindow, "List");
                Dictionary<string, int> activesuggesstionList = new Dictionary<string, int>();
                activesuggesstionList = WinDataUtilities.GetCurrentSuggestionsList(ref MainWindow, activesuggesstionList);


                foreach (string dataitem in activesuggesstionList.Keys)
                {
                    bool wordVerified = false;

                    if (hs.Contains(dataitem))
                    {
                        Console.WriteLine(dataitem + " exists in suggestions");
                    }
                    if (!hs.Contains(dataitem))
                    {
                        throw new Exception("DataItem verification failed");
                    }
                    if (wordVerified != true && verifyValue == true)
                    {
                        if (string.Equals(Value, dataitem.Substring(1, Value.Length)))//as every ticker symbol have space in first
                        {
                            wordVerified = true;
                        }
                    }
                    if (wordVerified == false && verifyValue == true)
                    {
                        // Throw an exception or handle as needed
                        throw new Exception("Word verification failed");
                    }
                }
                verified = true;
                if (alsoSelectElement)
                {
                    int currentIndex = 0;
                    int finalIndex = activesuggesstionList.ContainsKey(newSymbolValue) ? activesuggesstionList[newSymbolValue] : -1;
                    if (finalIndex == -1)
                    {
                        finalIndex = activesuggesstionList.ContainsKey(" " + newSymbolValue) ? activesuggesstionList[" " + newSymbolValue] : -1;
                    }
                    if (finalIndex < currentIndex)
                    {
                        throw new Exception("Word verification failed as no Index Found on ActiveSuggesstions list");
                    }
                    else
                    {
                        KeyboardUtilities.PressDownKeyWithWait(finalIndex - currentIndex);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                }
                else
                {
                    Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                }
                return verified;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured while verification:-" + ex.Message);
                return verified;
            }
            return verified;
        }
        public static int GetElementIndex(Dictionary<string, int> dict, string element)
        {

            int index = 0;
            foreach (var kvp in dict)
            {
                if (kvp.Key == element)
                {
                    return dict[element];
                }
                index++;
            }

            return -1;
        }

    }
    public class ConcurrentHashSet<T> : IDisposable
    {
        private readonly ConcurrentDictionary<T, byte> dictionary = new ConcurrentDictionary<T, byte>();

        public ConcurrentHashSet() { }

        public void Add(T item)
        {
            dictionary.TryAdd(item, 0);
        }

        public bool Contains(T item)
        {
            return dictionary.ContainsKey(item);
        }

        public void Dispose()
        {
            dictionary.Clear();
        }

        // Implement IEnumerable<T> to enable foreach loop
        public IEnumerator<T> GetEnumerator()
        {
            return dictionary.Keys.GetEnumerator();
        }
    }

}