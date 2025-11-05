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
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.ThirdParty
{
       class VerifyThirdPartyGenerate : ThirdPartyUIMap, ITestStep
       {
           /// <summary>
           /// RunTest method takes two parameter-
           /// 1.testData of DataSet type which gives xls sheet data.
           /// 2.sheetIndexToName of Dictionary<int,string> type which gives .xlsx file sheet No. and sheet name
           /// </summary>
           /// <param name="testData"></param>
           /// <param name="sheetIndexToName"></param>
           /// <returns></returns>
           public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
           {
               TestResult _result = new TestResult();
               try
               {
                   //open third party manager
                 OpenThirdPartyManager();
               
                 List<string> errors = VerifyData(testData.Tables[0]);
                 if (errors.Count > 0)
                     _result.ErrorMessage = String.Join("\n\r", errors);

                 //Minimize Third Party Manager
                 //MinimizeThirdPartyManager();
                 KeyboardUtilities.CloseWindow(ref FrmThirdParty_UltraFormManager_Dock_Area_Top);
               }
               catch (Exception ex)
               {
                   _result.IsPassed = false;
                   bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                   if (rethrow)
                       throw;
               }
               return _result;
           }


           /// <summary>
           /// Verify third party data after generate
           /// </summary>
           /// <param name="dTable"></param>
           /// <returns></returns>
           private List<string> VerifyData(DataTable dTable)
           {
               List<string> errors = new List<string>();
               try
               {
                   //Get name of last modified file
                   string fileName = GetLastModifiedFileFromEOD();

                   string filePath = (ApplicationArguments.ClientReleasePath + "\\EOD\\ISOX\\" + fileName);
                   DataTable generateFile = CSVHelper.GetDataSourceFromFile(filePath);
                   List<string> columns = new List<string>();
                   errors = Recon.RunRecon(generateFile, dTable, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
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
               base.Dispose(true);
               GC.SuppressFinalize(this);
           }

      }
 }
