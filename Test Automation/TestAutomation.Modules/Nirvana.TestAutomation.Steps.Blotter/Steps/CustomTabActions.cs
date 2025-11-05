using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Steps.Blotter;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class CustomTabActions : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                MaximizeBlotter();
                try
                {
                    UIAutomationElement accountComboItem = new UIAutomationElement();
                    accountComboItem.AutomationName = testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString();
                    accountComboItem.Comment = null;
                    accountComboItem.ItemType = "";
                    accountComboItem.MatchedIndex = 0;
                    accountComboItem.Name = testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString();
                    accountComboItem.Parent = this.BlotterTabControl1;
                    accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                    accountComboItem.UseCoordinatesOnClick = true;
                    accountComboItem.Click(MouseButtons.Left);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    UIAutomationElement accountComboItem = new UIAutomationElement();
                    accountComboItem.AutomationName = testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString();
                    accountComboItem.Comment = null;
                    accountComboItem.ItemType = "";
                    accountComboItem.MatchedIndex = 0;
                    accountComboItem.Name = testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString();
                    accountComboItem.Parent = this.BlotterTabControl1;
                    accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                    accountComboItem.UseCoordinatesOnClick = true;
                    accountComboItem.Click(MouseButtons.Left);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                DataTable dt = testData.Tables[0].Copy();
                dt.Columns.Remove("Action");
                dt.Columns.Remove("TabName");
                dt.Columns.Remove("ActionOnWhichGrid");

                if (testData.Tables[0].Rows[0]["ActionOnWhichGrid"].ToString().ToLower().Contains("suborder")) 
                {
                    var msaaObj = DgBlotter2.MsaaObject;
                    DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dt.Rows[0]);
                    int index = dtBlotter.Rows.IndexOf(dtRow);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, testData.Tables[0].Rows[0].Table, new List<string>(), 0.01);
                            throw new Exception("Trade not found during SelectTradeOnOrderGrid step. [Symbol= " + dt.Rows[0]["Symbol"] + "], Quantity = [" + dt.Rows[0]["Target Qty"] + "] Side = [" + dt.Rows[0]["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                }
                else if (testData.Tables[0].Rows[0]["ActionOnWhichGrid"].ToString().ToLower().Contains("order"))
                {
                    var msaaObj = DgBlotter1.MsaaObject;
                    DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dt.Rows[0]);
                    int index = dtBlotter.Rows.IndexOf(dtRow);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, testData.Tables[0].Rows[0].Table, new List<string>(), 0.01);
                            throw new Exception("Trade not found during SelectTradeOnOrderGrid step. [Symbol= " + dt.Rows[0]["Symbol"] + "], Quantity = [" + dt.Rows[0]["Target Qty"] + "] Side = [" + dt.Rows[0]["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                }
                else if (testData.Tables[0].Rows[0]["ActionOnWhichGrid"].ToString().ToLower().Contains("working")) 
                {
                    var msaaObj = DgBlotter.MsaaObject;
                    DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dt.Rows[0]);
                    int index = dtBlotter.Rows.IndexOf(dtRow);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, dt.Rows[0].Table, new List<string>(), 0.01);
                            throw new Exception("Trade not found during SelectTradeOnOrderGrid step. [Symbol= " + dt.Rows[0]["Symbol"] + "], Quantity = [" + dt.Rows[0]["Target Qty"] + "] Side = [" + dt.Rows[0]["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);                  
                }
                Wait(1500);
                pickFromMenuItem(PopupMenuContext, testData.Tables[0].Rows[0]["Action"].ToString());
                if (Warning2.IsVisible)
                    ButtonOK2.Click(MouseButtons.Left);
                if (NirvanaBlotter.IsVisible)
                    ButtonYes.Click(MouseButtons.Left);
                if (Warning.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);

            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}
