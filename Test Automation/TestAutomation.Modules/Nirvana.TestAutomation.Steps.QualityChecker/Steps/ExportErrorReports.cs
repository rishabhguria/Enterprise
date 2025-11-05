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
using System.IO;
using System.Reflection;

namespace Nirvana.TestAutomation.Steps.QualityChecker
{
    class ExportErrorReports: QualityCheckerUIMap, ITestStep
    {
        private string _qualitycheckerdirectorypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\QualityChecker\";
        private string _archivedirectorypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\QualityChecker_Archive\";
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                RestoreWindow();
                if (!Directory.Exists(_archivedirectorypath))
                {
                    Directory.CreateDirectory(_archivedirectorypath);
                }
                if (!Directory.Exists(_qualitycheckerdirectorypath))
                {
                    Directory.CreateDirectory(_qualitycheckerdirectorypath);
                }
                DetectError();
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

        /// <summary>
        /// Dietect the error
        /// </summary>
        /// <param name="disposing"></param>
        private void DetectError()
        {
            try
            {
                DataTable dtQualityChecker = CSVHelper.CSVAsDataTable(this.Datagridview_ScriptSelecter.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                MsaaObject obj = Datagridview_ScriptSelecter.MsaaObject.CachedChildren.First(x => x.Name.Equals("List`1"));
                if (Directory.EnumerateFiles(_qualitycheckerdirectorypath).Any())
                {
                    MoveToArchive();
                }
                for (int i = 0; i < dtQualityChecker.Rows.Count; i++)
                {
                    int status = Convert.ToInt16(Convert.ToChar(dtQualityChecker.Rows[i]["Status"]));
                    if (status == 10008 || status == 33)
                    {
                        var Row = obj.CachedChildren[i + 1];
                        Datagridview_ScriptSelecter.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, i);
                        Datagridview_ScriptSelecter.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 5);
                        Row.CachedChildren[5].Click(MouseButtons.Left);
                        if (uiWindow1.IsVisible == true)
                        {
                            ButtonOK.Click(MouseButtons.Left);
                        }
                        else
                        {
                            ExportFilesToLocation();
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
            finally
            {
                MinimizeWindow();
            }
        }

        /// <summary>
        /// Move the files to archive folder
        /// </summary>
        /// <param name="disposing"></param>
        private void MoveToArchive()
        {
            try
            {
                string[] fileList = Directory.GetFiles(_qualitycheckerdirectorypath);
                if (Directory.Exists(_qualitycheckerdirectorypath))
                {
                    foreach (var file in new DirectoryInfo(_qualitycheckerdirectorypath).GetFiles())
                    {
                        file.MoveTo(_archivedirectorypath + "\\" + file.Name);
                    }
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
        /// Export error log files to desired location
        /// </summary>
        /// <param name="disposing"></param>
        private void ExportFilesToLocation()
        {
            try
            {
                LblMoudleDetail.WaitForObject();
                LblErrorMsgDetail.WaitForObject();
                string filename = LblMoudleDetail.Text + "" + LblErrorMsgDetail.Text + "" + DateTime.Now.ToString();
                string modifiedfilename = filename.Replace(':', '-');
                Clipboard.SetText(_qualitycheckerdirectorypath + modifiedfilename);
                UltraGrid1.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                SaveAs.WaitForVisible();
                Keyboard.SendKeys("[CTRL+V]");
                ButtonSave.Click(MouseButtons.Left);
                Wait(2000);
                CloseErrorWindow();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
