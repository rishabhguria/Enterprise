using System;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class FutureSymbolData
    /// </summary>
    [Serializable]
    public class FutureSymbolData : SymbolData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FutureSymbolData"/> class.
        /// </summary>
        public FutureSymbolData()
        {
        }
        /// <summary>
        /// Updates the continuous data.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void UpdateContinuousData(SymbolData data)
        {
            FutureSymbolData eqData = data as FutureSymbolData;
            if (eqData != null)
            {
                base.UpdateContinuousData(data);
                if (eqData.OpenInterest != 0.0)
                    this.OpenInterest = eqData.OpenInterest;
                if (eqData.ExpirationDate != DateTimeConstants.MinValue)
                    this.ExpirationDate = eqData.ExpirationDate;
                if (eqData.DaysToExpiration != 0)
                    this.DaysToExpiration = eqData.DaysToExpiration;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public FutureSymbolData(string[] data, ref int i)
            : base(data, ref i)
        {
            if (i < data.Length)
            {
                this.ExpirationDate = DateTime.Parse(data[i++].ToString());
                this.DaysToExpiration = int.Parse(data[i++]);
                this.OpenInterest = double.Parse(data[i++]);
                this.SharesOutstanding = long.Parse(data[i++]);
            }
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(_splitter);
            sb.Append(this.ExpirationDate.ToString());
            sb.Append(_splitter);
            sb.Append(this.DaysToExpiration.ToString());
            sb.Append(_splitter);
            sb.Append(this.OpenInterest.ToString());
            sb.Append(_splitter);
            sb.Append(this.SharesOutstanding.ToString());
            return sb.ToString();
        }
        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>FutureSymbolData.</returns>
        public static FutureSymbolData Parse(string data)
        {
            string[] ar = data.Split(Seperators.SEPERATOR_3);
            FutureSymbolData futdata = new FutureSymbolData();
            int i = 0;
            SymbolData.Parse(futdata, ar, ref i);
            futdata.ExpirationDate = DateTime.Parse(ar[i++].ToString());
            futdata.DaysToExpiration = int.Parse(ar[i++]);
            futdata.OpenInterest = double.Parse(ar[i++]);
            return futdata;
        }


    }
}
