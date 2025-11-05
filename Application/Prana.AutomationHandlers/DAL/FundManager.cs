//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data.SqlClient;
//using Prana.Global;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using System.Data;

//namespace Prana.AutomationHandlers
//{
//    class FundManager
//    {
//        public string GetFundText(int ID)
//        {
//            if (fundsKeyValueCollection.ContainsKey(ID))
//            {
//                return fundsKeyValueCollection[ID];
//            }
//            else
//            {
//                return "";
//            }
//        }
//        public int GetFundID(string fundName)
//        {
//            foreach (KeyValuePair<int, string> kvp in fundsKeyValueCollection)
//            {
//                if (string.Compare(kvp.Value, fundName, true) == 0)
//                {
//                    return kvp.Key;
//                }
//            }
//            return int.MinValue;
//        }

//        System.Collections.Generic.Dictionary<int, string> fundsKeyValueCollection = new Dictionary<int, string>();

//        public System.Collections.Generic.Dictionary<int, string> GetFunds()
//        {
//            fundsKeyValueCollection = new Dictionary<int, string>();
//            Prana.BusinessObjects.FundCollection funds = GetFunds();

//            try
//            {
//                foreach (Prana.BusinessObjects.Fund fund in funds)
//                {
//                    fundsKeyValueCollection.Add(fund.FundID, fund.Name);
//                }

//            }
//            #region Catch
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//            #endregion
//            return fundsKeyValueCollection;


//        }

//        public static Prana.BusinessObjects.FundCollection GetFunds()
//        {
//            //Here we are getting the FUnds corresponding to the User that logs into Prana Client machine

//            Prana.BusinessObjects.FundCollection funds = new Prana.BusinessObjects.FundCollection();

//            Database db = DatabaseFactory.CreateDatabase();
//            try
//            {

//                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetCompanyFundsC"))
//                {
//                    while (reader.Read())
//                    {
//                        object[] row = new object[reader.FieldCount];
//                        reader.GetValues(row);
//                        funds.Add(FillFund(row, 0));
//                    }
//                }
//            }
//            #region Catch
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//            #endregion
//            return funds;
//        }
//    }
    
//}
