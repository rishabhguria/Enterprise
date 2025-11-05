
using Prana.APIAdapter.Sessions;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Prana.APIAdapter.Utilities
{
    internal class DataSanityChecker
    {
        /// <summary>
        /// Check Data Sanity And Dump File
        /// </summary>
        /// <param name="newPrices"></param>
        internal static bool CheckDataSanityAndDumpFile(List<SymbolData> newPrices, string priceString)
        {
            var IsDataFine = true;
            var basePrices = SessionManager.Instance.SymbolsBasePrices;
            var tolerance = SessionManager.Instance.PercentageTolerance;
            try
            {
                if (basePrices != null && basePrices.Count > 0)
                {
                    foreach (SymbolData sym in newPrices)
                    {
                        var symbolKey = sym.Symbol + "_" + sym.CurencyCode.ToUpper();
                        if (basePrices.ContainsKey(symbolKey) && tolerance != 0)
                        {
                            var basePrice = basePrices[symbolKey];

                            if (basePrice != 0 || sym.LastPrice != 0)
                            {
                                var upperValue = basePrice * tolerance;
                                var lowerValue = basePrice * (1 / tolerance);

                                if (sym.LastPrice >= upperValue || sym.LastPrice <= lowerValue)
                                {


                                    var message = DateTime.UtcNow.ToLongTimeString() + "- Data Exception occurred. symbol: " + sym.Symbol + " Currency: " + sym.CurencyCode + " Base: " + basePrice + " Current: " + sym.LastPrice;
                                    Logger.LoggerWrite(message, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                    InformationReporter.GetInstance.Write(message);


                                    IsDataFine = false;
                                    break;
                                }
                            }
                        }
                    }
                }


                if (!IsDataFine || basePrices == null)
                {
                    SessionManager.Instance.SymbolsBasePrices = newPrices.GroupBy(x => x.Symbol + "_" + x.CurencyCode.ToUpper()).ToDictionary(x => x.Key, y => y.FirstOrDefault().LastPrice);
                    var fileName = SaveFile(priceString);

                    if (!IsDataFine)
                    {
                        Logger.LoggerWrite(DateTime.UtcNow.ToLongTimeString() + "- Data Exception occurred. Prices dump file name:  " + fileName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        InformationReporter.GetInstance.Write(DateTime.UtcNow.ToLongTimeString() + "- Data Exception occured. Prices dump file name:  " + fileName);


                    }
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
            return IsDataFine;

        }

        /// <summary>
        /// Save File
        /// </summary>
        /// <param name="priceString"></param>
        private static string SaveFile(string priceString)
        {

            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff",
                                           CultureInfo.InvariantCulture) + ".txt";
                System.Threading.Tasks.Task.Run(() =>
                    {

                        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Pricing Files";
                        if (!Directory.Exists(assemblyFolder))
                            Directory.CreateDirectory(assemblyFolder);

                        System.IO.File.WriteAllText(assemblyFolder + "\\" + timestamp, priceString);
                    });
                return timestamp;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
