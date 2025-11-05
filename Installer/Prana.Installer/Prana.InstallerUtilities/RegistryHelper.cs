using Microsoft.Win32;
using Prana.InstallerUtilities.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Security;
using System.Text;

namespace Prana.InstallerUtilities
{
    public class RegistryHelper
    {
        private const string UpgradeCodeRegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UpgradeCodes";

        private static readonly int[] GuidRegistryFormatPattern = new[] { 8, 4, 4, 2, 2, 2, 2, 2, 2, 2, 2 };

        private static RegistryHelper singiltonInstance;

        List<InstallationDetails> installedClients = new List<InstallationDetails>();

        /// <summary>
        /// List of Browser EmulationVersion
        /// </summary>
        public enum BrowserEmulationVersion
        {
            Default = 0,
            Version7 = 7000,
            Version8 = 8000,
            Version8Standards = 8888,
            Version9 = 9000,
            Version9Standards = 9999,
            Version10 = 10000,
            Version10Standards = 10001,
            Version11 = 11000,
            Version11Edge = 11001
        }

        private const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

        private const string InternetExplorerRootKey = @"SOFTWARE\Microsoft\Internet Explorer";

        private RegistryHelper()
        {
            string query = string.Format("select Name,IdentifyingNumber from Win32_Product where Name like '{0}%'", InstallerConstants.ProductName);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                var collection = searcher.Get();
                foreach (ManagementObject product in collection)
                {
                    InstallationDetails details = new InstallationDetails();
                    details.Name = product.GetPropertyValue("Name").ToString();
                    details.Path = getPathForUpgrade(details.Name);
                    details.ProductCode = new Guid(product["IdentifyingNumber"].ToString());
                    details.UpdateCode = GetUpgradeCode(details.ProductCode);
                    installedClients.Add(details);
                }
            }
        }

        public static RegistryHelper Instance
        {
            get
            {
                if (singiltonInstance == null)
                {
                    singiltonInstance = new RegistryHelper();
                }
                return singiltonInstance;
            }
        }

        public List<InstallationDetails> GetInstalledClients()
        {
            return installedClients;
        }

