using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Linq;

namespace Prana.ClientCommon
{
    public class AllocationSubModulePermission
    {
        private static bool _isLevelingPermitted = false;
        public static bool IsLevelingPermitted
        {
            get { return _isLevelingPermitted; }
        }

        private static bool _isProrataByNavPermitted = false;
        public static bool IsProrataByNavPermitted
        {
            get { return _isProrataByNavPermitted; }
        }

        public static void SetAllocationSubModulePermission()
        {
            try
            {
                Modules modules = ModuleManager.CompanyModulesPermittedToUser;

                _isLevelingPermitted = modules.Cast<Module>().Any(module => module.ModuleName.Equals(PranaModules.ALLOCATION_LEVELING_MODULE));
                _isProrataByNavPermitted = modules.Cast<Module>().Any(module => module.ModuleName.Equals(PranaModules.ALLOCATION_PRORATA_NAV_MODULE));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
