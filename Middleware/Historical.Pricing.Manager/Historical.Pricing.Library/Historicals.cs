using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Linq;
namespace Historical.Pricing.Library
{
    /// <summary>
    /// Historicals
    /// </summary>
    /// <remarks></remarks>
    public class Historicals : IDisposable
    {

        /// <summary>
        /// Symbol Map
        /// </summary>
        public SymbolMapping Symbols = new SymbolMapping();
        /// <summary>
        /// Database Context
        /// </summary>
        public readonly HistoricalsDataContext db = null;

        /// <summary>
        /// Event Handler for Exceptions
        /// </summary>
        public EventHandler<ExceptionEventArgs> OnException;

        /// <summary>
        /// Event Handler for Exceptions
        /// </summary>
        public EventHandler<MessageInfoEventArgs> OnMessage;

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<Subscription> Subscriptions
        {
            get { return db.Subscriptions; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Historicals"/> class.
        /// </summary>
        /// <remarks></remarks>
        public Historicals()
        {
           // string cn = global::Historical.Pricing.Library.Properties.Settings.Default.HistoricalsConnectionString;

            db = new HistoricalsDataContext();
        }
        /// <summary>
        /// Gets the symbols.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> GetSymbols(Subscription subscriber)
        {
            var registrations = db.Registrations.Where(w=> w.SubscriberId == subscriber.SubscriberID);                     
            List<Query> Queries = db.Queries.ToList();

            List<string> Tickers = new List<string>();
            Parallel.ForEach(registrations, item =>
            {
                try
                {
                    string cnStr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Connect Timeout=3600",
                        item.Server, item.Database, item.UserName, item.Password);

                    using (SqlConnection con = new SqlConnection(cnStr))
                    {
                        int rowCount = 0;
                        con.Open();
                        string sql = Queries.Where(w => w.QueryId == item.QueryId).SingleOrDefault().Query1;
                                                                      
                        if (OnMessage != null)
                            OnMessage(this, new MessageInfoEventArgs() { Message = string.Format("Connecting to {0}", item.Server) });

                        using (SqlCommand cmd = new SqlCommand(sql, con) { CommandTimeout = 3600 })
                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {                                
                                while (reader.Read())
                                {
                                    lock (Tickers)
                                    {
                                        rowCount++;
                                        int colId;
                                        if (Int32.TryParse(item.Column, out colId))
                                        {
                                            string symbol = reader.GetString(colId).ToUpper();
                                            if (Tickers.Contains(symbol) == false)
                                            {
                                                Tickers.Add(symbol);
                                            }
                                        }
                                        else
                                        {
                                            string symbol = reader[item.Column].ToString().ToUpper();
                                            if (Tickers.Contains(symbol) == false)
                                            {
                                                Tickers.Add(symbol);
                                                // Symbols.Map(item.Server + "." + item.Database, Tickers);
                                            }
                                        }
                                    }
                                }                              
                            }
                            MessageInfoEventArgs e = new MessageInfoEventArgs();
                            e.Message = string.Format("{0} symbols for {1}.{2}", rowCount, item.Database, item.Server);

                            if (OnMessage != null)
                                OnMessage(this, e);
                        }

                        #region Old
                        //using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, con))
                        //{
                                                       
                        //    using (DataTable table = new DataTable())
                        //    {
                        //        dataAdapter.Fill(table);

                        //        MessageInfoEventArgs e = new MessageInfoEventArgs();
                        //            e.Message = string.Format("{0} symbols for {1}.{2}", table.Rows.Count, item.Database, item.Server);                                                                  

                        //        if (OnMessage != null)
                        //            OnMessage(this, e);
                                
                        //        foreach (DataRow row in table.Rows)
                        //        {
                        //            lock (Tickers)
                        //            {
                        //                int colId;
                        //                if (Int32.TryParse(item.Column, out colId))
                        //                {
                        //                    if (Tickers.Contains(row[colId].ToString().ToUpper()) == false)
                        //                    {
                        //                        Tickers.Add(row[colId].ToString().ToUpper());
                        //                        // Symbols.Map(item.Server + "." + item.Database, Tickers);
                        //                    } 
                        //                }
                        //                else
                        //                if (Tickers.Contains(row[item.Column].ToString().ToUpper()) == false)
                        //                {
                        //                    Tickers.Add(row[item.Column].ToString().ToUpper());
                        //                    // Symbols.Map(item.Server + "." + item.Database, Tickers);
                        //                }
                        //            }
                        //        }
                        //    }       
                                        
                        //    return;
                        //}
                        #endregion        
                    }
                }
                catch (Exception ex)
                {
                     string cnStr = string.Format("Invalid Registration [{0}].[{1}]", item.Server, item.Database);

                    if (OnException != null)
                        OnException(this, new ExceptionEventArgs() { Exception = ex, InnerMessage = cnStr });
                    
                    return;
                }

            });

            return Tickers;
        }


