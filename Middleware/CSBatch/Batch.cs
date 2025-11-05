using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Historical.Pricing.Library;
using Nirvana.Middleware;
using Nirvana.Middleware.Linq;

namespace CSBatch
{
    /// <summary>
    /// CSBatch Job
    /// </summary>
    /// <remarks></remarks>
    public class Batch
    {
        static Status _status = new Status();

        #region Variables

        static double SecondsRemaining = 0.0F;

        /// <summary>
        /// Thread Lock
        /// </summary>
        private readonly static Object locker = new object();

        /// <summary>
        /// Job Start Time
        /// </summary>
        private readonly static DateTime start = DateTime.Now;

        /// <summary>
        /// Gets or sets the blocks processed.
        /// </summary>
        /// <value>The blocks processed.</value>
        /// <remarks></remarks>
        private static UInt32 BlocksProcessed { get; set; }

        /// <summary>
        /// Gets or sets the records processed.
        /// </summary>
        /// <value>The records processed.</value>
        /// <remarks></remarks>
        private static UInt32 RecordsProcessed { get; set; }

        /// <summary>
        /// Gets or sets the days processed.
        /// </summary>
        /// <value>The days processed.</value>
        /// <remarks></remarks>
        private static Int32 DaysProcessed { get; set; }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        /// <remarks></remarks>
        public static Arguments Arguments { get; set; }

        #endregion

