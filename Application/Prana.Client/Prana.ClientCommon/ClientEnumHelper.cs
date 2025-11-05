using Prana.BusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.ClientCommon
{
    public class ClientEnumHelper
    {
        /// <summary>
        /// Converts the enum for binding.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static List<EnumerationValue> ConvertEnumForBindingWithSelectValueAndCaption(Type enumType)
        {
            List<EnumerationValue> results = ConvertEnumForBindingWithAssignedValuesWithCaption(enumType);

            results.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
            return results;
        }


        /// <summary>
        /// Converts the enum for binding with caption but with sorted value in the list.
        /// </summary>
        /// <param name="enumType">>Type of the enum.</param>
        /// <returns></returns>

        public static List<EnumerationValue> ConvertEnumForBindingWithSelectValueAndCaptionSortedByCaption(Type enumType)
        {
            List<EnumerationValue> results = ConvertEnumForBindingWithAssignedValuesWithCaption(enumType);
            var newList = results.OrderBy(x => x.DisplayText).ToList();
            newList.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
            return newList;
        }


        /// <summary>
        /// Converts the enum for binding with the used defined values of Enum.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static List<EnumerationValue> ConvertEnumForBindingWithAssignedValuesWithCaption(Type enumType)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();

            // Use reflection to see what values the enum provides
            Array enumValues = Enum.GetValues(enumType);

            foreach (Enum item in enumValues)
            {
                int i = Convert.ToInt32(Enum.Parse(enumType, item.ToString()));
                results.Add(new EnumerationValue(Prana.Utilities.MiscUtilities.EnumHelper.GetDescription(item), i));
            }
            return results;
        }

    }
}
