using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Utilities.XMLUtilities;
using System.ComponentModel;
using System.Data.Common;
using Prana.ClientCommon;
using System.Collections.Generic;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities.DateTimeUtilities;
namespace Prana.Allocation.BLL
{
   public  class OrderAllocationDBManager
    {
      
       private static string _groupID= string.Empty;
       private static string _preAllocatedGroupID = string.Empty;
       static int _userID = int.MinValue;
       private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;

       #region DB Related Activites of  a Group (Allocated and UnAllocated)
       /// <summary>
       /// Save All groups either allocated or unallocated
       /// </summary>
       /// <param name="group"></param>
       public static void SaveGroup(AllocationGroup group, DateTime auecLocaldate)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               AllocationOrder order = new AllocationOrder();
                object[] parameter = new object[30];
               parameter[0] = group.GroupID;
               parameter[1] = group.IsProrataActive;
               parameter[2] = group.ListID;
               parameter[3] = group.OrderSideTagValue;
               parameter[4] = group.OrderTypeTagValue;
               parameter[5] = group.Quantity;
               parameter[6] = group.SingleOrderAllocation;
               parameter[7] = (int)group.State;
               parameter[8] = group.Symbol;
               parameter[9] = group.TradingAccountID;
               parameter[10] = group.UnderLyingID;
               parameter[11] = group.VenueID;
               parameter[12] = group.AllocatedQty;
               parameter[13] = (int)group.AllocationType;
               parameter[14] = group.AssetID;
               parameter[15] = group.AUECID;
               parameter[16] = group.AutoGrouped;
               parameter[17] = group.CounterPartyID;
               parameter[18] = group.CumQty;
               parameter[19] = group.CurrencyID;
               parameter[20] = group.ExchangeID;
               parameter[21] = group.AvgPrice;
               parameter[22] = group.IsPreAllocated;
               parameter[23] = _userID;
               parameter[24] = DateTime.Now.ToUniversalTime();
               parameter[25] =group.IsBasketGroup;
               parameter[26] = group.BasketGroupID;
               if (!auecLocaldate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
               {
                   parameter[27] = auecLocaldate;//TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
               }
               else
               {
                   parameter[27] = TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
               } 
              
               parameter[28] = group.SettlementDate;
               parameter[29] = group.ExpirationDate;
               db.ExecuteScalar("P_SaveGroup", parameter);
               SaveGroupOrders(group);
               


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
       }
       /// <summary>
       /// remove group when unalloacte or ungroup 
       /// </summary>
       /// <param name="group"></param>
       public static void RemoveGroup(AllocationGroup group)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               object[] parameter = new object[2];

               parameter[0] = group.GroupID;
               parameter[1] = (int)group.AllocationType;
               db.ExecuteScalar("P_RemoveGroup", parameter);
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

       }
       /// <summary>
       /// remove basket groups Allocated or unallocated
       /// </summary>
       /// <param name="group"></param>
       public static void RemoveBasketGroupOrders(BasketGroup group)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               object[] parameter = new object[2];

