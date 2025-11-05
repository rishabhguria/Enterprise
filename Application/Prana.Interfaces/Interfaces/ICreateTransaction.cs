using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    public interface ICreateTransaction
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;

        void InitControl(CompanyUser user);
        ISecurityMasterServices SecurityMaster
        {
            set;
        }
        void SetUp();
        //ProxyBase<IAllocationServices> AllocationServices
        //{
        //    set;
        //}

    }
}
