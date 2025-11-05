using System;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class ClearCalculations : RebalancerUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
              //  Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);

                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];

                    ClearButton(row);
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
        ///<summary>
        ///Click clear button
        ///</summary>

        private void ClearButton(DataRow dr)
        {
            try
            {
                string value = string.Empty;

                if (!dr[TestDataConstants.COL_ClearCalculations].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_ClearCalculations].ToString();

                    if (value.Equals("Yes"))
                    {
                        ClearCalculations();
                    }
                    if (value.Equals("No"))
                    {
                        ClearCalculation1.Click(MouseButtons.Left);
                        ButtonNo2.Click(MouseButtons.Left);
                    }
                }
              
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

    }
}
