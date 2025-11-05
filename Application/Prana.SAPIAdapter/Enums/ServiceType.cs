using System;
using System.ComponentModel;

namespace Bloomberg.Library
{
    /// <summary>
    /// Enum ServiceType
    /// </summary>
    [Serializable]
    public enum ServiceType
    {
        /// <summary>
        /// The apiflds
        /// </summary>
        [Description("//blp/apiflds")]
        apiflds,
        /// <summary>
        /// The refdata
        /// </summary>
        [Description("//blp/refdata")]
        refdata,
        /// <summary>
        /// The mktdata
        /// </summary>
        [Description("//blp/mktdata")]
        mktdata
    }
}
