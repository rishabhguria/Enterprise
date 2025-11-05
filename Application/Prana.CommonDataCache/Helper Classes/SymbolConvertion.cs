using System;
using System.Collections.Generic;

namespace Prana.CommonDataCache
{
    public class SymbolConvertion
    {
        //                                                            Other Exchange Code,Prana Exchange Code
        Dictionary<string, string> _exchangeRICCodeToPrana = new Dictionary<string, string>();
        //                                                             Prana Exchange Code, Other Exchange Code
        Dictionary<string, string> _exchangePranaToRICCode = new Dictionary<string, string>();
        private static SymbolConvertion _symbolConvertion = new SymbolConvertion();

        private SymbolConvertion()
        {
            MapExchangeSymbolCodesFromRICToPrana();
            MapExchangeSymbolCodesPranaToRIC();
        }

        public static SymbolConvertion GetInstance
        {
            get
            {
                if (_symbolConvertion == null)
                {
                    _symbolConvertion = new SymbolConvertion();

                }
                return _symbolConvertion;
            }
        }


        /// <summary>
        /// by passing the symbol, get the Modified symbol like pass "6856.T", get "6856-TSE"
        /// </summary>
        /// <param name="exchangeCode"></param>
        /// <returns></returns>
        public string GetPranaSymbolFromRICCode(string RICCode)
        {
            string[] splitSymbol = RICCode.Split('.');
            string strConvertedSymbolfromEMSToPrana = string.Empty;
            string exchangeConvention = string.Empty;
            if (splitSymbol.Length > 1)
            {
                if (splitSymbol[1] != string.Empty)
                {
                    if (_exchangeRICCodeToPrana.ContainsKey(splitSymbol[1]))
                    {
                        exchangeConvention = _exchangeRICCodeToPrana[splitSymbol[1]];
                    }
                }

            }
            if (!String.IsNullOrEmpty(exchangeConvention))
            {
                strConvertedSymbolfromEMSToPrana = splitSymbol[0] + "-" + exchangeConvention;
            }
            return strConvertedSymbolfromEMSToPrana;
        }

        /// <summary>
        /// by passing the symbol, get the Modified symbol like pass "6856-TSE", get "6856.T"
        /// </summary>
        /// <param name="exchangeCode"></param>
        /// <returns></returns>
        public string GetRICCodeFromPranaSymbol(string pranaSymbol)
        {
            string[] splitSymbol = pranaSymbol.Split('-');
            string strConvertedSymbolfromPranaTOEMS = string.Empty;
            string exchangeConvention = string.Empty;
            if (splitSymbol.Length > 1)
            {
                if (splitSymbol[1] != string.Empty)
                {
                    if (_exchangePranaToRICCode.ContainsKey(splitSymbol[1]))
                    {
                        exchangeConvention = _exchangePranaToRICCode[splitSymbol[1]];
                    }
                }

            }
            if (!String.IsNullOrEmpty(exchangeConvention))
            {
                strConvertedSymbolfromPranaTOEMS = splitSymbol[0] + "." + exchangeConvention;
            }
            return strConvertedSymbolfromPranaTOEMS;
        }




        /// <summary>
        ///the given collection contains the symbol mapping from Other EMS to Prana symbol convention
        /// </summary>
        private void MapExchangeSymbolCodesFromRICToPrana()
        {
            _exchangeRICCodeToPrana.Add("T", "TSE"); //Tokyo Stock Exchange           
        }


        /// <summary>
        ///the given collection contains the symbol mapping from Prana symbol convention to Other EMS 
        /// </summary>
        private void MapExchangeSymbolCodesPranaToRIC()
        {
            _exchangePranaToRICCode.Add("TSE", "T"); //Tokyo Stock Exchange           
        }

    }
}
