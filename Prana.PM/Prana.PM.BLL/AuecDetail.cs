using Csla;

namespace Prana.PM.BLL
{
    public class AuecDetail : BusinessBase<AuecDetail>
    {
        private AUEC _auec;

        public AUEC AUEC
        {
            get { return _auec; }
            set { _auec = value; }
        }

        private Asset _asset;

        public Asset Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        private Underlying _underlying;

        public Underlying Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }

        private Exchange _exchange;

        public Exchange Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }

        private Currency _currency;

        public Currency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }


        protected override object GetIdValue()
        {
            return _auec.AUECID;
        }
    }
}
