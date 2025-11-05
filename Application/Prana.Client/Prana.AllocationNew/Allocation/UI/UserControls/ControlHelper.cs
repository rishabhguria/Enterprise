using Infragistics.Win;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    internal static class ControlHelper
    {
        /// <summary>
        /// Returns CSV for Value list checked items.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        internal static string GetCsv(ValueList list)
        {
            try
            {
                CheckedValueListItemsCollection col = list.CheckedItems;
                string csv = string.Empty;

                csv = String.Join(",", col.Select(x => x.DisplayText));

                return csv;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }

        }

        /// <summary>
        /// Returns List for the csv of symbols.
        /// </summary>
        /// <param name="prList"></param>
        /// <returns></returns>
        internal static List<string> GetStringList(string prList)
        {
            try
            {
                if (string.IsNullOrEmpty(prList))
                    return null;
                // String[] array = prList.Split(',');
                List<string> idList = prList.Split(new char[] { ',' }).ToList();
                //foreach (String name in array)
                //{
                //    idList.Add(name);
                //}
                return idList;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns list for exchange and asset csv.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="type">can be Exchnge or Asset</param>
        /// <returns></returns>
        internal static List<int> GetListId(string csv, String type)
        {
            try
            {
                if (string.IsNullOrEmpty(csv))
                    return null;
                String[] array = csv.Split(',');
                List<int> idList = new List<int>();
                foreach (String name in array)
                {
                    if (type.Equals("Exchange"))
                    {
                        int id = CommonDataCache.CachedDataManager.GetInstance.GetExchangeID(name);
                        idList.Add(id == int.MinValue ? -1 : id);
                    }
                    else if (type.Equals("Asset"))
                    {
                        int id = CommonDataCache.CachedDataManager.GetInstance.GetAssetID(name);
                        idList.Add(id == int.MinValue ? -1 : id);
                    }
                    else if (type.Equals("Account"))
                    {
                        int id = CommonDataCache.CachedDataManager.GetInstance.GetAccountID(name);
                        idList.Add(id == int.MinValue ? -1 : id);
                    }
                }
                return idList;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Return CSV for List
        /// </summary>
        /// <param name="type"></param>
        /// <param name="list"></param>
        /// <param name="stringList"></param>
        /// <returns></returns>
        internal static string GetCsvForList(string type, List<int> list = null, List<string> stringList = null)
        {
            try
            {
                // StringBuilder builder = new StringBuilder();
                string csv = string.Empty;
                if (list != null)
                {
                    switch (type)
                    {
                        case "Exchange":
                            csv = String.Join(",", list.Select(x => CommonDataCache.CachedDataManager.GetInstance.GetExchangeText(x)).ToArray());
                            break;
                        case "Asset":
                            csv = String.Join(",", list.Select(x => CommonDataCache.CachedDataManager.GetInstance.GetAssetText(x)).ToArray());
                            break;
                        case "Account":
                            csv = String.Join(",", list.Select(x => CommonDataCache.CachedDataManager.GetInstance.GetAccountText(x)).ToArray());
                            break;
                        #region unused
                        //if (type.Equals("Exchange"))
                        //{
                        //    csv = String.Join(",", list.Select(x => CommonDataCache.CachedDataManager.GetInstance.GetExchangeText(x)).ToArray());
                        //    // builder.Append(CommonDataCache.CachedDataManager.GetInstance.GetExchangeText(id));
                        //    //builder.Append(',');
                        //}
                        //else if (type.Equals("Asset"))
                        //{
                        //    csv = String.Join(",", list.Select(x => CommonDataCache.CachedDataManager.GetInstance.GetAssetText(x)).ToArray());
                        //    // builder.Append(CommonDataCache.CachedDataManager.GetInstance.GetAssetText(id));
                        //    // builder.Append(',');
                        //}
                        //else if (type.Equals("Account"))
                        //{
                        //    csv = String.Join(",", list.Select(x => CommonDataCache.CachedDataManager.GetInstance.GetAccountText(x)).ToArray());
                        //}
                        #endregion
                    }
                }
                if (stringList != null)
                {
                    csv = String.Join(",", stringList.Select(x => x.ToString()).ToArray());
                    // builder.Append(id);
                    //builder.Append(',');               
                }
                return csv;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns list of items to check from CSV.
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        internal static List<ValueListItem> GetValueListFromCsv(string csv)
        {
            try
            {
                if (string.IsNullOrEmpty(csv))
                    return null;
                String[] array = csv.Split(',');
                List<ValueListItem> idList = new List<ValueListItem>();
                foreach (String name in array)
                {
                    ValueListItem item = new ValueListItem();
                    item.DisplayText = name;
                    idList.Add(item);
                }
                return idList;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
