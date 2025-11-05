using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using Prana.Global;
using Prana.Interfaces;
using Prana.BusinessObjects;
using System.Collections;
using Prana.ClientCommon;
namespace Prana.BasketTrading
{
    class TemplateBasketDataManager
    {
        public enum BasketOrderType
        {
            TemplateBasket,
            UpLoadedBasket,
            Wave,
            Group
        }

        /// <summary>
        /// saves Template Basket 
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="sharedTradingAccounts"></param>
        /// <returns></returns>
        public static bool SaveTemplateBasket(BasketDetail basket)
        {

            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[8];
            parameter[0] = basket.UpLoadedBasketID;
            parameter[1] = basket.BasketName;
            parameter[2] = basket.TemplateID;
            parameter[3] = basket.TradingAccountVisible;
            parameter[4] = basket.AssetID;
            parameter[5] = basket.UnderLyingID;
            parameter[6] = basket.UserID;
           // parameter[7] = GeneralUtilities.GetStringFromList(basket.DisplayColumnList,',');
            parameter[7] = basket.BanchMarkValue;
            
            try
            {
                DeleteTemplateBasketOrders(basket.UpLoadedBasketID);
                db.ExecuteScalar("P_BTSaveTemplateBasket", parameter);
                //Save Individual Orders 
                SaveOrders(basket.UpLoadedBasketID,basket.BasketOrders,BasketOrderType.TemplateBasket);
                //if (sharedTradingAccounts != null)
                //{
                //    DeleteSharedTradingAccounts(basket.UpLoadedBasketID);
                //    SaveSharedTradingAccounts(basket.UpLoadedBasketID, sharedTradingAccounts);
                //}
                
                return true;
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return false ;
            }
            #endregion
            

        }
        /// <summary>
        /// Saves Template Basket Orders, Uploaded,WAve and Group Orders
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="orders"></param>
        public static void SaveOrders(string ID,OrderCollection orders, BasketOrderType basketOrderType)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                object[] parameter = new object[24];
                parameter[0] = ID;


                foreach (Order order in orders)
                {
                    //SavedOrderID 
                    parameter[1] = order.ClientOrderID;
                    parameter[2] = order.OrderSideTagValue.Trim();
                    parameter[3] = order.Symbol;
                    parameter[4] = order.ExchangeID;
                    parameter[5] = order.Quantity;
                    parameter[6] = order.AvgPrice;
                    parameter[7] = order.TradingAccountID;
                    parameter[8] = order.AUECID;
                    parameter[9] = order.AssetID;
                    parameter[10] = order.UnderlyingID;
                    parameter[11] = order.CompanyUserID;
                    parameter[12] = order.Price;
                    parameter[13] = order.ExecutionInstruction;
                    parameter[14] = order.OrderTypeTagValue.Trim();
                    parameter[15] = order.TIF;
                    parameter[16] = order.CounterPartyID;
                    parameter[17] = order.VenueID;
                    parameter[18] = (order.DiscretionOffset == double.Epsilon) ? 0 : order.DiscretionOffset;
                    parameter[19] = (order.PegDifference == double.Epsilon) ? 0 : order.PegDifference;
                    parameter[20] = (order.StopPrice == double.Epsilon) ? 0.0 : order.StopPrice;
                    parameter[21] = order.ParentClientOrderID;
                    parameter[22] = order.Level1ID;
                    parameter[23] = order.Level2ID;

                    if (basketOrderType == BasketOrderType.TemplateBasket)
                    {
                        db.ExecuteScalar("P_BTSavePortfolioBasketOrders", parameter);
                    }
                    else if (basketOrderType == BasketOrderType.UpLoadedBasket)
                    {
                        db.ExecuteScalar("P_BTSaveUploadedBasketOrders", parameter);
                    }
                    else if (basketOrderType == BasketOrderType.Wave)
                    {
                        db.ExecuteScalar("P_BTSaveWaveOrders", parameter);
                    }
                    else if (basketOrderType == BasketOrderType.Group)
                    {
                        db.ExecuteScalar("P_BTSaveGroupOrders", parameter);
                    }
                    
                    
                    
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// For Updating Order Details
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="basketOrderType"></param>
        public static void UpdateOrder(Order order, BasketOrderType basketOrderType)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                object[] parameter = new object[10];
                    //SavedOrderID 
                    parameter[0] = order.ParentClientOrderID;
                    parameter[1] = order.OrderSideTagValue.Trim();
                    parameter[2] = order.Symbol;
                    parameter[3] = order.Quantity;
                    parameter[4] = order.Price;
                    parameter[5] = order.OrderTypeTagValue.Trim();
                    parameter[6] = order.CounterPartyID;
                    parameter[7] = order.VenueID;
                    parameter[8] = order.Level2ID;
                    parameter[9] = order.Level1ID;


                    //if (basketOrderType == BasketOrderType.TemplateBasket)
                    //{
                    //    db.ExecuteScalar("P_BTUpdatePortfolioBasketOrders", parameter);
                    //}
                    if (basketOrderType == BasketOrderType.UpLoadedBasket)
                    {
                        db.ExecuteScalar("P_BTUpdateUploadedBasketOrder", parameter);
                    }
                    //else if (basketOrderType == BasketOrderType.Wave)
                    //{
                    //    db.ExecuteScalar("P_BTUpdateWaveOrders", parameter);
                    //}
                    //else if (basketOrderType == BasketOrderType.Group)
                    //{
                    //    db.ExecuteScalar("P_BTUpdateGroupOrders", parameter);
                    //}

            }
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

        }
        public  static void UpdateOrders(OrderCollection orders, BasketOrderType basketOrderType)
        {
            try
            {
                foreach (Order order in orders)
                {
                    UpdateOrder(order, basketOrderType);
                }
            }
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

        }

