using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Historical.Pricing.Library;

namespace Historical.Pricing.Library
{

    /// <summary>
    /// Historical Event Arguments
    /// </summary>
    /// <remarks></remarks>
    public class HistoricalEventArgs : Historical.Pricing.Library.DailyBar
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalEventArgs"/> class.
        /// </summary>
        /// <remarks></remarks>
        public HistoricalEventArgs() { }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalEventArgs"/> class.
        /// </summary>
        /// <param name="cols">The cols.</param>
        /// <param name="spec">The spec.</param>
        /// <remarks></remarks>
        public HistoricalEventArgs(string[] cols, FileSpec spec)
        {
            Symbol = cols[spec.Symbol.Value];
            Date = DateTime.Parse(cols[spec.Date.Value]);
            Open = Double.Parse(cols[spec.Open.Value]);
            High = Double.Parse(cols[spec.High.Value]);
            Low = Double.Parse(cols[spec.Low.Value]);
            Close = Double.Parse(cols[spec.Close.Value]);

           
            
            Flags = Int32.Parse(cols[cols.Length-1]);

            if (spec.Volume.Value != -1 && cols.Length > spec.Volume.Value)
                Volume = Double.Parse(cols[spec.Volume.Value]);

            if (spec.Interest.Value != -1 && cols.Length > spec.Interest.Value)
                Interest = Double.Parse(cols[spec.Interest.Value]);
        }

        /// <summary>
        /// Gets the row header.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetRowHeader(string token = "\t")
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
             "Symbol, Date", "Open", "High", "Low", "Close", "Volume", "Interest", "Flags").Replace("\t", token);
        }
    
        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetRow(string token = "\t")
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", 
                Symbol, Date.ToShortDateString(), Open, High, Low, Close, Volume, Interest, Flags).Replace("\t", token);
        }
        /// <summary>
        /// Gets the SQL insert.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetSQLInsert(int? provider)
        {
            return string.Format("Insert into DailyBars ([Symbol],[Date],[Open],[High],[Low],[Close],[Volume],[Interest],[Flags],[ProviderId]) Values('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9})", 
                Symbol.Trim(), Date.ToShortDateString(), Open, High, Low, Close, Volume, Interest, Flags, provider);
        }
    }
}
