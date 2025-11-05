using System.Collections.ObjectModel;
using Prana.ATDLLibrary.Model.Elements.Support;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection class for FIXatdl parameters, keyed on parameter name.
    /// </summary>
    public class ParameterCollection : KeyedCollection<string, IParameter>
    {
        /// <summary>
        /// Gets the key for the supplied item.
        /// </summary>
        /// <param name="parameter">Parameter to get key (name) for.</param>
        /// <returns>Key (name of parameter).</returns>
        protected override string GetKeyForItem(IParameter parameter)
        {
            return parameter.Name;
        }
    }
}
