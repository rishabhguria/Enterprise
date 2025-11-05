using Prana.LogManager;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Prana.Utilities.StringUtilities
{
    //All methods changed to private as this algorithm is currently not being used. Committed for Reference.
    //Reference URL : https://stackoverflow.com/questions/37499112/hash-algorithm-sha256-is-my-method-secure-how-do-i-add-a-salt-value-to-make-mo
    public static class SHA256Encryption
    {
        /// <summary>
        /// Take any string and encrypt it using SHA256 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enter to encrypt it</param>
        /// <returns>return the encrypted text as string</returns>
        private static string GetSHA256HashData(string data)
        {
            String returnValue = string.Empty;
            try
            {
                StringBuilder hash = new StringBuilder();
                SHA256Managed crypt = new SHA256Managed();
                var hashData = new byte[32];
                hashData = crypt.ComputeHash(Encoding.UTF8.GetBytes(data));
                RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
                var salt = new byte[32];
                rngCsp.GetBytes(salt);
                for (int i = 0; i < 32; i++)
                {
                    hash.Append(hashData[i].ToString("x2"));
                    hash.Append(salt[i].ToString("x2"));
                }
                returnValue = hash.ToString();
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
            return returnValue;
        }

        /// <summary>
        /// Take a password and verify it against a SHA256 hash
        /// </summary>
        /// <param name="pass">input text you will enter to verify it</param>
        /// <param name="hash">hash value against which text is verified</param>
        /// <returns>return true or false if password is verified or not</returns>
        private static bool VerifyPassword(string pass, string storedHashData)
        {
            bool returnValue = false;
            try
            {
                StringBuilder hash = new StringBuilder();
                SHA256Managed crypt = new SHA256Managed();
                var hashData = new byte[32];
                hashData = crypt.ComputeHash(Encoding.UTF8.GetBytes(pass));
                for (int i = 0; i < 32; i++)
                {
                    hash.Append(hashData[i].ToString("x2"));
                }
                storedHashData = new string(storedHashData.Where((c, i) => i % 4 < 2).ToArray());
                returnValue = string.Compare(hash.ToString(), storedHashData) == 0;
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
            return returnValue;
        }
    }
}
