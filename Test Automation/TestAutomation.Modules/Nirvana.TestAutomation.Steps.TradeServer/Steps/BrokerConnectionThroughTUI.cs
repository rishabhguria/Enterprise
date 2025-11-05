using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Diagnostics;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;



namespace Nirvana.TestAutomation.Steps.TradeServer
{
    class BrokerConnectionThroughTUI : TradeServerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        { 
            TestResult _result = new TestResult();
            try
            {
                ProcessStartInfo TUI = new ProcessStartInfo();
                TUI.FileName = "Prana.TradeServiceUI.exe";
                TUI.WorkingDirectory = ApplicationArguments.TradeServiceUIPath;
                Process TUIProc = new Process();
                TUIProc.StartInfo = TUI;
                TUIProc.Start();
                Thread.Sleep(7000);
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string broker = dr["Broker"].ToString().ToUpper();
                    string connection = dr["Connect/Disconnect"].ToString().ToUpper();
                    if (broker.Equals("MS"))
                    {
                        if (BtnConnect1.Window.Text.ToString().ToUpper().Equals(connection))
                        {
                            BtnConnect1.Click(MouseButtons.Left);
                            Thread.Sleep(1000);
                            if (PranaWarning.IsVisible) {
                                ButtonYes1.Click(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            throw new Exception("You can only Write Connect or Disconnect in Connect/Disconnect column");
                        }
                    }
                    else if (broker.Equals("GS"))
                    {
                        if (BtnConnect2.Window.Text.ToString().ToUpper().Equals(connection))
                        {
                            BtnConnect2.Click(MouseButtons.Left);
                            Thread.Sleep(1000);
                            if (PranaWarning.IsVisible)
                            {
                                ButtonYes1.Click(MouseButtons.Left);
                            }
                        }
                        else
                        {
                            throw new Exception("You can only Write Connect or Disconnect in Connect/Disconnect column");
                        }
                    }
                    else
                    {
                        throw new Exception("There is no broker with name " + dr["Broker"]);
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally 
            {
                KeyboardUtilities.CloseWindow(ref TitleBar3);
            }
            return _result;
           
        }
    }
}
