using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Recon
{
    public class ImportDataOnRecon : ReconUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRecon();
                MaximizeRecon();
                BtnImport.Click(MouseButtons.Left);
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                List<string> colList = new List<string>();
                foreach (DataColumn col in testData.Tables[sheetIndexToName[0]].Columns)
                {
                    colList.Add(col.ColumnName);
                }
                foreach (string colName in colList)
                {
                    if (colName.Equals(TestDataConstants.Col_ImportFile))
                    {
                        Filename2.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[colName].ToString());
                    }
                }
                ButtonOpen.Click(MouseButtons.Left);
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
                MinimizeRecon();
            }
            return _result;
        }
    }
}
