using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    public interface IAllocationPrefDataManager
    {
        List<AllocationDefault> GetAccountDefaults();
        void SaveDefaults(List<AllocationDefault> defaults);
        void DeleteDefaults(List<int> listDefaultID);
        AllocationDefault FillUserAccountDefaults(object[] row, int offSet);
    }
}
