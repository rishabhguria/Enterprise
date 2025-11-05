namespace Prana.Allocation.Client.Enums
{

    /// <summary>
    /// Enum for the actions which are passed to SaveDataAsync so that the save can be handled on a different thread everytime
    /// </summary>
    public enum ActionAfterSavingData
    {
        DoNothing,
        GetData,
        ClearData,
        CancelEditChanges,
        CloseAllocation,
        ChangeTab
    }
}
