

namespace Prana.ATDLLibrary.Model.Elements
{
    /// <summary>
    /// Represents a FIXatdl EditRef_t.
    /// </summary>
    public class EditRef_t<T>
    {
        public EditRef_t(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Refers to an ID of a previously defined edit element. The edit element may be defined at the strategy level or at the strategies level.
        /// </summary>
        public string Id { get; set; }
    }
}
