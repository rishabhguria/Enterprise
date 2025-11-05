using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'int field representing a field's tag number when using FIX "Tag=Value" syntax. Value must be positive and may not 
    /// contain leading zeros.'
    /// </summary>
    public class TagNum_t : AtdlValueType<uint>
    {
    }
}
