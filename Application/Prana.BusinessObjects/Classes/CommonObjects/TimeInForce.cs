namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for TimeInForce.
    /// </summary>
    public class TimeInForce
    {
        int _timeInForceID = int.MinValue;
        string _name = string.Empty;
        string _tagValue = string.Empty;
        int _cvAuecID = int.MinValue;

        public TimeInForce()
        {
        }

        public TimeInForce(int timeInForceID, string name, string tagValue)
        {
            _timeInForceID = timeInForceID;
            _name = name;
            _tagValue = tagValue;
        }

        public int TimeInForceID
        {
            get
            {
                return _timeInForceID;
            }

            set
            {
                _timeInForceID = value;
            }
        }


        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string TagValue
        {
            get
            {
                return _tagValue;
            }

            set
            {
                _tagValue = value;
            }
        }

        public int CVAUECID
        {
            get
            {
                return _cvAuecID;
            }

            set
            {
                _cvAuecID = value;
            }
        }

    }
}
