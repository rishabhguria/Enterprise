using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;
using System.Threading;
namespace Nirvana.TestAutomation.Steps.Import
{
    public class ActionOnImport : ImportUIMap, ITestStep
    {
         public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataSet tempDS = new DataSet();
                DataTable dt = testData.Tables[sheetIndexToName[0]].Copy();
                ActionBeforeImport(dt);
                tempDS.Tables.Add(dt);
                try
                {
                   ImportProcess(tempDS, sheetIndexToName);
                   TradesUpload();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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

         public void ActionBeforeImport(DataTable Importdt)
         {
             try
             {
                 List<DataRow> rowsToRemove = new List<DataRow>();
                 if (Importdt.Columns.Contains("File Path"))
                 {
                     Importdt.Columns["File Path"].ColumnName = "Select File";
                 }
                 foreach (DataRow row in Importdt.Rows)
                 {
                     bool hasFilePath = !string.IsNullOrWhiteSpace(row["Select File"].ToString());
                    // bool isSelectRecordOn = string.Equals(row["Select Record"].ToString(), "ToggleState_On", StringComparison.OrdinalIgnoreCase);

                     if (!hasFilePath )
                     {
                         rowsToRemove.Add(row);
                     }
                 }

                 foreach (var row in rowsToRemove)
                 {
                     Importdt.Rows.Remove(row);
                 }
                 foreach (DataRow row in Importdt.Rows)
                 {
                     if (row["Account"] != null && row["Account"].ToString().StartsWith("-", StringComparison.OrdinalIgnoreCase))
                     {
                         row["Account"] = DBNull.Value;
                     }
                 }
                 Console.WriteLine("Rows after filtering: " + Importdt.Rows.Count);




             }
             catch (Exception ex)
             {
                 throw new Exception("ActionOnImport failed :" + ex.Message);

             }
         }
         public TestResult ImportProcess(DataSet testData, Dictionary<int, string> sheetIndexToName)
         {
             TestResult _result = new TestResult();
             try
             {
                 string colImportType = string.Empty;
                 if (!PranaMain.IsVisible)
                 {
                     ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                 }
                 Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_IMPORT"]);
                 ExtentionMethods.WaitForVisible(ref ImportData1, 15);
                 KeyboardUtilities.MaximizeWindow(ref ImportData_UltraFormManager_Dock_Area_Top);
                 GridRunUpload.Click(MouseButtons.Left);

                 DataTable dtImportGrid = CSVHelper.CSVAsDataTable(this.GridRunUpload.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
              
                 var gridMssaObject = GridRunUpload.MsaaObject;
                 Dictionary<string, int> colToIndexMapping = new Dictionary<string, int>();

                 int size = gridMssaObject.FindDescendantByName("Column Headers", 8000).ChildCount;
                 var colMsaaObject = gridMssaObject.FindDescendantByName("Column Headers", 8000);
                 var rowMsaaObject = gridMssaObject.FindDescendantByName("RunUploadList", 8000);
                 for (int i = 0; i < size; i++)
                 {
                     colToIndexMapping.Add(colMsaaObject.CachedChildren[i].Name, i);
                     Console.WriteLine("UI ColumnNames" + colMsaaObject.CachedChildren[i].Name);
                 }
                 string tpName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_UPLOAD_THIRDPARTY].ToString();
                 if (!string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_Import_Type].ToString()))
                 {
                     colImportType = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_Import_Type].ToString();

                 }
                 string selectStatement = String.IsNullOrEmpty(colImportType) ? String.Format(@"[Upload ThirdParty]='{0}'", tpName) : String.Format(@"[Upload ThirdParty]='{0}' AND [Import Type]='{1}'", tpName, colImportType);
                 DataRow[] foundRow = dtImportGrid.Select(selectStatement);
                 int index = dtImportGrid.Rows.IndexOf(foundRow[0]);
                 Console.WriteLine("Row found at:" + index + 1);
                 GridRunUpload.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                 List<string> colList = new List<string>();
                 foreach (DataColumn col in testData.Tables[sheetIndexToName[0]].Columns)
                 {
                     colList.Add(col.ColumnName);
                 }
                 DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];

                 gridMssaObject.FindDescendantByName("RunUploadList", 8000).CachedChildren[index + 1].FindDescendantByName(TestDataConstants.COL_SELECT_RECORD, 8000).Click(MouseButtons.Left);



                 
                 foreach (string colNameExcel in colList)
                 {
                     Console.WriteLine("Checking:" +colNameExcel);
                     if (string.Equals(colNameExcel, TestDataConstants.COL_DATE, StringComparison.OrdinalIgnoreCase))
                     {
                         bool isDateSelected = true;

                         if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_SELECT_DATE].ToString()))
                         {
                             if (string.Equals("ToggleState_Off", dr[TestDataConstants.COL_SELECT_DATE].ToString(), StringComparison.OrdinalIgnoreCase))
                             {
                                 isDateSelected = false;

                             }
                             else if (string.Equals("ToggleState_On", dr[TestDataConstants.COL_SELECT_DATE].ToString(), StringComparison.OrdinalIgnoreCase))
                             {
                                 isDateSelected = true;

                             }
                         }
                         if (isDateSelected )
                         {
                             string tempDate = DataUtilities.DateHandler(dr[colNameExcel].ToString());
                             string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                             Thread.Sleep(3000);
                             GridRunUpload.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, colToIndexMapping[colNameExcel]);

                             rowMsaaObject.CachedChildren[index + 1].FindDescendantByName(colNameExcel, 8000).Click(MouseButtons.Left);
                             Keyboard.SendKeys(date);
                         }
                     }
                     else
                     {
                         try
                         {
                         if (string.IsNullOrEmpty(dr[colNameExcel].ToString()))
                         {
                             continue;
                         }
                         if (colNameExcel.Equals(TestDataConstants.COL_UPLOAD_THIRDPARTY))
                             continue;
                         
                         
                         GridRunUpload.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, colToIndexMapping[colNameExcel]);
                         rowMsaaObject.CachedChildren[index + 1].FindDescendantByName(colNameExcel, 8000).Click(MouseButtons.Left);

                         // gridMssaObject.CachedChildren[1].CachedChildren[index + 1].CachedChildren[colToIndexMapping[colNameExcel]].Click();
                         if (colNameExcel.Equals(TestDataConstants.COL_SELECT_FILE))
                         {
                             TextBoxFilename.Click(MouseButtons.Left);
                         }
                         Keyboard.SendKeys(dr[colNameExcel].ToString());
                         Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                         }
                          catch (Exception ex)
                         {
                             Console.WriteLine("Import Process may have not completed perfectly "+ex.Message);
                         }
                     }
                 }
             
             }
             catch (Exception ex)
             {
                 Console.WriteLine("Import Process may have not completed perfectly "+ex.Message);
             }
             return _result;
         }
    }
}
