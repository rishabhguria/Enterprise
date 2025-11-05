using Infragistics.Win;
using Prana.TradeManager.Extension;
using System;
using System.Drawing;

namespace Prana.TradingTicket
{
    /// <summary>
    /// BrokerComboDrawFilter
    /// </summary>
    /// <seealso cref="Infragistics.Win.IUIElementDrawFilter" />
    public sealed class BrokerComboDrawFilter : IUIElementDrawFilter
    {
        /// <summary>
        /// Called during the drawing operation of a UIElement for a specific phase
        /// of the operation. This will only be called for the phases returned
        /// from the GetPhasesToFilter method.
        /// </summary>
        /// <param name="drawPhase">Contains a single bit which identifies the current draw phase.</param>
        /// <param name="drawParams">The <see cref="T:Infragistics.Win.UIElementDrawParams" /> used to provide rendering information.</param>
        /// <returns>
        /// Returning true from this method indicates that this phase has been handled and the default processing should be skipped.
        /// </returns>
        bool IUIElementDrawFilter.DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            ValueListItemUIElement itemElement = drawParams.Element as ValueListItemUIElement;
            if (itemElement != null)
            {
                if (drawPhase == DrawPhase.BeforeDrawBackColor)
                {
                    if (((SolidBrush)drawParams.BackBrush).Color.Name.Equals("ff0078d7"))//If Blue background color
                        drawParams.AppearanceData.BackColor = Color.FromArgb(64, SystemColors.GrayText);
                }
                else
                {
                    if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(Convert.ToInt32(itemElement.ValueListItem.DataValue)) == BusinessObjects.PranaInternalConstants.ConnectionStatus.CONNECTED)
                        drawParams.AppearanceData.ForeColor = Color.Green;
                    else
                        drawParams.AppearanceData.ForeColor = Color.Red;
                }
            }
            return false;
        }

        /// <summary>
        /// Called before each element is about to be drawn.
        /// </summary>
        /// <param name="drawParams">The <see cref="T:Infragistics.Win.UIElementDrawParams" /> used to provide rendering information.</param>
        /// <returns>
        /// Bit flags indicating which phases of the drawing operation to filter. The DrawElement method will be called only for those phases.
        /// </returns>
        DrawPhase IUIElementDrawFilter.GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            return DrawPhase.BeforeDrawBackColor | DrawPhase.BeforeDrawForeground;

        }
    }
}
