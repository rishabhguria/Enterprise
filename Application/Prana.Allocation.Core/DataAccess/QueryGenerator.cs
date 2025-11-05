// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-03-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-05-2014
// ***********************************************************************
// <copyright file="QueryGenerator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// The DataAccess namespace.
/// </summary>
namespace Prana.Allocation.Core.DataAccess
{
    // TODO: This class is copied from existing code as it is. So need to REFACTOR it

    /// <summary>
    /// Class QueryGenerator.
    /// </summary>
    internal class QueryGenerator
    {
        /// <summary>
        /// Select operator type
        /// </summary>
        public enum OperatorType
        {
            /// <summary>
            /// Converts to '=' operator in sql query
            /// </summary>
            Equals,
            /// <summary>
            /// Converts to Like '%val%' in sql query
            /// </summary>
            Like,
            /// <summary>
            /// Like operator on multiple values passed as CSV, OR condition will be applied
            /// </summary>
            LikeMultipleOr,
            /// <summary>
            /// Equals conditions using 'in' condition
            /// </summary>
            EqualsMultiple,
            /// <summary>
            /// Denotes only condition after IN. Uses only value to create condition. ColumnName should already be used
            /// Such as "('2','3','4')"
            /// </summary>
            OnlyINCondition,
        }

        /// <summary>
        /// Gets the applied filters query.
        /// </summary>
        /// <param name="filterDictionary">The filter dictionary.</param>
        /// <returns></returns>
        private static string GetAppliedFiltersQuery(Dictionary<String, String> filterDictionary)
        {
            StringBuilder filterQuery = new StringBuilder();
            try
            {
                if (filterDictionary != null && filterDictionary.Count > 0)
                {
                    foreach (String key in filterDictionary.Keys)
                    {
                        switch (key)
                        {
                            case "GroupID":
                                filterQuery.Append(" and (g.GroupID in");
                                filterQuery.Append(CreateCondtion(key, filterDictionary[key], OperatorType.OnlyINCondition));
                                filterQuery.Append(") ");
                                continue;
                            case "Symbol":
                                filterQuery.Append(CreateCondtion(key, filterDictionary[key], OperatorType.LikeMultipleOr));
                                continue;
                            case "FundID":
                                filterQuery.Append(" and (g.GroupID in ( Select DISTINCT l.GroupID from l where l.AccountID in ");
                                filterQuery.Append(CreateCondtion(key, filterDictionary[key], OperatorType.OnlyINCondition));
                                filterQuery.Append(")) ");
                                continue;
                            case "StrategyID":
                                filterQuery.Append(" and (g.GroupID in ( Select DISTINCT t.GroupID from t where t.Level2ID  in ");
                                filterQuery.Append(CreateCondtion(key, filterDictionary[key], OperatorType.OnlyINCondition));
                                filterQuery.Append(")) ");
                                continue;

                            case "IsPreAllocated":
                            case "IsManualGroup":
                                filterQuery.Append(CreateCondtion(key, filterDictionary[key], OperatorType.Equals));
                                continue;
                            case "OrderSideTagValue":
                            case "CounterPartyID":
                            case "TradingAccountID":
                            case "VenueID":
                            case "CurrencyID":
                            case "ExchangeID":
                            case "AssetID":
                            case "UnderlyingID":
                                filterQuery.Append(CreateCondtion(key, filterDictionary[key], OperatorType.EqualsMultiple));
                                continue;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return filterQuery.ToString();
        }

        /// <summary>
        /// Generates the query for taxlot i ds.
        /// </summary>
        /// <param name="lstTaxlotIDs">The LST taxlot i ds.</param>
        /// <returns>String.</returns>
        public static String GenerateQueryForTaxlotIDs(List<string> lstTaxlotIDs)
        {
            StringBuilder query = new StringBuilder();
            try
            {
                #region Creating common query

                query.Append("from AllocationGroup g ");//This needs to be changed probably by joining tables
                ///* Extra join to reduce no of queries
                query.Append(" left outer join fetch g.Level1AllocationList l");
                query.Append(" left outer join fetch l.TaxLotsH t ");
                query.Append(" left outer join fetch g.OrdersH o left outer join fetch g.SwapParametersH ");
                query.Append(" left outer join fetch o.ImportFileLogObj i ");
                #endregion
                query.Append(" where t.TaxLotID in (");
                foreach (string taxlotID in lstTaxlotIDs)
                {
                    query.Append("'");
                    query.Append(taxlotID);
                    query.Append("'");
                    query.Append(",");
                }
                query.Remove(query.Length - 1, 1);
                query.Append(") ");
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
            return query.ToString();
        }



        /// <summary>
        /// Creates condition string based on operator type
        /// </summary>
        /// <param name="key">ColumnName</param>
        /// <param name="value">ColumnValue</param>
        /// <param name="operatorType">Condition</param>
        /// <returns>ConditionString in hql</returns>
        private static string CreateCondtion(string key, string value, OperatorType operatorType)
        {
            StringBuilder condition = new StringBuilder();
            try
            {
                if (value != null && value != String.Empty)
                {
                    switch (operatorType)
                    {
                        case OperatorType.Equals:
                            condition.Append(" and g.");
                            condition.Append(key);
                            condition.Append(" = '");
                            condition.Append(value);
                            condition.Append("' ");
                            break;
                        case OperatorType.Like:
                            condition.Append(" and g.");
                            condition.Append(key);
                            condition.Append(" like '%");
                            condition.Append(value);
                            condition.Append("%' ");
                            break;
                        case OperatorType.LikeMultipleOr:
                            List<String> ls = GetListFromCSV(value);
                            condition.Append(" and (");
                            for (int k = 0; k < ls.Count; k++)
                            {
                                if (k != 0)
                                {
                                    condition.Append(" or ");
                                }

                                condition.Append("g.");
                                condition.Append(key);
                                condition.Append(" like '%");
                                condition.Append(ls[k]);
                                condition.Append("%' ");
                            }
                            condition.Append(" )");
                            break;
                        case OperatorType.EqualsMultiple:
                            List<String> lsEquals = GetListFromCSV(value);
                            condition.Append(" and (g.");
                            condition.Append(key);
                            condition.Append(" in (");
                            for (int k = 0; k < lsEquals.Count; k++)
                            {
                                if (k != 0)
                                {
                                    condition.Append(',');
                                }
                                condition.Append("'");
                                condition.Append(lsEquals[k]);
                                condition.Append("'");
                            }
                            condition.Append(") )");
                            break;
                        case OperatorType.OnlyINCondition:
                            List<String> lsINCondition = GetListFromCSV(value);
                            condition.Append(" ( ");
                            for (int k = 0; k < lsINCondition.Count; k++)
                            {
                                if (k != 0)
                                {
                                    condition.Append(",");
                                }
                                condition.Append("'");
                                condition.Append(lsINCondition[k]);
                                condition.Append("'");
                            }
                            condition.Append(" ) ");
                            break;
                    }
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
            return condition.ToString();
        }


        /// <summary>
        /// Convert CSV string to list value operates on String
        /// </summary>
        /// <param name="value">Values in csv format</param>
        /// <returns>List of values</returns>
        private static List<string> GetListFromCSV(string value)
        {
            List<String> ls = new List<String>();
            try
            {
                String[] lsArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lsArray.Length; i++)
                {
                    ls.Add(lsArray[i].Trim());
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
            return ls;
        }

        public static String GenerateFilterQuery(AllocationPrefetchFilter filterList, bool isGetunallocatedGroups)
        {
            string filterQuery = string.Empty;
            try
            {
                Dictionary<String, String> filterAllocated = null;
                Dictionary<String, String> filterUnAllocated = null;

                if (filterList != null)
                {
                    if (filterList.Allocated.Count > 0)
                        filterAllocated = filterList.Allocated;
                    if (filterList.Unallocated.Count > 0)
                        filterUnAllocated = filterList.Unallocated;
                }
                if (isGetunallocatedGroups)
                {
                    filterQuery = GetCommaSeparatedFiltersQuery(filterUnAllocated);
                }
                else
                {
                    filterQuery = GetCommaSeparatedFiltersQuery(filterAllocated);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return filterQuery;
        }

        private static string GetCommaSeparatedFiltersQuery(Dictionary<string, string> filterDictionary)
        {
            StringBuilder filterQuery = new StringBuilder();
            try
            {
                if (filterDictionary != null && filterDictionary.Count > 0)
                {
                    foreach (String key in filterDictionary.Keys)
                    {
                        filterQuery.Append(key);
                        filterQuery.Append(":");
                        filterQuery.Append(filterDictionary[key]);
                        filterQuery.Append("~");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return filterQuery.ToString();
        }
        public static String GenerateSymbolFilterQuery(AllocationPrefetchFilter filterList, bool isGetunallocatedGroups)
        {
            string filterQuery = string.Empty;
            try
            {
                Dictionary<String, String> filterAllocated = null;
                Dictionary<String, String> filterUnAllocated = null;

                if (filterList != null)
                {
                    if (filterList.Allocated.Count > 0)
                        filterAllocated = filterList.Allocated;
                    if (filterList.Unallocated.Count > 0)
                        filterUnAllocated = filterList.Unallocated;
                }
                if (isGetunallocatedGroups && filterUnAllocated != null && filterUnAllocated.ContainsKey("Symbol"))
                {
                    filterQuery = CreateCondtion("Symbol", filterUnAllocated["Symbol"], OperatorType.LikeMultipleOr);
                }
                else if (!isGetunallocatedGroups && filterAllocated != null && filterAllocated.ContainsKey("Symbol"))
                {
                    filterQuery = CreateCondtion("Symbol", filterAllocated["Symbol"], OperatorType.LikeMultipleOr);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return filterQuery;
        }
    }
}
