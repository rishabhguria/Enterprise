using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CompanyManager
    {
        
        /// <summary>
        /// Gets all <see cref="Companies"/> from datatbase.
        /// </summary>
        /// <returns><see cref="Companies"/> fetched.</returns>
        public static SortableSearchableList<CompanyNameID> GetCompanyNameIDList()
        {
            SortableSearchableList<CompanyNameID> companies = new SortableSearchableList<CompanyNameID>();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "PMGetCompanyNameIDList"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companies.Add(FillCompanyNameID(row, 0));
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

                //if (rethrow)
                //{
                 throw;
                //}
            }
            #endregion
            return companies;
        }



        /// <summary>
        /// Fills the company name ID.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static CompanyNameID FillCompanyNameID(object[] row, int offset)
        { 
            if (offset < 0)
            {
                offset = 0;
            }
            CompanyNameID company = null;
            try
            {
                if (row != null)
                {
                    company = new CompanyNameID();
                    int ID = offset + 0;
                    int fullName = offset + 1;
                    int ShortName = offset + 2;

                    company.ID = Convert.ToInt32(row[ID]);
                    company.FullName = Convert.ToString(row[fullName]);
                    company.ShortName = Convert.ToString(row[ShortName]);                    
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

                //if (rethrow)
                //{
                 throw;
                //}
            }
            #endregion
            return company;
        
        }

        /// <summary>
        /// Gets the List of All Company Types.
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<CompanyType> GetCompanyTypeList()
        {
            SortableSearchableList<CompanyType> companyTypeList = new SortableSearchableList<CompanyType>();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetCompanyTypes"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyTypeList.Add(FillCompanyList(row, 0));
                        //dataSourceTypes.Add(FillDataSourceType(row, 0));
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

                //if (rethrow)
                //{
                throw;
                //}
            }
            #endregion
            return companyTypeList;
        }

        /// <summary>
        /// Fills the Company Type.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static CompanyType FillCompanyList(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            CompanyType companyType = null;
            try
            {
                if (row != null)
                {
                    companyType = new CompanyType();
                    int companyTypeID = offset + 0;
                    int companyTypeName = offset + 1;

                    companyType.CompanyTypeID = Convert.ToInt32(row[companyTypeID]);
                    companyType.Type = Convert.ToString(row[companyTypeName]);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

                //if (rethrow)
                //{
                throw;
                //}
            }
            #endregion
            return companyType;
        }


        /// <summary>
        /// Gets the List of All user for this Company .
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<User> GetCompanyUserList(int companyID)
        {
            SortableSearchableList<User> usersList = new SortableSearchableList<User>();

            Database db = DatabaseFactory.CreateDatabase();

            try
            {
                System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMGetCompanyUser");
                db.AddInParameter(commandSP, "@companyID", DbType.Int32, companyID);
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(commandSP))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        usersList.Add(FillUser(row, 0));
                        
                    }
                }
                usersList.Insert(0, new User("--select--", "--Select--"));
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

                //if (rethrow)
                //{
                throw;
                //}
            }
            #endregion
            return usersList;
        }

        /// <summary>
        /// Fills the User.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static User FillUser(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            User user = null;
            try
            {
                if (row != null)
                {
                    user = new User();
                    int login = offset + 0;
                    int name = offset + 1;
                    int password = offset + 2;

                    user.ID = Convert.ToString(row[login]);
                    user.UserName = Convert.ToString(row[name]);
                    user.Password = Convert.ToString(row[password]);
                    
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

                //if (rethrow)
                //{
                throw;
                //}
            }
            #endregion
            return user;
        }

        /// <summary>
        /// Gets the company details for ID.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <returns></returns>
        public static Company GetCompanyDetailsForID(int companyID)
        {

            Company companyDetails = new Company();


            #region dummy data

            companyDetails.CompanyNameID.ID = 1;
            companyDetails.CompanyNameID.FullName = "Nirvana Financial Solutions";
            companyDetails.CompanyNameID.ShortName = "NFS";
            companyDetails.CompanyType.CompanyTypeID = 1;

            // Address details
            companyDetails.AddressDetails.Address1 = "NY";
            companyDetails.AddressDetails.Address2 = "NJ";
            companyDetails.AddressDetails.StateId = 1;
            companyDetails.AddressDetails.Zip = "132213";
            companyDetails.AddressDetails.CountryId = 1;
            companyDetails.AddressDetails.WorkNumber = "1232323";
            companyDetails.AddressDetails.FaxNumber = "132434";


            // Admin details
            companyDetails.AdminFirstName = "Sugandh";
            companyDetails.AdminLastName = "Jain";
            companyDetails.AdminTitle = "asa";
            companyDetails.AdminUser.ID = "";
            companyDetails.AdminUser.UserName = "";
            companyDetails.AdminUser.Password = "";
            companyDetails.AdminEmail = "adsad@nira.com";
            companyDetails.AdminWorkNumber = "3452534";
            companyDetails.AdminCellNumber = "3452534";
            companyDetails.AdminPagerNumber = "3452534";
            companyDetails.AdminHomeNumber = "3452534";
            companyDetails.AdminFaxNumber = "3452534";

            #endregion

            //
            //try
            //{
            //    Database db = DatabaseFactory.CreateDatabase();
            //    System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMGetCompanyDetailsForID");

            //    db.AddInParameter(commandSP, "@ID", DbType.Int32, companyID);


            //    using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(commandSP))
            //    {
            //        while (reader.Read())
            //        {
            //            object[] row = new object[reader.FieldCount];
            //            reader.GetValues(row);
            //            companyDetails = FillCompanyDetails(row, 0);

            //        }
            //    }
            //}
            //#region Catch
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

            //    //if (rethrow)
            //    //{
            //    throw;
            //    //}
            //}
            //#endregion

            return companyDetails;
        }

        /// <summary>
        /// Fills the data sources.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static Company FillCompanyDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Company company = null;
            try
            {
                if (row != null)
                {
                    company = new Company();
                    int ID = offset + 0;
                    int FullName = offset + 1;
                    int ShortName = offset + 2;
                    int companyTypeID = offset + 3;

                    // Address details
                    int Address1 = offset + 4;
                    int Address2 = offset + 5;
                    int CountryID = offset + 6;
                    int CountryName = offset + 7;
                    int StateID = offset + 8;
                    int State = offset + 9;
                    int Zip = offset + 10;                    
                    int WorkNumber = offset + 11;
                    int FaxNumber = offset + 12;

                    // Admin details
                    int AdminContactFirstName = offset + 13;
                    int AdminContactLastName = offset + 14;
                    int AdminContactTitle = offset + 15;
                    int AdminContactUserID = offset + 16;
                    int AdminContactLoginName = offset + 17;
                    int AdminContactPassword = offset + 18;
                    int AdminContactEmail = offset + 19;
                    int AdminContactWorkNumber = offset + 20;
                    int AdminContactCellNumber = offset + 21;
                    int AdminContactPagerNumber = offset + 22;
                    int AdminContactHomeNumber = offset + 23;
                    int AdminContactFaxNumber = offset + 24;                    

                    company.CompanyNameID.ID = Convert.ToInt32(row[ID]);
                    company.CompanyNameID.FullName = Convert.ToString(row[FullName]);
                    company.CompanyNameID.ShortName = Convert.ToString(row[ShortName]);
                    company.CompanyType.CompanyTypeID = Convert.ToInt32(row[companyTypeID]);                                      
                    
                    // Address details
                    company.AddressDetails.Address1 = Convert.ToString(row[Address1]);
                    company.AddressDetails.Address2 = Convert.ToString(row[Address2]);
                    company.AddressDetails.StateId = Convert.ToInt32(row[StateID]);
                    company.AddressDetails.Zip = Convert.ToString(row[Zip]);
                    company.AddressDetails.CountryId = Convert.ToInt32(row[CountryID]);
                    company.AddressDetails.WorkNumber = Convert.ToString(row[WorkNumber]);
                    company.AddressDetails.FaxNumber = Convert.ToString(row[FaxNumber]);

                    
                    // Admin details
                    
                    company.AdminFirstName = Convert.ToString(row[AdminContactFirstName]);
                    company.AdminLastName = Convert.ToString(row[AdminContactLastName]);
                    company.AdminTitle = Convert.ToString(row[AdminContactTitle]);
                    company.AdminUser.ID = Convert.ToString(row[AdminContactUserID]);
                    company.AdminUser.UserName = Convert.ToString(row[AdminContactLoginName]);
                    company.AdminUser.Password = Convert.ToString(row[AdminContactPassword]);                    
                    company.AdminEmail = Convert.ToString(row[AdminContactEmail]);
                    company.AdminWorkNumber = Convert.ToString(row[AdminContactWorkNumber]);
                    company.AdminCellNumber = Convert.ToString(row[AdminContactCellNumber]);
                    company.AdminPagerNumber = Convert.ToString(row[AdminContactPagerNumber]);
                    company.AdminHomeNumber = Convert.ToString(row[AdminContactHomeNumber]);
                    company.AdminFaxNumber = Convert.ToString(row[AdminContactFaxNumber]);
                    
                    

                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy");

                //if (rethrow)
                //{
                throw;
                //}
            }
            #endregion
            return company;
        }




        /// <summary>
        /// Gets the company application details for the company ID from the DataBase.
        /// </summary>
        /// <param name="companyID">The companyID.</param>
        /// <returns></returns>
        public static CompanyApplicationDetails GetCompanyApplicationDetailsForID(int companyID)
        {
            CompanyApplicationDetails companyApplicationDetails = new CompanyApplicationDetails();
            
            #region dummy data

            companyApplicationDetails.CompanyNameID.ID = companyID;
            companyApplicationDetails.CompanyNameID.FullName = "Nirvana Financial Solutions";
            companyApplicationDetails.CompanyNameID.ShortName = "NFS";            
            // other details 
            companyApplicationDetails.AllowDailyImport = true;
            companyApplicationDetails.AllowDataMapping = true;

            // datasources list for this company
            SortableSearchableList<DataSourceNameID> companyApplicationDataSourcesList = new SortableSearchableList<DataSourceNameID>();
            companyApplicationDataSourcesList.Add(new DataSourceNameID(1, "Goldman Sachs", "GS"));
            companyApplicationDataSourcesList.Add(new DataSourceNameID(32, "Merril Lynch", "ML"));            
            companyApplicationDetails.DataSourceNameIDList = companyApplicationDataSourcesList;

            companyApplicationDetails.MinimumRefreshRate = 5;
            companyApplicationDetails.PricingModel.ID = 1;
            companyApplicationDetails.PricingModel.Name = "WinDale";           

            #endregion

            //ToDo : We will have the database call which will fetch the data for this here.



            return companyApplicationDetails;
        }
    }
}
