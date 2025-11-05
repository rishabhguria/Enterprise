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


namespace Nirvana.TestAutomation.Steps.Import
{
    public class Import : ImportUIMap, ITestStep
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
                string colImportType = string.Empty;
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                //Shortcut to open import module(CTRL + SHIFT + I)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_IMPORT"]);
               // Wait(5000);
                ExtentionMethods.WaitForVisible(ref ImportData1, 15);
                //DataManagement.Click(MouseButtons.Left);
                //ImportData.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref ImportData_UltraFormManager_Dock_Area_Top);
                //Wait(200);
                GridRunUpload.Click(MouseButtons.Left);

                DataTable dtImportGrid =CSVHelper.CSVAsDataTable(this.GridRunUpload.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                //DataTable dtImportGrid = CSVHelper.CSVAsDataTable(this.GridRunUpload.Properties[ExcelStructureConstants.COL_DESCRIPTION].ToString());
                var gridMssaObject = GridRunUpload.MsaaObject;
                Dictionary<string, int> colToIndexMapping = new Dictionary<string, int>();

                int size =gridMssaObject.FindDescendantByName("Column Headers", 8000).ChildCount;
                var colMsaaObject = gridMssaObject.FindDescendantByName("Column Headers", 8000);
                var rowMsaaObject = gridMssaObject.FindDescendantByName("RunUploadList", 8000);
                for (int i = 0; i < size; i++)
                {
                    colToIndexMapping.Add(colMsaaObject.CachedChildren[i].Name, i);
                    Console.WriteLine("UI ColumnNames" +colMsaaObject.CachedChildren[i].Name);
                }
                string tpName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_UPLOAD_THIRDPARTY].ToString();
                if (!string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_Import_Type].ToString()))
                {
                    colImportType = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_Import_Type].ToString();
 
                }
                string selectStatement = String.IsNullOrEmpty(colImportType) ? String.Format(@"[Upload ThirdParty]='{0}'", tpName) : String.Format(@"[Upload ThirdParty]='{0}' AND [Import Type]='{1}'", tpName, colImportType);
                DataRow[] foundRow =  dtImportGrid.Select(selectStatement);
                int index = dtImportGrid.Rows.IndexOf(foundRow[0]);
                Console.WriteLine("Row found at:"+index+1);
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
                    if (colNameExcel.Equals(TestDataConstants.COL_DATE))
                    {
                        bool isDateSelected = true;

                        if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_SELECT_DATE].ToString()))
                            isDateSelected = bool.Parse(dr[TestDataConstants.COL_SELECT_DATE].ToString());

                        if (isDateSelected)
                        {
                            GridRunUpload.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, colToIndexMapping[colNameExcel]);

                            rowMsaaObject.CachedChildren[index + 1].FindDescendantByName(colNameExcel, 8000).Click(MouseButtons.Left);
                          //  gridMssaObject.CachedChildren[1].CachedChildren[index + 1].CachedChildren[colToIndexMapping[colNameExcel] - 1].Click(MouseButtons.Left);
                           // gridMssaObject.CachedChildren[1].CachedChildren[index + 1].CachedChildren[colToIndexMapping[colNameExcel]].Click(MouseButtons.Left);
                            Keyboard.SendKeys(String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dr[colNameExcel].ToString())));
                        }
                    }
                    else
                    {
                        if (colNameExcel.Equals(TestDataConstants.COL_UPLOAD_THIRDPARTY))
                            continue;
                        if (colNameExcel.Equals(TestDataConstants.COL_SELECT_DATE) && !bool.Parse(dr[TestDataConstants.COL_SELECT_DATE].ToString()))
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
                }
                TradesUpload();
                /*bool isPopupVisible = false;
                if (NirvanaWarning.IsVisible)
                {
                    ButtonNo.Click(MouseButtons.Left);
                    isPopupVisible = true;
                }
                if (ImportData2.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                }
                if (isPopupVisible)
                {
                   TradesUpload();
                }
                Wait(5000);
                GridRunUpload.Click(MouseButtons.Left);*/
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
          /*  finally
            {
                KeyboardUtilities.CloseWindow(ref ImportData_UltraFormManager_Dock_Area_Top);
            }*/
            return _result;
        }
        private void TradesUpload()
        {
            try
            {
               // BtnUpload.Click(MouseButtons.Left);
                //ColumnHeader.Click(MouseButtons.Left);
                //btnContinue.Click(MouseButtons.Left);
                bool isWindowVisible = ExtentionMethods.ClickWindowAndWaitForExpectedWindow(ref BtnUpload, ref UltraPanel1ClientArea3, TimeSpan.FromSeconds(6));
                    //UltraPanel1ClientArea3

                Console.WriteLine(ExtentionMethods.GetActiveWindowTitle());
                
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}