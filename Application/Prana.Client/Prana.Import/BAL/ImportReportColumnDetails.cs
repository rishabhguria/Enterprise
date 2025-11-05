using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Import.BAL
{
    internal sealed class ImportReportColumnDetails
    {
        private static volatile ImportReportColumnDetails instance;
        private static object syncRoot = new Object();

        private ImportReportColumnDetails() { }

        public static ImportReportColumnDetails Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ImportReportColumnDetails();
                    }
                }

                return instance;
            }
        }

        public delegate void SetColumnPropFunction(UltraGridBand band);

        /// <summary>
        /// list of columns and their details for Position Master Import. Extra custom columns are handled separately.
        /// ColumnName,Caption,Visible
        /// </summary>
        List<SetColumnPropFunction> ColumnListForImport = new List<SetColumnPropFunction>();

        public List<SetColumnPropFunction> InitializeData(ImportType importType, bool isEditableReport = true)
        {
            try
            {
                switch (importType)
                {
                    case ImportType.StagedOrder:
                        FillStagedOrderImportList();
                        break;
                    case ImportType.Transaction:
                    case ImportType.NetPosition:
                        FillPositionMasterImportList(isEditableReport);
                        break;
                    case ImportType.AllocationScheme:
                    case ImportType.AllocationScheme_AppPositions:
                        FillAllocationSchemeImportList();
                        break;
                    case ImportType.Activities:
                        FillPositionMasterImportList(isEditableReport);
                        break;
                    case ImportType.DailyBeta:
                        FillBetaImportList();
                        break;
                    case ImportType.ForexPrice:
                        FillForexImportList();
                        break;
                    case ImportType.GenericImport:
                        FillPositionMasterImportList(isEditableReport);
                        break;
                    case ImportType.MarkPrice:
                        FillMarkPriceImportList();
                        break;
                    case ImportType.SecMasterInsert:
                    case ImportType.SecMasterUpdate:
                        FillSecMasterImportList();
                        break;
                    case ImportType.Cash:
                        FillCashImportList();
                        break;
                    case ImportType.OMI:
                        FillPositionMasterImportList(isEditableReport);
                        break;
                    case ImportType.CreditLimit:
                        FillCreditLimitImportList();
                        break;
                    case ImportType.DailyVolatility:
                        FillVolatilityImportList();
                        break;
                    case ImportType.DialyDividendYield:
                        FillDividendYieldImportList();
                        break;
                    case ImportType.DailyVWAP:
                        FillVWAPImportList();
                        break;
                    case ImportType.DailyCollateralPrice:
                        FillCollateralImportList();
                        break;
                    default:
                        FillPositionMasterImportList(isEditableReport);
                        break;
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
            return ColumnListForImport;
        }

        /// <summary>
        /// To set columns for position transaction handler.
        /// </summary>
        private void FillPositionMasterImportList(bool isEditableReport)
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetExecutingBroker));
                ColumnListForImport.Add(new SetColumnPropFunction(SetPositionStartDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAssetID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCurrencyID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSideTagValue));
                ColumnListForImport.Add(new SetColumnPropFunction(SetNetPosition));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCostBasis));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMarkPrice));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCommission));
                ColumnListForImport.Add(new SetColumnPropFunction(SetFees));
                ColumnListForImport.Add(new SetColumnPropFunction(SetStampDuty));
                ColumnListForImport.Add(new SetColumnPropFunction(SetTransactionLevy));
                ColumnListForImport.Add(new SetColumnPropFunction(SetClearingFee));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMiscFees));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOccFee));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOrfFee));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecFee));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSoftCommission));
                ColumnListForImport.Add(new SetColumnPropFunction(SetClearingBrokerFee));
                ColumnListForImport.Add(new SetColumnPropFunction(SetVenueID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetStrategy));
                ColumnListForImport.Add(new SetColumnPropFunction(SetPBSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetPBAssetType));
                ColumnListForImport.Add(new SetColumnPropFunction(SetDescription));
                ColumnListForImport.Add(new SetColumnPropFunction(SetFXConversionMethodOperator));
                ColumnListForImport.Add(new SetColumnPropFunction(SetFXRate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMultiplier));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUnderLyingID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetExchangeID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIssueDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetFirstCouponDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetExpirationDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUnderlyingSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCall_Put));
                ColumnListForImport.Add(new SetColumnPropFunction(SetStrikePrice));
                ColumnListForImport.Add(new SetColumnPropFunction(SetLeadCurrencyID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccrualBasis));
                ColumnListForImport.Add(new SetColumnPropFunction(SetFreq));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCoupon));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSymbolMapped));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchDetails));
                ColumnListForImport.Add(new SetColumnPropFunction(SetApprovedBy));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDAAssetClassID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDASectorID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDASubSectorID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDASecurityTypeID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDACountryID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSettlementCurrency));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOptionPremiumAdjustment));

                //modified by omshiv, Added ExecutionDate on Import report UI
                ColumnListForImport.Add(new SetColumnPropFunction(SetPranaExecutionDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetFxConversionMethodOperator));
                if (isEditableReport)
                {
                    ColumnListForImport.Add(new SetColumnPropFunction(SetCreateCounterParty));
                    ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchType));
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
        }

        /// <summary>
        /// To set columns for Beta handler.
        /// </summary>
        private void FillBetaImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBeta));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for MarkPrice handler.
        /// </summary>
        private void FillMarkPriceImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMarkPrice));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Cash import handler.
        /// </summary>
        private void FillCashImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidated));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBaseCurrency));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBaseCurrency));
                ColumnListForImport.Add(new SetColumnPropFunction(SetLocalCurrency));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCashValueBase));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCashValueLocal));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Forex handler.
        /// </summary>
        private void FillForexImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidated));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBaseCurrency));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSettlementCurrency));
                ColumnListForImport.Add(new SetColumnPropFunction(SetForexPrice));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
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

        /// <summary>
        /// To set columns for Credit limit handler.
        /// </summary>
        private void FillCreditLimitImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidated));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetLongDebitBalance));
                ColumnListForImport.Add(new SetColumnPropFunction(SetShortCreditBalance));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Allocation scheme handler and Allocation scheme app position handler.
        /// </summary>
        private void FillAllocationSchemeImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidated));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAssetID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCurrencyID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetPBSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchType));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchDetails));
                ColumnListForImport.Add(new SetColumnPropFunction(SetApprovedBy));
                ColumnListForImport.Add(new SetColumnPropFunction(SetQuantity));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOrderSideTagValue));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAllocationSchemeKey));
                ColumnListForImport.Add(new SetColumnPropFunction(SetPercentage));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRoundLot));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Sec Master Insert handler and Sec Master Update handler.
        /// </summary>
        private void FillSecMasterImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAssetID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCurrencyID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetPBSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUnderLyingID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetExchangeID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetExpirationDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUnderlyingSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCall_Put));
                ColumnListForImport.Add(new SetColumnPropFunction(SetLeadCurrencyID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchType));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchDetails));
                ColumnListForImport.Add(new SetColumnPropFunction(SetApprovedBy));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIssueDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetFirstCouponDate));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDAAssetClassID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDASectorID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDASubSectorID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDASecurityTypeID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetUDACountryID));
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

        /// <summary>
        /// To set columns for Staged order handler.
        /// </summary>
        private void FillStagedOrderImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetStrategy));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCurrencyID));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIsSecApproved));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchType));
                ColumnListForImport.Add(new SetColumnPropFunction(SetMismatchDetails));
                ColumnListForImport.Add(new SetColumnPropFunction(SetApprovedBy));
                ColumnListForImport.Add(new SetColumnPropFunction(SetQuantity));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAlgoStrategyName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCounterPartyName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCall_Put));
                ColumnListForImport.Add(new SetColumnPropFunction(SetStrikePrice));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Volatility handler.
        /// </summary>
        private void FillVolatilityImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetVolatility));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Volatility handler.
        /// </summary>
        private void FillVWAPImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetVWAP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Volatility handler.
        /// </summary>
        private void FillCollateralImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCollateralPrice));
                ColumnListForImport.Add(new SetColumnPropFunction(SetAccountName));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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

        /// <summary>
        /// To set columns for Dividend Yield handler.
        /// </summary>
        private void FillDividendYieldImportList()
        {
            try
            {
                ColumnListForImport.Clear();
                ColumnListForImport.Add(new SetColumnPropFunction(SetSecApprovalStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetBloomberg));
                ColumnListForImport.Add(new SetColumnPropFunction(SetCUSIP));
                ColumnListForImport.Add(new SetColumnPropFunction(SetImportStatus));
                ColumnListForImport.Add(new SetColumnPropFunction(SetRIC));
                ColumnListForImport.Add(new SetColumnPropFunction(SetISIN));
                ColumnListForImport.Add(new SetColumnPropFunction(SetSEDOL));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOSIOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetIDCOOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetOPRAOptionSymbol));
                ColumnListForImport.Add(new SetColumnPropFunction(SetDividendYield));
                ColumnListForImport.Add(new SetColumnPropFunction(SetValidationError));
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


        #region SetColumnPropertiesFunctions
        private void SetSecApprovalStatus(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(ApplicationConstants.CONST_SEC_APPROVED_STATUS))
            {
                //we are showing Approval Status based on isSecApproved property.
                UltraGridColumn colApprovalStatus = gridBand.Columns[ApplicationConstants.CONST_SEC_APPROVED_STATUS];
                colApprovalStatus.Header.Caption = "Security Approval Status";
                //colApprovalStatus.Header.VisiblePosition = visiblePosition++;
                //colApprovalStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //  colApprovalStatus.ValueList = _approvalSatus;
                colApprovalStatus.Hidden = false;
                colApprovalStatus.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetValidationStatus(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ValidationStatus"))
            {
                //we are showing Approval Status based on isSecApproved property.
                UltraGridColumn colApprovalStatus = gridBand.Columns["ValidationStatus"];
                colApprovalStatus.Header.Caption = "Trade Validation Status";
                //colApprovalStatus.Header.VisiblePosition = visiblePosition++;
                //colApprovalStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //  colApprovalStatus.ValueList = _approvalSatus;
                colApprovalStatus.Hidden = false;
                colApprovalStatus.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetSymbol(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Symbol"))
            {
                UltraGridColumn ColTickerSymbol = gridBand.Columns["Symbol"];
                //ColTickerSymbol.Header.VisiblePosition = visiblePosition++;
                ColTickerSymbol.Header.Caption = OrderFields.CAPTION_TICKERSYMBOL;
                ColTickerSymbol.CharacterCasing = CharacterCasing.Upper;
                ColTickerSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColTickerSymbol.NullText = String.Empty;
                ColTickerSymbol.SortIndicator = SortIndicator.Ascending;
                ColTickerSymbol.Hidden = false;
                ColTickerSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetBloomberg(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Bloomberg"))
            {
                UltraGridColumn ColBloombergSymbol = gridBand.Columns["Bloomberg"];
                //ColBloombergSymbol.Header.VisiblePosition = visiblePosition++;
                ColBloombergSymbol.CharacterCasing = CharacterCasing.Upper;
                ColBloombergSymbol.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL;
                ColBloombergSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColBloombergSymbol.NullText = String.Empty;
                ColBloombergSymbol.Hidden = false;
                ColBloombergSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCUSIP(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("CUSIP"))
            {
                UltraGridColumn ColCusipSymbol = gridBand.Columns["CUSIP"];
                //ColCusipSymbol.Header.VisiblePosition = visiblePosition++;
                ColCusipSymbol.CharacterCasing = CharacterCasing.Upper;
                ColCusipSymbol.Header.Caption = OrderFields.CAPTION_CUSIPSYMBOL;
                ColCusipSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColCusipSymbol.NullText = String.Empty;
                ColCusipSymbol.Hidden = false;
                ColCusipSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetAccountName(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("FundName"))
            {
                UltraGridColumn colAccount = gridBand.Columns["FundName"];
                //colAccount.Header.VisiblePosition = visiblePosition++;
                colAccount.Header.Caption = "Account Name";
                colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                colAccount.Hidden = false;
                colAccount.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetImportStatus(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ImportStatus"))
            {
                UltraGridColumn colAccount = gridBand.Columns["ImportStatus"];
                //colAccount.Header.VisiblePosition = visiblePosition++;
                colAccount.Header.Caption = "Import Status";
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1294
                // Modified by Ankit Gupta on 22 aug, 2014
                // Initially import status was 'UnImported' but when it was changed to 'Not Imported'
                // space was not displayed between the 'Not' and 'Imported', therefore this code was commented/removed.
                //colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                colAccount.Hidden = false;
                colAccount.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetExecutingBroker(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ExecutingBroker"))
            {
                UltraGridColumn colCounterParty = gridBand.Columns["ExecutingBroker"];
                //colCounterParty.Header.VisiblePosition = visiblePosition++;
                colCounterParty.Header.Caption = "Counter Party";
                colCounterParty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                colCounterParty.Hidden = false;
                colCounterParty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCreateCounterParty(UltraGridBand gridBand)
        {
            if (!gridBand.Columns.Exists("CreateCounterParty"))
            {
                UltraGridColumn colCrateCounterParty = gridBand.Columns.Add("CreateCounterParty");
                //colCrateCounterParty.Header.VisiblePosition = visiblePosition++;
                colCrateCounterParty.Header.Caption = "Create Counter Party";
                colCrateCounterParty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colCrateCounterParty.Hidden = false;
                colCrateCounterParty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                colCrateCounterParty.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colCrateCounterParty.NullText = "Create Counter Party";
            }
        }

        private void SetPositionStartDate(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("PositionStartDate"))
            {
                UltraGridColumn ColPositionStartDate = gridBand.Columns["PositionStartDate"];
                //ColPositionStartDate.Header.VisiblePosition = visiblePosition++;
                ColPositionStartDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColPositionStartDate.Header.Caption = "Position StartDate";
                ColPositionStartDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColPositionStartDate.Hidden = false;
                ColPositionStartDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetAssetID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_ASSET_ID))
            {
                UltraGridColumn colAsset = gridBand.Columns[OrderFields.PROPERTY_ASSET_ID];
                //colAsset.Header.VisiblePosition = visiblePosition++;
                colAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colAsset.Header.Caption = OrderFields.CAPTION_ASSET_CLASS;
                colAsset.ValueList = SecMasterHelper.getInstance().Assets.Clone();
                colAsset.Hidden = false;
                colAsset.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCurrencyID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_CURRENCYID))
            {
                UltraGridColumn ColCurrency = gridBand.Columns[OrderFields.PROPERTY_CURRENCYID];
                //ColCurrency.Header.VisiblePosition = visiblePosition++;
                ColCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColCurrency.Header.Caption = "Currency";
                //we are making clone because there was error we cannot add same valuelist again
                ColCurrency.ValueList = SecMasterHelper.getInstance().Currencies.Clone();
                ColCurrency.Hidden = false;
                ColCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetSideTagValue(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("SideTagValue"))
            {
                UltraGridColumn ColSide = gridBand.Columns["SideTagValue"];
                //ColSide.Header.VisiblePosition = visiblePosition++;
                ColSide.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColSide.Header.Caption = "Side";
                ColSide.ValueList = GetSideValueLists();
                ColSide.Hidden = false;
                ColSide.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetNetPosition(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("NetPosition"))
            {
                UltraGridColumn ColNetPosition = gridBand.Columns["NetPosition"];
                //ColNetPosition.Header.VisiblePosition = visiblePosition++;
                ColNetPosition.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColNetPosition.Header.Caption = "Quantity";
                ColNetPosition.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColNetPosition.Hidden = false;
                ColNetPosition.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCostBasis(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("CostBasis"))
            {
                UltraGridColumn ColCostBasis = gridBand.Columns["CostBasis"];
                //ColCostBasis.Header.VisiblePosition = visiblePosition++;
                ColCostBasis.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColCostBasis.Header.Caption = OrderFields.CAPTION_AVGPRICE;
                ColCostBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColCostBasis.Hidden = false;
                ColCostBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetMarkPrice(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("MarkPrice"))
            {
                UltraGridColumn ColCostBasis = gridBand.Columns["MarkPrice"];
                //ColCostBasis.Header.VisiblePosition = visiblePosition++;
                ColCostBasis.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColCostBasis.Header.Caption = "Mark Price";
                ColCostBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColCostBasis.Hidden = false;
                ColCostBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCommission(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_COMMISSION))
            {
                UltraGridColumn ColCommission = gridBand.Columns[OrderFields.PROPERTY_COMMISSION];
                //ColCommission.Header.VisiblePosition = visiblePosition++;
                ColCommission.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColCommission.Header.Caption = OrderFields.CAPTION_COMMISSION;
                ColCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColCommission.Hidden = false;
                ColCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetFees(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_FEES))
            {
                UltraGridColumn ColFees = gridBand.Columns[OrderFields.PROPERTY_FEES];
                //ColFees.Header.VisiblePosition = visiblePosition++;
                ColFees.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColFees.Header.Caption = OrderFields.CAPTION_FEES;
                ColFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColFees.Hidden = false;
                ColFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetStampDuty(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_STAMPDUTY))
            {
                UltraGridColumn ColStampDuty = gridBand.Columns[OrderFields.PROPERTY_STAMPDUTY];
                //ColStampDuty.Header.VisiblePosition = visiblePosition++;
                ColStampDuty.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColStampDuty.Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                ColStampDuty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColStampDuty.Hidden = false;
                ColStampDuty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetTransactionLevy(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_TRANSACTIONLEVY))
            {
                UltraGridColumn ColTransactionLevy = gridBand.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                //ColTransactionLevy.Header.VisiblePosition = visiblePosition++;
                ColTransactionLevy.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColTransactionLevy.Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                ColTransactionLevy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColTransactionLevy.Hidden = false;
                ColTransactionLevy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetClearingFee(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_CLEARINGFEE))
            {
                UltraGridColumn ColClearingFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                //ColClearingFee.Header.VisiblePosition = visiblePosition++;
                ColClearingFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColClearingFee.Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                ColClearingFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColClearingFee.Hidden = false;
                ColClearingFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetMiscFees(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_MISCFEES))
            {
                UltraGridColumn ColMiscFees = gridBand.Columns[OrderFields.PROPERTY_MISCFEES];
                //ColMiscFees.Header.VisiblePosition = visiblePosition++;
                ColMiscFees.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColMiscFees.Header.Caption = OrderFields.CAPTION_MISCFEES;
                ColMiscFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColMiscFees.Hidden = false;
                ColMiscFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetOccFee(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_OCCFEE))
            {
                UltraGridColumn ColOccFee = gridBand.Columns[OrderFields.PROPERTY_OCCFEE];
                //ColOccFee.Header.VisiblePosition = visiblePosition++;
                ColOccFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColOccFee.Header.Caption = OrderFields.CAPTION_OCCFEE;
                ColOccFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColOccFee.Hidden = false;
                ColOccFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetOrfFee(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_ORFFEE))
            {
                UltraGridColumn ColOrfFee = gridBand.Columns[OrderFields.PROPERTY_ORFFEE];
                //ColOrfFee.Header.VisiblePosition = visiblePosition++;
                ColOrfFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColOrfFee.Header.Caption = OrderFields.CAPTION_ORFFEE;
                ColOrfFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColOrfFee.Hidden = false;
                ColOrfFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetSecFee(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_SECFEE))
            {
                UltraGridColumn ColSecFee = gridBand.Columns[OrderFields.PROPERTY_SECFEE];
                //ColSecFee.Header.VisiblePosition = visiblePosition++;
                ColSecFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColSecFee.Header.Caption = OrderFields.CAPTION_SECFEE;
                ColSecFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColSecFee.Hidden = false;
                ColSecFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetSoftCommission(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_SOFTCOMMISSION))
            {
                UltraGridColumn ColSoftCommission = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                //ColSoftCommission.Header.VisiblePosition = visiblePosition++;
                ColSoftCommission.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;
                ColSoftCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColSoftCommission.Hidden = false;
                ColSoftCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetClearingBrokerFee(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_CLEARINGBROKERFEE))
            {
                UltraGridColumn ColClearingBrokerFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                //ColClearingBrokerFee.Header.VisiblePosition = visiblePosition++;
                ColClearingBrokerFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                ColClearingBrokerFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColClearingBrokerFee.Hidden = false;
                ColClearingBrokerFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetVenueID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("VenueID"))
            {
                UltraGridColumn ColVenue = gridBand.Columns["VenueID"];
                //ColVenue.Header.VisiblePosition = visiblePosition++;
                ColVenue.Header.Caption = "Venue";
                ColVenue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColVenue.ValueList = GetValueList(Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllVenues());
                ColVenue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetStrategy(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Strategy"))
            {
                UltraGridColumn ColStrategy = gridBand.Columns["Strategy"];
                //ColStrategy.Header.VisiblePosition = visiblePosition++;
                ColStrategy.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColStrategy.Header.Caption = "Strategy";
                ColStrategy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColStrategy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetPBSymbol(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("PBSymbol"))
            {
                UltraGridColumn ColPBSymbol = gridBand.Columns["PBSymbol"];
                //ColPBSymbol.Header.VisiblePosition = visiblePosition++;
                ColPBSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColPBSymbol.Header.Caption = "PB Symbol";
                ColPBSymbol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColPBSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetPBAssetType(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("PBAssetType"))
            {
                UltraGridColumn ColPBAssetType = gridBand.Columns["PBAssetType"];
                //ColPBAssetType.Header.VisiblePosition = visiblePosition++;
                ColPBAssetType.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColPBAssetType.Header.Caption = "PB AssetType";
                ColPBAssetType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColPBAssetType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetDescription(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Description"))
            {
                UltraGridColumn ColDescription = gridBand.Columns["Description"];
                //ColDescription.Header.VisiblePosition = visiblePosition++;
                ColDescription.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColDescription.Header.Caption = "Description";
                ColDescription.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColDescription.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetFXConversionMethodOperator(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("FXConversionMethodOperator"))
            {
                UltraGridColumn ColFXConversionMethodOperator = gridBand.Columns["FXConversionMethodOperator"];
                //ColFXConversionMethodOperator.Header.VisiblePosition = visiblePosition++;
                ColFXConversionMethodOperator.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColFXConversionMethodOperator.Header.Caption = "FXConversionMethodOperator";
                ColFXConversionMethodOperator.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                //TODO: Add value lis here
                ColFXConversionMethodOperator.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetFXRate(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("FXRate"))
            {
                UltraGridColumn ColFXRate = gridBand.Columns["FXRate"];
                //ColFXRate.Header.VisiblePosition = visiblePosition++;
                ColFXRate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColFXRate.Header.Caption = "FXRate";
                ColFXRate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColFXRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetRIC(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("RIC"))
            {
                UltraGridColumn ColReutresSymbol = gridBand.Columns["RIC"];
                //ColReutresSymbol.Header.VisiblePosition = visiblePosition++;
                ColReutresSymbol.CharacterCasing = CharacterCasing.Upper;
                ColReutresSymbol.Header.Caption = OrderFields.CAPTION_RICSYMBOL;
                ColReutresSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColReutresSymbol.NullText = String.Empty;
                ColReutresSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetISIN(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ISIN"))
            {
                UltraGridColumn ColISINSymbol = gridBand.Columns["ISIN"];
                //ColISINSymbol.Header.VisiblePosition = visiblePosition++;
                ColISINSymbol.CharacterCasing = CharacterCasing.Upper;
                ColISINSymbol.Header.Caption = OrderFields.CAPTION_ISINSYMBOL;
                ColISINSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColISINSymbol.NullText = String.Empty;
                ColISINSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetSEDOL(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("SEDOL"))
            {
                UltraGridColumn ColSEDOLSymbol = gridBand.Columns["SEDOL"];
                //ColSEDOLSymbol.Header.VisiblePosition = visiblePosition++;
                ColSEDOLSymbol.CharacterCasing = CharacterCasing.Upper;
                ColSEDOLSymbol.Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                ColSEDOLSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColSEDOLSymbol.NullText = String.Empty;
                ColSEDOLSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetOSIOptionSymbol(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()))
            {
                UltraGridColumn ColOSIOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()];
                //ColOSIOptionSymbol.Header.VisiblePosition = visiblePosition++;
                ColOSIOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                ColOSIOptionSymbol.Header.Caption = OrderFields.CAPTION_OSIOPTIONSYMBOL;
                ColOSIOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColOSIOptionSymbol.NullText = String.Empty;
                ColOSIOptionSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetIDCOOptionSymbol(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()))
            {
                UltraGridColumn ColIDCOOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()];
                //ColIDCOOptionSymbol.Header.VisiblePosition = visiblePosition++;
                ColIDCOOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                ColIDCOOptionSymbol.Header.Caption = OrderFields.CAPTION_IDCOOPTIONSYMBOL;
                ColIDCOOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColIDCOOptionSymbol.NullText = String.Empty;
                ColIDCOOptionSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetOPRAOptionSymbol(UltraGridBand gridBand)
        {
            // Hide column as gennaro asked to do.
            if (gridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.OPRAOptionSymbol.ToString()))
            {
                UltraGridColumn ColOPRAOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.OPRAOptionSymbol.ToString()];
                ColOPRAOptionSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetMultiplier(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Multiplier"))
            {
                UltraGridColumn ColMultiplier = gridBand.Columns["Multiplier"];
                //ColMultiplier.Header.VisiblePosition = visiblePosition++;
                ColMultiplier.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColMultiplier.NullText = null;
                ColMultiplier.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetUnderLyingID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_UNDERLYING_ID))
            {
                UltraGridColumn colUnderLying = gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID];
                //colUnderLying.Header.VisiblePosition = visiblePosition++;
                colUnderLying.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colUnderLying.Header.Caption = OrderFields.CAPTION_UNDERLYING_NAME;
                colUnderLying.ValueList = SecMasterHelper.getInstance().UnderLyings.Clone();
                colUnderLying.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

            }
        }

        private void SetExchangeID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_EXCHANGEID))
            {
                UltraGridColumn ColExchnage = gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID];
                //ColExchnage.Header.VisiblePosition = visiblePosition++;
                ColExchnage.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColExchnage.Header.Caption = OrderFields.CAPTION_EXCHANGE;
                ColExchnage.ValueList = SecMasterHelper.getInstance().Exchanges.Clone();
                ColExchnage.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetIssueDate(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("IssueDate"))
            {
                UltraGridColumn ColIssueDate = gridBand.Columns["IssueDate"];
                //ColIssueDate.Header.VisiblePosition = visiblePosition++;
                ColIssueDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetFirstCouponDate(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("FirstCouponDate"))
            {
                UltraGridColumn ColFirstCouponDate = gridBand.Columns["FirstCouponDate"];
                //ColFirstCouponDate.Header.VisiblePosition = visiblePosition++;
                ColFirstCouponDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetExpirationDate(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ExpirationDate"))
            {
                UltraGridColumn ColExpirationOrSettlementDate = gridBand.Columns["ExpirationDate"];
                //ColExpirationOrSettlementDate.Header.VisiblePosition = visiblePosition++;
                ColExpirationOrSettlementDate.Header.Caption = OrderFields.CAPTION_EXPIRATIONDATE; //"Expiration Date";
                ColExpirationOrSettlementDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColExpirationOrSettlementDate.NullText = "1/1/1800";
                ColExpirationOrSettlementDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetUnderlyingSymbol(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_UNDERLYINGSYMBOL))
            {
                UltraGridColumn ColUnderLyingSymbol = gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL];
                //ColUnderLyingSymbol.Header.VisiblePosition = visiblePosition++;
                ColUnderLyingSymbol.Header.Caption = OrderFields.CAPTION_UNDERLYINGSYMBOL;
                ColUnderLyingSymbol.CharacterCasing = CharacterCasing.Upper;
                ColUnderLyingSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColUnderLyingSymbol.NullText = String.Empty;
                ColUnderLyingSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCall_Put(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Call_Put"))
            {
                UltraGridColumn ColOptionType = gridBand.Columns["Call_Put"];
                //ColOptionType.Header.VisiblePosition = visiblePosition++;
                ColOptionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColOptionType.ValueList = SecMasterHelper.getInstance().OptionTypes.Clone();
                ColOptionType.Header.Caption = OrderFields.CAPTION_PUT_CALL;
                ColOptionType.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColOptionType.NullText = String.Empty;
                ColOptionType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetStrikePrice(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_STRIKE_PRICE))
            {
                UltraGridColumn ColStrikePirce = gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE];
                //ColStrikePirce.Header.VisiblePosition = visiblePosition++;
                ColStrikePirce.Header.Caption = OrderFields.CAPTION_STRIKE_PRICE;
                ColStrikePirce.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColStrikePirce.NullText = null;
                ColStrikePirce.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetLeadCurrencyID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_LEADCURRENCYID))
            {
                UltraGridColumn ColLeadCurrency = gridBand.Columns[OrderFields.PROPERTY_LEADCURRENCYID];
                UltraGridColumn ColVsCurrency = gridBand.Columns[OrderFields.PROPERTY_VSCURRENCYID];
                ValueList listCurencies = GetValueList(CachedDataManager.GetInstance.GetAllCurrencies());
                ColLeadCurrency.ValueList = listCurencies;
                ColVsCurrency.ValueList = listCurencies;
                ColVsCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetAccrualBasis(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("AccrualBasis"))
            {
                UltraGridColumn ColAccrualBasis = gridBand.Columns["AccrualBasis"];
                //ColAccrualBasis.Header.VisiblePosition = visiblePosition++;
                ColAccrualBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColAccrualBasis.Header.Caption = OrderFields.CAPTION_ACCRUALBASIS;
                ColAccrualBasis.ValueList = SecMasterHelper.getInstance().AccrualBasis.Clone();
                ColAccrualBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetFreq(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Freq"))
            {
                UltraGridColumn ColFrequency = gridBand.Columns["Freq"];
                //ColFrequency.Header.VisiblePosition = visiblePosition++;
                ColFrequency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColFrequency.Header.Caption = "Coupon Frequency";
                ColFrequency.ValueList = SecMasterHelper.getInstance().Frequencies.Clone();
                ColFrequency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCoupon(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Coupon"))
            {
                UltraGridColumn ColCoupon = gridBand.Columns["Coupon"];
                ColCoupon.Header.Caption = "Coupon (%)";
                ColCoupon.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetIsSecApproved(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(ApplicationConstants.CONST_IS_SECURITY_APPROVED))
            {
                //Hide isSecApproved Column becuase it showing checkbox , we dont want it.
                UltraGridColumn colIsSecApproved = gridBand.Columns[ApplicationConstants.CONST_IS_SECURITY_APPROVED];
                colIsSecApproved.Hidden = true;
                colIsSecApproved.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetIsSymbolMapped(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(ApplicationConstants.CONST_IS_SYMBOL_MAPPED))
            {
                UltraGridColumn colIsSymbolMapped = gridBand.Columns[ApplicationConstants.CONST_IS_SYMBOL_MAPPED];
                colIsSymbolMapped.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetValidationError(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ValidationError"))
            {
                //we are showing Approval Status based on isSecApproved property.
                UltraGridColumn colComments = gridBand.Columns["ValidationError"];
                colComments.Header.Caption = "Comments";
                //colApprovalStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //  colApprovalStatus.ValueList = _approvalSatus;
                colComments.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetMismatchType(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("MismatchType"))
            {
                UltraGridColumn colMismatchType = gridBand.Columns["MismatchType"];
                //colMismatchType.Header.VisiblePosition = visiblePosition++;
                colMismatchType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colMismatchType.Header.Caption = "MismatchType";
                colMismatchType.Hidden = false;
                colMismatchType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetMismatchDetails(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("MismatchDetails"))
            {
                UltraGridColumn colMismatchDetails = gridBand.Columns["MismatchDetails"];
                //colMismatchDetails.Header.VisiblePosition = visiblePosition++;
                colMismatchDetails.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colMismatchDetails.Header.Caption = "MismatchDetails";
                colMismatchDetails.Hidden = true;
                colMismatchDetails.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
        }

        private void SetApprovedBy(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ApprovedBy"))
            {
                UltraGridColumn ColApprovedBy = gridBand.Columns["ApprovedBy"];
                ColApprovedBy.Header.Caption = "Last Approved By ";
                ColApprovedBy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColApprovedBy.ValueList = GetValueList(CachedDataManager.GetInstance.GetAllUsersName());
                ColApprovedBy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetUDAAssetClassID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("UDAAssetClassID"))
            {
                UltraGridColumn colUDAAsset = gridBand.Columns["UDAAssetClassID"];
                colUDAAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colUDAAsset.Header.Caption = "UDA Asset";
                colUDAAsset.ValueList = SecMasterHelper.getInstance().UDAAssets.Clone();
                colUDAAsset.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetUDASectorID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("UDASectorID"))
            {
                UltraGridColumn colUDAUDASector = gridBand.Columns["UDASectorID"];
                colUDAUDASector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colUDAUDASector.Header.Caption = "UDA Sector";
                colUDAUDASector.ValueList = SecMasterHelper.getInstance().UDASectors.Clone();
                colUDAUDASector.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetUDASubSectorID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("UDASubSectorID"))
            {
                UltraGridColumn colUDASubSector = gridBand.Columns["UDASubSectorID"];
                colUDASubSector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colUDASubSector.Header.Caption = "UDA SubSector";
                colUDASubSector.ValueList = SecMasterHelper.getInstance().UDASubSectors.Clone();
                colUDASubSector.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetUDASecurityTypeID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("UDASecurityTypeID"))
            {
                UltraGridColumn colUDASecurityType = gridBand.Columns["UDASecurityTypeID"];
                colUDASecurityType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colUDASecurityType.Header.Caption = "UDA Security";
                colUDASecurityType.ValueList = SecMasterHelper.getInstance().UDASecurityTypes.Clone();
                colUDASecurityType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetUDACountryID(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("UDACountryID"))
            {
                UltraGridColumn colUDACountry = gridBand.Columns["UDACountryID"];
                colUDACountry.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colUDACountry.Header.Caption = "UDA Country";
                colUDACountry.ValueList = SecMasterHelper.getInstance().UDACountries.Clone();
                colUDACountry.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetPranaExecutionDate(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("NirvanaProcessDate"))
            {
                UltraGridColumn colExecutionDate = gridBand.Columns["NirvanaProcessDate"];
                colExecutionDate.Header.Caption = "Process Date";
                colExecutionDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

            }
        }

        //private void SetCounterPartyID(UltraGridBand gridBand)
        //{
        //if (gridBand.Columns.Exists("CounterPartyID"))
        //{
        //    UltraGridColumn ColCounterParty = gridBand.Columns["CounterPartyID"];
        //    ColCounterParty.Header.VisiblePosition = visiblePosition++;
        //    ColCounterParty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
        //    ColCounterParty.Header.Caption = "CounterParty";
        //    ColCounterParty.ValueList = GetValueList(CachedDataManager.GetInstance.GetUserCounterParties());
        //    ColCounterParty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
        //}
        //}

        private void SetBeta(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Beta"))
            {
                UltraGridColumn ColBeta = gridBand.Columns["Beta"];
                //ColBeta.Header.VisiblePosition = visiblePosition++;
                ColBeta.Header.Caption = "Beta";
                ColBeta.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColBeta.NullText = null;
                ColBeta.Hidden = false;
                ColBeta.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetBaseCurrency(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("BaseCurrency"))
            {
                UltraGridColumn ColBaseCurrency = gridBand.Columns["BaseCurrency"];
                //ColBaseCurrency.Header.VisiblePosition = visiblePosition++;
                ColBaseCurrency.Header.Caption = "Base Currency";
                ColBaseCurrency.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColBaseCurrency.NullText = null;
                ColBaseCurrency.Hidden = false;
                ColBaseCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetLocalCurrency(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("LocalCurrency"))
            {
                UltraGridColumn ColLocalCurrency = gridBand.Columns["LocalCurrency"];
                //ColLocalCurrency.Header.VisiblePosition = visiblePosition++;
                ColLocalCurrency.Header.Caption = "Local Currency";
                ColLocalCurrency.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColLocalCurrency.NullText = null;
                ColLocalCurrency.Hidden = false;
                ColLocalCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCashValueBase(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("CashValueBase"))
            {
                UltraGridColumn ColCashValueBase = gridBand.Columns["CashValueBase"];
                //ColCashValueBase.Header.VisiblePosition = visiblePosition++;
                ColCashValueBase.Header.Caption = "Cash Value Base";
                ColCashValueBase.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColCashValueBase.NullText = null;
                ColCashValueBase.Hidden = false;
                ColCashValueBase.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCashValueLocal(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("CashValueLocal"))
            {
                UltraGridColumn ColCashValueLocal = gridBand.Columns["CashValueLocal"];
                //ColCashValueLocal.Header.VisiblePosition = visiblePosition++;
                ColCashValueLocal.Header.Caption = "Cash Value Local";
                ColCashValueLocal.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColCashValueLocal.NullText = null;
                ColCashValueLocal.Hidden = false;
                ColCashValueLocal.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetSettlementCurrency(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists((OrderFields.PROPERTY_SETTLEMENTCURRENCYNAME)))
            {
                UltraGridColumn ColSettlementCurrency = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYNAME];
                //ColSettlementCurrency.Header.VisiblePosition = visiblePosition++;
                ColSettlementCurrency.Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                ColSettlementCurrency.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColSettlementCurrency.NullText = null;
                ColSettlementCurrency.Hidden = false;
                ColSettlementCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        //Added By : Manvendra Prajapati
        //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-10380
        /// <summary>
        /// TO Set Fx Conversion Operator Column for Import Status Report
        /// </summary>
        /// <param name="gridBand"></param>
        private void SetFxConversionMethodOperator(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR))
            {
                UltraGridColumn ColFXConversionMethodOperator = gridBand.Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR];
                ColFXConversionMethodOperator.Header.Caption = OrderFields.CAPTION_FX_CONVERSION_METHOD_OPERATOR;
                ColFXConversionMethodOperator.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing; ;
                ColFXConversionMethodOperator.NullText = null;
                ColFXConversionMethodOperator.Hidden = false;
                ColFXConversionMethodOperator.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                ColFXConversionMethodOperator.ValueList = GetFxConvertionMethodOperator();
                ColFXConversionMethodOperator.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

            }
        }

        /// <summary>
        /// Get Fx Conversion Method Operator valuelist
        /// </summary>
        /// <returns></returns>
        private ValueList GetFxConvertionMethodOperator()
        {
            ValueList fxConversionMethodOperatorList = new ValueList();
            try
            {
                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    fxConversionMethodOperatorList.ValueListItems.Add(var.Value.ToString(), var.DisplayText);
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
            return fxConversionMethodOperatorList;
        }

        private void SetForexPrice(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ForexPrice"))
            {
                UltraGridColumn ColForexPirce = gridBand.Columns["ForexPrice"];
                //ColForexPirce.Header.VisiblePosition = visiblePosition++;
                ColForexPirce.Header.Caption = "Forex Price";
                ColForexPirce.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColForexPirce.NullText = null;
                ColForexPirce.Hidden = false;
                ColForexPirce.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetLongDebitBalance(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("LongDebitBalance"))
            {
                UltraGridColumn ColLondDebitBal = gridBand.Columns["LongDebitBalance"];
                //ColLondDebitBal.Header.VisiblePosition = visiblePosition++;
                ColLondDebitBal.Header.Caption = "Long Debit Balance";
                ColLondDebitBal.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColLondDebitBal.NullText = null;
                ColLondDebitBal.Hidden = false;
                ColLondDebitBal.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetShortCreditBalance(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("ShortCreditBalance"))
            {
                UltraGridColumn ColShortCreditBal = gridBand.Columns["ShortCreditBalance"];
                //ColShortCreditBal.Header.VisiblePosition = visiblePosition++;
                ColShortCreditBal.Header.Caption = "Short Credit Balance";
                ColShortCreditBal.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColShortCreditBal.NullText = null;
                ColShortCreditBal.Hidden = false;
                ColShortCreditBal.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }


        private void SetRoundLot(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("RoundLot"))
            {
                UltraGridColumn ColRoundLot = gridBand.Columns["RoundLot"];
                //ColRoundLot.Header.VisiblePosition = visiblePosition++;
                ColRoundLot.Header.Caption = "RoundLot";
                ColRoundLot.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColRoundLot.NullText = null;
                ColRoundLot.Hidden = false;
                ColRoundLot.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColRoundLot.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetQuantity(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Quantity"))
            {
                UltraGridColumn ColQuantity = gridBand.Columns["Quantity"];
                //ColQuantity.Header.VisiblePosition = visiblePosition++;
                ColQuantity.Header.Caption = "Quantity";
                ColQuantity.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColQuantity.NullText = null;
                ColQuantity.Hidden = false;
                ColQuantity.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColQuantity.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetOrderSideTagValue(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("OrderSideTagValue"))
            {
                UltraGridColumn ColOrderSideTagValue = gridBand.Columns["OrderSideTagValue"];
                //ColOrderSideTagValue.Header.VisiblePosition = visiblePosition++;
                ColOrderSideTagValue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColOrderSideTagValue.Header.Caption = "OrderSideTagValue";
                ColOrderSideTagValue.ValueList = GetSideValueLists();
                ColOrderSideTagValue.Hidden = false;
                ColOrderSideTagValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetAllocationSchemeKey(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("AllocationSchemeKey"))
            {
                UltraGridColumn colAllocationSchemeKey = gridBand.Columns["AllocationSchemeKey"];
                //colAllocationSchemeKey.Header.VisiblePosition = visiblePosition++;
                colAllocationSchemeKey.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colAllocationSchemeKey.Header.Caption = "AllocationSchemeKey";
                colAllocationSchemeKey.Hidden = true;
                colAllocationSchemeKey.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
        }

        private void SetPercentage(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Percentage"))
            {
                UltraGridColumn ColPercentage = gridBand.Columns["Percentage"];
                //ColPercentage.Header.VisiblePosition = visiblePosition++;
                ColPercentage.Header.Caption = "Percentage";
                ColPercentage.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColPercentage.NullText = null;
                ColPercentage.Hidden = false;
                ColPercentage.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetAlgoStrategyName(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("AlgoStrategyName"))
            {
                UltraGridColumn colAlgoStrategyName = gridBand.Columns["AlgoStrategyName"];
                //colAlgoStrategyName.Header.VisiblePosition = visiblePosition++;
                colAlgoStrategyName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colAlgoStrategyName.Header.Caption = "AlgoStrategyName";
                colAlgoStrategyName.Hidden = false;
                colAlgoStrategyName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
        }

        private void SetCounterPartyName(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("CounterPartyName"))
            {
                UltraGridColumn colCounterPartyName = gridBand.Columns["CounterPartyName"];
                //colCounterPartyName.Header.VisiblePosition = visiblePosition++;
                colCounterPartyName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colCounterPartyName.Header.Caption = "CounterPartyName";
                colCounterPartyName.Hidden = false;
                colCounterPartyName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
        }

        // Some import types [Forex, Cash, Credit limit] save their validation status in "validated" property
        private void SetValidated(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Validated"))
            {
                UltraGridColumn colApprovalStatus = gridBand.Columns["Validated"];
                colApprovalStatus.Header.Caption = "Validation Status";
                colApprovalStatus.Hidden = false;
                colApprovalStatus.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }
        /// <summary>
        /// Set Properties for Column OptionPremiumAdjustment
        /// </summary>
        /// <param name="band"></param>
        private void SetOptionPremiumAdjustment(UltraGridBand band)
        {
            try
            {
                UltraGridColumn ColOptionPremiumAdjustment = band.Columns[OrderFields.PROPERTY_MISCFEES];
                //ColMiscFees.Header.VisiblePosition = visiblePosition++;
                ColOptionPremiumAdjustment.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColOptionPremiumAdjustment.Header.Caption = OrderFields.CAPTION_OPTION_PREMIUM_ADJUSTMENT;
                ColOptionPremiumAdjustment.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColOptionPremiumAdjustment.Hidden = false;
                ColOptionPremiumAdjustment.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
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

        private void SetVolatility(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Volatility"))
            {
                UltraGridColumn ColVolatility = gridBand.Columns["Volatility"];
                ColVolatility.Header.Caption = "Volatility";
                ColVolatility.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColVolatility.NullText = null;
                ColVolatility.Hidden = false;
                ColVolatility.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetVWAP(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("VWAP"))
            {
                UltraGridColumn ColVWAP = gridBand.Columns["VWAP"];
                ColVWAP.Header.Caption = "VWAP";
                ColVWAP.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColVWAP.NullText = null;
                ColVWAP.Hidden = false;
                ColVWAP.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetCollateralPrice(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("CollateralPrice"))
            {
                UltraGridColumn ColCollat = gridBand.Columns["CollateralPrice"];
                ColCollat.Header.Caption = "Collateral Price";
                ColCollat.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColCollat.NullText = null;
                ColCollat.Hidden = false;
                ColCollat.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            if (gridBand.Columns.Exists("Haircut"))
            {
                UltraGridColumn ColHaircut = gridBand.Columns["Haircut"];
                ColHaircut.Header.Caption = "Haircut(%)";
                ColHaircut.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColHaircut.NullText = null;
                ColHaircut.Hidden = false;
                ColHaircut.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            if (gridBand.Columns.Exists("RebateOnMV"))
            {
                UltraGridColumn ColFMV = gridBand.Columns["RebateOnMV"];
                ColFMV.Header.Caption = "Fee/Rebate on MV(%)";
                ColFMV.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColFMV.NullText = null;
                ColFMV.Hidden = false;
                ColFMV.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            if (gridBand.Columns.Exists("RebateOnCollateral"))
            {
                UltraGridColumn ColFRC = gridBand.Columns["RebateOnCollateral"];
                ColFRC.Header.Caption = "Fee/Rebate on Collateral(%)";
                ColFRC.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColFRC.NullText = null;
                ColFRC.Hidden = false;
                ColFRC.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void SetDividendYield(UltraGridBand gridBand)
        {
            if (gridBand.Columns.Exists("Dividend Yield"))
            {
                UltraGridColumn ColDividendYield = gridBand.Columns["Dividend Yield"];
                ColDividendYield.Header.Caption = "Dividend Yield";
                ColDividendYield.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                ColDividendYield.NullText = null;
                ColDividendYield.Hidden = false;
                ColDividendYield.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }
        #endregion SetColumnPropertiesFunctions

        #region valueLists for columns

        private ValueList GetValueList(Dictionary<int, string> values)
        {
            ValueList list = new ValueList();
            try
            {
                list.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> value in values)
                {
                    list.ValueListItems.Add(value.Key, value.Value);
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
            return list;
        }

        /// <summary>
        /// To set Value list for side
        /// </summary>
        /// <returns></returns>
        private ValueList GetSideValueLists()
        {
            #region side value lists
            ValueList valueListSides = new ValueList();
            try
            {
                valueListSides.ValueListItems.Clear();
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy, "Buy");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy_Closed, "Buy to Close");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy_Open, "Buy to Open");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell, "Sell");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell_Closed, "Sell to Close");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell_Open, "Sell to Open");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_SellShort, "Sell short");
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return valueListSides;
            #endregion
        }

        #endregion

    }
}
