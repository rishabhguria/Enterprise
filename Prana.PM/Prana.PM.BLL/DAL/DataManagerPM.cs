using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.PM.BLL
{
    public class DataManagerPM
    {
        #region PMCompanyDataSources
        public static bool SavePMCompanyDataSources(int companyID, SortableSearchableList<ThirdPartyNameID> dataSourceNameIDList)
        {
            //bool saved = false;
            //int result = int.MinValue;
            //StringBuilder moduleIDStringBuilder = new StringBuilder();
            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyModules", parameter).ToString();
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeletePMCompanyDataSources", parameter).ToString();
                foreach (ThirdPartyNameID dataSourceNameID in dataSourceNameIDList)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = dataSourceNameID.ID;

                    int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyPMDataSources", parameter).ToString());

                    //moduleIDStringBuilder.Append("'");
                    //moduleIDStringBuilder.Append(result.ToString());
                    //moduleIDStringBuilder.Append("',");
                }

                //int len = moduleIDStringBuilder.Length;
                //if (moduleIDStringBuilder.Length > 0)
                //{
                //    moduleIDStringBuilder.Remove((len - 1), 1);
                //}
                //parameter = new object[2];

                //parameter[0] = companyID;
                //parameter[1] = moduleIDStringBuilder.ToString();
                ////				if(moduleIDStringBuilder.Length > 0)
                ////				{
                //db.ExecuteNonQuery("P_DeleteCompanyModules", parameter).ToString();
                ////				}	

                ////Delete/Update all those Modules of 'company users' which are now not present for company.
                //parameter = new object[1];
                //parameter[0] = companyID;
                ////				db.ExecuteNonQuery("P_UpdateCompanyUserModules", parameter);
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
            return true;
        }

        public static ThirdPartyNameID FillCompanyDataSourceNameID(object[] row, int offSet)
        {
            int dataSourceNameID = 0 + offSet;

            ThirdPartyNameID objDataSourceNameID = new ThirdPartyNameID();
            try
            {
                if (row[dataSourceNameID] != null)
                {
                    objDataSourceNameID.ID = int.Parse(row[dataSourceNameID].ToString());
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
            return objDataSourceNameID;
        }

        public static SortableSearchableList<ThirdPartyNameID> GetCompanyPMDataSources(int companyID)
        {
            SortableSearchableList<ThirdPartyNameID> dataSourceNameIDList = new SortableSearchableList<ThirdPartyNameID>();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPMDataSource", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceNameIDList.Add(FillCompanyDataSourceNameID(row, 0));
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
            return dataSourceNameIDList;
        }




        #endregion
    }
}
