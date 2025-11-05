using Prana.BusinessObjects;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.ViewModels;
using System;
using System.ComponentModel;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class SecurityDataGridModel : BindableBase
    {
        private string _symbol;
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
            set 
            { 
                _symbol = value; 
                OnPropertyChanged(); 
            }
        }

        /// <summary>
        /// FactSet Symbol
        /// </summary>
        public string FactSetSymbol { get; set; }

        /// <summary>
        /// Activ Symbol
        /// </summary>
        public string ActivSymbol { get; set; }

        /// <summary>
        /// Bloomberg Symbol
        /// </summary>
        public string BloombergSymbol { get; set; }

        /// <summary>
        /// Bloomberg Symbol WithExchange Code
        /// </summary>
        public string BloombergSymbolWithExchangeCode { get; set; }

        public int AUECID { get; set; }

        [Browsable(false)]
        public string Asset { get; set; }

        [Browsable(false)]
        public decimal RoundLot { get; set; }

        public decimal FXRate { get; set; }

        [Browsable(false)]
        public decimal Multiplier { get; set; }

        [Browsable(false)]
        public string Sector { get; set; }

        [Browsable(false)]
        public decimal Delta { get; set; }

        [Browsable(false)]
        public decimal LeveragedFactor { get; set; }

        /// <summary>
        /// BuyOrSell
        /// </summary>
        private string increaseDecreaseOrSet;
        public string IncreaseDecreaseOrSet
        {
            get { return increaseDecreaseOrSet; }
            set
            {
                if (increaseDecreaseOrSet != null)
                {
                    decimal multiplierFactor = (value.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString()) ? -1 : 1);
                    bool res = AccountWiseSecurityDataGridModel.Instance.Validate<decimal>("IncreaseDecreaseOrSet", TargetPercentage, TargetPercentage * multiplierFactor, RebalancerCache.Instance.RebalancerHelperInstance.GetAccountWiseDict(), AccountOrGroupId, Symbol);
                    if (res)
                    {
                        increaseDecreaseOrSet = value;
                        if (increaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString()))
                            MultiplierFactor = -1;
                        else
                            MultiplierFactor = 1;
                    }

                    if (TargetPercentage < 0 && (IncreaseDecreaseOrSet == RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString() || IncreaseDecreaseOrSet == RebalancerEnums.RASIncreaseDecreaseOrSet.Increase.ToString()))
                    {
                        TargetPercentage = Math.Abs(TargetPercentage);
                    }
                }
                else
                {
                    increaseDecreaseOrSet = value;
                    if (increaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString()))
                        MultiplierFactor = -1;
                    else
                        MultiplierFactor = 1;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// BPS/% dropdown choice
        /// </summary>
        public string BPSOrPercentage { get; set; }

        private decimal target;

        public decimal Target
        {
            get { return target; }
            set
            {
                if (target > 0)
                {
                    var oldTarget = BPSOrPercentage.Equals(RebalancerEnums.BPSOrPercentage.BPS.ToString()) ? target / 100M : target;
                    var newTarget = BPSOrPercentage.Equals(RebalancerEnums.BPSOrPercentage.BPS.ToString()) ? value / 100M : value;
                    decimal multilplierFactor = (IncreaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString()) ? -1 : 1);
                    bool res = AccountWiseSecurityDataGridModel.Instance.Validate<decimal>("TargetPercentage", oldTarget, newTarget * multilplierFactor, RebalancerCache.Instance.RebalancerHelperInstance.GetAccountWiseDict(), AccountOrGroupId, Symbol);
                    if (res)
                    {
                        target = value;
                        TargetPercentage = BPSOrPercentage.Equals(RebalancerEnums.BPSOrPercentage.BPS.ToString()) ? target / 100M : target;
                    }
                }
                else
                {
                    target = value;
                    TargetPercentage = BPSOrPercentage.Equals(RebalancerEnums.BPSOrPercentage.BPS.ToString()) ? target / 100M : target;
                }
                OnPropertyChanged();

            }
        }

        public decimal TargetPercentage
        {
            get;
            set;
        }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set
            {
                if (price != 0)
                {
                    bool res = AccountWiseSecurityDataGridModel.Instance.Validate<decimal>("Price", price, value, RebalancerCache.Instance.RebalancerHelperInstance.GetAccountWiseDict());
                    if (res)
                    {
                        price = value;
                    }
                }
                else
                {
                    price = value;
                }
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// AccountOrGroup Id
        /// </summary>
        [Browsable(false)]
        public int AccountOrGroupId { get; set; }

        /// <summary>
        /// AccountOrGroup Name
        /// </summary>
        private string accountOrGroupName;
        public string AccountOrGroupName
        {
            get { return accountOrGroupName; }
            set
            {
                RASImportViewModel.OldAccountId = AccountOrGroupId;
                RASImportViewModel.OldAccountName = accountOrGroupName;
                accountOrGroupName = value;
            }
        }

        public string RemoveDummy { get; set; }

        private string remove = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Remove
        {
            get
            {
                return remove;
            }
            set
            {
                if (value != null)
                {
                    remove = value;
                    IsModelValid = value.Length > 0 ? false : true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// MultiplierFactor depends upon IncreaseDecreaseOrSet
        /// Increase=1
        /// Decrease=-1
        /// Set=1
        /// </summary>
        [Browsable(false)]
        public decimal MultiplierFactor { get; set; }

        private bool isModelValid;
        public bool IsModelValid
        {
            get
            {
                return isModelValid;
            }
            set
            {
                isModelValid = value;
                OnPropertyChanged();
            }
        }

        private string _sedolSymbol;
        public string SEDOLSymbol
        {
            get { return _sedolSymbol; }
            set
            {
                _sedolSymbol = value;
                OnPropertyChanged();
            }
        }
    }
}
