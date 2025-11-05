using Prana.BusinessObjects;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class ImportViewModel : BindableBase
    {
        public ImportViewModel(RebalancerEnums.ImportType importType)
        {
            if (importType.Equals(RebalancerEnums.ImportType.CustomGroupsImport))
            {
                IsCustomGroupGridVisible = true;
                IsCashFlowGridVisible = false;
            }
            else
            {
                IsCustomGroupGridVisible = false;
                IsCashFlowGridVisible = true;
            }
        }


        public bool IsSaveClicked { get; set; }

        private bool _isCashFlowGridVisible;

        public bool IsCashFlowGridVisible
        {
            get { return _isCashFlowGridVisible; }
            set
            {
                _isCashFlowGridVisible = value;
                OnPropertyChanged();
            }
        }


        private bool _isCustomGroupGridVisible;

        public bool IsCustomGroupGridVisible
        {
            get { return _isCustomGroupGridVisible; }
            set
            {
                _isCustomGroupGridVisible = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<ImportCustomGroupModel> _customGroupGrid;

        public ObservableCollection<ImportCustomGroupModel> CustomGroupGrid
        {
            get { return _customGroupGrid; }
            set
            {
                _customGroupGrid = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ImportCashFlowModel> _cashFlowGrid;

        public ObservableCollection<ImportCashFlowModel> CashFlowGrid
        {
            get { return _cashFlowGrid; }
            set
            {
                _cashFlowGrid = value;
                OnPropertyChanged();
            }
        }


        internal void SetUp<T>(List<T> lst)
        {
            if (typeof(T) == typeof(ImportCustomGroupModel))
                CustomGroupGrid = new ObservableCollection<ImportCustomGroupModel>((List<ImportCustomGroupModel>)Convert.ChangeType(lst, typeof(List<ImportCustomGroupModel>)));
            else
                CashFlowGrid = new ObservableCollection<ImportCashFlowModel>((List<ImportCashFlowModel>)Convert.ChangeType(lst, typeof(List<ImportCashFlowModel>)));
        }

    }
}
