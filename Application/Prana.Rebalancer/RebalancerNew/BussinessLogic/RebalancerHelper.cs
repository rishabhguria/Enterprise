using Newtonsoft.Json;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Prana.Rebalancer.RebalancerNew.BussinessLogic
{
    public class RebalancerHelper : IRebalancerHelper
    {
        public bool IsComingFromImport { get; set; }

        public Dictionary<int, Dictionary<string, SecurityDataGridModel>> GetAccountWiseDict()
        {
            return IsComingFromImport ? RASImportViewModel.AccountWiseDict : RebalancerViewModel.AccountWiseDict;
        }

        public bool CheckColumnsExistsInFile(DataColumnCollection dataColumnCollection, Type dtoType)
        {
            bool allColumnExists = true;
            try
            {
                List<string> columnNamesFromDto = dtoType.GetProperties().
                    Where(j => j.GetCustomAttributes(false).OfType<JsonPropertyAttribute>().Any())
                    .Select(p => p.GetCustomAttribute<JsonPropertyAttribute>())
                    .Select(jp => jp.PropertyName).ToList();

                foreach (string columnName in columnNamesFromDto)
                {
                    if (!dataColumnCollection.Contains(columnName))
                        allColumnExists = false;
                };
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allColumnExists;
        }

        public void DataTableToObservableCollection<T>(DataTable dt, ref ObservableCollection<T> observableCollection)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(dt);
                observableCollection =
                    JsonConvert.DeserializeObject<ObservableCollection<T>>(jsonString);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
        }
    }
}
