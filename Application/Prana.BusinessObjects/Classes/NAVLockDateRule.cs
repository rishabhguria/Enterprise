using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.Classes
{
    public class NAVLockDateRule 
    {
        public static DateTime? NAVLockDate { get; set; }

        /// <summary>
        /// This method is used to validate the given date against the NAV Lock date
        /// </summary>
        /// <param name="strDate"></param>
        public static bool ValidateNAVLockDate(string strDate)
        {
            try
            {
                if (DateTime.TryParse(strDate, out DateTime date))
                {
                    if (NAVLockDate.HasValue && date.Date <= NAVLockDate.Value)
                        return false;
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
            return true;
        }                         
    }
}
