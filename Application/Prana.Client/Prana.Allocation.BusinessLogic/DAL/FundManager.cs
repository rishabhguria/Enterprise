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
	/// Summary description for FundManager.
	/// </summary>
	public class FundManager
	{
		public FundManager()
		{
			//
			// TODO: Add constructor logic here
			//
        }
        public static AllocationGroups GetGroups(int fundID)
		{
			AllocationGroups groups= new AllocationGroups();			
			return groups;
		
		}
		public static Defaults  GetFundDefaults(int UserID)
		{
			Defaults  defaults=  new Defaults();

			Database db = DatabaseFactory.CreateDatabase();
			Object[] parameter = new object[1];
			parameter[0] = UserID;
			try
			{
				using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader("P_GetUserFundDefaults", parameter))
				{
					while(reader.Read())
					{
						object[] row = new object[reader.FieldCount];
						reader.GetValues(row);
						defaults.Add(FillUserFundDefaults(row, 0));		
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
			return defaults;
		}
		public static Default  FillUserFundDefaults(object[] row, int offSet)
		{
			
			int DefaultID=0+ offSet;
			int DefaultName=1 +offSet;
			int FundIDs=2 +offSet;
			int Percentages=3 +offSet;

			//int 


			Default defaultfund = new Default();
			
			if(row[DefaultName] != null)
			{			
				defaultfund.DefaultName      = row[DefaultName].ToString();
			}
			if(row[DefaultID] != null)
			{			
				defaultfund.DefaultID    = row[DefaultID].ToString();
			}
			if(row[FundIDs] != null)
			{			
				defaultfund.FundIDs     = row[FundIDs].ToString();
			}
			if(row[Percentages] != null)
			{			
				defaultfund.Percentages     = row[Percentages].ToString();
			}
			
			return defaultfund;
		}
        //SP_OBSOLETE func: GetFundsByEntityID SP:BT_GetFundsByBasketGroupID,P_GetFundsByEntityID
        public static AllocationFunds GetFundsByEntityID(string entityID,bool basketGroup)
        {
            AllocationFunds funds = new AllocationFunds();
            Object[] parameter = new object[1];
            parameter[0] = entityID;
            try
            {
                string spName = string.Empty;
                if (basketGroup)
                    spName = "BT_GetFundsByBasketGroupID";
                else
                   spName = "P_GetFundsByEntityID";
                Database db = DatabaseFactory.CreateDatabase();
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(spName, parameter))
                {
                    while (reader.Read())
                    {
                        AllocationFund fund = new AllocationFund();

                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        fund.FundID = int.Parse(row[0].ToString());
                        fund.FundName = CachedDataManager.GetInstance.GetFundText(fund.FundID);
                        fund.AllocatedQty = Convert.ToInt64(row[1]);
                        fund.Percentage = float.Parse(row[2].ToString());
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
		public static void SaveDefaults(int UserID,Defaults defaults)
		{
		
			Database db = DatabaseFactory.CreateDatabase();

			object[] parameter = new object[5];
			foreach(Default default1 in defaults)
			{


				#region try
				try
				{
					parameter[0] = default1.DefaultID ;
					parameter[1] = UserID ;
					parameter[2] = default1.DefaultName  ;
					parameter[3] = default1.FundIDs  ;
					parameter[4]= default1.Percentages ;
					db.ExecuteScalar("P_SaveFundDefaults", parameter);

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
		
		}
		public static void DeleteDefaults(int UserID)
		{
			
		//	string[] IDS=defaultIDS.Split(',');
			
			Database db = DatabaseFactory.CreateDatabase();
			//foreach(string ID in IDS)
		//	{
				object[] parameter = new object[1];
				
				#region try
				try
				{
					
					
					
					parameter[0] = UserID;
				
				
					db.ExecuteScalar("P_DeleteFundDefault", parameter);

					

					
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
	

		//	}
			


			

		
		}
	}
}
