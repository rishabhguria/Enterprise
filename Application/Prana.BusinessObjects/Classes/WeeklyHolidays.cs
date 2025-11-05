using System;

namespace Prana.BusinessObjects
{
    [Serializable()]
    public class WeeklyHoliday
    {
        #region Constructors

        public WeeklyHoliday()
        {
        }

        public WeeklyHoliday(int weeklyHolidayID, string weeklyHolidayName, int auecID)
        {
            _weeklyHolidayID = weeklyHolidayID;
            _weeklyHolidayName = weeklyHolidayName;
            _auecID = auecID;
        }

        #endregion

        #region Local variables

        int _weeklyHolidayID = int.MinValue;
        string _weeklyHolidayName = string.Empty;
        int _auecID = int.MinValue;

        #endregion



        #region Properties

        public int WeeklyHolidayID
        {
            get { return _weeklyHolidayID; }
            set { _weeklyHolidayID = value; }
        }

        public string WeeklyHolidayName
        {
            get { return _weeklyHolidayName; }
            set { _weeklyHolidayName = value; }
        }

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        #endregion
    }
}
