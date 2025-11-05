using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Reference : http://www.paulstovell.net/Posts/Post.aspx?postId=424d9f02-c1d8-4b2f-920c-389f602c27f8
    /// Using this class we can easily bind the enums to controls.
    /// </summary>
    [Serializable()]
    public class EnumerationValue
    {
        private string _displayText;
        private object _value;

        public EnumerationValue()
        {

        }

        public EnumerationValue(string displayText, object value)
        {
            _displayText = displayText;
            _value = value;
        }

        public string DisplayText
        {
            get { return _displayText; }
        }

        public object Value
        {
            get { return _value; }
        }
        public object Clone()
        {
            return this;
        }
    }
}
