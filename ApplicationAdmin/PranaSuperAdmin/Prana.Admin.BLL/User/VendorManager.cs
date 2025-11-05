using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for VendorManager.
    /// </summary>
    public class VendorManager
    {
        private VendorManager()
        {
        }

        public static Vendor FillVendors(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int vendorName = 1 + offSet;
            int lastName = 2 + offSet;
            int firstName = 3 + offSet;
            int shortName = 4 + offSet;
            int title = 5 + offSet;
            int product = 6 + offSet;
            int comment = 7 + offSet;
            int mailingAddress = 8 + offSet;
            int eMail = 9 + offSet;
            int telphoneWork = 10 + offSet;
            int telphoneHome = 11 + offSet;
            int telphoneMobile = 12 + offSet;
            int pager = 13 + offSet;
            int fax = 14 + offSet;
            int address1 = 15 + offSet;
            int address2 = 16 + offSet;

            Vendor vendor = new Vendor();
            try
            {
                if (row[ID] != null)
                {
                    vendor.VendorID = int.Parse(row[ID].ToString());
                }
                if (row[vendorName] != null)
                {
                    vendor.VendorName = row[vendorName].ToString();
                }
                if (row[lastName] != null)
                {
                    vendor.LastName = row[lastName].ToString();
                }
                if (row[firstName] != null)
                {
                    vendor.FirstName = row[firstName].ToString();
                }
                if (row[shortName] != null)
                {
                    vendor.ShortName = row[shortName].ToString();
                }
                if (row[title] != null)
                {
                    vendor.Title = row[title].ToString();
                }
                if (row[product] != null)
                {
                    vendor.Product = row[product].ToString();
                }
                if (row[comment] != null)
                {
                    vendor.Comment = row[comment].ToString();
                }
                if (row[mailingAddress] != null)
                {
                    vendor.MailingAddress = row[mailingAddress].ToString();
                }
                if (row[eMail] != null)
                {
                    vendor.EMail = row[eMail].ToString();
                }
                if (row[telphoneWork] != null)
                {
                    vendor.TelephoneWork = row[telphoneWork].ToString();
                }
                if (row[telphoneHome] != null)
                {
                    vendor.TelephoneHome = row[telphoneHome].ToString();
                }
                if (row[telphoneMobile] != null)
                {
                    vendor.TelephoneMobile = row[telphoneMobile].ToString();
                }
                if (row[pager] != null)
                {
                    vendor.TelephonePager = row[pager].ToString();
                }
                if (row[fax] != null)
                {
                    vendor.Fax = row[fax].ToString();
                }
                if (row[address1] != null)
                {
                    vendor.Address1 = row[address1].ToString();
                }
                if (row[address2] != null)
                {
                    vendor.Address2 = row[address2].ToString();
                }
            }
            #region Catch
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
            #endregion
            return vendor;
        }

        public static Vendors GetVendors()
        {
            Vendors vendors = new Vendors();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllVendors";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        vendors.Add(FillVendors(row, 0));
                    }
                }
            }
            #region Catch
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
            #endregion
            return vendors;
        }

        public static Vendor GetVendor(int vendorID)
        {
            Vendor vendor = new Vendor();

            object[] parameter = new object[1];
            parameter[0] = vendorID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetVendor", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        vendor = FillVendors(row, 0);
                    }
                }
            }
            #region Catch
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
            #endregion
            return vendor;
        }

        public static int SaveVendor(Vendor vendor)
        {
            int result = int.MinValue;

            object[] parameter = new object[18];
            parameter[0] = vendor.VendorName;
            parameter[1] = vendor.FirstName;
            parameter[2] = vendor.LastName;
            parameter[3] = vendor.ShortName;
            parameter[4] = vendor.Product;
            parameter[5] = vendor.EMail;
            parameter[6] = vendor.TelephoneWork;
            parameter[7] = vendor.TelephoneHome;
            parameter[8] = vendor.TelephoneMobile;
            parameter[9] = vendor.TelephonePager;
            parameter[10] = vendor.Address1;
            parameter[11] = vendor.Address2;
            parameter[12] = vendor.Fax;
            parameter[13] = vendor.Title;
            parameter[14] = vendor.Comment;
            parameter[15] = vendor.MailingAddress;
            parameter[16] = vendor.VendorID;
            parameter[17] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveVendorDetail", parameter).ToString());
            }
            #region Catch
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
            #endregion
            return result;
        }

        //		private static bool InsertVendor(Vendor vendor)
        //		{
        //			//TODO:
        //			return false;
        //		}

        //		private static bool UpdateVendor(Vendor vendor)
        //		{
        //			//TODO:
        //			return false;
        //		}

        public static bool DeleteVendor(int vendorID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = vendorID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteVendor", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            #region Catch
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
            #endregion
            return result;
        }
    }
}
