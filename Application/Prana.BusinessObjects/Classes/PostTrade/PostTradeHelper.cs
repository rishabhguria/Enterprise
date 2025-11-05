using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    public class PostTradeHelper
    {
        public static bool ISFractionalAllocationAllowed(int assetID)
        {
            int baseAssetID = Mapper.GetBaseAsset(assetID);
            bool result = true;
            switch (baseAssetID)
            {
                case (int)AssetCategory.Equity:
                case (int)AssetCategory.Option:
                case (int)AssetCategory.Future:
                case (int)AssetCategory.FutureOption:
                    // case (int)AssetCategory.Forward:

                    result = true;
                    break;
                default:
                    result = true;
                    break;
            }


            return result;

        }
        public static double GetAllocatedQty(int assetID, double percentage, double totalQty)
        {
            decimal qty = 0;
            qty = (Convert.ToDecimal(percentage) * Convert.ToDecimal(totalQty)) / Convert.ToDecimal(100);

            //It has been set to true for all the assetcategories as fractional shares 
            //allocation has been implemented in our application.
            if (!ISFractionalAllocationAllowed(assetID))
            {
                qty = Convert.ToInt64(qty);
            }
            return Convert.ToDouble(qty);
        }
    }
}
