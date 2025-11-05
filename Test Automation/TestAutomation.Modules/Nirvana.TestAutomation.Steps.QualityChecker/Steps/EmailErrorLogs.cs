using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Steps.QualityChecker
{
    class EmailErrorLogs : QualityCheckerUIMap, ITestStep
    {
      private string _qualitycheckerdirectorypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\QualityChecker\";
      private string _archivedirectorypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\QualityChecker_Archive\";
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                RestoreWindow();
                string smtp = null, sender = null, reciever = null, ccto = null, bccto = null, username = null, password = null,message=null;
                int port = 0;
                if (!Directory.Exists(_archivedirectorypath))
                {
                    Directory.CreateDirectory(_archivedirectorypath);
                }
                if (!Directory.Exists(_qualitycheckerdirectorypath))
                {
                    Directory.CreateDirectory(_qualitycheckerdirectorypath);
                }
                if (testData != null && Directory.Exists(_qualitycheckerdirectorypath) && Directory.EnumerateFiles(_qualitycheckerdirectorypath).Any())
                {
                    DataRow dr = testData.Tables[0].Rows[0];
                    smtp = dr[0].ToString();
                    port = Convert.ToInt16(dr[1].ToString());
                    sender = dr[2].ToString();
                    reciever = dr[3].ToString();
                    ccto = dr[4].ToString();
                    bccto = dr[5].ToString();
                    message = dr[6].ToString();
                    username = dr[7].ToString();
                    password = dr[8].ToString();
                    EmailHelper.Mail_ExportedFile(smtp, port, sender, reciever, ccto, bccto, username, password,message,_qualitycheckerdirectorypath);
                    MoveToArchive();
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
                CloseQualityCheckerWindow();
            }
            return _res;
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
                foreach (var file in new DirectoryInfo(_qualitycheckerdirectorypath).GetFiles())
                {
                    file.MoveTo(_archivedirectorypath + "\\" + file.Name);
                }
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
