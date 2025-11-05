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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.UIAutomation;
using TestAutomationFX.Core;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Unknown
{
    class CheckModStatus : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                DataTable dt = testData.Tables[0];
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CommonMappings"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                foreach (DataRow dr in dt.Rows)
                {
                    string isEnableModule = dr[TestDataConstants.COL_isEnableModule].ToString();
                    string moduleName = dr[TestDataConstants.COL_moduleName].ToString();
                    string OpenModuleConfigName = dr[TestDataConstants.COL_OpenModuleConfigName].ToString();
                    if (string.IsNullOrEmpty(OpenModuleConfigName))
                    {
                        throw new Exception("OpenModuleConfigName cannot be null");
                    }
                    object value = dr[TestDataConstants.COL_USER];
                    string user = (value != null && value.ToString().Trim() != "") ? value.ToString() : "jpearce";
                    string checkModuleStatusQuery = @"
                                                    DECLARE @User_ID int 
                                                    SET @User_ID = (SELECT UserID FROM T_CompanyUser WHERE ShortName = '" + user + @"')

                                                    IF EXISTS (
                                                        SELECT 1 FROM T_CompanyUserModule CUM 
                                                        INNER JOIN T_CompanyModule CM ON CUM.CompanyModuleID = CM.CompanyModuleID 
                                                        INNER JOIN T_Module M ON CM.ModuleID = M.ModuleID 
                                                        WHERE UPPER(M.ModuleName) = UPPER('" + moduleName + @"') 
                                                        AND CUM.CompanyUserID = @User_ID
                                                    ) 
                                                        SELECT 'Enabled' AS ModuleStatus 
                                                    ELSE 
                                                        SELECT 'Disabled' AS ModuleStatus";


                    Console.WriteLine("Query: " + checkModuleStatusQuery);


                    DataTable dtable = SqlUtilities.GetDataFromQuery(checkModuleStatusQuery, "Client");
                    if (dtable == null)
                    {
                        throw new Exception("CheckModStatus failed");
                    }

                    string defaultModuleStatus = "";
                    
                    if (dtable.Columns.Contains("ModuleStatus"))
                    {
                        defaultModuleStatus = dtable.Rows[0]["ModuleStatus"].ToString();
                    }

                    if (string.Equals(isEnableModule, defaultModuleStatus, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("CheckModuleStatus succeeded Excel TestData and SQL result  matched :" +isEnableModule+" "+defaultModuleStatus);
                        try
                        {

                            try
                            {
                                Keyboard.SendKeys(ConfigurationManager.AppSettings[OpenModuleConfigName]);
                            }
                            catch
                            {
                                throw new Exception("OpenModuleConfigName value is incorrect");
                            }

                            bool result = UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary[moduleName].AutomationUniqueValue);
                            if (result)
                            {
                                if (!string.Equals(defaultModuleStatus, "Enabled", StringComparison.OrdinalIgnoreCase))
                                {
                                    throw new Exception("CheckModuleStatus Excel TestData and UI result not matched");
                                }
                            }
                            else
                            {
                                if (!string.Equals(defaultModuleStatus, "Disabled", StringComparison.OrdinalIgnoreCase))
                                {
                                    throw new Exception("CheckModuleStatus Excel TestData and UI result not matched");
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            throw new Exception("CheckModuleStatus failed ");
                        }   
                    }
                    else
                    {
                       throw new Exception("CheckModuleStatus failed Excel TestData and SQL result not matched :" +isEnableModule+" "+defaultModuleStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}
