using Prana.CoreService.Interfaces;
using Prana.LogManager;
using Prana.WindowsService.Common;
using Prana.CommonDataCache;
using System;
using System.Configuration;
using Castle.Core.Resource;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor;

namespace Prana.ExpnlServiceHost
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
                PranaServiceStarter.Run<IExpnlService>("PranaExpnlSvc", "Prana Expnl Service", ConfigurationManager.AppSettings["ServiceDescription"]);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);

                Logger.HandleException(exp, LoggingConstants.POLICY_LOGONLY);
            }
        }
    }
}
