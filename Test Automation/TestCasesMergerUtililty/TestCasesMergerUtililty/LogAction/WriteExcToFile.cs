using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCasesMergerUtililty.LogAction
{
    class WriteExcToFile
    {
        public static void WriteToFileError(string error, string stackTrace)
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);
            StreamWriter writer = new StreamWriter(path + "\\MergerUtilityErrorLog.txt", true);
            DateTime localDate = DateTime.Now;
            writer.WriteLine(Environment.NewLine + "Time: " + localDate.TimeOfDay + Environment.NewLine + "Error Message: " + error + Environment.NewLine + "StackTrace: " + Environment.NewLine + stackTrace + Environment.NewLine);
            writer.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            writer.Close();
        }
    }
}
