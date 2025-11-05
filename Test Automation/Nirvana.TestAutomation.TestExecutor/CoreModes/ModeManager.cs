using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.TestExecutor
{
    public class ModeManager
    {
        public static IFixingApproach GetApproach()
        {
            try
            {
                string approachName = "";

                // Read approach name from config
                string configValue = ConfigurationManager.AppSettings["CustomApproachName"];
                if (!string.IsNullOrEmpty(configValue))
                {
                    approachName = configValue.ToLower();
                }

                var table = ApplicationArguments.ITestCaseFixingTables[TestDataConstants.Sheet_CaseWiseFixingApproach];
                if (table == null || table.Rows.Count == 0)
                {
                    throw new Exception("Sheet_CaseWiseFixingApproach not loaded successfully.");
                }

                approachName = FetchCustomizedApproach(table).ToLower();

                if (string.IsNullOrEmpty(approachName))
                {
                    throw new Exception("Approach name is missing from configuration.");
                }

                switch (approachName)
                {
                    case "columnvaluereplace":
                        Console.WriteLine("ModeManager: Using CustomApproach");
                        return new ColumnValueReplaceApproach();

                    case "defaultapproach":
                        Console.WriteLine("ModeManager: Using defaultapproach");
                        return null;//need to create its implementation

                    default:
                        Console.WriteLine("ModeManager: returning null");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ModeManager.GetApproach: " + ex.Message);
                return null;
            }
        }

        private static string FetchCustomizedApproach(System.Data.DataTable table)
        {
            try
            {
                if (table == null || table.Rows.Count == 0)
                    return string.Empty;

                int testCaseIdColIndex = -1;
                int fixingApproachColIndex = -1;

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    string colName = table.Columns[i].ColumnName.Trim();
                    if (string.Equals(colName, "Testcase ID", StringComparison.OrdinalIgnoreCase))
                        testCaseIdColIndex = i;

                    if (string.Equals(colName, "FixingApproach", StringComparison.OrdinalIgnoreCase))
                        fixingApproachColIndex = i;
                }

                if (testCaseIdColIndex == -1 || fixingApproachColIndex == -1)
                    return string.Empty;

                foreach (System.Data.DataRow row in table.Rows)
                {
                    object testCaseIdValue = row[testCaseIdColIndex];
                    if (testCaseIdValue != null &&
                        string.Equals(testCaseIdValue.ToString().Trim(), ApplicationArguments.TestCaseToBeRun, StringComparison.OrdinalIgnoreCase))
                    {
                        object fixingApproachValue = row[fixingApproachColIndex];
                        return fixingApproachValue != null ? fixingApproachValue.ToString().Trim() : string.Empty;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching customized approach: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
