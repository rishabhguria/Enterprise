using Csla;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class SelectColumnsItemList : BusinessListBase<SelectColumnsItemList, SelectColumnsItem>
    {
        public SelectColumnsItemList()
        {
            MarkAsChild();
        }

        ///// <summary>
        ///// Gets the retrieve.
        ///// </summary>
        ///// <value>The retrieve.</value>
        //public static SelectColumnsItemList Retrieve(DataSourceNameID dataSourceNameID)
        //{
        //    return GetAllSetupColumnItems(dataSourceNameID);
        //}

        ///// <summary>
        ///// Gets all setup column items.
        ///// </summary>
        ///// <returns></returns>
        //private static SelectColumnsItemList GetAllSetupColumnItems(DataSourceNameID dataSourceNameID)
        //{
        //    //To Do: Add Method in DataSourceManager to fetch!
        //    SelectColumnsItemList selectColumnsItemList = new SelectColumnsItemList();//DataSourceManager.GetAllDataSourceNames();

        //    if (int.Equals(dataSourceNameID.ID, 1))
        //    {
        //        SelectColumnsItem item = new SelectColumnsItem();
        //        item.SourceColumnName = "Manager";
        //        item.Description = "PM Manager Number";
        //        item.Type = SelectColumnsType.Numeric;
        //        item.SampleValue = "112222";
        //        item.Notes = "Manager ##";

        //        selectColumnsItemList.Add(item);

        //        item = new SelectColumnsItem();
        //        item.SourceColumnName = "Manager Name";
        //        item.Description = "PM Manager Name";
        //        item.Type = SelectColumnsType.Text;
        //        item.SampleValue = "Jhon";
        //        item.Notes = "Manager Name";
        //        selectColumnsItemList.Add(item);

        //    }
        //    else
        //    {
        //        SelectColumnsItem item = new SelectColumnsItem();
        //        selectColumnsItemList.Add(item);
        //    }

        //    return selectColumnsItemList;
        //}
    }
}
