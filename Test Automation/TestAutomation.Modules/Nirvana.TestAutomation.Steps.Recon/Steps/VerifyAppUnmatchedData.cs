using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Interfaces.Enums;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Recon
{
    public class VerifyAppUnmatchedData : ReconUIMap, ITestStep
    {
       public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string sheetName = sheetIndexToName[0];
                List<String> errors = VerifyApplicationUnmatchedData(testData, sheetName);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage = String.Join("\n\r", errors);
                }
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseRecon();
            }
            return _result;
       }
       public List<string> VerifyApplicationUnmatchedData(DataSet testData, String sheetName)
       {
           try
           {
               string uiName = "ApplicationUnMatchedData";
               DataTable dtApplicationData = ExportData(uiName);
               List<String> columns = new List<String>();
               columns.Add("AccountName");
               columns.Add("Symbol");
               columns.Add("Side");
               DataTable excelData = testData.Tables[sheetName];
               List<String> errors = Utilities.Recon.RunRecon(uiData: dtApplicationData, excelData: excelData, columns: columns, tolerance: 0.01, toleranceFlag: false, dateTimeFlag:false, reconType: ReconType.RoundingMatch, roundingDigit: 2, midpointRounding: MidpointRounding.AwayFromZero);  
               return errors;   

           }
           catch (Exception)
           {
               throw;
           }
       }
     
      /* public DataTable ExportApplicationData()
       {
           try
           {
               OpenRecon(); Wait(5000);
               MaximizeRecon(); Wait(5000);
               ExportToExcel.Click(MouseButtons.Left);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Wait(2000);
               Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
               Wait(5000);
               TextBoxFilename.Click(MouseButtons.Left);
               string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
               KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
               Keyboard.SendKeys(path + ExcelStructureConstants.ApplicationDataOnRecon);
               ButtonSave.Click(MouseButtons.Left);
               if (ConfirmSaveAs1.IsVisible)
               {
                   ButtonYes.Click(MouseButtons.Left);
               }
               ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.Xls);
               DataSet applicationDataSet = provider.GetTestData(path + @"\" + ExcelStructureConstants.ApplicationDataOnRecon);

               DataTable dtApplicationData = applicationDataSet.Tables[0];
               return dtApplicationData;
           }
           catch (Exception ex)
           {
               bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
               if (rethrow)
                   throw;
               return null;
           }

       }*/
        
            
    }
}
