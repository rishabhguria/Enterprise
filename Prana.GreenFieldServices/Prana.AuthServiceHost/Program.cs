using Prana.CoreService.Interfaces;
using Prana.LogManager;
using Prana.WindowsService.Common;
using System;
using System.Configuration;

namespace Prana.AuthServiceHost
{
    internal class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                PranaServiceStarter.Run<IAuthService>("PranaAuthSvc", "Prana Authentication Service", ConfigurationManager.AppSettings["ServiceDescription"]);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
                Logger.HandleException(exp, LoggingConstants.POLICY_LOGONLY);
            }
        }
    }
}
