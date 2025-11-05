using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'string field representing Date represented in UTC (Universal Time Coordinated, also known as "GMT") in YYYYMMDD format. This 
    /// special-purpose field is paired with UTCTimeOnly to form a proper UTCTimestamp for bandwidth-sensitive messages.
    /// Valid values: YYYY = 0000-9999, MM = 01-12, DD = 01-31.'
    /// </summary>
    /// <remarks>Currently, UTCDateOnly_t does NOT inherit from UTCDateTimeBase as there is no reason to
    /// apply any conversions to a date-only field.</remarks>
    public class UTCDateOnly_t : DateTimeTypeBase
    {
    }
}
