using System;

namespace Prana.BusinessObjects
{
    public class GenericNameID
    {
        public GenericNameID()
        {
        }

        private int _id = int.MinValue;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _shortName = string.Empty;
        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }
        public GenericNameID(object[] row)
        {

            _id = Convert.ToInt32(row[0].ToString());
            _name = Convert.ToString(row[1]);
        }
        public GenericNameID(int id, string name)
        {

            _id = id;
            _name = name;
        }
        public GenericNameID(string shortName, string name)
        {

            _shortName = shortName;
            _name = name;
        }
    }

}
