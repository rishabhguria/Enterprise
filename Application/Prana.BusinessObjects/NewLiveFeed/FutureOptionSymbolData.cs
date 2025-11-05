using Prana.BusinessObjects.AppConstants;
using System;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class FutureOptionSymbolData
    /// </summary>
    [Serializable]
    public class FutureOptionSymbolData : SymbolData
    {
        /// <summary>
        /// Updates the continuous data.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void UpdateContinuousData(SymbolData data)
        {
            FutureOptionSymbolData opData = data as FutureOptionSymbolData;

            if (opData != null)
            {
                base.UpdateContinuousData(data);
                if (opData.PutOrCall != OptionType.NONE)
                    this.PutOrCall = opData.PutOrCall;
                if (opData.StrikePrice != 0.0)
                    this.StrikePrice = opData.StrikePrice;
                if (opData.ExpirationDate != DateTimeConstants.MinValue)
                    this.ExpirationDate = opData.ExpirationDate;
                if (opData.DaysToExpiration != 0)
                    this.DaysToExpiration = opData.DaysToExpiration;
                if (opData.OpenInterest != 0.0)
                    this.OpenInterest = opData.OpenInterest;
                if (opData.InterestRate != 0.0)
                    this.InterestRate = opData.InterestRate;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public FutureOptionSymbolData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public FutureOptionSymbolData(string[] data, ref int i)
            : base(data, ref i)
        {

            this.PutOrCall = (OptionType)Enum.Parse(typeof(OptionType), data[i++]);
            this.ExpirationDate = DateTime.Parse(data[i++].ToString());
            this.StrikePrice = double.Parse(data[i++]);
            this.DaysToExpiration = int.Parse(data[i++]);
            this.ImpliedVol = double.Parse(data[i++]);
            this.OpenInterest = double.Parse(data[i++]);
            this.InterestRate = double.Parse(data[i++]);
            this.Delta = double.Parse(data[i++]);
            //this.FinalDelta = double.Parse(data[i++]);
            this.SharesOutstanding = long.Parse(data[i++]);
            this.FinalImpliedVol = double.Parse(data[i++]);
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.PutOrCall.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.ExpirationDate.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.StrikePrice.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.DaysToExpiration.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.ImpliedVol.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.OpenInterest.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.InterestRate);
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.Delta.ToString());
            //sb.Append(Seperators.SEPERATOR_8);
            //sb.Append(this.FinalDelta.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.SharesOutstanding.ToString());
            sb.Append(Seperators.SEPERATOR_8);
            sb.Append(this.FinalImpliedVol.ToString());
            return sb.ToString();
        }
    }
}
