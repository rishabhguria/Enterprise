using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Configuration;


namespace Nirvana.TestAutomation.Utilities
{
    public static class AccessBridgeHelper
    {
        private static IAccessBridgeService service;
        private static Process process;
        public static void Inititalize()
        {
            try
            {
                Process[] pname = Process.GetProcessesByName("Nirvana.TestAutomation.AccessBridgeApp");
                foreach (Process p in pname)
                {
                    p.Kill();
                }

                process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = Application.StartupPath + "\\Nirvana.TestAutomation.AccessBridgeApp.exe";
                process.Start();
                var pipeFactory = new ChannelFactory<IAccessBridgeService>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/AccessBridgeApp"));

                service = pipeFactory.CreateChannel();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }



        public static void SendMessage(string commandType, string message)
        {
            ExtentionMethods.SwitchToWindowTitle("MS");
            int maxRetries = int.Parse(ConfigurationManager.AppSettings["AccessBridgeRetry"].ToString());
            int delayBetweenRetriesMs = TestDataConstants.COMPLIANCE_PROCESS_WAIT;

            for (int retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    var pipeFactory = new ChannelFactory<IAccessBridgeService>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/AccessBridgeApp"));
                    service = pipeFactory.CreateChannel();
                    service.SendMessage(commandType, message);
                    UIMap.Wait(1000);
                    return;
                }
                catch (Exception ex)
                {
                    bool isLastRetry = (retry == maxRetries);
                    if (isLastRetry)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                        if (rethrow)
                            throw;
                    }

                    else
                    {
                        Console.WriteLine("Retry {" + (retry + 1) + "}/{" + maxRetries + "}: {" + ex.Message);
                        Thread.Sleep(delayBetweenRetriesMs);

                    }

                }

            }
        }


    }
}