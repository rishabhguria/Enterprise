using Castle.Core.Resource;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor;
using Prana.CoreService.Interfaces;
using Prana.LogManager;
using Prana.CommonDataCache;
using Prana.WindowsService.Common;
using System;
using System.Configuration;

namespace Prana.TradeServiceHost
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
                PranaServiceStarter.Run<ITradeService>("PranaTradeSvc", "Prana Trade Service", ConfigurationManager.AppSettings["ServiceDescription"]);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);

                Logger.HandleException(exp, LoggingConstants.POLICY_LOGONLY);
            }
        }
    }
}