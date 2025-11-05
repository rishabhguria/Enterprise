using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OptionCalculator.Common;
using Prana.PricingAnalysisModels;
using Prana.SocketCommunication;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Prana.OptionCalculator.CalculationComponent
{
    public class GreeksCalculator
    {
        IQueueProcessor _greekCalculatedQueue = null;
        IPricingAnalysisModel _pricingAnalysisModel = null;
        public WaitHandle event1;

        /// <summary>
        /// Intermediate cache of implied vol which will  be cleared after every refresh interval set for implied vol in app.config file.
        /// </summary>

        public GreeksCalculator()
        {
            SetPricingModel();
        }

        public GreeksCalculator(IQueueProcessor greekCalculatedQueue, int number)
            : this()
        {
            try
            {
                _greekCalculatedQueue = greekCalculatedQueue;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void SetPricingModel()
        {
            try
            {
                _pricingAnalysisModel = PricingAnalysisModelFactory.GetPricingAnalysisModel();
                if (_pricingAnalysisModel != null)
                {
                    _pricingAnalysisModel.CurrentPricingAnalysisModel = OptionInputValuesCache.SelectedPricingModel;
                    _pricingAnalysisModel.BinomialSteps = OptionInputValuesCache.BinomialSteps;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void CalculateGreek(QueueMessage message, WaitHandle waitHandle)
        {
            try
            {
                event1 = waitHandle;
                ThreadPool.QueueUserWorkItem(new WaitCallback(Calculate), message);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Greeks calculations are done here 
        /// </summary>
        private void Calculate(object msg)
        {
            try
            {
                QueueMessage message = (QueueMessage)msg;

                Dictionary<string, SymbolData> calculatedGreeks = new Dictionary<string, SymbolData>();
                Dictionary<string, SymbolData> dataColl = (Dictionary<string, SymbolData>)message.Message;


                if (_pricingAnalysisModel != null)
                {
                    _pricingAnalysisModel.CurrentPricingAnalysisModel = OptionInputValuesCache.SelectedPricingModel;
                    _pricingAnalysisModel.BinomialSteps = OptionInputValuesCache.BinomialSteps;
                }

                switch (message.MsgType)
                {
                    case OptionDataFormatter.MSGTYPE_PricingData:
                    case OptionDataFormatter.MSGTYPE_PricingDataFile:

                        foreach (KeyValuePair<string, SymbolData> data in dataColl)
                        {
                            OptionSymbolData opData = data.Value as OptionSymbolData;
                            SymbolData underlyingData = null;
                            if (opData != null)
                            {

                                if (!string.IsNullOrEmpty(opData.UnderlyingSymbol))
                                {
                                    if (dataColl.ContainsKey(opData.UnderlyingSymbol))
                                    {
                                        underlyingData = dataColl[opData.UnderlyingSymbol];
                                        if (underlyingData != null)
                                        {
                                            // placed a preventive check on the strike price and SelectedFeedPrice since it might be possible that in this particular greeks calculation cycle
                                            //the symbol data object is not updated from LiveFeed and has been added with default values while appending data in DataCopyComponent
                                            //This check will automatically be true once the live feed response comes for this symbol...
                                            if (opData.StrikePrice > 0)
                                            {
                                                CalculateGreeks(opData, underlyingData);
                                            }
                                        }
                                        else
                                        {
                                            if (LoggingConstants.LoggingEnabled)
                                            {
                                                Logger.HandleException(new Exception("The underlyind data is not available for symbol : " + opData.Symbol + "\t Underlying Symbol :" + opData.UnderlyingSymbol), LoggingConstants.POLICY_LOGONLY);
                                            }
                                        }
                                    }
                                    //As discussed with Mukul, Placed this outside IF check because there were some symbols whose strike price was coming negative and due to this check they were not 
                                    // added to the dictionery and thus data were not flowing properly.

                                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2731 : Kuldeep Agrawal
                                    // If the prices for underlying symbols from live feed are not coming and also not over-ridden from PI then for current 
                                    // day trades data for options is also not flowing properly.
                                    calculatedGreeks.Add(data.Key, opData);
                                }
                                else
                                {
                                    if (LoggingConstants.LoggingEnabled)
                                    {
                                        Logger.HandleException(new Exception("The underlying symbol is not available for symbol : " + opData.Symbol), LoggingConstants.POLICY_LOGONLY);
                                    }
                                }
                                #region Default Delta Calculations
                                // http://jira.nirvanasolutions.com:8080/browse/PRANA-6863 : Kuldeep Agrawal
                                if (underlyingData != null && opData.DeltaSource == DeltaSource.Default)
                                {
                                    // Setting delta equals to 0 if Option is Out Of The Money or Expired.
                                    if (opData.DaysToExpiration < 0 || (opData.PutOrCall == OptionType.CALL && opData.StrikePrice > underlyingData.SelectedFeedPrice) || (opData.PutOrCall == OptionType.PUT && opData.StrikePrice < underlyingData.SelectedFeedPrice) || (opData.StrikePrice == underlyingData.SelectedFeedPrice))
                                    {
                                        opData.Delta = 0;
                                    }
                                    else
                                    {
                                        // In The Money Settings for non-expired options.
                                        if (opData.PutOrCall == OptionType.CALL && opData.StrikePrice < underlyingData.SelectedFeedPrice)
                                        {
                                            opData.Delta = 1;
                                        }
                                        if (opData.PutOrCall == OptionType.PUT && opData.StrikePrice > underlyingData.SelectedFeedPrice)
                                        {
                                            opData.Delta = -1;
                                        }
                                    }
                                }
                                else if (underlyingData == null)
                                {
                                    opData.Delta = 0;
                                    opData.DeltaSource = DeltaSource.Default;
                                }
                                #endregion
                            }
                            else
                            {
                                calculatedGreeks.Add(data.Key, data.Value);
                            }
                        }
                        //send the message
                        if (calculatedGreeks.Count > 0)
                        {
                            QueueMessage queueMsg = new QueueMessage(message.MsgType, string.Empty, string.Empty, OptionDataFormatter.CreateMsgForOptionGreeks(calculatedGreeks, message.MsgType));
                            _greekCalculatedQueue.SendMessage(queueMsg);
                        }

                        #region CommentedCode
                        //if (SubscriberCollection.SubscriberCollectionData.Count == 0)
                        //{
                        //    return;
                        //}
                        //else
                        //{
                        //    foreach (KeyValuePair<string, SubscriberData> suscriberDataKeyValuePair in SubscriberCollection.SubscriberCollectionData)
                        //    {
                        //        Dictionary<string, SymbolData> userOptionData = suscriberDataKeyValuePair.Value.SetSymbolsData(calculatedGreeks);
                        //        if (userOptionData.Count > 0)
                        //        {
                        //            QueueMessage queueMsg = new QueueMessage(OptionDataFormatter.MSGTYPE_PricingData, string.Empty, string.Empty, OptionDataFormatter.CreateMsgForOptionGreeks(userOptionData, OptionDataFormatter.MSGTYPE_PricingData));
                        //            //send the message
                        //            _greekCalculatedQueue.SendMessage(queueMsg);
                        //        }
                        //    }
                        //}
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (event1 != null)
                    ((AutoResetEvent)event1).Set();
            }
        }

        /// <summary>
        /// Calculates Greeks Delta , Vega , Rho,Thetha for Live Feed Data        ///  
        /// </summary>
        /// <param name="SymbolData"></param>
        /// <returns></returns>
        private void CalculateGreeks(OptionSymbolData opData, SymbolData underlyingData)
        {
            try
            {
                if (opData != null)
                {
                    DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(opData.AUECID);
                    DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(opData.AUECID));
                    auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                    TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                    DateTime expirationDate = opData.ExpirationDate.AddDays(ts2.TotalDays);
                    TimeSpan ts = expirationDate.Subtract(auecLocalTime);
                    double daysToExpiration = ts.TotalDays;
                    if (FeedPriceChooser.UseClosingMark)
                    {
                        DateTime previousDate = BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(auecMarketEndTime, opData.AUECID);
                        TimeSpan ts1 = auecMarketEndTime.Date.Subtract(previousDate.Date);
                        daysToExpiration = daysToExpiration + ts1.Days;
                    }
                    // modification for Windale input DaysToExpiration should be in number of years 
                    //daysToExpiration = daysToExpiration > 0 ? daysToExpiration : 1;
                    daysToExpiration = daysToExpiration < 0 ? 0 : daysToExpiration;
                    double timeForExpirationinYrs = daysToExpiration * 1.0 / 365.0;
                    if (opData.InterestRate == 0.25)
                    {
                        opData.InterestRate = RiskPreferenceManager.GetInterestRate(daysToExpiration);
                    }
                    if (opData.FinalInterestRate == 0.25)
                    {
                        opData.FinalInterestRate = opData.InterestRate;
                    }
                    if (underlyingData.FinalDividendYield == -1)
                    {
                        underlyingData.FinalDividendYield = underlyingData.DividendYield;
                    }
                    //OptionGreeks opData = new OptionGreeks();

                    //Harsh: removed as not was used...need to ensure that Trim and upper are not required
                    //string optSymbol = pricingmodelData.OptSymbol.Trim().ToUpper();

                    double underlyingStockPrice = underlyingData.SelectedFeedPrice;
                    double impliedVol = 0;
                    if (underlyingStockPrice != 0 && opData.SelectedFeedPrice != 0)
                        impliedVol = CalculateImpliedVol(Math.Abs(underlyingStockPrice), opData.FinalInterestRate, daysToExpiration, Math.Abs(opData.SelectedFeedPrice), opData.UnderlyingSymbol, opData.BloombergSymbol, opData.StrikePrice, opData.PutOrCall, opData.AUECID);
                    opData.ImpliedVol = impliedVol;

                    if (opData.FinalImpliedVol == -1)
                    {
                        opData.FinalImpliedVol = opData.ImpliedVol;
                    }

                    // here we are calculating the delta value based on the OMI overridden Final Inputs
                    double cdelta = 2, pDelta = -2, deltaCalculated = 0, cTheta = 0, pTheta = 0, gamma = 0, cRho = 0, pRho = 0, vega = 0;

                    if (_pricingAnalysisModel != null && underlyingStockPrice != 0 && daysToExpiration > 0 && opData.DeltaSource != DeltaSource.UserDefined)
                        _pricingAnalysisModel.GetDelta(Math.Abs(underlyingStockPrice), opData.StrikePrice, opData.FinalInterestRate, timeForExpirationinYrs, opData.FinalImpliedVol, underlyingData.FinalDividendYield - underlyingData.StockBorrowCost, ref cdelta, ref pDelta);

                    double liveFeedPrice = opData.SelectedFeedPrice;
                    UserOptModelInput optInputs = OptionModelUserInputCache.GetUserOMIData(opData.Symbol);
                    opData.TheoreticalPrice = opData.SelectedFeedPrice;
                    if (opData.PutOrCall == OptionType.CALL) // put 
                    {
                        //opData.Delta = cdelta;
                        deltaCalculated = cdelta;
                        opData.Theta = cTheta;
                        opData.Rho = cRho;
                        if (optInputs != null)
                        {
                            if (!optInputs.LastPriceUsed && !FeedPriceChooser.UseClosingMark)
                            {
                                if (optInputs.TheoreticalPriceUsed.Equals(true))
                                {
                                    if (_pricingAnalysisModel != null && underlyingStockPrice != 0 && daysToExpiration > 0)
                                        opData.TheoreticalPrice = _pricingAnalysisModel.GetCallPrice(Math.Abs(underlyingStockPrice), opData.StrikePrice, opData.FinalInterestRate, timeForExpirationinYrs, opData.FinalImpliedVol, opData.FinalDividendYield - opData.StockBorrowCost);

                                    opData.SelectedFeedPrice = opData.TheoreticalPrice;
                                    opData.PreferencedPrice = opData.SelectedFeedPrice;
                                }
                            }
                        }
                    }
                    else
                    {
                        //opData.Delta = pDelta;
                        deltaCalculated = pDelta;
                        opData.Theta = pTheta;
                        opData.Rho = pRho;
                        if (optInputs != null)
                        {
                            if (!optInputs.LastPriceUsed && !FeedPriceChooser.UseClosingMark)
                            {
                                if (optInputs.TheoreticalPriceUsed.Equals(true))
                                {
                                    if (_pricingAnalysisModel != null && underlyingStockPrice != 0 && daysToExpiration > 0)
                                        opData.TheoreticalPrice = _pricingAnalysisModel.GetPutPrice(Math.Abs(underlyingStockPrice), opData.StrikePrice, opData.InterestRate, timeForExpirationinYrs, opData.FinalImpliedVol, opData.FinalDividendYield - opData.StockBorrowCost);

                                    opData.SelectedFeedPrice = opData.TheoreticalPrice;
                                    opData.PreferencedPrice = opData.SelectedFeedPrice;
                                }
                            }
                        }
                    }
                    opData.Theta = opData.Theta / 365.0;
                    #region Set Values in Greek Object
                    opData.Gamma = gamma;
                    opData.Vega = vega;

                    // DEEP ITM and DEEP OTM Checks  -- suggested by Alan
                    if (opData.FinalImpliedVol == 0)
                    {
                        // Moved this logic in below.
                        //if (opData.PutOrCall == OptionType.Call)
                        //{
                        //    opData.Delta = 1;
                        //}
                        //else
                        //{
                        //    opData.Delta = -1;
                        //}
                        opData.Gamma = 0;
                        opData.Theta = 0;
                        opData.Vega = 0;
                        opData.Rho = 0;
                    }
                    // Give highest preference to User Defined Delta
                    if (opData.DeltaSource != DeltaSource.UserDefined && !FeedPriceChooser.UseDefaultDelta)
                    {
                        // If deltaCalculated is 2 or -2 then it means that the delta was not calculated as calculated value of delta can only be between
                        // -1 to 1.
                        if (deltaCalculated != 2 && deltaCalculated != -2 && opData.FinalImpliedVol != 0)
                        {
                            opData.Delta = deltaCalculated;
                            opData.DeltaSource = DeltaSource.Calculated;
                        }
                    }
                    #endregion
                    #region Debug Mode Logging
                    if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("OptSymbol :" + opData.Symbol);
                        sb.Append("\t \t");
                        sb.Append("UnderlyingSymbol :" + opData.UnderlyingSymbol);
                        sb.Append("\t");
                        sb.Append("PutOrCall :" + opData.PutOrCall);
                        sb.Append("\t");
                        sb.Append("StockPrice :" + underlyingData.SelectedFeedPrice + "(OMI overridden)");
                        sb.Append("\t");
                        sb.Append("StrikePrice :" + opData.StrikePrice);
                        sb.Append("\t");
                        sb.Append("InterestRate :" + opData.FinalInterestRate + "(OMI overridden)");
                        sb.Append("\t");
                        sb.Append("DaysToExpiration :" + daysToExpiration);
                        sb.Append("\t");
                        sb.Append("timeForExpirationinYrs :" + timeForExpirationinYrs);
                        sb.Append("\t");
                        sb.Append("OMI overridden DividendYield : " + underlyingData.FinalDividendYield);
                        sb.Append("\t");
                        sb.Append("OMI overridden Stock Borrow Cost : " + underlyingData.StockBorrowCost);
                        sb.Append("\t");
                        sb.Append("Stock Borrow Cost calculated DividendYield : " + (underlyingData.FinalDividendYield - underlyingData.StockBorrowCost));
                        sb.Append("\t");
                        sb.Append("OptionPrice(Live Feed) :" + liveFeedPrice + "(OMI overridden)");
                        sb.Append("\t");
                        sb.Append("OptionPrice(Theoretical) :" + opData.TheoreticalPrice);
                        sb.Append("\t");
                        sb.Append("VolatilityIterations :" + OptionInputValuesCache.VolatilityIterations);
                        sb.Append("\t");
                        sb.Append("PricingAnalysisModel :" + OptionInputValuesCache.SelectedPricingModel.ToString());
                        sb.Append("\t");
                        sb.Append("Binomial Steps :" + OptionInputValuesCache.BinomialSteps.ToString());
                        if (opData.ExpirationDate == DateTime.MinValue || opData.DaysToExpiration <= 0 || opData.StrikePrice <= 0 || underlyingData.SelectedFeedPrice <= 0)
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.Write("Symbol : " + opData.Symbol + "\t Expiration Date :" + opData.ExpirationDate + "\t" + "Days To Expiration :" + opData.DaysToExpiration + "\t" + "Option Type :" + opData.PutOrCall + "\t" + "Underlying Price :" + underlyingData.SelectedFeedPrice + "\t" + "Strike Price :" + opData.StrikePrice, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        }
                        LogAndDisplayOnInformationReporter.GetInstance.Write((sb.ToString()), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Calculated IV : " + opData.ImpliedVol + "\t OMI overridden ImpliedVolitility :" + opData.FinalImpliedVol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        LogAndDisplayOnInformationReporter.GetInstance.Write("OMI overridden Delta : " + opData.Delta + "\t Calculated CallDelta :" + cdelta + "\t" + "Calculated PutDelta :" + pDelta, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //public OptionGreeks CalculateGreeksForStaticData(InputParametersForGreeks inputParams)
        //{
        //    // when days to expiration is zero, we assume it is due to simulation and take limiting case to be day 1
        //    double days = inputParams.DaysToExpiration > 0 ? inputParams.DaysToExpiration : .00001;
        //    // modification for Windale input DaysToExpiration should be in number of years
        //    double timeForExpirationinYrs = days * 1.0 / 365.0;


        //    if (inputParams.InterestRate == 0)
        //    {
        //        inputParams.InterestRate = RiskPreferenceManager.GetInterestRate(days);
        //    }
        //    OptionGreeks optionGreeks = new OptionGreeks();
        //    // copy market price of Option in Last Price
        //    optionGreeks.OptionMarketPrice = inputParams.OptionPrice;
        //    //optionGreeks.StockPrice = inputParams.StockPrice;

        //    try
        //    {

        //        //TODo
        //        //inputParams.callPrice
        //        double Dividend = inputParams.DividendYield;
        //        if (inputParams.DividendYield == 0)
        //        {
        //             Dividend = OptionInputValuesCache.GetInstance.GetDividend(inputParams.UnderLysymbol);
        //        }
        //        double impliedVolitility = inputParams.ImpliedVol;
        //        if (inputParams.ImpliedVol == 0 || inputParams.ImpliedVol == double.MinValue) // if implied vol is not already provided by user Input
        //        {
        //            //impliedVolitility = OptionInputValuesCache.GetInstance.GetBaseImpliedVol(inputParams.Symbol);
        //            impliedVolitility = CalculateImpliedVol(inputParams.StockPrice, inputParams.InterestRate, days, inputParams.OptionPrice, inputParams.UnderLysymbol, inputParams.StrikePrice, inputParams.PutOrCall);
        //        }
        //        inputParams.ImpliedVol = impliedVolitility;

        //        double cdelta = 0, pDelta = 0, cTheta = 0, pTheta = 0, gamma = 0, cRho = 0, pRho = 0, vega = 0;
        //        if (inputParams.ImpliedVol >= 0 && inputParams.StockPrice > 0 && inputParams.InterestRate >=0)
        //        {
        //            if (inputParams.PutOrCall == 'C') 
        //            {
        //                inputParams.OptionPrice = pricingAnalysisModel.GetCallPrice(inputParams.StockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, impliedVolitility, Dividend);
        //            }
        //            else
        //            {
        //                inputParams.OptionPrice = pricingAnalysisModel.GetPutPrice(inputParams.StockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, impliedVolitility, Dividend);
        //            }
        //            pricingAnalysisModel.GetDelta(inputParams.StockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, impliedVolitility, Dividend, ref cdelta, ref pDelta);
        //            pricingAnalysisModel.GetGamma(inputParams.StockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, impliedVolitility, Dividend, ref gamma);
        //            pricingAnalysisModel.GetTheta(inputParams.StockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, impliedVolitility, Dividend, ref cTheta, ref pTheta);
        //            pricingAnalysisModel.GetVega(inputParams.StockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, impliedVolitility, Dividend, ref vega);
        //            pricingAnalysisModel.GetRho(inputParams.StockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, impliedVolitility, Dividend, ref cRho, ref pRho);
        //        } 

        //        #region Set Values in Greek Object
        //        if (inputParams.PutOrCall == 'C') // put 
        //        {
        //            optionGreeks.Delta = cdelta;
        //            optionGreeks.Theta = cTheta;
        //            optionGreeks.Rho = cRho;
        //        }
        //        else
        //        {
        //            optionGreeks.Delta = pDelta;
        //            optionGreeks.Theta = pTheta;
        //            optionGreeks.Rho = pRho;
        //        }
        //        optionGreeks.Theta = optionGreeks.Theta / 365.0;
        //        optionGreeks.Rho = optionGreeks.Rho / 100;
        //        optionGreeks.Vega = vega / 100;

        //        optionGreeks.Gamma = gamma;
        //        optionGreeks.ImpliedVol = impliedVolitility;
        //        optionGreeks.Symbol = inputParams.Symbol;
        //        optionGreeks.PutOrCall = inputParams.PutOrCall;
        //        optionGreeks.StrikePrice = inputParams.StrikePrice;

        //        optionGreeks.OptionPrice = inputParams.OptionPrice;
        //        optionGreeks.InterestRate = inputParams.InterestRate;
        //        optionGreeks.StockPrice =inputParams.StockPrice;
        //        optionGreeks.DaysToExpiration = inputParams.DaysToExpiration > 0 ? inputParams.DaysToExpiration : 0;
        //        optionGreeks.DivYield = Dividend;
        //        #endregion

        //        #region Invalid Output Checks
        //        // DEEP ITM and DEEP OTM Checks  -- suggested by Alan
        //        if (inputParams.ImpliedVol == 0)
        //        {
        //            if (inputParams.PutOrCall == 'C')
        //            {
        //                optionGreeks.Delta = 1;
        //            }
        //            else
        //            {
        //                optionGreeks.Delta = -1;
        //            }
        //            optionGreeks.Gamma = 0;
        //            optionGreeks.Theta = 0;
        //            optionGreeks.Vega = 0;
        //            optionGreeks.Rho = 0;
        //            return optionGreeks;
        //        }
        //        #endregion

        //        #region setting minimum Value OLD
        //        //if (Math.Abs(optionGreeks.Delta) < .0001)
        //        //{
        //        //    optionGreeks.Delta = 0;
        //        //}
        //        //if (Math.Abs(optionGreeks.Gamma) < .0001 || Math.Abs(optionGreeks.Gamma) == 999.0) //Invalid Value 999.0
        //        //{
        //        //    optionGreeks.Gamma = 0;
        //        //}
        //        //if (Math.Abs(optionGreeks.Vega) < .0001 || Math.Abs(optionGreeks.Vega) == 9.99) //Invalid Value 9.99
        //        //{
        //        //    optionGreeks.Vega = 0;
        //        //}
        //        //if (Math.Abs(optionGreeks.Rho) < .0001)
        //        //{
        //        //    optionGreeks.Rho = 0;
        //        //}
        //        //if (Math.Abs(optionGreeks.ImpliedVol) < .0001)
        //        //{
        //        //    optionGreeks.ImpliedVol = 0;
        //        //}
        //        //if (Math.Abs(optionGreeks.OptionPrice) < .0001)
        //        //{
        //        //    optionGreeks.OptionPrice = 0;
        //        //}

        //        //if (inputParams.PutOrCall == 'C')
        //        //{
        //        //    if (impliedVolitility == 0)
        //        //    {
        //        //        optionGreeks.Delta = 1; // In the money
        //        //    }
        //        //    else if (impliedVolitility == 100)
        //        //    {
        //        //        optionGreeks.Delta = 0; // Out of money
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    if (impliedVolitility == 0)
        //        //    {
        //        //        optionGreeks.Delta = -1; // In the money
        //        //    }
        //        //    else if (impliedVolitility == 100)
        //        //    {
        //        //        optionGreeks.Delta = 0; // Out of money
        //        //    }
        //        //}

        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return optionGreeks;

        //}

        public OptionGreeks CalculateGreeksForStaticDataNew(InputParametersForGreeks inputParams)
        {
            if (_pricingAnalysisModel != null)
            {
                _pricingAnalysisModel.CurrentPricingAnalysisModel = OptionInputValuesCache.SelectedPricingModel;
                _pricingAnalysisModel.BinomialSteps = OptionInputValuesCache.BinomialSteps;
            }

            OptionGreeks optionGreeks = new OptionGreeks();
            DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(inputParams.AUECID);
            DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(inputParams.AUECID));
            auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
            TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
            DateTime expirationDate = inputParams.ExpirationDate.AddDays(ts2.TotalDays);

            TimeSpan ts = expirationDate.Subtract(auecLocalTime);
            double days = ts.TotalDays;
            if (FeedPriceChooser.UseClosingMark)
            {
                DateTime previousDate = BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(auecMarketEndTime, inputParams.AUECID);
                TimeSpan ts1 = auecMarketEndTime.Date.Subtract(previousDate.Date);
                days = days + ts1.Days;
            }
            //Commented this portion, need to check impact with Bharat
            //days = days > 0 ? days : .00001;// when days to expiration is zero, we assume it is due to simulation and take limiting case to be day 1            
            days = days < 0 ? 0 : days;
            double timeForExpirationinYrs = days * 1.0 / 365.0;
            // OTHER NORMAL CASES    
            try
            {
                double underlyingStockPrice = inputParams.SimulatedUnderlyingStockPrice;
                double cdelta = 0, pDelta = 0, cTheta = 0, pTheta = 0, gamma = 0, cRho = 0, pRho = 0, vega = 0;
                if (_pricingAnalysisModel != null && inputParams.Volatility >= 0 && inputParams.SimulatedUnderlyingStockPrice > 0 && inputParams.InterestRate >= 0 && days > 0)
                {
                    if (inputParams.PutOrCalls == 'C')
                    {
                        optionGreeks.SimulatedPrice = _pricingAnalysisModel.GetCallPrice(underlyingStockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, inputParams.Volatility, inputParams.DividendYield);
                    }
                    else
                    {
                        optionGreeks.SimulatedPrice = _pricingAnalysisModel.GetPutPrice(underlyingStockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, inputParams.Volatility, inputParams.DividendYield);
                    }

                    _pricingAnalysisModel.GetDelta(underlyingStockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, inputParams.Volatility, inputParams.DividendYield, ref cdelta, ref pDelta);
                    _pricingAnalysisModel.GetGamma(underlyingStockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, inputParams.Volatility, inputParams.DividendYield, ref gamma);
                    _pricingAnalysisModel.GetTheta(underlyingStockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, inputParams.Volatility, inputParams.DividendYield, ref cTheta, ref pTheta);
                    _pricingAnalysisModel.GetVega(underlyingStockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, inputParams.Volatility, inputParams.DividendYield, ref vega);
                    _pricingAnalysisModel.GetRho(underlyingStockPrice, inputParams.StrikePrice, inputParams.InterestRate, timeForExpirationinYrs, inputParams.Volatility, inputParams.DividendYield, ref cRho, ref pRho);
                }

                #region Set Values in Greek Object

                if (inputParams.PutOrCalls == 'C')
                {
                    optionGreeks.Delta = cdelta;
                    optionGreeks.Theta = cTheta;
                    optionGreeks.Rho = cRho;
                }
                else
                {
                    optionGreeks.Delta = pDelta;
                    optionGreeks.Theta = pTheta;
                    optionGreeks.Rho = pRho;
                }
                optionGreeks.Theta = optionGreeks.Theta / 365.0;
                optionGreeks.Rho = optionGreeks.Rho / 100;
                optionGreeks.Vega = vega / 100;
                optionGreeks.Gamma = gamma;
                // optionGreeks.Gamma = gamma;
                optionGreeks.Volatility = inputParams.Volatility;
                optionGreeks.Symbol = inputParams.Symbol;
                optionGreeks.PutOrCalls = inputParams.PutOrCalls;
                optionGreeks.StrikePrice = inputParams.StrikePrice;

                optionGreeks.SelectedFeedPrice = inputParams.SimulatedPrice;
                optionGreeks.InterestRate = inputParams.InterestRate;
                optionGreeks.SimulatedUnderlyingStockPrice = inputParams.SimulatedUnderlyingStockPrice;
                //change int.Parse to Convert.ToInt32 for avoding parsing exception
                optionGreeks.DaysToExpiration = days > 0 ? Convert.ToInt32(days) : 0;
                optionGreeks.DividendYield = inputParams.DividendYield;
                #endregion

                #region Invalid Output Checks
                // DEEP ITM and DEEP OTM Checks  -- suggested by Alan
                if (inputParams.Volatility == 0)
                {
                    if (inputParams.PutOrCalls == 'C')
                    {
                        optionGreeks.Delta = 1;
                    }
                    else
                    {
                        optionGreeks.Delta = -1;
                    }
                    optionGreeks.Gamma = 0;
                    optionGreeks.Theta = 0;
                    optionGreeks.Vega = 0;
                    optionGreeks.Rho = 0;
                }
                #endregion
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return optionGreeks;
        }

        public double CalculateImpliedVol(double stockPrice, double interestRate, double daysofExpiration, double optionPrice, string underLyingSymbol, string BloombergSymbol, double strikePrice, OptionType putOrCall, int AuecID)
        {
            double impliedVolitility = 0;
            // modification for Windale input DaysToExpiration should be in number of years 
            try
            {
                //if (FeedPriceChooser.UseClosingMark)
                //{
                //    DateTime previousDate = BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(DateTime.UtcNow, AuecID);
                //    TimeSpan ts = DateTime.UtcNow.Date.Subtract(previousDate.Date);
                //    daysofExpiration = daysofExpiration + ts.Days;
                //}
                daysofExpiration = daysofExpiration > 0 ? daysofExpiration : 1;
                double timeForExpirationinYrs = daysofExpiration * 1.0 / 365.0;
                double Dividend = OptionInputValuesCache.GetInstance.GetDividend(underLyingSymbol);
                double stockBorrowCost = OptionInputValuesCache.GetInstance.GetStockBorrowCost(underLyingSymbol);
                if (Dividend > 1)
                {
                    if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("For Underlying Symbol: " + underLyingSymbol + " Dividend Yield: " + Dividend + " cannot be greater than 1 ", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                    return 0.0;
                }
                if (optionPrice == 0)
                {
                    optionPrice = .000001;
                }
                if (_pricingAnalysisModel != null)
                {
                    if (putOrCall == OptionType.CALL)
                    {
                        impliedVolitility = _pricingAnalysisModel.GetImpliedVolatility(stockPrice, strikePrice, interestRate, timeForExpirationinYrs, Dividend - stockBorrowCost, optionPrice, OptionInputValuesCache.VolatilityIterations);
                    }
                    else
                    {
                        impliedVolitility = _pricingAnalysisModel.GetImpliedVolatilityPut(stockPrice, strikePrice, interestRate, timeForExpirationinYrs, Dividend - stockBorrowCost, optionPrice, OptionInputValuesCache.VolatilityIterations);
                    }
                }
                return impliedVolitility;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return impliedVolitility;
        }

        public double GetTheoreticalPrice(PricingModelData pricingModelData, double interestRate)
        {
            double optionPrice = 0.0;
            try
            {
                if (_pricingAnalysisModel != null)
                {
                    _pricingAnalysisModel.CurrentPricingAnalysisModel = OptionInputValuesCache.SelectedPricingModel;
                    _pricingAnalysisModel.BinomialSteps = OptionInputValuesCache.BinomialSteps;
                }

                DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(pricingModelData.AUECID);
                DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(pricingModelData.AUECID));
                auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                DateTime expirationDate = pricingModelData.ExpirationDate.AddDays(ts2.TotalDays);

                TimeSpan ts = expirationDate.Subtract(auecLocalTime);
                double daysToExpiration = ts.TotalDays;
                if (FeedPriceChooser.UseClosingMark)
                {
                    DateTime previousDate = BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(auecMarketEndTime, pricingModelData.AUECID);
                    TimeSpan ts1 = auecMarketEndTime.Date.Subtract(previousDate.Date);
                    daysToExpiration = daysToExpiration + ts1.Days;
                }
                // modification for Windale input DaysToExpiration should be in number of years 
                //daysToExpiration = daysToExpiration > 0 ? daysToExpiration : 1;
                daysToExpiration = daysToExpiration < 0 ? 0 : daysToExpiration;
                double timeForExpirationinYrs = daysToExpiration * 1.0 / 365.0;
                if (_pricingAnalysisModel != null && daysToExpiration > 0)
                {
                    if (pricingModelData.PutOrCall == 'C')
                    {
                        optionPrice = _pricingAnalysisModel.GetCallPrice(Math.Abs(pricingModelData.StockPrice), pricingModelData.StrikePrice, interestRate, timeForExpirationinYrs, pricingModelData.Volatility, pricingModelData.DividendYield);
                    }
                    else
                    {
                        optionPrice = _pricingAnalysisModel.GetPutPrice(Math.Abs(pricingModelData.StockPrice), pricingModelData.StrikePrice, interestRate, timeForExpirationinYrs, pricingModelData.Volatility, pricingModelData.DividendYield);
                    }
                }
                return optionPrice;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return optionPrice;
        }
    }
}
