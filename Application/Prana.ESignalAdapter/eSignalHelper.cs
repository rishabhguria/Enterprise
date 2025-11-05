using System;
using System.Collections.Generic;

namespace Prana.ESignalAdapter
{
    /// <summary>
    /// Helper class for eSignalManager
    /// </summary>
    public class eSignalHelper
    {
        /// <summary>
        /// The dtmonth
        /// </summary>
        static Dictionary<int, string> dtmonth = new Dictionary<int, string>();

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        static void initialise()
        {
            dtmonth.Add(1, "January");
            dtmonth.Add(2, "February");
            dtmonth.Add(3, "March");
            dtmonth.Add(4, "April");
            dtmonth.Add(5, "May");
            dtmonth.Add(6, "June");
            dtmonth.Add(7, "July");
            dtmonth.Add(8, "August");
            dtmonth.Add(9, "September");
            dtmonth.Add(10, "October");
            dtmonth.Add(11, "November");
            dtmonth.Add(12, "December");
        }

        /// <summary>
        /// Initializes the <see cref="eSignalHelper"/> class.
        /// </summary>
        static eSignalHelper()
        {
            initialise();
        }

        /// <summary>
        /// Determines whether [is option ticker symbol] [the specified symbol].
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>
        ///   <c>true</c> if [is option ticker symbol] [the specified symbol]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOptionTickerSymbol(string symbol)
        {
            string[] tickerStrArr = symbol.Split(new char[] { ':' });
            if (tickerStrArr.Length == 2 && tickerStrArr[0].Trim().ToUpper().Equals("O"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Commented Code
        //public static bool IsFutureOptionSymbol(string symbol)
        //{
        //    if (!String.IsNullOrEmpty(symbol))
        //    {
        //        string[] symbolArr = symbol.Split(' ');
        //        if (symbolArr.Length > 1)
        //        {
        //            // eg: ES G1C375 & ES G1P375 is future option
        //            //  ES G1 is future 
        //            if (symbolArr[1].Length > 2 && ((symbolArr[1].Contains("P") || symbolArr[1].Contains("C"))))
        //            {
        //                return true;

        //            }
        //        }
        //    }
        //    return false;
        //}
        #endregion

        /// <summary>
        /// Determines whether [is future or future option symbol] [the specified symbol].
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>
        ///   <c>true</c> if [is future or future option symbol] [the specified symbol]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFutureOrFutureOptionSymbol(string symbol)
        {
            if (!String.IsNullOrEmpty(symbol))
            {
                string[] symbolArr = symbol.Split(' ');
                if (symbolArr.Length > 1)
                {
                    // eg: ES G1C375 & ES G1P375 is future option
                    //  ES G1 is future 
                    if (symbolArr[1].Length >= 2)
                    {
                        if ((symbolArr[1].Contains("P") || symbolArr[1].Contains("C")) && !symbolArr[1].ToLower().Contains("o:"))
                            return true;
                        char yearDigit = Convert.ToChar(symbolArr[1].Substring(1, 1));
                        if (Char.IsDigit(yearDigit))
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is international future symbol] [the specified symbol].
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>
        ///   <c>true</c> if [is international future symbol] [the specified symbol]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInternationalFutureSymbol(string symbol)
        {
            if (!String.IsNullOrEmpty(symbol))
            {
                string[] symbolArr = symbol.Split(' ');
                if (symbolArr.Length > 1)
                {
                    if (symbolArr[1].Length >= 2)
                    {
                        if (symbolArr[1].Contains("-"))
                        {
                            return true;
                        }
                        //char yearDigit = Convert.ToChar(symbolArr[1].Substring(1, 1));
                        //if (Char.IsDigit(yearDigit))
                        //{
                        //    return true;
                        //}
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the name of the month.
        /// </summary>
        /// <param name="monthno">The monthno.</param>
        /// <returns></returns>
        public static string GetMonthName(int monthno)
        {
            return dtmonth[monthno];
        }

        /// <summary>
        /// Gets the strike price.
        /// </summary>
        /// <param name="OptionSymbol">The option symbol.</param>
        /// <returns></returns>
        public static double GetStrikePrice(string OptionSymbol)
        {
            double strikePrice = 0;
            if (!String.IsNullOrEmpty(OptionSymbol))
            {
                if (IsOptionTickerSymbol(OptionSymbol))
                {
                    //In case of international equity options, symbol will be splitted with - too, for extracting strike price
                    string[] symbolArr = OptionSymbol.Split(' ', '-');

                    if (symbolArr.Length > 1)
                    {
                        string strikePart = symbolArr[1];
                        //here 3 contains the 2 digit yr part and 1 char Month code..
                        double result = 0;
                        if (double.TryParse(strikePart.Substring(3), out result))
                        {
                            strikePrice = double.Parse(strikePart.Substring(3));
                        }
                    }
                }
                else if (IsFutureOrFutureOptionSymbol(OptionSymbol))
                {
                    string[] symbolArr = OptionSymbol.Split(' ');
                    if (symbolArr.Length > 1)
                    {
                        string strikePart = symbolArr[1];
                        if (symbolArr[1].Length >= 2)
                        {
                            int CALL_PUTindex = 0;
                            if (symbolArr[1].Contains("P"))
                            {
                                CALL_PUTindex = symbolArr[1].IndexOf("P");
                            }
                            else if (symbolArr[1].Contains("C"))
                            {
                                CALL_PUTindex = symbolArr[1].IndexOf("C");
                            }
                            double result = 0;
                            //Narendra Kumar jangir 2012 Dec 27
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-2223
                            //handling for international symbols which contains -xx format at the end (eg: AX Z2P730000-DT)
                            string[] symbolArrFutOpt = symbolArr[1].Split('-');
                            if (symbolArrFutOpt.Length > 1)
                            {
                                if (double.TryParse(symbolArrFutOpt[0].Substring((CALL_PUTindex + 1)), out result))
                                {
                                    strikePrice = double.Parse(symbolArrFutOpt[0].Substring(CALL_PUTindex + 1));
                                }
                            }
                            else
                            {
                                if (double.TryParse(symbolArr[1].Substring((CALL_PUTindex + 1)), out result))
                                {
                                    strikePrice = double.Parse(symbolArr[1].Substring(CALL_PUTindex + 1));
                                }
                            }
                        }
                    }
                }
            }
            return strikePrice;
        }
    }
}
