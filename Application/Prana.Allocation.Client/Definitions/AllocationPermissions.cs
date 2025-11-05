using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Linq;

namespace Prana.Allocation.Client.Definitions
{
    public static class AllocationPermissions
    {
        #region Members

        /// <summary>
        /// The _edit trade permission
        /// </summary>
        private static bool _editTradeModulePermission = false;

        /// <summary>
        /// The _allocation module permission
        /// </summary>
        private static bool _allocationModulePermission = false;

        /// <summary>
        /// The _ptt module Permission
        /// </summary>
        private static bool _pttCheckBoxPermission = false;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [allocation module permission].
        /// </summary>
        /// <value>
        /// <c>true</c> if [allocation module permission]; otherwise, <c>false</c>.
        /// </value>
        public static bool AllocationModulePermission
        {
            get { return _allocationModulePermission; }
            set { _allocationModulePermission = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [edit trade permission].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [edit trade permission]; otherwise, <c>false</c>.
        /// </value>
        public static bool EditTradeModulePermission
        {
            get { return _editTradeModulePermission; }
            set { _editTradeModulePermission = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [PTT module permission].
        /// </summary>
        /// <value>
        /// <c>true</c> if [ptt module permission]; otherwise, <c>false</c>.
        /// </value>
        public static bool PTTCheckBoxPermission
        {
            get { return _pttCheckBoxPermission; }
            set { _pttCheckBoxPermission = value; }
        }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Check Permission for Module
        /// </summary>
        public static bool CheckModulePermissionAllocation(string moduleName)
        {
            try
            {
                Modules modules = ModuleManager.CompanyModulesPermittedToUser;
                return modules.Cast<Module>().Any(module => module.ModuleName.Equals(moduleName));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }
        #endregion Methods
    }
}
