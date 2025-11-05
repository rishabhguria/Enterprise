using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.SecurityMasterNew
{
    internal class SecMasterValidationManager
    {

        /// <summary>
        /// Validate Secuirty
        /// </summary>
        /// <param name="secMasterObj"></param>
        internal static void ValidateSecurity(SecMasterBaseObj secMasterObj)
        {

            try
            {
                //cehck AUEC is valid or not
                CheckAUEC(secMasterObj);

                // if security is new then check already exists or not 
                if (secMasterObj.SymbolType.Equals((int)SymbolType.New))
                    CheckSecurityExists(secMasterObj);

                // if security is updated then check trades for symbol is  exists or not 
                if (secMasterObj.SymbolType.Equals((int)SymbolType.Updated))
                {
                    CheckTradeExists(secMasterObj);
                }


                //basic Checks on Security
                ValidateBasicDetails(secMasterObj);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        // Added By : Manvendra P.
        //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-7205
        // Basic check for FX and FxForward asset Class

        private static void ValidateAssetClassFxData(SecMasterFxObj secMasterFxObj)
        {
            if (secMasterFxObj.LeadCurrencyID == int.MinValue)
            {
                secMasterFxObj.ErrorMessage = "Not Saved, Please enter Lead Currency";
            }
            if (secMasterFxObj.VsCurrencyID == int.MinValue)
            {
                secMasterFxObj.ErrorMessage = "Not Saved, Please enter Vs Currency";
            }
        }

        private static void ValidateAssetClassFxForwardData(SecMasterFXForwardObj secMasterFxFrwdObj)
        {
            if (secMasterFxFrwdObj.LeadCurrencyID == int.MinValue)
            {
                secMasterFxFrwdObj.ErrorMessage = "Not Saved, Please enter Lead Currency";
            }
            if (secMasterFxFrwdObj.VsCurrencyID == int.MinValue)
            {
                secMasterFxFrwdObj.ErrorMessage = "Not Saved, Please enter Vs Currency";
            }
            if (secMasterFxFrwdObj.ExpirationDate == DateTimeConstants.MinValue)
            {
                secMasterFxFrwdObj.ErrorMessage = "Not Saved, Please enter Expiration Date";
            }
        }

        /// <summary>
        /// Basic validation on common fields 
        /// </summary>
        /// <param name="secMasterObj"></param>
        private static void ValidateBasicDetails(SecMasterBaseObj secMasterObj)
        {
            if (string.IsNullOrEmpty(secMasterObj.TickerSymbol))
            {
                secMasterObj.ErrorMessage = " Not Saved, Please enter Ticker Symbol";


            }
            if (string.IsNullOrEmpty(secMasterObj.LongName))
            {
                secMasterObj.ErrorMessage = "Not Saved, Please enter Long Name";


            }
            if (secMasterObj.CurrencyID == int.MinValue)
            {
                secMasterObj.ErrorMessage = "Not Saved, Please enter Currency ";


            }
            //if (secMasterObj.Multiplier == 0.0)
            //{
            //    secMasterObj.ErrorMessage = "Not Saved, Mutiplier can not be Zero ";


            //}
            if (Math.Round(secMasterObj.RoundLot, 10) <= 0)
            {
                secMasterObj.ErrorMessage = "Not Saved, RoundLots can not be less than equal to 0 ";


            }

            if (secMasterObj.AssetCategory == AssetCategory.FX)
            {
                SecMasterFxObj secMasterFxObj = secMasterObj as SecMasterFxObj;
                if (secMasterFxObj != null)
                {
                    ValidateAssetClassFxData(secMasterFxObj);
                }
            }
            if (secMasterObj.AssetCategory == AssetCategory.FXForward)
            {
                SecMasterFXForwardObj secMasterFxForwardObj = secMasterObj as SecMasterFXForwardObj;
                if (secMasterFxForwardObj != null)
                {
                    ValidateAssetClassFxForwardData(secMasterFxForwardObj);
                }
            }
        }

        //private String CheckDatesValidation(SecMasterBaseObj secMasterObj)
        //{
        //    StringBuilder error = new StringBuilder();
        //    try
        //    {
        //        //SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
        //        //ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
        //        //secMasterRequestObj.AddData(secMasterObj.TickerSymbol, symbology);

        //        //List<SecMasterBaseObj> existingSecurity = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterRequestObj, DateTime.UtcNow);

        //        //if (secMasterObj.SymbolType.Equals(SymbolType.New) && existingSecurity.Count > 0)
        //        //{
        //        //    error.Append(SecMasterConstants.SecMasterComments.SymbolExistsInSM.ToString());
        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return error.ToString();

        //}

        /// <summary>
        /// Check Security already Exists in DB
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <returns></returns>
        private static String CheckSecurityExists(SecMasterBaseObj secMasterObj)
        {
            StringBuilder error = new StringBuilder();
            try
            {
                //request for security for security already exists or not
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                secMasterRequestObj.AddData(secMasterObj.TickerSymbol, symbology);

                List<SecMasterBaseObj> existingSecurity = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterRequestObj, DateTime.UtcNow);

                if (existingSecurity.Count > 0)
                {

                    error.Append("Symbol already exists!");
                }

                secMasterObj.ErrorMessage = error.ToString();

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return error.ToString();

        }

        /// <summary>
        /// Check trades for updated security is in DB or not, Because currently
        /// we are allowed to change AUECID fo symbol.
        /// TODO- Create function for change AUECID of traded symbols.
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <returns>Validation Message</returns>
        private static String CheckTradeExists(SecMasterBaseObj secMasterObj)
        {
            StringBuilder error = new StringBuilder();
            try
            {
                //request for security already Exists
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                secMasterRequestObj.AddData(secMasterObj.TickerSymbol, symbology);

                List<SecMasterBaseObj> existingSecurity = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterRequestObj, DateTime.UtcNow);

                // if AUECID has changed then check trades exists or not
                Boolean isTradeExists = false;

                // check for AUECID has been changed or not
                if (existingSecurity.Count > 0 && secMasterObj.AUECID != existingSecurity[0].AUECID)
                {
                    //TODO Better solution for check  symbol traded
                    SecMasterRequestObj secMasterReqObj = CachedDataManager.GetInstance.GetAlltradedSymbols();

                    foreach (SymbolDataRow symbolData in secMasterReqObj.SymbolDataRowCollection)
                    {
                        if (symbolData.PrimarySymbol.Equals(secMasterObj.TickerSymbol))
                        {
                            isTradeExists = true;
                            break;
                        }
                    }
                }

                //https://jira.nirvanasolutions.com:8443/browse/PRANA-19274
                //Here we will check if the ticker symbol which user is trying to modify is traded or not.
                // Changes will be allowed only if the the symbol and none of its derivates are traded.
                //We are using Symbol_PK because, when we update ticker symbol, we get new ticker symbol (updated) in SecMasterBaseObj,
                // and we cannot use that to find if the original symbol is traded or not. Only Symbol_pk remains same.

                //PRANA-28411
                // Applied check for Indices to avoid going inside the method as Indices do not get Traded.
                else if (existingSecurity.Count == 0 && !secMasterObj.AssetCategory.Equals(AssetCategory.Indices))
                {
                    isTradeExists = CachedDataManager.GetInstance.IsSymbolTraded(secMasterObj.Symbol_PK);
                }
                if (isTradeExists)
                {
                    error.Append("Either Symbol or its derivatives are already traded, cannot modify security details.");
                }
                secMasterObj.ErrorMessage = error.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return error.ToString();
        }

        private static String CheckAUEC(SecMasterBaseObj secMasterObj)
        {
            StringBuilder error = new StringBuilder();
            try
            {
                //Check Asset has been set for secuirty or not
                if (secMasterObj.AssetID < 0)
                {
                    error.Append(SecMasterConstants.SecMasterComments.InvalidAssetID.ToString());
                    error.Append(",");
                }
                //Check UnderLyingID has been set for secuirty or not
                if (secMasterObj.UnderLyingID < 0)
                {
                    error.Append(SecMasterConstants.SecMasterComments.InvalidUnderLyingID.ToString());
                    error.Append(",");
                }
                //Check ExchangeID has been set for secuirty or not
                if (secMasterObj.ExchangeID < 0)
                {
                    error.Append(SecMasterConstants.SecMasterComments.InvalidExchangeID.ToString());
                    error.Append(",");
                }
                //Check CurrencyID has been set for secuirty or not
                if (secMasterObj.CurrencyID < 0)
                {
                    error.Append(SecMasterConstants.SecMasterComments.InvalidCurrencyID.ToString());
                    error.Append(",");
                }

                int AUECID = int.MinValue;
                if (secMasterObj.AssetID > 0 && secMasterObj.UnderLyingID > 0 && secMasterObj.ExchangeID > 0)
                    AUECID = CachedDataManager.GetInstance.GetAUECID(secMasterObj.AssetID, secMasterObj.UnderLyingID, secMasterObj.ExchangeID);

                //check combination of Asset, underlying and exchange is valid or not
                if (secMasterObj.AUECID != AUECID || secMasterObj.AUECID < 0)
                {
                    error.Append(SecMasterConstants.SecMasterComments.InvalidAUECID.ToString());

                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return error.ToString();
        }

    }
}