        /// <summary>
        /// returns template Basket 
        /// </summary>
        /// <param name="basketID"></param>
        /// <returns></returns>
        public static BasketDetail GetTemplateBasketDetails(string basketID)
        {
            BasketDetail basket = new BasketDetail();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = basketID;


                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetSavedBasketByBasketID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        basket.UpLoadedBasketID = row[0].ToString();
                        basket.BasketName = row[1].ToString();
                        basket.TemplateID = row[2].ToString();
                        basket.AssetID = int.Parse(row[3].ToString());
                        basket.UnderLyingID = int.Parse(row[4].ToString());
                        basket.UserID = int.Parse(row[5].ToString());
                        basket.DisplayColumnList = TemplateDataManager.GetTemplateColumns(basket.TemplateID);
                            //GeneralUtilities.GetListFromString(row[8].ToString().Trim(),',');
                        string benchMarkValue = row[6].ToString().Trim();
                        if (benchMarkValue != string.Empty)
                            basket.BanchMarkValue = double.Parse(benchMarkValue);
                       

                    }
                    basket.BasketOrders=GetTemplateBasketOrders(basketID);

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

            return basket;
        }

        /// <summary>
        /// For rerieving Basket Orders
        /// </summary>
        /// <param name="basketID"></param>
        /// <returns></returns>
        private static OrderCollection GetTemplateBasketOrders(string basketID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            OrderCollection orders = new OrderCollection();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = basketID;

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetTemplateBasketOrders", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orders.Add(BasketDataManager.FillTemplateOrder(row, 0));
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

        public static void DeleteSavedBasket(string basketID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = basketID;
                DeleteTemplateBasketOrders(basketID);
                db.ExecuteScalar("P_BTDeleteSavedBasket", parameter);

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
        /// Delete Template Basket Orders
        /// </summary>
        /// <param name="basketID"></param>
        public static void DeleteTemplateBasketOrders(string basketID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = basketID;
                db.ExecuteScalar("P_BTDeleteSavedBasketOrders", parameter);
            }
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
        }

        public static void DeleteUploadedBasketOrder(string clientOrderID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = clientOrderID;
                db.ExecuteScalar("P_BTDeleteUploadedBasketOrders", parameter);
            }
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
        }

        public static void UpdateUploadedBasket(BasketDetail basket)
        {
            try
            {

                UpdateOrders(basket.BasketOrders, BasketOrderType.UpLoadedBasket);
            }
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
        }

        public static DataTable GetAllSavedBaskets()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BasketID");
            dt.Columns.Add("BasketName");

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetSavedBaskets"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(row);
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
            return dt;
        }
       
        /// <summary>
        /// Saves UpLoaded Basket and Its Orders
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static bool SaveUploadedBasket(BasketDetail basket)
        {

            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[11];
            parameter[0] = basket.BasketID;
            parameter[1] = basket.UpLoadedBasketID;
            parameter[2] = basket.HasWaves;
            parameter[3] = basket.UserID;
            parameter[4] = "False";
            parameter[5] = basket.BasketName;
            parameter[6] =DateTime.Now.ToUniversalTime();
            parameter[7] = basket.AssetID;
            parameter[8] = basket.UnderLyingID;
            parameter[9] = basket.TemplateID;
            parameter[10] = basket.BanchMarkValue;
           

            try
            {
                db.ExecuteScalar("BT_SaveUploadedBasketDetails", parameter);
                //Save Individual Orders 
                SaveOrders(basket.BasketID, basket.BasketOrders, BasketOrderType.UpLoadedBasket);
                return true;
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return false;
            }
            #endregion


        }
      
        // Related to Waves
        /// <summary>
        /// Saves Details of Waves corresponding to a Basket 
        /// and waves orders
        /// </summary>
        /// <param name="basket"></param>
        public static void SaveWavesDetail(Waves newAddedwaves)
        {
           
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[3];
           

            try
            {
                foreach (Wave wave in newAddedwaves)
                {
                    parameter[0] = wave.WaveID;
                    parameter[1] = wave.BasketID;                    
                    parameter[2] = wave.Percentage;
                    db.ExecuteScalar("P_BTSaveWaveDetails", parameter);
                    SaveOrders(wave.WaveID, wave.WaveOrders, BasketOrderType.Wave);
                }
                
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
              
            }
            #endregion
        }


        // Related to Traded Basket
        public static DataTable GetTradedBasketIDS(string savedBasketID, int userID, DateTime date)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BasketID");
            dt.Columns.Add("TradedBasketID");
            dt.Columns.Add("Traded Time");
            dt.Columns.Add("BasketName");
           
            Database db = DatabaseFactory.CreateDatabase();
            
            try
            {
                object[] parameter = new object[3];
                parameter[0] = savedBasketID;
                parameter[1] = userID;
                parameter[2] = date;              
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetTradedBasketIDS", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(row);
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
            return dt;
        }
        /// <summary>
        /// returns UploadedBasket IDName Collection
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetUploadedBasketIDS(int userID, DateTime date)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BasketID");
            dt.Columns.Add("BasketName");
            dt.Columns.Add("TimeOfSave");
            dt.Columns.Add("Traded Time");
            dt.Columns.Add("UserID");
            dt.Columns.Add("UserName");
            Database db = DatabaseFactory.CreateDatabase();

            try
            {
                object[] parameter = new object[2];
                parameter[0] = userID;
                parameter[1] = date;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("BT_GetUploadedBasketIDS", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(row);
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
            return dt;
        }

      
        
       
    }
}

