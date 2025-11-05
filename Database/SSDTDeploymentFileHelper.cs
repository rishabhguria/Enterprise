using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDT
{
    class SSDTDeploymentFileHelper
    {
        private static String _oneTimeScriptTemplate ="IF NOT EXISTS(SELECT 1 FROM T_DataScripts WHERE ScriptName = '@ScriptNameXXXXX')\r\n" +
                                                    "BEGIN\r\n" +
                                                     "PRINT '-------- Started Executing @ScriptNameXXXXX ----------' \r\n" +
                                                    ":r @ScriptPathXXXXX\r\n" +
                                                    "INSERT INTO T_DataScripts(ScriptName) VALUES('@ScriptNameXXXXX')\r\n" +
                                                    "PRINT '-------- Done Executing @ScriptNameXXXXX ----------' \r\n" +
                                                    "END\r\n\r\n\r\n" +
                                                    "GO \r\n";
        
        private static String _mode, _folderToParse, _targetFilePath;


        static void Main(string[] args)
        {
            try
            {
                _mode = args[0]; // This should be either One Time (1) Or Multiple (2)
                _folderToParse = args[1];
                _targetFilePath = args[2];

                int mode = _mode.Equals("onetime") ? 1 : 2;

                String folderName = Path.GetFileName(_folderToParse);

                // delete the file if it exists, as we need a clean file always (this also takes care of the deleted scripts)
                if (File.Exists(_targetFilePath))
                    File.Delete(_targetFilePath);


                using (System.IO.StreamWriter file = new System.IO.StreamWriter(_targetFilePath, true))
                {
                    file.WriteLine("----------------------------------------");
                    file.WriteLine("----------AUTO GENERATED CODE-----------");
                    file.WriteLine("----------------------------------------");

                    if (mode == 1)
                    {
                        // ONE TIME DEPLOYMENT SCRIPT
                        foreach (String sqlFile in Directory.GetFiles(_folderToParse, "*.sql"))
                        {
                            // replace the file name and path
                            String textToWrite = _oneTimeScriptTemplate.Replace("@ScriptPathXXXXX", folderName + @"\" + Path.GetFileName(sqlFile));
                            textToWrite = textToWrite.Replace("@ScriptNameXXXXX", Path.GetFileName(sqlFile));
                            file.WriteLine(textToWrite);
                        }
                    }
                    if (mode == 2)
                    {
                        // Data Scripts
                        foreach (String sqlFile in Directory.GetFiles(_folderToParse, "*.sql"))
                        {
                            file.WriteLine(String.Format("PRINT '-------- Started Executing {0} ----------'", Path.GetFileName(sqlFile)));
                            file.WriteLine(String.Format(":r {0}", folderName + @"\" + Path.GetFileName(sqlFile)));
                            file.WriteLine(String.Format("PRINT '-------- Done Executing {0} ----------'", Path.GetFileName(sqlFile)));
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(System.Environment.CurrentDirectory , "CriticalErrorLog.txt"), true);
                file.WriteLine(ex.Message);
                file.WriteLine(ex.StackTrace);
                
            }
        }
    }
}