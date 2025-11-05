using Infragistics.Win;
using System.Drawing;

namespace Prana.Utilities.UI.UIUtilities
{

    public sealed class ToolTipItem : IToolTipItem
    {
        readonly string m_toolTipText;

        public ToolTipItem(string toolTipText)
        {
            m_toolTipText = toolTipText;
        }

        ToolTipInfo IToolTipItem.GetToolTipInfo(Point mousePosition, Infragistics.Win.UIElement element, Infragistics.Win.UIElement previousToolTipElement, ToolTipInfo toolTipInfoDefault)
        {
            toolTipInfoDefault.AutoPopDelay = 2000;

            toolTipInfoDefault.ToolTipText = m_toolTipText;

            return toolTipInfoDefault;
        }
    }

}
