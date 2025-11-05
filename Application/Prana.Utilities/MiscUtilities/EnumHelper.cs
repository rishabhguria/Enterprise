using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Prana.Utilities.MiscUtilities
{
    public class EnumHelper
    {

        /// <summary>
        /// Converts the enum for binding with the used defined values of Enum.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static List<EnumerationValue> ConvertEnumForBindingWithAssignedValues(Type enumType) // System.Enum enumeration)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();

            // Use reflection to see what values the enum provides
            string[] members = Enum.GetNames(enumType);

            foreach (string member in members)
            {
                string name = member; // sud -- delete this no need for new "name reference"
                int i = Convert.ToInt32(Enum.Parse(enumType, name));
                results.Add(new EnumerationValue(name, i));
            }

            return results;
        }

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
                results.Add(new EnumerationValue(GetDescription(item), i));
            }
            return results;
        }

        /// <summary>
        /// Converts the enum for binding with the used defined values of Enum.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static List<EnumerationValue> ConvertEnumForBindingWithDescriptionValues(Type enumType) // System.Enum enumeration)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();

            // Use reflection to see what values the enum provides
            Array enumValues = Enum.GetValues(enumType);

            foreach (Enum item in enumValues)
            {
                string name = GetDescriptionWithDescriptionAttribute(item);
                results.Add(new EnumerationValue(name, (int)Enum.Parse(enumType, item.ToString())));
            }

            return results;
        }

        /// <summary>
        /// Converts the enum for binding.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static List<EnumerationValue> ConvertEnumForBindingWithSelectValue(Type enumType) // System.Enum enumeration)
        {
            List<EnumerationValue> results = ConvertEnumForBindingWithAssignedValues(enumType);

            results.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
            return results;
        }

        /// <summary>
        /// Converts the enum for binding using Description & Values of Enum.
        /// Naresh Kumar (14 April, 2015)
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static List<EnumerationValue> ConvertEnumForBindingWithActualAssignedValuesWithCaption(Type enumType)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();

            // Use reflection to see what values the enum provides
            Array enumValues = Enum.GetValues(enumType);

            foreach (Enum item in enumValues)
            {
                results.Add(new EnumerationValue(GetDescription(item), item.ToString()));
            }
            return results;
        }

        public static EnumerationValueList ConvertEnumForBindingWithSelect(Type enumType) // System.Enum enumeration)
        {
            EnumerationValueList results = ConvertEnumForBindingWitouthSelect(enumType);

            results.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, int.MinValue));
            return results;
        }
        public static EnumerationValueList ConvertEnumForBindingWitouthSelect(Type enumType) // System.Enum enumeration)
        {
            EnumerationValueList results = new EnumerationValueList();
            // TODO : this method assign text values to both DisplayMember and Value of the combo, need to do correct
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
        /// <summary>
        /// Gets the <see cref=�DescriptionAttribute� /> of an <see cref=�Enum� /> type value.
        /// </summary>
        /// <param name=�value�>
        public static string GetDescription(Enum value)
        {

            string description = value.ToString();
            if (value == null)
            {
            }
            try
            {
                FieldInfo fieldInfo = value.GetType().GetField(description);
                EnumDescriptionAttribute[] attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    description = attributes[0].Description;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return description;
        }

        public static string GetDescriptionWithDescriptionAttribute(Enum value)
        {
            if (value == null)
                return null;

            string description = value.ToString();

            try
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                    description = attributes[0].Description;
            }
            catch
            {
            }

            return description;
        }

        public static T GetValueFromEnumDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(EnumDescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((EnumDescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static IList ToList(Type type)
        {
            if (type == null)
            {

            }
            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);
            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            }
            return list;
        }

        /// <summary>
        /// Determines whether [is description defined] [the specified enum type].
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="descriptionText">The description text.</param>
        /// <returns></returns>
        public static bool IsDescriptionDefined(Type enumType, string descriptionText)
        {
            bool isDescriptionExist = false;
            try
            {
                Array enumValues = Enum.GetValues(enumType);
                foreach (Enum item in enumValues)
                {
                    if (GetDescription(item) == descriptionText)
                    {
                        return true;
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
            return isDescriptionExist;
        }
    }

    //moved this class from third party Ui project to enum helper - Bharat, April 2014
    [Serializable]
    public class EnumToDataSet<T> : DataTable
    {
        protected EnumToDataSet(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public EnumToDataSet(string ValueMember, string DisplayMember)
        {
            Columns.Add(ValueMember, typeof(int));
            Columns.Add(DisplayMember, typeof(string));
            foreach (int value in Enum.GetValues((typeof(T))))
            {
                DataRow row = NewRow();
                row[DisplayMember] = Enum.GetName(typeof(T), value);
                row[ValueMember] = value;
                Rows.Add(row);
            }

        }
        [SecurityPermissionAttribute(SecurityAction.Demand,
              SerializationFormatter = true)]
        public override void GetObjectData(
           SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
