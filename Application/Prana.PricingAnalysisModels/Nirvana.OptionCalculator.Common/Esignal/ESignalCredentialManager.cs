using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Prana.OptionCalculator.Common
{
    /// <summary>
    /// Credential Manager for Esignal Connectivity
    /// </summary>
    public class ESignalCredentialManager
    {
        /// <summary>
        /// The encryption key
        /// </summary>
        private static String encryptionKey = @"sblw-3hn8-sqoy19";
        /// <summary>
        /// The file path
        /// </summary>
        private static String _filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_LiveFeed, ConfigurationHelper.CONFIGKEY_LiveFeed_EsignalDetails);

        /// <summary>
        /// Saves the e signal file.
        /// </summary>
        /// <param name="eSignalDetails">The e signal details.</param>
        public static void SaveESignalFile(string eSignalDetails)
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    string[] str = _filePath.Split('\\');
                    if (str.Length > 0)
                    {
                        string path = string.Empty;
                        int i = 0;
                        if (str[0].Equals(".."))
                        {
                            DirectoryInfo parentDir = new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory;
                            for (; i < str.Length; i++)
                            {
                                if (!str[i].Equals(".."))
                                    break;
                                parentDir = parentDir.Parent;
                            }
                            path = parentDir.FullName;
                            for (; i < str.Length - 1; i++)
                            {
                                path += '\\' + str[i];
                            }
                        }
                        else
                        {
                            if (str.Length > 1)
                                path = _filePath.Substring(0, _filePath.LastIndexOf('\\'));
                        }
                        if (string.IsNullOrWhiteSpace(path))
                            Logger.LoggerWrite("Esignal Details storage path is not valid. Please contact administrator.", "Warning!");
                        else if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                    }
                }
                File.WriteAllText(_filePath, TripleDESEncryptDecrypt.TripleDESEncryption(eSignalDetails, encryptionKey));
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessException ex = new UnauthorizedAccessException("Access to ESignal Credentials storage is denied. Please contact the administrator.");
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw ex;
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
        /// Reads the e signal file.
        /// </summary>
        /// <returns></returns>
        public static List<string> ReadESignalFile()
        {
            List<string> lsCredential = new List<string>();
            try
            {
                if (File.Exists(_filePath))
                {
                    string text = File.ReadAllText(_filePath, Encoding.UTF8);
                    string decryptedProperties = TripleDESEncryptDecrypt.TripleDESDecryption(text, encryptionKey);
                    if (!string.IsNullOrWhiteSpace(decryptedProperties))
                        lsCredential.AddRange(decryptedProperties.Split(Seperators.SEPERATOR_6));
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
            return lsCredential;
        }
    }
}
