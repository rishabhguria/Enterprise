#pragma warning disable 1591

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System;
using System.IO;

namespace Nirvana.Middleware.Linq
{
    using Settings = global::Nirvana.Middleware.Properties.Settings;

    /// <summary>
    /// Nirvana Data Context
    /// </summary>
    /// <remarks></remarks>
    public partial class NirvanaDataContext
    {

        /// <summary>
        /// Gets or sets the next id.
        /// </summary>
        /// <value>The next id.</value>
        /// <remarks></remarks>
        static int NextId { get; set; }

        /// <summary>
        /// LINQ Log
        /// </summary>
        volatile static object linqWriter = new object();

        TextWriter writer = null;
        /// <summary>
        /// Tables the exists.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool TableExists(string table)
        {
            try
            {
                string sql = string.Format("SELECT TOP 1 * FROM {0}", table);
                var cmd = Connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = Connection;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="srcTable">The SRC table.</param>
        /// <param name="dstTable">The DST table.</param>
        /// <remarks></remarks>
        private void CreateTable(string srcTable, string dstTable)
        {
            if (TableExists(dstTable))
                return;
            try
            {
                string sql = string.Format("SELECT TOP 1 * INTO {0} FROM {1}", dstTable, srcTable);
                var cmd = Connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = Connection;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return;
            }
        }
        /// <summary>
        /// Creates the tables.
        /// </summary>
        /// <remarks></remarks>
        public void CreateTables()
        {
            CreateTable("T_TS_GenericPNL", GenericPNLTable);
            CreateTable("T_TS_Transactions", TransactionsTable);
            CreateTable("T_TS_DerivedData", DerivedDataTable);
            CreateTable("T_TS_DailyReturns", DailyReturnsTable);
            CreateTable("T_TS_DailyReturns_Fund", DailyReturnsFundTable);
        }

        /// <summary>
        /// Get daily returns multi results.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="autoSave">The auto save.</param>
        /// <param name="returnAllData">The return all data.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [ResultType(typeof(T_MW_DailyReturns_Fund))]
        [ResultType(typeof(T_MW_DailyReturn))]
        [ResultType(typeof(int))]
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_GetDailyReturns")]

        public IMultipleResults P_MW_GetDailyReturns([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "AutoSave", DbType = "Int")] System.Nullable<int> autoSave, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ReturnAllData", DbType = "Int")] System.Nullable<int> returnAllData)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), fromDate, toDate, autoSave, returnAllData);
            return (IMultipleResults)result.ReturnValue;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "SenSatoV1.7")]
    public partial class NirvanaDataContext : System.Data.Linq.DataContext
    {

        /// <summary>
        /// 
        /// </summary>
        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions
        /// <summary>
        /// Called when [created].
        /// </summary>
        /// <remarks></remarks>
        partial void OnCreated();
        /// <summary>
        /// Inserts the  company fund cash currency value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void InsertPM_CompanyFundCashCurrencyValue(PM_CompanyFundCashCurrencyValue instance);
        /// <summary>
        /// Updates the  company fund cash currency value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void UpdatePM_CompanyFundCashCurrencyValue(PM_CompanyFundCashCurrencyValue instance);
        /// <summary>
        /// Deletes the  company fund cash currency value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void DeletePM_CompanyFundCashCurrencyValue(PM_CompanyFundCashCurrencyValue instance);
        /// <summary>
        /// Inserts the company fund.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void InsertT_CompanyFund(T_CompanyFund instance);
        /// <summary>
        /// Updates the company fund.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void UpdateT_CompanyFund(T_CompanyFund instance);
        /// <summary>
        /// Deletes the company fund.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void DeleteT_CompanyFund(T_CompanyFund instance);
        /// <summary>
        /// Inserts the sub account.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void InsertT_SubAccount(T_SubAccount instance);
        /// <summary>
        /// Updates the sub account.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void UpdateT_SubAccount(T_SubAccount instance);
        /// <summary>
        /// Deletes the sub account.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void DeleteT_SubAccount(T_SubAccount instance);
        /// <summary>
        /// Inserts the sub account cash value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void InsertT_SubAccountCashValue(T_SubAccountCashValue instance);
        /// <summary>
        /// Updates the sub account cash value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void UpdateT_SubAccountCashValue(T_SubAccountCashValue instance);
        /// <summary>
        /// Deletes the sub account cash value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void DeleteT_SubAccountCashValue(T_SubAccountCashValue instance);
        /// <summary>
        /// Inserts the  daily beta.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void InsertPM_DailyBeta(PM_DailyBeta instance);
        /// <summary>
        /// Updates the  daily beta.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void UpdatePM_DailyBeta(PM_DailyBeta instance);
        /// <summary>
        /// Deletes the  daily beta.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void DeletePM_DailyBeta(PM_DailyBeta instance);
        /// <summary>
        /// Inserts the  day mark price.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void InsertPM_DayMarkPrice(PM_DayMarkPrice instance);
        /// <summary>
        /// Updates the  day mark price.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void UpdatePM_DayMarkPrice(PM_DayMarkPrice instance);
        /// <summary>
        /// Deletes the  day mark price.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void DeletePM_DayMarkPrice(PM_DayMarkPrice instance);
        /// <summary>
        /// Inserts the company.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void InsertT_Company(T_Company instance);
        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void UpdateT_Company(T_Company instance);
        /// <summary>
        /// Deletes the company.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks></remarks>
        partial void DeleteT_Company(T_Company instance);
        #endregion

