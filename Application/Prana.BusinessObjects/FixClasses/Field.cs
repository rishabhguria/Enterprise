namespace Prana.BusinessObjects.FIX
{

    class Field<T>
    {

        private int tag;
        private T value;



        public Field(int field, T value)
        {
            this.tag = field;
            this.value = value;
        }

        /**
         * Gets the field's tag.
         * 
         * @return the tag
         */
        public int getTag()
        {
            return tag;
        }

        /**
         * Gets the field's tag. (QF/C++ compatibility)
         * 
         * @return the tag
         * @see quickfix.Field#getTag()
         */
        public int getField()
        {
            return getTag();
        }

        /**
         * Sets the field's value to the given object.
         * @param object
         */
        protected void setObject(T value)
        {
            this.value = value;

        }

        /**
         * Get the field value
         * @return an object representing the field's value
         */
        public T getObject()
        {
            return value;
        }
        //public boolean equals(Object value) {
        //    if (base.Equals(value) == true)
        //        return true;
        //    if (!(value is Field))
        //        return false;
        //    return tag == ((Field<?>) value).getField()
        //            && getObject().equals(((Field<?>) value).getObject());
        //     }

        //public int hashCode()
        //{
        //    return object.hashCode();
        //}









        public void setTag(int tag)
        {
            this.tag = tag;

        }
    }
}
