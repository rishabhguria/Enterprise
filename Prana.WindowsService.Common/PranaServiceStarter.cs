using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Prana.CoreService.Interfaces;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace Prana.WindowsService.Common
{
    public class PranaServiceStarter
    {
        private static IPranaServiceCommon pranaServiceCommon = null;
        internal static NativeMethods.PranaServiceConsoleCloseHandler consoleCloseHandler = null;
        static bool exitSystem = false;
        private const int argIndexCommand = 1;
        private const int argIndexServiceName = 2;
        private const int argIndexServiceDisplayName = 3;
        private const int STD_INPUT_HANDLE = -10;
        private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;

        public async static void Run<TPranaService>(string serviceName, string serviceDisplayName, string serviceDescription) where TPranaService : IPranaServiceCommon
        {
            try
            {
                //To Handle Exceptions
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                DisbleQuickEditMode();

                if (Environment.UserInteractive)
                {
                    string[] args = Environment.GetCommandLineArgs();

                    serviceName = args.ElementAtOrDefault(argIndexServiceName) ?? serviceName;
                    serviceDisplayName = args.ElementAtOrDefault(argIndexServiceDisplayName) ?? serviceDisplayName;

                    switch (args.ElementAtOrDefault(argIndexCommand))
                    {
                        case "-i":
                        case "-install":
                            Console.WriteLine("Installing {0}", serviceName);
                            PranaWindowsServiceInstaller.Install(serviceName, serviceDisplayName, serviceDescription);
                            break;
                        case "-u":
                        case "-uninstall":
                            Console.WriteLine("Uninstalling {0}", serviceName);
                            PranaWindowsServiceInstaller.Uninstall(serviceName, serviceDisplayName, serviceDescription);
                            break;
                        default:
                            Console.Title = serviceDisplayName + " - v" + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
                            consoleCloseHandler += new NativeMethods.PranaServiceConsoleCloseHandler(ConsoleCtrlCheck);
                            NativeMethods.SetConsoleCtrlHandler(consoleCloseHandler, true);
                            Console.CancelKeyPress += new ConsoleCancelEventHandler((object o, ConsoleCancelEventArgs args1) => { args1.Cancel = true; }); //Disable Ctrl+C and Ctrl+Break
                            await HostService<TPranaService>();

                            while (!exitSystem)
                                System.Threading.Thread.Sleep(500);
                            break;
                    }
                }
                else
                {
                    //Thread.Sleep(20000); //Give a chance to attach and debug.
                    ServiceBase.Run(new PranaWindowsService<TPranaService> { ServiceName = serviceName });
                }
            }
            catch (Exception ex)
            {
                if (Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW)) throw;
            }
        }

        /// <summary>
        /// Disable QuickEditMode to stop Command Prompt session getting paused due to Unintended clicks
        /// </summary>
        public static void DisbleQuickEditMode()
        {
            try
            {
                IntPtr hStdin = NativeMethods.GetStdHandle(STD_INPUT_HANDLE);
                uint mode = 0;

                NativeMethods.GetConsoleMode(hStdin, out mode);
                mode &= ~ENABLE_QUICK_EDIT_MODE;
                NativeMethods.SetConsoleMode(hStdin, mode);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static bool ConsoleCtrlCheck(NativeMethods.CtrlType closeSignal)
        {
            try
            {
                if (pranaServiceCommon != null)
                {
                    pranaServiceCommon.CleanUp();
                }
                exitSystem = true;
                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
                if (Logger.HandleException(exp, LoggingConstants.POLICY_LOGONLY)) throw;
                return false;
            }
        }

        internal async static System.Threading.Tasks.Task<IPranaServiceCommon> HostService<TPranaService>() where TPranaService : IPranaServiceCommon
        {
            try
            {
                Console.WriteLine("Hosting service.");

                IWindsorContainer container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

                // Initializing logging so that we don't miss any logs even exception occured in service hosting
                Logger.Initialize(container);
                Logger.LogMsg(LoggerLevel.Information, "Logger Initialized...");

                LogAppConfiguration();                     

                // Initializing DatabaseManager
                DatabaseManager.DatabaseManager.Initialize(container);

                if (!PranaServiceHost.IsServiceHosted())
                {
                    TPranaService pranaServerService = (TPranaService)container.Resolve<IPranaServiceCommon>();
                    pranaServiceCommon = (TPranaService)pranaServerService;
                    PranaServiceHost.HostPranaService(pranaServerService);
                    if (await pranaServiceCommon.InitialiseService(container))
                        Console.WriteLine("Successfully hosted service.");
                }

                return pranaServiceCommon;
            }
            catch (Exception exp)
            {
                exitSystem = true;
                if (Logger.HandleException(exp, LoggingConstants.POLICY_LOGANDTHROW)) throw;
                return null;
            }
        }

        /// <summary>
        /// Logs the application configuration details.
        /// </summary>
        private static void LogAppConfiguration()
        {
            try
            {
                string sectionTexts = LogExtensions.GetStartUpConfigurations();
                if (!string.IsNullOrWhiteSpace(sectionTexts))
                {
                    var propertyContext = Logger.PushProperty(LoggingConstants.PROPERTY_FILE_NAME, LoggingConstants.START_UP_CONFIG_FILE_NAME);

                    if (propertyContext != null && !(propertyContext is DefaultDispoable))
                    {
                        using (propertyContext)
                            Logger.LogToFile(sectionTexts);
                    }
                    else
                    {
                        Logger.LoggerWrite(sectionTexts, LoggingConstants.CATEGORY_START_UP_CONFIG);
                    }
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

        #region Unhandled Exceptions
        /// <summary>
        /// The .NET framework provides a lower-level event for global exception handling: AppDomain.UnhandledException. This event fires when there's an unhandled exception in any thread, 
        /// and in any type of application (with or without a user interface). However, while it offers a good last-resort mechanism for logging untrapped exceptions, it provides no means of 
        /// preventing the application from shutting down – and no means to suppress the .NET unhandled exception dialog.
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

                if (pranaServiceCommon != null)
                {
                    pranaServiceCommon.CleanUp();
                }
                ///Here if the exception is caught it will be handled by the catch and it will log it 
            }
            catch (Exception ex)
            {
                exitSystem = true;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                if (Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY))
                {
                    throw;
                }
            }
            finally
            {
                GC.Collect();
                Environment.Exit(1199);
            }
        }
        #endregion
    }
}
