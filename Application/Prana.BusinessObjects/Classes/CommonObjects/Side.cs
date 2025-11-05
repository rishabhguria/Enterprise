namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Side.
    /// </summary>
    public class Side
    {
        int _sideID = int.MinValue;
        string _name = string.Empty;
        string _tagValue = string.Empty;
        int _auecID = int.MinValue;
        int _cvAuecID = int.MinValue;

        public Side()
        {
        }
        public Side(string name, string tagValue)
        {
            _name = name;
            _tagValue = tagValue;
        }

        public Side(int sideID, string name, string tagValue)
        {
            _sideID = sideID;
            _name = name;
            _tagValue = tagValue;
        }

        public int SideID
        {
            get
            {
                return _sideID;
            }

            set
            {
                _sideID = value;
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

        public int AUECID
        {
            get
            {
                return _auecID;
            }

            set
            {
                _auecID = value;
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
