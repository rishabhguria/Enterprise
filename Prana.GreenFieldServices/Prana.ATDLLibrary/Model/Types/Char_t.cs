using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// Represents a single character value, which can include any alphanumeric character or punctuation except the delimiter. All char 
    /// fields are case sensitive (i.e. m != M).
    /// </summary>
    /// <remarks>Arguably this type of value should be stored as a byte rather than char, as FIX is an ASCII protocol.  However,
    /// this would mean converting from char to byte when working with strings, which isn't really desirable.</remarks>
    public class Char_t : AtdlValueType<char>
    {
    }
}
