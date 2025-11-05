using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
   public class AddFillsSubOrder : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();

            try
            {
                OpenBlotter();
                if (testData != null)
                {
                    DataTable subset = testData.Tables[sheetIndexToName[0]];
                    List<String> columns = new List<String>();
                    try
                    {
                        string StepName = "AddFillsSubOrder";
                        DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                        Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                        SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }
                    string tableName = sheetIndexToName[0];
                    testData.Tables.Remove(tableName);
                    DataTable updatedTable = subset.Copy();
                    updatedTable.TableName = tableName;
                    testData.Tables.Add(updatedTable);
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddFillsSubOrder");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return _res;
        }

        private string InputEnter(DataRow dr)
        {
            string errorMessage = string.Empty;
            try
            {
                //BlotterMain.BringToFront();
                var msaaObj = DgBlotter2.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(dtBlotter, dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Trade not found during AddFillsSubOrder step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Quantity"] + "] Side = [" + dr["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                DgBlotter2.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                bool isClicked = false;
                try
                    {
                        isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Add_Modify_Fills);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                if (isClicked == false)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                    if (AddDivideModifyFills.IsVisible)
                    {
                        ClickAddDivideModifyFills();
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", AddDivideModifyFills.MsaaName);
                    }
                }
                  
            }
            catch (Exception ex)
            {
				bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
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
