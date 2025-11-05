using System;
using System.ComponentModel;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core.UIAutomationSupport;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public partial class CheckDashboardWithPM : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Failed to check dashboard with PM. Reason : \n(" + ex.Message + ")
        /// </exception>

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {                  
			  OpenConsolidationView();
               
                //Wait(9000);
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetSummaryFromGrid", null).ToString()));
                DataTable subset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.DashboardGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
               
                Dictionary<string, string> dashBoardPMColumnMapping = CreateMapping();
                var string1 = string.Empty;
                foreach (DataColumn col in subset.Columns)
                {
                    if (dashBoardPMColumnMapping.ContainsKey(col.Caption))
                    {
                        subset.Columns[col.Caption].ColumnName = dashBoardPMColumnMapping[col.Caption];
                        string value = subset.Rows[0][col.Caption].ToString();
                        double result;
                        if (!double.TryParse(value, out result))
                        {
                            string1 = subset.Rows[0][col.Caption].ToString();
                            subset.Rows[0][col.Caption] = string1.Substring(0, string1.Length - 1);
                        }
                    }
                    else
                    {
                        subset.Rows[0][col.Caption] = "";
                    }
                }
                List<string> columns = new List<string>();
                List<string> errors = new List<string>();
                DataTable newSubset = DataUtilities.RemoveTrailingZeroes(subset);
                errors = Recon.RunRecon(superset, newSubset, columns, 0.0000000001);
                if (errors.Count > 0)
                {
                   _result.ErrorMessage=String.Join("\n\r", errors);
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
                PMclose();
            }
            return _result;
        }

        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> CreateMapping()
        {
            Dictionary<string, string> ColumnMapping = new Dictionary<string, string>();
            try
            {
                ColumnMapping.Add("Beta Adj. Exposure", "Beta Adj. Exposure (Base)");
                ColumnMapping.Add("Beta Adj. Gross Exposure", "Beta Adj. Gross Exposure (Underlying) (Base)");
                ColumnMapping.Add("Beta Adj. Gross Exposure %", "% Beta Adj. Gross Exposure (Base)");
                ColumnMapping.Add("Closing Market Value", "Closing Market Value (Base)");
                ColumnMapping.Add("Cost Basis P&L", "Cost Basis P&L (Base)");
                ColumnMapping.Add("Day P&L", "Day P&L (Base)");
                ColumnMapping.Add("Day P&L (FX)", "Day P&L (Base)(FX Gain)");
                ColumnMapping.Add("Day Return", "Day Return");
                ColumnMapping.Add("Earned Dividend Base", "Earned Dividend (Base)");
                ColumnMapping.Add("Gross Exposure", "Gross Exposure (Underlying) (Base)");
                ColumnMapping.Add("Gross Exposure %", "% Gross Exposure (Underlying) (Base)");
                ColumnMapping.Add("Gross Market Value", "Gross Market Value (Base)");
                ColumnMapping.Add("Gross Market Value %", "% Gross Market Value (Base)");
                ColumnMapping.Add("NAV", "NAV");
                ColumnMapping.Add("Net Exposure", "Net Exposure (Base)");
                ColumnMapping.Add("Net Market Value %", "% Net Market Value (Base)");
                ColumnMapping.Add("Net Exposure %", "% Net Exposure (Base)");
                ColumnMapping.Add("Start of Day NAV", "Start of Day NAV");
                ColumnMapping.Add("Underlying Value (Options)", "Underlying Value (Options)");
            }
            catch (Exception)
            {
                
                throw;
            }
            return ColumnMapping;
        }
    }
}
