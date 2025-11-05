using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Steps.PricingServer
{
    public partial class UpdatePricingConfig : PricingServerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            /// <summary>
            /// Updating config files
            /// </summary>
            /// <param name="testData">gets input of key and value pair to update</param>
            /// <param name="sheetIndexToName">sheet name to use from excel for this step</param>
            /// <returns></returns>
            TestResult _result = new TestResult();
            try
            {
                ConfigModificatorSettings _file = new ConfigModificatorSettings(ApplicationArguments.PricingReleasePath + "\\Prana.PricingService2Host.exe.config");
                DataTable data = testData.Tables["UpdatePricingConfig"];
                foreach (DataRow row in data.Rows)
                {
                    ConfigUpdater.ChangeValueByKey(row["AppSetting_Key"].ToString(), row["AppSetting_Value"].ToString(), _file);
                }
                return _result;
            }
            catch (Exception ex)
            {
                _result.IsPassed=false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }

        }
    }
}
