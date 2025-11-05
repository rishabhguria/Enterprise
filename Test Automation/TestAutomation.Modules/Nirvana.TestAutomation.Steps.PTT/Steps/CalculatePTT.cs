using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.PTT;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;
using System.Configuration;
using Nirvana.TestAutomation.UIAutomation;
using System.Diagnostics;
using Nirvana.TestAutomation.Factory;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class CalculatePTT : PTTUIMap, ITestStep 
    {
       
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
           
            if (string.Equals(ApplicationArguments.AutomationProviderKey, "WINAPP", StringComparison.Ordinal))
                {
                   //winapp
                    try
                    {
                        if (ApplicationArguments.winappStarted == false)
                        {
                            WinAppUtility.StartWinAppDriverServer();
                        }
                          Process[] _process = Process.GetProcessesByName("Prana");
                     foreach (Process proc in _process)
                        proc.Kill();


                     WinAppDriverProvider driver = new WinAppDriverProvider(true, ApplicationArguments.ClientReleasePath + "\\Prana.exe");
                    
                     try
                     {
                         string automationProviderKey = ApplicationArguments.AutomationProviderKey;
                         AutomationProviderFactory factory = new AutomationProviderFactory();


                         IAutomationProvider automationProvider = factory.CreateAutomationProvider(automationProviderKey);



                          WinAppUtility.ApplicationStartup(driver);
                         automationProvider.LeftClick(DataContainer.openPTT);



                               
                     }
                     catch (Exception ex)
                     {
                         SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CalculatePTT");
                         _result.IsPassed = false;
                         bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                         if (rethrow)
                             throw;
                     }
                    
                    }
                    catch (Exception ex)
                    {
                        SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CalculatePTT");
                        _result.IsPassed = false;
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }
                    return _result;







                }
                else
            {
            try
            {
                UIAutomation.DataContainer.CalculatePTT = testData;
                UIAutomation.DataContainer.tempsheetIndexToName = sheetIndexToName;
                
               
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                        foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                        {
                            if (dataRow.Table.Columns.Contains(TestDataConstants.COL_SYMBOLOGY))
                            {
                                if (dataRow[TestDataConstants.COL_SYMBOLOGY].ToString() != String.Empty && dataRow[TestDataConstants.COL_SYMBOLOGY].ToString().Equals("Bloomberg Symbol"))// Select Symbology to Default
                                {
                                    CopyTTGeneralPref();
                                    OpenGeneralPreferences();
                                    CmbSymbology.Click(MouseButtons.Left);
                                    CmbSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_SYMBOLOGY].ToString();
                                    BtnSave.DoubleClick(MouseButtons.Left);
                                    Wait(5000);
                                    BtnClose.DoubleClick(MouseButtons.Left);
                                }
                            }
                        }
                }
                    OpenPTT();
                    //Wait(2000);
                    InputParametersPTT(testData, sheetIndexToName);
                    Calculate.Click(MouseButtons.Left);
                    Calculate.Click(MouseButtons.Left);
                    Wait(2000);
                    string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                    if (File.Exists(DefaultSymbologySourceNewFile))
                    {
                        RevertTTGenPref();
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
                Nirvana.Click(MouseButtons.Left);
                Nirvana.Click(MouseButtons.Right);
               // Minimize.Click(MouseButtons.Left);
                MinimizePTT();

                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                if (File.Exists(DefaultSymbologySourceNewFile))
                {
                    RevertTTGenPref();
                }
             }
        

            return _result;
        }
            return _result;
        }

       

       

       
    }
}
