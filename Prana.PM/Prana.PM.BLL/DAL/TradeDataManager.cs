using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.DAL
{
    public class TradeDataManager
    {


        #region Get CashAccounts & Strategies

        static Dictionary<int, Prana.BusinessObjects.PositionManagement.Account> _accountsLookup = new Dictionary<int, Prana.BusinessObjects.PositionManagement.Account>();

        public static Prana.BusinessObjects.PositionManagement.Account GetAccount(int accountID)
        {
            Prana.BusinessObjects.PositionManagement.Account account = null;
            try
            {
                if (!_accountsLookup.ContainsKey(accountID))
                {
                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_GetAccount";
                    queryData.DictionaryDatabaseParameter.Add("@accountID", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@accountID",
                        ParameterType = DbType.Int32,
                        ParameterValue = accountID
                    });

                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);

                            account = new Prana.BusinessObjects.PositionManagement.Account();
                            account.ID = int.Parse(row[0].ToString());
                            account.FullName = row[1].ToString();
                            account.ShortName = row[2].ToString();
                            _accountsLookup.Add(accountID, account);
                        }
                    }
                }
                else
                {
                    account = _accountsLookup[accountID];
                }


            }
            #region catch

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                #endregion
            }

            if (account == null)
            {
                account = new Prana.BusinessObjects.PositionManagement.Account(-1, "Un-allocated", "Un-allocated");

            }
            return account;

        }

        static Dictionary<int, Prana.BusinessObjects.PositionManagement.Strategy> _strategyLookup = new Dictionary<int, Prana.BusinessObjects.PositionManagement.Strategy>();

        public static Prana.BusinessObjects.PositionManagement.Strategy GetStrategy(int strategyID)
        {
            Prana.BusinessObjects.PositionManagement.Strategy strategy = null;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetStrategy";
                queryData.DictionaryDatabaseParameter.Add("@strategyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@strategyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = strategyID
                });

                if (!_strategyLookup.ContainsKey(strategyID))
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);

                            strategy = new Prana.BusinessObjects.PositionManagement.Strategy();

                            strategy.StrategyID = int.Parse(row[0].ToString());
                            strategy.Name = row[1].ToString();
                            strategy.ShortName = row[2].ToString();

                            _strategyLookup.Add(strategyID, strategy);



                        }
                    }
                }

            }
            #region catch

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                #endregion
            }

            return strategy;

        }
        #endregion
    }
}