        /// <summary>
        /// Starts a parallel execution.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int Start(DateTime fromDate, DateTime toDate)
        {
            Console.Write(Client.Run().ToString());
            var option = new ParallelOptions() { MaxDegreeOfParallelism = Arguments.Cores };
            DateTime start = DateTime.Now;

            if (Arguments.Step <= 1)
            {
                Parallel.ForEach
                      (Arguments.Dates, option, date =>
                      {
                          int retries = 0;

                          ProcessDayCounters();
                          // ProcessStepCounters();
                          SetTitle();

                          Program.DebugPrint("{0} Processing Generics...", date.ShortDate());
                          while (true)
                          {
                              if (ProcessGenericPNL(date, date) == false)
                              {
                                  if (retries++ >= 3)
                                  {
                                      Program.DebugPrint("Step 1 Halted on {0} Generics. See above error.", date);
                                      Console.Beep();
                                      Program.Exit(-1);
                                  }

                                  Program.DebugPrint("{0} Resubmitting {1} Generics...", date.ShortDate(), retries);
                                  System.Threading.Thread.Sleep(1000);
                              }
                              else
                              {
                                  //ProcessStepCounters();
                                  retries = 0;
                                  break;
                              }
                          }

                          Program.DebugPrint("{0} Processing Transactions...", date.ShortDate());
                          while (true)
                          {
                              if (ProcessTransactions(date, date) == false)
                              {
                                  if (retries++ >= 3)
                                  {
                                      Program.DebugPrint("Step 2 Halted on {0} Transactions. See above error.", date);
                                      Console.Beep();
                                      Program.Exit(-2);
                                  }

                                  Program.DebugPrint("{0} Resubmitting {1} Transactions...", date.ShortDate(), retries);
                                  System.Threading.Thread.Sleep(1000);
                              }
                              else
                              {
                                  //ProcessStepCounters();
                                  retries = 0;
                                  break;
                              }
                          }

                      });
             SaveReportsDropDownFilters();

                if (Arguments.TouchStep)
                {
                    DateTime starttouch = DateTime.Now;
                    Program.DebugPrint("Processing Touch Step");

                    List<T_NT_GenericPNL_Insert> _GenericPNLs_Insert = new List<T_NT_GenericPNL_Insert>();
                    List<T_NT_Transaction_Insert> _Transactions_Insert = new List<T_NT_Transaction_Insert>();
                    List<T_NT_CashAccruals_Insert> _CashAccruals_Insert = new List<T_NT_CashAccruals_Insert>();
                    AutoMapper.Mapper.CreateMap<T_NT_GenericPNL, T_NT_GenericPNL_Insert>();
                    AutoMapper.Mapper.CreateMap<T_NT_Transaction, T_NT_Transaction_Insert>();
                    AutoMapper.Mapper.CreateMap<T_NT_CashAccruals, T_NT_CashAccruals_Insert>();
                    using (NirvanaDataContext db = new NirvanaDataContext())
                    {
                        //db.P_NT_HandleGenericPNL(fromDate, toDate, Arguments.FundIDSymbol);
                        //db.P_NT_HandleTransactions(fromDate, toDate, Arguments.FundIDSymbol);
                        Program.DebugPrint("Processing GenericPNL...");
                        List<T_NT_GenericPNL> _GenericPNLs = db.P_NT_GetGenericPNL(fromDate, toDate, Arguments.FundIDSymbol).ToList();
                        foreach (T_NT_GenericPNL _ in _GenericPNLs)
                        {
                            T_NT_GenericPNL_Insert _Insert = AutoMapper.Mapper.Map<T_NT_GenericPNL, T_NT_GenericPNL_Insert>(_);
                            byte[] _CheckSumId = _.ComputeHash();
                            _Insert.CheckSumId = _CheckSumId;
                            _GenericPNLs_Insert.Add(_Insert);
                        }
                        Program.DebugPrint("Processing Transactions...");
                        List<T_NT_Transaction> _Transactions = db.P_NT_GetTransactions(fromDate, toDate, Arguments.FundIDSymbol).ToList();
                        foreach (T_NT_Transaction _ in _Transactions)
                        {
                            T_NT_Transaction_Insert _Insert = AutoMapper.Mapper.Map<T_NT_Transaction, T_NT_Transaction_Insert>(_);
                            byte[] _CheckSumId = _.ComputeHash();
                            _Insert.CheckSumId = _CheckSumId;
                            _Transactions_Insert.Add(_Insert);
                        }
                        Program.DebugPrint("Processing CashAccurals...");
                        List<T_NT_CashAccruals> _CashAccruals = db.P_NT_GetCashAccruals(fromDate, toDate, Arguments.FundIDSymbol).ToList();
                        foreach (T_NT_CashAccruals _ in _CashAccruals)
                        {
                            T_NT_CashAccruals_Insert _Insert = AutoMapper.Mapper.Map<T_NT_CashAccruals, T_NT_CashAccruals_Insert>(_);
                            byte[] _CheckSumId = _.ComputeHash();
                            _Insert.CheckSumId = _CheckSumId;
                            _CashAccruals_Insert.Add(_Insert);
                        }
                        db.P_NT_DeleteGenericPNL(fromDate, toDate, Arguments.FundIDSymbol);
                        db.P_NT_DeleteTransactions(fromDate, toDate, Arguments.FundIDSymbol);
                        db.P_NT_DeleteCashAccruals(fromDate, toDate, Arguments.FundIDSymbol);
                    }
                    SQLExtensions.SQLBulkInsert(_GenericPNLs_Insert, "T_NT_GenericPNL", NirvanaDataContext.DestConnectString);
                    SQLExtensions.SQLBulkInsert(_Transactions_Insert, "T_NT_Transactions", NirvanaDataContext.DestConnectString);
                    SQLExtensions.SQLBulkInsert(_CashAccruals_Insert, "T_NT_CashAccruals", NirvanaDataContext.DestConnectString);

                    TimeSpan spantouch = new TimeSpan(DateTime.Now.Ticks - starttouch.Ticks);
                    Program.DebugPrint("Touch Step Completed. Duration {0}", spantouch);
                }
            }

            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - start.Ticks);
            Program.DebugPrint("Step 1 Completed. Duration {0}", span);

            bool Step2Enabled =
            global::Nirvana.Middleware.Linq.NirvanaDataContext.DerivedDataEnabled ||
            global::Nirvana.Middleware.Linq.NirvanaDataContext.DailyReturnsEnabled;

