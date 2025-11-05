using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Configuration;
using TestAutomationFX.UI;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class TradePTT : PTTUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            bool MTTpopupcheck = false;
            try
            {

                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                    {
                        if (dataRow.Table.Columns.Contains(TestDataConstants.COL_SYMBOLOGY))
                        {
                            if (dataRow[TestDataConstants.COL_SYMBOLOGY].ToString() != String.Empty && dataRow[TestDataConstants.COL_SYMBOLOGY].ToString().Equals("Bloomberg Symbol"))// Select Symbology to Default
                            {
                                CopyTTGeneralPref();
                                OpenGeneralPreferences();
                                CmbSymbology.Click(MouseButtons.Left);
                                CmbSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_SYMBOLOGY].ToString();
                                BtnSave.DoubleClick(MouseButtons.Left);
                                Wait(5000);
                                BtnClose.DoubleClick(MouseButtons.Left);
                            }
                        }
                    }
                }
                OpenPTT();
                InputParametersPTT(testData, sheetIndexToName);
                Calculate.Click(MouseButtons.Left);
                Calculate.Click(MouseButtons.Left);
                 Wait(2000);
                Trade1.Click(MouseButtons.Left);
                 Wait(5000);
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                if (File.Exists(DefaultSymbologySourceNewFile))
                {
                    RevertTTGenPref();
                }

                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_MTTPOPUP))
                {
                    if (testData.Tables[0].Rows[0][TestDataConstants.COL_MTTPOPUP].ToString().ToUpper().Equals("YES"))
                    {
                        if (ButtonOK3.IsVisible)
                        {
                            Keyboard.SendKeys(KeyboardConstants.SPACE);
                            MTTpopupcheck = true;
                        }
                        //Keyboard.SendKeys(KeyboardConstants.SPACE);
                        //MTTpopupcheck = true;
                    }

                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "TradePTT");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                if (File.Exists(DefaultSymbologySourceNewFile))
                {
                    RevertTTGenPref();
                }
                //handling for popup for live pricing data not available on MTT
                if (MTTpopupcheck == false)
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }

            return _result;
        }
    }
}
