using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
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
            statusList.Insert(0, new DataSourceStatus(0, "--Select--"));
            return statusList;
        }
    }
}