            //can't process in above iteration, because they are dependent on GenericPNL to be fully populated
            if (Step2Enabled)
            {
                DateTime start2 = DateTime.Now;

                //lets create the cache table for GenericPNL
                using (NirvanaDataContext db = new NirvanaDataContext())
                {
                    Program.DebugPrint("Generating Generic Cache...");
                    _status.SetCache("Running: " + DateTime.Now);
                    db.P_MW_CreateGenericCache();
                    _status.SetCache("Success: " + DateTime.Now);
                }

                DaysProcessed = 0;

                Parallel.ForEach(Arguments.Dates, option, date =>
                {
                    int retries = 0;

                    ProcessDayCounters();
                    SetTitle();

                    if (global::Nirvana.Middleware.Linq.NirvanaDataContext.DerivedDataEnabled)
                    {
                        Program.DebugPrint("{0} Processing Derived Data...", date.ShortDate());


                        while (true)
                        {
                            if (ProcessDerivedData(date, date) == false)
                            {
                                if (retries++ >= 3)
                                {
                                    Program.DebugPrint("Step 4 Halted on {0} Derived Data. See above error.", date);
                                    Console.Beep();
                                    Program.Exit(-4);
                                }
                                Program.DebugPrint("{0} Resubmitting {1} Derived Data...", date.ShortDate(), retries);
                                System.Threading.Thread.Sleep(1000);
                            }
                            else
                            {
                                retries = 0;
                                break;
                            }
                        }
                    }

                    if (global::Nirvana.Middleware.Linq.NirvanaDataContext.DailyReturnsEnabled)
                    {
                        Program.DebugPrint("{0} Processing Daily Returns...", date.ShortDate());
                        while (true)
                        {
                            if (ProcessDailyReturns(date, date) == false)
                            {
                                if (retries++ >= 3)
                                {
                                    Program.DebugPrint("Step 5 Halted on {0} Daily Returns. See above error.", date);
                                    Console.Beep();
                                    Program.Exit(-5);
                                }
                                Program.DebugPrint("{0} Resubmitting {1} Daily Returns...", date.ShortDate(), retries);
                                System.Threading.Thread.Sleep(1000);
                            }
                            else
                            {
                                retries = 0;
                                break;
                            }
                        }
                    }
                });

                span = new TimeSpan(DateTime.Now.Ticks - start2.Ticks);
                Program.DebugPrint("Step 2 Completed. Duration {0}", span);
            }

            Program.DebugPrint("Job Duration {0}\n", new TimeSpan(DateTime.Now.Ticks - start.Ticks));
            Program.DebugPrint(string.Format("{0} of {1} Days {2} of {3} Steps", DaysProcessed, Arguments.Dates.Count(), BlocksProcessed, Steps));
            Program.Exit(0);
            return 0;

        }
        /// <summary>
        /// Author: sachin Mishra
        /// Date -
        /// Purpose :Save all distinct reports filters in T_MW_PNLReportsDropDown/T_MW_TransactionReportsDropDown table from T_MW_GenericPNL/T_MW_Transactions table. 
        /// 
        /// </summary>
        private static void SaveReportsDropDownFilters()
        {
            try
            {
                DateTime start = DateTime.Now;
                Program.DebugPrint("{0} Processing Reports DropDown Filters...", start.ShortDate());
                using (NirvanaDataContext db = new NirvanaDataContext())
                {
                    var result = db.P_MW_UpdatePNLReportsDropDown();
                }
                Program.DebugPrint("{0} Saved Reports DropDown Filters ...", start.ShortDate());
            }
            catch (Exception ex)
            {
                _status.SetGeneric(start, "Error: " + ex.Message);
                Program.DebugPrint(string.Format("{0}", ex.Message));
                Program.ErrorPrint(ex);
            }





        }

