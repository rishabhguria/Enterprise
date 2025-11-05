using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// FixManager would be a static class to handel <see cref="Fix"/> related details.
    /// </summary>
    /// <remarks>Its a Static class like other managers class so that we don't have to instatiate it again and again while working with them.</remarks>
    public class Fixmanager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Fixmanager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Basic methods like Add/Update/Delete/Get

        public static Fix FillFixes(object[] row, int offSet)
        {
            int fixID = 0 + offSet;
            int fixVersion = 1 + offSet;

            Fix fix = new Fix();
            try
            {
                if (row[fixID] != null)
                {
                    fix.FixID = int.Parse(row[fixID].ToString());
                }
                if (row[fixVersion] != null)
                {
                    fix.FixVersion = row[fixVersion].ToString();
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
            return fix;
        }

        /// <summary>
        /// Gets <see cref="Fix"/> from the database.
        /// </summary>
        /// <param name="fixID">ID of <see cref="Fix"/> to be fetched from database.</param>
        /// <returns><see cref="Fix"/> fetched.</returns>
        public static Fixs GetFixs()
        {
            Fixs fixs = new Fixs();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFixs";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        fixs.Add(FillFixes(row, 0));
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
            return fixs;
        }

        public static int SaveFix(Fix fix)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            parameter[0] = fix.FixID;
            parameter[1] = fix.FixVersion;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveFix", parameter).ToString());
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

        public static bool DeleteFix(int fixID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = fixID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteFix", parameter) > 0)
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

        #region Basic methods like Add/Update/Delete/Get for FIX Capability

        public static FixCapability FillFixCapabilities(object[] row, int offSet)
        {
            int fixCapabilityID = 0 + offSet;
            int description = 1 + offSet;

            FixCapability fixCapability = new FixCapability();
            try
            {
                if (row[fixCapabilityID] != null)
                {
                    fixCapability.FixCapabilityID = int.Parse(row[fixCapabilityID].ToString());
                }
                if (row[description] != null)
                {
                    fixCapability.Name = row[description].ToString();
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
            return fixCapability;
        }

        /// <summary>
        /// Gets <see cref="FixCapability"/> from the database.
        /// </summary>
        /// <param name="fixCapabilityID">ID of <see cref="Fix"/> to be fetched from database.</param>
        /// <returns><see cref="FixCapability"/> fetched.</returns>
        public static FixCapabilities GetFixCapabilities()
        {
            FixCapabilities fixCapabilities = new FixCapabilities();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFixCapabilities";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        fixCapabilities.Add(FillFixCapabilities(row, 0));
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
            return fixCapabilities;
        }

        public static int SaveFixCapability(FixCapability fixCapability)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            parameter[0] = fixCapability.FixCapabilityID;
            parameter[1] = fixCapability.Name;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveFixCapability", parameter).ToString());
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

        public static bool DeleteFixCapability(int fixCapabilityID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = fixCapabilityID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteFixCapability", parameter) > 0)
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
    }
}
