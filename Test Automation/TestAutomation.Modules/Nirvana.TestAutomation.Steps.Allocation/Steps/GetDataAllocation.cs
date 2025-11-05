using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class GetDataAllocation : AllocationUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                GetAllocationDataBasedOnDateRange(testData, sheetIndexToName);
                string filterTabName = testData.Tables[sheetIndexToName[0]].Rows[0]["FilterTabName"].ToString();

                if (filterTabName != string.Empty)
                {
                    Filters1.Click(MouseButtons.Left);
                    if (filterTabName.Equals("All", StringComparison.InvariantCultureIgnoreCase))
                    {
                        All.Click(MouseButtons.Left);
                    }
                    else
                    {
                        AllocatedDivideUnallocated.Click(MouseButtons.Left);
                    }
                    if (filterTabName.Equals("Allocated", StringComparison.InvariantCultureIgnoreCase))
                    {
                        KeyboardUtilities.MouseScrollDown(5);

                    }
                    getFilters(testData, sheetIndexToName);
                }
            }
            catch(Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "GetDataAllocation");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
            MinimizeAllocation();
            }
            return _res;
        }


        private void GetAllocationDataBasedOnDateRange(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                //If RangeType cell value is Blank or White Space then by default Current Radio button will be selected
                String rangetype = TestDataConstants.COL_CURRENT;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    if (!String.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_RANGETYPE].ToString()))
                        rangetype = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_RANGETYPE].ToString();
                }
                if (rangetype.Equals(TestDataConstants.COL_CURRENT, StringComparison.InvariantCultureIgnoreCase))
                {
                    Current.Click(MouseButtons.Left);
                }
                else
                {
                    ExtentionMethods.WaitForUIElementEnable(ref Historical, 20);
                    if (Historical.IsEnabled)
                    {
                        Historical.Click(MouseButtons.Left);
                        String fromdate = string.Empty;
                        if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()))
                                fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()));
                            FromDate.Click(MouseButtons.Left);
                            ExtentionMethods.CheckCellValueConditions(fromdate, string.Empty, true);
                            ToDate.Click(MouseButtons.Left);
                            String todate = string.Empty;
                            if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TO].ToString()))
                                todate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TO].ToString()));
                            ExtentionMethods.CheckCellValueConditions(todate, string.Empty, true);
                        }
                       // GetData.Click(MouseButtons.Left);
                    }
                }
                GetData.Click(MouseButtons.Left);
              //  Wait(3000);
                // KeyboardUtilities.CloseUI();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Dictionary<string, int> CreateAssetDictionary()
        {
            Dictionary<string, int> AssetNameToId = new Dictionary<string, int>();
            AssetNameToId.Add("Equity", 1);
            AssetNameToId.Add("EquityOption", 2);
            AssetNameToId.Add("Future", 3);
            AssetNameToId.Add("FutureOption", 4);
            AssetNameToId.Add("FX", 5);
            AssetNameToId.Add("Cash", 6);
            AssetNameToId.Add("Indices", 7);
            AssetNameToId.Add("FixedIncome", 8);
            AssetNameToId.Add("PrivateEquity", 9);
            AssetNameToId.Add("FXOption", 10);
            AssetNameToId.Add("FXForward", 11);
            AssetNameToId.Add("Forex", 12);
            AssetNameToId.Add("ConvertibleBond", 13);
            AssetNameToId.Add("CreditDefaultSwap", 14);
            AssetNameToId.Add("Allocation1", 1186);
            AssetNameToId.Add("Allocation2", 1184);
            AssetNameToId.Add("Allocation3", 1185);
            AssetNameToId.Add("LP C/O", 1183);
            AssetNameToId.Add("OFFSHORE", 1182);
            AssetNameToId.Add("rt", 1189);
            AssetNameToId.Add("Buy", 1);
            AssetNameToId.Add("Sell", 2);
            AssetNameToId.Add("Buy minus", 3);
            AssetNameToId.Add("Sell plus", 4);
            AssetNameToId.Add("Sell short", 5);
            AssetNameToId.Add("Sell short exempt", 6);
            AssetNameToId.Add("Cross", 7);
            AssetNameToId.Add("Cross short", 8);
            AssetNameToId.Add("Buy to Open", 9);
            AssetNameToId.Add("Buy to Close", 10);
            AssetNameToId.Add("Sell to Open", 11);
            AssetNameToId.Add("Sell to Close", 12);
            AssetNameToId.Add("MS", 1);
            AssetNameToId.Add("GS", 2);
            AssetNameToId.Add("CSFB", 5);
            AssetNameToId.Add("PiperJaffray", 6);
            AssetNameToId.Add("Bernstein", 7);
            AssetNameToId.Add("STN", 9);
            AssetNameToId.Add("FIMAT", 10);
            AssetNameToId.Add("Source", 11);
            AssetNameToId.Add("Lakeshore", 12);
            AssetNameToId.Add("Wolverine", 13);
            AssetNameToId.Add("DC", 14);
            AssetNameToId.Add("DB", 15);
            AssetNameToId.Add("UBS", 16);
            AssetNameToId.Add("dik", 17);
            AssetNameToId.Add("Long", 18);
            AssetNameToId.Add("S1", 16);
            AssetNameToId.Add("S2", 17);
            AssetNameToId.Add("Short", 19);
            AssetNameToId.Add("shrt", 20);
            AssetNameToId.Add("Strategy Unallocated", 0);
            AssetNameToId.Add("US", 1);
            AssetNameToId.Add("EU", 2);
            AssetNameToId.Add("Japan", 3);
            AssetNameToId.Add("AsiaXJapan", 4);
            AssetNameToId.Add("Brazil", 5);
            AssetNameToId.Add("Spot", 6);
            AssetNameToId.Add("Forwards", 7);
            AssetNameToId.Add("SA", 8);
            AssetNameToId.Add("Australia", 9);
            AssetNameToId.Add("EmergingDebt", 10);
            AssetNameToId.Add("OTCDerivatives", 11);
            AssetNameToId.Add("Multiple", 12);
            AssetNameToId.Add("LatinAmerica", 13);
            AssetNameToId.Add("MiddleEast", 14);
            AssetNameToId.Add("NASDAQ", 1);
            AssetNameToId.Add("OPTS", 13);
            AssetNameToId.Add("NYSE", 16);
            AssetNameToId.Add("CMEE", 17);
            AssetNameToId.Add("CMG", 18);
            AssetNameToId.Add("Auto", 19);
            AssetNameToId.Add("AMEX", 20);
            AssetNameToId.Add("HKG", 21);
            AssetNameToId.Add("TSE", 22);
            AssetNameToId.Add("TPX-Fut", 23);
            AssetNameToId.Add("SIMX-Fut", 24);
            AssetNameToId.Add("OSM", 25);
            AssetNameToId.Add("JAQ", 26);
            AssetNameToId.Add("Bovespa", 27);
            AssetNameToId.Add("SES", 28);
            AssetNameToId.Add("Oslo", 29);
            AssetNameToId.Add("remove", 30);//FX
            AssetNameToId.Add("PSPT", 31);
            AssetNameToId.Add("FRA", 32);
            AssetNameToId.Add("SEHK", 34);
            AssetNameToId.Add("CLOB", 36);
            AssetNameToId.Add("SESDAQ", 37);
            AssetNameToId.Add("Algo", 41);
            AssetNameToId.Add("LSE", 42);
            AssetNameToId.Add("Euronext", 43);
            AssetNameToId.Add("VSE", 44);
            AssetNameToId.Add("KFX", 46);
            AssetNameToId.Add("HEX", 47);
            AssetNameToId.Add("ISE", 48);
            AssetNameToId.Add("MSE", 52);
            AssetNameToId.Add("SWX", 53);
            AssetNameToId.Add("ASE", 54);
            AssetNameToId.Add("MIL", 55);
            AssetNameToId.Add("SPBEX", 56);
            AssetNameToId.Add("RTSX", 57);
            AssetNameToId.Add("SSE", 58);
            AssetNameToId.Add("JSE", 59);
            AssetNameToId.Add("PINX", 61);
            AssetNameToId.Add("eurex", 62);
            AssetNameToId.Add("GEM", 64);
            AssetNameToId.Add("CDS-OTC", 65);
            AssetNameToId.Add("xy", 66);
            AssetNameToId.Add("LME", 67);
            AssetNameToId.Add("NYME", 68);
            AssetNameToId.Add("BB", 69);
            AssetNameToId.Add("MEX", 70);

            return AssetNameToId;
        }

        private UIAutomationElement GetXamCombo(int index, int matchindex)
        {
            UIAutomationElement XamCombo = new UIAutomationElement();
            XamCombo.AutomationName = "";
            XamCombo.ClassName = "XamComboEditor";
            XamCombo.Comment = null;
            XamCombo.Index = index;//15
            XamCombo.ItemType = "";
            XamCombo.MatchedIndex = matchindex;
            XamCombo.Name = "XamCombo";
            XamCombo.Parent = this.AllocationFilterCommonControl;
            XamCombo.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            XamCombo.UseCoordinatesOnClick = true;
            return XamCombo;
        }

        private UIAutomationElement GetToggleBtn(UIAutomationElement xamcombo)
        {
            UIAutomationElement Toggle = new UIAutomationElement();
            Toggle.AutomationId = "ToggleButton";
            Toggle.AutomationName = "";
            Toggle.ClassName = "Button";
            Toggle.Comment = null;
            Toggle.ItemType = "";
            Toggle.MatchedIndex = 0;
            Toggle.Name = "Toggle";
            Toggle.Parent = xamcombo;
            Toggle.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            Toggle.UseCoordinatesOnClick = false;
            return Toggle;
        }

        private void Chckbox(string automationName)
        {
            UIAutomationElement comboItem = new UIAutomationElement();
            comboItem.AutomationName = automationName;// "[14, CreditDefaultSwap]";
            comboItem.ClassName = "ComboEditorItemControl";
            comboItem.Comment = null;
            comboItem.ItemType = "";
            comboItem.MatchedIndex = 0;
            comboItem.Name = " comboItem";
            comboItem.Parent = Popup1;
            comboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            comboItem.UseCoordinatesOnClick = true;
            comboItem.Click(MouseButtons.Left);
        }

        private void getFilters(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                //var a = FilterTabControl.AutomationElementWrapper;

                Cmbboxsymbol.Click(MouseButtons.Left);
                Dictionary<String, int> Aset = CreateAssetDictionary();

                DataTable test = testData.Tables[sheetIndexToName[0]];
                int index = -1;
                Dictionary<string, int> columnDict = new Dictionary<string, int>();
                foreach (DataColumn col in test.Columns)
                {
                    if (col.ColumnName == "RangeType" || col.ColumnName == "From" || col.ColumnName == "To")
                        continue;
                    if (col.ColumnName == "Symbol" && test.Rows[0][col.ColumnName].ToString() != string.Empty)
                    {
                        Cmbboxsymbol.Click(MouseButtons.Left);
                        Keyboard.SendKeys(test.Rows[0][col.ColumnName].ToString());
                        continue;
                    }

                    if (test.Rows[0][col.ColumnName].ToString() != string.Empty)
                    {
                        //columnDict.Add(col.ColumnName, index);
                        UIAutomationElement XamCombo = new UIAutomationElement();
                        XamCombo = GetXamCombo(14 + index, index);
                        UIAutomationElement Toggle = new UIAutomationElement();
                        Toggle = GetToggleBtn(XamCombo);
                        Toggle.Click(MouseButtons.Left);
                        string[] commaSeparatedComboValues = test.Rows[0][col.ColumnName].ToString().Split(',');
                        foreach (string combovalue in commaSeparatedComboValues)
                        {
                            string automationName = String.Empty;
                            if (col.ColumnName == "PreAllocated" || col.ColumnName == "ManualGroup")
                                automationName = combovalue;
                            else
                            {
                                automationName = "[" + Aset[combovalue] + ", " + combovalue + "]";
                            }
                            Chckbox(automationName);
                        }

                        Toggle.Click(MouseButtons.Left);
                    }
                    index++;
                    if (index == 8)
                    {
                        ScrollDown();
                    }
                }
                Apply1.DoubleClick(MouseButtons.Left);
                Records.Click(MouseButtons.Left);
            }
            catch (Exception) { throw; }
        }

        private static void ScrollDown()
        {
            MouseController.ScrollWheelDown();
            Wait(2000);
            MouseController.ScrollWheelDown();
            Wait(2000);
            MouseController.ScrollWheelDown();
            Wait(2000);
            MouseController.ScrollWheelDown();
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}