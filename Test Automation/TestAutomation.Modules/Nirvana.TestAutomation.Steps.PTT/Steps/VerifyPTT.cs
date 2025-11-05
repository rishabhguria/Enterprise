using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class VerifyPTT : PTTUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenPTT();
               // Wait(1000);
                string sheetName = sheetIndexToName[0];
                List < String > errors =  VerifyPTTData(testData, sheetName);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage=String.Join("\n\r", errors);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyPTT");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;

            }
            finally
            {
                PercentTradingTool.Click(MouseButtons.Left);
                PercentTradingTool.Click(MouseButtons.Right);
                KeyboardUtilities.CloseWindow(ref PercentTradingTool);
                
            }

            return _result;
        }
       
        /// <summary>
        /// Verifies the PST data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="TickerID">The ticker id.</param>
        /// <returns></returns>
        private List<string> VerifyPTTData(DataSet testData, String sheetName)
        {
            try
            {
                DataTable dtPstData = ExportPSTData();
                dtPstData = DataUtilities.RemovePrecision(dtPstData, 2);
                DataTable excelData = new DataTable();
               
                string verifyRoundLots = string.Empty;
                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_VERIFYROUNDLOTS))
                {
                    excelData = DataUtilities.RemoveColumnsAndRows(TestDataConstants.COL_VERIFYROUNDLOTS, testData.Tables[sheetName]);
                    verifyRoundLots = testData.Tables[sheetName].Rows[0][TestDataConstants.COL_VERIFYROUNDLOTS].ToString();
                }
                else
                {
                    excelData = testData.Tables[sheetName];
                }
               

                if (!string.IsNullOrEmpty(verifyRoundLots))
                {
                    string defaultValueOnRoundLotsCombo = "No";

                    if (XamComboEditor7.IsEnabled)
                    {
                        defaultValueOnRoundLotsCombo = XamComboEditor7.Value.ToString();
                    }
                    else {
                        defaultValueOnRoundLotsCombo = XamComboEditor8.Value.ToString();
                    }
                    
                    if (dtPstData.Columns.Contains(TestDataConstants.COL_ROUND_LOTS))
                    {
                        if (defaultValueOnRoundLotsCombo.Equals("YES", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("PTT ROUND LOTS COMBO CONTAINS => " + defaultValueOnRoundLotsCombo + " VALUE AND PTT GRID ALSO CONTAINS ROUNDLOTS COLUMN");
                        }
                        else
                            throw new Exception("PTT ROUND LOTS COMBO CONTAINS =>" + defaultValueOnRoundLotsCombo + "BUT PTT GRID  CONTAINS ROUNDLOTS COLUMN");
                        
                    }
                    else
                    {
                        if (defaultValueOnRoundLotsCombo.Equals("No", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("PTT ROUND LOTS COMBO CONTAINS => " + defaultValueOnRoundLotsCombo + " VALUE AND PTT GRID DOES NOT CONTAINS Roundlists");
                        }
                        else
                        {
                            throw new Exception("PTT ROUND LOTS COMBO CONTAINS =>" + defaultValueOnRoundLotsCombo + "But PTT GRID DOES NOT CONTAINS ROUNDLOTS COLUMN");
                        }
                    }
                }

                List<String> columns = new List<String>();                
                           
                List < String > errors = Recon.RunRecon(dtPstData, excelData, columns);
                return errors;                
            }
            catch (Exception)
            {

                throw;
            }
        }        
    }
}
