using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    public interface ICashManagement
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosedHandler;
        void SetUp();
        event EventHandler LaunchAccountSetUpUI;
        Prana.BusinessObjects.CompanyUser loginUser
        {
            get;
            set;
        }
        ISecurityMasterServices SecurityMaster
        {
            set;
        }
        void AddCashTransaction(TaxLot taxlotToAddCash);
    }
}
