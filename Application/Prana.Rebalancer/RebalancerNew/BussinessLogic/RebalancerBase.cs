using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using System;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.BussinessLogic
{
    public abstract class RebalancerBase : BindableBase, IDisposable
    {

        public ISecurityMasterServices SecurityMaster { get; set; }
        public IRebalancerHelper RebalancerHelperInstance { get; set; }

        public RebalancerBase(ISecurityMasterServices securityMasterInstance, IRebalancerHelper rebalancerHelperInstance)
        {
            SecurityMaster = securityMasterInstance;
            RebalancerHelperInstance = rebalancerHelperInstance;
        }

        public abstract void Dispose();

        public void ShowErrorAlert(string msg)
        {
            MessageBox.Show(msg, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                       MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowInformationAlert(string msg)
        {
            MessageBox.Show(msg, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                       MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool ValidateName(string name)
        {
            if (name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                return false;
            }
            return true;
        }
    }
}
