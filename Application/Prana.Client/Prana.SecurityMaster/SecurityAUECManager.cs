using System;
using System.Collections.Generic;
using System.Text;

using Prana.CommonDataCache;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;

namespace Prana.SecurityMaster
{
    public class SecurityAUECManager
    {

        public static void GetAUECInfoFromPranaSymbol(string pranaSymbol, ref AssetCategory assetType, ref Underlying underlying, ref int exchangeID, ref int currencyId, ref int auecID)
        {
            #region Get Asset Type

            string exchangeIdentifier = string.Empty;

            string[] firstSplit = pranaSymbol.Split(' ');

            if (!String.IsNullOrEmpty(pranaSymbol))
            {
                if (firstSplit.Length == 1)
                {
                    assetType = AssetCategory.Equity;
                    string[] matMonthExchange = firstSplit[0].Split('-');
                    if (matMonthExchange.Length > 1)
                    {
                        exchangeIdentifier = matMonthExchange[1];
                    }
                    else
                    {
                        if (matMonthExchange[0].Length <= 3)
                            exchangeIdentifier = "NYSE";
                        else
                            exchangeIdentifier = "NASD";
                    }

                }
                else
                {
                    assetType = AssetCategory.Future;
                    string[] matMonthExchange = firstSplit[1].Split('-');
                    if (matMonthExchange.Length > 1)
                    {
                        exchangeIdentifier = matMonthExchange[1];
                    }
                }
            }

            #endregion Get Asset Type

            #region Get Auec and Underlying on basis of AssetType
            // Get the auec
            auecID = CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier(exchangeIdentifier + "-" + assetType.ToString());
           
            if (auecID != int.MinValue)
            {
                CachedDataManager.GetInstance.GetUnderlyingExchangeIDFromAUECID(auecID, ref underlying, ref exchangeID);
                // TODO : Currency - pickup from securitymaster
                currencyId = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(auecID);
            }
            #endregion Get Auec and Underlying on basis of AssetType

        }


        ///Logic applied by taking the following use cases in symbols only
        ///Possible type of symbols are ...
        /// Equities
        ///     0008-HKG
        ///     MSFT
        /// Equity Options
        ///     MSQ LA
        /// Futures
        ///     NI225 U7-SI
        ///     ES #F
        ///     ES U7
        public static AssetCategory GetAssetCategoryFromPranaSymbol(string pranaSymbol)
        {
            if (String.IsNullOrEmpty(pranaSymbol))
            {
                throw new ArgumentException("Supplied prana symbol is null or empty");
            }

            string[] firstSplit = pranaSymbol.Split(' ');

            if (firstSplit.Length == 1)
            {
                ///Assuming that only equity symbols don't have any spaces. 
                return AssetCategory.Equity;
            }
            else
            {
                string[] matMonthExchange = firstSplit[0].Split('-');
                if (matMonthExchange.Length > 1)
                {
                    ///Assuming that only future symbols have "-" in symbol. 
                    return AssetCategory.Future;
                }
                else if (matMonthExchange.Length == 1)
                {
                    char[] matMonthExchangeFirstPartArr = matMonthExchange[0].ToCharArray();
                    ///Assuming that future symbol's first part after space will have the first character as # or second character as digit
                    if (matMonthExchangeFirstPartArr[0].Equals('#') || char.IsDigit(matMonthExchangeFirstPartArr[1]))
                    {
                        return AssetCategory.Future;
                    }
                    else
                    {
                        return AssetCategory.EquityOption;
                    }
                }
            }
            return AssetCategory.None;
        }


    }
}
