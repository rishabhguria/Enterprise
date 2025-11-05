using Infragistics.Win;

namespace Prana.Utilities.UI.UIUtilities
{
    public class NoFocusIndicatorDrawFilter : IUIElementDrawFilter
    {
        /// <summary>
        /// This method is called during the drawing process for UI elements.
        /// It determines whether custom drawing should occur before the focus indicator is drawn.
        /// </summary>     
        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            if (drawPhase == DrawPhase.BeforeDrawFocus)
            {
                // Return true to indicate that custom drawing should happen.
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method specifies which drawing phases to filter.
        /// In this case, we want to filter the BeforeDrawFocus phase.
        /// </summary>
        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            return DrawPhase.BeforeDrawFocus;
        }
    }

}
