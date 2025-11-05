using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class VerifyLowerStripBlotterUI : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                var msaaObj = StatusStrip11.MsaaObject;
                string data = msaaObj.CachedChildren[2].Name.ToString();
                int rowcount = testData.Tables[0].Rows.Count;
                if (rowcount > 0)
                {
                    DataRow dr = testData.Tables[0].Rows[0];
                    List<String> errors = VerifyData(dr, data);

                    if (errors.Count > 0)
                        _res.ErrorMessage = String.Join("\n\r", errors);
                }

                else if (rowcount == 0 && data == "")
                {
                    Console.WriteLine("Result Passed");
                }
            }

            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseBlotter();
            }
            return _res;
        }

        public List<String> VerifyData(DataRow dr, string dt)
        {
            List<String> errors = new List<String>();
            if (dr[TestDataConstants.COUNT_ACCOUNT_SUM_TARGETQTY].ToString().Equals(dt))
            {
                Console.WriteLine("Result Passed");
            }

            else
            {
                errors.Add(dr[TestDataConstants.COUNT_ACCOUNT_SUM_TARGETQTY].ToString());
            }
            return errors;
        }
    }
}
