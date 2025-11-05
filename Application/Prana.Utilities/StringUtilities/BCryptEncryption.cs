using Prana.LogManager;
using System;
using BCr = BCrypt.Net.BCrypt;

namespace Prana.Utilities.StringUtilities
{
    //All methods changed to private as this algorithm is currently not being used. Committed for Reference.
    public static class BCryptEncryption
    {
        /// <summary>
        /// Take any string and encrypt it using BCrypt then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enter to encrypt it</param>
        /// <returns>return the encrypted text as string</returns>
        private static string GetBCryptHashData(string data)
        {
            String returnValue = string.Empty;
            try
            {
                returnValue = BCr.HashPassword(data);
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
        /// Take a password and verify it against a BCrypt hash
        /// </summary>
        /// <param name="pass">input text you will enter to verify it</param>
        /// <param name="hash">hash value against which text is verified</param>
        /// <returns>return true or false if password is verified or not</returns>
        private static bool VerifyPassword(string pass, string hash)
        {
            bool returnValue = false;
            try
            {
                returnValue = BCr.Verify(pass, hash);
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
