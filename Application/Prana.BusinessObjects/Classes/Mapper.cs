using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    public class Mapper
    {
        public static int GetBaseAsset(int assetID)
        {
            return (int)GetBaseAssetCategory((AssetCategory)assetID);
        }
        //public static AssetCategory GetBaseAsset(AssetCategory asset)
        //{
        //    return GetBaseAssetCategory((int)assetID);
        //}
        public static AssetCategory GetBaseAssetCategory(AssetCategory asset)
        {

            switch (asset)
            {
                case AssetCategory.PrivateEquity:
                case AssetCategory.CreditDefaultSwap:
                    //case AssetCategory.FixedIncome:
                    asset = AssetCategory.Equity;
                    break;
                case AssetCategory.EquityOption:
                case AssetCategory.FutureOption:
                    asset = AssetCategory.Option;
                    break;
                case AssetCategory.FXForward:
                    asset = AssetCategory.Future;
                    break;
                case AssetCategory.Indices:
                    asset = AssetCategory.Indices;
                    break;
                case AssetCategory.FixedIncome:
                    asset = AssetCategory.FixedIncome;
                    break;
                case AssetCategory.ConvertibleBond:
                    asset = AssetCategory.ConvertibleBond;
                    break;
                default:
                    break;
            }
            return asset;
        }
        public static string GetBaseSide(string side)
        {

            switch (side)
            {
                case FIXConstants.SIDE_Buy:
                case FIXConstants.SIDE_Buy_Closed:
                case FIXConstants.SIDE_Buy_Open:
                case FIXConstants.SIDE_Buy_Cover:
                    side = FIXConstants.SIDE_Buy;
                    break;
                case FIXConstants.SIDE_Sell:
                case FIXConstants.SIDE_Sell_Closed:
                case FIXConstants.SIDE_Sell_Open:
                case FIXConstants.SIDE_SellPlus:
                    side = FIXConstants.SIDE_Sell;
                    break;

                default:
                    break;
            }
            return side;
        }
    }
}
