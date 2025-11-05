using System;
using Prana.BusinessObjects;
using Prana.Global;

namespace Prana.Interfaces
{
    public interface ICommissionCalculation
    {
        System.Windows.Forms.Form Reference();
        void SetUp(CompanyUser CurrentUser, bool isAllocationUIOpened);
        event EventHandler CommissionFormClosed;
        /// <summary>
        /// Toggle UI level controls and display message on status bar
        /// </summary>
        /// <param name="message">Message to be displayed on status bar</param>
        /// <param name="elementStatus">True to enable controls, False to disable</param>
        void ToggleUIElementWithMessage(String message, bool elementStatus);
        /// <summary>
        /// Allocation datachange handler should be fired when data is going to be changed and when completed
        /// </summary>
        //event AllocationDataChangeHandler allocationDataChange;
        event EventHandler<EventArgs<bool>> allocationDataChange;
        //IAllocationServices AllocationServices
        //{
        //    set;
        //}
        // IClosingServices ClosingServices
        //{
        //    set;
        //}

    }
}