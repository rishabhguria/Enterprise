using Prana.ATDLLibrary.Model.Enumerations;

namespace Prana.ATDLLibrary.Model.Controls.Support
{
    public interface IOrientableControl
    {
        Orientation_t? Orientation { get; }
    }
}
