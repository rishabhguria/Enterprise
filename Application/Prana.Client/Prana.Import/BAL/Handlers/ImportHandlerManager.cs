using Prana.BusinessObjects.AppConstants;
using Prana.Interfaces;
using Prana.LogManager;
using System;

namespace Prana.Import
{
    class ImportHandlerManager
    {
        #region singleton
        private static volatile ImportHandlerManager instance;
        private static object syncRoot = new Object();

        private ImportHandlerManager() { }

        public static ImportHandlerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ImportHandlerManager();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        public IImportHandler GetHandler(ImportType importType)
        {

            switch (importType)
            {
                case ImportType.StagedOrder:
                    return StagedOrderHandler.Instance;
                //return new StagedOrderHandler();
                case ImportType.Transaction:
                case ImportType.NetPosition:
                    //return PositionAndTransactionHandler.Instance;
                    return new PositionAndTransactionHandler();
                case ImportType.AllocationScheme:
                    return AllocationSchemeHandler.Instance;
                //return new AllocationSchemeHandler();
                case ImportType.Activities:
                    //return ActivitiesHandler.Instance;
                    return new ActivitiesHandler();
                case ImportType.DailyBeta:
                    return new DailyBetaHandler();
                // return new GenericImportHandlerBeta();
                case ImportType.ForexPrice:
                    //return ForedxPriceHandler.Instance;
                    return new ForedxPriceHandler();
                case ImportType.GenericImport:
                    //return GenericImportHandler.Instance;
                    return new GenericImportHandler();
                case ImportType.MarkPrice:
                    //return MarkPriceHandler.Instance;
                    return new MarkPriceHandler();
                case ImportType.SecMasterInsert:
                    return new SecMasterHandler();
                case ImportType.SecMasterUpdate:
                    //return SecMasterHandler.Instance;
                    return new SecMasterUpdateHandler();
                case ImportType.AllocationScheme_AppPositions:
                    return new AllocationSchemeAppPositionHandler();
                case ImportType.Cash:
                    return new CashImportHandler();
                case ImportType.OMI:
                    return new OptionModelInputHandler();
                case ImportType.CreditLimit:
                    return new CreditLimitHandler();
                case ImportType.SMBatch:
                    return new SMPricingDataHandler();
                case ImportType.DailyVolatility:
                    return new DailyVolatilityHandler();
                case ImportType.DailyVWAP:
                    return new DailyVWAPHandler();
                case ImportType.DailyCollateralPrice:
                    return new CollateralPriceHandler();
                case ImportType.DialyDividendYield:
                    return new DailyDividendYieldHandler();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Function to get import source according to import type
        /// </summary>
        /// <param name="importType"></param>
        /// <returns></returns>
        public static ImportDataSource GetImportDataSource(ImportType importType)
        {
            try
            {
                switch (importType)
                {
                    case ImportType.AllocationScheme_AppPositions:
                        return ImportDataSource.Function;
                    case ImportType.StagedOrder:
                    case ImportType.Transaction:
                    case ImportType.NetPosition:
                    case ImportType.AllocationScheme:
                    case ImportType.Activities:
                    case ImportType.DailyBeta:
                    case ImportType.ForexPrice:
                    case ImportType.GenericImport:
                    case ImportType.MarkPrice:
                    case ImportType.SecMasterInsert:
                    case ImportType.SecMasterUpdate:
                    case ImportType.Cash:
                    case ImportType.OMI:
                    case ImportType.CreditLimit:
                    case ImportType.DailyVolatility:
                    case ImportType.DialyDividendYield:
                    case ImportType.DailyVWAP:
                    case ImportType.DailyCollateralPrice:
                    default:
                        return ImportDataSource.File;
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
            return ImportDataSource.File;
        }
    }
}
