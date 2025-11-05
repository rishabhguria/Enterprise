using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Text;


namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class OptionSymbolData
    /// </summary>
    [Serializable]
    public class OptionSymbolData : SymbolData
    {
        /// <summary>
        /// The default interest rate value
        /// </summary>
        // Kuldeep A.: changing it's default value as whenever the underlying symbol/underlying data is not available then it 
        // doesn't calculted and shows its default value as "-INFINITY".
        private const double defaultInterestRateValue = 0.25;

        /// <summary>
        /// 
        /// </summary>
        public OptionSymbolData() { }

        /// <summary>
        /// Updates the continuous data.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void UpdateContinuousData(SymbolData data)
        {
            OptionSymbolData opData = data as OptionSymbolData;

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
                if (opData.CFICode != string.Empty)
                    this.CFICode = opData.CFICode;
                if (opData.OSIOptionSymbol != string.Empty)
                    this.OSIOptionSymbol = opData.OSIOptionSymbol;
                if (opData.IDCOOptionSymbol != string.Empty)
                    this.IDCOOptionSymbol = opData.IDCOOptionSymbol;
                if (opData.OpraSymbol != string.Empty)
                    this.OpraSymbol = opData.OpraSymbol;
                if (opData.Delta != 0.0)
                    this.Delta = opData.Delta;
                if (opData.Theta != 0.0)
                    this.Theta = opData.Theta;
                if (opData.Vega != 0.0)
                    this.Vega = opData.Vega;
                if (opData.Rho != 0.0)
                    this.Rho = opData.Rho;
                if (opData.Gamma != 0.0)
                    this.Gamma = opData.Gamma;
                if (opData.InterestRate != defaultInterestRateValue)
                    this.InterestRate = opData.InterestRate;
                if (opData.FinalInterestRate != defaultInterestRateValue)
                    this.FinalInterestRate = opData.FinalInterestRate;
                if (opData.TheoreticalPrice != 0.0)
                    this.TheoreticalPrice = opData.TheoreticalPrice;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public OptionSymbolData(string[] data, ref int i)
            : base(data, ref i)
        {
            try
            {
                if (i < data.Length)
                {
                    this.PutOrCall = (OptionType)Enum.Parse(typeof(OptionType), data[i++]);
                    this.Vega = double.Parse(data[i++]);
                    this.ExpirationDate = DateTime.Parse(data[i++].ToString());
                    this.Delta = double.Parse(data[i++]);
                    this.Rho = double.Parse(data[i++]);
                    this.StrikePrice = double.Parse(data[i++]);
                    this.Theta = double.Parse(data[i++]);
                    this.Gamma = double.Parse(data[i++]);
                    this.DaysToExpiration = int.Parse(data[i++]);
                    this.ImpliedVol = double.Parse(data[i++]);
                    double result = 0;
                    if (double.TryParse(data[i], out result))
                        this.InterestRate = double.Parse(data[i++]);
                    else
                        i++;
                    this.OpenInterest = double.Parse(data[i++]);
                    this.CFICode = data[i++];
                    this.OSIOptionSymbol = data[i++];
                    this.IDCOOptionSymbol = data[i++];
                    this.OpraSymbol = data[i++];
                    this.RequestedSymbology = (ApplicationConstants.SymbologyCodes)Enum.Parse(typeof(ApplicationConstants.SymbologyCodes), data[i++]);
                    //this.FinalDelta = double.Parse(data[i++]);
                    this.SharesOutstanding = long.Parse(data[i++]);

                    this.FinalImpliedVol = double.Parse(data[i++]);
                    if (double.TryParse(data[i], out result))
                        this.FinalInterestRate = double.Parse(data[i++]);
                    else
                        i++;
                    this.TheoreticalPrice = double.Parse(data[i++]);
                    if (i < data.Length)
                        if (this.CategoryCode == AssetCategory.EquityOption)
                            this.UnderlyingData = EquitySymbolData.Parse(data[i++]);
                        else
                            this.UnderlyingData = FutureSymbolData.Parse(data[i++]);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                    throw;
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
            sb.Append(this.PutOrCall.ToString());
            sb.Append(_splitter);
            sb.Append(this.Vega.ToString());
            sb.Append(_splitter);
            sb.Append(this.ExpirationDate.ToString());
            sb.Append(_splitter);
            sb.Append(this.Delta.ToString());
            sb.Append(_splitter);
            sb.Append(this.Rho.ToString());
            sb.Append(_splitter);
            sb.Append(this.StrikePrice.ToString());
            sb.Append(_splitter);
            sb.Append(this.Theta.ToString());
            sb.Append(_splitter);
            sb.Append(this.Gamma.ToString());
            sb.Append(_splitter);
            sb.Append(this.DaysToExpiration.ToString());
            sb.Append(_splitter);
            sb.Append(this.ImpliedVol.ToString());
            sb.Append(_splitter);
            sb.Append(this.InterestRate.ToString());
            sb.Append(_splitter);
            sb.Append(this.OpenInterest.ToString());
            sb.Append(_splitter);
            sb.Append(this.CFICode);
            sb.Append(_splitter);
            sb.Append(this.OSIOptionSymbol);
            sb.Append(_splitter);
            sb.Append(this.IDCOOptionSymbol);
            sb.Append(_splitter);
            sb.Append(this.OpraSymbol);
            sb.Append(_splitter);
            sb.Append(this.RequestedSymbology.ToString());
            sb.Append(_splitter);
            //sb.Append(this.FinalDelta.ToString());
            //sb.Append(_splitter);
            sb.Append(this.SharesOutstanding.ToString());
            sb.Append(_splitter);
            sb.Append(this.FinalImpliedVol.ToString());
            sb.Append(_splitter);
            sb.Append(this.FinalInterestRate.ToString());
            sb.Append(_splitter);
            sb.Append(this.TheoreticalPrice.ToString());
            if (this.UnderlyingData != null)
            {
                sb.Append(_splitter);
                sb.Append(this.UnderlyingData.ToString());
            }
            return sb.ToString();
        }
    }
}
