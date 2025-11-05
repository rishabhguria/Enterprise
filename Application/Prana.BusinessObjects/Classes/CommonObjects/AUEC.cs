using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class AUEC
    {
        private Asset _asset = null;
        private UnderLying _underlying = null;
        private Exchange _exchange = null;
        private Currency _currency = null;
        public AUEC()
        {
            _asset = new Asset(int.MinValue, Global.ApplicationConstants.C_COMBO_SELECT);
            _underlying = new UnderLying();
            _exchange = new Exchange();
            _currency = new Currency();
        }
        public AUEC(int auecID, string name)
        {
            _auecID = auecID;
            _name = name;
        }
        int _auecID = int.MinValue;
        string _name = string.Empty;

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
        public Asset Asset
        {
            get
            {

                return _asset;
            }
        }

        public UnderLying UnderLying
        {
            get
            {

                return _underlying;
            }
        }

        public Exchange Exchange
        {
            get
            {
                return _exchange;
            }
        }

        public Currency Currency
        {
            get
            {
                return _currency;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
