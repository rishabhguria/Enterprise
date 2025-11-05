using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.TradeManager.Extension;
using System;
using System.Drawing;

namespace Prana.TradingTicket.Classes
{
    internal class MultiBrokerGridDrawFilter : IUIElementDrawFilter
    {
        /// <summary>
        /// The column broker
        /// </summary>
        private const string COLUMN_BROKER = "Broker";
        /// <summary>
        /// The column connection status
        /// </summary>
        private const string COLUMN_CONNECTION_STATUS = "Connection Status";

        /// <summary>
        /// This function is responsible for drawing elements on the screen.
        /// <summary>
        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            if (drawPhase == DrawPhase.BeforeDrawForeground)
            {
                var cell = drawParams.Element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
               
                if (cell != null && cell.Column.Key == COLUMN_CONNECTION_STATUS)
                {
                    int index = -1;
                    int counterPartyId = int.MinValue;
                    var brokerCell = cell.Row.Cells[COLUMN_BROKER];

                    if (!String.IsNullOrEmpty(brokerCell.Text) && brokerCell.ValueListResolved.GetValue(brokerCell.Text, ref index) != null)
                    {
                        counterPartyId = Convert.ToInt32(brokerCell.ValueListResolved.GetValue(brokerCell.Text, ref index));
                    }

                    // Get the rectangle for the cell from the UIElement
                    Rectangle cellRect = cell.GetUIElement().Rect;

                    // Calculate the rectangle for the square
                    Rectangle rect = new Rectangle(cellRect.Left + cellRect.Width / 2 - 10, cellRect.Top + cellRect.Height / 2 - 8, 20, 16);

                    // Set color based on the connection status of CounterParty Id
                    Color color = TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(Convert.ToInt32(counterPartyId)) == BusinessObjects.PranaInternalConstants.ConnectionStatus.CONNECTED ? Color.FromArgb(104, 156, 46) : Color.FromArgb(140, 5, 5);
                    
                    drawParams.Graphics.FillRectangle(new SolidBrush(color), rect);
                   
                    return true;
                }
            } 
            return false;
        }

        /// <summary>
        /// This function is responsible for retrieving the phases that need to be filtered.
        /// <summary>
        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            return DrawPhase.BeforeDrawForeground;
        }
    }
}
