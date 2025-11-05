using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Common.UnitTesting.MockDataCreation
{
    public static class CreateCsvFile
    {
        public static string CreateCsv(string filePath,string csvheader, List<string> rows)
        {
            // Create a StringBuilder to hold the CSV data
            StringBuilder csvContent = new StringBuilder();

            // Add the header row
            if (csvheader != null)
            {
                csvContent.AppendLine(csvheader);

                // Add some data rows
                foreach (var row in rows)
                {
                    csvContent.AppendLine(row);
                }
            }

            // Write the content to the file
            File.WriteAllText(filePath, csvContent.ToString());

            return filePath;
        }
    }
}
