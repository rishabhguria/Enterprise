using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
namespace Nirvana.TestAutomation.Steps.QualityChecker
{
    class CheckReleaseQuality : QualityCheckerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenQualityChecker();
                SelectAllDetectionScript();
                string usefromdate,fromdate,todate;
                if (testData != null)
                {
                    DataRow dr = testData.Tables[0].Rows[0];
                    usefromdate = dr[0].ToString();
                    fromdate = dr[1].ToString();
                    todate = dr[2].ToString();
                    if (usefromdate == "Yes")
                    {
                        DatePickerCheckedorNot(); 
                        FromDate(fromdate);
                    }
                    ToDate(todate);
                    Diagnose();
                    Wait(10000);
                    _res.IsPassed=DetectError();
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeWindow();
            }
            return _res;
        }

        /// <summary>
        /// Open Quality Checker Window
        /// </summary>
        /// <param name="disposing"></param>
        private void OpenQualityChecker()
        {
            try
            {
                Tools.Click(MouseButtons.Left);
                QualityChecker.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Check or uncheck the datepicker
        /// </summary>
        /// <param name="disposing"></param>
        private void DatePickerCheckedorNot()
        {
            try
            {
                if (DatePickerEnabler.IsChecked == false)
                    DatePickerEnabler.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// select all the given scripts
        /// </summary>
        /// <param name="disposing"></param>
        private void SelectAllDetectionScript()
        {
            try
            {
                if (DetectionScripts.IsChecked == false)
                {
                    DetectionScripts.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[SPACE]");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Check From_date
        /// </summary>
        /// <param name="disposing"></param>
        private void FromDate(string fromdate)
        {
            try
            {
                DateTimePicker1.Click(MouseButtons.Left);
                DateTimePicker1.Properties[TestDataConstants.TEXT_PROPERTY] = fromdate;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Check To_Date
        /// </summary>
        /// <param name="disposing"></param>
        private void ToDate(string todate)
        {
            try
            {
                DateTimePicker2.Click(MouseButtons.Left);
                DateTimePicker2.Properties[TestDataConstants.TEXT_PROPERTY] = todate;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Diagnose the given scripts
        /// </summary>
        /// <param name="disposing"></param>
        private void Diagnose()
        {
            try
            {
                BtnDiagnose.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Check Whether error occurs in quality or not. False if error occurs
        /// </summary>
        /// <param name="disposing"></param>
        private bool DetectError()
        {
            bool result = true;
            try
            {
                DataTable dtQualityChecker = CSVHelper.CSVAsDataTable(this.Datagridview_ScriptSelecter.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                for (int i = 0; i < dtQualityChecker.Rows.Count; i++)
                {
                    int status = Convert.ToInt16(Convert.ToChar(dtQualityChecker.Rows[i]["Status"]));
                    if (status == 10008)
                    {
                        result = false;
                        break; 
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
