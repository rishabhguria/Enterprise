using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.SecurityMaster
{
    public  class SecurityIdentifierManager
    {
        static SecurityIdentifierCache _securityIdentifierCache = null;

        #region Static Constructor
         static  SecurityIdentifierManager()
        {
            _securityIdentifierCache = SecurityIdentifierCache.GetInstance();
            if (_securityIdentifierCache == null)
            {
                RaiseOptionCacheError();
            }
        }

        private static void RaiseOptionCacheError()
        {
            if (_securityIdentifierCache == null)
            {
                throw new Exception("Error while accessing the internal option cache.");
            }
        }
       

        #endregion     


        #region Public Functions
        /// <summary>
        /// we need to write LoadSecurityIdentifierCache when SECURITY MASTER will be available
        /// </summary>
        /// <param name="symbolCodeData"></param>

        public static SecurityIdentifier InsertSecurityIdentifierInfo(string ISIN, string CUSIP, string SEDOL, string RIC, string BBCode, string CompanyName)
        {
            if (String.IsNullOrEmpty(RIC))
            {
                return null;
            }

            lock (_securityIdentifierCache.RICSymbolDict)
            {
                if (_securityIdentifierCache.RICSymbolDict.ContainsKey(RIC))
                {
                    return _securityIdentifierCache.RICSymbolDict[RIC];
                }
                else
                {
                    SecurityIdentifier securityIdentifier = new SecurityIdentifier();
                    // get prana symbol from RIC
                    string pranaSymbol = ConvertRICToPranaSymbol(RIC);

                    securityIdentifier.ISIN = ISIN;
                    securityIdentifier.CUSIP = CUSIP;
                    securityIdentifier.SEDOL = SEDOL;
                    securityIdentifier.RIC = RIC;
                    securityIdentifier.BBCode = BBCode;
                    securityIdentifier.CompanyName = CompanyName;
                    securityIdentifier.PranaSymbol = pranaSymbol;

                    // add Security Identifier info in to the Prana Symbol Dictionary
                    AddSecurityIdentifierForPranaSymbol(securityIdentifier);
                    // add Security Identifier info in to the RIC Dictionary
                    AddSecurityIdentifierForRIC(securityIdentifier);

                    return securityIdentifier;
                }
            }



            return null;

        }

        public static string GetPranaSymbolFromRIC(string RIC)
        {
            string pranaSymbol = string.Empty;
            lock (_securityIdentifierCache.RICSymbolDict)
            {
                if (_securityIdentifierCache.RICSymbolDict.ContainsKey(pranaSymbol))
                {
                    pranaSymbol = _securityIdentifierCache.RICSymbolDict[pranaSymbol].RIC;
                }
            }
            return pranaSymbol;

        }

        public static string GetRICFromPranaSymbol(string pranaSymbol)
        {
            string RIC = string.Empty;
            lock (_securityIdentifierCache.PranaSymbolDict)
            {
                if (_securityIdentifierCache.PranaSymbolDict.ContainsKey(pranaSymbol))
                {
                    RIC = _securityIdentifierCache.PranaSymbolDict[pranaSymbol].RIC;
                }
            }
            if (!String.IsNullOrEmpty(RIC))
            {
                return RIC;
            }
            else
            {
                RIC = ConvertRICCodeFromPranaSymbol(pranaSymbol);
            }

            return RIC;
        }

        #endregion Public Functions


        #region Private functions

        private static void AddSecurityIdentifierForRIC(SecurityIdentifier securityIdentifier)
        {
            lock (_securityIdentifierCache.RICSymbolDict)
            {
                if (!String.IsNullOrEmpty(securityIdentifier.RIC))
                {
                    if (!_securityIdentifierCache.RICSymbolDict.ContainsKey(securityIdentifier.RIC))
                    {
                        _securityIdentifierCache.RICSymbolDict.Add(securityIdentifier.RIC, securityIdentifier);
                    }
                }
            }
        }
     

        private static void AddSecurityIdentifierForPranaSymbol(SecurityIdentifier securityIdentifier)
        {
            lock (_securityIdentifierCache.PranaSymbolDict)
            {
                if (!String.IsNullOrEmpty(securityIdentifier.PranaSymbol))
                {
                    if (!_securityIdentifierCache.PranaSymbolDict.ContainsKey(securityIdentifier.PranaSymbol))
                    {
                        _securityIdentifierCache.PranaSymbolDict.Add(securityIdentifier.PranaSymbol, securityIdentifier);
                    }
                }
            }
        }

        private static string ConvertRICToPranaSymbol(string RIC)
        {
           
                string[] splitSymbol = RIC.Split('.');
                if (splitSymbol.Length > 1)
                {
                    string strConvertedSymbolfromRICToPrana = string.Empty;
                    string exchangeConvention = string.Empty;
                    if (splitSymbol.Length > 1)
                    {
                        if (splitSymbol[1] != string.Empty)
                        {
                            lock (_securityIdentifierCache.ExchangeRICSymbolCodeToPranaDict)
                            {
                                if (_securityIdentifierCache.ExchangeRICSymbolCodeToPranaDict.ContainsKey(splitSymbol[1]))
                                {
                                    exchangeConvention = _securityIdentifierCache.ExchangeRICSymbolCodeToPranaDict[splitSymbol[1]];
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(exchangeConvention))
                    {
                        //if (exchangeConvention == "JAQ")
                        //{
                        //    strConvertedSymbolfromRICToPrana = splitSymbol[0] +"0"+ "-" + exchangeConvention;
                        //}
                        //else if (_securityIdentifierCache.BlankExchanges.Contains(exchangeConvention))
                        if (_securityIdentifierCache.BlankExchanges.Contains(exchangeConvention))
                        {
                            strConvertedSymbolfromRICToPrana = splitSymbol[0];
                        }
                        else
                        {
                            strConvertedSymbolfromRICToPrana = splitSymbol[0] + exchangeConvention;
                        }
                    }
                    return strConvertedSymbolfromRICToPrana;
                }
                else
                {
                    //else not equity class
                    string symbol = RIC.Remove(RIC.Length - 2);
                    string expiryMonthYear = RIC.Substring(symbol.Length);

                    string exchangeConvention = string.Empty;
                    string rootSymbol = string.Empty;

                    lock (_securityIdentifierCache.RicToPranaFutureRootSymbolDict)
                    {
                        if (_securityIdentifierCache.RicToPranaFutureRootSymbolDict.ContainsKey(symbol))
                        {
                            rootSymbol = _securityIdentifierCache.RicToPranaFutureRootSymbolDict[symbol];
                        }
                    }

                    lock (_securityIdentifierCache.RICFutureCodeToPranaExchange)
                    {
                        if (_securityIdentifierCache.RICFutureCodeToPranaExchange.ContainsKey(symbol))
                        {
                            exchangeConvention = _securityIdentifierCache.RICFutureCodeToPranaExchange[symbol];
                        }
                    }

                    StringBuilder PranaSymbol = new StringBuilder();
                    PranaSymbol.Append(rootSymbol).Append(" ").Append(expiryMonthYear);

                    if (exchangeConvention != string.Empty)
                    {
                        PranaSymbol.Append("-").Append(exchangeConvention);
                    }

                    string strConvertedSymbolfromRICToPrana = PranaSymbol.ToString();
                    return strConvertedSymbolfromRICToPrana;
                }
        }

        /// <summary>
        /// by passing the Prana symbol, get the RIC symbol like pass "6856-TSE", get "6856.T"
        /// </summary>
        /// <param name="exchangeCode"></param>
        /// <returns></returns>
        private static string ConvertRICCodeFromPranaSymbol(string pranaSymbol)
        {
            string strConvertedSymbolfromPranaTORIC = string.Empty;
            int intIndexOfSpace = 0;
            intIndexOfSpace = pranaSymbol.IndexOf(' ');
            if (intIndexOfSpace <= 0)
            {
                int intIndex = pranaSymbol.IndexOf('-');
                if (intIndex != -1)
                {
                    string[] splitSymbol = pranaSymbol.Split('-');
                    string exchangeConvention = string.Empty;
                    if (splitSymbol.Length > 1)
                    {
                        if (splitSymbol[1] != string.Empty)
                        {
                            lock (_securityIdentifierCache.ExchangePranaToRICCodeDict)
                            {
                                if (_securityIdentifierCache.ExchangePranaToRICCodeDict.ContainsKey(splitSymbol[1]))
                                {
                                    exchangeConvention = _securityIdentifierCache.ExchangePranaToRICCodeDict[splitSymbol[1]];
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(exchangeConvention))
                    {
                        if (exchangeConvention == "Q")
                        {
                            strConvertedSymbolfromPranaTORIC = splitSymbol[0].Substring(0,splitSymbol[0].Length-1) + "." + exchangeConvention;
                        }
                        else
                        {
                            strConvertedSymbolfromPranaTORIC = splitSymbol[0] + "." + exchangeConvention;

                        }
                    }
                }
            }
            else
            {
                int intIndex = pranaSymbol.IndexOf('-');

                if (intIndex != -1)
                {
                    string[] splitSymbol = pranaSymbol.Split('-');
                    string[] splitSymbolMonthCode = splitSymbol[0].ToString().Split(' ');
                    string exchangeConvention = string.Empty;
                    if (splitSymbol.Length > 1)
                    {
                        if (splitSymbol[1] != string.Empty)
                        {
                            lock (_securityIdentifierCache.ExchangePranaToRICCodeDict)
                            {
                                if (_securityIdentifierCache.PranaToRICFutureRootSymbolDict.ContainsKey(splitSymbol[1]))
                                {
                                    exchangeConvention = _securityIdentifierCache.PranaToRICFutureRootSymbolDict[splitSymbol[1]];
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(exchangeConvention))
                    {
                        strConvertedSymbolfromPranaTORIC = exchangeConvention + splitSymbolMonthCode[1];
                    }
                }
                else
                {
                    //TODO: Treatment of option symbol and US Future symbol
                    string[] splitSymbolMonthCode = pranaSymbol.Split(' ');
                    string exchangeConvention = string.Empty;

                    int outParameter;
                    string lastChar = splitSymbolMonthCode[1].ToString().Substring(1);
                    string firstChar = splitSymbolMonthCode[1].ToString().Substring(0, 1);
                    if (int.TryParse(lastChar, out outParameter).Equals(true))
                    {
                        //US Future
                        strConvertedSymbolfromPranaTORIC = splitSymbolMonthCode[0] + splitSymbolMonthCode[1];
                    }
                    else if (firstChar.Equals("#"))
                    {
                        //US Future
                        strConvertedSymbolfromPranaTORIC = splitSymbolMonthCode[0] + splitSymbolMonthCode[1];
                    }
                    else
                    {
                        //Option: Dont know about US Option or International option.
                    }
                }
            }
            return strConvertedSymbolfromPranaTORIC;
        }

        #endregion Private functions

    }
}
