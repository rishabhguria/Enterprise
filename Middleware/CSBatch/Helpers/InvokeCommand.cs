using System;

namespace CSBatch
{

    /// <summary>
    /// Invoke Command Instructions
    /// </summary>
    /// <remarks></remarks>
    public class InvokeCommand
    {
        /// <summary>
        /// From Date
        /// </summary>
        public const int FROMDATE = 1;
        /// <summary>
        /// To Date
        /// </summary>
        public const int TODATE = 2;
        /// <summary>
        /// Group
        /// </summary>
        public const int GROUP = 4;
        /// <summary>
        /// Entity
        /// </summary>
        public const int ENTITY = 8;
        /// <summary>
        /// Direction
        /// </summary>
        public const int DIRECTION = 16;
        /// <summary>
        /// Confidence
        /// </summary>
        public const int CONFIDENCE = 32;
        /// <summary>
        /// Top N
        /// </summary>
        public const int TOPN = 64;

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        /// <remarks></remarks>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        /// <remarks></remarks>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>The function.</value>
        /// <remarks></remarks>
        public string CSFunction { get; set; }

        /// <summary>
        /// Gets or sets the constant.
        /// </summary>
        /// <value>The constant.</value>
        /// <remarks></remarks>
        public int Constant { get; set; }

        /// <summary>
        /// Gets or sets the SQL function.
        /// </summary>
        /// <value>The SQL function.</value>
        /// <remarks></remarks>
        public string SQLFunction { get; set; }

        /// <summary>
        /// Gets or sets the param flag.
        /// </summary>
        /// <value>The param flag.</value>
        /// <remarks></remarks>
        public int ParamFlag { get; set; }
    }
}
