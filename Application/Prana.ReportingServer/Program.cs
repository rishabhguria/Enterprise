using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using Prana.AutomationHandlers;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;


namespace Prana.ReportingServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                IWindsorContainer container =
                  new WindsorContainer(
                      new XmlInterpreter(new ConfigResource("castle")));
               
                ReportingServer form = (ReportingServer)container[typeof(ReportingServer)];
                form.SetContainer(container);    

                Application.Run(form);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}