using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prana.CentralSMDataCache
{
    public class SecurityMasterFactory
    {
        public static SecMasterBaseObj GetSecmasterObject(AssetCategory asset)
        {
            SecMasterBaseObj secMasterObj = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return secMasterObj;
        }
    }
}
