using Prana.ATDLLibrary.Model.Elements;
using System.Collections.ObjectModel;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection for storing instances of Control_t.  This class is used at the StrategyPanel level.
    /// </summary>
    public class ControlCollection : Collection<Control_t>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlCollection"/> class.
        /// </summary>
        public ControlCollection()
        {
        }
    }
}
