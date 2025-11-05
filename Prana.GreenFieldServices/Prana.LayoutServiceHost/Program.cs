using System;
using System.Configuration;
using Prana.CoreService.Interfaces;
using Prana.LogManager;
using Prana.WindowsService.Common;

namespace Prana.LayoutServiceHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PranaServiceStarter.Run<ILayoutService>("PranaLayoutSvc", "Prana Layout Service", ConfigurationManager.AppSettings["ServiceDescription"]);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);

                Logger.HandleException(exp, LoggingConstants.POLICY_LOGONLY);
            }
        }
    }
}
