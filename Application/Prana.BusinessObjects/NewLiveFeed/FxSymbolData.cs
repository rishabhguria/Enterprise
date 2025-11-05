using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class FxSymbolData
    /// </summary>
    [Serializable]
    public class FxSymbolData : SymbolData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FxSymbolData"/> class.
        /// </summary>
        public FxSymbolData()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public FxSymbolData(string[] data, ref int i)
            : base(data, ref i)
        {

        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
        /// <summary>
        /// Gets or sets the conversion method.
        /// </summary>
        /// <value>The conversion method.</value>
        public override Operator ConversionMethod
        {
            get
            {
                return base.ConversionMethod;
            }
            set
            {
                base.ConversionMethod = value;
                AdjustPricesBasedOnConversionMethod();
            }
        }
        /// <summary>
        /// Adjusts the prices based on conversion method.
        /// </summary>
        protected void AdjustPricesBasedOnConversionMethod()
        {
            if (ConversionMethod == Operator.D)
            {
                if (SelectedFeedPrice != double.MinValue && SelectedFeedPrice != 0)
                    SelectedFeedPrice = 1.0 / SelectedFeedPrice;
                if (Ask != double.MinValue && Ask != 0)
                    Ask = 1.0 / Ask;
                if (Bid != double.MinValue && Bid != 0)
                    Bid = 1.0 / Bid;
                if (LastPrice != double.MinValue && LastPrice != 0)
                    LastPrice = 1.0 / LastPrice;
                if (High != double.MinValue && High != 0)
                    High = 1.0 / High;
                if (Low != double.MinValue && Low != 0)
                    Low = 1.0 / Low;
                if (Previous != double.MinValue && Previous != 0)
                    Previous = 1.0 / Previous;
            }
        }
        /// <summary>
        /// Updates the continuous data.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        public override void UpdateContinuousData(SymbolData symbolData)
        {
            base.UpdateContinuousData(symbolData);
            AdjustPricesBasedOnConversionMethod();

        }

    }
}
