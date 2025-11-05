using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Elements.Support;
using Prana.ATDLLibrary.Model.Enumerations;

namespace Prana.ATDLLibrary.Model.Elements
{
    public class StrategyPanel_t : IStrategyPanel
    {
        private readonly StrategyPanelCollection _strategyPanels;
        private ControlCollection _controls;

        public Border_t? Border { get; set; }
        public bool? Collapsed { get; set; }
        public bool? Collapsible { get; set; }
        public string Color { get; set; }
        public Orientation_t? Orientation { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StrategyPanel_t()
        {
            // Set defaults
            Collapsed = true;
            Collapsible = false;

            _strategyPanels = new StrategyPanelCollection();
        }

        public StrategyPanelCollection StrategyPanels { get { return _strategyPanels; } }

        public ControlCollection Controls
        {
            get
            {
                // Lazy initialisation as we can't use 'this' pointer in constructor.
                if ( _controls == null)
                {
                    _controls = new ControlCollection();
                }

                return _controls;
            }
        }
    }
}
