using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.OptionCalculator.Common;
using System;
using System.Collections.Generic;

namespace Prana.OptionCalculator.CalculationComponent
{
    public class VolSkewManager
    {
        static int count = 0;
        ////private static Dictionary<string, Dictionary<string, VolSkewObject>> _dictVolSkewRequest = new Dictionary<string, Dictionary<string, VolSkewObject>>();
        ////private static Dictionary<string, Dictionary<string, List<VolSkewObject>>> _dictProxySymbolWise = new Dictionary<string, Dictionary<string, List<VolSkewObject>>>();
        private static object _lock = new object();
        private static void FillUnderlyingData(SymbolData UnderlyingData, VolSkewObject obj)
        {
            obj.UnderlyingPrice = UnderlyingData.SelectedFeedPrice; ;
            //obj.DividendYield = UnderlyingData.FinalDividendYield;
        }
        public static void SetToleranceAdjustedProxyValues(SymbolData UnderlyingData, VolSkewObject obj, double toleranceValue, List<OptionSimulationInputs> ListOptSimChanges)
        {
            double value = 0;
            string param = string.Empty;
            FillUnderlyingData(UnderlyingData, obj);
            double ProxyStrikeValue = obj.StrikePrice;
            foreach (OptionSimulationInputs inputs in ListOptSimChanges)
            {
                value = (inputs.ChangeUnderlyingPrice - 1) * 100;
                param = value.ToString();
                double simulatedUnderlyingPrice = obj.UnderlyingPrice;
                if (inputs.ChangeUnderlyingPrice != 1)
                {
                    //double value = (inputs.ChangeUnderlyingPrice - 1) * 100;
                    //param = value.ToString();
                    simulatedUnderlyingPrice = obj.UnderlyingPrice * inputs.ChangeUnderlyingPrice;
                    double PercentageIN_OUTOfMoney = (simulatedUnderlyingPrice - obj.StrikePrice) / obj.StrikePrice;
                    ProxyStrikeValue = (obj.UnderlyingPrice / (1 + PercentageIN_OUTOfMoney));
                    obj.PercentageIN_OUTofMoney = PercentageIN_OUTOfMoney * 100;
                    obj.DictProxyStrikes.Add(param, ProxyStrikeValue);
                }
                else
                {
                    //adding zero value step
                    if (obj.DictProxyStrikes.Count == 0)
                    {
                        obj.DictProxyStrikes.Add(param, ProxyStrikeValue);
                    }
                }
                obj.ProxyStrikePrice = ProxyStrikeValue;
                DateTime proxyExpirationDate = obj.ExpirationDate;
                param = inputs.ChangeDaysToExpiration.ToString();
                if (inputs.ChangeDaysToExpiration != 0)
                {
                    proxyExpirationDate = proxyExpirationDate.AddDays(inputs.ChangeDaysToExpiration * (-1));

                    obj.DictProxyExpirationDates.Add(param, proxyExpirationDate);
                }
                else
                {
                    //adding zero value step
                    if (obj.DictProxyExpirationDates.Count == 0)
                    {
                        obj.DictProxyExpirationDates.Add(param, proxyExpirationDate);
                    }
                }
                obj.ProxyExpirationDate = proxyExpirationDate;
            }
            obj.SetProxyStrikeRangeValues();
            if (obj.ProxyStrikeMin > obj.StrikePrice)
            {
                obj.ProxyStrikeMin = obj.StrikePrice;
            }
            else if (obj.ProxyStrikeMax < obj.StrikePrice)
            {
                obj.ProxyStrikeMax = obj.StrikePrice;
            }

            obj.ProxyStrikeMin = obj.ProxyStrikeMin * (1 - toleranceValue);
            obj.ProxyStrikeMax = obj.ProxyStrikeMax * (1 + toleranceValue);
            count++;

        }

