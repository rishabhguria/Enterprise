using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'string field representing Time/date combination represented in UTC (Universal Time Coordinated, also known as "GMT") 
    /// in either YYYYMMDD-HH:MM:SS (whole seconds) or YYYYMMDD-HH:MM:SS.sss (milliseconds) format, colons, dash, and period required.
    /// Valid values:
    /// * YYYY = 0000-9999, MM = 01-12, DD = 01-31, HH = 00-23, MM = 00-59, SS = 00-60 (60 only if UTC leap second) (without milliseconds).
    /// * YYYY = 0000-9999, MM = 01-12, DD = 01-31, HH = 00-23, MM = 00-59, SS = 00-60 (60 only if UTC leap second), sss=000-999 (indicating 
    /// milliseconds).
    /// Leap Seconds: Note that UTC includes corrections for leap seconds, which are inserted to account for slowing of the rotation of the
    /// earth. Leap second insertion is declared by the International Earth Rotation Service (IERS) and has, since 1972, only occurred on the
    /// night of Dec. 31 or Jun 30. The IERS considers March 31 and September 30 as secondary dates for leap second insertion, but has never
    /// utilized these dates. During a leap second insertion, a UTCTimestamp field may read "19981231-23:59:59", "19981231-23:59:60", 
    /// "19990101-00:00:00". (see http://tycho.usno.navy.mil/leapsec.html)'
    /// </summary>
    public class UTCTimestamp_t : DateTimeTypeBase
    {
        /// <summary>Gets or sets the local market timezone.<br/>
        /// Describes the time zone without indicating whether daylight savings is in effect. Valid values are taken from 
        /// names in the Olson time zone database. All are of the form Area/Location, where Area is the name of a continent 
        /// or ocean, and Location is the name of a specific location within that region. E.g. Americas/Chicago.
        /// Applicable when xsi:type is UTCTimestamp_t.</summary>
        /// <value>The local market timezone.</value>
        public string LocalMktTz { get; set; }
    }
}
