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
    internal class AccrualBasisMapping
    {
        internal static AccrualBasis GetAccrualBasisFromBloombergAccruedBasis(string accruedBasis)
        {
            try
            {
                if (accruedBasis.Contains("30/360"))
                {
                    return AccrualBasis.Accrual_30_360;
                }
                else if (accruedBasis.Contains("30E/360"))
                {
                    return AccrualBasis.Accrual_30E_360;
                }
                else if (accruedBasis.Contains("ACT/ACT"))
                {
                    return AccrualBasis.Actual_Actual;
                }
                else if (accruedBasis.Contains("ACT/360"))
                {
                    return AccrualBasis.Actual_360;
                }
                else if (accruedBasis.Contains("ACT/365"))
                {
                    return AccrualBasis.Actual_365;
                }
                else if (accruedBasis.Contains("30E/365"))
                {
                    return AccrualBasis.Accrual_30E_360;
                }
                else if (accruedBasis.Contains("30/365"))
                {
                    return AccrualBasis.Accrual_30_360;
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
            return AccrualBasis.Actual_Actual;
        }
    }
}
