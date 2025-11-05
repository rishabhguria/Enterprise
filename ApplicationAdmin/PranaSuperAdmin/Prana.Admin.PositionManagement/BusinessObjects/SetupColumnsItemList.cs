using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class SelectColumnsItemList: SortableSearchableList<SelectColumnsItem>
    {
        /// <summary>
        /// Gets the retrieve.
        /// </summary>
        /// <value>The retrieve.</value>
        public static SortableSearchableList<SelectColumnsItem> Retrieve(DataSourceNameID dataSourceNameID)
        {
            return GetAllSetupColumnItems(dataSourceNameID);
        }

        /// <summary>
        /// Gets all setup column items.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<SelectColumnsItem> GetAllSetupColumnItems(DataSourceNameID dataSourceNameID)
        {
            //To Do: Add Method in DataSourceManager to fetch!
            SortableSearchableList<SelectColumnsItem> selectColumnsItemList = new SortableSearchableList<SelectColumnsItem>();//DataSourceManager.GetAllDataSourceNames();

            if (int.Equals(dataSourceNameID.ID, 1))
            {
                SelectColumnsItem item = new SelectColumnsItem();
                item.SourceColumnName = "Manager";
                item.Description = "PM Manager Number";
                item.Type = SelectColumnsType.Numeric;
                item.SampleValue = "112222";
                item.Notes = "Manager ##";

                selectColumnsItemList.Add(item);

                item = new SelectColumnsItem();
                item.SourceColumnName = "Manager Name";
                item.Description = "PM Manager Name";
                item.Type = SelectColumnsType.Text;
                item.SampleValue = "Jhon";
                item.Notes = "Manager Name";
                selectColumnsItemList.Add(item);

            }

            return selectColumnsItemList;
        }
    }
}
