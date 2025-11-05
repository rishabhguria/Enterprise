using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.IO;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class VerifyCustomAllocationonTT: TradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                ExtentionMethods.WaitForVisible(ref TradingTicket1, 20);
                if (TradingTicket1.IsVisible)
                {
                    if (testData.Tables[0].Rows.Count>0)
                    {
                        BtnAccountQty.Click(MouseButtons.Left);
                       // Wait(6000);
                        List<String> errors = VerifyCustomAllocationGrid(testData.Tables[0]);
                        if (errors.Count > 0)
                            _result.ErrorMessage = String.Join("\n\r", errors);
                        else if (errors.Count == 0)
                        {
                            Console.WriteLine("Verification of Custom Alloaction grid is passed");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyCustomAllocationonTT");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                //Wait(1000);
                BtnOK.Click(MouseButtons.Left);
            }
            return _result;
        }

        private List<String> VerifyCustomAllocationGrid(DataTable dTable)
        {
            List<String> errors = new List<String>();
            // List<String> errors = Recon.RunRecon(TT, dTable, columns, 0.01);
       
            try
            {
                var gridMssaObject = GrdAccounts.MsaaObject;
                Dictionary<int, string> colToIndexMappingDictionary = new Dictionary<int, string>();
                DataTable dataTable = new DataTable();


                for (int i = 1; i < gridMssaObject.CachedChildren[0].CachedChildren[0].ChildCount; i++)
                {    
                    dataTable.Columns.Add(gridMssaObject.CachedChildren[0].CachedChildren[0].CachedChildren[i].Name, typeof(string));
                }

                 DataTable TT = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdAccounts.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                 TT = DataUtilities.RemoveCommas(TT);
                List<String> columns = new List<string>();

                columns.Add("Account");
                
                 errors = Recon.RunRecon(TT, dTable, columns, 0.01);
                return errors;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
 
        }
          protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
                
        }
        
         
    }

}
