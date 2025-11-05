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
using Nirvana.TestAutomation.UIAutomation;
using TestAutomationFX.Core;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class VerifyMPRB : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                if (testData.Tables[0] != null && testData.Tables[0].Rows.Count > 0)
                {
                    Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                    GridDataProvider gridDataProvider = new GridDataProvider();
                    DataTable dtable = gridDataProvider.GetWPFGridDataWithCheckBoxes("RebalancerWindow", "ModelPortfolioGrid");
                    if (testData.Tables[0].Columns.Contains("Action"))
                    {
                        if (testData.Tables[0].Columns.Contains("Tolerance BPS"))
                        {
                            testData.Tables[0].Columns["Tolerance BPS"].ColumnName = "Tolerance %";
                        }
                        gridDataProvider.EditGrid("ModelPortfolioGrid", testData.Tables[0]);
                        testData.Tables[0].Columns.Remove("Action");
                        dtable = gridDataProvider.GetWPFGridDataWithCheckBoxes("RebalancerWindow", "ModelPortfolioGrid");
                        if (dtable.Columns.Contains("Tolerance BPS"))
                        {
                            testData.Tables[0].Columns["Tolerance %"].ColumnName = "Tolerance BPS";
                        }
                    }
                    List<string> errors = new List<string>();
                    List<string> columns = new List<string>();

                    if (testData.Tables[0].Columns.Contains("Testing Type"))
                    {
                        for (int i = testData.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            if (testData.Tables[0].Rows[i]["Testing Type"].ToString().ToUpper().Equals("NEGATIVE"))
                            {
                                testData.Tables[0].Rows[i].Delete();
                            }
                            
                        }
                        testData.Tables[0].Columns.Remove("Testing Type");
                    } 
                    try
                    {
                        errors = Recon.RunRecon(dtable, testData.Tables[0], columns, 0.01);
                        if (errors.Count > 0)
                            _res.ErrorMessage = String.Join("\n\r", errors);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                        if (rethrow)
                            throw;
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
            return _res;
        }
    }
}
