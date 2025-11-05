using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;
using Nirvana.TestAutomation.Interfaces.Enums;


namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class VerifyGridData:RebalancerUIMap,ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                //Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);

                //Maximize Rebalancer
               // MaximizeRebalancer(RebalanceTab);
            
               //Export Data To Excel
               DataTable dTable=ExportRebalancerGridData();
             

               List<string>errors=VerifyGrid(dTable,testData.Tables[0]);
                if (errors.Count > 0)
                    _result.ErrorMessage = String.Join("\n\r", errors);


                //Minimize Rebalancer
                //KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                //Close window
                //KeyboardUtilities.CloseWindow(ref RebalanceTab);
    
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyGridData");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                //Minimize Rebalancer
                KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
            }
            return _result;
        }

      
        /// <summary>
        /// Verify Grid data
        /// </summary>
        /// <param name="dataTable"></param>
        private List<string> VerifyGrid(DataTable dataTable1, DataTable dataTable2)
        {
            List<string> errors = new List<string>();
            List<string> columns = new List<string>();
            try
            {
                errors = Recon.RunRecon(dataTable1, dataTable2, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
        }
    }
}
