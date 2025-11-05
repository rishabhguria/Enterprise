using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BlpDLWSAdapter.BusinessObject.Mappings
{
    internal class CouponFrequencyMapping
    {
        /// <summary>
        /// Maps coupon frequency enum with Bloomberg coupon frequency number
        /// </summary>
        /// <param name="couponFreq"></param>
        /// <returns></returns>
        internal static CouponFrequency GetCouponFrequency(string couponFreq)
        {
            int couponFrequency;
            CouponFrequency coup = CouponFrequency.None;
            try
            {
                if (Int32.TryParse(couponFreq.Trim(), out couponFrequency))
                {
                    switch (couponFrequency)
                    {
                        case 35:
                        case 12:
                        case 28: coup = CouponFrequency.Monthly;
                            break;
                        case 1: coup = CouponFrequency.Annually;
                            break;
                        case 4: coup = CouponFrequency.Quarterly;
                            break;
                        case 2: coup = CouponFrequency.SemiAnnually;
                            break;
                        case 52:
                        case 0:
                        case 360:
                        case 49:
                        case 6:
                        case 7:
                        case 3: coup = CouponFrequency.None;
                            break;
                        default: coup = CouponFrequency.None;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return coup;
        }
    }
}