        /// <summary>
        /// Gets the symbols.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<SymbolRequest> GetSymbolRequest(Subscription subscriber)
        {
            var registrations = db.Registrations.Where(w => w.SubscriberId == subscriber.SubscriberID);
            List<Query> Queries = db.Queries.ToList();

            List<SymbolRequest> Tickers = new List<SymbolRequest>();
            Parallel.ForEach(registrations, item =>
            {
                try
                {
                    string cnStr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Connect Timeout=180",
                        item.Server, item.Database, item.UserName, item.Password);

                    using (SqlConnection con = new SqlConnection(cnStr))
                    {                     
                        if (OnMessage != null)
                            OnMessage(this, new MessageInfoEventArgs() { Message = string.Format("Connecting to {0}.", item.Server) });

                        con.Open();
                        string sql = Queries.Where(w => w.QueryId == item.QueryId).SingleOrDefault().Query1;

                        int symbols = 0;
                        using (SqlCommand cmd = new SqlCommand(sql, con) { CommandTimeout = 3600 })
                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    lock (Tickers)
                                    {  
                                        string key = GetValue(item, reader, item.KeyColumn);
                                        string asset = GetValue(item, reader, item.AssetColumn);
                                        string symbol = GetValue(item, reader, item.TickerColumn);
                                        symbols++;                                   
                                        Tickers.Add(new SymbolRequest() { MarkSymbol = key, Asset = asset, PriceSymbol = symbol });                                        

                                    }
                                }
                            }
                            MessageInfoEventArgs e = new MessageInfoEventArgs();
                            e.Message = string.Format("{0} symbols for {1}.{2}", symbols, item.Database, item.Server);
                            symbols = 0;
                            if (OnMessage != null)
                                OnMessage(this, e);
                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    string cnStr = string.Format("Invalid Registration [{0}].[{1}]", item.Server, item.Database);

                    if (OnException != null)
                        OnException(this, new ExceptionEventArgs() { Exception = ex, InnerMessage = cnStr });

                    return;
                }

            });

            return Tickers.Distinct(new KeyCompare()).ToList();
        }
        private string GetValue(Registration reg, SqlDataReader reader, int col)
        {
            try
            {
                if (col == -1) return string.Empty;
                if (reader.FieldCount <= col) return string.Empty;                
                return reader.GetString(col);
            }
            catch (Exception ex)
            {
                OnMessage(this, new MessageInfoEventArgs() 
                { Message = ex.Message, Details = string.Format("{0}", ex), Source = string.Format("GetSymbolRequest::{0}.{1}", reg.Server, reg.Database) });
                return string.Empty;
            }
        }

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool BulkInsert(List<DailyBar> records)
        {
            var distinct = records.Distinct(new DistinctDailyBarComparer());

            using (Historicals history = new Historicals())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(history.db.Connection.ConnectionString))
                    {
                        con.Open();
                        using (SqlTransaction tran = con.BeginTransaction())
                        {
                            var positions =
                                    from p in records
                                    select p;

                            SqlBulkCopy bc = new SqlBulkCopy(con,
                                SqlBulkCopyOptions.CheckConstraints |
                                SqlBulkCopyOptions.FireTriggers |
                                SqlBulkCopyOptions.KeepNulls, tran);

                            bc.BulkCopyTimeout = 0;
                            bc.BatchSize = 4096;
                            bc.DestinationTableName = "DailyBars";
                            bc.WriteToServer(distinct.AsDataReader());

                            tran.Commit();
                        }
                        con.Close();
                    }
                    
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            
            return true;
        }

        public static bool BulkInsert(List<VWAPS> records)
        {

            using (Historicals history = new Historicals())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(history.db.Connection.ConnectionString))
                    {
                        con.Open();
                        using (SqlTransaction tran = con.BeginTransaction())
                        {
                            var positions =
                                    from p in records
                                    select p;

                            SqlBulkCopy bc = new SqlBulkCopy(con,
                                SqlBulkCopyOptions.CheckConstraints |
                                SqlBulkCopyOptions.FireTriggers |
                                SqlBulkCopyOptions.KeepNulls, tran);

                            bc.BulkCopyTimeout = 0;
                            bc.BatchSize = 4096;
                            bc.DestinationTableName = "VWAPS";
                            bc.WriteToServer(records.AsDataReader());

                            tran.Commit();
                        }
                        con.Close();
                    }

                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return true;
        }
        /// <summary>
        /// Deletes the subscription data.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool BulkDelete(string sql)
        {
            using (Historicals history = new Historicals())
            {
                using (SqlConnection con = new SqlConnection(history.db.Connection.ConnectionString))
                {

                    con.Open();
                    //  using (SqlTransaction tran = con.BeginTransaction())
                    //  {
                    //string sql = string.Format("Delete from DailyBars where SubscriberId = {0}", Subscriber.SubscriberID.GetHashCode());

                    SqlCommand cmd = new SqlCommand(sql, con);

                    cmd.CommandTimeout = 3600;
                    cmd.ExecuteNonQuery();
                    //     tran.Commit();
                    //  }
                    con.Close();
                }
            }

            return true;
        }


        public VWAPS getVWAP(string symbol, DateTime dt)
        {
            VWAPS vwap = null;
            var warps = db.VWAPS.Where(w => w.Symbol == symbol && w.Date == dt);
            foreach (VWAPS bar in warps)
            {
                vwap = bar;
                break;
            }
            return vwap;
        }
       
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <remarks></remarks>
        public static string ConnectionString
        {
            get
            {
                return global::Historical.Pricing.Library.Properties.Settings.Default.HistoricalsConnectionString;
            } 

        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
