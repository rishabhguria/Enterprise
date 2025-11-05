using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.IO;
using System.Reflection;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class ExportExecutionReportData:BlotterExecutionReportUIMap, ITestStep
    {
          public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
           {
               TestResult _res = new TestResult();
               try
               {
                   BlotterReports.Click(MouseButtons.Left);
                   BtnExportToExcel.Click(MouseButtons.Left);
                   string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                   if (!Directory.Exists(path))
                   {
                       Directory.CreateDirectory(path);
                   }
                   SaveAs.WaitForVisible();
                   Clipboard.SetText(path + ExcelStructureConstants.BlotterExecutionReportFileName);
                   Keyboard.SendKeys("[CTRL+V]");
                   ButtonSave.Click(MouseButtons.Left);
                   ConfirmSaveAs1.WaitForObject();
                   if (ConfirmSaveAs1.IsValid)
                       ButtonYes.Click(MouseButtons.Left);
                   Confirmation.WaitForObject();
                   ButtonOK.Click(MouseButtons.Left);
                  
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
                   MinimizeBlotterExecutionReport();
               }
               return _res;
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
