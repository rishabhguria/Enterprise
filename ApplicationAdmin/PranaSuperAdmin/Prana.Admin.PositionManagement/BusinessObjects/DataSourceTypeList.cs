using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class DataSourceTypeList : SortableSearchableList<DataSourceType>
    {
        public static SortableSearchableList<DataSourceType> Retrieve
        {
            get { return RetrieveDataSourceTypeList(); }
        }

        /// <summary>
        /// Retrieves the colection implementing inheriting BindingList  Generic Type
        /// using the appropriate mechanism
        /// </summary>
        /// <returns></returns>
        /// <remarks>In a real application, this would call a data access component,
        /// access a typed dataset or TableAdapter, or call a Web service.</remarks>
        private static SortableSearchableList<DataSourceType> RetrieveDataSourceTypeList()
        {
            SortableSearchableList<DataSourceType> dataSourceTypeList = DataSourceManager.GetDataSourceTypes();
            dataSourceTypeList.Insert(0, new DataSourceType(0, "--Select--"));
            return dataSourceTypeList;
        }

    }
}