        public static void ClearLinqLogs()
        {
            string[] files = Directory.GetFiles(".\\Linq", "*.log");

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Merges the logs.
        /// </summary>
        /// <remarks></remarks>
        public static void MergeLogs()
        {
            string[] files = Directory.GetFiles(".\\Linq", "*.log");

            foreach (string file in files)
            {
                try
                {
                    string buffer = String.Format("* * * {0} * * *\nn{1}\n\n", file, File.ReadAllText(file));
                    File.AppendAllText(LINQToSQLLog, buffer);
                    File.Delete(file);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <remarks></remarks>
        private void Initialize()
        {
            CommandTimeout = ConnectTimeout;

            if (Directory.Exists(".\\Linq") == false)
                Directory.CreateDirectory(".\\Linq");

            lock (linqWriter)
            {
                if (Settings.Default.EnableLINQLogging == false) return;
                writer = new StreamWriter(string.Format(".\\Linq\\{0:0000}-{1}.log", ++NextId, DateTime.Today.Key()));
                Log = writer;

            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception) { }
            base.Dispose(disposing);

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public NirvanaDataContext() :
            base(Settings.Default.SourceConnectString, mappingSource)
        {
            Initialize();
            OnCreated();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NirvanaDataContext"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <remarks></remarks>
        public NirvanaDataContext(string connection) :
            base(connection, mappingSource)
        {
            Initialize();
            OnCreated();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NirvanaDataContext"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <remarks></remarks>
        public NirvanaDataContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
            Initialize();
            OnCreated();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NirvanaDataContext"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="mappingSource">The mapping source.</param>
        /// <remarks></remarks>
        public NirvanaDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            Initialize();
            OnCreated();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NirvanaDataContext"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="mappingSource">The mapping source.</param>
        /// <remarks></remarks>
        public NirvanaDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            Initialize();
            OnCreated();
        }

        /// <summary>
        /// Gets the  company fund cash currency values.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<PM_CompanyFundCashCurrencyValue> PM_CompanyFundCashCurrencyValues
        {
            get
            {
                return this.GetTable<PM_CompanyFundCashCurrencyValue>();
            }
        }

        /// <summary>
        /// Gets the company funds.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_CompanyFund> T_CompanyFunds
        {
            get
            {
                return this.GetTable<T_CompanyFund>();
            }
        }

        /// <summary>
        /// Gets the sub accounts.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_SubAccount> T_SubAccounts
        {
            get
            {
                return this.GetTable<T_SubAccount>();
            }
        }

        /// <summary>
        /// Gets the sub account cash values.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_SubAccountCashValue> T_SubAccountCashValues
        {
            get
            {
                return this.GetTable<T_SubAccountCashValue>();
            }
        }

        /// <summary>
        /// Gets the  transactions.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_MW_Transaction> T_MW_Transactions
        {
            get
            {
                return this.GetTable<T_MW_Transaction>();
            }
        }

        /// <summary>
        /// Gets the  daily betas.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<PM_DailyBeta> PM_DailyBetas
        {
            get
            {
                return this.GetTable<PM_DailyBeta>();
            }
        }

        /// <summary>
        /// Gets the Middleware derived data.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_MW_DerivedData> T_MW_DerivedDatas
        {
            get
            {
                return this.GetTable<T_MW_DerivedData>();
            }
        }

        /// <summary>
        /// Gets the daily returns.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_MW_DailyReturn> T_MW_DailyReturns
        {
            get
            {
                return this.GetTable<T_MW_DailyReturn>();
            }
        }

        /// <summary>
        /// Gets the generic PNl.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_MW_GenericPNL> T_MW_GenericPNLs
        {
            get
            {
                return this.GetTable<T_MW_GenericPNL>();
            }
        }

        /// <summary>
        /// Gets the day mark prices.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<PM_DayMarkPrice> PM_DayMarkPrices
        {
            get
            {
                return this.GetTable<PM_DayMarkPrice>();
            }
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_Company> T_Companies
        {
            get
            {
                return this.GetTable<T_Company>();
            }
        }

        /// <summary>
        /// Gets the  daily returns_ funds.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_MW_DailyReturns_Fund> T_TS_DailyReturns_Funds
        {
            get
            {
                return this.GetTable<T_MW_DailyReturns_Fund>();
            }
        }

        /// <summary>
        /// Gets the sec master datas.
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<V_SecMasterData> V_SecMasterDatas
        {
            get
            {
                return this.GetTable<V_SecMasterData>();
            }
        }


        /// <summary>
        /// Gets the T_MW_Status
        /// </summary>
        /// <remarks></remarks>
        public System.Data.Linq.Table<T_MW_Status> T_MW_Status
        {
            get
            {
                return this.GetTable<T_MW_Status>();
            }
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_CreateGenericCache")]
        public int P_MW_CreateGenericCache()
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
            return ((int)(result.ReturnValue));
        }

        /// <summary>
        /// Adjusts the business days.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="numDays">The num days.</param>
        /// <param name="aUECID">A UECID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.AdjustBusinessDays", IsComposable = true)]
        public System.Nullable<System.DateTime> AdjustBusinessDays([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Int")] System.Nullable<int> numDays, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "AUECID", DbType = "Int")] System.Nullable<int> aUECID)
        {
            return ((System.Nullable<System.DateTime>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, numDays, aUECID).ReturnValue));
        }

        /// <summary>
        /// Determines whether [is business day] [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="auecid">The auecid.</param>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.IsBusinessDay", IsComposable = true)]
        public System.Nullable<bool> IsBusinessDay([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> date, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Int")] System.Nullable<int> auecid)
        {
            return ((System.Nullable<bool>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), date, auecid).ReturnValue));
        }

        /// <summary>
        /// get generic PNL.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="autoSave">The auto save.</param>
        /// <returns>ISingleResult{T_MW_GenericPNL}.</returns>
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_GetGenericPNL")]
        public ISingleResult<T_MW_GenericPNL> P_MW_GetGenericPNL([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "StartDate", DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "EndDate", DbType = "DateTime")] System.Nullable<System.DateTime> endDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "AutoSave", DbType = "Int")] System.Nullable<int> autoSave, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, endDate, autoSave, fundIDSymbol);
            return ((ISingleResult<T_MW_GenericPNL>)(result.ReturnValue));
        }

        /// <summary>
        /// get transactions.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="autoSave">The auto save.</param>
        /// <returns>ISingleResult{T_MW_Transaction}.</returns>
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_GetTransactions")]
        public ISingleResult<T_MW_Transaction> P_MW_GetTransactions([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "StartDate", DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "EndDate", DbType = "DateTime")] System.Nullable<System.DateTime> endDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "AutoSave", DbType = "Int")] System.Nullable<int> autoSave, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, endDate, autoSave, fundIDSymbol);
            return ((ISingleResult<T_MW_Transaction>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_DeleteGenericPNL")]
        public int P_MW_DeleteGenericPNL([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "StartDate", DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "EndDate", DbType = "DateTime")] System.Nullable<System.DateTime> endDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, endDate, fundIDSymbol);
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_DeleteTransactions")]
        public int P_MW_DeleteTransactions([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "StartDate", DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "EndDate", DbType = "DateTime")] System.Nullable<System.DateTime> endDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, endDate, fundIDSymbol);
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_GetFundIDSymbol")]
        public ISingleResult<FundSymbol> P_MW_GetFundIDSymbol
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "StartDate", DbType = "DateTime")] System.Nullable<System.DateTime> startDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "EndDate", DbType = "DateTime")] System.Nullable<System.DateTime> endDate)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, endDate);
            return ((ISingleResult<FundSymbol>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_GetGenericPNL")]
        public ISingleResult<T_NT_GenericPNL> P_NT_GetGenericPNL
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return ((ISingleResult<T_NT_GenericPNL>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_GetTransactions")]
        public ISingleResult<T_NT_Transaction> P_NT_GetTransactions
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return ((ISingleResult<T_NT_Transaction>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_GetCashAccruals")]
        public ISingleResult<T_NT_CashAccruals> P_NT_GetCashAccruals
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return ((ISingleResult<T_NT_CashAccruals>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_HandleGenericPNL")]
        public int P_NT_HandleGenericPNL
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return (int)(result.ReturnValue);
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_HandleTransactions")]
        public int P_NT_HandleTransactions
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return (int)(result.ReturnValue);
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_DeleteGenericPNL")]
        public int P_NT_DeleteGenericPNL
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return (int)(result.ReturnValue);
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_DeleteTransactions")]
        public int P_NT_DeleteTransactions
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return (int)(result.ReturnValue);
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_NT_DeleteCashAccruals")]
        public int P_NT_DeleteCashAccruals
            ([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ToDate", DbType = "DateTime")] System.Nullable<System.DateTime> toDate,
            [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FundIDSymbol", DbType = "Xml")] string fundIDSymbol)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fromDate, toDate, fundIDSymbol);
            return (int)(result.ReturnValue);
        }

        /// <summary>
        /// get derived data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="todate">todate.</param>
        /// <param name="autoSave"> auto save.</param>
        /// <param name="returnAllData">return all data.</param>
        /// <param name="lookBackDays">look back days.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_GetDerivedData")]
        public ISingleResult<T_MW_DerivedData> P_MW_GetDerivedData([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FromDate", DbType = "DateTime")] System.Nullable<System.DateTime> fromDate, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> todate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "AutoSave", DbType = "Int")] System.Nullable<int> autoSave, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ReturnAllData", DbType = "Int")] System.Nullable<int> returnAllData, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "LookBackDays", DbType = "Int")] System.Nullable<int> lookBackDays)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), fromDate, todate, autoSave, returnAllData, lookBackDays);
            return ((ISingleResult<T_MW_DerivedData>)(result.ReturnValue));
        }

        /// <summary>
        /// get Dividend Yield.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="autoSave">The auto save.</param>
        /// <returns>ISingleResult{PM_DailyDividendYield}.</returns>
        //  The Below procedure can be used if data between date range is required
        //  [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_DailyDividendYieldsProc")]
        public ISingleResult<PM_DailyDividendYield> PM_DailyDividendYieldsFiltered([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "StartDate", DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "EndDate", DbType = "DateTime")] System.Nullable<System.DateTime> endDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "AutoSave", DbType = "Int")] System.Nullable<int> autoSave)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, endDate, autoSave);
            return ((ISingleResult<PM_DailyDividendYield>)(result.ReturnValue));
        }
        /// <summary>
        /// get all the values from PM_DailyDividendYield
        /// </summary>
        public System.Data.Linq.Table<PM_DailyDividendYield> PM_DailyDividendYields
        {
            get
            {
                return this.GetTable<PM_DailyDividendYield>();
            }
        }

       /// <summary>
       /// Purpose :Save all distinct reports filters in T_MW_PNLReportsDropDown table from T_MW_GenericPNL table. 
       /// </summary>
       /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.P_MW_UpdatePNLReportsDropDown")]
        public int P_MW_UpdatePNLReportsDropDown()
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
            return ((int)(result.ReturnValue));    }

        /// <summary>
        /// get all the values from PM_DailyVolatility
        /// </summary>
        public System.Data.Linq.Table<PM_DailyVolatility> PM_DailyVolatility
        {
            get
            {
                return this.GetTable<PM_DailyVolatility>();
            }
        }
    }

}
