using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PM.Client.UI
{
    class AmendmentsHelper
    {

        static Dictionary<string, Dictionary<string, ApprovedChanges>> _dictAmendments = new Dictionary<string, Dictionary<string, ApprovedChanges>>();

        static double _accountRealizedPNL = double.MinValue;
        static double _accountUnRealizedPNL = double.MinValue;
        static double _symbolRealizedPNL = double.MinValue;
        static double _symbolUnRealizedPNL = double.MinValue;

        /// <summary>
        /// Update taxlot open quantity along with taxlot executed quantity
        /// </summary>
        /// <param name="taxlot"></param>
        /// <param name="originalValue"></param>
        /// <param name="updatedValue"></param>
        /// <returns></returns>
        internal static bool UpdateExecutedQty(TaxLot taxlot, double originalValue, double updatedValue)
        {
            try
            {
                if (updatedValue < originalValue)
                {
                    if ((originalValue - updatedValue) > taxlot.TaxLotQty)
                        return false;
                    else
                        taxlot.TaxLotQty -= (originalValue - updatedValue);
                }
                else
                {
                    taxlot.TaxLotQty += (updatedValue - originalValue);
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
            return true;
        }

        /// <summary>
        /// Get Total PNL
        /// </summary>
        /// <returns></returns>
        internal static void GetTotalPNL(out double accountPNL, out double SymbolPNL)
        {
            accountPNL = 0;
            SymbolPNL = 0;
            try
            {
                accountPNL = _accountRealizedPNL + _accountUnRealizedPNL;
                SymbolPNL = _symbolRealizedPNL + _symbolUnRealizedPNL;
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
        /// Update UnRealized PNL when amendments are made in trade
        /// </summary>
        /// <param name="unRealizedPNL"></param>
        internal static void UpdateUnRealizedPNL(double unRealizedPNL)
        {
            try
            {
                _accountUnRealizedPNL = _accountUnRealizedPNL + unRealizedPNL;
                _symbolUnRealizedPNL = _symbolUnRealizedPNL + unRealizedPNL;
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
        /// Set the new Realized UnRealized PNL in Amendment hepler
        /// </summary>
        /// <param name="realizedPNL"></param>
        /// <param name="unRealizedPNL"></param>
        internal static void SetRealizedUnRealizedPNL(Double accountRealizedPNL, Double accountUnRealizedPNL, Double SymbolRealizedPNL, Double SymbolUnRealizedPNL)
        {
            try
            {
                _accountRealizedPNL = accountRealizedPNL;
                _accountUnRealizedPNL = accountUnRealizedPNL;
                _symbolRealizedPNL = SymbolRealizedPNL;
                _symbolUnRealizedPNL = SymbolUnRealizedPNL;
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
        /// Update UnRealized/Realized PNL After Closing Unwinding
        /// </summary>
        /// <param name="newRealizedPNL"></param>
        internal static void UpdateUnRealizedRealizedPNLAfterClosingUnwinding(ClosingData closingdata)
        {
            try
            {
                double newRealizedPNL = closingdata.ClosedPositions.Sum(it => it.CostBasisRealizedPNL);

                double diffUnrealizedPNL = newRealizedPNL - _accountRealizedPNL;
                _accountRealizedPNL = newRealizedPNL;
                _accountUnRealizedPNL = _accountUnRealizedPNL - diffUnrealizedPNL;

                diffUnrealizedPNL = newRealizedPNL - _symbolRealizedPNL;
                _symbolRealizedPNL = newRealizedPNL;
                _symbolUnRealizedPNL = _symbolUnRealizedPNL - diffUnrealizedPNL;
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
        /// clear amendments
        /// </summary>
        internal static void ClearAmendments()
        {
            try
            {
                _dictAmendments.Clear();
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
        /// check if there are amendments to save
        /// </summary>
        /// <returns></returns>
        internal static bool IsAmendmentsToSave()
        {
            try
            {
                if (_dictAmendments.Count > 0)
                {
                    return true;
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
            return false;
        }

        /// <summary>
        /// get approved changes dictionary
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, List<ApprovedChanges>> GetApprovedChangesDictionary()
        {
            Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = new Dictionary<string, List<ApprovedChanges>>();
            try
            {
                _dictAmendments.Keys.ToList().ForEach(taxLotID =>
                {
                    _dictAmendments[taxLotID].Keys.ToList().ForEach(column =>
                    {
                        if (dictApprovedChanges.ContainsKey(taxLotID))
                        {
                            dictApprovedChanges[taxLotID].Add(_dictAmendments[taxLotID][column]);
                        }
                        else
                        {
                            List<ApprovedChanges> approvedChanges = new List<ApprovedChanges>();
                            approvedChanges.Add(_dictAmendments[taxLotID][column]);
                            dictApprovedChanges.Add(taxLotID, approvedChanges);
                        }
                    });
                });
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
            return dictApprovedChanges;
        }

        /// <summary>
        /// add amendments in the dictionary
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        /// <param name="taxlot"></param>
        internal static void UpdateAmendmetsDictionary(string columnName, string newValue, string oldValue, TaxLot taxlot)
        {
            try
            {

                if (_dictAmendments.Keys.Contains(taxlot.TaxLotID))
                {
                    if (_dictAmendments[taxlot.TaxLotID].Keys.Contains(columnName))
                    {
                        _dictAmendments[taxlot.TaxLotID][columnName].NewValue = newValue;

                        if (_dictAmendments[taxlot.TaxLotID][columnName].NewValue == _dictAmendments[taxlot.TaxLotID][columnName].OldValue)
                        {
                            _dictAmendments[taxlot.TaxLotID].Remove(columnName);
                            if (_dictAmendments[taxlot.TaxLotID].Count == 0)
                            {
                                _dictAmendments.Remove(taxlot.TaxLotID);
                            }
                        }
                    }
                    else
                    {
                        ApprovedChanges approvedChanges = new ApprovedChanges();

                        approvedChanges.ColumnName = ColumnMapping.GetColumnNameForApprovedChanged(columnName);
                        approvedChanges.NewValue = newValue;
                        approvedChanges.TaxlotID = taxlot.TaxLotID;
                        approvedChanges.OldValue = oldValue;

                        _dictAmendments[taxlot.TaxLotID].Add(columnName, approvedChanges);
                    }
                }
                else
                {
                    ApprovedChanges approvedChanges = new ApprovedChanges();
                    approvedChanges.ColumnName = ColumnMapping.GetColumnNameForApprovedChanged(columnName);
                    approvedChanges.NewValue = newValue;
                    approvedChanges.TaxlotID = taxlot.TaxLotID;
                    approvedChanges.OldValue = oldValue;

                    Dictionary<string, ApprovedChanges> columnAmendments = new Dictionary<string, ApprovedChanges>();
                    columnAmendments.Add(columnName, approvedChanges);
                    _dictAmendments.Add(taxlot.TaxLotID, columnAmendments);
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
        /// Moved to Amendments Helper as we needed to handle Virtual delete taxlots
        /// </summary>
        /// <param name="taxLot"></param>
        internal static void DeleteTaxLot(TaxLot taxLot)
        {
            try
            {

                //create dictionary to delete the tax lot with tax lotID and tax lot status
                ApprovedChanges approvedChanges = new ApprovedChanges();
                approvedChanges.TaxlotID = taxLot.TaxLotID;
                approvedChanges.TaxlotStatus = BusinessObjects.AppConstants.AmendedTaxLotStatus.Deleted;
                List<ApprovedChanges> lstApprovedChanges = new List<ApprovedChanges>();
                lstApprovedChanges.Add(approvedChanges);
                Dictionary<string, ApprovedChanges> deletedTaxlots = new Dictionary<string, ApprovedChanges>();
                deletedTaxlots.Add("Deleted", approvedChanges);
                if (!_dictAmendments.ContainsKey(taxLot.TaxLotID))
                {
                    _dictAmendments.Add(taxLot.TaxLotID, deletedTaxlots);
                }
                else if (!_dictAmendments[taxLot.TaxLotID].ContainsKey("Deleted"))
                {
                    _dictAmendments.Remove(taxLot.TaxLotID);
                    _dictAmendments.Add(taxLot.TaxLotID, deletedTaxlots);
                }
                if (PostReconClosingData.OpenTaxlots.ContainsKey(taxLot.TaxLotID))
                {
                    PostReconClosingData.OpenTaxlots.Remove(taxLot.TaxLotID);
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
    }
}

