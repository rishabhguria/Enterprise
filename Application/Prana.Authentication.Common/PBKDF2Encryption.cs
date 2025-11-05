using Prana.LogManager;
using System;
using System.Security.Cryptography;

namespace Prana.Authentication.Common
{
    //Reference URL: https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
    public static class PBKDF2Encryption
    {
        /// <summary>
        /// Take any string and encrypt it using PBKDF2 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enter to encrypt it</param>
        /// <returns>return the encrypted text as string</returns>
        public static string GetPBKDF2HashData(string data)
        {
            String returnValue = string.Empty;
            try
            {
                byte[] salt = new byte[16];
                new RNGCryptoServiceProvider().GetBytes(salt);
                var pbkdf2 = new Rfc2898DeriveBytes(data, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                returnValue = Convert.ToBase64String(hashBytes);
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
        /// Take a password and verify it against a PBKDF2 hash
        /// </summary>
        /// <param name="pass">input text you will enter to verify it</param>
        /// <param name="hash">hash value against which text is verified</param>
        /// <returns>return true or false if password is verified or not</returns>
        public static bool VerifyPassword(string pass, string storedHash)
        {
            bool returnValue = true;
            try
            {
                byte[] hashBytes = Convert.FromBase64String(storedHash);
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                for (int i = 0; i < 20; i++)
                    if (hashBytes[i + 16] != hash[i])
                        returnValue = false;
            }
            catch (Exception ex)
            {
                returnValue = false;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                throw new Exception(AuthenticationConstants.ENCRYPTION_ERROR);
            }
            return returnValue;
        }
    }
}
