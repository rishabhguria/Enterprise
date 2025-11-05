using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prana.BusinessObjects;
using Prana.Global;
using Prism.Commands;
using Task = System.Threading.Tasks.Task;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class AddModelPortfolioViewModel : BindableBase
    {

        #region Commands
        public DelegateCommand ImportModelPortfolioCommand { get; set; }
        public DelegateCommand SavePortfolioCommand { get; set; }
        public DelegateCommand ViewCommand { get; set; }
        #endregion

        public AddModelPortfolioViewModel()
        {
            ImportModelPortfolioCommand = new DelegateCommand(() => ImportModelPortfolio());
            SavePortfolioCommand = new DelegateCommand(() => SavePortfolio());
            ViewCommand = new DelegateCommand(() => ViewAccountOrMasterFund());
        }

        Task SavePortfolio()
        {
            return Task.CompletedTask;
        }
        Task ViewAccountOrMasterFund()
        {
            return Task.CompletedTask;
        }
        Task ImportModelPortfolio()
        {
            return Task.CompletedTask;
        }
        public Array GetPortfolioTypeList()
        {
            return HelperClass.GetDescriptions(typeof(ApplicationConstants.PortfolioType));
        }
    }
}
