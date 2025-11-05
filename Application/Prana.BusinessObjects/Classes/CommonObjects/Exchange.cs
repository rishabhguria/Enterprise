using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class Exchange
    {
        int _exchangeID = int.MinValue;
        string _name = string.Empty;

        /// <summary>
        /// constructor
        /// </summary>
        public Exchange(int exchangeID, string name)
        {
            _exchangeID = exchangeID;
            _name = name;
        }
        public Exchange()
        {

        }
        public int ExchangeID
        {
            get
            {
                return _exchangeID;
            }

            set
            {
                _exchangeID = value;
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


        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            //Given object can not be null
            if (obj == null)
            {
                return false;
            }

            // If Different types they can not be equal
            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            // Now if same type then match fields
            Exchange ex = (Exchange)obj;
            if (ex.ExchangeID == _exchangeID && ex.Name == _name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// overriding the GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + _exchangeID.GetHashCode();
            hash = (hash * 7) + _name.GetHashCode();
            return hash;
        }
    }
}
