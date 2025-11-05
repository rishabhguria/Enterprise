using Prana.BusinessObjects.Enums;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ModuleManager.
    /// </summary>
    public class ModuleManager
    {
        private ModuleManager()
        {
        }

        public static Module FillModule(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Module module = new Module();
            try
            {
                if (row != null)
                {
                    int MODULEID = offset + 0;
                    int MODULENAME = offset + 1;

                    module.ModuleID = Convert.ToInt32(row[MODULEID]);
                    module.ModuleName = Convert.ToString(row[MODULENAME]);
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
            return module;
        }

        /// <summary>
        /// Gets <see cref="Module"/> corresponding to specified ID.
        /// </summary>
        /// <param name="moduleID">ID for which <see cref="Module"/> is sought.</param>
        /// <returns>Object of <see cref="Module"/></returns>
        public static Module GetModule(int moduleID)
        {
            Module module = new Module();

            Object[] parameter = new object[1];
            parameter[0] = moduleID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetModule", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        module = FillModule(row, 0);
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
            return module;
        }

        public static Modules GetModules()
        {
            Modules modules = new Modules();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllModules";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        modules.Add(FillModule(row, 0));
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
            return modules;
        }

        public static int SaveModule(Module module)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            parameter[0] = module.ModuleID;
            parameter[1] = module.ModuleName;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveModule", parameter).ToString());
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

        public static bool DeleteModule(int moduleID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = moduleID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteModule", parameter) > 0)
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

        #region CompanyModules
        public static Module FillCompanyModules(object[] row, int offSet)
        {
            int companyModuleID = 0 + offSet;
            int companyID = 1 + offSet;
            int moduleName = 2 + offSet;
            int moduleID = 3 + offSet;
            int read_writeID = 4 + offSet;

            Module companyModule = new Module();
            try
            {
                if (row[companyModuleID] != null)
                {
                    companyModule.CompanyModuleID = int.Parse(row[companyModuleID].ToString());
                }
                if (row[companyID] != System.DBNull.Value)
                {
                    companyModule.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[moduleName] != System.DBNull.Value)
                {
                    companyModule.ModuleName = row[moduleName].ToString();
                }
                if (row[moduleID] != System.DBNull.Value)
                {
                    companyModule.ModuleID = int.Parse(row[moduleID].ToString());
                }
                if (row[read_writeID] != System.DBNull.Value)
                {
                    companyModule.ReadWriteID = int.Parse(row[read_writeID].ToString());
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
            return companyModule;
        }

        public static Modules GetCompanyModules(int companyID)
        {
            Modules companyModules = new Modules();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyModules", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //companyModules.Add(FillModule(row, 0));		
                        companyModules.Add(FillCompanyModules(row, 0));
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
            return companyModules;
        }



        public static Modules GetModulesForCompany(int companyID)
        {
            Modules companyModules = new Modules();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyModulesForCompany", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyModules.Add(FillCompanyModules(row, 0));
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
            return companyModules;
        }

        public static Module GetCompanyModule(int companyModuleID)
        {
            Module companyModule = new Module();

            Object[] parameter = new object[1];
            parameter[0] = companyModuleID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyModule", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyModule = FillCompanyModules(row, 0);
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
            return companyModule;
        }

        #endregion

        #region CompanyUserModules
        public static Module FillCompanyUserModules(object[] row, int offSet)
        {
            int companyUserModuleID = 0 + offSet;
            int companyModuleID = 1 + offSet;
            int moduleName = 2 + offSet;
            int read_writeID = 3 + offSet;
            int moduleId = 4 + offSet;
            int isShow_export = 5 + offSet;

            Module companyUserModule = new Module();
            try
            {
                if (row[companyUserModuleID] != System.DBNull.Value)
                {
                    companyUserModule.CompanyID = int.Parse(row[companyUserModuleID].ToString());
                }
                if (row[companyModuleID] != System.DBNull.Value)
                {
                    companyUserModule.CompanyModuleID = int.Parse(row[companyModuleID].ToString());
                }
                if (row[moduleName] != System.DBNull.Value)
                {
                    companyUserModule.ModuleName = row[moduleName].ToString();
                }
                if (row[read_writeID] != System.DBNull.Value)
                {
                    companyUserModule.ReadWriteID = int.Parse(row[read_writeID].ToString());
                }
                if (row[moduleId] != System.DBNull.Value)
                {
                    companyUserModule.ModuleID = int.Parse(row[moduleId].ToString());
                }
                if (row[isShow_export] != System.DBNull.Value)
                {
                    companyUserModule.IsShowExport = Convert.ToBoolean(row[isShow_export].ToString());
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
            return companyUserModule;
        }

        public static Modules GetModulesForCompanyUser(int userID)
        {
            Modules userModules = new Modules();

            object[] parameter = new object[1];
            parameter[0] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyModulesForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userModules.Add(FillCompanyUserModules(row, 0));

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
            _companyModulesPermittedToUser = userModules;
            return userModules;
        }
        #endregion

        private static Modules _companyModulesPermittedToUser = null;
        public static Modules CompanyModulesPermittedToUser
        {
            get { return _companyModulesPermittedToUser; }
            set { _companyModulesPermittedToUser = value; }
        }

        private static Hashtable _availableModulesDetails;
        public static Hashtable AvailableModulesDetails
        {
            get { return _availableModulesDetails; }
            set { _availableModulesDetails = value; }
        }

        private static Hashtable _availableTools;
        public static Hashtable AvailableTools
        {
            get { return _availableTools; }
            set { _availableTools = value; }
        }

        private static Hashtable _plugIns;
        public static Hashtable PlugIns
        {
            get { return _plugIns; }
            set { _plugIns = value; }
        }

        /// <summary>
        /// All oms module set
        /// </summary>
        static HashSet<string> _allOMSModuleSet = new HashSet<string>(){
            "Blotter",
            "Allocation",
            "Rebalancer",
            "Trading Ticket",
            "Quick Trading Ticket",
            "% Trading Tool",
            "Blotter/Execution Report",
            "Consolidated View",
            "User Profile",
            "General Ledger",
            "Daily Valuation",
            "Import Data",
            "Security Master",
            "Preferences",
            "Third Party Manager",
            "Audit Trail",
            "Broker Connections",
            "Compliance Engine",
            "Risk Analysis",
            "Pricing Inputs",
            "Watchlist",
            "Option Chain"
        };

        /// <summary>
        /// Gets all oms module set.
        /// </summary>
        /// <value>
        /// All oms module set.
        /// </value>
        public static HashSet<string> AllOMSModuleSet
        {
            get { return _allOMSModuleSet; }
        }

        /// <summary>
        /// All mandatory module set
        /// </summary>
        static HashSet<string> _allMandatoryModuleSet = new HashSet<string>()
        {
            "&Software Details",
            "Module Shortcuts",
            "Login",
            "Logout",
            "Layout",
            "Exit"
        };

        /// <summary>
        /// Gets all mandatory module set.
        /// </summary>
        /// <value>
        /// All mandatory module set.
        /// </value>
        public static HashSet<string> AllMandatoryModuleSet
        {
            get { return _allMandatoryModuleSet; }
        }

        //Checking module is allowed for user or not
        public static bool CheckModulePermissioning(string moduleNameInConfig, string moduleNameInDatabase)
        {
            bool moduleAvailable = false;
            try
            {
                moduleAvailable = CheckModulePermissioningForPrana(moduleNameInConfig, moduleNameInDatabase);
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
            return moduleAvailable;
        }

        /// <summary>
        /// Check Module Permissioning For Prana realease
        /// </summary>
        /// <param name="moduleNameInConfig"></param>
        /// <param name="moduleNameInDatabase"></param>
        /// <returns></returns>
        private static bool CheckModulePermissioningForPrana(string moduleNameInConfig, string moduleNameInDatabase)
        {
            bool moduleAvailable = false;
            try
            {
                if (_availableModulesDetails != null)
                {
                    if (_companyModulesPermittedToUser != null)
                    {
                        foreach (Prana.Admin.BLL.Module module in _companyModulesPermittedToUser)
                        {
                            if (module.ModuleName.Equals(moduleNameInDatabase))
                            {
                                moduleAvailable = true;
                                break;
                            }
                        }
                    }
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
            return moduleAvailable;
        }


        /// <summary>
        ///  //Checking tool is allowed for user or not
        /// </summary>
        /// <param name="toolNameInConfig"></param>
        /// <param name="toolNameInDatabase"></param>
        /// <returns></returns>
        public static bool CheckToolPermissioning(string toolNameInConfig, string toolNameInDatabase)
        {
            bool isToolAvailable = false;

            try
            {
                isToolAvailable = CheckToolPermissioningForPrana(toolNameInConfig, toolNameInDatabase);
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
            return isToolAvailable;
        }

        /// <summary>
        /// Check Tool Permissioning For Prana release
        /// Created by Omshiv, May 2014
        /// </summary>
        /// <param name="toolNameInConfig"></param>
        /// <param name="toolNameInDatabase"></param>
        /// <returns></returns>
        public static bool CheckToolPermissioningForPrana(string toolNameInConfig, string toolNameInDatabase)
        {
            bool toolAvailable = false;

            try
            {
                if (_availableTools != null)
                {
                    if (_companyModulesPermittedToUser != null)
                    {
                        foreach (Prana.Admin.BLL.Module module in _companyModulesPermittedToUser)
                        {
                            if (module.ModuleName.Equals(toolNameInDatabase))
                            {
                                toolAvailable = true;
                                break;
                            }
                        }
                    }
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
            return toolAvailable;
        }

        /// <summary>
        /// Check Tool Permissioning For CH release
        /// Created by Omshiv, May 2014
        /// </summary>
        /// <param name="toolNameInConfig"></param>
        /// <param name="toolNameInDatabase"></param>
        /// <returns></returns>
        public static bool CheckToolPermissioningForCH(string toolNameInConfig)
        {
            bool hasAccess = false;

            try
            {
                //remove spaces if any
                if (toolNameInConfig.Contains(" "))
                {
                    toolNameInConfig = toolNameInConfig.Replace(" ", "");
                }
                ModuleResources module = (ModuleResources)Enum.Parse(typeof(ModuleResources), toolNameInConfig.Trim());
                AuthAction action = AuthAction.Read;
                hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);


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
            return hasAccess;
        }
        private static IDictionary<string, ICollection<string>> _marketDataColumnsNotForUser;
        public static IDictionary<string, ICollection<string>> MarketDataColumnsNotForUser
        {
            get { return _marketDataColumnsNotForUser; }
            set { _marketDataColumnsNotForUser = value; }
        }

        public static IDictionary<string, ICollection<string>> GetMarketDataColumnsNotForUser(int companyUserID)
        {
            IDictionary<string, ICollection<string>> rtn = new Dictionary<string, ICollection<string>>();
            object[] parameter = new object[1];
            parameter[0] = companyUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetMarketDataColumnsNotForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string component = (string)row[0];
                        string column = (string)row[1];
                        ICollection<string> componentColumns = null;
                        if (rtn.ContainsKey(component))
                        {
                            componentColumns = rtn[component];
                        }
                        else
                        {
                            componentColumns = new HashSet<string>();
                            rtn[component] = componentColumns;
                        }

                        if (!componentColumns.Contains(column))
                        {
                            componentColumns.Add(column);
                        }
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
            _marketDataColumnsNotForUser = rtn;
            return rtn;
        }

        public static bool isMDHideColumn(string component, string column)
        {
            try
            {
                if (!_marketDataColumnsNotForUser.ContainsKey(component))
                {
                    return false;
                }
                ICollection<string> hideColumns = _marketDataColumnsNotForUser[component];
                if (hideColumns != null && hideColumns.Contains(column))
                {
                    return true;
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
            return false;
        }
    }
}
