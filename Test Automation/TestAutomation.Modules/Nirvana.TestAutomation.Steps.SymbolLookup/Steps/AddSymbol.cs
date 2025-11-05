using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    public class AddSymbol : SymbolLookupUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0].Columns.Contains(TestDataConstants.SM_FromTT))
                {
                    if (!String.IsNullOrEmpty(testData.Tables[0].Columns[TestDataConstants.SM_FromTT].ToString()))
                    {
                        OpenSymbolLookup();
                        GrdData.Click(MouseButtons.Right);
                        Wait(3000);
                    }

                }
                else
                {
                    OpenSymbolLookup();
                    GrdData.Click(MouseButtons.Right);
                    AddSymbol.Click(MouseButtons.Left);
                    Wait(3000);
                }
                KeyboardUtilities.MaximizeWindow(ref ValidAUECs_UltraFormManager_Dock_Area_Top);
                string exchangeIdentifier = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.EXCHANGE_IDENTIFIER].ToString();
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                var msaaObj = GrdAuec.MsaaObject;
                DataTable dtAuec = CSVHelper.CSVAsDataTable(this.GrdAuec.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] foundRow = dtAuec.Select(String.Format(@"[ExchangeIdentifier]='{0}'", exchangeIdentifier));
                int index = dtAuec.Rows.IndexOf(foundRow[0]);
                string asset = foundRow[0].ItemArray[2].ToString();
                GrdAuec.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                GrdAuec.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].CachedChildren[0].Click(MouseButtons.Left);
                Wait(3000);
                InputEnter(dr,asset);
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
                //MinimzieSybolLookup();
                //Wait(3000);
                //if (SymbolLookUp1.IsValid)
                //{
                //    if (Alert.IsVisible)
                //    {
                //        ButtonNo.Click(MouseButtons.Left);
                //    }
                //}
            }

            return _result;
        }
        private void InputEnter(DataRow dr, string Assets)
        {
            try
            {
                if (Assets.Equals("FX") && Assets.Equals("FXForward"))
                {
                    if (dr[TestDataConstants.COL_CURRENCY].ToString() != string.Empty)
                    {
                        UgcpCurrencyID.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[TestDataConstants.COL_CURRENCY].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                    if (dr[TestDataConstants.COL_SYMBOL].ToString() != string.Empty)
                    {
                        Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                        UgcpTickerSymbol_EmbeddableTextBox.Click(MouseButtons.Left);
                        //UgcpTickerSymbol.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[TestDataConstants.COL_SYMBOL].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                }
                else
                {
                    if (dr[TestDataConstants.COL_SYMBOL].ToString() != string.Empty)
                    {
                        Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                        UgcpTickerSymbol_EmbeddableTextBox.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[TestDataConstants.COL_SYMBOL].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                    if (dr[TestDataConstants.COL_CURRENCY].ToString() != string.Empty)
                    {
                        UgcpCurrencyID.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[TestDataConstants.COL_CURRENCY].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                }
                if (dr[TestDataConstants.COL_MULTIPLIER].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcMultiplier.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_MULTIPLIER].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_UNDERLYING_NAME].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgcpUnderLyingSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_UNDERLYING_NAME].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_BLOOMBERG_SYMBOL].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcBloombergSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_BLOOMBERG_SYMBOL].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Proxy_Symbol].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcProxySymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Proxy_Symbol].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_DESCRIPTION].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcLongName.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_DESCRIPTION].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                  
                }
                if (dr[TestDataConstants.COL_Expiration_Date].ToString() != string.Empty)
                {//////////////////////////////////////////////////////////////////
           //   string date = String.Format(ExcelStructureConstants.DATE_FORMAT, DateTime.Parse(dr[TestDataConstants.COL_Expiration_Date].ToString()));
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
              string date2 = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(dr[TestDataConstants.COL_Expiration_Date].ToString()));
                //DD/MM/YYYY
                    

                     Keyboard.SendKeys(date2);
                       Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                   // BtnNextTab.Click(MouseButtons.Left);
                if (dr[TestDataConstants.Col_Put_Call].ToString() != string.Empty)
                {
                    UgpcPutOrCal.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.Col_Put_Call].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.Col_Strike_Price].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcStrikePrice.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.Col_Strike_Price].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                 // Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                }
                if (dr[TestDataConstants.COL_Issue_Date].ToString() != string.Empty)
                {////////////////////////////////////////////////////////////////////////////////
                    UgpcIssueDate.Click(MouseButtons.Left);
                    string date2 = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(dr[TestDataConstants.COL_Issue_Date].ToString()));
                    //DD/MM/YYYY
                    Keyboard.SendKeys(date2);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                

                    
                }
                if (dr[TestDataConstants.COL_Accrual_Basis].ToString() != string.Empty)
                {
                    UgpcAccrualBasisID.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Accrual_Basis].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Coupon_Frequency].ToString() != string.Empty)
                {
                    UgpcCouponFrequencyID.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Coupon_Frequency].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Coupon].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcCoupon.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Coupon].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Bond_Type].ToString() != string.Empty)
                {
                    UgpcSecurityTypeID.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Bond_Type].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                   
                }
                if (dr[TestDataConstants.COL_First_Coupan_Date].ToString() != string.Empty)
                {////////////////////////////////////////////////////////////////////////////////_
                    string date2 = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(dr[TestDataConstants.COL_First_Coupan_Date].ToString()));
                    //DD/MM/YYYY
                   // UgpcFirstCouponDate.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(date2);
       

                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Days_To_Settlement].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcDaysToSettlement.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Days_To_Settlement].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Collateral_Type].ToString() != string.Empty)
                {
                    UgpcCollateralType.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Collateral_Type].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                BtnNextTab.Click(MouseButtons.Left);
                if (dr[TestDataConstants.COL_ROUNDLOTS].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcRoundLot.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_ROUNDLOTS].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Leveraged_FACTOR].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcDelta.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Leveraged_FACTOR].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Strike_Price_Multiplier].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgcpStrikePriceMultiplier.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Strike_Price_Multiplier].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_SEDOL_SYMBOL].ToString() != string.Empty)
                {
                    UgpcSedolSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SEDOL_SYMBOL].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_CUSIP_SYMBOL].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcCusipSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_CUSIP_SYMBOL].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_ISIN_SYMBOL].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcISINSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_ISIN_SYMBOL].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.Col_IDCO_Option].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcIDCOOptionSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.Col_IDCO_Option].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.Col_OSI_Option].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcOSIOptionSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.Col_OSI_Option].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_COMMENTS].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcComments.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_COMMENTS].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_SHARES_OUT_STANDING].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcShares.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SHARES_OUT_STANDING].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_FactSet_Symbol].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcFactSetSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_FactSet_Symbol].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_ACTIV_Symbol].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcActivSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_ACTIV_Symbol].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_ACTIV_Symbol].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcActivSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_ACTIV_Symbol].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                BtnNextTab.Click(MouseButtons.Left);
                if (dr[TestDataConstants.UDA_Security_Type].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgcpUDASecurity.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Security_Type].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Country].ToString() != string.Empty)
                {
                    UgpcUDACountry.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Country].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Asset_Class].ToString() != string.Empty)
                {
                    UgpcAssetID.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Asset_Class].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Sector].ToString() != string.Empty)
                {
                    UgpcUDASector.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Sector].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Sub_Sector].ToString() != string.Empty)
                {
                    UgpcUDASubSector.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Sub_Sector].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                BtnNextTab.Click(MouseButtons.Left);
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}