using Newtonsoft.Json;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LayoutService.Layout_Managers.Modules
{
    public class BlotterLayoutManager
    {
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static BlotterLayoutManager _blotterLayoutManager = null;
        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns></returns>
        public static BlotterLayoutManager GetInstance()
        {
            lock (_lock)
            {
                if (_blotterLayoutManager == null)
                    _blotterLayoutManager = new BlotterLayoutManager();
                return _blotterLayoutManager;
            }
        }
        #endregion


        public string UpdateBlotterLayout(string centralBlotterLayout ,string currentBlotterLayout)
        {
            try
            {
                // Check if centralBlotterLayout is empty
                if (string.IsNullOrEmpty(centralBlotterLayout))
                {
                    return currentBlotterLayout;
                }

                // Check if currentBlotterLayout is empty
                if (string.IsNullOrEmpty(currentBlotterLayout))
                {
                    return centralBlotterLayout;
                }
                var centralLayoutDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(centralBlotterLayout);
                var currentLayoutDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(currentBlotterLayout);

                foreach (var kvp in currentLayoutDict)
                {
                    // If the key exists in centralBlotterLayout, update its value
                    if (centralLayoutDict.ContainsKey(kvp.Key))
                    {
                        centralLayoutDict[kvp.Key] = kvp.Value;
                    }
                    // If the key does not exist, add it to centralBlotterLayout
                    else
                    {
                        centralLayoutDict.Add(kvp.Key, kvp.Value);
                    }
                }
                string updatedLayout = JsonConvert.SerializeObject(centralLayoutDict);

                // Return the serialized string
                return updatedLayout;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string RemoveBlotterCustomTab( string tabNameToRemove , string centralBlotterLayout)
        {
            string updatedLayout = centralBlotterLayout;
            try
            {                
                var centralLayoutDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(centralBlotterLayout);

                // Remove the specified tab name from the dictionary if it exists
                if (centralLayoutDict.ContainsKey(tabNameToRemove))
                {
                    centralLayoutDict.Remove(tabNameToRemove);
                }

                if (tabNameToRemove.StartsWith(LayoutServiceConstants.CONST_TAB_NAME_DYNAMIC_ORDER))
                {
                    tabNameToRemove = tabNameToRemove.Replace(LayoutServiceConstants.CONST_ORDER_SEPARATOR, LayoutServiceConstants.CONST_SUBORDER_SEPARATOR);

                    if (centralLayoutDict.ContainsKey(tabNameToRemove))
                    {
                        centralLayoutDict.Remove(tabNameToRemove);
                    }
                }
                    // Serialize the updated dictionary back to a string
                     updatedLayout = JsonConvert.SerializeObject(centralLayoutDict);

                
            }
            catch(Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            // Return the serialized layout
            return updatedLayout;
        }

        public string RenameBlotterCustomTab(string centralBlotterLayout , Dictionary<string,string> tabToRename)
        {
            string updatedLayout = centralBlotterLayout;
            try
            {
                // Deserialize the centralBlotterLayout string into a dictionary
                var centralLayoutDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(centralBlotterLayout);

                string oldTabName = tabToRename[LayoutServiceConstants.CONST_TAB_OLD_NAME];
                string newTabName = tabToRename[LayoutServiceConstants.CONST_TAB_NEW_NAME];
                // Check if the oldTabName exists in the dictionary
                if (centralLayoutDict.ContainsKey(oldTabName))
                {
                    // Get the value associated with oldTabName
                    string value = centralLayoutDict[oldTabName];

                    // Remove the oldTabName entry
                    centralLayoutDict.Remove(oldTabName);

                    // Add a new entry with newTabName and the same value
                    centralLayoutDict[newTabName] = value;
                }

                if(oldTabName.StartsWith(LayoutServiceConstants.CONST_TAB_NAME_DYNAMIC_ORDER))
                {
                    oldTabName = oldTabName.Replace(LayoutServiceConstants.CONST_ORDER_SEPARATOR, LayoutServiceConstants.CONST_SUBORDER_SEPARATOR);
                    newTabName = newTabName.Replace(LayoutServiceConstants.CONST_ORDER_SEPARATOR, LayoutServiceConstants.CONST_SUBORDER_SEPARATOR);
                    if (centralLayoutDict.ContainsKey(oldTabName))
                    {
                        string value = centralLayoutDict[oldTabName];
                        centralLayoutDict.Remove(oldTabName);
                        centralLayoutDict[newTabName] = value;
                    }
                }
                 updatedLayout = JsonConvert.SerializeObject(centralLayoutDict);
                return updatedLayout;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return updatedLayout;
        }

    }
}
