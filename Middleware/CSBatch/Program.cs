using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Historical.Pricing.Library;
using Nirvana.Middleware;
using Nirvana.Middleware.Exceptions;
using Nirvana.Middleware.Linq;

namespace CSBatch
{
    /// <summary>
    /// Nirvana Middleware Services
    /// </summary>
    /// <remarks></remarks>
    class Program //: UTCalculator
    {
        static Int32 PrintCounter = 0;

        static Int32 MessageIdentity = 0;
        /// <summary>
        /// Main Data Context
        /// </summary>
        static NirvanaDataContext ParentContext = null;
        /// <summary>
        /// App Mutex
        /// </summary>
        static Mutex mutex = null;
        /// <summary>
        /// App Log File
        /// </summary>
        const string AppLog = ".\\CSBatch.log";
        /// <summary>
        /// Error Log File
        /// </summary>
        const string ErrLog = ".\\CSError.log";
        /// <summary>
        /// Filter Log File
        /// </summary>
        const string FilterLog = ".\\CSFilter.log";
        /// <summary>
        /// Writer Lock
        /// </summary>
        readonly static object writer = new object();

        static Arguments Arguments = new Arguments();

        /// <summary>
        /// Inits the log file.
        /// </summary>
        /// <remarks></remarks>
        static void InitLogFile()
        {
            try
            {
                File.WriteAllText(AppLog, "");
                File.WriteAllText(ErrLog, "");
                File.WriteAllText(FilterLog, "");
                File.WriteAllText(NirvanaDataContext.LINQToSQLLog, "");
                NirvanaDataContext.ClearLinqLogs();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        /// <remarks></remarks>
        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Main Entry Point (Console Application)
        /// </summary>
        /// <param name="args">The args.</param>
        /// <remarks></remarks>
        static void Main(string[] args)
        {
            try
            {
                //if we run this from a service or agent, we need to make sure the working directory points to the correct config file
                string exe = Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName;

                Directory.SetCurrentDirectory(AssemblyDirectory);
                

                //lets prevent accidental execution against same database (applies to current machine)
                ConnectionStringParser parser = new ConnectionStringParser(NirvanaDataContext.SourceConnectString);

                mutex = new Mutex(false, "Global\\" + parser);
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        string contents = string.Format("{0} A Mutex is already in use for {1}\n", DateTime.Now, parser);
                        Console.WriteLine(contents);
                        File.AppendAllText(".\\Mutex.log", contents);
                        System.Threading.Thread.Sleep(10000);
                        Environment.Exit(-1);
                    }
                }
              
                InitLogFile();
                using (var db = new NirvanaDataContext())
                {
                    ParentContext = db;
                    try
                    {
                        Arguments.Parse(args);
                        File.AppendAllText(FilterLog, Arguments.FundIDSymbol);
                    }
                    catch (Exception ex)
                    {
                        Program.ErrorPrint(ex);
                        Program.Exit(-1);
                    }
                   
                    if (NirvanaDataContext.CreateMiddlewareTables)
                    {
                        db.Connection.Open();
                        db.CreateTables();
                    }
                  
                    DisplayConfig();
                  
                    using (var builder = new InvokeBuilder())
                    {
                        Batch.Arguments = Arguments;
                        int result = Batch.Start(Arguments.From, Arguments.To);
                        if (result == 0)
                        {
                            for (int i = 5; i >= 1; i--)
                            {
                                DebugPrint("Auto Exit in T - {0}", i);
                                System.Threading.Thread.Sleep(5000);
                            }
                            Exit(result);
                        }
                        Exit(result);
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("Error.log", string.Format("{0}\n\n", ex));
                if (ex.InnerException != null)
                {
                    File.WriteAllText("Error.log", string.Format("{0}", ex.InnerException));
                }
                Exit(-11); ;
            }
        }

        /// <summary>
        /// Displays the config.
        /// </summary>
        /// <remarks></remarks>
        static void DisplayConfig()
        {

            DebugPrint("Execution Directory {0}\n", Directory.GetCurrentDirectory());

            DebugPrint("Batch Middleware Version        {0} - {1}\n",
                Assembly.GetEntryAssembly().GetName().Version,
                File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location));

            DebugPrint("Machine Name:                   {0}", Environment.MachineName);
            DebugPrint("OS Version:                     {0}", Environment.OSVersion.VersionString);
            DebugPrint("User Interactive:               {0}", Environment.UserInteractive);
            DebugPrint("Processor Count:                {0}", Environment.ProcessorCount);
            DebugPrint("64 Bit Process                  {0}\n", Environment.Is64BitProcess);

            DebugPrint("SQL Processor Time OnStart      {0}\n", GetAppProcessorTime("sqlservr"));

            ConnectionStringParser parser = new ConnectionStringParser(NirvanaDataContext.SourceConnectString);
            DebugPrint("Source:      {0}", parser);

            parser = new ConnectionStringParser(NirvanaDataContext.DestConnectString);
            DebugPrint("Destination: {0}", parser);

            parser = new ConnectionStringParser(NirvanaDataContext.HistoricalConnectString);
            DebugPrint("Historical:  {0}\n", parser);

            DebugPrint("Command Timeout                 {0} ", NirvanaDataContext.ConnectTimeout);
            DebugPrint("Configured Days to Roll Back is {0} ", Math.Abs(Arguments.RollBackDays));
            DebugPrint("Average Daily Volume Days is    {0}", NirvanaDataContext.AverageVolumeDays);
            DebugPrint("Lookback Days (Derived Data)    {0}", NirvanaDataContext.LookBackDays);
            DebugPrint("Step Derived Data Enabled is    {0}", NirvanaDataContext.DerivedDataEnabled);
            DebugPrint("Step Daily Returns Enabled is   {0}", NirvanaDataContext.DailyReturnsEnabled);
            DebugPrint("Auto Create Middleware Tables   {0}", NirvanaDataContext.CreateMiddlewareTables);
            DebugPrint("");
            DebugPrint("Save Mode is                    {0}", Arguments.SaveEnabled);
            DebugPrint("LINQ to SQL Logging             {0}", NirvanaDataContext.EnableLINQLogging);
            DebugPrint("Function Cores                  {0}", Arguments.FunctionCores);
            DebugPrint("System Cores                    {0}", Arguments.Cores);
            DebugPrint("Recover Mode                    {0}", Arguments.Recovery);
            DebugPrint("From Date                       {0}", Arguments.From.ShortDate());
            DebugPrint("To Date                         {0}", Arguments.To.ShortDate());
            DebugPrint("Step                            {0}", Arguments.Step);
            DebugPrint("Touch Setup                 {0}", Arguments.TouchStep);
            DebugPrint("");

            DebugPrint("Initalizing Session...");
            System.Threading.Thread.Sleep(5000);
        }


        /// <summary>
        /// Shows the progress.
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <param name="start">The start.</param>
        /// <param name="items">The items.</param>
        /// <param name="sessions">The sessions.</param>
        /// <remarks></remarks>
        static void ShowProgress(int counter, DateTime start, int items, int sessions)
        {
            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - start.Ticks);
            Debug.Print("{0} of {1} records {2}", counter, items, span);
        }
        /// <summary>
        /// Processes the functions parallel.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="items">The items.</param>
        /// <remarks></remarks>
        public static void ProcessFunctionsParallel(NirvanaDataContext db, List<Nirvana.Middleware.Linq.T_MW_DerivedData> items)
        {
            var option = new ParallelOptions() { MaxDegreeOfParallelism = Arguments.FunctionCores };
            var start = DateTime.Now;
            var Sessions = new List<NirvanaDataContext>();
            int counter = 0;


            DebugPrint("Executing Parallel Functions");

            Parallel.ForEach
                  (items, option, item =>
                  {
                      ShowProgress(counter++, start, items.Count(), Sessions.Count());
                      item.Entity = item.Entity.Replace(",", " "); //parser will fail if there is a , in the string

                      InvokeExecute(db, item);
                  }
            );
        }
        /// <summary>
        /// Handles the StateChange event of the Connection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Data.StateChangeEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        static void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            DebugPrint("Connection State Changed from {0} to {1}", e.OriginalState.ToString(), e.CurrentState.ToString());
        }
        /// <summary>
        /// Invokes the execute.
        /// </summary>
        /// <param name="db">The db context.</param>
        /// <param name="item">The item.</param>
        /// <remarks></remarks>
        static void InvokeExecute(NirvanaDataContext db, Nirvana.Middleware.Linq.T_MW_DerivedData item)
        {
            var cmd = InvokeBuilder.cmds.Where(w => w.DataType == item.DataType).SingleOrDefault();

            Type reflectOb = typeof(Functions);
            MethodInfo method = reflectOb.GetMethod(cmd.CSFunction.Replace("Functions.", ""));
            if (method == null) throw new MethodNotFoundException(cmd.CSFunction);

            object[] reflectParms = GetParamters(db, item, cmd);

            if (reflectParms == null)
                throw new KeyNotFoundException(cmd.CSFunction);


            object value = method.Invoke(reflectOb, reflectParms);
            if (value != null)
            {


                Double result;
                Double.TryParse(value.ToString(), out result);
                item.Value = result;

                if (item.Value != null)
                {
                    Debug.Assert(double.IsNaN((double)item.Value) == false);
                    Debug.Assert(double.IsInfinity((double)item.Value) == false);
                }
            }

        }
        /// <summary>
        /// Gets the paramters.
        /// </summary>
        /// <param name="db">The db context.</param>
        /// <param name="item">The item.</param>
        /// <param name="cmd">The CMD.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static object[] GetParamters(NirvanaDataContext db, Nirvana.Middleware.Linq.T_MW_DerivedData item, InvokeCommand cmd)
        {
            switch (cmd.ParamFlag)
            {
                case 14: return new object[] { db, item.CalculatedToDate, item.Groupfield, item.Entity };
                case 15: return new object[] { db, item.CalculatedFromDate, item.CalculatedToDate, item.Groupfield, item.Entity };
                case 30: return new object[] { db, item.CalculatedToDate, item.Groupfield, item.Entity, cmd.Direction };
                case 31: return new object[] { db, item.CalculatedFromDate, item.CalculatedToDate, item.Groupfield, item.Entity, cmd.Direction };
                case 47: return new object[] { db, item.CalculatedFromDate, item.CalculatedToDate, item.Groupfield, item.Entity, cmd.Constant };
                case 94: return new object[] { db, item.CalculatedToDate, item.Groupfield, item.Entity, cmd.Constant, cmd.Direction };
                case 95: return new object[] { db, item.CalculatedFromDate, item.CalculatedToDate, item.Groupfield, item.Entity, cmd.Constant, cmd.Direction };
                default: return null;
            }
        }
        /// <summary>
        /// Error Print to Log and Console
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ErrorPrint(Exception ex)
        {
            lock (writer)
            {
                File.AppendAllText(ErrLog, String.Format("{0}\t{1}\n\n\r\n", DateTime.Now, ex));
                Console.WriteLine(string.Format("{0}", ex));
                return ex.Message;
            }
        }
        /// <summary>
        /// Debug Print to Log and Console
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DebugPrint(string format, params object[] args)
        {
            try
            {
                lock (writer)
                {
                    string buffer = format;

                    if (args.Length > 0)
                        buffer = string.Format(format, args);

                    File.AppendAllText(AppLog, String.Format("{0}\t{1}\t{2}\r\n", ++MessageIdentity, DateTime.Now, buffer));
                    Console.WriteLine(buffer);

                    PrintCounter = 0;
                    // if (writer == null) return buffer;              
                    // writer.WriteLine(String.Format("{0}\t{1}", DateTime.Now, buffer));                
                    // writer.Flush();
                    return buffer;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Print Counter is {0}\r\n", PrintCounter);

                if (PrintCounter > 100)
                {
                    return Program.ErrorPrint(ex);
                }
                else
                {
                    PrintCounter++;
                    System.Threading.Thread.Sleep(10);
                    return DebugPrint(format, args);
                }


            }
        }
        /// <summary>
        /// Gets the app processor time.
        /// </summary>
        /// <param name="appName">Name of the app.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetAppProcessorTime(string appName)
        {
            //bool done = false;
            PerformanceCounter total_cpu = new PerformanceCounter("Process", "% Processor Time", "_Total");
            PerformanceCounter process_cpu = new PerformanceCounter("Process", "% Processor Time", appName);
            //while (!done)
            //{
            float t = total_cpu.NextValue();
            float p = process_cpu.NextValue();
            return (String.Format("_Total = {0}  App = {1} {2}%\n", t, p, p / t * 100));
            //System.Threading.Thread.Sleep(1000);
            //}
        }
        /// <summary>
        /// Exits the specified exit code.
        /// </summary>
        /// <param name="ExitCode">The exit code.</param>
        /// <remarks></remarks>
        public static void Exit(int ExitCode)
        {
            try
            {
                ParentContext.Dispose();

                NirvanaDataContext.MergeLogs();

                NirvanaDataContext ds = new NirvanaDataContext();
                using (HistoricalsDataContext db = new HistoricalsDataContext(NirvanaDataContext.HistoricalConnectString))
                {
                    MailSettings settings = new MailSettings();
                    var mail = db.MailSettings.Where(w => w.ID == NirvanaDataContext.MailIdentity).SingleOrDefault();

                    lock (writer)
                    {
                        settings.Attachments.Add(AppLog);
                        settings.Attachments.Add(ErrLog);
                        settings.Attachments.Add(FilterLog);
                        if (File.Exists(NirvanaDataContext.LINQToSQLLog)) settings.Attachments.Add(NirvanaDataContext.LINQToSQLLog);
                    }
                    settings.From = mail.MailFrom;
                    settings.To.AddRange(mail.MailTo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList());

                    settings.Smtp = mail.MailSmtp;
                    settings.Port = mail.MailPort;

                    settings.Subject = string.Format(mail.MailSubject, ds.Connection.Database) + ((ExitCode == 0) ? " Success" : " Failed ");
                    settings.Body = mail.MailBody;

                    settings.User = mail.MailUser;
                    settings.Password = mail.MailPassword;
                    Mail.Send(settings);
                }
                NirvanaDataContext.ClearLinqLogs();
                Environment.Exit(ExitCode);
            }
            catch (Exception ex)
            {
                File.WriteAllText("Error.log", string.Format("{0}\n\n", ex));
                if (ex.InnerException != null)
                {
                    File.WriteAllText("Error.log", string.Format("{0}", ex.InnerException));
                }
                Environment.Exit(ExitCode);
            }

        }
    }
}