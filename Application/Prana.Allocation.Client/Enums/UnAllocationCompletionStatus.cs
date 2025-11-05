namespace Prana.Allocation.Client.Enums
{
    /// <summary>
    /// While doing unallocation, either it is successfully completed or there could be issues. Issues could be due to...
    /// 1) Problem in unallocating the trades
    /// 2) Problem in writing the file. Generally before unallocation, we find out what trades are already closed/CA applied 
    ///     and we don't unallocate unless these are reverted. We just write that info in the file for further viewing.
    /// </summary>
    public enum UnAllocationCompletionStatus
    {
        /// <summary>
        /// All of the groupids unallocated successfully.
        /// </summary>
        Success,
        /// <summary>
        /// Some error occured while unallocating trades
        /// </summary>
        UnallocationError,
        /// <summary>
        /// Some/All of the groupids were not unallocated due to closed/ca applied etc
        /// </summary>
        FileWriteError
    }
}
