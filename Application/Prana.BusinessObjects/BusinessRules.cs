using Csla.Validation;
using System;
using System.Reflection;

namespace Prana.BusinessObjects
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class BusinessRules : RuleArgs
    {

        public BusinessRules(string validation)
            : base(validation)
        {
        }


        /// <summary>
        /// Valid values will be greater than zero. So return true for valid values while false for invalid values.
        /// Right now specially for PositionMaster class, need to generalize.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool GreaterThanZeroCheck(object target, RuleArgs e)
        {
            PositionMaster finalTarget = target as PositionMaster;
            if (finalTarget == null)
            {

                return false;
            }

            Type typeToLoad = finalTarget.GetType();
            PropertyInfo property = typeToLoad.GetProperty(e.PropertyName);
            double propertyValue = (double)property.GetValue(finalTarget, null);
            if (propertyValue <= 0)
            {
                e.Description = "InValid " + e.PropertyName;
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static bool AUECCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget.AssetID <= CONST_ZERO)
        //        {
        //            e.Description = "InValid AUECID.";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool AssetCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget.AssetID <= CONST_ZERO)
        //        {
        //            e.Description = "Asset required";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool UnderLyingCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget.UnderlyingID <= CONST_ZERO)
        //        {
        //            e.Description = "Underlying required";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool ExchangeCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget.ExchangeID <= CONST_ZERO)
        //        {
        //            e.Description = "Exchange required";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool CurrencyCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget.CurrencyID <= CONST_ZERO)
        //        {
        //            e.Description = "Currency required";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool QuantityCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget.NetPosition <= CONST_ZERO)
        //        {
        //            e.Description = "Quantity required.";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        ///// <summary>
        ///// It is cost basis
        ///// </summary>
        ///// <param name="target"></param>
        ///// <param name="e"></param>
        ///// <returns></returns>
        //public static bool AveragePriceCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget._averagePrice <= CONST_ZERO)
        //        {
        //            e.Description = "Average price required.";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool UserIDCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget._averagePrice <= CONST_ZERO)
        //        {
        //            e.Description = "Average price required.";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool CompanyIDCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget._averagePrice <= CONST_ZERO)
        //        {
        //            e.Description = "Average price required.";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool TradingAccountIDCheck(object target, RuleArgs e)
        //{
        //    PositionMaster finalTarget = target as PositionMaster;
        //    if (finalTarget != null)
        //    {
        //        if (finalTarget._averagePrice <= CONST_ZERO)
        //        {
        //            e.Description = "Average price required.";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

    }

}