               parameter[0] = group.BasketGroupID;
               parameter[1] = (int)group.AllocationType;
               db.ExecuteScalar("P_RemoveBasketGroupOrders", parameter);
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

       }
       /// <summary>
       /// it changes the state of the group in case Allocate or Unallocate
       /// </summary>
       /// <param name="group"></param>
       public static void ChangeGroupState(AllocationGroup group, DateTime auecLocaldate)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
                object[] parameter = new object[7];
               parameter[0] = group.GroupID;
               parameter[1] = (int)group.State;
               parameter[2] = DateTime.Now.ToUniversalTime();
               parameter[3] = group.AllocatedQty;
               if (!auecLocaldate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
               {
                   parameter[4] = auecLocaldate;
               }
               else
               {
                   parameter[4] = TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
               }
               if (!group.Commission.Equals(double.Epsilon))
               {
                   parameter[5] = group.Commission;
               }
               else
               {
                   parameter[5] = 0.0;
               }
               if (!group.Fees.Equals(double.Epsilon))
               {
                   parameter[6] = group.Fees;
               }
               else
               {
                   parameter[6] = 0.0;
               } 
              
              
               if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
               {
                   // delete AllocationFund or Strategy IDS 
                   object[] SPParameters = new object[2];
                   SPParameters[0] = group.GroupID;
                   SPParameters[1] = (int)group.AllocationType;
                   db.ExecuteScalar("P_RemoveOnlyAllocated", SPParameters);
               }
               else
               {
                   if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                   {
                       SaveFunds(group);
                       SaveCommissionAndFeesForstrategy();
                   }
                   else if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
                   {
                       SaveStrategies(group);
                       SaveCommissionAndFeesForstrategy();
                   }
                   else
                   {
                       SaveFunds(group);
                       SaveStrategies(group);
                       SaveCommissionAndFeesForstrategy();
                   }
               }
               db.ExecuteScalar("P_ChangeGroupState", parameter);


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
       }
       /// <summary>
       /// it saves Allocation of  fund and save fund Position  
       /// </summary>
       /// <param name="group"></param>
       private static void SaveFunds(AllocationGroup group )
       {
             Database db = DatabaseFactory.CreateDatabase();
             try
             {
                 object[] parameter = new object[9];
                 parameter[0] = group.GroupID;
                 
                 foreach (AllocationFund fund in group.AllocationFunds)
                 {
                     parameter[1] = fund.FundID;
                     parameter[2] = fund.AllocatedQty;
                     parameter[3] = fund.Percentage;
                     if (!fund.Commission.Equals(double.Epsilon))
                     {
                         parameter[4] = fund.Commission;
                     }
                     else
                     {
                         parameter[4] = 0.0;
                     }
                     if (!fund.Commission.Equals(double.Epsilon))
                     {
                         parameter[5] = fund.Fees;
                     }
                     else
                     {
                         parameter[5] = 0.0;
                     }
                     if (!group.Commission.Equals(double.Epsilon))
                     {
                         parameter[6] = group.Commission;
                     }
                     else
                     {
                         parameter[6] = 0.0;
                     }
                     if (!group.Fees.Equals(double.Epsilon))
                     {
                         parameter[7] = group.Fees;
                     }
                     else
                     {
                         parameter[7] = 0.0;
                     } 
                     parameter[8] = group.IsBasketGroup;
                     //Added 2 parameters - rajat. 23 jan
                     //DateTime allocationTime = DateTime.UtcNow;
                     //parameter[6] = allocationTime;
                     //parameter[7] = TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, allocationTime).ToString();
                     db.ExecuteScalar("P_SaveAllocatedFunds", parameter);
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
       }
       /// <summary>
       /// it save Allocation of Strategies and save Strategies position 
       /// </summary>
       /// <param name="group"></param>
       private static void SaveStrategies(AllocationGroup group)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               object[] parameter = new object[4];
               parameter[0] = group.GroupID;
               
               foreach (AllocationStrategy strategy in group.Strategies)
               {
                   parameter[1] = strategy.StrategyID;
                   parameter[2] = strategy.AllocatedQty;
                   parameter[3] = strategy.Percentage;
                   //Added 2 parameters - rajat. 23 jan
                   //DateTime allocationTime = DateTime.UtcNow;
                   //parameter[4] = allocationTime;
                   //parameter[5] = TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, allocationTime).ToString();
                   db.ExecuteScalar("P_SaveAllocatedStrategies", parameter);
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
       }
       /// <summary>
       /// it updates the group when fill comes 
       /// </summary>
       /// <param name="group"></param>
       /// <param name="shouldDeleteOrders"></param>
       public static void UpdateGroup(AllocationGroup group,bool shouldDeleteOrders)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               AllocationOrder order = new AllocationOrder();
               object[] parameter = new object[21];
               parameter[0] = group.GroupID;
               parameter[1] = group.IsProrataActive;
               parameter[2] = group.OrderSideTagValue;
               parameter[3] = group.OrderTypeTagValue;
               parameter[4] = group.TradingAccountID;
               parameter[5] = group.UnderLyingID;
               parameter[6] = group.VenueID;
               parameter[7] = group.AllocatedQty;
               parameter[8] = group.AssetID;
               parameter[9] = group.AUECID;
               parameter[10] = group.AutoGrouped;
               parameter[11] = group.CounterPartyID;
               parameter[12] = group.CumQty;
               parameter[13] = group.CurrencyID;
               parameter[14] = group.ExchangeID;
               parameter[15] = group.AvgPrice;
               parameter[16] = group.Quantity;
               parameter[17] = DateTime.Now.ToUniversalTime();
               if (!group.AUECLocalDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
               {
                   parameter[18] = group.AUECLocalDate;//TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
               }
               else
               {
                   parameter[18] = TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
               }
               if (group.AllocationType.Equals(PranaInternalConstants.TYPE_OF_ALLOCATION.FUND))
               {
                   parameter[19] = group.Commission;
                   parameter[20] = group.Fees;
               }
               else
               {
                   parameter[19] = System.DBNull.Value;
                   parameter[20] = System.DBNull.Value;
               }
              db.ExecuteScalar("P_UpdateGroup", parameter);
               if (shouldDeleteOrders)
               {
                   DeleteGroupOrders(group);
                   SaveGroupOrders(group);
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
       }
       /// <summary>
       /// Modifies AllocatedQty in Allocated Groups in T_FundAllocation/T_StrategyAllocation Tables
       /// </summary>
       /// <param name="group"></param>
       public static void ModifyAllocatedFundsForProrata(AllocationFunds funds)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               object[] parameter = new object[5];
               foreach (AllocationFund allocatedFund in funds)
               {
                   parameter[0] = allocatedFund.GroupID;
                   parameter[1] = allocatedFund.FundID;
                   parameter[2] = allocatedFund.AllocatedQty;
                   parameter[3] = allocatedFund.Commission;
                   parameter[4] = allocatedFund.Fees;
                   db.ExecuteScalar("P_ModifyAllocatedFundForProrata", parameter);
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
       }
       /// <summary>
       /// Modifies AllocatedQty in Allocated Groups in T_StrategyAllocation Table
       /// </summary>
       /// <param name="strategies"></param>
       public static void ModifyAllocatedStraegiesForProrata(AllocationStrategies strategies)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               object[] parameter = new object[3];

               foreach (AllocationStrategy allocatedStrategy in strategies)
               {
                   parameter[0] = allocatedStrategy.GroupID;
                   parameter[1] = allocatedStrategy.StrategyID;
                   parameter[2] = allocatedStrategy.AllocatedQty;
                   db.ExecuteScalar("P_ModifyAllocatedStrategyForProrata", parameter);
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
       }
       /// <summary>
       /// it saves the grouped orders 
       /// </summary>
       /// <param name="group"></param>
       private static void SaveGroupOrders(AllocationGroup group)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               object[] parameter = new object[2];
               parameter[0] = group.GroupID;
               foreach (AllocationOrder order in group.Orders)
               {
                   parameter[1] = order.ClOrderID;
                   db.ExecuteScalar("P_SaveGroupOrders", parameter);
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
       }
       /// <summary>
       /// it removes the group order when unallocate or ungroup
       /// </summary>
       /// <param name="group"></param>
       private static void DeleteGroupOrders(AllocationGroup group)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               object[] parameter = new object[1];
               parameter[0] = group.GroupID;
               db.ExecuteScalar("P_DeleteGroupOrders", parameter);
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

       }
       #endregion


       //SP_OBSOLETE :As pre allocated is now handled with server fun: GetPreAllocatedOrderGroupID sp:P_GetPreAllocatedOrdergroupID
       public static string GetPreAllocatedOrderGroupID(string clOrderId, int allocationTypeID)
       {
           Database db = DatabaseFactory.CreateDatabase();
           string groupID = string.Empty;
           try
           {
               object[] parameter = new object[2];
               parameter[0] = clOrderId;
               parameter[1] = allocationTypeID;
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetPreAllocatedOrdergroupID", parameter))
               {
                   while (reader.Read())
                   {
                       object[] row = new object[reader.FieldCount];
                       reader.GetValues(row);
                       groupID = row[0].ToString();
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
           return groupID;
           #endregion

       }

       #region Getting Unallocated and Updated Orders
      /// <summary>
      /// bind data with allocation for both fund and Strategy 
      /// </summary>
      /// <param name="UserID"></param>
      /// <param name="currentTime"></param>
      /// <param name="typeOfAllocation"></param>
      /// <param name="lastOrderSeqNumber"></param>
      /// <returns></returns>
       public static AllocationOrderCollection GetUnAllocatedOrders(int UserID,string AllAUECDatesString, PranaInternalConstants.TYPE_OF_ALLOCATION typeOfAllocation)
       {
           Database db = DatabaseFactory.CreateDatabase();
           AllocationOrderCollection orders = new AllocationOrderCollection();
           try
           {
               AllocationOrder order = new AllocationOrder();
               object[] parameter = new object[2];
               parameter[0] = UserID;
               parameter[1] = AllAUECDatesString;
               //parameter[2] = lastOrderSeqNumber;
               string spName = "P_GetUnallocatedFundOrders";
               if (typeOfAllocation == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
               {
                   spName = "P_GetUnallocatedStrategyOrders";
               }
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(spName, parameter))
               {
                   while (reader.Read())
                   {
                       object[] row = new object[reader.FieldCount];
                       reader.GetValues(row);
                       orders.Add(FillOrder(row, 0, false));
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
           return orders;
       }
       /// <summary>
       /// when update allocation (button click) it gives updated orders
       /// </summary>
       /// <param name="UserID"></param>
       /// <param name="currentTime"></param>
       /// <param name="lastOrderSeqNumber"></param>
       /// <returns></returns>
       //public static AllocationOrderCollection GetUpdatedOrders(int UserID, DateTime currentTime, Int64 lastOrderSeqNumber)
       //{
       //    Database db = DatabaseFactory.CreateDatabase();
       //    AllocationOrderCollection orders = new AllocationOrderCollection();
       //    try
       //    {
       //        AllocationOrder order = new AllocationOrder();
       //        object[] parameter = new object[3];
       //        parameter[0] = UserID;
       //        parameter[1] = currentTime;
       //        parameter[2] = lastOrderSeqNumber;
       //        //Int64.MaxValue;
       //        using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetUpdatedOrders", parameter))
       //        {
       //            while (reader.Read())
       //            {
       //                object[] row = new object[reader.FieldCount];
       //                reader.GetValues(row);
       //                orders.Add(FillOrder(row, 0, false));
       //            }
       //        }
       //    }
       //    #region Catch
       //    catch (Exception ex)
       //    {
       //        // Invoke our policy that is responsible for making sure no secure information
       //        // gets out of our layer.
       //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


       //        if (rethrow)
       //        {
       //            throw;
       //        }

       //    }
       //    #endregion
       //    return orders;
       //}
       /// <summary>
       /// to get All PreAllocated  Basket orders fund
       /// </summary>
       /// <param name="lastOrderSeqNumber"></param>
       /// <returns></returns>
       //public static AllocationOrderCollection GetUpdatedBasketPreAllocatedFundOrders(Int64 lastOrderSeqNumber)
       //{
       //    Database db = DatabaseFactory.CreateDatabase();
       //    AllocationOrderCollection orders = new AllocationOrderCollection();
       //    try
       //    {
       //        AllocationOrder order = new AllocationOrder();
       //        object[] parameter = new object[1];
       //        parameter[0] = lastOrderSeqNumber;
       //        //Int64.MaxValue;
       //        using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetBasketPreAllocatedFundOrders", parameter))
       //        {
       //            while (reader.Read())
       //            {
       //                object[] row = new object[reader.FieldCount];
       //                reader.GetValues(row);
       //                orders.Add(FillOrder(row, 0, false));
       //            }
       //        }
       //    }
       //    #region Catch
       //    catch (Exception ex)
       //    {
       //        // Invoke our policy that is responsible for making sure no secure information
       //        // gets out of our layer.
       //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


       //        if (rethrow)
       //        {
       //            throw;
       //        }

       //    }
       //    #endregion
       //    return orders;
       //}
       /// <summary>
       /// to get All preAllocated Basket Strategy orders
       /// </summary>
       /// <param name="lastOrderSeqNumber"></param>
       /// <returns></returns>
       //public static AllocationOrderCollection GetUpdatedBasketPreAllocatedStrategyOrders(Int64 lastOrderSeqNumber)
       //{
       //    Database db = DatabaseFactory.CreateDatabase();
       //    AllocationOrderCollection orders = new AllocationOrderCollection();
       //    try
       //    {
       //        AllocationOrder order = new AllocationOrder();
       //        object[] parameter = new object[1];
       //        parameter[0] = lastOrderSeqNumber;
       //        //Int64.MaxValue;
       //        using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetBasketPreAllocatedStrategyOrders", parameter))
       //        {
       //            while (reader.Read())
       //            {
       //                object[] row = new object[reader.FieldCount];
       //                reader.GetValues(row);
       //                orders.Add(FillOrder(row, 0, false));
       //            }
       //        }
       //    }
       //    #region Catch
       //    catch (Exception ex)
       //    {
       //        // Invoke our policy that is responsible for making sure no secure information
       //        // gets out of our layer.
       //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


       //        if (rethrow)
       //        {
       //            throw;
       //        }

       //    }
       //    #endregion
       //    return orders;
       //}
       /// <summary>
       /// it takes All save commission and Fees form Database
       /// </summary>
       /// <returns></returns>
       public static AllocationFunds GetCommissionsAndFeesFromDB( )
       {
           Database db = DatabaseFactory.CreateDatabase();
           AllocationFunds commissionsAndFees = new AllocationFunds();
           try
           {
               AllocationFund commissionAndFee = new AllocationFund();
              
             
               //Int64.MaxValue;
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("[AL_GetAllCommissionFromDb]"))
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
               int GroupId = offset + 0;
               int FundId = offset + 1;
               int Commission = offset + 2;
               int Fees = offset + 3;
               try
               {
                   commissionAndFee.GroupID = row[GroupId].ToString();
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


       private static AllocationOrder FillOrder(object[] row, int offset, bool groupOrder)
       {
           if (offset < 0)
           {
               offset = 0;
           }

           AllocationOrder order = new AllocationOrder();


           if (row != null)
           {
               int ClOrderID = offset + 0;
               int SideTagValue = offset + 1;
               int Side = offset + 2;
               int TradingAcID = offset + 3;
               int TradingAcName = offset + 4;
               int Symbol = offset + 5;
               int CounterPartyID = offset + 6;
               int CounterParty = offset + 7;
               int VenueID = offset + 8;
               int VenueName = offset + 9;
               int OrderTypeTag = offset + 10;
               int OrderTypes = offset + 11;
               int AssetID = offset + 12;
               int AssetName = offset + 13;
               int UnderlyingID = offset + 14;
               int UnderLyingName = offset + 15;
               int ExchangeID = offset + 16;
               int ExchangeName = offset + 17;
               int AUECID = offset + 18;
               int Quantity = offset + 19;
               int CumQty = offset + 20;
               int Price = offset + 21;
               // For Grouped Order
               int GroupID = offset+22;
               // ForNormal Orders
               int FundID = offset + 22;
               int StrategyID = offset + 23;
               int OrigClOrderID = offset +24;
               int TransactTime= offset +25;
               int ListID = offset + 26;
               int SettlementDate = offset + 27;
               int CurrencyID = offset + 28;
               int ExpirationDate = offset + 29;
               //int Commission = offset + 27;
               //int Fees = offset + 28;
               // now dont incremtne with put checking GroupID
               try
               {
                  // order.AllocatedQty = int.Parse(row[AllocatedQty].ToString());
                   if (row[ClOrderID]!= System.DBNull.Value)
                   {
                       order.ClOrderID = row[ClOrderID].ToString();
                   }
                   order.OrderSide = row[Side].ToString();
                   order.OrderSideTagValue = row[SideTagValue].ToString();
                   order.Symbol = row[Symbol].ToString();
                   if (row[CounterParty] != System.DBNull.Value)
                   {
                       order.CounterPartyName = row[CounterParty].ToString();
                   }
                   //else
                   //{
                   //    order.CounterPartyName = "Multiple";
                   //}
                   order.CounterPartyID = int.Parse(row[CounterPartyID].ToString());
                   order.VenueID = int.Parse(row[VenueID].ToString());
                   if (row[VenueName] != System.DBNull.Value)
                   {
                       order.Venue = row[VenueName].ToString();
                   }
                   //else
                   //{
                   //    order.Venue = "Multiple";
                   //}
                   order.Quantity = Convert.ToInt64(row[Quantity]);
                   order.CumQty = Convert.ToInt64((row[CumQty]));
                   order.TradingAccountID = int.Parse(row[TradingAcID].ToString());
                   order.TradingAccountName = row[TradingAcName].ToString();

                   double avgPrice = Convert.ToDouble(row[Price]);
                   order.AvgPrice = avgPrice;

                   order.OrderType = row[OrderTypes].ToString();
                   order.OrderTypeTagValue = row[OrderTypeTag].ToString();
                   order.AssetID = int.Parse(row[AssetID].ToString());
                   order.AssetName = row[AssetName].ToString();
                   order.ExchangeID = int.Parse(row[ExchangeID].ToString());
                   order.ExchangeName = row[ExchangeName].ToString();
                   order.UnderlyingID = int.Parse(row[UnderlyingID].ToString());
                   order.UnderlyingName = row[UnderLyingName].ToString();
                   order.AUECID = int.Parse(row[AUECID].ToString());
                   

                   if (order.CumQty < order.Quantity)
                       order.NotAllExecuted = true;
                  
                   if (groupOrder)
                   {
                        order.GroupID = row[GroupID].ToString();
                        order.ListID = row[GroupID+1].ToString();
                        order.TransactionTime = row[GroupID+2].ToString();

                        if (row[GroupID + 3] != System.DBNull.Value)
                        {
                            order.AUECLocalDate = DateTime.Parse(row[GroupID + 3].ToString());
                        }
                        if (row[GroupID + 4] != System.DBNull.Value)
                       {
                           order.SettlementDate = DateTime.Parse(row[GroupID + 4].ToString());
                       }
                       if (row[GroupID + 5] != System.DBNull.Value)
                       {
                           order.ExpirationDate = DateTime.Parse(row[GroupID + 5].ToString());
                       }  
                       if (row[GroupID + 6] != System.DBNull.Value)
                       {
                           order.CurrencyID = int.Parse(row[GroupID + 6].ToString());
                       } 
                       //if (!row[GroupID + 2].Equals(System.DBNull.Value))
                       // {
                       //     order.Commission = float.Parse(row[GroupID + 2].ToString());
                       // }
                       // if (!row[GroupID + 3].Equals(System.DBNull.Value))
                       // {
                       //     order.Fees = float.Parse(row[GroupID + 3].ToString());
                       // }
                    }
                   else
                   {
                       order.FundID = int.Parse(row[FundID].ToString());
                       if (row[StrategyID] != System.DBNull.Value)
                       {
                           order.StrategyID = int.Parse(row[StrategyID].ToString());
                       }
                       if (row[OrigClOrderID] != System.DBNull.Value)
                       {
                           order.OrigClOrderID = row[OrigClOrderID].ToString();
                       }
                       if (row[TransactTime] != System.DBNull.Value)
                       {
                           order.TransactionTime = row[TransactTime].ToString();
                       }
                       if (row[ListID] != System.DBNull.Value)
                       {
                           order.ListID = row[ListID].ToString();
                       }
                       if (row[ListID+1] != System.DBNull.Value)
                       {
                           order.StateID = (PranaInternalConstants.ORDERSTATE_ALLOCATION)int.Parse(row[ListID+1].ToString());
                       }
                       if (row[ListID+2] != System.DBNull.Value)
                       {
                           order.AUECLocalDate = DateTime.Parse(row[ListID+2].ToString());
                       }
                       if (row[ListID + 3] != System.DBNull.Value)
                       {
                           order.GroupAuecLocalDate = DateTime.Parse(row[ListID + 3].ToString());
                       }
                       if (row[ListID + 4] != System.DBNull.Value)
                       {
                           order.SettlementDate = DateTime.Parse(row[ListID + 4].ToString());
                       }
                       if (row[ListID + 5] != System.DBNull.Value)
                       {
                           order.CurrencyID = int.Parse(row[ListID + 5].ToString());
                       }
                       if (row[ListID + 6] != System.DBNull.Value)
                       {
                           order.ExpirationDate = DateTime.Parse(row[ListID + 6].ToString());
                       }
                       //if (!row[Commission].Equals(System.DBNull.Value))
                       //{
                       //    order.Commission = float.Parse(row[Commission].ToString());
                       //}
                       //if (!row[Fees].Equals(System.DBNull.Value))
                       //{
                       //    order.Fees = float.Parse(row[Fees].ToString());
                       //}
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


           }


           return order;

       }
       #endregion

       #region Getting a Group
       /// <summary>
       /// For Getting all Groups (Allocated and UnAllocated) it also conatis Funds or Stategies Associated with them
       /// </summary>
       /// <param name="currentTime"></param>
       /// <returns></returns>
       public static AllocationGroups GetGroups(string AllAUECDatesString)
       {
           Database db = DatabaseFactory.CreateDatabase();
           AllocationGroups groups = new AllocationGroups();
           try
           {
               object[] parameter = new object[1];
               parameter[0] = AllAUECDatesString;
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetGroups", parameter))
               {
                   while (reader.Read())
                   {
                       object[] row = new object[reader.FieldCount];
                       reader.GetValues(row);
                       groups.Add(FillGroup(row, 0));
                       
                   }

               }
               if (groups.Count > 0)
               {
                   AllocationOrderCollection orders = GetGroupedOrders(AllAUECDatesString);
                   foreach (AllocationOrder order in orders)
                   {
                       AllocationGroup allocationGroup = groups.GetGroup(order.GroupID);
                       if (allocationGroup != null)
                       {
                           allocationGroup.AddOrder(order);
                       }
                   }

                   AllocationFunds funds = FundStraegyManager.GetAllocatedFunds(AllAUECDatesString);
                   foreach (AllocationFund fund in funds)
                   {
                       AllocationGroup group = groups.GetGroup(fund.GroupID);
                       if (group != null)
                       {
                           if (group.AllocationFunds == null)
                           {
                               group.AllocationFunds = new AllocationFunds();
                           }

                           group.AllocationFunds.Add(fund);
                           if (fund.Commission != double.MinValue)
                           {
                               group.Commission += fund.Commission;
                               group.Fees += fund.Fees;
                           }
                           else
                           {
                               CommissionCalculator commissioncalculator = new CommissionCalculator();
                               commissioncalculator.StartCalculation(group);
                               AllocationGroups grps = new AllocationGroups();
                               grps.Add(group);
                               //SaveAndUpdateCommissionandFees(grps);
                              
                             
                              
                           }
                          
                         //  fund.Parent = group;
                       }
                   }
                   AllocationStrategies strategies = FundStraegyManager.GetAllocatedStrategies(AllAUECDatesString);
                   foreach (AllocationStrategy strategy in strategies)
                   {
                       AllocationGroup group = groups.GetGroup(strategy.GroupID);
                       if (group != null)
                       {
                           if (group.Strategies == null)
                           {
                               group.Strategies = new AllocationStrategies();
                           }
                           group.Strategies.Add(strategy);
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
           return groups;
       }
       /// <summary>
       /// get All grouped orders
       /// </summary>
       /// <param name="currentTime"></param>
       /// <returns></returns>
       private static AllocationOrderCollection GetGroupedOrders(string AllAUECDatesString)
       {
           Database db = DatabaseFactory.CreateDatabase();
           AllocationOrderCollection orders = new AllocationOrderCollection();
           try
           {
               object[] parameter = new object[1];
               parameter[0] = AllAUECDatesString;
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetGroupedOrders", parameter))
               {
                   while (reader.Read())
                   {
                       object[] row = new object[reader.FieldCount];
                       reader.GetValues(row);
                       AllocationOrder order = FillOrder(row, 0, true);
                       orders.Add(order);
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
           return orders;
       }

       private static AllocationOrderCollection GetPreAllocatedGroupedOrders(string groupIDS)
       {
           Database db = DatabaseFactory.CreateDatabase();
           AllocationOrderCollection orders = new AllocationOrderCollection();
           try
           {
               object[] parameter = new object[1];
               parameter[0] = groupIDS;
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetPreAllocatedGroupedOrders", parameter))
               {
                   while (reader.Read())
                   {
                       object[] row = new object[reader.FieldCount];
                       reader.GetValues(row);
                       AllocationOrder order = FillOrder(row, 0, true);
                       orders.Add(order);
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
           return orders;
       }

       private static AllocationGroup FillGroup(object[] row, int offset)
       {
           
           if (offset < 0)
           {
               offset = 0;
           }
           AllocationGroup group= new AllocationGroup();
           try
           {
               int GroupID = offset + 0;
               int IsPreAllocated = offset + 1;
               int UserID = offset + 2;
               int AllocationTypeID = offset + 3;
               int ISProrataActive = offset + 4;
               int SingleOrderAllocation = offset + 5;
               int AutoGrouped = offset + 6;
               int StateID = offset + 7;
               int AllocatedQty = offset + 8;
               int IsBasketGroup = offset + 9;

               //int Commission = offset + 10;
               //int Fees = offset + 11;

               int OrderSideTagValue = offset + 10;
               int OrderSide = offset + 11;
               int TradingAccountID = offset + 12;
               int TradingAccount = offset + 13;
               int Symbol = offset + 14;
               int CounterPartyID = offset + 15;
               int CounterParty = offset + 16;
               int VenueID = offset + 17;
               int Venue = offset + 18;
               int OrderTypeTagValue = offset + 19;
               int OrderType = offset + 20;
               int AveragePrice = offset + 21;
               int CumQty = offset + 22;
               int TargetQty = offset + 23;
               int IsManualGroup = offset + 24;
               int AssetID = offset + 25;
               int UnderLyingID = offset + 26;
               int ExchangeID = offset + 27;
               int CurrencyID = offset + 28;
               int AUECID = offset +29;
               int AUECLocalDate = offset + 30;
               int SettlementDate = offset + 31;

               if (row != null)
               {
                   group.IsManualGroup = Convert.ToBoolean(row[IsManualGroup].ToString());
                   
                   group.GroupID = row[GroupID].ToString();
                  
                   group.IsPreAllocated = Convert.ToBoolean(row[IsPreAllocated].ToString());
                   group.UserID = int.Parse(row[UserID].ToString());
                   //group.AllocationType = row[AllocationTypeID].ToString();
                   int allocationTypeID = int.Parse(row[AllocationTypeID].ToString());


                   if (allocationTypeID == (int)PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                   {
                       group.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
                   }
                   else if (allocationTypeID == (int)PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
                   {
                       group.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY;
                   }
                   else
                       group.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.BOTH;
                   group.IsProrataActive = Convert.ToBoolean(row[ISProrataActive].ToString());
                   group.SingleOrderAllocation = Convert.ToBoolean(row[SingleOrderAllocation].ToString());
                   group.AutoGrouped = Convert.ToBoolean(row[AutoGrouped].ToString());
                   //group.StateID = row[StateID].ToString();
                   int stateID = int.Parse(row[StateID].ToString());


                   if (stateID == (int)PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                   {
                       group.State = PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
                   }
                   else if (stateID == (int)PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
                   {
                       group.State = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
                   }
                   group.AllocatedQty = double.Parse(row[AllocatedQty].ToString());
                   group.IsBasketGroup = Convert.ToBoolean(row[IsBasketGroup].ToString());
                   group.AUECLocalDate = DateTime.Parse(row[AUECLocalDate].ToString());

                    //if (row[Commission] != System.DBNull.Value)
                    //{
                    //    group.Commission = double.Parse(row[Commission].ToString());
                    //}
                    //if (row[Fees] != System.DBNull.Value)
                    //{
                    //    group.Fees = double.Parse(row[Fees].ToString());
                    //}
                   // added by Sandeep as on 04-Dec-2007 , these fields needed for Manual Groups
                   if (group.IsManualGroup)
                   {                      
                       group.OrderSideTagValue = row[OrderSideTagValue].ToString();
                       group.OrderSide = row[OrderSide].ToString();
                       group.TradingAccountID = int.Parse(row[TradingAccountID].ToString());
                       group.TradingAccountName =  CachedDataManager.GetInstance.GetTradingAccountText(Convert.ToInt16(row[TradingAccountID].ToString()));
                       group.Symbol = row[Symbol].ToString();

                       if (row[CounterPartyID] != System.DBNull.Value)
                       {
                           group.CounterPartyID = int.Parse(row[CounterPartyID].ToString());
                       }
                       if (row[CounterParty] != System.DBNull.Value)
                       {
                           group.CounterPartyName = row[CounterParty].ToString();
                       }
                       else
                       {
                           group.CounterPartyName = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;
                       }

                       if (row[VenueID] != System.DBNull.Value)
                       {
                           group.VenueID = int.Parse(row[VenueID].ToString());
                       }
                       if (row[Venue] != System.DBNull.Value)
                       {
                           group.Venue = row[Venue].ToString();
                       }
                       else
                       {
                           group.Venue = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;
                       }

                       if (row[OrderTypeTagValue] != System.DBNull.Value)
                       {
                           group.OrderTypeTagValue = row[OrderTypeTagValue].ToString();
                       }
                       if (row[OrderType] != System.DBNull.Value)
                       {
                           group.OrderType = row[OrderType].ToString();
                       }

                       group.AvgPrice = double.Parse(row[AveragePrice].ToString());
                       group.CumQty = double.Parse(row[CumQty].ToString());
                       group.Quantity = double.Parse(row[TargetQty].ToString());
                       if (row[AssetID] != System.DBNull.Value)
                       {
                           group.AssetID = int.Parse(row[AssetID].ToString());
                       }
                        if (row[UnderLyingID] != System.DBNull.Value)
                       {
                           group.UnderLyingID = int.Parse(row[UnderLyingID].ToString());
                       } 
                       if (row[ExchangeID] != System.DBNull.Value)
                       {
                           group.ExchangeID = int.Parse(row[ExchangeID].ToString());
                       }
                       if (row[CurrencyID] != System.DBNull.Value)
                       {
                           group.CurrencyID = int.Parse(row[CurrencyID].ToString());
                       }
                       if (row[AUECID] != System.DBNull.Value)
                       {
                           group.AUECID = int.Parse(row[AUECID].ToString());
                       }
                   }
                 
                   if (group.IsManualGroup == true || group.IsPreAllocated == true)
                   {
                       _groupID += "," + group.GroupID ;
                   }
                   if ( group.IsPreAllocated == true)
                   {
                       _preAllocatedGroupID += "," + group.GroupID;
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
           return group;

       }
       #endregion
       /// <summary>
       /// when Allocation or grouping will done 
       /// </summary>
       /// <param name="orders"></param>
       /// <param name="stateID"></param>
       /// <param name="typeOfAllocation"></param>
       public static void ChangeListOrdersState(AllocationOrderCollection orders, PranaInternalConstants.ORDERSTATE_ALLOCATION stateID, PranaInternalConstants.TYPE_OF_ALLOCATION typeOfAllocation)
       {
           Database db = DatabaseFactory.CreateDatabase();
           try
           {
               foreach (AllocationOrder order in orders)
               {
                   if (order.ListID != string.Empty)
                   {
                       object[] parameter = new object[3];
                       parameter[0] = order.ClOrderID;
                       parameter[1] = (int)stateID;
                       parameter[2] = (int)typeOfAllocation;
                       db.ExecuteScalar("P_ChangeListOrdersState", parameter);
                   }
               }
           }
           catch (Exception ex)
           {

               // Invoke our policy that is responsible for making sure no secure information
               // gets out of our layer.
               bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

               if (rethrow)
               {
                   throw;
               }
           }
       }

       #region Allocated Funds and Strategies for Reports
       /// <summary>
       /// to get fund allocation report
       /// </summary>
       /// <param name="date"></param>
       /// <returns></returns>
       public static AllocatedGroups GetAllocatedFundsFromDB(string AllAUECDatesString)
       {
           AllocatedGroups allocatedGroups = new AllocatedGroups();
           Object[] parameter = new object[1];
           parameter[0] = AllAUECDatesString;
           try
           {
               Database db = DatabaseFactory.CreateDatabase();
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetFundAllocation", parameter))
               {
                   while (reader.Read())
                   {

                       object[] row = new object[reader.FieldCount];
                       reader.GetValues(row);
                       allocatedGroups.Add(FillAllocatedFundsOrStrategies(true, row, 0));

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
           return allocatedGroups;


       }
       /// <summary>
       /// to get allocation report
       /// </summary>
       /// <param name="date"></param>
       /// <returns></returns>
       public static AllocatedGroups GetAllocatedStrategiesFromDb(string AllAUECDatesString)
       {
           AllocatedGroups allocatedGroups = new AllocatedGroups();
           Object[] parameter = new object[1];
           parameter[0] = AllAUECDatesString;
           try
           {
               Database db = DatabaseFactory.CreateDatabase();
               using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetStrategyAllocation", parameter))
               {
                   while (reader.Read())
                   {

                       object[] row = new object[reader.FieldCount];
                       reader.GetValues(row);
                       allocatedGroups.Add(FillAllocatedFundsOrStrategies(false, row, 0));

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
           return allocatedGroups;


       }
       private static AllocatedGroup FillAllocatedFundsOrStrategies(bool fund,object[] row, int offset)
       {
           if (offset < 0)
           {
               offset = 0;
           }
           AllocatedGroup allocatedGroup = new AllocatedGroup();
           if (row != null)
           {
              int AllocationID=0;
              int GroupID=1;
              int  AllocatedQty=2;
              int  FundID=3;
              int Quantity=4;
              int CumQty=5;
              int AvgPrice = 6;
              int Symbol=7;
              int CounterPartyID=8;
              int VenueID=9;
              int OrderSideTagValue=10;
              int OrderTypeTagValue=11;
              int TradingAccountID=12;
              int AssetID=13;
              int UnderLyingID=14;
              int ExchangeID=15;
              int AUECID = 16;
              int ExchangeName = 17;
              int Commission = 18;
              int Fees = 19;
               try
               {
                   allocatedGroup.AllocationID = row[AllocationID].ToString();

                   if (fund)
                   {
                       allocatedGroup.FundID = int.Parse(row[FundID].ToString());
                       allocatedGroup.AllocationFund = CachedDataManager.GetInstance.GetFundText(allocatedGroup.FundID);
                   }
                   else
                   {
                       allocatedGroup.StrategyID = int.Parse(row[FundID].ToString());
                       allocatedGroup.Strategy = CachedDataManager.GetInstance.GetStrategyText(allocatedGroup.StrategyID);
                   }

                   allocatedGroup.OrderTypeTagValue = row[OrderTypeTagValue].ToString();
                   allocatedGroup.SideTagValue = row[OrderSideTagValue].ToString();
                   allocatedGroup.Symbol = row[Symbol].ToString();
                   allocatedGroup.CounterPartyID = int.Parse(row[CounterPartyID].ToString());
                   allocatedGroup.VenueID = int.Parse(row[VenueID].ToString());
                   allocatedGroup.TradingAccountID =int.Parse(row[TradingAccountID].ToString());
                   allocatedGroup.AllocatedQty = Convert.ToInt64(row[AllocatedQty]);
                   allocatedGroup.AvgPrice = double.Parse((row[AvgPrice].ToString()));
                   allocatedGroup.CumQty = Convert.ToInt64(row[CumQty]);
                   allocatedGroup.Quantity = Convert.ToInt64(row[Quantity]);
                   allocatedGroup.AssetID = int.Parse(row[AssetID].ToString());
                   allocatedGroup.ExchangeID = int.Parse(row[ExchangeID].ToString());
                   allocatedGroup.UnderlyingID = int.Parse(row[UnderLyingID].ToString());
                   allocatedGroup.AUECID = int.Parse(row[AUECID].ToString());
                   allocatedGroup.OrderSide = row[18].ToString();
                   if (!row[AUECID+3].Equals(System.DBNull.Value))
                   {
                       allocatedGroup.Commission = float.Parse(row[AUECID + 3].ToString());
                   }
                   if (!row[AUECID + 4].Equals(System.DBNull.Value))
                   {
                       allocatedGroup.Fees = float.Parse(row[AUECID + 4].ToString());
                   }
              

                  allocatedGroup.AssetName= CachedDataManager.GetInstance.GetAssetText(allocatedGroup.AssetID);
                  allocatedGroup.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(allocatedGroup.UnderlyingID);
                  allocatedGroup.ExchangeName = row[ExchangeName].ToString();
                  allocatedGroup.Venue = CachedDataManager.GetInstance.GetVenueText(allocatedGroup.VenueID);
                  allocatedGroup.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(allocatedGroup.TradingAccountID);
                  allocatedGroup.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(allocatedGroup.CounterPartyID);
                  //allocatedGroup.Side = Prana.BusinessLogic.TagDatabaseManager.GetInstance.GetOrderSideText(allocatedGroup.SideTagValue);
                  allocatedGroup.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(allocatedGroup.OrderTypeTagValue);
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


           return allocatedGroup;   


       }
       #endregion

       public static int UserID
       {
           get { return _userID; }
           set { _userID = value; }
       }
       /// <summary>
       /// save commission and fees 
       /// </summary>
       /// <param name="allocatedGroups"></param>
       /// <returns></returns>
       public static int SaveAndUpdateCommissionandFees(AllocatedGroups allocatedGroups)
       {
           int rowsAffected = 0;
           string commissionandFeesXML = XMLUtilities.SerializeToXML(allocatedGroups);
           try
           {
               Database db = DatabaseFactory.CreateDatabase();
               DbCommand cmd = new SqlCommand();
               cmd.CommandText = "AL_SaveCommissionandFees";
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
       /// <summary>
       /// save commission  and fees for strategy 
       /// </summary>
       /// <returns></returns>
       public static int SaveCommissionAndFeesForstrategy()
       {
           int rowsAffected = 0;
           
           try
           {
               Database db = DatabaseFactory.CreateDatabase();
               DbCommand cmd = new SqlCommand();
               cmd.CommandText = "AL_SaveCommissionAndFeesForstrategy";
               cmd.CommandType = CommandType.StoredProcedure;
               

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
       /// <summary>
       /// get manual orders updated 
       /// </summary>
       /// <param name="currentTime"></param>
       /// <returns></returns>
       //public static AllocationGroups GetupdatedGroups(DateTime currentTime)
       //{
       //    Database db = DatabaseFactory.CreateDatabase();
       //    AllocationGroups groups = new AllocationGroups();
       //    try
       //    {
       //        object[] parameter = new object[2]; ;
       //        if (!string.IsNullOrEmpty(_groupID))
       //        {
       //            parameter[0] = _groupID.Substring(1);
       //        }
       //        else
       //        {
       //            parameter[0] = _groupID;
       //        }
       //        parameter[1] = currentTime;
       //        using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetGroupsManual", parameter))
       //        {
       //            while (reader.Read())
       //            {
       //                object[] row = new object[reader.FieldCount];
       //                reader.GetValues(row);
       //                groups.Add(FillGroup(row, 0));
       //            }

       //        }

       //        if (groups.Count > 0 || _preAllocatedGroupID!=string.Empty)
       //        {
                      
                  
       //            AllocationOrderCollection orders = GetPreAllocatedGroupedOrders(_preAllocatedGroupID);
       //            foreach (AllocationOrder order in orders)
       //            {
       //                AllocationGroup allocationGroup = groups.GetGroup(order.GroupID);
       //                if (allocationGroup != null)
       //                {
       //                    allocationGroup.AddOrder(order);
       //                }
       //            }

       //            AllocationFunds funds = FundStraegyManager.GetAllocatedFunds(currentTime);
       //            foreach (AllocationFund fund in funds)
       //            {
       //                AllocationGroup group = groups.GetGroup(fund.GroupID);
       //                if (group != null)
       //                {
       //                    if (group.AllocationFunds == null)
       //                    {
       //                        group.AllocationFunds = new AllocationFunds();
       //                    }
       //                    group.AllocationFunds.Add(fund);
       //                    fund.Parent = group;
       //                }
       //            }
       //            AllocationStrategies strategies = FundStraegyManager.GetAllocatedStrategies(currentTime);
       //            foreach (AllocationStrategy strategy in strategies)
       //            {
       //                AllocationGroup group = groups.GetGroup(strategy.GroupID);
       //                if (group != null)
       //                {
       //                    if (group.Strategies == null)
       //                    {
       //                        group.Strategies = new AllocationStrategies();
       //                    }
       //                    group.Strategies.Add(strategy);
       //                }
       //            }
       //        }
       //    }
       //    #region Catch
       //    catch (Exception ex)
       //    {
       //        // Invoke our policy that is responsible for making sure no secure information
       //        // gets out of our layer.
       //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


       //        if (rethrow)
       //        {
       //            throw;
       //        }

       //    }
       //    #endregion
       //    return groups;
       //}

        //#endregion DB Related Activites of  a Group (Allocated and UnAllocated)

        #region Commission Rules Cache

        static CommissionRulesCacheManager _commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();
        static List<CVAUECFundCommissionRule> _cvUECFundCommissionRuleList = new List<CVAUECFundCommissionRule>();

        public static void GetAllSavedCommissionRules()
        {
            CommissionRulesCacheManager.GetInstance().ClearAllCommissionRuleCollections();

            List<CommissionRule> SavedCommisionRules = GetCommissionRules();

            SavedCommisionRules.ToString();
            //DataSet dummydataset = new DataSet();
            //dummydataset.Tables.Add(SavedCommisionRules);
            //string s = dummydataset.GetXml();


            foreach (CommissionRule commRule in SavedCommisionRules)
            {
                //Guid commRuleId= commRule.RuleID;
                // get all the Asset 

                //TODO : Rajat .. Need to change the multiple db calls into one
                List<AssetCategory> commissionRuleAssets = GetCommissionRuleAssets(commRule.RuleID);

                if (commissionRuleAssets.Count > 0)
                {
                    commRule.AssetIdList = commissionRuleAssets;
                }
                List<CommissionRuleCriteria> CommRuleCriteria = new List<CommissionRuleCriteria>();
                if (commRule.IsCriteriaApplied == true)
                {
                    //TODO : Rajat .. Need to change the multiple db calls into one
                    CommRuleCriteria = GetCommissionRuleCriterias(commRule.RuleID);
                }
                if (CommRuleCriteria.Count > 0)
                {
                    commRule.CommissionRuleCriteiaList = CommRuleCriteria;
                }

                CommissionRulesCacheManager.GetInstance().AddCommissionRule(commRule);
            }
        }

        /// <summary>
        /// to get the specefied criteria for selected rule
        /// </summary>
        /// <param name="commissionRuleId"></param>
        /// <returns></returns>
        public static List<CommissionRuleCriteria> GetCommissionRuleCriterias(Guid commissionRuleId)
        {
            List<CommissionRuleCriteria> commissionRuleCriteriaColl = new List<CommissionRuleCriteria>();
            Object[] parameter = new object[1];
            parameter[0] = commissionRuleId;
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetCommissionRuleCriterias", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleCriteriaColl.Add(FillCommissionRuleCriteria(row, 0));
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
            return commissionRuleCriteriaColl;
        }

        private static CommissionRule FillCommissionRule(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int RuleName = 1 + offSet;
            int RuleDescription = 2 + offSet;
            int ApllyRuleForTrade = 3 + offSet;
            int CalculationBasedOn = 4 + offSet;
            int CommissionRate = 5 + offSet;
            int MinCommission = 6 + offSet;
            int IsCriteriaApplied = 7 + offSet;
            int IsClearingFeeApplied = 8 + offSet;
            int CalculationBasedOnClearing = 9 + offSet;
            int CommissionRateClearing = 10 + offSet;
            int MinimumCommissionClearing = 11 + offSet;


            CommissionRule commissionRule = new CommissionRule();
            try
            {
                if (row[RuleID] != null)
                {
                    commissionRule.RuleID = (Guid)row[RuleID];
                }
                if (row[RuleName] != null)
                {
                    commissionRule.RuleName = row[RuleName].ToString();
                }
                if (row[RuleDescription] != null && row[RuleDescription] != System.DBNull.Value)
                {
                    commissionRule.RuleDescription = row[RuleDescription].ToString();
                }
                if (row[ApllyRuleForTrade] != null)
                {
                    commissionRule.ApplyRuleForTrade = (TradeType)(row[ApllyRuleForTrade]);
                }
                if (row[CalculationBasedOn] != null)
                {
                    commissionRule.RuleAppliedOn = (CommissionCalculationBasis)row[CalculationBasedOn];
                }
                if (row[CommissionRate] != null)
                {
                    commissionRule.CommissionRate = float.Parse(row[CommissionRate].ToString());
                }
                if (row[MinCommission] != null)
                {
                    commissionRule.MinCommission = float.Parse(row[MinCommission].ToString());
                }
                if (row[IsCriteriaApplied] != null)
                {
                    commissionRule.IsCriteriaApplied = bool.Parse(row[IsCriteriaApplied].ToString());
                }
                if (row[IsClearingFeeApplied] != null)
                {
                    commissionRule.IsClearingFeeApplied = bool.Parse(row[IsClearingFeeApplied].ToString());
                }
                if (row[CalculationBasedOnClearing] != null && row[CalculationBasedOnClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeCalculationBasedOn = (CommissionCalculationBasis)row[CalculationBasedOnClearing];
                }
                if (row[CommissionRateClearing] != null && row[CommissionRateClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeRate = float.Parse(row[CommissionRateClearing].ToString());
                }
                if (row[MinimumCommissionClearing] != null && row[MinimumCommissionClearing] != System.DBNull.Value)
                {
                    commissionRule.MinClearingFee = float.Parse(row[MinimumCommissionClearing].ToString());
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
            return commissionRule;
        }
        /// <summary>
        /// for getting  assets permitted for particular rule
        /// </summary>
        /// <param name="commRuleId"></param>
        /// <returns></returns>
        private static List<AssetCategory> GetCommissionRuleAssets(Guid commRuleId)
        {
            List<AssetCategory> commissionRuleList = new List<AssetCategory>();

            Object[] parameter = new object[1];
            parameter[0] = commRuleId;

            Database db = DatabaseFactory.CreateDatabase();

            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetCommissionRuleAssets", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleList.Add(FillAssetCategory(row, 0));
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
            return commissionRuleList;

        }

        private static AssetCategory FillAssetCategory(object[] row, int offSet)
        {
            int AssetId = 0 + offSet;

            AssetCategory assetId = new AssetCategory();
            try
            {
                if (row[AssetId] != null && row[AssetId] != System.DBNull.Value)
                {
                    assetId = (AssetCategory)row[AssetId];
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
            return assetId;
        }

        /// <summary>
        /// for getting  All save commission rule to generate list for calculation of commission 
        /// </summary>
        /// <returns></returns>
        private static List<CommissionRule> GetCommissionRules()
        {
            List<CommissionRule> commissionRuleList = new List<CommissionRule>();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetCommissionRules"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleList.Add(FillCommissionRule(row, 0));
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
            return commissionRuleList;
        }

        private static CommissionRuleCriteria FillCommissionRuleCriteria(object[] row, int offSet)
        {
            int CommissionCriteriaId = 0 + offSet;
            int ValueGreaterThan = 1 + offSet;
            int ValueLessThanOrEqualTo = 2 + offSet;
            int CommissionRate = 3 + offSet;

            CommissionRuleCriteria commissionRuleCriteria = new CommissionRuleCriteria();
            try
            {
                if (row[CommissionCriteriaId] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionCriteriaId = int.Parse(row[CommissionCriteriaId].ToString());
                }
                if (row[ValueGreaterThan] != System.DBNull.Value)
                {
                    commissionRuleCriteria.ValueGreaterThan = Convert.ToDouble(row[ValueGreaterThan].ToString());
                }

                if (row[ValueLessThanOrEqualTo] != System.DBNull.Value)
                {
                    commissionRuleCriteria.ValueLessThanOrEqual = Convert.ToDouble(row[ValueLessThanOrEqualTo].ToString());
                }
                if (row[CommissionRate] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionRate = Convert.ToDouble(row[CommissionRate].ToString());
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
            return commissionRuleCriteria;
        }

        public static void GetAllCommissionRulesForCVAUEC(int companyID)
        {
            _commissionRulesCacheManager.GetAllCVAUECFundCommissionRules().Clear();
            _commissionRulesCacheManager.ClearCVAUECCommissionRulesDictionary();

            List<CVAUECFundCommissionRule> cvAUECFundCommissionRulesList = new List<CVAUECFundCommissionRule>();
            Object[] parameter = new object[1];
            parameter[0] = companyID;
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetAllCommissionRulesForCVAUEC", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //cvAUECFundCommissionRulesList.Add(FillCVAUECFundCommissionRules(row, 0));
                        _commissionRulesCacheManager.AddCVAUECRule(FillCVAUECFundCommissionRules(row, 0));
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


            //return cvAUECFundCommissionRulesList;
        }
        private static Prana.BusinessObjects.CommissionRule _tempCommRuleSelect = new Prana.BusinessObjects.CommissionRule();
        public static CVAUECFundCommissionRule FillCVAUECFundCommissionRules(object[] row, int offSet)
        {
            int CVAUECRuleId = 0 + offSet;
            int CVId = 1 + offSet;
            int AUECId = 2 + offSet;
            int FundId = 3 + offSet;
            int SingleRuleId = 4 + offSet;
            int BasketRuleId = 5 + offSet;
            int CompanyID = 6 + offSet;
            int CounterPartyID = 7 + offSet;
            int VenueID = 8 + offSet;

            CVAUECFundCommissionRule cvAUECFundCommissionRule = new CVAUECFundCommissionRule();
            CommissionRulesCacheManager commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();
            Guid ruleID = Guid.Empty;
            Prana.BusinessObjects.CommissionRule commissionRule = new Prana.BusinessObjects.CommissionRule();
            _tempCommRuleSelect.RuleID = Guid.Empty;
            _tempCommRuleSelect.RuleName = "-Select-";


            try
            {
                if (row[CVAUECRuleId] != System.DBNull.Value)
                {
                    cvAUECFundCommissionRule.CVAUECRuleID = int.Parse(row[CVAUECRuleId].ToString());
                }

                if (row[CVId] != System.DBNull.Value)
                {
                    cvAUECFundCommissionRule.CVID = int.Parse(row[CVId].ToString());
                }

                if (row[AUECId] != System.DBNull.Value)
                {
                    cvAUECFundCommissionRule.AUECID = int.Parse(row[AUECId].ToString());
                }
                if (row[FundId] != System.DBNull.Value)
                {
                    cvAUECFundCommissionRule.FundID = int.Parse(row[FundId].ToString());
                }

                if (row[SingleRuleId] != System.DBNull.Value)
                {
                    ruleID = (Guid)row[SingleRuleId];
                    commissionRule = commissionRulesCacheManager.GetCommissionRuleByRuleId(ruleID);
                    //cvAUECFundCommissionRule.SingleRule.RuleID = (Guid)row[SingleRuleId];
                    cvAUECFundCommissionRule.SingleRule = commissionRule;
                }

                if (row[BasketRuleId] != System.DBNull.Value)
                {
                    ruleID = (Guid)row[BasketRuleId];
                    commissionRule = commissionRulesCacheManager.GetCommissionRuleByRuleId(ruleID);
                    //cvAUECFundCommissionRule.BasketRule.RuleID = (Guid)(row[BasketRuleId]);
                    //As there is no need of set _tempCommRuleSelect:Am
                    //if (commissionRule == null)
                    //{
                    //    cvAUECFundCommissionRule.BasketRule = _tempCommRuleSelect;
                    //}
                    //else
                    {
                        cvAUECFundCommissionRule.BasketRule = commissionRule;
                    }
                }
                if (row[CompanyID] != System.DBNull.Value)
                {
                    cvAUECFundCommissionRule.CompanyID = int.Parse(row[CompanyID].ToString());
                }

                if (row[CounterPartyID] != System.DBNull.Value)
                {
                    cvAUECFundCommissionRule.CounterPartyId = int.Parse(row[CounterPartyID].ToString());
                }

                if (row[VenueID] != System.DBNull.Value)
                {
                    cvAUECFundCommissionRule.VenueId = int.Parse(row[VenueID].ToString());
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
            return cvAUECFundCommissionRule;
        }

        /// <summary>
        /// commission calculation methodlogy Pre or Post Allocation 
        /// </summary>
        /// <returns></returns>

        public static bool GetCommissionCalculationTime()
        {

            bool result = false;
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetCommissionCalculationTime"))
                {
                    if (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int offSet = 0;
                        if (row[offSet] != System.DBNull.Value)
                        {
                            result = bool.Parse(row[offSet].ToString());
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

            return result;
        }

        #endregion Commission Rules Cache

    }
}


