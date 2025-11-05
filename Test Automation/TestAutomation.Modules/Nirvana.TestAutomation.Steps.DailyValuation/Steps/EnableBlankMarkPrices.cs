using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    public class EnableBlankMarkPrices : BlankMarkPriceUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                /*code to change the key value of the PRana.exe configuration file to enable blank mark prices columns */

                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    String DefaultFilterToShow = dr[TestDataConstants.ISDEFAULTFILTERTOSHOWACCOUNTWISEDATAONDAILYVALUATION].ToString();//TRUE--True
                    String FilteringAccountWiseData = dr[TestDataConstants.ISFILTERINGACCOUNTWISEDATAALLOWEDONDAILYVALUATION].ToString();

                    ConfigModificatorSettings _file = new ConfigModificatorSettings(ApplicationArguments.ClientReleasePath + "\\Prana.exe.config");

                    ConfigUpdater.ChangeValueByKey(TestDataConstants.ISDEFAULTFILTERTOSHOWACCOUNTWISEDATAONDAILYVALUATION, DefaultFilterToShow, _file);
                    ConfigUpdater.ChangeValueByKey(TestDataConstants.ISFILTERINGACCOUNTWISEDATAALLOWEDONDAILYVALUATION, FilteringAccountWiseData, _file);

                }
                return _result;

            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                _result.IsPassed = false;
                return _result;
            }

        }


    }
}
