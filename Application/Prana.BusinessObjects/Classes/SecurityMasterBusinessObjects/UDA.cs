using System;

//UDA attributes key value
namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class UDA
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


    }
}
