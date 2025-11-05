using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.PricingService2UI
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
                //To Handle Exceptions
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                IWindsorContainer container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

                // Initializing logging
                Logger.Initialize(container);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new PricingService2UI());
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #region Unhandled Exceptions
        /// <summary>
        /// The Application.ThreadException event fires when an exception is thrown from code that was ultimately called as a result of a Windows message (for example, a keyboard, mouse or "paint" message) – in short, nearly all code in a typical Windows Forms application. While this works perfectly, it lulls one into a false sense of security – that all exceptions will be caught by the central exception handler. Exceptions thrown on worker threads are a good example of exceptions not caught by Application.ThreadException (the code inside the Main method is another – including the main form's constructor, which executes before the Windows message loop begins).
        /// http://www.albahari.com/threading/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs ex)
        {
            try
            {
                throw new Exception("Caught Unhandled Exception", ex.Exception);
                ///Here if the exception is caught it will be handled by the catch and it will log it 
            }
            catch (Exception e)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(e, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                GC.Collect();
            }
        }


        /// <summary>
        /// The .NET framework provides a lower-level event for global exception handling: AppDomain.UnhandledException. This event fires when there's an unhandled exception in any thread, and in any type of application (with or without a user interface). However, while it offers a good last-resort mechanism for logging untrapped exceptions, it provides no means of preventing the application from shutting down – and no means to suppress the .NET unhandled exception dialog.
        /// http://www.albahari.com/threading/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                string formattedInfo = "Caught unhandled. IsTerminating : " + e.IsTerminating + " " + e.ExceptionObject.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "Program");
                ///Here if the exception is caught it will be handled by the catch and it will log it 
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                GC.Collect();
                Application.Exit();
            }
        }
        #endregion
    }
}
