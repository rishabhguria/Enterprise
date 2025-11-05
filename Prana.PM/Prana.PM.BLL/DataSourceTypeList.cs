using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Prana.PM.DAL;
using Csla;
using Csla.Validation;

namespace Prana.PM.BLL
{
    public class DataSourceTypeList : BusinessListBase<DataSourceTypeList, DataSourceType>
    {
        //public static SortableSearchableList<DataSourceType> Retrieve
        //{
        //    get { return RetrieveDataSourceTypeList(); }
        //}

        //BB
        public static DataSourceTypeList Retrieve
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
        private static DataSourceTypeList RetrieveDataSourceTypeList()
        {
            DataSourceTypeList dataSourceTypeList = DataSourceManager.GetDataSourceTypes();
            dataSourceTypeList.Insert(0, new DataSourceType(0, Prana.Global.ApplicationConstants.C_COMBO_SELECT));
            return dataSourceTypeList; 
        }

    }
}
