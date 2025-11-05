using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BusinessObjects
{
    //http://www.c-sharpcorner.com/UploadFile/rmcochran/customgenericeventargs05132007100456AM/customgenericeventargs.aspx
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
}