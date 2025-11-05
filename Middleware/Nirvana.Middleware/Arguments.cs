using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nirvana.Middleware.Linq;

namespace Nirvana.Middleware
{
    /// <summary>
    /// Argument Class. Not very pretty
    /// </summary>
    /// <remarks></remarks>
    public class Arguments
    {
        /// <summary>AdjustFrom
        /// Adjust Business Days for fromDate
        /// </summary>
        public int AdjustFrom = 0;
        /// <summary>
        /// Adjust Business Days for toDate
        /// </summary>
        public int AdjustTo = 0;
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>From date.</value>
        /// <remarks></remarks>
        public DateTime? FromDate;
        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>To date.</value>
        /// <remarks></remarks>
        public DateTime? ToDate;
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>From date.</value>
        /// <remarks></remarks>
        public DateTime From;
        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>To date.</value>
        /// <remarks></remarks>
        public DateTime To;
        /// <summary>
        /// Gets or sets dates.
        /// </summary>
        /// <value>Dates.</value>
        /// <remarks></remarks>
        public List<DateTime> Dates = new List<DateTime>();
        /// <summary>
        /// Gets or sets fund symbols.
        /// </summary>
        /// <value>FundSymbols.</value>
        /// <remarks></remarks>
        public List<Fund> Funds = new List<Fund>();
        /// <summary>
        /// Number of Cores to use when calling  functions
        /// </summary>
        public int FunctionCores = 1;
        /// <summary>
        /// Number of CPU Cores to use when process days
        /// </summary>
        public int Cores = Environment.ProcessorCount;
        /// <summary>
        /// Step to Start at
        /// </summary>
        public int Step = 1;

        /// <summary>
        /// Allows for Recovery
        /// </summary>
        public bool Recovery = false;

        /// <summary>
        /// Override RollBackDays
        /// </summary>
        public int RollBackDays = global::Nirvana.Middleware.Linq.NirvanaDataContext.RollBackDays;

        /// <summary>
        /// Override IgnoreAUECID
        /// </summary>
        public bool IgnoreAUECID = global::Nirvana.Middleware.Linq.NirvanaDataContext.IgnoreAUECID;

        public string SourceConnectString = global::Nirvana.Middleware.Linq.NirvanaDataContext.SourceConnectString;

        /// <summary>
        /// Override SaveEnabled
        /// </summary>
        public bool SaveEnabled = global::Nirvana.Middleware.Linq.NirvanaDataContext.SaveEnabled;

        public bool TouchStep = global::Nirvana.Middleware.Linq.NirvanaDataContext.TouchStep;

        /// <summary>
        /// Determines whether [is business day] [the specified start date].
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="DefaultAUECID">The default AUECID.</param>
        /// <returns><c>true</c> if [is business day] [the specified start date]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool IsBusinessDay(DateTime startDate, int? DefaultAUECID)
        {
            using (NirvanaDataContext db = new NirvanaDataContext())
            {
                if (IgnoreAUECID)
                {
                    return true;
                }
                return (bool)db.IsBusinessDay(startDate, db.T_Companies.First().DefaultAUECID);
            }
        }

        /// <summary>
        /// Gets the date ranges.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<DateTime> GetDates(DateTime fromDate, DateTime toDate)
        {
            List<DateTime> _Dates = new List<DateTime>();

            Client.Run();

            DateTime startDate = fromDate;
            using (NirvanaDataContext db = new NirvanaDataContext())
            {
                while (startDate <= toDate)
                {
                    if (IsBusinessDay(startDate, db.T_Companies.First().DefaultAUECID) == true)
                    {
                        _Dates.Add(startDate);
                        db.T_MW_Status.InsertOnSubmit(new T_MW_Status() { Date = startDate });

                    }
                    startDate = startDate.AddDays(1);
                }

                return _Dates;
            }
            
        }

        /// <summary>
        /// FundIDSymbol Filter File
        /// </summary>
        public string FundIDSymbolFile;
        /// <summary>
        /// FundIDSymbol Filter
        /// </summary>
        public string FundIDSymbol;

