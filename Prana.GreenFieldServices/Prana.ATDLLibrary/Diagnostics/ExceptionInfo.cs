using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Prana.ATDLLibrary.Diagnostics
{
    /// <summary>
    /// This class is used to hold additional information about an exception, such as line number and line position for 
    /// errors in XML files.
    /// </summary>
    public class ExceptionInfo
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfo"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public ExceptionInfo(XObject obj)
        {
            if (obj is IXmlLineInfo && (obj as IXmlLineInfo).HasLineInfo())
            {
                _data["LineNumber"] = (obj as IXmlLineInfo).LineNumber;
                _data["LinePosition"] = (obj as IXmlLineInfo).LinePosition;
            }
        }

        /// <summary>
        /// Populates the exception data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void PopulateExceptionData(IDictionary data)
        {
            foreach (KeyValuePair<string, object> item in _data)
                data[item.Key] = item.Value;
        }
    }
}