        /// <summary>
        /// Processes the generic PNL.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessGenericPNL(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DateTime start = DateTime.Now;
                using (NirvanaDataContext db = new NirvanaDataContext())
                {
                    if (Arguments.Recovery)
                    {
                        if (db.T_MW_GenericPNLs.Where(w => w.Rundate == toDate).Count() > 0)
                        {
                            ProcessStepCounters();
                            return true;
                        }
                    }

                    _status.SetGeneric(toDate, "Running: " + DateTime.Now);

                    if (Arguments.SaveEnabled)
                    {
                        var GenericPNL = db.P_MW_GetGenericPNL(fromDate, toDate, Convert.ToInt32(!Arguments.SaveEnabled), Arguments.FundIDSymbol).ToList();
                        GenericPNL.ForEach(fe => { fe.Rundate = toDate; fe.Delta = 1.0d; fe.Beta = 1.0d; });

                        ProcessUnderlyingMarkPrices(db, GenericPNL, fromDate);
                        ProcessVolumeCalculations(GenericPNL, fromDate, NirvanaDataContext.AverageVolumeDays);
                        ProcessExposure(GenericPNL, fromDate, toDate);

                        db.P_MW_DeleteGenericPNL(fromDate, toDate, Arguments.FundIDSymbol);
                        SQLExtensions.SQLBulkInsert(GenericPNL, NirvanaDataContext.GenericPNLTable, NirvanaDataContext.DestConnectString);

                        ProcessRecordCounters(GenericPNL.Count());
                        GenericPNL.Clear();
                    }
                    else
                    {
                        //Program.DebugPrint("{0}", DateTime.Now.ToLongTimeString());
                        //var results = db.P_MW_GetGenericPNL(fromDate, toDate, Convert.ToInt32(!Arguments.SaveEnabled), Arguments.FundIDSymbol).ToList();
                        //Program.DebugPrint("{0}", DateTime.Now.ToLongTimeString());
                        db.P_MW_GetGenericPNL(fromDate, toDate, Convert.ToInt32(!Arguments.SaveEnabled), Arguments.FundIDSymbol).ToList();
                        //Program.DebugPrint("{0}", DateTime.Now.ToLongTimeString());
                        if (NirvanaDataContext.GetVedaPrices)
                        {
                            _status.SetVeda(toDate, "Running: " + DateTime.Now);
                            ProcessMarkPricesFromVEDA(db, fromDate, toDate);
                            _status.SetVeda(toDate, "Succcess: " + DateTime.Now);
                        }
                    }

                    ProcessStepCounters();

                    SetTitle();
                    _status.SetGeneric(toDate, "Success: " + DateTime.Now);
                }

