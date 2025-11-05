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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Admin 
{
    public class EnableDisableModule : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable dt = testData.Tables[sheetIndexToName[0]];
                foreach (DataRow dr in dt.Rows)
                {
                    string isEnableModule = dr[TestDataConstants.COL_isEnableModule].ToString();
                    string moduleName = dr[TestDataConstants.COL_moduleName].ToString();
                    if (moduleName.ToUpper().Equals("SHORT LOCATE") && ApplicationArguments.runType.ToLower() == "samsara")
                    {
                        DataUtilities.UpdateJson(ConfigurationManager.AppSettings["SamsaraDirectory"] + "\\public\\config\\config.json", "Always_PopUp_Short_Locate", true);
                    }
                    string user = "jpearce";
                    if (dr.Table.Columns.Contains(TestDataConstants.COL_USERNAME) && (dr[TestDataConstants.COL_USERNAME].ToString() != String.Empty))
                        {
                            user = dr[TestDataConstants.COL_USERNAME].ToString();
                        }
                    if (isEnableModule.Equals("True"))
                    {
                        SQLQueriesConstants.EnableModule(moduleName, user);
                    }
                    else
                        SQLQueriesConstants.DisableModule(moduleName, user);
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.isEnableModule);
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
