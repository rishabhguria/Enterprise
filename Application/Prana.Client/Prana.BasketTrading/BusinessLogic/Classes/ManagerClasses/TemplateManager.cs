using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using Prana.Global;

namespace Prana.BasketTrading
{
    public class TemplateManager
    {
        public static void SaveTemplate(BasketTemplate basketTemplate)
        {
           // TemplateDataManager.SaveConventionMappingList(basketTemplate.TemplateConvention);                                              
            Database db = DatabaseFactory.CreateDatabase();
            
            object[] parameter = new object[4];
            parameter[0] = basketTemplate.TemplateID; 
            parameter[1] = basketTemplate.TemplateName;
            parameter[2] = basketTemplate.GetColumns();
            //parameter[3] = basketTemplate.AssetID;
           // parameter[4] = basketTemplate.UnderLyingID;
            parameter[3] = basketTemplate.IsDefaultTemplate.ToString().ToUpper();
            //parameter[5] = basketTemplate.TemplateExchangeID;
            //parameter[6] = basketTemplate.ConventionMappingID;
            //parameter[7] = basketTemplate.OrderSideMappingID;            
            #region try
            try
            {
                db.ExecuteScalar("P_BTSaveTemplateList", parameter);
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

        public static void DeleteTemplate(string templateID)
        {             
            Database db = DatabaseFactory.CreateDatabase();
            
            object[] parameter = new object[1];
            parameter[0] = templateID;
            #region try
                try
                {
                    db.ExecuteScalar("P_BTDeleteTemplate", parameter);
                }
            #endregion

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
        
        public static void DeleteOrderSideMapping(string templateID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[1];
            parameter[0] = templateID;


            #region try
            try
            {
                db.ExecuteScalar("P_BTDeleteTempOrderMapping", parameter);
            }
            #endregion

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

        public static void DeleteExchangeMapping(string templateID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[1];
            parameter[0] = templateID;


            #region try
            try
            {
                db.ExecuteScalar("P_BTDeleteExchangeMapping", parameter);
            }
            #endregion

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

        public static Int32 IsCurrentlyInUse(string templateID)
        {
            Int32 count = 0;
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = templateID;

                count = (Int32)db.ExecuteScalar("BTCheckTemplateReference", parameter);
                
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
            return count;

        }

        // Make Stored Procedures and
        // uncomment if required 

        //private static void DeleteConventionMapping(string templateID)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    object[] parameter = new object[1];
        //    parameter[0] = templateID;


        //    #region try
        //    try
        //    {
        //        db.ExecuteScalar("P_BTDeleteTempConventionMapping", parameter);
        //    }
        //    #endregion

        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);


        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //}
        //private static void DeleteExchangeMapping(string templateID)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    object[] parameter = new object[1];
        //    parameter[0] = templateID;


        //    #region try
        //    try
        //    {
        //        db.ExecuteScalar("P_BTDeleteTempExchangeMapping", parameter);
        //    }
        //    #endregion

        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);


        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //}
    }
}
