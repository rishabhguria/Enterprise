using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;

namespace Prana.ClientCommon
{
    public static class SymbologyHelper
    {
        private static ApplicationConstants.SymbologyCodes _defaultSymbology;
        public static ApplicationConstants.SymbologyCodes DefaultSymbology
        {
            get
            {
                return _defaultSymbology;
            }
            private set
            {
                _defaultSymbology = value;
            }
        }

        static SymbologyHelper()
        {
            try
            {
                _defaultSymbology = (ApplicationConstants.SymbologyCodes)TradingTktPrefs.TTGeneralPrefs.DefaultSymbology;

                switch (_defaultSymbology)
                {
                    case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                        if (CachedDataManager.CompanyMarketDataProvider != MarketDataProvider.FactSet)
                            UpdateDefaultSymbology(ApplicationConstants.SymbologyCodes.TickerSymbol);
                        break;
                    case ApplicationConstants.SymbologyCodes.ActivSymbol:
                        if (CachedDataManager.CompanyMarketDataProvider != MarketDataProvider.ACTIV)
                            UpdateDefaultSymbology(ApplicationConstants.SymbologyCodes.TickerSymbol);
                        break;
                }
            }
            catch (Exception ex)
            {
                _defaultSymbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }

        public static List<EnumerationValue> GetAvailableSymbologies()
        {
            List<EnumerationValue> availableSymbologies = new List<EnumerationValue>();

            try
            {
                List<EnumerationValue> symbologies = EnumHelper.ConvertEnumForBindingWithDescriptionValues(typeof(ApplicationConstants.SymbologyCodes));
                availableSymbologies = symbologies.FindAll(s =>
                    (int)s.Value == (int)ApplicationConstants.SymbologyCodes.TickerSymbol ||
                    (int)s.Value == (int)ApplicationConstants.SymbologyCodes.BloombergSymbol
                );

                switch (CachedDataManager.CompanyMarketDataProvider)
                {
                    case MarketDataProvider.FactSet:
                        availableSymbologies.Add(symbologies.Find(s => (int)s.Value == (int)ApplicationConstants.SymbologyCodes.FactSetSymbol));
                        break;
                    case MarketDataProvider.ACTIV:
                        availableSymbologies.Add(symbologies.Find(s => (int)s.Value == (int)ApplicationConstants.SymbologyCodes.ActivSymbol));
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }

            return availableSymbologies;
        }

        public static bool UpdateDefaultSymbology(ApplicationConstants.SymbologyCodes symbology)
        {
            try
            {
                TradingTktPrefs.TTGeneralPrefs.DefaultSymbology = (int)symbology;
                TradingTktPrefs.SaveGeneralPrefs();

                _defaultSymbology = symbology;
                return true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }

            return false;
        }
    }
}
