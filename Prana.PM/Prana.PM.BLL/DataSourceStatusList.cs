using System;
using System.Collections.Generic;
using System.Text;
//
using Prana.BusinessObjects;
using Prana.PM.DAL;

namespace Prana.PM.BLL
{
    public class DataSourceStatusList : SortableSearchableList<DataSourceStatus>
    {
        public static SortableSearchableList<DataSourceStatus> Retrieve
        {
            get { return RetrieveStatusList(); }
        }

        /// <summary>
        /// Retrieves the country list.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<DataSourceStatus> RetrieveStatusList()
        {
            SortableSearchableList<DataSourceStatus> statusList = DataSourceManager.GetStatusList();
            statusList.Insert(0, new DataSourceStatus(0,Prana.Global.ApplicationConstants.C_COMBO_SELECT));
            return statusList;
        }
    }
}
