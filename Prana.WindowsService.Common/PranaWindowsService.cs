using Prana.CoreService.Interfaces;
using Prana.LogManager;
using System;
using System.ServiceProcess;

namespace Prana.WindowsService.Common
{
    //Inspired by http://www.devopsonwindows.com/build-windows-service-framework/
    public class PranaWindowsService<TPranaService> : ServiceBase where TPranaService : IPranaServiceCommon
    {
        private static IPranaServiceCommon pranaServiceCommon = null;
        protected override async void OnStart(string[] args)
        {
            try
            {
                //Thread.Sleep(20000); // Sleep for 20 seconds. Oppurtunity to attach and debug.
                pranaServiceCommon = await PranaServiceStarter.HostService<TPranaService>();
            }
            catch (Exception exp)
            {
                ExitCode = 1199; // Non zero Exit code means error.
                if (Logger.HandleException(exp, LoggingConstants.POLICY_LOGANDTHROW)) throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (pranaServiceCommon != null)
                {
                    pranaServiceCommon.CleanUp();
                }
                ExitCode = 0; // Zero Exit code means clean stop.
            }
            catch (Exception exp)
            {
                ExitCode = 1199; // Non zero Exit code means error.
                if (Logger.HandleException(exp, LoggingConstants.POLICY_LOGANDTHROW)) throw;
            }
        }
    }
}
