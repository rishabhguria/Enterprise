using Prana.ATDLLibrary.Model.Elements.Support;

namespace Prana.ATDLLibrary.Model.Elements
{
    /// <summary>
    /// Represents the FIXatdl StrategyLayout element that contains the root StrategyPanel.
    /// </summary>
    public class StrategyLayout_t : IStrategyPanel
    {
        /// <summary>
        /// Gets/sets the root StrategyPanel.
        /// </summary>
        public StrategyPanel_t StrategyPanel { get; set; }
    }
}
