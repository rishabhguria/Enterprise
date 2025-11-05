using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Field Exception
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class FieldException
    {

        /// <summary>
        /// Gets or sets the field id.
        /// </summary>
        /// <value>The field id.</value>
        /// <remarks></remarks>
        public string FieldId;
        /// <summary>
        /// Gets or sets the error info.
        /// </summary>
        /// <value>The error info.</value>
        /// <remarks></remarks>
        public ErrorInfo ErrorInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public FieldException()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldException"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <remarks></remarks>
        public FieldException(Bloomberglp.Blpapi.Element element)
        {

            if (element.HasElement(new Name("fieldId")))
            {
                FieldId = element.GetElementAsString(new Name("fieldId"));
            }

            if (element.HasElement(new Name("errorInfo")))
            {
                Element item = element.GetElement(new Name("errorInfo"));

                ErrorInfo = new ErrorInfo(item);

            }
        }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return FieldId;
        }

    }
}
