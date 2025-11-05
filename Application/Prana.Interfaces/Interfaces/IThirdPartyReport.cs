
using Prana.Global;
using System;
namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Charts module.
    /// </summary>
    public interface IThirdPartyReport
    {
        System.Windows.Forms.Form Reference();
        event EventHandler ThirdPartyFlatFileClosed;
        event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked;

        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
    }
}
