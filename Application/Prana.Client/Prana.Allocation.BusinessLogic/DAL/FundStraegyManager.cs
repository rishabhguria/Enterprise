using System;
using System.Data;
using System.Data.SqlClient;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.CommonDataCache;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for FundStraegyMAnager.
	/// </summary>
	public class FundStraegyManager
	{
		public FundStraegyManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static FundStrategies    GetFundStrategy()
		{
		
			FundStrategies  fundStrategies= new FundStrategies();
			Database db = DatabaseFactory.CreateDatabase();
		
			try
			{
				
				using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader("P_GetFundStrategy"))
				{
					while(reader.Read())
					{
						object[] row = new object[reader.FieldCount];
						reader.GetValues(row);
						fundStrategies.Add(FillCompanyFundStrategies(row, 0));		
					}
				}
			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw;			
				}
			}				
			#endregion
			return fundStrategies;
		}

		public static FundStrategy  FillCompanyFundStrategies(object[] row, int offSet)
		{


			int StrategyID=0+ offSet;
			int StrategyName=1 +offSet;
			int FundID=2+ offSet;
			int FundName=3 +offSet;
			
			

			


			FundStrategy  fundStrategy= new FundStrategy();
			
			if(row[StrategyID] != null)
			{			
				fundStrategy.StrategyID       = int.Parse(row[StrategyID].ToString());
			}
			if(row[StrategyName] != null)
			{			
				fundStrategy.StrategyName     = row[StrategyName].ToString();
			}
			if(row[FundID] != null)
			{			
				fundStrategy.FundID        = int.Parse(row[FundID].ToString());
			}
			if(row[FundName] != null)
			{			
				fundStrategy.FundName     = row[FundName].ToString();
			}
			return fundStrategy;
		
		
		}
        //SP_OBSOLETE: func SaveFundStrategy SP: P_DeleteFundStrategies,P_SaveFundStrategies
		public static void SaveFundStrategy(FundStrategies  _fundStrategies)
		{
			Database db = DatabaseFactory.CreateDatabase();

			object[] parameter = new object[2];
			
			
				
			#region try

			try
			{
                db.ExecuteScalar("P_DeleteFundStrategies");
				foreach(FundStrategy fundStrategy in _fundStrategies)		
				{
					
						parameter[0] = fundStrategy.FundID ;
						parameter[1] = fundStrategy.StrategyID ;					
						db.ExecuteScalar("P_SaveFundStrategies", parameter);
					
				}					
			}
				# endregion

				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


				if (rethrow)
				{
					throw;			
				}
			}				
			#endregion
	

		}


        #region Funds and Strategies Retrieval for Allocated Orders
        /// <summary>
        /// Gets All Allocated Funds and Adds them to Already Received Groups
        /// </summary>
        /// <param name="currentTime"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static AllocationFunds GetAllocatedFunds(string AllAUECDatesString)
        {
            AllocationFunds funds = new AllocationFunds();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = AllAUECDatesString;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetAllocatedFunds", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        AllocationFund fund = FillFund(row, 0);
                        funds.Add(fund);

                    }
                }


            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
            return funds;
        }
        //SP_OBSOLETE func: GetBasketAllocatedGroups Sp: P_BTGetAllocatedGroupByBasketGroupID
        public static AllocationGroups GetBasketAllocatedGroups(string basketGroupID)
        {
            AllocationGroups groups = new AllocationGroups();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = basketGroupID;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetAllocatedGroupByBasketGroupID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string groupID =row[0].ToString();
                        double allocatedQty = double.Parse(row[1].ToString());
                        AllocationGroup group = new AllocationGroup();
                        group.AllocatedQty = allocatedQty;
                        group.GroupID = groupID;
                        groups.Add(group);
                    }
                }
              AllocationFunds funds = GetBasketGroupAllocatedFunds(basketGroupID);
              foreach (AllocationFund fund in funds)
              {
                  if (groups.GetGroup(fund.GroupID).AllocationFunds == null)
                  {
                      groups.GetGroup(fund.GroupID).AllocationFunds = new AllocationFunds();
                  }
                  groups.GetGroup(fund.GroupID).AllocationFunds.Add(fund);
              }

            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
            return groups;
        }
        //SP_OBSOLETE func: GetBasketGroupAllocatedFunds sp:P_GetAllocatedFundsByGroupID
        private static AllocationFunds GetBasketGroupAllocatedFunds(string basketGroupID)
        {
            AllocationFunds funds = new AllocationFunds();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = basketGroupID;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetAllocatedFundsByGroupID", parameter))
                {
                    while (reader.Read())
                    {
                        AllocationGroup group = new AllocationGroup();
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        AllocationFund fund = FillFund(row, 0);
                        funds.Add(fund);
                    }
                }


            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
            return funds;
        }
        public static AllocationStrategies GetAllocatedStrategies(string AllAUECDatesString)
        {
            AllocationStrategies strategies = new AllocationStrategies();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = AllAUECDatesString;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetAllocatedStrategies", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        AllocationStrategy strategy = FillStrategy(row, 0);
                        strategies.Add(strategy);

                    }
                }


            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
            return strategies;
        }
        public static AllocationFunds GetBasketAllocatedFunds(string AllAUECDatesString)
        {
            AllocationFunds funds = new AllocationFunds();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = AllAUECDatesString;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetAllocatedFunds", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        AllocationFund fund = FillFund(row, 0);
                        funds.Add(fund);

                    }
                }


            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
            return funds;
        }
        public static AllocationStrategies GetBasketAllocatedStrategies(string AllAUECDatesString)
        {
            AllocationStrategies strategies = new AllocationStrategies();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = AllAUECDatesString;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetAllocatedStrategies", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        AllocationStrategy strategy = FillStrategy(row, 0);
                        strategies.Add(strategy);

                    }
                }


            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
            return strategies;
        }
        public static AllocationFund FillFund(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            AllocationFund fund = new AllocationFund();
            try
            {
                int GroupID = offset + 0;
                int FundID = offset + 1;

                int AllocatedQty = offset + 2;
                int Percentage = offset + 3;
                int Commission = offset+5;
                int Fees = offset+6;
                if (row != null)
                {
                    fund.GroupID = row[GroupID].ToString();
                    fund.FundID = int.Parse(row[FundID].ToString());
                    fund.Percentage = float.Parse(row[Percentage].ToString());
                    fund.AllocatedQty = double.Parse(row[AllocatedQty].ToString());
                    fund.FundName = CachedDataManager.GetInstance.GetFundText(fund.FundID);
                    if (row.Length > 5)
                    {
                        if (!row[Commission].Equals(System.DBNull.Value))
                        {
                            fund.Commission = double.Parse(row[Commission].ToString());
                        }
                        else
                        {
                            fund.Commission = double.MinValue;
                        }
                        if (!row[Fees].Equals(System.DBNull.Value))
                        {
                            fund.Fees = double.Parse(row[Fees].ToString());
                        }
                        //else
                        //{
                        //    fund.Fees = double.MinValue;
                        //}
                        
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
            }
            return fund;

        }
        public static AllocationStrategy FillStrategy(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            AllocationStrategy strategy = new AllocationStrategy();
            try
            {
                int GroupID = offset + 0;
                int StrategyID = offset + 1;
                int AllocatedQty = offset + 2;
                int Percentage = offset + 3;


                if (row != null)
                {
                    strategy.GroupID = row[GroupID].ToString();
                    strategy.StrategyID = int.Parse(row[StrategyID].ToString());
                    strategy.Percentage = float.Parse(row[Percentage].ToString());
                    strategy.AllocatedQty = double.Parse(row[AllocatedQty].ToString());


                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
            }
            return strategy;

        }
        #endregion

        
        public static void ProRataFunds(AllocationFunds funds ,double allocatedQty)
        {
            SetFundsAllocationQty(funds,allocatedQty);
        }
        public static void SetFundsAllocationQty(AllocationFunds funds,double allocatedQty)
        {
            int length = funds.Count;
            int count = 1;
            double sumofQty = 0;
            foreach (AllocationFund fund in funds)
            {
                if (count == length)
                {
                    fund.AllocatedQty = allocatedQty - sumofQty;
                }
                else
                {

                    fund.AllocatedQty = Convert.ToInt64((allocatedQty * fund.Percentage) / 100);
                    sumofQty += fund.AllocatedQty;
                }
                count++;
            }
        }

        public static void ProRataStrategies(AllocationStrategies  strategies, double allocatedQty)
        {
            SetStrategiesAllocationQty(strategies, allocatedQty);
        }
        public static void SetStrategiesAllocationQty(AllocationStrategies strategies, double allocatedQty)
        {
            int length = strategies.Count;
            int count = 1;
            double sumofQty = 0;
            foreach (AllocationStrategy strategy in strategies)
            {
                if (count == length)
                {
                    strategy.AllocatedQty = allocatedQty - sumofQty;
                }
                else
                {
                    strategy.AllocatedQty = Convert.ToInt64((allocatedQty * strategy.Percentage) / 100);
                    sumofQty += strategy.AllocatedQty;
                }
                count++;
            }
        }


	}
}
