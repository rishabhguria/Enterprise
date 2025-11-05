using Prana.LogManager;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace Prana.WindowsService.Common
{
    static class PranaWindowsServiceInstaller
    {
        public static void Install(string serviceName, string serviceDisplayName, string serviceDescription)
        {
            try
            {
                CreateInstaller(serviceName, serviceDisplayName, serviceDescription).Install(new Hashtable());
            }
            catch (System.Exception ex)
            {
                if (Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW)) throw;
            }
        }

        public static void Uninstall(string serviceName, string serviceDisplayName, string serviceDescription)
        {
            try
            {
                CreateInstaller(serviceName, serviceDisplayName, serviceDescription).Uninstall(null);

            }
            catch (System.Exception ex)
            {

                if (Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW)) throw;
            }
        }

        private static Installer CreateInstaller(string serviceName, string serviceDisplayName, string serviceDescription)
        {
            var installer = new TransactedInstaller();
            try
            {
                installer.Installers.Add(new ServiceInstaller
                {
                    ServiceName = serviceName,
                    DisplayName = serviceDisplayName,
                    Description = serviceDescription,
                    StartType = ServiceStartMode.Manual
                });
                installer.Installers.Add(new ServiceProcessInstaller
                {
                    Account = ServiceAccount.User,
                    Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name
                });
                var installContext = new InstallContext(
                    serviceName + ".install.log", null);
                installContext.Parameters["assemblypath"] =
                    Assembly.GetEntryAssembly().Location;
                installer.Context = installContext;
                return installer;
            }
            catch (System.Exception ex)
            {
                if (Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW)) throw;
                return null;
            }
        }
    }
}
