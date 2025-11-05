using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Steps;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    public class OpenSpinOff : CorporateActionUIMap,ITestStep
    {

        /// <summary>
        /// Run the Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int , String> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenCorporateActionsUI();
               // Wait(2000);
                InsertCorporateAction(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.MinimizeWindow(ref FrmCorporateActionNew_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }


        /// <summary>
        /// Insert the Corporate Action Value
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToNAme"></param>
        /// <returns></returns>
        public TestResult InsertCorporateAction(DataSet testData, Dictionary<int, string> sheetIndexToNAme)
        {
            TestResult _res = new TestResult();
            try
            {
                DataTable excelFileData = testData.Tables[sheetIndexToNAme[0]];

                foreach (DataRow dataRow in excelFileData.Rows)
                {
                    // Select the Corporation type from drop down
                   CmbCATypeApply1.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.COL_Corporate_Action].ToString() + KeyboardConstants.ENTERKEY);
                    Clear.Click(MouseButtons.Left);


                    // Select the Accounts from Account Filter
                    Clear.Click(MouseButtons.Left);
                    String accountFilter = TestDataConstants.COL_ACCOUNT_FILTER;
                    if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToNAme[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString()) && !testData.Tables[sheetIndexToNAme[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString().Equals("Select All"))
                    {
                        accountFilter = testData.Tables[sheetIndexToNAme[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString();
                        List<string> accountFilterList = accountFilter.Split(',').ToList();
                        ExtentionMethods.SelectMultipleItemsFromCombo(accountFilterList, MultiSelectDropDownNew1);
                    }
                    else if (testData.Tables[sheetIndexToNAme[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString().Equals("Select All"))
                    {
                        //if Account Filter cell value is Blank or White Space then by default select all in Account filter
                        Dictionary<int, string> allItems = (Dictionary<int, string>)MultiSelectDropDownNew1.InvokeMethod("GetAllItemsInDictionary", null);
                        object[] parameters = new object[2];
                        parameters[0] = allItems;
                        parameters[1] = CheckState.Checked;
                        MultiSelectDropDownNew1.InvokeMethod("SelectUnselectItems", parameters);
                    }
                   
                    // Select the Broker from the drop down list
                    CmbCounterParty.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.COL_Counter_Party].ToString() + KeyboardConstants.ENTERKEY);

                }
            }catch (Exception ex)
                 {
                    _res.IsPassed = false;
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                    throw;
                   }
            return _res ;
        }
    }
}
    

