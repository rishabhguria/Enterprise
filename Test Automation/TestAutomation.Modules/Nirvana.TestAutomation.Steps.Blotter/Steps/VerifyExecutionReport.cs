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
using TestAutomationFX.Core;
using System.IO;
using System.Reflection;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class VerifyExecutionReport : BlotterExecutionReportUIMap, ITestStep
    {
          public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
           {
               TestResult _res = new TestResult();
               try
               {
                   BlotterReports.BringToFront();
                   //OpenBlotterExecutionReport();
                   //BlotterReports.Click(MouseButtons.Left);
                   List<String> errors = VerifyData(testData.Tables[0]);
                   if (errors.Count > 0)
                       _res.ErrorMessage = String.Join("\n\r", errors);
                  
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
        /// a method to verify the blotter execution report data
        /// </summary>
        /// <param name="excelData"></param>
        /// <returns></returns>
          private List<string> VerifyData(DataTable excelData)
          {
               List<String> errors=new List<string>();
              try
              {   
                  DataTable dtExecutionReport = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.ExecutionReportGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                  dtExecutionReport = DataUtilities.RemoveCommas(dtExecutionReport);
                  List<String> columns = new List<string>();
                  columns.Add("Symbol");
                  errors = Recon.RunRecon(dtExecutionReport, excelData, columns, 0.01);    
              }
              catch (Exception ex)
              {
                  bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                  if (rethrow)
                      throw;                  
              }
              return errors;
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

