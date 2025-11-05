using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Reflection;
using System.Configuration;
using OpenQA.Selenium.Interactions;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Runtime.Caching;
using Nirvana.TestAutomation.Factory;

namespace Nirvana.TestAutomation.Steps.PranaClient
{
    class RunPortfolio : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {   
        
                DataRow dr = testData.Tables[0].Rows[0];  
                MemoryCache cache = CacheManager.Instance.GetCache();
                List<string> UndefinedModuleSteps = new List<string>();
                List<string> _modulesAndStepsMapping = new List<string>();
                string ModuleName = dr[TestDataConstants.COL_MODULENAME].ToString();
                string CaseID = dr[TestDataConstants.COL_CASEID].ToString();

                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet dtModulesAndStepsMapping = provider.GetTestData(ApplicationArguments.StepMappingFilePath + ExcelStructureConstants.FILE_STEPS_MAPPING, 3, 1);
                _modulesAndStepsMapping = new List<string>();
                for (int counter = 0; counter < dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows.Count; counter++)
                {
                    if (!_modulesAndStepsMapping.Contains(dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_MODULES].ToString() + "_" + dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_STEPS].ToString()))
                    {
                        _modulesAndStepsMapping.Add(dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_MODULES].ToString() + "_" + dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_STEPS].ToString());
                    }
                }

                // get list of sheets in workbook, the provider reads from 5th row and 2nd column to read data as per current test cases file format

                DataSet workbook = new DataSet();

                if (cache.Contains("Regression Test Cases"))
                {
                    Console.WriteLine("RegressionTestCases DataSet loaded from Cache");
                    workbook = cache["Regression Test Cases"] as DataSet;
                }
                else
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    // get list of sheets in workbook, the provider reads from 5th row and 2nd column to read data as per current test cases file format
                    workbook = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + ApplicationArguments.Workbook, 5, 2);

                    stopwatch.Stop();
                    long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine("Time taken to get Regression Test Cases dataset: " + elapsedMilliseconds + " ms");
                    DataSet workbookCopy = workbook.Copy();
                    cache.Add("Regression Test Cases", workbookCopy, new CacheItemPolicy());
                }

                String testCaseFileLocation = ApplicationArguments.TestDataFolderPath;

                String testCase = string.Empty;
                String Col_Module = string.Empty;
                String testCaseDescription = string.Empty;
                foreach (DataRow dr3 in workbook.Tables[ModuleName].Rows)
                {
                    if (String.IsNullOrWhiteSpace(dr3[ExcelStructureConstants.COL_TESTCASEID].ToString()))
                    {
                        dr3[ExcelStructureConstants.COL_TESTCASEID] = testCase;
                        ModuleName = Col_Module;
                    }
                    else
                        testCase = dr3[ExcelStructureConstants.COL_TESTCASEID].ToString();
                        Col_Module = ModuleName;
                }
                //Stopwatch timer = GetTimerInstance();
                string category = string.Empty;
                string moduleName = String.Empty;
                string stepName = String.Empty;

                DataRow[] testCaseData = workbook.Tables[ModuleName].Select(String.Format(ExcelStructureConstants.COL_TESTCASEID + " = '{0}'", CaseID));
                DataSet testCaseData1 = new DataSet();
                Log.Information("Running User Define test case : " + CaseID);

                moduleName = MessageConstants.MODULE_APPLICATION;
                stepName = MessageConstants.APPLICATION_START_UP;

                string dataSheetPath = testCaseFileLocation + "\\" + CaseID + "\\" + CaseID + ".xlsx";

                Dictionary<string, string> StepValueMapping = DataUtilities.GetTestCaseStepMapping(dataSheetPath, workbook.Tables[ModuleName], CaseID);
                //DataUtilities.DeleteUneccessarySheetsFromWorkbook(dataSheetPath, StepValueMapping, UndefinedModuleSteps);
                if (File.Exists(dataSheetPath))                 
                {
                    if (cache.Contains(CaseID))
                    {
                        testCaseData1 = cache[CaseID] as DataSet;
                        Console.WriteLine("Taking testsheetdata from cache => " + CaseID);
                    }
                    else
                    {
                        testCaseData1 = provider.GetTestData(dataSheetPath, 5, 2);
                    }
                }
                        //timer.Restart();
                        TestResult stepResult = new TestResult();
                        TestResult stepResult1 = new TestResult();
						//Check the step that can perform with multiuser or not
                        ConfigurationManager.AppSettings["IsMultiUser"] = "true";
                        string[] StepsList = ConfigurationManager.AppSettings["StepToPerformWithSingleUser"].Split(',');
                        foreach (var row in testCaseData)
                        {
                           if(StepsList.Contains(row[4].ToString()))
                               ConfigurationManager.AppSettings["IsMultiUser"] = "false";
                        }
                        //loop for each step and perform step action
                        foreach (DataRow dr2 in testCaseData)
                        {
                            /*if (testData[0].ItemArray[4].Equals(AutomationStepsConstants.LOGIN_CLIENT))  //"LoginClient"
                            {
                                continue;
                            }*/
                            moduleName = dr2[ExcelStructureConstants.COL_MODULE].ToString();
                            stepName = dr2[ExcelStructureConstants.COL_STEP].ToString();
                            /*if (testCaseDescription.Equals(string.Empty))
                                testCaseDescription = dr[ExcelStructureConstants.COL_DESCRIPTION].ToString();*/
                            Dictionary<int, string> sheetIndexToName1 = new Dictionary<int, string>();
                            // get data required for this step from test case data
                            DataSet stepData = new DataSet();
                            for (int i = 1; i <= ExcelStructureConstants.Total_input_sheets; i++)
                            {
                                if (!String.IsNullOrWhiteSpace(dr2[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()) && !UndefinedModuleSteps.Contains(dr2[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()))
                                {
                                    DataTable dt = testCaseData1.Tables[dr2[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()];

                                    sheetIndexToName1.Add(i - 1, dr2[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString());
                                    stepData.Tables.Add(dt.Copy());
                                }
                            }
                            if (_modulesAndStepsMapping.Contains(moduleName + "_" + stepName))
                            {
                                ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                stepResult = (TestResult)step.RunTest(stepData, sheetIndexToName1);
                            }
                            if (stepResult.IsPassed)
                            {
                                Log.Success(stepName + " passed");
                            }
                            else
                            {
                                Log.Error(stepName + " failed");
                                Log.Error(moduleName + "_" + stepName + " failed.");
                                break;
                            }
                        }
               /*         Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        testCaseData1 = DataUtilities.GetTestCaseTestData(dataSheetPath, 5, 2, UndefinedModuleSteps);
                        stopwatch.Stop();
                        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                        Console.WriteLine("Time taken to get TestCase dataset: " + elapsedMilliseconds + " ms");
                        if (ApplicationArguments.RetrySize > 0)
                        {
                            cache.Add(CaseID, testCaseData, new CacheItemPolicy());
                            Console.WriteLine(CaseID + "added to cache as retry ecexution is twice or more");
                        }
                    }
                }*/
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


            }
            return _result;
        }
        private class CacheManager
        {
            private static CacheManager instance;
            private MemoryCache cache;

            private CacheManager()
            {
                cache = MemoryCache.Default;
            }

            public static CacheManager Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new CacheManager();
                    }
                    return instance;
                }
            }

            public MemoryCache GetCache()
            {
                return cache;
            }
        }
        /*public interface ITestDataProvider
        {
        DataSet GetTestData(String filePath, int rowHeaderIndex = 1, int startColumnFrom = 1);
        }*/
        /*private class TestDataProvider
        {
            public static ITestDataProvider GetProvider(ProviderType type)
            {
                switch (type)
                {
                    case ProviderType.Excel:
                        return new ExcelDataProvider();
                    case ProviderType.GoogleSheets:
                        return new GoogleSheetsDataProvider();
                    case ProviderType.OpenXml:
                        return new OpenXmlDataProvider();
                    case ProviderType.Xls:
                        return new XlsDataProvider();

                }
                return null;
            }
        }*/      
    }
}
