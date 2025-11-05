using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;

namespace Prana.SecurityMasterNew
{
    class SecurityMasterFactory
    {
        public static SecMasterBaseObj GetSecmasterObject(AssetCategory asset)
        {
            SecMasterBaseObj secMasterObj = null;
            switch (asset)
            {
                case AssetCategory.FXForward:
                    secMasterObj = new SecMasterFXForwardObj();
                    break;

                default:
                    AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(asset);

                    switch (baseAssetCategory)
                    {
                        case AssetCategory.Equity:
                        case AssetCategory.PrivateEquity:
                        case AssetCategory.CreditDefaultSwap:
                            secMasterObj = new SecMasterEquityObj();

                            break;
                        case AssetCategory.Option:
                            secMasterObj = new SecMasterOptObj();
                            break;
                        case AssetCategory.Future:
                            secMasterObj = new SecMasterFutObj();
                            break;
                        case AssetCategory.FX:
                            secMasterObj = new SecMasterFxObj();
                            break;
                        case AssetCategory.FixedIncome:
                        case AssetCategory.ConvertibleBond:
                            secMasterObj = new SecMasterFixedIncome();
                            break;
                        case AssetCategory.Indices:
                            secMasterObj = new SecMasterIndexObj();
                            break;
                        default:
                            break;
                    }
                    break;
            }
            if (secMasterObj != null)
            {
                secMasterObj.AssetID = (int)asset;
            }
            return secMasterObj;
        }
    }
}
