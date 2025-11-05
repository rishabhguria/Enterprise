namespace Prana.ATDLLibrary.Xml.Serialization
{
    /// <summary>
    /// Enumeration that defines what type of containing is being deserialized.
    /// </summary>
    public enum StandardContainerMethod
    {
        /// <summary>
        /// The child item should be added to added to the container.
        /// </summary>
        Add = 0,

        /// <summary>
        /// The item should be assigned as the container.
        /// </summary>
        Assign
    }
}