                Program.DebugPrint("{0} Process Generics Duration {1}", fromDate.ShortDate(), new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());
                return true;
            }
            catch (Exception ex)
            {
                _status.SetGeneric(toDate, "Error: " + ex.Message);
                Program.DebugPrint(string.Format("{0}", ex.Message));
                Program.ErrorPrint(ex);
                return false;
            }
        }

        #region Generic Processes
        /// <summary>
        /// Requests the final mark prices.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <remarks></remarks>
        public static void RequestFinalMarkPrices(NirvanaDataContext db, DateTime fromDate, DateTime toDate)
        {
            // trap all the symbols with double mark prices and do not use them (see ticket PROD-44)

            var v = from p in db.PM_DayMarkPrices
                    where p.Date >= fromDate && p.Date <= toDate
                    group p by new { p.Symbol, p.Date } into g
                    where g.Count() > 1
                    select new { Symbol = g.Key.Symbol, Date = g.Key.Date };

            //int count = db.PM_DayMarkPrices.
            //    Where(w=> w.Date >= fromDate && w.Date <= toDate).GroupBy(g=> new { g.Symbol, g.Date }).
            //    Select(s=> new { Symbol = s.Key.Symbol, Date = s.Key.Date).Count();
        }

        /// <summary>
        /// Processes the volume calculations.
        /// </summary>
        /// <param name="GenericPNL">The generic PNL.</param>
        /// <param name="date">From date.</param>
        /// <param name="AvgVolume">The avg volume.</param>
        /// <remarks></remarks>
        public static void ProcessVolumeCalculations(List<T_MW_GenericPNL> GenericPNL, DateTime date, int AvgVolume = 30)
        {
            try
            {
                var start = DateTime.Now;

                using (HistoricalsDataContext server = new HistoricalsDataContext(NirvanaDataContext.HistoricalConnectString))
                {
                    var Averages = server.spGetAverageVolume(date, AvgVolume, 1);

                    var query = GenericPNL.Where(w => w.Rundate == date && w.Open_CloseTag.ToLower() != "c").
                        Join(Averages, pos => pos.Symbol, avg => avg.Symbol, (pos, avg) => new { position = pos, average = avg.AverageVolume });

                    foreach (var q in query)
                    {
                        if (q.position.BeginningQuantity == null || q.position.BeginningQuantity == 0 || q.average == null || q.average == 0)
                            continue;

                        q.position.AverageVolume = q.average;
                        q.position.AverageLiquidation = (Convert.ToDouble(q.position.SideMultiplier) * q.position.BeginningQuantity) / q.average;

                    }

                    var options = GenericPNL.Where(w => w.Rundate == date && w.Open_CloseTag.ToLower() != "c" && (w.Asset.ToLower().Contains("option") || w.Asset.ToLower().Contains("future")));
                    foreach (var option in options)
                    {
                        var value = server.DailyBars.FirstOrDefault(w => w.Symbol == option.Symbol && w.Date >= date);

                        if (value == null)
                        {
                            value = server.DailyBars.Where(w => w.Symbol == option.Symbol && w.Date <= date).OrderBy(o => o.Date).ToList().LastOrDefault();
                        }

                        if (value != null)
                        {
                            option.AverageVolume = value.Interest;
                            if (Convert.ToDouble(option.SideMultiplier) != 0 && option.BeginningQuantity != 0 && value.Interest != 0)
                            {
                                option.AverageLiquidation = (Convert.ToDouble(option.SideMultiplier) * option.BeginningQuantity) / value.Interest;
                            }
                        }
                    }

                }

                Program.DebugPrint("{0} Connecting to Historical Server for Volume Calculations {1}", date.ShortDate(), new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());
            }
            catch (Exception ex)
            {
                Program.DebugPrint(string.Format("{0}", ex.Message));
            }


        }

        /// <summary>
        /// Pricesses the mark prices from VEDA.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessMarkPricesFromVEDA(NirvanaDataContext db, DateTime fromDate, DateTime toDate)
        {
            try
            {
                Program.DebugPrint("Fetching Volume Calculations from VEDA");
                string recompile = NirvanaDataContext.ExecuteWithRecompile ? " WITH RECOMPILE" : "";

                string cmdSQL = string.Format("EXECUTE P_MW_GetDataFromVeda '{0}', '{1}', {2} {3}", fromDate, toDate, NirvanaDataContext.AverageVolumeDays, recompile);
                db.ExecuteCommand(cmdSQL);

                if (NirvanaDataContext.UseExternalCalcs)
                {
                    Program.DebugPrint("Updating Greeks and Exposure from VEDA");
                    cmdSQL = string.Format("EXECUTE P_MW_FillGreeksAndExposure '{0}', '{1}', 'Client' {2}", fromDate, toDate, recompile);
                    db.ExecuteCommand(cmdSQL);
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.DebugPrint(string.Format("{0}", ex.Message));
                Program.ErrorPrint(ex);
                return false;
            }
        }


        /// <summary>
        /// Processes the underlying mark prices.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="GenericPNL">The generic PNL.</param>
        /// <param name="date">From date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessUnderlyingMarkPrices(NirvanaDataContext db, List<T_MW_GenericPNL> GenericPNL, DateTime date)
        {


            var start = DateTime.Now;
            var query = GenericPNL.Where(w => w.Rundate == date).
                        Join(db.PM_DayMarkPrices.Where(w2 => w2.Date == date), pos => pos.UnderlyingSymbol, mrk => mrk.Symbol,
                            (pos, mrk) => new { position = pos, mark = mrk.FinalMarkPrice, key = mrk.Symbol, rundate = mrk.Date });


            foreach (var q in query)
            {
                q.position.UnderlyingSymbolPrice = q.mark;
            }

            ProcessUnderlyingMarkPricesServer(GenericPNL, date);

            #region old way, slower but shows more granularity
            ////var symbols = GenericPNL.
            ////         Where(w => string.IsNullOrEmpty(w.UnderlyingSymbol) == false && w.Open_CloseTag.ToLower() != "c").Select(s => s.UnderlyingSymbol).Distinct().ToList();


            ////using (HistoricalsDataContext hdb = new HistoricalsDataContext(NirvanaDataContext.HistoricalConnectString))
            ////{
            ////    double MarkPrice = 0;
            ////    //update underlying price
            ////    foreach (var symbol in symbols)
            ////    {

            ////        var localMark = db.PM_DayMarkPrices.Where(w => w.Symbol == symbol && w.Date >= date).FirstOrDefault();
            ////        if (localMark != null)
            ////            MarkPrice = localMark.FinalMarkPrice;
            ////        else
            ////        {
            ////            if (localMark == null)
            ////            {
            ////                var serverMark = hdb.DailyBars.Where(w => w.Symbol == symbol && w.Date >= date).OrderBy(o => o.Date).FirstOrDefault();
            ////                if (serverMark == null)
            ////                {
            ////                    //Program.DebugPrint("No Mark Price found for Underlying Symbol {0}", symbol);
            ////                    continue;
            ////                }
            ////                if (serverMark.SettlementPrice != null && serverMark.SettlementPrice != 0)
            ////                    MarkPrice = (double)serverMark.SettlementPrice * (double)serverMark.SplitRatio;
            ////                else
            ////                    MarkPrice = (double)serverMark.Close * (double)serverMark.SplitRatio;
            ////            }
            ////        }
            ////        var rows = GenericPNL.Where(w => w.UnderlyingSymbol == symbol && w.Open_CloseTag.ToLower() != "c").ToList();
            ////        if (rows != null && rows.Count > 0)
            ////        {
            ////            foreach (var row in rows)
            ////                row.UnderlyingSymbolPrice = MarkPrice;
            ////        }                   
            ////    }
            ////}
            #endregion

            Program.DebugPrint("{0} Process Underlying Mark Price Duration {1}", date.ShortDate(), new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());
            return true;
        }

        /// <summary>
        /// Processes the underlying mark prices from server.
        /// </summary>
        /// <param name="GenericPNL">The generic PNL.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessUnderlyingMarkPricesServer(List<T_MW_GenericPNL> GenericPNL, DateTime date)
        {
            try
            {
                using (HistoricalsDataContext hdb = new HistoricalsDataContext(NirvanaDataContext.HistoricalConnectString))
                {
                    var server = GenericPNL.Where(w => w.Rundate == date && (w.UnderlyingSymbolPrice == null || w.UnderlyingSymbolPrice == 0)).
                                 Join(hdb.DailyBars.Where(w2 => w2.Date == date), pos => pos.UnderlyingSymbol, mrk => mrk.Symbol,
                                    (pos, mrk) => new { position = pos, close = mrk.Close, key = mrk.Symbol, rundate = mrk.Date, SplitRatio = mrk.SplitRatio, Settlement = mrk.SettlementPrice });

                    foreach (var row in server)
                    {
                        if (row.position.UnderlyingSymbolPrice == null || row.position.UnderlyingSymbolPrice == 0)
                        {
                            if (row.Settlement != null && row.Settlement != 0)
                                row.position.UnderlyingSymbolPrice = row.Settlement * row.SplitRatio;
                            else
                                row.position.UnderlyingSymbolPrice = row.close * row.SplitRatio;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.ErrorPrint(ex);
                return false;
            }
        }
        /// <summary>
        /// Processes the exposure.
        /// </summary>
        /// 
        /// <param name="GenericPNL">The generic PNL.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessExposure(List<T_MW_GenericPNL> GenericPNL, DateTime fromDate, DateTime toDate)
        {
            var start = DateTime.Now;

            System.Diagnostics.TextWriterTraceListener myWriter = new System.Diagnostics.TextWriterTraceListener(System.Console.Out);

            Functions.UpdateBetas(GenericPNL, fromDate, toDate);

            myWriter.WriteLine("Betas End" + DateTime.Now);

            Functions.UpdateGreeks(GenericPNL, fromDate, toDate, NirvanaDataContext.InterestFreeRate, NirvanaDataContext.UseWinDaleToCalculateImpliedVol, NirvanaDataContext.UseUserPrefToCalculateImpliedVol);

            myWriter.WriteLine("Greeks End " + DateTime.Now);

            Program.DebugPrint("{0} Process Exposure Duration {1}", fromDate.ShortDate(), new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());

            return true;
        }
        #endregion

        /// <summary>
        /// Processes the transactions.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessTransactions(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DateTime start = DateTime.Now;
                using (NirvanaDataContext db = new NirvanaDataContext())
                {
                    if (Arguments.Recovery)
                    {
                        if (db.T_MW_Transactions.Where(w => w.RunDate == toDate).Count() > 0)
                        {
                            lock (locker)
                            {
                                ProcessStepCounters();
                                return true;
                            }
                        }
                    }

                    _status.SetTransactions(toDate, "Running: " + DateTime.Now);

                    if (Arguments.SaveEnabled)
                    {
                        var Transactions = db.P_MW_GetTransactions(fromDate, toDate, Convert.ToInt32(!Arguments.SaveEnabled), Arguments.FundIDSymbol).ToList();
                        Transactions.ForEach(fe => { fe.RunDate = toDate; });

                        db.P_MW_DeleteTransactions(fromDate, toDate, Arguments.FundIDSymbol);
                        SQLExtensions.SQLBulkInsert(Transactions, NirvanaDataContext.TransactionsTable, NirvanaDataContext.DestConnectString);
                        ProcessRecordCounters(Transactions.Count());
                        Transactions.Clear();
                    }
                    else
                    {
                        db.P_MW_GetTransactions(fromDate, toDate, Convert.ToInt32(!Arguments.SaveEnabled), Arguments.FundIDSymbol).ToList();
                    }

                    ProcessStepCounters();

                    SetTitle();
                    _status.SetTransactions(toDate, "Success: " + DateTime.Now);
                }

                Program.DebugPrint("{0} ProcessTransactions Duration {1}", fromDate.ShortDate(), new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());
                return true;
            }
            catch (Exception ex)
            {
                _status.SetTransactions(toDate, "Error: " + ex.Message);
                Program.DebugPrint(string.Format("{0}", ex.Message));
                Program.ErrorPrint(ex);
                return false;
            }
        }

        /// <summary>
        /// Processes the derived data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessDerivedData(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DateTime start = DateTime.Now;
                using (NirvanaDataContext db = new NirvanaDataContext())
                {
                    if (Arguments.Recovery)
                    {
                        if (db.GetTable<T_MW_DerivedData>().Select(s => s.CalculatedToDate).Where(w => w == toDate).Count() > 0)
                        {
                            lock (locker)
                            {
                                ProcessStepCounters();
                                return true;
                            }
                        }
                    }

                    //using WITH Recompile seems to fix the execution plan. The issue is related to parameter sniffing and cached procedure
                    _status.SetDerivedData(toDate, "Running: " + DateTime.Now);

                    string recompile = NirvanaDataContext.ExecuteWithRecompile ? " WITH RECOMPILE" : "";

                    string cmdSQL = string.Format("EXECUTE P_MW_GetDerivedData '{0}', '{1}', {2}, {3}, {4} {5}", fromDate, toDate, 1, 0, NirvanaDataContext.LookBackDays, recompile);

                    var DerivedData = db.ExecuteCommand(cmdSQL);

                    //var DerivedData = db.P_MW_GetDerivedData(fromDate, toDate, (int)Math.Abs(Convert.ToInt16(Arguments.SaveEnabled)), 0, NirvanaDataContext.LookBackDays ).Count();
                    //we fixed Stored Proc so we should be ok. lets proc do the work.
                    //Program.ProcessFunctionsParallel(db, DerivedData);                          

                    //if (Arguments.SaveEnabled == true)
                    //{
                    //    SQLExtensions.SQLBulkInsert(DerivedData, NirvanaDataContext.DerivedDataTable, NirvanaDataContext.DestConnectString);
                    //}

                    ProcessRecordCounters(DerivedData);
                    ProcessStepCounters();
                    SetTitle();
                    _status.SetDerivedData(toDate, "Success: " + DateTime.Now);
                }

                //Program.DebugPrint("{0} ProcessDerivedData Duration {1} / Cores", fromDate.ShortDate(), new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());
                return true;
            }
            catch (Exception ex)
            {
                _status.SetDerivedData(toDate, "Error: " + ex.Message);
                Program.DebugPrint(string.Format("{0}", ex.Message));
                Program.ErrorPrint(ex);
                return false;
            }

        }

        /// <summary>
        /// Processes the daily returns.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ProcessDailyReturns(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DateTime start = DateTime.Now;

                using (NirvanaDataContext db = new NirvanaDataContext())
                {
                    if (Arguments.Recovery)
                    {
                        if (db.T_MW_DailyReturns.Where(w => w.rundate == toDate).Count() > 0)
                        {
                            lock (locker)
                            {
                                ProcessStepCounters();
                                return true;
                            }
                        }
                    }

                    _status.SetDailyReturns(toDate, "Running... " + DateTime.Now.ToString());

                    var result = db.P_MW_GetDailyReturns(fromDate, toDate, 1, 0);

                    //ProcessRecordCounters((int)result.GetResult<int>());                                                                
                    ProcessStepCounters();
                    SetTitle();
                    _status.SetDailyReturns(toDate, "Success: " + DateTime.Now.ToString());
                }

                //Program.DebugPrint("{0} ProcessDailyReturns Duration {1} / Cores", fromDate.ShortDate(), new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());
                return true;
            }
            catch (Exception ex)
            {
                _status.SetDailyReturns(toDate, "Error: " + ex.Message);
                Program.DebugPrint(string.Format("{0}", ex.Message));
                Program.ErrorPrint(ex);
                return false;
            }
        }

        #region Counters
        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <remarks></remarks>
        private static void SetTitle()
        {
            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - start.Ticks);
            double pct = ((double)BlocksProcessed / (double)Steps);

            Console.Title = string.Format("{0} of {1} Days | {2} of {3} Steps | Elapsed {4} | Remaining  {5}  | {6} Complete",
                DaysProcessed, Arguments.Dates.Count(), BlocksProcessed, Steps,
                span.Duration().ToString().Substring(0, 8),
                TimeSpan.FromSeconds(SecondsRemaining).Duration().ToString().Substring(0, 8),
                 pct.ToString("P1"));
        }
        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <remarks></remarks>
        public static int Steps
        {
            get
            {
                int _Steps = 0;
                if (Arguments.Step <= 1)
                    _Steps = _Steps + 2;
                if (global::Nirvana.Middleware.Linq.NirvanaDataContext.DerivedDataEnabled)
                    _Steps++;
                if (global::Nirvana.Middleware.Linq.NirvanaDataContext.DailyReturnsEnabled)
                    _Steps++;

                //int steps = Arguments.Step < 2 ? 5 : 4;
                return Arguments.Dates.Count() * _Steps;
            }
        }
        /// <summary>
        /// Processes the record counters.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <remarks></remarks>
        public static void ProcessRecordCounters(int records)
        {
            lock (locker)
            {
                RecordsProcessed += (uint)records;
            }
        }

        /// <summary>
        /// Processes the block counters.
        /// </summary>
        /// <remarks></remarks>
        private static void ProcessStepCounters()
        {
            lock (locker)
            {
                BlocksProcessed++;
                TimeSpan span = new TimeSpan(DateTime.Now.Ticks - start.Ticks);

                uint totalBlocks = (uint)Steps;

                double bps = BlocksProcessed / span.TotalSeconds;
                SecondsRemaining = (totalBlocks - BlocksProcessed) / bps;
                SetTitle();
            }
        }

        /// <summary>
        /// Processes the day counters.
        /// </summary>
        /// <remarks></remarks>
        private static void ProcessDayCounters()
        {
            lock (locker)
            {
                DaysProcessed++;
                SetTitle();
            }
        }
        #endregion
    }
}