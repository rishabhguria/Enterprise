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
using System.Threading;

namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    public class EditSMGridUi : SymbolLookupUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            StringBuilder symbolErrors = new StringBuilder(string.Empty);
            try
            {
                if (testData != null)
                {
                    OpenSymbolLookup();
                    foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                    {
                        if (dtRow.Table.Columns.Contains(TestDataConstants.COL_SEARCH_BEFORE_EDIT))
                        {
                            if (!string.IsNullOrEmpty(TestDataConstants.COL_SEARCH_BEFORE_EDIT.ToString()))
                            {

                                TxtbxInput.DoubleClick(MouseButtons.Left);


                                Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);

                                while (TxtbxInput.Text.Length > 0)
                                {

                                    TxtbxInput.DoubleClick(MouseButtons.Left);

                                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                                }

                                Keyboard.SendKeys(dtRow[TestDataConstants.COL_SYMBOL].ToString());

                                CmbbxSearchType.Click(MouseButtons.Left);
                                Keyboard.SendKeys(dtRow[TestDataConstants.COL_SEARCH_TYPE].ToString());
                                BtnGetData.DoubleClick(MouseButtons.Left);
                                DataUtilities.waitForGridDataToGetVisible(GrdData, 10, 0);
                                // Wait(4000);
                            }
                        }
                        GrdData.WaitForVisible();
                        GrdData.Click(MouseButtons.Left);


                        if (testData.Tables[0] != null)
                        {
                            EditGrdDataUI(dtRow);
                        }
                        else if (testData.Tables[0] == null)
                        {
                            Console.WriteLine(" UI DATA IS NULL");
                        }
                        Wait(2000);
                        BtnSave1.Click(MouseButtons.Left);
                        Wait(2000); // improve wait to close UI after successfull release update

                    }

                }
                CloseSymbolLookup();
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

                //  Wait(3000);

            }

            return _result;
        }

        public void EditGrdDataUI(DataRow dr)
        {
            try
            {


                DataTable uiData = CSVHelper.CSVAsDataTable(this.GrdData.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                var msaaObj = GrdData.MsaaObject;
                // SELECTING COLUMN INDEXWISE
                //foreach (DataRow dr in exceldata.Rows)
                // {
                var underlyingindex = uiData.Columns["UseUDAFromUnderlyingOrRoot"];
                var isndfindex = uiData.Columns["Is NDF"];
                var iszero = uiData.Columns["IsZero"];
                var issecapprovedindex = uiData.Columns["IsSecApproved"];
                var strikePrice = uiData.Columns["Strike Price"];
                var multiplier = uiData.Columns["Multiplier"];
                var cusip = uiData.Columns["CUSIP"];
                var bloomberg = uiData.Columns["BB Code"];



                Dictionary<string, int> columnToIndexMapping = msaaObj.CachedChildren[1].CachedChildren[1].GetColumnIndexMaping(uiData);
                Wait(6000);
                if (underlyingindex != null && dr[TestDataConstants.USEUNDERLYINGROOTORROOT].ToString().ToUpper() == "YES")
                {
                    string colName = "UseUDAFromUnderlyingOrRoot";
                    int colIndex = columnToIndexMapping["UseUDAFromUnderlyingOrRoot"];
                    GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                    Wait(5000);
                    //msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);
                    msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);
                }
                if (isndfindex != null && dr[TestDataConstants.ISNDF].ToString().ToUpper() == "YES")
                {
                    string colName = "Is NDF";
                    int colIndex = columnToIndexMapping["Is NDF"];
                    GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                    Wait(5000);
                    msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);

                }
                if (iszero != null && dr[TestDataConstants.ISZERO].ToString().ToUpper() == "YES")
                {
                    string colName = "IsZero";
                    int colIndex = columnToIndexMapping["IsZero"];
                    GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                    Wait(5000);
                    msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);

                }
                if (issecapprovedindex != null && dr[TestDataConstants.ISSECAPPROVED].ToString().ToUpper() == "YES")
                {
                    string colName = "IsSecApproved";
                    int colIndex = columnToIndexMapping["IsSecApproved"];
                    GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                    Wait(5000);
                    msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);

                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_STRIKEPRICE))
                {

                    if (strikePrice != null && !string.IsNullOrEmpty(dr[TestDataConstants.COL_STRIKEPRICE].ToString()))
                    {
                        string colName = TestDataConstants.COL_STRIKEPRICE;
                        int colIndex = columnToIndexMapping[colName];
                        GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Wait(5000);
                        msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);
                        clearTextData();
                        Keyboard.SendKeys(dr[TestDataConstants.COL_STRIKEPRICE].ToString());

                    }
                }

                if (dr.Table.Columns.Contains(TestDataConstants.Col_Multiplier))
                {

                    if (strikePrice != null && !string.IsNullOrEmpty(dr[TestDataConstants.Col_Multiplier].ToString()))
                    {
                        string colName = TestDataConstants.Col_Multiplier;
                        int colIndex = columnToIndexMapping[colName];
                        GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Wait(5000);
                        msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);
                        clearTextData();
                        Keyboard.SendKeys(dr[TestDataConstants.Col_Multiplier].ToString());

                    }
                }

                if (dr.Table.Columns.Contains(TestDataConstants.COL_CUSIP))
                {

                    if (strikePrice != null && !string.IsNullOrEmpty(dr[TestDataConstants.COL_CUSIP].ToString()))
                    {
                        string colName = TestDataConstants.COL_CUSIP;
                        int colIndex = columnToIndexMapping[colName];
                        GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Wait(5000);
                        msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);
                        clearTextData();
                        Keyboard.SendKeys(dr[TestDataConstants.COL_CUSIP].ToString());

                    }
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_BLOOMBERG))
                {

                    if (bloomberg != null && !string.IsNullOrEmpty(dr[TestDataConstants.COL_BLOOMBERG].ToString()))
                    {
                        string colName = TestDataConstants.COL_BLOOMBERG;
                        int colIndex = columnToIndexMapping[colName];
                        GrdData.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Wait(5000);
                        msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[colIndex].Click(MouseButtons.Left);
                        clearTextData();
                        Keyboard.SendKeys(dr[TestDataConstants.COL_BLOOMBERG].ToString());

                    }
                }



            }
            catch (Exception)
            {
                throw;

            }


        }
        public void clearTextData()
        {
            try
            {
                PressRightArrow(15);
                PressBackspace(15);
            }
            catch (Exception) { throw; }

        }
        static void PressRightArrow(int remainingCount)
        {
            try
            {
                if (remainingCount <= 0)
                    return;
                SendKeys.SendWait("{END}");
                SendKeys.SendWait("{Right}");
                Thread.Sleep(100);

                PressRightArrow(remainingCount - 1);
            }
            catch (Exception) { throw; }
        }
        static void PressBackspace(int remainingCount)
        {
            try
            {
                if (remainingCount <= 0)
                    return;

                SendKeys.SendWait("{Backspace}");
                Thread.Sleep(100);

                PressBackspace(remainingCount - 1);
            }
            catch (Exception) { throw; }
        }
    }
}
