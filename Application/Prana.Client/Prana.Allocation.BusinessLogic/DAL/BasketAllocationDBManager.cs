using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using System.Data.Common;
using System.Data;
using Prana.Utilities.XMLUtilities;
namespace Prana.Allocation.BLL

{
    public class BasketAllocationDBManager
    {
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;
        public static void SaveAllocatedBasket(BasketGroup basketGroup)
        {

            #region try
            try
            {
                Database db = DatabaseFactory.CreateDatabase();

                object[] parameter = new object[4];


                parameter[0] = basketGroup.BasketGroupID;
                parameter[1] = basketGroup.GroupState;
                parameter[2] = basketGroup.AllocatedQty;
                if (!basketGroup.AUECLocalDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    parameter[3] = basketGroup.AUECLocalDate;
                }
                else
                {
                    parameter[3] = TimeZoneHelper.GetAUECLocalDateFromUTC(basketGroup.AUECID, DateTime.UtcNow);
                }
                db.ExecuteScalar("P_BTUpdateBasketGroup", parameter);


                if (basketGroup.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                {
                    SaveAllocatedFunds(basketGroup);
                }
                else
                {
                    SaveAllocatedStrategies(basketGroup);
                }


            }
            # endregion

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
        }
        public static void RemoveAllocatedBasket(BasketGroup basketGroup)
        {

            #region try
            try
            {
                Database db = DatabaseFactory.CreateDatabase();

               
                if (basketGroup.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                {
                    DeleteBasketAllocatedFund(basketGroup);
                }
                else
                {
                    DeleteBasketAllocatedSrategy(basketGroup);
                }
                if (basketGroup.AddedBaskets.Count == 1)
                {
                    DeleteBasketGroup(basketGroup);
                }
                else
                {
                    object[] parameter = new object[4];
                    parameter[0] = basketGroup.BasketGroupID;
                    parameter[1] = basketGroup.GroupState;
                    parameter[2] = basketGroup.AllocatedQty;
                    parameter[3] = basketGroup.AUECLocalDate;

                    db.ExecuteScalar("P_BTUpdateBasketGroup", parameter);
                }
            }
            # endregion

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
        }
        public static void SaveBasketGroup(BasketGroup basketGroup, DateTime currentDate)
        {
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameterBasketGroup = new object[15];
            parameterBasketGroup[0] = basketGroup.BasketGroupID;
            if (!currentDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
            {
                 parameterBasketGroup[1] = currentDate;
            }
            else
            {
                 parameterBasketGroup[1] = DateTime.UtcNow;
            }
           
            parameterBasketGroup[2] = (int)basketGroup.AllocationType;
            parameterBasketGroup[3] = basketGroup.UserID;
            parameterBasketGroup[4] = basketGroup.AssetID;
            parameterBasketGroup[5] = basketGroup.UnderLyingID ;
            parameterBasketGroup[6] = basketGroup.AllocatedQty;
            parameterBasketGroup[7] = basketGroup.CumQty;
            parameterBasketGroup[8] = basketGroup.Quantity;
            parameterBasketGroup[9] = basketGroup.AUECID;
            parameterBasketGroup[10] = basketGroup.ListID;
            if (!currentDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
            {
                parameterBasketGroup[11] = currentDate;
            }
            else
            {
                parameterBasketGroup[11] = DateTime.UtcNow;
            }
            parameterBasketGroup[12] = basketGroup.GroupState;
            parameterBasketGroup[13] = basketGroup.TradingAccountID;
            if (!basketGroup.AUECLocalDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
            {
                parameterBasketGroup[14] = basketGroup.AUECLocalDate;
            }
            else
            {
                parameterBasketGroup[14] = TimeZoneHelper.GetAUECLocalDateFromUTC(basketGroup.AUECID, DateTime.UtcNow);
            }
            //parameterBasketGroup[14] = TimeZoneHelper.GetAUECLocalDateFromUTC(basketGroup.AUECID, DateTime.UtcNow);

            #region try
            try
            {

                db.ExecuteScalar("P_BTSaveBasketGroup", parameterBasketGroup);
                
                foreach (Prana.BusinessObjects.BasketDetail basket in basketGroup.AddedBaskets)
                {
                    object[] parameterBasket = new object[2];
                    parameterBasket[0] = basketGroup.BasketGroupID;
                    parameterBasket[1] = basket.TradedBasketID;
                    db.ExecuteScalar("BT_SaveGroupsBaskets", parameterBasket);
                    
                }

            }
            # endregion

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
        }
        private  static void SaveAllocatedFunds(BasketGroup basketGroup)
        {
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameter = new object[4];
            
                
                parameter[0] = basketGroup.BasketGroupID;
                foreach (AllocationFund fund in basketGroup.AllocationFunds)
                {
                    parameter[1] = fund.FundID;
                    parameter[2] = fund.AllocatedQty;
                    parameter[3] = fund.Percentage;

                    #region try
                    try
                    {
                        db.ExecuteScalar("P_BTSaveAllocatedFunds", parameter);

                    }
                    # endregion

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
                }
        }
        public static void SaveAllocatedStrategies(BasketGroup basketGroup)
        {
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameter = new object[4];
            
                parameter[0] = basketGroup.BasketGroupID ;
                foreach (AllocationStrategy companyStrategy in basketGroup.Strategies)
                {
                    parameter[1] = companyStrategy.StrategyID;

                    parameter[2] = companyStrategy.AllocatedQty;
                    parameter[3] = companyStrategy.Percentage;

                    #region try
                    try
                    {

                        db.ExecuteScalar("P_BTSaveAllocatedStrategies", parameter);

                    }
                    # endregion

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
                }

        }
        public static BasketCollection GetUnAllocatedBasketDetails(int userID, DateTime date, int allocationType, Int64 lastID)
        {
            BasketCollection basketCollection = new BasketCollection();


            Database db = DatabaseFactory.CreateDatabase();

            try
            {

                object[] parameter = new object[4];

                parameter[0] = userID;
                parameter[1] = date;
                parameter[2] = lastID;
                parameter[3] = allocationType;

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("BT_GetUnallocatedBasketDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        BasketDetail tradedBasket = BasketDataManager.GetUnallocatedBasket(row[0].ToString(), allocationType);
                        basketCollection.Add(tradedBasket);
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
            return basketCollection;
        }
        /// <summary>
        /// Only for getting Basket Details  , it does not contain Basket Orders (used for update of basket)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="date"></param>
        /// <param name="lastID"></param>
        /// <param name="allocationType"></param>
        /// <returns></returns>
        public static BasketGroupCollection GetBasketGroups(string AllAUECDatesString, int typeOfAllocation)
        {
            BasketGroupCollection basketGroupCollection = new BasketGroupCollection();
            object[] parameter = new object[2];
            
            parameter[0] = AllAUECDatesString;
            parameter[1] = typeOfAllocation;
            
            #region try
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetBasketGroups", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        BasketGroup basketGroup = FillBasketGroup(row,0);
                        basketGroup.AddBaskets(GetGroupedBaskets(basketGroup.BasketGroupID, typeOfAllocation, AllAUECDatesString));
                        basketGroupCollection.Add(basketGroup);
                    }
                }
            }
            # endregion

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
            return basketGroupCollection;

        }
        public static void DeleteBasketGroup(BasketGroup basketGroup)
        { 
            
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameterBasketGroup = new object[1];
            parameterBasketGroup[0] = basketGroup.BasketGroupID;
            

            #region try
            try
            {
                db.ExecuteScalar("BT_DeleteBasketGroup", parameterBasketGroup);
            }
            # endregion

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
        }
        private static BasketGroup FillBasketGroup( object[] row,int offset)
        {
            BasketGroup basketGroup= new BasketGroup();
            int BasketGroupID = offset + 0;
            int ListID = offset + 1;
            int TradingAccountID = offset + 2;
            int AllocatedQty = offset + 3;
            int CumQty = offset + 4;
            int Quantity = offset + 5;
            int AUECID = offset + 6;
            int UserID = offset + 7;
            int AssetID = offset + 8;
            int UnderLyingID = offset + 9;
            int AllocationType = offset + 10;
            int StateID = offset + 11;
              int CounterPartyID = offset + 12;
              int VenueID = offset + 13;
              int AUECLocaldate = offset + 14;

            basketGroup.BasketGroupID = row[BasketGroupID].ToString();
            basketGroup.ListID = row[ListID].ToString();
            basketGroup.TradingAccountID = int.Parse(row[TradingAccountID].ToString());
            basketGroup.AllocatedQty = int.Parse(row[AllocatedQty].ToString());
            basketGroup.CumQty = Int64.Parse(row[CumQty].ToString());
            basketGroup.Quantity = double.Parse(row[Quantity].ToString());
            if (!row[AUECID].Equals(System.DBNull.Value))
            {
                basketGroup.AUECID = int.Parse(row[AUECID].ToString());
            }
            basketGroup.UserID = int.Parse(row[UserID].ToString());
            basketGroup.AssetID = int.Parse(row[AssetID].ToString());
            basketGroup.UnderLyingID = int.Parse(row[UnderLyingID].ToString());
            int allocationType=int.Parse(row[AllocationType].ToString());
            int stateID = int.Parse(row[StateID].ToString());

            basketGroup.GroupState = (PranaInternalConstants.ORDERSTATE_ALLOCATION)stateID;

            if (allocationType == (int)PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
            {
                basketGroup.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
               // basketGroup.AllocationFunds = FundStraegyManager.GetBasketAllocatedFunds(basketGroup.BasketGroupID, true);
            }
            else
            {
                basketGroup.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY;
              //  basketGroup.Strategies = FundStraegyManager.GetBasketAllocatedStrategies(basketGroup.BasketGroupID, true);
            }
            if (!row[CounterPartyID].Equals(System.DBNull.Value))
            {
                basketGroup.CounterPartyID = int.Parse(row[CounterPartyID].ToString());
            }
            if (!row[VenueID].Equals(System.DBNull.Value))
            {
                basketGroup.VenueID = int.Parse(row[VenueID].ToString());
            }
            if (!row[AUECLocaldate].Equals(System.DBNull.Value))
            {
                basketGroup.AUECLocalDate = DateTime.Parse(row[AUECLocaldate].ToString());
            }
            return basketGroup;

        }
        private static Prana.BusinessObjects.BasketCollection GetGroupedBaskets(string basketGroupID,int allocationTypeID, string AllAUECDatesString)
        {

            BasketCollection basketCollection = new BasketCollection();
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameter = new object[3];
            parameter[0] = basketGroupID;
            parameter[1] = allocationTypeID ;
            parameter[2] = AllAUECDatesString;
            try
            {

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetGroupedBaskets", parameter))
                {
                    while (reader.Read())
                    {
                       
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        BasketDetail basket = BasketDataManager.GetUnallocatedBasket(row[0].ToString().Trim(), allocationTypeID);
                        basketCollection.Add(basket);
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
            return basketCollection;
        }
        public static void DeleteBasketAllocatedFund(BasketGroup basketGroup)
        {

            Database db = DatabaseFactory.CreateDatabase();

            object[] parameterBasketGroup = new object[1];
            parameterBasketGroup[0] = basketGroup.BasketGroupID;


            #region try
            try
            {
                db.ExecuteScalar("P_BTDeleteAllocatedFund", parameterBasketGroup);
            }
            # endregion

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
        }
        public static void DeleteBasketAllocatedSrategy(BasketGroup basketGroup)
        {

            Database db = DatabaseFactory.CreateDatabase();

            object[] parameterBasketGroup = new object[1];
            parameterBasketGroup[0] = basketGroup.BasketGroupID;


            #region try
            try
            {
                db.ExecuteScalar("P_BTDeleteAllocatedStrategy", parameterBasketGroup);
            }
            # endregion

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
        }
        //SP_Obsolete: Func:ProRataBasketGroup Sp:P_BTProrataBasketAllocation
        public static void ProRataBasketGroup(BasketGroup basketGroup)
        {

            Database db = DatabaseFactory.CreateDatabase();

            object[] parameterBasketGroup = new object[1];
            parameterBasketGroup[0] = basketGroup.BasketGroupID;
            #region try
            try
            {
                db.ExecuteScalar("P_BTProrataBasketAllocation", parameterBasketGroup);
            }
            # endregion

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
        }
        public static int SaveAndUpdateCommissionandFeesForBasket(AllocationOrderCollection allocationOrderCollection)
        {
            int rowsAffected = 0;

            string commissionandFeesXML = XMLUtilities.SerializeToXML(allocationOrderCollection);
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cmd = new SqlCommand();
                cmd.CommandText = "AL_SaveCommissionandFeesForBasket";
                cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(cmd, "@Xml", DbType.String, commissionandFeesXML);

                XMLSaveManager.AddOutErrorParameters(db, cmd);

                rowsAffected = db.ExecuteNonQuery(cmd);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        public static AllocationFunds GetCommissionsAndFeesFromDBForBasket(string AllAUECDatesString)
        {
            Database db = DatabaseFactory.CreateDatabase();
            AllocationFunds commissionsAndFees = new AllocationFunds();
            try
            {
                AllocationFund commissionAndFee = new AllocationFund();
                object[] parameter = new object[1];

                parameter[0] = AllAUECDatesString;

                //Int64.MaxValue;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("AL_GetAllCommissionFromDbForBasket",parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionsAndFees.Add(FillcommissionAndFees(row, 0));
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
            return commissionsAndFees;
        }
        private static AllocationFund FillcommissionAndFees(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            AllocationFund commissionAndFee = new AllocationFund();

            if (row != null)
            {
                int ClorderID = offset + 0;
                int FundId = offset + 1;
                int Commission = offset + 2;
                int Fees = offset + 3;
                try
                {
                    commissionAndFee.GroupID = row[ClorderID].ToString();
                    commissionAndFee.FundID = int.Parse(row[FundId].ToString());
                    commissionAndFee.Commission = double.Parse(row[Commission].ToString());
                    commissionAndFee.Fees = double.Parse(row[Fees].ToString());
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
            return commissionAndFee;
        }
        public static BasketGroup GetBasketAllocation(string basketID, int typeOfAllocation)
        {
            Database db = DatabaseFactory.CreateDatabase();
            BasketGroup basketGroup = new BasketGroup();
            try
            {
                
                object[] parameter = new object[2];
                parameter[0] = basketID;
                parameter[1] = typeOfAllocation;

                //Int64.MaxValue;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_CheckBasketAllocation", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int GroupID = 0;
                            int date = 1;
                            basketGroup.GroupState = (PranaInternalConstants.ORDERSTATE_ALLOCATION)int.Parse(row[GroupID].ToString());
                            basketGroup.AUECLocalDate = DateTime.Parse(row[date].ToString());
                        }
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
            return basketGroup;
        }
    }
}
