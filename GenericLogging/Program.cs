using System;
using System.Text;
using System.Messaging;
using GenericLogging.ApplicationConstants;
using GenericLogging.Utility;
using Prana.BusinessObjects;
using System.IO;
using System.Threading;

namespace GenericLogging
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            foreach (var qName in ApplicationConstant.QueueName.Split(','))
            {
                var dtaLogger = new DataLogger(qName);
                dtaLogger.BeginWork();
            }
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine(ApplicationConstant.ToContinue);
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
