#region Using namespaces

using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;
#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AssetManager.
    /// </summary>
    public class AssetManager
    {
        private AssetManager()
        {
        }

        #region Asset Methods

        /// <summary>
        /// FillAssets is a method to fill a object of <see cref="Asset"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Asset"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="Asset"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of Asset class. So, if the row only contains result from T_Asset table then value of Offset would be zero ("0"), and, lets say the row contains value of Asset as well as any other table then we have to specify the offset from where the values of Asset starts. Note: Sequence of Asset class whould be always same as in this method.</remarks>		
        public static Asset FillAssets(object[] row, int offSet)
        {
            int assetID = 0 + offSet;
            int assetName = 1 + offSet;
            int comment = 2 + offSet;

            Asset asset = new Asset();

            try
            {
                if (!(row[assetID] is System.DBNull))
                {
                    asset.AssetID = int.Parse(row[assetID].ToString());
                }
                if (!(row[assetName] is System.DBNull))
                {
                    asset.Name = row[assetName].ToString();
                }
                if (!(row[comment] is System.DBNull))
                {
                    asset.Comment = row[comment].ToString();
                }
            }
            //			catch(DataAccessException ex)
            //			{
            //				bool rethrow = Logger.HandleException(ex, "Data Access Policy");
            //				if (rethrow)
            //				{
            //					throw;
            //				}
            //			}
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, "General Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return asset;
        }

        /// <summary>
        /// Added for Prana CLIENT 16 01 06
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Assets GetCompanyUserAssets(int userID)
        {
            Assets assets = new Assets();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAssetsByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        assets.Add(FillCompanyAssets(row, 0));

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
            return assets;
        }
        /// <summary>
        /// Gets all <see cref="Asset"/> in <see cref="Assets"/> collection.
        /// </summary>
        /// <returns>Object of <see cref="Assets"/>.</returns>
        public static Assets GetAssets()
        {
            Assets assets = new Assets();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllAssets";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        assets.Add(FillAssets(row, 0));
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
            return assets;
        }

        /// <summary>
        /// Gets all <see cref="Asset"/> in <see cref="Assets"/> collection. Fetches all the assets which are
        /// only present in AUEC not all in Assets table.
        /// </summary>
        /// <returns>Object of <see cref="Assets"/>.</returns>
        public static Assets GetAUECAssets(int assetID)
        {
            Assets auecAssets = new Assets();

            object[] parameter = new object[1];
            parameter[0] = assetID;
            try
            {
                //using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader(CommandType.StoredProcedure, "P_GetAllAUECAssets"))
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllAUECAssets", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        auecAssets.Add(FillAssets(row, 0));
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
            return auecAssets;
        }

        /// <summary>
        /// Gets all <see cref="UnderLying"/> in <see cref="UnderLyings"/> collection. Fetches all the underlyings which are
        /// only present in AUEC not all in UnderLying table.
        /// </summary>
        /// <returns>Object of <see cref="Assets"/>.</returns>
        public static UnderLyings GetAUECUnderLyings(int assetID, int underLyingID)
        {
            UnderLyings auecUnderLyings = new UnderLyings();

            try
            {
                object[] parameter = new object[2];
                parameter[0] = assetID;
                parameter[1] = underLyingID;
                //using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader(CommandType.StoredProcedure, "P_GetAllAUECUnderlyings"))
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllAUECUnderlyings", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        auecUnderLyings.Add(FillUnderLyings(row, 0));
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
            return auecUnderLyings;
        }



        /// <summary>
        /// Gets <see cref="Asset"/> for given assetID.
        /// </summary>
        /// <param name="assetID">AssetID for which <see cref="Asset"/> is required.</param>
        /// <returns>Object of <see cref="Asset"/>.</returns>
        public static Asset GetAssets(int assetID)
        {
            Asset asset = null;

            Object[] parameter = new object[1];
            parameter[0] = (int)assetID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAssetByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        asset = FillAssets(row, 0);
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
            return asset;
        }

        /// <summary>
        /// Saves <see cref="Asset"/> in the database.
        /// </summary>
        /// <param name="asset"><see cref="Asset"/> to be saved.</param>
        /// <returns>
        /// ID of the <see cref="Asset"/> saved(inserted/updated)
        /// </returns>
        public static int SaveAsset(Asset asset)
        {
            int result = int.MinValue;

            try
            {
                //				if (result != int.MinValue)
                //				{
                object[] parameter = new object[3];
                parameter[0] = asset.AssetID;
                parameter[1] = asset.Name;
                parameter[2] = asset.Comment;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveAsset", parameter).ToString());
                //				}
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
        /// Delets <see cref="Asset"/> from the datatbase.
        /// </summary>
        /// <param name="assetID">ID of <see cref="Asset"/> to be deleted.</param>
        /// <returns>
        /// true: If <see cref="Asset"/> deleted successfully.
        /// false: If <see cref="Asset"/> could not be deleted successfully.
        /// </returns>
        public static bool DeleteAsset(int assetID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = assetID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAsset", parameter) > 0)
                {
                    result = true;
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
        #endregion

        #region CompanyAssets
        /// <summary>
        /// FillCompanyAssets is a method to fill a object of <see cref="Asset"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Asset"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="Asset"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of Asset class. So, if the row only contains result from T_Asset table then value of Offset would be zero ("0"), and, lets say the row contains value of Asset as well as any other table then we have to specify the offset from where the values of Asset starts. Note: Sequence of Asset class whould be always same as in this method.</remarks> 
        public static Asset FillCompanyAssets(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int assetID = 1 + offSet;
            int assetName = 2 + offSet;
            int comment = 3 + offSet;

            Asset asset = new Asset();
            try
            {
                if (row[companyID] != null)
                {
                    asset.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[assetID] != null)
                {
                    asset.AssetID = int.Parse(row[assetID].ToString());
                }
                if (row[assetName] != null)
                {
                    asset.Name = row[assetName].ToString();
                }
                if (row[comment] != null)
                {
                    asset.Comment = row[comment].ToString();
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
            return asset;
        }

        /// <summary>
        /// Gets all <see cref="Asset"/> in <see cref="Assets"/> collection for a given companyID.
        /// </summary>
        /// <param name="companyID">companyID for which <see cref="Assets"/> are required.</param>
        /// <returns>Object of <see cref="Assets"/>.</returns>
        public static Assets GetCompanyAssets(int companyID)
        {
            Assets assets = new Assets();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyAssets", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        assets.Add(FillCompanyAssets(row, 0));

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
            return assets;
        }

        #endregion

        #region UnderLyings Methods

        /// <summary>
        /// FillUnderLyings is a method to fill a object of <see cref="Underlying"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Asset"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="UnderLying"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of UnderLying class. So, if the row only contains result from T_UnderLying table then value of Offset would be zero ("0"), and, lets say the row contains value of UnderLying as well as any other table then we have to specify the offset from where the values of UnderLying starts. Note: Sequence of UnderLying class whould be always same as in this method.</remarks>		
        public static UnderLying FillUnderLyings(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int name = 1 + offSet;
            int assetID = 2 + offSet;
            int comment = 3 + offSet;

            UnderLying underLying = new UnderLying();
            try
            {
                if (!(row[ID] is System.DBNull))
                {
                    underLying.UnderlyingID = int.Parse(row[ID].ToString());
                }
                if (!(row[name] is System.DBNull))
                {
                    underLying.Name = row[name].ToString();
                }
                if (!(row[assetID] is System.DBNull))
                {
                    underLying.AssetID = int.Parse(row[assetID].ToString());
                }
                if (!(row[comment] is System.DBNull))
                {
                    underLying.Comment = row[comment].ToString();
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
            return underLying;
        }

        /// <summary>
        /// Gets all <see cref="UnderLying"/> in <see cref="UnderLyings"/> collection.
        /// </summary>
        /// <returns>Object of <see cref="UnderLyings"/>.</returns>
        public static UnderLyings GetUnderLyings()
        {
            UnderLyings underLyings = new UnderLyings();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllUnderLyings";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLyings.Add(FillUnderLyings(row, 0));
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
            return underLyings;
        }

        /// <summary>
        /// Gets <see cref="UnderLying"/> for a given companyID. for a given companyID.
        /// </summary>
        /// <param name="companyID">companyID for which <see cref="UnderLying"/> is required.</param>
        /// <returns>Object of <see cref="UnderLying"/>.</returns>
        public static UnderLying GetUnderLyingForCompany(int companyID)
        {
            UnderLying underLying = null;

            Object[] parameter = new object[1];
            try
            {
                parameter[0] = companyID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsForCompany", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLying = FillUnderLyings(row, 0);
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
            return underLying;
        }

        public static UnderLyings GetUnderLyingsByAssetAndUserID(int userID, int assetID)
        {
            UnderLyings underLyings = new UnderLyings();

            Object[] parameter = new object[2];
            try
            {
                parameter[0] = userID;
                parameter[1] = assetID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsByAssetAndUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLyings.Add(FillUnderLyings(row, 0));
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
            return underLyings;
        }



        /// <summary>
        /// Gets <see cref="UnderLying"/> for given underLyingID.
        /// </summary>
        /// <param name="underLyingID">UnderLyingID for which <see cref="UnderLying"/> is required.</param>
        /// <returns>Object of <see cref="UnderLying"/>.</returns>
        public static UnderLying GetUnderLying(int underLyingID)
        {
            UnderLying underLying = null;

            Object[] parameter = new object[1];
            try
            {
                parameter[0] = (int)underLyingID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLying = FillUnderLyings(row, 0);
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
            return underLying;
        }

        /// <summary>
        ///  Gets all <see cref="UnderLying"/> in <see cref="UnderLyings"/> collection for given AssetID.
        /// </summary>
        /// <param name="assetID">AssetID for which UnderLying is seeked.</param>
        /// <returns>Object of <see cref="UnderLyings"/>.</returns>
        public static UnderLyings GetUnderLyings(int assetID)
        {
            UnderLyings underLyings = new UnderLyings();
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = (int)assetID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsByAssetID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLyings.Add(FillUnderLyings(row, 0));
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
            return underLyings;
        }


        public static UnderLyings GetUnderLyingsByUserID(int userID)
        {
            UnderLyings underLyings = new UnderLyings();
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = userID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLyings.Add(FillUnderLyings(row, 0));
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
            return underLyings;
        }

        /// <summary>
        /// Saves <see cref="UnderLying"/> in the database.
        /// </summary>
        /// <param name="underLying"><see cref="UnderLying"/> to be saved.</param>
        /// <returns>ID of the <see cref="UnderLying"/> saved(inserted/updated)</returns>
        public static int SaveUnderLying(UnderLying underLying)
        {
            int result = int.MinValue;

            try
            {
                //				if(result != int.MinValue)
                //				{
                object[] parameter = new object[4];

                parameter[0] = underLying.UnderlyingID;
                parameter[2] = underLying.Name;
                parameter[1] = underLying.AssetID;
                parameter[3] = underLying.Comment;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUnderlying", parameter).ToString());
                //				}
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
        /// Deletes <see cref="Underlying"/> from the datatbase.
        /// </summary>
        /// <param name="underlyingID">ID of <see cref="Underlying"/> to be deleted.</param>
        /// <param name="deleteForceFully">To delete <see cref="Underlying"/> desipte of its reference in other forms when this parameter is true.</param>
        /// <returns>true: If <see cref="Underlying"/> deleted successfully.
        /// false: If <see cref="Underlying"/> could not be deleted successfully.</returns>
        public static bool DeleteUnderlying(int underlyingID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = underlyingID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteUnderlying", parameter) > 0)
                {
                    result = true;
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



        #endregion

        #region CompanyUnderLyings
        /// <summary>
        /// FillCompanyUnderLyings is a method to fill a object of <see cref="UnderLying"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="UnderLying"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="UnderLying"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of UnderLying class. So, if the row only contains result from T_CompanyUnderLying table then value of Offset would be zero ("0"), and, lets say the row contains value of UnderLying as well as any other table then we have to specify the offset from where the values of UnderLying starts. Note: Sequence of UnderLying class whould be always same as in this method.</remarks>		
        public static UnderLying FillCompanyUnderLyings(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int underLyingID = 1 + offSet;
            int name = 2 + offSet;
            int assetID = 3 + offSet;
            int comment = 4 + offSet;

            UnderLying underLying = new UnderLying();
            try
            {
                if (row[companyID] != null)
                {
                    underLying.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[underLyingID] != null)
                {
                    underLying.UnderlyingID = int.Parse(row[underLyingID].ToString());
                }
                if (row[name] != null)
                {
                    underLying.Name = row[name].ToString();
                }
                if (row[assetID] != null)
                {
                    underLying.AssetID = int.Parse(row[assetID].ToString());
                }
                if (row[comment] != null)
                {
                    underLying.Comment = row[comment].ToString();
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
            return underLying;
        }


        //The following method not used now.
        /// <summary>
        /// Gets all <see cref="UnderLying"/> in <see cref="UnderLyings"/> collection for a given companyID.
        /// </summary>
        /// <param name="companyID">companyID for which <see cref="UnderLying"/> in <see cref="UnderLyings"/> are required.</param>
        /// <returns>Object of <see cref="UnderLyings"/>.</returns>

        public static UnderLyings GetCompanyUnderLyings(int companyID)
        {
            UnderLyings companyUnderLyings = new UnderLyings();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUnderLyings", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUnderLyings.Add(FillCompanyUnderLyings(row, 0));

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
            return companyUnderLyings;
        }

        /// <summary>
        /// Gets all <see cref="UnderLying"/> in <see cref="UnderLyings"/> collection for a given userID.
        /// </summary>
        /// <param name="userID">companyID for which <see cref="UnderLying"/> in <see cref="UnderLyings"/> are required. In this method the parameter userID here refers to the companyuserID.</param>
        /// <returns>Object of <see cref="UnderLyings"/>.</returns>
        public static UnderLyings GetCompanyUserUnderLyings(int userID)
        {
            UnderLyings companyUserUnderLyings = new UnderLyings();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUserUnderLyings.Add(FillUnderLyings(row, 0));

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
            return companyUserUnderLyings;
        }

        #endregion
    }
}
