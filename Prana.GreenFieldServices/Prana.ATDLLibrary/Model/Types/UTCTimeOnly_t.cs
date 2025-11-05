using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'string field representing Time-only represented in UTC (Universal Time Coordinated, also known as "GMT") in either HH:MM:SS 
    /// (whole seconds) or HH:MM:SS.sss (milliseconds) format, colons, and period required. This special-purpose field is paired with 
    /// UTCDateOnly to form a proper UTCTimestamp for bandwidth-sensitive messages.
    /// Valid values:
    /// HH = 00-23, MM = 00-60 (60 only if UTC leap second), SS = 00-59. (without milliseconds)
    /// HH = 00-23, MM = 00-59, SS = 00-60 (60 only if UTC leap second), sss=000-999 (indicating milliseconds).'
    /// </summary>
    public class UTCTimeOnly_t : DateTimeTypeBase
    {
    }
}
