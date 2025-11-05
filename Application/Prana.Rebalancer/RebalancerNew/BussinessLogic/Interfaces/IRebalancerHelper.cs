using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces
{
    public interface IRebalancerHelper
    {
        bool IsComingFromImport { get; set; }
        Dictionary<int, Dictionary<string, SecurityDataGridModel>> GetAccountWiseDict();
        bool CheckColumnsExistsInFile(DataColumnCollection dataColumnCollection, Type dtoType);
        void DataTableToObservableCollection<T>(DataTable dt, ref ObservableCollection<T> observableCollection);
    }
}
