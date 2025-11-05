namespace Prana.Allocation.Core.Enums
{
    /// <summary>
    /// Position type for group
    /// </summary>
    internal enum GroupPositionType
    {
        //Buy, Buy to open
        LongOpening,
        //Sell Short, Sell to open
        ShortOpening,
        //Sell, Sell to close
        LongClosing,
        //buy to close, Buy to cover.
        ShortClosing,
        None
    }
}
