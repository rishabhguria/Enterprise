using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;
using System.IO;
using System.Globalization;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class VerifyTagInAllFixLogs : CameronSimulator, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0].Columns.Contains("Negative Testing"))
                {
                    Dictionary<string, string> NegativeDict = new Dictionary<string, string>();
                    DataSet dt = testData.Copy();
                    for (int i = testData.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        if (dt.Tables[0].Rows[i]["Negative Testing"].ToString().ToUpper().Equals("TRUE"))
                        {
                            NegativeDict.Add(dt.Tables[0].Rows[i][TestDataConstants.COL_TAG].ToString(), dt.Tables[0].Rows[i][TestDataConstants.COL_VALUE].ToString());
                        }
                        else 
                        {
                            dt.Tables[0].Rows[i].Delete();
                        }
                    }
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        bool verification = FindandVerifyTags(dt.Tables[0], NegativeDict);
                        if (verification.Equals(true))
                        {
                            throw new Exception("Negative Verification of tags in allfixlogs Failed!!");
                        }
                        else
                        {
                            Console.WriteLine("Negative testing succeeded");
                        }
                    }
                    for (int i = testData.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        if (!string.IsNullOrEmpty(testData.Tables[0].Rows[i]["Negative Testing"].ToString()))
                        {
                            testData.Tables[0].Rows[i].Delete();
                        }
                    }
                }
                Dictionary<string, string> ExcelDataDict = new Dictionary<string, string>();
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    ExcelDataDict.Add(dr[TestDataConstants.COL_TAG].ToString(), dr[TestDataConstants.COL_VALUE].ToString());
                }
                bool verificationResult = FindandVerifyTags(testData.Tables[0], ExcelDataDict);
                if (verificationResult.Equals(true))
                {
                    return _result;
                }
                else
                    throw new Exception("Verification of tags in allfixlogs Failed!!");
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }

         public Boolean FindandVerifyTags(DataTable dt, Dictionary<string, string> ExcelDataDict)
         {

             string AllFixPath = ConfigurationManager.AppSettings["AllFixLog"];
             string tempFilePath = Path.Combine(Path.GetDirectoryName(AllFixPath), "AllFixLogs_backup.log");
             if (!File.Exists(AllFixPath))
             {
                 throw new Exception("AllFixLog file doesnot exist!!");
             }
             if (File.Exists(tempFilePath))
             {
                 File.Delete(tempFilePath);
             }

              if (File.Exists(AllFixPath))
             {
                 File.Copy(AllFixPath, tempFilePath, true);

                 Dictionary<string, List<string>> Data = new Dictionary<string, List<string>>();
                 foreach (KeyValuePair<string, string> dic in ExcelDataDict)
                 {
                     Data.Add(dic.Key + "=" + dic.Value, new List<string>());
                 }


                 using (StreamReader reader = new StreamReader(tempFilePath))
                 {
                     string line;
                     int linenumber = 0;

                     while ((line = reader.ReadLine()) != null)
                     {
                         
                         foreach (KeyValuePair<string, List<string>> entry in Data)
                         {
                             if (line.Contains((entry.Key)))
                             {
                                 Data[(entry.Key)].Add(linenumber.ToString());//collecting all the line number where that pair exists
                                 // List<string> FoundAt = new List<string>();
                                 string nextLine = reader.ReadLine();
                                 //FoundAt.Add(linenumber.ToString());
                                
                             }
                         }
                         linenumber++;
                     }

                 }


                 var commonNumbers = Data.Values.Aggregate((previousList, nextList) => previousList.Intersect(nextList).ToList());

                 if (commonNumbers.Any())
                 {
                     Console.WriteLine("The common tags found at line :{0}", string.Join(", ", commonNumbers));
                 }
                 else 
                 {
                     Console.WriteLine("There is no common line/lines containing all the tags at single line");
                     return false;
                 
                 }

             }

             return true;
         }
    }
}
