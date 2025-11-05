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
    public class SelectFromSMGridUI : SymbolLookupUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenSymbolLookup();

                GrdData.WaitForVisible();
                GrdData.Click(MouseButtons.Left);


                if (testData.Tables[0] != null)
                {
                    InputEnter(testData.Tables[0]);
                }
                else if (testData.Tables[0] == null)
                {
                    Console.WriteLine(" UI DATA IS NULL");
                }



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
                MinimzieSybolLookup();
               // Wait(3000);
                
            }
            return _result;
        }
        public void InputEnter( DataTable exceldata)
        {

            try
            {
                //Wait(5000);
               
               
                DataTable uiData = CSVHelper.CSVAsDataTable(this.GrdData.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                foreach (DataRow dr in exceldata.Rows)
                {
                    var msaaObj = GrdData.MsaaObject;
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(uiData), dr);
                    int index = uiData.Rows.IndexOf(dtRow);
                    GrdData.InvokeMethod("ScrollToRow", index);
                   // Wait(5000);


                    msaaObj.CachedChildren[1].CachedChildren[index + 1].CachedChildren[0].Click(MouseButtons.Left);
                    //msaaObj.CachedChildren[0].CachedChildren[0].Click(MouseButtons.Left);
                }
            }
            catch (Exception)
            {
                throw;

            }

 
        }


    }
}
