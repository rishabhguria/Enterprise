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
    class DataSourceManager
    {

        
        /// <summary>
        /// insert user in DB
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns>string message Showing whether the insert was successfull or not.</returns>
        public static int AddDataSource(DataSourceNameID dataSourceName)
        {
            int result = 0;
            string dbMessage = string.Empty;
            try
            {
                
                Database db = DatabaseFactory.CreateDatabase();
                System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMAddNewDataSource");

                db.AddInParameter(commandSP, "@FullName", DbType.String, dataSourceName.FullName);
                db.AddInParameter(commandSP, "@ShortName", DbType.String, dataSourceName.ShortName);
                db.AddOutParameter(commandSP, "@Error", DbType.Int32,0);
                db.AddOutParameter(commandSP, "@ErrorMessage", DbType.String, 100);
                         
                int numberOfRowsEffected = db.ExecuteNonQuery(commandSP); 
                
                result = Convert.ToInt32(commandSP.Parameters["@Error"].Value);
                dbMessage = Convert.ToString(commandSP.Parameters["@ErrorMessage"].Value);                
                
            }
            
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

            return result;
        }

        public static int SaveDataSourceData(DataSource dataSourceDetails)
        {
            int result = 0;
            string dbMessage = string.Empty;
            try
            {

                //Database db = DatabaseFactory.CreateDatabase();
                //System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMUpdateDataSourceData");

                //db.AddInParameter(commandSP, "@FullName", DbType.String, dataSource.FullName);
                //db.AddInParameter(commandSP, "@ShortName", DbType.String, dataSource.ShortName);
                //db.AddOutParameter(commandSP, "@Error", DbType.Int32, 0);
                //db.AddOutParameter(commandSP, "@ErrorMessage", DbType.String, 100);

                //int numberOfRowsEffected = db.ExecuteNonQuery(commandSP);

                //result = Convert.ToInt32(commandSP.Parameters["@Error"].Value);
                //dbMessage = Convert.ToString(commandSP.Parameters["@ErrorMessage"].Value);

            }

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

            return result;
        }

        /// <summary>
        /// Gets the data sources.
        /// </summary>
        /// <returns> SortableSearchableList<DataSourceNameID> </returns>
        public static SortableSearchableList<DataSourceNameID>  GetAllDataSourceNames()
        {
            Database db = DatabaseFactory.CreateDatabase();
            SortableSearchableList<DataSourceNameID> dataSourceList = new SortableSearchableList<DataSourceNameID>();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "PMGetAllDataSourceNames"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceList.Add(FillDataSourcesNameID(row, 0));
                        
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
            return dataSourceList;
        }

        /// <summary>
        /// Fills the data sources.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static DataSourceNameID FillDataSourcesNameID(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            DataSourceNameID dataSource = null;
            try
            {
                if (row != null)
                {
                    dataSource = new DataSourceNameID();
                    int DATASOURCEID = offset + 0;
                    int DATASOURCENAME = offset + 1;
                    int DATASOURCESHORTNAME = offset + 2;

                    dataSource.ID = Convert.ToInt32(row[DATASOURCEID].ToString());
                    dataSource.FullName= Convert.ToString(row[DATASOURCENAME]);
                    dataSource.ShortName = Convert.ToString(row[DATASOURCESHORTNAME]);
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
            return dataSource;
        }


        /// <summary>
        /// Gets the data source types.
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<DataSourceType> GetDataSourceTypes()
        {
            SortableSearchableList<DataSourceType> dataSourceTypeList = new SortableSearchableList<DataSourceType>();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "PMGetDataSourceTypes"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceTypeList.Add(FillDataSourceType(row, 0));
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
            return dataSourceTypeList;
        }

        /// <summary>
        /// Fills the type of the data source.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static DataSourceType FillDataSourceType(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            DataSourceType dataSourceType = null;
            try
            {
                if (row != null)
                {
                    dataSourceType = new DataSourceType();
                    int DATASOURCETYPEID = offset + 0;
                    int DATASOURCETYPENAME = offset + 1;

                    dataSourceType.DataSourceTypeID = Convert.ToInt32(row[DATASOURCETYPEID]);
                    dataSourceType.DataSourceTypeName = Convert.ToString(row[DATASOURCETYPENAME]);
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
            return dataSourceType;
        }

        /// <summary>
        /// Gets the List of All Countries.
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<Country> GetCountryList()
        {
            SortableSearchableList<Country> countryList = new SortableSearchableList<Country>();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetCountries"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        countryList.Add(FillCountryList(row, 0));
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
            return countryList;
        }

        /// <summary>
        /// Fills the Country.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static Country FillCountryList(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Country country = null;
            try
            {
                if (row != null)
                {
                    country = new Country();
                    int CountryID = offset + 0;
                    int CountryName = offset + 1;

                    country.CountryID = Convert.ToInt32(row[CountryID]);
                    country.Name = Convert.ToString(row[CountryName]);
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
            return country;
        }

        
        


        /// <summary>
        /// Gets the List of All States.
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<State> GetStateList()
        {
            SortableSearchableList<State> stateList = new SortableSearchableList<State>();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetStates"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        stateList.Add(FillState(row, 0));
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
            return stateList;
        }

        

        /// <summary>
        /// Fills the State.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static State FillState(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            State state = null;
            try
            {
                if (row != null)
                {
                    state = new State();
                    int stateID = offset + 0;
                    int stateName = offset + 1;
                    int countryID = offset + 2;

                    state.StateID = Convert.ToInt32(row[stateID]);
                    state.StateName = Convert.ToString(row[stateName]);
                    state.CountryID = Convert.ToInt32(row[countryID]);
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
            return state;
        }

        /// <summary>
        /// Gets the data source details for ID.
        /// </summary>
        /// <returns></returns>
        public static DataSource GetDataSourceDetailsForID()
        {
            return new DataSource();
        }

        /// <summary>
        /// Gets the data source details for ID.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <returns></returns>
        public static DataSource GetDataSourceDetailsForID(int dataSourceID)
        {
            
            DataSource dataSourceDetails = new DataSource();
            try
            {
                Database db = DatabaseFactory.CreateDatabase();    
                System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMGetDataSourceDetailsForID");

                db.AddInParameter(commandSP, "@ID", DbType.Int32, dataSourceID);
                

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(commandSP))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceDetails = FillDataSourceDetails(row, 0);
                        
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
            
            return dataSourceDetails;
        }

        /// <summary>
        /// Fills the data sources.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static DataSource FillDataSourceDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            DataSource dataSource = null;
            try
            {
                if (row != null)
                {
                    dataSource = new DataSource();
                    int ID = offset + 0;
                    int FullName = offset + 1;
                    int ShortName = offset + 2;
                    int SourceTypeID = offset + 3;
                    int Address1 = offset + 4;
                    int Address2 = offset + 5;
                    int StateID = offset + 6;
                    int State = offset + 7;
                    int Zip = offset + 8;
                    int CountryID = offset + 9;
                    int CountryName = offset + 10;
                    int WorkNumber = offset + 11;
                    int FaxNumber = offset + 12;
                    int PrimaryContactFirstName = offset + 13;
                    int PrimaryContactLastName = offset + 14; 
                    int PrimaryContactTitle = offset + 15;  
                    int PrimaryContactEmail = offset + 16;  
                    int PrimaryContactCellNumber = offset + 17;  
                    int PrimaryContactWorkNumber = offset + 18;
                    int StatusID = offset + 19;                    

                    dataSource.DataSourceNameID.ID = Convert.ToInt32(row[ID]);
                    dataSource.DataSourceNameID.FullName = Convert.ToString(row[FullName]);
                    dataSource.DataSourceNameID.ShortName = Convert.ToString(row[ShortName]);
                    dataSource.TypeID = Convert.ToInt32(row[SourceTypeID]);
                    dataSource.DataSourceAddressDetails.Address1 = Convert.ToString(row[Address1]);
                    dataSource.DataSourceAddressDetails.Address2 = Convert.ToString(row[Address2]);
                    dataSource.DataSourceAddressDetails.StateId = Convert.ToInt32(row[StateID]);
                    dataSource.DataSourceAddressDetails.Zip = Convert.ToString(row[Zip]);
                    dataSource.DataSourceAddressDetails.CountryId = Convert.ToInt32(row[CountryID]);
                    dataSource.DataSourceAddressDetails.WorkNumber = Convert.ToString(row[WorkNumber]);
                    dataSource.DataSourceAddressDetails.FaxNumber = Convert.ToString(row[FaxNumber]);                    
                    dataSource.PrimaryContact.FirstName = Convert.ToString(row[PrimaryContactFirstName]);
                    dataSource.PrimaryContact.LastName = Convert.ToString(row[PrimaryContactLastName]);
                    dataSource.PrimaryContact.Title = Convert.ToString(row[PrimaryContactTitle]);
                    dataSource.PrimaryContact.EMail = Convert.ToString(row[PrimaryContactEmail]);
                    dataSource.PrimaryContact.CellNumber = Convert.ToString(row[PrimaryContactCellNumber]);
                    dataSource.PrimaryContact.WorkNumber = Convert.ToString(row[PrimaryContactWorkNumber]);
                    dataSource.StatusID = Convert.ToInt16(row[StatusID]);                    
                    
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
            return dataSource;
        }


        /// <summary>
        /// Gets the List of All Status.
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<DataSourceStatus> GetStatusList()
        {
            SortableSearchableList<DataSourceStatus> statusList = new SortableSearchableList<DataSourceStatus>();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "PMGetStatusList"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        statusList.Add(FillStatus(row, 0));
                       
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
            return statusList;
        }

        /// <summary>
        /// Fills the Status.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static DataSourceStatus FillStatus(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            DataSourceStatus status = null;
            try
            {
                if (row != null)
                {
                    status = new DataSourceStatus();
                    int statusID = offset + 0;
                    int Name= offset + 1;

                    status.StatusId = Convert.ToInt32(row[statusID]);
                    status.Name = Convert.ToString(row[Name]);
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
            return status;
        }

        /// <summary>
        /// Gets the import set UP for ID.
        /// Sugandh - returning dummy data.. the constructor for importSetUp initialises dummy data for now. 
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <returns></returns>
        public static ImportSetup GetImportSetUPForID(int dataSourceID)
        {
            ImportSetup importSetUP = new ImportSetup();

            //Sugandh - returning dummy data.. the constructor for importSetUp initialises dummy data for now. 
            DataSourceNameID dataSourceNameID = new DataSourceNameID(dataSourceID, "Dummy");
            importSetUP.DataSourceNameID = dataSourceNameID;

            return importSetUP;
        }

        /// <summary>
        /// Gets the map columns for datasource ID.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <returns></returns>
        public static MapColumns GetMapColumnsForDatasourceID(int dataSourceID)
        {
            //MapColumns mapColumns = new MapColumns();

            
            ////Ram - returning dummy data.. the constructor for mapColumns initialises dummy data for now. 
            //DataSourceNameID dataSourceNameID = new DataSourceNameID(dataSourceID, "Dummy :: "+ dataSourceID.ToString() );
            //mapColumns.DataSourceNameID = dataSourceNameID;

            
            ////To Do: paas dataSourceID in Retrive and fetch DataSource specific mappings!
            //mapColumns.MappingItemList = MappingItemList.Retrieve(dataSourceID);

            return null;
        }
    }
}
