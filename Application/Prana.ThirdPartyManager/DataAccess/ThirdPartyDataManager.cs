using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Prana.ThirdPartyManager.DataAccess
{
    public static class ThirdPartyDataManager
    {
        /// <summary>
        /// Gets the third party gnu PG.
        /// </summary>
        /// <param name="gnuPGId">The gnu PG id.</param>
        /// <returns>The  GnuPG object</returns>
        /// <remarks></remarks>
        public static ThirdPartyGnuPGs GetThirdPartyGnuPGForDecryption(int gnuPGId)
        {
            return GetData<ThirdPartyGnuPGs, ThirdPartyGnuPG>("P_GetThirdPartyGnuPGForDecryption", gnuPGId);
        }

        /// <summary>
        /// Reads the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IList ReadItems<T>(IDataReader reader, IList list) where T : new()
        {
            while (reader.Read())
            {
                T entity = new T();
                Type entityType = typeof(T);

                for (int index = 0; index < reader.FieldCount; index++)
                {
                    string name = reader.GetName(index);
                    PropertyInfo info = entityType.GetProperty(name);

                    try
                    {
                        if (info != null && info.CanWrite && reader[index] != DBNull.Value)
                            info.SetValue(entity, reader[index], null);
                        else if (info == null)
                            Debug.Print("Can't find field {0}", name);
                    }
                    #region Catch
                    catch (Exception ex)
                    {
                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                        if (rethrow)
                        {
                            throw;
                        }
                        continue;
                    }
                    #endregion

                }
                list.Add(entity);
            }
            return list;
        }

        public static ThirdParty GetThirdParty(int thirdPartyID)
        {
            ThirdParty thirdParty = new ThirdParty();

            object[] parameter = new object[1];
            parameter[0] = thirdPartyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdParty", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdParty = FillThirdParties(row, 0);

                }
            }
            return thirdParty;
        }

        private static ThirdParty FillThirdParty(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ThirdParty thirdParty = new ThirdParty();

            if (row != null)
            {
                int THIRDPARTYID = offset + 0;
                int THIRDPARTYNAME = offset + 1;
                int DESCRIPTION = offset + 2;
                int THIRDPARTY_TYPEID = offset + 3;


                thirdParty.ThirdPartyID = Convert.ToInt32(row[THIRDPARTYID]);
                thirdParty.ThirdPartyName = Convert.ToString(row[THIRDPARTYNAME]);
                if (DBNull.Value != row[DESCRIPTION])
                {

                    thirdParty.Description = (row[DESCRIPTION].ToString());
                }
                thirdParty.ThirdPartyTypeID = Convert.ToInt32(row[THIRDPARTY_TYPEID]);
            }
            return thirdParty;
        }

        public static ThirdParties GetThirdParties()
        {
            ThirdParties thirdParties = new ThirdParties();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllThirdParties";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdParties.Add(FillThirdParty(row, 0));
                }
            }
            return thirdParties;
        }

        #region ThirdPartyModules


        public static ThirdParty GetCompanyThirdParty(int companyThirdPartyID)
        {
            ThirdParty thirdParty = new ThirdParty();

            object[] parameter = new object[1];
            parameter[0] = companyThirdPartyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdParty", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdParty = FillThirdParties(row, 0);

                }
            }
            return thirdParty;
        }

        public static ThirdParties GetCompanyThirdParties(int companyID)
        {
            ThirdParties thirdParties = new ThirdParties();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdParties", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdParties.Add(FillCompanyThirdParties(row, 0));

                }
            }
            return thirdParties;
        }


        public static List<ThirdParty> GetCompanyThirdParties_DayEnd(int companyID)
        {
            List<ThirdParty> thirdParties = new List<ThirdParty>();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdParties_DayEnd", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdParties.Add(FillCompanyThirdParties(row, 0));

                }
            }
            return thirdParties;
        }

        public static List<ThirdParty> GetCompanyThirdPartyTypeParty(int companyID, int thirdPartyTypeID)
        {
            List<ThirdParty> thirdParties = new List<ThirdParty>();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = thirdPartyTypeID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyType_Party", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdParties.Add(FillCompanyThirdParties(row, 0));

                }
            }
            return thirdParties;
        }

        private static ThirdParty FillCompanyThirdParties(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int thirdPartyID = 1 + offSet;
            int thirdPartyName = 2 + offSet;
            int description = 3 + offSet;
            int companyThirdPartyID = 4 + offSet;
            int symbolConvention = 5 + offSet;
            int securityIdentifierTypeID = 6 + offSet;

            ThirdParty thirdParty = new ThirdParty();
            if (row[companyID] != null)
            {
                thirdParty.CompanyID = int.Parse(row[companyID].ToString());
            }
            if (row[thirdPartyID] != null)
            {
                thirdParty.ThirdPartyID = int.Parse(row[thirdPartyID].ToString());
            }
            if (row[thirdPartyName] != null)
            {
                thirdParty.ThirdPartyName = row[thirdPartyName].ToString();
            }
            if (row[description] != null)
            {
                thirdParty.Description = row[description].ToString();
            }
            if (row[companyThirdPartyID] != null)
            {
                thirdParty.CompanyThirdPartyID = int.Parse(row[companyThirdPartyID].ToString());
            }
            if (row[symbolConvention] != System.DBNull.Value)// added by sandeep on 03-oct-2007
            {
                thirdParty.SymbolConvention = int.Parse(row[symbolConvention].ToString());
            }
            if (row[securityIdentifierTypeID] != System.DBNull.Value)// added by sandeep on 25-mar-2008
            {
                thirdParty.SecurityIdentifierType = (ApplicationConstants.SymbologyCodes)int.Parse(row[securityIdentifierTypeID].ToString());
            }
            return thirdParty;
        }

        //Currently these method have zero reference and using AUEC class which is not necessarily related to ThirdParty, so commenting for now and need to rethink this logic later
        //public static AUEC FillCompanyAUECDetails(object[] row, int offSet)
        //{
        //    int AUECID = 0 + offSet;
        //    int AUECDISPLAYNAME = 1 + offSet;

        //    AUEC companyAUEC = new AUEC();
        //    try
        //    {
        //        if (!(row[AUECID] is System.DBNull))
        //        {
        //            companyAUEC.AUECID = int.Parse(row[AUECID].ToString());
        //        }
        //        if (!(row[AUECDISPLAYNAME] is System.DBNull))
        //        {
        //            companyAUEC.DisplayName = row[AUECDISPLAYNAME].ToString();
        //        }
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //    return companyAUEC;
        //}

        //public static AUECs GetCompanyWorkingAUECs(int companyID)
        //{
        //    AUECs companyAUECs = new AUECs();

        //    object[] parameter = new object[1];
        //    parameter[0] = companyID;

        //    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetWorkingAUECForCompany", parameter))
        //    {
        //        while (reader.Read())
        //        {
        //            object[] row = new object[reader.FieldCount];
        //            reader.GetValues(row);
        //            companyAUECs.Add(FillCompanyAUECDetails(row, 0));

        //        }
        //    }
        //    return companyAUECs;
        //}
        #endregion

        #region ThirdPartyMethods
        private static ThirdParty FillThirdParties(object[] row, int offSet)
        {
            int thirdPartyID = 0 + offSet;
            int thirdPartyName = 1 + offSet;
            int description = 2 + offSet;
            int thirdPartyTypeID = 3 + offSet;
            int shortName = 4 + offSet;
            int contactPerson = 5 + offSet;
            int address1 = 6 + offSet;
            int address2 = 7 + offSet;
            int cellPhone = 8 + offSet;
            int workTelephone = 9 + offSet;
            int fax = 10 + offSet;
            int email = 11 + offSet;
            int countryID = 12 + offSet;
            int stateID = 13 + offSet;
            int zip = 14 + offSet;
            int primaryContactLastName = 15 + offSet;
            int primaryContactTitle = 16 + offSet;
            int primaryContactWorkTelephone = 17 + offSet;
            int primaryContactFax = 18 + offSet;
            int securityIdentifierType = 19 + offSet;
            int brokerCode = 20 + offSet;
            int counterPartyID = 21 + offSet;

            ThirdParty thirdParty = new ThirdParty();

            if (row[thirdPartyID] != System.DBNull.Value)
            {
                thirdParty.ThirdPartyID = int.Parse(row[thirdPartyID].ToString());
            }
            if (row[thirdPartyName] != System.DBNull.Value)
            {
                thirdParty.ThirdPartyName = row[thirdPartyName].ToString();
            }
            if (row[description] != System.DBNull.Value)
            {
                thirdParty.Description = row[description].ToString();
            }

            if (row[thirdPartyTypeID] != System.DBNull.Value)
            {
                thirdParty.ThirdPartyTypeID = int.Parse(row[thirdPartyTypeID].ToString());
            }
            if (row[shortName] != System.DBNull.Value)
            {
                thirdParty.ShortName = row[shortName].ToString();
            }
            if (row[contactPerson] != System.DBNull.Value)
            {
                thirdParty.ContactPerson = row[contactPerson].ToString();
            }
            if (row[address1] != System.DBNull.Value)
            {
                thirdParty.Address1 = row[address1].ToString();
            }
            if (row[address2] != System.DBNull.Value)
            {
                thirdParty.Address2 = row[address2].ToString();
            }
            if (row[cellPhone] != System.DBNull.Value)
            {
                thirdParty.CellPhone = row[cellPhone].ToString();
            }
            if (row[workTelephone] != System.DBNull.Value)
            {
                thirdParty.WorkTelephone = row[workTelephone].ToString();
            }
            if (row[fax] != System.DBNull.Value)
            {
                thirdParty.Fax = row[fax].ToString();
            }
            if (row[email] != System.DBNull.Value)
            {
                thirdParty.Email = row[email].ToString();
            }
            if (row[countryID] != System.DBNull.Value)
            {
                thirdParty.CountryID = int.Parse(row[countryID].ToString());
            }
            if (row[stateID] != System.DBNull.Value)
            {
                thirdParty.StateID = int.Parse(row[stateID].ToString());
            }
            if (row[zip] != System.DBNull.Value)
            {
                thirdParty.Zip = row[zip].ToString();
            }
            if (row[primaryContactLastName] != System.DBNull.Value)
            {
                thirdParty.PrimaryContactLastName = row[primaryContactLastName].ToString();
            }
            if (row[primaryContactTitle] != System.DBNull.Value)
            {
                thirdParty.PrimaryContactTitle = row[primaryContactTitle].ToString();
            }
            if (row[primaryContactWorkTelephone] != System.DBNull.Value)
            {
                thirdParty.PrimaryContactWorkTelephone = row[primaryContactWorkTelephone].ToString();
            }
            if (row[primaryContactFax] != System.DBNull.Value)
            {
                thirdParty.PrimaryContactFax = row[primaryContactFax].ToString();
            }
            if (row[securityIdentifierType] != System.DBNull.Value)
            {
                thirdParty.SecurityIdentifierType = (ApplicationConstants.SymbologyCodes)row[securityIdentifierType];
            }
            if (row[brokerCode] != System.DBNull.Value)
            {
                thirdParty.BrokerCode = row[brokerCode].ToString();
            }
            if (row[counterPartyID] != System.DBNull.Value)
            {
                thirdParty.CounterPartyID = Convert.ToInt32(row[counterPartyID]);
            }
            return thirdParty;
        }

        private static ThirdPartyFileFormat FillThirdPartyFileFormat(object[] row, int offSet)
        {
            int FileFormatId = 0 + offSet;
            int FileFormatName = 1 + offSet;
            int ThirdPartyId = 2 + offSet;
            int PranaToThirdParty = 3 + offSet;
            int HeaderFile = 4 + offSet;
            int FooterFile = 5 + offSet;
            int ExportOnly = 6 + offSet;
            int Delimiter = 7 + offSet;
            int DelimiterName = 8 + offSet;
            int FileExtension = 9 + offSet;
            int FileDisplayName = 10 + offSet;
            int SPName = 11 + offSet;
            int DoNotShowFileOpenDialogue = 12 + offSet;
            int ClearExternalTransID = 13 + offSet;
            int IncludeExercisedAssignedTransaction = 14 + offSet;
            int IncludeExerrcisedAssignedUnderlyingTransaction = 15 + offSet;
            int IncludeCATransaction = 16 + offSet;
            int GenerateCancelNewForAmend = 17 + offSet;
            int FIXEnabled = 18 + offSet;
            int FileEnabled = 19 + offSet;
            int FIXStorProc = 20 + offSet;
            int TimedBatchesEnabled = 21 + offSet;

            ThirdPartyFileFormat ThirdPartyFileForm = new ThirdPartyFileFormat();
            try
            {
                if (row[FileFormatId] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FileFormatId = int.Parse(row[FileFormatId].ToString());
                }
                if (row[FileFormatName] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FileFormatName = (row[FileFormatName].ToString());
                }

                if (row[ThirdPartyId] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.ThirdPartyID = int.Parse(row[ThirdPartyId].ToString());
                }
                if (row[PranaToThirdParty] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.PranaToThirdParty = row[PranaToThirdParty].ToString();
                }

                if (row[HeaderFile] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.HeaderFile = row[HeaderFile].ToString();
                }
                if (row[FooterFile] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FooterFile = row[FooterFile].ToString();
                }
                if (row[ExportOnly] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.ExportOnly = Convert.ToBoolean(row[ExportOnly].ToString());
                }
                if (row[Delimiter] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.Delimiter = row[Delimiter].ToString();
                }
                if (row[DelimiterName] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.DelimiterName = row[DelimiterName].ToString();
                }
                if (row[FileExtension] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FileExtension = row[FileExtension].ToString();
                }
                if (row[FileDisplayName] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FileDisplayName = row[FileDisplayName].ToString();
                }
                if (row[SPName] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.StoredProcName = row[SPName].ToString();
                }
                if (row[DoNotShowFileOpenDialogue] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.DoNotShowFileOpenDialogue = Convert.ToBoolean(row[DoNotShowFileOpenDialogue].ToString());
                }
                if (row[ClearExternalTransID] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.ClearExternalTransID = Convert.ToBoolean(row[ClearExternalTransID].ToString());
                }
                if (row[IncludeExercisedAssignedTransaction] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.IncludeExercisedAssignedTransaction = Convert.ToBoolean(row[IncludeExercisedAssignedTransaction].ToString());
                }
                if (row[IncludeExerrcisedAssignedUnderlyingTransaction] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.IncludeExercisedAssignedUnderlyingTransaction = Convert.ToBoolean(row[IncludeExerrcisedAssignedUnderlyingTransaction].ToString());
                }
                if (row[IncludeCATransaction] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.IncludeCATransaction = Convert.ToBoolean(row[IncludeCATransaction].ToString());
                }
                if (row[GenerateCancelNewForAmend] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.GenerateCancelNewForAmend = Convert.ToBoolean(row[GenerateCancelNewForAmend].ToString());
                }
                if (row[FIXEnabled] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FIXEnabled = Convert.ToBoolean(row[FIXEnabled].ToString());
                }
                if (row[FileEnabled] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FileEnabled = Convert.ToBoolean(row[FileEnabled].ToString());
                }
                if (row[FIXStorProc] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.FIXStorProc = row[FIXStorProc].ToString();
                }
                if (row[TimedBatchesEnabled] != System.DBNull.Value)
                {
                    ThirdPartyFileForm.TimeBatchesEnabled = Convert.ToBoolean(row[TimedBatchesEnabled].ToString());
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
            return ThirdPartyFileForm;
        }

        public static ThirdPartyFileFormats GetThirdPartyFileFormats(int thirdPartyId)
        {
            ThirdPartyFileFormats thirdPartyFileFormats = new ThirdPartyFileFormats();
            Object[] parameter = new object[1];
            parameter[0] = thirdPartyId;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyFileFormats", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyFileFormats.Add(FillThirdPartyFileFormat(row, 0));
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
            return thirdPartyFileFormats;
        }

        public static List<ThirdPartyFileFormat> GetCompanyThirdPartyFileFormats(int companyThirdPartyId)
        {
            List<ThirdPartyFileFormat> thirdPartyFileFormats = new List<ThirdPartyFileFormat>();
            Object[] parameter = new object[1];
            parameter[0] = companyThirdPartyId;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdPartyFileFormats", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyFileFormats.Add(FillThirdPartyFileFormat(row, 0));
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
            return thirdPartyFileFormats;
        }

        public static List<ThirdPartyFileFormat> GetCompanyThirdPartyTypeFileFormats(int companyThirdPartyId)
        {
            List<ThirdPartyFileFormat> thirdPartyFileFormats = new List<ThirdPartyFileFormat>();
            Object[] parameter = new object[1];
            parameter[0] = companyThirdPartyId;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdPartyTypeFileFormats", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyFileFormats.Add(FillThirdPartyFileFormat(row, 0));
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
            return thirdPartyFileFormats;
        }

        public static ThirdParties GetAllThirdPartiesForTree()
        {
            ThirdParties thirdParties = new ThirdParties();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllThirdPartiesForTree";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdParties.Add(FillThirdParties(row, 0));
                }
            }
            return thirdParties;
        }

        public static int SaveThirdParty(object[] parameter)
        {
            int result = int.MinValue;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyDetail", parameter).ToString());
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

        public static int SaveThirdPartyFileFormat(ThirdPartyFileFormats thirdPartyFileFormats, int thirdPartyId)
        {
            int result = int.MinValue;
            //System.Text.StringBuilder thirdPartyFiForm = new System.Text.StringBuilder();

            object[] parameter = new object[30];
            try
            {
                foreach (ThirdPartyFileFormat thirdPartyFileFormat in thirdPartyFileFormats)
                {
                    byte[] pranaToThirdPartyData = TransformToBinary(thirdPartyFileFormat.PranaToThirdParty);
                    byte[] headerFileData = TransformToBinary(thirdPartyFileFormat.HeaderFile);
                    byte[] footerFileData = TransformToBinary(thirdPartyFileFormat.FooterFile);
                    parameter[0] = thirdPartyId;
                    parameter[1] = thirdPartyFileFormat.FileFormatId;
                    parameter[2] = thirdPartyFileFormat.FileFormatName;
                    parameter[3] = GetFileNameFromPath(thirdPartyFileFormat.PranaToThirdParty);
                    parameter[4] = GetFileNameFromPath(thirdPartyFileFormat.HeaderFile);
                    parameter[5] = GetFileNameFromPath(thirdPartyFileFormat.FooterFile);
                    parameter[6] = pranaToThirdPartyData;
                    parameter[7] = headerFileData;
                    parameter[8] = footerFileData;
                    parameter[9] = Convert.ToInt32(Prana.Global.ApplicationConstants.MappingFileType.ThirdPartyXSLT);
                    parameter[10] = thirdPartyFileFormat.ExportOnly;
                    parameter[11] = DateTime.UtcNow;
                    parameter[12] = thirdPartyFileFormat.Delimiter;
                    parameter[13] = thirdPartyFileFormat.DelimiterName;
                    parameter[14] = thirdPartyFileFormat.FileExtension;
                    parameter[15] = thirdPartyFileFormat.FileDisplayName;
                    if (thirdPartyFileFormat.GenerateCancelNewForAmend.Equals(true))
                        parameter[16] = "P_FFGetThirdPartyFundsDetails_Amend";
                    else if (thirdPartyFileFormat.GenerateCancelNewForAmend.Equals(false) && thirdPartyFileFormat.StoredProcName.Equals("P_FFGetThirdPartyFundsDetails_Amend"))
                        parameter[16] = "";
                    else
                        parameter[16] = thirdPartyFileFormat.StoredProcName;
                    parameter[17] = thirdPartyFileFormat.DoNotShowFileOpenDialogue;
                    parameter[18] = thirdPartyFileFormat.ClearExternalTransID;
                    parameter[19] = thirdPartyFileFormat.IncludeExercisedAssignedTransaction;
                    parameter[20] = thirdPartyFileFormat.IncludeExercisedAssignedUnderlyingTransaction;
                    parameter[21] = thirdPartyFileFormat.IncludeCATransaction;
                    parameter[22] = "";
                    parameter[23] = 0;
                    parameter[24] = thirdPartyFileFormat.GenerateCancelNewForAmend;
                    parameter[25] = thirdPartyFileFormat.FIXEnabled;
                    parameter[26] = thirdPartyFileFormat.FileEnabled;
                    parameter[27] = thirdPartyFileFormat.FIXStorProc;
                    parameter[28] = thirdPartyFileFormat.TimeBatchesEnabled;
                    parameter[29] = CreateXMLTimeBatchData(thirdPartyFileFormat.TimeBatches, thirdPartyFileFormat.TimeBatchesEnabled);
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyFileFormat_New", parameter).ToString());
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

        /// <summary>
        /// Creates an XML representation of the records for saving into the database.
        /// </summary>
        /// <param name="timeBatches">List of ThirdPartyTimeBatch objects.</param>
        /// <param name="timeBatchesEnabled">Indicates whether time batches are enabled.</param>
        /// <returns>XML document containing time batch data.</returns>
        private static string CreateXMLTimeBatchData(List<ThirdPartyTimeBatch> timeBatches, bool timeBatchesEnabled)
        {
            string xmlData = string.Empty;
            try
            {
                // Create a DataTable to hold time batch data
                DataTable dtTimeBatchesData = new DataTable("DtTimeBatchesData");
                dtTimeBatchesData.Columns.Add("Id", typeof(int));
                dtTimeBatchesData.Columns.Add("BatchRunTime", typeof(string));
                dtTimeBatchesData.Columns.Add("IsPaused", typeof(bool));

                // Create a DataSet to organize the DataTable
                DataSet dsTimeBatchesData = new DataSet("DsTimeBatchesData");

                // Populate the DataTable if time batches are enabled
                if (timeBatchesEnabled)
                {
                    foreach (ThirdPartyTimeBatch timeBatch in timeBatches)
                    {
                        dtTimeBatchesData.Rows.Add(timeBatch.ID, timeBatch.BatchRunTime.ToString(), timeBatch.IsPaused);
                    }
                }

                dsTimeBatchesData.Tables.Add(dtTimeBatchesData);

                // Get the XML representation of the DataSet
                xmlData = dsTimeBatchesData.GetXml();
            }
            catch (Exception ex)
            {
                // Handle exceptions and ensure no secure information leaks
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return xmlData;
        }

        public static string DeleteThirdParty(int thirdPartyID, bool isDeletedForceFully)
        {
            string result = string.Empty;
            Object[] parameter = new object[2];
            parameter[0] = thirdPartyID;
            parameter[1] = (isDeletedForceFully == true ? 1 : 0);

            try
            {
                result = DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteThirdParty", parameter).ToString();
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

        private static string GetFileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        private static byte[] TransformToBinary(string path)
        {
            if (path != "" && path.Contains("\\"))
            {
                FileStream fs = null;
                BinaryReader br = null;
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                byte[] data = new byte[fs.Length];
                int length = (int)fs.Length;
                br.Read(data, 0, length);

                if (fs != null)
                {
                    fs.Close();
                    // br.Close();
                }

                return data;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ThirdPartyType
        public static ThirdPartyType FillThirdPartyType(object[] row, int offSet)
        {
            int thirdPartyTypeID = 0 + offSet;
            int thirdPartyTypeName = 1 + offSet;

            ThirdPartyType thirdPartyType = new ThirdPartyType();
            try
            {
                if (row[thirdPartyTypeID] != null)
                {
                    thirdPartyType.ThirdPartyTypeID = int.Parse(row[thirdPartyTypeID].ToString());
                }
                if (row[thirdPartyTypeName] != null)
                {
                    thirdPartyType.ThirdPartyTypeName = row[thirdPartyTypeName].ToString();
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
            return thirdPartyType;
        }

        public static List<ThirdPartyType> GetThirdPartyTypes()
        {
            List<ThirdPartyType> thirdPartyTypes = new List<ThirdPartyType>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllThirdPartyTypes";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdPartyTypes.Add(FillThirdPartyType(row, 0));
                }
            }
            return thirdPartyTypes;
        }

        public static ThirdPartyType GetThirdPartyType(int thirdPartyTypeID)
        {
            ThirdPartyType thirdPartyType = new ThirdPartyType();

            object[] parameter = new object[1];
            parameter[0] = thirdPartyTypeID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyType", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdPartyType = FillThirdPartyType(row, 0);

                }
            }
            return thirdPartyType;
        }

        #endregion

        #region ThirdPartyPermittedAccounts

        /// <summary>
        /// The method is used to fetch the accounts permitted to a thirdparty .
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public static List<ThirdPartyPermittedAccount> GetThirdPartyPermittedAccounts(int thirdPartyID)
        {
            List<ThirdPartyPermittedAccount> thirdPartyPermittedAccounts = new List<ThirdPartyPermittedAccount>();

            object[] parameter = new object[1];
            parameter[0] = thirdPartyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyPermittedFunds", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdPartyPermittedAccounts.Add(FillThirdPartyPermittedAccount(row, 0));

                }
            }
            return thirdPartyPermittedAccounts;
        }

        public static List<ThirdPartyPermittedAccount> GetThirdPartyAccounts(int companyID, int userID)
        {
            List<ThirdPartyPermittedAccount> thirdPartyPermittedAccounts = new List<ThirdPartyPermittedAccount>();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = userID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyFundsForThirdParty", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdPartyPermittedAccounts.Add(FillThirdPartyPermittedAccount(row, 0));

                }
            }
            return thirdPartyPermittedAccounts;
        }

        public static ThirdPartyPermittedAccounts GetThirdPartyPermittedAccountsNotTotheSame(int thirdPartyID)
        {
            ThirdPartyPermittedAccounts thirdPartyPermittedAccounts = new ThirdPartyPermittedAccounts();

            object[] parameter = new object[1];
            parameter[0] = thirdPartyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyFundsNotPermitted", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    thirdPartyPermittedAccounts.Add(FillThirdPartyPermittedAccount(row, 0));

                }
            }
            return thirdPartyPermittedAccounts;
        }

        /// <summary>
        /// The method is used to fill an object of ThirdPartyPermittedAccount .
        /// </summary>
        /// <param name="row"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static ThirdPartyPermittedAccount FillThirdPartyPermittedAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ThirdPartyPermittedAccount thirdPartyPermittedAccount = new ThirdPartyPermittedAccount();

            if (row != null)
            {
                int THIRDPARTYFUNDID_PK = offset + 0;
                int THIRDPARTYID = offset + 1;
                int COMPANYFUNDID = offset + 2;
                int FUNDYNAME = offset + 3;
                int FUNDTYPE = offset + 4;

                thirdPartyPermittedAccount.ThirdPartyAccountID_PK = Convert.ToInt32(row[THIRDPARTYFUNDID_PK]);
                thirdPartyPermittedAccount.ThirdPartyID = Convert.ToInt32(row[THIRDPARTYID]);
                thirdPartyPermittedAccount.CompanyAccountID = Convert.ToInt32(row[COMPANYFUNDID]);
                thirdPartyPermittedAccount.AccountName = Convert.ToString(row[FUNDYNAME]);
                thirdPartyPermittedAccount.AccountTypeID = Convert.ToInt32(row[FUNDTYPE]);
            }
            return thirdPartyPermittedAccount;
        }

        #endregion ThirdPartyPermittedAccounts

        /// <summary>
        /// This Method is to get Third Party Type Id
        /// </summary>
        /// <param name="thirdParty"></param>
        /// <returns>ThirdPartyTypeID</returns>
        public static int GetThirdPartyTypeId(ThirdParty thirdParty)
        {
            if (thirdParty.CompanyID > int.MinValue)
            {
                ThirdParty thirdParty1 = GetThirdParty(thirdParty.ThirdPartyID);
                thirdParty.ThirdPartyTypeID = thirdParty1.ThirdPartyTypeID;
            }

            return thirdParty.ThirdPartyTypeID;
        }

        /// <summary>
        /// This method is to get third party type name
        /// </summary>
        /// <param name="thirdParty"></param>
        /// <returns>Third Party Type Name</returns>
        public static string GetThirdPartyTypeName(ThirdParty thirdParty)
        {
            ThirdPartyType thirdPartyType = GetThirdPartyType(thirdParty.ThirdPartyTypeID);
            if (thirdPartyType != null)
            {
                thirdParty.ThirdPartyTypeName = thirdPartyType.ThirdPartyTypeName;
            }
            else
            {
                thirdParty.ThirdPartyTypeName = "";
            }
            return thirdParty.ThirdPartyTypeName;
        }

        public static int GetCompanyUserId()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "Select Top 1 UserId from T_CompanyUser";

                return (int)DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
            }
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
            return -1;
        }

        /// <summary>
        /// Gets the company id.
        /// </summary>
        /// <returns>THe ID of teh company</returns>
        /// <remarks></remarks>
        public static int GetCompanyId()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "Select Top 1 CompanyId from T_Company";

                return (int)DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
            }
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
            return -1;
        }
        /// <summary>
        /// Gets the name of the company.
        /// </summary>
        /// <returns>The name of the company</returns>
        /// <remarks></remarks>
        public static string GetCompanyName()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "Select Top 1 Name from T_Company";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    reader.Read();
                    return reader[0].ToString();
                }
            }
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
            return string.Empty;
        }

        #region ThirdPartyModules

        /// <summary>
        /// Gets the third party FTP.
        /// </summary>
        /// <param name="ftps">The FTPS.</param>
        /// <param name="ftpId">The FTP id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ThirdPartyFtp GetThirdPartyFtp(List<ThirdPartyFtp> ftps, int ftpId)
        {
            try
            {
                foreach (ThirdPartyFtp ftp in ftps)
                {
                    if (ftp.FtpId == ftpId)
                        return ftp;
                }
            }
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
            return null;
        }

        /// <summary>
        /// Gets the third party recipient.
        /// </summary>
        /// <param name="gnuPGs">The gnu P gs.</param>
        /// <param name="gnuPGId">The gnu PG id.</param>
        /// <returns>THe GnuPG object</returns>
        /// <remarks></remarks>
        public static ThirdPartyGnuPG GetThirdPartyGnuPG(List<ThirdPartyGnuPG> gnuPGs, int gnuPGId)
        {
            try
            {
                foreach (ThirdPartyGnuPG gnuPG in gnuPGs)
                {
                    if (gnuPG.GnuPGId == gnuPGId)
                        return gnuPG;
                }
            }
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
            return null;
        }

        /// <summary>
        /// Get the third party email
        /// </summary>
        /// <param name="emails">Colection of emails</param>
        /// <param name="emailId">ID of the email</param>
        /// <returns>THe email object</returns>
        public static ThirdPartyEmail GetThirdPartyEmail(List<ThirdPartyEmail> emails, int emailId)
        {
            try
            {
                foreach (ThirdPartyEmail email in emails)
                {
                    if (email.EmailId == emailId)
                        return email;
                }
            }
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
            return null;
        }

        /// <summary>
        /// Gets the third party FTP.
        /// </summary>
        /// <returns>The Collection of details of the third party FTPs</returns>
        /// <remarks></remarks>
        public static List<ThirdPartyFtp> GetThirdPartyFtps()
        {
            try
            {
                return GetData<List<ThirdPartyFtp>, ThirdPartyFtp>("P_GetThirdPartyFtp", -1);
            }
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
            return null;
        }

        /// <summary>
        /// Gets the third party gnu PG.
        /// </summary>
        /// <returns>The collection of the third party gnupg details</returns>
        /// <remarks></remarks>
        public static List<ThirdPartyGnuPG> GetThirdPartyGnuPG()
        {
            try
            {
                return GetData<List<ThirdPartyGnuPG>, ThirdPartyGnuPG>("P_GetThirdPartyGnuPG", -1);
            }
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
            return null;
        }

        public static List<ThirdPartyEmail> GetThirdPartyEmail()
        {
            try
            {
                return GetData<List<ThirdPartyEmail>, ThirdPartyEmail>("P_GetThirdPartyEmail", -1, -1);
            }
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
            return null;
        }

        /// <summary>
        /// Gets the third party Email.
        /// </summary>
        /// <param name="ftpId">The email id.</param>
        /// <returns>The third party email</returns>
        /// <remarks></remarks>
        public static ThirdPartyEmail GetThirdPartyEmail(int ftpId)
        {
            ThirdPartyEmails emails = GetData<ThirdPartyEmails, ThirdPartyEmail>("P_GetThirdPartyEmail", ftpId, -1);
            return (ThirdPartyEmail)emails[0];
        }

        /// <summary>
        /// Get the list of Email data objects from Database
        /// </summary>
        /// <returns>The list of email data objects</returns>
        public static List<ThirdPartyEmail> GetThirdPartyDataEmail()
        {
            try
            {
                return GetData<List<ThirdPartyEmail>, ThirdPartyEmail>("P_GetThirdPartyEmail", -1, (int)MailType.DataFile);
            }
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
            return null;
        }

        /// <summary>
        /// Get the list of email log objects
        /// </summary>
        /// <returns>The list of email log objects</returns>
        public static List<ThirdPartyEmail> GetThirdPartyLogEmail()
        {
            try
            {
                return GetData<List<ThirdPartyEmail>, ThirdPartyEmail>("P_GetThirdPartyEmail", -1, (int)MailType.LogFile);
            }
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
            return null;
        }




        #endregion

        #region Helper Functions
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T GetData<T, U>(string sql, params Object[] parameters)
            where T : IList, new()
            where U : class, new()
        {
            T list = new T();

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(sql, parameters))
            {
                ReadItems<U>(reader, list);
                return list;
            }
        }

        /// <summary>
        /// Deletes the items.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int DeleteItems(string sql, params object[] args)
        {
            return DatabaseManager.DatabaseManager.ExecuteNonQuery(sql, args);

        }

        internal static int SaveThirdPartyBatch(ThirdPartyBatch batch)
        {
            SqlParameterCollection col = DatabaseManager.DatabaseManager.GetParameters("P_SaveThirdPartyBatch");
            List<object> parms = new List<object>();
            foreach (SqlParameter p in col)
            {
                Type entityType = batch.GetType();
                PropertyInfo info = entityType.GetProperty(p.ParameterName.Replace("@", ""));

                if (info != null)
                {
                    parms.Add(info.GetValue(batch, null));
                    //p.Value = info.GetValue(thirdParty, null);
                }
            }

            //Debug.Print(string.Join(",",parms));
            return SaveItem<ThirdPartyBatch>("P_SaveThirdPartyBatch", parms.ToArray());
        }

        internal static void SaveThirdPartyBatchForRefresh(ThirdPartyBatch batch)
        {
            SqlParameterCollection col = DatabaseManager.DatabaseManager.GetParameters("P_SaveThirdPartyBatchForRefresh");
            List<object> parms = new List<object>();
            foreach (SqlParameter p in col)
            {
                Type entityType = batch.GetType();
                PropertyInfo info = entityType.GetProperty(p.ParameterName.Replace("@", ""));

                if (info != null)
                {
                    parms.Add(info.GetValue(batch, null));
                }
            }
            DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyBatchForRefresh", parms.ToArray());
        }

        internal static bool CheckDuplicateBatch(ThirdPartyBatch batch)
        {
            SqlParameterCollection col = DatabaseManager.DatabaseManager.GetParameters("P_CheckDuplicateThirdPartyBatch");
            List<object> parms = new List<object>();
            foreach (SqlParameter p in col)
            {
                Type entityType = batch.GetType();
                PropertyInfo info = entityType.GetProperty(p.ParameterName.Replace("@", ""));

                if (info != null)
                {
                    parms.Add(info.GetValue(batch, null));
                    //p.Value = info.GetValue(thirdParty, null);
                }
            }
            //Debug.Print(string.Join(",",parms));
            int count = SaveItem<ThirdPartyBatch>("P_CheckDuplicateThirdPartyBatch", parms.ToArray());

            if (count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Saves the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">The db.</param>
        /// <param name="item">The item.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int SaveItem<T>(string sql, params object[] args) where T : class
        {
            return ((int)DatabaseManager.DatabaseManager.ExecuteScalar(sql, args));
        }

        /// <summary>
        /// Saves the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int[] SaveItems<T, U>(T items, string sql, params object[] args)
            where T : IList
            where U : class
        {
            List<int> returns = new List<int>();

            foreach (U item in items)
            {
                returns.Add(Convert.ToInt16(DatabaseManager.DatabaseManager.ExecuteScalar(sql, args)));
            }
            return returns.ToArray();

        }
        #endregion

        /// <summary>
        /// Deletes the third party permitted accounts.
        /// </summary>
        /// <param name="thirdPartyID">The third party ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int DeleteThirdPartyPermittedAccounts(int thirdPartyID)
        {
            return DeleteItems("P_DeleteThirdPartyPermittedFunds", thirdPartyID);
        }

        /// <summary>
        /// Saves the third party permitted accounts.
        /// </summary>
        /// <param name="thirdPartyPermittedAccounts">The third party permitted accounts.</param>
        /// <param name="thirdPartyID">The third party ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int SaveThirdPartyPermittedAccounts(ThirdPartyPermittedAccounts thirdPartyPermittedAccounts, int thirdPartyID)
        {
            int rows = 0;
            DeleteThirdPartyPermittedAccounts(thirdPartyID);

            foreach (ThirdPartyPermittedAccount account in thirdPartyPermittedAccounts)
            {
                rows += SaveItem<ThirdPartyPermittedAccount>("P_SaveThirdPartyPermittedFund", thirdPartyID, account.CompanyAccountID);
            }
            return rows;
        }

        /// <summary>
        /// Saves the third party gnu PG.
        /// </summary>
        /// <param name="gnuPG">The gnu PG.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int SaveThirdPartyGnuPG(ThirdPartyGnuPG gnuPG)
        {
            try
            {
                SqlParameterCollection col = DatabaseManager.DatabaseManager.GetParameters("P_SaveThirdPartyGnuPG");
                List<object> parms = new List<object>();
                foreach (SqlParameter p in col)
                {
                    Type entityType = gnuPG.GetType();
                    PropertyInfo info = entityType.GetProperty(p.ParameterName.Replace("@", ""));

                    if (info != null)
                    {
                        parms.Add(info.GetValue(gnuPG, null));
                        //p.Value = info.GetValue(thirdParty, null);
                    }
                }
                return SaveItem<ThirdPartyGnuPG>("P_SaveThirdPartyGnuPG", parms.ToArray());
            }
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
            return -1;
        }

        /// <summary>
        /// Save the email of the third party
        /// </summary>
        /// <param name="email">Email object</param>
        /// <returns>positive integer id of the record if the record is saved</returns>
        public static int SaveThirdPartyEmail(ThirdPartyEmail email)
        {
            SqlParameterCollection col = DatabaseManager.DatabaseManager.GetParameters("P_SaveThirdPartyEmail");
            List<object> parms = new List<object>();
            foreach (SqlParameter p in col)
            {
                Type entityType = email.GetType();
                PropertyInfo info = entityType.GetProperty(p.ParameterName.Replace("@", ""));

                if (info != null)
                {
                    parms.Add(info.GetValue(email, null));
                    //p.Value = info.GetValue(thirdParty, null);
                }
            }
            return SaveItem<ThirdPartyEmail>("P_SaveThirdPartyEmail", parms.ToArray());

        }

        public static int DeleteThirdPartyBatch(ThirdPartyBatch batch)
        {
            return (int)DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteThirdPartyBatch", new object[] { batch.ThirdPartyBatchId });
        }

        /// <summary>
        /// Saves the third party FTP.
        /// </summary>
        /// <param name="ftp">The FTP.</param>
        /// <returns>ID of the saved record</returns>
        /// <remarks></remarks>
        public static int SaveThirdPartyFtp(ThirdPartyFtp ftp)
        {
            try
            {
                SqlParameterCollection col = DatabaseManager.DatabaseManager.GetParameters("P_SaveThirdPartyFtp");
                List<object> parms = new List<object>();
                foreach (SqlParameter p in col)
                {
                    Type entityType = ftp.GetType();
                    PropertyInfo info = entityType.GetProperty(p.ParameterName.Replace("@", ""));

                    if (info != null)
                    {
                        parms.Add(info.GetValue(ftp, null));
                    }
                }
                return SaveItem<ThirdPartyFtp>("P_SaveThirdPartyFtp", parms.ToArray());
            }
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
            return -1;
        }

        /// <summary>
        /// Saves the third party.
        /// </summary>
        /// <param name="thirdParty">The third party.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int SaveThirdParty(ThirdParty thirdParty)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[23];

                parameter[0] = thirdParty.ThirdPartyName;
                parameter[1] = thirdParty.Description;
                parameter[2] = thirdParty.ThirdPartyTypeID;
                parameter[3] = thirdParty.ShortName;
                parameter[4] = thirdParty.ContactPerson;
                parameter[5] = thirdParty.Address1;
                parameter[6] = thirdParty.Address2;
                parameter[7] = thirdParty.CellPhone;
                parameter[8] = thirdParty.WorkTelephone;
                parameter[9] = thirdParty.Fax;
                parameter[10] = thirdParty.Email;
                parameter[11] = thirdParty.CountryID;
                parameter[12] = thirdParty.StateID;
                parameter[13] = thirdParty.Zip;
                parameter[14] = thirdParty.PrimaryContactLastName;
                parameter[15] = thirdParty.PrimaryContactTitle;
                parameter[16] = thirdParty.PrimaryContactWorkTelephone;
                parameter[17] = thirdParty.PrimaryContactFax;
                parameter[18] = thirdParty.ThirdPartyID;
                parameter[19] = (int)thirdParty.SecurityIdentifierType;
                parameter[20] = int.MinValue;
                parameter[21] = thirdParty.BrokerCode;
                parameter[22] = thirdParty.CounterPartyID;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyDetail", parameter).ToString());
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

        /// <summary>
        /// Deletes the selected file format.
        /// </summary>
        /// <param name="thirdPartyId">The third party id.</param>
        /// <param name="fileFormatId">The file format id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int DeleteSelectedFileFormat(int thirdPartyId, int fileFormatId)
        {
            return DeleteItems("P_DeleteSelectedFileFormat", thirdPartyId, fileFormatId);
        }

        /// <summary>
        /// Deletes the third party.
        /// </summary>
        /// <param name="thirdPartyID">The third party ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool DeleteThirdParty(int thirdPartyID)
        {
            return DeleteItems("P_DeleteThirdParty", thirdPartyID) > 0;
        }

        /// <summary>
        /// Deletes the third party FTP.
        /// </summary>
        /// <param name="ftp">The FTP</param>
        /// <returns>The id of the deleted record</returns>
        /// <remarks></remarks>
        public static int DeleteThirdPartyFtp(ThirdPartyFtp ftp)
        {
            try
            {
                return (int)DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteThirdPartyFtp", new object[] { ftp.FtpId });
            }
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
            return -1;
        }

        /// <summary>
        /// Deletes the third party gnu PG.
        /// </summary>
        /// <param name="gnuPG">The gnu PG.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int DeleteThirdPartyGnuPG(ThirdPartyGnuPG gnuPG)
        {
            try
            {
                return (int)DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteThirdPartyGnuPG", new object[] { gnuPG.GnuPGId });
            }
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
            return -1;
        }

        /// <summary>
        /// Delete the Email of third party
        /// </summary>
        /// <param name="email">Email object</param>
        /// <returns>Positive integer if the record is deleted</returns>
        public static int DeleteThirdPartyEmail(ThirdPartyEmail email)
        {
            try
            {
                return (int)DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteThirdPartyEMail", new object[] { email.EmailId });
            }
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
            return -1;
        }

        /// <summary>
        /// Gets the third party FTP.
        /// </summary>
        /// <param name="ftpId">The FTP id.</param>
        /// <returns>The third party FTP</returns>
        /// <remarks></remarks>
        public static ThirdPartyFtp GetThirdPartyFtp(int ftpId)
        {
            ThirdPartyFtps ftps = GetData<ThirdPartyFtps, ThirdPartyFtp>("P_GetThirdPartyFtp", ftpId);
            if (ftps.Count > 0)
            {
                return (ThirdPartyFtp)ftps[0];
            }
            return null;
        }

        public static StringBuilder GetAUECIds(int companyID)
        {
            StringBuilder auecIds = new StringBuilder();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetWorkingAUECForCompany", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    auecIds.Append(row[0].ToString() + ",");
                }
            }
            return auecIds;
        }

        public static ThirdPartyFlatFileSaveDetail GetThirdPartyFlatFileSaveDetail(int thirdPartyID)
        {
            ThirdPartyFlatFileSaveDetail thirdPartyFlatFileSaveDetail = new ThirdPartyFlatFileSaveDetail();

            object[] parameter = new object[1];
            parameter[0] = thirdPartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFFThirdPartySaveDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyFlatFileSaveDetail = FillCompanyThirdPartySaveDetail(row, 0);
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
            return thirdPartyFlatFileSaveDetail;
        }

        public static ThirdPartyFlatFileSaveDetail FillCompanyThirdPartySaveDetail(object[] row, int offSet)
        {
            int companyThirdPartySaveDetailID = 0 + offSet;
            int companyThirdPartyID = 1 + offSet;
            int saveGeneratedFileIn = 2 + offSet;
            int namingConvention = 3 + offSet;
            int companyIdentifier = 4 + offSet;


            ThirdPartyFlatFileSaveDetail thirdPartyFlatFileSaveDetail = new ThirdPartyFlatFileSaveDetail();
            try
            {

                if (!(row[companyThirdPartySaveDetailID] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.CompanyThirdPartySaveDetailID = int.Parse(row[companyThirdPartySaveDetailID].ToString());
                }
                if (!(row[companyThirdPartyID] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.CompanyThirdPartyID = int.Parse(row[companyThirdPartyID].ToString());
                }
                if (!(row[saveGeneratedFileIn] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.SaveGeneratedFileIn = (row[saveGeneratedFileIn].ToString());
                }
                if (!(row[namingConvention] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.NamingConvention = row[namingConvention].ToString();
                }
                if (!(row[companyIdentifier] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.CompanyIdentifier = row[companyIdentifier].ToString();
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
            return thirdPartyFlatFileSaveDetail;
        }

        /// <summary>
        /// This method is to check for unallocated trades
        /// </summary>
        /// <param name="tradedate"></param>
        /// <returns>true if there are unallocated trades, otherwise false</returns>
        public static bool CheckForUnallocatedTrades(DateTime tradedate)
        {
            bool isUnallocatedTaxlots = false;
            try
            {
                object[] param = new object[1];

                param[0] = tradedate;

                object result = DatabaseManager.DatabaseManager.ExecuteScalar("P_GetUnallocatedOrdersCount", param);

                if (result == null || (int)result == 0)
                {
                    isUnallocatedTaxlots = false;
                }
                else
                {
                    isUnallocatedTaxlots = true;
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
            return isUnallocatedTaxlots;
        }

        /// <summary>
        /// Saves Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int SaveThirdPartyForceConfirmAuditData(List<ThirdPartyForceConfirm> dataSaveList)
        {
            try
            {
                int recordsSaved = 0;
                foreach (ThirdPartyForceConfirm data in dataSaveList)
                {
                    object[] parameterSave = new object[10];
                    parameterSave[0] = data.UserID;
                    parameterSave[1] = DateTime.UtcNow;
                    parameterSave[2] = data.Broker;
                    parameterSave[3] = data.Symbol;
                    parameterSave[4] = data.Side;
                    parameterSave[5] = data.Quantity;
                    parameterSave[6] = data.AllocationID;
                    parameterSave[7] = data.ThirdPartyBatchID;
                    parameterSave[8] = data.Comment;
                    parameterSave[9] = data.BlockID;

                    recordsSaved += DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveThirdPartyForceConfirmAuditData", parameterSave);
                }
                return recordsSaved;
            }
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
            return 0;
        }

        /// <summary>
        /// Gets Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <returns></returns>
        public static List<ThirdPartyForceConfirm> GetThirdPartyForceConfirmAuditData(int thirdPartyBatchId, DateTime runDate)
        {
            List<ThirdPartyForceConfirm> list = new List<ThirdPartyForceConfirm>();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = thirdPartyBatchId;
                parameter[1] = runDate;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyForceConfirmAuditData", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        list.Add(FillThirdPartyForceConfirmAuditData(row, 0));
                    }
                }
            }
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
            return list;
        }

        /// <summary>
        /// Fills Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static ThirdPartyForceConfirm FillThirdPartyForceConfirmAuditData(object[] row, int offSet)
        {
            int thirdPartyCompanyUserId = 0 + offSet;
            int thirdPartyConfirmationDateTime = 1 + offSet;
            int thirdPartyBroker = 2 + offSet;
            int thirdPartySymbol = 3 + offSet;
            int thirdPartySide = 4 + offSet;
            int thirdPartyQuantity = 5 + offSet;
            int thirdPartyAllocationID = 6 + offSet;
            int thirdPartyBatchID = 7 + offSet;
            int thirdPartyComment = 8 + offSet;
            int thirdPartyCompanyUserName = 9 + offSet;

            ThirdPartyForceConfirm thirdPartyforceConfirmAuditData = new ThirdPartyForceConfirm();
            try
            {
                if (!(row[thirdPartyCompanyUserId] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.UserID = int.Parse(row[thirdPartyCompanyUserId].ToString());
                }
                if (!(row[thirdPartyConfirmationDateTime] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.ConfirmationDateTime = DateTime.Parse(row[thirdPartyConfirmationDateTime].ToString());
                }
                if (!(row[thirdPartyBroker] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.Broker = row[thirdPartyBroker].ToString();
                }
                if (!(row[thirdPartySymbol] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.Symbol = row[thirdPartySymbol].ToString();
                }
                if (!(row[thirdPartySide] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.Side = row[thirdPartySide].ToString();
                }
                if (!(row[thirdPartyQuantity] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.Quantity = row[thirdPartyQuantity].ToString();
                }
                if (!(row[thirdPartyAllocationID] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.AllocationID = row[thirdPartyAllocationID].ToString();
                }
                if (!(row[thirdPartyBatchID] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.ThirdPartyBatchID = int.Parse(row[thirdPartyBatchID].ToString());
                }
                if (!(row[thirdPartyComment] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.Comment = row[thirdPartyComment].ToString();
                }
                if (!(row[thirdPartyCompanyUserName] is System.DBNull))
                {
                    thirdPartyforceConfirmAuditData.UserName = row[thirdPartyCompanyUserName].ToString();
                }
            }
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
            return thirdPartyforceConfirmAuditData;
        }

        /// <summary>
        /// This method is to get the job statuses
        /// </summary>
        /// <returns>mapping of job names and their statuses</returns>
        public static DataTable GetDailyJobStatuses(DateTime runDate)
        {
            DataTable jobStatusData = new DataTable();
            try
            {
                QueryData qData = new QueryData();
                qData.StoredProcedureName = "P_GetThirdPartyDailyJobStatuses";
                qData.DictionaryDatabaseParameter.Add("@runDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@runDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = runDate
                });
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(qData))
                {
                    jobStatusData.Load(reader);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return jobStatusData;
        }

        /// <summary>
        /// This method is get the block level details for a third party batch
        /// </summary>
        /// <param name="thirdPartyBatchId"></param>
        /// <param name="runDate"></param>
        /// <returns>List of ThirdPartyBlockLevelDetails if available, otherwise empty list</returns>
        public static DataSet GetBlockAllocationDetails(int thirdPartyBatchId, DateTime runDate)
        {
            DataSet blockAllocationDetails = new DataSet();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = thirdPartyBatchId;
                parameter[1] = runDate;
                blockAllocationDetails = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetBlockAllocationDetails", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return blockAllocationDetails;
        }

        /// <summary>
        /// This method is to get Force Confirm Details
        /// </summary>
        /// <param name="allocationId"></param>
        /// <returns></returns>
        public static DataSet GetForceConfirmDetails(string allocationId)
        {
            DataSet forceConfirmDetails = new DataSet();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = allocationId;
                forceConfirmDetails = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetBlockAllocationDetailsForForceConfirm", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return forceConfirmDetails;
        }

        /// <summary>
        /// This method gets taxlots which are eligible to be force matched/ reject
        /// </summary>
        /// <param name="allocationIdList"></param>
        /// <returns></returns>
        public static DataSet GetRequiredTaxlotsForAU(List<string> allocationIdList)
        {
            DataSet rejectableTaxlots = new DataSet();
            try
            {
                string allocationIdListCommaSeparated = string.Join(",", allocationIdList);
                object[] parameter = new object[1];
                parameter[0] = allocationIdListCommaSeparated;
                rejectableTaxlots = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetRequiredTaxlotsForAU", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rejectableTaxlots;
        }

        /// <summary>
        /// This method gets blocks which are eligible to be rejected
        /// </summary>
        /// <param name="allocationIdList"></param>
        /// <returns></returns>
        public static DataSet GetRejectableBlocksForAT(List<string> allocationIdList)
        {
            DataSet rejectableBlocks = new DataSet();
            try
            {
                string allocationIdListCommaSeparated = string.Join(",", allocationIdList);
                object[] parameter = new object[1];
                parameter[0] = allocationIdListCommaSeparated;
                rejectableBlocks = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetRejectableBlocksForAT", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rejectableBlocks;
        }

        /// <summary>
        /// Get AllocID From BlockID
        /// </summary>
        /// <param name="blockID"></param>
        /// <returns></returns>
        public static string GetAllocIDFromBlockID(int blockID)
        {
            try
            {
                string allocId = string.Empty;
                object[] parameter = new object[1];
                parameter[0] = blockID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllocIDFromBlockID", parameter))
                {
                    while (reader.Read())
                    {
                        allocId = reader.GetValue(0).ToString();
                    }
                }
                return allocId;
            }
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
            return null;
        }

        /// <summary>
        /// This method is get the file status details for a given date 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetFileStatus(DateTime date)
        {
            DataTable jobStatusData = new DataTable();
            try
            {
                QueryData qData = new QueryData();
                qData.StoredProcedureName = "P_GetThirdPartyFileStatus";
                qData.DictionaryDatabaseParameter.Add("@runDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@runDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = date
                });
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(qData))
                {
                    jobStatusData.Load(reader);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return jobStatusData;
        }

        /// <summary>
        /// This method saves the file status for a given third party batch and date.
        /// </summary>
        /// <param name="rundate"></param>
        /// <param name="thirdPartyBatchId"></param>
        /// <returns></returns>
        public static int SaveFileStatus(DateTime rundate, int thirdPartyBatchId)
        {
            try
            {
                object[] parameter = new object[2] { rundate, thirdPartyBatchId };
                return int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyFileStatus", parameter).ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        /// <summary>
        /// This method is get the block level details for a third party batch
        /// </summary>
        /// <param name="allocationID"></param>
        /// <returns>List of ThirdPartyBlockLevelDetails if available, otherwise empty list</returns>
        public static DataSet GetThirdPartyBatchEventData(int blockId)
        {
            DataSet batchEventData = new DataSet();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = blockId;
                batchEventData = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetThirdPartyBatchEventData", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return batchEventData;
        }

        /// <summary>
        /// This method checks for mismatch status for the given entity ids
        /// </summary>
        /// <param name="runDate"></param>
        /// <param name="entityId"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static bool GetMismatchStatus(DateTime runDate, string entityId, bool isLevel2Data)
        {
            bool isMismatch = false;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = entityId;
                parameter[1] = runDate.Date.ToString("yyyy-MM-dd");
                parameter[2] = isLevel2Data;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetMismatchStatusFromAllocIds", parameter))
                {
                    while (reader.Read())
                    {
                        isMismatch = Convert.ToBoolean(reader.GetValue(0));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isMismatch;
        }

        /// <summary>
        /// This method saves details when user overrides mismatch warning for prime broker
        /// </summary>
        /// <param name="batch"></param>
        public static void SavePBMismatchOverrideAudit(ThirdPartyBatch batch)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = batch.UserId;
                parameter[1] = DateTime.Now;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyPBMismatchOverrideAudit", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Tolerance Profile
        /// <summary>
        /// Fill Third Party Tolerance Profile Common Data
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static ThirdPartyToleranceProfileCommon FillThirdPartyToleranceProfileCommonData(object[] row, int offSet)
        {
            int thirdPartyBatchId = 0 + offSet;
            int jobName = 1 + offSet;
            int executingBroker = 2 + offSet;

            ThirdPartyToleranceProfileCommon thirdPartyToleranceProfileCommon = new ThirdPartyToleranceProfileCommon();
            try
            {
                if (!(row[thirdPartyBatchId] is System.DBNull))
                {
                    thirdPartyToleranceProfileCommon.ThirdPartyBatchId = int.Parse(row[thirdPartyBatchId].ToString());
                }
                if (!(row[jobName] is System.DBNull))
                {
                    thirdPartyToleranceProfileCommon.JobName = row[jobName].ToString();
                }
                if (!(row[executingBroker] is System.DBNull))
                {
                    thirdPartyToleranceProfileCommon.ExecutingBroker = row[executingBroker].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyToleranceProfileCommon;
        }

        /// <summary>
        /// Get ThirdParty Tolerance Profile Common data
        /// </summary>
        /// <returns></returns>
        public static List<ThirdPartyToleranceProfileCommon> GetToleranceProfileCommonData()
        {
            List<ThirdPartyToleranceProfileCommon> thirdPartyToleranceProfileCommonData = new List<ThirdPartyToleranceProfileCommon>();
            try
            {
                object[] parameter = new object[0];
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyToleranceProfilesCommon", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyToleranceProfileCommonData.Add(FillThirdPartyToleranceProfileCommonData(row, 0));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyToleranceProfileCommonData;
        }

        /// <summary>
        /// Fill Third Party Tolerance Profile Data
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static ThirdPartyToleranceProfile FillThirdPartyToleranceProfileData(object[] row, int offSet)
        {
            int toleranceProfileId = 0 + offSet;
            int thirdPartyBatchId = 1 + offSet;
            int jobName = 2 + offSet;
            int executingBroker = 3 + offSet;
            int thirdPartyName = 4 + offSet;
            int lastModified = 5 + offSet;
            int matchingField = 6 + offSet;
            int avgPrice = 7 + offSet;
            int commission = 8 + offSet;
            int miscFees = 9 + offSet;
            int netMoney = 10 + offSet;

            ThirdPartyToleranceProfile thirdPartyToleranceProfile = new ThirdPartyToleranceProfile();
            try
            {
                if (!(row[toleranceProfileId] is System.DBNull))
                {
                    thirdPartyToleranceProfile.ToleranceProfileId = int.Parse(row[toleranceProfileId].ToString());
                }
                if (!(row[thirdPartyBatchId] is System.DBNull))
                {
                    thirdPartyToleranceProfile.ThirdPartyBatchId = int.Parse(row[thirdPartyBatchId].ToString());
                }
                if (!(row[jobName] is System.DBNull))
                {
                    thirdPartyToleranceProfile.JobName = row[jobName].ToString();
                }
                if (!(row[executingBroker] is System.DBNull))
                {
                    thirdPartyToleranceProfile.ExecutingBroker = row[executingBroker].ToString();
                }
                if (!(row[thirdPartyName] is System.DBNull))
                {
                    thirdPartyToleranceProfile.ThirdPartyName = row[thirdPartyName].ToString();
                }
                if (!(row[lastModified] is System.DBNull))
                {
                    thirdPartyToleranceProfile.LastModified = DateTime.Parse(row[lastModified].ToString());
                }
                if (!(row[matchingField] is System.DBNull))
                {
                    thirdPartyToleranceProfile.MatchingField = int.Parse(row[matchingField].ToString());
                }
                if (!(row[avgPrice] is System.DBNull))
                {
                    thirdPartyToleranceProfile.AvgPrice = Convert.ToDecimal(row[avgPrice].ToString());
                }
                if (!(row[commission] is System.DBNull))
                {
                    thirdPartyToleranceProfile.Commission = Convert.ToDecimal(row[commission].ToString());
                }
                if (!(row[miscFees] is System.DBNull))
                {
                    thirdPartyToleranceProfile.MiscFees = Convert.ToDecimal(row[miscFees].ToString());
                }
                if (!(row[netMoney] is System.DBNull))
                {
                    thirdPartyToleranceProfile.NetMoney = Convert.ToDecimal(row[netMoney].ToString());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyToleranceProfile;
        }

        /// <summary>
        /// Delete ThirdParty Tolerance Profile data
        /// </summary>
        /// <param name="toleranceProfileId"></param>
        public static void DeleteThirdPartyToleranceProfile(int toleranceProfileId)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = toleranceProfileId;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteThirdPartyToleranceProfile", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save ThirdParty Tolerance Profile data
        /// </summary>
        /// <param name="toleranceProfile"></param>
        /// <returns> bool wether update or not</returns>
        public static int SaveThirdPartyToleranceProfile(ThirdPartyToleranceProfile toleranceProfile)
        {
            try
            {
                object[] parameter = new object[7];
                parameter[0] = toleranceProfile.ThirdPartyBatchId;
                parameter[1] = toleranceProfile.LastModified;
                parameter[2] = toleranceProfile.MatchingField;
                parameter[3] = toleranceProfile.AvgPrice;
                parameter[4] = toleranceProfile.Commission;
                parameter[5] = toleranceProfile.MiscFees;
                parameter[6] = toleranceProfile.NetMoney;
                return Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyToleranceProfile", parameter));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Update ThirdParty Tolerance Profile data
        /// </summary>
        /// <param name="toleranceProfile"></param>
        public static void UpdateThirdPartyToleranceProfile(ThirdPartyToleranceProfile toleranceProfile)
        {
            try
            {
                object[] parameter = new object[7];
                parameter[0] = toleranceProfile.ToleranceProfileId;
                parameter[1] = toleranceProfile.LastModified;
                parameter[2] = toleranceProfile.MatchingField;
                parameter[3] = toleranceProfile.AvgPrice;
                parameter[4] = toleranceProfile.Commission;
                parameter[5] = toleranceProfile.MiscFees;
                parameter[6] = toleranceProfile.NetMoney; ;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_UpdateThirdPartyToleranceProfile", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get ThirdParty Tolerance Profiles data
        /// </summary>
        /// <returns></returns>
        public static List<ThirdPartyToleranceProfile> GetThirdPartyToleranceProfiles()
        {
            List<ThirdPartyToleranceProfile> thirdPartyToleranceProfileData = new List<ThirdPartyToleranceProfile>();
            try
            {
                object[] parameter = new object[0];
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyToleranceProfiles", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyToleranceProfileData.Add(FillThirdPartyToleranceProfileData(row, 0));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyToleranceProfileData;
        }
        #endregion

        /// <summary>
        /// This method is get file logs data
        /// </summary>
        /// <param name="runDate"></param>
        public static DataSet GetThirdPartyFileLogs(DateTime runDate)
        {
            DataSet fileAuditLogs = new DataSet();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = runDate;
                fileAuditLogs = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetThirdPartyFileAuditLogs", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return fileAuditLogs;
        }

        /// <summary>
        /// Saves the third party file logs
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="loggedInUser"></param>
        public static void SaveThirdPartyFileLogs(ThirdPartyBatch batch, string loggedInUser)
        {
            try
            {
                object[] parameter = new object[4];
                parameter[0] = loggedInUser;
                parameter[1] = batch.ThirdPartyName;
                parameter[2] = batch.Description;
                parameter[3] = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, Prana.BusinessObjects.TimeZoneInfo.EasternTimeZone);
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveThirdPartyFileAuditLogs", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves ThirdPartyTimeBatches data.
        /// </summary>
        /// <returns>A dictionary containing ThirdPartyTimeBatch lists grouped by batch ID.</returns>
        public static Dictionary<int, List<ThirdPartyTimeBatch>> GetAllThirdPartyTimeBatches()
        {
            Dictionary<int, List<ThirdPartyTimeBatch>> thirdPartyTimedBatches = new Dictionary<int, List<ThirdPartyTimeBatch>>();
            try
            {
                // Execute the stored procedure to retrieve data
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllThirdPartyTimeBatches", new object[] { }))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int batchId = int.Parse(row[1].ToString());

                        // Initialize or update the list of ThirdPartyTimeBatch objects
                        if (!thirdPartyTimedBatches.ContainsKey(batchId))
                            thirdPartyTimedBatches[batchId] = new List<ThirdPartyTimeBatch>();
                        thirdPartyTimedBatches[batchId].Add(FillThirdPartyTimedBatch(row));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and ensure no secure information leaks
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return thirdPartyTimedBatches;
        }

        /// <summary>
        /// Retrieves ThirdPartyTimeBatches based on the specified third party ID.
        /// </summary>
        /// <param name="thirdPartyId">The ID of the third party.</param>
        /// <returns>A dictionary containing ThirdPartyTimeBatch lists grouped by format ID.</returns>
        public static Dictionary<int, List<ThirdPartyTimeBatch>> GetThirdPartyTimedBatches(int thirdPartyId)
        {
            Dictionary<int, List<ThirdPartyTimeBatch>> thirdPartyTimedBatches = new Dictionary<int, List<ThirdPartyTimeBatch>>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = thirdPartyId;

                // Execute the stored procedure to retrieve data
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyTimeBatches", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int formatID = int.Parse(row[1].ToString());

                        // Initialize or update the list of ThirdPartyTimeBatch objects
                        if (!thirdPartyTimedBatches.ContainsKey(formatID))
                            thirdPartyTimedBatches[formatID] = new List<ThirdPartyTimeBatch>();
                        thirdPartyTimedBatches[formatID].Add(FillThirdPartyTimedBatch(row));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and ensure no secure information leaks
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return thirdPartyTimedBatches;
        }

        /// <summary>
        /// Creates a ThirdPartyTimeBatch object from the provided row data.
        /// </summary>
        /// <param name="row">An array containing row data.</param>
        /// <returns>A ThirdPartyTimeBatch object.</returns>
        public static ThirdPartyTimeBatch FillThirdPartyTimedBatch(object[] row)
        {
            ThirdPartyTimeBatch thirdPartyTimeBatch = new ThirdPartyTimeBatch();
            try
            {
                if (row[0] != null)
                    thirdPartyTimeBatch.ID = int.Parse(row[0].ToString());
                if (row[2] != null)
                    thirdPartyTimeBatch.BatchRunTime = DateTime.Parse(row[2].ToString());
                if (row[3] != null)
                    thirdPartyTimeBatch.IsPaused = bool.Parse(row[3].ToString());
            }
            catch (Exception ex)
            {
                // Handle exceptions and ensure no secure information leaks
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return thirdPartyTimeBatch;
        }

        /// <summary>
        /// Updates the execution details for an automated batch.
        /// </summary>
        /// <param name="batchID">The ID of the batch.</param>
        /// <param name="scheduledTime">The scheduled time for the batch.</param>
        /// <param name="isSuccess">True if the batch execution was successful; otherwise, false.</param>
        public static void UpdateAutomatedBatchExecutionDetail(int batchID, string scheduledTime, bool isSuccess)
        {
            try
            {
                // Prepare parameters for the stored procedure
                object[] parameter = new object[3];
                parameter[0] = batchID;
                parameter[1] = isSuccess;
                parameter[2] = scheduledTime;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateAutomatedBatchExecutionDetail", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves the status of third-party automated batches based on the current EST time.
        /// </summary>
        /// <param name="currentESTTime">The current Eastern Standard Time.</param>
        /// <returns>A dataset containing batch status information.</returns>
        public static DataSet LoadThirdPartyAutomatedBatchStatus(DateTime currentESTTime)
        {
            DataSet result = null;
            try
            {
                // Prepare parameters for the stored procedure
                object[] parameter = new object[1];
                parameter[0] = currentESTTime.ToString();

                result = DatabaseManager.DatabaseManager.ExecuteDataSet("P_LoadThirdPartyAutomatedBatchStatus", parameter);                
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }
    }
}
