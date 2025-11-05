namespace Prana.BusinessObjects.FIX
{
    class IntField : Field<int>
    {

        public IntField(int field) : base(field, 0)
        {

        }

        public IntField(int field, int data) : base(field, data)
        {

        }



        //public void setValue(int value)
        //{
        //    setObject(value);
        //}



        //public int getValue()
        //{
        //    return getObject();
        //}
    }
}
