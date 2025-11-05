using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Prana.Utilities.XMLUtilities
{
    /// <summary>
    /// http://bytes.com/topic/net/answers/178328-date-formate-when-using-ds-writexml
    /// Whenever we serialize any datatable using WriteXml method, datetime is written into a string which contains timezone information.
    /// If we want to just write the date without time then this class comes into picture.
    /// </summary>
    public class DsDateFilterXmlTextWriter : XmlTextWriter
    {
        private List<string> watchElement;
        private bool onWatch;

        public DsDateFilterXmlTextWriter(TextWriter writer, List<string> watchElement)
            : base(writer)
        {
            this.watchElement = watchElement;
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            base.WriteStartElement(prefix, localName, ns);
            //if (0 == string.Compare(this.watchElement, localName))
            //{
            //    onWatch = true;
            //}

            if (watchElement.Contains(localName))
            {
                onWatch = true;
            }
        }

        public override void WriteString(string text)
        {
            if (onWatch)
            {
                try
                {

                    DateTime dt = DateTime.Parse(text);
                    text = dt.ToString("yyyy-MM-dd");
                }
                catch (FormatException)
                {
                    ;
                }
                onWatch = false;
            }
            base.WriteString(text);
        }
    }
}
