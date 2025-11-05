using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.UnitTesting.MockDataCreation
{
    public static class CreateDataTable
    {
        /// <summary>
        /// To create a DataTable with some Values
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetTable()
        {
            // here we create a DataTable.
            // We add 4 columns, each with a Type.
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Diagnosis", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Step 3: here we add 4 rows.
            table.Rows.Add(25, "Drug A", "Disease A", DateTime.Parse("7/3/2024 18:41:56"));
            table.Rows.Add(50, "Drug Z", "Problem Z", DateTime.Parse("7/3/2024 18:41:56"));
            table.Rows.Add(10, "Drug Q", "Disorder Q", DateTime.Parse("7/3/2024 18:41:56"));
            table.Rows.Add(21, "Medicine A", "Diagnosis A", DateTime.Parse("7/3/2024 18:41:56"));
            return table;
        }

        /// <summary>
        /// To get the DataTable as List of String, this list is represented as a string of above DataTable
        /// </summary>
        /// <returns></returns>
        public static List<String> GetTableDataAsListOfString()
        {
            return new List<string>()
            {
                "Header^Dosage=System.Int32^Drug=System.String^Diagnosis=System.String^Date=System.DateTime"
                ,"Dosage=25^Drug=Drug A^Diagnosis=Disease A^Date=7/3/2024 18:41:56"
                ,"Dosage=50^Drug=Drug Z^Diagnosis=Problem Z^Date=7/3/2024 18:41:56"
                ,"Dosage=10^Drug=Drug Q^Diagnosis=Disorder Q^Date=7/3/2024 18:41:56"
                ,"Dosage=21^Drug=Medicine A^Diagnosis=Diagnosis A^Date=7/3/2024 18:41:56"
             };
        }
    }
}
