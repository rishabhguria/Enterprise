using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.MonitoringServices
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IWindsorContainer container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

            // Initializing logging
            Logger.Initialize(container);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MonitoringMainForm());
        }
    }
}