using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.Classes
{
    public class EnumHelper
    {
        /// <summary>
        /// Converts the enum for binding.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static List<EnumerationValue> ConvertEnumForBinding(Type enumType) // System.Enum enumeration)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();

            // Use reflection to see what values the enum provides
            string[] members = Enum.GetNames(enumType); //.GetMembers();
            foreach (string member in members)
            {
                string name = member;
                object value = Enum.Parse(enumType, name);
                results.Add(new EnumerationValue(name, value));
            }

            return results;
        }

    }
}
