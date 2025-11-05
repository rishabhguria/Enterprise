using System;
using System.Collections;
using System.Configuration;

namespace Nirvana.Middleware.Linq
{
    /// <summary>
    /// Nirvana Data Context
    /// </summary>
    /// <remarks></remarks>
    public partial class NirvanaDataContext
    {
        /// <summary>
        /// LINQ to SQL Log File
        /// </summary>
        public const string LINQToSQLLog = ".\\LinqToSql.log";

        /// <summary>
        /// Gets a value indicating whether [ignore AUECID].
        /// </summary>
        /// <remarks></remarks>
        public static bool IgnoreAUECID
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.IgnoreAUECID; }
        }

        /// <summary>
        /// Gets a value indicating whether [save enabled].
        /// </summary>
        /// <remarks></remarks>
        public static bool UseExternalCalcs
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.UseExternalCalcs;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [log SQL status].
        /// </summary>
        /// <remarks></remarks>
        public static bool LogSQLStatus
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.LogSQLStatus; }
        }
        /// <summary>
        /// Gets a value indicating whether to execute Derived Data with Execute With Recompile option 
        /// </summary>
        /// <remarks></remarks>
        public static bool ExecuteWithRecompile
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.ExecuteWithRecompile; }
        }
        /// <summary>
        /// Gets a value indicating whether [get veda prices].
        /// </summary>
        /// <remarks></remarks>
        public static bool GetVedaPrices
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.GetVedaPrices; }
        }



        /// <summary>
        /// Gets a value indicating whether [get veda prices].
        /// </summary>
        /// <remarks></remarks>
        public static bool UseWinDaleToCalculateImpliedVol
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.UseWinDaleToCalculateImpliedVol; }
        }

        /// <summary>
        /// Gets a value indicating whether [get veda prices].
        /// </summary>
        /// <remarks></remarks>
        public static int ImpliedVolatiltyIterations
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.ImpliedVolatiltyIterations; }
        }

        /// <summary>
        /// Gets a value indicating whether use user defined ImpliedVol
        /// </summary>
        /// <remarks></remarks>
        public static bool UseUserPrefToCalculateImpliedVol
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.UseUserPrefToCalculateImpliedVol; }
        }

        /// <summary>
        /// Gets the interest free rate.
        /// </summary>
        /// <remarks></remarks>
        public static double InterestFreeRate
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.InterestFreeRate; }
        }
        /// <summary>
        /// Gets a value indicating whether [enable LINQ logging].
        /// </summary>
        /// <remarks></remarks>
        public static bool EnableLINQLogging
        {
            get { return global::Nirvana.Middleware.Properties.Settings.Default.EnableLINQLogging; }
        }
        /// <summary>
        /// Gets the connect timeout.
        /// </summary>
        /// <remarks></remarks>
        public static int ConnectTimeout
        {
            get { return Math.Abs(global::Nirvana.Middleware.Properties.Settings.Default.CommandTimeout); }
        }
        /// <summary>
        /// Gets the look back days.
        /// </summary>
        /// <remarks></remarks>
        public static int? LookBackDays
        {
            get { return Math.Abs(global::Nirvana.Middleware.Properties.Settings.Default.LookbackDays); }
        }

        /// <summary>
        /// Gets a value indicating whether [save enabled].
        /// </summary>
        /// <remarks></remarks>
        public static bool SaveEnabled
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.SaveEnabled;
            }
        }

        public static bool TouchStep
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.TouchStep;
            }
        }

        /// <summary>
        /// Gets the generic PNL table.
        /// </summary>
        /// <remarks></remarks>
        public static string GenericPNLTable
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.T_TS_GenericPNL;
            }
        }
        /// <summary>
        /// Gets the transactions table.
        /// </summary>
        /// <remarks></remarks>
        public static string TransactionsTable
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.T_TS_Transactions;
            }
        }
        /// <summary>
        /// Gets the derived data table.
        /// </summary>
        /// <remarks></remarks>
        public static string DerivedDataTable
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.T_TS_DerivedData;
            }
        }
        /// <summary>
        /// Gets the daily returns table.
        /// </summary>
        /// <remarks></remarks>
        public static string DailyReturnsTable
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.T_TS_DailyReturns;
            }
        }
        /// <summary>
        /// Gets the daily returns fund table.
        /// </summary>
        /// <remarks></remarks>
        public static string DailyReturnsFundTable
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.T_TS_DailyReturns_Fund;
            }
        }
        /// <summary>
        /// Gets a value indicating whether [create middleware table].
        /// </summary>
        /// <remarks></remarks>
        public static bool CreateMiddlewareTables
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.CreateTables;
            }
        }
        /// <summary>
        /// Gets the mail identity.
        /// </summary>
        /// <remarks></remarks>
        public static int MailIdentity
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.MailIdentity;
            }
        }
        /// <summary>
        /// Gets the default days back.
        /// </summary>
        /// <remarks></remarks>
        public static int RollBackDays
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.RollBackDays;
            }
        }
        /// <summary>
        /// Gets the configuartion connection string.
        /// </summary>
        /// <remarks></remarks>
        public static string DestConnectString
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.DestConnectString;
            }
        }
        /// <summary>
        /// Gets the configuartion connection string.
        /// </summary>
        /// <remarks></remarks>
        public static string SourceConnectString
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.SourceConnectString;
            }
        }
        /// <summary>
        /// Gets the pricing server.
        /// </summary>
        /// <remarks></remarks>
        public static string HistoricalConnectString
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.HistoricalConnectString;
            }
        }
        /// <summary>
        /// Gets the average volume.
        /// </summary>
        /// <remarks></remarks>
        public static int AverageVolumeDays
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.AverageVolumeDays;
            }
        }

        /// <summary>
        /// Gets whether derived data should be processed.
        /// </summary>
        /// <remarks></remarks>
        public static bool DerivedDataEnabled
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.DerivedDataEnabled;
            }
        }

        /// <summary>
        /// Gets whether daily returns should be processed.
        /// </summary>
        /// <remarks></remarks>
        public static bool DailyReturnsEnabled
        {
            get
            {
                return global::Nirvana.Middleware.Properties.Settings.Default.DailyReturnsEnabled;
            }
        }
        /// <summary>
        /// Gets the asset models.
        /// </summary>
        /// <remarks></remarks>
        public static Hashtable AssetModels
        {
            get
            {
                Hashtable _hashtable = ConfigurationManager.GetSection("assetModels") as Hashtable;
                return _hashtable;
            }
        }
    }
}
