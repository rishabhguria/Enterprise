#region Using

using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// PermissionManager is a Manager class to get related permissions of a user 
    /// and other permissions defined in the system.
    /// </summary>
    public class PermissionManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private PermissionManager()
        {
        }

        /// <summary>
        /// Maps permissions defined in database to <see cref="Permissions"/> in the system.
        /// </summary>
        /// <param name="row">Database row to be mapped.</param>
        /// <param name="offSet">Used in case of permissions fetched along with some other entity data.
        /// 0 - if data is fetched from the permissions table only.
        /// n - if the data is fetched through a join and the permission's coloumn starts at nth column.
        /// </param>
        /// <returns>Object of <see cref="Permission"/>.</returns>
        public static Permission FillPermissions(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            //int permissionName = 1 + offSet;
            int PermissionTypeID = 1 + offSet;
            int ModuleID = 2 + offSet;

            Permission permission = new Permission();
            try
            {
                if (row[ID] != System.DBNull.Value)
                {
                    permission.PermissionID = int.Parse(row[ID].ToString());
                }
                //if (row[permissionName] != null)
                //{
                //    permission.PermissionName = row[permissionName].ToString();
                //}
                if (row[PermissionTypeID] != System.DBNull.Value)
                {
                    permission.PermissionTypeID = int.Parse(row[PermissionTypeID].ToString());
                }
                if (row[ModuleID] != System.DBNull.Value)
                {
                    permission.ModuleID = int.Parse(row[ModuleID].ToString());
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
            return permission;
        }

        /// <summary>
        /// Gets all the <see cref="Permissions"/> defined in system.
        /// </summary>
        /// <returns>Object of <see cref="Permissions"/>.</returns>
        public static Permissions GetPermissions()
        {
            Permissions allPermissions = new Permissions();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllPermissions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        allPermissions.Add(FillPermissions(row, 0));
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
            return allPermissions;
        }

        /// <summary>
        /// Gets all the <see cref="Permissions"/> for a given <see cref="User"/>.
        /// </summary>
        /// <param name="userID">ID of <see cref="User"/> whoes permissions are required.</param>
        /// <returns><see cref="Permissions"/> of user.</returns>
        public static Permissions GetPermissions(int userID)
        {
            Permissions permissions = new Permissions();

            object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUserPermissions", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        permissions.Add(FillPermissions(row, 0));
                    }
                }
                if (permissions.Count > 0)
                {
                    permissions.UserID = userID;
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
            return permissions;
        }

        /// <summary>
        /// Gets all th details of the <see cref="Permission"/> whoes ID is passed.
        /// </summary>
        /// <param name="permissionID">ID of <see cref="Permission"/> whoes details are required.</param>
        /// <returns><see cref="Permission"/>.</returns>
        //public static Permission GetPermission(int permissionID)
        //{
        //    //ToDo: Impliment it whenever required.
        //    return null;
        //}

        #region PermissionTypes
        public static PermissionType FillPermissionType(object[] row, int offSet)
        {
            int permissionTypeID = 0 + offSet;
            int permissionTypeName = 1 + offSet;

            PermissionType permissionType = new PermissionType();
            try
            {

                if (!(row[permissionTypeID] is System.DBNull))
                {
                    permissionType.PermissionTypeID = int.Parse(row[permissionTypeID].ToString());
                }
                if (!(row[permissionTypeName] is System.DBNull))
                {
                    permissionType.PermissionTypeName = row[permissionTypeName].ToString();
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
            return permissionType;
        }

        /// <summary>
        /// Gets <see cref="PermissionType"/> corresponding to specified ID.
        /// </summary>
        /// <param name="permissionTypeID">ID for which <see cref="PermissionType"/> is sought.</param>
        /// <returns>Object of <see cref="PermissionType"/></returns>
        public static PermissionType GetPermissionType(int permissionTypeID)
        {
            PermissionType permissionType = new PermissionType();

            Object[] parameter = new object[1];
            parameter[0] = permissionTypeID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetPermissionType", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        permissionType = FillPermissionType(row, 0);
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
            return permissionType;
        }
        #endregion


    }
}
