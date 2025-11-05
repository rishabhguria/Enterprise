using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana
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

                //To Handle Exceptions
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                Application.EnterThreadModal += new EventHandler(Application_EnterThreadModal);
                Application.LeaveThreadModal += new EventHandler(Application_LeaveThreadModal);

                // Initializing logging
                Logger.Initialize(container);

                LogAppConfiguration();

                // Initializing DatabaseManager
                DatabaseManager.DatabaseManager.Initialize(container);

                WindsorContainerManager.Container = container;

                PranaMain form = (PranaMain)container[typeof(PranaMain)];
                form.SetContainer(container);
                form.SetAppArgs(args);
                form.LoadNirvanaForm();

                Application.Run(form);
            }
            catch (Exception ex)
            {
                //We are using try catch in catch block because in case of POLICY_LOGANDSHOW take time or not able to throw exception
                try
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(new Exception("Error initializing application", ex), LoggingConstants.POLICY_LOGANDSHOW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                catch
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error initializing application");
                }
            }
        }

        private static void Application_EnterThreadModal(object sender, EventArgs e)
        {
            IsModalDialogOpen = true;
        }

        private static void Application_LeaveThreadModal(object sender, EventArgs e)
        {
            IsModalDialogOpen = false;
        }

        public static bool IsModalDialogOpen { get; private set; }

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
                Logger.HandleException(ex.Exception, LoggingConstants.POLICY_LOGONLY);
                throw new Exception(ex.Exception.Source.ToString(), ex.Exception);
                //Here if the exception is caught it will be handled by the catch and it will log it 
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
            //string formattedInfo = ex.Exception.StackTrace.ToString();
            //Logger.LoggerWrite(formattedInfo, Prana.Global.Common.LOG_CATEGORY_EXCEPTION, 1,     1, System.Diagnostics.TraceEventType.Error, FORM_NAME);

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
                //Here if the exception is caught it will be handled by the catch and it will log it 
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
            //MessageBox.Show(sender.ToString() + " " + "Unhandled");

        }
        #endregion

        /// <summary>
        /// Logs the application configuration details.
        /// </summary>
        private static void LogAppConfiguration()
        {
            try
            {
                string sectionTexts = LogExtensions.GetStartUpConfigurations();
                if (!string.IsNullOrEmpty(sectionTexts))
                {
                    Logger.LoggerWrite(sectionTexts, LoggingConstants.CATEGORY_START_UP_CONFIG);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
