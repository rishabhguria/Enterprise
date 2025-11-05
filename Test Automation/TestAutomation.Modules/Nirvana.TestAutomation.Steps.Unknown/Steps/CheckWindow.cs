using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Configuration;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.Unknown
{
    class CheckWindow : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                if (testData.Tables[0] == null || testData.Tables[0].Rows.Count <= 0)
                {
                    return _res ;
                }
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["PranaMain"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                string error = string.Empty;
            
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(dr["ModuleWindowID"].ToString()) )
                    {
                        string moduleName =dr["ModuleWindowID"].ToString();
                        string testingType = !string.IsNullOrEmpty(dr["TestType"].ToString()) ? dr["TestType"].ToString().ToUpper() :"POSITIVE";

                        bool result = UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary[moduleName].AutomationUniqueValue);

                        if (result)
                        {
                            if (string.Equals(testingType,"NEGATIVE",StringComparison.OrdinalIgnoreCase))
                            {
                                error = "CheckWindow Step Failed as Window is Visible";
                            }
                        }
                        else
                        {
                            if (string.Equals(testingType, "POSITIVE", StringComparison.OrdinalIgnoreCase))
                            {
                                error = "CheckWindow Step Failed as Window is not Visible";
                            }
                        }



                        if (!string.IsNullOrEmpty(error))
                        {
                            throw new Exception(error);
                        }

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

