using Prana.CoreService.Interfaces;
using Prana.LogManager;
using Prana.CommonDataCache;
using Prana.WindowsService.Common;
using System;
using System.Configuration;
using Castle.Windsor;
using Castle.Core.Resource;
using Castle.Windsor.Configuration.Interpreters;

namespace Prana.PricingService2Host
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                IWindsorContainer container =
                    new WindsorContainer(
                        new XmlInterpreter(new ConfigResource("castle")));
                WindsorContainerManager.Container = container;
                PranaServiceStarter.Run<IPricingService2>("PranaPricingSvc", "Prana Pricing Service", ConfigurationManager.AppSettings["ServiceDescription"]);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);

                Logger.HandleException(exp, LoggingConstants.POLICY_LOGONLY);
            }
        }
    }
}