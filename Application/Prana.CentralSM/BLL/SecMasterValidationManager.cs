using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Prana.CentralSMDataCache;

namespace Prana.CentralSM
{
    internal class SecMasterValidationManager
    {
        private static SecMasterValidationManager _secMasterValidationManager = null;
        static object _locker = new object();

        /// <summary>
        /// Get Instance of validation manager
        /// </summary>
        public static SecMasterValidationManager GetInstance
        {
            get
            {
                if (_secMasterValidationManager == null)
                {
                    lock (_locker)
                    {
                        if (_secMasterValidationManager == null)
                        {
                            _secMasterValidationManager = new SecMasterValidationManager();
                        }
                    }
                }
                return _secMasterValidationManager;
            }
        }
        ///// <summary>
        ///// Validate Secuirty
        ///// </summary>
        ///// <param name="secMasterObj"></param>
        //internal void ValidateSecurity(SecMasterBaseObj secMasterObj)
        //{

        //    try
        //    {
        //        //cehck AUEC is valid or not
        //        CheckAUEC(secMasterObj);

        //        // if security is new then check already exists or not 
        //        if (secMasterObj.SymbolType.Equals((int)SymbolType.New))
        //            CheckSecurityExists(secMasterObj);

        //        // if security is updated then check trades for symbol is  exists or not 
        //        if (secMasterObj.SymbolType.Equals((int)SymbolType.Updated))
        //        {
        //            CheckTradeExists(secMasterObj);
        //        }


        //        //basic Checks on Security
        //        ValidateBasicDetails(secMasterObj);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}


        /// <summary>
        /// Basic validation on common fields 
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void ValidateBasicDetails(SecMasterBaseObj secMasterObj)
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
            if (secMasterObj.Multiplier == 0.0)
            {
                secMasterObj.ErrorMessage = "Not Saved, Mutiplier can not be Zero ";


            }
            if (secMasterObj.RoundLot < 1)
            {
                secMasterObj.ErrorMessage = "Not Saved, RoundLots can not be less than 1 ";


            }
        }

        private String CheckDatesValidation(SecMasterBaseObj secMasterObj)
        {
            StringBuilder error = new StringBuilder();
            try
            {
                //SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                //ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                //secMasterRequestObj.AddData(secMasterObj.TickerSymbol, symbology);

                //List<SecMasterBaseObj> existingSecurity = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterRequestObj, DateTime.UtcNow);

                //if (secMasterObj.SymbolType.Equals(SymbolType.New) && existingSecurity.Count > 0)
                //{
                //    error.Append(SecMasterConstants.SecMasterComments.SymbolExistsInSM.ToString());
                //}


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
            return error.ToString();

        }

        /// <summary>
        /// Check Security already Exists in DB
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <returns></returns>
        private String CheckSecurityExists(SecMasterBaseObj secMasterObj)
        {
            StringBuilder error = new StringBuilder();
            try
            {
                //request for security for security already exists or not
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                secMasterRequestObj.AddData(secMasterObj.TickerSymbol, symbology);

                List<SecMasterBaseObj> existingSecurity = CentralSMDataCache.CentralSMDataCache.Instance.GetSecMasterDataFromCache(secMasterRequestObj);

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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return error.ToString();

        }

        ///// <summary>
        ///// Check trades for updated security is in DB or not, Because currently
        ///// we are allowed to change AUECID fo symbol.
        ///// TODO- Create function for change AUECID of traded symbols.
        ///// </summary>
        ///// <param name="secMasterObj"></param>
        ///// <returns>Validation Message</returns>
        //private String CheckTradeExists(SecMasterBaseObj secMasterObj)
        //{
        //    StringBuilder error = new StringBuilder();
        //    try
        //    {
        //        //request for security already Exists
        //        SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
        //        ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
        //        secMasterRequestObj.AddData(secMasterObj.TickerSymbol, symbology);

        //        List<SecMasterBaseObj> existingSecurity = CentralSMDataCache.Instance.GetSecMasterData(secMasterRequestObj, DateTime.UtcNow);

        //        // check for AUECID has been changed or not
        //        if (existingSecurity.Count > 0 && secMasterObj.AUECID != existingSecurity[0].AUECID)
        //        {
        //            // if AUECID has changed then check trades exists or not
        //            Boolean isTradeExists = false;

        //            //TODO Better solution for check  symbol traded
        //            SecMasterRequestObj secMasterReqObj = CachedDataManager.GetInstance.GetAlltradedSymbols();

        //            foreach (SymbolDataRow symbolData in secMasterReqObj.SymbolDataRowCollection)
        //            {
        //                if (symbolData.PrimarySymbol.Equals(secMasterObj.TickerSymbol))
        //                {
        //                    isTradeExists = true;
        //                    break;
        //                }
        //            }
        //            //if trade exists
        //            if (isTradeExists)
        //            {
        //                error.Append("Can not change AUEC, Trade already exists!");

        //            }
        //        }

        //        secMasterObj.ErrorMessage = error.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return error.ToString();

        //}

        //private String CheckAUEC(SecMasterBaseObj secMasterObj)
        //{
        //    StringBuilder error = new StringBuilder();
        //    try
        //    {
        //        //Check Asset has been set for secuirty or not
        //        if (secMasterObj.AssetID < 0)
        //        {
        //            error.Append(SecMasterConstants.SecMasterComments.InvalidAssetID.ToString());
        //            error.Append(",");
        //        }
        //        //Check UnderLyingID has been set for secuirty or not
        //        if (secMasterObj.UnderLyingID < 0)
        //        {
        //            error.Append(SecMasterConstants.SecMasterComments.InvalidUnderLyingID.ToString());
        //            error.Append(",");
        //        }
        //        //Check ExchangeID has been set for secuirty or not
        //        if (secMasterObj.ExchangeID < 0)
        //        {
        //            error.Append(SecMasterConstants.SecMasterComments.InvalidExchangeID.ToString());
        //            error.Append(",");
        //        }
        //        //Check CurrencyID has been set for secuirty or not
        //        if (secMasterObj.CurrencyID < 0)
        //        {
        //            error.Append(SecMasterConstants.SecMasterComments.InvalidCurrencyID.ToString());
        //            error.Append(",");
        //        }

        //        int AUECID = int.MinValue;
        //        if (secMasterObj.AssetID > 0 && secMasterObj.UnderLyingID > 0 && secMasterObj.ExchangeID > 0)
        //            AUECID = CachedDataManager.GetInstance.GetAUECID(secMasterObj.AssetID, secMasterObj.UnderLyingID, secMasterObj.ExchangeID);

        //        //check combination of Asset, underlying and exchange is valid or not
        //        if (secMasterObj.AUECID != AUECID || secMasterObj.AUECID < 0)
        //        {
        //            error.Append(SecMasterConstants.SecMasterComments.InvalidAUECID.ToString());

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return error.ToString();
        //}

    }
}