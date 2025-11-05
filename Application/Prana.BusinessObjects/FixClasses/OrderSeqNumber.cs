namespace Prana.BusinessObjects.FIX
{
    class OrderSeqNumber : IntField
    {

        public static int FIELD = 45;

        public OrderSeqNumber()
            : base(FIELD)
        {

        }

        public OrderSeqNumber(int data)
            : base(FIELD, data)
        {

        }
    }
}