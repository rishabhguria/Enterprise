using System;
using Prana.BusinessObjects;
using Prana.Global;

namespace Prana.Interfaces
{
	/// <summary>
	/// Its a interface for Allocation Preference Module.
	/// </summary>
	public interface IAllocation
	{
		System.Windows.Forms.Form Reference();		
		event EventHandler LaunchPreferences;
		//event EventHandler AccountStrategyBundling;       
        /// <summary>
        /// Event to send the data and arguments to Audit UI to process through nirvanaMain
        /// </summary>
        event EventHandler GetAuditClick;
        void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e);
		event EventHandler AllocationClosed;
        // added by sandep To show the Commission Calculation UI from the Allocation
        event EventHandler LaunchCommissionCalculation;
        event EventHandler GenrateCashTransaction;

        /// <summary>
        /// Allocation datachange handler should be fired when data is going to be changed and when completed
        /// </summary>
        //event AllocationDataChangeHandler allocationDataChange;
        event EventHandler<EventArgs<bool>> allocationDataChange;
        //event LoadCloseTradeUIFromAllocationHandler loadCloseTradeUIFromAllocation;
        event EventHandler<EventArgs<AllocationGroup>> loadCloseTradeUIFromAllocation;
        //event LoadSymbolLookUpUIFromAllocationHandler loadSymbolLookUpUIFromAllocation;
        event EventHandler<EventArgs<string>> loadSymbolLookUpUIFromAllocation;
        
        /// <summary>
        /// Toggle UI level controls and display message on status bar
        /// </summary>
        /// <param name="message">Message to be displayed on status bar</param>
        /// <param name="elementStatus">True to enable controls, False to disable</param>
        void ToggleUIElementsWithMessage(String message, Boolean elementStatus);
		Prana.BusinessObjects.CompanyUser loginUser
		{
			get;
			set;
		}
		void SetUp();
        void UserPermissionSetUp(int readOrReadwrite);
        //IAllocationServices AllocationServices
        //{
        //    set;
        //}
	}
}
