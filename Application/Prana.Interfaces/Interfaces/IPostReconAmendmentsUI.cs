using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    public interface IPostReconAmendmentsUI
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;

        CompanyUser User
        {
            set;
        }
        ISecurityMasterServices SecurityMaster { set; }
        void SetUp(int accountID, string symbol, DateTime date, string comment, EventHandler UpdateCommentsFromPostReconAmendments);

    }
}