        public static List<string> GetProxyOptions(List<OptionStaticData> listOptData, VolSkewObject Reqobj, SubscriberViewData subscriberView, string requestID)
        {
            List<string> listProxyOptions = new List<string>();
            //DateTime proxyExpirationDate = DateTime.MinValue;
            int proxyExpirationMonth = 0;
            bool isResponseExist = true;
            string proxyOptSymbol = string.Empty;
            try
            {
                //bool isPresent = false;
                if (listOptData.Count > 0)
                {

                    if (Reqobj.PutOrCall == (int)OptionType.CALL)
                    {
                        listOptData = listOptData.FindAll(delegate (OptionStaticData obj)
                        {
                            if (obj.PutOrCall == (OptionType.CALL))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                    else
                    {
                        if (Reqobj.PutOrCall == ((int)OptionType.PUT))
                        {
                            listOptData = listOptData.FindAll(delegate (OptionStaticData obj)
                        {
                            if (obj.PutOrCall == (OptionType.PUT))
                            {
                                return true;
                            }

                            else
                            {
                                return false;
                            }
                        });
                        }
                    }
                }
                else
                {
                    isResponseExist = false;
                }

                List<string> listStepKey = new List<string>();
                lock (_lock)
                {
                    foreach (KeyValuePair<string, double> kp in Reqobj.DictProxyStrikes)
                    {
                        OptionStaticData proxyData = null;
                        if (isResponseExist)
                        {
                            proxyData = GetProxySymbolData(listOptData, Reqobj, kp.Value);
                            proxyOptSymbol = proxyData.Symbol;
                            proxyExpirationMonth = proxyData.ExpirationDate.Month;
                        }
                        else
                        {

                            //copying the ParentSymbol if option Response list count is zero.
                            proxyOptSymbol = Reqobj.Symbol;
                            proxyExpirationMonth = subscriberView.GetProxyExpirationMonthForGivenRequestID(requestID);
                        }
                        Reqobj.ProxySymbol = proxyOptSymbol;
                        if (!listProxyOptions.Contains(proxyOptSymbol))
                        {
                            listProxyOptions.Add(proxyOptSymbol);
                        }
                        //AddtoProxySymbolDict(paramUserID, paramViewID, proxyOptSymbol, Reqobj);
                        subscriberView.UpdateProxySymbolDict(proxyOptSymbol, Reqobj);
                        listStepKey.Add(kp.Key);
                        Reqobj.UpdateProxySymbolDictionary(kp.Key, proxyOptSymbol, proxyExpirationMonth);
                    }
                    Reqobj.RemoveFromProxyStrikeDictionary(listStepKey);
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
            return listProxyOptions;
        }


        private static OptionStaticData GetProxySymbolData(List<OptionStaticData> listOptData, VolSkewObject Reqobj, double proxyStrikePrice)
        {
            string proxySymbol = string.Empty;
            OptionStaticData proxyOptData = new OptionStaticData();
            //double proxyStrikePrice = kp.Value;
            int greaterStrikeIndex = 0;
            int lesserStrikeIndex = 0;
            //if (listOptData.Count > 0)
            //{

            listOptData.Sort((delegate (OptionStaticData p1, OptionStaticData p2) { return (p1.StrikePrice.CompareTo(p2.StrikePrice)); }));

            //listOptData.RemoveAll(delegate(OptionStaticData obj)
            //    {
            //        if (obj.ExpirationDate > Reqobj.ExpirationDate)
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    });

            greaterStrikeIndex = listOptData.FindIndex(delegate (OptionStaticData obj)
            {
                if (obj.StrikePrice >= proxyStrikePrice && obj.ExpirationDate <= Reqobj.ExpirationDate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            if (greaterStrikeIndex > 0)
            {
                lesserStrikeIndex = (greaterStrikeIndex - 1);
                OptionStaticData obj_lesserStrike = listOptData[lesserStrikeIndex];
                OptionStaticData obj_greaterStrike = listOptData[greaterStrikeIndex];
                double strikediff_lessStrike = (proxyStrikePrice - obj_lesserStrike.StrikePrice);
                double strikediff_GreaterStrike = (obj_greaterStrike.StrikePrice - proxyStrikePrice);
                if (strikediff_lessStrike <= strikediff_GreaterStrike)
                {
                    proxyOptData = obj_lesserStrike;
                    //proxyExpirationDate = obj_lesserStrike.ExpirationDate;
                }
                else
                {
                    proxyOptData = obj_greaterStrike;
                    //proxyExpirationDate = obj_greaterStrike.ExpirationDate;
                }
            }
            else
            {
                if (greaterStrikeIndex == 0)
                {
                    proxyOptData = listOptData[greaterStrikeIndex];
                    // proxyExpirationDate = listOptData[greaterStrikeIndex].ExpirationDate;
                }
                else
                {
                    proxyOptData = listOptData[listOptData.Count - 1];
                    //proxyExpirationDate = listOptData[listOptData.Count - 1].ExpirationDate;
                }
            }
            proxySymbol = proxyOptData.Symbol;
            return proxyOptData;
            //}
            //else
            //{
            //    proxyOptData = Reqobj.Symbol;
            //}
        }
    }

    public class VolSkewStrikeRange
    {
        private double _strikeMin;

        public double StrikeMin
        {
            get { return _strikeMin; }
            set { _strikeMin = value; }
        }

        private double _strikeMax;
        public double StrikeMax
        {
            get { return _strikeMax; }
            set { _strikeMax = value; }
        }

        List<VolSkewObject> _listVolSkew = new List<VolSkewObject>();

        public List<VolSkewObject> ListVolSkew
        {
            get { return _listVolSkew; }
            set { _listVolSkew = value; }
        }


    }
}
