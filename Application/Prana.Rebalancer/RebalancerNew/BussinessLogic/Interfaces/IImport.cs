using Prana.Rebalancer.RebalancerNew.Models;
using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces
{
    public interface IImport
    {
        List<T> ValidateAndGetData<T>(ImportModel model) where T : class;

        void DisposeData();
    }
}
