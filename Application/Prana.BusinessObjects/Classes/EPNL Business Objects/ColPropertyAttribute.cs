using System;

namespace Prana.BusinessObjects
{
    [AttributeUsage(AttributeTargets.All)]
    public class ColPropertyAttribute : System.Attribute
    {
        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        private string _format;

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        private bool _onlyInColChooser;

        public bool OnlyInColChooser
        {
            get { return _onlyInColChooser; }
            set { _onlyInColChooser = value; }
        }


    }
}
