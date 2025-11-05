using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Utilities
{
    public sealed class CommissionEnumHelper
    {
        public static EnumerationValueList GetCommissionCriterias()
        {
            EnumerationValueList results = new EnumerationValueList();
            // TODO : this method assign text values to both DisplayMember and Value of the combo, need to do correct
            // Use reflection to see what values the enum provides
            Type enumType = typeof(CalculationBasis);
            string[] members = Enum.GetNames(enumType); //.GetMembers();
            foreach (string member in members)
            {
                string name = member;
                object value = Enum.Parse(enumType, name);
                if (name != CalculationBasis.SoftCommission.ToString() && name != CalculationBasis.Commission.ToString() && name != CalculationBasis.NotionalPlusCommission.ToString())
                {
                    results.Add(new EnumerationValue(name, value));
                }
            }

            results.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, int.MinValue));
            return results;
        }

        /// This function removes the "AvgPrice" from Enum CommissionCalculationBasis. AvgPrice is added just for giving range in criteria.
        /// Also replicated in D:\NirvanaOMS\SourceCode\Dev\Prana\ApplicationAdmin\PranaSuperAdmin\Prana.Admin.Controls\CommissionRules\ctrlCommissionRule.cs
        public static EnumerationValueList GetOldListForCalculationBasis(bool isClearingFee)
        {

            EnumerationValueList results = new EnumerationValueList();
            // TODO : this method assign text values to both DisplayMember and Value of the combo, need to do correct
            // Use reflection to see what values the enum provides
            Type enumType = typeof(CalculationBasis);
            string[] members = Enum.GetNames(enumType); //.GetMembers();
            foreach (string member in members)
            {
                string name = member;
                object value = Enum.Parse(enumType, name);
                if (name == CalculationBasis.Shares.ToString() || name == CalculationBasis.Contracts.ToString() || name == CalculationBasis.Notional.ToString() || name == CalculationBasis.FlatAmount.ToString())
                {
                    results.Add(new EnumerationValue(name, value));
                }
                else if (isClearingFee && (name == CalculationBasis.Commission.ToString() || name == CalculationBasis.SoftCommission.ToString()))
                {
                    results.Add(new EnumerationValue(name, value));
                }
            }

            results.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, int.MinValue));
            return results;
        }

        public static EnumerationValueList GetNewListForCalculationBasis(OtherFeeType feeType)
        {
            EnumerationValueList listForCalculationBasis = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelect(typeof(CalculationFeeBasis));
            listForCalculationBasis.RemoveAt(4);
            EnumerationValueList listForCalculationFeeBasis = new EnumerationValueList();
            foreach (EnumerationValue enumerationValue in listForCalculationBasis)
            {
                if (!enumerationValue.DisplayText.ToString().Equals(feeType.ToString()))
                {
                    listForCalculationFeeBasis.Add(enumerationValue);
                }
            }
            return listForCalculationFeeBasis;
        }

        public static EnumerationValueList GetOtherFeeList()
        {
            EnumerationValueList listForOtherFee = new EnumerationValueList();
            List<string> tempEnumFeeNames = new List<string>(Enum.GetNames(typeof(OtherFeeType)));
            foreach (KeyValuePair<string, string> kvp in OrderFields.FeeNamesCollection)
            {
                if (tempEnumFeeNames.Contains(kvp.Key))
                    listForOtherFee.Add(new EnumerationValue(kvp.Key, kvp.Value));
            }
            return listForOtherFee;
        }

        /// <summary>
        /// Gets the old list for calculation basis as dic.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetOldListForCalculationBasisAsDic()
        {

            Dictionary<int, string> myDic = new Dictionary<int, string>();
            // TODO : this method assign text values to both DisplayMember and Value of the combo, need to do correct
            // Use reflection to see what values the enum provides
            try
            {
                Type enumType = typeof(CalculationBasis);
                string[] members = Enum.GetNames(enumType);
                foreach (string member in members)
                {
                    string name = member;
                    object value = Enum.Parse(enumType, name);
                    if (name == CalculationBasis.Shares.ToString() || name == CalculationBasis.Contracts.ToString() || name == CalculationBasis.Notional.ToString() || name == CalculationBasis.FlatAmount.ToString())
                    {
                        myDic.Add(Convert.ToInt32(value), name);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return myDic;
        }

    }
}
