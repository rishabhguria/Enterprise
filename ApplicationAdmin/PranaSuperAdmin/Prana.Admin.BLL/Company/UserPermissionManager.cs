using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// busineed logic class for user permission mapping
    /// </summary>
    public class UserPermissionManager
    {
        /// <summary>
        /// global variable for holding the permissionID
        /// </summary>
        public static int _permissionID = 0;

        public static Dictionary<int, string> _dictPrincipalType;//= new Dictionary<int, string>();
                                                                 // public static Dictionary<int, string> _dictPrincipalValue; //= new Dictionary<int, string>();
        public static Dictionary<int, string> _dictResourceType; //= new Dictionary<int, string>();
        //public static Dictionary<int, string> _dictResourceValue; //= new Dictionary<int, string>();
        public static Dictionary<int, string> _dictModule; //= new Dictionary<int, string>();
        public static Dictionary<int, string> _dictAccountGroups; //= new Dictionary<int, string>();
        public static Dictionary<int, string> _dictAuthAction; //= new Dictionary<int, string>();
        public static Dictionary<int, string> _dictAuthRoles;// = new Dictionary<int, string>();

        /// <summary>
        /// Get the permission details
        /// </summary>
        /// <returns>datatable holding the permissions</returns>
        public static DataTable GetUserPermissions()
        {
            DataTable dtPermission = new DataTable("dtPermission");
            try
            {
                dtPermission = UserPermissionDAL.GetPermisssionsFromDB();
                if (dtPermission.Rows.Count <= 0)
                {
                    _permissionID = 1;
                }
                else
                {
                    _permissionID = GetPermissionID(dtPermission);
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
            return dtPermission;
        }

        /// <summary>
        /// Get ID to assign to the new permission
        /// </summary>
        /// <param name="dtPermission">Datatable holding all the permissions</param>
        /// <returns>Integer ID for permission</returns>
        private static int GetPermissionID(DataTable dtPermission)
        {
            int i = 0;
            try
            {
                foreach (DataRow row in dtPermission.Rows)
                {
                    if (!string.IsNullOrEmpty(row["PermissionID"].ToString()) && Convert.ToInt32(row["PermissionID"]) > i)
                    {
                        i = Convert.ToInt32(row["PermissionID"]);
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
            return i;
        }

        /// <summary>
        /// Get the principal types
        /// </summary>
        /// <returns>dictionary of principal types</returns>
        public static Dictionary<int, string> GetPrincipalTypes()
        {
            return _dictPrincipalType;
        }

        /// <summary>
        /// Get the authorization roles
        /// </summary>
        /// <returns>dictionary of authorization roles</returns>
        public static Dictionary<int, string> GetAuthRoles()
        {
            //_dictAuthRoles.Remove(0);
            return _dictAuthRoles;
        }

        /// <summary>
        /// Get the resource types
        /// </summary>
        /// <returns>dictionary of resource types</returns>
        public static Dictionary<int, string> GetResourceTypes()
        {
            return _dictResourceType;
        }

        /// <summary>
        /// Get the resource values
        /// </summary>
        /// <returns>dictionary of resource values</returns>
        public static Dictionary<int, string> GetResourceValues()
        {
            int formatID = 0;
            Dictionary<int, string> dictResourceValue = new Dictionary<int, string>();
            try
            {
                foreach (ModuleResources val in ModuleResources.GetValues(typeof(ModuleResources)))
                {
                    dictResourceValue.Add(formatID++, val.ToString());
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
            return dictResourceValue;
        }

        /// <summary>
        /// Get the authorization action like read, write
        /// </summary>
        /// <returns>dictionary of authorization actions</returns>
        public static Dictionary<int, string> GetAuthActions()
        {
            return _dictAuthAction;
        }

        /// <summary>
        /// Get the dictionaries filled with the authorization data
        /// </summary>
        public static void GetAuthData()
        {
            try
            {
                _dictPrincipalType = new Dictionary<int, string>();
                //_dictPrincipalValue = new Dictionary<int, string>();
                _dictResourceType = new Dictionary<int, string>();
                //_dictResourceValue = new Dictionary<int, string>();
                _dictModule = new Dictionary<int, string>();
                _dictAccountGroups = new Dictionary<int, string>();
                _dictAuthAction = new Dictionary<int, string>();
                _dictAuthRoles = new Dictionary<int, string>();

                DataSet keyValuePairs = UserPermissionDAL.GetAuthKeyValuePairs();
                _dictAuthAction = FillAuthKeyValuePairs(keyValuePairs.Tables[0], 0);
                _dictPrincipalType = FillAuthKeyValuePairs(keyValuePairs.Tables[1], 0);
                _dictResourceType = FillAuthKeyValuePairs(keyValuePairs.Tables[2], 0);
                //_dictModule = FillAuthKeyValuePairs(keyValuePairs.Tables[3], 0);
                _dictAccountGroups = FillAuthKeyValuePairs(keyValuePairs.Tables[4], 0);
                _dictAuthRoles = FillAuthKeyValuePairs(keyValuePairs.Tables[5], 0);

                //To Do: Comment the following two rows when the implementation for the permissions
                //on the basis of the client and user has been done
                _dictPrincipalType.Remove(1);
                _dictPrincipalType.Remove(3);
                //To Do: finished
                int formatID = 0;
                foreach (ModuleResources val in ModuleResources.GetValues(typeof(ModuleResources)))
                {
                    _dictModule.Add(formatID++, val.ToString());
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Fill the dictionary with the data in the datatable
        /// </summary>
        /// <param name="keyValues">datatable holding the data</param>
        /// <param name="offset">offset for setting the value index</param>
        /// <returns>dictionary of data</returns>
        private static Dictionary<int, string> FillAuthKeyValuePairs(DataTable keyValues, int offset)
        {
            Dictionary<int, string> keyValue = new Dictionary<int, string>();
            int id = 0;
            int value = 1 + offset;
            try
            {
                foreach (DataRow dr in keyValues.Rows)
                {
                    if (keyValue.ContainsKey(Convert.ToInt32(dr[id].ToString())))
                    {
                        keyValue[Convert.ToInt32(dr[id].ToString())] = dr[value].ToString();
                    }
                    else
                    {
                        keyValue.Add(Convert.ToInt32(dr[id].ToString()), dr[value].ToString());
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
            return keyValue;
        }

        /// <summary>
        /// Get the resource values
        /// </summary>
        /// <returns>dictionary of resource values</returns>
        public static Dictionary<int, string> GetResourceValuesAccountGroups()
        {
            return _dictAccountGroups;
        }

        /// <summary>
        /// Get modules for permissions
        /// </summary>
        /// <returns>dictionary of data</returns>
        public static Dictionary<int, string> GetResourceValuesModule()
        {
            return _dictModule;
        }

        /// <summary>
        /// Save the permissions in the database
        /// </summary>
        /// <param name="dtPermissions">Datatable holding the data</param>
        /// <returns>number of affected rows</returns>
        public static int SaveUserPermissions(DataTable dtPermissions)
        {
            int i = 0;

            try
            {
                DataSet ds = new DataSet("dsPermission");
                DataTable dt = new DataTable();
                dtPermissions.AcceptChanges();
                dt = dtPermissions.Copy();
                dt.TableName = "dtPermission";
                SetPermissionIDs(dt);
                CheckDuplicates(dt);
                if (dt.TableName.Equals("Duplicate"))
                {
                    return -1;
                }
                ds.Tables.Add(dt);
                String xmlPermission = ds.GetXml();
                i = UserPermissionDAL.SavePermissionsInDB(xmlPermission);
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
            return i;
        }

        /// <summary>
        /// Check for the duplicate permissions
        /// </summary>
        /// <param name="dt">datatable holding the details</param>
        private static void CheckDuplicates(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr1["PermissionID"].ToString()) && Convert.ToInt32(dr["PermissionID"]) == Convert.ToInt32(dr1["PermissionID"]))
                        {
                            continue;
                        }
                        else
                        {
                            int prType1 = Convert.ToInt32(dr["PrincipalType"]);
                            int prType2 = Convert.ToInt32(dr1["PrincipalType"]);
                            int prVal1 = Convert.ToInt32(dr["PricipalValue"]);
                            int prVal2 = Convert.ToInt32(dr1["PricipalValue"]);
                            int resType1 = Convert.ToInt32(dr["ResourceDataType"]);
                            int resType2 = Convert.ToInt32(dr1["ResourceDataType"]);
                            int resVal1 = Convert.ToInt32(dr["ResourceDataValue"]);
                            int resVal2 = Convert.ToInt32(dr1["ResourceDataValue"]);
                            if (prType1 == prType2 && prVal1 == prVal2 && resType1 == resType2 && resVal1 == resVal2)
                            {
                                dt.TableName = "Duplicate";
                                return;
                            }
                        }
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
        }

        /// <summary>
        /// Set the IDs for the new permissions
        /// </summary>
        /// <param name="dt">Datatable of permission details</param>
        private static void SetPermissionIDs(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr["PermissionID"].ToString()))
                    {
                        dr["PermissionID"] = ++_permissionID;
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
        }
    }
}
