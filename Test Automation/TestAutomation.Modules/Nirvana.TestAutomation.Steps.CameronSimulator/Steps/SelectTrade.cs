using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.Simulator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Diagnostics;

namespace Nirvana.TestAutomation.Steps.Simulator
{
  public class SelectTrade : CameronSimulator,ITestStep
    {
      public BussinessObjects.TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
      {
          TestResult _res = new TestResult();
          try
          {
              Process[] processes = Process.GetProcesses();
              foreach (Process process in processes)
              {
                  if (process.MainWindowTitle.Contains("MS"))
                  {
                      IntPtr hWnd = process.MainWindowHandle;
                      ExtentionMethods.ShowWindow(hWnd, 9);
                  }
              }
              AccessBridgeHelper.Inititalize();
              BringSimToFront();
              ExtentionMethods.SwitchToWindowTitle("MS");

			// Step name will not always be select trade.
              AccessBridgeHelper.SendMessage(CameronConstants.gridCommand,GetTradeIndex(testData.Tables[0]));
              _res.IsPassed = true;
          }
          catch (Exception ex)
          {
              _res.ErrorMessage = ex.Message;
              _res.IsPassed = false;
              bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
              if (rethrow)
                  throw;
          }
          return _res;
      }
    }
}
