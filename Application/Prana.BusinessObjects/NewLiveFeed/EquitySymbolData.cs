using System;
using System.Runtime.Serialization;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class EquitySymbolData
    /// </summary>
    [Serializable]
    [DataContract]
    public class EquitySymbolData : SymbolData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquitySymbolData"/> class.
        /// </summary>
        public EquitySymbolData()
        {
        }
        /// <summary>
        /// Gets or sets the market capitalization.
        /// </summary>
        /// <value>The market capitalization.</value>
        [DataMember]
        public override double MarketCapitalization
        {
            get
            {
                return base.MarketCapitalization;
            }
            set
            {
                base.MarketCapitalization = value;
            }
        }
        /// <summary>
        /// Gets or sets the shares outstanding.
        /// </summary>
        /// <value>The shares outstanding.</value>
        [DataMember]
        public override long SharesOutstanding
        {
            get
            {
                return base.SharesOutstanding;
            }
            set
            {
                base.SharesOutstanding = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public EquitySymbolData(string[] data, ref int i)
            : base(data, ref i)
        {
            if (i < data.Length)
                this.SharesOutstanding = long.Parse(data[i++]);
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(Seperators.SEPERATOR_3);
            sb.Append(this.SharesOutstanding.ToString());
            sb.Append(Seperators.SEPERATOR_3);
            sb.Append(this.Delta.ToString());
            return sb.ToString();
        }
        /// <summary>
        /// Updates the continuous data.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void UpdateContinuousData(SymbolData data)
        {
            EquitySymbolData eqData = data as EquitySymbolData;
            if (eqData != null)
            {
                base.UpdateContinuousData(data);
                if (eqData.SharesOutstanding != 0)
                    this.SharesOutstanding = eqData.SharesOutstanding;
            }
        }
        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>EquitySymbolData.</returns>
        public static EquitySymbolData Parse(string data)
        {
            string[] ar = data.Split(Seperators.SEPERATOR_3);
            EquitySymbolData eqdata = new EquitySymbolData();
            int i = 0;
            SymbolData.Parse(eqdata, ar, ref i);
            if (i < ar.Length)
                eqdata.SharesOutstanding = long.Parse(ar[i++]);
            return eqdata;
        }
    }
}
