using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.BussinessObjects;
using System.Data;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Steps.Admin.Scripts;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.ComponentModel;
using System.Diagnostics;


namespace Nirvana.TestAutomation.Steps.Admin
{
    public class AccountPermission : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //DataRow dr = testData.Tables[0].Rows[0];
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    String UserID = string.Empty;
                    String Permission = dr[TestDataConstants.Col_Allocate_Unallocate].ToString();
                    String[] Accounts = dr[TestDataConstants.COL_ACCOUNTNAME].ToString().Split(',');
                    if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_ACCOUNTNAME].ToString()))
                    {
                        if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_ACCOUNTNAME].ToString()))
                        {
                            UserID = dr[TestDataConstants.USERID].ToString();
                        }
                        else
                        {
                            UserID = "17";
                        }
                        SQLQueriesConstants.AccountPermission(Accounts, UserID, Permission);
                    }
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
    }
}