        /// <summary>
        /// Parses the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <remarks></remarks>
        public void Parse(string[] args)
        {
            string cmdline = string.Join("", args);
            string[] items = cmdline.Trim().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            #region Loop
            foreach (string item in items)
            {
                if (HasParameter(item, "fromDate", "toDate"))
                {
                    SetParameter(item, DateTime.Today.ToString(), "last", "today");
                    continue;
                }

                if (HasParameter(item, "AdjustFrom", "AdjustTo", "RollbackDays", "IgnoreAUECID"))
                {
                    SetParameter(item);
                    continue;
                }

                if (HasParameter(item, "Cores", "FunctionCores", "Step", "Recovery", "SaveEnabled", "TouchStep"))
                {
                    SetParameter(item);
                    continue;
                }

                if (HasParameter(item, "FundIDSymbolFile"))
                {
                    SetParameter(item);
                    continue;
                }
            }

            #endregion

            using (NirvanaDataContext db = new NirvanaDataContext())
            {
                if (IgnoreAUECID)
                {
                    if (!ToDate.HasValue)
                        To = DateTime.Today;
                    else
                        To = ToDate.Value;

                    if (!FromDate.HasValue)
                        From = To.AddDays(-Math.Abs(RollBackDays));
                    else
                        From = FromDate.Value;

                    To = To.AddDays(AdjustTo);
                    From = From.AddDays(AdjustFrom);
                }
                else
                {
                    if (!ToDate.HasValue)
                        To = DateTime.Today;
                    else
                        To = ToDate.Value;

                    if (!FromDate.HasValue)
                    {
                        DateTime? _FromRollbackDays = db.AdjustBusinessDays(To, -Math.Abs(RollBackDays), db.T_Companies.First().DefaultAUECID);
                        if (_FromRollbackDays.HasValue)
                            From = _FromRollbackDays.Value;
                        else
                            From = To.AddDays(-Math.Abs(RollBackDays));
                    }
                    else
                        From = FromDate.Value;

                    DateTime? _ToAdjustTo = db.AdjustBusinessDays(To, AdjustTo, db.T_Companies.First().DefaultAUECID);
                    if (_ToAdjustTo.HasValue)
                        To = _ToAdjustTo.Value;
                    else
                        To = To.AddDays(AdjustTo);

                    DateTime? _FromAdjustFrom = db.AdjustBusinessDays(From, AdjustFrom, db.T_Companies.First().DefaultAUECID);
                    if (_FromAdjustFrom.HasValue)
                        From = _FromAdjustFrom.Value;
                    else
                        From = From.AddDays(AdjustFrom);
                }
            }

            if (From > To)
                From = To;

            Dates = GetDates(From, To);

            if (!string.IsNullOrWhiteSpace(FundIDSymbolFile))
            {
                try
                {
                    using (TextReader textReader = File.OpenText(FundIDSymbolFile))
                    {
                        FundIDSymbol = textReader.ReadToEnd();
                    }
                }
                catch
                {
                    FundIDSymbol = string.Empty;
                }
            }
            else
            {
                FundIDSymbol = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(FundIDSymbol))
            {
                try
                {
                    using (NirvanaDataContext db = new NirvanaDataContext())
                    {
                        Funds = db.T_CompanyFunds
                            .Select(p => new Fund { FundID = p.CompanyFundID, FundName = p.FundName })
                            .ToList();
                    }

                    StringBuilder sb = new StringBuilder();

                    foreach (Fund _FundElement in Funds)
                    {
                        sb.Append(_FundElement.ToString());
                        sb.Append(Environment.NewLine);
                    }

                    FundIDSymbol = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// StartsWith extension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        bool HasParameter(string name, params string[] tokens)
        {
            foreach (var token in tokens)
            {
                if (name.ToLower().StartsWith(String.Format("@{0}=", token.ToLower())))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Replace Extension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        string Replace(string name, string value, params string[] tokens)
        {
            foreach (var token in tokens)
            {
                name = name.Replace(token, value);
            }
            return name;
        }

        /// <summary>
        /// Parmeter Set Extension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="tokens"></param>
        void SetParameter(string name, string value = null, params string[] tokens)
        {
            string[] values = name.Split(new char[] { '@', '=' }, StringSplitOptions.RemoveEmptyEntries);

            if (value != null)
            {
                values[1] = Replace(values[1], value, tokens);
            }
            var field = typeof(Arguments).GetFields().
                Where(w => w.Name.ToLower() == values[0].ToLower()).SingleOrDefault();

            if (field != null)
            {
                Type t = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;
                object safeValue = (values[1] == null) ? null : Convert.ChangeType(values[1], t);
                field.SetValue(this, safeValue);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public new string ToString()
        {
            return String.Format("fromDate = {0} toDate = {1}  Cores = {2}  Step = {3}", From.ShortDate(), To.ShortDate(), Cores, Step);
        }
    }
}
