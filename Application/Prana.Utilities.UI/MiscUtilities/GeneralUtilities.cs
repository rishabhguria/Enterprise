using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Prana.Utilities.UI.MiscUtilities
{
    public static class GeneralUtilities
    {
        public static bool CheckNameValidation(string enteredName)
        {
            const int MAX_STRING_LENGTH = 50;
            if (enteredName.Length <= MAX_STRING_LENGTH)
                return true;
            else
            {
                System.Windows.Forms.MessageBox.Show("Name cannot be greater than " + MAX_STRING_LENGTH + " characters");
                return false;
            }
        }

        /// <summary>
        ///  Get Obj Prop Name and  Type Pair
        /// Created by Omshiv,Nov 2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, String> GetObjPropNameNTypePair(Type classType)
        {
            Dictionary<string, String> propNameTypePairs = new Dictionary<string, String>();
            try
            {
                PropertyInfo[] properties = classType.GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    string name = pi.Name;
                    String type = pi.PropertyType.Name;
                    if (!propNameTypePairs.ContainsKey(name))
                        propNameTypePairs.Add(name, type);
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
            return propNameTypePairs;
        }

        public static bool CheckOrderIDValidation(string enteredName)
        {
            const int MAX_STRING_LENGTH = 50;
            if (enteredName.Length <= MAX_STRING_LENGTH)
                return true;
            else
            {
                System.Windows.Forms.MessageBox.Show("OrderID cannot be greater than " + MAX_STRING_LENGTH + " numbers");
                return false;
            }
        }

        /// <summary>
        /// get Enumerable values from Class
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static List<EnumerationValue> GetValueListFromClass(Type classType)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();
            try
            {
                PropertyInfo[] properties = classType.GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    string propName = EnumHelper.GetFormatedText(pi.Name);
                    String disPlayName = propName;
                    if (propName.Contains(" ID"))
                    {
                        disPlayName = propName.Remove(propName.Length - 3);
                    }
                    results.Add(new EnumerationValue(disPlayName, pi.Name));
                }
                // Purpose : To sort results list
                results = results.OrderBy(res => res.DisplayText).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return results;
        }
    }
}
