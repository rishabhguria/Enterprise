using Prana.BusinessObjects.Classes.PositionManagement;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Prana.BusinessObjects
{
    [Serializable]
    [KnownType(typeof(FilterDataForLastDateModified))]
    [KnownType(typeof(FilterDataByToDate))]
    [KnownType(typeof(FilterDataByExactDate))]
    [KnownType(typeof(FilterDataByDateRange))]
    [KnownType(typeof(FilterDataByFromDate))]
    [KnownType(typeof(FilterDataByAUECDateWise))]
    [KnownType(typeof(FilterDataByExactAccount))]
    [KnownType(typeof(FilterDataByExactSymbol))]
    [KnownType(typeof(FilterDataForSameUsers))]
    public abstract class FilterData
    {
        public abstract List<IFilterable> Filterdata(ref System.Object[] data, string Topic, int userId);
    }
}
