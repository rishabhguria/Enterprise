using System.ComponentModel;

namespace Act40OrderGeneratorTool
{
    internal enum FilterFields
    {
        /**
         * THE DISCRIPTION HOLDS THE COLUMN NAME W.R.T TO ESPER
         * */

        [Description("underlyingMarketCapitalization")]
        MarketCap,

        [Description("deltaAdjPosition")]
        DeltaAdjPosition,

        [Description("avgVolume20Days")]
        ADTV20Days,

        [Description("betaAdjustedExposureBase")]
        BetaAdjDollarDelta,

        [Description("netExposureBase")]
        NetExposureBase
    }
}
