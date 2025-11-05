using System;

namespace Prana.Global
{
    //http://www.c-sharpcorner.com/UploadFile/rmcochran/customgenericeventargs05132007100456AM/customgenericeventargs.aspx
    [Serializable]
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            m_value = value;
        }

        private T m_value;
        public T Value
        {
            get { return m_value; }
        }
    }
    [Serializable]
    public class EventArgs<T, U> : EventArgs<T>
    {

        public EventArgs(T value, U value2)
            : base(value)
        {
            m_value2 = value2;
        }

        private U m_value2;
        public U Value2
        {
            get { return m_value2; }
        }
    }
    [Serializable]
    public class EventArgs<T, U, V> : EventArgs<T, U>
    {

        public EventArgs(T value, U value2, V value3)
            : base(value, value2)
        {
            m_value3 = value3;
        }

        private V m_value3;
        public V Value3
        {
            get { return m_value3; }
        }
    }
    [Serializable]
    public class EventArgs<T, U, V, W> : EventArgs<T, U, V>
    {

        public EventArgs(T value, U value2, V value3, W value4)
            : base(value, value2, value3)
        {
            m_value4 = value4;
        }

        private W m_value4;
        public W Value4
        {
            get { return m_value4; }
        }
    }
    [Serializable]
    public class EventArgs<T, U, V, W, X> : EventArgs<T, U, V, W>
    {

        public EventArgs(T value, U value2, V value3, W value4, X value5)
            : base(value, value2, value3, value4)
        {
            m_value5 = value5;
        }

        private X m_value5;
        public X Value5
        {
            get { return m_value5; }
        }
    }

}