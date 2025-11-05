using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Enum FieldType
    /// </summary>
    [Serializable]
    public enum FieldType
    {
        /// <summary>
        /// Results include fields that are both streaming (real-time and delayed) and reference (static)
        /// </summary>
        [ElementValue("All")]
        All,
        /// <summary>
        /// Results include fields that provide streaming data (real-time and delayed)
        /// </summary>
        [ElementValue("Realtime")]
        RealTime,
        /// <summary>
        /// Results include fields that provide reference data (static).
        /// </summary>
        [ElementValue("Static")]
        Static
    }
}
