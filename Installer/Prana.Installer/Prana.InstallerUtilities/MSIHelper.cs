using Microsoft.Deployment.WindowsInstaller;
using Prana.InstallerUtilities.Classes;
using System;

namespace Prana.InstallerUtilities
{
    /// <summary>
    /// This class helps to modify the MSI installer files
    /// The modifications are required because windows by default does not allow multiple installations of the same software
    /// Multiple installations can be achived by changing the required GUIDs
    /// THis class helps us achive those
    /// </summary>
    public static class MSIHelper
    {
        /// <summary>
        /// This function provides a new upgrade code and product id, this forces the MSI to install a new instance
        /// </summary>
        public static void RenewMSI(String msiPath, String releaseName)
        {
            using (var database = new Database(msiPath, DatabaseOpenMode.Direct))
            {
                string productVersion = GetProductVersion(database);
                var upgradeCode = Guid.NewGuid().ToString().ToUpper();

                database.SummaryInfo.RevisionNumber = "{" + Guid.NewGuid().ToString().ToUpper() + "}";

                database.Execute(@"UPDATE Property set Value = '" + InstallerConstants.ProductName + releaseName + "' where Property = 'ProductName' ");
                database.Execute(@"UPDATE Property set Value = '{" + Guid.NewGuid().ToString().ToUpper() + "}' where Property = 'ProductCode' ");
                database.Execute(@"UPDATE Property set Value = '{" + upgradeCode + "}' where Property = 'UpgradeCode' ");

                database.Execute(@"UPDATE Directory set DefaultDir = '" + releaseName + "' where Directory = 'INSTALL_SUBFOLDER'");
                database.Execute(@"UPDATE Directory set DefaultDir = '" + releaseName + "' where Directory = 'DesktopFolder_Shortcut'");

                database.Execute(@"DELETE from Upgrade where ActionProperty = 'WIX_DOWNGRADE_DETECTED' ");
                database.Execute(@"INSERT into Upgrade(UpgradeCode,VersionMin,VersionMax,Language,Attributes,Remove,ActionProperty) values('{" + upgradeCode + "}','" + productVersion + "','','',2,'','WIX_DOWNGRADE_DETECTED') ");

                database.Execute(@"DELETE from Upgrade where ActionProperty = 'WIX_UPGRADE_DETECTED' ");
                database.Execute(@"INSERT into Upgrade(UpgradeCode,VersionMin,VersionMax,Language,Attributes,Remove,ActionProperty) values('{" + upgradeCode + "}','','" + productVersion + "','',513,'','WIX_UPGRADE_DETECTED') ");

                database.Commit();
            }
        }

        /// <summary>
        /// Creates an MSI for updation by assiging it a desired name and upgrade code
        /// </summary>
        /// <param name="name"></param>
        /// <param name="upgradeCode"></param>
        public static void CreateUpgradeMSI(String msiPath, String releaseName, Guid upgradeCode)
        {
            using (var database = new Database(msiPath, DatabaseOpenMode.Direct))
            {
                string productVersion = GetProductVersion(database);
                database.SummaryInfo.RevisionNumber = "{" + Guid.NewGuid().ToString().ToUpper() + "}";

                database.Execute(@"UPDATE Property set Value = '" + releaseName + "' where Property = 'ProductName' ");
                database.Execute(@"UPDATE Property set Value = '{" + Guid.NewGuid().ToString().ToUpper() + "}' where Property = 'ProductCode' ");
                database.Execute(@"UPDATE Property set Value = '{" + upgradeCode + "}' where Property = 'UpgradeCode' ");

                database.Execute(@"UPDATE Directory set DefaultDir = '" + releaseName.Substring(InstallerConstants.ProductName.Length) + "' where Directory = 'INSTALL_SUBFOLDER'");
                database.Execute(@"UPDATE Directory set DefaultDir = '" + releaseName.Substring(InstallerConstants.ProductName.Length) + "' where Directory = 'DesktopFolder_Shortcut'");

                database.Execute(@"DELETE from Upgrade where ActionProperty = 'WIX_DOWNGRADE_DETECTED' ");
                database.Execute(@"INSERT into Upgrade(UpgradeCode,VersionMin,VersionMax,Language,Attributes,Remove,ActionProperty) values('{" + upgradeCode + "}','" + productVersion + "','','',2,'','WIX_DOWNGRADE_DETECTED') ");

                database.Execute(@"DELETE from Upgrade where ActionProperty = 'WIX_UPGRADE_DETECTED' ");
                database.Execute(@"INSERT into Upgrade(UpgradeCode,VersionMin,VersionMax,Language,Attributes,Remove,ActionProperty) values('{" + upgradeCode + "}','','" + productVersion + "','',513,'','WIX_UPGRADE_DETECTED') ");

                database.Commit();
            }
        }

        private static string GetProductVersion(Database database)
        {
            using (View view = database.OpenView("SELECT `Value` FROM `Property` where Property = 'ProductVersion' "))
            {
                view.Execute();

                // Get the record from the view
                Record record = view.Fetch();

                return record.GetString(1);
            }
        }
    }
}