// ***********************************************************************
// Assembly         : Prana.Utilities
// Author           : Disha Sharma
// Created          : 09-15-2015
// ***********************************************************************
// <copyright file="ValueListUtilities.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Infragistics.Win;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Utilities.UI.ExtensionUtilities
{
    /// <summary>
    /// Utilities class for ValueList
    /// </summary>
    public static class ValueListUtilities
    {
        /// <summary>
        /// checks if a particular datavalue exists in valuelist
        /// </summary>
        /// <param name="valueList">The valuelist</param>
        /// <param name="value">The DataValue</param>
        /// <returns>true if valuelist contains value, false otherwise</returns>
        public static bool CheckIfValueExistsInValuelist(ValueList valueList, string value)
        {
            try
            {
                foreach (ValueListItem val in valueList.ValueListItems)
                {
                    if (val.DataValue.ToString().Equals(value))
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
            return false;
        }

        public static bool CheckIfValueExistsInValuelist(IValueList valueList, string value)
        {
            try
            {
                for (int i = 0; i < valueList.ItemCount; i++)
                {
                    if (valueList.GetValue(i).ToString() == value)
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
            return false;
        }

        public static bool CheckIfValueExistsInValuelistFromMTT(IValueList valueList, string value)
        {
            try
            {
                int index = -1;
                object data = valueList.GetValue(value, ref index);
                if (data != null)
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
        /// <summary>
        /// returns list index on basis of datavalue
        /// </summary>
        /// <param name="valueList">The valuelist</param>
        /// <param name="value">The DataValue</param>
        /// <returns>list index of valuelist item</returns>
        public static int GetListIndexFromValue(ValueList valueList, string value)
        {
            try
            {
                foreach (ValueListItem val in valueList.ValueListItems)
                {
                    if (val.DataValue.ToString().Equals(value))
                    {
                        return val.ListIndex;
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
            return -1; // -1 indicates value has been deleted or not exists in valuelist
        }


        /// <summary>
        /// Get Value List From Dictionary
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static ValueList ToValueList(this Dictionary<int, string> dic)
        {
            if (dic != null)
            {
                ValueList list = new ValueList();
                foreach (KeyValuePair<int, string> item in dic)
                {
                    list.ValueListItems.Add(item.Key, item.Value);
                }
                return list;
            }
            else
                return null;

        }
    }
}
