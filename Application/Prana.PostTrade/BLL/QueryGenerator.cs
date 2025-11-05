using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.PostTrade.BLL
{
    public class QueryGenerator
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
        /// Genarates query for NHibernate based on filter conditions passed
        /// </summary>
        /// <param name="ToAllAUECDatesString">End date</param>
        /// <param name="FromAllAUECDatesString">Starting Date</param>
        /// <param name="filterList">List of filters to be applied on data</param>
        /// <returns>Query in string format to be executed by NHibernate session</returns>
        public static String GenerateQuery(string ToAllAUECDatesString, string FromAllAUECDatesString, Dictionary<String, Dictionary<String, String>> filterList)
        {
            StringBuilder query = new StringBuilder();
            try
            {
                Dictionary<String, String> filterAllocated = null;
                Dictionary<String, String> filterUnAllocated = null;

                if (filterList != null)
                {
                    if (filterList.ContainsKey("Allocated"))
                        filterAllocated = filterList["Allocated"];
                    if (filterList.ContainsKey("UnAllocated"))
                        filterUnAllocated = filterList["UnAllocated"];
                }
                #region Creating common query

                query.Append("from AllocationGroup g ");//This needs to be changed probably by joining tables
                ///* Extra join to reduce no of queries
                query.Append(" left outer join fetch g.Level1AllocationList l");
                query.Append(" left outer join fetch l.TaxLotsH t ");
                query.Append(" left outer join fetch g.OrdersH o left outer join fetch g.SwapParametersH ");
                query.Append(" left outer join fetch o.ImportFileLogObj i ");
                #endregion

                //Creaating query based on filter conditions
                #region Creating query for unAllocated data
                StringBuilder unAllocatedQuery = new StringBuilder();
                unAllocatedQuery.Append("g.StateID=1 and DATEDIFF(d,g.AllocationDate,'");
                unAllocatedQuery.Append(ToAllAUECDatesString);
                unAllocatedQuery.Append("'");
                unAllocatedQuery.Append(")>=0 and g.CumQty>0");
                if (filterUnAllocated != null && filterUnAllocated.Count > 0)
                {
                    foreach (String key in filterUnAllocated.Keys)
                    {
                        switch (key)
                        {
                            case "Symbol":
                                unAllocatedQuery.Append(CreateCondtion(key, filterUnAllocated[key], OperatorType.LikeMultipleOr));
                                continue;
                            case "IsPreAllocated":
                            case "IsManualGroup":
                                unAllocatedQuery.Append(CreateCondtion(key, filterUnAllocated[key], OperatorType.Equals));
                                continue;
                            case "OrderSideTagValue":
                            case "CounterPartyID":
                            case "TradingAccountID":
                            case "VenueID":
                            case "CurrencyID":
                            case "ExchangeID":
                            case "AssetID":
                            case "UnderlyingID":
                                unAllocatedQuery.Append(CreateCondtion(key, filterUnAllocated[key], OperatorType.EqualsMultiple));
                                continue;
                        }
                    }
                }
                #endregion

                //Creaating query based on filter conditions
                #region Creating query for allocated data
                StringBuilder allocatedQuery = new StringBuilder();
                allocatedQuery.Append(" g.StateID=2 and DATEDIFF(d,g.AllocationDate,'");
                allocatedQuery.Append(ToAllAUECDatesString);
                allocatedQuery.Append("')>=0 and DATEDIFF(d,g.AllocationDate,'");
                allocatedQuery.Append(FromAllAUECDatesString);
                allocatedQuery.Append("')<=0 and g.CumQty>0 ");
                if (filterAllocated != null && filterAllocated.Count > 0)
                {
                    foreach (String key in filterAllocated.Keys)
                    {
                        switch (key)
                        {
                            case "Symbol":
                                allocatedQuery.Append(CreateCondtion(key, filterAllocated[key], OperatorType.LikeMultipleOr));
                                continue;
                            case "FundID":
                                allocatedQuery.Append(" and (g.GroupID in ( Select DISTINCT l.GroupID from l where l.FundID in ");
                                allocatedQuery.Append(CreateCondtion(key, filterAllocated[key], OperatorType.OnlyINCondition));
                                allocatedQuery.Append(")) ");
                                continue;
                            case "StrategyID":
                                allocatedQuery.Append(" and (g.GroupID in ( Select DISTINCT t.GroupID from t where t.Level2ID  in ");
                                allocatedQuery.Append(CreateCondtion(key, filterAllocated[key], OperatorType.OnlyINCondition));
                                allocatedQuery.Append(")) ");
                                continue;

                            case "IsPreAllocated":
                            case "IsManualGroup":
                                allocatedQuery.Append(CreateCondtion(key, filterAllocated[key], OperatorType.Equals));
                                continue;
                            case "OrderSideTagValue":
                            case "CounterPartyID":
                            case "TradingAccountID":
                            case "VenueID":
                            case "CurrencyID":
                            case "ExchangeID":
                            case "AssetID":
                            case "UnderlyingID":
                                allocatedQuery.Append(CreateCondtion(key, filterAllocated[key], OperatorType.EqualsMultiple));
                                continue;

                        }
                    }
                }
                #endregion


                query.Append(" where ((");
                query.Append(unAllocatedQuery.ToString());
                query.Append(" ) ");
                query.Append(" or ");
                query.Append(" ( ");
                query.Append(allocatedQuery.ToString());
                query.Append(" ) )");
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
    }
}
