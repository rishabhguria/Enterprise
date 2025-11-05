using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using Prana.Global;
using Prana.CommonDataCache;
using Prana.Utilities.XMLUtilities;
using System.Data.Common;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
namespace Prana.UDATool
{
     class DataManager
    {
         public static UDACollection GetUDAAttributeData(string spName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            UDACollection coll = new UDACollection();
            UDA uda;

            try
            {
                using (SqlDataReader dr = (SqlDataReader)db.ExecuteReader(spName))
                {
                    while (dr.Read())
                    {
                        object[] row = new object[dr.FieldCount];
                        dr.GetValues(row);
                        int udaID = Int32.Parse(row[1].ToString());
                        if (udaID > -1)
                        {
                            uda = new UDA();
                            uda.Name = row[0].ToString();
                            uda.ID = udaID;
                            coll.Add(uda); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    
                }
            }
            return coll;
        }

         public static string GetSymbolUdaData(string symbol)
         {
             string companyName = string.Empty;
             Database db = DatabaseFactory.CreateDatabase();
             object[] parameter = new object[1];
             parameter[0] = symbol;
             try
             {

                 object obj = db.ExecuteScalar("P_GetSymbolUdaData", parameter);
                 if (obj != null)
                 {
                     companyName = obj.ToString();
                 }
                 
             }
             catch (Exception ex)
             {
                 bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                 if (rethrow)
                 {
                     throw;
                 }
             }
             return companyName;
         }

        
         public static void SaveInformation(string str, UDACollection udaCollection)
         {
             try
             {
                 Database db = DatabaseFactory.CreateDatabase();
                 foreach (UDA uda in udaCollection)
                 {
                    
                     object[] parameter = new object[2];
                     parameter[0] = uda.Name;
                     parameter[1] = uda.ID;
                     db.ExecuteNonQuery(str, parameter);
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
         public static void DeleteInformation(string str, List<int> deletedIDS)
         {
             try
             {
                 if (deletedIDS.Count.Equals(0))
                     return;
                 foreach (int i in deletedIDS)
                 {
                     Database db = DatabaseFactory.CreateDatabase();
                     object[] parameter = new object[1];
                     parameter[0] = i;

                     try
                     {
                         db.ExecuteNonQuery(str, parameter);
                     }
                     catch (Exception ex)
                     {
                         bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                         if (rethrow)
                         {
                             MessageBox.Show(ex.Message);
                         }
                     }
                 }
                 deletedIDS.Clear();
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



         internal static List<int> GetInUseUDAIDs(string spName)
         {
             Database db = DatabaseFactory.CreateDatabase();
             List<int> udaIDList = new List<int>();

             try
             {
                 using (SqlDataReader dr = (SqlDataReader)db.ExecuteReader(spName))
                 {
                     while (dr.Read())
                     {
                         object[] row = new object[dr.FieldCount];
                         dr.GetValues(row);
                         int udaID = Int32.Parse(row[0].ToString());
                         if (udaID > 0)
                         {
                             udaIDList.Add(udaID);
                         }
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
             return udaIDList;
         }
     }
}
