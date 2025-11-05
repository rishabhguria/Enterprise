using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.UDATool
{
    class CentralDataManager
    {
        static Dictionary<string, UDAControl> _dictColl = new Dictionary<string, UDAControl>();
        public static void AddUserControl(string name,UDAControl control)
        {
            if (!_dictColl.ContainsKey(name))
            {
                _dictColl.Add(name,control );
            }
        }
        public static void Clear()
        {
            _dictColl.Clear();
        }
        public static UDACollection  GetCollection(string name)
        {
            if (_dictColl.ContainsKey(name))
            {
                return _dictColl[name].Collection ;
            }
            else
            {
                return null;
            }
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
        public static Infragistics.Win.ValueList GetValueList(UDACollection coll)
        {
            Infragistics.Win.ValueList list = new Infragistics.Win.ValueList();
            list.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (UDA uda in coll)
            {

                list.ValueListItems.Add(uda.ID, uda.Name);
            }
            return list;
        }

        internal static void FillInUseUDAIDsList()
        {
            try
            {
                foreach (KeyValuePair<string, UDAControl>  udaCtrlItem in _dictColl)
                {
                    udaCtrlItem.Value.FillInUseUDAsIDList();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
    enum UDATypes
    {
        AssetClass,
        SecurityType,
        Sector,
        SubSector,
        Country
    }
}
