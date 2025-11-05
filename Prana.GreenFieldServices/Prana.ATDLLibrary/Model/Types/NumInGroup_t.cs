using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// NumInGroup_t is used when describing the number of entries in a FIX repeating group.
    /// </summary>
    /// <remarks>
    /// The FIX specification (5.0 SP2) describes this type as follows:
    /// <i>'int field representing the number of entries in a repeating group. Value must be positive.'</i>
    /// </remarks>
    public class NumInGroup_t : AtdlValueType<uint>
    {
    }
}
