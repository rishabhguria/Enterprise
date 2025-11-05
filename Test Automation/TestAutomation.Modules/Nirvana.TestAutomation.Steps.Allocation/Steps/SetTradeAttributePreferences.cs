using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Steps.Allocation.Scripts;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class SetTradeAttributePreferences : PreferencesUIMap, ITestStep
    {
       /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                string err_msg = SetAttributePreferences(testData, sheetIndexToName);
                if (!string.IsNullOrEmpty(err_msg))
                {
                    _res.ErrorMessage = err_msg;
                    _res.IsPassed = false;
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetTradeAttributePreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }
        /// <summary>
        /// Opens the Trade Attribute Preferences.
        /// </summary>
        private void OpenAttributePreferences()
        {
            try
            {
                Preferences.Click(MouseButtons.Left);
                AttributeRenaming.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Sets the Trade Attribute Preferences.
        /// </summary>
        private string SetAttributePreferences(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string err_msg = string.Empty;
            try
            {
                OpenAttributePreferences();
                SetAttributeName(testData, sheetIndexToName);
                SetKeepRecord(testData, sheetIndexToName);
                SetDefaultValues(testData, sheetIndexToName);
                Save.Click(MouseButtons.Left);
                Close.Click(MouseButtons.Left);
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
        }

        /// <summary>
        /// Sets the Keep Record Value.
        /// </summary>
        private string SetKeepRecord(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string err_msg = string.Empty;
            try 
            {
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty)
                    {
                        if (CheckBox.IsChecked)
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute1" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox.Click(MouseButtons.Left);
                            }   
                        }
                        else
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute1" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox.Click(MouseButtons.Left);
                            }

                        }

                        if (CheckBox1.IsChecked)
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute2" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord1.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox1.Click(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute2" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord1.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox1.Click(MouseButtons.Left);
                            }
 
                        }

                        if (CheckBox2.IsChecked)
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute3" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord2.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox2.Click(MouseButtons.Left);
                            }
                        }

                        else
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute3" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord2.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox2.Click(MouseButtons.Left);
                            }
                        }

                        if (CheckBox3.IsChecked)
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute4" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord3.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox3.Click(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute4" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord3.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox3.Click(MouseButtons.Left);
                            }
                        }
                        if (CheckBox4.IsChecked)
                        {

                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute5" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord4.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox4.Click(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute5" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord4.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox4.Click(MouseButtons.Left);
                            }
 
                        }

                        if (CheckBox5.IsChecked)
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute6" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord5.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox5.Click(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            if (dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute6" && dr[TestDataConstants.Col_KeepRecord].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                KeepRecord5.Click(MouseButtons.Left);
                                Wait(100);
                                CheckBox5.Click(MouseButtons.Left);
                            }
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
 
        }

        /// <summary>
        /// Sets the Default Value.
        /// </summary>
        private string SetDefaultValues(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string err_msg = string.Empty;
            try
            {
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute1" && dr[TestDataConstants.Col_DefaultValues].ToString() != String.Empty)
                    {
                        DefaultValues.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_DefaultValues].ToString());
                    }

                    if(dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute2" && dr[TestDataConstants.Col_DefaultValues].ToString() != String.Empty)
                    {
                        DefaultValues1.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_DefaultValues].ToString());
                    }

                    if(dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute3" && dr[TestDataConstants.Col_DefaultValues].ToString() != String.Empty)
                    {
                        DefaultValues2.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_DefaultValues].ToString());
                    }
                    if (dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute4" && dr[TestDataConstants.Col_DefaultValues].ToString() != String.Empty)
                    {
                        DefaultValues3.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_DefaultValues].ToString());
                    }

                    if (dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute5" && dr[TestDataConstants.Col_DefaultValues].ToString() != String.Empty)
                    {
                        DefaultValues4.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_DefaultValues].ToString());
                    }

                    if (dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute6" && dr[TestDataConstants.Col_DefaultValues].ToString() != String.Empty)
                    {
                        DefaultValues5.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_DefaultValues].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
        }

          /// <summary>
        /// Sets the Attribute Name.
        /// </summary>
        private string SetAttributeName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string err_msg = string.Empty;
            try
            {
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if(dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute1" && dr[TestDataConstants.Col_AttributeName].ToString() != String.Empty)
                    {
                        TradeAttribute12.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_AttributeName].ToString());
                    }

                    if(dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute2" && dr[TestDataConstants.Col_AttributeName].ToString() != String.Empty)
                    {
                        TradeAttribute22.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_AttributeName].ToString());
                    }

                    if(dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute3" && dr[TestDataConstants.Col_AttributeName].ToString() != String.Empty)
                    {
                        TradeAttribute32.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_AttributeName].ToString());
                    }

                    if(dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute4" && dr[TestDataConstants.Col_AttributeName].ToString() != String.Empty)
                    {
                        TradeAttribute42.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_AttributeName].ToString());
                    }

                    if(dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute5" && dr[TestDataConstants.Col_AttributeName].ToString() != String.Empty)
                    {
                        TradeAttribute52.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_AttributeName].ToString());
                    }

                    if (dr[TestDataConstants.Col_AttributeValue].ToString() != String.Empty && dr[TestDataConstants.Col_AttributeValue].ToString() == "TradeAttribute6" && dr[TestDataConstants.Col_AttributeName].ToString() != String.Empty)
                    {
                        TradeAttribute62.Click(MouseButtons.Left);
                        Wait(100);
                        Keyboard.SendKeys(dr[TestDataConstants.Col_AttributeName].ToString());
                    }
                }
 
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
        }
        protected override 
            void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