        private Guid? GetUpgradeCode(Guid productCode)
        {
            // Convert the product code to the format found in the registry
            var productCodeSearchString = ConvertToRegistryFormat(productCode);

            // Open the upgrade code registry key
            //var upgradeCodeRegistryRoot = Registry.LocalMachine.OpenSubKey(UpgradeCodeRegistryKey);

            RegistryKey upgradeCodeRegistryRoot = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(UpgradeCodeRegistryKey);

            if (upgradeCodeRegistryRoot == null)
                return null;

            // Iterate over each sub-key
            foreach (var subKeyName in upgradeCodeRegistryRoot.GetSubKeyNames())
            {
                var subkey = upgradeCodeRegistryRoot.OpenSubKey(subKeyName);

                if (subkey == null)
                    continue;

                // Check for a value containing the product code
                if (subkey.GetValueNames().Any(s => s.IndexOf(productCodeSearchString, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    // Extract the name of the subkey from the qualified name
                    var formattedUpgradeCode = subkey.Name.Split('\\').LastOrDefault();

                    // Convert it back to a Guid
                    return ConvertFromRegistryFormat(formattedUpgradeCode);
                }
            }

            return null;
        }

        private string ConvertToRegistryFormat(Guid productCode)
        {
            return Reverse(productCode, GuidRegistryFormatPattern);
        }

        private Guid ConvertFromRegistryFormat(string upgradeCode)
        {
            if (upgradeCode == null || upgradeCode.Length != 32)
                throw new FormatException("Product code was in an invalid format");

            upgradeCode = Reverse(upgradeCode, GuidRegistryFormatPattern);

            return Guid.Parse(upgradeCode);
        }

        private string Reverse(object value, params int[] pattern)
        {
            // Strip the hyphens
            var inputString = value.ToString().Replace("-", "");

            var returnString = new StringBuilder();

            var index = 0;

            // Iterate over the reversal pattern
            foreach (var length in pattern)
            {
                // Reverse the sub-string and append it
                returnString.Append(inputString.Substring(index, length).Reverse().ToArray());

                // Increment our posistion in the string
                index += length;
            }

            return returnString.ToString();
        }

        private static string getPathForUpgrade(string productname)
        {
            const string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";

            try
            {
                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {

                        if (subkey_name == productname)
                            using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                            {
                                if (!string.IsNullOrEmpty((string)subkey.GetValue("Path")))
                                    return (string)subkey.GetValue("Path");
                                else
                                    return null;
                            }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// BrowserEmulation Set or not
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static bool IsBrowserEmulationSet(string program)
        {
            return GetBrowserEmulationVersion(program) != BrowserEmulationVersion.Default;
        }

        /// <summary>
        /// Get Browser EmulationVersion
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static BrowserEmulationVersion GetBrowserEmulationVersion(string program)
        {
            BrowserEmulationVersion result;

            result = BrowserEmulationVersion.Default;

            try
            {
                RegistryKey key;

                key = Registry.LocalMachine.OpenSubKey(BrowserEmulationKey, true);
                if (key != null)
                {
                    object value;
                    value = key.GetValue(program, null);

                    if (value != null)
                    {
                        result = (BrowserEmulationVersion)Convert.ToInt64(value);
                    }
                }
            }
            catch (SecurityException)
            {
                LoggingHelper.GetInstance().LoggerWrite("Emulation browser will not set Because user does not have the permissions required to read from the registry key");
                // The user does not have the permissions required to read from the registry key.
                throw;
            }
            return result;
        }

        /// <summary>
        /// Set Browser EmulationVersion
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static bool SetBrowserEmulationVersion(string program)
        {
            int ieVersion;
            BrowserEmulationVersion emulationCode;

            ieVersion = GetInternetExplorerMajorVersion();

            if (ieVersion >= 11)
            {
                emulationCode = BrowserEmulationVersion.Version11;
            }
            else
            {
                LoggingHelper.GetInstance().LoggerWrite("Internet Explorer is not Updated");
                return false;
            }
            return SetBrowserEmulationVersion(emulationCode, program);
        }

        /// <summary>
        /// Get Internet Explorer MajorVersion from system
        /// </summary>
        /// <returns></returns>
        public static int GetInternetExplorerMajorVersion()
        {
            int result;

            result = 0;

            try
            {
                RegistryKey key;

                key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);

                if (key != null)
                {
                    object value;

                    value = key.GetValue("svcVersion", null) ?? key.GetValue("Version", null);

                    if (value != null)
                    {
                        string version;
                        int separator;

                        version = value.ToString();
                        separator = version.IndexOf('.');
                        if (separator != -1)
                        {
                            int.TryParse(version.Substring(0, separator), out result);
                        }
                    }
                }
            }
            catch (SecurityException)
            {
                LoggingHelper.GetInstance().LoggerWrite("Emulation browser will not set Because user does not have the permissions required to read from the registry key");
                // The user does not have the permissions required to read from the registry key.
                throw;
            }
            return result;
        }

        /// <summary>
        /// Set Browser EmulationVersion
        /// </summary>
        /// <param name="browserEmulationVersion"></param>
        /// <param name="program"></param>
        /// <returns></returns>
        public static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion, string program)
        {
            bool result;

            result = false;

            try
            {
                RegistryKey key;

                key = Registry.LocalMachine.OpenSubKey(BrowserEmulationKey, true);

                if (key != null)
                {
                    if (browserEmulationVersion != BrowserEmulationVersion.Default)
                    {
                        // if it's a valid value, update or create the value
                        key.SetValue(program, (int)browserEmulationVersion, RegistryValueKind.DWord);
                    }
                    else
                    {
                        // otherwise, remove the existing value
                        key.DeleteValue(program, false);
                    }

                    result = true;
                }
            }
            catch (SecurityException)
            {
                LoggingHelper.GetInstance().LoggerWrite("Emulation browser will not set Because user does not have the permissions required to read from the registry key");
                // The user does not have the permissions required to read from the registry key.
                throw;
            }
            return result;
        }
    }
}