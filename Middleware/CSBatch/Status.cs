using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nirvana.Middleware.Linq;

namespace CSBatch
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class Status : Nirvana.Middleware.Linq.NirvanaDataContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public Status()
        {
            try
            {
                this.Connection.Open();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Sets the dates.
        /// </summary>
        /// <param name="dates">The dates.</param>
        /// <remarks></remarks>
        public void SetDates(List<DateTime> dates)
        {
            if (NirvanaDataContext.LogSQLStatus == false) return;

            try
            {
                T_MW_Status.DeleteAllOnSubmit(T_MW_Status.ToList());

                foreach (var date in dates)
                {
                    T_MW_Status.InsertOnSubmit(new T_MW_Status() { Date = date });
                }
                SubmitChanges();
            }
            catch (Exception)
            {
                return;
            }
        }
        /// <summary>
        /// Sets the generic.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SetGeneric(DateTime date, string message)
        {
            try
            {
                if (NirvanaDataContext.LogSQLStatus == false) return;
                lock (this)
                {
                    var status = T_MW_Status.Where(w => w.Date == date).SingleOrDefault();
                    status.Generic = message.Length <= 200 ? message : message.Substring(0, 200);
                    SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Sets the veda.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SetVeda(string message)
        {
            try
            {
                if (NirvanaDataContext.LogSQLStatus == false) return;
                lock (this)
                {
                    T_MW_Status.ToList().ForEach(item => item.Veda = message);                                        
                    SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        /// <summary>
        /// Sets the cache.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SetCache(string message)
        {
            try
            {
                if (NirvanaDataContext.LogSQLStatus == false) return;
                lock (this)
                {
                    T_MW_Status.ToList().ForEach(item => item.Cache = message);
                    SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        /// <summary>
        /// Sets the veda.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SetVeda(DateTime date, string message)
        {
            try
            {
                if (NirvanaDataContext.LogSQLStatus == false) return;
                lock (this)
                {
                    var status = T_MW_Status.Where(w => w.Date == date).SingleOrDefault();
                    status.Veda = message.Length <= 200 ? message : message.Substring(0, 200);
                    SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        /// <summary>
        /// Sets the transactions.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SetTransactions(DateTime date, string message)
        {
            try
            {
                if (NirvanaDataContext.LogSQLStatus == false) return;
                lock (this)
                {
                    var status = T_MW_Status.Where(w => w.Date == date).SingleOrDefault();
                    status.Transactions = message.Length <= 200 ? message : message.Substring(0, 200);
                    SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Sets the derived data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SetDerivedData(DateTime date, string message)
        {
            try
            {
                if (NirvanaDataContext.LogSQLStatus == false) return;
                lock (this)
                {
                    var status = T_MW_Status.Where(w => w.Date == date).SingleOrDefault();
                    status.DerivedData = message.Length <= 200 ? message : message.Substring(0, 200);
                    SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Sets the daily returns.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SetDailyReturns(DateTime date, string message)
        {
            try
            {
                if (NirvanaDataContext.LogSQLStatus == false) return;
                lock (this)
                {
                    var status = T_MW_Status.Where(w => w.Date == date).SingleOrDefault();
                    status.DailyReturns = message.Length <= 200 ? message : message.Substring(0, 200);
                    SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
