using System;
//using Csla;

//using Csla;
//using Csla.Validation;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// List of the consolidationinfo class, displayed in the consolidation view
    /// </summary>
    //    public class ConsolidatedInfoList : BusinessListBase<ExposurePnlCacheItemList, ExposurePnlCacheItem>
    //    [Serializable()]
    //public class ExposurePnlCacheItemList : BusinessListBase<ExposurePnlCacheItemList, ExposurePnlCacheItem>
    //    {
    //    }
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class ExposurePnlCacheItemList : System.ComponentModel.BindingList<ExposurePnlCacheItem>
    {

        private bool _isUpdated = false;

        public bool IsUpdated
        {
            get { return _isUpdated; }
            set { _isUpdated = value; }
        }

    }

}
