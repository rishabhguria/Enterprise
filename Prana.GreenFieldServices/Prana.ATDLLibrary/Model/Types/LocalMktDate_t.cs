using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// Represents a date of a local market (as oppose to UTC) in YYYYMMDD format. This is the "normal" date field used
    /// by the FIX Protocol.
    /// Valid values: YYYY = 0000-9999, MM = 01-12, DD = 01-31.
    /// </summary>
    public class LocalMktDate_t : DateTimeTypeBase
    {
    }
}
