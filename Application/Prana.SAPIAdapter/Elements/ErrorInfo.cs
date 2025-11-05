using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ErrorInfo
    {

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        /// <remarks></remarks>
        public string Source;
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        /// <remarks></remarks>
        public int Code;
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        /// <remarks></remarks>
        public string Category;
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        /// <remarks></remarks>
        public string Message;
        /// <summary>
        /// Gets or sets the subcategory.
        /// </summary>
        /// <value>The subcategory.</value>
        /// <remarks></remarks>
        public string Subcategory;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public ErrorInfo()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfo"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <remarks></remarks>
        public ErrorInfo(Element element)
        {
            Source = element.GetElementAsString(new Name("source"));
            Code = element.GetElementAsInt32(new Name("code"));
            Category = element.GetElementAsString(new Name("category"));
            Message = element.GetElementAsString(new Name("message"));
            Subcategory = element.GetElementAsString(new Name("subcategory"));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Message;
        }

    }
}
