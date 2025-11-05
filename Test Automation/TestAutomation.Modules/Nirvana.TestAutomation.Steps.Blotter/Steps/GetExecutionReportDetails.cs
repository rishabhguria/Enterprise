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
namespace Nirvana.TestAutomation.Steps.Blotter
{
   public class GetExecutionReportDetails:BlotterExecutionReportUIMap, ITestStep
    {
           public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
           {
               TestResult _res = new TestResult();
               try
               {
                   OpenBlotterExecutionReport();
                   //Wait(100);
                   if (testData != null)
                   {
                       DataRow emptydatarow = null;
                       if (testData.Tables[0].Rows.Count == 0)
                       {
                           ExecutionReportDateRange(emptydatarow);
                       }
                       else
                       {
                           foreach (DataRow dr in testData.Tables[0].Rows)
                           {
                               ExecutionReportDateRange(dr);
                           }
                       }
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
                   //MinimizeBlotterExecutionReport();
               }
               return _res;
           }

           /// <summary>
           /// Enter From and TO Date Range
           /// </summary>
           private void ExecutionReportDateRange(DataRow dr)
           {
               try
               {

                   ExecutionReportCommonDateRange(dr);
                   BtnGetDetailedReport.Click(MouseButtons.Left);
               }
               catch (Exception ex)
               {
                   bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                   if (rethrow)
                       throw;

               }
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
