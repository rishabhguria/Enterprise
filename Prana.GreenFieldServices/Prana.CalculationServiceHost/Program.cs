using Prana.CoreService.Interfaces;
using Prana.LogManager;
using Prana.WindowsService.Common;
using System;
using System.Configuration;

namespace Prana.CalculationServiceHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PranaServiceStarter.Run<ICalculationService>("PranaCalculationSvc", "Prana Calculation Service", ConfigurationManager.AppSettings["ServiceDescription"]);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);

                Logger.HandleException(exp, LoggingConstants.POLICY_LOGONLY);
            }
        }
    }
}
