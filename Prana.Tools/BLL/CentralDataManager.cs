using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Tools
{
    class CentralDataManager
    {
        static Dictionary<string, UDAControl> _dictColl = new Dictionary<string, UDAControl>();

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="control"></param>
        public static void AddUserControl(string name, UDAControl control)
        {
            try
            {
                if (!_dictColl.ContainsKey(name))
                {
                    _dictColl.Add(name, control);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public static void Clear()
        {
            try
            {
                _dictColl.Clear();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        //public static UDACollection  GetCollection(string name)
        //{
        //    if (_dictColl.ContainsKey(name))
        //    {
        //        return _dictColl[name].Collection ;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// get UDA collection from UDA dict 
        /// </summary>
        /// <param name="udaCol"></param>
        /// <returns></returns>
        public static UDACollection GetCollection(Dictionary<int, string> udaCol)
        {
            UDACollection coll = new UDACollection();
            try
            {
                foreach (KeyValuePair<int, string> item in udaCol)
                {
                    if (item.Key != int.MinValue)
                    {
                        UDA uda = new UDA();
                        uda.ID = item.Key;
                        uda.Name = item.Value;
                        coll.Add(uda);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return coll;
        }
        public static List<UDAControl> UDACtrlCollection
        {
            get
            {
                List<UDAControl> coll = new List<UDAControl>();
                foreach (KeyValuePair<string, UDAControl> item in _dictColl)
                {
                    coll.Add(item.Value);
                }
                return coll;
            }
        }
        //public static Infragistics.Win.ValueList GetValueList(UDACollection coll)
        //{
        //    try
        //    {
        //        Infragistics.Win.ValueList list = new Infragistics.Win.ValueList();
        //        list.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
        //        foreach (UDA uda in coll)
        //        {
        //            list.ValueListItems.Add(uda.ID, uda.Name);
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return null;
        //}

        //internal static void FillInUseUDAIDsList()
        //{
        //    try
        //    {
        //        foreach (KeyValuePair<string, UDAControl>  udaCtrlItem in _dictColl)
        //        {
        //            udaCtrlItem.Value.FillInUseUDAsIDList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
    }

}
