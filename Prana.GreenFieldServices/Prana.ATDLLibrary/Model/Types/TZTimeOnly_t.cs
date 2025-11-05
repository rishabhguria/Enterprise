using Prana.ATDLLibrary.Fix;
using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'string field representing the time represented based on ISO 8601. This is the time with a UTC offset to allow identification 
    /// of local time and timezone of that time.
    /// Format is HH:MM[:SS][Z | [ + | - hh[:mm]]] where HH = 00-23 hours, MM = 00-59 minutes, SS = 00-59 seconds, hh = 01-12 offset 
    /// hours, mm = 00-59 offset minutes.
    /// Example: 07:39Z is 07:39 UTC
    /// Example: 02:39-05 is five hours behind UTC, thus Eastern Time
    /// Example: 15:39+08 is eight hours ahead of UTC, Hong Kong/Singapore time
    /// Example: 13:09+05:30 is 5.5 hours ahead of UTC, India time'
    /// </summary>
    public class TZTimeOnly_t : DateTimeTypeBase
    {
    }
}
