using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{

    /// <summary>
    /// Response Error
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ResponseError : EventArgs
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
        public ResponseError()
        {

        }

        /// <summary>
        ///Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="error"></param>
        public ResponseError(object Objerror)
        {
            Element error = Objerror as Element;

            if (error.HasElement(new Name("source")))
            {
                Source = error.GetElementAsString(new Name("source"));
            }

            if (error.HasElement(new Name("code")))
            {
                Code = error.GetElementAsInt32(new Name("code"));
            }
            else if (error.HasElement(new Name("errorCode")))
            {
                Code = error.GetElementAsInt32(new Name("errorCode"));
            }

            if (error.HasElement(new Name("category")))
            {
                Category = error.GetElementAsString(new Name("category"));
            }

            if (error.HasElement(new Name("message")))
            {
                Message = error.GetElementAsString(new Name("message"));
            }
            else if (error.HasElement(new Name("description")))
            {
                Message = error.GetElementAsString(new Name("description"));
            }

            if (error.HasElement(new Name("Subcategory")))
            {
                Subcategory = error.GetElementAsString(new Name("Subcategory"));
            }



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
